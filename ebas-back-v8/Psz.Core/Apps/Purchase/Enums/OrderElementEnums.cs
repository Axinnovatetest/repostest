namespace Psz.Core.Apps.Purchase.Enums
{
	public static class OrderElementEnums
	{
		internal enum OrderElementStatus: int
		{
			Original = 0,
			Changed = 1,
			Deleted = 2,
		}

		internal enum Types: int
		{
			Prototyp = 1,
			Erstmuster = 2,
			Nullserie = 3,
			Serie = 4
		}
		internal static string TypeToString(Types type)
		{
			switch(type)
			{
				default:
					return string.Empty;
				case Types.Prototyp:
					return "Prototyp";
				case Types.Erstmuster:
					return "Erstmuster";
				case Types.Nullserie:
					return "Nullserie";
				case Types.Serie:
					return "Serie";
			}
		}
	}
}
