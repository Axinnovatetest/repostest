using System.Collections.Generic;

namespace Psz.Core.Identity.Models
{
	public class Administration
	{
		public bool ModuleActivated { get; set; } = false;
		public Administration() { }
		public Administration(List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity> profileEntities)
		{
			if(profileEntities == null || profileEntities.Count <= 0)
				return;

			foreach(var item in profileEntities)
			{
				ModuleActivated = ModuleActivated || item.Administration;
			}
		}
	}
}
