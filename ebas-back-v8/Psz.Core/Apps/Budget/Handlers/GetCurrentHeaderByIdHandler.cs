using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.Apps.Budget.Handlers
{

	public class GetCurrentHeaderByIdHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.User.UserModel>>

	{
		//public Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		//public GetCurrentHeaderByIdHandler(/*Identity.Models.UserModel user,*/ int id)
		public GetCurrentHeaderByIdHandler(int id)
		{
			//this._user = user;
			this._data = id;

		}
		public ResponseModel<Models.User.UserModel> Handle()

		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new Models.User.UserModel();

				var userHead = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._data);
				responseBody.Id = userHead.Id;
				responseBody.Username = userHead.Username;
				responseBody.Name = userHead.Name;
				responseBody.Email = userHead.Email;
				responseBody.AccessProfileId = userHead.AccessProfileId;
				//responseBody.AccessProfileName = budgetLand.AccessProfileName;
				// responseBody.Depts = budgetLand.Depts;
				//responseBody.Lands = budgetLand.Lands;
				//responseBody.UsersAssign = budgetLand.UsersAssign;

				return ResponseModel<Models.User.UserModel>.SuccessResponse(responseBody);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.User.UserModel> Validate()
		{

			return ResponseModel<Models.User.UserModel>.SuccessResponse();


		}

	}
}
