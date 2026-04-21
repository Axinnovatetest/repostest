using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Identity.Models
{
	public class MasterDataAccessModel
	{
		public string AccessProfileName { get; set; }

		// -
		public bool Customers { get; set; }
		public int Id { get; set; }
		public int MainAccessProfileId { get; set; }
		public bool ModuleActivated { get; set; }
		public bool Suppliers { get; set; }

		#region Articles 
		public bool AddArticle { get; set; }
		public bool AltPositionsArticleBOM { get; set; }
		public bool ArchiveArticle { get; set; }
		public bool DeleteArticle { get; set; }
		public bool ArticleBOM { get; set; }
		public bool ArticleData { get; set; }
		public bool ArticleLogistics { get; set; }
		public bool ArticleLogisticsPrices { get; set; }
		public bool ArticleOverview { get; set; }
		public bool ArticleProduction { get; set; }
		public bool ArticlePurchase { get; set; }
		public bool ArticleQuality { get; set; }
		public bool ArticleCts { get; set; }
		public bool Administration { get; set; }
		public bool Articles { get; set; }
		public bool ArticleSales { get; set; }
		public bool ArticleSalesCustom { get; set; }
		public bool ArticleSalesItem { get; set; }
		public bool ConfigArticle { get; set; }
		public bool CreateArticlePurchase { get; set; }
		public bool CreateArticleSalesCustom { get; set; }
		public bool CreateArticleSalesItem { get; set; }
		public int CreationUserId { get; set; }
		public bool DeleteAltPositionsArticleBOM { get; set; }
		public bool DeleteArticleBOM { get; set; }
		public bool DeleteArticlePurchase { get; set; }
		public bool DeleteArticleSalesCustom { get; set; }
		public bool DeleteArticleSalesItem { get; set; }
		public bool EditAltPositionsArticleBOM { get; set; }
		public bool EditArticle { get; set; }
		public bool EditArticleBOM { get; set; }
		public bool EditArticleBOMPosition { get; set; }
		public bool EditArticleData { get; set; }
		public bool EditArticleDesignation { get; set; }
		public bool EditArticleImage { get; set; }
		public bool EditArticleLogistics { get; set; }
		public bool EditArticleManager { get; set; }
		public bool EditArticleProduction { get; set; }
		public bool EditArticlePurchase { get; set; }
		public bool EditArticleQuality { get; set; }
		public bool EditArticleSalesCustom { get; set; }
		public bool EditArticleSalesItem { get; set; }
		public bool EditArticleCts { get; set; }
		public bool ImportArticleBOM { get; set; }
		public bool LagerArticleLogistics { get; set; }
		public bool UploadAltPositionsArticleBOM { get; set; }
		public bool UploadArticleBOM { get; set; }
		public bool ValidateArticleBOM { get; set; }
		public bool ViewAltPositionsArticleBOM { get; set; }
		public bool ViewArticleLog { get; set; }
		public bool ViewArticles { get; set; }

		public bool ArticleStatistics { get; set; }
		public bool ArticleStatisticsLogistics { get; set; }
		public bool ArticleStatisticsLogisticsEdit { get; set; }
		public bool ArticleStatisticsEngineering { get; set; }
		public bool ArticleStatisticsEngineeringEdit { get; set; }
		public bool ArticleStatisticsPurchase { get; set; }
		public bool ArticleStatisticsPurchaseEdit { get; set; }
		public bool ArticleStatisticsTechnic { get; set; }
		public bool ArticleStatisticsTechnicEdit { get; set; }
		public bool ArticleStatisticsFinanceAccounting { get; set; }
		public bool ArticleStatisticsFinanceAccountingEdit { get; set; }
		//new
		public bool EditLagerStock { get; set; }
		public bool EditLagerMinStock { get; set; }
		public bool EditLagerCCID { get; set; }
		public bool EditLagerOrderProposal { get; set; }
		//lager list
		public List<KeyValuePair<int, string>> LagerIds { get; set; }

		// - Articles BOM / CP Control
		public bool ArticlesBOMCPControlEngineering { get; set; }
		public bool ArticlesBOMCPControlHistory { get; set; }
		public bool ArticlesBOMCPControlQuality { get; set; }
		#endregion Articles 

		//---Customers
		#region Customers 
		public bool? ViewCustomers { get; set; }
		public bool? AddCustomer { get; set; }
		public bool? EditCustomer { get; set; }
		public bool? ArchiveCustomer { get; set; }
		public bool? ConfigCustomer { get; set; }

		//TODO: Overview
		public bool? CustomerOverview { get; set; }
		public bool? ViewLPCustomer { get; set; }
		public bool? EditCustomerImage { get; set; }
		public bool? EditCustomerCoordination { get; set; }

		//TODO: Data
		public bool? CustomerData { get; set; }
		public bool? EditCustomerData { get; set; }
		//TODO: Address
		public bool? CustomerAddress { get; set; }
		public bool? EditCustomerAddress { get; set; }

		//TODO: Communication
		public bool? CustomerCommunication { get; set; }
		public bool? EditCustomerCommunication { get; set; }

		//TODO: ContactPerson
		public bool? CustomerContactPerson { get; set; }
		public bool? CreateCustomerContactPerson { get; set; }
		public bool? EditCustomerContactPerson { get; set; }
		public bool? DeleteCustomerContactPerson { get; set; }

		//TODO: Shipping
		public bool? CustomerShipping { get; set; }
		public bool? EditCustomerShipping { get; set; }

		//TODO: History
		public bool? CustomerHistory { get; set; }
		#endregion Customers 

		//---Suppliers
		#region Suppliers 

		public bool? ViewSuppliers { get; set; }
		public bool? AddSupplier { get; set; }
		public bool? EditSupplier { get; set; }
		public bool? ArchiveSupplier { get; set; }
		public bool? ConfigSupplier { get; set; }

		//TODO: Overview
		public bool? SupplierOverview { get; set; }
		public bool? ViewLPSupplier { get; set; }
		public bool? EditSupplierImage { get; set; }
		public bool? EditSupplierCoordination { get; set; }

		//TODO: Data
		public bool? SupplierData { get; set; }
		public bool? EditSupplierData { get; set; }
		//TODO: Address
		public bool? SupplierAddress { get; set; }
		public bool? EditSupplierAddress { get; set; }
		public bool? ViewSupplierAddressComments { get; set; }

		//TODO: Communication
		public bool? SupplierCommunication { get; set; }
		public bool? EditSupplierCommunication { get; set; }

		//TODO: ContactPerson
		public bool? SupplierContactPerson { get; set; }
		public bool? CreateSupplierContactPerson { get; set; }
		public bool? EditSupplierContactPerson { get; set; }
		public bool? DeleteSupplierContactPerson { get; set; }

		//TODO: Shipping
		public bool? SupplierShipping { get; set; }
		public bool? EditSupplierShipping { get; set; }

		//TODO: History
		public bool? SupplierHistory { get; set; }

		//---Settings
		public bool? Settings { get; set; }


		//TODO PricingGroup
		public bool? PricingGroup { get; set; }
		public bool? AddPricingGroup { get; set; }
		public bool? EditPricingGroup { get; set; }

		//TODO DiscountGroup
		public bool? DiscountGroup { get; set; }
		public bool? AddDiscountGroup { get; set; }
		public bool? EditDiscountGroup { get; set; }

		//TODO Industry
		public bool? Industry { get; set; }
		public bool? AddIndustry { get; set; }
		public bool? EditIndustry { get; set; }

		//TODO CustomerGroup
		public bool? CustomerGroup { get; set; }
		public bool? AddCustomerGroup { get; set; }
		public bool? EditCustomerGroup { get; set; }

		//TODO SupplierGroup
		public bool? SupplierGroup { get; set; }
		public bool? AddSupplierGroup { get; set; }
		public bool? EditSupplierGroup { get; set; }

		//TODO PayementPractises
		public bool? PayementPractises { get; set; }
		public bool? AddPayementPractises { get; set; }
		public bool? EditPayementPractises { get; set; }


		//TODO ConditionAssignment 
		public bool? ConditionAssignment { get; set; }
		public bool? AddConditionAssignment { get; set; }
		public bool? EditConditionAssignment { get; set; }


		//TODO TermsOfPayment  
		public bool? TermsOfPayment { get; set; }
		public bool? AddTermsOfPayment { get; set; }
		public bool? EditTermsOfPayment { get; set; }

		//TODO FibuFrame   
		public bool? FibuFrame { get; set; }
		public bool? AddFibuFrame { get; set; }
		public bool? EditFibuFrame { get; set; }

		//TODO SlipCircle    
		public bool? SlipCircle { get; set; }
		public bool? AddSlipCircle { get; set; }
		public bool? EditSlipCircle { get; set; }


		//TODO Currencies     
		public bool? Currencies { get; set; }
		public bool? AddCurrencies { get; set; }
		public bool? EditCurrencies { get; set; }


		//TODO ShippingMethods     
		public bool? ShippingMethods { get; set; }
		public bool? AddShippingMethods { get; set; }
		public bool? EditShippingMethods { get; set; }


		//TODO ContactAddress   
		public bool? ContactAddress { get; set; }
		public bool? AddContactAddress { get; set; }
		public bool? EditContactAddress { get; set; }

		//TODO ContactSalutation   
		public bool? ContactSalutation { get; set; }
		public bool? AddContactSalutation { get; set; }
		public bool? EditContactSalutation { get; set; }
		#endregion Suppliers


		//TODO CustomerItemNumber   
		public bool? CustomerItemNumber { get; set; }
		public bool? AddCustomerItemNumber { get; set; }
		public bool? EditCustomerItemNumber { get; set; }

		// - 2023-08-23
		public bool? CocType { get; set; }
		public bool? AddCocType { get; set; }
		public bool? EditCocType { get; set; }
		public bool? isDefault { get; set; }
		// - 2023-11-08
		public bool? ModuleAdministrator { get; set; }
		public bool? AddEdiConcern { get; set; }
		public bool? EdiConcern { get; set; }
		public bool? EditEdiConcern { get; set; }
		// - 2024-03-04
		public bool? EDrawingEdit { get; set; }
		// - 2024-03-11

		public bool? AddHourlyRate { get; set; }
		public bool? HourlyRate { get; set; }
		public bool? EditHourlyRate { get; set; }

		public bool? DownloadAllOutdatedEinkaufsPreis { get; set; }
		public bool? DownloadOutdatedEinkaufsPreis { get; set; }
		public bool? EinkaufsPreisUpdate { get; set; }

		// - 2024-04-11
		public bool? ArticleAddCustomerDocument { get; set; }
		public bool? ArticleDeleteCustomerDocument { get; set; }

		// -2024-05-13
		public bool? AddRohArtikelNummer { get; set; }
		public bool? EditRohArtikelNummer { get; set; }
		public bool? DeleteRohArtikelNummer { get; set; }
		public bool? GetRohArtikelNummer { get; set; }

		// 2024-04-24 
		public bool? AddArticleReference { get; set; }
		public bool? EditArticleReference { get; set; }
		public bool? RemoveArticleReference { get; set; }
		public bool? ViewArticleReference { get; set; }

		public bool? OfferRequestADD { get; set; }
		public bool? OfferRequestDelete { get; set; }
		public bool? OfferRequestEdit { get; set; }
		public bool? OfferRequestEditEmail { get; set; }
		public bool? OfferRequestSendEmail { get; set; }
		public bool? OfferRequestView { get; set; }
		public bool? offer { get; set; }
		public bool? OfferRequestApplyPrice { get; set; }
		public bool? DeleteFiles { get; set; }
		public bool? AddFiles { get; set; }
		public bool? DownloadFiles { get; set; }
		public bool? SupplierAttachementRemoveFile { get; set; }
		public bool? SupplierAttachementAddFile { get; set; }
		public bool? SupplierAttachementGetFile { get; set; }

		// - 2025-04-25
		public bool? PackagingsLgtPhotoAdd { get; set; }
		public bool? PackagingsLgtPhotoDelete { get; set; }
		public bool? PackagingsLgtPhotoView { get; set; }

		#region PM
		public bool? PMModule { get; set; }
		public bool? PMViewProjectsCompact { get; set; }
		public bool? PMViewProjectsMedium { get; set; }
		public bool? PMViewProjectsDetail { get; set; }
		public bool? PMAddProject { get; set; }
		public bool? PMEditProject { get; set; }
		public bool? PMDeleteProject { get; set; }
		public bool? PMAddMileStone { get; set; }
		public bool? PMEditMileStone { get; set; }
		public bool? PMDeleteMileStone { get; set; }
		public bool? PMAddCable { get; set; }
		public bool? PMEditCable { get; set; }
		public bool? PMDeleteCable { get; set; }
		#endregion
		public MasterDataAccessModel()
		{

		}
		public MasterDataAccessModel(MasterDataAccessModel entity)
		{
			Administration = entity?.Administration ?? false;
			Articles = entity?.Articles ?? false;
			Id = entity?.Id ?? -1;
			MainAccessProfileId = entity?.MainAccessProfileId ?? -1;
			ModuleActivated = entity?.ModuleActivated ?? false;
			Customers = entity?.Customers ?? false;
			Suppliers = entity?.Suppliers ?? false;
		}

		public MasterDataAccessModel(List<Infrastructure.Data.Entities.Tables.BSD.AccessProfileEntity> entities)
		{
			if(entities == null || entities.Count <= 0)
				return;

			DownloadAllOutdatedEinkaufsPreis = false;
			DownloadOutdatedEinkaufsPreis = false;
			EinkaufsPreisUpdate = false;
			AddArticle = false;
			AltPositionsArticleBOM = false;
			ArchiveArticle = false;
			DeleteArticle = false;
			ArticleBOM = false;
			ArticleCts = false;
			ArticleData = false;
			ArticleLogistics = false;
			ArticleLogisticsPrices = false;
			ArticleOverview = false;
			ArticleProduction = false;
			ArticlePurchase = false;
			ArticleQuality = false;
			Administration = false;
			Articles = false;
			ArticleSales = false;
			ArticleSalesCustom = false;
			ArticleSalesItem = false;
			ConfigArticle = false;
			CreateArticlePurchase = false;
			CreateArticleSalesCustom = false;
			CreateArticleSalesItem = false;
			DeleteAltPositionsArticleBOM = false;
			DeleteArticleBOM = false;
			DeleteArticlePurchase = false;
			DeleteArticleSalesCustom = false;
			DeleteArticleSalesItem = false;
			EditAltPositionsArticleBOM = false;
			EditArticle = false;
			EditArticleBOM = false;
			EditArticleBOMPosition = false;
			EditArticleCts = false;
			EditArticleData = false;
			EditArticleDesignation = false;
			EditArticleImage = false;
			EditArticleLogistics = false;
			EditArticleManager = false;
			EditArticleProduction = false;
			EditArticlePurchase = false;
			EditArticleQuality = false;
			EditArticleSalesCustom = false;
			EditArticleSalesItem = false;
			ImportArticleBOM = false;
			LagerArticleLogistics = false;
			UploadAltPositionsArticleBOM = false;
			UploadArticleBOM = false;
			ValidateArticleBOM = false;
			ViewAltPositionsArticleBOM = false;
			ViewArticleLog = false;
			ViewArticles = false;

			// - Articles BOM / CP Control
			ArticlesBOMCPControlEngineering = false;
			ArticlesBOMCPControlHistory = false;
			ArticlesBOMCPControlQuality = false;
			ArticleStatistics = false;
			ArticleStatisticsLogistics = false;
			ArticleStatisticsLogisticsEdit = false;
			ArticleStatisticsEngineering = false;
			ArticleStatisticsEngineeringEdit = false;
			ArticleStatisticsPurchase = false;
			ArticleStatisticsPurchaseEdit = false;
			ArticleStatisticsTechnic = false;
			ArticleStatisticsTechnicEdit = false;
			ArticleStatisticsFinanceAccounting = false;
			ArticleStatisticsFinanceAccountingEdit = false;
			// -
			Customers = false;
			ModuleActivated = false;
			Suppliers = false;
			//new
			EditLagerStock = false;
			EditLagerMinStock = false;
			EditLagerCCID = false;
			EditLagerOrderProposal = false;
			LagerIds = new List<KeyValuePair<int, string>>();
			//NEW -2
			//---Customers
			ViewCustomers = false;
			AddCustomer = false;
			EditCustomer = false;
			ArchiveCustomer = false;
			ConfigCustomer = false;

			//TODO: Overview
			CustomerOverview = false;
			ViewLPCustomer = false;
			EditCustomerImage = false;
			EditCustomerCoordination = false;

			//TODO: Data
			CustomerData = false;
			EditCustomerData = false;
			//TODO: Address
			CustomerAddress = false;
			EditCustomerAddress = false;

			//TODO: Communication
			CustomerCommunication = false;
			EditCustomerCommunication = false;

			//TODO: ContactPerson
			CustomerContactPerson = false;
			CreateCustomerContactPerson = false;
			EditCustomerContactPerson = false;
			DeleteCustomerContactPerson = false;

			//TODO: Shipping
			CustomerShipping = false;
			EditCustomerShipping = false;

			//TODO: History
			CustomerHistory = false;

			//---Suppliers

			ViewSuppliers = false;
			AddSupplier = false;
			EditSupplier = false;
			ArchiveSupplier = false;
			ConfigSupplier = false;

			//TODO: Overview
			SupplierOverview = false;
			ViewLPSupplier = false;
			EditSupplierImage = false;
			EditSupplierCoordination = false;

			//TODO: Data
			SupplierData = false;
			EditSupplierData = false;
			//TODO: Address
			SupplierAddress = false;
			EditSupplierAddress = false;
			ViewSupplierAddressComments = false;

			//TODO: Communication
			SupplierCommunication = false;
			EditSupplierCommunication = false;

			//TODO: ContactPerson
			SupplierContactPerson = false;
			CreateSupplierContactPerson = false;
			EditSupplierContactPerson = false;
			DeleteSupplierContactPerson = false;

			//TODO: Shipping
			SupplierShipping = false;
			EditSupplierShipping = false;

			//TODO: History
			SupplierHistory = false;

			//---Settings
			Settings = false;


			//TODO PricingGroup
			PricingGroup = false;
			AddPricingGroup = false;
			EditPricingGroup = false;

			//TODO DiscountGroup
			DiscountGroup = false;
			AddDiscountGroup = false;
			EditDiscountGroup = false;

			//TODO Industry
			Industry = false;
			AddIndustry = false;
			EditIndustry = false;

			//TODO CustomerGroup
			CustomerGroup = false;
			AddCustomerGroup = false;
			EditCustomerGroup = false;

			//TODO SupplierGroup
			SupplierGroup = false;
			AddSupplierGroup = false;
			EditSupplierGroup = false;

			//TODO PayementPractises
			PayementPractises = false;
			AddPayementPractises = false;
			EditPayementPractises = false;


			//TODO ConditionAssignment 
			ConditionAssignment = false;
			AddConditionAssignment = false;
			EditConditionAssignment = false;


			//TODO TermsOfPayment  
			TermsOfPayment = false;
			AddTermsOfPayment = false;
			EditTermsOfPayment = false;

			//TODO FibuFrame   
			FibuFrame = false;
			AddFibuFrame = false;
			EditFibuFrame = false;

			//TODO SlipCircle    
			SlipCircle = false;
			AddSlipCircle = false;
			EditSlipCircle = false;


			//TODO Currencies     
			Currencies = false;
			AddCurrencies = false;
			EditCurrencies = false;


			//TODO ShippingMethods     
			ShippingMethods = false;
			AddShippingMethods = false;
			EditShippingMethods = false;


			//TODO ContactAddress   
			ContactAddress = false;
			AddContactAddress = false;
			EditContactAddress = false;

			//TODO ContactSalutation   
			ContactSalutation = false;
			AddContactSalutation = false;
			EditContactSalutation = false;

			// - 2022-11-15
			CustomerItemNumber = false;
			AddCustomerItemNumber = false;
			EditCustomerItemNumber = false;

			// - 2023-08-23
			CocType = false;
			AddCocType = false;
			EditCocType = false;
			isDefault = false;
			// - 2023-11-08
			ModuleAdministrator = false;
			AddEdiConcern = false;
			EdiConcern = false;
			EditEdiConcern = false;
			EDrawingEdit = false;
			AddHourlyRate = false;
			HourlyRate = false;
			EditHourlyRate = false;

			DownloadAllOutdatedEinkaufsPreis = false;
			DownloadOutdatedEinkaufsPreis = false;
			EinkaufsPreisUpdate = false;

			// - 2024-04-11
			ArticleAddCustomerDocument = false;
			ArticleDeleteCustomerDocument = false;

			// - 2024-10-03
			PMModule = false;
			PMViewProjectsCompact = false;
			PMViewProjectsMedium = false;
			PMViewProjectsDetail = false;
			PMAddProject = false;
			PMEditProject = false;
			PMDeleteProject = false;
			PMAddMileStone = false;
			PMEditMileStone = false;
			PMDeleteMileStone = false;
			PMAddCable = false;
			PMEditCable = false;
			PMDeleteCable = false;

			AddRohArtikelNummer = false;
			EditRohArtikelNummer = false;
			DeleteRohArtikelNummer = false;
			GetRohArtikelNummer = false;
			// 2024-04-24
			RemoveArticleReference = false;
			EditArticleReference = false;
			AddArticleReference = false;
			ViewArticleReference = false;

			//2024-05-29
			OfferRequestADD = false;
			OfferRequestDelete = false;
			OfferRequestEdit = false;
			OfferRequestEditEmail = false;
			OfferRequestSendEmail = false;
			OfferRequestView = false;
			offer = false;
			OfferRequestApplyPrice = false;
			DeleteFiles = false;
			AddFiles = false;
			DownloadFiles = false;

			SupplierAttachementGetFile = false;
			SupplierAttachementAddFile = false;
			SupplierAttachementRemoveFile = false;

			//2025-04-25
			PackagingsLgtPhotoAdd = false;
			PackagingsLgtPhotoDelete = false;
			PackagingsLgtPhotoView = false;


			foreach(var item in entities)
			{
				DownloadAllOutdatedEinkaufsPreis = DownloadAllOutdatedEinkaufsPreis.Value || (item?.DownloadAllOutdatedEinkaufsPreis ?? false);
				DownloadOutdatedEinkaufsPreis = DownloadOutdatedEinkaufsPreis.Value || (item?.DownloadOutdatedEinkaufsPreis ?? false);
				EinkaufsPreisUpdate = EinkaufsPreisUpdate.Value || (item?.EinkaufsPreisUpdate ?? false);
				AddArticle = AddArticle || (item?.AddArticle ?? false);
				AltPositionsArticleBOM = AltPositionsArticleBOM || (item?.AltPositionsArticleBOM ?? false);
				ArchiveArticle = ArchiveArticle || (item?.ArchiveArticle ?? false);
				DeleteArticle = DeleteArticle || (item?.DeleteArticle ?? false);
				ArticleBOM = ArticleBOM || (item?.ArticleBOM ?? false);
				ArticleCts = ArticleCts || (item?.ArticleCts ?? false);
				ArticleData = ArticleData || (item?.ArticleData ?? false);
				ArticleLogistics = ArticleLogistics || (item?.ArticleLogistics ?? false);
				ArticleLogisticsPrices = ArticleLogisticsPrices || (item?.ArticleLogisticsPrices ?? false);
				ArticleOverview = ArticleOverview || (item?.ArticleOverview ?? false);
				ArticleProduction = ArticleProduction || (item?.ArticleProduction ?? false);
				ArticlePurchase = ArticlePurchase || (item?.ArticlePurchase ?? false);
				ArticleQuality = ArticleQuality || (item?.ArticleQuality ?? false);
				Administration = Administration || (item?.Administration ?? false);
				Articles = Articles || (item?.Articles ?? false);
				ArticleSales = ArticleSales || (item?.ArticleSales ?? false);
				ArticleSalesCustom = ArticleSalesCustom || (item?.ArticleSalesCustom ?? false);
				ArticleSalesItem = ArticleSalesItem || (item?.ArticleSalesItem ?? false);
				ConfigArticle = ConfigArticle || (item?.ConfigArticle ?? false);
				CreateArticlePurchase = CreateArticlePurchase || (item?.CreateArticlePurchase ?? false);
				CreateArticleSalesCustom = CreateArticleSalesCustom || (item?.CreateArticleSalesCustom ?? false);
				CreateArticleSalesItem = CreateArticleSalesItem || (item?.CreateArticleSalesItem ?? false);
				DeleteAltPositionsArticleBOM = DeleteAltPositionsArticleBOM || (item?.DeleteAltPositionsArticleBOM ?? false);
				DeleteArticleBOM = DeleteArticleBOM || (item?.DeleteArticleBOM ?? false);
				DeleteArticlePurchase = DeleteArticlePurchase || (item?.DeleteArticlePurchase ?? false);
				DeleteArticleSalesCustom = DeleteArticleSalesCustom || (item?.DeleteArticleSalesCustom ?? false);
				DeleteArticleSalesItem = DeleteArticleSalesItem || (item?.DeleteArticleSalesItem ?? false);
				EditAltPositionsArticleBOM = EditAltPositionsArticleBOM || (item?.EditAltPositionsArticleBOM ?? false);
				EditArticle = EditArticle || (item?.EditArticle ?? false);
				EditArticleBOM = EditArticleBOM || (item?.EditArticleBOM ?? false);
				EditArticleBOMPosition = EditArticleBOMPosition || (item?.EditArticleBOMPosition ?? false);
				EditArticleCts = EditArticleCts || (item?.EditArticleCts ?? false);
				EditArticleData = EditArticleData || (item?.EditArticleData ?? false);
				EditArticleDesignation = EditArticleDesignation || (item?.EditArticleDesignation ?? false);
				EditArticleImage = EditArticleImage || (item?.EditArticleImage ?? false);
				EditArticleLogistics = EditArticleLogistics || (item?.EditArticleLogistics ?? false);
				EditArticleManager = EditArticleManager || (item?.EditArticleManager ?? false);
				EditArticleProduction = EditArticleProduction || (item?.EditArticleProduction ?? false);
				EditArticlePurchase = EditArticlePurchase || (item?.EditArticlePurchase ?? false);
				EditArticleQuality = EditArticleQuality || (item?.EditArticleQuality ?? false);
				EditArticleSalesCustom = EditArticleSalesCustom || (item?.EditArticleSalesCustom ?? false);
				EditArticleSalesItem = EditArticleSalesItem || (item?.EditArticleSalesItem ?? false);
				ImportArticleBOM = ImportArticleBOM || (item?.ImportArticleBOM ?? false);
				LagerArticleLogistics = LagerArticleLogistics || (item?.LagerArticleLogistics ?? false);
				UploadAltPositionsArticleBOM = UploadAltPositionsArticleBOM || (item?.UploadAltPositionsArticleBOM ?? false);
				UploadArticleBOM = UploadArticleBOM || (item?.UploadArticleBOM ?? false);
				ValidateArticleBOM = ValidateArticleBOM || (item?.ValidateArticleBOM ?? false);
				ViewAltPositionsArticleBOM = ViewAltPositionsArticleBOM || (item?.ViewAltPositionsArticleBOM ?? false);
				ViewArticleLog = ViewArticleLog || (item?.ViewArticleLog ?? false);
				ViewArticles = ViewArticles || (item?.ViewArticles ?? false);
				ArticleStatistics = ArticleStatistics || (item?.ArticleStatistics ?? false);
				ArticleStatisticsLogistics = ArticleStatisticsLogistics || (item?.ArticleStatisticsLogistics ?? false);
				ArticleStatisticsLogisticsEdit = ArticleStatisticsLogistics || (item?.ArticleStatisticsLogistics ?? false);
				ArticleStatisticsEngineering = ArticleStatisticsEngineering || (item?.ArticleStatisticsEngineering ?? false);
				ArticleStatisticsEngineeringEdit = ArticleStatisticsEngineeringEdit || (item?.ArticleStatisticsEngineeringEdit ?? false);
				ArticleStatisticsPurchase = ArticleStatisticsPurchase || (item?.ArticleStatisticsPurchase ?? false);
				ArticleStatisticsPurchaseEdit = ArticleStatisticsPurchaseEdit || (item?.ArticleStatisticsPurchaseEdit ?? false);
				ArticleStatisticsTechnic = ArticleStatisticsTechnic || (item?.ArticleStatisticsTechnic ?? false);
				ArticleStatisticsTechnicEdit = ArticleStatisticsTechnicEdit || (item?.ArticleStatisticsTechnicEdit ?? false);
				ArticleStatisticsFinanceAccounting = ArticleStatisticsFinanceAccounting || (item?.ArticleStatisticsFinanceAccounting ?? false);
				ArticleStatisticsFinanceAccountingEdit = ArticleStatisticsFinanceAccountingEdit || (item?.ArticleStatisticsFinanceAccountingEdit ?? false);
				// -
				Customers = Customers || (item?.Customers ?? false);
				ModuleActivated = ModuleActivated || (item?.ModuleActivated ?? false);
				Suppliers = Suppliers || (item?.Suppliers ?? false);
				//new
				EditLagerStock = EditLagerStock || (item?.EditLagerStock ?? false);
				EditLagerMinStock = EditLagerMinStock || (item?.EditLagerMinStock ?? false);
				EditLagerCCID = EditLagerCCID || (item?.EditLagerCCID ?? false);
				EditLagerOrderProposal = EditLagerOrderProposal || (item?.EditLagerOrderProposal ?? false);
				//
				LagerIds = LagerIds.Union(item.LagerIds).ToList();

				// - Articles BOM / CP Control
				ArticlesBOMCPControlEngineering = ArticlesBOMCPControlEngineering || (item?.ArticlesBOMCPControlEngineering ?? false);
				ArticlesBOMCPControlHistory = ArticlesBOMCPControlHistory || (item?.ArticlesBOMCPControlHistory ?? false);
				ArticlesBOMCPControlQuality = ArticlesBOMCPControlQuality || (item?.ArticlesBOMCPControlQuality ?? false);

				//new -2
				//---Customers
				ViewCustomers = ViewCustomers.Value || (item?.ViewCustomers ?? false);
				AddCustomer = AddCustomer.Value || (item?.AddCustomer ?? false);
				EditCustomer = EditCustomer.Value || (item?.EditCustomer ?? false);
				ArchiveCustomer = ArchiveCustomer.Value || (item?.ArchiveCustomer ?? false);
				ConfigCustomer = ConfigCustomer.Value || (item?.ConfigCustomer ?? false);

				//TODO: Overview
				CustomerOverview = CustomerOverview.Value || (item?.CustomerOverview ?? false);
				ViewLPCustomer = ViewLPCustomer.Value || (item?.ViewLPCustomer ?? false);
				EditCustomerImage = EditCustomerImage.Value || (item?.EditCustomerImage ?? false);
				EditCustomerCoordination = EditCustomerCoordination.Value || (item?.EditCustomerCoordination ?? false);

				//TODO: Data
				CustomerData = CustomerData.Value || (item?.CustomerData ?? false);
				EditCustomerData = EditCustomerData.Value || (item?.EditCustomerData ?? false);
				//TODO: Address
				CustomerAddress = CustomerAddress.Value || (item?.CustomerAddress ?? false);
				EditCustomerAddress = EditCustomerAddress.Value || (item?.EditCustomerAddress ?? false);

				//TODO: Communication
				CustomerCommunication = CustomerCommunication.Value || (item?.CustomerCommunication ?? false);
				EditCustomerCommunication = EditCustomerCommunication.Value || (item?.EditCustomerCommunication ?? false);

				//TODO: ContactPerson
				CustomerContactPerson = CustomerContactPerson.Value || (item?.CustomerContactPerson ?? false);
				CreateCustomerContactPerson = CreateCustomerContactPerson.Value || (item?.CreateCustomerContactPerson ?? false);
				EditCustomerContactPerson = EditCustomerContactPerson.Value || (item?.EditCustomerContactPerson ?? false);
				DeleteCustomerContactPerson = DeleteCustomerContactPerson.Value || (item?.DeleteCustomerContactPerson ?? false);

				//TODO: Shipping
				CustomerShipping = CustomerShipping.Value || (item?.CustomerShipping ?? false);
				EditCustomerShipping = EditCustomerShipping.Value || (item?.EditCustomerShipping ?? false);

				//TODO: History
				CustomerHistory = CustomerHistory.Value || (item?.CustomerHistory ?? false);

				//---Suppliers

				ViewSuppliers = ViewSuppliers.Value || (item?.ViewSuppliers ?? false);
				AddSupplier = AddSupplier.Value || (item?.AddSupplier ?? false);
				EditSupplier = EditSupplier.Value || (item?.EditSupplier ?? false);
				ArchiveSupplier = ArchiveSupplier.Value || (item?.ArchiveSupplier ?? false);
				ConfigSupplier = ConfigSupplier.Value || (item?.ConfigSupplier ?? false);

				//TODO: Overview
				SupplierOverview = SupplierOverview.Value || (item?.SupplierOverview ?? false);
				ViewLPSupplier = ViewLPSupplier.Value || (item?.ViewLPSupplier ?? false);
				EditSupplierImage = EditSupplierImage.Value || (item?.EditSupplierImage ?? false);
				EditSupplierCoordination = EditSupplierCoordination.Value || (item?.EditSupplierCoordination ?? false);

				//TODO: Data
				SupplierData = SupplierData.Value || (item?.SupplierData ?? false);
				EditSupplierData = EditSupplierData.Value || (item?.EditSupplierData ?? false);
				//TODO: Address
				SupplierAddress = SupplierAddress.Value || (item?.SupplierAddress ?? false);
				EditSupplierAddress = EditSupplierAddress.Value || (item?.EditSupplierAddress ?? false);
				ViewSupplierAddressComments = ViewSupplierAddressComments.Value || (item?.ViewSupplierAddressComments ?? false);

				//TODO: Communication
				SupplierCommunication = SupplierCommunication.Value || (item?.SupplierCommunication ?? false);
				EditSupplierCommunication = EditSupplierCommunication.Value || (item?.EditSupplierCommunication ?? false);

				//TODO: ContactPerson
				SupplierContactPerson = SupplierContactPerson.Value || (item?.SupplierContactPerson ?? false);
				CreateSupplierContactPerson = CreateSupplierContactPerson.Value || (item?.CreateSupplierContactPerson ?? false);
				EditSupplierContactPerson = EditSupplierContactPerson.Value || (item?.EditSupplierContactPerson ?? false);
				DeleteSupplierContactPerson = DeleteSupplierContactPerson.Value || (item?.DeleteSupplierContactPerson ?? false);

				//TODO: Shipping
				SupplierShipping = SupplierShipping.Value || (item?.SupplierShipping ?? false);
				EditSupplierShipping = EditSupplierShipping.Value || (item?.EditSupplierShipping ?? false);

				//TODO: History
				SupplierHistory = SupplierHistory.Value || (item?.SupplierHistory ?? false);

				//---Settings
				Settings = Settings.Value || (item?.Settings ?? false);


				//TODO PricingGroup
				PricingGroup = PricingGroup.Value || (item?.PricingGroup ?? false);
				AddPricingGroup = AddPricingGroup.Value || (item?.AddPricingGroup ?? false);
				EditPricingGroup = EditPricingGroup.Value || (item?.EditPricingGroup ?? false);

				//TODO DiscountGroup
				DiscountGroup = DiscountGroup.Value || (item?.DiscountGroup ?? false);
				AddDiscountGroup = AddDiscountGroup.Value || (item?.AddDiscountGroup ?? false);
				EditDiscountGroup = EditDiscountGroup.Value || (item?.EditDiscountGroup ?? false);

				//TODO Industry
				Industry = Industry.Value || (item?.Industry ?? false);
				AddIndustry = AddIndustry.Value || (item?.AddIndustry ?? false);
				EditIndustry = EditIndustry.Value || (item?.EditIndustry ?? false);

				//TODO CustomerGroup
				CustomerGroup = CustomerGroup.Value || (item?.CustomerGroup ?? false);
				AddCustomerGroup = AddCustomerGroup.Value || (item?.AddCustomerGroup ?? false);
				EditCustomerGroup = EditCustomerGroup.Value || (item?.EditCustomerGroup ?? false);

				//TODO SupplierGroup
				SupplierGroup = SupplierGroup.Value || (item?.SupplierGroup ?? false);
				AddSupplierGroup = AddSupplierGroup.Value || (item?.AddSupplierGroup ?? false);
				EditSupplierGroup = EditSupplierGroup.Value || (item?.EditSupplierGroup ?? false);

				//TODO PayementPractises
				PayementPractises = PayementPractises.Value || (item?.PayementPractises ?? false);
				AddPayementPractises = AddPayementPractises.Value || (item?.AddPayementPractises ?? false);
				EditPayementPractises = EditPayementPractises.Value || (item?.EditPayementPractises ?? false);


				//TODO ConditionAssignment 
				ConditionAssignment = ConditionAssignment.Value || (item?.ConditionAssignment ?? false);
				AddConditionAssignment = AddConditionAssignment.Value || (item?.AddConditionAssignment ?? false);
				EditConditionAssignment = EditConditionAssignment.Value || (item?.EditConditionAssignment ?? false);


				//TODO TermsOfPayment  
				TermsOfPayment = TermsOfPayment.Value || (item?.TermsOfPayment ?? false);
				AddTermsOfPayment = AddTermsOfPayment.Value || (item?.AddTermsOfPayment ?? false);
				EditTermsOfPayment = EditTermsOfPayment.Value || (item?.EditTermsOfPayment ?? false);

				//TODO FibuFrame   
				FibuFrame = FibuFrame.Value || (item?.FibuFrame ?? false);
				AddFibuFrame = AddFibuFrame.Value || (item?.AddFibuFrame ?? false);
				EditFibuFrame = EditFibuFrame.Value || (item?.EditFibuFrame ?? false);

				//TODO SlipCircle    
				SlipCircle = SlipCircle.Value || (item?.SlipCircle ?? false);
				AddSlipCircle = AddSlipCircle.Value || (item?.AddSlipCircle ?? false);
				EditSlipCircle = EditSlipCircle.Value || (item?.EditSlipCircle ?? false);


				//TODO Currencies     
				Currencies = Currencies.Value || (item?.Currencies ?? false);
				AddCurrencies = AddCurrencies.Value || (item?.AddCurrencies ?? false);
				EditCurrencies = EditCurrencies.Value || (item?.EditCurrencies ?? false);


				//TODO ShippingMethods     
				ShippingMethods = ShippingMethods.Value || (item?.ShippingMethods ?? false);
				AddShippingMethods = AddShippingMethods.Value || (item?.AddShippingMethods ?? false);
				EditShippingMethods = EditShippingMethods.Value || (item?.EditShippingMethods ?? false);


				//TODO ContactAddress   
				ContactAddress = ContactAddress.Value || (item?.ContactAddress ?? false);
				AddContactAddress = AddContactAddress.Value || (item?.AddContactAddress ?? false);
				EditContactAddress = EditContactAddress.Value || (item?.EditContactAddress ?? false);

				//TODO ContactSalutation   
				ContactSalutation = ContactSalutation.Value || (item?.ContactSalutation ?? false);
				AddContactSalutation = AddContactSalutation.Value || (item?.AddContactSalutation ?? false);
				EditContactSalutation = EditContactSalutation.Value || (item?.EditContactSalutation ?? false);

				// - 2022-11-15
				CustomerItemNumber = CustomerItemNumber.Value || (item?.CustomerItemNumber ?? false);
				AddCustomerItemNumber = AddCustomerItemNumber.Value || (item?.AddCustomerItemNumber ?? false);
				EditCustomerItemNumber = EditCustomerItemNumber.Value || (item?.EditCustomerItemNumber ?? false);

				// - 2023-08-23
				CocType = CocType.Value || (item?.CocType ?? false);
				AddCocType = AddCocType.Value || (item?.AddCocType ?? false);
				EditCocType = EditCocType.Value || (item?.EditCocType ?? false);
				isDefault = isDefault.Value || (item?.isDefault ?? false);
				// - 2023-11-08
				ModuleAdministrator = ModuleAdministrator.Value || (item?.ModuleAdministrator ?? false);
				AddEdiConcern = AddEdiConcern.Value || (item?.AddEdiConcern ?? false);
				EdiConcern = EdiConcern.Value || (item?.EdiConcern ?? false);
				EditEdiConcern = EditEdiConcern.Value || (item?.EditEdiConcern ?? false);
				EDrawingEdit = EDrawingEdit.Value || (item?.EDrawingEdit ?? false);
				EditHourlyRate = EditHourlyRate.Value || (item?.EditHourlyRate ?? false);
				AddHourlyRate = AddHourlyRate.Value || (item?.AddHourlyRate ?? false);
				HourlyRate = HourlyRate.Value || (item?.HourlyRate ?? false);

				DownloadAllOutdatedEinkaufsPreis = DownloadAllOutdatedEinkaufsPreis.Value || (item?.DownloadAllOutdatedEinkaufsPreis ?? false);
				DownloadOutdatedEinkaufsPreis = DownloadOutdatedEinkaufsPreis.Value || (item?.DownloadOutdatedEinkaufsPreis ?? false);
				EinkaufsPreisUpdate = EinkaufsPreisUpdate.Value || (item?.EinkaufsPreisUpdate ?? false);
				// - 2024-04-11
				ArticleAddCustomerDocument = ArticleAddCustomerDocument.Value || (item?.ArticleAddCustomerDocument ?? false);
				ArticleDeleteCustomerDocument = ArticleDeleteCustomerDocument.Value || (item?.ArticleDeleteCustomerDocument ?? false);
				// - 2024-10-03
				PMModule = PMModule.Value || (item?.PMModule ?? false);
				PMViewProjectsCompact = PMViewProjectsCompact.Value || (item?.PMViewProjectsCompact ?? false);
				PMViewProjectsMedium = PMViewProjectsMedium.Value || (item?.PMViewProjectsMedium ?? false);
				PMViewProjectsDetail = PMViewProjectsDetail.Value || (item?.PMViewProjectsDetail ?? false);
				PMAddProject = PMAddProject.Value || (item?.PMAddProject ?? false);
				PMEditProject = PMEditProject.Value || (item?.PMEditProject ?? false);
				PMDeleteProject = PMDeleteProject.Value || (item?.PMDeleteProject ?? false);
				PMAddMileStone = PMAddMileStone.Value || (item?.PMAddMileStone ?? false);
				PMEditMileStone = PMEditMileStone.Value || (item?.PMEditMileStone ?? false);
				PMDeleteMileStone = PMDeleteMileStone.Value || (item?.PMDeleteMileStone ?? false);
				PMAddCable = PMAddCable.Value || (item?.PMAddCable ?? false);
				PMEditCable = PMEditCable.Value || (item?.PMEditCable ?? false);
				PMDeleteCable = PMDeleteCable.Value || (item?.PMDeleteCable ?? false);

				//- 2024-05-13
				AddRohArtikelNummer = AddRohArtikelNummer.Value || (item?.AddRohArtikelNummer ?? false);
				EditRohArtikelNummer = EditRohArtikelNummer.Value || (item?.EditRohArtikelNummer ?? false);
				DeleteRohArtikelNummer = DeleteRohArtikelNummer.Value || (item?.DeleteRohArtikelNummer ?? false);
				GetRohArtikelNummer = GetRohArtikelNummer.Value || (item?.GetRohArtikelNummer ?? false);
				// 2024-04-24
				AddArticleReference = AddArticleReference.Value || (item?.AddArticleReference ?? false);
				EditArticleReference = EditArticleReference.Value || (item?.EditArticleReference ?? false);
				ViewArticleReference = ViewArticleReference.Value || (item?.ViewArticleReference ?? false);
				RemoveArticleReference = RemoveArticleReference.Value || (item?.RemoveArticleReference ?? false);

				//2024-05-29
				OfferRequestADD = OfferRequestADD.Value || (item?.OfferRequestADD ?? false);
				OfferRequestDelete = OfferRequestDelete.Value || (item?.OfferRequestDelete ?? false);
				OfferRequestEdit = OfferRequestEdit.Value || (item?.OfferRequestEdit ?? false);
				OfferRequestEditEmail = OfferRequestEditEmail.Value || (item?.OfferRequestEditEmail ?? false);
				OfferRequestSendEmail = OfferRequestSendEmail.Value || (item?.OfferRequestSendEmail ?? false);
				OfferRequestApplyPrice = OfferRequestApplyPrice.Value || (item?.OfferRequestApplyPrice ?? false);
				OfferRequestView = OfferRequestView.Value || (item?.OfferRequestView ?? false);
				offer = offer.Value || (item?.offer ?? false);
				DeleteFiles = DeleteFiles.Value || (item?.DeleteFiles ?? false);
				AddFiles = AddFiles.Value || (item?.AddFiles ?? false);
				DownloadFiles = DownloadFiles.Value || (item?.DownloadFiles ?? false);

				SupplierAttachementGetFile = SupplierAttachementGetFile.Value || (item?.SupplierAttachementGetFile ?? false);
				SupplierAttachementAddFile = SupplierAttachementAddFile.Value || (item?.SupplierAttachementAddFile ?? false);
				SupplierAttachementRemoveFile = SupplierAttachementRemoveFile.Value || (item?.SupplierAttachementRemoveFile ?? false);


				// 2025-04-2025
				PackagingsLgtPhotoAdd = PackagingsLgtPhotoAdd.Value || (item?.PackagingsLgtPhotoAdd ?? false);
				PackagingsLgtPhotoDelete = PackagingsLgtPhotoDelete.Value || (item?.PackagingsLgtPhotoDelete ?? false);
				PackagingsLgtPhotoView = PackagingsLgtPhotoView.Value || (item?.PackagingsLgtPhotoView ?? false);

			}
		}
		public Infrastructure.Data.Entities.Tables.BSD.AccessProfileEntity ToDbEntity(int id, int mainId)
		{
			return new Infrastructure.Data.Entities.Tables.BSD.AccessProfileEntity
			{
				Id = id,
				Administration = Administration,
				Articles = Articles,
				Customers = Customers,
				ModuleActivated = ModuleActivated,
				Suppliers = Suppliers
			};
		}
	}
}