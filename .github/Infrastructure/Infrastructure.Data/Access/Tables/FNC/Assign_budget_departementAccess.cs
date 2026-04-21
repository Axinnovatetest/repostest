using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class Assign_budget_departementAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Assign_budget_departement] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Assign_budget_departement]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Assign_budget_departement] WHERE [ID] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Assign_budget_departement] ([B_year],[budget],[Departement_name],[DepartmentId],[Land_name],[LandId],[TotalSpent])  VALUES (@B_year,@budget,@Departement_name,@DepartmentId,@Land_name,@LandId,@TotalSpent); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("B_year", item.B_year == null ? (object)DBNull.Value : item.B_year);
					sqlCommand.Parameters.AddWithValue("budget", item.budget == null ? (object)DBNull.Value : item.budget);
					sqlCommand.Parameters.AddWithValue("Departement_name", item.Departement_name == null ? (object)DBNull.Value : item.Departement_name);
					sqlCommand.Parameters.AddWithValue("DepartmentId", item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
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
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 8; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity> items)
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
						query += " INSERT INTO [Assign_budget_departement] ([B_year],[budget],[Departement_name],[DepartmentId],[Land_name],[LandId],[TotalSpent]) VALUES ( "

							+ "@B_year" + i + ","
							+ "@budget" + i + ","
							+ "@Departement_name" + i + ","
							+ "@DepartmentId" + i + ","
							+ "@Land_name" + i + ","
							+ "@LandId" + i + ","
							+ "@TotalSpent" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("B_year" + i, item.B_year == null ? (object)DBNull.Value : item.B_year);
						sqlCommand.Parameters.AddWithValue("budget" + i, item.budget == null ? (object)DBNull.Value : item.budget);
						sqlCommand.Parameters.AddWithValue("Departement_name" + i, item.Departement_name == null ? (object)DBNull.Value : item.Departement_name);
						sqlCommand.Parameters.AddWithValue("DepartmentId" + i, item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
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

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [Assign_budget_departement] SET [B_year]=@B_year, [budget]=@budget, [Departement_name]=@Departement_name, [DepartmentId]=@DepartmentId, [Land_name]=@Land_name, [LandId]=@LandId, [TotalSpent]=@TotalSpent WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("B_year", item.B_year == null ? (object)DBNull.Value : item.B_year);
				sqlCommand.Parameters.AddWithValue("budget", item.budget == null ? (object)DBNull.Value : item.budget);
				sqlCommand.Parameters.AddWithValue("Departement_name", item.Departement_name == null ? (object)DBNull.Value : item.Departement_name);
				sqlCommand.Parameters.AddWithValue("DepartmentId", item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
				sqlCommand.Parameters.AddWithValue("Land_name", item.Land_name == null ? (object)DBNull.Value : item.Land_name);
				sqlCommand.Parameters.AddWithValue("LandId", item.LandId == null ? (object)DBNull.Value : item.LandId);
				sqlCommand.Parameters.AddWithValue("TotalSpent", item.TotalSpent == null ? (object)DBNull.Value : item.TotalSpent);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 8; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity> items)
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
						query += " UPDATE [Assign_budget_departement] SET "

							+ "[B_year]=@B_year" + i + ","
							+ "[budget]=@budget" + i + ","
							+ "[Departement_name]=@Departement_name" + i + ","
							+ "[DepartmentId]=@DepartmentId" + i + ","
							+ "[Land_name]=@Land_name" + i + ","
							+ "[LandId]=@LandId" + i + ","
							+ "[TotalSpent]=@TotalSpent" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("B_year" + i, item.B_year == null ? (object)DBNull.Value : item.B_year);
						sqlCommand.Parameters.AddWithValue("budget" + i, item.budget == null ? (object)DBNull.Value : item.budget);
						sqlCommand.Parameters.AddWithValue("Departement_name" + i, item.Departement_name == null ? (object)DBNull.Value : item.Departement_name);
						sqlCommand.Parameters.AddWithValue("DepartmentId" + i, item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
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
				string query = "DELETE FROM [Assign_budget_departement] WHERE [ID]=@ID";
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

					string query = "DELETE FROM [Assign_budget_departement] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity> Get(string Land_name, int year)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Assign_budget_departement] WHERE [Land_name]=@Land_name and [B_year]=@year and [budget] IS NOT NULL and [budget] <> 0";
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

		public static List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity> Get(string Land_name, int year, int user_id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				//string query = "SELECT * FROM [Assign_budget_departement] WHERE [Land_name]=@Land_name and [B_year]=@year and [budget] IS NOT NULL and [budget] <> 0 and ID in (select ID_Departement from [Departement_User_Joint] where [ID_user]=@user_id )";
				//string query = "SELECT * FROM [Assign_budget_departement] where [Land_name]=@Land_name and [B_year]=@year and [budget] IS NOT NULL and[budget] <> 0  and Departement_name in (select Departement_name from [Budget_departement] where ID in (select ID_Departement from [Departement_User_Joint] where [ID_user]=@user_id ))";
				string query = "SELECT * FROM [Assign_budget_departement] where [Land_name]=@Land_name and [B_year]=@year and [budget] IS NOT NULL and[budget] <> 0  and Departement_name in (select Departement_name from [Budget_departement] where ID in (select ID_Department from [Land_Department_Joint] where [ID] in (select ID_Departement from[Departement_User_Joint] where[ID_user] =@user_id)))";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Land_name", Land_name);
				sqlCommand.Parameters.AddWithValue("year", year);
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

		//*****
		public static List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity> GetByUser(int user_id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				//string query = "SELECT * FROM [Assign_budget_departement] WHERE [Land_name]=@Land_name and [B_year]=@year and [budget] IS NOT NULL and [budget] <> 0 and ID in (select ID_Departement from [Departement_User_Joint] where [ID_user]=@user_id )";
				//string query = "SELECT * FROM [Assign_budget_departement] where [Land_name]=@Land_name and [B_year]=@year and [budget] IS NOT NULL and[budget] <> 0  and Departement_name in (select Departement_name from [Budget_departement] where ID in (select ID_Departement from [Departement_User_Joint] where [ID_user]=@user_id ))";
				//string query = "SELECT AD.ID,AD.Departement_name,AD.Land_name,AD.budget,AD.B_year FROM [Assign_budget_departement] AD,[Departement_User_Joint] DU,[Budget_departement] D WHERE DU.ID_Departement=D.[ID] and AD.[Departement_name]=D.[Departement_name] and DU.[ID_user]=@user_id )";

				//string query = "SELECT * FROM [Assign_budget_departement] where Departement_name in (select Departement_name from[Budget_departement] where ID in (select ID_Departement from[Departement_User_Joint] where[ID_user] = @user_id ))";
				string query = "SELECT * FROM [Assign_budget_departement] where Departement_name in (select Departement_name from[Budget_departement] where ID in (select ID_Department from[Land_Department_Joint] where[ID] in (select ID_Departement from[Departement_User_Joint] where[ID_user] = @user_id )))";
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

		public static List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity> GetByUserLand(int user_id, string land)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				//string query = "SELECT * FROM [Assign_budget_departement] WHERE [Land_name]=@Land_name and [B_year]=@year and [budget] IS NOT NULL and [budget] <> 0 and ID in (select ID_Departement from [Departement_User_Joint] where [ID_user]=@user_id )";
				//string query = "SELECT * FROM [Assign_budget_departement] where [Land_name]=@Land_name and [B_year]=@year and [budget] IS NOT NULL and[budget] <> 0  and Departement_name in (select Departement_name from [Budget_departement] where ID in (select ID_Departement from [Departement_User_Joint] where [ID_user]=@user_id ))";
				//string query = "SELECT AD.ID,AD.Departement_name,AD.Land_name,AD.budget,AD.B_year FROM [Assign_budget_departement] AD,[Departement_User_Joint] DU,[Budget_departement] D WHERE DU.ID_Departement=D.[ID] and AD.[Departement_name]=D.[Departement_name] and DU.[ID_user]=@user_id )";

				string query = "SELECT * FROM [Assign_budget_departement] where Departement_name in (select Departement_name from[Budget_departement] where ID in (select ID_Departement from[Departement_User_Joint] where[ID_user] = @user_id )) and Land_name=@land";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("user_id", user_id);
				sqlCommand.Parameters.AddWithValue("land", land);

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
		public static Infrastructure.Data.Entities.Tables.FNC.checkDeptBdgEntity CheckBudgetDept(string Land_name, int? Year, int ID, string dept_name)
		{
			var dataTable = new DataTable();
			var dataTable_2 = new DataTable();
			var dataTable_3 = new DataTable();

			var response = new Infrastructure.Data.Entities.Tables.FNC.checkDeptBdgEntity();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "select isnull(SUM(AL.budget),0) as LandBudget from [assign_budget_land] AL where AL.Land_name=@Land_name and AL.B_year=@Year";
				//string query_2 = "select isnull(SUM(AD.budget),0) as SOMME_DEPT from [assign_budget_departement] AD where AD.Land_name=@Land_name and AD.B_year=@Year";
				string query_2 = "IF EXISTS (select isnull(SUM(AD.budget),0) as SOMME_DEPT from [assign_budget_departement] AD where AD.Land_name=@Land_name and AD.B_year=@Year)select isnull(SUM(AD.budget),0) as SOMME_DEPT from [assign_budget_departement] AD where AD.Land_name=@Land_name and AD.B_year=@Year ELSE SELECT 0 as SOMME_DEPT";
				string query_3 = "select isnull(SUM(Supplement_Budget),0) as LandBudgetSupplement from [Supplement_Budget_Land] SL where id_AL in (select AL.ID from [assign_budget_land] AL where ID=@ID)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Land_name", Land_name);
				sqlCommand.Parameters.AddWithValue("Year", Year);

				DbExecution.Fill(sqlCommand, dataTable);

				//******
				var sqlCommand_2 = new SqlCommand(query_2, sqlConnection);
				sqlCommand_2.Parameters.AddWithValue("Land_name", Land_name);
				sqlCommand_2.Parameters.AddWithValue("Year", Year);

				new SqlDataAdapter(sqlCommand_2).Fill(dataTable_2);

				var sqlCommand_3 = new SqlCommand(query_3, sqlConnection);
				sqlCommand_3.Parameters.AddWithValue("ID", ID);


				new SqlDataAdapter(sqlCommand_3).Fill(dataTable_3);
			}
			string exsist = CheckDeptExsistance(Land_name, dept_name, Year);
			response.DPT = exsist;
			if(dataTable.Rows.Count > 0)
			{
				/*var v1 = dataTable.Rows[0]["LandBudget"].ToString();
                var v2 = dataTable.Rows[0]["SOMME_DEPT"].ToString();
                 if (v1 == "" && v2 == "") {
                    response.LandBudget = null;
                    response.SOMME_DEPT = null;
                        }
                else { 
                response.LandBudget = float.Parse(dataTable.Rows[0]["LandBudget"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                response.SOMME_DEPT = float.Parse(dataTable.Rows[0]["SOMME_DEPT"].ToString(), System.Globalization.CultureInfo.InvariantCulture);

                    // return toList2(dataTable);
                }*/
				response.LandBudgetSupplement = float.Parse(dataTable_3.Rows[0]["LandBudgetSupplement"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
				response.LandBudget = float.Parse(dataTable.Rows[0]["LandBudget"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
				response.SOMME_DEPT = float.Parse(dataTable_2.Rows[0]["SOMME_DEPT"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
			}
			return response;
		}

		public static Infrastructure.Data.Entities.Tables.FNC.checkDeptBdgEntity CheckBudgetDeptbyId(int? Land, int? Year, int ID, int? dept)
		{
			var dataTable = new DataTable();
			var dataTable_2 = new DataTable();
			var dataTable_3 = new DataTable();

			var response = new Infrastructure.Data.Entities.Tables.FNC.checkDeptBdgEntity();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "select isnull(SUM(AL.budget),0) as LandBudget from [assign_budget_land] AL where AL.LandId=@Land and AL.B_year=@Year";
				string query_2 = "IF EXISTS (select isnull(SUM(AD.budget),0) as SOMME_DEPT from [assign_budget_departement] AD where AD.LandId=@Land and AD.B_year=@Year)select isnull(SUM(AD.budget),0) as SOMME_DEPT from [assign_budget_departement] AD where AD.LandId=@Land and AD.B_year=@Year ELSE SELECT 0 as SOMME_DEPT";
				string query_3 = "select isnull(SUM(Supplement_Budget),0) as LandBudgetSupplement from [Supplement_Budget_Land] SL where id_AL in (select AL.ID from [assign_budget_land] AL where ID=@ID)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Land", Land);
				sqlCommand.Parameters.AddWithValue("Year", Year);

				DbExecution.Fill(sqlCommand, dataTable);

				//******
				var sqlCommand_2 = new SqlCommand(query_2, sqlConnection);
				sqlCommand_2.Parameters.AddWithValue("Land", Land);
				sqlCommand_2.Parameters.AddWithValue("Year", Year);

				new SqlDataAdapter(sqlCommand_2).Fill(dataTable_2);

				var sqlCommand_3 = new SqlCommand(query_3, sqlConnection);
				sqlCommand_3.Parameters.AddWithValue("ID", ID);


				new SqlDataAdapter(sqlCommand_3).Fill(dataTable_3);
			}
			string exsist = CheckDeptExsistancebyId(Land, dept, Year);
			response.DPT = exsist;
			if(dataTable.Rows.Count > 0)
			{

				response.LandBudgetSupplement = float.Parse(dataTable_3.Rows[0]["LandBudgetSupplement"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
				response.LandBudget = float.Parse(dataTable.Rows[0]["LandBudget"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
				response.SOMME_DEPT = float.Parse(dataTable_2.Rows[0]["SOMME_DEPT"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
			}
			return response;
		}
		public static Infrastructure.Data.Entities.Tables.FNC.checkDeptBdgEntity SumBudgetDept(string Land_name, int? Year)
		{
			var dataTable = new DataTable();
			var dataTable_2 = new DataTable();

			var response = new Infrastructure.Data.Entities.Tables.FNC.checkDeptBdgEntity();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "select isnull(SUM(AL.budget),0) as LandBudget from [assign_budget_land] AL where AL.Land_name=@Land_name and AL.B_year=@Year";
				string query_2 = "select isnull(SUM(AD.budget),0) as SOMME_DEPT from [assign_budget_departement] AD where AD.Land_name=@Land_name and AD.B_year=@Year";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Land_name", Land_name);
				sqlCommand.Parameters.AddWithValue("Year", Year);

				DbExecution.Fill(sqlCommand, dataTable);

				//******
				var sqlCommand_2 = new SqlCommand(query_2, sqlConnection);
				sqlCommand_2.Parameters.AddWithValue("Land_name", Land_name);
				sqlCommand_2.Parameters.AddWithValue("Year", Year);

				new SqlDataAdapter(sqlCommand_2).Fill(dataTable_2);
			}

			if(dataTable.Rows.Count > 0)
			{
				/*var v1 = dataTable.Rows[0]["LandBudget"].ToString();
                var v2 = dataTable.Rows[0]["SOMME_DEPT"].ToString();
                 if (v1 == "" && v2 == "") {
                    response.LandBudget = null;
                    response.SOMME_DEPT = null;
                        }
                else { 
                response.LandBudget = float.Parse(dataTable.Rows[0]["LandBudget"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                response.SOMME_DEPT = float.Parse(dataTable.Rows[0]["SOMME_DEPT"].ToString(), System.Globalization.CultureInfo.InvariantCulture);

                    // return toList2(dataTable);
                }*/
				response.LandBudget = float.Parse(dataTable.Rows[0]["LandBudget"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
				response.SOMME_DEPT = float.Parse(dataTable_2.Rows[0]["SOMME_DEPT"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
			}
			return response;
		}

		public static string CheckDeptExsistance(string Land_name, string Dept_name, int? Year)
		{
			var dataTable = new DataTable();
			string response = string.Empty;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = @"select AD.Departement_name AS DPT from assign_budget_departement AD 
                               where AD.Departement_name=@Dept_name
                               and AD.B_year=@Year
                               and AD.Land_name=@Land_name";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Land_name", Land_name);
				sqlCommand.Parameters.AddWithValue("Year", Year);
				sqlCommand.Parameters.AddWithValue("Dept_name", Dept_name);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				response = dataTable.Rows[0]["DPT"].ToString();
				// return toList2(dataTable);
			}
			return response;
		}

		public static string CheckDeptExsistancebyId(int? Land, int? Dept, int? Year)
		{
			var dataTable = new DataTable();
			string response = string.Empty;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = @"select AD.DepartmentId AS DPT from assign_budget_departement AD 
                               where AD.DepartmentId=@Dept
                               and AD.B_year=@Year
                               and AD.LandId=@Land";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Land", Land);
				sqlCommand.Parameters.AddWithValue("Year", Year);
				sqlCommand.Parameters.AddWithValue("Dept", Dept);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				response = dataTable.Rows[0]["DPT"].ToString();
				// return toList2(dataTable);
			}
			return response;
		}
		public static float GetSumBudgetUsers(string Land_name, string Dept_name, int? Year)
		{
			var dataTable = new DataTable();
			float response = 0;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "select ISNULL(sum(budget_year),0) AS SUM_BUDGET_USERS from [Budget_users] where land=@Land_name and departement_user=@Dept_name and U_year=@Year";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Land_name", Land_name);
				sqlCommand.Parameters.AddWithValue("Year", Year);
				sqlCommand.Parameters.AddWithValue("Dept_name", Dept_name);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				string rps = dataTable.Rows[0]["SUM_BUDGET_USERS"].ToString();
				response = float.Parse(rps, CultureInfo.InvariantCulture.NumberFormat);
				// return toList2(dataTable);
			}
			return response;
		}
		public static int GetUsersCount(string Land_name, string Dept_name, int? Year)
		{
			var dataTable = new DataTable();
			int response = 0;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "select count(username) AS USERS_COUNTS from [Budget_users] where land=@Land_name and departement_user=@Dept_name and U_year=@Year";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Land_name", Land_name);
				sqlCommand.Parameters.AddWithValue("Dept_name", Dept_name);
				sqlCommand.Parameters.AddWithValue("Year", Year);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				response = Convert.ToInt32(dataTable.Rows[0]["USERS_COUNTS"].ToString());
				// return toList2(dataTable);
			}
			return response;
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.AllDataAssign_budget_departementEntity> GetByUserAllDataDept(int user_id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = @"SELECT AD.ID,AD.Land_name,AD.Departement_name,AD.budget,AD.B_year,AD.LandId,AD.DepartmentId,AD.TotalSpent, isnull(SUM(AD.budget), 0) as SommebudgetDept, isnull(SUM(BU.budget_year), 0) as SommebudgetUser, isnull(AD.budget, 0) - isnull(SUM(BU.budget_year), 0) as NotAssignedBudgetUser
                                FROM[Assign_budget_departement] AD
                                Left join[Budget_users] as BU on(BU.[LandId] = AD.[LandId] and BU.[U_year] = AD.[B_year] and BU.[DepartmentId] = AD.[DepartmentId])
                                where AD.[DepartmentId] in (select[Departement_User_Joint].ID_Departement from[Departement_User_Joint] where[Departement_User_Joint].[ID_user] = @user_id)
                                and AD.LandId in (select[Land_User_Joint].ID_Land from[Land_User_Joint] where[Land_User_Joint].[ID_user] = @user_id)
                                Group by AD.ID,AD.Land_name,AD.Departement_name,AD.budget,AD.B_year,AD.LandId,AD.DepartmentId,AD.TotalSpent";
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

		public static Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity GetUniqueByDepartment(string landName, string departmentName, int year)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Assign_budget_departement] WHERE [Land_name]=@landName AND [Departement_name]=@departmentName AND [B_year]=@year";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("landName", landName);
				sqlCommand.Parameters.AddWithValue("departmentName", departmentName);
				sqlCommand.Parameters.AddWithValue("year", year);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity(dataRow)); }
			return list;
		}

		private static List<Infrastructure.Data.Entities.Tables.FNC.checkDeptBdgEntity> toList2(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.checkDeptBdgEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.checkDeptBdgEntity(dataRow)); }
			return list;
		}

		private static List<Infrastructure.Data.Entities.Tables.FNC.AllDataAssign_budget_departementEntity> toListAllData(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.AllDataAssign_budget_departementEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.AllDataAssign_budget_departementEntity(dataRow)); }
			return list;
		}

		#endregion
	}
}
