using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class MaterialRequirementsFaNeedsSnapshotEntity
	{
		public int? FaArticleId { get; set; }
		public string FaArticleNumber { get; set; }
		public DateTime? FaDate { get; set; }
		public int? FaId { get; set; }
		public int? FaMaterialArticleId { get; set; }
		public string FaMaterialArticleNumber { get; set; }
		public DateTime? FaMaterialDate { get; set; }
		public decimal? FaMaterialOpenQuantity { get; set; }
		public int? FaNumber { get; set; }
		public decimal? FaOpenQuantity { get; set; }
		public int Id { get; set; }
		public int FaProductionSite { get; set; }

		public MaterialRequirementsFaNeedsSnapshotEntity() { }

		public MaterialRequirementsFaNeedsSnapshotEntity(DataRow dataRow)
		{
			FaArticleId = (dataRow["FaArticleId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FaArticleId"]);
			FaArticleNumber = (dataRow["FaArticleNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FaArticleNumber"]);
			FaDate = (dataRow["FaDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["FaDate"]);
			FaId = (dataRow["FaId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FaId"]);
			FaMaterialArticleId = (dataRow["FaMaterialArticleId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FaMaterialArticleId"]);
			FaMaterialArticleNumber = (dataRow["FaMaterialArticleNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FaMaterialArticleNumber"]);
			FaMaterialDate = (dataRow["FaMaterialDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["FaMaterialDate"]);
			FaMaterialOpenQuantity = (dataRow["FaMaterialOpenQuantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["FaMaterialOpenQuantity"]);
			FaNumber = (dataRow["FaNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FaNumber"]);
			FaOpenQuantity = (dataRow["FaOpenQuantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["FaOpenQuantity"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			FaProductionSite = Convert.ToInt32(dataRow["FaProductionSite"]);
		}

		public MaterialRequirementsFaNeedsSnapshotEntity ShallowClone()
		{
			return new MaterialRequirementsFaNeedsSnapshotEntity
			{
				FaArticleId = FaArticleId,
				FaArticleNumber = FaArticleNumber,
				FaDate = FaDate,
				FaId = FaId,
				FaMaterialArticleId = FaMaterialArticleId,
				FaMaterialArticleNumber = FaMaterialArticleNumber,
				FaMaterialDate = FaMaterialDate,
				FaMaterialOpenQuantity = FaMaterialOpenQuantity,
				FaNumber = FaNumber,
				FaOpenQuantity = FaOpenQuantity,
				Id = Id
			};
		}
	}
}

