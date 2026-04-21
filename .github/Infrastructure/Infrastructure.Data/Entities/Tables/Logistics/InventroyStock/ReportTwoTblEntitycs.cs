namespace Infrastructure.Data.Entities.Tables.Logistics.InventroyStock
{
	public class ReportTwoTblEntity
	{
		public int ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public decimal GefundeneMengeInProduktion { get; set; }
		public int Id { get; set; }
		public decimal Lagerbestand { get; set; }
		public decimal MengeInProduktion { get; set; }
		public string RueckbuchungBestaetigt { get; set; }
		public DateTime? InventoryYear { get; set; }
		public int LagerId { get; set; }

		public ReportTwoTblEntity() { }

		public ReportTwoTblEntity(DataRow dataRow)
		{
			ArtikelNr = Convert.ToInt32(dataRow["ArtikelNr"]);
			Artikelnummer = Convert.ToString(dataRow["Artikelnummer"]);
			GefundeneMengeInProduktion = Convert.ToDecimal(dataRow["GefundeneMengeInProduktion"]);
			Lagerbestand = Convert.ToDecimal(dataRow["Lagerbestand"]);
			MengeInProduktion = Convert.ToDecimal(dataRow["MengeInProduktion"]);
			RueckbuchungBestaetigt = Convert.ToString(dataRow["RueckbuchungBestaetigt"]);
			InventoryYear = (dataRow["InventoryYear"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["InventoryYear"]);
			LagerId = Convert.ToInt32(dataRow["LagerId"]);
		}

		public ReportTwoTblEntity ShallowClone()
		{
			return new ReportTwoTblEntity
			{
				ArtikelNr = ArtikelNr,
				Artikelnummer = Artikelnummer,
				GefundeneMengeInProduktion = GefundeneMengeInProduktion,
				InventoryYear = InventoryYear,
				Lagerbestand = Lagerbestand,
				LagerId = LagerId,
				MengeInProduktion = MengeInProduktion,
				RueckbuchungBestaetigt = RueckbuchungBestaetigt
			};
		}
	}
}
