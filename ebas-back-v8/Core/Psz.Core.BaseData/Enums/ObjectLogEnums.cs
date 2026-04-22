using System.ComponentModel;

namespace Psz.Core.BaseData.Enums
{
	public static class ObjectLogEnums
	{
		public enum Objects
		{
			[Description("Article")]
			Article = 1,
			[Description("Article Class")]
			ArticleConfig_Class = 2,
			[Description("Project Type")]
			ArticleConfig_ProjectType = 3,
			[Description("Article Packaging")]
			ArticleConfig_Packaging = 4,
			[Description("Product Group")]
			ArticleConfig_ProductGroup = 5,
			[Description("Manager Users")]
			ArticleConfig_ManagerUsers = 6,
			[Description("External status")]
			ArticleConfig_ExternalStatus = 7,
			[Description("Check status")]
			ArticleConfig_CheckStatus = 8,
			[Description("Internal status")]
			ArticleConfig_InternalStatus = 9,
			[Description("MHD Tag")]
			ArticleConfig_MHDTag = 10,
			[Description("BOM Position")]
			ArticleBOM_Position = 21,
			[Description("BOM Position ALT")]
			ArticleBOM_PositionALT = 22,
			//
			[Description("Customer_Overview")]
			Customer_Overview = 23,
			[Description("Customer_Adress")]
			Customer_Adress = 24,
			[Description("Customer_Data")]
			Customer_Data = 25,
			[Description("Customer_Communication")]
			Customer_Communication = 26,
			[Description("Customer_Shipping")]
			Customer_Shipping = 27,
			[Description("Customer_ContcatPerson")]
			Customer_ContcatPerson = 28,
			//
			[Description("Supplier_Overview")]
			Supplier_Overview = 29,
			[Description("Supplier_Adress")]
			Supplier_Adress = 30,
			[Description("Supplier_Data")]
			Supplier_Data = 31,
			[Description("Supplier_Communication")]
			Supplier_Communication = 32,
			[Description("Supplier_Shipping")]
			Supplier_Shipping = 33,
			[Description("Supplier_ContcatPerson")]
			Supplier_ContcatPerson = 34,
			[Description("Discount_groups")]
			//Settings
			Discount_groups = 35,
			[Description("Condition_assignement")]
			Condition_assignement = 36,
			[Description("Industry")]
			Industry = 37,
			[Description("Supplier_group")]
			Supplier_group = 38,
			[Description("Payement_practice")]
			Payement_practice = 39,
			[Description("Currency")]
			Currency = 40,
			[Description("Slip_circle")]
			Slip_circle = 41,
			[Description("Pricing_group")]
			Pricing_group = 42,
			[Description("Adress_types")]
			Adress_types = 43,
			[Description("Customer_frames")]
			Customer_frames = 44,
			[Description("Shipping_methods")]
			Shipping_methods = 45,
			[Description("Terms_of_payement")]
			Terms_of_payement = 46,
			[Description("Customer_group")]
			Customer_group = 47,
			[Description("Salutation_contact_person")]
			Salutation_contact_person = 48,
			[Description("Address_contact_person")]
			Address_contact_person = 48,
			[Description("Customer")]
			Customer = 49,
			//
			[Description("Supplier_SupplierNumber")]
			Supplier_SupplierNumber = 50,
			[Description("Customer_CustomerNumber")]
			Customer_CustomerNumber = 51,
			//
			[Description("Supplier")]
			Supplier = 52,
			//
			[Description("Technician")]
			Technician = 53,
			[Description("ProjectData")]
			ProjectData = 54,
			[Description("SuperbillROH")]
			SuperbillROH = 55,
			//
			[Description("Angebote")]
			Angebote = 56,
			[Description("Angebotene Article")]
			AngeboteneArticle = 57,
			[Description("Project Class")]
			ArticleConfig_ProjectClass = 58,
			[Description("Article Contact AV")]
			ArticleConfig_ArticleContactAV = 59,
			[Description("Article Contact CS")]
			ArticleConfig_ArticleContactCS = 60,
			[Description("Article Contact Technic")]
			ArticleConfig_ArticleContactTechnic = 61,
			[Description("Article Sample")]
			ArticleConfig_ArticleSample = 62,
			[Description("Article Standort Master")]
			ArticleConfig_ArticleStandortMaster = 63,
			[Description("Article Standort Serie")]
			ArticleConfig_ArticleStandortSerie = 64,
			[Description("Article Project Meldung Price")]
			ArticleConfig_ArticleProjectmsgPrice = 65,
			[Description("Fertigung")]
			ArticleFertigung = 66,
			[Description("Customer Item Number")]
			ArticleConfig_CustomerItemNumber = 67,
			[Description("Article Employee AV")]
			ArticleConfig_ArticleEmployeeAV = 68,
			[Description("Article Teams")]
			ArticleConfig_ArticleTeams = 69,
			[Description("Article CoC Types")]
			ArticleConfig_CoCTypes = 70,
			[Description("EDI Concerns")]
			ArticleConfig_EdiConcerns = 71,
			[Description("EDI Concerns customer")]
			ArticleConfig_EdiConcernCustomers = 72,
			[Description("Unit of measure")]
			ArticleConfig_UnitOfMeasure = 73,
			[Description("Hourly Rate")]
			ArticleConfig_HourlyRates = 74,
			[Description("Article Customer_References")]
			Article_CustomerReference = 75,
			[Description("Article ROH Number")]
			ArticleRohNumber = 76,

		}

		public enum LogType
		{
			[Description("Add")]
			Add = 1,
			[Description("Update")]
			Edit = 2,
			[Description("Delete")]
			Delete = 3,
			[Description("Archive")]
			Archive = 4,
			[Description("BulkUpdate")]
			BulkUpdate = 5,
			[Description("AddFromCopy")]
			AddFromCopy = 6,
		}

		public enum BOMLogType
		{
			[Description("Add")]
			Add = 1,
			[Description("Article Update")]
			EditArt = 2,
			[Description("Qty Update")]
			EditQty = 3,
			[Description("Delete")]
			Delete = 4,
			[Description("StatusChange")]
			StatusChange = 5,
			[Description("ImportExcel")]
			ImportExcel = 6,
			[Description("Overwrite")]
			Overwrite = 7,
			[Description("Version")]
			Version = 8,
			[Description("CPRequired")]
			CPRequired = 9,
			[Description("ValidFrom")]
			ValidFrom = 10,
			[Description("E-Drawing")]
			EDrawing = 11
		}

		public enum BOMCRUdState
		{
			[Description("Added")]
			Added = 0,
			[Description("Edited")]
			Edited = 1,
			[Description("Deleted")]
			Deleted = 2
		}
		public enum FertigungLog
		{
			[Description("Upgrade From BOM Validation")]
			UpgradeFromBOM = 0
		}
	}
}
