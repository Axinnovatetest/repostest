using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Data
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetUBGNewerIndexHandler: IHandle<UserModel, ResponseModel<List<Tuple<Models.Article.ArticleMinimalModel, List<Models.Article.ArticleMinimalModel>>>>>
	{
		private UserModel _user { get; set; }
		private int _data { get; set; }
		public GetUBGNewerIndexHandler(UserModel user, int data)
		{
			this._user = user;
			_data = data;
		}
		public ResponseModel<List<Tuple<Models.Article.ArticleMinimalModel, List<Models.Article.ArticleMinimalModel>>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// - 
				return ResponseModel<List<Tuple<Models.Article.ArticleMinimalModel, List<Models.Article.ArticleMinimalModel>>>>.SuccessResponse(getData());
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Tuple<Models.Article.ArticleMinimalModel, List<Models.Article.ArticleMinimalModel>>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Tuple<Models.Article.ArticleMinimalModel, List<Models.Article.ArticleMinimalModel>>>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data) == null)
				return ResponseModel<List<Tuple<Models.Article.ArticleMinimalModel, List<Models.Article.ArticleMinimalModel>>>>.FailureResponse("Article not found");

			return ResponseModel<List<Tuple<Models.Article.ArticleMinimalModel, List<Models.Article.ArticleMinimalModel>>>>.SuccessResponse();
		}
		List<Tuple<Models.Article.ArticleMinimalModel, List<Models.Article.ArticleMinimalModel>>> getData()
		{
			var bomEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(this._data);
			if(bomEntities == null || bomEntities.Count <= 0)
			{
				return null;
			}

			var ubgEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(bomEntities.Select(x => x.Artikel_Nr_des_Bauteils ?? -1)?.ToList())
				?.Where(x => x.UBG == true && x.Warengruppe?.ToLower()?.Trim() == "ef")?.ToList();
			if(ubgEntities == null || ubgEntities.Count <= 0)
			{
				return null;
			}

			var results = new List<Tuple<Models.Article.ArticleMinimalModel, List<Models.Article.ArticleMinimalModel>>>();
			foreach(var ubgEntity in ubgEntities)
			{
				var newerCustomerVersions = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByCustomerItemNumber(ubgEntity.CustomerNumber ?? -1, ubgEntity.ArtikelNummer?.Substring(0, ubgEntity.ArtikelNummer?.IndexOf('-') ?? 0), ubgEntity.CustomerItemNumber)
					?.Where(x => x.CustomerIndexSequence > ubgEntity.CustomerIndexSequence)?.ToList();
				if(newerCustomerVersions != null && newerCustomerVersions.Count > 0)
				{
					results.Add(new Tuple<Models.Article.ArticleMinimalModel, List<Models.Article.ArticleMinimalModel>>(
						new Models.Article.ArticleMinimalModel(ubgEntity),
						newerCustomerVersions.Select(x => new Models.Article.ArticleMinimalModel(x))?.ToList()));
				}
			}
			// - 
			return results;
		}
	}
}
