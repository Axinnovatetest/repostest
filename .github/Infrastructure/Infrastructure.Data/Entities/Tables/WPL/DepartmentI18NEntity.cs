using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.WPL
{
	public class DepartmentI18NEntity
	{
		public string CodeLanguage { get; set; }
		public string Description { get; set; }
		public int Id { get; set; }
		public int IdDepartment { get; set; }
		public int IdLanguage { get; set; }
		public string Name { get; set; }

		public DepartmentI18NEntity() { }

		public DepartmentI18NEntity(DataRow dataRow)
		{
			CodeLanguage = Convert.ToString(dataRow["CodeLanguage"]);
			Description = (dataRow["Description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IdDepartment = Convert.ToInt32(dataRow["IdDepartment"]);
			IdLanguage = Convert.ToInt32(dataRow["IdLanguage"]);
			Name = (dataRow["Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name"]);
		}
	}
}

