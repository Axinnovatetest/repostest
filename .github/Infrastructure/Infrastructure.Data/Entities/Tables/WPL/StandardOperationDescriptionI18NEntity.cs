using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.WPL
{
	public class StandardOperationDescriptionI18NEntity
	{
		public string CodeLanguage { get; set; }
		public string Description { get; set; }
		public int Id { get; set; }
		public int IdLanguage { get; set; }
		public int IdStandardOperationDescription { get; set; }
		public string Name { get; set; }

		public StandardOperationDescriptionI18NEntity() { }

		public StandardOperationDescriptionI18NEntity(DataRow dataRow)
		{
			CodeLanguage = Convert.ToString(dataRow["CodeLanguage"]);
			Description = (dataRow["Description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IdLanguage = Convert.ToInt32(dataRow["IdLanguage"]);
			IdStandardOperationDescription = Convert.ToInt32(dataRow["IdStandardOperationDescription"]);
			Name = (dataRow["Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name"]);
		}
	}
}

