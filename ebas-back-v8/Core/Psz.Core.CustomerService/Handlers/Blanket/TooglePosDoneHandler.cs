using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class TooglePosDoneHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{

		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public TooglePosDoneHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<int> Handle()
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //

				var rahmenPositionEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetWithTransaction(_data, botransaction.connection, botransaction.transaction);
				var rahmenEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(rahmenPositionEntity.AngebotNr ?? -1, botransaction.connection, botransaction.transaction);
				rahmenPositionEntity.erledigt_pos = !rahmenPositionEntity.erledigt_pos;
				var response = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateWithTransaction(rahmenPositionEntity, botransaction.connection, botransaction.transaction);

				//logging
				var _log = new Psz.Core.CustomerService.Helpers.LogHelper(rahmenEntity.Nr, rahmenEntity.Angebot_Nr ?? -1, rahmenEntity.Angebot_Nr ?? -1, "Rahmenauftrag", Psz.Core.CustomerService.Helpers.LogHelper.LogType.MODIFICATIONPOS, "CTS", _user);
				Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(
						_log.LogCTS($"Erledigt", $"{!rahmenPositionEntity.erledigt_pos}", $"{rahmenPositionEntity.erledigt_pos}", rahmenPositionEntity.Position ?? -1),
						botransaction.connection, botransaction.transaction);
				#endregion // -- transaction-based logic -- //


				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(response);
				}
				else
				{
					return ResponseModel<int>.FailureResponse("Transaction error");
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

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
