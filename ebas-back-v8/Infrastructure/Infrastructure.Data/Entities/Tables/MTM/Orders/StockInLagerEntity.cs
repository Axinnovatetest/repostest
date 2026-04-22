using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.MTM.Orders
{
	public class StockInLagerEntity
	{
		public decimal? Stock { get; set; }
		public StockInLagerEntity(DataRow dataRow)
		{
			Stock = (dataRow["Stock"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Stock"]);
		}
		public StockInLagerEntity()
		{

		}
	}
	public class StockInLagerTypeEntity
	{
		public decimal? Stock { get; set; }
		public StockInLagerTypeEntity(DataRow dataRow)
		{
			Stock = (dataRow["totalInitStock"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["totalInitStock"]);
		}
		public StockInLagerTypeEntity()
		{

		}
	}
	// last touched ...
	public class StockInLagerTypeWithArtikelNrEntity
	{
		public int ArtikelNr { get; set; }
		public decimal? Stock { get; set; }
		public StockInLagerTypeWithArtikelNrEntity(DataRow dataRow)
		{
			ArtikelNr = (dataRow["ArtikelNr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["ArtikelNr"]);
			Stock = (dataRow["totalInitStock"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["totalInitStock"]);
		}
		public StockInLagerTypeWithArtikelNrEntity()
		{

		}
	}
	public class MinStockInLagerEntity
	{
		public decimal? Mindestbestand { get; set; }
		public MinStockInLagerEntity(DataRow dataRow)
		{
			Mindestbestand = (dataRow["Mindestbestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Mindestbestand"]);
		}
		public MinStockInLagerEntity()
		{

		}
	}
}
