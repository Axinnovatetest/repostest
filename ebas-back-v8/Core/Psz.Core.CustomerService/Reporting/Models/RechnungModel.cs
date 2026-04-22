using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Reporting.Models
{
	public class RechnungModel
	{
		public RechnungHeaderModel ReportParameters { get; set; }
		public List<RechnungDetailsModel> Details { get; set; }
		public List<RechnungGroupedZollaTarif> Zollatarif { get; set; }
	}
	public class RechnungDetailsModel
	{
		public Decimal Ausdr3 { get; set; }
		public string Datum { get; set; }
		public int Fertigungsnummer { get; set; }
		public int Originalanzahl { get; set; }
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
		public Decimal Material1 { get; set; }
		public Decimal Größe { get; set; }
		public Decimal Gesamtgewicht { get; set; }
		public Decimal Stundensatz { get; set; }
		public Decimal MinutenKosten { get; set; }
		public Decimal Zusatzkosten_FA_Basis_30_Min { get; set; }
		public Decimal PREIS_Mit_Zusatzkosten_Pro_Stück { get; set; }
		public Decimal Zusatzkosten_Produktion { get; set; }
		public Decimal Preis_Total_Mit_Zusatzkosten { get; set; }
		public int Repeated { get; set; }
		public RechnungDetailsModel()
		{

		}

		public RechnungDetailsModel(Infrastructure.Data.Entities.Joins.CTS.RechnungEntity entity)
		{
			Ausdr3 = entity.Ausdr3 ?? 0;
			Datum = entity.Datum.HasValue ? entity.Datum.Value.ToString("dd.MM.yyyy") : "";
			Fertigungsnummer = entity.Fertigungsnummer ?? 0;
			Originalanzahl = entity.Originalanzahl ?? 0;
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
			Material1 = entity.Material1 ?? 0;
			Größe = entity.Größe ?? 0;
			Gesamtgewicht = entity.Gesamtgewicht ?? 0;
			Stundensatz = entity.Stundensatz ?? 0;
			MinutenKosten = entity.MinutenKosten ?? 0;
			Zusatzkosten_FA_Basis_30_Min = entity.Zusatzkosten_FA_Basis_30_Min ?? 0;
			PREIS_Mit_Zusatzkosten_Pro_Stück = entity.PREIS_Mit_Zusatzkosten_Pro_Stück ?? 0;
			Zusatzkosten_Produktion = entity.Zusatzkosten_Produktion ?? 0;
			Preis_Total_Mit_Zusatzkosten = entity.Preis_Total_Mit_Zusatzkosten ?? 0;
			Repeated = 0;
		}
		public RechnungDetailsModel(Infrastructure.Data.Entities.Joins.CTS.RgSpritzgussEntity entity)
		{
			Ausdr3 = entity.Ausdr3 ?? 0;
			Datum = entity.Datum.HasValue ? entity.Datum.Value.ToString("dd.MM.yyyy") : "";
			Fertigungsnummer = entity.Fertigungsnummer ?? 0;
			Originalanzahl = entity.Originalanzahl ?? 0;
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
			Material1 = entity.Material1 ?? 0;
			Größe = entity.Größe ?? 0;
			Gesamtgewicht = entity.Gesamtgewicht ?? 0;
			Repeated = 0;
		}
	}

	public class RechnungHeaderModel: RechnungReportParametersModel
	{
		public string From { get; set; }
		public string To { get; set; }
		public string RechnungDatum { get; set; }
		public string RechnungNummer { get; set; }
		public string LimitDate { get; set; }
		public RechnungHeaderModel(Infrastructure.Data.Entities.Tables.CTS.RechnungReportingEntity entity, RechnungEntryModel model, string rechnungNummer)
		{
			From = model.From.ToString("dd.MM.yyyy");
			To = model.To.ToString("dd.MM.yyyy");
			RechnungDatum = model.RechnungsDatum.HasValue ? model.RechnungsDatum.Value.ToString("dd.MM.yyyy") : "";
			RechnungNummer = rechnungNummer;
			LimitDate = DateTime.Now.AddDays(10).ToString("dd.MM.yyyy");
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
			//try
			//{
			//	var comanyLogo = Module.FilesManager.GetFile(entity.LogoId ?? -1);
			//	Logo = comanyLogo?.FileBytes;
			//} catch(Exception)
			//{
			//	Logo = null;
			//}
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

	public class RechnungGroupedZollaTarif
	{
		public string Zolltarif_nummer { get; set; }
		public Decimal Gewicht { get; set; }
		public int Positionen { get; set; }
		public Decimal Lohnleistun { get; set; }
		public Decimal Zusatzkosten { get; set; }
		public Decimal Material { get; set; }
		public Decimal Stat_Wert { get; set; }
	}

	public class RechnungROHModel
	{
		public List<RechnungROHDetailsModel> Details { get; set; }
		public List<RechnungROHHeaderModel> Header { get; set; }
	}
	public class RechnungROHDetailsModel
	{
		public string Warenummer { get; set; }
		public Decimal Anzahl { get; set; }
		public Decimal Gewicht { get; set; }
		public Decimal WarenWert { get; set; }
	}
	public class RechnungROHTNModel
	{
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public Decimal Anzahl { get; set; }
	}
	public class RechnungROHHeaderModel
	{
		public string From { get; set; }
		public string To { get; set; }
		public string RechnungDatum { get; set; }
	}
	public class RechnungReportParametersModel
	{
		public string Footer1 { get; set; }
		public string Footer10 { get; set; }
		public string Footer11 { get; set; }
		public string Footer12 { get; set; }
		public string Footer13 { get; set; }
		public string Footer14 { get; set; }
		public string Footer15 { get; set; }
		public string Footer16 { get; set; }
		public string Footer17 { get; set; }
		public string Footer18 { get; set; }
		public string Footer19 { get; set; }
		public string Footer2 { get; set; }
		public string Footer20 { get; set; }
		public string Footer21 { get; set; }
		public string Footer22 { get; set; }
		public string Footer23 { get; set; }
		public string Footer3 { get; set; }
		public string Footer4 { get; set; }
		public string Footer5 { get; set; }
		public string Footer6 { get; set; }
		public string Footer7 { get; set; }
		public string Footer8 { get; set; }
		public string Footer9 { get; set; }
		public string Header1 { get; set; }
		public string Header2 { get; set; }
		public string Header3 { get; set; }
		public string Header4 { get; set; }
		public string Header5 { get; set; }
		public int Id { get; set; }
		public int Lager { get; set; }
		public DateTime LastUpdateTime { get; set; }
		public int LastUpdateUser { get; set; }
		public string List1Column1 { get; set; }
		public string List1Column10 { get; set; }
		public string List1Column11 { get; set; }
		public string List1Column12 { get; set; }
		public string List1Column13 { get; set; }
		public string List1Column14 { get; set; }
		public string List1Column15 { get; set; }
		public string List1Column16 { get; set; }
		public string List1Column17 { get; set; }
		public string List1Column18 { get; set; }
		public string List1Column2 { get; set; }
		public string List1Column3 { get; set; }
		public string List1Column4 { get; set; }
		public string List1Column5 { get; set; }
		public string List1Column6 { get; set; }
		public string List1Column7 { get; set; }
		public string List1Column8 { get; set; }
		public string List1Column9 { get; set; }
		public string List2Column1 { get; set; }
		public string List2Column2 { get; set; }
		public string List2Column3 { get; set; }
		public string List2Column4 { get; set; }
		public string List2Column5 { get; set; }
		public string List2Column6 { get; set; }
		public string List2Column7 { get; set; }
		public string List2Sum { get; set; }
		public string Logo { get; set; }
		public string SumTitle1 { get; set; }
		public string SumTitle2 { get; set; }
		public string SumTitle3 { get; set; }
		public string SumTitle4 { get; set; }
		public string SumTitle5 { get; set; }
		public string Title1 { get; set; }
		public string Title2 { get; set; }
		public string Title3 { get; set; }
		public string Title4 { get; set; }
		public string Title5 { get; set; }
		public string Title6 { get; set; }
		public string Title7 { get; set; }
		public string Legend { get; set; }

		public RechnungReportParametersModel()
		{

		}

		public Infrastructure.Data.Entities.Tables.CTS.RechnungReportingEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.CTS.RechnungReportingEntity
			{
				Footer1 = Footer1,
				Footer10 = Footer10,
				Footer11 = Footer11,
				Footer12 = Footer12,
				Footer13 = Footer13,
				Footer14 = Footer14,
				Footer15 = Footer15,
				Footer16 = Footer16,
				Footer17 = Footer17,
				Footer18 = Footer18,
				Footer19 = Footer19,
				Footer2 = Footer2,
				Footer20 = Footer20,
				Footer21 = Footer21,
				Footer22 = Footer22,
				Footer23 = Footer23,
				Footer3 = Footer3,
				Footer4 = Footer4,
				Footer5 = Footer5,
				Footer6 = Footer6,
				Footer7 = Footer7,
				Footer8 = Footer8,
				Footer9 = Footer9,
				Header1 = Header1,
				Header2 = Header2,
				Header3 = Header3,
				Header4 = Header4,
				Header5 = Header5,
				Id = Id,
				Lager = Lager,
				LastUpdateTime = LastUpdateTime,
				LastUpdateUser = LastUpdateUser,
				List1Column1 = List1Column1,
				List1Column10 = List1Column10,
				List1Column11 = List1Column11,
				List1Column12 = List1Column12,
				List1Column13 = List1Column13,
				List1Column14 = List1Column14,
				List1Column15 = List1Column15,
				List1Column16 = List1Column16,
				List1Column17 = List1Column17,
				List1Column18 = List1Column18,
				List1Column2 = List1Column2,
				List1Column3 = List1Column3,
				List1Column4 = List1Column4,
				List1Column5 = List1Column5,
				List1Column6 = List1Column6,
				List1Column7 = List1Column7,
				List1Column8 = List1Column8,
				List1Column9 = List1Column9,
				List2Column1 = List2Column1,
				List2Column2 = List2Column2,
				List2Column3 = List2Column3,
				List2Column4 = List2Column4,
				List2Column5 = List2Column5,
				List2Column6 = List2Column6,
				List2Column7 = List2Column7,
				List2Sum = List2Sum,
				SumTitle1 = SumTitle1,
				SumTitle2 = SumTitle2,
				SumTitle3 = SumTitle3,
				SumTitle4 = SumTitle4,
				SumTitle5 = SumTitle5,
				Title1 = Title1,
				Title2 = Title2,
				Title3 = Title3,
				Title4 = Title4,
				Title5 = Title5,
				Title6 = Title6,
				Title7 = Title7,
			};
		}
	}
}
