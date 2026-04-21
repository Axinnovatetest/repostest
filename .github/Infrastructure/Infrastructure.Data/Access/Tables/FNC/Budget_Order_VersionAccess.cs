using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class Budget_Order_VersionAccess
	{

		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.Budget_Order_VersionEntity Get(int id_vo)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Budget_Order_Version] WHERE [Id_VO]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id_vo);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Budget_Order_VersionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_Order_VersionEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Budget_Order_Version]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_Order_VersionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_Order_VersionEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.Budget_Order_VersionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.Budget_Order_VersionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_Order_VersionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.Budget_Order_VersionEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [Budget_Order_Version] WHERE [Id_VO] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_Order_VersionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_Order_VersionEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.Budget_Order_VersionEntity item)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Budget_Order_Version] ([Dept_name],[Id_Currency_Order],[Id_Dept],[Id_Land],[Id_Level],[Id_Order],[Id_Project],[Id_Status],[Id_Supplier_VersionOrder],[Id_User],[Land_name],[Nr_version_Order],[Step_Order],[TotalCost_Order],[Version_Order_date])  VALUES (@Dept_name,@Id_Currency_Order,@Id_Dept,@Id_Land,@Id_Level,@Id_Order,@Id_Project,@Id_Status,@Id_Supplier_VersionOrder,@Id_User,@Land_name,@Nr_version_Order,@Step_Order,@TotalCost_Order,@Version_Order_date)";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Dept_name", item.Dept_name == null ? (object)DBNull.Value : item.Dept_name);
					sqlCommand.Parameters.AddWithValue("Id_Currency_Order", item.Id_Currency_Order == null ? (object)DBNull.Value : item.Id_Currency_Order);
					sqlCommand.Parameters.AddWithValue("Id_Dept", item.Id_Dept == null ? (object)DBNull.Value : item.Id_Dept);
					sqlCommand.Parameters.AddWithValue("Id_Land", item.Id_Land == null ? (object)DBNull.Value : item.Id_Land);
					sqlCommand.Parameters.AddWithValue("Id_Level", item.Id_Level == null ? (object)DBNull.Value : item.Id_Level);
					sqlCommand.Parameters.AddWithValue("Id_Order", item.Id_Order);
					sqlCommand.Parameters.AddWithValue("Id_Project", item.Id_Project == null ? (object)DBNull.Value : item.Id_Project);
					sqlCommand.Parameters.AddWithValue("Id_Status", item.Id_Status == null ? (object)DBNull.Value : item.Id_Status);
					sqlCommand.Parameters.AddWithValue("Id_Supplier_VersionOrder", item.Id_Supplier_VersionOrder == null ? (object)DBNull.Value : item.Id_Supplier_VersionOrder);
					sqlCommand.Parameters.AddWithValue("Id_User", item.Id_User);
					sqlCommand.Parameters.AddWithValue("Land_name", item.Land_name == null ? (object)DBNull.Value : item.Land_name);
					sqlCommand.Parameters.AddWithValue("Nr_version_Order", item.Nr_version_Order == null ? (object)DBNull.Value : item.Nr_version_Order);
					sqlCommand.Parameters.AddWithValue("Step_Order", item.Step_Order == null ? (object)DBNull.Value : item.Step_Order);
					sqlCommand.Parameters.AddWithValue("TotalCost_Order", item.TotalCost_Order == null ? (object)DBNull.Value : item.TotalCost_Order);
					sqlCommand.Parameters.AddWithValue("Version_Order_date", item.Version_Order_date == null ? (object)DBNull.Value : item.Version_Order_date);

					DbExecution.ExecuteNonQuery(sqlCommand);
				}

				using(var sqlCommand = new SqlCommand("SELECT [Id_VO] FROM [Budget_Order_Version] WHERE [Id_VO] = @@IDENTITY", sqlConnection, sqlTransaction))
				{
					response = int.Parse(DbExecution.ExecuteScalar(sqlCommand)?.ToString() ?? "-1");
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.Budget_Order_VersionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 16; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.Budget_Order_VersionEntity> items)
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
						query += " INSERT INTO [Budget_Order_Version] ([Dept_name],[Id_Currency_Order],[Id_Dept],[Id_Land],[Id_Level],[Id_Order],[Id_Project],[Id_Status],[Id_Supplier_VersionOrder],[Id_User],[Land_name],[Nr_version_Order],[Step_Order],[TotalCost_Order],[Version_Order_date]) VALUES ( "

							+ "@Dept_name" + i + ","
							+ "@Id_Currency_Order" + i + ","
							+ "@Id_Dept" + i + ","
							+ "@Id_Land" + i + ","
							+ "@Id_Level" + i + ","
							+ "@Id_Order" + i + ","
							+ "@Id_Project" + i + ","
							+ "@Id_Status" + i + ","
							+ "@Id_Supplier_VersionOrder" + i + ","
							+ "@Id_User" + i + ","
							+ "@Land_name" + i + ","
							+ "@Nr_version_Order" + i + ","
							+ "@Step_Order" + i + ","
							+ "@TotalCost_Order" + i + ","
							+ "@Version_Order_date" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Dept_name" + i, item.Dept_name == null ? (object)DBNull.Value : item.Dept_name);
						sqlCommand.Parameters.AddWithValue("Id_Currency_Order" + i, item.Id_Currency_Order == null ? (object)DBNull.Value : item.Id_Currency_Order);
						sqlCommand.Parameters.AddWithValue("Id_Dept" + i, item.Id_Dept == null ? (object)DBNull.Value : item.Id_Dept);
						sqlCommand.Parameters.AddWithValue("Id_Land" + i, item.Id_Land == null ? (object)DBNull.Value : item.Id_Land);
						sqlCommand.Parameters.AddWithValue("Id_Level" + i, item.Id_Level == null ? (object)DBNull.Value : item.Id_Level);
						sqlCommand.Parameters.AddWithValue("Id_Order" + i, item.Id_Order);
						sqlCommand.Parameters.AddWithValue("Id_Project" + i, item.Id_Project == null ? (object)DBNull.Value : item.Id_Project);
						sqlCommand.Parameters.AddWithValue("Id_Status" + i, item.Id_Status == null ? (object)DBNull.Value : item.Id_Status);
						sqlCommand.Parameters.AddWithValue("Id_Supplier_VersionOrder" + i, item.Id_Supplier_VersionOrder == null ? (object)DBNull.Value : item.Id_Supplier_VersionOrder);
						sqlCommand.Parameters.AddWithValue("Id_User" + i, item.Id_User);
						sqlCommand.Parameters.AddWithValue("Land_name" + i, item.Land_name == null ? (object)DBNull.Value : item.Land_name);
						sqlCommand.Parameters.AddWithValue("Nr_version_Order" + i, item.Nr_version_Order == null ? (object)DBNull.Value : item.Nr_version_Order);
						sqlCommand.Parameters.AddWithValue("Step_Order" + i, item.Step_Order == null ? (object)DBNull.Value : item.Step_Order);
						sqlCommand.Parameters.AddWithValue("TotalCost_Order" + i, item.TotalCost_Order == null ? (object)DBNull.Value : item.TotalCost_Order);
						sqlCommand.Parameters.AddWithValue("Version_Order_date" + i, item.Version_Order_date == null ? (object)DBNull.Value : item.Version_Order_date);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.Budget_Order_VersionEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [Budget_Order_Version] SET [Dept_name]=@Dept_name, [Id_Currency_Order]=@Id_Currency_Order, [Id_Dept]=@Id_Dept, [Id_Land]=@Id_Land, [Id_Level]=@Id_Level, [Id_Order]=@Id_Order, [Id_Project]=@Id_Project, [Id_Status]=@Id_Status, [Id_Supplier_VersionOrder]=@Id_Supplier_VersionOrder, [Id_User]=@Id_User, [Land_name]=@Land_name, [Nr_version_Order]=@Nr_version_Order, [Step_Order]=@Step_Order, [TotalCost_Order]=@TotalCost_Order, [Version_Order_date]=@Version_Order_date WHERE [Id_VO]=@Id_VO";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id_VO", item.Id_VO);
				sqlCommand.Parameters.AddWithValue("Dept_name", item.Dept_name == null ? (object)DBNull.Value : item.Dept_name);
				sqlCommand.Parameters.AddWithValue("Id_Currency_Order", item.Id_Currency_Order == null ? (object)DBNull.Value : item.Id_Currency_Order);
				sqlCommand.Parameters.AddWithValue("Id_Dept", item.Id_Dept == null ? (object)DBNull.Value : item.Id_Dept);
				sqlCommand.Parameters.AddWithValue("Id_Land", item.Id_Land == null ? (object)DBNull.Value : item.Id_Land);
				sqlCommand.Parameters.AddWithValue("Id_Level", item.Id_Level == null ? (object)DBNull.Value : item.Id_Level);
				sqlCommand.Parameters.AddWithValue("Id_Order", item.Id_Order);
				sqlCommand.Parameters.AddWithValue("Id_Project", item.Id_Project == null ? (object)DBNull.Value : item.Id_Project);
				sqlCommand.Parameters.AddWithValue("Id_Status", item.Id_Status == null ? (object)DBNull.Value : item.Id_Status);
				sqlCommand.Parameters.AddWithValue("Id_Supplier_VersionOrder", item.Id_Supplier_VersionOrder == null ? (object)DBNull.Value : item.Id_Supplier_VersionOrder);
				sqlCommand.Parameters.AddWithValue("Id_User", item.Id_User);
				sqlCommand.Parameters.AddWithValue("Land_name", item.Land_name == null ? (object)DBNull.Value : item.Land_name);
				sqlCommand.Parameters.AddWithValue("Nr_version_Order", item.Nr_version_Order == null ? (object)DBNull.Value : item.Nr_version_Order);
				sqlCommand.Parameters.AddWithValue("Step_Order", item.Step_Order == null ? (object)DBNull.Value : item.Step_Order);
				sqlCommand.Parameters.AddWithValue("TotalCost_Order", item.TotalCost_Order == null ? (object)DBNull.Value : item.TotalCost_Order);
				sqlCommand.Parameters.AddWithValue("Version_Order_date", item.Version_Order_date == null ? (object)DBNull.Value : item.Version_Order_date);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.Budget_Order_VersionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 16; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.Budget_Order_VersionEntity> items)
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
						query += " UPDATE [Budget_Order_Version] SET "

							+ "[Dept_name]=@Dept_name" + i + ","
							+ "[Id_Currency_Order]=@Id_Currency_Order" + i + ","
							+ "[Id_Dept]=@Id_Dept" + i + ","
							+ "[Id_Land]=@Id_Land" + i + ","
							+ "[Id_Level]=@Id_Level" + i + ","
							+ "[Id_Order]=@Id_Order" + i + ","
							+ "[Id_Project]=@Id_Project" + i + ","
							+ "[Id_Status]=@Id_Status" + i + ","
							+ "[Id_Supplier_VersionOrder]=@Id_Supplier_VersionOrder" + i + ","
							+ "[Id_User]=@Id_User" + i + ","
							+ "[Land_name]=@Land_name" + i + ","
							+ "[Nr_version_Order]=@Nr_version_Order" + i + ","
							+ "[Step_Order]=@Step_Order" + i + ","
							+ "[TotalCost_Order]=@TotalCost_Order" + i + ","
							+ "[Version_Order_date]=@Version_Order_date" + i + " WHERE [Id_VO]=@Id_VO" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id_VO" + i, item.Id_VO);
						sqlCommand.Parameters.AddWithValue("Dept_name" + i, item.Dept_name == null ? (object)DBNull.Value : item.Dept_name);
						sqlCommand.Parameters.AddWithValue("Id_Currency_Order" + i, item.Id_Currency_Order == null ? (object)DBNull.Value : item.Id_Currency_Order);
						sqlCommand.Parameters.AddWithValue("Id_Dept" + i, item.Id_Dept == null ? (object)DBNull.Value : item.Id_Dept);
						sqlCommand.Parameters.AddWithValue("Id_Land" + i, item.Id_Land == null ? (object)DBNull.Value : item.Id_Land);
						sqlCommand.Parameters.AddWithValue("Id_Level" + i, item.Id_Level == null ? (object)DBNull.Value : item.Id_Level);
						sqlCommand.Parameters.AddWithValue("Id_Order" + i, item.Id_Order);
						sqlCommand.Parameters.AddWithValue("Id_Project" + i, item.Id_Project == null ? (object)DBNull.Value : item.Id_Project);
						sqlCommand.Parameters.AddWithValue("Id_Status" + i, item.Id_Status == null ? (object)DBNull.Value : item.Id_Status);
						sqlCommand.Parameters.AddWithValue("Id_Supplier_VersionOrder" + i, item.Id_Supplier_VersionOrder == null ? (object)DBNull.Value : item.Id_Supplier_VersionOrder);
						sqlCommand.Parameters.AddWithValue("Id_User" + i, item.Id_User);
						sqlCommand.Parameters.AddWithValue("Land_name" + i, item.Land_name == null ? (object)DBNull.Value : item.Land_name);
						sqlCommand.Parameters.AddWithValue("Nr_version_Order" + i, item.Nr_version_Order == null ? (object)DBNull.Value : item.Nr_version_Order);
						sqlCommand.Parameters.AddWithValue("Step_Order" + i, item.Step_Order == null ? (object)DBNull.Value : item.Step_Order);
						sqlCommand.Parameters.AddWithValue("TotalCost_Order" + i, item.TotalCost_Order == null ? (object)DBNull.Value : item.TotalCost_Order);
						sqlCommand.Parameters.AddWithValue("Version_Order_date" + i, item.Version_Order_date == null ? (object)DBNull.Value : item.Version_Order_date);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int id_vo)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Budget_Order_Version] WHERE [Id_VO]=@Id_VO";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id_VO", id_vo);

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

					string query = "DELETE FROM [Budget_Order_Version] WHERE [Id_VO] IN (" + queryIds + ")";
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

		private static List<Infrastructure.Data.Entities.Tables.FNC.Budget_Order_VersionEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.Budget_Order_VersionEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.Budget_Order_VersionEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
