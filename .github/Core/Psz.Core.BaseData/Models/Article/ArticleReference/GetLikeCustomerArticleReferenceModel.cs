

namespace Psz.Core.BaseData.Models.Article.ArticleReference;

public class GetLikeCustomerArticleReferenceModel
{
	public string CustomerName { get; set; }
	public int? CustomerNumber { get; set; }
	public string CustomerReference { get; set; }

	public GetLikeCustomerArticleReferenceModel(Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesLikeEntity entity)
	{
		CustomerName = entity.CustomerName;
		CustomerNumber = entity.CustomerNumber;
		CustomerReference = entity.CustomerReference;
	}
}

public class GetLikeCustomerArticleReferenceRequestModel
{
	public int? CustomerId { get; set; } = -1;
	public string CustomerReference { get; set; } = null;
	public string supplierName { get; set; } = null;
}
public class ArticleInformationMinimalModel
{
	public int? ArticleId { get; set; } = -1;
	public string Bezeichnung1 { get; set; } = null;
}
