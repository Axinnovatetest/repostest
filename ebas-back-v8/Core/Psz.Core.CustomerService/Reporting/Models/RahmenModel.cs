using System.Collections.Generic;

namespace Psz.Core.CustomerService.Reporting.Models
{
	public class RahmenModel
	{
		public string Adress1 { get; set; }
		public string Adress2 { get; set; }
		public string Adress3 { get; set; }
		public string Adress4 { get; set; }
		public string Adress5 { get; set; }
		public string Adress6 { get; set; }
		public string Adress7 { get; set; }
		public string Adress8 { get; set; }
		public string Adress9 { get; set; }
		public string Adress10 { get; set; }
		public string Nr { get; set; }
		public string Date { get; set; }
		public string ExtensionDate { get; set; }
		public string TradingTerm { get; set; }
		public string PayementTerm { get; set; }
		public string Customer { get; set; }
		public string Supplier { get; set; }
		public string Payement { get; set; }
		public string Logo { get; set; }
		public List<RahmenPositionsModel> Positions { get; set; }
		public class RahmenPositionsModel
		{
			public string Artikel { get; set; }
			public string Description { get; set; }
			public decimal Quantity { get; set; }
			public string UOM { get; set; }
			public string Price { get; set; }
			public decimal VAT { get; set; }
			public decimal Price_UOM { get; set; }
			public decimal Total { get; set; }
			public string WishData { get; set; }
			public string Position { get; set; }
			public string Bestellnummer { get; set; }
			public string LT { get; set; }
			public decimal MengeImSicherheitslager { get; set; }
		}
	}
}
