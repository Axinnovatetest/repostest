using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Handlers.CustomerService
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetLanguagesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.CustomerService.LanguageModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetLanguagesHandler(Identity.Models.UserModel user)
		{
			_user = user;
		}
		public ResponseModel<List<Models.CustomerService.LanguageModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.CustomerService.LanguageModel>();

				var langEntities = Infrastructure.Data.Access.Tables.STG.SprachenAccess.Get();
				foreach(var langEntity in langEntities)
				{
					responseBody.Add(new Models.CustomerService.LanguageModel(langEntity));
				}

				return ResponseModel<List<Models.CustomerService.LanguageModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.CustomerService.LanguageModel>> Validate()
		{
			return ResponseModel<List<Models.CustomerService.LanguageModel>>.SuccessResponse();
		}
	}
}
