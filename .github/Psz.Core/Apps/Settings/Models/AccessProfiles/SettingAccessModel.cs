namespace Psz.Core.Apps.Settings.Models.AccessProfiles
{
	public class SettingsAccessModel
	{
		public bool ModuleActivated { get; set; }

		public bool AccessProfiles { get; set; }
		public bool AccessProfilesCreate { get; set; }
		public bool AccessProfilesDelete { get; set; }
		public bool AccessProfilesUpdate { get; set; }

		public bool Users { get; set; }
		public bool UsersCreate { get; set; }
		public bool UsersDelete { get; set; }
		public bool UsersUpdate { get; set; }
		public bool SuperAdministrator { get; set; }

		internal Infrastructure.Data.Entities.Tables.STG.AccessProfileEntity ToDbEntity(int id, int mainAccessProfileId)
		{
			if(!this.ModuleActivated)
			{
				return new Infrastructure.Data.Entities.Tables.STG.AccessProfileEntity()
				{
					Id = id,
					MainAccessProfileId = mainAccessProfileId,
					ModuleActivated = false,
				};
			}

			return new Infrastructure.Data.Entities.Tables.STG.AccessProfileEntity()
			{
				Id = id,
				MainAccessProfileId = mainAccessProfileId,
				ModuleActivated = true,

				AccessProfiles = this.AccessProfiles,
				AccessProfilesCreate = this.AccessProfilesCreate,
				AccessProfilesDelete = this.AccessProfilesDelete,
				AccessProfilesUpdate = this.AccessProfilesUpdate,

				Users = this.Users,
				UsersCreate = this.UsersCreate,
				UsersDelete = this.UsersDelete,
				UsersUpdate = this.UsersUpdate,
			};
		}

		internal void DenyAll()
		{
			ModuleActivated = false;

			AccessProfiles = false;
			AccessProfilesCreate = false;
			AccessProfilesDelete = false;
			AccessProfilesUpdate = false;

			Users = false;
			UsersCreate = false;
			UsersDelete = false;
			UsersUpdate = false;
		}
	}
}
