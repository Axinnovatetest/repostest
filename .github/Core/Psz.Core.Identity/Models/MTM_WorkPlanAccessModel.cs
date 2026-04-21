using Infrastructure.Data.Entities.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Identity.Models
{
	public class MTM_WorkPlanAccessModel
	{
		public bool? Country { get; set; }
		public bool? CountryCreate { get; set; }
		public bool? CountryDelete { get; set; }
		public bool? CountryUpdate { get; set; }

		// Departement
		public bool? Departement { get; set; }
		public bool? DepartementCreate { get; set; }
		public bool? DepartementDelete { get; set; }
		public bool? DepartementUpdate { get; set; }

		// Hall
		public bool? Hall { get; set; }
		public bool? HallCreate { get; set; }
		public bool? HallDelete { get; set; }
		public bool? HallUpdate { get; set; }

		// StandardOperation
		public bool? StandardOperation { get; set; }
		public bool? StandardOperationCreate { get; set; }
		public bool? StandardOperationDelete { get; set; }
		public bool? StandardOperationUpdate { get; set; }

		// WorkArea
		public bool? WorkArea { get; set; }
		public bool? WorkAreaCreate { get; set; }
		public bool? WorkAreaDelete { get; set; }
		public bool? WorkAreaUpdate { get; set; }

		// WorkPlan
		public bool? WorkPlan { get; set; }
		public bool? WorkPlanCreate { get; set; }
		public bool? WorkPlanDelete { get; set; }
		public bool? WorkPlanUpdate { get; set; }

		// WorkPlanReporting
		public bool? WorkPlanReporting { get; set; }
		public bool? WorkPlanReportingCreate { get; set; }
		public bool? WorkPlanReportingDelete { get; set; }
		public bool? WorkPlanReportingUpdate { get; set; }

		// WorkStation
		public bool? WorkStation { get; set; }
		public bool? WorkStationCreate { get; set; }
		public bool? WorkStationDelete { get; set; }
		public bool? WorkStationUpdate { get; set; }

		public bool? ModuleActivated { get; set; } = false;

		public MTM_WorkPlanAccessModel()
		{
			
		}
		public MTM_WorkPlanAccessModel(List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity> profileEntities)
		{
			Country = false;
			CountryCreate = false;
			CountryDelete = false;
			CountryUpdate = false;

			Departement = false;
			DepartementCreate = false;
			DepartementDelete = false;
			DepartementUpdate = false;

			Hall = false;
			HallCreate = false;
			HallDelete = false;
			HallUpdate = false;

			StandardOperation = false;
			StandardOperationCreate = false;
			StandardOperationDelete = false;
			StandardOperationUpdate = false;

			WorkArea = false;
			WorkAreaCreate = false;
			WorkAreaDelete = false;
			WorkAreaUpdate = false;

			WorkPlan = false;
			WorkPlanCreate = false;
			WorkPlanDelete = false;
			WorkPlanUpdate = false;

			WorkPlanReporting = false;
			WorkPlanReportingCreate = false;
			WorkPlanReportingDelete = false;
			WorkPlanReportingUpdate = false;

			WorkStation = false;
			WorkStationCreate = false;
			WorkStationDelete = false;
			WorkStationUpdate = false;

			if(profileEntities == null || profileEntities.Count <= 0)
				return;

			foreach(var profile in profileEntities)
			{
				Country = (Country ?? false) || (profile?.WP_Country ?? false);
				CountryCreate = (CountryCreate ?? false) || (profile?.WP_CountryCreate ?? false);
				CountryDelete = (CountryDelete ?? false) || (profile?.WP_CountryDelete ?? false);
				CountryUpdate = (CountryUpdate ?? false) || (profile?.WP_CountryUpdate ?? false);

				Departement = (Departement ?? false) || (profile?.WP_Departement ?? false);
				DepartementCreate = (DepartementCreate ?? false) || (profile?.WP_DepartementCreate ?? false);
				DepartementDelete = (DepartementDelete ?? false) || (profile?.WP_DepartementDelete ?? false);
				DepartementUpdate = (DepartementUpdate ?? false) || (profile?.WP_DepartementUpdate ?? false);

				Hall = (Hall ?? false) || (profile?.WP_Hall ?? false);
				HallCreate = (HallCreate ?? false) || (profile?.WP_HallCreate ?? false);
				HallDelete = (HallDelete ?? false) || (profile?.WP_HallDelete ?? false);
				HallUpdate = (HallUpdate ?? false) || (profile?.WP_HallUpdate ?? false);

				StandardOperation = (StandardOperation ?? false) || (profile?.WP_StandardOperation ?? false);
				StandardOperationCreate = (StandardOperationCreate ?? false) || (profile?.WP_StandardOperationCreate ?? false);
				StandardOperationDelete = (StandardOperationDelete ?? false) || (profile?.WP_StandardOperationDelete ?? false);
				StandardOperationUpdate = (StandardOperationUpdate ?? false) || (profile?.WP_StandardOperationUpdate ?? false);

				WorkArea = (WorkArea ?? false) || (profile?.WP_WorkArea ?? false);
				WorkAreaCreate = (WorkAreaCreate ?? false) || (profile?.WP_WorkAreaCreate ?? false);
				WorkAreaDelete = (WorkAreaDelete ?? false) || (profile?.WP_WorkAreaDelete ?? false);
				WorkAreaUpdate = (WorkAreaUpdate ?? false) || (profile?.WP_WorkAreaUpdate ?? false);

				WorkPlan = (WorkPlan ?? false) || (profile?.WP_WorkPlan ?? false);
				WorkPlanCreate = (WorkPlanCreate ?? false) || (profile?.WP_WorkPlanCreate ?? false);
				WorkPlanDelete = (WorkPlanDelete ?? false) || (profile?.WP_WorkPlanDelete ?? false);
				WorkPlanUpdate = (WorkPlanUpdate ?? false) || (profile?.WP_WorkPlanUpdate ?? false);

				WorkPlanReporting = (WorkPlanReporting ?? false) || (profile?.WP_WorkPlanReporting ?? false);
				WorkPlanReportingCreate = (WorkPlanReportingCreate ?? false) || (profile?.WP_WorkPlanReportingCreate ?? false);
				WorkPlanReportingDelete = (WorkPlanReportingDelete ?? false) || (profile?.WP_WorkPlanReportingDelete ?? false);
				WorkPlanReportingUpdate = (WorkPlanReportingUpdate ?? false) || (profile?.WP_WorkPlanReportingUpdate ?? false);

				WorkStation = (WorkStation ?? false) || (profile?.WP_WorkStation ?? false);
				WorkStationCreate = (WorkStationCreate ?? false) || (profile?.WP_WorkStationCreate ?? false);
				WorkStationDelete = (WorkStationDelete ?? false) || (profile?.WP_WorkStationDelete ?? false);
				WorkStationUpdate = (WorkStationUpdate ?? false) || (profile?.WP_WorkStationUpdate ?? false);
			}


			ModuleActivated = Country == true || CountryCreate == true || CountryDelete == true || CountryUpdate == true
				|| Departement == true || DepartementCreate == true || DepartementDelete == true || DepartementUpdate == true
				|| Hall == true || HallCreate == true || HallDelete == true || HallUpdate == true
				|| StandardOperation == true || StandardOperationCreate == true || StandardOperationDelete == true || StandardOperationUpdate == true
				|| WorkArea == true || WorkAreaCreate == true || WorkAreaDelete == true || WorkAreaUpdate == true
				|| WorkPlan == true || WorkPlanCreate == true || WorkPlanDelete == true || WorkPlanUpdate == true
				|| WorkPlanReporting == true || WorkPlanReportingCreate == true || WorkPlanReportingDelete == true || WorkPlanReportingUpdate == true
				|| WorkStation == true || WorkStationCreate == true || WorkStationDelete == true || WorkStationUpdate == true;
		}
	}
}
