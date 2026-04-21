using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class Fibu_kunden_rahmenEntity
	{
		public int ID { get; set; }
		public string Rahmen { get; set; }

		public Fibu_kunden_rahmenEntity() { }

		public Fibu_kunden_rahmenEntity(DataRow dataRow)
		{
			ID = Convert.ToInt32(dataRow["ID"]);
			Rahmen = (dataRow["Rahmen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rahmen"]);
		}
	}
}

