using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	public class DeleteScannerRohmaterialHandle: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public DeleteScannerRohmaterialHandle(Identity.Models.UserModel user, int data)
		{
			this._data = data;
			this._user = user;
		}
		public ResponseModel<int> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{

				botransaction.beginTransaction();
				Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.DeleteScannerRohmaterial(_data, botransaction.connection, botransaction.transaction);
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(1);
				}
				else
				{
					return ResponseModel<int>.FailureResponse(key: "1", value: $"Transaction diden't commit");
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
