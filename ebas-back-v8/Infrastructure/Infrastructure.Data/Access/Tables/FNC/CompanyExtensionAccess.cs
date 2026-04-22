using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class CompanyExtensionAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_CompanyExtension] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_CompanyExtension]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__FNC_CompanyExtension] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__FNC_CompanyExtension] ([Account1],[Account2],[Account3],[Account4],[Address],[BankDetails1],[BankDetails2],[BankDetails3],[BankDetails4],[BLZ1],[BLZ2],[BLZ3],[BLZ4],[City],[CompanyId],[CompanyName],[Country],[CustomsNumber],[DefaultCurrencyId],[DefaultCurrencyName],[DeliveryAddress],[Email],[Fax],[FinanceProfileEmail],[FinanceProfileId],[FinanceProfileName],[FinanceValidatorOneEmail],[FinanceValidatorOneId],[FinanceValidatorOneName],[FinanceValidatorTowEmail],[FinanceValidatorTowId],[FinanceValidatorTowName],[HRB],[IBAN1],[IBAN2],[IBAN3],[IBAN4],[Manager1],[Manager2],[Manager3],[OrderPrefix],[Phone],[PostalCode],[PurchaseGroupEmail],[PurchaseGroupName],[PurchaseId],[PurchaseName],[PurchaseProfileId],[ReportDefaultLanguage],[ReportDefaultLanguageId],[Site],[SuperValidatorOneEmail],[SuperValidatorOneId],[SuperValidatorOneName],[SuperValidatorTowEmail],[SuperValidatorTowId],[SuperValidatorTowName],[SWIFT_BIC1],[SWIFT_BIC2],[SWIFT_BIC3],[SWIFT_BIC4],[TaxNumberID],[VATNumberID]) OUTPUT INSERTED.[Id] VALUES (@Account1,@Account2,@Account3,@Account4,@Address,@BankDetails1,@BankDetails2,@BankDetails3,@BankDetails4,@BLZ1,@BLZ2,@BLZ3,@BLZ4,@City,@CompanyId,@CompanyName,@Country,@CustomsNumber,@DefaultCurrencyId,@DefaultCurrencyName,@DeliveryAddress,@Email,@Fax,@FinanceProfileEmail,@FinanceProfileId,@FinanceProfileName,@FinanceValidatorOneEmail,@FinanceValidatorOneId,@FinanceValidatorOneName,@FinanceValidatorTowEmail,@FinanceValidatorTowId,@FinanceValidatorTowName,@HRB,@IBAN1,@IBAN2,@IBAN3,@IBAN4,@Manager1,@Manager2,@Manager3,@OrderPrefix,@Phone,@PostalCode,@PurchaseGroupEmail,@PurchaseGroupName,@PurchaseId,@PurchaseName,@PurchaseProfileId,@ReportDefaultLanguage,@ReportDefaultLanguageId,@Site,@SuperValidatorOneEmail,@SuperValidatorOneId,@SuperValidatorOneName,@SuperValidatorTowEmail,@SuperValidatorTowId,@SuperValidatorTowName,@SWIFT_BIC1,@SWIFT_BIC2,@SWIFT_BIC3,@SWIFT_BIC4,@TaxNumberID,@VATNumberID); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Account1", item.Account1 == null ? (object)DBNull.Value : item.Account1);
					sqlCommand.Parameters.AddWithValue("Account2", item.Account2 == null ? (object)DBNull.Value : item.Account2);
					sqlCommand.Parameters.AddWithValue("Account3", item.Account3 == null ? (object)DBNull.Value : item.Account3);
					sqlCommand.Parameters.AddWithValue("Account4", item.Account4 == null ? (object)DBNull.Value : item.Account4);
					sqlCommand.Parameters.AddWithValue("Address", item.Address == null ? (object)DBNull.Value : item.Address);
					sqlCommand.Parameters.AddWithValue("BankDetails1", item.BankDetails1 == null ? (object)DBNull.Value : item.BankDetails1);
					sqlCommand.Parameters.AddWithValue("BankDetails2", item.BankDetails2 == null ? (object)DBNull.Value : item.BankDetails2);
					sqlCommand.Parameters.AddWithValue("BankDetails3", item.BankDetails3 == null ? (object)DBNull.Value : item.BankDetails3);
					sqlCommand.Parameters.AddWithValue("BankDetails4", item.BankDetails4 == null ? (object)DBNull.Value : item.BankDetails4);
					sqlCommand.Parameters.AddWithValue("BLZ1", item.BLZ1 == null ? (object)DBNull.Value : item.BLZ1);
					sqlCommand.Parameters.AddWithValue("BLZ2", item.BLZ2 == null ? (object)DBNull.Value : item.BLZ2);
					sqlCommand.Parameters.AddWithValue("BLZ3", item.BLZ3 == null ? (object)DBNull.Value : item.BLZ3);
					sqlCommand.Parameters.AddWithValue("BLZ4", item.BLZ4 == null ? (object)DBNull.Value : item.BLZ4);
					sqlCommand.Parameters.AddWithValue("City", item.City == null ? (object)DBNull.Value : item.City);
					sqlCommand.Parameters.AddWithValue("CompanyId", item.CompanyId);
					sqlCommand.Parameters.AddWithValue("CompanyName", item.CompanyName == null ? (object)DBNull.Value : item.CompanyName);
					sqlCommand.Parameters.AddWithValue("Country", item.Country == null ? (object)DBNull.Value : item.Country);
					sqlCommand.Parameters.AddWithValue("CustomsNumber", item.CustomsNumber == null ? (object)DBNull.Value : item.CustomsNumber);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyId", item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyName", item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
					sqlCommand.Parameters.AddWithValue("DeliveryAddress", item.DeliveryAddress == null ? (object)DBNull.Value : item.DeliveryAddress);
					sqlCommand.Parameters.AddWithValue("Email", item.Email == null ? (object)DBNull.Value : item.Email);
					sqlCommand.Parameters.AddWithValue("Fax", item.Fax == null ? (object)DBNull.Value : item.Fax);
					sqlCommand.Parameters.AddWithValue("FinanceProfileEmail", item.FinanceProfileEmail == null ? (object)DBNull.Value : item.FinanceProfileEmail);
					sqlCommand.Parameters.AddWithValue("FinanceProfileId", item.FinanceProfileId == null ? (object)DBNull.Value : item.FinanceProfileId);
					sqlCommand.Parameters.AddWithValue("FinanceProfileName", item.FinanceProfileName == null ? (object)DBNull.Value : item.FinanceProfileName);
					sqlCommand.Parameters.AddWithValue("FinanceValidatorOneEmail", item.FinanceValidatorOneEmail == null ? (object)DBNull.Value : item.FinanceValidatorOneEmail);
					sqlCommand.Parameters.AddWithValue("FinanceValidatorOneId", item.FinanceValidatorOneId == null ? (object)DBNull.Value : item.FinanceValidatorOneId);
					sqlCommand.Parameters.AddWithValue("FinanceValidatorOneName", item.FinanceValidatorOneName == null ? (object)DBNull.Value : item.FinanceValidatorOneName);
					sqlCommand.Parameters.AddWithValue("FinanceValidatorTowEmail", item.FinanceValidatorTowEmail == null ? (object)DBNull.Value : item.FinanceValidatorTowEmail);
					sqlCommand.Parameters.AddWithValue("FinanceValidatorTowId", item.FinanceValidatorTowId == null ? (object)DBNull.Value : item.FinanceValidatorTowId);
					sqlCommand.Parameters.AddWithValue("FinanceValidatorTowName", item.FinanceValidatorTowName == null ? (object)DBNull.Value : item.FinanceValidatorTowName);
					sqlCommand.Parameters.AddWithValue("HRB", item.HRB == null ? (object)DBNull.Value : item.HRB);
					sqlCommand.Parameters.AddWithValue("IBAN1", item.IBAN1 == null ? (object)DBNull.Value : item.IBAN1);
					sqlCommand.Parameters.AddWithValue("IBAN2", item.IBAN2 == null ? (object)DBNull.Value : item.IBAN2);
					sqlCommand.Parameters.AddWithValue("IBAN3", item.IBAN3 == null ? (object)DBNull.Value : item.IBAN3);
					sqlCommand.Parameters.AddWithValue("IBAN4", item.IBAN4 == null ? (object)DBNull.Value : item.IBAN4);
					sqlCommand.Parameters.AddWithValue("Manager1", item.Manager1 == null ? (object)DBNull.Value : item.Manager1);
					sqlCommand.Parameters.AddWithValue("Manager2", item.Manager2 == null ? (object)DBNull.Value : item.Manager2);
					sqlCommand.Parameters.AddWithValue("Manager3", item.Manager3 == null ? (object)DBNull.Value : item.Manager3);
					sqlCommand.Parameters.AddWithValue("OrderPrefix", item.OrderPrefix == null ? (object)DBNull.Value : item.OrderPrefix);
					sqlCommand.Parameters.AddWithValue("Phone", item.Phone == null ? (object)DBNull.Value : item.Phone);
					sqlCommand.Parameters.AddWithValue("PostalCode", item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
					sqlCommand.Parameters.AddWithValue("PurchaseGroupEmail", item.PurchaseGroupEmail == null ? (object)DBNull.Value : item.PurchaseGroupEmail);
					sqlCommand.Parameters.AddWithValue("PurchaseGroupName", item.PurchaseGroupName == null ? (object)DBNull.Value : item.PurchaseGroupName);
					sqlCommand.Parameters.AddWithValue("PurchaseId", item.PurchaseId == null ? (object)DBNull.Value : item.PurchaseId);
					sqlCommand.Parameters.AddWithValue("PurchaseName", item.PurchaseName == null ? (object)DBNull.Value : item.PurchaseName);
					sqlCommand.Parameters.AddWithValue("PurchaseProfileId", item.PurchaseProfileId == null ? (object)DBNull.Value : item.PurchaseProfileId);
					sqlCommand.Parameters.AddWithValue("ReportDefaultLanguage", item.ReportDefaultLanguage == null ? (object)DBNull.Value : item.ReportDefaultLanguage);
					sqlCommand.Parameters.AddWithValue("ReportDefaultLanguageId", item.ReportDefaultLanguageId == null ? (object)DBNull.Value : item.ReportDefaultLanguageId);
					sqlCommand.Parameters.AddWithValue("Site", item.Site == null ? (object)DBNull.Value : item.Site);
					sqlCommand.Parameters.AddWithValue("SuperValidatorOneEmail", item.SuperValidatorOneEmail == null ? (object)DBNull.Value : item.SuperValidatorOneEmail);
					sqlCommand.Parameters.AddWithValue("SuperValidatorOneId", item.SuperValidatorOneId == null ? (object)DBNull.Value : item.SuperValidatorOneId);
					sqlCommand.Parameters.AddWithValue("SuperValidatorOneName", item.SuperValidatorOneName == null ? (object)DBNull.Value : item.SuperValidatorOneName);
					sqlCommand.Parameters.AddWithValue("SuperValidatorTowEmail", item.SuperValidatorTowEmail == null ? (object)DBNull.Value : item.SuperValidatorTowEmail);
					sqlCommand.Parameters.AddWithValue("SuperValidatorTowId", item.SuperValidatorTowId == null ? (object)DBNull.Value : item.SuperValidatorTowId);
					sqlCommand.Parameters.AddWithValue("SuperValidatorTowName", item.SuperValidatorTowName == null ? (object)DBNull.Value : item.SuperValidatorTowName);
					sqlCommand.Parameters.AddWithValue("SWIFT_BIC1", item.SWIFT_BIC1 == null ? (object)DBNull.Value : item.SWIFT_BIC1);
					sqlCommand.Parameters.AddWithValue("SWIFT_BIC2", item.SWIFT_BIC2 == null ? (object)DBNull.Value : item.SWIFT_BIC2);
					sqlCommand.Parameters.AddWithValue("SWIFT_BIC3", item.SWIFT_BIC3 == null ? (object)DBNull.Value : item.SWIFT_BIC3);
					sqlCommand.Parameters.AddWithValue("SWIFT_BIC4", item.SWIFT_BIC4 == null ? (object)DBNull.Value : item.SWIFT_BIC4);
					sqlCommand.Parameters.AddWithValue("TaxNumberID", item.TaxNumberID == null ? (object)DBNull.Value : item.TaxNumberID);
					sqlCommand.Parameters.AddWithValue("VATNumberID", item.VATNumberID == null ? (object)DBNull.Value : item.VATNumberID);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 64; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity> items)
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
						query += " INSERT INTO [__FNC_CompanyExtension] ([Account1],[Account2],[Account3],[Account4],[Address],[BankDetails1],[BankDetails2],[BankDetails3],[BankDetails4],[BLZ1],[BLZ2],[BLZ3],[BLZ4],[City],[CompanyId],[CompanyName],[Country],[CustomsNumber],[DefaultCurrencyId],[DefaultCurrencyName],[DeliveryAddress],[Email],[Fax],[FinanceProfileEmail],[FinanceProfileId],[FinanceProfileName],[FinanceValidatorOneEmail],[FinanceValidatorOneId],[FinanceValidatorOneName],[FinanceValidatorTowEmail],[FinanceValidatorTowId],[FinanceValidatorTowName],[HRB],[IBAN1],[IBAN2],[IBAN3],[IBAN4],[Manager1],[Manager2],[Manager3],[OrderPrefix],[Phone],[PostalCode],[PurchaseGroupEmail],[PurchaseGroupName],[PurchaseId],[PurchaseName],[PurchaseProfileId],[ReportDefaultLanguage],[ReportDefaultLanguageId],[Site],[SuperValidatorOneEmail],[SuperValidatorOneId],[SuperValidatorOneName],[SuperValidatorTowEmail],[SuperValidatorTowId],[SuperValidatorTowName],[SWIFT_BIC1],[SWIFT_BIC2],[SWIFT_BIC3],[SWIFT_BIC4],[TaxNumberID],[VATNumberID]) VALUES ( "

							+ "@Account1" + i + ","
							+ "@Account2" + i + ","
							+ "@Account3" + i + ","
							+ "@Account4" + i + ","
							+ "@Address" + i + ","
							+ "@BankDetails1" + i + ","
							+ "@BankDetails2" + i + ","
							+ "@BankDetails3" + i + ","
							+ "@BankDetails4" + i + ","
							+ "@BLZ1" + i + ","
							+ "@BLZ2" + i + ","
							+ "@BLZ3" + i + ","
							+ "@BLZ4" + i + ","
							+ "@City" + i + ","
							+ "@CompanyId" + i + ","
							+ "@CompanyName" + i + ","
							+ "@Country" + i + ","
							+ "@CustomsNumber" + i + ","
							+ "@DefaultCurrencyId" + i + ","
							+ "@DefaultCurrencyName" + i + ","
							+ "@DeliveryAddress" + i + ","
							+ "@Email" + i + ","
							+ "@Fax" + i + ","
							+ "@FinanceProfileEmail" + i + ","
							+ "@FinanceProfileId" + i + ","
							+ "@FinanceProfileName" + i + ","
							+ "@FinanceValidatorOneEmail" + i + ","
							+ "@FinanceValidatorOneId" + i + ","
							+ "@FinanceValidatorOneName" + i + ","
							+ "@FinanceValidatorTowEmail" + i + ","
							+ "@FinanceValidatorTowId" + i + ","
							+ "@FinanceValidatorTowName" + i + ","
							+ "@HRB" + i + ","
							+ "@IBAN1" + i + ","
							+ "@IBAN2" + i + ","
							+ "@IBAN3" + i + ","
							+ "@IBAN4" + i + ","
							+ "@Manager1" + i + ","
							+ "@Manager2" + i + ","
							+ "@Manager3" + i + ","
							+ "@OrderPrefix" + i + ","
							+ "@Phone" + i + ","
							+ "@PostalCode" + i + ","
							+ "@PurchaseGroupEmail" + i + ","
							+ "@PurchaseGroupName" + i + ","
							+ "@PurchaseId" + i + ","
							+ "@PurchaseName" + i + ","
							+ "@PurchaseProfileId" + i + ","
							+ "@ReportDefaultLanguage" + i + ","
							+ "@ReportDefaultLanguageId" + i + ","
							+ "@Site" + i + ","
							+ "@SuperValidatorOneEmail" + i + ","
							+ "@SuperValidatorOneId" + i + ","
							+ "@SuperValidatorOneName" + i + ","
							+ "@SuperValidatorTowEmail" + i + ","
							+ "@SuperValidatorTowId" + i + ","
							+ "@SuperValidatorTowName" + i + ","
							+ "@SWIFT_BIC1" + i + ","
							+ "@SWIFT_BIC2" + i + ","
							+ "@SWIFT_BIC3" + i + ","
							+ "@SWIFT_BIC4" + i + ","
							+ "@TaxNumberID" + i + ","
							+ "@VATNumberID" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Account1" + i, item.Account1 == null ? (object)DBNull.Value : item.Account1);
						sqlCommand.Parameters.AddWithValue("Account2" + i, item.Account2 == null ? (object)DBNull.Value : item.Account2);
						sqlCommand.Parameters.AddWithValue("Account3" + i, item.Account3 == null ? (object)DBNull.Value : item.Account3);
						sqlCommand.Parameters.AddWithValue("Account4" + i, item.Account4 == null ? (object)DBNull.Value : item.Account4);
						sqlCommand.Parameters.AddWithValue("Address" + i, item.Address == null ? (object)DBNull.Value : item.Address);
						sqlCommand.Parameters.AddWithValue("BankDetails1" + i, item.BankDetails1 == null ? (object)DBNull.Value : item.BankDetails1);
						sqlCommand.Parameters.AddWithValue("BankDetails2" + i, item.BankDetails2 == null ? (object)DBNull.Value : item.BankDetails2);
						sqlCommand.Parameters.AddWithValue("BankDetails3" + i, item.BankDetails3 == null ? (object)DBNull.Value : item.BankDetails3);
						sqlCommand.Parameters.AddWithValue("BankDetails4" + i, item.BankDetails4 == null ? (object)DBNull.Value : item.BankDetails4);
						sqlCommand.Parameters.AddWithValue("BLZ1" + i, item.BLZ1 == null ? (object)DBNull.Value : item.BLZ1);
						sqlCommand.Parameters.AddWithValue("BLZ2" + i, item.BLZ2 == null ? (object)DBNull.Value : item.BLZ2);
						sqlCommand.Parameters.AddWithValue("BLZ3" + i, item.BLZ3 == null ? (object)DBNull.Value : item.BLZ3);
						sqlCommand.Parameters.AddWithValue("BLZ4" + i, item.BLZ4 == null ? (object)DBNull.Value : item.BLZ4);
						sqlCommand.Parameters.AddWithValue("City" + i, item.City == null ? (object)DBNull.Value : item.City);
						sqlCommand.Parameters.AddWithValue("CompanyId" + i, item.CompanyId);
						sqlCommand.Parameters.AddWithValue("CompanyName" + i, item.CompanyName == null ? (object)DBNull.Value : item.CompanyName);
						sqlCommand.Parameters.AddWithValue("Country" + i, item.Country == null ? (object)DBNull.Value : item.Country);
						sqlCommand.Parameters.AddWithValue("CustomsNumber" + i, item.CustomsNumber == null ? (object)DBNull.Value : item.CustomsNumber);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyId" + i, item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyName" + i, item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
						sqlCommand.Parameters.AddWithValue("DeliveryAddress" + i, item.DeliveryAddress == null ? (object)DBNull.Value : item.DeliveryAddress);
						sqlCommand.Parameters.AddWithValue("Email" + i, item.Email == null ? (object)DBNull.Value : item.Email);
						sqlCommand.Parameters.AddWithValue("Fax" + i, item.Fax == null ? (object)DBNull.Value : item.Fax);
						sqlCommand.Parameters.AddWithValue("FinanceProfileEmail" + i, item.FinanceProfileEmail == null ? (object)DBNull.Value : item.FinanceProfileEmail);
						sqlCommand.Parameters.AddWithValue("FinanceProfileId" + i, item.FinanceProfileId == null ? (object)DBNull.Value : item.FinanceProfileId);
						sqlCommand.Parameters.AddWithValue("FinanceProfileName" + i, item.FinanceProfileName == null ? (object)DBNull.Value : item.FinanceProfileName);
						sqlCommand.Parameters.AddWithValue("FinanceValidatorOneEmail" + i, item.FinanceValidatorOneEmail == null ? (object)DBNull.Value : item.FinanceValidatorOneEmail);
						sqlCommand.Parameters.AddWithValue("FinanceValidatorOneId" + i, item.FinanceValidatorOneId == null ? (object)DBNull.Value : item.FinanceValidatorOneId);
						sqlCommand.Parameters.AddWithValue("FinanceValidatorOneName" + i, item.FinanceValidatorOneName == null ? (object)DBNull.Value : item.FinanceValidatorOneName);
						sqlCommand.Parameters.AddWithValue("FinanceValidatorTowEmail" + i, item.FinanceValidatorTowEmail == null ? (object)DBNull.Value : item.FinanceValidatorTowEmail);
						sqlCommand.Parameters.AddWithValue("FinanceValidatorTowId" + i, item.FinanceValidatorTowId == null ? (object)DBNull.Value : item.FinanceValidatorTowId);
						sqlCommand.Parameters.AddWithValue("FinanceValidatorTowName" + i, item.FinanceValidatorTowName == null ? (object)DBNull.Value : item.FinanceValidatorTowName);
						sqlCommand.Parameters.AddWithValue("HRB" + i, item.HRB == null ? (object)DBNull.Value : item.HRB);
						sqlCommand.Parameters.AddWithValue("IBAN1" + i, item.IBAN1 == null ? (object)DBNull.Value : item.IBAN1);
						sqlCommand.Parameters.AddWithValue("IBAN2" + i, item.IBAN2 == null ? (object)DBNull.Value : item.IBAN2);
						sqlCommand.Parameters.AddWithValue("IBAN3" + i, item.IBAN3 == null ? (object)DBNull.Value : item.IBAN3);
						sqlCommand.Parameters.AddWithValue("IBAN4" + i, item.IBAN4 == null ? (object)DBNull.Value : item.IBAN4);
						sqlCommand.Parameters.AddWithValue("Manager1" + i, item.Manager1 == null ? (object)DBNull.Value : item.Manager1);
						sqlCommand.Parameters.AddWithValue("Manager2" + i, item.Manager2 == null ? (object)DBNull.Value : item.Manager2);
						sqlCommand.Parameters.AddWithValue("Manager3" + i, item.Manager3 == null ? (object)DBNull.Value : item.Manager3);
						sqlCommand.Parameters.AddWithValue("OrderPrefix" + i, item.OrderPrefix == null ? (object)DBNull.Value : item.OrderPrefix);
						sqlCommand.Parameters.AddWithValue("Phone" + i, item.Phone == null ? (object)DBNull.Value : item.Phone);
						sqlCommand.Parameters.AddWithValue("PostalCode" + i, item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
						sqlCommand.Parameters.AddWithValue("PurchaseGroupEmail" + i, item.PurchaseGroupEmail == null ? (object)DBNull.Value : item.PurchaseGroupEmail);
						sqlCommand.Parameters.AddWithValue("PurchaseGroupName" + i, item.PurchaseGroupName == null ? (object)DBNull.Value : item.PurchaseGroupName);
						sqlCommand.Parameters.AddWithValue("PurchaseId" + i, item.PurchaseId == null ? (object)DBNull.Value : item.PurchaseId);
						sqlCommand.Parameters.AddWithValue("PurchaseName" + i, item.PurchaseName == null ? (object)DBNull.Value : item.PurchaseName);
						sqlCommand.Parameters.AddWithValue("PurchaseProfileId" + i, item.PurchaseProfileId == null ? (object)DBNull.Value : item.PurchaseProfileId);
						sqlCommand.Parameters.AddWithValue("ReportDefaultLanguage" + i, item.ReportDefaultLanguage == null ? (object)DBNull.Value : item.ReportDefaultLanguage);
						sqlCommand.Parameters.AddWithValue("ReportDefaultLanguageId" + i, item.ReportDefaultLanguageId == null ? (object)DBNull.Value : item.ReportDefaultLanguageId);
						sqlCommand.Parameters.AddWithValue("Site" + i, item.Site == null ? (object)DBNull.Value : item.Site);
						sqlCommand.Parameters.AddWithValue("SuperValidatorOneEmail" + i, item.SuperValidatorOneEmail == null ? (object)DBNull.Value : item.SuperValidatorOneEmail);
						sqlCommand.Parameters.AddWithValue("SuperValidatorOneId" + i, item.SuperValidatorOneId == null ? (object)DBNull.Value : item.SuperValidatorOneId);
						sqlCommand.Parameters.AddWithValue("SuperValidatorOneName" + i, item.SuperValidatorOneName == null ? (object)DBNull.Value : item.SuperValidatorOneName);
						sqlCommand.Parameters.AddWithValue("SuperValidatorTowEmail" + i, item.SuperValidatorTowEmail == null ? (object)DBNull.Value : item.SuperValidatorTowEmail);
						sqlCommand.Parameters.AddWithValue("SuperValidatorTowId" + i, item.SuperValidatorTowId == null ? (object)DBNull.Value : item.SuperValidatorTowId);
						sqlCommand.Parameters.AddWithValue("SuperValidatorTowName" + i, item.SuperValidatorTowName == null ? (object)DBNull.Value : item.SuperValidatorTowName);
						sqlCommand.Parameters.AddWithValue("SWIFT_BIC1" + i, item.SWIFT_BIC1 == null ? (object)DBNull.Value : item.SWIFT_BIC1);
						sqlCommand.Parameters.AddWithValue("SWIFT_BIC2" + i, item.SWIFT_BIC2 == null ? (object)DBNull.Value : item.SWIFT_BIC2);
						sqlCommand.Parameters.AddWithValue("SWIFT_BIC3" + i, item.SWIFT_BIC3 == null ? (object)DBNull.Value : item.SWIFT_BIC3);
						sqlCommand.Parameters.AddWithValue("SWIFT_BIC4" + i, item.SWIFT_BIC4 == null ? (object)DBNull.Value : item.SWIFT_BIC4);
						sqlCommand.Parameters.AddWithValue("TaxNumberID" + i, item.TaxNumberID == null ? (object)DBNull.Value : item.TaxNumberID);
						sqlCommand.Parameters.AddWithValue("VATNumberID" + i, item.VATNumberID == null ? (object)DBNull.Value : item.VATNumberID);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_CompanyExtension] SET [Account1]=@Account1, [Account2]=@Account2, [Account3]=@Account3, [Account4]=@Account4, [Address]=@Address, [BankDetails1]=@BankDetails1, [BankDetails2]=@BankDetails2, [BankDetails3]=@BankDetails3, [BankDetails4]=@BankDetails4, [BLZ1]=@BLZ1, [BLZ2]=@BLZ2, [BLZ3]=@BLZ3, [BLZ4]=@BLZ4, [City]=@City, [CompanyId]=@CompanyId, [CompanyName]=@CompanyName, [Country]=@Country, [CustomsNumber]=@CustomsNumber, [DefaultCurrencyId]=@DefaultCurrencyId, [DefaultCurrencyName]=@DefaultCurrencyName, [DeliveryAddress]=@DeliveryAddress, [Email]=@Email, [Fax]=@Fax, [FinanceProfileEmail]=@FinanceProfileEmail, [FinanceProfileId]=@FinanceProfileId, [FinanceProfileName]=@FinanceProfileName, [FinanceValidatorOneEmail]=@FinanceValidatorOneEmail, [FinanceValidatorOneId]=@FinanceValidatorOneId, [FinanceValidatorOneName]=@FinanceValidatorOneName, [FinanceValidatorTowEmail]=@FinanceValidatorTowEmail, [FinanceValidatorTowId]=@FinanceValidatorTowId, [FinanceValidatorTowName]=@FinanceValidatorTowName, [HRB]=@HRB, [IBAN1]=@IBAN1, [IBAN2]=@IBAN2, [IBAN3]=@IBAN3, [IBAN4]=@IBAN4, [Manager1]=@Manager1, [Manager2]=@Manager2, [Manager3]=@Manager3, [OrderPrefix]=@OrderPrefix, [Phone]=@Phone, [PostalCode]=@PostalCode, [PurchaseGroupEmail]=@PurchaseGroupEmail, [PurchaseGroupName]=@PurchaseGroupName, [PurchaseId]=@PurchaseId, [PurchaseName]=@PurchaseName, [PurchaseProfileId]=@PurchaseProfileId, [ReportDefaultLanguage]=@ReportDefaultLanguage, [ReportDefaultLanguageId]=@ReportDefaultLanguageId, [Site]=@Site, [SuperValidatorOneEmail]=@SuperValidatorOneEmail, [SuperValidatorOneId]=@SuperValidatorOneId, [SuperValidatorOneName]=@SuperValidatorOneName, [SuperValidatorTowEmail]=@SuperValidatorTowEmail, [SuperValidatorTowId]=@SuperValidatorTowId, [SuperValidatorTowName]=@SuperValidatorTowName, [SWIFT_BIC1]=@SWIFT_BIC1, [SWIFT_BIC2]=@SWIFT_BIC2, [SWIFT_BIC3]=@SWIFT_BIC3, [SWIFT_BIC4]=@SWIFT_BIC4, [TaxNumberID]=@TaxNumberID, [VATNumberID]=@VATNumberID WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Account1", item.Account1 == null ? (object)DBNull.Value : item.Account1);
				sqlCommand.Parameters.AddWithValue("Account2", item.Account2 == null ? (object)DBNull.Value : item.Account2);
				sqlCommand.Parameters.AddWithValue("Account3", item.Account3 == null ? (object)DBNull.Value : item.Account3);
				sqlCommand.Parameters.AddWithValue("Account4", item.Account4 == null ? (object)DBNull.Value : item.Account4);
				sqlCommand.Parameters.AddWithValue("Address", item.Address == null ? (object)DBNull.Value : item.Address);
				sqlCommand.Parameters.AddWithValue("BankDetails1", item.BankDetails1 == null ? (object)DBNull.Value : item.BankDetails1);
				sqlCommand.Parameters.AddWithValue("BankDetails2", item.BankDetails2 == null ? (object)DBNull.Value : item.BankDetails2);
				sqlCommand.Parameters.AddWithValue("BankDetails3", item.BankDetails3 == null ? (object)DBNull.Value : item.BankDetails3);
				sqlCommand.Parameters.AddWithValue("BankDetails4", item.BankDetails4 == null ? (object)DBNull.Value : item.BankDetails4);
				sqlCommand.Parameters.AddWithValue("BLZ1", item.BLZ1 == null ? (object)DBNull.Value : item.BLZ1);
				sqlCommand.Parameters.AddWithValue("BLZ2", item.BLZ2 == null ? (object)DBNull.Value : item.BLZ2);
				sqlCommand.Parameters.AddWithValue("BLZ3", item.BLZ3 == null ? (object)DBNull.Value : item.BLZ3);
				sqlCommand.Parameters.AddWithValue("BLZ4", item.BLZ4 == null ? (object)DBNull.Value : item.BLZ4);
				sqlCommand.Parameters.AddWithValue("City", item.City == null ? (object)DBNull.Value : item.City);
				sqlCommand.Parameters.AddWithValue("CompanyId", item.CompanyId);
				sqlCommand.Parameters.AddWithValue("CompanyName", item.CompanyName == null ? (object)DBNull.Value : item.CompanyName);
				sqlCommand.Parameters.AddWithValue("Country", item.Country == null ? (object)DBNull.Value : item.Country);
				sqlCommand.Parameters.AddWithValue("CustomsNumber", item.CustomsNumber == null ? (object)DBNull.Value : item.CustomsNumber);
				sqlCommand.Parameters.AddWithValue("DefaultCurrencyId", item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
				sqlCommand.Parameters.AddWithValue("DefaultCurrencyName", item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
				sqlCommand.Parameters.AddWithValue("DeliveryAddress", item.DeliveryAddress == null ? (object)DBNull.Value : item.DeliveryAddress);
				sqlCommand.Parameters.AddWithValue("Email", item.Email == null ? (object)DBNull.Value : item.Email);
				sqlCommand.Parameters.AddWithValue("Fax", item.Fax == null ? (object)DBNull.Value : item.Fax);
				sqlCommand.Parameters.AddWithValue("FinanceProfileEmail", item.FinanceProfileEmail == null ? (object)DBNull.Value : item.FinanceProfileEmail);
				sqlCommand.Parameters.AddWithValue("FinanceProfileId", item.FinanceProfileId == null ? (object)DBNull.Value : item.FinanceProfileId);
				sqlCommand.Parameters.AddWithValue("FinanceProfileName", item.FinanceProfileName == null ? (object)DBNull.Value : item.FinanceProfileName);
				sqlCommand.Parameters.AddWithValue("FinanceValidatorOneEmail", item.FinanceValidatorOneEmail == null ? (object)DBNull.Value : item.FinanceValidatorOneEmail);
				sqlCommand.Parameters.AddWithValue("FinanceValidatorOneId", item.FinanceValidatorOneId == null ? (object)DBNull.Value : item.FinanceValidatorOneId);
				sqlCommand.Parameters.AddWithValue("FinanceValidatorOneName", item.FinanceValidatorOneName == null ? (object)DBNull.Value : item.FinanceValidatorOneName);
				sqlCommand.Parameters.AddWithValue("FinanceValidatorTowEmail", item.FinanceValidatorTowEmail == null ? (object)DBNull.Value : item.FinanceValidatorTowEmail);
				sqlCommand.Parameters.AddWithValue("FinanceValidatorTowId", item.FinanceValidatorTowId == null ? (object)DBNull.Value : item.FinanceValidatorTowId);
				sqlCommand.Parameters.AddWithValue("FinanceValidatorTowName", item.FinanceValidatorTowName == null ? (object)DBNull.Value : item.FinanceValidatorTowName);
				sqlCommand.Parameters.AddWithValue("HRB", item.HRB == null ? (object)DBNull.Value : item.HRB);
				sqlCommand.Parameters.AddWithValue("IBAN1", item.IBAN1 == null ? (object)DBNull.Value : item.IBAN1);
				sqlCommand.Parameters.AddWithValue("IBAN2", item.IBAN2 == null ? (object)DBNull.Value : item.IBAN2);
				sqlCommand.Parameters.AddWithValue("IBAN3", item.IBAN3 == null ? (object)DBNull.Value : item.IBAN3);
				sqlCommand.Parameters.AddWithValue("IBAN4", item.IBAN4 == null ? (object)DBNull.Value : item.IBAN4);
				sqlCommand.Parameters.AddWithValue("Manager1", item.Manager1 == null ? (object)DBNull.Value : item.Manager1);
				sqlCommand.Parameters.AddWithValue("Manager2", item.Manager2 == null ? (object)DBNull.Value : item.Manager2);
				sqlCommand.Parameters.AddWithValue("Manager3", item.Manager3 == null ? (object)DBNull.Value : item.Manager3);
				sqlCommand.Parameters.AddWithValue("OrderPrefix", item.OrderPrefix == null ? (object)DBNull.Value : item.OrderPrefix);
				sqlCommand.Parameters.AddWithValue("Phone", item.Phone == null ? (object)DBNull.Value : item.Phone);
				sqlCommand.Parameters.AddWithValue("PostalCode", item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
				sqlCommand.Parameters.AddWithValue("PurchaseGroupEmail", item.PurchaseGroupEmail == null ? (object)DBNull.Value : item.PurchaseGroupEmail);
				sqlCommand.Parameters.AddWithValue("PurchaseGroupName", item.PurchaseGroupName == null ? (object)DBNull.Value : item.PurchaseGroupName);
				sqlCommand.Parameters.AddWithValue("PurchaseId", item.PurchaseId == null ? (object)DBNull.Value : item.PurchaseId);
				sqlCommand.Parameters.AddWithValue("PurchaseName", item.PurchaseName == null ? (object)DBNull.Value : item.PurchaseName);
				sqlCommand.Parameters.AddWithValue("PurchaseProfileId", item.PurchaseProfileId == null ? (object)DBNull.Value : item.PurchaseProfileId);
				sqlCommand.Parameters.AddWithValue("ReportDefaultLanguage", item.ReportDefaultLanguage == null ? (object)DBNull.Value : item.ReportDefaultLanguage);
				sqlCommand.Parameters.AddWithValue("ReportDefaultLanguageId", item.ReportDefaultLanguageId == null ? (object)DBNull.Value : item.ReportDefaultLanguageId);
				sqlCommand.Parameters.AddWithValue("Site", item.Site == null ? (object)DBNull.Value : item.Site);
				sqlCommand.Parameters.AddWithValue("SuperValidatorOneEmail", item.SuperValidatorOneEmail == null ? (object)DBNull.Value : item.SuperValidatorOneEmail);
				sqlCommand.Parameters.AddWithValue("SuperValidatorOneId", item.SuperValidatorOneId == null ? (object)DBNull.Value : item.SuperValidatorOneId);
				sqlCommand.Parameters.AddWithValue("SuperValidatorOneName", item.SuperValidatorOneName == null ? (object)DBNull.Value : item.SuperValidatorOneName);
				sqlCommand.Parameters.AddWithValue("SuperValidatorTowEmail", item.SuperValidatorTowEmail == null ? (object)DBNull.Value : item.SuperValidatorTowEmail);
				sqlCommand.Parameters.AddWithValue("SuperValidatorTowId", item.SuperValidatorTowId == null ? (object)DBNull.Value : item.SuperValidatorTowId);
				sqlCommand.Parameters.AddWithValue("SuperValidatorTowName", item.SuperValidatorTowName == null ? (object)DBNull.Value : item.SuperValidatorTowName);
				sqlCommand.Parameters.AddWithValue("SWIFT_BIC1", item.SWIFT_BIC1 == null ? (object)DBNull.Value : item.SWIFT_BIC1);
				sqlCommand.Parameters.AddWithValue("SWIFT_BIC2", item.SWIFT_BIC2 == null ? (object)DBNull.Value : item.SWIFT_BIC2);
				sqlCommand.Parameters.AddWithValue("SWIFT_BIC3", item.SWIFT_BIC3 == null ? (object)DBNull.Value : item.SWIFT_BIC3);
				sqlCommand.Parameters.AddWithValue("SWIFT_BIC4", item.SWIFT_BIC4 == null ? (object)DBNull.Value : item.SWIFT_BIC4);
				sqlCommand.Parameters.AddWithValue("TaxNumberID", item.TaxNumberID == null ? (object)DBNull.Value : item.TaxNumberID);
				sqlCommand.Parameters.AddWithValue("VATNumberID", item.VATNumberID == null ? (object)DBNull.Value : item.VATNumberID);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 64; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity> items)
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
						query += " UPDATE [__FNC_CompanyExtension] SET "

							+ "[Account1]=@Account1" + i + ","
							+ "[Account2]=@Account2" + i + ","
							+ "[Account3]=@Account3" + i + ","
							+ "[Account4]=@Account4" + i + ","
							+ "[Address]=@Address" + i + ","
							+ "[BankDetails1]=@BankDetails1" + i + ","
							+ "[BankDetails2]=@BankDetails2" + i + ","
							+ "[BankDetails3]=@BankDetails3" + i + ","
							+ "[BankDetails4]=@BankDetails4" + i + ","
							+ "[BLZ1]=@BLZ1" + i + ","
							+ "[BLZ2]=@BLZ2" + i + ","
							+ "[BLZ3]=@BLZ3" + i + ","
							+ "[BLZ4]=@BLZ4" + i + ","
							+ "[City]=@City" + i + ","
							+ "[CompanyId]=@CompanyId" + i + ","
							+ "[CompanyName]=@CompanyName" + i + ","
							+ "[Country]=@Country" + i + ","
							+ "[CustomsNumber]=@CustomsNumber" + i + ","
							+ "[DefaultCurrencyId]=@DefaultCurrencyId" + i + ","
							+ "[DefaultCurrencyName]=@DefaultCurrencyName" + i + ","
							+ "[DeliveryAddress]=@DeliveryAddress" + i + ","
							+ "[Email]=@Email" + i + ","
							+ "[Fax]=@Fax" + i + ","
							+ "[FinanceProfileEmail]=@FinanceProfileEmail" + i + ","
							+ "[FinanceProfileId]=@FinanceProfileId" + i + ","
							+ "[FinanceProfileName]=@FinanceProfileName" + i + ","
							+ "[FinanceValidatorOneEmail]=@FinanceValidatorOneEmail" + i + ","
							+ "[FinanceValidatorOneId]=@FinanceValidatorOneId" + i + ","
							+ "[FinanceValidatorOneName]=@FinanceValidatorOneName" + i + ","
							+ "[FinanceValidatorTowEmail]=@FinanceValidatorTowEmail" + i + ","
							+ "[FinanceValidatorTowId]=@FinanceValidatorTowId" + i + ","
							+ "[FinanceValidatorTowName]=@FinanceValidatorTowName" + i + ","
							+ "[HRB]=@HRB" + i + ","
							+ "[IBAN1]=@IBAN1" + i + ","
							+ "[IBAN2]=@IBAN2" + i + ","
							+ "[IBAN3]=@IBAN3" + i + ","
							+ "[IBAN4]=@IBAN4" + i + ","
							+ "[Manager1]=@Manager1" + i + ","
							+ "[Manager2]=@Manager2" + i + ","
							+ "[Manager3]=@Manager3" + i + ","
							+ "[OrderPrefix]=@OrderPrefix" + i + ","
							+ "[Phone]=@Phone" + i + ","
							+ "[PostalCode]=@PostalCode" + i + ","
							+ "[PurchaseGroupEmail]=@PurchaseGroupEmail" + i + ","
							+ "[PurchaseGroupName]=@PurchaseGroupName" + i + ","
							+ "[PurchaseId]=@PurchaseId" + i + ","
							+ "[PurchaseName]=@PurchaseName" + i + ","
							+ "[PurchaseProfileId]=@PurchaseProfileId" + i + ","
							+ "[ReportDefaultLanguage]=@ReportDefaultLanguage" + i + ","
							+ "[ReportDefaultLanguageId]=@ReportDefaultLanguageId" + i + ","
							+ "[Site]=@Site" + i + ","
							+ "[SuperValidatorOneEmail]=@SuperValidatorOneEmail" + i + ","
							+ "[SuperValidatorOneId]=@SuperValidatorOneId" + i + ","
							+ "[SuperValidatorOneName]=@SuperValidatorOneName" + i + ","
							+ "[SuperValidatorTowEmail]=@SuperValidatorTowEmail" + i + ","
							+ "[SuperValidatorTowId]=@SuperValidatorTowId" + i + ","
							+ "[SuperValidatorTowName]=@SuperValidatorTowName" + i + ","
							+ "[SWIFT_BIC1]=@SWIFT_BIC1" + i + ","
							+ "[SWIFT_BIC2]=@SWIFT_BIC2" + i + ","
							+ "[SWIFT_BIC3]=@SWIFT_BIC3" + i + ","
							+ "[SWIFT_BIC4]=@SWIFT_BIC4" + i + ","
							+ "[TaxNumberID]=@TaxNumberID" + i + ","
							+ "[VATNumberID]=@VATNumberID" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("Account1" + i, item.Account1 == null ? (object)DBNull.Value : item.Account1);
						sqlCommand.Parameters.AddWithValue("Account2" + i, item.Account2 == null ? (object)DBNull.Value : item.Account2);
						sqlCommand.Parameters.AddWithValue("Account3" + i, item.Account3 == null ? (object)DBNull.Value : item.Account3);
						sqlCommand.Parameters.AddWithValue("Account4" + i, item.Account4 == null ? (object)DBNull.Value : item.Account4);
						sqlCommand.Parameters.AddWithValue("Address" + i, item.Address == null ? (object)DBNull.Value : item.Address);
						sqlCommand.Parameters.AddWithValue("BankDetails1" + i, item.BankDetails1 == null ? (object)DBNull.Value : item.BankDetails1);
						sqlCommand.Parameters.AddWithValue("BankDetails2" + i, item.BankDetails2 == null ? (object)DBNull.Value : item.BankDetails2);
						sqlCommand.Parameters.AddWithValue("BankDetails3" + i, item.BankDetails3 == null ? (object)DBNull.Value : item.BankDetails3);
						sqlCommand.Parameters.AddWithValue("BankDetails4" + i, item.BankDetails4 == null ? (object)DBNull.Value : item.BankDetails4);
						sqlCommand.Parameters.AddWithValue("BLZ1" + i, item.BLZ1 == null ? (object)DBNull.Value : item.BLZ1);
						sqlCommand.Parameters.AddWithValue("BLZ2" + i, item.BLZ2 == null ? (object)DBNull.Value : item.BLZ2);
						sqlCommand.Parameters.AddWithValue("BLZ3" + i, item.BLZ3 == null ? (object)DBNull.Value : item.BLZ3);
						sqlCommand.Parameters.AddWithValue("BLZ4" + i, item.BLZ4 == null ? (object)DBNull.Value : item.BLZ4);
						sqlCommand.Parameters.AddWithValue("City" + i, item.City == null ? (object)DBNull.Value : item.City);
						sqlCommand.Parameters.AddWithValue("CompanyId" + i, item.CompanyId);
						sqlCommand.Parameters.AddWithValue("CompanyName" + i, item.CompanyName == null ? (object)DBNull.Value : item.CompanyName);
						sqlCommand.Parameters.AddWithValue("Country" + i, item.Country == null ? (object)DBNull.Value : item.Country);
						sqlCommand.Parameters.AddWithValue("CustomsNumber" + i, item.CustomsNumber == null ? (object)DBNull.Value : item.CustomsNumber);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyId" + i, item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyName" + i, item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
						sqlCommand.Parameters.AddWithValue("DeliveryAddress" + i, item.DeliveryAddress == null ? (object)DBNull.Value : item.DeliveryAddress);
						sqlCommand.Parameters.AddWithValue("Email" + i, item.Email == null ? (object)DBNull.Value : item.Email);
						sqlCommand.Parameters.AddWithValue("Fax" + i, item.Fax == null ? (object)DBNull.Value : item.Fax);
						sqlCommand.Parameters.AddWithValue("FinanceProfileEmail" + i, item.FinanceProfileEmail == null ? (object)DBNull.Value : item.FinanceProfileEmail);
						sqlCommand.Parameters.AddWithValue("FinanceProfileId" + i, item.FinanceProfileId == null ? (object)DBNull.Value : item.FinanceProfileId);
						sqlCommand.Parameters.AddWithValue("FinanceProfileName" + i, item.FinanceProfileName == null ? (object)DBNull.Value : item.FinanceProfileName);
						sqlCommand.Parameters.AddWithValue("FinanceValidatorOneEmail" + i, item.FinanceValidatorOneEmail == null ? (object)DBNull.Value : item.FinanceValidatorOneEmail);
						sqlCommand.Parameters.AddWithValue("FinanceValidatorOneId" + i, item.FinanceValidatorOneId == null ? (object)DBNull.Value : item.FinanceValidatorOneId);
						sqlCommand.Parameters.AddWithValue("FinanceValidatorOneName" + i, item.FinanceValidatorOneName == null ? (object)DBNull.Value : item.FinanceValidatorOneName);
						sqlCommand.Parameters.AddWithValue("FinanceValidatorTowEmail" + i, item.FinanceValidatorTowEmail == null ? (object)DBNull.Value : item.FinanceValidatorTowEmail);
						sqlCommand.Parameters.AddWithValue("FinanceValidatorTowId" + i, item.FinanceValidatorTowId == null ? (object)DBNull.Value : item.FinanceValidatorTowId);
						sqlCommand.Parameters.AddWithValue("FinanceValidatorTowName" + i, item.FinanceValidatorTowName == null ? (object)DBNull.Value : item.FinanceValidatorTowName);
						sqlCommand.Parameters.AddWithValue("HRB" + i, item.HRB == null ? (object)DBNull.Value : item.HRB);
						sqlCommand.Parameters.AddWithValue("IBAN1" + i, item.IBAN1 == null ? (object)DBNull.Value : item.IBAN1);
						sqlCommand.Parameters.AddWithValue("IBAN2" + i, item.IBAN2 == null ? (object)DBNull.Value : item.IBAN2);
						sqlCommand.Parameters.AddWithValue("IBAN3" + i, item.IBAN3 == null ? (object)DBNull.Value : item.IBAN3);
						sqlCommand.Parameters.AddWithValue("IBAN4" + i, item.IBAN4 == null ? (object)DBNull.Value : item.IBAN4);
						sqlCommand.Parameters.AddWithValue("Manager1" + i, item.Manager1 == null ? (object)DBNull.Value : item.Manager1);
						sqlCommand.Parameters.AddWithValue("Manager2" + i, item.Manager2 == null ? (object)DBNull.Value : item.Manager2);
						sqlCommand.Parameters.AddWithValue("Manager3" + i, item.Manager3 == null ? (object)DBNull.Value : item.Manager3);
						sqlCommand.Parameters.AddWithValue("OrderPrefix" + i, item.OrderPrefix == null ? (object)DBNull.Value : item.OrderPrefix);
						sqlCommand.Parameters.AddWithValue("Phone" + i, item.Phone == null ? (object)DBNull.Value : item.Phone);
						sqlCommand.Parameters.AddWithValue("PostalCode" + i, item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
						sqlCommand.Parameters.AddWithValue("PurchaseGroupEmail" + i, item.PurchaseGroupEmail == null ? (object)DBNull.Value : item.PurchaseGroupEmail);
						sqlCommand.Parameters.AddWithValue("PurchaseGroupName" + i, item.PurchaseGroupName == null ? (object)DBNull.Value : item.PurchaseGroupName);
						sqlCommand.Parameters.AddWithValue("PurchaseId" + i, item.PurchaseId == null ? (object)DBNull.Value : item.PurchaseId);
						sqlCommand.Parameters.AddWithValue("PurchaseName" + i, item.PurchaseName == null ? (object)DBNull.Value : item.PurchaseName);
						sqlCommand.Parameters.AddWithValue("PurchaseProfileId" + i, item.PurchaseProfileId == null ? (object)DBNull.Value : item.PurchaseProfileId);
						sqlCommand.Parameters.AddWithValue("ReportDefaultLanguage" + i, item.ReportDefaultLanguage == null ? (object)DBNull.Value : item.ReportDefaultLanguage);
						sqlCommand.Parameters.AddWithValue("ReportDefaultLanguageId" + i, item.ReportDefaultLanguageId == null ? (object)DBNull.Value : item.ReportDefaultLanguageId);
						sqlCommand.Parameters.AddWithValue("Site" + i, item.Site == null ? (object)DBNull.Value : item.Site);
						sqlCommand.Parameters.AddWithValue("SuperValidatorOneEmail" + i, item.SuperValidatorOneEmail == null ? (object)DBNull.Value : item.SuperValidatorOneEmail);
						sqlCommand.Parameters.AddWithValue("SuperValidatorOneId" + i, item.SuperValidatorOneId == null ? (object)DBNull.Value : item.SuperValidatorOneId);
						sqlCommand.Parameters.AddWithValue("SuperValidatorOneName" + i, item.SuperValidatorOneName == null ? (object)DBNull.Value : item.SuperValidatorOneName);
						sqlCommand.Parameters.AddWithValue("SuperValidatorTowEmail" + i, item.SuperValidatorTowEmail == null ? (object)DBNull.Value : item.SuperValidatorTowEmail);
						sqlCommand.Parameters.AddWithValue("SuperValidatorTowId" + i, item.SuperValidatorTowId == null ? (object)DBNull.Value : item.SuperValidatorTowId);
						sqlCommand.Parameters.AddWithValue("SuperValidatorTowName" + i, item.SuperValidatorTowName == null ? (object)DBNull.Value : item.SuperValidatorTowName);
						sqlCommand.Parameters.AddWithValue("SWIFT_BIC1" + i, item.SWIFT_BIC1 == null ? (object)DBNull.Value : item.SWIFT_BIC1);
						sqlCommand.Parameters.AddWithValue("SWIFT_BIC2" + i, item.SWIFT_BIC2 == null ? (object)DBNull.Value : item.SWIFT_BIC2);
						sqlCommand.Parameters.AddWithValue("SWIFT_BIC3" + i, item.SWIFT_BIC3 == null ? (object)DBNull.Value : item.SWIFT_BIC3);
						sqlCommand.Parameters.AddWithValue("SWIFT_BIC4" + i, item.SWIFT_BIC4 == null ? (object)DBNull.Value : item.SWIFT_BIC4);
						sqlCommand.Parameters.AddWithValue("TaxNumberID" + i, item.TaxNumberID == null ? (object)DBNull.Value : item.TaxNumberID);
						sqlCommand.Parameters.AddWithValue("VATNumberID" + i, item.VATNumberID == null ? (object)DBNull.Value : item.VATNumberID);
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
				string query = "DELETE FROM [__FNC_CompanyExtension] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__FNC_CompanyExtension] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__FNC_CompanyExtension] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__FNC_CompanyExtension]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__FNC_CompanyExtension] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [__FNC_CompanyExtension] ([Account1],[Account2],[Account3],[Account4],[Address],[BankDetails1],[BankDetails2],[BankDetails3],[BankDetails4],[BLZ1],[BLZ2],[BLZ3],[BLZ4],[City],[CompanyId],[CompanyName],[Country],[CustomsNumber],[DefaultCurrencyId],[DefaultCurrencyName],[DeliveryAddress],[Email],[Fax],[FinanceProfileEmail],[FinanceProfileId],[FinanceProfileName],[FinanceValidatorOneEmail],[FinanceValidatorOneId],[FinanceValidatorOneName],[FinanceValidatorTowEmail],[FinanceValidatorTowId],[FinanceValidatorTowName],[HRB],[IBAN1],[IBAN2],[IBAN3],[IBAN4],[Manager1],[Manager2],[Manager3],[OrderPrefix],[Phone],[PostalCode],[PurchaseGroupEmail],[PurchaseGroupName],[PurchaseId],[PurchaseName],[PurchaseProfileId],[ReportDefaultLanguage],[ReportDefaultLanguageId],[Site],[SuperValidatorOneEmail],[SuperValidatorOneId],[SuperValidatorOneName],[SuperValidatorTowEmail],[SuperValidatorTowId],[SuperValidatorTowName],[SWIFT_BIC1],[SWIFT_BIC2],[SWIFT_BIC3],[SWIFT_BIC4],[TaxNumberID],[VATNumberID]) OUTPUT INSERTED.[Id] VALUES (@Account1,@Account2,@Account3,@Account4,@Address,@BankDetails1,@BankDetails2,@BankDetails3,@BankDetails4,@BLZ1,@BLZ2,@BLZ3,@BLZ4,@City,@CompanyId,@CompanyName,@Country,@CustomsNumber,@DefaultCurrencyId,@DefaultCurrencyName,@DeliveryAddress,@Email,@Fax,@FinanceProfileEmail,@FinanceProfileId,@FinanceProfileName,@FinanceValidatorOneEmail,@FinanceValidatorOneId,@FinanceValidatorOneName,@FinanceValidatorTowEmail,@FinanceValidatorTowId,@FinanceValidatorTowName,@HRB,@IBAN1,@IBAN2,@IBAN3,@IBAN4,@Manager1,@Manager2,@Manager3,@OrderPrefix,@Phone,@PostalCode,@PurchaseGroupEmail,@PurchaseGroupName,@PurchaseId,@PurchaseName,@PurchaseProfileId,@ReportDefaultLanguage,@ReportDefaultLanguageId,@Site,@SuperValidatorOneEmail,@SuperValidatorOneId,@SuperValidatorOneName,@SuperValidatorTowEmail,@SuperValidatorTowId,@SuperValidatorTowName,@SWIFT_BIC1,@SWIFT_BIC2,@SWIFT_BIC3,@SWIFT_BIC4,@TaxNumberID,@VATNumberID); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Account1", item.Account1 == null ? (object)DBNull.Value : item.Account1);
			sqlCommand.Parameters.AddWithValue("Account2", item.Account2 == null ? (object)DBNull.Value : item.Account2);
			sqlCommand.Parameters.AddWithValue("Account3", item.Account3 == null ? (object)DBNull.Value : item.Account3);
			sqlCommand.Parameters.AddWithValue("Account4", item.Account4 == null ? (object)DBNull.Value : item.Account4);
			sqlCommand.Parameters.AddWithValue("Address", item.Address == null ? (object)DBNull.Value : item.Address);
			sqlCommand.Parameters.AddWithValue("BankDetails1", item.BankDetails1 == null ? (object)DBNull.Value : item.BankDetails1);
			sqlCommand.Parameters.AddWithValue("BankDetails2", item.BankDetails2 == null ? (object)DBNull.Value : item.BankDetails2);
			sqlCommand.Parameters.AddWithValue("BankDetails3", item.BankDetails3 == null ? (object)DBNull.Value : item.BankDetails3);
			sqlCommand.Parameters.AddWithValue("BankDetails4", item.BankDetails4 == null ? (object)DBNull.Value : item.BankDetails4);
			sqlCommand.Parameters.AddWithValue("BLZ1", item.BLZ1 == null ? (object)DBNull.Value : item.BLZ1);
			sqlCommand.Parameters.AddWithValue("BLZ2", item.BLZ2 == null ? (object)DBNull.Value : item.BLZ2);
			sqlCommand.Parameters.AddWithValue("BLZ3", item.BLZ3 == null ? (object)DBNull.Value : item.BLZ3);
			sqlCommand.Parameters.AddWithValue("BLZ4", item.BLZ4 == null ? (object)DBNull.Value : item.BLZ4);
			sqlCommand.Parameters.AddWithValue("City", item.City == null ? (object)DBNull.Value : item.City);
			sqlCommand.Parameters.AddWithValue("CompanyId", item.CompanyId);
			sqlCommand.Parameters.AddWithValue("CompanyName", item.CompanyName == null ? (object)DBNull.Value : item.CompanyName);
			sqlCommand.Parameters.AddWithValue("Country", item.Country == null ? (object)DBNull.Value : item.Country);
			sqlCommand.Parameters.AddWithValue("CustomsNumber", item.CustomsNumber == null ? (object)DBNull.Value : item.CustomsNumber);
			sqlCommand.Parameters.AddWithValue("DefaultCurrencyId", item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
			sqlCommand.Parameters.AddWithValue("DefaultCurrencyName", item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
			sqlCommand.Parameters.AddWithValue("DeliveryAddress", item.DeliveryAddress == null ? (object)DBNull.Value : item.DeliveryAddress);
			sqlCommand.Parameters.AddWithValue("Email", item.Email == null ? (object)DBNull.Value : item.Email);
			sqlCommand.Parameters.AddWithValue("Fax", item.Fax == null ? (object)DBNull.Value : item.Fax);
			sqlCommand.Parameters.AddWithValue("FinanceProfileEmail", item.FinanceProfileEmail == null ? (object)DBNull.Value : item.FinanceProfileEmail);
			sqlCommand.Parameters.AddWithValue("FinanceProfileId", item.FinanceProfileId == null ? (object)DBNull.Value : item.FinanceProfileId);
			sqlCommand.Parameters.AddWithValue("FinanceProfileName", item.FinanceProfileName == null ? (object)DBNull.Value : item.FinanceProfileName);
			sqlCommand.Parameters.AddWithValue("FinanceValidatorOneEmail", item.FinanceValidatorOneEmail == null ? (object)DBNull.Value : item.FinanceValidatorOneEmail);
			sqlCommand.Parameters.AddWithValue("FinanceValidatorOneId", item.FinanceValidatorOneId == null ? (object)DBNull.Value : item.FinanceValidatorOneId);
			sqlCommand.Parameters.AddWithValue("FinanceValidatorOneName", item.FinanceValidatorOneName == null ? (object)DBNull.Value : item.FinanceValidatorOneName);
			sqlCommand.Parameters.AddWithValue("FinanceValidatorTowEmail", item.FinanceValidatorTowEmail == null ? (object)DBNull.Value : item.FinanceValidatorTowEmail);
			sqlCommand.Parameters.AddWithValue("FinanceValidatorTowId", item.FinanceValidatorTowId == null ? (object)DBNull.Value : item.FinanceValidatorTowId);
			sqlCommand.Parameters.AddWithValue("FinanceValidatorTowName", item.FinanceValidatorTowName == null ? (object)DBNull.Value : item.FinanceValidatorTowName);
			sqlCommand.Parameters.AddWithValue("HRB", item.HRB == null ? (object)DBNull.Value : item.HRB);
			sqlCommand.Parameters.AddWithValue("IBAN1", item.IBAN1 == null ? (object)DBNull.Value : item.IBAN1);
			sqlCommand.Parameters.AddWithValue("IBAN2", item.IBAN2 == null ? (object)DBNull.Value : item.IBAN2);
			sqlCommand.Parameters.AddWithValue("IBAN3", item.IBAN3 == null ? (object)DBNull.Value : item.IBAN3);
			sqlCommand.Parameters.AddWithValue("IBAN4", item.IBAN4 == null ? (object)DBNull.Value : item.IBAN4);
			sqlCommand.Parameters.AddWithValue("Manager1", item.Manager1 == null ? (object)DBNull.Value : item.Manager1);
			sqlCommand.Parameters.AddWithValue("Manager2", item.Manager2 == null ? (object)DBNull.Value : item.Manager2);
			sqlCommand.Parameters.AddWithValue("Manager3", item.Manager3 == null ? (object)DBNull.Value : item.Manager3);
			sqlCommand.Parameters.AddWithValue("OrderPrefix", item.OrderPrefix == null ? (object)DBNull.Value : item.OrderPrefix);
			sqlCommand.Parameters.AddWithValue("Phone", item.Phone == null ? (object)DBNull.Value : item.Phone);
			sqlCommand.Parameters.AddWithValue("PostalCode", item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
			sqlCommand.Parameters.AddWithValue("PurchaseGroupEmail", item.PurchaseGroupEmail == null ? (object)DBNull.Value : item.PurchaseGroupEmail);
			sqlCommand.Parameters.AddWithValue("PurchaseGroupName", item.PurchaseGroupName == null ? (object)DBNull.Value : item.PurchaseGroupName);
			sqlCommand.Parameters.AddWithValue("PurchaseId", item.PurchaseId == null ? (object)DBNull.Value : item.PurchaseId);
			sqlCommand.Parameters.AddWithValue("PurchaseName", item.PurchaseName == null ? (object)DBNull.Value : item.PurchaseName);
			sqlCommand.Parameters.AddWithValue("PurchaseProfileId", item.PurchaseProfileId == null ? (object)DBNull.Value : item.PurchaseProfileId);
			sqlCommand.Parameters.AddWithValue("ReportDefaultLanguage", item.ReportDefaultLanguage == null ? (object)DBNull.Value : item.ReportDefaultLanguage);
			sqlCommand.Parameters.AddWithValue("ReportDefaultLanguageId", item.ReportDefaultLanguageId == null ? (object)DBNull.Value : item.ReportDefaultLanguageId);
			sqlCommand.Parameters.AddWithValue("Site", item.Site == null ? (object)DBNull.Value : item.Site);
			sqlCommand.Parameters.AddWithValue("SuperValidatorOneEmail", item.SuperValidatorOneEmail == null ? (object)DBNull.Value : item.SuperValidatorOneEmail);
			sqlCommand.Parameters.AddWithValue("SuperValidatorOneId", item.SuperValidatorOneId == null ? (object)DBNull.Value : item.SuperValidatorOneId);
			sqlCommand.Parameters.AddWithValue("SuperValidatorOneName", item.SuperValidatorOneName == null ? (object)DBNull.Value : item.SuperValidatorOneName);
			sqlCommand.Parameters.AddWithValue("SuperValidatorTowEmail", item.SuperValidatorTowEmail == null ? (object)DBNull.Value : item.SuperValidatorTowEmail);
			sqlCommand.Parameters.AddWithValue("SuperValidatorTowId", item.SuperValidatorTowId == null ? (object)DBNull.Value : item.SuperValidatorTowId);
			sqlCommand.Parameters.AddWithValue("SuperValidatorTowName", item.SuperValidatorTowName == null ? (object)DBNull.Value : item.SuperValidatorTowName);
			sqlCommand.Parameters.AddWithValue("SWIFT_BIC1", item.SWIFT_BIC1 == null ? (object)DBNull.Value : item.SWIFT_BIC1);
			sqlCommand.Parameters.AddWithValue("SWIFT_BIC2", item.SWIFT_BIC2 == null ? (object)DBNull.Value : item.SWIFT_BIC2);
			sqlCommand.Parameters.AddWithValue("SWIFT_BIC3", item.SWIFT_BIC3 == null ? (object)DBNull.Value : item.SWIFT_BIC3);
			sqlCommand.Parameters.AddWithValue("SWIFT_BIC4", item.SWIFT_BIC4 == null ? (object)DBNull.Value : item.SWIFT_BIC4);
			sqlCommand.Parameters.AddWithValue("TaxNumberID", item.TaxNumberID == null ? (object)DBNull.Value : item.TaxNumberID);
			sqlCommand.Parameters.AddWithValue("VATNumberID", item.VATNumberID == null ? (object)DBNull.Value : item.VATNumberID);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 64; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__FNC_CompanyExtension] ([Account1],[Account2],[Account3],[Account4],[Address],[BankDetails1],[BankDetails2],[BankDetails3],[BankDetails4],[BLZ1],[BLZ2],[BLZ3],[BLZ4],[City],[CompanyId],[CompanyName],[Country],[CustomsNumber],[DefaultCurrencyId],[DefaultCurrencyName],[DeliveryAddress],[Email],[Fax],[FinanceProfileEmail],[FinanceProfileId],[FinanceProfileName],[FinanceValidatorOneEmail],[FinanceValidatorOneId],[FinanceValidatorOneName],[FinanceValidatorTowEmail],[FinanceValidatorTowId],[FinanceValidatorTowName],[HRB],[IBAN1],[IBAN2],[IBAN3],[IBAN4],[Manager1],[Manager2],[Manager3],[OrderPrefix],[Phone],[PostalCode],[PurchaseGroupEmail],[PurchaseGroupName],[PurchaseId],[PurchaseName],[PurchaseProfileId],[ReportDefaultLanguage],[ReportDefaultLanguageId],[Site],[SuperValidatorOneEmail],[SuperValidatorOneId],[SuperValidatorOneName],[SuperValidatorTowEmail],[SuperValidatorTowId],[SuperValidatorTowName],[SWIFT_BIC1],[SWIFT_BIC2],[SWIFT_BIC3],[SWIFT_BIC4],[TaxNumberID],[VATNumberID]) VALUES ( "

						+ "@Account1" + i + ","
						+ "@Account2" + i + ","
						+ "@Account3" + i + ","
						+ "@Account4" + i + ","
						+ "@Address" + i + ","
						+ "@BankDetails1" + i + ","
						+ "@BankDetails2" + i + ","
						+ "@BankDetails3" + i + ","
						+ "@BankDetails4" + i + ","
						+ "@BLZ1" + i + ","
						+ "@BLZ2" + i + ","
						+ "@BLZ3" + i + ","
						+ "@BLZ4" + i + ","
						+ "@City" + i + ","
						+ "@CompanyId" + i + ","
						+ "@CompanyName" + i + ","
						+ "@Country" + i + ","
						+ "@CustomsNumber" + i + ","
						+ "@DefaultCurrencyId" + i + ","
						+ "@DefaultCurrencyName" + i + ","
						+ "@DeliveryAddress" + i + ","
						+ "@Email" + i + ","
						+ "@Fax" + i + ","
						+ "@FinanceProfileEmail" + i + ","
						+ "@FinanceProfileId" + i + ","
						+ "@FinanceProfileName" + i + ","
						+ "@FinanceValidatorOneEmail" + i + ","
						+ "@FinanceValidatorOneId" + i + ","
						+ "@FinanceValidatorOneName" + i + ","
						+ "@FinanceValidatorTowEmail" + i + ","
						+ "@FinanceValidatorTowId" + i + ","
						+ "@FinanceValidatorTowName" + i + ","
						+ "@HRB" + i + ","
						+ "@IBAN1" + i + ","
						+ "@IBAN2" + i + ","
						+ "@IBAN3" + i + ","
						+ "@IBAN4" + i + ","
						+ "@Manager1" + i + ","
						+ "@Manager2" + i + ","
						+ "@Manager3" + i + ","
						+ "@OrderPrefix" + i + ","
						+ "@Phone" + i + ","
						+ "@PostalCode" + i + ","
						+ "@PurchaseGroupEmail" + i + ","
						+ "@PurchaseGroupName" + i + ","
						+ "@PurchaseId" + i + ","
						+ "@PurchaseName" + i + ","
						+ "@PurchaseProfileId" + i + ","
						+ "@ReportDefaultLanguage" + i + ","
						+ "@ReportDefaultLanguageId" + i + ","
						+ "@Site" + i + ","
						+ "@SuperValidatorOneEmail" + i + ","
						+ "@SuperValidatorOneId" + i + ","
						+ "@SuperValidatorOneName" + i + ","
						+ "@SuperValidatorTowEmail" + i + ","
						+ "@SuperValidatorTowId" + i + ","
						+ "@SuperValidatorTowName" + i + ","
						+ "@SWIFT_BIC1" + i + ","
						+ "@SWIFT_BIC2" + i + ","
						+ "@SWIFT_BIC3" + i + ","
						+ "@SWIFT_BIC4" + i + ","
						+ "@TaxNumberID" + i + ","
						+ "@VATNumberID" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Account1" + i, item.Account1 == null ? (object)DBNull.Value : item.Account1);
					sqlCommand.Parameters.AddWithValue("Account2" + i, item.Account2 == null ? (object)DBNull.Value : item.Account2);
					sqlCommand.Parameters.AddWithValue("Account3" + i, item.Account3 == null ? (object)DBNull.Value : item.Account3);
					sqlCommand.Parameters.AddWithValue("Account4" + i, item.Account4 == null ? (object)DBNull.Value : item.Account4);
					sqlCommand.Parameters.AddWithValue("Address" + i, item.Address == null ? (object)DBNull.Value : item.Address);
					sqlCommand.Parameters.AddWithValue("BankDetails1" + i, item.BankDetails1 == null ? (object)DBNull.Value : item.BankDetails1);
					sqlCommand.Parameters.AddWithValue("BankDetails2" + i, item.BankDetails2 == null ? (object)DBNull.Value : item.BankDetails2);
					sqlCommand.Parameters.AddWithValue("BankDetails3" + i, item.BankDetails3 == null ? (object)DBNull.Value : item.BankDetails3);
					sqlCommand.Parameters.AddWithValue("BankDetails4" + i, item.BankDetails4 == null ? (object)DBNull.Value : item.BankDetails4);
					sqlCommand.Parameters.AddWithValue("BLZ1" + i, item.BLZ1 == null ? (object)DBNull.Value : item.BLZ1);
					sqlCommand.Parameters.AddWithValue("BLZ2" + i, item.BLZ2 == null ? (object)DBNull.Value : item.BLZ2);
					sqlCommand.Parameters.AddWithValue("BLZ3" + i, item.BLZ3 == null ? (object)DBNull.Value : item.BLZ3);
					sqlCommand.Parameters.AddWithValue("BLZ4" + i, item.BLZ4 == null ? (object)DBNull.Value : item.BLZ4);
					sqlCommand.Parameters.AddWithValue("City" + i, item.City == null ? (object)DBNull.Value : item.City);
					sqlCommand.Parameters.AddWithValue("CompanyId" + i, item.CompanyId);
					sqlCommand.Parameters.AddWithValue("CompanyName" + i, item.CompanyName == null ? (object)DBNull.Value : item.CompanyName);
					sqlCommand.Parameters.AddWithValue("Country" + i, item.Country == null ? (object)DBNull.Value : item.Country);
					sqlCommand.Parameters.AddWithValue("CustomsNumber" + i, item.CustomsNumber == null ? (object)DBNull.Value : item.CustomsNumber);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyId" + i, item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyName" + i, item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
					sqlCommand.Parameters.AddWithValue("DeliveryAddress" + i, item.DeliveryAddress == null ? (object)DBNull.Value : item.DeliveryAddress);
					sqlCommand.Parameters.AddWithValue("Email" + i, item.Email == null ? (object)DBNull.Value : item.Email);
					sqlCommand.Parameters.AddWithValue("Fax" + i, item.Fax == null ? (object)DBNull.Value : item.Fax);
					sqlCommand.Parameters.AddWithValue("FinanceProfileEmail" + i, item.FinanceProfileEmail == null ? (object)DBNull.Value : item.FinanceProfileEmail);
					sqlCommand.Parameters.AddWithValue("FinanceProfileId" + i, item.FinanceProfileId == null ? (object)DBNull.Value : item.FinanceProfileId);
					sqlCommand.Parameters.AddWithValue("FinanceProfileName" + i, item.FinanceProfileName == null ? (object)DBNull.Value : item.FinanceProfileName);
					sqlCommand.Parameters.AddWithValue("FinanceValidatorOneEmail" + i, item.FinanceValidatorOneEmail == null ? (object)DBNull.Value : item.FinanceValidatorOneEmail);
					sqlCommand.Parameters.AddWithValue("FinanceValidatorOneId" + i, item.FinanceValidatorOneId == null ? (object)DBNull.Value : item.FinanceValidatorOneId);
					sqlCommand.Parameters.AddWithValue("FinanceValidatorOneName" + i, item.FinanceValidatorOneName == null ? (object)DBNull.Value : item.FinanceValidatorOneName);
					sqlCommand.Parameters.AddWithValue("FinanceValidatorTowEmail" + i, item.FinanceValidatorTowEmail == null ? (object)DBNull.Value : item.FinanceValidatorTowEmail);
					sqlCommand.Parameters.AddWithValue("FinanceValidatorTowId" + i, item.FinanceValidatorTowId == null ? (object)DBNull.Value : item.FinanceValidatorTowId);
					sqlCommand.Parameters.AddWithValue("FinanceValidatorTowName" + i, item.FinanceValidatorTowName == null ? (object)DBNull.Value : item.FinanceValidatorTowName);
					sqlCommand.Parameters.AddWithValue("HRB" + i, item.HRB == null ? (object)DBNull.Value : item.HRB);
					sqlCommand.Parameters.AddWithValue("IBAN1" + i, item.IBAN1 == null ? (object)DBNull.Value : item.IBAN1);
					sqlCommand.Parameters.AddWithValue("IBAN2" + i, item.IBAN2 == null ? (object)DBNull.Value : item.IBAN2);
					sqlCommand.Parameters.AddWithValue("IBAN3" + i, item.IBAN3 == null ? (object)DBNull.Value : item.IBAN3);
					sqlCommand.Parameters.AddWithValue("IBAN4" + i, item.IBAN4 == null ? (object)DBNull.Value : item.IBAN4);
					sqlCommand.Parameters.AddWithValue("Manager1" + i, item.Manager1 == null ? (object)DBNull.Value : item.Manager1);
					sqlCommand.Parameters.AddWithValue("Manager2" + i, item.Manager2 == null ? (object)DBNull.Value : item.Manager2);
					sqlCommand.Parameters.AddWithValue("Manager3" + i, item.Manager3 == null ? (object)DBNull.Value : item.Manager3);
					sqlCommand.Parameters.AddWithValue("OrderPrefix" + i, item.OrderPrefix == null ? (object)DBNull.Value : item.OrderPrefix);
					sqlCommand.Parameters.AddWithValue("Phone" + i, item.Phone == null ? (object)DBNull.Value : item.Phone);
					sqlCommand.Parameters.AddWithValue("PostalCode" + i, item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
					sqlCommand.Parameters.AddWithValue("PurchaseGroupEmail" + i, item.PurchaseGroupEmail == null ? (object)DBNull.Value : item.PurchaseGroupEmail);
					sqlCommand.Parameters.AddWithValue("PurchaseGroupName" + i, item.PurchaseGroupName == null ? (object)DBNull.Value : item.PurchaseGroupName);
					sqlCommand.Parameters.AddWithValue("PurchaseId" + i, item.PurchaseId == null ? (object)DBNull.Value : item.PurchaseId);
					sqlCommand.Parameters.AddWithValue("PurchaseName" + i, item.PurchaseName == null ? (object)DBNull.Value : item.PurchaseName);
					sqlCommand.Parameters.AddWithValue("PurchaseProfileId" + i, item.PurchaseProfileId == null ? (object)DBNull.Value : item.PurchaseProfileId);
					sqlCommand.Parameters.AddWithValue("ReportDefaultLanguage" + i, item.ReportDefaultLanguage == null ? (object)DBNull.Value : item.ReportDefaultLanguage);
					sqlCommand.Parameters.AddWithValue("ReportDefaultLanguageId" + i, item.ReportDefaultLanguageId == null ? (object)DBNull.Value : item.ReportDefaultLanguageId);
					sqlCommand.Parameters.AddWithValue("Site" + i, item.Site == null ? (object)DBNull.Value : item.Site);
					sqlCommand.Parameters.AddWithValue("SuperValidatorOneEmail" + i, item.SuperValidatorOneEmail == null ? (object)DBNull.Value : item.SuperValidatorOneEmail);
					sqlCommand.Parameters.AddWithValue("SuperValidatorOneId" + i, item.SuperValidatorOneId == null ? (object)DBNull.Value : item.SuperValidatorOneId);
					sqlCommand.Parameters.AddWithValue("SuperValidatorOneName" + i, item.SuperValidatorOneName == null ? (object)DBNull.Value : item.SuperValidatorOneName);
					sqlCommand.Parameters.AddWithValue("SuperValidatorTowEmail" + i, item.SuperValidatorTowEmail == null ? (object)DBNull.Value : item.SuperValidatorTowEmail);
					sqlCommand.Parameters.AddWithValue("SuperValidatorTowId" + i, item.SuperValidatorTowId == null ? (object)DBNull.Value : item.SuperValidatorTowId);
					sqlCommand.Parameters.AddWithValue("SuperValidatorTowName" + i, item.SuperValidatorTowName == null ? (object)DBNull.Value : item.SuperValidatorTowName);
					sqlCommand.Parameters.AddWithValue("SWIFT_BIC1" + i, item.SWIFT_BIC1 == null ? (object)DBNull.Value : item.SWIFT_BIC1);
					sqlCommand.Parameters.AddWithValue("SWIFT_BIC2" + i, item.SWIFT_BIC2 == null ? (object)DBNull.Value : item.SWIFT_BIC2);
					sqlCommand.Parameters.AddWithValue("SWIFT_BIC3" + i, item.SWIFT_BIC3 == null ? (object)DBNull.Value : item.SWIFT_BIC3);
					sqlCommand.Parameters.AddWithValue("SWIFT_BIC4" + i, item.SWIFT_BIC4 == null ? (object)DBNull.Value : item.SWIFT_BIC4);
					sqlCommand.Parameters.AddWithValue("TaxNumberID" + i, item.TaxNumberID == null ? (object)DBNull.Value : item.TaxNumberID);
					sqlCommand.Parameters.AddWithValue("VATNumberID" + i, item.VATNumberID == null ? (object)DBNull.Value : item.VATNumberID);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__FNC_CompanyExtension] SET [Account1]=@Account1, [Account2]=@Account2, [Account3]=@Account3, [Account4]=@Account4, [Address]=@Address, [BankDetails1]=@BankDetails1, [BankDetails2]=@BankDetails2, [BankDetails3]=@BankDetails3, [BankDetails4]=@BankDetails4, [BLZ1]=@BLZ1, [BLZ2]=@BLZ2, [BLZ3]=@BLZ3, [BLZ4]=@BLZ4, [City]=@City, [CompanyId]=@CompanyId, [CompanyName]=@CompanyName, [Country]=@Country, [CustomsNumber]=@CustomsNumber, [DefaultCurrencyId]=@DefaultCurrencyId, [DefaultCurrencyName]=@DefaultCurrencyName, [DeliveryAddress]=@DeliveryAddress, [Email]=@Email, [Fax]=@Fax, [FinanceProfileEmail]=@FinanceProfileEmail, [FinanceProfileId]=@FinanceProfileId, [FinanceProfileName]=@FinanceProfileName, [FinanceValidatorOneEmail]=@FinanceValidatorOneEmail, [FinanceValidatorOneId]=@FinanceValidatorOneId, [FinanceValidatorOneName]=@FinanceValidatorOneName, [FinanceValidatorTowEmail]=@FinanceValidatorTowEmail, [FinanceValidatorTowId]=@FinanceValidatorTowId, [FinanceValidatorTowName]=@FinanceValidatorTowName, [HRB]=@HRB, [IBAN1]=@IBAN1, [IBAN2]=@IBAN2, [IBAN3]=@IBAN3, [IBAN4]=@IBAN4, [Manager1]=@Manager1, [Manager2]=@Manager2, [Manager3]=@Manager3, [OrderPrefix]=@OrderPrefix, [Phone]=@Phone, [PostalCode]=@PostalCode, [PurchaseGroupEmail]=@PurchaseGroupEmail, [PurchaseGroupName]=@PurchaseGroupName, [PurchaseId]=@PurchaseId, [PurchaseName]=@PurchaseName, [PurchaseProfileId]=@PurchaseProfileId, [ReportDefaultLanguage]=@ReportDefaultLanguage, [ReportDefaultLanguageId]=@ReportDefaultLanguageId, [Site]=@Site, [SuperValidatorOneEmail]=@SuperValidatorOneEmail, [SuperValidatorOneId]=@SuperValidatorOneId, [SuperValidatorOneName]=@SuperValidatorOneName, [SuperValidatorTowEmail]=@SuperValidatorTowEmail, [SuperValidatorTowId]=@SuperValidatorTowId, [SuperValidatorTowName]=@SuperValidatorTowName, [SWIFT_BIC1]=@SWIFT_BIC1, [SWIFT_BIC2]=@SWIFT_BIC2, [SWIFT_BIC3]=@SWIFT_BIC3, [SWIFT_BIC4]=@SWIFT_BIC4, [TaxNumberID]=@TaxNumberID, [VATNumberID]=@VATNumberID WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("Account1", item.Account1 == null ? (object)DBNull.Value : item.Account1);
			sqlCommand.Parameters.AddWithValue("Account2", item.Account2 == null ? (object)DBNull.Value : item.Account2);
			sqlCommand.Parameters.AddWithValue("Account3", item.Account3 == null ? (object)DBNull.Value : item.Account3);
			sqlCommand.Parameters.AddWithValue("Account4", item.Account4 == null ? (object)DBNull.Value : item.Account4);
			sqlCommand.Parameters.AddWithValue("Address", item.Address == null ? (object)DBNull.Value : item.Address);
			sqlCommand.Parameters.AddWithValue("BankDetails1", item.BankDetails1 == null ? (object)DBNull.Value : item.BankDetails1);
			sqlCommand.Parameters.AddWithValue("BankDetails2", item.BankDetails2 == null ? (object)DBNull.Value : item.BankDetails2);
			sqlCommand.Parameters.AddWithValue("BankDetails3", item.BankDetails3 == null ? (object)DBNull.Value : item.BankDetails3);
			sqlCommand.Parameters.AddWithValue("BankDetails4", item.BankDetails4 == null ? (object)DBNull.Value : item.BankDetails4);
			sqlCommand.Parameters.AddWithValue("BLZ1", item.BLZ1 == null ? (object)DBNull.Value : item.BLZ1);
			sqlCommand.Parameters.AddWithValue("BLZ2", item.BLZ2 == null ? (object)DBNull.Value : item.BLZ2);
			sqlCommand.Parameters.AddWithValue("BLZ3", item.BLZ3 == null ? (object)DBNull.Value : item.BLZ3);
			sqlCommand.Parameters.AddWithValue("BLZ4", item.BLZ4 == null ? (object)DBNull.Value : item.BLZ4);
			sqlCommand.Parameters.AddWithValue("City", item.City == null ? (object)DBNull.Value : item.City);
			sqlCommand.Parameters.AddWithValue("CompanyId", item.CompanyId);
			sqlCommand.Parameters.AddWithValue("CompanyName", item.CompanyName == null ? (object)DBNull.Value : item.CompanyName);
			sqlCommand.Parameters.AddWithValue("Country", item.Country == null ? (object)DBNull.Value : item.Country);
			sqlCommand.Parameters.AddWithValue("CustomsNumber", item.CustomsNumber == null ? (object)DBNull.Value : item.CustomsNumber);
			sqlCommand.Parameters.AddWithValue("DefaultCurrencyId", item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
			sqlCommand.Parameters.AddWithValue("DefaultCurrencyName", item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
			sqlCommand.Parameters.AddWithValue("DeliveryAddress", item.DeliveryAddress == null ? (object)DBNull.Value : item.DeliveryAddress);
			sqlCommand.Parameters.AddWithValue("Email", item.Email == null ? (object)DBNull.Value : item.Email);
			sqlCommand.Parameters.AddWithValue("Fax", item.Fax == null ? (object)DBNull.Value : item.Fax);
			sqlCommand.Parameters.AddWithValue("FinanceProfileEmail", item.FinanceProfileEmail == null ? (object)DBNull.Value : item.FinanceProfileEmail);
			sqlCommand.Parameters.AddWithValue("FinanceProfileId", item.FinanceProfileId == null ? (object)DBNull.Value : item.FinanceProfileId);
			sqlCommand.Parameters.AddWithValue("FinanceProfileName", item.FinanceProfileName == null ? (object)DBNull.Value : item.FinanceProfileName);
			sqlCommand.Parameters.AddWithValue("FinanceValidatorOneEmail", item.FinanceValidatorOneEmail == null ? (object)DBNull.Value : item.FinanceValidatorOneEmail);
			sqlCommand.Parameters.AddWithValue("FinanceValidatorOneId", item.FinanceValidatorOneId == null ? (object)DBNull.Value : item.FinanceValidatorOneId);
			sqlCommand.Parameters.AddWithValue("FinanceValidatorOneName", item.FinanceValidatorOneName == null ? (object)DBNull.Value : item.FinanceValidatorOneName);
			sqlCommand.Parameters.AddWithValue("FinanceValidatorTowEmail", item.FinanceValidatorTowEmail == null ? (object)DBNull.Value : item.FinanceValidatorTowEmail);
			sqlCommand.Parameters.AddWithValue("FinanceValidatorTowId", item.FinanceValidatorTowId == null ? (object)DBNull.Value : item.FinanceValidatorTowId);
			sqlCommand.Parameters.AddWithValue("FinanceValidatorTowName", item.FinanceValidatorTowName == null ? (object)DBNull.Value : item.FinanceValidatorTowName);
			sqlCommand.Parameters.AddWithValue("HRB", item.HRB == null ? (object)DBNull.Value : item.HRB);
			sqlCommand.Parameters.AddWithValue("IBAN1", item.IBAN1 == null ? (object)DBNull.Value : item.IBAN1);
			sqlCommand.Parameters.AddWithValue("IBAN2", item.IBAN2 == null ? (object)DBNull.Value : item.IBAN2);
			sqlCommand.Parameters.AddWithValue("IBAN3", item.IBAN3 == null ? (object)DBNull.Value : item.IBAN3);
			sqlCommand.Parameters.AddWithValue("IBAN4", item.IBAN4 == null ? (object)DBNull.Value : item.IBAN4);
			sqlCommand.Parameters.AddWithValue("Manager1", item.Manager1 == null ? (object)DBNull.Value : item.Manager1);
			sqlCommand.Parameters.AddWithValue("Manager2", item.Manager2 == null ? (object)DBNull.Value : item.Manager2);
			sqlCommand.Parameters.AddWithValue("Manager3", item.Manager3 == null ? (object)DBNull.Value : item.Manager3);
			sqlCommand.Parameters.AddWithValue("OrderPrefix", item.OrderPrefix == null ? (object)DBNull.Value : item.OrderPrefix);
			sqlCommand.Parameters.AddWithValue("Phone", item.Phone == null ? (object)DBNull.Value : item.Phone);
			sqlCommand.Parameters.AddWithValue("PostalCode", item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
			sqlCommand.Parameters.AddWithValue("PurchaseGroupEmail", item.PurchaseGroupEmail == null ? (object)DBNull.Value : item.PurchaseGroupEmail);
			sqlCommand.Parameters.AddWithValue("PurchaseGroupName", item.PurchaseGroupName == null ? (object)DBNull.Value : item.PurchaseGroupName);
			sqlCommand.Parameters.AddWithValue("PurchaseId", item.PurchaseId == null ? (object)DBNull.Value : item.PurchaseId);
			sqlCommand.Parameters.AddWithValue("PurchaseName", item.PurchaseName == null ? (object)DBNull.Value : item.PurchaseName);
			sqlCommand.Parameters.AddWithValue("PurchaseProfileId", item.PurchaseProfileId == null ? (object)DBNull.Value : item.PurchaseProfileId);
			sqlCommand.Parameters.AddWithValue("ReportDefaultLanguage", item.ReportDefaultLanguage == null ? (object)DBNull.Value : item.ReportDefaultLanguage);
			sqlCommand.Parameters.AddWithValue("ReportDefaultLanguageId", item.ReportDefaultLanguageId == null ? (object)DBNull.Value : item.ReportDefaultLanguageId);
			sqlCommand.Parameters.AddWithValue("Site", item.Site == null ? (object)DBNull.Value : item.Site);
			sqlCommand.Parameters.AddWithValue("SuperValidatorOneEmail", item.SuperValidatorOneEmail == null ? (object)DBNull.Value : item.SuperValidatorOneEmail);
			sqlCommand.Parameters.AddWithValue("SuperValidatorOneId", item.SuperValidatorOneId == null ? (object)DBNull.Value : item.SuperValidatorOneId);
			sqlCommand.Parameters.AddWithValue("SuperValidatorOneName", item.SuperValidatorOneName == null ? (object)DBNull.Value : item.SuperValidatorOneName);
			sqlCommand.Parameters.AddWithValue("SuperValidatorTowEmail", item.SuperValidatorTowEmail == null ? (object)DBNull.Value : item.SuperValidatorTowEmail);
			sqlCommand.Parameters.AddWithValue("SuperValidatorTowId", item.SuperValidatorTowId == null ? (object)DBNull.Value : item.SuperValidatorTowId);
			sqlCommand.Parameters.AddWithValue("SuperValidatorTowName", item.SuperValidatorTowName == null ? (object)DBNull.Value : item.SuperValidatorTowName);
			sqlCommand.Parameters.AddWithValue("SWIFT_BIC1", item.SWIFT_BIC1 == null ? (object)DBNull.Value : item.SWIFT_BIC1);
			sqlCommand.Parameters.AddWithValue("SWIFT_BIC2", item.SWIFT_BIC2 == null ? (object)DBNull.Value : item.SWIFT_BIC2);
			sqlCommand.Parameters.AddWithValue("SWIFT_BIC3", item.SWIFT_BIC3 == null ? (object)DBNull.Value : item.SWIFT_BIC3);
			sqlCommand.Parameters.AddWithValue("SWIFT_BIC4", item.SWIFT_BIC4 == null ? (object)DBNull.Value : item.SWIFT_BIC4);
			sqlCommand.Parameters.AddWithValue("TaxNumberID", item.TaxNumberID == null ? (object)DBNull.Value : item.TaxNumberID);
			sqlCommand.Parameters.AddWithValue("VATNumberID", item.VATNumberID == null ? (object)DBNull.Value : item.VATNumberID);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 64; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__FNC_CompanyExtension] SET "

					+ "[Account1]=@Account1" + i + ","
					+ "[Account2]=@Account2" + i + ","
					+ "[Account3]=@Account3" + i + ","
					+ "[Account4]=@Account4" + i + ","
					+ "[Address]=@Address" + i + ","
					+ "[BankDetails1]=@BankDetails1" + i + ","
					+ "[BankDetails2]=@BankDetails2" + i + ","
					+ "[BankDetails3]=@BankDetails3" + i + ","
					+ "[BankDetails4]=@BankDetails4" + i + ","
					+ "[BLZ1]=@BLZ1" + i + ","
					+ "[BLZ2]=@BLZ2" + i + ","
					+ "[BLZ3]=@BLZ3" + i + ","
					+ "[BLZ4]=@BLZ4" + i + ","
					+ "[City]=@City" + i + ","
					+ "[CompanyId]=@CompanyId" + i + ","
					+ "[CompanyName]=@CompanyName" + i + ","
					+ "[Country]=@Country" + i + ","
					+ "[CustomsNumber]=@CustomsNumber" + i + ","
					+ "[DefaultCurrencyId]=@DefaultCurrencyId" + i + ","
					+ "[DefaultCurrencyName]=@DefaultCurrencyName" + i + ","
					+ "[DeliveryAddress]=@DeliveryAddress" + i + ","
					+ "[Email]=@Email" + i + ","
					+ "[Fax]=@Fax" + i + ","
					+ "[FinanceProfileEmail]=@FinanceProfileEmail" + i + ","
					+ "[FinanceProfileId]=@FinanceProfileId" + i + ","
					+ "[FinanceProfileName]=@FinanceProfileName" + i + ","
					+ "[FinanceValidatorOneEmail]=@FinanceValidatorOneEmail" + i + ","
					+ "[FinanceValidatorOneId]=@FinanceValidatorOneId" + i + ","
					+ "[FinanceValidatorOneName]=@FinanceValidatorOneName" + i + ","
					+ "[FinanceValidatorTowEmail]=@FinanceValidatorTowEmail" + i + ","
					+ "[FinanceValidatorTowId]=@FinanceValidatorTowId" + i + ","
					+ "[FinanceValidatorTowName]=@FinanceValidatorTowName" + i + ","
					+ "[HRB]=@HRB" + i + ","
					+ "[IBAN1]=@IBAN1" + i + ","
					+ "[IBAN2]=@IBAN2" + i + ","
					+ "[IBAN3]=@IBAN3" + i + ","
					+ "[IBAN4]=@IBAN4" + i + ","
					+ "[Manager1]=@Manager1" + i + ","
					+ "[Manager2]=@Manager2" + i + ","
					+ "[Manager3]=@Manager3" + i + ","
					+ "[OrderPrefix]=@OrderPrefix" + i + ","
					+ "[Phone]=@Phone" + i + ","
					+ "[PostalCode]=@PostalCode" + i + ","
					+ "[PurchaseGroupEmail]=@PurchaseGroupEmail" + i + ","
					+ "[PurchaseGroupName]=@PurchaseGroupName" + i + ","
					+ "[PurchaseId]=@PurchaseId" + i + ","
					+ "[PurchaseName]=@PurchaseName" + i + ","
					+ "[PurchaseProfileId]=@PurchaseProfileId" + i + ","
					+ "[ReportDefaultLanguage]=@ReportDefaultLanguage" + i + ","
					+ "[ReportDefaultLanguageId]=@ReportDefaultLanguageId" + i + ","
					+ "[Site]=@Site" + i + ","
					+ "[SuperValidatorOneEmail]=@SuperValidatorOneEmail" + i + ","
					+ "[SuperValidatorOneId]=@SuperValidatorOneId" + i + ","
					+ "[SuperValidatorOneName]=@SuperValidatorOneName" + i + ","
					+ "[SuperValidatorTowEmail]=@SuperValidatorTowEmail" + i + ","
					+ "[SuperValidatorTowId]=@SuperValidatorTowId" + i + ","
					+ "[SuperValidatorTowName]=@SuperValidatorTowName" + i + ","
					+ "[SWIFT_BIC1]=@SWIFT_BIC1" + i + ","
					+ "[SWIFT_BIC2]=@SWIFT_BIC2" + i + ","
					+ "[SWIFT_BIC3]=@SWIFT_BIC3" + i + ","
					+ "[SWIFT_BIC4]=@SWIFT_BIC4" + i + ","
					+ "[TaxNumberID]=@TaxNumberID" + i + ","
					+ "[VATNumberID]=@VATNumberID" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("Account1" + i, item.Account1 == null ? (object)DBNull.Value : item.Account1);
					sqlCommand.Parameters.AddWithValue("Account2" + i, item.Account2 == null ? (object)DBNull.Value : item.Account2);
					sqlCommand.Parameters.AddWithValue("Account3" + i, item.Account3 == null ? (object)DBNull.Value : item.Account3);
					sqlCommand.Parameters.AddWithValue("Account4" + i, item.Account4 == null ? (object)DBNull.Value : item.Account4);
					sqlCommand.Parameters.AddWithValue("Address" + i, item.Address == null ? (object)DBNull.Value : item.Address);
					sqlCommand.Parameters.AddWithValue("BankDetails1" + i, item.BankDetails1 == null ? (object)DBNull.Value : item.BankDetails1);
					sqlCommand.Parameters.AddWithValue("BankDetails2" + i, item.BankDetails2 == null ? (object)DBNull.Value : item.BankDetails2);
					sqlCommand.Parameters.AddWithValue("BankDetails3" + i, item.BankDetails3 == null ? (object)DBNull.Value : item.BankDetails3);
					sqlCommand.Parameters.AddWithValue("BankDetails4" + i, item.BankDetails4 == null ? (object)DBNull.Value : item.BankDetails4);
					sqlCommand.Parameters.AddWithValue("BLZ1" + i, item.BLZ1 == null ? (object)DBNull.Value : item.BLZ1);
					sqlCommand.Parameters.AddWithValue("BLZ2" + i, item.BLZ2 == null ? (object)DBNull.Value : item.BLZ2);
					sqlCommand.Parameters.AddWithValue("BLZ3" + i, item.BLZ3 == null ? (object)DBNull.Value : item.BLZ3);
					sqlCommand.Parameters.AddWithValue("BLZ4" + i, item.BLZ4 == null ? (object)DBNull.Value : item.BLZ4);
					sqlCommand.Parameters.AddWithValue("City" + i, item.City == null ? (object)DBNull.Value : item.City);
					sqlCommand.Parameters.AddWithValue("CompanyId" + i, item.CompanyId);
					sqlCommand.Parameters.AddWithValue("CompanyName" + i, item.CompanyName == null ? (object)DBNull.Value : item.CompanyName);
					sqlCommand.Parameters.AddWithValue("Country" + i, item.Country == null ? (object)DBNull.Value : item.Country);
					sqlCommand.Parameters.AddWithValue("CustomsNumber" + i, item.CustomsNumber == null ? (object)DBNull.Value : item.CustomsNumber);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyId" + i, item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyName" + i, item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
					sqlCommand.Parameters.AddWithValue("DeliveryAddress" + i, item.DeliveryAddress == null ? (object)DBNull.Value : item.DeliveryAddress);
					sqlCommand.Parameters.AddWithValue("Email" + i, item.Email == null ? (object)DBNull.Value : item.Email);
					sqlCommand.Parameters.AddWithValue("Fax" + i, item.Fax == null ? (object)DBNull.Value : item.Fax);
					sqlCommand.Parameters.AddWithValue("FinanceProfileEmail" + i, item.FinanceProfileEmail == null ? (object)DBNull.Value : item.FinanceProfileEmail);
					sqlCommand.Parameters.AddWithValue("FinanceProfileId" + i, item.FinanceProfileId == null ? (object)DBNull.Value : item.FinanceProfileId);
					sqlCommand.Parameters.AddWithValue("FinanceProfileName" + i, item.FinanceProfileName == null ? (object)DBNull.Value : item.FinanceProfileName);
					sqlCommand.Parameters.AddWithValue("FinanceValidatorOneEmail" + i, item.FinanceValidatorOneEmail == null ? (object)DBNull.Value : item.FinanceValidatorOneEmail);
					sqlCommand.Parameters.AddWithValue("FinanceValidatorOneId" + i, item.FinanceValidatorOneId == null ? (object)DBNull.Value : item.FinanceValidatorOneId);
					sqlCommand.Parameters.AddWithValue("FinanceValidatorOneName" + i, item.FinanceValidatorOneName == null ? (object)DBNull.Value : item.FinanceValidatorOneName);
					sqlCommand.Parameters.AddWithValue("FinanceValidatorTowEmail" + i, item.FinanceValidatorTowEmail == null ? (object)DBNull.Value : item.FinanceValidatorTowEmail);
					sqlCommand.Parameters.AddWithValue("FinanceValidatorTowId" + i, item.FinanceValidatorTowId == null ? (object)DBNull.Value : item.FinanceValidatorTowId);
					sqlCommand.Parameters.AddWithValue("FinanceValidatorTowName" + i, item.FinanceValidatorTowName == null ? (object)DBNull.Value : item.FinanceValidatorTowName);
					sqlCommand.Parameters.AddWithValue("HRB" + i, item.HRB == null ? (object)DBNull.Value : item.HRB);
					sqlCommand.Parameters.AddWithValue("IBAN1" + i, item.IBAN1 == null ? (object)DBNull.Value : item.IBAN1);
					sqlCommand.Parameters.AddWithValue("IBAN2" + i, item.IBAN2 == null ? (object)DBNull.Value : item.IBAN2);
					sqlCommand.Parameters.AddWithValue("IBAN3" + i, item.IBAN3 == null ? (object)DBNull.Value : item.IBAN3);
					sqlCommand.Parameters.AddWithValue("IBAN4" + i, item.IBAN4 == null ? (object)DBNull.Value : item.IBAN4);
					sqlCommand.Parameters.AddWithValue("Manager1" + i, item.Manager1 == null ? (object)DBNull.Value : item.Manager1);
					sqlCommand.Parameters.AddWithValue("Manager2" + i, item.Manager2 == null ? (object)DBNull.Value : item.Manager2);
					sqlCommand.Parameters.AddWithValue("Manager3" + i, item.Manager3 == null ? (object)DBNull.Value : item.Manager3);
					sqlCommand.Parameters.AddWithValue("OrderPrefix" + i, item.OrderPrefix == null ? (object)DBNull.Value : item.OrderPrefix);
					sqlCommand.Parameters.AddWithValue("Phone" + i, item.Phone == null ? (object)DBNull.Value : item.Phone);
					sqlCommand.Parameters.AddWithValue("PostalCode" + i, item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
					sqlCommand.Parameters.AddWithValue("PurchaseGroupEmail" + i, item.PurchaseGroupEmail == null ? (object)DBNull.Value : item.PurchaseGroupEmail);
					sqlCommand.Parameters.AddWithValue("PurchaseGroupName" + i, item.PurchaseGroupName == null ? (object)DBNull.Value : item.PurchaseGroupName);
					sqlCommand.Parameters.AddWithValue("PurchaseId" + i, item.PurchaseId == null ? (object)DBNull.Value : item.PurchaseId);
					sqlCommand.Parameters.AddWithValue("PurchaseName" + i, item.PurchaseName == null ? (object)DBNull.Value : item.PurchaseName);
					sqlCommand.Parameters.AddWithValue("PurchaseProfileId" + i, item.PurchaseProfileId == null ? (object)DBNull.Value : item.PurchaseProfileId);
					sqlCommand.Parameters.AddWithValue("ReportDefaultLanguage" + i, item.ReportDefaultLanguage == null ? (object)DBNull.Value : item.ReportDefaultLanguage);
					sqlCommand.Parameters.AddWithValue("ReportDefaultLanguageId" + i, item.ReportDefaultLanguageId == null ? (object)DBNull.Value : item.ReportDefaultLanguageId);
					sqlCommand.Parameters.AddWithValue("Site" + i, item.Site == null ? (object)DBNull.Value : item.Site);
					sqlCommand.Parameters.AddWithValue("SuperValidatorOneEmail" + i, item.SuperValidatorOneEmail == null ? (object)DBNull.Value : item.SuperValidatorOneEmail);
					sqlCommand.Parameters.AddWithValue("SuperValidatorOneId" + i, item.SuperValidatorOneId == null ? (object)DBNull.Value : item.SuperValidatorOneId);
					sqlCommand.Parameters.AddWithValue("SuperValidatorOneName" + i, item.SuperValidatorOneName == null ? (object)DBNull.Value : item.SuperValidatorOneName);
					sqlCommand.Parameters.AddWithValue("SuperValidatorTowEmail" + i, item.SuperValidatorTowEmail == null ? (object)DBNull.Value : item.SuperValidatorTowEmail);
					sqlCommand.Parameters.AddWithValue("SuperValidatorTowId" + i, item.SuperValidatorTowId == null ? (object)DBNull.Value : item.SuperValidatorTowId);
					sqlCommand.Parameters.AddWithValue("SuperValidatorTowName" + i, item.SuperValidatorTowName == null ? (object)DBNull.Value : item.SuperValidatorTowName);
					sqlCommand.Parameters.AddWithValue("SWIFT_BIC1" + i, item.SWIFT_BIC1 == null ? (object)DBNull.Value : item.SWIFT_BIC1);
					sqlCommand.Parameters.AddWithValue("SWIFT_BIC2" + i, item.SWIFT_BIC2 == null ? (object)DBNull.Value : item.SWIFT_BIC2);
					sqlCommand.Parameters.AddWithValue("SWIFT_BIC3" + i, item.SWIFT_BIC3 == null ? (object)DBNull.Value : item.SWIFT_BIC3);
					sqlCommand.Parameters.AddWithValue("SWIFT_BIC4" + i, item.SWIFT_BIC4 == null ? (object)DBNull.Value : item.SWIFT_BIC4);
					sqlCommand.Parameters.AddWithValue("TaxNumberID" + i, item.TaxNumberID == null ? (object)DBNull.Value : item.TaxNumberID);
					sqlCommand.Parameters.AddWithValue("VATNumberID" + i, item.VATNumberID == null ? (object)DBNull.Value : item.VATNumberID);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__FNC_CompanyExtension] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [__FNC_CompanyExtension] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods

		public static Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity GetByCompany(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_CompanyExtension] WHERE [CompanyId]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity> GetByCompanyIds(List<int> ids)
		{
			if(ids == null || ids.Count <= 0)
			{
				return null;
			}

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_CompanyExtension] WHERE [CompanyId] IN ({string.Join(", ", ids)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity> GetByPurchaseIds(List<int> ids)
		{
			if(ids == null || ids.Count <= 0)
			{
				return null;
			}

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_CompanyExtension] WHERE [PurchaseProfileId] IN ({string.Join(", ", ids)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity>();
			}
		}

		public static bool IsUserFinanceValidator(int companyId, int userId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT COUNT(*) FROM [__FNC_CompanyExtension] WHERE [CompanyId]=@companyId AND ([FinanceValidatorOneId]=@userId OR [FinanceValidatorTowId]=@userId)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("companyId", companyId);
				sqlCommand.Parameters.AddWithValue("userId", userId);
				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var _i) ? _i>0 : false;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity> GetByFinanceValidatorIds(List<int> ids)
		{
			if(ids == null || ids.Count <= 0)
			{
				return null;
			}

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_CompanyExtension] WHERE [FinanceValidatorOneId] IN ({string.Join(", ", ids)}) OR [FinanceValidatorTowId] IN ({string.Join(", ", ids)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity>();
			}
		}
		#endregion
	}
}
