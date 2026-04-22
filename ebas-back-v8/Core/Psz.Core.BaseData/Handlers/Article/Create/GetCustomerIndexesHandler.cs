using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Create
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetCustomerIndexesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Article.CustomerIndexesResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.CustomerIndexesRequestModel _data { get; set; }

		public GetCustomerIndexesHandler(Identity.Models.UserModel user, Models.Article.CustomerIndexesRequestModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<List<Models.Article.CustomerIndexesResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// - check for CustomerItemNumber
				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByCustomerItemNumber(this._data.CustomerNumber, this._data.CustomerPrefix, this._data.CustomerItemNumber)
					?? new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();

				// -
				return ResponseModel<List<Models.Article.CustomerIndexesResponseModel>>.SuccessResponse(articleEntities.Select(x => new Models.Article.CustomerIndexesResponseModel
				{
					Key = x.CustomerIndexSequence ?? 0,
					Value = x.CustomerIndex,
					IsArticleNumberSpecial = x.IsArticleNumberSpecial ?? false,
				}).Distinct().ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.CustomerIndexesResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.CustomerIndexesResponseModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.Get(this._data.CustomerId) == null)
			{
				return ResponseModel<List<Models.Article.CustomerIndexesResponseModel>>.FailureResponse("Customer not found in Nummerschlüssel");
			}

			if(Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByKundenNummer(this._data.CustomerNumber) == null)
			{
				return ResponseModel<List<Models.Article.CustomerIndexesResponseModel>>.FailureResponse("Customer not found in Adressen");
			}

			// -
			return ResponseModel<List<Models.Article.CustomerIndexesResponseModel>>.SuccessResponse();
		}
	}
}
