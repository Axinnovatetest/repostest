using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.MGO
{
	public class CTSDashboardSummaryEntity
	{
		public decimal? ImmediatAmount { get; set; }
		public decimal? TotalAmount { get; set; }
		public decimal? ProductionAmount { get; set; }

		public CTSDashboardSummaryEntity() { }

		public CTSDashboardSummaryEntity(DataRow dataRow)
		{
			ImmediatAmount = (dataRow["ImmediatAmount"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ImmediatAmount"]);
			TotalAmount = (dataRow["TotalAmount"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["TotalAmount"]);
			ProductionAmount = (dataRow["ProductionAmount"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ProductionAmount"]);
		}

		public CTSDashboardSummaryEntity ShallowClone()
		{
			return new CTSDashboardSummaryEntity
			{
				ImmediatAmount = ImmediatAmount,
				TotalAmount = TotalAmount,
				ProductionAmount = ProductionAmount
			};
		}
	}
}
