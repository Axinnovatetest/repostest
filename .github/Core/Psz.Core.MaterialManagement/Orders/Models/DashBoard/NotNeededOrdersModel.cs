namespace Psz.Core.MaterialManagement.Orders.Models.DashBoard
{
	public class NotNeededOrdersResponseModel: IPaginatedResponseModel<NotNeededOrdersResponseModel>
	{
		public string ArtikelNummer { get; set; }
		public int ArtikelNr { get; set; }
		public bool ProjectPurchase { get; set; }
		public NotNeededOrdersResponseModel() { }
		public NotNeededOrdersResponseModel(Infrastructure.Data.Entities.Joins.MTM.Order.NotNeededOrdersEntity notNeededOrdersEntity)
		{
			ArtikelNummer = notNeededOrdersEntity.Artikelnummer;
			ArtikelNr = notNeededOrdersEntity.ArtikelNr;
			ProjectPurchase = notNeededOrdersEntity.ProjectPurchase ?? false;
		}

	}
	public class NotNeededOrdersRequestModel: IPaginatedRequestModel
	{
		public int AreaLager { get; set; }
		public int ArtikelNr { get; set; }
		public bool? ProjectOrders { get; set; }
		public string Artikelnummer { get; set; }
		public bool? OnlyUnconfirmed { get; set; }
		public DateTime? DateConfirmationBefore { get; set; }
	}
}
