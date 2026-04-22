using System.Collections.Generic;

namespace Psz.Core.Identity.Models
{
	public class FNC_AccessProfileModel
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
		//-
		public bool ViewLiquiditatsplanungSkontozahlerAccounting { get; set; }
		public bool ViewRechnungsTransferAccounting { get; set; }
		public bool ViewKontenrahmenAccounting { get; set; }
		public bool AddKontenrahmenAccounting { get; set; }
		public bool UpdateKontenrahmenAccounting { get; set; }
		public bool DeleteKontenrahmenAccounting { get; set; }
		public bool ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting { get; set; }
		public bool ViewZahlungskonditionenKundenAccounting { get; set; }
		public bool UpdateZahlungskonditionenKundenAccounting { get; set; }
		public bool DeleteZahlungskonditionenKundenAccounting { get; set; }
		public bool ViewZahlungskonditionenLieferantenAccounting { get; set; }
		public bool UpdateZahlungskonditionenLieferantenAccounting { get; set; }
		public bool DeleteZahlungskonditionenLieferantenAccounting { get; set; }
		public bool ViewFactoringRgGutschriftlistAccounting { get; set; }
		public bool ViewAusfuhrAccounting { get; set; }
		public bool ViewStammdatenkontrolleWareneingangeAccounting { get; set; }
		public bool ViewEinfuhrAccounting { get; set; }
		public bool ViewRMDCZAccounting { get; set; }
		public bool Accounting { get; set; }
		//-
		public bool Statistics { get; set; }
		public bool StatisticsViewAll { get; set; }
		public FNC_AccessProfileModel()
		{

		}

		public FNC_AccessProfileModel(int mainAccessProfileId, Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity entity)
		{
			ID = entity.Id;
			MainAccessProfileId = entity.MainAccessProfileId ?? -1;
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

			//-
			Accounting = entity.Accounting ?? false;
			ViewLiquiditatsplanungSkontozahlerAccounting = entity.ViewLiquiditatsplanungSkontozahlerAccounting ?? false;
			ViewRechnungsTransferAccounting = entity.ViewRechnungsTransferAccounting ?? false;
			ViewKontenrahmenAccounting = entity.ViewKontenrahmenAccounting ?? false;
			AddKontenrahmenAccounting = entity.AddKontenrahmenAccounting ?? false;
			UpdateKontenrahmenAccounting = entity.UpdateKontenrahmenAccounting ?? false;
			DeleteKontenrahmenAccounting = entity.DeleteKontenrahmenAccounting ?? false;
			ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting = entity.ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting ?? false;
			ViewZahlungskonditionenKundenAccounting = entity.ViewZahlungskonditionenKundenAccounting ?? false;
			UpdateZahlungskonditionenKundenAccounting = entity.UpdateZahlungskonditionenKundenAccounting ?? false;
			DeleteZahlungskonditionenKundenAccounting = entity.DeleteZahlungskonditionenKundenAccounting ?? false;
			ViewZahlungskonditionenLieferantenAccounting = entity.ViewZahlungskonditionenLieferantenAccounting ?? false;
			UpdateZahlungskonditionenLieferantenAccounting = entity.UpdateZahlungskonditionenLieferantenAccounting ?? false;
			DeleteZahlungskonditionenLieferantenAccounting = entity.DeleteZahlungskonditionenLieferantenAccounting ?? false;
			ViewFactoringRgGutschriftlistAccounting = entity.ViewFactoringRgGutschriftlistAccounting ?? false;
			ViewAusfuhrAccounting = entity.ViewAusfuhrAccounting ?? false;
			ViewStammdatenkontrolleWareneingangeAccounting = entity.ViewStammdatenkontrolleWareneingangeAccounting ?? false;
			ViewEinfuhrAccounting = entity.ViewEinfuhrAccounting ?? false;
			ViewRMDCZAccounting = entity.ViewRMDCZAccounting ?? false;
			//-
			Statistics = entity.Statistics ?? false;
			StatisticsViewAll = entity.StatisticsViewAll ?? false;
		}
		public FNC_AccessProfileModel(List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity> entities)
		{
			if(entities == null || entities.Count <= 0)
				return;

			//ID = entity.Id;
			//MainAccessProfileId = entity.MainAccessProfileId ?? -1;

			foreach(var entity in entities)
			{
				ModuleActivated = ModuleActivated || (entity.ModuleActivated ?? false);

				Administration = Administration || (entity.Administration ?? false);
				AdministrationAccessProfiles = AdministrationAccessProfiles || (entity.AdministrationAccessProfiles ?? false);
				AdministrationAccessProfilesUpdate = AdministrationAccessProfilesUpdate || (entity.Administration ?? false && (entity.AdministrationAccessProfilesUpdate ?? false));
				AdministrationUser = AdministrationUser || ((entity.Administration ?? false) && (entity.AdministrationUser ?? false));
				AdministrationUserUpdate = AdministrationUserUpdate || ((entity.Administration ?? false) && (entity.AdministrationUserUpdate ?? false));
				//-
				Assign = Assign || (entity.Assign ?? false);
				AssignAllDepartments = AssignAllDepartments || (entity.AssignAllDepartments ?? false);
				AssignAllEmployees = AssignAllEmployees || (entity.AssignAllEmployees ?? false);
				AssignAllSites = AssignAllSites || (entity.AssignAllSites ?? false);
				AssignCreateDept = AssignCreateDept || ((entity.Assign ?? false) && (entity.AssignCreateDept ?? false));
				AssignCreateLand = AssignCreateLand || ((entity.Assign ?? false) && (entity.AssignCreateLand ?? false));
				AssignCreateUser = AssignCreateUser || ((entity.Assign ?? false) && (entity.AssignCreateUser ?? false));
				AssignDeleteDept = AssignDeleteDept || ((entity.Assign ?? false) && (entity.AssignDeleteDept ?? false));
				AssignDeleteLand = AssignDeleteLand || ((entity.Assign ?? false) && (entity.AssignDeleteLand ?? false));
				AssignDeleteUser = AssignDeleteUser || ((entity.Assign ?? false) && (entity.AssignDeleteUser ?? false));
				AssignEditDept = AssignEditDept || ((entity.Assign ?? false) && (entity.AssignEditDept ?? false));
				AssignEditLand = AssignEditLand || ((entity.Assign ?? false) && (entity.AssignEditLand ?? false));
				AssignEditUser = AssignEditUser || ((entity.Assign ?? false) && (entity.AssignEditUser ?? false));
				AssignViewDept = AssignViewDept || ((entity.Assign ?? false) && (entity.AssignViewDept ?? false));
				AssignViewLand = AssignViewLand || ((entity.Assign ?? false) && (entity.AssignViewLand ?? false));
				AssignViewUser = AssignViewUser || ((entity.Assign ?? false) && (entity.AssignViewUser ?? false));
				//-
				Commande = Commande || (entity.Commande ?? false);
				AddExternalCommande = AddExternalCommande || ((entity.Commande ?? false) && (entity.AddExternalCommande ?? false));
				AddInternalCommande = AddInternalCommande || ((entity.Commande ?? false) && (entity.AddInternalCommande ?? false));
				DeleteExternalCommande = DeleteExternalCommande || ((entity.Commande ?? false) && (entity.DeleteExternalCommande ?? false));
				DeleteInternalCommande = DeleteInternalCommande || ((entity.Commande ?? false) && (entity.DeleteInternalCommande ?? false));
				UpdateExternalCommande = UpdateExternalCommande || ((entity.Commande ?? false) && (entity.UpdateExternalCommande ?? false));
				UpdateInternalCommande = UpdateInternalCommande || ((entity.Commande ?? false) && (entity.UpdateInternalCommande ?? false));
				ViewExternalCommande = ViewExternalCommande || ((entity.Commande ?? false) && (entity.ViewExternalCommande ?? false));
				ViewInternalCommande = ViewInternalCommande || ((entity.Commande ?? false) && (entity.ViewInternalCommande ?? false));
				//-
				Config = Config || (entity.Config ?? false);
				ConfigCreateArtikel = ConfigCreateArtikel || (entity.ConfigCreateArtikel ?? false);
				ConfigCreateDept = ConfigCreateDept || (entity.ConfigCreateDept ?? false);
				ConfigCreateLand = ConfigCreateLand || (entity.ConfigCreateLand ?? false);
				ConfigCreateSupplier = ConfigCreateSupplier || (entity.ConfigCreateSupplier ?? false);
				ConfigDeleteArtikel = ConfigDeleteArtikel || (entity.ConfigDeleteArtikel ?? false);
				ConfigDeleteDept = ConfigDeleteDept || (entity.ConfigDeleteDept ?? false);
				ConfigDeleteLand = ConfigDeleteLand || (entity.ConfigDeleteLand ?? false);
				ConfigDeleteSupplier = ConfigDeleteSupplier || (entity.ConfigDeleteSupplier ?? false);
				ConfigEditArtikel = ConfigEditArtikel || (entity.ConfigEditArtikel ?? false);
				ConfigEditDept = ConfigEditDept || (entity.ConfigEditDept ?? false);
				ConfigEditLand = ConfigEditLand || (entity.ConfigEditLand ?? false);
				ConfigEditSupplier = ConfigEditSupplier || (entity.ConfigEditSupplier ?? false);
				//-
				Project = Project || (entity.Project ?? false);
				AddExternalProject = AddExternalProject || (entity.AddExternalProject ?? false);
				AddInternalProject = AddInternalProject || (entity.AddInternalProject ?? false);
				UpdateExternalProject = UpdateExternalProject || (entity.UpdateExternalProject ?? false);
				UpdateInternalProject = UpdateInternalProject || (entity.UpdateInternalProject ?? false);
				ViewExternalProject = ViewExternalProject || (entity.ViewExternalProject ?? false);
				ViewInternalProject = ViewInternalProject || (entity.ViewInternalProject ?? false);
				DeleteExternalProject = DeleteExternalProject || (entity.DeleteExternalProject ?? false);
				DeleteInternalProject = DeleteInternalProject || (entity.DeleteInternalProject ?? false);
				//-
				Article = Article || (entity.Article ?? false);
				Suppliers = Suppliers || (entity.Suppliers ?? false);
				Units = Units || (entity.Units ?? false);
				// -
				Receptions = Receptions || (entity.Receptions ?? false);
				ReceptionsEdit = ReceptionsEdit || ((entity.Receptions ?? false) && (entity.ReceptionsEdit ?? false));
				ReceptionsView = ReceptionsView || ((entity.Receptions ?? false) && (entity.ReceptionsView ?? false));
				// -
				Budget = Budget || (entity.Budget);
				CashLiquidity = CashLiquidity || (entity.CashLiquidity);
				CreditManagement = CreditManagement || (entity.CreditManagement);
				//-
				CommandeExternalEditAllGroup = (CommandeExternalEditAllGroup ?? false) || ((entity.Commande ?? false) && (entity.CommandeExternalEditAllGroup ?? false));
				CommandeExternalEditAllSite = (CommandeExternalEditAllSite ?? false) || ((entity.Commande ?? false) && (entity.CommandeExternalEditAllSite ?? false));
				CommandeExternalViewAllGroup = (CommandeExternalViewAllGroup ?? false) || ((entity.Commande ?? false) && (entity.CommandeExternalViewAllGroup ?? false));
				CommandeExternalViewAllSite = (CommandeExternalViewAllSite ?? false) || ((entity.Commande ?? false) && (entity.CommandeExternalViewAllSite ?? false));
				CommandeInternalEditAllDepartment = (CommandeInternalEditAllDepartment ?? false) || ((entity.Commande ?? false) && (entity.CommandeInternalEditAllDepartment ?? false));
				CommandeInternalEditAllGroup = (CommandeInternalEditAllGroup ?? false) || ((entity.Commande ?? false) && (entity.CommandeInternalEditAllGroup ?? false));
				CommandeInternalEditAllSite = (CommandeInternalEditAllSite ?? false) || ((entity.Commande ?? false) && (entity.CommandeInternalEditAllSite ?? false));
				CommandeInternalViewAllDepartment = (CommandeInternalViewAllDepartment ?? false) || ((entity.Commande ?? false) && (entity.CommandeInternalViewAllDepartment ?? false));
				CommandeInternalViewAllGroup = (CommandeInternalViewAllGroup ?? false) || ((entity.Commande ?? false) && (entity.CommandeInternalViewAllGroup ?? false));
				CommandeInternalViewAllSite = (CommandeInternalViewAllSite ?? false) || ((entity.Commande ?? false) && (entity.CommandeInternalViewAllSite ?? false));
				//-
				ProjectExternalEditAllGroup = (ProjectExternalEditAllGroup ?? false) || ((entity.Project ?? false) && (entity.ProjectExternalEditAllGroup ?? false));
				ProjectExternalEditAllSite = (ProjectExternalEditAllSite ?? false) || ((entity.Project ?? false) && (entity.ProjectExternalEditAllSite ?? false));
				ProjectExternalViewAllGroup = (ProjectExternalViewAllGroup ?? false) || ((entity.Project ?? false) && (entity.ProjectExternalViewAllGroup ?? false));
				ProjectExternalViewAllSite = (ProjectExternalViewAllSite ?? false) || ((entity.Project ?? false) && (entity.ProjectExternalViewAllSite ?? false));
				ProjectInternalEditAllGroup = (ProjectInternalEditAllGroup ?? false) || ((entity.Project ?? false) && (entity.ProjectInternalEditAllGroup ?? false));
				ProjectInternalEditAllSite = (ProjectInternalEditAllSite ?? false) || ((entity.Project ?? false) && (entity.ProjectInternalEditAllSite ?? false));
				ProjectInternalViewAllGroup = (ProjectInternalViewAllGroup ?? false) || ((entity.Project ?? false) && (entity.ProjectInternalViewAllGroup ?? false));
				ProjectInternalViewAllSite = (ProjectInternalViewAllSite ?? false) || ((entity.Project ?? false) && (entity.ProjectInternalViewAllSite ?? false));
				//-
				Accounting = Accounting || (entity.Accounting ?? false);
				ViewLiquiditatsplanungSkontozahlerAccounting = ViewLiquiditatsplanungSkontozahlerAccounting || (entity.ViewLiquiditatsplanungSkontozahlerAccounting ?? false);
				ViewRechnungsTransferAccounting = ViewRechnungsTransferAccounting || (entity.ViewRechnungsTransferAccounting ?? false);
				ViewKontenrahmenAccounting = ViewKontenrahmenAccounting || (entity.ViewKontenrahmenAccounting ?? false);
				AddKontenrahmenAccounting = AddKontenrahmenAccounting || (entity.AddKontenrahmenAccounting ?? false);
				UpdateKontenrahmenAccounting = UpdateKontenrahmenAccounting || (entity.UpdateKontenrahmenAccounting ?? false);
				DeleteKontenrahmenAccounting = DeleteKontenrahmenAccounting || (entity.DeleteKontenrahmenAccounting ?? false);
				ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting = ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting || (entity.ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting ?? false);
				ViewZahlungskonditionenKundenAccounting = ViewZahlungskonditionenKundenAccounting || (entity.ViewZahlungskonditionenKundenAccounting ?? false);
				UpdateZahlungskonditionenKundenAccounting = UpdateZahlungskonditionenKundenAccounting || (entity.UpdateZahlungskonditionenKundenAccounting ?? false);
				DeleteZahlungskonditionenKundenAccounting = DeleteZahlungskonditionenKundenAccounting || (entity.DeleteZahlungskonditionenKundenAccounting ?? false);
				ViewZahlungskonditionenLieferantenAccounting = ViewZahlungskonditionenLieferantenAccounting || (entity.ViewZahlungskonditionenLieferantenAccounting ?? false);
				UpdateZahlungskonditionenLieferantenAccounting = UpdateZahlungskonditionenLieferantenAccounting || (entity.UpdateZahlungskonditionenLieferantenAccounting ?? false);
				DeleteZahlungskonditionenLieferantenAccounting = DeleteZahlungskonditionenLieferantenAccounting || (entity.DeleteZahlungskonditionenLieferantenAccounting ?? false);
				ViewFactoringRgGutschriftlistAccounting = ViewFactoringRgGutschriftlistAccounting || (entity.ViewFactoringRgGutschriftlistAccounting ?? false);
				ViewAusfuhrAccounting = ViewAusfuhrAccounting || (entity.ViewAusfuhrAccounting ?? false);
				ViewStammdatenkontrolleWareneingangeAccounting = ViewStammdatenkontrolleWareneingangeAccounting || (entity.ViewStammdatenkontrolleWareneingangeAccounting ?? false);
				ViewEinfuhrAccounting = ViewEinfuhrAccounting || (entity.ViewEinfuhrAccounting ?? false);
				ViewRMDCZAccounting = ViewRMDCZAccounting || (entity.ViewRMDCZAccounting ?? false);
				//-
				Statistics = Statistics || (entity.Statistics ?? false);
				StatisticsViewAll = StatisticsViewAll || (entity.StatisticsViewAll ?? false);

			}
		}
		public Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity ToDbEntity(int id, int mainAccessProfileId)
		{
			if(!this.ModuleActivated)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity()
				{
					Id = id,
					MainAccessProfileId = mainAccessProfileId,
					ModuleActivated = false,
				};
			}

			return new Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity()
			{
				Id = id,
				MainAccessProfileId = mainAccessProfileId,
				ModuleActivated = true,

				Administration = this.Administration,
				AdministrationAccessProfiles = this.AdministrationAccessProfiles,
				AdministrationAccessProfilesUpdate = this.AdministrationAccessProfilesUpdate,
				AdministrationUser = this.AdministrationUser,
				AdministrationUserUpdate = this.AdministrationUserUpdate,
				Assign = this.Assign,
				AssignAllDepartments = this.AssignAllDepartments,
				AssignAllEmployees = this.AssignAllEmployees,
				AssignAllSites = this.AssignAllSites,
				AssignCreateDept = this.AssignCreateDept,
				AssignCreateLand = this.AssignCreateLand,
				AssignCreateUser = this.AssignCreateUser,
				AssignDeleteDept = this.AssignDeleteDept,
				AssignDeleteLand = this.AssignDeleteLand,
				AssignDeleteUser = this.AssignDeleteUser,
				AssignEditDept = this.AssignEditDept,
				AssignEditLand = this.AssignEditLand,
				AssignEditUser = this.AssignEditUser,
				AssignViewDept = this.AssignViewDept,
				AssignViewLand = this.AssignViewLand,
				AssignViewUser = this.AssignViewUser,
				Config = this.Config,
				Units = this.Units,
				ConfigCreateDept = this.ConfigCreateDept,
				ConfigCreateLand = this.ConfigCreateLand,
				ConfigDeleteDept = this.ConfigDeleteDept,
				ConfigDeleteLand = this.ConfigDeleteLand,
				ConfigEditDept = this.ConfigEditDept,
				ConfigEditLand = this.ConfigEditLand,
				ConfigCreateArtikel = this.ConfigCreateArtikel,
				ConfigEditArtikel = this.ConfigEditArtikel,
				ConfigDeleteArtikel = this.ConfigDeleteArtikel,
				Suppliers = this.Suppliers,
				ConfigCreateSupplier = this.ConfigCreateSupplier,
				ConfigEditSupplier = this.ConfigEditSupplier,
				ConfigDeleteSupplier = this.ConfigDeleteSupplier,
				Project = this.Project,
				ViewInternalProject = this.ViewInternalProject,
				AddInternalProject = this.AddInternalProject,
				UpdateInternalProject = this.UpdateInternalProject,
				DeleteInternalProject = this.DeleteInternalProject,
				ViewExternalProject = this.ViewExternalProject,
				AddExternalProject = this.AddExternalProject,
				UpdateExternalProject = this.UpdateExternalProject,
				DeleteExternalProject = this.DeleteExternalProject,
				Commande = this.Commande,
				ViewInternalCommande = this.ViewInternalCommande,
				AddInternalCommande = this.AddInternalCommande,
				UpdateInternalCommande = this.UpdateInternalCommande,
				DeleteInternalCommande = this.DeleteInternalCommande,
				ViewExternalCommande = this.ViewExternalCommande,
				AddExternalCommande = this.AddExternalCommande,
				UpdateExternalCommande = this.UpdateExternalCommande,
				DeleteExternalCommande = this.DeleteExternalCommande,
				//-
				Receptions = this.Receptions,
				ReceptionsEdit = this.ReceptionsEdit,
				ReceptionsView = this.ReceptionsView,
				//- 
				Budget = this.Budget,
				CashLiquidity = this.CashLiquidity,
				CreditManagement = this.CreditManagement,
				//-
				CommandeExternalEditAllGroup = this.CommandeExternalEditAllGroup,
				CommandeExternalEditAllSite = this.CommandeExternalEditAllSite,
				CommandeExternalViewAllGroup = this.CommandeExternalViewAllGroup,
				CommandeExternalViewAllSite = this.CommandeExternalViewAllSite,
				CommandeInternalEditAllDepartment = this.CommandeInternalEditAllDepartment,
				CommandeInternalEditAllGroup = this.CommandeInternalEditAllGroup,
				CommandeInternalEditAllSite = this.CommandeInternalEditAllSite,
				CommandeInternalViewAllDepartment = this.CommandeInternalViewAllDepartment,
				CommandeInternalViewAllGroup = this.CommandeInternalViewAllGroup,
				CommandeInternalViewAllSite = this.CommandeInternalViewAllSite,
				//-
				ProjectExternalEditAllGroup = this.ProjectExternalEditAllGroup,
				ProjectExternalEditAllSite = this.ProjectExternalEditAllSite,
				ProjectExternalViewAllGroup = this.ProjectExternalViewAllGroup,
				ProjectExternalViewAllSite = this.ProjectExternalViewAllSite,
				ProjectInternalEditAllGroup = this.ProjectInternalEditAllGroup,
				ProjectInternalEditAllSite = this.ProjectInternalEditAllSite,
				ProjectInternalViewAllGroup = this.ProjectInternalViewAllGroup,
				ProjectInternalViewAllSite = this.ProjectInternalViewAllSite,
				//-
				Accounting = this.Accounting,
				ViewLiquiditatsplanungSkontozahlerAccounting = this.ViewLiquiditatsplanungSkontozahlerAccounting,
				ViewRechnungsTransferAccounting = this.ViewRechnungsTransferAccounting,
				ViewKontenrahmenAccounting = this.ViewKontenrahmenAccounting,
				AddKontenrahmenAccounting = this.AddKontenrahmenAccounting,
				UpdateKontenrahmenAccounting = this.UpdateKontenrahmenAccounting,
				DeleteKontenrahmenAccounting = this.DeleteKontenrahmenAccounting,
				ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting = this.ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting,
				ViewZahlungskonditionenKundenAccounting = this.ViewZahlungskonditionenKundenAccounting,
				UpdateZahlungskonditionenKundenAccounting = this.UpdateZahlungskonditionenKundenAccounting,
				DeleteZahlungskonditionenKundenAccounting = this.DeleteZahlungskonditionenKundenAccounting,
				ViewZahlungskonditionenLieferantenAccounting = this.ViewZahlungskonditionenLieferantenAccounting,
				UpdateZahlungskonditionenLieferantenAccounting = this.UpdateZahlungskonditionenLieferantenAccounting,
				DeleteZahlungskonditionenLieferantenAccounting = this.DeleteZahlungskonditionenLieferantenAccounting,
				ViewFactoringRgGutschriftlistAccounting = this.ViewFactoringRgGutschriftlistAccounting,
				ViewAusfuhrAccounting = this.ViewAusfuhrAccounting,
				ViewStammdatenkontrolleWareneingangeAccounting = this.ViewStammdatenkontrolleWareneingangeAccounting,
				ViewEinfuhrAccounting = this.ViewEinfuhrAccounting,
				ViewRMDCZAccounting = this.ViewRMDCZAccounting,
				//-
				Statistics = this.Statistics,
				StatisticsViewAll = this.StatisticsViewAll
			};
		}
	}
	public class FNC_AccessProfileMinimalModel
	{
		public bool Administration { get; set; }
		public bool Assign { get; set; }
		public bool Commande { get; set; }
		public bool Config { get; set; }
		public int ID { get; set; }
		public int MainAccessProfileId { get; set; }
		public bool ModuleActivated { get; set; }
		public bool Project { get; set; }
		public bool Receptions { get; set; }
		// -
		public bool Budget { get; set; }
		public bool CashLiquidity { get; set; }
		public bool CreditManagement { get; set; }
		public bool Accounting { get; set; }

		public FNC_AccessProfileMinimalModel()
		{

		}

		public FNC_AccessProfileMinimalModel(FNC_AccessProfileModel entity)
		{
			ID = entity.ID;
			MainAccessProfileId = entity?.MainAccessProfileId ?? -1;
			ModuleActivated = entity?.ModuleActivated ?? false;
			//-
			Administration = entity?.Administration ?? false;
			//-
			Assign = entity?.Assign ?? false;
			Commande = entity?.Commande ?? false;
			Config = entity?.Config ?? false;
			Project = entity?.Project ?? false;
			// -
			Receptions = entity?.Receptions ?? false;
			// - 
			Budget = entity?.Budget ?? false;
			CashLiquidity = entity?.CashLiquidity ?? false;
			CreditManagement = entity?.CreditManagement ?? false;
			//- 
			Accounting = entity?.Accounting ?? false;
		}

		public FNC_AccessProfileMinimalModel(int mainAccessProfileId, Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity entity)
		{
			ID = entity.Id;
			MainAccessProfileId = entity.MainAccessProfileId ?? -1;
			ModuleActivated = entity.ModuleActivated ?? false;
			Administration = entity.Administration ?? false;
			Assign = entity.Assign ?? false;
			Commande = entity.Commande ?? false;
			Config = entity.Config ?? false;
			Project = entity.Project ?? false;
			// -
			Receptions = entity.Receptions ?? false;
			// - 
			Budget = entity.Budget;
			CashLiquidity = entity.CashLiquidity;
			CreditManagement = entity.CreditManagement;
			//-
			Accounting = entity?.Accounting ?? false;
		}
		public Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity ToDbEntity(int id, int mainAccessProfileId)
		{
			if(!this.ModuleActivated)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity()
				{
					Id = id,
					MainAccessProfileId = mainAccessProfileId,
					ModuleActivated = false,
				};
			}

			return new Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity()
			{
				Id = id,
				MainAccessProfileId = mainAccessProfileId,
				ModuleActivated = true,
				Administration = this.Administration,
				AdministrationAccessProfiles = this.Administration,
				AdministrationAccessProfilesUpdate = this.Administration,
				AdministrationUser = this.Administration,
				AdministrationUserUpdate = this.Administration,
				//-
				Assign = this.Assign,
				Config = this.Config,
				Project = this.Project,
				Commande = this.Commande,
				//-
				Receptions = this.Receptions,
				//- 
				Budget = this.Budget,
				CashLiquidity = this.CashLiquidity,
				CreditManagement = this.CreditManagement,
				//-
				Accounting = this.Accounting,
			};
		}
	}
}