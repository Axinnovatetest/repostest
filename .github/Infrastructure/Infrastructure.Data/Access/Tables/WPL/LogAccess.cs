using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.WPL
{
	public class LogAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.WPL.LogEntity Get(int id)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM Log WHERE Id=@Id;";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Id", id);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.LogEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.LogEntity> Get()
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM Log";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.LogEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.LogEntity> Get(List<int> ids)
		{
			if(ids == null || ids.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.LogEntity>();
			}

			int max = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;

			if(ids.Count <= max)
			{
				return get(ids);
			}

			int batchNumber = ids.Count / max;
			var result = new List<Infrastructure.Data.Entities.Tables.WPL.LogEntity>();
			for(int i = 0; i < batchNumber; i++)
			{
				result.AddRange(get(ids.GetRange(i * max, max)));
			}
			result.AddRange(get(ids.GetRange(batchNumber * max, ids.Count - batchNumber * max)));
			return result;
		}
		private static List<Infrastructure.Data.Entities.Tables.WPL.LogEntity> get(List<int> ids)
		{
			if(ids == null || ids.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.LogEntity>();
			}

			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
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

			sqlCommand.CommandText = "SELECT * FROM Log WHERE Id IN (" + queryIds + ")";


			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dt = new DataTable();
			selectAdapter.Fill(dt);
			if(dt.Rows.Count > 0)
			{
				return toList(dt);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.LogEntity>();
			}
		}
		public static int Insert(Infrastructure.Data.Entities.Tables.WPL.LogEntity element)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "INSERT INTO Log (Creation_Date,Creation_User_Id,Action,Type) " +
										" VALUES (@Creation_Date,@Creation_User_Id,@Action,@Type);";
			query += "SELECT SCOPE_IDENTITY();";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("Creation_Date", element.CreationTime);
			sqlCommand.Parameters.AddWithValue("Creation_User_Id", element.CreationUserId);
			sqlCommand.Parameters.AddWithValue("Action", element.Action);
			sqlCommand.Parameters.AddWithValue("Type", element.Type);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			var response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;

			sqlConnection.Close();

			return response;
		}
		public static int Update(Infrastructure.Data.Entities.Tables.WPL.LogEntity element)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "UPDATE Log SET Creation_Date=@Creation_Date,Creation_User_Id=@Creation_User_Id," +
				"Action=@Action,Type=@Type WHERE Id=@Id";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Id", element.Id);
			sqlCommand.Parameters.AddWithValue("Creation_Date", element.CreationTime);
			sqlCommand.Parameters.AddWithValue("Creation_User_Id", element.CreationUserId);
			sqlCommand.Parameters.AddWithValue("Action", element.Action);
			sqlCommand.Parameters.AddWithValue("Type", element.Type);

			int response = DbExecution.ExecuteNonQuery(sqlCommand);

			sqlConnection.Close();

			return response;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.WPL.LogEntity> elements)
		{
			if(elements != null && elements.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 10; // Nb params per query
				int result = 0;
				if(elements.Count <= maxParamsNumber)
				{
					result = update(elements);
				}
				else
				{
					int batchNumber = elements.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						result += update(elements.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					result += update(elements.GetRange(batchNumber * maxParamsNumber, elements.Count - batchNumber * maxParamsNumber));
				}
			}

			return -1;
		}
		private static int update(List<Infrastructure.Data.Entities.Tables.WPL.LogEntity> elements)
		{
			if(elements != null && elements.Count > 0)
			{
				var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
				sqlConnection.Open();

				string query = "";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				int i = 0;
				foreach(var element in elements)
				{
					i++;
					query += " UPDATE Log SET "

						+ "Creation_Date=@Creation_Date" + i + ","
						+ "Creation_User_Id=@Creation_User_Id" + i + ","
						+ "Action=@Action" + i + ","
						+ "Type=@Type" + i + ","
						+ " WHERE Id=@Id" + i
					+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, element.Id);
					sqlCommand.Parameters.AddWithValue("Creation_Date" + i, element.CreationTime);
					sqlCommand.Parameters.AddWithValue("Creation_User_Id" + i, element.CreationUserId);
					sqlCommand.Parameters.AddWithValue("Action" + i, element.Action);
					sqlCommand.Parameters.AddWithValue("Type" + i, element.Type);
				}

				sqlCommand.CommandText = query;

				int response = DbExecution.ExecuteNonQuery(sqlCommand);

				sqlConnection.Close();

				return response;
			}

			return -1;
		}
		public static int Delete(int id)
		{
			var sqlConection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConection.Open();

			string query = "DELETE FROM Log WHERE Id=@Id";

			var sqlCommand = new SqlCommand(query, sqlConection);
			sqlCommand.Parameters.AddWithValue("Id", id);

			int response = DbExecution.ExecuteNonQuery(sqlCommand);

			sqlConection.Close();

			return response;
		}
		public static int Delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;

				if(ids.Count <= maxParamsNumber)
				{
					return delete(ids);
				}
				int result = 0;
				int batchNumber = ids.Count / maxParamsNumber;
				for(int i = 0; i < batchNumber; i++)
				{
					result += delete(ids.GetRange(i * maxParamsNumber, maxParamsNumber));
				}
				result += delete(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber));
				return result;
			}
			return -1;
		}
		private static int delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
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

				string query = "DELETE FROM Log WHERE Id IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				int response = DbExecution.ExecuteNonQuery(sqlCommand);

				sqlConnection.Close();

				return response;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.WPL.LogEntity> GetByType(int Type)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT TOP 2000 * FROM Log Where Type=@Type Order By Id DESC";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Type", Type);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.LogEntity>();
			}
		}
		#endregion

		#region Helpers
		private static List<Infrastructure.Data.Entities.Tables.WPL.LogEntity> toList(DataTable dataTable)
		{
			var result = new List<Infrastructure.Data.Entities.Tables.WPL.LogEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{
				result.Add(new Infrastructure.Data.Entities.Tables.WPL.LogEntity(dataRow));
			}
			return result;
		}
		#endregion
	}
}
