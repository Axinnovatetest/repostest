using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{

	public class CountryISOAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_CountryISO] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_CountryISO]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [__BSD_CountryISO] WHERE [Id] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__BSD_CountryISO] ([alpha2Code],[alpha3Code],[Capital],[CreateTime],[CreateUserId],[Description],[Flag],[Name],[NativeName],[NumericCode],[Region],[Subregion],[UpdateTime],[UpdateUserId])  VALUES (@alpha2Code,@alpha3Code,@Capital,@CreateTime,@CreateUserId,@Description,@Flag,@Name,@NativeName,@NumericCode,@Region,@Subregion,@UpdateTime,@UpdateUserId);";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("alpha2Code", item.alpha2Code == null ? (object)DBNull.Value : item.alpha2Code);
					sqlCommand.Parameters.AddWithValue("alpha3Code", item.alpha3Code == null ? (object)DBNull.Value : item.alpha3Code);
					sqlCommand.Parameters.AddWithValue("Capital", item.Capital == null ? (object)DBNull.Value : item.Capital);
					sqlCommand.Parameters.AddWithValue("CreateTime", item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
					sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
					sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
					sqlCommand.Parameters.AddWithValue("Flag", item.Flag == null ? (object)DBNull.Value : item.Flag);
					sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? (object)DBNull.Value : item.Name);
					sqlCommand.Parameters.AddWithValue("NativeName", item.NativeName == null ? (object)DBNull.Value : item.NativeName);
					sqlCommand.Parameters.AddWithValue("NumericCode", item.NumericCode == null ? (object)DBNull.Value : item.NumericCode);
					sqlCommand.Parameters.AddWithValue("Region", item.Region == null ? (object)DBNull.Value : item.Region);
					sqlCommand.Parameters.AddWithValue("Subregion", item.Subregion == null ? (object)DBNull.Value : item.Subregion);
					sqlCommand.Parameters.AddWithValue("UpdateTime", item.UpdateTime == null ? (object)DBNull.Value : item.UpdateTime);
					sqlCommand.Parameters.AddWithValue("UpdateUserId", item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);


					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 15; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity> items)
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
						query += " INSERT INTO [__BSD_CountryISO] ([alpha2Code],[alpha3Code],[Capital],[CreateTime],[CreateUserId],[Description],[Flag],[Name],[NativeName],[NumericCode],[Region],[Subregion],[UpdateTime],[UpdateUserId]) VALUES ( "

							+ "@alpha2Code" + i + ","
							+ "@alpha3Code" + i + ","
							+ "@Capital" + i + ","
							+ "@CreateTime" + i + ","
							+ "@CreateUserId" + i + ","
							+ "@Description" + i + ","
							+ "@Flag" + i + ","
							+ "@Name" + i + ","
							+ "@NativeName" + i + ","
							+ "@NumericCode" + i + ","
							+ "@Region" + i + ","
							+ "@Subregion" + i + ","
							+ "@UpdateTime" + i + ","
							+ "@UpdateUserId" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("alpha2Code" + i, item.alpha2Code == null ? (object)DBNull.Value : item.alpha2Code);
						sqlCommand.Parameters.AddWithValue("alpha3Code" + i, item.alpha3Code == null ? (object)DBNull.Value : item.alpha3Code);
						sqlCommand.Parameters.AddWithValue("Capital" + i, item.Capital == null ? (object)DBNull.Value : item.Capital);
						sqlCommand.Parameters.AddWithValue("CreateTime" + i, item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
						sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("Flag" + i, item.Flag == null ? (object)DBNull.Value : item.Flag);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name == null ? (object)DBNull.Value : item.Name);
						sqlCommand.Parameters.AddWithValue("NativeName" + i, item.NativeName == null ? (object)DBNull.Value : item.NativeName);
						sqlCommand.Parameters.AddWithValue("NumericCode" + i, item.NumericCode == null ? (object)DBNull.Value : item.NumericCode);
						sqlCommand.Parameters.AddWithValue("Region" + i, item.Region == null ? (object)DBNull.Value : item.Region);
						sqlCommand.Parameters.AddWithValue("Subregion" + i, item.Subregion == null ? (object)DBNull.Value : item.Subregion);
						sqlCommand.Parameters.AddWithValue("UpdateTime" + i, item.UpdateTime == null ? (object)DBNull.Value : item.UpdateTime);
						sqlCommand.Parameters.AddWithValue("UpdateUserId" + i, item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__BSD_CountryISO] SET [alpha2Code]=@alpha2Code, [alpha3Code]=@alpha3Code, [Capital]=@Capital, [CreateTime]=@CreateTime, [CreateUserId]=@CreateUserId, [Description]=@Description, [Flag]=@Flag, [Name]=@Name, [NativeName]=@NativeName, [NumericCode]=@NumericCode, [Region]=@Region, [Subregion]=@Subregion, [UpdateTime]=@UpdateTime, [UpdateUserId]=@UpdateUserId WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("alpha2Code", item.alpha2Code == null ? (object)DBNull.Value : item.alpha2Code);
				sqlCommand.Parameters.AddWithValue("alpha3Code", item.alpha3Code == null ? (object)DBNull.Value : item.alpha3Code);
				sqlCommand.Parameters.AddWithValue("Capital", item.Capital == null ? (object)DBNull.Value : item.Capital);
				sqlCommand.Parameters.AddWithValue("CreateTime", item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
				sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
				sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
				sqlCommand.Parameters.AddWithValue("Flag", item.Flag == null ? (object)DBNull.Value : item.Flag);
				sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? (object)DBNull.Value : item.Name);
				sqlCommand.Parameters.AddWithValue("NativeName", item.NativeName == null ? (object)DBNull.Value : item.NativeName);
				sqlCommand.Parameters.AddWithValue("NumericCode", item.NumericCode == null ? (object)DBNull.Value : item.NumericCode);
				sqlCommand.Parameters.AddWithValue("Region", item.Region == null ? (object)DBNull.Value : item.Region);
				sqlCommand.Parameters.AddWithValue("Subregion", item.Subregion == null ? (object)DBNull.Value : item.Subregion);
				sqlCommand.Parameters.AddWithValue("UpdateTime", item.UpdateTime == null ? (object)DBNull.Value : item.UpdateTime);
				sqlCommand.Parameters.AddWithValue("UpdateUserId", item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 15; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity> items)
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
						query += " UPDATE [__BSD_CountryISO] SET "

							+ "[alpha2Code]=@alpha2Code" + i + ","
							+ "[alpha3Code]=@alpha3Code" + i + ","
							+ "[Capital]=@Capital" + i + ","
							+ "[CreateTime]=@CreateTime" + i + ","
							+ "[CreateUserId]=@CreateUserId" + i + ","
							+ "[Description]=@Description" + i + ","
							+ "[Flag]=@Flag" + i + ","
							+ "[Name]=@Name" + i + ","
							+ "[NativeName]=@NativeName" + i + ","
							+ "[NumericCode]=@NumericCode" + i + ","
							+ "[Region]=@Region" + i + ","
							+ "[Subregion]=@Subregion" + i + ","
							+ "[UpdateTime]=@UpdateTime" + i + ","
							+ "[UpdateUserId]=@UpdateUserId" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("alpha2Code" + i, item.alpha2Code == null ? (object)DBNull.Value : item.alpha2Code);
						sqlCommand.Parameters.AddWithValue("alpha3Code" + i, item.alpha3Code == null ? (object)DBNull.Value : item.alpha3Code);
						sqlCommand.Parameters.AddWithValue("Capital" + i, item.Capital == null ? (object)DBNull.Value : item.Capital);
						sqlCommand.Parameters.AddWithValue("CreateTime" + i, item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
						sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("Flag" + i, item.Flag == null ? (object)DBNull.Value : item.Flag);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name == null ? (object)DBNull.Value : item.Name);
						sqlCommand.Parameters.AddWithValue("NativeName" + i, item.NativeName == null ? (object)DBNull.Value : item.NativeName);
						sqlCommand.Parameters.AddWithValue("NumericCode" + i, item.NumericCode == null ? (object)DBNull.Value : item.NumericCode);
						sqlCommand.Parameters.AddWithValue("Region" + i, item.Region == null ? (object)DBNull.Value : item.Region);
						sqlCommand.Parameters.AddWithValue("Subregion" + i, item.Subregion == null ? (object)DBNull.Value : item.Subregion);
						sqlCommand.Parameters.AddWithValue("UpdateTime" + i, item.UpdateTime == null ? (object)DBNull.Value : item.UpdateTime);
						sqlCommand.Parameters.AddWithValue("UpdateUserId" + i, item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);
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
				string query = "DELETE FROM [__BSD_CountryISO] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__BSD_CountryISO] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		public static Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity CheckName(string name)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_CountryISO] WHERE [Name]=@name";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("name", name);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity CheckNativeName(string name)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_CountryISO] WHERE [NativeName]=@name";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("name", name);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity CheckNumericCode(string code)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_CountryISO] WHERE [NumericCode]=@code";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("code", code);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity CheckAlpha2Code(string code)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_CountryISO] WHERE [alpha2Code]=@code";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("code", code);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity CheckAlpha3Code(string code)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_CountryISO] WHERE [alpha3Code]=@code";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("code", code);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.CountryISOEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		#endregion
	}
}
