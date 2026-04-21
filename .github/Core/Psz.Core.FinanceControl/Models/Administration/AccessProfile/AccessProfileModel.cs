using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Models.Administration.AccessProfile
{
	public class AccessProfileModel
	{
		public string AccessProfileName { get; set; }
		public bool? AddExternalCommande { get; set; }
		public bool? AddExternalProject { get; set; }
		public bool? AddInternalCommande { get; set; }
		public bool? AddInternalProject { get; set; }
		public bool? AddKontenrahmenAccounting { get; set; }
		public bool? Administration { get; set; }
		public bool? AdministrationAccessProfiles { get; set; }
		public bool? AdministrationAccessProfilesUpdate { get; set; }
		public bool? AdministrationUser { get; set; }
		public bool? AdministrationUserUpdate { get; set; }
		public bool? Article { get; set; }
		public bool? Assign { get; set; }
		public bool? AssignAllDepartments { get; set; }
		public bool? AssignAllEmployees { get; set; }
		public bool? AssignAllSites { get; set; }
		public bool? AssignCreateDept { get; set; }
		public bool? AssignCreateLand { get; set; }
		public bool? AssignCreateUser { get; set; }
		public bool? AssignDeleteDept { get; set; }
		public bool? AssignDeleteLand { get; set; }
		public bool? AssignDeleteUser { get; set; }
		public bool? AssignEditDept { get; set; }
		public bool? AssignEditLand { get; set; }
		public bool? AssignEditUser { get; set; }
		public bool? AssignViewDept { get; set; }
		public bool? AssignViewLand { get; set; }
		public bool? AssignViewUser { get; set; }
		public bool Budget { get; set; }
		public bool CashLiquidity { get; set; }
		public bool? Commande { get; set; }
		public bool? CommandeExternalEditAllGroup { get; set; }
		public bool? CommandeExternalEditAllSite { get; set; }
		public bool? CommandeExternalViewAllGroup { get; set; }
		public bool? CommandeExternalViewAllSite { get; set; }
		public bool? CommandeExternalViewInvoice { get; set; }
		public bool? CommandeExternalViewInvoiceAllGroup { get; set; }
		public bool? CommandeInternalEditAllDepartment { get; set; }
		public bool? CommandeInternalEditAllGroup { get; set; }
		public bool? CommandeInternalEditAllSite { get; set; }
		public bool? CommandeInternalViewAllDepartment { get; set; }
		public bool? CommandeInternalViewAllGroup { get; set; }
		public bool? CommandeInternalViewAllSite { get; set; }
		public bool? CommandeInternalViewInvoice { get; set; }
		public bool? CommandeInternalViewInvoiceAllGroup { get; set; }
		public bool? Config { get; set; }
		public bool? ConfigCreateArtikel { get; set; }
		public bool? ConfigCreateDept { get; set; }
		public bool? ConfigCreateLand { get; set; }
		public bool? ConfigCreateSupplier { get; set; }
		public bool? ConfigDeleteArtikel { get; set; }
		public bool? ConfigDeleteDept { get; set; }
		public bool? ConfigDeleteLand { get; set; }
		public bool? ConfigDeleteSupplier { get; set; }
		public bool? ConfigEditArtikel { get; set; }
		public bool? ConfigEditDept { get; set; }
		public bool? ConfigEditLand { get; set; }
		public bool? ConfigEditSupplier { get; set; }
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public bool CreditManagement { get; set; }
		public bool? DeleteExternalCommande { get; set; }
		public bool? DeleteExternalProject { get; set; }
		public bool? DeleteInternalCommande { get; set; }
		public bool? DeleteInternalProject { get; set; }
		public bool? DeleteKontenrahmenAccounting { get; set; }
		public bool? DeleteZahlungskonditionenKundenAccounting { get; set; }
		public bool? DeleteZahlungskonditionenLieferantenAccounting { get; set; }
		public int Id { get; set; }
		public bool? IsDefault { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public int? MainAccessProfileId { get; set; }
		public bool? ModuleActivated { get; set; }
		public bool? Project { get; set; }
		public bool? ProjectExternalEditAllGroup { get; set; }
		public bool? ProjectExternalEditAllSite { get; set; }
		public bool? ProjectExternalViewAllGroup { get; set; }
		public bool? ProjectExternalViewAllSite { get; set; }
		public bool? ProjectInternalEditAllGroup { get; set; }
		public bool? ProjectInternalEditAllSite { get; set; }
		public bool? ProjectInternalViewAllGroup { get; set; }
		public bool? ProjectInternalViewAllSite { get; set; }
		public bool? Receptions { get; set; }
		public bool? ReceptionsEdit { get; set; }
		public bool? ReceptionsView { get; set; }
		public bool? Suppliers { get; set; }
		public bool? Units { get; set; }
		public bool? UpdateExternalCommande { get; set; }
		public bool? UpdateExternalProject { get; set; }
		public bool? UpdateInternalCommande { get; set; }
		public bool? UpdateInternalProject { get; set; }
		public bool? UpdateKontenrahmenAccounting { get; set; }
		public bool? UpdateZahlungskonditionenKundenAccounting { get; set; }
		public bool? UpdateZahlungskonditionenLieferantenAccounting { get; set; }
		public bool? ViewAusfuhrAccounting { get; set; }
		public bool? ViewEinfuhrAccounting { get; set; }
		public bool? ViewExternalCommande { get; set; }
		public bool? ViewExternalProject { get; set; }
		public bool? ViewFactoringRgGutschriftlistAccounting { get; set; }
		public bool? ViewInternalCommande { get; set; }
		public bool? ViewInternalProject { get; set; }
		public bool? ViewKontenrahmenAccounting { get; set; }
		public bool? ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting { get; set; }
		public bool? ViewLiquiditatsplanungSkontozahlerAccounting { get; set; }
		public bool? ViewRechnungsTransferAccounting { get; set; }
		public bool? ViewRMDCZAccounting { get; set; }
		public bool? ViewStammdatenkontrolleWareneingangeAccounting { get; set; }
		public bool? ViewZahlungskonditionenKundenAccounting { get; set; }
		public bool? ViewZahlungskonditionenLieferantenAccounting { get; set; }
		public bool? Accounting { get; set; }
		//-
		public bool? Statistics { get; set; }
		public bool? StatisticsViewAll { get; set; }
		// - 
		public bool? FinanceOrder { get; set; }
		public bool? FinanceProject { get; set; }

		public AccessProfileModel()
		{

		}

		public AccessProfileModel(Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity accessProfileEntity)
		{
			if(accessProfileEntity == null)
				return;

			AccessProfileName = accessProfileEntity.AccessProfileName;
			AddExternalCommande = accessProfileEntity.AddExternalCommande ?? false;
			AddExternalProject = accessProfileEntity.AddExternalProject ?? false;
			AddInternalCommande = accessProfileEntity.AddInternalCommande ?? false;
			AddInternalProject = accessProfileEntity.AddInternalProject ?? false;
			Administration = accessProfileEntity.Administration ?? false;
			AdministrationAccessProfiles = accessProfileEntity.AdministrationAccessProfiles ?? false;
			AdministrationAccessProfilesUpdate = accessProfileEntity.AdministrationAccessProfilesUpdate ?? false;
			AdministrationUser = accessProfileEntity.AdministrationUser ?? false;
			AdministrationUserUpdate = accessProfileEntity.AdministrationUserUpdate ?? false;
			Article = accessProfileEntity.Article ?? false;
			Assign = accessProfileEntity.Assign ?? false;
			AssignAllDepartments = accessProfileEntity.AssignAllDepartments ?? false;
			AssignAllEmployees = accessProfileEntity.AssignAllEmployees ?? false;
			AssignAllSites = accessProfileEntity.AssignAllSites ?? false;
			AssignCreateDept = accessProfileEntity.AssignCreateDept ?? false;
			AssignCreateLand = accessProfileEntity.AssignCreateLand ?? false;
			AssignCreateUser = accessProfileEntity.AssignCreateUser ?? false;
			AssignDeleteDept = accessProfileEntity.AssignDeleteDept ?? false;
			AssignDeleteLand = accessProfileEntity.AssignDeleteLand ?? false;
			AssignDeleteUser = accessProfileEntity.AssignDeleteUser ?? false;
			AssignEditDept = accessProfileEntity.AssignEditDept ?? false;
			AssignEditLand = accessProfileEntity.AssignEditLand ?? false;
			AssignEditUser = accessProfileEntity.AssignEditUser ?? false;
			AssignViewDept = accessProfileEntity.AssignViewDept ?? false;
			AssignViewLand = accessProfileEntity.AssignViewLand ?? false;
			AssignViewUser = accessProfileEntity.AssignViewUser ?? false;
			CashLiquidity = accessProfileEntity.CashLiquidity;
			Commande = accessProfileEntity.Commande ?? false;
			CommandeExternalEditAllGroup = accessProfileEntity.CommandeExternalEditAllGroup ?? false;
			CommandeExternalEditAllSite = accessProfileEntity.CommandeExternalEditAllSite ?? false;
			CommandeExternalViewAllGroup = accessProfileEntity.CommandeExternalViewAllGroup ?? false;
			CommandeExternalViewAllSite = accessProfileEntity.CommandeExternalViewAllSite ?? false;
			CommandeInternalEditAllDepartment = accessProfileEntity.CommandeInternalEditAllDepartment ?? false;
			CommandeInternalEditAllGroup = accessProfileEntity.CommandeInternalEditAllGroup ?? false;
			CommandeInternalEditAllSite = accessProfileEntity.CommandeInternalEditAllSite ?? false;
			CommandeInternalViewAllDepartment = accessProfileEntity.CommandeInternalViewAllDepartment ?? false;
			CommandeInternalViewAllGroup = accessProfileEntity.CommandeInternalViewAllGroup ?? false;
			CommandeInternalViewAllSite = accessProfileEntity.CommandeInternalViewAllSite ?? false;
			// - 
			CommandeExternalViewInvoice = accessProfileEntity.CommandeExternalViewInvoice ?? false;
			CommandeExternalViewInvoiceAllGroup = accessProfileEntity.CommandeExternalViewInvoiceAllGroup ?? false;
			CommandeInternalViewInvoice = accessProfileEntity.CommandeInternalViewInvoice ?? false;
			CommandeInternalViewInvoiceAllGroup = accessProfileEntity.CommandeInternalViewInvoiceAllGroup ?? false;
			Config = accessProfileEntity.Config ?? false;
			ConfigCreateArtikel = accessProfileEntity.ConfigCreateArtikel ?? false;
			ConfigCreateDept = accessProfileEntity.ConfigCreateDept ?? false;
			ConfigCreateLand = accessProfileEntity.ConfigCreateLand ?? false;
			ConfigCreateSupplier = accessProfileEntity.ConfigCreateSupplier ?? false;
			ConfigDeleteArtikel = accessProfileEntity.ConfigDeleteArtikel ?? false;
			ConfigDeleteDept = accessProfileEntity.ConfigDeleteDept ?? false;
			ConfigDeleteLand = accessProfileEntity.ConfigDeleteLand ?? false;
			ConfigDeleteSupplier = accessProfileEntity.ConfigDeleteSupplier ?? false;
			ConfigEditArtikel = accessProfileEntity.ConfigEditArtikel ?? false;
			ConfigEditDept = accessProfileEntity.ConfigEditDept ?? false;
			ConfigEditLand = accessProfileEntity.ConfigEditLand ?? false;
			ConfigEditSupplier = accessProfileEntity.ConfigEditSupplier ?? false;
			CreationTime = accessProfileEntity.CreationTime ?? new DateTime(2000, 1, 1);
			CreationUserId = accessProfileEntity.CreationUserId ?? -1;
			CreditManagement = accessProfileEntity.CreditManagement;
			DeleteExternalCommande = accessProfileEntity.DeleteExternalCommande ?? false;
			DeleteExternalProject = accessProfileEntity.DeleteExternalProject ?? false;
			DeleteInternalCommande = accessProfileEntity.DeleteInternalCommande ?? false;
			DeleteInternalProject = accessProfileEntity.DeleteInternalProject ?? false;
			Id = accessProfileEntity.Id;
			IsDefault = accessProfileEntity.IsDefault ?? false;
			LastEditTime = accessProfileEntity.LastEditTime ?? new DateTime(2000, 1, 1);
			LastEditUserId = accessProfileEntity.LastEditUserId ?? -1;
			ModuleActivated = accessProfileEntity.ModuleActivated ?? false;
			Project = accessProfileEntity.Project ?? false;
			ProjectExternalEditAllGroup = accessProfileEntity.ProjectExternalEditAllGroup ?? false;
			ProjectExternalEditAllSite = accessProfileEntity.ProjectExternalEditAllSite ?? false;
			ProjectExternalViewAllGroup = accessProfileEntity.ProjectExternalViewAllGroup ?? false;
			ProjectExternalViewAllSite = accessProfileEntity.ProjectExternalViewAllSite ?? false;
			ProjectInternalEditAllGroup = accessProfileEntity.ProjectInternalEditAllGroup ?? false;
			ProjectInternalEditAllSite = accessProfileEntity.ProjectInternalEditAllSite ?? false;
			ProjectInternalViewAllGroup = accessProfileEntity.ProjectInternalViewAllGroup ?? false;
			ProjectInternalViewAllSite = accessProfileEntity.ProjectInternalViewAllSite ?? false;
			Receptions = accessProfileEntity.Receptions ?? false;
			ReceptionsEdit = accessProfileEntity.ReceptionsEdit ?? false;
			ReceptionsView = accessProfileEntity.ReceptionsView ?? false;
			Suppliers = accessProfileEntity.Suppliers ?? false;
			Units = accessProfileEntity.Units ?? false;
			UpdateExternalCommande = accessProfileEntity.UpdateExternalCommande ?? false;
			UpdateExternalProject = accessProfileEntity.UpdateExternalProject ?? false;
			UpdateInternalCommande = accessProfileEntity.UpdateInternalCommande ?? false;
			UpdateInternalProject = accessProfileEntity.UpdateInternalProject ?? false;
			ViewExternalCommande = accessProfileEntity.ViewExternalCommande ?? false;
			ViewExternalProject = accessProfileEntity.ViewExternalProject ?? false;
			ViewInternalCommande = accessProfileEntity.ViewInternalCommande ?? false;
			ViewInternalProject = accessProfileEntity.ViewInternalProject ?? false;
			// Accounting 
			Accounting = accessProfileEntity.Accounting ?? false;
			ViewLiquiditatsplanungSkontozahlerAccounting = accessProfileEntity.ViewLiquiditatsplanungSkontozahlerAccounting ?? false;
			ViewRechnungsTransferAccounting = accessProfileEntity.ViewRechnungsTransferAccounting ?? false;
			ViewKontenrahmenAccounting = accessProfileEntity.ViewKontenrahmenAccounting ?? false;
			AddKontenrahmenAccounting = accessProfileEntity.AddKontenrahmenAccounting ?? false;
			UpdateKontenrahmenAccounting = accessProfileEntity.UpdateKontenrahmenAccounting ?? false;
			DeleteKontenrahmenAccounting = accessProfileEntity.DeleteKontenrahmenAccounting ?? false;
			ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting = accessProfileEntity.ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting ?? false;
			ViewZahlungskonditionenKundenAccounting = accessProfileEntity.ViewZahlungskonditionenKundenAccounting ?? false;
			UpdateZahlungskonditionenKundenAccounting = accessProfileEntity.UpdateZahlungskonditionenKundenAccounting ?? false;
			DeleteZahlungskonditionenKundenAccounting = accessProfileEntity.DeleteZahlungskonditionenKundenAccounting ?? false;
			ViewZahlungskonditionenLieferantenAccounting = accessProfileEntity.ViewZahlungskonditionenLieferantenAccounting ?? false;
			UpdateZahlungskonditionenLieferantenAccounting = accessProfileEntity.UpdateZahlungskonditionenLieferantenAccounting ?? false;
			DeleteZahlungskonditionenLieferantenAccounting = accessProfileEntity.DeleteZahlungskonditionenLieferantenAccounting ?? false;
			ViewFactoringRgGutschriftlistAccounting = accessProfileEntity.ViewFactoringRgGutschriftlistAccounting ?? false;
			ViewAusfuhrAccounting = accessProfileEntity.ViewAusfuhrAccounting ?? false;
			ViewStammdatenkontrolleWareneingangeAccounting = accessProfileEntity.ViewStammdatenkontrolleWareneingangeAccounting ?? false;
			ViewEinfuhrAccounting = accessProfileEntity.ViewEinfuhrAccounting ?? false;
			ViewRMDCZAccounting = accessProfileEntity.ViewRMDCZAccounting ?? false;
			//-
			Statistics = accessProfileEntity.Statistics ?? false;
			StatisticsViewAll = accessProfileEntity.StatisticsViewAll ?? false;
			// - 
			FinanceOrder = accessProfileEntity.FinanceOrder ?? false;
			FinanceProject = accessProfileEntity.FinanceProject ?? false;
		}
		public AccessProfileModel(List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity> accessProfileEntities)
		{
			if(accessProfileEntities == null || accessProfileEntities.Count <= 0)
				return;

			foreach(var accessProfileEntity in accessProfileEntities)
			{
				AddExternalCommande = (AddExternalCommande ?? false) || (accessProfileEntity.AddExternalCommande ?? false);
				AddExternalProject = (AddExternalProject ?? false) || (accessProfileEntity.AddExternalProject ?? false);
				AddInternalCommande = (AddInternalCommande ?? false) || (accessProfileEntity.AddInternalCommande ?? false);
				AddInternalProject = (AddInternalProject ?? false) || (accessProfileEntity.AddInternalProject ?? false);
				Administration = (Administration ?? false) || (accessProfileEntity.Administration ?? false);
				AdministrationAccessProfiles = (AdministrationAccessProfiles ?? false) || (accessProfileEntity.AdministrationAccessProfiles ?? false);
				AdministrationAccessProfilesUpdate = (AdministrationAccessProfilesUpdate ?? false) || (accessProfileEntity.AdministrationAccessProfilesUpdate ?? false);
				AdministrationUser = (AdministrationUser ?? false) || (accessProfileEntity.AdministrationUser ?? false);
				AdministrationUserUpdate = (AdministrationUserUpdate ?? false) || (accessProfileEntity.AdministrationUserUpdate ?? false);
				Article = (Article ?? false) || (accessProfileEntity.Article ?? false);
				Assign = (Assign ?? false) || (accessProfileEntity.Assign ?? false);
				AssignAllDepartments = (AssignAllDepartments ?? false) || (accessProfileEntity.AssignAllDepartments ?? false);
				AssignAllEmployees = (AssignAllEmployees ?? false) || (accessProfileEntity.AssignAllEmployees ?? false);
				AssignAllSites = (AssignAllSites ?? false) || (accessProfileEntity.AssignAllSites ?? false);
				AssignCreateDept = (AssignCreateDept ?? false) || (accessProfileEntity.AssignCreateDept ?? false);
				AssignCreateLand = (AssignCreateLand ?? false) || (accessProfileEntity.AssignCreateLand ?? false);
				AssignCreateUser = (AssignCreateUser ?? false) || (accessProfileEntity.AssignCreateUser ?? false);
				AssignDeleteDept = (AssignDeleteDept ?? false) || (accessProfileEntity.AssignDeleteDept ?? false);
				AssignDeleteLand = (AssignDeleteLand ?? false) || (accessProfileEntity.AssignDeleteLand ?? false);
				AssignDeleteUser = (AssignDeleteUser ?? false) || (accessProfileEntity.AssignDeleteUser ?? false);
				AssignEditDept = (AssignEditDept ?? false) || (accessProfileEntity.AssignEditDept ?? false);
				AssignEditLand = (AssignEditLand ?? false) || (accessProfileEntity.AssignEditLand ?? false);
				AssignEditUser = (AssignEditUser ?? false) || (accessProfileEntity.AssignEditUser ?? false);
				AssignViewDept = (AssignViewDept ?? false) || (accessProfileEntity.AssignViewDept ?? false);
				AssignViewLand = (AssignViewLand ?? false) || (accessProfileEntity.AssignViewLand ?? false);
				AssignViewUser = (AssignViewUser ?? false) || (accessProfileEntity.AssignViewUser ?? false);
				CashLiquidity = (CashLiquidity) || (accessProfileEntity.CashLiquidity);
				Commande = (Commande ?? false) || (accessProfileEntity.Commande ?? false);
				CommandeExternalEditAllGroup = (CommandeExternalEditAllGroup ?? false) || (accessProfileEntity.CommandeExternalEditAllGroup ?? false);
				CommandeExternalEditAllSite = (CommandeExternalEditAllSite ?? false) || (accessProfileEntity.CommandeExternalEditAllSite ?? false);
				CommandeExternalViewAllGroup = (CommandeExternalViewAllGroup ?? false) || (accessProfileEntity.CommandeExternalViewAllGroup ?? false);
				CommandeExternalViewAllSite = (CommandeExternalViewAllSite ?? false) || (accessProfileEntity.CommandeExternalViewAllSite ?? false);
				CommandeInternalEditAllDepartment = (CommandeInternalEditAllDepartment ?? false) || (accessProfileEntity.CommandeInternalEditAllDepartment ?? false);
				CommandeInternalEditAllGroup = (CommandeInternalEditAllGroup ?? false) || (accessProfileEntity.CommandeInternalEditAllGroup ?? false);
				CommandeInternalEditAllSite = (CommandeInternalEditAllSite ?? false) || (accessProfileEntity.CommandeInternalEditAllSite ?? false);
				CommandeInternalViewAllDepartment = (CommandeInternalViewAllDepartment ?? false) || (accessProfileEntity.CommandeInternalViewAllDepartment ?? false);
				CommandeInternalViewAllGroup = (CommandeInternalViewAllGroup ?? false) || (accessProfileEntity.CommandeInternalViewAllGroup ?? false);
				CommandeInternalViewAllSite = (CommandeInternalViewAllSite ?? false) || (accessProfileEntity.CommandeInternalViewAllSite ?? false);
				// - 
				CommandeExternalViewInvoice = (CommandeExternalViewInvoice ?? false) || (accessProfileEntity.CommandeExternalViewInvoice ?? false);
				CommandeExternalViewInvoiceAllGroup = (CommandeExternalViewInvoiceAllGroup ?? false) || (accessProfileEntity.CommandeExternalViewInvoiceAllGroup ?? false);
				CommandeInternalViewInvoice = (CommandeInternalViewInvoice ?? false) || (accessProfileEntity.CommandeInternalViewInvoice ?? false);
				CommandeInternalViewInvoiceAllGroup = (CommandeInternalViewInvoiceAllGroup ?? false) || (accessProfileEntity.CommandeInternalViewInvoiceAllGroup ?? false);

				Config = (Config ?? false) || (accessProfileEntity.Config ?? false);
				ConfigCreateArtikel = (ConfigCreateArtikel ?? false) || (accessProfileEntity.ConfigCreateArtikel ?? false);
				ConfigCreateDept = (ConfigCreateDept ?? false) || (accessProfileEntity.ConfigCreateDept ?? false);
				ConfigCreateLand = (ConfigCreateLand ?? false) || (accessProfileEntity.ConfigCreateLand ?? false);
				ConfigCreateSupplier = (ConfigCreateSupplier ?? false) || (accessProfileEntity.ConfigCreateSupplier ?? false);
				ConfigDeleteArtikel = (ConfigDeleteArtikel ?? false) || (accessProfileEntity.ConfigDeleteArtikel ?? false);
				ConfigDeleteDept = (ConfigDeleteDept ?? false) || (accessProfileEntity.ConfigDeleteDept ?? false);
				ConfigDeleteLand = (ConfigDeleteLand ?? false) || (accessProfileEntity.ConfigDeleteLand ?? false);
				ConfigDeleteSupplier = (ConfigDeleteSupplier ?? false) || (accessProfileEntity.ConfigDeleteSupplier ?? false);
				ConfigEditArtikel = (ConfigEditArtikel ?? false) || (accessProfileEntity.ConfigEditArtikel ?? false);
				ConfigEditDept = (ConfigEditDept ?? false) || (accessProfileEntity.ConfigEditDept ?? false);
				ConfigEditLand = (ConfigEditLand ?? false) || (accessProfileEntity.ConfigEditLand ?? false);
				ConfigEditSupplier = (ConfigEditSupplier ?? false) || (accessProfileEntity.ConfigEditSupplier ?? false);
				//CreationTime = (CreationTime ?? false) || (accessProfileEntity.CreationTime ?? new DateTime(2000,1,1));
				//CreationUserId = (CreationUserId ?? false) || (accessProfileEntity.CreationUserId ?? -1);
				CreditManagement = (CreditManagement) || (accessProfileEntity.CreditManagement);
				DeleteExternalCommande = (DeleteExternalCommande ?? false) || (accessProfileEntity.DeleteExternalCommande ?? false);
				DeleteExternalProject = (DeleteExternalProject ?? false) || (accessProfileEntity.DeleteExternalProject ?? false);
				DeleteInternalCommande = (DeleteInternalCommande ?? false) || (accessProfileEntity.DeleteInternalCommande ?? false);
				DeleteInternalProject = (DeleteInternalProject ?? false) || (accessProfileEntity.DeleteInternalProject ?? false);
				//Id = (Id ?? false) || (accessProfileEntity.Id);
				//LastEditTime = (LastEditTime ?? false) || (accessProfileEntity.LastEditTime ?? new DateTime(2000,1,1));
				//LastEditUserId = (LastEditUserId ?? false) || (accessProfileEntity.LastEditUserId ?? -1);
				IsDefault = (IsDefault ?? false) || (accessProfileEntity.IsDefault ?? false);
				ModuleActivated = (ModuleActivated ?? false) || (accessProfileEntity.ModuleActivated ?? false);
				Project = (Project ?? false) || (accessProfileEntity.Project ?? false);
				ProjectExternalEditAllGroup = (ProjectExternalEditAllGroup ?? false) || (accessProfileEntity.ProjectExternalEditAllGroup ?? false);
				ProjectExternalEditAllSite = (ProjectExternalEditAllSite ?? false) || (accessProfileEntity.ProjectExternalEditAllSite ?? false);
				ProjectExternalViewAllGroup = (ProjectExternalViewAllGroup ?? false) || (accessProfileEntity.ProjectExternalViewAllGroup ?? false);
				ProjectExternalViewAllSite = (ProjectExternalViewAllSite ?? false) || (accessProfileEntity.ProjectExternalViewAllSite ?? false);
				ProjectInternalEditAllGroup = (ProjectInternalEditAllGroup ?? false) || (accessProfileEntity.ProjectInternalEditAllGroup ?? false);
				ProjectInternalEditAllSite = (ProjectInternalEditAllSite ?? false) || (accessProfileEntity.ProjectInternalEditAllSite ?? false);
				ProjectInternalViewAllGroup = (ProjectInternalViewAllGroup ?? false) || (accessProfileEntity.ProjectInternalViewAllGroup ?? false);
				ProjectInternalViewAllSite = (ProjectInternalViewAllSite ?? false) || (accessProfileEntity.ProjectInternalViewAllSite ?? false);
				Receptions = (Receptions ?? false) || (accessProfileEntity.Receptions ?? false);
				ReceptionsEdit = (ReceptionsEdit ?? false) || (accessProfileEntity.ReceptionsEdit ?? false);
				ReceptionsView = (ReceptionsView ?? false) || (accessProfileEntity.ReceptionsView ?? false);
				Suppliers = (Suppliers ?? false) || (accessProfileEntity.Suppliers ?? false);
				Units = (Units ?? false) || (accessProfileEntity.Units ?? false);
				UpdateExternalCommande = (UpdateExternalCommande ?? false) || (accessProfileEntity.UpdateExternalCommande ?? false);
				UpdateExternalProject = (UpdateExternalProject ?? false) || (accessProfileEntity.UpdateExternalProject ?? false);
				UpdateInternalCommande = (UpdateInternalCommande ?? false) || (accessProfileEntity.UpdateInternalCommande ?? false);
				UpdateInternalProject = (UpdateInternalProject ?? false) || (accessProfileEntity.UpdateInternalProject ?? false);
				ViewExternalCommande = (ViewExternalCommande ?? false) || (accessProfileEntity.ViewExternalCommande ?? false);
				ViewExternalProject = (ViewExternalProject ?? false) || (accessProfileEntity.ViewExternalProject ?? false);
				ViewInternalCommande = (ViewInternalCommande ?? false) || (accessProfileEntity.ViewInternalCommande ?? false);
				ViewInternalProject = (ViewInternalProject ?? false) || (accessProfileEntity.ViewInternalProject ?? false);
				// Accounting 
				Accounting = (Accounting ?? false) || (accessProfileEntity.Accounting ?? false);
				ViewLiquiditatsplanungSkontozahlerAccounting = (ViewLiquiditatsplanungSkontozahlerAccounting ?? false) || (accessProfileEntity.ViewLiquiditatsplanungSkontozahlerAccounting ?? false);
				ViewRechnungsTransferAccounting = (ViewRechnungsTransferAccounting ?? false) || (accessProfileEntity.ViewRechnungsTransferAccounting ?? false);
				ViewKontenrahmenAccounting = (ViewKontenrahmenAccounting ?? false) || (accessProfileEntity.ViewKontenrahmenAccounting ?? false);
				AddKontenrahmenAccounting = (AddKontenrahmenAccounting ?? false) || (accessProfileEntity.AddKontenrahmenAccounting ?? false);
				UpdateKontenrahmenAccounting = (UpdateKontenrahmenAccounting ?? false) || (accessProfileEntity.UpdateKontenrahmenAccounting ?? false);
				DeleteKontenrahmenAccounting = (DeleteKontenrahmenAccounting ?? false) || (accessProfileEntity.DeleteKontenrahmenAccounting ?? false);
				ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting = (ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting ?? false) || (accessProfileEntity.ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting ?? false);
				ViewZahlungskonditionenKundenAccounting = (ViewZahlungskonditionenKundenAccounting ?? false) || (accessProfileEntity.ViewZahlungskonditionenKundenAccounting ?? false);
				UpdateZahlungskonditionenKundenAccounting = (UpdateZahlungskonditionenKundenAccounting ?? false) || (accessProfileEntity.UpdateZahlungskonditionenKundenAccounting ?? false);
				DeleteZahlungskonditionenKundenAccounting = (DeleteZahlungskonditionenKundenAccounting ?? false) || (accessProfileEntity.DeleteZahlungskonditionenKundenAccounting ?? false);
				ViewZahlungskonditionenLieferantenAccounting = (ViewZahlungskonditionenLieferantenAccounting ?? false) || (accessProfileEntity.ViewZahlungskonditionenLieferantenAccounting ?? false);
				UpdateZahlungskonditionenLieferantenAccounting = (UpdateZahlungskonditionenLieferantenAccounting ?? false) || (accessProfileEntity.UpdateZahlungskonditionenLieferantenAccounting ?? false);
				DeleteZahlungskonditionenLieferantenAccounting = (DeleteZahlungskonditionenLieferantenAccounting ?? false) || (accessProfileEntity.DeleteZahlungskonditionenLieferantenAccounting ?? false);
				ViewFactoringRgGutschriftlistAccounting = (ViewFactoringRgGutschriftlistAccounting ?? false) || (accessProfileEntity.ViewFactoringRgGutschriftlistAccounting ?? false);
				ViewAusfuhrAccounting = (ViewAusfuhrAccounting ?? false) || (accessProfileEntity.ViewAusfuhrAccounting ?? false);
				ViewStammdatenkontrolleWareneingangeAccounting = (ViewStammdatenkontrolleWareneingangeAccounting ?? false) || (accessProfileEntity.ViewStammdatenkontrolleWareneingangeAccounting ?? false);
				ViewEinfuhrAccounting = (ViewEinfuhrAccounting ?? false) || (accessProfileEntity.ViewEinfuhrAccounting ?? false);
				ViewRMDCZAccounting = (ViewRMDCZAccounting ?? false) || (accessProfileEntity.ViewRMDCZAccounting ?? false);
				//-
				Statistics = (Statistics ?? false) || (accessProfileEntity.Statistics ?? false);
				StatisticsViewAll = (StatisticsViewAll ?? false) || (accessProfileEntity.StatisticsViewAll ?? false);
				// -
				FinanceOrder = (FinanceOrder ?? false) || (accessProfileEntity.FinanceOrder ?? false);
				FinanceProject = (FinanceProject ?? false) || (accessProfileEntity.FinanceProject ?? false);
			}
		}
		public Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity
			{
				AccessProfileName = AccessProfileName,
				AddExternalCommande = AddExternalCommande ?? false,
				AddExternalProject = AddExternalProject ?? false,
				AddInternalCommande = AddInternalCommande ?? false,
				AddInternalProject = AddInternalProject ?? false,
				Administration = Administration ?? false,
				AdministrationAccessProfiles = AdministrationAccessProfiles ?? false,
				AdministrationAccessProfilesUpdate = AdministrationAccessProfilesUpdate ?? false,
				AdministrationUser = AdministrationUser ?? false,
				AdministrationUserUpdate = AdministrationUserUpdate ?? false,
				Article = Article ?? false,
				Assign = Assign ?? false,
				AssignAllDepartments = AssignAllDepartments ?? false,
				AssignAllEmployees = AssignAllEmployees ?? false,
				AssignAllSites = AssignAllSites ?? false,
				AssignCreateDept = AssignCreateDept ?? false,
				AssignCreateLand = AssignCreateLand ?? false,
				AssignCreateUser = AssignCreateUser ?? false,
				AssignDeleteDept = AssignDeleteDept ?? false,
				AssignDeleteLand = AssignDeleteLand ?? false,
				AssignDeleteUser = AssignDeleteUser ?? false,
				AssignEditDept = AssignEditDept ?? false,
				AssignEditLand = AssignEditLand ?? false,
				AssignEditUser = AssignEditUser ?? false,
				AssignViewDept = AssignViewDept ?? false,
				AssignViewLand = AssignViewLand ?? false,
				AssignViewUser = AssignViewUser ?? false,
				CashLiquidity = CashLiquidity,
				Commande = Commande ?? false,
				CommandeExternalEditAllGroup = CommandeExternalEditAllGroup ?? false,
				CommandeExternalEditAllSite = CommandeExternalEditAllSite ?? false,
				CommandeExternalViewAllGroup = CommandeExternalViewAllGroup ?? false,
				CommandeExternalViewAllSite = CommandeExternalViewAllSite ?? false,
				CommandeInternalEditAllDepartment = CommandeInternalEditAllDepartment ?? false,
				CommandeInternalEditAllGroup = CommandeInternalEditAllGroup ?? false,
				CommandeInternalEditAllSite = CommandeInternalEditAllSite ?? false,
				CommandeInternalViewAllDepartment = CommandeInternalViewAllDepartment ?? false,
				CommandeInternalViewAllGroup = CommandeInternalViewAllGroup ?? false,
				CommandeInternalViewAllSite = CommandeInternalViewAllSite ?? false,
				Config = Config ?? false,
				ConfigCreateArtikel = ConfigCreateArtikel ?? false,
				ConfigCreateDept = ConfigCreateDept ?? false,
				ConfigCreateLand = ConfigCreateLand ?? false,
				ConfigCreateSupplier = ConfigCreateSupplier ?? false,
				ConfigDeleteArtikel = ConfigDeleteArtikel ?? false,
				ConfigDeleteDept = ConfigDeleteDept ?? false,
				ConfigDeleteLand = ConfigDeleteLand ?? false,
				ConfigDeleteSupplier = ConfigDeleteSupplier ?? false,
				ConfigEditArtikel = ConfigEditArtikel ?? false,
				ConfigEditDept = ConfigEditDept ?? false,
				ConfigEditLand = ConfigEditLand ?? false,
				ConfigEditSupplier = ConfigEditSupplier ?? false,
				CreationTime = CreationTime ?? new DateTime(2000, 1, 1),
				CreationUserId = CreationUserId ?? -1,
				CreditManagement = CreditManagement,
				DeleteExternalCommande = DeleteExternalCommande ?? false,
				DeleteExternalProject = DeleteExternalProject ?? false,
				DeleteInternalCommande = DeleteInternalCommande ?? false,
				DeleteInternalProject = DeleteInternalProject ?? false,
				Id = Id,
				IsDefault = IsDefault,
				LastEditTime = LastEditTime ?? new DateTime(2000, 1, 1),
				LastEditUserId = LastEditUserId ?? -1,
				ModuleActivated = ModuleActivated ?? false,
				Project = Project ?? false,
				ProjectExternalEditAllGroup = ProjectExternalEditAllGroup ?? false,
				ProjectExternalEditAllSite = ProjectExternalEditAllSite ?? false,
				ProjectExternalViewAllGroup = ProjectExternalViewAllGroup ?? false,
				ProjectExternalViewAllSite = ProjectExternalViewAllSite ?? false,
				ProjectInternalEditAllGroup = ProjectInternalEditAllGroup ?? false,
				ProjectInternalEditAllSite = ProjectInternalEditAllSite ?? false,
				ProjectInternalViewAllGroup = ProjectInternalViewAllGroup ?? false,
				ProjectInternalViewAllSite = ProjectInternalViewAllSite ?? false,
				Receptions = Receptions ?? false,
				ReceptionsEdit = ReceptionsEdit ?? false,
				ReceptionsView = ReceptionsView ?? false,
				Suppliers = Suppliers ?? false,
				Units = Units ?? false,
				UpdateExternalCommande = UpdateExternalCommande ?? false,
				UpdateExternalProject = UpdateExternalProject ?? false,
				UpdateInternalCommande = UpdateInternalCommande ?? false,
				UpdateInternalProject = UpdateInternalProject ?? false,
				ViewExternalCommande = ViewExternalCommande ?? false,
				ViewExternalProject = ViewExternalProject ?? false,
				ViewInternalCommande = ViewInternalCommande ?? false,
				ViewInternalProject = ViewInternalProject ?? false,
				// - 
				CommandeExternalViewInvoice = CommandeExternalViewInvoice ?? false,
				CommandeExternalViewInvoiceAllGroup = CommandeExternalViewInvoiceAllGroup ?? false,
				CommandeInternalViewInvoice = CommandeInternalViewInvoice ?? false,
				CommandeInternalViewInvoiceAllGroup = CommandeInternalViewInvoiceAllGroup ?? false,
				// Accounting 
				Accounting = Accounting ?? false,
				ViewLiquiditatsplanungSkontozahlerAccounting = ViewLiquiditatsplanungSkontozahlerAccounting ?? false,
				ViewRechnungsTransferAccounting = ViewRechnungsTransferAccounting ?? false,
				ViewKontenrahmenAccounting = ViewKontenrahmenAccounting ?? false,
				AddKontenrahmenAccounting = AddKontenrahmenAccounting ?? false,
				UpdateKontenrahmenAccounting = UpdateKontenrahmenAccounting ?? false,
				DeleteKontenrahmenAccounting = DeleteKontenrahmenAccounting ?? false,
				ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting = ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting ?? false,
				ViewZahlungskonditionenKundenAccounting = ViewZahlungskonditionenKundenAccounting ?? false,
				UpdateZahlungskonditionenKundenAccounting = UpdateZahlungskonditionenKundenAccounting ?? false,
				DeleteZahlungskonditionenKundenAccounting = DeleteZahlungskonditionenKundenAccounting ?? false,
				ViewZahlungskonditionenLieferantenAccounting = ViewZahlungskonditionenLieferantenAccounting ?? false,
				UpdateZahlungskonditionenLieferantenAccounting = UpdateZahlungskonditionenLieferantenAccounting ?? false,
				DeleteZahlungskonditionenLieferantenAccounting = DeleteZahlungskonditionenLieferantenAccounting ?? false,
				ViewFactoringRgGutschriftlistAccounting = ViewFactoringRgGutschriftlistAccounting ?? false,
				ViewAusfuhrAccounting = ViewAusfuhrAccounting ?? false,
				ViewStammdatenkontrolleWareneingangeAccounting = ViewStammdatenkontrolleWareneingangeAccounting ?? false,
				ViewEinfuhrAccounting = ViewEinfuhrAccounting ?? false,
				ViewRMDCZAccounting = ViewRMDCZAccounting ?? false,
				//-
				Statistics = Statistics ?? false,
				StatisticsViewAll = StatisticsViewAll ?? false,
				//-
				FinanceOrder = FinanceOrder ?? false,
				FinanceProject = FinanceProject ?? false,
			};
		}
	}
}