using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.LGT
{
	public class ArticleCustomsNumberCheckAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [__LGT_ArticleCustomsNumberCheck] WHERE [Id]=@Id", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [__LGT_ArticleCustomsNumberCheck]", sqlConnection))
			{
				sqlConnection.Open();
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				using(var sqlCommand = new SqlCommand("", sqlConnection))
				{
					sqlConnection.Open();

					var queryIds = new List<string>();
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds.Add($"@Id{i}");
						sqlCommand.Parameters.AddWithValue($"Id{i}", ids[i]);
					}

					sqlCommand.CommandText = $"SELECT * FROM [__LGT_ArticleCustomsNumberCheck] WHERE [Id] IN ({string.Join(",", queryIds)})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity>();

		}

		public static int Insert(Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__LGT_ArticleCustomsNumberCheck] ([ArticlesTotalCount],[ArticlesWithoutNumberCount],[ArticlesWithWrongNumberCount],[CheckDate],[CheckUser],[CheckUserName]) OUTPUT INSERTED.[Id] VALUES (@ArticlesTotalCount,@ArticlesWithoutNumberCount,@ArticlesWithWrongNumberCount,@CheckDate,@CheckUser,@CheckUserName); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArticlesTotalCount", item.ArticlesTotalCount == null ? (object)DBNull.Value : item.ArticlesTotalCount);
					sqlCommand.Parameters.AddWithValue("ArticlesWithoutNumberCount", item.ArticlesWithoutNumberCount == null ? (object)DBNull.Value : item.ArticlesWithoutNumberCount);
					sqlCommand.Parameters.AddWithValue("ArticlesWithWrongNumberCount", item.ArticlesWithWrongNumberCount == null ? (object)DBNull.Value : item.ArticlesWithWrongNumberCount);
					sqlCommand.Parameters.AddWithValue("CheckDate", item.CheckDate == null ? (object)DBNull.Value : item.CheckDate);
					sqlCommand.Parameters.AddWithValue("CheckUser", item.CheckUser == null ? (object)DBNull.Value : item.CheckUser);
					sqlCommand.Parameters.AddWithValue("CheckUserName", item.CheckUserName == null ? (object)DBNull.Value : item.CheckUserName);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; /* Nb params per query */
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				using(var sqlCommand = new SqlCommand("", sqlConnection))
				{
					sqlConnection.Open();
					string query = "";
					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [__LGT_ArticleCustomsNumberCheck] ([ArticlesTotalCount],[ArticlesWithoutNumberCount],[ArticlesWithWrongNumberCount],[CheckDate],[CheckUser],[CheckUserName]) VALUES ("

							+ "@ArticlesTotalCount" + i + ","
							+ "@ArticlesWithoutNumberCount" + i + ","
							+ "@ArticlesWithWrongNumberCount" + i + ","
							+ "@CheckDate" + i + ","
							+ "@CheckUser" + i + ","
							+ "@CheckUserName" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArticlesTotalCount" + i, item.ArticlesTotalCount == null ? (object)DBNull.Value : item.ArticlesTotalCount);
						sqlCommand.Parameters.AddWithValue("ArticlesWithoutNumberCount" + i, item.ArticlesWithoutNumberCount == null ? (object)DBNull.Value : item.ArticlesWithoutNumberCount);
						sqlCommand.Parameters.AddWithValue("ArticlesWithWrongNumberCount" + i, item.ArticlesWithWrongNumberCount == null ? (object)DBNull.Value : item.ArticlesWithWrongNumberCount);
						sqlCommand.Parameters.AddWithValue("CheckDate" + i, item.CheckDate == null ? (object)DBNull.Value : item.CheckDate);
						sqlCommand.Parameters.AddWithValue("CheckUser" + i, item.CheckUser == null ? (object)DBNull.Value : item.CheckUser);
						sqlCommand.Parameters.AddWithValue("CheckUserName" + i, item.CheckUserName == null ? (object)DBNull.Value : item.CheckUserName);
					}

					sqlCommand.CommandText = query;
					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity item)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("", sqlConnection))
			{
				sqlConnection.Open();
				string query = "UPDATE [__LGT_ArticleCustomsNumberCheck] SET [ArticlesTotalCount]=@ArticlesTotalCount, [ArticlesWithoutNumberCount]=@ArticlesWithoutNumberCount, [ArticlesWithWrongNumberCount]=@ArticlesWithWrongNumberCount, [CheckDate]=@CheckDate, [CheckUser]=@CheckUser, [CheckUserName]=@CheckUserName WHERE [Id]=@Id";
				sqlCommand.CommandText = query;
				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArticlesTotalCount", item.ArticlesTotalCount == null ? (object)DBNull.Value : item.ArticlesTotalCount);
				sqlCommand.Parameters.AddWithValue("ArticlesWithoutNumberCount", item.ArticlesWithoutNumberCount == null ? (object)DBNull.Value : item.ArticlesWithoutNumberCount);
				sqlCommand.Parameters.AddWithValue("ArticlesWithWrongNumberCount", item.ArticlesWithWrongNumberCount == null ? (object)DBNull.Value : item.ArticlesWithWrongNumberCount);
				sqlCommand.Parameters.AddWithValue("CheckDate", item.CheckDate == null ? (object)DBNull.Value : item.CheckDate);
				sqlCommand.Parameters.AddWithValue("CheckUser", item.CheckUser == null ? (object)DBNull.Value : item.CheckUser);
				sqlCommand.Parameters.AddWithValue("CheckUserName", item.CheckUserName == null ? (object)DBNull.Value : item.CheckUserName);

				return sqlCommand.ExecuteNonQuery();
			}
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; /* Nb params per query */
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
		private static int update(List<Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				using(var sqlCommand = new SqlCommand("", sqlConnection))
				{
					sqlConnection.Open();
					string query = "";

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += "UPDATE [__LGT_ArticleCustomsNumberCheck] SET "

							+ "[ArticlesTotalCount]=@ArticlesTotalCount" + i + ","
							+ "[ArticlesWithoutNumberCount]=@ArticlesWithoutNumberCount" + i + ","
							+ "[ArticlesWithWrongNumberCount]=@ArticlesWithWrongNumberCount" + i + ","
							+ "[CheckDate]=@CheckDate" + i + ","
							+ "[CheckUser]=@CheckUser" + i + ","
							+ "[CheckUserName]=@CheckUserName" + i + $" WHERE [Id]=@Id{i}"
							+ "; ";

						sqlCommand.Parameters.AddWithValue($"Id{i}", item.Id);

						sqlCommand.Parameters.AddWithValue("ArticlesTotalCount" + i, item.ArticlesTotalCount == null ? (object)DBNull.Value : item.ArticlesTotalCount);
						sqlCommand.Parameters.AddWithValue("ArticlesWithoutNumberCount" + i, item.ArticlesWithoutNumberCount == null ? (object)DBNull.Value : item.ArticlesWithoutNumberCount);
						sqlCommand.Parameters.AddWithValue("ArticlesWithWrongNumberCount" + i, item.ArticlesWithWrongNumberCount == null ? (object)DBNull.Value : item.ArticlesWithWrongNumberCount);
						sqlCommand.Parameters.AddWithValue("CheckDate" + i, item.CheckDate == null ? (object)DBNull.Value : item.CheckDate);
						sqlCommand.Parameters.AddWithValue("CheckUser" + i, item.CheckUser == null ? (object)DBNull.Value : item.CheckUser);
						sqlCommand.Parameters.AddWithValue("CheckUserName" + i, item.CheckUserName == null ? (object)DBNull.Value : item.CheckUserName);
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
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("DELETE FROM [__LGT_ArticleCustomsNumberCheck] WHERE [Id]=@Id", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("Id", id);

				return sqlCommand.ExecuteNonQuery();
			}
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
				using(var sqlCommand = new SqlCommand("", sqlConnection))
				{
					sqlConnection.Open();
					var queryIds = new List<string>();
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds.Add($"@Id{i}");
						sqlCommand.Parameters.AddWithValue($"Id{i}", ids[i]);
					}

					string query = $"DELETE FROM [__LGT_ArticleCustomsNumberCheck] WHERE [Id] IN ({string.Join(",", queryIds)})";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			using(var sqlCommand = new SqlCommand("SELECT * FROM [__LGT_ArticleCustomsNumberCheck] WHERE [Id] = @Id", connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", id);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			using(var sqlCommand = new SqlCommand("SELECT * FROM [__LGT_ArticleCustomsNumberCheck]", connection, transaction))
			{
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlCommand = new SqlCommand("", connection, transaction))
				{
					var queryIds = new List<string>();
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds.Add($"@Id{i}");
						sqlCommand.Parameters.AddWithValue($"Id{i}", ids[i]);
					}

					sqlCommand.CommandText = $"SELECT * FROM [__LGT_ArticleCustomsNumberCheck] WHERE [Id] IN ({string.Join(",", queryIds)})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "INSERT INTO __LGT_ArticleCustomsNumberCheck ([ArticlesTotalCount],[ArticlesWithoutNumberCount],[ArticlesWithWrongNumberCount],[CheckDate],[CheckUser],[CheckUserName]) OUTPUT INSERTED.[Id] VALUES (@ArticlesTotalCount,@ArticlesWithoutNumberCount,@ArticlesWithWrongNumberCount,@CheckDate,@CheckUser,@CheckUserName); ";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("ArticlesTotalCount", item.ArticlesTotalCount == null ? (object)DBNull.Value : item.ArticlesTotalCount);
				sqlCommand.Parameters.AddWithValue("ArticlesWithoutNumberCount", item.ArticlesWithoutNumberCount == null ? (object)DBNull.Value : item.ArticlesWithoutNumberCount);
				sqlCommand.Parameters.AddWithValue("ArticlesWithWrongNumberCount", item.ArticlesWithWrongNumberCount == null ? (object)DBNull.Value : item.ArticlesWithWrongNumberCount);
				sqlCommand.Parameters.AddWithValue("CheckDate", item.CheckDate == null ? (object)DBNull.Value : item.CheckDate);
				sqlCommand.Parameters.AddWithValue("CheckUser", item.CheckUser == null ? (object)DBNull.Value : item.CheckUser);
				sqlCommand.Parameters.AddWithValue("CheckUserName", item.CheckUserName == null ? (object)DBNull.Value : item.CheckUserName);
				var result = sqlCommand.ExecuteScalar();
				return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; /* Nb params per query */
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				using(var sqlCommand = new SqlCommand(query, connection, transaction))
				{
					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += "INSERT INTO [__LGT_ArticleCustomsNumberCheck] ([ArticlesTotalCount],[ArticlesWithoutNumberCount],[ArticlesWithWrongNumberCount],[CheckDate],[CheckUser],[CheckUserName]) VALUES ( "

						+ "@ArticlesTotalCount" + i + ","
						+ "@ArticlesWithoutNumberCount" + i + ","
						+ "@ArticlesWithWrongNumberCount" + i + ","
						+ "@CheckDate" + i + ","
						+ "@CheckUser" + i + ","
						+ "@CheckUserName" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArticlesTotalCount" + i, item.ArticlesTotalCount == null ? (object)DBNull.Value : item.ArticlesTotalCount);
						sqlCommand.Parameters.AddWithValue("ArticlesWithoutNumberCount" + i, item.ArticlesWithoutNumberCount == null ? (object)DBNull.Value : item.ArticlesWithoutNumberCount);
						sqlCommand.Parameters.AddWithValue("ArticlesWithWrongNumberCount" + i, item.ArticlesWithWrongNumberCount == null ? (object)DBNull.Value : item.ArticlesWithWrongNumberCount);
						sqlCommand.Parameters.AddWithValue("CheckDate" + i, item.CheckDate == null ? (object)DBNull.Value : item.CheckDate);
						sqlCommand.Parameters.AddWithValue("CheckUser" + i, item.CheckUser == null ? (object)DBNull.Value : item.CheckUser);
						sqlCommand.Parameters.AddWithValue("CheckUserName" + i, item.CheckUserName == null ? (object)DBNull.Value : item.CheckUserName);
					}

					sqlCommand.CommandText = query;

					return sqlCommand.ExecuteNonQuery();
				}
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [__LGT_ArticleCustomsNumberCheck] SET [ArticlesTotalCount]=@ArticlesTotalCount, [ArticlesWithoutNumberCount]=@ArticlesWithoutNumberCount, [ArticlesWithWrongNumberCount]=@ArticlesWithWrongNumberCount, [CheckDate]=@CheckDate, [CheckUser]=@CheckUser, [CheckUserName]=@CheckUserName WHERE [Id]=@Id";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArticlesTotalCount", item.ArticlesTotalCount == null ? (object)DBNull.Value : item.ArticlesTotalCount);
				sqlCommand.Parameters.AddWithValue("ArticlesWithoutNumberCount", item.ArticlesWithoutNumberCount == null ? (object)DBNull.Value : item.ArticlesWithoutNumberCount);
				sqlCommand.Parameters.AddWithValue("ArticlesWithWrongNumberCount", item.ArticlesWithWrongNumberCount == null ? (object)DBNull.Value : item.ArticlesWithWrongNumberCount);
				sqlCommand.Parameters.AddWithValue("CheckDate", item.CheckDate == null ? (object)DBNull.Value : item.CheckDate);
				sqlCommand.Parameters.AddWithValue("CheckUser", item.CheckUser == null ? (object)DBNull.Value : item.CheckUser);
				sqlCommand.Parameters.AddWithValue("CheckUserName", item.CheckUserName == null ? (object)DBNull.Value : item.CheckUserName);

				return sqlCommand.ExecuteNonQuery();
			}
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; /* Nb params per query */
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				using(var sqlCommand = new SqlCommand(query, connection, transaction))
				{
					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += "UPDATE [__LGT_ArticleCustomsNumberCheck] SET "

						+ "[ArticlesTotalCount]=@ArticlesTotalCount" + i + ","
						+ "[ArticlesWithoutNumberCount]=@ArticlesWithoutNumberCount" + i + ","
						+ "[ArticlesWithWrongNumberCount]=@ArticlesWithWrongNumberCount" + i + ","
						+ "[CheckDate]=@CheckDate" + i + ","
						+ "[CheckUser]=@CheckUser" + i + ","
						+ "[CheckUserName]=@CheckUserName" + i + " WHERE [Id]=@Id" + i
							+ ";";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);

						sqlCommand.Parameters.AddWithValue("ArticlesTotalCount" + i, item.ArticlesTotalCount == null ? (object)DBNull.Value : item.ArticlesTotalCount);
						sqlCommand.Parameters.AddWithValue("ArticlesWithoutNumberCount" + i, item.ArticlesWithoutNumberCount == null ? (object)DBNull.Value : item.ArticlesWithoutNumberCount);
						sqlCommand.Parameters.AddWithValue("ArticlesWithWrongNumberCount" + i, item.ArticlesWithWrongNumberCount == null ? (object)DBNull.Value : item.ArticlesWithWrongNumberCount);
						sqlCommand.Parameters.AddWithValue("CheckDate" + i, item.CheckDate == null ? (object)DBNull.Value : item.CheckDate);
						sqlCommand.Parameters.AddWithValue("CheckUser" + i, item.CheckUser == null ? (object)DBNull.Value : item.CheckUser);
						sqlCommand.Parameters.AddWithValue("CheckUserName" + i, item.CheckUserName == null ? (object)DBNull.Value : item.CheckUserName);
					}

					sqlCommand.CommandText = query;
					return sqlCommand.ExecuteNonQuery();
				}
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "DELETE FROM [__LGT_ArticleCustomsNumberCheck] WHERE [Id]=@Id";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", id);
				return sqlCommand.ExecuteNonQuery();
			}
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
				using(var sqlCommand = new SqlCommand("", connection, transaction))
				{
					var queryIds = new List<string>();
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds.Add($"@Id{i}");
						sqlCommand.Parameters.AddWithValue($"Id{i}", ids[i]);
					}

					string query = $"DELETE FROM [__LGT_ArticleCustomsNumberCheck] WHERE [Id] IN ({string.Join(",", queryIds)})";
					sqlCommand.CommandText = query;

					return sqlCommand.ExecuteNonQuery();
				}
			}
			return -1;
		}

		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity GetLastInsertedRecord()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [__LGT_ArticleCustomsNumberCheck] WHERE [CheckDate]=(SELECT MAX([CheckDate]) FROM [__LGT_ArticleCustomsNumberCheck])", sqlConnection))
			{
				sqlConnection.Open();

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.LGT.ArticleCustomsNumberCheckEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		#endregion Custom Methods

	}
}
