using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class Assign_budget_landAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Assign_budget_land] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Assign_budget_land]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Assign_budget_land] WHERE [ID] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Assign_budget_land] ([B_year],[budget],[Land_name],[LandId],[TotalSpent])  VALUES (@B_year,@budget,@Land_name,@LandId,@TotalSpent); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("B_year", item.B_year == null ? (object)DBNull.Value : item.B_year);
					sqlCommand.Parameters.AddWithValue("budget", item.budget == null ? (object)DBNull.Value : item.budget);
					sqlCommand.Parameters.AddWithValue("Land_name", item.Land_name == null ? (object)DBNull.Value : item.Land_name);
					sqlCommand.Parameters.AddWithValue("LandId", item.LandId == null ? (object)DBNull.Value : item.LandId);
					sqlCommand.Parameters.AddWithValue("TotalSpent", item.TotalSpent == null ? (object)DBNull.Value : item.TotalSpent);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 6; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity> items)
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
						query += " INSERT INTO [Assign_budget_land] ([B_year],[budget],[Land_name],[LandId],[TotalSpent]) VALUES ( "

							+ "@B_year" + i + ","
							+ "@budget" + i + ","
							+ "@Land_name" + i + ","
							+ "@LandId" + i + ","
							+ "@TotalSpent" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("B_year" + i, item.B_year == null ? (object)DBNull.Value : item.B_year);
						sqlCommand.Parameters.AddWithValue("budget" + i, item.budget == null ? (object)DBNull.Value : item.budget);
						sqlCommand.Parameters.AddWithValue("Land_name" + i, item.Land_name == null ? (object)DBNull.Value : item.Land_name);
						sqlCommand.Parameters.AddWithValue("LandId" + i, item.LandId == null ? (object)DBNull.Value : item.LandId);
						sqlCommand.Parameters.AddWithValue("TotalSpent" + i, item.TotalSpent == null ? (object)DBNull.Value : item.TotalSpent);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [Assign_budget_land] SET [B_year]=@B_year, [budget]=@budget, [Land_name]=@Land_name, [LandId]=@LandId, [TotalSpent]=@TotalSpent WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("B_year", item.B_year == null ? (object)DBNull.Value : item.B_year);
				sqlCommand.Parameters.AddWithValue("budget", item.budget == null ? (object)DBNull.Value : item.budget);
				sqlCommand.Parameters.AddWithValue("Land_name", item.Land_name == null ? (object)DBNull.Value : item.Land_name);
				sqlCommand.Parameters.AddWithValue("LandId", item.LandId == null ? (object)DBNull.Value : item.LandId);
				sqlCommand.Parameters.AddWithValue("TotalSpent", item.TotalSpent == null ? (object)DBNull.Value : item.TotalSpent);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 6; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity> items)
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
						query += " UPDATE [Assign_budget_land] SET "

							+ "[B_year]=@B_year" + i + ","
							+ "[budget]=@budget" + i + ","
							+ "[Land_name]=@Land_name" + i + ","
							+ "[LandId]=@LandId" + i + ","
							+ "[TotalSpent]=@TotalSpent" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("B_year" + i, item.B_year == null ? (object)DBNull.Value : item.B_year);
						sqlCommand.Parameters.AddWithValue("budget" + i, item.budget == null ? (object)DBNull.Value : item.budget);
						sqlCommand.Parameters.AddWithValue("Land_name" + i, item.Land_name == null ? (object)DBNull.Value : item.Land_name);
						sqlCommand.Parameters.AddWithValue("LandId" + i, item.LandId == null ? (object)DBNull.Value : item.LandId);
						sqlCommand.Parameters.AddWithValue("TotalSpent" + i, item.TotalSpent == null ? (object)DBNull.Value : item.TotalSpent);
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
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Assign_budget_land] WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ID", id);

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

					string query = "DELETE FROM [Assign_budget_land] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion


		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity> Get(string Land_name, int year)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [assign_budget_land] WHERE [Land_name]=@Land_name and [B_year]=@year and [budget] IS NOT NULL and [budget] <> 0";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Land_name", Land_name);
				sqlCommand.Parameters.AddWithValue("year", year);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static int GetDeptsCount(string Land_name, int? Year)
		{
			var dataTable = new DataTable();
			int respone = 0;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "select count(Departement_name)  AS DEPT_COUNTS from [assign_budget_departement] where Land_name=@Land_name and B_year=@Year";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Land_name", Land_name);
				sqlCommand.Parameters.AddWithValue("Year", Year);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				respone = Convert.ToInt32(dataTable.Rows[0]["DEPT_COUNTS"].ToString());
				// return toList2(dataTable);
			}
			return respone;
		}
		//public static float GetSumBudgetDepartements(string Land_name, int? Year)
		public static float GetSumBudgetDepartements(int? Land, int? Year)

		{
			var dataTable = new DataTable();
			float respone = 0;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				//string query = "select ISNULL(sum(budget),0) AS SUM_BUDGET_DEPTS from [assign_budget_departement] where Land_name=@Land_name and B_year=@Year";
				string query = "select ISNULL(sum(budget),0) AS SUM_BUDGET_DEPTS from [assign_budget_departement] where LandId=@Land and B_year=@Year";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				//sqlCommand.Parameters.AddWithValue("Land_name", Land_name);
				sqlCommand.Parameters.AddWithValue("Land", Land);
				sqlCommand.Parameters.AddWithValue("Year", Year);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				string rps = dataTable.Rows[0]["SUM_BUDGET_DEPTS"].ToString();
				respone = float.Parse(rps, CultureInfo.InvariantCulture.NumberFormat);
			}
			return respone;
		}

		public static float GetSumBudgetLandSupplements(int ID)
		{
			var dataTable = new DataTable();
			float respone = 0;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "select isnull(SUM(Supplement_Budget),0) as LandBudgetSupplement from [Supplement_Budget_Land] SL where id_AL in (select AL.ID from [assign_budget_land] AL where ID=@ID)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ID", ID);


				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				string rps = dataTable.Rows[0]["LandBudgetSupplement"].ToString();
				respone = float.Parse(rps, CultureInfo.InvariantCulture.NumberFormat);
				// return toList2(dataTable);
			}
			return respone;
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.AllDataAssign_budget_landEntity> GetByUserAllDataLand(int user_id, bool isGlobalDirector = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				string query = @"select AL.ID,AL.Land_name,AL.budget,AL.B_year,AL.LandId,AL.TotalSpent,isnull(T.SommeSupplement, 0) as SommeSupplement ,isnull(T1.SommebudgetDept, 0) as SommebudgetDept,
                isnull(T.SommeSupplement, 0) + Al.budget as SommebudgetSupplement,
                (isnull(T.SommeSupplement, 0) + Al.budget) - isnull(T1.SommebudgetDept, 0) as NotAssignedBudgetDept
                from [assign_budget_land] AL
                Left join(select Id_Al, isnull(sum(Supplement_Budget), 0) as SommeSupplement from[Supplement_Budget_Land]
                group by id_AL) T on AL.ID = T.Id_AL
                Left join(select LandId, B_year, isnull(sum(budget),0) as SommebudgetDept from[Assign_budget_departement]
                group by LandId,B_year) T1 on T1.LandId = Al.LandId and T1.B_year = Al.B_year 
                ";

				if(isGlobalDirector)
				{
					query += "where AL.LandId in (select[Land_User_Joint].ID_Land from [Land_User_Joint] )";
				}
				else
				{
					query += "where AL.LandId in (select[Land_User_Joint].ID_Land from [Land_User_Joint] where [Land_User_Joint].[ID_user] = @user_id )";
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("user_id", user_id);
				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return toListAllData(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity GetIDbyName(string land, int? year)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				//string query = "SELECT * FROM [assign_budget_land] WHERE [Land_name]=@land and [B_year]=@year";
				string query = "IF EXISTS(SELECT * FROM [assign_budget_land] WHERE[Land_name]=@land and [B_year]=@year)SELECT * FROM [assign_budget_land] WHERE[Land_name]=@land and [B_year]=@year ELSE SELECT 0 as ID,''  as Land_name,0  as budget ,0  as B_year";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("land", land);
				sqlCommand.Parameters.AddWithValue("year", year);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity GetIDbyIdLand(int land, int? year)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "IF EXISTS( SELECT * FROM [assign_budget_land] WHERE[LandId]=@land and [B_year]=@year) SELECT * FROM [assign_budget_land] WHERE[LandId]=@land and [B_year]=@year ELSE SELECT 0 as ID,0  as LandId,0  as budget ,0  as B_year,0 as TotalSpent, '' as Land_name";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("land", land);
				sqlCommand.Parameters.AddWithValue("year", year);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity> GetByUser(int user_id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Assign_budget_land] where Land_name in (select Land_name from[Budget_lands] where ID in (select ID_Land from[Land_User_Joint] where[ID_user] = @user_id ))";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("user_id", user_id);
				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity GetUniqueByLand(string landName, int year)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [assign_budget_land] WHERE [Land_name]=@landName AND [B_year]=@year";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("landName", landName);
				sqlCommand.Parameters.AddWithValue("year", year);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity(dataRow)); }
			return list;
		}

		private static List<Infrastructure.Data.Entities.Tables.FNC.AllDataAssign_budget_landEntity> toListAllData(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.AllDataAssign_budget_landEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.AllDataAssign_budget_landEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
