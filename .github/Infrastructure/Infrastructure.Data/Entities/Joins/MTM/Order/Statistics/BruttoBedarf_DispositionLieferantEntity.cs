using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.MTM.Order.Statistics
{
	public class BruttoBedarf_DispositionLieferantEntity
	{
		public int? Lief_Nr { get; set; }
		public int? Artikel_Nr { get; set; }
		public string N1 { get; set; }
		public bool? St1 { get; set; }
		public int? BW { get; set; }
		public decimal? M1 { get; set; }
		public decimal? P1 { get; set; }
		public string B1 { get; set; }
		public string T1 { get; set; }
		public string F1 { get; set; }
		public BruttoBedarf_DispositionLieferantEntity()
		{

		}
		public BruttoBedarf_DispositionLieferantEntity(DataRow dataRow)
		{
			Lief_Nr = (dataRow["Lief_Nr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Lief_Nr"].ToString());
			Artikel_Nr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Artikel-Nr"].ToString());
			N1 = (dataRow["N1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["N1"].ToString());
			St1 = (dataRow["St1"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["St1"].ToString());
			BW = (dataRow["BW"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["BW"].ToString());
			M1 = (dataRow["M1"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["M1"].ToString());
			P1 = (dataRow["P1"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["P1"].ToString());
			B1 = (dataRow["B1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["B1"].ToString());
			T1 = (dataRow["T1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["T1"].ToString());
			F1 = (dataRow["F1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["F1"].ToString());
		}
	}
}
