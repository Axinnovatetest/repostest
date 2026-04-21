using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.MTM.CRP
{
	public class ArticleFaTimeEntity
	{
		public int ArticleNr { get; set; }
		public string ArticleNumber { get; set; }
		public int FaNr { get; set; }
		public string FaNumber { get; set; }
		public decimal? FaQuantity { get; set; }
		public decimal? FaTime { get; set; }
		public decimal? ApTime { get; set; }
		public ArticleFaTimeEntity(DataRow dataRow)
		{
			ArticleNr = Convert.ToInt32(dataRow["ArticleNr"]);
			ArticleNumber = (dataRow["ArticleNumber"] == System.DBNull.Value) ? (string)null : Convert.ToString(dataRow["ArticleNumber"]);
			FaNr = Convert.ToInt32(dataRow["FaNr"]);
			FaNumber = (dataRow["FaNumber"] == System.DBNull.Value) ? (string)null : Convert.ToString(dataRow["FaNumber"]);
			FaQuantity = (dataRow["FaQuantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["FaQuantity"]);
			FaTime = (dataRow["FaTime"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["FaTime"]);
			ApTime = (dataRow["ApTime"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ApTime"]);
		}
	}
}
