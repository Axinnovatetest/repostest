using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class Land_Department_JointAccessXXX
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Land_Department_Joint] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Land_Department_Joint]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Land_Department_Joint] WHERE [ID] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Land_Department_Joint] ([EmailUser],[ID_Department],[ID_Land],[ID_user])  VALUES (@EmailUser,@ID_Department,@ID_Land,@ID_user); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("EmailUser", item.EmailUser == null ? (object)DBNull.Value : item.EmailUser);
					sqlCommand.Parameters.AddWithValue("ID_Department", item.ID_Department == null ? (object)DBNull.Value : item.ID_Department);
					sqlCommand.Parameters.AddWithValue("ID_Land", item.ID_Land == null ? (object)DBNull.Value : item.ID_Land);
					sqlCommand.Parameters.AddWithValue("ID_user", item.ID_user == null ? (object)DBNull.Value : item.ID_user);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity> items)
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
						query += " INSERT INTO [Land_Department_Joint] ([EmailUser],[ID_Department],[ID_Land],[ID_user]) VALUES ( "

							+ "@EmailUser" + i + ","
							+ "@ID_Department" + i + ","
							+ "@ID_Land" + i + ","
							+ "@ID_user" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("EmailUser" + i, item.EmailUser == null ? (object)DBNull.Value : item.EmailUser);
						sqlCommand.Parameters.AddWithValue("ID_Department" + i, item.ID_Department == null ? (object)DBNull.Value : item.ID_Department);
						sqlCommand.Parameters.AddWithValue("ID_Land" + i, item.ID_Land == null ? (object)DBNull.Value : item.ID_Land);
						sqlCommand.Parameters.AddWithValue("ID_user" + i, item.ID_user == null ? (object)DBNull.Value : item.ID_user);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [Land_Department_Joint] SET [EmailUser]=@EmailUser, [ID_Department]=@ID_Department, [ID_Land]=@ID_Land, [ID_user]=@ID_user WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("EmailUser", item.EmailUser == null ? (object)DBNull.Value : item.EmailUser);
				sqlCommand.Parameters.AddWithValue("ID_Department", item.ID_Department == null ? (object)DBNull.Value : item.ID_Department);
				sqlCommand.Parameters.AddWithValue("ID_Land", item.ID_Land == null ? (object)DBNull.Value : item.ID_Land);
				sqlCommand.Parameters.AddWithValue("ID_user", item.ID_user == null ? (object)DBNull.Value : item.ID_user);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity> items)
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
						query += " UPDATE [Land_Department_Joint] SET "

							+ "[EmailUser]=@EmailUser" + i + ","
							+ "[ID_Department]=@ID_Department" + i + ","
							+ "[ID_Land]=@ID_Land" + i + ","
							+ "[ID_user]=@ID_user" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("EmailUser" + i, item.EmailUser == null ? (object)DBNull.Value : item.EmailUser);
						sqlCommand.Parameters.AddWithValue("ID_Department" + i, item.ID_Department == null ? (object)DBNull.Value : item.ID_Department);
						sqlCommand.Parameters.AddWithValue("ID_Land" + i, item.ID_Land == null ? (object)DBNull.Value : item.ID_Land);
						sqlCommand.Parameters.AddWithValue("ID_user" + i, item.ID_user == null ? (object)DBNull.Value : item.ID_user);
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
				string query = "DELETE FROM [Land_Department_Joint] WHERE [ID]=@ID";
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

					string query = "DELETE FROM [Land_Department_Joint] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity GetHeadOfDepartment(int idDept, int idLand)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Land_Department_Joint] WHERE [ID_Department]=@idDept AND [ID_Land]=@idLand";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("idDept", idDept);
				sqlCommand.Parameters.AddWithValue("idLand", idLand);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity> GetbyLandId(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Land_Department_Joint] WHERE [ID_Land]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity>();
			}
		}


		private static List<Infrastructure.Data.Entities.Tables.FNC.AllDataLand_Department_JointEntity> getbyId(List<int> ids)
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

					//sqlCommand.CommandText = "SELECT * FROM [Land_Department_Joint] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = "select deptjointland.[ID], deptjointland.[ID_Department], BDGdept.[Departement_name], deptjointland.[ID_Land], BDGland.[Land_name], deptjointland.[ID_user], usr.[Name] as Name, deptjointland.[EmailUser] from[Budget].[dbo].[Land_Department_Joint] as deptjointland " +
					"Left join [dbo].[User] as usr on usr.[Id] = deptjointland.[ID_user] " +
					"inner join [dbo].[Budget_departement] as BDGdept  on BDGdept.[ID] = deptjointland.[ID_Department] " +
					"inner join [dbo].[Budget_lands] as BDGland  on BDGland.[ID] = deptjointland.[ID_Land] WHERE deptjointland.[ID] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toListAllData(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.AllDataLand_Department_JointEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.AllDataLand_Department_JointEntity>();
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.AllDataLand_Department_JointEntity> GetJointedbyId(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.AllDataLand_Department_JointEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getbyId(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.AllDataLand_Department_JointEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getbyId(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(getbyId(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.AllDataLand_Department_JointEntity>();
		}
		public static Infrastructure.Data.Entities.Tables.FNC.AllDataLand_Department_JointEntity GetbyDept(int id)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "select deptjointland.[ID], deptjointland.[ID_Department], BDGdept.[Departement_name], deptjointland.[ID_Land], BDGland.[Land_name], deptjointland.[ID_user], usr.[Name] as Name, deptjointland.[EmailUser] from[Budget].[dbo].[Land_Department_Joint] as deptjointland " +
							 "Left join [dbo].[User] as usr on usr.[Id] = deptjointland.[ID_user] " +
							 "inner join[Budget].[dbo].[Budget_departement] as BDGdept  on BDGdept.[ID] = deptjointland.[ID_Department] " +
							 "inner join[Budget].[dbo].[Budget_lands] as BDGland  on BDGland.[ID] = deptjointland.[ID_Land] " +
							 "WHERE[ID_Department] =@Id";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Id", id);


			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.AllDataLand_Department_JointEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.AllDataLand_Department_JointEntity> getjointdeptland(List<int> ids, int idLand)
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

					//sqlCommand.CommandText = "SELECT * FROM [Land_Department_Joint] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = "select deptjointland.[ID], deptjointland.[ID_Department], BDGdept.[Departement_name], deptjointland.[ID_Land], BDGland.[Land_name], deptjointland.[ID_user], usr.[Name] as Name, deptjointland.[EmailUser] from[Budget].[dbo].[Land_Department_Joint] as deptjointland " +
					"Left join [dbo].[User] as usr on usr.[Id] = deptjointland.[ID_user] " +
					"inner join[Budget].[dbo].[Budget_departement] as BDGdept  on BDGdept.[ID] = deptjointland.[ID_Department] " +
					"inner join[Budget].[dbo].[Budget_lands] as BDGland  on BDGland.[ID] = deptjointland.[ID_Land] WHERE deptjointland.[ID] IN (" + queryIds + ") and [ID_Land] =@IdLand";
					sqlCommand.Parameters.AddWithValue("IdLand", idLand);
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toListAllData(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.AllDataLand_Department_JointEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.AllDataLand_Department_JointEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.AllDataLand_Department_JointEntity> getjointdeptlandID(List<int> ids, int idLand)
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

					//sqlCommand.CommandText = "SELECT * FROM [Land_Department_Joint] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = "select deptjointland.[ID], deptjointland.[ID_Department], BDGdept.[Departement_name], deptjointland.[ID_Land], BDGland.[Land_name], deptjointland.[ID_user], usr.[Name] as Name, deptjointland.[EmailUser] from[Budget].[dbo].[Land_Department_Joint] as deptjointland " +
					"Left join [dbo].[User] as usr on usr.[Id] = deptjointland.[ID_user] " +
					"inner join[Budget].[dbo].[Budget_departement] as BDGdept  on BDGdept.[ID] = deptjointland.[ID_Department] " +
					"inner join[Budget].[dbo].[Budget_lands] as BDGland  on BDGland.[ID] = deptjointland.[ID_Land] WHERE deptjointland.[ID] IN (" + queryIds + ") and [ID_Land] =@IdLand";
					sqlCommand.Parameters.AddWithValue("IdLand", idLand);
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toListAllData(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.AllDataLand_Department_JointEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.AllDataLand_Department_JointEntity>();
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.AllDataLand_Department_JointEntity> GetDeptJoint(List<int> ids, int idLand)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.AllDataLand_Department_JointEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getjointdeptland(ids, idLand);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.AllDataLand_Department_JointEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getjointdeptland(ids.GetRange(i * maxQueryNumber, maxQueryNumber), idLand));
					}
					results.AddRange(getjointdeptland(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), idLand));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.AllDataLand_Department_JointEntity>();
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.AllDataLand_Department_JointEntity> GetDeptJointID(List<int> ids, int idLand)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.AllDataLand_Department_JointEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getjointdeptlandID(ids, idLand);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.AllDataLand_Department_JointEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getjointdeptlandID(ids.GetRange(i * maxQueryNumber, maxQueryNumber), idLand));
					}
					results.AddRange(getjointdeptlandID(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), idLand));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.AllDataLand_Department_JointEntity>();
		}
		public static int InsertDeptJointLand(Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity item)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "IF EXISTS(select* from [Budget].[dbo].[Land_Department_Joint] where[Budget].[dbo].[Land_Department_Joint].ID_Department=@ID_Department and[Budget].[dbo].[Land_Department_Joint].ID_Land=@ID_Land) select 0 " +
				"ELSE " +
				"INSERT INTO[Land_Department_Joint] ([ID_Department],[ID_Land],[ID_user],[EmailUser]) VALUES(@ID_Department, @ID_Land, @ID_user,@EmailUser)";
				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ID_Department", item.ID_Department == null ? (object)DBNull.Value : item.ID_Department);
					sqlCommand.Parameters.AddWithValue("ID_Land", item.ID_Land == null ? (object)DBNull.Value : item.ID_Land);
					sqlCommand.Parameters.AddWithValue("ID_user", item.ID_user == null ? (object)DBNull.Value : item.ID_user);
					sqlCommand.Parameters.AddWithValue("EmailUser", item.EmailUser == null ? (object)DBNull.Value : item.EmailUser);

					DbExecution.ExecuteNonQuery(sqlCommand);
				}

				using(var sqlCommand = new SqlCommand("SELECT [ID] FROM [Land_Department_Joint] WHERE [ID] = @@IDENTITY", sqlConnection, sqlTransaction))
				{
					response = int.Parse(DbExecution.ExecuteScalar(sqlCommand)?.ToString() ?? "-1");
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int UpdateDeptJointLand(Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "IF EXISTS(select* from [Budget].[dbo].[Land_Department_Joint] where[Budget].[dbo].[Land_Department_Joint].ID_Department=@ID_Department and[Budget].[dbo].[Land_Department_Joint].ID_Land=@ID_Land) select 0 " +
				"ELSE " +
				"UPDATE [Land_Department_Joint] SET [ID_Department]=@ID_Department, [ID_Land]=@ID_Land, [ID_user]=@ID_user, [EmailUser]=@EmailUser WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("ID_Department", item.ID_Department == null ? (object)DBNull.Value : item.ID_Department);
				sqlCommand.Parameters.AddWithValue("ID_Land", item.ID_Land == null ? (object)DBNull.Value : item.ID_Land);
				sqlCommand.Parameters.AddWithValue("ID_user", item.ID_user == null ? (object)DBNull.Value : item.ID_user);
				sqlCommand.Parameters.AddWithValue("EmailUser", item.EmailUser == null ? (object)DBNull.Value : item.EmailUser);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}

		public static Infrastructure.Data.Entities.Tables.FNC.Department_Responsable_JointEntity GetDeptResponsable(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				string query = "select resp.ID,resp.ID_Land,resp.ID_Department,resp.ID_user,user0.Name,user0.Username from Land_Department_Joint as resp Left join [dbo].[User] as user0 on user0.[Id]= resp.[ID_user] WHERE [ID_Department]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Department_Responsable_JointEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.FNC.Department_Responsable_JointEntity GetDeptResponsablebyLand(int id, int LandId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				string query = "select resp.ID,resp.ID_Land,resp.ID_Department,resp.ID_user,user0.Name,user0.Username from Land_Department_Joint as resp Left join [dbo].[User] as user0 on user0.[Id]= resp.[ID_user] WHERE [ID_Department]=@Id AND [ID_Land]=@LandId ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);
				sqlCommand.Parameters.AddWithValue("LandId", LandId);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Department_Responsable_JointEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity> GetByIdDirector(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Land_Department_Joint] WHERE [ID_user]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

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

		public static Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity GetByIdDirectorDept(int id, int dept)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Land_Department_Joint] WHERE [ID_user]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);
				sqlCommand.Parameters.AddWithValue("dept", dept);
				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity(dataRow)); }
			return list;
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.AllDataLand_Department_JointEntity> toListAllData(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.AllDataLand_Department_JointEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.AllDataLand_Department_JointEntity(dataRow)); }
			return list;
		}
		#endregion

	}
}
