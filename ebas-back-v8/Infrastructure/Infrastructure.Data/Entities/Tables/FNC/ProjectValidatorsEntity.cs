using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class ProjectValidatorsEntity
	{
		public string email { get; set; }
		public int? Id_Project { get; set; }
		public int Id_User { get; set; }
		public int Id_Validator { get; set; }
		public int ID { get; set; }
		public int? Level { get; set; }
		public DateTime? Validation_date { get; set; }

		public ProjectValidatorsEntity() { }

		public ProjectValidatorsEntity(DataRow dataRow)
		{
			email = (dataRow["email"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["email"]);
			Id_Project = (dataRow["Id_Project"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Project"]);
			Id_User = Convert.ToInt32(dataRow["Id_User"]);
			Id_Validator = Convert.ToInt32(dataRow["Id_Validator"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Level = (dataRow["Level"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Level"]);
			Validation_date = (dataRow["Validation_date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Validation_date"]);
		}
	}
}

