using Infrastructure.Data.Entities.Joins.CTS;
using Psz.Core.Common.Models;

namespace Psz.Core.CRP.Models.FA
{
	public class FaChangesHistoryResponseModel
	{
		public int ID { get; set; }
		public int? DiffInDays { get; set; }
		public DateTime? Anderungsdatum { get; set; }
		public int? Fertigungsnummer { get; set; }
		public string Artikelnummer { get; set; }
		public string Bemerkung { get; set; }
		public string Bezeichnung { get; set; }
		public string CS_Mitarbeiter { get; set; }
		public bool? Erstmuster { get; set; }
		public int? FA_Menge { get; set; }
		public string Grund_CS { get; set; }
		public string Mitarbeiter { get; set; }
		public DateTime? Termin_Bestatigt1 { get; set; }
		public DateTime? Termin_voranderung { get; set; }
		public DateTime? Termin_Wunsch { get; set; }
		public bool? Wunsch_CS { get; set; }
		public int? ArticleNr { get; set; }
		public string? LagerId { get; set; }
		public string FaStatus { get; set; }
		public int? FaPositionZone { get; set; }
		public decimal HoursLeft { get; set; }

		public FaChangesHistoryResponseModel()
		{

		}
		public FaChangesHistoryResponseModel(FertigungAuftragChangeEntity entity)
		{
			ID = entity.ID ?? 0;
			DiffInDays = entity.DiffInDays;
			Anderungsdatum = entity.Änderungsdatum;
			Artikelnummer = entity.Artikelnummer;
			Bemerkung = entity.Bemerkung;
			Bezeichnung = entity.Bezeichnung;
			CS_Mitarbeiter = entity.CS_Mitarbeiter;
			Erstmuster = entity.Erstmuster;
			FA_Menge = entity.FA_Menge;
			Fertigungsnummer = entity.Fertigungsnummer;
			Grund_CS = entity.Grund_CS;
			Mitarbeiter = entity.Mitarbeiter;
			Termin_Bestatigt1 = entity.Termin_Bestätigt1;
			Termin_voranderung = entity.Termin_voränderung;
			Termin_Wunsch = entity.Termin_Wunsch;
			Wunsch_CS = entity.Wunsch_CS;
			ArticleNr = entity.ArticleNr ?? 0;
			LagerId = entity.Lager ?? "";
			FaStatus = entity.FaStatus.ToLower();
			FaPositionZone = entity.FaPositionZone ?? 0;
			HoursLeft = entity.HoursLeft ?? 0;
		}
	}


	public class GetFaChangesHistoryResponseModel: IPaginatedResponseModel<FaChangesHistoryResponseModel>
	{

	}
}
