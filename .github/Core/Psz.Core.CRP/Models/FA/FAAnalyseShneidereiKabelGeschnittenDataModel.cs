namespace Psz.Core.CRP.Models.FA
{
	public class FAAnalyseShneidereiKabelGeschnittenDataModel
	{
		public int Fertigungsnummer { get; set; }
		public bool Gewerk_1_isDisabled { get; set; }
		public bool Gewerk_2_isDisabled { get; set; }
		public bool Gewerk_3_isDisabled { get; set; }
		public bool Gewerk_1T_isDisabled { get; set; }
		public bool Gewerk_2T_isDisabled { get; set; }
		public bool Gewerk_3T_isDisabled { get; set; }
		public bool Kabel_geschnitten_isDisabled { get; set; }
		public bool FA_Begonnen_isDisabled { get; set; }
		public string Bemerkung { get; set; }
		//checks
		public bool Gewerk_1_check { get; set; }
		public bool Gewerk_2_check { get; set; }
		public bool Gewerk_3_check { get; set; }
		public bool Gewerk_1T_check { get; set; }
		public bool Gewerk_2T_check { get; set; }
		public bool Gewerk_3T_check { get; set; }
		public bool Kabel_geschnitten_check { get; set; }
		public bool FA_Begonnen_check { get; set; }

		public int? Lager { get; set; }


		public FAAnalyseShneidereiKabelGeschnittenDataModel()
		{

		}
		public FAAnalyseShneidereiKabelGeschnittenDataModel(Infrastructure.Data.Entities.Tables.PRS.FertigungEntity faEntity)
		{
			if(faEntity == null)
				return;

			Fertigungsnummer = faEntity.Fertigungsnummer ?? -1;
			Bemerkung = faEntity.Gewerk_Teilweise_Bemerkung;
			FA_Begonnen_isDisabled = false;
			FA_Begonnen_check = (faEntity.Check_FAbegonnen.HasValue && faEntity.Check_FAbegonnen.Value) ? true : false;

			Kabel_geschnitten_isDisabled = false;//(faEntity.Kabel_geschnitten.HasValue && faEntity.Kabel_geschnitten.Value == false) ? false : true;

			Kabel_geschnitten_check = (faEntity.Check_Kabelgeschnitten.HasValue && faEntity.Check_Kabelgeschnitten.Value) ? true : false;

			Gewerk_1_isDisabled = (faEntity.Gewerk_1.ToLower() == "no") ? true : false;
			Gewerk_1_check = (faEntity.Check_Gewerk1.HasValue && faEntity.Check_Gewerk1.Value) ? true : false;
			Gewerk_1T_isDisabled = (faEntity.Gewerk_1.ToLower() == "no") ? true : false;
			Gewerk_1T_check = (faEntity.Check_Gewerk1_Teilweise.HasValue && faEntity.Check_Gewerk1_Teilweise.Value) ? true : false;

			Gewerk_2_isDisabled = (faEntity.Gewerk_2.ToLower() == "no") ? true : false;
			Gewerk_2_check = (faEntity.Check_Gewerk2.HasValue && faEntity.Check_Gewerk2.Value) ? true : false;
			Gewerk_2T_isDisabled = (faEntity.Gewerk_2.ToLower() == "no") ? true : false;
			Gewerk_2T_check = (faEntity.Check_Gewerk2_Teilweise.HasValue && faEntity.Check_Gewerk2_Teilweise.Value) ? true : false;

			Gewerk_3_isDisabled = (faEntity.Gewerk_3.ToLower() == "no") ? true : false;
			Gewerk_3_check = (faEntity.Check_Gewerk3.HasValue && faEntity.Check_Gewerk3.Value) ? true : false;
			Gewerk_3T_isDisabled = (faEntity.Gewerk_3.ToLower() == "no") ? true : false;
			Gewerk_3T_check = (faEntity.Check_Gewerk3_Teilweise.HasValue && faEntity.Check_Gewerk3_Teilweise.Value) ? true : false;


		}
	}
}
