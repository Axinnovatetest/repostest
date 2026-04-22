
namespace Psz.Core.CRP.Locks
{
	public class Locks
	{
		public static System.Collections.Concurrent.ConcurrentDictionary<int, object> FACreateLock { get; set; } = new System.Collections.Concurrent.ConcurrentDictionary<int, object>();
		public static object FALock { get; set; } = new object();
		public static object DocumentsLock { get; set; } = new object();
	}
}
