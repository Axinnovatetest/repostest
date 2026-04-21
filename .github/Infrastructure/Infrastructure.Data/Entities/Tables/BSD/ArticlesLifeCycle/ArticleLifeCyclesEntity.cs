using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.BSD
{
    public class ArticleLifeCyclesEntity
    {
		public int? ArticleId { get; set; }
		public DateTime? CreateTime { get; set; }
		public int? CreateUserId { get; set; }
		public string CreateUserName { get; set; }
		public int Id { get; set; }
		public int? PhaseId { get; set; }
		public string PhaseName { get; set; }
		public int? PhaseOrderInCycle { get; set; }
		public DateTime? UpdateTime { get; set; }
		public int? UpdateUserId { get; set; }
		public string UpdateUserName { get; set; }

        public ArticleLifeCyclesEntity() { }

        public ArticleLifeCyclesEntity(DataRow dataRow)
        {
			ArticleId = (dataRow["ArticleId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArticleId"]);
			CreateTime = (dataRow["CreateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreateTime"]);
			CreateUserId = (dataRow["CreateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreateUserId"]);
			CreateUserName = (dataRow["CreateUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CreateUserName"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			PhaseId = (dataRow["PhaseId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PhaseId"]);
			PhaseName = (dataRow["PhaseName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PhaseName"]);
			PhaseOrderInCycle = (dataRow["PhaseOrderInCycle"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PhaseOrderInCycle"]);
			UpdateTime = (dataRow["UpdateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["UpdateTime"]);
			UpdateUserId = (dataRow["UpdateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UpdateUserId"]);
			UpdateUserName = (dataRow["UpdateUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UpdateUserName"]);
        }
    
        public ArticleLifeCyclesEntity ShallowClone()
        {
            return new ArticleLifeCyclesEntity
            {
			ArticleId = ArticleId,
			CreateTime = CreateTime,
			CreateUserId = CreateUserId,
			CreateUserName = CreateUserName,
			Id = Id,
			PhaseId = PhaseId,
			PhaseName = PhaseName,
			PhaseOrderInCycle = PhaseOrderInCycle,
			UpdateTime = UpdateTime,
			UpdateUserId = UpdateUserId,
			UpdateUserName = UpdateUserName
            };
        }
    }
}

