using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class Stucklisten_SnapshotAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity Get(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_Stucklisten_Snapshot] WHERE [Nr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", nr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_Stucklisten_Snapshot]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__BSD_Stucklisten_Snapshot] WHERE [Nr] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__BSD_Stucklisten_Snapshot] ([Anzahl],[Artikel-Nr],[Artikel-Nr des Bauteils],[Artikelnummer],[Bezeichnung des Bauteils],[BomVersion],[DocumentId],[KundenIndex],[KundenIndexDate],[Overwritten],[OverwrittenTime],[OverwrittenUserId],[Position],[SnapshotTime],[SnapshotUserId],[Variante],[Vorgang_Nr]) OUTPUT INSERTED.[Nr] VALUES (@Anzahl,@Artikel_Nr,@Artikel_Nr_des_Bauteils,@Artikelnummer,@Bezeichnung_des_Bauteils,@BomVersion,@DocumentId,@KundenIndex,@KundenIndexDate,@Overwritten,@OverwrittenTime,@OverwrittenUserId,@Position,@SnapshotTime,@SnapshotUserId,@Variante,@Vorgang_Nr); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr_des_Bauteils", item.Artikel_Nr_des_Bauteils == null ? (object)DBNull.Value : item.Artikel_Nr_des_Bauteils);
					sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_des_Bauteils", item.Bezeichnung_des_Bauteils == null ? (object)DBNull.Value : item.Bezeichnung_des_Bauteils);
					sqlCommand.Parameters.AddWithValue("BomVersion", item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
					sqlCommand.Parameters.AddWithValue("DocumentId", item.DocumentId == null ? (object)DBNull.Value : item.DocumentId);
					sqlCommand.Parameters.AddWithValue("KundenIndex", item.KundenIndex == null ? (object)DBNull.Value : item.KundenIndex);
					sqlCommand.Parameters.AddWithValue("KundenIndexDate", item.KundenIndexDate == null ? (object)DBNull.Value : item.KundenIndexDate);
					sqlCommand.Parameters.AddWithValue("Overwritten", item.Overwritten == null ? (object)DBNull.Value : item.Overwritten);
					sqlCommand.Parameters.AddWithValue("OverwrittenTime", item.OverwrittenTime == null ? (object)DBNull.Value : item.OverwrittenTime);
					sqlCommand.Parameters.AddWithValue("OverwrittenUserId", item.OverwrittenUserId == null ? (object)DBNull.Value : item.OverwrittenUserId);
					sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
					sqlCommand.Parameters.AddWithValue("SnapshotTime", item.SnapshotTime);
					sqlCommand.Parameters.AddWithValue("SnapshotUserId", item.SnapshotUserId);
					sqlCommand.Parameters.AddWithValue("Variante", item.Variante == null ? (object)DBNull.Value : item.Variante);
					sqlCommand.Parameters.AddWithValue("Vorgang_Nr", item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 19; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity> items)
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
						query += " INSERT INTO [__BSD_Stucklisten_Snapshot] ([Anzahl],[Artikel-Nr],[Artikel-Nr des Bauteils],[Artikelnummer],[Bezeichnung des Bauteils],[BomVersion],[DocumentId],[KundenIndex],[KundenIndexDate],[Overwritten],[OverwrittenTime],[OverwrittenUserId],[Position],[SnapshotTime],[SnapshotUserId],[Variante],[Vorgang_Nr]) VALUES ( "

							+ "@Anzahl" + i + ","
							+ "@Artikel_Nr" + i + ","
							+ "@Artikel_Nr_des_Bauteils" + i + ","
							+ "@Artikelnummer" + i + ","
							+ "@Bezeichnung_des_Bauteils" + i + ","
							+ "@BomVersion" + i + ","
							+ "@DocumentId" + i + ","
							+ "@KundenIndex" + i + ","
							+ "@KundenIndexDate" + i + ","
							+ "@Overwritten" + i + ","
							+ "@OverwrittenTime" + i + ","
							+ "@OverwrittenUserId" + i + ","
							+ "@Position" + i + ","
							+ "@SnapshotTime" + i + ","
							+ "@SnapshotUserId" + i + ","
							+ "@Variante" + i + ","
							+ "@Vorgang_Nr" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr_des_Bauteils" + i, item.Artikel_Nr_des_Bauteils == null ? (object)DBNull.Value : item.Artikel_Nr_des_Bauteils);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_des_Bauteils" + i, item.Bezeichnung_des_Bauteils == null ? (object)DBNull.Value : item.Bezeichnung_des_Bauteils);
						sqlCommand.Parameters.AddWithValue("BomVersion" + i, item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
						sqlCommand.Parameters.AddWithValue("DocumentId" + i, item.DocumentId == null ? (object)DBNull.Value : item.DocumentId);
						sqlCommand.Parameters.AddWithValue("KundenIndex" + i, item.KundenIndex == null ? (object)DBNull.Value : item.KundenIndex);
						sqlCommand.Parameters.AddWithValue("KundenIndexDate" + i, item.KundenIndexDate == null ? (object)DBNull.Value : item.KundenIndexDate);
						sqlCommand.Parameters.AddWithValue("Overwritten" + i, item.Overwritten == null ? (object)DBNull.Value : item.Overwritten);
						sqlCommand.Parameters.AddWithValue("OverwrittenTime" + i, item.OverwrittenTime == null ? (object)DBNull.Value : item.OverwrittenTime);
						sqlCommand.Parameters.AddWithValue("OverwrittenUserId" + i, item.OverwrittenUserId == null ? (object)DBNull.Value : item.OverwrittenUserId);
						sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
						sqlCommand.Parameters.AddWithValue("SnapshotTime" + i, item.SnapshotTime);
						sqlCommand.Parameters.AddWithValue("SnapshotUserId" + i, item.SnapshotUserId);
						sqlCommand.Parameters.AddWithValue("Variante" + i, item.Variante == null ? (object)DBNull.Value : item.Variante);
						sqlCommand.Parameters.AddWithValue("Vorgang_Nr" + i, item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__BSD_Stucklisten_Snapshot] SET [Anzahl]=@Anzahl, [Artikel-Nr]=@Artikel_Nr, [Artikel-Nr des Bauteils]=@Artikel_Nr_des_Bauteils, [Artikelnummer]=@Artikelnummer, [Bezeichnung des Bauteils]=@Bezeichnung_des_Bauteils, [BomVersion]=@BomVersion, [DocumentId]=@DocumentId, [KundenIndex]=@KundenIndex, [KundenIndexDate]=@KundenIndexDate, [Overwritten]=@Overwritten, [OverwrittenTime]=@OverwrittenTime, [OverwrittenUserId]=@OverwrittenUserId, [Position]=@Position, [SnapshotTime]=@SnapshotTime, [SnapshotUserId]=@SnapshotUserId, [Variante]=@Variante, [Vorgang_Nr]=@Vorgang_Nr WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
				sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr_des_Bauteils", item.Artikel_Nr_des_Bauteils == null ? (object)DBNull.Value : item.Artikel_Nr_des_Bauteils);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("Bezeichnung_des_Bauteils", item.Bezeichnung_des_Bauteils == null ? (object)DBNull.Value : item.Bezeichnung_des_Bauteils);
				sqlCommand.Parameters.AddWithValue("BomVersion", item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
				sqlCommand.Parameters.AddWithValue("DocumentId", item.DocumentId == null ? (object)DBNull.Value : item.DocumentId);
				sqlCommand.Parameters.AddWithValue("KundenIndex", item.KundenIndex == null ? (object)DBNull.Value : item.KundenIndex);
				sqlCommand.Parameters.AddWithValue("KundenIndexDate", item.KundenIndexDate == null ? (object)DBNull.Value : item.KundenIndexDate);
				sqlCommand.Parameters.AddWithValue("Overwritten", item.Overwritten == null ? (object)DBNull.Value : item.Overwritten);
				sqlCommand.Parameters.AddWithValue("OverwrittenTime", item.OverwrittenTime == null ? (object)DBNull.Value : item.OverwrittenTime);
				sqlCommand.Parameters.AddWithValue("OverwrittenUserId", item.OverwrittenUserId == null ? (object)DBNull.Value : item.OverwrittenUserId);
				sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
				sqlCommand.Parameters.AddWithValue("SnapshotTime", item.SnapshotTime);
				sqlCommand.Parameters.AddWithValue("SnapshotUserId", item.SnapshotUserId);
				sqlCommand.Parameters.AddWithValue("Variante", item.Variante == null ? (object)DBNull.Value : item.Variante);
				sqlCommand.Parameters.AddWithValue("Vorgang_Nr", item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 19; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity> items)
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
						query += " UPDATE [__BSD_Stucklisten_Snapshot] SET "

							+ "[Anzahl]=@Anzahl" + i + ","
							+ "[Artikel-Nr]=@Artikel_Nr" + i + ","
							+ "[Artikel-Nr des Bauteils]=@Artikel_Nr_des_Bauteils" + i + ","
							+ "[Artikelnummer]=@Artikelnummer" + i + ","
							+ "[Bezeichnung des Bauteils]=@Bezeichnung_des_Bauteils" + i + ","
							+ "[BomVersion]=@BomVersion" + i + ","
							+ "[DocumentId]=@DocumentId" + i + ","
							+ "[KundenIndex]=@KundenIndex" + i + ","
							+ "[KundenIndexDate]=@KundenIndexDate" + i + ","
							+ "[Overwritten]=@Overwritten" + i + ","
							+ "[OverwrittenTime]=@OverwrittenTime" + i + ","
							+ "[OverwrittenUserId]=@OverwrittenUserId" + i + ","
							+ "[Position]=@Position" + i + ","
							+ "[SnapshotTime]=@SnapshotTime" + i + ","
							+ "[SnapshotUserId]=@SnapshotUserId" + i + ","
							+ "[Variante]=@Variante" + i + ","
							+ "[Vorgang_Nr]=@Vorgang_Nr" + i + " WHERE [Nr]=@Nr" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr_des_Bauteils" + i, item.Artikel_Nr_des_Bauteils == null ? (object)DBNull.Value : item.Artikel_Nr_des_Bauteils);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_des_Bauteils" + i, item.Bezeichnung_des_Bauteils == null ? (object)DBNull.Value : item.Bezeichnung_des_Bauteils);
						sqlCommand.Parameters.AddWithValue("BomVersion" + i, item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
						sqlCommand.Parameters.AddWithValue("DocumentId" + i, item.DocumentId == null ? (object)DBNull.Value : item.DocumentId);
						sqlCommand.Parameters.AddWithValue("KundenIndex" + i, item.KundenIndex == null ? (object)DBNull.Value : item.KundenIndex);
						sqlCommand.Parameters.AddWithValue("KundenIndexDate" + i, item.KundenIndexDate == null ? (object)DBNull.Value : item.KundenIndexDate);
						sqlCommand.Parameters.AddWithValue("Overwritten" + i, item.Overwritten == null ? (object)DBNull.Value : item.Overwritten);
						sqlCommand.Parameters.AddWithValue("OverwrittenTime" + i, item.OverwrittenTime == null ? (object)DBNull.Value : item.OverwrittenTime);
						sqlCommand.Parameters.AddWithValue("OverwrittenUserId" + i, item.OverwrittenUserId == null ? (object)DBNull.Value : item.OverwrittenUserId);
						sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
						sqlCommand.Parameters.AddWithValue("SnapshotTime" + i, item.SnapshotTime);
						sqlCommand.Parameters.AddWithValue("SnapshotUserId" + i, item.SnapshotUserId);
						sqlCommand.Parameters.AddWithValue("Variante" + i, item.Variante == null ? (object)DBNull.Value : item.Variante);
						sqlCommand.Parameters.AddWithValue("Vorgang_Nr" + i, item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);
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
				string query = "DELETE FROM [__BSD_Stucklisten_Snapshot] WHERE [Nr]=@Nr";
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

					string query = "DELETE FROM [__BSD_Stucklisten_Snapshot] WHERE [Nr] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity GetWithTransaction(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__BSD_Stucklisten_Snapshot] WHERE [Nr]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", nr);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__BSD_Stucklisten_Snapshot]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__BSD_Stucklisten_Snapshot] WHERE [Nr] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [__BSD_Stucklisten_Snapshot] ([Anzahl],[Artikel-Nr],[Artikel-Nr des Bauteils],[Artikelnummer],[Bezeichnung des Bauteils],[BomVersion],[DocumentId],[KundenIndex],[KundenIndexDate],[Overwritten],[OverwrittenTime],[OverwrittenUserId],[Position],[SnapshotTime],[SnapshotUserId],[Variante],[Vorgang_Nr]) OUTPUT INSERTED.[Nr] VALUES (@Anzahl,@Artikel_Nr,@Artikel_Nr_des_Bauteils,@Artikelnummer,@Bezeichnung_des_Bauteils,@BomVersion,@DocumentId,@KundenIndex,@KundenIndexDate,@Overwritten,@OverwrittenTime,@OverwrittenUserId,@Position,@SnapshotTime,@SnapshotUserId,@Variante,@Vorgang_Nr); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr_des_Bauteils", item.Artikel_Nr_des_Bauteils == null ? (object)DBNull.Value : item.Artikel_Nr_des_Bauteils);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_des_Bauteils", item.Bezeichnung_des_Bauteils == null ? (object)DBNull.Value : item.Bezeichnung_des_Bauteils);
			sqlCommand.Parameters.AddWithValue("BomVersion", item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
			sqlCommand.Parameters.AddWithValue("DocumentId", item.DocumentId == null ? (object)DBNull.Value : item.DocumentId);
			sqlCommand.Parameters.AddWithValue("KundenIndex", item.KundenIndex == null ? (object)DBNull.Value : item.KundenIndex);
			sqlCommand.Parameters.AddWithValue("KundenIndexDate", item.KundenIndexDate == null ? (object)DBNull.Value : item.KundenIndexDate);
			sqlCommand.Parameters.AddWithValue("Overwritten", item.Overwritten == null ? (object)DBNull.Value : item.Overwritten);
			sqlCommand.Parameters.AddWithValue("OverwrittenTime", item.OverwrittenTime == null ? (object)DBNull.Value : item.OverwrittenTime);
			sqlCommand.Parameters.AddWithValue("OverwrittenUserId", item.OverwrittenUserId == null ? (object)DBNull.Value : item.OverwrittenUserId);
			sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
			sqlCommand.Parameters.AddWithValue("SnapshotTime", item.SnapshotTime);
			sqlCommand.Parameters.AddWithValue("SnapshotUserId", item.SnapshotUserId);
			sqlCommand.Parameters.AddWithValue("Variante", item.Variante == null ? (object)DBNull.Value : item.Variante);
			sqlCommand.Parameters.AddWithValue("Vorgang_Nr", item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 19; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__BSD_Stucklisten_Snapshot] ([Anzahl],[Artikel-Nr],[Artikel-Nr des Bauteils],[Artikelnummer],[Bezeichnung des Bauteils],[BomVersion],[DocumentId],[KundenIndex],[KundenIndexDate],[Overwritten],[OverwrittenTime],[OverwrittenUserId],[Position],[SnapshotTime],[SnapshotUserId],[Variante],[Vorgang_Nr]) VALUES ( "

						+ "@Anzahl" + i + ","
						+ "@Artikel_Nr" + i + ","
						+ "@Artikel_Nr_des_Bauteils" + i + ","
						+ "@Artikelnummer" + i + ","
						+ "@Bezeichnung_des_Bauteils" + i + ","
						+ "@BomVersion" + i + ","
						+ "@DocumentId" + i + ","
						+ "@KundenIndex" + i + ","
						+ "@KundenIndexDate" + i + ","
						+ "@Overwritten" + i + ","
						+ "@OverwrittenTime" + i + ","
						+ "@OverwrittenUserId" + i + ","
						+ "@Position" + i + ","
						+ "@SnapshotTime" + i + ","
						+ "@SnapshotUserId" + i + ","
						+ "@Variante" + i + ","
						+ "@Vorgang_Nr" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr_des_Bauteils" + i, item.Artikel_Nr_des_Bauteils == null ? (object)DBNull.Value : item.Artikel_Nr_des_Bauteils);
					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_des_Bauteils" + i, item.Bezeichnung_des_Bauteils == null ? (object)DBNull.Value : item.Bezeichnung_des_Bauteils);
					sqlCommand.Parameters.AddWithValue("BomVersion" + i, item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
					sqlCommand.Parameters.AddWithValue("DocumentId" + i, item.DocumentId == null ? (object)DBNull.Value : item.DocumentId);
					sqlCommand.Parameters.AddWithValue("KundenIndex" + i, item.KundenIndex == null ? (object)DBNull.Value : item.KundenIndex);
					sqlCommand.Parameters.AddWithValue("KundenIndexDate" + i, item.KundenIndexDate == null ? (object)DBNull.Value : item.KundenIndexDate);
					sqlCommand.Parameters.AddWithValue("Overwritten" + i, item.Overwritten == null ? (object)DBNull.Value : item.Overwritten);
					sqlCommand.Parameters.AddWithValue("OverwrittenTime" + i, item.OverwrittenTime == null ? (object)DBNull.Value : item.OverwrittenTime);
					sqlCommand.Parameters.AddWithValue("OverwrittenUserId" + i, item.OverwrittenUserId == null ? (object)DBNull.Value : item.OverwrittenUserId);
					sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
					sqlCommand.Parameters.AddWithValue("SnapshotTime" + i, item.SnapshotTime);
					sqlCommand.Parameters.AddWithValue("SnapshotUserId" + i, item.SnapshotUserId);
					sqlCommand.Parameters.AddWithValue("Variante" + i, item.Variante == null ? (object)DBNull.Value : item.Variante);
					sqlCommand.Parameters.AddWithValue("Vorgang_Nr" + i, item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__BSD_Stucklisten_Snapshot] SET [Anzahl]=@Anzahl, [Artikel-Nr]=@Artikel_Nr, [Artikel-Nr des Bauteils]=@Artikel_Nr_des_Bauteils, [Artikelnummer]=@Artikelnummer, [Bezeichnung des Bauteils]=@Bezeichnung_des_Bauteils, [BomVersion]=@BomVersion, [DocumentId]=@DocumentId, [KundenIndex]=@KundenIndex, [KundenIndexDate]=@KundenIndexDate, [Overwritten]=@Overwritten, [OverwrittenTime]=@OverwrittenTime, [OverwrittenUserId]=@OverwrittenUserId, [Position]=@Position, [SnapshotTime]=@SnapshotTime, [SnapshotUserId]=@SnapshotUserId, [Variante]=@Variante, [Vorgang_Nr]=@Vorgang_Nr WHERE [Nr]=@Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
			sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr_des_Bauteils", item.Artikel_Nr_des_Bauteils == null ? (object)DBNull.Value : item.Artikel_Nr_des_Bauteils);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_des_Bauteils", item.Bezeichnung_des_Bauteils == null ? (object)DBNull.Value : item.Bezeichnung_des_Bauteils);
			sqlCommand.Parameters.AddWithValue("BomVersion", item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
			sqlCommand.Parameters.AddWithValue("DocumentId", item.DocumentId == null ? (object)DBNull.Value : item.DocumentId);
			sqlCommand.Parameters.AddWithValue("KundenIndex", item.KundenIndex == null ? (object)DBNull.Value : item.KundenIndex);
			sqlCommand.Parameters.AddWithValue("KundenIndexDate", item.KundenIndexDate == null ? (object)DBNull.Value : item.KundenIndexDate);
			sqlCommand.Parameters.AddWithValue("Overwritten", item.Overwritten == null ? (object)DBNull.Value : item.Overwritten);
			sqlCommand.Parameters.AddWithValue("OverwrittenTime", item.OverwrittenTime == null ? (object)DBNull.Value : item.OverwrittenTime);
			sqlCommand.Parameters.AddWithValue("OverwrittenUserId", item.OverwrittenUserId == null ? (object)DBNull.Value : item.OverwrittenUserId);
			sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
			sqlCommand.Parameters.AddWithValue("SnapshotTime", item.SnapshotTime);
			sqlCommand.Parameters.AddWithValue("SnapshotUserId", item.SnapshotUserId);
			sqlCommand.Parameters.AddWithValue("Variante", item.Variante == null ? (object)DBNull.Value : item.Variante);
			sqlCommand.Parameters.AddWithValue("Vorgang_Nr", item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 19; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__BSD_Stucklisten_Snapshot] SET "

					+ "[Anzahl]=@Anzahl" + i + ","
					+ "[Artikel-Nr]=@Artikel_Nr" + i + ","
					+ "[Artikel-Nr des Bauteils]=@Artikel_Nr_des_Bauteils" + i + ","
					+ "[Artikelnummer]=@Artikelnummer" + i + ","
					+ "[Bezeichnung des Bauteils]=@Bezeichnung_des_Bauteils" + i + ","
					+ "[BomVersion]=@BomVersion" + i + ","
					+ "[DocumentId]=@DocumentId" + i + ","
					+ "[KundenIndex]=@KundenIndex" + i + ","
					+ "[KundenIndexDate]=@KundenIndexDate" + i + ","
					+ "[Overwritten]=@Overwritten" + i + ","
					+ "[OverwrittenTime]=@OverwrittenTime" + i + ","
					+ "[OverwrittenUserId]=@OverwrittenUserId" + i + ","
					+ "[Position]=@Position" + i + ","
					+ "[SnapshotTime]=@SnapshotTime" + i + ","
					+ "[SnapshotUserId]=@SnapshotUserId" + i + ","
					+ "[Variante]=@Variante" + i + ","
					+ "[Vorgang_Nr]=@Vorgang_Nr" + i + " WHERE [Nr]=@Nr" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
					sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr_des_Bauteils" + i, item.Artikel_Nr_des_Bauteils == null ? (object)DBNull.Value : item.Artikel_Nr_des_Bauteils);
					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_des_Bauteils" + i, item.Bezeichnung_des_Bauteils == null ? (object)DBNull.Value : item.Bezeichnung_des_Bauteils);
					sqlCommand.Parameters.AddWithValue("BomVersion" + i, item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
					sqlCommand.Parameters.AddWithValue("DocumentId" + i, item.DocumentId == null ? (object)DBNull.Value : item.DocumentId);
					sqlCommand.Parameters.AddWithValue("KundenIndex" + i, item.KundenIndex == null ? (object)DBNull.Value : item.KundenIndex);
					sqlCommand.Parameters.AddWithValue("KundenIndexDate" + i, item.KundenIndexDate == null ? (object)DBNull.Value : item.KundenIndexDate);
					sqlCommand.Parameters.AddWithValue("Overwritten" + i, item.Overwritten == null ? (object)DBNull.Value : item.Overwritten);
					sqlCommand.Parameters.AddWithValue("OverwrittenTime" + i, item.OverwrittenTime == null ? (object)DBNull.Value : item.OverwrittenTime);
					sqlCommand.Parameters.AddWithValue("OverwrittenUserId" + i, item.OverwrittenUserId == null ? (object)DBNull.Value : item.OverwrittenUserId);
					sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
					sqlCommand.Parameters.AddWithValue("SnapshotTime" + i, item.SnapshotTime);
					sqlCommand.Parameters.AddWithValue("SnapshotUserId" + i, item.SnapshotUserId);
					sqlCommand.Parameters.AddWithValue("Variante" + i, item.Variante == null ? (object)DBNull.Value : item.Variante);
					sqlCommand.Parameters.AddWithValue("Vorgang_Nr" + i, item.Vorgang_Nr == null ? (object)DBNull.Value : item.Vorgang_Nr);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__BSD_Stucklisten_Snapshot] WHERE [Nr]=@Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Nr", nr);

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

				string query = "DELETE FROM [__BSD_Stucklisten_Snapshot] WHERE [Nr] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion


		#region Custom Methods

		public static List<string> GetKundenIndexByArticle(int articleId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT DISTINCT KundenIndex FROM [__BSD_Stucklisten_Snapshot] WHERE [Artikel-Nr]=@articleId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => x[0]?.ToString()).ToList();
			}
			else
			{
				return new List<string>();
			}
		}
		public static List<KeyValuePair<int, string>> GetKundenIndexByArticle(List<int> articleIds)
		{
			if(articleIds == null || articleIds.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT DISTINCT [Artikel-Nr], KundenIndex FROM [__BSD_Stucklisten_Snapshot] WHERE [Artikel-Nr] IN ({string.Join(",", articleIds)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, string>(int.TryParse(x[0]?.ToString(), out var v) ? v : 0, x[1]?.ToString())).ToList();
			}
			else
			{
				return new List<KeyValuePair<int, string>>();
			}
		}
		public static int GetKundenIndexByArticle_Count(int articleId)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT COUNT(DISTINCT KundenIndex) FROM [__BSD_Stucklisten_Snapshot] WHERE [Artikel-Nr]=@articleId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);

				return int.TryParse(sqlCommand.ExecuteScalar()?.ToString(), out var v) ? v : 0;
			}
		}
		public static int GetBOMVersionByArticle_Count(int articleId)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT COUNT(DISTINCT BomVersion) FROM [__BSD_Stucklisten_Snapshot] WHERE [Artikel-Nr]=@articleId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);

				return int.TryParse(sqlCommand.ExecuteScalar()?.ToString(), out var v) ? v : 0;
			}
		}
		public static int GetBOMVersionByArticle_Count(int articleId, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			{
				string query = $"SELECT COUNT(DISTINCT BomVersion) FROM [__BSD_Stucklisten_Snapshot] WHERE [Artikel-Nr]=@articleId";
				var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);

				return int.TryParse(sqlCommand.ExecuteScalar()?.ToString(), out var v) ? v : 0;
			}
		}
		public static List<int> GetBOMVersionByArticle(int articleId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT DISTINCT BomVersion FROM [__BSD_Stucklisten_Snapshot] WHERE [Artikel-Nr]=@articleId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => (int.TryParse(x[0]?.ToString(), out var v) ? v : 0)).ToList();
			}
			else
			{
				return new List<int>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity> GetByArticleAndVersion(int articleId, int? version)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__BSD_Stucklisten_Snapshot] WHERE [Artikel-Nr]=@articleId {(version.HasValue && version.Value >= 0 ? $" AND [BomVersion]={version.Value}" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity> GetByArticleAndVersion(int articleId, int? version, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			{
				string query = $"SELECT * FROM [__BSD_Stucklisten_Snapshot] WHERE [Artikel-Nr]=@articleId {(version.HasValue && version.Value >= 0 ? $" AND [BomVersion]={version.Value}" : "")}";
				var sqlCommand = new SqlCommand(query, connection, transaction);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity> GetLastByArticleAndIndex(int articleId, string indexKunde)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__BSD_Stucklisten_Snapshot] WHERE [Artikel-Nr]=@articleId AND TRIM(KundenIndex)=@indexKunde" +
					$" AND BomVersion = (SELECT Max(BomVersion) FROM [__BSD_Stucklisten_Snapshot] WHERE [Artikel-Nr]=@articleId AND TRIM(KundenIndex)=@indexKunde)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);
				sqlCommand.Parameters.AddWithValue("indexKunde", indexKunde == null ? (object)DBNull.Value : indexKunde.Trim());

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity> GetLastByArticleAndIndex(int articleId, string indexKunde, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();

			string query = $"SELECT * FROM [__BSD_Stucklisten_Snapshot] WHERE [Artikel-Nr]=@articleId AND TRIM(KundenIndex)=@indexKunde" +
				$" AND BomVersion = (SELECT Max(BomVersion) FROM [__BSD_Stucklisten_Snapshot] WHERE [Artikel-Nr]=@articleId AND TRIM(KundenIndex)=@indexKunde)";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("articleId", articleId);
			sqlCommand.Parameters.AddWithValue("indexKunde", indexKunde == null ? (object)DBNull.Value : indexKunde.Trim());

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity>();
			}
		}
		public static int Get_count(int articleId, int version)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT COUNT(*) FROM [__BSD_Stucklisten_Snapshot] WHERE [Artikel-Nr]=@articleId AND [BomVersion]=@version";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);
				sqlCommand.Parameters.AddWithValue("version", version);

				return int.TryParse(sqlCommand.ExecuteScalar()?.ToString(), out var v) ? v : 0;
			}
		}
		public static List<Tuple<int, int, int>> GetPositionsCount(List<Tuple<int, int>> articleIdVersions)
		{
			if(articleIdVersions == null || articleIdVersions.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT [Artikel-Nr], BomVersion, COUNT(*) counts FROM [__BSD_Stucklisten_Snapshot] WHERE ({string.Join(") OR (", articleIdVersions.Select(x => $"[Artikel-Nr]={x.Item1} AND [BomVersion]={x.Item2}")?.ToList())}) Group By [Artikel-Nr], BomVersion";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Tuple<int, int, int>(
					int.TryParse(x[0]?.ToString(), out var a) ? a : -1,
					int.TryParse(x[1]?.ToString(), out var b) ? b : -1,
					int.TryParse(x[2]?.ToString(), out var c) ? c : 0)).ToList();
			}
			else
			{
				return new List<Tuple<int, int, int>>();
			}
		}
		public static Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity GetFirstPosition(int articleId, int version)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT TOP 1 * FROM [__BSD_Stucklisten_Snapshot] WHERE [Artikel-Nr]=@articleId AND [BomVersion]=@version";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);
				sqlCommand.Parameters.AddWithValue("version", version);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity> GetFirstPositions(int articleId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"Select * From __BSD_Stucklisten_Snapshot 
	                                Where [Artikel-Nr]=@articleId
	                                AND Nr IN (Select min(Nr) Nr From __BSD_Stucklisten_Snapshot Group By [Artikel-Nr], BomVersion)
	                                Order by BomVersion";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity>();
			}
		}
		public static int Get_count(int articleId, int version, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			{
				string query = "SELECT COUNT(*) FROM [__BSD_Stucklisten_Snapshot] WHERE [Artikel-Nr]=@articleId AND [BomVersion]=@version";
				var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);
				sqlCommand.Parameters.AddWithValue("version", version);

				return int.TryParse(sqlCommand.ExecuteScalar()?.ToString(), out var v) ? v : 0;
			}
		}

		//     public static List<Tuple<int, int, int>> GetPositionsCount(List<Tuple<int, int>> articleIdVersions)
		//     {
		//         if (articleIdVersions == null || articleIdVersions.Count <= 0)
		//             return null;

		//         var dataTable = new DataTable();
		//         using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
		//         {
		//             sqlConnection.Open();
		//             string query = $"SELECT [Artikel-Nr], BomVersion, COUNT(*) counts FROM [__BSD_Stucklisten_Snapshot] WHERE ({string.Join(") OR (", articleIdVersions.Select(x => $"[Artikel-Nr]={x.Item1} AND [BomVersion]={x.Item2}")?.ToList())}) Group By [Artikel-Nr], BomVersion";
		//             var sqlCommand = new SqlCommand(query, sqlConnection);
		//             new SqlDataAdapter(sqlCommand).Fill(dataTable);
		//         }

		//         if (dataTable.Rows.Count > 0)
		//         {
		//             return dataTable.Rows.Cast<DataRow>().Select(x => new Tuple<int, int, int>(
		//                 int.TryParse(x[0]?.ToString(), out var a) ? a : -1,
		//                 int.TryParse(x[1]?.ToString(), out var b) ? b : -1,
		//                 int.TryParse(x[2]?.ToString(), out var c) ? c : 0)).ToList();
		//         }
		//         else
		//         {
		//             return new List<Tuple<int, int, int>>();
		//         }
		//     }
		//     public static Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity GetFirstPosition(int articleId, int version)
		//     {
		//         var dataTable = new DataTable();
		//         using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
		//         {
		//             sqlConnection.Open();
		//             string query = "SELECT TOP 1 * FROM [__BSD_Stucklisten_Snapshot] WHERE [Artikel-Nr]=@articleId AND [BomVersion]=@version";
		//             var sqlCommand = new SqlCommand(query, sqlConnection);
		//             sqlCommand.Parameters.AddWithValue("articleId", articleId);
		//             sqlCommand.Parameters.AddWithValue("version", version);

		//	new SqlDataAdapter(sqlCommand).Fill(dataTable);

		//}

		//         if (dataTable.Rows.Count > 0)
		//         {
		//             return new Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity(dataTable.Rows[0]);
		//         }
		//         else
		//         {
		//             return null;
		//         }
		//     }
		//     public static List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity> GetFirstPositions(int articleId)
		//     {
		//         var dataTable = new DataTable();
		//         using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
		//         {
		//             sqlConnection.Open();
		//             string query = $@"Select * From __BSD_Stucklisten_Snapshot 
		//                              Where [Artikel-Nr]=@articleId
		//                              AND Nr IN (Select min(Nr) Nr From __BSD_Stucklisten_Snapshot Group By [Artikel-Nr], BomVersion)
		//                              Order by BomVersion";
		//             var sqlCommand = new SqlCommand(query, sqlConnection);
		//             sqlCommand.Parameters.AddWithValue("articleId", articleId);
		//             new SqlDataAdapter(sqlCommand).Fill(dataTable);
		//         }

		//         if (dataTable.Rows.Count > 0)
		//         {
		//             return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity(x)).ToList();
		//         }
		//         else
		//         {
		//             return new List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity>();
		//         }
		//     }

		public static Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity GetLastByArticle(int articleId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT TOP 1 * FROM [__BSD_Stucklisten_Snapshot] WHERE [Artikel-Nr]=@articleId AND [BomVersion] IN (SELECT MAX(BomVersion) FROM [__BSD_Stucklisten_Snapshot] WHERE [Artikel-Nr]=@articleId GROUP BY [Artikel-Nr])";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<int> GetBomVersionList(int articleId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT DISTINCT [BomVersion] FROM [__BSD_Stucklisten_Snapshot] WHERE [Artikel-Nr]=@articleId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => int.TryParse(x[0]?.ToString(), out var v) ? v : 0).ToList();
			}
			else
			{
				return new List<int>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_KundenIndex> GetKundenIndexSnapshotTimeByArticle(int articleId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"Select KundenIndex, kundenIndexDate, SnapshotTime From (" +
					$"Select KundenIndex, Max(kundenIndexDate) kundenIndexDate, Max(SnapshotTime) SnapshotTime From [__BSD_Stucklisten_Snapshot] " +
					$"Where [Artikel-Nr]=@articleId Group By KundenIndex) AS tmp ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.Stucklisten_KundenIndex(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_KundenIndex>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_KundenIndex> GetKundenIndexSnapshotTimeByArticle(int articleId, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			string query = $"Select KundenIndex, kundenIndexDate, SnapshotTime From (" +
				$"Select KundenIndex, Max(kundenIndexDate) kundenIndexDate, Max(SnapshotTime) SnapshotTime From [__BSD_Stucklisten_Snapshot] " +
				$"Where [Artikel-Nr]=@articleId Group By KundenIndex) AS tmp ";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("articleId", articleId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.Stucklisten_KundenIndex(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_KundenIndex>();
			}
		}
		public static int OverwriteSnapshotWithTransaction(int articleId, int bomVersion, int userId, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__BSD_Stucklisten_Snapshot] SET [OverwrittenArticleId]=[Artikel-Nr] WHERE [Artikel-Nr]=@ArtikelNr AND [BomVersion]=@BomVersion;" +
				"UPDATE [__BSD_Stucklisten_Snapshot] SET [Artikel-Nr]=-1, [Overwritten]=1, [OverwrittenTime]=GETDATE(), [OverwrittenUserId]=@OverwrittenUserId WHERE [Artikel-Nr]=@ArtikelNr AND [BomVersion]=@BomVersion";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("ArtikelNr", articleId);
			sqlCommand.Parameters.AddWithValue("BomVersion", bomVersion);
			sqlCommand.Parameters.AddWithValue("OverwrittenUserId", userId);


			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity> GetByArticleAndKundenIndex(int articleId, string index)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__BSD_Stucklisten_Snapshot] WHERE [Artikel-Nr]=@articleId  AND [KundenIndex]=@index";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);
				sqlCommand.Parameters.AddWithValue("index", index ?? "");

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity>();
			}
		}
		#endregion
	}
}
