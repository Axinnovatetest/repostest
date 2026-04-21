namespace Psz.Core.Common.Enums
{
	public abstract class AnalyseTimePointEnums
	{
		public enum DataTypes: int
		{
			Custom = 0,
			Week = 1,
			Month = 2,
			Quarter = 3,
			Year = 4
		}
		public static DataTypes DataTypeFromKey(string key)
		{
			switch(key.ToLower())
			{
				default:
				case "custom":
					return DataTypes.Custom;
				case "week":
					return DataTypes.Week;
				case "month":
					return DataTypes.Month;
				case "quarter":
					return DataTypes.Quarter;
				case "year":
					return DataTypes.Year;
			}
		}
	}
}
