using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class LagerEntity
	{
		public decimal? AB { get; set; }
		public int? Artikel_Nr { get; set; }
		public decimal? Bestand { get; set; }
		public decimal? Bestand_reserviert { get; set; }
		public bool? Bestellvorschläge { get; set; }
		public decimal? BW { get; set; }
		public bool? CCID { get; set; }
		public DateTime? CCID_Datum { get; set; }
		public int? Dispoformel { get; set; }
		public decimal? Durchschnittspreis { get; set; }
		public decimal? GesamtBestand { get; set; }
		public decimal? Höchstbestand { get; set; }
		public int ID { get; set; }
		public int? Lagerort_id { get; set; }
		public DateTime? letzte_Bewegung { get; set; }
		public decimal? Meldebestand { get; set; }
		public decimal? Mindestbestand { get; set; }
		public decimal? Sollbestand { get; set; }

		public LagerEntity() { }

		public LagerEntity(DataRow dataRow)
		{
			AB = (dataRow["AB"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["AB"]);
			Artikel_Nr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Bestand"]);
			Bestand_reserviert = (dataRow["Bestand_reserviert"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Bestand_reserviert"]);
			Bestellvorschläge = (dataRow["Bestellvorschläge"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Bestellvorschläge"]);
			BW = (dataRow["BW"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["BW"]);
			CCID = (dataRow["CCID"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CCID"]);
			CCID_Datum = (dataRow["CCID_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CCID_Datum"]);
			Dispoformel = (dataRow["Dispoformel"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Dispoformel"]);
			Durchschnittspreis = (dataRow["Durchschnittspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Durchschnittspreis"]);
			GesamtBestand = (dataRow["GesamtBestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["GesamtBestand"]);
			Höchstbestand = (dataRow["Höchstbestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Höchstbestand"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
			letzte_Bewegung = (dataRow["letzte Bewegung"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["letzte Bewegung"]);
			Meldebestand = (dataRow["Meldebestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Meldebestand"]);
			Mindestbestand = (dataRow["Mindestbestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Mindestbestand"]);
			Sollbestand = (dataRow["Sollbestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Sollbestand"]);
		}
	}
}

