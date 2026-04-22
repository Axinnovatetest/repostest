namespace Psz.Core.CustomerService.Models.Gutshrift
{
	public class UpdateGutschriftPositionEntryModel
	{
		public int GutschriftNr { get; set; }
		public int PositionNr { get; set; }
		public decimal Quantity { get; set; }

		public UpdateGutschriftPositionEntryModel()
		{

		}
	}
}
