namespace Psz.Core.MaterialManagement.Orders.Models.Statistics
{
	public class FAVerschiebungModel
	{
		public int ID { get; set; }
		public int? Fertigungsnummer { get; set; }
		public int? Lagerort_id { get; set; }
		public DateTime? Termin_Ursprunglich { get; set; }
		public DateTime? Termin_voranderung { get; set; }
		public DateTime? Termin_Bestatigt1 { get; set; }
		public string Kennzeichen { get; set; }
		public DateTime? Anderungsdatum { get; set; }
		public int? Zeitraum { get; set; }
		public FAVerschiebungModel()
		{

		}
		public FAVerschiebungModel(Infrastructure.Data.Entities.Joins.MTM.Order.Statistics.FAVerschiebungEntity entity)
		{
			var diffrence = !entity.Termin_voranderung.HasValue
					? (entity.Termin_Bestatigt1.Value - entity.Termin_Ursprunglich.Value).Days
					: (entity.Termin_Bestatigt1.Value - entity.Termin_voranderung.Value).Days;
			ID = entity.ID;
			Fertigungsnummer = entity.Fertigungsnummer;
			Lagerort_id = entity.Lagerort_id;
			Termin_Ursprunglich = entity.Termin_Ursprunglich;
			Termin_voranderung = entity.Termin_voranderung;
			Termin_Bestatigt1 = entity.Termin_Bestatigt1;
			Kennzeichen = entity.Kennzeichen;
			Anderungsdatum = entity.Anderungsdatum;
			Zeitraum = diffrence;
		}
	}

	public class FAVerschiebungResponseModel
	{
		public string Lagerort { get; set; }
		public int Period { get; set; }
		public List<FAVerschiebungModel> Data { get; set; }
	}

	public class FAVerschiebungRequestModel
	{
		public int Lager { get; set; }
		public int Period { get; set; }
	}
}
