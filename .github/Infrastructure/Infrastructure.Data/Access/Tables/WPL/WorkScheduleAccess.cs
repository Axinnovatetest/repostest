using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.WPL
{
	public class WorkPlanAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity Get(int id)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM WorkSchedule WHERE Id=@Id";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Id", id);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity> Get()
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM WorkSchedule";

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
				return new List<Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity> Get(List<int> ids)
		{
			if(ids == null || ids.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity>();
			}

			int max = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;

			if(ids.Count <= max)
			{
				return get(ids);
			}

			int batchNumber = ids.Count / max;
			var result = new List<Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity>();
			for(int i = 0; i < batchNumber; i++)
			{
				result.AddRange(get(ids.GetRange(i * max, max)));
			}
			result.AddRange(get(ids.GetRange(batchNumber * max, ids.Count - batchNumber * max)));
			return result;
		}
		private static List<Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity> get(List<int> ids)
		{
			if(ids == null || ids.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity>();
			}

			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			var sqlCommand = new SqlCommand
			{
				Connection = sqlConnection
			};

			string queryIds = string.Empty;
			for(int i = 0; i < ids.Count; i++)
			{
				queryIds += "@Id" + i + ",";
				sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
			}
			queryIds = queryIds.TrimEnd(',');

			sqlCommand.CommandText = "SELECT * FROM WorkSchedule WHERE Id IN (" + queryIds + ")";


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
				return new List<Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity>();
			}
		}
		public static int Insert(Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity element)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "INSERT INTO WorkSchedule (Name,Hall_Id,Article_Id,Creation_Date,Creation_User_Id,Is_Active,Last_Edit_Date,Last_Edit_User_Id)" +
										" VALUES (@Name,@HallId,@ArticleId,@CreationTime,@CreationUserId,@IsActive,@LastEditTime,@LastEditUserId);";
			query += "SELECT SCOPE_IDENTITY();";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Name", element.Name);
			sqlCommand.Parameters.AddWithValue("CreationUserId", element.CreationUserId);
			sqlCommand.Parameters.AddWithValue("HallId", element.HallId);
			sqlCommand.Parameters.AddWithValue("IsActive", element.IsActive);
			sqlCommand.Parameters.AddWithValue("ArticleId", element.ArticleId);
			sqlCommand.Parameters.AddWithValue("CreationTime", element.CreationTime);
			sqlCommand.Parameters.AddWithValue("LastEditUserId", element.LastEditUserId == null ? (object)DBNull.Value : element.LastEditUserId);
			sqlCommand.Parameters.AddWithValue("LastEditTime", element.LastEditTime == null ? (object)DBNull.Value : element.LastEditTime);

			var result = sqlCommand.ExecuteScalar();
			var response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;

			sqlConnection.Close();

			return response;
		}
		public static int Update(Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity element)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "UPDATE WorkSchedule SET Name=@Name,Creation_Date=@CreationTime,Creation_User_Id=@CreationUserId," +
						   "Hall_Id=@HallId,Last_Edit_Date=@LastEditTime,Last_Edit_User_Id=@LastEditUserId," +
						   "Article_Id=@ArticleId,Is_Active=@IsActive WHERE Id=@Id";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Id", element.Id);
			sqlCommand.Parameters.AddWithValue("Name", element.Name);
			sqlCommand.Parameters.AddWithValue("HallId", element.HallId);
			sqlCommand.Parameters.AddWithValue("ArticleId", element.ArticleId);
			sqlCommand.Parameters.AddWithValue("IsActive", element.IsActive);
			sqlCommand.Parameters.AddWithValue("CreationUserId", element.CreationUserId);
			sqlCommand.Parameters.AddWithValue("CreationTime", element.CreationTime);
			sqlCommand.Parameters.AddWithValue("LastEditTime", element.LastEditTime == null ? (object)DBNull.Value : element.LastEditTime);
			sqlCommand.Parameters.AddWithValue("LastEditUserId", element.LastEditUserId == null ? (object)DBNull.Value : element.LastEditUserId);

			int response = sqlCommand.ExecuteNonQuery();

			sqlConnection.Close();

			return response;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity> elements)
		{
			int result = 0;
			if(elements != null && elements.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE / 10; // Nb params per query

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

			return result;
		}
		private static int update(List<Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity> elements)
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
					query += " UPDATE WorkSchedule SET "

							+ "Name=@Name" + i + ","
							+ "Article_Id=@ArticleId" + i + ","
							+ "Creation_Date=@CreationTime" + i + ","
							+ "Creation_User_Id=@CreationUserId" + i + ","
							+ "Hall_Id=@HallId" + i + ","
							+ "Last_Edit_Date=@LastEditTime" + i + ","
							+ "Last_Edit_User_Id=@LastEditUserId" + i + ","
							+ "Is_Active=@IsActive" + i + " WHERE Id=@Id" + i
							+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, element.Id);
					sqlCommand.Parameters.AddWithValue("Name" + i, element.Name);
					sqlCommand.Parameters.AddWithValue("ArticleId" + i, element.ArticleId);
					sqlCommand.Parameters.AddWithValue("HallId" + i, element.HallId);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, element.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId" + i, element.CreationUserId);
					sqlCommand.Parameters.AddWithValue("IsActive" + i, element.IsActive);
					sqlCommand.Parameters.AddWithValue("LastEditTime" + i, element.LastEditTime == null ? (object)DBNull.Value : element.LastEditTime);
					sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, element.LastEditUserId == null ? (object)DBNull.Value : element.LastEditUserId);

				}

				sqlCommand.CommandText = query;

				int response = sqlCommand.ExecuteNonQuery();

				sqlConnection.Close();

				return response;
			}

			return -1;
		}
		public static int Delete(int id)
		{
			var sqlConection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConection.Open();

			string query = "DELETE FROM WorkSchedule WHERE Id=@Id";

			var sqlCommand = new SqlCommand(query, sqlConection);
			sqlCommand.Parameters.AddWithValue("Id", id);

			int response = sqlCommand.ExecuteNonQuery();

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

				var sqlCommand = new SqlCommand
				{
					Connection = sqlConnection
				};

				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				string query = "DELETE FROM WorkSchedule WHERE Id IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				int response = sqlCommand.ExecuteNonQuery();

				sqlConnection.Close();

				return response;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity> GetByHallIdArticleId(int HallId, int ArticleId)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM WorkSchedule WHERE Hall_Id=@HallId AND Article_Id=@ArticleId";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("HallId", HallId);
			sqlCommand.Parameters.AddWithValue("ArticleId", ArticleId);

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
				return new List<Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity>();
			}

		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity> GetByHallId(int HallId)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM WorkSchedule WHERE Hall_Id=@HallId";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("HallId", HallId);

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
				return new List<Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity> GetByHallIds(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;

				if(ids.Count <= maxParamsNumber)
				{
					return getByHallIds(ids);
				}
				var result = new List<Entities.Tables.WPL.WorkPlanEntity>();
				int batchNumber = ids.Count / maxParamsNumber;
				for(int i = 0; i < batchNumber; i++)
				{
					result.AddRange(getByHallIds(ids.GetRange(i * maxParamsNumber, maxParamsNumber)));
				}
				result.AddRange(getByHallIds(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber)));
				return result;
			}
			return new List<Entities.Tables.WPL.WorkPlanEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity> getByHallIds(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
				sqlConnection.Open();

				var sqlCommand = new SqlCommand
				{
					Connection = sqlConnection
				};

				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@HallId" + i + ",";
					sqlCommand.Parameters.AddWithValue("HallId" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				string query = "SELECT * FROM WorkSchedule WHERE Hall_Id IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

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
					return new List<Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity>();
				}
			}
			return new List<Entities.Tables.WPL.WorkPlanEntity>();
		}

		public static List<Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity> GetByArticleId(int ArticleId)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM WorkSchedule WHERE Article_Id=@ArticleId";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("ArticleId", ArticleId);

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
				return new List<Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity>();
			}
		}
		public static int CountByArticleId(int ArticleId)
		{
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT COUNT(*) FROM WorkSchedule WHERE Article_Id=@ArticleId";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ArticleId", ArticleId);
				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var x) ? x : 0;
			}
		}
		#endregion

		#region Helpers
		private static List<Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity> toList(DataTable dataTable)
		{
			var result = new List<Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{
				result.Add(new Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity(dataRow));
			}
			return result;
		}
		#endregion
	}
}
