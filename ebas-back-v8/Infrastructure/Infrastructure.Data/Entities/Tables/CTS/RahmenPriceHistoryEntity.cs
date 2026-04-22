using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class RahmenPriceHistoryEntity
	{
		public decimal? BasePrice { get; set; }
		public DateTime? DateUpdate { get; set; }
		public int id { get; set; }
		public int PositionNr { get; set; }
		public decimal? Price { get; set; }
		public decimal? PriceDefault { get; set; }
		public int RahmenNr { get; set; }
		public string UserName { get; set; }
		public DateTime? ValidFrom { get; set; }
		public string WarungSymbol { get; set; }

		public RahmenPriceHistoryEntity() { }

		public RahmenPriceHistoryEntity(DataRow dataRow)
		{
			BasePrice = (dataRow["BasePrice"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["BasePrice"]);
			DateUpdate = (dataRow["DateUpdate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["DateUpdate"]);
			id = Convert.ToInt32(dataRow["id"]);
			PositionNr = Convert.ToInt32(dataRow["PositionNr"]);
			Price = (dataRow["Price"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Price"]);
			PriceDefault = (dataRow["PriceDefault"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["PriceDefault"]);
			RahmenNr = Convert.ToInt32(dataRow["RahmenNr"]);
			UserName = (dataRow["UserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UserName"]);
			ValidFrom = (dataRow["ValidFrom"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ValidFrom"]);
			WarungSymbol = (dataRow["WarungSymbol"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["WarungSymbol"]);
		}

		public RahmenPriceHistoryEntity ShallowClone()
		{
			return new RahmenPriceHistoryEntity
			{
				BasePrice = BasePrice,
				DateUpdate = DateUpdate,
				id = id,
				PositionNr = PositionNr,
				Price = Price,
				PriceDefault = PriceDefault,
				RahmenNr = RahmenNr,
				UserName = UserName,
				ValidFrom = ValidFrom,
				WarungSymbol = WarungSymbol
			};
		}
	}
}

