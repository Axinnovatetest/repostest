using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class DraftInventoryListEntity
	{
		public DraftInventoryListEntity(DataRow datarow)
		{
			ArtikelNr = (datarow["Artikel-Nr"] == DBNull.Value) ? 0 : Convert.ToInt32(datarow["Artikel-Nr"]);
			Artikelnummer = (datarow["Artikelnummer"] == DBNull.Value) ? "" : Convert.ToString(datarow["Artikelnummer"]);
			Bezeichnung1 = (datarow["Bezeichnung 1"] == DBNull.Value) ? "" : Convert.ToString(datarow["Bezeichnung 1"]);
			StorageID = (datarow["Storage-ID"] == DBNull.Value) ? 0 : Convert.ToInt32(datarow["Storage-ID"]);
			QuantityP3000 = (datarow["Quantity P3000"] == DBNull.Value) ? 0 : Convert.ToDecimal(datarow["Quantity P3000"]);
			InventurQuantity = (datarow["Inventur Quantity"] == DBNull.Value) ? "" : Convert.ToString(datarow["Inventur Quantity"]);
			Difference = (datarow["Difference"] == DBNull.Value) ? "" : Convert.ToString(datarow["Difference"]);
			letzteBewegung = (datarow["letzte Bewegung"] == DBNull.Value) ? null : Convert.ToDateTime(datarow["letzte Bewegung"]);
			CCID_Datum = (datarow["CCID_Datum"] == DBNull.Value) ? null : Convert.ToDateTime(datarow["CCID_Datum"]);
			totalRows = (datarow["totalRows"] == DBNull.Value) ? 0 : Convert.ToInt32(datarow["totalRows"]);

		}
		public int ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public int StorageID { get; set; }
		public decimal QuantityP3000 { get; set; }
		public string InventurQuantity { get; set; }
		public string Difference { get; set; }
		public DateTime? letzteBewegung { get; set; }
		public DateTime? CCID_Datum { get; set; }
		public int totalRows { get; set; }
	}
}
