namespace Psz.Core.FinanceControl
{
	public static class Locks
	{
		public static readonly object ProjectLock = new object();
		public static readonly object OrderLock = new object();
		public static readonly object StatsLock = new object();
	}
}
