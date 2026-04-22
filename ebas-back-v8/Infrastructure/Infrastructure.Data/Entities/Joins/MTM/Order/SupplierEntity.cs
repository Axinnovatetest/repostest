using System;
using System.Data;
using System.Globalization;

namespace Infrastructure.Data.Entities.Joins.MTM.Order
{
	public class SupplierEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public decimal MinimumQuantity { get; set; }
		public decimal ShippingDays { get; set; }
		public decimal PurchasePrice { get; set; }
		public bool IsDefault { get; set; }
		public SupplierEntity(DataRow dataRow)
		{
			Id = (dataRow["Lieferanten-Nr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Lieferanten-Nr"]);
			Name = dataRow["Name"].ToString();
			MinimumQuantity = (dataRow["Mindestbestellmenge"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Mindestbestellmenge"].ToString());
			ShippingDays = (dataRow["Wiederbeschaffungszeitraum"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Wiederbeschaffungszeitraum"].ToString());
			PurchasePrice = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? 0 : Decimal.TryParse(dataRow["Einkaufspreis"].ToString(), out var e) ? e : 0m;
			IsDefault = (dataRow["Standardlieferant"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["Standardlieferant"].ToString());
		}
	}
}
