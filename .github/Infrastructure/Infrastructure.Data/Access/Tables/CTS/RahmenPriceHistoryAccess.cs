using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class RahmenPriceHistoryAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_RahmenPriceHistory] WHERE [id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_RahmenPriceHistory]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__CTS_RahmenPriceHistory] WHERE [id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__CTS_RahmenPriceHistory] ([BasePrice],[DateUpdate],[PositionNr],[Price],[PriceDefault],[RahmenNr],[UserName],[ValidFrom],[WarungSymbol]) OUTPUT INSERTED.[id] VALUES (@BasePrice,@DateUpdate,@PositionNr,@Price,@PriceDefault,@RahmenNr,@UserName,@ValidFrom,@WarungSymbol); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("BasePrice", item.BasePrice == null ? (object)DBNull.Value : item.BasePrice);
					sqlCommand.Parameters.AddWithValue("DateUpdate", item.DateUpdate == null ? (object)DBNull.Value : item.DateUpdate);
					sqlCommand.Parameters.AddWithValue("PositionNr", item.PositionNr);
					sqlCommand.Parameters.AddWithValue("Price", item.Price == null ? (object)DBNull.Value : item.Price);
					sqlCommand.Parameters.AddWithValue("PriceDefault", item.PriceDefault == null ? (object)DBNull.Value : item.PriceDefault);
					sqlCommand.Parameters.AddWithValue("RahmenNr", item.RahmenNr);
					sqlCommand.Parameters.AddWithValue("UserName", item.UserName == null ? (object)DBNull.Value : item.UserName);
					sqlCommand.Parameters.AddWithValue("ValidFrom", item.ValidFrom == null ? (object)DBNull.Value : item.ValidFrom);
					sqlCommand.Parameters.AddWithValue("WarungSymbol", item.WarungSymbol == null ? (object)DBNull.Value : item.WarungSymbol);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity> items)
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
						query += " INSERT INTO [__CTS_RahmenPriceHistory] ([BasePrice],[DateUpdate],[PositionNr],[Price],[PriceDefault],[RahmenNr],[UserName],[ValidFrom],[WarungSymbol]) VALUES ( "

							+ "@BasePrice" + i + ","
							+ "@DateUpdate" + i + ","
							+ "@PositionNr" + i + ","
							+ "@Price" + i + ","
							+ "@PriceDefault" + i + ","
							+ "@RahmenNr" + i + ","
							+ "@UserName" + i + ","
							+ "@ValidFrom" + i + ","
							+ "@WarungSymbol" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("BasePrice" + i, item.BasePrice == null ? (object)DBNull.Value : item.BasePrice);
						sqlCommand.Parameters.AddWithValue("DateUpdate" + i, item.DateUpdate == null ? (object)DBNull.Value : item.DateUpdate);
						sqlCommand.Parameters.AddWithValue("PositionNr" + i, item.PositionNr);
						sqlCommand.Parameters.AddWithValue("Price" + i, item.Price == null ? (object)DBNull.Value : item.Price);
						sqlCommand.Parameters.AddWithValue("PriceDefault" + i, item.PriceDefault == null ? (object)DBNull.Value : item.PriceDefault);
						sqlCommand.Parameters.AddWithValue("RahmenNr" + i, item.RahmenNr);
						sqlCommand.Parameters.AddWithValue("UserName" + i, item.UserName == null ? (object)DBNull.Value : item.UserName);
						sqlCommand.Parameters.AddWithValue("ValidFrom" + i, item.ValidFrom == null ? (object)DBNull.Value : item.ValidFrom);
						sqlCommand.Parameters.AddWithValue("WarungSymbol" + i, item.WarungSymbol == null ? (object)DBNull.Value : item.WarungSymbol);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__CTS_RahmenPriceHistory] SET [BasePrice]=@BasePrice, [DateUpdate]=@DateUpdate, [PositionNr]=@PositionNr, [Price]=@Price, [PriceDefault]=@PriceDefault, [RahmenNr]=@RahmenNr, [UserName]=@UserName, [ValidFrom]=@ValidFrom, [WarungSymbol]=@WarungSymbol WHERE [id]=@id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("id", item.id);
				sqlCommand.Parameters.AddWithValue("BasePrice", item.BasePrice == null ? (object)DBNull.Value : item.BasePrice);
				sqlCommand.Parameters.AddWithValue("DateUpdate", item.DateUpdate == null ? (object)DBNull.Value : item.DateUpdate);
				sqlCommand.Parameters.AddWithValue("PositionNr", item.PositionNr);
				sqlCommand.Parameters.AddWithValue("Price", item.Price == null ? (object)DBNull.Value : item.Price);
				sqlCommand.Parameters.AddWithValue("PriceDefault", item.PriceDefault == null ? (object)DBNull.Value : item.PriceDefault);
				sqlCommand.Parameters.AddWithValue("RahmenNr", item.RahmenNr);
				sqlCommand.Parameters.AddWithValue("UserName", item.UserName == null ? (object)DBNull.Value : item.UserName);
				sqlCommand.Parameters.AddWithValue("ValidFrom", item.ValidFrom == null ? (object)DBNull.Value : item.ValidFrom);
				sqlCommand.Parameters.AddWithValue("WarungSymbol", item.WarungSymbol == null ? (object)DBNull.Value : item.WarungSymbol);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity> items)
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
						query += " UPDATE [__CTS_RahmenPriceHistory] SET "

							+ "[BasePrice]=@BasePrice" + i + ","
							+ "[DateUpdate]=@DateUpdate" + i + ","
							+ "[PositionNr]=@PositionNr" + i + ","
							+ "[Price]=@Price" + i + ","
							+ "[PriceDefault]=@PriceDefault" + i + ","
							+ "[RahmenNr]=@RahmenNr" + i + ","
							+ "[UserName]=@UserName" + i + ","
							+ "[ValidFrom]=@ValidFrom" + i + ","
							+ "[WarungSymbol]=@WarungSymbol" + i + " WHERE [id]=@id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("id" + i, item.id);
						sqlCommand.Parameters.AddWithValue("BasePrice" + i, item.BasePrice == null ? (object)DBNull.Value : item.BasePrice);
						sqlCommand.Parameters.AddWithValue("DateUpdate" + i, item.DateUpdate == null ? (object)DBNull.Value : item.DateUpdate);
						sqlCommand.Parameters.AddWithValue("PositionNr" + i, item.PositionNr);
						sqlCommand.Parameters.AddWithValue("Price" + i, item.Price == null ? (object)DBNull.Value : item.Price);
						sqlCommand.Parameters.AddWithValue("PriceDefault" + i, item.PriceDefault == null ? (object)DBNull.Value : item.PriceDefault);
						sqlCommand.Parameters.AddWithValue("RahmenNr" + i, item.RahmenNr);
						sqlCommand.Parameters.AddWithValue("UserName" + i, item.UserName == null ? (object)DBNull.Value : item.UserName);
						sqlCommand.Parameters.AddWithValue("ValidFrom" + i, item.ValidFrom == null ? (object)DBNull.Value : item.ValidFrom);
						sqlCommand.Parameters.AddWithValue("WarungSymbol" + i, item.WarungSymbol == null ? (object)DBNull.Value : item.WarungSymbol);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
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
				string query = "DELETE FROM [__CTS_RahmenPriceHistory] WHERE [id]=@id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);

				results = sqlCommand.ExecuteNonQuery();
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

					string query = "DELETE FROM [__CTS_RahmenPriceHistory] WHERE [id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__CTS_RahmenPriceHistory] WHERE [id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__CTS_RahmenPriceHistory]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__CTS_RahmenPriceHistory] WHERE [id] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [__CTS_RahmenPriceHistory] ([BasePrice],[DateUpdate],[PositionNr],[Price],[PriceDefault],[RahmenNr],[UserName],[ValidFrom],[WarungSymbol]) OUTPUT INSERTED.[id] VALUES (@BasePrice,@DateUpdate,@PositionNr,@Price,@PriceDefault,@RahmenNr,@UserName,@ValidFrom,@WarungSymbol); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("BasePrice", item.BasePrice == null ? (object)DBNull.Value : item.BasePrice);
			sqlCommand.Parameters.AddWithValue("DateUpdate", item.DateUpdate == null ? (object)DBNull.Value : item.DateUpdate);
			sqlCommand.Parameters.AddWithValue("PositionNr", item.PositionNr);
			sqlCommand.Parameters.AddWithValue("Price", item.Price == null ? (object)DBNull.Value : item.Price);
			sqlCommand.Parameters.AddWithValue("PriceDefault", item.PriceDefault == null ? (object)DBNull.Value : item.PriceDefault);
			sqlCommand.Parameters.AddWithValue("RahmenNr", item.RahmenNr);
			sqlCommand.Parameters.AddWithValue("UserName", item.UserName == null ? (object)DBNull.Value : item.UserName);
			sqlCommand.Parameters.AddWithValue("ValidFrom", item.ValidFrom == null ? (object)DBNull.Value : item.ValidFrom);
			sqlCommand.Parameters.AddWithValue("WarungSymbol", item.WarungSymbol == null ? (object)DBNull.Value : item.WarungSymbol);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__CTS_RahmenPriceHistory] ([BasePrice],[DateUpdate],[PositionNr],[Price],[PriceDefault],[RahmenNr],[UserName],[ValidFrom],[WarungSymbol]) VALUES ( "

						+ "@BasePrice" + i + ","
						+ "@DateUpdate" + i + ","
						+ "@PositionNr" + i + ","
						+ "@Price" + i + ","
						+ "@PriceDefault" + i + ","
						+ "@RahmenNr" + i + ","
						+ "@UserName" + i + ","
						+ "@ValidFrom" + i + ","
						+ "@WarungSymbol" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("BasePrice" + i, item.BasePrice == null ? (object)DBNull.Value : item.BasePrice);
					sqlCommand.Parameters.AddWithValue("DateUpdate" + i, item.DateUpdate == null ? (object)DBNull.Value : item.DateUpdate);
					sqlCommand.Parameters.AddWithValue("PositionNr" + i, item.PositionNr);
					sqlCommand.Parameters.AddWithValue("Price" + i, item.Price == null ? (object)DBNull.Value : item.Price);
					sqlCommand.Parameters.AddWithValue("PriceDefault" + i, item.PriceDefault == null ? (object)DBNull.Value : item.PriceDefault);
					sqlCommand.Parameters.AddWithValue("RahmenNr" + i, item.RahmenNr);
					sqlCommand.Parameters.AddWithValue("UserName" + i, item.UserName == null ? (object)DBNull.Value : item.UserName);
					sqlCommand.Parameters.AddWithValue("ValidFrom" + i, item.ValidFrom == null ? (object)DBNull.Value : item.ValidFrom);
					sqlCommand.Parameters.AddWithValue("WarungSymbol" + i, item.WarungSymbol == null ? (object)DBNull.Value : item.WarungSymbol);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__CTS_RahmenPriceHistory] SET [BasePrice]=@BasePrice, [DateUpdate]=@DateUpdate, [PositionNr]=@PositionNr, [Price]=@Price, [PriceDefault]=@PriceDefault, [RahmenNr]=@RahmenNr, [UserName]=@UserName, [ValidFrom]=@ValidFrom, [WarungSymbol]=@WarungSymbol WHERE [id]=@id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("id", item.id);
			sqlCommand.Parameters.AddWithValue("BasePrice", item.BasePrice == null ? (object)DBNull.Value : item.BasePrice);
			sqlCommand.Parameters.AddWithValue("DateUpdate", item.DateUpdate == null ? (object)DBNull.Value : item.DateUpdate);
			sqlCommand.Parameters.AddWithValue("PositionNr", item.PositionNr);
			sqlCommand.Parameters.AddWithValue("Price", item.Price == null ? (object)DBNull.Value : item.Price);
			sqlCommand.Parameters.AddWithValue("PriceDefault", item.PriceDefault == null ? (object)DBNull.Value : item.PriceDefault);
			sqlCommand.Parameters.AddWithValue("RahmenNr", item.RahmenNr);
			sqlCommand.Parameters.AddWithValue("UserName", item.UserName == null ? (object)DBNull.Value : item.UserName);
			sqlCommand.Parameters.AddWithValue("ValidFrom", item.ValidFrom == null ? (object)DBNull.Value : item.ValidFrom);
			sqlCommand.Parameters.AddWithValue("WarungSymbol", item.WarungSymbol == null ? (object)DBNull.Value : item.WarungSymbol);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__CTS_RahmenPriceHistory] SET "

					+ "[BasePrice]=@BasePrice" + i + ","
					+ "[DateUpdate]=@DateUpdate" + i + ","
					+ "[PositionNr]=@PositionNr" + i + ","
					+ "[Price]=@Price" + i + ","
					+ "[PriceDefault]=@PriceDefault" + i + ","
					+ "[RahmenNr]=@RahmenNr" + i + ","
					+ "[UserName]=@UserName" + i + ","
					+ "[ValidFrom]=@ValidFrom" + i + ","
					+ "[WarungSymbol]=@WarungSymbol" + i + " WHERE [id]=@id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("id" + i, item.id);
					sqlCommand.Parameters.AddWithValue("BasePrice" + i, item.BasePrice == null ? (object)DBNull.Value : item.BasePrice);
					sqlCommand.Parameters.AddWithValue("DateUpdate" + i, item.DateUpdate == null ? (object)DBNull.Value : item.DateUpdate);
					sqlCommand.Parameters.AddWithValue("PositionNr" + i, item.PositionNr);
					sqlCommand.Parameters.AddWithValue("Price" + i, item.Price == null ? (object)DBNull.Value : item.Price);
					sqlCommand.Parameters.AddWithValue("PriceDefault" + i, item.PriceDefault == null ? (object)DBNull.Value : item.PriceDefault);
					sqlCommand.Parameters.AddWithValue("RahmenNr" + i, item.RahmenNr);
					sqlCommand.Parameters.AddWithValue("UserName" + i, item.UserName == null ? (object)DBNull.Value : item.UserName);
					sqlCommand.Parameters.AddWithValue("ValidFrom" + i, item.ValidFrom == null ? (object)DBNull.Value : item.ValidFrom);
					sqlCommand.Parameters.AddWithValue("WarungSymbol" + i, item.WarungSymbol == null ? (object)DBNull.Value : item.WarungSymbol);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__CTS_RahmenPriceHistory] WHERE [id]=@id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("id", id);

			results = sqlCommand.ExecuteNonQuery();


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

				string query = "DELETE FROM [__CTS_RahmenPriceHistory] WHERE [id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity> GetByPosition(int position)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_RahmenPriceHistory] WHERE [PositionNr]=@position";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("position", position);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity> GetByPositions(List<int> ids)
		{
			if(ids == null || ids.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__CTS_RahmenPriceHistory] WHERE [PositionNr] IN ({string.Join(",", ids)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity> GetByPositionPriceAndDate(int position, decimal? price, DateTime? date)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_RahmenPriceHistory] WHERE [PositionNr]=@position AND [Price]=@price AND [ValidFrom]=@date";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("position", position);
				sqlCommand.Parameters.AddWithValue("price", price);
				sqlCommand.Parameters.AddWithValue("date", date);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity> GetByPositionPriceAndDate(int position, decimal? price, DateTime? date, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			string query = "SELECT * FROM [__CTS_RahmenPriceHistory] WHERE [PositionNr]=@position AND [Price]=@price AND [ValidFrom]=@date";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("position", position);
				sqlCommand.Parameters.AddWithValue("price", price);
				sqlCommand.Parameters.AddWithValue("date", date);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity> GetByMaxPriceAndDate(int position, DateTime? date)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT * FROM [__CTS_RahmenPriceHistory] WHERE [PositionNr]=@position
                               AND [ValidFrom]<= @date AND [DateUpdate] = (SELECT MAX([DateUpdate]) FROM [__CTS_RahmenPriceHistory] WHERE [PositionNr] = @position AND [ValidFrom]<= @date)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("position", position);
				sqlCommand.Parameters.AddWithValue("date", date);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static int DeleteByPosition(int position)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__CTS_RahmenPriceHistory] WHERE [PositionNr]=@position";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("position", position);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int DeleteByPosition(int position, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			string query = "DELETE FROM [__CTS_RahmenPriceHistory] WHERE [PositionNr]=@position";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("position", position);
				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}

		public static int DeleteByPositions(List<int> positions)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"DELETE FROM [__CTS_RahmenPriceHistory] WHERE [PositionNr] IN ({string.Join(",", positions)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int DeleteByPositionsWithTransaction(List<int> positions, SqlConnection connection, SqlTransaction transaction)
		{

			if(positions != null && positions.Count > 0)
			{
				int results = -1;

				var sqlCommand = new SqlCommand("", connection, transaction);

				string queryIds = string.Empty;
				for(int i = 0; i < positions.Count; i++)
				{
					queryIds += "@PositionNr" + i + ",";
					sqlCommand.Parameters.AddWithValue("PositionNr" + i, positions[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				string query = "DELETE FROM [__CTS_RahmenPriceHistory] WHERE [PositionNr] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Custom Methods

	}
}
