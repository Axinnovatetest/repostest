using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Budget_testEntity
	{
		public int ID { get; set; }
		public string value_1 { get; set; }
		public string value_2 { get; set; }
		public int? value_3 { get; set; }
		public bool? value_4 { get; set; }

		public Budget_testEntity() { }

		public Budget_testEntity(DataRow dataRow)
		{
			ID = Convert.ToInt32(dataRow["ID"]);
			value_1 = (dataRow["value_1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["value_1"]);
			value_2 = (dataRow["value_2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["value_2"]);
			value_3 = (dataRow["value_3"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["value_3"]);
			value_4 = (dataRow["value_4"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["value_4"]);
		}
	}
}

