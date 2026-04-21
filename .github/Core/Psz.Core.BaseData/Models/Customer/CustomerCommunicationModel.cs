namespace Psz.Core.BaseData.Models.Customer
{
	public class CustomerCommunicationModel
	{
		public int Id { get; set; }
		public int Number { get; set; }
		public int AdressId { get; set; }
		public bool? Isarchived { get; set; }
		public string AdressMailboxZipCode { get; set; }
		public string AdressMailbox { get; set; }
		public bool AdressMailboxIsPreferred { get; set; }
		public string AdressPhoneNumber { get; set; }
		public string AdressFaxNumber { get; set; }
		public string AdressEmailAdress { get; set; }
		public string AdressWebsite { get; set; }
		public string AdressNote { get; set; }
		public string OrderAddress { get; set; }
		public int? LanguageId { get; set; }

		public CustomerCommunicationModel()
		{

		}

		public CustomerCommunicationModel(Infrastructure.Data.Entities.Tables.PRS.AdressenEntity adressenEntity)
		{
			AdressId = adressenEntity.Nr;
			Number = adressenEntity.Kundennummer.HasValue ? adressenEntity.Kundennummer.Value : -1;
			AdressMailboxZipCode = adressenEntity.PLZ_Postfach;
			AdressMailbox = adressenEntity.Postfach;
			AdressMailboxIsPreferred = adressenEntity.Postfach_bevorzugt.HasValue ? adressenEntity.Postfach_bevorzugt.Value : false;
			AdressPhoneNumber = adressenEntity.Telefon;
			AdressFaxNumber = adressenEntity.Fax;
			AdressEmailAdress = adressenEntity.EMail;
			AdressWebsite = adressenEntity.WWW;
			AdressNote = adressenEntity.Bemerkung;
		}
		public CustomerCommunicationModel(Infrastructure.Data.Entities.Tables.PRS.AdressenEntity adressenEntity, Infrastructure.Data.Entities.Tables.PRS.KundenEntity kundenEntity)
		{
			AdressId = adressenEntity.Nr;
			Number = adressenEntity.Kundennummer.HasValue ? adressenEntity.Kundennummer.Value : -1;
			AdressMailboxZipCode = adressenEntity.PLZ_Postfach;
			AdressMailbox = adressenEntity.Postfach;
			AdressMailboxIsPreferred = adressenEntity.Postfach_bevorzugt.HasValue ? adressenEntity.Postfach_bevorzugt.Value : false;
			AdressPhoneNumber = adressenEntity.Telefon;
			AdressFaxNumber = adressenEntity.Fax;
			AdressEmailAdress = adressenEntity.EMail;
			AdressWebsite = adressenEntity.WWW;
			AdressNote = adressenEntity.Bemerkung;
			LanguageId = kundenEntity?.Sprache ?? 1;
			OrderAddress = kundenEntity?.OrderAddress ?? "";
			Isarchived = kundenEntity?.gesperrt_für_weitere_Lieferungen ?? false;
		}
		public Infrastructure.Data.Entities.Tables.PRS.AdressenEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity
			{
				Nr = AdressId,
				Kundennummer = Number,
				PLZ_Postfach = AdressMailboxZipCode,
				Postfach = AdressMailbox,
				Postfach_bevorzugt = AdressMailboxIsPreferred,
				Telefon = AdressPhoneNumber,
				Fax = AdressFaxNumber,
				EMail = AdressEmailAdress,
				WWW = AdressWebsite,
				Bemerkung = AdressNote,
			};
		}
	}
}
