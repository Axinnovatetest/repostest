using System;

namespace Psz.Core.FinanceControl.Models.Budget
{
	public class OrderModel
	{
		public int Id { get; set; }
		public string Number { get; set; }
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

		public OrderModel()
		{ }
		public OrderModel(Infrastructure.Data.Entities.Tables.FNC.Budget_OrderEntity orderEntity,
			 Infrastructure.Data.Entities.Tables.FNC.AdressenEntity adressenEntity,
			 Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity projectEntity,
			 Infrastructure.Data.Entities.Tables.FNC.WahrungenEntity currencyEntity)
		{
			this.Id = orderEntity.Id_Order;
			this.Number = orderEntity.Order_Number;

			this.ProjectId = orderEntity.Id_Project ?? -1;
			this.ProjectName = projectEntity.ProjectName;

			this.DepartementId = orderEntity.Id_Dept;
			this.DepartementName = orderEntity.Dept_name;

			this.LandId = orderEntity.Id_Land ?? -1;
			this.LandName = orderEntity.Land_name;

			this.SupplierId = orderEntity.Id_Supplier;
			this.SupplierName = adressenEntity.Name1;

			this.CurrencyId = orderEntity.Id_Currency_Order;
			this.CurrencyName = currencyEntity.Wahrung;
		}

	}


}
