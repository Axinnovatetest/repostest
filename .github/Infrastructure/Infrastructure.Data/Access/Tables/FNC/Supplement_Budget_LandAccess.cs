using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class Supplement_Budget_LandAccess
	{

		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.Supplement_Budget_LandEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Supplement_Budget_Land] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Supplement_Budget_LandEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.Supplement_Budget_LandEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Supplement_Budget_Land]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Supplement_Budget_LandEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Supplement_Budget_LandEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.Supplement_Budget_LandEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.Supplement_Budget_LandEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Supplement_Budget_LandEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.Supplement_Budget_LandEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [Supplement_Budget_Land] WHERE [Id] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.Supplement_Budget_LandEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Supplement_Budget_LandEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.Supplement_Budget_LandEntity item)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Supplement_Budget_Land] ([Id_AL],[Supplement_Budget],[Creation_Date])  VALUES (@Id_AL,@Supplement_Budget,@Creation_Date)";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Id_AL", item.Id_AL);
					sqlCommand.Parameters.AddWithValue("Supplement_Budget", item.Supplement_Budget == null ? (object)DBNull.Value : item.Supplement_Budget);
					sqlCommand.Parameters.AddWithValue("Creation_Date", item.Creation_Date);

					sqlCommand.ExecuteNonQuery();
				}

				using(var sqlCommand = new SqlCommand("SELECT [Id] FROM [Supplement_Budget_Land] WHERE [Id] = @@IDENTITY", sqlConnection, sqlTransaction))
				{
					response = int.Parse(sqlCommand.ExecuteScalar()?.ToString() ?? "-1");
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.Supplement_Budget_LandEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.Supplement_Budget_LandEntity> items)
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
						query += " INSERT INTO [Supplement_Budget_Land] ([Id_AL],[Supplement_Budget],[Creation_Date]) VALUES ( "

							+ "@Id_AL" + i + ","
							+ "@Supplement_Budget" + i + ","
							+ "@Creation_Date" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Id_AL" + i, item.Id_AL);
						sqlCommand.Parameters.AddWithValue("Supplement_Budget" + i, item.Supplement_Budget == null ? (object)DBNull.Value : item.Supplement_Budget);
						sqlCommand.Parameters.AddWithValue("Creation_Date" + i, item.Creation_Date);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.Supplement_Budget_LandEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [Supplement_Budget_Land] SET [Id_AL]=@Id_AL, [Supplement_Budget]=@Supplement_Budget, [Creation_Date]=@Creation_Date WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Id_AL", item.Id_AL);
				sqlCommand.Parameters.AddWithValue("Supplement_Budget", item.Supplement_Budget == null ? (object)DBNull.Value : item.Supplement_Budget);
				sqlCommand.Parameters.AddWithValue("Creation_Date", item.Creation_Date);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.Supplement_Budget_LandEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.Supplement_Budget_LandEntity> items)
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
						query += " UPDATE [Supplement_Budget_Land] SET "

							+ "[Id_AL]=@Id_AL" + i + ","
							+ "[Supplement_Budget]=@Supplement_Budget" + i + ","
							+ "[Creation_Date]=@Creation_Date" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("Id_AL" + i, item.Id_AL);
						sqlCommand.Parameters.AddWithValue("Supplement_Budget" + i, item.Supplement_Budget == null ? (object)DBNull.Value : item.Supplement_Budget);
						sqlCommand.Parameters.AddWithValue("Creation_Date" + i, item.Creation_Date);
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
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Supplement_Budget_Land] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [Supplement_Budget_Land] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.FNC.Supplement_Budget_LandEntity> GetSupplementLandBdg(int id)
		{
			var dataTable = new DataTable();
			//var dataTable_2 = new DataTable();

			//var response = new Infrastructure.Data.Entities.Tables.FNC.SupplementLandBdgEntity();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				//string query = "select AL.Land_Name, AL.budget, isnull(SL.Supplement_Budget, 0) as SupplementLandBudget,AL.B_year, SL.Creation_Date from[Supplement_Budget_Land] SL inner Join[assign_budget_land] AL ON SL.Id_AL = AL.ID where SL.Id_AL=@ID";
				/*select AL.Land_Name, AL.budget,isnull(SUM(SL.Supplement_Budget),0) as SumSupplementLandBudget,AL.B_year
                from [Supplement_Budget_Land] SL,[assign_budget_land] AL where SL.Id_AL = AL.ID 
                Group by AL.Land_Name, AL.budget,AL.B_year*/

				//string query = "select AL.Land_Name, AL.budget, isnull(SL.Supplement_Budget, 0) as SupplementLandBudget,AL.B_year, SL.Creation_Date from[Supplement_Budget_Land] SL,[assign_budget_land] AL where SL.Id_AL=AL.ID and SL.Id_AL=@ID";
				string query = "SELECT * FROM [Supplement_Budget_Land] where Id_AL in (select ID from[assign_budget_land] where ID=@ID)";
				//string query_2 = "select isnull(SUM(SL.Supplement_Budget),0) as SOMME_Supplement_Land from [Supplement_Budget_Land] SL where SL.Id_AL=@ID";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ID", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				//******
				// var sqlCommand_2 = new SqlCommand(query_2, sqlConnection);
				// sqlCommand_2.Parameters.AddWithValue("ID", id);

				//  new SqlDataAdapter(sqlCommand_2).Fill(dataTable_2);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
				/* foreach(DataRow Supp in dataTable.Rows)
                 { 
                 response.LandBudget = float.Parse(Supp["LandBudget"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                 response.Supplement_Land = float.Parse(Supp["Supplement_Land"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                 response.Land = Supp["Land"].ToString();
                 response.Year = int.Parse(Supp["Year"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                 response.Creation_Date = DateTime.Parse(Supp["Creation_Date"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
             }*/
				//response.SOMME_Supplement_Land = float.Parse(dataTable_2.Rows[0]["SOMME_Supplement_Land"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
			}
			else
			{ return null; }
			//return response;


		}

		public static Infrastructure.Data.Entities.Tables.FNC.SupplementLandBdgEntity SommeSupplementLandBdg(int ID)
		{
			var dataTable = new DataTable();
			var dataTable_2 = new DataTable();

			var response = new Infrastructure.Data.Entities.Tables.FNC.SupplementLandBdgEntity();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				string query = "select ID,Land_Name, budget, B_year from [assign_budget_land] AL where AL.ID=@ID";
				string query_2 = "select isnull(SUM(Supplement_Budget),0) as SOMME_Supplement_Land_Budget from [Supplement_Budget_Land] SL where id_AL in (select AL.ID from [assign_budget_land] AL where ID=@ID)";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ID", ID);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				//******
				var sqlCommand_2 = new SqlCommand(query_2, sqlConnection);
				sqlCommand_2.Parameters.AddWithValue("ID", ID);

				new SqlDataAdapter(sqlCommand_2).Fill(dataTable_2);
			}

			if(dataTable.Rows.Count > 0)
			{
				response.ID = int.Parse(dataTable.Rows[0]["ID"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
				response.Land_Name = dataTable.Rows[0]["Land_Name"].ToString();
				response.budget = float.Parse(dataTable.Rows[0]["budget"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
				response.SOMME_Supplement_Land_Budget = float.Parse(dataTable_2.Rows[0]["SOMME_Supplement_Land_Budget"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
				response.B_year = int.Parse(dataTable.Rows[0]["B_year"].ToString(), System.Globalization.CultureInfo.InvariantCulture);

			}

			return response;


		}

		public static Infrastructure.Data.Entities.Tables.FNC.Supplement_Budget_LandEntity GetSupplementLandBdgID(int Id)
		{
			var dataTable = new DataTable();


			var response = new Infrastructure.Data.Entities.Tables.FNC.Supplement_Budget_LandEntity();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				string query = "select * from [Supplement_Budget_Land] where Id=@Id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", Id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				response.Id = int.Parse(dataTable.Rows[0]["Id"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
				response.Id_AL = int.Parse(dataTable.Rows[0]["Id_AL"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
				response.Supplement_Budget = float.Parse(dataTable.Rows[0]["Supplement_Budget"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
				response.Creation_Date = DateTime.Parse(dataTable.Rows[0]["Creation_Date"].ToString().Replace("T", " "));

			}

			return response;


		}

		public static int InsertSupplement(Infrastructure.Data.Entities.Tables.FNC.Supplement_Budget_LandEntity item)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Supplement_Budget_Land] ([Id_AL],[Supplement_Budget],[Creation_Date])  VALUES (@Id_AL,@Supplement_Budget,@Creation_Date)";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Id_AL", item.Id_AL);
					sqlCommand.Parameters.AddWithValue("Supplement_Budget", item.Supplement_Budget == null ? (object)DBNull.Value : item.Supplement_Budget);
					sqlCommand.Parameters.AddWithValue("Creation_Date", item.Creation_Date);

					sqlCommand.ExecuteNonQuery();
				}

				using(var sqlCommand = new SqlCommand("SELECT [Id] FROM [Supplement_Budget_Land] WHERE [Id] = @@IDENTITY", sqlConnection, sqlTransaction))
				{
					response = int.Parse(sqlCommand.ExecuteScalar()?.ToString() ?? "-1");
				}

				sqlTransaction.Commit();

				return response;
			}
		}

		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.FNC.Supplement_Budget_LandEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.Supplement_Budget_LandEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.Supplement_Budget_LandEntity(dataRow)); }
			return list;
		}

		private static List<Infrastructure.Data.Entities.Tables.FNC.SupplementLandBdgEntity> toListBDG(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.SupplementLandBdgEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.SupplementLandBdgEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
