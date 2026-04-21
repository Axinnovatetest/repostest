using System;

namespace Psz.Core.BaseData.Handlers.Customer
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetNewCustomerNumberHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }


		public GetNewCustomerNumberHandler(Identity.Models.UserModel user)
		{
			this._user = user;
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

				// -
				return ResponseModel<int>.SuccessResponse(
					Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetNewKundennummer(
						Core.BaseData.Module.AppSettings.SpecialCustomerNumberStart));
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

			if(Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetNewKundennummer(Core.BaseData.Module.AppSettings.SpecialCustomerNumberStart) <= 0)
				return ResponseModel<int>.FailureResponse("Cannot define Customer Number");

			return ResponseModel<int>.SuccessResponse();
		}
	}

}
