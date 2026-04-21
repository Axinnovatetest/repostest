using System;

namespace Psz.Core.Apps.Settings.Handlers.Department
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetHandler: IHandle<Identity.Models.UserModel, ResponseModel<Settings.Models.Department.GetModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<Settings.Models.Department.GetModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				return ResponseModel<Settings.Models.Department.GetModel>.SuccessResponse(new Settings.Models.Department.GetModel(Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(this._data)));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Settings.Models.Department.GetModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Settings.Models.Department.GetModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(this._data) == null)
				return ResponseModel<Settings.Models.Department.GetModel>.FailureResponse($"Department not found");

			return ResponseModel<Settings.Models.Department.GetModel>.SuccessResponse();
		}
	}
}
