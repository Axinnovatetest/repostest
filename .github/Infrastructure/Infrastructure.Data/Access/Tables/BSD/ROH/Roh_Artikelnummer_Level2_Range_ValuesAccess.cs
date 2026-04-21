using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class Roh_Artikelnummer_Level2_Range_ValuesAccess
	{
		#region Default Methods
		public static Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Roh_Artikelnummer_Level2_Range_Values] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Roh_Artikelnummer_Level2_Range_Values]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity>();
			}
		}
		public static List<Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity>();
		}
		private static List<Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Roh_Artikelnummer_Level2_Range_Values] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity(x)).ToList();
				}
				else
				{
					return new List<Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity>();
				}
			}
			return new List<Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity>();
		}

		public static int Insert(Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Roh_Artikelnummer_Level2_Range_Values] ([FromOrTwo],[IdLevelTwo],[RangeValue]) OUTPUT INSERTED.[Id] VALUES (@FromOrTwo,@IdLevelTwo,@RangeValue); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("FromOrTwo", item.FromOrTwo == null ? (object)DBNull.Value : item.FromOrTwo);
					sqlCommand.Parameters.AddWithValue("IdLevelTwo", item.IdLevelTwo == null ? (object)DBNull.Value : item.IdLevelTwo);
					sqlCommand.Parameters.AddWithValue("RangeValue", item.RangeValue == null ? (object)DBNull.Value : item.RangeValue);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 4; // Nb params per query
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
		private static int insert(List<Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity> items)
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
						query += " INSERT INTO [Roh_Artikelnummer_Level2_Range_Values] ([FromOrTwo],[IdLevelTwo],[RangeValue]) VALUES ( "

							+ "@FromOrTwo" + i + ","
							+ "@IdLevelTwo" + i + ","
							+ "@RangeValue" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("FromOrTwo" + i, item.FromOrTwo == null ? (object)DBNull.Value : item.FromOrTwo);
						sqlCommand.Parameters.AddWithValue("IdLevelTwo" + i, item.IdLevelTwo == null ? (object)DBNull.Value : item.IdLevelTwo);
						sqlCommand.Parameters.AddWithValue("RangeValue" + i, item.RangeValue == null ? (object)DBNull.Value : item.RangeValue);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Roh_Artikelnummer_Level2_Range_Values] SET [FromOrTwo]=@FromOrTwo, [IdLevelTwo]=@IdLevelTwo, [RangeValue]=@RangeValue WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("FromOrTwo", item.FromOrTwo == null ? (object)DBNull.Value : item.FromOrTwo);
				sqlCommand.Parameters.AddWithValue("IdLevelTwo", item.IdLevelTwo == null ? (object)DBNull.Value : item.IdLevelTwo);
				sqlCommand.Parameters.AddWithValue("RangeValue", item.RangeValue == null ? (object)DBNull.Value : item.RangeValue);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 4; // Nb params per query
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
		private static int update(List<Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity> items)
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
						query += " UPDATE [Roh_Artikelnummer_Level2_Range_Values] SET "

							+ "[FromOrTwo]=@FromOrTwo" + i + ","
							+ "[IdLevelTwo]=@IdLevelTwo" + i + ","
							+ "[RangeValue]=@RangeValue" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("FromOrTwo" + i, item.FromOrTwo == null ? (object)DBNull.Value : item.FromOrTwo);
						sqlCommand.Parameters.AddWithValue("IdLevelTwo" + i, item.IdLevelTwo == null ? (object)DBNull.Value : item.IdLevelTwo);
						sqlCommand.Parameters.AddWithValue("RangeValue" + i, item.RangeValue == null ? (object)DBNull.Value : item.RangeValue);
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
				string query = "DELETE FROM [Roh_Artikelnummer_Level2_Range_Values] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [Roh_Artikelnummer_Level2_Range_Values] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Roh_Artikelnummer_Level2_Range_Values] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Roh_Artikelnummer_Level2_Range_Values]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity>();
			}
		}
		public static List<Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity>();
		}
		private static List<Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Roh_Artikelnummer_Level2_Range_Values] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity(x)).ToList();
				}
				else
				{
					return new List<Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity>();
				}
			}
			return new List<Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity>();
		}

		public static int InsertWithTransaction(Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [Roh_Artikelnummer_Level2_Range_Values] ([FromOrTwo],[IdLevelTwo],[RangeValue]) OUTPUT INSERTED.[Id] VALUES (@FromOrTwo,@IdLevelTwo,@RangeValue); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("FromOrTwo", item.FromOrTwo == null ? (object)DBNull.Value : item.FromOrTwo);
			sqlCommand.Parameters.AddWithValue("IdLevelTwo", item.IdLevelTwo == null ? (object)DBNull.Value : item.IdLevelTwo);
			sqlCommand.Parameters.AddWithValue("RangeValue", item.RangeValue == null ? (object)DBNull.Value : item.RangeValue);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 4; // Nb params per query
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
		private static int insertWithTransaction(List<Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Roh_Artikelnummer_Level2_Range_Values] ([FromOrTwo],[IdLevelTwo],[RangeValue]) VALUES ( "

						+ "@FromOrTwo" + i + ","
						+ "@IdLevelTwo" + i + ","
						+ "@RangeValue" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("FromOrTwo" + i, item.FromOrTwo == null ? (object)DBNull.Value : item.FromOrTwo);
					sqlCommand.Parameters.AddWithValue("IdLevelTwo" + i, item.IdLevelTwo == null ? (object)DBNull.Value : item.IdLevelTwo);
					sqlCommand.Parameters.AddWithValue("RangeValue" + i, item.RangeValue == null ? (object)DBNull.Value : item.RangeValue);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Roh_Artikelnummer_Level2_Range_Values] SET [FromOrTwo]=@FromOrTwo, [IdLevelTwo]=@IdLevelTwo, [RangeValue]=@RangeValue WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("FromOrTwo", item.FromOrTwo == null ? (object)DBNull.Value : item.FromOrTwo);
			sqlCommand.Parameters.AddWithValue("IdLevelTwo", item.IdLevelTwo == null ? (object)DBNull.Value : item.IdLevelTwo);
			sqlCommand.Parameters.AddWithValue("RangeValue", item.RangeValue == null ? (object)DBNull.Value : item.RangeValue);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 4; // Nb params per query
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
		private static int updateWithTransaction(List<Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Roh_Artikelnummer_Level2_Range_Values] SET "

					+ "[FromOrTwo]=@FromOrTwo" + i + ","
					+ "[IdLevelTwo]=@IdLevelTwo" + i + ","
					+ "[RangeValue]=@RangeValue" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("FromOrTwo" + i, item.FromOrTwo == null ? (object)DBNull.Value : item.FromOrTwo);
					sqlCommand.Parameters.AddWithValue("IdLevelTwo" + i, item.IdLevelTwo == null ? (object)DBNull.Value : item.IdLevelTwo);
					sqlCommand.Parameters.AddWithValue("RangeValue" + i, item.RangeValue == null ? (object)DBNull.Value : item.RangeValue);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Roh_Artikelnummer_Level2_Range_Values] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [Roh_Artikelnummer_Level2_Range_Values] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity> GetByLevelTwoId(int levelTwoId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Roh_Artikelnummer_Level2_Range_Values] WHERE [IdLevelTwo]=@levelTwoId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("levelTwoId", levelTwoId);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.BSD.Roh_Artikelnummer_Level2_Range_ValuesEntity>();
			}
		}
		#endregion Custom Methods

	}
}
