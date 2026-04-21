using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class FertigungFGEntity
	{
		public FertigungFGEntity(DataRow dataRow)
		{
			Lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);
		}
		public string Lagerort { get; set; }
	}
}
