using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class Tbl_E_Rechnung_KundendefinitionenAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Tbl_E-Rechnung_Kundendefinitionen] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Tbl_E-Rechnung_Kundendefinitionen]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Tbl_E-Rechnung_Kundendefinitionen] WHERE [ID] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Tbl_E-Rechnung_Kundendefinitionen] ([Betreff],[Email],[EmailVermerk],[Kundenname],[Kundennummer],[Rechnung_Name],[Typ],[Versand]) OUTPUT INSERTED.[ID] VALUES (@Betreff,@Email,@EmailVermerk,@Kundenname,@Kundennummer,@Rechnung_Name,@Typ,@Versand); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Betreff", item.Betreff == null ? (object)DBNull.Value : item.Betreff);
					sqlCommand.Parameters.AddWithValue("Email", item.Email == null ? (object)DBNull.Value : item.Email);
					sqlCommand.Parameters.AddWithValue("EmailVermerk", item.EmailVermerk == null ? (object)DBNull.Value : item.EmailVermerk);
					sqlCommand.Parameters.AddWithValue("Kundenname", item.Kundenname == null ? (object)DBNull.Value : item.Kundenname);
					sqlCommand.Parameters.AddWithValue("Kundennummer", item.Kundennummer == null ? (object)DBNull.Value : item.Kundennummer);
					sqlCommand.Parameters.AddWithValue("Rechnung_Name", item.Rechnung_Name == null ? (object)DBNull.Value : item.Rechnung_Name);
					sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
					sqlCommand.Parameters.AddWithValue("Versand", item.Versand == null ? (object)DBNull.Value : item.Versand);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity> items)
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
						query += " INSERT INTO [Tbl_E-Rechnung_Kundendefinitionen] ([Betreff],[Email],[EmailVermerk],[Kundenname],[Kundennummer],[Rechnung_Name],[Typ],[Versand]) VALUES ( "

							+ "@Betreff" + i + ","
							+ "@Email" + i + ","
							+ "@EmailVermerk" + i + ","
							+ "@Kundenname" + i + ","
							+ "@Kundennummer" + i + ","
							+ "@Rechnung_Name" + i + ","
							+ "@Typ" + i + ","
							+ "@Versand" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Betreff" + i, item.Betreff == null ? (object)DBNull.Value : item.Betreff);
						sqlCommand.Parameters.AddWithValue("Email" + i, item.Email == null ? (object)DBNull.Value : item.Email);
						sqlCommand.Parameters.AddWithValue("EmailVermerk" + i, item.EmailVermerk == null ? (object)DBNull.Value : item.EmailVermerk);
						sqlCommand.Parameters.AddWithValue("Kundenname" + i, item.Kundenname == null ? (object)DBNull.Value : item.Kundenname);
						sqlCommand.Parameters.AddWithValue("Kundennummer" + i, item.Kundennummer == null ? (object)DBNull.Value : item.Kundennummer);
						sqlCommand.Parameters.AddWithValue("Rechnung_Name" + i, item.Rechnung_Name == null ? (object)DBNull.Value : item.Rechnung_Name);
						sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
						sqlCommand.Parameters.AddWithValue("Versand" + i, item.Versand == null ? (object)DBNull.Value : item.Versand);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Tbl_E-Rechnung_Kundendefinitionen] SET [Betreff]=@Betreff, [Email]=@Email, [EmailVermerk]=@EmailVermerk, [Kundenname]=@Kundenname, [Rechnung_Name]=@Rechnung_Name, [Typ]=@Typ, [Versand]=@Versand WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("Betreff", item.Betreff == null ? (object)DBNull.Value : item.Betreff);
				sqlCommand.Parameters.AddWithValue("Email", item.Email == null ? (object)DBNull.Value : item.Email);
				sqlCommand.Parameters.AddWithValue("EmailVermerk", item.EmailVermerk == null ? (object)DBNull.Value : item.EmailVermerk);
				sqlCommand.Parameters.AddWithValue("Kundenname", item.Kundenname == null ? (object)DBNull.Value : item.Kundenname);
				//sqlCommand.Parameters.AddWithValue("Kundennummer", item.Kundennummer == null ? (object)DBNull.Value : item.Kundennummer);
				sqlCommand.Parameters.AddWithValue("Rechnung_Name", item.Rechnung_Name == null ? (object)DBNull.Value : item.Rechnung_Name);
				sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
				sqlCommand.Parameters.AddWithValue("Versand", item.Versand == null ? (object)DBNull.Value : item.Versand);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity> items)
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
						query += " UPDATE [Tbl_E-Rechnung_Kundendefinitionen] SET "

							+ "[Betreff]=@Betreff" + i + ","
							+ "[Email]=@Email" + i + ","
							+ "[EmailVermerk]=@EmailVermerk" + i + ","
							+ "[Kundenname]=@Kundenname" + i + ","
							+ "[Kundennummer]=@Kundennummer" + i + ","
							+ "[Rechnung_Name]=@Rechnung_Name" + i + ","
							+ "[Typ]=@Typ" + i + ","
							+ "[Versand]=@Versand" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("Betreff" + i, item.Betreff == null ? (object)DBNull.Value : item.Betreff);
						sqlCommand.Parameters.AddWithValue("Email" + i, item.Email == null ? (object)DBNull.Value : item.Email);
						sqlCommand.Parameters.AddWithValue("EmailVermerk" + i, item.EmailVermerk == null ? (object)DBNull.Value : item.EmailVermerk);
						sqlCommand.Parameters.AddWithValue("Kundenname" + i, item.Kundenname == null ? (object)DBNull.Value : item.Kundenname);
						sqlCommand.Parameters.AddWithValue("Kundennummer" + i, item.Kundennummer == null ? (object)DBNull.Value : item.Kundennummer);
						sqlCommand.Parameters.AddWithValue("Rechnung_Name" + i, item.Rechnung_Name == null ? (object)DBNull.Value : item.Rechnung_Name);
						sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
						sqlCommand.Parameters.AddWithValue("Versand" + i, item.Versand == null ? (object)DBNull.Value : item.Versand);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
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
				string query = "DELETE FROM [Tbl_E-Rechnung_Kundendefinitionen] WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ID", id);

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

					string query = "DELETE FROM [Tbl_E-Rechnung_Kundendefinitionen] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Tbl_E-Rechnung_Kundendefinitionen] WHERE [ID]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Tbl_E-Rechnung_Kundendefinitionen]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Tbl_E-Rechnung_Kundendefinitionen] WHERE [ID] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [Tbl_E-Rechnung_Kundendefinitionen] ([Betreff],[Email],[EmailVermerk],[Kundenname],[Kundennummer],[Rechnung_Name],[Typ],[Versand]) OUTPUT INSERTED.[ID] VALUES (@Betreff,@Email,@EmailVermerk,@Kundenname,@Kundennummer,@Rechnung_Name,@Typ,@Versand); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Betreff", item.Betreff == null ? (object)DBNull.Value : item.Betreff);
			sqlCommand.Parameters.AddWithValue("Email", item.Email == null ? (object)DBNull.Value : item.Email);
			sqlCommand.Parameters.AddWithValue("EmailVermerk", item.EmailVermerk == null ? (object)DBNull.Value : item.EmailVermerk);
			sqlCommand.Parameters.AddWithValue("Kundenname", item.Kundenname == null ? (object)DBNull.Value : item.Kundenname);
			sqlCommand.Parameters.AddWithValue("Kundennummer", item.Kundennummer == null ? (object)DBNull.Value : item.Kundennummer);
			sqlCommand.Parameters.AddWithValue("Rechnung_Name", item.Rechnung_Name == null ? (object)DBNull.Value : item.Rechnung_Name);
			sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
			sqlCommand.Parameters.AddWithValue("Versand", item.Versand == null ? (object)DBNull.Value : item.Versand);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Tbl_E-Rechnung_Kundendefinitionen] ([Betreff],[Email],[EmailVermerk],[Kundenname],[Kundennummer],[Rechnung_Name],[Typ],[Versand]) VALUES ( "

						+ "@Betreff" + i + ","
						+ "@Email" + i + ","
						+ "@EmailVermerk" + i + ","
						+ "@Kundenname" + i + ","
						+ "@Kundennummer" + i + ","
						+ "@Rechnung_Name" + i + ","
						+ "@Typ" + i + ","
						+ "@Versand" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Betreff" + i, item.Betreff == null ? (object)DBNull.Value : item.Betreff);
					sqlCommand.Parameters.AddWithValue("Email" + i, item.Email == null ? (object)DBNull.Value : item.Email);
					sqlCommand.Parameters.AddWithValue("EmailVermerk" + i, item.EmailVermerk == null ? (object)DBNull.Value : item.EmailVermerk);
					sqlCommand.Parameters.AddWithValue("Kundenname" + i, item.Kundenname == null ? (object)DBNull.Value : item.Kundenname);
					sqlCommand.Parameters.AddWithValue("Kundennummer" + i, item.Kundennummer == null ? (object)DBNull.Value : item.Kundennummer);
					sqlCommand.Parameters.AddWithValue("Rechnung_Name" + i, item.Rechnung_Name == null ? (object)DBNull.Value : item.Rechnung_Name);
					sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
					sqlCommand.Parameters.AddWithValue("Versand" + i, item.Versand == null ? (object)DBNull.Value : item.Versand);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Tbl_E-Rechnung_Kundendefinitionen] SET [Betreff]=@Betreff, [Email]=@Email, [EmailVermerk]=@EmailVermerk, [Kundenname]=@Kundenname, [Kundennummer]=@Kundennummer, [Rechnung_Name]=@Rechnung_Name, [Typ]=@Typ, [Versand]=@Versand WHERE [ID]=@ID";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("ID", item.ID);
			sqlCommand.Parameters.AddWithValue("Betreff", item.Betreff == null ? (object)DBNull.Value : item.Betreff);
			sqlCommand.Parameters.AddWithValue("Email", item.Email == null ? (object)DBNull.Value : item.Email);
			sqlCommand.Parameters.AddWithValue("EmailVermerk", item.EmailVermerk == null ? (object)DBNull.Value : item.EmailVermerk);
			sqlCommand.Parameters.AddWithValue("Kundenname", item.Kundenname == null ? (object)DBNull.Value : item.Kundenname);
			sqlCommand.Parameters.AddWithValue("Kundennummer", item.Kundennummer == null ? (object)DBNull.Value : item.Kundennummer);
			sqlCommand.Parameters.AddWithValue("Rechnung_Name", item.Rechnung_Name == null ? (object)DBNull.Value : item.Rechnung_Name);
			sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
			sqlCommand.Parameters.AddWithValue("Versand", item.Versand == null ? (object)DBNull.Value : item.Versand);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Tbl_E-Rechnung_Kundendefinitionen] SET "

					+ "[Betreff]=@Betreff" + i + ","
					+ "[Email]=@Email" + i + ","
					+ "[EmailVermerk]=@EmailVermerk" + i + ","
					+ "[Kundenname]=@Kundenname" + i + ","
					+ "[Kundennummer]=@Kundennummer" + i + ","
					+ "[Rechnung_Name]=@Rechnung_Name" + i + ","
					+ "[Typ]=@Typ" + i + ","
					+ "[Versand]=@Versand" + i + " WHERE [ID]=@ID" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
					sqlCommand.Parameters.AddWithValue("Betreff" + i, item.Betreff == null ? (object)DBNull.Value : item.Betreff);
					sqlCommand.Parameters.AddWithValue("Email" + i, item.Email == null ? (object)DBNull.Value : item.Email);
					sqlCommand.Parameters.AddWithValue("EmailVermerk" + i, item.EmailVermerk == null ? (object)DBNull.Value : item.EmailVermerk);
					sqlCommand.Parameters.AddWithValue("Kundenname" + i, item.Kundenname == null ? (object)DBNull.Value : item.Kundenname);
					sqlCommand.Parameters.AddWithValue("Kundennummer" + i, item.Kundennummer == null ? (object)DBNull.Value : item.Kundennummer);
					sqlCommand.Parameters.AddWithValue("Rechnung_Name" + i, item.Rechnung_Name == null ? (object)DBNull.Value : item.Rechnung_Name);
					sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
					sqlCommand.Parameters.AddWithValue("Versand" + i, item.Versand == null ? (object)DBNull.Value : item.Versand);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Tbl_E-Rechnung_Kundendefinitionen] WHERE [ID]=@ID";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ID", id);

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

				string query = "DELETE FROM [Tbl_E-Rechnung_Kundendefinitionen] WHERE [ID] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity GetByKundennummer(int kundennumer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Tbl_E-Rechnung_Kundendefinitionen] WHERE [Kundennummer]=@kundennumer";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("kundennumer", kundennumer);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity GetByKundennummer(int kundennumer, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			string query = "SELECT * FROM [Tbl_E-Rechnung_Kundendefinitionen] WHERE [Kundennummer]=@kundennumer";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("kundennumer", kundennumer);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity GetByCustomerIdName(int kundennumer, string customerName)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Tbl_E-Rechnung_Kundendefinitionen] WHERE [Kundennummer]=@kundennumer AND TRIM(CAST([Kundenname] as varchar(500)))=@customerName";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("kundennumer", kundennumer);
				sqlCommand.Parameters.AddWithValue("customerName", customerName?.Trim());

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		#endregion Custom Methods

	}
}
