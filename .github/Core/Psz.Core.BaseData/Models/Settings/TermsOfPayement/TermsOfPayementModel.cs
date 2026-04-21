namespace Psz.Core.BaseData.Models.TermsOfPayement
{
	public class TermsOfPayementModel
	{
		public string Description { get; set; }
		public bool? Calculate_amount { get; set; }
		public int Id { get; set; }
		public double? Estate_1 { get; set; }
		public double? Estate_2 { get; set; }
		public double? Estate_3 { get; set; }
		public int? Days1 { get; set; }
		public int? Days2 { get; set; }
		public int? Days3 { get; set; }
		public string Text11 { get; set; }
		public string Text12 { get; set; }
		public string Text21 { get; set; }
		public string Text22 { get; set; }
		public string Text31 { get; set; }
		public string Text32 { get; set; }

		public TermsOfPayementModel()
		{

		}

		public TermsOfPayementModel(Infrastructure.Data.Entities.Tables.BSD.ZahlungskonditionenEntity paymentPracticeEntiy)
		{
			Description = paymentPracticeEntiy.Bemerkungen;
			Calculate_amount = paymentPracticeEntiy.Betrag_berechnen;
			Id = paymentPracticeEntiy.ID;
			Estate_1 = paymentPracticeEntiy.Nachlaß1;
			Estate_2 = paymentPracticeEntiy.Nachlaß2;
			Estate_3 = paymentPracticeEntiy.Nachlaß3;
			Days1 = paymentPracticeEntiy.Tage1;
			Days2 = paymentPracticeEntiy.Tage2;
			Days3 = paymentPracticeEntiy.Tage3;
			Text11 = paymentPracticeEntiy.Text11;
			Text12 = paymentPracticeEntiy.Text12;
			Text21 = paymentPracticeEntiy.Text21;
			Text22 = paymentPracticeEntiy.Text22;
			Text31 = paymentPracticeEntiy.Text31;
			Text32 = paymentPracticeEntiy.Text32;
		}

		public Infrastructure.Data.Entities.Tables.BSD.ZahlungskonditionenEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.ZahlungskonditionenEntity
			{
				Bemerkungen = Description,
				Betrag_berechnen = Calculate_amount,
				ID = Id,
				Nachlaß1 = Estate_1,
				Nachlaß2 = Estate_2,
				Nachlaß3 = Estate_3,
				Tage1 = Days1,
				Tage2 = Days2,
				Tage3 = Days3,
				Text11 = Text11,
				Text12 = Text12,
				Text21 = Text21,
				Text22 = Text22,
				Text31 = Text31,
				Text32 = Text32,
			};
		}
	}
}
