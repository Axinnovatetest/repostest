namespace Psz.Core.SharedKernel.DataSettings
{
	public class MainDataBase
	{
		private static string _connectionString { get; set; }
		public static string ConnectionString { get { return _connectionString; } }

		public static void SetConnectionString(string connectionString)
		{
			_connectionString = connectionString;
		}
	}
}
