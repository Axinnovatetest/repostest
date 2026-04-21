using Psz.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Interfaces.Article
{
	public interface IArticleService
	{
		ResponseModel<Models.Article.ArticleSearchResponseModel> SearchArticleAdvanced(Identity.Models.UserModel user, Models.Article.ArticleSearchAdvancedModel data);
		public ResponseModel<int> UpdatePreviousManufacturerArticle(Identity.Models.UserModel user, Models.Article.Overview.UpdateManufacturerArticleRequestModel data);
		public ResponseModel<int> UpdateNextManufacturerArticle(Identity.Models.UserModel user, Models.Article.Overview.UpdateManufacturerArticleRequestModel data);
		public ResponseModel<int> ResetPreviousManufacturerArticle(Identity.Models.UserModel user, int data);
		public ResponseModel<int> ResetNextManufacturerArticle(Identity.Models.UserModel user, int data);
		ResponseModel<int> UpdateArticlePmData(Identity.Models.UserModel user, Models.Article.ArticleOverviewModel data);
		ResponseModel<List<Models.Article.ArtikelOutilCossModel>> GetDetailsOutilCossByArtikel(Identity.Models.UserModel user, int data);
		ResponseModel<byte[]> GetListeOutilCossByArtikelExcel(Identity.Models.UserModel user, int data);
	}
}