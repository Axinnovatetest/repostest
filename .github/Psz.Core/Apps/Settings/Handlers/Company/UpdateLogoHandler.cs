using System;

namespace Psz.Core.Apps.Settings.Handlers.Company
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class UpdateLogoHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Settings.Models.Company.EditLogoModel _data { get; set; }
		public UpdateLogoHandler(Identity.Models.UserModel user, Settings.Models.Company.EditLogoModel data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<byte[]> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				//Common.Helpers.ImageFileHelper.updateImage()
				var companyEntity = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(this._data.CompanyId);
				companyEntity.Logo = this._data.FileData;
				companyEntity.LogoExtension = this._data.FileExtension;

				companyEntity.LastUpdateTime = DateTime.Now;
				companyEntity.LastUpdateUserId = this._user.Id;

				if(Infrastructure.Data.Access.Tables.STG.CompanyAccess.UpdateLogo(companyEntity) > 0)
				{
					return ResponseModel<byte[]>.SuccessResponse(companyEntity.Logo);
				}

				return ResponseModel<byte[]>.SuccessResponse();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);

				Infrastructure.Services.Logging.Logger.Log(e, e.StackTrace);
				throw;
			}
		}
		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}


			if(Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(this._data.CompanyId) == null)
				return ResponseModel<byte[]>.FailureResponse($"Company not found");

			return ResponseModel<byte[]>.SuccessResponse();
		}
	}

}
