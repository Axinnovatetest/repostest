using System;

namespace Infrastructure.Data.Entities.Joins.MTM.Order
{
	public class GetFaultyFasEntity
	{
		public int TotalCount { get; set; }
		public int Fertigungsnummer { get; set; }
		public int ID { get; set; }
		public DateTime? Termin_Bestätigt { get; set; }
		public decimal? Quantity { get; set; }
		public GetFaultyFasEntity()
		{

		}
		public GetFaultyFasEntity(System.Data.DataRow datarow)
		{
			TotalCount = (datarow["TotalCount"] == System.DBNull.Value) ? -1 : Convert.ToInt32(datarow["TotalCount"]);
			ID = (datarow["ID"] == System.DBNull.Value) ? -1 : Convert.ToInt32(datarow["ID"]);
			Fertigungsnummer = (datarow["Fertigungsnummer"] == System.DBNull.Value) ? -1 : Convert.ToInt32(datarow["Fertigungsnummer"]);
			Termin_Bestätigt = (datarow["Termin_Bestätigt1"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(datarow["Termin_Bestätigt1"]);
		}
	}
}
