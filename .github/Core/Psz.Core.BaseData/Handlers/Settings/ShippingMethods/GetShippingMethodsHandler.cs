using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.ShippingMethods
{
	public class GetShippingMethodsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<string, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetShippingMethodsHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<KeyValuePair<string, string>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<KeyValuePair<string, string>>();
				var versandartenEntities = Infrastructure.Data.Access.Tables.BSD.Versandarten_AuswahlAccess.GetShipppingMethods();
				var S = versandartenEntities.OrderBy(t => t.Versandarten != null)
						.ThenBy(t => t.ID).ToArray();
				if(versandartenEntities != null && versandartenEntities.Count > 0)
				{
					foreach(var versandartenEntity in S)
					{
						responseBody.Add(new KeyValuePair<string, string>(versandartenEntity.Versandarten.Trim(), $"{versandartenEntity.ID} || {versandartenEntity.Versandarten.Trim()}"));
					}
					responseBody = responseBody.Distinct().ToList();
				}

				return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<KeyValuePair<string, string>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<string, string>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse();
		}
	}
}
