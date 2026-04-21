namespace Psz.Core.Tools
{
	public class Hasher
	{
		private static string GetRandomSalt()
		{
			return BCrypt.Net.BCrypt.GenerateSalt(4);
		}

		public static string HashPassword(string password)
		{
			return BCrypt.Net.BCrypt.HashPassword(password, GetRandomSalt());
		}

		public static bool ValidatePassword(string password, string correctHash)
		{
			return BCrypt.Net.BCrypt.Verify(password, correctHash);
		}
	}
}
