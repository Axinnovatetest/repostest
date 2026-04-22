using Psz.Core.Common.Models;
namespace Psz.Core.BaseData.Models.Article.Statistics.Sales
{
	public class ArticleMinStockDetailedAnalysisModels
	{
		public class ArticleMinStockDetailedAnalysisResponseModel: IPaginatedResponseModel<Infrastructure.Data.Entities.Joins.MinStockAnalysisEntity>
		{
		}

		public class ArticleMinStockDetailedAnalysisRequestModel: IPaginatedRequestModel
		{
			public string ArticleNumber { get; set; }

		}

	}
}
