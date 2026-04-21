using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class Budget_Article_VersionAccess
	{

		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.Budget_Article_VersionEntity Get(int id_aov)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_Article_Version] WHERE [Id_AOV]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id_aov);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Budget_Article_VersionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_Article_VersionEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_Article_Version]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_Article_VersionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_Article_VersionEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.Budget_Article_VersionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.Budget_Article_VersionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_Article_VersionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.Budget_Article_VersionEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [__FNC_Article_Version] WHERE [Id_AOV] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_Article_VersionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_Article_VersionEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.Budget_Article_VersionEntity item)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__FNC_Article_Version] ([Action_Version_Article],[Currency_Version_Article],[Dept_name_VersionArticle],[Id_Article],[Id_Currency_Version_Article],[Id_Dept_VersionArticle],[Id_Land_VersionArticle],[Id_Level_VersionArticle],[Id_Order_Version],[Id_Project_VersionArticle],[Id_Status_VersionArticle],[Id_Supplier_VersionArticle],[Id_User_VersionArticle],[Land_name_VersionArticle],[Quantity_VersionArticle],[TotalCost__VersionArticle],[Unit_Price_VersionArticle],[Version_Article_date])  VALUES (@Action_Version_Article,@Currency_Version_Article,@Dept_name_VersionArticle,@Id_Article,@Id_Currency_Version_Article,@Id_Dept_VersionArticle,@Id_Land_VersionArticle,@Id_Level_VersionArticle,@Id_Order_Version,@Id_Project_VersionArticle,@Id_Status_VersionArticle,@Id_Supplier_VersionArticle,@Id_User_VersionArticle,@Land_name_VersionArticle,@Quantity_VersionArticle,@TotalCost__VersionArticle,@Unit_Price_VersionArticle,@Version_Article_date)";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Action_Version_Article", item.Action_Version_Article == null ? (object)DBNull.Value : item.Action_Version_Article);
					sqlCommand.Parameters.AddWithValue("Currency_Version_Article", item.Currency_Version_Article == null ? (object)DBNull.Value : item.Currency_Version_Article);
					sqlCommand.Parameters.AddWithValue("Dept_name_VersionArticle", item.Dept_name_VersionArticle == null ? (object)DBNull.Value : item.Dept_name_VersionArticle);
					sqlCommand.Parameters.AddWithValue("Id_Article", item.Id_Article);
					sqlCommand.Parameters.AddWithValue("Id_Currency_Version_Article", item.Id_Currency_Version_Article == null ? (object)DBNull.Value : item.Id_Currency_Version_Article);
					sqlCommand.Parameters.AddWithValue("Id_Dept_VersionArticle", item.Id_Dept_VersionArticle == null ? (object)DBNull.Value : item.Id_Dept_VersionArticle);
					sqlCommand.Parameters.AddWithValue("Id_Land_VersionArticle", item.Id_Land_VersionArticle == null ? (object)DBNull.Value : item.Id_Land_VersionArticle);
					sqlCommand.Parameters.AddWithValue("Id_Level_VersionArticle", item.Id_Level_VersionArticle == null ? (object)DBNull.Value : item.Id_Level_VersionArticle);
					sqlCommand.Parameters.AddWithValue("Id_Order_Version", item.Id_Order_Version);
					sqlCommand.Parameters.AddWithValue("Id_Project_VersionArticle", item.Id_Project_VersionArticle == null ? (object)DBNull.Value : item.Id_Project_VersionArticle);
					sqlCommand.Parameters.AddWithValue("Id_Status_VersionArticle", item.Id_Status_VersionArticle == null ? (object)DBNull.Value : item.Id_Status_VersionArticle);
					sqlCommand.Parameters.AddWithValue("Id_Supplier_VersionArticle", item.Id_Supplier_VersionArticle == null ? (object)DBNull.Value : item.Id_Supplier_VersionArticle);
					sqlCommand.Parameters.AddWithValue("Id_User_VersionArticle", item.Id_User_VersionArticle);
					sqlCommand.Parameters.AddWithValue("Land_name_VersionArticle", item.Land_name_VersionArticle == null ? (object)DBNull.Value : item.Land_name_VersionArticle);
					sqlCommand.Parameters.AddWithValue("Quantity_VersionArticle", item.Quantity_VersionArticle == null ? (object)DBNull.Value : item.Quantity_VersionArticle);
					sqlCommand.Parameters.AddWithValue("TotalCost__VersionArticle", item.TotalCost__VersionArticle == null ? (object)DBNull.Value : item.TotalCost__VersionArticle);
					sqlCommand.Parameters.AddWithValue("Unit_Price_VersionArticle", item.Unit_Price_VersionArticle == null ? (object)DBNull.Value : item.Unit_Price_VersionArticle);
					sqlCommand.Parameters.AddWithValue("Version_Article_date", item.Version_Article_date == null ? (object)DBNull.Value : item.Version_Article_date);

					sqlCommand.ExecuteNonQuery();
				}

				using(var sqlCommand = new SqlCommand("SELECT [Id_AOV] FROM [__FNC_Article_Version] WHERE [Id_AOV] = @@IDENTITY", sqlConnection, sqlTransaction))
				{
					response = int.Parse(sqlCommand.ExecuteScalar()?.ToString() ?? "-1");
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.Budget_Article_VersionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 19; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.Budget_Article_VersionEntity> items)
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
						query += " INSERT INTO [__FNC_Article_Version] ([Action_Version_Article],[Currency_Version_Article],[Dept_name_VersionArticle],[Id_Article],[Id_Currency_Version_Article],[Id_Dept_VersionArticle],[Id_Land_VersionArticle],[Id_Level_VersionArticle],[Id_Order_Version],[Id_Project_VersionArticle],[Id_Status_VersionArticle],[Id_Supplier_VersionArticle],[Id_User_VersionArticle],[Land_name_VersionArticle],[Quantity_VersionArticle],[TotalCost__VersionArticle],[Unit_Price_VersionArticle],[Version_Article_date]) VALUES ( "

							+ "@Action_Version_Article" + i + ","
							+ "@Currency_Version_Article" + i + ","
							+ "@Dept_name_VersionArticle" + i + ","
							+ "@Id_Article" + i + ","
							+ "@Id_Currency_Version_Article" + i + ","
							+ "@Id_Dept_VersionArticle" + i + ","
							+ "@Id_Land_VersionArticle" + i + ","
							+ "@Id_Level_VersionArticle" + i + ","
							+ "@Id_Order_Version" + i + ","
							+ "@Id_Project_VersionArticle" + i + ","
							+ "@Id_Status_VersionArticle" + i + ","
							+ "@Id_Supplier_VersionArticle" + i + ","
							+ "@Id_User_VersionArticle" + i + ","
							+ "@Land_name_VersionArticle" + i + ","
							+ "@Quantity_VersionArticle" + i + ","
							+ "@TotalCost__VersionArticle" + i + ","
							+ "@Unit_Price_VersionArticle" + i + ","
							+ "@Version_Article_date" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Action_Version_Article" + i, item.Action_Version_Article == null ? (object)DBNull.Value : item.Action_Version_Article);
						sqlCommand.Parameters.AddWithValue("Currency_Version_Article" + i, item.Currency_Version_Article == null ? (object)DBNull.Value : item.Currency_Version_Article);
						sqlCommand.Parameters.AddWithValue("Dept_name_VersionArticle" + i, item.Dept_name_VersionArticle == null ? (object)DBNull.Value : item.Dept_name_VersionArticle);
						sqlCommand.Parameters.AddWithValue("Id_Article" + i, item.Id_Article);
						sqlCommand.Parameters.AddWithValue("Id_Currency_Version_Article" + i, item.Id_Currency_Version_Article == null ? (object)DBNull.Value : item.Id_Currency_Version_Article);
						sqlCommand.Parameters.AddWithValue("Id_Dept_VersionArticle" + i, item.Id_Dept_VersionArticle == null ? (object)DBNull.Value : item.Id_Dept_VersionArticle);
						sqlCommand.Parameters.AddWithValue("Id_Land_VersionArticle" + i, item.Id_Land_VersionArticle == null ? (object)DBNull.Value : item.Id_Land_VersionArticle);
						sqlCommand.Parameters.AddWithValue("Id_Level_VersionArticle" + i, item.Id_Level_VersionArticle == null ? (object)DBNull.Value : item.Id_Level_VersionArticle);
						sqlCommand.Parameters.AddWithValue("Id_Order_Version" + i, item.Id_Order_Version);
						sqlCommand.Parameters.AddWithValue("Id_Project_VersionArticle" + i, item.Id_Project_VersionArticle == null ? (object)DBNull.Value : item.Id_Project_VersionArticle);
						sqlCommand.Parameters.AddWithValue("Id_Status_VersionArticle" + i, item.Id_Status_VersionArticle == null ? (object)DBNull.Value : item.Id_Status_VersionArticle);
						sqlCommand.Parameters.AddWithValue("Id_Supplier_VersionArticle" + i, item.Id_Supplier_VersionArticle == null ? (object)DBNull.Value : item.Id_Supplier_VersionArticle);
						sqlCommand.Parameters.AddWithValue("Id_User_VersionArticle" + i, item.Id_User_VersionArticle);
						sqlCommand.Parameters.AddWithValue("Land_name_VersionArticle" + i, item.Land_name_VersionArticle == null ? (object)DBNull.Value : item.Land_name_VersionArticle);
						sqlCommand.Parameters.AddWithValue("Quantity_VersionArticle" + i, item.Quantity_VersionArticle == null ? (object)DBNull.Value : item.Quantity_VersionArticle);
						sqlCommand.Parameters.AddWithValue("TotalCost__VersionArticle" + i, item.TotalCost__VersionArticle == null ? (object)DBNull.Value : item.TotalCost__VersionArticle);
						sqlCommand.Parameters.AddWithValue("Unit_Price_VersionArticle" + i, item.Unit_Price_VersionArticle == null ? (object)DBNull.Value : item.Unit_Price_VersionArticle);
						sqlCommand.Parameters.AddWithValue("Version_Article_date" + i, item.Version_Article_date == null ? (object)DBNull.Value : item.Version_Article_date);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.Budget_Article_VersionEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_Article_Version] SET [Action_Version_Article]=@Action_Version_Article, [Currency_Version_Article]=@Currency_Version_Article, [Dept_name_VersionArticle]=@Dept_name_VersionArticle, [Id_Article]=@Id_Article, [Id_Currency_Version_Article]=@Id_Currency_Version_Article, [Id_Dept_VersionArticle]=@Id_Dept_VersionArticle, [Id_Land_VersionArticle]=@Id_Land_VersionArticle, [Id_Level_VersionArticle]=@Id_Level_VersionArticle, [Id_Order_Version]=@Id_Order_Version, [Id_Project_VersionArticle]=@Id_Project_VersionArticle, [Id_Status_VersionArticle]=@Id_Status_VersionArticle, [Id_Supplier_VersionArticle]=@Id_Supplier_VersionArticle, [Id_User_VersionArticle]=@Id_User_VersionArticle, [Land_name_VersionArticle]=@Land_name_VersionArticle, [Quantity_VersionArticle]=@Quantity_VersionArticle, [TotalCost__VersionArticle]=@TotalCost__VersionArticle, [Unit_Price_VersionArticle]=@Unit_Price_VersionArticle, [Version_Article_date]=@Version_Article_date WHERE [Id_AOV]=@Id_AOV";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id_AOV", item.Id_AOV);
				sqlCommand.Parameters.AddWithValue("Action_Version_Article", item.Action_Version_Article == null ? (object)DBNull.Value : item.Action_Version_Article);
				sqlCommand.Parameters.AddWithValue("Currency_Version_Article", item.Currency_Version_Article == null ? (object)DBNull.Value : item.Currency_Version_Article);
				sqlCommand.Parameters.AddWithValue("Dept_name_VersionArticle", item.Dept_name_VersionArticle == null ? (object)DBNull.Value : item.Dept_name_VersionArticle);
				sqlCommand.Parameters.AddWithValue("Id_Article", item.Id_Article);
				sqlCommand.Parameters.AddWithValue("Id_Currency_Version_Article", item.Id_Currency_Version_Article == null ? (object)DBNull.Value : item.Id_Currency_Version_Article);
				sqlCommand.Parameters.AddWithValue("Id_Dept_VersionArticle", item.Id_Dept_VersionArticle == null ? (object)DBNull.Value : item.Id_Dept_VersionArticle);
				sqlCommand.Parameters.AddWithValue("Id_Land_VersionArticle", item.Id_Land_VersionArticle == null ? (object)DBNull.Value : item.Id_Land_VersionArticle);
				sqlCommand.Parameters.AddWithValue("Id_Level_VersionArticle", item.Id_Level_VersionArticle == null ? (object)DBNull.Value : item.Id_Level_VersionArticle);
				sqlCommand.Parameters.AddWithValue("Id_Order_Version", item.Id_Order_Version);
				sqlCommand.Parameters.AddWithValue("Id_Project_VersionArticle", item.Id_Project_VersionArticle == null ? (object)DBNull.Value : item.Id_Project_VersionArticle);
				sqlCommand.Parameters.AddWithValue("Id_Status_VersionArticle", item.Id_Status_VersionArticle == null ? (object)DBNull.Value : item.Id_Status_VersionArticle);
				sqlCommand.Parameters.AddWithValue("Id_Supplier_VersionArticle", item.Id_Supplier_VersionArticle == null ? (object)DBNull.Value : item.Id_Supplier_VersionArticle);
				sqlCommand.Parameters.AddWithValue("Id_User_VersionArticle", item.Id_User_VersionArticle);
				sqlCommand.Parameters.AddWithValue("Land_name_VersionArticle", item.Land_name_VersionArticle == null ? (object)DBNull.Value : item.Land_name_VersionArticle);
				sqlCommand.Parameters.AddWithValue("Quantity_VersionArticle", item.Quantity_VersionArticle == null ? (object)DBNull.Value : item.Quantity_VersionArticle);
				sqlCommand.Parameters.AddWithValue("TotalCost__VersionArticle", item.TotalCost__VersionArticle == null ? (object)DBNull.Value : item.TotalCost__VersionArticle);
				sqlCommand.Parameters.AddWithValue("Unit_Price_VersionArticle", item.Unit_Price_VersionArticle == null ? (object)DBNull.Value : item.Unit_Price_VersionArticle);
				sqlCommand.Parameters.AddWithValue("Version_Article_date", item.Version_Article_date == null ? (object)DBNull.Value : item.Version_Article_date);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.Budget_Article_VersionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 19; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.Budget_Article_VersionEntity> items)
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
						query += " UPDATE [__FNC_Article_Version] SET "

							+ "[Action_Version_Article]=@Action_Version_Article" + i + ","
							+ "[Currency_Version_Article]=@Currency_Version_Article" + i + ","
							+ "[Dept_name_VersionArticle]=@Dept_name_VersionArticle" + i + ","
							+ "[Id_Article]=@Id_Article" + i + ","
							+ "[Id_Currency_Version_Article]=@Id_Currency_Version_Article" + i + ","
							+ "[Id_Dept_VersionArticle]=@Id_Dept_VersionArticle" + i + ","
							+ "[Id_Land_VersionArticle]=@Id_Land_VersionArticle" + i + ","
							+ "[Id_Level_VersionArticle]=@Id_Level_VersionArticle" + i + ","
							+ "[Id_Order_Version]=@Id_Order_Version" + i + ","
							+ "[Id_Project_VersionArticle]=@Id_Project_VersionArticle" + i + ","
							+ "[Id_Status_VersionArticle]=@Id_Status_VersionArticle" + i + ","
							+ "[Id_Supplier_VersionArticle]=@Id_Supplier_VersionArticle" + i + ","
							+ "[Id_User_VersionArticle]=@Id_User_VersionArticle" + i + ","
							+ "[Land_name_VersionArticle]=@Land_name_VersionArticle" + i + ","
							+ "[Quantity_VersionArticle]=@Quantity_VersionArticle" + i + ","
							+ "[TotalCost__VersionArticle]=@TotalCost__VersionArticle" + i + ","
							+ "[Unit_Price_VersionArticle]=@Unit_Price_VersionArticle" + i + ","
							+ "[Version_Article_date]=@Version_Article_date" + i + " WHERE [Id_AOV]=@Id_AOV" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id_AOV" + i, item.Id_AOV);
						sqlCommand.Parameters.AddWithValue("Action_Version_Article" + i, item.Action_Version_Article == null ? (object)DBNull.Value : item.Action_Version_Article);
						sqlCommand.Parameters.AddWithValue("Currency_Version_Article" + i, item.Currency_Version_Article == null ? (object)DBNull.Value : item.Currency_Version_Article);
						sqlCommand.Parameters.AddWithValue("Dept_name_VersionArticle" + i, item.Dept_name_VersionArticle == null ? (object)DBNull.Value : item.Dept_name_VersionArticle);
						sqlCommand.Parameters.AddWithValue("Id_Article" + i, item.Id_Article);
						sqlCommand.Parameters.AddWithValue("Id_Currency_Version_Article" + i, item.Id_Currency_Version_Article == null ? (object)DBNull.Value : item.Id_Currency_Version_Article);
						sqlCommand.Parameters.AddWithValue("Id_Dept_VersionArticle" + i, item.Id_Dept_VersionArticle == null ? (object)DBNull.Value : item.Id_Dept_VersionArticle);
						sqlCommand.Parameters.AddWithValue("Id_Land_VersionArticle" + i, item.Id_Land_VersionArticle == null ? (object)DBNull.Value : item.Id_Land_VersionArticle);
						sqlCommand.Parameters.AddWithValue("Id_Level_VersionArticle" + i, item.Id_Level_VersionArticle == null ? (object)DBNull.Value : item.Id_Level_VersionArticle);
						sqlCommand.Parameters.AddWithValue("Id_Order_Version" + i, item.Id_Order_Version);
						sqlCommand.Parameters.AddWithValue("Id_Project_VersionArticle" + i, item.Id_Project_VersionArticle == null ? (object)DBNull.Value : item.Id_Project_VersionArticle);
						sqlCommand.Parameters.AddWithValue("Id_Status_VersionArticle" + i, item.Id_Status_VersionArticle == null ? (object)DBNull.Value : item.Id_Status_VersionArticle);
						sqlCommand.Parameters.AddWithValue("Id_Supplier_VersionArticle" + i, item.Id_Supplier_VersionArticle == null ? (object)DBNull.Value : item.Id_Supplier_VersionArticle);
						sqlCommand.Parameters.AddWithValue("Id_User_VersionArticle" + i, item.Id_User_VersionArticle);
						sqlCommand.Parameters.AddWithValue("Land_name_VersionArticle" + i, item.Land_name_VersionArticle == null ? (object)DBNull.Value : item.Land_name_VersionArticle);
						sqlCommand.Parameters.AddWithValue("Quantity_VersionArticle" + i, item.Quantity_VersionArticle == null ? (object)DBNull.Value : item.Quantity_VersionArticle);
						sqlCommand.Parameters.AddWithValue("TotalCost__VersionArticle" + i, item.TotalCost__VersionArticle == null ? (object)DBNull.Value : item.TotalCost__VersionArticle);
						sqlCommand.Parameters.AddWithValue("Unit_Price_VersionArticle" + i, item.Unit_Price_VersionArticle == null ? (object)DBNull.Value : item.Unit_Price_VersionArticle);
						sqlCommand.Parameters.AddWithValue("Version_Article_date" + i, item.Version_Article_date == null ? (object)DBNull.Value : item.Version_Article_date);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int id_aov)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__FNC_Article_Version] WHERE [Id_AOV]=@Id_AOV";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id_AOV", id_aov);

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

					string query = "DELETE FROM [__FNC_Article_Version] WHERE [Id_AOV] IN (" + queryIds + ")";
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

		private static List<Infrastructure.Data.Entities.Tables.FNC.Budget_Article_VersionEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.Budget_Article_VersionEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.Budget_Article_VersionEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
