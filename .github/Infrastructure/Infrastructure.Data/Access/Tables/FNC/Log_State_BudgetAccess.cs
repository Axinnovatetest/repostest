using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class Log_State_BudgetAccess
	{

		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity Get(int id_ls)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Log_State_Budget] WHERE [Id_LS]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id_ls);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Log_State_Budget]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [Log_State_Budget] WHERE [Id_LS] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity item)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Log_State_Budget] ([Action_date],[Id_proj],[Id_state],[Id_user])  VALUES (@Action_date,@Id_proj,@Id_state,@Id_user)";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Action_date", item.Action_date == null ? (object)DBNull.Value : item.Action_date);
					sqlCommand.Parameters.AddWithValue("Id_proj", item.Id_proj);
					sqlCommand.Parameters.AddWithValue("Id_state", item.Id_state);
					sqlCommand.Parameters.AddWithValue("Id_user", item.Id_user);

					DbExecution.ExecuteNonQuery(sqlCommand);
				}

				using(var sqlCommand = new SqlCommand("SELECT [Id_LS] FROM [Log_State_Budget] WHERE [Id_LS] = @@IDENTITY", sqlConnection, sqlTransaction))
				{
					response = int.Parse(DbExecution.ExecuteScalar(sqlCommand)?.ToString() ?? "-1");
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 5; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity> items)
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
						query += " INSERT INTO [Log_State_Budget] ([Action_date],[Id_proj],[Id_state],[Id_user]) VALUES ( "

							+ "@Action_date" + i + ","
							+ "@Id_proj" + i + ","
							+ "@Id_state" + i + ","
							+ "@Id_user" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Action_date" + i, item.Action_date == null ? (object)DBNull.Value : item.Action_date);
						sqlCommand.Parameters.AddWithValue("Id_proj" + i, item.Id_proj);
						sqlCommand.Parameters.AddWithValue("Id_state" + i, item.Id_state);
						sqlCommand.Parameters.AddWithValue("Id_user" + i, item.Id_user);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [Log_State_Budget] SET [Action_date]=@Action_date, [Id_proj]=@Id_proj, [Id_state]=@Id_state, [Id_user]=@Id_user WHERE [Id_LS]=@Id_LS";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id_LS", item.Id_LS);
				sqlCommand.Parameters.AddWithValue("Action_date", item.Action_date == null ? (object)DBNull.Value : item.Action_date);
				sqlCommand.Parameters.AddWithValue("Id_proj", item.Id_proj);
				sqlCommand.Parameters.AddWithValue("Id_state", item.Id_state);
				sqlCommand.Parameters.AddWithValue("Id_user", item.Id_user);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 5; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity> items)
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
						query += " UPDATE [Log_State_Budget] SET "

							+ "[Action_date]=@Action_date" + i + ","
							+ "[Id_proj]=@Id_proj" + i + ","
							+ "[Id_state]=@Id_state" + i + ","
							+ "[Id_user]=@Id_user" + i + " WHERE [Id_LS]=@Id_LS" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id_LS" + i, item.Id_LS);
						sqlCommand.Parameters.AddWithValue("Action_date" + i, item.Action_date == null ? (object)DBNull.Value : item.Action_date);
						sqlCommand.Parameters.AddWithValue("Id_proj" + i, item.Id_proj);
						sqlCommand.Parameters.AddWithValue("Id_state" + i, item.Id_state);
						sqlCommand.Parameters.AddWithValue("Id_user" + i, item.Id_user);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int id_ls)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Log_State_Budget] WHERE [Id_LS]=@Id_LS";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id_LS", id_ls);

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

					string query = "DELETE FROM [Log_State_Budget] WHERE [Id_LS] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity> GetByProjectIds(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getByProjectIds(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(getByProjectIds(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity> getByProjectIds(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [Log_State_Budget] WHERE [Id_proj] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity>();
		}

		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
