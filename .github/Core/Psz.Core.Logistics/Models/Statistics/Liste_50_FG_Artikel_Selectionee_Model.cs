using System;
using System.Collections.Generic;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class Liste_50_FG_Artikel_Selectionee_Model
	{
		public Liste_50_FG_Artikel_Selectionee_Model(Infrastructure.Data.Entities.Joins.Logistics.Liste_50_FG_Artikel_Selectionee_Entity _data)
		{
			Lagerort_id = _data.Lagerort_id;
			CCID = _data.CCID;
			ArtikelNr = _data.ArtikelNr;
			Bestand = _data.Bestand;
			Artikelnummer = _data.Artikelnummer;
			Bezeichnung1 = _data.Bezeichnung1;
			Stuckliste = _data.Stuckliste;
			CCID_Datum = _data.CCID_Datum;
			Lagerort = _data.Lagerort;
		}
		public int Lagerort_id { get; set; }
		public int CCID { get; set; }
		public int ArtikelNr { get; set; }
		public decimal Bestand { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public int Stuckliste { get; set; }
		public DateTime? CCID_Datum { get; set; }
		public string Lagerort { get; set; }
	}
	public class Liste_50_FG_Artikel_Selectione_Model_Details
	{
		public Liste_50_FG_Artikel_Selectione_Model_Details()
		{

		}
		public Liste_50_FG_Artikel_Selectione_Model_Details(Liste_50_FG_Artikel_Selectionee_Model _data)
		{
			if(_data == null)
			{ return; }

			Lagerort_id = _data.Lagerort_id;
			CCID = _data.CCID;
			ArtikelNr = _data.ArtikelNr;
			Bestand = Convert.ToDecimal(String.Format("{0:0.00}", _data.Bestand));
			Artikelnummer = _data.Artikelnummer;
			Bezeichnung1 = _data.Bezeichnung1;
			Stuckliste = _data.Stuckliste;
			CCID_Datum = _data.CCID_Datum.HasValue ? _data.CCID_Datum.Value.ToString("dd/MM/yyyy") : "";
			Lagerort = _data.Lagerort;
		}
		public int Lagerort_id { get; set; }
		public int CCID { get; set; }
		public int ArtikelNr { get; set; }
		public decimal Bestand { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public int Stuckliste { get; set; }
		public string CCID_Datum { get; set; }
		public string Lagerort { get; set; }

	}
	public class PSZ_CCFG_Artikeltabelle_Header
	{
		public int Rest { get; set; }
		public int Total { get; set; }
		public int Lagerort_id { get; set; }
		public string Lagerort { get; set; }
		public string DTime { get; set; }
	}
	public class PSZ_CCFG_Artikeltabelle_Report
	{
		public List<Liste_50_FG_Artikel_Selectione_Model_Details> Details { get; set; }
		public List<PSZ_CCFG_Artikeltabelle_Header> Header { get; set; }
	}
}
