using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.CRP
{
    public class CrpPreviewLpEntity
    {
		public int? ArticleId { get; set; }
		public int Id { get; set; }
		public decimal? Quantity { get; set; }
		public int? Week { get; set; }
		public int? Year { get; set; }

        public CrpPreviewLpEntity() { }

        public CrpPreviewLpEntity(DataRow dataRow)
        {
			ArticleId = (dataRow["ArticleId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArticleId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Quantity = (dataRow["Quantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Quantity"]);
			Week = (dataRow["Week"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Week"]);
			Year = (dataRow["Year"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Year"]);
        }
    
        public CrpPreviewLpEntity ShallowClone()
        {
            return new CrpPreviewLpEntity
            {
			ArticleId = ArticleId,
			Id = Id,
			Quantity = Quantity,
			Week = Week,
			Year = Year
            };
        }
    }
}

