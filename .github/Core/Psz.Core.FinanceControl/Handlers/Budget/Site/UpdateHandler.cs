using System;

namespace Psz.Core.FinanceControl.Handlers.Budget.Site
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class UpdateHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Budget.Site.UpdateModel _data { get; set; }

		public UpdateHandler(Identity.Models.UserModel user, Models.Budget.Site.UpdateModel model)
		{
			this._user = user;
			this._data = model;
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
				var companyEntity = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(this._data.CompanyId);
				var companyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(this._data.CompanyId);
				var result = -1;

				if(companyExtensionEntity == null)
				{
					result = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.Insert(this._data.ToCompanyExtension());
				}
				else
				{
					companyExtensionEntity = this._data.ToCompanyExtension(companyExtensionEntity);
					this._data.Id = companyExtensionEntity.Id;
					Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.Update(companyExtensionEntity);
					result = companyExtensionEntity.Id;
				}

				companyEntity.DirectorId = this._data.DirectorId;
				companyEntity.DirectorName = this._data.DirectorName;
				companyEntity.DirectorEmail = this._data.DirectorEmail;
				Infrastructure.Data.Access.Tables.STG.CompanyAccess.Update(companyEntity);

				return ResponseModel<int>.SuccessResponse(result);
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

			if(Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(this._data.CompanyId) == null)
				return ResponseModel<int>.FailureResponse("Company not found");
			if((_data.SuperValidatorOneId == _data.DirectorId) || (_data.SuperValidatorTowId == _data.DirectorId))
				return ResponseModel<int>.FailureResponse("Super validator cannot be the same as Site Director");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}