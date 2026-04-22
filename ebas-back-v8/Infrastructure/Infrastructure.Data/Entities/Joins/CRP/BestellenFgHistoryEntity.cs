using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.CRP
{
	public class BestellenFgHistoryEntity
	{
		public string? ArticleDesignation1 { get; set; }
		public string? ArticleDesignation2 { get; set; }
		public string? ArticleNumber { get; set; }
		public string? ArticleReleaseStatus { get; set; }
		public string? CsContact { get; set; }
		public string? CustomerName { get; set; }
		public int? CustomerNumber { get; set; }
		public bool? EdiStandard { get; set; }
		public decimal? StockQuantity { get; set; }
		public decimal? TotalCostsWithCu { get; set; }
		public decimal? TotalCostsWithoutCu { get; set; }
		public decimal? TotalSalesPrice { get; set; }
		public bool? UBG { get; set; }
		public bool? VKE { get; set; }
		public decimal? UnitSalesPrice { get; set; }
		public string? WarehouseName { get; set; }

		public DateTime? ImportDate { get; set; }
	public BestellenFgHistoryEntity(DataRow dataRow)
		{
			ArticleDesignation1 = (dataRow["ArticleDesignation1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleDesignation1"]);
			ArticleDesignation2 = (dataRow["ArticleDesignation2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleDesignation2"]);
			ArticleNumber = (dataRow["ArticleNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleNumber"]);
			ArticleReleaseStatus = (dataRow["ArticleReleaseStatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleReleaseStatus"]);
			CsContact = (dataRow["CsContact"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CsContact"]);
			CustomerName = (dataRow["CustomerName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerName"]);
			CustomerNumber = (dataRow["CustomerNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerNumber"]);
			EdiStandard = (dataRow["EdiStandard"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EdiStandard"]);
			StockQuantity = (dataRow["StockQuantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["StockQuantity"]);
			TotalCostsWithCu = (dataRow["TotalCostsWithCu"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["TotalCostsWithCu"]);
			TotalCostsWithoutCu = (dataRow["TotalCostsWithoutCu"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["TotalCostsWithoutCu"]);
			TotalSalesPrice = (dataRow["TotalSalesPrice"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["TotalSalesPrice"]);
			UBG = (dataRow["UBG"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["UBG"]);
			UnitSalesPrice = (dataRow["UnitSalesPrice"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["UnitSalesPrice"]);
			VKE = (dataRow["VKE"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["VKE"]);
			WarehouseName = (dataRow["WarehouseName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["WarehouseName"]);
			ImportDate = (dataRow["ImportDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ImportDate"]);

		}
	} 
}
