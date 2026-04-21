namespace Psz.Core.BaseData.Models.Article
{
	public class ArticleOverviewMinimalModel
	{
		public int ArtikelNr { get; set; }
		//public string ArtikelNummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public string Bezeichnung2 { get; set; }
		public string Bezeichnung3 { get; set; }
		public string Verpackung { get; set; }
		public string Artikelbezeichnung { get; set; }
		public string OrderNumber { get; set; }
		public string Consumption12Months { get; set; }
		public int? Losgroesse { get; set; }
		public decimal? Produktionlosgrosse { get; set; }
		public string Langtext { get; set; }
		public string Lieferzeit { get; set; }
		public string Manufacturer { get; set; }
		public string ManufacturerNumber { get; set; }

		public ArticleOverviewMinimalModel()
		{

		}
		public Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity
			{
				ArtikelNr = this.ArtikelNr,
				//ArtikelNummer = this.ArtikelNummer,
				Bezeichnung1 = this.Bezeichnung1,
				Bezeichnung2 = this.Bezeichnung2,
				Bezeichnung3 = this.Bezeichnung3,
				Verpackung = Verpackung,
				Artikelbezeichnung = Artikelbezeichnung,
				Losgroesse = Losgroesse,
				Produktionlosgrosse = Produktionlosgrosse,
				Langtext = Langtext,
				Lieferzeit = Lieferzeit,
				Manufacturer = Manufacturer,
				ManufacturerNumber = ManufacturerNumber,
			};
		}
		public Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity ToExtensionEntity()
		{
			return new Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity
			{
				ArtikelNr = this.ArtikelNr,
				OrderNumber = this.OrderNumber,
				Consumption12Months = this.Consumption12Months,
			};
		}
	}
}
