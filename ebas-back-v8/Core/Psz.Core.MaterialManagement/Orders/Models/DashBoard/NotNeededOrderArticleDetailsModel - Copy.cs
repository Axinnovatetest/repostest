namespace Psz.Core.MaterialManagement.Orders.Models.DashBoard
{
	public class NotNeededOrderDetailsAllResponseModel
	{
		public string Lieferant { get; set; }
		public DateTime? Bestatigter_Termin { get; set; }
		public DateTime? Wunschtermin { get; set; }
		public string BestellungNr { get; set; }
		public decimal Anzahl { get; set; }
		public decimal Total { get; set; }
		public NotNeededOrderDetailsAllResponseModel() { }
		public NotNeededOrderDetailsAllResponseModel(Infrastructure.Data.Entities.Joins.MTM.Order.NotNeededOrderDetailsAllEntity notNeededOrdersEntity)
		{
			Lieferant = notNeededOrdersEntity.Lieferant;
			Bestatigter_Termin = notNeededOrdersEntity.Bestatigter_Termin;
			Wunschtermin = notNeededOrdersEntity.Wunschtermin;
			BestellungNr = notNeededOrdersEntity.BestellungNr;
			Anzahl = notNeededOrdersEntity.Anzahl;
			Total = notNeededOrdersEntity.Total;
		}

	}
	public class NotNeededOrderDetailsAllRequestModel: IPaginatedRequestModel
	{
		public int AreaLager { get; set; }
		public int Week { get; set; }
		public int Year { get; set; }
	}
}
