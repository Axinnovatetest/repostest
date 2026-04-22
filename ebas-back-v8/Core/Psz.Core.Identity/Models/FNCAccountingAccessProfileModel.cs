using System.Collections.Generic;

namespace Psz.Core.Identity.Models
{
	public class FNCAccountingAccessProfileModel
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

		public FNCAccountingAccessProfileModel(List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity> entities)
		{
			if(entities == null || entities.Count <= 0)
				return;

			foreach(var entity in entities)
			{
				ModuleActivated = ModuleActivated || (entity.ModuleActivated ?? false);

				Administration = Administration || (entity.Administration ?? false);
				AdministrationAccessProfiles = AdministrationAccessProfiles || (entity.AdministrationAccessProfiles ?? false);
				AdministrationAccessProfilesUpdate = AdministrationAccessProfilesUpdate || ((entity.Administration ?? false) && (entity.AdministrationAccessProfilesUpdate ?? false));
				AdministrationUser = AdministrationUser || ((entity.Administration ?? false) && (entity.AdministrationUser ?? false));
				AdministrationUserUpdate = AdministrationUserUpdate || ((entity.Administration ?? false) && (entity.AdministrationUserUpdate ?? false));


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
				AssignEditUser = AssignEditUser || (entity.Assign ?? false) && (entity.AssignEditUser ?? false);
				AssignViewDept = AssignViewDept || (entity.Assign ?? false) && (entity.AssignViewDept ?? false);
				AssignViewLand = AssignViewLand || (entity.Assign ?? false) && (entity.AssignViewLand ?? false);
				AssignViewUser = AssignViewUser || (entity.Assign ?? false) && (entity.AssignViewUser ?? false);

				Commande = Commande || (entity.Commande ?? false);
				AddExternalCommande = AddExternalCommande || ((entity.Commande ?? false) && (entity.AddExternalCommande ?? false));
				AddInternalCommande = AddInternalCommande || ((entity.Commande ?? false) && (entity.AddInternalCommande ?? false));
				DeleteExternalCommande = DeleteExternalCommande || ((entity.Commande ?? false) && (entity.DeleteExternalCommande ?? false));
				DeleteInternalCommande = DeleteInternalCommande || ((entity.Commande ?? false) && (entity.DeleteInternalCommande ?? false));
				UpdateExternalCommande = UpdateExternalCommande || ((entity.Commande ?? false) && (entity.UpdateExternalCommande ?? false));
				UpdateInternalCommande = UpdateInternalCommande || ((entity.Commande ?? false) && (entity.UpdateInternalCommande ?? false));
				ViewExternalCommande = ViewExternalCommande || ((entity.Commande ?? false) && (entity.ViewExternalCommande ?? false));
				ViewInternalCommande = ViewInternalCommande || ((entity.Commande ?? false) && (entity.ViewInternalCommande ?? false));



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

				Project = Project || (entity.Project ?? false);
				AddExternalProject = AddExternalProject || (entity.AddExternalProject ?? false);
				AddInternalProject = AddInternalProject || (entity.AddInternalProject ?? false);
				UpdateExternalProject = UpdateExternalProject || (entity.UpdateExternalProject ?? false);
				UpdateInternalProject = UpdateInternalProject || (entity.UpdateInternalProject ?? false);
				ViewExternalProject = ViewExternalProject || (entity.ViewExternalProject ?? false);
				ViewInternalProject = ViewInternalProject || (entity.ViewInternalProject ?? false);
				DeleteExternalProject = DeleteExternalProject || (entity.DeleteExternalProject ?? false);
				DeleteInternalProject = DeleteInternalProject || (entity.DeleteInternalProject ?? false);

				Article = Article || (entity.Article ?? false);
				Suppliers = Suppliers || (entity.Suppliers ?? false);
				Units = Units || (entity.Units ?? false);
				// -

				Receptions = Receptions || (entity.Receptions ?? false);
				ReceptionsEdit = ReceptionsEdit || ((entity.Receptions ?? false) && (entity.ReceptionsEdit ?? false));
				ReceptionsView = ReceptionsView || ((entity.Receptions ?? false) && (entity.ReceptionsView ?? false));
				// - 

			}
		}
		public Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity ToDbEntity(int id, int mainAccessProfileId)
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
			};
		}
	}

}

