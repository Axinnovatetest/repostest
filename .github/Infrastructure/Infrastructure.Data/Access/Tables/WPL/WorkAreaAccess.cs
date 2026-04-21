using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.WPL
{

	public class WorkAreaAccess
	{
		public static Boolean GetArchived { get; private set; } = false;
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Work_Area] WHERE [Id]=@Id AND Is_Archived=@IsArchived";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);
				sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Work_Area] WHERE Is_Archived=@IsArchived";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
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

					sqlCommand.CommandText = "SELECT * FROM [Work_Area] WHERE [Id] IN (" + queryIds + ") AND Is_Archived=@IsArchived";
					sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);

					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Work_Area] ([Country_Id],[Creation_Date],[Creation_User_Id],[Delete_Date],[Delete_User_Id],[Department_Id],[Hall_Id],[Is_Archived],[Last_Edit_Date],[Last_Edit_User_Id],[Name])  VALUES (@Country_Id,@Creation_Date,@Creation_User_Id,@Delete_Date,@Delete_User_Id,@Department_Id,@Hall_Id,@Is_Archived,@Last_Edit_Date,@Last_Edit_User_Id,@Name);";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Country_Id", item.CountryId);
					sqlCommand.Parameters.AddWithValue("Creation_Date", item.CreationTime);
					sqlCommand.Parameters.AddWithValue("Creation_User_Id", item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("Delete_Date", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
					sqlCommand.Parameters.AddWithValue("Delete_User_Id", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
					sqlCommand.Parameters.AddWithValue("Department_Id", item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
					sqlCommand.Parameters.AddWithValue("Hall_Id", item.HallId);
					sqlCommand.Parameters.AddWithValue("Is_Archived", item.IsArchived);
					sqlCommand.Parameters.AddWithValue("Last_Edit_Date", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
					sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("Name", item.Name);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 12; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [Work_Area] ([Country_Id],[Creation_Date],[Creation_User_Id],[Delete_Date],[Delete_User_Id],[Department_Id],[Hall_Id],[Is_Archived],[Last_Edit_Date],[Last_Edit_User_Id],[Name]) VALUES ( "

							+ "@Country_Id" + i + ","
							+ "@Creation_Date" + i + ","
							+ "@Creation_User_Id" + i + ","
							+ "@Delete_Date" + i + ","
							+ "@Delete_User_Id" + i + ","
							+ "@Department_Id" + i + ","
							+ "@Hall_Id" + i + ","
							+ "@Is_Archived" + i + ","
							+ "@Last_Edit_Date" + i + ","
							+ "@Last_Edit_User_Id" + i + ","
							+ "@Name" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Country_Id" + i, item.CountryId);
						sqlCommand.Parameters.AddWithValue("Creation_Date" + i, item.CreationTime);
						sqlCommand.Parameters.AddWithValue("Creation_User_Id" + i, item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("Delete_Date" + i, item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
						sqlCommand.Parameters.AddWithValue("Delete_User_Id" + i, item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
						sqlCommand.Parameters.AddWithValue("Department_Id" + i, item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
						sqlCommand.Parameters.AddWithValue("Hall_Id" + i, item.HallId);
						sqlCommand.Parameters.AddWithValue("Is_Archived" + i, item.IsArchived);
						sqlCommand.Parameters.AddWithValue("Last_Edit_Date" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
						sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Work_Area] SET [Country_Id]=@Country_Id, [Creation_Date]=@Creation_Date, [Creation_User_Id]=@Creation_User_Id, [Delete_Date]=@Delete_Date, [Delete_User_Id]=@Delete_User_Id, [Department_Id]=@Department_Id, [Hall_Id]=@Hall_Id, [Is_Archived]=@Is_Archived, [Last_Edit_Date]=@Last_Edit_Date, [Last_Edit_User_Id]=@Last_Edit_User_Id, [Name]=@Name WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Country_Id", item.CountryId);
				sqlCommand.Parameters.AddWithValue("Creation_Date", item.CreationTime);
				sqlCommand.Parameters.AddWithValue("Creation_User_Id", item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("Delete_Date", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
				sqlCommand.Parameters.AddWithValue("Delete_User_Id", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
				sqlCommand.Parameters.AddWithValue("Department_Id", item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
				sqlCommand.Parameters.AddWithValue("Hall_Id", item.HallId);
				sqlCommand.Parameters.AddWithValue("Is_Archived", item.IsArchived);
				sqlCommand.Parameters.AddWithValue("Last_Edit_Date", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
				sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
				sqlCommand.Parameters.AddWithValue("Name", item.Name);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 12; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [Work_Area] SET "

							+ "[Country_Id]=@Country_Id" + i + ","
							+ "[Creation_Date]=@Creation_Date" + i + ","
							+ "[Creation_User_Id]=@Creation_User_Id" + i + ","
							+ "[Delete_Date]=@Delete_Date" + i + ","
							+ "[Delete_User_Id]=@Delete_User_Id" + i + ","
							+ "[Department_Id]=@Department_Id" + i + ","
							+ "[Hall_Id]=@Hall_Id" + i + ","
							+ "[Is_Archived]=@Is_Archived" + i + ","
							+ "[Last_Edit_Date]=@Last_Edit_Date" + i + ","
							+ "[Last_Edit_User_Id]=@Last_Edit_User_Id" + i + ","
							+ "[Name]=@Name" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("Country_Id" + i, item.CountryId);
						sqlCommand.Parameters.AddWithValue("Creation_Date" + i, item.CreationTime);
						sqlCommand.Parameters.AddWithValue("Creation_User_Id" + i, item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("Delete_Date" + i, item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
						sqlCommand.Parameters.AddWithValue("Delete_User_Id" + i, item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
						sqlCommand.Parameters.AddWithValue("Department_Id" + i, item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
						sqlCommand.Parameters.AddWithValue("Hall_Id" + i, item.HallId);
						sqlCommand.Parameters.AddWithValue("Is_Archived" + i, item.IsArchived);
						sqlCommand.Parameters.AddWithValue("Last_Edit_Date" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
						sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name);
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
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Work_Area] WHERE [Id]=@Id";
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
				using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
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

					string query = "DELETE FROM [Work_Area] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		public static Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity GetByName(string name)
		{
			if(string.IsNullOrEmpty(name))
			{
				return null;
			}

			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM Work_Area WHERE Name=@Name AND Is_Archived=@IsArchived";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Name", name);
			sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity> GetByHallId(int hallId)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM Work_Area WHERE Hall_Id=@HallId AND Is_Archived=@IsArchived";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("HallId", hallId);
			sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.WPL.WorkAreaEntity>();
			}
		}
		public static int CountByHallId(int hallId)
		{
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) FROM Work_Area WHERE Hall_Id=@HallId AND Is_Archived=@IsArchived";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("HallId", hallId);
				sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);

				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var x) ? x : 0;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity> GetByHallIdAndName(int hallId, string name)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM Work_Area WHERE Hall_Id=@HallId AND TRIM([Name])=TRIM(@name) AND Is_Archived=@IsArchived AND Delete_Date IS NULL";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("HallId", hallId);
			sqlCommand.Parameters.AddWithValue("name", name);
			sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.WPL.WorkAreaEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity> GetByCountryId(int CountryId)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM Work_Area WHERE Country_Id=@CountryId AND Is_Archived=@IsArchived";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("CountryId", CountryId);
			sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.WPL.WorkAreaEntity>();
			}
		}
		public static int CountByCountryId(int CountryId)
		{
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) FROM Work_Area WHERE Country_Id=@CountryId AND Is_Archived=@IsArchived";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("CountryId", CountryId);
				sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);

				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var x) ? x : 0;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity> GetByDepartmentId(int departmenrtId)
		{
			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = "SELECT * FROM Work_Area WHERE [Department_Id]=@departmentId AND Is_Archived=@IsArchived";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("departmentId", departmenrtId);
			sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.WPL.WorkAreaEntity>();
			}
		}
		public static int CountByDepartmentId(int departmenrtId)
		{
			using(var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) FROM Work_Area WHERE [Department_Id]=@departmentId AND Is_Archived=@IsArchived";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("departmentId", departmenrtId);
				sqlCommand.Parameters.AddWithValue("IsArchived", GetArchived);

				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var x) ? x : 0;
			}
		}
		#endregion
	}
}
