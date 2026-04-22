using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.BSD
{

	public class __PSZERP_DATA_TEMP_FILEEntity
	{
		public DateTime? CreationDate { get; set; }
		public int? CreationUserId { get; set; }
		public int Id { get; set; }
		public int? LastModifiedBy { get; set; }
		public DateTime? LastModifiedDate { get; set; }
		public string TempFileName { get; set; }

		public __PSZERP_DATA_TEMP_FILEEntity() { }

		public __PSZERP_DATA_TEMP_FILEEntity(DataRow dataRow)
		{
			CreationDate = (dataRow["CreationDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationDate"]);
			CreationUserId = (dataRow["CreationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreationUserId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LastModifiedBy = (dataRow["LastModifiedBy"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastModifiedBy"]);
			LastModifiedDate = (dataRow["LastModifiedDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastModifiedDate"]);
			TempFileName = (dataRow["TempFileName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["TempFileName"]);
		}

		public __PSZERP_DATA_TEMP_FILEEntity ShallowClone()
		{
			return new __PSZERP_DATA_TEMP_FILEEntity
			{
				CreationDate = CreationDate,
				CreationUserId = CreationUserId,
				Id = Id,
				LastModifiedBy = LastModifiedBy,
				LastModifiedDate = LastModifiedDate,
				TempFileName = TempFileName
			};
		}
	}



}
