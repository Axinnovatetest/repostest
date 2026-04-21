using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Handlers
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public partial class Order
	{
		public class GetShippingMethodsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
		{
			private Identity.Models.UserModel _user { get; set; }
			public GetShippingMethodsHandler(Identity.Models.UserModel user)
			{
				_user = user;
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
					List<KeyValuePair<int, string>> responseBody = null;
					var shippingMethods = Infrastructure.Data.Access.Tables.BSD.Versandarten_AuswahlAccess.Get();
					if(shippingMethods != null && shippingMethods.Count > 0)
					{
						responseBody = new List<KeyValuePair<int, string>>();
						foreach(var item in shippingMethods)
						{
							responseBody.Add(new KeyValuePair<int, string>(item.ID, item.Versandarten));
						}
					}
					responseBody.Insert(0, new KeyValuePair<int, string>(-1, null));
					return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(responseBody);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
			public ResponseModel<List<KeyValuePair<int, string>>> Validate()
			{
				if(this._user == null)
				{
					return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
				}

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
			}
		}
	}
}
