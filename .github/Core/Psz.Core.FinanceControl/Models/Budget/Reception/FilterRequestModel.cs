namespace Psz.Core.FinanceControl.Models.Budget.Reception
{
	public class FilterRequestModel
	{
		public int OrderType { get; set; }
		public string Value { get; set; }
		public bool InProgressOnly { get; set; }
	}
}
