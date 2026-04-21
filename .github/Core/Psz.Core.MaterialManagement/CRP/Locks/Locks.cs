namespace Psz.Core.MaterialManagement.CRP
{
	public class Locks
	{
		public static object CapacityLock = new object();
		public static object CapacityPlanLock = new object();
		public static object HolidayLock = new object();
		public static object OrdersArticleReadLock = new object();
		public static object OrdersReadLock = new object();
		public static object ArticlesReadLock = new object();
		public static object OrdersAllReadLock = new object();

	}
}
