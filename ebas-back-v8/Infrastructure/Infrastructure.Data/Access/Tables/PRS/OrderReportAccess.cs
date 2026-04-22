using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.PRS
{
	public class OrderReportAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__PRS_OrderReport] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__PRS_OrderReport]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__PRS_OrderReport] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__PRS_OrderReport] ([Abladestelle],[AccountOwner1],[AccountOwner2],[AccountOwner3],[AccountOwner4],[Address1],[Address2],[Address3],[Address4],[Amount],[Article],[ArtikelCountry],[ArtikelPrice],[ArtikelQuantity],[ArtikelStock],[ArtikelWeight],[BasisPrice150],[Bestellt],[ClientNumber],[CompanyLogoImageId],[Cu_G],[Cu_Surcharge],[Cu_Total],[CustomerDate],[CustomerNumber],[DEL],[Description],[Designation],[Designation1],[Designation2],[DiscountText1],[DiscountText2],[DiscountText3],[DiscountText4],[DiscountText5],[DiscountText6],[DocumentType],[FactoringText1],[FactoringText2],[FactoringText3],[FactoringText4],[FactoringText5],[FactoringText6],[Footer11],[Footer12],[Footer13],[Footer14],[Footer15],[Footer16],[Footer17],[Footer21],[Footer22],[Footer23],[Footer24],[Footer25],[Footer26],[Footer27],[Footer31],[Footer32],[Footer33],[Footer34],[Footer35],[Footer36],[Footer37],[Footer41],[Footer42],[Footer43],[Footer44],[Footer45],[Footer46],[Footer47],[Footer51],[Footer52],[Footer53],[Footer54],[Footer55],[Footer56],[Footer57],[Footer61],[Footer62],[Footer63],[Footer64],[Footer65],[Footer66],[Footer67],[Footer71],[Footer72],[Footer73],[Footer74],[Footer75],[Footer76],[Footer77],[ForDeliveryNote],[ForPosDeliveryNote],[Geliefert],[Header],[ImportLogoImageId],[Index_Kunde],[InternalNumber],[ItemsFooter1],[ItemsFooter2],[ItemsHeader],[LanguageId],[LastPageText1],[LastPageText10],[LastPageText11],[LastPageText2],[LastPageText3],[LastPageText4],[LastPageText5],[LastPageText6],[LastPageText7],[LastPageText8],[LastPageText9],[LastUpdateTime],[LastUpdateUserId],[Lieferadresse],[Liefertermin],[Offen],[OrderDate],[OrderNumber],[OrderNumberPO],[OrderTypeId],[PaymentMethod],[PaymentTarget],[PE],[Position],[ShippingMethod],[SummarySum],[SummaryTotal],[SummaryUST],[TotalPrice150],[Unit],[UnitPrice],[UnitTotal],[UST_ID]) OUTPUT INSERTED.[Id] VALUES (@Abladestelle,@AccountOwner1,@AccountOwner2,@AccountOwner3,@AccountOwner4,@Address1,@Address2,@Address3,@Address4,@Amount,@Article,@ArtikelCountry,@ArtikelPrice,@ArtikelQuantity,@ArtikelStock,@ArtikelWeight,@BasisPrice150,@Bestellt,@ClientNumber,@CompanyLogoImageId,@Cu_G,@Cu_Surcharge,@Cu_Total,@CustomerDate,@CustomerNumber,@DEL,@Description,@Designation,@Designation1,@Designation2,@DiscountText1,@DiscountText2,@DiscountText3,@DiscountText4,@DiscountText5,@DiscountText6,@DocumentType,@FactoringText1,@FactoringText2,@FactoringText3,@FactoringText4,@FactoringText5,@FactoringText6,@Footer11,@Footer12,@Footer13,@Footer14,@Footer15,@Footer16,@Footer17,@Footer21,@Footer22,@Footer23,@Footer24,@Footer25,@Footer26,@Footer27,@Footer31,@Footer32,@Footer33,@Footer34,@Footer35,@Footer36,@Footer37,@Footer41,@Footer42,@Footer43,@Footer44,@Footer45,@Footer46,@Footer47,@Footer51,@Footer52,@Footer53,@Footer54,@Footer55,@Footer56,@Footer57,@Footer61,@Footer62,@Footer63,@Footer64,@Footer65,@Footer66,@Footer67,@Footer71,@Footer72,@Footer73,@Footer74,@Footer75,@Footer76,@Footer77,@ForDeliveryNote,@ForPosDeliveryNote,@Geliefert,@Header,@ImportLogoImageId,@Index_Kunde,@InternalNumber,@ItemsFooter1,@ItemsFooter2,@ItemsHeader,@LanguageId,@LastPageText1,@LastPageText10,@LastPageText11,@LastPageText2,@LastPageText3,@LastPageText4,@LastPageText5,@LastPageText6,@LastPageText7,@LastPageText8,@LastPageText9,@LastUpdateTime,@LastUpdateUserId,@Lieferadresse,@Liefertermin,@Offen,@OrderDate,@OrderNumber,@OrderNumberPO,@OrderTypeId,@PaymentMethod,@PaymentTarget,@PE,@Position,@ShippingMethod,@SummarySum,@SummaryTotal,@SummaryUST,@TotalPrice150,@Unit,@UnitPrice,@UnitTotal,@UST_ID); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Abladestelle", item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
					sqlCommand.Parameters.AddWithValue("AccountOwner1", item.AccountOwner1 == null ? (object)DBNull.Value : item.AccountOwner1);
					sqlCommand.Parameters.AddWithValue("AccountOwner2", item.AccountOwner2 == null ? (object)DBNull.Value : item.AccountOwner2);
					sqlCommand.Parameters.AddWithValue("AccountOwner3", item.AccountOwner3 == null ? (object)DBNull.Value : item.AccountOwner3);
					sqlCommand.Parameters.AddWithValue("AccountOwner4", item.AccountOwner4 == null ? (object)DBNull.Value : item.AccountOwner4);
					sqlCommand.Parameters.AddWithValue("Address1", item.Address1 == null ? (object)DBNull.Value : item.Address1);
					sqlCommand.Parameters.AddWithValue("Address2", item.Address2 == null ? (object)DBNull.Value : item.Address2);
					sqlCommand.Parameters.AddWithValue("Address3", item.Address3 == null ? (object)DBNull.Value : item.Address3);
					sqlCommand.Parameters.AddWithValue("Address4", item.Address4 == null ? (object)DBNull.Value : item.Address4);
					sqlCommand.Parameters.AddWithValue("Amount", item.Amount == null ? (object)DBNull.Value : item.Amount);
					sqlCommand.Parameters.AddWithValue("Article", item.Article == null ? (object)DBNull.Value : item.Article);
					sqlCommand.Parameters.AddWithValue("ArtikelCountry", item.ArtikelCountry == null ? (object)DBNull.Value : item.ArtikelCountry);
					sqlCommand.Parameters.AddWithValue("ArtikelPrice", item.ArtikelPrice == null ? (object)DBNull.Value : item.ArtikelPrice);
					sqlCommand.Parameters.AddWithValue("ArtikelQuantity", item.ArtikelQuantity == null ? (object)DBNull.Value : item.ArtikelQuantity);
					sqlCommand.Parameters.AddWithValue("ArtikelStock", item.ArtikelStock == null ? (object)DBNull.Value : item.ArtikelStock);
					sqlCommand.Parameters.AddWithValue("ArtikelWeight", item.ArtikelWeight == null ? (object)DBNull.Value : item.ArtikelWeight);
					sqlCommand.Parameters.AddWithValue("BasisPrice150", item.BasisPrice150 == null ? (object)DBNull.Value : item.BasisPrice150);
					sqlCommand.Parameters.AddWithValue("Bestellt", item.Bestellt == null ? (object)DBNull.Value : item.Bestellt);
					sqlCommand.Parameters.AddWithValue("ClientNumber", item.ClientNumber == null ? (object)DBNull.Value : item.ClientNumber);
					sqlCommand.Parameters.AddWithValue("CompanyLogoImageId", item.CompanyLogoImageId);
					sqlCommand.Parameters.AddWithValue("Cu_G", item.Cu_G == null ? (object)DBNull.Value : item.Cu_G);
					sqlCommand.Parameters.AddWithValue("Cu_Surcharge", item.Cu_Surcharge == null ? (object)DBNull.Value : item.Cu_Surcharge);
					sqlCommand.Parameters.AddWithValue("Cu_Total", item.Cu_Total == null ? (object)DBNull.Value : item.Cu_Total);
					sqlCommand.Parameters.AddWithValue("CustomerDate", item.CustomerDate == null ? (object)DBNull.Value : item.CustomerDate);
					sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("DEL", item.DEL == null ? (object)DBNull.Value : item.DEL);
					sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
					sqlCommand.Parameters.AddWithValue("Designation", item.Designation == null ? (object)DBNull.Value : item.Designation);
					sqlCommand.Parameters.AddWithValue("Designation1", item.Designation1 == null ? (object)DBNull.Value : item.Designation1);
					sqlCommand.Parameters.AddWithValue("Designation2", item.Designation2 == null ? (object)DBNull.Value : item.Designation2);
					sqlCommand.Parameters.AddWithValue("DiscountText1", item.DiscountText1 == null ? (object)DBNull.Value : item.DiscountText1);
					sqlCommand.Parameters.AddWithValue("DiscountText2", item.DiscountText2 == null ? (object)DBNull.Value : item.DiscountText2);
					sqlCommand.Parameters.AddWithValue("DiscountText3", item.DiscountText3 == null ? (object)DBNull.Value : item.DiscountText3);
					sqlCommand.Parameters.AddWithValue("DiscountText4", item.DiscountText4 == null ? (object)DBNull.Value : item.DiscountText4);
					sqlCommand.Parameters.AddWithValue("DiscountText5", item.DiscountText5 == null ? (object)DBNull.Value : item.DiscountText5);
					sqlCommand.Parameters.AddWithValue("DiscountText6", item.DiscountText6 == null ? (object)DBNull.Value : item.DiscountText6);
					sqlCommand.Parameters.AddWithValue("DocumentType", item.DocumentType == null ? (object)DBNull.Value : item.DocumentType);
					sqlCommand.Parameters.AddWithValue("FactoringText1", item.FactoringText1 == null ? (object)DBNull.Value : item.FactoringText1);
					sqlCommand.Parameters.AddWithValue("FactoringText2", item.FactoringText2 == null ? (object)DBNull.Value : item.FactoringText2);
					sqlCommand.Parameters.AddWithValue("FactoringText3", item.FactoringText3 == null ? (object)DBNull.Value : item.FactoringText3);
					sqlCommand.Parameters.AddWithValue("FactoringText4", item.FactoringText4 == null ? (object)DBNull.Value : item.FactoringText4);
					sqlCommand.Parameters.AddWithValue("FactoringText5", item.FactoringText5 == null ? (object)DBNull.Value : item.FactoringText5);
					sqlCommand.Parameters.AddWithValue("FactoringText6", item.FactoringText6 == null ? (object)DBNull.Value : item.FactoringText6);
					sqlCommand.Parameters.AddWithValue("Footer11", item.Footer11 == null ? (object)DBNull.Value : item.Footer11);
					sqlCommand.Parameters.AddWithValue("Footer12", item.Footer12 == null ? (object)DBNull.Value : item.Footer12);
					sqlCommand.Parameters.AddWithValue("Footer13", item.Footer13 == null ? (object)DBNull.Value : item.Footer13);
					sqlCommand.Parameters.AddWithValue("Footer14", item.Footer14 == null ? (object)DBNull.Value : item.Footer14);
					sqlCommand.Parameters.AddWithValue("Footer15", item.Footer15 == null ? (object)DBNull.Value : item.Footer15);
					sqlCommand.Parameters.AddWithValue("Footer16", item.Footer16 == null ? (object)DBNull.Value : item.Footer16);
					sqlCommand.Parameters.AddWithValue("Footer17", item.Footer17 == null ? (object)DBNull.Value : item.Footer17);
					sqlCommand.Parameters.AddWithValue("Footer21", item.Footer21 == null ? (object)DBNull.Value : item.Footer21);
					sqlCommand.Parameters.AddWithValue("Footer22", item.Footer22 == null ? (object)DBNull.Value : item.Footer22);
					sqlCommand.Parameters.AddWithValue("Footer23", item.Footer23 == null ? (object)DBNull.Value : item.Footer23);
					sqlCommand.Parameters.AddWithValue("Footer24", item.Footer24 == null ? (object)DBNull.Value : item.Footer24);
					sqlCommand.Parameters.AddWithValue("Footer25", item.Footer25 == null ? (object)DBNull.Value : item.Footer25);
					sqlCommand.Parameters.AddWithValue("Footer26", item.Footer26 == null ? (object)DBNull.Value : item.Footer26);
					sqlCommand.Parameters.AddWithValue("Footer27", item.Footer27 == null ? (object)DBNull.Value : item.Footer27);
					sqlCommand.Parameters.AddWithValue("Footer31", item.Footer31 == null ? (object)DBNull.Value : item.Footer31);
					sqlCommand.Parameters.AddWithValue("Footer32", item.Footer32 == null ? (object)DBNull.Value : item.Footer32);
					sqlCommand.Parameters.AddWithValue("Footer33", item.Footer33 == null ? (object)DBNull.Value : item.Footer33);
					sqlCommand.Parameters.AddWithValue("Footer34", item.Footer34 == null ? (object)DBNull.Value : item.Footer34);
					sqlCommand.Parameters.AddWithValue("Footer35", item.Footer35 == null ? (object)DBNull.Value : item.Footer35);
					sqlCommand.Parameters.AddWithValue("Footer36", item.Footer36 == null ? (object)DBNull.Value : item.Footer36);
					sqlCommand.Parameters.AddWithValue("Footer37", item.Footer37 == null ? (object)DBNull.Value : item.Footer37);
					sqlCommand.Parameters.AddWithValue("Footer41", item.Footer41 == null ? (object)DBNull.Value : item.Footer41);
					sqlCommand.Parameters.AddWithValue("Footer42", item.Footer42 == null ? (object)DBNull.Value : item.Footer42);
					sqlCommand.Parameters.AddWithValue("Footer43", item.Footer43 == null ? (object)DBNull.Value : item.Footer43);
					sqlCommand.Parameters.AddWithValue("Footer44", item.Footer44 == null ? (object)DBNull.Value : item.Footer44);
					sqlCommand.Parameters.AddWithValue("Footer45", item.Footer45 == null ? (object)DBNull.Value : item.Footer45);
					sqlCommand.Parameters.AddWithValue("Footer46", item.Footer46 == null ? (object)DBNull.Value : item.Footer46);
					sqlCommand.Parameters.AddWithValue("Footer47", item.Footer47 == null ? (object)DBNull.Value : item.Footer47);
					sqlCommand.Parameters.AddWithValue("Footer51", item.Footer51 == null ? (object)DBNull.Value : item.Footer51);
					sqlCommand.Parameters.AddWithValue("Footer52", item.Footer52 == null ? (object)DBNull.Value : item.Footer52);
					sqlCommand.Parameters.AddWithValue("Footer53", item.Footer53 == null ? (object)DBNull.Value : item.Footer53);
					sqlCommand.Parameters.AddWithValue("Footer54", item.Footer54 == null ? (object)DBNull.Value : item.Footer54);
					sqlCommand.Parameters.AddWithValue("Footer55", item.Footer55 == null ? (object)DBNull.Value : item.Footer55);
					sqlCommand.Parameters.AddWithValue("Footer56", item.Footer56 == null ? (object)DBNull.Value : item.Footer56);
					sqlCommand.Parameters.AddWithValue("Footer57", item.Footer57 == null ? (object)DBNull.Value : item.Footer57);
					sqlCommand.Parameters.AddWithValue("Footer61", item.Footer61 == null ? (object)DBNull.Value : item.Footer61);
					sqlCommand.Parameters.AddWithValue("Footer62", item.Footer62 == null ? (object)DBNull.Value : item.Footer62);
					sqlCommand.Parameters.AddWithValue("Footer63", item.Footer63 == null ? (object)DBNull.Value : item.Footer63);
					sqlCommand.Parameters.AddWithValue("Footer64", item.Footer64 == null ? (object)DBNull.Value : item.Footer64);
					sqlCommand.Parameters.AddWithValue("Footer65", item.Footer65 == null ? (object)DBNull.Value : item.Footer65);
					sqlCommand.Parameters.AddWithValue("Footer66", item.Footer66 == null ? (object)DBNull.Value : item.Footer66);
					sqlCommand.Parameters.AddWithValue("Footer67", item.Footer67 == null ? (object)DBNull.Value : item.Footer67);
					sqlCommand.Parameters.AddWithValue("Footer71", item.Footer71 == null ? (object)DBNull.Value : item.Footer71);
					sqlCommand.Parameters.AddWithValue("Footer72", item.Footer72 == null ? (object)DBNull.Value : item.Footer72);
					sqlCommand.Parameters.AddWithValue("Footer73", item.Footer73 == null ? (object)DBNull.Value : item.Footer73);
					sqlCommand.Parameters.AddWithValue("Footer74", item.Footer74 == null ? (object)DBNull.Value : item.Footer74);
					sqlCommand.Parameters.AddWithValue("Footer75", item.Footer75 == null ? (object)DBNull.Value : item.Footer75);
					sqlCommand.Parameters.AddWithValue("Footer76", item.Footer76 == null ? (object)DBNull.Value : item.Footer76);
					sqlCommand.Parameters.AddWithValue("Footer77", item.Footer77 == null ? (object)DBNull.Value : item.Footer77);
					sqlCommand.Parameters.AddWithValue("ForDeliveryNote", item.ForDeliveryNote == null ? (object)DBNull.Value : item.ForDeliveryNote);
					sqlCommand.Parameters.AddWithValue("ForPosDeliveryNote", item.ForPosDeliveryNote == null ? (object)DBNull.Value : item.ForPosDeliveryNote);
					sqlCommand.Parameters.AddWithValue("Geliefert", item.Geliefert == null ? (object)DBNull.Value : item.Geliefert);
					sqlCommand.Parameters.AddWithValue("Header", item.Header == null ? (object)DBNull.Value : item.Header);
					sqlCommand.Parameters.AddWithValue("ImportLogoImageId", item.ImportLogoImageId);
					sqlCommand.Parameters.AddWithValue("Index_Kunde", item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
					sqlCommand.Parameters.AddWithValue("InternalNumber", item.InternalNumber == null ? (object)DBNull.Value : item.InternalNumber);
					sqlCommand.Parameters.AddWithValue("ItemsFooter1", item.ItemsFooter1 == null ? (object)DBNull.Value : item.ItemsFooter1);
					sqlCommand.Parameters.AddWithValue("ItemsFooter2", item.ItemsFooter2 == null ? (object)DBNull.Value : item.ItemsFooter2);
					sqlCommand.Parameters.AddWithValue("ItemsHeader", item.ItemsHeader == null ? (object)DBNull.Value : item.ItemsHeader);
					sqlCommand.Parameters.AddWithValue("LanguageId", item.LanguageId);
					sqlCommand.Parameters.AddWithValue("LastPageText1", item.LastPageText1 == null ? (object)DBNull.Value : item.LastPageText1);
					sqlCommand.Parameters.AddWithValue("LastPageText10", item.LastPageText10 == null ? (object)DBNull.Value : item.LastPageText10);
					sqlCommand.Parameters.AddWithValue("LastPageText11", item.LastPageText11 == null ? (object)DBNull.Value : item.LastPageText11);
					sqlCommand.Parameters.AddWithValue("LastPageText2", item.LastPageText2 == null ? (object)DBNull.Value : item.LastPageText2);
					sqlCommand.Parameters.AddWithValue("LastPageText3", item.LastPageText3 == null ? (object)DBNull.Value : item.LastPageText3);
					sqlCommand.Parameters.AddWithValue("LastPageText4", item.LastPageText4 == null ? (object)DBNull.Value : item.LastPageText4);
					sqlCommand.Parameters.AddWithValue("LastPageText5", item.LastPageText5 == null ? (object)DBNull.Value : item.LastPageText5);
					sqlCommand.Parameters.AddWithValue("LastPageText6", item.LastPageText6 == null ? (object)DBNull.Value : item.LastPageText6);
					sqlCommand.Parameters.AddWithValue("LastPageText7", item.LastPageText7 == null ? (object)DBNull.Value : item.LastPageText7);
					sqlCommand.Parameters.AddWithValue("LastPageText8", item.LastPageText8 == null ? (object)DBNull.Value : item.LastPageText8);
					sqlCommand.Parameters.AddWithValue("LastPageText9", item.LastPageText9 == null ? (object)DBNull.Value : item.LastPageText9);
					sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
					sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
					sqlCommand.Parameters.AddWithValue("Lieferadresse", item.Lieferadresse == null ? (object)DBNull.Value : item.Lieferadresse);
					sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("Offen", item.Offen == null ? (object)DBNull.Value : item.Offen);
					sqlCommand.Parameters.AddWithValue("OrderDate", item.OrderDate == null ? (object)DBNull.Value : item.OrderDate);
					sqlCommand.Parameters.AddWithValue("OrderNumber", item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
					sqlCommand.Parameters.AddWithValue("OrderNumberPO", item.OrderNumberPO == null ? (object)DBNull.Value : item.OrderNumberPO);
					sqlCommand.Parameters.AddWithValue("OrderTypeId", item.OrderTypeId);
					sqlCommand.Parameters.AddWithValue("PaymentMethod", item.PaymentMethod == null ? (object)DBNull.Value : item.PaymentMethod);
					sqlCommand.Parameters.AddWithValue("PaymentTarget", item.PaymentTarget == null ? (object)DBNull.Value : item.PaymentTarget);
					sqlCommand.Parameters.AddWithValue("PE", item.PE == null ? (object)DBNull.Value : item.PE);
					sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
					sqlCommand.Parameters.AddWithValue("ShippingMethod", item.ShippingMethod == null ? (object)DBNull.Value : item.ShippingMethod);
					sqlCommand.Parameters.AddWithValue("SummarySum", item.SummarySum == null ? (object)DBNull.Value : item.SummarySum);
					sqlCommand.Parameters.AddWithValue("SummaryTotal", item.SummaryTotal == null ? (object)DBNull.Value : item.SummaryTotal);
					sqlCommand.Parameters.AddWithValue("SummaryUST", item.SummaryUST == null ? (object)DBNull.Value : item.SummaryUST);
					sqlCommand.Parameters.AddWithValue("TotalPrice150", item.TotalPrice150 == null ? (object)DBNull.Value : item.TotalPrice150);
					sqlCommand.Parameters.AddWithValue("Unit", item.Unit == null ? (object)DBNull.Value : item.Unit);
					sqlCommand.Parameters.AddWithValue("UnitPrice", item.UnitPrice == null ? (object)DBNull.Value : item.UnitPrice);
					sqlCommand.Parameters.AddWithValue("UnitTotal", item.UnitTotal == null ? (object)DBNull.Value : item.UnitTotal);
					sqlCommand.Parameters.AddWithValue("UST_ID", item.UST_ID == null ? (object)DBNull.Value : item.UST_ID);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 137; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity> items)
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
						query += " INSERT INTO [__PRS_OrderReport] ([Abladestelle],[AccountOwner1],[AccountOwner2],[AccountOwner3],[AccountOwner4],[Address1],[Address2],[Address3],[Address4],[Amount],[Article],[ArtikelCountry],[ArtikelPrice],[ArtikelQuantity],[ArtikelStock],[ArtikelWeight],[BasisPrice150],[Bestellt],[ClientNumber],[CompanyLogoImageId],[Cu_G],[Cu_Surcharge],[Cu_Total],[CustomerDate],[CustomerNumber],[DEL],[Description],[Designation],[Designation1],[Designation2],[DiscountText1],[DiscountText2],[DiscountText3],[DiscountText4],[DiscountText5],[DiscountText6],[DocumentType],[FactoringText1],[FactoringText2],[FactoringText3],[FactoringText4],[FactoringText5],[FactoringText6],[Footer11],[Footer12],[Footer13],[Footer14],[Footer15],[Footer16],[Footer17],[Footer21],[Footer22],[Footer23],[Footer24],[Footer25],[Footer26],[Footer27],[Footer31],[Footer32],[Footer33],[Footer34],[Footer35],[Footer36],[Footer37],[Footer41],[Footer42],[Footer43],[Footer44],[Footer45],[Footer46],[Footer47],[Footer51],[Footer52],[Footer53],[Footer54],[Footer55],[Footer56],[Footer57],[Footer61],[Footer62],[Footer63],[Footer64],[Footer65],[Footer66],[Footer67],[Footer71],[Footer72],[Footer73],[Footer74],[Footer75],[Footer76],[Footer77],[ForDeliveryNote],[ForPosDeliveryNote],[Geliefert],[Header],[ImportLogoImageId],[Index_Kunde],[InternalNumber],[ItemsFooter1],[ItemsFooter2],[ItemsHeader],[LanguageId],[LastPageText1],[LastPageText10],[LastPageText11],[LastPageText2],[LastPageText3],[LastPageText4],[LastPageText5],[LastPageText6],[LastPageText7],[LastPageText8],[LastPageText9],[LastUpdateTime],[LastUpdateUserId],[Lieferadresse],[Liefertermin],[Offen],[OrderDate],[OrderNumber],[OrderNumberPO],[OrderTypeId],[PaymentMethod],[PaymentTarget],[PE],[Position],[ShippingMethod],[SummarySum],[SummaryTotal],[SummaryUST],[TotalPrice150],[Unit],[UnitPrice],[UnitTotal],[UST_ID]) VALUES ( "

							+ "@Abladestelle" + i + ","
							+ "@AccountOwner1" + i + ","
							+ "@AccountOwner2" + i + ","
							+ "@AccountOwner3" + i + ","
							+ "@AccountOwner4" + i + ","
							+ "@Address1" + i + ","
							+ "@Address2" + i + ","
							+ "@Address3" + i + ","
							+ "@Address4" + i + ","
							+ "@Amount" + i + ","
							+ "@Article" + i + ","
							+ "@ArtikelCountry" + i + ","
							+ "@ArtikelPrice" + i + ","
							+ "@ArtikelQuantity" + i + ","
							+ "@ArtikelStock" + i + ","
							+ "@ArtikelWeight" + i + ","
							+ "@BasisPrice150" + i + ","
							+ "@Bestellt" + i + ","
							+ "@ClientNumber" + i + ","
							+ "@CompanyLogoImageId" + i + ","
							+ "@Cu_G" + i + ","
							+ "@Cu_Surcharge" + i + ","
							+ "@Cu_Total" + i + ","
							+ "@CustomerDate" + i + ","
							+ "@CustomerNumber" + i + ","
							+ "@DEL" + i + ","
							+ "@Description" + i + ","
							+ "@Designation" + i + ","
							+ "@Designation1" + i + ","
							+ "@Designation2" + i + ","
							+ "@DiscountText1" + i + ","
							+ "@DiscountText2" + i + ","
							+ "@DiscountText3" + i + ","
							+ "@DiscountText4" + i + ","
							+ "@DiscountText5" + i + ","
							+ "@DiscountText6" + i + ","
							+ "@DocumentType" + i + ","
							+ "@FactoringText1" + i + ","
							+ "@FactoringText2" + i + ","
							+ "@FactoringText3" + i + ","
							+ "@FactoringText4" + i + ","
							+ "@FactoringText5" + i + ","
							+ "@FactoringText6" + i + ","
							+ "@Footer11" + i + ","
							+ "@Footer12" + i + ","
							+ "@Footer13" + i + ","
							+ "@Footer14" + i + ","
							+ "@Footer15" + i + ","
							+ "@Footer16" + i + ","
							+ "@Footer17" + i + ","
							+ "@Footer21" + i + ","
							+ "@Footer22" + i + ","
							+ "@Footer23" + i + ","
							+ "@Footer24" + i + ","
							+ "@Footer25" + i + ","
							+ "@Footer26" + i + ","
							+ "@Footer27" + i + ","
							+ "@Footer31" + i + ","
							+ "@Footer32" + i + ","
							+ "@Footer33" + i + ","
							+ "@Footer34" + i + ","
							+ "@Footer35" + i + ","
							+ "@Footer36" + i + ","
							+ "@Footer37" + i + ","
							+ "@Footer41" + i + ","
							+ "@Footer42" + i + ","
							+ "@Footer43" + i + ","
							+ "@Footer44" + i + ","
							+ "@Footer45" + i + ","
							+ "@Footer46" + i + ","
							+ "@Footer47" + i + ","
							+ "@Footer51" + i + ","
							+ "@Footer52" + i + ","
							+ "@Footer53" + i + ","
							+ "@Footer54" + i + ","
							+ "@Footer55" + i + ","
							+ "@Footer56" + i + ","
							+ "@Footer57" + i + ","
							+ "@Footer61" + i + ","
							+ "@Footer62" + i + ","
							+ "@Footer63" + i + ","
							+ "@Footer64" + i + ","
							+ "@Footer65" + i + ","
							+ "@Footer66" + i + ","
							+ "@Footer67" + i + ","
							+ "@Footer71" + i + ","
							+ "@Footer72" + i + ","
							+ "@Footer73" + i + ","
							+ "@Footer74" + i + ","
							+ "@Footer75" + i + ","
							+ "@Footer76" + i + ","
							+ "@Footer77" + i + ","
							+ "@ForDeliveryNote" + i + ","
							+ "@ForPosDeliveryNote" + i + ","
							+ "@Geliefert" + i + ","
							+ "@Header" + i + ","
							+ "@ImportLogoImageId" + i + ","
							+ "@Index_Kunde" + i + ","
							+ "@InternalNumber" + i + ","
							+ "@ItemsFooter1" + i + ","
							+ "@ItemsFooter2" + i + ","
							+ "@ItemsHeader" + i + ","
							+ "@LanguageId" + i + ","
							+ "@LastPageText1" + i + ","
							+ "@LastPageText10" + i + ","
							+ "@LastPageText11" + i + ","
							+ "@LastPageText2" + i + ","
							+ "@LastPageText3" + i + ","
							+ "@LastPageText4" + i + ","
							+ "@LastPageText5" + i + ","
							+ "@LastPageText6" + i + ","
							+ "@LastPageText7" + i + ","
							+ "@LastPageText8" + i + ","
							+ "@LastPageText9" + i + ","
							+ "@LastUpdateTime" + i + ","
							+ "@LastUpdateUserId" + i + ","
							+ "@Lieferadresse" + i + ","
							+ "@Liefertermin" + i + ","
							+ "@Offen" + i + ","
							+ "@OrderDate" + i + ","
							+ "@OrderNumber" + i + ","
							+ "@OrderNumberPO" + i + ","
							+ "@OrderTypeId" + i + ","
							+ "@PaymentMethod" + i + ","
							+ "@PaymentTarget" + i + ","
							+ "@PE" + i + ","
							+ "@Position" + i + ","
							+ "@ShippingMethod" + i + ","
							+ "@SummarySum" + i + ","
							+ "@SummaryTotal" + i + ","
							+ "@SummaryUST" + i + ","
							+ "@TotalPrice150" + i + ","
							+ "@Unit" + i + ","
							+ "@UnitPrice" + i + ","
							+ "@UnitTotal" + i + ","
							+ "@UST_ID" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Abladestelle" + i, item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
						sqlCommand.Parameters.AddWithValue("AccountOwner1" + i, item.AccountOwner1 == null ? (object)DBNull.Value : item.AccountOwner1);
						sqlCommand.Parameters.AddWithValue("AccountOwner2" + i, item.AccountOwner2 == null ? (object)DBNull.Value : item.AccountOwner2);
						sqlCommand.Parameters.AddWithValue("AccountOwner3" + i, item.AccountOwner3 == null ? (object)DBNull.Value : item.AccountOwner3);
						sqlCommand.Parameters.AddWithValue("AccountOwner4" + i, item.AccountOwner4 == null ? (object)DBNull.Value : item.AccountOwner4);
						sqlCommand.Parameters.AddWithValue("Address1" + i, item.Address1 == null ? (object)DBNull.Value : item.Address1);
						sqlCommand.Parameters.AddWithValue("Address2" + i, item.Address2 == null ? (object)DBNull.Value : item.Address2);
						sqlCommand.Parameters.AddWithValue("Address3" + i, item.Address3 == null ? (object)DBNull.Value : item.Address3);
						sqlCommand.Parameters.AddWithValue("Address4" + i, item.Address4 == null ? (object)DBNull.Value : item.Address4);
						sqlCommand.Parameters.AddWithValue("Amount" + i, item.Amount == null ? (object)DBNull.Value : item.Amount);
						sqlCommand.Parameters.AddWithValue("Article" + i, item.Article == null ? (object)DBNull.Value : item.Article);
						sqlCommand.Parameters.AddWithValue("ArtikelCountry" + i, item.ArtikelCountry == null ? (object)DBNull.Value : item.ArtikelCountry);
						sqlCommand.Parameters.AddWithValue("ArtikelPrice" + i, item.ArtikelPrice == null ? (object)DBNull.Value : item.ArtikelPrice);
						sqlCommand.Parameters.AddWithValue("ArtikelQuantity" + i, item.ArtikelQuantity == null ? (object)DBNull.Value : item.ArtikelQuantity);
						sqlCommand.Parameters.AddWithValue("ArtikelStock" + i, item.ArtikelStock == null ? (object)DBNull.Value : item.ArtikelStock);
						sqlCommand.Parameters.AddWithValue("ArtikelWeight" + i, item.ArtikelWeight == null ? (object)DBNull.Value : item.ArtikelWeight);
						sqlCommand.Parameters.AddWithValue("BasisPrice150" + i, item.BasisPrice150 == null ? (object)DBNull.Value : item.BasisPrice150);
						sqlCommand.Parameters.AddWithValue("Bestellt" + i, item.Bestellt == null ? (object)DBNull.Value : item.Bestellt);
						sqlCommand.Parameters.AddWithValue("ClientNumber" + i, item.ClientNumber == null ? (object)DBNull.Value : item.ClientNumber);
						sqlCommand.Parameters.AddWithValue("CompanyLogoImageId" + i, item.CompanyLogoImageId);
						sqlCommand.Parameters.AddWithValue("Cu_G" + i, item.Cu_G == null ? (object)DBNull.Value : item.Cu_G);
						sqlCommand.Parameters.AddWithValue("Cu_Surcharge" + i, item.Cu_Surcharge == null ? (object)DBNull.Value : item.Cu_Surcharge);
						sqlCommand.Parameters.AddWithValue("Cu_Total" + i, item.Cu_Total == null ? (object)DBNull.Value : item.Cu_Total);
						sqlCommand.Parameters.AddWithValue("CustomerDate" + i, item.CustomerDate == null ? (object)DBNull.Value : item.CustomerDate);
						sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
						sqlCommand.Parameters.AddWithValue("DEL" + i, item.DEL == null ? (object)DBNull.Value : item.DEL);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("Designation" + i, item.Designation == null ? (object)DBNull.Value : item.Designation);
						sqlCommand.Parameters.AddWithValue("Designation1" + i, item.Designation1 == null ? (object)DBNull.Value : item.Designation1);
						sqlCommand.Parameters.AddWithValue("Designation2" + i, item.Designation2 == null ? (object)DBNull.Value : item.Designation2);
						sqlCommand.Parameters.AddWithValue("DiscountText1" + i, item.DiscountText1 == null ? (object)DBNull.Value : item.DiscountText1);
						sqlCommand.Parameters.AddWithValue("DiscountText2" + i, item.DiscountText2 == null ? (object)DBNull.Value : item.DiscountText2);
						sqlCommand.Parameters.AddWithValue("DiscountText3" + i, item.DiscountText3 == null ? (object)DBNull.Value : item.DiscountText3);
						sqlCommand.Parameters.AddWithValue("DiscountText4" + i, item.DiscountText4 == null ? (object)DBNull.Value : item.DiscountText4);
						sqlCommand.Parameters.AddWithValue("DiscountText5" + i, item.DiscountText5 == null ? (object)DBNull.Value : item.DiscountText5);
						sqlCommand.Parameters.AddWithValue("DiscountText6" + i, item.DiscountText6 == null ? (object)DBNull.Value : item.DiscountText6);
						sqlCommand.Parameters.AddWithValue("DocumentType" + i, item.DocumentType == null ? (object)DBNull.Value : item.DocumentType);
						sqlCommand.Parameters.AddWithValue("FactoringText1" + i, item.FactoringText1 == null ? (object)DBNull.Value : item.FactoringText1);
						sqlCommand.Parameters.AddWithValue("FactoringText2" + i, item.FactoringText2 == null ? (object)DBNull.Value : item.FactoringText2);
						sqlCommand.Parameters.AddWithValue("FactoringText3" + i, item.FactoringText3 == null ? (object)DBNull.Value : item.FactoringText3);
						sqlCommand.Parameters.AddWithValue("FactoringText4" + i, item.FactoringText4 == null ? (object)DBNull.Value : item.FactoringText4);
						sqlCommand.Parameters.AddWithValue("FactoringText5" + i, item.FactoringText5 == null ? (object)DBNull.Value : item.FactoringText5);
						sqlCommand.Parameters.AddWithValue("FactoringText6" + i, item.FactoringText6 == null ? (object)DBNull.Value : item.FactoringText6);
						sqlCommand.Parameters.AddWithValue("Footer11" + i, item.Footer11 == null ? (object)DBNull.Value : item.Footer11);
						sqlCommand.Parameters.AddWithValue("Footer12" + i, item.Footer12 == null ? (object)DBNull.Value : item.Footer12);
						sqlCommand.Parameters.AddWithValue("Footer13" + i, item.Footer13 == null ? (object)DBNull.Value : item.Footer13);
						sqlCommand.Parameters.AddWithValue("Footer14" + i, item.Footer14 == null ? (object)DBNull.Value : item.Footer14);
						sqlCommand.Parameters.AddWithValue("Footer15" + i, item.Footer15 == null ? (object)DBNull.Value : item.Footer15);
						sqlCommand.Parameters.AddWithValue("Footer16" + i, item.Footer16 == null ? (object)DBNull.Value : item.Footer16);
						sqlCommand.Parameters.AddWithValue("Footer17" + i, item.Footer17 == null ? (object)DBNull.Value : item.Footer17);
						sqlCommand.Parameters.AddWithValue("Footer21" + i, item.Footer21 == null ? (object)DBNull.Value : item.Footer21);
						sqlCommand.Parameters.AddWithValue("Footer22" + i, item.Footer22 == null ? (object)DBNull.Value : item.Footer22);
						sqlCommand.Parameters.AddWithValue("Footer23" + i, item.Footer23 == null ? (object)DBNull.Value : item.Footer23);
						sqlCommand.Parameters.AddWithValue("Footer24" + i, item.Footer24 == null ? (object)DBNull.Value : item.Footer24);
						sqlCommand.Parameters.AddWithValue("Footer25" + i, item.Footer25 == null ? (object)DBNull.Value : item.Footer25);
						sqlCommand.Parameters.AddWithValue("Footer26" + i, item.Footer26 == null ? (object)DBNull.Value : item.Footer26);
						sqlCommand.Parameters.AddWithValue("Footer27" + i, item.Footer27 == null ? (object)DBNull.Value : item.Footer27);
						sqlCommand.Parameters.AddWithValue("Footer31" + i, item.Footer31 == null ? (object)DBNull.Value : item.Footer31);
						sqlCommand.Parameters.AddWithValue("Footer32" + i, item.Footer32 == null ? (object)DBNull.Value : item.Footer32);
						sqlCommand.Parameters.AddWithValue("Footer33" + i, item.Footer33 == null ? (object)DBNull.Value : item.Footer33);
						sqlCommand.Parameters.AddWithValue("Footer34" + i, item.Footer34 == null ? (object)DBNull.Value : item.Footer34);
						sqlCommand.Parameters.AddWithValue("Footer35" + i, item.Footer35 == null ? (object)DBNull.Value : item.Footer35);
						sqlCommand.Parameters.AddWithValue("Footer36" + i, item.Footer36 == null ? (object)DBNull.Value : item.Footer36);
						sqlCommand.Parameters.AddWithValue("Footer37" + i, item.Footer37 == null ? (object)DBNull.Value : item.Footer37);
						sqlCommand.Parameters.AddWithValue("Footer41" + i, item.Footer41 == null ? (object)DBNull.Value : item.Footer41);
						sqlCommand.Parameters.AddWithValue("Footer42" + i, item.Footer42 == null ? (object)DBNull.Value : item.Footer42);
						sqlCommand.Parameters.AddWithValue("Footer43" + i, item.Footer43 == null ? (object)DBNull.Value : item.Footer43);
						sqlCommand.Parameters.AddWithValue("Footer44" + i, item.Footer44 == null ? (object)DBNull.Value : item.Footer44);
						sqlCommand.Parameters.AddWithValue("Footer45" + i, item.Footer45 == null ? (object)DBNull.Value : item.Footer45);
						sqlCommand.Parameters.AddWithValue("Footer46" + i, item.Footer46 == null ? (object)DBNull.Value : item.Footer46);
						sqlCommand.Parameters.AddWithValue("Footer47" + i, item.Footer47 == null ? (object)DBNull.Value : item.Footer47);
						sqlCommand.Parameters.AddWithValue("Footer51" + i, item.Footer51 == null ? (object)DBNull.Value : item.Footer51);
						sqlCommand.Parameters.AddWithValue("Footer52" + i, item.Footer52 == null ? (object)DBNull.Value : item.Footer52);
						sqlCommand.Parameters.AddWithValue("Footer53" + i, item.Footer53 == null ? (object)DBNull.Value : item.Footer53);
						sqlCommand.Parameters.AddWithValue("Footer54" + i, item.Footer54 == null ? (object)DBNull.Value : item.Footer54);
						sqlCommand.Parameters.AddWithValue("Footer55" + i, item.Footer55 == null ? (object)DBNull.Value : item.Footer55);
						sqlCommand.Parameters.AddWithValue("Footer56" + i, item.Footer56 == null ? (object)DBNull.Value : item.Footer56);
						sqlCommand.Parameters.AddWithValue("Footer57" + i, item.Footer57 == null ? (object)DBNull.Value : item.Footer57);
						sqlCommand.Parameters.AddWithValue("Footer61" + i, item.Footer61 == null ? (object)DBNull.Value : item.Footer61);
						sqlCommand.Parameters.AddWithValue("Footer62" + i, item.Footer62 == null ? (object)DBNull.Value : item.Footer62);
						sqlCommand.Parameters.AddWithValue("Footer63" + i, item.Footer63 == null ? (object)DBNull.Value : item.Footer63);
						sqlCommand.Parameters.AddWithValue("Footer64" + i, item.Footer64 == null ? (object)DBNull.Value : item.Footer64);
						sqlCommand.Parameters.AddWithValue("Footer65" + i, item.Footer65 == null ? (object)DBNull.Value : item.Footer65);
						sqlCommand.Parameters.AddWithValue("Footer66" + i, item.Footer66 == null ? (object)DBNull.Value : item.Footer66);
						sqlCommand.Parameters.AddWithValue("Footer67" + i, item.Footer67 == null ? (object)DBNull.Value : item.Footer67);
						sqlCommand.Parameters.AddWithValue("Footer71" + i, item.Footer71 == null ? (object)DBNull.Value : item.Footer71);
						sqlCommand.Parameters.AddWithValue("Footer72" + i, item.Footer72 == null ? (object)DBNull.Value : item.Footer72);
						sqlCommand.Parameters.AddWithValue("Footer73" + i, item.Footer73 == null ? (object)DBNull.Value : item.Footer73);
						sqlCommand.Parameters.AddWithValue("Footer74" + i, item.Footer74 == null ? (object)DBNull.Value : item.Footer74);
						sqlCommand.Parameters.AddWithValue("Footer75" + i, item.Footer75 == null ? (object)DBNull.Value : item.Footer75);
						sqlCommand.Parameters.AddWithValue("Footer76" + i, item.Footer76 == null ? (object)DBNull.Value : item.Footer76);
						sqlCommand.Parameters.AddWithValue("Footer77" + i, item.Footer77 == null ? (object)DBNull.Value : item.Footer77);
						sqlCommand.Parameters.AddWithValue("ForDeliveryNote" + i, item.ForDeliveryNote == null ? (object)DBNull.Value : item.ForDeliveryNote);
						sqlCommand.Parameters.AddWithValue("ForPosDeliveryNote" + i, item.ForPosDeliveryNote == null ? (object)DBNull.Value : item.ForPosDeliveryNote);
						sqlCommand.Parameters.AddWithValue("Geliefert" + i, item.Geliefert == null ? (object)DBNull.Value : item.Geliefert);
						sqlCommand.Parameters.AddWithValue("Header" + i, item.Header == null ? (object)DBNull.Value : item.Header);
						sqlCommand.Parameters.AddWithValue("ImportLogoImageId" + i, item.ImportLogoImageId);
						sqlCommand.Parameters.AddWithValue("Index_Kunde" + i, item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
						sqlCommand.Parameters.AddWithValue("InternalNumber" + i, item.InternalNumber == null ? (object)DBNull.Value : item.InternalNumber);
						sqlCommand.Parameters.AddWithValue("ItemsFooter1" + i, item.ItemsFooter1 == null ? (object)DBNull.Value : item.ItemsFooter1);
						sqlCommand.Parameters.AddWithValue("ItemsFooter2" + i, item.ItemsFooter2 == null ? (object)DBNull.Value : item.ItemsFooter2);
						sqlCommand.Parameters.AddWithValue("ItemsHeader" + i, item.ItemsHeader == null ? (object)DBNull.Value : item.ItemsHeader);
						sqlCommand.Parameters.AddWithValue("LanguageId" + i, item.LanguageId);
						sqlCommand.Parameters.AddWithValue("LastPageText1" + i, item.LastPageText1 == null ? (object)DBNull.Value : item.LastPageText1);
						sqlCommand.Parameters.AddWithValue("LastPageText10" + i, item.LastPageText10 == null ? (object)DBNull.Value : item.LastPageText10);
						sqlCommand.Parameters.AddWithValue("LastPageText11" + i, item.LastPageText11 == null ? (object)DBNull.Value : item.LastPageText11);
						sqlCommand.Parameters.AddWithValue("LastPageText2" + i, item.LastPageText2 == null ? (object)DBNull.Value : item.LastPageText2);
						sqlCommand.Parameters.AddWithValue("LastPageText3" + i, item.LastPageText3 == null ? (object)DBNull.Value : item.LastPageText3);
						sqlCommand.Parameters.AddWithValue("LastPageText4" + i, item.LastPageText4 == null ? (object)DBNull.Value : item.LastPageText4);
						sqlCommand.Parameters.AddWithValue("LastPageText5" + i, item.LastPageText5 == null ? (object)DBNull.Value : item.LastPageText5);
						sqlCommand.Parameters.AddWithValue("LastPageText6" + i, item.LastPageText6 == null ? (object)DBNull.Value : item.LastPageText6);
						sqlCommand.Parameters.AddWithValue("LastPageText7" + i, item.LastPageText7 == null ? (object)DBNull.Value : item.LastPageText7);
						sqlCommand.Parameters.AddWithValue("LastPageText8" + i, item.LastPageText8 == null ? (object)DBNull.Value : item.LastPageText8);
						sqlCommand.Parameters.AddWithValue("LastPageText9" + i, item.LastPageText9 == null ? (object)DBNull.Value : item.LastPageText9);
						sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
						sqlCommand.Parameters.AddWithValue("LastUpdateUserId" + i, item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
						sqlCommand.Parameters.AddWithValue("Lieferadresse" + i, item.Lieferadresse == null ? (object)DBNull.Value : item.Lieferadresse);
						sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
						sqlCommand.Parameters.AddWithValue("Offen" + i, item.Offen == null ? (object)DBNull.Value : item.Offen);
						sqlCommand.Parameters.AddWithValue("OrderDate" + i, item.OrderDate == null ? (object)DBNull.Value : item.OrderDate);
						sqlCommand.Parameters.AddWithValue("OrderNumber" + i, item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
						sqlCommand.Parameters.AddWithValue("OrderNumberPO" + i, item.OrderNumberPO == null ? (object)DBNull.Value : item.OrderNumberPO);
						sqlCommand.Parameters.AddWithValue("OrderTypeId" + i, item.OrderTypeId);
						sqlCommand.Parameters.AddWithValue("PaymentMethod" + i, item.PaymentMethod == null ? (object)DBNull.Value : item.PaymentMethod);
						sqlCommand.Parameters.AddWithValue("PaymentTarget" + i, item.PaymentTarget == null ? (object)DBNull.Value : item.PaymentTarget);
						sqlCommand.Parameters.AddWithValue("PE" + i, item.PE == null ? (object)DBNull.Value : item.PE);
						sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
						sqlCommand.Parameters.AddWithValue("ShippingMethod" + i, item.ShippingMethod == null ? (object)DBNull.Value : item.ShippingMethod);
						sqlCommand.Parameters.AddWithValue("SummarySum" + i, item.SummarySum == null ? (object)DBNull.Value : item.SummarySum);
						sqlCommand.Parameters.AddWithValue("SummaryTotal" + i, item.SummaryTotal == null ? (object)DBNull.Value : item.SummaryTotal);
						sqlCommand.Parameters.AddWithValue("SummaryUST" + i, item.SummaryUST == null ? (object)DBNull.Value : item.SummaryUST);
						sqlCommand.Parameters.AddWithValue("TotalPrice150" + i, item.TotalPrice150 == null ? (object)DBNull.Value : item.TotalPrice150);
						sqlCommand.Parameters.AddWithValue("Unit" + i, item.Unit == null ? (object)DBNull.Value : item.Unit);
						sqlCommand.Parameters.AddWithValue("UnitPrice" + i, item.UnitPrice == null ? (object)DBNull.Value : item.UnitPrice);
						sqlCommand.Parameters.AddWithValue("UnitTotal" + i, item.UnitTotal == null ? (object)DBNull.Value : item.UnitTotal);
						sqlCommand.Parameters.AddWithValue("UST_ID" + i, item.UST_ID == null ? (object)DBNull.Value : item.UST_ID);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__PRS_OrderReport] SET [Abladestelle]=@Abladestelle, [AccountOwner1]=@AccountOwner1, [AccountOwner2]=@AccountOwner2, [AccountOwner3]=@AccountOwner3, [AccountOwner4]=@AccountOwner4, [Address1]=@Address1, [Address2]=@Address2, [Address3]=@Address3, [Address4]=@Address4, [Amount]=@Amount, [Article]=@Article, [ArtikelCountry]=@ArtikelCountry, [ArtikelPrice]=@ArtikelPrice, [ArtikelQuantity]=@ArtikelQuantity, [ArtikelStock]=@ArtikelStock, [ArtikelWeight]=@ArtikelWeight, [BasisPrice150]=@BasisPrice150, [Bestellt]=@Bestellt, [ClientNumber]=@ClientNumber, [CompanyLogoImageId]=@CompanyLogoImageId, [Cu_G]=@Cu_G, [Cu_Surcharge]=@Cu_Surcharge, [Cu_Total]=@Cu_Total, [CustomerDate]=@CustomerDate, [CustomerNumber]=@CustomerNumber, [DEL]=@DEL, [Description]=@Description, [Designation]=@Designation, [Designation1]=@Designation1, [Designation2]=@Designation2, [DiscountText1]=@DiscountText1, [DiscountText2]=@DiscountText2, [DiscountText3]=@DiscountText3, [DiscountText4]=@DiscountText4, [DiscountText5]=@DiscountText5, [DiscountText6]=@DiscountText6, [DocumentType]=@DocumentType, [FactoringText1]=@FactoringText1, [FactoringText2]=@FactoringText2, [FactoringText3]=@FactoringText3, [FactoringText4]=@FactoringText4, [FactoringText5]=@FactoringText5, [FactoringText6]=@FactoringText6, [Footer11]=@Footer11, [Footer12]=@Footer12, [Footer13]=@Footer13, [Footer14]=@Footer14, [Footer15]=@Footer15, [Footer16]=@Footer16, [Footer17]=@Footer17, [Footer21]=@Footer21, [Footer22]=@Footer22, [Footer23]=@Footer23, [Footer24]=@Footer24, [Footer25]=@Footer25, [Footer26]=@Footer26, [Footer27]=@Footer27, [Footer31]=@Footer31, [Footer32]=@Footer32, [Footer33]=@Footer33, [Footer34]=@Footer34, [Footer35]=@Footer35, [Footer36]=@Footer36, [Footer37]=@Footer37, [Footer41]=@Footer41, [Footer42]=@Footer42, [Footer43]=@Footer43, [Footer44]=@Footer44, [Footer45]=@Footer45, [Footer46]=@Footer46, [Footer47]=@Footer47, [Footer51]=@Footer51, [Footer52]=@Footer52, [Footer53]=@Footer53, [Footer54]=@Footer54, [Footer55]=@Footer55, [Footer56]=@Footer56, [Footer57]=@Footer57, [Footer61]=@Footer61, [Footer62]=@Footer62, [Footer63]=@Footer63, [Footer64]=@Footer64, [Footer65]=@Footer65, [Footer66]=@Footer66, [Footer67]=@Footer67, [Footer71]=@Footer71, [Footer72]=@Footer72, [Footer73]=@Footer73, [Footer74]=@Footer74, [Footer75]=@Footer75, [Footer76]=@Footer76, [Footer77]=@Footer77, [ForDeliveryNote]=@ForDeliveryNote, [ForPosDeliveryNote]=@ForPosDeliveryNote, [Geliefert]=@Geliefert, [Header]=@Header, [ImportLogoImageId]=@ImportLogoImageId, [Index_Kunde]=@Index_Kunde, [InternalNumber]=@InternalNumber, [ItemsFooter1]=@ItemsFooter1, [ItemsFooter2]=@ItemsFooter2, [ItemsHeader]=@ItemsHeader, [LanguageId]=@LanguageId, [LastPageText1]=@LastPageText1, [LastPageText10]=@LastPageText10, [LastPageText11]=@LastPageText11, [LastPageText2]=@LastPageText2, [LastPageText3]=@LastPageText3, [LastPageText4]=@LastPageText4, [LastPageText5]=@LastPageText5, [LastPageText6]=@LastPageText6, [LastPageText7]=@LastPageText7, [LastPageText8]=@LastPageText8, [LastPageText9]=@LastPageText9, [LastUpdateTime]=@LastUpdateTime, [LastUpdateUserId]=@LastUpdateUserId, [Lieferadresse]=@Lieferadresse, [Liefertermin]=@Liefertermin, [Offen]=@Offen, [OrderDate]=@OrderDate, [OrderNumber]=@OrderNumber, [OrderNumberPO]=@OrderNumberPO, [OrderTypeId]=@OrderTypeId, [PaymentMethod]=@PaymentMethod, [PaymentTarget]=@PaymentTarget, [PE]=@PE, [Position]=@Position, [ShippingMethod]=@ShippingMethod, [SummarySum]=@SummarySum, [SummaryTotal]=@SummaryTotal, [SummaryUST]=@SummaryUST, [TotalPrice150]=@TotalPrice150, [Unit]=@Unit, [UnitPrice]=@UnitPrice, [UnitTotal]=@UnitTotal, [UST_ID]=@UST_ID WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Abladestelle", item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
				sqlCommand.Parameters.AddWithValue("AccountOwner1", item.AccountOwner1 == null ? (object)DBNull.Value : item.AccountOwner1);
				sqlCommand.Parameters.AddWithValue("AccountOwner2", item.AccountOwner2 == null ? (object)DBNull.Value : item.AccountOwner2);
				sqlCommand.Parameters.AddWithValue("AccountOwner3", item.AccountOwner3 == null ? (object)DBNull.Value : item.AccountOwner3);
				sqlCommand.Parameters.AddWithValue("AccountOwner4", item.AccountOwner4 == null ? (object)DBNull.Value : item.AccountOwner4);
				sqlCommand.Parameters.AddWithValue("Address1", item.Address1 == null ? (object)DBNull.Value : item.Address1);
				sqlCommand.Parameters.AddWithValue("Address2", item.Address2 == null ? (object)DBNull.Value : item.Address2);
				sqlCommand.Parameters.AddWithValue("Address3", item.Address3 == null ? (object)DBNull.Value : item.Address3);
				sqlCommand.Parameters.AddWithValue("Address4", item.Address4 == null ? (object)DBNull.Value : item.Address4);
				sqlCommand.Parameters.AddWithValue("Amount", item.Amount == null ? (object)DBNull.Value : item.Amount);
				sqlCommand.Parameters.AddWithValue("Article", item.Article == null ? (object)DBNull.Value : item.Article);
				sqlCommand.Parameters.AddWithValue("ArtikelCountry", item.ArtikelCountry == null ? (object)DBNull.Value : item.ArtikelCountry);
				sqlCommand.Parameters.AddWithValue("ArtikelPrice", item.ArtikelPrice == null ? (object)DBNull.Value : item.ArtikelPrice);
				sqlCommand.Parameters.AddWithValue("ArtikelQuantity", item.ArtikelQuantity == null ? (object)DBNull.Value : item.ArtikelQuantity);
				sqlCommand.Parameters.AddWithValue("ArtikelStock", item.ArtikelStock == null ? (object)DBNull.Value : item.ArtikelStock);
				sqlCommand.Parameters.AddWithValue("ArtikelWeight", item.ArtikelWeight == null ? (object)DBNull.Value : item.ArtikelWeight);
				sqlCommand.Parameters.AddWithValue("BasisPrice150", item.BasisPrice150 == null ? (object)DBNull.Value : item.BasisPrice150);
				sqlCommand.Parameters.AddWithValue("Bestellt", item.Bestellt == null ? (object)DBNull.Value : item.Bestellt);
				sqlCommand.Parameters.AddWithValue("ClientNumber", item.ClientNumber == null ? (object)DBNull.Value : item.ClientNumber);
				sqlCommand.Parameters.AddWithValue("CompanyLogoImageId", item.CompanyLogoImageId);
				sqlCommand.Parameters.AddWithValue("Cu_G", item.Cu_G == null ? (object)DBNull.Value : item.Cu_G);
				sqlCommand.Parameters.AddWithValue("Cu_Surcharge", item.Cu_Surcharge == null ? (object)DBNull.Value : item.Cu_Surcharge);
				sqlCommand.Parameters.AddWithValue("Cu_Total", item.Cu_Total == null ? (object)DBNull.Value : item.Cu_Total);
				sqlCommand.Parameters.AddWithValue("CustomerDate", item.CustomerDate == null ? (object)DBNull.Value : item.CustomerDate);
				sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
				sqlCommand.Parameters.AddWithValue("DEL", item.DEL == null ? (object)DBNull.Value : item.DEL);
				sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
				sqlCommand.Parameters.AddWithValue("Designation", item.Designation == null ? (object)DBNull.Value : item.Designation);
				sqlCommand.Parameters.AddWithValue("Designation1", item.Designation1 == null ? (object)DBNull.Value : item.Designation1);
				sqlCommand.Parameters.AddWithValue("Designation2", item.Designation2 == null ? (object)DBNull.Value : item.Designation2);
				sqlCommand.Parameters.AddWithValue("DiscountText1", item.DiscountText1 == null ? (object)DBNull.Value : item.DiscountText1);
				sqlCommand.Parameters.AddWithValue("DiscountText2", item.DiscountText2 == null ? (object)DBNull.Value : item.DiscountText2);
				sqlCommand.Parameters.AddWithValue("DiscountText3", item.DiscountText3 == null ? (object)DBNull.Value : item.DiscountText3);
				sqlCommand.Parameters.AddWithValue("DiscountText4", item.DiscountText4 == null ? (object)DBNull.Value : item.DiscountText4);
				sqlCommand.Parameters.AddWithValue("DiscountText5", item.DiscountText5 == null ? (object)DBNull.Value : item.DiscountText5);
				sqlCommand.Parameters.AddWithValue("DiscountText6", item.DiscountText6 == null ? (object)DBNull.Value : item.DiscountText6);
				sqlCommand.Parameters.AddWithValue("DocumentType", item.DocumentType == null ? (object)DBNull.Value : item.DocumentType);
				sqlCommand.Parameters.AddWithValue("FactoringText1", item.FactoringText1 == null ? (object)DBNull.Value : item.FactoringText1);
				sqlCommand.Parameters.AddWithValue("FactoringText2", item.FactoringText2 == null ? (object)DBNull.Value : item.FactoringText2);
				sqlCommand.Parameters.AddWithValue("FactoringText3", item.FactoringText3 == null ? (object)DBNull.Value : item.FactoringText3);
				sqlCommand.Parameters.AddWithValue("FactoringText4", item.FactoringText4 == null ? (object)DBNull.Value : item.FactoringText4);
				sqlCommand.Parameters.AddWithValue("FactoringText5", item.FactoringText5 == null ? (object)DBNull.Value : item.FactoringText5);
				sqlCommand.Parameters.AddWithValue("FactoringText6", item.FactoringText6 == null ? (object)DBNull.Value : item.FactoringText6);
				sqlCommand.Parameters.AddWithValue("Footer11", item.Footer11 == null ? (object)DBNull.Value : item.Footer11);
				sqlCommand.Parameters.AddWithValue("Footer12", item.Footer12 == null ? (object)DBNull.Value : item.Footer12);
				sqlCommand.Parameters.AddWithValue("Footer13", item.Footer13 == null ? (object)DBNull.Value : item.Footer13);
				sqlCommand.Parameters.AddWithValue("Footer14", item.Footer14 == null ? (object)DBNull.Value : item.Footer14);
				sqlCommand.Parameters.AddWithValue("Footer15", item.Footer15 == null ? (object)DBNull.Value : item.Footer15);
				sqlCommand.Parameters.AddWithValue("Footer16", item.Footer16 == null ? (object)DBNull.Value : item.Footer16);
				sqlCommand.Parameters.AddWithValue("Footer17", item.Footer17 == null ? (object)DBNull.Value : item.Footer17);
				sqlCommand.Parameters.AddWithValue("Footer21", item.Footer21 == null ? (object)DBNull.Value : item.Footer21);
				sqlCommand.Parameters.AddWithValue("Footer22", item.Footer22 == null ? (object)DBNull.Value : item.Footer22);
				sqlCommand.Parameters.AddWithValue("Footer23", item.Footer23 == null ? (object)DBNull.Value : item.Footer23);
				sqlCommand.Parameters.AddWithValue("Footer24", item.Footer24 == null ? (object)DBNull.Value : item.Footer24);
				sqlCommand.Parameters.AddWithValue("Footer25", item.Footer25 == null ? (object)DBNull.Value : item.Footer25);
				sqlCommand.Parameters.AddWithValue("Footer26", item.Footer26 == null ? (object)DBNull.Value : item.Footer26);
				sqlCommand.Parameters.AddWithValue("Footer27", item.Footer27 == null ? (object)DBNull.Value : item.Footer27);
				sqlCommand.Parameters.AddWithValue("Footer31", item.Footer31 == null ? (object)DBNull.Value : item.Footer31);
				sqlCommand.Parameters.AddWithValue("Footer32", item.Footer32 == null ? (object)DBNull.Value : item.Footer32);
				sqlCommand.Parameters.AddWithValue("Footer33", item.Footer33 == null ? (object)DBNull.Value : item.Footer33);
				sqlCommand.Parameters.AddWithValue("Footer34", item.Footer34 == null ? (object)DBNull.Value : item.Footer34);
				sqlCommand.Parameters.AddWithValue("Footer35", item.Footer35 == null ? (object)DBNull.Value : item.Footer35);
				sqlCommand.Parameters.AddWithValue("Footer36", item.Footer36 == null ? (object)DBNull.Value : item.Footer36);
				sqlCommand.Parameters.AddWithValue("Footer37", item.Footer37 == null ? (object)DBNull.Value : item.Footer37);
				sqlCommand.Parameters.AddWithValue("Footer41", item.Footer41 == null ? (object)DBNull.Value : item.Footer41);
				sqlCommand.Parameters.AddWithValue("Footer42", item.Footer42 == null ? (object)DBNull.Value : item.Footer42);
				sqlCommand.Parameters.AddWithValue("Footer43", item.Footer43 == null ? (object)DBNull.Value : item.Footer43);
				sqlCommand.Parameters.AddWithValue("Footer44", item.Footer44 == null ? (object)DBNull.Value : item.Footer44);
				sqlCommand.Parameters.AddWithValue("Footer45", item.Footer45 == null ? (object)DBNull.Value : item.Footer45);
				sqlCommand.Parameters.AddWithValue("Footer46", item.Footer46 == null ? (object)DBNull.Value : item.Footer46);
				sqlCommand.Parameters.AddWithValue("Footer47", item.Footer47 == null ? (object)DBNull.Value : item.Footer47);
				sqlCommand.Parameters.AddWithValue("Footer51", item.Footer51 == null ? (object)DBNull.Value : item.Footer51);
				sqlCommand.Parameters.AddWithValue("Footer52", item.Footer52 == null ? (object)DBNull.Value : item.Footer52);
				sqlCommand.Parameters.AddWithValue("Footer53", item.Footer53 == null ? (object)DBNull.Value : item.Footer53);
				sqlCommand.Parameters.AddWithValue("Footer54", item.Footer54 == null ? (object)DBNull.Value : item.Footer54);
				sqlCommand.Parameters.AddWithValue("Footer55", item.Footer55 == null ? (object)DBNull.Value : item.Footer55);
				sqlCommand.Parameters.AddWithValue("Footer56", item.Footer56 == null ? (object)DBNull.Value : item.Footer56);
				sqlCommand.Parameters.AddWithValue("Footer57", item.Footer57 == null ? (object)DBNull.Value : item.Footer57);
				sqlCommand.Parameters.AddWithValue("Footer61", item.Footer61 == null ? (object)DBNull.Value : item.Footer61);
				sqlCommand.Parameters.AddWithValue("Footer62", item.Footer62 == null ? (object)DBNull.Value : item.Footer62);
				sqlCommand.Parameters.AddWithValue("Footer63", item.Footer63 == null ? (object)DBNull.Value : item.Footer63);
				sqlCommand.Parameters.AddWithValue("Footer64", item.Footer64 == null ? (object)DBNull.Value : item.Footer64);
				sqlCommand.Parameters.AddWithValue("Footer65", item.Footer65 == null ? (object)DBNull.Value : item.Footer65);
				sqlCommand.Parameters.AddWithValue("Footer66", item.Footer66 == null ? (object)DBNull.Value : item.Footer66);
				sqlCommand.Parameters.AddWithValue("Footer67", item.Footer67 == null ? (object)DBNull.Value : item.Footer67);
				sqlCommand.Parameters.AddWithValue("Footer71", item.Footer71 == null ? (object)DBNull.Value : item.Footer71);
				sqlCommand.Parameters.AddWithValue("Footer72", item.Footer72 == null ? (object)DBNull.Value : item.Footer72);
				sqlCommand.Parameters.AddWithValue("Footer73", item.Footer73 == null ? (object)DBNull.Value : item.Footer73);
				sqlCommand.Parameters.AddWithValue("Footer74", item.Footer74 == null ? (object)DBNull.Value : item.Footer74);
				sqlCommand.Parameters.AddWithValue("Footer75", item.Footer75 == null ? (object)DBNull.Value : item.Footer75);
				sqlCommand.Parameters.AddWithValue("Footer76", item.Footer76 == null ? (object)DBNull.Value : item.Footer76);
				sqlCommand.Parameters.AddWithValue("Footer77", item.Footer77 == null ? (object)DBNull.Value : item.Footer77);
				sqlCommand.Parameters.AddWithValue("ForDeliveryNote", item.ForDeliveryNote == null ? (object)DBNull.Value : item.ForDeliveryNote);
				sqlCommand.Parameters.AddWithValue("ForPosDeliveryNote", item.ForPosDeliveryNote == null ? (object)DBNull.Value : item.ForPosDeliveryNote);
				sqlCommand.Parameters.AddWithValue("Geliefert", item.Geliefert == null ? (object)DBNull.Value : item.Geliefert);
				sqlCommand.Parameters.AddWithValue("Header", item.Header == null ? (object)DBNull.Value : item.Header);
				sqlCommand.Parameters.AddWithValue("ImportLogoImageId", item.ImportLogoImageId);
				sqlCommand.Parameters.AddWithValue("Index_Kunde", item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
				sqlCommand.Parameters.AddWithValue("InternalNumber", item.InternalNumber == null ? (object)DBNull.Value : item.InternalNumber);
				sqlCommand.Parameters.AddWithValue("ItemsFooter1", item.ItemsFooter1 == null ? (object)DBNull.Value : item.ItemsFooter1);
				sqlCommand.Parameters.AddWithValue("ItemsFooter2", item.ItemsFooter2 == null ? (object)DBNull.Value : item.ItemsFooter2);
				sqlCommand.Parameters.AddWithValue("ItemsHeader", item.ItemsHeader == null ? (object)DBNull.Value : item.ItemsHeader);
				sqlCommand.Parameters.AddWithValue("LanguageId", item.LanguageId);
				sqlCommand.Parameters.AddWithValue("LastPageText1", item.LastPageText1 == null ? (object)DBNull.Value : item.LastPageText1);
				sqlCommand.Parameters.AddWithValue("LastPageText10", item.LastPageText10 == null ? (object)DBNull.Value : item.LastPageText10);
				sqlCommand.Parameters.AddWithValue("LastPageText11", item.LastPageText11 == null ? (object)DBNull.Value : item.LastPageText11);
				sqlCommand.Parameters.AddWithValue("LastPageText2", item.LastPageText2 == null ? (object)DBNull.Value : item.LastPageText2);
				sqlCommand.Parameters.AddWithValue("LastPageText3", item.LastPageText3 == null ? (object)DBNull.Value : item.LastPageText3);
				sqlCommand.Parameters.AddWithValue("LastPageText4", item.LastPageText4 == null ? (object)DBNull.Value : item.LastPageText4);
				sqlCommand.Parameters.AddWithValue("LastPageText5", item.LastPageText5 == null ? (object)DBNull.Value : item.LastPageText5);
				sqlCommand.Parameters.AddWithValue("LastPageText6", item.LastPageText6 == null ? (object)DBNull.Value : item.LastPageText6);
				sqlCommand.Parameters.AddWithValue("LastPageText7", item.LastPageText7 == null ? (object)DBNull.Value : item.LastPageText7);
				sqlCommand.Parameters.AddWithValue("LastPageText8", item.LastPageText8 == null ? (object)DBNull.Value : item.LastPageText8);
				sqlCommand.Parameters.AddWithValue("LastPageText9", item.LastPageText9 == null ? (object)DBNull.Value : item.LastPageText9);
				sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
				sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
				sqlCommand.Parameters.AddWithValue("Lieferadresse", item.Lieferadresse == null ? (object)DBNull.Value : item.Lieferadresse);
				sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
				sqlCommand.Parameters.AddWithValue("Offen", item.Offen == null ? (object)DBNull.Value : item.Offen);
				sqlCommand.Parameters.AddWithValue("OrderDate", item.OrderDate == null ? (object)DBNull.Value : item.OrderDate);
				sqlCommand.Parameters.AddWithValue("OrderNumber", item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
				sqlCommand.Parameters.AddWithValue("OrderNumberPO", item.OrderNumberPO == null ? (object)DBNull.Value : item.OrderNumberPO);
				sqlCommand.Parameters.AddWithValue("OrderTypeId", item.OrderTypeId);
				sqlCommand.Parameters.AddWithValue("PaymentMethod", item.PaymentMethod == null ? (object)DBNull.Value : item.PaymentMethod);
				sqlCommand.Parameters.AddWithValue("PaymentTarget", item.PaymentTarget == null ? (object)DBNull.Value : item.PaymentTarget);
				sqlCommand.Parameters.AddWithValue("PE", item.PE == null ? (object)DBNull.Value : item.PE);
				sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
				sqlCommand.Parameters.AddWithValue("ShippingMethod", item.ShippingMethod == null ? (object)DBNull.Value : item.ShippingMethod);
				sqlCommand.Parameters.AddWithValue("SummarySum", item.SummarySum == null ? (object)DBNull.Value : item.SummarySum);
				sqlCommand.Parameters.AddWithValue("SummaryTotal", item.SummaryTotal == null ? (object)DBNull.Value : item.SummaryTotal);
				sqlCommand.Parameters.AddWithValue("SummaryUST", item.SummaryUST == null ? (object)DBNull.Value : item.SummaryUST);
				sqlCommand.Parameters.AddWithValue("TotalPrice150", item.TotalPrice150 == null ? (object)DBNull.Value : item.TotalPrice150);
				sqlCommand.Parameters.AddWithValue("Unit", item.Unit == null ? (object)DBNull.Value : item.Unit);
				sqlCommand.Parameters.AddWithValue("UnitPrice", item.UnitPrice == null ? (object)DBNull.Value : item.UnitPrice);
				sqlCommand.Parameters.AddWithValue("UnitTotal", item.UnitTotal == null ? (object)DBNull.Value : item.UnitTotal);
				sqlCommand.Parameters.AddWithValue("UST_ID", item.UST_ID == null ? (object)DBNull.Value : item.UST_ID);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 137; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity> items)
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
						query += " UPDATE [__PRS_OrderReport] SET "

							+ "[Abladestelle]=@Abladestelle" + i + ","
							+ "[AccountOwner1]=@AccountOwner1" + i + ","
							+ "[AccountOwner2]=@AccountOwner2" + i + ","
							+ "[AccountOwner3]=@AccountOwner3" + i + ","
							+ "[AccountOwner4]=@AccountOwner4" + i + ","
							+ "[Address1]=@Address1" + i + ","
							+ "[Address2]=@Address2" + i + ","
							+ "[Address3]=@Address3" + i + ","
							+ "[Address4]=@Address4" + i + ","
							+ "[Amount]=@Amount" + i + ","
							+ "[Article]=@Article" + i + ","
							+ "[ArtikelCountry]=@ArtikelCountry" + i + ","
							+ "[ArtikelPrice]=@ArtikelPrice" + i + ","
							+ "[ArtikelQuantity]=@ArtikelQuantity" + i + ","
							+ "[ArtikelStock]=@ArtikelStock" + i + ","
							+ "[ArtikelWeight]=@ArtikelWeight" + i + ","
							+ "[BasisPrice150]=@BasisPrice150" + i + ","
							+ "[Bestellt]=@Bestellt" + i + ","
							+ "[ClientNumber]=@ClientNumber" + i + ","
							+ "[CompanyLogoImageId]=@CompanyLogoImageId" + i + ","
							+ "[Cu_G]=@Cu_G" + i + ","
							+ "[Cu_Surcharge]=@Cu_Surcharge" + i + ","
							+ "[Cu_Total]=@Cu_Total" + i + ","
							+ "[CustomerDate]=@CustomerDate" + i + ","
							+ "[CustomerNumber]=@CustomerNumber" + i + ","
							+ "[DEL]=@DEL" + i + ","
							+ "[Description]=@Description" + i + ","
							+ "[Designation]=@Designation" + i + ","
							+ "[Designation1]=@Designation1" + i + ","
							+ "[Designation2]=@Designation2" + i + ","
							+ "[DiscountText1]=@DiscountText1" + i + ","
							+ "[DiscountText2]=@DiscountText2" + i + ","
							+ "[DiscountText3]=@DiscountText3" + i + ","
							+ "[DiscountText4]=@DiscountText4" + i + ","
							+ "[DiscountText5]=@DiscountText5" + i + ","
							+ "[DiscountText6]=@DiscountText6" + i + ","
							+ "[DocumentType]=@DocumentType" + i + ","
							+ "[FactoringText1]=@FactoringText1" + i + ","
							+ "[FactoringText2]=@FactoringText2" + i + ","
							+ "[FactoringText3]=@FactoringText3" + i + ","
							+ "[FactoringText4]=@FactoringText4" + i + ","
							+ "[FactoringText5]=@FactoringText5" + i + ","
							+ "[FactoringText6]=@FactoringText6" + i + ","
							+ "[Footer11]=@Footer11" + i + ","
							+ "[Footer12]=@Footer12" + i + ","
							+ "[Footer13]=@Footer13" + i + ","
							+ "[Footer14]=@Footer14" + i + ","
							+ "[Footer15]=@Footer15" + i + ","
							+ "[Footer16]=@Footer16" + i + ","
							+ "[Footer17]=@Footer17" + i + ","
							+ "[Footer21]=@Footer21" + i + ","
							+ "[Footer22]=@Footer22" + i + ","
							+ "[Footer23]=@Footer23" + i + ","
							+ "[Footer24]=@Footer24" + i + ","
							+ "[Footer25]=@Footer25" + i + ","
							+ "[Footer26]=@Footer26" + i + ","
							+ "[Footer27]=@Footer27" + i + ","
							+ "[Footer31]=@Footer31" + i + ","
							+ "[Footer32]=@Footer32" + i + ","
							+ "[Footer33]=@Footer33" + i + ","
							+ "[Footer34]=@Footer34" + i + ","
							+ "[Footer35]=@Footer35" + i + ","
							+ "[Footer36]=@Footer36" + i + ","
							+ "[Footer37]=@Footer37" + i + ","
							+ "[Footer41]=@Footer41" + i + ","
							+ "[Footer42]=@Footer42" + i + ","
							+ "[Footer43]=@Footer43" + i + ","
							+ "[Footer44]=@Footer44" + i + ","
							+ "[Footer45]=@Footer45" + i + ","
							+ "[Footer46]=@Footer46" + i + ","
							+ "[Footer47]=@Footer47" + i + ","
							+ "[Footer51]=@Footer51" + i + ","
							+ "[Footer52]=@Footer52" + i + ","
							+ "[Footer53]=@Footer53" + i + ","
							+ "[Footer54]=@Footer54" + i + ","
							+ "[Footer55]=@Footer55" + i + ","
							+ "[Footer56]=@Footer56" + i + ","
							+ "[Footer57]=@Footer57" + i + ","
							+ "[Footer61]=@Footer61" + i + ","
							+ "[Footer62]=@Footer62" + i + ","
							+ "[Footer63]=@Footer63" + i + ","
							+ "[Footer64]=@Footer64" + i + ","
							+ "[Footer65]=@Footer65" + i + ","
							+ "[Footer66]=@Footer66" + i + ","
							+ "[Footer67]=@Footer67" + i + ","
							+ "[Footer71]=@Footer71" + i + ","
							+ "[Footer72]=@Footer72" + i + ","
							+ "[Footer73]=@Footer73" + i + ","
							+ "[Footer74]=@Footer74" + i + ","
							+ "[Footer75]=@Footer75" + i + ","
							+ "[Footer76]=@Footer76" + i + ","
							+ "[Footer77]=@Footer77" + i + ","
							+ "[ForDeliveryNote]=@ForDeliveryNote" + i + ","
							+ "[ForPosDeliveryNote]=@ForPosDeliveryNote" + i + ","
							+ "[Geliefert]=@Geliefert" + i + ","
							+ "[Header]=@Header" + i + ","
							+ "[ImportLogoImageId]=@ImportLogoImageId" + i + ","
							+ "[Index_Kunde]=@Index_Kunde" + i + ","
							+ "[InternalNumber]=@InternalNumber" + i + ","
							+ "[ItemsFooter1]=@ItemsFooter1" + i + ","
							+ "[ItemsFooter2]=@ItemsFooter2" + i + ","
							+ "[ItemsHeader]=@ItemsHeader" + i + ","
							+ "[LanguageId]=@LanguageId" + i + ","
							+ "[LastPageText1]=@LastPageText1" + i + ","
							+ "[LastPageText10]=@LastPageText10" + i + ","
							+ "[LastPageText11]=@LastPageText11" + i + ","
							+ "[LastPageText2]=@LastPageText2" + i + ","
							+ "[LastPageText3]=@LastPageText3" + i + ","
							+ "[LastPageText4]=@LastPageText4" + i + ","
							+ "[LastPageText5]=@LastPageText5" + i + ","
							+ "[LastPageText6]=@LastPageText6" + i + ","
							+ "[LastPageText7]=@LastPageText7" + i + ","
							+ "[LastPageText8]=@LastPageText8" + i + ","
							+ "[LastPageText9]=@LastPageText9" + i + ","
							+ "[LastUpdateTime]=@LastUpdateTime" + i + ","
							+ "[LastUpdateUserId]=@LastUpdateUserId" + i + ","
							+ "[Lieferadresse]=@Lieferadresse" + i + ","
							+ "[Liefertermin]=@Liefertermin" + i + ","
							+ "[Offen]=@Offen" + i + ","
							+ "[OrderDate]=@OrderDate" + i + ","
							+ "[OrderNumber]=@OrderNumber" + i + ","
							+ "[OrderNumberPO]=@OrderNumberPO" + i + ","
							+ "[OrderTypeId]=@OrderTypeId" + i + ","
							+ "[PaymentMethod]=@PaymentMethod" + i + ","
							+ "[PaymentTarget]=@PaymentTarget" + i + ","
							+ "[PE]=@PE" + i + ","
							+ "[Position]=@Position" + i + ","
							+ "[ShippingMethod]=@ShippingMethod" + i + ","
							+ "[SummarySum]=@SummarySum" + i + ","
							+ "[SummaryTotal]=@SummaryTotal" + i + ","
							+ "[SummaryUST]=@SummaryUST" + i + ","
							+ "[TotalPrice150]=@TotalPrice150" + i + ","
							+ "[Unit]=@Unit" + i + ","
							+ "[UnitPrice]=@UnitPrice" + i + ","
							+ "[UnitTotal]=@UnitTotal" + i + ","
							+ "[UST_ID]=@UST_ID" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("Abladestelle" + i, item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
						sqlCommand.Parameters.AddWithValue("AccountOwner1" + i, item.AccountOwner1 == null ? (object)DBNull.Value : item.AccountOwner1);
						sqlCommand.Parameters.AddWithValue("AccountOwner2" + i, item.AccountOwner2 == null ? (object)DBNull.Value : item.AccountOwner2);
						sqlCommand.Parameters.AddWithValue("AccountOwner3" + i, item.AccountOwner3 == null ? (object)DBNull.Value : item.AccountOwner3);
						sqlCommand.Parameters.AddWithValue("AccountOwner4" + i, item.AccountOwner4 == null ? (object)DBNull.Value : item.AccountOwner4);
						sqlCommand.Parameters.AddWithValue("Address1" + i, item.Address1 == null ? (object)DBNull.Value : item.Address1);
						sqlCommand.Parameters.AddWithValue("Address2" + i, item.Address2 == null ? (object)DBNull.Value : item.Address2);
						sqlCommand.Parameters.AddWithValue("Address3" + i, item.Address3 == null ? (object)DBNull.Value : item.Address3);
						sqlCommand.Parameters.AddWithValue("Address4" + i, item.Address4 == null ? (object)DBNull.Value : item.Address4);
						sqlCommand.Parameters.AddWithValue("Amount" + i, item.Amount == null ? (object)DBNull.Value : item.Amount);
						sqlCommand.Parameters.AddWithValue("Article" + i, item.Article == null ? (object)DBNull.Value : item.Article);
						sqlCommand.Parameters.AddWithValue("ArtikelCountry" + i, item.ArtikelCountry == null ? (object)DBNull.Value : item.ArtikelCountry);
						sqlCommand.Parameters.AddWithValue("ArtikelPrice" + i, item.ArtikelPrice == null ? (object)DBNull.Value : item.ArtikelPrice);
						sqlCommand.Parameters.AddWithValue("ArtikelQuantity" + i, item.ArtikelQuantity == null ? (object)DBNull.Value : item.ArtikelQuantity);
						sqlCommand.Parameters.AddWithValue("ArtikelStock" + i, item.ArtikelStock == null ? (object)DBNull.Value : item.ArtikelStock);
						sqlCommand.Parameters.AddWithValue("ArtikelWeight" + i, item.ArtikelWeight == null ? (object)DBNull.Value : item.ArtikelWeight);
						sqlCommand.Parameters.AddWithValue("BasisPrice150" + i, item.BasisPrice150 == null ? (object)DBNull.Value : item.BasisPrice150);
						sqlCommand.Parameters.AddWithValue("Bestellt" + i, item.Bestellt == null ? (object)DBNull.Value : item.Bestellt);
						sqlCommand.Parameters.AddWithValue("ClientNumber" + i, item.ClientNumber == null ? (object)DBNull.Value : item.ClientNumber);
						sqlCommand.Parameters.AddWithValue("CompanyLogoImageId" + i, item.CompanyLogoImageId);
						sqlCommand.Parameters.AddWithValue("Cu_G" + i, item.Cu_G == null ? (object)DBNull.Value : item.Cu_G);
						sqlCommand.Parameters.AddWithValue("Cu_Surcharge" + i, item.Cu_Surcharge == null ? (object)DBNull.Value : item.Cu_Surcharge);
						sqlCommand.Parameters.AddWithValue("Cu_Total" + i, item.Cu_Total == null ? (object)DBNull.Value : item.Cu_Total);
						sqlCommand.Parameters.AddWithValue("CustomerDate" + i, item.CustomerDate == null ? (object)DBNull.Value : item.CustomerDate);
						sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
						sqlCommand.Parameters.AddWithValue("DEL" + i, item.DEL == null ? (object)DBNull.Value : item.DEL);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("Designation" + i, item.Designation == null ? (object)DBNull.Value : item.Designation);
						sqlCommand.Parameters.AddWithValue("Designation1" + i, item.Designation1 == null ? (object)DBNull.Value : item.Designation1);
						sqlCommand.Parameters.AddWithValue("Designation2" + i, item.Designation2 == null ? (object)DBNull.Value : item.Designation2);
						sqlCommand.Parameters.AddWithValue("DiscountText1" + i, item.DiscountText1 == null ? (object)DBNull.Value : item.DiscountText1);
						sqlCommand.Parameters.AddWithValue("DiscountText2" + i, item.DiscountText2 == null ? (object)DBNull.Value : item.DiscountText2);
						sqlCommand.Parameters.AddWithValue("DiscountText3" + i, item.DiscountText3 == null ? (object)DBNull.Value : item.DiscountText3);
						sqlCommand.Parameters.AddWithValue("DiscountText4" + i, item.DiscountText4 == null ? (object)DBNull.Value : item.DiscountText4);
						sqlCommand.Parameters.AddWithValue("DiscountText5" + i, item.DiscountText5 == null ? (object)DBNull.Value : item.DiscountText5);
						sqlCommand.Parameters.AddWithValue("DiscountText6" + i, item.DiscountText6 == null ? (object)DBNull.Value : item.DiscountText6);
						sqlCommand.Parameters.AddWithValue("DocumentType" + i, item.DocumentType == null ? (object)DBNull.Value : item.DocumentType);
						sqlCommand.Parameters.AddWithValue("FactoringText1" + i, item.FactoringText1 == null ? (object)DBNull.Value : item.FactoringText1);
						sqlCommand.Parameters.AddWithValue("FactoringText2" + i, item.FactoringText2 == null ? (object)DBNull.Value : item.FactoringText2);
						sqlCommand.Parameters.AddWithValue("FactoringText3" + i, item.FactoringText3 == null ? (object)DBNull.Value : item.FactoringText3);
						sqlCommand.Parameters.AddWithValue("FactoringText4" + i, item.FactoringText4 == null ? (object)DBNull.Value : item.FactoringText4);
						sqlCommand.Parameters.AddWithValue("FactoringText5" + i, item.FactoringText5 == null ? (object)DBNull.Value : item.FactoringText5);
						sqlCommand.Parameters.AddWithValue("FactoringText6" + i, item.FactoringText6 == null ? (object)DBNull.Value : item.FactoringText6);
						sqlCommand.Parameters.AddWithValue("Footer11" + i, item.Footer11 == null ? (object)DBNull.Value : item.Footer11);
						sqlCommand.Parameters.AddWithValue("Footer12" + i, item.Footer12 == null ? (object)DBNull.Value : item.Footer12);
						sqlCommand.Parameters.AddWithValue("Footer13" + i, item.Footer13 == null ? (object)DBNull.Value : item.Footer13);
						sqlCommand.Parameters.AddWithValue("Footer14" + i, item.Footer14 == null ? (object)DBNull.Value : item.Footer14);
						sqlCommand.Parameters.AddWithValue("Footer15" + i, item.Footer15 == null ? (object)DBNull.Value : item.Footer15);
						sqlCommand.Parameters.AddWithValue("Footer16" + i, item.Footer16 == null ? (object)DBNull.Value : item.Footer16);
						sqlCommand.Parameters.AddWithValue("Footer17" + i, item.Footer17 == null ? (object)DBNull.Value : item.Footer17);
						sqlCommand.Parameters.AddWithValue("Footer21" + i, item.Footer21 == null ? (object)DBNull.Value : item.Footer21);
						sqlCommand.Parameters.AddWithValue("Footer22" + i, item.Footer22 == null ? (object)DBNull.Value : item.Footer22);
						sqlCommand.Parameters.AddWithValue("Footer23" + i, item.Footer23 == null ? (object)DBNull.Value : item.Footer23);
						sqlCommand.Parameters.AddWithValue("Footer24" + i, item.Footer24 == null ? (object)DBNull.Value : item.Footer24);
						sqlCommand.Parameters.AddWithValue("Footer25" + i, item.Footer25 == null ? (object)DBNull.Value : item.Footer25);
						sqlCommand.Parameters.AddWithValue("Footer26" + i, item.Footer26 == null ? (object)DBNull.Value : item.Footer26);
						sqlCommand.Parameters.AddWithValue("Footer27" + i, item.Footer27 == null ? (object)DBNull.Value : item.Footer27);
						sqlCommand.Parameters.AddWithValue("Footer31" + i, item.Footer31 == null ? (object)DBNull.Value : item.Footer31);
						sqlCommand.Parameters.AddWithValue("Footer32" + i, item.Footer32 == null ? (object)DBNull.Value : item.Footer32);
						sqlCommand.Parameters.AddWithValue("Footer33" + i, item.Footer33 == null ? (object)DBNull.Value : item.Footer33);
						sqlCommand.Parameters.AddWithValue("Footer34" + i, item.Footer34 == null ? (object)DBNull.Value : item.Footer34);
						sqlCommand.Parameters.AddWithValue("Footer35" + i, item.Footer35 == null ? (object)DBNull.Value : item.Footer35);
						sqlCommand.Parameters.AddWithValue("Footer36" + i, item.Footer36 == null ? (object)DBNull.Value : item.Footer36);
						sqlCommand.Parameters.AddWithValue("Footer37" + i, item.Footer37 == null ? (object)DBNull.Value : item.Footer37);
						sqlCommand.Parameters.AddWithValue("Footer41" + i, item.Footer41 == null ? (object)DBNull.Value : item.Footer41);
						sqlCommand.Parameters.AddWithValue("Footer42" + i, item.Footer42 == null ? (object)DBNull.Value : item.Footer42);
						sqlCommand.Parameters.AddWithValue("Footer43" + i, item.Footer43 == null ? (object)DBNull.Value : item.Footer43);
						sqlCommand.Parameters.AddWithValue("Footer44" + i, item.Footer44 == null ? (object)DBNull.Value : item.Footer44);
						sqlCommand.Parameters.AddWithValue("Footer45" + i, item.Footer45 == null ? (object)DBNull.Value : item.Footer45);
						sqlCommand.Parameters.AddWithValue("Footer46" + i, item.Footer46 == null ? (object)DBNull.Value : item.Footer46);
						sqlCommand.Parameters.AddWithValue("Footer47" + i, item.Footer47 == null ? (object)DBNull.Value : item.Footer47);
						sqlCommand.Parameters.AddWithValue("Footer51" + i, item.Footer51 == null ? (object)DBNull.Value : item.Footer51);
						sqlCommand.Parameters.AddWithValue("Footer52" + i, item.Footer52 == null ? (object)DBNull.Value : item.Footer52);
						sqlCommand.Parameters.AddWithValue("Footer53" + i, item.Footer53 == null ? (object)DBNull.Value : item.Footer53);
						sqlCommand.Parameters.AddWithValue("Footer54" + i, item.Footer54 == null ? (object)DBNull.Value : item.Footer54);
						sqlCommand.Parameters.AddWithValue("Footer55" + i, item.Footer55 == null ? (object)DBNull.Value : item.Footer55);
						sqlCommand.Parameters.AddWithValue("Footer56" + i, item.Footer56 == null ? (object)DBNull.Value : item.Footer56);
						sqlCommand.Parameters.AddWithValue("Footer57" + i, item.Footer57 == null ? (object)DBNull.Value : item.Footer57);
						sqlCommand.Parameters.AddWithValue("Footer61" + i, item.Footer61 == null ? (object)DBNull.Value : item.Footer61);
						sqlCommand.Parameters.AddWithValue("Footer62" + i, item.Footer62 == null ? (object)DBNull.Value : item.Footer62);
						sqlCommand.Parameters.AddWithValue("Footer63" + i, item.Footer63 == null ? (object)DBNull.Value : item.Footer63);
						sqlCommand.Parameters.AddWithValue("Footer64" + i, item.Footer64 == null ? (object)DBNull.Value : item.Footer64);
						sqlCommand.Parameters.AddWithValue("Footer65" + i, item.Footer65 == null ? (object)DBNull.Value : item.Footer65);
						sqlCommand.Parameters.AddWithValue("Footer66" + i, item.Footer66 == null ? (object)DBNull.Value : item.Footer66);
						sqlCommand.Parameters.AddWithValue("Footer67" + i, item.Footer67 == null ? (object)DBNull.Value : item.Footer67);
						sqlCommand.Parameters.AddWithValue("Footer71" + i, item.Footer71 == null ? (object)DBNull.Value : item.Footer71);
						sqlCommand.Parameters.AddWithValue("Footer72" + i, item.Footer72 == null ? (object)DBNull.Value : item.Footer72);
						sqlCommand.Parameters.AddWithValue("Footer73" + i, item.Footer73 == null ? (object)DBNull.Value : item.Footer73);
						sqlCommand.Parameters.AddWithValue("Footer74" + i, item.Footer74 == null ? (object)DBNull.Value : item.Footer74);
						sqlCommand.Parameters.AddWithValue("Footer75" + i, item.Footer75 == null ? (object)DBNull.Value : item.Footer75);
						sqlCommand.Parameters.AddWithValue("Footer76" + i, item.Footer76 == null ? (object)DBNull.Value : item.Footer76);
						sqlCommand.Parameters.AddWithValue("Footer77" + i, item.Footer77 == null ? (object)DBNull.Value : item.Footer77);
						sqlCommand.Parameters.AddWithValue("ForDeliveryNote" + i, item.ForDeliveryNote == null ? (object)DBNull.Value : item.ForDeliveryNote);
						sqlCommand.Parameters.AddWithValue("ForPosDeliveryNote" + i, item.ForPosDeliveryNote == null ? (object)DBNull.Value : item.ForPosDeliveryNote);
						sqlCommand.Parameters.AddWithValue("Geliefert" + i, item.Geliefert == null ? (object)DBNull.Value : item.Geliefert);
						sqlCommand.Parameters.AddWithValue("Header" + i, item.Header == null ? (object)DBNull.Value : item.Header);
						sqlCommand.Parameters.AddWithValue("ImportLogoImageId" + i, item.ImportLogoImageId);
						sqlCommand.Parameters.AddWithValue("Index_Kunde" + i, item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
						sqlCommand.Parameters.AddWithValue("InternalNumber" + i, item.InternalNumber == null ? (object)DBNull.Value : item.InternalNumber);
						sqlCommand.Parameters.AddWithValue("ItemsFooter1" + i, item.ItemsFooter1 == null ? (object)DBNull.Value : item.ItemsFooter1);
						sqlCommand.Parameters.AddWithValue("ItemsFooter2" + i, item.ItemsFooter2 == null ? (object)DBNull.Value : item.ItemsFooter2);
						sqlCommand.Parameters.AddWithValue("ItemsHeader" + i, item.ItemsHeader == null ? (object)DBNull.Value : item.ItemsHeader);
						sqlCommand.Parameters.AddWithValue("LanguageId" + i, item.LanguageId);
						sqlCommand.Parameters.AddWithValue("LastPageText1" + i, item.LastPageText1 == null ? (object)DBNull.Value : item.LastPageText1);
						sqlCommand.Parameters.AddWithValue("LastPageText10" + i, item.LastPageText10 == null ? (object)DBNull.Value : item.LastPageText10);
						sqlCommand.Parameters.AddWithValue("LastPageText11" + i, item.LastPageText11 == null ? (object)DBNull.Value : item.LastPageText11);
						sqlCommand.Parameters.AddWithValue("LastPageText2" + i, item.LastPageText2 == null ? (object)DBNull.Value : item.LastPageText2);
						sqlCommand.Parameters.AddWithValue("LastPageText3" + i, item.LastPageText3 == null ? (object)DBNull.Value : item.LastPageText3);
						sqlCommand.Parameters.AddWithValue("LastPageText4" + i, item.LastPageText4 == null ? (object)DBNull.Value : item.LastPageText4);
						sqlCommand.Parameters.AddWithValue("LastPageText5" + i, item.LastPageText5 == null ? (object)DBNull.Value : item.LastPageText5);
						sqlCommand.Parameters.AddWithValue("LastPageText6" + i, item.LastPageText6 == null ? (object)DBNull.Value : item.LastPageText6);
						sqlCommand.Parameters.AddWithValue("LastPageText7" + i, item.LastPageText7 == null ? (object)DBNull.Value : item.LastPageText7);
						sqlCommand.Parameters.AddWithValue("LastPageText8" + i, item.LastPageText8 == null ? (object)DBNull.Value : item.LastPageText8);
						sqlCommand.Parameters.AddWithValue("LastPageText9" + i, item.LastPageText9 == null ? (object)DBNull.Value : item.LastPageText9);
						sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
						sqlCommand.Parameters.AddWithValue("LastUpdateUserId" + i, item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
						sqlCommand.Parameters.AddWithValue("Lieferadresse" + i, item.Lieferadresse == null ? (object)DBNull.Value : item.Lieferadresse);
						sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
						sqlCommand.Parameters.AddWithValue("Offen" + i, item.Offen == null ? (object)DBNull.Value : item.Offen);
						sqlCommand.Parameters.AddWithValue("OrderDate" + i, item.OrderDate == null ? (object)DBNull.Value : item.OrderDate);
						sqlCommand.Parameters.AddWithValue("OrderNumber" + i, item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
						sqlCommand.Parameters.AddWithValue("OrderNumberPO" + i, item.OrderNumberPO == null ? (object)DBNull.Value : item.OrderNumberPO);
						sqlCommand.Parameters.AddWithValue("OrderTypeId" + i, item.OrderTypeId);
						sqlCommand.Parameters.AddWithValue("PaymentMethod" + i, item.PaymentMethod == null ? (object)DBNull.Value : item.PaymentMethod);
						sqlCommand.Parameters.AddWithValue("PaymentTarget" + i, item.PaymentTarget == null ? (object)DBNull.Value : item.PaymentTarget);
						sqlCommand.Parameters.AddWithValue("PE" + i, item.PE == null ? (object)DBNull.Value : item.PE);
						sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
						sqlCommand.Parameters.AddWithValue("ShippingMethod" + i, item.ShippingMethod == null ? (object)DBNull.Value : item.ShippingMethod);
						sqlCommand.Parameters.AddWithValue("SummarySum" + i, item.SummarySum == null ? (object)DBNull.Value : item.SummarySum);
						sqlCommand.Parameters.AddWithValue("SummaryTotal" + i, item.SummaryTotal == null ? (object)DBNull.Value : item.SummaryTotal);
						sqlCommand.Parameters.AddWithValue("SummaryUST" + i, item.SummaryUST == null ? (object)DBNull.Value : item.SummaryUST);
						sqlCommand.Parameters.AddWithValue("TotalPrice150" + i, item.TotalPrice150 == null ? (object)DBNull.Value : item.TotalPrice150);
						sqlCommand.Parameters.AddWithValue("Unit" + i, item.Unit == null ? (object)DBNull.Value : item.Unit);
						sqlCommand.Parameters.AddWithValue("UnitPrice" + i, item.UnitPrice == null ? (object)DBNull.Value : item.UnitPrice);
						sqlCommand.Parameters.AddWithValue("UnitTotal" + i, item.UnitTotal == null ? (object)DBNull.Value : item.UnitTotal);
						sqlCommand.Parameters.AddWithValue("UST_ID" + i, item.UST_ID == null ? (object)DBNull.Value : item.UST_ID);
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
				string query = "DELETE FROM [__PRS_OrderReport] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__PRS_OrderReport] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__PRS_OrderReport] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__PRS_OrderReport]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__PRS_OrderReport] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [__PRS_OrderReport] ([Abladestelle],[AccountOwner1],[AccountOwner2],[AccountOwner3],[AccountOwner4],[Address1],[Address2],[Address3],[Address4],[Amount],[Article],[ArtikelCountry],[ArtikelPrice],[ArtikelQuantity],[ArtikelStock],[ArtikelWeight],[BasisPrice150],[Bestellt],[ClientNumber],[CompanyLogoImageId],[Cu_G],[Cu_Surcharge],[Cu_Total],[CustomerDate],[CustomerNumber],[DEL],[Description],[Designation],[Designation1],[Designation2],[DiscountText1],[DiscountText2],[DiscountText3],[DiscountText4],[DiscountText5],[DiscountText6],[DocumentType],[FactoringText1],[FactoringText2],[FactoringText3],[FactoringText4],[FactoringText5],[FactoringText6],[Footer11],[Footer12],[Footer13],[Footer14],[Footer15],[Footer16],[Footer17],[Footer21],[Footer22],[Footer23],[Footer24],[Footer25],[Footer26],[Footer27],[Footer31],[Footer32],[Footer33],[Footer34],[Footer35],[Footer36],[Footer37],[Footer41],[Footer42],[Footer43],[Footer44],[Footer45],[Footer46],[Footer47],[Footer51],[Footer52],[Footer53],[Footer54],[Footer55],[Footer56],[Footer57],[Footer61],[Footer62],[Footer63],[Footer64],[Footer65],[Footer66],[Footer67],[Footer71],[Footer72],[Footer73],[Footer74],[Footer75],[Footer76],[Footer77],[ForDeliveryNote],[ForPosDeliveryNote],[Geliefert],[Header],[ImportLogoImageId],[Index_Kunde],[InternalNumber],[ItemsFooter1],[ItemsFooter2],[ItemsHeader],[LanguageId],[LastPageText1],[LastPageText10],[LastPageText11],[LastPageText2],[LastPageText3],[LastPageText4],[LastPageText5],[LastPageText6],[LastPageText7],[LastPageText8],[LastPageText9],[LastUpdateTime],[LastUpdateUserId],[Lieferadresse],[Liefertermin],[Offen],[OrderDate],[OrderNumber],[OrderNumberPO],[OrderTypeId],[PaymentMethod],[PaymentTarget],[PE],[Position],[ShippingMethod],[SummarySum],[SummaryTotal],[SummaryUST],[TotalPrice150],[Unit],[UnitPrice],[UnitTotal],[UST_ID]) OUTPUT INSERTED.[Id] VALUES (@Abladestelle,@AccountOwner1,@AccountOwner2,@AccountOwner3,@AccountOwner4,@Address1,@Address2,@Address3,@Address4,@Amount,@Article,@ArtikelCountry,@ArtikelPrice,@ArtikelQuantity,@ArtikelStock,@ArtikelWeight,@BasisPrice150,@Bestellt,@ClientNumber,@CompanyLogoImageId,@Cu_G,@Cu_Surcharge,@Cu_Total,@CustomerDate,@CustomerNumber,@DEL,@Description,@Designation,@Designation1,@Designation2,@DiscountText1,@DiscountText2,@DiscountText3,@DiscountText4,@DiscountText5,@DiscountText6,@DocumentType,@FactoringText1,@FactoringText2,@FactoringText3,@FactoringText4,@FactoringText5,@FactoringText6,@Footer11,@Footer12,@Footer13,@Footer14,@Footer15,@Footer16,@Footer17,@Footer21,@Footer22,@Footer23,@Footer24,@Footer25,@Footer26,@Footer27,@Footer31,@Footer32,@Footer33,@Footer34,@Footer35,@Footer36,@Footer37,@Footer41,@Footer42,@Footer43,@Footer44,@Footer45,@Footer46,@Footer47,@Footer51,@Footer52,@Footer53,@Footer54,@Footer55,@Footer56,@Footer57,@Footer61,@Footer62,@Footer63,@Footer64,@Footer65,@Footer66,@Footer67,@Footer71,@Footer72,@Footer73,@Footer74,@Footer75,@Footer76,@Footer77,@ForDeliveryNote,@ForPosDeliveryNote,@Geliefert,@Header,@ImportLogoImageId,@Index_Kunde,@InternalNumber,@ItemsFooter1,@ItemsFooter2,@ItemsHeader,@LanguageId,@LastPageText1,@LastPageText10,@LastPageText11,@LastPageText2,@LastPageText3,@LastPageText4,@LastPageText5,@LastPageText6,@LastPageText7,@LastPageText8,@LastPageText9,@LastUpdateTime,@LastUpdateUserId,@Lieferadresse,@Liefertermin,@Offen,@OrderDate,@OrderNumber,@OrderNumberPO,@OrderTypeId,@PaymentMethod,@PaymentTarget,@PE,@Position,@ShippingMethod,@SummarySum,@SummaryTotal,@SummaryUST,@TotalPrice150,@Unit,@UnitPrice,@UnitTotal,@UST_ID); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Abladestelle", item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
			sqlCommand.Parameters.AddWithValue("AccountOwner1", item.AccountOwner1 == null ? (object)DBNull.Value : item.AccountOwner1);
			sqlCommand.Parameters.AddWithValue("AccountOwner2", item.AccountOwner2 == null ? (object)DBNull.Value : item.AccountOwner2);
			sqlCommand.Parameters.AddWithValue("AccountOwner3", item.AccountOwner3 == null ? (object)DBNull.Value : item.AccountOwner3);
			sqlCommand.Parameters.AddWithValue("AccountOwner4", item.AccountOwner4 == null ? (object)DBNull.Value : item.AccountOwner4);
			sqlCommand.Parameters.AddWithValue("Address1", item.Address1 == null ? (object)DBNull.Value : item.Address1);
			sqlCommand.Parameters.AddWithValue("Address2", item.Address2 == null ? (object)DBNull.Value : item.Address2);
			sqlCommand.Parameters.AddWithValue("Address3", item.Address3 == null ? (object)DBNull.Value : item.Address3);
			sqlCommand.Parameters.AddWithValue("Address4", item.Address4 == null ? (object)DBNull.Value : item.Address4);
			sqlCommand.Parameters.AddWithValue("Amount", item.Amount == null ? (object)DBNull.Value : item.Amount);
			sqlCommand.Parameters.AddWithValue("Article", item.Article == null ? (object)DBNull.Value : item.Article);
			sqlCommand.Parameters.AddWithValue("ArtikelCountry", item.ArtikelCountry == null ? (object)DBNull.Value : item.ArtikelCountry);
			sqlCommand.Parameters.AddWithValue("ArtikelPrice", item.ArtikelPrice == null ? (object)DBNull.Value : item.ArtikelPrice);
			sqlCommand.Parameters.AddWithValue("ArtikelQuantity", item.ArtikelQuantity == null ? (object)DBNull.Value : item.ArtikelQuantity);
			sqlCommand.Parameters.AddWithValue("ArtikelStock", item.ArtikelStock == null ? (object)DBNull.Value : item.ArtikelStock);
			sqlCommand.Parameters.AddWithValue("ArtikelWeight", item.ArtikelWeight == null ? (object)DBNull.Value : item.ArtikelWeight);
			sqlCommand.Parameters.AddWithValue("BasisPrice150", item.BasisPrice150 == null ? (object)DBNull.Value : item.BasisPrice150);
			sqlCommand.Parameters.AddWithValue("Bestellt", item.Bestellt == null ? (object)DBNull.Value : item.Bestellt);
			sqlCommand.Parameters.AddWithValue("ClientNumber", item.ClientNumber == null ? (object)DBNull.Value : item.ClientNumber);
			sqlCommand.Parameters.AddWithValue("CompanyLogoImageId", item.CompanyLogoImageId);
			sqlCommand.Parameters.AddWithValue("Cu_G", item.Cu_G == null ? (object)DBNull.Value : item.Cu_G);
			sqlCommand.Parameters.AddWithValue("Cu_Surcharge", item.Cu_Surcharge == null ? (object)DBNull.Value : item.Cu_Surcharge);
			sqlCommand.Parameters.AddWithValue("Cu_Total", item.Cu_Total == null ? (object)DBNull.Value : item.Cu_Total);
			sqlCommand.Parameters.AddWithValue("CustomerDate", item.CustomerDate == null ? (object)DBNull.Value : item.CustomerDate);
			sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
			sqlCommand.Parameters.AddWithValue("DEL", item.DEL == null ? (object)DBNull.Value : item.DEL);
			sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
			sqlCommand.Parameters.AddWithValue("Designation", item.Designation == null ? (object)DBNull.Value : item.Designation);
			sqlCommand.Parameters.AddWithValue("Designation1", item.Designation1 == null ? (object)DBNull.Value : item.Designation1);
			sqlCommand.Parameters.AddWithValue("Designation2", item.Designation2 == null ? (object)DBNull.Value : item.Designation2);
			sqlCommand.Parameters.AddWithValue("DiscountText1", item.DiscountText1 == null ? (object)DBNull.Value : item.DiscountText1);
			sqlCommand.Parameters.AddWithValue("DiscountText2", item.DiscountText2 == null ? (object)DBNull.Value : item.DiscountText2);
			sqlCommand.Parameters.AddWithValue("DiscountText3", item.DiscountText3 == null ? (object)DBNull.Value : item.DiscountText3);
			sqlCommand.Parameters.AddWithValue("DiscountText4", item.DiscountText4 == null ? (object)DBNull.Value : item.DiscountText4);
			sqlCommand.Parameters.AddWithValue("DiscountText5", item.DiscountText5 == null ? (object)DBNull.Value : item.DiscountText5);
			sqlCommand.Parameters.AddWithValue("DiscountText6", item.DiscountText6 == null ? (object)DBNull.Value : item.DiscountText6);
			sqlCommand.Parameters.AddWithValue("DocumentType", item.DocumentType == null ? (object)DBNull.Value : item.DocumentType);
			sqlCommand.Parameters.AddWithValue("FactoringText1", item.FactoringText1 == null ? (object)DBNull.Value : item.FactoringText1);
			sqlCommand.Parameters.AddWithValue("FactoringText2", item.FactoringText2 == null ? (object)DBNull.Value : item.FactoringText2);
			sqlCommand.Parameters.AddWithValue("FactoringText3", item.FactoringText3 == null ? (object)DBNull.Value : item.FactoringText3);
			sqlCommand.Parameters.AddWithValue("FactoringText4", item.FactoringText4 == null ? (object)DBNull.Value : item.FactoringText4);
			sqlCommand.Parameters.AddWithValue("FactoringText5", item.FactoringText5 == null ? (object)DBNull.Value : item.FactoringText5);
			sqlCommand.Parameters.AddWithValue("FactoringText6", item.FactoringText6 == null ? (object)DBNull.Value : item.FactoringText6);
			sqlCommand.Parameters.AddWithValue("Footer11", item.Footer11 == null ? (object)DBNull.Value : item.Footer11);
			sqlCommand.Parameters.AddWithValue("Footer12", item.Footer12 == null ? (object)DBNull.Value : item.Footer12);
			sqlCommand.Parameters.AddWithValue("Footer13", item.Footer13 == null ? (object)DBNull.Value : item.Footer13);
			sqlCommand.Parameters.AddWithValue("Footer14", item.Footer14 == null ? (object)DBNull.Value : item.Footer14);
			sqlCommand.Parameters.AddWithValue("Footer15", item.Footer15 == null ? (object)DBNull.Value : item.Footer15);
			sqlCommand.Parameters.AddWithValue("Footer16", item.Footer16 == null ? (object)DBNull.Value : item.Footer16);
			sqlCommand.Parameters.AddWithValue("Footer17", item.Footer17 == null ? (object)DBNull.Value : item.Footer17);
			sqlCommand.Parameters.AddWithValue("Footer21", item.Footer21 == null ? (object)DBNull.Value : item.Footer21);
			sqlCommand.Parameters.AddWithValue("Footer22", item.Footer22 == null ? (object)DBNull.Value : item.Footer22);
			sqlCommand.Parameters.AddWithValue("Footer23", item.Footer23 == null ? (object)DBNull.Value : item.Footer23);
			sqlCommand.Parameters.AddWithValue("Footer24", item.Footer24 == null ? (object)DBNull.Value : item.Footer24);
			sqlCommand.Parameters.AddWithValue("Footer25", item.Footer25 == null ? (object)DBNull.Value : item.Footer25);
			sqlCommand.Parameters.AddWithValue("Footer26", item.Footer26 == null ? (object)DBNull.Value : item.Footer26);
			sqlCommand.Parameters.AddWithValue("Footer27", item.Footer27 == null ? (object)DBNull.Value : item.Footer27);
			sqlCommand.Parameters.AddWithValue("Footer31", item.Footer31 == null ? (object)DBNull.Value : item.Footer31);
			sqlCommand.Parameters.AddWithValue("Footer32", item.Footer32 == null ? (object)DBNull.Value : item.Footer32);
			sqlCommand.Parameters.AddWithValue("Footer33", item.Footer33 == null ? (object)DBNull.Value : item.Footer33);
			sqlCommand.Parameters.AddWithValue("Footer34", item.Footer34 == null ? (object)DBNull.Value : item.Footer34);
			sqlCommand.Parameters.AddWithValue("Footer35", item.Footer35 == null ? (object)DBNull.Value : item.Footer35);
			sqlCommand.Parameters.AddWithValue("Footer36", item.Footer36 == null ? (object)DBNull.Value : item.Footer36);
			sqlCommand.Parameters.AddWithValue("Footer37", item.Footer37 == null ? (object)DBNull.Value : item.Footer37);
			sqlCommand.Parameters.AddWithValue("Footer41", item.Footer41 == null ? (object)DBNull.Value : item.Footer41);
			sqlCommand.Parameters.AddWithValue("Footer42", item.Footer42 == null ? (object)DBNull.Value : item.Footer42);
			sqlCommand.Parameters.AddWithValue("Footer43", item.Footer43 == null ? (object)DBNull.Value : item.Footer43);
			sqlCommand.Parameters.AddWithValue("Footer44", item.Footer44 == null ? (object)DBNull.Value : item.Footer44);
			sqlCommand.Parameters.AddWithValue("Footer45", item.Footer45 == null ? (object)DBNull.Value : item.Footer45);
			sqlCommand.Parameters.AddWithValue("Footer46", item.Footer46 == null ? (object)DBNull.Value : item.Footer46);
			sqlCommand.Parameters.AddWithValue("Footer47", item.Footer47 == null ? (object)DBNull.Value : item.Footer47);
			sqlCommand.Parameters.AddWithValue("Footer51", item.Footer51 == null ? (object)DBNull.Value : item.Footer51);
			sqlCommand.Parameters.AddWithValue("Footer52", item.Footer52 == null ? (object)DBNull.Value : item.Footer52);
			sqlCommand.Parameters.AddWithValue("Footer53", item.Footer53 == null ? (object)DBNull.Value : item.Footer53);
			sqlCommand.Parameters.AddWithValue("Footer54", item.Footer54 == null ? (object)DBNull.Value : item.Footer54);
			sqlCommand.Parameters.AddWithValue("Footer55", item.Footer55 == null ? (object)DBNull.Value : item.Footer55);
			sqlCommand.Parameters.AddWithValue("Footer56", item.Footer56 == null ? (object)DBNull.Value : item.Footer56);
			sqlCommand.Parameters.AddWithValue("Footer57", item.Footer57 == null ? (object)DBNull.Value : item.Footer57);
			sqlCommand.Parameters.AddWithValue("Footer61", item.Footer61 == null ? (object)DBNull.Value : item.Footer61);
			sqlCommand.Parameters.AddWithValue("Footer62", item.Footer62 == null ? (object)DBNull.Value : item.Footer62);
			sqlCommand.Parameters.AddWithValue("Footer63", item.Footer63 == null ? (object)DBNull.Value : item.Footer63);
			sqlCommand.Parameters.AddWithValue("Footer64", item.Footer64 == null ? (object)DBNull.Value : item.Footer64);
			sqlCommand.Parameters.AddWithValue("Footer65", item.Footer65 == null ? (object)DBNull.Value : item.Footer65);
			sqlCommand.Parameters.AddWithValue("Footer66", item.Footer66 == null ? (object)DBNull.Value : item.Footer66);
			sqlCommand.Parameters.AddWithValue("Footer67", item.Footer67 == null ? (object)DBNull.Value : item.Footer67);
			sqlCommand.Parameters.AddWithValue("Footer71", item.Footer71 == null ? (object)DBNull.Value : item.Footer71);
			sqlCommand.Parameters.AddWithValue("Footer72", item.Footer72 == null ? (object)DBNull.Value : item.Footer72);
			sqlCommand.Parameters.AddWithValue("Footer73", item.Footer73 == null ? (object)DBNull.Value : item.Footer73);
			sqlCommand.Parameters.AddWithValue("Footer74", item.Footer74 == null ? (object)DBNull.Value : item.Footer74);
			sqlCommand.Parameters.AddWithValue("Footer75", item.Footer75 == null ? (object)DBNull.Value : item.Footer75);
			sqlCommand.Parameters.AddWithValue("Footer76", item.Footer76 == null ? (object)DBNull.Value : item.Footer76);
			sqlCommand.Parameters.AddWithValue("Footer77", item.Footer77 == null ? (object)DBNull.Value : item.Footer77);
			sqlCommand.Parameters.AddWithValue("ForDeliveryNote", item.ForDeliveryNote == null ? (object)DBNull.Value : item.ForDeliveryNote);
			sqlCommand.Parameters.AddWithValue("ForPosDeliveryNote", item.ForPosDeliveryNote == null ? (object)DBNull.Value : item.ForPosDeliveryNote);
			sqlCommand.Parameters.AddWithValue("Geliefert", item.Geliefert == null ? (object)DBNull.Value : item.Geliefert);
			sqlCommand.Parameters.AddWithValue("Header", item.Header == null ? (object)DBNull.Value : item.Header);
			sqlCommand.Parameters.AddWithValue("ImportLogoImageId", item.ImportLogoImageId);
			sqlCommand.Parameters.AddWithValue("Index_Kunde", item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
			sqlCommand.Parameters.AddWithValue("InternalNumber", item.InternalNumber == null ? (object)DBNull.Value : item.InternalNumber);
			sqlCommand.Parameters.AddWithValue("ItemsFooter1", item.ItemsFooter1 == null ? (object)DBNull.Value : item.ItemsFooter1);
			sqlCommand.Parameters.AddWithValue("ItemsFooter2", item.ItemsFooter2 == null ? (object)DBNull.Value : item.ItemsFooter2);
			sqlCommand.Parameters.AddWithValue("ItemsHeader", item.ItemsHeader == null ? (object)DBNull.Value : item.ItemsHeader);
			sqlCommand.Parameters.AddWithValue("LanguageId", item.LanguageId);
			sqlCommand.Parameters.AddWithValue("LastPageText1", item.LastPageText1 == null ? (object)DBNull.Value : item.LastPageText1);
			sqlCommand.Parameters.AddWithValue("LastPageText10", item.LastPageText10 == null ? (object)DBNull.Value : item.LastPageText10);
			sqlCommand.Parameters.AddWithValue("LastPageText11", item.LastPageText11 == null ? (object)DBNull.Value : item.LastPageText11);
			sqlCommand.Parameters.AddWithValue("LastPageText2", item.LastPageText2 == null ? (object)DBNull.Value : item.LastPageText2);
			sqlCommand.Parameters.AddWithValue("LastPageText3", item.LastPageText3 == null ? (object)DBNull.Value : item.LastPageText3);
			sqlCommand.Parameters.AddWithValue("LastPageText4", item.LastPageText4 == null ? (object)DBNull.Value : item.LastPageText4);
			sqlCommand.Parameters.AddWithValue("LastPageText5", item.LastPageText5 == null ? (object)DBNull.Value : item.LastPageText5);
			sqlCommand.Parameters.AddWithValue("LastPageText6", item.LastPageText6 == null ? (object)DBNull.Value : item.LastPageText6);
			sqlCommand.Parameters.AddWithValue("LastPageText7", item.LastPageText7 == null ? (object)DBNull.Value : item.LastPageText7);
			sqlCommand.Parameters.AddWithValue("LastPageText8", item.LastPageText8 == null ? (object)DBNull.Value : item.LastPageText8);
			sqlCommand.Parameters.AddWithValue("LastPageText9", item.LastPageText9 == null ? (object)DBNull.Value : item.LastPageText9);
			sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
			sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
			sqlCommand.Parameters.AddWithValue("Lieferadresse", item.Lieferadresse == null ? (object)DBNull.Value : item.Lieferadresse);
			sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
			sqlCommand.Parameters.AddWithValue("Offen", item.Offen == null ? (object)DBNull.Value : item.Offen);
			sqlCommand.Parameters.AddWithValue("OrderDate", item.OrderDate == null ? (object)DBNull.Value : item.OrderDate);
			sqlCommand.Parameters.AddWithValue("OrderNumber", item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
			sqlCommand.Parameters.AddWithValue("OrderNumberPO", item.OrderNumberPO == null ? (object)DBNull.Value : item.OrderNumberPO);
			sqlCommand.Parameters.AddWithValue("OrderTypeId", item.OrderTypeId);
			sqlCommand.Parameters.AddWithValue("PaymentMethod", item.PaymentMethod == null ? (object)DBNull.Value : item.PaymentMethod);
			sqlCommand.Parameters.AddWithValue("PaymentTarget", item.PaymentTarget == null ? (object)DBNull.Value : item.PaymentTarget);
			sqlCommand.Parameters.AddWithValue("PE", item.PE == null ? (object)DBNull.Value : item.PE);
			sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
			sqlCommand.Parameters.AddWithValue("ShippingMethod", item.ShippingMethod == null ? (object)DBNull.Value : item.ShippingMethod);
			sqlCommand.Parameters.AddWithValue("SummarySum", item.SummarySum == null ? (object)DBNull.Value : item.SummarySum);
			sqlCommand.Parameters.AddWithValue("SummaryTotal", item.SummaryTotal == null ? (object)DBNull.Value : item.SummaryTotal);
			sqlCommand.Parameters.AddWithValue("SummaryUST", item.SummaryUST == null ? (object)DBNull.Value : item.SummaryUST);
			sqlCommand.Parameters.AddWithValue("TotalPrice150", item.TotalPrice150 == null ? (object)DBNull.Value : item.TotalPrice150);
			sqlCommand.Parameters.AddWithValue("Unit", item.Unit == null ? (object)DBNull.Value : item.Unit);
			sqlCommand.Parameters.AddWithValue("UnitPrice", item.UnitPrice == null ? (object)DBNull.Value : item.UnitPrice);
			sqlCommand.Parameters.AddWithValue("UnitTotal", item.UnitTotal == null ? (object)DBNull.Value : item.UnitTotal);
			sqlCommand.Parameters.AddWithValue("UST_ID", item.UST_ID == null ? (object)DBNull.Value : item.UST_ID);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 137; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__PRS_OrderReport] ([Abladestelle],[AccountOwner1],[AccountOwner2],[AccountOwner3],[AccountOwner4],[Address1],[Address2],[Address3],[Address4],[Amount],[Article],[ArtikelCountry],[ArtikelPrice],[ArtikelQuantity],[ArtikelStock],[ArtikelWeight],[BasisPrice150],[Bestellt],[ClientNumber],[CompanyLogoImageId],[Cu_G],[Cu_Surcharge],[Cu_Total],[CustomerDate],[CustomerNumber],[DEL],[Description],[Designation],[Designation1],[Designation2],[DiscountText1],[DiscountText2],[DiscountText3],[DiscountText4],[DiscountText5],[DiscountText6],[DocumentType],[FactoringText1],[FactoringText2],[FactoringText3],[FactoringText4],[FactoringText5],[FactoringText6],[Footer11],[Footer12],[Footer13],[Footer14],[Footer15],[Footer16],[Footer17],[Footer21],[Footer22],[Footer23],[Footer24],[Footer25],[Footer26],[Footer27],[Footer31],[Footer32],[Footer33],[Footer34],[Footer35],[Footer36],[Footer37],[Footer41],[Footer42],[Footer43],[Footer44],[Footer45],[Footer46],[Footer47],[Footer51],[Footer52],[Footer53],[Footer54],[Footer55],[Footer56],[Footer57],[Footer61],[Footer62],[Footer63],[Footer64],[Footer65],[Footer66],[Footer67],[Footer71],[Footer72],[Footer73],[Footer74],[Footer75],[Footer76],[Footer77],[ForDeliveryNote],[ForPosDeliveryNote],[Geliefert],[Header],[ImportLogoImageId],[Index_Kunde],[InternalNumber],[ItemsFooter1],[ItemsFooter2],[ItemsHeader],[LanguageId],[LastPageText1],[LastPageText10],[LastPageText11],[LastPageText2],[LastPageText3],[LastPageText4],[LastPageText5],[LastPageText6],[LastPageText7],[LastPageText8],[LastPageText9],[LastUpdateTime],[LastUpdateUserId],[Lieferadresse],[Liefertermin],[Offen],[OrderDate],[OrderNumber],[OrderNumberPO],[OrderTypeId],[PaymentMethod],[PaymentTarget],[PE],[Position],[ShippingMethod],[SummarySum],[SummaryTotal],[SummaryUST],[TotalPrice150],[Unit],[UnitPrice],[UnitTotal],[UST_ID]) VALUES ( "

						+ "@Abladestelle" + i + ","
						+ "@AccountOwner1" + i + ","
						+ "@AccountOwner2" + i + ","
						+ "@AccountOwner3" + i + ","
						+ "@AccountOwner4" + i + ","
						+ "@Address1" + i + ","
						+ "@Address2" + i + ","
						+ "@Address3" + i + ","
						+ "@Address4" + i + ","
						+ "@Amount" + i + ","
						+ "@Article" + i + ","
						+ "@ArtikelCountry" + i + ","
						+ "@ArtikelPrice" + i + ","
						+ "@ArtikelQuantity" + i + ","
						+ "@ArtikelStock" + i + ","
						+ "@ArtikelWeight" + i + ","
						+ "@BasisPrice150" + i + ","
						+ "@Bestellt" + i + ","
						+ "@ClientNumber" + i + ","
						+ "@CompanyLogoImageId" + i + ","
						+ "@Cu_G" + i + ","
						+ "@Cu_Surcharge" + i + ","
						+ "@Cu_Total" + i + ","
						+ "@CustomerDate" + i + ","
						+ "@CustomerNumber" + i + ","
						+ "@DEL" + i + ","
						+ "@Description" + i + ","
						+ "@Designation" + i + ","
						+ "@Designation1" + i + ","
						+ "@Designation2" + i + ","
						+ "@DiscountText1" + i + ","
						+ "@DiscountText2" + i + ","
						+ "@DiscountText3" + i + ","
						+ "@DiscountText4" + i + ","
						+ "@DiscountText5" + i + ","
						+ "@DiscountText6" + i + ","
						+ "@DocumentType" + i + ","
						+ "@FactoringText1" + i + ","
						+ "@FactoringText2" + i + ","
						+ "@FactoringText3" + i + ","
						+ "@FactoringText4" + i + ","
						+ "@FactoringText5" + i + ","
						+ "@FactoringText6" + i + ","
						+ "@Footer11" + i + ","
						+ "@Footer12" + i + ","
						+ "@Footer13" + i + ","
						+ "@Footer14" + i + ","
						+ "@Footer15" + i + ","
						+ "@Footer16" + i + ","
						+ "@Footer17" + i + ","
						+ "@Footer21" + i + ","
						+ "@Footer22" + i + ","
						+ "@Footer23" + i + ","
						+ "@Footer24" + i + ","
						+ "@Footer25" + i + ","
						+ "@Footer26" + i + ","
						+ "@Footer27" + i + ","
						+ "@Footer31" + i + ","
						+ "@Footer32" + i + ","
						+ "@Footer33" + i + ","
						+ "@Footer34" + i + ","
						+ "@Footer35" + i + ","
						+ "@Footer36" + i + ","
						+ "@Footer37" + i + ","
						+ "@Footer41" + i + ","
						+ "@Footer42" + i + ","
						+ "@Footer43" + i + ","
						+ "@Footer44" + i + ","
						+ "@Footer45" + i + ","
						+ "@Footer46" + i + ","
						+ "@Footer47" + i + ","
						+ "@Footer51" + i + ","
						+ "@Footer52" + i + ","
						+ "@Footer53" + i + ","
						+ "@Footer54" + i + ","
						+ "@Footer55" + i + ","
						+ "@Footer56" + i + ","
						+ "@Footer57" + i + ","
						+ "@Footer61" + i + ","
						+ "@Footer62" + i + ","
						+ "@Footer63" + i + ","
						+ "@Footer64" + i + ","
						+ "@Footer65" + i + ","
						+ "@Footer66" + i + ","
						+ "@Footer67" + i + ","
						+ "@Footer71" + i + ","
						+ "@Footer72" + i + ","
						+ "@Footer73" + i + ","
						+ "@Footer74" + i + ","
						+ "@Footer75" + i + ","
						+ "@Footer76" + i + ","
						+ "@Footer77" + i + ","
						+ "@ForDeliveryNote" + i + ","
						+ "@ForPosDeliveryNote" + i + ","
						+ "@Geliefert" + i + ","
						+ "@Header" + i + ","
						+ "@ImportLogoImageId" + i + ","
						+ "@Index_Kunde" + i + ","
						+ "@InternalNumber" + i + ","
						+ "@ItemsFooter1" + i + ","
						+ "@ItemsFooter2" + i + ","
						+ "@ItemsHeader" + i + ","
						+ "@LanguageId" + i + ","
						+ "@LastPageText1" + i + ","
						+ "@LastPageText10" + i + ","
						+ "@LastPageText11" + i + ","
						+ "@LastPageText2" + i + ","
						+ "@LastPageText3" + i + ","
						+ "@LastPageText4" + i + ","
						+ "@LastPageText5" + i + ","
						+ "@LastPageText6" + i + ","
						+ "@LastPageText7" + i + ","
						+ "@LastPageText8" + i + ","
						+ "@LastPageText9" + i + ","
						+ "@LastUpdateTime" + i + ","
						+ "@LastUpdateUserId" + i + ","
						+ "@Lieferadresse" + i + ","
						+ "@Liefertermin" + i + ","
						+ "@Offen" + i + ","
						+ "@OrderDate" + i + ","
						+ "@OrderNumber" + i + ","
						+ "@OrderNumberPO" + i + ","
						+ "@OrderTypeId" + i + ","
						+ "@PaymentMethod" + i + ","
						+ "@PaymentTarget" + i + ","
						+ "@PE" + i + ","
						+ "@Position" + i + ","
						+ "@ShippingMethod" + i + ","
						+ "@SummarySum" + i + ","
						+ "@SummaryTotal" + i + ","
						+ "@SummaryUST" + i + ","
						+ "@TotalPrice150" + i + ","
						+ "@Unit" + i + ","
						+ "@UnitPrice" + i + ","
						+ "@UnitTotal" + i + ","
						+ "@UST_ID" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Abladestelle" + i, item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
					sqlCommand.Parameters.AddWithValue("AccountOwner1" + i, item.AccountOwner1 == null ? (object)DBNull.Value : item.AccountOwner1);
					sqlCommand.Parameters.AddWithValue("AccountOwner2" + i, item.AccountOwner2 == null ? (object)DBNull.Value : item.AccountOwner2);
					sqlCommand.Parameters.AddWithValue("AccountOwner3" + i, item.AccountOwner3 == null ? (object)DBNull.Value : item.AccountOwner3);
					sqlCommand.Parameters.AddWithValue("AccountOwner4" + i, item.AccountOwner4 == null ? (object)DBNull.Value : item.AccountOwner4);
					sqlCommand.Parameters.AddWithValue("Address1" + i, item.Address1 == null ? (object)DBNull.Value : item.Address1);
					sqlCommand.Parameters.AddWithValue("Address2" + i, item.Address2 == null ? (object)DBNull.Value : item.Address2);
					sqlCommand.Parameters.AddWithValue("Address3" + i, item.Address3 == null ? (object)DBNull.Value : item.Address3);
					sqlCommand.Parameters.AddWithValue("Address4" + i, item.Address4 == null ? (object)DBNull.Value : item.Address4);
					sqlCommand.Parameters.AddWithValue("Amount" + i, item.Amount == null ? (object)DBNull.Value : item.Amount);
					sqlCommand.Parameters.AddWithValue("Article" + i, item.Article == null ? (object)DBNull.Value : item.Article);
					sqlCommand.Parameters.AddWithValue("ArtikelCountry" + i, item.ArtikelCountry == null ? (object)DBNull.Value : item.ArtikelCountry);
					sqlCommand.Parameters.AddWithValue("ArtikelPrice" + i, item.ArtikelPrice == null ? (object)DBNull.Value : item.ArtikelPrice);
					sqlCommand.Parameters.AddWithValue("ArtikelQuantity" + i, item.ArtikelQuantity == null ? (object)DBNull.Value : item.ArtikelQuantity);
					sqlCommand.Parameters.AddWithValue("ArtikelStock" + i, item.ArtikelStock == null ? (object)DBNull.Value : item.ArtikelStock);
					sqlCommand.Parameters.AddWithValue("ArtikelWeight" + i, item.ArtikelWeight == null ? (object)DBNull.Value : item.ArtikelWeight);
					sqlCommand.Parameters.AddWithValue("BasisPrice150" + i, item.BasisPrice150 == null ? (object)DBNull.Value : item.BasisPrice150);
					sqlCommand.Parameters.AddWithValue("Bestellt" + i, item.Bestellt == null ? (object)DBNull.Value : item.Bestellt);
					sqlCommand.Parameters.AddWithValue("ClientNumber" + i, item.ClientNumber == null ? (object)DBNull.Value : item.ClientNumber);
					sqlCommand.Parameters.AddWithValue("CompanyLogoImageId" + i, item.CompanyLogoImageId);
					sqlCommand.Parameters.AddWithValue("Cu_G" + i, item.Cu_G == null ? (object)DBNull.Value : item.Cu_G);
					sqlCommand.Parameters.AddWithValue("Cu_Surcharge" + i, item.Cu_Surcharge == null ? (object)DBNull.Value : item.Cu_Surcharge);
					sqlCommand.Parameters.AddWithValue("Cu_Total" + i, item.Cu_Total == null ? (object)DBNull.Value : item.Cu_Total);
					sqlCommand.Parameters.AddWithValue("CustomerDate" + i, item.CustomerDate == null ? (object)DBNull.Value : item.CustomerDate);
					sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("DEL" + i, item.DEL == null ? (object)DBNull.Value : item.DEL);
					sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
					sqlCommand.Parameters.AddWithValue("Designation" + i, item.Designation == null ? (object)DBNull.Value : item.Designation);
					sqlCommand.Parameters.AddWithValue("Designation1" + i, item.Designation1 == null ? (object)DBNull.Value : item.Designation1);
					sqlCommand.Parameters.AddWithValue("Designation2" + i, item.Designation2 == null ? (object)DBNull.Value : item.Designation2);
					sqlCommand.Parameters.AddWithValue("DiscountText1" + i, item.DiscountText1 == null ? (object)DBNull.Value : item.DiscountText1);
					sqlCommand.Parameters.AddWithValue("DiscountText2" + i, item.DiscountText2 == null ? (object)DBNull.Value : item.DiscountText2);
					sqlCommand.Parameters.AddWithValue("DiscountText3" + i, item.DiscountText3 == null ? (object)DBNull.Value : item.DiscountText3);
					sqlCommand.Parameters.AddWithValue("DiscountText4" + i, item.DiscountText4 == null ? (object)DBNull.Value : item.DiscountText4);
					sqlCommand.Parameters.AddWithValue("DiscountText5" + i, item.DiscountText5 == null ? (object)DBNull.Value : item.DiscountText5);
					sqlCommand.Parameters.AddWithValue("DiscountText6" + i, item.DiscountText6 == null ? (object)DBNull.Value : item.DiscountText6);
					sqlCommand.Parameters.AddWithValue("DocumentType" + i, item.DocumentType == null ? (object)DBNull.Value : item.DocumentType);
					sqlCommand.Parameters.AddWithValue("FactoringText1" + i, item.FactoringText1 == null ? (object)DBNull.Value : item.FactoringText1);
					sqlCommand.Parameters.AddWithValue("FactoringText2" + i, item.FactoringText2 == null ? (object)DBNull.Value : item.FactoringText2);
					sqlCommand.Parameters.AddWithValue("FactoringText3" + i, item.FactoringText3 == null ? (object)DBNull.Value : item.FactoringText3);
					sqlCommand.Parameters.AddWithValue("FactoringText4" + i, item.FactoringText4 == null ? (object)DBNull.Value : item.FactoringText4);
					sqlCommand.Parameters.AddWithValue("FactoringText5" + i, item.FactoringText5 == null ? (object)DBNull.Value : item.FactoringText5);
					sqlCommand.Parameters.AddWithValue("FactoringText6" + i, item.FactoringText6 == null ? (object)DBNull.Value : item.FactoringText6);
					sqlCommand.Parameters.AddWithValue("Footer11" + i, item.Footer11 == null ? (object)DBNull.Value : item.Footer11);
					sqlCommand.Parameters.AddWithValue("Footer12" + i, item.Footer12 == null ? (object)DBNull.Value : item.Footer12);
					sqlCommand.Parameters.AddWithValue("Footer13" + i, item.Footer13 == null ? (object)DBNull.Value : item.Footer13);
					sqlCommand.Parameters.AddWithValue("Footer14" + i, item.Footer14 == null ? (object)DBNull.Value : item.Footer14);
					sqlCommand.Parameters.AddWithValue("Footer15" + i, item.Footer15 == null ? (object)DBNull.Value : item.Footer15);
					sqlCommand.Parameters.AddWithValue("Footer16" + i, item.Footer16 == null ? (object)DBNull.Value : item.Footer16);
					sqlCommand.Parameters.AddWithValue("Footer17" + i, item.Footer17 == null ? (object)DBNull.Value : item.Footer17);
					sqlCommand.Parameters.AddWithValue("Footer21" + i, item.Footer21 == null ? (object)DBNull.Value : item.Footer21);
					sqlCommand.Parameters.AddWithValue("Footer22" + i, item.Footer22 == null ? (object)DBNull.Value : item.Footer22);
					sqlCommand.Parameters.AddWithValue("Footer23" + i, item.Footer23 == null ? (object)DBNull.Value : item.Footer23);
					sqlCommand.Parameters.AddWithValue("Footer24" + i, item.Footer24 == null ? (object)DBNull.Value : item.Footer24);
					sqlCommand.Parameters.AddWithValue("Footer25" + i, item.Footer25 == null ? (object)DBNull.Value : item.Footer25);
					sqlCommand.Parameters.AddWithValue("Footer26" + i, item.Footer26 == null ? (object)DBNull.Value : item.Footer26);
					sqlCommand.Parameters.AddWithValue("Footer27" + i, item.Footer27 == null ? (object)DBNull.Value : item.Footer27);
					sqlCommand.Parameters.AddWithValue("Footer31" + i, item.Footer31 == null ? (object)DBNull.Value : item.Footer31);
					sqlCommand.Parameters.AddWithValue("Footer32" + i, item.Footer32 == null ? (object)DBNull.Value : item.Footer32);
					sqlCommand.Parameters.AddWithValue("Footer33" + i, item.Footer33 == null ? (object)DBNull.Value : item.Footer33);
					sqlCommand.Parameters.AddWithValue("Footer34" + i, item.Footer34 == null ? (object)DBNull.Value : item.Footer34);
					sqlCommand.Parameters.AddWithValue("Footer35" + i, item.Footer35 == null ? (object)DBNull.Value : item.Footer35);
					sqlCommand.Parameters.AddWithValue("Footer36" + i, item.Footer36 == null ? (object)DBNull.Value : item.Footer36);
					sqlCommand.Parameters.AddWithValue("Footer37" + i, item.Footer37 == null ? (object)DBNull.Value : item.Footer37);
					sqlCommand.Parameters.AddWithValue("Footer41" + i, item.Footer41 == null ? (object)DBNull.Value : item.Footer41);
					sqlCommand.Parameters.AddWithValue("Footer42" + i, item.Footer42 == null ? (object)DBNull.Value : item.Footer42);
					sqlCommand.Parameters.AddWithValue("Footer43" + i, item.Footer43 == null ? (object)DBNull.Value : item.Footer43);
					sqlCommand.Parameters.AddWithValue("Footer44" + i, item.Footer44 == null ? (object)DBNull.Value : item.Footer44);
					sqlCommand.Parameters.AddWithValue("Footer45" + i, item.Footer45 == null ? (object)DBNull.Value : item.Footer45);
					sqlCommand.Parameters.AddWithValue("Footer46" + i, item.Footer46 == null ? (object)DBNull.Value : item.Footer46);
					sqlCommand.Parameters.AddWithValue("Footer47" + i, item.Footer47 == null ? (object)DBNull.Value : item.Footer47);
					sqlCommand.Parameters.AddWithValue("Footer51" + i, item.Footer51 == null ? (object)DBNull.Value : item.Footer51);
					sqlCommand.Parameters.AddWithValue("Footer52" + i, item.Footer52 == null ? (object)DBNull.Value : item.Footer52);
					sqlCommand.Parameters.AddWithValue("Footer53" + i, item.Footer53 == null ? (object)DBNull.Value : item.Footer53);
					sqlCommand.Parameters.AddWithValue("Footer54" + i, item.Footer54 == null ? (object)DBNull.Value : item.Footer54);
					sqlCommand.Parameters.AddWithValue("Footer55" + i, item.Footer55 == null ? (object)DBNull.Value : item.Footer55);
					sqlCommand.Parameters.AddWithValue("Footer56" + i, item.Footer56 == null ? (object)DBNull.Value : item.Footer56);
					sqlCommand.Parameters.AddWithValue("Footer57" + i, item.Footer57 == null ? (object)DBNull.Value : item.Footer57);
					sqlCommand.Parameters.AddWithValue("Footer61" + i, item.Footer61 == null ? (object)DBNull.Value : item.Footer61);
					sqlCommand.Parameters.AddWithValue("Footer62" + i, item.Footer62 == null ? (object)DBNull.Value : item.Footer62);
					sqlCommand.Parameters.AddWithValue("Footer63" + i, item.Footer63 == null ? (object)DBNull.Value : item.Footer63);
					sqlCommand.Parameters.AddWithValue("Footer64" + i, item.Footer64 == null ? (object)DBNull.Value : item.Footer64);
					sqlCommand.Parameters.AddWithValue("Footer65" + i, item.Footer65 == null ? (object)DBNull.Value : item.Footer65);
					sqlCommand.Parameters.AddWithValue("Footer66" + i, item.Footer66 == null ? (object)DBNull.Value : item.Footer66);
					sqlCommand.Parameters.AddWithValue("Footer67" + i, item.Footer67 == null ? (object)DBNull.Value : item.Footer67);
					sqlCommand.Parameters.AddWithValue("Footer71" + i, item.Footer71 == null ? (object)DBNull.Value : item.Footer71);
					sqlCommand.Parameters.AddWithValue("Footer72" + i, item.Footer72 == null ? (object)DBNull.Value : item.Footer72);
					sqlCommand.Parameters.AddWithValue("Footer73" + i, item.Footer73 == null ? (object)DBNull.Value : item.Footer73);
					sqlCommand.Parameters.AddWithValue("Footer74" + i, item.Footer74 == null ? (object)DBNull.Value : item.Footer74);
					sqlCommand.Parameters.AddWithValue("Footer75" + i, item.Footer75 == null ? (object)DBNull.Value : item.Footer75);
					sqlCommand.Parameters.AddWithValue("Footer76" + i, item.Footer76 == null ? (object)DBNull.Value : item.Footer76);
					sqlCommand.Parameters.AddWithValue("Footer77" + i, item.Footer77 == null ? (object)DBNull.Value : item.Footer77);
					sqlCommand.Parameters.AddWithValue("ForDeliveryNote" + i, item.ForDeliveryNote == null ? (object)DBNull.Value : item.ForDeliveryNote);
					sqlCommand.Parameters.AddWithValue("ForPosDeliveryNote" + i, item.ForPosDeliveryNote == null ? (object)DBNull.Value : item.ForPosDeliveryNote);
					sqlCommand.Parameters.AddWithValue("Geliefert" + i, item.Geliefert == null ? (object)DBNull.Value : item.Geliefert);
					sqlCommand.Parameters.AddWithValue("Header" + i, item.Header == null ? (object)DBNull.Value : item.Header);
					sqlCommand.Parameters.AddWithValue("ImportLogoImageId" + i, item.ImportLogoImageId);
					sqlCommand.Parameters.AddWithValue("Index_Kunde" + i, item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
					sqlCommand.Parameters.AddWithValue("InternalNumber" + i, item.InternalNumber == null ? (object)DBNull.Value : item.InternalNumber);
					sqlCommand.Parameters.AddWithValue("ItemsFooter1" + i, item.ItemsFooter1 == null ? (object)DBNull.Value : item.ItemsFooter1);
					sqlCommand.Parameters.AddWithValue("ItemsFooter2" + i, item.ItemsFooter2 == null ? (object)DBNull.Value : item.ItemsFooter2);
					sqlCommand.Parameters.AddWithValue("ItemsHeader" + i, item.ItemsHeader == null ? (object)DBNull.Value : item.ItemsHeader);
					sqlCommand.Parameters.AddWithValue("LanguageId" + i, item.LanguageId);
					sqlCommand.Parameters.AddWithValue("LastPageText1" + i, item.LastPageText1 == null ? (object)DBNull.Value : item.LastPageText1);
					sqlCommand.Parameters.AddWithValue("LastPageText10" + i, item.LastPageText10 == null ? (object)DBNull.Value : item.LastPageText10);
					sqlCommand.Parameters.AddWithValue("LastPageText11" + i, item.LastPageText11 == null ? (object)DBNull.Value : item.LastPageText11);
					sqlCommand.Parameters.AddWithValue("LastPageText2" + i, item.LastPageText2 == null ? (object)DBNull.Value : item.LastPageText2);
					sqlCommand.Parameters.AddWithValue("LastPageText3" + i, item.LastPageText3 == null ? (object)DBNull.Value : item.LastPageText3);
					sqlCommand.Parameters.AddWithValue("LastPageText4" + i, item.LastPageText4 == null ? (object)DBNull.Value : item.LastPageText4);
					sqlCommand.Parameters.AddWithValue("LastPageText5" + i, item.LastPageText5 == null ? (object)DBNull.Value : item.LastPageText5);
					sqlCommand.Parameters.AddWithValue("LastPageText6" + i, item.LastPageText6 == null ? (object)DBNull.Value : item.LastPageText6);
					sqlCommand.Parameters.AddWithValue("LastPageText7" + i, item.LastPageText7 == null ? (object)DBNull.Value : item.LastPageText7);
					sqlCommand.Parameters.AddWithValue("LastPageText8" + i, item.LastPageText8 == null ? (object)DBNull.Value : item.LastPageText8);
					sqlCommand.Parameters.AddWithValue("LastPageText9" + i, item.LastPageText9 == null ? (object)DBNull.Value : item.LastPageText9);
					sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
					sqlCommand.Parameters.AddWithValue("LastUpdateUserId" + i, item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
					sqlCommand.Parameters.AddWithValue("Lieferadresse" + i, item.Lieferadresse == null ? (object)DBNull.Value : item.Lieferadresse);
					sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("Offen" + i, item.Offen == null ? (object)DBNull.Value : item.Offen);
					sqlCommand.Parameters.AddWithValue("OrderDate" + i, item.OrderDate == null ? (object)DBNull.Value : item.OrderDate);
					sqlCommand.Parameters.AddWithValue("OrderNumber" + i, item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
					sqlCommand.Parameters.AddWithValue("OrderNumberPO" + i, item.OrderNumberPO == null ? (object)DBNull.Value : item.OrderNumberPO);
					sqlCommand.Parameters.AddWithValue("OrderTypeId" + i, item.OrderTypeId);
					sqlCommand.Parameters.AddWithValue("PaymentMethod" + i, item.PaymentMethod == null ? (object)DBNull.Value : item.PaymentMethod);
					sqlCommand.Parameters.AddWithValue("PaymentTarget" + i, item.PaymentTarget == null ? (object)DBNull.Value : item.PaymentTarget);
					sqlCommand.Parameters.AddWithValue("PE" + i, item.PE == null ? (object)DBNull.Value : item.PE);
					sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
					sqlCommand.Parameters.AddWithValue("ShippingMethod" + i, item.ShippingMethod == null ? (object)DBNull.Value : item.ShippingMethod);
					sqlCommand.Parameters.AddWithValue("SummarySum" + i, item.SummarySum == null ? (object)DBNull.Value : item.SummarySum);
					sqlCommand.Parameters.AddWithValue("SummaryTotal" + i, item.SummaryTotal == null ? (object)DBNull.Value : item.SummaryTotal);
					sqlCommand.Parameters.AddWithValue("SummaryUST" + i, item.SummaryUST == null ? (object)DBNull.Value : item.SummaryUST);
					sqlCommand.Parameters.AddWithValue("TotalPrice150" + i, item.TotalPrice150 == null ? (object)DBNull.Value : item.TotalPrice150);
					sqlCommand.Parameters.AddWithValue("Unit" + i, item.Unit == null ? (object)DBNull.Value : item.Unit);
					sqlCommand.Parameters.AddWithValue("UnitPrice" + i, item.UnitPrice == null ? (object)DBNull.Value : item.UnitPrice);
					sqlCommand.Parameters.AddWithValue("UnitTotal" + i, item.UnitTotal == null ? (object)DBNull.Value : item.UnitTotal);
					sqlCommand.Parameters.AddWithValue("UST_ID" + i, item.UST_ID == null ? (object)DBNull.Value : item.UST_ID);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__PRS_OrderReport] SET [Abladestelle]=@Abladestelle, [AccountOwner1]=@AccountOwner1, [AccountOwner2]=@AccountOwner2, [AccountOwner3]=@AccountOwner3, [AccountOwner4]=@AccountOwner4, [Address1]=@Address1, [Address2]=@Address2, [Address3]=@Address3, [Address4]=@Address4, [Amount]=@Amount, [Article]=@Article, [ArtikelCountry]=@ArtikelCountry, [ArtikelPrice]=@ArtikelPrice, [ArtikelQuantity]=@ArtikelQuantity, [ArtikelStock]=@ArtikelStock, [ArtikelWeight]=@ArtikelWeight, [BasisPrice150]=@BasisPrice150, [Bestellt]=@Bestellt, [ClientNumber]=@ClientNumber, [CompanyLogoImageId]=@CompanyLogoImageId, [Cu_G]=@Cu_G, [Cu_Surcharge]=@Cu_Surcharge, [Cu_Total]=@Cu_Total, [CustomerDate]=@CustomerDate, [CustomerNumber]=@CustomerNumber, [DEL]=@DEL, [Description]=@Description, [Designation]=@Designation, [Designation1]=@Designation1, [Designation2]=@Designation2, [DiscountText1]=@DiscountText1, [DiscountText2]=@DiscountText2, [DiscountText3]=@DiscountText3, [DiscountText4]=@DiscountText4, [DiscountText5]=@DiscountText5, [DiscountText6]=@DiscountText6, [DocumentType]=@DocumentType, [FactoringText1]=@FactoringText1, [FactoringText2]=@FactoringText2, [FactoringText3]=@FactoringText3, [FactoringText4]=@FactoringText4, [FactoringText5]=@FactoringText5, [FactoringText6]=@FactoringText6, [Footer11]=@Footer11, [Footer12]=@Footer12, [Footer13]=@Footer13, [Footer14]=@Footer14, [Footer15]=@Footer15, [Footer16]=@Footer16, [Footer17]=@Footer17, [Footer21]=@Footer21, [Footer22]=@Footer22, [Footer23]=@Footer23, [Footer24]=@Footer24, [Footer25]=@Footer25, [Footer26]=@Footer26, [Footer27]=@Footer27, [Footer31]=@Footer31, [Footer32]=@Footer32, [Footer33]=@Footer33, [Footer34]=@Footer34, [Footer35]=@Footer35, [Footer36]=@Footer36, [Footer37]=@Footer37, [Footer41]=@Footer41, [Footer42]=@Footer42, [Footer43]=@Footer43, [Footer44]=@Footer44, [Footer45]=@Footer45, [Footer46]=@Footer46, [Footer47]=@Footer47, [Footer51]=@Footer51, [Footer52]=@Footer52, [Footer53]=@Footer53, [Footer54]=@Footer54, [Footer55]=@Footer55, [Footer56]=@Footer56, [Footer57]=@Footer57, [Footer61]=@Footer61, [Footer62]=@Footer62, [Footer63]=@Footer63, [Footer64]=@Footer64, [Footer65]=@Footer65, [Footer66]=@Footer66, [Footer67]=@Footer67, [Footer71]=@Footer71, [Footer72]=@Footer72, [Footer73]=@Footer73, [Footer74]=@Footer74, [Footer75]=@Footer75, [Footer76]=@Footer76, [Footer77]=@Footer77, [ForDeliveryNote]=@ForDeliveryNote, [ForPosDeliveryNote]=@ForPosDeliveryNote, [Geliefert]=@Geliefert, [Header]=@Header, [ImportLogoImageId]=@ImportLogoImageId, [Index_Kunde]=@Index_Kunde, [InternalNumber]=@InternalNumber, [ItemsFooter1]=@ItemsFooter1, [ItemsFooter2]=@ItemsFooter2, [ItemsHeader]=@ItemsHeader, [LanguageId]=@LanguageId, [LastPageText1]=@LastPageText1, [LastPageText10]=@LastPageText10, [LastPageText11]=@LastPageText11, [LastPageText2]=@LastPageText2, [LastPageText3]=@LastPageText3, [LastPageText4]=@LastPageText4, [LastPageText5]=@LastPageText5, [LastPageText6]=@LastPageText6, [LastPageText7]=@LastPageText7, [LastPageText8]=@LastPageText8, [LastPageText9]=@LastPageText9, [LastUpdateTime]=@LastUpdateTime, [LastUpdateUserId]=@LastUpdateUserId, [Lieferadresse]=@Lieferadresse, [Liefertermin]=@Liefertermin, [Offen]=@Offen, [OrderDate]=@OrderDate, [OrderNumber]=@OrderNumber, [OrderNumberPO]=@OrderNumberPO, [OrderTypeId]=@OrderTypeId, [PaymentMethod]=@PaymentMethod, [PaymentTarget]=@PaymentTarget, [PE]=@PE, [Position]=@Position, [ShippingMethod]=@ShippingMethod, [SummarySum]=@SummarySum, [SummaryTotal]=@SummaryTotal, [SummaryUST]=@SummaryUST, [TotalPrice150]=@TotalPrice150, [Unit]=@Unit, [UnitPrice]=@UnitPrice, [UnitTotal]=@UnitTotal, [UST_ID]=@UST_ID WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("Abladestelle", item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
			sqlCommand.Parameters.AddWithValue("AccountOwner1", item.AccountOwner1 == null ? (object)DBNull.Value : item.AccountOwner1);
			sqlCommand.Parameters.AddWithValue("AccountOwner2", item.AccountOwner2 == null ? (object)DBNull.Value : item.AccountOwner2);
			sqlCommand.Parameters.AddWithValue("AccountOwner3", item.AccountOwner3 == null ? (object)DBNull.Value : item.AccountOwner3);
			sqlCommand.Parameters.AddWithValue("AccountOwner4", item.AccountOwner4 == null ? (object)DBNull.Value : item.AccountOwner4);
			sqlCommand.Parameters.AddWithValue("Address1", item.Address1 == null ? (object)DBNull.Value : item.Address1);
			sqlCommand.Parameters.AddWithValue("Address2", item.Address2 == null ? (object)DBNull.Value : item.Address2);
			sqlCommand.Parameters.AddWithValue("Address3", item.Address3 == null ? (object)DBNull.Value : item.Address3);
			sqlCommand.Parameters.AddWithValue("Address4", item.Address4 == null ? (object)DBNull.Value : item.Address4);
			sqlCommand.Parameters.AddWithValue("Amount", item.Amount == null ? (object)DBNull.Value : item.Amount);
			sqlCommand.Parameters.AddWithValue("Article", item.Article == null ? (object)DBNull.Value : item.Article);
			sqlCommand.Parameters.AddWithValue("ArtikelCountry", item.ArtikelCountry == null ? (object)DBNull.Value : item.ArtikelCountry);
			sqlCommand.Parameters.AddWithValue("ArtikelPrice", item.ArtikelPrice == null ? (object)DBNull.Value : item.ArtikelPrice);
			sqlCommand.Parameters.AddWithValue("ArtikelQuantity", item.ArtikelQuantity == null ? (object)DBNull.Value : item.ArtikelQuantity);
			sqlCommand.Parameters.AddWithValue("ArtikelStock", item.ArtikelStock == null ? (object)DBNull.Value : item.ArtikelStock);
			sqlCommand.Parameters.AddWithValue("ArtikelWeight", item.ArtikelWeight == null ? (object)DBNull.Value : item.ArtikelWeight);
			sqlCommand.Parameters.AddWithValue("BasisPrice150", item.BasisPrice150 == null ? (object)DBNull.Value : item.BasisPrice150);
			sqlCommand.Parameters.AddWithValue("Bestellt", item.Bestellt == null ? (object)DBNull.Value : item.Bestellt);
			sqlCommand.Parameters.AddWithValue("ClientNumber", item.ClientNumber == null ? (object)DBNull.Value : item.ClientNumber);
			sqlCommand.Parameters.AddWithValue("CompanyLogoImageId", item.CompanyLogoImageId);
			sqlCommand.Parameters.AddWithValue("Cu_G", item.Cu_G == null ? (object)DBNull.Value : item.Cu_G);
			sqlCommand.Parameters.AddWithValue("Cu_Surcharge", item.Cu_Surcharge == null ? (object)DBNull.Value : item.Cu_Surcharge);
			sqlCommand.Parameters.AddWithValue("Cu_Total", item.Cu_Total == null ? (object)DBNull.Value : item.Cu_Total);
			sqlCommand.Parameters.AddWithValue("CustomerDate", item.CustomerDate == null ? (object)DBNull.Value : item.CustomerDate);
			sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
			sqlCommand.Parameters.AddWithValue("DEL", item.DEL == null ? (object)DBNull.Value : item.DEL);
			sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
			sqlCommand.Parameters.AddWithValue("Designation", item.Designation == null ? (object)DBNull.Value : item.Designation);
			sqlCommand.Parameters.AddWithValue("Designation1", item.Designation1 == null ? (object)DBNull.Value : item.Designation1);
			sqlCommand.Parameters.AddWithValue("Designation2", item.Designation2 == null ? (object)DBNull.Value : item.Designation2);
			sqlCommand.Parameters.AddWithValue("DiscountText1", item.DiscountText1 == null ? (object)DBNull.Value : item.DiscountText1);
			sqlCommand.Parameters.AddWithValue("DiscountText2", item.DiscountText2 == null ? (object)DBNull.Value : item.DiscountText2);
			sqlCommand.Parameters.AddWithValue("DiscountText3", item.DiscountText3 == null ? (object)DBNull.Value : item.DiscountText3);
			sqlCommand.Parameters.AddWithValue("DiscountText4", item.DiscountText4 == null ? (object)DBNull.Value : item.DiscountText4);
			sqlCommand.Parameters.AddWithValue("DiscountText5", item.DiscountText5 == null ? (object)DBNull.Value : item.DiscountText5);
			sqlCommand.Parameters.AddWithValue("DiscountText6", item.DiscountText6 == null ? (object)DBNull.Value : item.DiscountText6);
			sqlCommand.Parameters.AddWithValue("DocumentType", item.DocumentType == null ? (object)DBNull.Value : item.DocumentType);
			sqlCommand.Parameters.AddWithValue("FactoringText1", item.FactoringText1 == null ? (object)DBNull.Value : item.FactoringText1);
			sqlCommand.Parameters.AddWithValue("FactoringText2", item.FactoringText2 == null ? (object)DBNull.Value : item.FactoringText2);
			sqlCommand.Parameters.AddWithValue("FactoringText3", item.FactoringText3 == null ? (object)DBNull.Value : item.FactoringText3);
			sqlCommand.Parameters.AddWithValue("FactoringText4", item.FactoringText4 == null ? (object)DBNull.Value : item.FactoringText4);
			sqlCommand.Parameters.AddWithValue("FactoringText5", item.FactoringText5 == null ? (object)DBNull.Value : item.FactoringText5);
			sqlCommand.Parameters.AddWithValue("FactoringText6", item.FactoringText6 == null ? (object)DBNull.Value : item.FactoringText6);
			sqlCommand.Parameters.AddWithValue("Footer11", item.Footer11 == null ? (object)DBNull.Value : item.Footer11);
			sqlCommand.Parameters.AddWithValue("Footer12", item.Footer12 == null ? (object)DBNull.Value : item.Footer12);
			sqlCommand.Parameters.AddWithValue("Footer13", item.Footer13 == null ? (object)DBNull.Value : item.Footer13);
			sqlCommand.Parameters.AddWithValue("Footer14", item.Footer14 == null ? (object)DBNull.Value : item.Footer14);
			sqlCommand.Parameters.AddWithValue("Footer15", item.Footer15 == null ? (object)DBNull.Value : item.Footer15);
			sqlCommand.Parameters.AddWithValue("Footer16", item.Footer16 == null ? (object)DBNull.Value : item.Footer16);
			sqlCommand.Parameters.AddWithValue("Footer17", item.Footer17 == null ? (object)DBNull.Value : item.Footer17);
			sqlCommand.Parameters.AddWithValue("Footer21", item.Footer21 == null ? (object)DBNull.Value : item.Footer21);
			sqlCommand.Parameters.AddWithValue("Footer22", item.Footer22 == null ? (object)DBNull.Value : item.Footer22);
			sqlCommand.Parameters.AddWithValue("Footer23", item.Footer23 == null ? (object)DBNull.Value : item.Footer23);
			sqlCommand.Parameters.AddWithValue("Footer24", item.Footer24 == null ? (object)DBNull.Value : item.Footer24);
			sqlCommand.Parameters.AddWithValue("Footer25", item.Footer25 == null ? (object)DBNull.Value : item.Footer25);
			sqlCommand.Parameters.AddWithValue("Footer26", item.Footer26 == null ? (object)DBNull.Value : item.Footer26);
			sqlCommand.Parameters.AddWithValue("Footer27", item.Footer27 == null ? (object)DBNull.Value : item.Footer27);
			sqlCommand.Parameters.AddWithValue("Footer31", item.Footer31 == null ? (object)DBNull.Value : item.Footer31);
			sqlCommand.Parameters.AddWithValue("Footer32", item.Footer32 == null ? (object)DBNull.Value : item.Footer32);
			sqlCommand.Parameters.AddWithValue("Footer33", item.Footer33 == null ? (object)DBNull.Value : item.Footer33);
			sqlCommand.Parameters.AddWithValue("Footer34", item.Footer34 == null ? (object)DBNull.Value : item.Footer34);
			sqlCommand.Parameters.AddWithValue("Footer35", item.Footer35 == null ? (object)DBNull.Value : item.Footer35);
			sqlCommand.Parameters.AddWithValue("Footer36", item.Footer36 == null ? (object)DBNull.Value : item.Footer36);
			sqlCommand.Parameters.AddWithValue("Footer37", item.Footer37 == null ? (object)DBNull.Value : item.Footer37);
			sqlCommand.Parameters.AddWithValue("Footer41", item.Footer41 == null ? (object)DBNull.Value : item.Footer41);
			sqlCommand.Parameters.AddWithValue("Footer42", item.Footer42 == null ? (object)DBNull.Value : item.Footer42);
			sqlCommand.Parameters.AddWithValue("Footer43", item.Footer43 == null ? (object)DBNull.Value : item.Footer43);
			sqlCommand.Parameters.AddWithValue("Footer44", item.Footer44 == null ? (object)DBNull.Value : item.Footer44);
			sqlCommand.Parameters.AddWithValue("Footer45", item.Footer45 == null ? (object)DBNull.Value : item.Footer45);
			sqlCommand.Parameters.AddWithValue("Footer46", item.Footer46 == null ? (object)DBNull.Value : item.Footer46);
			sqlCommand.Parameters.AddWithValue("Footer47", item.Footer47 == null ? (object)DBNull.Value : item.Footer47);
			sqlCommand.Parameters.AddWithValue("Footer51", item.Footer51 == null ? (object)DBNull.Value : item.Footer51);
			sqlCommand.Parameters.AddWithValue("Footer52", item.Footer52 == null ? (object)DBNull.Value : item.Footer52);
			sqlCommand.Parameters.AddWithValue("Footer53", item.Footer53 == null ? (object)DBNull.Value : item.Footer53);
			sqlCommand.Parameters.AddWithValue("Footer54", item.Footer54 == null ? (object)DBNull.Value : item.Footer54);
			sqlCommand.Parameters.AddWithValue("Footer55", item.Footer55 == null ? (object)DBNull.Value : item.Footer55);
			sqlCommand.Parameters.AddWithValue("Footer56", item.Footer56 == null ? (object)DBNull.Value : item.Footer56);
			sqlCommand.Parameters.AddWithValue("Footer57", item.Footer57 == null ? (object)DBNull.Value : item.Footer57);
			sqlCommand.Parameters.AddWithValue("Footer61", item.Footer61 == null ? (object)DBNull.Value : item.Footer61);
			sqlCommand.Parameters.AddWithValue("Footer62", item.Footer62 == null ? (object)DBNull.Value : item.Footer62);
			sqlCommand.Parameters.AddWithValue("Footer63", item.Footer63 == null ? (object)DBNull.Value : item.Footer63);
			sqlCommand.Parameters.AddWithValue("Footer64", item.Footer64 == null ? (object)DBNull.Value : item.Footer64);
			sqlCommand.Parameters.AddWithValue("Footer65", item.Footer65 == null ? (object)DBNull.Value : item.Footer65);
			sqlCommand.Parameters.AddWithValue("Footer66", item.Footer66 == null ? (object)DBNull.Value : item.Footer66);
			sqlCommand.Parameters.AddWithValue("Footer67", item.Footer67 == null ? (object)DBNull.Value : item.Footer67);
			sqlCommand.Parameters.AddWithValue("Footer71", item.Footer71 == null ? (object)DBNull.Value : item.Footer71);
			sqlCommand.Parameters.AddWithValue("Footer72", item.Footer72 == null ? (object)DBNull.Value : item.Footer72);
			sqlCommand.Parameters.AddWithValue("Footer73", item.Footer73 == null ? (object)DBNull.Value : item.Footer73);
			sqlCommand.Parameters.AddWithValue("Footer74", item.Footer74 == null ? (object)DBNull.Value : item.Footer74);
			sqlCommand.Parameters.AddWithValue("Footer75", item.Footer75 == null ? (object)DBNull.Value : item.Footer75);
			sqlCommand.Parameters.AddWithValue("Footer76", item.Footer76 == null ? (object)DBNull.Value : item.Footer76);
			sqlCommand.Parameters.AddWithValue("Footer77", item.Footer77 == null ? (object)DBNull.Value : item.Footer77);
			sqlCommand.Parameters.AddWithValue("ForDeliveryNote", item.ForDeliveryNote == null ? (object)DBNull.Value : item.ForDeliveryNote);
			sqlCommand.Parameters.AddWithValue("ForPosDeliveryNote", item.ForPosDeliveryNote == null ? (object)DBNull.Value : item.ForPosDeliveryNote);
			sqlCommand.Parameters.AddWithValue("Geliefert", item.Geliefert == null ? (object)DBNull.Value : item.Geliefert);
			sqlCommand.Parameters.AddWithValue("Header", item.Header == null ? (object)DBNull.Value : item.Header);
			sqlCommand.Parameters.AddWithValue("ImportLogoImageId", item.ImportLogoImageId);
			sqlCommand.Parameters.AddWithValue("Index_Kunde", item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
			sqlCommand.Parameters.AddWithValue("InternalNumber", item.InternalNumber == null ? (object)DBNull.Value : item.InternalNumber);
			sqlCommand.Parameters.AddWithValue("ItemsFooter1", item.ItemsFooter1 == null ? (object)DBNull.Value : item.ItemsFooter1);
			sqlCommand.Parameters.AddWithValue("ItemsFooter2", item.ItemsFooter2 == null ? (object)DBNull.Value : item.ItemsFooter2);
			sqlCommand.Parameters.AddWithValue("ItemsHeader", item.ItemsHeader == null ? (object)DBNull.Value : item.ItemsHeader);
			sqlCommand.Parameters.AddWithValue("LanguageId", item.LanguageId);
			sqlCommand.Parameters.AddWithValue("LastPageText1", item.LastPageText1 == null ? (object)DBNull.Value : item.LastPageText1);
			sqlCommand.Parameters.AddWithValue("LastPageText10", item.LastPageText10 == null ? (object)DBNull.Value : item.LastPageText10);
			sqlCommand.Parameters.AddWithValue("LastPageText11", item.LastPageText11 == null ? (object)DBNull.Value : item.LastPageText11);
			sqlCommand.Parameters.AddWithValue("LastPageText2", item.LastPageText2 == null ? (object)DBNull.Value : item.LastPageText2);
			sqlCommand.Parameters.AddWithValue("LastPageText3", item.LastPageText3 == null ? (object)DBNull.Value : item.LastPageText3);
			sqlCommand.Parameters.AddWithValue("LastPageText4", item.LastPageText4 == null ? (object)DBNull.Value : item.LastPageText4);
			sqlCommand.Parameters.AddWithValue("LastPageText5", item.LastPageText5 == null ? (object)DBNull.Value : item.LastPageText5);
			sqlCommand.Parameters.AddWithValue("LastPageText6", item.LastPageText6 == null ? (object)DBNull.Value : item.LastPageText6);
			sqlCommand.Parameters.AddWithValue("LastPageText7", item.LastPageText7 == null ? (object)DBNull.Value : item.LastPageText7);
			sqlCommand.Parameters.AddWithValue("LastPageText8", item.LastPageText8 == null ? (object)DBNull.Value : item.LastPageText8);
			sqlCommand.Parameters.AddWithValue("LastPageText9", item.LastPageText9 == null ? (object)DBNull.Value : item.LastPageText9);
			sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
			sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
			sqlCommand.Parameters.AddWithValue("Lieferadresse", item.Lieferadresse == null ? (object)DBNull.Value : item.Lieferadresse);
			sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
			sqlCommand.Parameters.AddWithValue("Offen", item.Offen == null ? (object)DBNull.Value : item.Offen);
			sqlCommand.Parameters.AddWithValue("OrderDate", item.OrderDate == null ? (object)DBNull.Value : item.OrderDate);
			sqlCommand.Parameters.AddWithValue("OrderNumber", item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
			sqlCommand.Parameters.AddWithValue("OrderNumberPO", item.OrderNumberPO == null ? (object)DBNull.Value : item.OrderNumberPO);
			sqlCommand.Parameters.AddWithValue("OrderTypeId", item.OrderTypeId);
			sqlCommand.Parameters.AddWithValue("PaymentMethod", item.PaymentMethod == null ? (object)DBNull.Value : item.PaymentMethod);
			sqlCommand.Parameters.AddWithValue("PaymentTarget", item.PaymentTarget == null ? (object)DBNull.Value : item.PaymentTarget);
			sqlCommand.Parameters.AddWithValue("PE", item.PE == null ? (object)DBNull.Value : item.PE);
			sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
			sqlCommand.Parameters.AddWithValue("ShippingMethod", item.ShippingMethod == null ? (object)DBNull.Value : item.ShippingMethod);
			sqlCommand.Parameters.AddWithValue("SummarySum", item.SummarySum == null ? (object)DBNull.Value : item.SummarySum);
			sqlCommand.Parameters.AddWithValue("SummaryTotal", item.SummaryTotal == null ? (object)DBNull.Value : item.SummaryTotal);
			sqlCommand.Parameters.AddWithValue("SummaryUST", item.SummaryUST == null ? (object)DBNull.Value : item.SummaryUST);
			sqlCommand.Parameters.AddWithValue("TotalPrice150", item.TotalPrice150 == null ? (object)DBNull.Value : item.TotalPrice150);
			sqlCommand.Parameters.AddWithValue("Unit", item.Unit == null ? (object)DBNull.Value : item.Unit);
			sqlCommand.Parameters.AddWithValue("UnitPrice", item.UnitPrice == null ? (object)DBNull.Value : item.UnitPrice);
			sqlCommand.Parameters.AddWithValue("UnitTotal", item.UnitTotal == null ? (object)DBNull.Value : item.UnitTotal);
			sqlCommand.Parameters.AddWithValue("UST_ID", item.UST_ID == null ? (object)DBNull.Value : item.UST_ID);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 137; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__PRS_OrderReport] SET "

					+ "[Abladestelle]=@Abladestelle" + i + ","
					+ "[AccountOwner1]=@AccountOwner1" + i + ","
					+ "[AccountOwner2]=@AccountOwner2" + i + ","
					+ "[AccountOwner3]=@AccountOwner3" + i + ","
					+ "[AccountOwner4]=@AccountOwner4" + i + ","
					+ "[Address1]=@Address1" + i + ","
					+ "[Address2]=@Address2" + i + ","
					+ "[Address3]=@Address3" + i + ","
					+ "[Address4]=@Address4" + i + ","
					+ "[Amount]=@Amount" + i + ","
					+ "[Article]=@Article" + i + ","
					+ "[ArtikelCountry]=@ArtikelCountry" + i + ","
					+ "[ArtikelPrice]=@ArtikelPrice" + i + ","
					+ "[ArtikelQuantity]=@ArtikelQuantity" + i + ","
					+ "[ArtikelStock]=@ArtikelStock" + i + ","
					+ "[ArtikelWeight]=@ArtikelWeight" + i + ","
					+ "[BasisPrice150]=@BasisPrice150" + i + ","
					+ "[Bestellt]=@Bestellt" + i + ","
					+ "[ClientNumber]=@ClientNumber" + i + ","
					+ "[CompanyLogoImageId]=@CompanyLogoImageId" + i + ","
					+ "[Cu_G]=@Cu_G" + i + ","
					+ "[Cu_Surcharge]=@Cu_Surcharge" + i + ","
					+ "[Cu_Total]=@Cu_Total" + i + ","
					+ "[CustomerDate]=@CustomerDate" + i + ","
					+ "[CustomerNumber]=@CustomerNumber" + i + ","
					+ "[DEL]=@DEL" + i + ","
					+ "[Description]=@Description" + i + ","
					+ "[Designation]=@Designation" + i + ","
					+ "[Designation1]=@Designation1" + i + ","
					+ "[Designation2]=@Designation2" + i + ","
					+ "[DiscountText1]=@DiscountText1" + i + ","
					+ "[DiscountText2]=@DiscountText2" + i + ","
					+ "[DiscountText3]=@DiscountText3" + i + ","
					+ "[DiscountText4]=@DiscountText4" + i + ","
					+ "[DiscountText5]=@DiscountText5" + i + ","
					+ "[DiscountText6]=@DiscountText6" + i + ","
					+ "[DocumentType]=@DocumentType" + i + ","
					+ "[FactoringText1]=@FactoringText1" + i + ","
					+ "[FactoringText2]=@FactoringText2" + i + ","
					+ "[FactoringText3]=@FactoringText3" + i + ","
					+ "[FactoringText4]=@FactoringText4" + i + ","
					+ "[FactoringText5]=@FactoringText5" + i + ","
					+ "[FactoringText6]=@FactoringText6" + i + ","
					+ "[Footer11]=@Footer11" + i + ","
					+ "[Footer12]=@Footer12" + i + ","
					+ "[Footer13]=@Footer13" + i + ","
					+ "[Footer14]=@Footer14" + i + ","
					+ "[Footer15]=@Footer15" + i + ","
					+ "[Footer16]=@Footer16" + i + ","
					+ "[Footer17]=@Footer17" + i + ","
					+ "[Footer21]=@Footer21" + i + ","
					+ "[Footer22]=@Footer22" + i + ","
					+ "[Footer23]=@Footer23" + i + ","
					+ "[Footer24]=@Footer24" + i + ","
					+ "[Footer25]=@Footer25" + i + ","
					+ "[Footer26]=@Footer26" + i + ","
					+ "[Footer27]=@Footer27" + i + ","
					+ "[Footer31]=@Footer31" + i + ","
					+ "[Footer32]=@Footer32" + i + ","
					+ "[Footer33]=@Footer33" + i + ","
					+ "[Footer34]=@Footer34" + i + ","
					+ "[Footer35]=@Footer35" + i + ","
					+ "[Footer36]=@Footer36" + i + ","
					+ "[Footer37]=@Footer37" + i + ","
					+ "[Footer41]=@Footer41" + i + ","
					+ "[Footer42]=@Footer42" + i + ","
					+ "[Footer43]=@Footer43" + i + ","
					+ "[Footer44]=@Footer44" + i + ","
					+ "[Footer45]=@Footer45" + i + ","
					+ "[Footer46]=@Footer46" + i + ","
					+ "[Footer47]=@Footer47" + i + ","
					+ "[Footer51]=@Footer51" + i + ","
					+ "[Footer52]=@Footer52" + i + ","
					+ "[Footer53]=@Footer53" + i + ","
					+ "[Footer54]=@Footer54" + i + ","
					+ "[Footer55]=@Footer55" + i + ","
					+ "[Footer56]=@Footer56" + i + ","
					+ "[Footer57]=@Footer57" + i + ","
					+ "[Footer61]=@Footer61" + i + ","
					+ "[Footer62]=@Footer62" + i + ","
					+ "[Footer63]=@Footer63" + i + ","
					+ "[Footer64]=@Footer64" + i + ","
					+ "[Footer65]=@Footer65" + i + ","
					+ "[Footer66]=@Footer66" + i + ","
					+ "[Footer67]=@Footer67" + i + ","
					+ "[Footer71]=@Footer71" + i + ","
					+ "[Footer72]=@Footer72" + i + ","
					+ "[Footer73]=@Footer73" + i + ","
					+ "[Footer74]=@Footer74" + i + ","
					+ "[Footer75]=@Footer75" + i + ","
					+ "[Footer76]=@Footer76" + i + ","
					+ "[Footer77]=@Footer77" + i + ","
					+ "[ForDeliveryNote]=@ForDeliveryNote" + i + ","
					+ "[ForPosDeliveryNote]=@ForPosDeliveryNote" + i + ","
					+ "[Geliefert]=@Geliefert" + i + ","
					+ "[Header]=@Header" + i + ","
					+ "[ImportLogoImageId]=@ImportLogoImageId" + i + ","
					+ "[Index_Kunde]=@Index_Kunde" + i + ","
					+ "[InternalNumber]=@InternalNumber" + i + ","
					+ "[ItemsFooter1]=@ItemsFooter1" + i + ","
					+ "[ItemsFooter2]=@ItemsFooter2" + i + ","
					+ "[ItemsHeader]=@ItemsHeader" + i + ","
					+ "[LanguageId]=@LanguageId" + i + ","
					+ "[LastPageText1]=@LastPageText1" + i + ","
					+ "[LastPageText10]=@LastPageText10" + i + ","
					+ "[LastPageText11]=@LastPageText11" + i + ","
					+ "[LastPageText2]=@LastPageText2" + i + ","
					+ "[LastPageText3]=@LastPageText3" + i + ","
					+ "[LastPageText4]=@LastPageText4" + i + ","
					+ "[LastPageText5]=@LastPageText5" + i + ","
					+ "[LastPageText6]=@LastPageText6" + i + ","
					+ "[LastPageText7]=@LastPageText7" + i + ","
					+ "[LastPageText8]=@LastPageText8" + i + ","
					+ "[LastPageText9]=@LastPageText9" + i + ","
					+ "[LastUpdateTime]=@LastUpdateTime" + i + ","
					+ "[LastUpdateUserId]=@LastUpdateUserId" + i + ","
					+ "[Lieferadresse]=@Lieferadresse" + i + ","
					+ "[Liefertermin]=@Liefertermin" + i + ","
					+ "[Offen]=@Offen" + i + ","
					+ "[OrderDate]=@OrderDate" + i + ","
					+ "[OrderNumber]=@OrderNumber" + i + ","
					+ "[OrderNumberPO]=@OrderNumberPO" + i + ","
					+ "[OrderTypeId]=@OrderTypeId" + i + ","
					+ "[PaymentMethod]=@PaymentMethod" + i + ","
					+ "[PaymentTarget]=@PaymentTarget" + i + ","
					+ "[PE]=@PE" + i + ","
					+ "[Position]=@Position" + i + ","
					+ "[ShippingMethod]=@ShippingMethod" + i + ","
					+ "[SummarySum]=@SummarySum" + i + ","
					+ "[SummaryTotal]=@SummaryTotal" + i + ","
					+ "[SummaryUST]=@SummaryUST" + i + ","
					+ "[TotalPrice150]=@TotalPrice150" + i + ","
					+ "[Unit]=@Unit" + i + ","
					+ "[UnitPrice]=@UnitPrice" + i + ","
					+ "[UnitTotal]=@UnitTotal" + i + ","
					+ "[UST_ID]=@UST_ID" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("Abladestelle" + i, item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
					sqlCommand.Parameters.AddWithValue("AccountOwner1" + i, item.AccountOwner1 == null ? (object)DBNull.Value : item.AccountOwner1);
					sqlCommand.Parameters.AddWithValue("AccountOwner2" + i, item.AccountOwner2 == null ? (object)DBNull.Value : item.AccountOwner2);
					sqlCommand.Parameters.AddWithValue("AccountOwner3" + i, item.AccountOwner3 == null ? (object)DBNull.Value : item.AccountOwner3);
					sqlCommand.Parameters.AddWithValue("AccountOwner4" + i, item.AccountOwner4 == null ? (object)DBNull.Value : item.AccountOwner4);
					sqlCommand.Parameters.AddWithValue("Address1" + i, item.Address1 == null ? (object)DBNull.Value : item.Address1);
					sqlCommand.Parameters.AddWithValue("Address2" + i, item.Address2 == null ? (object)DBNull.Value : item.Address2);
					sqlCommand.Parameters.AddWithValue("Address3" + i, item.Address3 == null ? (object)DBNull.Value : item.Address3);
					sqlCommand.Parameters.AddWithValue("Address4" + i, item.Address4 == null ? (object)DBNull.Value : item.Address4);
					sqlCommand.Parameters.AddWithValue("Amount" + i, item.Amount == null ? (object)DBNull.Value : item.Amount);
					sqlCommand.Parameters.AddWithValue("Article" + i, item.Article == null ? (object)DBNull.Value : item.Article);
					sqlCommand.Parameters.AddWithValue("ArtikelCountry" + i, item.ArtikelCountry == null ? (object)DBNull.Value : item.ArtikelCountry);
					sqlCommand.Parameters.AddWithValue("ArtikelPrice" + i, item.ArtikelPrice == null ? (object)DBNull.Value : item.ArtikelPrice);
					sqlCommand.Parameters.AddWithValue("ArtikelQuantity" + i, item.ArtikelQuantity == null ? (object)DBNull.Value : item.ArtikelQuantity);
					sqlCommand.Parameters.AddWithValue("ArtikelStock" + i, item.ArtikelStock == null ? (object)DBNull.Value : item.ArtikelStock);
					sqlCommand.Parameters.AddWithValue("ArtikelWeight" + i, item.ArtikelWeight == null ? (object)DBNull.Value : item.ArtikelWeight);
					sqlCommand.Parameters.AddWithValue("BasisPrice150" + i, item.BasisPrice150 == null ? (object)DBNull.Value : item.BasisPrice150);
					sqlCommand.Parameters.AddWithValue("Bestellt" + i, item.Bestellt == null ? (object)DBNull.Value : item.Bestellt);
					sqlCommand.Parameters.AddWithValue("ClientNumber" + i, item.ClientNumber == null ? (object)DBNull.Value : item.ClientNumber);
					sqlCommand.Parameters.AddWithValue("CompanyLogoImageId" + i, item.CompanyLogoImageId);
					sqlCommand.Parameters.AddWithValue("Cu_G" + i, item.Cu_G == null ? (object)DBNull.Value : item.Cu_G);
					sqlCommand.Parameters.AddWithValue("Cu_Surcharge" + i, item.Cu_Surcharge == null ? (object)DBNull.Value : item.Cu_Surcharge);
					sqlCommand.Parameters.AddWithValue("Cu_Total" + i, item.Cu_Total == null ? (object)DBNull.Value : item.Cu_Total);
					sqlCommand.Parameters.AddWithValue("CustomerDate" + i, item.CustomerDate == null ? (object)DBNull.Value : item.CustomerDate);
					sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("DEL" + i, item.DEL == null ? (object)DBNull.Value : item.DEL);
					sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
					sqlCommand.Parameters.AddWithValue("Designation" + i, item.Designation == null ? (object)DBNull.Value : item.Designation);
					sqlCommand.Parameters.AddWithValue("Designation1" + i, item.Designation1 == null ? (object)DBNull.Value : item.Designation1);
					sqlCommand.Parameters.AddWithValue("Designation2" + i, item.Designation2 == null ? (object)DBNull.Value : item.Designation2);
					sqlCommand.Parameters.AddWithValue("DiscountText1" + i, item.DiscountText1 == null ? (object)DBNull.Value : item.DiscountText1);
					sqlCommand.Parameters.AddWithValue("DiscountText2" + i, item.DiscountText2 == null ? (object)DBNull.Value : item.DiscountText2);
					sqlCommand.Parameters.AddWithValue("DiscountText3" + i, item.DiscountText3 == null ? (object)DBNull.Value : item.DiscountText3);
					sqlCommand.Parameters.AddWithValue("DiscountText4" + i, item.DiscountText4 == null ? (object)DBNull.Value : item.DiscountText4);
					sqlCommand.Parameters.AddWithValue("DiscountText5" + i, item.DiscountText5 == null ? (object)DBNull.Value : item.DiscountText5);
					sqlCommand.Parameters.AddWithValue("DiscountText6" + i, item.DiscountText6 == null ? (object)DBNull.Value : item.DiscountText6);
					sqlCommand.Parameters.AddWithValue("DocumentType" + i, item.DocumentType == null ? (object)DBNull.Value : item.DocumentType);
					sqlCommand.Parameters.AddWithValue("FactoringText1" + i, item.FactoringText1 == null ? (object)DBNull.Value : item.FactoringText1);
					sqlCommand.Parameters.AddWithValue("FactoringText2" + i, item.FactoringText2 == null ? (object)DBNull.Value : item.FactoringText2);
					sqlCommand.Parameters.AddWithValue("FactoringText3" + i, item.FactoringText3 == null ? (object)DBNull.Value : item.FactoringText3);
					sqlCommand.Parameters.AddWithValue("FactoringText4" + i, item.FactoringText4 == null ? (object)DBNull.Value : item.FactoringText4);
					sqlCommand.Parameters.AddWithValue("FactoringText5" + i, item.FactoringText5 == null ? (object)DBNull.Value : item.FactoringText5);
					sqlCommand.Parameters.AddWithValue("FactoringText6" + i, item.FactoringText6 == null ? (object)DBNull.Value : item.FactoringText6);
					sqlCommand.Parameters.AddWithValue("Footer11" + i, item.Footer11 == null ? (object)DBNull.Value : item.Footer11);
					sqlCommand.Parameters.AddWithValue("Footer12" + i, item.Footer12 == null ? (object)DBNull.Value : item.Footer12);
					sqlCommand.Parameters.AddWithValue("Footer13" + i, item.Footer13 == null ? (object)DBNull.Value : item.Footer13);
					sqlCommand.Parameters.AddWithValue("Footer14" + i, item.Footer14 == null ? (object)DBNull.Value : item.Footer14);
					sqlCommand.Parameters.AddWithValue("Footer15" + i, item.Footer15 == null ? (object)DBNull.Value : item.Footer15);
					sqlCommand.Parameters.AddWithValue("Footer16" + i, item.Footer16 == null ? (object)DBNull.Value : item.Footer16);
					sqlCommand.Parameters.AddWithValue("Footer17" + i, item.Footer17 == null ? (object)DBNull.Value : item.Footer17);
					sqlCommand.Parameters.AddWithValue("Footer21" + i, item.Footer21 == null ? (object)DBNull.Value : item.Footer21);
					sqlCommand.Parameters.AddWithValue("Footer22" + i, item.Footer22 == null ? (object)DBNull.Value : item.Footer22);
					sqlCommand.Parameters.AddWithValue("Footer23" + i, item.Footer23 == null ? (object)DBNull.Value : item.Footer23);
					sqlCommand.Parameters.AddWithValue("Footer24" + i, item.Footer24 == null ? (object)DBNull.Value : item.Footer24);
					sqlCommand.Parameters.AddWithValue("Footer25" + i, item.Footer25 == null ? (object)DBNull.Value : item.Footer25);
					sqlCommand.Parameters.AddWithValue("Footer26" + i, item.Footer26 == null ? (object)DBNull.Value : item.Footer26);
					sqlCommand.Parameters.AddWithValue("Footer27" + i, item.Footer27 == null ? (object)DBNull.Value : item.Footer27);
					sqlCommand.Parameters.AddWithValue("Footer31" + i, item.Footer31 == null ? (object)DBNull.Value : item.Footer31);
					sqlCommand.Parameters.AddWithValue("Footer32" + i, item.Footer32 == null ? (object)DBNull.Value : item.Footer32);
					sqlCommand.Parameters.AddWithValue("Footer33" + i, item.Footer33 == null ? (object)DBNull.Value : item.Footer33);
					sqlCommand.Parameters.AddWithValue("Footer34" + i, item.Footer34 == null ? (object)DBNull.Value : item.Footer34);
					sqlCommand.Parameters.AddWithValue("Footer35" + i, item.Footer35 == null ? (object)DBNull.Value : item.Footer35);
					sqlCommand.Parameters.AddWithValue("Footer36" + i, item.Footer36 == null ? (object)DBNull.Value : item.Footer36);
					sqlCommand.Parameters.AddWithValue("Footer37" + i, item.Footer37 == null ? (object)DBNull.Value : item.Footer37);
					sqlCommand.Parameters.AddWithValue("Footer41" + i, item.Footer41 == null ? (object)DBNull.Value : item.Footer41);
					sqlCommand.Parameters.AddWithValue("Footer42" + i, item.Footer42 == null ? (object)DBNull.Value : item.Footer42);
					sqlCommand.Parameters.AddWithValue("Footer43" + i, item.Footer43 == null ? (object)DBNull.Value : item.Footer43);
					sqlCommand.Parameters.AddWithValue("Footer44" + i, item.Footer44 == null ? (object)DBNull.Value : item.Footer44);
					sqlCommand.Parameters.AddWithValue("Footer45" + i, item.Footer45 == null ? (object)DBNull.Value : item.Footer45);
					sqlCommand.Parameters.AddWithValue("Footer46" + i, item.Footer46 == null ? (object)DBNull.Value : item.Footer46);
					sqlCommand.Parameters.AddWithValue("Footer47" + i, item.Footer47 == null ? (object)DBNull.Value : item.Footer47);
					sqlCommand.Parameters.AddWithValue("Footer51" + i, item.Footer51 == null ? (object)DBNull.Value : item.Footer51);
					sqlCommand.Parameters.AddWithValue("Footer52" + i, item.Footer52 == null ? (object)DBNull.Value : item.Footer52);
					sqlCommand.Parameters.AddWithValue("Footer53" + i, item.Footer53 == null ? (object)DBNull.Value : item.Footer53);
					sqlCommand.Parameters.AddWithValue("Footer54" + i, item.Footer54 == null ? (object)DBNull.Value : item.Footer54);
					sqlCommand.Parameters.AddWithValue("Footer55" + i, item.Footer55 == null ? (object)DBNull.Value : item.Footer55);
					sqlCommand.Parameters.AddWithValue("Footer56" + i, item.Footer56 == null ? (object)DBNull.Value : item.Footer56);
					sqlCommand.Parameters.AddWithValue("Footer57" + i, item.Footer57 == null ? (object)DBNull.Value : item.Footer57);
					sqlCommand.Parameters.AddWithValue("Footer61" + i, item.Footer61 == null ? (object)DBNull.Value : item.Footer61);
					sqlCommand.Parameters.AddWithValue("Footer62" + i, item.Footer62 == null ? (object)DBNull.Value : item.Footer62);
					sqlCommand.Parameters.AddWithValue("Footer63" + i, item.Footer63 == null ? (object)DBNull.Value : item.Footer63);
					sqlCommand.Parameters.AddWithValue("Footer64" + i, item.Footer64 == null ? (object)DBNull.Value : item.Footer64);
					sqlCommand.Parameters.AddWithValue("Footer65" + i, item.Footer65 == null ? (object)DBNull.Value : item.Footer65);
					sqlCommand.Parameters.AddWithValue("Footer66" + i, item.Footer66 == null ? (object)DBNull.Value : item.Footer66);
					sqlCommand.Parameters.AddWithValue("Footer67" + i, item.Footer67 == null ? (object)DBNull.Value : item.Footer67);
					sqlCommand.Parameters.AddWithValue("Footer71" + i, item.Footer71 == null ? (object)DBNull.Value : item.Footer71);
					sqlCommand.Parameters.AddWithValue("Footer72" + i, item.Footer72 == null ? (object)DBNull.Value : item.Footer72);
					sqlCommand.Parameters.AddWithValue("Footer73" + i, item.Footer73 == null ? (object)DBNull.Value : item.Footer73);
					sqlCommand.Parameters.AddWithValue("Footer74" + i, item.Footer74 == null ? (object)DBNull.Value : item.Footer74);
					sqlCommand.Parameters.AddWithValue("Footer75" + i, item.Footer75 == null ? (object)DBNull.Value : item.Footer75);
					sqlCommand.Parameters.AddWithValue("Footer76" + i, item.Footer76 == null ? (object)DBNull.Value : item.Footer76);
					sqlCommand.Parameters.AddWithValue("Footer77" + i, item.Footer77 == null ? (object)DBNull.Value : item.Footer77);
					sqlCommand.Parameters.AddWithValue("ForDeliveryNote" + i, item.ForDeliveryNote == null ? (object)DBNull.Value : item.ForDeliveryNote);
					sqlCommand.Parameters.AddWithValue("ForPosDeliveryNote" + i, item.ForPosDeliveryNote == null ? (object)DBNull.Value : item.ForPosDeliveryNote);
					sqlCommand.Parameters.AddWithValue("Geliefert" + i, item.Geliefert == null ? (object)DBNull.Value : item.Geliefert);
					sqlCommand.Parameters.AddWithValue("Header" + i, item.Header == null ? (object)DBNull.Value : item.Header);
					sqlCommand.Parameters.AddWithValue("ImportLogoImageId" + i, item.ImportLogoImageId);
					sqlCommand.Parameters.AddWithValue("Index_Kunde" + i, item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
					sqlCommand.Parameters.AddWithValue("InternalNumber" + i, item.InternalNumber == null ? (object)DBNull.Value : item.InternalNumber);
					sqlCommand.Parameters.AddWithValue("ItemsFooter1" + i, item.ItemsFooter1 == null ? (object)DBNull.Value : item.ItemsFooter1);
					sqlCommand.Parameters.AddWithValue("ItemsFooter2" + i, item.ItemsFooter2 == null ? (object)DBNull.Value : item.ItemsFooter2);
					sqlCommand.Parameters.AddWithValue("ItemsHeader" + i, item.ItemsHeader == null ? (object)DBNull.Value : item.ItemsHeader);
					sqlCommand.Parameters.AddWithValue("LanguageId" + i, item.LanguageId);
					sqlCommand.Parameters.AddWithValue("LastPageText1" + i, item.LastPageText1 == null ? (object)DBNull.Value : item.LastPageText1);
					sqlCommand.Parameters.AddWithValue("LastPageText10" + i, item.LastPageText10 == null ? (object)DBNull.Value : item.LastPageText10);
					sqlCommand.Parameters.AddWithValue("LastPageText11" + i, item.LastPageText11 == null ? (object)DBNull.Value : item.LastPageText11);
					sqlCommand.Parameters.AddWithValue("LastPageText2" + i, item.LastPageText2 == null ? (object)DBNull.Value : item.LastPageText2);
					sqlCommand.Parameters.AddWithValue("LastPageText3" + i, item.LastPageText3 == null ? (object)DBNull.Value : item.LastPageText3);
					sqlCommand.Parameters.AddWithValue("LastPageText4" + i, item.LastPageText4 == null ? (object)DBNull.Value : item.LastPageText4);
					sqlCommand.Parameters.AddWithValue("LastPageText5" + i, item.LastPageText5 == null ? (object)DBNull.Value : item.LastPageText5);
					sqlCommand.Parameters.AddWithValue("LastPageText6" + i, item.LastPageText6 == null ? (object)DBNull.Value : item.LastPageText6);
					sqlCommand.Parameters.AddWithValue("LastPageText7" + i, item.LastPageText7 == null ? (object)DBNull.Value : item.LastPageText7);
					sqlCommand.Parameters.AddWithValue("LastPageText8" + i, item.LastPageText8 == null ? (object)DBNull.Value : item.LastPageText8);
					sqlCommand.Parameters.AddWithValue("LastPageText9" + i, item.LastPageText9 == null ? (object)DBNull.Value : item.LastPageText9);
					sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
					sqlCommand.Parameters.AddWithValue("LastUpdateUserId" + i, item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
					sqlCommand.Parameters.AddWithValue("Lieferadresse" + i, item.Lieferadresse == null ? (object)DBNull.Value : item.Lieferadresse);
					sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("Offen" + i, item.Offen == null ? (object)DBNull.Value : item.Offen);
					sqlCommand.Parameters.AddWithValue("OrderDate" + i, item.OrderDate == null ? (object)DBNull.Value : item.OrderDate);
					sqlCommand.Parameters.AddWithValue("OrderNumber" + i, item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
					sqlCommand.Parameters.AddWithValue("OrderNumberPO" + i, item.OrderNumberPO == null ? (object)DBNull.Value : item.OrderNumberPO);
					sqlCommand.Parameters.AddWithValue("OrderTypeId" + i, item.OrderTypeId);
					sqlCommand.Parameters.AddWithValue("PaymentMethod" + i, item.PaymentMethod == null ? (object)DBNull.Value : item.PaymentMethod);
					sqlCommand.Parameters.AddWithValue("PaymentTarget" + i, item.PaymentTarget == null ? (object)DBNull.Value : item.PaymentTarget);
					sqlCommand.Parameters.AddWithValue("PE" + i, item.PE == null ? (object)DBNull.Value : item.PE);
					sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
					sqlCommand.Parameters.AddWithValue("ShippingMethod" + i, item.ShippingMethod == null ? (object)DBNull.Value : item.ShippingMethod);
					sqlCommand.Parameters.AddWithValue("SummarySum" + i, item.SummarySum == null ? (object)DBNull.Value : item.SummarySum);
					sqlCommand.Parameters.AddWithValue("SummaryTotal" + i, item.SummaryTotal == null ? (object)DBNull.Value : item.SummaryTotal);
					sqlCommand.Parameters.AddWithValue("SummaryUST" + i, item.SummaryUST == null ? (object)DBNull.Value : item.SummaryUST);
					sqlCommand.Parameters.AddWithValue("TotalPrice150" + i, item.TotalPrice150 == null ? (object)DBNull.Value : item.TotalPrice150);
					sqlCommand.Parameters.AddWithValue("Unit" + i, item.Unit == null ? (object)DBNull.Value : item.Unit);
					sqlCommand.Parameters.AddWithValue("UnitPrice" + i, item.UnitPrice == null ? (object)DBNull.Value : item.UnitPrice);
					sqlCommand.Parameters.AddWithValue("UnitTotal" + i, item.UnitTotal == null ? (object)DBNull.Value : item.UnitTotal);
					sqlCommand.Parameters.AddWithValue("UST_ID" + i, item.UST_ID == null ? (object)DBNull.Value : item.UST_ID);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__PRS_OrderReport] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [__PRS_OrderReport] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods
		#region Custom Methods
		public static Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity GetByLanguageAndType(int languageId, int typeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "";

				query = "SELECT * FROM [__PRS_OrderReport] WHERE [LanguageId]=@languageId AND [OrderTypeId]=@typeId";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("languageId", languageId);
				sqlCommand.Parameters.AddWithValue("typeId", typeId);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity GetByLanguageAndType(int languageId, int typeId, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			string query = "";
			if(typeId == 5)
			{
				query = "SELECT * FROM [__PRS_DelieveryNoteReport] WHERE [LanguageId]=@languageId AND [OrderTypeId]=@typeId";
			}
			else
			{
				query = "SELECT * FROM [__PRS_OrderReport] WHERE [LanguageId]=@languageId AND [OrderTypeId]=@typeId";
			}

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("languageId", languageId);
			sqlCommand.Parameters.AddWithValue("typeId", typeId);

			DbExecution.Fill(sqlCommand, dataTable);


			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static int UpdateComapnyLogo(int Id, int logoImageId, int updateUserId, DateTime updateTime)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__PRS_OrderReport] SET [CompanyLogoImageId]=@CompanyLogoImageId,[LastUpdateUserId]=@LastUpdateUserId,[LastUpdateTime]=@LastUpdateTime WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", Id);
				sqlCommand.Parameters.AddWithValue("CompanyLogoImageId", logoImageId);
				sqlCommand.Parameters.AddWithValue("LastUpdateUserId", updateUserId);
				sqlCommand.Parameters.AddWithValue("LastUpdateTime", updateTime);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int UpdateImportLogo(int Id, int logoImageId, int updateUserId, DateTime updateTime)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__PRS_OrderReport] SET [ImportLogoImageId]=@ImportLogoImageId,[LastUpdateUserId]=@LastUpdateUserId,[LastUpdateTime]=@LastUpdateTime WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", Id);
				sqlCommand.Parameters.AddWithValue("ImportLogoImageId", logoImageId);
				sqlCommand.Parameters.AddWithValue("LastUpdateUserId", updateUserId);
				sqlCommand.Parameters.AddWithValue("LastUpdateTime", updateTime);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		#endregion Custom Methods
	}
}