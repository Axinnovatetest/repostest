using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class WahrungenAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.WahrungenEntity Get(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Währungen] WHERE [Nr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", nr);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.WahrungenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.WahrungenEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Währungen]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.WahrungenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.WahrungenEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.WahrungenEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.WahrungenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.WahrungenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.WahrungenEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = "SELECT * FROM [Währungen] WHERE [Nr] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.WahrungenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.WahrungenEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.WahrungenEntity item)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Währungen] "
					+ " ([Betrag Fremdwährung],[Dezimalstellen],[entspricht DM],[EU],[Land],[Stand],[Symbol],[Währung]) "
					+ " VALUES "
					+ " (@Betrag_Fremdwährung,@Dezimalstellen,@entspricht_DM,@EU,@Land,@Stand,@Symbol,@Währung); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("Betrag_Fremdwährung", item.Betrag_Fremdwahrung == null ? (object)DBNull.Value : item.Betrag_Fremdwahrung);
					sqlCommand.Parameters.AddWithValue("Dezimalstellen", item.Dezimalstellen == null ? (object)DBNull.Value : item.Dezimalstellen);
					sqlCommand.Parameters.AddWithValue("entspricht_DM", item.Entspricht_DM == null ? (object)DBNull.Value : item.Entspricht_DM);
					sqlCommand.Parameters.AddWithValue("EU", item.EU == null ? (object)DBNull.Value : item.EU);
					sqlCommand.Parameters.AddWithValue("Land", item.Land == null ? (object)DBNull.Value : item.Land);
					sqlCommand.Parameters.AddWithValue("Stand", item.Stand == null ? (object)DBNull.Value : item.Stand);
					sqlCommand.Parameters.AddWithValue("Symbol", item.Symbol == null ? (object)DBNull.Value : item.Symbol);
					sqlCommand.Parameters.AddWithValue("Währung", item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}

				sqlTransaction.Commit();

				return response;
			}
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.WahrungenEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "UPDATE [Währungen] SET [Betrag Fremdwährung]=@Betrag_Fremdwährung, [Dezimalstellen]=@Dezimalstellen, [entspricht DM]=@entspricht_DM, [EU]=@EU, [Land]=@Land, [Stand]=@Stand, [Symbol]=@Symbol, [Währung]=@Währung WHERE [Nr]=@Nr";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
				sqlCommand.Parameters.AddWithValue("Betrag_Fremdwährung", item.Betrag_Fremdwahrung == null ? (object)DBNull.Value : item.Betrag_Fremdwahrung);
				sqlCommand.Parameters.AddWithValue("Dezimalstellen", item.Dezimalstellen == null ? (object)DBNull.Value : item.Dezimalstellen);
				sqlCommand.Parameters.AddWithValue("entspricht_DM", item.Entspricht_DM == null ? (object)DBNull.Value : item.Entspricht_DM);
				sqlCommand.Parameters.AddWithValue("EU", item.EU == null ? (object)DBNull.Value : item.EU);
				sqlCommand.Parameters.AddWithValue("Land", item.Land == null ? (object)DBNull.Value : item.Land);
				sqlCommand.Parameters.AddWithValue("Stand", item.Stand == null ? (object)DBNull.Value : item.Stand);
				sqlCommand.Parameters.AddWithValue("Symbol", item.Symbol == null ? (object)DBNull.Value : item.Symbol);
				sqlCommand.Parameters.AddWithValue("Währung", item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}

		public static int Delete(int nr)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Währungen] WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Nr", nr);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE;
				int results = 0;
				if(ids.Count <= maxParamsNumber)
				{
					results = delete(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += delete(ids.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					results += delete(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber));
				}
			}
			return -1;
		}
		private static int delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					string query = "DELETE FROM [Währungen] WHERE [Nr] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.BSD.WahrungenEntity> GetCurrencies()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Währungen] ORDER BY Land";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.WahrungenEntity>();
			}
		}
		public static Infrastructure.Data.Entities.Tables.BSD.WahrungenEntity GetBySymbole(string symbole)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Währungen] WHERE [Symbol]=@symbole";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("symbole", symbole);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.WahrungenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.BSD.WahrungenEntity Get(int nr, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();
			string query = "SELECT * FROM [Währungen] WHERE [Nr]=@Id";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("Id", nr);

			DbExecution.Fill(sqlCommand, dataTable);


			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.WahrungenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.BSD.WahrungenEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.BSD.WahrungenEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.BSD.WahrungenEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
