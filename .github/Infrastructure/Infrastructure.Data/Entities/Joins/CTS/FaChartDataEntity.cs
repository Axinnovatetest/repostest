using Infrastructure.Data.Entities.Joins.Logistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class FaChartDataEntity
	{
		public int ChangedYear { get; set; }
		public int AffectedWeek { get; set; }
		public int ChangeWeek { get; set; }
		public int AffectedYear { get; set; }
		public decimal? HoursLeft { get; set; }

		public FaChartDataEntity(DataRow dataRow)
		{
			ChangedYear = Convert.ToInt32(dataRow["ChangedYear"]);
			AffectedWeek = Convert.ToInt32(dataRow["AffectedWeek"]);
			ChangeWeek = Convert.ToInt32(dataRow["ChangeWeek"]);
			AffectedYear = Convert.ToInt32(dataRow["AffectedYear"]);
			HoursLeft = (dataRow["HoursLeft"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["HoursLeft"]);
		}
	}
}
