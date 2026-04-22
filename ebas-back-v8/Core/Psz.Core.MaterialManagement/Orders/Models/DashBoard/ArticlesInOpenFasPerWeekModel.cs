using Psz.Core.MaterialManagement.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Psz.Core.MaterialManagement.Orders.Models.DashBoard
{
	public class FaultyArticlesInOpenFasPerWeekResponseModel: IComparable<FaultyArticlesInOpenFasPerWeekResponseModel>
	{
		public string Week { get; set; }
		public decimal? NeedQuantity { get; set; }
		public string Artikelnummer { get; set; }
		public int Artikel_Nr { get; set; }
		public DateTime? Termin_Bestätigt1 { get; set; }

		public FaultyArticlesInOpenFasPerWeekResponseModel(Infrastructure.Data.Entities.Joins.MTM.Order.NeededQuantityByArticleAndWeekEntity data)
		{
			Week = data.week ?? null;
			NeedQuantity = data.NeedQuantity ?? 0;
			Artikelnummer = data.Artikelnummer ?? null;
			Artikel_Nr = data.ArtikelNr;
		}
		public int CompareTo(FaultyArticlesInOpenFasPerWeekResponseModel other)
		{
			return SpecialHelper.CompareWeekPatternDiff(Week, other.Week) ? 1 : -1;
		}
	}
	public class FaultyArticlesInOpenFasRequestModel: IPaginatedRequestModel
	{
		[Required]
		[Range(0, int.MaxValue, ErrorMessage = "Please provide correct location")]
		public int CountryId { get; set; }
		[Required]
		[Range(0, int.MaxValue, ErrorMessage = "Please provide correct location")]
		public int UnitId { get; set; }
		[Required]
		[Range(-1, int.MaxValue, ErrorMessage = "Please provide correct family")]
		public int Family { get; set; }
		public int ArtikelNR { get; set; }

	}
}
