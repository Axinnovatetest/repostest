using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FNC.Accounting
{
	public class RMDCZEntity
	{
		public string Ursprungsland { get; set; }
		public string Zolltarif_nr { get; set; }
		public int TotalCount { get; set; }
		public double Gewichte { get; set; }
		public double Warenwert { get; set; }
		public RMDCZEntity(DataRow dataRow)
		{
			TotalCount = (dataRow["TotalCount"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["TotalCount"].ToString());
			Zolltarif_nr = (dataRow["Zolltarif_nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zolltarif_nr"].ToString());
			//Zolltarif_nr = ((string.IsNullOrWhiteSpace(dataRow["Zolltarif_nr"].ToString())) ||  (dataRow["Zolltarif_nr"].ToString() == string.Empty) || (dataRow["Zolltarif_nr"] == System.DBNull.Value)) ? 0 : Convert.ToInt32(dataRow["Zolltarif_nr"].ToString());
			Ursprungsland = (dataRow["Ursprungsland"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ursprungsland"].ToString());
			Gewichte = ((string.IsNullOrEmpty(dataRow["Gewichte"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["Gewichte"].ToString())) || dataRow["Gewichte"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Gewichte"].ToString());
			Warenwert = ((string.IsNullOrEmpty(dataRow["Warenwert"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["Warenwert"].ToString())) || dataRow["Warenwert"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Warenwert"].ToString());
		}
	}
}
