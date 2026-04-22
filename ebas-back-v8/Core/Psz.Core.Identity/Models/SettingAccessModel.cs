using System.Collections.Generic;

namespace Psz.Core.Identity.Models
{
	public class SettingsAccessModel
	{
		public bool ModuleActivated { get; set; } = false;

		public bool AccessProfiles { get; set; } = false;
		public bool AccessProfilesCreate { get; set; } = false;
		public bool AccessProfilesDelete { get; set; } = false;
		public bool AccessProfilesUpdate { get; set; } = false;

		public bool Users { get; set; } = false;
		public bool UsersCreate { get; set; } = false;
		public bool UsersDelete { get; set; } = false;
		public bool UsersUpdate { get; set; } = false;
		public bool SuperAdministrator { get; set; } = false;
		public SettingsAccessModel() { }
		public SettingsAccessModel(List<Infrastructure.Data.Entities.Tables.STG.AccessProfileEntity> settingsAccessProfileDb)
		{
			if(settingsAccessProfileDb == null || settingsAccessProfileDb.Count <= 0)
				return;

			foreach(var accessItem in settingsAccessProfileDb)
			{
				ModuleActivated = ModuleActivated || accessItem.ModuleActivated;

				AccessProfiles = AccessProfiles || accessItem.AccessProfiles;
				AccessProfilesCreate = AccessProfilesCreate || accessItem.AccessProfilesUpdate; // settingsAccessProfileDb.AccessProfilesCreate;
				AccessProfilesDelete = AccessProfilesDelete || accessItem.AccessProfilesUpdate; //settingsAccessProfileDb.AccessProfilesDelete;
				AccessProfilesUpdate = AccessProfilesUpdate || accessItem.AccessProfilesUpdate;

				Users = Users || accessItem.Users;
				UsersCreate = UsersCreate || accessItem.UsersUpdate; // settingsAccessProfileDb.UsersCreate;
				UsersDelete = UsersDelete || accessItem.UsersUpdate; //settingsAccessProfileDb.UsersDelete;
				UsersUpdate = UsersUpdate || accessItem.UsersUpdate;

				SuperAdministrator = SuperAdministrator || accessItem.SuperAdministrator;
			}
		}

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
				SuperAdministrator = this.SuperAdministrator
			};
		}
	}
	public class SettingsAccessMinimalModel
	{
		public bool ModuleActivated { get; set; } = true;

		public bool AccessProfiles { get; set; } = false;

		public bool Users { get; set; } = false;
		public bool SuperAdministrator { get; set; } = false;
		public SettingsAccessMinimalModel() { }
		public SettingsAccessMinimalModel(Infrastructure.Data.Entities.Tables.STG.AccessProfileEntity settingsAccessProfileDb)
		{
			if(settingsAccessProfileDb == null)
				return;

			ModuleActivated = settingsAccessProfileDb.ModuleActivated;
			AccessProfiles = settingsAccessProfileDb.AccessProfiles;
			Users = settingsAccessProfileDb.Users;
			SuperAdministrator = settingsAccessProfileDb.SuperAdministrator;
		}
		public SettingsAccessMinimalModel(SettingsAccessModel settingsAccessProfileDb)
		{
			if(settingsAccessProfileDb == null)
				return;

			ModuleActivated = settingsAccessProfileDb.ModuleActivated;
			AccessProfiles = settingsAccessProfileDb.AccessProfiles;
			Users = settingsAccessProfileDb.Users;
			SuperAdministrator = settingsAccessProfileDb.SuperAdministrator;
		}

		public Infrastructure.Data.Entities.Tables.STG.AccessProfileEntity ToDbEntity(int id, int mainAccessProfileId)
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
				AccessProfilesCreate = true,
				AccessProfilesDelete = true,
				AccessProfilesUpdate = true,

				Users = this.Users,
				UsersCreate = true,
				UsersDelete = true,
				UsersUpdate = true,
				SuperAdministrator = this.SuperAdministrator
			};
		}
	}
}
