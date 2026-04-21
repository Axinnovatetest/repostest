using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Joins.BSD
{
	public class KundenAccess
	{
		public static int GetPrevCustomer(int Number)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"select top 1 k.Nr from adressen as a join Kunden as k on k.nummer=a.Nr where Kundennummer<@id order by Kundennummer desc";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", Number);

				return int.TryParse((sqlCommand.ExecuteScalar() ?? -1).ToString(), out var _id) ? _id : 0;
			}
		}
		public static int GetNextCustomer(int Number)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"select top 1 k.Nr from adressen as a join Kunden as k on k.nummer=a.Nr where Kundennummer>@id order by Kundennummer asc";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", Number);

				return int.TryParse((sqlCommand.ExecuteScalar() ?? -1).ToString(), out var _id) ? _id : 0;
			}
		}
		public static int GetPrevSupplier(int Number)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"select top 1 l.Nr from adressen as a join Lieferanten as l on l.nummer=a.Nr where Lieferantennummer<@id order by Lieferantennummer desc";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", Number);

				return int.TryParse((sqlCommand.ExecuteScalar() ?? -1).ToString(), out var _id) ? _id : 0;
			}
		}
		public static int GetNextSupplier(int Number)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"select top 1 l.Nr from adressen as a join Lieferanten as l on l.nummer=a.Nr where Lieferantennummer>@id order by Lieferantennummer asc";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", Number);

				return int.TryParse((sqlCommand.ExecuteScalar() ?? -1).ToString(), out var _id) ? _id : 0;
			}
		}
	}
}
