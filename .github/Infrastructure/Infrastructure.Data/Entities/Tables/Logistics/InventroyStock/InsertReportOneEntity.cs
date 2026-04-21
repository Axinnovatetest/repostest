
namespace Infrastructure.Data.Entities.Tables.Logistics.InventroyStock
{
	public class InsertReportOneEntity
	{
		public int ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public string FaGeschnitten { get; set; }
		public string FaKommisioniert { get; set; }
		public DateTime? FaTermin { get; set; }
		public string Fertigungsnummer { get; set; }
		public DateTime? InventoryYear { get; set; }
		public int LagerId { get; set; }
		public decimal OffeneMenge { get; set; }

		public InsertReportOneEntity() { }

		public InsertReportOneEntity(DataRow dataRow)
		{
			ArtikelNr = Convert.ToInt32(dataRow["ArtikelNr"]);
			Artikelnummer = Convert.ToString(dataRow["Artikelnummer"]);
			FaGeschnitten = Convert.ToString(dataRow["FaGeschnitten"]);
			FaKommisioniert = Convert.ToString(dataRow["FaKommisioniert"]);
			FaTermin = (dataRow["FaTermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["FaTermin"]);
			Fertigungsnummer = Convert.ToString(dataRow["Fertigungsnummer"]);
			OffeneMenge = Convert.ToDecimal(dataRow["OffeneMenge"]);
			InventoryYear = (dataRow["InventoryYear"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["InventoryYear"]);
			LagerId = Convert.ToInt32(dataRow["LagerId"]);
		}

		public InsertReportOneEntity ShallowClone()
		{
			return new InsertReportOneEntity
			{
				ArtikelNr = ArtikelNr,
				Artikelnummer = Artikelnummer,
				FaGeschnitten = FaGeschnitten,
				FaKommisioniert = FaKommisioniert,
				FaTermin = FaTermin,
				Fertigungsnummer = Fertigungsnummer,
				OffeneMenge = OffeneMenge,
				InventoryYear = InventoryYear,
			};
		}
	}
}
