using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.PRS
{
	public class DelieveryNoteReportAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.DelieveryNoteReportEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__PRS_DelieveryNoteReport] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.DelieveryNoteReportEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.DelieveryNoteReportEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__PRS_DelieveryNoteReport]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.DelieveryNoteReportEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.DelieveryNoteReportEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.DelieveryNoteReportEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.DelieveryNoteReportEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.DelieveryNoteReportEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.DelieveryNoteReportEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.DelieveryNoteReportEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__PRS_DelieveryNoteReport] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.DelieveryNoteReportEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.DelieveryNoteReportEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.DelieveryNoteReportEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.DelieveryNoteReportEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__PRS_DelieveryNoteReport] ([Abladestelle],[Address1],[Address2],[Address3],[Address4],[Article],[ArtikelCountry],[ArtikelPrice],[ArtikelQuantity],[ArtikelStock],[ArtikelWeight],[ClientNumber],[CompanyLogoImageId],[Cu_Surcharge],[CustomerDate],[CustomerNumber],[Description],[Designation1],[Designation2],[DocumentType],[Footer11],[Footer12],[Footer13],[Footer14],[Footer15],[Footer16],[Footer17],[Footer21],[Footer22],[Footer23],[Footer24],[Footer25],[Footer26],[Footer27],[Footer31],[Footer32],[Footer33],[Footer34],[Footer35],[Footer36],[Footer37],[Footer41],[Footer42],[Footer43],[Footer44],[Footer45],[Footer46],[Footer47],[Footer51],[Footer52],[Footer53],[Footer54],[Footer55],[Footer56],[Footer57],[Footer61],[Footer62],[Footer63],[Footer64],[Footer65],[Footer66],[Footer67],[Footer71],[Footer72],[Footer73],[Footer74],[Footer75],[Footer76],[Footer77],[Header],[ImportLogoImageId],[ItemsFooter1],[ItemsFooter2],[ItemsHeader],[LanguageId],[LastPageText1],[LastPageText2],[LastPageText3],[LastPageText4],[LastPageText5],[LastPageText6],[LastPageText7],[LastPageText8],[LastPageText9],[LastUpdateTime],[LastUpdateUserId],[Lieferadresse],[OrderDate],[OrderNumber],[OrderNumberPO],[OrderTypeId],[Position],[ShippingMethod],[SummarySum],[SummaryTotal],[SummaryUST])  VALUES (@Abladestelle,@Address1,@Address2,@Address3,@Address4,@Article,@ArtikelCountry,@ArtikelPrice,@ArtikelQuantity,@ArtikelStock,@ArtikelWeight,@ClientNumber,@CompanyLogoImageId,@Cu_Surcharge,@CustomerDate,@CustomerNumber,@Description,@Designation1,@Designation2,@DocumentType,@Footer11,@Footer12,@Footer13,@Footer14,@Footer15,@Footer16,@Footer17,@Footer21,@Footer22,@Footer23,@Footer24,@Footer25,@Footer26,@Footer27,@Footer31,@Footer32,@Footer33,@Footer34,@Footer35,@Footer36,@Footer37,@Footer41,@Footer42,@Footer43,@Footer44,@Footer45,@Footer46,@Footer47,@Footer51,@Footer52,@Footer53,@Footer54,@Footer55,@Footer56,@Footer57,@Footer61,@Footer62,@Footer63,@Footer64,@Footer65,@Footer66,@Footer67,@Footer71,@Footer72,@Footer73,@Footer74,@Footer75,@Footer76,@Footer77,@Header,@ImportLogoImageId,@ItemsFooter1,@ItemsFooter2,@ItemsHeader,@LanguageId,@LastPageText1,@LastPageText2,@LastPageText3,@LastPageText4,@LastPageText5,@LastPageText6,@LastPageText7,@LastPageText8,@LastPageText9,@LastUpdateTime,@LastUpdateUserId,@Lieferadresse,@OrderDate,@OrderNumber,@OrderNumberPO,@OrderTypeId,@Position,@ShippingMethod,@SummarySum,@SummaryTotal,@SummaryUST); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Abladestelle", item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
					sqlCommand.Parameters.AddWithValue("Address1", item.Address1 == null ? (object)DBNull.Value : item.Address1);
					sqlCommand.Parameters.AddWithValue("Address2", item.Address2 == null ? (object)DBNull.Value : item.Address2);
					sqlCommand.Parameters.AddWithValue("Address3", item.Address3 == null ? (object)DBNull.Value : item.Address3);
					sqlCommand.Parameters.AddWithValue("Address4", item.Address4 == null ? (object)DBNull.Value : item.Address4);
					sqlCommand.Parameters.AddWithValue("Article", item.Article == null ? (object)DBNull.Value : item.Article);
					sqlCommand.Parameters.AddWithValue("ArtikelCountry", item.ArtikelCountry == null ? (object)DBNull.Value : item.ArtikelCountry);
					sqlCommand.Parameters.AddWithValue("ArtikelPrice", item.ArtikelPrice == null ? (object)DBNull.Value : item.ArtikelPrice);
					sqlCommand.Parameters.AddWithValue("ArtikelQuantity", item.ArtikelQuantity == null ? (object)DBNull.Value : item.ArtikelQuantity);
					sqlCommand.Parameters.AddWithValue("ArtikelStock", item.ArtikelStock == null ? (object)DBNull.Value : item.ArtikelStock);
					sqlCommand.Parameters.AddWithValue("ArtikelWeight", item.ArtikelWeight == null ? (object)DBNull.Value : item.ArtikelWeight);
					sqlCommand.Parameters.AddWithValue("ClientNumber", item.ClientNumber == null ? (object)DBNull.Value : item.ClientNumber);
					sqlCommand.Parameters.AddWithValue("CompanyLogoImageId", item.CompanyLogoImageId);
					sqlCommand.Parameters.AddWithValue("Cu_Surcharge", item.Cu_Surcharge == null ? (object)DBNull.Value : item.Cu_Surcharge);
					sqlCommand.Parameters.AddWithValue("CustomerDate", item.CustomerDate == null ? (object)DBNull.Value : item.CustomerDate);
					sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
					sqlCommand.Parameters.AddWithValue("Designation1", item.Designation1 == null ? (object)DBNull.Value : item.Designation1);
					sqlCommand.Parameters.AddWithValue("Designation2", item.Designation2 == null ? (object)DBNull.Value : item.Designation2);
					sqlCommand.Parameters.AddWithValue("DocumentType", item.DocumentType == null ? (object)DBNull.Value : item.DocumentType);
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
					sqlCommand.Parameters.AddWithValue("Header", item.Header == null ? (object)DBNull.Value : item.Header);
					sqlCommand.Parameters.AddWithValue("ImportLogoImageId", item.ImportLogoImageId);
					sqlCommand.Parameters.AddWithValue("ItemsFooter1", item.ItemsFooter1 == null ? (object)DBNull.Value : item.ItemsFooter1);
					sqlCommand.Parameters.AddWithValue("ItemsFooter2", item.ItemsFooter2 == null ? (object)DBNull.Value : item.ItemsFooter2);
					sqlCommand.Parameters.AddWithValue("ItemsHeader", item.ItemsHeader == null ? (object)DBNull.Value : item.ItemsHeader);
					sqlCommand.Parameters.AddWithValue("LanguageId", item.LanguageId);
					sqlCommand.Parameters.AddWithValue("LastPageText1", item.LastPageText1 == null ? (object)DBNull.Value : item.LastPageText1);
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
					sqlCommand.Parameters.AddWithValue("OrderDate", item.OrderDate == null ? (object)DBNull.Value : item.OrderDate);
					sqlCommand.Parameters.AddWithValue("OrderNumber", item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
					sqlCommand.Parameters.AddWithValue("OrderNumberPO", item.OrderNumberPO == null ? (object)DBNull.Value : item.OrderNumberPO);
					sqlCommand.Parameters.AddWithValue("OrderTypeId", item.OrderTypeId);
					sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
					sqlCommand.Parameters.AddWithValue("ShippingMethod", item.ShippingMethod == null ? (object)DBNull.Value : item.ShippingMethod);
					sqlCommand.Parameters.AddWithValue("SummarySum", item.SummarySum == null ? (object)DBNull.Value : item.SummarySum);
					sqlCommand.Parameters.AddWithValue("SummaryTotal", item.SummaryTotal == null ? (object)DBNull.Value : item.SummaryTotal);
					sqlCommand.Parameters.AddWithValue("SummaryUST", item.SummaryUST == null ? (object)DBNull.Value : item.SummaryUST);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.PRS.DelieveryNoteReportEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 97; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.PRS.DelieveryNoteReportEntity> items)
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
						query += " INSERT INTO [__PRS_DelieveryNoteReport] ([Abladestelle],[Address1],[Address2],[Address3],[Address4],[Article],[ArtikelCountry],[ArtikelPrice],[ArtikelQuantity],[ArtikelStock],[ArtikelWeight],[ClientNumber],[CompanyLogoImageId],[Cu_Surcharge],[CustomerDate],[CustomerNumber],[Description],[Designation1],[Designation2],[DocumentType],[Footer11],[Footer12],[Footer13],[Footer14],[Footer15],[Footer16],[Footer17],[Footer21],[Footer22],[Footer23],[Footer24],[Footer25],[Footer26],[Footer27],[Footer31],[Footer32],[Footer33],[Footer34],[Footer35],[Footer36],[Footer37],[Footer41],[Footer42],[Footer43],[Footer44],[Footer45],[Footer46],[Footer47],[Footer51],[Footer52],[Footer53],[Footer54],[Footer55],[Footer56],[Footer57],[Footer61],[Footer62],[Footer63],[Footer64],[Footer65],[Footer66],[Footer67],[Footer71],[Footer72],[Footer73],[Footer74],[Footer75],[Footer76],[Footer77],[Header],[ImportLogoImageId],[ItemsFooter1],[ItemsFooter2],[ItemsHeader],[LanguageId],[LastPageText1],[LastPageText2],[LastPageText3],[LastPageText4],[LastPageText5],[LastPageText6],[LastPageText7],[LastPageText8],[LastPageText9],[LastUpdateTime],[LastUpdateUserId],[Lieferadresse],[OrderDate],[OrderNumber],[OrderNumberPO],[OrderTypeId],[Position],[ShippingMethod],[SummarySum],[SummaryTotal],[SummaryUST]) VALUES ( "

							+ "@Abladestelle" + i + ","
							+ "@Address1" + i + ","
							+ "@Address2" + i + ","
							+ "@Address3" + i + ","
							+ "@Address4" + i + ","
							+ "@Article" + i + ","
							+ "@ArtikelCountry" + i + ","
							+ "@ArtikelPrice" + i + ","
							+ "@ArtikelQuantity" + i + ","
							+ "@ArtikelStock" + i + ","
							+ "@ArtikelWeight" + i + ","
							+ "@ClientNumber" + i + ","
							+ "@CompanyLogoImageId" + i + ","
							+ "@Cu_Surcharge" + i + ","
							+ "@CustomerDate" + i + ","
							+ "@CustomerNumber" + i + ","
							+ "@Description" + i + ","
							+ "@Designation1" + i + ","
							+ "@Designation2" + i + ","
							+ "@DocumentType" + i + ","
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
							+ "@Header" + i + ","
							+ "@ImportLogoImageId" + i + ","
							+ "@ItemsFooter1" + i + ","
							+ "@ItemsFooter2" + i + ","
							+ "@ItemsHeader" + i + ","
							+ "@LanguageId" + i + ","
							+ "@LastPageText1" + i + ","
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
							+ "@OrderDate" + i + ","
							+ "@OrderNumber" + i + ","
							+ "@OrderNumberPO" + i + ","
							+ "@OrderTypeId" + i + ","
							+ "@Position" + i + ","
							+ "@ShippingMethod" + i + ","
							+ "@SummarySum" + i + ","
							+ "@SummaryTotal" + i + ","
							+ "@SummaryUST" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Abladestelle" + i, item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
						sqlCommand.Parameters.AddWithValue("Address1" + i, item.Address1 == null ? (object)DBNull.Value : item.Address1);
						sqlCommand.Parameters.AddWithValue("Address2" + i, item.Address2 == null ? (object)DBNull.Value : item.Address2);
						sqlCommand.Parameters.AddWithValue("Address3" + i, item.Address3 == null ? (object)DBNull.Value : item.Address3);
						sqlCommand.Parameters.AddWithValue("Address4" + i, item.Address4 == null ? (object)DBNull.Value : item.Address4);
						sqlCommand.Parameters.AddWithValue("Article" + i, item.Article == null ? (object)DBNull.Value : item.Article);
						sqlCommand.Parameters.AddWithValue("ArtikelCountry" + i, item.ArtikelCountry == null ? (object)DBNull.Value : item.ArtikelCountry);
						sqlCommand.Parameters.AddWithValue("ArtikelPrice" + i, item.ArtikelPrice == null ? (object)DBNull.Value : item.ArtikelPrice);
						sqlCommand.Parameters.AddWithValue("ArtikelQuantity" + i, item.ArtikelQuantity == null ? (object)DBNull.Value : item.ArtikelQuantity);
						sqlCommand.Parameters.AddWithValue("ArtikelStock" + i, item.ArtikelStock == null ? (object)DBNull.Value : item.ArtikelStock);
						sqlCommand.Parameters.AddWithValue("ArtikelWeight" + i, item.ArtikelWeight == null ? (object)DBNull.Value : item.ArtikelWeight);
						sqlCommand.Parameters.AddWithValue("ClientNumber" + i, item.ClientNumber == null ? (object)DBNull.Value : item.ClientNumber);
						sqlCommand.Parameters.AddWithValue("CompanyLogoImageId" + i, item.CompanyLogoImageId);
						sqlCommand.Parameters.AddWithValue("Cu_Surcharge" + i, item.Cu_Surcharge == null ? (object)DBNull.Value : item.Cu_Surcharge);
						sqlCommand.Parameters.AddWithValue("CustomerDate" + i, item.CustomerDate == null ? (object)DBNull.Value : item.CustomerDate);
						sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("Designation1" + i, item.Designation1 == null ? (object)DBNull.Value : item.Designation1);
						sqlCommand.Parameters.AddWithValue("Designation2" + i, item.Designation2 == null ? (object)DBNull.Value : item.Designation2);
						sqlCommand.Parameters.AddWithValue("DocumentType" + i, item.DocumentType == null ? (object)DBNull.Value : item.DocumentType);
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
						sqlCommand.Parameters.AddWithValue("Header" + i, item.Header == null ? (object)DBNull.Value : item.Header);
						sqlCommand.Parameters.AddWithValue("ImportLogoImageId" + i, item.ImportLogoImageId);
						sqlCommand.Parameters.AddWithValue("ItemsFooter1" + i, item.ItemsFooter1 == null ? (object)DBNull.Value : item.ItemsFooter1);
						sqlCommand.Parameters.AddWithValue("ItemsFooter2" + i, item.ItemsFooter2 == null ? (object)DBNull.Value : item.ItemsFooter2);
						sqlCommand.Parameters.AddWithValue("ItemsHeader" + i, item.ItemsHeader == null ? (object)DBNull.Value : item.ItemsHeader);
						sqlCommand.Parameters.AddWithValue("LanguageId" + i, item.LanguageId);
						sqlCommand.Parameters.AddWithValue("LastPageText1" + i, item.LastPageText1 == null ? (object)DBNull.Value : item.LastPageText1);
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
						sqlCommand.Parameters.AddWithValue("OrderDate" + i, item.OrderDate == null ? (object)DBNull.Value : item.OrderDate);
						sqlCommand.Parameters.AddWithValue("OrderNumber" + i, item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
						sqlCommand.Parameters.AddWithValue("OrderNumberPO" + i, item.OrderNumberPO == null ? (object)DBNull.Value : item.OrderNumberPO);
						sqlCommand.Parameters.AddWithValue("OrderTypeId" + i, item.OrderTypeId);
						sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
						sqlCommand.Parameters.AddWithValue("ShippingMethod" + i, item.ShippingMethod == null ? (object)DBNull.Value : item.ShippingMethod);
						sqlCommand.Parameters.AddWithValue("SummarySum" + i, item.SummarySum == null ? (object)DBNull.Value : item.SummarySum);
						sqlCommand.Parameters.AddWithValue("SummaryTotal" + i, item.SummaryTotal == null ? (object)DBNull.Value : item.SummaryTotal);
						sqlCommand.Parameters.AddWithValue("SummaryUST" + i, item.SummaryUST == null ? (object)DBNull.Value : item.SummaryUST);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.PRS.DelieveryNoteReportEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__PRS_DelieveryNoteReport] SET [Abladestelle]=@Abladestelle, [Address1]=@Address1, [Address2]=@Address2, [Address3]=@Address3, [Address4]=@Address4, [Article]=@Article, [ArtikelCountry]=@ArtikelCountry, [ArtikelPrice]=@ArtikelPrice, [ArtikelQuantity]=@ArtikelQuantity, [ArtikelStock]=@ArtikelStock, [ArtikelWeight]=@ArtikelWeight, [ClientNumber]=@ClientNumber, [CompanyLogoImageId]=@CompanyLogoImageId, [Cu_Surcharge]=@Cu_Surcharge, [CustomerDate]=@CustomerDate, [CustomerNumber]=@CustomerNumber, [Description]=@Description, [Designation1]=@Designation1, [Designation2]=@Designation2, [DocumentType]=@DocumentType, [Footer11]=@Footer11, [Footer12]=@Footer12, [Footer13]=@Footer13, [Footer14]=@Footer14, [Footer15]=@Footer15, [Footer16]=@Footer16, [Footer17]=@Footer17, [Footer21]=@Footer21, [Footer22]=@Footer22, [Footer23]=@Footer23, [Footer24]=@Footer24, [Footer25]=@Footer25, [Footer26]=@Footer26, [Footer27]=@Footer27, [Footer31]=@Footer31, [Footer32]=@Footer32, [Footer33]=@Footer33, [Footer34]=@Footer34, [Footer35]=@Footer35, [Footer36]=@Footer36, [Footer37]=@Footer37, [Footer41]=@Footer41, [Footer42]=@Footer42, [Footer43]=@Footer43, [Footer44]=@Footer44, [Footer45]=@Footer45, [Footer46]=@Footer46, [Footer47]=@Footer47, [Footer51]=@Footer51, [Footer52]=@Footer52, [Footer53]=@Footer53, [Footer54]=@Footer54, [Footer55]=@Footer55, [Footer56]=@Footer56, [Footer57]=@Footer57, [Footer61]=@Footer61, [Footer62]=@Footer62, [Footer63]=@Footer63, [Footer64]=@Footer64, [Footer65]=@Footer65, [Footer66]=@Footer66, [Footer67]=@Footer67, [Footer71]=@Footer71, [Footer72]=@Footer72, [Footer73]=@Footer73, [Footer74]=@Footer74, [Footer75]=@Footer75, [Footer76]=@Footer76, [Footer77]=@Footer77, [Header]=@Header, [ImportLogoImageId]=@ImportLogoImageId, [ItemsFooter1]=@ItemsFooter1, [ItemsFooter2]=@ItemsFooter2, [ItemsHeader]=@ItemsHeader, [LanguageId]=@LanguageId, [LastPageText1]=@LastPageText1, [LastPageText2]=@LastPageText2, [LastPageText3]=@LastPageText3, [LastPageText4]=@LastPageText4, [LastPageText5]=@LastPageText5, [LastPageText6]=@LastPageText6, [LastPageText7]=@LastPageText7, [LastPageText8]=@LastPageText8, [LastPageText9]=@LastPageText9, [LastUpdateTime]=@LastUpdateTime, [LastUpdateUserId]=@LastUpdateUserId, [Lieferadresse]=@Lieferadresse, [OrderDate]=@OrderDate, [OrderNumber]=@OrderNumber, [OrderNumberPO]=@OrderNumberPO, [OrderTypeId]=@OrderTypeId, [Position]=@Position, [ShippingMethod]=@ShippingMethod, [SummarySum]=@SummarySum, [SummaryTotal]=@SummaryTotal, [SummaryUST]=@SummaryUST WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Abladestelle", item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
				sqlCommand.Parameters.AddWithValue("Address1", item.Address1 == null ? (object)DBNull.Value : item.Address1);
				sqlCommand.Parameters.AddWithValue("Address2", item.Address2 == null ? (object)DBNull.Value : item.Address2);
				sqlCommand.Parameters.AddWithValue("Address3", item.Address3 == null ? (object)DBNull.Value : item.Address3);
				sqlCommand.Parameters.AddWithValue("Address4", item.Address4 == null ? (object)DBNull.Value : item.Address4);
				sqlCommand.Parameters.AddWithValue("Article", item.Article == null ? (object)DBNull.Value : item.Article);
				sqlCommand.Parameters.AddWithValue("ArtikelCountry", item.ArtikelCountry == null ? (object)DBNull.Value : item.ArtikelCountry);
				sqlCommand.Parameters.AddWithValue("ArtikelPrice", item.ArtikelPrice == null ? (object)DBNull.Value : item.ArtikelPrice);
				sqlCommand.Parameters.AddWithValue("ArtikelQuantity", item.ArtikelQuantity == null ? (object)DBNull.Value : item.ArtikelQuantity);
				sqlCommand.Parameters.AddWithValue("ArtikelStock", item.ArtikelStock == null ? (object)DBNull.Value : item.ArtikelStock);
				sqlCommand.Parameters.AddWithValue("ArtikelWeight", item.ArtikelWeight == null ? (object)DBNull.Value : item.ArtikelWeight);
				sqlCommand.Parameters.AddWithValue("ClientNumber", item.ClientNumber == null ? (object)DBNull.Value : item.ClientNumber);
				sqlCommand.Parameters.AddWithValue("CompanyLogoImageId", item.CompanyLogoImageId);
				sqlCommand.Parameters.AddWithValue("Cu_Surcharge", item.Cu_Surcharge == null ? (object)DBNull.Value : item.Cu_Surcharge);
				sqlCommand.Parameters.AddWithValue("CustomerDate", item.CustomerDate == null ? (object)DBNull.Value : item.CustomerDate);
				sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
				sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
				sqlCommand.Parameters.AddWithValue("Designation1", item.Designation1 == null ? (object)DBNull.Value : item.Designation1);
				sqlCommand.Parameters.AddWithValue("Designation2", item.Designation2 == null ? (object)DBNull.Value : item.Designation2);
				sqlCommand.Parameters.AddWithValue("DocumentType", item.DocumentType == null ? (object)DBNull.Value : item.DocumentType);
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
				sqlCommand.Parameters.AddWithValue("Header", item.Header == null ? (object)DBNull.Value : item.Header);
				sqlCommand.Parameters.AddWithValue("ImportLogoImageId", item.ImportLogoImageId);
				sqlCommand.Parameters.AddWithValue("ItemsFooter1", item.ItemsFooter1 == null ? (object)DBNull.Value : item.ItemsFooter1);
				sqlCommand.Parameters.AddWithValue("ItemsFooter2", item.ItemsFooter2 == null ? (object)DBNull.Value : item.ItemsFooter2);
				sqlCommand.Parameters.AddWithValue("ItemsHeader", item.ItemsHeader == null ? (object)DBNull.Value : item.ItemsHeader);
				sqlCommand.Parameters.AddWithValue("LanguageId", item.LanguageId);
				sqlCommand.Parameters.AddWithValue("LastPageText1", item.LastPageText1 == null ? (object)DBNull.Value : item.LastPageText1);
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
				sqlCommand.Parameters.AddWithValue("OrderDate", item.OrderDate == null ? (object)DBNull.Value : item.OrderDate);
				sqlCommand.Parameters.AddWithValue("OrderNumber", item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
				sqlCommand.Parameters.AddWithValue("OrderNumberPO", item.OrderNumberPO == null ? (object)DBNull.Value : item.OrderNumberPO);
				sqlCommand.Parameters.AddWithValue("OrderTypeId", item.OrderTypeId);
				sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
				sqlCommand.Parameters.AddWithValue("ShippingMethod", item.ShippingMethod == null ? (object)DBNull.Value : item.ShippingMethod);
				sqlCommand.Parameters.AddWithValue("SummarySum", item.SummarySum == null ? (object)DBNull.Value : item.SummarySum);
				sqlCommand.Parameters.AddWithValue("SummaryTotal", item.SummaryTotal == null ? (object)DBNull.Value : item.SummaryTotal);
				sqlCommand.Parameters.AddWithValue("SummaryUST", item.SummaryUST == null ? (object)DBNull.Value : item.SummaryUST);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRS.DelieveryNoteReportEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 97; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.PRS.DelieveryNoteReportEntity> items)
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
						query += " UPDATE [__PRS_DelieveryNoteReport] SET "

							+ "[Abladestelle]=@Abladestelle" + i + ","
							+ "[Address1]=@Address1" + i + ","
							+ "[Address2]=@Address2" + i + ","
							+ "[Address3]=@Address3" + i + ","
							+ "[Address4]=@Address4" + i + ","
							+ "[Article]=@Article" + i + ","
							+ "[ArtikelCountry]=@ArtikelCountry" + i + ","
							+ "[ArtikelPrice]=@ArtikelPrice" + i + ","
							+ "[ArtikelQuantity]=@ArtikelQuantity" + i + ","
							+ "[ArtikelStock]=@ArtikelStock" + i + ","
							+ "[ArtikelWeight]=@ArtikelWeight" + i + ","
							+ "[ClientNumber]=@ClientNumber" + i + ","
							+ "[CompanyLogoImageId]=@CompanyLogoImageId" + i + ","
							+ "[Cu_Surcharge]=@Cu_Surcharge" + i + ","
							+ "[CustomerDate]=@CustomerDate" + i + ","
							+ "[CustomerNumber]=@CustomerNumber" + i + ","
							+ "[Description]=@Description" + i + ","
							+ "[Designation1]=@Designation1" + i + ","
							+ "[Designation2]=@Designation2" + i + ","
							+ "[DocumentType]=@DocumentType" + i + ","
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
							+ "[Header]=@Header" + i + ","
							+ "[ImportLogoImageId]=@ImportLogoImageId" + i + ","
							+ "[ItemsFooter1]=@ItemsFooter1" + i + ","
							+ "[ItemsFooter2]=@ItemsFooter2" + i + ","
							+ "[ItemsHeader]=@ItemsHeader" + i + ","
							+ "[LanguageId]=@LanguageId" + i + ","
							+ "[LastPageText1]=@LastPageText1" + i + ","
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
							+ "[OrderDate]=@OrderDate" + i + ","
							+ "[OrderNumber]=@OrderNumber" + i + ","
							+ "[OrderNumberPO]=@OrderNumberPO" + i + ","
							+ "[OrderTypeId]=@OrderTypeId" + i + ","
							+ "[Position]=@Position" + i + ","
							+ "[ShippingMethod]=@ShippingMethod" + i + ","
							+ "[SummarySum]=@SummarySum" + i + ","
							+ "[SummaryTotal]=@SummaryTotal" + i + ","
							+ "[SummaryUST]=@SummaryUST" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("Abladestelle" + i, item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
						sqlCommand.Parameters.AddWithValue("Address1" + i, item.Address1 == null ? (object)DBNull.Value : item.Address1);
						sqlCommand.Parameters.AddWithValue("Address2" + i, item.Address2 == null ? (object)DBNull.Value : item.Address2);
						sqlCommand.Parameters.AddWithValue("Address3" + i, item.Address3 == null ? (object)DBNull.Value : item.Address3);
						sqlCommand.Parameters.AddWithValue("Address4" + i, item.Address4 == null ? (object)DBNull.Value : item.Address4);
						sqlCommand.Parameters.AddWithValue("Article" + i, item.Article == null ? (object)DBNull.Value : item.Article);
						sqlCommand.Parameters.AddWithValue("ArtikelCountry" + i, item.ArtikelCountry == null ? (object)DBNull.Value : item.ArtikelCountry);
						sqlCommand.Parameters.AddWithValue("ArtikelPrice" + i, item.ArtikelPrice == null ? (object)DBNull.Value : item.ArtikelPrice);
						sqlCommand.Parameters.AddWithValue("ArtikelQuantity" + i, item.ArtikelQuantity == null ? (object)DBNull.Value : item.ArtikelQuantity);
						sqlCommand.Parameters.AddWithValue("ArtikelStock" + i, item.ArtikelStock == null ? (object)DBNull.Value : item.ArtikelStock);
						sqlCommand.Parameters.AddWithValue("ArtikelWeight" + i, item.ArtikelWeight == null ? (object)DBNull.Value : item.ArtikelWeight);
						sqlCommand.Parameters.AddWithValue("ClientNumber" + i, item.ClientNumber == null ? (object)DBNull.Value : item.ClientNumber);
						sqlCommand.Parameters.AddWithValue("CompanyLogoImageId" + i, item.CompanyLogoImageId);
						sqlCommand.Parameters.AddWithValue("Cu_Surcharge" + i, item.Cu_Surcharge == null ? (object)DBNull.Value : item.Cu_Surcharge);
						sqlCommand.Parameters.AddWithValue("CustomerDate" + i, item.CustomerDate == null ? (object)DBNull.Value : item.CustomerDate);
						sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("Designation1" + i, item.Designation1 == null ? (object)DBNull.Value : item.Designation1);
						sqlCommand.Parameters.AddWithValue("Designation2" + i, item.Designation2 == null ? (object)DBNull.Value : item.Designation2);
						sqlCommand.Parameters.AddWithValue("DocumentType" + i, item.DocumentType == null ? (object)DBNull.Value : item.DocumentType);
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
						sqlCommand.Parameters.AddWithValue("Header" + i, item.Header == null ? (object)DBNull.Value : item.Header);
						sqlCommand.Parameters.AddWithValue("ImportLogoImageId" + i, item.ImportLogoImageId);
						sqlCommand.Parameters.AddWithValue("ItemsFooter1" + i, item.ItemsFooter1 == null ? (object)DBNull.Value : item.ItemsFooter1);
						sqlCommand.Parameters.AddWithValue("ItemsFooter2" + i, item.ItemsFooter2 == null ? (object)DBNull.Value : item.ItemsFooter2);
						sqlCommand.Parameters.AddWithValue("ItemsHeader" + i, item.ItemsHeader == null ? (object)DBNull.Value : item.ItemsHeader);
						sqlCommand.Parameters.AddWithValue("LanguageId" + i, item.LanguageId);
						sqlCommand.Parameters.AddWithValue("LastPageText1" + i, item.LastPageText1 == null ? (object)DBNull.Value : item.LastPageText1);
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
						sqlCommand.Parameters.AddWithValue("OrderDate" + i, item.OrderDate == null ? (object)DBNull.Value : item.OrderDate);
						sqlCommand.Parameters.AddWithValue("OrderNumber" + i, item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
						sqlCommand.Parameters.AddWithValue("OrderNumberPO" + i, item.OrderNumberPO == null ? (object)DBNull.Value : item.OrderNumberPO);
						sqlCommand.Parameters.AddWithValue("OrderTypeId" + i, item.OrderTypeId);
						sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
						sqlCommand.Parameters.AddWithValue("ShippingMethod" + i, item.ShippingMethod == null ? (object)DBNull.Value : item.ShippingMethod);
						sqlCommand.Parameters.AddWithValue("SummarySum" + i, item.SummarySum == null ? (object)DBNull.Value : item.SummarySum);
						sqlCommand.Parameters.AddWithValue("SummaryTotal" + i, item.SummaryTotal == null ? (object)DBNull.Value : item.SummaryTotal);
						sqlCommand.Parameters.AddWithValue("SummaryUST" + i, item.SummaryUST == null ? (object)DBNull.Value : item.SummaryUST);
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
				string query = "DELETE FROM [__PRS_DelieveryNoteReport] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__PRS_DelieveryNoteReport] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		public static Infrastructure.Data.Entities.Tables.PRS.DelieveryNoteReportEntity GetByLanguageAndType(int languageId, int typeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__PRS_DelieveryNoteReport] WHERE [LanguageId]=@languageId AND [OrderTypeId]=@typeId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("languageId", languageId);
				sqlCommand.Parameters.AddWithValue("typeId", typeId);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.DelieveryNoteReportEntity(dataTable.Rows[0]);
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
				string query = "UPDATE [__PRS_DelieveryNoteReport] SET [CompanyLogoImageId]=@CompanyLogoImageId,[LastUpdateUserId]=@LastUpdateUserId,[LastUpdateTime]=@LastUpdateTime WHERE [Id]=@Id";
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
				string query = "UPDATE [__PRS_DelieveryNoteReport] SET [ImportLogoImageId]=@ImportLogoImageId,[LastUpdateUserId]=@LastUpdateUserId,[LastUpdateTime]=@LastUpdateTime WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", Id);
				sqlCommand.Parameters.AddWithValue("ImportLogoImageId", logoImageId);
				sqlCommand.Parameters.AddWithValue("LastUpdateUserId", updateUserId);
				sqlCommand.Parameters.AddWithValue("LastUpdateTime", updateTime);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.PRS.DelieveryNoteReportEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.PRS.DelieveryNoteReportEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.PRS.DelieveryNoteReportEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
