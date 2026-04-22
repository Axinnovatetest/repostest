using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.FinanceControl.Models.Statistics
{
	public class OrdersOverviewModel
	{
		public int Id { get; set; }
		public string OrderNumber { get; set; }
		public string Supplier { get; set; }
		public decimal Amount { get; set; }
		public string PoPaymentTypeName { get; set; }
		public string OrderType { get; set; }
		public string Status { get; set; }
		public OrdersOverviewModel()
		{

		}
		public OrdersOverviewModel(Infrastructure.Data.Entities.Joins.FNC.Statistics.OrdersOverviewEntity entity)
		{
			Id = entity.Id;
			OrderNumber = entity.OrderNumber;
			Supplier = entity.SupplierName;
			Amount = entity.Amount;
			PoPaymentTypeName = entity.PoPaymentTypeName;
			OrderType = entity.OrderType;
			Status = ((Enums.BudgetEnums.ValidationLevels)entity.Status).GetDescription();
		}
	}
}