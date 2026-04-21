using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.CRP
{
	public class HistoryHeaderFGBestandEntity
	{
		public DateTime? CreateDate { get; set; }
		public string CreatedUserName { get; set; }
		public int? CreateUserId { get; set; }
		public int Id { get; set; }
		public DateTime? ImportDate { get; set; }
		public int? ImportType { get; set; }


		public HistoryHeaderFGBestandEntity() { }

		public HistoryHeaderFGBestandEntity(DataRow dataRow)
		{
			CreateDate = (dataRow["CreateDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreateDate"]);
			CreatedUserName = (dataRow["CreatedUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CreatedUserName"]);
			CreateUserId = (dataRow["CreateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreateUserId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			ImportDate = (dataRow["ImportDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ImportDate"]);
			ImportType = (dataRow["ImportType"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ImportType"]);
		}

		public HistoryHeaderFGBestandEntity ShallowClone()
		{
			return new HistoryHeaderFGBestandEntity
			{
				CreateDate = CreateDate,
				CreatedUserName = CreatedUserName,
				CreateUserId = CreateUserId,
				Id = Id,
				ImportDate = ImportDate,
				ImportType = ImportType
			};
		}
	}
}

