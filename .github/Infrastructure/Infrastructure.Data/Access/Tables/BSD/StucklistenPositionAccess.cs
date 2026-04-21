using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{

	public class StucklistenPositionAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity Get(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Stücklisten] WHERE [Nr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", nr);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Stücklisten]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Stücklisten] WHERE [Nr] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Stücklisten] ([Anzahl],[Artikel-Nr],[Artikel-Nr des Bauteils],[Artikelnummer],[Bezeichnung des Bauteils],[DocumentId],[Position],[Variante],[Vorgang_Nr]) OUTPUT INSERTED.[Nr] VALUES (@Anzahl,@Artikel_Nr,@Artikel_Nr_des_Bauteils,@Artikelnummer,@Bezeichnung_des_Bauteils,@DocumentId,@Position,@Variante,@Vorgang_Nr); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr_des_Bauteils", item.Artikel_Nr_des_Bauteils == null ? (object)DBNull.Value : item.Artikel_Nr_des_Bauteils);
					sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_des_Bauteils", item.Bezeichnung_des_Bauteils == null ? (object)DBNull.Value : item.Bezeichnung_des_Bauteils);
					sqlCommand.Parameters.AddWithValue("DocumentId", item.DocumentId == null ? (object)DBNull.Value : item.DocumentId);
					sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
					sqlCommand.Parameters.AddWithValue("Variante", item.Variante == null ? (object)DBNull.Value : item.Variante);
					sqlCommand.Parameters.AddWithValue("Vorgang_Nr", item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 11; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity> items)
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
						query += " INSERT INTO [Stücklisten] ([Anzahl],[Artikel-Nr],[Artikel-Nr des Bauteils],[Artikelnummer],[Bezeichnung des Bauteils],[DocumentId],[Position],[Variante],[Vorgang_Nr]) VALUES ( "

							+ "@Anzahl" + i + ","
							+ "@Artikel_Nr" + i + ","
							+ "@Artikel_Nr_des_Bauteils" + i + ","
							+ "@Artikelnummer" + i + ","
							+ "@Bezeichnung_des_Bauteils" + i + ","
							+ "@DocumentId" + i + ","
							+ "@Position" + i + ","
							+ "@Variante" + i + ","
							+ "@Vorgang_Nr" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr_des_Bauteils" + i, item.Artikel_Nr_des_Bauteils == null ? (object)DBNull.Value : item.Artikel_Nr_des_Bauteils);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_des_Bauteils" + i, item.Bezeichnung_des_Bauteils == null ? (object)DBNull.Value : item.Bezeichnung_des_Bauteils);
						sqlCommand.Parameters.AddWithValue("DocumentId" + i, item.DocumentId == null ? (object)DBNull.Value : item.DocumentId);
						sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
						sqlCommand.Parameters.AddWithValue("Variante" + i, item.Variante == null ? (object)DBNull.Value : item.Variante);
						sqlCommand.Parameters.AddWithValue("Vorgang_Nr" + i, item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Stücklisten] SET [Anzahl]=@Anzahl, [Artikel-Nr]=@Artikel_Nr, [Artikel-Nr des Bauteils]=@Artikel_Nr_des_Bauteils, [Artikelnummer]=@Artikelnummer, [Bezeichnung des Bauteils]=@Bezeichnung_des_Bauteils, [DocumentId]=@DocumentId, [Position]=@Position, [Variante]=@Variante, [Vorgang_Nr]=@Vorgang_Nr WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
				sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr_des_Bauteils", item.Artikel_Nr_des_Bauteils == null ? (object)DBNull.Value : item.Artikel_Nr_des_Bauteils);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("Bezeichnung_des_Bauteils", item.Bezeichnung_des_Bauteils == null ? (object)DBNull.Value : item.Bezeichnung_des_Bauteils);
				sqlCommand.Parameters.AddWithValue("DocumentId", item.DocumentId == null ? (object)DBNull.Value : item.DocumentId);
				sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
				sqlCommand.Parameters.AddWithValue("Variante", item.Variante == null ? (object)DBNull.Value : item.Variante);
				sqlCommand.Parameters.AddWithValue("Vorgang_Nr", item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 11; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity> items)
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
						query += " UPDATE [Stücklisten] SET "

							+ "[Anzahl]=@Anzahl" + i + ","
							+ "[Artikel-Nr]=@Artikel_Nr" + i + ","
							+ "[Artikel-Nr des Bauteils]=@Artikel_Nr_des_Bauteils" + i + ","
							+ "[Artikelnummer]=@Artikelnummer" + i + ","
							+ "[Bezeichnung des Bauteils]=@Bezeichnung_des_Bauteils" + i + ","
							+ "[DocumentId]=@DocumentId" + i + ","
							+ "[Position]=@Position" + i + ","
							+ "[Variante]=@Variante" + i + ","
							+ "[Vorgang_Nr]=@Vorgang_Nr" + i + " WHERE [Nr]=@Nr" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr_des_Bauteils" + i, item.Artikel_Nr_des_Bauteils == null ? (object)DBNull.Value : item.Artikel_Nr_des_Bauteils);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_des_Bauteils" + i, item.Bezeichnung_des_Bauteils == null ? (object)DBNull.Value : item.Bezeichnung_des_Bauteils);
						sqlCommand.Parameters.AddWithValue("DocumentId" + i, item.DocumentId == null ? (object)DBNull.Value : item.DocumentId);
						sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
						sqlCommand.Parameters.AddWithValue("Variante" + i, item.Variante == null ? (object)DBNull.Value : item.Variante);
						sqlCommand.Parameters.AddWithValue("Vorgang_Nr" + i, item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
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
				string query = "DELETE FROM [Stücklisten] WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Nr", nr);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int DeleteByArticleID(int articleID)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Stücklisten] WHERE [Artikel-Nr]=@articleID";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleID", articleID);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
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

					string query = "DELETE FROM [Stücklisten] WHERE [Nr] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity GetWithTransaction(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Stücklisten] WHERE [Nr]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", nr);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Stücklisten]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Stücklisten] WHERE [Nr] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [Stücklisten] ([Anzahl],[Artikel-Nr],[Artikel-Nr des Bauteils],[Artikelnummer],[Bezeichnung des Bauteils],[DocumentId],[Position],[Variante],[Vorgang_Nr]) OUTPUT INSERTED.[Nr] VALUES (@Anzahl,@Artikel_Nr,@Artikel_Nr_des_Bauteils,@Artikelnummer,@Bezeichnung_des_Bauteils,@DocumentId,@Position,@Variante,@Vorgang_Nr); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr_des_Bauteils", item.Artikel_Nr_des_Bauteils == null ? (object)DBNull.Value : item.Artikel_Nr_des_Bauteils);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_des_Bauteils", item.Bezeichnung_des_Bauteils == null ? (object)DBNull.Value : item.Bezeichnung_des_Bauteils);
			sqlCommand.Parameters.AddWithValue("DocumentId", item.DocumentId == null ? (object)DBNull.Value : item.DocumentId);
			sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
			sqlCommand.Parameters.AddWithValue("Variante", item.Variante == null ? (object)DBNull.Value : item.Variante);
			sqlCommand.Parameters.AddWithValue("Vorgang_Nr", item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 11; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Stücklisten] ([Anzahl],[Artikel-Nr],[Artikel-Nr des Bauteils],[Artikelnummer],[Bezeichnung des Bauteils],[DocumentId],[Position],[Variante],[Vorgang_Nr]) VALUES ( "

						+ "@Anzahl" + i + ","
						+ "@Artikel_Nr" + i + ","
						+ "@Artikel_Nr_des_Bauteils" + i + ","
						+ "@Artikelnummer" + i + ","
						+ "@Bezeichnung_des_Bauteils" + i + ","
						+ "@DocumentId" + i + ","
						+ "@Position" + i + ","
						+ "@Variante" + i + ","
						+ "@Vorgang_Nr" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr_des_Bauteils" + i, item.Artikel_Nr_des_Bauteils == null ? (object)DBNull.Value : item.Artikel_Nr_des_Bauteils);
					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_des_Bauteils" + i, item.Bezeichnung_des_Bauteils == null ? (object)DBNull.Value : item.Bezeichnung_des_Bauteils);
					sqlCommand.Parameters.AddWithValue("DocumentId" + i, item.DocumentId == null ? (object)DBNull.Value : item.DocumentId);
					sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
					sqlCommand.Parameters.AddWithValue("Variante" + i, item.Variante == null ? (object)DBNull.Value : item.Variante);
					sqlCommand.Parameters.AddWithValue("Vorgang_Nr" + i, item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Stücklisten] SET [Anzahl]=@Anzahl, [Artikel-Nr]=@Artikel_Nr, [Artikel-Nr des Bauteils]=@Artikel_Nr_des_Bauteils, [Artikelnummer]=@Artikelnummer, [Bezeichnung des Bauteils]=@Bezeichnung_des_Bauteils, [DocumentId]=@DocumentId, [Position]=@Position, [Variante]=@Variante, [Vorgang_Nr]=@Vorgang_Nr WHERE [Nr]=@Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
			sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr_des_Bauteils", item.Artikel_Nr_des_Bauteils == null ? (object)DBNull.Value : item.Artikel_Nr_des_Bauteils);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_des_Bauteils", item.Bezeichnung_des_Bauteils == null ? (object)DBNull.Value : item.Bezeichnung_des_Bauteils);
			sqlCommand.Parameters.AddWithValue("DocumentId", item.DocumentId == null ? (object)DBNull.Value : item.DocumentId);
			sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
			sqlCommand.Parameters.AddWithValue("Variante", item.Variante == null ? (object)DBNull.Value : item.Variante);
			sqlCommand.Parameters.AddWithValue("Vorgang_Nr", item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 11; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				//int results = -1;
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " UPDATE [Stücklisten] SET "

					+ "[Anzahl]=@Anzahl" + i + ","
					+ "[Artikel-Nr]=@Artikel_Nr" + i + ","
					+ "[Artikel-Nr des Bauteils]=@Artikel_Nr_des_Bauteils" + i + ","
					+ "[Artikelnummer]=@Artikelnummer" + i + ","
					+ "[Bezeichnung des Bauteils]=@Bezeichnung_des_Bauteils" + i + ","
					+ "[DocumentId]=@DocumentId" + i + ","
					+ "[Position]=@Position" + i + ","
					+ "[Variante]=@Variante" + i + ","
					+ "[Vorgang_Nr]=@Vorgang_Nr" + i + " WHERE [Nr]=@Nr" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
					sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr_des_Bauteils" + i, item.Artikel_Nr_des_Bauteils == null ? (object)DBNull.Value : item.Artikel_Nr_des_Bauteils);
					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_des_Bauteils" + i, item.Bezeichnung_des_Bauteils == null ? (object)DBNull.Value : item.Bezeichnung_des_Bauteils);
					sqlCommand.Parameters.AddWithValue("DocumentId" + i, item.DocumentId == null ? (object)DBNull.Value : item.DocumentId);
					sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
					sqlCommand.Parameters.AddWithValue("Variante" + i, item.Variante == null ? (object)DBNull.Value : item.Variante);
					sqlCommand.Parameters.AddWithValue("Vorgang_Nr" + i, item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Stücklisten] WHERE [Nr]=@Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Nr", nr);

			results = DbExecution.ExecuteNonQuery(sqlCommand);


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

				string query = "DELETE FROM [Stücklisten] WHERE [Nr] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity> GetByArticle(int articleId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Stücklisten] WHERE [Artikel-Nr]=@articleId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity> GetByArticleUBG(int articleId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Stücklisten] WHERE [Artikel-Nr des Bauteils]=@articleId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity> GetByArticleUBG(List<int> articleIds)
		{
			if(articleIds == null || articleIds.Count <= 0)
				return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity>();

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Stücklisten] WHERE [Artikel-Nr des Bauteils] IN ({string.Join(",", articleIds)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity> GetByArticle(int articleId, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();
			{
				string query = "SELECT * FROM [Stücklisten] WHERE [Artikel-Nr]=@articleId";
				var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity> GetByArticles(List<int> articleIds)
		{
			if(articleIds == null || articleIds.Count <= 0)
				return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity>();

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Stücklisten] WHERE [Artikel-Nr] IN ({string.Join(",", articleIds)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity> GetByParentAndChildID(int parentID, int childId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Stücklisten] WHERE [Artikel-Nr]=@parentID and [Artikel-Nr des Bauteils]=@childId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("parentID", parentID);
				sqlCommand.Parameters.AddWithValue("childId", childId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity>();
			}
		}

		public static List<int> GetParentIds(int articleId, bool isActive = true)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT [Artikel-Nr] FROM [Stücklisten] WHERE [Artikel-Nr des Bauteils]=@articleId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);
				sqlCommand.Parameters.AddWithValue("isActive", isActive);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => int.TryParse(x[0]?.ToString(), out var _x) ? _x : -1).ToList();
			}
			else
			{
				return new List<int>();
			}
		}
		public static List<int> GetChildrenIds(int articleId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT [Artikel-Nr des Bauteils] FROM [Stücklisten] WHERE [Artikel-Nr]=@articleId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => Convert.ToInt32(x[0] ?? -1)).ToList();
			}
			else
			{
				return new List<int>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity> GetByArticleWithTransaction(int articleId, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			string query = "SELECT * FROM [Stücklisten] WHERE [Artikel-Nr]=@articleId";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("articleId", articleId);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity>();
			}
		}
		#endregion
	}
}
