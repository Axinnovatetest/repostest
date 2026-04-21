using Newtonsoft.Json;

namespace Psz.Core.Identity.Models
{
	public class HumanResourcesAccessModel
	{
		[JsonProperty("ModuleActivated")]
		public bool ModuleActivated;

		[JsonProperty("Recruitement")]
		public RecruitementAccessModel Recruitement;

		[JsonProperty("TravelManagement")]
		public TravelManagementAccessModel TravelManagement;

		[JsonProperty("PersonnelDevelopment")]
		public PersonnelDevelopmentAccessModel PersonnelDevelopment;

		[JsonProperty("Administration")]
		public Administration Administration;
		public HumanResourcesAccessModel()
		{
			Recruitement = new RecruitementAccessModel();
			TravelManagement = new TravelManagementAccessModel();
			PersonnelDevelopment = new PersonnelDevelopmentAccessModel();
			Administration = new Administration();

		}
	}
	public class HumanResourcesAccessMinimalModel
	{
		[JsonProperty("ModuleActivated")]
		public bool ModuleActivated;

		[JsonProperty("Recruitement")]
		public bool Recruitement;

		[JsonProperty("TravelManagement")]
		public bool TravelManagement;

		[JsonProperty("PersonnelDevelopment")]
		public bool PersonnelDevelopment;

		[JsonProperty("Administration")]
		public bool Administration;
		public HumanResourcesAccessMinimalModel()
		{

		}
		public HumanResourcesAccessMinimalModel(HumanResourcesAccessModel model)
		{
			ModuleActivated = model.ModuleActivated;
		}
	}
}
