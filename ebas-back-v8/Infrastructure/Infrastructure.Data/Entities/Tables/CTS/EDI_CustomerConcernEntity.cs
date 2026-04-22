using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class EDI_CustomerConcernEntity
	{
		public string ConcernName { get; set; }
		public int? ConcernNumber { get; set; }
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public string CreationUserName { get; set; }
		public int Id { get; set; }
		public bool? IncludeDescription { get; set; }
		public int? LastEditUserId { get; set; }
		public string LastEditUserName { get; set; }
		public bool? TrimLeadingZeros { get; set; }

		public EDI_CustomerConcernEntity() { }

		public EDI_CustomerConcernEntity(DataRow dataRow)
		{
			ConcernName = (dataRow["ConcernName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ConcernName"]);
			ConcernNumber = (dataRow["ConcernNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ConcernNumber"]);
			CreationTime = (dataRow["CreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = (dataRow["CreationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreationUserId"]);
			CreationUserName = (dataRow["CreationUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CreationUserName"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IncludeDescription = (dataRow["IncludeDescription"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IncludeDescription"]);
			LastEditUserId = (dataRow["LastEditUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastEditUserId"]);
			LastEditUserName = (dataRow["LastEditUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LastEditUserName"]);
			TrimLeadingZeros = (dataRow["TrimLeadingZeros"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["TrimLeadingZeros"]);
		}

		public EDI_CustomerConcernEntity ShallowClone()
		{
			return new EDI_CustomerConcernEntity
			{
				ConcernName = ConcernName,
				ConcernNumber = ConcernNumber,
				CreationTime = CreationTime,
				CreationUserId = CreationUserId,
				CreationUserName = CreationUserName,
				Id = Id,
				IncludeDescription = IncludeDescription,
				LastEditUserId = LastEditUserId,
				LastEditUserName = LastEditUserName,
				TrimLeadingZeros = TrimLeadingZeros
			};
		}
	}
}

