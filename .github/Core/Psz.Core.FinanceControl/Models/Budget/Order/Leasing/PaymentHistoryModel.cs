using System;

namespace Psz.Core.FinanceControl.Models.Budget.Order.Leasing
{
	public class PaymentHistoryModel
	{
		public int OrderId { get; set; }
		public decimal TotalAmount { get; set; }
		public int NbTotalMonths { get; set; }
		public decimal MonthAmount { get; set; }
		public int NbPayedMonths { get; set; }
		public DateTime LastPaymentDate { get; set; }
		public DateTime NextPaymentDate { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string CurrencyName { get; set; }
		public string DefaultCurrencyName { get; set; }
		public decimal DefaultCurrencyRate { get; set; }
		public PaymentHistoryModel(
			Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity bestellungenExtensionEntity,
			Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity lastValidation)
		{
			if(bestellungenExtensionEntity == null)
				return;

			OrderId = bestellungenExtensionEntity.OrderId;
			TotalAmount = bestellungenExtensionEntity.LeasingTotalAmount ?? 0;
			NbTotalMonths = bestellungenExtensionEntity.LeasingNbMonths ?? 0;
			MonthAmount = (bestellungenExtensionEntity.LeasingTotalAmount ?? 0) / (bestellungenExtensionEntity.LeasingNbMonths ?? 1);
			NbPayedMonths = (lastValidation.ValidationTime.Year - (bestellungenExtensionEntity.LeasingStartYear ?? (bestellungenExtensionEntity.ValidationRequestTime ?? DateTime.MinValue).Year)) * 12 + 1 + lastValidation.ValidationTime.Month - (bestellungenExtensionEntity.LeasingStartMonth ?? (bestellungenExtensionEntity.ValidationRequestTime ?? DateTime.MinValue).Month);
			LastPaymentDate = lastValidation.ValidationTime;
			NextPaymentDate = lastValidation.ValidationTime.AddMonths(1);
			StartDate = bestellungenExtensionEntity.ValidationRequestTime ?? DateTime.MinValue;
			EndDate = (bestellungenExtensionEntity.ValidationRequestTime ?? DateTime.MinValue).AddMonths(bestellungenExtensionEntity.LeasingNbMonths ?? 0);

			CurrencyName = bestellungenExtensionEntity.CurrencyName;
			DefaultCurrencyName = bestellungenExtensionEntity.DefaultCurrencyName;
			DefaultCurrencyRate = lastValidation?.DefaultCurrencyRate ?? 0;
		}
	}
}
