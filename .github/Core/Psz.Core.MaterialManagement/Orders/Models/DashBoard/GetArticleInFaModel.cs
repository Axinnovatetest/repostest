using System.ComponentModel.DataAnnotations;

namespace Psz.Core.MaterialManagement.Orders.Models.DashBoard
{
	public class GetArticleInFaResponseModel
	{
		public DateTime? IssueDate { get; set; }
		public decimal CumulativeStock { get; set; }
		public string Artikelnummer { get; set; }
		public int Artikel_Nr { get; set; }

		public GetArticleInFaResponseModel(Infrastructure.Data.Entities.Joins.MTM.Order.ArticlesInFaFiltered data)
		{
			IssueDate = data.DateIssue;
			CumulativeStock = data.CumulativeStock;
			Artikelnummer = data.ArtikelNummer;
			Artikel_Nr = data.ArtikelNr;
		}
	}
	public class GetArticleInFaRequestModel: IPaginatedRequestModel
	{
		[Required]
		public int CountryId { get; set; }
		[Required]
		public int UnitId { get; set; }
		//[Required]
		//public int Family { get; set; }
		public int ArtikelNr { get; set; }

		public int Months { get; set; }
	}
}
