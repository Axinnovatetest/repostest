namespace Psz.Core.Apps.Settings.Models.AccessProfiles
{
	public class UpdateModel
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public SettingsAccessModel Settings { get; set; }
		public WorkPlanAccessModel WorkPlan { get; set; }
		public PurchaseAccessModel Purchase { get; set; }

		public BudgetAccessModel Budget { get; set; }
	}
}
