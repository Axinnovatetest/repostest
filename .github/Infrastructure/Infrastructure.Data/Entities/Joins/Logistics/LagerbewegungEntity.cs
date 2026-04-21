using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class LagerbewegungEntity
	{
		public int LagerFrom { get; set; }
		public int LagerTo { get; set; }
		public DateTime? TransferDate { get; set; }
		public int SiteFromId { get; set; }
		public string SiteFromName { get; set; }
		public int SiteToId { get; set; }
		public string SiteToName { get; set; }
		public LagerbewegungEntity()
		{

		}
		public LagerbewegungEntity(DataRow dr)
		{
			LagerFrom = (dr["LagerFrom"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["LagerFrom"]);
			LagerTo = (dr["LagerTo"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["LagerTo"]);
			TransferDate = (dr["TransferDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["TransferDate"]);
			SiteFromId = (dr["SiteFromId"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["SiteFromId"]);
			SiteFromName = (dr["SiteFromName"] == System.DBNull.Value) ? null : Convert.ToString(dr["SiteFromName"]);
			SiteToId = (dr["SiteToId"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dr["SiteToId"]);
			SiteToName = (dr["SiteToName"] == System.DBNull.Value) ? null : Convert.ToString(dr["SiteToName"]);
		}
	}
}
