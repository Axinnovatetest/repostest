using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.MTM
{
	public class HolidayEntity
	{
		public DateTime? ArchiveTime { get; set; }
		public int? ArchiveUserId { get; set; }
		public int CountryId { get; set; }
		public string CountryName { get; set; }
		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public DateTime Day { get; set; }
		public int HallId { get; set; }
		public string HallName { get; set; }
		public int Id { get; set; }
		public bool IsArchived { get; set; }
		public bool? IsOverwritten { get; set; }
		public string Name { get; set; }
		public int? WeekNumber { get; set; }

		public HolidayEntity() { }
		public HolidayEntity(DataRow dataRow)
		{
			ArchiveTime = (dataRow["ArchiveTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ArchiveTime"]);
			ArchiveUserId = (dataRow["ArchiveUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArchiveUserId"]);
			CountryId = Convert.ToInt32(dataRow["CountryId"]);
			CountryName = Convert.ToString(dataRow["CountryName"]);
			CreationTime = Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = Convert.ToInt32(dataRow["CreationUserId"]);
			Day = Convert.ToDateTime(dataRow["Day"]);
			HallId = Convert.ToInt32(dataRow["HallId"]);
			HallName = Convert.ToString(dataRow["HallName"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IsArchived = Convert.ToBoolean(dataRow["IsArchived"]);
			IsOverwritten = (dataRow["IsOverwritten"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsOverwritten"]);
			Name = Convert.ToString(dataRow["Name"]);
			WeekNumber = (dataRow["WeekNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["WeekNumber"]);
		}
	}
}

