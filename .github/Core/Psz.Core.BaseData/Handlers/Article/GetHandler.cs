using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Article.ArticleModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public GetHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<Models.Article.ArticleModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// get  angebote -- angebote-nr
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
				var artikelExtensionEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.GetByArticleNr(articleEntity.ArtikelNr);
				var artikelQualityExtensionEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelQualityExtensionAccess.GetByArticleId(articleEntity.ArtikelNr);
				var artikelProductionExtensionEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.GetByArticleId(articleEntity.ArtikelNr);
				var artikelLogisticsExtensionEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelLogisticsExtensionAccess.GetByArticleId(articleEntity.ArtikelNr);

				var result = new Models.Article.ArticleModel(articleEntity,
						artikelExtensionEntity,
						artikelQualityExtensionEntity,
						artikelProductionExtensionEntity,
						artikelLogisticsExtensionEntity);

				var next = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetNextArtikel(articleEntity.ArtikelNummer);
				if(next is not null && next.Count > 0)
				{
					result.NextNr = next.First().ArtikelNummer;
					result.NextArtikelNr = next.First().ArtikelNr;
				}


				var previous = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetPrevArtikel(articleEntity.ArtikelNummer);
				if(previous is not null && previous.Count > 0)
				{
					result.PreviousNr = previous.First().ArtikelNummer;
					result.PreviousArtikelNr = previous.First().ArtikelNr;
				}

				return ResponseModel<Models.Article.ArticleModel>.SuccessResponse(result
					);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Article.ArticleModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.ArticleModel>.AccessDeniedResponse();
			}

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
			if(articleEntity == null)
			{
				return new ResponseModel<Models.Article.ArticleModel>()
				{
					Errors = new List<ResponseModel<Models.Article.ArticleModel>.ResponseError>() {
						new ResponseModel<Models.Article.ArticleModel>.ResponseError {Key = "1", Value = "Article not found"}
					}
				};
			}

			return ResponseModel<Models.Article.ArticleModel>.SuccessResponse();
		}
	}

}
