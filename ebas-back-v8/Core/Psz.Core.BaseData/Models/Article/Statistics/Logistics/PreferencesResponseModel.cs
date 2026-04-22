namespace Psz.Core.BaseData.Models.Article.Statistics.Logistics
{
	public class PreferencesResponseModel
	{
		public string DescriptionBauteils { get; set; }
		public string Designation1 { get; set; }
		public string ArticleNumber { get; set; }
		public string Position { get; set; }
		public string Number { get; set; }
		public string PurchasePrice { get; set; }
		public string Name { get; set; }
		public string PurchasePricingGroup { get; set; }
		public string ArticleNumberBOM { get; set; }
		public string OriginCountry { get; set; }
		public string Preference { get; set; }
		public string StandardSupplier { get; set; }
		public string TotalEK { get; set; }
		public string WennK { get; set; }
		public string SummevonBetrag { get; set; }
		public PreferencesResponseModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.LogisticsPreferences logisticsPreferences)
		{
			if(logisticsPreferences == null)
				return;

			DescriptionBauteils = logisticsPreferences.DescriptionBauteils;
			Designation1 = logisticsPreferences.Designation1;
			ArticleNumber = logisticsPreferences.ArticleNumber;
			Position = logisticsPreferences.Position;
			Number = logisticsPreferences.Number;
			PurchasePrice = logisticsPreferences.PurchasePrice;
			Name = logisticsPreferences.Name;
			PurchasePricingGroup = logisticsPreferences.PurchasePricingGroup;
			ArticleNumberBOM = logisticsPreferences.ArticleNumberBOM;
			OriginCountry = logisticsPreferences.OriginCountry;
			Preference = logisticsPreferences.Preference;
			StandardSupplier = logisticsPreferences.StandardSupplier;
			TotalEK = logisticsPreferences.TotalEK;
			WennK = logisticsPreferences.WennK;
			SummevonBetrag = logisticsPreferences.SummevonBetrag;
		}
	}
}
