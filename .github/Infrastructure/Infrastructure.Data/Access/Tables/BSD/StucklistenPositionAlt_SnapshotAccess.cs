using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class StucklistenPositionAlt_SnapshotAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity Get(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_StucklistenPositionAlt_Snapshot] WHERE [Nr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", nr);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_StucklistenPositionAlt_Snapshot]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__BSD_StucklistenPositionAlt_Snapshot] WHERE [Nr] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__BSD_StucklistenPositionAlt_Snapshot] ([Anzahl],[ArtikelBezeichnung],[ArtikelNr],[ArtikelNummer],[BomVersion],[DocumentId],[LastUpdateTime],[LastUpdateUserId],[OriginalStucklistenNr],[Overwritten],[OverwrittenTime],[OverwrittenUserId],[ParentArtikelNr],[Position],[SnapshotTime],[SnapshotUserId]) OUTPUT INSERTED.[Nr] VALUES (@Anzahl,@ArtikelBezeichnung,@ArtikelNr,@ArtikelNummer,@BomVersion,@DocumentId,@LastUpdateTime,@LastUpdateUserId,@OriginalStucklistenNr,@Overwritten,@OverwrittenTime,@OverwrittenUserId,@ParentArtikelNr,@Position,@SnapshotTime,@SnapshotUserId); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("ArtikelBezeichnung", item.ArtikelBezeichnung == null ? (object)DBNull.Value : item.ArtikelBezeichnung);
					sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("ArtikelNummer", item.ArtikelNummer == null ? (object)DBNull.Value : item.ArtikelNummer);
					sqlCommand.Parameters.AddWithValue("BomVersion", item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
					sqlCommand.Parameters.AddWithValue("DocumentId", item.DocumentId == null ? (object)DBNull.Value : item.DocumentId);
					sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
					sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
					sqlCommand.Parameters.AddWithValue("OriginalStucklistenNr", item.OriginalStucklistenNr);
					sqlCommand.Parameters.AddWithValue("Overwritten", item.Overwritten == null ? (object)DBNull.Value : item.Overwritten);
					sqlCommand.Parameters.AddWithValue("OverwrittenTime", item.OverwrittenTime == null ? (object)DBNull.Value : item.OverwrittenTime);
					sqlCommand.Parameters.AddWithValue("OverwrittenUserId", item.OverwrittenUserId == null ? (object)DBNull.Value : item.OverwrittenUserId);
					sqlCommand.Parameters.AddWithValue("ParentArtikelNr", item.ParentArtikelNr);
					sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
					sqlCommand.Parameters.AddWithValue("SnapshotTime", item.SnapshotTime);
					sqlCommand.Parameters.AddWithValue("SnapshotUserId", item.SnapshotUserId);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 18; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity> items)
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
						query += " INSERT INTO [__BSD_StucklistenPositionAlt_Snapshot] ([Anzahl],[ArtikelBezeichnung],[ArtikelNr],[ArtikelNummer],[BomVersion],[DocumentId],[LastUpdateTime],[LastUpdateUserId],[OriginalStucklistenNr],[Overwritten],[OverwrittenTime],[OverwrittenUserId],[ParentArtikelNr],[Position],[SnapshotTime],[SnapshotUserId]) VALUES ( "

							+ "@Anzahl" + i + ","
							+ "@ArtikelBezeichnung" + i + ","
							+ "@ArtikelNr" + i + ","
							+ "@ArtikelNummer" + i + ","
							+ "@BomVersion" + i + ","
							+ "@DocumentId" + i + ","
							+ "@LastUpdateTime" + i + ","
							+ "@LastUpdateUserId" + i + ","
							+ "@OriginalStucklistenNr" + i + ","
							+ "@Overwritten" + i + ","
							+ "@OverwrittenTime" + i + ","
							+ "@OverwrittenUserId" + i + ","
							+ "@ParentArtikelNr" + i + ","
							+ "@Position" + i + ","
							+ "@SnapshotTime" + i + ","
							+ "@SnapshotUserId" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("ArtikelBezeichnung" + i, item.ArtikelBezeichnung == null ? (object)DBNull.Value : item.ArtikelBezeichnung);
						sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
						sqlCommand.Parameters.AddWithValue("ArtikelNummer" + i, item.ArtikelNummer == null ? (object)DBNull.Value : item.ArtikelNummer);
						sqlCommand.Parameters.AddWithValue("BomVersion" + i, item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
						sqlCommand.Parameters.AddWithValue("DocumentId" + i, item.DocumentId == null ? (object)DBNull.Value : item.DocumentId);
						sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
						sqlCommand.Parameters.AddWithValue("LastUpdateUserId" + i, item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
						sqlCommand.Parameters.AddWithValue("OriginalStucklistenNr" + i, item.OriginalStucklistenNr);
						sqlCommand.Parameters.AddWithValue("Overwritten" + i, item.Overwritten == null ? (object)DBNull.Value : item.Overwritten);
						sqlCommand.Parameters.AddWithValue("OverwrittenTime" + i, item.OverwrittenTime == null ? (object)DBNull.Value : item.OverwrittenTime);
						sqlCommand.Parameters.AddWithValue("OverwrittenUserId" + i, item.OverwrittenUserId == null ? (object)DBNull.Value : item.OverwrittenUserId);
						sqlCommand.Parameters.AddWithValue("ParentArtikelNr" + i, item.ParentArtikelNr);
						sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
						sqlCommand.Parameters.AddWithValue("SnapshotTime" + i, item.SnapshotTime);
						sqlCommand.Parameters.AddWithValue("SnapshotUserId" + i, item.SnapshotUserId);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__BSD_StucklistenPositionAlt_Snapshot] SET [Anzahl]=@Anzahl, [ArtikelBezeichnung]=@ArtikelBezeichnung, [ArtikelNr]=@ArtikelNr, [ArtikelNummer]=@ArtikelNummer, [BomVersion]=@BomVersion, [DocumentId]=@DocumentId, [LastUpdateTime]=@LastUpdateTime, [LastUpdateUserId]=@LastUpdateUserId, [OriginalStucklistenNr]=@OriginalStucklistenNr, [Overwritten]=@Overwritten, [OverwrittenTime]=@OverwrittenTime, [OverwrittenUserId]=@OverwrittenUserId, [ParentArtikelNr]=@ParentArtikelNr, [Position]=@Position, [SnapshotTime]=@SnapshotTime, [SnapshotUserId]=@SnapshotUserId WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
				sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
				sqlCommand.Parameters.AddWithValue("ArtikelBezeichnung", item.ArtikelBezeichnung == null ? (object)DBNull.Value : item.ArtikelBezeichnung);
				sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
				sqlCommand.Parameters.AddWithValue("ArtikelNummer", item.ArtikelNummer == null ? (object)DBNull.Value : item.ArtikelNummer);
				sqlCommand.Parameters.AddWithValue("BomVersion", item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
				sqlCommand.Parameters.AddWithValue("DocumentId", item.DocumentId == null ? (object)DBNull.Value : item.DocumentId);
				sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
				sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
				sqlCommand.Parameters.AddWithValue("OriginalStucklistenNr", item.OriginalStucklistenNr);
				sqlCommand.Parameters.AddWithValue("Overwritten", item.Overwritten == null ? (object)DBNull.Value : item.Overwritten);
				sqlCommand.Parameters.AddWithValue("OverwrittenTime", item.OverwrittenTime == null ? (object)DBNull.Value : item.OverwrittenTime);
				sqlCommand.Parameters.AddWithValue("OverwrittenUserId", item.OverwrittenUserId == null ? (object)DBNull.Value : item.OverwrittenUserId);
				sqlCommand.Parameters.AddWithValue("ParentArtikelNr", item.ParentArtikelNr);
				sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
				sqlCommand.Parameters.AddWithValue("SnapshotTime", item.SnapshotTime);
				sqlCommand.Parameters.AddWithValue("SnapshotUserId", item.SnapshotUserId);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 18; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity> items)
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
						query += " UPDATE [__BSD_StucklistenPositionAlt_Snapshot] SET "

							+ "[Anzahl]=@Anzahl" + i + ","
							+ "[ArtikelBezeichnung]=@ArtikelBezeichnung" + i + ","
							+ "[ArtikelNr]=@ArtikelNr" + i + ","
							+ "[ArtikelNummer]=@ArtikelNummer" + i + ","
							+ "[BomVersion]=@BomVersion" + i + ","
							+ "[DocumentId]=@DocumentId" + i + ","
							+ "[LastUpdateTime]=@LastUpdateTime" + i + ","
							+ "[LastUpdateUserId]=@LastUpdateUserId" + i + ","
							+ "[OriginalStucklistenNr]=@OriginalStucklistenNr" + i + ","
							+ "[Overwritten]=@Overwritten" + i + ","
							+ "[OverwrittenTime]=@OverwrittenTime" + i + ","
							+ "[OverwrittenUserId]=@OverwrittenUserId" + i + ","
							+ "[ParentArtikelNr]=@ParentArtikelNr" + i + ","
							+ "[Position]=@Position" + i + ","
							+ "[SnapshotTime]=@SnapshotTime" + i + ","
							+ "[SnapshotUserId]=@SnapshotUserId" + i + " WHERE [Nr]=@Nr" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("ArtikelBezeichnung" + i, item.ArtikelBezeichnung == null ? (object)DBNull.Value : item.ArtikelBezeichnung);
						sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
						sqlCommand.Parameters.AddWithValue("ArtikelNummer" + i, item.ArtikelNummer == null ? (object)DBNull.Value : item.ArtikelNummer);
						sqlCommand.Parameters.AddWithValue("BomVersion" + i, item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
						sqlCommand.Parameters.AddWithValue("DocumentId" + i, item.DocumentId == null ? (object)DBNull.Value : item.DocumentId);
						sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
						sqlCommand.Parameters.AddWithValue("LastUpdateUserId" + i, item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
						sqlCommand.Parameters.AddWithValue("OriginalStucklistenNr" + i, item.OriginalStucklistenNr);
						sqlCommand.Parameters.AddWithValue("Overwritten" + i, item.Overwritten == null ? (object)DBNull.Value : item.Overwritten);
						sqlCommand.Parameters.AddWithValue("OverwrittenTime" + i, item.OverwrittenTime == null ? (object)DBNull.Value : item.OverwrittenTime);
						sqlCommand.Parameters.AddWithValue("OverwrittenUserId" + i, item.OverwrittenUserId == null ? (object)DBNull.Value : item.OverwrittenUserId);
						sqlCommand.Parameters.AddWithValue("ParentArtikelNr" + i, item.ParentArtikelNr);
						sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
						sqlCommand.Parameters.AddWithValue("SnapshotTime" + i, item.SnapshotTime);
						sqlCommand.Parameters.AddWithValue("SnapshotUserId" + i, item.SnapshotUserId);
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
				string query = "DELETE FROM [__BSD_StucklistenPositionAlt_Snapshot] WHERE [Nr]=@Nr";
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

					string query = "DELETE FROM [__BSD_StucklistenPositionAlt_Snapshot] WHERE [Nr] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity GetWithTransaction(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__BSD_StucklistenPositionAlt_Snapshot] WHERE [Nr]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", nr);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__BSD_StucklistenPositionAlt_Snapshot]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__BSD_StucklistenPositionAlt_Snapshot] WHERE [Nr] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [__BSD_StucklistenPositionAlt_Snapshot] ([Anzahl],[ArtikelBezeichnung],[ArtikelNr],[ArtikelNummer],[BomVersion],[DocumentId],[LastUpdateTime],[LastUpdateUserId],[OriginalStucklistenNr],[Overwritten],[OverwrittenTime],[OverwrittenUserId],[ParentArtikelNr],[Position],[SnapshotTime],[SnapshotUserId]) OUTPUT INSERTED.[Nr] VALUES (@Anzahl,@ArtikelBezeichnung,@ArtikelNr,@ArtikelNummer,@BomVersion,@DocumentId,@LastUpdateTime,@LastUpdateUserId,@OriginalStucklistenNr,@Overwritten,@OverwrittenTime,@OverwrittenUserId,@ParentArtikelNr,@Position,@SnapshotTime,@SnapshotUserId); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
			sqlCommand.Parameters.AddWithValue("ArtikelBezeichnung", item.ArtikelBezeichnung == null ? (object)DBNull.Value : item.ArtikelBezeichnung);
			sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
			sqlCommand.Parameters.AddWithValue("ArtikelNummer", item.ArtikelNummer == null ? (object)DBNull.Value : item.ArtikelNummer);
			sqlCommand.Parameters.AddWithValue("BomVersion", item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
			sqlCommand.Parameters.AddWithValue("DocumentId", item.DocumentId == null ? (object)DBNull.Value : item.DocumentId);
			sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
			sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
			sqlCommand.Parameters.AddWithValue("OriginalStucklistenNr", item.OriginalStucklistenNr);
			sqlCommand.Parameters.AddWithValue("Overwritten", item.Overwritten == null ? (object)DBNull.Value : item.Overwritten);
			sqlCommand.Parameters.AddWithValue("OverwrittenTime", item.OverwrittenTime == null ? (object)DBNull.Value : item.OverwrittenTime);
			sqlCommand.Parameters.AddWithValue("OverwrittenUserId", item.OverwrittenUserId == null ? (object)DBNull.Value : item.OverwrittenUserId);
			sqlCommand.Parameters.AddWithValue("ParentArtikelNr", item.ParentArtikelNr);
			sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
			sqlCommand.Parameters.AddWithValue("SnapshotTime", item.SnapshotTime);
			sqlCommand.Parameters.AddWithValue("SnapshotUserId", item.SnapshotUserId);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 18; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__BSD_StucklistenPositionAlt_Snapshot] ([Anzahl],[ArtikelBezeichnung],[ArtikelNr],[ArtikelNummer],[BomVersion],[DocumentId],[LastUpdateTime],[LastUpdateUserId],[OriginalStucklistenNr],[Overwritten],[OverwrittenTime],[OverwrittenUserId],[ParentArtikelNr],[Position],[SnapshotTime],[SnapshotUserId]) VALUES ( "

						+ "@Anzahl" + i + ","
						+ "@ArtikelBezeichnung" + i + ","
						+ "@ArtikelNr" + i + ","
						+ "@ArtikelNummer" + i + ","
						+ "@BomVersion" + i + ","
						+ "@DocumentId" + i + ","
						+ "@LastUpdateTime" + i + ","
						+ "@LastUpdateUserId" + i + ","
						+ "@OriginalStucklistenNr" + i + ","
						+ "@Overwritten" + i + ","
						+ "@OverwrittenTime" + i + ","
						+ "@OverwrittenUserId" + i + ","
						+ "@ParentArtikelNr" + i + ","
						+ "@Position" + i + ","
						+ "@SnapshotTime" + i + ","
						+ "@SnapshotUserId" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("ArtikelBezeichnung" + i, item.ArtikelBezeichnung == null ? (object)DBNull.Value : item.ArtikelBezeichnung);
					sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("ArtikelNummer" + i, item.ArtikelNummer == null ? (object)DBNull.Value : item.ArtikelNummer);
					sqlCommand.Parameters.AddWithValue("BomVersion" + i, item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
					sqlCommand.Parameters.AddWithValue("DocumentId" + i, item.DocumentId == null ? (object)DBNull.Value : item.DocumentId);
					sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
					sqlCommand.Parameters.AddWithValue("LastUpdateUserId" + i, item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
					sqlCommand.Parameters.AddWithValue("OriginalStucklistenNr" + i, item.OriginalStucklistenNr);
					sqlCommand.Parameters.AddWithValue("Overwritten" + i, item.Overwritten == null ? (object)DBNull.Value : item.Overwritten);
					sqlCommand.Parameters.AddWithValue("OverwrittenTime" + i, item.OverwrittenTime == null ? (object)DBNull.Value : item.OverwrittenTime);
					sqlCommand.Parameters.AddWithValue("OverwrittenUserId" + i, item.OverwrittenUserId == null ? (object)DBNull.Value : item.OverwrittenUserId);
					sqlCommand.Parameters.AddWithValue("ParentArtikelNr" + i, item.ParentArtikelNr);
					sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
					sqlCommand.Parameters.AddWithValue("SnapshotTime" + i, item.SnapshotTime);
					sqlCommand.Parameters.AddWithValue("SnapshotUserId" + i, item.SnapshotUserId);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__BSD_StucklistenPositionAlt_Snapshot] SET [Anzahl]=@Anzahl, [ArtikelBezeichnung]=@ArtikelBezeichnung, [ArtikelNr]=@ArtikelNr, [ArtikelNummer]=@ArtikelNummer, [BomVersion]=@BomVersion, [DocumentId]=@DocumentId, [LastUpdateTime]=@LastUpdateTime, [LastUpdateUserId]=@LastUpdateUserId, [OriginalStucklistenNr]=@OriginalStucklistenNr, [Overwritten]=@Overwritten, [OverwrittenTime]=@OverwrittenTime, [OverwrittenUserId]=@OverwrittenUserId, [ParentArtikelNr]=@ParentArtikelNr, [Position]=@Position, [SnapshotTime]=@SnapshotTime, [SnapshotUserId]=@SnapshotUserId WHERE [Nr]=@Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
			sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
			sqlCommand.Parameters.AddWithValue("ArtikelBezeichnung", item.ArtikelBezeichnung == null ? (object)DBNull.Value : item.ArtikelBezeichnung);
			sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
			sqlCommand.Parameters.AddWithValue("ArtikelNummer", item.ArtikelNummer == null ? (object)DBNull.Value : item.ArtikelNummer);
			sqlCommand.Parameters.AddWithValue("BomVersion", item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
			sqlCommand.Parameters.AddWithValue("DocumentId", item.DocumentId == null ? (object)DBNull.Value : item.DocumentId);
			sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
			sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
			sqlCommand.Parameters.AddWithValue("OriginalStucklistenNr", item.OriginalStucklistenNr);
			sqlCommand.Parameters.AddWithValue("Overwritten", item.Overwritten == null ? (object)DBNull.Value : item.Overwritten);
			sqlCommand.Parameters.AddWithValue("OverwrittenTime", item.OverwrittenTime == null ? (object)DBNull.Value : item.OverwrittenTime);
			sqlCommand.Parameters.AddWithValue("OverwrittenUserId", item.OverwrittenUserId == null ? (object)DBNull.Value : item.OverwrittenUserId);
			sqlCommand.Parameters.AddWithValue("ParentArtikelNr", item.ParentArtikelNr);
			sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
			sqlCommand.Parameters.AddWithValue("SnapshotTime", item.SnapshotTime);
			sqlCommand.Parameters.AddWithValue("SnapshotUserId", item.SnapshotUserId);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 18; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionAlt_SnapshotEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__BSD_StucklistenPositionAlt_Snapshot] SET "

					+ "[Anzahl]=@Anzahl" + i + ","
					+ "[ArtikelBezeichnung]=@ArtikelBezeichnung" + i + ","
					+ "[ArtikelNr]=@ArtikelNr" + i + ","
					+ "[ArtikelNummer]=@ArtikelNummer" + i + ","
					+ "[BomVersion]=@BomVersion" + i + ","
					+ "[DocumentId]=@DocumentId" + i + ","
					+ "[LastUpdateTime]=@LastUpdateTime" + i + ","
					+ "[LastUpdateUserId]=@LastUpdateUserId" + i + ","
					+ "[OriginalStucklistenNr]=@OriginalStucklistenNr" + i + ","
					+ "[Overwritten]=@Overwritten" + i + ","
					+ "[OverwrittenTime]=@OverwrittenTime" + i + ","
					+ "[OverwrittenUserId]=@OverwrittenUserId" + i + ","
					+ "[ParentArtikelNr]=@ParentArtikelNr" + i + ","
					+ "[Position]=@Position" + i + ","
					+ "[SnapshotTime]=@SnapshotTime" + i + ","
					+ "[SnapshotUserId]=@SnapshotUserId" + i + " WHERE [Nr]=@Nr" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
					sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("ArtikelBezeichnung" + i, item.ArtikelBezeichnung == null ? (object)DBNull.Value : item.ArtikelBezeichnung);
					sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("ArtikelNummer" + i, item.ArtikelNummer == null ? (object)DBNull.Value : item.ArtikelNummer);
					sqlCommand.Parameters.AddWithValue("BomVersion" + i, item.BomVersion == null ? (object)DBNull.Value : item.BomVersion);
					sqlCommand.Parameters.AddWithValue("DocumentId" + i, item.DocumentId == null ? (object)DBNull.Value : item.DocumentId);
					sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
					sqlCommand.Parameters.AddWithValue("LastUpdateUserId" + i, item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
					sqlCommand.Parameters.AddWithValue("OriginalStucklistenNr" + i, item.OriginalStucklistenNr);
					sqlCommand.Parameters.AddWithValue("Overwritten" + i, item.Overwritten == null ? (object)DBNull.Value : item.Overwritten);
					sqlCommand.Parameters.AddWithValue("OverwrittenTime" + i, item.OverwrittenTime == null ? (object)DBNull.Value : item.OverwrittenTime);
					sqlCommand.Parameters.AddWithValue("OverwrittenUserId" + i, item.OverwrittenUserId == null ? (object)DBNull.Value : item.OverwrittenUserId);
					sqlCommand.Parameters.AddWithValue("ParentArtikelNr" + i, item.ParentArtikelNr);
					sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
					sqlCommand.Parameters.AddWithValue("SnapshotTime" + i, item.SnapshotTime);
					sqlCommand.Parameters.AddWithValue("SnapshotUserId" + i, item.SnapshotUserId);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__BSD_StucklistenPositionAlt_Snapshot] WHERE [Nr]=@Nr";
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

				string query = "DELETE FROM [__BSD_StucklistenPositionAlt_Snapshot] WHERE [Nr] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods

		public static int Get_count(int parentArticleId, int version)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT COUNT(*) FROM [__BSD_StucklistenPositionAlt_Snapshot] WHERE [ParentArtikelNr]=@articleId AND [BomVersion]=@version";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", parentArticleId);
				sqlCommand.Parameters.AddWithValue("version", version);

				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand)?.ToString(), out var v) ? v : 0;
			}
		}
		public static int Get_count(int parentArticleId, int version, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			{
				string query = "SELECT COUNT(*) FROM [__BSD_StucklistenPositionAlt_Snapshot] WHERE [ParentArtikelNr]=@articleId AND [BomVersion]=@version";
				var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
				sqlCommand.Parameters.AddWithValue("articleId", parentArticleId);
				sqlCommand.Parameters.AddWithValue("version", version);

				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand)?.ToString(), out var v) ? v : 0;
			}
		}
		public static int OverwriteSnapshotWithTransaction(int articleId, int bomVersion, int userId, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__BSD_StucklistenPositionAlt_Snapshot] SET [OverwrittenArticleId]=[ArtikelNr] WHERE [ArtikelNr]=@ArtikelNr AND [BomVersion]=@BomVersion;" +
				"UPDATE [__BSD_StucklistenPositionAlt_Snapshot] SET [ArtikelNr]=-1, [Overwritten]=1, [OverwrittenTime]=GETDATE(), [OverwrittenUserId]=@OverwrittenUserId WHERE [ArtikelNr]=@ArtikelNr AND [BomVersion]=@BomVersion";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("ArtikelNr", articleId);
			sqlCommand.Parameters.AddWithValue("BomVersion", bomVersion);
			sqlCommand.Parameters.AddWithValue("OverwrittenUserId", userId);


			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		#endregion
	}
}
