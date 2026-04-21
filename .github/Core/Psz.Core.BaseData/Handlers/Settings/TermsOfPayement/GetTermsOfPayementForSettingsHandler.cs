using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Settings.TermsOfPayement
{
	public class GetTermsOfPayementForSettingsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.TermsOfPayement.TermsOfPayementModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetTermsOfPayementForSettingsHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.TermsOfPayement.TermsOfPayementModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.TermsOfPayement.TermsOfPayementModel>();
				var zahlungskonditionenEntities = Infrastructure.Data.Access.Tables.BSD.ZahlungskonditionenAccess.Get();

				if(zahlungskonditionenEntities != null && zahlungskonditionenEntities.Count > 0)
				{
					foreach(var zahlungskonditionenEntity in zahlungskonditionenEntities)
					{
						responseBody.Add(new Models.TermsOfPayement.TermsOfPayementModel(zahlungskonditionenEntity));
					}
				}

				return ResponseModel<List<Models.TermsOfPayement.TermsOfPayementModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.TermsOfPayement.TermsOfPayementModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.TermsOfPayement.TermsOfPayementModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.TermsOfPayement.TermsOfPayementModel>>.SuccessResponse();
		}
	}
}
