using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class LagerBestandEntity
	{
		public int nombreTotal { get; set; }
		public int artikelNr { get; set; }
		public string artikelnummer { get; set; }
		public string bezeichnung1 { get; set; }
		public string bezeichnung2 { get; set; }
		public string lagerort { get; set; }
		public decimal bestand { get; set; }
		public bool cCID { get; set; }
		public DateTime? cCID_Datum { get; set; }
		public int lagerID { get; set; }
		public decimal bestandReserviert { get; set; }
		public decimal gesammtBestand { get; set; }

		public LagerBestandEntity()
		{
		}
		public LagerBestandEntity(DataRow dataRow)
		{

			nombreTotal = (dataRow["NombreTotal"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["NombreTotal"]);
			artikelNr = (dataRow["ArtikelNr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["ArtikelNr"]);
			artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			bezeichnung1 = (dataRow["Bezeichnung1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung1"]);
			bezeichnung2 = (dataRow["Bezeichnung2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung2"]);
			lagerort = (dataRow["lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["lagerort"]);
			bestand = (dataRow["bestand"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["bestand"]);
			cCID = (dataRow["CCID"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["CCID"]);
			cCID_Datum = (dataRow["CCID_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CCID_Datum"]);



		}
		public LagerBestandEntity(DataRow dataRow, int i)
		{

			artikelNr = (dataRow["ArtikelNr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["ArtikelNr"]);
			lagerID = (dataRow["lagerID"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["lagerID"]);
			bestand = (dataRow["Bestand"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Bestand"]);
			bestandReserviert = (dataRow["ReserviertBestand"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["ReserviertBestand"]);
			gesammtBestand = (dataRow["GesamtBestand"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["GesamtBestand"]);
			lagerort = (dataRow["lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["lagerort"]);

		}
	}
}
