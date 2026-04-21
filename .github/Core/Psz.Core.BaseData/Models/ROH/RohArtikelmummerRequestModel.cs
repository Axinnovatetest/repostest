using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.ROH
{
	public class RohArtikelnummerPreviewRequestModel
	{
		public int IdLevelOne { get; set; }
		public List<int> IdsLevelThree { get; set; }
		public List<KeyValuePair<int, string>> FreeTextLevelTwoValues { get; set; }
		public List<KeyValuePair<int, string>> FreeTextLevelThreeValues { get; set; }
		public List<RangesLevelTwoValues> RangesLevelTwoValues { get; set; }
	}
	public class RangesLevelTwoValues
	{
		public int IdLevelTwo { get; set; }
		public string From { get; set; }
		public string To { get; set; }
	}
}