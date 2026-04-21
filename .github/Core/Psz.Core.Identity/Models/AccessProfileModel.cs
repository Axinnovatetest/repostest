namespace Psz.Core.Identity.Models
{
	public class AccessProfileModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string CreationUser { get; set; }

		public SettingsAccessModel Settings { get; set; }
		public WorkPlanAccessModel WorkPlan { get; set; }
		public PurchaseAccessModel Purchase { get; set; }
		public FinancialAccessModel Financial { get; set; }
		public SalesDistributionAccessModel SalesDistribution { get; set; }
		public CustomerServiceAccessModel CustomerService { get; set; }
		public LogisticsAccessModel Logistics { get; set; }
		public HumanResourcesAccessModel HumanResources { get; set; }
		public MaterialManagementAccessModel MaterialManagement { get; set; }
		public MasterDataAccessModel MasterData { get; set; }

		public AdministrationAccessModel Administration { get; set; }

		public FNC_AccessProfileModel Budget { get; set; }
		public ManagementOverviewAccessModel ManagementOverview { get; set; }
		public CRPAccessModel CRP { get; set; }
		public CapitalRequestsAccessModel CapitalRequests { get; set; }
	}

	public class AccessProfileMinimalModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string CreationUser { get; set; }

		public SettingsAccessMinimalModel Settings { get; set; }
		public WorkPlanAccessMinimalModel WorkPlan { get; set; }
		public PurchaseAccessMinimalModel Purchase { get; set; }
		public FinancialAccessMinimalModel Financal { get; set; }
		public SalesDistributionAccessMinimalModel SalesDistribution { get; set; }
		public CustomerServiceAccessModel CustomerService { get; set; }
		public LogisticsAccessMinimalModel Logistics { get; set; }
		public HumanResourcesAccessMinimalModel HumanResources { get; set; }
		public MaterialManagementAccessMinimalModel MaterialManagement { get; set; }
		public MasterDataAccessModel MasterData { get; set; }
		public AdministrationAccessModel Administration { get; set; }
		public FNC_AccessProfileMinimalModel Budget { get; set; }
		public ManagementOverviewAccessModel ManagementOverviewAccess { get; set; }
		public CRPAccessModel CRP { get; set; }
		public CapitalRequestsAccessModel CapitalRequests { get; set; }


		public AccessProfileMinimalModel()
		{

		}
		public AccessProfileMinimalModel(AccessProfileModel model)
		{
			Id = model.Id;
			Name = model.Name;
			CreationUser = model.CreationUser;
			// -
			Settings = new SettingsAccessMinimalModel(model.Settings);
			WorkPlan = new WorkPlanAccessMinimalModel(model.WorkPlan);
			Purchase = new PurchaseAccessMinimalModel(model.Purchase);
			Financal = new FinancialAccessMinimalModel(model.Financial);
			SalesDistribution = new SalesDistributionAccessMinimalModel(model.SalesDistribution);
			CustomerService = new CustomerServiceAccessModel(model.CustomerService);
			Logistics = new LogisticsAccessMinimalModel(model.Logistics);
			HumanResources = new HumanResourcesAccessMinimalModel(model.HumanResources);
			MaterialManagement = new MaterialManagementAccessMinimalModel(model.MaterialManagement);
			MasterData = model.MasterData;
			Administration = model.Administration;
			Budget = new FNC_AccessProfileMinimalModel(model.Budget);
			ManagementOverviewAccess = new ManagementOverviewAccessModel(model.ManagementOverview);
			CapitalRequests = new CapitalRequestsAccessModel(model.CapitalRequests);
			CRP = new CRPAccessModel(model.CRP);
		}
	}
}
