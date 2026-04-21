namespace Infrastructure.Services.Reporting.Models.Logistics
{
	public class LSDruckDetailsReportModel
	{
		public string artikelnummer { get; set; }
		public int position { get; set; }
		public string bezeichnung1 { get; set; }
		public string bezeichnung1_b { get; set; }
		public string bezeichnung2 { get; set; }
		public string indexKundeDatum { get; set; }
		public string indexKunde { get; set; }
		public string ursprungsland { get; set; }
		public string zolltarifNummer { get; set; }
		public decimal grosse { get; set; }
		public string einheit { get; set; }
		public decimal gesammtGewicht { get; set; }
		public decimal anzahl { get; set; }
		public string posText { get; set; }

		public LSDruckDetailsReportModel()
		{

		}
		public LSDruckDetailsReportModel(Infrastructure.Data.Entities.Joins.Logistics.LSDetailsEntity lsDetailsEntity)
		{
			artikelnummer = lsDetailsEntity.artikelnummer.Substring(0, 3).ToUpper() == "ALB" || lsDetailsEntity.artikelnummer.Substring(0, 4).ToUpper() == "SELT"
							|| lsDetailsEntity.artikelnummer.Substring(lsDetailsEntity.artikelnummer.Length - 2, 2).ToUpper() == "NS"
							|| lsDetailsEntity.artikelnummer.Substring(lsDetailsEntity.artikelnummer.Length - 4, 4).ToUpper() == "NSTN"
							? lsDetailsEntity.artikelnummer : lsDetailsEntity.artikelnummer.Length >= 10 ? lsDetailsEntity.artikelnummer.Substring(0, 10) : lsDetailsEntity.artikelnummer.Substring(0, lsDetailsEntity.artikelnummer.Length);


			position = lsDetailsEntity.position;
			bezeichnung1 = lsDetailsEntity.bezeichnung1;
			bezeichnung1_b = bezeichnung1.Length > 8 ? bezeichnung1.Substring(0, 8) : bezeichnung1;
			bezeichnung2 = lsDetailsEntity.bezeichnung2;
			indexKundeDatum = lsDetailsEntity.indexKundeDatum != null ? lsDetailsEntity.indexKundeDatum.Value.ToString("dd-MM-yyyy") : "";
			indexKunde = lsDetailsEntity.indexKunde;
			ursprungsland = lsDetailsEntity.ursprungsland;
			zolltarifNummer = lsDetailsEntity.zolltarifNummer;
			grosse = lsDetailsEntity.grosse / 1000;
			einheit = lsDetailsEntity.einheit;
			gesammtGewicht = lsDetailsEntity.gesammtGewicht;
			anzahl = lsDetailsEntity.anzahl;
			posText = lsDetailsEntity.posText;
		}
	}
}
