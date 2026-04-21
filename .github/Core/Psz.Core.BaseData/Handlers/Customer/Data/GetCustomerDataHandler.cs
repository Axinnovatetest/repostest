using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
namespace Psz.Core.BaseData.Handlers.Customer.Data
{
	public class GetCustomerDataHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Customer.CustomerDataModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetCustomerDataHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}
		public ResponseModel<Models.Customer.CustomerDataModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(this._data);
				var adressEntity = int.TryParse(kundenEntity.Nummer.ToString(), out int knudenNummer)
					? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(knudenNummer)
					: null;
				return ResponseModel<Models.Customer.CustomerDataModel>.SuccessResponse(new Models.Customer.CustomerDataModel(kundenEntity, adressEntity));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<Models.Customer.CustomerDataModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Customer.CustomerDataModel>.AccessDeniedResponse();
			}

			var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(this._data);
			if(kundenEntity == null)
			{
				return new ResponseModel<Models.Customer.CustomerDataModel>()
				{
					Errors = new List<ResponseModel<Models.Customer.CustomerDataModel>.ResponseError>() {
						new ResponseModel<Models.Customer.CustomerDataModel>.ResponseError {Key = "1", Value = "Customer not found"}
					}
				};
			}


			return ResponseModel<Models.Customer.CustomerDataModel>.SuccessResponse();
		}
	}
}
