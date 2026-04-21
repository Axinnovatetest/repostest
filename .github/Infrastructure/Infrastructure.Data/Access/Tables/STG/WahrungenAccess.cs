using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.STG
{
	public class WahrungenAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.STG.WahrungenEntity Get(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Währungen] WHERE [Nr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", nr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.STG.WahrungenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.STG.WahrungenEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Währungen]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.STG.WahrungenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.STG.WahrungenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.STG.WahrungenEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.STG.WahrungenEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.STG.WahrungenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.STG.WahrungenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.STG.WahrungenEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
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

					sqlCommand.CommandText = $"SELECT * FROM [Währungen] WHERE [Nr] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.STG.WahrungenEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.STG.WahrungenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.STG.WahrungenEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.STG.WahrungenEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Währungen] ([Betrag Fremdwährung],[Dezimalstellen],[entspricht DM],[EU],[Land],[Stand],[Symbol],[Währung])  VALUES (@Betrag_Fremdwährung,@Dezimalstellen,@entspricht_DM,@EU,@Land,@Stand,@Symbol,@Währung); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Betrag_Fremdwährung", item.Betrag_Fremdwahrung == null ? (object)DBNull.Value : item.Betrag_Fremdwahrung);
					sqlCommand.Parameters.AddWithValue("Dezimalstellen", item.Dezimalstellen == null ? (object)DBNull.Value : item.Dezimalstellen);
					sqlCommand.Parameters.AddWithValue("entspricht_DM", item.entspricht_DM == null ? (object)DBNull.Value : item.entspricht_DM);
					sqlCommand.Parameters.AddWithValue("EU", item.EU == null ? (object)DBNull.Value : item.EU);
					sqlCommand.Parameters.AddWithValue("Land", item.Land == null ? (object)DBNull.Value : item.Land);
					sqlCommand.Parameters.AddWithValue("Stand", item.Stand == null ? (object)DBNull.Value : item.Stand);
					sqlCommand.Parameters.AddWithValue("Symbol", item.Symbol == null ? (object)DBNull.Value : item.Symbol);
					sqlCommand.Parameters.AddWithValue("Währung", item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.STG.WahrungenEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = insert(items);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += insert(items.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					results += insert(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber));
				}
				return results;
			}

			return -1;
		}
		private static int insert(List<Infrastructure.Data.Entities.Tables.STG.WahrungenEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [Währungen] ([Betrag Fremdwährung],[Dezimalstellen],[entspricht DM],[EU],[Land],[Stand],[Symbol],[Währung]) VALUES ( "

							+ "@Betrag_Fremdwährung" + i + ","
							+ "@Dezimalstellen" + i + ","
							+ "@entspricht_DM" + i + ","
							+ "@EU" + i + ","
							+ "@Land" + i + ","
							+ "@Stand" + i + ","
							+ "@Symbol" + i + ","
							+ "@Währung" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Betrag_Fremdwährung" + i, item.Betrag_Fremdwahrung == null ? (object)DBNull.Value : item.Betrag_Fremdwahrung);
						sqlCommand.Parameters.AddWithValue("Dezimalstellen" + i, item.Dezimalstellen == null ? (object)DBNull.Value : item.Dezimalstellen);
						sqlCommand.Parameters.AddWithValue("entspricht_DM" + i, item.entspricht_DM == null ? (object)DBNull.Value : item.entspricht_DM);
						sqlCommand.Parameters.AddWithValue("EU" + i, item.EU == null ? (object)DBNull.Value : item.EU);
						sqlCommand.Parameters.AddWithValue("Land" + i, item.Land == null ? (object)DBNull.Value : item.Land);
						sqlCommand.Parameters.AddWithValue("Stand" + i, item.Stand == null ? (object)DBNull.Value : item.Stand);
						sqlCommand.Parameters.AddWithValue("Symbol" + i, item.Symbol == null ? (object)DBNull.Value : item.Symbol);
						sqlCommand.Parameters.AddWithValue("Währung" + i, item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.STG.WahrungenEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Währungen] SET [Betrag Fremdwährung]=@Betrag_Fremdwährung, [Dezimalstellen]=@Dezimalstellen, [entspricht DM]=@entspricht_DM, [EU]=@EU, [Land]=@Land, [Stand]=@Stand, [Symbol]=@Symbol, [Währung]=@Währung WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
				sqlCommand.Parameters.AddWithValue("Betrag_Fremdwährung", item.Betrag_Fremdwahrung == null ? (object)DBNull.Value : item.Betrag_Fremdwahrung);
				sqlCommand.Parameters.AddWithValue("Dezimalstellen", item.Dezimalstellen == null ? (object)DBNull.Value : item.Dezimalstellen);
				sqlCommand.Parameters.AddWithValue("entspricht_DM", item.entspricht_DM == null ? (object)DBNull.Value : item.entspricht_DM);
				sqlCommand.Parameters.AddWithValue("EU", item.EU == null ? (object)DBNull.Value : item.EU);
				sqlCommand.Parameters.AddWithValue("Land", item.Land == null ? (object)DBNull.Value : item.Land);
				sqlCommand.Parameters.AddWithValue("Stand", item.Stand == null ? (object)DBNull.Value : item.Stand);
				sqlCommand.Parameters.AddWithValue("Symbol", item.Symbol == null ? (object)DBNull.Value : item.Symbol);
				sqlCommand.Parameters.AddWithValue("Währung", item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.STG.WahrungenEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = update(items);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += update(items.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					results += update(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber));
				}

				return results;
			}

			return -1;
		}
		private static int update(List<Infrastructure.Data.Entities.Tables.STG.WahrungenEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [Währungen] SET "

							+ "[Betrag Fremdwährung]=@Betrag_Fremdwährung" + i + ","
							+ "[Dezimalstellen]=@Dezimalstellen" + i + ","
							+ "[entspricht DM]=@entspricht_DM" + i + ","
							+ "[EU]=@EU" + i + ","
							+ "[Land]=@Land" + i + ","
							+ "[Stand]=@Stand" + i + ","
							+ "[Symbol]=@Symbol" + i + ","
							+ "[Währung]=@Währung" + i + " WHERE [Nr]=@Nr" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
						sqlCommand.Parameters.AddWithValue("Betrag_Fremdwährung" + i, item.Betrag_Fremdwahrung == null ? (object)DBNull.Value : item.Betrag_Fremdwahrung);
						sqlCommand.Parameters.AddWithValue("Dezimalstellen" + i, item.Dezimalstellen == null ? (object)DBNull.Value : item.Dezimalstellen);
						sqlCommand.Parameters.AddWithValue("entspricht_DM" + i, item.entspricht_DM == null ? (object)DBNull.Value : item.entspricht_DM);
						sqlCommand.Parameters.AddWithValue("EU" + i, item.EU == null ? (object)DBNull.Value : item.EU);
						sqlCommand.Parameters.AddWithValue("Land" + i, item.Land == null ? (object)DBNull.Value : item.Land);
						sqlCommand.Parameters.AddWithValue("Stand" + i, item.Stand == null ? (object)DBNull.Value : item.Stand);
						sqlCommand.Parameters.AddWithValue("Symbol" + i, item.Symbol == null ? (object)DBNull.Value : item.Symbol);
						sqlCommand.Parameters.AddWithValue("Währung" + i, item.Wahrung == null ? (object)DBNull.Value : item.Wahrung);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int nr)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Währungen] WHERE [Nr]=@Nr";
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
				int maxParamsNumber = Settings.MAX_BATCH_SIZE;
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
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
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

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.STG.WahrungenEntity> GetByName(string name)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Währungen] WHERE [Währung]=@name";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("name", name);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.STG.WahrungenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.STG.WahrungenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.STG.WahrungenEntity> GetBySymbol(string symbol)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Währungen] WHERE [Symbol]=@symbol";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("symbol", symbol);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.STG.WahrungenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.STG.WahrungenEntity>();
			}
		}

		#endregion
	}
}
