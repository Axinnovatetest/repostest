using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class PSZ_Protokollierung_Angebote2Access
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.PSZ_Protokollierung_Angebote2Entity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_Protokollierung_Angebote2] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.PSZ_Protokollierung_Angebote2Entity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Protokollierung_Angebote2Entity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_Protokollierung_Angebote2]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Protokollierung_Angebote2Entity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Protokollierung_Angebote2Entity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Protokollierung_Angebote2Entity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Protokollierung_Angebote2Entity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Protokollierung_Angebote2Entity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Protokollierung_Angebote2Entity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Protokollierung_Angebote2Entity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [PSZ_Protokollierung_Angebote2] WHERE [ID] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Protokollierung_Angebote2Entity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Protokollierung_Angebote2Entity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Protokollierung_Angebote2Entity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.PSZ_Protokollierung_Angebote2Entity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [PSZ_Protokollierung_Angebote2] ([Angebot-Nr],[bestellung_Typ],[Bezug],[Gelöscht-am],[Gelöscht-durch],[Kunden-Nr],[Name],[Projekt-Nr])  VALUES (@Angebot_Nr,@bestellung_Typ,@Bezug,@Gelöscht_am,@Gelöscht_durch,@Kunden_Nr,@Name,@Projekt_Nr); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Angebot_Nr", item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
					sqlCommand.Parameters.AddWithValue("bestellung_Typ", item.bestellung_Typ == null ? (object)DBNull.Value : item.bestellung_Typ);
					sqlCommand.Parameters.AddWithValue("Bezug", item.Bezug == null ? (object)DBNull.Value : item.Bezug);
					sqlCommand.Parameters.AddWithValue("Gelöscht_am", item.Gelöscht_am == null ? (object)DBNull.Value : item.Gelöscht_am);
					sqlCommand.Parameters.AddWithValue("Gelöscht_durch", item.Gelöscht_durch == null ? (object)DBNull.Value : item.Gelöscht_durch);
					sqlCommand.Parameters.AddWithValue("Kunden_Nr", item.Kunden_Nr == null ? (object)DBNull.Value : item.Kunden_Nr);
					sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? (object)DBNull.Value : item.Name);
					sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Protokollierung_Angebote2Entity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Protokollierung_Angebote2Entity> items)
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
						query += " INSERT INTO [PSZ_Protokollierung_Angebote2] ([Angebot-Nr],[bestellung_Typ],[Bezug],[Gelöscht-am],[Gelöscht-durch],[Kunden-Nr],[Name],[Projekt-Nr]) VALUES ( "

							+ "@Angebot_Nr" + i + ","
							+ "@bestellung_Typ" + i + ","
							+ "@Bezug" + i + ","
							+ "@Gelöscht_am" + i + ","
							+ "@Gelöscht_durch" + i + ","
							+ "@Kunden_Nr" + i + ","
							+ "@Name" + i + ","
							+ "@Projekt_Nr" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Angebot_Nr" + i, item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
						sqlCommand.Parameters.AddWithValue("bestellung_Typ" + i, item.bestellung_Typ == null ? (object)DBNull.Value : item.bestellung_Typ);
						sqlCommand.Parameters.AddWithValue("Bezug" + i, item.Bezug == null ? (object)DBNull.Value : item.Bezug);
						sqlCommand.Parameters.AddWithValue("Gelöscht_am" + i, item.Gelöscht_am == null ? (object)DBNull.Value : item.Gelöscht_am);
						sqlCommand.Parameters.AddWithValue("Gelöscht_durch" + i, item.Gelöscht_durch == null ? (object)DBNull.Value : item.Gelöscht_durch);
						sqlCommand.Parameters.AddWithValue("Kunden_Nr" + i, item.Kunden_Nr == null ? (object)DBNull.Value : item.Kunden_Nr);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name == null ? (object)DBNull.Value : item.Name);
						sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.PSZ_Protokollierung_Angebote2Entity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [PSZ_Protokollierung_Angebote2] SET [Angebot-Nr]=@Angebot_Nr, [bestellung_Typ]=@bestellung_Typ, [Bezug]=@Bezug, [Gelöscht-am]=@Gelöscht_am, [Gelöscht-durch]=@Gelöscht_durch, [Kunden-Nr]=@Kunden_Nr, [Name]=@Name, [Projekt-Nr]=@Projekt_Nr WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("Angebot_Nr", item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
				sqlCommand.Parameters.AddWithValue("bestellung_Typ", item.bestellung_Typ == null ? (object)DBNull.Value : item.bestellung_Typ);
				sqlCommand.Parameters.AddWithValue("Bezug", item.Bezug == null ? (object)DBNull.Value : item.Bezug);
				sqlCommand.Parameters.AddWithValue("Gelöscht_am", item.Gelöscht_am == null ? (object)DBNull.Value : item.Gelöscht_am);
				sqlCommand.Parameters.AddWithValue("Gelöscht_durch", item.Gelöscht_durch == null ? (object)DBNull.Value : item.Gelöscht_durch);
				sqlCommand.Parameters.AddWithValue("Kunden_Nr", item.Kunden_Nr == null ? (object)DBNull.Value : item.Kunden_Nr);
				sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? (object)DBNull.Value : item.Name);
				sqlCommand.Parameters.AddWithValue("Projekt_Nr", item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Protokollierung_Angebote2Entity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Protokollierung_Angebote2Entity> items)
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
						query += " UPDATE [PSZ_Protokollierung_Angebote2] SET "

							+ "[Angebot-Nr]=@Angebot_Nr" + i + ","
							+ "[bestellung_Typ]=@bestellung_Typ" + i + ","
							+ "[Bezug]=@Bezug" + i + ","
							+ "[Gelöscht-am]=@Gelöscht_am" + i + ","
							+ "[Gelöscht-durch]=@Gelöscht_durch" + i + ","
							+ "[Kunden-Nr]=@Kunden_Nr" + i + ","
							+ "[Name]=@Name" + i + ","
							+ "[Projekt-Nr]=@Projekt_Nr" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("Angebot_Nr" + i, item.Angebot_Nr == null ? (object)DBNull.Value : item.Angebot_Nr);
						sqlCommand.Parameters.AddWithValue("bestellung_Typ" + i, item.bestellung_Typ == null ? (object)DBNull.Value : item.bestellung_Typ);
						sqlCommand.Parameters.AddWithValue("Bezug" + i, item.Bezug == null ? (object)DBNull.Value : item.Bezug);
						sqlCommand.Parameters.AddWithValue("Gelöscht_am" + i, item.Gelöscht_am == null ? (object)DBNull.Value : item.Gelöscht_am);
						sqlCommand.Parameters.AddWithValue("Gelöscht_durch" + i, item.Gelöscht_durch == null ? (object)DBNull.Value : item.Gelöscht_durch);
						sqlCommand.Parameters.AddWithValue("Kunden_Nr" + i, item.Kunden_Nr == null ? (object)DBNull.Value : item.Kunden_Nr);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name == null ? (object)DBNull.Value : item.Name);
						sqlCommand.Parameters.AddWithValue("Projekt_Nr" + i, item.Projekt_Nr == null ? (object)DBNull.Value : item.Projekt_Nr);
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
				string query = "DELETE FROM [PSZ_Protokollierung_Angebote2] WHERE [ID]=@ID";
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

					string query = "DELETE FROM [PSZ_Protokollierung_Angebote2] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods



		#endregion
	}
}
