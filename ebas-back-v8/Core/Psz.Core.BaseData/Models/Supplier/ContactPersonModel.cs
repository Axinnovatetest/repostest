using System;

namespace Psz.Core.BaseData.Models.Supplier
{
	public class ContactPersonModel
	{
		public int Id { get; set; }
		public int Number { get; set; }
		public int KundenId { get; set; }//kunden and lieferanten Nr

		public string Title { get; set; }
		public string ContactPerson { get; set; }
		public string Department { get; set; }
		public string Position { get; set; }
		public string Salutation { get; set; }
		public string SalutationValue { get; set; }
		public string Notes { get; set; }

		public DateTime? Birthday { get; set; }

		public string EmailAdress { get; set; }
		public string PhoneNumber { get; set; }
		public string MobileNumber { get; set; }
		public string FaxNumber { get; set; }

		public string Adress { get; set; }
		public string AdressValue { get; set; }

		public string Street { get; set; }
		public string PostalCode { get; set; }
		public string City { get; set; }
		public string CountryId { get; set; }

		public int? LanguageId { get; set; }
		public string LanguageName { get; set; }

		public string Attention { get; set; }
		public bool? FormLetter { get; set; }
		public bool? SelectionAbBw { get; set; }

		public ContactPersonModel() { }
		public ContactPersonModel(Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity ansprechpartnerEntity)
		{
			var IdSalutation = (int.TryParse(ansprechpartnerEntity.Briefanrede, out int x) ? x : -1);
			var salutationEntity = Infrastructure.Data.Access.Tables.BSD.Adressen_briefanredenAccess.Get(IdSalutation);
			//
			var IdAdress = (int.TryParse(ansprechpartnerEntity.Anrede, out int y) ? y : -1);
			var adressContactPersonEntity = Infrastructure.Data.Access.Tables.BSD.Adressen_anredenAccess.Get(IdAdress);
			Department = ansprechpartnerEntity.Abteilung;
			Adress = ansprechpartnerEntity.Anrede;
			AdressValue = (adressContactPersonEntity != null) ? adressContactPersonEntity.Anrede : string.Empty;
			ContactPerson = ansprechpartnerEntity.Ansprechpartner;
			SelectionAbBw = ansprechpartnerEntity.Auswahl_AB_BW;
			Notes = ansprechpartnerEntity.Bemerkung;

			Salutation = ansprechpartnerEntity.Briefanrede;
			SalutationValue = (salutationEntity != null) ? salutationEntity.Anrede : string.Empty;

			EmailAdress = ansprechpartnerEntity.EMail;
			FaxNumber = ansprechpartnerEntity.FAX;
			Birthday = ansprechpartnerEntity.Geburtstag;
			CountryId = ansprechpartnerEntity.Land;

			MobileNumber = ansprechpartnerEntity.Mobil;
			Id = ansprechpartnerEntity.Nr;
			Number = ansprechpartnerEntity.Nummer ?? -1;
			City = ansprechpartnerEntity.Ort;
			PostalCode = ansprechpartnerEntity.PLZ;

			Position = ansprechpartnerEntity.Position;
			FormLetter = ansprechpartnerEntity.Serienbrief;
			LanguageId = ansprechpartnerEntity.Sprache;
			LanguageName = ansprechpartnerEntity.Sprache.HasValue
				? ansprechpartnerEntity.Sprache.Value == 0 ? "German" : "English"
				: string.Empty;
			Street = ansprechpartnerEntity.StraBe;
			PhoneNumber = ansprechpartnerEntity.Telefon;

			Title = ansprechpartnerEntity.Titel;
			Attention = ansprechpartnerEntity.Zu_Handen;
		}

		public Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity ToDataEntity(int? number)
		{
			return new Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity()
			{
				Nr = Id,
				Nummer = number ?? Number,
				Abteilung = Department,
				Anrede = Adress,
				Ansprechpartner = ContactPerson,
				Auswahl_AB_BW = SelectionAbBw,
				Bemerkung = Notes,

				Briefanrede = Salutation,
				EMail = EmailAdress,
				FAX = FaxNumber,
				Geburtstag = Birthday,
				Land = CountryId,

				Mobil = MobileNumber,
				Ort = City,
				PLZ = PostalCode,

				Position = Position,
				Serienbrief = FormLetter,
				Sprache = LanguageId,

				StraBe = Street,
				Telefon = PhoneNumber,

				Titel = Title,
				Zu_Handen = Attention,
			};
		}
	}
}
