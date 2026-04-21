using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class ProjectHistoryAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.ProjectHistoryEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_ProjectHistory] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.ProjectHistoryEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.ProjectHistoryEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_ProjectHistory]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.ProjectHistoryEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.ProjectHistoryEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.ProjectHistoryEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.ProjectHistoryEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.ProjectHistoryEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.ProjectHistoryEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.ProjectHistoryEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__FNC_ProjectHistory] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.ProjectHistoryEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.ProjectHistoryEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.ProjectHistoryEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.ProjectHistoryEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__FNC_ProjectHistory] ([ApprovalTime],[ApprovalUserEmail],[ApprovalUserId],[ApprovalUserName],[Archived],[ArchiveTime],[ArchiveUserId],[BudgetYear],[Closed],[ClosedTime],[ClosedUserEmail],[ClosedUserId],[ClosedUserName],[CompanyId],[CompanyName],[CreationDate],[CurrencyId],[CurrencyName],[CustomerId],[CustomerName],[CustomerNr],[Deleted],[DeleteTime],[DeleteUserId],[DepartmentId],[DepartmentName],[Description],[HistoryTime],[HistoryUserEmail],[HistoryUserId],[HistoryUserName],[Id_State],[Id_Type],[OrderCount],[ProjectBudget],[ProjectId],[ProjectName],[ProjectStatus],[ProjectStatusChangeTime],[ProjectStatusChangeUserId],[ProjectStatusChangeUserName],[ProjectStatusName],[PSZOffer],[ResponsableEmail],[ResponsableId],[ResponsableName],[TotalSpent],[Type])  VALUES (@ApprovalTime,@ApprovalUserEmail,@ApprovalUserId,@ApprovalUserName,@Archived,@ArchiveTime,@ArchiveUserId,@BudgetYear,@Closed,@ClosedTime,@ClosedUserEmail,@ClosedUserId,@ClosedUserName,@CompanyId,@CompanyName,@CreationDate,@CurrencyId,@CurrencyName,@CustomerId,@CustomerName,@CustomerNr,@Deleted,@DeleteTime,@DeleteUserId,@DepartmentId,@DepartmentName,@Description,@HistoryTime,@HistoryUserEmail,@HistoryUserId,@HistoryUserName,@Id_State,@Id_Type,@OrderCount,@ProjectBudget,@ProjectId,@ProjectName,@ProjectStatus,@ProjectStatusChangeTime,@ProjectStatusChangeUserId,@ProjectStatusChangeUserName,@ProjectStatusName,@PSZOffer,@ResponsableEmail,@ResponsableId,@ResponsableName,@TotalSpent,@Type); ";
				query += "SELECT SCOPE_IDENTITY();";

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
					sqlCommand.Parameters.AddWithValue("HistoryTime", item.HistoryTime);
					sqlCommand.Parameters.AddWithValue("HistoryUserEmail", item.HistoryUserEmail == null ? (object)DBNull.Value : item.HistoryUserEmail);
					sqlCommand.Parameters.AddWithValue("HistoryUserId", item.HistoryUserId);
					sqlCommand.Parameters.AddWithValue("HistoryUserName", item.HistoryUserName == null ? (object)DBNull.Value : item.HistoryUserName);
					sqlCommand.Parameters.AddWithValue("Id_State", item.Id_State);
					sqlCommand.Parameters.AddWithValue("Id_Type", item.Id_Type);
					sqlCommand.Parameters.AddWithValue("OrderCount", item.OrderCount == null ? (object)DBNull.Value : item.OrderCount);
					sqlCommand.Parameters.AddWithValue("ProjectBudget", item.ProjectBudget);
					sqlCommand.Parameters.AddWithValue("ProjectId", item.ProjectId);
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

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.ProjectHistoryEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 49; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.ProjectHistoryEntity> items)
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
						query += " INSERT INTO [__FNC_ProjectHistory] ([ApprovalTime],[ApprovalUserEmail],[ApprovalUserId],[ApprovalUserName],[Archived],[ArchiveTime],[ArchiveUserId],[BudgetYear],[Closed],[ClosedTime],[ClosedUserEmail],[ClosedUserId],[ClosedUserName],[CompanyId],[CompanyName],[CreationDate],[CurrencyId],[CurrencyName],[CustomerId],[CustomerName],[CustomerNr],[Deleted],[DeleteTime],[DeleteUserId],[DepartmentId],[DepartmentName],[Description],[HistoryTime],[HistoryUserEmail],[HistoryUserId],[HistoryUserName],[Id_State],[Id_Type],[OrderCount],[ProjectBudget],[ProjectId],[ProjectName],[ProjectStatus],[ProjectStatusChangeTime],[ProjectStatusChangeUserId],[ProjectStatusChangeUserName],[ProjectStatusName],[PSZOffer],[ResponsableEmail],[ResponsableId],[ResponsableName],[TotalSpent],[Type]) VALUES ( "

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
							+ "@HistoryTime" + i + ","
							+ "@HistoryUserEmail" + i + ","
							+ "@HistoryUserId" + i + ","
							+ "@HistoryUserName" + i + ","
							+ "@Id_State" + i + ","
							+ "@Id_Type" + i + ","
							+ "@OrderCount" + i + ","
							+ "@ProjectBudget" + i + ","
							+ "@ProjectId" + i + ","
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
						sqlCommand.Parameters.AddWithValue("HistoryTime" + i, item.HistoryTime);
						sqlCommand.Parameters.AddWithValue("HistoryUserEmail" + i, item.HistoryUserEmail == null ? (object)DBNull.Value : item.HistoryUserEmail);
						sqlCommand.Parameters.AddWithValue("HistoryUserId" + i, item.HistoryUserId);
						sqlCommand.Parameters.AddWithValue("HistoryUserName" + i, item.HistoryUserName == null ? (object)DBNull.Value : item.HistoryUserName);
						sqlCommand.Parameters.AddWithValue("Id_State" + i, item.Id_State);
						sqlCommand.Parameters.AddWithValue("Id_Type" + i, item.Id_Type);
						sqlCommand.Parameters.AddWithValue("OrderCount" + i, item.OrderCount == null ? (object)DBNull.Value : item.OrderCount);
						sqlCommand.Parameters.AddWithValue("ProjectBudget" + i, item.ProjectBudget);
						sqlCommand.Parameters.AddWithValue("ProjectId" + i, item.ProjectId);
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

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.ProjectHistoryEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_ProjectHistory] SET [ApprovalTime]=@ApprovalTime, [ApprovalUserEmail]=@ApprovalUserEmail, [ApprovalUserId]=@ApprovalUserId, [ApprovalUserName]=@ApprovalUserName, [Archived]=@Archived, [ArchiveTime]=@ArchiveTime, [ArchiveUserId]=@ArchiveUserId, [BudgetYear]=@BudgetYear, [Closed]=@Closed, [ClosedTime]=@ClosedTime, [ClosedUserEmail]=@ClosedUserEmail, [ClosedUserId]=@ClosedUserId, [ClosedUserName]=@ClosedUserName, [CompanyId]=@CompanyId, [CompanyName]=@CompanyName, [CreationDate]=@CreationDate, [CurrencyId]=@CurrencyId, [CurrencyName]=@CurrencyName, [CustomerId]=@CustomerId, [CustomerName]=@CustomerName, [CustomerNr]=@CustomerNr, [Deleted]=@Deleted, [DeleteTime]=@DeleteTime, [DeleteUserId]=@DeleteUserId, [DepartmentId]=@DepartmentId, [DepartmentName]=@DepartmentName, [Description]=@Description, [HistoryTime]=@HistoryTime, [HistoryUserEmail]=@HistoryUserEmail, [HistoryUserId]=@HistoryUserId, [HistoryUserName]=@HistoryUserName, [Id_State]=@Id_State, [Id_Type]=@Id_Type, [OrderCount]=@OrderCount, [ProjectBudget]=@ProjectBudget, [ProjectId]=@ProjectId, [ProjectName]=@ProjectName, [ProjectStatus]=@ProjectStatus, [ProjectStatusChangeTime]=@ProjectStatusChangeTime, [ProjectStatusChangeUserId]=@ProjectStatusChangeUserId, [ProjectStatusChangeUserName]=@ProjectStatusChangeUserName, [ProjectStatusName]=@ProjectStatusName, [PSZOffer]=@PSZOffer, [ResponsableEmail]=@ResponsableEmail, [ResponsableId]=@ResponsableId, [ResponsableName]=@ResponsableName, [TotalSpent]=@TotalSpent, [Type]=@Type WHERE [Id]=@Id";
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
				sqlCommand.Parameters.AddWithValue("HistoryTime", item.HistoryTime);
				sqlCommand.Parameters.AddWithValue("HistoryUserEmail", item.HistoryUserEmail == null ? (object)DBNull.Value : item.HistoryUserEmail);
				sqlCommand.Parameters.AddWithValue("HistoryUserId", item.HistoryUserId);
				sqlCommand.Parameters.AddWithValue("HistoryUserName", item.HistoryUserName == null ? (object)DBNull.Value : item.HistoryUserName);
				sqlCommand.Parameters.AddWithValue("Id_State", item.Id_State);
				sqlCommand.Parameters.AddWithValue("Id_Type", item.Id_Type);
				sqlCommand.Parameters.AddWithValue("OrderCount", item.OrderCount == null ? (object)DBNull.Value : item.OrderCount);
				sqlCommand.Parameters.AddWithValue("ProjectBudget", item.ProjectBudget);
				sqlCommand.Parameters.AddWithValue("ProjectId", item.ProjectId);
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
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.ProjectHistoryEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 49; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.ProjectHistoryEntity> items)
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
						query += " UPDATE [__FNC_ProjectHistory] SET "

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
							+ "[HistoryTime]=@HistoryTime" + i + ","
							+ "[HistoryUserEmail]=@HistoryUserEmail" + i + ","
							+ "[HistoryUserId]=@HistoryUserId" + i + ","
							+ "[HistoryUserName]=@HistoryUserName" + i + ","
							+ "[Id_State]=@Id_State" + i + ","
							+ "[Id_Type]=@Id_Type" + i + ","
							+ "[OrderCount]=@OrderCount" + i + ","
							+ "[ProjectBudget]=@ProjectBudget" + i + ","
							+ "[ProjectId]=@ProjectId" + i + ","
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
						sqlCommand.Parameters.AddWithValue("HistoryTime" + i, item.HistoryTime);
						sqlCommand.Parameters.AddWithValue("HistoryUserEmail" + i, item.HistoryUserEmail == null ? (object)DBNull.Value : item.HistoryUserEmail);
						sqlCommand.Parameters.AddWithValue("HistoryUserId" + i, item.HistoryUserId);
						sqlCommand.Parameters.AddWithValue("HistoryUserName" + i, item.HistoryUserName == null ? (object)DBNull.Value : item.HistoryUserName);
						sqlCommand.Parameters.AddWithValue("Id_State" + i, item.Id_State);
						sqlCommand.Parameters.AddWithValue("Id_Type" + i, item.Id_Type);
						sqlCommand.Parameters.AddWithValue("OrderCount" + i, item.OrderCount == null ? (object)DBNull.Value : item.OrderCount);
						sqlCommand.Parameters.AddWithValue("ProjectBudget" + i, item.ProjectBudget);
						sqlCommand.Parameters.AddWithValue("ProjectId" + i, item.ProjectId);
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
				string query = "DELETE FROM [__FNC_ProjectHistory] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__FNC_ProjectHistory] WHERE [Id] IN (" + queryIds + ")";
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
	}
}
