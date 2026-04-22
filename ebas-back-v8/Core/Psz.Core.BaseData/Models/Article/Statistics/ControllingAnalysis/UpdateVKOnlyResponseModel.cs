namespace Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis
{
	public class UpdateVKOnlyResponseModel
	{
		public string Artikelnummer { get; set; }
		public decimal? Staffelpreis1 { get; set; }
		public decimal? Staffelpreis2 { get; set; }
		public decimal? Staffelpreis3 { get; set; }
		public decimal? Staffelpreis4 { get; set; }
		public decimal? VK { get; set; }
		public decimal? VK_Simulation { get; set; }
		public UpdateVKOnlyResponseModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_VKSimulationStffelPreis entity)
		{
			if(entity == null)
				return;

			Artikelnummer = entity.Artikelnummer;
			Staffelpreis1 = entity.Staffelpreis1;
			Staffelpreis2 = entity.Staffelpreis2;
			Staffelpreis3 = entity.Staffelpreis3;
			Staffelpreis4 = entity.Staffelpreis4;
			VK = entity.VK;
			VK_Simulation = entity.VK_Simulation;
		}
	}
}
