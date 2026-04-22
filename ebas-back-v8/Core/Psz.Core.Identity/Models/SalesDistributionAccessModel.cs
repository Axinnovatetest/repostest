using Newtonsoft.Json;

namespace Psz.Core.Identity.Models
{
	public class SalesDistributionAccessModel
	{
		[JsonProperty("ModuleActivated")]
		public bool ModuleActivated;

		[JsonProperty("ProjectSystem")]
		public ProjectSystem ProjectSystem;

		[JsonProperty("ArticleMasterData")]
		public ArticleMasterData ArticleMasterData;

		[JsonProperty("WorkPlan")]
		public WorkPlan WorkPlan;

		[JsonProperty("Administration")]
		public Administration Administration;
		public SalesDistributionAccessModel()
		{
			ProjectSystem = new ProjectSystem();
			ArticleMasterData = new ArticleMasterData();
			WorkPlan = new WorkPlan();
			Administration = new Administration();
		}
	}
	public class SalesDistributionAccessMinimalModel
	{
		[JsonProperty("ModuleActivated")]
		public bool ModuleActivated;

		[JsonProperty("ProjectSystem")]
		public bool ProjectSystem;

		[JsonProperty("ArticleMasterData")]
		public bool ArticleMasterData;

		[JsonProperty("WorkPlan")]
		public bool WorkPlan;

		[JsonProperty("Administration")]
		public bool Administration;
		public SalesDistributionAccessMinimalModel()
		{
		}
		public SalesDistributionAccessMinimalModel(SalesDistributionAccessModel model)
		{
			ModuleActivated = model.ModuleActivated;
		}
	}
}
