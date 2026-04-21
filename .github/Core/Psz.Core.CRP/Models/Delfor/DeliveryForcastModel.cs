

namespace Psz.Core.CRP.Models.Delfor
{
	public class DeliveryForcastLineItemModel
	{
		public string Position { get; set; }
		public string Material { get; set; }
		public string PSZ_Artikelnummer { get; set; }
		public decimal? Eingeteilte_Menge { get; set; }
		public decimal? Gelieferte_Menge { get; set; }
		public decimal? Letzter_Wareneing { get; set; }
		public decimal? Letzte_Lieferung { get; set; }
		public DateTime? Am { get; set; }
		public int? Lieferscheinnummer { get; set; }
		public bool Valid { get; set; }
		public List<DeliveryForcastLineItemPlanModel> LineItemPlans { get; set; }
	}
	public class DeliveryForcastLineItemPlanModel
	{
		public string Period { get; set; }
		public string Liefertermin { get; set; }
		public decimal? Einteilungs_FZ { get; set; }
		public decimal? Menge { get; set; }
		public decimal? Abw { get; set; }
		public bool Valid { get; set; }
	}
	public class DeliveryForcastHeaderModel
	{
		public string? DocumentNumber { get; set; }
		public int CustomerId { get; set; }
		public string? CustomerName { get; set; }
		public DateTime? Date { get; set; }
		public string? CustomerStreet { get; set; }
		public string? CustomerCity { get; set; }
		public string? CustomerPostCode { get; set; }
		public string? CustomerAdressStreet { get; set; }
		public string? CustomerAdressCity { get; set; }
		public string? CustomerAdressPostCode { get; set; }
		public string? CustomerAdressCountry { get; set; }
		public string? CustomerAdressFax { get; set; }
		public string? CustomerAdressTelephone { get; set; }
		public string? CustomerAdressCustomerNumber { get; set; }
		public string? CustomerAddressName { get; set; }
		public string? CustomerContactName { get; set; }
		public string? CustomerContactPhone { get; set; }
		public string? CustomerContactFax { get; set; }
		public DateTime? ValidFrom { get; set; }
		public DateTime? ValidTo { get; set; }
		public string? DUNS { get; set; }
	}
	public class DeliveryForcastModel
	{
		public DeliveryForcastHeaderModel Header { get; set; }
		public List<DeliveryForcastLineItemModel> LineItems { get; set; }
	}
}