using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Tables.Logistics.InventoryStock
{
	public class ReportOneTblAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Inventory].[ReportOneTbl] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Inventory].[ReportOneTbl]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
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

					sqlCommand.CommandText = $"SELECT * FROM [Inventory].[ReportOneTbl] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [ReportOneTbl] ([ArtikelNr],[Artikelnummer],[FaGeschnitten],[FaKommisioniert],[FaTermin],[FertigungId],[Fertigungsnummer],[InventoryYear],[LagerId],[OffeneMenge]) OUTPUT INSERTED.[Id] VALUES (@ArtikelNr,@Artikelnummer,@FaGeschnitten,@FaKommisioniert,@FaTermin,@FertigungId,@Fertigungsnummer,@InventoryYear,@LagerId,@OffeneMenge); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("FaGeschnitten", item.FaGeschnitten);
					sqlCommand.Parameters.AddWithValue("FaKommisioniert", item.FaKommisioniert);
					sqlCommand.Parameters.AddWithValue("FaTermin", item.FaTermin == null ? (object)DBNull.Value : item.FaTermin);
					sqlCommand.Parameters.AddWithValue("FertigungId", item.FertigungId);
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("InventoryYear", item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
					sqlCommand.Parameters.AddWithValue("LagerId", item.LagerId);
					sqlCommand.Parameters.AddWithValue("OffeneMenge", item.OffeneMenge);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 11; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [ReportOneTbl] ([ArtikelNr],[Artikelnummer],[FaGeschnitten],[FaKommisioniert],[FaTermin],[FertigungId],[Fertigungsnummer],[InventoryYear],[LagerId],[OffeneMenge]) VALUES ( "

							+ "@ArtikelNr" + i + ","
							+ "@Artikelnummer" + i + ","
							+ "@FaGeschnitten" + i + ","
							+ "@FaKommisioniert" + i + ","
							+ "@FaTermin" + i + ","
							+ "@FertigungId" + i + ","
							+ "@Fertigungsnummer" + i + ","
							+ "@InventoryYear" + i + ","
							+ "@LagerId" + i + ","
							+ "@OffeneMenge" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("FaGeschnitten" + i, item.FaGeschnitten);
						sqlCommand.Parameters.AddWithValue("FaKommisioniert" + i, item.FaKommisioniert);
						sqlCommand.Parameters.AddWithValue("FaTermin" + i, item.FaTermin == null ? (object)DBNull.Value : item.FaTermin);
						sqlCommand.Parameters.AddWithValue("FertigungId" + i, item.FertigungId);
						sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer);
						sqlCommand.Parameters.AddWithValue("InventoryYear" + i, item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
						sqlCommand.Parameters.AddWithValue("LagerId" + i, item.LagerId);
						sqlCommand.Parameters.AddWithValue("OffeneMenge" + i, item.OffeneMenge);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [ReportOneTbl] SET [ArtikelNr]=@ArtikelNr, [Artikelnummer]=@Artikelnummer, [FaGeschnitten]=@FaGeschnitten, [FaKommisioniert]=@FaKommisioniert, [FaTermin]=@FaTermin, [FertigungId]=@FertigungId, [Fertigungsnummer]=@Fertigungsnummer, [InventoryYear]=@InventoryYear, [LagerId]=@LagerId, [OffeneMenge]=@OffeneMenge WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("FaGeschnitten", item.FaGeschnitten);
				sqlCommand.Parameters.AddWithValue("FaKommisioniert", item.FaKommisioniert);
				sqlCommand.Parameters.AddWithValue("FaTermin", item.FaTermin == null ? (object)DBNull.Value : item.FaTermin);
				sqlCommand.Parameters.AddWithValue("FertigungId", item.FertigungId);
				sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer);
				sqlCommand.Parameters.AddWithValue("InventoryYear", item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
				sqlCommand.Parameters.AddWithValue("LagerId", item.LagerId);
				sqlCommand.Parameters.AddWithValue("OffeneMenge", item.OffeneMenge);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 11; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [ReportOneTbl] SET "

							+ "[ArtikelNr]=@ArtikelNr" + i + ","
							+ "[Artikelnummer]=@Artikelnummer" + i + ","
							+ "[FaGeschnitten]=@FaGeschnitten" + i + ","
							+ "[FaKommisioniert]=@FaKommisioniert" + i + ","
							+ "[FaTermin]=@FaTermin" + i + ","
							+ "[FertigungId]=@FertigungId" + i + ","
							+ "[Fertigungsnummer]=@Fertigungsnummer" + i + ","
							+ "[InventoryYear]=@InventoryYear" + i + ","
							+ "[LagerId]=@LagerId" + i + ","
							+ "[OffeneMenge]=@OffeneMenge" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("FaGeschnitten" + i, item.FaGeschnitten);
						sqlCommand.Parameters.AddWithValue("FaKommisioniert" + i, item.FaKommisioniert);
						sqlCommand.Parameters.AddWithValue("FaTermin" + i, item.FaTermin == null ? (object)DBNull.Value : item.FaTermin);
						sqlCommand.Parameters.AddWithValue("FertigungId" + i, item.FertigungId);
						sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer);
						sqlCommand.Parameters.AddWithValue("InventoryYear" + i, item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
						sqlCommand.Parameters.AddWithValue("LagerId" + i, item.LagerId);
						sqlCommand.Parameters.AddWithValue("OffeneMenge" + i, item.OffeneMenge);
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
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Inventory].[ReportOneTbl] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
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
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
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

					string query = "DELETE FROM [Inventory].[ReportOneTbl] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [ReportOneTbl] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [ReportOneTbl]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [ReportOneTbl] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [ReportOneTbl] ([ArtikelNr],[Artikelnummer],[FaGeschnitten],[FaKommisioniert],[FaTermin],[FertigungId],[Fertigungsnummer],[InventoryYear],[LagerId],[OffeneMenge]) OUTPUT INSERTED.[Id] VALUES (@ArtikelNr,@Artikelnummer,@FaGeschnitten,@FaKommisioniert,@FaTermin,@FertigungId,@Fertigungsnummer,@InventoryYear,@LagerId,@OffeneMenge); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer);
			sqlCommand.Parameters.AddWithValue("FaGeschnitten", item.FaGeschnitten);
			sqlCommand.Parameters.AddWithValue("FaKommisioniert", item.FaKommisioniert);
			sqlCommand.Parameters.AddWithValue("FaTermin", item.FaTermin == null ? (object)DBNull.Value : item.FaTermin);
			sqlCommand.Parameters.AddWithValue("FertigungId", item.FertigungId);
			sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer);
			sqlCommand.Parameters.AddWithValue("InventoryYear", item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
			sqlCommand.Parameters.AddWithValue("LagerId", item.LagerId);
			sqlCommand.Parameters.AddWithValue("OffeneMenge", item.OffeneMenge);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 11; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [ReportOneTbl] ([ArtikelNr],[Artikelnummer],[FaGeschnitten],[FaKommisioniert],[FaTermin],[FertigungId],[Fertigungsnummer],[InventoryYear],[LagerId],[OffeneMenge]) VALUES ( "

						+ "@ArtikelNr" + i + ","
						+ "@Artikelnummer" + i + ","
						+ "@FaGeschnitten" + i + ","
						+ "@FaKommisioniert" + i + ","
						+ "@FaTermin" + i + ","
						+ "@FertigungId" + i + ","
						+ "@Fertigungsnummer" + i + ","
						+ "@InventoryYear" + i + ","
						+ "@LagerId" + i + ","
						+ "@OffeneMenge" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("FaGeschnitten" + i, item.FaGeschnitten);
					sqlCommand.Parameters.AddWithValue("FaKommisioniert" + i, item.FaKommisioniert);
					sqlCommand.Parameters.AddWithValue("FaTermin" + i, item.FaTermin == null ? (object)DBNull.Value : item.FaTermin);
					sqlCommand.Parameters.AddWithValue("FertigungId" + i, item.FertigungId);
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("InventoryYear" + i, item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
					sqlCommand.Parameters.AddWithValue("LagerId" + i, item.LagerId);
					sqlCommand.Parameters.AddWithValue("OffeneMenge" + i, item.OffeneMenge);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [ReportOneTbl] SET [ArtikelNr]=@ArtikelNr, [Artikelnummer]=@Artikelnummer, [FaGeschnitten]=@FaGeschnitten, [FaKommisioniert]=@FaKommisioniert, [FaTermin]=@FaTermin, [FertigungId]=@FertigungId, [Fertigungsnummer]=@Fertigungsnummer, [InventoryYear]=@InventoryYear, [LagerId]=@LagerId, [OffeneMenge]=@OffeneMenge WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer);
			sqlCommand.Parameters.AddWithValue("FaGeschnitten", item.FaGeschnitten);
			sqlCommand.Parameters.AddWithValue("FaKommisioniert", item.FaKommisioniert);
			sqlCommand.Parameters.AddWithValue("FaTermin", item.FaTermin == null ? (object)DBNull.Value : item.FaTermin);
			sqlCommand.Parameters.AddWithValue("FertigungId", item.FertigungId);
			sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer);
			sqlCommand.Parameters.AddWithValue("InventoryYear", item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
			sqlCommand.Parameters.AddWithValue("LagerId", item.LagerId);
			sqlCommand.Parameters.AddWithValue("OffeneMenge", item.OffeneMenge);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 11; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [ReportOneTbl] SET "

					+ "[ArtikelNr]=@ArtikelNr" + i + ","
					+ "[Artikelnummer]=@Artikelnummer" + i + ","
					+ "[FaGeschnitten]=@FaGeschnitten" + i + ","
					+ "[FaKommisioniert]=@FaKommisioniert" + i + ","
					+ "[FaTermin]=@FaTermin" + i + ","
					+ "[FertigungId]=@FertigungId" + i + ","
					+ "[Fertigungsnummer]=@Fertigungsnummer" + i + ","
					+ "[InventoryYear]=@InventoryYear" + i + ","
					+ "[LagerId]=@LagerId" + i + ","
					+ "[OffeneMenge]=@OffeneMenge" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("FaGeschnitten" + i, item.FaGeschnitten);
					sqlCommand.Parameters.AddWithValue("FaKommisioniert" + i, item.FaKommisioniert);
					sqlCommand.Parameters.AddWithValue("FaTermin" + i, item.FaTermin == null ? (object)DBNull.Value : item.FaTermin);
					sqlCommand.Parameters.AddWithValue("FertigungId" + i, item.FertigungId);
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("InventoryYear" + i, item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
					sqlCommand.Parameters.AddWithValue("LagerId" + i, item.LagerId);
					sqlCommand.Parameters.AddWithValue("OffeneMenge" + i, item.OffeneMenge);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [ReportOneTbl] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			results = DbExecution.ExecuteNonQuery(sqlCommand);


			return results;
		}
		public static int DeleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
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

				string query = "DELETE FROM [ReportOneTbl] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods

		static string DATA_QUERY_SELECT_CLAUSE(int lager) { return $@"SELECT
				a.[Artikel-Nr] as ArtikelNr,
				a.Artikelnummer, 
				f.ID FertigungId,
				f.Fertigungsnummer,
				f.Anzahl OffeneMenge, 
				f.Termin_Bestätigt1 FaTermin, 
				{lager} AS LagerId,
				CASE WHEN ISNULL(f.Kabel_geschnitten,0) = 0 THEN 'N' ELSE 'Y' END FaGeschnitten, 
				CASE WHEN ISNULL(f.Kommisioniert_komplett,0) = 0 THEN 'N' ELSE 'Y' END  FaKommisioniert,
				GETDATE() AS InventoryYear
				"; }
		static string DATA_QUERY_FROM_CLAUSE(List<int> ids) { return $@"FROM Fertigung f
				JOIN Artikel a on a.[Artikel-Nr]=f.Artikel_Nr
				WHERE Kennzeichen='offen' AND ISNULL(FA_Gestartet,0)=1 AND Lagerort_id IN ({string.Join(",", ids)})"; }
		public static int CountReportOneData(string? filterSearch, List<int> LagerIds)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				var query = $@"SELECT COUNT(*) {DATA_QUERY_FROM_CLAUSE(LagerIds)}";

				if(!string.IsNullOrWhiteSpace(filterSearch) && filterSearch.Length > 1)
				{
					query += @"
                AND (
                    f.Fertigungsnummer LIKE @filterSearch
                    OR a.Artikelnummer LIKE @filterSearch
                    OR CAST(f.Anzahl AS NVARCHAR) LIKE @filterSearch
                )
            ";
				}

				using(var sqlCommand = new SqlCommand(query, sqlConnection))
				{
					if(!string.IsNullOrWhiteSpace(filterSearch) && filterSearch.Length > 1)
						sqlCommand.Parameters.Add("@filterSearch", SqlDbType.NVarChar, 100).Value = $"%{filterSearch}%";

					sqlCommand.CommandTimeout = 300;
					return int.TryParse(DbExecution.ExecuteScalar(sqlCommand)?.ToString(), out var val) ? val : 0;
				}
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity> GetReportOneData(
		string? filterSearch,
		List<int> LagerIds,
		Settings.SortingModel dataSorting,
		Settings.PaginModel? dataPaging)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				// Base query
				var query = $@"{DATA_QUERY_SELECT_CLAUSE(LagerIds[0])} {DATA_QUERY_FROM_CLAUSE(LagerIds)}";

				// Search filter
				if(!string.IsNullOrWhiteSpace(filterSearch) && filterSearch.Length > 1)
				{
					query += @"
                AND (
                    f.Fertigungsnummer LIKE @filterSearch
                    OR a.Artikelnummer LIKE @filterSearch
                    OR CAST(f.Anzahl AS NVARCHAR) LIKE @filterSearch
                )
            ";
				}

				// Sorting
				string sortColumn = "Fertigungsnummer"; // default
				if(dataSorting != null && !string.IsNullOrWhiteSpace(dataSorting.SortFieldName))
				{
					sortColumn = dataSorting.SortFieldName;
				}
				query += $" ORDER BY {sortColumn} {(dataSorting?.SortDesc == true ? "DESC" : "ASC")}";

				// Pagination
				if(dataPaging != null)
				{
					query += $" OFFSET {dataPaging.FirstRowNumber} ROWS FETCH NEXT {dataPaging.RequestRows} ROWS ONLY";
				}

				using(var sqlCommand = new SqlCommand(query, sqlConnection))
				{
					if(!string.IsNullOrWhiteSpace(filterSearch) && filterSearch.Length > 1)
						sqlCommand.Parameters.Add("@filterSearch", SqlDbType.NVarChar, 100).Value = $"%{filterSearch}%";

					DbExecution.Fill(sqlCommand, dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity>();
			}
		}
		public static int InitOpenFaReport(List<int> LagerIds, SqlConnection connection, SqlTransaction transaction)
		{
			string query = $@"
			INSERT INTO [Inventory].[ReportOneTbl] ([ArtikelNr],[Artikelnummer],[FertigungId],[Fertigungsnummer],[OffeneMenge],[FaTermin],[LagerId],[FaGeschnitten],[FaKommisioniert],[InventoryYear]) 
				OUTPUT INSERTED.[Id] {DATA_QUERY_SELECT_CLAUSE(LagerIds[0])} {DATA_QUERY_FROM_CLAUSE(LagerIds)}";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.CommandTimeout = 180;
			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity> GetAll(List<int> LagerIds)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"{DATA_QUERY_SELECT_CLAUSE(LagerIds[0])} {DATA_QUERY_FROM_CLAUSE(LagerIds)}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity>();
			}
		}
		#endregion Custom Methods

	}
}
