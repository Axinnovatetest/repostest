using Infrastructure.Data.Entities.Joins.MTM.Order;

namespace Psz.Core.MaterialManagement.Orders.Models.Orders
{
	public class ROHArticleRahmenNeedsResponseModel
	{
		public decimal NeedsRahmenPurchase { get; set; }
		public decimal NeedsRahmenSale { get; set; }
	}
	public class NeedsInRahmenSaleModel
	{
		public int? Nr { get; set; }
		public int? Angebot_Nr { get; set; }
		public string Artikelnummer { get; set; }
		public int? ArtikelNr { get; set; }
		public int? Position { get; set; }
		public string Bezug { get; set; }
		public decimal? RestQty { get; set; }
		public decimal? NeededInBOM { get; set; }
		public decimal? SumNeeded { get; set; }
		public DateTime? ExtensionDate { get; set; }
		public string Customer { get; set; }
		public NeedsInRahmenSaleModel()
		{

		}
		public NeedsInRahmenSaleModel(NeedsInRahmenSaleEntity entity)
		{
			Nr = entity.Nr;
			Angebot_Nr = entity.Angebot_Nr;
			Bezug = entity.Bezug;
			RestQty = entity.RestQty;
			SumNeeded = entity.SumNeeded;
			NeededInBOM = entity.NeededInBOM;
			Position = entity.Position;
			ExtensionDate = entity.ExtensionDate;
			Artikelnummer = entity.Artikelnummer;
			ArtikelNr = entity.ArtikelNr;
			Customer = entity.Customer;
		}
	}
}