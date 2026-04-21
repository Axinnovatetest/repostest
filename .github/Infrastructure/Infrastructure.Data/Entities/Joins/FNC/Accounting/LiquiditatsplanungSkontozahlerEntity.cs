using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FNC.Accounting
{
	public class LiquiditatsplanungSkontozahlerEntity
	{
		public string Name1 { get; set; }
		public DateTime? Ausliefertermin { get; set; }
		public string Konditionen { get; set; }
		public DateTime? Zahlungseingang { get; set; }
		public double Brutto_inkl_Skonto { get; set; }
		public int TotalCount { get; set; }
		public LiquiditatsplanungSkontozahlerEntity(DataRow dataRow)
		{
			TotalCount = (dataRow["TotalCount"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["TotalCount"].ToString());
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"].ToString());
			Ausliefertermin = (dataRow["Ausliefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Ausliefertermin"].ToString());
			Konditionen = (dataRow["Konditionen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Konditionen"].ToString());
			Zahlungseingang = (dataRow["Zahlungseingang"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Zahlungseingang"].ToString());
			Brutto_inkl_Skonto = ((string.IsNullOrEmpty(dataRow["Brutto_inkl_Skonto"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["Brutto_inkl_Skonto"].ToString())) || dataRow["Brutto_inkl_Skonto"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Brutto_inkl_Skonto"].ToString());
		}
	}
}
