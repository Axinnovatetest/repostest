using System;

namespace Psz.Core.BaseData.Models.Customer
{
	public class CustomerAdressModel
	{
		public int Id { get; set; }
		public int AddressId { get; set; }
		public int? Number { get; set; }
		public bool? Isarchived { get; set; }
		public bool? AddressEDIActive { get; set; }
		public int? AddressType { get; set; }
		public string AdressPreName { get; set; }
		// > 
		public string AdressDUNS { get; set; }
		// >
		public string AdressTitle { get; set; }
		public string AdressName1 { get; set; }
		public string AdressName2 { get; set; }
		public string AdressName3 { get; set; }
		// >
		public string AdressCountry { get; set; }
		public string AdressCity { get; set; }
		public string AdressStreet { get; set; }
		public string AdressStreetZipCode { get; set; }
		public string AdressMailbox { get; set; }
		public string AdressMailboxZipCode { get; set; }
		public bool AdressMailboxIsPreferred { get; set; }
		// >
		public string AdressPhoneNumber { get; set; }
		public string AdressFaxNumber { get; set; }
		public string AdressEmailAdress { get; set; }
		public string AdressWebsite { get; set; }
		// >
		public string AdressNote { get; set; }
		public string AdressNotes { get; set; }
		// >
		public bool AdressSelection { get; set; }
		public bool? AdressLock { get; set; }
		public string AdressLevel { get; set; }
		public DateTime? AdressRecordTime { get; set; }
		public string AdressFrom { get; set; }
		public string AdressSortTerm { get; set; }
		// >
		public string AdressFirstName { get; set; }
		public string AdressSalutation { get; set; }
		public string AdressDepartment { get; set; }
		public string AdressFunction { get; set; }
		//
		public bool IsBoth { get; set; }

		public CustomerAdressModel()
		{

		}

		public CustomerAdressModel(Infrastructure.Data.Entities.Tables.PRS.AdressenEntity adressEntity)
		{
			var address = adressEntity != null
				? new Address.AddressItemModel(adressEntity)
				: null;
			if(address != null)
			{
				AddressId = address.AdressId;
				Number = address.Number;
				Id = address.Id;
				AddressType = address.AddressType;
				AdressPreName = address.PreName;
				// > 
				AddressEDIActive = address.AddressEDIActive;
				AdressDUNS = address.DUNS;
				// >
				AdressTitle = address.Title;
				AdressName1 = address.Name1;
				AdressName2 = address.Name2;
				AdressName3 = address.Name3;
				// >
				AdressCountry = address.Country;
				AdressCity = address.City;
				AdressStreet = address.Street;
				AdressStreetZipCode = address.StreetZipCode;
				AdressMailbox = address.Mailbox;
				AdressMailboxZipCode = address.MailboxZipCode;
				AdressMailboxIsPreferred = address.MailboxIsPreferred;
				// >
				AdressPhoneNumber = address.PhoneNumber;
				AdressFaxNumber = address.FaxNumber;
				AdressEmailAdress = address.EmailAdress;
				AdressWebsite = address.Website;
				// >
				AdressNote = address.Note;
				AdressNotes = address.Notes;
				// >
				AdressSelection = address.Selection;
				//AdressLock = adress.loc;
				AdressLevel = address.Level;
				AdressRecordTime = address.RecordTime;
				AdressFrom = address.From;
				AdressSortTerm = address.SortTerm;
				// >
				AdressFirstName = address.FirstName;
				AdressSalutation = address.Salutation;
				AdressDepartment = address.Department;
				AdressFunction = address.Function;
				//
				AdressLock = address.Adresslock;
			}
		}

		public Infrastructure.Data.Entities.Tables.PRS.AdressenEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity
			{
				Abteilung = AdressDepartment,
				Adresstyp = AddressType,
				Anrede = AdressPreName,
				Auswahl = AdressSelection,
				Bemerkung = AdressNote,
				Bemerkungen = AdressNotes,
				Briefanrede = AdressSalutation,
				EMail = AdressEmailAdress,
				Fax = AdressFaxNumber,
				Funktion = AdressFunction,
				Kundennummer = Number,
				Land = AdressCountry,
				Name1 = AdressName1,
				Name2 = AdressName2,
				Name3 = AdressName3,
				Nr = AddressId,
				Ort = AdressCity,
				PLZ_Postfach = AdressMailboxZipCode,
				PLZ_StraBe = AdressStreetZipCode,
				Postfach = AdressMailbox,
				Postfach_bevorzugt = AdressMailboxIsPreferred,
				Sortierbegriff = AdressSortTerm,
				StraBe = AdressStreet,
				Stufe = AdressLevel,
				Telefon = AdressPhoneNumber,
				Titel = AdressTitle,
				Von = AdressFrom,
				Vorname = AdressFirstName,
				WWW = AdressWebsite,
				Duns = AdressDUNS,
				EDI_Aktiv = AddressEDIActive,
				Erfasst = AdressRecordTime,
				Sperren = AdressLock,
			};
		}
	}
}
