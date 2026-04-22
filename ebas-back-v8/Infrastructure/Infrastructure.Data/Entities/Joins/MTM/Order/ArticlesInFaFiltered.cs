using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.MTM.Order
{
	public class ArticlesInFaFiltered
	{
		public int ArtikelNr { get; set; }
		public string ArtikelNummer { get; set; }
		public decimal CumulativeStock { get; set; }
		public DateTime? DateIssue { get; set; }
		public int TotalCount { get; set; }
		public ArticlesInFaFiltered(DataRow dataRow)
		{
			ArtikelNr = (dataRow["ArtikelNr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["ArtikelNr"].ToString());
			TotalCount = (dataRow["TotalCount"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["TotalCount"].ToString());
			ArtikelNummer = (dataRow["ArtikelNummer"] == System.DBNull.Value) ? "" : dataRow["ArtikelNummer"].ToString();
			CumulativeStock = (dataRow["CumulativeStock"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["CumulativeStock"].ToString());
			DateIssue = (dataRow["DateIssue"] == System.DBNull.Value) ? null : Convert.ToDateTime(dataRow["DateIssue"].ToString());
		}
	}
}
