using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class ProjectTypeOrderCountEntity
	{
		public string Month { get; set; }
		public int MonthNumber { get; set; }
		public string ProjectTypeName { get; set; }
		public int NumberOfOrders { get; set; }

		public ProjectTypeOrderCountEntity()
		{

		}

		public ProjectTypeOrderCountEntity(DataRow dataRow)
		{
			Month = (dataRow["Month"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Month"]);

			MonthNumber = Convert.ToInt32(dataRow["MonthNumber"]);

			ProjectTypeName = (dataRow["Type"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Type"]);

			NumberOfOrders = Convert.ToInt32(dataRow["NumberOfOrders"]);
		}


	}
}
