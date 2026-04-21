using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class VDAEntity
	{
		public string artikelnummer { get; set; }
		public string bezeichnung1 { get; set; }
		public string bezeichnung2 { get; set; }
		public string customerItemNumber { get; set; }
		public decimal groesse { get; set; }
		public string lVornameNameFirma { get; set; }
		public string lStrassePostfach { get; set; }
		public string lLandPLZORT { get; set; }
		public string verpackungsart { get; set; }
		public int? verpackungsmenge { get; set; }
		public string abladestelle { get; set; }
		public bool packstatus { get; set; }
		public DateTime? liefertermin { get; set; }
		public int anzahl { get; set; }
		public string bezug { get; set; }
		public string ihrZeichen { get; set; }
		public long angeboteNr { get; set; }
		public string index_Kunde { get; set; }
		public bool versand_gedruckt { get; set; }
		public long nrAngeboteArtikel { get; set; }
		public bool vDAGedruckt { get; set; }
		public VDAEntity()
		{

		}
		public VDAEntity(DataRow dataRow)
		{
			artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			bezeichnung1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			bezeichnung2 = (dataRow["Bezeichnung 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 2"]);
			customerItemNumber= (dataRow["customerItemNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["customerItemNumber"]);
			groesse = (dataRow["grosse"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["grosse"]);
			lVornameNameFirma = (dataRow["LVorname/NameFirma"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LVorname/NameFirma"]);
			lStrassePostfach = (dataRow["LStraße/Postfach"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LStraße/Postfach"]);
			lLandPLZORT = (dataRow["LLand/PLZ/Ort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LLand/PLZ/Ort"]);
			verpackungsart = (dataRow["Verpackungsart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Verpackungsart"]);
			verpackungsmenge = (dataRow["Verpackungsmenge"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Verpackungsmenge"]);
			abladestelle = (dataRow["Abladestelle"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Abladestelle"]);
			packstatus = (dataRow["Packstatus"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["Packstatus"]);
			liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
			anzahl = (dataRow["Fuellmenge"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Fuellmenge"]);
			bezug = (dataRow["Bezug"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezug"]);
			ihrZeichen = (dataRow["Ihr Zeichen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ihr Zeichen"]);
			angeboteNr = (dataRow["Angebot-Nr"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dataRow["Angebot-Nr"]);
			index_Kunde = (dataRow["Index_Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Index_Kunde"]);
			versand_gedruckt = (dataRow["Versand_gedruckt"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["Versand_gedruckt"]);
			nrAngeboteArtikel = (dataRow["Nr"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dataRow["Nr"]);
			vDAGedruckt = (dataRow["VDA_gedruckt"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["VDA_gedruckt"]);

		}
	}
}
