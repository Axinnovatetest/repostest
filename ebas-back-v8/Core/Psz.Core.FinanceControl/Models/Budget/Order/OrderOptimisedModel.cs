using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Models.Budget.Order
{
	public class OrderOptimisedModel
	{
		public int Id_Order { get; set; }
		public string Number { get; set; }
		public string PoPaymentTypeName { get; set; }
		public string ProjectName { get; set; }
		public string ResponsableName { get; set; }
		public string SupplierName { get; set; }
		public string DepartmentName { get; set; }
		public DateTime? OrderDate { get; set; }
		public decimal TotalAmount { get; set; }
		public string CurrencyName { get; set; }
		public int? CurrencyId { get; set; }
		public int? DefaultCurrencyId { get; set; }
		public decimal TotalAmountDefaultCurrency { get; set; }
		public string DefaultCurrencySymbol { get; set; }
		public string Type { get; set; }
		public string CompanyName { get; set; }
		public int? ApprovalUserId { get; set; }
		public int? LastRejectionUserId { get; set; }

		public OrderOptimisedModel(
			Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity orderEntity,
			Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity projectEntity,
		List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity> articles
			)
		{
			if(orderEntity == null)
				throw new ArgumentNullException(nameof(orderEntity));

			this.Id_Order = orderEntity.OrderId;
			this.Number = orderEntity.OrderNumber;
			this.PoPaymentTypeName = orderEntity.PoPaymentTypeName;
			this.ProjectName = orderEntity.ProjectName;
			this.ResponsableName = orderEntity.IssuerName;
			this.SupplierName = orderEntity.SupplierName;
			this.DepartmentName = orderEntity?.DepartmentName;
			this.OrderDate = orderEntity.CreationDate;

			decimal? TotalAmountBeforeDiscount = orderEntity.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase
	? Psz.Core.FinanceControl.Helpers.Processings.Budget.Order.getItemsAmount(articles, false)
	: (orderEntity.LeasingTotalAmount ?? 0);
			// Calculate TotalAmount
			TotalAmount = orderEntity.Discount.HasValue && orderEntity.Discount.Value > 0 ? (1 - orderEntity.Discount.Value / 100) * (TotalAmountBeforeDiscount ?? 0) : (TotalAmountBeforeDiscount ?? 0);
			this.CurrencyName = orderEntity.CurrencyName;
			this.CurrencyId = orderEntity.CurrencyId;
			DefaultCurrencyId = orderEntity.DefaultCurrencyId;
			decimal? TotalAmountDefaultCurrencyBeforeDiscount = orderEntity.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase
				? Psz.Core.FinanceControl.Helpers.Processings.Budget.Order.getItemsAmount(articles, false, true)
				: (orderEntity.LeasingTotalAmount ?? 0);
			TotalAmountDefaultCurrency = orderEntity.Discount.HasValue && orderEntity.Discount.Value > 0 ? (1 - orderEntity.Discount.Value / 100) * (TotalAmountDefaultCurrencyBeforeDiscount ?? 0) : (TotalAmountDefaultCurrencyBeforeDiscount ?? 0);

			DefaultCurrencySymbol = orderEntity.DefaultCurrencyName;
			this.Type = orderEntity.OrderType;
			this.CompanyName = orderEntity.CompanyName;
			this.ApprovalUserId = orderEntity.ApprovalUserId;
			this.LastRejectionUserId = orderEntity.LastRejectionUserId;

		}

	}
}
