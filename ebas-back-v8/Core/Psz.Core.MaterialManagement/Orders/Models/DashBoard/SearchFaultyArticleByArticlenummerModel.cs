using System.ComponentModel.DataAnnotations;

namespace Psz.Core.MaterialManagement.Orders.Models.DashBoard
{
	public class SearchFaultyArticleByArticlenummerRequestModel
	{
		[Required]
		[Range(0, int.MaxValue, ErrorMessage = "Please provide correct location")]
		public int CountryId { get; set; }
		[Required]
		[Range(0, int.MaxValue, ErrorMessage = "Please provide correct location")]
		public int UnitId { get; set; }
		//[Required]
		//[Range(0 , int.MaxValue , ErrorMessage = "Please provide correct article Family")]
		//public int ArticleFamily { get; set; }
		[Required]
		public string nummer { get; set; }
	}
	public class SearchFaultyArticleByArticlenummerResponseModel
	{
		public DateTime? IssueDate { get; set; }
		public decimal CumulativeStock { get; set; }
		public string Artikelnummer { get; set; }
		public int Artikel_Nr { get; set; }

		public SearchFaultyArticleByArticlenummerResponseModel(Infrastructure.Data.Entities.Joins.MTM.Order.ArticlesInFaFiltered data)
		{
			IssueDate = data.DateIssue;
			CumulativeStock = data.CumulativeStock;
			Artikelnummer = data.ArtikelNummer;
			Artikel_Nr = data.ArtikelNr;
		}
	}
}
