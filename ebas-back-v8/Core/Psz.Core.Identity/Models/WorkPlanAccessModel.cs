using System.Collections.Generic;

namespace Psz.Core.Identity.Models
{
	public class WorkPlanAccessModel
	{
		public bool ModuleActivated { get; set; } = false;

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

		public bool SuperAdministrator { get; set; }
		public bool isDefault { get; set; }
		public WorkPlanAccessModel()
		{

		}
		public WorkPlanAccessModel(List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity> accessProfileEntities)
		{
			if(accessProfileEntities == null || accessProfileEntities.Count <= 0)
				return;

			foreach(var accessItem in accessProfileEntities)
			{
				ModuleActivated = ModuleActivated || accessItem.ModuleActivated;

				WorkPlan = WorkPlan || accessItem.WorkPlan;
				WorkPlanCreate = WorkPlanCreate || accessItem.WorkPlanCreate;
				WorkPlanDelete = WorkPlanDelete || accessItem.WorkPlanDelete;
				WorkPlanUpdate = WorkPlanUpdate || accessItem.WorkPlanUpdate;

				Access = Access || accessItem.Access;
				AccessUpdate = AccessUpdate || accessItem.AccessUpdate;

				Country = Country || accessItem.Country;
				CountryCreate = CountryCreate || accessItem.CountryCreate;
				CountryDelete = CountryDelete || accessItem.CountryDelete;
				CountryUpdate = CountryUpdate || accessItem.CountryUpdate;

				Departement = Departement || accessItem.Departement;
				DepartementCreate = DepartementCreate || accessItem.DepartementCreate;
				DepartementDelete = DepartementDelete || accessItem.DepartementDelete;
				DepartementUpdate = DepartementUpdate || accessItem.DepartementUpdate;

				Hall = Hall || accessItem.Hall;
				HallCreate = HallCreate || accessItem.HallCreate;
				HallDelete = HallDelete || accessItem.HallDelete;
				HallUpdate = HallUpdate || accessItem.HallUpdate;

				StandardOperation = StandardOperation || accessItem.StandardOperation;
				StandardOperationCreate = StandardOperationCreate || accessItem.StandardOperationCreate;
				StandardOperationDelete = StandardOperationDelete || accessItem.StandardOperationDelete;
				StandardOperationUpdate = StandardOperationUpdate || accessItem.StandardOperationUpdate;

				WorkArea = WorkArea || accessItem.WorkArea;
				WorkAreaCreate = WorkAreaCreate || accessItem.WorkAreaCreate;
				WorkAreaDelete = WorkAreaDelete || accessItem.WorkAreaDelete;
				WorkAreaUpdate = WorkAreaUpdate || accessItem.WorkAreaUpdate;

				WorkPlanReporting = WorkPlanReporting || accessItem.WorkPlanReporting;
				WorkPlanReportingCreate = WorkPlanReportingCreate || accessItem.WorkPlanReportingCreate;
				WorkPlanReportingDelete = WorkPlanReportingDelete || accessItem.WorkPlanReportingDelete;
				WorkPlanReportingUpdate = WorkPlanReportingUpdate || accessItem.WorkPlanReportingUpdate;

				WorkStation = WorkStation || accessItem.WorkStation;
				WorkStationCreate = WorkStationCreate || accessItem.WorkStationCreate;
				WorkStationDelete = WorkStationDelete || accessItem.WorkStationDelete;
				WorkStationUpdate = WorkStationUpdate || accessItem.WorkStationUpdate;

				AdministrationUser = AdministrationUser || accessItem.AdminstrationUser;
				AdministrationUserUpdate = AdministrationUserUpdate || accessItem.AdminstrationUserUpdate;
				AdministrationAccessProfiles = AdministrationAccessProfiles || accessItem.AdministrationAccessProfiles;
				AdministrationAccessProfilesUpdate = AdministrationAccessProfilesUpdate || accessItem.AdministrationAccessProfilesUpdate;

				SuperAdministrator = SuperAdministrator || accessItem.SuperAdministrator;
				isDefault = isDefault || (accessItem.isDefault.HasValue) ? accessItem.isDefault.Value : false;
			}
		}

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

				SuperAdministrator = SuperAdministrator,
				isDefault = isDefault,
			};
		}
	}
	public class WorkPlanAccessMinimalModel
	{
		public bool ModuleActivated { get; set; }

		public bool Access { get; set; }

		public bool Country { get; set; }

		public bool Departement { get; set; }

		public bool Hall { get; set; }

		public bool StandardOperation { get; set; }

		public bool WorkArea { get; set; }

		public bool WorkPlan { get; set; }

		public bool WorkPlanReporting { get; set; }

		public bool WorkStation { get; set; }

		public bool SuperAdministrator { get; set; }
		public WorkPlanAccessMinimalModel()
		{

		}
		public WorkPlanAccessMinimalModel(WorkPlanAccessModel model)
		{
			ModuleActivated = model.ModuleActivated;

			Access = model.Access;

			Country = model.Country;

			Departement = model.Departement;

			Hall = model.Hall;

			StandardOperation = model.StandardOperation;

			WorkArea = model.WorkArea;

			WorkPlan = model.WorkPlan;

			WorkPlanReporting = model.WorkPlanReporting;

			WorkStation = model.WorkStation;

			SuperAdministrator = model.SuperAdministrator;
		}
		public Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity ToDbEntity(int id, int mainAccessProfileId)
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
				Access = this.Access,
				Country = this.Country,
				Departement = this.Departement,
				Hall = this.Hall,
				StandardOperation = this.StandardOperation,
				WorkArea = this.WorkArea,
				WorkPlanReporting = this.WorkPlanReporting,
				WorkStation = this.WorkStation,
				SuperAdministrator = SuperAdministrator
			};
		}
	}
}
