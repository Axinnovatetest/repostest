using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Budget_landsNamesEntity
	{
		public int ID { get; set; }
		public string Land_name { get; set; }

		public Budget_landsNamesEntity() { }

		public Budget_landsNamesEntity(DataRow dataRow)
		{
			Land_name = (dataRow["Land_name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Land_name"]);
			ID = Convert.ToInt32(dataRow["ID"]);
		}
	}
}
