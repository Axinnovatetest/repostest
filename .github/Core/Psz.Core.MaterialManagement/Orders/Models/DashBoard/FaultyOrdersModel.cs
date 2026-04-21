using System.ComponentModel.DataAnnotations;

namespace Psz.Core.MaterialManagement.Orders.Models.DashBoard
{
	public class FaultyOrdersResponseModel
	{
		public int Nr { get; set; }
		public DateTime? Liefertermin { get; set; }
		public int TotalCount { get; set; }
		public string Supplier { get; set; }
		public FaultyOrdersResponseModel(Infrastructure.Data.Entities.Joins.MTM.Order.FaultyOrdersEntity foe)
		{
			Nr = foe.Nr;
			Liefertermin = foe.Bestatigter_Termin ?? default;
			Supplier = foe.Supplier;
		}
	}
	public class FaultyOrdersRequestModel: IPaginatedRequestModel
	{
		[Required]
		[Range(0, int.MaxValue, ErrorMessage = "Please provide correct location")]
		public int CountryId { get; set; }
		[Required]
		[Range(0, int.MaxValue, ErrorMessage = "Please provide correct location")]
		public int UnitId { get; set; }
		public int ArtikelNr { get; set; }
	}


}
