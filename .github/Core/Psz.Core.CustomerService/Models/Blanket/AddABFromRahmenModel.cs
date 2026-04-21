using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Models.Blanket
{
	public class AddABFromRahmenModel
	{
		public int AbId { get; set; }
		public int RahmenId { get; set; }
		public int CustomerId { get; set; }

		public List<RahmenPosition> Positions { get; set; }
	}

	public class RahmenPosition
	{
		public int AngeboteneArtikelNr { get; set; }
		public int PositionId { get; set; }
		public int ArticleId { get; set; }
		public decimal? RAQuantity { get; set; }
		public decimal? ABQuantity { get; set; }
		public decimal? RAPrice { get; set; }
		public DateTime? ABWunstermin { get; set; }
		public DateTime? ABLifetremin { get; set; }
		public DateTime? ExtensionDate { get; set; }
		public int? Lagerort { get; set; }
		public decimal? RARestQuantity { get; set; }

	}
}
