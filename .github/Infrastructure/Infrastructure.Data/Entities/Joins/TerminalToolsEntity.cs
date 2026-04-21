using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins
{
	public class TerminalToolsEntity
	{
		public string Tool { get; set; }
		public int? Lager { get; set; }
		public TerminalToolsEntity()
		{

		}
		public TerminalToolsEntity(DataRow dataRow)
		{
			Tool = (dataRow["Inventarnummer JPM"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Inventarnummer JPM"]);
			Lager = (dataRow["Lagerplatz"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerplatz"]);
		}
	}
}
