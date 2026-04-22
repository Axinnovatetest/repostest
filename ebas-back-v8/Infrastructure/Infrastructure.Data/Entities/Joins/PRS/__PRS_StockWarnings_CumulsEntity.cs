using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Joins.PRS
{
	public class __PRS_StockWarnings_CumulsEntity
	{
		public int? ArtikelNr { get; set; }
		public string Unit { get; set; }
		public decimal? Qty { get; set; }
		public decimal? SumProd_cumul { get; set; }
		public int? Week { get; set; }
		public int? Year { get; set; }

		public __PRS_StockWarnings_CumulsEntity() { }

		public __PRS_StockWarnings_CumulsEntity(DataRow dataRow)
		{
			ArtikelNr = dataRow["ArtikelNr"] == DBNull.Value ? null : Convert.ToInt32(dataRow["ArtikelNr"]);
			Unit = dataRow["Unit"] == DBNull.Value ? null : Convert.ToString(dataRow["Unit"]);
			Qty = dataRow["Qty"] == DBNull.Value ? null : Convert.ToDecimal(dataRow["Qty"]);
			SumProd_cumul = dataRow["SumProd_cumul"] == DBNull.Value ? null : Convert.ToDecimal(dataRow["SumProd_cumul"]);
			Week = dataRow["Week"] == DBNull.Value ? null : Convert.ToInt32(dataRow["Week"]);
			Year = dataRow["Year"] == DBNull.Value ? null : Convert.ToInt32(dataRow["Year"]);
		}

		public __PRS_StockWarnings_CumulsEntity ShallowClone()
		{
			return new __PRS_StockWarnings_CumulsEntity
			{
				ArtikelNr = ArtikelNr,
				Unit = Unit,
				Qty = Qty,
				SumProd_cumul = SumProd_cumul,
				Week = Week,
				Year = Year
			};
		}
	}
}

