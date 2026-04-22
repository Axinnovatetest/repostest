using System;
using System.Collections.Generic;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class GetAuschusskosten_Technik_InfoModel
	{
		public DateTime? Datum { get; set; }
		public string Typ { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public decimal? Anzahl { get; set; }
		public string Einheit { get; set; }
		public string Lagerort { get; set; }
		public int? Fertigungsnummer { get; set; }
		public int? Grund { get; set; }
		public string Bemerkung { get; set; }
		public decimal Kosten { get; set; }
		public int totalRows { get; set; }
		public int Rollennummer { get; set; }
		public GetAuschusskosten_Technik_InfoModel() { }
		public GetAuschusskosten_Technik_InfoModel(Infrastructure.Data.Entities.Joins.Logistics.Auschusskosten_Technik_InfoEntity Auschusskosten_Technik_InfoEntity)
		{

			if(Auschusskosten_Technik_InfoEntity == null)
				return;

			Datum = Auschusskosten_Technik_InfoEntity.Datum;
			Typ = Auschusskosten_Technik_InfoEntity.Typ;
			Artikelnummer = Auschusskosten_Technik_InfoEntity.Artikelnummer;
			Bezeichnung1 = Auschusskosten_Technik_InfoEntity.Bezeichnung1;
			Anzahl = Auschusskosten_Technik_InfoEntity.Anzahl;
			Einheit = Auschusskosten_Technik_InfoEntity.Einheit;
			Lagerort = Auschusskosten_Technik_InfoEntity.Lagerort;
			Fertigungsnummer = Auschusskosten_Technik_InfoEntity.Fertigungsnummer;
			Grund = Auschusskosten_Technik_InfoEntity.Grund;
			Bemerkung = Auschusskosten_Technik_InfoEntity.Bemerkung;
			Kosten = Auschusskosten_Technik_InfoEntity.Kosten;
			totalRows = Auschusskosten_Technik_InfoEntity.totalRows;
			Rollennummer = Auschusskosten_Technik_InfoEntity.Rollennummer;


		}
	}

	public class GetAuschusskosten_Technik_InfoPDFDetailsModel
	{
		public string Datum { get; set; }
		public string Typ { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public decimal Anzahl { get; set; }
		public string Einheit { get; set; }
		public string Lagerort { get; set; }
		public int Fertigungsnummer { get; set; }
		public int Grund { get; set; }
		public string Bemerkung { get; set; }
		public decimal Kosten { get; set; }
		public int totalRows { get; set; }
		public int Rollennummer { get; set; }
		public GetAuschusskosten_Technik_InfoPDFDetailsModel()
		{

		}

		public GetAuschusskosten_Technik_InfoPDFDetailsModel(Infrastructure.Data.Entities.Joins.Logistics.Auschusskosten_Technik_InfoEntity Auschusskosten_Technik_InfoEntity)
		{
			if(Auschusskosten_Technik_InfoEntity == null)
				return;

			Datum = Auschusskosten_Technik_InfoEntity.Datum.HasValue ? Auschusskosten_Technik_InfoEntity.Datum.Value.ToString("dd/MM/yyyy") : "";
			Typ = Auschusskosten_Technik_InfoEntity.Typ;
			Artikelnummer = Auschusskosten_Technik_InfoEntity.Artikelnummer;
			Bezeichnung1 = Auschusskosten_Technik_InfoEntity.Bezeichnung1;
			Anzahl = Auschusskosten_Technik_InfoEntity.Anzahl ?? 0;
			Einheit = Auschusskosten_Technik_InfoEntity.Einheit;
			Lagerort = Auschusskosten_Technik_InfoEntity.Lagerort;
			Fertigungsnummer = Auschusskosten_Technik_InfoEntity.Fertigungsnummer ?? 0;
			Grund = Auschusskosten_Technik_InfoEntity.Grund ?? -1;
			Bemerkung = Auschusskosten_Technik_InfoEntity.Bemerkung;
			Kosten = Auschusskosten_Technik_InfoEntity.Kosten;
			totalRows = Auschusskosten_Technik_InfoEntity.totalRows;
			Rollennummer = Auschusskosten_Technik_InfoEntity.Rollennummer;
		}
	}

	public class GetAuschusskosten_Technik_InfoPDFHeaderModel
	{
		public string LagerNummer { get; set; }
		public string Von { get; set; }
		public string Bis { get; set; }
		public decimal TotalKosten { get; set; }
		public string CurrentDate { get; set; }
	}

	public class GetAuschusskosten_Technik_InfoPDFSumModel
	{
		public string Date { get; set; }
		public string FooterText { get; set; }
		public decimal SumKosten { get; set; }
		public decimal PercentageKosten { get; set; }
	}

	public class GetAuschusskosten_Technik_InfoPDFModel
	{
		public List<GetAuschusskosten_Technik_InfoPDFDetailsModel> Details { get; set; }
		public List<GetAuschusskosten_Technik_InfoPDFHeaderModel> Header { get; set; }
		public List<GetAuschusskosten_Technik_InfoPDFSumModel> Sums { get; set; }
	}
}
