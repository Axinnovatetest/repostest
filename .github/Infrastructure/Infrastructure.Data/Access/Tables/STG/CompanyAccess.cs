using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.STG
{

	public class CompanyAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.STG.CompanyEntity Get(long id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__STG_Company] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.STG.CompanyEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.STG.CompanyEntity> Get(bool includeClosed = true)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__STG_Company]{(includeClosed ? "" : " WHERE ISNULL([Closed],0)<>1;")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.STG.CompanyEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.STG.CompanyEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.STG.CompanyEntity> Get(List<long> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.STG.CompanyEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.STG.CompanyEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.STG.CompanyEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.STG.CompanyEntity> get(List<long> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__STG_Company] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.STG.CompanyEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.STG.CompanyEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.STG.CompanyEntity>();
		}
		public static int Insert(Infrastructure.Data.Entities.Tables.STG.CompanyEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__STG_Company] ([Address],[Address2],[City],[Country],[CreateTime],[CreateUserId],[Description],[DirectorEmail],[DirectorId],[DirectorName],[Email],[Fax],[IsActive],[LagalName],[LastUpdateTime],[LastUpdateUserId],[Logo],[LogoExtension],[Name],[PostalCode],[Telephone],[Telephone2],[Type])  VALUES (@Address,@Address2,@City,@Country,@CreateTime,@CreateUserId,@Description,@DirectorEmail,@DirectorId,@DirectorName,@Email,@Fax,@IsActive,@LagalName,@LastUpdateTime,@LastUpdateUserId,@Logo,@LogoExtension,@Name,@PostalCode,@Telephone,@Telephone2,@Type); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Address", item.Address == null ? (object)DBNull.Value : item.Address);
					sqlCommand.Parameters.AddWithValue("Address2", item.Address2 == null ? (object)DBNull.Value : item.Address2);
					sqlCommand.Parameters.AddWithValue("City", item.City == null ? (object)DBNull.Value : item.City);
					sqlCommand.Parameters.AddWithValue("Country", item.Country == null ? (object)DBNull.Value : item.Country);
					sqlCommand.Parameters.AddWithValue("CreateTime", item.CreateTime);
					sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId);
					sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
					sqlCommand.Parameters.AddWithValue("DirectorEmail", item.DirectorEmail == null ? (object)DBNull.Value : item.DirectorEmail);
					sqlCommand.Parameters.AddWithValue("DirectorId", item.DirectorId == null ? (object)DBNull.Value : item.DirectorId);
					sqlCommand.Parameters.AddWithValue("DirectorName", item.DirectorName == null ? (object)DBNull.Value : item.DirectorName);
					sqlCommand.Parameters.AddWithValue("Email", item.Email == null ? (object)DBNull.Value : item.Email);
					sqlCommand.Parameters.AddWithValue("Fax", item.Fax == null ? (object)DBNull.Value : item.Fax);
					sqlCommand.Parameters.AddWithValue("IsActive", item.IsActive == null ? (object)DBNull.Value : item.IsActive);
					sqlCommand.Parameters.AddWithValue("LagalName", item.LagalName == null ? (object)DBNull.Value : item.LagalName);
					sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
					sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
					sqlCommand.Parameters.AddWithValue("Logo", item.Logo == null ? (object)DBNull.Value : item.Logo);
					sqlCommand.Parameters.AddWithValue("LogoExtension", item.LogoExtension == null ? (object)DBNull.Value : item.LogoExtension);
					sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? (object)DBNull.Value : item.Name);
					sqlCommand.Parameters.AddWithValue("PostalCode", item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
					sqlCommand.Parameters.AddWithValue("Telephone", item.Telephone == null ? (object)DBNull.Value : item.Telephone);
					sqlCommand.Parameters.AddWithValue("Telephone2", item.Telephone2 == null ? (object)DBNull.Value : item.Telephone2);
					sqlCommand.Parameters.AddWithValue("Type", item.Type == null ? (object)DBNull.Value : item.Type);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.STG.CompanyEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 24; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.STG.CompanyEntity> items)
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
						query += " INSERT INTO [__STG_Company] ([Address],[Address2],[City],[Country],[CreateTime],[CreateUserId],[Description],[DirectorEmail],[DirectorId],[DirectorName],[Email],[Fax],[IsActive],[LagalName],[LastUpdateTime],[LastUpdateUserId],[Logo],[LogoExtension],[Name],[PostalCode],[Telephone],[Telephone2],[Type]) VALUES ( "

							+ "@Address" + i + ","
							+ "@Address2" + i + ","
							+ "@City" + i + ","
							+ "@Country" + i + ","
							+ "@CreateTime" + i + ","
							+ "@CreateUserId" + i + ","
							+ "@Description" + i + ","
							+ "@DirectorEmail" + i + ","
							+ "@DirectorId" + i + ","
							+ "@DirectorName" + i + ","
							+ "@Email" + i + ","
							+ "@Fax" + i + ","
							+ "@IsActive" + i + ","
							+ "@LagalName" + i + ","
							+ "@LastUpdateTime" + i + ","
							+ "@LastUpdateUserId" + i + ","
							+ "@Logo" + i + ","
							+ "@LogoExtension" + i + ","
							+ "@Name" + i + ","
							+ "@PostalCode" + i + ","
							+ "@Telephone" + i + ","
							+ "@Telephone2" + i + ","
							+ "@Type" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Address" + i, item.Address == null ? (object)DBNull.Value : item.Address);
						sqlCommand.Parameters.AddWithValue("Address2" + i, item.Address2 == null ? (object)DBNull.Value : item.Address2);
						sqlCommand.Parameters.AddWithValue("City" + i, item.City == null ? (object)DBNull.Value : item.City);
						sqlCommand.Parameters.AddWithValue("Country" + i, item.Country == null ? (object)DBNull.Value : item.Country);
						sqlCommand.Parameters.AddWithValue("CreateTime" + i, item.CreateTime);
						sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("DirectorEmail" + i, item.DirectorEmail == null ? (object)DBNull.Value : item.DirectorEmail);
						sqlCommand.Parameters.AddWithValue("DirectorId" + i, item.DirectorId == null ? (object)DBNull.Value : item.DirectorId);
						sqlCommand.Parameters.AddWithValue("DirectorName" + i, item.DirectorName == null ? (object)DBNull.Value : item.DirectorName);
						sqlCommand.Parameters.AddWithValue("Email" + i, item.Email == null ? (object)DBNull.Value : item.Email);
						sqlCommand.Parameters.AddWithValue("Fax" + i, item.Fax == null ? (object)DBNull.Value : item.Fax);
						sqlCommand.Parameters.AddWithValue("IsActive" + i, item.IsActive == null ? (object)DBNull.Value : item.IsActive);
						sqlCommand.Parameters.AddWithValue("LagalName" + i, item.LagalName == null ? (object)DBNull.Value : item.LagalName);
						sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
						sqlCommand.Parameters.AddWithValue("LastUpdateUserId" + i, item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
						sqlCommand.Parameters.AddWithValue("Logo" + i, item.Logo == null ? null : item.Logo);
						sqlCommand.Parameters.AddWithValue("LogoExtension" + i, item.LogoExtension == null ? (object)DBNull.Value : item.LogoExtension);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name == null ? (object)DBNull.Value : item.Name);
						sqlCommand.Parameters.AddWithValue("PostalCode" + i, item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
						sqlCommand.Parameters.AddWithValue("Telephone" + i, item.Telephone == null ? (object)DBNull.Value : item.Telephone);
						sqlCommand.Parameters.AddWithValue("Telephone2" + i, item.Telephone2 == null ? (object)DBNull.Value : item.Telephone2);
						sqlCommand.Parameters.AddWithValue("Type" + i, item.Type == null ? (object)DBNull.Value : item.Type);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}
		public static int Update(Infrastructure.Data.Entities.Tables.STG.CompanyEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__STG_Company] SET [Address]=@Address, [Address2]=@Address2, [City]=@City, [Country]=@Country, [CreateTime]=@CreateTime, [CreateUserId]=@CreateUserId, [Description]=@Description, [DirectorEmail]=@DirectorEmail, [DirectorId]=@DirectorId, [DirectorName]=@DirectorName, [Email]=@Email, [Fax]=@Fax, [IsActive]=@IsActive, [LagalName]=@LagalName, [LastUpdateTime]=@LastUpdateTime, [LastUpdateUserId]=@LastUpdateUserId, [Logo]=@Logo, [LogoExtension]=@LogoExtension, [Name]=@Name, [PostalCode]=@PostalCode, [Telephone]=@Telephone, [Telephone2]=@Telephone2, [Type]=@Type WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Address", item.Address == null ? (object)DBNull.Value : item.Address);
				sqlCommand.Parameters.AddWithValue("Address2", item.Address2 == null ? (object)DBNull.Value : item.Address2);
				sqlCommand.Parameters.AddWithValue("City", item.City == null ? (object)DBNull.Value : item.City);
				sqlCommand.Parameters.AddWithValue("Country", item.Country == null ? (object)DBNull.Value : item.Country);
				sqlCommand.Parameters.AddWithValue("CreateTime", item.CreateTime);
				sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId);
				sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
				sqlCommand.Parameters.AddWithValue("DirectorEmail", item.DirectorEmail == null ? (object)DBNull.Value : item.DirectorEmail);
				sqlCommand.Parameters.AddWithValue("DirectorId", item.DirectorId == null ? (object)DBNull.Value : item.DirectorId);
				sqlCommand.Parameters.AddWithValue("DirectorName", item.DirectorName == null ? (object)DBNull.Value : item.DirectorName);
				sqlCommand.Parameters.AddWithValue("Email", item.Email == null ? (object)DBNull.Value : item.Email);
				sqlCommand.Parameters.AddWithValue("Fax", item.Fax == null ? (object)DBNull.Value : item.Fax);
				sqlCommand.Parameters.AddWithValue("IsActive", item.IsActive == null ? (object)DBNull.Value : item.IsActive);
				sqlCommand.Parameters.AddWithValue("LagalName", item.LagalName == null ? (object)DBNull.Value : item.LagalName);
				sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
				sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
				sqlCommand.Parameters.AddWithValue("Logo", item.Logo == null ? null : item.Logo);
				sqlCommand.Parameters.AddWithValue("LogoExtension", item.LogoExtension == null ? (object)DBNull.Value : item.LogoExtension);
				sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? (object)DBNull.Value : item.Name);
				sqlCommand.Parameters.AddWithValue("PostalCode", item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
				sqlCommand.Parameters.AddWithValue("Telephone", item.Telephone == null ? (object)DBNull.Value : item.Telephone);
				sqlCommand.Parameters.AddWithValue("Telephone2", item.Telephone2 == null ? (object)DBNull.Value : item.Telephone2);
				sqlCommand.Parameters.AddWithValue("Type", item.Type == null ? (object)DBNull.Value : item.Type);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.STG.CompanyEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 24; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.STG.CompanyEntity> items)
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
						query += " UPDATE [__STG_Company] SET "

							+ "[Address]=@Address" + i + ","
							+ "[Address2]=@Address2" + i + ","
							+ "[City]=@City" + i + ","
							+ "[Country]=@Country" + i + ","
							+ "[CreateTime]=@CreateTime" + i + ","
							+ "[CreateUserId]=@CreateUserId" + i + ","
							+ "[Description]=@Description" + i + ","
							+ "[DirectorEmail]=@DirectorEmail" + i + ","
							+ "[DirectorId]=@DirectorId" + i + ","
							+ "[DirectorName]=@DirectorName" + i + ","
							+ "[Email]=@Email" + i + ","
							+ "[Fax]=@Fax" + i + ","
							+ "[IsActive]=@IsActive" + i + ","
							+ "[LagalName]=@LagalName" + i + ","
							+ "[LastUpdateTime]=@LastUpdateTime" + i + ","
							+ "[LastUpdateUserId]=@LastUpdateUserId" + i + ","
							+ "[Logo]=@Logo" + i + ","
							+ "[LogoExtension]=@LogoExtension" + i + ","
							+ "[Name]=@Name" + i + ","
							+ "[PostalCode]=@PostalCode" + i + ","
							+ "[Telephone]=@Telephone" + i + ","
							+ "[Telephone2]=@Telephone2" + i + ","
							+ "[Type]=@Type" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("Address" + i, item.Address == null ? (object)DBNull.Value : item.Address);
						sqlCommand.Parameters.AddWithValue("Address2" + i, item.Address2 == null ? (object)DBNull.Value : item.Address2);
						sqlCommand.Parameters.AddWithValue("City" + i, item.City == null ? (object)DBNull.Value : item.City);
						sqlCommand.Parameters.AddWithValue("Country" + i, item.Country == null ? (object)DBNull.Value : item.Country);
						sqlCommand.Parameters.AddWithValue("CreateTime" + i, item.CreateTime);
						sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("DirectorEmail" + i, item.DirectorEmail == null ? (object)DBNull.Value : item.DirectorEmail);
						sqlCommand.Parameters.AddWithValue("DirectorId" + i, item.DirectorId == null ? (object)DBNull.Value : item.DirectorId);
						sqlCommand.Parameters.AddWithValue("DirectorName" + i, item.DirectorName == null ? (object)DBNull.Value : item.DirectorName);
						sqlCommand.Parameters.AddWithValue("Email" + i, item.Email == null ? (object)DBNull.Value : item.Email);
						sqlCommand.Parameters.AddWithValue("Fax" + i, item.Fax == null ? (object)DBNull.Value : item.Fax);
						sqlCommand.Parameters.AddWithValue("IsActive" + i, item.IsActive == null ? (object)DBNull.Value : item.IsActive);
						sqlCommand.Parameters.AddWithValue("LagalName" + i, item.LagalName == null ? (object)DBNull.Value : item.LagalName);
						sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
						sqlCommand.Parameters.AddWithValue("LastUpdateUserId" + i, item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
						sqlCommand.Parameters.AddWithValue("Logo" + i, item.Logo == null ? null : item.Logo);
						sqlCommand.Parameters.AddWithValue("LogoExtension" + i, item.LogoExtension == null ? (object)DBNull.Value : item.LogoExtension);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name == null ? (object)DBNull.Value : item.Name);
						sqlCommand.Parameters.AddWithValue("PostalCode" + i, item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
						sqlCommand.Parameters.AddWithValue("Telephone" + i, item.Telephone == null ? (object)DBNull.Value : item.Telephone);
						sqlCommand.Parameters.AddWithValue("Telephone2" + i, item.Telephone2 == null ? (object)DBNull.Value : item.Telephone2);
						sqlCommand.Parameters.AddWithValue("Type" + i, item.Type == null ? (object)DBNull.Value : item.Type);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}
		public static int Delete(long id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__STG_Company] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Delete(List<long> ids)
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
		private static int delete(List<long> ids)
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

					string query = "DELETE FROM [__STG_Company] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion
		#region Custom Methods
		public static Infrastructure.Data.Entities.Tables.STG.CompanyEntity GetByName(string name)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__STG_Company] WHERE [Name] LIKE @name";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("name", name ?? "");

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.STG.CompanyEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.STG.CompanyEntity> FilterByName(string name)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__STG_Company] WHERE [Name] LIKE %@name%";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("name", name ?? "");

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.STG.CompanyEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.STG.CompanyEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.STG.CompanyEntity> GetByDirectorId(List<int> id)
		{
			if(id == null || id.Count <= 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.STG.CompanyEntity>();
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__STG_Company] WHERE [DirectorId] IN ({string.Join(",", id)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.STG.CompanyEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.STG.CompanyEntity>();
			}
		}
		public static int UpdateLogo(Infrastructure.Data.Entities.Tables.STG.CompanyEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__STG_Company] SET [LastUpdateTime]=@LastUpdateTime, [LastUpdateUserId]=@LastUpdateUserId, [Logo]=CONVERT(varbinary(max),@Logo), [LogoExtension]=@LogoExtension WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
				sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
				sqlCommand.Parameters.AddWithValue("Logo", item.Logo == null ? (object)DBNull.Value : item.Logo);
				sqlCommand.Parameters.AddWithValue("LogoExtension", item.LogoExtension == null ? (object)DBNull.Value : item.LogoExtension);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int InsertWoLogo(Infrastructure.Data.Entities.Tables.STG.CompanyEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__STG_Company] ([Address],[Address2],[City],[Country],[CreateTime],[CreateUserId],[Description],[DirectorEmail],[DirectorId],[DirectorName],[Email],[Fax],[IsActive],[LagalName],[LastUpdateTime],[LastUpdateUserId],[Name],[PostalCode],[Telephone],[Telephone2],[Type])  VALUES (@Address,@Address2,@City,@Country,@CreateTime,@CreateUserId,@Description,@DirectorEmail,@DirectorId,@DirectorName,@Email,@Fax,@IsActive,@LagalName,@LastUpdateTime,@LastUpdateUserId,@Name,@PostalCode,@Telephone,@Telephone2,@Type); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Address", item.Address == null ? (object)DBNull.Value : item.Address);
					sqlCommand.Parameters.AddWithValue("Address2", item.Address2 == null ? (object)DBNull.Value : item.Address2);
					sqlCommand.Parameters.AddWithValue("City", item.City == null ? (object)DBNull.Value : item.City);
					sqlCommand.Parameters.AddWithValue("Country", item.Country == null ? (object)DBNull.Value : item.Country);
					sqlCommand.Parameters.AddWithValue("CreateTime", item.CreateTime);
					sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId);
					sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
					sqlCommand.Parameters.AddWithValue("DirectorEmail", item.DirectorEmail == null ? (object)DBNull.Value : item.DirectorEmail);
					sqlCommand.Parameters.AddWithValue("DirectorId", item.DirectorId == null ? (object)DBNull.Value : item.DirectorId);
					sqlCommand.Parameters.AddWithValue("DirectorName", item.DirectorName == null ? (object)DBNull.Value : item.DirectorName);
					sqlCommand.Parameters.AddWithValue("Email", item.Email == null ? (object)DBNull.Value : item.Email);
					sqlCommand.Parameters.AddWithValue("Fax", item.Fax == null ? (object)DBNull.Value : item.Fax);
					sqlCommand.Parameters.AddWithValue("IsActive", item.IsActive == null ? (object)DBNull.Value : item.IsActive);
					sqlCommand.Parameters.AddWithValue("LagalName", item.LagalName == null ? (object)DBNull.Value : item.LagalName);
					sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
					sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
					sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? (object)DBNull.Value : item.Name);
					sqlCommand.Parameters.AddWithValue("PostalCode", item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
					sqlCommand.Parameters.AddWithValue("Telephone", item.Telephone == null ? (object)DBNull.Value : item.Telephone);
					sqlCommand.Parameters.AddWithValue("Telephone2", item.Telephone2 == null ? (object)DBNull.Value : item.Telephone2);
					sqlCommand.Parameters.AddWithValue("Type", item.Type == null ? (object)DBNull.Value : item.Type);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static Infrastructure.Data.Entities.Tables.STG.CompanyEntity GetFirst()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT TOP 1 * FROM [__STG_Company] ORDER BY Id ASC";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.STG.CompanyEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		//REM: BAD CODES - hard coded where clauses
		public static Infrastructure.Data.Entities.Tables.STG.CompanyEntity GetTN()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__STG_Company] WHERE [Name] LIKE 'PSZ TUNISIE%'";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.STG.CompanyEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.STG.CompanyEntity GetTNWS()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__STG_Company] WHERE [Name] LIKE '%WS%'";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.STG.CompanyEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.STG.CompanyEntity GetTNGZ()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__STG_Company] WHERE [Name] LIKE '%Ghezala%'";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.STG.CompanyEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.STG.CompanyEntity GetCZ()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__STG_Company] WHERE [Name] LIKE '%Czech%'";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.STG.CompanyEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.STG.CompanyEntity GetAL()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__STG_Company] WHERE [Name] LIKE '%Albani%'";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.STG.CompanyEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		#endregion
	}
}