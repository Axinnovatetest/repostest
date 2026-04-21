namespace Psz.Core.MaterialManagement.Orders.Models.DashBoard
{
	public class NotNeededOrderArticleDetailsResponseModel
	{
		public int Nr { get; set; }
		public string Lieferant { get; set; }
		public DateTime? Bestatigter_Termin { get; set; }
		public DateTime? Liefertermin { get; set; }
		public string BestellungNr { get; set; }
		public decimal Anzahl { get; set; }
		public decimal Total { get; set; }
		public NotNeededOrderArticleDetailsResponseModel() { }
		public NotNeededOrderArticleDetailsResponseModel(Infrastructure.Data.Entities.Joins.MTM.Order.NotNeededOrderArticleDetailsEntity notNeededOrdersEntity)
		{
			Nr = notNeededOrdersEntity.Nr;
			Lieferant = notNeededOrdersEntity.Lieferant;
			Bestatigter_Termin = notNeededOrdersEntity.Bestatigter_Termin;
			Liefertermin = notNeededOrdersEntity.Liefertermin;
			BestellungNr = notNeededOrdersEntity.BestellungNr;
			Anzahl = notNeededOrdersEntity.Anzahl;
			Total = notNeededOrdersEntity.Total;
		}

	}
	public class NotNeededOrderArticleDetailsRequestModel: IPaginatedRequestModel
	{
		public int AreaLager { get; set; }
		public int ArtikelNr { get; set; }
		public int Week { get; set; }
		public int Year { get; set; }
	}
}
