using System;

namespace Psz.Core.FinanceControl.Models.Budget.Order
{
	public class OrderExtensionModel
	{
		public DateTime? ApprovalTime { get; set; }
		public int? ApprovalUserId { get; set; }
		public bool? Archived { get; set; }
		public DateTime? ArchiveTime { get; set; }
		public int? ArchiveUserId { get; set; }
		public string ComapnyName { get; set; }
		public int? CompanyId { get; set; }
		public DateTime? CreationDate { get; set; }
		public int? CurrencyId { get; set; }
		public string CurrencyName { get; set; }
		public bool? Deleted { get; set; }
		public DateTime? DeleteTime { get; set; }
		public int? DeleteUserId { get; set; }
		public string DeliveryAddress { get; set; }
		public int? DepartmentId { get; set; }
		public string DepartmentName { get; set; }
		public string Description { get; set; }
		public int Id { get; set; }
		public string InternalContact { get; set; }
		public int IssuerId { get; set; }
		public string IssuerName { get; set; }
		public int? LastRejectionLevel { get; set; }
		public DateTime? LastRejectionTime { get; set; }
		public int? LastRejectionUserId { get; set; }
		public int? Level { get; set; }
		public int? LocationId { get; set; }
		public int? MandantId { get; set; }
		public int OrderId { get; set; }
		public string OrderNumber { get; set; }
		public string OrderType { get; set; }
		public int? ProjectId { get; set; }
		public int? Status { get; set; }
		public int? StorageLocationId { get; set; }
		public string StorageLocationName { get; set; }
		public int? SupplierId { get; set; }

		// - 
		public string BillingAddress { get; set; }
		public int? BillingCompanyId { get; set; }
		public string BillingCompanyName { get; set; }
		public string BillingContactName { get; set; }
		public int? BillingDepartmentId { get; set; }
		public string BillingDepartmentName { get; set; }
		public string BillingFax { get; set; }
		public string BillingTelephone { get; set; }
		public int BudgetYear { get; set; }
		public string CompanyName { get; set; }
		public int? DefaultCurrencyDecimals { get; set; }
		public int? DefaultCurrencyId { get; set; }
		public string DefaultCurrencyName { get; set; }
		public decimal? DefaultCurrencyRate { get; set; }
		public int? DeliveryCompanyId { get; set; }
		public string DeliveryCompanyName { get; set; }
		public int? DeliveryDepartmentId { get; set; }
		public string DeliveryDepartmentName { get; set; }
		public string DeliveryFax { get; set; }
		public string DeliveryTelephone { get; set; }
		public string ProjectName { get; set; }
		public string SupplierFax { get; set; }
		public string SupplierName { get; set; }
		public string SupplierNumber { get; set; }
		public string SupplierNummer { get; set; }
		public string SupplierPaymentMethod { get; set; }
		public string SupplierPaymentTerm { get; set; }
		public string SupplierTelephone { get; set; }
		public string SupplierTradingTerm { get; set; }
		public DateTime? ValidationRequestTime { get; set; }

		public OrderExtensionModel(Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity entity)
		{
			if(entity == null)
				return;

			ApprovalTime = entity.ApprovalTime;
			ApprovalUserId = entity.ApprovalUserId;
			Archived = entity.Archived;
			ArchiveTime = entity.ArchiveTime;
			ArchiveUserId = entity.ArchiveUserId;
			ComapnyName = entity.CompanyName;
			CompanyId = entity.CompanyId;
			CreationDate = entity.CreationDate;
			CurrencyId = entity.CurrencyId;
			CurrencyName = entity.CurrencyName;
			Deleted = entity.Deleted;
			DeleteTime = entity.DeleteTime;
			DeleteUserId = entity.DeleteUserId;
			DeliveryAddress = entity.DeliveryAddress;
			DepartmentId = entity.DepartmentId;
			DepartmentName = entity.DepartmentName;
			Description = entity.Description;
			Id = entity.Id;
			InternalContact = entity.InternalContact;
			IssuerId = entity.IssuerId;
			IssuerName = entity.IssuerName;
			LastRejectionLevel = entity.LastRejectionLevel;
			LastRejectionTime = entity.LastRejectionTime;
			LastRejectionUserId = entity.LastRejectionUserId;
			Level = entity.Level;
			LocationId = entity.LocationId;
			MandantId = entity.MandantId;
			OrderId = entity.OrderId;
			OrderNumber = entity.OrderNumber;
			OrderType = entity.OrderType;
			ProjectId = entity.ProjectId;
			Status = entity.Status;
			StorageLocationId = entity.StorageLocationId;
			StorageLocationName = entity.StorageLocationName;
			SupplierId = entity.SupplierId;

			// - 
			BillingAddress = entity.BillingAddress;
			BillingCompanyId = entity.BillingCompanyId;
			BillingCompanyName = entity.BillingCompanyName;
			BillingContactName = entity.BillingContactName;
			BillingDepartmentId = entity.BillingDepartmentId;
			BillingDepartmentName = entity.BillingDepartmentName;
			BillingFax = entity.BillingFax;
			BillingTelephone = entity.BillingTelephone;
			BudgetYear = entity.BudgetYear;
			CompanyName = entity.CompanyName;
			DefaultCurrencyDecimals = entity.DefaultCurrencyDecimals;
			DefaultCurrencyId = entity.DefaultCurrencyId;
			DefaultCurrencyName = entity.DefaultCurrencyName;
			DefaultCurrencyRate = entity.DefaultCurrencyRate;
			DeliveryCompanyId = entity.DeliveryCompanyId;
			DeliveryCompanyName = entity.DeliveryCompanyName;
			DeliveryDepartmentId = entity.DeliveryDepartmentId;
			DeliveryDepartmentName = entity.DeliveryDepartmentName;
			DeliveryFax = entity.DeliveryFax;
			DeliveryTelephone = entity.DeliveryTelephone;
			ProjectName = entity.ProjectName;
			SupplierFax = entity.SupplierFax;
			SupplierName = entity.SupplierName;
			SupplierNumber = entity.SupplierNumber;
			SupplierNummer = entity.SupplierNummer;
			SupplierPaymentMethod = entity.SupplierPaymentMethod;
			SupplierPaymentTerm = entity.SupplierPaymentTerm;
			SupplierTelephone = entity.SupplierTelephone;
			SupplierTradingTerm = entity.SupplierTradingTerm;
			ValidationRequestTime = entity.ValidationRequestTime;
		}

		public Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity
			{
				ApprovalTime = ApprovalTime,
				ApprovalUserId = ApprovalUserId,
				Archived = Archived,
				ArchiveTime = ArchiveTime,
				ArchiveUserId = ArchiveUserId,
				CompanyName = ComapnyName,
				CompanyId = CompanyId,
				CreationDate = CreationDate,
				CurrencyId = CurrencyId,
				CurrencyName = CurrencyName,
				Deleted = Deleted,
				DeleteTime = DeleteTime,
				DeleteUserId = DeleteUserId,
				DeliveryAddress = DeliveryAddress,
				DepartmentId = DepartmentId,
				DepartmentName = DepartmentName,
				Description = Description,
				Id = Id,
				InternalContact = InternalContact,
				IssuerId = IssuerId,
				IssuerName = IssuerName,
				LastRejectionLevel = LastRejectionLevel,
				LastRejectionTime = LastRejectionTime,
				LastRejectionUserId = LastRejectionUserId,
				Level = Level,
				LocationId = LocationId,
				MandantId = MandantId,
				OrderId = OrderId,
				OrderNumber = OrderNumber,
				OrderType = OrderType,
				ProjectId = ProjectId,
				Status = Status,
				StorageLocationId = StorageLocationId,
				StorageLocationName = StorageLocationName,
				SupplierId = SupplierId,
				// - 
				BillingAddress = BillingAddress,
				BillingCompanyId = BillingCompanyId,
				BillingCompanyName = BillingCompanyName,
				BillingContactName = BillingContactName,
				BillingDepartmentId = BillingDepartmentId,
				BillingDepartmentName = BillingDepartmentName,
				BillingFax = BillingFax,
				BillingTelephone = BillingTelephone,
				BudgetYear = BudgetYear,
				DefaultCurrencyDecimals = DefaultCurrencyDecimals,
				DefaultCurrencyId = DefaultCurrencyId,
				DefaultCurrencyName = DefaultCurrencyName,
				DefaultCurrencyRate = DefaultCurrencyRate,
				DeliveryCompanyId = DeliveryCompanyId,
				DeliveryCompanyName = DeliveryCompanyName,
				DeliveryDepartmentId = DeliveryDepartmentId,
				DeliveryDepartmentName = DeliveryDepartmentName,
				DeliveryFax = DeliveryFax,
				DeliveryTelephone = DeliveryTelephone,
				ProjectName = ProjectName,
				SupplierFax = SupplierFax,
				SupplierName = SupplierName,
				SupplierNumber = SupplierNumber,
				SupplierNummer = SupplierNummer,
				SupplierPaymentMethod = SupplierPaymentMethod,
				SupplierPaymentTerm = SupplierPaymentTerm,
				SupplierTelephone = SupplierTelephone,
				SupplierTradingTerm = SupplierTradingTerm,
				ValidationRequestTime = ValidationRequestTime
			};
		}
	}
}
