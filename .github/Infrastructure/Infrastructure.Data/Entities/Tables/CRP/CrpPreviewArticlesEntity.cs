using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.CRP
{
    public class CrpPreviewArticlesEntity
    {
		public int ArticleId { get; set; }
		public string ArticleNumber { get; set; }
		public string Designation { get; set; }
		public string ExternalStatus { get; set; }
		public int Id { get; set; }
		public decimal MinimumStock { get; set; }
		public decimal Stock { get; set; }
		public decimal SumNeeds { get; set; }
		public decimal SumNeedsAB { get; set; }
		public decimal SumNeedsFC { get; set; }
		public decimal SumNeedsLP { get; set; }
		public decimal SumProds { get; set; }
		public DateTime? SyncDate { get; set; }
		public int? SyncId { get; set; }

        public CrpPreviewArticlesEntity() { }

        public CrpPreviewArticlesEntity(DataRow dataRow)
        {
			ArticleId = (dataRow["ArticleId"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["ArticleId"]);
			ArticleNumber = (dataRow["ArticleNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleNumber"]);
			Designation = (dataRow["Designation"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Designation"]);
			ExternalStatus = (dataRow["ExternalStatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ExternalStatus"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			MinimumStock = (dataRow["MinimumStock"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["MinimumStock"]);
			Stock = (dataRow["Stock"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Stock"]);
			SumNeeds = (dataRow["SumNeeds"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["SumNeeds"]);
			SumNeedsAB = (dataRow["SumNeedsAB"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["SumNeedsAB"]);
			SumNeedsFC = (dataRow["SumNeedsFC"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["SumNeedsFC"]);
			SumNeedsLP = (dataRow["SumNeedsLP"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["SumNeedsLP"]);
			SumProds = (dataRow["SumProds"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["SumProds"]);
			SyncDate = (dataRow["SyncDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["SyncDate"]);
			SyncId = (dataRow["SyncId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SyncId"]);
        }
    
        public CrpPreviewArticlesEntity ShallowClone()
        {
            return new CrpPreviewArticlesEntity
            {
			ArticleId = ArticleId,
			ArticleNumber = ArticleNumber,
			Designation = Designation,
			ExternalStatus = ExternalStatus,
			Id = Id,
			MinimumStock = MinimumStock,
			Stock = Stock,
			SumNeeds = SumNeeds,
			SumNeedsAB = SumNeedsAB,
			SumNeedsFC = SumNeedsFC,
			SumNeedsLP = SumNeedsLP,
			SumProds = SumProds,
			SyncDate = SyncDate,
			SyncId = SyncId
            };
        }
    }
}

