

namespace Infrastructure.Data.Access.Joins.CRP
{
	public class CRPDashboardAccess
	{
		private  const int CUSTOM_SQLCOMMAND_WAITINGTIME  = 90;
		public static int GetCreatedFas(int month, int year)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"select COUNT(*) AS [Created FAs] FROM Fertigung where MONTH(Datum)=@month AND YEAR(Datum)=@year";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("month", month);
				sqlCommand.Parameters.AddWithValue("year", year);
				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var v) ? v : 0;
			}
		}
		public static int GetCancelledFas(int month, int year)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT COUNT(Nr) AS [Cancelled FAs] FROM __CTS_OrderProcesssing_Log 
								WHERE LogObject='Fertigung' 
								AND LogType='DELETIONOBJECT' 
								AND YEAR([DateTime])=@year 
								AND MONTH([DateTime])=@month";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("month", month);
				sqlCommand.Parameters.AddWithValue("year", year);
				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var v) ? v : 0;
			}
		}

		public static int GetOpenFasByYear(int year)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"select COUNT(*) AS [Open FAs By Year] from Fertigung where Kennzeichen=N'offen' AND YEAR(termin_bestätigt1)=@year
									AND Artikel_Nr NOT IN (select [Artikel-Nr] from Artikel where Artikelnummer in ('Reparatur','Analyse','Technik','Technik - N','Technik - PCN','Technik - RP'))";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("year", year);
				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var v) ? v : 0;
			}
		}
		public static int GetActiveArticlesByYear(int year)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"select COUNT(distinct A.[Artikel-Nr]) AS [Active Articles By Year] from Fertigung F inner join Artikel A on F.artikel_nr=A.[Artikel-Nr]
                               where F.Kennzeichen=N'offen' AND YEAR(termin_bestätigt1)=@year
								AND Artikel_Nr NOT IN (select [Artikel-Nr] from Artikel where Artikelnummer in ('Reparatur','Analyse','Technik','Technik - N','Technik - PCN','Technik - RP'))";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("year", year);
				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var v) ? v : 0;
			}
		}
		public static decimal GetOpenFasHoursByYear(int year)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT SUM((Zeit*Anzahl)/60) AS [Open FAs Hours] from Fertigung where YEAR(termin_bestätigt1)=@year AND Kennzeichen=N'offen'
                                AND Artikel_Nr NOT IN (select [Artikel-Nr] from Artikel where Artikelnummer in ('Reparatur','Analyse','Technik','Technik - N','Technik - PCN','Technik - RP'))";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("year", year);
				return decimal.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var v) ? v : 0m;
			}
		}
		/// <summary>
		/// it uses custom command time out (defined in the access class as constant value)
		/// </summary>
		/// <returns></returns>
		public static decimal GetTotalStockFGByYear()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT SUM(v.VK) FROM [View_Lagerbestände Analyse L_CS_wNegativ] v LEFT JOIN [Artikel] a ON a.Artikelnummer = v.Artikelnummer WHERE v.Lagerort_id NOT IN (928, 825)";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = CUSTOM_SQLCOMMAND_WAITINGTIME;
				return decimal.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var v) ? v : 0;
			}
		}
	}
}