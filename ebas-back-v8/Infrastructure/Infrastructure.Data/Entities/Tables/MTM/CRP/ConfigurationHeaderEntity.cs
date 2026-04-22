using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.MTM
{
	public class ConfigurationHeaderEntity
	{
		public int CountryId { get; set; }
		public string CountryName { get; set; }
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public int HallId { get; set; }
		public string HallName { get; set; }
		public int Id { get; set; }
		public decimal ProductionOrderThreshold { get; set; }

		public ConfigurationHeaderEntity() { }

		public ConfigurationHeaderEntity(DataRow dataRow)
		{
			CountryId = Convert.ToInt32(dataRow["CountryId"]);
			CountryName = (dataRow["CountryName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CountryName"]);
			CreationTime = (dataRow["CreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = (dataRow["CreationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreationUserId"]);
			HallId = Convert.ToInt32(dataRow["HallId"]);
			HallName = (dataRow["HallName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["HallName"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			ProductionOrderThreshold = Convert.ToDecimal(dataRow["ProductionOrderThreshold"]);
		}
	}
}

