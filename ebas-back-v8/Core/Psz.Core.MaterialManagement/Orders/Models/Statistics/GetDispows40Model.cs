

namespace Psz.Core.MaterialManagement.Orders.Models.Statistics
{
	public class GetDispows40ResponseModel
	{
		public string Name1 { get; set; } //
		public string Stücklisten_Artikelnummer { get; set; }//
		public double SummevonBruttobedarf { get; set; } //
		public DateTime? MaxvonTermin_Materialbedarf { get; set; }
		public double Bestand { get; set; }//
		public double Differenz { get; set; } //
		public double Mindestbestellmenge { get; set; } //
		public string Lagerort { get; set; }//
		public int Lagerort_id { get; set; } //
		public int TotalCount { get; set; } //
		public string Rahmen_Nr { get; set; } //
		public int? Rahmenmenge { get; set; }
		public Boolean obsolet { get; set; }
		public GetDispows40ResponseModel(Infrastructure.Data.Entities.Joins.MTM.Order.Statistics.Dispows40Entity data)
		{

			Name1 = data.Name1 ?? string.Empty;
			Stücklisten_Artikelnummer = data.Stücklisten_Artikelnummer ?? string.Empty;
			SummevonBruttobedarf = data.SummevonBruttobedarf;
			MaxvonTermin_Materialbedarf = data.MaxvonTermin_Materialbedarf;
			Bestand = data.Bestand;
			Differenz = data.Differenz;
			Mindestbestellmenge = data.Mindestbestellmenge;
			Lagerort = data.Lagerort ?? string.Empty;
			Rahmen_Nr = data.Rahmen_Nr ?? string.Empty;
			Rahmenmenge = data.Rahmenmenge;
			obsolet = data.obsolet;
			TotalCount = data.TotalCount;
			obsolet = data.obsolet;
		}

	}
	public class GetDispows40RequestModel: IPaginatedRequestModel
	{

	}

}
