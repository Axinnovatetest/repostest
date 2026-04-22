using Infrastructure.Services.Utils;
using Psz.Core.MaterialManagement.Helpers;
using Psz.Core.MaterialManagement.Orders.Models.OrderValidation;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;
using LogHelper = Psz.Core.MaterialManagement.Orders.Helpers.RahmenLogHelper;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Rahmen
{
	public class UpdateRAStatusHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Rahmen.UpdateRAStatusModel _data { get; set; }
		public UpdateRAStatusHandler(Identity.Models.UserModel user, Models.Rahmen.UpdateRAStatusModel model)
		{
			this._user = user;
			this._data = model;
		}
		public ResponseModel<int> Handle()
		{
			TransactionsManager botransaction = new Infrastructure.Services.Utils.TransactionsManager(Infrastructure.Services.Utils.TransactionsManager.Database.Default);
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// changing to transactions


				botransaction.beginTransaction();

				var RahmenEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data.RahmenId);
				var RahmenExtensionEntity = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(this._data.RahmenId);
				var _oldstatusBlanket = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(this._data.RahmenId);

				switch((Enums.BlanketEnums.ActionStatus)this._data.ActionStatusId)
				{
					case Enums.BlanketEnums.ActionStatus.SubmitValidate:
						RahmenExtensionEntity.StatusName = Enums.BlanketEnums.RAStatus.InProgress.GetDescription();
						RahmenExtensionEntity.StatusId = (int)Enums.BlanketEnums.RAStatus.InProgress;

						break;
					case Enums.BlanketEnums.ActionStatus.Valider:
						RahmenExtensionEntity.StatusName = Enums.BlanketEnums.RAStatus.Validated.GetDescription();
						RahmenExtensionEntity.StatusId = (int)Enums.BlanketEnums.RAStatus.Validated;
						ResetPriceHistory(_data.RahmenId, _user.Name, botransaction);
						break;

					case Enums.BlanketEnums.ActionStatus.Rejeter:
						RahmenExtensionEntity.StatusName = Enums.BlanketEnums.RAStatus.Draft.GetDescription();
						RahmenExtensionEntity.StatusId = (int)Enums.BlanketEnums.RAStatus.Draft;
						DeletePriceHistory(_data.RahmenId, botransaction);

						break;
					case Enums.BlanketEnums.ActionStatus.Fermer:
						RahmenExtensionEntity.StatusName = Enums.BlanketEnums.RAStatus.Closed.GetDescription();
						RahmenExtensionEntity.StatusId = (int)Enums.BlanketEnums.RAStatus.Closed;
						break;

					case Enums.BlanketEnums.ActionStatus.Annuler:
						RahmenExtensionEntity.StatusName = Enums.BlanketEnums.RAStatus.Draft.GetDescription();
						RahmenExtensionEntity.StatusId = (int)Enums.BlanketEnums.RAStatus.Draft;
						DeletePriceHistory(_data.RahmenId, botransaction);
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


				if(botransaction.commit())
				{
					// send Email
					Infrastructure.Services.Email.Helpers.StatusEmailNotification((Infrastructure.Services.Email.Enums.RahmenAction)this._data.ActionStatusId, _user.Name, _data.RahmenId, Infrastructure.Services.Email.Enums.RahmenRedirect.MTM);
					return ResponseModel<int>.SuccessResponse(response);
				}
				else
					return ResponseModel<int>.FailureResponse("Transaction didn't commit.");

			} catch(Exception e)
			{
				// botransaction.rollback() == false => Error with Sending Email... do we need to specifically capture this.? 
				// botransaction.connection.State == System.Data.ConnectionState.Closed ==> checks if transaction commited and connection closed successfully.

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

			if(this._data.ActionStatusId == (int)Enums.BlanketEnums.ActionStatus.Annuler)
			{
				var raPositionIds = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data.RahmenId)
					?.Select(x => x.Nr)?.ToList();
				var raAbPos = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetbyRahmenPositions(raPositionIds)?.Where(x => x.erledigt_pos != true)?.ToList();
				if(raAbPos != null && raAbPos.Count > 0)
				{
					var raAB = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(raAbPos.Select(x => x.AngebotNr ?? -1).ToList());
					return ResponseModel<int>.FailureResponse($"Cannot cancel Rahmen with orders [{string.Join(", ", raAB.Take(5).Select(x => x.Angebot_Nr ?? 0))}]");
				}
			}
			return ResponseModel<int>.SuccessResponse();
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity> GetLogs(Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity _old,
		Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity _new, Core.Identity.Models.UserModel user)
		{
			List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity> _logs = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();
			var RahmenExtensionEntity = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(_old.AngeboteNr);
			var rahmenItem = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_old.AngeboteNr);
			LogHelper _log = new LogHelper(rahmenItem.Nr, rahmenItem.Angebot_Nr ?? -1, int.TryParse(rahmenItem.Projekt_Nr.ToString(), out var v) ? v : 0, rahmenItem.Typ, LogHelper.LogType.MODIFICATIONSTATUSBLANKET, "CTS", user);

			if(_old.StatusName != _new.StatusName)
			{
				_logs.Add(_log.LogCTS("StatusName", _old.StatusName.ToString(), _new.StatusName.ToString(), 0));
			}
			return _logs;
		}

		public static void DeletePriceHistory(int rahmenNr, TransactionsManager botransaction)
		{
			var rahmenPositions = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(rahmenNr);
			if(rahmenPositions != null && rahmenPositions.Count > 0)
			{
				var posIds = rahmenPositions.Select(x => x.Nr).ToList();
				Infrastructure.Data.Access.Tables.CTS.RahmenPriceHistoryAccess.DeleteByPositionsWithTransaction(posIds, botransaction.connection, botransaction.transaction);
			}
		}

		public static void ResetPriceHistory(int rahmenNr, string user, TransactionsManager botransaction)
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
						Infrastructure.Data.Access.Tables.CTS.RahmenPriceHistoryAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity
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
						}, botransaction.connection, botransaction.transaction);
					}
				}
			}
		}
	}
}
