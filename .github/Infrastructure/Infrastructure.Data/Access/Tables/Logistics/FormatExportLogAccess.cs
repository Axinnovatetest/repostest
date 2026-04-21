using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.Logistics
{
	public class FormatExportLogAccess
	{
		#region Default Methods
		public static Entities.Tables.LGT.FormatExportLogEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__LGT_FormatExportLog] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.LGT.FormatExportLogEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.LGT.FormatExportLogEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__LGT_FormatExportLog]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.LGT.FormatExportLogEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.LGT.FormatExportLogEntity>();
			}
		}
		public static List<Entities.Tables.LGT.FormatExportLogEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Entities.Tables.LGT.FormatExportLogEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Entities.Tables.LGT.FormatExportLogEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Entities.Tables.LGT.FormatExportLogEntity>();
		}
		private static List<Entities.Tables.LGT.FormatExportLogEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__LGT_FormatExportLog] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.LGT.FormatExportLogEntity(x)).ToList();
				}
				else
				{
					return new List<Entities.Tables.LGT.FormatExportLogEntity>();
				}
			}
			return new List<Entities.Tables.LGT.FormatExportLogEntity>();
		}

		public static int Insert(Entities.Tables.LGT.FormatExportLogEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__LGT_FormatExportLog] ([ExportDate],[ExportUserId],[ExportUserName],[LagerBewegungId],[SelectedDate],[SelectedLagerFrom],[SelectedLagerTo]) OUTPUT INSERTED.[Id] VALUES (@ExportDate,@ExportUserId,@ExportUserName,@LagerBewegungId,@SelectedDate,@SelectedLagerFrom,@SelectedLagerTo); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ExportDate", item.ExportDate == null ? DBNull.Value : item.ExportDate);
					sqlCommand.Parameters.AddWithValue("ExportUserId", item.ExportUserId == null ? DBNull.Value : item.ExportUserId);
					sqlCommand.Parameters.AddWithValue("ExportUserName", item.ExportUserName == null ? DBNull.Value : item.ExportUserName);
					sqlCommand.Parameters.AddWithValue("LagerBewegungId", item.LagerBewegungId == null ? DBNull.Value : item.LagerBewegungId);
					sqlCommand.Parameters.AddWithValue("SelectedDate", item.SelectedDate == null ? DBNull.Value : item.SelectedDate);
					sqlCommand.Parameters.AddWithValue("SelectedLagerFrom", item.SelectedLagerFrom == null ? DBNull.Value : item.SelectedLagerFrom);
					sqlCommand.Parameters.AddWithValue("SelectedLagerTo", item.SelectedLagerTo == null ? DBNull.Value : item.SelectedLagerTo);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Entities.Tables.LGT.FormatExportLogEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 8; // Nb params per query
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
		private static int insert(List<Entities.Tables.LGT.FormatExportLogEntity> items)
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
						query += " INSERT INTO [__LGT_FormatExportLog] ([ExportDate],[ExportUserId],[ExportUserName],[LagerBewegungId],[SelectedDate],[SelectedLagerFrom],[SelectedLagerTo]) VALUES ( "

							+ "@ExportDate" + i + ","
							+ "@ExportUserId" + i + ","
							+ "@ExportUserName" + i + ","
							+ "@LagerBewegungId" + i + ","
							+ "@SelectedDate" + i + ","
							+ "@SelectedLagerFrom" + i + ","
							+ "@SelectedLagerTo" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ExportDate" + i, item.ExportDate == null ? DBNull.Value : item.ExportDate);
						sqlCommand.Parameters.AddWithValue("ExportUserId" + i, item.ExportUserId == null ? DBNull.Value : item.ExportUserId);
						sqlCommand.Parameters.AddWithValue("ExportUserName" + i, item.ExportUserName == null ? DBNull.Value : item.ExportUserName);
						sqlCommand.Parameters.AddWithValue("LagerBewegungId" + i, item.LagerBewegungId == null ? DBNull.Value : item.LagerBewegungId);
						sqlCommand.Parameters.AddWithValue("SelectedDate" + i, item.SelectedDate == null ? DBNull.Value : item.SelectedDate);
						sqlCommand.Parameters.AddWithValue("SelectedLagerFrom" + i, item.SelectedLagerFrom == null ? DBNull.Value : item.SelectedLagerFrom);
						sqlCommand.Parameters.AddWithValue("SelectedLagerTo" + i, item.SelectedLagerTo == null ? DBNull.Value : item.SelectedLagerTo);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Entities.Tables.LGT.FormatExportLogEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__LGT_FormatExportLog] SET [ExportDate]=@ExportDate, [ExportUserId]=@ExportUserId, [ExportUserName]=@ExportUserName, [LagerBewegungId]=@LagerBewegungId, [SelectedDate]=@SelectedDate, [SelectedLagerFrom]=@SelectedLagerFrom, [SelectedLagerTo]=@SelectedLagerTo WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ExportDate", item.ExportDate == null ? DBNull.Value : item.ExportDate);
				sqlCommand.Parameters.AddWithValue("ExportUserId", item.ExportUserId == null ? DBNull.Value : item.ExportUserId);
				sqlCommand.Parameters.AddWithValue("ExportUserName", item.ExportUserName == null ? DBNull.Value : item.ExportUserName);
				sqlCommand.Parameters.AddWithValue("LagerBewegungId", item.LagerBewegungId == null ? DBNull.Value : item.LagerBewegungId);
				sqlCommand.Parameters.AddWithValue("SelectedDate", item.SelectedDate == null ? DBNull.Value : item.SelectedDate);
				sqlCommand.Parameters.AddWithValue("SelectedLagerFrom", item.SelectedLagerFrom == null ? DBNull.Value : item.SelectedLagerFrom);
				sqlCommand.Parameters.AddWithValue("SelectedLagerTo", item.SelectedLagerTo == null ? DBNull.Value : item.SelectedLagerTo);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Entities.Tables.LGT.FormatExportLogEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 8; // Nb params per query
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
		private static int update(List<Entities.Tables.LGT.FormatExportLogEntity> items)
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
						query += " UPDATE [__LGT_FormatExportLog] SET "

							+ "[ExportDate]=@ExportDate" + i + ","
							+ "[ExportUserId]=@ExportUserId" + i + ","
							+ "[ExportUserName]=@ExportUserName" + i + ","
							+ "[LagerBewegungId]=@LagerBewegungId" + i + ","
							+ "[SelectedDate]=@SelectedDate" + i + ","
							+ "[SelectedLagerFrom]=@SelectedLagerFrom" + i + ","
							+ "[SelectedLagerTo]=@SelectedLagerTo" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ExportDate" + i, item.ExportDate == null ? DBNull.Value : item.ExportDate);
						sqlCommand.Parameters.AddWithValue("ExportUserId" + i, item.ExportUserId == null ? DBNull.Value : item.ExportUserId);
						sqlCommand.Parameters.AddWithValue("ExportUserName" + i, item.ExportUserName == null ? DBNull.Value : item.ExportUserName);
						sqlCommand.Parameters.AddWithValue("LagerBewegungId" + i, item.LagerBewegungId == null ? DBNull.Value : item.LagerBewegungId);
						sqlCommand.Parameters.AddWithValue("SelectedDate" + i, item.SelectedDate == null ? DBNull.Value : item.SelectedDate);
						sqlCommand.Parameters.AddWithValue("SelectedLagerFrom" + i, item.SelectedLagerFrom == null ? DBNull.Value : item.SelectedLagerFrom);
						sqlCommand.Parameters.AddWithValue("SelectedLagerTo" + i, item.SelectedLagerTo == null ? DBNull.Value : item.SelectedLagerTo);
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
				string query = "DELETE FROM [__LGT_FormatExportLog] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

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

					string query = "DELETE FROM [__LGT_FormatExportLog] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Entities.Tables.LGT.FormatExportLogEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__LGT_FormatExportLog] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.LGT.FormatExportLogEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.LGT.FormatExportLogEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__LGT_FormatExportLog]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.LGT.FormatExportLogEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.LGT.FormatExportLogEntity>();
			}
		}
		public static List<Entities.Tables.LGT.FormatExportLogEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Entities.Tables.LGT.FormatExportLogEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Entities.Tables.LGT.FormatExportLogEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Entities.Tables.LGT.FormatExportLogEntity>();
		}
		private static List<Entities.Tables.LGT.FormatExportLogEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__LGT_FormatExportLog] WHERE [Id] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.LGT.FormatExportLogEntity(x)).ToList();
				}
				else
				{
					return new List<Entities.Tables.LGT.FormatExportLogEntity>();
				}
			}
			return new List<Entities.Tables.LGT.FormatExportLogEntity>();
		}

		public static int InsertWithTransaction(Entities.Tables.LGT.FormatExportLogEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [__LGT_FormatExportLog] ([ExportDate],[ExportUserId],[ExportUserName],[LagerBewegungId],[SelectedDate],[SelectedLagerFrom],[SelectedLagerTo]) OUTPUT INSERTED.[Id] VALUES (@ExportDate,@ExportUserId,@ExportUserName,@LagerBewegungId,@SelectedDate,@SelectedLagerFrom,@SelectedLagerTo); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ExportDate", item.ExportDate == null ? DBNull.Value : item.ExportDate);
			sqlCommand.Parameters.AddWithValue("ExportUserId", item.ExportUserId == null ? DBNull.Value : item.ExportUserId);
			sqlCommand.Parameters.AddWithValue("ExportUserName", item.ExportUserName == null ? DBNull.Value : item.ExportUserName);
			sqlCommand.Parameters.AddWithValue("LagerBewegungId", item.LagerBewegungId == null ? DBNull.Value : item.LagerBewegungId);
			sqlCommand.Parameters.AddWithValue("SelectedDate", item.SelectedDate == null ? DBNull.Value : item.SelectedDate);
			sqlCommand.Parameters.AddWithValue("SelectedLagerFrom", item.SelectedLagerFrom == null ? DBNull.Value : item.SelectedLagerFrom);
			sqlCommand.Parameters.AddWithValue("SelectedLagerTo", item.SelectedLagerTo == null ? DBNull.Value : item.SelectedLagerTo);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Entities.Tables.LGT.FormatExportLogEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 8; // Nb params per query
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
		private static int insertWithTransaction(List<Entities.Tables.LGT.FormatExportLogEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__LGT_FormatExportLog] ([ExportDate],[ExportUserId],[ExportUserName],[LagerBewegungId],[SelectedDate],[SelectedLagerFrom],[SelectedLagerTo]) VALUES ( "

						+ "@ExportDate" + i + ","
						+ "@ExportUserId" + i + ","
						+ "@ExportUserName" + i + ","
						+ "@LagerBewegungId" + i + ","
						+ "@SelectedDate" + i + ","
						+ "@SelectedLagerFrom" + i + ","
						+ "@SelectedLagerTo" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("ExportDate" + i, item.ExportDate == null ? DBNull.Value : item.ExportDate);
					sqlCommand.Parameters.AddWithValue("ExportUserId" + i, item.ExportUserId == null ? DBNull.Value : item.ExportUserId);
					sqlCommand.Parameters.AddWithValue("ExportUserName" + i, item.ExportUserName == null ? DBNull.Value : item.ExportUserName);
					sqlCommand.Parameters.AddWithValue("LagerBewegungId" + i, item.LagerBewegungId == null ? DBNull.Value : item.LagerBewegungId);
					sqlCommand.Parameters.AddWithValue("SelectedDate" + i, item.SelectedDate == null ? DBNull.Value : item.SelectedDate);
					sqlCommand.Parameters.AddWithValue("SelectedLagerFrom" + i, item.SelectedLagerFrom == null ? DBNull.Value : item.SelectedLagerFrom);
					sqlCommand.Parameters.AddWithValue("SelectedLagerTo" + i, item.SelectedLagerTo == null ? DBNull.Value : item.SelectedLagerTo);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Entities.Tables.LGT.FormatExportLogEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__LGT_FormatExportLog] SET [ExportDate]=@ExportDate, [ExportUserId]=@ExportUserId, [ExportUserName]=@ExportUserName, [LagerBewegungId]=@LagerBewegungId, [SelectedDate]=@SelectedDate, [SelectedLagerFrom]=@SelectedLagerFrom, [SelectedLagerTo]=@SelectedLagerTo WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("ExportDate", item.ExportDate == null ? DBNull.Value : item.ExportDate);
			sqlCommand.Parameters.AddWithValue("ExportUserId", item.ExportUserId == null ? DBNull.Value : item.ExportUserId);
			sqlCommand.Parameters.AddWithValue("ExportUserName", item.ExportUserName == null ? DBNull.Value : item.ExportUserName);
			sqlCommand.Parameters.AddWithValue("LagerBewegungId", item.LagerBewegungId == null ? DBNull.Value : item.LagerBewegungId);
			sqlCommand.Parameters.AddWithValue("SelectedDate", item.SelectedDate == null ? DBNull.Value : item.SelectedDate);
			sqlCommand.Parameters.AddWithValue("SelectedLagerFrom", item.SelectedLagerFrom == null ? DBNull.Value : item.SelectedLagerFrom);
			sqlCommand.Parameters.AddWithValue("SelectedLagerTo", item.SelectedLagerTo == null ? DBNull.Value : item.SelectedLagerTo);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Entities.Tables.LGT.FormatExportLogEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 8; // Nb params per query
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
		private static int updateWithTransaction(List<Entities.Tables.LGT.FormatExportLogEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__LGT_FormatExportLog] SET "

					+ "[ExportDate]=@ExportDate" + i + ","
					+ "[ExportUserId]=@ExportUserId" + i + ","
					+ "[ExportUserName]=@ExportUserName" + i + ","
					+ "[LagerBewegungId]=@LagerBewegungId" + i + ","
					+ "[SelectedDate]=@SelectedDate" + i + ","
					+ "[SelectedLagerFrom]=@SelectedLagerFrom" + i + ","
					+ "[SelectedLagerTo]=@SelectedLagerTo" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("ExportDate" + i, item.ExportDate == null ? DBNull.Value : item.ExportDate);
					sqlCommand.Parameters.AddWithValue("ExportUserId" + i, item.ExportUserId == null ? DBNull.Value : item.ExportUserId);
					sqlCommand.Parameters.AddWithValue("ExportUserName" + i, item.ExportUserName == null ? DBNull.Value : item.ExportUserName);
					sqlCommand.Parameters.AddWithValue("LagerBewegungId" + i, item.LagerBewegungId == null ? DBNull.Value : item.LagerBewegungId);
					sqlCommand.Parameters.AddWithValue("SelectedDate" + i, item.SelectedDate == null ? DBNull.Value : item.SelectedDate);
					sqlCommand.Parameters.AddWithValue("SelectedLagerFrom" + i, item.SelectedLagerFrom == null ? DBNull.Value : item.SelectedLagerFrom);
					sqlCommand.Parameters.AddWithValue("SelectedLagerTo" + i, item.SelectedLagerTo == null ? DBNull.Value : item.SelectedLagerTo);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__LGT_FormatExportLog] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

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

				string query = "DELETE FROM [__LGT_FormatExportLog] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Entities.Tables.LGT.FormatExportLogEntity> Get(IEnumerable<Tuple<DateTime, int, int>> data)
		{
			if(data is null || data.Count()<=0)
			{
				return null;
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__LGT_FormatExportLog] WHERE {string.Join(" OR ",data.Select(x=> $"([SelectedDate]='{x.Item1.ToString("yyyyMMdd")}' AND [SelectedLagerFrom]={x.Item2} AND [SelectedLagerTo]={x.Item3})"))}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.LGT.FormatExportLogEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.LGT.FormatExportLogEntity>();
			}
		}
		public static bool TransferSent(DateTime date, int lagerFrom, int lagerTo)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT COUNT(Id) FROM [__LGT_FormatExportLog] WHERE [SelectedDate]='{date.ToString("yyyyMMdd")}' AND [SelectedLagerFrom]={lagerFrom} AND [SelectedLagerTo]={lagerTo}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				return (int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var _x) ? _x : 0)>0;
			}
		}
		
		#endregion Custom Methods

	}
}
