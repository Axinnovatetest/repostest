using System.ComponentModel;

namespace Psz.Core.CRP.Enums
{
	public class FAEnums
	{
		public enum FaStatus
		{
			[Description("Offen")]
			Offen = 0,
			[Description("erledigt")]
			erledigt = 1,
			[Description("storno")]
			storno = 2,
		}
		public enum FaLands
		{
			[Description("Albania")]
			AL = 26,
			[Description("CZ")]
			CZ = 6,
			[Description("TN")]
			TN = 7,
			[Description("BETN")]
			BETN = 60,
			[Description("WS")]
			WS = 42,
			[Description("GZTN")]
			GZTN = 102,
		}
		public enum AnalyseLands
		{
			[Description("TN")]
			TN = 1,
			[Description("WS")]
			WS = 2,
			[Description("CZ")]
			CZ = 3,
			[Description("AL")]
			AL = 4,
			[Description("DE")]
			DE = 6,
			[Description("BETN")]
			BETN = 7,
			[Description("GZTN")]
			GZTN = 8,
		}
		public enum ItemType
		{
			[Description("Prototype")]
			Prototype = 1,
			[Description("First Sample")]
			Erstmuster = 2,
			[Description("Null Serie")]
			Nullserie = 3,
			[Description("Serie")]
			Serie = 4,
		}
	}
}