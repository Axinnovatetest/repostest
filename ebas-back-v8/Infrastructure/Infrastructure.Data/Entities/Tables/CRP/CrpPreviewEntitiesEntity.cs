using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.CRP
{
    public class CrpPreviewEntitiesEntity
    {
		public int? ArticleId { get; set; }
		public int? EntityId { get; set; }
		public string EntityNumber { get; set; }
		public string EntityType { get; set; }
		public int Id { get; set; }
		public int? Week { get; set; }
		public int? Year { get; set; }

        public CrpPreviewEntitiesEntity() { }

        public CrpPreviewEntitiesEntity(DataRow dataRow)
        {
			ArticleId = (dataRow["ArticleId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArticleId"]);
			EntityId = (dataRow["EntityId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["EntityId"]);
			EntityNumber = (dataRow["EntityNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EntityNumber"]);
			EntityType = (dataRow["EntityType"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EntityType"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Week = (dataRow["Week"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Week"]);
			Year = (dataRow["Year"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Year"]);
        }
    
        public CrpPreviewEntitiesEntity ShallowClone()
        {
            return new CrpPreviewEntitiesEntity
            {
			ArticleId = ArticleId,
			EntityId = EntityId,
			EntityNumber = EntityNumber,
			EntityType = EntityType,
			Id = Id,
			Week = Week,
			Year = Year
            };
        }
    }
}

