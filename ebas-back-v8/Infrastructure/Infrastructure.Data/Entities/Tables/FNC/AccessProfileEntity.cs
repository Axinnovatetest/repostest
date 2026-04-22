using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class AccessProfileEntity
	{
		public string AccessProfileName { get; set; }
		public bool? Accounting { get; set; }
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
		public bool? FinanceOrder { get; set; }
		public bool? FinanceProject { get; set; }
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
		public bool? Statistics { get; set; }
		public bool? StatisticsViewAll { get; set; }
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

		public AccessProfileEntity() { }

		public AccessProfileEntity(DataRow dataRow)
		{
			AccessProfileName = (dataRow["AccessProfileName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AccessProfileName"]);
			Accounting = (dataRow["Accounting"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Accounting"]);
			AddExternalCommande = (dataRow["AddExternalCommande"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddExternalCommande"]);
			AddExternalProject = (dataRow["AddExternalProject"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddExternalProject"]);
			AddInternalCommande = (dataRow["AddInternalCommande"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddInternalCommande"]);
			AddInternalProject = (dataRow["AddInternalProject"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddInternalProject"]);
			AddKontenrahmenAccounting = (dataRow["AddKontenrahmenAccounting"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddKontenrahmenAccounting"]);
			Administration = (dataRow["Administration"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Administration"]);
			AdministrationAccessProfiles = (dataRow["AdministrationAccessProfiles"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AdministrationAccessProfiles"]);
			AdministrationAccessProfilesUpdate = (dataRow["AdministrationAccessProfilesUpdate"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AdministrationAccessProfilesUpdate"]);
			AdministrationUser = (dataRow["AdministrationUser"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AdministrationUser"]);
			AdministrationUserUpdate = (dataRow["AdministrationUserUpdate"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AdministrationUserUpdate"]);
			Article = (dataRow["Article"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Article"]);
			Assign = (dataRow["Assign"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Assign"]);
			AssignAllDepartments = (dataRow["AssignAllDepartments"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AssignAllDepartments"]);
			AssignAllEmployees = (dataRow["AssignAllEmployees"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AssignAllEmployees"]);
			AssignAllSites = (dataRow["AssignAllSites"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AssignAllSites"]);
			AssignCreateDept = (dataRow["AssignCreateDept"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AssignCreateDept"]);
			AssignCreateLand = (dataRow["AssignCreateLand"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AssignCreateLand"]);
			AssignCreateUser = (dataRow["AssignCreateUser"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AssignCreateUser"]);
			AssignDeleteDept = (dataRow["AssignDeleteDept"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AssignDeleteDept"]);
			AssignDeleteLand = (dataRow["AssignDeleteLand"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AssignDeleteLand"]);
			AssignDeleteUser = (dataRow["AssignDeleteUser"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AssignDeleteUser"]);
			AssignEditDept = (dataRow["AssignEditDept"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AssignEditDept"]);
			AssignEditLand = (dataRow["AssignEditLand"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AssignEditLand"]);
			AssignEditUser = (dataRow["AssignEditUser"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AssignEditUser"]);
			AssignViewDept = (dataRow["AssignViewDept"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AssignViewDept"]);
			AssignViewLand = (dataRow["AssignViewLand"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AssignViewLand"]);
			AssignViewUser = (dataRow["AssignViewUser"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AssignViewUser"]);
			Budget = Convert.ToBoolean(dataRow["Budget"]);
			CashLiquidity = Convert.ToBoolean(dataRow["CashLiquidity"]);
			Commande = (dataRow["Commande"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Commande"]);
			CommandeExternalEditAllGroup = (dataRow["CommandeExternalEditAllGroup"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CommandeExternalEditAllGroup"]);
			CommandeExternalEditAllSite = (dataRow["CommandeExternalEditAllSite"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CommandeExternalEditAllSite"]);
			CommandeExternalViewAllGroup = (dataRow["CommandeExternalViewAllGroup"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CommandeExternalViewAllGroup"]);
			CommandeExternalViewAllSite = (dataRow["CommandeExternalViewAllSite"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CommandeExternalViewAllSite"]);
			CommandeExternalViewInvoice = (dataRow["CommandeExternalViewInvoice"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CommandeExternalViewInvoice"]);
			CommandeExternalViewInvoiceAllGroup = (dataRow["CommandeExternalViewInvoiceAllGroup"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CommandeExternalViewInvoiceAllGroup"]);
			CommandeInternalEditAllDepartment = (dataRow["CommandeInternalEditAllDepartment"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CommandeInternalEditAllDepartment"]);
			CommandeInternalEditAllGroup = (dataRow["CommandeInternalEditAllGroup"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CommandeInternalEditAllGroup"]);
			CommandeInternalEditAllSite = (dataRow["CommandeInternalEditAllSite"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CommandeInternalEditAllSite"]);
			CommandeInternalViewAllDepartment = (dataRow["CommandeInternalViewAllDepartment"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CommandeInternalViewAllDepartment"]);
			CommandeInternalViewAllGroup = (dataRow["CommandeInternalViewAllGroup"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CommandeInternalViewAllGroup"]);
			CommandeInternalViewAllSite = (dataRow["CommandeInternalViewAllSite"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CommandeInternalViewAllSite"]);
			CommandeInternalViewInvoice = (dataRow["CommandeInternalViewInvoice"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CommandeInternalViewInvoice"]);
			CommandeInternalViewInvoiceAllGroup = (dataRow["CommandeInternalViewInvoiceAllGroup"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CommandeInternalViewInvoiceAllGroup"]);
			Config = (dataRow["Config"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Config"]);
			ConfigCreateArtikel = (dataRow["ConfigCreateArtikel"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfigCreateArtikel"]);
			ConfigCreateDept = (dataRow["ConfigCreateDept"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfigCreateDept"]);
			ConfigCreateLand = (dataRow["ConfigCreateLand"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfigCreateLand"]);
			ConfigCreateSupplier = (dataRow["ConfigCreateSupplier"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfigCreateSupplier"]);
			ConfigDeleteArtikel = (dataRow["ConfigDeleteArtikel"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfigDeleteArtikel"]);
			ConfigDeleteDept = (dataRow["ConfigDeleteDept"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfigDeleteDept"]);
			ConfigDeleteLand = (dataRow["ConfigDeleteLand"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfigDeleteLand"]);
			ConfigDeleteSupplier = (dataRow["ConfigDeleteSupplier"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfigDeleteSupplier"]);
			ConfigEditArtikel = (dataRow["ConfigEditArtikel"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfigEditArtikel"]);
			ConfigEditDept = (dataRow["ConfigEditDept"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfigEditDept"]);
			ConfigEditLand = (dataRow["ConfigEditLand"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfigEditLand"]);
			ConfigEditSupplier = (dataRow["ConfigEditSupplier"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfigEditSupplier"]);
			CreationTime = (dataRow["CreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = (dataRow["CreationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreationUserId"]);
			CreditManagement = Convert.ToBoolean(dataRow["CreditManagement"]);
			DeleteExternalCommande = (dataRow["DeleteExternalCommande"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DeleteExternalCommande"]);
			DeleteExternalProject = (dataRow["DeleteExternalProject"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DeleteExternalProject"]);
			DeleteInternalCommande = (dataRow["DeleteInternalCommande"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DeleteInternalCommande"]);
			DeleteInternalProject = (dataRow["DeleteInternalProject"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DeleteInternalProject"]);
			DeleteKontenrahmenAccounting = (dataRow["DeleteKontenrahmenAccounting"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DeleteKontenrahmenAccounting"]);
			DeleteZahlungskonditionenKundenAccounting = (dataRow["DeleteZahlungskonditionenKundenAccounting"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DeleteZahlungskonditionenKundenAccounting"]);
			DeleteZahlungskonditionenLieferantenAccounting = (dataRow["DeleteZahlungskonditionenLieferantenAccounting"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DeleteZahlungskonditionenLieferantenAccounting"]);
			FinanceOrder = (dataRow["FinanceOrder"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FinanceOrder"]);
			FinanceProject = (dataRow["FinanceProject"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FinanceProject"]);
			Id = Convert.ToInt32(dataRow["ID"]);
			IsDefault = (dataRow["IsDefault"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsDefault"]);
			LastEditTime = (dataRow["LastEditTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastEditTime"]);
			LastEditUserId = (dataRow["LastEditUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastEditUserId"]);
			MainAccessProfileId = (dataRow["MainAccessProfileId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["MainAccessProfileId"]);
			ModuleActivated = (dataRow["ModuleActivated"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ModuleActivated"]);
			Project = (dataRow["Project"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Project"]);
			ProjectExternalEditAllGroup = (dataRow["ProjectExternalEditAllGroup"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ProjectExternalEditAllGroup"]);
			ProjectExternalEditAllSite = (dataRow["ProjectExternalEditAllSite"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ProjectExternalEditAllSite"]);
			ProjectExternalViewAllGroup = (dataRow["ProjectExternalViewAllGroup"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ProjectExternalViewAllGroup"]);
			ProjectExternalViewAllSite = (dataRow["ProjectExternalViewAllSite"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ProjectExternalViewAllSite"]);
			ProjectInternalEditAllGroup = (dataRow["ProjectInternalEditAllGroup"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ProjectInternalEditAllGroup"]);
			ProjectInternalEditAllSite = (dataRow["ProjectInternalEditAllSite"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ProjectInternalEditAllSite"]);
			ProjectInternalViewAllGroup = (dataRow["ProjectInternalViewAllGroup"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ProjectInternalViewAllGroup"]);
			ProjectInternalViewAllSite = (dataRow["ProjectInternalViewAllSite"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ProjectInternalViewAllSite"]);
			Receptions = (dataRow["Receptions"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Receptions"]);
			ReceptionsEdit = (dataRow["ReceptionsEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ReceptionsEdit"]);
			ReceptionsView = (dataRow["ReceptionsView"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ReceptionsView"]);
			Statistics = (dataRow["Statistics"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Statistics"]);
			StatisticsViewAll = (dataRow["StatisticsViewAll"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StatisticsViewAll"]);
			Suppliers = (dataRow["Suppliers"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Suppliers"]);
			Units = (dataRow["Units"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Units"]);
			UpdateExternalCommande = (dataRow["UpdateExternalCommande"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["UpdateExternalCommande"]);
			UpdateExternalProject = (dataRow["UpdateExternalProject"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["UpdateExternalProject"]);
			UpdateInternalCommande = (dataRow["UpdateInternalCommande"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["UpdateInternalCommande"]);
			UpdateInternalProject = (dataRow["UpdateInternalProject"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["UpdateInternalProject"]);
			UpdateKontenrahmenAccounting = (dataRow["UpdateKontenrahmenAccounting"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["UpdateKontenrahmenAccounting"]);
			UpdateZahlungskonditionenKundenAccounting = (dataRow["UpdateZahlungskonditionenKundenAccounting"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["UpdateZahlungskonditionenKundenAccounting"]);
			UpdateZahlungskonditionenLieferantenAccounting = (dataRow["UpdateZahlungskonditionenLieferantenAccounting"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["UpdateZahlungskonditionenLieferantenAccounting"]);
			ViewAusfuhrAccounting = (dataRow["ViewAusfuhrAccounting"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ViewAusfuhrAccounting"]);
			ViewEinfuhrAccounting = (dataRow["ViewEinfuhrAccounting"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ViewEinfuhrAccounting"]);
			ViewExternalCommande = (dataRow["ViewExternalCommande"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ViewExternalCommande"]);
			ViewExternalProject = (dataRow["ViewExternalProject"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ViewExternalProject"]);
			ViewFactoringRgGutschriftlistAccounting = (dataRow["ViewFactoringRgGutschriftlistAccounting"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ViewFactoringRgGutschriftlistAccounting"]);
			ViewInternalCommande = (dataRow["ViewInternalCommande"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ViewInternalCommande"]);
			ViewInternalProject = (dataRow["ViewInternalProject"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ViewInternalProject"]);
			ViewKontenrahmenAccounting = (dataRow["ViewKontenrahmenAccounting"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ViewKontenrahmenAccounting"]);
			ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting = (dataRow["ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting"]);
			ViewLiquiditatsplanungSkontozahlerAccounting = (dataRow["ViewLiquiditatsplanungSkontozahlerAccounting"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ViewLiquiditatsplanungSkontozahlerAccounting"]);
			ViewRechnungsTransferAccounting = (dataRow["ViewRechnungsTransferAccounting"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ViewRechnungsTransferAccounting"]);
			ViewRMDCZAccounting = (dataRow["ViewRMDCZAccounting"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ViewRMDCZAccounting"]);
			ViewStammdatenkontrolleWareneingangeAccounting = (dataRow["ViewStammdatenkontrolleWareneingangeAccounting"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ViewStammdatenkontrolleWareneingangeAccounting"]);
			ViewZahlungskonditionenKundenAccounting = (dataRow["ViewZahlungskonditionenKundenAccounting"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ViewZahlungskonditionenKundenAccounting"]);
			ViewZahlungskonditionenLieferantenAccounting = (dataRow["ViewZahlungskonditionenLieferantenAccounting"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ViewZahlungskonditionenLieferantenAccounting"]);
		}

		public AccessProfileEntity ShallowClone()
		{
			return new AccessProfileEntity
			{
				AccessProfileName = AccessProfileName,
				Accounting = Accounting,
				AddExternalCommande = AddExternalCommande,
				AddExternalProject = AddExternalProject,
				AddInternalCommande = AddInternalCommande,
				AddInternalProject = AddInternalProject,
				AddKontenrahmenAccounting = AddKontenrahmenAccounting,
				Administration = Administration,
				AdministrationAccessProfiles = AdministrationAccessProfiles,
				AdministrationAccessProfilesUpdate = AdministrationAccessProfilesUpdate,
				AdministrationUser = AdministrationUser,
				AdministrationUserUpdate = AdministrationUserUpdate,
				Article = Article,
				Assign = Assign,
				AssignAllDepartments = AssignAllDepartments,
				AssignAllEmployees = AssignAllEmployees,
				AssignAllSites = AssignAllSites,
				AssignCreateDept = AssignCreateDept,
				AssignCreateLand = AssignCreateLand,
				AssignCreateUser = AssignCreateUser,
				AssignDeleteDept = AssignDeleteDept,
				AssignDeleteLand = AssignDeleteLand,
				AssignDeleteUser = AssignDeleteUser,
				AssignEditDept = AssignEditDept,
				AssignEditLand = AssignEditLand,
				AssignEditUser = AssignEditUser,
				AssignViewDept = AssignViewDept,
				AssignViewLand = AssignViewLand,
				AssignViewUser = AssignViewUser,
				Budget = Budget,
				CashLiquidity = CashLiquidity,
				Commande = Commande,
				CommandeExternalEditAllGroup = CommandeExternalEditAllGroup,
				CommandeExternalEditAllSite = CommandeExternalEditAllSite,
				CommandeExternalViewAllGroup = CommandeExternalViewAllGroup,
				CommandeExternalViewAllSite = CommandeExternalViewAllSite,
				CommandeExternalViewInvoice = CommandeExternalViewInvoice,
				CommandeExternalViewInvoiceAllGroup = CommandeExternalViewInvoiceAllGroup,
				CommandeInternalEditAllDepartment = CommandeInternalEditAllDepartment,
				CommandeInternalEditAllGroup = CommandeInternalEditAllGroup,
				CommandeInternalEditAllSite = CommandeInternalEditAllSite,
				CommandeInternalViewAllDepartment = CommandeInternalViewAllDepartment,
				CommandeInternalViewAllGroup = CommandeInternalViewAllGroup,
				CommandeInternalViewAllSite = CommandeInternalViewAllSite,
				CommandeInternalViewInvoice = CommandeInternalViewInvoice,
				CommandeInternalViewInvoiceAllGroup = CommandeInternalViewInvoiceAllGroup,
				Config = Config,
				ConfigCreateArtikel = ConfigCreateArtikel,
				ConfigCreateDept = ConfigCreateDept,
				ConfigCreateLand = ConfigCreateLand,
				ConfigCreateSupplier = ConfigCreateSupplier,
				ConfigDeleteArtikel = ConfigDeleteArtikel,
				ConfigDeleteDept = ConfigDeleteDept,
				ConfigDeleteLand = ConfigDeleteLand,
				ConfigDeleteSupplier = ConfigDeleteSupplier,
				ConfigEditArtikel = ConfigEditArtikel,
				ConfigEditDept = ConfigEditDept,
				ConfigEditLand = ConfigEditLand,
				ConfigEditSupplier = ConfigEditSupplier,
				CreationTime = CreationTime,
				CreationUserId = CreationUserId,
				CreditManagement = CreditManagement,
				DeleteExternalCommande = DeleteExternalCommande,
				DeleteExternalProject = DeleteExternalProject,
				DeleteInternalCommande = DeleteInternalCommande,
				DeleteInternalProject = DeleteInternalProject,
				DeleteKontenrahmenAccounting = DeleteKontenrahmenAccounting,
				DeleteZahlungskonditionenKundenAccounting = DeleteZahlungskonditionenKundenAccounting,
				DeleteZahlungskonditionenLieferantenAccounting = DeleteZahlungskonditionenLieferantenAccounting,
				FinanceOrder = FinanceOrder,
				FinanceProject = FinanceProject,
				Id = Id,
				IsDefault = IsDefault,
				LastEditTime = LastEditTime,
				LastEditUserId = LastEditUserId,
				MainAccessProfileId = MainAccessProfileId,
				ModuleActivated = ModuleActivated,
				Project = Project,
				ProjectExternalEditAllGroup = ProjectExternalEditAllGroup,
				ProjectExternalEditAllSite = ProjectExternalEditAllSite,
				ProjectExternalViewAllGroup = ProjectExternalViewAllGroup,
				ProjectExternalViewAllSite = ProjectExternalViewAllSite,
				ProjectInternalEditAllGroup = ProjectInternalEditAllGroup,
				ProjectInternalEditAllSite = ProjectInternalEditAllSite,
				ProjectInternalViewAllGroup = ProjectInternalViewAllGroup,
				ProjectInternalViewAllSite = ProjectInternalViewAllSite,
				Receptions = Receptions,
				ReceptionsEdit = ReceptionsEdit,
				ReceptionsView = ReceptionsView,
				Statistics = Statistics,
				StatisticsViewAll = StatisticsViewAll,
				Suppliers = Suppliers,
				Units = Units,
				UpdateExternalCommande = UpdateExternalCommande,
				UpdateExternalProject = UpdateExternalProject,
				UpdateInternalCommande = UpdateInternalCommande,
				UpdateInternalProject = UpdateInternalProject,
				UpdateKontenrahmenAccounting = UpdateKontenrahmenAccounting,
				UpdateZahlungskonditionenKundenAccounting = UpdateZahlungskonditionenKundenAccounting,
				UpdateZahlungskonditionenLieferantenAccounting = UpdateZahlungskonditionenLieferantenAccounting,
				ViewAusfuhrAccounting = ViewAusfuhrAccounting,
				ViewEinfuhrAccounting = ViewEinfuhrAccounting,
				ViewExternalCommande = ViewExternalCommande,
				ViewExternalProject = ViewExternalProject,
				ViewFactoringRgGutschriftlistAccounting = ViewFactoringRgGutschriftlistAccounting,
				ViewInternalCommande = ViewInternalCommande,
				ViewInternalProject = ViewInternalProject,
				ViewKontenrahmenAccounting = ViewKontenrahmenAccounting,
				ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting = ViewLiquiditatsplanungOffeneMaterialbestellungenAccounting,
				ViewLiquiditatsplanungSkontozahlerAccounting = ViewLiquiditatsplanungSkontozahlerAccounting,
				ViewRechnungsTransferAccounting = ViewRechnungsTransferAccounting,
				ViewRMDCZAccounting = ViewRMDCZAccounting,
				ViewStammdatenkontrolleWareneingangeAccounting = ViewStammdatenkontrolleWareneingangeAccounting,
				ViewZahlungskonditionenKundenAccounting = ViewZahlungskonditionenKundenAccounting,
				ViewZahlungskonditionenLieferantenAccounting = ViewZahlungskonditionenLieferantenAccounting
			};
		}
	}
}