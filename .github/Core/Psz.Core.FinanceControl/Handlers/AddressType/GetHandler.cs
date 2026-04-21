using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.AddressType
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.AddressType.GetModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.AddressType.GetModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var addressTypeEntities = Infrastructure.Data.Access.Tables.FNC.Adressen_typenAccess.Get();

				var responseBody = new List<Models.AddressType.GetModel>();

				foreach(var addressTypeEntity in addressTypeEntities)
				{
					responseBody.Add(new Models.AddressType.GetModel(addressTypeEntity));
				}

				return ResponseModel<List<Models.AddressType.GetModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.AddressType.GetModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<Models.AddressType.GetModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.AddressType.GetModel>>.SuccessResponse();
		}
	}
}
