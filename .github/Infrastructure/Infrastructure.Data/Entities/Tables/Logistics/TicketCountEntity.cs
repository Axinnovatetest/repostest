namespace Infrastructure.Data.Entities.Tables.Logistics
{
	public class TicketCountEntity
	{
		public int? ticketscount { get; set; }
		public DateTime? CreationDate { get; set; }
		public DateTime? TicektCountTime { get; set; }
		//public int? UserId { get; set; }
		public int? lagerID { get; set; }
		//public string? Username { get; set; }
		public string? Artikelnummer { get; set; }
		public TicketCountEntity(DataRow dataRow)
		{
			ticketscount = (dataRow["ticketcount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ticketcount"]);
			//CreationDate = (dataRow["CreationDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationDate"]);
			//TicektCountTime = (dataRow["TicektCountTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["TicektCountTime"]);
			//UserId = (dataRow["UserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserId"]);
			lagerID = (dataRow["lagerID"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["lagerID"]);
			//Username = (dataRow["Username"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Username"]);
			//Artikelnummer = (dataRow["artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["artikelnummer"]);
		}
	}
}


