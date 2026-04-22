using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class BestellungenExtensionEntity
	{
		public int AllocationType { get; set; }
		public string AllocationTypeName { get; set; }
		public DateTime? ApprovalTime { get; set; }
		public int? ApprovalUserId { get; set; }
		public bool? Archived { get; set; }
		public DateTime? ArchiveTime { get; set; }
		public int? ArchiveUserId { get; set; }
		public string BillingAddress { get; set; }
		public int? BillingCompanyId { get; set; }
		public string BillingCompanyName { get; set; }
		public string BillingContactName { get; set; }
		public int? BillingDepartmentId { get; set; }
		public string BillingDepartmentName { get; set; }
		public string BillingFax { get; set; }
		public string BillingTelephone { get; set; }
		public int BudgetYear { get; set; }
		public int? CompanyId { get; set; }
		public string CompanyName { get; set; }
		public DateTime? CreationDate { get; set; }
		public int? CurrencyId { get; set; }
		public string CurrencyName { get; set; }
		public int? DefaultCurrencyDecimals { get; set; }
		public int? DefaultCurrencyId { get; set; }
		public string DefaultCurrencyName { get; set; }
		public decimal? DefaultCurrencyRate { get; set; }
		public bool? Deleted { get; set; }
		public DateTime? DeleteTime { get; set; }
		public int? DeleteUserId { get; set; }
		public string DeliveryAddress { get; set; }
		public int? DeliveryCompanyId { get; set; }
		public string DeliveryCompanyName { get; set; }
		public int? DeliveryDepartmentId { get; set; }
		public string DeliveryDepartmentName { get; set; }
		public string DeliveryFax { get; set; }
		public string DeliveryTelephone { get; set; }
		public int? DepartmentId { get; set; }
		public string DepartmentName { get; set; }
		public string Description { get; set; }
		public decimal? Discount { get; set; }
		public int Id { get; set; }
		public string InternalContact { get; set; }
		public int IssuerId { get; set; }
		public string IssuerName { get; set; }
		public int? LastRejectionLevel { get; set; }
		public DateTime? LastRejectionTime { get; set; }
		public int? LastRejectionUserId { get; set; }
		public decimal? LeasingMonthAmount { get; set; }
		public int? LeasingNbMonths { get; set; }
		public int? LeasingStartMonth { get; set; }
		public int? LeasingStartYear { get; set; }
		public decimal? LeasingTotalAmount { get; set; }
		public int? Level { get; set; }
		public int? LocationId { get; set; }
		public int? MandantId { get; set; }
		public int OrderId { get; set; }
		public string OrderNumber { get; set; }
		public string OrderPlacedEmailMessage { get; set; }
		public string OrderPlacedEmailTitle { get; set; }
		public int? OrderPlacedReportFileId { get; set; }
		public string OrderPlacedSendingEmail { get; set; }
		public string OrderPlacedSupplierEmail { get; set; }
		public DateTime? OrderPlacedTime { get; set; }
		public string OrderPlacedUserEmail { get; set; }
		public int? OrderPlacedUserId { get; set; }
		public string OrderPlacedUserName { get; set; }
		public string OrderPlacementCCEmail { get; set; }
		public string OrderType { get; set; }
		public int? PoPaymentType { get; set; }
		public string PoPaymentTypeName { get; set; }
		public int? ProjectId { get; set; }
		public string ProjectName { get; set; }
		public int? Status { get; set; }
		public int? StorageLocationId { get; set; }
		public string StorageLocationName { get; set; }
		public string SupplierEmail { get; set; }
		public string SupplierFax { get; set; }
		public int? SupplierId { get; set; }
		public string SupplierName { get; set; }
		public string SupplierNumber { get; set; }
		public string SupplierNummer { get; set; }
		public string SupplierPaymentMethod { get; set; }
		public string SupplierPaymentTerm { get; set; }
		public string SupplierTelephone { get; set; }
		public string SupplierTradingTerm { get; set; }
		public DateTime? ValidationRequestTime { get; set; }

		public BestellungenExtensionEntity() { }

		public BestellungenExtensionEntity(DataRow dataRow)
		{
			AllocationType = Convert.ToInt32(dataRow["AllocationType"]);
			AllocationTypeName = (dataRow["AllocationTypeName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AllocationTypeName"]);
			ApprovalTime = (dataRow["ApprovalTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ApprovalTime"]);
			ApprovalUserId = (dataRow["ApprovalUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ApprovalUserId"]);
			Archived = (dataRow["Archived"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Archived"]);
			ArchiveTime = (dataRow["ArchiveTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ArchiveTime"]);
			ArchiveUserId = (dataRow["ArchiveUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArchiveUserId"]);
			BillingAddress = (dataRow["BillingAddress"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BillingAddress"]);
			BillingCompanyId = (dataRow["BillingCompanyId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BillingCompanyId"]);
			BillingCompanyName = (dataRow["BillingCompanyName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BillingCompanyName"]);
			BillingContactName = (dataRow["BillingContactName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BillingContactName"]);
			BillingDepartmentId = (dataRow["BillingDepartmentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BillingDepartmentId"]);
			BillingDepartmentName = (dataRow["BillingDepartmentName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BillingDepartmentName"]);
			BillingFax = (dataRow["BillingFax"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BillingFax"]);
			BillingTelephone = (dataRow["BillingTelephone"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BillingTelephone"]);
			BudgetYear = Convert.ToInt32(dataRow["BudgetYear"]);
			CompanyId = (dataRow["CompanyId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CompanyId"]);
			CompanyName = (dataRow["CompanyName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CompanyName"]);
			CreationDate = (dataRow["CreationDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationDate"]);
			CurrencyId = (dataRow["CurrencyId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CurrencyId"]);
			CurrencyName = (dataRow["CurrencyName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CurrencyName"]);
			DefaultCurrencyDecimals = (dataRow["DefaultCurrencyDecimals"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DefaultCurrencyDecimals"]);
			DefaultCurrencyId = (dataRow["DefaultCurrencyId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DefaultCurrencyId"]);
			DefaultCurrencyName = (dataRow["DefaultCurrencyName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DefaultCurrencyName"]);
			DefaultCurrencyRate = (dataRow["DefaultCurrencyRate"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["DefaultCurrencyRate"]);
			Deleted = (dataRow["Deleted"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Deleted"]);
			DeleteTime = (dataRow["DeleteTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["DeleteTime"]);
			DeleteUserId = (dataRow["DeleteUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DeleteUserId"]);
			DeliveryAddress = (dataRow["DeliveryAddress"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DeliveryAddress"]);
			DeliveryCompanyId = (dataRow["DeliveryCompanyId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DeliveryCompanyId"]);
			DeliveryCompanyName = (dataRow["DeliveryCompanyName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DeliveryCompanyName"]);
			DeliveryDepartmentId = (dataRow["DeliveryDepartmentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DeliveryDepartmentId"]);
			DeliveryDepartmentName = (dataRow["DeliveryDepartmentName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DeliveryDepartmentName"]);
			DeliveryFax = (dataRow["DeliveryFax"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DeliveryFax"]);
			DeliveryTelephone = (dataRow["DeliveryTelephone"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DeliveryTelephone"]);
			DepartmentId = (dataRow["DepartmentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DepartmentId"]);
			DepartmentName = (dataRow["DepartmentName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DepartmentName"]);
			Description = (dataRow["Description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
			Discount = (dataRow["Discount"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Discount"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			InternalContact = (dataRow["InternalContact"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["InternalContact"]);
			IssuerId = Convert.ToInt32(dataRow["IssuerId"]);
			IssuerName = (dataRow["IssuerName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["IssuerName"]);
			LastRejectionLevel = (dataRow["LastRejectionLevel"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastRejectionLevel"]);
			LastRejectionTime = (dataRow["LastRejectionTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastRejectionTime"]);
			LastRejectionUserId = (dataRow["LastRejectionUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastRejectionUserId"]);
			LeasingMonthAmount = (dataRow["LeasingMonthAmount"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["LeasingMonthAmount"]);
			LeasingNbMonths = (dataRow["LeasingNbMonths"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LeasingNbMonths"]);
			LeasingStartMonth = (dataRow["LeasingStartMonth"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LeasingStartMonth"]);
			LeasingStartYear = (dataRow["LeasingStartYear"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LeasingStartYear"]);
			LeasingTotalAmount = (dataRow["LeasingTotalAmount"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["LeasingTotalAmount"]);
			Level = (dataRow["Level"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Level"]);
			LocationId = (dataRow["LocationId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LocationId"]);
			MandantId = (dataRow["MandantId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["MandantId"]);
			OrderId = Convert.ToInt32(dataRow["OrderId"]);
			OrderNumber = (dataRow["OrderNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderNumber"]);
			OrderPlacedEmailMessage = (dataRow["OrderPlacedEmailMessage"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderPlacedEmailMessage"]);
			OrderPlacedEmailTitle = (dataRow["OrderPlacedEmailTitle"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderPlacedEmailTitle"]);
			OrderPlacedReportFileId = (dataRow["OrderPlacedReportFileId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OrderPlacedReportFileId"]);
			OrderPlacedSendingEmail = (dataRow["OrderPlacedSendingEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderPlacedSendingEmail"]);
			OrderPlacedSupplierEmail = (dataRow["OrderPlacedSupplierEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderPlacedSupplierEmail"]);
			OrderPlacedTime = (dataRow["OrderPlacedTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["OrderPlacedTime"]);
			OrderPlacedUserEmail = (dataRow["OrderPlacedUserEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderPlacedUserEmail"]);
			OrderPlacedUserId = (dataRow["OrderPlacedUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OrderPlacedUserId"]);
			OrderPlacedUserName = (dataRow["OrderPlacedUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderPlacedUserName"]);
			OrderPlacementCCEmail = (dataRow["OrderPlacementCCEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderPlacementCCEmail"]);
			OrderType = (dataRow["OrderType"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderType"]);
			PoPaymentType = (dataRow["PoPaymentType"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PoPaymentType"]);
			PoPaymentTypeName = (dataRow["PoPaymentTypeName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PoPaymentTypeName"]);
			ProjectId = (dataRow["ProjectId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProjectId"]);
			ProjectName = (dataRow["ProjectName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProjectName"]);
			Status = (dataRow["Status"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Status"]);
			StorageLocationId = (dataRow["StorageLocationId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["StorageLocationId"]);
			StorageLocationName = (dataRow["StorageLocationName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["StorageLocationName"]);
			SupplierEmail = (dataRow["SupplierEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierEmail"]);
			SupplierFax = (dataRow["SupplierFax"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierFax"]);
			SupplierId = (dataRow["SupplierId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SupplierId"]);
			SupplierName = (dataRow["SupplierName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierName"]);
			SupplierNumber = (dataRow["SupplierNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierNumber"]);
			SupplierNummer = (dataRow["SupplierNummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierNummer"]);
			SupplierPaymentMethod = (dataRow["SupplierPaymentMethod"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierPaymentMethod"]);
			SupplierPaymentTerm = (dataRow["SupplierPaymentTerm"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierPaymentTerm"]);
			SupplierTelephone = (dataRow["SupplierTelephone"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierTelephone"]);
			SupplierTradingTerm = (dataRow["SupplierTradingTerm"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierTradingTerm"]);
			ValidationRequestTime = (dataRow["ValidationRequestTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ValidationRequestTime"]);
		}
	}
}

