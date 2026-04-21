using System.Collections.Generic;

namespace Psz.Core.Identity.Models
{
	public class MaterialRequirementPlanning
	{
		public bool AllRessourcesAuthorized { get; set; }
		public bool Capacity { get; set; }
		public bool CapacityEdit { get; set; }
		public bool CapacityPlan { get; set; }
		public bool CapacityPlanEdit { get; set; }
		public bool Holiday { get; set; }
		public bool HolidayEdit { get; set; }
		public bool RessourceAuthorizationEdit { get; set; }
		public bool Validation { get; set; }
		public bool ValidationEdit { get; set; }
		public bool Configuration { get; set; }
		public bool ConfigurationEdit { get; set; }

		public bool ModuleActivated { get; set; } = false;
		public MaterialRequirementPlanning()
		{

		}
		public MaterialRequirementPlanning(List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity> profileEntities)
		{
			AllRessourcesAuthorized = false;
			Capacity = false;
			CapacityEdit = false;
			CapacityPlan = false;
			CapacityPlanEdit = false;
			Holiday = false;
			HolidayEdit = false;
			RessourceAuthorizationEdit = false;
			Validation = false;
			ValidationEdit = false;
			Configuration = false;
			ConfigurationEdit = false;

			// - 
			if(profileEntities == null || profileEntities.Count <= 0)
				return;
			// - 
			foreach(var profileEntityItem in profileEntities)
			{
				AllRessourcesAuthorized = AllRessourcesAuthorized || profileEntityItem.CRP_AllRessourcesAuthorized;
				Capacity = Capacity || profileEntityItem.CRP_Capacity;
				CapacityEdit = CapacityEdit || profileEntityItem.CRP_CapacityEdit;
				CapacityPlan = CapacityPlan || profileEntityItem.CRP_CapacityPlan;
				CapacityPlanEdit = CapacityPlanEdit || profileEntityItem.CRP_CapacityPlanEdit;
				Holiday = Holiday || profileEntityItem.CRP_Holiday;
				HolidayEdit = HolidayEdit || profileEntityItem.CRP_HolidayEdit;
				Validation = Holiday || profileEntityItem.CRP_Validation;
				ValidationEdit = HolidayEdit || profileEntityItem.CRP_ValidationEdit;
				Configuration = Holiday || profileEntityItem.CRP_Configuration;
				ConfigurationEdit = HolidayEdit || profileEntityItem.CRP_ConfigurationEdit;
				RessourceAuthorizationEdit = RessourceAuthorizationEdit || profileEntityItem.CRP_RessourceAuthorizationEdit;
			}

			// -
			ModuleActivated = Capacity || CapacityEdit || CapacityPlan || CapacityPlanEdit || Holiday
				|| HolidayEdit || AllRessourcesAuthorized || RessourceAuthorizationEdit
				|| Validation || ValidationEdit || Configuration || ConfigurationEdit;
		}
	}
}
