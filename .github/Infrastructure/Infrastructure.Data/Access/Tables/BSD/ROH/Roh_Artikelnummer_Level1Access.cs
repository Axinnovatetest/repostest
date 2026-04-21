using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class Roh_Artikelnummer_Level1Access
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Roh_Artikelnummer_Level1] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Roh_Artikelnummer_Level1]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Roh_Artikelnummer_Level1] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Roh_Artikelnummer_Level1] ([ClassificationId],[IncludeInDescription],[Name],[OrderInDescription],[Part],[PartOrder],[Seperator],[ValueAtBeginningOfDescription],[ValueInDescription]) OUTPUT INSERTED.[Id] VALUES (@ClassificationId,@IncludeInDescription,@Name,@OrderInDescription,@Part,@PartOrder,@Seperator,@ValueAtBeginningOfDescription,@ValueInDescription); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ClassificationId", item.ClassificationId == null ? (object)DBNull.Value : item.ClassificationId);
					sqlCommand.Parameters.AddWithValue("IncludeInDescription", item.IncludeInDescription == null ? (object)DBNull.Value : item.IncludeInDescription);
					sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? (object)DBNull.Value : item.Name);
					sqlCommand.Parameters.AddWithValue("OrderInDescription", item.OrderInDescription == null ? (object)DBNull.Value : item.OrderInDescription);
					sqlCommand.Parameters.AddWithValue("Part", item.Part == null ? (object)DBNull.Value : item.Part);
					sqlCommand.Parameters.AddWithValue("PartOrder", item.PartOrder == null ? (object)DBNull.Value : item.PartOrder);
					sqlCommand.Parameters.AddWithValue("Seperator", item.Seperator == null ? (object)DBNull.Value : item.Seperator);
					sqlCommand.Parameters.AddWithValue("ValueAtBeginningOfDescription", item.ValueAtBeginningOfDescription == null ? (object)DBNull.Value : item.ValueAtBeginningOfDescription);
					sqlCommand.Parameters.AddWithValue("ValueInDescription", item.ValueInDescription == null ? (object)DBNull.Value : item.ValueInDescription);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity> items)
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
						query += " INSERT INTO [Roh_Artikelnummer_Level1] ([ClassificationId],[IncludeInDescription],[Name],[OrderInDescription],[Part],[PartOrder],[Seperator],[ValueAtBeginningOfDescription],[ValueInDescription]) VALUES ( "

							+ "@ClassificationId" + i + ","
							+ "@IncludeInDescription" + i + ","
							+ "@Name" + i + ","
							+ "@OrderInDescription" + i + ","
							+ "@Part" + i + ","
							+ "@PartOrder" + i + ","
							+ "@Seperator" + i + ","
							+ "@ValueAtBeginningOfDescription" + i + ","
							+ "@ValueInDescription" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ClassificationId" + i, item.ClassificationId == null ? (object)DBNull.Value : item.ClassificationId);
						sqlCommand.Parameters.AddWithValue("IncludeInDescription" + i, item.IncludeInDescription == null ? (object)DBNull.Value : item.IncludeInDescription);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name == null ? (object)DBNull.Value : item.Name);
						sqlCommand.Parameters.AddWithValue("OrderInDescription" + i, item.OrderInDescription == null ? (object)DBNull.Value : item.OrderInDescription);
						sqlCommand.Parameters.AddWithValue("Part" + i, item.Part == null ? (object)DBNull.Value : item.Part);
						sqlCommand.Parameters.AddWithValue("PartOrder" + i, item.PartOrder == null ? (object)DBNull.Value : item.PartOrder);
						sqlCommand.Parameters.AddWithValue("Seperator" + i, item.Seperator == null ? (object)DBNull.Value : item.Seperator);
						sqlCommand.Parameters.AddWithValue("ValueAtBeginningOfDescription" + i, item.ValueAtBeginningOfDescription == null ? (object)DBNull.Value : item.ValueAtBeginningOfDescription);
						sqlCommand.Parameters.AddWithValue("ValueInDescription" + i, item.ValueInDescription == null ? (object)DBNull.Value : item.ValueInDescription);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Roh_Artikelnummer_Level1] SET [ClassificationId]=@ClassificationId, [IncludeInDescription]=@IncludeInDescription, [Name]=@Name, [OrderInDescription]=@OrderInDescription, [Part]=@Part, [PartOrder]=@PartOrder, [Seperator]=@Seperator, [ValueAtBeginningOfDescription]=@ValueAtBeginningOfDescription, [ValueInDescription]=@ValueInDescription WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ClassificationId", item.ClassificationId == null ? (object)DBNull.Value : item.ClassificationId);
				sqlCommand.Parameters.AddWithValue("IncludeInDescription", item.IncludeInDescription == null ? (object)DBNull.Value : item.IncludeInDescription);
				sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? (object)DBNull.Value : item.Name);
				sqlCommand.Parameters.AddWithValue("OrderInDescription", item.OrderInDescription == null ? (object)DBNull.Value : item.OrderInDescription);
				sqlCommand.Parameters.AddWithValue("Part", item.Part == null ? (object)DBNull.Value : item.Part);
				sqlCommand.Parameters.AddWithValue("PartOrder", item.PartOrder == null ? (object)DBNull.Value : item.PartOrder);
				sqlCommand.Parameters.AddWithValue("Seperator", item.Seperator == null ? (object)DBNull.Value : item.Seperator);
				sqlCommand.Parameters.AddWithValue("ValueAtBeginningOfDescription", item.ValueAtBeginningOfDescription == null ? (object)DBNull.Value : item.ValueAtBeginningOfDescription);
				sqlCommand.Parameters.AddWithValue("ValueInDescription", item.ValueInDescription == null ? (object)DBNull.Value : item.ValueInDescription);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity> items)
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
						query += " UPDATE [Roh_Artikelnummer_Level1] SET "

							+ "[ClassificationId]=@ClassificationId" + i + ","
							+ "[IncludeInDescription]=@IncludeInDescription" + i + ","
							+ "[Name]=@Name" + i + ","
							+ "[OrderInDescription]=@OrderInDescription" + i + ","
							+ "[Part]=@Part" + i + ","
							+ "[PartOrder]=@PartOrder" + i + ","
							+ "[Seperator]=@Seperator" + i + ","
							+ "[ValueAtBeginningOfDescription]=@ValueAtBeginningOfDescription" + i + ","
							+ "[ValueInDescription]=@ValueInDescription" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ClassificationId" + i, item.ClassificationId == null ? (object)DBNull.Value : item.ClassificationId);
						sqlCommand.Parameters.AddWithValue("IncludeInDescription" + i, item.IncludeInDescription == null ? (object)DBNull.Value : item.IncludeInDescription);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name == null ? (object)DBNull.Value : item.Name);
						sqlCommand.Parameters.AddWithValue("OrderInDescription" + i, item.OrderInDescription == null ? (object)DBNull.Value : item.OrderInDescription);
						sqlCommand.Parameters.AddWithValue("Part" + i, item.Part == null ? (object)DBNull.Value : item.Part);
						sqlCommand.Parameters.AddWithValue("PartOrder" + i, item.PartOrder == null ? (object)DBNull.Value : item.PartOrder);
						sqlCommand.Parameters.AddWithValue("Seperator" + i, item.Seperator == null ? (object)DBNull.Value : item.Seperator);
						sqlCommand.Parameters.AddWithValue("ValueAtBeginningOfDescription" + i, item.ValueAtBeginningOfDescription == null ? (object)DBNull.Value : item.ValueAtBeginningOfDescription);
						sqlCommand.Parameters.AddWithValue("ValueInDescription" + i, item.ValueInDescription == null ? (object)DBNull.Value : item.ValueInDescription);
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
				string query = "DELETE FROM [Roh_Artikelnummer_Level1] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [Roh_Artikelnummer_Level1] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Roh_Artikelnummer_Level1] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Roh_Artikelnummer_Level1]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Roh_Artikelnummer_Level1] WHERE [Id] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [Roh_Artikelnummer_Level1] ([ClassificationId],[IncludeInDescription],[Name],[OrderInDescription],[Part],[PartOrder],[Seperator],[ValueAtBeginningOfDescription],[ValueInDescription]) OUTPUT INSERTED.[Id] VALUES (@ClassificationId,@IncludeInDescription,@Name,@OrderInDescription,@Part,@PartOrder,@Seperator,@ValueAtBeginningOfDescription,@ValueInDescription); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ClassificationId", item.ClassificationId == null ? (object)DBNull.Value : item.ClassificationId);
			sqlCommand.Parameters.AddWithValue("IncludeInDescription", item.IncludeInDescription == null ? (object)DBNull.Value : item.IncludeInDescription);
			sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? (object)DBNull.Value : item.Name);
			sqlCommand.Parameters.AddWithValue("OrderInDescription", item.OrderInDescription == null ? (object)DBNull.Value : item.OrderInDescription);
			sqlCommand.Parameters.AddWithValue("Part", item.Part == null ? (object)DBNull.Value : item.Part);
			sqlCommand.Parameters.AddWithValue("PartOrder", item.PartOrder == null ? (object)DBNull.Value : item.PartOrder);
			sqlCommand.Parameters.AddWithValue("Seperator", item.Seperator == null ? (object)DBNull.Value : item.Seperator);
			sqlCommand.Parameters.AddWithValue("ValueAtBeginningOfDescription", item.ValueAtBeginningOfDescription == null ? (object)DBNull.Value : item.ValueAtBeginningOfDescription);
			sqlCommand.Parameters.AddWithValue("ValueInDescription", item.ValueInDescription == null ? (object)DBNull.Value : item.ValueInDescription);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Roh_Artikelnummer_Level1] ([ClassificationId],[IncludeInDescription],[Name],[OrderInDescription],[Part],[PartOrder],[Seperator],[ValueAtBeginningOfDescription],[ValueInDescription]) VALUES ( "

						+ "@ClassificationId" + i + ","
						+ "@IncludeInDescription" + i + ","
						+ "@Name" + i + ","
						+ "@OrderInDescription" + i + ","
						+ "@Part" + i + ","
						+ "@PartOrder" + i + ","
						+ "@Seperator" + i + ","
						+ "@ValueAtBeginningOfDescription" + i + ","
						+ "@ValueInDescription" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("ClassificationId" + i, item.ClassificationId == null ? (object)DBNull.Value : item.ClassificationId);
					sqlCommand.Parameters.AddWithValue("IncludeInDescription" + i, item.IncludeInDescription == null ? (object)DBNull.Value : item.IncludeInDescription);
					sqlCommand.Parameters.AddWithValue("Name" + i, item.Name == null ? (object)DBNull.Value : item.Name);
					sqlCommand.Parameters.AddWithValue("OrderInDescription" + i, item.OrderInDescription == null ? (object)DBNull.Value : item.OrderInDescription);
					sqlCommand.Parameters.AddWithValue("Part" + i, item.Part == null ? (object)DBNull.Value : item.Part);
					sqlCommand.Parameters.AddWithValue("PartOrder" + i, item.PartOrder == null ? (object)DBNull.Value : item.PartOrder);
					sqlCommand.Parameters.AddWithValue("Seperator" + i, item.Seperator == null ? (object)DBNull.Value : item.Seperator);
					sqlCommand.Parameters.AddWithValue("ValueAtBeginningOfDescription" + i, item.ValueAtBeginningOfDescription == null ? (object)DBNull.Value : item.ValueAtBeginningOfDescription);
					sqlCommand.Parameters.AddWithValue("ValueInDescription" + i, item.ValueInDescription == null ? (object)DBNull.Value : item.ValueInDescription);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Roh_Artikelnummer_Level1] SET [ClassificationId]=@ClassificationId, [IncludeInDescription]=@IncludeInDescription, [Name]=@Name, [OrderInDescription]=@OrderInDescription, [Part]=@Part, [PartOrder]=@PartOrder, [Seperator]=@Seperator, [ValueAtBeginningOfDescription]=@ValueAtBeginningOfDescription, [ValueInDescription]=@ValueInDescription WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("ClassificationId", item.ClassificationId == null ? (object)DBNull.Value : item.ClassificationId);
			sqlCommand.Parameters.AddWithValue("IncludeInDescription", item.IncludeInDescription == null ? (object)DBNull.Value : item.IncludeInDescription);
			sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? (object)DBNull.Value : item.Name);
			sqlCommand.Parameters.AddWithValue("OrderInDescription", item.OrderInDescription == null ? (object)DBNull.Value : item.OrderInDescription);
			sqlCommand.Parameters.AddWithValue("Part", item.Part == null ? (object)DBNull.Value : item.Part);
			sqlCommand.Parameters.AddWithValue("PartOrder", item.PartOrder == null ? (object)DBNull.Value : item.PartOrder);
			sqlCommand.Parameters.AddWithValue("Seperator", item.Seperator == null ? (object)DBNull.Value : item.Seperator);
			sqlCommand.Parameters.AddWithValue("ValueAtBeginningOfDescription", item.ValueAtBeginningOfDescription == null ? (object)DBNull.Value : item.ValueAtBeginningOfDescription);
			sqlCommand.Parameters.AddWithValue("ValueInDescription", item.ValueInDescription == null ? (object)DBNull.Value : item.ValueInDescription);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Roh_Artikelnummer_Level1] SET "

					+ "[ClassificationId]=@ClassificationId" + i + ","
					+ "[IncludeInDescription]=@IncludeInDescription" + i + ","
					+ "[Name]=@Name" + i + ","
					+ "[OrderInDescription]=@OrderInDescription" + i + ","
					+ "[Part]=@Part" + i + ","
					+ "[PartOrder]=@PartOrder" + i + ","
					+ "[Seperator]=@Seperator" + i + ","
					+ "[ValueAtBeginningOfDescription]=@ValueAtBeginningOfDescription" + i + ","
					+ "[ValueInDescription]=@ValueInDescription" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("ClassificationId" + i, item.ClassificationId == null ? (object)DBNull.Value : item.ClassificationId);
					sqlCommand.Parameters.AddWithValue("IncludeInDescription" + i, item.IncludeInDescription == null ? (object)DBNull.Value : item.IncludeInDescription);
					sqlCommand.Parameters.AddWithValue("Name" + i, item.Name == null ? (object)DBNull.Value : item.Name);
					sqlCommand.Parameters.AddWithValue("OrderInDescription" + i, item.OrderInDescription == null ? (object)DBNull.Value : item.OrderInDescription);
					sqlCommand.Parameters.AddWithValue("Part" + i, item.Part == null ? (object)DBNull.Value : item.Part);
					sqlCommand.Parameters.AddWithValue("PartOrder" + i, item.PartOrder == null ? (object)DBNull.Value : item.PartOrder);
					sqlCommand.Parameters.AddWithValue("Seperator" + i, item.Seperator == null ? (object)DBNull.Value : item.Seperator);
					sqlCommand.Parameters.AddWithValue("ValueAtBeginningOfDescription" + i, item.ValueAtBeginningOfDescription == null ? (object)DBNull.Value : item.ValueAtBeginningOfDescription);
					sqlCommand.Parameters.AddWithValue("ValueInDescription" + i, item.ValueInDescription == null ? (object)DBNull.Value : item.ValueInDescription);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Roh_Artikelnummer_Level1] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [Roh_Artikelnummer_Level1] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity GetByNamePartOrderNadPart(string name, int partOrder, string part)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Roh_Artikelnummer_Level1] WHERE [Name]=@name AND [PartOrder]=@partOrder AND [Part]=@part";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("name", name);
				sqlCommand.Parameters.AddWithValue("partOrder", partOrder);
				sqlCommand.Parameters.AddWithValue("part", part);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.Roh_Artikelnummer_Level1Entity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		#endregion Custom Methods
	}
}
