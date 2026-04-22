using System.ComponentModel.DataAnnotations;

namespace Psz.Core.MaterialManagement.Orders.Models.DashBoard
{
	public class GetFasInTimeSpanResponseModel
	{
		public decimal? NeededQuantity { get; set; }
		public DateTime? Termin_Bestatigt1 { get; set; }
		public int Fertigungsnummer { get; set; }
		public int FertigungId { get; set; }
		public GetFasInTimeSpanResponseModel(Infrastructure.Data.Entities.Joins.MTM.Order.GetFAsInTimeSpanEntity data)
		{
			NeededQuantity = data.NeededQuantity ?? 0;
			Termin_Bestatigt1 = data.Termin_Bestätigt1 ?? default;
			Fertigungsnummer = data.Fertigungsnummer;
			FertigungId = data.FertigungId;
		}
	}
	public class GetFasInTimeSpanRequestModel
	{
		[Required]
		[Range(0, int.MaxValue, ErrorMessage = "Please enter valid Artikel Number")]
		public int ArtikleNr { get; set; }
		[Required]
		public DateTime SpanStart { get; set; }
		[Required]
		public DateTime SpanEnd { get; set; }
		[Required]
		[Range(0, int.MaxValue, ErrorMessage = "Please enter valid Location ")]
		public int CountryId { get; set; }
		[Required]
		[Range(0, int.MaxValue, ErrorMessage = "Please enter valid Location ")]
		public int UnitId { get; set; }
	}
}
