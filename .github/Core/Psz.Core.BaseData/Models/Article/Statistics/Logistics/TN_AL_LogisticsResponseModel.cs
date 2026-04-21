namespace Psz.Core.BaseData.Models.Article.Statistics.Logistics
{
	public class TN_AL_LogisticsResponseModel
	{
		public string FinishedProduct { get; set; }
		public string Designation2 { get; set; }
		public string Designation1 { get; set; }
		public string CustomsTariff { get; set; }
		public string ExportWeight { get; set; }
		public string MaterialCosts { get; set; }
		public string LaborCosts { get; set; }
		public string PackagingType { get; set; }
		public string PackingQuantity { get; set; }
		public TN_AL_LogisticsResponseModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.Logistics_TN_AL_Logistics logistics_tn_al_logisitics)
		{
			if(logistics_tn_al_logisitics == null)
				return;

			FinishedProduct = logistics_tn_al_logisitics.Fertigprodukt;
			Designation1 = logistics_tn_al_logisitics.Designation1;
			Designation2 = logistics_tn_al_logisitics.Designation2;
			CustomsTariff = logistics_tn_al_logisitics.Zolltarif_nr;
			ExportWeight = logistics_tn_al_logisitics.Exportgewicht;
			MaterialCosts = logistics_tn_al_logisitics.Materialkosten;
			LaborCosts = logistics_tn_al_logisitics.Arbeitskosten;
			PackagingType = logistics_tn_al_logisitics.Verpackungsart;
			PackingQuantity = logistics_tn_al_logisitics.Verpackungsmenge;
		}
	}
}
