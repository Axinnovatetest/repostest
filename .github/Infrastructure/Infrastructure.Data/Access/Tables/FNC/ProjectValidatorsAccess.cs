using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.FNC
{
	public class ProjectValidatorsAccess
	{

		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_ProjectValidators] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_ProjectValidators]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [__FNC_ProjectValidators] WHERE [ID] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity item)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__FNC_ProjectValidators] ([email],[Id_Project],[Id_User],[Id_Validator],[Level],[Validation_date])  VALUES (@email,@Id_Project,@Id_User,@Id_Validator,@Level,@Validation_date)";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("email", item.email == null ? (object)DBNull.Value : item.email);
					sqlCommand.Parameters.AddWithValue("Id_Project", item.Id_Project == null ? (object)DBNull.Value : item.Id_Project);
					sqlCommand.Parameters.AddWithValue("Id_User", item.Id_User);
					sqlCommand.Parameters.AddWithValue("Id_Validator", item.Id_Validator);
					sqlCommand.Parameters.AddWithValue("Level", item.Level == null ? (object)DBNull.Value : item.Level);
					sqlCommand.Parameters.AddWithValue("Validation_date", item.Validation_date == null ? (object)DBNull.Value : item.Validation_date);

					DbExecution.ExecuteNonQuery(sqlCommand);
				}

				using(var sqlCommand = new SqlCommand("SELECT [ID] FROM [__FNC_ProjectValidators] WHERE [ID] = @@IDENTITY", sqlConnection, sqlTransaction))
				{
					response = int.Parse(DbExecution.ExecuteScalar(sqlCommand)?.ToString() ?? "-1");
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity> items)
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
						query += " INSERT INTO [__FNC_ProjectValidators] ([email],[Id_Project],[Id_User],[Id_Validator],[Level],[Validation_date]) VALUES ( "

							+ "@email" + i + ","
							+ "@Id_Project" + i + ","
							+ "@Id_User" + i + ","
							+ "@Id_Validator" + i + ","
							+ "@Level" + i + ","
							+ "@Validation_date" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("email" + i, item.email == null ? (object)DBNull.Value : item.email);
						sqlCommand.Parameters.AddWithValue("Id_Project" + i, item.Id_Project == null ? (object)DBNull.Value : item.Id_Project);
						sqlCommand.Parameters.AddWithValue("Id_User" + i, item.Id_User);
						sqlCommand.Parameters.AddWithValue("Id_Validator" + i, item.Id_Validator);
						sqlCommand.Parameters.AddWithValue("Level" + i, item.Level == null ? (object)DBNull.Value : item.Level);
						sqlCommand.Parameters.AddWithValue("Validation_date" + i, item.Validation_date == null ? (object)DBNull.Value : item.Validation_date);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_ProjectValidators] SET [email]=@email, [Id_Project]=@Id_Project, [Id_User]=@Id_User, [Id_Validator]=@Id_Validator, [Level]=@Level, [Validation_date]=@Validation_date WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("email", item.email == null ? (object)DBNull.Value : item.email);
				sqlCommand.Parameters.AddWithValue("Id_Project", item.Id_Project == null ? (object)DBNull.Value : item.Id_Project);
				sqlCommand.Parameters.AddWithValue("Id_User", item.Id_User);
				sqlCommand.Parameters.AddWithValue("Id_Validator", item.Id_Validator);
				sqlCommand.Parameters.AddWithValue("Level", item.Level == null ? (object)DBNull.Value : item.Level);
				sqlCommand.Parameters.AddWithValue("Validation_date", item.Validation_date == null ? (object)DBNull.Value : item.Validation_date);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity> items)
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
						query += " UPDATE [__FNC_ProjectValidators] SET "

							+ "[email]=@email" + i + ","
							+ "[Id_Project]=@Id_Project" + i + ","
							+ "[Id_User]=@Id_User" + i + ","
							+ "[Id_Validator]=@Id_Validator" + i + ","
							+ "[Level]=@Level" + i + ","
							+ "[Validation_date]=@Validation_date" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("email" + i, item.email == null ? (object)DBNull.Value : item.email);
						sqlCommand.Parameters.AddWithValue("Id_Project" + i, item.Id_Project == null ? (object)DBNull.Value : item.Id_Project);
						sqlCommand.Parameters.AddWithValue("Id_User" + i, item.Id_User);
						sqlCommand.Parameters.AddWithValue("Id_Validator" + i, item.Id_Validator);
						sqlCommand.Parameters.AddWithValue("Level" + i, item.Level == null ? (object)DBNull.Value : item.Level);
						sqlCommand.Parameters.AddWithValue("Validation_date" + i, item.Validation_date == null ? (object)DBNull.Value : item.Validation_date);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int ID)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__FNC_ProjectValidators] WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ID", ID);

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

					string query = "DELETE FROM [__FNC_ProjectValidators] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity> GetByProjectId(int id)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();


				string query = "SELECT * FROM [__FNC_ProjectValidators] where Id_Project=@Id_Project ORDER BY [Level] ASC";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id_Project", id);

				DbExecution.Fill(sqlCommand, dataTable);


			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);

			}
			else
			{ return null; }



		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity> GetByProjectIds(List<int> ids)
		{
			if(ids == null || ids.Count <= 0)
			{ return null; }

			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				string query = $"SELECT * FROM [__FNC_ProjectValidators] where Id_Project IN ({string.Join(", ", ids)})";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);


			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);

			}
			else
			{ return null; }



		}
		public static Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity GetNextByProjectId(int projectId, int currentStep)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();


				string query = "SELECT * FROM [__FNC_ProjectValidators] where Id_Project=@Id_Project AND Level=@currentStep";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id_Project", projectId);
				sqlCommand.Parameters.AddWithValue("currentStep", currentStep); // next validator

				DbExecution.Fill(sqlCommand, dataTable);


			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity(dataTable.Rows[0]);

			}
			else
			{ return null; }
		}
		public static Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity GetLastByProjectId(int projectId)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();


				string query = "SELECT * FROM [__FNC_ProjectValidators] WHERE Id_Project=@projectId AND Level=( SELECT Max(Level) as Level FROM [__FNC_ProjectValidators] WHERE Id_Project=@projectId)";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("projectId", projectId);

				DbExecution.Fill(sqlCommand, dataTable);


			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity(dataTable.Rows[0]);

			}
			else
			{ return null; }
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity> GetByValidatorId(int validatorId)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();


				string query = "SELECT * FROM [__FNC_ProjectValidators]  where [Id_Validator] = @validatorId";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("validatorId", validatorId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);

			}
			else
			{ return null; }
		}
		public static int GetValidationLevel(int projectId, int validatorId)
		{

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();


				string query = "SELECT [level]  FROM [__FNC_ProjectValidators]  where Id_Project =@projectId AND Id_Validator =@validatorId";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("projectId", projectId);
				sqlCommand.Parameters.AddWithValue("validatorId", validatorId);

				return int.Parse(DbExecution.ExecuteScalar(sqlCommand)?.ToString() ?? "-1");
			}



		}
		public static int DeletebyProject(int Id_Project)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__FNC_ProjectValidators] WHERE [Id_Project]=@Id_Project";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id_Project", Id_Project);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity> GetUserProjects(int userId)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();


				string query = "SELECT * FROM [__FNC_ProjectValidators] where Id_Validator=@userId ORDER BY [Level] ASC";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("userId", userId);

				DbExecution.Fill(sqlCommand, dataTable);


			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);

			}
			else
			{ return null; }



		}
		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
