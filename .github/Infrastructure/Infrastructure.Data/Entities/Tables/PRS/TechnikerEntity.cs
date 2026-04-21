using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class TechnikerEntity
	{
		public int ID { get; set; }
		public string Name { get; set; }

		public TechnikerEntity() { }

		public TechnikerEntity(DataRow dataRow)
		{
			ID = Convert.ToInt32(dataRow["ID"]);
			Name = (dataRow["Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name"]);
		}
	}
}

