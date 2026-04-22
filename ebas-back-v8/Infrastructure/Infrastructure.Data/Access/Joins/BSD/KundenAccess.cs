using System.Data;
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

				return int.TryParse((DbExecution.ExecuteScalar(sqlCommand) ?? -1).ToString(), out var _id) ? _id : 0;
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

				return int.TryParse((DbExecution.ExecuteScalar(sqlCommand) ?? -1).ToString(), out var _id) ? _id : 0;
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

				return int.TryParse((DbExecution.ExecuteScalar(sqlCommand) ?? -1).ToString(), out var _id) ? _id : 0;
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

				return int.TryParse((DbExecution.ExecuteScalar(sqlCommand) ?? -1).ToString(), out var _id) ? _id : 0;
			}
		}

		public static int GetConfirmationsCountForKonditionUpdate(int kundenNr, string newConditions = null)
		{
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand($"SELECT COUNT(DISTINCT a.Nr) FROM Angebote a WHERE a.[Kunden-Nr]=@kundenNr AND a.[Typ]=@typ AND isnull(a.erledigt,0)=0 AND isnull(a.gebucht,0)=1{(newConditions is null ? "" : $" AND trim(isnull(a.Konditionen,''))<>TRIM('{newConditions}')")}", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("typ", Tables.PRS.AngeboteAccess.TYP_CONFIRMATION);
				sqlCommand.Parameters.AddWithValue("kundenNr", kundenNr);

				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand)?.ToString(), out var x) ? x : 0;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> GetConfirmationsForKonditionUpdate(int kundenNr, string newConditions, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			using(var sqlCommand = new SqlCommand($"SELECT a.* FROM Angebote a WHERE a.[Kunden-Nr]=@kundenNr AND a.[Typ]=@typ AND isnull(a.erledigt,0)=0 AND isnull(a.gebucht,0)=1{(newConditions is null ? "" : $" AND trim(isnull(a.Konditionen,''))<>TRIM('{newConditions}')")}", connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("typ", Tables.PRS.AngeboteAccess.TYP_CONFIRMATION);
				sqlCommand.Parameters.AddWithValue("kundenNr", kundenNr);

				var selectAdapter = new SqlDataAdapter(sqlCommand);
				selectAdapter.Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			}
		}
		public static int UpdateConfirmationForKonditionUpdate(int kundenNr, string newConditions, SqlConnection connection, SqlTransaction transaction)
		{
			using(var sqlCommand = new SqlCommand($"UPDATE a SET a.Konditionen=TRIM('{newConditions}') FROM Angebote a WHERE a.[Kunden-Nr]=@kundenNr AND a.[Typ]=@typ AND isnull(a.erledigt,0)=0 AND isnull(a.gebucht,0)=1{(newConditions is null ? "" : $" AND trim(isnull(a.Konditionen,''))<>TRIM('{newConditions}')")}", connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("typ", Tables.PRS.AngeboteAccess.TYP_CONFIRMATION);
				sqlCommand.Parameters.AddWithValue("kundenNr", kundenNr);

				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand)?.ToString(), out var x) ? x : 0;
			}
		}
	}
}
