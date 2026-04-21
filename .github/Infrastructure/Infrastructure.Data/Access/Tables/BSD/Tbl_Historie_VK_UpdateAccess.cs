using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class Tbl_Historie_VK_UpdateAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.Tbl_Historie_VK_UpdateEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Tbl_Historie_VK_Update] WHERE [id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.Tbl_Historie_VK_UpdateEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.Tbl_Historie_VK_UpdateEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Tbl_Historie_VK_Update]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.Tbl_Historie_VK_UpdateEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.Tbl_Historie_VK_UpdateEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.Tbl_Historie_VK_UpdateEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.Tbl_Historie_VK_UpdateEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.Tbl_Historie_VK_UpdateEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.Tbl_Historie_VK_UpdateEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.Tbl_Historie_VK_UpdateEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Tbl_Historie_VK_Update] WHERE [id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.Tbl_Historie_VK_UpdateEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.Tbl_Historie_VK_UpdateEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.Tbl_Historie_VK_UpdateEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.Tbl_Historie_VK_UpdateEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Tbl_Historie_VK_Update] ([Alte Preis],[Artikelnummer],[Datum],[Neue Preis],[User])  VALUES (@Alte_Preis,@Artikelnummer,@Datum,@Neue_Preis,@User); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Alte_Preis", item.Alte_Preis == null ? (object)DBNull.Value : item.Alte_Preis);
					sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
					sqlCommand.Parameters.AddWithValue("Neue_Preis", item.Neue_Preis == null ? (object)DBNull.Value : item.Neue_Preis);
					sqlCommand.Parameters.AddWithValue("User", item.User == null ? (object)DBNull.Value : item.User);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.Tbl_Historie_VK_UpdateEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 6; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.Tbl_Historie_VK_UpdateEntity> items)
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
						query += " INSERT INTO [Tbl_Historie_VK_Update] ([Alte Preis],[Artikelnummer],[Datum],[Neue Preis],[User]) VALUES ( "

							+ "@Alte_Preis" + i + ","
							+ "@Artikelnummer" + i + ","
							+ "@Datum" + i + ","
							+ "@Neue_Preis" + i + ","
							+ "@User" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Alte_Preis" + i, item.Alte_Preis == null ? (object)DBNull.Value : item.Alte_Preis);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("Neue_Preis" + i, item.Neue_Preis == null ? (object)DBNull.Value : item.Neue_Preis);
						sqlCommand.Parameters.AddWithValue("User" + i, item.User == null ? (object)DBNull.Value : item.User);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.Tbl_Historie_VK_UpdateEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Tbl_Historie_VK_Update] SET [Alte Preis]=@Alte_Preis, [Artikelnummer]=@Artikelnummer, [Datum]=@Datum, [Neue Preis]=@Neue_Preis, [User]=@User WHERE [id]=@id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("id", item.id);
				sqlCommand.Parameters.AddWithValue("Alte_Preis", item.Alte_Preis == null ? (object)DBNull.Value : item.Alte_Preis);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("Datum", item.Datum == null ? (object)DBNull.Value : item.Datum);
				sqlCommand.Parameters.AddWithValue("Neue_Preis", item.Neue_Preis == null ? (object)DBNull.Value : item.Neue_Preis);
				sqlCommand.Parameters.AddWithValue("User", item.User == null ? (object)DBNull.Value : item.User);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.Tbl_Historie_VK_UpdateEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 6; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.Tbl_Historie_VK_UpdateEntity> items)
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
						query += " UPDATE [Tbl_Historie_VK_Update] SET "

							+ "[Alte Preis]=@Alte_Preis" + i + ","
							+ "[Artikelnummer]=@Artikelnummer" + i + ","
							+ "[Datum]=@Datum" + i + ","
							+ "[Neue Preis]=@Neue_Preis" + i + ","
							+ "[User]=@User" + i + " WHERE [id]=@id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("id" + i, item.id);
						sqlCommand.Parameters.AddWithValue("Alte_Preis" + i, item.Alte_Preis == null ? (object)DBNull.Value : item.Alte_Preis);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Datum" + i, item.Datum == null ? (object)DBNull.Value : item.Datum);
						sqlCommand.Parameters.AddWithValue("Neue_Preis" + i, item.Neue_Preis == null ? (object)DBNull.Value : item.Neue_Preis);
						sqlCommand.Parameters.AddWithValue("User" + i, item.User == null ? (object)DBNull.Value : item.User);
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
				string query = "DELETE FROM [Tbl_Historie_VK_Update] WHERE [id]=@id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);

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

					string query = "DELETE FROM [Tbl_Historie_VK_Update] WHERE [id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods


		public static List<Infrastructure.Data.Entities.Tables.BSD.Tbl_Historie_VK_UpdateEntity> Get(Settings.SortingModel sorting, Settings.PaginModel paging, string articleNumber, DateTime? from = null, DateTime? to = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				List<string> where = new List<string>();
				string query = "SELECT * FROM [Tbl_Historie_VK_Update]";

				// -
				if(!string.IsNullOrWhiteSpace(articleNumber))
					where.Add($"[Artikelnummer] LIKE '{articleNumber.Trim()}%'");
				if(from.HasValue)
					where.Add($"[Datum]>='{from.Value.ToString("yyyyMMdd")}'");
				if(to.HasValue)
					where.Add($"[Datum]<='{to.Value.ToString("yyyyMMdd")}'");

				if(where.Count > 0)
					query += $" WHERE {string.Join(" AND ", where)}";

				#region >>>>> pagination <<<<<<<
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY [Id] DESC ";
				}
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				#endregion pagination

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.Tbl_Historie_VK_UpdateEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.Tbl_Historie_VK_UpdateEntity>();
			}
		}
		public static int Get_Count(string articleNumber, DateTime? from = null, DateTime? to = null)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				List<string> where = new List<string>();
				string query = "SELECT COUNT(*) FROM [Tbl_Historie_VK_Update]";

				// -
				if(!string.IsNullOrWhiteSpace(articleNumber))
					where.Add($"[Artikelnummer] LIKE '{articleNumber.Trim()}%'");
				if(from.HasValue)
					where.Add($"[Datum]>='{from.Value.ToString("yyyyMMdd")}'");
				if(to.HasValue)
					where.Add($"[Datum]<='{to.Value.ToString("yyyyMMdd")}'");

				if(where.Count > 0)
					query += $" WHERE {string.Join(" AND ", where)}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var v) ? v : 0;
			}
		}
		#endregion
	}
}
