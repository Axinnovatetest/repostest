using System;

namespace Psz.Core.FinanceControl.Models.Address
{
	public class AddressModel
	{
		public int Id { get; set; }
		public int AddressType { get; set; }
		public int? DUNS { get; set; }
		public string PreName { get; set; }
		public Enums.AddressEnums.Categories Category { get; set; }
		public int? SupplierNumber { get; set; }
		public int? CustomerNumber { get; set; }
		public int? PersonalNumber { get; set; }

		public string Title { get; set; }
		public string Name1 { get; set; }
		public string Name2 { get; set; }
		public string Name3 { get; set; }

		public string Country { get; set; }
		public string City { get; set; }
		public string Street { get; set; }
		public string StreetZipCode { get; set; }
		public string Mailbox { get; set; }
		public string MailboxZipCode { get; set; }
		public bool MailboxIsPreferred { get; set; }

		public string PhoneNumber { get; set; }
		public string FaxNumber { get; set; }
		public string EmailAdress { get; set; }
		public string Website { get; set; }

		public string Note { get; set; }
		public string Notes { get; set; }

		public bool Selection { get; set; }
		public string Level { get; set; }
		public DateTime? RecordTime { get; set; }
		public string From { get; set; }
		public string SortTerm { get; set; }

		public string Salutation { get; set; }
		public string Function { get; set; }
		public string FirstName { get; set; }
		public string Department { get; set; }
		public bool? AddressEDIActive { get; set; }

		public AddressModel()
		{ }
		public AddressModel(Infrastructure.Data.Entities.Tables.FNC.AdressenEntity addressEntity)
		{
			this.Id = addressEntity.Nr;
			this.AddressType = int.TryParse(addressEntity.Adresstyp.ToString(), out int _type) ? _type : -1;
			//this.DUNS = addressEntity.Duns;
			this.PreName = addressEntity.Anrede;
			this.Category = addressEntity.Adresstyp.HasValue
				? (Enums.AddressEnums.Categories)addressEntity.Adresstyp.Value
				: Enums.AddressEnums.Categories.Standard;
			this.SupplierNumber = addressEntity.Lieferantennummer;
			this.CustomerNumber = addressEntity.Kundennummer;
			this.PersonalNumber = addressEntity.Personalnummer;

			this.Title = addressEntity.Titel;
			this.Name1 = addressEntity.Name1;
			this.Name2 = addressEntity.Name2;
			this.Name3 = addressEntity.Name3;

			this.Country = addressEntity.Land;
			this.City = addressEntity.Ort;
			this.Street = addressEntity.StraBe;
			this.StreetZipCode = addressEntity.PLZ_StraBe;
			this.Mailbox = addressEntity.Postfach;
			this.MailboxZipCode = addressEntity.PLZ_Postfach;
			//this.MailboxIsPreferred = addressEntity.Postfach_bevorzugt ?? false;

			this.PhoneNumber = addressEntity.Telefon;
			this.FaxNumber = addressEntity.Fax;
			this.EmailAdress = addressEntity.eMail;
			this.Website = addressEntity.WWW;

			//this.Note = addressEntity.Bemerkung;
			this.Notes = addressEntity.Bemerkungen;

			//this.Selection = addressEntity.Auswahl ?? false;
			this.Level = addressEntity.stufe;
			this.RecordTime = addressEntity.erfasst;
			this.From = addressEntity.von;
			this.SortTerm = addressEntity.Sortierbegriff;

			this.Salutation = addressEntity.Briefanrede;
			this.FirstName = addressEntity.Vorname;
			this.Department = addressEntity.Abteilung;
			this.Function = addressEntity.Funktion;

			//this.AddressEDIActive = addressEntity.EDI_Aktiv;
		}
	}
}
