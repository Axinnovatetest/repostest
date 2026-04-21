using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class Budget_JointFile_OrderAccess
	{

		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity Get(int id_file)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_OrderFile] WHERE [Id_File]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id_file);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_OrderFile]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
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

					sqlCommand.CommandText = $"SELECT * FROM [__FNC_OrderFile] WHERE [Id_File] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__FNC_OrderFile] ([Action_File],[File_date],[FileId],[Id_Order],[Id_Order_Version],[Id_User])  VALUES (@Action_File,@File_date,@FileId,@Id_Order,@Id_Order_Version,@Id_User)";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Action_File", item.Action_File == null ? (object)DBNull.Value : item.Action_File);
					sqlCommand.Parameters.AddWithValue("File_date", item.File_date == null ? (object)DBNull.Value : item.File_date);
					sqlCommand.Parameters.AddWithValue("FileId", item.FileId);
					sqlCommand.Parameters.AddWithValue("Id_Order", item.Id_Order == null ? (object)DBNull.Value : item.Id_Order);
					sqlCommand.Parameters.AddWithValue("Id_Order_Version", item.Id_Order_Version);
					sqlCommand.Parameters.AddWithValue("Id_User", item.Id_User);

					sqlCommand.ExecuteNonQuery();
				}

				using(var sqlCommand = new SqlCommand("SELECT [Id_File] FROM [__FNC_OrderFile] WHERE [Id_File] = @@IDENTITY", sqlConnection, sqlTransaction))
				{
					response = sqlCommand.ExecuteScalar() == null ? int.MinValue : int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var insertedId) ? insertedId : int.MinValue;
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [__FNC_OrderFile] ([Action_File],[File_date],[FileId],[Id_Order],[Id_Order_Version],[Id_User]) VALUES ( "

							+ "@Action_File" + i + ","
							+ "@File_date" + i + ","
							+ "@FileId" + i + ","
							+ "@Id_Order" + i + ","
							+ "@Id_Order_Version" + i + ","
							+ "@Id_User" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Action_File" + i, item.Action_File == null ? (object)DBNull.Value : item.Action_File);
						sqlCommand.Parameters.AddWithValue("File_date" + i, item.File_date == null ? (object)DBNull.Value : item.File_date);
						sqlCommand.Parameters.AddWithValue("FileId" + i, item.FileId);
						sqlCommand.Parameters.AddWithValue("Id_Order" + i, item.Id_Order == null ? (object)DBNull.Value : item.Id_Order);
						sqlCommand.Parameters.AddWithValue("Id_Order_Version" + i, item.Id_Order_Version);
						sqlCommand.Parameters.AddWithValue("Id_User" + i, item.Id_User);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_OrderFile] SET [Action_File]=@Action_File, [File_date]=@File_date, [FileId]=@FileId, [Id_Order]=@Id_Order, [Id_Order_Version]=@Id_Order_Version, [Id_User]=@Id_User WHERE [Id_File]=@Id_File";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id_File", item.Id_File);
				sqlCommand.Parameters.AddWithValue("Action_File", item.Action_File == null ? (object)DBNull.Value : item.Action_File);
				sqlCommand.Parameters.AddWithValue("File_date", item.File_date == null ? (object)DBNull.Value : item.File_date);
				sqlCommand.Parameters.AddWithValue("FileId", item.FileId);
				sqlCommand.Parameters.AddWithValue("Id_Order", item.Id_Order == null ? (object)DBNull.Value : item.Id_Order);
				sqlCommand.Parameters.AddWithValue("Id_Order_Version", item.Id_Order_Version);
				sqlCommand.Parameters.AddWithValue("Id_User", item.Id_User);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [__FNC_OrderFile] SET "

							+ "[Action_File]=@Action_File" + i + ","
							+ "[File_date]=@File_date" + i + ","
							+ "[FileId]=@FileId" + i + ","
							+ "[Id_Order]=@Id_Order" + i + ","
							+ "[Id_Order_Version]=@Id_Order_Version" + i + ","
							+ "[Id_User]=@Id_User" + i + " WHERE [Id_File]=@Id_File" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id_File" + i, item.Id_File);
						sqlCommand.Parameters.AddWithValue("Action_File" + i, item.Action_File == null ? (object)DBNull.Value : item.Action_File);
						sqlCommand.Parameters.AddWithValue("File_date" + i, item.File_date == null ? (object)DBNull.Value : item.File_date);
						sqlCommand.Parameters.AddWithValue("FileId" + i, item.FileId);
						sqlCommand.Parameters.AddWithValue("Id_Order" + i, item.Id_Order == null ? (object)DBNull.Value : item.Id_Order);
						sqlCommand.Parameters.AddWithValue("Id_Order_Version" + i, item.Id_Order_Version);
						sqlCommand.Parameters.AddWithValue("Id_User" + i, item.Id_User);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int id_file)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__FNC_OrderFile] WHERE [Id_File]=@Id_File";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id_File", id_file);

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
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
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

					string query = "DELETE FROM [__FNC_OrderFile] WHERE [Id_File] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity> GetByIdOrder(int id_Order)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_OrderFile] WHERE [Id_Order]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id_Order);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity> GetByIdsOrder(List<int> ids_Order)
		{
			if(ids_Order == null || ids_Order.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_OrderFile] WHERE [Id_Order] IN ({string.Join(",", ids_Order)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}

		public static int DeleteByOrderIdwExceptIds(int orderId, List<int> orderIds)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"DELETE FROM [__FNC_OrderFile] WHERE [Id_Order]=@id {(orderIds != null && orderIds.Count > 0 ? $" AND [FileId] NOT IN ({string.Join(", ", orderIds)})" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", orderId);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}

		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.Budget_JointFile_OrderEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
