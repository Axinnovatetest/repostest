namespace Psz.Core.BaseData.Models.Supplier
{
	public class GetSupplierRequestModel
	{
		public int SupplierId { get; set; }
		public Identity.Models.UserModel User { get; set; }
	}
}
