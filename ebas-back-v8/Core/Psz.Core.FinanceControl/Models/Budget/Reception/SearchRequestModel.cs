namespace Psz.Core.FinanceControl.Models.Budget.Reception
{
	public class SearchRequestModel
	{
		public bool InProgressOnly { get; set; }
		public int OrderType { get; set; }
		public string Bestellung_Nr { get; set; }
		public string Projekt_Nr { get; set; }
		public string Bezug { get; set; }
		public string Artikel_Nr { get; set; }

		public string Datum { get; set; }
		public string Liefertermin { get; set; }
		public string Lieferanten_Nr { get; set; }

		// - Extension props
		public int? CompanyId { get; set; }
		public int? DepartmentId { get; set; }
		public int? IssuerId { get; set; }

		// -
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
		public Enums.BudgetEnums.ReceptionSearchFields SortFieldKey { get; set; }
		public bool SortDesc { get; set; } = false;
	}
}
