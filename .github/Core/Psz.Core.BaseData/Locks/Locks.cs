namespace Psz.Core.BaseData
{
	public class Locks
	{
		public static System.Collections.Concurrent.ConcurrentDictionary<int, object> ArticleEditLock { get; set; } = new System.Collections.Concurrent.ConcurrentDictionary<int, object>();
		//
		public static System.Collections.Concurrent.ConcurrentDictionary<int, object> CostumerEditLock { get; set; } = new System.Collections.Concurrent.ConcurrentDictionary<int, object>();
		public static System.Collections.Concurrent.ConcurrentDictionary<int, object> SupplierEditLock { get; set; } = new System.Collections.Concurrent.ConcurrentDictionary<int, object>();
	}
}
