using System;

namespace Psz.Core.BaseData.Models.Supplier
{
	public class SupplierShippingModel
	{
		public int Id { get; set; }
		public int Number { get; set; }
		public int AdressId { get; set; }
		public bool? Isarchived { get; set; }
		public string ShippingWeekDay { get; set; }
		public string ShippingMethod { get; set; }//
		public decimal? ShippingCosts { get; set; }
		public decimal? ExpressSurcharge { get; set; }//
		public decimal? FreightAllowance { get; set; }//
		public bool MondayIsDeliveryDay { get; set; }
		public bool TuesdayIsDeliveryDay { get; set; }
		public bool WednesdayIsDeliveryDay { get; set; }
		public bool ThursdayIsDeliveryDay { get; set; }
		public bool FridayIsDeliveryDay { get; set; }

		public SupplierShippingModel()
		{

		}

		public SupplierShippingModel(Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity supplierEntity,
			Infrastructure.Data.Entities.Tables.PRS.AdressenEntity addressEntity)
		{
			if(supplierEntity != null)
			{
				this.Id = supplierEntity.Nr;
				Isarchived = (Infrastructure.Data.Access.Tables.BSD.LieferantenExtensionAccess.GetByLieferantenNr(Id) != null) ? Infrastructure.Data.Access.Tables.BSD.LieferantenExtensionAccess.GetByLieferantenNr(Id).IsArchived : false;
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
				//
				Frachtfreigrenze = float.TryParse(FreightAllowance.ToString(), out var val1) ? val1 : 0,
				Eilzuschlag = float.TryParse(ExpressSurcharge.ToString(), out var val2) ? val2 : 0,
				Versandart = ShippingMethod,
			};
		}
	}
}
