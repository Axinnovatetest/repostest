using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Models.DeliveryNote
{
	public class ValidateDeliveryNoteModel
	{
		public int Nr { get; set; }
		public int AngebotNr { get; set; }
		public int KundenNr { get; set; }
		public string VornameFirma { get; set; }
		public string Bezug { get; set; }
		public bool? VersandBerechnen { get; set; }
		public string Standardversand { get; set; }
		public DateTime? Versandatum { get; set; }
		public decimal? VersandKosten { get; set; }
		public DateTime? DeliveryDate { get; set; }
		public List<ItemModel> Items { get; set; }

		public ValidateDeliveryNoteModel()
		{

		}
	}
}
