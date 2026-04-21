using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Customer
{
	public class GetCustomersDropdown_2Handler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetCustomersDropdown_2Handler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<KeyValuePair<int, string>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var CustomerDropdownEntity = Infrastructure.Data.Access.Joins.Kunden_LieferantenAccess.GetCustomerDropdown();
				var response = new List<KeyValuePair<int, string>>();
				if(CustomerDropdownEntity != null && CustomerDropdownEntity.Count > 0)
				{
					foreach(var item in CustomerDropdownEntity)
					{
						response.Add(new KeyValuePair<int, string>(item.Nr, $"{item.Kundennummer}||{item.Name1}||{item.Name2}||{item.Ort}||{item.AdressType}||{item.Nr}"));
					}
				}
				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}
