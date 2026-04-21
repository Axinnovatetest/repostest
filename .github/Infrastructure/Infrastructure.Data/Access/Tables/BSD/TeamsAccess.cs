using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class TeamsAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.TeamsEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [BSD_Teams] WHERE [Id]=@Id", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.TeamsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.TeamsEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [BSD_Teams]", sqlConnection))
			{
				sqlConnection.Open();
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.TeamsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.TeamsEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.TeamsEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.TeamsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.TeamsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.TeamsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.TeamsEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				using(var sqlCommand = new SqlCommand("", sqlConnection))
				{
					sqlConnection.Open();

					var queryIds = new List<string>();
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds.Add($"@Id{i}");
						sqlCommand.Parameters.AddWithValue($"Id{i}", ids[i]);
					}

					sqlCommand.CommandText = $"SELECT * FROM [BSD_Teams] WHERE [Id] IN ({string.Join(",", queryIds)})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.TeamsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.TeamsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.TeamsEntity>();

		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.TeamsEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [BSD_Teams] ([Description],[Name],[SiteId],[SitePrefix],[TeamCategory],[TeamIndex]) OUTPUT INSERTED.[Id] VALUES (@Description,@Name,@SiteId,@SitePrefix,@TeamCategory,@TeamIndex); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
					sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? (object)DBNull.Value : item.Name);
					sqlCommand.Parameters.AddWithValue("SiteId", item.SiteId == null ? (object)DBNull.Value : item.SiteId);
					sqlCommand.Parameters.AddWithValue("SitePrefix", item.SitePrefix == null ? (object)DBNull.Value : item.SitePrefix);
					sqlCommand.Parameters.AddWithValue("TeamCategory", item.TeamCategory == null ? (object)DBNull.Value : item.TeamCategory);
					sqlCommand.Parameters.AddWithValue("TeamIndex", item.TeamIndex == null ? (object)DBNull.Value : item.TeamIndex);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.TeamsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; /* Nb params per query */
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.TeamsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				using(var sqlCommand = new SqlCommand("", sqlConnection))
				{
					sqlConnection.Open();
					string query = "";
					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [BSD_Teams] ([Description],[Name],[SiteId],[SitePrefix],[TeamCategory],[TeamIndex]) VALUES ("

							+ "@Description" + i + ","
							+ "@Name" + i + ","
							+ "@SiteId" + i + ","
							+ "@SitePrefix" + i + ","
							+ "@TeamCategory" + i + ","
							+ "@TeamIndex" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name == null ? (object)DBNull.Value : item.Name);
						sqlCommand.Parameters.AddWithValue("SiteId" + i, item.SiteId == null ? (object)DBNull.Value : item.SiteId);
						sqlCommand.Parameters.AddWithValue("SitePrefix" + i, item.SitePrefix == null ? (object)DBNull.Value : item.SitePrefix);
						sqlCommand.Parameters.AddWithValue("TeamCategory" + i, item.TeamCategory == null ? (object)DBNull.Value : item.TeamCategory);
						sqlCommand.Parameters.AddWithValue("TeamIndex" + i, item.TeamIndex == null ? (object)DBNull.Value : item.TeamIndex);
					}

					sqlCommand.CommandText = query;
					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.TeamsEntity item)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("", sqlConnection))
			{
				sqlConnection.Open();
				string query = "UPDATE [BSD_Teams] SET [Description]=@Description, [Name]=@Name, [SiteId]=@SiteId, [SitePrefix]=@SitePrefix, [TeamCategory]=@TeamCategory, [TeamIndex]=@TeamIndex WHERE [Id]=@Id";
				sqlCommand.CommandText = query;
				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
				sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? (object)DBNull.Value : item.Name);
				sqlCommand.Parameters.AddWithValue("SiteId", item.SiteId == null ? (object)DBNull.Value : item.SiteId);
				sqlCommand.Parameters.AddWithValue("SitePrefix", item.SitePrefix == null ? (object)DBNull.Value : item.SitePrefix);
				sqlCommand.Parameters.AddWithValue("TeamCategory", item.TeamCategory == null ? (object)DBNull.Value : item.TeamCategory);
				sqlCommand.Parameters.AddWithValue("TeamIndex", item.TeamIndex == null ? (object)DBNull.Value : item.TeamIndex);

				return sqlCommand.ExecuteNonQuery();
			}
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.TeamsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; /* Nb params per query */
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.TeamsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				using(var sqlCommand = new SqlCommand("", sqlConnection))
				{
					sqlConnection.Open();
					string query = "";

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += "UPDATE [BSD_Teams] SET "

							+ "[Description]=@Description" + i + ","
							+ "[Name]=@Name" + i + ","
							+ "[SiteId]=@SiteId" + i + ","
							+ "[SitePrefix]=@SitePrefix" + i + ","
							+ "[TeamCategory]=@TeamCategory" + i + ","
							+ "[TeamIndex]=@TeamIndex" + i + $" WHERE [Id]=@Id{i}"
							+ "; ";

						sqlCommand.Parameters.AddWithValue($"Id{i}", item.Id);

						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name == null ? (object)DBNull.Value : item.Name);
						sqlCommand.Parameters.AddWithValue("SiteId" + i, item.SiteId == null ? (object)DBNull.Value : item.SiteId);
						sqlCommand.Parameters.AddWithValue("SitePrefix" + i, item.SitePrefix == null ? (object)DBNull.Value : item.SitePrefix);
						sqlCommand.Parameters.AddWithValue("TeamCategory" + i, item.TeamCategory == null ? (object)DBNull.Value : item.TeamCategory);
						sqlCommand.Parameters.AddWithValue("TeamIndex" + i, item.TeamIndex == null ? (object)DBNull.Value : item.TeamIndex);
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
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("DELETE FROM [BSD_Teams] WHERE [Id]=@Id", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("Id", id);

				return sqlCommand.ExecuteNonQuery();
			}
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
				using(var sqlCommand = new SqlCommand("", sqlConnection))
				{
					sqlConnection.Open();
					var queryIds = new List<string>();
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds.Add($"@Id{i}");
						sqlCommand.Parameters.AddWithValue($"Id{i}", ids[i]);
					}

					string query = $"DELETE FROM [BSD_Teams] WHERE [Id] IN ({string.Join(",", queryIds)})";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.BSD.TeamsEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			using(var sqlCommand = new SqlCommand("SELECT * FROM [BSD_Teams] WHERE [Id] = @Id", connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", id);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.TeamsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.TeamsEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			using(var sqlCommand = new SqlCommand("SELECT * FROM [BSD_Teams]", connection, transaction))
			{
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.TeamsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.TeamsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.TeamsEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.TeamsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.TeamsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.TeamsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.TeamsEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlCommand = new SqlCommand("", connection, transaction))
				{
					var queryIds = new List<string>();
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds.Add($"@Id{i}");
						sqlCommand.Parameters.AddWithValue($"Id{i}", ids[i]);
					}

					sqlCommand.CommandText = $"SELECT * FROM [BSD_Teams] WHERE [Id] IN ({string.Join(",", queryIds)})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.TeamsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.TeamsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.TeamsEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.BSD.TeamsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "INSERT INTO BSD_Teams ([Description],[Name],[SiteId],[SitePrefix],[TeamCategory],[TeamIndex]) OUTPUT INSERTED.[Id] VALUES (@Description,@Name,@SiteId,@SitePrefix,@TeamCategory,@TeamIndex); ";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
				sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? (object)DBNull.Value : item.Name);
				sqlCommand.Parameters.AddWithValue("SiteId", item.SiteId == null ? (object)DBNull.Value : item.SiteId);
				sqlCommand.Parameters.AddWithValue("SitePrefix", item.SitePrefix == null ? (object)DBNull.Value : item.SitePrefix);
				sqlCommand.Parameters.AddWithValue("TeamCategory", item.TeamCategory == null ? (object)DBNull.Value : item.TeamCategory);
				sqlCommand.Parameters.AddWithValue("TeamIndex", item.TeamIndex == null ? (object)DBNull.Value : item.TeamIndex);
				var result = sqlCommand.ExecuteScalar();
				return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.TeamsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; /* Nb params per query */
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.TeamsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				using(var sqlCommand = new SqlCommand(query, connection, transaction))
				{
					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += "INSERT INTO [BSD_Teams] ([Description],[Name],[SiteId],[SitePrefix],[TeamCategory],[TeamIndex]) VALUES ( "

						+ "@Description" + i + ","
						+ "@Name" + i + ","
						+ "@SiteId" + i + ","
						+ "@SitePrefix" + i + ","
						+ "@TeamCategory" + i + ","
						+ "@TeamIndex" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name == null ? (object)DBNull.Value : item.Name);
						sqlCommand.Parameters.AddWithValue("SiteId" + i, item.SiteId == null ? (object)DBNull.Value : item.SiteId);
						sqlCommand.Parameters.AddWithValue("SitePrefix" + i, item.SitePrefix == null ? (object)DBNull.Value : item.SitePrefix);
						sqlCommand.Parameters.AddWithValue("TeamCategory" + i, item.TeamCategory == null ? (object)DBNull.Value : item.TeamCategory);
						sqlCommand.Parameters.AddWithValue("TeamIndex" + i, item.TeamIndex == null ? (object)DBNull.Value : item.TeamIndex);
					}

					sqlCommand.CommandText = query;

					return sqlCommand.ExecuteNonQuery();
				}
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.BSD.TeamsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [BSD_Teams] SET [Description]=@Description, [Name]=@Name, [SiteId]=@SiteId, [SitePrefix]=@SitePrefix, [TeamCategory]=@TeamCategory, [TeamIndex]=@TeamIndex WHERE [Id]=@Id";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
				sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? (object)DBNull.Value : item.Name);
				sqlCommand.Parameters.AddWithValue("SiteId", item.SiteId == null ? (object)DBNull.Value : item.SiteId);
				sqlCommand.Parameters.AddWithValue("SitePrefix", item.SitePrefix == null ? (object)DBNull.Value : item.SitePrefix);
				sqlCommand.Parameters.AddWithValue("TeamCategory", item.TeamCategory == null ? (object)DBNull.Value : item.TeamCategory);
				sqlCommand.Parameters.AddWithValue("TeamIndex", item.TeamIndex == null ? (object)DBNull.Value : item.TeamIndex);

				return sqlCommand.ExecuteNonQuery();
			}
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.TeamsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; /* Nb params per query */
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.TeamsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				using(var sqlCommand = new SqlCommand(query, connection, transaction))
				{
					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += "UPDATE [BSD_Teams] SET "

						+ "[Description]=@Description" + i + ","
						+ "[Name]=@Name" + i + ","
						+ "[SiteId]=@SiteId" + i + ","
						+ "[SitePrefix]=@SitePrefix" + i + ","
						+ "[TeamCategory]=@TeamCategory" + i + ","
						+ "[TeamIndex]=@TeamIndex" + i + " WHERE [Id]=@Id" + i
							+ ";";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);

						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name == null ? (object)DBNull.Value : item.Name);
						sqlCommand.Parameters.AddWithValue("SiteId" + i, item.SiteId == null ? (object)DBNull.Value : item.SiteId);
						sqlCommand.Parameters.AddWithValue("SitePrefix" + i, item.SitePrefix == null ? (object)DBNull.Value : item.SitePrefix);
						sqlCommand.Parameters.AddWithValue("TeamCategory" + i, item.TeamCategory == null ? (object)DBNull.Value : item.TeamCategory);
						sqlCommand.Parameters.AddWithValue("TeamIndex" + i, item.TeamIndex == null ? (object)DBNull.Value : item.TeamIndex);
					}

					sqlCommand.CommandText = query;
					return sqlCommand.ExecuteNonQuery();
				}
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "DELETE FROM [BSD_Teams] WHERE [Id]=@Id";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", id);
				return sqlCommand.ExecuteNonQuery();
			}
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
				using(var sqlCommand = new SqlCommand("", connection, transaction))
				{
					var queryIds = new List<string>();
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds.Add($"@Id{i}");
						sqlCommand.Parameters.AddWithValue($"Id{i}", ids[i]);
					}

					string query = $"DELETE FROM BSD_Teams] WHERE [Id] IN ({string.Join(",", queryIds)})";
					sqlCommand.CommandText = query;

					return sqlCommand.ExecuteNonQuery();
				}
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static Infrastructure.Data.Entities.Tables.BSD.TeamsEntity GetMaxBySite(int siteId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand($@"SELECT TOP 1 * FROM [BSD_Teams] 
																		WHERE [SiteId]=@Id 
																			AND [TeamIndex]=(SELECT MAX([TeamIndex]) FROM [BSD_Teams] WHERE [SiteId]=@Id)
																			AND [TeamCategory]=(SELECT MAX([TeamCategory]) FROM [BSD_Teams] WHERE [SiteId]=@Id AND [TeamIndex]=(SELECT MAX([TeamIndex]) FROM [BSD_Teams] WHERE [SiteId]=@Id))
															", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("Id", siteId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.TeamsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.TeamsEntity> GetNextTeams(int id, int index, char category)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand($@"SELECT * from BSD_Teams Where [SiteId]=@id AND ([TeamIndex]>@index or ([TeamIndex]=@index AND [TeamCategory]>@category))
															", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("id", id);
				sqlCommand.Parameters.AddWithValue("index", index);
				sqlCommand.Parameters.AddWithValue("category", category);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.TeamsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.TeamsEntity>();
			}
		}
		public static Infrastructure.Data.Entities.Tables.BSD.TeamsEntity GetMaxBySite(int siteId, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			using(var sqlCommand = new SqlCommand($@"SELECT TOP 1 * FROM [BSD_Teams] 
																		WHERE [SiteId]=@Id 
																			AND [TeamIndex]=(SELECT MAX([TeamIndex]) FROM [BSD_Teams] WHERE [SiteId]=@Id)
																			AND [TeamCategory]=(SELECT MAX([TeamCategory]) FROM [BSD_Teams] WHERE [SiteId]=@Id AND [TeamIndex]=(SELECT MAX([TeamIndex]) FROM [BSD_Teams] WHERE [SiteId]=@Id))
															", connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", siteId);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.TeamsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.BSD.TeamsEntity GetByName(string name)
		{
			name = name ?? "";
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [BSD_Teams] WHERE TRIM([Name])=TRIM(@name)", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("name", name);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.TeamsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		#endregion Custom Methods

	}
}
