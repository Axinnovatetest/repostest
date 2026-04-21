using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Joins.PRS
{
	public class __PRS_StockWarnings_FaStatusEntity
	{
		public int? ArtikelNr { get; set; }
		public string Unit { get; set; }
		public decimal? Qty { get; set; }
		public int? Week { get; set; }
		public int? Year { get; set; }

		public __PRS_StockWarnings_FaStatusEntity() { }

		public __PRS_StockWarnings_FaStatusEntity(DataRow dataRow)
		{
			ArtikelNr = (dataRow["ArtikelNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArtikelNr"]);
			Unit = (dataRow["Unit"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["Unit"]);
			Qty = (dataRow["Qty"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Qty"]);
			Week = (dataRow["Week"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Week"]);
			Year = (dataRow["Year"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Year"]);
		}

		public __PRS_StockWarnings_FaStatusEntity ShallowClone()
		{
			return new __PRS_StockWarnings_FaStatusEntity
			{
				ArtikelNr = ArtikelNr,
				Unit = Unit,
				Qty = Qty,
				Week = Week,
				Year = Year
			};
		}
	}
}

