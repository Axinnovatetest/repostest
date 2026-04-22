namespace Psz.Core.MaterialManagement.Orders.Models.Statistics
{
	public class PrioEinkaufRequestModel
	{
		public int LagerId { get; set; }
		public string OrderId { get; set; }
		public string ArticleNummer { get; set; }
		public string SupplierName { get; set; }
	}
	public class PrioEinkaufResponseModel
	{
		public List<ABNotAvailable> ABNotAvailables { get; set; }
		public List<DispositionDateDifference> DispositionDateDifferences { get; set; }
	}
	public class ABNotAvailable
	{
		public string Name1 { get; set; }
		public string Telefon { get; set; }
		public string Fax { get; set; }
		public List<ReportItem> Items { get; set; }
	}
	public class DispositionDateDifference
	{
		public string Name1 { get; set; }
		public string Telefon { get; set; }
		public string Fax { get; set; }
		public List<ReortItemDateDifference> Items { get; set; }
	}

	public class ReportItem
	{
		public DateTime? Datum { get; set; }
		public int? BestellungNr { get; set; }
		public int? Lagerort_id { get; set; }
		public decimal? Anzahl { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public DateTime? Liefertermin { get; set; }
	}
	public class ReortItemDateDifference: ReportItem
	{
		public DateTime? Bestatigter_Termin { get; set; }
	}
}
