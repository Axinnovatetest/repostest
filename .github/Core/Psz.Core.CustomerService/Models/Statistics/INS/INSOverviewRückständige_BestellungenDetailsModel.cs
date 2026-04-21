using Psz.Core.Common.Models;
using System;

namespace Psz.Core.CustomerService.Models.Statistics.INS
{
	public class INSOverviewRückständige_BestellungenDetailsModel
	{
		public int? AngeboteNr { get; set; }
		public int? Nr { get; set; }
		public int? Position { get; set; }
		public int? ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public decimal? Anzahl { get; set; }
		public decimal? Gesamtpreis { get; set; }
		public DateTime? Liefertermin { get; set; }
		public INSOverviewRückständige_BestellungenDetailsModel(Infrastructure.Data.Entities.Joins.CTS.INSOverview.INSOverviewRückständige_BestellungenDetailsEntity entity)
		{
			AngeboteNr = entity.AngeboteNr;
			Nr = entity.Nr;
			Position = entity.Position;
			ArtikelNr = entity.ArtikelNr;
			Artikelnummer = entity.Artikelnummer;
			Anzahl = entity.Anzahl;
			Gesamtpreis = entity.Gesamtpreis;
			Liefertermin = entity.Liefertermin;
		}
	}
	public class INSOverviewRückständige_BestellungenDetailsRequestModel: IPaginatedRequestModel
	{
		public bool OlderDate { get; set; }
		public string Date { get; set; }
		public int? CustomerNumber { get; set; }
		public int? userId { get; set; }
		public string SearchText { get; set; }
	}
	public class INSOverviewRückständige_BestellungenDetailsResponseModel: IPaginatedResponseModel<INSOverviewRückständige_BestellungenDetailsModel> { }
}