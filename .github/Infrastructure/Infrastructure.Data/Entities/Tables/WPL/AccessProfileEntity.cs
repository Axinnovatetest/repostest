using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.WPL
{
	public class AccessProfileEntity
	{
		public bool Access { get; set; }
		public bool AccessUpdate { get; set; }
		public bool AdministrationAccessProfiles { get; set; }
		public bool AdministrationAccessProfilesUpdate { get; set; }
		public bool AdminstrationUser { get; set; }
		public bool AdminstrationUserUpdate { get; set; }
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
		public int Id { get; set; }
		public bool? isDefault { get; set; }
		public int MainAccessProfileId { get; set; }
		public bool ModuleActivated { get; set; }
		public bool StandardOperation { get; set; }
		public bool StandardOperationCreate { get; set; }
		public bool StandardOperationDelete { get; set; }
		public bool StandardOperationUpdate { get; set; }
		public bool SuperAdministrator { get; set; }
		public bool WorkArea { get; set; }
		public bool WorkAreaCreate { get; set; }
		public bool WorkAreaDelete { get; set; }
		public bool WorkAreaUpdate { get; set; }
		public bool WorkPlan { get; set; }
		public bool WorkPlanCreate { get; set; }
		public bool WorkPlanDelete { get; set; }
		public bool WorkPlanReporting { get; set; }
		public bool WorkPlanReportingCreate { get; set; }
		public bool WorkPlanReportingDelete { get; set; }
		public bool WorkPlanReportingUpdate { get; set; }
		public bool WorkPlanUpdate { get; set; }
		public bool WorkStation { get; set; }
		public bool WorkStationCreate { get; set; }
		public bool WorkStationDelete { get; set; }
		public bool WorkStationUpdate { get; set; }

		public AccessProfileEntity() { }

		public AccessProfileEntity(DataRow dataRow)
		{
			Access = Convert.ToBoolean(dataRow["Access"]);
			AccessUpdate = Convert.ToBoolean(dataRow["AccessUpdate"]);
			AdministrationAccessProfiles = Convert.ToBoolean(dataRow["AdministrationAccessProfiles"]);
			AdministrationAccessProfilesUpdate = Convert.ToBoolean(dataRow["AdministrationAccessProfilesUpdate"]);
			AdminstrationUser = Convert.ToBoolean(dataRow["AdminstrationUser"]);
			AdminstrationUserUpdate = Convert.ToBoolean(dataRow["AdminstrationUserUpdate"]);
			Country = Convert.ToBoolean(dataRow["Country"]);
			CountryCreate = Convert.ToBoolean(dataRow["CountryCreate"]);
			CountryDelete = Convert.ToBoolean(dataRow["CountryDelete"]);
			CountryUpdate = Convert.ToBoolean(dataRow["CountryUpdate"]);
			Departement = Convert.ToBoolean(dataRow["Departement"]);
			DepartementCreate = Convert.ToBoolean(dataRow["DepartementCreate"]);
			DepartementDelete = Convert.ToBoolean(dataRow["DepartementDelete"]);
			DepartementUpdate = Convert.ToBoolean(dataRow["DepartementUpdate"]);
			Hall = Convert.ToBoolean(dataRow["Hall"]);
			HallCreate = Convert.ToBoolean(dataRow["HallCreate"]);
			HallDelete = Convert.ToBoolean(dataRow["HallDelete"]);
			HallUpdate = Convert.ToBoolean(dataRow["HallUpdate"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			isDefault = (dataRow["isDefault"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["isDefault"]);
			MainAccessProfileId = Convert.ToInt32(dataRow["MainAccessProfileId"]);
			ModuleActivated = Convert.ToBoolean(dataRow["ModuleActivated"]);
			StandardOperation = Convert.ToBoolean(dataRow["StandardOperation"]);
			StandardOperationCreate = Convert.ToBoolean(dataRow["StandardOperationCreate"]);
			StandardOperationDelete = Convert.ToBoolean(dataRow["StandardOperationDelete"]);
			StandardOperationUpdate = Convert.ToBoolean(dataRow["StandardOperationUpdate"]);
			SuperAdministrator = Convert.ToBoolean(dataRow["SuperAdministrator"]);
			WorkArea = Convert.ToBoolean(dataRow["WorkArea"]);
			WorkAreaCreate = Convert.ToBoolean(dataRow["WorkAreaCreate"]);
			WorkAreaDelete = Convert.ToBoolean(dataRow["WorkAreaDelete"]);
			WorkAreaUpdate = Convert.ToBoolean(dataRow["WorkAreaUpdate"]);
			WorkPlan = Convert.ToBoolean(dataRow["WorkPlan"]);
			WorkPlanCreate = Convert.ToBoolean(dataRow["WorkPlanCreate"]);
			WorkPlanDelete = Convert.ToBoolean(dataRow["WorkPlanDelete"]);
			WorkPlanReporting = Convert.ToBoolean(dataRow["WorkPlanReporting"]);
			WorkPlanReportingCreate = Convert.ToBoolean(dataRow["WorkPlanReportingCreate"]);
			WorkPlanReportingDelete = Convert.ToBoolean(dataRow["WorkPlanReportingDelete"]);
			WorkPlanReportingUpdate = Convert.ToBoolean(dataRow["WorkPlanReportingUpdate"]);
			WorkPlanUpdate = Convert.ToBoolean(dataRow["WorkPlanUpdate"]);
			WorkStation = Convert.ToBoolean(dataRow["WorkStation"]);
			WorkStationCreate = Convert.ToBoolean(dataRow["WorkStationCreate"]);
			WorkStationDelete = Convert.ToBoolean(dataRow["WorkStationDelete"]);
			WorkStationUpdate = Convert.ToBoolean(dataRow["WorkStationUpdate"]);
		}

		public AccessProfileEntity ShallowClone()
		{
			return new AccessProfileEntity
			{
				Access = Access,
				AccessUpdate = AccessUpdate,
				AdministrationAccessProfiles = AdministrationAccessProfiles,
				AdministrationAccessProfilesUpdate = AdministrationAccessProfilesUpdate,
				AdminstrationUser = AdminstrationUser,
				AdminstrationUserUpdate = AdminstrationUserUpdate,
				Country = Country,
				CountryCreate = CountryCreate,
				CountryDelete = CountryDelete,
				CountryUpdate = CountryUpdate,
				Departement = Departement,
				DepartementCreate = DepartementCreate,
				DepartementDelete = DepartementDelete,
				DepartementUpdate = DepartementUpdate,
				Hall = Hall,
				HallCreate = HallCreate,
				HallDelete = HallDelete,
				HallUpdate = HallUpdate,
				Id = Id,
				isDefault = isDefault,
				MainAccessProfileId = MainAccessProfileId,
				ModuleActivated = ModuleActivated,
				StandardOperation = StandardOperation,
				StandardOperationCreate = StandardOperationCreate,
				StandardOperationDelete = StandardOperationDelete,
				StandardOperationUpdate = StandardOperationUpdate,
				SuperAdministrator = SuperAdministrator,
				WorkArea = WorkArea,
				WorkAreaCreate = WorkAreaCreate,
				WorkAreaDelete = WorkAreaDelete,
				WorkAreaUpdate = WorkAreaUpdate,
				WorkPlan = WorkPlan,
				WorkPlanCreate = WorkPlanCreate,
				WorkPlanDelete = WorkPlanDelete,
				WorkPlanReporting = WorkPlanReporting,
				WorkPlanReportingCreate = WorkPlanReportingCreate,
				WorkPlanReportingDelete = WorkPlanReportingDelete,
				WorkPlanReportingUpdate = WorkPlanReportingUpdate,
				WorkPlanUpdate = WorkPlanUpdate,
				WorkStation = WorkStation,
				WorkStationCreate = WorkStationCreate,
				WorkStationDelete = WorkStationDelete,
				WorkStationUpdate = WorkStationUpdate
			};
		}
	}
}

