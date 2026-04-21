using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Data.Entities.Joins.Logistics;

namespace Infrastructure.Data.Access.Tables.Logistics.InventoryStock
{
	public class ReportTwoTblAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Inventory].[ReportTwoTbl] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Inventory].[ReportTwoTbl]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Inventory].[ReportTwoTbl] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Inventory].[ReportTwoTbl] ([ArtikelNr],[Artikelnummer],[GefundeneMengeInProduktion],[InventoryYear],[Lagerbestand],[LagerId],[MengeInProduktion],[RueckbuchungBestaetigt]) OUTPUT INSERTED.[Id] VALUES (@ArtikelNr,@Artikelnummer,@GefundeneMengeInProduktion,@InventoryYear,@Lagerbestand,@LagerId,@MengeInProduktion,@RueckbuchungBestaetigt); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("GefundeneMengeInProduktion", item.GefundeneMengeInProduktion);
					sqlCommand.Parameters.AddWithValue("InventoryYear", item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
					sqlCommand.Parameters.AddWithValue("Lagerbestand", item.Lagerbestand);
					sqlCommand.Parameters.AddWithValue("LagerId", item.LagerId);
					sqlCommand.Parameters.AddWithValue("MengeInProduktion", item.MengeInProduktion);
					sqlCommand.Parameters.AddWithValue("RueckbuchungBestaetigt", item.RueckbuchungBestaetigt);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity> items)
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
						query += " INSERT INTO [Inventory].[ReportTwoTbl] ([ArtikelNr],[Artikelnummer],[GefundeneMengeInProduktion],[InventoryYear],[Lagerbestand],[LagerId],[MengeInProduktion],[RueckbuchungBestaetigt]) VALUES ( "

							+ "@ArtikelNr" + i + ","
							+ "@Artikelnummer" + i + ","
							+ "@GefundeneMengeInProduktion" + i + ","
							+ "@InventoryYear" + i + ","
							+ "@Lagerbestand" + i + ","
							+ "@LagerId" + i + ","
							+ "@MengeInProduktion" + i + ","
							+ "@RueckbuchungBestaetigt" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("GefundeneMengeInProduktion" + i, item.GefundeneMengeInProduktion);
						sqlCommand.Parameters.AddWithValue("InventoryYear" + i, item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
						sqlCommand.Parameters.AddWithValue("Lagerbestand" + i, item.Lagerbestand);
						sqlCommand.Parameters.AddWithValue("LagerId" + i, item.LagerId);
						sqlCommand.Parameters.AddWithValue("MengeInProduktion" + i, item.MengeInProduktion);
						sqlCommand.Parameters.AddWithValue("RueckbuchungBestaetigt" + i, item.RueckbuchungBestaetigt);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Inventory].[ReportTwoTbl] SET [ArtikelNr]=@ArtikelNr, [Artikelnummer]=@Artikelnummer, [GefundeneMengeInProduktion]=@GefundeneMengeInProduktion, [InventoryYear]=@InventoryYear, [Lagerbestand]=@Lagerbestand, [LagerId]=@LagerId, [MengeInProduktion]=@MengeInProduktion, [RueckbuchungBestaetigt]=@RueckbuchungBestaetigt WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("GefundeneMengeInProduktion", item.GefundeneMengeInProduktion);
				sqlCommand.Parameters.AddWithValue("InventoryYear", item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
				sqlCommand.Parameters.AddWithValue("Lagerbestand", item.Lagerbestand);
				sqlCommand.Parameters.AddWithValue("LagerId", item.LagerId);
				sqlCommand.Parameters.AddWithValue("MengeInProduktion", item.MengeInProduktion);
				sqlCommand.Parameters.AddWithValue("RueckbuchungBestaetigt", item.RueckbuchungBestaetigt);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity> items)
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
						query += " UPDATE [Inventory].[ReportTwoTbl] SET "

							+ "[ArtikelNr]=@ArtikelNr" + i + ","
							+ "[Artikelnummer]=@Artikelnummer" + i + ","
							+ "[GefundeneMengeInProduktion]=@GefundeneMengeInProduktion" + i + ","
							+ "[InventoryYear]=@InventoryYear" + i + ","
							+ "[Lagerbestand]=@Lagerbestand" + i + ","
							+ "[LagerId]=@LagerId" + i + ","
							+ "[MengeInProduktion]=@MengeInProduktion" + i + ","
							+ "[RueckbuchungBestaetigt]=@RueckbuchungBestaetigt" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("GefundeneMengeInProduktion" + i, item.GefundeneMengeInProduktion);
						sqlCommand.Parameters.AddWithValue("InventoryYear" + i, item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
						sqlCommand.Parameters.AddWithValue("Lagerbestand" + i, item.Lagerbestand);
						sqlCommand.Parameters.AddWithValue("LagerId" + i, item.LagerId);
						sqlCommand.Parameters.AddWithValue("MengeInProduktion" + i, item.MengeInProduktion);
						sqlCommand.Parameters.AddWithValue("RueckbuchungBestaetigt" + i, item.RueckbuchungBestaetigt);
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
				string query = "DELETE FROM [Inventory].[ReportTwoTbl] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [Inventory].[ReportTwoTbl] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Inventory].[ReportTwoTbl] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Inventory].[ReportTwoTbl]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Inventory].[ReportTwoTbl] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [Inventory].[ReportTwoTbl] ([ArtikelNr],[Artikelnummer],[GefundeneMengeInProduktion],[InventoryYear],[Lagerbestand],[LagerId],[MengeInProduktion],[RueckbuchungBestaetigt]) OUTPUT INSERTED.[Id] VALUES (@ArtikelNr,@Artikelnummer,@GefundeneMengeInProduktion,@InventoryYear,@Lagerbestand,@LagerId,@MengeInProduktion,@RueckbuchungBestaetigt); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer);
			sqlCommand.Parameters.AddWithValue("GefundeneMengeInProduktion", item.GefundeneMengeInProduktion);
			sqlCommand.Parameters.AddWithValue("InventoryYear", item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
			sqlCommand.Parameters.AddWithValue("Lagerbestand", item.Lagerbestand);
			sqlCommand.Parameters.AddWithValue("LagerId", item.LagerId);
			sqlCommand.Parameters.AddWithValue("MengeInProduktion", item.MengeInProduktion);
			sqlCommand.Parameters.AddWithValue("RueckbuchungBestaetigt", item.RueckbuchungBestaetigt);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Inventory].[ReportTwoTbl] ([ArtikelNr],[Artikelnummer],[GefundeneMengeInProduktion],[InventoryYear],[Lagerbestand],[LagerId],[MengeInProduktion],[RueckbuchungBestaetigt]) VALUES ( "

						+ "@ArtikelNr" + i + ","
						+ "@Artikelnummer" + i + ","
						+ "@GefundeneMengeInProduktion" + i + ","
						+ "@InventoryYear" + i + ","
						+ "@Lagerbestand" + i + ","
						+ "@LagerId" + i + ","
						+ "@MengeInProduktion" + i + ","
						+ "@RueckbuchungBestaetigt" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("GefundeneMengeInProduktion" + i, item.GefundeneMengeInProduktion);
					sqlCommand.Parameters.AddWithValue("InventoryYear" + i, item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
					sqlCommand.Parameters.AddWithValue("Lagerbestand" + i, item.Lagerbestand);
					sqlCommand.Parameters.AddWithValue("LagerId" + i, item.LagerId);
					sqlCommand.Parameters.AddWithValue("MengeInProduktion" + i, item.MengeInProduktion);
					sqlCommand.Parameters.AddWithValue("RueckbuchungBestaetigt" + i, item.RueckbuchungBestaetigt);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Inventory].[ReportTwoTbl] SET [ArtikelNr]=@ArtikelNr, [Artikelnummer]=@Artikelnummer, [GefundeneMengeInProduktion]=@GefundeneMengeInProduktion, [InventoryYear]=@InventoryYear, [Lagerbestand]=@Lagerbestand, [LagerId]=@LagerId, [MengeInProduktion]=@MengeInProduktion, [RueckbuchungBestaetigt]=@RueckbuchungBestaetigt WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer);
			sqlCommand.Parameters.AddWithValue("GefundeneMengeInProduktion", item.GefundeneMengeInProduktion);
			sqlCommand.Parameters.AddWithValue("InventoryYear", item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
			sqlCommand.Parameters.AddWithValue("Lagerbestand", item.Lagerbestand);
			sqlCommand.Parameters.AddWithValue("LagerId", item.LagerId);
			sqlCommand.Parameters.AddWithValue("MengeInProduktion", item.MengeInProduktion);
			sqlCommand.Parameters.AddWithValue("RueckbuchungBestaetigt", item.RueckbuchungBestaetigt);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Inventory].[ReportTwoTbl] SET "

					+ "[ArtikelNr]=@ArtikelNr" + i + ","
					+ "[Artikelnummer]=@Artikelnummer" + i + ","
					+ "[GefundeneMengeInProduktion]=@GefundeneMengeInProduktion" + i + ","
					+ "[InventoryYear]=@InventoryYear" + i + ","
					+ "[Lagerbestand]=@Lagerbestand" + i + ","
					+ "[LagerId]=@LagerId" + i + ","
					+ "[MengeInProduktion]=@MengeInProduktion" + i + ","
					+ "[RueckbuchungBestaetigt]=@RueckbuchungBestaetigt" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("GefundeneMengeInProduktion" + i, item.GefundeneMengeInProduktion);
					sqlCommand.Parameters.AddWithValue("InventoryYear" + i, item.InventoryYear == null ? (object)DBNull.Value : item.InventoryYear);
					sqlCommand.Parameters.AddWithValue("Lagerbestand" + i, item.Lagerbestand);
					sqlCommand.Parameters.AddWithValue("LagerId" + i, item.LagerId);
					sqlCommand.Parameters.AddWithValue("MengeInProduktion" + i, item.MengeInProduktion);
					sqlCommand.Parameters.AddWithValue("RueckbuchungBestaetigt" + i, item.RueckbuchungBestaetigt);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Inventory].[Inventory].[ReportTwoTbl] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [Inventory].[ReportTwoTbl] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		static string DATA_SELECT_CLAUSE() { return @"SELECT
							h.[Artikel-Nr] as ArtikelNr,
							Artikelnummer,
							0 GefundeneMengeInProduktion, 
							GETDATE() AS InventoryYear,
							CAST(h.Bestand AS MONEY) Lagerbestand,
							h.Lagerort_id AS LagerId,
							CASE WHEN a.Warentyp = 1 THEN CAST(l.Bestand AS MONEY) ELSE CAST(l.Gesamtbestand AS MONEY) END MengeInProduktion, 
							'N' RueckbuchungBestaetigt
							"; }
		static string DATA_SELECT_CLAUSE01() { return @"SELECT
					h.[Artikel-Nr] as ArtikelNr,
					Artikelnummer,
					0 GefundeneMengeInProduktion, 
					GETDATE() AS InventoryYear,
					 SUM(CAST(h.Bestand AS MONEY)) AS Lagerbestand,
					IIF(h.Lagerort_id= 7,42,h.Lagerort_id) AS LagerId,
					CASE WHEN a.Warentyp = 1 THEN CAST(l.Bestand AS MONEY) ELSE CAST(l.Gesamtbestand AS MONEY) END MengeInProduktion, 
					'N' RueckbuchungBestaetigt
					"; }
		static string DATA_FROM_CLAUSE(string ids, string prodIds) { return $@"FROM Artikel a
							JOIN Lager h on h.[Artikel-Nr] = a.[Artikel-Nr] AND h.Lagerort_id IN ({ids})
							JOIN Lager l on l.[Artikel-Nr] = a.[Artikel-Nr] AND l.Lagerort_id IN {prodIds} AND (l.Bestand > 0 OR l.Gesamtbestand > 0)
							LEFT JOIN (SELECT p.Artikel_Nr FROM Fertigung g JOIN Fertigung_Positionen p on p.ID_Fertigung=g.ID WHERE Kennzeichen='offen' AND ISNULL(FA_Gestartet,0)= 1 AND g.Lagerort_id IN ({ids})) f on f.Artikel_Nr = a.[Artikel-Nr]
							WHERE a.Warengruppe = 'ROH' AND f.Artikel_Nr IS NULL";}
		public static int CountReportTwoData(string? filterSearch, List<int> lagerIds, List<int> prodLagerId)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				var query = $@"SELECT COUNT(*) {String.Format(DATA_FROM_CLAUSE(string.Join(",", lagerIds), $"({string.Join(",", prodLagerId)})"))}";

				if(!string.IsNullOrWhiteSpace(filterSearch) && filterSearch.Length > 1)
				{
					query += @" AND (Artikelnummer LIKE @filterSearch OR CAST(h.Bestand AS NVARCHAR) LIKE @filterSearch )";
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
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity> GetReportTwoData(
		string? filterSearch,
		List<int> lagerIds,
		List<int> prodLagerId,
		Settings.SortingModel dataSorting,
		Settings.PaginModel? dataPaging)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				var query = $@"{DATA_SELECT_CLAUSE()} {String.Format(DATA_FROM_CLAUSE(string.Join(",", lagerIds), $"({string.Join(",", prodLagerId)})"))}";

				// Search filter
				if(!string.IsNullOrWhiteSpace(filterSearch) && filterSearch.Length > 1)
				{
					query += @"
                AND (Artikelnummer LIKE @filterSearch OR CAST(h.Bestand AS NVARCHAR) LIKE @filterSearch)";
				}

				// Sorting
				string sortColumn = "Artikelnummer"; // default
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
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity>();
			}
		}
		public static int DeleteReportTwo()
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "TRUNCATE TABLE  [Inventory].[ReportTwoTbl]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity> GetReportXlsTwoDataWithoutPag(List<int> lagerIds, List<int> prodLagerId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string groupBy = @" and (CASE WHEN a.Warentyp = 1 THEN CAST(l.Bestand AS MONEY) ELSE CAST(l.Gesamtbestand AS MONEY) end) >1  GROUP BY 
					    h.[Artikel-Nr], a.Artikelnummer,a.Warentyp, IIF(h.Lagerort_id= 7,42,h.Lagerort_id),CASE WHEN a.Warentyp = 1 THEN CAST(l.Bestand AS MONEY)         ELSE CAST(l.Gesamtbestand AS MONEY)     END";
				string query = $"{DATA_SELECT_CLAUSE01()} {String.Format(DATA_FROM_CLAUSE(string.Join(",", lagerIds), $"({string.Join(",", prodLagerId)})"))}  {groupBy} ORDER BY a.Warentyp, a.Artikelnummer";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity>();

			}
		}
		public static int InitStartedFaReport(List<int> lagerIds, List<int> prodLagerId, SqlConnection connection, SqlTransaction transaction)
		{
			string query = $@"INSERT INTO [Inventory].[ReportTwoTbl] ([ArtikelNr],[Artikelnummer],[GefundeneMengeInProduktion],[InventoryYear],[Lagerbestand],[LagerId],[MengeInProduktion],[RueckbuchungBestaetigt]) 
								OUTPUT INSERTED.[Id] 
							{DATA_SELECT_CLAUSE()} {String.Format(DATA_FROM_CLAUSE(string.Join(",", lagerIds), $"({string.Join(",", prodLagerId)})"))}
							ORDER BY a.Artikelnummer ";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.CommandTimeout = 180;

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		#endregion Custom Methods

	}
}
