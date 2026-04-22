using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Settings.Handlers.Currency
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetAllHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Settings.Models.Currency.GetModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetAllHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Settings.Models.Currency.GetModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var currencyEntities = Infrastructure.Data.Access.Tables.STG.WahrungenAccess.Get() ?? new List<Infrastructure.Data.Entities.Tables.STG.WahrungenEntity>();
				var response = new List<Settings.Models.Currency.GetModel>();
				foreach(var currencyEntity in currencyEntities)
				{
					response.Add(new Settings.Models.Currency.GetModel(currencyEntity));
				}

				return ResponseModel<List<Settings.Models.Currency.GetModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Settings.Models.Currency.GetModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Settings.Models.Currency.GetModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Settings.Models.Currency.GetModel>>.SuccessResponse();
		}
	}
}
