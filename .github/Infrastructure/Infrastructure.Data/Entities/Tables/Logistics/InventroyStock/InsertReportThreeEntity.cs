
namespace Infrastructure.Data.Entities.Tables.Logistics.InventroyStock
{
	public class InsertReportThreeEntity
	{
		public int ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public decimal BedarfFa { get; set; }
		public int Id { get; set; }
		public decimal MengeInProduktion { get; set; }
		public string RueckbuchungBestaetigt { get; set; }
		public decimal UeberschussInProduktion { get; set; }

		public InsertReportThreeEntity() { }

		public InsertReportThreeEntity(DataRow dataRow)
		{
			ArtikelNr = Convert.ToInt32(dataRow["ArtikelNr"]);
			Artikelnummer = Convert.ToString(dataRow["Artikelnummer"]);
			BedarfFa = Convert.ToDecimal(dataRow["BedarfFa"]);
			MengeInProduktion = Convert.ToDecimal(dataRow["MengeInProduktion"]);
			RueckbuchungBestaetigt = Convert.ToString(dataRow["RueckbuchungBestaetigt"]);
			UeberschussInProduktion = Convert.ToDecimal(dataRow["UeberschussInProduktion"]);
		}

		public InsertReportThreeEntity ShallowClone()
		{
			return new InsertReportThreeEntity
			{
				ArtikelNr = ArtikelNr,
				Artikelnummer = Artikelnummer,
				BedarfFa = BedarfFa,
				MengeInProduktion = MengeInProduktion,
				RueckbuchungBestaetigt = RueckbuchungBestaetigt,
				UeberschussInProduktion = UeberschussInProduktion,
			};
		}
	}
}
