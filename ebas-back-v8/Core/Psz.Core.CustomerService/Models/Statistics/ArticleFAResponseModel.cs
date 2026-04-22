namespace Psz.Core.CustomerService.Models.Statistics
{
	public class ArticleFAResponseModel
	{
		public string ArtikelNum { get; set; }
		public int? NBFA { get; set; }
		public int? ArtikelNr { get; set; }
		public string LagerName { get; set; }

		public ArticleFAResponseModel(Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatARTEntity entity)
		{
			ArtikelNum = entity.ArtikelNum;
			NBFA = entity.NbFA;
			ArtikelNr = entity.ArtikelNr;
			LagerName = entity.LagerName;
		}
	}
}
