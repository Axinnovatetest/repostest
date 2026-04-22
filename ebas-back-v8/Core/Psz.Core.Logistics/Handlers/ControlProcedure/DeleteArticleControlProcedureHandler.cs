using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.Logistics.Handlers.ControlProcedure
{
	public class DeleteArticleControlProcedureHandler:IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{

		private int _ID;
		private Core.Identity.Models.UserModel _user;
		public DeleteArticleControlProcedureHandler(Core.Identity.Models.UserModel user, int ID)
		{
			_user = user;
			_ID = ID;
		}

		public ResponseModel<int> Handle()
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();

			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success || _ID < 0)
				{
					return validationResponse;
				}

				#region Deleting Script
				var data = Infrastructure.Data.Access.Tables.Logistics.PlantBookingArticleControlProcedureAccess.Delete(_ID);
				#endregion
				#region Adding Delete Log
				//var logs = PlantBookingLogHelper.GenerateLogForDelete(_user, _ID);
				botransaction.beginTransaction();
				//var InsertLogsResult = Infrastructure.Data.Access.Tables.Logistics.PlantBookingsLogsAccess.InsertWithTransaction(logs, botransaction.connection, botransaction.transaction);
				#endregion
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(data);
				}
				else
				{
					return ResponseModel<int>.FailureResponse(key: "1", value: "Transaction error");
				}
			} catch(Exception e)
			{
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

