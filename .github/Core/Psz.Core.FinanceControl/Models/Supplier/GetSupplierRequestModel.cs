namespace Psz.Core.FinanceControl.Models.Supplier
{
	public class GetSupplierRequestModel
	{
		public int SupplierId { get; set; }
		public Identity.Models.UserModel User { get; set; }
	}
}
