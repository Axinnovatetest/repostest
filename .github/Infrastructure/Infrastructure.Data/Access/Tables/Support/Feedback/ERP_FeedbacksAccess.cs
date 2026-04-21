using Infrastructure.Data.Entities.Tables.Support.Feedback;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.Support.Feedback
{
	public class ERP_FeedbacksAccess
	{
		#region Default Methods
		public static ERP_FeedbacksEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [ERP_Feedbacks] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new ERP_FeedbacksEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<ERP_FeedbacksEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [ERP_Feedbacks]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new ERP_FeedbacksEntity(x)).ToList();
			}
			else
			{
				return new List<ERP_FeedbacksEntity>();
			}
		}
		public static List<ERP_FeedbacksEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<ERP_FeedbacksEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<ERP_FeedbacksEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<ERP_FeedbacksEntity>();
		}
		private static List<ERP_FeedbacksEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [ERP_Feedbacks] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new ERP_FeedbacksEntity(x)).ToList();
				}
				else
				{
					return new List<ERP_FeedbacksEntity>();
				}
			}
			return new List<ERP_FeedbacksEntity>();
		}

		public static int Insert(ERP_FeedbacksEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [ERP_Feedbacks] ([Comment],[CreationDate],[FeedbackType],[Image],[Module],[PageUrl],[priority],[Rating],[Submodule],[Treated],[TreatedDate],[TreatedUser],[UserId],[Username]) OUTPUT INSERTED.[Id] VALUES (@Comment,@CreationDate,@FeedbackType,@Image,@Module,@PageUrl,@priority,@Rating,@Submodule,@Treated,@TreatedDate,@TreatedUser,@UserId,@Username); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Comment", item.Comment == null ? (object)DBNull.Value : item.Comment);
					sqlCommand.Parameters.AddWithValue("CreationDate", item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
					sqlCommand.Parameters.AddWithValue("FeedbackType", item.FeedbackType == null ? (object)DBNull.Value : item.FeedbackType);
					sqlCommand.Parameters.AddWithValue("Image", item.Image == null ? null : item.Image);
					sqlCommand.Parameters.AddWithValue("Module", item.Module == null ? (object)DBNull.Value : item.Module);
					sqlCommand.Parameters.AddWithValue("PageUrl", item.PageUrl == null ? (object)DBNull.Value : item.PageUrl);
					sqlCommand.Parameters.AddWithValue("priority", item.priority == null ? (object)DBNull.Value : item.priority);
					sqlCommand.Parameters.AddWithValue("Rating", item.Rating == null ? (object)DBNull.Value : item.Rating);
					sqlCommand.Parameters.AddWithValue("Submodule", item.Submodule == null ? (object)DBNull.Value : item.Submodule);
					sqlCommand.Parameters.AddWithValue("Treated", item.Treated == null ? (object)DBNull.Value : item.Treated);
					sqlCommand.Parameters.AddWithValue("TreatedDate", item.TreatedDate == null ? (object)DBNull.Value : item.TreatedDate);
					sqlCommand.Parameters.AddWithValue("TreatedUser", item.TreatedUser == null ? (object)DBNull.Value : item.TreatedUser);
					sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
					sqlCommand.Parameters.AddWithValue("Username", item.Username == null ? (object)DBNull.Value : item.Username);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<ERP_FeedbacksEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 15; // Nb params per query
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
		private static int insert(List<ERP_FeedbacksEntity> items)
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
						query += " INSERT INTO [ERP_Feedbacks] ([Comment],[CreationDate],[FeedbackType],[Image],[Module],[PageUrl],[priority],[Rating],[Submodule],[Treated],[TreatedDate],[TreatedUser],[UserId],[Username]) VALUES ( "

							+ "@Comment" + i + ","
							+ "@CreationDate" + i + ","
							+ "@FeedbackType" + i + ","
							+ "@Image" + i + ","
							+ "@Module" + i + ","
							+ "@PageUrl" + i + ","
							+ "@priority" + i + ","
							+ "@Rating" + i + ","
							+ "@Submodule" + i + ","
							+ "@Treated" + i + ","
							+ "@TreatedDate" + i + ","
							+ "@TreatedUser" + i + ","
							+ "@UserId" + i + ","
							+ "@Username" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Comment" + i, item.Comment == null ? (object)DBNull.Value : item.Comment);
						sqlCommand.Parameters.AddWithValue("CreationDate" + i, item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
						sqlCommand.Parameters.AddWithValue("FeedbackType" + i, item.FeedbackType == null ? (object)DBNull.Value : item.FeedbackType);
						sqlCommand.Parameters.AddWithValue("Image" + i, item.Image == null ? null : item.Image);
						sqlCommand.Parameters.AddWithValue("Module" + i, item.Module == null ? (object)DBNull.Value : item.Module);
						sqlCommand.Parameters.AddWithValue("PageUrl" + i, item.PageUrl == null ? (object)DBNull.Value : item.PageUrl);
						sqlCommand.Parameters.AddWithValue("priority" + i, item.priority == null ? (object)DBNull.Value : item.priority);
						sqlCommand.Parameters.AddWithValue("Rating" + i, item.Rating == null ? (object)DBNull.Value : item.Rating);
						sqlCommand.Parameters.AddWithValue("Submodule" + i, item.Submodule == null ? (object)DBNull.Value : item.Submodule);
						sqlCommand.Parameters.AddWithValue("Treated" + i, item.Treated == null ? (object)DBNull.Value : item.Treated);
						sqlCommand.Parameters.AddWithValue("TreatedDate" + i, item.TreatedDate == null ? (object)DBNull.Value : item.TreatedDate);
						sqlCommand.Parameters.AddWithValue("TreatedUser" + i, item.TreatedUser == null ? (object)DBNull.Value : item.TreatedUser);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
						sqlCommand.Parameters.AddWithValue("Username" + i, item.Username == null ? (object)DBNull.Value : item.Username);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(ERP_FeedbacksEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [ERP_Feedbacks] SET [Comment]=@Comment, [CreationDate]=@CreationDate, [FeedbackType]=@FeedbackType, [Image]=@Image, [Module]=@Module, [PageUrl]=@PageUrl, [priority]=@priority, [Rating]=@Rating, [Submodule]=@Submodule, [Treated]=@Treated, [TreatedDate]=@TreatedDate, [TreatedUser]=@TreatedUser, [UserId]=@UserId, [Username]=@Username WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Comment", item.Comment == null ? (object)DBNull.Value : item.Comment);
				sqlCommand.Parameters.AddWithValue("CreationDate", item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
				sqlCommand.Parameters.AddWithValue("FeedbackType", item.FeedbackType == null ? (object)DBNull.Value : item.FeedbackType);
				sqlCommand.Parameters.AddWithValue("Image", item.Image == null ? null : item.Image);
				sqlCommand.Parameters.AddWithValue("Module", item.Module == null ? (object)DBNull.Value : item.Module);
				sqlCommand.Parameters.AddWithValue("PageUrl", item.PageUrl == null ? (object)DBNull.Value : item.PageUrl);
				sqlCommand.Parameters.AddWithValue("priority", item.priority == null ? (object)DBNull.Value : item.priority);
				sqlCommand.Parameters.AddWithValue("Rating", item.Rating == null ? (object)DBNull.Value : item.Rating);
				sqlCommand.Parameters.AddWithValue("Submodule", item.Submodule == null ? (object)DBNull.Value : item.Submodule);
				sqlCommand.Parameters.AddWithValue("Treated", item.Treated == null ? (object)DBNull.Value : item.Treated);
				sqlCommand.Parameters.AddWithValue("TreatedDate", item.TreatedDate == null ? (object)DBNull.Value : item.TreatedDate);
				sqlCommand.Parameters.AddWithValue("TreatedUser", item.TreatedUser == null ? (object)DBNull.Value : item.TreatedUser);
				sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
				sqlCommand.Parameters.AddWithValue("Username", item.Username == null ? (object)DBNull.Value : item.Username);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<ERP_FeedbacksEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 15; // Nb params per query
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
		private static int update(List<ERP_FeedbacksEntity> items)
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
						query += " UPDATE [ERP_Feedbacks] SET "

							+ "[Comment]=@Comment" + i + ","
							+ "[CreationDate]=@CreationDate" + i + ","
							+ "[FeedbackType]=@FeedbackType" + i + ","
							+ "[Image]=@Image" + i + ","
							+ "[Module]=@Module" + i + ","
							+ "[PageUrl]=@PageUrl" + i + ","
							+ "[priority]=@priority" + i + ","
							+ "[Rating]=@Rating" + i + ","
							+ "[Submodule]=@Submodule" + i + ","
							+ "[Treated]=@Treated" + i + ","
							+ "[TreatedDate]=@TreatedDate" + i + ","
							+ "[TreatedUser]=@TreatedUser" + i + ","
							+ "[UserId]=@UserId" + i + ","
							+ "[Username]=@Username" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("Comment" + i, item.Comment == null ? (object)DBNull.Value : item.Comment);
						sqlCommand.Parameters.AddWithValue("CreationDate" + i, item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
						sqlCommand.Parameters.AddWithValue("FeedbackType" + i, item.FeedbackType == null ? (object)DBNull.Value : item.FeedbackType);
						sqlCommand.Parameters.AddWithValue("Image" + i, item.Image == null ? null : item.Image);
						sqlCommand.Parameters.AddWithValue("Module" + i, item.Module == null ? (object)DBNull.Value : item.Module);
						sqlCommand.Parameters.AddWithValue("PageUrl" + i, item.PageUrl == null ? (object)DBNull.Value : item.PageUrl);
						sqlCommand.Parameters.AddWithValue("priority" + i, item.priority == null ? (object)DBNull.Value : item.priority);
						sqlCommand.Parameters.AddWithValue("Rating" + i, item.Rating == null ? (object)DBNull.Value : item.Rating);
						sqlCommand.Parameters.AddWithValue("Submodule" + i, item.Submodule == null ? (object)DBNull.Value : item.Submodule);
						sqlCommand.Parameters.AddWithValue("Treated" + i, item.Treated == null ? (object)DBNull.Value : item.Treated);
						sqlCommand.Parameters.AddWithValue("TreatedDate" + i, item.TreatedDate == null ? (object)DBNull.Value : item.TreatedDate);
						sqlCommand.Parameters.AddWithValue("TreatedUser" + i, item.TreatedUser == null ? (object)DBNull.Value : item.TreatedUser);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
						sqlCommand.Parameters.AddWithValue("Username" + i, item.Username == null ? (object)DBNull.Value : item.Username);
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
				string query = "DELETE FROM [ERP_Feedbacks] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [ERP_Feedbacks] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static ERP_FeedbacksEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [ERP_Feedbacks] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new ERP_FeedbacksEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<ERP_FeedbacksEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [ERP_Feedbacks]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new ERP_FeedbacksEntity(x)).ToList();
			}
			else
			{
				return new List<ERP_FeedbacksEntity>();
			}
		}
		public static List<ERP_FeedbacksEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<ERP_FeedbacksEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<ERP_FeedbacksEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<ERP_FeedbacksEntity>();
		}
		private static List<ERP_FeedbacksEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [ERP_Feedbacks] WHERE [Id] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new ERP_FeedbacksEntity(x)).ToList();
				}
				else
				{
					return new List<ERP_FeedbacksEntity>();
				}
			}
			return new List<ERP_FeedbacksEntity>();
		}

		public static int InsertWithTransaction(ERP_FeedbacksEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [ERP_Feedbacks] ([Comment],[CreationDate],[FeedbackType],[Image],[Module],[PageUrl],[priority],[Rating],[Submodule],[Treated],[TreatedDate],[TreatedUser],[UserId],[Username]) OUTPUT INSERTED.[Id] VALUES (@Comment,@CreationDate,@FeedbackType,@Image,@Module,@PageUrl,@priority,@Rating,@Submodule,@Treated,@TreatedDate,@TreatedUser,@UserId,@Username); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Comment", item.Comment == null ? (object)DBNull.Value : item.Comment);
			sqlCommand.Parameters.AddWithValue("CreationDate", item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
			sqlCommand.Parameters.AddWithValue("FeedbackType", item.FeedbackType == null ? (object)DBNull.Value : item.FeedbackType);
			sqlCommand.Parameters.AddWithValue("Image", item.Image == null ? null : item.Image);
			sqlCommand.Parameters.AddWithValue("Module", item.Module == null ? (object)DBNull.Value : item.Module);
			sqlCommand.Parameters.AddWithValue("PageUrl", item.PageUrl == null ? (object)DBNull.Value : item.PageUrl);
			sqlCommand.Parameters.AddWithValue("priority", item.priority == null ? (object)DBNull.Value : item.priority);
			sqlCommand.Parameters.AddWithValue("Rating", item.Rating == null ? (object)DBNull.Value : item.Rating);
			sqlCommand.Parameters.AddWithValue("Submodule", item.Submodule == null ? (object)DBNull.Value : item.Submodule);
			sqlCommand.Parameters.AddWithValue("Treated", item.Treated == null ? (object)DBNull.Value : item.Treated);
			sqlCommand.Parameters.AddWithValue("TreatedDate", item.TreatedDate == null ? (object)DBNull.Value : item.TreatedDate);
			sqlCommand.Parameters.AddWithValue("TreatedUser", item.TreatedUser == null ? (object)DBNull.Value : item.TreatedUser);
			sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
			sqlCommand.Parameters.AddWithValue("Username", item.Username == null ? (object)DBNull.Value : item.Username);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<ERP_FeedbacksEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 15; // Nb params per query
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
		private static int insertWithTransaction(List<ERP_FeedbacksEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [ERP_Feedbacks] ([Comment],[CreationDate],[FeedbackType],[Image],[Module],[PageUrl],[priority],[Rating],[Submodule],[Treated],[TreatedDate],[TreatedUser],[UserId],[Username]) VALUES ( "

						+ "@Comment" + i + ","
						+ "@CreationDate" + i + ","
						+ "@FeedbackType" + i + ","
						+ "@Image" + i + ","
						+ "@Module" + i + ","
						+ "@PageUrl" + i + ","
						+ "@priority" + i + ","
						+ "@Rating" + i + ","
						+ "@Submodule" + i + ","
						+ "@Treated" + i + ","
						+ "@TreatedDate" + i + ","
						+ "@TreatedUser" + i + ","
						+ "@UserId" + i + ","
						+ "@Username" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Comment" + i, item.Comment == null ? (object)DBNull.Value : item.Comment);
					sqlCommand.Parameters.AddWithValue("CreationDate" + i, item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
					sqlCommand.Parameters.AddWithValue("FeedbackType" + i, item.FeedbackType == null ? (object)DBNull.Value : item.FeedbackType);
					sqlCommand.Parameters.AddWithValue("Image" + i, item.Image == null ? null : item.Image);
					sqlCommand.Parameters.AddWithValue("Module" + i, item.Module == null ? (object)DBNull.Value : item.Module);
					sqlCommand.Parameters.AddWithValue("PageUrl" + i, item.PageUrl == null ? (object)DBNull.Value : item.PageUrl);
					sqlCommand.Parameters.AddWithValue("priority" + i, item.priority == null ? (object)DBNull.Value : item.priority);
					sqlCommand.Parameters.AddWithValue("Rating" + i, item.Rating == null ? (object)DBNull.Value : item.Rating);
					sqlCommand.Parameters.AddWithValue("Submodule" + i, item.Submodule == null ? (object)DBNull.Value : item.Submodule);
					sqlCommand.Parameters.AddWithValue("Treated" + i, item.Treated == null ? (object)DBNull.Value : item.Treated);
					sqlCommand.Parameters.AddWithValue("TreatedDate" + i, item.TreatedDate == null ? (object)DBNull.Value : item.TreatedDate);
					sqlCommand.Parameters.AddWithValue("TreatedUser" + i, item.TreatedUser == null ? (object)DBNull.Value : item.TreatedUser);
					sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
					sqlCommand.Parameters.AddWithValue("Username" + i, item.Username == null ? (object)DBNull.Value : item.Username);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(ERP_FeedbacksEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [ERP_Feedbacks] SET [Comment]=@Comment, [CreationDate]=@CreationDate, [FeedbackType]=@FeedbackType, [Image]=@Image, [Module]=@Module, [PageUrl]=@PageUrl, [priority]=@priority, [Rating]=@Rating, [Submodule]=@Submodule, [Treated]=@Treated, [TreatedDate]=@TreatedDate, [TreatedUser]=@TreatedUser, [UserId]=@UserId, [Username]=@Username WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("Comment", item.Comment == null ? (object)DBNull.Value : item.Comment);
			sqlCommand.Parameters.AddWithValue("CreationDate", item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
			sqlCommand.Parameters.AddWithValue("FeedbackType", item.FeedbackType == null ? (object)DBNull.Value : item.FeedbackType);
			sqlCommand.Parameters.AddWithValue("Image", item.Image == null ? null : item.Image);
			sqlCommand.Parameters.AddWithValue("Module", item.Module == null ? (object)DBNull.Value : item.Module);
			sqlCommand.Parameters.AddWithValue("PageUrl", item.PageUrl == null ? (object)DBNull.Value : item.PageUrl);
			sqlCommand.Parameters.AddWithValue("priority", item.priority == null ? (object)DBNull.Value : item.priority);
			sqlCommand.Parameters.AddWithValue("Rating", item.Rating == null ? (object)DBNull.Value : item.Rating);
			sqlCommand.Parameters.AddWithValue("Submodule", item.Submodule == null ? (object)DBNull.Value : item.Submodule);
			sqlCommand.Parameters.AddWithValue("Treated", item.Treated == null ? (object)DBNull.Value : item.Treated);
			sqlCommand.Parameters.AddWithValue("TreatedDate", item.TreatedDate == null ? (object)DBNull.Value : item.TreatedDate);
			sqlCommand.Parameters.AddWithValue("TreatedUser", item.TreatedUser == null ? (object)DBNull.Value : item.TreatedUser);
			sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
			sqlCommand.Parameters.AddWithValue("Username", item.Username == null ? (object)DBNull.Value : item.Username);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<ERP_FeedbacksEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 15; // Nb params per query
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
		private static int updateWithTransaction(List<ERP_FeedbacksEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [ERP_Feedbacks] SET "

					+ "[Comment]=@Comment" + i + ","
					+ "[CreationDate]=@CreationDate" + i + ","
					+ "[FeedbackType]=@FeedbackType" + i + ","
					+ "[Image]=@Image" + i + ","
					+ "[Module]=@Module" + i + ","
					+ "[PageUrl]=@PageUrl" + i + ","
					+ "[priority]=@priority" + i + ","
					+ "[Rating]=@Rating" + i + ","
					+ "[Submodule]=@Submodule" + i + ","
					+ "[Treated]=@Treated" + i + ","
					+ "[TreatedDate]=@TreatedDate" + i + ","
					+ "[TreatedUser]=@TreatedUser" + i + ","
					+ "[UserId]=@UserId" + i + ","
					+ "[Username]=@Username" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("Comment" + i, item.Comment == null ? (object)DBNull.Value : item.Comment);
					sqlCommand.Parameters.AddWithValue("CreationDate" + i, item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
					sqlCommand.Parameters.AddWithValue("FeedbackType" + i, item.FeedbackType == null ? (object)DBNull.Value : item.FeedbackType);
					sqlCommand.Parameters.AddWithValue("Image" + i, item.Image == null ? null : item.Image);
					sqlCommand.Parameters.AddWithValue("Module" + i, item.Module == null ? (object)DBNull.Value : item.Module);
					sqlCommand.Parameters.AddWithValue("PageUrl" + i, item.PageUrl == null ? (object)DBNull.Value : item.PageUrl);
					sqlCommand.Parameters.AddWithValue("priority" + i, item.priority == null ? (object)DBNull.Value : item.priority);
					sqlCommand.Parameters.AddWithValue("Rating" + i, item.Rating == null ? (object)DBNull.Value : item.Rating);
					sqlCommand.Parameters.AddWithValue("Submodule" + i, item.Submodule == null ? (object)DBNull.Value : item.Submodule);
					sqlCommand.Parameters.AddWithValue("Treated" + i, item.Treated == null ? (object)DBNull.Value : item.Treated);
					sqlCommand.Parameters.AddWithValue("TreatedDate" + i, item.TreatedDate == null ? (object)DBNull.Value : item.TreatedDate);
					sqlCommand.Parameters.AddWithValue("TreatedUser" + i, item.TreatedUser == null ? (object)DBNull.Value : item.TreatedUser);
					sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
					sqlCommand.Parameters.AddWithValue("Username" + i, item.Username == null ? (object)DBNull.Value : item.Username);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [ERP_Feedbacks] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [ERP_Feedbacks] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods



		#region Custom Methods
		public static List<ERP_FeedbacksEntity> GetBySearchValue(string filterValue, string module, string subModule, string feedbackType, string priority, Settings.PaginModel paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [ERP_Feedbacks] ";
				var clauses = new List<string>();

				if(paging.RequestRows <= 0)
				{
					paging.RequestRows = 1;
				}

				if(!string.IsNullOrWhiteSpace(filterValue))
				{
					clauses.Add($" [FeedbackType] like '%{filterValue}%' or [Module] like '%{filterValue}%' or [Submodule] like '%{filterValue}%' or [Comment] like '%{filterValue}%' ");
				}
				if(!string.IsNullOrWhiteSpace(module))
				{
					clauses.Add($" [Module]='{module}' ");
				}
				if(!string.IsNullOrWhiteSpace(subModule))
				{
					clauses.Add($" [Submodule]='{subModule}' ");
				}
				if(!string.IsNullOrWhiteSpace(feedbackType))
				{
					clauses.Add($" [FeedbackType]='{feedbackType}' ");
				}
				if(!string.IsNullOrWhiteSpace(priority))
				{
					clauses.Add($" [priority]='{priority}' ");
				}

				if(clauses.Count > 0)
				{
					query += $" WHERE {string.Join(" AND ", clauses)}";
				}

				query += " Order By [CreationDate] desc";

				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new ERP_FeedbacksEntity(x)).ToList();
			}
			else
			{
				return new List<ERP_FeedbacksEntity>();
			}
		}
		public static int GetBySearchValue_Count(string filterValue, string module, string subModule, string feedbackType, string priority)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT COUNT(*) As Nb FROM [ERP_Feedbacks]";

				var clauses = new List<string>();
				if(!string.IsNullOrWhiteSpace(filterValue))
				{
					clauses.Add($" [FeedbackType] like '%{filterValue}%' or [Module] like '%{filterValue}%' or [Submodule] like '%{filterValue}%' or [Comment] like '%{filterValue}%' ");
				}
				if(!string.IsNullOrWhiteSpace(module))
				{
					clauses.Add($" [Module]='{module}' ");
				}
				if(!string.IsNullOrWhiteSpace(subModule))
				{
					clauses.Add($" [Submodule]='{subModule}' ");
				}
				if(!string.IsNullOrWhiteSpace(feedbackType))
				{
					clauses.Add($" [FeedbackType]='{feedbackType}' ");
				}
				if(!string.IsNullOrWhiteSpace(priority))
				{
					clauses.Add($" [priority]='{priority}' ");
				}

				if(clauses.Count > 0)
				{
					query += $" WHERE {string.Join(" AND ", clauses)}";
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);

				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var val) ? val : 0;
			}
		}

		public static List<ERP_FeedbacksEntity> GetByPageLink(string pageLink, Settings.SortingModel sorting, Settings.PaginModel paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [ERP_Feedbacks] ";
				var clauses = new List<string>();
				if(paging.RequestRows <= 0)
				{
					paging.RequestRows = 1;
				}

				if(!string.IsNullOrWhiteSpace(pageLink))
				{
					clauses.Add($"[PageUrl] = '{pageLink}'");
				}
				if(clauses.Count > 0)
				{
					query += $" WHERE {string.Join(" AND ", clauses)}";
				}
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY [CreationDate]";
				}

				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new ERP_FeedbacksEntity(x)).ToList();
			}
			else
			{
				return new List<ERP_FeedbacksEntity>();
			}
		}
		public static int GetByPageLink_Count(string pageLink)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT COUNT(*) As Nb FROM [ERP_Feedbacks]";

				var clauses = new List<string>();


				if(!string.IsNullOrWhiteSpace(pageLink))
				{
					clauses.Add($" [PageUrl]='{pageLink}'");
				}


				if(clauses.Count > 0)
				{
					query += $" WHERE {string.Join(" AND ", clauses)}";
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);

				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var val) ? val : 0;
			}
		}

		#endregion
	}
}
