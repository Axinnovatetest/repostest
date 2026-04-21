using System.ComponentModel;

namespace Psz.Core.CustomerService.Enums
{
	public class LagerEnums
	{
		public enum KapazitatLager
		{
			[Description("CZ")]
			CZ = 1,
			[Description("TN")]
			TN = 2,
			[Description("AL")]
			AL = 3,
			[Description("BETN")]
			BETN = 4,
			[Description("WS")]
			WS = 5,
			[Description("GZTN")]
			GZTN = 6,
		}
	}
}
