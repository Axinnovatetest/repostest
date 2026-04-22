using System;


namespace Infrastructure.Data.Entities.Joins.MTM.Order
{
	public class GetFAsInTimeSpanEntity
	{
		public decimal? NeededQuantity { get; set; }
		public DateTime? Termin_Bestätigt1 { get; set; }
		public int Fertigungsnummer { get; set; }
		public int FertigungId { get; set; }
		public GetFAsInTimeSpanEntity(System.Data.DataRow datarow)
		{
			FertigungId = (datarow["ID"] == System.DBNull.Value) ? -1 : Convert.ToInt32(datarow["ID"]);
			Fertigungsnummer = (datarow["Fertigungsnummer"] == System.DBNull.Value) ? -1 : Convert.ToInt32(datarow["Fertigungsnummer"]);
			NeededQuantity = (datarow["NeededQuantity"] == System.DBNull.Value) ? -1 : Convert.ToDecimal(datarow["NeededQuantity"]);
			Termin_Bestätigt1 = (datarow["Termin_Bestätigt1"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(datarow["Termin_Bestätigt1"]);
		}
	}
}
