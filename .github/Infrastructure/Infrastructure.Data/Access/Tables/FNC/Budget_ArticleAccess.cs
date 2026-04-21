using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class Budget_ArticleAccess
	{

		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.Budget_ArticleEntity Get(int article_number)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Budget_Article] WHERE [Article_number]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", article_number);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Budget_ArticleEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_ArticleEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Budget_Article]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_ArticleEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_ArticleEntity> Get(List<string> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.Budget_ArticleEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.Budget_ArticleEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_ArticleEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.Budget_ArticleEntity> get(List<string> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [Budget_Article] WHERE [Article_code] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_ArticleEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_ArticleEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.Budget_ArticleEntity item)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Budget_Article] ([Article_code],[Article_designation1],[Article_designation2],[Article_number],[Article_supplier],[Creator_Bind],[Description],[Editor_Bind],[Id_Currency],[Unit_Price])  VALUES (@Article_code,@Article_designation1,@Article_designation2,@Article_number,@Article_supplier,@Creator_Bind,@Description,@Editor_Bind,@Id_Currency,@Unit_Price)";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Article_code", item.Article_code);
					sqlCommand.Parameters.AddWithValue("Article_designation1", item.Article_designation1);
					sqlCommand.Parameters.AddWithValue("Article_designation2", item.Article_designation2);
					sqlCommand.Parameters.AddWithValue("Article_number", item.Article_number);
					sqlCommand.Parameters.AddWithValue("Article_supplier", item.Article_supplier);
					sqlCommand.Parameters.AddWithValue("Creator_Bind", item.Creator_Bind == null ? (object)DBNull.Value : item.Creator_Bind);
					sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
					sqlCommand.Parameters.AddWithValue("Editor_Bind", item.Editor_Bind == null ? (object)DBNull.Value : item.Editor_Bind);
					sqlCommand.Parameters.AddWithValue("Id_Currency", item.Id_Currency);
					sqlCommand.Parameters.AddWithValue("Unit_Price", item.Unit_Price);

					sqlCommand.ExecuteNonQuery();
				}

				using(var sqlCommand = new SqlCommand("SELECT [Article_code] FROM [Budget_Article] WHERE [Article_code] = @@IDENTITY", sqlConnection, sqlTransaction))
				{
					response = int.Parse(sqlCommand.ExecuteScalar()?.ToString() ?? "-1");
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.Budget_ArticleEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.Budget_ArticleEntity> items)
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
						query += " INSERT INTO [Budget_Article] ([Article_code],[Article_designation1],[Article_designation2],[Article_number],[Article_supplier],[Creator_Bind],[Description],[Editor_Bind],[Id_Currency],[Unit_Price]) VALUES ( "

							+ "@Article_code" + i + ","
							+ "@Article_designation1" + i + ","
							+ "@Article_designation2" + i + ","
							+ "@Article_number" + i + ","
							+ "@Article_supplier" + i + ","
							+ "@Creator_Bind" + i + ","
							+ "@Description" + i + ","
							+ "@Editor_Bind" + i + ","
							+ "@Id_Currency" + i + ","
							+ "@Unit_Price" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Article_code" + i, item.Article_code);
						sqlCommand.Parameters.AddWithValue("Article_designation1" + i, item.Article_designation1);
						sqlCommand.Parameters.AddWithValue("Article_designation2" + i, item.Article_designation2);
						sqlCommand.Parameters.AddWithValue("Article_number" + i, item.Article_number);
						sqlCommand.Parameters.AddWithValue("Article_supplier" + i, item.Article_supplier);
						sqlCommand.Parameters.AddWithValue("Creator_Bind" + i, item.Creator_Bind == null ? (object)DBNull.Value : item.Creator_Bind);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("Editor_Bind" + i, item.Editor_Bind == null ? (object)DBNull.Value : item.Editor_Bind);
						sqlCommand.Parameters.AddWithValue("Id_Currency" + i, item.Id_Currency);
						sqlCommand.Parameters.AddWithValue("Unit_Price" + i, item.Unit_Price);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.Budget_ArticleEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [Budget_Article] SET [Article_designation1]=@Article_designation1, [Article_designation2]=@Article_designation2, [Article_code]=@Article_code, [Article_supplier]=@Article_supplier, [Creator_Bind]=@Creator_Bind, [Description]=@Description, [Editor_Bind]=@Editor_Bind, [Id_Currency]=@Id_Currency, [Unit_Price]=@Unit_Price WHERE [Article_number]=@Article_number";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Article_number", item.Article_number);
				sqlCommand.Parameters.AddWithValue("Article_code", item.Article_code);
				sqlCommand.Parameters.AddWithValue("Article_designation1", item.Article_designation1);
				sqlCommand.Parameters.AddWithValue("Article_designation2", item.Article_designation2 == null ? (object)DBNull.Value : item.Article_designation2);
				sqlCommand.Parameters.AddWithValue("Article_supplier", item.Article_supplier);
				sqlCommand.Parameters.AddWithValue("Creator_Bind", item.Creator_Bind == null ? (object)DBNull.Value : item.Creator_Bind);
				sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
				sqlCommand.Parameters.AddWithValue("Editor_Bind", item.Editor_Bind == null ? (object)DBNull.Value : item.Editor_Bind);
				sqlCommand.Parameters.AddWithValue("Id_Currency", item.Id_Currency);
				sqlCommand.Parameters.AddWithValue("Unit_Price", item.Unit_Price);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.Budget_ArticleEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.Budget_ArticleEntity> items)
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
						query += " UPDATE [Budget_Article] SET "

							+ "[Article_designation1]=@Article_designation1" + i + ","
							+ "[Article_designation2]=@Article_designation2" + i + ","
							+ "[Article_number]=@Article_number" + i + ","
							+ "[Article_supplier]=@Article_supplier" + i + ","
							+ "[Creator_Bind]=@Creator_Bind" + i + ","
							+ "[Description]=@Description" + i + ","
							+ "[Editor_Bind]=@Editor_Bind" + i + ","
							+ "[Id_Currency]=@Id_Currency" + i + ","
							+ "[Unit_Price]=@Unit_Price" + i + " WHERE [Article_code]=@Article_code" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Article_code" + i, item.Article_code);
						sqlCommand.Parameters.AddWithValue("Article_designation1" + i, item.Article_designation1);
						sqlCommand.Parameters.AddWithValue("Article_designation2" + i, item.Article_designation2);
						sqlCommand.Parameters.AddWithValue("Article_number" + i, item.Article_number);
						sqlCommand.Parameters.AddWithValue("Article_supplier" + i, item.Article_supplier);
						sqlCommand.Parameters.AddWithValue("Creator_Bind" + i, item.Creator_Bind == null ? (object)DBNull.Value : item.Creator_Bind);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("Editor_Bind" + i, item.Editor_Bind == null ? (object)DBNull.Value : item.Editor_Bind);
						sqlCommand.Parameters.AddWithValue("Id_Currency" + i, item.Id_Currency);
						sqlCommand.Parameters.AddWithValue("Unit_Price" + i, item.Unit_Price);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int article_number)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Budget_Article] WHERE [Article_number]=@Article_number";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Article_number", article_number);

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

					string query = "DELETE FROM [Budget_Article] WHERE [Article_number] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.FNC.AllBudget_ArticleEntity> GetAllDataArticle()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();


				string query = "select art.[Article_number], art.[Article_code], art.[Article_designation1], art.[Article_designation2], art.[Article_supplier],adressen_BDG.[Name1] as Article_supplier_name,  adressen_BDG.[Lieferantennummer], adressen_BDG.[Nr], adressen_BDG.[Ort], art.[Unit_Price], art.[Id_Currency],curr_BDG.[Symol] as Symol,art.[Description], art.[Creator_Bind],user0.[Name] as Article_creator_name, art.[Editor_Bind], user1.[Name] as Article_editor_name from[Budget].[dbo].[Budget_Article] as art " +
				   "Left join [dbo].[__FNC_Adressen] as adressen_BDG on adressen_BDG.[Nr]= art.[Article_supplier]" +
				   "inner join [dbo].[User] as user0 on user0.[Id] =art.[Creator_Bind] " +
				   "Left join [dbo].[User] as user1 on user1.[Id] =art.[Editor_Bind] " +
				   "inner join [dbo].[Currency_Budget] as curr_BDG on curr_BDG.[IdC] = art.[Id_Currency] ";


				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toListAllData(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.AllBudget_ArticleEntity>();
			}
		}

		public static int InsertArticle(Infrastructure.Data.Entities.Tables.FNC.Budget_ArticleEntity item)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Budget_Article] ([Article_code],[Article_designation1],[Article_designation2],[Article_supplier],[Creator_Bind],[Description],[Editor_Bind],[Id_Currency],[Unit_Price])  VALUES (@Article_code,@Article_designation1,@Article_designation2,@Article_supplier,@Creator_Bind,@Description,@Editor_Bind,@Id_Currency,@Unit_Price)";
				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{


					//sqlCommand.Parameters.AddWithValue("Article_number", item.Article_number);
					sqlCommand.Parameters.AddWithValue("Article_code", item.Article_code);
					sqlCommand.Parameters.AddWithValue("Article_designation1", item.Article_designation1);
					sqlCommand.Parameters.AddWithValue("Article_designation2", item.Article_designation2 == null ? (object)DBNull.Value : item.Article_designation2);
					sqlCommand.Parameters.AddWithValue("Article_supplier", item.Article_supplier);
					sqlCommand.Parameters.AddWithValue("Creator_Bind", item.Creator_Bind == null ? (object)DBNull.Value : item.Creator_Bind);
					sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
					sqlCommand.Parameters.AddWithValue("Editor_Bind", item.Editor_Bind == null ? (object)DBNull.Value : item.Editor_Bind);
					sqlCommand.Parameters.AddWithValue("Id_Currency", item.Id_Currency);
					sqlCommand.Parameters.AddWithValue("Unit_Price", item.Unit_Price);


					sqlCommand.ExecuteNonQuery();
				}

				using(var sqlCommand = new SqlCommand("SELECT [Article_number] FROM [Budget_Article] WHERE [Article_number] = @@IDENTITY", sqlConnection, sqlTransaction))

				{
					response = int.Parse(sqlCommand.ExecuteScalar()?.ToString() ?? "-1");
				}

				sqlTransaction.Commit();

				return response;
			}
		}

		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.FNC.Budget_ArticleEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.Budget_ArticleEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.Budget_ArticleEntity(dataRow)); }
			return list;
		}

		private static List<Infrastructure.Data.Entities.Tables.FNC.AllBudget_ArticleEntity> toListAllData(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.AllBudget_ArticleEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.AllBudget_ArticleEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
