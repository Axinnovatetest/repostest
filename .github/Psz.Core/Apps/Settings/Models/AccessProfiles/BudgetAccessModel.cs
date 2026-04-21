namespace Psz.Core.Apps.Settings.Models.AccessProfiles
{
	public class BudgetAccessModel
	{
		public bool AddExternalCommande { get; set; }
		public bool AddExternalProject { get; set; }
		public bool AddInternalCommande { get; set; }
		public bool AddInternalProject { get; set; }
		public bool Administration { get; set; }
		public bool AdministrationAccessProfiles { get; set; }
		public bool AdministrationAccessProfilesUpdate { get; set; }
		public bool AdministrationUser { get; set; }
		public bool AdministrationUserUpdate { get; set; }
		public bool Article { get; set; }
		public bool Assign { get; set; }
		public bool AssignAllDepartments { get; set; }
		public bool AssignAllEmployees { get; set; }
		public bool AssignAllSites { get; set; }
		public bool AssignCreateDept { get; set; }
		public bool AssignCreateLand { get; set; }
		public bool AssignCreateUser { get; set; }
		public bool AssignDeleteDept { get; set; }
		public bool AssignDeleteLand { get; set; }
		public bool AssignDeleteUser { get; set; }
		public bool AssignEditDept { get; set; }
		public bool AssignEditLand { get; set; }
		public bool AssignEditUser { get; set; }
		public bool AssignViewDept { get; set; }
		public bool AssignViewLand { get; set; }
		public bool AssignViewUser { get; set; }
		public bool Commande { get; set; }
		public bool Config { get; set; }
		public bool ConfigCreateArtikel { get; set; }
		public bool ConfigCreateDept { get; set; }
		public bool ConfigCreateLand { get; set; }
		public bool ConfigCreateSupplier { get; set; }
		public bool ConfigDeleteArtikel { get; set; }
		public bool ConfigDeleteDept { get; set; }
		public bool ConfigDeleteLand { get; set; }
		public bool ConfigDeleteSupplier { get; set; }
		public bool ConfigEditArtikel { get; set; }
		public bool ConfigEditDept { get; set; }
		public bool ConfigEditLand { get; set; }
		public bool ConfigEditSupplier { get; set; }
		public bool DeleteExternalCommande { get; set; }
		public bool DeleteExternalProject { get; set; }
		public bool DeleteInternalCommande { get; set; }
		public bool DeleteInternalProject { get; set; }
		public int ID { get; set; }
		public int MainAccessProfileId { get; set; }
		public bool ModuleActivated { get; set; }
		public bool Project { get; set; }
		public bool Suppliers { get; set; }
		public bool Units { get; set; }
		public bool UpdateExternalCommande { get; set; }
		public bool UpdateExternalProject { get; set; }
		public bool UpdateInternalCommande { get; set; }
		public bool UpdateInternalProject { get; set; }
		public bool ViewExternalCommande { get; set; }
		public bool ViewExternalProject { get; set; }
		public bool ViewInternalCommande { get; set; }
		public bool ViewInternalProject { get; set; }

		public bool Receptions { get; set; }
		public bool ReceptionsView { get; set; }
		public bool ReceptionsEdit { get; set; }

		// -
		public bool Budget { get; set; }
		public bool CashLiquidity { get; set; }
		public bool CreditManagement { get; set; }

		//- 
		public bool? CommandeExternalEditAllGroup { get; set; }
		public bool? CommandeExternalEditAllSite { get; set; }
		public bool? CommandeExternalViewAllGroup { get; set; }
		public bool? CommandeExternalViewAllSite { get; set; }
		public bool? CommandeInternalEditAllDepartment { get; set; }
		public bool? CommandeInternalEditAllGroup { get; set; }
		public bool? CommandeInternalEditAllSite { get; set; }
		public bool? CommandeInternalViewAllDepartment { get; set; }
		public bool? CommandeInternalViewAllGroup { get; set; }
		public bool? CommandeInternalViewAllSite { get; set; }
		//-
		public bool? ProjectExternalEditAllGroup { get; set; }
		public bool? ProjectExternalEditAllSite { get; set; }
		public bool? ProjectExternalViewAllGroup { get; set; }
		public bool? ProjectExternalViewAllSite { get; set; }
		public bool? ProjectInternalEditAllGroup { get; set; }
		public bool? ProjectInternalEditAllSite { get; set; }
		public bool? ProjectInternalViewAllGroup { get; set; }
		public bool? ProjectInternalViewAllSite { get; set; }
		// - 
		public bool? CommandeExternalViewInvoice { get; set; }
		public bool? CommandeExternalViewInvoiceAllGroup { get; set; }
		public bool? CommandeInternalViewInvoice { get; set; }
		public bool? CommandeInternalViewInvoiceAllGroup { get; set; }
		public BudgetAccessModel()
		{

		}

		public BudgetAccessModel(int mainAccessProfileId, Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity entity)
		{
			ID = entity.Id;
			//MainAccessProfileId = entity.MainAccessProfileId ?? -1;
			ModuleActivated = entity.ModuleActivated ?? false;

			Administration = entity.Administration ?? false;
			AdministrationAccessProfiles = entity.AdministrationAccessProfiles ?? false;
			AdministrationAccessProfilesUpdate = entity.Administration ?? false && (entity.AdministrationAccessProfilesUpdate ?? false);
			AdministrationUser = (entity.Administration ?? false) && (entity.AdministrationUser ?? false);
			AdministrationUserUpdate = (entity.Administration ?? false) && (entity.AdministrationUserUpdate ?? false);


			Assign = entity.Assign ?? false;
			AssignAllDepartments = entity.AssignAllDepartments ?? false;
			AssignAllEmployees = entity.AssignAllEmployees ?? false;
			AssignAllSites = entity.AssignAllSites ?? false;
			AssignCreateDept = (entity.Assign ?? false) && (entity.AssignCreateDept ?? false);
			AssignCreateLand = (entity.Assign ?? false) && (entity.AssignCreateLand ?? false);
			AssignCreateUser = (entity.Assign ?? false) && (entity.AssignCreateUser ?? false);
			AssignDeleteDept = (entity.Assign ?? false) && (entity.AssignDeleteDept ?? false);
			AssignDeleteLand = (entity.Assign ?? false) && (entity.AssignDeleteLand ?? false);
			AssignDeleteUser = (entity.Assign ?? false) && (entity.AssignDeleteUser ?? false);
			AssignEditDept = (entity.Assign ?? false) && (entity.AssignEditDept ?? false);
			AssignEditLand = (entity.Assign ?? false) && (entity.AssignEditLand ?? false);
			AssignEditUser = (entity.Assign ?? false) && (entity.AssignEditUser ?? false);
			AssignViewDept = (entity.Assign ?? false) && (entity.AssignViewDept ?? false);
			AssignViewLand = (entity.Assign ?? false) && (entity.AssignViewLand ?? false);
			AssignViewUser = (entity.Assign ?? false) && (entity.AssignViewUser ?? false);

			Commande = entity.Commande ?? false;
			AddExternalCommande = (entity.Commande ?? false) && (entity.AddExternalCommande ?? false);
			AddInternalCommande = (entity.Commande ?? false) && (entity.AddInternalCommande ?? false);
			DeleteExternalCommande = (entity.Commande ?? false) && (entity.DeleteExternalCommande ?? false);
			DeleteInternalCommande = (entity.Commande ?? false) && (entity.DeleteInternalCommande ?? false);
			UpdateExternalCommande = (entity.Commande ?? false) && (entity.UpdateExternalCommande ?? false);
			UpdateInternalCommande = (entity.Commande ?? false) && (entity.UpdateInternalCommande ?? false);
			ViewExternalCommande = (entity.Commande ?? false) && (entity.ViewExternalCommande ?? false);
			ViewInternalCommande = (entity.Commande ?? false) && (entity.ViewInternalCommande ?? false);

			CommandeExternalEditAllGroup = (entity.Commande ?? false) && (entity.CommandeExternalEditAllGroup ?? false);
			CommandeExternalEditAllSite = (entity.Commande ?? false) && (entity.CommandeExternalEditAllSite ?? false);
			CommandeExternalViewAllGroup = (entity.Commande ?? false) && (entity.CommandeExternalViewAllGroup ?? false);
			CommandeExternalViewAllSite = (entity.Commande ?? false) && (entity.CommandeExternalViewAllSite ?? false);
			CommandeInternalEditAllDepartment = (entity.Commande ?? false) && (entity.CommandeInternalEditAllDepartment ?? false);
			CommandeInternalEditAllGroup = (entity.Commande ?? false) && (entity.CommandeInternalEditAllGroup ?? false);
			CommandeInternalEditAllSite = (entity.Commande ?? false) && (entity.CommandeInternalEditAllSite ?? false);
			CommandeInternalViewAllDepartment = (entity.Commande ?? false) && (entity.CommandeInternalViewAllDepartment ?? false);
			CommandeInternalViewAllGroup = (entity.Commande ?? false) && (entity.CommandeInternalViewAllGroup ?? false);
			CommandeInternalViewAllSite = (entity.Commande ?? false) && (entity.CommandeInternalViewAllSite ?? false);
			// - 
			CommandeExternalViewInvoice = (entity.Commande ?? false) && (entity.CommandeExternalViewInvoice ?? false);
			CommandeExternalViewInvoiceAllGroup = (entity.Commande ?? false) && (entity.CommandeExternalViewInvoiceAllGroup ?? false);
			CommandeInternalViewInvoice = (entity.Commande ?? false) && (entity.CommandeInternalViewInvoice ?? false);
			CommandeInternalViewInvoiceAllGroup = (entity.Commande ?? false) && (entity.CommandeInternalViewInvoiceAllGroup ?? false);

			Config = entity.Config ?? false;
			ConfigCreateArtikel = entity.ConfigCreateArtikel ?? false;
			ConfigCreateDept = entity.ConfigCreateDept ?? false;
			ConfigCreateLand = entity.ConfigCreateLand ?? false;
			ConfigCreateSupplier = entity.ConfigCreateSupplier ?? false;
			ConfigDeleteArtikel = entity.ConfigDeleteArtikel ?? false;
			ConfigDeleteDept = entity.ConfigDeleteDept ?? false;
			ConfigDeleteLand = entity.ConfigDeleteLand ?? false;
			ConfigDeleteSupplier = entity.ConfigDeleteSupplier ?? false;
			ConfigEditArtikel = entity.ConfigEditArtikel ?? false;
			ConfigEditDept = entity.ConfigEditDept ?? false;
			ConfigEditLand = entity.ConfigEditLand ?? false;
			ConfigEditSupplier = entity.ConfigEditSupplier ?? false;

			Project = entity.Project ?? false;
			AddExternalProject = entity.AddExternalProject ?? false;
			AddInternalProject = entity.AddInternalProject ?? false;
			UpdateExternalProject = entity.UpdateExternalProject ?? false;
			UpdateInternalProject = entity.UpdateInternalProject ?? false;
			ViewExternalProject = entity.ViewExternalProject ?? false;
			ViewInternalProject = entity.ViewInternalProject ?? false;
			DeleteExternalProject = entity.DeleteExternalProject ?? false;
			DeleteInternalProject = entity.DeleteInternalProject ?? false;

			Article = entity.Article ?? false;
			Suppliers = entity.Suppliers ?? false;
			Units = entity.Units ?? false;
			// -

			Receptions = entity.Receptions ?? false;
			ReceptionsEdit = (entity.Receptions ?? false) && (entity.ReceptionsEdit ?? false);
			ReceptionsView = (entity.Receptions ?? false) && (entity.ReceptionsView ?? false);
			// - 
			Budget = entity.Budget;
			CashLiquidity = entity.CashLiquidity;
			CreditManagement = entity.CreditManagement;


			//-
			CommandeExternalEditAllGroup = (entity.Commande ?? false) && (entity.CommandeExternalEditAllGroup ?? false);
			CommandeExternalEditAllSite = (entity.Commande ?? false) && (entity.CommandeExternalEditAllSite ?? false);
			CommandeExternalViewAllGroup = (entity.Commande ?? false) && (entity.CommandeExternalViewAllGroup ?? false);
			CommandeExternalViewAllSite = (entity.Commande ?? false) && (entity.CommandeExternalViewAllSite ?? false);
			CommandeInternalEditAllDepartment = (entity.Commande ?? false) && (entity.CommandeInternalEditAllDepartment ?? false);
			CommandeInternalEditAllGroup = (entity.Commande ?? false) && (entity.CommandeInternalEditAllGroup ?? false);
			CommandeInternalEditAllSite = (entity.Commande ?? false) && (entity.CommandeInternalEditAllSite ?? false);
			CommandeInternalViewAllDepartment = (entity.Commande ?? false) && (entity.CommandeInternalViewAllDepartment ?? false);
			CommandeInternalViewAllGroup = (entity.Commande ?? false) && (entity.CommandeInternalViewAllGroup ?? false);
			CommandeInternalViewAllSite = (entity.Commande ?? false) && (entity.CommandeInternalViewAllSite ?? false);
			//-
			ProjectExternalEditAllGroup = (entity.Project ?? false) && (entity.ProjectExternalEditAllGroup ?? false);
			ProjectExternalEditAllSite = (entity.Project ?? false) && (entity.ProjectExternalEditAllSite ?? false);
			ProjectExternalViewAllGroup = (entity.Project ?? false) && (entity.ProjectExternalViewAllGroup ?? false);
			ProjectExternalViewAllSite = (entity.Project ?? false) && (entity.ProjectExternalViewAllSite ?? false);
			ProjectInternalEditAllGroup = (entity.Project ?? false) && (entity.ProjectInternalEditAllGroup ?? false);
			ProjectInternalEditAllSite = (entity.Project ?? false) && (entity.ProjectInternalEditAllSite ?? false);
			ProjectInternalViewAllGroup = (entity.Project ?? false) && (entity.ProjectInternalViewAllGroup ?? false);
			ProjectInternalViewAllSite = (entity.Project ?? false) && (entity.ProjectInternalViewAllSite ?? false);
		}
		internal Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity ToDbEntity(int id, int mainAccessProfileId)
		{
			if(!this.ModuleActivated)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity()
				{
					Id = id,
					//MainAccessProfileId = mainAccessProfileId,
					ModuleActivated = false,
				};
			}

			return new Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity()
			{
				Id = id,
				//MainAccessProfileId = mainAccessProfileId,
				ModuleActivated = true,

				Administration = this.Administration,
				AdministrationAccessProfiles = this.Administration && this.AdministrationAccessProfiles,
				AdministrationAccessProfilesUpdate = this.Administration && this.AdministrationAccessProfilesUpdate,
				AdministrationUser = this.Administration && this.AdministrationUser,
				AdministrationUserUpdate = this.Administration && this.AdministrationUserUpdate,


				Assign = this.Assign,
				AssignAllDepartments = this.AssignAllDepartments,
				AssignAllEmployees = this.AssignAllEmployees,
				AssignAllSites = this.AssignAllSites,
				AssignCreateDept = this.Assign && this.AssignCreateDept,
				AssignCreateLand = this.Assign && this.AssignCreateLand,
				AssignCreateUser = this.Assign && this.AssignCreateUser,
				AssignDeleteDept = this.Assign && this.AssignDeleteDept,
				AssignDeleteLand = this.Assign && this.AssignDeleteLand,
				AssignDeleteUser = this.Assign && this.AssignDeleteUser,
				AssignEditDept = this.Assign && this.AssignEditDept,
				AssignEditLand = this.Assign && this.AssignEditLand,
				AssignEditUser = this.Assign && this.AssignEditUser,
				AssignViewDept = this.Assign && this.AssignViewDept,
				AssignViewLand = this.Assign && this.AssignViewLand,
				AssignViewUser = this.Assign && this.AssignViewUser,

				Commande = this.Commande,
				AddExternalCommande = this.Commande && this.AddExternalCommande,
				AddInternalCommande = this.Commande && this.AddInternalCommande,
				DeleteExternalCommande = this.Commande && this.DeleteExternalCommande,
				DeleteInternalCommande = this.Commande && this.DeleteInternalCommande,
				UpdateExternalCommande = this.Commande && this.UpdateExternalCommande,
				UpdateInternalCommande = this.Commande && this.UpdateInternalCommande,
				ViewExternalCommande = this.Commande && this.ViewExternalCommande,
				ViewInternalCommande = this.Commande && this.ViewInternalCommande,

				Config = this.Config,
				ConfigCreateArtikel = this.ConfigCreateArtikel,
				ConfigCreateDept = this.ConfigCreateDept,
				ConfigCreateLand = this.ConfigCreateLand,
				ConfigCreateSupplier = this.ConfigCreateSupplier,
				ConfigDeleteArtikel = this.ConfigDeleteArtikel,
				ConfigDeleteDept = this.ConfigDeleteDept,
				ConfigDeleteLand = this.ConfigDeleteLand,
				ConfigDeleteSupplier = this.ConfigDeleteSupplier,
				ConfigEditArtikel = this.ConfigEditArtikel,
				ConfigEditDept = this.ConfigEditDept,
				ConfigEditLand = this.ConfigEditLand,
				ConfigEditSupplier = this.ConfigEditSupplier,

				Project = this.Project,
				AddExternalProject = this.AddExternalProject,
				AddInternalProject = this.AddInternalProject,
				UpdateExternalProject = this.UpdateExternalProject,
				UpdateInternalProject = this.UpdateInternalProject,
				ViewExternalProject = this.ViewExternalProject,
				ViewInternalProject = this.ViewInternalProject,
				DeleteExternalProject = this.DeleteExternalProject,
				DeleteInternalProject = this.DeleteInternalProject,

				Article = this.Article,
				Suppliers = this.Suppliers,
				Units = this.Units,
				// -

				Receptions = this.Receptions,
				ReceptionsEdit = this.Receptions && this.ReceptionsEdit,
				ReceptionsView = this.Receptions && this.ReceptionsView,
				// - 
				Budget = this.Budget,
				CashLiquidity = this.CashLiquidity,
				CreditManagement = this.CreditManagement,

				//-
				CommandeExternalEditAllGroup = this.Commande && (this.CommandeExternalEditAllGroup ?? false),
				CommandeExternalEditAllSite = this.Commande && (this.CommandeExternalEditAllSite ?? false),
				CommandeExternalViewAllGroup = this.Commande && (this.CommandeExternalViewAllGroup ?? false),
				CommandeExternalViewAllSite = this.Commande && (this.CommandeExternalViewAllSite ?? false),
				CommandeInternalEditAllDepartment = this.Commande && (this.CommandeInternalEditAllDepartment ?? false),
				CommandeInternalEditAllGroup = this.Commande && (this.CommandeInternalEditAllGroup ?? false),
				CommandeInternalEditAllSite = this.Commande && (this.CommandeInternalEditAllSite ?? false),
				CommandeInternalViewAllDepartment = this.Commande && (this.CommandeInternalViewAllDepartment ?? false),
				CommandeInternalViewAllGroup = this.Commande && (this.CommandeInternalViewAllGroup ?? false),
				CommandeInternalViewAllSite = this.Commande && (this.CommandeInternalViewAllSite ?? false),
				// - 
				CommandeExternalViewInvoice = this.Commande && (this.CommandeExternalViewInvoice ?? false),
				CommandeExternalViewInvoiceAllGroup = this.Commande && (this.CommandeExternalViewInvoiceAllGroup ?? false),
				CommandeInternalViewInvoice = this.Commande && (this.CommandeInternalViewInvoice ?? false),
				CommandeInternalViewInvoiceAllGroup = this.Commande && (this.CommandeInternalViewInvoiceAllGroup ?? false),
				//-
				ProjectExternalEditAllGroup = this.ProjectExternalEditAllGroup,
				ProjectExternalEditAllSite = this.ProjectExternalEditAllSite,
				ProjectExternalViewAllGroup = this.ProjectExternalViewAllGroup,
				ProjectExternalViewAllSite = this.ProjectExternalViewAllSite,
				ProjectInternalEditAllGroup = this.ProjectInternalEditAllGroup,
				ProjectInternalEditAllSite = this.ProjectInternalEditAllSite,
				ProjectInternalViewAllGroup = this.ProjectInternalViewAllGroup,
				ProjectInternalViewAllSite = this.ProjectInternalViewAllSite
			};
		}
	}
}
