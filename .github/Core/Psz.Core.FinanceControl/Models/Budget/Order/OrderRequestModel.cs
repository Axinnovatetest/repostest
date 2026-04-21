namespace Psz.Core.FinanceControl.Models.Budget.Order
{
	public class OrderRequestModel
	{
		public bool? ShowOnlyInProgress { get; set; }
		public bool? ShowCompletelyBooked { get; set; }
		public int? Year { get; set; }
	}
}
