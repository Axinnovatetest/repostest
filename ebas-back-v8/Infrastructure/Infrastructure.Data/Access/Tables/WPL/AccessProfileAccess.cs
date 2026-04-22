using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Collections;

namespace Infrastructure.Data.Access.Tables.WPL
{
	public class AccessProfileAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [WPL_AccessProfile] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [WPL_AccessProfile]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [WPL_AccessProfile] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [WPL_AccessProfile] ([Access],[AccessUpdate],[AdministrationAccessProfiles],[AdministrationAccessProfilesUpdate],[AdminstrationUser],[AdminstrationUserUpdate],[Country],[CountryCreate],[CountryDelete],[CountryUpdate],[Departement],[DepartementCreate],[DepartementDelete],[DepartementUpdate],[Hall],[HallCreate],[HallDelete],[HallUpdate],[isDefault],[MainAccessProfileId],[ModuleActivated],[StandardOperation],[StandardOperationCreate],[StandardOperationDelete],[StandardOperationUpdate],[SuperAdministrator],[WorkArea],[WorkAreaCreate],[WorkAreaDelete],[WorkAreaUpdate],[WorkPlan],[WorkPlanCreate],[WorkPlanDelete],[WorkPlanReporting],[WorkPlanReportingCreate],[WorkPlanReportingDelete],[WorkPlanReportingUpdate],[WorkPlanUpdate],[WorkStation],[WorkStationCreate],[WorkStationDelete],[WorkStationUpdate]) OUTPUT INSERTED.[Id] VALUES (@Access,@AccessUpdate,@AdministrationAccessProfiles,@AdministrationAccessProfilesUpdate,@AdminstrationUser,@AdminstrationUserUpdate,@Country,@CountryCreate,@CountryDelete,@CountryUpdate,@Departement,@DepartementCreate,@DepartementDelete,@DepartementUpdate,@Hall,@HallCreate,@HallDelete,@HallUpdate,@isDefault,@MainAccessProfileId,@ModuleActivated,@StandardOperation,@StandardOperationCreate,@StandardOperationDelete,@StandardOperationUpdate,@SuperAdministrator,@WorkArea,@WorkAreaCreate,@WorkAreaDelete,@WorkAreaUpdate,@WorkPlan,@WorkPlanCreate,@WorkPlanDelete,@WorkPlanReporting,@WorkPlanReportingCreate,@WorkPlanReportingDelete,@WorkPlanReportingUpdate,@WorkPlanUpdate,@WorkStation,@WorkStationCreate,@WorkStationDelete,@WorkStationUpdate); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Access", item.Access);
					sqlCommand.Parameters.AddWithValue("AccessUpdate", item.AccessUpdate);
					sqlCommand.Parameters.AddWithValue("AdministrationAccessProfiles", item.AdministrationAccessProfiles);
					sqlCommand.Parameters.AddWithValue("AdministrationAccessProfilesUpdate", item.AdministrationAccessProfilesUpdate);
					sqlCommand.Parameters.AddWithValue("AdminstrationUser", item.AdminstrationUser);
					sqlCommand.Parameters.AddWithValue("AdminstrationUserUpdate", item.AdminstrationUserUpdate);
					sqlCommand.Parameters.AddWithValue("Country", item.Country);
					sqlCommand.Parameters.AddWithValue("CountryCreate", item.CountryCreate);
					sqlCommand.Parameters.AddWithValue("CountryDelete", item.CountryDelete);
					sqlCommand.Parameters.AddWithValue("CountryUpdate", item.CountryUpdate);
					sqlCommand.Parameters.AddWithValue("Departement", item.Departement);
					sqlCommand.Parameters.AddWithValue("DepartementCreate", item.DepartementCreate);
					sqlCommand.Parameters.AddWithValue("DepartementDelete", item.DepartementDelete);
					sqlCommand.Parameters.AddWithValue("DepartementUpdate", item.DepartementUpdate);
					sqlCommand.Parameters.AddWithValue("Hall", item.Hall);
					sqlCommand.Parameters.AddWithValue("HallCreate", item.HallCreate);
					sqlCommand.Parameters.AddWithValue("HallDelete", item.HallDelete);
					sqlCommand.Parameters.AddWithValue("HallUpdate", item.HallUpdate);
					sqlCommand.Parameters.AddWithValue("isDefault", item.isDefault == null ? (object)DBNull.Value : item.isDefault);
					sqlCommand.Parameters.AddWithValue("MainAccessProfileId", item.MainAccessProfileId);
					sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated);
					sqlCommand.Parameters.AddWithValue("StandardOperation", item.StandardOperation);
					sqlCommand.Parameters.AddWithValue("StandardOperationCreate", item.StandardOperationCreate);
					sqlCommand.Parameters.AddWithValue("StandardOperationDelete", item.StandardOperationDelete);
					sqlCommand.Parameters.AddWithValue("StandardOperationUpdate", item.StandardOperationUpdate);
					sqlCommand.Parameters.AddWithValue("SuperAdministrator", item.SuperAdministrator);
					sqlCommand.Parameters.AddWithValue("WorkArea", item.WorkArea);
					sqlCommand.Parameters.AddWithValue("WorkAreaCreate", item.WorkAreaCreate);
					sqlCommand.Parameters.AddWithValue("WorkAreaDelete", item.WorkAreaDelete);
					sqlCommand.Parameters.AddWithValue("WorkAreaUpdate", item.WorkAreaUpdate);
					sqlCommand.Parameters.AddWithValue("WorkPlan", item.WorkPlan);
					sqlCommand.Parameters.AddWithValue("WorkPlanCreate", item.WorkPlanCreate);
					sqlCommand.Parameters.AddWithValue("WorkPlanDelete", item.WorkPlanDelete);
					sqlCommand.Parameters.AddWithValue("WorkPlanReporting", item.WorkPlanReporting);
					sqlCommand.Parameters.AddWithValue("WorkPlanReportingCreate", item.WorkPlanReportingCreate);
					sqlCommand.Parameters.AddWithValue("WorkPlanReportingDelete", item.WorkPlanReportingDelete);
					sqlCommand.Parameters.AddWithValue("WorkPlanReportingUpdate", item.WorkPlanReportingUpdate);
					sqlCommand.Parameters.AddWithValue("WorkPlanUpdate", item.WorkPlanUpdate);
					sqlCommand.Parameters.AddWithValue("WorkStation", item.WorkStation);
					sqlCommand.Parameters.AddWithValue("WorkStationCreate", item.WorkStationCreate);
					sqlCommand.Parameters.AddWithValue("WorkStationDelete", item.WorkStationDelete);
					sqlCommand.Parameters.AddWithValue("WorkStationUpdate", item.WorkStationUpdate);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 43; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity> items)
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
						query += " INSERT INTO [WPL_AccessProfile] ([Access],[AccessUpdate],[AdministrationAccessProfiles],[AdministrationAccessProfilesUpdate],[AdminstrationUser],[AdminstrationUserUpdate],[Country],[CountryCreate],[CountryDelete],[CountryUpdate],[Departement],[DepartementCreate],[DepartementDelete],[DepartementUpdate],[Hall],[HallCreate],[HallDelete],[HallUpdate],[isDefault],[MainAccessProfileId],[ModuleActivated],[StandardOperation],[StandardOperationCreate],[StandardOperationDelete],[StandardOperationUpdate],[SuperAdministrator],[WorkArea],[WorkAreaCreate],[WorkAreaDelete],[WorkAreaUpdate],[WorkPlan],[WorkPlanCreate],[WorkPlanDelete],[WorkPlanReporting],[WorkPlanReportingCreate],[WorkPlanReportingDelete],[WorkPlanReportingUpdate],[WorkPlanUpdate],[WorkStation],[WorkStationCreate],[WorkStationDelete],[WorkStationUpdate]) VALUES ( "

							+ "@Access" + i + ","
							+ "@AccessUpdate" + i + ","
							+ "@AdministrationAccessProfiles" + i + ","
							+ "@AdministrationAccessProfilesUpdate" + i + ","
							+ "@AdminstrationUser" + i + ","
							+ "@AdminstrationUserUpdate" + i + ","
							+ "@Country" + i + ","
							+ "@CountryCreate" + i + ","
							+ "@CountryDelete" + i + ","
							+ "@CountryUpdate" + i + ","
							+ "@Departement" + i + ","
							+ "@DepartementCreate" + i + ","
							+ "@DepartementDelete" + i + ","
							+ "@DepartementUpdate" + i + ","
							+ "@Hall" + i + ","
							+ "@HallCreate" + i + ","
							+ "@HallDelete" + i + ","
							+ "@HallUpdate" + i + ","
							+ "@isDefault" + i + ","
							+ "@MainAccessProfileId" + i + ","
							+ "@ModuleActivated" + i + ","
							+ "@StandardOperation" + i + ","
							+ "@StandardOperationCreate" + i + ","
							+ "@StandardOperationDelete" + i + ","
							+ "@StandardOperationUpdate" + i + ","
							+ "@SuperAdministrator" + i + ","
							+ "@WorkArea" + i + ","
							+ "@WorkAreaCreate" + i + ","
							+ "@WorkAreaDelete" + i + ","
							+ "@WorkAreaUpdate" + i + ","
							+ "@WorkPlan" + i + ","
							+ "@WorkPlanCreate" + i + ","
							+ "@WorkPlanDelete" + i + ","
							+ "@WorkPlanReporting" + i + ","
							+ "@WorkPlanReportingCreate" + i + ","
							+ "@WorkPlanReportingDelete" + i + ","
							+ "@WorkPlanReportingUpdate" + i + ","
							+ "@WorkPlanUpdate" + i + ","
							+ "@WorkStation" + i + ","
							+ "@WorkStationCreate" + i + ","
							+ "@WorkStationDelete" + i + ","
							+ "@WorkStationUpdate" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Access" + i, item.Access);
						sqlCommand.Parameters.AddWithValue("AccessUpdate" + i, item.AccessUpdate);
						sqlCommand.Parameters.AddWithValue("AdministrationAccessProfiles" + i, item.AdministrationAccessProfiles);
						sqlCommand.Parameters.AddWithValue("AdministrationAccessProfilesUpdate" + i, item.AdministrationAccessProfilesUpdate);
						sqlCommand.Parameters.AddWithValue("AdminstrationUser" + i, item.AdminstrationUser);
						sqlCommand.Parameters.AddWithValue("AdminstrationUserUpdate" + i, item.AdminstrationUserUpdate);
						sqlCommand.Parameters.AddWithValue("Country" + i, item.Country);
						sqlCommand.Parameters.AddWithValue("CountryCreate" + i, item.CountryCreate);
						sqlCommand.Parameters.AddWithValue("CountryDelete" + i, item.CountryDelete);
						sqlCommand.Parameters.AddWithValue("CountryUpdate" + i, item.CountryUpdate);
						sqlCommand.Parameters.AddWithValue("Departement" + i, item.Departement);
						sqlCommand.Parameters.AddWithValue("DepartementCreate" + i, item.DepartementCreate);
						sqlCommand.Parameters.AddWithValue("DepartementDelete" + i, item.DepartementDelete);
						sqlCommand.Parameters.AddWithValue("DepartementUpdate" + i, item.DepartementUpdate);
						sqlCommand.Parameters.AddWithValue("Hall" + i, item.Hall);
						sqlCommand.Parameters.AddWithValue("HallCreate" + i, item.HallCreate);
						sqlCommand.Parameters.AddWithValue("HallDelete" + i, item.HallDelete);
						sqlCommand.Parameters.AddWithValue("HallUpdate" + i, item.HallUpdate);
						sqlCommand.Parameters.AddWithValue("isDefault" + i, item.isDefault == null ? (object)DBNull.Value : item.isDefault);
						sqlCommand.Parameters.AddWithValue("MainAccessProfileId" + i, item.MainAccessProfileId);
						sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated);
						sqlCommand.Parameters.AddWithValue("StandardOperation" + i, item.StandardOperation);
						sqlCommand.Parameters.AddWithValue("StandardOperationCreate" + i, item.StandardOperationCreate);
						sqlCommand.Parameters.AddWithValue("StandardOperationDelete" + i, item.StandardOperationDelete);
						sqlCommand.Parameters.AddWithValue("StandardOperationUpdate" + i, item.StandardOperationUpdate);
						sqlCommand.Parameters.AddWithValue("SuperAdministrator" + i, item.SuperAdministrator);
						sqlCommand.Parameters.AddWithValue("WorkArea" + i, item.WorkArea);
						sqlCommand.Parameters.AddWithValue("WorkAreaCreate" + i, item.WorkAreaCreate);
						sqlCommand.Parameters.AddWithValue("WorkAreaDelete" + i, item.WorkAreaDelete);
						sqlCommand.Parameters.AddWithValue("WorkAreaUpdate" + i, item.WorkAreaUpdate);
						sqlCommand.Parameters.AddWithValue("WorkPlan" + i, item.WorkPlan);
						sqlCommand.Parameters.AddWithValue("WorkPlanCreate" + i, item.WorkPlanCreate);
						sqlCommand.Parameters.AddWithValue("WorkPlanDelete" + i, item.WorkPlanDelete);
						sqlCommand.Parameters.AddWithValue("WorkPlanReporting" + i, item.WorkPlanReporting);
						sqlCommand.Parameters.AddWithValue("WorkPlanReportingCreate" + i, item.WorkPlanReportingCreate);
						sqlCommand.Parameters.AddWithValue("WorkPlanReportingDelete" + i, item.WorkPlanReportingDelete);
						sqlCommand.Parameters.AddWithValue("WorkPlanReportingUpdate" + i, item.WorkPlanReportingUpdate);
						sqlCommand.Parameters.AddWithValue("WorkPlanUpdate" + i, item.WorkPlanUpdate);
						sqlCommand.Parameters.AddWithValue("WorkStation" + i, item.WorkStation);
						sqlCommand.Parameters.AddWithValue("WorkStationCreate" + i, item.WorkStationCreate);
						sqlCommand.Parameters.AddWithValue("WorkStationDelete" + i, item.WorkStationDelete);
						sqlCommand.Parameters.AddWithValue("WorkStationUpdate" + i, item.WorkStationUpdate);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [WPL_AccessProfile] SET [Access]=@Access, [AccessUpdate]=@AccessUpdate, [AdministrationAccessProfiles]=@AdministrationAccessProfiles, [AdministrationAccessProfilesUpdate]=@AdministrationAccessProfilesUpdate, [AdminstrationUser]=@AdminstrationUser, [AdminstrationUserUpdate]=@AdminstrationUserUpdate, [Country]=@Country, [CountryCreate]=@CountryCreate, [CountryDelete]=@CountryDelete, [CountryUpdate]=@CountryUpdate, [Departement]=@Departement, [DepartementCreate]=@DepartementCreate, [DepartementDelete]=@DepartementDelete, [DepartementUpdate]=@DepartementUpdate, [Hall]=@Hall, [HallCreate]=@HallCreate, [HallDelete]=@HallDelete, [HallUpdate]=@HallUpdate, [isDefault]=@isDefault, [MainAccessProfileId]=@MainAccessProfileId, [ModuleActivated]=@ModuleActivated, [StandardOperation]=@StandardOperation, [StandardOperationCreate]=@StandardOperationCreate, [StandardOperationDelete]=@StandardOperationDelete, [StandardOperationUpdate]=@StandardOperationUpdate, [SuperAdministrator]=@SuperAdministrator, [WorkArea]=@WorkArea, [WorkAreaCreate]=@WorkAreaCreate, [WorkAreaDelete]=@WorkAreaDelete, [WorkAreaUpdate]=@WorkAreaUpdate, [WorkPlan]=@WorkPlan, [WorkPlanCreate]=@WorkPlanCreate, [WorkPlanDelete]=@WorkPlanDelete, [WorkPlanReporting]=@WorkPlanReporting, [WorkPlanReportingCreate]=@WorkPlanReportingCreate, [WorkPlanReportingDelete]=@WorkPlanReportingDelete, [WorkPlanReportingUpdate]=@WorkPlanReportingUpdate, [WorkPlanUpdate]=@WorkPlanUpdate, [WorkStation]=@WorkStation, [WorkStationCreate]=@WorkStationCreate, [WorkStationDelete]=@WorkStationDelete, [WorkStationUpdate]=@WorkStationUpdate WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Access", item.Access);
				sqlCommand.Parameters.AddWithValue("AccessUpdate", item.AccessUpdate);
				sqlCommand.Parameters.AddWithValue("AdministrationAccessProfiles", item.AdministrationAccessProfiles);
				sqlCommand.Parameters.AddWithValue("AdministrationAccessProfilesUpdate", item.AdministrationAccessProfilesUpdate);
				sqlCommand.Parameters.AddWithValue("AdminstrationUser", item.AdminstrationUser);
				sqlCommand.Parameters.AddWithValue("AdminstrationUserUpdate", item.AdminstrationUserUpdate);
				sqlCommand.Parameters.AddWithValue("Country", item.Country);
				sqlCommand.Parameters.AddWithValue("CountryCreate", item.CountryCreate);
				sqlCommand.Parameters.AddWithValue("CountryDelete", item.CountryDelete);
				sqlCommand.Parameters.AddWithValue("CountryUpdate", item.CountryUpdate);
				sqlCommand.Parameters.AddWithValue("Departement", item.Departement);
				sqlCommand.Parameters.AddWithValue("DepartementCreate", item.DepartementCreate);
				sqlCommand.Parameters.AddWithValue("DepartementDelete", item.DepartementDelete);
				sqlCommand.Parameters.AddWithValue("DepartementUpdate", item.DepartementUpdate);
				sqlCommand.Parameters.AddWithValue("Hall", item.Hall);
				sqlCommand.Parameters.AddWithValue("HallCreate", item.HallCreate);
				sqlCommand.Parameters.AddWithValue("HallDelete", item.HallDelete);
				sqlCommand.Parameters.AddWithValue("HallUpdate", item.HallUpdate);
				sqlCommand.Parameters.AddWithValue("isDefault", item.isDefault == null ? (object)DBNull.Value : item.isDefault);
				sqlCommand.Parameters.AddWithValue("MainAccessProfileId", item.MainAccessProfileId);
				sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated);
				sqlCommand.Parameters.AddWithValue("StandardOperation", item.StandardOperation);
				sqlCommand.Parameters.AddWithValue("StandardOperationCreate", item.StandardOperationCreate);
				sqlCommand.Parameters.AddWithValue("StandardOperationDelete", item.StandardOperationDelete);
				sqlCommand.Parameters.AddWithValue("StandardOperationUpdate", item.StandardOperationUpdate);
				sqlCommand.Parameters.AddWithValue("SuperAdministrator", item.SuperAdministrator);
				sqlCommand.Parameters.AddWithValue("WorkArea", item.WorkArea);
				sqlCommand.Parameters.AddWithValue("WorkAreaCreate", item.WorkAreaCreate);
				sqlCommand.Parameters.AddWithValue("WorkAreaDelete", item.WorkAreaDelete);
				sqlCommand.Parameters.AddWithValue("WorkAreaUpdate", item.WorkAreaUpdate);
				sqlCommand.Parameters.AddWithValue("WorkPlan", item.WorkPlan);
				sqlCommand.Parameters.AddWithValue("WorkPlanCreate", item.WorkPlanCreate);
				sqlCommand.Parameters.AddWithValue("WorkPlanDelete", item.WorkPlanDelete);
				sqlCommand.Parameters.AddWithValue("WorkPlanReporting", item.WorkPlanReporting);
				sqlCommand.Parameters.AddWithValue("WorkPlanReportingCreate", item.WorkPlanReportingCreate);
				sqlCommand.Parameters.AddWithValue("WorkPlanReportingDelete", item.WorkPlanReportingDelete);
				sqlCommand.Parameters.AddWithValue("WorkPlanReportingUpdate", item.WorkPlanReportingUpdate);
				sqlCommand.Parameters.AddWithValue("WorkPlanUpdate", item.WorkPlanUpdate);
				sqlCommand.Parameters.AddWithValue("WorkStation", item.WorkStation);
				sqlCommand.Parameters.AddWithValue("WorkStationCreate", item.WorkStationCreate);
				sqlCommand.Parameters.AddWithValue("WorkStationDelete", item.WorkStationDelete);
				sqlCommand.Parameters.AddWithValue("WorkStationUpdate", item.WorkStationUpdate);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 43; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity> items)
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
						query += " UPDATE [WPL_AccessProfile] SET "

							+ "[Access]=@Access" + i + ","
							+ "[AccessUpdate]=@AccessUpdate" + i + ","
							+ "[AdministrationAccessProfiles]=@AdministrationAccessProfiles" + i + ","
							+ "[AdministrationAccessProfilesUpdate]=@AdministrationAccessProfilesUpdate" + i + ","
							+ "[AdminstrationUser]=@AdminstrationUser" + i + ","
							+ "[AdminstrationUserUpdate]=@AdminstrationUserUpdate" + i + ","
							+ "[Country]=@Country" + i + ","
							+ "[CountryCreate]=@CountryCreate" + i + ","
							+ "[CountryDelete]=@CountryDelete" + i + ","
							+ "[CountryUpdate]=@CountryUpdate" + i + ","
							+ "[Departement]=@Departement" + i + ","
							+ "[DepartementCreate]=@DepartementCreate" + i + ","
							+ "[DepartementDelete]=@DepartementDelete" + i + ","
							+ "[DepartementUpdate]=@DepartementUpdate" + i + ","
							+ "[Hall]=@Hall" + i + ","
							+ "[HallCreate]=@HallCreate" + i + ","
							+ "[HallDelete]=@HallDelete" + i + ","
							+ "[HallUpdate]=@HallUpdate" + i + ","
							+ "[isDefault]=@isDefault" + i + ","
							+ "[MainAccessProfileId]=@MainAccessProfileId" + i + ","
							+ "[ModuleActivated]=@ModuleActivated" + i + ","
							+ "[StandardOperation]=@StandardOperation" + i + ","
							+ "[StandardOperationCreate]=@StandardOperationCreate" + i + ","
							+ "[StandardOperationDelete]=@StandardOperationDelete" + i + ","
							+ "[StandardOperationUpdate]=@StandardOperationUpdate" + i + ","
							+ "[SuperAdministrator]=@SuperAdministrator" + i + ","
							+ "[WorkArea]=@WorkArea" + i + ","
							+ "[WorkAreaCreate]=@WorkAreaCreate" + i + ","
							+ "[WorkAreaDelete]=@WorkAreaDelete" + i + ","
							+ "[WorkAreaUpdate]=@WorkAreaUpdate" + i + ","
							+ "[WorkPlan]=@WorkPlan" + i + ","
							+ "[WorkPlanCreate]=@WorkPlanCreate" + i + ","
							+ "[WorkPlanDelete]=@WorkPlanDelete" + i + ","
							+ "[WorkPlanReporting]=@WorkPlanReporting" + i + ","
							+ "[WorkPlanReportingCreate]=@WorkPlanReportingCreate" + i + ","
							+ "[WorkPlanReportingDelete]=@WorkPlanReportingDelete" + i + ","
							+ "[WorkPlanReportingUpdate]=@WorkPlanReportingUpdate" + i + ","
							+ "[WorkPlanUpdate]=@WorkPlanUpdate" + i + ","
							+ "[WorkStation]=@WorkStation" + i + ","
							+ "[WorkStationCreate]=@WorkStationCreate" + i + ","
							+ "[WorkStationDelete]=@WorkStationDelete" + i + ","
							+ "[WorkStationUpdate]=@WorkStationUpdate" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("Access" + i, item.Access);
						sqlCommand.Parameters.AddWithValue("AccessUpdate" + i, item.AccessUpdate);
						sqlCommand.Parameters.AddWithValue("AdministrationAccessProfiles" + i, item.AdministrationAccessProfiles);
						sqlCommand.Parameters.AddWithValue("AdministrationAccessProfilesUpdate" + i, item.AdministrationAccessProfilesUpdate);
						sqlCommand.Parameters.AddWithValue("AdminstrationUser" + i, item.AdminstrationUser);
						sqlCommand.Parameters.AddWithValue("AdminstrationUserUpdate" + i, item.AdminstrationUserUpdate);
						sqlCommand.Parameters.AddWithValue("Country" + i, item.Country);
						sqlCommand.Parameters.AddWithValue("CountryCreate" + i, item.CountryCreate);
						sqlCommand.Parameters.AddWithValue("CountryDelete" + i, item.CountryDelete);
						sqlCommand.Parameters.AddWithValue("CountryUpdate" + i, item.CountryUpdate);
						sqlCommand.Parameters.AddWithValue("Departement" + i, item.Departement);
						sqlCommand.Parameters.AddWithValue("DepartementCreate" + i, item.DepartementCreate);
						sqlCommand.Parameters.AddWithValue("DepartementDelete" + i, item.DepartementDelete);
						sqlCommand.Parameters.AddWithValue("DepartementUpdate" + i, item.DepartementUpdate);
						sqlCommand.Parameters.AddWithValue("Hall" + i, item.Hall);
						sqlCommand.Parameters.AddWithValue("HallCreate" + i, item.HallCreate);
						sqlCommand.Parameters.AddWithValue("HallDelete" + i, item.HallDelete);
						sqlCommand.Parameters.AddWithValue("HallUpdate" + i, item.HallUpdate);
						sqlCommand.Parameters.AddWithValue("isDefault" + i, item.isDefault == null ? (object)DBNull.Value : item.isDefault);
						sqlCommand.Parameters.AddWithValue("MainAccessProfileId" + i, item.MainAccessProfileId);
						sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated);
						sqlCommand.Parameters.AddWithValue("StandardOperation" + i, item.StandardOperation);
						sqlCommand.Parameters.AddWithValue("StandardOperationCreate" + i, item.StandardOperationCreate);
						sqlCommand.Parameters.AddWithValue("StandardOperationDelete" + i, item.StandardOperationDelete);
						sqlCommand.Parameters.AddWithValue("StandardOperationUpdate" + i, item.StandardOperationUpdate);
						sqlCommand.Parameters.AddWithValue("SuperAdministrator" + i, item.SuperAdministrator);
						sqlCommand.Parameters.AddWithValue("WorkArea" + i, item.WorkArea);
						sqlCommand.Parameters.AddWithValue("WorkAreaCreate" + i, item.WorkAreaCreate);
						sqlCommand.Parameters.AddWithValue("WorkAreaDelete" + i, item.WorkAreaDelete);
						sqlCommand.Parameters.AddWithValue("WorkAreaUpdate" + i, item.WorkAreaUpdate);
						sqlCommand.Parameters.AddWithValue("WorkPlan" + i, item.WorkPlan);
						sqlCommand.Parameters.AddWithValue("WorkPlanCreate" + i, item.WorkPlanCreate);
						sqlCommand.Parameters.AddWithValue("WorkPlanDelete" + i, item.WorkPlanDelete);
						sqlCommand.Parameters.AddWithValue("WorkPlanReporting" + i, item.WorkPlanReporting);
						sqlCommand.Parameters.AddWithValue("WorkPlanReportingCreate" + i, item.WorkPlanReportingCreate);
						sqlCommand.Parameters.AddWithValue("WorkPlanReportingDelete" + i, item.WorkPlanReportingDelete);
						sqlCommand.Parameters.AddWithValue("WorkPlanReportingUpdate" + i, item.WorkPlanReportingUpdate);
						sqlCommand.Parameters.AddWithValue("WorkPlanUpdate" + i, item.WorkPlanUpdate);
						sqlCommand.Parameters.AddWithValue("WorkStation" + i, item.WorkStation);
						sqlCommand.Parameters.AddWithValue("WorkStationCreate" + i, item.WorkStationCreate);
						sqlCommand.Parameters.AddWithValue("WorkStationDelete" + i, item.WorkStationDelete);
						sqlCommand.Parameters.AddWithValue("WorkStationUpdate" + i, item.WorkStationUpdate);
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
				string query = "DELETE FROM [WPL_AccessProfile] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [WPL_AccessProfile] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [WPL_AccessProfile] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [WPL_AccessProfile]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [WPL_AccessProfile] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [WPL_AccessProfile] ([Access],[AccessUpdate],[AdministrationAccessProfiles],[AdministrationAccessProfilesUpdate],[AdminstrationUser],[AdminstrationUserUpdate],[Country],[CountryCreate],[CountryDelete],[CountryUpdate],[Departement],[DepartementCreate],[DepartementDelete],[DepartementUpdate],[Hall],[HallCreate],[HallDelete],[HallUpdate],[isDefault],[MainAccessProfileId],[ModuleActivated],[StandardOperation],[StandardOperationCreate],[StandardOperationDelete],[StandardOperationUpdate],[SuperAdministrator],[WorkArea],[WorkAreaCreate],[WorkAreaDelete],[WorkAreaUpdate],[WorkPlan],[WorkPlanCreate],[WorkPlanDelete],[WorkPlanReporting],[WorkPlanReportingCreate],[WorkPlanReportingDelete],[WorkPlanReportingUpdate],[WorkPlanUpdate],[WorkStation],[WorkStationCreate],[WorkStationDelete],[WorkStationUpdate]) OUTPUT INSERTED.[Id] VALUES (@Access,@AccessUpdate,@AdministrationAccessProfiles,@AdministrationAccessProfilesUpdate,@AdminstrationUser,@AdminstrationUserUpdate,@Country,@CountryCreate,@CountryDelete,@CountryUpdate,@Departement,@DepartementCreate,@DepartementDelete,@DepartementUpdate,@Hall,@HallCreate,@HallDelete,@HallUpdate,@isDefault,@MainAccessProfileId,@ModuleActivated,@StandardOperation,@StandardOperationCreate,@StandardOperationDelete,@StandardOperationUpdate,@SuperAdministrator,@WorkArea,@WorkAreaCreate,@WorkAreaDelete,@WorkAreaUpdate,@WorkPlan,@WorkPlanCreate,@WorkPlanDelete,@WorkPlanReporting,@WorkPlanReportingCreate,@WorkPlanReportingDelete,@WorkPlanReportingUpdate,@WorkPlanUpdate,@WorkStation,@WorkStationCreate,@WorkStationDelete,@WorkStationUpdate); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Access", item.Access);
			sqlCommand.Parameters.AddWithValue("AccessUpdate", item.AccessUpdate);
			sqlCommand.Parameters.AddWithValue("AdministrationAccessProfiles", item.AdministrationAccessProfiles);
			sqlCommand.Parameters.AddWithValue("AdministrationAccessProfilesUpdate", item.AdministrationAccessProfilesUpdate);
			sqlCommand.Parameters.AddWithValue("AdminstrationUser", item.AdminstrationUser);
			sqlCommand.Parameters.AddWithValue("AdminstrationUserUpdate", item.AdminstrationUserUpdate);
			sqlCommand.Parameters.AddWithValue("Country", item.Country);
			sqlCommand.Parameters.AddWithValue("CountryCreate", item.CountryCreate);
			sqlCommand.Parameters.AddWithValue("CountryDelete", item.CountryDelete);
			sqlCommand.Parameters.AddWithValue("CountryUpdate", item.CountryUpdate);
			sqlCommand.Parameters.AddWithValue("Departement", item.Departement);
			sqlCommand.Parameters.AddWithValue("DepartementCreate", item.DepartementCreate);
			sqlCommand.Parameters.AddWithValue("DepartementDelete", item.DepartementDelete);
			sqlCommand.Parameters.AddWithValue("DepartementUpdate", item.DepartementUpdate);
			sqlCommand.Parameters.AddWithValue("Hall", item.Hall);
			sqlCommand.Parameters.AddWithValue("HallCreate", item.HallCreate);
			sqlCommand.Parameters.AddWithValue("HallDelete", item.HallDelete);
			sqlCommand.Parameters.AddWithValue("HallUpdate", item.HallUpdate);
			sqlCommand.Parameters.AddWithValue("isDefault", item.isDefault == null ? (object)DBNull.Value : item.isDefault);
			sqlCommand.Parameters.AddWithValue("MainAccessProfileId", item.MainAccessProfileId);
			sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated);
			sqlCommand.Parameters.AddWithValue("StandardOperation", item.StandardOperation);
			sqlCommand.Parameters.AddWithValue("StandardOperationCreate", item.StandardOperationCreate);
			sqlCommand.Parameters.AddWithValue("StandardOperationDelete", item.StandardOperationDelete);
			sqlCommand.Parameters.AddWithValue("StandardOperationUpdate", item.StandardOperationUpdate);
			sqlCommand.Parameters.AddWithValue("SuperAdministrator", item.SuperAdministrator);
			sqlCommand.Parameters.AddWithValue("WorkArea", item.WorkArea);
			sqlCommand.Parameters.AddWithValue("WorkAreaCreate", item.WorkAreaCreate);
			sqlCommand.Parameters.AddWithValue("WorkAreaDelete", item.WorkAreaDelete);
			sqlCommand.Parameters.AddWithValue("WorkAreaUpdate", item.WorkAreaUpdate);
			sqlCommand.Parameters.AddWithValue("WorkPlan", item.WorkPlan);
			sqlCommand.Parameters.AddWithValue("WorkPlanCreate", item.WorkPlanCreate);
			sqlCommand.Parameters.AddWithValue("WorkPlanDelete", item.WorkPlanDelete);
			sqlCommand.Parameters.AddWithValue("WorkPlanReporting", item.WorkPlanReporting);
			sqlCommand.Parameters.AddWithValue("WorkPlanReportingCreate", item.WorkPlanReportingCreate);
			sqlCommand.Parameters.AddWithValue("WorkPlanReportingDelete", item.WorkPlanReportingDelete);
			sqlCommand.Parameters.AddWithValue("WorkPlanReportingUpdate", item.WorkPlanReportingUpdate);
			sqlCommand.Parameters.AddWithValue("WorkPlanUpdate", item.WorkPlanUpdate);
			sqlCommand.Parameters.AddWithValue("WorkStation", item.WorkStation);
			sqlCommand.Parameters.AddWithValue("WorkStationCreate", item.WorkStationCreate);
			sqlCommand.Parameters.AddWithValue("WorkStationDelete", item.WorkStationDelete);
			sqlCommand.Parameters.AddWithValue("WorkStationUpdate", item.WorkStationUpdate);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 43; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [WPL_AccessProfile] ([Access],[AccessUpdate],[AdministrationAccessProfiles],[AdministrationAccessProfilesUpdate],[AdminstrationUser],[AdminstrationUserUpdate],[Country],[CountryCreate],[CountryDelete],[CountryUpdate],[Departement],[DepartementCreate],[DepartementDelete],[DepartementUpdate],[Hall],[HallCreate],[HallDelete],[HallUpdate],[isDefault],[MainAccessProfileId],[ModuleActivated],[StandardOperation],[StandardOperationCreate],[StandardOperationDelete],[StandardOperationUpdate],[SuperAdministrator],[WorkArea],[WorkAreaCreate],[WorkAreaDelete],[WorkAreaUpdate],[WorkPlan],[WorkPlanCreate],[WorkPlanDelete],[WorkPlanReporting],[WorkPlanReportingCreate],[WorkPlanReportingDelete],[WorkPlanReportingUpdate],[WorkPlanUpdate],[WorkStation],[WorkStationCreate],[WorkStationDelete],[WorkStationUpdate]) VALUES ( "

						+ "@Access" + i + ","
						+ "@AccessUpdate" + i + ","
						+ "@AdministrationAccessProfiles" + i + ","
						+ "@AdministrationAccessProfilesUpdate" + i + ","
						+ "@AdminstrationUser" + i + ","
						+ "@AdminstrationUserUpdate" + i + ","
						+ "@Country" + i + ","
						+ "@CountryCreate" + i + ","
						+ "@CountryDelete" + i + ","
						+ "@CountryUpdate" + i + ","
						+ "@Departement" + i + ","
						+ "@DepartementCreate" + i + ","
						+ "@DepartementDelete" + i + ","
						+ "@DepartementUpdate" + i + ","
						+ "@Hall" + i + ","
						+ "@HallCreate" + i + ","
						+ "@HallDelete" + i + ","
						+ "@HallUpdate" + i + ","
						+ "@isDefault" + i + ","
						+ "@MainAccessProfileId" + i + ","
						+ "@ModuleActivated" + i + ","
						+ "@StandardOperation" + i + ","
						+ "@StandardOperationCreate" + i + ","
						+ "@StandardOperationDelete" + i + ","
						+ "@StandardOperationUpdate" + i + ","
						+ "@SuperAdministrator" + i + ","
						+ "@WorkArea" + i + ","
						+ "@WorkAreaCreate" + i + ","
						+ "@WorkAreaDelete" + i + ","
						+ "@WorkAreaUpdate" + i + ","
						+ "@WorkPlan" + i + ","
						+ "@WorkPlanCreate" + i + ","
						+ "@WorkPlanDelete" + i + ","
						+ "@WorkPlanReporting" + i + ","
						+ "@WorkPlanReportingCreate" + i + ","
						+ "@WorkPlanReportingDelete" + i + ","
						+ "@WorkPlanReportingUpdate" + i + ","
						+ "@WorkPlanUpdate" + i + ","
						+ "@WorkStation" + i + ","
						+ "@WorkStationCreate" + i + ","
						+ "@WorkStationDelete" + i + ","
						+ "@WorkStationUpdate" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Access" + i, item.Access);
					sqlCommand.Parameters.AddWithValue("AccessUpdate" + i, item.AccessUpdate);
					sqlCommand.Parameters.AddWithValue("AdministrationAccessProfiles" + i, item.AdministrationAccessProfiles);
					sqlCommand.Parameters.AddWithValue("AdministrationAccessProfilesUpdate" + i, item.AdministrationAccessProfilesUpdate);
					sqlCommand.Parameters.AddWithValue("AdminstrationUser" + i, item.AdminstrationUser);
					sqlCommand.Parameters.AddWithValue("AdminstrationUserUpdate" + i, item.AdminstrationUserUpdate);
					sqlCommand.Parameters.AddWithValue("Country" + i, item.Country);
					sqlCommand.Parameters.AddWithValue("CountryCreate" + i, item.CountryCreate);
					sqlCommand.Parameters.AddWithValue("CountryDelete" + i, item.CountryDelete);
					sqlCommand.Parameters.AddWithValue("CountryUpdate" + i, item.CountryUpdate);
					sqlCommand.Parameters.AddWithValue("Departement" + i, item.Departement);
					sqlCommand.Parameters.AddWithValue("DepartementCreate" + i, item.DepartementCreate);
					sqlCommand.Parameters.AddWithValue("DepartementDelete" + i, item.DepartementDelete);
					sqlCommand.Parameters.AddWithValue("DepartementUpdate" + i, item.DepartementUpdate);
					sqlCommand.Parameters.AddWithValue("Hall" + i, item.Hall);
					sqlCommand.Parameters.AddWithValue("HallCreate" + i, item.HallCreate);
					sqlCommand.Parameters.AddWithValue("HallDelete" + i, item.HallDelete);
					sqlCommand.Parameters.AddWithValue("HallUpdate" + i, item.HallUpdate);
					sqlCommand.Parameters.AddWithValue("isDefault" + i, item.isDefault == null ? (object)DBNull.Value : item.isDefault);
					sqlCommand.Parameters.AddWithValue("MainAccessProfileId" + i, item.MainAccessProfileId);
					sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated);
					sqlCommand.Parameters.AddWithValue("StandardOperation" + i, item.StandardOperation);
					sqlCommand.Parameters.AddWithValue("StandardOperationCreate" + i, item.StandardOperationCreate);
					sqlCommand.Parameters.AddWithValue("StandardOperationDelete" + i, item.StandardOperationDelete);
					sqlCommand.Parameters.AddWithValue("StandardOperationUpdate" + i, item.StandardOperationUpdate);
					sqlCommand.Parameters.AddWithValue("SuperAdministrator" + i, item.SuperAdministrator);
					sqlCommand.Parameters.AddWithValue("WorkArea" + i, item.WorkArea);
					sqlCommand.Parameters.AddWithValue("WorkAreaCreate" + i, item.WorkAreaCreate);
					sqlCommand.Parameters.AddWithValue("WorkAreaDelete" + i, item.WorkAreaDelete);
					sqlCommand.Parameters.AddWithValue("WorkAreaUpdate" + i, item.WorkAreaUpdate);
					sqlCommand.Parameters.AddWithValue("WorkPlan" + i, item.WorkPlan);
					sqlCommand.Parameters.AddWithValue("WorkPlanCreate" + i, item.WorkPlanCreate);
					sqlCommand.Parameters.AddWithValue("WorkPlanDelete" + i, item.WorkPlanDelete);
					sqlCommand.Parameters.AddWithValue("WorkPlanReporting" + i, item.WorkPlanReporting);
					sqlCommand.Parameters.AddWithValue("WorkPlanReportingCreate" + i, item.WorkPlanReportingCreate);
					sqlCommand.Parameters.AddWithValue("WorkPlanReportingDelete" + i, item.WorkPlanReportingDelete);
					sqlCommand.Parameters.AddWithValue("WorkPlanReportingUpdate" + i, item.WorkPlanReportingUpdate);
					sqlCommand.Parameters.AddWithValue("WorkPlanUpdate" + i, item.WorkPlanUpdate);
					sqlCommand.Parameters.AddWithValue("WorkStation" + i, item.WorkStation);
					sqlCommand.Parameters.AddWithValue("WorkStationCreate" + i, item.WorkStationCreate);
					sqlCommand.Parameters.AddWithValue("WorkStationDelete" + i, item.WorkStationDelete);
					sqlCommand.Parameters.AddWithValue("WorkStationUpdate" + i, item.WorkStationUpdate);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [WPL_AccessProfile] SET [Access]=@Access, [AccessUpdate]=@AccessUpdate, [AdministrationAccessProfiles]=@AdministrationAccessProfiles, [AdministrationAccessProfilesUpdate]=@AdministrationAccessProfilesUpdate, [AdminstrationUser]=@AdminstrationUser, [AdminstrationUserUpdate]=@AdminstrationUserUpdate, [Country]=@Country, [CountryCreate]=@CountryCreate, [CountryDelete]=@CountryDelete, [CountryUpdate]=@CountryUpdate, [Departement]=@Departement, [DepartementCreate]=@DepartementCreate, [DepartementDelete]=@DepartementDelete, [DepartementUpdate]=@DepartementUpdate, [Hall]=@Hall, [HallCreate]=@HallCreate, [HallDelete]=@HallDelete, [HallUpdate]=@HallUpdate, [isDefault]=@isDefault, [MainAccessProfileId]=@MainAccessProfileId, [ModuleActivated]=@ModuleActivated, [StandardOperation]=@StandardOperation, [StandardOperationCreate]=@StandardOperationCreate, [StandardOperationDelete]=@StandardOperationDelete, [StandardOperationUpdate]=@StandardOperationUpdate, [SuperAdministrator]=@SuperAdministrator, [WorkArea]=@WorkArea, [WorkAreaCreate]=@WorkAreaCreate, [WorkAreaDelete]=@WorkAreaDelete, [WorkAreaUpdate]=@WorkAreaUpdate, [WorkPlan]=@WorkPlan, [WorkPlanCreate]=@WorkPlanCreate, [WorkPlanDelete]=@WorkPlanDelete, [WorkPlanReporting]=@WorkPlanReporting, [WorkPlanReportingCreate]=@WorkPlanReportingCreate, [WorkPlanReportingDelete]=@WorkPlanReportingDelete, [WorkPlanReportingUpdate]=@WorkPlanReportingUpdate, [WorkPlanUpdate]=@WorkPlanUpdate, [WorkStation]=@WorkStation, [WorkStationCreate]=@WorkStationCreate, [WorkStationDelete]=@WorkStationDelete, [WorkStationUpdate]=@WorkStationUpdate WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("Access", item.Access);
			sqlCommand.Parameters.AddWithValue("AccessUpdate", item.AccessUpdate);
			sqlCommand.Parameters.AddWithValue("AdministrationAccessProfiles", item.AdministrationAccessProfiles);
			sqlCommand.Parameters.AddWithValue("AdministrationAccessProfilesUpdate", item.AdministrationAccessProfilesUpdate);
			sqlCommand.Parameters.AddWithValue("AdminstrationUser", item.AdminstrationUser);
			sqlCommand.Parameters.AddWithValue("AdminstrationUserUpdate", item.AdminstrationUserUpdate);
			sqlCommand.Parameters.AddWithValue("Country", item.Country);
			sqlCommand.Parameters.AddWithValue("CountryCreate", item.CountryCreate);
			sqlCommand.Parameters.AddWithValue("CountryDelete", item.CountryDelete);
			sqlCommand.Parameters.AddWithValue("CountryUpdate", item.CountryUpdate);
			sqlCommand.Parameters.AddWithValue("Departement", item.Departement);
			sqlCommand.Parameters.AddWithValue("DepartementCreate", item.DepartementCreate);
			sqlCommand.Parameters.AddWithValue("DepartementDelete", item.DepartementDelete);
			sqlCommand.Parameters.AddWithValue("DepartementUpdate", item.DepartementUpdate);
			sqlCommand.Parameters.AddWithValue("Hall", item.Hall);
			sqlCommand.Parameters.AddWithValue("HallCreate", item.HallCreate);
			sqlCommand.Parameters.AddWithValue("HallDelete", item.HallDelete);
			sqlCommand.Parameters.AddWithValue("HallUpdate", item.HallUpdate);
			sqlCommand.Parameters.AddWithValue("isDefault", item.isDefault == null ? (object)DBNull.Value : item.isDefault);
			sqlCommand.Parameters.AddWithValue("MainAccessProfileId", item.MainAccessProfileId);
			sqlCommand.Parameters.AddWithValue("ModuleActivated", item.ModuleActivated);
			sqlCommand.Parameters.AddWithValue("StandardOperation", item.StandardOperation);
			sqlCommand.Parameters.AddWithValue("StandardOperationCreate", item.StandardOperationCreate);
			sqlCommand.Parameters.AddWithValue("StandardOperationDelete", item.StandardOperationDelete);
			sqlCommand.Parameters.AddWithValue("StandardOperationUpdate", item.StandardOperationUpdate);
			sqlCommand.Parameters.AddWithValue("SuperAdministrator", item.SuperAdministrator);
			sqlCommand.Parameters.AddWithValue("WorkArea", item.WorkArea);
			sqlCommand.Parameters.AddWithValue("WorkAreaCreate", item.WorkAreaCreate);
			sqlCommand.Parameters.AddWithValue("WorkAreaDelete", item.WorkAreaDelete);
			sqlCommand.Parameters.AddWithValue("WorkAreaUpdate", item.WorkAreaUpdate);
			sqlCommand.Parameters.AddWithValue("WorkPlan", item.WorkPlan);
			sqlCommand.Parameters.AddWithValue("WorkPlanCreate", item.WorkPlanCreate);
			sqlCommand.Parameters.AddWithValue("WorkPlanDelete", item.WorkPlanDelete);
			sqlCommand.Parameters.AddWithValue("WorkPlanReporting", item.WorkPlanReporting);
			sqlCommand.Parameters.AddWithValue("WorkPlanReportingCreate", item.WorkPlanReportingCreate);
			sqlCommand.Parameters.AddWithValue("WorkPlanReportingDelete", item.WorkPlanReportingDelete);
			sqlCommand.Parameters.AddWithValue("WorkPlanReportingUpdate", item.WorkPlanReportingUpdate);
			sqlCommand.Parameters.AddWithValue("WorkPlanUpdate", item.WorkPlanUpdate);
			sqlCommand.Parameters.AddWithValue("WorkStation", item.WorkStation);
			sqlCommand.Parameters.AddWithValue("WorkStationCreate", item.WorkStationCreate);
			sqlCommand.Parameters.AddWithValue("WorkStationDelete", item.WorkStationDelete);
			sqlCommand.Parameters.AddWithValue("WorkStationUpdate", item.WorkStationUpdate);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 43; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [WPL_AccessProfile] SET "

					+ "[Access]=@Access" + i + ","
					+ "[AccessUpdate]=@AccessUpdate" + i + ","
					+ "[AdministrationAccessProfiles]=@AdministrationAccessProfiles" + i + ","
					+ "[AdministrationAccessProfilesUpdate]=@AdministrationAccessProfilesUpdate" + i + ","
					+ "[AdminstrationUser]=@AdminstrationUser" + i + ","
					+ "[AdminstrationUserUpdate]=@AdminstrationUserUpdate" + i + ","
					+ "[Country]=@Country" + i + ","
					+ "[CountryCreate]=@CountryCreate" + i + ","
					+ "[CountryDelete]=@CountryDelete" + i + ","
					+ "[CountryUpdate]=@CountryUpdate" + i + ","
					+ "[Departement]=@Departement" + i + ","
					+ "[DepartementCreate]=@DepartementCreate" + i + ","
					+ "[DepartementDelete]=@DepartementDelete" + i + ","
					+ "[DepartementUpdate]=@DepartementUpdate" + i + ","
					+ "[Hall]=@Hall" + i + ","
					+ "[HallCreate]=@HallCreate" + i + ","
					+ "[HallDelete]=@HallDelete" + i + ","
					+ "[HallUpdate]=@HallUpdate" + i + ","
					+ "[isDefault]=@isDefault" + i + ","
					+ "[MainAccessProfileId]=@MainAccessProfileId" + i + ","
					+ "[ModuleActivated]=@ModuleActivated" + i + ","
					+ "[StandardOperation]=@StandardOperation" + i + ","
					+ "[StandardOperationCreate]=@StandardOperationCreate" + i + ","
					+ "[StandardOperationDelete]=@StandardOperationDelete" + i + ","
					+ "[StandardOperationUpdate]=@StandardOperationUpdate" + i + ","
					+ "[SuperAdministrator]=@SuperAdministrator" + i + ","
					+ "[WorkArea]=@WorkArea" + i + ","
					+ "[WorkAreaCreate]=@WorkAreaCreate" + i + ","
					+ "[WorkAreaDelete]=@WorkAreaDelete" + i + ","
					+ "[WorkAreaUpdate]=@WorkAreaUpdate" + i + ","
					+ "[WorkPlan]=@WorkPlan" + i + ","
					+ "[WorkPlanCreate]=@WorkPlanCreate" + i + ","
					+ "[WorkPlanDelete]=@WorkPlanDelete" + i + ","
					+ "[WorkPlanReporting]=@WorkPlanReporting" + i + ","
					+ "[WorkPlanReportingCreate]=@WorkPlanReportingCreate" + i + ","
					+ "[WorkPlanReportingDelete]=@WorkPlanReportingDelete" + i + ","
					+ "[WorkPlanReportingUpdate]=@WorkPlanReportingUpdate" + i + ","
					+ "[WorkPlanUpdate]=@WorkPlanUpdate" + i + ","
					+ "[WorkStation]=@WorkStation" + i + ","
					+ "[WorkStationCreate]=@WorkStationCreate" + i + ","
					+ "[WorkStationDelete]=@WorkStationDelete" + i + ","
					+ "[WorkStationUpdate]=@WorkStationUpdate" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("Access" + i, item.Access);
					sqlCommand.Parameters.AddWithValue("AccessUpdate" + i, item.AccessUpdate);
					sqlCommand.Parameters.AddWithValue("AdministrationAccessProfiles" + i, item.AdministrationAccessProfiles);
					sqlCommand.Parameters.AddWithValue("AdministrationAccessProfilesUpdate" + i, item.AdministrationAccessProfilesUpdate);
					sqlCommand.Parameters.AddWithValue("AdminstrationUser" + i, item.AdminstrationUser);
					sqlCommand.Parameters.AddWithValue("AdminstrationUserUpdate" + i, item.AdminstrationUserUpdate);
					sqlCommand.Parameters.AddWithValue("Country" + i, item.Country);
					sqlCommand.Parameters.AddWithValue("CountryCreate" + i, item.CountryCreate);
					sqlCommand.Parameters.AddWithValue("CountryDelete" + i, item.CountryDelete);
					sqlCommand.Parameters.AddWithValue("CountryUpdate" + i, item.CountryUpdate);
					sqlCommand.Parameters.AddWithValue("Departement" + i, item.Departement);
					sqlCommand.Parameters.AddWithValue("DepartementCreate" + i, item.DepartementCreate);
					sqlCommand.Parameters.AddWithValue("DepartementDelete" + i, item.DepartementDelete);
					sqlCommand.Parameters.AddWithValue("DepartementUpdate" + i, item.DepartementUpdate);
					sqlCommand.Parameters.AddWithValue("Hall" + i, item.Hall);
					sqlCommand.Parameters.AddWithValue("HallCreate" + i, item.HallCreate);
					sqlCommand.Parameters.AddWithValue("HallDelete" + i, item.HallDelete);
					sqlCommand.Parameters.AddWithValue("HallUpdate" + i, item.HallUpdate);
					sqlCommand.Parameters.AddWithValue("isDefault" + i, item.isDefault == null ? (object)DBNull.Value : item.isDefault);
					sqlCommand.Parameters.AddWithValue("MainAccessProfileId" + i, item.MainAccessProfileId);
					sqlCommand.Parameters.AddWithValue("ModuleActivated" + i, item.ModuleActivated);
					sqlCommand.Parameters.AddWithValue("StandardOperation" + i, item.StandardOperation);
					sqlCommand.Parameters.AddWithValue("StandardOperationCreate" + i, item.StandardOperationCreate);
					sqlCommand.Parameters.AddWithValue("StandardOperationDelete" + i, item.StandardOperationDelete);
					sqlCommand.Parameters.AddWithValue("StandardOperationUpdate" + i, item.StandardOperationUpdate);
					sqlCommand.Parameters.AddWithValue("SuperAdministrator" + i, item.SuperAdministrator);
					sqlCommand.Parameters.AddWithValue("WorkArea" + i, item.WorkArea);
					sqlCommand.Parameters.AddWithValue("WorkAreaCreate" + i, item.WorkAreaCreate);
					sqlCommand.Parameters.AddWithValue("WorkAreaDelete" + i, item.WorkAreaDelete);
					sqlCommand.Parameters.AddWithValue("WorkAreaUpdate" + i, item.WorkAreaUpdate);
					sqlCommand.Parameters.AddWithValue("WorkPlan" + i, item.WorkPlan);
					sqlCommand.Parameters.AddWithValue("WorkPlanCreate" + i, item.WorkPlanCreate);
					sqlCommand.Parameters.AddWithValue("WorkPlanDelete" + i, item.WorkPlanDelete);
					sqlCommand.Parameters.AddWithValue("WorkPlanReporting" + i, item.WorkPlanReporting);
					sqlCommand.Parameters.AddWithValue("WorkPlanReportingCreate" + i, item.WorkPlanReportingCreate);
					sqlCommand.Parameters.AddWithValue("WorkPlanReportingDelete" + i, item.WorkPlanReportingDelete);
					sqlCommand.Parameters.AddWithValue("WorkPlanReportingUpdate" + i, item.WorkPlanReportingUpdate);
					sqlCommand.Parameters.AddWithValue("WorkPlanUpdate" + i, item.WorkPlanUpdate);
					sqlCommand.Parameters.AddWithValue("WorkStation" + i, item.WorkStation);
					sqlCommand.Parameters.AddWithValue("WorkStationCreate" + i, item.WorkStationCreate);
					sqlCommand.Parameters.AddWithValue("WorkStationDelete" + i, item.WorkStationDelete);
					sqlCommand.Parameters.AddWithValue("WorkStationUpdate" + i, item.WorkStationUpdate);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [WPL_AccessProfile] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [WPL_AccessProfile] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Entities.Tables.WPL.AccessProfileEntity> GetByMainAccessProfilesIds(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				var response = new List<Entities.Tables.WPL.AccessProfileEntity>();
				if(ids.Count <= maxQueryNumber)
				{
					response = getByMainAccessProfilesIds(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					response = new List<Entities.Tables.WPL.AccessProfileEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						response.AddRange(getByMainAccessProfilesIds(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					response.AddRange(getByMainAccessProfilesIds(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return response;
			}
			return new List<Entities.Tables.WPL.AccessProfileEntity>();
		}
		private static List<Entities.Tables.WPL.AccessProfileEntity> getByMainAccessProfilesIds(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();

				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
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

					sqlCommand.CommandText = "SELECT * FROM WPL_AccessProfile WHERE [MainAccessProfileId] IN (" + queryIds + ")";

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity>();
				}
			}
			return new List<Entities.Tables.WPL.AccessProfileEntity>();
		}
		public static int DeleteByMainAccessProfilesId(int id)
		{
			int r = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [WPL_AccessProfile] WHERE [MainAccessProfileId]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				r = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return r;
		}
		public static List<Entities.Tables.WPL.AccessProfileEntity> GetByUerId(int userId)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM WPL_AccessProfile";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("userId", userId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity> GetDefaultProfiles(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			using(var sqlCommand = new SqlCommand("SELECT * FROM[WPL_AccessProfile] WHERE[isDefault] = 1", connection,transaction))
			{
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.WPL.AccessProfileEntity>();
			}
		}
		#endregion Custom Methods

	}
}
