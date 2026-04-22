using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.MTM.CRP
{
	public class FertigungFaultyTimeEntity
	{
		public int FaNumber { get; set; }
		public DateTime? FaDate { get; set; }
		public string FaArticle { get; set; }
		public decimal FaQuantity { get; set; }
		public decimal FaUnitTime { get; set; }
		public decimal FaTotalTime { get; set; }
		public int WPL_ArticleId { get; set; }
		public int WorkScheduleId { get; set; }
		public FertigungFaultyTimeEntity(DataRow dataRow)
		{
			FaNumber = Convert.ToInt32(dataRow["FaNumber"]);
			FaDate = (dataRow["FaDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["FaDate"]);
			FaArticle = (dataRow["FaArticle"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FaArticle"]);
			FaQuantity = Convert.ToDecimal(dataRow["FaQuantity"]);
			FaUnitTime = Convert.ToDecimal(dataRow["FaUnitTime"]);
			FaTotalTime = Convert.ToDecimal(dataRow["FaTotalTime"]);
			WPL_ArticleId = Convert.ToInt32(dataRow["WPL_ArticleId"]);
			WorkScheduleId = Convert.ToInt32(dataRow["WorkScheduleId"]);
		}
	}
}
