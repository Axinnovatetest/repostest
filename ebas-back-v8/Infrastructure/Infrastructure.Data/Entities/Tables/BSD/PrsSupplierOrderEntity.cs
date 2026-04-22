using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class PrsSupplierOrderEntity
	{
		public decimal? BeArticleAmount { get; set; }
		public long? BeArticleCount { get; set; }
		public long? BeCount { get; set; }
		public int? BeKw { get; set; }
		public int? BeYear { get; set; }
		public int? LagerId { get; set; }
		public int Id { get; set; }
		public int? SupplierAddressNr { get; set; }
		public int? SyncId { get; set; }

		public PrsSupplierOrderEntity() { }

		public PrsSupplierOrderEntity(DataRow dataRow)
		{
			BeArticleAmount = (dataRow["BeArticleAmount"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["BeArticleAmount"]);
			BeArticleCount = (dataRow["BeArticleCount"] == System.DBNull.Value) ? (long?)null : Convert.ToInt64(dataRow["BeArticleCount"]);
			BeCount = (dataRow["BeCount"] == System.DBNull.Value) ? (long?)null : Convert.ToInt64(dataRow["BeCount"]);
			BeKw = (dataRow["BeKw"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BeKw"]);
			BeYear = (dataRow["BeYear"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BeYear"]);
			LagerId = (dataRow["LagerId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LagerId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			SupplierAddressNr = (dataRow["SupplierAddressNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SupplierAddressNr"]);
			SyncId = (dataRow["SyncId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SyncId"]);
		}

		public PrsSupplierOrderEntity ShallowClone()
		{
			return new PrsSupplierOrderEntity
			{
				BeArticleAmount = BeArticleAmount,
				BeArticleCount = BeArticleCount,
				BeCount = BeCount,
				BeKw = BeKw,
				BeYear = BeYear,
				LagerId = LagerId,
				Id = Id,
				SupplierAddressNr = SupplierAddressNr,
				SyncId = SyncId
			};
		}
	}
	public class PrsSupplierOrderRecivedEntity
	{
		public decimal? BeArticleAmount { get; set; }
		public long? BeArticleCount { get; set; }
		public long? BeCount { get; set; }
		public int? BeKw { get; set; }
		public int? BeYear { get; set; }
		// -
		public decimal? WeArticleAmount { get; set; }
		public long? WeArticleCount { get; set; }
		public long? WeCount { get; set; }
		public int? WeKw { get; set; }
		public int? WeYear { get; set; }
		// -
		public int? LagerId { get; set; }
		public int Id { get; set; }
		public int? SupplierAddressNr { get; set; }
		public int? SyncId { get; set; }

		public PrsSupplierOrderRecivedEntity() { }

		public PrsSupplierOrderRecivedEntity(DataRow dataRow)
		{
			BeArticleAmount = (dataRow["BeArticleAmount"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["BeArticleAmount"]);
			BeArticleCount = (dataRow["BeArticleCount"] == System.DBNull.Value) ? (long?)null : Convert.ToInt64(dataRow["BeArticleCount"]);
			BeCount = (dataRow["BeCount"] == System.DBNull.Value) ? (long?)null : Convert.ToInt64(dataRow["BeCount"]);
			BeKw = (dataRow["BeKw"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BeKw"]);
			BeYear = (dataRow["BeYear"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BeYear"]);
			// -
			WeArticleAmount = (dataRow["WeArticleAmount"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["WeArticleAmount"]);
			WeArticleCount = (dataRow["WeArticleCount"] == System.DBNull.Value) ? (long?)null : Convert.ToInt32(dataRow["WeArticleCount"]);
			WeCount = (dataRow["WeCount"] == System.DBNull.Value) ? (long?)null : Convert.ToInt64(dataRow["WeCount"]);
			WeKw = (dataRow["WeKw"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["WeKw"]);
			WeYear = (dataRow["WeYear"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["WeYear"]);
			// -
			LagerId = (dataRow["LagerId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LagerId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			SupplierAddressNr = (dataRow["SupplierAddressNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SupplierAddressNr"]);
			SyncId = (dataRow["SyncId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SyncId"]);
		}

		public PrsSupplierOrderRecivedEntity ShallowClone()
		{
			return new PrsSupplierOrderRecivedEntity
			{
				BeArticleAmount = BeArticleAmount,
				BeArticleCount = BeArticleCount,
				BeCount = BeCount,
				BeKw = BeKw,
				BeYear = BeYear,
				LagerId = LagerId,
				Id = Id,
				SupplierAddressNr = SupplierAddressNr,
				SyncId = SyncId
			};
		}
	}
}

