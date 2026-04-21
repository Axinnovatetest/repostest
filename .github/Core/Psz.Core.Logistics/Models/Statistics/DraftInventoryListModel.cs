using System;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class DraftInventoryListModel
	{
		public DraftInventoryListModel(Infrastructure.Data.Entities.Joins.Logistics.DraftInventoryListEntity DraftInventoryListEntity)
		{
			if(DraftInventoryListEntity == null)
			{
				return;
			}
			ArtikelNr = DraftInventoryListEntity.ArtikelNr;
			Artikelnummer = DraftInventoryListEntity.Artikelnummer;
			Bezeichnung1 = DraftInventoryListEntity.Bezeichnung1;
			StorageID = DraftInventoryListEntity.StorageID;
			QuantityP3000 = DraftInventoryListEntity.QuantityP3000;
			InventurQuantity = DraftInventoryListEntity.InventurQuantity;
			InventurQuantity = DraftInventoryListEntity.InventurQuantity;
			Difference = DraftInventoryListEntity.Difference;
			letzteBewegung = DraftInventoryListEntity.letzteBewegung;
			CCID_Datum = DraftInventoryListEntity.CCID_Datum;
			totalRows = DraftInventoryListEntity.totalRows;

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
