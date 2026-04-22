using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Settings.PayementPractice
{
	public class GetPayementPracticeForSettingsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.PayementPractice.PayementPracticeModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetPayementPracticeForSettingsHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.PayementPractice.PayementPracticeModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.PayementPractice.PayementPracticeModel>();
				var termsOfPayementEntities = Infrastructure.Data.Access.Tables.BSD.Mahnwesen_zahlungsmoralAccess.Get();

				if(termsOfPayementEntities != null && termsOfPayementEntities.Count > 0)
				{
					foreach(var termsOfPayementEntity in termsOfPayementEntities)
					{
						responseBody.Add(new Models.PayementPractice.PayementPracticeModel(termsOfPayementEntity));
					}
				}

				return ResponseModel<List<Models.PayementPractice.PayementPracticeModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.PayementPractice.PayementPracticeModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.PayementPractice.PayementPracticeModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.PayementPractice.PayementPracticeModel>>.SuccessResponse();
		}
	}
}
