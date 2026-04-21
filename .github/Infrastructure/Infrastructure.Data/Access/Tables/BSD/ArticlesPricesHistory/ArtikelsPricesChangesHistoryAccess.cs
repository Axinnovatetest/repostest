using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Channels;


namespace Infrastructure.Data.Access.Tables.BSD.ArticlesPricesHistory
{

	public class ArtikelsPricesChangesHistoryAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [ArtikelsPricesChangesHistory] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [ArtikelsPricesChangesHistory]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [ArtikelsPricesChangesHistory] WHERE [ID] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [ArtikelsPricesChangesHistory] ([ArtikelnummerOriginal],[DB],[DB_wo],[EK],[EK_wo],[KalkulatorischeKosten],[KalkulatorischeKosten_wo],[lastupdated],[Logs],[precent],[precent_wo],[SumMaterial],[SumMaterial_wo],[VK],[VK_wo]) OUTPUT INSERTED.[ID] VALUES (@ArtikelnummerOriginal,@DB,@DB_wo,@EK,@EK_wo,@KalkulatorischeKosten,@KalkulatorischeKosten_wo,@lastupdated,@Logs,@precent,@precent_wo,@SumMaterial,@SumMaterial_wo,@VK,@VK_wo); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArtikelnummerOriginal", item.ArtikelnummerOriginal == null ? (object)DBNull.Value : item.ArtikelnummerOriginal);
					sqlCommand.Parameters.AddWithValue("DB", item.DB == null ? (object)DBNull.Value : item.DB);
					sqlCommand.Parameters.AddWithValue("DB_wo", item.DB_wo == null ? (object)DBNull.Value : item.DB_wo);
					sqlCommand.Parameters.AddWithValue("EK", item.EK == null ? (object)DBNull.Value : item.EK);
					sqlCommand.Parameters.AddWithValue("EK_wo", item.EK_wo == null ? (object)DBNull.Value : item.EK_wo);
					sqlCommand.Parameters.AddWithValue("KalkulatorischeKosten", item.KalkulatorischeKosten == null ? (object)DBNull.Value : item.KalkulatorischeKosten);
					sqlCommand.Parameters.AddWithValue("KalkulatorischeKosten_wo", item.KalkulatorischeKosten_wo == null ? (object)DBNull.Value : item.KalkulatorischeKosten_wo);
					sqlCommand.Parameters.AddWithValue("lastupdated", item.lastupdated == null ? (object)DBNull.Value : item.lastupdated);
					sqlCommand.Parameters.AddWithValue("Logs", item.Logs == null ? (object)DBNull.Value : item.Logs);
					sqlCommand.Parameters.AddWithValue("precent", item.precent == null ? (object)DBNull.Value : item.precent);
					sqlCommand.Parameters.AddWithValue("precent_wo", item.precent_wo == null ? (object)DBNull.Value : item.precent_wo);
					sqlCommand.Parameters.AddWithValue("SumMaterial", item.SumMaterial == null ? (object)DBNull.Value : item.SumMaterial);
					sqlCommand.Parameters.AddWithValue("SumMaterial_wo", item.SumMaterial_wo == null ? (object)DBNull.Value : item.SumMaterial_wo);
					sqlCommand.Parameters.AddWithValue("VK", item.VK == null ? (object)DBNull.Value : item.VK);
					sqlCommand.Parameters.AddWithValue("VK_wo", item.VK_wo == null ? (object)DBNull.Value : item.VK_wo);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 16; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity> items)
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
						query += " INSERT INTO [ArtikelsPricesChangesHistory] ([ArtikelnummerOriginal],[DB],[DB_wo],[EK],[EK_wo],[KalkulatorischeKosten],[KalkulatorischeKosten_wo],[lastupdated],[Logs],[precent],[precent_wo],[SumMaterial],[SumMaterial_wo],[VK],[VK_wo]) VALUES ( "

							+ "@ArtikelnummerOriginal" + i + ","
							+ "@DB" + i + ","
							+ "@DB_wo" + i + ","
							+ "@EK" + i + ","
							+ "@EK_wo" + i + ","
							+ "@KalkulatorischeKosten" + i + ","
							+ "@KalkulatorischeKosten_wo" + i + ","
							+ "@lastupdated" + i + ","
							+ "@Logs" + i + ","
							+ "@precent" + i + ","
							+ "@precent_wo" + i + ","
							+ "@SumMaterial" + i + ","
							+ "@SumMaterial_wo" + i + ","
							+ "@VK" + i + ","
							+ "@VK_wo" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArtikelnummerOriginal" + i, item.ArtikelnummerOriginal == null ? (object)DBNull.Value : item.ArtikelnummerOriginal);
						sqlCommand.Parameters.AddWithValue("DB" + i, item.DB == null ? (object)DBNull.Value : item.DB);
						sqlCommand.Parameters.AddWithValue("DB_wo" + i, item.DB_wo == null ? (object)DBNull.Value : item.DB_wo);
						sqlCommand.Parameters.AddWithValue("EK" + i, item.EK == null ? (object)DBNull.Value : item.EK);
						sqlCommand.Parameters.AddWithValue("EK_wo" + i, item.EK_wo == null ? (object)DBNull.Value : item.EK_wo);
						sqlCommand.Parameters.AddWithValue("KalkulatorischeKosten" + i, item.KalkulatorischeKosten == null ? (object)DBNull.Value : item.KalkulatorischeKosten);
						sqlCommand.Parameters.AddWithValue("KalkulatorischeKosten_wo" + i, item.KalkulatorischeKosten_wo == null ? (object)DBNull.Value : item.KalkulatorischeKosten_wo);
						sqlCommand.Parameters.AddWithValue("lastupdated" + i, item.lastupdated == null ? (object)DBNull.Value : item.lastupdated);
						sqlCommand.Parameters.AddWithValue("Logs" + i, item.Logs == null ? (object)DBNull.Value : item.Logs);
						sqlCommand.Parameters.AddWithValue("precent" + i, item.precent == null ? (object)DBNull.Value : item.precent);
						sqlCommand.Parameters.AddWithValue("precent_wo" + i, item.precent_wo == null ? (object)DBNull.Value : item.precent_wo);
						sqlCommand.Parameters.AddWithValue("SumMaterial" + i, item.SumMaterial == null ? (object)DBNull.Value : item.SumMaterial);
						sqlCommand.Parameters.AddWithValue("SumMaterial_wo" + i, item.SumMaterial_wo == null ? (object)DBNull.Value : item.SumMaterial_wo);
						sqlCommand.Parameters.AddWithValue("VK" + i, item.VK == null ? (object)DBNull.Value : item.VK);
						sqlCommand.Parameters.AddWithValue("VK_wo" + i, item.VK_wo == null ? (object)DBNull.Value : item.VK_wo);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [ArtikelsPricesChangesHistory] SET [ArtikelnummerOriginal]=@ArtikelnummerOriginal, [DB]=@DB, [DB_wo]=@DB_wo, [EK]=@EK, [EK_wo]=@EK_wo, [KalkulatorischeKosten]=@KalkulatorischeKosten, [KalkulatorischeKosten_wo]=@KalkulatorischeKosten_wo, [lastupdated]=@lastupdated, [Logs]=@Logs, [precent]=@precent, [precent_wo]=@precent_wo, [SumMaterial]=@SumMaterial, [SumMaterial_wo]=@SumMaterial_wo, [VK]=@VK, [VK_wo]=@VK_wo WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("ArtikelnummerOriginal", item.ArtikelnummerOriginal == null ? (object)DBNull.Value : item.ArtikelnummerOriginal);
				sqlCommand.Parameters.AddWithValue("DB", item.DB == null ? (object)DBNull.Value : item.DB);
				sqlCommand.Parameters.AddWithValue("DB_wo", item.DB_wo == null ? (object)DBNull.Value : item.DB_wo);
				sqlCommand.Parameters.AddWithValue("EK", item.EK == null ? (object)DBNull.Value : item.EK);
				sqlCommand.Parameters.AddWithValue("EK_wo", item.EK_wo == null ? (object)DBNull.Value : item.EK_wo);
				sqlCommand.Parameters.AddWithValue("KalkulatorischeKosten", item.KalkulatorischeKosten == null ? (object)DBNull.Value : item.KalkulatorischeKosten);
				sqlCommand.Parameters.AddWithValue("KalkulatorischeKosten_wo", item.KalkulatorischeKosten_wo == null ? (object)DBNull.Value : item.KalkulatorischeKosten_wo);
				sqlCommand.Parameters.AddWithValue("lastupdated", item.lastupdated == null ? (object)DBNull.Value : item.lastupdated);
				sqlCommand.Parameters.AddWithValue("Logs", item.Logs == null ? (object)DBNull.Value : item.Logs);
				sqlCommand.Parameters.AddWithValue("precent", item.precent == null ? (object)DBNull.Value : item.precent);
				sqlCommand.Parameters.AddWithValue("precent_wo", item.precent_wo == null ? (object)DBNull.Value : item.precent_wo);
				sqlCommand.Parameters.AddWithValue("SumMaterial", item.SumMaterial == null ? (object)DBNull.Value : item.SumMaterial);
				sqlCommand.Parameters.AddWithValue("SumMaterial_wo", item.SumMaterial_wo == null ? (object)DBNull.Value : item.SumMaterial_wo);
				sqlCommand.Parameters.AddWithValue("VK", item.VK == null ? (object)DBNull.Value : item.VK);
				sqlCommand.Parameters.AddWithValue("VK_wo", item.VK_wo == null ? (object)DBNull.Value : item.VK_wo);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 16; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity> items)
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
						query += " UPDATE [ArtikelsPricesChangesHistory] SET "

							+ "[ArtikelnummerOriginal]=@ArtikelnummerOriginal" + i + ","
							+ "[DB]=@DB" + i + ","
							+ "[DB_wo]=@DB_wo" + i + ","
							+ "[EK]=@EK" + i + ","
							+ "[EK_wo]=@EK_wo" + i + ","
							+ "[KalkulatorischeKosten]=@KalkulatorischeKosten" + i + ","
							+ "[KalkulatorischeKosten_wo]=@KalkulatorischeKosten_wo" + i + ","
							+ "[lastupdated]=@lastupdated" + i + ","
							+ "[Logs]=@Logs" + i + ","
							+ "[precent]=@precent" + i + ","
							+ "[precent_wo]=@precent_wo" + i + ","
							+ "[SumMaterial]=@SumMaterial" + i + ","
							+ "[SumMaterial_wo]=@SumMaterial_wo" + i + ","
							+ "[VK]=@VK" + i + ","
							+ "[VK_wo]=@VK_wo" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("ArtikelnummerOriginal" + i, item.ArtikelnummerOriginal == null ? (object)DBNull.Value : item.ArtikelnummerOriginal);
						sqlCommand.Parameters.AddWithValue("DB" + i, item.DB == null ? (object)DBNull.Value : item.DB);
						sqlCommand.Parameters.AddWithValue("DB_wo" + i, item.DB_wo == null ? (object)DBNull.Value : item.DB_wo);
						sqlCommand.Parameters.AddWithValue("EK" + i, item.EK == null ? (object)DBNull.Value : item.EK);
						sqlCommand.Parameters.AddWithValue("EK_wo" + i, item.EK_wo == null ? (object)DBNull.Value : item.EK_wo);
						sqlCommand.Parameters.AddWithValue("KalkulatorischeKosten" + i, item.KalkulatorischeKosten == null ? (object)DBNull.Value : item.KalkulatorischeKosten);
						sqlCommand.Parameters.AddWithValue("KalkulatorischeKosten_wo" + i, item.KalkulatorischeKosten_wo == null ? (object)DBNull.Value : item.KalkulatorischeKosten_wo);
						sqlCommand.Parameters.AddWithValue("lastupdated" + i, item.lastupdated == null ? (object)DBNull.Value : item.lastupdated);
						sqlCommand.Parameters.AddWithValue("Logs" + i, item.Logs == null ? (object)DBNull.Value : item.Logs);
						sqlCommand.Parameters.AddWithValue("precent" + i, item.precent == null ? (object)DBNull.Value : item.precent);
						sqlCommand.Parameters.AddWithValue("precent_wo" + i, item.precent_wo == null ? (object)DBNull.Value : item.precent_wo);
						sqlCommand.Parameters.AddWithValue("SumMaterial" + i, item.SumMaterial == null ? (object)DBNull.Value : item.SumMaterial);
						sqlCommand.Parameters.AddWithValue("SumMaterial_wo" + i, item.SumMaterial_wo == null ? (object)DBNull.Value : item.SumMaterial_wo);
						sqlCommand.Parameters.AddWithValue("VK" + i, item.VK == null ? (object)DBNull.Value : item.VK);
						sqlCommand.Parameters.AddWithValue("VK_wo" + i, item.VK_wo == null ? (object)DBNull.Value : item.VK_wo);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [ArtikelsPricesChangesHistory] WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ID", id);

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

					string query = "DELETE FROM [ArtikelsPricesChangesHistory] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [ArtikelsPricesChangesHistory] WHERE [ID]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [ArtikelsPricesChangesHistory]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();

				var sqlCommand = new SqlCommand("", connection, transaction);
				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				sqlCommand.CommandText = $"SELECT * FROM [ArtikelsPricesChangesHistory] WHERE [ID] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [ArtikelsPricesChangesHistory] ([ArtikelnummerOriginal],[DB],[DB_wo],[EK],[EK_wo],[KalkulatorischeKosten],[KalkulatorischeKosten_wo],[lastupdated],[Logs],[precent],[precent_wo],[SumMaterial],[SumMaterial_wo],[VK],[VK_wo]) OUTPUT INSERTED.[ID] VALUES (@ArtikelnummerOriginal,@DB,@DB_wo,@EK,@EK_wo,@KalkulatorischeKosten,@KalkulatorischeKosten_wo,@lastupdated,@Logs,@precent,@precent_wo,@SumMaterial,@SumMaterial_wo,@VK,@VK_wo); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ArtikelnummerOriginal", item.ArtikelnummerOriginal == null ? (object)DBNull.Value : item.ArtikelnummerOriginal);
			sqlCommand.Parameters.AddWithValue("DB", item.DB == null ? (object)DBNull.Value : item.DB);
			sqlCommand.Parameters.AddWithValue("DB_wo", item.DB_wo == null ? (object)DBNull.Value : item.DB_wo);
			sqlCommand.Parameters.AddWithValue("EK", item.EK == null ? (object)DBNull.Value : item.EK);
			sqlCommand.Parameters.AddWithValue("EK_wo", item.EK_wo == null ? (object)DBNull.Value : item.EK_wo);
			sqlCommand.Parameters.AddWithValue("KalkulatorischeKosten", item.KalkulatorischeKosten == null ? (object)DBNull.Value : item.KalkulatorischeKosten);
			sqlCommand.Parameters.AddWithValue("KalkulatorischeKosten_wo", item.KalkulatorischeKosten_wo == null ? (object)DBNull.Value : item.KalkulatorischeKosten_wo);
			sqlCommand.Parameters.AddWithValue("lastupdated", item.lastupdated == null ? (object)DBNull.Value : item.lastupdated);
			sqlCommand.Parameters.AddWithValue("Logs", item.Logs == null ? (object)DBNull.Value : item.Logs);
			sqlCommand.Parameters.AddWithValue("precent", item.precent == null ? (object)DBNull.Value : item.precent);
			sqlCommand.Parameters.AddWithValue("precent_wo", item.precent_wo == null ? (object)DBNull.Value : item.precent_wo);
			sqlCommand.Parameters.AddWithValue("SumMaterial", item.SumMaterial == null ? (object)DBNull.Value : item.SumMaterial);
			sqlCommand.Parameters.AddWithValue("SumMaterial_wo", item.SumMaterial_wo == null ? (object)DBNull.Value : item.SumMaterial_wo);
			sqlCommand.Parameters.AddWithValue("VK", item.VK == null ? (object)DBNull.Value : item.VK);
			sqlCommand.Parameters.AddWithValue("VK_wo", item.VK_wo == null ? (object)DBNull.Value : item.VK_wo);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 16; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = insertWithTransaction(items, connection, transaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += insertWithTransaction(items.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += insertWithTransaction(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber), connection, transaction);
				}
				return results;
			}

			return -1;
		}
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [ArtikelsPricesChangesHistory] ([ArtikelnummerOriginal],[DB],[DB_wo],[EK],[EK_wo],[KalkulatorischeKosten],[KalkulatorischeKosten_wo],[lastupdated],[Logs],[precent],[precent_wo],[SumMaterial],[SumMaterial_wo],[VK],[VK_wo]) VALUES ( "

						+ "@ArtikelnummerOriginal" + i + ","
						+ "@DB" + i + ","
						+ "@DB_wo" + i + ","
						+ "@EK" + i + ","
						+ "@EK_wo" + i + ","
						+ "@KalkulatorischeKosten" + i + ","
						+ "@KalkulatorischeKosten_wo" + i + ","
						+ "@lastupdated" + i + ","
						+ "@Logs" + i + ","
						+ "@precent" + i + ","
						+ "@precent_wo" + i + ","
						+ "@SumMaterial" + i + ","
						+ "@SumMaterial_wo" + i + ","
						+ "@VK" + i + ","
						+ "@VK_wo" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("ArtikelnummerOriginal" + i, item.ArtikelnummerOriginal == null ? (object)DBNull.Value : item.ArtikelnummerOriginal);
					sqlCommand.Parameters.AddWithValue("DB" + i, item.DB == null ? (object)DBNull.Value : item.DB);
					sqlCommand.Parameters.AddWithValue("DB_wo" + i, item.DB_wo == null ? (object)DBNull.Value : item.DB_wo);
					sqlCommand.Parameters.AddWithValue("EK" + i, item.EK == null ? (object)DBNull.Value : item.EK);
					sqlCommand.Parameters.AddWithValue("EK_wo" + i, item.EK_wo == null ? (object)DBNull.Value : item.EK_wo);
					sqlCommand.Parameters.AddWithValue("KalkulatorischeKosten" + i, item.KalkulatorischeKosten == null ? (object)DBNull.Value : item.KalkulatorischeKosten);
					sqlCommand.Parameters.AddWithValue("KalkulatorischeKosten_wo" + i, item.KalkulatorischeKosten_wo == null ? (object)DBNull.Value : item.KalkulatorischeKosten_wo);
					sqlCommand.Parameters.AddWithValue("lastupdated" + i, item.lastupdated == null ? (object)DBNull.Value : item.lastupdated);
					sqlCommand.Parameters.AddWithValue("Logs" + i, item.Logs == null ? (object)DBNull.Value : item.Logs);
					sqlCommand.Parameters.AddWithValue("precent" + i, item.precent == null ? (object)DBNull.Value : item.precent);
					sqlCommand.Parameters.AddWithValue("precent_wo" + i, item.precent_wo == null ? (object)DBNull.Value : item.precent_wo);
					sqlCommand.Parameters.AddWithValue("SumMaterial" + i, item.SumMaterial == null ? (object)DBNull.Value : item.SumMaterial);
					sqlCommand.Parameters.AddWithValue("SumMaterial_wo" + i, item.SumMaterial_wo == null ? (object)DBNull.Value : item.SumMaterial_wo);
					sqlCommand.Parameters.AddWithValue("VK" + i, item.VK == null ? (object)DBNull.Value : item.VK);
					sqlCommand.Parameters.AddWithValue("VK_wo" + i, item.VK_wo == null ? (object)DBNull.Value : item.VK_wo);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [ArtikelsPricesChangesHistory] SET [ArtikelnummerOriginal]=@ArtikelnummerOriginal, [DB]=@DB, [DB_wo]=@DB_wo, [EK]=@EK, [EK_wo]=@EK_wo, [KalkulatorischeKosten]=@KalkulatorischeKosten, [KalkulatorischeKosten_wo]=@KalkulatorischeKosten_wo, [lastupdated]=@lastupdated, [Logs]=@Logs, [precent]=@precent, [precent_wo]=@precent_wo, [SumMaterial]=@SumMaterial, [SumMaterial_wo]=@SumMaterial_wo, [VK]=@VK, [VK_wo]=@VK_wo WHERE [ID]=@ID";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("ID", item.ID);
			sqlCommand.Parameters.AddWithValue("ArtikelnummerOriginal", item.ArtikelnummerOriginal == null ? (object)DBNull.Value : item.ArtikelnummerOriginal);
			sqlCommand.Parameters.AddWithValue("DB", item.DB == null ? (object)DBNull.Value : item.DB);
			sqlCommand.Parameters.AddWithValue("DB_wo", item.DB_wo == null ? (object)DBNull.Value : item.DB_wo);
			sqlCommand.Parameters.AddWithValue("EK", item.EK == null ? (object)DBNull.Value : item.EK);
			sqlCommand.Parameters.AddWithValue("EK_wo", item.EK_wo == null ? (object)DBNull.Value : item.EK_wo);
			sqlCommand.Parameters.AddWithValue("KalkulatorischeKosten", item.KalkulatorischeKosten == null ? (object)DBNull.Value : item.KalkulatorischeKosten);
			sqlCommand.Parameters.AddWithValue("KalkulatorischeKosten_wo", item.KalkulatorischeKosten_wo == null ? (object)DBNull.Value : item.KalkulatorischeKosten_wo);
			sqlCommand.Parameters.AddWithValue("lastupdated", item.lastupdated == null ? (object)DBNull.Value : item.lastupdated);
			sqlCommand.Parameters.AddWithValue("Logs", item.Logs == null ? (object)DBNull.Value : item.Logs);
			sqlCommand.Parameters.AddWithValue("precent", item.precent == null ? (object)DBNull.Value : item.precent);
			sqlCommand.Parameters.AddWithValue("precent_wo", item.precent_wo == null ? (object)DBNull.Value : item.precent_wo);
			sqlCommand.Parameters.AddWithValue("SumMaterial", item.SumMaterial == null ? (object)DBNull.Value : item.SumMaterial);
			sqlCommand.Parameters.AddWithValue("SumMaterial_wo", item.SumMaterial_wo == null ? (object)DBNull.Value : item.SumMaterial_wo);
			sqlCommand.Parameters.AddWithValue("VK", item.VK == null ? (object)DBNull.Value : item.VK);
			sqlCommand.Parameters.AddWithValue("VK_wo", item.VK_wo == null ? (object)DBNull.Value : item.VK_wo);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 16; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = updateWithTransaction(items, connection, transaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += updateWithTransaction(items.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += updateWithTransaction(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber), connection, transaction);
				}

				return results;
			}

			return -1;
		}
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " UPDATE [ArtikelsPricesChangesHistory] SET "

					+ "[ArtikelnummerOriginal]=@ArtikelnummerOriginal" + i + ","
					+ "[DB]=@DB" + i + ","
					+ "[DB_wo]=@DB_wo" + i + ","
					+ "[EK]=@EK" + i + ","
					+ "[EK_wo]=@EK_wo" + i + ","
					+ "[KalkulatorischeKosten]=@KalkulatorischeKosten" + i + ","
					+ "[KalkulatorischeKosten_wo]=@KalkulatorischeKosten_wo" + i + ","
					+ "[lastupdated]=@lastupdated" + i + ","
					+ "[Logs]=@Logs" + i + ","
					+ "[precent]=@precent" + i + ","
					+ "[precent_wo]=@precent_wo" + i + ","
					+ "[SumMaterial]=@SumMaterial" + i + ","
					+ "[SumMaterial_wo]=@SumMaterial_wo" + i + ","
					+ "[VK]=@VK" + i + ","
					+ "[VK_wo]=@VK_wo" + i + " WHERE [ID]=@ID" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
					sqlCommand.Parameters.AddWithValue("ArtikelnummerOriginal" + i, item.ArtikelnummerOriginal == null ? (object)DBNull.Value : item.ArtikelnummerOriginal);
					sqlCommand.Parameters.AddWithValue("DB" + i, item.DB == null ? (object)DBNull.Value : item.DB);
					sqlCommand.Parameters.AddWithValue("DB_wo" + i, item.DB_wo == null ? (object)DBNull.Value : item.DB_wo);
					sqlCommand.Parameters.AddWithValue("EK" + i, item.EK == null ? (object)DBNull.Value : item.EK);
					sqlCommand.Parameters.AddWithValue("EK_wo" + i, item.EK_wo == null ? (object)DBNull.Value : item.EK_wo);
					sqlCommand.Parameters.AddWithValue("KalkulatorischeKosten" + i, item.KalkulatorischeKosten == null ? (object)DBNull.Value : item.KalkulatorischeKosten);
					sqlCommand.Parameters.AddWithValue("KalkulatorischeKosten_wo" + i, item.KalkulatorischeKosten_wo == null ? (object)DBNull.Value : item.KalkulatorischeKosten_wo);
					sqlCommand.Parameters.AddWithValue("lastupdated" + i, item.lastupdated == null ? (object)DBNull.Value : item.lastupdated);
					sqlCommand.Parameters.AddWithValue("Logs" + i, item.Logs == null ? (object)DBNull.Value : item.Logs);
					sqlCommand.Parameters.AddWithValue("precent" + i, item.precent == null ? (object)DBNull.Value : item.precent);
					sqlCommand.Parameters.AddWithValue("precent_wo" + i, item.precent_wo == null ? (object)DBNull.Value : item.precent_wo);
					sqlCommand.Parameters.AddWithValue("SumMaterial" + i, item.SumMaterial == null ? (object)DBNull.Value : item.SumMaterial);
					sqlCommand.Parameters.AddWithValue("SumMaterial_wo" + i, item.SumMaterial_wo == null ? (object)DBNull.Value : item.SumMaterial_wo);
					sqlCommand.Parameters.AddWithValue("VK" + i, item.VK == null ? (object)DBNull.Value : item.VK);
					sqlCommand.Parameters.AddWithValue("VK_wo" + i, item.VK_wo == null ? (object)DBNull.Value : item.VK_wo);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [ArtikelsPricesChangesHistory] WHERE [ID]=@ID";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ID", id);

			results = sqlCommand.ExecuteNonQuery();


			return results;
		}
		public static int DeleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE;
				int results = 0;
				if(ids.Count <= maxParamsNumber)
				{
					results = deleteWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += deleteWithTransaction(ids.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += deleteWithTransaction(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber), connection, transaction);
				}
			}
			return -1;
		}
		private static int deleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int results = -1;

				var sqlCommand = new SqlCommand("", connection, transaction);

				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				string query = "DELETE FROM [ArtikelsPricesChangesHistory] WHERE [ID] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity> GetByDate(DateTime afterDate, Settings.PaginModel paging)
		{
			string pagingSorting = "";

			if(paging != null && (0 >= paging.RequestRows || paging.RequestRows > 100))
				paging.RequestRows = 100;

			if(paging != null)
			{
				pagingSorting += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
			}

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				/*string query = @"WITH RankedData AS (
									SELECT
									 ID,
									 ArtikelnummerOriginal,
									 Logs,
									SumMaterial SumMaterial,
									SumMaterial_wo SumMaterial_wo,
									KalkulatorischeKosten KalkulatorischeKosten,
									KalkulatorischeKosten_wo KalkulatorischeKosten_wo,
									VK VK,
									VK_wo VK_wo,
									EK EK,
									EK_wo EK_wo, 
									DB DB,
									DB_wo DB_wo,
									precent precent,
									precent_wo precent_wo,
									lastupdated,
										ROW_NUMBER() OVER (PARTITION BY ArtikelnummerOriginal ORDER BY lastupdated DESC) AS RowRank
									FROM
										ArtikelsPricesChangesHistory
								)
								SELECT Count(*) Over() TotalCount, *
									FROM RankedData
									WHERE 
									RowRank = 1 
									and CAST(lastupdated AS DATE)  >=   CAST( @afterdate AS DATE) order by ArtikelnummerOriginal " + pagingSorting;*/

				string query = @"WITH RankedData AS (
								SELECT
								 ID,
								 ArtikelnummerOriginal,
								 Logs,
								SumMaterial SumMaterial,
								SumMaterial_wo SumMaterial_wo,
								KalkulatorischeKosten KalkulatorischeKosten,
								KalkulatorischeKosten_wo KalkulatorischeKosten_wo,
								VK VK,
								VK_wo VK_wo,
								EK EK,
								EK_wo EK_wo, 
								DB DB,
								DB_wo DB_wo,
								precent precent,
								precent_wo precent_wo,
								lastupdated,
									ROW_NUMBER() OVER (PARTITION BY ArtikelnummerOriginal ORDER BY lastupdated desc) AS RowRank
								FROM
									ArtikelsPricesChangesHistory 
		
							)
							select Count(*) Over() TotalCount,* from (SELECT  ID,
								ArtikelnummerOriginal,
								Logs,
								SumMaterial SumMaterial,
								SumMaterial_wo SumMaterial_wo,
								KalkulatorischeKosten KalkulatorischeKosten,
								KalkulatorischeKosten_wo KalkulatorischeKosten_wo,
								VK VK,
								VK_wo VK_wo,
								EK EK,
								EK_wo EK_wo, 
								DB DB,
								DB_wo DB_wo,
								precent precent,
								precent_wo precent_wo,
								lastupdated,
								ISNULL(SumMaterialNewMinusPrevious,0) SumMaterialNewMinusPrevious,
								IIF(ISNULL(KalkulatorischeKostenNewMinusPrevious,0)>0,1,0) KalkulatorischeKostenNewMinusPrevious,
								IIF(ISNULL(EKNewMinusPrevious,0)>0,1,0) EKNewMinusPrevious,
								CASE
									WHEN ISNULL(SumMaterialNewMinusPrevious, 0) = 0 THEN 0
									WHEN ISNULL(SumMaterialNewMinusPrevious, 0) > 0 THEN 1
									WHEN ISNULL(SumMaterialNewMinusPrevious, 0) < 0 THEN -1
								END AS SumMaterialNewMinusPreviousState,
								CASE
									WHEN ISNULL(KalkulatorischeKostenNewMinusPrevious, 0) = 0 THEN 0
									WHEN ISNULL(KalkulatorischeKostenNewMinusPrevious, 0) > 0 THEN 1
									WHEN ISNULL(KalkulatorischeKostenNewMinusPrevious, 0) < 0 THEN -1
								END AS KalkulatorischeKostenNewMinusPreviousState,
								CASE
									WHEN ISNULL(EKNewMinusPrevious, 0) = 0 THEN 0
									WHEN ISNULL(EKNewMinusPrevious, 0) > 0 THEN 1
									WHEN ISNULL(EKNewMinusPrevious, 0) < 0 THEN -1
								END AS EKNewMinusPreviousState,
								RowRank
								from (
							select *
							,SumMaterial-LEAD(SumMaterial) over (Partition by ArtikelnummerOriginal Order by ArtikelnummerOriginal ,RowRank ) SumMaterialNewMinusPrevious
							,KalkulatorischeKosten-LEAD(KalkulatorischeKosten) over (Partition by ArtikelnummerOriginal Order by ArtikelnummerOriginal ,RowRank ) KalkulatorischeKostenNewMinusPrevious
							,EK-LEAD(EK) over (Partition by ArtikelnummerOriginal Order by ArtikelnummerOriginal ,RowRank ) EKNewMinusPrevious
							from 
							RankedData
							where RowRank <= 2 )  f where  RowRank = 1 
							) df where (SumMaterialNewMinusPreviousState IN (1,-1) OR  KalkulatorischeKostenNewMinusPreviousState IN (1,-1) OR EKNewMinusPreviousState IN (1,-1) ) AND CAST(lastupdated AS DATE)  >=   CAST( @afterdate AS DATE)  
							 order by lastupdated desc " + pagingSorting;
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("afterdate", afterDate);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity(x, true)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity> GetArticleHistoryByDate(string Artikelnummer, Settings.PaginModel paging)
		{
			string pagingSorting = "";

			if(paging != null && (0 >= paging.RequestRows || paging.RequestRows > 100))
				paging.RequestRows = 100;

			if(paging != null)
			{
				pagingSorting += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
			}

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @$"select  
							  Count(*) over () TotalCount, *
							 from ArtikelsPricesChangesHistory 
							 where ArtikelnummerOriginal = '{Artikelnummer}' order by lastupdated desc" + pagingSorting;
				var sqlCommand = new SqlCommand(query, sqlConnection);
				//sqlCommand.Parameters.AddWithValue("artikelnummer", afterDate);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity(x, 1)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity> GetArticleHistoryByDateLast(string Artikelnummer, Settings.PaginModel paging)
		{
			string pagingSorting = "";

			if(paging != null && (0 >= paging.RequestRows || paging.RequestRows > 100))
				paging.RequestRows = 100;

			if(paging != null)
			{
				pagingSorting += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
			}

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @$"select *
											,
											CASE
												WHEN ISNULL(SubtractedSumMaterial, 0) = 0 THEN 0
												WHEN ISNULL(SubtractedSumMaterial, 0) > 0 THEN 1
												WHEN ISNULL(SubtractedSumMaterial, 0) < 0 THEN -1
											END AS SubtractedSumMaterialres
											,
											CASE
												WHEN ISNULL(SubtractedKalkulatorischeKosten, 0) = 0 THEN 0
												WHEN ISNULL(SubtractedKalkulatorischeKosten, 0) > 0 THEN 1
												WHEN ISNULL(SubtractedKalkulatorischeKosten, 0) < 0 THEN -1
											END AS SubtractedKalkulatorischeKostenres
											,
											CASE
												WHEN ISNULL(SubtractedVK, 0) = 0 THEN 0
												WHEN ISNULL(SubtractedVK, 0) > 0 THEN 1
												WHEN ISNULL(SubtractedVK, 0) < 0 THEN -1
											END AS SubtractedVKres
											,
											CASE
												WHEN ISNULL(SubtractedEK, 0) = 0 THEN 0
												WHEN ISNULL(SubtractedEK, 0) > 0 THEN 1
												WHEN ISNULL(SubtractedEK, 0) < 0 THEN -1
											END AS SubtractedEKres
											,
											CASE
												WHEN ISNULL(SubtractedDB, 0) = 0 THEN 0
												WHEN ISNULL(SubtractedDB, 0) > 0 THEN 1
												WHEN ISNULL(SubtractedDB, 0) < 0 THEN -1
											END AS SubtractedDBres
											,
											CASE
												WHEN ISNULL(Subtractedprecent, 0) = 0 THEN 0
												WHEN ISNULL(Subtractedprecent, 0) > 0 THEN 1
												WHEN ISNULL(Subtractedprecent, 0) < 0 THEN -1
											END AS Subtractedprecentres

											from

											(
											select  
											Count(*) over () TotalCount
											,
											 SumMaterial - LAG(SumMaterial, 1, 0) OVER (ORDER BY ArtikelnummerOriginal,lastupdated) AS SubtractedSumMaterial
											,KalkulatorischeKosten - LAG(KalkulatorischeKosten, 1, 0) OVER (ORDER BY ArtikelnummerOriginal,lastupdated) AS SubtractedKalkulatorischeKosten
											,VK - LAG(VK, 1, 0) OVER (ORDER BY ArtikelnummerOriginal,lastupdated) AS SubtractedVK
											,EK - LAG(EK, 1, 0) OVER (ORDER BY ArtikelnummerOriginal,lastupdated) AS SubtractedEK
											,DB - LAG(DB, 1, 0) OVER (ORDER BY ArtikelnummerOriginal,lastupdated) AS SubtractedDB
											,precent - LAG(precent, 1, 0) OVER (ORDER BY ArtikelnummerOriginal,lastupdated) AS Subtractedprecent
											, *
											from ArtikelsPricesChangesHistory 
											where ArtikelnummerOriginal = '{Artikelnummer}' 
											) d
											order by lastupdated desc " + pagingSorting;
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity(x, 1)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity> GetArticleHistoryByDateForChart(string Artikelnummer, int changes = 10)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"select *
											,
											CASE
												WHEN ISNULL(SubtractedSumMaterial, 0) = 0 THEN 0
												WHEN ISNULL(SubtractedSumMaterial, 0) > 0 THEN 1
												WHEN ISNULL(SubtractedSumMaterial, 0) < 0 THEN -1
											END AS SubtractedSumMaterialres
											,
											CASE
												WHEN ISNULL(SubtractedKalkulatorischeKosten, 0) = 0 THEN 0
												WHEN ISNULL(SubtractedKalkulatorischeKosten, 0) > 0 THEN 1
												WHEN ISNULL(SubtractedKalkulatorischeKosten, 0) < 0 THEN -1
											END AS SubtractedKalkulatorischeKostenres
											,
											CASE
												WHEN ISNULL(SubtractedVK, 0) = 0 THEN 0
												WHEN ISNULL(SubtractedVK, 0) > 0 THEN 1
												WHEN ISNULL(SubtractedVK, 0) < 0 THEN -1
											END AS SubtractedVKres
											,
											CASE
												WHEN ISNULL(SubtractedEK, 0) = 0 THEN 0
												WHEN ISNULL(SubtractedEK, 0) > 0 THEN 1
												WHEN ISNULL(SubtractedEK, 0) < 0 THEN -1
											END AS SubtractedEKres
											,
											CASE
												WHEN ISNULL(SubtractedDB, 0) = 0 THEN 0
												WHEN ISNULL(SubtractedDB, 0) > 0 THEN 1
												WHEN ISNULL(SubtractedDB, 0) < 0 THEN -1
											END AS SubtractedDBres
											,
											CASE
												WHEN ISNULL(Subtractedprecent, 0) = 0 THEN 0
												WHEN ISNULL(Subtractedprecent, 0) > 0 THEN 1
												WHEN ISNULL(Subtractedprecent, 0) < 0 THEN -1
											END AS Subtractedprecentres

											from

											(
											select  
											Count(*) over () TotalCount
											,
											 SumMaterial - LAG(SumMaterial, 1, 0) OVER (ORDER BY ArtikelnummerOriginal,lastupdated) AS SubtractedSumMaterial
											,KalkulatorischeKosten - LAG(KalkulatorischeKosten, 1, 0) OVER (ORDER BY ArtikelnummerOriginal,lastupdated) AS SubtractedKalkulatorischeKosten
											,VK - LAG(VK, 1, 0) OVER (ORDER BY ArtikelnummerOriginal,lastupdated) AS SubtractedVK
											,EK - LAG(EK, 1, 0) OVER (ORDER BY ArtikelnummerOriginal,lastupdated) AS SubtractedEK
											,DB - LAG(DB, 1, 0) OVER (ORDER BY ArtikelnummerOriginal,lastupdated) AS SubtractedDB
											,precent - LAG(precent, 1, 0) OVER (ORDER BY ArtikelnummerOriginal,lastupdated) AS Subtractedprecent
											, *
											from ArtikelsPricesChangesHistory 
											where ArtikelnummerOriginal = '{Artikelnummer}' 
											) d
											order by lastupdated  
											OFFSET 0 ROWS FETCH NEXT {changes} ROWS ONLY";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity(x, 1)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity>();
			}
		}
		#endregion Custom Methods

	}


}
