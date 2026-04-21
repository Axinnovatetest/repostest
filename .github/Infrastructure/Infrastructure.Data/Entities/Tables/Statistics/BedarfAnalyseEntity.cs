using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.Statistics
{
	public class BedarfAnalyseEntity
	{
		public string Artikelnummer { get; set; }
		public string Bestell_Nr { get; set; }
		public decimal? DiffPrice { get; set; }
		public decimal? DiffQuantity { get; set; }
		public decimal? Einkaufspreis { get; set; }
		public decimal? Gesamtpreis { get; set; }
		public string Name1 { get; set; }
		public decimal? ROH_Bestand { get; set; }
		public decimal? ROH_Quantity { get; set; }
		public decimal? Wert_LagerBestandBedarf { get; set; }
		public DateTime? LastSyncDate { get; set; }
		public int? TotalCount { get; set; }

		public BedarfAnalyseEntity() { }

		public BedarfAnalyseEntity(DataRow dataRow)
		{
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bestell_Nr = (dataRow["Bestell_Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestell_Nr"]);
			DiffPrice = (dataRow["DiffPrice"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["DiffPrice"]);
			DiffQuantity = (dataRow["DiffQuantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["DiffQuantity"]);
			Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufspreis"]);
			Gesamtpreis = (dataRow["Gesamtpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gesamtpreis"]);
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
			ROH_Bestand = (dataRow["ROH_Bestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ROH_Bestand"]);
			ROH_Quantity = (dataRow["ROH_Quantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ROH_Quantity"]);
			Wert_LagerBestandBedarf = (dataRow["Wert_LagerBestandBedarf"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Wert_LagerBestandBedarf"]);
			LastSyncDate = (dataRow["LastSyncDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastSyncDate"]);
			TotalCount = (dataRow["TotalCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TotalCount"]);
		}

		public BedarfAnalyseEntity ShallowClone()
		{
			return new BedarfAnalyseEntity
			{
				Artikelnummer = Artikelnummer,
				Bestell_Nr = Bestell_Nr,
				DiffPrice = DiffPrice,
				DiffQuantity = DiffQuantity,
				Einkaufspreis = Einkaufspreis,
				Gesamtpreis = Gesamtpreis,
				Name1 = Name1,
				ROH_Bestand = ROH_Bestand,
				ROH_Quantity = ROH_Quantity,
				Wert_LagerBestandBedarf = Wert_LagerBestandBedarf,
				LastSyncDate = LastSyncDate
			};
		}
	}
}