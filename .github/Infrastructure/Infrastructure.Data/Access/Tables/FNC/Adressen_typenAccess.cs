using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class Adressen_typenAccess
	{

		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.Adressen_typenEntity Get(int id_typ)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Adressen_typen] WHERE [ID_typ]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id_typ);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Adressen_typenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.Adressen_typenEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Adressen_typen]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Adressen_typenEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Adressen_typenEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.Adressen_typenEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.Adressen_typenEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Adressen_typenEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.Adressen_typenEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [Adressen_typen] WHERE [ID_typ] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.Adressen_typenEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Adressen_typenEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.Adressen_typenEntity item)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Adressen_typen] ([Bezeichnung])  VALUES (@Bezeichnung)";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Bezeichnung", item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);

					sqlCommand.ExecuteNonQuery();
				}

				using(var sqlCommand = new SqlCommand("SELECT [ID_typ] FROM [Adressen_typen] WHERE [ID_typ] = @@IDENTITY", sqlConnection, sqlTransaction))
				{
					response = int.Parse(sqlCommand.ExecuteScalar()?.ToString() ?? "-1");
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.Adressen_typenEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 2; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.Adressen_typenEntity> items)
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
						query += " INSERT INTO [Adressen_typen] ([Bezeichnung]) VALUES ( "

							+ "@Bezeichnung" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Bezeichnung" + i, item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.Adressen_typenEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [Adressen_typen] SET [Bezeichnung]=@Bezeichnung WHERE [ID_typ]=@ID_typ";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID_typ", item.ID_typ);
				sqlCommand.Parameters.AddWithValue("Bezeichnung", item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.Adressen_typenEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 2; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.Adressen_typenEntity> items)
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
						query += " UPDATE [Adressen_typen] SET "

							+ "[Bezeichnung]=@Bezeichnung" + i + " WHERE [ID_typ]=@ID_typ" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID_typ" + i, item.ID_typ);
						sqlCommand.Parameters.AddWithValue("Bezeichnung" + i, item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int id_typ)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Adressen_typen] WHERE [ID_typ]=@ID_typ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ID_typ", id_typ);

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

					string query = "DELETE FROM [Adressen_typen] WHERE [ID_typ] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods


		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.FNC.Adressen_typenEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.Adressen_typenEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.Adressen_typenEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
