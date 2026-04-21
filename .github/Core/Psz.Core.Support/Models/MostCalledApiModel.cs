namespace Psz.Core.Support.Models
{
	public class MostCalledApiModel
	{
		public int Count { get; set; }
		public KeyValuePair<string, int> MostCalledApi { get; set; }
		public KeyValuePair<string, int> LeastCalledApi { get; set; }
		public MostCalledApiModel()
		{

		}
	}
}