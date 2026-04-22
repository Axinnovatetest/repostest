namespace Psz.Core.CustomerService.Locks
{
	public class Locks
	{
		public static object DocumentsLock { get; set; } = new object();
		public static System.Collections.Concurrent.ConcurrentDictionary<int, object> FACreateLock { get; set; } = new System.Collections.Concurrent.ConcurrentDictionary<int, object>();
		public static System.Collections.Concurrent.ConcurrentDictionary<int, object> ArticleEditLock { get; set; } = new System.Collections.Concurrent.ConcurrentDictionary<int, object>();
		public static object OrdersLock { get; set; } = new object();
		public static System.Collections.Concurrent.ConcurrentDictionary<int, object> RahmenLock { get; set; } = new System.Collections.Concurrent.ConcurrentDictionary<int, object>();
	}
}
