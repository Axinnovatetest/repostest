using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class ProjectTypeEntity
	{
		public string Description { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }

		public ProjectTypeEntity() { }

		public ProjectTypeEntity(DataRow dataRow)
		{
			Description = (dataRow["Description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Name = (dataRow["Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name"]);
		}
	}
}

