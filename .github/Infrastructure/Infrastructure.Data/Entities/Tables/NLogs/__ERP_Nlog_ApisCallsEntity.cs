using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.NLogs
{
	public class __ERP_Nlog_ApisCallsEntity
	{
		public int Id { get; set; }
		public string Api { get; set; }
		public string Url { get; set; }
		public int? calls_all { get; set; }
		public int? calls_last_3_months { get; set; }
		public int? calls_last_6_months { get; set; }
		public int? calls_last_12_months { get; set; }
		public __ERP_Nlog_ApisCallsEntity()
		{

		}
		public __ERP_Nlog_ApisCallsEntity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			Api = (dataRow["Api"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["Api"]);
			Url = (dataRow["Url"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["Url"]);
			calls_all = (dataRow["calls_all"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["calls_all"]);
			calls_last_3_months = (dataRow["calls_last_3_months"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["calls_last_3_months"]);
			calls_last_6_months = (dataRow["calls_last_6_months"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["calls_last_6_months"]);
			calls_last_3_months = (dataRow["calls_last_3_months"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["calls_last_3_months"]);
			calls_last_12_months = (dataRow["calls_last_12_months"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["calls_last_12_months"]);
		}
	}
}
