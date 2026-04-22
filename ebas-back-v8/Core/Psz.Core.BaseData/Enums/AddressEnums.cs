using System.ComponentModel;

namespace Psz.Core.BaseData.Enums
{
	public class AddressEnums
	{
		public enum Categories: int
		{
			[Description("Standard")]
			Standard = 1,
			[Description("Interessant")]
			Interessed = 2,
			[Description("Delivery Address")]
			Supplier = 3
		}

		public enum Industries: int
		{
			Customer = 1,
			Supplier = 2,
		}

		public enum LSCodeTypes
		{
			[Description("Barcode")]
			Barcode = 0,
			[Description("QRCode")]
			QRCode = 1,
		}
	}
}
