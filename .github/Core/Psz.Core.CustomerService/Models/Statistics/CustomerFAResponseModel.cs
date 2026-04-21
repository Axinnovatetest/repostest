namespace Psz.Core.CustomerService.Models.Statistics
{
	public class CustomerFAResponseModel
	{
		public string ArtikelNum { get; set; }
		public string Kunde { get; set; }
		public int? NBFA { get; set; }
		public int? ArtikelNr { get; set; }
		public CustomerFAResponseModel(Infrastructure.Data.Entities.Joins.CTS.StatisticsEntity.StatCustomerFAEntity entity)
		{
			ArtikelNum = entity.ArtikelNum;
			NBFA = entity.NbFA;
			ArtikelNr = entity.ArtikelNr;
			Kunde = entity.Kunde;
		}
	}
}
