using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class FaHoursMovmentDataEntity
	{
		public int Week { get; set; }
		public int Year { get; set; }
		public string? Lager { get; set; }
		public decimal? Hours { get; set; }
		public int? LagerId { get; set; }
		public int? FertigungNummer { get; set; }

		public FaHoursMovmentDataEntity(DataRow dataRow)
		{
			Week = Convert.ToInt32(dataRow["KW"]);
			Year = Convert.ToInt32(dataRow["KW_YEAR"]);
			Lager = (dataRow["Lager"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lager"]);
			Hours = (dataRow["Stunden_Sum"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Stunden_Sum"]);
			LagerId = (dataRow["LagerId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LagerId"]);
			FertigungNummer = (dataRow["fertigungNummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["fertigungNummer"]);
		}
	}
}
