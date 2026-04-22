using System.ComponentModel;

namespace Psz.Core.BaseData.Enums
{
	public class SettingsEnums
	{
		public enum CheckDelete
		{
			[Description("DiscountGroups")]
			DiscountGroups = 1,
			[Description("Conditions_assignement")]
			Conditions_assignement = 2,
			[Description("Industry")]
			Industry = 4,
			[Description("SuppliersGroups")]
			SuppliersGroups = 5,
			[Description("PaymentPractices")]
			PaymentPractices = 6,
			[Description("Currencies")]
			Currencies = 7,
			[Description("SlipCircles")]
			SlipCircles = 8,
			[Description("PricingGroups")]
			PricingGroups = 9,
			[Description("AddressTypes")]
			AddressTypes = 10,
			[Description("CustomerFrames")]
			CustomerFrames = 11,
			[Description("ShippingMethods")]
			ShippingMethods = 12,
		}
	}
}
