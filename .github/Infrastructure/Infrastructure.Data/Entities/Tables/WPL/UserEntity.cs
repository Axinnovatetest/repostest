using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.WPL
{
	public class UserEntity
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public DateTime CreationTime { get; set; }
		public bool IsActivated { get; set; }
		public int CreationUserId { get; set; }
		public string Name { get; set; }

		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public DateTime? DeleteTime { get; set; }
		public int? DeleteUserId { get; set; }
		public bool IsArchived { get; set; }

		public int AccessProfileId { get; set; }
		public string SelectedLanguage { get; set; }

		public bool SuperAdministrator { get; set; }

		public UserEntity() { }
		public UserEntity(DataRow dataRow)
		{
			Password = Convert.ToString(dataRow["Password"]);
			CreationTime = Convert.ToDateTime(dataRow["CreationTime"]);
			Name = (dataRow["Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IsActivated = Convert.ToBoolean(dataRow["IsActivated"]);
			Username = Convert.ToString(dataRow["Username"]);
			CreationUserId = Convert.ToInt32(dataRow["Creation_User_Id"]);
			LastEditTime = (dataRow["Last_Edit_Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Last_Edit_Date"]);
			DeleteTime = (dataRow["Delete_Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Delete_Date"]);
			LastEditUserId = (dataRow["Last_Edit_User_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Last_Edit_User_Id"]);
			DeleteUserId = (dataRow["Delete_User_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Delete_User_Id"]);
			IsArchived = Convert.ToBoolean(dataRow["Is_Archived"]);

			AccessProfileId = Convert.ToInt32(dataRow["AccessProfileId"]);
			SelectedLanguage = (dataRow["SelectedLanguage"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SelectedLanguage"]);

			SuperAdministrator = Convert.ToBoolean(dataRow["SuperAdministrator"]);
		}
	}
}

