using System;

namespace Psz.Core.BaseData.Models.Article.Statistics.Controlling
{
	public class DBResponseModel
	{
		public Single? Anzahl { get; set; }
		public int? Artikel_Nr { get; set; }
		public string Artikelnummer { get; set; }
		public string ArtikelnummerOriginal { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Bezeichnung_2 { get; set; }
		public string Bezeichnung_des_Bauteils { get; set; }
		public int? DEL { get; set; }
		public decimal? Einkaufspreis { get; set; }
		public decimal? Gewicht { get; set; }
		public int? Kupferbasis { get; set; }
		public decimal? Kupferzahl { get; set; }
		public decimal? Kupferzuschlag { get; set; }
		public string Name1 { get; set; }
		public string Position { get; set; }
		public int? Preisgruppe { get; set; }
		public bool? Standardlieferant { get; set; }
		public decimal? Summe { get; set; }
		public decimal? SummeEK { get; set; }
		public decimal? SummeEKohneCU { get; set; }
		public decimal? SummevonBetrag { get; set; }
		public decimal? Verkaufspreis { get; set; }
		public decimal? Verkaufspreis_1 { get; set; }
		public decimal? VK_PSZ_ink_Kupfer { get; set; }
		public DBResponseModel()
		{

		}
		public DBResponseModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingDB controllingDb)
		{
			if(controllingDb == null)
				return;

			Anzahl = controllingDb.Anzahl;
			Artikel_Nr = controllingDb.Artikel_Nr;
			Artikelnummer = controllingDb.Artikelnummer;
			ArtikelnummerOriginal = controllingDb.ArtikelnummerOriginal;
			Bezeichnung_1 = controllingDb.Bezeichnung_1;
			Bezeichnung_2 = controllingDb.Bezeichnung_2;
			Bezeichnung_des_Bauteils = controllingDb.Bezeichnung_des_Bauteils;
			DEL = controllingDb.DEL;
			Einkaufspreis = controllingDb.Einkaufspreis;
			Gewicht = controllingDb.Gewicht;
			Kupferbasis = controllingDb.Kupferbasis;
			Kupferzahl = controllingDb.Kupferzahl;
			Kupferzuschlag = controllingDb.Kupferzuschlag;
			Name1 = controllingDb.Name1;
			Position = controllingDb.Position;
			Preisgruppe = controllingDb.Preisgruppe;
			Standardlieferant = controllingDb.Standardlieferant;
			Summe = controllingDb.Summe;
			SummeEK = controllingDb.SummeEK;
			SummeEKohneCU = controllingDb.SummeEKohneCU;
			SummevonBetrag = controllingDb.SummevonBetrag;
			Verkaufspreis = controllingDb.Verkaufspreis;
			Verkaufspreis_1 = controllingDb.Verkaufspreis_1;
			VK_PSZ_ink_Kupfer = controllingDb.VK_PSZ_ink_Kupfer;
		}
	}
}
