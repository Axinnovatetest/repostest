using Infrastructure.Data.Entities.Joins.Logistics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.MGO
{
	public class ProductionWorkload_WeekFa
	{
		public int FaId { get; set; }
		public int? FaNumber { get; set; }
		public DateTime? FaCreationTime { get; set; }
		public DateTime? FaProductionTime { get; set; }
		public int? ArticleId { get; set; }
		public string ArticleNumber { get; set; }
		public int? OrderId { get; set; }
		public int? OrderNumber { get; set; }
		public ProductionWorkload_WeekFa() { }

		public ProductionWorkload_WeekFa(DataRow dataRow)
		{
			FaId =  Convert.ToInt32(dataRow["FaId"]);
			FaNumber = (dataRow["FaNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FaNumber"]);
			ArticleId = (dataRow["ArticleId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArticleId"]);
			OrderId = (dataRow["OrderId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OrderId"]);
			OrderNumber = (dataRow["OrderNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OrderNumber"]);
			FaCreationTime = (dataRow["FaCreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["FaCreationTime"]);
			FaProductionTime = (dataRow["FaProductionTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["FaProductionTime"]);
			ArticleNumber = (dataRow["ArticleNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleNumber"]);
		}
	}
}
