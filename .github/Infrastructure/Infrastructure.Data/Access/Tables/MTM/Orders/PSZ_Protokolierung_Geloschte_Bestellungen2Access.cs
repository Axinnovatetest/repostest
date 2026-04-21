using Infrastructure.Data.Entities.Tables.MTM.Orders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.MTM.Orders
{
	public class PSZ_Protokolierung_Geloschte_Bestellungen2Access
	{
		#region Default Methods
		public static PSZ_Protokolierung_Geloschte_Bestellungen2Entity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_Protokolierung Gelöschte Bestellungen2] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new PSZ_Protokolierung_Geloschte_Bestellungen2Entity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<PSZ_Protokolierung_Geloschte_Bestellungen2Entity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_Protokolierung Gelöschte Bestellungen2]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new PSZ_Protokolierung_Geloschte_Bestellungen2Entity(x)).ToList();
			}
			else
			{
				return new List<PSZ_Protokolierung_Geloschte_Bestellungen2Entity>();
			}
		}
		public static List<PSZ_Protokolierung_Geloschte_Bestellungen2Entity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<PSZ_Protokolierung_Geloschte_Bestellungen2Entity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<PSZ_Protokolierung_Geloschte_Bestellungen2Entity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<PSZ_Protokolierung_Geloschte_Bestellungen2Entity>();
		}
		private static List<PSZ_Protokolierung_Geloschte_Bestellungen2Entity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [PSZ_Protokolierung Gelöschte Bestellungen2] WHERE [ID] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new PSZ_Protokolierung_Geloschte_Bestellungen2Entity(x)).ToList();
				}
				else
				{
					return new List<PSZ_Protokolierung_Geloschte_Bestellungen2Entity>();
				}
			}
			return new List<PSZ_Protokolierung_Geloschte_Bestellungen2Entity>();
		}

		public static int Insert(PSZ_Protokolierung_Geloschte_Bestellungen2Entity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [PSZ_Protokolierung Gelöschte Bestellungen2] ([Bestellung_Typ],[Bestellung-Nr],[Gelöscht AM],[Gelöscht durch],[Lieferanten-nr],[Name],[Projekt-Nr]) OUTPUT INSERTED.[ID] VALUES (@Bestellung_Typ,@Bestellung_Nr,@Geloscht_AM,@Geloscht_durch,@Lieferanten_nr,@Name,@Projekt_Nr); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Bestellung_Typ", item.Bestellung_Typ == null ? DBNull.Value : item.Bestellung_Typ);
					sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? DBNull.Value : item.Bestellung_Nr);
					sqlCommand.Parameters.AddWithValue("Geloscht_AM", item.Geloscht_AM == null ? DBNull.Value : item.Geloscht_AM);
					sqlCommand.Parameters.AddWithValue("Geloscht_durch", item.Geloscht_durch == null ? DBNull.Value : item.Geloscht_durch);
					sqlCommand.Parameters.AddWithValue("Lieferanten_nr", item.Lieferanten_nr == null ? DBNull.Value : item.Lieferanten_nr);
					sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? DBNull.Value : item.Name);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? DBNull.Value : item.Projekt_Nr);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<PSZ_Protokolierung_Geloschte_Bestellungen2Entity> items)
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
		private static int insert(List<PSZ_Protokolierung_Geloschte_Bestellungen2Entity> items)
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
						query += " INSERT INTO [PSZ_Protokolierung Gelöschte Bestellungen2] ([Bestellung_Typ],[Bestellung-Nr],[Gelöscht AM],[Gelöscht durch],[Lieferanten-nr],[Name],[Projekt-Nr]) VALUES ( "

							+ "@Bestellung_Typ" + i + ","
							+ "@Bestellung_Nr" + i + ","
							+ "@Geloscht_AM" + i + ","
							+ "@Geloscht_durch" + i + ","
							+ "@Lieferanten_nr" + i + ","
							+ "@Name" + i + ","
							+ "@Projekt_Nr" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Bestellung_Typ" + i, item.Bestellung_Typ == null ? DBNull.Value : item.Bestellung_Typ);
						sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? DBNull.Value : item.Bestellung_Nr);
						sqlCommand.Parameters.AddWithValue("Geloscht_AM" + i, item.Geloscht_AM == null ? DBNull.Value : item.Geloscht_AM);
						sqlCommand.Parameters.AddWithValue("Geloscht_durch" + i, item.Geloscht_durch == null ? DBNull.Value : item.Geloscht_durch);
						sqlCommand.Parameters.AddWithValue("Lieferanten_nr" + i, item.Lieferanten_nr == null ? DBNull.Value : item.Lieferanten_nr);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name == null ? DBNull.Value : item.Name);
						sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? DBNull.Value : item.Projekt_Nr);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(PSZ_Protokolierung_Geloschte_Bestellungen2Entity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [PSZ_Protokolierung Gelöschte Bestellungen2] SET [Bestellung_Typ]=@Bestellung_Typ, [Bestellung-Nr]=@Bestellung_Nr, [Gelöscht AM]=@Geloscht_AM, [Gelöscht durch]=@Geloscht_durch, [Lieferanten-nr]=@Lieferanten_nr, [Name]=@Name, [Projekt-Nr]=@Projekt_Nr WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("Bestellung_Typ", item.Bestellung_Typ == null ? DBNull.Value : item.Bestellung_Typ);
				sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? DBNull.Value : item.Bestellung_Nr);
				sqlCommand.Parameters.AddWithValue("Geloscht_AM", item.Geloscht_AM == null ? DBNull.Value : item.Geloscht_AM);
				sqlCommand.Parameters.AddWithValue("Geloscht_durch", item.Geloscht_durch == null ? DBNull.Value : item.Geloscht_durch);
				sqlCommand.Parameters.AddWithValue("Lieferanten_nr", item.Lieferanten_nr == null ? DBNull.Value : item.Lieferanten_nr);
				sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? DBNull.Value : item.Name);
				sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? DBNull.Value : item.Projekt_Nr);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<PSZ_Protokolierung_Geloschte_Bestellungen2Entity> items)
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
		private static int update(List<PSZ_Protokolierung_Geloschte_Bestellungen2Entity> items)
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
						query += " UPDATE [PSZ_Protokolierung Gelöschte Bestellungen2] SET "

							+ "[Bestellung_Typ]=@Bestellung_Typ" + i + ","
							+ "[Bestellung-Nr]=@Bestellung_Nr" + i + ","
							+ "[Gelöscht AM]=@Geloscht_AM" + i + ","
							+ "[Gelöscht durch]=@Geloscht_durch" + i + ","
							+ "[Lieferanten-nr]=@Lieferanten_nr" + i + ","
							+ "[Name]=@Name" + i + ","
							+ "[Projekt-Nr]=@Projekt_Nr" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("Bestellung_Typ" + i, item.Bestellung_Typ == null ? DBNull.Value : item.Bestellung_Typ);
						sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? DBNull.Value : item.Bestellung_Nr);
						sqlCommand.Parameters.AddWithValue("Geloscht_AM" + i, item.Geloscht_AM == null ? DBNull.Value : item.Geloscht_AM);
						sqlCommand.Parameters.AddWithValue("Geloscht_durch" + i, item.Geloscht_durch == null ? DBNull.Value : item.Geloscht_durch);
						sqlCommand.Parameters.AddWithValue("Lieferanten_nr" + i, item.Lieferanten_nr == null ? DBNull.Value : item.Lieferanten_nr);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name == null ? DBNull.Value : item.Name);
						sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? DBNull.Value : item.Projekt_Nr);
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
				string query = "DELETE FROM [PSZ_Protokolierung Gelöschte Bestellungen2] WHERE [ID]=@ID";
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

					string query = "DELETE FROM [PSZ_Protokolierung Gelöschte Bestellungen2] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static PSZ_Protokolierung_Geloschte_Bestellungen2Entity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [PSZ_Protokolierung Gelöschte Bestellungen2] WHERE [ID]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new PSZ_Protokolierung_Geloschte_Bestellungen2Entity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<PSZ_Protokolierung_Geloschte_Bestellungen2Entity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [PSZ_Protokolierung Gelöschte Bestellungen2]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new PSZ_Protokolierung_Geloschte_Bestellungen2Entity(x)).ToList();
			}
			else
			{
				return new List<PSZ_Protokolierung_Geloschte_Bestellungen2Entity>();
			}
		}
		public static List<PSZ_Protokolierung_Geloschte_Bestellungen2Entity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<PSZ_Protokolierung_Geloschte_Bestellungen2Entity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<PSZ_Protokolierung_Geloschte_Bestellungen2Entity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<PSZ_Protokolierung_Geloschte_Bestellungen2Entity>();
		}
		private static List<PSZ_Protokolierung_Geloschte_Bestellungen2Entity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [PSZ_Protokolierung Gelöschte Bestellungen2] WHERE [ID] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new PSZ_Protokolierung_Geloschte_Bestellungen2Entity(x)).ToList();
				}
				else
				{
					return new List<PSZ_Protokolierung_Geloschte_Bestellungen2Entity>();
				}
			}
			return new List<PSZ_Protokolierung_Geloschte_Bestellungen2Entity>();
		}

		public static int InsertWithTransaction(PSZ_Protokolierung_Geloschte_Bestellungen2Entity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [PSZ_Protokolierung Gelöschte Bestellungen2] ([Bestellung_Typ],[Bestellung-Nr],[Gelöscht AM],[Gelöscht durch],[Lieferanten-nr],[Name],[Projekt-Nr]) OUTPUT INSERTED.[ID] VALUES (@Bestellung_Typ,@Bestellung_Nr,@Geloscht_AM,@Geloscht_durch,@Lieferanten_nr,@Name,@Projekt_Nr); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Bestellung_Typ", item.Bestellung_Typ == null ? DBNull.Value : item.Bestellung_Typ);
			sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? DBNull.Value : item.Bestellung_Nr);
			sqlCommand.Parameters.AddWithValue("Geloscht_AM", item.Geloscht_AM == null ? DBNull.Value : item.Geloscht_AM);
			sqlCommand.Parameters.AddWithValue("Geloscht_durch", item.Geloscht_durch == null ? DBNull.Value : item.Geloscht_durch);
			sqlCommand.Parameters.AddWithValue("Lieferanten_nr", item.Lieferanten_nr == null ? DBNull.Value : item.Lieferanten_nr);
			sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? DBNull.Value : item.Name);
			sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? DBNull.Value : item.Projekt_Nr);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<PSZ_Protokolierung_Geloschte_Bestellungen2Entity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int insertWithTransaction(List<PSZ_Protokolierung_Geloschte_Bestellungen2Entity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [PSZ_Protokolierung Gelöschte Bestellungen2] ([Bestellung_Typ],[Bestellung-Nr],[Gelöscht AM],[Gelöscht durch],[Lieferanten-nr],[Name],[Projekt-Nr]) VALUES ( "

						+ "@Bestellung_Typ" + i + ","
						+ "@Bestellung_Nr" + i + ","
						+ "@Geloscht_AM" + i + ","
						+ "@Geloscht_durch" + i + ","
						+ "@Lieferanten_nr" + i + ","
						+ "@Name" + i + ","
						+ "@Projekt_Nr" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Bestellung_Typ" + i, item.Bestellung_Typ == null ? DBNull.Value : item.Bestellung_Typ);
					sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? DBNull.Value : item.Bestellung_Nr);
					sqlCommand.Parameters.AddWithValue("Geloscht_AM" + i, item.Geloscht_AM == null ? DBNull.Value : item.Geloscht_AM);
					sqlCommand.Parameters.AddWithValue("Geloscht_durch" + i, item.Geloscht_durch == null ? DBNull.Value : item.Geloscht_durch);
					sqlCommand.Parameters.AddWithValue("Lieferanten_nr" + i, item.Lieferanten_nr == null ? DBNull.Value : item.Lieferanten_nr);
					sqlCommand.Parameters.AddWithValue("Name" + i, item.Name == null ? DBNull.Value : item.Name);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? DBNull.Value : item.Projekt_Nr);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(PSZ_Protokolierung_Geloschte_Bestellungen2Entity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [PSZ_Protokolierung Gelöschte Bestellungen2] SET [Bestellung_Typ]=@Bestellung_Typ, [Bestellung-Nr]=@Bestellung_Nr, [Gelöscht AM]=@Geloscht_AM, [Gelöscht durch]=@Geloscht_durch, [Lieferanten-nr]=@Lieferanten_nr, [Name]=@Name, [Projekt-Nr]=@Projekt_Nr WHERE [ID]=@ID";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("ID", item.ID);
			sqlCommand.Parameters.AddWithValue("Bestellung_Typ", item.Bestellung_Typ == null ? DBNull.Value : item.Bestellung_Typ);
			sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? DBNull.Value : item.Bestellung_Nr);
			sqlCommand.Parameters.AddWithValue("Geloscht_AM", item.Geloscht_AM == null ? DBNull.Value : item.Geloscht_AM);
			sqlCommand.Parameters.AddWithValue("Geloscht_durch", item.Geloscht_durch == null ? DBNull.Value : item.Geloscht_durch);
			sqlCommand.Parameters.AddWithValue("Lieferanten_nr", item.Lieferanten_nr == null ? DBNull.Value : item.Lieferanten_nr);
			sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? DBNull.Value : item.Name);
			sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? DBNull.Value : item.Projekt_Nr);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<PSZ_Protokolierung_Geloschte_Bestellungen2Entity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int updateWithTransaction(List<PSZ_Protokolierung_Geloschte_Bestellungen2Entity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [PSZ_Protokolierung Gelöschte Bestellungen2] SET "

					+ "[Bestellung_Typ]=@Bestellung_Typ" + i + ","
					+ "[Bestellung-Nr]=@Bestellung_Nr" + i + ","
					+ "[Gelöscht AM]=@Geloscht_AM" + i + ","
					+ "[Gelöscht durch]=@Geloscht_durch" + i + ","
					+ "[Lieferanten-nr]=@Lieferanten_nr" + i + ","
					+ "[Name]=@Name" + i + ","
					+ "[Projekt-Nr]=@Projekt_Nr" + i + " WHERE [ID]=@ID" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
					sqlCommand.Parameters.AddWithValue("Bestellung_Typ" + i, item.Bestellung_Typ == null ? DBNull.Value : item.Bestellung_Typ);
					sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? DBNull.Value : item.Bestellung_Nr);
					sqlCommand.Parameters.AddWithValue("Geloscht_AM" + i, item.Geloscht_AM == null ? DBNull.Value : item.Geloscht_AM);
					sqlCommand.Parameters.AddWithValue("Geloscht_durch" + i, item.Geloscht_durch == null ? DBNull.Value : item.Geloscht_durch);
					sqlCommand.Parameters.AddWithValue("Lieferanten_nr" + i, item.Lieferanten_nr == null ? DBNull.Value : item.Lieferanten_nr);
					sqlCommand.Parameters.AddWithValue("Name" + i, item.Name == null ? DBNull.Value : item.Name);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? DBNull.Value : item.Projekt_Nr);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [PSZ_Protokolierung Gelöschte Bestellungen2] WHERE [ID]=@ID";
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

				string query = "DELETE FROM [PSZ_Protokolierung Gelöschte Bestellungen2] WHERE [ID] IN (" + queryIds + ")";
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
