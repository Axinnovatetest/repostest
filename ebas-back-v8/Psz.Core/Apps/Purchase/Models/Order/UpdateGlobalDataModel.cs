using System;

namespace Psz.Core.Apps.Purchase.Models.Order
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
		public int CustomerId { get; set; }
		public bool HasChangedFromRA(Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity entity)
		{
			if(entity.Vorname_NameFirma?.Trim()?.ToLower() != Name?.Trim()?.ToLower())
			{ return true; }
			if(entity.Name2?.Trim()?.ToLower() != Name2?.Trim()?.ToLower())
			{ return true; }
			if(entity.Name3?.Trim()?.ToLower() != Name3?.Trim()?.ToLower())
			{ return true; }
			if(entity.Ansprechpartner?.Trim()?.ToLower() != Contact?.Trim()?.ToLower())
			{ return true; }
			if(entity.Abteilung?.Trim()?.ToLower() != Department?.Trim()?.ToLower())
			{ return true; }
			if(entity.Straße_Postfach?.Trim()?.ToLower() != StreetPOBox?.Trim()?.ToLower())
			{ return true; }
			if(entity.Land_PLZ_Ort?.Trim()?.ToLower() != CountryPostcode?.Trim()?.ToLower())
			{ return true; }
			if(entity.Versandart?.Trim()?.ToLower() != Shipping?.Trim()?.ToLower())
			{ return true; }
			if(entity.Zahlungsweise?.Trim()?.ToLower() != Payment?.Trim()?.ToLower())
			{ return true; }
			if(entity.Konditionen?.Trim()?.ToLower() != Conditions?.Trim()?.ToLower())
			{ return true; }
			if(entity.USt_Berechnen != Vat)
			{ return true; }
			if(entity.Falligkeit.Value.Date != DueDate.Value.Date)
			{ return true; }
			if(entity.Briefanrede?.Trim()?.ToLower() != OrderTitle?.Trim()?.ToLower())
			{ return true; }
			if(entity.Personal_Nr != PersonalNumber)
			{ return true; }
			if(entity.Freitext?.Trim()?.ToLower() != Freetext?.Trim()?.ToLower())
			{ return true; }
			if(entity.Lieferadresse?.Trim()?.ToLower() != ShippingAddress?.Trim()?.ToLower())
			{ return true; }
			if(entity.Reparatur_nr != RepairNumber)
			{ return true; }
			if(entity.Datum.Value.Date != Date.Value.Date)
			{ return true; }
			if(entity.Wunschtermin != DesiredDate)
			{ return true; }
			if(entity.Liefertermin != DeliveryDate)
			{ return true; }
			//if(entity.Bezug?.Trim()?.ToLower() != DocumentNumber?.Trim()?.ToLower())
			//{ return true; }

			// - 
			return false;
		}
	}
}
