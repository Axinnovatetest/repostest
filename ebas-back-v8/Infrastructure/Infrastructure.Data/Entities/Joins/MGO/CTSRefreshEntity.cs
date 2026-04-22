using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.MGO
{
	public class CTSRefreshEntity
	{
		public DateTime? RefreshDate { get; set; }

		public CTSRefreshEntity() { }

		public CTSRefreshEntity(DataRow dataRow)
		{
			RefreshDate = (dataRow["RefreshDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["RefreshDate"]);
		}

		public CTSRefreshEntity ShallowClone()
		{
			return new CTSRefreshEntity
			{
				RefreshDate = RefreshDate
			};
		}
	}
}
