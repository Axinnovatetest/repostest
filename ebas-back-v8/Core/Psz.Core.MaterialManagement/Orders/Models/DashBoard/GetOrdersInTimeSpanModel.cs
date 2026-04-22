using System.ComponentModel.DataAnnotations;


namespace Psz.Core.MaterialManagement.Orders.Models.DashBoard
{
	public class GetOrdersInTimeSpanResponseModel
	{
		public decimal? OrderedQuantity { get; set; }
		public int BestellungenNr { get; set; }
		public int BestellungenNummer { get; set; }
		public string SupplierName { get; set; }
		public DateTime? Bestatigter_Termin { get; set; }
		public DateTime? Liefertermin { get; set; }
		public GetOrdersInTimeSpanResponseModel(Infrastructure.Data.Entities.Joins.MTM.Order.GetOrdersInTimeSpanEntity data)
		{
			OrderedQuantity = data.OrderedQuantity ?? 0;
			BestellungenNummer = data.BestellungenNummer.HasValue ? data.BestellungenNummer.Value : -1;
			BestellungenNr = data.BestellungenNr.HasValue ? data.BestellungenNr.Value : -1;
			Bestatigter_Termin = data.Bestätigter_Termin;
			Liefertermin = data.Liefertermin;
			SupplierName = data.SupplierName;
		}
	}
	public class GetOrdersInTimeSpanRequestModel
	{
		[Required]
		[Range(0, int.MaxValue, ErrorMessage = "Please enter valid Artikel Number")]
		public int ArtikleNr { get; set; }
		[Required]
		[DataType(DataType.Date)]
		public DateTime SpanStart { get; set; }
		[Required]
		[DataType(DataType.Date)]
		public DateTime SpanEnd { get; set; }
		[Required]
		[Range(0, int.MaxValue, ErrorMessage = "Please enter valid Location ")]
		public int CountryId { get; set; }
		[Required]
		[Range(0, int.MaxValue, ErrorMessage = "Please enter valid Location")]
		public int UnitId { get; set; }
	}
}
