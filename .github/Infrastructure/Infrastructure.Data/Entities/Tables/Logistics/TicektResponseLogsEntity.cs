namespace Infrastructure.Data.Entities.Tables.Logistics
{
	public class TicektResponseLogsEntity
	{
		public int? ticketscount { get; set; }
		public DateTime? TicektCountTime { get; set; }
		public string? Artikelnummer { get; set; }
		public TicektResponseLogsEntity(DataRow dataRow)
		{
			ticketscount = (dataRow["ticketscount"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["ticketscount"]);
			TicektCountTime = (dataRow["TicektCountTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["TicektCountTime"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
		}
	}
}
