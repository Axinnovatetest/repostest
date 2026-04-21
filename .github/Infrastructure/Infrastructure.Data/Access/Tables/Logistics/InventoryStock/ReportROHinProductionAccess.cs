using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Infrastructure.Data.Entities.Joins.Logistics;

namespace Infrastructure.Data.Access.Tables.Logistics.InventoryStock
{
	public class ReportROHinProductionAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Inventory].[ReportROHinProduction] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Inventory].[ReportROHinProduction]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Inventory].[ReportROHinProduction] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Inventory].[ReportROHinProduction] ([ArtikelNr],[Artikelnummer],[IdSpule],[InventoryYear],[LagerId],[MengeInProduktion],[StatusSpule]) OUTPUT INSERTED.[Id] VALUES (@ArtikelNr,@Artikelnummer,@IdSpule,@InventoryYear,@LagerId,@MengeInProduktion,@StatusSpule); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("IdSpule", item.IdSpule);
					sqlCommand.Parameters.AddWithValue("InventoryYear", item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
					sqlCommand.Parameters.AddWithValue("LagerId", item.LagerId);
					sqlCommand.Parameters.AddWithValue("MengeInProduktion", item.MengeInProduktion);
					sqlCommand.Parameters.AddWithValue("StatusSpule", item.StatusSpule);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity> items)
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
						query += " INSERT INTO [Inventory].[ReportROHinProduction] ([ArtikelNr],[Artikelnummer],[IdSpule],[InventoryYear],[LagerId],[MengeInProduktion],[StatusSpule]) VALUES ( "

							+ "@ArtikelNr" + i + ","
							+ "@Artikelnummer" + i + ","
							+ "@IdSpule" + i + ","
							+ "@InventoryYear" + i + ","
							+ "@LagerId" + i + ","
							+ "@MengeInProduktion" + i + ","
							+ "@StatusSpule" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("IdSpule" + i, item.IdSpule);
						sqlCommand.Parameters.AddWithValue("InventoryYear" + i, item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
						sqlCommand.Parameters.AddWithValue("LagerId" + i, item.LagerId);
						sqlCommand.Parameters.AddWithValue("MengeInProduktion" + i, item.MengeInProduktion);
						sqlCommand.Parameters.AddWithValue("StatusSpule" + i, item.StatusSpule);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Inventory].[ReportROHinProduction] SET [ArtikelNr]=@ArtikelNr, [Artikelnummer]=@Artikelnummer, [IdSpule]=@IdSpule, [InventoryYear]=@InventoryYear, [LagerId]=@LagerId, [MengeInProduktion]=@MengeInProduktion, [StatusSpule]=@StatusSpule WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("IdSpule", item.IdSpule);
				sqlCommand.Parameters.AddWithValue("InventoryYear", item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
				sqlCommand.Parameters.AddWithValue("LagerId", item.LagerId);
				sqlCommand.Parameters.AddWithValue("MengeInProduktion", item.MengeInProduktion);
				sqlCommand.Parameters.AddWithValue("StatusSpule", item.StatusSpule);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity> items)
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
						query += " UPDATE [Inventory].[ReportROHinProduction] SET "

							+ "[ArtikelNr]=@ArtikelNr" + i + ","
							+ "[Artikelnummer]=@Artikelnummer" + i + ","
							+ "[IdSpule]=@IdSpule" + i + ","
							+ "[InventoryYear]=@InventoryYear" + i + ","
							+ "[LagerId]=@LagerId" + i + ","
							+ "[MengeInProduktion]=@MengeInProduktion" + i + ","
							+ "[StatusSpule]=@StatusSpule" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("IdSpule" + i, item.IdSpule);
						sqlCommand.Parameters.AddWithValue("InventoryYear" + i, item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
						sqlCommand.Parameters.AddWithValue("LagerId" + i, item.LagerId);
						sqlCommand.Parameters.AddWithValue("MengeInProduktion" + i, item.MengeInProduktion);
						sqlCommand.Parameters.AddWithValue("StatusSpule" + i, item.StatusSpule);
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
				string query = "DELETE FROM [Inventory].[ReportROHinProduction] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [Inventory].[ReportROHinProduction] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Inventory].[ReportROHinProduction] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Inventory].[ReportROHinProduction]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Inventory].[ReportROHinProduction] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [Inventory].[ReportROHinProduction] ([ArtikelNr],[Artikelnummer],[IdSpule],[InventoryYear],[LagerId],[MengeInProduktion],[StatusSpule]) OUTPUT INSERTED.[Id] VALUES (@ArtikelNr,@Artikelnummer,@IdSpule,@InventoryYear,@LagerId,@MengeInProduktion,@StatusSpule); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer);
			sqlCommand.Parameters.AddWithValue("IdSpule", item.IdSpule);
			sqlCommand.Parameters.AddWithValue("InventoryYear", item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
			sqlCommand.Parameters.AddWithValue("LagerId", item.LagerId);
			sqlCommand.Parameters.AddWithValue("MengeInProduktion", item.MengeInProduktion);
			sqlCommand.Parameters.AddWithValue("StatusSpule", item.StatusSpule);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Inventory].[ReportROHinProduction] ([ArtikelNr],[Artikelnummer],[IdSpule],[InventoryYear],[LagerId],[MengeInProduktion],[StatusSpule]) VALUES ( "

						+ "@ArtikelNr" + i + ","
						+ "@Artikelnummer" + i + ","
						+ "@IdSpule" + i + ","
						+ "@InventoryYear" + i + ","
						+ "@LagerId" + i + ","
						+ "@MengeInProduktion" + i + ","
						+ "@StatusSpule" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("IdSpule" + i, item.IdSpule);
					sqlCommand.Parameters.AddWithValue("InventoryYear" + i, item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
					sqlCommand.Parameters.AddWithValue("LagerId" + i, item.LagerId);
					sqlCommand.Parameters.AddWithValue("MengeInProduktion" + i, item.MengeInProduktion);
					sqlCommand.Parameters.AddWithValue("StatusSpule" + i, item.StatusSpule);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Inventory].[ReportROHinProduction] SET [ArtikelNr]=@ArtikelNr, [Artikelnummer]=@Artikelnummer, [IdSpule]=@IdSpule, [InventoryYear]=@InventoryYear, [LagerId]=@LagerId, [MengeInProduktion]=@MengeInProduktion, [StatusSpule]=@StatusSpule WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer);
			sqlCommand.Parameters.AddWithValue("IdSpule", item.IdSpule);
			sqlCommand.Parameters.AddWithValue("InventoryYear", item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
			sqlCommand.Parameters.AddWithValue("LagerId", item.LagerId);
			sqlCommand.Parameters.AddWithValue("MengeInProduktion", item.MengeInProduktion);
			sqlCommand.Parameters.AddWithValue("StatusSpule", item.StatusSpule);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Inventory].[ReportROHinProduction] SET "

					+ "[ArtikelNr]=@ArtikelNr" + i + ","
					+ "[Artikelnummer]=@Artikelnummer" + i + ","
					+ "[IdSpule]=@IdSpule" + i + ","
					+ "[InventoryYear]=@InventoryYear" + i + ","
					+ "[LagerId]=@LagerId" + i + ","
					+ "[MengeInProduktion]=@MengeInProduktion" + i + ","
					+ "[StatusSpule]=@StatusSpule" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("IdSpule" + i, item.IdSpule);
					sqlCommand.Parameters.AddWithValue("InventoryYear" + i, item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
					sqlCommand.Parameters.AddWithValue("LagerId" + i, item.LagerId);
					sqlCommand.Parameters.AddWithValue("MengeInProduktion" + i, item.MengeInProduktion);
					sqlCommand.Parameters.AddWithValue("StatusSpule" + i, item.StatusSpule);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Inventory].[ReportROHinProduction] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

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

				string query = "DELETE FROM [Inventory].[ReportROHinProduction] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		static string DATA_SELECT_CLAUSE() => @"SELECT 
								a.[Artikel-Nr] as ArtikelNr,
								a.Artikelnummer, 
								GETDATE() AS InventoryYear,
								@LagerId AS LagerId,
								f.Bedarf AS BedarfFa,	
								0 GefundeneMengeInProduktion,	
								CAST(CASE WHEN a.Warentyp=1 THEN l.Bestand ELSE l.Gesamtbestand END AS MONEY) MengeInProduktion
								";
		static string DATA_FROM_CLAUSE() => @"FROM Artikel a
								JOIN Lager l on l.[Artikel-Nr]=a.[Artikel-Nr] AND l.Lagerort_id IN {0}
								JOIN (SELECT p.Artikel_Nr, SUM(p.Anzahl * f.Anzahl) Bedarf FROM Fertigung f 
									JOIN Fertigung_Positionen p on p.ID_Fertigung=f.ID 
									WHERE f.Kennzeichen='offen' AND ISNULL(FA_Gestartet,0)=1 AND f.Lagerort_id=@LagerId
									GROUP BY p.Artikel_Nr
								) f on f.Artikel_Nr=a.[Artikel-Nr]
								WHERE a.Warengruppe='ROH' AND (l.Bestand>0.0001 OR l.Gesamtbestand>0.0001) AND f.Bedarf < CASE WHEN a.Warentyp=1 THEN l.Bestand ELSE l.Gesamtbestand END";
		public static int CountData(string? filterSearch, int LagerId, List<int> prodLagerId, int warentyp)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				var query = $"SELECT COUNT(*) {String.Format(DATA_FROM_CLAUSE(), $"({string.Join(",", prodLagerId)})")}";

				// Search filter if provided
				if(!string.IsNullOrWhiteSpace(filterSearch) && filterSearch.Length > 1)
				{
					query += @" AND (a.Artikelnummer LIKE @filterSearch OR CAST(BedarfFa AS NVARCHAR) LIKE @filterSearch) ";
				}

				query += $" AND a.[Warentyp]={warentyp}";
				using(var sqlCommand = new SqlCommand(query, sqlConnection))
				{
					sqlCommand.Parameters.Add("@LagerId", SqlDbType.Int).Value = LagerId;

					if(!string.IsNullOrWhiteSpace(filterSearch) && filterSearch.Length > 1)
						sqlCommand.Parameters.Add("@filterSearch", SqlDbType.NVarChar, 100).Value = $"%{filterSearch}%";

					sqlCommand.CommandTimeout = 300;

					return int.TryParse(DbExecution.ExecuteScalar(sqlCommand)?.ToString(), out var val) ? val : 0;
				}
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity> GetData(
		string? filterSearch,
		int LagerId,
		List<int> prodLagerId,
		int warentyp,
		bool? isContact,
		Settings.SortingModel dataSorting,
		Settings.PaginModel? dataPaging)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				// Base query
				var query = $"{DATA_SELECT_CLAUSE()} {String.Format(DATA_FROM_CLAUSE(), $"({string.Join(",", prodLagerId)})")}";

				// Search filter if provided
				if(!string.IsNullOrWhiteSpace(filterSearch) && filterSearch.Length > 1)
				{
					query += @" AND (a.Artikelnummer LIKE @filterSearch OR CAST(BedarfFa AS NVARCHAR) LIKE @filterSearch) ";
				}
				query += $" AND a.[Warentyp]={warentyp}";
				if(isContact.HasValue)
				{
					if(isContact.Value)
					{
						query += $" AND a.[Artikelnummer] LIKE '05%'";
					}
					else
					{
						query += $" AND a.[Artikelnummer] NOT LIKE '05%'";
					}
				}

				// Sorting - whitelist to prevent SQL injection
				string sortColumn = "a.Artikelnummer"; // default
				if(dataSorting != null && !string.IsNullOrWhiteSpace(dataSorting.SortFieldName))
				{
					sortColumn = dataSorting.SortFieldName;
				}
				query += $" ORDER BY {sortColumn} {(dataSorting?.SortDesc == true ? "DESC" : "ASC")}";

				// Pagination if provided
				if(dataPaging != null)
				{
					query += $" OFFSET {dataPaging.FirstRowNumber} ROWS FETCH NEXT {dataPaging.RequestRows} ROWS ONLY";
				}

				using(var sqlCommand = new SqlCommand(query, sqlConnection))
				{
					sqlCommand.Parameters.Add("@LagerId", SqlDbType.Int).Value = LagerId;

					if(!string.IsNullOrWhiteSpace(filterSearch) && filterSearch.Length > 1)
						sqlCommand.Parameters.Add("@filterSearch", SqlDbType.NVarChar, 100).Value = $"%{filterSearch}%";

					DbExecution.Fill(sqlCommand, dataTable);
				}
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportROHinProductionEntity>();
			}
		}
		public static int InitRohNotNeeded(int LagerId, SqlConnection connection, SqlTransaction transaction)
		{
			string query = $@"
			INSERT INTO [Inventory].[ReportROHinProduction] ([ArtikelNr],[Artikelnummer],[InventoryYear],[LagerId],[MengeInProduktion],[IdSpule],[StatusSpule]) 
				OUTPUT INSERTED.[Id] {DATA_SELECT_CLAUSE()} {DATA_FROM_CLAUSE()}";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.CommandTimeout = 180;
			sqlCommand.Parameters.AddWithValue("LagerId", LagerId);
			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		#endregion Custom Methods

	}
}
