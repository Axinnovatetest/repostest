using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.WPL
{
	public class FileEntity
	{
		public int Id { get; set; }

		public string Title { get; set; }

		public string Path { get; set; }
		public string Name { get; set; }
		public string Extension { get; set; }

		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }

		public DateTime? ArchiveTime { get; set; }
		public int? ArchiveUserId { get; set; }

		public FileEntity() { }
		public FileEntity(DataRow dataRow)
		{
			CreationTime = (DateTime)dataRow["Creation_Date"];
			CreationUserId = int.Parse(dataRow["Creation_User_Id"].ToString());
			ArchiveTime = (dataRow["Delete_Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Delete_Date"]);
			ArchiveUserId = (dataRow["Delete_User_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Delete_User_Id"]);
			Extension = (string)dataRow["Extension"];
			Id = int.Parse(dataRow["Id"].ToString());
			Name = (string)dataRow["Name"];
			Path = (string)dataRow["Path"];
			Title = (dataRow["Title"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Title"]);
		}
	}
}

