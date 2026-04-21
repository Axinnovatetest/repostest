using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class AccessProfileEntity
	{
		public string AccessProfileName { get; set; }
		public bool AddArticle { get; set; }
		public bool? AddArticleReference { get; set; }
		public bool? AddBCR { get; set; }
		public bool? AddCocType { get; set; }
		public bool? AddConditionAssignment { get; set; }
		public bool? AddContactAddress { get; set; }
		public bool? AddContactSalutation { get; set; }
		public bool? AddCurrencies { get; set; }
		public bool? AddCustomer { get; set; }
		public bool? AddCustomerGroup { get; set; }
		public bool? AddCustomerItemNumber { get; set; }
		public bool? AddDiscountGroup { get; set; }
		public bool? AddEdiConcern { get; set; }
		public bool? AddFibuFrame { get; set; }
		public bool? AddFiles { get; set; }
		public bool? AddHourlyRate { get; set; }
		public bool? AddIndustry { get; set; }
		public bool? AddPayementPractises { get; set; }
		public bool? AddPricingGroup { get; set; }
		public bool? AddRohArtikelNummer { get; set; }
		public bool? AddShippingMethods { get; set; }
		public bool? AddSlipCircle { get; set; }
		public bool? AddSupplier { get; set; }
		public bool? AddSupplierGroup { get; set; }
		public bool? AddTermsOfPayment { get; set; }
		public bool Administration { get; set; }
		public bool AltPositionsArticleBOM { get; set; }
		public bool ArchiveArticle { get; set; }
		public bool? ArchiveCustomer { get; set; }
		public bool? ArchiveSupplier { get; set; }
		public bool? ArticleAddCustomerDocument { get; set; }
		public bool ArticleBOM { get; set; }
		public bool ArticleCts { get; set; }
		public bool ArticleData { get; set; }
		public bool? ArticleDeleteCustomerDocument { get; set; }
		public bool ArticleLogistics { get; set; }
		public bool? ArticleLogisticsPrices { get; set; }
		public bool ArticleOverview { get; set; }
		public bool ArticleProduction { get; set; }
		public bool ArticlePurchase { get; set; }
		public bool ArticleQuality { get; set; }
		public bool Articles { get; set; }
		public bool ArticleSales { get; set; }
		public bool ArticleSalesCustom { get; set; }
		public bool ArticleSalesItem { get; set; }
		public bool? ArticlesBOMCPControlEngineering { get; set; }
		public bool? ArticlesBOMCPControlHistory { get; set; }
		public bool? ArticlesBOMCPControlQuality { get; set; }
		public bool ArticleStatistics { get; set; }
		public bool ArticleStatisticsEngineering { get; set; }
		public bool ArticleStatisticsEngineeringEdit { get; set; }
		public bool ArticleStatisticsFinanceAccounting { get; set; }
		public bool ArticleStatisticsFinanceAccountingEdit { get; set; }
		public bool ArticleStatisticsLogistics { get; set; }
		public bool ArticleStatisticsLogisticsEdit { get; set; }
		public bool ArticleStatisticsPurchase { get; set; }
		public bool ArticleStatisticsPurchaseEdit { get; set; }
		public bool ArticleStatisticsTechnic { get; set; }
		public bool ArticleStatisticsTechnicEdit { get; set; }
		public bool? BomChangeRequests { get; set; }
		public bool? CocType { get; set; }
		public bool? ConditionAssignment { get; set; }
		public bool ConfigArticle { get; set; }
		public bool? ConfigCustomer { get; set; }
		public bool? ConfigSupplier { get; set; }
		public bool? ContactAddress { get; set; }
		public bool? ContactSalutation { get; set; }
		public bool CreateArticlePurchase { get; set; }
		public bool CreateArticleSalesCustom { get; set; }
		public bool CreateArticleSalesItem { get; set; }
		public bool? CreateCustomerContactPerson { get; set; }
		public bool? CreateSupplierContactPerson { get; set; }
		public DateTime? CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public bool? Currencies { get; set; }
		public bool? CustomerAddress { get; set; }
		public bool? CustomerCommunication { get; set; }
		public bool? CustomerContactPerson { get; set; }
		public bool? CustomerData { get; set; }
		public bool? CustomerGroup { get; set; }
		public bool? CustomerHistory { get; set; }
		public bool? CustomerItemNumber { get; set; }
		public bool? CustomerOverview { get; set; }
		public bool Customers { get; set; }
		public bool? CustomerShipping { get; set; }
		public bool DeleteAltPositionsArticleBOM { get; set; }
		public bool? DeleteArticle { get; set; }
		public bool DeleteArticleBOM { get; set; }
		public bool DeleteArticlePurchase { get; set; }
		public bool DeleteArticleSalesCustom { get; set; }
		public bool DeleteArticleSalesItem { get; set; }
		public bool? DeleteBCR { get; set; }
		public bool? DeleteCustomerContactPerson { get; set; }
		public bool? DeleteFiles { get; set; }
		public bool? DeleteRohArtikelNummer { get; set; }
		public bool? DeleteSupplierContactPerson { get; set; }
		public bool? DiscountGroup { get; set; }
		public bool? DownloadAllOutdatedEinkaufsPreis { get; set; }
		public bool? DownloadFiles { get; set; }
		public bool? DownloadOutdatedEinkaufsPreis { get; set; }
		public bool? EdiConcern { get; set; }
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
		public bool? EditArticleReference { get; set; }
		public bool EditArticleSalesCustom { get; set; }
		public bool EditArticleSalesItem { get; set; }
		public bool? EditCocType { get; set; }
		public bool? EditConditionAssignment { get; set; }
		public bool? EditContactAddress { get; set; }
		public bool? EditContactSalutation { get; set; }
		public bool? EditCurrencies { get; set; }
		public bool? EditCustomer { get; set; }
		public bool? EditCustomerAddress { get; set; }
		public bool? EditCustomerCommunication { get; set; }
		public bool? EditCustomerContactPerson { get; set; }
		public bool? EditCustomerCoordination { get; set; }
		public bool? EditCustomerData { get; set; }
		public bool? EditCustomerGroup { get; set; }
		public bool? EditCustomerImage { get; set; }
		public bool? EditCustomerItemNumber { get; set; }
		public bool? EditCustomerShipping { get; set; }
		public bool? EditDiscountGroup { get; set; }
		public bool? EditEdiConcern { get; set; }
		public bool? EditFibuFrame { get; set; }
		public bool? EditHourlyRate { get; set; }
		public bool? EditIndustry { get; set; }
		public bool? EditLagerCCID { get; set; }
		public bool EditLagerMinStock { get; set; }
		public bool? EditLagerOrderProposal { get; set; }
		public bool? EditLagerStock { get; set; }
		public bool? EditPayementPractises { get; set; }
		public bool? EditPricingGroup { get; set; }
		public bool? EditRohArtikelNummer { get; set; }
		public bool? EditShippingMethods { get; set; }
		public bool? EditSlipCircle { get; set; }
		public bool? EditSupplier { get; set; }
		public bool? EditSupplierAddress { get; set; }
		public bool? EditSupplierCommunication { get; set; }
		public bool? EditSupplierContactPerson { get; set; }
		public bool? EditSupplierCoordination { get; set; }
		public bool? EditSupplierData { get; set; }
		public bool? EditSupplierGroup { get; set; }
		public bool? EditSupplierImage { get; set; }
		public bool? EditSupplierShipping { get; set; }
		public bool? EditTermsOfPayment { get; set; }
		public bool? EDrawingEdit { get; set; }
		public bool? EinkaufsPreisUpdate { get; set; }
		public bool? FibuFrame { get; set; }
		public bool? GetRohArtikelNummer { get; set; }
		public bool? HourlyRate { get; set; }
		public int Id { get; set; }
		public bool ImportArticleBOM { get; set; }
		public bool? Industry { get; set; }
		public bool? isDefault { get; set; }
		public bool LagerArticleLogistics { get; set; }
		public bool ModuleActivated { get; set; }
		public bool? ModuleAdministrator { get; set; }
		public bool? offer { get; set; }
		public bool? OfferRequestADD { get; set; }
		public bool? OfferRequestApplyPrice { get; set; }
		public bool? OfferRequestDelete { get; set; }
		public bool? OfferRequestEdit { get; set; }
		public bool? OfferRequestEditEmail { get; set; }
		public bool? OfferRequestSendEmail { get; set; }
		public bool? OfferRequestView { get; set; }
		public bool? PayementPractises { get; set; }
		public bool? PMAddCable { get; set; }
		public bool? PMAddMileStone { get; set; }
		public bool? PMAddProject { get; set; }
		public bool? PMDeleteCable { get; set; }
		public bool? PMDeleteMileStone { get; set; }
		public bool? PMDeleteProject { get; set; }
		public bool? PMEditCable { get; set; }
		public bool? PMEditMileStone { get; set; }
		public bool? PMEditProject { get; set; }
		public bool? PMModule { get; set; }
		public bool? PMViewProjectsCompact { get; set; }
		public bool? PMViewProjectsDetail { get; set; }
		public bool? PMViewProjectsMedium { get; set; }
		public bool? PricingGroup { get; set; }
		public bool? RemoveArticleReference { get; set; }
		public bool? Settings { get; set; }
		public bool? ShippingMethods { get; set; }
		public bool? SlipCircle { get; set; }
		public bool? SupplierAddress { get; set; }
		public bool? SupplierCommunication { get; set; }
		public bool? SupplierContactPerson { get; set; }
		public bool? SupplierData { get; set; }
		public bool? SupplierGroup { get; set; }
		public bool? SupplierHistory { get; set; }
		public bool? SupplierOverview { get; set; }
		public bool Suppliers { get; set; }
		public bool? SupplierShipping { get; set; }
		public bool? TermsOfPayment { get; set; }
		public bool UploadAltPositionsArticleBOM { get; set; }
		public bool UploadArticleBOM { get; set; }
		public bool? ValidateArticleBOM { get; set; }
		public bool? ValidateBCR { get; set; }
		public bool ViewAltPositionsArticleBOM { get; set; }
		public bool ViewArticleLog { get; set; }
		public bool? ViewArticleReference { get; set; }
		public bool ViewArticles { get; set; }
		public bool? ViewBCR { get; set; }
		public bool? ViewCustomers { get; set; }
		public bool? ViewLPCustomer { get; set; }
		public bool? ViewLPSupplier { get; set; }
		public bool? ViewSupplierAddressComments { get; set; }
		public bool? ViewSuppliers { get; set; }
		// --> 
		public bool? SupplierAttachementRemoveFile { get; set; }
		public bool? SupplierAttachementAddFile { get; set; }
		public bool? SupplierAttachementGetFile { get; set; }
		public List<KeyValuePair<int, string>> LagerIds { get; set; }
		//2025-04-25
		public bool? PackagingsLgtPhotoAdd { get; set; }
		public bool? PackagingsLgtPhotoDelete { get; set; }
		public bool? PackagingsLgtPhotoView { get; set; }
		public AccessProfileEntity() { }

		public AccessProfileEntity(DataRow dataRow)
		{
			AccessProfileName = (dataRow["AccessProfileName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AccessProfileName"]);
			AddArticle = Convert.ToBoolean(dataRow["AddArticle"]);
			AddArticleReference = (dataRow["AddArticleReference"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddArticleReference"]);
			AddBCR = (dataRow["AddBCR"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddBCR"]);
			AddCocType = (dataRow["AddCocType"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddCocType"]);
			AddConditionAssignment = (dataRow["AddConditionAssignment"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddConditionAssignment"]);
			AddContactAddress = (dataRow["AddContactAddress"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddContactAddress"]);
			AddContactSalutation = (dataRow["AddContactSalutation"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddContactSalutation"]);
			AddCurrencies = (dataRow["AddCurrencies"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddCurrencies"]);
			AddCustomer = (dataRow["AddCustomer"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddCustomer"]);
			AddCustomerGroup = (dataRow["AddCustomerGroup"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddCustomerGroup"]);
			AddCustomerItemNumber = (dataRow["AddCustomerItemNumber"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddCustomerItemNumber"]);
			AddDiscountGroup = (dataRow["AddDiscountGroup"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddDiscountGroup"]);
			AddEdiConcern = (dataRow["AddEdiConcern"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddEdiConcern"]);
			AddFibuFrame = (dataRow["AddFibuFrame"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddFibuFrame"]);
			AddFiles = (dataRow["AddFiles"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddFiles"]);
			AddHourlyRate = (dataRow["AddHourlyRate"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddHourlyRate"]);
			AddIndustry = (dataRow["AddIndustry"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddIndustry"]);
			AddPayementPractises = (dataRow["AddPayementPractises"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddPayementPractises"]);
			AddPricingGroup = (dataRow["AddPricingGroup"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddPricingGroup"]);
			AddRohArtikelNummer = (dataRow["AddRohArtikelNummer"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddRohArtikelNummer"]);
			AddShippingMethods = (dataRow["AddShippingMethods"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddShippingMethods"]);
			AddSlipCircle = (dataRow["AddSlipCircle"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddSlipCircle"]);
			AddSupplier = (dataRow["AddSupplier"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddSupplier"]);
			AddSupplierGroup = (dataRow["AddSupplierGroup"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddSupplierGroup"]);
			AddTermsOfPayment = (dataRow["AddTermsOfPayment"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["AddTermsOfPayment"]);
			Administration = Convert.ToBoolean(dataRow["Administration"]);
			AltPositionsArticleBOM = Convert.ToBoolean(dataRow["AltPositionsArticleBOM"]);
			ArchiveArticle = Convert.ToBoolean(dataRow["ArchiveArticle"]);
			ArchiveCustomer = (dataRow["ArchiveCustomer"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ArchiveCustomer"]);
			ArchiveSupplier = (dataRow["ArchiveSupplier"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ArchiveSupplier"]);
			ArticleAddCustomerDocument = (dataRow["ArticleAddCustomerDocument"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ArticleAddCustomerDocument"]);
			ArticleBOM = Convert.ToBoolean(dataRow["ArticleBOM"]);
			ArticleCts = Convert.ToBoolean(dataRow["ArticleCts"]);
			ArticleData = Convert.ToBoolean(dataRow["ArticleData"]);
			ArticleDeleteCustomerDocument = (dataRow["ArticleDeleteCustomerDocument"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ArticleDeleteCustomerDocument"]);
			ArticleLogistics = Convert.ToBoolean(dataRow["ArticleLogistics"]);
			ArticleLogisticsPrices = (dataRow["ArticleLogisticsPrices"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ArticleLogisticsPrices"]);
			ArticleOverview = Convert.ToBoolean(dataRow["ArticleOverview"]);
			ArticleProduction = Convert.ToBoolean(dataRow["ArticleProduction"]);
			ArticlePurchase = Convert.ToBoolean(dataRow["ArticlePurchase"]);
			ArticleQuality = Convert.ToBoolean(dataRow["ArticleQuality"]);
			Articles = Convert.ToBoolean(dataRow["Articles"]);
			ArticleSales = Convert.ToBoolean(dataRow["ArticleSales"]);
			ArticleSalesCustom = Convert.ToBoolean(dataRow["ArticleSalesCustom"]);
			ArticleSalesItem = Convert.ToBoolean(dataRow["ArticleSalesItem"]);
			ArticlesBOMCPControlEngineering = (dataRow["ArticlesBOMCPControlEngineering"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ArticlesBOMCPControlEngineering"]);
			ArticlesBOMCPControlHistory = (dataRow["ArticlesBOMCPControlHistory"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ArticlesBOMCPControlHistory"]);
			ArticlesBOMCPControlQuality = (dataRow["ArticlesBOMCPControlQuality"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ArticlesBOMCPControlQuality"]);
			ArticleStatistics = Convert.ToBoolean(dataRow["ArticleStatistics"]);
			ArticleStatisticsEngineering = Convert.ToBoolean(dataRow["ArticleStatisticsEngineering"]);
			ArticleStatisticsEngineeringEdit = Convert.ToBoolean(dataRow["ArticleStatisticsEngineeringEdit"]);
			ArticleStatisticsFinanceAccounting = Convert.ToBoolean(dataRow["ArticleStatisticsFinanceAccounting"]);
			ArticleStatisticsFinanceAccountingEdit = Convert.ToBoolean(dataRow["ArticleStatisticsFinanceAccountingEdit"]);
			ArticleStatisticsLogistics = Convert.ToBoolean(dataRow["ArticleStatisticsLogistics"]);
			ArticleStatisticsLogisticsEdit = Convert.ToBoolean(dataRow["ArticleStatisticsLogisticsEdit"]);
			ArticleStatisticsPurchase = Convert.ToBoolean(dataRow["ArticleStatisticsPurchase"]);
			ArticleStatisticsPurchaseEdit = Convert.ToBoolean(dataRow["ArticleStatisticsPurchaseEdit"]);
			ArticleStatisticsTechnic = Convert.ToBoolean(dataRow["ArticleStatisticsTechnic"]);
			ArticleStatisticsTechnicEdit = Convert.ToBoolean(dataRow["ArticleStatisticsTechnicEdit"]);
			BomChangeRequests = (dataRow["BomChangeRequests"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["BomChangeRequests"]);
			CocType = (dataRow["CocType"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CocType"]);
			ConditionAssignment = (dataRow["ConditionAssignment"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConditionAssignment"]);
			ConfigArticle = Convert.ToBoolean(dataRow["ConfigArticle"]);
			ConfigCustomer = (dataRow["ConfigCustomer"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfigCustomer"]);
			ConfigSupplier = (dataRow["ConfigSupplier"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ConfigSupplier"]);
			ContactAddress = (dataRow["ContactAddress"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ContactAddress"]);
			ContactSalutation = (dataRow["ContactSalutation"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ContactSalutation"]);
			CreateArticlePurchase = Convert.ToBoolean(dataRow["CreateArticlePurchase"]);
			CreateArticleSalesCustom = Convert.ToBoolean(dataRow["CreateArticleSalesCustom"]);
			CreateArticleSalesItem = Convert.ToBoolean(dataRow["CreateArticleSalesItem"]);
			CreateCustomerContactPerson = (dataRow["CreateCustomerContactPerson"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CreateCustomerContactPerson"]);
			CreateSupplierContactPerson = (dataRow["CreateSupplierContactPerson"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CreateSupplierContactPerson"]);
			CreationTime = (dataRow["CreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = Convert.ToInt32(dataRow["CreationUserId"]);
			Currencies = (dataRow["Currencies"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Currencies"]);
			CustomerAddress = (dataRow["CustomerAddress"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CustomerAddress"]);
			CustomerCommunication = (dataRow["CustomerCommunication"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CustomerCommunication"]);
			CustomerContactPerson = (dataRow["CustomerContactPerson"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CustomerContactPerson"]);
			CustomerData = (dataRow["CustomerData"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CustomerData"]);
			CustomerGroup = (dataRow["CustomerGroup"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CustomerGroup"]);
			CustomerHistory = (dataRow["CustomerHistory"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CustomerHistory"]);
			CustomerItemNumber = (dataRow["CustomerItemNumber"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CustomerItemNumber"]);
			CustomerOverview = (dataRow["CustomerOverview"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CustomerOverview"]);
			Customers = Convert.ToBoolean(dataRow["Customers"]);
			CustomerShipping = (dataRow["CustomerShipping"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CustomerShipping"]);
			DeleteAltPositionsArticleBOM = Convert.ToBoolean(dataRow["DeleteAltPositionsArticleBOM"]);
			DeleteArticle = (dataRow["DeleteArticle"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DeleteArticle"]);
			DeleteArticleBOM = Convert.ToBoolean(dataRow["DeleteArticleBOM"]);
			DeleteArticlePurchase = Convert.ToBoolean(dataRow["DeleteArticlePurchase"]);
			DeleteArticleSalesCustom = Convert.ToBoolean(dataRow["DeleteArticleSalesCustom"]);
			DeleteArticleSalesItem = Convert.ToBoolean(dataRow["DeleteArticleSalesItem"]);
			DeleteBCR = (dataRow["DeleteBCR"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DeleteBCR"]);
			DeleteCustomerContactPerson = (dataRow["DeleteCustomerContactPerson"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DeleteCustomerContactPerson"]);
			DeleteFiles = (dataRow["DeleteFiles"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DeleteFiles"]);
			DeleteRohArtikelNummer = (dataRow["DeleteRohArtikelNummer"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DeleteRohArtikelNummer"]);
			DeleteSupplierContactPerson = (dataRow["DeleteSupplierContactPerson"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DeleteSupplierContactPerson"]);
			DiscountGroup = (dataRow["DiscountGroup"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DiscountGroup"]);
			DownloadAllOutdatedEinkaufsPreis = (dataRow["DownloadAllOutdatedEinkaufsPreis"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DownloadAllOutdatedEinkaufsPreis"]);
			DownloadFiles = (dataRow["DownloadFiles"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DownloadFiles"]);
			DownloadOutdatedEinkaufsPreis = (dataRow["DownloadOutdatedEinkaufsPreis"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["DownloadOutdatedEinkaufsPreis"]);
			EdiConcern = (dataRow["EdiConcern"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EdiConcern"]);
			EditAltPositionsArticleBOM = Convert.ToBoolean(dataRow["EditAltPositionsArticleBOM"]);
			EditArticle = Convert.ToBoolean(dataRow["EditArticle"]);
			EditArticleBOM = Convert.ToBoolean(dataRow["EditArticleBOM"]);
			EditArticleBOMPosition = Convert.ToBoolean(dataRow["EditArticleBOMPosition"]);
			EditArticleCts = Convert.ToBoolean(dataRow["EditArticleCts"]);
			EditArticleData = Convert.ToBoolean(dataRow["EditArticleData"]);
			EditArticleDesignation = Convert.ToBoolean(dataRow["EditArticleDesignation"]);
			EditArticleImage = Convert.ToBoolean(dataRow["EditArticleImage"]);
			EditArticleLogistics = Convert.ToBoolean(dataRow["EditArticleLogistics"]);
			EditArticleManager = Convert.ToBoolean(dataRow["EditArticleManager"]);
			EditArticleProduction = Convert.ToBoolean(dataRow["EditArticleProduction"]);
			EditArticlePurchase = Convert.ToBoolean(dataRow["EditArticlePurchase"]);
			EditArticleQuality = Convert.ToBoolean(dataRow["EditArticleQuality"]);
			EditArticleReference = (dataRow["EditArticleReference"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditArticleReference"]);
			EditArticleSalesCustom = Convert.ToBoolean(dataRow["EditArticleSalesCustom"]);
			EditArticleSalesItem = Convert.ToBoolean(dataRow["EditArticleSalesItem"]);
			EditCocType = (dataRow["EditCocType"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditCocType"]);
			EditConditionAssignment = (dataRow["EditConditionAssignment"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditConditionAssignment"]);
			EditContactAddress = (dataRow["EditContactAddress"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditContactAddress"]);
			EditContactSalutation = (dataRow["EditContactSalutation"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditContactSalutation"]);
			EditCurrencies = (dataRow["EditCurrencies"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditCurrencies"]);
			EditCustomer = (dataRow["EditCustomer"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditCustomer"]);
			EditCustomerAddress = (dataRow["EditCustomerAddress"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditCustomerAddress"]);
			EditCustomerCommunication = (dataRow["EditCustomerCommunication"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditCustomerCommunication"]);
			EditCustomerContactPerson = (dataRow["EditCustomerContactPerson"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditCustomerContactPerson"]);
			EditCustomerCoordination = (dataRow["EditCustomerCoordination"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditCustomerCoordination"]);
			EditCustomerData = (dataRow["EditCustomerData"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditCustomerData"]);
			EditCustomerGroup = (dataRow["EditCustomerGroup"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditCustomerGroup"]);
			EditCustomerImage = (dataRow["EditCustomerImage"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditCustomerImage"]);
			EditCustomerItemNumber = (dataRow["EditCustomerItemNumber"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditCustomerItemNumber"]);
			EditCustomerShipping = (dataRow["EditCustomerShipping"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditCustomerShipping"]);
			EditDiscountGroup = (dataRow["EditDiscountGroup"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditDiscountGroup"]);
			EditEdiConcern = (dataRow["EditEdiConcern"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditEdiConcern"]);
			EditFibuFrame = (dataRow["EditFibuFrame"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditFibuFrame"]);
			EditHourlyRate = (dataRow["EditHourlyRate"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditHourlyRate"]);
			EditIndustry = (dataRow["EditIndustry"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditIndustry"]);
			EditLagerCCID = (dataRow["EditLagerCCID"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditLagerCCID"]);
			EditLagerMinStock = Convert.ToBoolean(dataRow["EditLagerMinStock"]);
			EditLagerOrderProposal = (dataRow["EditLagerOrderProposal"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditLagerOrderProposal"]);
			EditLagerStock = (dataRow["EditLagerStock"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditLagerStock"]);
			EditPayementPractises = (dataRow["EditPayementPractises"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditPayementPractises"]);
			EditPricingGroup = (dataRow["EditPricingGroup"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditPricingGroup"]);
			EditRohArtikelNummer = (dataRow["EditRohArtikelNummer"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditRohArtikelNummer"]);
			EditShippingMethods = (dataRow["EditShippingMethods"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditShippingMethods"]);
			EditSlipCircle = (dataRow["EditSlipCircle"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditSlipCircle"]);
			EditSupplier = (dataRow["EditSupplier"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditSupplier"]);
			EditSupplierAddress = (dataRow["EditSupplierAddress"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditSupplierAddress"]);
			EditSupplierCommunication = (dataRow["EditSupplierCommunication"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditSupplierCommunication"]);
			EditSupplierContactPerson = (dataRow["EditSupplierContactPerson"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditSupplierContactPerson"]);
			EditSupplierCoordination = (dataRow["EditSupplierCoordination"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditSupplierCoordination"]);
			EditSupplierData = (dataRow["EditSupplierData"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditSupplierData"]);
			EditSupplierGroup = (dataRow["EditSupplierGroup"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditSupplierGroup"]);
			EditSupplierImage = (dataRow["EditSupplierImage"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditSupplierImage"]);
			EditSupplierShipping = (dataRow["EditSupplierShipping"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditSupplierShipping"]);
			EditTermsOfPayment = (dataRow["EditTermsOfPayment"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EditTermsOfPayment"]);
			EDrawingEdit = (dataRow["EDrawingEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EDrawingEdit"]);
			EinkaufsPreisUpdate = (dataRow["EinkaufsPreisUpdate"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EinkaufsPreisUpdate"]);
			FibuFrame = (dataRow["FibuFrame"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FibuFrame"]);
			GetRohArtikelNummer = (dataRow["GetRohArtikelNummer"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["GetRohArtikelNummer"]);
			HourlyRate = (dataRow["HourlyRate"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["HourlyRate"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			ImportArticleBOM = Convert.ToBoolean(dataRow["ImportArticleBOM"]);
			Industry = (dataRow["Industry"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Industry"]);
			isDefault = (dataRow["isDefault"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["isDefault"]);
			LagerArticleLogistics = Convert.ToBoolean(dataRow["LagerArticleLogistics"]);
			ModuleActivated = Convert.ToBoolean(dataRow["ModuleActivated"]);
			ModuleAdministrator = (dataRow["ModuleAdministrator"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ModuleAdministrator"]);
			offer = (dataRow["offer"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["offer"]);
			OfferRequestADD = (dataRow["OfferRequestADD"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["OfferRequestADD"]);
			OfferRequestApplyPrice = (dataRow["OfferRequestApplyPrice"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["OfferRequestApplyPrice"]);
			OfferRequestDelete = (dataRow["OfferRequestDelete"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["OfferRequestDelete"]);
			OfferRequestEdit = (dataRow["OfferRequestEdit"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["OfferRequestEdit"]);
			OfferRequestEditEmail = (dataRow["OfferRequestEditEmail"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["OfferRequestEditEmail"]);
			OfferRequestSendEmail = (dataRow["OfferRequestSendEmail"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["OfferRequestSendEmail"]);
			OfferRequestView = (dataRow["OfferRequestView"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["OfferRequestView"]);
			PayementPractises = (dataRow["PayementPractises"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PayementPractises"]);
			PMAddCable = (dataRow["PMAddCable"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PMAddCable"]);
			PMAddMileStone = (dataRow["PMAddMileStone"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PMAddMileStone"]);
			PMAddProject = (dataRow["PMAddProject"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PMAddProject"]);
			PMDeleteCable = (dataRow["PMDeleteCable"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PMDeleteCable"]);
			PMDeleteMileStone = (dataRow["PMDeleteMileStone"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PMDeleteMileStone"]);
			PMDeleteProject = (dataRow["PMDeleteProject"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PMDeleteProject"]);
			PMEditCable = (dataRow["PMEditCable"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PMEditCable"]);
			PMEditMileStone = (dataRow["PMEditMileStone"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PMEditMileStone"]);
			PMEditProject = (dataRow["PMEditProject"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PMEditProject"]);
			PMModule = (dataRow["PMModule"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PMModule"]);
			PMViewProjectsCompact = (dataRow["PMViewProjectsCompact"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PMViewProjectsCompact"]);
			PMViewProjectsDetail = (dataRow["PMViewProjectsDetail"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PMViewProjectsDetail"]);
			PMViewProjectsMedium = (dataRow["PMViewProjectsMedium"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PMViewProjectsMedium"]);
			PricingGroup = (dataRow["PricingGroup"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PricingGroup"]);
			RemoveArticleReference = (dataRow["RemoveArticleReference"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RemoveArticleReference"]);
			Settings = (dataRow["Settings"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Settings"]);
			ShippingMethods = (dataRow["ShippingMethods"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ShippingMethods"]);
			SlipCircle = (dataRow["SlipCircle"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SlipCircle"]);
			SupplierAddress = (dataRow["SupplierAddress"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SupplierAddress"]);
			SupplierCommunication = (dataRow["SupplierCommunication"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SupplierCommunication"]);
			SupplierContactPerson = (dataRow["SupplierContactPerson"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SupplierContactPerson"]);
			SupplierData = (dataRow["SupplierData"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SupplierData"]);
			SupplierGroup = (dataRow["SupplierGroup"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SupplierGroup"]);
			SupplierHistory = (dataRow["SupplierHistory"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SupplierHistory"]);
			SupplierOverview = (dataRow["SupplierOverview"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SupplierOverview"]);
			Suppliers = Convert.ToBoolean(dataRow["Suppliers"]);
			SupplierShipping = (dataRow["SupplierShipping"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SupplierShipping"]);
			TermsOfPayment = (dataRow["TermsOfPayment"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["TermsOfPayment"]);
			UploadAltPositionsArticleBOM = Convert.ToBoolean(dataRow["UploadAltPositionsArticleBOM"]);
			UploadArticleBOM = Convert.ToBoolean(dataRow["UploadArticleBOM"]);
			ValidateArticleBOM = (dataRow["ValidateArticleBOM"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ValidateArticleBOM"]);
			ValidateBCR = (dataRow["ValidateBCR"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ValidateBCR"]);
			ViewAltPositionsArticleBOM = Convert.ToBoolean(dataRow["ViewAltPositionsArticleBOM"]);
			ViewArticleLog = Convert.ToBoolean(dataRow["ViewArticleLog"]);
			ViewArticleReference = (dataRow["ViewArticleReference"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ViewArticleReference"]);
			ViewArticles = Convert.ToBoolean(dataRow["ViewArticles"]);
			ViewBCR = (dataRow["ViewBCR"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ViewBCR"]);
			ViewCustomers = (dataRow["ViewCustomers"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ViewCustomers"]);
			ViewLPCustomer = (dataRow["ViewLPCustomer"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ViewLPCustomer"]);
			ViewLPSupplier = (dataRow["ViewLPSupplier"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ViewLPSupplier"]);
			ViewSupplierAddressComments = (dataRow["ViewSupplierAddressComments"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ViewSupplierAddressComments"]);
			ViewSuppliers = (dataRow["ViewSuppliers"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ViewSuppliers"]);
			SupplierAttachementRemoveFile = (dataRow["SupplierAttachementRemoveFile"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SupplierAttachementRemoveFile"]);
			SupplierAttachementAddFile = (dataRow["SupplierAttachementAddFile"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SupplierAttachementAddFile"]);
			SupplierAttachementGetFile = (dataRow["SupplierAttachementGetFile"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SupplierAttachementGetFile"]);
			PackagingsLgtPhotoAdd = (dataRow["PackagingsLgtPhotoAdd"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PackagingsLgtPhotoAdd"]);
			PackagingsLgtPhotoDelete = (dataRow["PackagingsLgtPhotoDelete"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PackagingsLgtPhotoDelete"]);
			PackagingsLgtPhotoView = (dataRow["PackagingsLgtPhotoView"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PackagingsLgtPhotoView"]);
		}

		public AccessProfileEntity ShallowClone()
		{
			return new AccessProfileEntity
			{
				AccessProfileName = AccessProfileName,
				AddArticle = AddArticle,
				AddArticleReference = AddArticleReference,
				AddBCR = AddBCR,
				AddCocType = AddCocType,
				AddConditionAssignment = AddConditionAssignment,
				AddContactAddress = AddContactAddress,
				AddContactSalutation = AddContactSalutation,
				AddCurrencies = AddCurrencies,
				AddCustomer = AddCustomer,
				AddCustomerGroup = AddCustomerGroup,
				AddCustomerItemNumber = AddCustomerItemNumber,
				AddDiscountGroup = AddDiscountGroup,
				AddEdiConcern = AddEdiConcern,
				AddFibuFrame = AddFibuFrame,
				AddFiles = AddFiles,
				AddHourlyRate = AddHourlyRate,
				AddIndustry = AddIndustry,
				AddPayementPractises = AddPayementPractises,
				AddPricingGroup = AddPricingGroup,
				AddRohArtikelNummer = AddRohArtikelNummer,
				AddShippingMethods = AddShippingMethods,
				AddSlipCircle = AddSlipCircle,
				AddSupplier = AddSupplier,
				AddSupplierGroup = AddSupplierGroup,
				AddTermsOfPayment = AddTermsOfPayment,
				Administration = Administration,
				AltPositionsArticleBOM = AltPositionsArticleBOM,
				ArchiveArticle = ArchiveArticle,
				ArchiveCustomer = ArchiveCustomer,
				ArchiveSupplier = ArchiveSupplier,
				ArticleAddCustomerDocument = ArticleAddCustomerDocument,
				ArticleBOM = ArticleBOM,
				ArticleCts = ArticleCts,
				ArticleData = ArticleData,
				ArticleDeleteCustomerDocument = ArticleDeleteCustomerDocument,
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
				ArticlesBOMCPControlEngineering = ArticlesBOMCPControlEngineering,
				ArticlesBOMCPControlHistory = ArticlesBOMCPControlHistory,
				ArticlesBOMCPControlQuality = ArticlesBOMCPControlQuality,
				ArticleStatistics = ArticleStatistics,
				ArticleStatisticsEngineering = ArticleStatisticsEngineering,
				ArticleStatisticsEngineeringEdit = ArticleStatisticsEngineeringEdit,
				ArticleStatisticsFinanceAccounting = ArticleStatisticsFinanceAccounting,
				ArticleStatisticsFinanceAccountingEdit = ArticleStatisticsFinanceAccountingEdit,
				ArticleStatisticsLogistics = ArticleStatisticsLogistics,
				ArticleStatisticsLogisticsEdit = ArticleStatisticsLogisticsEdit,
				ArticleStatisticsPurchase = ArticleStatisticsPurchase,
				ArticleStatisticsPurchaseEdit = ArticleStatisticsPurchaseEdit,
				ArticleStatisticsTechnic = ArticleStatisticsTechnic,
				ArticleStatisticsTechnicEdit = ArticleStatisticsTechnicEdit,
				BomChangeRequests = BomChangeRequests,
				CocType = CocType,
				ConditionAssignment = ConditionAssignment,
				ConfigArticle = ConfigArticle,
				ConfigCustomer = ConfigCustomer,
				ConfigSupplier = ConfigSupplier,
				ContactAddress = ContactAddress,
				ContactSalutation = ContactSalutation,
				CreateArticlePurchase = CreateArticlePurchase,
				CreateArticleSalesCustom = CreateArticleSalesCustom,
				CreateArticleSalesItem = CreateArticleSalesItem,
				CreateCustomerContactPerson = CreateCustomerContactPerson,
				CreateSupplierContactPerson = CreateSupplierContactPerson,
				CreationTime = CreationTime,
				CreationUserId = CreationUserId,
				Currencies = Currencies,
				CustomerAddress = CustomerAddress,
				CustomerCommunication = CustomerCommunication,
				CustomerContactPerson = CustomerContactPerson,
				CustomerData = CustomerData,
				CustomerGroup = CustomerGroup,
				CustomerHistory = CustomerHistory,
				CustomerItemNumber = CustomerItemNumber,
				CustomerOverview = CustomerOverview,
				Customers = Customers,
				CustomerShipping = CustomerShipping,
				DeleteAltPositionsArticleBOM = DeleteAltPositionsArticleBOM,
				DeleteArticle = DeleteArticle,
				DeleteArticleBOM = DeleteArticleBOM,
				DeleteArticlePurchase = DeleteArticlePurchase,
				DeleteArticleSalesCustom = DeleteArticleSalesCustom,
				DeleteArticleSalesItem = DeleteArticleSalesItem,
				DeleteBCR = DeleteBCR,
				DeleteCustomerContactPerson = DeleteCustomerContactPerson,
				DeleteFiles = DeleteFiles,
				DeleteRohArtikelNummer = DeleteRohArtikelNummer,
				DeleteSupplierContactPerson = DeleteSupplierContactPerson,
				DiscountGroup = DiscountGroup,
				DownloadAllOutdatedEinkaufsPreis = DownloadAllOutdatedEinkaufsPreis,
				DownloadFiles = DownloadFiles,
				DownloadOutdatedEinkaufsPreis = DownloadOutdatedEinkaufsPreis,
				EdiConcern = EdiConcern,
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
				EditArticleReference = EditArticleReference,
				EditArticleSalesCustom = EditArticleSalesCustom,
				EditArticleSalesItem = EditArticleSalesItem,
				EditCocType = EditCocType,
				EditConditionAssignment = EditConditionAssignment,
				EditContactAddress = EditContactAddress,
				EditContactSalutation = EditContactSalutation,
				EditCurrencies = EditCurrencies,
				EditCustomer = EditCustomer,
				EditCustomerAddress = EditCustomerAddress,
				EditCustomerCommunication = EditCustomerCommunication,
				EditCustomerContactPerson = EditCustomerContactPerson,
				EditCustomerCoordination = EditCustomerCoordination,
				EditCustomerData = EditCustomerData,
				EditCustomerGroup = EditCustomerGroup,
				EditCustomerImage = EditCustomerImage,
				EditCustomerItemNumber = EditCustomerItemNumber,
				EditCustomerShipping = EditCustomerShipping,
				EditDiscountGroup = EditDiscountGroup,
				EditEdiConcern = EditEdiConcern,
				EditFibuFrame = EditFibuFrame,
				EditHourlyRate = EditHourlyRate,
				EditIndustry = EditIndustry,
				EditLagerCCID = EditLagerCCID,
				EditLagerMinStock = EditLagerMinStock,
				EditLagerOrderProposal = EditLagerOrderProposal,
				EditLagerStock = EditLagerStock,
				EditPayementPractises = EditPayementPractises,
				EditPricingGroup = EditPricingGroup,
				EditRohArtikelNummer = EditRohArtikelNummer,
				EditShippingMethods = EditShippingMethods,
				EditSlipCircle = EditSlipCircle,
				EditSupplier = EditSupplier,
				EditSupplierAddress = EditSupplierAddress,
				EditSupplierCommunication = EditSupplierCommunication,
				EditSupplierContactPerson = EditSupplierContactPerson,
				EditSupplierCoordination = EditSupplierCoordination,
				EditSupplierData = EditSupplierData,
				EditSupplierGroup = EditSupplierGroup,
				EditSupplierImage = EditSupplierImage,
				EditSupplierShipping = EditSupplierShipping,
				EditTermsOfPayment = EditTermsOfPayment,
				EDrawingEdit = EDrawingEdit,
				EinkaufsPreisUpdate = EinkaufsPreisUpdate,
				FibuFrame = FibuFrame,
				GetRohArtikelNummer = GetRohArtikelNummer,
				HourlyRate = HourlyRate,
				Id = Id,
				ImportArticleBOM = ImportArticleBOM,
				Industry = Industry,
				isDefault = isDefault,
				LagerArticleLogistics = LagerArticleLogistics,
				ModuleActivated = ModuleActivated,
				ModuleAdministrator = ModuleAdministrator,
				offer = offer,
				OfferRequestADD = OfferRequestADD,
				OfferRequestApplyPrice = OfferRequestApplyPrice,
				OfferRequestDelete = OfferRequestDelete,
				OfferRequestEdit = OfferRequestEdit,
				OfferRequestEditEmail = OfferRequestEditEmail,
				OfferRequestSendEmail = OfferRequestSendEmail,
				OfferRequestView = OfferRequestView,
				PayementPractises = PayementPractises,
				PMAddCable = PMAddCable,
				PMAddMileStone = PMAddMileStone,
				PMAddProject = PMAddProject,
				PMDeleteCable = PMDeleteCable,
				PMDeleteMileStone = PMDeleteMileStone,
				PMDeleteProject = PMDeleteProject,
				PMEditCable = PMEditCable,
				PMEditMileStone = PMEditMileStone,
				PMEditProject = PMEditProject,
				PMModule = PMModule,
				PMViewProjectsCompact = PMViewProjectsCompact,
				PMViewProjectsDetail = PMViewProjectsDetail,
				PMViewProjectsMedium = PMViewProjectsMedium,
				PricingGroup = PricingGroup,
				RemoveArticleReference = RemoveArticleReference,
				Settings = Settings,
				ShippingMethods = ShippingMethods,
				SlipCircle = SlipCircle,
				SupplierAddress = SupplierAddress,
				SupplierCommunication = SupplierCommunication,
				SupplierContactPerson = SupplierContactPerson,
				SupplierData = SupplierData,
				SupplierGroup = SupplierGroup,
				SupplierHistory = SupplierHistory,
				SupplierOverview = SupplierOverview,
				Suppliers = Suppliers,
				SupplierShipping = SupplierShipping,
				TermsOfPayment = TermsOfPayment,
				UploadAltPositionsArticleBOM = UploadAltPositionsArticleBOM,
				UploadArticleBOM = UploadArticleBOM,
				ValidateArticleBOM = ValidateArticleBOM,
				ValidateBCR = ValidateBCR,
				ViewAltPositionsArticleBOM = ViewAltPositionsArticleBOM,
				ViewArticleLog = ViewArticleLog,
				ViewArticleReference = ViewArticleReference,
				ViewArticles = ViewArticles,
				ViewBCR = ViewBCR,
				ViewCustomers = ViewCustomers,
				ViewLPCustomer = ViewLPCustomer,
				ViewLPSupplier = ViewLPSupplier,
				ViewSupplierAddressComments = ViewSupplierAddressComments,
				ViewSuppliers = ViewSuppliers,
				SupplierAttachementRemoveFile = SupplierAttachementRemoveFile,
				SupplierAttachementAddFile = SupplierAttachementAddFile,
				SupplierAttachementGetFile = SupplierAttachementGetFile,
				PackagingsLgtPhotoAdd= PackagingsLgtPhotoAdd,
				PackagingsLgtPhotoDelete= PackagingsLgtPhotoDelete,
				PackagingsLgtPhotoView= PackagingsLgtPhotoView
			};
		}
	}
}

