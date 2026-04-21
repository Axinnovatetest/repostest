using System;

namespace Psz.Core.FinanceControl.Models.Supplier
{
	public class ContactPersonModel
	{
		public int Id { get; set; }
		public int Number { get; set; }

		public string Title { get; set; }
		public string ContactPerson { get; set; }
		public string Department { get; set; }
		public string Position { get; set; }
		public string Salutation { get; set; }
		public string Notes { get; set; }

		public DateTime? Birthday { get; set; }

		public string EmailAdress { get; set; }
		public string PhoneNumber { get; set; }
		public string MobileNumber { get; set; }
		public string FaxNumber { get; set; }

		public string Adress { get; set; }
		public string Street { get; set; }
		public string PostalCode { get; set; }
		public string City { get; set; }
		public string Country { get; set; }

		public int? LanguageId { get; set; }
		public string LanguageName { get; set; }

		public string Attention { get; set; }
		public bool? FormLetter { get; set; }
		public bool? SelectionAbBw { get; set; }

		public ContactPersonModel() { }
		public ContactPersonModel(Infrastructure.Data.Entities.Tables.FNC.AnsprechpartnerEntity ansprechpartnerEntity)
		{
			this.Department = ansprechpartnerEntity.Abteilung;
			this.Adress = ansprechpartnerEntity.Anrede;
			this.ContactPerson = ansprechpartnerEntity.Ansprechpartner;
			this.SelectionAbBw = ansprechpartnerEntity.Auswahl_AB_BW;
			this.Notes = ansprechpartnerEntity.Bemerkung;

			this.Salutation = ansprechpartnerEntity.Briefanrede;
			this.EmailAdress = ansprechpartnerEntity.EMail;
			this.FaxNumber = ansprechpartnerEntity.FAX;
			this.Birthday = ansprechpartnerEntity.Geburtstag;
			this.Country = ansprechpartnerEntity.Land;

			this.MobileNumber = ansprechpartnerEntity.Mobil;
			this.Id = ansprechpartnerEntity.Nr;
			this.Number = ansprechpartnerEntity.Nummer ?? -1;
			this.City = ansprechpartnerEntity.Ort;
			this.PostalCode = ansprechpartnerEntity.PLZ;

			this.Position = ansprechpartnerEntity.Position;
			this.FormLetter = ansprechpartnerEntity.Serienbrief;
			this.LanguageId = ansprechpartnerEntity.Sprache;
			this.LanguageName = ansprechpartnerEntity.Sprache.HasValue
				? ansprechpartnerEntity.Sprache.Value == 0 ? "German" : "English"
				: string.Empty;
			this.Street = ansprechpartnerEntity.StraBe;
			this.PhoneNumber = ansprechpartnerEntity.Telefon;

			this.Title = ansprechpartnerEntity.Titel;
			this.Attention = ansprechpartnerEntity.Zu_Handen;
		}

		public Infrastructure.Data.Entities.Tables.FNC.AnsprechpartnerEntity ToDataEntity(int? number)
		{
			return new Infrastructure.Data.Entities.Tables.FNC.AnsprechpartnerEntity()
			{
				Nr = this.Id,
				Nummer = number ?? this.Number,

				Abteilung = Department,
				Anrede = this.Adress,
				Ansprechpartner = this.ContactPerson,
				Auswahl_AB_BW = this.SelectionAbBw,
				Bemerkung = this.Notes,

				Briefanrede = this.Salutation,
				EMail = this.EmailAdress,
				FAX = this.FaxNumber,
				Geburtstag = this.Birthday,
				Land = this.Country,

				Mobil = this.MobileNumber,
				Ort = this.City,
				PLZ = this.PostalCode,

				Position = this.Position,
				Serienbrief = this.FormLetter,
				Sprache = this.LanguageId,

				StraBe = this.Street,
				Telefon = this.PhoneNumber,

				Titel = this.Title,
				Zu_Handen = this.Attention,
			};
		}
	}
}
