using System.Globalization;

namespace Psz.Core.Common.Helpers
{
	public static class Numbers
	{
		public static string getNumeral(int number)
		{
			switch(number)
			{
				case 0:
					return "Initial";
				case 1:
					return "First";
				case 2:
					return "Second";
				case 3:
					return "Third";
				case 4:
					return "Fourth";
				case 5:
					return "Fifth";
				case 6:
					return /*"Sixth"*/"Super";
				case 7:
					return "Seventh";
				case 8:
					return "Eighth";
				case 9:
					return "ninth";
				default:
					return $"{number} th";
			}
		}
	}
}