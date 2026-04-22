using System;

namespace Psz.Core.BaseData.Handlers.Supplier
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetNewSupplierNumberHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		public GetNewSupplierNumberHandler(UserModel user)
		{
			_user = user;
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

				return ResponseModel<int>.SuccessResponse(
					Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetNewLieferantennummer(
						Core.BaseData.Module.AppSettings.SpecialSupplierNumberStart));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
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

			if(Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetNewLieferantennummer(Core.BaseData.Module.AppSettings.SpecialSupplierNumberStart) <= 0)
				return ResponseModel<int>.FailureResponse("Cannot define Supplier Number");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
