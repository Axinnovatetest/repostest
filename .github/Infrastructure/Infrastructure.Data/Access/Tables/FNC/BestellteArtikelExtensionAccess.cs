using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.FNC
{
	public class BestellteArtikelExtensionAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_BestellteArtikelExtension] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_BestellteArtikelExtension]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__FNC_BestellteArtikelExtension] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__FNC_BestellteArtikelExtension] ([AccountId],[AccountName],[ArticleId],[BestellteArtikelNr],[ConfirmationDate],[CurrencyId],[CurrencyName],[DefaultCurrencyDecimals],[DefaultCurrencyId],[DefaultCurrencyName],[DefaultCurrencyRate],[DeliveryDate],[Description],[Discount],[InternalContact],[LocationId],[LocationName],[OrderId],[Quantity],[SupplierDeliveryDate],[SupplierOrderNumber],[TotalCost],[TotalCostDefaultCurrency],[UnitPrice],[UnitPriceDefaultCurrency],[VAT])  VALUES (@AccountId,@AccountName,@ArticleId,@BestellteArtikelNr,@ConfirmationDate,@CurrencyId,@CurrencyName,@DefaultCurrencyDecimals,@DefaultCurrencyId,@DefaultCurrencyName,@DefaultCurrencyRate,@DeliveryDate,@Description,@Discount,@InternalContact,@LocationId,@LocationName,@OrderId,@Quantity,@SupplierDeliveryDate,@SupplierOrderNumber,@TotalCost,@TotalCostDefaultCurrency,@UnitPrice,@UnitPriceDefaultCurrency,@VAT); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AccountId", item.AccountId == null ? (object)DBNull.Value : item.AccountId);
					sqlCommand.Parameters.AddWithValue("AccountName", item.AccountName == null ? (object)DBNull.Value : item.AccountName);
					sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId);
					sqlCommand.Parameters.AddWithValue("BestellteArtikelNr", item.BestellteArtikelNr);
					sqlCommand.Parameters.AddWithValue("ConfirmationDate", item.ConfirmationDate == null ? (object)DBNull.Value : item.ConfirmationDate);
					sqlCommand.Parameters.AddWithValue("CurrencyId", item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
					sqlCommand.Parameters.AddWithValue("CurrencyName", item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyDecimals", item.DefaultCurrencyDecimals == null ? (object)DBNull.Value : item.DefaultCurrencyDecimals);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyId", item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyName", item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyRate", item.DefaultCurrencyRate == null ? (object)DBNull.Value : item.DefaultCurrencyRate);
					sqlCommand.Parameters.AddWithValue("DeliveryDate", item.DeliveryDate == null ? (object)DBNull.Value : item.DeliveryDate);
					sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
					sqlCommand.Parameters.AddWithValue("Discount", item.Discount == null ? (object)DBNull.Value : item.Discount);
					sqlCommand.Parameters.AddWithValue("InternalContact", item.InternalContact == null ? (object)DBNull.Value : item.InternalContact);
					sqlCommand.Parameters.AddWithValue("LocationId", item.LocationId == null ? (object)DBNull.Value : item.LocationId);
					sqlCommand.Parameters.AddWithValue("LocationName", item.LocationName == null ? (object)DBNull.Value : item.LocationName);
					sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
					sqlCommand.Parameters.AddWithValue("Quantity", item.Quantity == null ? (object)DBNull.Value : item.Quantity);
					sqlCommand.Parameters.AddWithValue("SupplierDeliveryDate", item.SupplierDeliveryDate == null ? (object)DBNull.Value : item.SupplierDeliveryDate);
					sqlCommand.Parameters.AddWithValue("SupplierOrderNumber", item.SupplierOrderNumber == null ? (object)DBNull.Value : item.SupplierOrderNumber);
					sqlCommand.Parameters.AddWithValue("TotalCost", item.TotalCost == null ? (object)DBNull.Value : item.TotalCost);
					sqlCommand.Parameters.AddWithValue("TotalCostDefaultCurrency", item.TotalCostDefaultCurrency == null ? (object)DBNull.Value : item.TotalCostDefaultCurrency);
					sqlCommand.Parameters.AddWithValue("UnitPrice", item.UnitPrice == null ? (object)DBNull.Value : item.UnitPrice);
					sqlCommand.Parameters.AddWithValue("UnitPriceDefaultCurrency", item.UnitPriceDefaultCurrency == null ? (object)DBNull.Value : item.UnitPriceDefaultCurrency);
					sqlCommand.Parameters.AddWithValue("VAT", item.VAT == null ? (object)DBNull.Value : item.VAT);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 27; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity> items)
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
						query += " INSERT INTO [__FNC_BestellteArtikelExtension] ([AccountId],[AccountName],[ArticleId],[BestellteArtikelNr],[ConfirmationDate],[CurrencyId],[CurrencyName],[DefaultCurrencyDecimals],[DefaultCurrencyId],[DefaultCurrencyName],[DefaultCurrencyRate],[DeliveryDate],[Description],[Discount],[InternalContact],[LocationId],[LocationName],[OrderId],[Quantity],[SupplierDeliveryDate],[SupplierOrderNumber],[TotalCost],[TotalCostDefaultCurrency],[UnitPrice],[UnitPriceDefaultCurrency],[VAT]) VALUES ( "

							+ "@AccountId" + i + ","
							+ "@AccountName" + i + ","
							+ "@ArticleId" + i + ","
							+ "@BestellteArtikelNr" + i + ","
							+ "@ConfirmationDate" + i + ","
							+ "@CurrencyId" + i + ","
							+ "@CurrencyName" + i + ","
							+ "@DefaultCurrencyDecimals" + i + ","
							+ "@DefaultCurrencyId" + i + ","
							+ "@DefaultCurrencyName" + i + ","
							+ "@DefaultCurrencyRate" + i + ","
							+ "@DeliveryDate" + i + ","
							+ "@Description" + i + ","
							+ "@Discount" + i + ","
							+ "@InternalContact" + i + ","
							+ "@LocationId" + i + ","
							+ "@LocationName" + i + ","
							+ "@OrderId" + i + ","
							+ "@Quantity" + i + ","
							+ "@SupplierDeliveryDate" + i + ","
							+ "@SupplierOrderNumber" + i + ","
							+ "@TotalCost" + i + ","
							+ "@TotalCostDefaultCurrency" + i + ","
							+ "@UnitPrice" + i + ","
							+ "@UnitPriceDefaultCurrency" + i + ","
							+ "@VAT" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("AccountId" + i, item.AccountId == null ? (object)DBNull.Value : item.AccountId);
						sqlCommand.Parameters.AddWithValue("AccountName" + i, item.AccountName == null ? (object)DBNull.Value : item.AccountName);
						sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId);
						sqlCommand.Parameters.AddWithValue("BestellteArtikelNr" + i, item.BestellteArtikelNr);
						sqlCommand.Parameters.AddWithValue("ConfirmationDate" + i, item.ConfirmationDate == null ? (object)DBNull.Value : item.ConfirmationDate);
						sqlCommand.Parameters.AddWithValue("CurrencyId" + i, item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
						sqlCommand.Parameters.AddWithValue("CurrencyName" + i, item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyDecimals" + i, item.DefaultCurrencyDecimals == null ? (object)DBNull.Value : item.DefaultCurrencyDecimals);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyId" + i, item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyName" + i, item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyRate" + i, item.DefaultCurrencyRate == null ? (object)DBNull.Value : item.DefaultCurrencyRate);
						sqlCommand.Parameters.AddWithValue("DeliveryDate" + i, item.DeliveryDate == null ? (object)DBNull.Value : item.DeliveryDate);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("Discount" + i, item.Discount == null ? (object)DBNull.Value : item.Discount);
						sqlCommand.Parameters.AddWithValue("InternalContact" + i, item.InternalContact == null ? (object)DBNull.Value : item.InternalContact);
						sqlCommand.Parameters.AddWithValue("LocationId" + i, item.LocationId == null ? (object)DBNull.Value : item.LocationId);
						sqlCommand.Parameters.AddWithValue("LocationName" + i, item.LocationName == null ? (object)DBNull.Value : item.LocationName);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
						sqlCommand.Parameters.AddWithValue("Quantity" + i, item.Quantity == null ? (object)DBNull.Value : item.Quantity);
						sqlCommand.Parameters.AddWithValue("SupplierDeliveryDate" + i, item.SupplierDeliveryDate == null ? (object)DBNull.Value : item.SupplierDeliveryDate);
						sqlCommand.Parameters.AddWithValue("SupplierOrderNumber" + i, item.SupplierOrderNumber == null ? (object)DBNull.Value : item.SupplierOrderNumber);
						sqlCommand.Parameters.AddWithValue("TotalCost" + i, item.TotalCost == null ? (object)DBNull.Value : item.TotalCost);
						sqlCommand.Parameters.AddWithValue("TotalCostDefaultCurrency" + i, item.TotalCostDefaultCurrency == null ? (object)DBNull.Value : item.TotalCostDefaultCurrency);
						sqlCommand.Parameters.AddWithValue("UnitPrice" + i, item.UnitPrice == null ? (object)DBNull.Value : item.UnitPrice);
						sqlCommand.Parameters.AddWithValue("UnitPriceDefaultCurrency" + i, item.UnitPriceDefaultCurrency == null ? (object)DBNull.Value : item.UnitPriceDefaultCurrency);
						sqlCommand.Parameters.AddWithValue("VAT" + i, item.VAT == null ? (object)DBNull.Value : item.VAT);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_BestellteArtikelExtension] SET [AccountId]=@AccountId, [AccountName]=@AccountName, [ArticleId]=@ArticleId, [BestellteArtikelNr]=@BestellteArtikelNr, [ConfirmationDate]=@ConfirmationDate, [CurrencyId]=@CurrencyId, [CurrencyName]=@CurrencyName, [DefaultCurrencyDecimals]=@DefaultCurrencyDecimals, [DefaultCurrencyId]=@DefaultCurrencyId, [DefaultCurrencyName]=@DefaultCurrencyName, [DefaultCurrencyRate]=@DefaultCurrencyRate, [DeliveryDate]=@DeliveryDate, [Description]=@Description, [Discount]=@Discount, [InternalContact]=@InternalContact, [LocationId]=@LocationId, [LocationName]=@LocationName, [OrderId]=@OrderId, [Quantity]=@Quantity, [SupplierDeliveryDate]=@SupplierDeliveryDate, [SupplierOrderNumber]=@SupplierOrderNumber, [TotalCost]=@TotalCost, [TotalCostDefaultCurrency]=@TotalCostDefaultCurrency, [UnitPrice]=@UnitPrice, [UnitPriceDefaultCurrency]=@UnitPriceDefaultCurrency, [VAT]=@VAT WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AccountId", item.AccountId == null ? (object)DBNull.Value : item.AccountId);
				sqlCommand.Parameters.AddWithValue("AccountName", item.AccountName == null ? (object)DBNull.Value : item.AccountName);
				sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId);
				sqlCommand.Parameters.AddWithValue("BestellteArtikelNr", item.BestellteArtikelNr);
				sqlCommand.Parameters.AddWithValue("ConfirmationDate", item.ConfirmationDate == null ? (object)DBNull.Value : item.ConfirmationDate);
				sqlCommand.Parameters.AddWithValue("CurrencyId", item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
				sqlCommand.Parameters.AddWithValue("CurrencyName", item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
				sqlCommand.Parameters.AddWithValue("DefaultCurrencyDecimals", item.DefaultCurrencyDecimals == null ? (object)DBNull.Value : item.DefaultCurrencyDecimals);
				sqlCommand.Parameters.AddWithValue("DefaultCurrencyId", item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
				sqlCommand.Parameters.AddWithValue("DefaultCurrencyName", item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
				sqlCommand.Parameters.AddWithValue("DefaultCurrencyRate", item.DefaultCurrencyRate == null ? (object)DBNull.Value : item.DefaultCurrencyRate);
				sqlCommand.Parameters.AddWithValue("DeliveryDate", item.DeliveryDate == null ? (object)DBNull.Value : item.DeliveryDate);
				sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
				sqlCommand.Parameters.AddWithValue("Discount", item.Discount == null ? (object)DBNull.Value : item.Discount);
				sqlCommand.Parameters.AddWithValue("InternalContact", item.InternalContact == null ? (object)DBNull.Value : item.InternalContact);
				sqlCommand.Parameters.AddWithValue("LocationId", item.LocationId == null ? (object)DBNull.Value : item.LocationId);
				sqlCommand.Parameters.AddWithValue("LocationName", item.LocationName == null ? (object)DBNull.Value : item.LocationName);
				sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
				sqlCommand.Parameters.AddWithValue("Quantity", item.Quantity == null ? (object)DBNull.Value : item.Quantity);
				sqlCommand.Parameters.AddWithValue("SupplierDeliveryDate", item.SupplierDeliveryDate == null ? (object)DBNull.Value : item.SupplierDeliveryDate);
				sqlCommand.Parameters.AddWithValue("SupplierOrderNumber", item.SupplierOrderNumber == null ? (object)DBNull.Value : item.SupplierOrderNumber);
				sqlCommand.Parameters.AddWithValue("TotalCost", item.TotalCost == null ? (object)DBNull.Value : item.TotalCost);
				sqlCommand.Parameters.AddWithValue("TotalCostDefaultCurrency", item.TotalCostDefaultCurrency == null ? (object)DBNull.Value : item.TotalCostDefaultCurrency);
				sqlCommand.Parameters.AddWithValue("UnitPrice", item.UnitPrice == null ? (object)DBNull.Value : item.UnitPrice);
				sqlCommand.Parameters.AddWithValue("UnitPriceDefaultCurrency", item.UnitPriceDefaultCurrency == null ? (object)DBNull.Value : item.UnitPriceDefaultCurrency);
				sqlCommand.Parameters.AddWithValue("VAT", item.VAT == null ? (object)DBNull.Value : item.VAT);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 27; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity> items)
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
						query += " UPDATE [__FNC_BestellteArtikelExtension] SET "

							+ "[AccountId]=@AccountId" + i + ","
							+ "[AccountName]=@AccountName" + i + ","
							+ "[ArticleId]=@ArticleId" + i + ","
							+ "[BestellteArtikelNr]=@BestellteArtikelNr" + i + ","
							+ "[ConfirmationDate]=@ConfirmationDate" + i + ","
							+ "[CurrencyId]=@CurrencyId" + i + ","
							+ "[CurrencyName]=@CurrencyName" + i + ","
							+ "[DefaultCurrencyDecimals]=@DefaultCurrencyDecimals" + i + ","
							+ "[DefaultCurrencyId]=@DefaultCurrencyId" + i + ","
							+ "[DefaultCurrencyName]=@DefaultCurrencyName" + i + ","
							+ "[DefaultCurrencyRate]=@DefaultCurrencyRate" + i + ","
							+ "[DeliveryDate]=@DeliveryDate" + i + ","
							+ "[Description]=@Description" + i + ","
							+ "[Discount]=@Discount" + i + ","
							+ "[InternalContact]=@InternalContact" + i + ","
							+ "[LocationId]=@LocationId" + i + ","
							+ "[LocationName]=@LocationName" + i + ","
							+ "[OrderId]=@OrderId" + i + ","
							+ "[Quantity]=@Quantity" + i + ","
							+ "[SupplierDeliveryDate]=@SupplierDeliveryDate" + i + ","
							+ "[SupplierOrderNumber]=@SupplierOrderNumber" + i + ","
							+ "[TotalCost]=@TotalCost" + i + ","
							+ "[TotalCostDefaultCurrency]=@TotalCostDefaultCurrency" + i + ","
							+ "[UnitPrice]=@UnitPrice" + i + ","
							+ "[UnitPriceDefaultCurrency]=@UnitPriceDefaultCurrency" + i + ","
							+ "[VAT]=@VAT" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("AccountId" + i, item.AccountId == null ? (object)DBNull.Value : item.AccountId);
						sqlCommand.Parameters.AddWithValue("AccountName" + i, item.AccountName == null ? (object)DBNull.Value : item.AccountName);
						sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId);
						sqlCommand.Parameters.AddWithValue("BestellteArtikelNr" + i, item.BestellteArtikelNr);
						sqlCommand.Parameters.AddWithValue("ConfirmationDate" + i, item.ConfirmationDate == null ? (object)DBNull.Value : item.ConfirmationDate);
						sqlCommand.Parameters.AddWithValue("CurrencyId" + i, item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
						sqlCommand.Parameters.AddWithValue("CurrencyName" + i, item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyDecimals" + i, item.DefaultCurrencyDecimals == null ? (object)DBNull.Value : item.DefaultCurrencyDecimals);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyId" + i, item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyName" + i, item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyRate" + i, item.DefaultCurrencyRate == null ? (object)DBNull.Value : item.DefaultCurrencyRate);
						sqlCommand.Parameters.AddWithValue("DeliveryDate" + i, item.DeliveryDate == null ? (object)DBNull.Value : item.DeliveryDate);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("Discount" + i, item.Discount == null ? (object)DBNull.Value : item.Discount);
						sqlCommand.Parameters.AddWithValue("InternalContact" + i, item.InternalContact == null ? (object)DBNull.Value : item.InternalContact);
						sqlCommand.Parameters.AddWithValue("LocationId" + i, item.LocationId == null ? (object)DBNull.Value : item.LocationId);
						sqlCommand.Parameters.AddWithValue("LocationName" + i, item.LocationName == null ? (object)DBNull.Value : item.LocationName);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
						sqlCommand.Parameters.AddWithValue("Quantity" + i, item.Quantity == null ? (object)DBNull.Value : item.Quantity);
						sqlCommand.Parameters.AddWithValue("SupplierDeliveryDate" + i, item.SupplierDeliveryDate == null ? (object)DBNull.Value : item.SupplierDeliveryDate);
						sqlCommand.Parameters.AddWithValue("SupplierOrderNumber" + i, item.SupplierOrderNumber == null ? (object)DBNull.Value : item.SupplierOrderNumber);
						sqlCommand.Parameters.AddWithValue("TotalCost" + i, item.TotalCost == null ? (object)DBNull.Value : item.TotalCost);
						sqlCommand.Parameters.AddWithValue("TotalCostDefaultCurrency" + i, item.TotalCostDefaultCurrency == null ? (object)DBNull.Value : item.TotalCostDefaultCurrency);
						sqlCommand.Parameters.AddWithValue("UnitPrice" + i, item.UnitPrice == null ? (object)DBNull.Value : item.UnitPrice);
						sqlCommand.Parameters.AddWithValue("UnitPriceDefaultCurrency" + i, item.UnitPriceDefaultCurrency == null ? (object)DBNull.Value : item.UnitPriceDefaultCurrency);
						sqlCommand.Parameters.AddWithValue("VAT" + i, item.VAT == null ? (object)DBNull.Value : item.VAT);
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
				string query = "DELETE FROM [__FNC_BestellteArtikelExtension] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__FNC_BestellteArtikelExtension] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity GetByBestellteArtikelNr(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_BestellteArtikelExtension] WHERE [BestellteArtikelNr]=@nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("nr", nr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity> GetByOrderId(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_BestellteArtikelExtension] WHERE OrderId=@id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity> GetByOrderId(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			string query = "";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.CommandText = "SELECT * FROM [__FNC_BestellteArtikelExtension] WHERE OrderId=@id";
				sqlCommand.Parameters.AddWithValue("id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity>();
			}
		}
		public static int DeleteByOrderId(int id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__FNC_BestellteArtikelExtension] WHERE [OrderId]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}


		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity> GetByOrderIds(List<int> ids)
		{
			if(ids == null || ids.Count <= 0)
				return new List<Entities.Tables.FNC.BestellteArtikelExtensionEntity>();

			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				string query = $"SELECT * FROM [__FNC_BestellteArtikelExtension] where OrderId IN ({string.Join(", ", ids)})";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity(x)).ToList();

			}
			else
			{ return new List<Entities.Tables.FNC.BestellteArtikelExtensionEntity>(); }
		}

		#endregion
	}
}
