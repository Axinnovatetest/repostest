namespace Psz.Core.BaseData.Models.Customer
{
	public class CustomerShippingModel
	{
		public int Id { get; set; }
		public int Number { get; set; }
		public int AdressId { get; set; }
		public bool? Isarchived { get; set; }
		public decimal? FreightAllowance { get; set; }
		public bool? MondayIsDeliveryDay { get; set; }
		public bool? TuesdayIsDeliveryDay { get; set; }
		public bool? WednesdayIsDeliveryDay { get; set; }
		public bool? ThursdayIsDeliveryDay { get; set; }
		public bool? FridayIsDeliveryDay { get; set; }
		//
		public string StandardShipping { get; set; } // Standardversand
		public int? LSADR { get; set; } // __ Address Nr
		public bool? LSADRANG { get; set; } // __ Angebote
		public bool? LSADRAUF { get; set; } // __ Auftragsbestatigungen
		public bool? LSADRGUT { get; set; } // __ Gutschritten
		public bool? LSADRPROF { get; set; } // __ Proformarechnungen
		public bool? LSADRRG { get; set; } // __ Rechnungen
		public bool? LSADRSTO { get; set; } // __ Stornierungen
		public bool? LSRG { get; set; } // __ RechnungensAdresse

		public CustomerShippingModel()
		{

		}

		public CustomerShippingModel(Infrastructure.Data.Entities.Tables.PRS.KundenEntity kundenEntity,
			Infrastructure.Data.Entities.Tables.PRS.AdressenEntity addressEntity)
		{
			if(addressEntity != null)
			{
				this.Number = addressEntity.Lieferantennummer.HasValue ? addressEntity.Lieferantennummer.Value : -1;
				this.AdressId = addressEntity.Nr;
				this.MondayIsDeliveryDay = addressEntity.Montag_Anliefertag ?? false;
				this.TuesdayIsDeliveryDay = addressEntity.Dienstag_Anliefertag ?? false;
				this.WednesdayIsDeliveryDay = addressEntity.Mittwoch_Anliefertag ?? false;
				this.ThursdayIsDeliveryDay = addressEntity.Donnerstag_Anliefertag ?? false;
				this.FridayIsDeliveryDay = addressEntity.Freitag_Anliefertag ?? false;
			}
			//
			if(kundenEntity != null)
			{
				Id = kundenEntity.Nr;
				Isarchived = (Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.GetByKundenNr(Id) != null) ? Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.GetByKundenNr(Id).IsArchived : false;
				LSADR = kundenEntity.LSADR; // __ Address Nr
				LSADRANG = kundenEntity.LSADRANG; // __ Angebote
				LSADRAUF = kundenEntity.LSADRAUF; // __ Auftragsbestatigungen
				LSADRGUT = kundenEntity.LSADRGUT; // __ Gutschritten
				LSADRPROF = kundenEntity.LSADRPROF; // __ Proformarechnungen
				LSADRRG = kundenEntity.LSADRRG; // __ Rechnungen
				LSADRSTO = kundenEntity.LSADRSTO; // __ Stornierungen
				LSRG = kundenEntity.LSRG; // __ RechnungensAdresse
				StandardShipping = kundenEntity.Standardversand; // Standardversand
			}
		}

		public Infrastructure.Data.Entities.Tables.PRS.KundenEntity ToKundenEntity()
		{
			return new Infrastructure.Data.Entities.Tables.PRS.KundenEntity
			{
				Nr = Id,
				LSADR = LSADR,
				LSADRANG = LSADRANG,
				LSADRAUF = LSADRAUF,
				LSADRGUT = LSADRGUT,
				LSADRPROF = LSADRPROF,
				LSADRRG = LSADRRG,
				LSADRSTO = LSADRSTO,
				LSRG = LSRG,
				Standardversand = StandardShipping,
			};
		}

		public Infrastructure.Data.Entities.Tables.PRS.AdressenEntity ToAdressenEntity()
		{
			return new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity
			{
				Nr = AdressId,
				Kundennummer = Number,
				Montag_Anliefertag = MondayIsDeliveryDay,
				Freitag_Anliefertag = FridayIsDeliveryDay,
				Dienstag_Anliefertag = TuesdayIsDeliveryDay,
				Donnerstag_Anliefertag = ThursdayIsDeliveryDay,
				Mittwoch_Anliefertag = WednesdayIsDeliveryDay,
			};
		}
	}
}
