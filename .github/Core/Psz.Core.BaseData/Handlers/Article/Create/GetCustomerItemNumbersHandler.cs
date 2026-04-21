using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Create
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetCustomerItemNumbersHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Article.CustomerItemNumbersResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.CustomerItemNumbersRequestModel _data { get; set; }

		public GetCustomerItemNumbersHandler(Identity.Models.UserModel user, Models.Article.CustomerItemNumbersRequestModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<List<Models.Article.CustomerItemNumbersResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// - check for CustomerItemNumber
				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByCustomerNumberAndPrefix(this._data.CustomerNumber, this._data.CustomerPrefix)
					?? new List<Tuple<int, string, string>>();

				// -
				return ResponseModel<List<Models.Article.CustomerItemNumbersResponseModel>>.SuccessResponse(articleEntities.Select(x => new Models.Article.CustomerItemNumbersResponseModel(x)).Distinct().ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.CustomerItemNumbersResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.CustomerItemNumbersResponseModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.Get(this._data.CustomerId) == null)
			{
				return ResponseModel<List<Models.Article.CustomerItemNumbersResponseModel>>.FailureResponse("Customer not found in Nummerschlüssel");
			}

			if(Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByKundenNummer(this._data.CustomerNumber) == null)
			{
				return ResponseModel<List<Models.Article.CustomerItemNumbersResponseModel>>.FailureResponse("Customer not found in Adressen");
			}

			// -
			return ResponseModel<List<Models.Article.CustomerItemNumbersResponseModel>>.SuccessResponse();
		}
	}
}
