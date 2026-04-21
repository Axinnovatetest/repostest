using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class ProjectFileEntity
	{
		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public string CreationUserName { get; set; }
		public string FileExtension { get; set; }
		public int FileId { get; set; }
		public string FileName { get; set; }
		public int Id { get; set; }
		public int ProjectId { get; set; }

		public ProjectFileEntity() { }

		public ProjectFileEntity(DataRow dataRow)
		{
			CreationTime = Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = Convert.ToInt32(dataRow["CreationUserId"]);
			CreationUserName = Convert.ToString(dataRow["CreationUserName"]);
			FileExtension = Convert.ToString(dataRow["FileExtension"]);
			FileId = Convert.ToInt32(dataRow["FileId"]);
			FileName = Convert.ToString(dataRow["FileName"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			ProjectId = Convert.ToInt32(dataRow["ProjectId"]);
		}
	}
}

