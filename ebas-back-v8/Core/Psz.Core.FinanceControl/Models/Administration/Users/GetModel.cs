using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Models.Administration.Users
{
	public class GetModel
	{
		public int AccessProfileId { get; set; }
		public int? CompanyId { get; set; }
		public string CompanyName { get; set; }
		public int? CountryId { get; set; }
		public int CreationUserId { get; set; }
		public DateTime CreationTime { get; set; }
		public bool? CustomerServiceApp { get; set; }
		public DateTime? DeleteDate { get; set; }
		public int? DeleteUserId { get; set; }
		public int? DepartmentId { get; set; }
		public string DepartmentName { get; set; }
		public string Email { get; set; }
		public string Fax { get; set; }
		public bool? FinanceControlApp { get; set; }
		public bool? HumanResourcesApp { get; set; }
		public int Id { get; set; }
		public bool IsArchived { get; set; }
		public bool IsActivated { get; set; }
		public bool? IsGlobalDirector { get; set; }
		public DateTime? LastEditDate { get; set; }
		public int? LastEditUserId { get; set; }
		public bool? LogisticsApp { get; set; }
		public bool? MasterDataApp { get; set; }
		public bool? MaterialManagementApp { get; set; }
		public string Name { get; set; }
		public string Password { get; set; }
		public bool? SalesDistributionApp { get; set; }
		public string SelectedLanguage { get; set; }
		public bool? SettingsApp { get; set; }
		public bool SuperAdministrator { get; set; }
		public string TelephoneHome { get; set; }
		public string TelephoneIP { get; set; }
		public string TelephoneMobile { get; set; }
		public string Username { get; set; }

		//
		public List<string> AccessProfileNames { get; set; }
		public List<AccessProfileModel> AccessProfiles { get; set; }

		public GetModel(Infrastructure.Data.Entities.Tables.COR.UserEntity userEntity,
			Infrastructure.Data.Entities.Tables.STG.CompanyEntity companyEntity,
			Infrastructure.Data.Entities.Tables.STG.DepartmentEntity departmentEntity,
			List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity> accessProfileEntities)
		{
			if(userEntity == null)
				return;

			AccessProfileId = userEntity.AccessProfileId;
			CompanyId = userEntity.CompanyId;
			CompanyName = companyEntity?.Name;
			CountryId = userEntity.CountryId;
			CreationUserId = userEntity.CreationUserId;
			CreationTime = userEntity.CreationTime;
			CustomerServiceApp = userEntity.CustomerServiceApp;
			DeleteDate = userEntity.DeleteDate;
			DeleteUserId = userEntity.DeleteUserId;
			DepartmentId = userEntity.DepartmentId;
			DepartmentName = departmentEntity?.Name;
			Email = userEntity.Email;
			Fax = userEntity.Fax;
			FinanceControlApp = userEntity.FinanceControlApp;
			HumanResourcesApp = userEntity.HumanResourcesApp;
			Id = userEntity.Id;
			IsArchived = userEntity.IsArchived;
			IsActivated = userEntity.IsActivated;
			IsGlobalDirector = userEntity.IsGlobalDirector;
			LastEditDate = userEntity.LastEditDate;
			LastEditUserId = userEntity.LastEditUserId;
			LogisticsApp = userEntity.LogisticsApp;
			MasterDataApp = userEntity.MasterDataApp;
			MaterialManagementApp = userEntity.MaterialManagementApp;
			Name = userEntity.Name;
			Password = userEntity.Password;
			SalesDistributionApp = userEntity.SalesDistributionApp;
			SelectedLanguage = userEntity.SelectedLanguage;
			SettingsApp = userEntity.SettingsApp;
			SuperAdministrator = userEntity.SuperAdministrator;
			TelephoneHome = userEntity.TelephoneHome;
			TelephoneIP = userEntity.TelephoneIP;
			TelephoneMobile = userEntity.TelephoneMobile;
			Username = userEntity.Username;

			//- 
			if(accessProfileEntities != null && accessProfileEntities.Count > 0)
			{
				AccessProfileNames = new List<string>();
				AccessProfiles = new List<AccessProfileModel>();
				accessProfileEntities?.ForEach(x =>
				{
					AccessProfileNames.Add(x.AccessProfileName);
					AccessProfiles.Add(new AccessProfileModel(x));
				});
			}
		}

		public class AccessProfileModel
		{
			public string AccessProfileName { get; set; }
			public bool? AddExternalCommande { get; set; }
			public bool? AddExternalProject { get; set; }
			public bool? AddInternalCommande { get; set; }
			public bool? AddInternalProject { get; set; }
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
			public bool? CommandeInternalEditAllDepartment { get; set; }
			public bool? CommandeInternalEditAllGroup { get; set; }
			public bool? CommandeInternalEditAllSite { get; set; }
			public bool? CommandeInternalViewAllDepartment { get; set; }
			public bool? CommandeInternalViewAllGroup { get; set; }
			public bool? CommandeInternalViewAllSite { get; set; }
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
			public int Id { get; set; }
			public DateTime? LastEditTime { get; set; }
			public int? LastEditUserId { get; set; }
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
			public bool? ViewExternalCommande { get; set; }
			public bool? ViewExternalProject { get; set; }
			public bool? ViewInternalCommande { get; set; }
			public bool? ViewInternalProject { get; set; }
			// - 
			public bool? CommandeExternalViewInvoice { get; set; }
			public bool? CommandeExternalViewInvoiceAllGroup { get; set; }
			public bool? CommandeInternalViewInvoice { get; set; }
			public bool? CommandeInternalViewInvoiceAllGroup { get; set; }

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
			}
		}
	}
}
