using System.ComponentModel;

namespace Psz.Core.Apps.Support
{
	public class Enums
	{
		public enum FeedbackType
		{
			[Description("Error")]
			Error = 0,
			[Description("Appreciation")]
			Appreciation = 1,
		}

		public enum FeedbackModules
		{
			[Description("sld")]
			sld = 0,
			[Description("cts")]
			cts = 1,
			[Description("lgt")]
			lgt = 2,
			[Description("hmr")]
			hmr = 3,
			[Description("fnc")]
			fnc = 4,
			[Description("mtm")]
			mtm = 5,
			[Description("mtd")]
			mtd = 6,
			[Description("adm")]
			adm = 7,
		}

		public enum ItTicketStatus
		{
			[Description("Draft")]
			Draft = 0,
			[Description("Accepted")]
			Acctepted = 1,
			[Description("Refused")]
			Refused = 2,

		}
	}
}
