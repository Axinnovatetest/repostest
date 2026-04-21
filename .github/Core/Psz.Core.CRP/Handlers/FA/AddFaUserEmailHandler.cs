using Infrastructure.Data.Access.Tables.CRP;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA
{
	public class AddFaUserEmailHandler: IHandle<UserModel, ResponseModel<string>>
	{
		protected AddFaUserRequestModel _data;
		protected UserModel _user;
		public AddFaUserEmailHandler(UserModel user, AddFaUserRequestModel data)
		{
			this._data = data;
			this._user = user;
		}
		public ResponseModel<string> Handle()
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
				ResponseModel<string> responseModel = new ResponseModel<string>();

				List<Infrastructure.Data.Entities.Tables.CRP.CRP_FA_EmailUsersEntity> entitiesToAdd = new List<Infrastructure.Data.Entities.Tables.CRP.CRP_FA_EmailUsersEntity>();
				entitiesToAdd = this._data.Items.Select(x => new Infrastructure.Data.Entities.Tables.CRP.CRP_FA_EmailUsersEntity
				{
					UserId = x.UserId,
					UserName = x.Username,
					CreateTime = DateTime.Now,
					CreateUserId = this._user.Id
				}).ToList();

				int result = CRP_FA_EmailUsersAccess.InsertWithTransaction(entitiesToAdd, botransaction.connection, botransaction.transaction);

				//Logs after adding fa user

				Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity
				{
					DateTime = DateTime.Now,
					LogObject = "Fa-Notification",
					UserId = _user.Id,
					Username = _user.Username,
					LogText = $"FA E-mail Users  : [{string.Join(",", entitiesToAdd.Select(x => x.UserName).ToList())}] has been added."
				}, botransaction.connection, botransaction.transaction);


				if(botransaction.commit())
				{
					return ResponseModel<string>.SuccessResponse("Fa User email added successfully");
				}
				else
				{
					return ResponseModel<string>.FailureResponse(key: "1", value: "Transaction error");
				}

			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<string> Validate()
		{
			List<int> usersIds = this._data.Items.Select(x => x.UserId).ToList();
			List<string> faUsernames = CRP_FA_EmailUsersAccess.GetByUsersIds(usersIds).Select(x => x.UserName).ToList();
			if(faUsernames.Count != 0)
			{
				return ResponseModel<string>.FailureResponse($"Users : {string.Join(",", faUsernames)} , already exists");
			}
			if(this._user == null)
			{
				return ResponseModel<string>.AccessDeniedResponse();
			}

			return ResponseModel<string>.SuccessResponse();
		}
	}
}
