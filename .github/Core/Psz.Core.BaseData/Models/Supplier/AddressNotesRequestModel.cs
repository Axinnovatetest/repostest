namespace Psz.Core.BaseData.Models.Supplier
{
	public class AddressNotesRequestModel
	{
		public int SupplierId { get; set; }
		public int AddressId { get; set; }
		public string Notes { get; set; }
	}
}
