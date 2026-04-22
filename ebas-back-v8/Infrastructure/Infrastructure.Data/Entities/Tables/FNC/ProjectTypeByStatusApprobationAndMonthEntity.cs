using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class ProjectTypeByStatusApprobationAndMonthEntity
	{
		public int MonthNumber { get; set; }
		public string ProjectType { get; set; }

		public string Status { get; set; }
		public int Count { get; set; }
		public ProjectTypeByStatusApprobationAndMonthEntity() { }


		public ProjectTypeByStatusApprobationAndMonthEntity(DataRow dataRow)
		{
			MonthNumber = Convert.ToInt32(dataRow["monthNumber"]);

			ProjectType = (dataRow["Type"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Type"]);

			Status = (dataRow["ProjectStatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProjectStatus"]);

			Count = Convert.ToInt32(dataRow["Count"]);
		}

	}
}
