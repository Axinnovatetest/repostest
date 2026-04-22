using Psz.Core.Common.Models;

namespace Psz.Core.BaseData.Models.Article.Statistics.Basics
{
	public class BinshRequestModel: IPaginatedRequestModel
	{
		public string ArticleNumber { get; set; }
		public string Designation { get; set; }
		public string ConfirmationStatus { get; set; }
	}
	public class BinshResponseModel: IPaginatedResponseModel<BinshItem>
	{
	}
	public class BinshItem
	{
		public string Artikelnummer { get; set; }

		public int? ArtikelNr { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Prufstatus_TN_AL { get; set; }
		public string Pruftiefe { get; set; }
		public BinshItem(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Binsh entity)
		{
			if(entity == null)
				return;

			// -
			ArtikelNr = entity.ArtikelNr;
			Artikelnummer = entity.Artikelnummer;
			Bezeichnung_1 = entity.Bezeichnung_1;
			Prufstatus_TN_AL = entity.Prufstatus_TN_AL;
			Pruftiefe = entity.Pruftiefe;
		}
	}
}
