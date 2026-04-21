namespace Psz.Core.MaterialManagement.Orders.Models.OrderDetails
{
	public class PriceRequestModel
	{
		public int? RahmenPosId { get; set; }
		public int? ArtikelNr { get; set; }
		public int SupplierID { get; set; }
		public int? Nr { get; set; }
	}
}
