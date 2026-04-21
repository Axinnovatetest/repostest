using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Create
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public partial class GetCustomersHandler: IHandle<string, ResponseModel<List<Models.Article.CustomerResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetCustomersHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Article.CustomerResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				var results = Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.Get();
				var customerAdressen = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetWhereKundennummerIsNotNull()
					?.Select(x => x.Kundennummer ?? -1)?.ToList();
				if(customerAdressen != null && customerAdressen.Count > 0)
				{
					results = results.Where(x => customerAdressen.Contains(x.Kundennummer ?? -2))?.ToList();
				}

				// -
				return ResponseModel<List<Models.Article.CustomerResponseModel>>.SuccessResponse(
					results?.Select(x => new Models.Article.CustomerResponseModel(x))?.Distinct()?.ToList());

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.CustomerResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.CustomerResponseModel>>.AccessDeniedResponse();
			}

			// -
			return ResponseModel<List<Models.Article.CustomerResponseModel>>.SuccessResponse();
		}
	}
}
