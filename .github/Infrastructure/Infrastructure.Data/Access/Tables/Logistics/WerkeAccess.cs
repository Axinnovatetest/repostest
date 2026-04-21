using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.Logistics
{
	public class WerkeAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__LGT_Werke] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__LGT_Werke]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__LGT_Werke] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__LGT_Werke] ([City1],[City2],[Country],[IdCompany],[Name1],[Name2],[SiteName],[Street1],[Street2],[ZipCode]) OUTPUT INSERTED.[Id] VALUES (@City1,@City2,@Country,@IdCompany,@Name1,@Name2,@SiteName,@Street1,@Street2,@ZipCode); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("City1", item.City1 == null ? (object)DBNull.Value : item.City1);
					sqlCommand.Parameters.AddWithValue("City2", item.City2 == null ? (object)DBNull.Value : item.City2);
					sqlCommand.Parameters.AddWithValue("Country", item.Country == null ? (object)DBNull.Value : item.Country);
					sqlCommand.Parameters.AddWithValue("IdCompany", item.IdCompany == null ? (object)DBNull.Value : item.IdCompany);
					sqlCommand.Parameters.AddWithValue("Name1", item.Name1 == null ? (object)DBNull.Value : item.Name1);
					sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
					sqlCommand.Parameters.AddWithValue("SiteName", item.SiteName == null ? (object)DBNull.Value : item.SiteName);
					sqlCommand.Parameters.AddWithValue("Street1", item.Street1 == null ? (object)DBNull.Value : item.Street1);
					sqlCommand.Parameters.AddWithValue("Street2", item.Street2 == null ? (object)DBNull.Value : item.Street2);
					sqlCommand.Parameters.AddWithValue("ZipCode", item.ZipCode == null ? (object)DBNull.Value : item.ZipCode);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 11; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity> items)
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
						query += " INSERT INTO [__LGT_Werke] ([City1],[City2],[Country],[IdCompany],[Name1],[Name2],[SiteName],[Street1],[Street2],[ZipCode]) VALUES ( "

							+ "@City1" + i + ","
							+ "@City2" + i + ","
							+ "@Country" + i + ","
							+ "@IdCompany" + i + ","
							+ "@Name1" + i + ","
							+ "@Name2" + i + ","
							+ "@SiteName" + i + ","
							+ "@Street1" + i + ","
							+ "@Street2" + i + ","
							+ "@ZipCode" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("City1" + i, item.City1 == null ? (object)DBNull.Value : item.City1);
						sqlCommand.Parameters.AddWithValue("City2" + i, item.City2 == null ? (object)DBNull.Value : item.City2);
						sqlCommand.Parameters.AddWithValue("Country" + i, item.Country == null ? (object)DBNull.Value : item.Country);
						sqlCommand.Parameters.AddWithValue("IdCompany" + i, item.IdCompany == null ? (object)DBNull.Value : item.IdCompany);
						sqlCommand.Parameters.AddWithValue("Name1" + i, item.Name1 == null ? (object)DBNull.Value : item.Name1);
						sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
						sqlCommand.Parameters.AddWithValue("SiteName" + i, item.SiteName == null ? (object)DBNull.Value : item.SiteName);
						sqlCommand.Parameters.AddWithValue("Street1" + i, item.Street1 == null ? (object)DBNull.Value : item.Street1);
						sqlCommand.Parameters.AddWithValue("Street2" + i, item.Street2 == null ? (object)DBNull.Value : item.Street2);
						sqlCommand.Parameters.AddWithValue("ZipCode" + i, item.ZipCode == null ? (object)DBNull.Value : item.ZipCode);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__LGT_Werke] SET [City1]=@City1, [City2]=@City2, [Country]=@Country, [IdCompany]=@IdCompany, [Name1]=@Name1, [Name2]=@Name2, [SiteName]=@SiteName, [Street1]=@Street1, [Street2]=@Street2, [ZipCode]=@ZipCode WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("City1", item.City1 == null ? (object)DBNull.Value : item.City1);
				sqlCommand.Parameters.AddWithValue("City2", item.City2 == null ? (object)DBNull.Value : item.City2);
				sqlCommand.Parameters.AddWithValue("Country", item.Country == null ? (object)DBNull.Value : item.Country);
				sqlCommand.Parameters.AddWithValue("IdCompany", item.IdCompany == null ? (object)DBNull.Value : item.IdCompany);
				sqlCommand.Parameters.AddWithValue("Name1", item.Name1 == null ? (object)DBNull.Value : item.Name1);
				sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
				sqlCommand.Parameters.AddWithValue("SiteName", item.SiteName == null ? (object)DBNull.Value : item.SiteName);
				sqlCommand.Parameters.AddWithValue("Street1", item.Street1 == null ? (object)DBNull.Value : item.Street1);
				sqlCommand.Parameters.AddWithValue("Street2", item.Street2 == null ? (object)DBNull.Value : item.Street2);
				sqlCommand.Parameters.AddWithValue("ZipCode", item.ZipCode == null ? (object)DBNull.Value : item.ZipCode);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 11; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity> items)
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
						query += " UPDATE [__LGT_Werke] SET "

							+ "[City1]=@City1" + i + ","
							+ "[City2]=@City2" + i + ","
							+ "[Country]=@Country" + i + ","
							+ "[IdCompany]=@IdCompany" + i + ","
							+ "[Name1]=@Name1" + i + ","
							+ "[Name2]=@Name2" + i + ","
							+ "[SiteName]=@SiteName" + i + ","
							+ "[Street1]=@Street1" + i + ","
							+ "[Street2]=@Street2" + i + ","
							+ "[ZipCode]=@ZipCode" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("City1" + i, item.City1 == null ? (object)DBNull.Value : item.City1);
						sqlCommand.Parameters.AddWithValue("City2" + i, item.City2 == null ? (object)DBNull.Value : item.City2);
						sqlCommand.Parameters.AddWithValue("Country" + i, item.Country == null ? (object)DBNull.Value : item.Country);
						sqlCommand.Parameters.AddWithValue("IdCompany" + i, item.IdCompany == null ? (object)DBNull.Value : item.IdCompany);
						sqlCommand.Parameters.AddWithValue("Name1" + i, item.Name1 == null ? (object)DBNull.Value : item.Name1);
						sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
						sqlCommand.Parameters.AddWithValue("SiteName" + i, item.SiteName == null ? (object)DBNull.Value : item.SiteName);
						sqlCommand.Parameters.AddWithValue("Street1" + i, item.Street1 == null ? (object)DBNull.Value : item.Street1);
						sqlCommand.Parameters.AddWithValue("Street2" + i, item.Street2 == null ? (object)DBNull.Value : item.Street2);
						sqlCommand.Parameters.AddWithValue("ZipCode" + i, item.ZipCode == null ? (object)DBNull.Value : item.ZipCode);
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
				string query = "DELETE FROM [__LGT_Werke] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__LGT_Werke] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__LGT_Werke] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__LGT_Werke]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__LGT_Werke] WHERE [Id] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [__LGT_Werke] ([City1],[City2],[Country],[IdCompany],[Name1],[Name2],[SiteName],[Street1],[Street2],[ZipCode]) OUTPUT INSERTED.[Id] VALUES (@City1,@City2,@Country,@IdCompany,@Name1,@Name2,@SiteName,@Street1,@Street2,@ZipCode); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("City1", item.City1 == null ? (object)DBNull.Value : item.City1);
			sqlCommand.Parameters.AddWithValue("City2", item.City2 == null ? (object)DBNull.Value : item.City2);
			sqlCommand.Parameters.AddWithValue("Country", item.Country == null ? (object)DBNull.Value : item.Country);
			sqlCommand.Parameters.AddWithValue("IdCompany", item.IdCompany == null ? (object)DBNull.Value : item.IdCompany);
			sqlCommand.Parameters.AddWithValue("Name1", item.Name1 == null ? (object)DBNull.Value : item.Name1);
			sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
			sqlCommand.Parameters.AddWithValue("SiteName", item.SiteName == null ? (object)DBNull.Value : item.SiteName);
			sqlCommand.Parameters.AddWithValue("Street1", item.Street1 == null ? (object)DBNull.Value : item.Street1);
			sqlCommand.Parameters.AddWithValue("Street2", item.Street2 == null ? (object)DBNull.Value : item.Street2);
			sqlCommand.Parameters.AddWithValue("ZipCode", item.ZipCode == null ? (object)DBNull.Value : item.ZipCode);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 11; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__LGT_Werke] ([City1],[City2],[Country],[IdCompany],[Name1],[Name2],[SiteName],[Street1],[Street2],[ZipCode]) VALUES ( "

						+ "@City1" + i + ","
						+ "@City2" + i + ","
						+ "@Country" + i + ","
						+ "@IdCompany" + i + ","
						+ "@Name1" + i + ","
						+ "@Name2" + i + ","
						+ "@SiteName" + i + ","
						+ "@Street1" + i + ","
						+ "@Street2" + i + ","
						+ "@ZipCode" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("City1" + i, item.City1 == null ? (object)DBNull.Value : item.City1);
					sqlCommand.Parameters.AddWithValue("City2" + i, item.City2 == null ? (object)DBNull.Value : item.City2);
					sqlCommand.Parameters.AddWithValue("Country" + i, item.Country == null ? (object)DBNull.Value : item.Country);
					sqlCommand.Parameters.AddWithValue("IdCompany" + i, item.IdCompany == null ? (object)DBNull.Value : item.IdCompany);
					sqlCommand.Parameters.AddWithValue("Name1" + i, item.Name1 == null ? (object)DBNull.Value : item.Name1);
					sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
					sqlCommand.Parameters.AddWithValue("SiteName" + i, item.SiteName == null ? (object)DBNull.Value : item.SiteName);
					sqlCommand.Parameters.AddWithValue("Street1" + i, item.Street1 == null ? (object)DBNull.Value : item.Street1);
					sqlCommand.Parameters.AddWithValue("Street2" + i, item.Street2 == null ? (object)DBNull.Value : item.Street2);
					sqlCommand.Parameters.AddWithValue("ZipCode" + i, item.ZipCode == null ? (object)DBNull.Value : item.ZipCode);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__LGT_Werke] SET [City1]=@City1, [City2]=@City2, [Country]=@Country, [IdCompany]=@IdCompany, [Name1]=@Name1, [Name2]=@Name2, [SiteName]=@SiteName, [Street1]=@Street1, [Street2]=@Street2, [ZipCode]=@ZipCode WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("City1", item.City1 == null ? (object)DBNull.Value : item.City1);
			sqlCommand.Parameters.AddWithValue("City2", item.City2 == null ? (object)DBNull.Value : item.City2);
			sqlCommand.Parameters.AddWithValue("Country", item.Country == null ? (object)DBNull.Value : item.Country);
			sqlCommand.Parameters.AddWithValue("IdCompany", item.IdCompany == null ? (object)DBNull.Value : item.IdCompany);
			sqlCommand.Parameters.AddWithValue("Name1", item.Name1 == null ? (object)DBNull.Value : item.Name1);
			sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
			sqlCommand.Parameters.AddWithValue("SiteName", item.SiteName == null ? (object)DBNull.Value : item.SiteName);
			sqlCommand.Parameters.AddWithValue("Street1", item.Street1 == null ? (object)DBNull.Value : item.Street1);
			sqlCommand.Parameters.AddWithValue("Street2", item.Street2 == null ? (object)DBNull.Value : item.Street2);
			sqlCommand.Parameters.AddWithValue("ZipCode", item.ZipCode == null ? (object)DBNull.Value : item.ZipCode);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 11; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.WerkeEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__LGT_Werke] SET "

					+ "[City1]=@City1" + i + ","
					+ "[City2]=@City2" + i + ","
					+ "[Country]=@Country" + i + ","
					+ "[IdCompany]=@IdCompany" + i + ","
					+ "[Name1]=@Name1" + i + ","
					+ "[Name2]=@Name2" + i + ","
					+ "[SiteName]=@SiteName" + i + ","
					+ "[Street1]=@Street1" + i + ","
					+ "[Street2]=@Street2" + i + ","
					+ "[ZipCode]=@ZipCode" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("City1" + i, item.City1 == null ? (object)DBNull.Value : item.City1);
					sqlCommand.Parameters.AddWithValue("City2" + i, item.City2 == null ? (object)DBNull.Value : item.City2);
					sqlCommand.Parameters.AddWithValue("Country" + i, item.Country == null ? (object)DBNull.Value : item.Country);
					sqlCommand.Parameters.AddWithValue("IdCompany" + i, item.IdCompany == null ? (object)DBNull.Value : item.IdCompany);
					sqlCommand.Parameters.AddWithValue("Name1" + i, item.Name1 == null ? (object)DBNull.Value : item.Name1);
					sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
					sqlCommand.Parameters.AddWithValue("SiteName" + i, item.SiteName == null ? (object)DBNull.Value : item.SiteName);
					sqlCommand.Parameters.AddWithValue("Street1" + i, item.Street1 == null ? (object)DBNull.Value : item.Street1);
					sqlCommand.Parameters.AddWithValue("Street2" + i, item.Street2 == null ? (object)DBNull.Value : item.Street2);
					sqlCommand.Parameters.AddWithValue("ZipCode" + i, item.ZipCode == null ? (object)DBNull.Value : item.ZipCode);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__LGT_Werke] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [__LGT_Werke] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods

		#endregion Custom Methods

	}
}
