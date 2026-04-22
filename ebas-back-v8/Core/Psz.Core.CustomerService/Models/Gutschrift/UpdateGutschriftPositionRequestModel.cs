namespace Psz.Core.CustomerService.Models.Gutshrift
{
	public class UpdateGutschriftPositionRequestModel
	{
		public int GutschriftNr { get; set; }
		public int PositionNr { get; set; }
		public decimal Quantity { get; set; }
		public bool WithoutCopper { get; set; } = false;
		// - 2022-11-07
		public string InternComment { get; set; }
		public string Comment { get; set; }
	}
}
