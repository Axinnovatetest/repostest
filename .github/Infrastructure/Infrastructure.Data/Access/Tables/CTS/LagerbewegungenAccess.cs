using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class LagerbewegungenAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.LagerbewegungenEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Lagerbewegungen] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.LagerbewegungenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.LagerbewegungenEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Lagerbewegungen]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.LagerbewegungenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.LagerbewegungenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.LagerbewegungenEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.LagerbewegungenEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.LagerbewegungenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.LagerbewegungenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.LagerbewegungenEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Lagerbewegungen] WHERE [ID] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.LagerbewegungenEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.LagerbewegungenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.LagerbewegungenEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.LagerbewegungenEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Lagerbewegungen] ([angebot-nr],[Datum],[gebucht],[Gebucht von],[Kunden_nr],[Löschen],[Ort],[Typ])  VALUES (@angebot_nr,@Datum,@gebucht,@Gebucht_von,@Kunden_nr,@Löschen,@Ort,@Typ); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("angebot_nr", item.angebot_nr == null ? (object)DBNull.Value : item.angebot_nr);
					sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("gebucht", item.gebucht == null ? (object)DBNull.Value : item.gebucht);
					sqlCommand.Parameters.AddWithValue("Gebucht_von", item.Gebucht_von == null ? (object)DBNull.Value : item.Gebucht_von);
					sqlCommand.Parameters.AddWithValue("Kunden_nr", item.Kunden_nr == null ? (object)DBNull.Value : item.Kunden_nr);
					sqlCommand.Parameters.AddWithValue("Löschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
					sqlCommand.Parameters.AddWithValue("Ort", item.Ort == null ? (object)DBNull.Value : item.Ort);
					sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.LagerbewegungenEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.LagerbewegungenEntity> items)
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
						query += " INSERT INTO [Lagerbewegungen] ([angebot-nr],[Datum],[gebucht],[Gebucht von],[Kunden_nr],[Löschen],[Ort],[Typ]) VALUES ( "

							+ "@angebot_nr" + i + ","
							+ "@Datum" + i + ","
							+ "@gebucht" + i + ","
							+ "@Gebucht_von" + i + ","
							+ "@Kunden_nr" + i + ","
							+ "@Löschen" + i + ","
							+ "@Ort" + i + ","
							+ "@Typ" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("angebot_nr" + i, item.angebot_nr == null ? (object)DBNull.Value : item.angebot_nr);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("gebucht" + i, item.gebucht == null ? (object)DBNull.Value : item.gebucht);
						sqlCommand.Parameters.AddWithValue("Gebucht_von" + i, item.Gebucht_von == null ? (object)DBNull.Value : item.Gebucht_von);
						sqlCommand.Parameters.AddWithValue("Kunden_nr" + i, item.Kunden_nr == null ? (object)DBNull.Value : item.Kunden_nr);
						sqlCommand.Parameters.AddWithValue("Löschen" + i, item.Löschen == null ? (object)DBNull.Value : item.Löschen);
						sqlCommand.Parameters.AddWithValue("Ort" + i, item.Ort == null ? (object)DBNull.Value : item.Ort);
						sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.LagerbewegungenEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Lagerbewegungen] SET [angebot-nr]=@angebot_nr, [Datum]=@Datum, [gebucht]=@gebucht, [Gebucht von]=@Gebucht_von, [Kunden_nr]=@Kunden_nr, [Löschen]=@Löschen, [Ort]=@Ort, [Typ]=@Typ WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("angebot_nr", item.angebot_nr == null ? (object)DBNull.Value : item.angebot_nr);
				sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
				sqlCommand.Parameters.AddWithValue("gebucht", item.gebucht == null ? (object)DBNull.Value : item.gebucht);
				sqlCommand.Parameters.AddWithValue("Gebucht_von", item.Gebucht_von == null ? (object)DBNull.Value : item.Gebucht_von);
				sqlCommand.Parameters.AddWithValue("Kunden_nr", item.Kunden_nr == null ? (object)DBNull.Value : item.Kunden_nr);
				sqlCommand.Parameters.AddWithValue("Löschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
				sqlCommand.Parameters.AddWithValue("Ort", item.Ort == null ? (object)DBNull.Value : item.Ort);
				sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.LagerbewegungenEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.LagerbewegungenEntity> items)
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
						query += " UPDATE [Lagerbewegungen] SET "

							+ "[angebot-nr]=@angebot_nr" + i + ","
							+ "[Datum]=@Datum" + i + ","
							+ "[gebucht]=@gebucht" + i + ","
							+ "[Gebucht von]=@Gebucht_von" + i + ","
							+ "[Kunden_nr]=@Kunden_nr" + i + ","
							+ "[Löschen]=@Löschen" + i + ","
							+ "[Ort]=@Ort" + i + ","
							+ "[Typ]=@Typ" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("angebot_nr" + i, item.angebot_nr == null ? (object)DBNull.Value : item.angebot_nr);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("gebucht" + i, item.gebucht == null ? (object)DBNull.Value : item.gebucht);
						sqlCommand.Parameters.AddWithValue("Gebucht_von" + i, item.Gebucht_von == null ? (object)DBNull.Value : item.Gebucht_von);
						sqlCommand.Parameters.AddWithValue("Kunden_nr" + i, item.Kunden_nr == null ? (object)DBNull.Value : item.Kunden_nr);
						sqlCommand.Parameters.AddWithValue("Löschen" + i, item.Löschen == null ? (object)DBNull.Value : item.Löschen);
						sqlCommand.Parameters.AddWithValue("Ort" + i, item.Ort == null ? (object)DBNull.Value : item.Ort);
						sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
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
				string query = "DELETE FROM [Lagerbewegungen] WHERE [ID]=@ID";
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

					string query = "DELETE FROM [Lagerbewegungen] WHERE [ID] IN (" + queryIds + ")";
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
		#region Querys with transaction
		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CTS.LagerbewegungenEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;
			//using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
			//{
			//sqlConnection.Open();
			//var sqlTransaction = sqlConnection.BeginTransaction();

			string query = "INSERT INTO [Lagerbewegungen] ([angebot-nr],[Datum],[gebucht],[Gebucht von],[Kunden_nr],[Löschen],[Ort],[Typ])  VALUES (@angebot_nr,@Datum,@gebucht,@Gebucht_von,@Kunden_nr,@Löschen,@Ort,@Typ); ";
			query += "SELECT SCOPE_IDENTITY();";

			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{

				sqlCommand.Parameters.AddWithValue("angebot_nr", item.angebot_nr == null ? (object)DBNull.Value : item.angebot_nr);
				sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
				sqlCommand.Parameters.AddWithValue("gebucht", item.gebucht == null ? (object)DBNull.Value : item.gebucht);
				sqlCommand.Parameters.AddWithValue("Gebucht_von", item.Gebucht_von == null ? (object)DBNull.Value : item.Gebucht_von);
				sqlCommand.Parameters.AddWithValue("Kunden_nr", item.Kunden_nr == null ? (object)DBNull.Value : item.Kunden_nr);
				sqlCommand.Parameters.AddWithValue("Löschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
				sqlCommand.Parameters.AddWithValue("Ort", item.Ort == null ? (object)DBNull.Value : item.Ort);
				sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);

				var result = DbExecution.ExecuteScalar(sqlCommand);
				response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}
			//sqlTransaction.Commit();

			return response;
			//}
		}
		#endregion
	}
}
