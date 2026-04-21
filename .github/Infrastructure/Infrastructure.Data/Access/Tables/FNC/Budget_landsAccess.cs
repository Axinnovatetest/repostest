using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class Budget_landsAccessXXXX
	{

		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Budget_lands] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Budget_lands]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Budget_lands] WHERE [ID] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Budget_lands] ([Land_name],[PurchaseEmail],[PurchaseGroupName],[PurchaseId],[PurchaseName],[SiteDirectorEmail],[SiteDirectorId],[SiteDirectorName])  VALUES (@Land_name,@PurchaseEmail,@PurchaseGroupName,@PurchaseId,@PurchaseName,@SiteDirectorEmail,@SiteDirectorId,@SiteDirectorName); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Land_name", item.Land_name == null ? (object)DBNull.Value : item.Land_name);
					sqlCommand.Parameters.AddWithValue("PurchaseEmail", item.PurchaseEmail == null ? (object)DBNull.Value : item.PurchaseEmail);
					sqlCommand.Parameters.AddWithValue("PurchaseGroupName", item.PurchaseGroupName == null ? (object)DBNull.Value : item.PurchaseGroupName);
					sqlCommand.Parameters.AddWithValue("PurchaseId", item.PurchaseId == null ? (object)DBNull.Value : item.PurchaseId);
					sqlCommand.Parameters.AddWithValue("PurchaseName", item.PurchaseName == null ? (object)DBNull.Value : item.PurchaseName);
					sqlCommand.Parameters.AddWithValue("SiteDirectorEmail", item.SiteDirectorEmail == null ? (object)DBNull.Value : item.SiteDirectorEmail);
					sqlCommand.Parameters.AddWithValue("SiteDirectorId", item.SiteDirectorId == null ? (object)DBNull.Value : item.SiteDirectorId);
					sqlCommand.Parameters.AddWithValue("SiteDirectorName", item.SiteDirectorName == null ? (object)DBNull.Value : item.SiteDirectorName);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity> items)
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
						query += " INSERT INTO [Budget_lands] ([Land_name],[PurchaseEmail],[PurchaseGroupName],[PurchaseId],[PurchaseName],[SiteDirectorEmail],[SiteDirectorId],[SiteDirectorName]) VALUES ( "

							+ "@Land_name" + i + ","
							+ "@PurchaseEmail" + i + ","
							+ "@PurchaseGroupName" + i + ","
							+ "@PurchaseId" + i + ","
							+ "@PurchaseName" + i + ","
							+ "@SiteDirectorEmail" + i + ","
							+ "@SiteDirectorId" + i + ","
							+ "@SiteDirectorName" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Land_name" + i, item.Land_name == null ? (object)DBNull.Value : item.Land_name);
						sqlCommand.Parameters.AddWithValue("PurchaseEmail" + i, item.PurchaseEmail == null ? (object)DBNull.Value : item.PurchaseEmail);
						sqlCommand.Parameters.AddWithValue("PurchaseGroupName" + i, item.PurchaseGroupName == null ? (object)DBNull.Value : item.PurchaseGroupName);
						sqlCommand.Parameters.AddWithValue("PurchaseId" + i, item.PurchaseId == null ? (object)DBNull.Value : item.PurchaseId);
						sqlCommand.Parameters.AddWithValue("PurchaseName" + i, item.PurchaseName == null ? (object)DBNull.Value : item.PurchaseName);
						sqlCommand.Parameters.AddWithValue("SiteDirectorEmail" + i, item.SiteDirectorEmail == null ? (object)DBNull.Value : item.SiteDirectorEmail);
						sqlCommand.Parameters.AddWithValue("SiteDirectorId" + i, item.SiteDirectorId == null ? (object)DBNull.Value : item.SiteDirectorId);
						sqlCommand.Parameters.AddWithValue("SiteDirectorName" + i, item.SiteDirectorName == null ? (object)DBNull.Value : item.SiteDirectorName);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [Budget_lands] SET [Land_name]=@Land_name, [PurchaseEmail]=@PurchaseEmail, [PurchaseGroupName]=@PurchaseGroupName, [PurchaseId]=@PurchaseId, [PurchaseName]=@PurchaseName, [SiteDirectorEmail]=@SiteDirectorEmail, [SiteDirectorId]=@SiteDirectorId, [SiteDirectorName]=@SiteDirectorName WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("Land_name", item.Land_name == null ? (object)DBNull.Value : item.Land_name);
				sqlCommand.Parameters.AddWithValue("PurchaseEmail", item.PurchaseEmail == null ? (object)DBNull.Value : item.PurchaseEmail);
				sqlCommand.Parameters.AddWithValue("PurchaseGroupName", item.PurchaseGroupName == null ? (object)DBNull.Value : item.PurchaseGroupName);
				sqlCommand.Parameters.AddWithValue("PurchaseId", item.PurchaseId == null ? (object)DBNull.Value : item.PurchaseId);
				sqlCommand.Parameters.AddWithValue("PurchaseName", item.PurchaseName == null ? (object)DBNull.Value : item.PurchaseName);
				sqlCommand.Parameters.AddWithValue("SiteDirectorEmail", item.SiteDirectorEmail == null ? (object)DBNull.Value : item.SiteDirectorEmail);
				sqlCommand.Parameters.AddWithValue("SiteDirectorId", item.SiteDirectorId == null ? (object)DBNull.Value : item.SiteDirectorId);
				sqlCommand.Parameters.AddWithValue("SiteDirectorName", item.SiteDirectorName == null ? (object)DBNull.Value : item.SiteDirectorName);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity> items)
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
						query += " UPDATE [Budget_lands] SET "

							+ "[Land_name]=@Land_name" + i + ","
							+ "[PurchaseEmail]=@PurchaseEmail" + i + ","
							+ "[PurchaseGroupName]=@PurchaseGroupName" + i + ","
							+ "[PurchaseId]=@PurchaseId" + i + ","
							+ "[PurchaseName]=@PurchaseName" + i + ","
							+ "[SiteDirectorEmail]=@SiteDirectorEmail" + i + ","
							+ "[SiteDirectorId]=@SiteDirectorId" + i + ","
							+ "[SiteDirectorName]=@SiteDirectorName" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("Land_name" + i, item.Land_name == null ? (object)DBNull.Value : item.Land_name);
						sqlCommand.Parameters.AddWithValue("PurchaseEmail" + i, item.PurchaseEmail == null ? (object)DBNull.Value : item.PurchaseEmail);
						sqlCommand.Parameters.AddWithValue("PurchaseGroupName" + i, item.PurchaseGroupName == null ? (object)DBNull.Value : item.PurchaseGroupName);
						sqlCommand.Parameters.AddWithValue("PurchaseId" + i, item.PurchaseId == null ? (object)DBNull.Value : item.PurchaseId);
						sqlCommand.Parameters.AddWithValue("PurchaseName" + i, item.PurchaseName == null ? (object)DBNull.Value : item.PurchaseName);
						sqlCommand.Parameters.AddWithValue("SiteDirectorEmail" + i, item.SiteDirectorEmail == null ? (object)DBNull.Value : item.SiteDirectorEmail);
						sqlCommand.Parameters.AddWithValue("SiteDirectorId" + i, item.SiteDirectorId == null ? (object)DBNull.Value : item.SiteDirectorId);
						sqlCommand.Parameters.AddWithValue("SiteDirectorName" + i, item.SiteDirectorName == null ? (object)DBNull.Value : item.SiteDirectorName);
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
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Budget_lands] WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ID", id);

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

					string query = "DELETE FROM [Budget_lands] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion


		#region Custom Methods

		public static Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity GetByName(string landName)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Budget_lands] WHERE [Land_name]=@landName";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("landName", landName.Trim());

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity> GetAllowedLandsbyDept(int id_user)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "select * from [Budget_lands] where [Budget_lands].[ID] in" +
					" (select [ID_Land] from [Land_Department_Joint] where [Land_Department_Joint].[ID] in " +
					"(select [ID_Departement] from [Departement_User_Joint] where [ID_user]=@Id))";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id_user);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);

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
		public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity> GetByIdDirector(int directorId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Budget_lands] WHERE [SiteDirectorId]=@directorId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("directorId", directorId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

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
		public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity> GetByLandAndPurchase(List<int> landIds, int purchaseProfileId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Budget_lands] WHERE [ID] IN ({string.Join(", ", landIds)}) AND [PurchaseId]=@purchaseProfileId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("purchaseProfileId", purchaseProfileId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

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
		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.Budget_landsEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
