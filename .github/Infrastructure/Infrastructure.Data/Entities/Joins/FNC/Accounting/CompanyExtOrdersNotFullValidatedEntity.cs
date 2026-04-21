using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.FNC.Accounting
{
	public class CompanyExtOrdersNotFullValidatedEntity
	{
		public int Count { get; set; }
		public int ValidationLevel { get; set; }
		public string CompanyName { get; set; }
		public string Username { get; set; }

		public CompanyExtOrdersNotFullValidatedEntity(DataRow dataRow)
		{
			Count = (dataRow["Count"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Count"].ToString());
			ValidationLevel = (dataRow["ValidationLevel"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["ValidationLevel"].ToString());
			CompanyName = (dataRow["CompanyName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CompanyName"].ToString());
			Username = (dataRow["Username"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Username"].ToString());
		}
	}
}
