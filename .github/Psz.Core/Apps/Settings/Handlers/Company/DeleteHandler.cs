using System;

namespace Psz.Core.Apps.Settings.Handlers.Company
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class DeleteHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public DeleteHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var result = -1;
				var companyEntiy = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(this._data);
				if(companyEntiy != null)
				{
					companyEntiy.IsActive = false;
					result = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Update(companyEntiy);
				}

				return ResponseModel<int>.SuccessResponse(result);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			//if (Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(this._data) == null)
			//    return ResponseModel<int>.FailureResponse($"Department not found");

			var companyDepartemnts = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByCompany(this._data);
			if(companyDepartemnts != null && companyDepartemnts.Count > 0)
				return ResponseModel<int>.FailureResponse($"Company contains departments");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
