using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{

	public class Protokolierung_Stücklisten_Bei_AktionAccess
	{

		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Protokolierung_Stücklisten_Bei_Aktion] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Protokolierung_Stücklisten_Bei_Aktion]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [Protokolierung_Stücklisten_Bei_Aktion] WHERE [ID] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity item)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Protokolierung_Stücklisten_Bei_Aktion] ([Aenderungsdatum],[Alter_menge],[Bearbeiter],[FG_Artikelnummer],[Neuer_menge],[Status],[Stück_Artikelnummer_Aktuell],[Stück_Artikelnummer_Voränderung])  VALUES (@Aenderungsdatum,@Alter_menge,@Bearbeiter,@FG_Artikelnummer,@Neuer_menge,@Status,@Stück_Artikelnummer_Aktuell,@Stück_Artikelnummer_Voränderung)";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Aenderungsdatum", item.Aenderungsdatum == null ? (object)DBNull.Value : item.Aenderungsdatum);
					sqlCommand.Parameters.AddWithValue("Alter_menge", item.Alter_menge == null ? (object)DBNull.Value : item.Alter_menge);
					sqlCommand.Parameters.AddWithValue("Bearbeiter", item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
					sqlCommand.Parameters.AddWithValue("FG_Artikelnummer", item.FG_Artikelnummer == null ? (object)DBNull.Value : item.FG_Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Neuer_menge", item.Neuer_menge == null ? (object)DBNull.Value : item.Neuer_menge);
					sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("Stück_Artikelnummer_Aktuell", item.Stück_Artikelnummer_Aktuell == null ? (object)DBNull.Value : item.Stück_Artikelnummer_Aktuell);
					sqlCommand.Parameters.AddWithValue("Stück_Artikelnummer_Voränderung", item.Stück_Artikelnummer_Voränderung == null ? (object)DBNull.Value : item.Stück_Artikelnummer_Voränderung);

					DbExecution.ExecuteNonQuery(sqlCommand);
				}

				using(var sqlCommand = new SqlCommand("SELECT [ID] FROM [Protokolierung_Stücklisten_Bei_Aktion] WHERE [ID] = @@IDENTITY", sqlConnection, sqlTransaction))
				{
					response = int.Parse(DbExecution.ExecuteScalar(sqlCommand)?.ToString() ?? "-1");
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity> items)
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
						query += " INSERT INTO [Protokolierung_Stücklisten_Bei_Aktion] ([Aenderungsdatum],[Alter_menge],[Bearbeiter],[FG_Artikelnummer],[Neuer_menge],[Status],[Stück_Artikelnummer_Aktuell],[Stück_Artikelnummer_Voränderung]) VALUES ( "

							+ "@Aenderungsdatum" + i + ","
							+ "@Alter_menge" + i + ","
							+ "@Bearbeiter" + i + ","
							+ "@FG_Artikelnummer" + i + ","
							+ "@Neuer_menge" + i + ","
							+ "@Status" + i + ","
							+ "@Stück_Artikelnummer_Aktuell" + i + ","
							+ "@Stück_Artikelnummer_Voränderung" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Aenderungsdatum" + i, item.Aenderungsdatum == null ? (object)DBNull.Value : item.Aenderungsdatum);
						sqlCommand.Parameters.AddWithValue("Alter_menge" + i, item.Alter_menge == null ? (object)DBNull.Value : item.Alter_menge);
						sqlCommand.Parameters.AddWithValue("Bearbeiter" + i, item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
						sqlCommand.Parameters.AddWithValue("FG_Artikelnummer" + i, item.FG_Artikelnummer == null ? (object)DBNull.Value : item.FG_Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Neuer_menge" + i, item.Neuer_menge == null ? (object)DBNull.Value : item.Neuer_menge);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
						sqlCommand.Parameters.AddWithValue("Stück_Artikelnummer_Aktuell" + i, item.Stück_Artikelnummer_Aktuell == null ? (object)DBNull.Value : item.Stück_Artikelnummer_Aktuell);
						sqlCommand.Parameters.AddWithValue("Stück_Artikelnummer_Voränderung" + i, item.Stück_Artikelnummer_Voränderung == null ? (object)DBNull.Value : item.Stück_Artikelnummer_Voränderung);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Protokolierung_Stücklisten_Bei_Aktion] SET [Aenderungsdatum]=@Aenderungsdatum, [Alter_menge]=@Alter_menge, [Bearbeiter]=@Bearbeiter, [FG_Artikelnummer]=@FG_Artikelnummer, [Neuer_menge]=@Neuer_menge, [Status]=@Status, [Stück_Artikelnummer_Aktuell]=@Stück_Artikelnummer_Aktuell, [Stück_Artikelnummer_Voränderung]=@Stück_Artikelnummer_Voränderung WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("Aenderungsdatum", item.Aenderungsdatum == null ? (object)DBNull.Value : item.Aenderungsdatum);
				sqlCommand.Parameters.AddWithValue("Alter_menge", item.Alter_menge == null ? (object)DBNull.Value : item.Alter_menge);
				sqlCommand.Parameters.AddWithValue("Bearbeiter", item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
				sqlCommand.Parameters.AddWithValue("FG_Artikelnummer", item.FG_Artikelnummer == null ? (object)DBNull.Value : item.FG_Artikelnummer);
				sqlCommand.Parameters.AddWithValue("Neuer_menge", item.Neuer_menge == null ? (object)DBNull.Value : item.Neuer_menge);
				sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
				sqlCommand.Parameters.AddWithValue("Stück_Artikelnummer_Aktuell", item.Stück_Artikelnummer_Aktuell == null ? (object)DBNull.Value : item.Stück_Artikelnummer_Aktuell);
				sqlCommand.Parameters.AddWithValue("Stück_Artikelnummer_Voränderung", item.Stück_Artikelnummer_Voränderung == null ? (object)DBNull.Value : item.Stück_Artikelnummer_Voränderung);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity> items)
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
						query += " UPDATE [Protokolierung_Stücklisten_Bei_Aktion] SET "

							+ "[Aenderungsdatum]=@Aenderungsdatum" + i + ","
							+ "[Alter_menge]=@Alter_menge" + i + ","
							+ "[Bearbeiter]=@Bearbeiter" + i + ","
							+ "[FG_Artikelnummer]=@FG_Artikelnummer" + i + ","
							+ "[Neuer_menge]=@Neuer_menge" + i + ","
							+ "[Status]=@Status" + i + ","
							+ "[Stück_Artikelnummer_Aktuell]=@Stück_Artikelnummer_Aktuell" + i + ","
							+ "[Stück_Artikelnummer_Voränderung]=@Stück_Artikelnummer_Voränderung" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("Aenderungsdatum" + i, item.Aenderungsdatum == null ? (object)DBNull.Value : item.Aenderungsdatum);
						sqlCommand.Parameters.AddWithValue("Alter_menge" + i, item.Alter_menge == null ? (object)DBNull.Value : item.Alter_menge);
						sqlCommand.Parameters.AddWithValue("Bearbeiter" + i, item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
						sqlCommand.Parameters.AddWithValue("FG_Artikelnummer" + i, item.FG_Artikelnummer == null ? (object)DBNull.Value : item.FG_Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Neuer_menge" + i, item.Neuer_menge == null ? (object)DBNull.Value : item.Neuer_menge);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
						sqlCommand.Parameters.AddWithValue("Stück_Artikelnummer_Aktuell" + i, item.Stück_Artikelnummer_Aktuell == null ? (object)DBNull.Value : item.Stück_Artikelnummer_Aktuell);
						sqlCommand.Parameters.AddWithValue("Stück_Artikelnummer_Voränderung" + i, item.Stück_Artikelnummer_Voränderung == null ? (object)DBNull.Value : item.Stück_Artikelnummer_Voränderung);
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
				string query = "DELETE FROM [Protokolierung_Stücklisten_Bei_Aktion] WHERE [ID]=@ID";
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

					string query = "DELETE FROM [Protokolierung_Stücklisten_Bei_Aktion] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Protokolierung_Stücklisten_Bei_Aktion] WHERE [ID]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Protokolierung_Stücklisten_Bei_Aktion]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Protokolierung_Stücklisten_Bei_Aktion] WHERE [ID] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [Protokolierung_Stücklisten_Bei_Aktion] ([Aenderungsdatum],[Alter_menge],[Bearbeiter],[FG_Artikelnummer],[Neuer_menge],[Status],[Stück_Artikelnummer_Aktuell],[Stück_Artikelnummer_Voränderung]) OUTPUT INSERTED.[ID] VALUES (@Aenderungsdatum,@Alter_menge,@Bearbeiter,@FG_Artikelnummer,@Neuer_menge,@Status,@Stuck_Artikelnummer_Aktuell,@Stuck_Artikelnummer_Voranderung); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Aenderungsdatum", item.Aenderungsdatum == null ? (object)DBNull.Value : item.Aenderungsdatum);
			sqlCommand.Parameters.AddWithValue("Alter_menge", item.Alter_menge == null ? (object)DBNull.Value : item.Alter_menge);
			sqlCommand.Parameters.AddWithValue("Bearbeiter", item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
			sqlCommand.Parameters.AddWithValue("FG_Artikelnummer", item.FG_Artikelnummer == null ? (object)DBNull.Value : item.FG_Artikelnummer);
			sqlCommand.Parameters.AddWithValue("Neuer_menge", item.Neuer_menge == null ? (object)DBNull.Value : item.Neuer_menge);
			sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
			sqlCommand.Parameters.AddWithValue("Stuck_Artikelnummer_Aktuell", item.Stück_Artikelnummer_Aktuell == null ? (object)DBNull.Value : item.Stück_Artikelnummer_Aktuell);
			sqlCommand.Parameters.AddWithValue("Stuck_Artikelnummer_Voranderung", item.Stück_Artikelnummer_Voränderung == null ? (object)DBNull.Value : item.Stück_Artikelnummer_Voränderung);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Protokolierung_Stücklisten_Bei_Aktion] ([Aenderungsdatum],[Alter_menge],[Bearbeiter],[FG_Artikelnummer],[Neuer_menge],[Status],[Stück_Artikelnummer_Aktuell],[Stück_Artikelnummer_Voränderung]) VALUES ( "

						+ "@Aenderungsdatum" + i + ","
						+ "@Alter_menge" + i + ","
						+ "@Bearbeiter" + i + ","
						+ "@FG_Artikelnummer" + i + ","
						+ "@Neuer_menge" + i + ","
						+ "@Status" + i + ","
						+ "@Stuck_Artikelnummer_Aktuell" + i + ","
						+ "@Stuck_Artikelnummer_Voranderung" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Aenderungsdatum" + i, item.Aenderungsdatum == null ? (object)DBNull.Value : item.Aenderungsdatum);
					sqlCommand.Parameters.AddWithValue("Alter_menge" + i, item.Alter_menge == null ? (object)DBNull.Value : item.Alter_menge);
					sqlCommand.Parameters.AddWithValue("Bearbeiter" + i, item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
					sqlCommand.Parameters.AddWithValue("FG_Artikelnummer" + i, item.FG_Artikelnummer == null ? (object)DBNull.Value : item.FG_Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Neuer_menge" + i, item.Neuer_menge == null ? (object)DBNull.Value : item.Neuer_menge);
					sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("Stuck_Artikelnummer_Aktuell" + i, item.Stück_Artikelnummer_Aktuell == null ? (object)DBNull.Value : item.Stück_Artikelnummer_Aktuell);
					sqlCommand.Parameters.AddWithValue("Stuck_Artikelnummer_Voranderung" + i, item.Stück_Artikelnummer_Voränderung == null ? (object)DBNull.Value : item.Stück_Artikelnummer_Voränderung);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Protokolierung_Stücklisten_Bei_Aktion] SET [Aenderungsdatum]=@Aenderungsdatum, [Alter_menge]=@Alter_menge, [Bearbeiter]=@Bearbeiter, [FG_Artikelnummer]=@FG_Artikelnummer, [Neuer_menge]=@Neuer_menge, [Status]=@Status, [Stück_Artikelnummer_Aktuell]=@Stuck_Artikelnummer_Aktuell, [Stück_Artikelnummer_Voränderung]=@Stuck_Artikelnummer_Voranderung WHERE [ID]=@ID";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("ID", item.ID);
			sqlCommand.Parameters.AddWithValue("Aenderungsdatum", item.Aenderungsdatum == null ? (object)DBNull.Value : item.Aenderungsdatum);
			sqlCommand.Parameters.AddWithValue("Alter_menge", item.Alter_menge == null ? (object)DBNull.Value : item.Alter_menge);
			sqlCommand.Parameters.AddWithValue("Bearbeiter", item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
			sqlCommand.Parameters.AddWithValue("FG_Artikelnummer", item.FG_Artikelnummer == null ? (object)DBNull.Value : item.FG_Artikelnummer);
			sqlCommand.Parameters.AddWithValue("Neuer_menge", item.Neuer_menge == null ? (object)DBNull.Value : item.Neuer_menge);
			sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
			sqlCommand.Parameters.AddWithValue("Stuck_Artikelnummer_Aktuell", item.Stück_Artikelnummer_Aktuell == null ? (object)DBNull.Value : item.Stück_Artikelnummer_Aktuell);
			sqlCommand.Parameters.AddWithValue("Stuck_Artikelnummer_Voranderung", item.Stück_Artikelnummer_Voränderung == null ? (object)DBNull.Value : item.Stück_Artikelnummer_Voränderung);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Protokolierung_Stücklisten_Bei_Aktion] SET "

					+ "[Aenderungsdatum]=@Aenderungsdatum" + i + ","
					+ "[Alter_menge]=@Alter_menge" + i + ","
					+ "[Bearbeiter]=@Bearbeiter" + i + ","
					+ "[FG_Artikelnummer]=@FG_Artikelnummer" + i + ","
					+ "[Neuer_menge]=@Neuer_menge" + i + ","
					+ "[Status]=@Status" + i + ","
					+ "[Stück_Artikelnummer_Aktuell]=@Stuck_Artikelnummer_Aktuell" + i + ","
					+ "[Stück_Artikelnummer_Voränderung]=@Stuck_Artikelnummer_Voranderung" + i + " WHERE [ID]=@ID" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
					sqlCommand.Parameters.AddWithValue("Aenderungsdatum" + i, item.Aenderungsdatum == null ? (object)DBNull.Value : item.Aenderungsdatum);
					sqlCommand.Parameters.AddWithValue("Alter_menge" + i, item.Alter_menge == null ? (object)DBNull.Value : item.Alter_menge);
					sqlCommand.Parameters.AddWithValue("Bearbeiter" + i, item.Bearbeiter == null ? (object)DBNull.Value : item.Bearbeiter);
					sqlCommand.Parameters.AddWithValue("FG_Artikelnummer" + i, item.FG_Artikelnummer == null ? (object)DBNull.Value : item.FG_Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Neuer_menge" + i, item.Neuer_menge == null ? (object)DBNull.Value : item.Neuer_menge);
					sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("Stuck_Artikelnummer_Aktuell" + i, item.Stück_Artikelnummer_Aktuell == null ? (object)DBNull.Value : item.Stück_Artikelnummer_Aktuell);
					sqlCommand.Parameters.AddWithValue("Stuck_Artikelnummer_Voranderung" + i, item.Stück_Artikelnummer_Voränderung == null ? (object)DBNull.Value : item.Stück_Artikelnummer_Voränderung);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Protokolierung_Stücklisten_Bei_Aktion] WHERE [ID]=@ID";
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

				string query = "DELETE FROM [Protokolierung_Stücklisten_Bei_Aktion] WHERE [ID] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion
		public static List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity> GetByArtikelID(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Protokolierung_Stücklisten_Bei_Aktion] where FG_Artikelnummer=@FG_Artikelnummer order by Aenderungsdatum desc";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("FG_Artikelnummer", artikelnummer);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>();
			}
		}
		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity> GetTop(int max, string article)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT TOP {max} FROM [Protokolierung_Stücklisten_Bei_Aktion] WHERE [FG_Artikelnummer]=@article";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("article", article);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>();
			}
		}

		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
