using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class UserAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.UserEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [User] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.UserEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.UserEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [User]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.UserEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.UserEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.UserEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.UserEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.UserEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.UserEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.UserEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [User] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.UserEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.UserEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.UserEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.UserEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [User] ([AccessProfileId],[CompanyId],[CountryId],[Creation_User_Id],[CreationTime],[CustomerServiceApp],[Delete_Date],[Delete_User_Id],[DepartmentId],[Email],[Fax],[FinanceControlApp],[HumanResourcesApp],[Is_Archived],[IsActivated],[IsGlobalDirector],[Last_Edit_Date],[Last_Edit_User_Id],[LastConnectErrorCount],[LastConnectErrorTime],[LastConnectTime],[LegacyUsername],[LogisticsApp],[MasterDataApp],[MaterialManagementApp],[Name],[Nummer],[Password],[SalesDistributionApp],[SelectedLanguage],[SettingsApp],[SuperAdministrator],[TelephoneHome],[TelephoneIP],[TelephoneMobile],[Username]) OUTPUT INSERTED.[Id] VALUES (@AccessProfileId,@CompanyId,@CountryId,@Creation_User_Id,@CreationTime,@CustomerServiceApp,@Delete_Date,@Delete_User_Id,@DepartmentId,@Email,@Fax,@FinanceControlApp,@HumanResourcesApp,@Is_Archived,@IsActivated,@IsGlobalDirector,@Last_Edit_Date,@Last_Edit_User_Id,@LastConnectErrorCount,@LastConnectErrorTime,@LastConnectTime,@LegacyUsername,@LogisticsApp,@MasterDataApp,@MaterialManagementApp,@Name,@Nummer,@Password,@SalesDistributionApp,@SelectedLanguage,@SettingsApp,@SuperAdministrator,@TelephoneHome,@TelephoneIP,@TelephoneMobile,@Username); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AccessProfileId", item.AccessProfileId);
					sqlCommand.Parameters.AddWithValue("CompanyId", item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
					sqlCommand.Parameters.AddWithValue("CountryId", item.CountryId == null ? (object)DBNull.Value : item.CountryId);
					sqlCommand.Parameters.AddWithValue("Creation_User_Id", item.Creation_User_Id);
					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CustomerServiceApp", item.CustomerServiceApp == null ? (object)DBNull.Value : item.CustomerServiceApp);
					sqlCommand.Parameters.AddWithValue("Delete_Date", item.Delete_Date == null ? (object)DBNull.Value : item.Delete_Date);
					sqlCommand.Parameters.AddWithValue("Delete_User_Id", item.Delete_User_Id == null ? (object)DBNull.Value : item.Delete_User_Id);
					sqlCommand.Parameters.AddWithValue("DepartmentId", item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
					sqlCommand.Parameters.AddWithValue("Email", item.Email == null ? (object)DBNull.Value : item.Email);
					sqlCommand.Parameters.AddWithValue("Fax", item.Fax == null ? (object)DBNull.Value : item.Fax);
					sqlCommand.Parameters.AddWithValue("FinanceControlApp", item.FinanceControlApp == null ? (object)DBNull.Value : item.FinanceControlApp);
					sqlCommand.Parameters.AddWithValue("HumanResourcesApp", item.HumanResourcesApp == null ? (object)DBNull.Value : item.HumanResourcesApp);
					sqlCommand.Parameters.AddWithValue("Is_Archived", item.Is_Archived);
					sqlCommand.Parameters.AddWithValue("IsActivated", item.IsActivated);
					sqlCommand.Parameters.AddWithValue("IsGlobalDirector", item.IsGlobalDirector == null ? (object)DBNull.Value : item.IsGlobalDirector);
					sqlCommand.Parameters.AddWithValue("Last_Edit_Date", item.Last_Edit_Date == null ? (object)DBNull.Value : item.Last_Edit_Date);
					sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id", item.Last_Edit_User_Id == null ? (object)DBNull.Value : item.Last_Edit_User_Id);
					sqlCommand.Parameters.AddWithValue("LastConnectErrorCount", item.LastConnectErrorCount == null ? (object)DBNull.Value : item.LastConnectErrorCount);
					sqlCommand.Parameters.AddWithValue("LastConnectErrorTime", item.LastConnectErrorTime == null ? (object)DBNull.Value : item.LastConnectErrorTime);
					sqlCommand.Parameters.AddWithValue("LastConnectTime", item.LastConnectTime == null ? (object)DBNull.Value : item.LastConnectTime);
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
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.UserEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 37; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.UserEntity> items)
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
						query += " INSERT INTO [User] ([AccessProfileId],[CompanyId],[CountryId],[Creation_User_Id],[CreationTime],[CustomerServiceApp],[Delete_Date],[Delete_User_Id],[DepartmentId],[Email],[Fax],[FinanceControlApp],[HumanResourcesApp],[Is_Archived],[IsActivated],[IsGlobalDirector],[Last_Edit_Date],[Last_Edit_User_Id],[LastConnectErrorCount],[LastConnectErrorTime],[LastConnectTime],[LegacyUsername],[LogisticsApp],[MasterDataApp],[MaterialManagementApp],[Name],[Nummer],[Password],[SalesDistributionApp],[SelectedLanguage],[SettingsApp],[SuperAdministrator],[TelephoneHome],[TelephoneIP],[TelephoneMobile],[Username]) VALUES ( "

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
							+ "@IsGlobalDirector" + i + ","
							+ "@Last_Edit_Date" + i + ","
							+ "@Last_Edit_User_Id" + i + ","
							+ "@LastConnectErrorCount" + i + ","
							+ "@LastConnectErrorTime" + i + ","
							+ "@LastConnectTime" + i + ","
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
						sqlCommand.Parameters.AddWithValue("Creation_User_Id" + i, item.Creation_User_Id);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CustomerServiceApp" + i, item.CustomerServiceApp == null ? (object)DBNull.Value : item.CustomerServiceApp);
						sqlCommand.Parameters.AddWithValue("Delete_Date" + i, item.Delete_Date == null ? (object)DBNull.Value : item.Delete_Date);
						sqlCommand.Parameters.AddWithValue("Delete_User_Id" + i, item.Delete_User_Id == null ? (object)DBNull.Value : item.Delete_User_Id);
						sqlCommand.Parameters.AddWithValue("DepartmentId" + i, item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
						sqlCommand.Parameters.AddWithValue("Email" + i, item.Email == null ? (object)DBNull.Value : item.Email);
						sqlCommand.Parameters.AddWithValue("Fax" + i, item.Fax == null ? (object)DBNull.Value : item.Fax);
						sqlCommand.Parameters.AddWithValue("FinanceControlApp" + i, item.FinanceControlApp == null ? (object)DBNull.Value : item.FinanceControlApp);
						sqlCommand.Parameters.AddWithValue("HumanResourcesApp" + i, item.HumanResourcesApp == null ? (object)DBNull.Value : item.HumanResourcesApp);
						sqlCommand.Parameters.AddWithValue("Is_Archived" + i, item.Is_Archived);
						sqlCommand.Parameters.AddWithValue("IsActivated" + i, item.IsActivated);
						sqlCommand.Parameters.AddWithValue("IsGlobalDirector" + i, item.IsGlobalDirector == null ? (object)DBNull.Value : item.IsGlobalDirector);
						sqlCommand.Parameters.AddWithValue("Last_Edit_Date" + i, item.Last_Edit_Date == null ? (object)DBNull.Value : item.Last_Edit_Date);
						sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id" + i, item.Last_Edit_User_Id == null ? (object)DBNull.Value : item.Last_Edit_User_Id);
						sqlCommand.Parameters.AddWithValue("LastConnectErrorCount" + i, item.LastConnectErrorCount == null ? (object)DBNull.Value : item.LastConnectErrorCount);
						sqlCommand.Parameters.AddWithValue("LastConnectErrorTime" + i, item.LastConnectErrorTime == null ? (object)DBNull.Value : item.LastConnectErrorTime);
						sqlCommand.Parameters.AddWithValue("LastConnectTime" + i, item.LastConnectTime == null ? (object)DBNull.Value : item.LastConnectTime);
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

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.UserEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [User] SET [AccessProfileId]=@AccessProfileId, [CompanyId]=@CompanyId, [CountryId]=@CountryId, [Creation_User_Id]=@Creation_User_Id, [CreationTime]=@CreationTime, [CustomerServiceApp]=@CustomerServiceApp, [Delete_Date]=@Delete_Date, [Delete_User_Id]=@Delete_User_Id, [DepartmentId]=@DepartmentId, [Email]=@Email, [Fax]=@Fax, [FinanceControlApp]=@FinanceControlApp, [HumanResourcesApp]=@HumanResourcesApp, [Is_Archived]=@Is_Archived, [IsActivated]=@IsActivated, [IsGlobalDirector]=@IsGlobalDirector, [Last_Edit_Date]=@Last_Edit_Date, [Last_Edit_User_Id]=@Last_Edit_User_Id, [LastConnectErrorCount]=@LastConnectErrorCount, [LastConnectErrorTime]=@LastConnectErrorTime, [LastConnectTime]=@LastConnectTime, [LegacyUsername]=@LegacyUsername, [LogisticsApp]=@LogisticsApp, [MasterDataApp]=@MasterDataApp, [MaterialManagementApp]=@MaterialManagementApp, [Name]=@Name, [Nummer]=@Nummer, [Password]=@Password, [SalesDistributionApp]=@SalesDistributionApp, [SelectedLanguage]=@SelectedLanguage, [SettingsApp]=@SettingsApp, [SuperAdministrator]=@SuperAdministrator, [TelephoneHome]=@TelephoneHome, [TelephoneIP]=@TelephoneIP, [TelephoneMobile]=@TelephoneMobile, [Username]=@Username WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AccessProfileId", item.AccessProfileId);
				sqlCommand.Parameters.AddWithValue("CompanyId", item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
				sqlCommand.Parameters.AddWithValue("CountryId", item.CountryId == null ? (object)DBNull.Value : item.CountryId);
				sqlCommand.Parameters.AddWithValue("Creation_User_Id", item.Creation_User_Id);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CustomerServiceApp", item.CustomerServiceApp == null ? (object)DBNull.Value : item.CustomerServiceApp);
				sqlCommand.Parameters.AddWithValue("Delete_Date", item.Delete_Date == null ? (object)DBNull.Value : item.Delete_Date);
				sqlCommand.Parameters.AddWithValue("Delete_User_Id", item.Delete_User_Id == null ? (object)DBNull.Value : item.Delete_User_Id);
				sqlCommand.Parameters.AddWithValue("DepartmentId", item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
				sqlCommand.Parameters.AddWithValue("Email", item.Email == null ? (object)DBNull.Value : item.Email);
				sqlCommand.Parameters.AddWithValue("Fax", item.Fax == null ? (object)DBNull.Value : item.Fax);
				sqlCommand.Parameters.AddWithValue("FinanceControlApp", item.FinanceControlApp == null ? (object)DBNull.Value : item.FinanceControlApp);
				sqlCommand.Parameters.AddWithValue("HumanResourcesApp", item.HumanResourcesApp == null ? (object)DBNull.Value : item.HumanResourcesApp);
				sqlCommand.Parameters.AddWithValue("Is_Archived", item.Is_Archived);
				sqlCommand.Parameters.AddWithValue("IsActivated", item.IsActivated);
				sqlCommand.Parameters.AddWithValue("IsGlobalDirector", item.IsGlobalDirector == null ? (object)DBNull.Value : item.IsGlobalDirector);
				sqlCommand.Parameters.AddWithValue("Last_Edit_Date", item.Last_Edit_Date == null ? (object)DBNull.Value : item.Last_Edit_Date);
				sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id", item.Last_Edit_User_Id == null ? (object)DBNull.Value : item.Last_Edit_User_Id);
				sqlCommand.Parameters.AddWithValue("LastConnectErrorCount", item.LastConnectErrorCount == null ? (object)DBNull.Value : item.LastConnectErrorCount);
				sqlCommand.Parameters.AddWithValue("LastConnectErrorTime", item.LastConnectErrorTime == null ? (object)DBNull.Value : item.LastConnectErrorTime);
				sqlCommand.Parameters.AddWithValue("LastConnectTime", item.LastConnectTime == null ? (object)DBNull.Value : item.LastConnectTime);
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
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.UserEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 37; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.UserEntity> items)
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
							+ "[IsGlobalDirector]=@IsGlobalDirector" + i + ","
							+ "[Last_Edit_Date]=@Last_Edit_Date" + i + ","
							+ "[Last_Edit_User_Id]=@Last_Edit_User_Id" + i + ","
							+ "[LastConnectErrorCount]=@LastConnectErrorCount" + i + ","
							+ "[LastConnectErrorTime]=@LastConnectErrorTime" + i + ","
							+ "[LastConnectTime]=@LastConnectTime" + i + ","
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
						sqlCommand.Parameters.AddWithValue("Creation_User_Id" + i, item.Creation_User_Id);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CustomerServiceApp" + i, item.CustomerServiceApp == null ? (object)DBNull.Value : item.CustomerServiceApp);
						sqlCommand.Parameters.AddWithValue("Delete_Date" + i, item.Delete_Date == null ? (object)DBNull.Value : item.Delete_Date);
						sqlCommand.Parameters.AddWithValue("Delete_User_Id" + i, item.Delete_User_Id == null ? (object)DBNull.Value : item.Delete_User_Id);
						sqlCommand.Parameters.AddWithValue("DepartmentId" + i, item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
						sqlCommand.Parameters.AddWithValue("Email" + i, item.Email == null ? (object)DBNull.Value : item.Email);
						sqlCommand.Parameters.AddWithValue("Fax" + i, item.Fax == null ? (object)DBNull.Value : item.Fax);
						sqlCommand.Parameters.AddWithValue("FinanceControlApp" + i, item.FinanceControlApp == null ? (object)DBNull.Value : item.FinanceControlApp);
						sqlCommand.Parameters.AddWithValue("HumanResourcesApp" + i, item.HumanResourcesApp == null ? (object)DBNull.Value : item.HumanResourcesApp);
						sqlCommand.Parameters.AddWithValue("Is_Archived" + i, item.Is_Archived);
						sqlCommand.Parameters.AddWithValue("IsActivated" + i, item.IsActivated);
						sqlCommand.Parameters.AddWithValue("IsGlobalDirector" + i, item.IsGlobalDirector == null ? (object)DBNull.Value : item.IsGlobalDirector);
						sqlCommand.Parameters.AddWithValue("Last_Edit_Date" + i, item.Last_Edit_Date == null ? (object)DBNull.Value : item.Last_Edit_Date);
						sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id" + i, item.Last_Edit_User_Id == null ? (object)DBNull.Value : item.Last_Edit_User_Id);
						sqlCommand.Parameters.AddWithValue("LastConnectErrorCount" + i, item.LastConnectErrorCount == null ? (object)DBNull.Value : item.LastConnectErrorCount);
						sqlCommand.Parameters.AddWithValue("LastConnectErrorTime" + i, item.LastConnectErrorTime == null ? (object)DBNull.Value : item.LastConnectErrorTime);
						sqlCommand.Parameters.AddWithValue("LastConnectTime" + i, item.LastConnectTime == null ? (object)DBNull.Value : item.LastConnectTime);
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
		public static Infrastructure.Data.Entities.Tables.FNC.UserEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [User] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.UserEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.UserEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [User]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.UserEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.UserEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.UserEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.UserEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.UserEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.UserEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.UserEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [User] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.UserEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.UserEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.UserEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.FNC.UserEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [User] ([AccessProfileId],[CompanyId],[CountryId],[Creation_User_Id],[CreationTime],[CustomerServiceApp],[Delete_Date],[Delete_User_Id],[DepartmentId],[Email],[Fax],[FinanceControlApp],[HumanResourcesApp],[Is_Archived],[IsActivated],[IsGlobalDirector],[Last_Edit_Date],[Last_Edit_User_Id],[LastConnectErrorCount],[LastConnectErrorTime],[LastConnectTime],[LegacyUsername],[LogisticsApp],[MasterDataApp],[MaterialManagementApp],[Name],[Nummer],[Password],[SalesDistributionApp],[SelectedLanguage],[SettingsApp],[SuperAdministrator],[TelephoneHome],[TelephoneIP],[TelephoneMobile],[Username]) OUTPUT INSERTED.[Id] VALUES (@AccessProfileId,@CompanyId,@CountryId,@Creation_User_Id,@CreationTime,@CustomerServiceApp,@Delete_Date,@Delete_User_Id,@DepartmentId,@Email,@Fax,@FinanceControlApp,@HumanResourcesApp,@Is_Archived,@IsActivated,@IsGlobalDirector,@Last_Edit_Date,@Last_Edit_User_Id,@LastConnectErrorCount,@LastConnectErrorTime,@LastConnectTime,@LegacyUsername,@LogisticsApp,@MasterDataApp,@MaterialManagementApp,@Name,@Nummer,@Password,@SalesDistributionApp,@SelectedLanguage,@SettingsApp,@SuperAdministrator,@TelephoneHome,@TelephoneIP,@TelephoneMobile,@Username); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("AccessProfileId", item.AccessProfileId);
			sqlCommand.Parameters.AddWithValue("CompanyId", item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
			sqlCommand.Parameters.AddWithValue("CountryId", item.CountryId == null ? (object)DBNull.Value : item.CountryId);
			sqlCommand.Parameters.AddWithValue("Creation_User_Id", item.Creation_User_Id);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
			sqlCommand.Parameters.AddWithValue("CustomerServiceApp", item.CustomerServiceApp == null ? (object)DBNull.Value : item.CustomerServiceApp);
			sqlCommand.Parameters.AddWithValue("Delete_Date", item.Delete_Date == null ? (object)DBNull.Value : item.Delete_Date);
			sqlCommand.Parameters.AddWithValue("Delete_User_Id", item.Delete_User_Id == null ? (object)DBNull.Value : item.Delete_User_Id);
			sqlCommand.Parameters.AddWithValue("DepartmentId", item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
			sqlCommand.Parameters.AddWithValue("Email", item.Email == null ? (object)DBNull.Value : item.Email);
			sqlCommand.Parameters.AddWithValue("Fax", item.Fax == null ? (object)DBNull.Value : item.Fax);
			sqlCommand.Parameters.AddWithValue("FinanceControlApp", item.FinanceControlApp == null ? (object)DBNull.Value : item.FinanceControlApp);
			sqlCommand.Parameters.AddWithValue("HumanResourcesApp", item.HumanResourcesApp == null ? (object)DBNull.Value : item.HumanResourcesApp);
			sqlCommand.Parameters.AddWithValue("Is_Archived", item.Is_Archived);
			sqlCommand.Parameters.AddWithValue("IsActivated", item.IsActivated);
			sqlCommand.Parameters.AddWithValue("IsGlobalDirector", item.IsGlobalDirector == null ? (object)DBNull.Value : item.IsGlobalDirector);
			sqlCommand.Parameters.AddWithValue("Last_Edit_Date", item.Last_Edit_Date == null ? (object)DBNull.Value : item.Last_Edit_Date);
			sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id", item.Last_Edit_User_Id == null ? (object)DBNull.Value : item.Last_Edit_User_Id);
			sqlCommand.Parameters.AddWithValue("LastConnectErrorCount", item.LastConnectErrorCount == null ? (object)DBNull.Value : item.LastConnectErrorCount);
			sqlCommand.Parameters.AddWithValue("LastConnectErrorTime", item.LastConnectErrorTime == null ? (object)DBNull.Value : item.LastConnectErrorTime);
			sqlCommand.Parameters.AddWithValue("LastConnectTime", item.LastConnectTime == null ? (object)DBNull.Value : item.LastConnectTime);
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
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.UserEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 37; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.UserEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [User] ([AccessProfileId],[CompanyId],[CountryId],[Creation_User_Id],[CreationTime],[CustomerServiceApp],[Delete_Date],[Delete_User_Id],[DepartmentId],[Email],[Fax],[FinanceControlApp],[HumanResourcesApp],[Is_Archived],[IsActivated],[IsGlobalDirector],[Last_Edit_Date],[Last_Edit_User_Id],[LastConnectErrorCount],[LastConnectErrorTime],[LastConnectTime],[LegacyUsername],[LogisticsApp],[MasterDataApp],[MaterialManagementApp],[Name],[Nummer],[Password],[SalesDistributionApp],[SelectedLanguage],[SettingsApp],[SuperAdministrator],[TelephoneHome],[TelephoneIP],[TelephoneMobile],[Username]) VALUES ( "

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
						+ "@IsGlobalDirector" + i + ","
						+ "@Last_Edit_Date" + i + ","
						+ "@Last_Edit_User_Id" + i + ","
						+ "@LastConnectErrorCount" + i + ","
						+ "@LastConnectErrorTime" + i + ","
						+ "@LastConnectTime" + i + ","
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
					sqlCommand.Parameters.AddWithValue("Creation_User_Id" + i, item.Creation_User_Id);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CustomerServiceApp" + i, item.CustomerServiceApp == null ? (object)DBNull.Value : item.CustomerServiceApp);
					sqlCommand.Parameters.AddWithValue("Delete_Date" + i, item.Delete_Date == null ? (object)DBNull.Value : item.Delete_Date);
					sqlCommand.Parameters.AddWithValue("Delete_User_Id" + i, item.Delete_User_Id == null ? (object)DBNull.Value : item.Delete_User_Id);
					sqlCommand.Parameters.AddWithValue("DepartmentId" + i, item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
					sqlCommand.Parameters.AddWithValue("Email" + i, item.Email == null ? (object)DBNull.Value : item.Email);
					sqlCommand.Parameters.AddWithValue("Fax" + i, item.Fax == null ? (object)DBNull.Value : item.Fax);
					sqlCommand.Parameters.AddWithValue("FinanceControlApp" + i, item.FinanceControlApp == null ? (object)DBNull.Value : item.FinanceControlApp);
					sqlCommand.Parameters.AddWithValue("HumanResourcesApp" + i, item.HumanResourcesApp == null ? (object)DBNull.Value : item.HumanResourcesApp);
					sqlCommand.Parameters.AddWithValue("Is_Archived" + i, item.Is_Archived);
					sqlCommand.Parameters.AddWithValue("IsActivated" + i, item.IsActivated);
					sqlCommand.Parameters.AddWithValue("IsGlobalDirector" + i, item.IsGlobalDirector == null ? (object)DBNull.Value : item.IsGlobalDirector);
					sqlCommand.Parameters.AddWithValue("Last_Edit_Date" + i, item.Last_Edit_Date == null ? (object)DBNull.Value : item.Last_Edit_Date);
					sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id" + i, item.Last_Edit_User_Id == null ? (object)DBNull.Value : item.Last_Edit_User_Id);
					sqlCommand.Parameters.AddWithValue("LastConnectErrorCount" + i, item.LastConnectErrorCount == null ? (object)DBNull.Value : item.LastConnectErrorCount);
					sqlCommand.Parameters.AddWithValue("LastConnectErrorTime" + i, item.LastConnectErrorTime == null ? (object)DBNull.Value : item.LastConnectErrorTime);
					sqlCommand.Parameters.AddWithValue("LastConnectTime" + i, item.LastConnectTime == null ? (object)DBNull.Value : item.LastConnectTime);
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

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.FNC.UserEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [User] SET [AccessProfileId]=@AccessProfileId, [CompanyId]=@CompanyId, [CountryId]=@CountryId, [Creation_User_Id]=@Creation_User_Id, [CreationTime]=@CreationTime, [CustomerServiceApp]=@CustomerServiceApp, [Delete_Date]=@Delete_Date, [Delete_User_Id]=@Delete_User_Id, [DepartmentId]=@DepartmentId, [Email]=@Email, [Fax]=@Fax, [FinanceControlApp]=@FinanceControlApp, [HumanResourcesApp]=@HumanResourcesApp, [Is_Archived]=@Is_Archived, [IsActivated]=@IsActivated, [IsGlobalDirector]=@IsGlobalDirector, [Last_Edit_Date]=@Last_Edit_Date, [Last_Edit_User_Id]=@Last_Edit_User_Id, [LastConnectErrorCount]=@LastConnectErrorCount, [LastConnectErrorTime]=@LastConnectErrorTime, [LastConnectTime]=@LastConnectTime, [LegacyUsername]=@LegacyUsername, [LogisticsApp]=@LogisticsApp, [MasterDataApp]=@MasterDataApp, [MaterialManagementApp]=@MaterialManagementApp, [Name]=@Name, [Nummer]=@Nummer, [Password]=@Password, [SalesDistributionApp]=@SalesDistributionApp, [SelectedLanguage]=@SelectedLanguage, [SettingsApp]=@SettingsApp, [SuperAdministrator]=@SuperAdministrator, [TelephoneHome]=@TelephoneHome, [TelephoneIP]=@TelephoneIP, [TelephoneMobile]=@TelephoneMobile, [Username]=@Username WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("AccessProfileId", item.AccessProfileId);
			sqlCommand.Parameters.AddWithValue("CompanyId", item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
			sqlCommand.Parameters.AddWithValue("CountryId", item.CountryId == null ? (object)DBNull.Value : item.CountryId);
			sqlCommand.Parameters.AddWithValue("Creation_User_Id", item.Creation_User_Id);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
			sqlCommand.Parameters.AddWithValue("CustomerServiceApp", item.CustomerServiceApp == null ? (object)DBNull.Value : item.CustomerServiceApp);
			sqlCommand.Parameters.AddWithValue("Delete_Date", item.Delete_Date == null ? (object)DBNull.Value : item.Delete_Date);
			sqlCommand.Parameters.AddWithValue("Delete_User_Id", item.Delete_User_Id == null ? (object)DBNull.Value : item.Delete_User_Id);
			sqlCommand.Parameters.AddWithValue("DepartmentId", item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
			sqlCommand.Parameters.AddWithValue("Email", item.Email == null ? (object)DBNull.Value : item.Email);
			sqlCommand.Parameters.AddWithValue("Fax", item.Fax == null ? (object)DBNull.Value : item.Fax);
			sqlCommand.Parameters.AddWithValue("FinanceControlApp", item.FinanceControlApp == null ? (object)DBNull.Value : item.FinanceControlApp);
			sqlCommand.Parameters.AddWithValue("HumanResourcesApp", item.HumanResourcesApp == null ? (object)DBNull.Value : item.HumanResourcesApp);
			sqlCommand.Parameters.AddWithValue("Is_Archived", item.Is_Archived);
			sqlCommand.Parameters.AddWithValue("IsActivated", item.IsActivated);
			sqlCommand.Parameters.AddWithValue("IsGlobalDirector", item.IsGlobalDirector == null ? (object)DBNull.Value : item.IsGlobalDirector);
			sqlCommand.Parameters.AddWithValue("Last_Edit_Date", item.Last_Edit_Date == null ? (object)DBNull.Value : item.Last_Edit_Date);
			sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id", item.Last_Edit_User_Id == null ? (object)DBNull.Value : item.Last_Edit_User_Id);
			sqlCommand.Parameters.AddWithValue("LastConnectErrorCount", item.LastConnectErrorCount == null ? (object)DBNull.Value : item.LastConnectErrorCount);
			sqlCommand.Parameters.AddWithValue("LastConnectErrorTime", item.LastConnectErrorTime == null ? (object)DBNull.Value : item.LastConnectErrorTime);
			sqlCommand.Parameters.AddWithValue("LastConnectTime", item.LastConnectTime == null ? (object)DBNull.Value : item.LastConnectTime);
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
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.UserEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 37; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.UserEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				//int results = -1;
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
					+ "[IsGlobalDirector]=@IsGlobalDirector" + i + ","
					+ "[Last_Edit_Date]=@Last_Edit_Date" + i + ","
					+ "[Last_Edit_User_Id]=@Last_Edit_User_Id" + i + ","
					+ "[LastConnectErrorCount]=@LastConnectErrorCount" + i + ","
					+ "[LastConnectErrorTime]=@LastConnectErrorTime" + i + ","
					+ "[LastConnectTime]=@LastConnectTime" + i + ","
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
					sqlCommand.Parameters.AddWithValue("Creation_User_Id" + i, item.Creation_User_Id);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CustomerServiceApp" + i, item.CustomerServiceApp == null ? (object)DBNull.Value : item.CustomerServiceApp);
					sqlCommand.Parameters.AddWithValue("Delete_Date" + i, item.Delete_Date == null ? (object)DBNull.Value : item.Delete_Date);
					sqlCommand.Parameters.AddWithValue("Delete_User_Id" + i, item.Delete_User_Id == null ? (object)DBNull.Value : item.Delete_User_Id);
					sqlCommand.Parameters.AddWithValue("DepartmentId" + i, item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
					sqlCommand.Parameters.AddWithValue("Email" + i, item.Email == null ? (object)DBNull.Value : item.Email);
					sqlCommand.Parameters.AddWithValue("Fax" + i, item.Fax == null ? (object)DBNull.Value : item.Fax);
					sqlCommand.Parameters.AddWithValue("FinanceControlApp" + i, item.FinanceControlApp == null ? (object)DBNull.Value : item.FinanceControlApp);
					sqlCommand.Parameters.AddWithValue("HumanResourcesApp" + i, item.HumanResourcesApp == null ? (object)DBNull.Value : item.HumanResourcesApp);
					sqlCommand.Parameters.AddWithValue("Is_Archived" + i, item.Is_Archived);
					sqlCommand.Parameters.AddWithValue("IsActivated" + i, item.IsActivated);
					sqlCommand.Parameters.AddWithValue("IsGlobalDirector" + i, item.IsGlobalDirector == null ? (object)DBNull.Value : item.IsGlobalDirector);
					sqlCommand.Parameters.AddWithValue("Last_Edit_Date" + i, item.Last_Edit_Date == null ? (object)DBNull.Value : item.Last_Edit_Date);
					sqlCommand.Parameters.AddWithValue("Last_Edit_User_Id" + i, item.Last_Edit_User_Id == null ? (object)DBNull.Value : item.Last_Edit_User_Id);
					sqlCommand.Parameters.AddWithValue("LastConnectErrorCount" + i, item.LastConnectErrorCount == null ? (object)DBNull.Value : item.LastConnectErrorCount);
					sqlCommand.Parameters.AddWithValue("LastConnectErrorTime" + i, item.LastConnectErrorTime == null ? (object)DBNull.Value : item.LastConnectErrorTime);
					sqlCommand.Parameters.AddWithValue("LastConnectTime" + i, item.LastConnectTime == null ? (object)DBNull.Value : item.LastConnectTime);
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
		#endregion Default Methods

		#region Custom Methods

		#endregion Custom Methods

	}


}
