using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class PSZ_TechnikerEntity
	{
		public int ID { get; set; }
		public string Name { get; set; }

		public PSZ_TechnikerEntity() { }

		public PSZ_TechnikerEntity(DataRow dataRow)
		{
			ID = Convert.ToInt32(dataRow["ID"]);
			Name = (dataRow["Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name"]);
		}
	}
}

