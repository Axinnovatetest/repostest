using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.WPL
{
	public class CountryEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Designation { get; set; }
		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public DateTime? ArchiveTime { get; set; }
		public int? ArchiveUserId { get; set; }
		public bool IsArchived { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public int? MtdArticleSequence { get; set; }
		public CountryEntity() { }
		public CountryEntity(DataRow dataRow)
		{
			CreationTime = (DateTime)dataRow["Creation_Date"];
			CreationUserId = int.Parse(dataRow["Creation_User_Id"].ToString());
			ArchiveTime = (dataRow["Delete_Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Delete_Date"]);
			ArchiveUserId = (dataRow["Delete_User_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Delete_User_Id"]);
			LastEditTime = (dataRow["Last_Edit_Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Last_Edit_Date"]);
			LastEditUserId = (dataRow["Last_Edit_User_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Last_Edit_User_Id"]);
			IsArchived = (Boolean)dataRow["Is_Archived"];

			Id = int.Parse(dataRow["Id"].ToString());
			Name = (string)dataRow["Name"];
			Designation = (string)dataRow["Designation"];
			MtdArticleSequence = (dataRow["MtdArticleSequence"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["MtdArticleSequence"]);
		}
		public CountryEntity ShallowClone()
		{
			return new CountryEntity
			{
				CreationTime = CreationTime,
				CreationUserId = CreationUserId,
				ArchiveTime = ArchiveTime,
				ArchiveUserId = ArchiveUserId,
				Designation = Designation,
				Id = Id,
				IsArchived = IsArchived,
				LastEditTime = LastEditTime,
				LastEditUserId = LastEditUserId,
				MtdArticleSequence = MtdArticleSequence,
				Name = Name
			};
		}
	}
}
