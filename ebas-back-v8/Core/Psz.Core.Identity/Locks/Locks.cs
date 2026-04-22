namespace Psz.Core.Identity.Locks
{
	public class Locks
	{
		public static object AccessProfilesLock { get; set; } = new object();
		public static object UsersLock { get; set; } = new object();
	}
}
