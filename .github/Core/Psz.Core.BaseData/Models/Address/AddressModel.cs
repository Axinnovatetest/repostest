using Infrastructure.Data.Entities.Joins.PRS;
using Psz.Core.Common.Models;
using System;

namespace Psz.Core.BaseData.Models.Address
{
	public class CopyRequestModel
	{
		public int CopyId { get; set; }
		public int NewAddressType { get; set; }
	}
	// - Customer/Suppplier Address
	public class AddressItemModel
	{
		public int Id { get; set; }
		public int Number { get; set; }
		public int AdressId { get; set; }
		public bool? Isarchived { get; set; }
		public int AddressType { get; set; }
		public string DUNS { get; set; }
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
		public bool? Adresslock { get; set; }
		//
		public bool? Edi_Aktiv_Delfor { get; set; }
		public bool? Edi_Aktiv_Desadv { get; set; }

		public AddressItemModel()
		{ }
		public AddressItemModel(Infrastructure.Data.Entities.Tables.PRS.AdressenEntity addressEntity)
		{
			this.Id = /*Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(addressEntity.Nr) != null ? Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(addressEntity.Nr).Nr :*/ -1;
			this.Number = /*int.TryParse(addressEntity.Kundennummer.ToString(), out int nummer) ? nummer : */-1;
			this.AdressId = addressEntity.Nr;
			this.AddressType = (addressEntity.Adresstyp.HasValue) ? addressEntity.Adresstyp.Value : -1;/*int.TryParse(addressEntity.Adresstyp.ToString(), out int _type) ? _type : -1;*/
			this.DUNS = addressEntity.Duns;
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
			this.MailboxIsPreferred = addressEntity.Postfach_bevorzugt ?? false;

			this.PhoneNumber = addressEntity.Telefon;
			this.FaxNumber = addressEntity.Fax;
			this.EmailAdress = addressEntity.EMail;
			this.Website = addressEntity.WWW;

			this.Note = addressEntity.Bemerkung;
			this.Notes = addressEntity.Bemerkungen;

			this.Selection = addressEntity.Auswahl ?? false;
			this.Level = addressEntity.Stufe;
			this.RecordTime = addressEntity.Erfasst;
			this.From = addressEntity.Von;
			this.SortTerm = addressEntity.Sortierbegriff;

			this.Salutation = addressEntity.Briefanrede;
			this.FirstName = addressEntity.Vorname;
			this.Department = addressEntity.Abteilung;
			this.Function = addressEntity.Funktion;

			this.AddressEDIActive = addressEntity.EDI_Aktiv;
			this.Adresslock = addressEntity.Sperren;
		}
	}

	// - Address
	public class AddressModel
	{
		public int? CustomerId { get; set; }
		public int Id { get; set; }
		public int Number { get; set; }
		public bool? Isarchived { get; set; }
		public int AddressType { get; set; }
		public string DUNS { get; set; }
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
		public bool AddressEDIActive { get; set; }
		public bool Adresslock { get; set; }
		public int? SupplierId { get; set; }
		public string UnloadingPoint { get; set; }
		public string StorageLocation { get; set; }

		public AddressModel()
		{ }
		public AddressModel(AdressenKundenEntity addressEntity)
		{
			this.Number = /*int.TryParse(addressEntity.Kundennummer.ToString(), out int nummer) ? nummer : */-1;
			this.Id = addressEntity.Id;
			this.AddressType = addressEntity.AddressType;
			this.DUNS = addressEntity.DUNS;
			this.PreName = addressEntity.PreName;
			this.Category = (Enums.AddressEnums.Categories)addressEntity.AddressType;
			this.SupplierNumber = addressEntity.SupplierNumber;
			this.CustomerNumber = addressEntity.CustomerNumber;
			this.Name1 = addressEntity.Name1;
			this.Name2 = addressEntity.Name2;
			this.Name3 = addressEntity.Name3;
			this.Country = addressEntity.Country;
			this.City = addressEntity.City;
			this.Street = addressEntity.Street;
			this.StreetZipCode = addressEntity.StreetZipCode;
			this.Mailbox = addressEntity.Mailbox;
			this.MailboxZipCode = addressEntity.MailboxZipCode;
			this.MailboxIsPreferred = addressEntity.MailboxIsPreferred ?? false;

			this.PhoneNumber = addressEntity.PhoneNumber;
			this.FaxNumber = addressEntity.FaxNumber;
			this.EmailAdress = addressEntity.EmailAdress;
			this.Website = addressEntity.Website;

			this.Note = addressEntity.Note;
			this.Notes = addressEntity.Notes;
			this.Salutation = addressEntity.Salutation;
			this.Department = addressEntity.Department;
			this.Adresslock = addressEntity.Adresslock ?? false;
			this.CustomerId = addressEntity.CustomerId;
			this.SupplierId = addressEntity.SupplierId;
		}
		public AddressModel(Infrastructure.Data.Entities.Tables.PRS.AdressenEntity addressEntity)
		{
			this.Number = /*int.TryParse(addressEntity.Kundennummer.ToString(), out int nummer) ? nummer : */-1;
			this.Id = addressEntity.Nr;
			this.AddressType = (addressEntity.Adresstyp.HasValue) ? addressEntity.Adresstyp.Value : -1;/*int.TryParse(addressEntity.Adresstyp.ToString(), out int _type) ? _type : -1;*/
			this.DUNS = addressEntity.Duns;
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
			this.MailboxIsPreferred = addressEntity.Postfach_bevorzugt ?? false;

			this.PhoneNumber = addressEntity.Telefon;
			this.FaxNumber = addressEntity.Fax;
			this.EmailAdress = addressEntity.EMail;
			this.Website = addressEntity.WWW;

			this.Note = addressEntity.Bemerkung;
			this.Notes = addressEntity.Bemerkungen;

			this.Selection = addressEntity.Auswahl ?? false;
			this.Level = addressEntity.Stufe;
			this.RecordTime = addressEntity.Erfasst;
			this.From = addressEntity.Von;
			this.SortTerm = addressEntity.Sortierbegriff;

			this.Salutation = addressEntity.Briefanrede;
			this.FirstName = addressEntity.Vorname;
			this.Department = addressEntity.Abteilung;
			this.Function = addressEntity.Funktion;

			this.AddressEDIActive = addressEntity.EDI_Aktiv ?? false;
			this.Adresslock = addressEntity.Sperren ?? false;
		}
	}
	public class AddAdresseRequestModel
	{
		public string CustomerNumber { get; set; }
		public string SupplierNumber { get; set; }
		public Enums.AddressEnums.Categories Type { get; set; }
		public string Name1 { get; set; }
		public string Name2 { get; set; }
		public string Name3 { get; set; }
		public string Street { get; set; }
		public string City { get; set; }
		public string StreetZipCode { get; set; }
		public string Country { get; set; }
		public string PhoneNumber { get; set; }
		public string FaxNumber { get; set; }
		public string EmailAdress { get; set; }
		public string Website { get; set; }
		public string Notes { get; set; }
		public string Note { get; set; }
		public string Salutation { get; set; }
		public string Department { get; set; }
		public string Mailbox { get; set; }
		public string PreName { get; set; }
		public string UnloadingPoint { get; set; }
		public string StorageLocation { get; set; }
		public AddAdresseRequestModel() { }
		public AddAdresseRequestModel(Infrastructure.Data.Entities.Tables.PRS.AdressenEntity addressEntity)
		{
			this.PreName = addressEntity.Anrede;
			this.Type = addressEntity.Adresstyp.HasValue
				? (Enums.AddressEnums.Categories)addressEntity.Adresstyp.Value
				: Enums.AddressEnums.Categories.Standard;
			this.SupplierNumber = addressEntity.Lieferantennummer.ToString();
			this.CustomerNumber = addressEntity.Kundennummer.ToString();

			this.Name1 = addressEntity.Name1;
			this.Name2 = addressEntity.Name2;
			this.Name3 = addressEntity.Name3;

			this.Country = addressEntity.Land;
			this.City = addressEntity.Ort;
			this.Street = addressEntity.StraBe;
			this.StreetZipCode = addressEntity.PLZ_StraBe;
			this.Mailbox = addressEntity.Postfach;


			this.PhoneNumber = addressEntity.Telefon;
			this.FaxNumber = addressEntity.Fax;
			this.EmailAdress = addressEntity.EMail;
			this.Website = addressEntity.WWW;

			this.Note = addressEntity.Bemerkung;
			this.Notes = addressEntity.Bemerkungen;

			this.Salutation = addressEntity.Briefanrede;
			this.Department = addressEntity.Abteilung;

            this.UnloadingPoint = addressEntity.UnloadingPoint;
            this.StorageLocation = addressEntity.StorageLocation;
        }
	}
	public class GetAddressesListRequestModel: IPaginatedRequestModel
	{
		public string SearchValue { get; set; }
		public int AdresseType { get; set; }

	}
	public class GetAdressesListResponseModel: IPaginatedResponseModel<AddressModel>
	{

	}
}
