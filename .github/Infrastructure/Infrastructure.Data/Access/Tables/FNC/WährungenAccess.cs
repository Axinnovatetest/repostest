using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.FNC
{
	public class WahrungenAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.WahrungenEntity Get(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Währungen_Budget] WHERE [Nr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", nr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.WahrungenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.WahrungenEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Währungen_Budget]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.WahrungenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.WahrungenEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.WahrungenEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.WahrungenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.WahrungenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.WahrungenEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
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

					sqlCommand.CommandText = "SELECT * FROM [Währungen_Budget] WHERE [Nr] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.WahrungenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.WahrungenEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.WahrungenEntity item)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Währungen_Budget] "
					+ " ([Betrag Fremdwährung],[Dezimalstellen],[entspricht DM],[EU],[Land],[Stand],[Symbol],[Währung]) "
					+ " VALUES "
					+ " (@Betrag Fremdwährung,@Dezimalstellen,@entspricht DM,@EU,@Land,@Stand,@Symbol,@Währung) ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("Betrag Fremdwährung", item.Betrag_Fremdwahrung == null ? (object)DBNull.Value : item.Betrag_Fremdwahrung);
					sqlCommand.Parameters.AddWithValue("Dezimalstellen", item.Dezimalstellen == null ? (object)DBNull.Value : item.Dezimalstellen);
					sqlCommand.Parameters.AddWithValue("entspricht DM", item.Entspricht_DM == null ? (object)DBNull.Value : item.Entspricht_DM);
					sqlCommand.Parameters.AddWithValue("EU", item.EU == null ? (object)DBNull.Value : item.EU);
					sqlCommand.Parameters.AddWithValue("Land", item.Land == null ? (object)DBNull.Value : item.Land);
					sqlCommand.Parameters.AddWithValue("Stand", item.Stand == null ? (object)DBNull.Value : item.Stand);
					sqlCommand.Parameters.AddWithValue("Symbol", item.Symbol == null ? (object)DBNull.Value : item.Symbol);
					sqlCommand.Parameters.AddWithValue("Währung", item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);

					sqlCommand.ExecuteNonQuery();
				}

				using(var sqlCommand = new SqlCommand("SELECT [Nr] FROM [Währungen_Budget] WHERE [Nr] = @@IDENTITY", sqlConnection, sqlTransaction))
				{
					response = int.Parse(sqlCommand.ExecuteScalar()?.ToString() ?? "-1");
				}

				sqlTransaction.Commit();

				return response;
			}
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.WahrungenEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				string query = "UPDATE [Währungen_Budget] SET [Betrag Fremdwährung]=@Betrag Fremdwährung, [Dezimalstellen]=@Dezimalstellen, [entspricht DM]=@entspricht DM, [EU]=@EU, [Land]=@Land, [Stand]=@Stand, [Symbol]=@Symbol, [Währung]=@Währung WHERE [Nr]=@Nr";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
				sqlCommand.Parameters.AddWithValue("Betrag Fremdwährung", item.Betrag_Fremdwahrung == null ? (object)DBNull.Value : item.Betrag_Fremdwahrung);
				sqlCommand.Parameters.AddWithValue("Dezimalstellen", item.Dezimalstellen == null ? (object)DBNull.Value : item.Dezimalstellen);
				sqlCommand.Parameters.AddWithValue("entspricht DM", item.Entspricht_DM == null ? (object)DBNull.Value : item.Entspricht_DM);
				sqlCommand.Parameters.AddWithValue("EU", item.EU == null ? (object)DBNull.Value : item.EU);
				sqlCommand.Parameters.AddWithValue("Land", item.Land == null ? (object)DBNull.Value : item.Land);
				sqlCommand.Parameters.AddWithValue("Stand", item.Stand == null ? (object)DBNull.Value : item.Stand);
				sqlCommand.Parameters.AddWithValue("Symbol", item.Symbol == null ? (object)DBNull.Value : item.Symbol);
				sqlCommand.Parameters.AddWithValue("Währung", item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}

		public static int Delete(int nr)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Währungen_Budget] WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Nr", nr);

				results = sqlCommand.ExecuteNonQuery();
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
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
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

					string query = "DELETE FROM [Währungen_Budget] WHERE [Nr] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.FNC.WahrungenEntity> GetCurrencies()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Währungen_Budget] ORDER BY Land";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.WahrungenEntity>();
			}
		}
		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.FNC.WahrungenEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.WahrungenEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.WahrungenEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
