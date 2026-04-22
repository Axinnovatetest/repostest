using System.Data.Common;

namespace Infrastructure.Data.Access.Tables.COR
{

	public class UserAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.COR.UserEntity Get(int id, bool ignoreDeleted = true)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [User] WHERE [Id]=@Id{(ignoreDeleted ? $" AND [Delete_Date] IS NULL" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.COR.UserEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.COR.UserEntity> Get(bool ignoreDeleted = true)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [User]{(ignoreDeleted ? $" WHERE [Delete_Date] IS NULL" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.COR.UserEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.COR.UserEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.COR.UserEntity> Get(List<int> ids, bool ignoreDeleted = true)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.COR.UserEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids, ignoreDeleted);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.COR.UserEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber), ignoreDeleted));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), ignoreDeleted));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.COR.UserEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.COR.UserEntity> get(List<int> ids, bool ignoreDeleted = true)
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

					sqlCommand.CommandText = $"SELECT * FROM [User] WHERE [Id] IN ({queryIds}){(ignoreDeleted ? $" AND [Delete_Date] IS NULL" : "")}";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.COR.UserEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.COR.UserEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.COR.UserEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.COR.UserEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [User] ([AccessProfileId],[CompanyId],[CountryId],[Creation_User_Id],[CreationTime],[CustomerServiceApp],[Delete_Date],[Delete_User_Id],[DepartmentId],[Email],[Fax],[FinanceControlApp],[HumanResourcesApp],[Is_Archived],[IsActivated],[IsAdministrator],[IsCorporateDirector],[IsGlobalDirector],[Last_Edit_Date],[Last_Edit_User_Id],[LegacyUsername],[LogisticsApp],[MasterDataApp],[MaterialManagementApp],[Name],[Nummer],[Password],[SalesDistributionApp],[SelectedLanguage],[SettingsApp],[SuperAdministrator],[TelephoneHome],[TelephoneIP],[TelephoneMobile],[Username]) OUTPUT INSERTED.[Id] VALUES (@AccessProfileId,@CompanyId,@CountryId,@Creation_User_Id,@CreationTime,@CustomerServiceApp,@Delete_Date,@Delete_User_Id,@DepartmentId,@Email,@Fax,@FinanceControlApp,@HumanResourcesApp,@Is_Archived,@IsActivated,@IsAdministrator,@IsCorporateDirector,@IsGlobalDirector,@Last_Edit_Date,@Last_Edit_User_Id,@LegacyUsername,@LogisticsApp,@MasterDataApp,@MaterialManagementApp,@Name,@Nummer,@Password,@SalesDistributionApp,@SelectedLanguage,@SettingsApp,@SuperAdministrator,@TelephoneHome,@TelephoneIP,@TelephoneMobile,@Username); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AccessProfileId", item.AccessProfileId);
					sqlCommand.Parameters.AddWithValue("CompanyId", item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
					sqlCommand.Parameters.AddWithValue("CountryId", item.CountryId == null ? (object)DBNull.Value : item.CountryId);
					sqlCommand.Parameters.AddWithValue("Creation_User_Id", item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CustomerServiceApp", item.CustomerServiceApp == null ? (object)DBNull.Value : item.CustomerServiceApp);
					sqlCommand.Parameters.AddWithValue("Delete_Date", item.DeleteDate == null ? (object)DBNull.Value : item.DeleteDate);
					sqlCommand.Parameters.AddWithValue("Delete_User_Id", item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
					sqlCommand.Parameters.AddWithValue("DepartmentId", item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
					sqlCommand.Parameters.AddWithValue("Email", item.Email == null ? (object)DBNull.Value : item.Email);
					sqlCommand.Parameters.AddWithValue("Fax", item.Fax == null ? (object)DBNull.Value : item.Fax);
					sqlCommand.Parameters.AddWithValue("FinanceControlApp", item.FinanceControlApp == null ? (object)DBNull.Value : item.FinanceControlApp);
					sqlCommand.Parameters.AddWithValue("HumanResourcesApp", item.HumanResourcesApp == null ? (object)DBNull.Value : item.HumanResourcesApp);
					sqlCommand.Parameters.AddWithValue("Is_Archived", item.IsArchived);
					sqlCommand.Parameters.AddWithValue("IsActivated", item.IsActivated);
					sqlCommand.Parameters.AddWithValue("IsAdministrator", item.IsAdministrator == null ? (object)DBNull.Value : item.IsAdministrator);
					sqlCommand.Parameters.AddWithValue("IsCorporateDirector", item.IsCorporateDirector == null ? (object)DBNull.Value : item.IsCorporateDirector);
					sqlCommand.Parameters.AddWithValue("IsGlobalDirector", item.IsGlobalDirector == null ? (object)DBNull.Value : item.IsGlobalDirector);
					sqlCommand.Parameters.AddWithValue("Last_Edit_Date", item.LastEditDate == null ? (object)DBNull.Value : item.LastEditDate);
					sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("LegacyUsername", item.LegacyUsername == null ? (object)DBNull.Value : item.LegacyUsername);
					sqlCommand.Parameters.AddWithValue("LogisticsApp", item.LogisticsApp == null ? (object)DBNull.Value : item.LogisticsApp);
					sqlCommand.Parameters.AddWithValue("MasterDataApp", item.MasterDataApp == null ? (object)DBNull.Value : item.MasterDataApp);
					sqlCommand.Parameters.AddWithValue("MaterialManagementApp", item.MaterialManagementApp == null ? (object)DBNull.Value : item.MaterialManagementApp);
					sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? (object)DBNull.Value : item.Name);
					sqlCommand.Parameters.AddWithValue("Nummer", item.Nummer == null ? (object)DBNull.Value : item.Nummer);
					sqlCommand.Parameters.AddWithValue("Password", item.Password);
					sqlCommand.Parameters.AddWithValue("SalesDistributionApp", item.SalesDistributionApp == null ? (object)DBNull.Value : item.SalesDistributionApp);
					sqlCommand.Parameters.AddWithValue("SelectedLanguage", item.SelectedLanguage == null ? (object)DBNull.Value : item.SelectedLanguage);
					sqlCommand.Parameters.AddWithValue("SettingsApp", item.SettingsApp == null ? (object)DBNull.Value : item.SettingsApp);
					sqlCommand.Parameters.AddWithValue("SuperAdministrator", item.SuperAdministrator);
					sqlCommand.Parameters.AddWithValue("TelephoneHome", item.TelephoneHome == null ? (object)DBNull.Value : item.TelephoneHome);
					sqlCommand.Parameters.AddWithValue("TelephoneIP", item.TelephoneIP == null ? (object)DBNull.Value : item.TelephoneIP);
					sqlCommand.Parameters.AddWithValue("TelephoneMobile", item.TelephoneMobile == null ? (object)DBNull.Value : item.TelephoneMobile);
					sqlCommand.Parameters.AddWithValue("Username", item.Username == null ? (object)DBNull.Value : item.Username);

					var result = DbExecution.ExecuteScalar(sqlCommand); 
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.COR.UserEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 39; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.COR.UserEntity> items)
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
						query += " INSERT INTO [User] ([AccessProfileId],[CompanyId],[CountryId],[Creation_User_Id],[CreationTime],[CustomerServiceApp],[Delete_Date],[Delete_User_Id],[DepartmentId],[Email],[Fax],[FinanceControlApp],[HumanResourcesApp],[Is_Archived],[IsActivated],[IsAdministrator],[IsCorporateDirector],[IsGlobalDirector],[Last_Edit_Date],[Last_Edit_User_Id],[LegacyUsername],[LogisticsApp],[MasterDataApp],[MaterialManagementApp],[Name],[Nummer],[Password],[SalesDistributionApp],[SelectedLanguage],[SettingsApp],[SuperAdministrator],[TelephoneHome],[TelephoneIP],[TelephoneMobile],[Username]) VALUES ( "

							+ "@AccessProfileId" + i + ","
							+ "@CompanyId" + i + ","
							+ "@CountryId" + i + ","
							+ "@Creation_User_Id" + i + ","
							+ "@CreationTime" + i + ","
							+ "@CustomerServiceApp" + i + ","
							+ "@Delete_Date" + i + ","
							+ "@Delete_User_Id" + i + ","
							+ "@DepartmentId" + i + ","
							+ "@Email" + i + ","
							+ "@Fax" + i + ","
							+ "@FinanceControlApp" + i + ","
							+ "@HumanResourcesApp" + i + ","
							+ "@Is_Archived" + i + ","
							+ "@IsActivated" + i + ","
							+ "@IsAdministrator" + i + ","
							+ "@IsCorporateDirector" + i + ","
							+ "@IsGlobalDirector" + i + ","
							+ "@Last_Edit_Date" + i + ","
							+ "@Last_Edit_User_Id" + i + ","
							+ "@LegacyUsername" + i + ","
							+ "@LogisticsApp" + i + ","
							+ "@MasterDataApp" + i + ","
							+ "@MaterialManagementApp" + i + ","
							+ "@Name" + i + ","
							+ "@Nummer" + i + ","
							+ "@Password" + i + ","
							+ "@SalesDistributionApp" + i + ","
							+ "@SelectedLanguage" + i + ","
							+ "@SettingsApp" + i + ","
							+ "@SuperAdministrator" + i + ","
							+ "@TelephoneHome" + i + ","
							+ "@TelephoneIP" + i + ","
							+ "@TelephoneMobile" + i + ","
							+ "@Username" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("AccessProfileId" + i, item.AccessProfileId);
						sqlCommand.Parameters.AddWithValue("CompanyId" + i, item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
						sqlCommand.Parameters.AddWithValue("CountryId" + i, item.CountryId == null ? (object)DBNull.Value : item.CountryId);
						sqlCommand.Parameters.AddWithValue("Creation_User_Id" + i, item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CustomerServiceApp" + i, item.CustomerServiceApp == null ? (object)DBNull.Value : item.CustomerServiceApp);
						sqlCommand.Parameters.AddWithValue("Delete_Date" + i, item.DeleteDate == null ? (object)DBNull.Value : item.DeleteDate);
						sqlCommand.Parameters.AddWithValue("Delete_User_Id" + i, item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
						sqlCommand.Parameters.AddWithValue("DepartmentId" + i, item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
						sqlCommand.Parameters.AddWithValue("Email" + i, item.Email == null ? (object)DBNull.Value : item.Email);
						sqlCommand.Parameters.AddWithValue("Fax" + i, item.Fax == null ? (object)DBNull.Value : item.Fax);
						sqlCommand.Parameters.AddWithValue("FinanceControlApp" + i, item.FinanceControlApp == null ? (object)DBNull.Value : item.FinanceControlApp);
						sqlCommand.Parameters.AddWithValue("HumanResourcesApp" + i, item.HumanResourcesApp == null ? (object)DBNull.Value : item.HumanResourcesApp);
						sqlCommand.Parameters.AddWithValue("Is_Archived" + i, item.IsArchived);
						sqlCommand.Parameters.AddWithValue("IsActivated" + i, item.IsActivated);
						sqlCommand.Parameters.AddWithValue("IsAdministrator" + i, item.IsAdministrator == null ? (object)DBNull.Value : item.IsAdministrator);
						sqlCommand.Parameters.AddWithValue("IsCorporateDirector" + i, item.IsCorporateDirector == null ? (object)DBNull.Value : item.IsCorporateDirector);
						sqlCommand.Parameters.AddWithValue("IsGlobalDirector" + i, item.IsGlobalDirector == null ? (object)DBNull.Value : item.IsGlobalDirector);
						sqlCommand.Parameters.AddWithValue("Last_Edit_Date" + i, item.LastEditDate == null ? (object)DBNull.Value : item.LastEditDate);
						sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
						sqlCommand.Parameters.AddWithValue("LegacyUsername" + i, item.LegacyUsername == null ? (object)DBNull.Value : item.LegacyUsername);
						sqlCommand.Parameters.AddWithValue("LogisticsApp" + i, item.LogisticsApp == null ? (object)DBNull.Value : item.LogisticsApp);
						sqlCommand.Parameters.AddWithValue("MasterDataApp" + i, item.MasterDataApp == null ? (object)DBNull.Value : item.MasterDataApp);
						sqlCommand.Parameters.AddWithValue("MaterialManagementApp" + i, item.MaterialManagementApp == null ? (object)DBNull.Value : item.MaterialManagementApp);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name == null ? (object)DBNull.Value : item.Name);
						sqlCommand.Parameters.AddWithValue("Nummer" + i, item.Nummer == null ? (object)DBNull.Value : item.Nummer);
						sqlCommand.Parameters.AddWithValue("Password" + i, item.Password);
						sqlCommand.Parameters.AddWithValue("SalesDistributionApp" + i, item.SalesDistributionApp == null ? (object)DBNull.Value : item.SalesDistributionApp);
						sqlCommand.Parameters.AddWithValue("SelectedLanguage" + i, item.SelectedLanguage == null ? (object)DBNull.Value : item.SelectedLanguage);
						sqlCommand.Parameters.AddWithValue("SettingsApp" + i, item.SettingsApp == null ? (object)DBNull.Value : item.SettingsApp);
						sqlCommand.Parameters.AddWithValue("SuperAdministrator" + i, item.SuperAdministrator);
						sqlCommand.Parameters.AddWithValue("TelephoneHome" + i, item.TelephoneHome == null ? (object)DBNull.Value : item.TelephoneHome);
						sqlCommand.Parameters.AddWithValue("TelephoneIP" + i, item.TelephoneIP == null ? (object)DBNull.Value : item.TelephoneIP);
						sqlCommand.Parameters.AddWithValue("TelephoneMobile" + i, item.TelephoneMobile == null ? (object)DBNull.Value : item.TelephoneMobile);
						sqlCommand.Parameters.AddWithValue("Username" + i, item.Username == null ? (object)DBNull.Value : item.Username);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.COR.UserEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [User] SET [AccessProfileId]=@AccessProfileId, [CompanyId]=@CompanyId, [CountryId]=@CountryId, [Creation_User_Id]=@Creation_User_Id, [CreationTime]=@CreationTime, [CustomerServiceApp]=@CustomerServiceApp, [Delete_Date]=@Delete_Date, [Delete_User_Id]=@Delete_User_Id, [DepartmentId]=@DepartmentId, [Email]=@Email, [Fax]=@Fax, [FinanceControlApp]=@FinanceControlApp, [HumanResourcesApp]=@HumanResourcesApp, [Is_Archived]=@Is_Archived, [IsActivated]=@IsActivated, [IsAdministrator]=@IsAdministrator, [IsCorporateDirector]=@IsCorporateDirector, [IsGlobalDirector]=@IsGlobalDirector, [Last_Edit_Date]=@Last_Edit_Date, [Last_Edit_User_Id]=@Last_Edit_User_Id, [LegacyUsername]=@LegacyUsername, [LogisticsApp]=@LogisticsApp, [MasterDataApp]=@MasterDataApp, [MaterialManagementApp]=@MaterialManagementApp, [Name]=@Name, [Nummer]=@Nummer, [Password]=@Password, [SalesDistributionApp]=@SalesDistributionApp, [SelectedLanguage]=@SelectedLanguage, [SettingsApp]=@SettingsApp, [SuperAdministrator]=@SuperAdministrator, [TelephoneHome]=@TelephoneHome, [TelephoneIP]=@TelephoneIP, [TelephoneMobile]=@TelephoneMobile, [Username]=@Username WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AccessProfileId", item.AccessProfileId);
				sqlCommand.Parameters.AddWithValue("CompanyId", item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
				sqlCommand.Parameters.AddWithValue("CountryId", item.CountryId == null ? (object)DBNull.Value : item.CountryId);
				sqlCommand.Parameters.AddWithValue("Creation_User_Id", item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CustomerServiceApp", item.CustomerServiceApp == null ? (object)DBNull.Value : item.CustomerServiceApp);
				sqlCommand.Parameters.AddWithValue("Delete_Date", item.DeleteDate == null ? (object)DBNull.Value : item.DeleteDate);
				sqlCommand.Parameters.AddWithValue("Delete_User_Id", item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
				sqlCommand.Parameters.AddWithValue("DepartmentId", item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
				sqlCommand.Parameters.AddWithValue("Email", item.Email == null ? (object)DBNull.Value : item.Email);
				sqlCommand.Parameters.AddWithValue("Fax", item.Fax == null ? (object)DBNull.Value : item.Fax);
				sqlCommand.Parameters.AddWithValue("FinanceControlApp", item.FinanceControlApp == null ? (object)DBNull.Value : item.FinanceControlApp);
				sqlCommand.Parameters.AddWithValue("HumanResourcesApp", item.HumanResourcesApp == null ? (object)DBNull.Value : item.HumanResourcesApp);
				sqlCommand.Parameters.AddWithValue("Is_Archived", item.IsArchived);
				sqlCommand.Parameters.AddWithValue("IsActivated", item.IsActivated);
				sqlCommand.Parameters.AddWithValue("IsAdministrator", item.IsAdministrator == null ? (object)DBNull.Value : item.IsAdministrator);
				sqlCommand.Parameters.AddWithValue("IsCorporateDirector", item.IsCorporateDirector == null ? (object)DBNull.Value : item.IsCorporateDirector);
				sqlCommand.Parameters.AddWithValue("IsGlobalDirector", item.IsGlobalDirector == null ? (object)DBNull.Value : item.IsGlobalDirector);
				sqlCommand.Parameters.AddWithValue("Last_Edit_Date", item.LastEditDate == null ? (object)DBNull.Value : item.LastEditDate);
				sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
				sqlCommand.Parameters.AddWithValue("LegacyUsername", item.LegacyUsername == null ? (object)DBNull.Value : item.LegacyUsername);
				sqlCommand.Parameters.AddWithValue("LogisticsApp", item.LogisticsApp == null ? (object)DBNull.Value : item.LogisticsApp);
				sqlCommand.Parameters.AddWithValue("MasterDataApp", item.MasterDataApp == null ? (object)DBNull.Value : item.MasterDataApp);
				sqlCommand.Parameters.AddWithValue("MaterialManagementApp", item.MaterialManagementApp == null ? (object)DBNull.Value : item.MaterialManagementApp);
				sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? (object)DBNull.Value : item.Name);
				sqlCommand.Parameters.AddWithValue("Nummer", item.Nummer == null ? (object)DBNull.Value : item.Nummer);
				sqlCommand.Parameters.AddWithValue("Password", item.Password);
				sqlCommand.Parameters.AddWithValue("SalesDistributionApp", item.SalesDistributionApp == null ? (object)DBNull.Value : item.SalesDistributionApp);
				sqlCommand.Parameters.AddWithValue("SelectedLanguage", item.SelectedLanguage == null ? (object)DBNull.Value : item.SelectedLanguage);
				sqlCommand.Parameters.AddWithValue("SettingsApp", item.SettingsApp == null ? (object)DBNull.Value : item.SettingsApp);
				sqlCommand.Parameters.AddWithValue("SuperAdministrator", item.SuperAdministrator);
				sqlCommand.Parameters.AddWithValue("TelephoneHome", item.TelephoneHome == null ? (object)DBNull.Value : item.TelephoneHome);
				sqlCommand.Parameters.AddWithValue("TelephoneIP", item.TelephoneIP == null ? (object)DBNull.Value : item.TelephoneIP);
				sqlCommand.Parameters.AddWithValue("TelephoneMobile", item.TelephoneMobile == null ? (object)DBNull.Value : item.TelephoneMobile);
				sqlCommand.Parameters.AddWithValue("Username", item.Username == null ? (object)DBNull.Value : item.Username);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.COR.UserEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 39; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.COR.UserEntity> items)
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
						query += " UPDATE [User] SET "

							+ "[AccessProfileId]=@AccessProfileId" + i + ","
							+ "[CompanyId]=@CompanyId" + i + ","
							+ "[CountryId]=@CountryId" + i + ","
							+ "[Creation_User_Id]=@Creation_User_Id" + i + ","
							+ "[CreationTime]=@CreationTime" + i + ","
							+ "[CustomerServiceApp]=@CustomerServiceApp" + i + ","
							+ "[Delete_Date]=@Delete_Date" + i + ","
							+ "[Delete_User_Id]=@Delete_User_Id" + i + ","
							+ "[DepartmentId]=@DepartmentId" + i + ","
							+ "[Email]=@Email" + i + ","
							+ "[Fax]=@Fax" + i + ","
							+ "[FinanceControlApp]=@FinanceControlApp" + i + ","
							+ "[HumanResourcesApp]=@HumanResourcesApp" + i + ","
							+ "[Is_Archived]=@Is_Archived" + i + ","
							+ "[IsActivated]=@IsActivated" + i + ","
							+ "[IsAdministrator]=@IsAdministrator" + i + ","
							+ "[IsCorporateDirector]=@IsCorporateDirector" + i + ","
							+ "[IsGlobalDirector]=@IsGlobalDirector" + i + ","
							+ "[Last_Edit_Date]=@Last_Edit_Date" + i + ","
							+ "[Last_Edit_User_Id]=@Last_Edit_User_Id" + i + ","
							+ "[LegacyUsername]=@LegacyUsername" + i + ","
							+ "[LogisticsApp]=@LogisticsApp" + i + ","
							+ "[MasterDataApp]=@MasterDataApp" + i + ","
							+ "[MaterialManagementApp]=@MaterialManagementApp" + i + ","
							+ "[Name]=@Name" + i + ","
							+ "[Nummer]=@Nummer" + i + ","
							+ "[Password]=@Password" + i + ","
							+ "[SalesDistributionApp]=@SalesDistributionApp" + i + ","
							+ "[SelectedLanguage]=@SelectedLanguage" + i + ","
							+ "[SettingsApp]=@SettingsApp" + i + ","
							+ "[SuperAdministrator]=@SuperAdministrator" + i + ","
							+ "[TelephoneHome]=@TelephoneHome" + i + ","
							+ "[TelephoneIP]=@TelephoneIP" + i + ","
							+ "[TelephoneMobile]=@TelephoneMobile" + i + ","
							+ "[Username]=@Username" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("AccessProfileId" + i, item.AccessProfileId);
						sqlCommand.Parameters.AddWithValue("CompanyId" + i, item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
						sqlCommand.Parameters.AddWithValue("CountryId" + i, item.CountryId == null ? (object)DBNull.Value : item.CountryId);
						sqlCommand.Parameters.AddWithValue("Creation_User_Id" + i, item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CustomerServiceApp" + i, item.CustomerServiceApp == null ? (object)DBNull.Value : item.CustomerServiceApp);
						sqlCommand.Parameters.AddWithValue("Delete_Date" + i, item.DeleteDate == null ? (object)DBNull.Value : item.DeleteDate);
						sqlCommand.Parameters.AddWithValue("Delete_User_Id" + i, item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
						sqlCommand.Parameters.AddWithValue("DepartmentId" + i, item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
						sqlCommand.Parameters.AddWithValue("Email" + i, item.Email == null ? (object)DBNull.Value : item.Email);
						sqlCommand.Parameters.AddWithValue("Fax" + i, item.Fax == null ? (object)DBNull.Value : item.Fax);
						sqlCommand.Parameters.AddWithValue("FinanceControlApp" + i, item.FinanceControlApp == null ? (object)DBNull.Value : item.FinanceControlApp);
						sqlCommand.Parameters.AddWithValue("HumanResourcesApp" + i, item.HumanResourcesApp == null ? (object)DBNull.Value : item.HumanResourcesApp);
						sqlCommand.Parameters.AddWithValue("Is_Archived" + i, item.IsArchived);
						sqlCommand.Parameters.AddWithValue("IsActivated" + i, item.IsActivated);
						sqlCommand.Parameters.AddWithValue("IsAdministrator" + i, item.IsAdministrator == null ? (object)DBNull.Value : item.IsAdministrator);
						sqlCommand.Parameters.AddWithValue("IsCorporateDirector" + i, item.IsCorporateDirector == null ? (object)DBNull.Value : item.IsCorporateDirector);
						sqlCommand.Parameters.AddWithValue("IsGlobalDirector" + i, item.IsGlobalDirector == null ? (object)DBNull.Value : item.IsGlobalDirector);
						sqlCommand.Parameters.AddWithValue("Last_Edit_Date" + i, item.LastEditDate == null ? (object)DBNull.Value : item.LastEditDate);
						sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
						sqlCommand.Parameters.AddWithValue("LegacyUsername" + i, item.LegacyUsername == null ? (object)DBNull.Value : item.LegacyUsername);
						sqlCommand.Parameters.AddWithValue("LogisticsApp" + i, item.LogisticsApp == null ? (object)DBNull.Value : item.LogisticsApp);
						sqlCommand.Parameters.AddWithValue("MasterDataApp" + i, item.MasterDataApp == null ? (object)DBNull.Value : item.MasterDataApp);
						sqlCommand.Parameters.AddWithValue("MaterialManagementApp" + i, item.MaterialManagementApp == null ? (object)DBNull.Value : item.MaterialManagementApp);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name == null ? (object)DBNull.Value : item.Name);
						sqlCommand.Parameters.AddWithValue("Nummer" + i, item.Nummer == null ? (object)DBNull.Value : item.Nummer);
						sqlCommand.Parameters.AddWithValue("Password" + i, item.Password);
						sqlCommand.Parameters.AddWithValue("SalesDistributionApp" + i, item.SalesDistributionApp == null ? (object)DBNull.Value : item.SalesDistributionApp);
						sqlCommand.Parameters.AddWithValue("SelectedLanguage" + i, item.SelectedLanguage == null ? (object)DBNull.Value : item.SelectedLanguage);
						sqlCommand.Parameters.AddWithValue("SettingsApp" + i, item.SettingsApp == null ? (object)DBNull.Value : item.SettingsApp);
						sqlCommand.Parameters.AddWithValue("SuperAdministrator" + i, item.SuperAdministrator);
						sqlCommand.Parameters.AddWithValue("TelephoneHome" + i, item.TelephoneHome == null ? (object)DBNull.Value : item.TelephoneHome);
						sqlCommand.Parameters.AddWithValue("TelephoneIP" + i, item.TelephoneIP == null ? (object)DBNull.Value : item.TelephoneIP);
						sqlCommand.Parameters.AddWithValue("TelephoneMobile" + i, item.TelephoneMobile == null ? (object)DBNull.Value : item.TelephoneMobile);
						sqlCommand.Parameters.AddWithValue("Username" + i, item.Username == null ? (object)DBNull.Value : item.Username);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
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
				string query = "DELETE FROM [User] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

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

					string query = "DELETE FROM [User] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.COR.UserEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction, bool ignoreDeleted = true)
		{
			var dataTable = new DataTable();

			string query = $"SELECT * FROM [User] WHERE [Id]=@Id{(ignoreDeleted ? $" AND [Delete_Date] IS NULL" : "")}";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.COR.UserEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.COR.UserEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction, bool ignoreDeleted = true)
		{
			var dataTable = new DataTable();

			string query = $"SELECT * FROM [User]{(ignoreDeleted ? $" WHERE [Delete_Date] IS NULL" : "")}";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.COR.UserEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.COR.UserEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.COR.UserEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction, bool ignoreDeleted = true)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.COR.UserEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction, ignoreDeleted);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.COR.UserEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction, ignoreDeleted));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction, ignoreDeleted));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.COR.UserEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.COR.UserEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction, bool ignoreDeleted = true)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();

				var sqlCommand = new SqlCommand("", connection, transaction);
				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				sqlCommand.CommandText = $"SELECT * FROM [User] WHERE [Id] IN ({queryIds}){(ignoreDeleted ? $" AND [Delete_Date] IS NULL" : "")}";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.COR.UserEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.COR.UserEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.COR.UserEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.COR.UserEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [User] ([AccessProfileId],[CompanyId],[CountryId],[Creation_User_Id],[CreationTime],[CustomerServiceApp],[Delete_Date],[Delete_User_Id],[DepartmentId],[Email],[Fax],[FinanceControlApp],[HumanResourcesApp],[Is_Archived],[IsActivated],[IsAdministrator],[IsCorporateDirector],[IsGlobalDirector],[Last_Edit_Date],[Last_Edit_User_Id],[LegacyUsername],[LogisticsApp],[MasterDataApp],[MaterialManagementApp],[Name],[Nummer],[Password],[SalesDistributionApp],[SelectedLanguage],[SettingsApp],[SuperAdministrator],[TelephoneHome],[TelephoneIP],[TelephoneMobile],[Username]) OUTPUT INSERTED.[Id] VALUES (@AccessProfileId,@CompanyId,@CountryId,@Creation_User_Id,@CreationTime,@CustomerServiceApp,@Delete_Date,@Delete_User_Id,@DepartmentId,@Email,@Fax,@FinanceControlApp,@HumanResourcesApp,@Is_Archived,@IsActivated,@IsAdministrator,@IsCorporateDirector,@IsGlobalDirector,@Last_Edit_Date,@Last_Edit_User_Id,@LegacyUsername,@LogisticsApp,@MasterDataApp,@MaterialManagementApp,@Name,@Nummer,@Password,@SalesDistributionApp,@SelectedLanguage,@SettingsApp,@SuperAdministrator,@TelephoneHome,@TelephoneIP,@TelephoneMobile,@Username); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("AccessProfileId", item.AccessProfileId);
			sqlCommand.Parameters.AddWithValue("CompanyId", item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
			sqlCommand.Parameters.AddWithValue("CountryId", item.CountryId == null ? (object)DBNull.Value : item.CountryId);
			sqlCommand.Parameters.AddWithValue("Creation_User_Id", item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
			sqlCommand.Parameters.AddWithValue("CustomerServiceApp", item.CustomerServiceApp == null ? (object)DBNull.Value : item.CustomerServiceApp);
			sqlCommand.Parameters.AddWithValue("Delete_Date", item.DeleteDate == null ? (object)DBNull.Value : item.DeleteDate);
			sqlCommand.Parameters.AddWithValue("Delete_User_Id", item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
			sqlCommand.Parameters.AddWithValue("DepartmentId", item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
			sqlCommand.Parameters.AddWithValue("Email", item.Email == null ? (object)DBNull.Value : item.Email);
			sqlCommand.Parameters.AddWithValue("Fax", item.Fax == null ? (object)DBNull.Value : item.Fax);
			sqlCommand.Parameters.AddWithValue("FinanceControlApp", item.FinanceControlApp == null ? (object)DBNull.Value : item.FinanceControlApp);
			sqlCommand.Parameters.AddWithValue("HumanResourcesApp", item.HumanResourcesApp == null ? (object)DBNull.Value : item.HumanResourcesApp);
			sqlCommand.Parameters.AddWithValue("Is_Archived", item.IsArchived);
			sqlCommand.Parameters.AddWithValue("IsActivated", item.IsActivated);
			sqlCommand.Parameters.AddWithValue("IsAdministrator", item.IsAdministrator == null ? (object)DBNull.Value : item.IsAdministrator);
			sqlCommand.Parameters.AddWithValue("IsCorporateDirector", item.IsCorporateDirector == null ? (object)DBNull.Value : item.IsCorporateDirector);
			sqlCommand.Parameters.AddWithValue("IsGlobalDirector", item.IsGlobalDirector == null ? (object)DBNull.Value : item.IsGlobalDirector);
			sqlCommand.Parameters.AddWithValue("Last_Edit_Date", item.LastEditDate == null ? (object)DBNull.Value : item.LastEditDate);
			sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
			sqlCommand.Parameters.AddWithValue("LegacyUsername", item.LegacyUsername == null ? (object)DBNull.Value : item.LegacyUsername);
			sqlCommand.Parameters.AddWithValue("LogisticsApp", item.LogisticsApp == null ? (object)DBNull.Value : item.LogisticsApp);
			sqlCommand.Parameters.AddWithValue("MasterDataApp", item.MasterDataApp == null ? (object)DBNull.Value : item.MasterDataApp);
			sqlCommand.Parameters.AddWithValue("MaterialManagementApp", item.MaterialManagementApp == null ? (object)DBNull.Value : item.MaterialManagementApp);
			sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? (object)DBNull.Value : item.Name);
			sqlCommand.Parameters.AddWithValue("Nummer", item.Nummer == null ? (object)DBNull.Value : item.Nummer);
			sqlCommand.Parameters.AddWithValue("Password", item.Password);
			sqlCommand.Parameters.AddWithValue("SalesDistributionApp", item.SalesDistributionApp == null ? (object)DBNull.Value : item.SalesDistributionApp);
			sqlCommand.Parameters.AddWithValue("SelectedLanguage", item.SelectedLanguage == null ? (object)DBNull.Value : item.SelectedLanguage);
			sqlCommand.Parameters.AddWithValue("SettingsApp", item.SettingsApp == null ? (object)DBNull.Value : item.SettingsApp);
			sqlCommand.Parameters.AddWithValue("SuperAdministrator", item.SuperAdministrator);
			sqlCommand.Parameters.AddWithValue("TelephoneHome", item.TelephoneHome == null ? (object)DBNull.Value : item.TelephoneHome);
			sqlCommand.Parameters.AddWithValue("TelephoneIP", item.TelephoneIP == null ? (object)DBNull.Value : item.TelephoneIP);
			sqlCommand.Parameters.AddWithValue("TelephoneMobile", item.TelephoneMobile == null ? (object)DBNull.Value : item.TelephoneMobile);
			sqlCommand.Parameters.AddWithValue("Username", item.Username == null ? (object)DBNull.Value : item.Username);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.COR.UserEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 39; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = insertWithTransaction(items, connection, transaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += insertWithTransaction(items.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += insertWithTransaction(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber), connection, transaction);
				}
				return results;
			}

			return -1;
		}
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.COR.UserEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [User] ([AccessProfileId],[CompanyId],[CountryId],[Creation_User_Id],[CreationTime],[CustomerServiceApp],[Delete_Date],[Delete_User_Id],[DepartmentId],[Email],[Fax],[FinanceControlApp],[HumanResourcesApp],[Is_Archived],[IsActivated],[IsAdministrator],[IsCorporateDirector],[IsGlobalDirector],[Last_Edit_Date],[Last_Edit_User_Id],[LegacyUsername],[LogisticsApp],[MasterDataApp],[MaterialManagementApp],[Name],[Nummer],[Password],[SalesDistributionApp],[SelectedLanguage],[SettingsApp],[SuperAdministrator],[TelephoneHome],[TelephoneIP],[TelephoneMobile],[Username]) VALUES ( "

						+ "@AccessProfileId" + i + ","
						+ "@CompanyId" + i + ","
						+ "@CountryId" + i + ","
						+ "@Creation_User_Id" + i + ","
						+ "@CreationTime" + i + ","
						+ "@CustomerServiceApp" + i + ","
						+ "@Delete_Date" + i + ","
						+ "@Delete_User_Id" + i + ","
						+ "@DepartmentId" + i + ","
						+ "@Email" + i + ","
						+ "@Fax" + i + ","
						+ "@FinanceControlApp" + i + ","
						+ "@HumanResourcesApp" + i + ","
						+ "@Is_Archived" + i + ","
						+ "@IsActivated" + i + ","
						+ "@IsAdministrator" + i + ","
						+ "@IsCorporateDirector" + i + ","
						+ "@IsGlobalDirector" + i + ","
						+ "@Last_Edit_Date" + i + ","
						+ "@Last_Edit_User_Id" + i + ","
						+ "@LegacyUsername" + i + ","
						+ "@LogisticsApp" + i + ","
						+ "@MasterDataApp" + i + ","
						+ "@MaterialManagementApp" + i + ","
						+ "@Name" + i + ","
						+ "@Nummer" + i + ","
						+ "@Password" + i + ","
						+ "@SalesDistributionApp" + i + ","
						+ "@SelectedLanguage" + i + ","
						+ "@SettingsApp" + i + ","
						+ "@SuperAdministrator" + i + ","
						+ "@TelephoneHome" + i + ","
						+ "@TelephoneIP" + i + ","
						+ "@TelephoneMobile" + i + ","
						+ "@Username" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("AccessProfileId" + i, item.AccessProfileId);
					sqlCommand.Parameters.AddWithValue("CompanyId" + i, item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
					sqlCommand.Parameters.AddWithValue("CountryId" + i, item.CountryId == null ? (object)DBNull.Value : item.CountryId);
					sqlCommand.Parameters.AddWithValue("Creation_User_Id" + i, item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CustomerServiceApp" + i, item.CustomerServiceApp == null ? (object)DBNull.Value : item.CustomerServiceApp);
					sqlCommand.Parameters.AddWithValue("Delete_Date" + i, item.DeleteDate == null ? (object)DBNull.Value : item.DeleteDate);
					sqlCommand.Parameters.AddWithValue("Delete_User_Id" + i, item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
					sqlCommand.Parameters.AddWithValue("DepartmentId" + i, item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
					sqlCommand.Parameters.AddWithValue("Email" + i, item.Email == null ? (object)DBNull.Value : item.Email);
					sqlCommand.Parameters.AddWithValue("Fax" + i, item.Fax == null ? (object)DBNull.Value : item.Fax);
					sqlCommand.Parameters.AddWithValue("FinanceControlApp" + i, item.FinanceControlApp == null ? (object)DBNull.Value : item.FinanceControlApp);
					sqlCommand.Parameters.AddWithValue("HumanResourcesApp" + i, item.HumanResourcesApp == null ? (object)DBNull.Value : item.HumanResourcesApp);
					sqlCommand.Parameters.AddWithValue("Is_Archived" + i, item.IsArchived);
					sqlCommand.Parameters.AddWithValue("IsActivated" + i, item.IsActivated);
					sqlCommand.Parameters.AddWithValue("IsAdministrator" + i, item.IsAdministrator == null ? (object)DBNull.Value : item.IsAdministrator);
					sqlCommand.Parameters.AddWithValue("IsCorporateDirector" + i, item.IsCorporateDirector == null ? (object)DBNull.Value : item.IsCorporateDirector);
					sqlCommand.Parameters.AddWithValue("IsGlobalDirector" + i, item.IsGlobalDirector == null ? (object)DBNull.Value : item.IsGlobalDirector);
					sqlCommand.Parameters.AddWithValue("Last_Edit_Date" + i, item.LastEditDate == null ? (object)DBNull.Value : item.LastEditDate);
					sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("LegacyUsername" + i, item.LegacyUsername == null ? (object)DBNull.Value : item.LegacyUsername);
					sqlCommand.Parameters.AddWithValue("LogisticsApp" + i, item.LogisticsApp == null ? (object)DBNull.Value : item.LogisticsApp);
					sqlCommand.Parameters.AddWithValue("MasterDataApp" + i, item.MasterDataApp == null ? (object)DBNull.Value : item.MasterDataApp);
					sqlCommand.Parameters.AddWithValue("MaterialManagementApp" + i, item.MaterialManagementApp == null ? (object)DBNull.Value : item.MaterialManagementApp);
					sqlCommand.Parameters.AddWithValue("Name" + i, item.Name == null ? (object)DBNull.Value : item.Name);
					sqlCommand.Parameters.AddWithValue("Nummer" + i, item.Nummer == null ? (object)DBNull.Value : item.Nummer);
					sqlCommand.Parameters.AddWithValue("Password" + i, item.Password);
					sqlCommand.Parameters.AddWithValue("SalesDistributionApp" + i, item.SalesDistributionApp == null ? (object)DBNull.Value : item.SalesDistributionApp);
					sqlCommand.Parameters.AddWithValue("SelectedLanguage" + i, item.SelectedLanguage == null ? (object)DBNull.Value : item.SelectedLanguage);
					sqlCommand.Parameters.AddWithValue("SettingsApp" + i, item.SettingsApp == null ? (object)DBNull.Value : item.SettingsApp);
					sqlCommand.Parameters.AddWithValue("SuperAdministrator" + i, item.SuperAdministrator);
					sqlCommand.Parameters.AddWithValue("TelephoneHome" + i, item.TelephoneHome == null ? (object)DBNull.Value : item.TelephoneHome);
					sqlCommand.Parameters.AddWithValue("TelephoneIP" + i, item.TelephoneIP == null ? (object)DBNull.Value : item.TelephoneIP);
					sqlCommand.Parameters.AddWithValue("TelephoneMobile" + i, item.TelephoneMobile == null ? (object)DBNull.Value : item.TelephoneMobile);
					sqlCommand.Parameters.AddWithValue("Username" + i, item.Username == null ? (object)DBNull.Value : item.Username);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.COR.UserEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [User] SET [AccessProfileId]=@AccessProfileId, [CompanyId]=@CompanyId, [CountryId]=@CountryId, [Creation_User_Id]=@Creation_User_Id, [CreationTime]=@CreationTime, [CustomerServiceApp]=@CustomerServiceApp, [Delete_Date]=@Delete_Date, [Delete_User_Id]=@Delete_User_Id, [DepartmentId]=@DepartmentId, [Email]=@Email, [Fax]=@Fax, [FinanceControlApp]=@FinanceControlApp, [HumanResourcesApp]=@HumanResourcesApp, [Is_Archived]=@Is_Archived, [IsActivated]=@IsActivated, [IsAdministrator]=@IsAdministrator, [IsCorporateDirector]=@IsCorporateDirector, [IsGlobalDirector]=@IsGlobalDirector, [Last_Edit_Date]=@Last_Edit_Date, [Last_Edit_User_Id]=@Last_Edit_User_Id, [LegacyUsername]=@LegacyUsername, [LogisticsApp]=@LogisticsApp, [MasterDataApp]=@MasterDataApp, [MaterialManagementApp]=@MaterialManagementApp, [Name]=@Name, [Nummer]=@Nummer, [Password]=@Password, [SalesDistributionApp]=@SalesDistributionApp, [SelectedLanguage]=@SelectedLanguage, [SettingsApp]=@SettingsApp, [SuperAdministrator]=@SuperAdministrator, [TelephoneHome]=@TelephoneHome, [TelephoneIP]=@TelephoneIP, [TelephoneMobile]=@TelephoneMobile, [Username]=@Username WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("AccessProfileId", item.AccessProfileId);
			sqlCommand.Parameters.AddWithValue("CompanyId", item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
			sqlCommand.Parameters.AddWithValue("CountryId", item.CountryId == null ? (object)DBNull.Value : item.CountryId);
			sqlCommand.Parameters.AddWithValue("Creation_User_Id", item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
			sqlCommand.Parameters.AddWithValue("CustomerServiceApp", item.CustomerServiceApp == null ? (object)DBNull.Value : item.CustomerServiceApp);
			sqlCommand.Parameters.AddWithValue("Delete_Date", item.DeleteDate == null ? (object)DBNull.Value : item.DeleteDate);
			sqlCommand.Parameters.AddWithValue("Delete_User_Id", item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
			sqlCommand.Parameters.AddWithValue("DepartmentId", item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
			sqlCommand.Parameters.AddWithValue("Email", item.Email == null ? (object)DBNull.Value : item.Email);
			sqlCommand.Parameters.AddWithValue("Fax", item.Fax == null ? (object)DBNull.Value : item.Fax);
			sqlCommand.Parameters.AddWithValue("FinanceControlApp", item.FinanceControlApp == null ? (object)DBNull.Value : item.FinanceControlApp);
			sqlCommand.Parameters.AddWithValue("HumanResourcesApp", item.HumanResourcesApp == null ? (object)DBNull.Value : item.HumanResourcesApp);
			sqlCommand.Parameters.AddWithValue("Is_Archived", item.IsArchived);
			sqlCommand.Parameters.AddWithValue("IsActivated", item.IsActivated);
			sqlCommand.Parameters.AddWithValue("IsAdministrator", item.IsAdministrator == null ? (object)DBNull.Value : item.IsAdministrator);
			sqlCommand.Parameters.AddWithValue("IsCorporateDirector", item.IsCorporateDirector == null ? (object)DBNull.Value : item.IsCorporateDirector);
			sqlCommand.Parameters.AddWithValue("IsGlobalDirector", item.IsGlobalDirector == null ? (object)DBNull.Value : item.IsGlobalDirector);
			sqlCommand.Parameters.AddWithValue("Last_Edit_Date", item.LastEditDate == null ? (object)DBNull.Value : item.LastEditDate);
			sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
			sqlCommand.Parameters.AddWithValue("LegacyUsername", item.LegacyUsername == null ? (object)DBNull.Value : item.LegacyUsername);
			sqlCommand.Parameters.AddWithValue("LogisticsApp", item.LogisticsApp == null ? (object)DBNull.Value : item.LogisticsApp);
			sqlCommand.Parameters.AddWithValue("MasterDataApp", item.MasterDataApp == null ? (object)DBNull.Value : item.MasterDataApp);
			sqlCommand.Parameters.AddWithValue("MaterialManagementApp", item.MaterialManagementApp == null ? (object)DBNull.Value : item.MaterialManagementApp);
			sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? (object)DBNull.Value : item.Name);
			sqlCommand.Parameters.AddWithValue("Nummer", item.Nummer == null ? (object)DBNull.Value : item.Nummer);
			sqlCommand.Parameters.AddWithValue("Password", item.Password);
			sqlCommand.Parameters.AddWithValue("SalesDistributionApp", item.SalesDistributionApp == null ? (object)DBNull.Value : item.SalesDistributionApp);
			sqlCommand.Parameters.AddWithValue("SelectedLanguage", item.SelectedLanguage == null ? (object)DBNull.Value : item.SelectedLanguage);
			sqlCommand.Parameters.AddWithValue("SettingsApp", item.SettingsApp == null ? (object)DBNull.Value : item.SettingsApp);
			sqlCommand.Parameters.AddWithValue("SuperAdministrator", item.SuperAdministrator);
			sqlCommand.Parameters.AddWithValue("TelephoneHome", item.TelephoneHome == null ? (object)DBNull.Value : item.TelephoneHome);
			sqlCommand.Parameters.AddWithValue("TelephoneIP", item.TelephoneIP == null ? (object)DBNull.Value : item.TelephoneIP);
			sqlCommand.Parameters.AddWithValue("TelephoneMobile", item.TelephoneMobile == null ? (object)DBNull.Value : item.TelephoneMobile);
			sqlCommand.Parameters.AddWithValue("Username", item.Username == null ? (object)DBNull.Value : item.Username);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.COR.UserEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 39; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = updateWithTransaction(items, connection, transaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += updateWithTransaction(items.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += updateWithTransaction(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber), connection, transaction);
				}

				return results;
			}

			return -1;
		}
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.COR.UserEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " UPDATE [User] SET "

					+ "[AccessProfileId]=@AccessProfileId" + i + ","
					+ "[CompanyId]=@CompanyId" + i + ","
					+ "[CountryId]=@CountryId" + i + ","
					+ "[Creation_User_Id]=@Creation_User_Id" + i + ","
					+ "[CreationTime]=@CreationTime" + i + ","
					+ "[CustomerServiceApp]=@CustomerServiceApp" + i + ","
					+ "[Delete_Date]=@Delete_Date" + i + ","
					+ "[Delete_User_Id]=@Delete_User_Id" + i + ","
					+ "[DepartmentId]=@DepartmentId" + i + ","
					+ "[Email]=@Email" + i + ","
					+ "[Fax]=@Fax" + i + ","
					+ "[FinanceControlApp]=@FinanceControlApp" + i + ","
					+ "[HumanResourcesApp]=@HumanResourcesApp" + i + ","
					+ "[Is_Archived]=@Is_Archived" + i + ","
					+ "[IsActivated]=@IsActivated" + i + ","
					+ "[IsAdministrator]=@IsAdministrator" + i + ","
					+ "[IsCorporateDirector]=@IsCorporateDirector" + i + ","
					+ "[IsGlobalDirector]=@IsGlobalDirector" + i + ","
					+ "[Last_Edit_Date]=@Last_Edit_Date" + i + ","
					+ "[Last_Edit_User_Id]=@Last_Edit_User_Id" + i + ","
					+ "[LegacyUsername]=@LegacyUsername" + i + ","
					+ "[LogisticsApp]=@LogisticsApp" + i + ","
					+ "[MasterDataApp]=@MasterDataApp" + i + ","
					+ "[MaterialManagementApp]=@MaterialManagementApp" + i + ","
					+ "[Name]=@Name" + i + ","
					+ "[Nummer]=@Nummer" + i + ","
					+ "[Password]=@Password" + i + ","
					+ "[SalesDistributionApp]=@SalesDistributionApp" + i + ","
					+ "[SelectedLanguage]=@SelectedLanguage" + i + ","
					+ "[SettingsApp]=@SettingsApp" + i + ","
					+ "[SuperAdministrator]=@SuperAdministrator" + i + ","
					+ "[TelephoneHome]=@TelephoneHome" + i + ","
					+ "[TelephoneIP]=@TelephoneIP" + i + ","
					+ "[TelephoneMobile]=@TelephoneMobile" + i + ","
					+ "[Username]=@Username" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("AccessProfileId" + i, item.AccessProfileId);
					sqlCommand.Parameters.AddWithValue("CompanyId" + i, item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
					sqlCommand.Parameters.AddWithValue("CountryId" + i, item.CountryId == null ? (object)DBNull.Value : item.CountryId);
					sqlCommand.Parameters.AddWithValue("Creation_User_Id" + i, item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CustomerServiceApp" + i, item.CustomerServiceApp == null ? (object)DBNull.Value : item.CustomerServiceApp);
					sqlCommand.Parameters.AddWithValue("Delete_Date" + i, item.DeleteDate == null ? (object)DBNull.Value : item.DeleteDate);
					sqlCommand.Parameters.AddWithValue("Delete_User_Id" + i, item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
					sqlCommand.Parameters.AddWithValue("DepartmentId" + i, item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
					sqlCommand.Parameters.AddWithValue("Email" + i, item.Email == null ? (object)DBNull.Value : item.Email);
					sqlCommand.Parameters.AddWithValue("Fax" + i, item.Fax == null ? (object)DBNull.Value : item.Fax);
					sqlCommand.Parameters.AddWithValue("FinanceControlApp" + i, item.FinanceControlApp == null ? (object)DBNull.Value : item.FinanceControlApp);
					sqlCommand.Parameters.AddWithValue("HumanResourcesApp" + i, item.HumanResourcesApp == null ? (object)DBNull.Value : item.HumanResourcesApp);
					sqlCommand.Parameters.AddWithValue("Is_Archived" + i, item.IsArchived);
					sqlCommand.Parameters.AddWithValue("IsActivated" + i, item.IsActivated);
					sqlCommand.Parameters.AddWithValue("IsAdministrator" + i, item.IsAdministrator == null ? (object)DBNull.Value : item.IsAdministrator);
					sqlCommand.Parameters.AddWithValue("IsCorporateDirector" + i, item.IsCorporateDirector == null ? (object)DBNull.Value : item.IsCorporateDirector);
					sqlCommand.Parameters.AddWithValue("IsGlobalDirector" + i, item.IsGlobalDirector == null ? (object)DBNull.Value : item.IsGlobalDirector);
					sqlCommand.Parameters.AddWithValue("Last_Edit_Date" + i, item.LastEditDate == null ? (object)DBNull.Value : item.LastEditDate);
					sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("LegacyUsername" + i, item.LegacyUsername == null ? (object)DBNull.Value : item.LegacyUsername);
					sqlCommand.Parameters.AddWithValue("LogisticsApp" + i, item.LogisticsApp == null ? (object)DBNull.Value : item.LogisticsApp);
					sqlCommand.Parameters.AddWithValue("MasterDataApp" + i, item.MasterDataApp == null ? (object)DBNull.Value : item.MasterDataApp);
					sqlCommand.Parameters.AddWithValue("MaterialManagementApp" + i, item.MaterialManagementApp == null ? (object)DBNull.Value : item.MaterialManagementApp);
					sqlCommand.Parameters.AddWithValue("Name" + i, item.Name == null ? (object)DBNull.Value : item.Name);
					sqlCommand.Parameters.AddWithValue("Nummer" + i, item.Nummer == null ? (object)DBNull.Value : item.Nummer);
					sqlCommand.Parameters.AddWithValue("Password" + i, item.Password);
					sqlCommand.Parameters.AddWithValue("SalesDistributionApp" + i, item.SalesDistributionApp == null ? (object)DBNull.Value : item.SalesDistributionApp);
					sqlCommand.Parameters.AddWithValue("SelectedLanguage" + i, item.SelectedLanguage == null ? (object)DBNull.Value : item.SelectedLanguage);
					sqlCommand.Parameters.AddWithValue("SettingsApp" + i, item.SettingsApp == null ? (object)DBNull.Value : item.SettingsApp);
					sqlCommand.Parameters.AddWithValue("SuperAdministrator" + i, item.SuperAdministrator);
					sqlCommand.Parameters.AddWithValue("TelephoneHome" + i, item.TelephoneHome == null ? (object)DBNull.Value : item.TelephoneHome);
					sqlCommand.Parameters.AddWithValue("TelephoneIP" + i, item.TelephoneIP == null ? (object)DBNull.Value : item.TelephoneIP);
					sqlCommand.Parameters.AddWithValue("TelephoneMobile" + i, item.TelephoneMobile == null ? (object)DBNull.Value : item.TelephoneMobile);
					sqlCommand.Parameters.AddWithValue("Username" + i, item.Username == null ? (object)DBNull.Value : item.Username);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [User] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			results = DbExecution.ExecuteNonQuery(sqlCommand);


			return results;
		}
		public static int DeleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE;
				int results = 0;
				if(ids.Count <= maxParamsNumber)
				{
					results = deleteWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += deleteWithTransaction(ids.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += deleteWithTransaction(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber), connection, transaction);
				}
			}
			return -1;
		}
		private static int deleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int results = -1;

				var sqlCommand = new SqlCommand("", connection, transaction);

				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				string query = "DELETE FROM [User] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion


		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.COR.UserEntity> GetLegacyUsernameNotNull(bool ignoreDeleted = true)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [User] WHERE LegacyUserName <> '' AND LegacyUserName IS NOT NULL{(ignoreDeleted ? $" AND [Delete_Date] IS NULL" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.COR.UserEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.COR.UserEntity>();
			}
		}
		public static int UpdateLegacy(Infrastructure.Data.Entities.Tables.COR.UserEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [User] SET  [Nummer]=@Nummer, [LegacyUsername]=@LegacyUsername WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Nummer", item.Nummer);
				sqlCommand.Parameters.AddWithValue("LegacyUsername", item.LegacyUsername);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int GetMaxNummer(List<int> specialUserNummers)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $" Select Max(Nummer) from [User] {(specialUserNummers?.Count > 0 ? $"WHERE [Nummer] NOT IN ({string.Join(",", specialUserNummers)})" : "")}";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				var result = DbExecution.ExecuteScalar(sqlCommand);
				response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}

			return response;
		}
		public static bool CheckExists(string username, bool ignoreDeleted = true)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT * FROM [User] WHERE [Username]=@userName AND ISNULL(Is_Archived,0)=0{(ignoreDeleted ? $" AND [Delete_Date] IS NULL" : "")}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("userName", username);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		public static Data.Entities.Tables.COR.UserEntity GetByUsername(string username, bool ignoreDeleted = true)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT * FROM [User] WHERE [Username]=@username AND ISNULL(Is_Archived,0)=@IsArchived{(ignoreDeleted ? $" AND [Delete_Date] IS NULL" : "")}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("username", username);
				sqlCommand.Parameters.AddWithValue("IsArchived", false);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Data.Entities.Tables.COR.UserEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.COR.UserEntity> GetByUsernames(List<string> userNames)
		{
			SqlDataAdapter SelectAdapter = new SqlDataAdapter();
			DataTable dt = new DataTable();
			using(SqlConnection sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [User] WHERE [Username] IN ({string.Join(",", userNames.Select(x => $"'{x}'").ToList())}) AND Is_Archived=0";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				SelectAdapter = new SqlDataAdapter(sqlCommand);
				SelectAdapter.Fill(dt);
			}

			if(dt.Rows.Count > 0)
			{
				return dt.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.COR.UserEntity(x)).ToList();
			}
			else
			{
				return new List<Data.Entities.Tables.COR.UserEntity>();

			}
		}
		public static Data.Entities.Tables.COR.UserEntity GetByName(string username, bool ignoreDeleted = true)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT * FROM [User] WHERE [Name]=@username AND ISNULL(Is_Archived,0)=@IsArchived{(ignoreDeleted ? $" AND [Delete_Date] IS NULL" : "")}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("username", username);
				sqlCommand.Parameters.AddWithValue("IsArchived", false);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Data.Entities.Tables.COR.UserEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Data.Entities.Tables.COR.UserEntity GetByNummer(int Nummer, bool ignoreDeleted = true)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT * FROM [User] WHERE Nummer=@Nummer{(ignoreDeleted ? $" AND [Delete_Date] IS NULL" : "")}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Nummer", Nummer);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Data.Entities.Tables.COR.UserEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.COR.UserEntity> GetLikeName(string searchText, bool ignoreDeleted = true)
		{
			var sqlConnection = new SqlConnection(Settings.ConnectionString);
			sqlConnection.Open();

			string query = $"SELECT * FROM [User] WHERE [Name] LIKE @searchText OR [Username] LIKE @searchText OR [Email] LIKE @searchText{(ignoreDeleted ? $" AND [Delete_Date] IS NULL" : "")}";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("searchText", "%" + searchText.SqlEscape() + "%");

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.COR.UserEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.COR.UserEntity>();
			}
		}

		public static List<Entities.Tables.COR.UserEntity> GetLikeNameLegacyNotNull(string searchText, bool ignoreDeleted = true)
		{
			var sqlConnection = new SqlConnection(Settings.ConnectionString);
			sqlConnection.Open();

			string query = $"SELECT * FROM [User] WHERE ([Name] LIKE @searchText OR [Username] LIKE @searchText OR [LegacyUserName] LIKE @searchText) AND legacyUsername is not null AND legacyUsername <> ''{(ignoreDeleted ? $" AND [Delete_Date] IS NULL" : "")}";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("searchText", "%" + searchText + "%");

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.COR.UserEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.COR.UserEntity>();
			}
		}
		public static List<Data.Entities.Tables.COR.UserEntity> GetByAccessProfilesIds(List<int> accessProfilesIds, bool includeArchived = false, bool ignoreDeleted = true)
		{
			if(accessProfilesIds != null && accessProfilesIds.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				var result = new List<Data.Entities.Tables.COR.UserEntity>();
				if(accessProfilesIds.Count <= maxQueryNumber)
				{
					result = getByAccessProfilesIds(accessProfilesIds, includeArchived, ignoreDeleted);
				}
				else
				{
					int batchNumber = accessProfilesIds.Count / maxQueryNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						result.AddRange(getByAccessProfilesIds(accessProfilesIds.GetRange(i * maxQueryNumber, maxQueryNumber), includeArchived, ignoreDeleted));
					}
					result.AddRange(getByAccessProfilesIds(accessProfilesIds.GetRange(batchNumber * maxQueryNumber, accessProfilesIds.Count - batchNumber * maxQueryNumber), includeArchived, ignoreDeleted));
				}
				return result;
			}
			return new List<Data.Entities.Tables.COR.UserEntity>();
		}
		private static List<Data.Entities.Tables.COR.UserEntity> getByAccessProfilesIds(List<int> accessProfilesIds, bool includeArchived, bool ignoreDeleted = true)
		{
			if(accessProfilesIds != null && accessProfilesIds.Count > 0)
			{
				var dataTable = new DataTable();

				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					var sqlCommand = new SqlCommand
					{
						Connection = sqlConnection
					};

					string queryIds = string.Empty;
					for(int i = 0; i < accessProfilesIds.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, accessProfilesIds[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = $"SELECT * FROM [User] WHERE AccessProfileId IN ({queryIds} ) {(ignoreDeleted ? $" AND [Delete_Date] IS NULL" : "")}"
						 + (!includeArchived ? " AND ISNULL(Is_Archived,0)=0 " : "");

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.COR.UserEntity(x)).ToList();
				}
				else
				{
					return new List<Data.Entities.Tables.COR.UserEntity>();
				}
			}
			return new List<Data.Entities.Tables.COR.UserEntity>();
		}
		public static int UpdateLanguage(Data.Entities.Tables.COR.UserEntity element)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = " UPDATE [User] SET [SelectedLanguage]=@SelectedLanguage "
					+ " WHERE [Id]=@Id ";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", element.Id);
				sqlCommand.Parameters.AddWithValue("SelectedLanguage", element.SelectedLanguage);

				response = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return response;
		}
		public static int UpdateAccessProfile(int userId, int accessProfileId)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = " UPDATE [User] SET [AccessProfileId]=@accessProfileId "
					+ " WHERE [Id]=@Id ";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", userId);
				sqlCommand.Parameters.AddWithValue("accessProfileId", accessProfileId);

				response = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return response;
		}
		public static int UpdateEmail(Data.Entities.Tables.COR.UserEntity element)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = " UPDATE [User] SET  "
					+ " [Email]=@Email, [Last_Edit_User_Id]=@LastEditUserId,[Last_Edit_Date]=@LastEditTime "
					+ " WHERE [Id]=@Id ";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", element.Id);
				sqlCommand.Parameters.AddWithValue("LastEditTime", DateTime.Now);
				sqlCommand.Parameters.AddWithValue("LastEditUserId", element.LastEditUserId ?? (object)DBNull.Value);
				sqlCommand.Parameters.AddWithValue("Email", element.Email ?? (object)DBNull.Value);

				response = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return response;
		}
		public static List<Entities.Tables.COR.UserEntity> GetByDepartmentIds(List<int> departmentIds, bool ignoreDeleted = true)
		{
			if(departmentIds == null || departmentIds.Count <= 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.COR.UserEntity>();
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"select * from [User] where DepartmentId IN ({string.Join(",", departmentIds)}) AND ISNULL(Is_Archived,0)=0 AND IsActivated=1{(ignoreDeleted ? $" AND [Delete_Date] IS NULL" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.COR.UserEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.COR.UserEntity>();
			}
		}
		public static List<Entities.Tables.COR.UserEntity> GetByCompanyId(int companyId, bool ignoreDeleted = true)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"select * from [User] where CompanyId=@companyId AND ISNULL(Is_Archived,0)=0 AND IsActivated=1{(ignoreDeleted ? $" AND [Delete_Date] IS NULL" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("companyId", companyId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.COR.UserEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.COR.UserEntity>();
			}
		}
		public static List<Entities.Tables.COR.UserEntity> GetByCompanyIds(List<int> companyIds, bool ignoreDeleted = true)
		{
			if(companyIds == null && companyIds.Count <= 0)
				return null;
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"select * from [User] where CompanyId in ({string.Join(",", companyIds)}) AND ISNULL(Is_Archived,0)=0 AND IsActivated=1{(ignoreDeleted ? $" AND [Delete_Date] IS NULL" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.COR.UserEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.COR.UserEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.COR.UserEntity> GetGlobalDirectors(bool ignoreDeleted = true)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [User] WHERE IsGlobalDirector=1 AND IsActivated=1 AND ISNULL(Is_Archived,0)=0{(ignoreDeleted ? $" AND [Delete_Date] IS NULL" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.COR.UserEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.COR.UserEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.COR.UserEntity> GetSuperAdmins(bool ignoreDeleted = true)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [User] WHERE SuperAdministrator=1 AND IsActivated=1 AND ISNULL(Is_Archived,0)=0{(ignoreDeleted ? $" AND [Delete_Date] IS NULL" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.COR.UserEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.COR.UserEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.COR.UserEntity> GetActive(string name = "", bool ignoreDeleted = true)
		{
			name = (name ?? "").SqlEscape();
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [User] WHERE IsActivated=1 AND ISNULL(Is_Archived,0)=0 {(!string.IsNullOrWhiteSpace(name) ? $"AND ([Name] LIKE '%{name}%' OR [Username] LIKE '%{name}%' OR [Email] LIKE '%{name}%')" : "")} {(ignoreDeleted ? $" AND [Delete_Date] IS NULL" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.COR.UserEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.COR.UserEntity>();
			}
		}
		public static int SetLastConnect(int id)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "UPDATE [User] SET [LastConnectTime]=@date, [LastConnectErrorTime]=NULL, [LastConnectErrorCount]=0 WHERE [Id]=@Id ";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", id);
				sqlCommand.Parameters.AddWithValue("date", DateTime.Now);

				response = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return response;
		}
		public static int SetLastConnectError(string username)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "UPDATE [User] SET [LastConnectErrorTime]=@date, [LastConnectErrorCount]=ISNULL([LastConnectErrorCount],0)+1 WHERE [Username]=@username ";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("username", username ?? "");
				sqlCommand.Parameters.AddWithValue("date", DateTime.Now);

				response = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return response;
		}
		public static Infrastructure.Data.Entities.Tables.COR.UserEntity GetByMail(string mail, bool ignoreDeleted = true)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [User] WHERE [Email]=@mail{(ignoreDeleted ? $" AND [Delete_Date] IS NULL" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("mail", mail);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.COR.UserEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.COR.UserEntity> GetByMails(List<string> emailAddresses, bool ignoreDeleted = true)
		{
			if(emailAddresses == null || emailAddresses.Count <= 0)
				return new List<Infrastructure.Data.Entities.Tables.COR.UserEntity>();

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [User] WHERE [Email] IN ('{string.Join("','", emailAddresses)}'){(ignoreDeleted ? $" AND [Delete_Date] IS NULL" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.COR.UserEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.COR.UserEntity>();
			}
		}

		public static int GetLagerUser(int companyId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "select * from _lagerCompany where Company_id=@companyId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("companyId", companyId);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				string vale = dataTable.Rows[0]["Lagerort_id"].ToString();
				if(vale == "")
				{
					return 0;
				}
				return Convert.ToInt32(vale);
			}
			else
			{
				return 0;
			}
		}

		public static List<Entities.Tables.COR.UserEntity> GetUnarchivedUsesList(string filter, bool ignoreDeleted = true)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"Select * from [User]{(ignoreDeleted ? $" WHERE [Delete_Date] IS NULL" : "")}";
				var clauses = new List<string>();


				clauses.Add($" Is_Archived=0");

				if(!String.IsNullOrWhiteSpace(filter))
				{
					clauses.Add($" Name like '{filter.ToLower()}%' ");
				}
				if(clauses.Count > 0)
				{
					query += $" AND {string.Join(" AND ", clauses)}";
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.COR.UserEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.COR.UserEntity>();
			}
		}
		public static int UpdateAccessProfileId(int userId, int accessProfileId, int editUserId, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [User] SET [AccessProfileId]=@AccessProfileId, [Last_Edit_Date]=GETDATE(), [Last_Edit_User_Id]=@Last_Edit_User_Id WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", userId);
			sqlCommand.Parameters.AddWithValue("AccessProfileId", accessProfileId);
			sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id", editUserId);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}

		public static List<Entities.Tables.COR.UserEntity> GetBySite(List<int> usersIds, int lagerortId, bool ignoreDeleted = true)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				string ids = string.Join(",", usersIds);

				sqlConnection.Open();
				string query = $@"Select * from [User] u  left join [__LGT_Werke] w on u.CompanyId = w.IdCompany
					left join [Lagerorte] l on l.WerkVonId = w.Id
					where l.Lagerort_id = {lagerortId} and u.Id in ({ids}){(ignoreDeleted ? $" AND u.[Delete_Date] IS NULL" : "")}";
				var clauses = new List<string>();

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.COR.UserEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.COR.UserEntity>();
			}
		}

		public static List<Entities.Tables.COR.UserEntity> GetByUsersAndLagerorts(List<int> usersIds, List<int> lagerortIds)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				string ids = string.Join(",", usersIds);

				sqlConnection.Open();
				string query = $@"Select * from [User] u  left join [__LGT_Werke] w on u.CompanyId = w.IdCompany
					left join [Lagerorte] l on l.WerkVonId = w.Id
					where l.Lagerort_id in ({string.Join(",", lagerortIds)}) and u.Id in ({ids})";
				var clauses = new List<string>();

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.COR.UserEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.COR.UserEntity>();
			}
		}

		#endregion
	}
}
