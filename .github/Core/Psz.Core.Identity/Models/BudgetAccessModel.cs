using Newtonsoft.Json;

namespace Psz.Core.Identity.Models
{
	public class BudgetAccessModel
	{


		[JsonProperty("ModuleActivated")]
		public bool ModuleActivated = true;

		[JsonProperty("Config")]
		public bool Config;

		[JsonProperty("Units")]
		public bool Units;

		[JsonProperty("ConfigCreateLand")]
		public bool ConfigCreateLand;

		[JsonProperty("ConfigEditLand")]
		public bool ConfigEditLand;

		[JsonProperty("ConfigDeleteLand")]
		public bool ConfigDeleteLand;

		[JsonProperty("ConfigCreateDept")]
		public bool ConfigCreateDept;

		[JsonProperty("ConfigEditDept")]
		public bool ConfigEditDept;

		[JsonProperty("ConfigDeleteDept")]
		public bool ConfigDeleteDept;

		[JsonProperty("Suppliers")]
		public bool Suppliers;

		[JsonProperty("ConfigCreateSupplier")]
		public bool ConfigCreateSupplier;

		[JsonProperty("ConfigEditSupplier")]
		public bool ConfigEditSupplier;

		[JsonProperty("ConfigDeleteSupplier")]
		public bool ConfigDeleteSupplier;

		[JsonProperty("Article")]
		public bool Article;

		[JsonProperty("ConfigCreateArtikel")]
		public bool ConfigCreateArtikel;

		[JsonProperty("ConfigEditArtikel")]
		public bool ConfigEditArtikel;

		[JsonProperty("ConfigDeleteArtikel")]
		public bool ConfigDeleteArtikel;

		[JsonProperty("Assign")]
		public bool Assign;

		[JsonProperty("AssignViewLand")]
		public bool AssignViewLand;

		[JsonProperty("AssignCreateLand")]
		public bool AssignCreateLand;

		[JsonProperty("AssignEditLand")]
		public bool AssignEditLand;

		[JsonProperty("AssignDeleteLand")]
		public bool AssignDeleteLand;

		[JsonProperty("AssignViewDept")]
		public bool AssignViewDept;

		[JsonProperty("AssignCreateDept")]
		public bool AssignCreateDept;

		[JsonProperty("AssignEditDept")]
		public bool AssignEditDept;

		[JsonProperty("AssignDeleteDept")]
		public bool AssignDeleteDept;

		[JsonProperty("AssignViewUser")]
		public bool AssignViewUser;

		[JsonProperty("AssignCreateUser")]
		public bool AssignCreateUser;

		[JsonProperty("AssignEditUser")]
		public bool AssignEditUser;

		[JsonProperty("AssignDeleteUser")]
		public bool AssignDeleteUser;

		[JsonProperty("Project")]
		public bool Project;

		[JsonProperty("ViewExternalProject")]
		public bool ViewExternalProject;

		[JsonProperty("AddExternalProject")]
		public bool AddExternalProject;

		[JsonProperty("UpdateExternalProject")]
		public bool UpdateExternalProject;

		[JsonProperty("DeleteExternalProject")]
		public bool DeleteExternalProject;

		[JsonProperty("ViewInternalProject")]
		public bool ViewInternalProject;

		[JsonProperty("AddInternalProject")]
		public bool AddInternalProject;

		[JsonProperty("UpdateInternalProject")]
		public bool UpdateInternalProject;

		[JsonProperty("DeleteInternalProject")]
		public bool DeleteInternalProject;

		[JsonProperty("Commande")]
		public bool Commande;

		[JsonProperty("ViewExternalCommande")]
		public bool ViewExternalCommande;

		[JsonProperty("AddExternalCommande")]
		public bool AddExternalCommande;

		[JsonProperty("UpdateExternalCommande")]
		public bool UpdateExternalCommande;

		[JsonProperty("DeleteExternalCommande")]
		public bool DeleteExternalCommande;

		[JsonProperty("ViewInternalCommande")]
		public bool ViewInternalCommande;

		[JsonProperty("AddInternalCommande")]
		public bool AddInternalCommande;

		[JsonProperty("UpdateInternalCommande")]
		public bool UpdateInternalCommande;

		[JsonProperty("DeleteInternalCommande")]
		public bool DeleteInternalCommande;

		[JsonProperty("Administration")]
		public bool Administration;

		[JsonProperty("AdministrationUser")]
		public bool AdministrationUser;

		[JsonProperty("AdministrationUserUpdate")]
		public bool AdministrationUserUpdate;

		[JsonProperty("AdministrationAccessProfiles")]
		public bool AdministrationAccessProfiles;

		[JsonProperty("AdministrationAccessProfilesUpdate")]
		public bool AdministrationAccessProfilesUpdate;



	}
}
