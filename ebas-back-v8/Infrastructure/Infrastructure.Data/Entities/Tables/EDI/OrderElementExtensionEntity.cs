using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class OrderItemExtensionEntity
	{
		public DateTime CreationDate { get; set; }
		public int CreationUserId { get; set; }
		public int Id { get; set; }
		public int OrderItemId { get; set; }
		public int OrderId { get; set; }
		public decimal OriginalGesamtpreis { get; set; }
		public decimal OriginalQuantity { get; set; }
		public decimal OriginalVKGesamtpreis { get; set; }
		public int Status { get; set; }
		public DateTime? DesiredDate { get; set; }

		public int Version { get; set; }
		public DateTime LastUpdateTime { get; set; }
		public int LastUpdateUserId { get; set; }
		public string LastUpdateUsername { get; set; }

		public int? PrimaryPositionNumber { get; set; }
		public int? SecondaryPositionsCount { get; set; }

		public OrderItemExtensionEntity() { }
		public OrderItemExtensionEntity(DataRow dataRow)
		{
			CreationDate = Convert.ToDateTime(dataRow["CreationDate"]);
			CreationUserId = Convert.ToInt32(dataRow["CreationUserId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			OrderItemId = Convert.ToInt32(dataRow["OrderElementId"]);
			OrderId = Convert.ToInt32(dataRow["OrderId"]);
			OriginalGesamtpreis = Convert.ToDecimal(dataRow["OriginalGesamtpreis"]);
			OriginalQuantity = Convert.ToDecimal(dataRow["OriginalQuantity"]);
			OriginalVKGesamtpreis = Convert.ToDecimal(dataRow["OriginalVKGesamtpreis"]);
			Status = Convert.ToInt32(dataRow["Status"]);
			DesiredDate = Convert.ToDateTime(dataRow["DeliveryDate"]);

			Version = Convert.ToInt32(dataRow["Version"]);
			LastUpdateTime = Convert.ToDateTime(dataRow["LastUpdateTime"]);
			LastUpdateUserId = Convert.ToInt32(dataRow["LastUpdateUserId"]);
			LastUpdateUsername = Convert.ToString(dataRow["LastUpdateUsername"]);

			PrimaryPositionNumber = (dataRow["PrimaryPositionNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PrimaryPositionNumber"]);
			SecondaryPositionsCount = (dataRow["SecondaryPositionsCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SecondaryPositionsCount"]);
		}
	}
}

