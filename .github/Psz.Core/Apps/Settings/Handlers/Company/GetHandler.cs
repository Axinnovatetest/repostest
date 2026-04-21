using System;

namespace Psz.Core.Apps.Settings.Handlers.Company
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetHandler: IHandle<Identity.Models.UserModel, ResponseModel<Settings.Models.Company.GetModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<Settings.Models.Company.GetModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				return ResponseModel<Settings.Models.Company.GetModel>.SuccessResponse(new Settings.Models.Company.GetModel(Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(this._data)));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Settings.Models.Company.GetModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Settings.Models.Company.GetModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(this._data) == null)
				return ResponseModel<Settings.Models.Company.GetModel>.FailureResponse($"Company not found");

			return ResponseModel<Settings.Models.Company.GetModel>.SuccessResponse();
		}
	}
}
