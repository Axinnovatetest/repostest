namespace Infrastructure.Data.Entities.Tables.Logistics.InventroyStock
{
	public class ReportROHinProductionEntity
	{
		public int ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public int Id { get; set; }
		public int IdSpule { get; set; }
		public DateTime? InventoryYear { get; set; }
		public int LagerId { get; set; }
		public decimal MengeInProduktion { get; set; }
		public string StatusSpule { get; set; }

		public ReportROHinProductionEntity() { }

		public ReportROHinProductionEntity(DataRow dataRow)
		{
			ArtikelNr = Convert.ToInt32(dataRow["ArtikelNr"]);
			Artikelnummer = Convert.ToString(dataRow["Artikelnummer"]);
			//Id = Convert.ToInt32(dataRow["Id"]);
			IdSpule = Convert.ToInt32(dataRow["IdSpule"]);
			InventoryYear = (dataRow["InventoryYear"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["InventoryYear"]);
			LagerId = Convert.ToInt32(dataRow["LagerId"]);
			MengeInProduktion = Convert.ToDecimal(dataRow["MengeInProduktion"]);
			StatusSpule = Convert.ToString(dataRow["StatusSpule"]);
		}

		public ReportROHinProductionEntity ShallowClone()
		{
			return new ReportROHinProductionEntity
			{
				ArtikelNr = ArtikelNr,
				Artikelnummer = Artikelnummer,
				Id = Id,
				IdSpule = IdSpule,
				InventoryYear = InventoryYear,
				LagerId = LagerId,
				MengeInProduktion = MengeInProduktion,
				StatusSpule = StatusSpule
			};
		}
	}
}
