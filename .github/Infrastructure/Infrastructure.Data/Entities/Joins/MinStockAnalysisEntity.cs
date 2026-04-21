namespace Infrastructure.Data.Entities.Joins
{
	public class MinStockAnalysisEntity
	{
		public int ArticleNr { get; set; }

		public string ArticleNumber { get; set; }

		public int Mindesbestand { get; set; }

		public int Bestand { get; set; }

		public decimal? Einzelpreis_VK_CU150 { get; set; }

		public decimal? Mindestbestandgesamtpreis_VK_CU150 { get; set; }

		public decimal? Einzelpreis_VK_PSZ_ink_Kupfer { get; set; }
		public decimal? Mindestbestandgesamtpreis_VK_PSZ_ink_Kupfer { get; set; }

		public decimal? Einzelpreis_VK_CU150_Herstellkosten { get; set; }
		public decimal? Enizelpreis_VK_CU_DEL_Herstellkosten { get; set; }
		public decimal? GesamtPreis_Herstellerkosten_CU_150 { get; set; }
		public decimal? GesamtPreis_Herstellerkosten_CU_Del { get; set; }
		public int TotalCount { get; set; }


		public MinStockAnalysisEntity(DataRow dataRow)
		{
			if(dataRow == null)
				return;

			ArticleNr = (dataRow["ArticleNr"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["ArticleNr"]);

			ArticleNumber = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);

			Mindesbestand = (dataRow["Mindestbestand"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Mindestbestand"]);

			Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Bestand"]);

			Einzelpreis_VK_CU150 = (dataRow["Vkmitcu150Einzelpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Vkmitcu150Einzelpreis"]);

			Mindestbestandgesamtpreis_VK_CU150 = (dataRow["Vkmitcu150MindestbestandGesamtpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Vkmitcu150MindestbestandGesamtpreis"]);

			Einzelpreis_VK_PSZ_ink_Kupfer = (dataRow["VkmitcuEinzelpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VkmitcuEinzelpreis"]);

			Mindestbestandgesamtpreis_VK_PSZ_ink_Kupfer = (dataRow["VkmitcuMindestbestandGesamtpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VkmitcuMindestbestandGesamtpreis"]);
			Einzelpreis_VK_CU150_Herstellkosten = (dataRow["Vkcu150Herstellkosten"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Vkcu150Herstellkosten"]);
			Enizelpreis_VK_CU_DEL_Herstellkosten = (dataRow["VkCuDelHerstellkosten"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VkCuDelHerstellkosten"]);
			GesamtPreis_Herstellerkosten_CU_150 = (dataRow["GesamtpreisHerstellkosten150"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["GesamtpreisHerstellkosten150"]);
			GesamtPreis_Herstellerkosten_CU_Del = (dataRow["GesamtpreisHerstellkostenDel"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["GesamtpreisHerstellkostenDel"]);
			TotalCount = Convert.ToInt32(dataRow["TotalCount"]);

		}
	}
}
