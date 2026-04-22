using System;

namespace Psz.Core.CustomerService.Models.OrderProcessing
{
	public class UpdateGlobalDataModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Name2 { get; set; }
		public string Name3 { get; set; }
		public string Contact { get; set; }
		public string Department { get; set; } // Abteilung
		public string StreetPOBox { get; set; } // Straße/Postfach
		public string CountryPostcode { get; set; } //Land/PLZ/Ort
		public string Shipping { get; set; } // Versandart
		public string Payment { get; set; } // Zahlungsweise
		public string Conditions { get; set; } // Konditionen
		public bool Vat { get; set; } // USt_Berechnen
		public DateTime? DueDate { get; set; } //Fälligkeit
		public string OrderTitle { get; set; } // Briefanrede
		public int? PersonalNumber { get; set; } //Personal-Nr
		public string Freetext { get; set; } // Freitext
		public string ShippingAddress { get; set; } // Lieferadresse
		public int? RepairNumber { get; set; } // reparatur_nr
		public DateTime? Date { get; set; } // Datum
		public DateTime? DesiredDate { get; set; } // Wunschtermin
		public DateTime? DeliveryDate { get; set; } // Fälligkeit
		public string DocumentNumber { get; set; }
		public int Version { get; set; }
	}
}
