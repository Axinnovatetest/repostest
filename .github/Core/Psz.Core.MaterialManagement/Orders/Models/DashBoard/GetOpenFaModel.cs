using System.ComponentModel.DataAnnotations;

namespace Psz.Core.MaterialManagement.Orders.Models.DashBoard
{
	public class FaultyOrdersAndFasResponseModel
	{
		public int? FaultyFaCount { get; set; }
		public int? FaultyOrdersCount { get; set; }
		public int? countryId { get; set; }
		public int? UnitId { get; set; }
		public FaultyOrdersAndFasResponseModel(Infrastructure.Data.Entities.Joins.MTM.Order.FaultyFertigungCountEntity ffacount, Infrastructure.Data.Entities.Joins.MTM.Order.FaultyOrdersCountEntity fc)
		{
			if(ffacount is not null)
				FaultyFaCount = ffacount.faulty_Fa_Count ?? 0;
			if(fc is not null)
				FaultyOrdersCount = fc.faulty_Orders_Count ?? 0;
		}
	}
	public class FaultyOrdersAndFasRequestModel
	{
		public int ArtikelNr { get; set; }
		[Required]
		[Range(0, int.MaxValue, ErrorMessage = "Please provide correct location")]
		public int countryId { get; set; }
		[Required]
		[Range(0, int.MaxValue, ErrorMessage = "Please provide correct location")]
		public int UnitId { get; set; }
	}

}
