using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	//
	public class ProjectTypeByMonth
	{
		public string Month { get; set; }
		public string ProjectTypeName { get; set; }
		public int Count { get; set; }
	}

	//


	public class ProjectTypeByMonthEntity
	{
		public string Month { get; set; }

		public int MonthNumber { get; set; }
		public string ProjectTypeName { get; set; }
		public int Count { get; set; }

		public ProjectTypeByMonthEntity()
		{

		}

		public ProjectTypeByMonthEntity(DataRow dataRow)
		{
			Month = (dataRow["_month"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["_month"]);

			MonthNumber = Convert.ToInt32(dataRow["MonthNumber"]);

			ProjectTypeName = (dataRow["projectTypeName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["projectTypeName"]);

			Count = Convert.ToInt32(dataRow["_count"]);
		}


	}
}
