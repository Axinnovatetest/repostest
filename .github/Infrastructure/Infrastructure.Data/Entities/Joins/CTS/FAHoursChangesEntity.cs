namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class FAHoursChangesEntity
	{
		public int Week { get; set; }
		public int Year { get; set; }
		public string? Lager { get; set; }
		public decimal? Hours { get; set; }
		public int? FaPositionZone { get; set; }
		public int TotalCount { get; set; }
		public int? LagerId { get; set; }

		public FAHoursChangesEntity(DataRow dataRow)
		{
			Week = Convert.ToInt32(dataRow["KW"]);
			Year = Convert.ToInt32(dataRow["KW_YEAR"]);
			Lager = (dataRow["Lager"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lager"]);
			FaPositionZone = (dataRow["FaPositionZone"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FaPositionZone"]);
			Hours = (dataRow["HoursLeft"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["HoursLeft"]);
			LagerId = (dataRow["LagerId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LagerId"]);
			TotalCount = Convert.ToInt32(dataRow["TotalCount"]);
		}
	}
}
