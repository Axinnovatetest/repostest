namespace Psz.Core.Helpers
{
	public class ReferencesHelper
	{
		public static string Convert(string subText,
			int referenceNumber,
			int referenceYear,
			int minNumberLength = 7,
			string separator = "-")
		{
			return (!string.IsNullOrEmpty(subText) ? (subText + separator) : "")
				+ referenceNumber.ToString().PadLeft(minNumberLength, '0')
				+ separator
				+ referenceYear;
		}
	}
}
