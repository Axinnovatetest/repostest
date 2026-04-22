namespace Psz.Core.FinanceControl.Models.Budget.Order
{
	public class RejectModel
	{
		public int OrderId { get; set; }
		public int CurrentStep { get; set; }
		public string Notes { get; set; }
	}
}
