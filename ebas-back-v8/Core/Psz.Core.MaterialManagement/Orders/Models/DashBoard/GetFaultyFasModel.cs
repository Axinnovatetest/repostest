using Psz.Core.MaterialManagement.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Psz.Core.MaterialManagement.Orders.Models.DashBoard
{
	public class GetFaultyFasResponseModel
	{
		public int Fertigungsnummer { get; set; }
		public int ID { get; set; }
		public DateTime? Termin_Bestätigt { get; set; }
		public GetFaultyFasResponseModel(Infrastructure.Data.Entities.Joins.MTM.Order.GetFaultyFasEntity fae)
		{
			Fertigungsnummer = fae.Fertigungsnummer;
			ID = fae.ID;
			Termin_Bestätigt = fae.Termin_Bestätigt ?? default(DateTime);
		}
	}
	public class ArticleAndFaultyWeek: IComparable<ArticleAndFaultyWeek>
	{
		public int artcileNr { get; set; }
		public string artikelnummer { get; set; }
		public string Week { get; set; }
		public ArticleAndFaultyWeek(int nr, string wk, string anummer)
		{
			artcileNr = nr;
			Week = wk;
			artikelnummer = anummer;
		}
		public ArticleAndFaultyWeek(int nr, string anummer)
		{
			artcileNr = nr;
			artikelnummer = anummer;
		}
		public int CompareTo(ArticleAndFaultyWeek other)
		{
			return SpecialHelper.CompareWeekPatternDiff(Week, other.Week) ? 1 : -1;
		}
	}
	public class GetFaultyFasRequestModel: IPaginatedRequestModel
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
