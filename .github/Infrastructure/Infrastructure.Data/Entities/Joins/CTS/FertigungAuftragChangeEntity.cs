namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class FertigungAuftragChangeEntity
	{
		public int? ID { get; set; }
		public int? DiffInDays { get; set; }
		public DateTime? Änderungsdatum { get; set; }
		public int? Fertigungsnummer { get; set; }
		public string Artikelnummer { get; set; }
		public string Bemerkung { get; set; }
		public string Bezeichnung { get; set; }
		public string CS_Mitarbeiter { get; set; }
		public bool? Erstmuster { get; set; }
		public int? FA_Menge { get; set; }
		public string Grund_CS { get; set; }
		public string Mitarbeiter { get; set; }
		public DateTime? Termin_Bestätigt1 { get; set; }
		public DateTime? Termin_voränderung { get; set; }
		public DateTime? Termin_Wunsch { get; set; }
		public bool? Wunsch_CS { get; set; }
		public int? ArticleNr { get; set; }
		public string? Lager { get; set; }
		public string FaStatus { get; set; }
		public int? FaPositionZone { get; set; }
		public decimal? HoursLeft { get; set; }

		public FertigungAuftragChangeEntity(DataRow dataRow)
		{
			Änderungsdatum = (dataRow["Änderungsdatum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Änderungsdatum"]);
			DiffInDays = (dataRow["DiffInDays"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DiffInDays"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bemerkung = (dataRow["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung"]);
			Bezeichnung = (dataRow["Bezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung"]);
			CS_Mitarbeiter = (dataRow["CS_Mitarbeiter"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CS_Mitarbeiter"]);
			Erstmuster = (dataRow["Erstmuster"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Erstmuster"]);
			FA_Menge = (dataRow["FA_Menge"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FA_Menge"]);
			FaStatus = (dataRow["FaStatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FaStatus"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Grund_CS = (dataRow["Grund_CS"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Grund_CS"]);
			ID = (dataRow["Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id"]);
			Lager = (dataRow["Lager"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lager"]);
			ArticleNr = (dataRow["ArticleNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArticleNr"]);
			FaPositionZone = (dataRow["FaPositionZone"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FaPositionZone"]);
			Mitarbeiter = (dataRow["Mitarbeiter"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mitarbeiter"]);
			Termin_Bestätigt1 = (dataRow["Termin_Bestätigt1"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Bestätigt1"]);
			Termin_voränderung = (dataRow["Termin_voränderung"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_voränderung"]);
			Termin_Wunsch = (dataRow["Termin_Wunsch"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Wunsch"]);
			Wunsch_CS = (dataRow["Wunsch_CS"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Wunsch_CS"]);
			HoursLeft = (dataRow["HoursLeft"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["HoursLeft"]);
		}

	}
}
