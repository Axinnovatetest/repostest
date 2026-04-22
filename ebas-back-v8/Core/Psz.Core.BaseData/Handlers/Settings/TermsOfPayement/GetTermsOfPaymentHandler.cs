using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Settings.TermsOfPayement
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	public class GetTermsOfPaymentHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetTermsOfPaymentHandler(Identity.Models.UserModel user)
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
				var zahlungskonditionenEntities = Infrastructure.Data.Access.Tables.BSD.ZahlungskonditionenAccess.Get();
				var S = zahlungskonditionenEntities.OrderBy(t => t.Text11 != null)
							.ThenBy(t => t.ID).ToArray();
				if(zahlungskonditionenEntities != null && zahlungskonditionenEntities.Count > 0)
				{
					foreach(var zahlungskonditionenEntity in S)
					{
						responseBody.Add(new KeyValuePair<int, string>(zahlungskonditionenEntity.ID, $"{zahlungskonditionenEntity.ID} || {zahlungskonditionenEntity.Text11.Trim()}"));
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
