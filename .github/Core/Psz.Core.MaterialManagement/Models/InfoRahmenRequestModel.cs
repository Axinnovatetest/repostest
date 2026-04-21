namespace Psz.Core.MaterialManagement.Models
{
	public class InfoRahmenRequestModel
	{
		public int? ArtikelNr { get; set; }
		public int? PositionNr { get; set; }
		public decimal Quantity { get; set; }
		public int SupplierId { get; set; }
		public DateTime? ConfirmationDate { get; set; }
	}
}
