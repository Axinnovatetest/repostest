using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.BSD
{
    public class ArticleLifeCyclePhasesEntity
    {
		public DateTime? CreateTime { get; set; }
		public int? CreateUserId { get; set; }
		public string CreateUserName { get; set; }
		public int Id { get; set; }
		public string PhaseDescription { get; set; }
		public string PhaseName { get; set; }
		public DateTime? UpdateTime { get; set; }
		public int? UpdateUserId { get; set; }
		public string UpdateUserName { get; set; }

        public ArticleLifeCyclePhasesEntity() { }

        public ArticleLifeCyclePhasesEntity(DataRow dataRow)
        {
			CreateTime = (dataRow["CreateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreateTime"]);
			CreateUserId = (dataRow["CreateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreateUserId"]);
			CreateUserName = (dataRow["CreateUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CreateUserName"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			PhaseDescription = (dataRow["PhaseDescription"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PhaseDescription"]);
			PhaseName = (dataRow["PhaseName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PhaseName"]);
			UpdateTime = (dataRow["UpdateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["UpdateTime"]);
			UpdateUserId = (dataRow["UpdateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UpdateUserId"]);
			UpdateUserName = (dataRow["UpdateUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UpdateUserName"]);
        }
    
        public ArticleLifeCyclePhasesEntity ShallowClone()
        {
            return new ArticleLifeCyclePhasesEntity
            {
			CreateTime = CreateTime,
			CreateUserId = CreateUserId,
			CreateUserName = CreateUserName,
			Id = Id,
			PhaseDescription = PhaseDescription,
			PhaseName = PhaseName,
			UpdateTime = UpdateTime,
			UpdateUserId = UpdateUserId,
			UpdateUserName = UpdateUserName
            };
        }
    }
}

