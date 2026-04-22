namespace Psz.Core.MaterialManagement.Orders.Models.DashBoard
{
	public class NotNeededOrdersArticleRequestModel
	{
		public int Area { get; set; }
		public int ArtikelNr { get; set; }
	}
	public class NotNeededOrdersArticleResponseModel
	{
		public decimal Anzahl { get; set; }
		public int ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public string Week { get; set; }
		public NotNeededOrdersArticleResponseModel() { }
		public NotNeededOrdersArticleResponseModel(Infrastructure.Data.Entities.Joins.MTM.Order.NotNeededOrdersArticleEntity notNeededOrdersEntity)
		{
			Artikelnummer = notNeededOrdersEntity.Artikelnummer;
			ArtikelNr = notNeededOrdersEntity.ArtikelNr;
			Anzahl = notNeededOrdersEntity.Anzahl;
			Week = notNeededOrdersEntity.Week;
		}

	}
}
