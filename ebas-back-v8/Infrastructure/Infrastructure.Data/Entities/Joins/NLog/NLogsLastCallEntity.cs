using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.NLog
{
	public class NLogsLastCallEntity
	{
		public DateTime? LastCall { get; set; }
		public string Url { get; set; }
		public NLogsLastCallEntity()
		{

		}
		public NLogsLastCallEntity(DataRow dataRow)
		{
			LastCall = (dataRow["LastCall"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastCall"]);
			Url = (dataRow["Url"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["Url"]);
		}

	}
}
