using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.Logistics.Handlers.Verpackung
{
	public class GetMitarbeitetLogisticDEHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<string, string>>>>
	{

		private Identity.Models.UserModel _user { get; set; }

		public GetMitarbeitetLogisticDEHandler(Identity.Models.UserModel user)
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

				var mitarbeiterEntity = Infrastructure.Data.Access.Joins.Logistics.PackingAccess.GetMitarbeiterLogistic();
				var response = new List<KeyValuePair<string, string>>();
				if(mitarbeiterEntity != null && mitarbeiterEntity.Count > 0)
				{
					foreach(var item in mitarbeiterEntity)
					{
						response.Add(new KeyValuePair<string, string>(item.username, item.name));
					}
				}
				return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);

				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
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
