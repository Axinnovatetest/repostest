using Infrastructure.Data.Entities.Tables.FNC;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class BudgetProjectAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity Get(int id, bool isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_BudgetProject] WHERE [Id]=@Id AND [Archived]=@archived AND [Deleted]=@deleted";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity> Get(bool isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_BudgetProject] WHERE [Archived]=@archived AND [Deleted]=@deleted";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity> Get(List<int> ids, bool isArchived = false, bool isDeleted = false)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids, isArchived, isDeleted);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber), isArchived, isDeleted));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), isArchived, isDeleted));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity> get(List<int> ids, bool isArchived = false, bool isDeleted = false)
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

					sqlCommand.CommandText = $"SELECT * FROM [__FNC_BudgetProject] WHERE [Id] IN ({queryIds}) AND [Archived]=@archived AND [Deleted]=@deleted";
					sqlCommand.Parameters.AddWithValue("archived", isArchived);
					sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__FNC_BudgetProject] ([ApprovalTime],[ApprovalUserEmail],[ApprovalUserId],[ApprovalUserName],[Archived],[ArchiveTime],[ArchiveUserId],[BudgetYear],[Closed],[ClosedTime],[ClosedUserEmail],[ClosedUserId],[ClosedUserName],[CompanyId],[CompanyName],[CreationDate],[CreationUserEmail],[CreationUserId],[CreationUserName],[CurrencyId],[CurrencyName],[CustomerId],[CustomerName],[CustomerNr],[Deleted],[DeleteTime],[DeleteUserId],[DepartmentId],[DepartmentName],[Description],[Id_State],[Id_Type],[OrderCount],[ProjectBudget],[ProjectName],[ProjectStatus],[ProjectStatusChangeTime],[ProjectStatusChangeUserId],[ProjectStatusChangeUserName],[ProjectStatusName],[PSZOffer],[ResponsableEmail],[ResponsableId],[ResponsableName],[SiteLevelVisibility],[TotalSpent],[Type]) OUTPUT INSERTED.[Id] VALUES (@ApprovalTime,@ApprovalUserEmail,@ApprovalUserId,@ApprovalUserName,@Archived,@ArchiveTime,@ArchiveUserId,@BudgetYear,@Closed,@ClosedTime,@ClosedUserEmail,@ClosedUserId,@ClosedUserName,@CompanyId,@CompanyName,@CreationDate,@CreationUserEmail,@CreationUserId,@CreationUserName,@CurrencyId,@CurrencyName,@CustomerId,@CustomerName,@CustomerNr,@Deleted,@DeleteTime,@DeleteUserId,@DepartmentId,@DepartmentName,@Description,@Id_State,@Id_Type,@OrderCount,@ProjectBudget,@ProjectName,@ProjectStatus,@ProjectStatusChangeTime,@ProjectStatusChangeUserId,@ProjectStatusChangeUserName,@ProjectStatusName,@PSZOffer,@ResponsableEmail,@ResponsableId,@ResponsableName,@SiteLevelVisibility,@TotalSpent,@Type); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ApprovalTime", item.ApprovalTime == null ? (object)DBNull.Value : item.ApprovalTime);
					sqlCommand.Parameters.AddWithValue("ApprovalUserEmail", item.ApprovalUserEmail == null ? (object)DBNull.Value : item.ApprovalUserEmail);
					sqlCommand.Parameters.AddWithValue("ApprovalUserId", item.ApprovalUserId == null ? (object)DBNull.Value : item.ApprovalUserId);
					sqlCommand.Parameters.AddWithValue("ApprovalUserName", item.ApprovalUserName == null ? (object)DBNull.Value : item.ApprovalUserName);
					sqlCommand.Parameters.AddWithValue("Archived", item.Archived == null ? (object)DBNull.Value : item.Archived);
					sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
					sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
					sqlCommand.Parameters.AddWithValue("BudgetYear", item.BudgetYear);
					sqlCommand.Parameters.AddWithValue("Closed", item.Closed == null ? (object)DBNull.Value : item.Closed);
					sqlCommand.Parameters.AddWithValue("ClosedTime", item.ClosedTime == null ? (object)DBNull.Value : item.ClosedTime);
					sqlCommand.Parameters.AddWithValue("ClosedUserEmail", item.ClosedUserEmail == null ? (object)DBNull.Value : item.ClosedUserEmail);
					sqlCommand.Parameters.AddWithValue("ClosedUserId", item.ClosedUserId == null ? (object)DBNull.Value : item.ClosedUserId);
					sqlCommand.Parameters.AddWithValue("ClosedUserName", item.ClosedUserName == null ? (object)DBNull.Value : item.ClosedUserName);
					sqlCommand.Parameters.AddWithValue("CompanyId", item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
					sqlCommand.Parameters.AddWithValue("CompanyName", item.CompanyName == null ? (object)DBNull.Value : item.CompanyName);
					sqlCommand.Parameters.AddWithValue("CreationDate", item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
					sqlCommand.Parameters.AddWithValue("CreationUserEmail", item.CreationUserEmail == null ? (object)DBNull.Value : item.CreationUserEmail);
					sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CreationUserName", item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
					sqlCommand.Parameters.AddWithValue("CurrencyId", item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
					sqlCommand.Parameters.AddWithValue("CurrencyName", item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
					sqlCommand.Parameters.AddWithValue("CustomerId", item.CustomerId == null ? (object)DBNull.Value : item.CustomerId);
					sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("CustomerNr", item.CustomerNr == null ? (object)DBNull.Value : item.CustomerNr);
					sqlCommand.Parameters.AddWithValue("Deleted", item.Deleted == null ? (object)DBNull.Value : item.Deleted);
					sqlCommand.Parameters.AddWithValue("DeleteTime", item.DeleteTime == null ? (object)DBNull.Value : item.DeleteTime);
					sqlCommand.Parameters.AddWithValue("DeleteUserId", item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
					sqlCommand.Parameters.AddWithValue("DepartmentId", item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
					sqlCommand.Parameters.AddWithValue("DepartmentName", item.DepartmentName == null ? (object)DBNull.Value : item.DepartmentName);
					sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
					sqlCommand.Parameters.AddWithValue("Id_State", item.Id_State);
					sqlCommand.Parameters.AddWithValue("Id_Type", item.Id_Type);
					sqlCommand.Parameters.AddWithValue("OrderCount", item.OrderCount == null ? (object)DBNull.Value : item.OrderCount);
					sqlCommand.Parameters.AddWithValue("ProjectBudget", item.ProjectBudget);
					sqlCommand.Parameters.AddWithValue("ProjectName", item.ProjectName);
					sqlCommand.Parameters.AddWithValue("ProjectStatus", item.ProjectStatus == null ? (object)DBNull.Value : item.ProjectStatus);
					sqlCommand.Parameters.AddWithValue("ProjectStatusChangeTime", item.ProjectStatusChangeTime == null ? (object)DBNull.Value : item.ProjectStatusChangeTime);
					sqlCommand.Parameters.AddWithValue("ProjectStatusChangeUserId", item.ProjectStatusChangeUserId == null ? (object)DBNull.Value : item.ProjectStatusChangeUserId);
					sqlCommand.Parameters.AddWithValue("ProjectStatusChangeUserName", item.ProjectStatusChangeUserName == null ? (object)DBNull.Value : item.ProjectStatusChangeUserName);
					sqlCommand.Parameters.AddWithValue("ProjectStatusName", item.ProjectStatusName == null ? (object)DBNull.Value : item.ProjectStatusName);
					sqlCommand.Parameters.AddWithValue("PSZOffer", item.PSZOffer == null ? (object)DBNull.Value : item.PSZOffer);
					sqlCommand.Parameters.AddWithValue("ResponsableEmail", item.ResponsableEmail == null ? (object)DBNull.Value : item.ResponsableEmail);
					sqlCommand.Parameters.AddWithValue("ResponsableId", item.ResponsableId);
					sqlCommand.Parameters.AddWithValue("ResponsableName", item.ResponsableName == null ? (object)DBNull.Value : item.ResponsableName);
					sqlCommand.Parameters.AddWithValue("SiteLevelVisibility", item.SiteLevelVisibility == null ? (object)DBNull.Value : item.SiteLevelVisibility);
					sqlCommand.Parameters.AddWithValue("TotalSpent", item.TotalSpent == null ? (object)DBNull.Value : item.TotalSpent);
					sqlCommand.Parameters.AddWithValue("Type", item.Type == null ? (object)DBNull.Value : item.Type);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 48; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity> items)
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
						query += " INSERT INTO [__FNC_BudgetProject] ([ApprovalTime],[ApprovalUserEmail],[ApprovalUserId],[ApprovalUserName],[Archived],[ArchiveTime],[ArchiveUserId],[BudgetYear],[Closed],[ClosedTime],[ClosedUserEmail],[ClosedUserId],[ClosedUserName],[CompanyId],[CompanyName],[CreationDate],[CreationUserEmail],[CreationUserId],[CreationUserName],[CurrencyId],[CurrencyName],[CustomerId],[CustomerName],[CustomerNr],[Deleted],[DeleteTime],[DeleteUserId],[DepartmentId],[DepartmentName],[Description],[Id_State],[Id_Type],[OrderCount],[ProjectBudget],[ProjectName],[ProjectStatus],[ProjectStatusChangeTime],[ProjectStatusChangeUserId],[ProjectStatusChangeUserName],[ProjectStatusName],[PSZOffer],[ResponsableEmail],[ResponsableId],[ResponsableName],[SiteLevelVisibility],[TotalSpent],[Type]) VALUES ( "

							+ "@ApprovalTime" + i + ","
							+ "@ApprovalUserEmail" + i + ","
							+ "@ApprovalUserId" + i + ","
							+ "@ApprovalUserName" + i + ","
							+ "@Archived" + i + ","
							+ "@ArchiveTime" + i + ","
							+ "@ArchiveUserId" + i + ","
							+ "@BudgetYear" + i + ","
							+ "@Closed" + i + ","
							+ "@ClosedTime" + i + ","
							+ "@ClosedUserEmail" + i + ","
							+ "@ClosedUserId" + i + ","
							+ "@ClosedUserName" + i + ","
							+ "@CompanyId" + i + ","
							+ "@CompanyName" + i + ","
							+ "@CreationDate" + i + ","
							+ "@CreationUserEmail" + i + ","
							+ "@CreationUserId" + i + ","
							+ "@CreationUserName" + i + ","
							+ "@CurrencyId" + i + ","
							+ "@CurrencyName" + i + ","
							+ "@CustomerId" + i + ","
							+ "@CustomerName" + i + ","
							+ "@CustomerNr" + i + ","
							+ "@Deleted" + i + ","
							+ "@DeleteTime" + i + ","
							+ "@DeleteUserId" + i + ","
							+ "@DepartmentId" + i + ","
							+ "@DepartmentName" + i + ","
							+ "@Description" + i + ","
							+ "@Id_State" + i + ","
							+ "@Id_Type" + i + ","
							+ "@OrderCount" + i + ","
							+ "@ProjectBudget" + i + ","
							+ "@ProjectName" + i + ","
							+ "@ProjectStatus" + i + ","
							+ "@ProjectStatusChangeTime" + i + ","
							+ "@ProjectStatusChangeUserId" + i + ","
							+ "@ProjectStatusChangeUserName" + i + ","
							+ "@ProjectStatusName" + i + ","
							+ "@PSZOffer" + i + ","
							+ "@ResponsableEmail" + i + ","
							+ "@ResponsableId" + i + ","
							+ "@ResponsableName" + i + ","
							+ "@SiteLevelVisibility" + i + ","
							+ "@TotalSpent" + i + ","
							+ "@Type" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ApprovalTime" + i, item.ApprovalTime == null ? (object)DBNull.Value : item.ApprovalTime);
						sqlCommand.Parameters.AddWithValue("ApprovalUserEmail" + i, item.ApprovalUserEmail == null ? (object)DBNull.Value : item.ApprovalUserEmail);
						sqlCommand.Parameters.AddWithValue("ApprovalUserId" + i, item.ApprovalUserId == null ? (object)DBNull.Value : item.ApprovalUserId);
						sqlCommand.Parameters.AddWithValue("ApprovalUserName" + i, item.ApprovalUserName == null ? (object)DBNull.Value : item.ApprovalUserName);
						sqlCommand.Parameters.AddWithValue("Archived" + i, item.Archived == null ? (object)DBNull.Value : item.Archived);
						sqlCommand.Parameters.AddWithValue("ArchiveTime" + i, item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
						sqlCommand.Parameters.AddWithValue("ArchiveUserId" + i, item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
						sqlCommand.Parameters.AddWithValue("BudgetYear" + i, item.BudgetYear);
						sqlCommand.Parameters.AddWithValue("Closed" + i, item.Closed == null ? (object)DBNull.Value : item.Closed);
						sqlCommand.Parameters.AddWithValue("ClosedTime" + i, item.ClosedTime == null ? (object)DBNull.Value : item.ClosedTime);
						sqlCommand.Parameters.AddWithValue("ClosedUserEmail" + i, item.ClosedUserEmail == null ? (object)DBNull.Value : item.ClosedUserEmail);
						sqlCommand.Parameters.AddWithValue("ClosedUserId" + i, item.ClosedUserId == null ? (object)DBNull.Value : item.ClosedUserId);
						sqlCommand.Parameters.AddWithValue("ClosedUserName" + i, item.ClosedUserName == null ? (object)DBNull.Value : item.ClosedUserName);
						sqlCommand.Parameters.AddWithValue("CompanyId" + i, item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
						sqlCommand.Parameters.AddWithValue("CompanyName" + i, item.CompanyName == null ? (object)DBNull.Value : item.CompanyName);
						sqlCommand.Parameters.AddWithValue("CreationDate" + i, item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
						sqlCommand.Parameters.AddWithValue("CreationUserEmail" + i, item.CreationUserEmail == null ? (object)DBNull.Value : item.CreationUserEmail);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("CreationUserName" + i, item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
						sqlCommand.Parameters.AddWithValue("CurrencyId" + i, item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
						sqlCommand.Parameters.AddWithValue("CurrencyName" + i, item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
						sqlCommand.Parameters.AddWithValue("CustomerId" + i, item.CustomerId == null ? (object)DBNull.Value : item.CustomerId);
						sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
						sqlCommand.Parameters.AddWithValue("CustomerNr" + i, item.CustomerNr == null ? (object)DBNull.Value : item.CustomerNr);
						sqlCommand.Parameters.AddWithValue("Deleted" + i, item.Deleted == null ? (object)DBNull.Value : item.Deleted);
						sqlCommand.Parameters.AddWithValue("DeleteTime" + i, item.DeleteTime == null ? (object)DBNull.Value : item.DeleteTime);
						sqlCommand.Parameters.AddWithValue("DeleteUserId" + i, item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
						sqlCommand.Parameters.AddWithValue("DepartmentId" + i, item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
						sqlCommand.Parameters.AddWithValue("DepartmentName" + i, item.DepartmentName == null ? (object)DBNull.Value : item.DepartmentName);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("Id_State" + i, item.Id_State);
						sqlCommand.Parameters.AddWithValue("Id_Type" + i, item.Id_Type);
						sqlCommand.Parameters.AddWithValue("OrderCount" + i, item.OrderCount == null ? (object)DBNull.Value : item.OrderCount);
						sqlCommand.Parameters.AddWithValue("ProjectBudget" + i, item.ProjectBudget);
						sqlCommand.Parameters.AddWithValue("ProjectName" + i, item.ProjectName);
						sqlCommand.Parameters.AddWithValue("ProjectStatus" + i, item.ProjectStatus == null ? (object)DBNull.Value : item.ProjectStatus);
						sqlCommand.Parameters.AddWithValue("ProjectStatusChangeTime" + i, item.ProjectStatusChangeTime == null ? (object)DBNull.Value : item.ProjectStatusChangeTime);
						sqlCommand.Parameters.AddWithValue("ProjectStatusChangeUserId" + i, item.ProjectStatusChangeUserId == null ? (object)DBNull.Value : item.ProjectStatusChangeUserId);
						sqlCommand.Parameters.AddWithValue("ProjectStatusChangeUserName" + i, item.ProjectStatusChangeUserName == null ? (object)DBNull.Value : item.ProjectStatusChangeUserName);
						sqlCommand.Parameters.AddWithValue("ProjectStatusName" + i, item.ProjectStatusName == null ? (object)DBNull.Value : item.ProjectStatusName);
						sqlCommand.Parameters.AddWithValue("PSZOffer" + i, item.PSZOffer == null ? (object)DBNull.Value : item.PSZOffer);
						sqlCommand.Parameters.AddWithValue("ResponsableEmail" + i, item.ResponsableEmail == null ? (object)DBNull.Value : item.ResponsableEmail);
						sqlCommand.Parameters.AddWithValue("ResponsableId" + i, item.ResponsableId);
						sqlCommand.Parameters.AddWithValue("ResponsableName" + i, item.ResponsableName == null ? (object)DBNull.Value : item.ResponsableName);
						sqlCommand.Parameters.AddWithValue("SiteLevelVisibility" + i, item.SiteLevelVisibility == null ? (object)DBNull.Value : item.SiteLevelVisibility);
						sqlCommand.Parameters.AddWithValue("TotalSpent" + i, item.TotalSpent == null ? (object)DBNull.Value : item.TotalSpent);
						sqlCommand.Parameters.AddWithValue("Type" + i, item.Type == null ? (object)DBNull.Value : item.Type);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_BudgetProject] SET [ApprovalTime]=@ApprovalTime, [ApprovalUserEmail]=@ApprovalUserEmail, [ApprovalUserId]=@ApprovalUserId, [ApprovalUserName]=@ApprovalUserName, [Archived]=@Archived, [ArchiveTime]=@ArchiveTime, [ArchiveUserId]=@ArchiveUserId, [BudgetYear]=@BudgetYear, [Closed]=@Closed, [ClosedTime]=@ClosedTime, [ClosedUserEmail]=@ClosedUserEmail, [ClosedUserId]=@ClosedUserId, [ClosedUserName]=@ClosedUserName, [CompanyId]=@CompanyId, [CompanyName]=@CompanyName, [CreationDate]=@CreationDate, [CreationUserEmail]=@CreationUserEmail, [CreationUserId]=@CreationUserId, [CreationUserName]=@CreationUserName, [CurrencyId]=@CurrencyId, [CurrencyName]=@CurrencyName, [CustomerId]=@CustomerId, [CustomerName]=@CustomerName, [CustomerNr]=@CustomerNr, [Deleted]=@Deleted, [DeleteTime]=@DeleteTime, [DeleteUserId]=@DeleteUserId, [DepartmentId]=@DepartmentId, [DepartmentName]=@DepartmentName, [Description]=@Description, [Id_State]=@Id_State, [Id_Type]=@Id_Type, [OrderCount]=@OrderCount, [ProjectBudget]=@ProjectBudget, [ProjectName]=@ProjectName, [ProjectStatus]=@ProjectStatus, [ProjectStatusChangeTime]=@ProjectStatusChangeTime, [ProjectStatusChangeUserId]=@ProjectStatusChangeUserId, [ProjectStatusChangeUserName]=@ProjectStatusChangeUserName, [ProjectStatusName]=@ProjectStatusName, [PSZOffer]=@PSZOffer, [ResponsableEmail]=@ResponsableEmail, [ResponsableId]=@ResponsableId, [ResponsableName]=@ResponsableName, [SiteLevelVisibility]=@SiteLevelVisibility, [TotalSpent]=@TotalSpent, [Type]=@Type WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ApprovalTime", item.ApprovalTime == null ? (object)DBNull.Value : item.ApprovalTime);
				sqlCommand.Parameters.AddWithValue("ApprovalUserEmail", item.ApprovalUserEmail == null ? (object)DBNull.Value : item.ApprovalUserEmail);
				sqlCommand.Parameters.AddWithValue("ApprovalUserId", item.ApprovalUserId == null ? (object)DBNull.Value : item.ApprovalUserId);
				sqlCommand.Parameters.AddWithValue("ApprovalUserName", item.ApprovalUserName == null ? (object)DBNull.Value : item.ApprovalUserName);
				sqlCommand.Parameters.AddWithValue("Archived", item.Archived == null ? (object)DBNull.Value : item.Archived);
				sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
				sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
				sqlCommand.Parameters.AddWithValue("BudgetYear", item.BudgetYear);
				sqlCommand.Parameters.AddWithValue("Closed", item.Closed == null ? (object)DBNull.Value : item.Closed);
				sqlCommand.Parameters.AddWithValue("ClosedTime", item.ClosedTime == null ? (object)DBNull.Value : item.ClosedTime);
				sqlCommand.Parameters.AddWithValue("ClosedUserEmail", item.ClosedUserEmail == null ? (object)DBNull.Value : item.ClosedUserEmail);
				sqlCommand.Parameters.AddWithValue("ClosedUserId", item.ClosedUserId == null ? (object)DBNull.Value : item.ClosedUserId);
				sqlCommand.Parameters.AddWithValue("ClosedUserName", item.ClosedUserName == null ? (object)DBNull.Value : item.ClosedUserName);
				sqlCommand.Parameters.AddWithValue("CompanyId", item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
				sqlCommand.Parameters.AddWithValue("CompanyName", item.CompanyName == null ? (object)DBNull.Value : item.CompanyName);
				sqlCommand.Parameters.AddWithValue("CreationDate", item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
				sqlCommand.Parameters.AddWithValue("CreationUserEmail", item.CreationUserEmail == null ? (object)DBNull.Value : item.CreationUserEmail);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("CreationUserName", item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
				sqlCommand.Parameters.AddWithValue("CurrencyId", item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
				sqlCommand.Parameters.AddWithValue("CurrencyName", item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
				sqlCommand.Parameters.AddWithValue("CustomerId", item.CustomerId == null ? (object)DBNull.Value : item.CustomerId);
				sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
				sqlCommand.Parameters.AddWithValue("CustomerNr", item.CustomerNr == null ? (object)DBNull.Value : item.CustomerNr);
				sqlCommand.Parameters.AddWithValue("Deleted", item.Deleted == null ? (object)DBNull.Value : item.Deleted);
				sqlCommand.Parameters.AddWithValue("DeleteTime", item.DeleteTime == null ? (object)DBNull.Value : item.DeleteTime);
				sqlCommand.Parameters.AddWithValue("DeleteUserId", item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
				sqlCommand.Parameters.AddWithValue("DepartmentId", item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
				sqlCommand.Parameters.AddWithValue("DepartmentName", item.DepartmentName == null ? (object)DBNull.Value : item.DepartmentName);
				sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
				sqlCommand.Parameters.AddWithValue("Id_State", item.Id_State);
				sqlCommand.Parameters.AddWithValue("Id_Type", item.Id_Type);
				sqlCommand.Parameters.AddWithValue("OrderCount", item.OrderCount == null ? (object)DBNull.Value : item.OrderCount);
				sqlCommand.Parameters.AddWithValue("ProjectBudget", item.ProjectBudget);
				sqlCommand.Parameters.AddWithValue("ProjectName", item.ProjectName);
				sqlCommand.Parameters.AddWithValue("ProjectStatus", item.ProjectStatus == null ? (object)DBNull.Value : item.ProjectStatus);
				sqlCommand.Parameters.AddWithValue("ProjectStatusChangeTime", item.ProjectStatusChangeTime == null ? (object)DBNull.Value : item.ProjectStatusChangeTime);
				sqlCommand.Parameters.AddWithValue("ProjectStatusChangeUserId", item.ProjectStatusChangeUserId == null ? (object)DBNull.Value : item.ProjectStatusChangeUserId);
				sqlCommand.Parameters.AddWithValue("ProjectStatusChangeUserName", item.ProjectStatusChangeUserName == null ? (object)DBNull.Value : item.ProjectStatusChangeUserName);
				sqlCommand.Parameters.AddWithValue("ProjectStatusName", item.ProjectStatusName == null ? (object)DBNull.Value : item.ProjectStatusName);
				sqlCommand.Parameters.AddWithValue("PSZOffer", item.PSZOffer == null ? (object)DBNull.Value : item.PSZOffer);
				sqlCommand.Parameters.AddWithValue("ResponsableEmail", item.ResponsableEmail == null ? (object)DBNull.Value : item.ResponsableEmail);
				sqlCommand.Parameters.AddWithValue("ResponsableId", item.ResponsableId);
				sqlCommand.Parameters.AddWithValue("ResponsableName", item.ResponsableName == null ? (object)DBNull.Value : item.ResponsableName);
				sqlCommand.Parameters.AddWithValue("SiteLevelVisibility", item.SiteLevelVisibility == null ? (object)DBNull.Value : item.SiteLevelVisibility);
				sqlCommand.Parameters.AddWithValue("TotalSpent", item.TotalSpent == null ? (object)DBNull.Value : item.TotalSpent);
				sqlCommand.Parameters.AddWithValue("Type", item.Type == null ? (object)DBNull.Value : item.Type);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 48; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity> items)
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
						query += " UPDATE [__FNC_BudgetProject] SET "

							+ "[ApprovalTime]=@ApprovalTime" + i + ","
							+ "[ApprovalUserEmail]=@ApprovalUserEmail" + i + ","
							+ "[ApprovalUserId]=@ApprovalUserId" + i + ","
							+ "[ApprovalUserName]=@ApprovalUserName" + i + ","
							+ "[Archived]=@Archived" + i + ","
							+ "[ArchiveTime]=@ArchiveTime" + i + ","
							+ "[ArchiveUserId]=@ArchiveUserId" + i + ","
							+ "[BudgetYear]=@BudgetYear" + i + ","
							+ "[Closed]=@Closed" + i + ","
							+ "[ClosedTime]=@ClosedTime" + i + ","
							+ "[ClosedUserEmail]=@ClosedUserEmail" + i + ","
							+ "[ClosedUserId]=@ClosedUserId" + i + ","
							+ "[ClosedUserName]=@ClosedUserName" + i + ","
							+ "[CompanyId]=@CompanyId" + i + ","
							+ "[CompanyName]=@CompanyName" + i + ","
							+ "[CreationDate]=@CreationDate" + i + ","
							+ "[CreationUserEmail]=@CreationUserEmail" + i + ","
							+ "[CreationUserId]=@CreationUserId" + i + ","
							+ "[CreationUserName]=@CreationUserName" + i + ","
							+ "[CurrencyId]=@CurrencyId" + i + ","
							+ "[CurrencyName]=@CurrencyName" + i + ","
							+ "[CustomerId]=@CustomerId" + i + ","
							+ "[CustomerName]=@CustomerName" + i + ","
							+ "[CustomerNr]=@CustomerNr" + i + ","
							+ "[Deleted]=@Deleted" + i + ","
							+ "[DeleteTime]=@DeleteTime" + i + ","
							+ "[DeleteUserId]=@DeleteUserId" + i + ","
							+ "[DepartmentId]=@DepartmentId" + i + ","
							+ "[DepartmentName]=@DepartmentName" + i + ","
							+ "[Description]=@Description" + i + ","
							+ "[Id_State]=@Id_State" + i + ","
							+ "[Id_Type]=@Id_Type" + i + ","
							+ "[OrderCount]=@OrderCount" + i + ","
							+ "[ProjectBudget]=@ProjectBudget" + i + ","
							+ "[ProjectName]=@ProjectName" + i + ","
							+ "[ProjectStatus]=@ProjectStatus" + i + ","
							+ "[ProjectStatusChangeTime]=@ProjectStatusChangeTime" + i + ","
							+ "[ProjectStatusChangeUserId]=@ProjectStatusChangeUserId" + i + ","
							+ "[ProjectStatusChangeUserName]=@ProjectStatusChangeUserName" + i + ","
							+ "[ProjectStatusName]=@ProjectStatusName" + i + ","
							+ "[PSZOffer]=@PSZOffer" + i + ","
							+ "[ResponsableEmail]=@ResponsableEmail" + i + ","
							+ "[ResponsableId]=@ResponsableId" + i + ","
							+ "[ResponsableName]=@ResponsableName" + i + ","
							+ "[SiteLevelVisibility]=@SiteLevelVisibility" + i + ","
							+ "[TotalSpent]=@TotalSpent" + i + ","
							+ "[Type]=@Type" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ApprovalTime" + i, item.ApprovalTime == null ? (object)DBNull.Value : item.ApprovalTime);
						sqlCommand.Parameters.AddWithValue("ApprovalUserEmail" + i, item.ApprovalUserEmail == null ? (object)DBNull.Value : item.ApprovalUserEmail);
						sqlCommand.Parameters.AddWithValue("ApprovalUserId" + i, item.ApprovalUserId == null ? (object)DBNull.Value : item.ApprovalUserId);
						sqlCommand.Parameters.AddWithValue("ApprovalUserName" + i, item.ApprovalUserName == null ? (object)DBNull.Value : item.ApprovalUserName);
						sqlCommand.Parameters.AddWithValue("Archived" + i, item.Archived == null ? (object)DBNull.Value : item.Archived);
						sqlCommand.Parameters.AddWithValue("ArchiveTime" + i, item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
						sqlCommand.Parameters.AddWithValue("ArchiveUserId" + i, item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
						sqlCommand.Parameters.AddWithValue("BudgetYear" + i, item.BudgetYear);
						sqlCommand.Parameters.AddWithValue("Closed" + i, item.Closed == null ? (object)DBNull.Value : item.Closed);
						sqlCommand.Parameters.AddWithValue("ClosedTime" + i, item.ClosedTime == null ? (object)DBNull.Value : item.ClosedTime);
						sqlCommand.Parameters.AddWithValue("ClosedUserEmail" + i, item.ClosedUserEmail == null ? (object)DBNull.Value : item.ClosedUserEmail);
						sqlCommand.Parameters.AddWithValue("ClosedUserId" + i, item.ClosedUserId == null ? (object)DBNull.Value : item.ClosedUserId);
						sqlCommand.Parameters.AddWithValue("ClosedUserName" + i, item.ClosedUserName == null ? (object)DBNull.Value : item.ClosedUserName);
						sqlCommand.Parameters.AddWithValue("CompanyId" + i, item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
						sqlCommand.Parameters.AddWithValue("CompanyName" + i, item.CompanyName == null ? (object)DBNull.Value : item.CompanyName);
						sqlCommand.Parameters.AddWithValue("CreationDate" + i, item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
						sqlCommand.Parameters.AddWithValue("CreationUserEmail" + i, item.CreationUserEmail == null ? (object)DBNull.Value : item.CreationUserEmail);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("CreationUserName" + i, item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
						sqlCommand.Parameters.AddWithValue("CurrencyId" + i, item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
						sqlCommand.Parameters.AddWithValue("CurrencyName" + i, item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
						sqlCommand.Parameters.AddWithValue("CustomerId" + i, item.CustomerId == null ? (object)DBNull.Value : item.CustomerId);
						sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
						sqlCommand.Parameters.AddWithValue("CustomerNr" + i, item.CustomerNr == null ? (object)DBNull.Value : item.CustomerNr);
						sqlCommand.Parameters.AddWithValue("Deleted" + i, item.Deleted == null ? (object)DBNull.Value : item.Deleted);
						sqlCommand.Parameters.AddWithValue("DeleteTime" + i, item.DeleteTime == null ? (object)DBNull.Value : item.DeleteTime);
						sqlCommand.Parameters.AddWithValue("DeleteUserId" + i, item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
						sqlCommand.Parameters.AddWithValue("DepartmentId" + i, item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
						sqlCommand.Parameters.AddWithValue("DepartmentName" + i, item.DepartmentName == null ? (object)DBNull.Value : item.DepartmentName);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("Id_State" + i, item.Id_State);
						sqlCommand.Parameters.AddWithValue("Id_Type" + i, item.Id_Type);
						sqlCommand.Parameters.AddWithValue("OrderCount" + i, item.OrderCount == null ? (object)DBNull.Value : item.OrderCount);
						sqlCommand.Parameters.AddWithValue("ProjectBudget" + i, item.ProjectBudget);
						sqlCommand.Parameters.AddWithValue("ProjectName" + i, item.ProjectName);
						sqlCommand.Parameters.AddWithValue("ProjectStatus" + i, item.ProjectStatus == null ? (object)DBNull.Value : item.ProjectStatus);
						sqlCommand.Parameters.AddWithValue("ProjectStatusChangeTime" + i, item.ProjectStatusChangeTime == null ? (object)DBNull.Value : item.ProjectStatusChangeTime);
						sqlCommand.Parameters.AddWithValue("ProjectStatusChangeUserId" + i, item.ProjectStatusChangeUserId == null ? (object)DBNull.Value : item.ProjectStatusChangeUserId);
						sqlCommand.Parameters.AddWithValue("ProjectStatusChangeUserName" + i, item.ProjectStatusChangeUserName == null ? (object)DBNull.Value : item.ProjectStatusChangeUserName);
						sqlCommand.Parameters.AddWithValue("ProjectStatusName" + i, item.ProjectStatusName == null ? (object)DBNull.Value : item.ProjectStatusName);
						sqlCommand.Parameters.AddWithValue("PSZOffer" + i, item.PSZOffer == null ? (object)DBNull.Value : item.PSZOffer);
						sqlCommand.Parameters.AddWithValue("ResponsableEmail" + i, item.ResponsableEmail == null ? (object)DBNull.Value : item.ResponsableEmail);
						sqlCommand.Parameters.AddWithValue("ResponsableId" + i, item.ResponsableId);
						sqlCommand.Parameters.AddWithValue("ResponsableName" + i, item.ResponsableName == null ? (object)DBNull.Value : item.ResponsableName);
						sqlCommand.Parameters.AddWithValue("SiteLevelVisibility" + i, item.SiteLevelVisibility == null ? (object)DBNull.Value : item.SiteLevelVisibility);
						sqlCommand.Parameters.AddWithValue("TotalSpent" + i, item.TotalSpent == null ? (object)DBNull.Value : item.TotalSpent);
						sqlCommand.Parameters.AddWithValue("Type" + i, item.Type == null ? (object)DBNull.Value : item.Type);
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
				string query = "DELETE FROM [__FNC_BudgetProject] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__FNC_BudgetProject] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__FNC_BudgetProject] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__FNC_BudgetProject]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__FNC_BudgetProject] WHERE [Id] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [__FNC_BudgetProject] ([ApprovalTime],[ApprovalUserEmail],[ApprovalUserId],[ApprovalUserName],[Archived],[ArchiveTime],[ArchiveUserId],[BudgetYear],[Closed],[ClosedTime],[ClosedUserEmail],[ClosedUserId],[ClosedUserName],[CompanyId],[CompanyName],[CreationDate],[CreationUserEmail],[CreationUserId],[CreationUserName],[CurrencyId],[CurrencyName],[CustomerId],[CustomerName],[CustomerNr],[Deleted],[DeleteTime],[DeleteUserId],[DepartmentId],[DepartmentName],[Description],[Id_State],[Id_Type],[OrderCount],[ProjectBudget],[ProjectName],[ProjectStatus],[ProjectStatusChangeTime],[ProjectStatusChangeUserId],[ProjectStatusChangeUserName],[ProjectStatusName],[PSZOffer],[ResponsableEmail],[ResponsableId],[ResponsableName],[SiteLevelVisibility],[TotalSpent],[Type]) OUTPUT INSERTED.[Id] VALUES (@ApprovalTime,@ApprovalUserEmail,@ApprovalUserId,@ApprovalUserName,@Archived,@ArchiveTime,@ArchiveUserId,@BudgetYear,@Closed,@ClosedTime,@ClosedUserEmail,@ClosedUserId,@ClosedUserName,@CompanyId,@CompanyName,@CreationDate,@CreationUserEmail,@CreationUserId,@CreationUserName,@CurrencyId,@CurrencyName,@CustomerId,@CustomerName,@CustomerNr,@Deleted,@DeleteTime,@DeleteUserId,@DepartmentId,@DepartmentName,@Description,@Id_State,@Id_Type,@OrderCount,@ProjectBudget,@ProjectName,@ProjectStatus,@ProjectStatusChangeTime,@ProjectStatusChangeUserId,@ProjectStatusChangeUserName,@ProjectStatusName,@PSZOffer,@ResponsableEmail,@ResponsableId,@ResponsableName,@SiteLevelVisibility,@TotalSpent,@Type); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ApprovalTime", item.ApprovalTime == null ? (object)DBNull.Value : item.ApprovalTime);
			sqlCommand.Parameters.AddWithValue("ApprovalUserEmail", item.ApprovalUserEmail == null ? (object)DBNull.Value : item.ApprovalUserEmail);
			sqlCommand.Parameters.AddWithValue("ApprovalUserId", item.ApprovalUserId == null ? (object)DBNull.Value : item.ApprovalUserId);
			sqlCommand.Parameters.AddWithValue("ApprovalUserName", item.ApprovalUserName == null ? (object)DBNull.Value : item.ApprovalUserName);
			sqlCommand.Parameters.AddWithValue("Archived", item.Archived == null ? (object)DBNull.Value : item.Archived);
			sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
			sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
			sqlCommand.Parameters.AddWithValue("BudgetYear", item.BudgetYear);
			sqlCommand.Parameters.AddWithValue("Closed", item.Closed == null ? (object)DBNull.Value : item.Closed);
			sqlCommand.Parameters.AddWithValue("ClosedTime", item.ClosedTime == null ? (object)DBNull.Value : item.ClosedTime);
			sqlCommand.Parameters.AddWithValue("ClosedUserEmail", item.ClosedUserEmail == null ? (object)DBNull.Value : item.ClosedUserEmail);
			sqlCommand.Parameters.AddWithValue("ClosedUserId", item.ClosedUserId == null ? (object)DBNull.Value : item.ClosedUserId);
			sqlCommand.Parameters.AddWithValue("ClosedUserName", item.ClosedUserName == null ? (object)DBNull.Value : item.ClosedUserName);
			sqlCommand.Parameters.AddWithValue("CompanyId", item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
			sqlCommand.Parameters.AddWithValue("CompanyName", item.CompanyName == null ? (object)DBNull.Value : item.CompanyName);
			sqlCommand.Parameters.AddWithValue("CreationDate", item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
			sqlCommand.Parameters.AddWithValue("CreationUserEmail", item.CreationUserEmail == null ? (object)DBNull.Value : item.CreationUserEmail);
			sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("CreationUserName", item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
			sqlCommand.Parameters.AddWithValue("CurrencyId", item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
			sqlCommand.Parameters.AddWithValue("CurrencyName", item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
			sqlCommand.Parameters.AddWithValue("CustomerId", item.CustomerId == null ? (object)DBNull.Value : item.CustomerId);
			sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
			sqlCommand.Parameters.AddWithValue("CustomerNr", item.CustomerNr == null ? (object)DBNull.Value : item.CustomerNr);
			sqlCommand.Parameters.AddWithValue("Deleted", item.Deleted == null ? (object)DBNull.Value : item.Deleted);
			sqlCommand.Parameters.AddWithValue("DeleteTime", item.DeleteTime == null ? (object)DBNull.Value : item.DeleteTime);
			sqlCommand.Parameters.AddWithValue("DeleteUserId", item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
			sqlCommand.Parameters.AddWithValue("DepartmentId", item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
			sqlCommand.Parameters.AddWithValue("DepartmentName", item.DepartmentName == null ? (object)DBNull.Value : item.DepartmentName);
			sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
			sqlCommand.Parameters.AddWithValue("Id_State", item.Id_State);
			sqlCommand.Parameters.AddWithValue("Id_Type", item.Id_Type);
			sqlCommand.Parameters.AddWithValue("OrderCount", item.OrderCount == null ? (object)DBNull.Value : item.OrderCount);
			sqlCommand.Parameters.AddWithValue("ProjectBudget", item.ProjectBudget);
			sqlCommand.Parameters.AddWithValue("ProjectName", item.ProjectName);
			sqlCommand.Parameters.AddWithValue("ProjectStatus", item.ProjectStatus == null ? (object)DBNull.Value : item.ProjectStatus);
			sqlCommand.Parameters.AddWithValue("ProjectStatusChangeTime", item.ProjectStatusChangeTime == null ? (object)DBNull.Value : item.ProjectStatusChangeTime);
			sqlCommand.Parameters.AddWithValue("ProjectStatusChangeUserId", item.ProjectStatusChangeUserId == null ? (object)DBNull.Value : item.ProjectStatusChangeUserId);
			sqlCommand.Parameters.AddWithValue("ProjectStatusChangeUserName", item.ProjectStatusChangeUserName == null ? (object)DBNull.Value : item.ProjectStatusChangeUserName);
			sqlCommand.Parameters.AddWithValue("ProjectStatusName", item.ProjectStatusName == null ? (object)DBNull.Value : item.ProjectStatusName);
			sqlCommand.Parameters.AddWithValue("PSZOffer", item.PSZOffer == null ? (object)DBNull.Value : item.PSZOffer);
			sqlCommand.Parameters.AddWithValue("ResponsableEmail", item.ResponsableEmail == null ? (object)DBNull.Value : item.ResponsableEmail);
			sqlCommand.Parameters.AddWithValue("ResponsableId", item.ResponsableId);
			sqlCommand.Parameters.AddWithValue("ResponsableName", item.ResponsableName == null ? (object)DBNull.Value : item.ResponsableName);
			sqlCommand.Parameters.AddWithValue("SiteLevelVisibility", item.SiteLevelVisibility == null ? (object)DBNull.Value : item.SiteLevelVisibility);
			sqlCommand.Parameters.AddWithValue("TotalSpent", item.TotalSpent == null ? (object)DBNull.Value : item.TotalSpent);
			sqlCommand.Parameters.AddWithValue("Type", item.Type == null ? (object)DBNull.Value : item.Type);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 48; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__FNC_BudgetProject] ([ApprovalTime],[ApprovalUserEmail],[ApprovalUserId],[ApprovalUserName],[Archived],[ArchiveTime],[ArchiveUserId],[BudgetYear],[Closed],[ClosedTime],[ClosedUserEmail],[ClosedUserId],[ClosedUserName],[CompanyId],[CompanyName],[CreationDate],[CreationUserEmail],[CreationUserId],[CreationUserName],[CurrencyId],[CurrencyName],[CustomerId],[CustomerName],[CustomerNr],[Deleted],[DeleteTime],[DeleteUserId],[DepartmentId],[DepartmentName],[Description],[Id_State],[Id_Type],[OrderCount],[ProjectBudget],[ProjectName],[ProjectStatus],[ProjectStatusChangeTime],[ProjectStatusChangeUserId],[ProjectStatusChangeUserName],[ProjectStatusName],[PSZOffer],[ResponsableEmail],[ResponsableId],[ResponsableName],[SiteLevelVisibility],[TotalSpent],[Type]) VALUES ( "

						+ "@ApprovalTime" + i + ","
						+ "@ApprovalUserEmail" + i + ","
						+ "@ApprovalUserId" + i + ","
						+ "@ApprovalUserName" + i + ","
						+ "@Archived" + i + ","
						+ "@ArchiveTime" + i + ","
						+ "@ArchiveUserId" + i + ","
						+ "@BudgetYear" + i + ","
						+ "@Closed" + i + ","
						+ "@ClosedTime" + i + ","
						+ "@ClosedUserEmail" + i + ","
						+ "@ClosedUserId" + i + ","
						+ "@ClosedUserName" + i + ","
						+ "@CompanyId" + i + ","
						+ "@CompanyName" + i + ","
						+ "@CreationDate" + i + ","
						+ "@CreationUserEmail" + i + ","
						+ "@CreationUserId" + i + ","
						+ "@CreationUserName" + i + ","
						+ "@CurrencyId" + i + ","
						+ "@CurrencyName" + i + ","
						+ "@CustomerId" + i + ","
						+ "@CustomerName" + i + ","
						+ "@CustomerNr" + i + ","
						+ "@Deleted" + i + ","
						+ "@DeleteTime" + i + ","
						+ "@DeleteUserId" + i + ","
						+ "@DepartmentId" + i + ","
						+ "@DepartmentName" + i + ","
						+ "@Description" + i + ","
						+ "@Id_State" + i + ","
						+ "@Id_Type" + i + ","
						+ "@OrderCount" + i + ","
						+ "@ProjectBudget" + i + ","
						+ "@ProjectName" + i + ","
						+ "@ProjectStatus" + i + ","
						+ "@ProjectStatusChangeTime" + i + ","
						+ "@ProjectStatusChangeUserId" + i + ","
						+ "@ProjectStatusChangeUserName" + i + ","
						+ "@ProjectStatusName" + i + ","
						+ "@PSZOffer" + i + ","
						+ "@ResponsableEmail" + i + ","
						+ "@ResponsableId" + i + ","
						+ "@ResponsableName" + i + ","
						+ "@SiteLevelVisibility" + i + ","
						+ "@TotalSpent" + i + ","
						+ "@Type" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("ApprovalTime" + i, item.ApprovalTime == null ? (object)DBNull.Value : item.ApprovalTime);
					sqlCommand.Parameters.AddWithValue("ApprovalUserEmail" + i, item.ApprovalUserEmail == null ? (object)DBNull.Value : item.ApprovalUserEmail);
					sqlCommand.Parameters.AddWithValue("ApprovalUserId" + i, item.ApprovalUserId == null ? (object)DBNull.Value : item.ApprovalUserId);
					sqlCommand.Parameters.AddWithValue("ApprovalUserName" + i, item.ApprovalUserName == null ? (object)DBNull.Value : item.ApprovalUserName);
					sqlCommand.Parameters.AddWithValue("Archived" + i, item.Archived == null ? (object)DBNull.Value : item.Archived);
					sqlCommand.Parameters.AddWithValue("ArchiveTime" + i, item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
					sqlCommand.Parameters.AddWithValue("ArchiveUserId" + i, item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
					sqlCommand.Parameters.AddWithValue("BudgetYear" + i, item.BudgetYear);
					sqlCommand.Parameters.AddWithValue("Closed" + i, item.Closed == null ? (object)DBNull.Value : item.Closed);
					sqlCommand.Parameters.AddWithValue("ClosedTime" + i, item.ClosedTime == null ? (object)DBNull.Value : item.ClosedTime);
					sqlCommand.Parameters.AddWithValue("ClosedUserEmail" + i, item.ClosedUserEmail == null ? (object)DBNull.Value : item.ClosedUserEmail);
					sqlCommand.Parameters.AddWithValue("ClosedUserId" + i, item.ClosedUserId == null ? (object)DBNull.Value : item.ClosedUserId);
					sqlCommand.Parameters.AddWithValue("ClosedUserName" + i, item.ClosedUserName == null ? (object)DBNull.Value : item.ClosedUserName);
					sqlCommand.Parameters.AddWithValue("CompanyId" + i, item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
					sqlCommand.Parameters.AddWithValue("CompanyName" + i, item.CompanyName == null ? (object)DBNull.Value : item.CompanyName);
					sqlCommand.Parameters.AddWithValue("CreationDate" + i, item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
					sqlCommand.Parameters.AddWithValue("CreationUserEmail" + i, item.CreationUserEmail == null ? (object)DBNull.Value : item.CreationUserEmail);
					sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CreationUserName" + i, item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
					sqlCommand.Parameters.AddWithValue("CurrencyId" + i, item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
					sqlCommand.Parameters.AddWithValue("CurrencyName" + i, item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
					sqlCommand.Parameters.AddWithValue("CustomerId" + i, item.CustomerId == null ? (object)DBNull.Value : item.CustomerId);
					sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("CustomerNr" + i, item.CustomerNr == null ? (object)DBNull.Value : item.CustomerNr);
					sqlCommand.Parameters.AddWithValue("Deleted" + i, item.Deleted == null ? (object)DBNull.Value : item.Deleted);
					sqlCommand.Parameters.AddWithValue("DeleteTime" + i, item.DeleteTime == null ? (object)DBNull.Value : item.DeleteTime);
					sqlCommand.Parameters.AddWithValue("DeleteUserId" + i, item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
					sqlCommand.Parameters.AddWithValue("DepartmentId" + i, item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
					sqlCommand.Parameters.AddWithValue("DepartmentName" + i, item.DepartmentName == null ? (object)DBNull.Value : item.DepartmentName);
					sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
					sqlCommand.Parameters.AddWithValue("Id_State" + i, item.Id_State);
					sqlCommand.Parameters.AddWithValue("Id_Type" + i, item.Id_Type);
					sqlCommand.Parameters.AddWithValue("OrderCount" + i, item.OrderCount == null ? (object)DBNull.Value : item.OrderCount);
					sqlCommand.Parameters.AddWithValue("ProjectBudget" + i, item.ProjectBudget);
					sqlCommand.Parameters.AddWithValue("ProjectName" + i, item.ProjectName);
					sqlCommand.Parameters.AddWithValue("ProjectStatus" + i, item.ProjectStatus == null ? (object)DBNull.Value : item.ProjectStatus);
					sqlCommand.Parameters.AddWithValue("ProjectStatusChangeTime" + i, item.ProjectStatusChangeTime == null ? (object)DBNull.Value : item.ProjectStatusChangeTime);
					sqlCommand.Parameters.AddWithValue("ProjectStatusChangeUserId" + i, item.ProjectStatusChangeUserId == null ? (object)DBNull.Value : item.ProjectStatusChangeUserId);
					sqlCommand.Parameters.AddWithValue("ProjectStatusChangeUserName" + i, item.ProjectStatusChangeUserName == null ? (object)DBNull.Value : item.ProjectStatusChangeUserName);
					sqlCommand.Parameters.AddWithValue("ProjectStatusName" + i, item.ProjectStatusName == null ? (object)DBNull.Value : item.ProjectStatusName);
					sqlCommand.Parameters.AddWithValue("PSZOffer" + i, item.PSZOffer == null ? (object)DBNull.Value : item.PSZOffer);
					sqlCommand.Parameters.AddWithValue("ResponsableEmail" + i, item.ResponsableEmail == null ? (object)DBNull.Value : item.ResponsableEmail);
					sqlCommand.Parameters.AddWithValue("ResponsableId" + i, item.ResponsableId);
					sqlCommand.Parameters.AddWithValue("ResponsableName" + i, item.ResponsableName == null ? (object)DBNull.Value : item.ResponsableName);
					sqlCommand.Parameters.AddWithValue("SiteLevelVisibility" + i, item.SiteLevelVisibility == null ? (object)DBNull.Value : item.SiteLevelVisibility);
					sqlCommand.Parameters.AddWithValue("TotalSpent" + i, item.TotalSpent == null ? (object)DBNull.Value : item.TotalSpent);
					sqlCommand.Parameters.AddWithValue("Type" + i, item.Type == null ? (object)DBNull.Value : item.Type);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__FNC_BudgetProject] SET [ApprovalTime]=@ApprovalTime, [ApprovalUserEmail]=@ApprovalUserEmail, [ApprovalUserId]=@ApprovalUserId, [ApprovalUserName]=@ApprovalUserName, [Archived]=@Archived, [ArchiveTime]=@ArchiveTime, [ArchiveUserId]=@ArchiveUserId, [BudgetYear]=@BudgetYear, [Closed]=@Closed, [ClosedTime]=@ClosedTime, [ClosedUserEmail]=@ClosedUserEmail, [ClosedUserId]=@ClosedUserId, [ClosedUserName]=@ClosedUserName, [CompanyId]=@CompanyId, [CompanyName]=@CompanyName, [CreationDate]=@CreationDate, [CreationUserEmail]=@CreationUserEmail, [CreationUserId]=@CreationUserId, [CreationUserName]=@CreationUserName, [CurrencyId]=@CurrencyId, [CurrencyName]=@CurrencyName, [CustomerId]=@CustomerId, [CustomerName]=@CustomerName, [CustomerNr]=@CustomerNr, [Deleted]=@Deleted, [DeleteTime]=@DeleteTime, [DeleteUserId]=@DeleteUserId, [DepartmentId]=@DepartmentId, [DepartmentName]=@DepartmentName, [Description]=@Description, [Id_State]=@Id_State, [Id_Type]=@Id_Type, [OrderCount]=@OrderCount, [ProjectBudget]=@ProjectBudget, [ProjectName]=@ProjectName, [ProjectStatus]=@ProjectStatus, [ProjectStatusChangeTime]=@ProjectStatusChangeTime, [ProjectStatusChangeUserId]=@ProjectStatusChangeUserId, [ProjectStatusChangeUserName]=@ProjectStatusChangeUserName, [ProjectStatusName]=@ProjectStatusName, [PSZOffer]=@PSZOffer, [ResponsableEmail]=@ResponsableEmail, [ResponsableId]=@ResponsableId, [ResponsableName]=@ResponsableName, [SiteLevelVisibility]=@SiteLevelVisibility, [TotalSpent]=@TotalSpent, [Type]=@Type WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("ApprovalTime", item.ApprovalTime == null ? (object)DBNull.Value : item.ApprovalTime);
			sqlCommand.Parameters.AddWithValue("ApprovalUserEmail", item.ApprovalUserEmail == null ? (object)DBNull.Value : item.ApprovalUserEmail);
			sqlCommand.Parameters.AddWithValue("ApprovalUserId", item.ApprovalUserId == null ? (object)DBNull.Value : item.ApprovalUserId);
			sqlCommand.Parameters.AddWithValue("ApprovalUserName", item.ApprovalUserName == null ? (object)DBNull.Value : item.ApprovalUserName);
			sqlCommand.Parameters.AddWithValue("Archived", item.Archived == null ? (object)DBNull.Value : item.Archived);
			sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
			sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
			sqlCommand.Parameters.AddWithValue("BudgetYear", item.BudgetYear);
			sqlCommand.Parameters.AddWithValue("Closed", item.Closed == null ? (object)DBNull.Value : item.Closed);
			sqlCommand.Parameters.AddWithValue("ClosedTime", item.ClosedTime == null ? (object)DBNull.Value : item.ClosedTime);
			sqlCommand.Parameters.AddWithValue("ClosedUserEmail", item.ClosedUserEmail == null ? (object)DBNull.Value : item.ClosedUserEmail);
			sqlCommand.Parameters.AddWithValue("ClosedUserId", item.ClosedUserId == null ? (object)DBNull.Value : item.ClosedUserId);
			sqlCommand.Parameters.AddWithValue("ClosedUserName", item.ClosedUserName == null ? (object)DBNull.Value : item.ClosedUserName);
			sqlCommand.Parameters.AddWithValue("CompanyId", item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
			sqlCommand.Parameters.AddWithValue("CompanyName", item.CompanyName == null ? (object)DBNull.Value : item.CompanyName);
			sqlCommand.Parameters.AddWithValue("CreationDate", item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
			sqlCommand.Parameters.AddWithValue("CreationUserEmail", item.CreationUserEmail == null ? (object)DBNull.Value : item.CreationUserEmail);
			sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("CreationUserName", item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
			sqlCommand.Parameters.AddWithValue("CurrencyId", item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
			sqlCommand.Parameters.AddWithValue("CurrencyName", item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
			sqlCommand.Parameters.AddWithValue("CustomerId", item.CustomerId == null ? (object)DBNull.Value : item.CustomerId);
			sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
			sqlCommand.Parameters.AddWithValue("CustomerNr", item.CustomerNr == null ? (object)DBNull.Value : item.CustomerNr);
			sqlCommand.Parameters.AddWithValue("Deleted", item.Deleted == null ? (object)DBNull.Value : item.Deleted);
			sqlCommand.Parameters.AddWithValue("DeleteTime", item.DeleteTime == null ? (object)DBNull.Value : item.DeleteTime);
			sqlCommand.Parameters.AddWithValue("DeleteUserId", item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
			sqlCommand.Parameters.AddWithValue("DepartmentId", item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
			sqlCommand.Parameters.AddWithValue("DepartmentName", item.DepartmentName == null ? (object)DBNull.Value : item.DepartmentName);
			sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
			sqlCommand.Parameters.AddWithValue("Id_State", item.Id_State);
			sqlCommand.Parameters.AddWithValue("Id_Type", item.Id_Type);
			sqlCommand.Parameters.AddWithValue("OrderCount", item.OrderCount == null ? (object)DBNull.Value : item.OrderCount);
			sqlCommand.Parameters.AddWithValue("ProjectBudget", item.ProjectBudget);
			sqlCommand.Parameters.AddWithValue("ProjectName", item.ProjectName);
			sqlCommand.Parameters.AddWithValue("ProjectStatus", item.ProjectStatus == null ? (object)DBNull.Value : item.ProjectStatus);
			sqlCommand.Parameters.AddWithValue("ProjectStatusChangeTime", item.ProjectStatusChangeTime == null ? (object)DBNull.Value : item.ProjectStatusChangeTime);
			sqlCommand.Parameters.AddWithValue("ProjectStatusChangeUserId", item.ProjectStatusChangeUserId == null ? (object)DBNull.Value : item.ProjectStatusChangeUserId);
			sqlCommand.Parameters.AddWithValue("ProjectStatusChangeUserName", item.ProjectStatusChangeUserName == null ? (object)DBNull.Value : item.ProjectStatusChangeUserName);
			sqlCommand.Parameters.AddWithValue("ProjectStatusName", item.ProjectStatusName == null ? (object)DBNull.Value : item.ProjectStatusName);
			sqlCommand.Parameters.AddWithValue("PSZOffer", item.PSZOffer == null ? (object)DBNull.Value : item.PSZOffer);
			sqlCommand.Parameters.AddWithValue("ResponsableEmail", item.ResponsableEmail == null ? (object)DBNull.Value : item.ResponsableEmail);
			sqlCommand.Parameters.AddWithValue("ResponsableId", item.ResponsableId);
			sqlCommand.Parameters.AddWithValue("ResponsableName", item.ResponsableName == null ? (object)DBNull.Value : item.ResponsableName);
			sqlCommand.Parameters.AddWithValue("SiteLevelVisibility", item.SiteLevelVisibility == null ? (object)DBNull.Value : item.SiteLevelVisibility);
			sqlCommand.Parameters.AddWithValue("TotalSpent", item.TotalSpent == null ? (object)DBNull.Value : item.TotalSpent);
			sqlCommand.Parameters.AddWithValue("Type", item.Type == null ? (object)DBNull.Value : item.Type);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 48; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__FNC_BudgetProject] SET "

					+ "[ApprovalTime]=@ApprovalTime" + i + ","
					+ "[ApprovalUserEmail]=@ApprovalUserEmail" + i + ","
					+ "[ApprovalUserId]=@ApprovalUserId" + i + ","
					+ "[ApprovalUserName]=@ApprovalUserName" + i + ","
					+ "[Archived]=@Archived" + i + ","
					+ "[ArchiveTime]=@ArchiveTime" + i + ","
					+ "[ArchiveUserId]=@ArchiveUserId" + i + ","
					+ "[BudgetYear]=@BudgetYear" + i + ","
					+ "[Closed]=@Closed" + i + ","
					+ "[ClosedTime]=@ClosedTime" + i + ","
					+ "[ClosedUserEmail]=@ClosedUserEmail" + i + ","
					+ "[ClosedUserId]=@ClosedUserId" + i + ","
					+ "[ClosedUserName]=@ClosedUserName" + i + ","
					+ "[CompanyId]=@CompanyId" + i + ","
					+ "[CompanyName]=@CompanyName" + i + ","
					+ "[CreationDate]=@CreationDate" + i + ","
					+ "[CreationUserEmail]=@CreationUserEmail" + i + ","
					+ "[CreationUserId]=@CreationUserId" + i + ","
					+ "[CreationUserName]=@CreationUserName" + i + ","
					+ "[CurrencyId]=@CurrencyId" + i + ","
					+ "[CurrencyName]=@CurrencyName" + i + ","
					+ "[CustomerId]=@CustomerId" + i + ","
					+ "[CustomerName]=@CustomerName" + i + ","
					+ "[CustomerNr]=@CustomerNr" + i + ","
					+ "[Deleted]=@Deleted" + i + ","
					+ "[DeleteTime]=@DeleteTime" + i + ","
					+ "[DeleteUserId]=@DeleteUserId" + i + ","
					+ "[DepartmentId]=@DepartmentId" + i + ","
					+ "[DepartmentName]=@DepartmentName" + i + ","
					+ "[Description]=@Description" + i + ","
					+ "[Id_State]=@Id_State" + i + ","
					+ "[Id_Type]=@Id_Type" + i + ","
					+ "[OrderCount]=@OrderCount" + i + ","
					+ "[ProjectBudget]=@ProjectBudget" + i + ","
					+ "[ProjectName]=@ProjectName" + i + ","
					+ "[ProjectStatus]=@ProjectStatus" + i + ","
					+ "[ProjectStatusChangeTime]=@ProjectStatusChangeTime" + i + ","
					+ "[ProjectStatusChangeUserId]=@ProjectStatusChangeUserId" + i + ","
					+ "[ProjectStatusChangeUserName]=@ProjectStatusChangeUserName" + i + ","
					+ "[ProjectStatusName]=@ProjectStatusName" + i + ","
					+ "[PSZOffer]=@PSZOffer" + i + ","
					+ "[ResponsableEmail]=@ResponsableEmail" + i + ","
					+ "[ResponsableId]=@ResponsableId" + i + ","
					+ "[ResponsableName]=@ResponsableName" + i + ","
					+ "[SiteLevelVisibility]=@SiteLevelVisibility" + i + ","
					+ "[TotalSpent]=@TotalSpent" + i + ","
					+ "[Type]=@Type" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("ApprovalTime" + i, item.ApprovalTime == null ? (object)DBNull.Value : item.ApprovalTime);
					sqlCommand.Parameters.AddWithValue("ApprovalUserEmail" + i, item.ApprovalUserEmail == null ? (object)DBNull.Value : item.ApprovalUserEmail);
					sqlCommand.Parameters.AddWithValue("ApprovalUserId" + i, item.ApprovalUserId == null ? (object)DBNull.Value : item.ApprovalUserId);
					sqlCommand.Parameters.AddWithValue("ApprovalUserName" + i, item.ApprovalUserName == null ? (object)DBNull.Value : item.ApprovalUserName);
					sqlCommand.Parameters.AddWithValue("Archived" + i, item.Archived == null ? (object)DBNull.Value : item.Archived);
					sqlCommand.Parameters.AddWithValue("ArchiveTime" + i, item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
					sqlCommand.Parameters.AddWithValue("ArchiveUserId" + i, item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
					sqlCommand.Parameters.AddWithValue("BudgetYear" + i, item.BudgetYear);
					sqlCommand.Parameters.AddWithValue("Closed" + i, item.Closed == null ? (object)DBNull.Value : item.Closed);
					sqlCommand.Parameters.AddWithValue("ClosedTime" + i, item.ClosedTime == null ? (object)DBNull.Value : item.ClosedTime);
					sqlCommand.Parameters.AddWithValue("ClosedUserEmail" + i, item.ClosedUserEmail == null ? (object)DBNull.Value : item.ClosedUserEmail);
					sqlCommand.Parameters.AddWithValue("ClosedUserId" + i, item.ClosedUserId == null ? (object)DBNull.Value : item.ClosedUserId);
					sqlCommand.Parameters.AddWithValue("ClosedUserName" + i, item.ClosedUserName == null ? (object)DBNull.Value : item.ClosedUserName);
					sqlCommand.Parameters.AddWithValue("CompanyId" + i, item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
					sqlCommand.Parameters.AddWithValue("CompanyName" + i, item.CompanyName == null ? (object)DBNull.Value : item.CompanyName);
					sqlCommand.Parameters.AddWithValue("CreationDate" + i, item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
					sqlCommand.Parameters.AddWithValue("CreationUserEmail" + i, item.CreationUserEmail == null ? (object)DBNull.Value : item.CreationUserEmail);
					sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CreationUserName" + i, item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
					sqlCommand.Parameters.AddWithValue("CurrencyId" + i, item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
					sqlCommand.Parameters.AddWithValue("CurrencyName" + i, item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
					sqlCommand.Parameters.AddWithValue("CustomerId" + i, item.CustomerId == null ? (object)DBNull.Value : item.CustomerId);
					sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("CustomerNr" + i, item.CustomerNr == null ? (object)DBNull.Value : item.CustomerNr);
					sqlCommand.Parameters.AddWithValue("Deleted" + i, item.Deleted == null ? (object)DBNull.Value : item.Deleted);
					sqlCommand.Parameters.AddWithValue("DeleteTime" + i, item.DeleteTime == null ? (object)DBNull.Value : item.DeleteTime);
					sqlCommand.Parameters.AddWithValue("DeleteUserId" + i, item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
					sqlCommand.Parameters.AddWithValue("DepartmentId" + i, item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
					sqlCommand.Parameters.AddWithValue("DepartmentName" + i, item.DepartmentName == null ? (object)DBNull.Value : item.DepartmentName);
					sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
					sqlCommand.Parameters.AddWithValue("Id_State" + i, item.Id_State);
					sqlCommand.Parameters.AddWithValue("Id_Type" + i, item.Id_Type);
					sqlCommand.Parameters.AddWithValue("OrderCount" + i, item.OrderCount == null ? (object)DBNull.Value : item.OrderCount);
					sqlCommand.Parameters.AddWithValue("ProjectBudget" + i, item.ProjectBudget);
					sqlCommand.Parameters.AddWithValue("ProjectName" + i, item.ProjectName);
					sqlCommand.Parameters.AddWithValue("ProjectStatus" + i, item.ProjectStatus == null ? (object)DBNull.Value : item.ProjectStatus);
					sqlCommand.Parameters.AddWithValue("ProjectStatusChangeTime" + i, item.ProjectStatusChangeTime == null ? (object)DBNull.Value : item.ProjectStatusChangeTime);
					sqlCommand.Parameters.AddWithValue("ProjectStatusChangeUserId" + i, item.ProjectStatusChangeUserId == null ? (object)DBNull.Value : item.ProjectStatusChangeUserId);
					sqlCommand.Parameters.AddWithValue("ProjectStatusChangeUserName" + i, item.ProjectStatusChangeUserName == null ? (object)DBNull.Value : item.ProjectStatusChangeUserName);
					sqlCommand.Parameters.AddWithValue("ProjectStatusName" + i, item.ProjectStatusName == null ? (object)DBNull.Value : item.ProjectStatusName);
					sqlCommand.Parameters.AddWithValue("PSZOffer" + i, item.PSZOffer == null ? (object)DBNull.Value : item.PSZOffer);
					sqlCommand.Parameters.AddWithValue("ResponsableEmail" + i, item.ResponsableEmail == null ? (object)DBNull.Value : item.ResponsableEmail);
					sqlCommand.Parameters.AddWithValue("ResponsableId" + i, item.ResponsableId);
					sqlCommand.Parameters.AddWithValue("ResponsableName" + i, item.ResponsableName == null ? (object)DBNull.Value : item.ResponsableName);
					sqlCommand.Parameters.AddWithValue("SiteLevelVisibility" + i, item.SiteLevelVisibility == null ? (object)DBNull.Value : item.SiteLevelVisibility);
					sqlCommand.Parameters.AddWithValue("TotalSpent" + i, item.TotalSpent == null ? (object)DBNull.Value : item.TotalSpent);
					sqlCommand.Parameters.AddWithValue("Type" + i, item.Type == null ? (object)DBNull.Value : item.Type);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__FNC_BudgetProject] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			results = sqlCommand.ExecuteNonQuery();


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

				string query = "DELETE FROM [__FNC_BudgetProject] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods


		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity> GetByState(int id_state, bool isArchived = false, bool isDeleted = false)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_BudgetProject] WHERE [Archived]=@archived AND [Deleted]=@deleted AND [Id_state]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id_state);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>();
			}

		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity> GetbyCurrent(int id_user, bool isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_BudgetProject] where [Archived]=@archived AND [Deleted]=@deleted AND ResponsableId=@id_user";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id_user", id_user);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity> GetbyCurrentAndState(int id_user, int state, bool isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_BudgetProject] where [Archived]=@archived AND [Deleted]=@deleted AND ResponsableId=@id_user and Id_State=@state";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id_user", id_user);
				sqlCommand.Parameters.AddWithValue("state", state);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>();
			}
		}

		public static int MaxIdProj()
		{
			var dataTable = new DataTable();
			int response = 0;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "select max(Id) as Max_Id from [__FNC_BudgetProject]";
				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				response = Convert.ToInt32(dataTable.Rows[0]["Max_Id"]);
				// return toList2(dataTable);
			}
			return response;
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity> GetProjectByName(string value, bool isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_BudgetProject]  where [Archived]=@archived AND [Deleted]=@deleted AND ProjectName=@value";
				//SELECT * FROM [__FNC_BudgetProject]  where Name_proj Like '%project%'
				//SELECT * FROM [__FNC_BudgetProject]  where Name_proj=@val
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				sqlCommand.Parameters.AddWithValue("value", value);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>();
			}
		}
		public static int UpdateExceptDept(Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_BudgetProject] SET [ApprovalTime]=@ApprovalTime, [ApprovalUserEmail]=@ApprovalUserEmail, [ApprovalUserId]=@ApprovalUserId, [ApprovalUserName]=@ApprovalUserName, [Archived]=@Archived, [ArchiveTime]=@ArchiveTime, [ArchiveUserId]=@ArchiveUserId, [BudgetYear]=@BudgetYear, [CompanyId]=@CompanyId, [CompanyName]=@CompanyName, [CreationDate]=@CreationDate, [CurrencyId]=@CurrencyId, [CurrencyName]=@CurrencyName, [CustomerId]=@CustomerId, [CustomerName]=@CustomerName, [CustomerNr]=@CustomerNr, [Deleted]=@Deleted, [DeleteTime]=@DeleteTime, [DeleteUserId]=@DeleteUserId, [Description]=@Description, [Id_State]=@Id_State, [Id_Type]=@Id_Type, [OrderCount]=@OrderCount, [ProjectBudget]=@ProjectBudget, [ProjectName]=@ProjectName, [ProjectStatus]=@ProjectStatus, [ProjectStatusChangeTime]=@ProjectStatusChangeTime, [ProjectStatusChangeUserId]=@ProjectStatusChangeUserId, [ProjectStatusChangeUserName]=@ProjectStatusChangeUserName, [ProjectStatusName]=@ProjectStatusName, [PSZOffer]=@PSZOffer, [ResponsableEmail]=@ResponsableEmail, [ResponsableId]=@ResponsableId, [ResponsableName]=@ResponsableName, [TotalSpent]=@TotalSpent, [Type]=@Type WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ApprovalTime", item.ApprovalTime == null ? (object)DBNull.Value : item.ApprovalTime);
				sqlCommand.Parameters.AddWithValue("ApprovalUserEmail", item.ApprovalUserEmail == null ? (object)DBNull.Value : item.ApprovalUserEmail);
				sqlCommand.Parameters.AddWithValue("ApprovalUserId", item.ApprovalUserId == null ? (object)DBNull.Value : item.ApprovalUserId);
				sqlCommand.Parameters.AddWithValue("ApprovalUserName", item.ApprovalUserName == null ? (object)DBNull.Value : item.ApprovalUserName);
				sqlCommand.Parameters.AddWithValue("Archived", item.Archived == null ? (object)DBNull.Value : item.Archived);
				sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
				sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
				sqlCommand.Parameters.AddWithValue("BudgetYear", item.BudgetYear);
				sqlCommand.Parameters.AddWithValue("CompanyId", item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
				sqlCommand.Parameters.AddWithValue("CompanyName", item.CompanyName == null ? (object)DBNull.Value : item.CompanyName);
				sqlCommand.Parameters.AddWithValue("CreationDate", item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
				sqlCommand.Parameters.AddWithValue("CurrencyId", item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
				sqlCommand.Parameters.AddWithValue("CurrencyName", item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
				sqlCommand.Parameters.AddWithValue("CustomerId", item.CustomerId == null ? (object)DBNull.Value : item.CustomerId);
				sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
				sqlCommand.Parameters.AddWithValue("CustomerNr", item.CustomerNr == null ? (object)DBNull.Value : item.CustomerNr);
				sqlCommand.Parameters.AddWithValue("Deleted", item.Deleted == null ? (object)DBNull.Value : item.Deleted);
				sqlCommand.Parameters.AddWithValue("DeleteTime", item.DeleteTime == null ? (object)DBNull.Value : item.DeleteTime);
				sqlCommand.Parameters.AddWithValue("DeleteUserId", item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
				//sqlCommand.Parameters.AddWithValue("DepartmentId", item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
				//sqlCommand.Parameters.AddWithValue("DepartmentName", item.DepartmentName == null ? (object)DBNull.Value : item.DepartmentName);
				sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
				sqlCommand.Parameters.AddWithValue("Id_State", item.Id_State);
				sqlCommand.Parameters.AddWithValue("Id_Type", item.Id_Type);
				sqlCommand.Parameters.AddWithValue("OrderCount", item.OrderCount == null ? (object)DBNull.Value : item.OrderCount);
				sqlCommand.Parameters.AddWithValue("ProjectBudget", item.ProjectBudget);
				sqlCommand.Parameters.AddWithValue("ProjectName", item.ProjectName);
				sqlCommand.Parameters.AddWithValue("ProjectStatus", item.ProjectStatus == null ? (object)DBNull.Value : item.ProjectStatus);
				sqlCommand.Parameters.AddWithValue("ProjectStatusChangeTime", item.ProjectStatusChangeTime == null ? (object)DBNull.Value : item.ProjectStatusChangeTime);
				sqlCommand.Parameters.AddWithValue("ProjectStatusChangeUserId", item.ProjectStatusChangeUserId == null ? (object)DBNull.Value : item.ProjectStatusChangeUserId);
				sqlCommand.Parameters.AddWithValue("ProjectStatusChangeUserName", item.ProjectStatusChangeUserName == null ? (object)DBNull.Value : item.ProjectStatusChangeUserName);
				sqlCommand.Parameters.AddWithValue("ProjectStatusName", item.ProjectStatusName == null ? (object)DBNull.Value : item.ProjectStatusName);
				sqlCommand.Parameters.AddWithValue("PSZOffer", item.PSZOffer == null ? (object)DBNull.Value : item.PSZOffer);
				sqlCommand.Parameters.AddWithValue("ResponsableEmail", item.ResponsableEmail == null ? (object)DBNull.Value : item.ResponsableEmail);
				sqlCommand.Parameters.AddWithValue("ResponsableId", item.ResponsableId);
				sqlCommand.Parameters.AddWithValue("ResponsableName", item.ResponsableName == null ? (object)DBNull.Value : item.ResponsableName);
				sqlCommand.Parameters.AddWithValue("TotalSpent", item.TotalSpent == null ? (object)DBNull.Value : item.TotalSpent);
				sqlCommand.Parameters.AddWithValue("Type", item.Type == null ? (object)DBNull.Value : item.Type);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity> GetByDepartmentIds(List<int> departmentIds, int? type, bool isArchived = false, bool isDeleted = false)
		{
			if(departmentIds == null || departmentIds.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BudgetProject] WHERE {(type.HasValue ? $"[Id_Type]={type.Value} AND " : "")} [DepartmentId] IN ({string.Join(",", departmentIds)}) AND [Archived]=@archived AND [Deleted]=@deleted AND [Id_Type]<>3";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>();
			}

		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity> GetByType(int? type, bool isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BudgetProject] WHERE {(type.HasValue ? $"[Id_Type]={type.Value} AND " : "")} [Archived]=@archived AND [Deleted]=@deleted";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>();
			}

		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity> GetBySitesIds(List<int> siteIds, int? type, bool isArchived = false, bool isDeleted = false)
		{
			if(siteIds == null || siteIds.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BudgetProject] WHERE {(type.HasValue ? $"[Id_Type]={type.Value} AND " : "")} [CompanyId] IN ({string.Join(",", siteIds)}) AND [Archived]=@archived AND [Deleted]=@deleted";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>();
			}

		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity> GetSiteLevel(List<int> siteIds, bool isArchived = false, bool isDeleted = false)
		{
			if(siteIds == null || siteIds.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BudgetProject] WHERE [CompanyId] IN ({string.Join(",", siteIds)}) AND [SiteLevelVisibility]=1 AND [Archived]=@archived AND [Deleted]=@deleted";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>();
			}

		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity> GetBySiteWODepartment(List<int> siteIds, int? type, bool isArchived = false, bool isDeleted = false)
		{
			if(siteIds == null || siteIds.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BudgetProject] WHERE {(type.HasValue ? $"[Id_Type]={type.Value} AND " : "")} [CompanyId] IN ({string.Join(",", siteIds)}) AND ([DepartmentId] IS NULL OR [DepartmentId] <= 0) AND [Archived]=@archived AND [Deleted]=@deleted";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("type", type);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>();
			}

		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity> Get(int? userId, int? companyId, int? departmentId, int? minOrderCount, int? creationYear, int? type, int? status, bool? approved, bool? closed, bool? isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_BudgetProject] WHERE [Deleted]=@deleted";
				if(userId.HasValue)
					query += $" AND ResponsableId={userId.Value}";
				// - 
				if(companyId.HasValue)
					query += $" AND CompanyId={companyId.Value}";
				// - 
				if(departmentId.HasValue)
					query += $" AND DepartmentId={departmentId.Value}";
				// - 
				if(minOrderCount.HasValue)
					query += $" AND OrderCount>={minOrderCount.Value}";
				// - 
				if(creationYear.HasValue)
					query += $" AND YEAR([CreationDate])={creationYear.Value}";
				// - type
				if(type.HasValue)
					query += $" AND [Id_Type]={type.Value}";
				// - status
				if(status.HasValue)
					query += $" AND ProjectStatus={status.Value}";

				// - approved
				if(approved.HasValue)
					query += $" AND {(approved.Value == true ? "ApprovalUserId IS NOT NULL AND ApprovalTime IS NOT NULL" : "ApprovalUserId IS NULL AND ApprovalTime IS NULL")}";
				// - closed
				if(closed.HasValue)
					query += $" AND {(closed.Value == true ? "Closed=1 AND ClosedUserId IS NOT NULL" : "(Closed IS NULL OR Closed=0)")}";
				// - isArchived
				if(isArchived.HasValue)
					query += $" AND {(isArchived.Value == true ? "Archived=1 AND ArchiveUserId IS NOT NULL" : "(Archived IS NULL OR Archived=0)")}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity> GetByReponsible(int responsibleId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_BudgetProject] WHERE [ResponsableId]=@responsibleId";
				

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("responsibleId", responsibleId);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>();
			}
		}

		#endregion
		#region >> personalize Methods

		#region>> get Project per Month << OK >>
		public static List<KeyValuePair<int, int>> GetProjectPerMonth(int? year, bool isArchived = false, bool isDeleted = false)
		{
			if(year is null)
			{
				// if year is not specified use current year by default
				year = DateTime.Now.Year;
			}

			//List<KeyValuePair<string, int>> projectByMonth = new List<KeyValuePair<string, int>>();

			// --1
			var dataTable = new DataTable();

			// --2
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				// --3
				sqlConnection.Open();

				// --4  REM : if you want you can apply your verification here inside your request (check if your year variable is null or nor by using c# expression )
				string query = @"SELECT MONTH(CreationDate) As monthNumber, COUNT(*)  AS number_of_project FROM [__FNC_BudgetProject] WHERE YEAR(CreationDate) =@year AND [Deleted] = @deleted AND [Archived]= @archived GROUP BY MONTH(CreationDate) ORDER BY MONTH(MIN(CreationDate));";

				// --5
				SqlCommand command = new SqlCommand(query, sqlConnection);


				// --6
				command.Parameters.AddWithValue("archived", isArchived);
				command.Parameters.AddWithValue("deleted", isDeleted);
				command.Parameters.Add("year", SqlDbType.Int).Value = year;

				new SqlDataAdapter(command).Fill(dataTable);
			}

			// --7
			if(dataTable.Rows.Count > 0)
			{
				return (List<KeyValuePair<int, int>>)dataTable.Rows.Cast<DataRow>().Select(row => new KeyValuePair<int, int>(
					 Convert.ToInt32(row["monthNumber"]),
					 Convert.ToInt32(row["number_of_project"]))).ToList();
			}
			else
			{
				return new List<KeyValuePair<int, int>>();
			}

		}

		#endregion

		#region  >>> Get Available Year
		public static List<int> GetAvailableYear()
		{
			// --1
			var dataTable = new DataTable();

			// --2 
			using(SqlConnection sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				// --3
				sqlConnection.Open();

				// --4
				string query = @"SELECT DISTINCT YEAR(CreationDate) AS project_year FROM [__FNC_BudgetProject] ORDER BY project_year;";

				// --5
				SqlCommand command = new SqlCommand(query, sqlConnection);

				// --6
				new SqlDataAdapter(command).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(row => (Convert.ToInt32(row["project_year"]))).ToList();
			}
			else
			{
				return new List<int>();
			}




		}

		#endregion

		#region >> get project types by month ||
		public static List<ProjectTypeByMonthEntity> GetProjectTypeByMonth(int? year)
		{
			if(!year.HasValue)
			{
				year = DateTime.Now.Year; // use current year if no year is specified 
			}

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				string query = @"
            SELECT 
            DATENAME(MONTH, CreationDate) AS _month,
			MONTH(CreationDate)  AS monthNumber,
            Type AS projectTypeName,
            COUNT(Id) AS _count
            FROM __FNC_BudgetProject
            WHERE YEAR(CreationDate) = @Year
            GROUP BY MONTH(CreationDate), DATENAME(MONTH, CreationDate), Type
            ORDER BY  MONTH(MIN(CreationDate));";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@Year", year);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(row => new ProjectTypeByMonthEntity(row)).ToList();
			}
			else
			{
				return new List<ProjectTypeByMonthEntity>();
			}
		}
		#endregion

		#region >> Project Approbations statuses per Month  ||
		public static List<ProjectTypeByStatusApprobationAndMonthEntity> GetProjectTypeByStatusAndMonth(List<int> projectID_Type, List<int> statuses, int? year)
		{
			if(!year.HasValue)
			{
				year = DateTime.Now.Year; // Utilisation de l'année actuelle si aucune année n'est spécifiée
			}

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $@"
            SELECT 
            MONTH(CreationDate) AS monthNumber, Id_Type AS projectId_Type , Type AS project_type,
            ProjectStatus AS status,
            COUNT(Id) AS _count
            FROM __FNC_BudgetProject
            WHERE YEAR(CreationDate) = @Year AND ProjectStatus IN ({string.Join(',', statuses)}) AND Id_Type IN ({string.Join(',', projectID_Type)})
            GROUP BY MONTH(CreationDate), DATENAME(MONTH, CreationDate), ProjectStatus, Type, Id_Type
            ORDER BY  MONTH(MIN(CreationDate)), Type";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@Year", year);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(row => new ProjectTypeByStatusApprobationAndMonthEntity
				{
					MonthNumber = Convert.ToInt32(row["monthNumber"]),
					ProjectType = Convert.ToString(row["project_Type"]),
					Status = row["status"].ToString(),
					Count = Convert.ToInt32(row["_count"])

				}).ToList();

			}
			else
			{
				return new List<ProjectTypeByStatusApprobationAndMonthEntity>();
			}
		}
		#endregion

		#region >>> Project Statuses per Month  || ??
		public static List<ProjectTypeByStatusApprobationAndMonthEntity> GetProjectTypeByStatusApprobationAndMonth(List<int> projectID_Type, List<int> projectStatusApprobation, int? year)
		{
			if(!year.HasValue)
			{
				year = DateTime.Now.Year; // Utilisation de l'année actuelle si aucune année n'est spécifiée
			}

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{

				sqlConnection.Open();

				string query = $@"  
				SELECT 
				MONTH(CreationDate) AS _month, 
				Type as project_Type,
				Id_Type AS projectId_Type,
				Id_State AS projectApprobation,
				COUNT(Id) AS _count
				FROM __FNC_BudgetProject
				WHERE YEAR(CreationDate) = @Year AND [Id_State]  IN ({string.Join(',', projectStatusApprobation)}) AND [Id_Type] IN ({string.Join(',', projectID_Type)})
				GROUP BY MONTH(CreationDate), DATENAME(MONTH, CreationDate), Id_State,Id_Type, Type
				ORDER BY  MONTH(MIN(CreationDate)), Type";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@Year", year);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(row => new ProjectTypeByStatusApprobationAndMonthEntity
				{
					MonthNumber = Convert.ToInt32(row["_month"]),
					ProjectType = Convert.ToString(row["project_Type"]),
					Status = row["projectApprobation"].ToString(),
					Count = Convert.ToInt32(row["_count"])
				}).ToList();
			}
			else
			{
				return new List<ProjectTypeByStatusApprobationAndMonthEntity>();
			}
		}
		#endregion

		#region >> Count Order by project type

		public static List<ProjectTypeOrderCountEntity> CountOrderByProjectType(List<int> Project_Type, int? year)
		{
			var Datatable = new DataTable();

			using(SqlConnection connection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				connection.Open();
				string query = $"SELECT MONTH(bpe.CreationDate) AS monthNumber, DATENAME(MONTH, bpe.CreationDate) AS Month, bpe.Id_Type AS ProjectId_Type, bpe.Type AS Type, COUNT(bpe.Id) AS NumberOfOrders FROM __FNC_BudgetProject bpe JOIN [dbo].[__FNC_BestellungenExtension] cmd ON  cmd.ProjectId = bpe.Id WHERE  bpe.Id_Type IN ({string.Join(',', Project_Type)}) AND YEAR(bpe.CreationDate) = @Year  GROUP BY  DATENAME(MONTH, bpe.CreationDate), MONTH(bpe.CreationDate),bpe.Id_Type, bpe.Type ORDER BY MIN(MONTH(bpe.CreationDate)), bpe.Id_Type, bpe.Type;";

				SqlCommand command = new SqlCommand(query, connection);
				command.Parameters.AddWithValue("@Year", year);
				new SqlDataAdapter(command).Fill(Datatable);

			};

			if(Datatable.Rows.Count > 0)
			{
				return (List<ProjectTypeOrderCountEntity>)Datatable.Rows.Cast<DataRow>().Select(row => new ProjectTypeOrderCountEntity(row)).ToList();
			}
			{
				return new List<ProjectTypeOrderCountEntity>();
			}

		}

		#endregion


		#region
		public static List<Tuple<int, int, int>> GetProjectsMonthlyByStatus(List<int> statuses, int? year)
		{

			if(statuses == null || statuses.Count <= 0)
				return null;


			if(!year.HasValue) // if no value get current year
			{
				year = DateTime.Now.Year;
			}
			var dataTable = new DataTable();

			using(SqlConnection sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				// 1-
				sqlConnection.Open();

				// 2-query development
				string query = $@"SELECT MONTH(CreationDate) AS monthNumber, ProjectStatus AS statuses, Count(Id) AS count_project FROM __FNC_BudgetProject WHERE ProjectStatus IN({string.Join(',', statuses)})  AND YEAR(CreationDate) = @Year  GROUP BY MONTH(CreationDate), ProjectStatus  ORDER BY MONTH(MIN(CreationDate));";
				// 3- Command object

				SqlCommand command = new SqlCommand(query, sqlConnection);

				// 4- Parameter

				command.Parameters.AddWithValue("Year", year);


				//5- filling of the object dataTable stored in memory (query execution)
				new SqlDataAdapter(command).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(row => new Tuple<int, int, int>(
						 Convert.ToInt32(row["monthNumber"]),
						 Convert.ToInt32(row["statuses"]),
						 Convert.ToInt32(row["count_project"])
					)).ToList();

			}
			else
			{
				return new List<Tuple<int, int, int>>();
			}



		}

		#endregion

		#region >> Status Approbation Per Month

		public static List<Tuple<int, int, int>> GetProjectByApprovalStatus(List<int> statusApprobation, int? year)
		{
			// check our list in parameter

			if(statusApprobation == null || statusApprobation.Count <= 0)

				return null;

			// perform a nullable test 

			if(!year.HasValue)
			{
				year = DateTime.Now.Year; // used current year
			}


			// use Memory cache
			var dataTable = new DataTable();

			// connection
			using(SqlConnection sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				// Create Query
				string query = $@"SELECT MONTH(CreationDate) AS monthNumber, Id_State AS statusesApprobation, Count(Id) AS count_project FROM __FNC_BudgetProject WHERE Id_State  IN({string.Join(',', statusApprobation)})  AND YEAR(CreationDate) = @Year  GROUP BY MONTH(CreationDate), Id_State  ORDER BY MONTH(MIN(CreationDate));";

				//Create command Object

				SqlCommand command = new SqlCommand(query, sqlConnection);

				command.Parameters.AddWithValue("Year", year);

				// execute our request
				new SqlDataAdapter(command).Fill(dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(row => new Tuple<int, int, int>(
						Convert.ToInt32(row["monthNumber"]),
						Convert.ToInt32(row["statusesApprobation"]),
						Convert.ToInt32(row["count_project"]))).ToList();

			}
			else
			{
				return null;
			}
		}

		#endregion

		#endregion
	}
}
