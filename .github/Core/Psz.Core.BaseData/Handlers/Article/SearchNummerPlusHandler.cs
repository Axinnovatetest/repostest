using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article
{

	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public partial class SearchNummerPlusHandler: IHandle<string, ResponseModel<List<Models.Article.SearchNummerResponseModel>>>
	{
		private Models.Article.SearchNummerRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public SearchNummerPlusHandler(Identity.Models.UserModel user, Models.Article.SearchNummerRequestModel data)
		{
			this._data = data;
			this._user = user;
		}

		public ResponseModel<List<Models.Article.SearchNummerResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				List<Models.Article.SearchNummerResponseModel> responseBody = new List<Models.Article.SearchNummerResponseModel> { };
				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetLikeNummer(this._data.Terms.Trim(), this._data.MaxItemsCount);
				if(articleEntities != null && articleEntities.Count > 0)
				{
					foreach(var articleEntity in articleEntities)
					{
						responseBody.Add(new Models.Article.SearchNummerResponseModel(articleEntity));
					}
				}

				return ResponseModel<List<Models.Article.SearchNummerResponseModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.SearchNummerResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.SearchNummerResponseModel>>.AccessDeniedResponse();
			}

			if(string.IsNullOrEmpty(this._data.Terms) || string.IsNullOrWhiteSpace(this._data.Terms) || this._data.Terms.Trim().Length < 2)
			{
				return ResponseModel<List<Models.Article.SearchNummerResponseModel>>.SuccessResponse(
					new List<Models.Article.SearchNummerResponseModel> { });
			}

			return ResponseModel<List<Models.Article.SearchNummerResponseModel>>.SuccessResponse();
		}
	}
}
