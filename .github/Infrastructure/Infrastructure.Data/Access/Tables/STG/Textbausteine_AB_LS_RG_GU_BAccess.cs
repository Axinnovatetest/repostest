using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.STG
{

	public class Textbausteine_AB_LS_RG_GU_BAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.STG.Textbausteine_AB_LS_RG_GU_BEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Textbausteine AB LS RG GU B] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.STG.Textbausteine_AB_LS_RG_GU_BEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.STG.Textbausteine_AB_LS_RG_GU_BEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Textbausteine AB LS RG GU B]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.STG.Textbausteine_AB_LS_RG_GU_BEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.STG.Textbausteine_AB_LS_RG_GU_BEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.STG.Textbausteine_AB_LS_RG_GU_BEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.STG.Textbausteine_AB_LS_RG_GU_BEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.STG.Textbausteine_AB_LS_RG_GU_BEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.STG.Textbausteine_AB_LS_RG_GU_BEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.STG.Textbausteine_AB_LS_RG_GU_BEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Textbausteine AB LS RG GU B] WHERE [ID] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.STG.Textbausteine_AB_LS_RG_GU_BEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.STG.Textbausteine_AB_LS_RG_GU_BEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.STG.Textbausteine_AB_LS_RG_GU_BEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.STG.Textbausteine_AB_LS_RG_GU_BEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Textbausteine AB LS RG GU B] ([Auftragsbestätigung],[Bestellung],[Gutschrift],[Lieferschein],[Rechnung])  VALUES (@Auftragsbestätigung,@Bestellung,@Gutschrift,@Lieferschein,@Rechnung); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Auftragsbestätigung", item.Auftragsbestätigung == null ? (object)DBNull.Value : item.Auftragsbestätigung);
					sqlCommand.Parameters.AddWithValue("Bestellung", item.Bestellung == null ? (object)DBNull.Value : item.Bestellung);
					sqlCommand.Parameters.AddWithValue("Gutschrift", item.Gutschrift == null ? (object)DBNull.Value : item.Gutschrift);
					sqlCommand.Parameters.AddWithValue("Lieferschein", item.Lieferschein == null ? (object)DBNull.Value : item.Lieferschein);
					sqlCommand.Parameters.AddWithValue("Rechnung", item.Rechnung == null ? (object)DBNull.Value : item.Rechnung);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.STG.Textbausteine_AB_LS_RG_GU_BEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.STG.Textbausteine_AB_LS_RG_GU_BEntity> items)
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
						query += " INSERT INTO [Textbausteine AB LS RG GU B] ([Auftragsbestätigung],[Bestellung],[Gutschrift],[Lieferschein],[Rechnung]) VALUES ( "

							+ "@Auftragsbestätigung" + i + ","
							+ "@Bestellung" + i + ","
							+ "@Gutschrift" + i + ","
							+ "@Lieferschein" + i + ","
							+ "@Rechnung" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Auftragsbestätigung" + i, item.Auftragsbestätigung == null ? (object)DBNull.Value : item.Auftragsbestätigung);
						sqlCommand.Parameters.AddWithValue("Bestellung" + i, item.Bestellung == null ? (object)DBNull.Value : item.Bestellung);
						sqlCommand.Parameters.AddWithValue("Gutschrift" + i, item.Gutschrift == null ? (object)DBNull.Value : item.Gutschrift);
						sqlCommand.Parameters.AddWithValue("Lieferschein" + i, item.Lieferschein == null ? (object)DBNull.Value : item.Lieferschein);
						sqlCommand.Parameters.AddWithValue("Rechnung" + i, item.Rechnung == null ? (object)DBNull.Value : item.Rechnung);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.STG.Textbausteine_AB_LS_RG_GU_BEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Textbausteine AB LS RG GU B] SET [Auftragsbestätigung]=@Auftragsbestätigung, [Bestellung]=@Bestellung, [Gutschrift]=@Gutschrift, [Lieferschein]=@Lieferschein, [Rechnung]=@Rechnung WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("Auftragsbestätigung", item.Auftragsbestätigung == null ? (object)DBNull.Value : item.Auftragsbestätigung);
				sqlCommand.Parameters.AddWithValue("Bestellung", item.Bestellung == null ? (object)DBNull.Value : item.Bestellung);
				sqlCommand.Parameters.AddWithValue("Gutschrift", item.Gutschrift == null ? (object)DBNull.Value : item.Gutschrift);
				sqlCommand.Parameters.AddWithValue("Lieferschein", item.Lieferschein == null ? (object)DBNull.Value : item.Lieferschein);
				sqlCommand.Parameters.AddWithValue("Rechnung", item.Rechnung == null ? (object)DBNull.Value : item.Rechnung);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.STG.Textbausteine_AB_LS_RG_GU_BEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.STG.Textbausteine_AB_LS_RG_GU_BEntity> items)
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
						query += " UPDATE [Textbausteine AB LS RG GU B] SET "

							+ "[Auftragsbestätigung]=@Auftragsbestätigung" + i + ","
							+ "[Bestellung]=@Bestellung" + i + ","
							+ "[Gutschrift]=@Gutschrift" + i + ","
							+ "[Lieferschein]=@Lieferschein" + i + ","
							+ "[Rechnung]=@Rechnung" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("Auftragsbestätigung" + i, item.Auftragsbestätigung == null ? (object)DBNull.Value : item.Auftragsbestätigung);
						sqlCommand.Parameters.AddWithValue("Bestellung" + i, item.Bestellung == null ? (object)DBNull.Value : item.Bestellung);
						sqlCommand.Parameters.AddWithValue("Gutschrift" + i, item.Gutschrift == null ? (object)DBNull.Value : item.Gutschrift);
						sqlCommand.Parameters.AddWithValue("Lieferschein" + i, item.Lieferschein == null ? (object)DBNull.Value : item.Lieferschein);
						sqlCommand.Parameters.AddWithValue("Rechnung" + i, item.Rechnung == null ? (object)DBNull.Value : item.Rechnung);
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
				string query = "DELETE FROM [Textbausteine AB LS RG GU B] WHERE [ID]=@ID";
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

					string query = "DELETE FROM [Textbausteine AB LS RG GU B] WHERE [ID] IN (" + queryIds + ")";
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
	}
}
