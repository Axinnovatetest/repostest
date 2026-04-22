using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.MaterialManagement.Orders.Models.Orders
{
	public class OrderPrioViewModel
	{
		public int Nr { get; set; }
		public int? OrderNumber { get; set; }
		public string ProjectNumber { get; set; }
		public string Type { get; set; }
		public string Name_CompanyName { get; set; }
		public DateTime? Date { get; set; }
		public string Condition { get; set; }
		public string User { get; set; }
		public bool? gebucht { get; set; }
		public string Status { get; set; }
		public int? Position { get; set; }
		public string Artikelnummer { get; set; }
		public int? ArtikelNr { get; set; }
		public DateTime? Liefertermin { get; set; }
		public DateTime? Bestatigter_Termin { get; set; }
		public string StorageLocation { get; set; }
		public bool? ProjectPurchase { get; set; }
		public bool HavePlacements { get; set; }
		public bool? StandardSupplierViolation { get; set; }
		public OrderPrioViewModel()
		{

		}
		public OrderPrioViewModel(Infrastructure.Data.Entities.Joins.MTM.Order.OrdersPrioViewEntity entity)
		{
			Nr = entity.Nr;
			OrderNumber = entity.Bestellung_Nr;
			ProjectNumber = entity.Projekt_Nr;
			Type = entity.Typ;
			Name_CompanyName = entity.Vorname_NameFirma;
			Date = entity.Datum;
			Condition = entity.Konditionen;
			User = entity.Bearbeiter;
			gebucht = entity.gebucht;
			Position = entity.Position;
			ArtikelNr = entity.Artikel_Nr;
			Artikelnummer = entity.Artikelnummer;
			Liefertermin = entity.Liefertermin;
			Bestatigter_Termin = entity.Bestatigter_Termin;
			StorageLocation = entity.Lagerort;
			ProjectPurchase = entity.ProjectPurchase;
			StandardSupplierViolation = entity.StandardSupplierViolation;
		}
	}
	public class OrderPrioViewResponseModel: IPaginatedResponseModel<OrderPrioViewModel> { }

	public class OrdersAnomaliesRequestModel: IPaginatedRequestModel
	{
		public string SearchText { get; set; }
	}
}
