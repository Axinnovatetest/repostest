namespace Psz.Core.FinanceControl.Models.Budget.Order
{
	public class ValidateModel
	{
		public int OrderId { get; set; }
		public int CurrentStep { get; set; }
		public int MandantId { get; set; }
		public string Notes { get; set; }
	}
}
