using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.STG
{
	public class AccessProfileEntity
	{
		public bool AccessProfiles { get; set; }
		public bool AccessProfilesCreate { get; set; }
		public bool AccessProfilesDelete { get; set; }
		public bool AccessProfilesUpdate { get; set; }
		public int Id { get; set; }
		public int MainAccessProfileId { get; set; }
		public bool ModuleActivated { get; set; }
		public bool Users { get; set; }
		public bool UsersCreate { get; set; }
		public bool UsersDelete { get; set; }
		public bool UsersUpdate { get; set; }
		public bool SuperAdministrator { get; set; }

		public AccessProfileEntity() { }
		public AccessProfileEntity(DataRow dataRow)
		{
			AccessProfiles = Convert.ToBoolean(dataRow["AccessProfiles"]);
			AccessProfilesCreate = Convert.ToBoolean(dataRow["AccessProfilesCreate"]);
			AccessProfilesDelete = Convert.ToBoolean(dataRow["AccessProfilesDelete"]);
			AccessProfilesUpdate = Convert.ToBoolean(dataRow["AccessProfilesUpdate"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			MainAccessProfileId = Convert.ToInt32(dataRow["MainAccessProfileId"]);
			ModuleActivated = Convert.ToBoolean(dataRow["ModuleActivated"]);
			Users = Convert.ToBoolean(dataRow["Users"]);
			UsersCreate = Convert.ToBoolean(dataRow["UsersCreate"]);
			UsersDelete = Convert.ToBoolean(dataRow["UsersDelete"]);
			UsersUpdate = Convert.ToBoolean(dataRow["UsersUpdate"]);
			SuperAdministrator = Convert.ToBoolean(dataRow["SuperAdministrator"]);
		}
	}
}

