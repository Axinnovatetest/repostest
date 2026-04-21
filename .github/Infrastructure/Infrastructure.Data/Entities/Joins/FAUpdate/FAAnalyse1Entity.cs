using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FAUpdate
{
	public class FAAnalyse1Entity
	{
		public DateTime? T_F { get; set; }
		public DateTime? T_B1 { get; set; }
		public int? Fer { get; set; }
		public Decimal? Anz { get; set; }
		public string Artik_Nr { get; set; }
		public string B1 { get; set; }
		public Decimal? UmCZ { get; set; }
		public Decimal? Prod { get; set; }
		public string Artik_Nr2 { get; set; }
		public Decimal? Ver { get; set; }
		public Decimal? SummFAB { get; set; }
		public Decimal? SummBe { get; set; }
		public string BZ2 { get; set; }
		public FAAnalyse1Entity(DataRow dataRow)
		{
			T_F = (dataRow["T_F"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["T_F"]);
			T_B1 = (dataRow["T_B1"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["T_B1"]);
			Fer = (dataRow["Fer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fer"]);
			Anz = (dataRow["Anz"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Anz"]);
			Artik_Nr = (dataRow["Artik_Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artik_Nr"]);
			B1 = (dataRow["B1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["B1"]);
			UmCZ = (dataRow["UmCZ"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["UmCZ"]);
			Prod = (dataRow["Prod"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Prod"]);
			Artik_Nr2 = (dataRow["Artik_Nr2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artik_Nr2"]);
			Ver = (dataRow["Ver"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Ver"]);
			SummFAB = (dataRow["SummFAB"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["SummFAB"]);
			SummBe = (dataRow["SummBe"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["SummBe"]);
			BZ2 = (dataRow["BZ2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BZ2"]);

		}
	}
}
