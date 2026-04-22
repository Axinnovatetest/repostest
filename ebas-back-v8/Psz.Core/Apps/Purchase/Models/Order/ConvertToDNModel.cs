using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Models.Order
{
	public class ConvertToDNModel
	{
		public int OrderId { get; set; } // Angebote nr
		public bool erledigt_pos { get; set; }
		public bool gebucht { get; set; }
		public bool erledigt { get; set; }
		public DateTime DeliveryDate { get; set; }
		public List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> Articles { get; set; } = new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> { };
	}
}
