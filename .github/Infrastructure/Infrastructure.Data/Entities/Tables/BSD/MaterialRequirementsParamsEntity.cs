using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class MaterialRequirementsParamsEntity
	{
		public int SyncId { get; set; }
		public DateTime? SyncDate { get; set; }

		public MaterialRequirementsParamsEntity() { }

		public MaterialRequirementsParamsEntity(DataRow dataRow)
		{
			SyncId = Convert.ToInt32(dataRow["SyncId"]);
			SyncDate = (dataRow["SyncDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["SyncDate"]);
		}
	}
}
