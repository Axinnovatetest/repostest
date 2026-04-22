using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.FNC
{
	public class KonditionszuordnungstabelleAccess2
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2 Get(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Konditionszuordnungstabelle] WHERE [Nr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", nr);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Konditionszuordnungstabelle]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Konditionszuordnungstabelle] WHERE [Nr] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2 item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Konditionszuordnungstabelle] ([Bemerkung],[Nettotage],[Skonto],[Skontotage],[Text]) OUTPUT INSERTED.[Nr] VALUES (@Bemerkung,@Nettotage,@Skonto,@Skontotage,@Text); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("Nettotage", item.Nettotage == null ? (object)DBNull.Value : item.Nettotage);
					sqlCommand.Parameters.AddWithValue("Skonto", item.Skonto == null ? (object)DBNull.Value : item.Skonto);
					sqlCommand.Parameters.AddWithValue("Skontotage", item.Skontotage == null ? (object)DBNull.Value : item.Skontotage);
					sqlCommand.Parameters.AddWithValue("Text", item.Text == null ? (object)DBNull.Value : item.Text);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2> items)
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
						query += " INSERT INTO [Konditionszuordnungstabelle] ([Bemerkung],[Nettotage],[Skonto],[Skontotage],[Text]) VALUES ( "

							+ "@Bemerkung" + i + ","
							+ "@Nettotage" + i + ","
							+ "@Skonto" + i + ","
							+ "@Skontotage" + i + ","
							+ "@Text" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
						sqlCommand.Parameters.AddWithValue("Nettotage" + i, item.Nettotage == null ? (object)DBNull.Value : item.Nettotage);
						sqlCommand.Parameters.AddWithValue("Skonto" + i, item.Skonto == null ? (object)DBNull.Value : item.Skonto);
						sqlCommand.Parameters.AddWithValue("Skontotage" + i, item.Skontotage == null ? (object)DBNull.Value : item.Skontotage);
						sqlCommand.Parameters.AddWithValue("Text" + i, item.Text == null ? (object)DBNull.Value : item.Text);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2 item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Konditionszuordnungstabelle] SET [Bemerkung]=@Bemerkung, [Nettotage]=@Nettotage, [Skonto]=@Skonto, [Skontotage]=@Skontotage, [Text]=@Text WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
				sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
				sqlCommand.Parameters.AddWithValue("Nettotage", item.Nettotage == null ? (object)DBNull.Value : item.Nettotage);
				sqlCommand.Parameters.AddWithValue("Skonto", item.Skonto == null ? (object)DBNull.Value : item.Skonto);
				sqlCommand.Parameters.AddWithValue("Skontotage", item.Skontotage == null ? (object)DBNull.Value : item.Skontotage);
				sqlCommand.Parameters.AddWithValue("Text", item.Text == null ? (object)DBNull.Value : item.Text);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2> items)
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
						query += " UPDATE [Konditionszuordnungstabelle] SET "

							+ "[Bemerkung]=@Bemerkung" + i + ","
							+ "[Nettotage]=@Nettotage" + i + ","
							+ "[Skonto]=@Skonto" + i + ","
							+ "[Skontotage]=@Skontotage" + i + ","
							+ "[Text]=@Text" + i + " WHERE [Nr]=@Nr" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
						sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
						sqlCommand.Parameters.AddWithValue("Nettotage" + i, item.Nettotage == null ? (object)DBNull.Value : item.Nettotage);
						sqlCommand.Parameters.AddWithValue("Skonto" + i, item.Skonto == null ? (object)DBNull.Value : item.Skonto);
						sqlCommand.Parameters.AddWithValue("Skontotage" + i, item.Skontotage == null ? (object)DBNull.Value : item.Skontotage);
						sqlCommand.Parameters.AddWithValue("Text" + i, item.Text == null ? (object)DBNull.Value : item.Text);
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
				string query = "DELETE FROM [Konditionszuordnungstabelle] WHERE [Nr]=@Nr";
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

					string query = "DELETE FROM [Konditionszuordnungstabelle] WHERE [Nr] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2 GetWithTransaction(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Konditionszuordnungstabelle] WHERE [Nr]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", nr);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Konditionszuordnungstabelle]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Konditionszuordnungstabelle] WHERE [Nr] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2 item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [Konditionszuordnungstabelle] ([Bemerkung],[Nettotage],[Skonto],[Skontotage],[Text]) OUTPUT INSERTED.[Nr] VALUES (@Bemerkung,@Nettotage,@Skonto,@Skontotage,@Text); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
			sqlCommand.Parameters.AddWithValue("Nettotage", item.Nettotage == null ? (object)DBNull.Value : item.Nettotage);
			sqlCommand.Parameters.AddWithValue("Skonto", item.Skonto == null ? (object)DBNull.Value : item.Skonto);
			sqlCommand.Parameters.AddWithValue("Skontotage", item.Skontotage == null ? (object)DBNull.Value : item.Skontotage);
			sqlCommand.Parameters.AddWithValue("Text", item.Text == null ? (object)DBNull.Value : item.Text);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Konditionszuordnungstabelle] ([Bemerkung],[Nettotage],[Skonto],[Skontotage],[Text]) VALUES ( "

						+ "@Bemerkung" + i + ","
						+ "@Nettotage" + i + ","
						+ "@Skonto" + i + ","
						+ "@Skontotage" + i + ","
						+ "@Text" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("Nettotage" + i, item.Nettotage == null ? (object)DBNull.Value : item.Nettotage);
					sqlCommand.Parameters.AddWithValue("Skonto" + i, item.Skonto == null ? (object)DBNull.Value : item.Skonto);
					sqlCommand.Parameters.AddWithValue("Skontotage" + i, item.Skontotage == null ? (object)DBNull.Value : item.Skontotage);
					sqlCommand.Parameters.AddWithValue("Text" + i, item.Text == null ? (object)DBNull.Value : item.Text);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2 item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Konditionszuordnungstabelle] SET [Bemerkung]=@Bemerkung, [Nettotage]=@Nettotage, [Skonto]=@Skonto, [Skontotage]=@Skontotage, [Text]=@Text WHERE [Nr]=@Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
			sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
			sqlCommand.Parameters.AddWithValue("Nettotage", item.Nettotage == null ? (object)DBNull.Value : item.Nettotage);
			sqlCommand.Parameters.AddWithValue("Skonto", item.Skonto == null ? (object)DBNull.Value : item.Skonto);
			sqlCommand.Parameters.AddWithValue("Skontotage", item.Skontotage == null ? (object)DBNull.Value : item.Skontotage);
			sqlCommand.Parameters.AddWithValue("Text", item.Text == null ? (object)DBNull.Value : item.Text);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.KonditionszuordnungstabelleEntity2> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Konditionszuordnungstabelle] SET "

					+ "[Bemerkung]=@Bemerkung" + i + ","
					+ "[Nettotage]=@Nettotage" + i + ","
					+ "[Skonto]=@Skonto" + i + ","
					+ "[Skontotage]=@Skontotage" + i + ","
					+ "[Text]=@Text" + i + " WHERE [Nr]=@Nr" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
					sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("Nettotage" + i, item.Nettotage == null ? (object)DBNull.Value : item.Nettotage);
					sqlCommand.Parameters.AddWithValue("Skonto" + i, item.Skonto == null ? (object)DBNull.Value : item.Skonto);
					sqlCommand.Parameters.AddWithValue("Skontotage" + i, item.Skontotage == null ? (object)DBNull.Value : item.Skontotage);
					sqlCommand.Parameters.AddWithValue("Text" + i, item.Text == null ? (object)DBNull.Value : item.Text);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Konditionszuordnungstabelle] WHERE [Nr]=@Nr";
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

				string query = "DELETE FROM [Konditionszuordnungstabelle] WHERE [Nr] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods

		#endregion Custom Methods

	}
}
