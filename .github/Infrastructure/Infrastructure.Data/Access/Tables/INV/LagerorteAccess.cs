using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.INV
{
	public class LagerorteAccess
	{
		#region Default Methods
		public static Entities.Tables.INV.LagerorteEntity Get(int id)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Lagerorte] WHERE [Lagerort_id]=@Id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.INV.LagerorteEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.INV.LagerorteEntity> Get()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Lagerorte]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.INV.LagerorteEntity>();
			}
		}

		public static List<Entities.Tables.INV.LagerorteEntity> GetLagortes()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Lagerorte] WHERE Lagerort_id = 3 Or Lagerort_id = 4 Or Lagerort_id = 8 Or Lagerort_id= 20 Or Lagerort_id= 24 Or Lagerort_id= 58 Or Lagerort_id= 101 ORDER BY Lagerorte.Lagerort";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.INV.LagerorteEntity>();
			}
		}
		public static List<Entities.Tables.INV.LagerorteEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				var result = new List<Entities.Tables.INV.LagerorteEntity>();
				if(ids.Count <= maxQueryNumber)
				{
					result = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					result = new List<Entities.Tables.INV.LagerorteEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						result.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					result.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return result;
			}
			return new List<Entities.Tables.INV.LagerorteEntity>();
		}
		private static List<Entities.Tables.INV.LagerorteEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [Lagerorte] WHERE [Lagerort_id] IN (" + queryIds + ")";

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Entities.Tables.INV.LagerorteEntity>();
				}
			}
			return new List<Entities.Tables.INV.LagerorteEntity>();
		}

		public static int Insert(Entities.Tables.INV.LagerorteEntity element)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "INSERT INTO [Lagerorte]([Lagerort],[Standard]) VALUES (@Lagerort,@Standard);";
				query += "SELECT SCOPE_IDENTITY();";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Lagerort", element.Lagerort == null ? (object)DBNull.Value : element.Lagerort);
				sqlCommand.Parameters.AddWithValue("Standard", element.Standard == null ? (object)DBNull.Value : element.Standard);

				var result = DbExecution.ExecuteScalar(sqlCommand);
				response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}

			return response;
		}
		public static int Insert(List<Entities.Tables.INV.LagerorteEntity> elements)
		{
			if(elements != null && elements.Count > 0)
			{
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE / 4; // Nb params per query
				int result = 0;
				if(elements.Count <= maxParamsNumber)
				{
					result = insert(elements);
				}
				else
				{
					int batchNumber = elements.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						result += insert(elements.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					result += insert(elements.GetRange(batchNumber * maxParamsNumber, elements.Count - batchNumber * maxParamsNumber));
				}
			}

			return -1;
		}
		private static int insert(List<Entities.Tables.INV.LagerorteEntity> elements)
		{
			if(elements != null && elements.Count > 0)
			{
				int response = -1;

				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();

					string query = " INSERT INTO [Lagerorte] ([Lagerort],[Standard]) VALUES ";

					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var element in elements)
					{
						i++;
						query += " ( "

							+ "@Lagerort" + i + ","
							+ "@Standard" + i
								+ "), ";


						sqlCommand.Parameters.AddWithValue("Lagerort" + i, element.Lagerort == null ? (object)DBNull.Value : element.Lagerort);
						sqlCommand.Parameters.AddWithValue("Standard" + i, element.Standard == null ? (object)DBNull.Value : element.Standard);
					}

					query = query.TrimEnd(',');
					query += ';';
					sqlCommand.CommandText = query;

					response = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return response;
			}

			return -1;
		}

		public static int Update(Entities.Tables.INV.LagerorteEntity element)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "UPDATE [Lagerorte] SET [Lagerort]=@Lagerort,[Standard]=@Standard WHERE [Lagerort_id]=@Lagerort_id";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Lagerort_id", element.LagerortId);
				sqlCommand.Parameters.AddWithValue("Lagerort", element.Lagerort == null ? (object)DBNull.Value : element.Lagerort);
				sqlCommand.Parameters.AddWithValue("Standard", element.Standard == null ? (object)DBNull.Value : element.Standard);

				response = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return response;
		}
		public static int Update(List<Entities.Tables.INV.LagerorteEntity> elements)
		{
			if(elements != null && elements.Count > 0)
			{
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE / 4; // Nb params per query
				int result = 0;
				if(elements.Count <= maxParamsNumber)
				{
					result = update(elements);
				}
				else
				{
					int batchNumber = elements.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						result += update(elements.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					result += update(elements.GetRange(batchNumber * maxParamsNumber, elements.Count - batchNumber * maxParamsNumber));
				}
			}

			return -1;
		}
		private static int update(List<Entities.Tables.INV.LagerorteEntity> elements)
		{
			if(elements != null && elements.Count > 0)
			{
				int response = -1;

				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();

					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(Entities.Tables.INV.LagerorteEntity t in elements)
					{
						i++;
						query += " UPDATE [Lagerorte] SET "

							+ "[Lagerort]=@Lagerort" + i + ","
							+ "[Standard]=@Standard" + i
							+ " WHERE [Lagerort_id]=@Lagerort_id[Lagerort_id]=@Lagerort_id" + i
								+ "; ";

						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, t.LagerortId);
						sqlCommand.Parameters.AddWithValue("Lagerort" + i, t.Lagerort == null ? (object)DBNull.Value : t.Lagerort);
						sqlCommand.Parameters.AddWithValue("Standard" + i, t.Standard == null ? (object)DBNull.Value : t.Standard);
					}

					sqlCommand.CommandText = query;

					response = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return response;
			}

			return -1;
		}

		public static int Delete(int id)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "DELETE FROM [Lagerorte] WHERE [Lagerort_id]=@Lagerort_id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Lagerort_id", id);

				response = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return response;
		}
		public static int Delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE;
				int result = 0;
				if(ids.Count <= maxParamsNumber)
				{
					result = delete(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						result += delete(ids.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					result += delete(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber));
				}
			}
			return -1;
		}
		private static int delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int response = -1;
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

					string query = "DELETE FROM [Lagerorte] WHERE [Lagerort_id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					response = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return response;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static Entities.Tables.INV.LagerorteEntity GetWithTransaction(int id, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Lagerorte] WHERE [Lagerort_id]=@Id";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			DbExecution.Fill(sqlCommand, dataTable);


			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.INV.LagerorteEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.INV.LagerorteEntity> GetManufacturingFacilities(List<int> lagerIds)
		{
			if(lagerIds == null || lagerIds.Count <= 0)
				return null;

			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT * FROM Lagerorte WHERE Lagerort_id IN ({string.Join(",", lagerIds)}) ORDER BY Lagerort;";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.INV.LagerorteEntity>();
			}
		}
		public static List<Entities.Tables.INV.LagerorteEntity> GetForOrderReception(List<int> lagerIds)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT * FROM [Lagerorte] WHERE Lagerort LIKE 'haup%' {(lagerIds != null && lagerIds.Count > 0 ? " OR Lagerort_id IN (" + string.Join(",", lagerIds) + ")" : "")} ORDER BY Lagerorte.Lagerort_id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.INV.LagerorteEntity>();
			}
		}
		public static List<Entities.Tables.INV.LagerorteEntity> GetForFAStcklist(List<int> lagerIds)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT * FROM [Lagerorte] WHERE [Lagerort_id] IN (" + string.Join(",", lagerIds) + ") ORDER BY [Lagerort_id]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.INV.LagerorteEntity>();
			}
		}
		public static List<Entities.Tables.INV.LagerorteEntity> GetHauptlager()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT * FROM [Lagerorte] WHERE [Lagerort_id] NOT IN (7,21) AND [Lagerort] NOT LIKE '%BE_TN%' ORDER BY [Lagerort_id]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.INV.LagerorteEntity>();
			}
		}
		#endregion

		#region Helpers
		private static List<Entities.Tables.INV.LagerorteEntity> toList(DataTable dataTable)
		{
			var list = new List<Entities.Tables.INV.LagerorteEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{
				list.Add(new Entities.Tables.INV.LagerorteEntity(dataRow));
			}
			return list;
		}
		#endregion
		public static string GetLagerort(int id)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT Lagerort FROM [Lagerorte] WHERE [Lagerort_id]=@Id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows[0]["Lagerort"].ToString();
			}
			else
			{
				return null;
			}
		}
	}
}
