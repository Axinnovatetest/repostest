namespace Psz.Core.CustomerService.Models.Gutshrift
{
	public class CreateGutshriftItemModel
	{
		public int GutshriftId { get; set; }
		public int RechnungId { get; set; }
		public int RechnungItemId { get; set; }
		public int Quantity { get; set; }
	}
}
