using Psz.Core.Common.Models;


namespace Psz.Core.CRP.Models.FAPlanning
{
	public class FaultyFAModel
	{
		public int FertigungID { get; set; }
		public int? Fertigungsauftrag { get; set; }
		public string BemerkungExtern { get; set; }
		public DateTime? AktuellerTermin { get; set; }
		public DateTime? Druckdatum { get; set; }
		public string ManBemerkungPlannung { get; set; }
		public FaultyFAModel()
		{

		}
		public FaultyFAModel(Infrastructure.Data.Entities.Tables.PRS.FertigungEntity entity)
		{
			FertigungID = entity.ID;
			Fertigungsauftrag = entity.Fertigungsnummer;
			BemerkungExtern = entity.Bemerkung;
			AktuellerTermin = entity.Termin_Bestatigt1;
			Druckdatum = entity.FA_Druckdatum;
			ManBemerkungPlannung = entity.Bemerkung_Planung;
		}
	}
	public class FaultyFARequestModel: IPaginatedRequestModel
	{
		public string SearchTerms { get; set; } = "";
		public bool? Ubg { get; set; } = false;
	}
	public class FaultyFAReponseModel: IPaginatedResponseModel<FaultyFAModel> { }
}
