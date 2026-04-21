using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.MTM.Order
{
	public class DashboardEntity: Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity
	{
		public int RahmenStatus { get; set; }
		public DashboardEntity()
		{
		}
		public DashboardEntity(DataRow dataRow)
			: base(dataRow)
		{
			RahmenStatus = Convert.ToInt32(dataRow["RahmenStatus"]);
		}
	}
}
