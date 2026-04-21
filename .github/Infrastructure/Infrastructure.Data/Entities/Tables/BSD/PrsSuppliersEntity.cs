using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class PrsSuppliersEntity
	{
		public int? AllActiveArticlesCount { get; set; }
		public int? AllArticlesCount { get; set; }
		public int? BeKw { get; set; }
		public int? BeYear { get; set; }
		public int Id { get; set; }
		public int? StandardActiveArticlesCount { get; set; }
		public int? StandardArticlesCount { get; set; }
		public string Stufe { get; set; }
		public bool? SupplierAddressBlocked { get; set; }
		public int? SupplierAddressNr { get; set; }
		public bool? SupplierBlockedForFurtherBe { get; set; }
		public int? SupplierId { get; set; }
		public string SupplierName { get; set; }
		public DateTime? SyncDate { get; set; }
		public int? SyncId { get; set; }

		public PrsSuppliersEntity() { }

		public PrsSuppliersEntity(DataRow dataRow)
		{
			AllActiveArticlesCount = (dataRow["AllActiveArticlesCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["AllActiveArticlesCount"]);
			AllArticlesCount = (dataRow["AllArticlesCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["AllArticlesCount"]);
			BeKw = (dataRow["BeKw"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BeKw"]);
			BeYear = (dataRow["BeYear"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BeYear"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			StandardActiveArticlesCount = (dataRow["StandardActiveArticlesCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["StandardActiveArticlesCount"]);
			StandardArticlesCount = (dataRow["StandardArticlesCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["StandardArticlesCount"]);
			Stufe = (dataRow["Stufe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Stufe"]);
			SupplierAddressBlocked = (dataRow["SupplierAddressBlocked"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SupplierAddressBlocked"]);
			SupplierAddressNr = (dataRow["SupplierAddressNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SupplierAddressNr"]);
			SupplierBlockedForFurtherBe = (dataRow["SupplierBlockedForFurtherBe"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["SupplierBlockedForFurtherBe"]);
			SupplierId = (dataRow["SupplierId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SupplierId"]);
			SupplierName = (dataRow["SupplierName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierName"]);
			SyncDate = (dataRow["SyncDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["SyncDate"]);
			SyncId = (dataRow["SyncId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SyncId"]);
		}

		public PrsSuppliersEntity ShallowClone()
		{
			return new PrsSuppliersEntity
			{
				AllActiveArticlesCount = AllActiveArticlesCount,
				AllArticlesCount = AllArticlesCount,
				BeKw = BeKw,
				BeYear = BeYear,
				Id = Id,
				StandardActiveArticlesCount = StandardActiveArticlesCount,
				StandardArticlesCount = StandardArticlesCount,
				Stufe = Stufe,
				SupplierAddressBlocked = SupplierAddressBlocked,
				SupplierAddressNr = SupplierAddressNr,
				SupplierBlockedForFurtherBe = SupplierBlockedForFurtherBe,
				SupplierId = SupplierId,
				SupplierName = SupplierName,
				SyncDate = SyncDate,
				SyncId = SyncId
			};
		}
	}
}

