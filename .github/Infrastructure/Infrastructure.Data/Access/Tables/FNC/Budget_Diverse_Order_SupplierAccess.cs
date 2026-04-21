using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class Budget_Diverse_Order_SupplierAccess
	{

		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Order_SupplierEntity Get(int id_diverse_supplier_order)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Budget_Diverse_Order_Supplier] WHERE [Id_Diverse_Supplier_Order]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id_diverse_supplier_order);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Order_SupplierEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Order_SupplierEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Budget_Diverse_Order_Supplier]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Order_SupplierEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Order_SupplierEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Order_SupplierEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Order_SupplierEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Order_SupplierEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Order_SupplierEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [Budget_Diverse_Order_Supplier] WHERE [Id_Diverse_Supplier_Order] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Order_SupplierEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Order_SupplierEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Order_SupplierEntity item)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Budget_Diverse_Order_Supplier] ([Id_Order_Diverse],[Id_Supplier_Order_Diverse],[Lieferantennummer_Order_Diverse],[Ort_Order_Supplier_Diverse],[Supplier_Contact_Description_Order_Diverse],[Supplier_Contact_Order_Diverse],[Supplier_Name_Order_Diverse])  VALUES (@Id_Order_Diverse,@Id_Supplier_Order_Diverse,@Lieferantennummer_Order_Diverse,@Ort_Order_Supplier_Diverse,@Supplier_Contact_Description_Order_Diverse,@Supplier_Contact_Order_Diverse,@Supplier_Name_Order_Diverse)";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Id_Order_Diverse", item.Id_Order_Diverse == null ? (object)DBNull.Value : item.Id_Order_Diverse);
					sqlCommand.Parameters.AddWithValue("Id_Supplier_Order_Diverse", item.Id_Supplier_Order_Diverse == null ? (object)DBNull.Value : item.Id_Supplier_Order_Diverse);
					sqlCommand.Parameters.AddWithValue("Lieferantennummer_Order_Diverse", item.Lieferantennummer_Order_Diverse == null ? (object)DBNull.Value : item.Lieferantennummer_Order_Diverse);
					sqlCommand.Parameters.AddWithValue("Ort_Order_Supplier_Diverse", item.Ort_Order_Supplier_Diverse == null ? (object)DBNull.Value : item.Ort_Order_Supplier_Diverse);
					sqlCommand.Parameters.AddWithValue("Supplier_Contact_Description_Order_Diverse", item.Supplier_Contact_Description_Order_Diverse == null ? (object)DBNull.Value : item.Supplier_Contact_Description_Order_Diverse);
					sqlCommand.Parameters.AddWithValue("Supplier_Contact_Order_Diverse", item.Supplier_Contact_Order_Diverse == null ? (object)DBNull.Value : item.Supplier_Contact_Order_Diverse);
					sqlCommand.Parameters.AddWithValue("Supplier_Name_Order_Diverse", item.Supplier_Name_Order_Diverse == null ? (object)DBNull.Value : item.Supplier_Name_Order_Diverse);

					sqlCommand.ExecuteNonQuery();
				}

				using(var sqlCommand = new SqlCommand("SELECT [Id_Diverse_Supplier_Order] FROM [Budget_Diverse_Order_Supplier] WHERE [Id_Diverse_Supplier_Order] = @@IDENTITY", sqlConnection, sqlTransaction))
				{
					response = int.Parse(sqlCommand.ExecuteScalar()?.ToString() ?? "-1");
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Order_SupplierEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 8; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Order_SupplierEntity> items)
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
						query += " INSERT INTO [Budget_Diverse_Order_Supplier] ([Id_Order_Diverse],[Id_Supplier_Order_Diverse],[Lieferantennummer_Order_Diverse],[Ort_Order_Supplier_Diverse],[Supplier_Contact_Description_Order_Diverse],[Supplier_Contact_Order_Diverse],[Supplier_Name_Order_Diverse]) VALUES ( "

							+ "@Id_Order_Diverse" + i + ","
							+ "@Id_Supplier_Order_Diverse" + i + ","
							+ "@Lieferantennummer_Order_Diverse" + i + ","
							+ "@Ort_Order_Supplier_Diverse" + i + ","
							+ "@Supplier_Contact_Description_Order_Diverse" + i + ","
							+ "@Supplier_Contact_Order_Diverse" + i + ","
							+ "@Supplier_Name_Order_Diverse" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Id_Order_Diverse" + i, item.Id_Order_Diverse == null ? (object)DBNull.Value : item.Id_Order_Diverse);
						sqlCommand.Parameters.AddWithValue("Id_Supplier_Order_Diverse" + i, item.Id_Supplier_Order_Diverse == null ? (object)DBNull.Value : item.Id_Supplier_Order_Diverse);
						sqlCommand.Parameters.AddWithValue("Lieferantennummer_Order_Diverse" + i, item.Lieferantennummer_Order_Diverse == null ? (object)DBNull.Value : item.Lieferantennummer_Order_Diverse);
						sqlCommand.Parameters.AddWithValue("Ort_Order_Supplier_Diverse" + i, item.Ort_Order_Supplier_Diverse == null ? (object)DBNull.Value : item.Ort_Order_Supplier_Diverse);
						sqlCommand.Parameters.AddWithValue("Supplier_Contact_Description_Order_Diverse" + i, item.Supplier_Contact_Description_Order_Diverse == null ? (object)DBNull.Value : item.Supplier_Contact_Description_Order_Diverse);
						sqlCommand.Parameters.AddWithValue("Supplier_Contact_Order_Diverse" + i, item.Supplier_Contact_Order_Diverse == null ? (object)DBNull.Value : item.Supplier_Contact_Order_Diverse);
						sqlCommand.Parameters.AddWithValue("Supplier_Name_Order_Diverse" + i, item.Supplier_Name_Order_Diverse == null ? (object)DBNull.Value : item.Supplier_Name_Order_Diverse);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Order_SupplierEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [Budget_Diverse_Order_Supplier] SET [Id_Order_Diverse]=@Id_Order_Diverse, [Id_Supplier_Order_Diverse]=@Id_Supplier_Order_Diverse, [Lieferantennummer_Order_Diverse]=@Lieferantennummer_Order_Diverse, [Ort_Order_Supplier_Diverse]=@Ort_Order_Supplier_Diverse, [Supplier_Contact_Description_Order_Diverse]=@Supplier_Contact_Description_Order_Diverse, [Supplier_Contact_Order_Diverse]=@Supplier_Contact_Order_Diverse, [Supplier_Name_Order_Diverse]=@Supplier_Name_Order_Diverse WHERE [Id_Diverse_Supplier_Order]=@Id_Diverse_Supplier_Order";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id_Diverse_Supplier_Order", item.Id_Diverse_Supplier_Order);
				sqlCommand.Parameters.AddWithValue("Id_Order_Diverse", item.Id_Order_Diverse == null ? (object)DBNull.Value : item.Id_Order_Diverse);
				sqlCommand.Parameters.AddWithValue("Id_Supplier_Order_Diverse", item.Id_Supplier_Order_Diverse == null ? (object)DBNull.Value : item.Id_Supplier_Order_Diverse);
				sqlCommand.Parameters.AddWithValue("Lieferantennummer_Order_Diverse", item.Lieferantennummer_Order_Diverse == null ? (object)DBNull.Value : item.Lieferantennummer_Order_Diverse);
				sqlCommand.Parameters.AddWithValue("Ort_Order_Supplier_Diverse", item.Ort_Order_Supplier_Diverse == null ? (object)DBNull.Value : item.Ort_Order_Supplier_Diverse);
				sqlCommand.Parameters.AddWithValue("Supplier_Contact_Description_Order_Diverse", item.Supplier_Contact_Description_Order_Diverse == null ? (object)DBNull.Value : item.Supplier_Contact_Description_Order_Diverse);
				sqlCommand.Parameters.AddWithValue("Supplier_Contact_Order_Diverse", item.Supplier_Contact_Order_Diverse == null ? (object)DBNull.Value : item.Supplier_Contact_Order_Diverse);
				sqlCommand.Parameters.AddWithValue("Supplier_Name_Order_Diverse", item.Supplier_Name_Order_Diverse == null ? (object)DBNull.Value : item.Supplier_Name_Order_Diverse);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Order_SupplierEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 8; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Order_SupplierEntity> items)
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
						query += " UPDATE [Budget_Diverse_Order_Supplier] SET "

							+ "[Id_Order_Diverse]=@Id_Order_Diverse" + i + ","
							+ "[Id_Supplier_Order_Diverse]=@Id_Supplier_Order_Diverse" + i + ","
							+ "[Lieferantennummer_Order_Diverse]=@Lieferantennummer_Order_Diverse" + i + ","
							+ "[Ort_Order_Supplier_Diverse]=@Ort_Order_Supplier_Diverse" + i + ","
							+ "[Supplier_Contact_Description_Order_Diverse]=@Supplier_Contact_Description_Order_Diverse" + i + ","
							+ "[Supplier_Contact_Order_Diverse]=@Supplier_Contact_Order_Diverse" + i + ","
							+ "[Supplier_Name_Order_Diverse]=@Supplier_Name_Order_Diverse" + i + " WHERE [Id_Diverse_Supplier_Order]=@Id_Diverse_Supplier_Order" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id_Diverse_Supplier_Order" + i, item.Id_Diverse_Supplier_Order);
						sqlCommand.Parameters.AddWithValue("Id_Order_Diverse" + i, item.Id_Order_Diverse == null ? (object)DBNull.Value : item.Id_Order_Diverse);
						sqlCommand.Parameters.AddWithValue("Id_Supplier_Order_Diverse" + i, item.Id_Supplier_Order_Diverse == null ? (object)DBNull.Value : item.Id_Supplier_Order_Diverse);
						sqlCommand.Parameters.AddWithValue("Lieferantennummer_Order_Diverse" + i, item.Lieferantennummer_Order_Diverse == null ? (object)DBNull.Value : item.Lieferantennummer_Order_Diverse);
						sqlCommand.Parameters.AddWithValue("Ort_Order_Supplier_Diverse" + i, item.Ort_Order_Supplier_Diverse == null ? (object)DBNull.Value : item.Ort_Order_Supplier_Diverse);
						sqlCommand.Parameters.AddWithValue("Supplier_Contact_Description_Order_Diverse" + i, item.Supplier_Contact_Description_Order_Diverse == null ? (object)DBNull.Value : item.Supplier_Contact_Description_Order_Diverse);
						sqlCommand.Parameters.AddWithValue("Supplier_Contact_Order_Diverse" + i, item.Supplier_Contact_Order_Diverse == null ? (object)DBNull.Value : item.Supplier_Contact_Order_Diverse);
						sqlCommand.Parameters.AddWithValue("Supplier_Name_Order_Diverse" + i, item.Supplier_Name_Order_Diverse == null ? (object)DBNull.Value : item.Supplier_Name_Order_Diverse);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int id_diverse_supplier_order)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Budget_Diverse_Order_Supplier] WHERE [Id_Diverse_Supplier_Order]=@Id_Diverse_Supplier_Order";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id_Diverse_Supplier_Order", id_diverse_supplier_order);

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

					string query = "DELETE FROM [Budget_Diverse_Order_Supplier] WHERE [Id_Diverse_Supplier_Order] IN (" + queryIds + ")";
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

		private static List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Order_SupplierEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Order_SupplierEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Order_SupplierEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
