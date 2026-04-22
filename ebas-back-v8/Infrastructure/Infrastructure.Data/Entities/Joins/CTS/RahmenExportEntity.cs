using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class RahmenExportEntity
	{
		public string BlanketDocNumber { get; set; }
		public string BlanketNumber { get; set; }
		public string SupplierName { get; set; }
		public string ArticleNumber { get; set; }
		public string ArticleDesignation1 { get; set; }
		public string ArticleDesignation2 { get; set; }
		public decimal OriginalQuantity { get; set; }
		public decimal RestQuantity { get; set; }
		public decimal UnitPrice { get; set; }
		public decimal RestPrice { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string Status { get; set; }
		public decimal Consumption { get; set; }
		public RahmenExportEntity(DataRow dataRow)
		{
			BlanketDocNumber = (dataRow["BlanketDocNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BlanketDocNumber"]);
			BlanketNumber = (dataRow["BlanketNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BlanketNumber"]);
			SupplierName = (dataRow["SupplierName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierName"]);
			ArticleNumber = (dataRow["ArticleNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleNumber"]);
			ArticleDesignation1 = (dataRow["ArticleDesignation1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleDesignation1"]);
			ArticleDesignation2 = (dataRow["ArticleDesignation2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleDesignation2"]);
			OriginalQuantity = (dataRow["OriginalQuantity"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["OriginalQuantity"]);
			RestQuantity = (dataRow["RestQuantity"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["RestQuantity"]);
			UnitPrice = (dataRow["UnitPrice"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["UnitPrice"]);
			RestPrice = (dataRow["RestPrice"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["RestPrice"]);
			StartDate = (dataRow["StartDate"] == System.DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(dataRow["StartDate"]);
			EndDate = (dataRow["EndDate"] == System.DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(dataRow["EndDate"]);
			Status = (dataRow["Status"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Status"]);
			Consumption = (dataRow["Consumption"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Consumption"]);
		}
	}
}
