using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables
{
	public class FileEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Extension { get; set; }
		public string Title { get; set; }
		public string Path { get; set; }

		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }

		public DateTime? ArchiveTime { get; set; }
		public int? ArchiveUserId { get; set; }
		//
		public int? Module { get; set; }

		public FileEntity() { }
		public FileEntity(DataRow dataRow)
		{
			CreationTime = Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = Convert.ToInt32(dataRow["CreationUserId"]);
			ArchiveTime = (dataRow["ArchiveTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ArchiveTime"]);
			ArchiveUserId = (dataRow["ArchiveUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArchiveUserId"]);
			Extension = Convert.ToString(dataRow["Extension"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Name = Convert.ToString(dataRow["Name"]);
			Path = Convert.ToString(dataRow["Path"]);
			Title = (dataRow["Title"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Title"]);
			//
			Module = (dataRow["Module"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Module"]);
		}
	}
}

