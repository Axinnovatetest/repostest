using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.Reporting.Models
{
	public class RechnungStatsReportModel
	{
		public RechnungStatsReportHeaderModel ReportParameters { get; set; }
		public List<RechnungStatsReportDetailsModel> Details { get; set; }
		public List<RechnungStatsReportGroupedZollaTarif> Zollatarif { get; set; }
		public string TotalAusdr3 { get; set; }
		public string TotalZusatzkosten_Produktion { get; set; }
		public string TotalPreis_Total_Mit_Zusatzkosten { get; set; }
		public string TotalGewecht { get; set; }
		public string TotalPositionen { get; set; }
		public string TotalLohnleistun { get; set; }
		public string TotalZusatkosten { get; set; }
		public string TotalMaterial { get; set; }
		public string TotalStatWert { get; set; }
	}
	public class RechnungStatsReportHeaderModel: RechnungReportParametersModel
	{
		public string From { get; set; }
		public string To { get; set; }
		public string RechnungDatum { get; set; }
		public string RechnungNummer { get; set; }
		public string LimitDate { get; set; }
		public RechnungStatsReportHeaderModel(Infrastructure.Data.Entities.Tables.CTS.RechnungReportingEntity entity, RechnungStatsReportEntryModel model, string rechnungNummer)
		{
			From = model.From.ToString("dd.MM.yyyy");
			To = model.To.ToString("dd.MM.yyyy");
			RechnungDatum = model.RechnungsDatum.HasValue ? model.RechnungsDatum.Value.ToString("dd.MM.yyyy") : "";
			RechnungNummer = rechnungNummer;
			LimitDate = entity.Lager == 6 ? DateTime.Now.AddDays(10).ToString("dd.MM.yyyy") : "";
			Footer1 = entity.Footer1;
			Footer10 = entity.Footer10;
			Footer11 = entity.Footer11;
			Footer12 = entity.Footer12;
			Footer13 = entity.Footer13;
			Footer14 = entity.Footer14;
			Footer15 = entity.Footer15;
			Footer16 = entity.Footer16;
			Footer17 = entity.Footer17;
			Footer18 = entity.Footer18;
			Footer19 = entity.Footer19;
			Footer2 = entity.Footer2;
			Footer20 = entity.Footer20;
			Footer21 = entity.Footer21;
			Footer22 = entity.Footer22;
			Footer23 = entity.Footer23;
			Footer3 = entity.Footer3;
			Footer4 = entity.Footer4;
			Footer5 = entity.Footer5;
			Footer6 = entity.Footer6;
			Footer7 = entity.Footer7;
			Footer8 = entity.Footer8;
			Footer9 = entity.Footer9;
			Header1 = entity.Header1;
			Header2 = entity.Header2;
			Header3 = entity.Header3;
			Header4 = entity.Header4;
			Header5 = entity.Header5;
			Id = entity.Id;
			Lager = entity.Lager ?? 0;
			LastUpdateTime = entity.LastUpdateTime ?? DateTime.Now;
			LastUpdateUser = entity.LastUpdateUser ?? 0;
			List1Column1 = entity.List1Column1;
			List1Column10 = entity.List1Column10;
			List1Column11 = entity.List1Column11;
			List1Column12 = entity.List1Column12;
			List1Column13 = entity.List1Column13;
			List1Column14 = entity.List1Column14;
			List1Column15 = entity.List1Column15;
			List1Column16 = entity.List1Column16;
			List1Column17 = entity.List1Column17;
			List1Column18 = entity.List1Column18;
			List1Column2 = entity.List1Column2;
			List1Column3 = entity.List1Column3;
			List1Column4 = entity.List1Column4;
			List1Column5 = entity.List1Column5;
			List1Column6 = entity.List1Column6;
			List1Column7 = entity.List1Column7;
			List1Column8 = entity.List1Column8;
			List1Column9 = entity.List1Column9;
			List2Column1 = entity.List2Column1;
			List2Column2 = entity.List2Column2;
			List2Column3 = entity.List2Column3;
			List2Column4 = entity.List2Column4;
			List2Column5 = entity.List2Column5;
			List2Column6 = entity.List2Column6;
			List2Column7 = entity.List2Column7;
			List2Sum = entity.List2Sum;

			SumTitle1 = entity.SumTitle1;
			SumTitle2 = entity.SumTitle2;
			SumTitle3 = entity.SumTitle3;
			SumTitle4 = entity.SumTitle4;
			SumTitle5 = entity.SumTitle5;
			Title1 = entity.Title1;
			Title2 = entity.Title2;
			Title3 = entity.Title3;
			Title4 = entity.Title4;
			Title5 = entity.Title5;
			Title6 = entity.Title6;
			Title7 = entity.Title7;
		}
	}
	public class RechnungStatsReportDetailsModel
	{
		public string Ausdr3 { get; set; }
		public Decimal Ausdr3_decimal { get; set; }
		public string Datum { get; set; }
		public int Fertigungsnummer { get; set; }
		public string Originalanzahl { get; set; }
		public int Originalanzahl_Decimal { get; set; }
		public int Anzahl_erledigt { get; set; }
		public int Anzahl { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public Decimal Betrag { get; set; }
		public Decimal Preis { get; set; }
		public string Bemerkung { get; set; }
		public string Bezfeld { get; set; }
		public bool Erstmuster { get; set; }
		public string Zolltarif_nr { get; set; }
		public string Material1 { get; set; }
		public Decimal Größe { get; set; }
		public Decimal Gesamtgewicht { get; set; }
		public Decimal Stundensatz { get; set; }
		public string MinutenKosten { get; set; }
		public string Zusatzkosten_FA_Basis_30_Min { get; set; }
		public Decimal Zusatzkosten_FA_Basis_30_Min_Decimal { get; set; }
		public Decimal PREIS_Mit_Zusatzkosten_Pro_Stück { get; set; }
		public string Zusatzkosten_Produktion { get; set; }
		public Decimal Zusatzkosten_Produktion_Decimal { get; set; }
		public string Preis_Total_Mit_Zusatzkosten { get; set; }
		public Decimal Preis_Total_Mit_Zusatzkosten_Decimal { get; set; }
		public int Repeated { get; set; }
		public RechnungStatsReportDetailsModel()
		{

		}

		public RechnungStatsReportDetailsModel(Infrastructure.Data.Entities.Joins.CTS.RechnungEntity entity)
		{
			Ausdr3 = Psz.Core.CustomerService.Helpers.FormatHelper.FormatNumber(entity.Ausdr3 ?? 0);
			Ausdr3_decimal = entity.Ausdr3 ?? 0;
			Datum = entity.Datum.HasValue ? entity.Datum.Value.ToString("dd.MM.yyyy") : "";
			Fertigungsnummer = entity.Fertigungsnummer ?? 0;
			Originalanzahl = Psz.Core.CustomerService.Helpers.FormatHelper.FormatNumber(entity.Originalanzahl ?? 0);
			Originalanzahl_Decimal = entity.Originalanzahl ?? 0;
			Anzahl_erledigt = entity.Anzahl_erledigt ?? 0;
			Anzahl = entity.Anzahl ?? 0;
			Artikelnummer = entity.Artikelnummer;
			Bezeichnung1 = entity.Bezeichnung1;
			Betrag = entity.Betrag ?? 0;
			Preis = entity.Preis ?? 0;
			Bemerkung = entity.Bemerkung;
			Bezfeld = entity.Bezfeld;
			Erstmuster = entity.Erstmuster ?? false;
			Zolltarif_nr = entity.Zolltarif_nr;
			Material1 = Psz.Core.CustomerService.Helpers.FormatHelper.FormatNumber(entity.Material1 ?? 0);
			Größe = entity.Größe ?? 0;
			Gesamtgewicht = entity.Gesamtgewicht ?? 0;
			Stundensatz = entity.Stundensatz ?? 0;
			MinutenKosten = Psz.Core.CustomerService.Helpers.FormatHelper.FormatNumber(entity.MinutenKosten ?? 0);
			Zusatzkosten_FA_Basis_30_Min = Psz.Core.CustomerService.Helpers.FormatHelper.FormatNumber(entity.Zusatzkosten_FA_Basis_30_Min ?? 0);
			Zusatzkosten_FA_Basis_30_Min_Decimal = entity.Zusatzkosten_FA_Basis_30_Min ?? 0;
			PREIS_Mit_Zusatzkosten_Pro_Stück = entity.PREIS_Mit_Zusatzkosten_Pro_Stück ?? 0;
			Zusatzkosten_Produktion = Psz.Core.CustomerService.Helpers.FormatHelper.FormatNumber(entity.Zusatzkosten_Produktion ?? 0);
			Zusatzkosten_Produktion_Decimal = entity.Zusatzkosten_Produktion ?? 0;
			Preis_Total_Mit_Zusatzkosten = Psz.Core.CustomerService.Helpers.FormatHelper.FormatNumber(entity.Preis_Total_Mit_Zusatzkosten ?? 0);
			Preis_Total_Mit_Zusatzkosten_Decimal = entity.Preis_Total_Mit_Zusatzkosten ?? 0;
			Repeated = 0;
		}
	}
	public class RechnungStatsReportGroupedZollaTarif
	{
		public string Zolltarif_nummer { get; set; }
		public string Gewicht { get; set; }
		public int Positionen { get; set; }
		public string Lohnleistun { get; set; }
		public string Zusatzkosten { get; set; }
		public string Material { get; set; }
		public string Stat_Wert { get; set; }
	}
	public class RechnungStatsReportEntryModel
	{
		public DateTime From { get; set; }
		public DateTime To { get; set; }
		public int Lager { get; set; }
		public DateTime? RechnungsDatum { get; set; }
	}
}