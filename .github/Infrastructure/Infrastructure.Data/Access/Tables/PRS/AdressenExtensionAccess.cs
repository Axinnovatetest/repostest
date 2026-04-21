using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.PRS
{

	public class AdressenExtensionAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__PRS_AdressenExtension] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__PRS_AdressenExtension]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [__PRS_AdressenExtension] WHERE [Id] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity>();
		}
		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__PRS_AdressenExtension] ([AdressenNr],[Duns],[LastUpdate])  VALUES (@AdressenNr,@Duns,@LastUpdate);";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AdressenNr", item.AdressenNr);
					sqlCommand.Parameters.AddWithValue("Duns", item.Duns == null ? (object)DBNull.Value : item.Duns);
					sqlCommand.Parameters.AddWithValue("LastUpdate", item.LastUpdate == null ? (object)DBNull.Value : item.LastUpdate);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity> items)
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
						query += " INSERT INTO [__PRS_AdressenExtension] ([AdressenNr],[Duns],[LastUpdate]) VALUES ( "

							+ "@AdressenNr" + i + ","
							+ "@Duns" + i + ","
							+ "@LastUpdate" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("AdressenNr" + i, item.AdressenNr);
						sqlCommand.Parameters.AddWithValue("Duns" + i, item.Duns == null ? (object)DBNull.Value : item.Duns);
						sqlCommand.Parameters.AddWithValue("LastUpdate" + i, item.LastUpdate == null ? (object)DBNull.Value : item.LastUpdate);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}
		public static int Update(Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__PRS_AdressenExtension] SET [AdressenNr]=@AdressenNr, [Duns]=@Duns, [LastUpdate]=@LastUpdate WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AdressenNr", item.AdressenNr);
				sqlCommand.Parameters.AddWithValue("Duns", item.Duns == null ? (object)DBNull.Value : item.Duns);
				sqlCommand.Parameters.AddWithValue("LastUpdate", item.LastUpdate == null ? (object)DBNull.Value : item.LastUpdate);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity> items)
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
						query += " UPDATE [__PRS_AdressenExtension] SET "

							+ "[AdressenNr]=@AdressenNr" + i + ","
							+ "[Duns]=@Duns" + i + ","
							+ "[LastUpdate]=@LastUpdate" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("AdressenNr" + i, item.AdressenNr);
						sqlCommand.Parameters.AddWithValue("Duns" + i, item.Duns == null ? (object)DBNull.Value : item.Duns);
						sqlCommand.Parameters.AddWithValue("LastUpdate" + i, item.LastUpdate == null ? (object)DBNull.Value : item.LastUpdate);
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
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__PRS_AdressenExtension] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

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

					string query = "DELETE FROM [__PRS_AdressenExtension] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion
		#region Custom Methods
		public static Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity GetByAddressNr(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__PRS_AdressenExtension] WHERE [AdressenNr]=@nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("nr", nr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity GetByAddressNr(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			using(var sqlCommand = new SqlCommand("SELECT * FROM [__PRS_AdressenExtension] WHERE [AdressenNr]=@nr", connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("nr", nr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity> GetByAddressNr(List<int> nrs)
		{
			if(nrs == null || nrs.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__PRS_AdressenExtension] WHERE [AdressenNr] IN ({string.Join(",", nrs)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity>();
			}
		}
		public static Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity GetByDuns(string duns)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__PRS_AdressenExtension] WHERE [Duns]=@duns";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("duns", duns ?? "");

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity GetByDuns(string duns, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();
			string query = "SELECT * FROM [__PRS_AdressenExtension] WHERE [Duns]=@duns";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("duns", duns);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity GetByIdAdressen(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__PRS_AdressenExtension] WHERE [IdAdressen]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity item, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			int response = int.MinValue;
			using(var sqlCommand = new SqlCommand("INSERT INTO [__PRS_AdressenExtension] ([AdressenNr],[Duns],[LastUpdate])  VALUES (@AdressenNr,@Duns,@LastUpdate);SELECT SCOPE_IDENTITY();", sqlConnection, sqlTransaction))
			{
				sqlCommand.Parameters.AddWithValue("AdressenNr", item.AdressenNr);
				sqlCommand.Parameters.AddWithValue("Duns", item.Duns == null ? (object)DBNull.Value : item.Duns);
				sqlCommand.Parameters.AddWithValue("LastUpdate", item.LastUpdate == null ? (object)DBNull.Value : item.LastUpdate);

				var result = sqlCommand.ExecuteScalar();
				response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}

			sqlTransaction.Commit();

			return response;
		}
		public static int Update(Infrastructure.Data.Entities.Tables.PRS.AdressenExtensionEntity item, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			int results = -1;
			using(var sqlCommand = new SqlCommand("UPDATE [__PRS_AdressenExtension] SET [AdressenNr]=@AdressenNr, [Duns]=@Duns, [LastUpdate]=@LastUpdate WHERE [Id]=@Id", sqlConnection, sqlTransaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AdressenNr", item.AdressenNr);
				sqlCommand.Parameters.AddWithValue("Duns", item.Duns == null ? (object)DBNull.Value : item.Duns);
				sqlCommand.Parameters.AddWithValue("LastUpdate", item.LastUpdate == null ? (object)DBNull.Value : item.LastUpdate);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		#endregion
	}
}