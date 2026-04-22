using System.ComponentModel;

namespace Psz.Core.Purchase.Enums
{
	public class StockWarningEnums
	{
		public enum StockWarningsUnits
		{
			[Description("WS-TN")]
			WS_TN = 42,
			[Description("GZ")]
			GZ = 102,
			[Description("CZ")]
			CZ = 6,
			[Description("AL")]
			AL = 26,
			[Description("DE")]
			DE = 15
		}
		public enum StcoWarningCountries
		{
			[Description("Albania")]
			Albania = 1,
			[Description("Czech Republic")]
			Czech_Republic = 2,
			[Description("Germany")]
			Germany = 3,
			[Description("Tunisia")]
			Tunisia = 4,
		}
		public static List<int> GetWarehousesFromUnit(StockWarningsUnits? unit)
		{
			return unit switch
			{
				StockWarningsUnits.WS_TN => new List<int> { 41, 42 },
				StockWarningsUnits.GZ => new List<int> { 101, 102 },
				StockWarningsUnits.CZ => new List<int> { 3, 6 },
				StockWarningsUnits.AL => new List<int> { 24, 26 },
				StockWarningsUnits.DE => new List<int> { 15, 8 },
				_ => new List<int> { },
			};
		}
	}
}