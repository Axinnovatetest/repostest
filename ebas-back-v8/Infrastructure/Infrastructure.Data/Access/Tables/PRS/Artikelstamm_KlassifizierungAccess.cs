using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.PRS
{

	public class ArtikelstammKlassifizierungAccess
	{

		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.Artikelstamm_KlassifizierungEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Artikelstamm_Klassifizierung] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.Artikelstamm_KlassifizierungEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.Artikelstamm_KlassifizierungEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Artikelstamm_Klassifizierung]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.Artikelstamm_KlassifizierungEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.Artikelstamm_KlassifizierungEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.Artikelstamm_KlassifizierungEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.Artikelstamm_KlassifizierungEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.Artikelstamm_KlassifizierungEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.Artikelstamm_KlassifizierungEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [Artikelstamm_Klassifizierung] WHERE [ID] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.Artikelstamm_KlassifizierungEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.Artikelstamm_KlassifizierungEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.Artikelstamm_KlassifizierungEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Artikelstamm_Klassifizierung] ([Bezeichnung],[Gewerk],[Klassifizierung],[Kupferzahl],[Nummernkreis])  VALUES (@Bezeichnung,@Gewerk,@Klassifizierung,@Kupferzahl,@Nummernkreis);";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Bezeichnung", item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
					sqlCommand.Parameters.AddWithValue("Gewerk", item.Gewerk == null ? (object)DBNull.Value : item.Gewerk);
					sqlCommand.Parameters.AddWithValue("Klassifizierung", item.Klassifizierung == null ? (object)DBNull.Value : item.Klassifizierung);
					sqlCommand.Parameters.AddWithValue("Kupferzahl", item.Kupferzahl == null ? (object)DBNull.Value : item.Kupferzahl);
					sqlCommand.Parameters.AddWithValue("Nummernkreis", item.Nummernkreis == null ? (object)DBNull.Value : item.Nummernkreis);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.PRS.Artikelstamm_KlassifizierungEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE / 6; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.PRS.Artikelstamm_KlassifizierungEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [Artikelstamm_Klassifizierung] ([Bezeichnung],[Gewerk],[Klassifizierung],[Kupferzahl],[Nummernkreis]) VALUES ( "

							+ "@Bezeichnung" + i + ","
							+ "@Gewerk" + i + ","
							+ "@Klassifizierung" + i + ","
							+ "@Kupferzahl" + i + ","
							+ "@Nummernkreis" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Bezeichnung" + i, item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
						sqlCommand.Parameters.AddWithValue("Gewerk" + i, item.Gewerk == null ? (object)DBNull.Value : item.Gewerk);
						sqlCommand.Parameters.AddWithValue("Klassifizierung" + i, item.Klassifizierung == null ? (object)DBNull.Value : item.Klassifizierung);
						sqlCommand.Parameters.AddWithValue("Kupferzahl" + i, item.Kupferzahl == null ? (object)DBNull.Value : item.Kupferzahl);
						sqlCommand.Parameters.AddWithValue("Nummernkreis" + i, item.Nummernkreis == null ? (object)DBNull.Value : item.Nummernkreis);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.PRS.Artikelstamm_KlassifizierungEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Artikelstamm_Klassifizierung] SET [Bezeichnung]=@Bezeichnung, [Gewerk]=@Gewerk, [Klassifizierung]=@Klassifizierung, [Kupferzahl]=@Kupferzahl, [Nummernkreis]=@Nummernkreis WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("Bezeichnung", item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
				sqlCommand.Parameters.AddWithValue("Gewerk", item.Gewerk == null ? (object)DBNull.Value : item.Gewerk);
				sqlCommand.Parameters.AddWithValue("Klassifizierung", item.Klassifizierung == null ? (object)DBNull.Value : item.Klassifizierung);
				sqlCommand.Parameters.AddWithValue("Kupferzahl", item.Kupferzahl == null ? (object)DBNull.Value : item.Kupferzahl);
				sqlCommand.Parameters.AddWithValue("Nummernkreis", item.Nummernkreis == null ? (object)DBNull.Value : item.Nummernkreis);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRS.Artikelstamm_KlassifizierungEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE / 6; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.PRS.Artikelstamm_KlassifizierungEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [Artikelstamm_Klassifizierung] SET "

							+ "[Bezeichnung]=@Bezeichnung" + i + ","
							+ "[Gewerk]=@Gewerk" + i + ","
							+ "[Klassifizierung]=@Klassifizierung" + i + ","
							+ "[Kupferzahl]=@Kupferzahl" + i + ","
							+ "[Nummernkreis]=@Nummernkreis" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("Bezeichnung" + i, item.Bezeichnung == null ? (object)DBNull.Value : item.Bezeichnung);
						sqlCommand.Parameters.AddWithValue("Gewerk" + i, item.Gewerk == null ? (object)DBNull.Value : item.Gewerk);
						sqlCommand.Parameters.AddWithValue("Klassifizierung" + i, item.Klassifizierung == null ? (object)DBNull.Value : item.Klassifizierung);
						sqlCommand.Parameters.AddWithValue("Kupferzahl" + i, item.Kupferzahl == null ? (object)DBNull.Value : item.Kupferzahl);
						sqlCommand.Parameters.AddWithValue("Nummernkreis" + i, item.Nummernkreis == null ? (object)DBNull.Value : item.Nummernkreis);
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
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Artikelstamm_Klassifizierung] WHERE [ID]=@ID";
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

					string query = "DELETE FROM [Artikelstamm_Klassifizierung] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.PRS.Artikelstamm_KlassifizierungEntity> GetByName(string name)
		{
			if(string.IsNullOrWhiteSpace(name))
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Artikelstamm_Klassifizierung] WHERE RTRIM(LTRIM([Klassifizierung]))=@name";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("name", name.Trim());

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.Artikelstamm_KlassifizierungEntity>();
			}
		}

		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.PRS.Artikelstamm_KlassifizierungEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.PRS.Artikelstamm_KlassifizierungEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.PRS.Artikelstamm_KlassifizierungEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
