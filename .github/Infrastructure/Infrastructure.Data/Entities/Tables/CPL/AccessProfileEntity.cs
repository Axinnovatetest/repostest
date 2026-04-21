using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.CPL
{
	public class AccessProfileEntity
	{
		public string AccessProfileName { get; set; }
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public int Id { get; set; }
		public bool? ModuleActivated { get; set; }
		public bool? RequestAdmin { get; set; }
		public bool? RequestCapital { get; set; }
		public bool? RequestCreation { get; set; }
		public bool? RequestEngeneering { get; set; }

		public AccessProfileEntity() { }

		public AccessProfileEntity(DataRow dataRow)
		{
			AccessProfileName = (dataRow["AccessProfileName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AccessProfileName"]);
			CreationTime = (dataRow["CreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = (dataRow["CreationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreationUserId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			ModuleActivated = (dataRow["ModuleActivated"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ModuleActivated"]);
			RequestAdmin = (dataRow["RequestAdmin"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RequestAdmin"]);
			RequestCapital = (dataRow["RequestCapital"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RequestCapital"]);
			RequestCreation = (dataRow["RequestCreation"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RequestCreation"]);
			RequestEngeneering = (dataRow["RequestEngeneering"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["RequestEngeneering"]);
		}

		public AccessProfileEntity ShallowClone()
		{
			return new AccessProfileEntity
			{
				AccessProfileName = AccessProfileName,
				CreationTime = CreationTime,
				CreationUserId = CreationUserId,
				Id = Id,
				ModuleActivated = ModuleActivated,
				RequestAdmin = RequestAdmin,
				RequestCapital = RequestCapital,
				RequestCreation = RequestCreation,
				RequestEngeneering = RequestEngeneering
			};
		}
	}
}

