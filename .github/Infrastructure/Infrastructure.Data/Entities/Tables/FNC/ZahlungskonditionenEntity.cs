using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class ZahlungskonditionenEntity
	{
		public string Bemerkungen { get; set; }
		public bool? Betrag_berechnen { get; set; }
		public int ID { get; set; }
		public double? Nachlaß1 { get; set; }
		public double? Nachlaß2 { get; set; }
		public double? Nachlaß3 { get; set; }
		public int? Tage1 { get; set; }
		public int? Tage2 { get; set; }
		public int? Tage3 { get; set; }
		public string Text11 { get; set; }
		public string Text12 { get; set; }
		public string Text21 { get; set; }
		public string Text22 { get; set; }
		public string Text31 { get; set; }
		public string Text32 { get; set; }

		public ZahlungskonditionenEntity() { }

		public ZahlungskonditionenEntity(DataRow dataRow)
		{
			Bemerkungen = (dataRow["Bemerkungen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkungen"]);
			Betrag_berechnen = (dataRow["Betrag_berechnen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Betrag_berechnen"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Nachlaß1 = (dataRow["Nachlaß1"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Nachlaß1"]);
			Nachlaß2 = (dataRow["Nachlaß2"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Nachlaß2"]);
			Nachlaß3 = (dataRow["Nachlaß3"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Nachlaß3"]);
			Tage1 = (dataRow["Tage1"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Tage1"]);
			Tage2 = (dataRow["Tage2"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Tage2"]);
			Tage3 = (dataRow["Tage3"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Tage3"]);
			Text11 = (dataRow["Text11"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Text11"]);
			Text12 = (dataRow["Text12"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Text12"]);
			Text21 = (dataRow["Text21"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Text21"]);
			Text22 = (dataRow["Text22"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Text22"]);
			Text31 = (dataRow["Text31"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Text31"]);
			Text32 = (dataRow["Text32"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Text32"]);
		}
	}
}

