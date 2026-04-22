using Infrastructure.Data.Entities.Joins.Logistics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.CRP
{
	public class OrderProcessingLogs
	{

		public OrderProcessingLogs() { }

		public OrderProcessingLogs(DataRow dataRow)
		{
			VorfallNr = (dataRow["Vorfall-Nr"] == System.DBNull.Value) ? null : Convert.ToInt32(dataRow["Vorfall-Nr"]);
			DokNr = (dataRow["DokNr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DokNr"]);
			Pos = (dataRow["Pos"] == System.DBNull.Value) ? null : Convert.ToInt32(dataRow["Pos"]);
			artikelnummer = (dataRow["PSZ#"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PSZ#"]);
			User = (dataRow["User"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["User"]);
			TYP = (dataRow["TYP"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["TYP"]);
			Log = (dataRow["Log"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Log"]);
			DateTime = (dataRow["DateTime"] == System.DBNull.Value) ? null : Convert.ToDateTime(dataRow["DateTime"]);
			totalRows = (dataRow["totalRows"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["totalRows"]);


		}


		public int? VorfallNr { get; set; }
		public string DokNr { get; set; }
		public int? Pos { get; set; }
		public string artikelnummer { get; set; }
		public string User { get; set; }
		public string TYP { get; set; }
		public string Log { get; set; }
		public int totalRows { get; set; }
		public DateTime? DateTime { get; set; }

	}
}
