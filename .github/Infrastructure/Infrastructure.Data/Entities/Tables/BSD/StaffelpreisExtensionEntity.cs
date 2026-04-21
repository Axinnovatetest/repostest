using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class StaffelpreisExtensionEntity
	{
		public string DeliveryTime { get; set; }
		public int Id { get; set; }
		public int? LotSize { get; set; }
		public string PackagingQuantity { get; set; }
		public string PackagingType { get; set; }
		public int? PackagingTypeId { get; set; }
		public int? StaffelNr { get; set; }
		public string Type { get; set; }
		public int? TypeId { get; set; }

		public StaffelpreisExtensionEntity() { }

		public StaffelpreisExtensionEntity(DataRow dataRow)
		{
			DeliveryTime = (dataRow["DeliveryTime"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DeliveryTime"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LotSize = (dataRow["LotSize"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LotSize"]);
			PackagingQuantity = (dataRow["PackagingQuantity"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PackagingQuantity"]);
			PackagingType = (dataRow["PackagingType"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PackagingType"]);
			PackagingTypeId = (dataRow["PackagingTypeId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PackagingTypeId"]);
			StaffelNr = (dataRow["StaffelNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["StaffelNr"]);
			Type = (dataRow["Type"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Type"]);
			TypeId = (dataRow["TypeId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TypeId"]);
		}
	}
}

