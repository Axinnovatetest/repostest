using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Overview
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	public class GetEFByCustomerHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Article.ArticleMinimalModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.Overview.EFByCustomerRequestModel _data { get; set; }


		public GetEFByCustomerHandler(Identity.Models.UserModel user, Models.Article.Overview.EFByCustomerRequestModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<List<Models.Article.ArticleMinimalModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetEFByKreis(this._data.CustomerPrefix)
					 ?? new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();
				if(!string.IsNullOrWhiteSpace(this._data.CustomerIndex) && articleEntities.Count > 0)
				{
					articleEntities = articleEntities.Where(x => x.CustomerIndex == this._data.CustomerPrefix || x.Index_Kunde == this._data.CustomerIndex)
						?.ToList();
				}

				// -
				return ResponseModel<List<Models.Article.ArticleMinimalModel>>.SuccessResponse(
					articleEntities.Select(x => new Models.Article.ArticleMinimalModel()).ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.ArticleMinimalModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.ArticleMinimalModel>>.AccessDeniedResponse();
			}

			var customerEntities = Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.GetByKundeKreis(this._data.CustomerPrefix);
			if(customerEntities == null || customerEntities.Count <= 0)
			{
				return ResponseModel<List<Models.Article.ArticleMinimalModel>>.FailureResponse($"Customer [{this._data.CustomerPrefix}] not found in Nummerschlüssel.");
			}

			return ResponseModel<List<Models.Article.ArticleMinimalModel>>.SuccessResponse();
		}
	}

}
