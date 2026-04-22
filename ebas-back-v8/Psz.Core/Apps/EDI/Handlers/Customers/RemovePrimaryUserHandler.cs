using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.Apps.EDI.Handlers
{
	public class RemovePrimaryUserHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public RemovePrimaryUserHandler(Identity.Models.UserModel user, int data)
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

				var customerDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(_data);
				var id = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.DeleteBy_IsPrimary_CustomerNumber(true, customerDb.Nummer ?? -1, botransaction.connection, botransaction.transaction);

				// - logs
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(
					new Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity
					{
						Id = 0,
						LastUpdateTime = DateTime.Now,
						LastUpdateUserFullName = _user.Name,
						LastUpdateUserId = _user.Id,	
						LastUpdateUsername = _user.Username,
						LogDescription = $"Primary user removed from customer {customerDb.Nummer} by {_user.Username}",
						LogObject = "CustomerUser",
						LogObjectId = _data
					}, botransaction.connection, botransaction.transaction);

				//-
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(id);
				}
				else
				{
					return ResponseModel<int>.FailureResponse("Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(_user == null || (!_user.Access.CustomerService.ModuleActivated && !_user.Access.Purchase.ModuleActivated))
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var customerDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(_data);
			if(customerDb == null)
			{
				return ResponseModel<int>.FailureResponse("Customer not found");
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
