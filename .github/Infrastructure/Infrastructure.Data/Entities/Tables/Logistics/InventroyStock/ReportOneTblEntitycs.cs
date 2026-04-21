namespace Infrastructure.Data.Entities.Tables.Logistics.InventroyStock
{
	public class ReportOneTblEntity
	{
		public int ArtikelNr { get; set; }
		public int FertigungId { get; set; }
		public string Artikelnummer { get; set; }
		public string FaGeschnitten { get; set; }
		public string FaKommisioniert { get; set; }
		public DateTime? FaTermin { get; set; }
		public string Fertigungsnummer { get; set; }
		public int Id { get; set; }
		public DateTime? InventoryYear { get; set; }
		public int LagerId { get; set; }
		public decimal OffeneMenge { get; set; }

		public ReportOneTblEntity() { }

		public ReportOneTblEntity(DataRow dataRow)
		{
			//Id = Convert.ToInt32(dataRow["Id"]);
			ArtikelNr = Convert.ToInt32(dataRow["ArtikelNr"]);
			FertigungId = Convert.ToInt32(dataRow["FertigungId"]);
			Artikelnummer = Convert.ToString(dataRow["Artikelnummer"]);
			FaGeschnitten = Convert.ToString(dataRow["FaGeschnitten"]);
			FaKommisioniert = Convert.ToString(dataRow["FaKommisioniert"]);
			FaTermin = (dataRow["FaTermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["FaTermin"]);
			Fertigungsnummer = Convert.ToString(dataRow["Fertigungsnummer"]);
			InventoryYear = (dataRow["InventoryYear"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["InventoryYear"]);
			LagerId = Convert.ToInt32(dataRow["LagerId"]);
			OffeneMenge = Convert.ToDecimal(dataRow["OffeneMenge"]);
		}

		public ReportOneTblEntity ShallowClone()
		{
			return new ReportOneTblEntity
			{
				ArtikelNr = ArtikelNr,
				Artikelnummer = Artikelnummer,
				FaGeschnitten = FaGeschnitten,
				FaKommisioniert = FaKommisioniert,
				FaTermin = FaTermin,
				Fertigungsnummer = Fertigungsnummer,
				Id = Id,
				InventoryYear = InventoryYear,
				LagerId = LagerId,
				OffeneMenge = OffeneMenge,
				FertigungId= FertigungId
			};
		}
	}
}
