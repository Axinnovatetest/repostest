using System;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.Logistics
{
	public class OrderReportAccess
	{
		public static Infrastructure.Data.Entities.Tables.Logistics.OrderReportEntity GetByLanguageAndType(int languageId, int typeId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "";
				if(typeId == 5)
				{
					query = "SELECT * FROM [__PRS_DelieveryNoteReport] WHERE [LanguageId]=@languageId AND [OrderTypeId]=@typeId";
				}
				else
				{
					query = "SELECT * FROM [__PRS_OrderReport] WHERE [LanguageId]=@languageId AND [OrderTypeId]=@typeId";
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("languageId", languageId);
				sqlCommand.Parameters.AddWithValue("typeId", typeId);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.OrderReportEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static int Insert(Infrastructure.Data.Entities.Tables.Logistics.OrderReportEntity item)
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
	}
}
