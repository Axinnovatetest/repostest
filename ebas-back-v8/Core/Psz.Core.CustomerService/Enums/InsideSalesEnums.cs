using System.ComponentModel;

namespace Psz.Core.CustomerService.Enums
{
	public class InsideSalesEnums
	{

		public enum CheckTypes
		{
			[Description("Stock")]
			stock = 0,
			[Description("FA")]
			FA = 1,
			[Description("FST")]
			FST = 2,
			[Description("PRS")]
			PRS = 3,
			[Description("CRP")]
			CRP = 4,
			[Description("INS")]
			INS = 5,

		}

		public enum Warehouses
		{
			[Description("TN")]
			TN = 7,
			[Description("WS")]
			WS = 42,
			[Description("GZ")]
			GZ = 102,
			[Description("AL")]
			AL = 26,
			[Description("CZ")]
			CZ = 6,
			[Description("DE")]
			DE = 15,
		}

		public enum SecurityStockDiffrenceTypes
		{
			[Description("Untedeckung")]
			Untedeckung = 0,
			[Description("Uberdeckung")]
			Uberdeckung = 1,
			[Description("Gleiche Abdeckung")]
			GleicheAbdeckung = 2,
		}
	}
}
