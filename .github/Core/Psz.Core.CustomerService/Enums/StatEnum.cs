using System.ComponentModel;

namespace Psz.Core.CustomerService.Enums
{
	public class StatEnum
	{
		public enum StatPayment
		{
			[Description("Rechnung")]
			Rechnung,
			[Description("Vorkasse")]
			Vorkasse,
			[Description("Vorauskasse")]
			Vorauskasse,
			[Description("Gutschriftverfahren")]
			Gutschriftverfahren,
			[Description("Lastschrift")]
			Lastschrift,
			[Description("Überweisung direkt")]
			Überweisungdirekt,
		}
		public enum StatEdi
		{
			[Description("EDI_Order_Neu")]
			Edi = 1,
			[Description("EDI_Order_Neu")]
			NotEdi = 0,
		}

		public enum InsideSalesStatusEnums
		{
			[Description("InProgress")]
			InProgress = 0,
			[Description("Bestätigung")]
			Bestatigung = 1,
			[Description("Change")]
			Change = 2
		}

	}
}
