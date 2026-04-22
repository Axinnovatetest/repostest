namespace Psz.Core.Apps.Settings
{
	public class Locks
	{
		public static object AccessProfilesLock { get; set; } = new object();
		public static object UsersLock { get; set; } = new object();
	}
}
