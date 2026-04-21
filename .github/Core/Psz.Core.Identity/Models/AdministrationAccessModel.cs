using System.Collections.Generic;

namespace Psz.Core.Identity.Models
{
	public class AdministrationAccessModel
	{
		public bool? ModuleActivated { get; set; } = false;
		public AdministrationAccessModel()
		{

		}
		public AdministrationAccessModel(Infrastructure.Data.Entities.Tables.STG.AccessProfileEntity entity)
		{
			ModuleActivated = entity?.ModuleActivated ?? false;
		}
		public AdministrationAccessModel(List<Infrastructure.Data.Entities.Tables.STG.AccessProfileEntity> entities)
		{
			ModuleActivated = false;
			if(entities != null && entities.Count > 0)
			{
				foreach(var entity in entities)
				{
					ModuleActivated = (ModuleActivated ?? false) || (entity?.ModuleActivated ?? false);
				}
			}
		}
		public Infrastructure.Data.Entities.Tables.STG.AccessProfileEntity ToDbEntity(int id, int mainId)
		{
			return new Infrastructure.Data.Entities.Tables.STG.AccessProfileEntity
			{
				Id = id,
				MainAccessProfileId = mainId,
				ModuleActivated = ModuleActivated ?? false,
				AccessProfiles = ModuleActivated ?? false,
				SuperAdministrator = ModuleActivated ?? false,
				Users = ModuleActivated ?? false,
			};
		}
	}
}
