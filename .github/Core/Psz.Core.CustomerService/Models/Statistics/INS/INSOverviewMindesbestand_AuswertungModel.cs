using Psz.Core.Common.Models;

namespace Psz.Core.CustomerService.Models.Statistics.INS
{
	public class INSOverviewMindesbestand_AuswertungModel
	{
		public int? ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public string Kunde { get; set; }
		public decimal? Verkaufspreis { get; set; }
		public string Produktionslager { get; set; }
		public string Mitarbeiter { get; set; }
		public decimal? Mindestbestand { get; set; }
		public decimal? Bestand { get; set; }
		public decimal? Differenz { get; set; }
		public decimal? Differenzwert { get; set; }
		public INSOverviewMindesbestand_AuswertungModel()
		{

		}
		public INSOverviewMindesbestand_AuswertungModel(Infrastructure.Data.Entities.Joins.CTS.INSOverview.INSOverviewMindesbestand_AuswertungEntity entity)
		{
			ArtikelNr = entity.ArtikelNr;
			Artikelnummer = entity.Artikelnummer;
			Kunde = entity.Kunde;
			Verkaufspreis = entity.Verkaufspreis;
			Produktionslager = ((Enums.InsideSalesEnums.Warehouses)entity.Produktionslager).GetDescription();
			Mitarbeiter = entity.Mitarbeiter;
			Mindestbestand = entity.Mindestbestand;
			Bestand = entity.Bestand;
			Differenz = entity.Differenz;
			Differenzwert = entity.Differenzwert;
		}
	}
	public class INSOverviewMindesbestand_AuswertungRequestModel: IPaginatedRequestModel
	{
		public string Artikelnummer { get; set; }
		public int? Kundennummer { get; set; }
		public int? MitarbeiterId { get; set; }
		public int? Produktionslager { get; set; }
		public int? Type { get; set; }
	}
	public class INSOverviewMindesbestand_AuswertungResponseModel: IPaginatedResponseModel<INSOverviewMindesbestand_AuswertungModel> { }
}