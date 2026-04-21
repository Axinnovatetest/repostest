using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public partial class SearchNrHandler: IHandle<string, ResponseModel<List<Models.Article.CustomerSearchResponseModel>>>
	{
		private string _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public SearchNrHandler(Identity.Models.UserModel user, string data)
		{
			this._data = data;
			this._user = user;
		}

		public ResponseModel<List<Models.Article.CustomerSearchResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				this._data = (this._data ?? "").Trim();

				var responseBody = new List<Models.Article.CustomerSearchResponseModel> { };
				var articleEntities = Infrastructure.Data.Access.Tables.PRS.View_PSZ_Artikel_Kundenzuweisung2Access.GetLikeNr(this._data);
				if(articleEntities != null && articleEntities.Count > 0)
				{
					foreach(var articleEntity in articleEntities)
					{
						responseBody.Add(new Models.Article.CustomerSearchResponseModel { Key = articleEntity.Kunde.ToString(), Value = $"{articleEntity.PSZ_Artikel3} | {articleEntity.Kunde}".Trim(), CustomerPrefix = articleEntity.PSZ_Artikel3 });
					}
					responseBody = responseBody.DistinctBy(x => x.Value).ToList();
				}

				return ResponseModel<List<Models.Article.CustomerSearchResponseModel>>.SuccessResponse(responseBody);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.CustomerSearchResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.CustomerSearchResponseModel>>.AccessDeniedResponse();
			}

			if(string.IsNullOrEmpty(this._data) || string.IsNullOrWhiteSpace(this._data) || this._data.Trim().Length < 2)
			{
				return ResponseModel<List<Models.Article.CustomerSearchResponseModel>>.SuccessResponse(
					new List<Models.Article.CustomerSearchResponseModel> { });
			}

			return ResponseModel<List<Models.Article.CustomerSearchResponseModel>>.SuccessResponse();
		}
	}
}
