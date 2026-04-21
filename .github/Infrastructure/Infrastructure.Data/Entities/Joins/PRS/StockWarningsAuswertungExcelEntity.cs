using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Infrastructure.Data.Entities.Joins.PRS
{
	public class StockWarningsAuswertungExcelEntity
	{
		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public string Name1 { get; set; }
		public decimal? Einkaufspreis { get; set; }
		public decimal? stock { get; set; }
		public decimal? Total_gross_requirements { get; set; }
		public decimal? Total_confirmed_orders { get; set; }
		public decimal? Total_unconfirmed_orders { get; set; }
		public int? Material_date_CW { get; set; }
		public decimal? Difference { get; set; }
		public decimal? Minimum_stock { get; set; }
		public decimal? Minimum_order_quantity { get; set; }
		public decimal? Packaging_quantity { get; set; }
		public int? Replenishment_time { get; set; }
		public decimal? Open_frame_quantity { get; set; }
		public decimal? Obsolete { get; set; }
		public StockWarningsAuswertungExcelEntity()
		{

		}
		public StockWarningsAuswertungExcelEntity(DataRow dataRow)
		{
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bezeichnung1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
			Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? 0m : Convert.ToDecimal(dataRow["Einkaufspreis"]);
			stock = (dataRow["stock"] == System.DBNull.Value) ? 0m : Convert.ToDecimal(dataRow["stock"]);
			Total_gross_requirements = (dataRow["Total_gross_requirements"] == System.DBNull.Value) ? 0m : Convert.ToDecimal(dataRow["Total_gross_requirements"]);
			Total_confirmed_orders = (dataRow["Total_confirmed_orders"] == System.DBNull.Value) ? 0m : Convert.ToDecimal(dataRow["Total_confirmed_orders"]);
			Total_unconfirmed_orders = (dataRow["Total_unconfirmed_orders"] == System.DBNull.Value) ? 0m : Convert.ToDecimal(dataRow["Total_unconfirmed_orders"]);
			Material_date_CW = (dataRow["Material_date_CW"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Material_date_CW"]);
			Difference = (dataRow["Difference"] == System.DBNull.Value) ? 0m : Convert.ToDecimal(dataRow["Difference"]);
			Minimum_stock = (dataRow["Minimum_stock"] == System.DBNull.Value) ? 0m : Convert.ToDecimal(dataRow["Minimum_stock"]);
			Minimum_order_quantity = (dataRow["Minimum_order_quantity"] == System.DBNull.Value) ? 0m : Convert.ToDecimal(dataRow["Minimum_order_quantity"]);
			Packaging_quantity = (dataRow["Packaging_quantity"] == System.DBNull.Value) ? 0m : Convert.ToDecimal(dataRow["Packaging_quantity"]);
			Replenishment_time = (dataRow["Replenishment_time"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Replenishment_time"]);
			Open_frame_quantity = (dataRow["Open_frame_quantity"] == System.DBNull.Value) ? 0m : Convert.ToDecimal(dataRow["Open_frame_quantity"]);
			Obsolete = (dataRow["Obsolete"] == System.DBNull.Value) ? 0m : Convert.ToDecimal(dataRow["Obsolete"]);
		}
	}
}