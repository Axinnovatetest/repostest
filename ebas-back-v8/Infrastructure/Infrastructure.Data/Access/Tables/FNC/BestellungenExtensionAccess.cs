using System.ComponentModel;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.FNC
{
	public class BestellungenExtensionEnums
	{
		public enum OrderPaymentTypes: int
		{
			[Description("Purchase")]
			Purchase = 0,
			[Description("Leasing")]
			Leasing = 1
		}
	}
	public class BestellungenExtensionAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity Get(int id, bool isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_BestellungenExtension] WHERE [Id]=@Id AND [Archived]=@archived AND [Deleted]=@deleted";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> Get(int? year = null, bool? booked = null, bool isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BestellungenExtension] AS E {(!booked.HasValue ? " WHERE " : "LEFT JOIN[Bestellungen] AS B ON E.OrderId = B.Nr WHERE B.Erledigt = @booked AND ")} [Archived]=@archived AND [Deleted]=@deleted {(year.HasValue ? $" AND Year(CreationDate)={year.Value}" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				if(booked.HasValue)
					sqlCommand.Parameters.AddWithValue("booked", booked);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> Get(List<int> ids, bool isArchived = false, bool isDeleted = false)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids, isArchived, isDeleted);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber), isArchived, isDeleted));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), isArchived, isDeleted));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> get(List<int> ids, bool isArchived = false, bool isDeleted = false)
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

					sqlCommand.CommandText = $"SELECT * FROM [__FNC_BestellungenExtension] WHERE [Id] IN ({queryIds}) AND [Archived]=@archived AND [Deleted]=@deleted";
					sqlCommand.Parameters.AddWithValue("archived", isArchived);
					sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__FNC_BestellungenExtension] ([AllocationType],[AllocationTypeName],[ApprovalTime],[ApprovalUserId],[Archived],[ArchiveTime],[ArchiveUserId],[BillingAddress],[BillingCompanyId],[BillingCompanyName],[BillingContactName],[BillingDepartmentId],[BillingDepartmentName],[BillingFax],[BillingTelephone],[BudgetYear],[CompanyId],[CompanyName],[CreationDate],[CurrencyId],[CurrencyName],[DefaultCurrencyDecimals],[DefaultCurrencyId],[DefaultCurrencyName],[DefaultCurrencyRate],[Deleted],[DeleteTime],[DeleteUserId],[DeliveryAddress],[DeliveryCompanyId],[DeliveryCompanyName],[DeliveryDepartmentId],[DeliveryDepartmentName],[DeliveryFax],[DeliveryTelephone],[DepartmentId],[DepartmentName],[Description],[Discount],[InternalContact],[IssuerId],[IssuerName],[LastRejectionLevel],[LastRejectionTime],[LastRejectionUserId],[LeasingMonthAmount],[LeasingNbMonths],[LeasingStartMonth],[LeasingStartYear],[LeasingTotalAmount],[Level],[LocationId],[MandantId],[OrderId],[OrderNumber],[OrderPlacedEmailMessage],[OrderPlacedEmailTitle],[OrderPlacedReportFileId],[OrderPlacedSendingEmail],[OrderPlacedSupplierEmail],[OrderPlacedTime],[OrderPlacedUserEmail],[OrderPlacedUserId],[OrderPlacedUserName],[OrderPlacementCCEmail],[OrderType],[PoPaymentType],[PoPaymentTypeName],[ProjectId],[ProjectName],[Status],[StorageLocationId],[StorageLocationName],[SupplierEmail],[SupplierFax],[SupplierId],[SupplierName],[SupplierNumber],[SupplierNummer],[SupplierPaymentMethod],[SupplierPaymentTerm],[SupplierTelephone],[SupplierTradingTerm],[ValidationRequestTime])  VALUES (@AllocationType,@AllocationTypeName,@ApprovalTime,@ApprovalUserId,@Archived,@ArchiveTime,@ArchiveUserId,@BillingAddress,@BillingCompanyId,@BillingCompanyName,@BillingContactName,@BillingDepartmentId,@BillingDepartmentName,@BillingFax,@BillingTelephone,@BudgetYear,@CompanyId,@CompanyName,@CreationDate,@CurrencyId,@CurrencyName,@DefaultCurrencyDecimals,@DefaultCurrencyId,@DefaultCurrencyName,@DefaultCurrencyRate,@Deleted,@DeleteTime,@DeleteUserId,@DeliveryAddress,@DeliveryCompanyId,@DeliveryCompanyName,@DeliveryDepartmentId,@DeliveryDepartmentName,@DeliveryFax,@DeliveryTelephone,@DepartmentId,@DepartmentName,@Description,@Discount,@InternalContact,@IssuerId,@IssuerName,@LastRejectionLevel,@LastRejectionTime,@LastRejectionUserId,@LeasingMonthAmount,@LeasingNbMonths,@LeasingStartMonth,@LeasingStartYear,@LeasingTotalAmount,@Level,@LocationId,@MandantId,@OrderId,@OrderNumber,@OrderPlacedEmailMessage,@OrderPlacedEmailTitle,@OrderPlacedReportFileId,@OrderPlacedSendingEmail,@OrderPlacedSupplierEmail,@OrderPlacedTime,@OrderPlacedUserEmail,@OrderPlacedUserId,@OrderPlacedUserName,@OrderPlacementCCEmail,@OrderType,@PoPaymentType,@PoPaymentTypeName,@ProjectId,@ProjectName,@Status,@StorageLocationId,@StorageLocationName,@SupplierEmail,@SupplierFax,@SupplierId,@SupplierName,@SupplierNumber,@SupplierNummer,@SupplierPaymentMethod,@SupplierPaymentTerm,@SupplierTelephone,@SupplierTradingTerm,@ValidationRequestTime); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AllocationType", item.AllocationType);
					sqlCommand.Parameters.AddWithValue("AllocationTypeName", item.AllocationTypeName == null ? (object)DBNull.Value : item.AllocationTypeName);
					sqlCommand.Parameters.AddWithValue("ApprovalTime", item.ApprovalTime == null ? (object)DBNull.Value : item.ApprovalTime);
					sqlCommand.Parameters.AddWithValue("ApprovalUserId", item.ApprovalUserId == null ? (object)DBNull.Value : item.ApprovalUserId);
					sqlCommand.Parameters.AddWithValue("Archived", item.Archived == null ? (object)DBNull.Value : item.Archived);
					sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
					sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
					sqlCommand.Parameters.AddWithValue("BillingAddress", item.BillingAddress == null ? (object)DBNull.Value : item.BillingAddress);
					sqlCommand.Parameters.AddWithValue("BillingCompanyId", item.BillingCompanyId == null ? (object)DBNull.Value : item.BillingCompanyId);
					sqlCommand.Parameters.AddWithValue("BillingCompanyName", item.BillingCompanyName == null ? (object)DBNull.Value : item.BillingCompanyName);
					sqlCommand.Parameters.AddWithValue("BillingContactName", item.BillingContactName == null ? (object)DBNull.Value : item.BillingContactName);
					sqlCommand.Parameters.AddWithValue("BillingDepartmentId", item.BillingDepartmentId == null ? (object)DBNull.Value : item.BillingDepartmentId);
					sqlCommand.Parameters.AddWithValue("BillingDepartmentName", item.BillingDepartmentName == null ? (object)DBNull.Value : item.BillingDepartmentName);
					sqlCommand.Parameters.AddWithValue("BillingFax", item.BillingFax == null ? (object)DBNull.Value : item.BillingFax);
					sqlCommand.Parameters.AddWithValue("BillingTelephone", item.BillingTelephone == null ? (object)DBNull.Value : item.BillingTelephone);
					sqlCommand.Parameters.AddWithValue("BudgetYear", item.BudgetYear);
					sqlCommand.Parameters.AddWithValue("CompanyId", item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
					sqlCommand.Parameters.AddWithValue("CompanyName", item.CompanyName == null ? (object)DBNull.Value : item.CompanyName);
					sqlCommand.Parameters.AddWithValue("CreationDate", item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
					sqlCommand.Parameters.AddWithValue("CurrencyId", item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
					sqlCommand.Parameters.AddWithValue("CurrencyName", item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyDecimals", item.DefaultCurrencyDecimals == null ? (object)DBNull.Value : item.DefaultCurrencyDecimals);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyId", item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyName", item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyRate", item.DefaultCurrencyRate == null ? (object)DBNull.Value : item.DefaultCurrencyRate);
					sqlCommand.Parameters.AddWithValue("Deleted", item.Deleted == null ? (object)DBNull.Value : item.Deleted);
					sqlCommand.Parameters.AddWithValue("DeleteTime", item.DeleteTime == null ? (object)DBNull.Value : item.DeleteTime);
					sqlCommand.Parameters.AddWithValue("DeleteUserId", item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
					sqlCommand.Parameters.AddWithValue("DeliveryAddress", item.DeliveryAddress == null ? (object)DBNull.Value : item.DeliveryAddress);
					sqlCommand.Parameters.AddWithValue("DeliveryCompanyId", item.DeliveryCompanyId == null ? (object)DBNull.Value : item.DeliveryCompanyId);
					sqlCommand.Parameters.AddWithValue("DeliveryCompanyName", item.DeliveryCompanyName == null ? (object)DBNull.Value : item.DeliveryCompanyName);
					sqlCommand.Parameters.AddWithValue("DeliveryDepartmentId", item.DeliveryDepartmentId == null ? (object)DBNull.Value : item.DeliveryDepartmentId);
					sqlCommand.Parameters.AddWithValue("DeliveryDepartmentName", item.DeliveryDepartmentName == null ? (object)DBNull.Value : item.DeliveryDepartmentName);
					sqlCommand.Parameters.AddWithValue("DeliveryFax", item.DeliveryFax == null ? (object)DBNull.Value : item.DeliveryFax);
					sqlCommand.Parameters.AddWithValue("DeliveryTelephone", item.DeliveryTelephone == null ? (object)DBNull.Value : item.DeliveryTelephone);
					sqlCommand.Parameters.AddWithValue("DepartmentId", item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
					sqlCommand.Parameters.AddWithValue("DepartmentName", item.DepartmentName == null ? (object)DBNull.Value : item.DepartmentName);
					sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
					sqlCommand.Parameters.AddWithValue("Discount", item.Discount == null ? (object)DBNull.Value : item.Discount);
					sqlCommand.Parameters.AddWithValue("InternalContact", item.InternalContact == null ? (object)DBNull.Value : item.InternalContact);
					sqlCommand.Parameters.AddWithValue("IssuerId", item.IssuerId);
					sqlCommand.Parameters.AddWithValue("IssuerName", item.IssuerName == null ? (object)DBNull.Value : item.IssuerName);
					sqlCommand.Parameters.AddWithValue("LastRejectionLevel", item.LastRejectionLevel == null ? (object)DBNull.Value : item.LastRejectionLevel);
					sqlCommand.Parameters.AddWithValue("LastRejectionTime", item.LastRejectionTime == null ? (object)DBNull.Value : item.LastRejectionTime);
					sqlCommand.Parameters.AddWithValue("LastRejectionUserId", item.LastRejectionUserId == null ? (object)DBNull.Value : item.LastRejectionUserId);
					sqlCommand.Parameters.AddWithValue("LeasingMonthAmount", item.LeasingMonthAmount == null ? (object)DBNull.Value : item.LeasingMonthAmount);
					sqlCommand.Parameters.AddWithValue("LeasingNbMonths", item.LeasingNbMonths == null ? (object)DBNull.Value : item.LeasingNbMonths);
					sqlCommand.Parameters.AddWithValue("LeasingStartMonth", item.LeasingStartMonth == null ? (object)DBNull.Value : item.LeasingStartMonth);
					sqlCommand.Parameters.AddWithValue("LeasingStartYear", item.LeasingStartYear == null ? (object)DBNull.Value : item.LeasingStartYear);
					sqlCommand.Parameters.AddWithValue("LeasingTotalAmount", item.LeasingTotalAmount == null ? (object)DBNull.Value : item.LeasingTotalAmount);
					sqlCommand.Parameters.AddWithValue("Level", item.Level == null ? (object)DBNull.Value : item.Level);
					sqlCommand.Parameters.AddWithValue("LocationId", item.LocationId == null ? (object)DBNull.Value : item.LocationId);
					sqlCommand.Parameters.AddWithValue("MandantId", item.MandantId == null ? (object)DBNull.Value : item.MandantId);
					sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
					sqlCommand.Parameters.AddWithValue("OrderNumber", item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
					sqlCommand.Parameters.AddWithValue("OrderPlacedEmailMessage", item.OrderPlacedEmailMessage == null ? (object)DBNull.Value : item.OrderPlacedEmailMessage);
					sqlCommand.Parameters.AddWithValue("OrderPlacedEmailTitle", item.OrderPlacedEmailTitle == null ? (object)DBNull.Value : item.OrderPlacedEmailTitle);
					sqlCommand.Parameters.AddWithValue("OrderPlacedReportFileId", item.OrderPlacedReportFileId == null ? (object)DBNull.Value : item.OrderPlacedReportFileId);
					sqlCommand.Parameters.AddWithValue("OrderPlacedSendingEmail", item.OrderPlacedSendingEmail == null ? (object)DBNull.Value : item.OrderPlacedSendingEmail);
					sqlCommand.Parameters.AddWithValue("OrderPlacedSupplierEmail", item.OrderPlacedSupplierEmail == null ? (object)DBNull.Value : item.OrderPlacedSupplierEmail);
					sqlCommand.Parameters.AddWithValue("OrderPlacedTime", item.OrderPlacedTime == null ? (object)DBNull.Value : item.OrderPlacedTime);
					sqlCommand.Parameters.AddWithValue("OrderPlacedUserEmail", item.OrderPlacedUserEmail == null ? (object)DBNull.Value : item.OrderPlacedUserEmail);
					sqlCommand.Parameters.AddWithValue("OrderPlacedUserId", item.OrderPlacedUserId == null ? (object)DBNull.Value : item.OrderPlacedUserId);
					sqlCommand.Parameters.AddWithValue("OrderPlacedUserName", item.OrderPlacedUserName == null ? (object)DBNull.Value : item.OrderPlacedUserName);
					sqlCommand.Parameters.AddWithValue("OrderPlacementCCEmail", item.OrderPlacementCCEmail == null ? (object)DBNull.Value : item.OrderPlacementCCEmail);
					sqlCommand.Parameters.AddWithValue("OrderType", item.OrderType == null ? (object)DBNull.Value : item.OrderType);
					sqlCommand.Parameters.AddWithValue("PoPaymentType", item.PoPaymentType == null ? (object)DBNull.Value : item.PoPaymentType);
					sqlCommand.Parameters.AddWithValue("PoPaymentTypeName", item.PoPaymentTypeName == null ? (object)DBNull.Value : item.PoPaymentTypeName);
					sqlCommand.Parameters.AddWithValue("ProjectId", item.ProjectId == null ? (object)DBNull.Value : item.ProjectId);
					sqlCommand.Parameters.AddWithValue("ProjectName", item.ProjectName == null ? (object)DBNull.Value : item.ProjectName);
					sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("StorageLocationId", item.StorageLocationId == null ? (object)DBNull.Value : item.StorageLocationId);
					sqlCommand.Parameters.AddWithValue("StorageLocationName", item.StorageLocationName == null ? (object)DBNull.Value : item.StorageLocationName);
					sqlCommand.Parameters.AddWithValue("SupplierEmail", item.SupplierEmail == null ? (object)DBNull.Value : item.SupplierEmail);
					sqlCommand.Parameters.AddWithValue("SupplierFax", item.SupplierFax == null ? (object)DBNull.Value : item.SupplierFax);
					sqlCommand.Parameters.AddWithValue("SupplierId", item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
					sqlCommand.Parameters.AddWithValue("SupplierName", item.SupplierName == null ? (object)DBNull.Value : item.SupplierName);
					sqlCommand.Parameters.AddWithValue("SupplierNumber", item.SupplierNumber == null ? (object)DBNull.Value : item.SupplierNumber);
					sqlCommand.Parameters.AddWithValue("SupplierNummer", item.SupplierNummer == null ? (object)DBNull.Value : item.SupplierNummer);
					sqlCommand.Parameters.AddWithValue("SupplierPaymentMethod", item.SupplierPaymentMethod == null ? (object)DBNull.Value : item.SupplierPaymentMethod);
					sqlCommand.Parameters.AddWithValue("SupplierPaymentTerm", item.SupplierPaymentTerm == null ? (object)DBNull.Value : item.SupplierPaymentTerm);
					sqlCommand.Parameters.AddWithValue("SupplierTelephone", item.SupplierTelephone == null ? (object)DBNull.Value : item.SupplierTelephone);
					sqlCommand.Parameters.AddWithValue("SupplierTradingTerm", item.SupplierTradingTerm == null ? (object)DBNull.Value : item.SupplierTradingTerm);
					sqlCommand.Parameters.AddWithValue("ValidationRequestTime", item.ValidationRequestTime == null ? (object)DBNull.Value : item.ValidationRequestTime);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 85; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> items)
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
						query += " INSERT INTO [__FNC_BestellungenExtension] ([AllocationType],[AllocationTypeName],[ApprovalTime],[ApprovalUserId],[Archived],[ArchiveTime],[ArchiveUserId],[BillingAddress],[BillingCompanyId],[BillingCompanyName],[BillingContactName],[BillingDepartmentId],[BillingDepartmentName],[BillingFax],[BillingTelephone],[BudgetYear],[CompanyId],[CompanyName],[CreationDate],[CurrencyId],[CurrencyName],[DefaultCurrencyDecimals],[DefaultCurrencyId],[DefaultCurrencyName],[DefaultCurrencyRate],[Deleted],[DeleteTime],[DeleteUserId],[DeliveryAddress],[DeliveryCompanyId],[DeliveryCompanyName],[DeliveryDepartmentId],[DeliveryDepartmentName],[DeliveryFax],[DeliveryTelephone],[DepartmentId],[DepartmentName],[Description],[Discount],[InternalContact],[IssuerId],[IssuerName],[LastRejectionLevel],[LastRejectionTime],[LastRejectionUserId],[LeasingMonthAmount],[LeasingNbMonths],[LeasingStartMonth],[LeasingStartYear],[LeasingTotalAmount],[Level],[LocationId],[MandantId],[OrderId],[OrderNumber],[OrderPlacedEmailMessage],[OrderPlacedEmailTitle],[OrderPlacedReportFileId],[OrderPlacedSendingEmail],[OrderPlacedSupplierEmail],[OrderPlacedTime],[OrderPlacedUserEmail],[OrderPlacedUserId],[OrderPlacedUserName],[OrderPlacementCCEmail],[OrderType],[PoPaymentType],[PoPaymentTypeName],[ProjectId],[ProjectName],[Status],[StorageLocationId],[StorageLocationName],[SupplierEmail],[SupplierFax],[SupplierId],[SupplierName],[SupplierNumber],[SupplierNummer],[SupplierPaymentMethod],[SupplierPaymentTerm],[SupplierTelephone],[SupplierTradingTerm],[ValidationRequestTime]) VALUES ( "

							+ "@AllocationType" + i + ","
							+ "@AllocationTypeName" + i + ","
							+ "@ApprovalTime" + i + ","
							+ "@ApprovalUserId" + i + ","
							+ "@Archived" + i + ","
							+ "@ArchiveTime" + i + ","
							+ "@ArchiveUserId" + i + ","
							+ "@BillingAddress" + i + ","
							+ "@BillingCompanyId" + i + ","
							+ "@BillingCompanyName" + i + ","
							+ "@BillingContactName" + i + ","
							+ "@BillingDepartmentId" + i + ","
							+ "@BillingDepartmentName" + i + ","
							+ "@BillingFax" + i + ","
							+ "@BillingTelephone" + i + ","
							+ "@BudgetYear" + i + ","
							+ "@CompanyId" + i + ","
							+ "@CompanyName" + i + ","
							+ "@CreationDate" + i + ","
							+ "@CurrencyId" + i + ","
							+ "@CurrencyName" + i + ","
							+ "@DefaultCurrencyDecimals" + i + ","
							+ "@DefaultCurrencyId" + i + ","
							+ "@DefaultCurrencyName" + i + ","
							+ "@DefaultCurrencyRate" + i + ","
							+ "@Deleted" + i + ","
							+ "@DeleteTime" + i + ","
							+ "@DeleteUserId" + i + ","
							+ "@DeliveryAddress" + i + ","
							+ "@DeliveryCompanyId" + i + ","
							+ "@DeliveryCompanyName" + i + ","
							+ "@DeliveryDepartmentId" + i + ","
							+ "@DeliveryDepartmentName" + i + ","
							+ "@DeliveryFax" + i + ","
							+ "@DeliveryTelephone" + i + ","
							+ "@DepartmentId" + i + ","
							+ "@DepartmentName" + i + ","
							+ "@Description" + i + ","
							+ "@Discount" + i + ","
							+ "@InternalContact" + i + ","
							+ "@IssuerId" + i + ","
							+ "@IssuerName" + i + ","
							+ "@LastRejectionLevel" + i + ","
							+ "@LastRejectionTime" + i + ","
							+ "@LastRejectionUserId" + i + ","
							+ "@LeasingMonthAmount" + i + ","
							+ "@LeasingNbMonths" + i + ","
							+ "@LeasingStartMonth" + i + ","
							+ "@LeasingStartYear" + i + ","
							+ "@LeasingTotalAmount" + i + ","
							+ "@Level" + i + ","
							+ "@LocationId" + i + ","
							+ "@MandantId" + i + ","
							+ "@OrderId" + i + ","
							+ "@OrderNumber" + i + ","
							+ "@OrderPlacedEmailMessage" + i + ","
							+ "@OrderPlacedEmailTitle" + i + ","
							+ "@OrderPlacedReportFileId" + i + ","
							+ "@OrderPlacedSendingEmail" + i + ","
							+ "@OrderPlacedSupplierEmail" + i + ","
							+ "@OrderPlacedTime" + i + ","
							+ "@OrderPlacedUserEmail" + i + ","
							+ "@OrderPlacedUserId" + i + ","
							+ "@OrderPlacedUserName" + i + ","
							+ "@OrderPlacementCCEmail" + i + ","
							+ "@OrderType" + i + ","
							+ "@PoPaymentType" + i + ","
							+ "@PoPaymentTypeName" + i + ","
							+ "@ProjectId" + i + ","
							+ "@ProjectName" + i + ","
							+ "@Status" + i + ","
							+ "@StorageLocationId" + i + ","
							+ "@StorageLocationName" + i + ","
							+ "@SupplierEmail" + i + ","
							+ "@SupplierFax" + i + ","
							+ "@SupplierId" + i + ","
							+ "@SupplierName" + i + ","
							+ "@SupplierNumber" + i + ","
							+ "@SupplierNummer" + i + ","
							+ "@SupplierPaymentMethod" + i + ","
							+ "@SupplierPaymentTerm" + i + ","
							+ "@SupplierTelephone" + i + ","
							+ "@SupplierTradingTerm" + i + ","
							+ "@ValidationRequestTime" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("AllocationType" + i, item.AllocationType);
						sqlCommand.Parameters.AddWithValue("AllocationTypeName" + i, item.AllocationTypeName == null ? (object)DBNull.Value : item.AllocationTypeName);
						sqlCommand.Parameters.AddWithValue("ApprovalTime" + i, item.ApprovalTime == null ? (object)DBNull.Value : item.ApprovalTime);
						sqlCommand.Parameters.AddWithValue("ApprovalUserId" + i, item.ApprovalUserId == null ? (object)DBNull.Value : item.ApprovalUserId);
						sqlCommand.Parameters.AddWithValue("Archived" + i, item.Archived == null ? (object)DBNull.Value : item.Archived);
						sqlCommand.Parameters.AddWithValue("ArchiveTime" + i, item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
						sqlCommand.Parameters.AddWithValue("ArchiveUserId" + i, item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
						sqlCommand.Parameters.AddWithValue("BillingAddress" + i, item.BillingAddress == null ? (object)DBNull.Value : item.BillingAddress);
						sqlCommand.Parameters.AddWithValue("BillingCompanyId" + i, item.BillingCompanyId == null ? (object)DBNull.Value : item.BillingCompanyId);
						sqlCommand.Parameters.AddWithValue("BillingCompanyName" + i, item.BillingCompanyName == null ? (object)DBNull.Value : item.BillingCompanyName);
						sqlCommand.Parameters.AddWithValue("BillingContactName" + i, item.BillingContactName == null ? (object)DBNull.Value : item.BillingContactName);
						sqlCommand.Parameters.AddWithValue("BillingDepartmentId" + i, item.BillingDepartmentId == null ? (object)DBNull.Value : item.BillingDepartmentId);
						sqlCommand.Parameters.AddWithValue("BillingDepartmentName" + i, item.BillingDepartmentName == null ? (object)DBNull.Value : item.BillingDepartmentName);
						sqlCommand.Parameters.AddWithValue("BillingFax" + i, item.BillingFax == null ? (object)DBNull.Value : item.BillingFax);
						sqlCommand.Parameters.AddWithValue("BillingTelephone" + i, item.BillingTelephone == null ? (object)DBNull.Value : item.BillingTelephone);
						sqlCommand.Parameters.AddWithValue("BudgetYear" + i, item.BudgetYear);
						sqlCommand.Parameters.AddWithValue("CompanyId" + i, item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
						sqlCommand.Parameters.AddWithValue("CompanyName" + i, item.CompanyName == null ? (object)DBNull.Value : item.CompanyName);
						sqlCommand.Parameters.AddWithValue("CreationDate" + i, item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
						sqlCommand.Parameters.AddWithValue("CurrencyId" + i, item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
						sqlCommand.Parameters.AddWithValue("CurrencyName" + i, item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyDecimals" + i, item.DefaultCurrencyDecimals == null ? (object)DBNull.Value : item.DefaultCurrencyDecimals);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyId" + i, item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyName" + i, item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyRate" + i, item.DefaultCurrencyRate == null ? (object)DBNull.Value : item.DefaultCurrencyRate);
						sqlCommand.Parameters.AddWithValue("Deleted" + i, item.Deleted == null ? (object)DBNull.Value : item.Deleted);
						sqlCommand.Parameters.AddWithValue("DeleteTime" + i, item.DeleteTime == null ? (object)DBNull.Value : item.DeleteTime);
						sqlCommand.Parameters.AddWithValue("DeleteUserId" + i, item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
						sqlCommand.Parameters.AddWithValue("DeliveryAddress" + i, item.DeliveryAddress == null ? (object)DBNull.Value : item.DeliveryAddress);
						sqlCommand.Parameters.AddWithValue("DeliveryCompanyId" + i, item.DeliveryCompanyId == null ? (object)DBNull.Value : item.DeliveryCompanyId);
						sqlCommand.Parameters.AddWithValue("DeliveryCompanyName" + i, item.DeliveryCompanyName == null ? (object)DBNull.Value : item.DeliveryCompanyName);
						sqlCommand.Parameters.AddWithValue("DeliveryDepartmentId" + i, item.DeliveryDepartmentId == null ? (object)DBNull.Value : item.DeliveryDepartmentId);
						sqlCommand.Parameters.AddWithValue("DeliveryDepartmentName" + i, item.DeliveryDepartmentName == null ? (object)DBNull.Value : item.DeliveryDepartmentName);
						sqlCommand.Parameters.AddWithValue("DeliveryFax" + i, item.DeliveryFax == null ? (object)DBNull.Value : item.DeliveryFax);
						sqlCommand.Parameters.AddWithValue("DeliveryTelephone" + i, item.DeliveryTelephone == null ? (object)DBNull.Value : item.DeliveryTelephone);
						sqlCommand.Parameters.AddWithValue("DepartmentId" + i, item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
						sqlCommand.Parameters.AddWithValue("DepartmentName" + i, item.DepartmentName == null ? (object)DBNull.Value : item.DepartmentName);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("Discount" + i, item.Discount == null ? (object)DBNull.Value : item.Discount);
						sqlCommand.Parameters.AddWithValue("InternalContact" + i, item.InternalContact == null ? (object)DBNull.Value : item.InternalContact);
						sqlCommand.Parameters.AddWithValue("IssuerId" + i, item.IssuerId);
						sqlCommand.Parameters.AddWithValue("IssuerName" + i, item.IssuerName == null ? (object)DBNull.Value : item.IssuerName);
						sqlCommand.Parameters.AddWithValue("LastRejectionLevel" + i, item.LastRejectionLevel == null ? (object)DBNull.Value : item.LastRejectionLevel);
						sqlCommand.Parameters.AddWithValue("LastRejectionTime" + i, item.LastRejectionTime == null ? (object)DBNull.Value : item.LastRejectionTime);
						sqlCommand.Parameters.AddWithValue("LastRejectionUserId" + i, item.LastRejectionUserId == null ? (object)DBNull.Value : item.LastRejectionUserId);
						sqlCommand.Parameters.AddWithValue("LeasingMonthAmount" + i, item.LeasingMonthAmount == null ? (object)DBNull.Value : item.LeasingMonthAmount);
						sqlCommand.Parameters.AddWithValue("LeasingNbMonths" + i, item.LeasingNbMonths == null ? (object)DBNull.Value : item.LeasingNbMonths);
						sqlCommand.Parameters.AddWithValue("LeasingStartMonth" + i, item.LeasingStartMonth == null ? (object)DBNull.Value : item.LeasingStartMonth);
						sqlCommand.Parameters.AddWithValue("LeasingStartYear" + i, item.LeasingStartYear == null ? (object)DBNull.Value : item.LeasingStartYear);
						sqlCommand.Parameters.AddWithValue("LeasingTotalAmount" + i, item.LeasingTotalAmount == null ? (object)DBNull.Value : item.LeasingTotalAmount);
						sqlCommand.Parameters.AddWithValue("Level" + i, item.Level == null ? (object)DBNull.Value : item.Level);
						sqlCommand.Parameters.AddWithValue("LocationId" + i, item.LocationId == null ? (object)DBNull.Value : item.LocationId);
						sqlCommand.Parameters.AddWithValue("MandantId" + i, item.MandantId == null ? (object)DBNull.Value : item.MandantId);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
						sqlCommand.Parameters.AddWithValue("OrderNumber" + i, item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
						sqlCommand.Parameters.AddWithValue("OrderPlacedEmailMessage" + i, item.OrderPlacedEmailMessage == null ? (object)DBNull.Value : item.OrderPlacedEmailMessage);
						sqlCommand.Parameters.AddWithValue("OrderPlacedEmailTitle" + i, item.OrderPlacedEmailTitle == null ? (object)DBNull.Value : item.OrderPlacedEmailTitle);
						sqlCommand.Parameters.AddWithValue("OrderPlacedReportFileId" + i, item.OrderPlacedReportFileId == null ? (object)DBNull.Value : item.OrderPlacedReportFileId);
						sqlCommand.Parameters.AddWithValue("OrderPlacedSendingEmail" + i, item.OrderPlacedSendingEmail == null ? (object)DBNull.Value : item.OrderPlacedSendingEmail);
						sqlCommand.Parameters.AddWithValue("OrderPlacedSupplierEmail" + i, item.OrderPlacedSupplierEmail == null ? (object)DBNull.Value : item.OrderPlacedSupplierEmail);
						sqlCommand.Parameters.AddWithValue("OrderPlacedTime" + i, item.OrderPlacedTime == null ? (object)DBNull.Value : item.OrderPlacedTime);
						sqlCommand.Parameters.AddWithValue("OrderPlacedUserEmail" + i, item.OrderPlacedUserEmail == null ? (object)DBNull.Value : item.OrderPlacedUserEmail);
						sqlCommand.Parameters.AddWithValue("OrderPlacedUserId" + i, item.OrderPlacedUserId == null ? (object)DBNull.Value : item.OrderPlacedUserId);
						sqlCommand.Parameters.AddWithValue("OrderPlacedUserName" + i, item.OrderPlacedUserName == null ? (object)DBNull.Value : item.OrderPlacedUserName);
						sqlCommand.Parameters.AddWithValue("OrderPlacementCCEmail" + i, item.OrderPlacementCCEmail == null ? (object)DBNull.Value : item.OrderPlacementCCEmail);
						sqlCommand.Parameters.AddWithValue("OrderType" + i, item.OrderType == null ? (object)DBNull.Value : item.OrderType);
						sqlCommand.Parameters.AddWithValue("PoPaymentType" + i, item.PoPaymentType == null ? (object)DBNull.Value : item.PoPaymentType);
						sqlCommand.Parameters.AddWithValue("PoPaymentTypeName" + i, item.PoPaymentTypeName == null ? (object)DBNull.Value : item.PoPaymentTypeName);
						sqlCommand.Parameters.AddWithValue("ProjectId" + i, item.ProjectId == null ? (object)DBNull.Value : item.ProjectId);
						sqlCommand.Parameters.AddWithValue("ProjectName" + i, item.ProjectName == null ? (object)DBNull.Value : item.ProjectName);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
						sqlCommand.Parameters.AddWithValue("StorageLocationId" + i, item.StorageLocationId == null ? (object)DBNull.Value : item.StorageLocationId);
						sqlCommand.Parameters.AddWithValue("StorageLocationName" + i, item.StorageLocationName == null ? (object)DBNull.Value : item.StorageLocationName);
						sqlCommand.Parameters.AddWithValue("SupplierEmail" + i, item.SupplierEmail == null ? (object)DBNull.Value : item.SupplierEmail);
						sqlCommand.Parameters.AddWithValue("SupplierFax" + i, item.SupplierFax == null ? (object)DBNull.Value : item.SupplierFax);
						sqlCommand.Parameters.AddWithValue("SupplierId" + i, item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
						sqlCommand.Parameters.AddWithValue("SupplierName" + i, item.SupplierName == null ? (object)DBNull.Value : item.SupplierName);
						sqlCommand.Parameters.AddWithValue("SupplierNumber" + i, item.SupplierNumber == null ? (object)DBNull.Value : item.SupplierNumber);
						sqlCommand.Parameters.AddWithValue("SupplierNummer" + i, item.SupplierNummer == null ? (object)DBNull.Value : item.SupplierNummer);
						sqlCommand.Parameters.AddWithValue("SupplierPaymentMethod" + i, item.SupplierPaymentMethod == null ? (object)DBNull.Value : item.SupplierPaymentMethod);
						sqlCommand.Parameters.AddWithValue("SupplierPaymentTerm" + i, item.SupplierPaymentTerm == null ? (object)DBNull.Value : item.SupplierPaymentTerm);
						sqlCommand.Parameters.AddWithValue("SupplierTelephone" + i, item.SupplierTelephone == null ? (object)DBNull.Value : item.SupplierTelephone);
						sqlCommand.Parameters.AddWithValue("SupplierTradingTerm" + i, item.SupplierTradingTerm == null ? (object)DBNull.Value : item.SupplierTradingTerm);
						sqlCommand.Parameters.AddWithValue("ValidationRequestTime" + i, item.ValidationRequestTime == null ? (object)DBNull.Value : item.ValidationRequestTime);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_BestellungenExtension] SET [AllocationType]=@AllocationType, [AllocationTypeName]=@AllocationTypeName, [ApprovalTime]=@ApprovalTime, [ApprovalUserId]=@ApprovalUserId, [Archived]=@Archived, [ArchiveTime]=@ArchiveTime, [ArchiveUserId]=@ArchiveUserId, [BillingAddress]=@BillingAddress, [BillingCompanyId]=@BillingCompanyId, [BillingCompanyName]=@BillingCompanyName, [BillingContactName]=@BillingContactName, [BillingDepartmentId]=@BillingDepartmentId, [BillingDepartmentName]=@BillingDepartmentName, [BillingFax]=@BillingFax, [BillingTelephone]=@BillingTelephone, [BudgetYear]=@BudgetYear, [CompanyId]=@CompanyId, [CompanyName]=@CompanyName, [CreationDate]=@CreationDate, [CurrencyId]=@CurrencyId, [CurrencyName]=@CurrencyName, [DefaultCurrencyDecimals]=@DefaultCurrencyDecimals, [DefaultCurrencyId]=@DefaultCurrencyId, [DefaultCurrencyName]=@DefaultCurrencyName, [DefaultCurrencyRate]=@DefaultCurrencyRate, [Deleted]=@Deleted, [DeleteTime]=@DeleteTime, [DeleteUserId]=@DeleteUserId, [DeliveryAddress]=@DeliveryAddress, [DeliveryCompanyId]=@DeliveryCompanyId, [DeliveryCompanyName]=@DeliveryCompanyName, [DeliveryDepartmentId]=@DeliveryDepartmentId, [DeliveryDepartmentName]=@DeliveryDepartmentName, [DeliveryFax]=@DeliveryFax, [DeliveryTelephone]=@DeliveryTelephone, [DepartmentId]=@DepartmentId, [DepartmentName]=@DepartmentName, [Description]=@Description, [Discount]=@Discount, [InternalContact]=@InternalContact, [IssuerId]=@IssuerId, [IssuerName]=@IssuerName, [LastRejectionLevel]=@LastRejectionLevel, [LastRejectionTime]=@LastRejectionTime, [LastRejectionUserId]=@LastRejectionUserId, [LeasingMonthAmount]=@LeasingMonthAmount, [LeasingNbMonths]=@LeasingNbMonths, [LeasingStartMonth]=@LeasingStartMonth, [LeasingStartYear]=@LeasingStartYear, [LeasingTotalAmount]=@LeasingTotalAmount, [Level]=@Level, [LocationId]=@LocationId, [MandantId]=@MandantId, [OrderId]=@OrderId, [OrderNumber]=@OrderNumber, [OrderPlacedEmailMessage]=@OrderPlacedEmailMessage, [OrderPlacedEmailTitle]=@OrderPlacedEmailTitle, [OrderPlacedReportFileId]=@OrderPlacedReportFileId, [OrderPlacedSendingEmail]=@OrderPlacedSendingEmail, [OrderPlacedSupplierEmail]=@OrderPlacedSupplierEmail, [OrderPlacedTime]=@OrderPlacedTime, [OrderPlacedUserEmail]=@OrderPlacedUserEmail, [OrderPlacedUserId]=@OrderPlacedUserId, [OrderPlacedUserName]=@OrderPlacedUserName, [OrderPlacementCCEmail]=@OrderPlacementCCEmail, [OrderType]=@OrderType, [PoPaymentType]=@PoPaymentType, [PoPaymentTypeName]=@PoPaymentTypeName, [ProjectId]=@ProjectId, [ProjectName]=@ProjectName, [Status]=@Status, [StorageLocationId]=@StorageLocationId, [StorageLocationName]=@StorageLocationName, [SupplierEmail]=@SupplierEmail, [SupplierFax]=@SupplierFax, [SupplierId]=@SupplierId, [SupplierName]=@SupplierName, [SupplierNumber]=@SupplierNumber, [SupplierNummer]=@SupplierNummer, [SupplierPaymentMethod]=@SupplierPaymentMethod, [SupplierPaymentTerm]=@SupplierPaymentTerm, [SupplierTelephone]=@SupplierTelephone, [SupplierTradingTerm]=@SupplierTradingTerm, [ValidationRequestTime]=@ValidationRequestTime WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AllocationType", item.AllocationType);
				sqlCommand.Parameters.AddWithValue("AllocationTypeName", item.AllocationTypeName == null ? (object)DBNull.Value : item.AllocationTypeName);
				sqlCommand.Parameters.AddWithValue("ApprovalTime", item.ApprovalTime == null ? (object)DBNull.Value : item.ApprovalTime);
				sqlCommand.Parameters.AddWithValue("ApprovalUserId", item.ApprovalUserId == null ? (object)DBNull.Value : item.ApprovalUserId);
				sqlCommand.Parameters.AddWithValue("Archived", item.Archived == null ? (object)DBNull.Value : item.Archived);
				sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
				sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
				sqlCommand.Parameters.AddWithValue("BillingAddress", item.BillingAddress == null ? (object)DBNull.Value : item.BillingAddress);
				sqlCommand.Parameters.AddWithValue("BillingCompanyId", item.BillingCompanyId == null ? (object)DBNull.Value : item.BillingCompanyId);
				sqlCommand.Parameters.AddWithValue("BillingCompanyName", item.BillingCompanyName == null ? (object)DBNull.Value : item.BillingCompanyName);
				sqlCommand.Parameters.AddWithValue("BillingContactName", item.BillingContactName == null ? (object)DBNull.Value : item.BillingContactName);
				sqlCommand.Parameters.AddWithValue("BillingDepartmentId", item.BillingDepartmentId == null ? (object)DBNull.Value : item.BillingDepartmentId);
				sqlCommand.Parameters.AddWithValue("BillingDepartmentName", item.BillingDepartmentName == null ? (object)DBNull.Value : item.BillingDepartmentName);
				sqlCommand.Parameters.AddWithValue("BillingFax", item.BillingFax == null ? (object)DBNull.Value : item.BillingFax);
				sqlCommand.Parameters.AddWithValue("BillingTelephone", item.BillingTelephone == null ? (object)DBNull.Value : item.BillingTelephone);
				sqlCommand.Parameters.AddWithValue("BudgetYear", item.BudgetYear);
				sqlCommand.Parameters.AddWithValue("CompanyId", item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
				sqlCommand.Parameters.AddWithValue("CompanyName", item.CompanyName == null ? (object)DBNull.Value : item.CompanyName);
				sqlCommand.Parameters.AddWithValue("CreationDate", item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
				sqlCommand.Parameters.AddWithValue("CurrencyId", item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
				sqlCommand.Parameters.AddWithValue("CurrencyName", item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
				sqlCommand.Parameters.AddWithValue("DefaultCurrencyDecimals", item.DefaultCurrencyDecimals == null ? (object)DBNull.Value : item.DefaultCurrencyDecimals);
				sqlCommand.Parameters.AddWithValue("DefaultCurrencyId", item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
				sqlCommand.Parameters.AddWithValue("DefaultCurrencyName", item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
				sqlCommand.Parameters.AddWithValue("DefaultCurrencyRate", item.DefaultCurrencyRate == null ? (object)DBNull.Value : item.DefaultCurrencyRate);
				sqlCommand.Parameters.AddWithValue("Deleted", item.Deleted == null ? (object)DBNull.Value : item.Deleted);
				sqlCommand.Parameters.AddWithValue("DeleteTime", item.DeleteTime == null ? (object)DBNull.Value : item.DeleteTime);
				sqlCommand.Parameters.AddWithValue("DeleteUserId", item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
				sqlCommand.Parameters.AddWithValue("DeliveryAddress", item.DeliveryAddress == null ? (object)DBNull.Value : item.DeliveryAddress);
				sqlCommand.Parameters.AddWithValue("DeliveryCompanyId", item.DeliveryCompanyId == null ? (object)DBNull.Value : item.DeliveryCompanyId);
				sqlCommand.Parameters.AddWithValue("DeliveryCompanyName", item.DeliveryCompanyName == null ? (object)DBNull.Value : item.DeliveryCompanyName);
				sqlCommand.Parameters.AddWithValue("DeliveryDepartmentId", item.DeliveryDepartmentId == null ? (object)DBNull.Value : item.DeliveryDepartmentId);
				sqlCommand.Parameters.AddWithValue("DeliveryDepartmentName", item.DeliveryDepartmentName == null ? (object)DBNull.Value : item.DeliveryDepartmentName);
				sqlCommand.Parameters.AddWithValue("DeliveryFax", item.DeliveryFax == null ? (object)DBNull.Value : item.DeliveryFax);
				sqlCommand.Parameters.AddWithValue("DeliveryTelephone", item.DeliveryTelephone == null ? (object)DBNull.Value : item.DeliveryTelephone);
				sqlCommand.Parameters.AddWithValue("DepartmentId", item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
				sqlCommand.Parameters.AddWithValue("DepartmentName", item.DepartmentName == null ? (object)DBNull.Value : item.DepartmentName);
				sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
				sqlCommand.Parameters.AddWithValue("Discount", item.Discount == null ? (object)DBNull.Value : item.Discount);
				sqlCommand.Parameters.AddWithValue("InternalContact", item.InternalContact == null ? (object)DBNull.Value : item.InternalContact);
				sqlCommand.Parameters.AddWithValue("IssuerId", item.IssuerId);
				sqlCommand.Parameters.AddWithValue("IssuerName", item.IssuerName == null ? (object)DBNull.Value : item.IssuerName);
				sqlCommand.Parameters.AddWithValue("LastRejectionLevel", item.LastRejectionLevel == null ? (object)DBNull.Value : item.LastRejectionLevel);
				sqlCommand.Parameters.AddWithValue("LastRejectionTime", item.LastRejectionTime == null ? (object)DBNull.Value : item.LastRejectionTime);
				sqlCommand.Parameters.AddWithValue("LastRejectionUserId", item.LastRejectionUserId == null ? (object)DBNull.Value : item.LastRejectionUserId);
				sqlCommand.Parameters.AddWithValue("LeasingMonthAmount", item.LeasingMonthAmount == null ? (object)DBNull.Value : item.LeasingMonthAmount);
				sqlCommand.Parameters.AddWithValue("LeasingNbMonths", item.LeasingNbMonths == null ? (object)DBNull.Value : item.LeasingNbMonths);
				sqlCommand.Parameters.AddWithValue("LeasingStartMonth", item.LeasingStartMonth == null ? (object)DBNull.Value : item.LeasingStartMonth);
				sqlCommand.Parameters.AddWithValue("LeasingStartYear", item.LeasingStartYear == null ? (object)DBNull.Value : item.LeasingStartYear);
				sqlCommand.Parameters.AddWithValue("LeasingTotalAmount", item.LeasingTotalAmount == null ? (object)DBNull.Value : item.LeasingTotalAmount);
				sqlCommand.Parameters.AddWithValue("Level", item.Level == null ? (object)DBNull.Value : item.Level);
				sqlCommand.Parameters.AddWithValue("LocationId", item.LocationId == null ? (object)DBNull.Value : item.LocationId);
				sqlCommand.Parameters.AddWithValue("MandantId", item.MandantId == null ? (object)DBNull.Value : item.MandantId);
				sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
				sqlCommand.Parameters.AddWithValue("OrderNumber", item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
				sqlCommand.Parameters.AddWithValue("OrderPlacedEmailMessage", item.OrderPlacedEmailMessage == null ? (object)DBNull.Value : item.OrderPlacedEmailMessage);
				sqlCommand.Parameters.AddWithValue("OrderPlacedEmailTitle", item.OrderPlacedEmailTitle == null ? (object)DBNull.Value : item.OrderPlacedEmailTitle);
				sqlCommand.Parameters.AddWithValue("OrderPlacedReportFileId", item.OrderPlacedReportFileId == null ? (object)DBNull.Value : item.OrderPlacedReportFileId);
				sqlCommand.Parameters.AddWithValue("OrderPlacedSendingEmail", item.OrderPlacedSendingEmail == null ? (object)DBNull.Value : item.OrderPlacedSendingEmail);
				sqlCommand.Parameters.AddWithValue("OrderPlacedSupplierEmail", item.OrderPlacedSupplierEmail == null ? (object)DBNull.Value : item.OrderPlacedSupplierEmail);
				sqlCommand.Parameters.AddWithValue("OrderPlacedTime", item.OrderPlacedTime == null ? (object)DBNull.Value : item.OrderPlacedTime);
				sqlCommand.Parameters.AddWithValue("OrderPlacedUserEmail", item.OrderPlacedUserEmail == null ? (object)DBNull.Value : item.OrderPlacedUserEmail);
				sqlCommand.Parameters.AddWithValue("OrderPlacedUserId", item.OrderPlacedUserId == null ? (object)DBNull.Value : item.OrderPlacedUserId);
				sqlCommand.Parameters.AddWithValue("OrderPlacedUserName", item.OrderPlacedUserName == null ? (object)DBNull.Value : item.OrderPlacedUserName);
				sqlCommand.Parameters.AddWithValue("OrderPlacementCCEmail", item.OrderPlacementCCEmail == null ? (object)DBNull.Value : item.OrderPlacementCCEmail);
				sqlCommand.Parameters.AddWithValue("OrderType", item.OrderType == null ? (object)DBNull.Value : item.OrderType);
				sqlCommand.Parameters.AddWithValue("PoPaymentType", item.PoPaymentType == null ? (object)DBNull.Value : item.PoPaymentType);
				sqlCommand.Parameters.AddWithValue("PoPaymentTypeName", item.PoPaymentTypeName == null ? (object)DBNull.Value : item.PoPaymentTypeName);
				sqlCommand.Parameters.AddWithValue("ProjectId", item.ProjectId == null ? (object)DBNull.Value : item.ProjectId);
				sqlCommand.Parameters.AddWithValue("ProjectName", item.ProjectName == null ? (object)DBNull.Value : item.ProjectName);
				sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
				sqlCommand.Parameters.AddWithValue("StorageLocationId", item.StorageLocationId == null ? (object)DBNull.Value : item.StorageLocationId);
				sqlCommand.Parameters.AddWithValue("StorageLocationName", item.StorageLocationName == null ? (object)DBNull.Value : item.StorageLocationName);
				sqlCommand.Parameters.AddWithValue("SupplierEmail", item.SupplierEmail == null ? (object)DBNull.Value : item.SupplierEmail);
				sqlCommand.Parameters.AddWithValue("SupplierFax", item.SupplierFax == null ? (object)DBNull.Value : item.SupplierFax);
				sqlCommand.Parameters.AddWithValue("SupplierId", item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
				sqlCommand.Parameters.AddWithValue("SupplierName", item.SupplierName == null ? (object)DBNull.Value : item.SupplierName);
				sqlCommand.Parameters.AddWithValue("SupplierNumber", item.SupplierNumber == null ? (object)DBNull.Value : item.SupplierNumber);
				sqlCommand.Parameters.AddWithValue("SupplierNummer", item.SupplierNummer == null ? (object)DBNull.Value : item.SupplierNummer);
				sqlCommand.Parameters.AddWithValue("SupplierPaymentMethod", item.SupplierPaymentMethod == null ? (object)DBNull.Value : item.SupplierPaymentMethod);
				sqlCommand.Parameters.AddWithValue("SupplierPaymentTerm", item.SupplierPaymentTerm == null ? (object)DBNull.Value : item.SupplierPaymentTerm);
				sqlCommand.Parameters.AddWithValue("SupplierTelephone", item.SupplierTelephone == null ? (object)DBNull.Value : item.SupplierTelephone);
				sqlCommand.Parameters.AddWithValue("SupplierTradingTerm", item.SupplierTradingTerm == null ? (object)DBNull.Value : item.SupplierTradingTerm);
				sqlCommand.Parameters.AddWithValue("ValidationRequestTime", item.ValidationRequestTime == null ? (object)DBNull.Value : item.ValidationRequestTime);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 85; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> items)
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
						query += " UPDATE [__FNC_BestellungenExtension] SET "

							+ "[AllocationType]=@AllocationType" + i + ","
							+ "[AllocationTypeName]=@AllocationTypeName" + i + ","
							+ "[ApprovalTime]=@ApprovalTime" + i + ","
							+ "[ApprovalUserId]=@ApprovalUserId" + i + ","
							+ "[Archived]=@Archived" + i + ","
							+ "[ArchiveTime]=@ArchiveTime" + i + ","
							+ "[ArchiveUserId]=@ArchiveUserId" + i + ","
							+ "[BillingAddress]=@BillingAddress" + i + ","
							+ "[BillingCompanyId]=@BillingCompanyId" + i + ","
							+ "[BillingCompanyName]=@BillingCompanyName" + i + ","
							+ "[BillingContactName]=@BillingContactName" + i + ","
							+ "[BillingDepartmentId]=@BillingDepartmentId" + i + ","
							+ "[BillingDepartmentName]=@BillingDepartmentName" + i + ","
							+ "[BillingFax]=@BillingFax" + i + ","
							+ "[BillingTelephone]=@BillingTelephone" + i + ","
							+ "[BudgetYear]=@BudgetYear" + i + ","
							+ "[CompanyId]=@CompanyId" + i + ","
							+ "[CompanyName]=@CompanyName" + i + ","
							+ "[CreationDate]=@CreationDate" + i + ","
							+ "[CurrencyId]=@CurrencyId" + i + ","
							+ "[CurrencyName]=@CurrencyName" + i + ","
							+ "[DefaultCurrencyDecimals]=@DefaultCurrencyDecimals" + i + ","
							+ "[DefaultCurrencyId]=@DefaultCurrencyId" + i + ","
							+ "[DefaultCurrencyName]=@DefaultCurrencyName" + i + ","
							+ "[DefaultCurrencyRate]=@DefaultCurrencyRate" + i + ","
							+ "[Deleted]=@Deleted" + i + ","
							+ "[DeleteTime]=@DeleteTime" + i + ","
							+ "[DeleteUserId]=@DeleteUserId" + i + ","
							+ "[DeliveryAddress]=@DeliveryAddress" + i + ","
							+ "[DeliveryCompanyId]=@DeliveryCompanyId" + i + ","
							+ "[DeliveryCompanyName]=@DeliveryCompanyName" + i + ","
							+ "[DeliveryDepartmentId]=@DeliveryDepartmentId" + i + ","
							+ "[DeliveryDepartmentName]=@DeliveryDepartmentName" + i + ","
							+ "[DeliveryFax]=@DeliveryFax" + i + ","
							+ "[DeliveryTelephone]=@DeliveryTelephone" + i + ","
							+ "[DepartmentId]=@DepartmentId" + i + ","
							+ "[DepartmentName]=@DepartmentName" + i + ","
							+ "[Description]=@Description" + i + ","
							+ "[Discount]=@Discount" + i + ","
							+ "[InternalContact]=@InternalContact" + i + ","
							+ "[IssuerId]=@IssuerId" + i + ","
							+ "[IssuerName]=@IssuerName" + i + ","
							+ "[LastRejectionLevel]=@LastRejectionLevel" + i + ","
							+ "[LastRejectionTime]=@LastRejectionTime" + i + ","
							+ "[LastRejectionUserId]=@LastRejectionUserId" + i + ","
							+ "[LeasingMonthAmount]=@LeasingMonthAmount" + i + ","
							+ "[LeasingNbMonths]=@LeasingNbMonths" + i + ","
							+ "[LeasingStartMonth]=@LeasingStartMonth" + i + ","
							+ "[LeasingStartYear]=@LeasingStartYear" + i + ","
							+ "[LeasingTotalAmount]=@LeasingTotalAmount" + i + ","
							+ "[Level]=@Level" + i + ","
							+ "[LocationId]=@LocationId" + i + ","
							+ "[MandantId]=@MandantId" + i + ","
							+ "[OrderId]=@OrderId" + i + ","
							+ "[OrderNumber]=@OrderNumber" + i + ","
							+ "[OrderPlacedEmailMessage]=@OrderPlacedEmailMessage" + i + ","
							+ "[OrderPlacedEmailTitle]=@OrderPlacedEmailTitle" + i + ","
							+ "[OrderPlacedReportFileId]=@OrderPlacedReportFileId" + i + ","
							+ "[OrderPlacedSendingEmail]=@OrderPlacedSendingEmail" + i + ","
							+ "[OrderPlacedSupplierEmail]=@OrderPlacedSupplierEmail" + i + ","
							+ "[OrderPlacedTime]=@OrderPlacedTime" + i + ","
							+ "[OrderPlacedUserEmail]=@OrderPlacedUserEmail" + i + ","
							+ "[OrderPlacedUserId]=@OrderPlacedUserId" + i + ","
							+ "[OrderPlacedUserName]=@OrderPlacedUserName" + i + ","
							+ "[OrderPlacementCCEmail]=@OrderPlacementCCEmail" + i + ","
							+ "[OrderType]=@OrderType" + i + ","
							+ "[PoPaymentType]=@PoPaymentType" + i + ","
							+ "[PoPaymentTypeName]=@PoPaymentTypeName" + i + ","
							+ "[ProjectId]=@ProjectId" + i + ","
							+ "[ProjectName]=@ProjectName" + i + ","
							+ "[Status]=@Status" + i + ","
							+ "[StorageLocationId]=@StorageLocationId" + i + ","
							+ "[StorageLocationName]=@StorageLocationName" + i + ","
							+ "[SupplierEmail]=@SupplierEmail" + i + ","
							+ "[SupplierFax]=@SupplierFax" + i + ","
							+ "[SupplierId]=@SupplierId" + i + ","
							+ "[SupplierName]=@SupplierName" + i + ","
							+ "[SupplierNumber]=@SupplierNumber" + i + ","
							+ "[SupplierNummer]=@SupplierNummer" + i + ","
							+ "[SupplierPaymentMethod]=@SupplierPaymentMethod" + i + ","
							+ "[SupplierPaymentTerm]=@SupplierPaymentTerm" + i + ","
							+ "[SupplierTelephone]=@SupplierTelephone" + i + ","
							+ "[SupplierTradingTerm]=@SupplierTradingTerm" + i + ","
							+ "[ValidationRequestTime]=@ValidationRequestTime" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("AllocationType" + i, item.AllocationType);
						sqlCommand.Parameters.AddWithValue("AllocationTypeName" + i, item.AllocationTypeName == null ? (object)DBNull.Value : item.AllocationTypeName);
						sqlCommand.Parameters.AddWithValue("ApprovalTime" + i, item.ApprovalTime == null ? (object)DBNull.Value : item.ApprovalTime);
						sqlCommand.Parameters.AddWithValue("ApprovalUserId" + i, item.ApprovalUserId == null ? (object)DBNull.Value : item.ApprovalUserId);
						sqlCommand.Parameters.AddWithValue("Archived" + i, item.Archived == null ? (object)DBNull.Value : item.Archived);
						sqlCommand.Parameters.AddWithValue("ArchiveTime" + i, item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
						sqlCommand.Parameters.AddWithValue("ArchiveUserId" + i, item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
						sqlCommand.Parameters.AddWithValue("BillingAddress" + i, item.BillingAddress == null ? (object)DBNull.Value : item.BillingAddress);
						sqlCommand.Parameters.AddWithValue("BillingCompanyId" + i, item.BillingCompanyId == null ? (object)DBNull.Value : item.BillingCompanyId);
						sqlCommand.Parameters.AddWithValue("BillingCompanyName" + i, item.BillingCompanyName == null ? (object)DBNull.Value : item.BillingCompanyName);
						sqlCommand.Parameters.AddWithValue("BillingContactName" + i, item.BillingContactName == null ? (object)DBNull.Value : item.BillingContactName);
						sqlCommand.Parameters.AddWithValue("BillingDepartmentId" + i, item.BillingDepartmentId == null ? (object)DBNull.Value : item.BillingDepartmentId);
						sqlCommand.Parameters.AddWithValue("BillingDepartmentName" + i, item.BillingDepartmentName == null ? (object)DBNull.Value : item.BillingDepartmentName);
						sqlCommand.Parameters.AddWithValue("BillingFax" + i, item.BillingFax == null ? (object)DBNull.Value : item.BillingFax);
						sqlCommand.Parameters.AddWithValue("BillingTelephone" + i, item.BillingTelephone == null ? (object)DBNull.Value : item.BillingTelephone);
						sqlCommand.Parameters.AddWithValue("BudgetYear" + i, item.BudgetYear);
						sqlCommand.Parameters.AddWithValue("CompanyId" + i, item.CompanyId == null ? (object)DBNull.Value : item.CompanyId);
						sqlCommand.Parameters.AddWithValue("CompanyName" + i, item.CompanyName == null ? (object)DBNull.Value : item.CompanyName);
						sqlCommand.Parameters.AddWithValue("CreationDate" + i, item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
						sqlCommand.Parameters.AddWithValue("CurrencyId" + i, item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
						sqlCommand.Parameters.AddWithValue("CurrencyName" + i, item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyDecimals" + i, item.DefaultCurrencyDecimals == null ? (object)DBNull.Value : item.DefaultCurrencyDecimals);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyId" + i, item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyName" + i, item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyRate" + i, item.DefaultCurrencyRate == null ? (object)DBNull.Value : item.DefaultCurrencyRate);
						sqlCommand.Parameters.AddWithValue("Deleted" + i, item.Deleted == null ? (object)DBNull.Value : item.Deleted);
						sqlCommand.Parameters.AddWithValue("DeleteTime" + i, item.DeleteTime == null ? (object)DBNull.Value : item.DeleteTime);
						sqlCommand.Parameters.AddWithValue("DeleteUserId" + i, item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
						sqlCommand.Parameters.AddWithValue("DeliveryAddress" + i, item.DeliveryAddress == null ? (object)DBNull.Value : item.DeliveryAddress);
						sqlCommand.Parameters.AddWithValue("DeliveryCompanyId" + i, item.DeliveryCompanyId == null ? (object)DBNull.Value : item.DeliveryCompanyId);
						sqlCommand.Parameters.AddWithValue("DeliveryCompanyName" + i, item.DeliveryCompanyName == null ? (object)DBNull.Value : item.DeliveryCompanyName);
						sqlCommand.Parameters.AddWithValue("DeliveryDepartmentId" + i, item.DeliveryDepartmentId == null ? (object)DBNull.Value : item.DeliveryDepartmentId);
						sqlCommand.Parameters.AddWithValue("DeliveryDepartmentName" + i, item.DeliveryDepartmentName == null ? (object)DBNull.Value : item.DeliveryDepartmentName);
						sqlCommand.Parameters.AddWithValue("DeliveryFax" + i, item.DeliveryFax == null ? (object)DBNull.Value : item.DeliveryFax);
						sqlCommand.Parameters.AddWithValue("DeliveryTelephone" + i, item.DeliveryTelephone == null ? (object)DBNull.Value : item.DeliveryTelephone);
						sqlCommand.Parameters.AddWithValue("DepartmentId" + i, item.DepartmentId == null ? (object)DBNull.Value : item.DepartmentId);
						sqlCommand.Parameters.AddWithValue("DepartmentName" + i, item.DepartmentName == null ? (object)DBNull.Value : item.DepartmentName);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("Discount" + i, item.Discount == null ? (object)DBNull.Value : item.Discount);
						sqlCommand.Parameters.AddWithValue("InternalContact" + i, item.InternalContact == null ? (object)DBNull.Value : item.InternalContact);
						sqlCommand.Parameters.AddWithValue("IssuerId" + i, item.IssuerId);
						sqlCommand.Parameters.AddWithValue("IssuerName" + i, item.IssuerName == null ? (object)DBNull.Value : item.IssuerName);
						sqlCommand.Parameters.AddWithValue("LastRejectionLevel" + i, item.LastRejectionLevel == null ? (object)DBNull.Value : item.LastRejectionLevel);
						sqlCommand.Parameters.AddWithValue("LastRejectionTime" + i, item.LastRejectionTime == null ? (object)DBNull.Value : item.LastRejectionTime);
						sqlCommand.Parameters.AddWithValue("LastRejectionUserId" + i, item.LastRejectionUserId == null ? (object)DBNull.Value : item.LastRejectionUserId);
						sqlCommand.Parameters.AddWithValue("LeasingMonthAmount" + i, item.LeasingMonthAmount == null ? (object)DBNull.Value : item.LeasingMonthAmount);
						sqlCommand.Parameters.AddWithValue("LeasingNbMonths" + i, item.LeasingNbMonths == null ? (object)DBNull.Value : item.LeasingNbMonths);
						sqlCommand.Parameters.AddWithValue("LeasingStartMonth" + i, item.LeasingStartMonth == null ? (object)DBNull.Value : item.LeasingStartMonth);
						sqlCommand.Parameters.AddWithValue("LeasingStartYear" + i, item.LeasingStartYear == null ? (object)DBNull.Value : item.LeasingStartYear);
						sqlCommand.Parameters.AddWithValue("LeasingTotalAmount" + i, item.LeasingTotalAmount == null ? (object)DBNull.Value : item.LeasingTotalAmount);
						sqlCommand.Parameters.AddWithValue("Level" + i, item.Level == null ? (object)DBNull.Value : item.Level);
						sqlCommand.Parameters.AddWithValue("LocationId" + i, item.LocationId == null ? (object)DBNull.Value : item.LocationId);
						sqlCommand.Parameters.AddWithValue("MandantId" + i, item.MandantId == null ? (object)DBNull.Value : item.MandantId);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
						sqlCommand.Parameters.AddWithValue("OrderNumber" + i, item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
						sqlCommand.Parameters.AddWithValue("OrderPlacedEmailMessage" + i, item.OrderPlacedEmailMessage == null ? (object)DBNull.Value : item.OrderPlacedEmailMessage);
						sqlCommand.Parameters.AddWithValue("OrderPlacedEmailTitle" + i, item.OrderPlacedEmailTitle == null ? (object)DBNull.Value : item.OrderPlacedEmailTitle);
						sqlCommand.Parameters.AddWithValue("OrderPlacedReportFileId" + i, item.OrderPlacedReportFileId == null ? (object)DBNull.Value : item.OrderPlacedReportFileId);
						sqlCommand.Parameters.AddWithValue("OrderPlacedSendingEmail" + i, item.OrderPlacedSendingEmail == null ? (object)DBNull.Value : item.OrderPlacedSendingEmail);
						sqlCommand.Parameters.AddWithValue("OrderPlacedSupplierEmail" + i, item.OrderPlacedSupplierEmail == null ? (object)DBNull.Value : item.OrderPlacedSupplierEmail);
						sqlCommand.Parameters.AddWithValue("OrderPlacedTime" + i, item.OrderPlacedTime == null ? (object)DBNull.Value : item.OrderPlacedTime);
						sqlCommand.Parameters.AddWithValue("OrderPlacedUserEmail" + i, item.OrderPlacedUserEmail == null ? (object)DBNull.Value : item.OrderPlacedUserEmail);
						sqlCommand.Parameters.AddWithValue("OrderPlacedUserId" + i, item.OrderPlacedUserId == null ? (object)DBNull.Value : item.OrderPlacedUserId);
						sqlCommand.Parameters.AddWithValue("OrderPlacedUserName" + i, item.OrderPlacedUserName == null ? (object)DBNull.Value : item.OrderPlacedUserName);
						sqlCommand.Parameters.AddWithValue("OrderPlacementCCEmail" + i, item.OrderPlacementCCEmail == null ? (object)DBNull.Value : item.OrderPlacementCCEmail);
						sqlCommand.Parameters.AddWithValue("OrderType" + i, item.OrderType == null ? (object)DBNull.Value : item.OrderType);
						sqlCommand.Parameters.AddWithValue("PoPaymentType" + i, item.PoPaymentType == null ? (object)DBNull.Value : item.PoPaymentType);
						sqlCommand.Parameters.AddWithValue("PoPaymentTypeName" + i, item.PoPaymentTypeName == null ? (object)DBNull.Value : item.PoPaymentTypeName);
						sqlCommand.Parameters.AddWithValue("ProjectId" + i, item.ProjectId == null ? (object)DBNull.Value : item.ProjectId);
						sqlCommand.Parameters.AddWithValue("ProjectName" + i, item.ProjectName == null ? (object)DBNull.Value : item.ProjectName);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
						sqlCommand.Parameters.AddWithValue("StorageLocationId" + i, item.StorageLocationId == null ? (object)DBNull.Value : item.StorageLocationId);
						sqlCommand.Parameters.AddWithValue("StorageLocationName" + i, item.StorageLocationName == null ? (object)DBNull.Value : item.StorageLocationName);
						sqlCommand.Parameters.AddWithValue("SupplierEmail" + i, item.SupplierEmail == null ? (object)DBNull.Value : item.SupplierEmail);
						sqlCommand.Parameters.AddWithValue("SupplierFax" + i, item.SupplierFax == null ? (object)DBNull.Value : item.SupplierFax);
						sqlCommand.Parameters.AddWithValue("SupplierId" + i, item.SupplierId == null ? (object)DBNull.Value : item.SupplierId);
						sqlCommand.Parameters.AddWithValue("SupplierName" + i, item.SupplierName == null ? (object)DBNull.Value : item.SupplierName);
						sqlCommand.Parameters.AddWithValue("SupplierNumber" + i, item.SupplierNumber == null ? (object)DBNull.Value : item.SupplierNumber);
						sqlCommand.Parameters.AddWithValue("SupplierNummer" + i, item.SupplierNummer == null ? (object)DBNull.Value : item.SupplierNummer);
						sqlCommand.Parameters.AddWithValue("SupplierPaymentMethod" + i, item.SupplierPaymentMethod == null ? (object)DBNull.Value : item.SupplierPaymentMethod);
						sqlCommand.Parameters.AddWithValue("SupplierPaymentTerm" + i, item.SupplierPaymentTerm == null ? (object)DBNull.Value : item.SupplierPaymentTerm);
						sqlCommand.Parameters.AddWithValue("SupplierTelephone" + i, item.SupplierTelephone == null ? (object)DBNull.Value : item.SupplierTelephone);
						sqlCommand.Parameters.AddWithValue("SupplierTradingTerm" + i, item.SupplierTradingTerm == null ? (object)DBNull.Value : item.SupplierTradingTerm);
						sqlCommand.Parameters.AddWithValue("ValidationRequestTime" + i, item.ValidationRequestTime == null ? (object)DBNull.Value : item.ValidationRequestTime);
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
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__FNC_BestellungenExtension] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__FNC_BestellungenExtension] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion


		#region Custom Methods
		public static Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity GetByOrderId(int id, bool? isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BestellungenExtension] WHERE [OrderId]=@Id {(isArchived.HasValue ? $" AND [Archived]={(isArchived.Value ? "1" : "0")}" : "")} AND [Deleted]=@deleted";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity GetByOrderId(int id, SqlConnection connection, SqlTransaction transaction, bool? isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			string query = "";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.CommandText = $"SELECT * FROM [__FNC_BestellungenExtension] WHERE [OrderId]=@Id {(isArchived.HasValue ? $" AND [Archived]={(isArchived.Value ? "1" : "0")}" : "")} AND [Deleted]=@deleted";

				sqlCommand.Parameters.AddWithValue("Id", id);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetByOrderIds(List<int> ids, bool? isArchived = false, bool? isDeleted = false)
		{
			if(ids == null || ids.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BestellungenExtension] WHERE [OrderId] IN ({string.Join(",", ids)}){(isArchived.HasValue ? $" AND [Archived]=@archived" : "")}{(isDeleted.HasValue ? $" AND [Deleted]=@deleted" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				if(isArchived.HasValue)
				{
					sqlCommand.Parameters.AddWithValue("archived", isArchived);
				}
				if(isDeleted.HasValue)
				{
					sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				}

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}

		// - FIXME: foreign table JOIN inside
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetByUser(int userId, bool? booked, int? year = null, bool isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BestellungenExtension] AS E {(!booked.HasValue ? " WHERE " : "LEFT JOIN [Bestellungen] AS B ON E.OrderId=B.Nr WHERE B.Erledigt=@booked AND ")} [IssuerId]=@Id AND [Archived]=@archived AND [Deleted]=@deleted {(year.HasValue ? $" AND YEAR(CreationDate)={year.Value}" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", userId);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				if(booked.HasValue)
					sqlCommand.Parameters.AddWithValue("booked", booked);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetByUserAndYear(int userId, int year, BestellungenExtensionEnums.OrderPaymentTypes paymentType, bool? isArchived = null, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = paymentType == BestellungenExtensionEnums.OrderPaymentTypes.Purchase
					? $"SELECT * FROM [__FNC_BestellungenExtension] WHERE [IssuerId]=@Id AND [PoPaymentType]=@paymentType {(isArchived.HasValue ? $" AND [Archived]={(isArchived.Value == true ? "1" : "0")}" : "")} AND [Deleted]=@deleted AND year(CreationDate)=@year "
					: $"SELECT * FROM [__FNC_BestellungenExtension] WHERE [IssuerId]=@Id AND [PoPaymentType]=@paymentType {(isArchived.HasValue ? $" AND [Archived]={(isArchived.Value == true ? "1" : "0")}" : "")} AND [Deleted]=@deleted "
						+ "AND ([LeasingStartYear]=@year OR ([LeasingStartYear]<@year AND (CAST(FLOOR(CAST(([LeasingStartMonth] - 1 + [LeasingNbMonths]) AS FLOAT) / 12) AS INTEGER) + [LeasingStartYear])>=@year ))";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", userId);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("paymentType", (int)paymentType);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetByUserAndYearAndMonth(int id_user, int year, int month, BestellungenExtensionEnums.OrderPaymentTypes paymentType, bool? isArchived = null, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = paymentType == BestellungenExtensionEnums.OrderPaymentTypes.Purchase
					? $"SELECT * FROM [__FNC_BestellungenExtension] WHERE [IssuerId]=@Id AND [PoPaymentType]=@paymentType {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} AND [Deleted]=@deleted AND year(CreationDate)=@year AND month(CreationDate)=@month"
					: $"SELECT * FROM [__FNC_BestellungenExtension] WHERE [IssuerId]=@Id AND [PoPaymentType]=@paymentType {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} AND [Deleted]=@deleted "
						+ "AND ("
							+ "([LeasingStartYear] = @year AND [LeasingStartMonth] <= @month) "
							+ "OR ([LeasingStartYear] < @year AND (CAST(FLOOR(CAST(([LeasingStartMonth] - 1 + [LeasingNbMonths]) AS FLOAT) / 12) AS INTEGER) + [LeasingStartYear]) < @year) " // - > last year
							+ "OR ([LeasingStartYear] < @year AND (CAST(FLOOR(CAST(([LeasingStartMonth] - 1 + [LeasingNbMonths]) AS FLOAT) / 12) AS INTEGER) + [LeasingStartYear]) = @year AND (([LeasingStartMonth] - 1 + [LeasingNbMonths]) % 12) >= @month)" // - = last & <= last month
						+ " ) ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id_user);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("month", month);
				sqlCommand.Parameters.AddWithValue("paymentType", (int)paymentType);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetLeasingByYearAndMonth(int year, int month, bool? isArchived = null, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BestellungenExtension] WHERE [PoPaymentType]=@paymentType {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} AND [Deleted]=@deleted "
						+ "AND DATEADD(MONTH, [LeasingNbMonths], DATEFROMPARTS([LeasingStartYear], [LeasingStartMonth], 1)) > DATEFROMPARTS(@year, @month, 1)";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("month", month);
				sqlCommand.Parameters.AddWithValue("paymentType", (int)BestellungenExtensionEnums.OrderPaymentTypes.Leasing);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetLeasingByYear(int year, bool? isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BestellungenExtension] WHERE [PoPaymentType]=@paymentType {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} AND [Deleted]=@deleted "
						+ "AND ("
							+ "([LeasingStartYear] <= @year ) " // - frist year
							+ "OR (CAST(FLOOR(CAST(([LeasingStartMonth] - 1 + [LeasingNbMonths]) AS FLOAT) / 12) AS INTEGER) + [LeasingStartYear]) >= @year "  // - last year
						+ " ) ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("paymentType", (int)BestellungenExtensionEnums.OrderPaymentTypes.Leasing);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetLeasingByYearAndUser(int year, int userId, bool? isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BestellungenExtension] WHERE [IssuerId]=@Id AND [PoPaymentType]=@paymentType {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} AND [Deleted]=@deleted "
						+ "AND ([LeasingStartYear]=@year OR ([LeasingStartYear]<@year AND (CAST(FLOOR(CAST(([LeasingStartMonth] - 1 + [LeasingNbMonths]) AS FLOAT) / 12) AS INTEGER) + [LeasingStartYear])>=@year ))";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", userId);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("paymentType", (int)BestellungenExtensionEnums.OrderPaymentTypes.Leasing);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetLeasingByYearAndCompanies(int year, List<int> comapnyIds, bool? isArchived = false, bool isDeleted = false)
		{
			if(comapnyIds == null || comapnyIds.Count <= 0)
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BestellungenExtension] WHERE [CompanyId] IS NOT NULL AND [CompanyId] IN ({string.Join(",", comapnyIds)}) AND [PoPaymentType]=@paymentType {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} AND [Deleted]=@deleted "
						+ "AND ([LeasingStartYear]=@year OR ([LeasingStartYear]<@year AND (CAST(FLOOR(CAST(([LeasingStartMonth] - 1 + [LeasingNbMonths]) AS FLOAT) / 12) AS INTEGER) + [LeasingStartYear])>=@year ))";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("paymentType", (int)BestellungenExtensionEnums.OrderPaymentTypes.Leasing);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetLeasingByYearAndDepartments(int year, List<int> departmentIds, bool? isArchived = false, bool isDeleted = false)
		{
			if(departmentIds == null || departmentIds.Count <= 0)
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BestellungenExtension] WHERE [DepartmentId] IS NOT NULL AND [DepartmentId] IN ({string.Join(",", departmentIds)}) AND [PoPaymentType]=@paymentType {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} AND [Deleted]=@deleted "
						+ "AND ([LeasingStartYear]=@year OR ([LeasingStartYear]<@year AND (CAST(FLOOR(CAST(([LeasingStartMonth] - 1 + [LeasingNbMonths]) AS FLOAT) / 12) AS INTEGER) + [LeasingStartYear])>=@year ))";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("paymentType", (int)BestellungenExtensionEnums.OrderPaymentTypes.Leasing);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetLeasingByYearCompanyDepartmentEmployee(int year, int? companyId, int? departmentId, int? employeeId, bool? isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BestellungenExtension] WHERE "
						+ $"{(companyId.HasValue ? " [CompanyId] IS NOT NULL AND [CompanyId]=@companyId AND " : "")} "
						+ $"{(departmentId.HasValue ? " [DepartmentId] IS NOT NULL AND [DepartmentId]=@departmentId AND " : "")} "
						+ $"{(employeeId.HasValue ? " [IssuerId] IS NOT NULL AND [IssuerId]=@employeeId AND " : "")} "
						+ $"[PoPaymentType]=@paymentType {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} AND [Deleted]=@deleted "
						+ "AND ([LeasingStartYear]=@year OR ([LeasingStartYear]<@year AND (CAST(FLOOR(CAST(([LeasingStartMonth] - 1 + [LeasingNbMonths]) AS FLOAT) / 12) AS INTEGER) + [LeasingStartYear])>=@year ))";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("paymentType", (int)BestellungenExtensionEnums.OrderPaymentTypes.Leasing);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				if(companyId.HasValue)
					sqlCommand.Parameters.AddWithValue("companyId", companyId);
				if(departmentId.HasValue)
					sqlCommand.Parameters.AddWithValue("departmentId", departmentId);
				if(employeeId.HasValue)
					sqlCommand.Parameters.AddWithValue("employeeId", employeeId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static int MaxIdOrder()
		{
			var dataTable = new DataTable();
			int response = 0;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "select max(OrderId) as Max_Id from [__FNC_BestellungenExtension]";
				var sqlCommand = new SqlCommand(query, sqlConnection);


				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				response = Convert.ToInt32(dataTable.Rows[0]["Max_Id"]);
				// return toList2(dataTable);
			}
			return response;
		}
		public static int MaxVersionOrder()
		{
			var dataTable = new DataTable();
			int response = 0;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "select max(Nr_version_Order) as Max_Version from [Budget_Order_Version] where Id_Order=@Id_Order";
				var sqlCommand = new SqlCommand(query, sqlConnection);


				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				response = Convert.ToInt32(dataTable.Rows[0]["Max_Version"]);
			}
			return response;
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetByProject(int id_project, bool? isArchived = null, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BestellungenExtension] WHERE [ProjectId]=@Id {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} AND [Deleted]=@deleted";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id_project);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetByProjectsMaxLevel(List<Tuple<int, int>> ids_levels, bool? isArchived = null, bool isDeleted = false)
		{
			if(ids_levels == null || ids_levels.Count <= 0)
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				var queryParam = new List<string> { };
				foreach(var item in ids_levels)
				{
					queryParam.Add($" ([ProjectId]={item.Item1} AND [Level]<={item.Item2}) ");
				}
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BestellungenExtension] WHERE ({string.Join(" OR ", queryParam)}) AND [Deleted]=@deleted {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetByProjects_MinLevel(List<Tuple<int, int>> ids_levels, bool? isArchived = false, bool isDeleted = false)
		{
			if(ids_levels == null || ids_levels.Count <= 0)
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				var queryParam = new List<string> { };
				foreach(var item in ids_levels)
				{
					queryParam.Add($" ([ProjectId]={item.Item1} AND [Level]>={item.Item2}) ");
				}
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BestellungenExtension] WHERE ({string.Join(" OR ", queryParam)}) {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} AND [Deleted]=@deleted";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetByDepartmentIdAndLevel(int departmentId, int level, string type = "internal", bool? isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BestellungenExtension] WHERE [DepartmentId]=@departmentId AND [Level]=@level AND [OrderType]=@type {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} AND [Deleted]=@deleted";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("departmentId", departmentId);
				sqlCommand.Parameters.AddWithValue("level", level);
				sqlCommand.Parameters.AddWithValue("type", type);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetByCompanyIdAndLevel(int companyId, int level, string type = "internal", bool? isArchived = false, bool isDeleted = false, bool strictLevel = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BestellungenExtension] WHERE [CompanyId]=@companyId AND [Level] > 0 AND [Level]{(strictLevel ? "=" : "<=")}@level AND [OrderType]=@type {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} AND [Deleted]=@deleted";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("companyId", companyId);
				sqlCommand.Parameters.AddWithValue("level", level);
				sqlCommand.Parameters.AddWithValue("type", type);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetForPurchaseUser(int companyId, int level, bool? isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM dbo.[__FNC_BestellungenExtension] WHERE [Deleted]=@deleted {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} AND [OrderId] IN ( "
							+ "SELECT DISTINCT O.[orderId] FROM dbo.[__FNC_BestellungenExtension] AS O"
								+ " LEFT JOIN dbo.[__FNC_ProjectValidators]  AS P ON O.[ProjectId] = P.[Id_Project]"
								+ " WHERE [CompanyId]=@companyId AND ((O.[Level]=@level AND O.[OrderType] = 'internal')"
									+ " OR (O.[Level] = (SELECT Count(*) + 1 FROM dbo.[__FNC_ProjectValidators]  WHERE [Id_Project] = P.[Id_Project]) AND O.[OrderType]='external')))";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("companyId", companyId);
				sqlCommand.Parameters.AddWithValue("level", level);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetOrdersByValidators(int id, bool? isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BestellungenExtension]  where [Deleted]=@deleted {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} AND [ProjectId] In (SELECT [Id_Project] FROM [__FNC_ProjectValidators]  where [Id_Validator] = @Id)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetByDepartments(List<int> departmentIds, bool? booked, int? year = null, bool? isArchived = false, bool isDeleted = false)
		{
			if(departmentIds == null || departmentIds.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BestellungenExtension] AS E {(!booked.HasValue ? " WHERE " : "LEFT JOIN [Bestellungen] AS B ON E.OrderId=B.Nr WHERE B.Erledigt=@booked AND ")} [DepartmentId] IN ({string.Join(",", departmentIds)}) {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} AND [Deleted]=@deleted {(year.HasValue ? $" AND YEAR(CreationDate)={year.Value}" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				if(booked.HasValue)
					sqlCommand.Parameters.AddWithValue("booked", booked);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetByCompanies(List<int> companyIds, bool? booked, int? year = null, bool? isArchived = false, bool isDeleted = false)
		{
			if(companyIds == null || companyIds.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BestellungenExtension] AS E {(!booked.HasValue ? " WHERE " : "LEFT JOIN [Bestellungen] AS B ON E.OrderId=B.Nr WHERE B.Erledigt=@booked AND ")} [CompanyId] IN ({string.Join(",", companyIds)}) {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} AND [Deleted]=@deleted {(year.HasValue ? $" AND YEAR(CreationDate)={year.Value}" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				if(booked.HasValue)
					sqlCommand.Parameters.AddWithValue("booked", booked);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<int> FilterProjectByIds(List<int> id, bool? isArchived = false, bool isDeleted = false)
		{
			if(id == null || id.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT  DISTINCT [ProjectId] FROM [__FNC_BestellungenExtension]  where [Deleted]=@deleted {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} AND [ProjectId] In  ({string.Join(",", id)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => Convert.ToInt32(x[0])).ToList();
			}
			else
			{
				return new List<int>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetOrdersByLevelValidation(int Id_Project, int Level, bool? isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * from [__FNC_BestellungenExtension]  where [Level] > 0 AND [Level] <= @Level and [ProjectId] = @Id_Project";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id_Project", Id_Project);
				sqlCommand.Parameters.AddWithValue("Level", Level);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetOpenByCurrency(int currencyId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_BestellungenExtension] WHERE [CurrencyId]=@Id AND ISNULL([Archived],0)=@archived AND ISNULL([Deleted],0)=@deleted AND [Level] > 0 AND ApprovalTime IS NULL AND ISNULL(ApprovalUserId,0) <=0";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", currencyId);
				sqlCommand.Parameters.AddWithValue("archived", false);
				sqlCommand.Parameters.AddWithValue("deleted", false);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetValidatedbyUser(int id_user, bool? booked, bool? isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BestellungenExtension] AS E {(!booked.HasValue ? " WHERE " : "LEFT JOIN [Bestellungen] AS B ON E.OrderId=B.Nr WHERE B.Erledigt=@booked AND ")} [Deleted]=@deleted {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} AND [Level]>0";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id_user);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				if(booked.HasValue)
					sqlCommand.Parameters.AddWithValue("booked", booked);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetExternal(bool? booked, int? year = null, bool? isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BestellungenExtension] AS E {(!booked.HasValue ? " WHERE " : "LEFT JOIN [Bestellungen] AS B ON E.OrderId=B.Nr WHERE B.Erledigt=@booked AND ")} E.[OrderType]='external' {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} AND [Deleted]=@deleted {(year.HasValue ? $" AND YEAR(CreationDate)={year.Value}" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				if(booked.HasValue)
					sqlCommand.Parameters.AddWithValue("booked", booked);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetInternal(bool? booked, int? year = null, bool? isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BestellungenExtension] AS E {(!booked.HasValue ? " WHERE " : "LEFT JOIN [Bestellungen] AS B ON E.OrderId=B.Nr WHERE B.Erledigt=@booked AND ")} E.[OrderType]='internal' {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} AND [Deleted]=@deleted {(year.HasValue ? $" AND YEAR(CreationDate)={year.Value}" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				if(booked.HasValue)
					sqlCommand.Parameters.AddWithValue("booked", booked);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetForPurchaseUserExternal(List<int> projectIds, bool isArchived = false, bool isDeleted = false)
		{
			if(projectIds == null || projectIds.Count <= 0)
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM dbo.[__FNC_BestellungenExtension] WHERE [Archived]=@archived AND [Deleted]=@deleted AND [OrderId] IN ( "
							+ "SELECT DISTINCT O.[orderId] FROM dbo.[__FNC_BestellungenExtension] AS O"
								+ " LEFT JOIN dbo.[__FNC_ProjectValidators]  AS P ON O.[ProjectId] = P.[Id_Project]"
								+ $" WHERE [ProjectId] IN ({(string.Join(", ", projectIds))}) AND "
									+ " (O.[Level] = (SELECT Count(*) + 1 FROM dbo.[__FNC_ProjectValidators]  WHERE [Id_Project] = P.[Id_Project]) AND O.[OrderType]='external'))";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetByDepartmentAndYear(int departmentId, int year, BestellungenExtensionEnums.OrderPaymentTypes paymentType, bool? isArchived = null, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = paymentType == BestellungenExtensionEnums.OrderPaymentTypes.Purchase
					? $"SELECT * FROM [__FNC_BestellungenExtension] WHERE [DepartmentId]=@Id AND [PoPaymentType]=@paymentType {(isArchived.HasValue ? $" AND[Archived] ={(isArchived.Value == true ? "1" : "0")}" : "")} AND [Deleted]=@deleted AND year(CreationDate)=@year "
					: $"SELECT * FROM [__FNC_BestellungenExtension] WHERE [DepartmentId]=@Id AND [PoPaymentType]=@paymentType {(isArchived.HasValue ? $" AND [Archived]={(isArchived.Value == true ? "1" : "0")}" : "")} AND [Deleted]=@deleted "
						+ "AND ([LeasingStartYear]=@year OR ([LeasingStartYear]<@year AND (CAST(FLOOR(CAST(([LeasingStartMonth] - 1 + [LeasingNbMonths]) AS FLOAT) / 12) AS INTEGER) + [LeasingStartYear])>=@year ))";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", departmentId);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("paymentType", (int)paymentType);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetByDepartmentAndYearAndMonth(int departmentId, int year, int month, BestellungenExtensionEnums.OrderPaymentTypes paymentType, bool? isArchived = null, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = paymentType == BestellungenExtensionEnums.OrderPaymentTypes.Purchase
					? $"SELECT * FROM [__FNC_BestellungenExtension] WHERE [DepartmentId]=@Id AND [PoPaymentType]=@paymentType {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} AND [Deleted]=@deleted AND year(CreationDate)=@year AND month(CreationDate)=@month"
					: $"SELECT * FROM [__FNC_BestellungenExtension] WHERE [DepartmentId]=@Id AND [PoPaymentType]=@paymentType {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} AND [Deleted]=@deleted "
						+ "AND ("
							+ "([LeasingStartYear] = @year AND [LeasingStartMonth] <= @month) "
							+ "OR ([LeasingStartYear] < @year AND (CAST(FLOOR(CAST(([LeasingStartMonth] - 1 + [LeasingNbMonths]) AS FLOAT) / 12) AS INTEGER) + [LeasingStartYear]) < @year) " // - > last year
							+ "OR ([LeasingStartYear] < @year AND (CAST(FLOOR(CAST(([LeasingStartMonth] - 1 + [LeasingNbMonths]) AS FLOAT) / 12) AS INTEGER) + [LeasingStartYear]) = @year AND (([LeasingStartMonth] - 1 + [LeasingNbMonths]) % 12) >= @month)" // - = last & <= last month
						+ " ) ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", departmentId);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("month", month);
				sqlCommand.Parameters.AddWithValue("paymentType", (int)paymentType);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetByCompanyAndYear(int companyId, int year, BestellungenExtensionEnums.OrderPaymentTypes paymentType, bool? isArchived = null, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = paymentType == BestellungenExtensionEnums.OrderPaymentTypes.Purchase
					? $"SELECT * FROM [__FNC_BestellungenExtension] WHERE [CompanyId]=@Id AND [PoPaymentType]=@paymentType {(isArchived.HasValue ? $" AND [Archived]={(isArchived.Value == true ? "1" : "0")}" : "")} AND [Deleted]=@deleted AND year(CreationDate)=@year "
					: $"SELECT * FROM [__FNC_BestellungenExtension] WHERE [CompanyId]=@Id AND [PoPaymentType]=@paymentType {(isArchived.HasValue ? $" AND [Archived]={(isArchived.Value == true ? "1" : "0")}" : "")} AND [Deleted]=@deleted "
						+ "AND ([LeasingStartYear]=@year OR ([LeasingStartYear]<@year AND (CAST(FLOOR(CAST(([LeasingStartMonth] - 1 + [LeasingNbMonths]) AS FLOAT) / 12) AS INTEGER) + [LeasingStartYear])>=@year ))";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", companyId);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("paymentType", (int)paymentType);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetByCompanyAndYearAndMonth(int companyId, int year, int month, BestellungenExtensionEnums.OrderPaymentTypes paymentType, bool? isArchived = null, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = paymentType == BestellungenExtensionEnums.OrderPaymentTypes.Purchase
					? $"SELECT * FROM [__FNC_BestellungenExtension] WHERE [CompanyId]=@Id AND [PoPaymentType]=@paymentType {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} AND [Deleted]=@deleted AND year(CreationDate)=@year AND month(CreationDate)=@month"
					: $"SELECT * FROM [__FNC_BestellungenExtension] WHERE [CompanyId]=@Id AND [PoPaymentType]=@paymentType {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} AND [Deleted]=@deleted "
						+ "AND ("
							+ "([LeasingStartYear] = @year AND [LeasingStartMonth] <= @month) "
							+ "OR ([LeasingStartYear] < @year AND (CAST(FLOOR(CAST(([LeasingStartMonth] - 1 + [LeasingNbMonths]) AS FLOAT) / 12) AS INTEGER) + [LeasingStartYear]) < @year) " // - > last year
							+ "OR ([LeasingStartYear] < @year AND (CAST(FLOOR(CAST(([LeasingStartMonth] - 1 + [LeasingNbMonths]) AS FLOAT) / 12) AS INTEGER) + [LeasingStartYear]) = @year AND (([LeasingStartMonth] - 1 + [LeasingNbMonths]) % 12) >= @month)" // - = last & <= last month
						+ " ) ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", companyId);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("month", month);
				sqlCommand.Parameters.AddWithValue("paymentType", (int)paymentType);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		#region >>>> Stats <<<<<

		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetValidatedNonPlaced(string filter = null, int? year = null, List<int> companyIds = null, List<int> departmentIds = null, int? employeeId = null, bool isArchived = false, bool isDeleted = false, Settings.SortingModel sorting = null, Settings.PaginModel paging = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT DISTINCT E.* FROM [__FNC_BestellungenExtension] AS E WHERE "
						+ $"{(companyIds != null && companyIds.Count > 0 ? $" [CompanyId] IS NOT NULL AND [CompanyId] IN ({string.Join(",", companyIds)}) AND " : "")} "
						+ $"{(departmentIds != null && departmentIds.Count > 0 ? $" [DepartmentId] IS NOT NULL AND [DepartmentId] IN ({string.Join(",", departmentIds)}) AND " : "")} "
						+ $"{(employeeId.HasValue ? " [IssuerId] IS NOT NULL AND [IssuerId]=@employeeId AND " : "")} "
					+ $"ApprovalTime is not null AND ApprovalUserId is not null AND (OrderPlacedUserId is null OR OrderPlacedUserEmail is null) AND [Archived]=@archived AND [Deleted]=@deleted"
					+ $"{(year.HasValue ? $" AND YEAR([ValidationRequestTime])={year.Value}" : "")}"
					+ $"{(!String.IsNullOrEmpty(filter) ? $" AND ( [OrderNumber] LIKE '{filter}%' OR [ProjectName] LIKE '{filter}%' OR [SupplierName] LIKE '{filter}%' OR [IssuerName] LIKE '{filter}%' OR [DepartmentName] LIKE '{filter}%') " : "")}";

				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY [CreationDate] DESC ";
				}

				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				if(employeeId.HasValue)
					sqlCommand.Parameters.AddWithValue("employeeId", employeeId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetPlacedNonSupplierConfirmed(string filter = null, int? year = null, List<int> companyIds = null, List<int> departmentIds = null, int? employeeId = null, bool isArchived = false, bool isDeleted = false, Settings.SortingModel sorting = null, Settings.PaginModel paging = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT DISTINCT B.* FROM [__FNC_BestellungenExtension] AS B JOIN [__FNC_BestellteArtikelExtension] AS A ON B.OrderId=A.OrderId WHERE "
						+ $" {(companyIds != null && companyIds.Count > 0 ? $" [CompanyId] IS NOT NULL AND [CompanyId] IN ({string.Join(",", companyIds)}) AND " : "")} "
						+ $" {(departmentIds != null && departmentIds.Count > 0 ? $" [DepartmentId] IS NOT NULL AND [DepartmentId] IN ({string.Join(",", departmentIds)}) AND " : "")} "
						+ $" {(employeeId.HasValue ? " [IssuerId] IS NOT NULL AND [IssuerId]=@employeeId AND " : "")} "
						+ " ApprovalTime IS NOT NULL AND ApprovalUserId IS NOT NULL AND OrderPlacedUserId IS NOT NULL AND OrderPlacedUserEmail IS NOT NULL AND "
						+ " /*A.DeliveryDate IS NOT NULL AND*/ A.SupplierDeliveryDate IS NULL AND "
						+ $"[Archived]=@archived AND [Deleted]=@deleted{(year.HasValue ? $" AND YEAR([ValidationRequestTime])={year.Value}" : "")}"
				+ $"{(!String.IsNullOrEmpty(filter) ? $" AND ( [OrderNumber] LIKE '{filter}%' OR [ProjectName] LIKE '{filter}%' OR [SupplierName] LIKE '{filter}%' OR [IssuerName] LIKE '{filter}%' OR [DepartmentName] LIKE '{filter}%') " : "")}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				if(employeeId.HasValue)
					sqlCommand.Parameters.AddWithValue("employeeId", employeeId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetSupplierDeliveryOverdue(string filter = null, int? year = null, List<int> companyIds = null, List<int> departmentIds = null, int? employeeId = null, bool isArchived = false, bool isDeleted = false, Settings.SortingModel sorting = null, Settings.PaginModel paging = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT DISTINCT B.* FROM [__FNC_BestellungenExtension] AS B "
						+ $" JOIN [Bestellungen] AS O ON B.OrderId=O.Nr "
						+ $" JOIN [__FNC_BestellteArtikelExtension] AS A ON B.OrderId=A.OrderId WHERE "
						+ $" {(companyIds != null && companyIds.Count > 0 ? $" [CompanyId] IS NOT NULL AND [CompanyId] IN ({string.Join(",", companyIds)}) AND " : "")} "
						+ $" {(departmentIds != null && departmentIds.Count > 0 ? $" [DepartmentId] IS NOT NULL AND [DepartmentId] IN ({string.Join(",", departmentIds)}) AND " : "")} "
						+ $" {(employeeId.HasValue ? " [IssuerId] IS NOT NULL AND [IssuerId]=@employeeId AND " : "")} "
						+ $" ApprovalTime IS NOT NULL AND ApprovalUserId IS NOT NULL AND OrderPlacedUserId IS NOT NULL AND OrderPlacedUserEmail IS NOT NULL AND "
						+ $" O.Typ='{BestellungenAccess.ORDER_TYPE}' AND O.[erledigt]<>1 AND " // - not fully booked
						+ " ((A.SupplierDeliveryDate IS NOT NULL AND A.SupplierDeliveryDate < @today) OR (A.SupplierDeliveryDate IS NULL AND A.DeliveryDate < @today)) AND "
						+ $"[Archived]=@archived AND [Deleted]=@deleted{(year.HasValue ? $" AND YEAR([ValidationRequestTime])={year.Value}" : "")}"
				+ $"{(!String.IsNullOrEmpty(filter) ? $" AND ( [OrderNumber] LIKE '{filter}%' OR [ProjectName] LIKE '{filter}%' OR [SupplierName] LIKE '{filter}%' OR [IssuerName] LIKE '{filter}%' OR [DepartmentName] LIKE '{filter}%') " : "")}";


				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY [CreationDate] DESC ";
				}

				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				sqlCommand.Parameters.AddWithValue("today", DateTime.Today);
				if(employeeId.HasValue)
					sqlCommand.Parameters.AddWithValue("employeeId", employeeId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetUpcomingDeliveries(string filter = null, int? year = null, DateTime? endDate = null, List<int> companyIds = null, List<int> departmentIds = null, int? employeeId = null, bool isArchived = false, bool isDeleted = false, Settings.SortingModel sorting = null, Settings.PaginModel paging = null)
		{
			if(!endDate.HasValue || endDate.HasValue && endDate.Value < DateTime.Today)
				endDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT DISTINCT B.* FROM [__FNC_BestellungenExtension] AS B "
						+ $" JOIN [Bestellungen] AS O ON B.OrderId=O.Nr "
						+ $" JOIN [__FNC_BestellteArtikelExtension] AS A ON B.OrderId=A.OrderId WHERE "
						+ $" {(companyIds != null && companyIds.Count > 0 ? $" [CompanyId] IS NOT NULL AND [CompanyId] IN ({string.Join(",", companyIds)}) AND " : "")} "
						+ $" {(departmentIds != null && departmentIds.Count > 0 ? $" [DepartmentId] IS NOT NULL AND [DepartmentId] IN ({string.Join(",", departmentIds)}) AND " : "")} "
						+ $" {(employeeId.HasValue ? " [IssuerId] IS NOT NULL AND [IssuerId]=@employeeId AND " : "")} "
						+ " ApprovalTime IS NOT NULL AND ApprovalUserId IS NOT NULL AND OrderPlacedUserId IS NOT NULL AND OrderPlacedUserEmail IS NOT NULL AND "
						+ $" O.Typ='{BestellungenAccess.ORDER_TYPE}' AND O.[erledigt]<>1 AND " // - not fully booked
						+ " A.DeliveryDate IS NOT NULL AND ((A.SupplierDeliveryDate IS NOT NULL AND @today < A.SupplierDeliveryDate AND A.SupplierDeliveryDate < @endDate) OR (A.SupplierDeliveryDate IS NULL AND @today < A.DeliveryDate AND A.DeliveryDate < @endDate)) AND "
						+ $"[Archived]=@archived AND [Deleted]=@deleted{(year.HasValue ? $" AND YEAR([ValidationRequestTime])={year.Value}" : "")}";
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY [CreationDate] DESC ";
				}

				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				sqlCommand.Parameters.AddWithValue("today", DateTime.Today.AddDays(-1));
				sqlCommand.Parameters.AddWithValue("endDate", endDate.Value.AddDays(1));
				if(employeeId.HasValue)
					sqlCommand.Parameters.AddWithValue("employeeId", employeeId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetBooked(string filter = null, int? year = null, List<int> companyIds = null, List<int> departmentIds = null, int? employeeId = null, bool isArchived = false, bool isDeleted = false, Settings.SortingModel sorting = null, Settings.PaginModel paging = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT DISTINCT E.* FROM [__FNC_BestellungenExtension] AS E "
						+ $" JOIN [Bestellungen] AS B ON E.OrderId=B.Nr "
						+ $" JOIN (SELECT MAX(Datum) AS [Datum], [Bestellung-Nr] from [Bestellungen] WHERE Typ='{BestellungenAccess.RECEPTION_TYPE}' "
						+ " GROUP BY [bestellung-nr]) as W ON E.OrderId=W.[Bestellung-Nr]"
						+ " WHERE "
						+ $" {(companyIds != null && companyIds.Count > 0 ? $" [CompanyId] IS NOT NULL AND [CompanyId] IN ({string.Join(",", companyIds)}) AND " : "")} "
						+ $" {(departmentIds != null && departmentIds.Count > 0 ? $" [DepartmentId] IS NOT NULL AND [DepartmentId] IN ({string.Join(",", departmentIds)}) AND " : "")} "
						+ $" {(employeeId.HasValue ? " [IssuerId] IS NOT NULL AND [IssuerId]=@employeeId AND " : "")} "
						+ $" ApprovalTime IS NOT NULL AND ApprovalUserId IS NOT NULL AND OrderPlacedUserId IS NOT NULL AND OrderPlacedUserEmail IS NOT NULL AND "
						+ $" B.Typ='{BestellungenAccess.ORDER_TYPE}' AND B.[erledigt]=1 AND "
						+ $" Year(W.[Datum])=Year(@today) AND Month(W.[Datum])=Month(@today) AND "
						+ $"[Archived]=@archived AND [Deleted]=@deleted{(year.HasValue ? $" AND YEAR([ValidationRequestTime])={year.Value}" : "")}"
				+ $"{(!String.IsNullOrEmpty(filter) ? $" AND ( [OrderNumber] LIKE '{filter}%' OR [ProjectName] LIKE '{filter}%' OR [SupplierName] LIKE '{filter}%' OR [IssuerName] LIKE '{filter}%' OR [DepartmentName] LIKE '{filter}%') " : "")}";

				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY [CreationDate] DESC ";
				}

				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				sqlCommand.Parameters.AddWithValue("today", DateTime.Today.AddDays(-1));
				if(employeeId.HasValue)
					sqlCommand.Parameters.AddWithValue("employeeId", employeeId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetOpenLeasing(string filter = null, int? year = null, DateTime? endDate = null, List<int> companyIds = null, List<int> departmentIds = null, int? employeeId = null, bool isArchived = false, bool isDeleted = false, Settings.SortingModel sorting = null, Settings.PaginModel paging = null)
		{
			if(!endDate.HasValue || endDate.HasValue && endDate.Value < DateTime.Today)
				endDate = new DateTime(DateTime.Today.Year + 999, DateTime.Today.Month, 1);
			filter = filter?.Trim() ?? "";
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT DISTINCT E.* FROM [__FNC_BestellungenExtension] AS E WHERE "
						+ $" {(companyIds != null && companyIds.Count > 0 ? $" [CompanyId] IS NOT NULL AND [CompanyId] IN ({string.Join(",", companyIds)}) AND " : "")} "
						+ $" {(departmentIds != null && departmentIds.Count > 0 ? $" [DepartmentId] IS NOT NULL AND [DepartmentId] IN ({string.Join(",", departmentIds)}) AND " : "")} "
						+ $" {(employeeId.HasValue ? " [IssuerId] IS NOT NULL AND [IssuerId]=@employeeId AND " : "")} "
						+ $" ApprovalTime IS NOT NULL AND ApprovalUserId IS NOT NULL AND OrderPlacedUserId IS NOT NULL AND OrderPlacedUserEmail IS NOT NULL AND "
						+ $" [PoPaymentType]=@paymentType AND "
						+ " ("
							+ "([LeasingStartYear] = @year AND [LeasingStartMonth] <= @month) "
							+ "OR ([LeasingStartYear] < @year AND (CAST(FLOOR(CAST(([LeasingStartMonth] - 1 + [LeasingNbMonths]) AS FLOAT) / 12) AS INTEGER) + [LeasingStartYear]) < @year) " // - > last year
							+ "OR ([LeasingStartYear] < @year AND (CAST(FLOOR(CAST(([LeasingStartMonth] - 1 + [LeasingNbMonths]) AS FLOAT) / 12) AS INTEGER) + [LeasingStartYear]) = @year AND (([LeasingStartMonth] - 1 + [LeasingNbMonths]) % 12) >= @month)" // - = last & <= last month
						+ " ) AND "
						+ $"[Archived]=@archived AND [Deleted]=@deleted{(year.HasValue ? $" AND YEAR([ValidationRequestTime])={year.Value}" : "")}"
						+ $"{(!String.IsNullOrEmpty(filter) ? $" AND ( [OrderNumber] LIKE '{filter.SqlEscape()}%' OR [ProjectName] LIKE '{filter.SqlEscape()}%' OR [SupplierName] LIKE '{filter.SqlEscape()}%' OR [IssuerName] LIKE '{filter.SqlEscape()}%' OR [DepartmentName] LIKE '{filter.SqlEscape()}%') " : "")}";

				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY [CreationDate] DESC ";
				}

				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				sqlCommand.Parameters.AddWithValue("paymentType", (int)BestellungenExtensionEnums.OrderPaymentTypes.Leasing);
				sqlCommand.Parameters.AddWithValue("year", endDate.Value.Year);
				sqlCommand.Parameters.AddWithValue("month", endDate.Value.Month);
				if(employeeId.HasValue)
					sqlCommand.Parameters.AddWithValue("employeeId", employeeId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}


		public static int GetValidatedNonPlaced_Count(string filter = null, int? year = null, List<int> companyIds = null, List<int> departmentIds = null, int? employeeId = null, bool isArchived = false, bool isDeleted = false)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT COUNT( DISTINCT E.Id) FROM [__FNC_BestellungenExtension] AS E WHERE "
						+ $"{(companyIds != null && companyIds.Count > 0 ? $" [CompanyId] IS NOT NULL AND [CompanyId] IN ({string.Join(",", companyIds)}) AND " : "")} "
						+ $"{(departmentIds != null && departmentIds.Count > 0 ? $" [DepartmentId] IS NOT NULL AND [DepartmentId] IN ({string.Join(",", departmentIds)}) AND " : "")} "
						+ $"{(employeeId.HasValue ? " [IssuerId] IS NOT NULL AND [IssuerId]=@employeeId AND " : "")} "
					+ $"ApprovalTime is not null AND ApprovalUserId is not null AND (OrderPlacedUserId is null OR OrderPlacedUserEmail is null) AND [Archived]=@archived AND [Deleted]=@deleted {(year.HasValue ? $" AND YEAR([ValidationRequestTime])={year.Value}" : "")}"
				+ $"{(!String.IsNullOrEmpty(filter) ? $" AND ( [OrderNumber] LIKE '{filter}%' OR [ProjectName] LIKE '{filter}%' OR [SupplierName] LIKE '{filter}%' OR [IssuerName] LIKE '{filter}%' OR [DepartmentName] LIKE '{filter}%') " : "")}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				if(employeeId.HasValue)
					sqlCommand.Parameters.AddWithValue("employeeId", employeeId);

				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
			}

		}
		public static int GetPlacedNonSupplierConfirmed_Count(string filter = null, int? year = null, List<int> companyIds = null, List<int> departmentIds = null, int? employeeId = null, bool isArchived = false, bool isDeleted = false)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT COUNT(DISTINCT B.Id) FROM [__FNC_BestellungenExtension] AS B JOIN [__FNC_BestellteArtikelExtension] AS A ON B.OrderId=A.OrderId WHERE "
						+ $" {(companyIds != null && companyIds.Count > 0 ? $" [CompanyId] IS NOT NULL AND [CompanyId] IN ({string.Join(",", companyIds)}) AND " : "")} "
						+ $" {(departmentIds != null && departmentIds.Count > 0 ? $" [DepartmentId] IS NOT NULL AND [DepartmentId] IN ({string.Join(",", departmentIds)}) AND " : "")} "
						+ $" {(employeeId.HasValue ? " [IssuerId] IS NOT NULL AND [IssuerId]=@employeeId AND " : "")} "
						+ " ApprovalTime IS NOT NULL AND ApprovalUserId IS NOT NULL AND OrderPlacedUserId IS NOT NULL AND OrderPlacedUserEmail IS NOT NULL AND "
						+ " /*A.DeliveryDate IS NOT NULL AND*/ A.SupplierDeliveryDate IS NULL AND "
						+ $"[Archived]=@archived AND [Deleted]=@deleted {(year.HasValue ? $" AND YEAR([ValidationRequestTime])={year.Value}" : "")}"
				+ $"{(!String.IsNullOrEmpty(filter) ? $" AND ( [OrderNumber] LIKE '{filter}%' OR [ProjectName] LIKE '{filter}%' OR [SupplierName] LIKE '{filter}%' OR [IssuerName] LIKE '{filter}%' OR [DepartmentName] LIKE '{filter}%') " : "")}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				if(employeeId.HasValue)
					sqlCommand.Parameters.AddWithValue("employeeId", employeeId);

				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
			}
		}
		public static int GetSupplierDeliveryOverdue_Count(string filter = null, int? year = null, List<int> companyIds = null, List<int> departmentIds = null, int? employeeId = null, bool isArchived = false, bool isDeleted = false)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT COUNT(DISTINCT B.ID) FROM [__FNC_BestellungenExtension] AS B "
						+ $" JOIN [Bestellungen] AS O ON B.OrderId=O.Nr "
						+ $" JOIN [__FNC_BestellteArtikelExtension] AS A ON B.OrderId=A.OrderId WHERE "
						+ $" {(companyIds != null && companyIds.Count > 0 ? $" [CompanyId] IS NOT NULL AND [CompanyId] IN ({string.Join(",", companyIds)}) AND " : "")} "
						+ $" {(departmentIds != null && departmentIds.Count > 0 ? $" [DepartmentId] IS NOT NULL AND [DepartmentId] IN ({string.Join(",", departmentIds)}) AND " : "")} "
						+ $" {(employeeId.HasValue ? " [IssuerId] IS NOT NULL AND [IssuerId]=@employeeId AND " : "")} "
						+ $" ApprovalTime IS NOT NULL AND ApprovalUserId IS NOT NULL AND OrderPlacedUserId IS NOT NULL AND OrderPlacedUserEmail IS NOT NULL AND "
						+ $" O.Typ='{BestellungenAccess.ORDER_TYPE}' AND O.[erledigt]<>1 AND " // - not fully booked
						+ " ((A.SupplierDeliveryDate IS NOT NULL AND A.SupplierDeliveryDate < @today) OR (A.SupplierDeliveryDate IS NULL AND A.DeliveryDate < @today)) AND "
						+ $"[Archived]=@archived AND [Deleted]=@deleted{(year.HasValue ? $" AND YEAR([ValidationRequestTime])={year.Value}" : "")}"
				+ $"{(!String.IsNullOrEmpty(filter) ? $" AND ( [OrderNumber] LIKE '{filter}%' OR [ProjectName] LIKE '{filter}%' OR [SupplierName] LIKE '{filter}%' OR [IssuerName] LIKE '{filter}%' OR [DepartmentName] LIKE '{filter}%') " : "")}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				sqlCommand.Parameters.AddWithValue("today", DateTime.Today);
				if(employeeId.HasValue)
					sqlCommand.Parameters.AddWithValue("employeeId", employeeId);

				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
			}
		}
		public static int GetUpcomingDeliveries_Count(string filter = null, int? year = null, DateTime? endDate = null, List<int> companyIds = null, List<int> departmentIds = null, int? employeeId = null, bool isArchived = false, bool isDeleted = false)
		{
			if(!endDate.HasValue || endDate.HasValue && endDate.Value < DateTime.Today)
				endDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT COUNT(DISTINCT B.Id) FROM [__FNC_BestellungenExtension] AS B JOIN [__FNC_BestellteArtikelExtension] AS A ON B.OrderId=A.OrderId WHERE "
						+ $" {(companyIds != null && companyIds.Count > 0 ? $" [CompanyId] IS NOT NULL AND [CompanyId] IN ({string.Join(",", companyIds)}) AND " : "")} "
						+ $" {(departmentIds != null && departmentIds.Count > 0 ? $" [DepartmentId] IS NOT NULL AND [DepartmentId] IN ({string.Join(",", departmentIds)}) AND " : "")} "
						+ $" {(employeeId.HasValue ? " [IssuerId] IS NOT NULL AND [IssuerId]=@employeeId AND " : "")} "
						+ " ApprovalTime IS NOT NULL AND ApprovalUserId IS NOT NULL AND OrderPlacedUserId IS NOT NULL AND OrderPlacedUserEmail IS NOT NULL AND "
						+ " A.DeliveryDate IS NOT NULL AND ((A.SupplierDeliveryDate IS NOT NULL AND @today < A.SupplierDeliveryDate AND A.SupplierDeliveryDate < @endDate) OR (A.SupplierDeliveryDate IS NULL AND @today < A.DeliveryDate AND A.DeliveryDate < @endDate)) AND "
						+ $"[Archived]=@archived AND [Deleted]=@deleted {(year.HasValue ? $" AND YEAR([ValidationRequestTime])={year.Value}" : "")}"
						+ $"{(!String.IsNullOrEmpty(filter) ? $" AND ( [OrderNumber] LIKE '{filter}%' OR [ProjectName] LIKE '{filter}%' OR [SupplierName] LIKE '{filter}%' OR [IssuerName] LIKE '{filter}%' OR [DepartmentName] LIKE '{filter}%') " : "")}";


				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				sqlCommand.Parameters.AddWithValue("today", DateTime.Today.AddDays(-1));
				sqlCommand.Parameters.AddWithValue("endDate", endDate.Value.AddDays(1));
				if(employeeId.HasValue)
					sqlCommand.Parameters.AddWithValue("employeeId", employeeId);

				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
			}
		}
		public static int GetBooked_Count(string filter = null, int? year = null, DateTime? endDate = null, List<int> companyIds = null, List<int> departmentIds = null, int? employeeId = null, bool isArchived = false, bool isDeleted = false)
		{
			if(!endDate.HasValue || endDate.HasValue && endDate.Value < DateTime.Today)
				endDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT COUNT(DISTINCT E.Id) FROM [__FNC_BestellungenExtension] AS E "
						+ " JOIN [Bestellungen] AS B ON E.OrderId=B.Nr "
						+ $" JOIN (SELECT MAX(Datum) AS [Datum], [Bestellung-Nr] from [Bestellungen] WHERE Typ='{BestellungenAccess.RECEPTION_TYPE}' "
						+ " GROUP BY [bestellung-nr]) as W ON E.OrderId=W.[Bestellung-Nr]"
						+ " WHERE "
						+ $" {(companyIds != null && companyIds.Count > 0 ? $" [CompanyId] IS NOT NULL AND [CompanyId] IN ({string.Join(",", companyIds)}) AND " : "")} "
						+ $" {(departmentIds != null && departmentIds.Count > 0 ? $" [DepartmentId] IS NOT NULL AND [DepartmentId] IN ({string.Join(",", departmentIds)}) AND " : "")} "
						+ $" {(employeeId.HasValue ? " [IssuerId] IS NOT NULL AND [IssuerId]=@employeeId AND " : "")} "
						+ $" ApprovalTime IS NOT NULL AND ApprovalUserId IS NOT NULL AND OrderPlacedUserId IS NOT NULL AND OrderPlacedUserEmail IS NOT NULL AND "
						+ $" B.Typ='{BestellungenAccess.ORDER_TYPE}' AND B.[erledigt]=1 AND "
						+ $" Year(W.[Datum])=Year(@today) AND Month(W.[Datum])=Month(@today) AND "
						+ $"[Archived]=@archived AND [Deleted]=@deleted {(year.HasValue ? $" AND YEAR([ValidationRequestTime])={year.Value}" : "")}"
				+ $"{(!String.IsNullOrEmpty(filter) ? $" AND ( [OrderNumber] LIKE '{filter}%' OR [ProjectName] LIKE '{filter}%' OR [SupplierName] LIKE '{filter}%' OR [IssuerName] LIKE '{filter}%' OR [DepartmentName] LIKE '{filter}%') " : "")}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				sqlCommand.Parameters.AddWithValue("today", DateTime.Today.AddDays(-1));
				sqlCommand.Parameters.AddWithValue("endDate", endDate.Value.AddDays(1));
				if(employeeId.HasValue)
					sqlCommand.Parameters.AddWithValue("employeeId", employeeId);

				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
			}
		}
		public static int GetOpenLeasing_Count(string filter = null, int? year = null, DateTime? endDate = null, List<int> companyIds = null, List<int> departmentIds = null, int? employeeId = null, bool isArchived = false, bool isDeleted = false)
		{
			if(!endDate.HasValue || endDate.HasValue && endDate.Value < DateTime.Today)
				endDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
			filter = filter?.Trim() ?? "";

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT COUNT(DISTINCT E.Id) FROM [__FNC_BestellungenExtension] AS E WHERE "
						+ $" {(companyIds != null && companyIds.Count > 0 ? $" [CompanyId] IS NOT NULL AND [CompanyId] IN ({string.Join(",", companyIds)}) AND " : "")} "
						+ $" {(departmentIds != null && departmentIds.Count > 0 ? $" [DepartmentId] IS NOT NULL AND [DepartmentId] IN ({string.Join(",", departmentIds)}) AND " : "")} "
						+ $" {(employeeId.HasValue ? " [IssuerId] IS NOT NULL AND [IssuerId]=@employeeId AND " : "")} "
						+ $" ApprovalTime IS NOT NULL AND ApprovalUserId IS NOT NULL AND OrderPlacedUserId IS NOT NULL AND OrderPlacedUserEmail IS NOT NULL AND "
						+ $" [PoPaymentType]=@paymentType AND "
						+ " ("
							+ "([LeasingStartYear] = @year AND [LeasingStartMonth] <= @month) "
							+ "OR ([LeasingStartYear] < @year AND (CAST(FLOOR(CAST(([LeasingStartMonth] - 1 + [LeasingNbMonths]) AS FLOAT) / 12) AS INTEGER) + [LeasingStartYear]) < @year) " // - > last year
							+ "OR ([LeasingStartYear] < @year AND (CAST(FLOOR(CAST(([LeasingStartMonth] - 1 + [LeasingNbMonths]) AS FLOAT) / 12) AS INTEGER) + [LeasingStartYear]) = @year AND (([LeasingStartMonth] - 1 + [LeasingNbMonths]) % 12) >= @month)" // - = last & <= last month
						+ " ) AND "
						+ $"[Archived]=@archived AND [Deleted]=@deleted {(year.HasValue ? $" AND YEAR([ValidationRequestTime])={year.Value}" : "")}"
						+ $"{(!String.IsNullOrEmpty(filter) ? $" AND ( [OrderNumber] LIKE '{filter.SqlEscape()}%' OR [ProjectName] LIKE '{filter.SqlEscape()}%' OR [SupplierName] LIKE '{filter.SqlEscape()}%' OR [IssuerName] LIKE '{filter.SqlEscape()}%' OR [DepartmentName] LIKE '{filter.SqlEscape()}%') " : "")}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				sqlCommand.Parameters.AddWithValue("paymentType", (int)BestellungenExtensionEnums.OrderPaymentTypes.Leasing);
				sqlCommand.Parameters.AddWithValue("year", endDate.Value.Year);
				sqlCommand.Parameters.AddWithValue("month", endDate.Value.Month);
				if(employeeId.HasValue)
					sqlCommand.Parameters.AddWithValue("employeeId", employeeId);

				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
			}
		}
		public static decimal GetValidatedNonPlaced_Amount(List<int> companyIds = null, List<int> departmentIds = null, int? employeeId = null, bool isArchived = false, bool isDeleted = false)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT SUM(A.[TotalCost]) FROM [__FNC_BestellungenExtension] AS B "
						+ $" JOIN [__FNC_BestellteArtikelExtension] AS A ON A.OrderId=B.[OrderId] "
						+ "WHERE "
						+ $"{(companyIds != null && companyIds.Count > 0 ? $" [CompanyId] IS NOT NULL AND [CompanyId] IN ({string.Join(",", companyIds)}) AND " : "")} "
						+ $"{(departmentIds != null && departmentIds.Count > 0 ? $" [DepartmentId] IS NOT NULL AND [DepartmentId] IN ({string.Join(",", departmentIds)}) AND " : "")} "
						+ $"{(employeeId.HasValue ? " [IssuerId] IS NOT NULL AND [IssuerId]=@employeeId AND " : "")} "
					+ "ApprovalTime is not null AND ApprovalUserId is not null AND (OrderPlacedUserId is null OR OrderPlacedUserEmail is null) AND [Archived]=@archived AND [Deleted]=@deleted";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				if(employeeId.HasValue)
					sqlCommand.Parameters.AddWithValue("employeeId", employeeId);

				return decimal.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
			}

		}
		public static decimal GetPlacedNonSupplierConfirmed_Amount(List<int> companyIds = null, List<int> departmentIds = null, int? employeeId = null, bool isArchived = false, bool isDeleted = false)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT SUM(A.[TotalCost]) FROM [__FNC_BestellungenExtension] AS B "
						+ " JOIN [__FNC_BestellteArtikelExtension] AS A ON B.OrderId=A.OrderId WHERE "
						+ $" {(companyIds != null && companyIds.Count > 0 ? $" [CompanyId] IS NOT NULL AND [CompanyId] IN ({string.Join(",", companyIds)}) AND " : "")} "
						+ $" {(departmentIds != null && departmentIds.Count > 0 ? $" [DepartmentId] IS NOT NULL AND [DepartmentId] IN ({string.Join(",", departmentIds)}) AND " : "")} "
						+ $" {(employeeId.HasValue ? " [IssuerId] IS NOT NULL AND [IssuerId]=@employeeId AND " : "")} "
						+ " ApprovalTime IS NOT NULL AND ApprovalUserId IS NOT NULL AND OrderPlacedUserId IS NOT NULL AND OrderPlacedUserEmail IS NOT NULL AND "
						+ " /*A.DeliveryDate IS NOT NULL AND*/ A.SupplierDeliveryDate IS NULL AND "
						+ "[Archived]=@archived AND [Deleted]=@deleted";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				if(employeeId.HasValue)
					sqlCommand.Parameters.AddWithValue("employeeId", employeeId);

				return decimal.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
			}
		}
		public static decimal GetSupplierDeliveryOverdue_Amount(List<int> companyIds = null, List<int> departmentIds = null, int? employeeId = null, bool isArchived = false, bool isDeleted = false)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT SUM(A.[TotalCost]) FROM [__FNC_BestellungenExtension] AS B "
						+ "JOIN [__FNC_BestellteArtikelExtension] AS A ON B.OrderId=A.OrderId WHERE "
						+ $" {(companyIds != null && companyIds.Count > 0 ? $" [CompanyId] IS NOT NULL AND [CompanyId] IN ({string.Join(",", companyIds)}) AND " : "")} "
						+ $" {(departmentIds != null && departmentIds.Count > 0 ? $" [DepartmentId] IS NOT NULL AND [DepartmentId] IN ({string.Join(",", departmentIds)}) AND " : "")} "
						+ $" {(employeeId.HasValue ? " [IssuerId] IS NOT NULL AND [IssuerId]=@employeeId AND " : "")} "
						+ " ApprovalTime IS NOT NULL AND ApprovalUserId IS NOT NULL AND OrderPlacedUserId IS NOT NULL AND OrderPlacedUserEmail IS NOT NULL AND "
						+ " A.DeliveryDate IS NOT NULL AND ((A.SupplierDeliveryDate IS NOT NULL AND A.DeliveryDate < A.SupplierDeliveryDate) OR (A.SupplierDeliveryDate IS NULL AND A.DeliveryDate < @deliveryDate)) AND "
						+ "[Archived]=@archived AND [Deleted]=@deleted";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				sqlCommand.Parameters.AddWithValue("deliveryDate", DateTime.Today);
				if(employeeId.HasValue)
					sqlCommand.Parameters.AddWithValue("employeeId", employeeId);

				return decimal.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
			}
		}
		public static decimal GetUpcomingDeliveries_Amount(DateTime? endDate = null, List<int> companyIds = null, List<int> departmentIds = null, int? employeeId = null, bool isArchived = false, bool isDeleted = false)
		{
			if(!endDate.HasValue || endDate.HasValue && endDate.Value < DateTime.Today)
				endDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT SUM(A.[TotalCost]) FROM [__FNC_BestellungenExtension] AS B "
						+ "JOIN [__FNC_BestellteArtikelExtension] AS A ON B.OrderId=A.OrderId WHERE "
						+ $" {(companyIds != null && companyIds.Count > 0 ? $" [CompanyId] IS NOT NULL AND [CompanyId] IN ({string.Join(",", companyIds)}) AND " : "")} "
						+ $" {(departmentIds != null && departmentIds.Count > 0 ? $" [DepartmentId] IS NOT NULL AND [DepartmentId] IN ({string.Join(",", departmentIds)}) AND " : "")} "
						+ $" {(employeeId.HasValue ? " [IssuerId] IS NOT NULL AND [IssuerId]=@employeeId AND " : "")} "
						+ " ApprovalTime IS NOT NULL AND ApprovalUserId IS NOT NULL AND OrderPlacedUserId IS NOT NULL AND OrderPlacedUserEmail IS NOT NULL AND "
						+ " A.DeliveryDate IS NOT NULL AND ((A.SupplierDeliveryDate IS NOT NULL AND @today < A.SupplierDeliveryDate AND A.SupplierDeliveryDate < @endDate) OR (A.SupplierDeliveryDate IS NULL AND @today < A.DeliveryDate AND A.DeliveryDate < @endDate)) AND "
						+ "[Archived]=@archived AND [Deleted]=@deleted";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				sqlCommand.Parameters.AddWithValue("today", DateTime.Today.AddDays(-1));
				sqlCommand.Parameters.AddWithValue("endDate", endDate.Value.AddDays(1));
				if(employeeId.HasValue)
					sqlCommand.Parameters.AddWithValue("employeeId", employeeId);

				return decimal.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
			}
		}
		public static decimal GetBooked_Amount(DateTime? endDate = null, List<int> companyIds = null, List<int> departmentIds = null, int? employeeId = null, bool isArchived = false, bool isDeleted = false)
		{
			if(!endDate.HasValue || endDate.HasValue && endDate.Value < DateTime.Today)
				endDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT SUM(A.[TotalCost]) total FROM [__FNC_BestellungenExtension] AS E "
						+ $" JOIN [__FNC_BestellteArtikelExtension] AS A ON A.OrderId=E.[OrderId] "
						+ $" JOIN [Bestellungen] AS B ON E.OrderId=B.Nr "
						+ $" JOIN (SELECT MAX(Datum) AS [Datum], [Bestellung-Nr] from [Bestellungen] WHERE Typ='{BestellungenAccess.RECEPTION_TYPE}' "
						+ $" GROUP BY [bestellung-nr]) as W ON E.OrderId=W.[Bestellung-Nr]"
						+ $" WHERE "
						+ $" {(companyIds != null && companyIds.Count > 0 ? $" [CompanyId] IS NOT NULL AND [CompanyId] IN ({string.Join(",", companyIds)}) AND " : "")} "
						+ $" {(departmentIds != null && departmentIds.Count > 0 ? $" [DepartmentId] IS NOT NULL AND [DepartmentId] IN ({string.Join(",", departmentIds)}) AND " : "")} "
						+ $" {(employeeId.HasValue ? " [IssuerId] IS NOT NULL AND [IssuerId]=@employeeId AND " : "")} "
						+ $" ApprovalTime IS NOT NULL AND ApprovalUserId IS NOT NULL AND OrderPlacedUserId IS NOT NULL AND OrderPlacedUserEmail IS NOT NULL AND "
						+ $" B.Typ='{BestellungenAccess.ORDER_TYPE}' AND B.[erledigt]=1 AND "
						+ $" Year(W.[Datum])=Year(@today) AND Month(W.[Datum])=Month(@today) AND "
						+ "[Archived]=@archived AND [Deleted]=@deleted";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				sqlCommand.Parameters.AddWithValue("today", DateTime.Today.AddDays(-1));
				sqlCommand.Parameters.AddWithValue("endDate", endDate.Value.AddDays(1));
				if(employeeId.HasValue)
					sqlCommand.Parameters.AddWithValue("employeeId", employeeId);

				return decimal.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
			}
		}
		public static decimal GetOpenLeasing_Amount(DateTime? endDate = null, List<int> companyIds = null, List<int> departmentIds = null, int? employeeId = null, bool isArchived = false, bool isDeleted = false)
		{
			if(!endDate.HasValue || endDate.HasValue && endDate.Value < DateTime.Today)
				endDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT COUNT(*) FROM [__FNC_BestellungenExtension] AS E WHERE "
						+ $" {(companyIds != null && companyIds.Count > 0 ? $" [CompanyId] IS NOT NULL AND [CompanyId] IN ({string.Join(",", companyIds)}) AND " : "")} "
						+ $" {(departmentIds != null && departmentIds.Count > 0 ? $" [DepartmentId] IS NOT NULL AND [DepartmentId] IN ({string.Join(",", departmentIds)}) AND " : "")} "
						+ $" {(employeeId.HasValue ? " [IssuerId] IS NOT NULL AND [IssuerId]=@employeeId AND " : "")} "
						+ $" ApprovalTime IS NOT NULL AND ApprovalUserId IS NOT NULL AND OrderPlacedUserId IS NOT NULL AND OrderPlacedUserEmail IS NOT NULL AND "
						+ $" [PoPaymentType]=@paymentType AND "
						+ " ("
							+ "([LeasingStartYear] = @year AND [LeasingStartMonth] <= @month) "
							+ "OR ([LeasingStartYear] < @year AND (CAST(FLOOR(CAST(([LeasingStartMonth] - 1 + [LeasingNbMonths]) AS FLOAT) / 12) AS INTEGER) + [LeasingStartYear]) < @year) " // - > last year
							+ "OR ([LeasingStartYear] < @year AND (CAST(FLOOR(CAST(([LeasingStartMonth] - 1 + [LeasingNbMonths]) AS FLOAT) / 12) AS INTEGER) + [LeasingStartYear]) = @year AND (([LeasingStartMonth] - 1 + [LeasingNbMonths]) % 12) >= @month)" // - = last & <= last month
						+ " ) AND "
						+ "[Archived]=@archived AND [Deleted]=@deleted";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				sqlCommand.Parameters.AddWithValue("paymentType", (int)BestellungenExtensionEnums.OrderPaymentTypes.Leasing);
				sqlCommand.Parameters.AddWithValue("year", endDate.Value.Year);
				sqlCommand.Parameters.AddWithValue("month", endDate.Value.Month);
				if(employeeId.HasValue)
					sqlCommand.Parameters.AddWithValue("employeeId", employeeId);

				return decimal.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
			}
		}
		public static decimal GetLeasingMonth_Amount(DateTime? endDate = null, List<int> companyIds = null, List<int> departmentIds = null, int? employeeId = null, bool isArchived = false, bool isDeleted = false)
		{
			if(!endDate.HasValue)
				endDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT sum(LeasingMonthAmount) FROM [__FNC_BestellungenExtension] AS E WHERE "
						+ $" {(companyIds != null && companyIds.Count > 0 ? $" [CompanyId] IS NOT NULL AND [CompanyId] IN ({string.Join(",", companyIds)}) AND " : "")} "
						+ $" {(departmentIds != null && departmentIds.Count > 0 ? $" [DepartmentId] IS NOT NULL AND [DepartmentId] IN ({string.Join(",", departmentIds)}) AND " : "")} "
						+ $" {(employeeId.HasValue ? " [IssuerId] IS NOT NULL AND [IssuerId]=@employeeId AND " : "")} "
						+ $" ApprovalTime IS NOT NULL AND ApprovalUserId IS NOT NULL AND OrderPlacedUserId IS NOT NULL AND OrderPlacedUserEmail IS NOT NULL AND "
						+ $" [PoPaymentType]=@paymentType AND "
						+ " ("
							+ "([LeasingStartYear] = @year AND [LeasingStartMonth] <= @month) "
							+ "OR ([LeasingStartYear] < @year AND (CAST(FLOOR(CAST(([LeasingStartMonth] - 1 + [LeasingNbMonths]) AS FLOAT) / 12) AS INTEGER) + [LeasingStartYear]) < @year) " // - > last year
							+ "OR ([LeasingStartYear] < @year AND (CAST(FLOOR(CAST(([LeasingStartMonth] - 1 + [LeasingNbMonths]) AS FLOAT) / 12) AS INTEGER) + [LeasingStartYear]) = @year AND (([LeasingStartMonth] - 1 + [LeasingNbMonths]) % 12) >= @month)" // - = last & <= last month
						+ " ) AND "
						+ "[Archived]=@archived AND [Deleted]=@deleted ";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				sqlCommand.Parameters.AddWithValue("paymentType", (int)BestellungenExtensionEnums.OrderPaymentTypes.Leasing);
				sqlCommand.Parameters.AddWithValue("year", endDate.Value.Year);
				sqlCommand.Parameters.AddWithValue("month", endDate.Value.Month);
				if(employeeId.HasValue)
					sqlCommand.Parameters.AddWithValue("employeeId", employeeId);

				return decimal.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetByLevel(int level, string type = "internal", bool? isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BestellungenExtension] WHERE [Level] > @level AND [OrderType]=@type {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} AND [Deleted]=@deleted";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("level", level);
				sqlCommand.Parameters.AddWithValue("type", type);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		#endregion Stats
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetOpenByUser(int employeeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT DISTINCT E.* FROM [__FNC_BestellungenExtension] AS E WHERE "
						+ $" [IssuerId] IS NOT NULL AND [IssuerId]=@employeeId AND "
						+ $" [Level]>0 AND (ApprovalTime IS NULL OR ApprovalUserId IS NULL) ";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("employeeId", employeeId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetFinanceByValidator(int companyId, string type, bool? isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BestellungenExtension] WHERE [CompanyId]=@companyId AND [Level]>0 AND ISNULL([ApprovalUserId],0)=0 AND [OrderType]=@type {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} AND [Deleted]=@deleted";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("companyId", companyId);
				sqlCommand.Parameters.AddWithValue("type", type);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}

		public static int SoftDelete(int id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_BestellungenExtension] SET [Description] = WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetByUserAndLevelAndPayementTypeAndYear(int level, int payementType, int year, int userId, string type = "internal", bool? isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $@"SELECT * FROM [__FNC_BestellungenExtension] WHERE [Level] > @level 
                                AND [OrderType]=@type {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} 
                                AND [Deleted]=@deleted
                                AND [PoPaymentType]<>@payementType AND [CreationDate] IS NOT NULL AND YEAR([CreationDate])=@year
                                AND [IssuerId]=@userId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("level", level);
				sqlCommand.Parameters.AddWithValue("type", type);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				sqlCommand.Parameters.AddWithValue("payementType", payementType);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("userId", userId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetByCompanyAndLevelAndPayementTypeAndYear(int level, int payementType, int year, int companyId, string type = "internal", bool? isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $@"SELECT * FROM [__FNC_BestellungenExtension] WHERE [Level] > @level 
                                AND [OrderType]=@type {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} 
                                AND [Deleted]=@deleted
                                AND [PoPaymentType]<>@payementType AND [CreationDate] IS NOT NULL AND YEAR([CreationDate])=@year
                                AND [CompanyId]=@companyId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("level", level);
				sqlCommand.Parameters.AddWithValue("type", type);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				sqlCommand.Parameters.AddWithValue("payementType", payementType);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("companyId", companyId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetByDepartementAndLevelAndPayementTypeAndYear(int level, int payementType, int year, int departmentId, string type = "internal", bool? isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $@"SELECT * FROM [__FNC_BestellungenExtension] WHERE [Level] > @level 
                                AND [OrderType]=@type {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")} 
                                AND [Deleted]=@deleted
                                AND [PoPaymentType]<>@payementType AND [CreationDate] IS NOT NULL AND YEAR([CreationDate])=@year
                                AND [DepartmentId]=@departmentId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("level", level);
				sqlCommand.Parameters.AddWithValue("type", type);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				sqlCommand.Parameters.AddWithValue("payementType", payementType);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("departmentId", departmentId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetLeasingByCompanyAndYear(int year, int companyId, bool? isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $@"SELECT * FROM [__FNC_BestellungenExtension] WHERE
                        [PoPaymentType]=@paymentType {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")}
                        AND [Deleted]=@deleted
						AND (
						([LeasingStartYear] <= @year )
						OR (CAST(FLOOR(CAST(([LeasingStartMonth] - 1 + [LeasingNbMonths]) AS FLOAT) / 12) AS INTEGER) + [LeasingStartYear]) >= @year
						) AND [CompanyId]=@companyId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("paymentType", (int)BestellungenExtensionEnums.OrderPaymentTypes.Leasing);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				sqlCommand.Parameters.AddWithValue("companyId", companyId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetLeasingByDepartemantAndYear(int year, int departmentId, bool? isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $@"SELECT * FROM [__FNC_BestellungenExtension] WHERE
                        [PoPaymentType]=@paymentType {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")}
                        AND [Deleted]=@deleted
						AND (
						([LeasingStartYear] <= @year )
						OR (CAST(FLOOR(CAST(([LeasingStartMonth] - 1 + [LeasingNbMonths]) AS FLOAT) / 12) AS INTEGER) + [LeasingStartYear]) >= @year
						) AND [DepartmentId]=@departmentId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("paymentType", (int)BestellungenExtensionEnums.OrderPaymentTypes.Leasing);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				sqlCommand.Parameters.AddWithValue("departmentId", departmentId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetLeasingByUserAndYear(int year, int userId, bool? isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $@"SELECT * FROM [__FNC_BestellungenExtension] WHERE
                        [PoPaymentType]=@paymentType {(isArchived.HasValue ? $"AND [Archived]={(isArchived.Value ? "1" : "0")} " : "")}
                        AND [Deleted]=@deleted
						AND (
						([LeasingStartYear] <= @year )
						OR (CAST(FLOOR(CAST(([LeasingStartMonth] - 1 + [LeasingNbMonths]) AS FLOAT) / 12) AS INTEGER) + [LeasingStartYear]) >= @year
						) AND [IssuerId]=@userId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("paymentType", (int)BestellungenExtensionEnums.OrderPaymentTypes.Leasing);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				sqlCommand.Parameters.AddWithValue("userId", userId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetBySuperValidators(int validatorId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = @$"select * from __FNC_BestellungenExtension where CompanyId in 
								(
								select CompanyId from __FNC_CompanyExtension where SuperValidatorOneId=@validatorId or SuperValidatorTowId=@validatorId
								) and level=6";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("validatorId", validatorId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}

		#endregion
		//--

		#region >>  custom DAL methods added by Adda Issa Abdoul Razak 
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetByProjectIds(List<int> ids, bool isArchived = false, bool isDeleted = false)
		{
			if(ids == null || ids.Count <= 0)// because sometime we used ( predicat ?? -1 )
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{

				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BestellungenExtension] WHERE [ProjectId] IN ({string.Join(",", ids)})  AND [Archived]=@archived AND [Deleted]=@deleted";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}

		#endregion

		#region >>>>  personalize 05-02-2024 added by Adda Issa Abdoul razak

		//-- above there is a question for M.sani >> Bestellegun Get method

		// REM : Review After 
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetDataFromLastYearLeasing(BestellungenExtensionEnums.OrderPaymentTypes paymentType, bool? isArchived = null, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = paymentType == BestellungenExtensionEnums.OrderPaymentTypes.Purchase
					? $"SELECT * FROM [__FNC_BestellungenExtension] WHERE  [PoPaymentType]=@paymentType {(isArchived.HasValue ? $" AND [Archived]={(isArchived.Value == true ? "1" : "0")}" : "")} AND [Deleted]=@deleted AND year(CreationDate) = YEAR(GETDATE()) - 1 "
					: $"SELECT * FROM [__FNC_BestellungenExtension] WHERE  [PoPaymentType]=@paymentType {(isArchived.HasValue ? $" AND [Archived]={(isArchived.Value == true ? "1" : "0")}" : "")} AND [Deleted]=@deleted "
						+ "AND ([LeasingStartYear]=@year OR ([LeasingStartYear] = @year AND (CAST(FLOOR(CAST(([LeasingStartMonth] - 1 + [LeasingNbMonths]) AS FLOAT) / 12) AS INTEGER) + [LeasingStartYear]) >= YEAR(GETDATE()) - 1 ))";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				//sqlCommand.Parameters.AddWithValue("Id", userId);

				//var currentYear = DateTime.Now.Year;

				//var lastyear = currentYear - 1;

				//sqlCommand.Parameters.AddWithValue("year", lastyear);
				sqlCommand.Parameters.AddWithValue("paymentType", (int)paymentType);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}


		#endregion

		#region >>>> Get Data From Last Year until Today		
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetDataFromLastYear()
		{
			DataTable dataTable = new DataTable();

			using(SqlConnection sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{

				sqlConnection.Open();

				string Query = $"SELECT * FROM __FNC_BestellungenExtension WHERE CreationDate >= DATEADD(year, -1, DATEADD(day, DATEDIFF(day, 0, GETDATE()), 0)) AND CreationDate < CAST(GETDATE() AS DATE);";

				SqlCommand command = new SqlCommand(Query, sqlConnection);

				new SqlDataAdapter(command).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}


		}
		#endregion

		#region  >>>>> matching between Order and booked
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetbBookedOrder(bool? booked = null, bool isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BestellungenExtension] AS E {(!booked.HasValue ? " WHERE " : "LEFT JOIN[Bestellungen] AS B ON E.OrderId = B.Nr WHERE B.Erledigt = @booked AND ")} [Archived]=@archived AND [Deleted]=@deleted";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("archived", isArchived);
				sqlCommand.Parameters.AddWithValue("deleted", isDeleted);
				if(booked.HasValue)
					sqlCommand.Parameters.AddWithValue("booked", booked);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
		}

		#endregion

		#region >>> Matching beetween Besttelungen and OrderValidation 

		public static List<(int OrderId, string OrderNumber, DateTime ValidationTime, DateTime ValidationRequestTime, string TimeDifference)> GetTopFiveFastesFilteredOrders()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT TOP 5 o.OrderId, o.OrderNumber, ov.ValidationTime, o.ValidationRequestTime, o.ApprovalTime, o.ApprovalUserId FROM __FNC_BestellungenExtension AS o "
							 + $"INNER JOIN (SELECT OrderID, MAX(ValidationTime) As ValidationTime FROM [dbo].[__FNC_OrderValidation] GROUP BY OrderId) AS ov "
							 + $"ON o.OrderId = ov.OrderId "
							 + $"WHERE ov.ValidationTime IS NOT NULL AND ov.ValidationTime > o.ValidationRequestTime "
							 + $"AND o.ApprovalUserId IS NOT NULL AND o.ApprovalTime IS NOT NULL "
							 + $"ORDER BY DATEDIFF(SECOND, o.ValidationRequestTime, ov.ValidationTime) ASC;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				List<(int OrderId, string OrderNumber, DateTime ValidationTime, DateTime ValidationRequestTime, string TimeDifference)> filteredOrders = new List<(int OrderId, string OrderNumber, DateTime ValidationTime, DateTime ValidationRequestTime, string TimeDifference)>();

				foreach(DataRow row in dataTable.Rows)
				{
					int orderId = Convert.ToInt32(row["OrderId"]);
					string orderNumber = row["OrderNumber"].ToString();
					DateTime validationTime = Convert.ToDateTime(row["ValidationTime"]);
					DateTime validationRequestTime = Convert.ToDateTime(row["ValidationRequestTime"]);

					// Calculer la différence entre les temps
					TimeSpan timeDifference = validationTime - validationRequestTime;
					string formattedTimeDifference = ConvertToSeconds(timeDifference);

					filteredOrders.Add((orderId, orderNumber, validationTime, validationRequestTime, formattedTimeDifference));
				}

				return filteredOrders;
			}
			else
			{
				// Si aucun enregistrement, retourner une liste vide
				return new List<(int OrderId, string OrderNumber, DateTime ValidationTime, DateTime ValidationRequestTime, string TimeDifference)>();
			}
		}

		public static string ConvertToSeconds(TimeSpan timeSpan)
		{
			int days = (int)timeSpan.TotalDays;
			int years = days / 365;
			int remainingDays = days % 365;

			int months = remainingDays / 30; // Approximation, 30 days per month
			int remainingDaysInMonth = remainingDays % 30;

			int hours = timeSpan.Hours;
			int minutes = timeSpan.Minutes;
			int seconds = timeSpan.Seconds;

			string timeToken = "";

			timeToken = minutes == 0 ? "" : $"{minutes} minutes";
			timeToken += seconds == 0 ? "" : $"{seconds} seconds ";

			return timeToken;
		}


		// Fonction pour formater la différence entre les temps


		// Fonction pour formater la différence entre les temps
		//private static string ConvertToYearsMonthsDaysHoursMinutesSeconds(TimeSpan timeDifference)
		//{
		//	string formattedTimeDifference = $"{timeDifference.Days} jours, {timeDifference.Hours} heures, {timeDifference.Minutes} minutes, {timeDifference.Seconds} secondes";
		//	return formattedTimeDifference;
		//}



		public static List<(int OrderId, string OrderNumber, DateTime ValidationTime, DateTime ValidationRequestTime, string TimeDifference)> GetTopFiveCheapestFilteredOrders()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT TOP 5 o.OrderId, o.OrderNumber, ov.ValidationTime, o.ValidationRequestTime, o.ApprovalTime, o.ApprovalUserId FROM __FNC_BestellungenExtension AS o "
							 + $"INNER JOIN (SELECT OrderID, MAX(ValidationTime) As ValidationTime FROM [dbo].[__FNC_OrderValidation] GROUP BY OrderId) AS ov "
							 + $"ON o.OrderId = ov.OrderId "
							 + $"WHERE ov.ValidationTime IS NOT NULL AND ov.ValidationTime > o.ValidationRequestTime "
							 + $"AND o.ApprovalUserId IS NOT NULL AND o.ApprovalTime IS NOT NULL "
							 + $"ORDER BY DATEDIFF(DAY, o.ValidationRequestTime, ov.ValidationTime) DESC;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				List<(int OrderId, string OrderNumber, DateTime ValidationTime, DateTime ValidationRequestTime, string TimeDifference)> filteredOrders = new List<(int OrderId, string OrderNumber, DateTime ValidationTime, DateTime ValidationRequestTime, string TimeDifference)>();

				foreach(DataRow row in dataTable.Rows)
				{
					int orderId = Convert.ToInt32(row["OrderId"]);
					string orderNumber = row["OrderNumber"].ToString();
					DateTime validationTime = Convert.ToDateTime(row["ValidationTime"]);
					DateTime validationRequestTime = Convert.ToDateTime(row["ValidationRequestTime"]);

					// Calculer la différence entre les temps
					TimeSpan timeDifference = validationTime - validationRequestTime;
					string formattedTimeDifference = ConvertToYearsMonthsDaysHoursMinutesSeconds(timeDifference);

					filteredOrders.Add((orderId, orderNumber, validationTime, validationRequestTime, formattedTimeDifference));

				}

				return filteredOrders;
			}
			else
			{
				// Si le DataTable est vide, retourne une liste vide
				return new List<(int OrderId, string OrderNumber, DateTime ValidationTime, DateTime ValidationRequestTime, string TimeDifference)>();
			}
		}

		#endregion

		public static string ConvertToYearsMonthsDaysHoursMinutesSeconds(TimeSpan timeSpan)
		{
			int days = (int)timeSpan.TotalDays;
			int years = days / 365;
			int remainingDays = days % 365;

			int months = remainingDays / 30; // Approximation, 30 days per month
			int remainingDaysInMonth = remainingDays % 30;

			int hours = timeSpan.Hours;
			int minutes = timeSpan.Minutes;
			int seconds = timeSpan.Seconds;

			string timeToken = "";
			timeToken = years == 0 ? "" : $"{years} années ";
			timeToken += months == 0 ? "" : $"{months} mois ";
			//timeToken += remainingDaysInMonth == 0 ? "" : $"{remainingDaysInMonth} jours ";
			//timeToken += hours == 0 ? "" : $" {hours} heures";

			return timeToken;
		}
	}
}
