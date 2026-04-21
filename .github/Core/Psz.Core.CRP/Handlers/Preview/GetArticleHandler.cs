using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.Preview;
using Psz.Core.CRP.Interfaces;
using MoreLinq;

namespace Psz.Core.CRP.Handlers.Preview
{
	public partial class PreviewService: IPreviewService
	{
		public ResponseModel<PreviewArticleResponseModel> GetArticleHandler(Identity.Models.UserModel user, int articleId)
		{
			if(user == null)
				return ResponseModel<PreviewArticleResponseModel>.AccessDeniedResponse();

			var result = new PreviewArticleResponseModel();
			/* header */
			var article = Infrastructure.Data.Access.Tables.CRP.CrpPreviewArticlesAccess.GetByArticle(articleId);
			if(article == null)
				return ResponseModel<PreviewArticleResponseModel>.SuccessResponse(result);

			result.ArticleNumber = article.ArticleNumber;
			result.ArticleId = article.ArticleId;
			result.ArticleDesignation = article.Designation;
			result.ArticleStatus = article.ExternalStatus;
			result.Stock = article.Stock;
			result.MinimumStock = article.MinimumStock;
			result.SumFa = article.SumProds;
			result.SumNeeds = article.SumNeeds;
			result.SumNeedsAB = article.SumNeedsAB;
			result.SumNeedsFC = article.SumNeedsFC;
			result.SumNeedsLP = article.SumNeedsLP;
			result.SyncDate = article.SyncDate;
			// -
			result.Difference = result.Stock + result.SumFa - (result.SumNeedsAB + result.SumNeedsFC + result.SumNeedsLP);

			/* data */
			var backlog = Infrastructure.Data.Access.Joins.CRP.PreviewAccess.GetQuantitiesByArticleId_Backlog(articleId);
			var entities = Infrastructure.Data.Access.Joins.CRP.PreviewAccess.GetQuantitiesByArticleId(articleId)
				?? new List<Infrastructure.Data.Entities.Joins.CRP.PreviewQuantitiesEntity>();
			

			var weeks = GetHeadersHandler(user);
			if(weeks.Success && weeks.Body?.Count > 0)
			{
				var missingWeeks = weeks.Body.Where(x => !entities.Any(y => y.Year == x.Year && y.Week == x.Week));
				entities.AddRange(missingWeeks.Select(x => new Infrastructure.Data.Entities.Joins.CRP.PreviewQuantitiesEntity { ArticleId = articleId, Year = x.Year, Week = x.Week, ABQuantity=0, FAQuantity=0, FCQuantity=0, LPQuantity=0 }));
			}

			result.Data = entities?.OrderBy(x => x.Year).ThenBy(x => x.Week)?.Select(x => new PreviewDataWeekResponseModel(x))?.ToList();

			// -
			if(backlog is not null)
			{
				var b = result.Data.FirstOrDefault(x => x.Year == (DateTime.Today.AddDays(-7)).Year && x.Week == Common.Helpers.DateHelpers.GetPreviousIsoWeekNumber(DateTime.Today));
				if(b is not null)
				{
					b.SumAb = backlog?.ABQuantity ?? 0;
					b.SumFa = backlog?.FAQuantity ?? 0;
					b.SumFc = backlog?.FCQuantity ?? 0;
					b.SumLp = backlog?.LPQuantity ?? 0;
				}
			}
			// -
			return ResponseModel<PreviewArticleResponseModel>.SuccessResponse(result);
		}
	}
}
