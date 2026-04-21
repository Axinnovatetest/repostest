namespace Psz.Core.CustomerService.Models.Gutshrift
{
	public class UpdateGutschriftPriceRequestModel
	{

		public int GutschriftNr { get; set; }
		public int PositionNr { get; set; }
		public decimal UnitPrice { get; set; }
		public bool PriceFixed { get; set; } = false;
		// - 2022-10-14 - without Copper
		public bool WithoutCopper { get; set; } = false;
	}
}
