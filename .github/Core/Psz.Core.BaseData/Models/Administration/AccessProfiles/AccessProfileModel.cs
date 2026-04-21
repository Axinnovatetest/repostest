using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Administration.AccessProfiles
{
	public class AccessProfileModel
	{
		public string AccessProfileName { get; set; }
		public bool AddArticle { get; set; }
		public bool Administration { get; set; }
		public bool AltPositionsArticleBOM { get; set; }
		public bool ArchiveArticle { get; set; }
		public bool DeleteArticle { get; set; }
		public bool ArticleBOM { get; set; }
		public bool ArticleCts { get; set; }
		public bool ArticleData { get; set; }
		public bool ArticleLogistics { get; set; }
		public bool ArticleLogisticsPrices { get; set; }
		public bool ArticleOverview { get; set; }
		public bool ArticleProduction { get; set; }
		public bool ArticlePurchase { get; set; }
		public bool ArticleQuality { get; set; }
		public bool Articles { get; set; }
		public bool ArticleSales { get; set; }
		public bool ArticleSalesCustom { get; set; }
		public bool ArticleSalesItem { get; set; }
		public bool ConfigArticle { get; set; }
		public bool CreateArticlePurchase { get; set; }
		public bool CreateArticleSalesCustom { get; set; }
		public bool CreateArticleSalesItem { get; set; }
		public DateTime? CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public bool Customers { get; set; }
		public bool DeleteAltPositionsArticleBOM { get; set; }
		public bool DeleteArticleBOM { get; set; }
		public bool DeleteArticlePurchase { get; set; }
		public bool DeleteArticleSalesCustom { get; set; }
		public bool DeleteArticleSalesItem { get; set; }
		public bool EditAltPositionsArticleBOM { get; set; }
		public bool EditArticle { get; set; }
		public bool EditArticleBOM { get; set; }
		public bool EditArticleBOMPosition { get; set; }
		public bool EditArticleCts { get; set; }
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
		public int Id { get; set; }
		public bool ImportArticleBOM { get; set; }
		public bool LagerArticleLogistics { get; set; }
		public bool ModuleActivated { get; set; }
		public bool Suppliers { get; set; }
		public bool UploadAltPositionsArticleBOM { get; set; }
		public bool UploadArticleBOM { get; set; }
		public bool ValidateArticleBOM { get; set; }
		public bool ViewAltPositionsArticleBOM { get; set; }
		public bool ViewArticleLog { get; set; }
		public bool ViewArticles { get; set; }
		//new
		public bool? EditLagerStock { get; set; }
		public bool? EditLagerMinStock { get; set; }
		public bool? EditLagerCCID { get; set; }
		public bool? EditLagerOrderProposal { get; set; }
		//new -2

		//---Customers
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

		//---Suppliers

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
		//Lager list
		public List<KeyValuePair<int, string>> LagerIds { get; set; }

		// - Articles BOM / CP Control
		public bool ArticlesBOMCPControlEngineering { get; set; }
		public bool ArticlesBOMCPControlHistory { get; set; }
		public bool ArticlesBOMCPControlQuality { get; set; }
		public bool ArticlesBOMCPControl { get; set; }

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

		// - 2022-11-15
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
		// -2024-03-04 
		public bool? EDrawingEdit { get; set; }
		// - 2024-03-11
		public bool? AddHourlyRate { get; set; }
		public bool? HourlyRate { get; set; }
		public bool? EditHourlyRate { get; set; }
		//24
		public bool? DownloadAllOutdatedEinkaufsPreis { get; set; }
		public bool? DownloadOutdatedEinkaufsPreis { get; set; }
		public bool? EinkaufsPreisUpdate { get; set; }

		// - 2024-04-11
		public bool? ArticleAddCustomerDocument { get; set; }
		public bool? ArticleDeleteCustomerDocument { get; set; }

		// ArticleROH nummer 
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
		public bool? SupplierAttachementRemoveFile { get; set; }
		public bool? SupplierAttachementAddFile { get; set; }
		public bool? SupplierAttachementGetFile { get; set; }
		#endregion

		//2025-04-25
		public bool? PackagingsLgtPhotoAdd { get; set; }
		public bool? PackagingsLgtPhotoDelete { get; set; }
		public bool? PackagingsLgtPhotoView { get; set; }

		public AccessProfileModel(Infrastructure.Data.Entities.Tables.BSD.AccessProfileEntity accessProfileEntity)
		{
			if(accessProfileEntity == null)
				return;
			// -


			DownloadAllOutdatedEinkaufsPreis = accessProfileEntity.DownloadAllOutdatedEinkaufsPreis;
			DownloadOutdatedEinkaufsPreis = accessProfileEntity.DownloadOutdatedEinkaufsPreis;
			EinkaufsPreisUpdate = accessProfileEntity.EinkaufsPreisUpdate;

			AccessProfileName = accessProfileEntity.AccessProfileName;
			AddArticle = accessProfileEntity.AddArticle;
			Administration = accessProfileEntity.Administration;
			AltPositionsArticleBOM = accessProfileEntity.AltPositionsArticleBOM;
			ArchiveArticle = accessProfileEntity.ArchiveArticle;
			DeleteArticle = accessProfileEntity.DeleteArticle ?? false;
			ArticleBOM = accessProfileEntity.ArticleBOM;
			ArticleCts = accessProfileEntity.ArticleCts;
			ArticleData = accessProfileEntity.ArticleData;
			ArticleLogistics = accessProfileEntity.ArticleLogistics;
			ArticleLogisticsPrices = accessProfileEntity.ArticleLogisticsPrices ?? false;
			ArticleOverview = accessProfileEntity.ArticleOverview;
			ArticleProduction = accessProfileEntity.ArticleProduction;
			ArticlePurchase = accessProfileEntity.ArticlePurchase;
			ArticleQuality = accessProfileEntity.ArticleQuality;
			Articles = accessProfileEntity.Articles;
			ArticleSales = accessProfileEntity.ArticleSales;
			ArticleSalesCustom = accessProfileEntity.ArticleSalesCustom;
			ArticleSalesItem = accessProfileEntity.ArticleSalesItem;
			ConfigArticle = accessProfileEntity.ConfigArticle;
			CreateArticlePurchase = accessProfileEntity.CreateArticlePurchase;
			CreateArticleSalesCustom = accessProfileEntity.CreateArticleSalesCustom;
			CreateArticleSalesItem = accessProfileEntity.CreateArticleSalesItem;
			CreationTime = accessProfileEntity.CreationTime;
			CreationUserId = accessProfileEntity.CreationUserId;
			Customers = accessProfileEntity.Customers;
			DeleteAltPositionsArticleBOM = accessProfileEntity.DeleteAltPositionsArticleBOM;
			DeleteArticleBOM = accessProfileEntity.DeleteArticleBOM;
			DeleteArticlePurchase = accessProfileEntity.DeleteArticlePurchase;
			DeleteArticleSalesCustom = accessProfileEntity.DeleteArticleSalesCustom;
			DeleteArticleSalesItem = accessProfileEntity.DeleteArticleSalesItem;
			EditAltPositionsArticleBOM = accessProfileEntity.EditAltPositionsArticleBOM;
			EditArticle = accessProfileEntity.EditArticle;
			EditArticleBOM = accessProfileEntity.EditArticleBOM;
			EditArticleBOMPosition = accessProfileEntity.EditArticleBOMPosition;
			EditArticleCts = accessProfileEntity.EditArticleCts;
			EditArticleData = accessProfileEntity.EditArticleData;
			EditArticleDesignation = accessProfileEntity.EditArticleDesignation;
			EditArticleImage = accessProfileEntity.EditArticleImage;
			EditArticleLogistics = accessProfileEntity.EditArticleLogistics;
			EditArticleManager = accessProfileEntity.EditArticleManager;
			EditArticleProduction = accessProfileEntity.EditArticleProduction;
			EditArticlePurchase = accessProfileEntity.EditArticlePurchase;
			EditArticleQuality = accessProfileEntity.EditArticleQuality;
			EditArticleSalesCustom = accessProfileEntity.EditArticleSalesCustom;
			EditArticleSalesItem = accessProfileEntity.EditArticleSalesItem;
			Id = accessProfileEntity.Id;
			ImportArticleBOM = accessProfileEntity.ImportArticleBOM;
			LagerArticleLogistics = accessProfileEntity.LagerArticleLogistics;
			ModuleActivated = accessProfileEntity.ModuleActivated;
			Suppliers = accessProfileEntity.Suppliers;
			UploadAltPositionsArticleBOM = accessProfileEntity.UploadAltPositionsArticleBOM;
			UploadArticleBOM = accessProfileEntity.UploadArticleBOM;
			ValidateArticleBOM = accessProfileEntity.ValidateArticleBOM ?? false;
			ViewAltPositionsArticleBOM = accessProfileEntity.ViewAltPositionsArticleBOM;
			ViewArticleLog = accessProfileEntity.ViewArticleLog;
			ViewArticles = accessProfileEntity.ViewArticles;
			//new
			EditLagerStock = accessProfileEntity.EditLagerStock;
			EditLagerMinStock = accessProfileEntity.EditLagerMinStock;
			EditLagerCCID = accessProfileEntity.EditLagerCCID;
			EditLagerOrderProposal = accessProfileEntity.EditLagerOrderProposal;
			//
			LagerIds = accessProfileEntity.LagerIds;
			//
			//---Customers
			ViewCustomers = accessProfileEntity.ViewCustomers;
			AddCustomer = accessProfileEntity.AddCustomer;
			EditCustomer = accessProfileEntity.EditCustomer;
			ArchiveCustomer = accessProfileEntity.ArchiveCustomer;
			ConfigCustomer = accessProfileEntity.ConfigCustomer;

			//TODO: Overview
			CustomerOverview = accessProfileEntity.CustomerOverview;
			ViewLPCustomer = accessProfileEntity.ViewLPCustomer;
			EditCustomerImage = accessProfileEntity.EditCustomerImage;
			EditCustomerCoordination = accessProfileEntity.EditCustomerCoordination;

			//TODO: Data
			CustomerData = accessProfileEntity.CustomerData;
			EditCustomerData = accessProfileEntity.EditCustomerData;
			//TODO: Address
			CustomerAddress = accessProfileEntity.CustomerAddress;
			EditCustomerAddress = accessProfileEntity.EditCustomerAddress;

			//TODO: Communication
			CustomerCommunication = accessProfileEntity.CustomerCommunication;
			EditCustomerCommunication = accessProfileEntity.EditCustomerCommunication;

			//TODO: ContactPerson
			CustomerContactPerson = accessProfileEntity.CustomerContactPerson;
			CreateCustomerContactPerson = accessProfileEntity.CreateCustomerContactPerson;
			EditCustomerContactPerson = accessProfileEntity.EditCustomerContactPerson;
			DeleteCustomerContactPerson = accessProfileEntity.DeleteCustomerContactPerson;

			//TODO: Shipping
			CustomerShipping = accessProfileEntity.CustomerShipping;
			EditCustomerShipping = accessProfileEntity.EditCustomerShipping;

			//TODO: History
			CustomerHistory = accessProfileEntity.CustomerHistory;

			//---Suppliers

			ViewSuppliers = accessProfileEntity.ViewSuppliers;
			AddSupplier = accessProfileEntity.AddSupplier;
			EditSupplier = accessProfileEntity.EditSupplier;
			ArchiveSupplier = accessProfileEntity.ArchiveSupplier;
			ConfigSupplier = accessProfileEntity.ConfigSupplier;

			//TODO: Overview
			SupplierOverview = accessProfileEntity.SupplierOverview;
			ViewLPSupplier = accessProfileEntity.ViewLPSupplier;
			EditSupplierImage = accessProfileEntity.EditSupplierImage;
			EditSupplierCoordination = accessProfileEntity.EditSupplierCoordination;

			//TODO: Data
			SupplierData = accessProfileEntity.SupplierData;
			EditSupplierData = accessProfileEntity.EditSupplierData;
			//TODO: Address
			SupplierAddress = accessProfileEntity.SupplierAddress;
			EditSupplierAddress = accessProfileEntity.EditSupplierAddress;
			ViewSupplierAddressComments = accessProfileEntity.ViewSupplierAddressComments;

			//TODO: Communication
			SupplierCommunication = accessProfileEntity.SupplierCommunication;
			EditSupplierCommunication = accessProfileEntity.EditSupplierCommunication;

			//TODO: ContactPerson
			SupplierContactPerson = accessProfileEntity.SupplierContactPerson;
			CreateSupplierContactPerson = accessProfileEntity.CreateSupplierContactPerson;
			EditSupplierContactPerson = accessProfileEntity.EditSupplierContactPerson;
			DeleteSupplierContactPerson = accessProfileEntity.DeleteSupplierContactPerson;

			//TODO: Shipping
			SupplierShipping = accessProfileEntity.SupplierShipping;
			EditSupplierShipping = accessProfileEntity.EditSupplierShipping;

			//TODO: History
			SupplierHistory = accessProfileEntity.SupplierHistory;

			//---Settings
			Settings = accessProfileEntity.Settings;


			//TODO PricingGroup
			PricingGroup = accessProfileEntity.PricingGroup;
			AddPricingGroup = accessProfileEntity.AddPricingGroup;
			EditPricingGroup = accessProfileEntity.EditPricingGroup;

			//TODO DiscountGroup
			DiscountGroup = accessProfileEntity.DiscountGroup;
			AddDiscountGroup = accessProfileEntity.AddDiscountGroup;
			EditDiscountGroup = accessProfileEntity.EditDiscountGroup;

			//TODO Industry
			Industry = accessProfileEntity.Industry;
			AddIndustry = accessProfileEntity.AddIndustry;
			EditIndustry = accessProfileEntity.EditIndustry;

			//TODO CustomerGroup
			CustomerGroup = accessProfileEntity.CustomerGroup;
			AddCustomerGroup = accessProfileEntity.AddCustomerGroup;
			EditCustomerGroup = accessProfileEntity.EditCustomerGroup;

			//TODO SupplierGroup
			SupplierGroup = accessProfileEntity.SupplierGroup;
			AddSupplierGroup = accessProfileEntity.AddSupplierGroup;
			EditSupplierGroup = accessProfileEntity.EditSupplierGroup;

			//TODO PayementPractises
			PayementPractises = accessProfileEntity.PayementPractises;
			AddPayementPractises = accessProfileEntity.AddPayementPractises;
			EditPayementPractises = accessProfileEntity.EditPayementPractises;


			//TODO ConditionAssignment 
			ConditionAssignment = accessProfileEntity.ConditionAssignment;
			AddConditionAssignment = accessProfileEntity.AddConditionAssignment;
			EditConditionAssignment = accessProfileEntity.EditConditionAssignment;


			//TODO TermsOfPayment  
			TermsOfPayment = accessProfileEntity.TermsOfPayment;
			AddTermsOfPayment = accessProfileEntity.AddTermsOfPayment;
			EditTermsOfPayment = accessProfileEntity.EditTermsOfPayment;

			//TODO FibuFrame   
			FibuFrame = accessProfileEntity.FibuFrame;
			AddFibuFrame = accessProfileEntity.AddFibuFrame;
			EditFibuFrame = accessProfileEntity.EditFibuFrame;

			//TODO SlipCircle    
			SlipCircle = accessProfileEntity.SlipCircle;
			AddSlipCircle = accessProfileEntity.AddSlipCircle;
			EditSlipCircle = accessProfileEntity.EditSlipCircle;


			//TODO Currencies     
			Currencies = accessProfileEntity.Currencies;
			AddCurrencies = accessProfileEntity.AddCurrencies;
			EditCurrencies = accessProfileEntity.EditCurrencies;


			//TODO ShippingMethods     
			ShippingMethods = accessProfileEntity.ShippingMethods;
			AddShippingMethods = accessProfileEntity.AddShippingMethods;
			EditShippingMethods = accessProfileEntity.EditShippingMethods;


			//TODO ContactAddress   
			ContactAddress = accessProfileEntity.ContactAddress;
			AddContactAddress = accessProfileEntity.AddContactAddress;
			EditContactAddress = accessProfileEntity.EditContactAddress;

			//TODO ContactSalutation   
			ContactSalutation = accessProfileEntity.ContactSalutation;
			AddContactSalutation = accessProfileEntity.AddContactSalutation;
			EditContactSalutation = accessProfileEntity.EditContactSalutation;

			// - Articles BOM / CP Control
			ArticlesBOMCPControlEngineering = accessProfileEntity.ArticlesBOMCPControlEngineering ?? false;
			ArticlesBOMCPControlHistory = accessProfileEntity.ArticlesBOMCPControlHistory ?? false;
			ArticlesBOMCPControlQuality = accessProfileEntity.ArticlesBOMCPControlQuality ?? false;
			ArticlesBOMCPControl = (accessProfileEntity.ArticlesBOMCPControlQuality ?? false) || (accessProfileEntity.ArticlesBOMCPControlHistory ?? false) || (accessProfileEntity.ArticlesBOMCPControlEngineering ?? false);

			ArticleStatisticsLogistics = accessProfileEntity.ArticleStatisticsLogistics;
			ArticleStatisticsLogisticsEdit = accessProfileEntity.ArticleStatisticsLogisticsEdit;
			ArticleStatisticsEngineering = accessProfileEntity.ArticleStatisticsEngineering;
			ArticleStatisticsEngineeringEdit = accessProfileEntity.ArticleStatisticsEngineeringEdit;
			ArticleStatisticsPurchase = accessProfileEntity.ArticleStatisticsPurchase;
			ArticleStatisticsPurchaseEdit = accessProfileEntity.ArticleStatisticsPurchaseEdit;
			ArticleStatisticsTechnic = accessProfileEntity.ArticleStatisticsTechnic;
			ArticleStatisticsTechnicEdit = accessProfileEntity.ArticleStatisticsTechnicEdit;
			ArticleStatisticsFinanceAccounting = accessProfileEntity.ArticleStatisticsFinanceAccounting;
			ArticleStatisticsFinanceAccountingEdit = accessProfileEntity.ArticleStatisticsFinanceAccountingEdit;

			// - 2022-11-15
			CustomerItemNumber = accessProfileEntity.CustomerItemNumber;
			AddCustomerItemNumber = accessProfileEntity.AddCustomerItemNumber;
			EditCustomerItemNumber = accessProfileEntity.EditCustomerItemNumber;

			// - 2023-08-23
			CocType = accessProfileEntity.CocType;
			AddCocType = accessProfileEntity.AddCocType;
			EditCocType = accessProfileEntity.EditCocType;
			isDefault = accessProfileEntity.isDefault;
			// - 2023-11-08
			ModuleAdministrator = accessProfileEntity.ModuleAdministrator;
			AddEdiConcern = accessProfileEntity.AddEdiConcern;
			EdiConcern = accessProfileEntity.EdiConcern;
			EditEdiConcern = accessProfileEntity.EditEdiConcern;
			EDrawingEdit = accessProfileEntity.EDrawingEdit;
			AddHourlyRate = accessProfileEntity.AddHourlyRate;
			HourlyRate = accessProfileEntity.HourlyRate;
			EditHourlyRate = accessProfileEntity.EditHourlyRate;

			DownloadAllOutdatedEinkaufsPreis = accessProfileEntity.DownloadAllOutdatedEinkaufsPreis;
			DownloadOutdatedEinkaufsPreis = accessProfileEntity.DownloadOutdatedEinkaufsPreis;
			EinkaufsPreisUpdate = accessProfileEntity.EinkaufsPreisUpdate;

			// - 2024-04-11
			ArticleAddCustomerDocument = accessProfileEntity.ArticleAddCustomerDocument;
			ArticleDeleteCustomerDocument = accessProfileEntity.ArticleDeleteCustomerDocument;

			AddArticleReference = accessProfileEntity.AddArticleReference;
			RemoveArticleReference = accessProfileEntity.RemoveArticleReference;
			EditArticleReference = accessProfileEntity.EditArticleReference;
			ViewArticleReference = accessProfileEntity.ViewArticleReference;

			OfferRequestADD = accessProfileEntity.OfferRequestADD;
			OfferRequestDelete = accessProfileEntity.OfferRequestDelete;
			OfferRequestEdit = accessProfileEntity.OfferRequestEdit;
			OfferRequestEditEmail = accessProfileEntity.OfferRequestEditEmail;
			OfferRequestSendEmail = accessProfileEntity.OfferRequestSendEmail;
			OfferRequestView = accessProfileEntity.OfferRequestView;
			offer = accessProfileEntity.offer;
			OfferRequestApplyPrice = accessProfileEntity.OfferRequestApplyPrice;
			DeleteFiles = accessProfileEntity.DeleteFiles;
			AddFiles = accessProfileEntity.AddFiles;
			DownloadFiles = accessProfileEntity.DownloadFiles;



			ArticleStatistics = (accessProfileEntity.ArticleStatistics) ||
				(accessProfileEntity.ArticleStatisticsLogistics) ||
				(accessProfileEntity.ArticleStatisticsLogisticsEdit) ||
				(accessProfileEntity.ArticleStatisticsEngineering) ||
				(accessProfileEntity.ArticleStatisticsEngineeringEdit) ||
				(accessProfileEntity.ArticleStatisticsPurchase) ||
				(accessProfileEntity.ArticleStatisticsPurchaseEdit) ||
				(accessProfileEntity.ArticleStatisticsTechnic) ||
				(accessProfileEntity.ArticleStatisticsTechnicEdit) ||
				(accessProfileEntity.ArticleStatisticsFinanceAccounting) ||
				(accessProfileEntity.ArticleStatisticsFinanceAccountingEdit);

			//TODO RohArtikel
			AddRohArtikelNummer = accessProfileEntity.AddRohArtikelNummer;
			EditRohArtikelNummer = accessProfileEntity.EditRohArtikelNummer;
			DeleteRohArtikelNummer = accessProfileEntity.DeleteRohArtikelNummer;
			GetRohArtikelNummer = accessProfileEntity.GetRohArtikelNummer;

			// - 2024-10-03
			PMModule = accessProfileEntity.PMModule;
			PMViewProjectsCompact = accessProfileEntity.PMViewProjectsCompact;
			PMViewProjectsMedium = accessProfileEntity.PMViewProjectsMedium;
			PMViewProjectsDetail = accessProfileEntity.PMViewProjectsDetail;
			PMAddProject = accessProfileEntity.PMAddProject;
			PMEditProject = accessProfileEntity.PMEditProject;
			PMDeleteProject = accessProfileEntity.PMDeleteProject;
			PMAddMileStone = accessProfileEntity.PMAddMileStone;
			PMEditMileStone = accessProfileEntity.PMEditMileStone;
			PMDeleteMileStone = accessProfileEntity.PMDeleteMileStone;
			PMAddCable = accessProfileEntity.PMAddCable;
			PMEditCable = accessProfileEntity.PMEditCable;
			PMDeleteCable = accessProfileEntity.PMEditCable;
			SupplierAttachementRemoveFile = accessProfileEntity.SupplierAttachementRemoveFile;
			SupplierAttachementAddFile = accessProfileEntity.SupplierAttachementAddFile;
			SupplierAttachementGetFile = accessProfileEntity.SupplierAttachementGetFile;

			//2025-04-25
			PackagingsLgtPhotoAdd = accessProfileEntity.PackagingsLgtPhotoAdd;
			PackagingsLgtPhotoDelete = accessProfileEntity.PackagingsLgtPhotoDelete;
			PackagingsLgtPhotoView = accessProfileEntity.PackagingsLgtPhotoView;
		}
		public Infrastructure.Data.Entities.Tables.BSD.AccessProfileEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.AccessProfileEntity
			{
				AccessProfileName = AccessProfileName,
				AddArticle = AddArticle,
				Administration = Administration,
				AltPositionsArticleBOM = AltPositionsArticleBOM,
				ArchiveArticle = ArchiveArticle,
				DeleteArticle = DeleteArticle,
				ArticleBOM = ArticleBOM,
				ArticleCts = ArticleCts,
				ArticleData = ArticleData,
				ArticleLogistics = ArticleLogistics,
				ArticleLogisticsPrices = ArticleLogisticsPrices,
				ArticleOverview = ArticleOverview,
				ArticleProduction = ArticleProduction,
				ArticlePurchase = ArticlePurchase,
				ArticleQuality = ArticleQuality,
				Articles = Articles,
				ArticleSales = ArticleSales,
				ArticleSalesCustom = ArticleSalesCustom,
				ArticleSalesItem = ArticleSalesItem,
				ConfigArticle = ConfigArticle,
				CreateArticlePurchase = CreateArticlePurchase,
				CreateArticleSalesCustom = CreateArticleSalesCustom,
				CreateArticleSalesItem = CreateArticleSalesItem,
				CreationTime = CreationTime,
				CreationUserId = CreationUserId,
				Customers = Customers,
				DeleteAltPositionsArticleBOM = DeleteAltPositionsArticleBOM,
				DeleteArticleBOM = DeleteArticleBOM,
				DeleteArticlePurchase = DeleteArticlePurchase,
				DeleteArticleSalesCustom = DeleteArticleSalesCustom,
				DeleteArticleSalesItem = DeleteArticleSalesItem,
				EditAltPositionsArticleBOM = EditAltPositionsArticleBOM,
				EditArticle = EditArticle,
				EditArticleBOM = EditArticleBOM,
				EditArticleBOMPosition = EditArticleBOMPosition,
				EditArticleCts = EditArticleCts,
				EditArticleData = EditArticleData,
				EditArticleDesignation = EditArticleDesignation,
				EditArticleImage = EditArticleImage,
				EditArticleLogistics = EditArticleLogistics,
				EditArticleManager = EditArticleManager,
				EditArticleProduction = EditArticleProduction,
				EditArticlePurchase = EditArticlePurchase,
				EditArticleQuality = EditArticleQuality,
				EditArticleSalesCustom = EditArticleSalesCustom,
				EditArticleSalesItem = EditArticleSalesItem,
				Id = Id,
				ImportArticleBOM = ImportArticleBOM,
				LagerArticleLogistics = LagerArticleLogistics,
				ModuleActivated = ModuleActivated,
				Suppliers = Suppliers,
				UploadAltPositionsArticleBOM = UploadAltPositionsArticleBOM,
				UploadArticleBOM = UploadArticleBOM,
				ValidateArticleBOM = ValidateArticleBOM,
				ViewAltPositionsArticleBOM = ViewAltPositionsArticleBOM,
				ViewArticleLog = ViewArticleLog,
				ViewArticles = ViewArticles,
				//new
				EditLagerStock = EditArticleLogistics == false ? false : EditLagerStock,
				EditLagerMinStock = EditArticleLogistics == false ? false : EditLagerMinStock ?? false,
				EditLagerCCID = EditArticleLogistics == false ? false : EditLagerCCID,
				EditLagerOrderProposal = EditArticleLogistics == false ? false : EditLagerOrderProposal,
				//
				//---Customers
				ViewCustomers = ViewCustomers,
				AddCustomer = AddCustomer,
				EditCustomer = EditCustomer,
				ArchiveCustomer = ArchiveCustomer,
				ConfigCustomer = ConfigCustomer,

				//TODO: Overview
				CustomerOverview = CustomerOverview,
				ViewLPCustomer = ViewLPCustomer,
				EditCustomerImage = EditCustomerImage,
				EditCustomerCoordination = EditCustomerCoordination,

				//TODO: Data
				CustomerData = CustomerData,
				EditCustomerData = EditCustomerData,
				//TODO: Address
				CustomerAddress = CustomerAddress,
				EditCustomerAddress = EditCustomerAddress,

				//TODO: Communication
				CustomerCommunication = CustomerCommunication,
				EditCustomerCommunication = EditCustomerCommunication,

				//TODO: ContactPerson
				CustomerContactPerson = CustomerContactPerson,
				CreateCustomerContactPerson = CreateCustomerContactPerson,
				EditCustomerContactPerson = EditCustomerContactPerson,
				DeleteCustomerContactPerson = DeleteCustomerContactPerson,

				//TODO: Shipping
				CustomerShipping = CustomerShipping,
				EditCustomerShipping = EditCustomerShipping,

				//TODO: History
				CustomerHistory = CustomerHistory,

				//---Suppliers

				ViewSuppliers = ViewSuppliers,
				AddSupplier = AddSupplier,
				EditSupplier = EditSupplier,
				ArchiveSupplier = ArchiveSupplier,
				ConfigSupplier = ConfigSupplier,

				//TODO: Overview
				SupplierOverview = SupplierOverview,
				ViewLPSupplier = ViewLPSupplier,
				EditSupplierImage = EditSupplierImage,
				EditSupplierCoordination = EditSupplierCoordination,

				//TODO: Data
				SupplierData = SupplierData,
				EditSupplierData = EditSupplierData,
				//TODO: Address
				SupplierAddress = SupplierAddress,
				EditSupplierAddress = EditSupplierAddress,
				ViewSupplierAddressComments = ViewSupplierAddressComments,

				//TODO: Communication
				SupplierCommunication = SupplierCommunication,
				EditSupplierCommunication = EditSupplierCommunication,

				//TODO: ContactPerson
				SupplierContactPerson = SupplierContactPerson,
				CreateSupplierContactPerson = CreateSupplierContactPerson,
				EditSupplierContactPerson = EditSupplierContactPerson,
				DeleteSupplierContactPerson = DeleteSupplierContactPerson,

				//TODO: Shipping
				SupplierShipping = SupplierShipping,
				EditSupplierShipping = EditSupplierShipping,

				//TODO: History
				SupplierHistory = SupplierHistory,

				//---Settings
				Settings = Settings,


				//TODO PricingGroup
				PricingGroup = PricingGroup,
				AddPricingGroup = AddPricingGroup,
				EditPricingGroup = EditPricingGroup,

				//TODO DiscountGroup
				DiscountGroup = DiscountGroup,
				AddDiscountGroup = AddDiscountGroup,
				EditDiscountGroup = EditDiscountGroup,

				//TODO Industry
				Industry = Industry,
				AddIndustry = AddIndustry,
				EditIndustry = EditIndustry,

				//TODO CustomerGroup
				CustomerGroup = CustomerGroup,
				AddCustomerGroup = AddCustomerGroup,
				EditCustomerGroup = EditCustomerGroup,

				//TODO SupplierGroup
				SupplierGroup = SupplierGroup,
				AddSupplierGroup = AddSupplierGroup,
				EditSupplierGroup = EditSupplierGroup,

				//TODO PayementPractises
				PayementPractises = PayementPractises,
				AddPayementPractises = AddPayementPractises,
				EditPayementPractises = EditPayementPractises,


				//TODO ConditionAssignment 
				ConditionAssignment = ConditionAssignment,
				AddConditionAssignment = AddConditionAssignment,
				EditConditionAssignment = EditConditionAssignment,


				//TODO TermsOfPayment  
				TermsOfPayment = TermsOfPayment,
				AddTermsOfPayment = AddTermsOfPayment,
				EditTermsOfPayment = EditTermsOfPayment,

				//TODO FibuFrame   
				FibuFrame = FibuFrame,
				AddFibuFrame = AddFibuFrame,
				EditFibuFrame = EditFibuFrame,

				//TODO SlipCircle    
				SlipCircle = SlipCircle,
				AddSlipCircle = AddSlipCircle,
				EditSlipCircle = EditSlipCircle,


				//TODO Currencies     
				Currencies = Currencies,
				AddCurrencies = AddCurrencies,
				EditCurrencies = EditCurrencies,


				//TODO ShippingMethods     
				ShippingMethods = ShippingMethods,
				AddShippingMethods = AddShippingMethods,
				EditShippingMethods = EditShippingMethods,


				//TODO ContactAddress   
				ContactAddress = ContactAddress,
				AddContactAddress = AddContactAddress,
				EditContactAddress = EditContactAddress,

				//TODO ContactSalutation   
				ContactSalutation = ContactSalutation,
				AddContactSalutation = AddContactSalutation,
				EditContactSalutation = EditContactSalutation,


				// - Articles BOM / CP Control
				ArticlesBOMCPControlEngineering = ArticlesBOMCPControlEngineering,
				ArticlesBOMCPControlHistory = ArticlesBOMCPControlHistory,
				ArticlesBOMCPControlQuality = ArticlesBOMCPControlQuality,

				ArticleStatisticsLogistics = ArticleStatisticsLogistics,
				ArticleStatisticsLogisticsEdit = ArticleStatisticsLogisticsEdit,
				ArticleStatisticsEngineering = ArticleStatisticsEngineering,
				ArticleStatisticsEngineeringEdit = ArticleStatisticsEngineeringEdit,
				ArticleStatisticsPurchase = ArticleStatisticsPurchase,
				ArticleStatisticsPurchaseEdit = ArticleStatisticsPurchaseEdit,
				ArticleStatisticsTechnic = ArticleStatisticsTechnic,
				ArticleStatisticsTechnicEdit = ArticleStatisticsTechnicEdit,
				ArticleStatistics = ArticleStatistics,
				ArticleStatisticsFinanceAccounting = ArticleStatisticsFinanceAccounting,
				ArticleStatisticsFinanceAccountingEdit = ArticleStatisticsFinanceAccountingEdit,

				// - 2022-11-15
				CustomerItemNumber = CustomerItemNumber,
				AddCustomerItemNumber = AddCustomerItemNumber,
				EditCustomerItemNumber = EditCustomerItemNumber,
				// - 2023-08-23
				CocType = CocType,
				AddCocType = AddCocType,
				EditCocType = EditCocType,
				isDefault = isDefault,
				// - 2023-11-08
				ModuleAdministrator = ModuleAdministrator,
				AddEdiConcern = AddEdiConcern,
				EdiConcern = EdiConcern,
				EditEdiConcern = EditEdiConcern,
				// - 2024-03-04
				EDrawingEdit = EDrawingEdit,
				AddHourlyRate = AddHourlyRate,
				HourlyRate = HourlyRate,
				EditHourlyRate = EditHourlyRate,

				DownloadAllOutdatedEinkaufsPreis = DownloadAllOutdatedEinkaufsPreis,
				DownloadOutdatedEinkaufsPreis = DownloadOutdatedEinkaufsPreis,
				EinkaufsPreisUpdate = EinkaufsPreisUpdate,

				// - 2024-04-11
				ArticleAddCustomerDocument = ArticleAddCustomerDocument,
				ArticleDeleteCustomerDocument = ArticleDeleteCustomerDocument,

				// - 2024-05-13
				AddRohArtikelNummer = AddRohArtikelNummer,
				EditRohArtikelNummer = EditRohArtikelNummer,
				DeleteRohArtikelNummer = DeleteRohArtikelNummer,
				GetRohArtikelNummer = GetRohArtikelNummer,
				// 2024-04-24
				ViewArticleReference = ViewArticleReference,
				AddArticleReference = AddArticleReference,
				EditArticleReference = EditArticleReference,
				RemoveArticleReference = RemoveArticleReference,

				OfferRequestADD = OfferRequestADD,
				OfferRequestDelete = OfferRequestDelete,
				OfferRequestEdit = OfferRequestEdit,
				OfferRequestEditEmail = OfferRequestEditEmail,
				OfferRequestSendEmail = OfferRequestSendEmail,
				OfferRequestView = OfferRequestView,
				OfferRequestApplyPrice = OfferRequestApplyPrice,
				offer = offer,

				// - 2024-10-03
				PMModule = PMModule,
				PMViewProjectsCompact = PMViewProjectsCompact,
				PMViewProjectsMedium = PMViewProjectsMedium,
				PMViewProjectsDetail = PMViewProjectsDetail,
				PMAddProject = PMAddProject,
				PMEditProject = PMEditProject,
				PMDeleteProject = PMDeleteProject,
				PMAddMileStone = PMAddMileStone,
				PMEditMileStone = PMEditMileStone,
				PMDeleteMileStone = PMDeleteMileStone,
				PMAddCable = PMAddCable,
				PMEditCable = PMEditCable,
				PMDeleteCable = PMDeleteCable,
				DeleteFiles = DeleteFiles,
				AddFiles = AddFiles,
				DownloadFiles = DownloadFiles,
				SupplierAttachementGetFile = SupplierAttachementGetFile,
				SupplierAttachementAddFile = SupplierAttachementAddFile,
				SupplierAttachementRemoveFile = SupplierAttachementRemoveFile,

				//2025-04-25
				PackagingsLgtPhotoAdd = PackagingsLgtPhotoAdd,
				PackagingsLgtPhotoDelete = PackagingsLgtPhotoDelete,
				PackagingsLgtPhotoView = PackagingsLgtPhotoView
			};
		}
	}
}
