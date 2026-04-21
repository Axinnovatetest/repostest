using Psz.Core.Common.Models;
using System;

namespace Psz.Core.CustomerService.Models.Statistics.INS
{
	public class INSOverviewRückständige_BestellungenModel
	{
		public int? AngebotNr { get; set; }
		public int? Nr { get; set; }
		public int? ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public string Kunde { get; set; }
		public string Produktionslager { get; set; }
		public string Mitarbeiter { get; set; }
		public decimal? Gesamtpreis { get; set; }
		public DateTime? Liefertermin { get; set; }
		public INSOverviewRückständige_BestellungenModel()
		{

		}

		public INSOverviewRückständige_BestellungenModel(Infrastructure.Data.Entities.Joins.CTS.INSOverview.INSOverviewRückständige_BestellungenEntity entity)
		{
			AngebotNr = entity.AngebotNr;
			Nr = entity.Nr;
			ArtikelNr = entity.ArtikelNr;
			Artikelnummer = entity.Artikelnummer;
			Kunde = entity.Kunde;
			Produktionslager = ((Enums.InsideSalesEnums.Warehouses)entity.Produktionslager).GetDescription();
			Mitarbeiter = entity.Mitarbeiter;
			Gesamtpreis = entity.Gesamtpreis;
			Liefertermin = entity.Liefertermin;
		}
	}
	public class INSOverviewRückständige_BestellungenModelRequestModel: IPaginatedRequestModel
	{
		public string Artikelnummer { get; set; }
		public int? Kundennummer { get; set; }
		public int? MitarbeiterId { get; set; }
		public int? Produktionslager { get; set; }
	}
	public class INSOverviewRückständige_BestellungenModelResponseModel: IPaginatedResponseModel<INSOverviewRückständige_BestellungenModel> { }

}