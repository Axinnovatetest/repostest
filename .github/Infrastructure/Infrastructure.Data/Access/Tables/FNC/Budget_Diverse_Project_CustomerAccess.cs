using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class Budget_Diverse_Project_CustomerAccess
	{

		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Project_CustomerEntity Get(int id_diverse_customer_project)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Budget_Diverse_Project_Customer] WHERE [Id_Diverse_Customer_Project]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id_diverse_customer_project);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Project_CustomerEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Project_CustomerEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Budget_Diverse_Project_Customer]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Project_CustomerEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Project_CustomerEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Project_CustomerEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Project_CustomerEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Project_CustomerEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Project_CustomerEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [Budget_Diverse_Project_Customer] WHERE [Id_Diverse_Customer_Project] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Project_CustomerEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Project_CustomerEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Project_CustomerEntity item)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Budget_Diverse_Project_Customer] ([Customer_Contact_Description_Project_Diverse],[Customer_Contact_Project_Diverse],[Custommer_Name_Project_Diverse],[Id_Customer_Project_Diverse],[Id_Project_Diverse],[kumdennummer_Project_Diverse],[Nr_Customer_Project_Diverse],[Ort_Project_Diverse])  VALUES (@Customer_Contact_Description_Project_Diverse,@Customer_Contact_Project_Diverse,@Custommer_Name_Project_Diverse,@Id_Customer_Project_Diverse,@Id_Project_Diverse,@kumdennummer_Project_Diverse,@Nr_Customer_Project_Diverse,@Ort_Project_Diverse)";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Customer_Contact_Description_Project_Diverse", item.Customer_Contact_Description_Project_Diverse == null ? (object)DBNull.Value : item.Customer_Contact_Description_Project_Diverse);
					sqlCommand.Parameters.AddWithValue("Customer_Contact_Project_Diverse", item.Customer_Contact_Project_Diverse == null ? (object)DBNull.Value : item.Customer_Contact_Project_Diverse);
					sqlCommand.Parameters.AddWithValue("Custommer_Name_Project_Diverse", item.Custommer_Name_Project_Diverse == null ? (object)DBNull.Value : item.Custommer_Name_Project_Diverse);
					sqlCommand.Parameters.AddWithValue("Id_Customer_Project_Diverse", item.Id_Customer_Project_Diverse == null ? (object)DBNull.Value : item.Id_Customer_Project_Diverse);
					sqlCommand.Parameters.AddWithValue("Id_Project_Diverse", item.Id_Project_Diverse == null ? (object)DBNull.Value : item.Id_Project_Diverse);
					sqlCommand.Parameters.AddWithValue("kumdennummer_Project_Diverse", item.kumdennummer_Project_Diverse == null ? (object)DBNull.Value : item.kumdennummer_Project_Diverse);
					sqlCommand.Parameters.AddWithValue("Nr_Customer_Project_Diverse", item.Nr_Customer_Project_Diverse == null ? (object)DBNull.Value : item.Nr_Customer_Project_Diverse);
					sqlCommand.Parameters.AddWithValue("Ort_Project_Diverse", item.Ort_Project_Diverse == null ? (object)DBNull.Value : item.Ort_Project_Diverse);

					sqlCommand.ExecuteNonQuery();
				}

				using(var sqlCommand = new SqlCommand("SELECT [Id_Diverse_Customer_Project] FROM [Budget_Diverse_Project_Customer] WHERE [Id_Diverse_Customer_Project] = @@IDENTITY", sqlConnection, sqlTransaction))
				{
					response = int.Parse(sqlCommand.ExecuteScalar()?.ToString() ?? "-1");
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Project_CustomerEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Project_CustomerEntity> items)
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
						query += " INSERT INTO [Budget_Diverse_Project_Customer] ([Customer_Contact_Description_Project_Diverse],[Customer_Contact_Project_Diverse],[Custommer_Name_Project_Diverse],[Id_Customer_Project_Diverse],[Id_Project_Diverse],[kumdennummer_Project_Diverse],[Nr_Customer_Project_Diverse],[Ort_Project_Diverse]) VALUES ( "

							+ "@Customer_Contact_Description_Project_Diverse" + i + ","
							+ "@Customer_Contact_Project_Diverse" + i + ","
							+ "@Custommer_Name_Project_Diverse" + i + ","
							+ "@Id_Customer_Project_Diverse" + i + ","
							+ "@Id_Project_Diverse" + i + ","
							+ "@kumdennummer_Project_Diverse" + i + ","
							+ "@Nr_Customer_Project_Diverse" + i + ","
							+ "@Ort_Project_Diverse" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Customer_Contact_Description_Project_Diverse" + i, item.Customer_Contact_Description_Project_Diverse == null ? (object)DBNull.Value : item.Customer_Contact_Description_Project_Diverse);
						sqlCommand.Parameters.AddWithValue("Customer_Contact_Project_Diverse" + i, item.Customer_Contact_Project_Diverse == null ? (object)DBNull.Value : item.Customer_Contact_Project_Diverse);
						sqlCommand.Parameters.AddWithValue("Custommer_Name_Project_Diverse" + i, item.Custommer_Name_Project_Diverse == null ? (object)DBNull.Value : item.Custommer_Name_Project_Diverse);
						sqlCommand.Parameters.AddWithValue("Id_Customer_Project_Diverse" + i, item.Id_Customer_Project_Diverse == null ? (object)DBNull.Value : item.Id_Customer_Project_Diverse);
						sqlCommand.Parameters.AddWithValue("Id_Project_Diverse" + i, item.Id_Project_Diverse == null ? (object)DBNull.Value : item.Id_Project_Diverse);
						sqlCommand.Parameters.AddWithValue("kumdennummer_Project_Diverse" + i, item.kumdennummer_Project_Diverse == null ? (object)DBNull.Value : item.kumdennummer_Project_Diverse);
						sqlCommand.Parameters.AddWithValue("Nr_Customer_Project_Diverse" + i, item.Nr_Customer_Project_Diverse == null ? (object)DBNull.Value : item.Nr_Customer_Project_Diverse);
						sqlCommand.Parameters.AddWithValue("Ort_Project_Diverse" + i, item.Ort_Project_Diverse == null ? (object)DBNull.Value : item.Ort_Project_Diverse);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Project_CustomerEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [Budget_Diverse_Project_Customer] SET [Customer_Contact_Description_Project_Diverse]=@Customer_Contact_Description_Project_Diverse, [Customer_Contact_Project_Diverse]=@Customer_Contact_Project_Diverse, [Custommer_Name_Project_Diverse]=@Custommer_Name_Project_Diverse, [Id_Customer_Project_Diverse]=@Id_Customer_Project_Diverse, [Id_Project_Diverse]=@Id_Project_Diverse, [kumdennummer_Project_Diverse]=@kumdennummer_Project_Diverse, [Nr_Customer_Project_Diverse]=@Nr_Customer_Project_Diverse, [Ort_Project_Diverse]=@Ort_Project_Diverse WHERE [Id_Diverse_Customer_Project]=@Id_Diverse_Customer_Project";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id_Diverse_Customer_Project", item.Id_Diverse_Customer_Project);
				sqlCommand.Parameters.AddWithValue("Customer_Contact_Description_Project_Diverse", item.Customer_Contact_Description_Project_Diverse == null ? (object)DBNull.Value : item.Customer_Contact_Description_Project_Diverse);
				sqlCommand.Parameters.AddWithValue("Customer_Contact_Project_Diverse", item.Customer_Contact_Project_Diverse == null ? (object)DBNull.Value : item.Customer_Contact_Project_Diverse);
				sqlCommand.Parameters.AddWithValue("Custommer_Name_Project_Diverse", item.Custommer_Name_Project_Diverse == null ? (object)DBNull.Value : item.Custommer_Name_Project_Diverse);
				sqlCommand.Parameters.AddWithValue("Id_Customer_Project_Diverse", item.Id_Customer_Project_Diverse == null ? (object)DBNull.Value : item.Id_Customer_Project_Diverse);
				sqlCommand.Parameters.AddWithValue("Id_Project_Diverse", item.Id_Project_Diverse == null ? (object)DBNull.Value : item.Id_Project_Diverse);
				sqlCommand.Parameters.AddWithValue("kumdennummer_Project_Diverse", item.kumdennummer_Project_Diverse == null ? (object)DBNull.Value : item.kumdennummer_Project_Diverse);
				sqlCommand.Parameters.AddWithValue("Nr_Customer_Project_Diverse", item.Nr_Customer_Project_Diverse == null ? (object)DBNull.Value : item.Nr_Customer_Project_Diverse);
				sqlCommand.Parameters.AddWithValue("Ort_Project_Diverse", item.Ort_Project_Diverse == null ? (object)DBNull.Value : item.Ort_Project_Diverse);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Project_CustomerEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Project_CustomerEntity> items)
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
						query += " UPDATE [Budget_Diverse_Project_Customer] SET "

							+ "[Customer_Contact_Description_Project_Diverse]=@Customer_Contact_Description_Project_Diverse" + i + ","
							+ "[Customer_Contact_Project_Diverse]=@Customer_Contact_Project_Diverse" + i + ","
							+ "[Custommer_Name_Project_Diverse]=@Custommer_Name_Project_Diverse" + i + ","
							+ "[Id_Customer_Project_Diverse]=@Id_Customer_Project_Diverse" + i + ","
							+ "[Id_Project_Diverse]=@Id_Project_Diverse" + i + ","
							+ "[kumdennummer_Project_Diverse]=@kumdennummer_Project_Diverse" + i + ","
							+ "[Nr_Customer_Project_Diverse]=@Nr_Customer_Project_Diverse" + i + ","
							+ "[Ort_Project_Diverse]=@Ort_Project_Diverse" + i + " WHERE [Id_Diverse_Customer_Project]=@Id_Diverse_Customer_Project" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id_Diverse_Customer_Project" + i, item.Id_Diverse_Customer_Project);
						sqlCommand.Parameters.AddWithValue("Customer_Contact_Description_Project_Diverse" + i, item.Customer_Contact_Description_Project_Diverse == null ? (object)DBNull.Value : item.Customer_Contact_Description_Project_Diverse);
						sqlCommand.Parameters.AddWithValue("Customer_Contact_Project_Diverse" + i, item.Customer_Contact_Project_Diverse == null ? (object)DBNull.Value : item.Customer_Contact_Project_Diverse);
						sqlCommand.Parameters.AddWithValue("Custommer_Name_Project_Diverse" + i, item.Custommer_Name_Project_Diverse == null ? (object)DBNull.Value : item.Custommer_Name_Project_Diverse);
						sqlCommand.Parameters.AddWithValue("Id_Customer_Project_Diverse" + i, item.Id_Customer_Project_Diverse == null ? (object)DBNull.Value : item.Id_Customer_Project_Diverse);
						sqlCommand.Parameters.AddWithValue("Id_Project_Diverse" + i, item.Id_Project_Diverse == null ? (object)DBNull.Value : item.Id_Project_Diverse);
						sqlCommand.Parameters.AddWithValue("kumdennummer_Project_Diverse" + i, item.kumdennummer_Project_Diverse == null ? (object)DBNull.Value : item.kumdennummer_Project_Diverse);
						sqlCommand.Parameters.AddWithValue("Nr_Customer_Project_Diverse" + i, item.Nr_Customer_Project_Diverse == null ? (object)DBNull.Value : item.Nr_Customer_Project_Diverse);
						sqlCommand.Parameters.AddWithValue("Ort_Project_Diverse" + i, item.Ort_Project_Diverse == null ? (object)DBNull.Value : item.Ort_Project_Diverse);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int id_diverse_customer_project)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Budget_Diverse_Project_Customer] WHERE [Id_Diverse_Customer_Project]=@Id_Diverse_Customer_Project";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id_Diverse_Customer_Project", id_diverse_customer_project);

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

					string query = "DELETE FROM [Budget_Diverse_Project_Customer] WHERE [Id_Diverse_Customer_Project] IN (" + queryIds + ")";
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

		private static List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Project_CustomerEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Project_CustomerEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Project_CustomerEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
