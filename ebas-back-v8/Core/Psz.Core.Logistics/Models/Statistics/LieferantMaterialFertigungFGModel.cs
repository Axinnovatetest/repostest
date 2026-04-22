using System.Collections.Generic;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class LieferantMaterialFertigungFGModel
	{
		public List<LieferantMaterialModel> LieferantMaterialModel { get; set; }
		public List<FertigungFGModel> FertigungFGModel { get; set; }
	}
	public class LieferantMaterialModel
	{
		public LieferantMaterialModel(Infrastructure.Data.Entities.Joins.Logistics.LieferantMaterialEntity LieferantMaterialEntity)
		{
			if(LieferantMaterialEntity == null)
			{
				return;
			}
			Name1 = LieferantMaterialEntity.Name1;
		}
		public string Name1 { get; set; }
	}
	public class FertigungFGModel
	{
		public FertigungFGModel(Infrastructure.Data.Entities.Joins.Logistics.FertigungFGEntity FertigungFGEntity)
		{
			if(FertigungFGEntity == null)
			{
				return;
			}
			Lagerort = FertigungFGEntity.Lagerort;
		}
		public string Lagerort { get; set; }
	}
}
