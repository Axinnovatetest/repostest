namespace Psz.Core.Apps.Settings.Models.AccessProfiles
{
	public class WorkPlanAccessModel
	{
		public bool ModuleActivated { get; set; }

		public bool Access { get; set; }
		public bool AccessUpdate { get; set; }

		public bool Country { get; set; }
		public bool CountryCreate { get; set; }
		public bool CountryDelete { get; set; }
		public bool CountryUpdate { get; set; }

		public bool Departement { get; set; }
		public bool DepartementCreate { get; set; }
		public bool DepartementDelete { get; set; }
		public bool DepartementUpdate { get; set; }

		public bool Hall { get; set; }
		public bool HallCreate { get; set; }
		public bool HallDelete { get; set; }
		public bool HallUpdate { get; set; }

		public bool StandardOperation { get; set; }
		public bool StandardOperationCreate { get; set; }
		public bool StandardOperationDelete { get; set; }
		public bool StandardOperationUpdate { get; set; }

		public bool WorkArea { get; set; }
		public bool WorkAreaCreate { get; set; }
		public bool WorkAreaDelete { get; set; }
		public bool WorkAreaUpdate { get; set; }

		public bool WorkPlan { get; set; }
		public bool WorkPlanCreate { get; set; }
		public bool WorkPlanDelete { get; set; }
		public bool WorkPlanUpdate { get; set; }

		public bool WorkPlanReporting { get; set; }
		public bool WorkPlanReportingCreate { get; set; }
		public bool WorkPlanReportingDelete { get; set; }
		public bool WorkPlanReportingUpdate { get; set; }

		public bool WorkStation { get; set; }
		public bool WorkStationCreate { get; set; }
		public bool WorkStationDelete { get; set; }
		public bool WorkStationUpdate { get; set; }

		public bool AdministrationUser { get; set; }
		public bool AdministrationUserUpdate { get; set; }
		public bool AdministrationAccessProfiles { get; set; }
		public bool AdministrationAccessProfilesUpdate { get; set; }
		public bool isDefault { get; set; }
		public bool SuperAdministrator { get; set; }

		internal Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity ToDbEntity(int id, int mainAccessProfileId)
		{
			if(!this.ModuleActivated)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity()
				{
					Id = id,
					MainAccessProfileId = mainAccessProfileId,
					ModuleActivated = false,
				};
			}

			return new Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity()
			{
				Id = id,
				MainAccessProfileId = mainAccessProfileId,
				ModuleActivated = true,

				WorkPlan = this.WorkPlan,
				WorkPlanCreate = this.WorkPlanCreate,
				WorkPlanDelete = this.WorkPlanDelete,
				WorkPlanUpdate = this.WorkPlanUpdate,

				Access = this.Access,
				AccessUpdate = this.AccessUpdate,

				Country = this.Country,
				CountryCreate = this.CountryCreate,
				CountryDelete = this.CountryDelete,
				CountryUpdate = this.CountryUpdate,

				Departement = this.Departement,
				DepartementCreate = this.DepartementCreate,
				DepartementDelete = this.DepartementDelete,
				DepartementUpdate = this.DepartementUpdate,

				Hall = this.Hall,
				HallCreate = this.HallCreate,
				HallDelete = this.HallDelete,
				HallUpdate = this.HallUpdate,

				StandardOperation = this.StandardOperation,
				StandardOperationCreate = this.StandardOperationCreate,
				StandardOperationDelete = this.StandardOperationDelete,
				StandardOperationUpdate = this.StandardOperationUpdate,

				WorkArea = this.WorkArea,
				WorkAreaCreate = this.WorkAreaCreate,
				WorkAreaDelete = this.WorkAreaDelete,
				WorkAreaUpdate = this.WorkAreaUpdate,

				WorkPlanReporting = this.WorkPlanReporting,
				WorkPlanReportingCreate = this.WorkPlanReportingCreate,
				WorkPlanReportingDelete = this.WorkPlanReportingDelete,
				WorkPlanReportingUpdate = this.WorkPlanReportingUpdate,

				WorkStation = this.WorkStation,
				WorkStationCreate = this.WorkStationCreate,
				WorkStationDelete = this.WorkStationDelete,
				WorkStationUpdate = this.WorkStationUpdate,

				AdminstrationUser = AdministrationUser,
				AdminstrationUserUpdate = AdministrationUserUpdate,
				AdministrationAccessProfiles = AdministrationAccessProfiles,
				AdministrationAccessProfilesUpdate = AdministrationAccessProfilesUpdate,
				isDefault = isDefault
			};
		}
	}
}
