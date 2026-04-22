using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Models.DeliveryNote
{
	public class CreateResponseModel
	{
		public int Nr { get; set; }
		public int AngebotNr { get; set; }          // Angebote.[Angebot - Nr]
		public int KundenNr { get; set; }           //Angebote.[Kunden-Nr]
		public string VornameFirma { get; set; }    //Angebote.[Vorname/NameFirma]
		public string Bezug { get; set; }//Angebote.[Bezug]
		public string Standardversand { get; set; }
		public DateTime? Versandatum { get; set; }
		public List<KeyValuePair<string, string>> Infos { get; set; }
		public List<ItemModel> Items { get; set; } = new List<ItemModel>();

	}
}
