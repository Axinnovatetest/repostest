using System;

namespace Psz.Core.BaseData.Models.Article.Statistics.CustomerService
{
	public class AVEvaluationResponseModel
	{
		public string Anderung_von { get; set; }
		public string Anderungsbeschreibung { get; set; }
		public string Artikelnummer { get; set; }
		public int? ArtikelNr { get; set; }
		public string Bezeichnung_1 { get; set; }
		public DateTime? Datum_Anderung { get; set; }
		public decimal? DB_I_ohne { get; set; }
		public string Einheit { get; set; }
		public decimal? Einkaufspreis { get; set; }
		public Single? Exportgewicht { get; set; }
		public decimal? Gewicht { get; set; }
		public Single? Groesse { get; set; }
		public int? Losgroesse { get; set; }
		public string Name1 { get; set; }
		public decimal? Preiseinheit { get; set; }
		public decimal? Preisgruppen_Einkaufspreis { get; set; }
		public Single? Produktionszeit { get; set; }
		public bool? Standardlieferant { get; set; }
		public decimal? Stundensatz { get; set; }
		public decimal? Umsatzsteuer { get; set; }
		public decimal? Verkaufspreis { get; set; }
		public string Verpackungsart { get; set; }
		public int? Verpackungsmenge { get; set; }
		public string Warengruppe { get; set; }
		public int? Warentyp { get; set; }
		public string Zolltarif_nr { get; set; }

		public AVEvaluationResponseModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CS_AVEvaluation entity)
		{
			if(entity == null)
				return;
			ArtikelNr = entity.ArtikelNr;
			Anderung_von = entity.Anderung_von;
			Anderungsbeschreibung = entity.Anderungsbeschreibung;
			Artikelnummer = entity.Artikelnummer;
			Bezeichnung_1 = entity.Bezeichnung_1;
			Datum_Anderung = entity.Datum_Anderung;
			DB_I_ohne = entity.DB_I_ohne;
			Einheit = entity.Einheit;
			Einkaufspreis = entity.Einkaufspreis;
			Exportgewicht = entity.Exportgewicht;
			Gewicht = entity.Gewicht;
			Groesse = entity.Groesse;
			Losgroesse = entity.Losgroesse;
			Name1 = entity.Name1;
			Preiseinheit = entity.Preiseinheit;
			Preisgruppen_Einkaufspreis = entity.Preisgruppen_Einkaufspreis;
			Produktionszeit = entity.Produktionszeit;
			Standardlieferant = entity.Standardlieferant;
			Stundensatz = entity.Stundensatz;
			Umsatzsteuer = entity.Umsatzsteuer;
			Verkaufspreis = entity.Verkaufspreis;
			Verpackungsart = entity.Verpackungsart;
			Verpackungsmenge = entity.Verpackungsmenge;
			Warengruppe = entity.Warengruppe;
			Warentyp = entity.Warentyp;
			Zolltarif_nr = entity.Zolltarif_nr;
		}
	}
}
