using Psz.Core.Common.Models;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Article.Statistics.Basics
{
	public class ArticleLogisticsRequestModel: IPaginatedRequestModel
	{
		public bool? activeOnly { get; set; } = true;
		public bool? hasProductionPlace { get; set; } = null;
		public string ArticleNumber { get; set; }
		public List<string> ArticleEndings { get; set; }
	}
	public class ArticleLogisticsResponseModel: IPaginatedResponseModel<ArticleLogisticsItem>
	{
	}
	public class ArticleLogisticsItem
	{
		public int ArticleNr { get; set; }
		public string ArticleNumber { get; set; }
		public string Designation1 { get; set; }
		public int ProductionPlaceId { get; set; }
		public string ProductionPlaceName { get; set; }
		public ArticleLogisticsItem(Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity entity,
			Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity extensionEntity)
		{
			if(entity == null)
				return;

			ArticleNr = entity.ArtikelNr;
			ArticleNumber = entity.ArtikelNummer;
			ProductionPlaceId = (extensionEntity?.Lagerort_id ?? 0);
			ProductionPlaceName = extensionEntity?.Lagerort;
			Designation1 = entity.Bezeichnung1;
		}
	}
}
