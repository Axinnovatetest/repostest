using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Settings.Frames
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	public class GetCustomerFramesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetCustomerFramesHandler(Identity.Models.UserModel user)
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
				var kundenEntities = Infrastructure.Data.Access.Tables.BSD.Fibu_kunden_rahmenAccess.Get();
				var S = kundenEntities.OrderBy(t => t.Rahmen != null)
.ThenByDescending(t => t.Rahmen).ToArray();
				if(kundenEntities != null && kundenEntities.Count > 0)
				{
					foreach(var kundenEntity in S)
					{
						responseBody.Add(new KeyValuePair<int, string>(kundenEntity.ID, $"{kundenEntity.ID} || {kundenEntity.Rahmen.Trim()}"));
					}
					responseBody = responseBody.Distinct().ToList();
				}

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
