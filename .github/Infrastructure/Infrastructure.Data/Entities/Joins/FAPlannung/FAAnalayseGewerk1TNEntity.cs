using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FAPlannung
{
	public class FAAnalayseGewerk1TNEntity
	{
		public int? Fertigungsnummer { get; set; }
		public string Freigabestatus { get; set; }
		public Decimal? Gesamt_Menge { get; set; }
		public string Artikelnummer { get; set; }
		public string ROH_Artikelnummer { get; set; }
		public Decimal? Bedarf { get; set; }
		public Decimal? In_P3000 { get; set; }
		public Decimal? Ve_skladu { get; set; }
		public Decimal? Ve_vyrobe { get; set; }
		public DateTime? Termin_Schneiderei { get; set; }
		public string Material { get; set; }
		public string Typ_Material { get; set; }
		public string Gewerk_1 { get; set; }
		public string Gewerk_2 { get; set; }
		public string Gewerk_3 { get; set; }
		public Decimal? SummevonBestand { get; set; }
		public Decimal? SummevonBedarf { get; set; }
		public DateTime? FA_begonnen { get; set; }
		public Decimal RestBestand { get; set; }

		public FAAnalayseGewerk1TNEntity(DataRow dataRow)
		{
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Freigabestatus = (dataRow["Freigabestatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus"]);
			Gesamt_Menge = (dataRow["Gesamt Menge"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Gesamt Menge"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			ROH_Artikelnummer = (dataRow["ROH Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ROH Artikelnummer"]);
			Bedarf = (dataRow["Bedarf"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Bedarf"]);
			In_P3000 = (dataRow["In P3000"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["In P3000"]);
			Ve_skladu = (dataRow["Ve_skladu"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Ve_skladu"]);
			Ve_vyrobe = (dataRow["Ve_vyrobe"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Ve_vyrobe"]);
			Termin_Schneiderei = (dataRow["Termin_Schneiderei"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Schneiderei"]);
			Material = (dataRow["Material"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Material"]);
			Typ_Material = (dataRow["Typ_Material"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ_Material"]);
			Gewerk_1 = (dataRow["Gewerk 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gewerk 1"]);
			Gewerk_2 = (dataRow["Gewerk 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gewerk 2"]);
			Gewerk_3 = (dataRow["Gewerk 3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gewerk 3"]);
			SummevonBestand = (dataRow["SummevonBestand"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["SummevonBestand"]);
			SummevonBedarf = (dataRow["SummevonBedarf"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["SummevonBedarf"]);
			FA_begonnen = (dataRow["FA_begonnen"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["FA_begonnen"]);
		}
	}
}
