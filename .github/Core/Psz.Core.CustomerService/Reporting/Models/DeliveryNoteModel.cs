using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.Reporting.Models
{
	public class DeliveryNoteModel
	{
		public string Anrede { get; set; }
		public string Name1 { get; set; }
		public string Name2 { get; set; }
		public string Street { get; set; }
		public string Country { get; set; }
		public string LAnrede { get; set; }
		public string LName1 { get; set; }
		public string LName2 { get; set; }
		public string LStreet { get; set; }
		public string LCountry { get; set; }
		public string MessageHeader { get; set; }
		public string DocumentTitle { get; set; }
		public string TopHeader { get; set; }
		// -
		public string CustomerNumber { get; set; }
		public string ShippingMethod { get; set; }
		public string VAT_ID { get; set; }
		public string PosText { get; set; }
		public int Angebote_Unser_Zeichen { get; set; }
		public class DeliveryNoteItemModel
		{
			public int? Angebote_Angebot_Nr { get; set; }
			public string Angebote_Bezug { get; set; }
			public double? ANgeboteArtikel_Anzahl { get; set; }
			public string ANgeboteArtikel_Bezeichnung1 { get; set; }
			public string ANgeboteArtikel_Bezeichnung2 { get; set; }
			public string ANgeboteArtikel_Bezeichnung2_Kunde { get; set; }
			public string ANgeboteArtikel_Bezeichnung3 { get; set; }
			public string ANgeboteArtikel_Einheit { get; set; }
			public Single? ANgeboteArtikel_EinzelCu_Gewicht { get; set; }
			public Single? ANgeboteArtikel_GesamtCu_Gewicht { get; set; }
			public DateTime? ANgeboteArtikel_Liefertermin { get; set; }
			public string Artikelnummer { get; set; }
			public string Ursprungsland { get; set; }
			public string Zolltarif_nr { get; set; }
		}
	}
}
