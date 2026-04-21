using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.NLog
{
	public class NLogsCountEntity
	{
		public string Url { get; set; }
		public int RecentMonthCall { get; set; }
		public int Month2Count { get; set; }
		public int Month3Count { get; set; }
		public int Month4Count { get; set; }
		public int Month5Count { get; set; }
		public int Month6Count { get; set; }
		public int TotalCount { get; set; }
		public NLogsCountEntity()
		{

		}
		public NLogsCountEntity(DataRow dataRow)
		{
			Url = (dataRow["Url"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["Url"]);
			RecentMonthCall = Convert.ToInt32(dataRow["Recent_Month_Count"]);
			Month2Count = Convert.ToInt32(dataRow["Month_2_Count"]);
			Month3Count = Convert.ToInt32(dataRow["Month_3_Count"]);
			Month4Count = Convert.ToInt32(dataRow["Month_4_Count"]);
			Month5Count = Convert.ToInt32(dataRow["Month_5_Count"]);
			Month6Count = Convert.ToInt32(dataRow["Month_6_Count"]);
			TotalCount = Convert.ToInt32(dataRow["Total_Count"]);

		}
	}
}
