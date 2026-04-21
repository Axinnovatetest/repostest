using Psz.Core.Identity.Models;
using System.Collections.Generic;

namespace Psz.Core.Apps.Settings.Models.AccessProfiles
{
	public class AccessProfileModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string CreationUser { get; set; }
		public bool SuperAdministrator { get; set; }

		public SettingsAccessModel Settings { get; set; }
		public WorkPlanAccessModel WorkPlan { get; set; }
		public PurchaseAccessModel Purchase { get; set; }
		public FinancialAccessModel Financal { get; set; }
		public SalesDistributionAccessModel SalesDistribution { get; set; }
		public CustomerServiceAccessModel CustomerService { get; set; }
		public LogisticsAccessModel Logistics { get; set; }
		public HumanResourcesAccessModel HumanResources { get; set; }
		public MaterialManagementAccessModel MaterialManagement { get; set; }
		public MasterDataAccessModel MasterData { get; set; }

		public AdministrationAccessModel Administration { get; set; }

		//public FNC_AccessProfileModel Budget { get; set; }
		public BudgetAccessModel Budget { get; set; }
	}
	public class AddUsersModel
	{
		public int ProfileId { get; set; }
		public List<int> UserIds { get; set; }
	}
}
