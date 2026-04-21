using Infrastructure.Services.Reporting.Models.MTM;
using Psz.Core.BaseData.Interfaces.Article;
using Psz.Core.BaseData.Models;
using Psz.Core.BaseData.Models.Article;
using Psz.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Handlers.Article
{
	public partial class ArticleService: IArticleService
	{

		public ResponseModel<List<Models.Article.ArtikelOutilCossModel>> GetDetailsOutilCossByArtikel(Identity.Models.UserModel user, int data)
		{
			try
			{
				var validationResponse = this.ValidationGetDetailsOutilCossByArtikel(user, data);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				//var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(data);
				var artikelExtensionProductionEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.GetByArticleId(data);
				var listArticleCossArtikelEntity = new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelOutilCossEntity>();
				var listArticleCossArtikelModel = new List<Models.Article.ArtikelOutilCossModel>();
				if(artikelExtensionProductionEntity != null)
				{

					listArticleCossArtikelEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelOutilCossAccess.Get(data, artikelExtensionProductionEntity.ProductionPlace1_Id.ToString());
					if(listArticleCossArtikelEntity != null && listArticleCossArtikelEntity.Count > 0)
					{


						for(int i = 0; i < listArticleCossArtikelEntity.Count; i++)
						{
							listArticleCossArtikelModel.Add(new Models.Article.ArtikelOutilCossModel(listArticleCossArtikelEntity[i], listArticleCossArtikelEntity[i].Outil.ToUpper().Contains("TN AB")));
						}
					}
				}

				return ResponseModel<List<Models.Article.ArtikelOutilCossModel>>.SuccessResponse(listArticleCossArtikelModel);


			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}

		}
		public ResponseModel<List<Models.Article.ArtikelOutilCossModel>> ValidationGetDetailsOutilCossByArtikel(Identity.Models.UserModel user, int data)
		{
			if(user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.ArtikelOutilCossModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Article.ArtikelOutilCossModel>>.SuccessResponse();
		}
	}
}
