using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FAPlanning;
using Psz.Core.Identity.Models;


namespace Psz.Core.CRP.Handlers.FAPlannung
{
	public partial class CrpFAPlannung
	{
		public ResponseModel<ArticleInfoModel> ValidateGetArticleInformations(UserModel user)
		{
			if(user == null)
				return ResponseModel<ArticleInfoModel>.AccessDeniedResponse();
			return ResponseModel<ArticleInfoModel>.SuccessResponse();
		}
		public ResponseModel<ArticleInfoModel> GetArticleInformation(UserModel user, int articleNr)
		{
			try
			{
				var validationResponse = ValidateGetArticleInformations(user);
				if(!validationResponse.Success)
					return validationResponse;

				var response = new ArticleInfoModel();
				var minimumStock = 0;
				var sales = new Psz.Core.BaseData.Handlers.Article.SalesExtension.GetByArticleHandler(user, articleNr).Handle();
				if(!sales.Success)
				{
					sales.Body = new List<BaseData.Models.Article.SalesExtension.SalesItemModel> { new BaseData.Models.Article.SalesExtension.SalesItemModel { Type = "Serie", LotSize = 0, PackagingQuantity = 0, PackagingTypeId = 0, ProductionTime = 0 } };
				}
				// - 
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(articleNr);
				minimumStock = Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.GetArticleMinimumStock(articleNr);
				var residueFas = Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.GetResidueFaByArticle(articleNr);
				var productionExtEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.GetByArticleId(articleNr);
				response = new ArticleInfoModel(sales.Body.FirstOrDefault(x => x.Type == "Serie"), minimumStock, articleEntity?.ProductionLotSize ?? 0, residueFas.Key, residueFas.Value, articleEntity.Freigabestatus, productionExtEntity?.ProductionPlace1_Name ?? "", articleEntity.BemerkungCRPPlanung ?? "");

				return ResponseModel<ArticleInfoModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}