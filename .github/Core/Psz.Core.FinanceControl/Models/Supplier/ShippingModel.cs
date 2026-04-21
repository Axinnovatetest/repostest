using System;

namespace Psz.Core.FinanceControl.Models.Supplier
{
	public class ShippingModel
	{
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
		public ShippingModel(Infrastructure.Data.Entities.Tables.FNC.LieferantenEntity supplierEntity,
			Infrastructure.Data.Entities.Tables.FNC.AdressenEntity addressEntity)
		{
			if(supplierEntity != null)
			{
				this.ShippingWeekDay = supplierEntity.Wochentag_Anlieferung;
				this.ShippingMethod = supplierEntity.Versandart;
				this.ShippingCosts = Convert.ToDecimal(supplierEntity.Versandkosten ?? 0);
				this.ExpressSurcharge = Convert.ToDecimal(supplierEntity.Eilzuschlag ?? 0);
				this.FreightAllowance = Convert.ToDecimal(supplierEntity.Frachtfreigrenze ?? 0);
			}

			//if (addressEntity != null)
			//{
			//    this.MondayIsDeliveryDay = addressEntity.Montag_Anliefertag ?? false;
			//    this.TuesdayIsDeliveryDay = addressEntity.Dienstag_Anliefertag ?? false;
			//    this.WednesdayIsDeliveryDay = addressEntity.Mittwoch_Anliefertag ?? false;
			//    this.ThursdayIsDeliveryDay = addressEntity.Donnerstag_Anliefertag ?? false;
			//    this.FridayIsDeliveryDay = addressEntity.Freitag_Anliefertag ?? false;
			//}
		}
		public ShippingModel(Infrastructure.Data.Entities.Tables.FNC.KundenEntity kundenEntity,
			Infrastructure.Data.Entities.Tables.FNC.AdressenEntity addressEntity)
		{
			if(kundenEntity != null)
			{
				this.ShippingWeekDay = ""; //addressEntity.shi;
				this.ShippingMethod = kundenEntity.Versandart;
				this.ShippingCosts = 0; //Convert.ToDecimal(kundenEntity.sh ?? 0);
				this.ExpressSurcharge = Convert.ToDecimal(kundenEntity.Eilzuschlag ?? 0);
				this.FreightAllowance = 0; // Convert.ToDecimal(kundenEntity.Frachtfreigrenze ?? 0);
			}

			//if (addressEntity != null)
			//{
			//    this.MondayIsDeliveryDay = addressEntity.Montag_Anliefertag ?? false;
			//    this.TuesdayIsDeliveryDay = addressEntity.Dienstag_Anliefertag ?? false;
			//    this.WednesdayIsDeliveryDay = addressEntity.Mittwoch_Anliefertag ?? false;
			//    this.ThursdayIsDeliveryDay = addressEntity.Donnerstag_Anliefertag ?? false;
			//    this.FridayIsDeliveryDay = addressEntity.Freitag_Anliefertag ?? false;
			//}
		}
	}
}
