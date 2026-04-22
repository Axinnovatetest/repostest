using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class UnitOfMeasureEntity
	{
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public string CreationUserName { get; set; }
		public string Description { get; set; }
		public int Id { get; set; }
		public int? LastEditUserId { get; set; }
		public string LastEditUserName { get; set; }
		public string Name { get; set; }
		public string Symbol { get; set; }

		public UnitOfMeasureEntity() { }

		public UnitOfMeasureEntity(DataRow dataRow)
		{
			CreationTime = (dataRow["CreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = (dataRow["CreationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreationUserId"]);
			CreationUserName = (dataRow["CreationUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CreationUserName"]);
			Description = (dataRow["Description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LastEditUserId = (dataRow["LastEditUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastEditUserId"]);
			LastEditUserName = (dataRow["LastEditUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LastEditUserName"]);
			Name = (dataRow["Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name"]);
			Symbol = (dataRow["Symbol"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Symbol"]);
		}

		public UnitOfMeasureEntity ShallowClone()
		{
			return new UnitOfMeasureEntity
			{
				CreationTime = CreationTime,
				CreationUserId = CreationUserId,
				CreationUserName = CreationUserName,
				Description = Description,
				Id = Id,
				LastEditUserId = LastEditUserId,
				LastEditUserName = LastEditUserName,
				Name = Name,
				Symbol = Symbol
			};
		}
	}
}

