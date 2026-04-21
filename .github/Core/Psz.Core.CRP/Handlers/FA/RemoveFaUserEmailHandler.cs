using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA
{
	public class RemoveFaUserEmailHandler: IHandle<UserModel, ResponseModel<int>>
	{
		protected UserModel _user;
		protected int _data;
		public RemoveFaUserEmailHandler(UserModel user, int data)
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

				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var userEntity = Infrastructure.Data.Access.Tables.CRP.CRP_FA_EmailUsersAccess.GetByUserWithTransaction(this._data, botransaction.connection, botransaction.transaction);

				int deletionResult = Infrastructure.Data.Access.Tables.CRP.CRP_FA_EmailUsersAccess.DeleteWithTransaction(userEntity.Id, botransaction.connection, botransaction.transaction);

				//Logs after deleting fa user

				if(deletionResult > 0)
				{
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity
					{
						DateTime = DateTime.Now,
						LogObject = "Fa-Notification",
						UserId = _user.Id,
						Username = _user.Username,
						LogText = $"FA E-mail User  : [{string.Join(",", userEntity.UserName)}] has been deleted."
					}, botransaction.connection, botransaction.transaction);
				}

				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse();
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
			if(this._user == null)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var faUser = Infrastructure.Data.Access.Tables.CRP.CRP_FA_EmailUsersAccess.GetByUser(this._data);
			if(faUser == null)
			{
				return ResponseModel<int>.FailureResponse("Fa User not found");
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
