using System;

namespace Psz.Core.BaseData.Models.Supplier
{
	public class ShippingModel
	{
		public int Id { get; set; }
		public int Number { get; set; }
		public int AdressId { get; set; }
		public bool? Isarchived { get; set; }
		public string ShippingWeekDay { get; set; }
		public string ShippingMethod { get; set; }
		public decimal? ShippingCosts { get; set; }
		public decimal? ExpressSurcharge { get; set; }
		public decimal? FreightAllowance { get; set; }

		public bool MondayIsDeliveryDay { get; set; }
		public bool TuesdayIsDeliveryDay { get; set; }
		public bool WednesdayIsDeliveryDay { get; set; }
		public bool ThursdayIsDeliveryDay { get; set; }
		public bool FridayIsDeliveryDay { get; set; }

		public ShippingModel()
		{ }
		public ShippingModel(Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity supplierEntity,
			Infrastructure.Data.Entities.Tables.PRS.AdressenEntity addressEntity)
		{
			if(supplierEntity != null)
			{
				this.Id = supplierEntity.Nr;
				Isarchived = (Infrastructure.Data.Access.Tables.BSD.LieferantenExtensionAccess.Get(Id) != null) ? Infrastructure.Data.Access.Tables.BSD.LieferantenExtensionAccess.Get(Id).IsArchived : false;
				this.AdressId = supplierEntity.Nummer.HasValue ? supplierEntity.Nummer.Value : -1;
				this.ShippingWeekDay = supplierEntity.Wochentag_Anlieferung;
				this.ShippingMethod = supplierEntity.Versandart;
				this.ShippingCosts = Convert.ToDecimal(supplierEntity.Versandkosten ?? 0);
				this.ExpressSurcharge = Convert.ToDecimal(supplierEntity.Eilzuschlag ?? 0);
				this.FreightAllowance = Convert.ToDecimal(supplierEntity.Frachtfreigrenze ?? 0);
			}

			if(addressEntity != null)
			{
				this.Number = addressEntity.Lieferantennummer.HasValue ? addressEntity.Lieferantennummer.Value : -1;
				this.MondayIsDeliveryDay = addressEntity.Montag_Anliefertag ?? false;
				this.TuesdayIsDeliveryDay = addressEntity.Dienstag_Anliefertag ?? false;
				this.WednesdayIsDeliveryDay = addressEntity.Mittwoch_Anliefertag ?? false;
				this.ThursdayIsDeliveryDay = addressEntity.Donnerstag_Anliefertag ?? false;
				this.FridayIsDeliveryDay = addressEntity.Freitag_Anliefertag ?? false;
			}
		}
		public ShippingModel(Infrastructure.Data.Entities.Tables.PRS.KundenEntity kundenEntity,
			Infrastructure.Data.Entities.Tables.PRS.AdressenEntity addressEntity)
		{
			if(kundenEntity != null)
			{
				this.Id = kundenEntity.Nr;
				this.AdressId = kundenEntity.Nummer.HasValue ? kundenEntity.Nummer.Value : -1;
				this.ShippingWeekDay = ""; //addressEntity.shi;
				this.ShippingMethod = kundenEntity.Versandart;
				this.ShippingCosts = 0; //Convert.ToDecimal(kundenEntity.sh ?? 0);
				this.ExpressSurcharge = Convert.ToDecimal(kundenEntity.Eilzuschlag ?? 0);
				this.FreightAllowance = 0; // Convert.ToDecimal(kundenEntity.Frachtfreigrenze ?? 0);
			}

			if(addressEntity != null)
			{
				this.Number = addressEntity.Kundennummer.HasValue ? addressEntity.Kundennummer.Value : -1;
				this.MondayIsDeliveryDay = addressEntity.Montag_Anliefertag ?? false;
				this.TuesdayIsDeliveryDay = addressEntity.Dienstag_Anliefertag ?? false;
				this.WednesdayIsDeliveryDay = addressEntity.Mittwoch_Anliefertag ?? false;
				this.ThursdayIsDeliveryDay = addressEntity.Donnerstag_Anliefertag ?? false;
				this.FridayIsDeliveryDay = addressEntity.Freitag_Anliefertag ?? false;
			}
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

		public Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity ToLieferantenEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity
			{
				Nr = Id,
				Wochentag_Anlieferung = ShippingWeekDay,
				Versandkosten = float.TryParse(ShippingCosts.ToString(), out var val) ? val : 0,
			};
		}
	}
}
