using System.ComponentModel;

namespace Psz.Core.Common.Enums
{
	public class ArticleEnums
	{
		public enum ArticleProductionPlace
		{
			[Description("AL")]
			AL = 26,
			[Description("TN")]
			TN = 7,
			//[Description("BETN")]
			//BETN = 60,
			[Description("WS")]
			WS = 42,
			[Description("GZTN")]
			GZTN = 102,
			[Description("DE")]
			DE = 15,
			[Description("CZ")]
			CZ = 6
		}

		public enum SalesItemType
		{
			[Description("First Sample")]
			FirstSample = 0,
			[Description("Prototype")]
			Prototype = 1,
			[Description("Null Serie")]
			NullSerie = 2,
			[Description("Serie")]
			Serie = 3
		}
		// - 2023-10-23 - convert CTS values (english) to MTD values (german)
		public static SalesItemType GetItemType(string type)
		{
			type = (type ?? "").ToLower();
			return type switch
			{
				"prototype" or "prototyp" => SalesItemType.Prototype,
				"first sample" or "erstmuster" => SalesItemType.FirstSample,
				"null serie" or "nullserie" => SalesItemType.NullSerie,
				"serie" => SalesItemType.Serie,
				_ => SalesItemType.Serie
			};
		}
	}
}
