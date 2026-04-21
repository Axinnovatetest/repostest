using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Customer
{
	public class GetCustomersDropdownHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Customer.CustomerDropdownModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetCustomersDropdownHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Customer.CustomerDropdownModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var CustomerDropdownEntity = Infrastructure.Data.Access.Joins.Kunden_LieferantenAccess.GetCustomerDropdown();
				var response = new List<Models.Customer.CustomerDropdownModel>();
				if(CustomerDropdownEntity != null && CustomerDropdownEntity.Count > 0)
				{
					foreach(var item in CustomerDropdownEntity)
					{
						response.Add(new Models.Customer.CustomerDropdownModel(item));
					}
				}
				return ResponseModel<List<Models.Customer.CustomerDropdownModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.Customer.CustomerDropdownModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Customer.CustomerDropdownModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Customer.CustomerDropdownModel>>.SuccessResponse();
		}
	}
}
