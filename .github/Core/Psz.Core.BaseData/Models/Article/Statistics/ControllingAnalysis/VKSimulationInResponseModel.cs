namespace Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis
{
	public class VKSimulationInResponseModel
	{
		public decimal? Anteilberechen { get; set; }
		public int Artikel_Nr { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Bezeichnung_2 { get; set; }
		public int? DEL { get; set; }
		public string Prozent { get; set; }
		public int Update { get; set; }
		public decimal? VK { get; set; }
		public decimal? VK_Simulation { get; set; }
		public decimal? VKCU { get; set; }
		public decimal? VKCU_Simulation { get; set; }

		public VKSimulationInResponseModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_VKSimulationInData entity)
		{
			if(entity == null)
			{
				return;
			}

			Anteilberechen = entity.Anteilberechen;
			Artikel_Nr = entity.Artikel_Nr;
			Artikelnummer = entity.Artikelnummer;
			Bezeichnung_1 = entity.Bezeichnung_1;
			Bezeichnung_2 = entity.Bezeichnung_2;
			DEL = entity.DEL;
			Prozent = entity.Prozent;
			Update = entity.Update;
			VK = entity.VK;
			VK_Simulation = entity.VK_Simulation;
			VKCU = entity.VKCU;
			VKCU_Simulation = entity.VKCU_Simulation;
		}

		public Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_VKSimulationInData ToEntity()
		{
			return new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_VKSimulationInData
			{
				Anteilberechen = Anteilberechen,
				Artikel_Nr = Artikel_Nr,
				Artikelnummer = Artikelnummer,
				Bezeichnung_1 = Bezeichnung_1,
				Bezeichnung_2 = Bezeichnung_2,
				DEL = DEL,
				Prozent = Prozent,
				Update = Update,
				VK = VK,
				VK_Simulation = VK_Simulation,
				VKCU = VKCU,
				VKCU_Simulation = VKCU_Simulation
			};
		}
	}
}
