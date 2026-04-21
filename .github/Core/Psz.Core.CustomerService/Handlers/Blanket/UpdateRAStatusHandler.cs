using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

using System.Linq;
using LogHelper = Psz.Core.CustomerService.Helpers.LogHelper;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class UpdateRAStatusHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Blanket.UpdateRAStatusModel _data { get; set; }
		public UpdateRAStatusHandler(Identity.Models.UserModel user, Models.Blanket.UpdateRAStatusModel model)
		{
			this._user = user;
			this._data = model;
		}
		public ResponseModel<int> Handle()
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //

				var RahmenEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(this._data.RahmenId, botransaction.connection, botransaction.transaction);
				var RahmenExtensionEntity = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(this._data.RahmenId, botransaction.connection, botransaction.transaction);
				var _oldstatusBlanket = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(this._data.RahmenId, botransaction.connection, botransaction.transaction);

				switch((Enums.BlanketEnums.ActionStatus)this._data.ActionStatusId)
				{
					case Enums.BlanketEnums.ActionStatus.SubmitValidate:
						RahmenExtensionEntity.StatusName = Enums.BlanketEnums.RAStatus.InProgress.GetDescription();
						RahmenExtensionEntity.StatusId = (int)Enums.BlanketEnums.RAStatus.InProgress;

						break;
					case Enums.BlanketEnums.ActionStatus.Valider:
						RahmenExtensionEntity.StatusName = Enums.BlanketEnums.RAStatus.Validated.GetDescription();
						RahmenExtensionEntity.StatusId = (int)Enums.BlanketEnums.RAStatus.Validated;
						ResetPriceHistory(_data.RahmenId, _user.Name);
						break;

					case Enums.BlanketEnums.ActionStatus.Rejeter:
						RahmenExtensionEntity.StatusName = Enums.BlanketEnums.RAStatus.Draft.GetDescription();
						RahmenExtensionEntity.StatusId = (int)Enums.BlanketEnums.RAStatus.Draft;
						DeletePriceHistory(_data.RahmenId);

						break;
					case Enums.BlanketEnums.ActionStatus.Fermer:
						RahmenExtensionEntity.StatusName = Enums.BlanketEnums.RAStatus.Closed.GetDescription();
						RahmenExtensionEntity.StatusId = (int)Enums.BlanketEnums.RAStatus.Closed;
						break;

					case Enums.BlanketEnums.ActionStatus.Annuler:
						RahmenExtensionEntity.StatusName = Enums.BlanketEnums.RAStatus.Draft.GetDescription();
						RahmenExtensionEntity.StatusId = (int)Enums.BlanketEnums.RAStatus.Draft;
						DeletePriceHistory(_data.RahmenId);
						break;
					default:
						break;
				}

				RahmenExtensionEntity.LastEditTime = DateTime.Now;
				RahmenExtensionEntity.LastEditUserId = this._user.Id;
				var response = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.UpdateWithTransaction(RahmenExtensionEntity, botransaction.connection, botransaction.transaction);
				//logging
				var _logs = GetLogs(_oldstatusBlanket, RahmenExtensionEntity, _user);
				if(_logs != null && _logs.Count > 0)
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(_logs, botransaction.connection, botransaction.transaction);
				Infrastructure.Services.Email.Helpers.StatusEmailNotification((Infrastructure.Services.Email.Enums.RahmenAction)this._data.ActionStatusId, _user.Name, _data.RahmenId, Infrastructure.Services.Email.Enums.RahmenRedirect.INS);


				#endregion // -- transaction-based logic -- //


				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(response);
				}
				else
				{
					return ResponseModel<int>.FailureResponse(key: "1", value: "Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(this._data == null)
			{
				return ResponseModel<int>.FailureResponse("Invalid input data");
			}

			if(this._data.ActionStatusId == (int)Enums.BlanketEnums.ActionStatus.Rejeter)
			{
				var raPositionIds = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data.RahmenId)
					?.Select(x => x.Nr)?.ToList();
				var raAbPos = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetbyRahmenPositions(raPositionIds);
				if(raAbPos != null && raAbPos.Count > 0)
				{
					var raAB = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(raAbPos.Select(x => x.AngebotNr ?? -1).ToList());
					return ResponseModel<int>.FailureResponse($"Cannot cancel Rahmen with orders [{string.Join(", ", raAB.Take(5).Select(x => x.Angebot_Nr ?? 0))}]");
				}
			}
			var raPositions = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data.RahmenId);
			var raPositionsExtensions = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByRahmenPositionNr(raPositions?.Select(p => p.Nr).ToList() ?? new List<int> { });
			if(this._data.ActionStatusId == (int)Enums.BlanketEnums.ActionStatus.Annuler)
			{
				var raPositionIds = raPositions?.Select(x => x.Nr)?.ToList();
				var raAbPos = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetbyRahmenPositions(raPositionIds)?.Where(x => x.erledigt_pos != true)?.ToList();
				if(raAbPos != null && raAbPos.Count > 0)
				{
					var raAB = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(raAbPos.Select(x => x.AngebotNr ?? -1).ToList());
					return ResponseModel<int>.FailureResponse($"Cannot cancel Rahmen with orders [{string.Join(", ", raAB.Take(5).Select(x => x.Angebot_Nr ?? 0))}]");
				}
			}
			if(raPositionsExtensions != null && raPositionsExtensions.Count > 0)
			{
				var horizonErrors = new List<string>();
				foreach(var item in raPositionsExtensions)
				{
					DateTime _newDate, _oldDate;
					_newDate = _oldDate = item.GultigBis ?? new DateTime(1900, 1, 1);
					var horizonCheck = Helpers.HorizonsHelper.userHasRAPosHorizonRight(_newDate, _oldDate, _user, out List<string> messages);
					if(!horizonCheck)
						horizonErrors.AddRange(messages);
				}
				if(horizonErrors != null && horizonErrors.Count > 0)
					return ResponseModel<int>.FailureResponse(horizonErrors);
			}
			return ResponseModel<int>.SuccessResponse();
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity> GetLogs(Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity _old,
		Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity _new, Core.Identity.Models.UserModel user)
		{
			List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity> _logs = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();
			var rahmenItem = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_old.AngeboteNr);
			LogHelper _log = new LogHelper(rahmenItem.Nr, rahmenItem.Angebot_Nr ?? -1, int.TryParse(rahmenItem.Projekt_Nr.ToString(), out var v) ? v : 0, rahmenItem.Typ, LogHelper.LogType.MODIFICATIONSTATUSBLANKET, "CTS", user);

			if(_old.StatusName != _new.StatusName)
			{
				_logs.Add(_log.LogCTS("Status", _old.StatusName.ToString(), _new.StatusName.ToString(), rahmenItem.Angebot_Nr ?? 0));
			}
			return _logs;
		}
		public static void DeletePriceHistory(int rahmenNr)
		{
			var rahmenPositions = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(rahmenNr);
			if(rahmenPositions != null && rahmenPositions.Count > 0)
			{
				var posIds = rahmenPositions.Select(x => x.Nr).ToList();
				Infrastructure.Data.Access.Tables.CTS.RahmenPriceHistoryAccess.DeleteByPositions(posIds);
			}
		}
		public static void ResetPriceHistory(int rahmenNr, string user)
		{
			var rahmenPositions = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(rahmenNr);
			if(rahmenPositions != null && rahmenPositions.Count > 0)
			{
				foreach(var item in rahmenPositions)
				{
					var _history = Infrastructure.Data.Access.Tables.CTS.RahmenPriceHistoryAccess.GetByPosition(item.Nr);
					if(_history == null)
					{
						var extension = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngeboteneArtikelNr(item.Nr);
						Infrastructure.Data.Access.Tables.CTS.RahmenPriceHistoryAccess.Insert(new Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity
						{
							DateUpdate = DateTime.Now,
							PositionNr = item.Nr,
							Price = extension.Preis,
							PriceDefault = extension.PreisDefault,
							RahmenNr = rahmenNr,
							UserName = user,
							ValidFrom = extension.GultigAb,
							WarungSymbol = extension.WahrungSymbole,
							BasePrice = extension.BasePrice,
						});
					}
				}
			}
		}
	}
}
