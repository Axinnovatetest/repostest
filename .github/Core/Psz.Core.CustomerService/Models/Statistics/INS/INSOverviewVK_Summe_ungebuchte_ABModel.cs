using Psz.Core.Common.Models;

namespace Psz.Core.CustomerService.Models.Statistics.INS
{
	public class INSOverviewVK_Summe_ungebuchte_ABModel
	{
		public int? AngebotNr { get; set; }
		public int? Nr { get; set; }
		public int? ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public decimal? Verkaufspreis { get; set; }
		public string Kunde { get; set; }
		public string Produktionslager { get; set; }
		public string Mitarbeiter { get; set; }
		public decimal? Gesamtpreis { get; set; }
		public INSOverviewVK_Summe_ungebuchte_ABModel()
		{

		}
		public INSOverviewVK_Summe_ungebuchte_ABModel(Infrastructure.Data.Entities.Joins.CTS.INSOverview.INSOverviewVK_Summe_ungebuchte_ABEntity entity)
		{
			AngebotNr = entity.AngebotNr;
			Nr = entity.Nr;
			ArtikelNr = entity.ArtikelNr;
			Artikelnummer = entity.Artikelnummer;
			Verkaufspreis = entity.Verkaufspreis;
			Kunde = entity.Kunde;
			Produktionslager = ((Enums.InsideSalesEnums.Warehouses)entity.Produktionslager).GetDescription();
			Mitarbeiter = entity.Mitarbeiter;
			Gesamtpreis = entity.Gesamtpreis;
		}
	}

	public class INSOverviewVK_Summe_ungebuchte_ABRequestModel: IPaginatedRequestModel
	{
		public string Artikelnummer { get; set; }
		public int? Kundennummer { get; set; }
		public int? MitarbeiterId { get; set; }
		public int? Produktionslager { get; set; }
	}
	public class INSOverviewVK_Summe_ungebuchte_ABResponseModel: IPaginatedResponseModel<INSOverviewVK_Summe_ungebuchte_ABModel> { }
}