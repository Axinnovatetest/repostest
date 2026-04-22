using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.NLog
{
	public class NLogsSummary
	{
		public int TotalApiCalls { get; set; }
		public string MostCalledApi { get; set; }
		public DateTime LastUpdateDate { get; set; }

		public NLogsSummary(DataRow dataRow)
		{
			TotalApiCalls = Convert.ToInt32(dataRow["TotalApiCalls"]);
			MostCalledApi = (dataRow["MostCalledApi"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["MostCalledApi"]);
			LastUpdateDate = Convert.ToDateTime(dataRow["LastUpdateDate"]);
		}

	}
}
