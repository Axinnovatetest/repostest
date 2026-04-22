using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Helpers;
using Psz.Core.CustomerService.Models.Blanket;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class AddABPositionsFromBlanketHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private AddABFromRahmenModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public AddABPositionsFromBlanketHandler(Identity.Models.UserModel user, AddABFromRahmenModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var abEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data.AbId);
				var rahmenEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data.RahmenId);
				var rahmenExtensionEntity = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(rahmenEntity.Nr);

				botransaction.beginTransaction();

				//Angebot Artikel insert
				var ItemsToInsert = _data.Positions.Where(i => i.ABQuantity.HasValue && i.ABQuantity.Value > 0).ToList();
				var entitiesToInsert = new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
				var RahmenItemsToUpdate = new List<KeyValuePair<int, decimal>>();
				if(ItemsToInsert != null && ItemsToInsert.Count > 0)
				{
					foreach(var item in ItemsToInsert)
					{
						var extension = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngeboteneArtikelNr(item.AngeboteneArtikelNr);
						var angebotPosition = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(extension?.AngeboteArtikelNr ?? -1);
						var postext = $"Aus Ihrer Rahmenbestellung: {rahmenEntity.Bezug}, PSZ Rahmenauftrag Nr.: {rahmenEntity.Angebot_Nr}, Position: {angebotPosition.Position}";
						if(item.ABQuantity > angebotPosition.Anzahl)
						{
							var pos1 = Helpers.BlanketHelper.GetCalculatedPositon(_data.AbId, item.ArticleId, angebotPosition.Anzahl ?? 0, true, item.ABWunstermin ?? System.Data.SqlTypes.SqlDateTime.MinValue.Value, angebotPosition.Nr, postext);
							entitiesToInsert.Add(pos1);
							var pos2 = Helpers.BlanketHelper.GetCalculatedPositon(_data.AbId, item.ArticleId, (item.ABQuantity.Value - angebotPosition.Anzahl.Value), false, (DateTime)item.ABWunstermin, angebotPosition.Nr, postext);
							entitiesToInsert.Add(pos2);
						}
						else
						{
							var pos3 = Helpers.BlanketHelper.GetCalculatedPositon(_data.AbId, item.ArticleId, item.ABQuantity.Value, true, item.ABWunstermin ?? System.Data.SqlTypes.SqlDateTime.MinValue.Value, angebotPosition.Nr, postext);
							entitiesToInsert.Add(pos3);
						}
					}
					// - 2022-10-31
					var errorsQty = new List<string>();
					var raPositions = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data.RahmenId, botransaction.connection, botransaction.transaction)
						?? new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
					foreach(var raPos in raPositions)
					{
						var abPos = this._data.Positions?.Where(x => x.AngeboteneArtikelNr == raPos.Nr)?.ToList();
						if(abPos != null && abPos.Count > 0)
						{
							RahmenItemsToUpdate.Add(new KeyValuePair<int, decimal>(raPos.Nr, abPos.Sum(x => x.ABQuantity ?? 0)));
						}
					}
				}

				var abPositions = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data.AbId, botransaction.connection, botransaction.transaction)
					?? new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
				var i = Math.Max(abPositions.Max(x => x.Position ?? 0) + 10, 10);
				foreach(var a in entitiesToInsert)
				{
					a.Position = i;
					i += 10;
				}
				Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.InsertWithTransaction(entitiesToInsert, botransaction.connection, botransaction.transaction);

				// - 2022-10-31
				AddABFromRahmenHandler.UpdateRahmenPostions(RahmenItemsToUpdate, _data.RahmenId, botransaction);
				var RahmenAfterInsertion = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(_data.AbId, botransaction.connection, botransaction.transaction);
				RahmenAfterInsertion.Projekt_Nr = rahmenEntity.Projekt_Nr;
				Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateWithTransaction(RahmenAfterInsertion, botransaction.connection, botransaction.transaction);

				if(botransaction.commit())
				{
					//Logging
					var _log = new LogHelper(_data.AbId, (int)abEntity.Angebot_Nr,
						int.TryParse(abEntity.Projekt_Nr, out var val) ? val : 0, abEntity.Typ, LogHelper.LogType.CREATIONOBJECT, "CTS", _user)
						.LogCTS(null, null, null, 0);
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_log);
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(new Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity
					{
						AngebotNr = rahmenEntity.Angebot_Nr,
						Nr = rahmenEntity.Nr,
						DateTime = DateTime.Now,
						LogObject = rahmenEntity.Typ,
						LogText = $"AB [{abEntity.Angebot_Nr}] Created from Position(s) [{string.Join(",", ItemsToInsert.Select(x => x.PositionId).ToList())}]",
						LogType = "CREATIONOBJECT",
						Origin = "CTS",
						ProjektNr = int.TryParse(rahmenEntity.Projekt_Nr, out var v) ? v : 0,
						UserId = _user.Id,
						Username = _user.Name
					});
					return ResponseModel<int>.SuccessResponse(_data.AbId);
				}
				else
					return ResponseModel<int>.FailureResponse("Transaction did not commit.");
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var abEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data.AbId);
			if(abEntity == null)
				return ResponseModel<int>.FailureResponse("AB not found.");
			var rahmenEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data.RahmenId);
			if(rahmenEntity == null)
				return ResponseModel<int>.FailureResponse("Rahmen not found.");
			if(_data.Positions == null || _data.Positions.Count <= 0)
				return ResponseModel<int>.FailureResponse("No positions to add.");
			if(_data.Positions?.Where(x => x.ABQuantity.HasValue && x.ABQuantity.Value > 0)?.Count() <= 0)
				return ResponseModel<int>.FailureResponse("No positions with valid AB Quantity.");
			var errors = new List<string>();
			var rahmenExtensionEntity = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(rahmenEntity.Nr);
			for(int i = 0; i < _data.Positions.Count; i++)
			{
				if(_data.Positions[i].ABQuantity.HasValue && _data.Positions[i].ABQuantity.Value > 0)
				{
					// validate date
					if(!_data.Positions[i].ABWunstermin.HasValue || _data.Positions[i].ABWunstermin.Value <= System.Data.SqlTypes.SqlDateTime.MinValue.Value)
					{
						errors.Add($"Position {i + 1}: invalid Wunschtermin {_data.Positions[i].ABWunstermin}");
					}
					var extension = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngeboteneArtikelNr(_data.Positions[i].AngeboteneArtikelNr);
					if(rahmenExtensionEntity.BlanketTypeId == (int)Enums.BlanketEnums.Types.purchase)
					{
						extension.ExtensionDate = extension.GultigBis;// - 2025-08-14 Hejdukova remove ExtDate .Value.AddDays(90);
					}

					if(DateTime.Now > extension.ExtensionDate)
					{
						errors.Add($"Position {i + 10}: Expired");
					}
					if(_data.Positions[i].ABWunstermin > extension.ExtensionDate)
					{
						errors.Add($"Position {i + 10}: invalid Wunschtermin should be before RA ExpiryDate");
					}
				}
			}

			// - 2022-10-31
			var errorsQty = new List<string>();
			var raPositions = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data.RahmenId)
				?? new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
			foreach(var raPos in raPositions)
			{
				var abPos = this._data.Positions?.Where(x => x.AngeboteneArtikelNr == raPos.Nr)?.ToList();
				if(abPos != null && abPos.Count > 0)
				{
					if(abPos.Sum(x => x.ABQuantity ?? 0) > raPos.Anzahl)
					{
						errorsQty.Add($"Position [{raPos.Position}]: not enough quantity");
					}
				}
			}
			if(errorsQty.Count > 0)
				return ResponseModel<int>.FailureResponse(errorsQty);

			var rahmenExtension = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(_data.RahmenId);
			if(rahmenExtension.StatusId != (int)Enums.BlanketEnums.RAStatus.Validated)
				errors.Add($"Rahmen is not validated, Action is blocked .");
			if(errors.Count > 0)
				return ResponseModel<int>.FailureResponse(errors);

			return ResponseModel<int>.SuccessResponse();
		}
	}
}