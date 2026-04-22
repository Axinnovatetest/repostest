namespace Psz.Core.Apps.EDI
{
	public class Locks
	{
		public static object DocumentsLock { get; set; } = new object();
		public static object OrdersLock { get; set; } = new object();
		public static object CustomersLock { get; set; } = new object();
		public static object DLF_ProductionLock { get; set; } = new object();
		public static System.Collections.Concurrent.ConcurrentDictionary<int, object> OrderElementsLock { get; set; } = new System.Collections.Concurrent.ConcurrentDictionary<int, object>();
	}
}
