

namespace Infrastructure.Data.Entities.Tables.CRP
{
	public class __crp_FertigungsnummerEntity
	{
		public int Id { get; set; }
		public int? Fertigungsnummer { get; set; }
		public int? angebotNr { get; set; }
		public string User { get; set; }
		public __crp_FertigungsnummerEntity() { }
		public __crp_FertigungsnummerEntity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			angebotNr = (dataRow["angebotNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["angebotNr"]);
			User = (dataRow["User"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["User"]);
		}
		public __crp_FertigungsnummerEntity ShallowClone()
		{
			return new __crp_FertigungsnummerEntity
			{
				Id = Id,
				Fertigungsnummer = Fertigungsnummer,
				angebotNr = angebotNr,
				User = User,
			};
		}
	}
}
