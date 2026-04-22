using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests
{
	public class OfferToArticleEKAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [OfferToArticleEK] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [OfferToArticleEK]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [OfferToArticleEK] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [OfferToArticleEK] ([ArtikelNr],[BestellnummernNr],[LastUpdate],[OfferId],[RequestUI],[SupplierId]) OUTPUT INSERTED.[Id] VALUES (@ArtikelNr,@BestellnummernNr,@LastUpdate,@OfferId,@RequestUI,@SupplierId); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("BestellnummernNr", item.BestellnummernNr == null ? (object)DBNull.Value : item.BestellnummernNr);
					sqlCommand.Parameters.AddWithValue("LastUpdate", item.LastUpdate == null ? (object)DBNull.Value : item.LastUpdate);
					sqlCommand.Parameters.AddWithValue("OfferId", item.OfferId == null ? (object)DBNull.Value : item.OfferId);
					sqlCommand.Parameters.AddWithValue("RequestUI", item.RequestUI == null ? (object)DBNull.Value : item.RequestUI);
					sqlCommand.Parameters.AddWithValue("SupplierId", item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity> items)
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
						query += " INSERT INTO [OfferToArticleEK] ([ArtikelNr],[BestellnummernNr],[LastUpdate],[OfferId],[RequestUI],[SupplierId]) VALUES ( "

							+ "@ArtikelNr" + i + ","
							+ "@BestellnummernNr" + i + ","
							+ "@LastUpdate" + i + ","
							+ "@OfferId" + i + ","
							+ "@RequestUI" + i + ","
							+ "@SupplierId" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
						sqlCommand.Parameters.AddWithValue("BestellnummernNr" + i, item.BestellnummernNr == null ? (object)DBNull.Value : item.BestellnummernNr);
						sqlCommand.Parameters.AddWithValue("LastUpdate" + i, item.LastUpdate == null ? (object)DBNull.Value : item.LastUpdate);
						sqlCommand.Parameters.AddWithValue("OfferId" + i, item.OfferId == null ? (object)DBNull.Value : item.OfferId);
						sqlCommand.Parameters.AddWithValue("RequestUI" + i, item.RequestUI == null ? (object)DBNull.Value : item.RequestUI);
						sqlCommand.Parameters.AddWithValue("SupplierId" + i, item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [OfferToArticleEK] SET [ArtikelNr]=@ArtikelNr, [BestellnummernNr]=@BestellnummernNr, [LastUpdate]=@LastUpdate, [OfferId]=@OfferId, [RequestUI]=@RequestUI, [SupplierId]=@SupplierId WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
				sqlCommand.Parameters.AddWithValue("BestellnummernNr", item.BestellnummernNr == null ? (object)DBNull.Value : item.BestellnummernNr);
				sqlCommand.Parameters.AddWithValue("LastUpdate", item.LastUpdate == null ? (object)DBNull.Value : item.LastUpdate);
				sqlCommand.Parameters.AddWithValue("OfferId", item.OfferId == null ? (object)DBNull.Value : item.OfferId);
				sqlCommand.Parameters.AddWithValue("RequestUI", item.RequestUI == null ? (object)DBNull.Value : item.RequestUI);
				sqlCommand.Parameters.AddWithValue("SupplierId", item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity> items)
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
						query += " UPDATE [OfferToArticleEK] SET "

							+ "[ArtikelNr]=@ArtikelNr" + i + ","
							+ "[BestellnummernNr]=@BestellnummernNr" + i + ","
							+ "[LastUpdate]=@LastUpdate" + i + ","
							+ "[OfferId]=@OfferId" + i + ","
							+ "[RequestUI]=@RequestUI" + i + ","
							+ "[SupplierId]=@SupplierId" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
						sqlCommand.Parameters.AddWithValue("BestellnummernNr" + i, item.BestellnummernNr == null ? (object)DBNull.Value : item.BestellnummernNr);
						sqlCommand.Parameters.AddWithValue("LastUpdate" + i, item.LastUpdate == null ? (object)DBNull.Value : item.LastUpdate);
						sqlCommand.Parameters.AddWithValue("OfferId" + i, item.OfferId == null ? (object)DBNull.Value : item.OfferId);
						sqlCommand.Parameters.AddWithValue("RequestUI" + i, item.RequestUI == null ? (object)DBNull.Value : item.RequestUI);
						sqlCommand.Parameters.AddWithValue("SupplierId" + i, item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
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
				string query = "DELETE FROM [OfferToArticleEK] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [OfferToArticleEK] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [OfferToArticleEK] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [OfferToArticleEK]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [OfferToArticleEK] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [OfferToArticleEK] ([ArtikelNr],[BestellnummernNr],[LastUpdate],[OfferId],[RequestUI],[SupplierId]) OUTPUT INSERTED.[Id] VALUES (@ArtikelNr,@BestellnummernNr,@LastUpdate,@OfferId,@RequestUI,@SupplierId); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
			sqlCommand.Parameters.AddWithValue("BestellnummernNr", item.BestellnummernNr == null ? (object)DBNull.Value : item.BestellnummernNr);
			sqlCommand.Parameters.AddWithValue("LastUpdate", item.LastUpdate == null ? (object)DBNull.Value : item.LastUpdate);
			sqlCommand.Parameters.AddWithValue("OfferId", item.OfferId == null ? (object)DBNull.Value : item.OfferId);
			sqlCommand.Parameters.AddWithValue("RequestUI", item.RequestUI == null ? (object)DBNull.Value : item.RequestUI);
			sqlCommand.Parameters.AddWithValue("SupplierId", item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [OfferToArticleEK] ([ArtikelNr],[BestellnummernNr],[LastUpdate],[OfferId],[RequestUI],[SupplierId]) VALUES ( "

						+ "@ArtikelNr" + i + ","
						+ "@BestellnummernNr" + i + ","
						+ "@LastUpdate" + i + ","
						+ "@OfferId" + i + ","
						+ "@RequestUI" + i + ","
						+ "@SupplierId" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("BestellnummernNr" + i, item.BestellnummernNr == null ? (object)DBNull.Value : item.BestellnummernNr);
					sqlCommand.Parameters.AddWithValue("LastUpdate" + i, item.LastUpdate == null ? (object)DBNull.Value : item.LastUpdate);
					sqlCommand.Parameters.AddWithValue("OfferId" + i, item.OfferId == null ? (object)DBNull.Value : item.OfferId);
					sqlCommand.Parameters.AddWithValue("RequestUI" + i, item.RequestUI == null ? (object)DBNull.Value : item.RequestUI);
					sqlCommand.Parameters.AddWithValue("SupplierId" + i, item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [OfferToArticleEK] SET [ArtikelNr]=@ArtikelNr, [BestellnummernNr]=@BestellnummernNr, [LastUpdate]=@LastUpdate, [OfferId]=@OfferId, [RequestUI]=@RequestUI, [SupplierId]=@SupplierId WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
			sqlCommand.Parameters.AddWithValue("BestellnummernNr", item.BestellnummernNr == null ? (object)DBNull.Value : item.BestellnummernNr);
			sqlCommand.Parameters.AddWithValue("LastUpdate", item.LastUpdate == null ? (object)DBNull.Value : item.LastUpdate);
			sqlCommand.Parameters.AddWithValue("OfferId", item.OfferId == null ? (object)DBNull.Value : item.OfferId);
			sqlCommand.Parameters.AddWithValue("RequestUI", item.RequestUI == null ? (object)DBNull.Value : item.RequestUI);
			sqlCommand.Parameters.AddWithValue("SupplierId", item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [OfferToArticleEK] SET "

					+ "[ArtikelNr]=@ArtikelNr" + i + ","
					+ "[BestellnummernNr]=@BestellnummernNr" + i + ","
					+ "[LastUpdate]=@LastUpdate" + i + ","
					+ "[OfferId]=@OfferId" + i + ","
					+ "[RequestUI]=@RequestUI" + i + ","
					+ "[SupplierId]=@SupplierId" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("BestellnummernNr" + i, item.BestellnummernNr == null ? (object)DBNull.Value : item.BestellnummernNr);
					sqlCommand.Parameters.AddWithValue("LastUpdate" + i, item.LastUpdate == null ? (object)DBNull.Value : item.LastUpdate);
					sqlCommand.Parameters.AddWithValue("OfferId" + i, item.OfferId == null ? (object)DBNull.Value : item.OfferId);
					sqlCommand.Parameters.AddWithValue("RequestUI" + i, item.RequestUI == null ? (object)DBNull.Value : item.RequestUI);
					sqlCommand.Parameters.AddWithValue("SupplierId" + i, item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [OfferToArticleEK] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [OfferToArticleEK] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity> GetByArtikelNr(int? ArtikelNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [OfferToArticleEK] Where ArtikelNr=@ArtikelNr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ArtikelNr", ArtikelNr == null ? (object)DBNull.Value : ArtikelNr);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity>();
			}
		}
		public static Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity GetByOfferID(int? OfferId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [OfferToArticleEK] Where OfferId=@OfferId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("OfferId", OfferId == null ? (object)DBNull.Value : OfferId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferToArticleEKEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods

		#endregion Custom Methods

	}
}
