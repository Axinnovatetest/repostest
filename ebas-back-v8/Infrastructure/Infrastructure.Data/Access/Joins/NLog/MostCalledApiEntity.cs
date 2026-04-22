

namespace Psz.Core.Support.Models
{
	public class MostCalledApiEntity
	{
		public int Count { get; set; }
		public string MostCalledApi { get; set; }
		public int TotalApiCount { get; set; }

		public MostCalledApiEntity(DataRow dataRow)
		{
			Count = Convert.ToInt32(dataRow["MostCalledCount"]);
			MostCalledApi = Convert.ToString(dataRow["MemberName"]);
			TotalApiCount = Convert.ToInt32(dataRow["TotalCount"]);
		}
	}
}
