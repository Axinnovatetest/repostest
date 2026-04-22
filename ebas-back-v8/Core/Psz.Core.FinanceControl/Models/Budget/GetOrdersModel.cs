using System;

namespace Psz.Core.FinanceControl.Models.Budget
{
	public class GetOrdersModel
	{
		public int Id { get; set; }
		public string Number { get; set; }
		public string Type { get; set; }
		public int ProjectId { get; set; }
		public string ProjectName { get; set; }
		public int? DepartementId { get; set; }
		public string DepartementName { get; set; }

		public int LandId { get; set; }
		public string LandName { get; set; }
		public int? SupplierId { get; set; }
		public string SupplierName { get; set; }
		public int? CurrencyId { get; set; }
		public string CurrencyName { get; set; }

		public DateTime? OrderDate { get; set; }

		public GetOrdersModel()
		{ }
		public GetOrdersModel(Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity orderEntity,
			 Infrastructure.Data.Entities.Tables.FNC.AdressenEntity adressenEntity,
			 Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity projectEntity,
			 Infrastructure.Data.Entities.Tables.FNC.Currency_BudgetEntity currencyEntity)
		{
			if(orderEntity == null || adressenEntity == null || projectEntity == null || currencyEntity == null)
				return;

			this.Id = orderEntity.Id_Order;
			this.Number = orderEntity.Order_Number;

			this.ProjectId = orderEntity.Id_Project ?? -1;
			this.ProjectName = projectEntity.ProjectName;

			this.DepartementId = orderEntity.Id_Dept;
			this.DepartementName = orderEntity.Dept_name;

			this.LandId = orderEntity.Id_Land ?? -1;
			this.LandName = orderEntity.Land_name;

			this.SupplierId = orderEntity.Id_Supplier;
			this.SupplierName = adressenEntity?.Name1;

			this.CurrencyId = orderEntity.Id_Currency_Order;

			// this.CurrencyName = "( " + currencyEntity.Symol + " )";


			this.CurrencyName = '(' + currencyEntity.Symol + ')';


			this.OrderDate = orderEntity.Order_date;
			this.Type = orderEntity.Type_Order;
		}
		public Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity ToBudgetOrders()
		{
			throw new Exception("not implemented");
		}
	}
}
