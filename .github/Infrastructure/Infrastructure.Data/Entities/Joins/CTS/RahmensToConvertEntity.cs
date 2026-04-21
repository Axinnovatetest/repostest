using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class RahmensToConvertEntity
	{
		public string Artikelnummer { get; set; }
		public int? Artikel_Nr { get; set; }
		public bool? Rahmen { get; set; }
		public string Rahmen_Nr { get; set; }
		public decimal? Rahmenmenge { get; set; }
		public DateTime? Rahmenauslauf { get; set; }
		public bool? Rahmen2 { get; set; }
		public string Rahmen_Nr2 { get; set; }
		public decimal? Rahmenmenge2 { get; set; }
		public DateTime? Rahmenauslauf2 { get; set; }
		public decimal? Einkaufspreis { get; set; }
		public string Name1 { get; set; }

		public RahmensToConvertEntity(DataRow dataRow)
		{
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Artikel_Nr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Rahmen = (dataRow["Rahmen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Rahmen"]);
			Rahmen_Nr = (dataRow["Rahmen-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rahmen-Nr"]);
			Rahmenmenge = (dataRow["Rahmenmenge"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Rahmenmenge"]);
			Rahmenauslauf = (dataRow["Rahmenauslauf"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Rahmenauslauf"]);
			Rahmen2 = (dataRow["Rahmen2"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Rahmen2"]);
			Rahmen_Nr2 = (dataRow["Rahmen-Nr2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rahmen-Nr2"]);
			Rahmenmenge2 = (dataRow["Rahmenmenge2"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Rahmenmenge2"]);
			Rahmenauslauf2 = (dataRow["Rahmenauslauf2"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Rahmenauslauf2"]);
			Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufspreis"]);
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);

		}
	}
}
