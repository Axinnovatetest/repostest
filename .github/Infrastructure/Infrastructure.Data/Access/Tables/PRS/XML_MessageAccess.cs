using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.PRS
{

	public class XML_MessageAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.XML_MessageEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__EDI_XML_Message] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.XML_MessageEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.XML_MessageEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__EDI_XML_Message]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.XML_MessageEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.XML_MessageEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.XML_MessageEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.XML_MessageEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.XML_MessageEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.XML_MessageEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.XML_MessageEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__EDI_XML_Message] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.XML_MessageEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.XML_MessageEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.XML_MessageEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.XML_MessageEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__EDI_XML_Message] ([EditTime],[IdFile],[IdOrder],[Message_Document_Footer_InvoiceFooter_InvoiceTotals_CurrencyQualifier],[Message_Document_Footer_InvoiceFooter_InvoiceTotals_TotalLineItemAmount],[Message_Document_Header_BeginningOfMessage_DocumentNameEncoded],[Message_Document_Header_BeginningOfMessage_DocumentNumber],[Message_Document_Header_BeginningOfMessage_MessageFunction],[Message_Document_Header_Dates_Date_DateQualifier],[Message_Document_Header_Dates_Date_DateTime],[Message_Document_Header_Dates_DocumentDate_DateQualifier],[Message_Document_Header_Dates_DocumentDate_DateTime],[Message_Document_Header_MessageHeader_MessageReferenceNumber],[Message_Document_Header_MessageHeader_MessageType],[Message_Document_Header_ReferenceCurrency],[Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateQualifier],[Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateTime],[Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceNumber],[Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceQualifier],[Message_Header_InterchangeHeader_ApplicationRef],[Message_Header_InterchangeHeader_ControlRef],[Message_Header_InterchangeHeader_DateTime_date],[Message_Header_InterchangeHeader_DateTime_time],[Message_Header_InterchangeHeader_Recipient_id],[Message_Header_InterchangeHeader_Sender_id])  VALUES (@EditTime,@IdFile,@IdOrder,@Message_Document_Footer_InvoiceFooter_InvoiceTotals_CurrencyQualifier,@Message_Document_Footer_InvoiceFooter_InvoiceTotals_TotalLineItemAmount,@Message_Document_Header_BeginningOfMessage_DocumentNameEncoded,@Message_Document_Header_BeginningOfMessage_DocumentNumber,@Message_Document_Header_BeginningOfMessage_MessageFunction,@Message_Document_Header_Dates_Date_DateQualifier,@Message_Document_Header_Dates_Date_DateTime,@Message_Document_Header_Dates_DocumentDate_DateQualifier,@Message_Document_Header_Dates_DocumentDate_DateTime,@Message_Document_Header_MessageHeader_MessageReferenceNumber,@Message_Document_Header_MessageHeader_MessageType,@Message_Document_Header_ReferenceCurrency,@Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateQualifier,@Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateTime,@Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceNumber,@Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceQualifier,@Message_Header_InterchangeHeader_ApplicationRef,@Message_Header_InterchangeHeader_ControlRef,@Message_Header_InterchangeHeader_DateTime_date,@Message_Header_InterchangeHeader_DateTime_time,@Message_Header_InterchangeHeader_Recipient_id,@Message_Header_InterchangeHeader_Sender_id);";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("EditTime", item.EditTime == null ? (object)DBNull.Value : item.EditTime);
					sqlCommand.Parameters.AddWithValue("IdFile", item.IdFile);
					sqlCommand.Parameters.AddWithValue("IdOrder", item.IdOrder);
					sqlCommand.Parameters.AddWithValue("Message_Document_Footer_InvoiceFooter_InvoiceTotals_CurrencyQualifier", item.Message_Document_Footer_InvoiceFooter_InvoiceTotals_CurrencyQualifier == null ? (object)DBNull.Value : item.Message_Document_Footer_InvoiceFooter_InvoiceTotals_CurrencyQualifier);
					sqlCommand.Parameters.AddWithValue("Message_Document_Footer_InvoiceFooter_InvoiceTotals_TotalLineItemAmount", item.Message_Document_Footer_InvoiceFooter_InvoiceTotals_TotalLineItemAmount == null ? (object)DBNull.Value : item.Message_Document_Footer_InvoiceFooter_InvoiceTotals_TotalLineItemAmount);
					sqlCommand.Parameters.AddWithValue("Message_Document_Header_BeginningOfMessage_DocumentNameEncoded", item.Message_Document_Header_BeginningOfMessage_DocumentNameEncoded == null ? (object)DBNull.Value : item.Message_Document_Header_BeginningOfMessage_DocumentNameEncoded);
					sqlCommand.Parameters.AddWithValue("Message_Document_Header_BeginningOfMessage_DocumentNumber", item.Message_Document_Header_BeginningOfMessage_DocumentNumber == null ? (object)DBNull.Value : item.Message_Document_Header_BeginningOfMessage_DocumentNumber);
					sqlCommand.Parameters.AddWithValue("Message_Document_Header_BeginningOfMessage_MessageFunction", item.Message_Document_Header_BeginningOfMessage_MessageFunction == null ? (object)DBNull.Value : item.Message_Document_Header_BeginningOfMessage_MessageFunction);
					sqlCommand.Parameters.AddWithValue("Message_Document_Header_Dates_Date_DateQualifier", item.Message_Document_Header_Dates_Date_DateQualifier == null ? (object)DBNull.Value : item.Message_Document_Header_Dates_Date_DateQualifier);
					sqlCommand.Parameters.AddWithValue("Message_Document_Header_Dates_Date_DateTime", item.Message_Document_Header_Dates_Date_DateTime == null ? (object)DBNull.Value : item.Message_Document_Header_Dates_Date_DateTime);
					sqlCommand.Parameters.AddWithValue("Message_Document_Header_Dates_DocumentDate_DateQualifier", item.Message_Document_Header_Dates_DocumentDate_DateQualifier == null ? (object)DBNull.Value : item.Message_Document_Header_Dates_DocumentDate_DateQualifier);
					sqlCommand.Parameters.AddWithValue("Message_Document_Header_Dates_DocumentDate_DateTime", item.Message_Document_Header_Dates_DocumentDate_DateTime == null ? (object)DBNull.Value : item.Message_Document_Header_Dates_DocumentDate_DateTime);
					sqlCommand.Parameters.AddWithValue("Message_Document_Header_MessageHeader_MessageReferenceNumber", item.Message_Document_Header_MessageHeader_MessageReferenceNumber == null ? (object)DBNull.Value : item.Message_Document_Header_MessageHeader_MessageReferenceNumber);
					sqlCommand.Parameters.AddWithValue("Message_Document_Header_MessageHeader_MessageType", item.Message_Document_Header_MessageHeader_MessageType == null ? (object)DBNull.Value : item.Message_Document_Header_MessageHeader_MessageType);
					sqlCommand.Parameters.AddWithValue("Message_Document_Header_ReferenceCurrency", item.Message_Document_Header_ReferenceCurrency == null ? (object)DBNull.Value : item.Message_Document_Header_ReferenceCurrency);
					sqlCommand.Parameters.AddWithValue("Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateQualifier", item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateQualifier == null ? (object)DBNull.Value : item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateQualifier);
					sqlCommand.Parameters.AddWithValue("Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateTime", item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateTime == null ? (object)DBNull.Value : item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateTime);
					sqlCommand.Parameters.AddWithValue("Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceNumber", item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceNumber == null ? (object)DBNull.Value : item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceNumber);
					sqlCommand.Parameters.AddWithValue("Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceQualifier", item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceQualifier == null ? (object)DBNull.Value : item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceQualifier);
					sqlCommand.Parameters.AddWithValue("Message_Header_InterchangeHeader_ApplicationRef", item.Message_Header_InterchangeHeader_ApplicationRef == null ? (object)DBNull.Value : item.Message_Header_InterchangeHeader_ApplicationRef);
					sqlCommand.Parameters.AddWithValue("Message_Header_InterchangeHeader_ControlRef", item.Message_Header_InterchangeHeader_ControlRef == null ? (object)DBNull.Value : item.Message_Header_InterchangeHeader_ControlRef);
					sqlCommand.Parameters.AddWithValue("Message_Header_InterchangeHeader_DateTime_date", item.Message_Header_InterchangeHeader_DateTime_date == null ? (object)DBNull.Value : item.Message_Header_InterchangeHeader_DateTime_date);
					sqlCommand.Parameters.AddWithValue("Message_Header_InterchangeHeader_DateTime_time", item.Message_Header_InterchangeHeader_DateTime_time == null ? (object)DBNull.Value : item.Message_Header_InterchangeHeader_DateTime_time);
					sqlCommand.Parameters.AddWithValue("Message_Header_InterchangeHeader_Recipient_id", item.Message_Header_InterchangeHeader_Recipient_id == null ? (object)DBNull.Value : item.Message_Header_InterchangeHeader_Recipient_id);
					sqlCommand.Parameters.AddWithValue("Message_Header_InterchangeHeader_Sender_id", item.Message_Header_InterchangeHeader_Sender_id == null ? (object)DBNull.Value : item.Message_Header_InterchangeHeader_Sender_id);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.PRS.XML_MessageEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 26; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.PRS.XML_MessageEntity> items)
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
						query += " INSERT INTO [__EDI_XML_Message] ([EditTime],[IdFile],[IdOrder],[Message_Document_Footer_InvoiceFooter_InvoiceTotals_CurrencyQualifier],[Message_Document_Footer_InvoiceFooter_InvoiceTotals_TotalLineItemAmount],[Message_Document_Header_BeginningOfMessage_DocumentNameEncoded],[Message_Document_Header_BeginningOfMessage_DocumentNumber],[Message_Document_Header_BeginningOfMessage_MessageFunction],[Message_Document_Header_Dates_Date_DateQualifier],[Message_Document_Header_Dates_Date_DateTime],[Message_Document_Header_Dates_DocumentDate_DateQualifier],[Message_Document_Header_Dates_DocumentDate_DateTime],[Message_Document_Header_MessageHeader_MessageReferenceNumber],[Message_Document_Header_MessageHeader_MessageType],[Message_Document_Header_ReferenceCurrency],[Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateQualifier],[Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateTime],[Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceNumber],[Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceQualifier],[Message_Header_InterchangeHeader_ApplicationRef],[Message_Header_InterchangeHeader_ControlRef],[Message_Header_InterchangeHeader_DateTime_date],[Message_Header_InterchangeHeader_DateTime_time],[Message_Header_InterchangeHeader_Recipient_id],[Message_Header_InterchangeHeader_Sender_id]) VALUES ( "

							+ "@EditTime" + i + ","
							+ "@IdFile" + i + ","
							+ "@IdOrder" + i + ","
							+ "@Message_Document_Footer_InvoiceFooter_InvoiceTotals_CurrencyQualifier" + i + ","
							+ "@Message_Document_Footer_InvoiceFooter_InvoiceTotals_TotalLineItemAmount" + i + ","
							+ "@Message_Document_Header_BeginningOfMessage_DocumentNameEncoded" + i + ","
							+ "@Message_Document_Header_BeginningOfMessage_DocumentNumber" + i + ","
							+ "@Message_Document_Header_BeginningOfMessage_MessageFunction" + i + ","
							+ "@Message_Document_Header_Dates_Date_DateQualifier" + i + ","
							+ "@Message_Document_Header_Dates_Date_DateTime" + i + ","
							+ "@Message_Document_Header_Dates_DocumentDate_DateQualifier" + i + ","
							+ "@Message_Document_Header_Dates_DocumentDate_DateTime" + i + ","
							+ "@Message_Document_Header_MessageHeader_MessageReferenceNumber" + i + ","
							+ "@Message_Document_Header_MessageHeader_MessageType" + i + ","
							+ "@Message_Document_Header_ReferenceCurrency" + i + ","
							+ "@Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateQualifier" + i + ","
							+ "@Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateTime" + i + ","
							+ "@Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceNumber" + i + ","
							+ "@Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceQualifier" + i + ","
							+ "@Message_Header_InterchangeHeader_ApplicationRef" + i + ","
							+ "@Message_Header_InterchangeHeader_ControlRef" + i + ","
							+ "@Message_Header_InterchangeHeader_DateTime_date" + i + ","
							+ "@Message_Header_InterchangeHeader_DateTime_time" + i + ","
							+ "@Message_Header_InterchangeHeader_Recipient_id" + i + ","
							+ "@Message_Header_InterchangeHeader_Sender_id" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("EditTime" + i, item.EditTime == null ? (object)DBNull.Value : item.EditTime);
						sqlCommand.Parameters.AddWithValue("IdFile" + i, item.IdFile);
						sqlCommand.Parameters.AddWithValue("IdOrder" + i, item.IdOrder);
						sqlCommand.Parameters.AddWithValue("Message_Document_Footer_InvoiceFooter_InvoiceTotals_CurrencyQualifier" + i, item.Message_Document_Footer_InvoiceFooter_InvoiceTotals_CurrencyQualifier == null ? (object)DBNull.Value : item.Message_Document_Footer_InvoiceFooter_InvoiceTotals_CurrencyQualifier);
						sqlCommand.Parameters.AddWithValue("Message_Document_Footer_InvoiceFooter_InvoiceTotals_TotalLineItemAmount" + i, item.Message_Document_Footer_InvoiceFooter_InvoiceTotals_TotalLineItemAmount == null ? (object)DBNull.Value : item.Message_Document_Footer_InvoiceFooter_InvoiceTotals_TotalLineItemAmount);
						sqlCommand.Parameters.AddWithValue("Message_Document_Header_BeginningOfMessage_DocumentNameEncoded" + i, item.Message_Document_Header_BeginningOfMessage_DocumentNameEncoded == null ? (object)DBNull.Value : item.Message_Document_Header_BeginningOfMessage_DocumentNameEncoded);
						sqlCommand.Parameters.AddWithValue("Message_Document_Header_BeginningOfMessage_DocumentNumber" + i, item.Message_Document_Header_BeginningOfMessage_DocumentNumber == null ? (object)DBNull.Value : item.Message_Document_Header_BeginningOfMessage_DocumentNumber);
						sqlCommand.Parameters.AddWithValue("Message_Document_Header_BeginningOfMessage_MessageFunction" + i, item.Message_Document_Header_BeginningOfMessage_MessageFunction == null ? (object)DBNull.Value : item.Message_Document_Header_BeginningOfMessage_MessageFunction);
						sqlCommand.Parameters.AddWithValue("Message_Document_Header_Dates_Date_DateQualifier" + i, item.Message_Document_Header_Dates_Date_DateQualifier == null ? (object)DBNull.Value : item.Message_Document_Header_Dates_Date_DateQualifier);
						sqlCommand.Parameters.AddWithValue("Message_Document_Header_Dates_Date_DateTime" + i, item.Message_Document_Header_Dates_Date_DateTime == null ? (object)DBNull.Value : item.Message_Document_Header_Dates_Date_DateTime);
						sqlCommand.Parameters.AddWithValue("Message_Document_Header_Dates_DocumentDate_DateQualifier" + i, item.Message_Document_Header_Dates_DocumentDate_DateQualifier == null ? (object)DBNull.Value : item.Message_Document_Header_Dates_DocumentDate_DateQualifier);
						sqlCommand.Parameters.AddWithValue("Message_Document_Header_Dates_DocumentDate_DateTime" + i, item.Message_Document_Header_Dates_DocumentDate_DateTime == null ? (object)DBNull.Value : item.Message_Document_Header_Dates_DocumentDate_DateTime);
						sqlCommand.Parameters.AddWithValue("Message_Document_Header_MessageHeader_MessageReferenceNumber" + i, item.Message_Document_Header_MessageHeader_MessageReferenceNumber == null ? (object)DBNull.Value : item.Message_Document_Header_MessageHeader_MessageReferenceNumber);
						sqlCommand.Parameters.AddWithValue("Message_Document_Header_MessageHeader_MessageType" + i, item.Message_Document_Header_MessageHeader_MessageType == null ? (object)DBNull.Value : item.Message_Document_Header_MessageHeader_MessageType);
						sqlCommand.Parameters.AddWithValue("Message_Document_Header_ReferenceCurrency" + i, item.Message_Document_Header_ReferenceCurrency == null ? (object)DBNull.Value : item.Message_Document_Header_ReferenceCurrency);
						sqlCommand.Parameters.AddWithValue("Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateQualifier" + i, item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateQualifier == null ? (object)DBNull.Value : item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateQualifier);
						sqlCommand.Parameters.AddWithValue("Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateTime" + i, item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateTime == null ? (object)DBNull.Value : item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateTime);
						sqlCommand.Parameters.AddWithValue("Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceNumber" + i, item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceNumber == null ? (object)DBNull.Value : item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceNumber);
						sqlCommand.Parameters.AddWithValue("Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceQualifier" + i, item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceQualifier == null ? (object)DBNull.Value : item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceQualifier);
						sqlCommand.Parameters.AddWithValue("Message_Header_InterchangeHeader_ApplicationRef" + i, item.Message_Header_InterchangeHeader_ApplicationRef == null ? (object)DBNull.Value : item.Message_Header_InterchangeHeader_ApplicationRef);
						sqlCommand.Parameters.AddWithValue("Message_Header_InterchangeHeader_ControlRef" + i, item.Message_Header_InterchangeHeader_ControlRef == null ? (object)DBNull.Value : item.Message_Header_InterchangeHeader_ControlRef);
						sqlCommand.Parameters.AddWithValue("Message_Header_InterchangeHeader_DateTime_date" + i, item.Message_Header_InterchangeHeader_DateTime_date == null ? (object)DBNull.Value : item.Message_Header_InterchangeHeader_DateTime_date);
						sqlCommand.Parameters.AddWithValue("Message_Header_InterchangeHeader_DateTime_time" + i, item.Message_Header_InterchangeHeader_DateTime_time == null ? (object)DBNull.Value : item.Message_Header_InterchangeHeader_DateTime_time);
						sqlCommand.Parameters.AddWithValue("Message_Header_InterchangeHeader_Recipient_id" + i, item.Message_Header_InterchangeHeader_Recipient_id == null ? (object)DBNull.Value : item.Message_Header_InterchangeHeader_Recipient_id);
						sqlCommand.Parameters.AddWithValue("Message_Header_InterchangeHeader_Sender_id" + i, item.Message_Header_InterchangeHeader_Sender_id == null ? (object)DBNull.Value : item.Message_Header_InterchangeHeader_Sender_id);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.PRS.XML_MessageEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__EDI_XML_Message] SET [EditTime]=@EditTime, [IdFile]=@IdFile, [IdOrder]=@IdOrder, [Message_Document_Footer_InvoiceFooter_InvoiceTotals_CurrencyQualifier]=@Message_Document_Footer_InvoiceFooter_InvoiceTotals_CurrencyQualifier, [Message_Document_Footer_InvoiceFooter_InvoiceTotals_TotalLineItemAmount]=@Message_Document_Footer_InvoiceFooter_InvoiceTotals_TotalLineItemAmount, [Message_Document_Header_BeginningOfMessage_DocumentNameEncoded]=@Message_Document_Header_BeginningOfMessage_DocumentNameEncoded, [Message_Document_Header_BeginningOfMessage_DocumentNumber]=@Message_Document_Header_BeginningOfMessage_DocumentNumber, [Message_Document_Header_BeginningOfMessage_MessageFunction]=@Message_Document_Header_BeginningOfMessage_MessageFunction, [Message_Document_Header_Dates_Date_DateQualifier]=@Message_Document_Header_Dates_Date_DateQualifier, [Message_Document_Header_Dates_Date_DateTime]=@Message_Document_Header_Dates_Date_DateTime, [Message_Document_Header_Dates_DocumentDate_DateQualifier]=@Message_Document_Header_Dates_DocumentDate_DateQualifier, [Message_Document_Header_Dates_DocumentDate_DateTime]=@Message_Document_Header_Dates_DocumentDate_DateTime, [Message_Document_Header_MessageHeader_MessageReferenceNumber]=@Message_Document_Header_MessageHeader_MessageReferenceNumber, [Message_Document_Header_MessageHeader_MessageType]=@Message_Document_Header_MessageHeader_MessageType, [Message_Document_Header_ReferenceCurrency]=@Message_Document_Header_ReferenceCurrency, [Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateQualifier]=@Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateQualifier, [Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateTime]=@Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateTime, [Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceNumber]=@Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceNumber, [Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceQualifier]=@Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceQualifier, [Message_Header_InterchangeHeader_ApplicationRef]=@Message_Header_InterchangeHeader_ApplicationRef, [Message_Header_InterchangeHeader_ControlRef]=@Message_Header_InterchangeHeader_ControlRef, [Message_Header_InterchangeHeader_DateTime_date]=@Message_Header_InterchangeHeader_DateTime_date, [Message_Header_InterchangeHeader_DateTime_time]=@Message_Header_InterchangeHeader_DateTime_time, [Message_Header_InterchangeHeader_Recipient_id]=@Message_Header_InterchangeHeader_Recipient_id, [Message_Header_InterchangeHeader_Sender_id]=@Message_Header_InterchangeHeader_Sender_id WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("EditTime", item.EditTime == null ? (object)DBNull.Value : item.EditTime);
				sqlCommand.Parameters.AddWithValue("IdFile", item.IdFile);
				sqlCommand.Parameters.AddWithValue("IdOrder", item.IdOrder);
				sqlCommand.Parameters.AddWithValue("Message_Document_Footer_InvoiceFooter_InvoiceTotals_CurrencyQualifier", item.Message_Document_Footer_InvoiceFooter_InvoiceTotals_CurrencyQualifier == null ? (object)DBNull.Value : item.Message_Document_Footer_InvoiceFooter_InvoiceTotals_CurrencyQualifier);
				sqlCommand.Parameters.AddWithValue("Message_Document_Footer_InvoiceFooter_InvoiceTotals_TotalLineItemAmount", item.Message_Document_Footer_InvoiceFooter_InvoiceTotals_TotalLineItemAmount == null ? (object)DBNull.Value : item.Message_Document_Footer_InvoiceFooter_InvoiceTotals_TotalLineItemAmount);
				sqlCommand.Parameters.AddWithValue("Message_Document_Header_BeginningOfMessage_DocumentNameEncoded", item.Message_Document_Header_BeginningOfMessage_DocumentNameEncoded == null ? (object)DBNull.Value : item.Message_Document_Header_BeginningOfMessage_DocumentNameEncoded);
				sqlCommand.Parameters.AddWithValue("Message_Document_Header_BeginningOfMessage_DocumentNumber", item.Message_Document_Header_BeginningOfMessage_DocumentNumber == null ? (object)DBNull.Value : item.Message_Document_Header_BeginningOfMessage_DocumentNumber);
				sqlCommand.Parameters.AddWithValue("Message_Document_Header_BeginningOfMessage_MessageFunction", item.Message_Document_Header_BeginningOfMessage_MessageFunction == null ? (object)DBNull.Value : item.Message_Document_Header_BeginningOfMessage_MessageFunction);
				sqlCommand.Parameters.AddWithValue("Message_Document_Header_Dates_Date_DateQualifier", item.Message_Document_Header_Dates_Date_DateQualifier == null ? (object)DBNull.Value : item.Message_Document_Header_Dates_Date_DateQualifier);
				sqlCommand.Parameters.AddWithValue("Message_Document_Header_Dates_Date_DateTime", item.Message_Document_Header_Dates_Date_DateTime == null ? (object)DBNull.Value : item.Message_Document_Header_Dates_Date_DateTime);
				sqlCommand.Parameters.AddWithValue("Message_Document_Header_Dates_DocumentDate_DateQualifier", item.Message_Document_Header_Dates_DocumentDate_DateQualifier == null ? (object)DBNull.Value : item.Message_Document_Header_Dates_DocumentDate_DateQualifier);
				sqlCommand.Parameters.AddWithValue("Message_Document_Header_Dates_DocumentDate_DateTime", item.Message_Document_Header_Dates_DocumentDate_DateTime == null ? (object)DBNull.Value : item.Message_Document_Header_Dates_DocumentDate_DateTime);
				sqlCommand.Parameters.AddWithValue("Message_Document_Header_MessageHeader_MessageReferenceNumber", item.Message_Document_Header_MessageHeader_MessageReferenceNumber == null ? (object)DBNull.Value : item.Message_Document_Header_MessageHeader_MessageReferenceNumber);
				sqlCommand.Parameters.AddWithValue("Message_Document_Header_MessageHeader_MessageType", item.Message_Document_Header_MessageHeader_MessageType == null ? (object)DBNull.Value : item.Message_Document_Header_MessageHeader_MessageType);
				sqlCommand.Parameters.AddWithValue("Message_Document_Header_ReferenceCurrency", item.Message_Document_Header_ReferenceCurrency == null ? (object)DBNull.Value : item.Message_Document_Header_ReferenceCurrency);
				sqlCommand.Parameters.AddWithValue("Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateQualifier", item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateQualifier == null ? (object)DBNull.Value : item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateQualifier);
				sqlCommand.Parameters.AddWithValue("Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateTime", item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateTime == null ? (object)DBNull.Value : item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateTime);
				sqlCommand.Parameters.AddWithValue("Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceNumber", item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceNumber == null ? (object)DBNull.Value : item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceNumber);
				sqlCommand.Parameters.AddWithValue("Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceQualifier", item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceQualifier == null ? (object)DBNull.Value : item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceQualifier);
				sqlCommand.Parameters.AddWithValue("Message_Header_InterchangeHeader_ApplicationRef", item.Message_Header_InterchangeHeader_ApplicationRef == null ? (object)DBNull.Value : item.Message_Header_InterchangeHeader_ApplicationRef);
				sqlCommand.Parameters.AddWithValue("Message_Header_InterchangeHeader_ControlRef", item.Message_Header_InterchangeHeader_ControlRef == null ? (object)DBNull.Value : item.Message_Header_InterchangeHeader_ControlRef);
				sqlCommand.Parameters.AddWithValue("Message_Header_InterchangeHeader_DateTime_date", item.Message_Header_InterchangeHeader_DateTime_date == null ? (object)DBNull.Value : item.Message_Header_InterchangeHeader_DateTime_date);
				sqlCommand.Parameters.AddWithValue("Message_Header_InterchangeHeader_DateTime_time", item.Message_Header_InterchangeHeader_DateTime_time == null ? (object)DBNull.Value : item.Message_Header_InterchangeHeader_DateTime_time);
				sqlCommand.Parameters.AddWithValue("Message_Header_InterchangeHeader_Recipient_id", item.Message_Header_InterchangeHeader_Recipient_id == null ? (object)DBNull.Value : item.Message_Header_InterchangeHeader_Recipient_id);
				sqlCommand.Parameters.AddWithValue("Message_Header_InterchangeHeader_Sender_id", item.Message_Header_InterchangeHeader_Sender_id == null ? (object)DBNull.Value : item.Message_Header_InterchangeHeader_Sender_id);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRS.XML_MessageEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 26; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.PRS.XML_MessageEntity> items)
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
						query += " UPDATE [__EDI_XML_Message] SET "

							+ "[EditTime]=@EditTime" + i + ","
							+ "[IdFile]=@IdFile" + i + ","
							+ "[IdOrder]=@IdOrder" + i + ","
							+ "[Message_Document_Footer_InvoiceFooter_InvoiceTotals_CurrencyQualifier]=@Message_Document_Footer_InvoiceFooter_InvoiceTotals_CurrencyQualifier" + i + ","
							+ "[Message_Document_Footer_InvoiceFooter_InvoiceTotals_TotalLineItemAmount]=@Message_Document_Footer_InvoiceFooter_InvoiceTotals_TotalLineItemAmount" + i + ","
							+ "[Message_Document_Header_BeginningOfMessage_DocumentNameEncoded]=@Message_Document_Header_BeginningOfMessage_DocumentNameEncoded" + i + ","
							+ "[Message_Document_Header_BeginningOfMessage_DocumentNumber]=@Message_Document_Header_BeginningOfMessage_DocumentNumber" + i + ","
							+ "[Message_Document_Header_BeginningOfMessage_MessageFunction]=@Message_Document_Header_BeginningOfMessage_MessageFunction" + i + ","
							+ "[Message_Document_Header_Dates_Date_DateQualifier]=@Message_Document_Header_Dates_Date_DateQualifier" + i + ","
							+ "[Message_Document_Header_Dates_Date_DateTime]=@Message_Document_Header_Dates_Date_DateTime" + i + ","
							+ "[Message_Document_Header_Dates_DocumentDate_DateQualifier]=@Message_Document_Header_Dates_DocumentDate_DateQualifier" + i + ","
							+ "[Message_Document_Header_Dates_DocumentDate_DateTime]=@Message_Document_Header_Dates_DocumentDate_DateTime" + i + ","
							+ "[Message_Document_Header_MessageHeader_MessageReferenceNumber]=@Message_Document_Header_MessageHeader_MessageReferenceNumber" + i + ","
							+ "[Message_Document_Header_MessageHeader_MessageType]=@Message_Document_Header_MessageHeader_MessageType" + i + ","
							+ "[Message_Document_Header_ReferenceCurrency]=@Message_Document_Header_ReferenceCurrency" + i + ","
							+ "[Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateQualifier]=@Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateQualifier" + i + ","
							+ "[Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateTime]=@Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateTime" + i + ","
							+ "[Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceNumber]=@Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceNumber" + i + ","
							+ "[Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceQualifier]=@Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceQualifier" + i + ","
							+ "[Message_Header_InterchangeHeader_ApplicationRef]=@Message_Header_InterchangeHeader_ApplicationRef" + i + ","
							+ "[Message_Header_InterchangeHeader_ControlRef]=@Message_Header_InterchangeHeader_ControlRef" + i + ","
							+ "[Message_Header_InterchangeHeader_DateTime_date]=@Message_Header_InterchangeHeader_DateTime_date" + i + ","
							+ "[Message_Header_InterchangeHeader_DateTime_time]=@Message_Header_InterchangeHeader_DateTime_time" + i + ","
							+ "[Message_Header_InterchangeHeader_Recipient_id]=@Message_Header_InterchangeHeader_Recipient_id" + i + ","
							+ "[Message_Header_InterchangeHeader_Sender_id]=@Message_Header_InterchangeHeader_Sender_id" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("EditTime" + i, item.EditTime == null ? (object)DBNull.Value : item.EditTime);
						sqlCommand.Parameters.AddWithValue("IdFile" + i, item.IdFile);
						sqlCommand.Parameters.AddWithValue("IdOrder" + i, item.IdOrder);
						sqlCommand.Parameters.AddWithValue("Message_Document_Footer_InvoiceFooter_InvoiceTotals_CurrencyQualifier" + i, item.Message_Document_Footer_InvoiceFooter_InvoiceTotals_CurrencyQualifier == null ? (object)DBNull.Value : item.Message_Document_Footer_InvoiceFooter_InvoiceTotals_CurrencyQualifier);
						sqlCommand.Parameters.AddWithValue("Message_Document_Footer_InvoiceFooter_InvoiceTotals_TotalLineItemAmount" + i, item.Message_Document_Footer_InvoiceFooter_InvoiceTotals_TotalLineItemAmount == null ? (object)DBNull.Value : item.Message_Document_Footer_InvoiceFooter_InvoiceTotals_TotalLineItemAmount);
						sqlCommand.Parameters.AddWithValue("Message_Document_Header_BeginningOfMessage_DocumentNameEncoded" + i, item.Message_Document_Header_BeginningOfMessage_DocumentNameEncoded == null ? (object)DBNull.Value : item.Message_Document_Header_BeginningOfMessage_DocumentNameEncoded);
						sqlCommand.Parameters.AddWithValue("Message_Document_Header_BeginningOfMessage_DocumentNumber" + i, item.Message_Document_Header_BeginningOfMessage_DocumentNumber == null ? (object)DBNull.Value : item.Message_Document_Header_BeginningOfMessage_DocumentNumber);
						sqlCommand.Parameters.AddWithValue("Message_Document_Header_BeginningOfMessage_MessageFunction" + i, item.Message_Document_Header_BeginningOfMessage_MessageFunction == null ? (object)DBNull.Value : item.Message_Document_Header_BeginningOfMessage_MessageFunction);
						sqlCommand.Parameters.AddWithValue("Message_Document_Header_Dates_Date_DateQualifier" + i, item.Message_Document_Header_Dates_Date_DateQualifier == null ? (object)DBNull.Value : item.Message_Document_Header_Dates_Date_DateQualifier);
						sqlCommand.Parameters.AddWithValue("Message_Document_Header_Dates_Date_DateTime" + i, item.Message_Document_Header_Dates_Date_DateTime == null ? (object)DBNull.Value : item.Message_Document_Header_Dates_Date_DateTime);
						sqlCommand.Parameters.AddWithValue("Message_Document_Header_Dates_DocumentDate_DateQualifier" + i, item.Message_Document_Header_Dates_DocumentDate_DateQualifier == null ? (object)DBNull.Value : item.Message_Document_Header_Dates_DocumentDate_DateQualifier);
						sqlCommand.Parameters.AddWithValue("Message_Document_Header_Dates_DocumentDate_DateTime" + i, item.Message_Document_Header_Dates_DocumentDate_DateTime == null ? (object)DBNull.Value : item.Message_Document_Header_Dates_DocumentDate_DateTime);
						sqlCommand.Parameters.AddWithValue("Message_Document_Header_MessageHeader_MessageReferenceNumber" + i, item.Message_Document_Header_MessageHeader_MessageReferenceNumber == null ? (object)DBNull.Value : item.Message_Document_Header_MessageHeader_MessageReferenceNumber);
						sqlCommand.Parameters.AddWithValue("Message_Document_Header_MessageHeader_MessageType" + i, item.Message_Document_Header_MessageHeader_MessageType == null ? (object)DBNull.Value : item.Message_Document_Header_MessageHeader_MessageType);
						sqlCommand.Parameters.AddWithValue("Message_Document_Header_ReferenceCurrency" + i, item.Message_Document_Header_ReferenceCurrency == null ? (object)DBNull.Value : item.Message_Document_Header_ReferenceCurrency);
						sqlCommand.Parameters.AddWithValue("Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateQualifier" + i, item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateQualifier == null ? (object)DBNull.Value : item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateQualifier);
						sqlCommand.Parameters.AddWithValue("Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateTime" + i, item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateTime == null ? (object)DBNull.Value : item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_Date_DateTime);
						sqlCommand.Parameters.AddWithValue("Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceNumber" + i, item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceNumber == null ? (object)DBNull.Value : item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceNumber);
						sqlCommand.Parameters.AddWithValue("Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceQualifier" + i, item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceQualifier == null ? (object)DBNull.Value : item.Message_Document_Header_ReferencedDocuments_PurchaseOrderReferenceNumber_ReferenceQualifier);
						sqlCommand.Parameters.AddWithValue("Message_Header_InterchangeHeader_ApplicationRef" + i, item.Message_Header_InterchangeHeader_ApplicationRef == null ? (object)DBNull.Value : item.Message_Header_InterchangeHeader_ApplicationRef);
						sqlCommand.Parameters.AddWithValue("Message_Header_InterchangeHeader_ControlRef" + i, item.Message_Header_InterchangeHeader_ControlRef == null ? (object)DBNull.Value : item.Message_Header_InterchangeHeader_ControlRef);
						sqlCommand.Parameters.AddWithValue("Message_Header_InterchangeHeader_DateTime_date" + i, item.Message_Header_InterchangeHeader_DateTime_date == null ? (object)DBNull.Value : item.Message_Header_InterchangeHeader_DateTime_date);
						sqlCommand.Parameters.AddWithValue("Message_Header_InterchangeHeader_DateTime_time" + i, item.Message_Header_InterchangeHeader_DateTime_time == null ? (object)DBNull.Value : item.Message_Header_InterchangeHeader_DateTime_time);
						sqlCommand.Parameters.AddWithValue("Message_Header_InterchangeHeader_Recipient_id" + i, item.Message_Header_InterchangeHeader_Recipient_id == null ? (object)DBNull.Value : item.Message_Header_InterchangeHeader_Recipient_id);
						sqlCommand.Parameters.AddWithValue("Message_Header_InterchangeHeader_Sender_id" + i, item.Message_Header_InterchangeHeader_Sender_id == null ? (object)DBNull.Value : item.Message_Header_InterchangeHeader_Sender_id);
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
				string query = "DELETE FROM [__EDI_XML_Message] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__EDI_XML_Message] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods


		public static int GetMaxFileId()
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT CASE WHEN MAX([IdFile]) IS NULL THEN 0 ELSE MAX([IdFile]) END AS MaxId FROM [__EDI_XML_Message]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				return int.Parse(DbExecution.ExecuteScalar(sqlCommand).ToString()) + 1;

			}
		}
		public static Infrastructure.Data.Entities.Tables.PRS.XML_MessageEntity GetByOrderId(int orderId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__EDI_XML_Message] WHERE [IdOrder]=@orderId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("orderId", orderId);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.XML_MessageEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		#endregion
	}
}
