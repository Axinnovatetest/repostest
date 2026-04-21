using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class PszLieferantengruppenAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.PszLieferantengruppenEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_Lieferantengruppen] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.PszLieferantengruppenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.PszLieferantengruppenEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_Lieferantengruppen]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.PszLieferantengruppenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.PszLieferantengruppenEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.PszLieferantengruppenEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.PszLieferantengruppenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.PszLieferantengruppenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.PszLieferantengruppenEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
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

					sqlCommand.CommandText = "SELECT * FROM [PSZ_Lieferantengruppen] WHERE [ID] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.PszLieferantengruppenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.PszLieferantengruppenEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.PszLieferantengruppenEntity item)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [PSZ_Lieferantengruppen] ([Lieferantengruppe])  VALUES (@Lieferantengruppe);";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("Lieferantengruppe", item.Lieferantengruppe == null ? (object)DBNull.Value : item.Lieferantengruppe);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}

				sqlTransaction.Commit();

				return response;
			}
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.PszLieferantengruppenEntity item)
		{
			int results = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "UPDATE [PSZ_Lieferantengruppen] SET [Lieferantengruppe]=@Lieferantengruppe WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("Lieferantengruppe", item.Lieferantengruppe == null ? (object)DBNull.Value : item.Lieferantengruppe);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}

		public static int Delete(int id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [PSZ_Lieferantengruppen] WHERE [ID]=@ID";
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
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE;
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
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
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

					string query = "DELETE FROM [PSZ_Lieferantengruppen] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.BSD.PszLieferantengruppenEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.BSD.PszLieferantengruppenEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.BSD.PszLieferantengruppenEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
