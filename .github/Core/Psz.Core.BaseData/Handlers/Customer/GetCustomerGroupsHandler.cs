using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Customer
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	public class GetCustomerGroupsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetCustomerGroupsHandler(Identity.Models.UserModel user)
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

				var responseBody = new List<KeyValuePair<int, string>>();
				var kundenEntities = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get();

				if(kundenEntities != null && kundenEntities.Count > 0)
				{
					foreach(var kundenEntity in kundenEntities)
					{
						responseBody.Add(new KeyValuePair<int, string>(kundenEntity.Nr, $"{kundenEntity.Kundengruppe.Trim()}"));
					}
					responseBody = responseBody.Distinct().ToList();
				}
				responseBody = responseBody.Distinct().ToList();

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(responseBody);
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
