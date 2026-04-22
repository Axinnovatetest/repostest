namespace Psz.Core.MaterialManagement.Orders.Models.Orders
{
	public class GetOrderNrReponseModel
	{
		public int Bestellung_Nr { get; set; }
		public string Projekt_Nr { get; set; }
		public int Nr { get; set; }
		public string SupplierName { get; set; }
		public DateTime? CreationDate { get; set; }
		public GetOrderNrReponseModel(Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity bestellungenEntity)
		{
			Bestellung_Nr = bestellungenEntity.Bestellung_Nr.HasValue ? bestellungenEntity.Bestellung_Nr.Value : -1;
			Nr = bestellungenEntity.Nr;
			CreationDate = bestellungenEntity.Datum;
			Projekt_Nr = bestellungenEntity.Projekt_Nr;
			SupplierName = bestellungenEntity.Vorname_NameFirma;

		}
		public GetOrderNrReponseModel(Infrastructure.Data.Entities.Joins.MTM.Order.BestellungenEntity bestellungenEntity)
		{
			Bestellung_Nr = bestellungenEntity.Bestellung_Nr.HasValue ? bestellungenEntity.Bestellung_Nr.Value : -1;
			Projekt_Nr = bestellungenEntity.Projekt_Nr;
			Nr = bestellungenEntity.Nr;
			CreationDate = bestellungenEntity.Datum;
			SupplierName = bestellungenEntity.Vorname_NameFirma;
		}

	}
	public class GetOrderNrRequestModel
	{
		public bool CanCreateWareneingang { get; set; }
		public string Filter { get; set; }
	}
}
