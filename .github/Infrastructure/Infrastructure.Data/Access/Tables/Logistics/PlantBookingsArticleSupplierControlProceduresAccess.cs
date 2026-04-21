using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Infrastructure.Data.Entities.Tables.Logistics;

namespace Infrastructure.Data.Access.Tables.Logistics
{
	public class PlantBookingsArticleSupplierControlProceduresAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__LGT_PlantBookingsArticleSupplierControlProcedures] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__LGT_PlantBookingsArticleSupplierControlProcedures]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__LGT_PlantBookingsArticleSupplierControlProcedures] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__LGT_PlantBookingsArticleSupplierControlProcedures] ([ArticleId],[ArticleNumber],[ControlledAverage],[ControlledFailedQuantity],[ControlledMeasuredValue],[ControlledQuantity],[ControlledSum],[ControlledTotalQuantity],[CreateTime],[CreateUserId],[LastEditTime],[LastEditUserId],[ProcedureDescription],[ProcedureName],[ProcedureType],[SupplierId],[SupplierName]) OUTPUT INSERTED.[Id] VALUES (@ArticleId,@ArticleNumber,@ControlledAverage,@ControlledFailedQuantity,@ControlledMeasuredValue,@ControlledQuantity,@ControlledSum,@ControlledTotalQuantity,@CreateTime,@CreateUserId,@LastEditTime,@LastEditUserId,@ProcedureDescription,@ProcedureName,@ProcedureType,@SupplierId,@SupplierName); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
					sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("ControlledAverage", item.ControlledAverage == null ? (object)DBNull.Value : item.ControlledAverage);
					sqlCommand.Parameters.AddWithValue("ControlledFailedQuantity", item.ControlledFailedQuantity == null ? (object)DBNull.Value : item.ControlledFailedQuantity);
					sqlCommand.Parameters.AddWithValue("ControlledMeasuredValue", item.ControlledMeasuredValue == null ? (object)DBNull.Value : item.ControlledMeasuredValue);
					sqlCommand.Parameters.AddWithValue("ControlledQuantity", item.ControlledQuantity == null ? (object)DBNull.Value : item.ControlledQuantity);
					sqlCommand.Parameters.AddWithValue("ControlledSum", item.ControlledSum == null ? (object)DBNull.Value : item.ControlledSum);
					sqlCommand.Parameters.AddWithValue("ControlledTotalQuantity", item.ControlledTotalQuantity == null ? (object)DBNull.Value : item.ControlledTotalQuantity);
					sqlCommand.Parameters.AddWithValue("CreateTime", item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
					sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
					sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
					sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("ProcedureDescription", item.ProcedureDescription == null ? (object)DBNull.Value : item.ProcedureDescription);
					sqlCommand.Parameters.AddWithValue("ProcedureName", item.ProcedureName == null ? (object)DBNull.Value : item.ProcedureName);
					sqlCommand.Parameters.AddWithValue("ProcedureType", item.ProcedureType == null ? (object)DBNull.Value : item.ProcedureType);
					sqlCommand.Parameters.AddWithValue("SupplierId", item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
					sqlCommand.Parameters.AddWithValue("SupplierName", item.SupplierName == null ? (object)DBNull.Value : item.SupplierName);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 18; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity> items)
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
						query += " INSERT INTO [__LGT_PlantBookingsArticleSupplierControlProcedures] ([ArticleId],[ArticleNumber],[ControlledAverage],[ControlledFailedQuantity],[ControlledMeasuredValue],[ControlledQuantity],[ControlledSum],[ControlledTotalQuantity],[CreateTime],[CreateUserId],[LastEditTime],[LastEditUserId],[ProcedureDescription],[ProcedureName],[ProcedureType],[SupplierId],[SupplierName]) VALUES ( "

							+ "@ArticleId" + i + ","
							+ "@ArticleNumber" + i + ","
							+ "@ControlledAverage" + i + ","
							+ "@ControlledFailedQuantity" + i + ","
							+ "@ControlledMeasuredValue" + i + ","
							+ "@ControlledQuantity" + i + ","
							+ "@ControlledSum" + i + ","
							+ "@ControlledTotalQuantity" + i + ","
							+ "@CreateTime" + i + ","
							+ "@CreateUserId" + i + ","
							+ "@LastEditTime" + i + ","
							+ "@LastEditUserId" + i + ","
							+ "@ProcedureDescription" + i + ","
							+ "@ProcedureName" + i + ","
							+ "@ProcedureType" + i + ","
							+ "@SupplierId" + i + ","
							+ "@SupplierName" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
						sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
						sqlCommand.Parameters.AddWithValue("ControlledAverage" + i, item.ControlledAverage == null ? (object)DBNull.Value : item.ControlledAverage);
						sqlCommand.Parameters.AddWithValue("ControlledFailedQuantity" + i, item.ControlledFailedQuantity == null ? (object)DBNull.Value : item.ControlledFailedQuantity);
						sqlCommand.Parameters.AddWithValue("ControlledMeasuredValue" + i, item.ControlledMeasuredValue == null ? (object)DBNull.Value : item.ControlledMeasuredValue);
						sqlCommand.Parameters.AddWithValue("ControlledQuantity" + i, item.ControlledQuantity == null ? (object)DBNull.Value : item.ControlledQuantity);
						sqlCommand.Parameters.AddWithValue("ControlledSum" + i, item.ControlledSum == null ? (object)DBNull.Value : item.ControlledSum);
						sqlCommand.Parameters.AddWithValue("ControlledTotalQuantity" + i, item.ControlledTotalQuantity == null ? (object)DBNull.Value : item.ControlledTotalQuantity);
						sqlCommand.Parameters.AddWithValue("CreateTime" + i, item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
						sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
						sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
						sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
						sqlCommand.Parameters.AddWithValue("ProcedureDescription" + i, item.ProcedureDescription == null ? (object)DBNull.Value : item.ProcedureDescription);
						sqlCommand.Parameters.AddWithValue("ProcedureName" + i, item.ProcedureName == null ? (object)DBNull.Value : item.ProcedureName);
						sqlCommand.Parameters.AddWithValue("ProcedureType" + i, item.ProcedureType == null ? (object)DBNull.Value : item.ProcedureType);
						sqlCommand.Parameters.AddWithValue("SupplierId" + i, item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
						sqlCommand.Parameters.AddWithValue("SupplierName" + i, item.SupplierName == null ? (object)DBNull.Value : item.SupplierName);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__LGT_PlantBookingsArticleSupplierControlProcedures] SET [ArticleId]=@ArticleId, [ArticleNumber]=@ArticleNumber, [ControlledAverage]=@ControlledAverage, [ControlledFailedQuantity]=@ControlledFailedQuantity, [ControlledMeasuredValue]=@ControlledMeasuredValue, [ControlledQuantity]=@ControlledQuantity, [ControlledSum]=@ControlledSum, [ControlledTotalQuantity]=@ControlledTotalQuantity, [CreateTime]=@CreateTime, [CreateUserId]=@CreateUserId, [LastEditTime]=@LastEditTime, [LastEditUserId]=@LastEditUserId, [ProcedureDescription]=@ProcedureDescription, [ProcedureName]=@ProcedureName, [ProcedureType]=@ProcedureType, [SupplierId]=@SupplierId, [SupplierName]=@SupplierName WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
				sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
				sqlCommand.Parameters.AddWithValue("ControlledAverage", item.ControlledAverage == null ? (object)DBNull.Value : item.ControlledAverage);
				sqlCommand.Parameters.AddWithValue("ControlledFailedQuantity", item.ControlledFailedQuantity == null ? (object)DBNull.Value : item.ControlledFailedQuantity);
				sqlCommand.Parameters.AddWithValue("ControlledMeasuredValue", item.ControlledMeasuredValue == null ? (object)DBNull.Value : item.ControlledMeasuredValue);
				sqlCommand.Parameters.AddWithValue("ControlledQuantity", item.ControlledQuantity == null ? (object)DBNull.Value : item.ControlledQuantity);
				sqlCommand.Parameters.AddWithValue("ControlledSum", item.ControlledSum == null ? (object)DBNull.Value : item.ControlledSum);
				sqlCommand.Parameters.AddWithValue("ControlledTotalQuantity", item.ControlledTotalQuantity == null ? (object)DBNull.Value : item.ControlledTotalQuantity);
				sqlCommand.Parameters.AddWithValue("CreateTime", item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
				sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
				sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
				sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
				sqlCommand.Parameters.AddWithValue("ProcedureDescription", item.ProcedureDescription == null ? (object)DBNull.Value : item.ProcedureDescription);
				sqlCommand.Parameters.AddWithValue("ProcedureName", item.ProcedureName == null ? (object)DBNull.Value : item.ProcedureName);
				sqlCommand.Parameters.AddWithValue("ProcedureType", item.ProcedureType == null ? (object)DBNull.Value : item.ProcedureType);
				sqlCommand.Parameters.AddWithValue("SupplierId", item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
				sqlCommand.Parameters.AddWithValue("SupplierName", item.SupplierName == null ? (object)DBNull.Value : item.SupplierName);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 18; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity> items)
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
						query += " UPDATE [__LGT_PlantBookingsArticleSupplierControlProcedures] SET "

							+ "[ArticleId]=@ArticleId" + i + ","
							+ "[ArticleNumber]=@ArticleNumber" + i + ","
							+ "[ControlledAverage]=@ControlledAverage" + i + ","
							+ "[ControlledFailedQuantity]=@ControlledFailedQuantity" + i + ","
							+ "[ControlledMeasuredValue]=@ControlledMeasuredValue" + i + ","
							+ "[ControlledQuantity]=@ControlledQuantity" + i + ","
							+ "[ControlledSum]=@ControlledSum" + i + ","
							+ "[ControlledTotalQuantity]=@ControlledTotalQuantity" + i + ","
							+ "[CreateTime]=@CreateTime" + i + ","
							+ "[CreateUserId]=@CreateUserId" + i + ","
							+ "[LastEditTime]=@LastEditTime" + i + ","
							+ "[LastEditUserId]=@LastEditUserId" + i + ","
							+ "[ProcedureDescription]=@ProcedureDescription" + i + ","
							+ "[ProcedureName]=@ProcedureName" + i + ","
							+ "[ProcedureType]=@ProcedureType" + i + ","
							+ "[SupplierId]=@SupplierId" + i + ","
							+ "[SupplierName]=@SupplierName" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
						sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
						sqlCommand.Parameters.AddWithValue("ControlledAverage" + i, item.ControlledAverage == null ? (object)DBNull.Value : item.ControlledAverage);
						sqlCommand.Parameters.AddWithValue("ControlledFailedQuantity" + i, item.ControlledFailedQuantity == null ? (object)DBNull.Value : item.ControlledFailedQuantity);
						sqlCommand.Parameters.AddWithValue("ControlledMeasuredValue" + i, item.ControlledMeasuredValue == null ? (object)DBNull.Value : item.ControlledMeasuredValue);
						sqlCommand.Parameters.AddWithValue("ControlledQuantity" + i, item.ControlledQuantity == null ? (object)DBNull.Value : item.ControlledQuantity);
						sqlCommand.Parameters.AddWithValue("ControlledSum" + i, item.ControlledSum == null ? (object)DBNull.Value : item.ControlledSum);
						sqlCommand.Parameters.AddWithValue("ControlledTotalQuantity" + i, item.ControlledTotalQuantity == null ? (object)DBNull.Value : item.ControlledTotalQuantity);
						sqlCommand.Parameters.AddWithValue("CreateTime" + i, item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
						sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
						sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
						sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
						sqlCommand.Parameters.AddWithValue("ProcedureDescription" + i, item.ProcedureDescription == null ? (object)DBNull.Value : item.ProcedureDescription);
						sqlCommand.Parameters.AddWithValue("ProcedureName" + i, item.ProcedureName == null ? (object)DBNull.Value : item.ProcedureName);
						sqlCommand.Parameters.AddWithValue("ProcedureType" + i, item.ProcedureType == null ? (object)DBNull.Value : item.ProcedureType);
						sqlCommand.Parameters.AddWithValue("SupplierId" + i, item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
						sqlCommand.Parameters.AddWithValue("SupplierName" + i, item.SupplierName == null ? (object)DBNull.Value : item.SupplierName);
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
				string query = "DELETE FROM [__LGT_PlantBookingsArticleSupplierControlProcedures] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

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

					string query = "DELETE FROM [__LGT_PlantBookingsArticleSupplierControlProcedures] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__LGT_PlantBookingsArticleSupplierControlProcedures] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__LGT_PlantBookingsArticleSupplierControlProcedures]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__LGT_PlantBookingsArticleSupplierControlProcedures] WHERE [Id] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [__LGT_PlantBookingsArticleSupplierControlProcedures] ([ArticleId],[ArticleNumber],[ControlledAverage],[ControlledFailedQuantity],[ControlledMeasuredValue],[ControlledQuantity],[ControlledSum],[ControlledTotalQuantity],[CreateTime],[CreateUserId],[LastEditTime],[LastEditUserId],[ProcedureDescription],[ProcedureName],[ProcedureType],[SupplierId],[SupplierName]) OUTPUT INSERTED.[Id] VALUES (@ArticleId,@ArticleNumber,@ControlledAverage,@ControlledFailedQuantity,@ControlledMeasuredValue,@ControlledQuantity,@ControlledSum,@ControlledTotalQuantity,@CreateTime,@CreateUserId,@LastEditTime,@LastEditUserId,@ProcedureDescription,@ProcedureName,@ProcedureType,@SupplierId,@SupplierName); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
			sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
			sqlCommand.Parameters.AddWithValue("ControlledAverage", item.ControlledAverage == null ? (object)DBNull.Value : item.ControlledAverage);
			sqlCommand.Parameters.AddWithValue("ControlledFailedQuantity", item.ControlledFailedQuantity == null ? (object)DBNull.Value : item.ControlledFailedQuantity);
			sqlCommand.Parameters.AddWithValue("ControlledMeasuredValue", item.ControlledMeasuredValue == null ? (object)DBNull.Value : item.ControlledMeasuredValue);
			sqlCommand.Parameters.AddWithValue("ControlledQuantity", item.ControlledQuantity == null ? (object)DBNull.Value : item.ControlledQuantity);
			sqlCommand.Parameters.AddWithValue("ControlledSum", item.ControlledSum == null ? (object)DBNull.Value : item.ControlledSum);
			sqlCommand.Parameters.AddWithValue("ControlledTotalQuantity", item.ControlledTotalQuantity == null ? (object)DBNull.Value : item.ControlledTotalQuantity);
			sqlCommand.Parameters.AddWithValue("CreateTime", item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
			sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
			sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
			sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
			sqlCommand.Parameters.AddWithValue("ProcedureDescription", item.ProcedureDescription == null ? (object)DBNull.Value : item.ProcedureDescription);
			sqlCommand.Parameters.AddWithValue("ProcedureName", item.ProcedureName == null ? (object)DBNull.Value : item.ProcedureName);
			sqlCommand.Parameters.AddWithValue("ProcedureType", item.ProcedureType == null ? (object)DBNull.Value : item.ProcedureType);
			sqlCommand.Parameters.AddWithValue("SupplierId", item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
			sqlCommand.Parameters.AddWithValue("SupplierName", item.SupplierName == null ? (object)DBNull.Value : item.SupplierName);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 18; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__LGT_PlantBookingsArticleSupplierControlProcedures] ([ArticleId],[ArticleNumber],[ControlledAverage],[ControlledFailedQuantity],[ControlledMeasuredValue],[ControlledQuantity],[ControlledSum],[ControlledTotalQuantity],[CreateTime],[CreateUserId],[LastEditTime],[LastEditUserId],[ProcedureDescription],[ProcedureName],[ProcedureType],[SupplierId],[SupplierName]) VALUES ( "

						+ "@ArticleId" + i + ","
						+ "@ArticleNumber" + i + ","
						+ "@ControlledAverage" + i + ","
						+ "@ControlledFailedQuantity" + i + ","
						+ "@ControlledMeasuredValue" + i + ","
						+ "@ControlledQuantity" + i + ","
						+ "@ControlledSum" + i + ","
						+ "@ControlledTotalQuantity" + i + ","
						+ "@CreateTime" + i + ","
						+ "@CreateUserId" + i + ","
						+ "@LastEditTime" + i + ","
						+ "@LastEditUserId" + i + ","
						+ "@ProcedureDescription" + i + ","
						+ "@ProcedureName" + i + ","
						+ "@ProcedureType" + i + ","
						+ "@SupplierId" + i + ","
						+ "@SupplierName" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
					sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("ControlledAverage" + i, item.ControlledAverage == null ? (object)DBNull.Value : item.ControlledAverage);
					sqlCommand.Parameters.AddWithValue("ControlledFailedQuantity" + i, item.ControlledFailedQuantity == null ? (object)DBNull.Value : item.ControlledFailedQuantity);
					sqlCommand.Parameters.AddWithValue("ControlledMeasuredValue" + i, item.ControlledMeasuredValue == null ? (object)DBNull.Value : item.ControlledMeasuredValue);
					sqlCommand.Parameters.AddWithValue("ControlledQuantity" + i, item.ControlledQuantity == null ? (object)DBNull.Value : item.ControlledQuantity);
					sqlCommand.Parameters.AddWithValue("ControlledSum" + i, item.ControlledSum == null ? (object)DBNull.Value : item.ControlledSum);
					sqlCommand.Parameters.AddWithValue("ControlledTotalQuantity" + i, item.ControlledTotalQuantity == null ? (object)DBNull.Value : item.ControlledTotalQuantity);
					sqlCommand.Parameters.AddWithValue("CreateTime" + i, item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
					sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
					sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
					sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("ProcedureDescription" + i, item.ProcedureDescription == null ? (object)DBNull.Value : item.ProcedureDescription);
					sqlCommand.Parameters.AddWithValue("ProcedureName" + i, item.ProcedureName == null ? (object)DBNull.Value : item.ProcedureName);
					sqlCommand.Parameters.AddWithValue("ProcedureType" + i, item.ProcedureType == null ? (object)DBNull.Value : item.ProcedureType);
					sqlCommand.Parameters.AddWithValue("SupplierId" + i, item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
					sqlCommand.Parameters.AddWithValue("SupplierName" + i, item.SupplierName == null ? (object)DBNull.Value : item.SupplierName);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__LGT_PlantBookingsArticleSupplierControlProcedures] SET [ArticleId]=@ArticleId, [ArticleNumber]=@ArticleNumber, [ControlledAverage]=@ControlledAverage, [ControlledFailedQuantity]=@ControlledFailedQuantity, [ControlledMeasuredValue]=@ControlledMeasuredValue, [ControlledQuantity]=@ControlledQuantity, [ControlledSum]=@ControlledSum, [ControlledTotalQuantity]=@ControlledTotalQuantity, [CreateTime]=@CreateTime, [CreateUserId]=@CreateUserId, [LastEditTime]=@LastEditTime, [LastEditUserId]=@LastEditUserId, [ProcedureDescription]=@ProcedureDescription, [ProcedureName]=@ProcedureName, [ProcedureType]=@ProcedureType, [SupplierId]=@SupplierId, [SupplierName]=@SupplierName WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
			sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
			sqlCommand.Parameters.AddWithValue("ControlledAverage", item.ControlledAverage == null ? (object)DBNull.Value : item.ControlledAverage);
			sqlCommand.Parameters.AddWithValue("ControlledFailedQuantity", item.ControlledFailedQuantity == null ? (object)DBNull.Value : item.ControlledFailedQuantity);
			sqlCommand.Parameters.AddWithValue("ControlledMeasuredValue", item.ControlledMeasuredValue == null ? (object)DBNull.Value : item.ControlledMeasuredValue);
			sqlCommand.Parameters.AddWithValue("ControlledQuantity", item.ControlledQuantity == null ? (object)DBNull.Value : item.ControlledQuantity);
			sqlCommand.Parameters.AddWithValue("ControlledSum", item.ControlledSum == null ? (object)DBNull.Value : item.ControlledSum);
			sqlCommand.Parameters.AddWithValue("ControlledTotalQuantity", item.ControlledTotalQuantity == null ? (object)DBNull.Value : item.ControlledTotalQuantity);
			sqlCommand.Parameters.AddWithValue("CreateTime", item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
			sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
			sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
			sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
			sqlCommand.Parameters.AddWithValue("ProcedureDescription", item.ProcedureDescription == null ? (object)DBNull.Value : item.ProcedureDescription);
			sqlCommand.Parameters.AddWithValue("ProcedureName", item.ProcedureName == null ? (object)DBNull.Value : item.ProcedureName);
			sqlCommand.Parameters.AddWithValue("ProcedureType", item.ProcedureType == null ? (object)DBNull.Value : item.ProcedureType);
			sqlCommand.Parameters.AddWithValue("SupplierId", item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
			sqlCommand.Parameters.AddWithValue("SupplierName", item.SupplierName == null ? (object)DBNull.Value : item.SupplierName);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 18; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__LGT_PlantBookingsArticleSupplierControlProcedures] SET "

					+ "[ArticleId]=@ArticleId" + i + ","
					+ "[ArticleNumber]=@ArticleNumber" + i + ","
					+ "[ControlledAverage]=@ControlledAverage" + i + ","
					+ "[ControlledFailedQuantity]=@ControlledFailedQuantity" + i + ","
					+ "[ControlledMeasuredValue]=@ControlledMeasuredValue" + i + ","
					+ "[ControlledQuantity]=@ControlledQuantity" + i + ","
					+ "[ControlledSum]=@ControlledSum" + i + ","
					+ "[ControlledTotalQuantity]=@ControlledTotalQuantity" + i + ","
					+ "[CreateTime]=@CreateTime" + i + ","
					+ "[CreateUserId]=@CreateUserId" + i + ","
					+ "[LastEditTime]=@LastEditTime" + i + ","
					+ "[LastEditUserId]=@LastEditUserId" + i + ","
					+ "[ProcedureDescription]=@ProcedureDescription" + i + ","
					+ "[ProcedureName]=@ProcedureName" + i + ","
					+ "[ProcedureType]=@ProcedureType" + i + ","
					+ "[SupplierId]=@SupplierId" + i + ","
					+ "[SupplierName]=@SupplierName" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
					sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("ControlledAverage" + i, item.ControlledAverage == null ? (object)DBNull.Value : item.ControlledAverage);
					sqlCommand.Parameters.AddWithValue("ControlledFailedQuantity" + i, item.ControlledFailedQuantity == null ? (object)DBNull.Value : item.ControlledFailedQuantity);
					sqlCommand.Parameters.AddWithValue("ControlledMeasuredValue" + i, item.ControlledMeasuredValue == null ? (object)DBNull.Value : item.ControlledMeasuredValue);
					sqlCommand.Parameters.AddWithValue("ControlledQuantity" + i, item.ControlledQuantity == null ? (object)DBNull.Value : item.ControlledQuantity);
					sqlCommand.Parameters.AddWithValue("ControlledSum" + i, item.ControlledSum == null ? (object)DBNull.Value : item.ControlledSum);
					sqlCommand.Parameters.AddWithValue("ControlledTotalQuantity" + i, item.ControlledTotalQuantity == null ? (object)DBNull.Value : item.ControlledTotalQuantity);
					sqlCommand.Parameters.AddWithValue("CreateTime" + i, item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
					sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
					sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
					sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("ProcedureDescription" + i, item.ProcedureDescription == null ? (object)DBNull.Value : item.ProcedureDescription);
					sqlCommand.Parameters.AddWithValue("ProcedureName" + i, item.ProcedureName == null ? (object)DBNull.Value : item.ProcedureName);
					sqlCommand.Parameters.AddWithValue("ProcedureType" + i, item.ProcedureType == null ? (object)DBNull.Value : item.ProcedureType);
					sqlCommand.Parameters.AddWithValue("SupplierId" + i, item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
					sqlCommand.Parameters.AddWithValue("SupplierName" + i, item.SupplierName == null ? (object)DBNull.Value : item.SupplierName);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__LGT_PlantBookingsArticleSupplierControlProcedures] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

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

				string query = "DELETE FROM [__LGT_PlantBookingsArticleSupplierControlProcedures] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity> GetDataArtikleProcedure(string searchTerms, Settings.SortingModel dataSorting, Settings.PaginModel dataPaging)
		{
			string paging = "";
			string sorting = "";

			if(dataPaging != null && (0 >= dataPaging.RequestRows || dataPaging.RequestRows > 100))
			{
				dataPaging.RequestRows = 100;
			}

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__LGT_PlantBookingsArticleSupplierControlProcedures]";

				if(!string.IsNullOrWhiteSpace(searchTerms))
				{
					query += $" WHERE [ArticleNumber] LIKE '%{searchTerms.SqlEscape()}%'";
				}
				if(dataSorting != null && !string.IsNullOrWhiteSpace(dataSorting.SortFieldName))
				{
					query += $" ORDER BY {dataSorting.SortFieldName} {(dataSorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else

				{
					query += " ORDER BY [CreateTime] DESC";
				}
				if(paging is not null)
				{
					query += $" OFFSET {dataPaging.FirstRowNumber} ROWS FETCH NEXT {dataPaging.RequestRows} ROWS ONLY ";
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresEntity>();

			}
		}

		public static int CountArticleProcedureData(string searchTerms, Settings.SortingModel dataSorting, Settings.PaginModel dataPaging)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{

				sqlConnection.Open();
				string query = @$"SELECT Count(*) FROM  [__LGT_PlantBookingsArticleSupplierControlProcedures]";
				if(!string.IsNullOrWhiteSpace(searchTerms))
				{
					query += $" WHERE [ArticleNumber] LIKE '%{searchTerms.SqlEscape()}%'";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 300;

				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var val) ? val : 0;
			}
		}


		public static int UpdateArticleControlProcedureTrans(Infrastructure.Data.Entities.Tables.Logistics.UpdateArticelControlProcedureEntity item, SqlConnection connection, SqlTransaction transaction)

		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__LGT_PlantBookingsArticleSupplierControlProcedures] SET [ControlledAverage]=@ControlledAverage, [ControlledFailedQuantity]=@ControlledFailedQuantity, [ControlledMeasuredValue]=@ControlledMeasuredValue, [ControlledQuantity]=@ControlledQuantity, [ControlledSum]=@ControlledSum, [ControlledTotalQuantity]=@ControlledTotalQuantity,  [LastEditTime]=@LastEditTime, [LastEditUserId]=@LastEditUserId, [ProcedureDescription]=@ProcedureDescription, [ProcedureName]=@ProcedureName, [ProcedureType]=@ProcedureType WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
				sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
				sqlCommand.Parameters.AddWithValue("ProcedureDescription", item.ProcedureDescription == null ? (object)DBNull.Value : item.ProcedureDescription);
				sqlCommand.Parameters.AddWithValue("ProcedureName", item.ProcedureName == null ? (object)DBNull.Value : item.ProcedureName);
				sqlCommand.Parameters.AddWithValue("ControlledAverage", item.ControlledAverage == null ? (object)DBNull.Value : item.ControlledAverage);
				sqlCommand.Parameters.AddWithValue("ControlledFailedQuantity", item.ControlledFailedQuantity == null ? (object)DBNull.Value : item.ControlledFailedQuantity);
				sqlCommand.Parameters.AddWithValue("ControlledMeasuredValue", item.ControlledMeasuredValue == null ? (object)DBNull.Value : item.ControlledMeasuredValue);
				sqlCommand.Parameters.AddWithValue("ControlledQuantity", item.ControlledQuantity == null ? (object)DBNull.Value : item.ControlledQuantity);
				sqlCommand.Parameters.AddWithValue("ControlledSum", item.ControlledSum == null ? (object)DBNull.Value : item.ControlledSum);
				sqlCommand.Parameters.AddWithValue("ControlledTotalQuantity", item.ControlledTotalQuantity == null ? (object)DBNull.Value : item.ControlledTotalQuantity);
				sqlCommand.Parameters.AddWithValue("ProcedureType", item.ProcedureType == null ? (object)DBNull.Value : item.ProcedureType);


				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}


		public static List<string> GetProcudreDescription(string ProcedureDescription,string ArtikelNummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "";

				if(ProcedureDescription is not null && !string.IsNullOrWhiteSpace(ProcedureDescription) && ProcedureDescription.Length>0)
				{
					query = @$"
								SELECT top 10 ProcedureDescription FROM [__LGT_PlantBookingsArticleSupplierControlProcedures]
								WHERE ProcedureDescription is not null 
								and [ArticleNumber]='{ArtikelNummer}'
								AND ProcedureDescription like '{ProcedureDescription.SqlEscape()}%'";
				}
				else
				{
					query = @$"
								SELECT top 10 ProcedureDescription FROM [__LGT_PlantBookingsArticleSupplierControlProcedures]
								WHERE ProcedureDescription is not null 
								and [ArticleNumber]='{ArtikelNummer}'
								order by ProcedureDescription ";
				}
				 

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select( x=> (x["ProcedureDescription"] == System.DBNull.Value) ? "" : Convert.ToString(x["ProcedureDescription"])).ToList();
			}
			else
			{
				return new List<string>();
			}
		}
		public static List<string> GetProcedureName(string procedureName, string ArtikelNummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "";

				if(procedureName is not null && !string.IsNullOrWhiteSpace(procedureName) && procedureName.Length > 0)
				{
					query = @$"
								SELECT top 10 ProcedureName FROM [__LGT_PlantBookingsArticleSupplierControlProcedures]
								WHERE ProcedureName is not null 
								and [ArticleNumber]='{ArtikelNummer}'
								AND ProcedureName like '{procedureName.SqlEscape()}%'";
				}
				else
				{
					query = @$"
								SELECT top 10 ProcedureName FROM [__LGT_PlantBookingsArticleSupplierControlProcedures]
								WHERE ProcedureName is not null 
								and [ArticleNumber]='{ArtikelNummer}'
								order by ProcedureName ";
				}


				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => (x["ProcedureName"] == System.DBNull.Value) ? "" : Convert.ToString(x["ProcedureName"])).ToList();
			}
			else
			{
				return new List<string>();
			}
		}

		#endregion Custom Methods

	}
}
