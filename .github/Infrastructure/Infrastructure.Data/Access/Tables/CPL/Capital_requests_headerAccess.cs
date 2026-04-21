using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.CPL
{
	public class Capital_requests_headerAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Capital_requests_header] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Capital_requests_header]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Capital_requests_header] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Capital_requests_header] ([Artikelnummer],[CloseDate],[Date],[Fertigungsnummer],[Plant],[PlantId],[Status],[StatusId],[UserId],[UserName]) OUTPUT INSERTED.[Id] VALUES (@Artikelnummer,@CloseDate,@Date,@Fertigungsnummer,@Plant,@PlantId,@Status,@StatusId,@UserId,@UserName); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("CloseDate", item.CloseDate == null ? (object)DBNull.Value : item.CloseDate);
					sqlCommand.Parameters.AddWithValue("Date", item.Date == null ? (object)DBNull.Value : item.Date);
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("Plant", item.Plant == null ? (object)DBNull.Value : item.Plant);
					sqlCommand.Parameters.AddWithValue("PlantId", item.PlantId == null ? (object)DBNull.Value : item.PlantId);
					sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("StatusId", item.StatusId == null ? (object)DBNull.Value : item.StatusId);
					sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
					sqlCommand.Parameters.AddWithValue("UserName", item.UserName == null ? (object)DBNull.Value : item.UserName);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity> items)
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
						query += " INSERT INTO [Capital_requests_header] ([Artikelnummer],[CloseDate],[Date],[Fertigungsnummer],[Plant],[PlantId],[Status],[StatusId],[UserId],[UserName]) VALUES ( "

							+ "@Artikelnummer" + i + ","
							+ "@CloseDate" + i + ","
							+ "@Date" + i + ","
							+ "@Fertigungsnummer" + i + ","
							+ "@Plant" + i + ","
							+ "@PlantId" + i + ","
							+ "@Status" + i + ","
							+ "@StatusId" + i + ","
							+ "@UserId" + i + ","
							+ "@UserName" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("CloseDate" + i, item.CloseDate == null ? (object)DBNull.Value : item.CloseDate);
						sqlCommand.Parameters.AddWithValue("Date" + i, item.Date == null ? (object)DBNull.Value : item.Date);
						sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
						sqlCommand.Parameters.AddWithValue("Plant" + i, item.Plant == null ? (object)DBNull.Value : item.Plant);
						sqlCommand.Parameters.AddWithValue("PlantId" + i, item.PlantId == null ? (object)DBNull.Value : item.PlantId);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
						sqlCommand.Parameters.AddWithValue("StatusId" + i, item.StatusId == null ? (object)DBNull.Value : item.StatusId);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
						sqlCommand.Parameters.AddWithValue("UserName" + i, item.UserName == null ? (object)DBNull.Value : item.UserName);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Capital_requests_header] SET [Artikelnummer]=@Artikelnummer, [CloseDate]=@CloseDate, [Date]=@Date, [Fertigungsnummer]=@Fertigungsnummer, [Plant]=@Plant, [PlantId]=@PlantId, [Status]=@Status, [StatusId]=@StatusId, [UserId]=@UserId, [UserName]=@UserName WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("CloseDate", item.CloseDate == null ? (object)DBNull.Value : item.CloseDate);
				sqlCommand.Parameters.AddWithValue("Date", item.Date == null ? (object)DBNull.Value : item.Date);
				sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
				sqlCommand.Parameters.AddWithValue("Plant", item.Plant == null ? (object)DBNull.Value : item.Plant);
				sqlCommand.Parameters.AddWithValue("PlantId", item.PlantId == null ? (object)DBNull.Value : item.PlantId);
				sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
				sqlCommand.Parameters.AddWithValue("StatusId", item.StatusId == null ? (object)DBNull.Value : item.StatusId);
				sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
				sqlCommand.Parameters.AddWithValue("UserName", item.UserName == null ? (object)DBNull.Value : item.UserName);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity> items)
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
						query += " UPDATE [Capital_requests_header] SET "

							+ "[Artikelnummer]=@Artikelnummer" + i + ","
							+ "[CloseDate]=@CloseDate" + i + ","
							+ "[Date]=@Date" + i + ","
							+ "[Fertigungsnummer]=@Fertigungsnummer" + i + ","
							+ "[Plant]=@Plant" + i + ","
							+ "[PlantId]=@PlantId" + i + ","
							+ "[Status]=@Status" + i + ","
							+ "[StatusId]=@StatusId" + i + ","
							+ "[UserId]=@UserId" + i + ","
							+ "[UserName]=@UserName" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("CloseDate" + i, item.CloseDate == null ? (object)DBNull.Value : item.CloseDate);
						sqlCommand.Parameters.AddWithValue("Date" + i, item.Date == null ? (object)DBNull.Value : item.Date);
						sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
						sqlCommand.Parameters.AddWithValue("Plant" + i, item.Plant == null ? (object)DBNull.Value : item.Plant);
						sqlCommand.Parameters.AddWithValue("PlantId" + i, item.PlantId == null ? (object)DBNull.Value : item.PlantId);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
						sqlCommand.Parameters.AddWithValue("StatusId" + i, item.StatusId == null ? (object)DBNull.Value : item.StatusId);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
						sqlCommand.Parameters.AddWithValue("UserName" + i, item.UserName == null ? (object)DBNull.Value : item.UserName);
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
				string query = "DELETE FROM [Capital_requests_header] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [Capital_requests_header] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Capital_requests_header] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Capital_requests_header]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Capital_requests_header] WHERE [Id] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [Capital_requests_header] ([Artikelnummer],[CloseDate],[Date],[Fertigungsnummer],[Plant],[PlantId],[Status],[StatusId],[UserId],[UserName]) OUTPUT INSERTED.[Id] VALUES (@Artikelnummer,@CloseDate,@Date,@Fertigungsnummer,@Plant,@PlantId,@Status,@StatusId,@UserId,@UserName); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
			sqlCommand.Parameters.AddWithValue("CloseDate", item.CloseDate == null ? (object)DBNull.Value : item.CloseDate);
			sqlCommand.Parameters.AddWithValue("Date", item.Date == null ? (object)DBNull.Value : item.Date);
			sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
			sqlCommand.Parameters.AddWithValue("Plant", item.Plant == null ? (object)DBNull.Value : item.Plant);
			sqlCommand.Parameters.AddWithValue("PlantId", item.PlantId == null ? (object)DBNull.Value : item.PlantId);
			sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
			sqlCommand.Parameters.AddWithValue("StatusId", item.StatusId == null ? (object)DBNull.Value : item.StatusId);
			sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
			sqlCommand.Parameters.AddWithValue("UserName", item.UserName == null ? (object)DBNull.Value : item.UserName);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Capital_requests_header] ([Artikelnummer],[CloseDate],[Date],[Fertigungsnummer],[Plant],[PlantId],[Status],[StatusId],[UserId],[UserName]) VALUES ( "

						+ "@Artikelnummer" + i + ","
						+ "@CloseDate" + i + ","
						+ "@Date" + i + ","
						+ "@Fertigungsnummer" + i + ","
						+ "@Plant" + i + ","
						+ "@PlantId" + i + ","
						+ "@Status" + i + ","
						+ "@StatusId" + i + ","
						+ "@UserId" + i + ","
						+ "@UserName" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("CloseDate" + i, item.CloseDate == null ? (object)DBNull.Value : item.CloseDate);
					sqlCommand.Parameters.AddWithValue("Date" + i, item.Date == null ? (object)DBNull.Value : item.Date);
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("Plant" + i, item.Plant == null ? (object)DBNull.Value : item.Plant);
					sqlCommand.Parameters.AddWithValue("PlantId" + i, item.PlantId == null ? (object)DBNull.Value : item.PlantId);
					sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("StatusId" + i, item.StatusId == null ? (object)DBNull.Value : item.StatusId);
					sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
					sqlCommand.Parameters.AddWithValue("UserName" + i, item.UserName == null ? (object)DBNull.Value : item.UserName);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Capital_requests_header] SET [Artikelnummer]=@Artikelnummer, [CloseDate]=@CloseDate, [Date]=@Date, [Fertigungsnummer]=@Fertigungsnummer, [Plant]=@Plant, [PlantId]=@PlantId, [Status]=@Status, [StatusId]=@StatusId, [UserId]=@UserId, [UserName]=@UserName WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
			sqlCommand.Parameters.AddWithValue("CloseDate", item.CloseDate == null ? (object)DBNull.Value : item.CloseDate);
			sqlCommand.Parameters.AddWithValue("Date", item.Date == null ? (object)DBNull.Value : item.Date);
			sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
			sqlCommand.Parameters.AddWithValue("Plant", item.Plant == null ? (object)DBNull.Value : item.Plant);
			sqlCommand.Parameters.AddWithValue("PlantId", item.PlantId == null ? (object)DBNull.Value : item.PlantId);
			sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
			sqlCommand.Parameters.AddWithValue("StatusId", item.StatusId == null ? (object)DBNull.Value : item.StatusId);
			sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
			sqlCommand.Parameters.AddWithValue("UserName", item.UserName == null ? (object)DBNull.Value : item.UserName);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_headerEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Capital_requests_header] SET "

					+ "[Artikelnummer]=@Artikelnummer" + i + ","
					+ "[CloseDate]=@CloseDate" + i + ","
					+ "[Date]=@Date" + i + ","
					+ "[Fertigungsnummer]=@Fertigungsnummer" + i + ","
					+ "[Plant]=@Plant" + i + ","
					+ "[PlantId]=@PlantId" + i + ","
					+ "[Status]=@Status" + i + ","
					+ "[StatusId]=@StatusId" + i + ","
					+ "[UserId]=@UserId" + i + ","
					+ "[UserName]=@UserName" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("CloseDate" + i, item.CloseDate == null ? (object)DBNull.Value : item.CloseDate);
					sqlCommand.Parameters.AddWithValue("Date" + i, item.Date == null ? (object)DBNull.Value : item.Date);
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("Plant" + i, item.Plant == null ? (object)DBNull.Value : item.Plant);
					sqlCommand.Parameters.AddWithValue("PlantId" + i, item.PlantId == null ? (object)DBNull.Value : item.PlantId);
					sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("StatusId" + i, item.StatusId == null ? (object)DBNull.Value : item.StatusId);
					sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
					sqlCommand.Parameters.AddWithValue("UserName" + i, item.UserName == null ? (object)DBNull.Value : item.UserName);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Capital_requests_header] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [Capital_requests_header] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<KeyValuePair<string, int>> GetStatsByStatus(int plantId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT [Status] AS [Name], COUNT(*) AS [Value] FROM [capital_requests_header]";
				if(plantId != -1)
					query += $" WHERE [PlantId]={plantId}";
				query += " GROUP BY [Status]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x =>
				new KeyValuePair<string, int>(
					   Convert.ToString(x["Name"]),
						Convert.ToInt32(x["Value"])
						)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<KeyValuePair<string, int>> GetStatsByPlant(int plantId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT [Plant] AS [Name], COUNT(*) AS [Value] FROM [capital_requests_header]";
				if(plantId != -1)
					query += $" WHERE [PlantId]={plantId}";
				query += " GROUP BY [Plant]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x =>
				new KeyValuePair<string, int>(
					   Convert.ToString(x["Name"]),
						Convert.ToInt32(x["Value"])
						)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<KeyValuePair<string, int>> GetStatsByCategory(int plantId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT [IncidentCategory] AS [Name],COUNT(*) AS [Value] FROM [capital_requests_positions]";
				if(plantId != -1)
					query += $" P INNER JOIN [capital_requests_header] H ON P.HeaderId=H.Id WHERE H.PlantId={plantId}";
				query += " GROUP BY [IncidentCategory]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x =>
				new KeyValuePair<string, int>(
					   Convert.ToString(x["Name"]),
						Convert.ToInt32(x["Value"])
						)).ToList();
			}
			else
			{
				return null;
			}
		}
		#endregion Custom Methods
	}
}