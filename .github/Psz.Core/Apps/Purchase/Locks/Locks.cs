namespace Psz.Core.Apps.Purchase
{
	public class Locks
	{
		public static object DocumentsLock { get; set; } = new object();
		public static object OrdersLock { get; set; } = new object();
		public static object OrderItemsLock { get; set; } = new object();
		public static object CustomersLock { get; set; } = new object();
		public static object ProductionLock { get; set; } = new object();
		public static object DeliveryNotesLock { get; set; } = new object();
		public static object FALock { get; set; } = new object();
	}
}
