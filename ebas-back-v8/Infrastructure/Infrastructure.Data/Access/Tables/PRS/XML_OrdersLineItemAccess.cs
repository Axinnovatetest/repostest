using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.PRS
{

	public class XML_OrdersLineItemAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.XML_OrdersLineItemEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__EDI_XML_OrdersLineItem] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.XML_OrdersLineItemEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersLineItemEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__EDI_XML_OrdersLineItem]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.XML_OrdersLineItemEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersLineItemEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersLineItemEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersLineItemEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersLineItemEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersLineItemEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersLineItemEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__EDI_XML_OrdersLineItem] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.XML_OrdersLineItemEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersLineItemEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersLineItemEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.XML_OrdersLineItemEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__EDI_XML_OrdersLineItem] ([EditTime],[IdFile],[Message_Document_Details_OrdersData_OrdersLineItem_CurrentItemPriceCalculationNet],[Message_Document_Details_OrdersData_OrdersLineItem_CustomersItemMaterialNumber],[Message_Document_Details_OrdersData_OrdersLineItem_FreeText_Text],[Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextLanguage],[Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextQualifier],[Message_Document_Details_OrdersData_OrdersLineItem_ItemDescription],[Message_Document_Details_OrdersData_OrdersLineItem_LineItemActionRequest],[Message_Document_Details_OrdersData_OrdersLineItem_LineItemAmount],[Message_Document_Details_OrdersData_OrdersLineItem_MeasureUnitQualifier],[Message_Document_Details_OrdersData_OrdersLineItem_OrderedQuantity],[Message_Document_Details_OrdersData_OrdersLineItem_PositionNumber],[Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateQualifier],[Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateTime],[Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_LineNumber],[Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceNumber],[Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceQualifier],[Message_Document_Details_OrdersData_OrdersLineItem_SuppliersItemMaterialNumber],[Message_Document_Details_OrdersData_OrdersLineItem_UnitPriceBasis])  VALUES (@EditTime,@IdFile,@Message_Document_Details_OrdersData_OrdersLineItem_CurrentItemPriceCalculationNet,@Message_Document_Details_OrdersData_OrdersLineItem_CustomersItemMaterialNumber,@Message_Document_Details_OrdersData_OrdersLineItem_FreeText_Text,@Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextLanguage,@Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextQualifier,@Message_Document_Details_OrdersData_OrdersLineItem_ItemDescription,@Message_Document_Details_OrdersData_OrdersLineItem_LineItemActionRequest,@Message_Document_Details_OrdersData_OrdersLineItem_LineItemAmount,@Message_Document_Details_OrdersData_OrdersLineItem_MeasureUnitQualifier,@Message_Document_Details_OrdersData_OrdersLineItem_OrderedQuantity,@Message_Document_Details_OrdersData_OrdersLineItem_PositionNumber,@Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateQualifier,@Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateTime,@Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_LineNumber,@Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceNumber,@Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceQualifier,@Message_Document_Details_OrdersData_OrdersLineItem_SuppliersItemMaterialNumber,@Message_Document_Details_OrdersData_OrdersLineItem_UnitPriceBasis);";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("EditTime", item.EditTime == null ? (object)DBNull.Value : item.EditTime);
					sqlCommand.Parameters.AddWithValue("IdFile", item.IdFile);
					sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_CurrentItemPriceCalculationNet", item.Message_Document_Details_OrdersData_OrdersLineItem_CurrentItemPriceCalculationNet == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_CurrentItemPriceCalculationNet);
					sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_CustomersItemMaterialNumber", item.Message_Document_Details_OrdersData_OrdersLineItem_CustomersItemMaterialNumber == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_CustomersItemMaterialNumber);
					sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_FreeText_Text", item.Message_Document_Details_OrdersData_OrdersLineItem_FreeText_Text == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_FreeText_Text);
					sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextLanguage", item.Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextLanguage == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextLanguage);
					sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextQualifier", item.Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextQualifier == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextQualifier);
					sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_ItemDescription", item.Message_Document_Details_OrdersData_OrdersLineItem_ItemDescription == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_ItemDescription);
					sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_LineItemActionRequest", item.Message_Document_Details_OrdersData_OrdersLineItem_LineItemActionRequest == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_LineItemActionRequest);
					sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_LineItemAmount", item.Message_Document_Details_OrdersData_OrdersLineItem_LineItemAmount == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_LineItemAmount);
					sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_MeasureUnitQualifier", item.Message_Document_Details_OrdersData_OrdersLineItem_MeasureUnitQualifier == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_MeasureUnitQualifier);
					sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_OrderedQuantity", item.Message_Document_Details_OrdersData_OrdersLineItem_OrderedQuantity == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_OrderedQuantity);
					sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_PositionNumber", item.Message_Document_Details_OrdersData_OrdersLineItem_PositionNumber == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_PositionNumber);
					sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateQualifier", item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateQualifier == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateQualifier);
					sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateTime", item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateTime == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateTime);
					sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_LineNumber", item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_LineNumber == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_LineNumber);
					sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceNumber", item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceNumber == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceNumber);
					sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceQualifier", item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceQualifier == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceQualifier);
					sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_SuppliersItemMaterialNumber", item.Message_Document_Details_OrdersData_OrdersLineItem_SuppliersItemMaterialNumber == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_SuppliersItemMaterialNumber);
					sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_UnitPriceBasis", item.Message_Document_Details_OrdersData_OrdersLineItem_UnitPriceBasis == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_UnitPriceBasis);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersLineItemEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 21; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersLineItemEntity> items)
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
						query += " INSERT INTO [__EDI_XML_OrdersLineItem] ([EditTime],[IdFile],[Message_Document_Details_OrdersData_OrdersLineItem_CurrentItemPriceCalculationNet],[Message_Document_Details_OrdersData_OrdersLineItem_CustomersItemMaterialNumber],[Message_Document_Details_OrdersData_OrdersLineItem_FreeText_Text],[Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextLanguage],[Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextQualifier],[Message_Document_Details_OrdersData_OrdersLineItem_ItemDescription],[Message_Document_Details_OrdersData_OrdersLineItem_LineItemActionRequest],[Message_Document_Details_OrdersData_OrdersLineItem_LineItemAmount],[Message_Document_Details_OrdersData_OrdersLineItem_MeasureUnitQualifier],[Message_Document_Details_OrdersData_OrdersLineItem_OrderedQuantity],[Message_Document_Details_OrdersData_OrdersLineItem_PositionNumber],[Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateQualifier],[Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateTime],[Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_LineNumber],[Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceNumber],[Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceQualifier],[Message_Document_Details_OrdersData_OrdersLineItem_SuppliersItemMaterialNumber],[Message_Document_Details_OrdersData_OrdersLineItem_UnitPriceBasis]) VALUES ( "

							+ "@EditTime" + i + ","
							+ "@IdFile" + i + ","
							+ "@Message_Document_Details_OrdersData_OrdersLineItem_CurrentItemPriceCalculationNet" + i + ","
							+ "@Message_Document_Details_OrdersData_OrdersLineItem_CustomersItemMaterialNumber" + i + ","
							+ "@Message_Document_Details_OrdersData_OrdersLineItem_FreeText_Text" + i + ","
							+ "@Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextLanguage" + i + ","
							+ "@Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextQualifier" + i + ","
							+ "@Message_Document_Details_OrdersData_OrdersLineItem_ItemDescription" + i + ","
							+ "@Message_Document_Details_OrdersData_OrdersLineItem_LineItemActionRequest" + i + ","
							+ "@Message_Document_Details_OrdersData_OrdersLineItem_LineItemAmount" + i + ","
							+ "@Message_Document_Details_OrdersData_OrdersLineItem_MeasureUnitQualifier" + i + ","
							+ "@Message_Document_Details_OrdersData_OrdersLineItem_OrderedQuantity" + i + ","
							+ "@Message_Document_Details_OrdersData_OrdersLineItem_PositionNumber" + i + ","
							+ "@Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateQualifier" + i + ","
							+ "@Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateTime" + i + ","
							+ "@Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_LineNumber" + i + ","
							+ "@Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceNumber" + i + ","
							+ "@Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceQualifier" + i + ","
							+ "@Message_Document_Details_OrdersData_OrdersLineItem_SuppliersItemMaterialNumber" + i + ","
							+ "@Message_Document_Details_OrdersData_OrdersLineItem_UnitPriceBasis" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("EditTime" + i, item.EditTime == null ? (object)DBNull.Value : item.EditTime);
						sqlCommand.Parameters.AddWithValue("IdFile" + i, item.IdFile);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_CurrentItemPriceCalculationNet" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_CurrentItemPriceCalculationNet == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_CurrentItemPriceCalculationNet);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_CustomersItemMaterialNumber" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_CustomersItemMaterialNumber == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_CustomersItemMaterialNumber);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_FreeText_Text" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_FreeText_Text == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_FreeText_Text);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextLanguage" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextLanguage == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextLanguage);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextQualifier" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextQualifier == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextQualifier);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_ItemDescription" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_ItemDescription == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_ItemDescription);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_LineItemActionRequest" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_LineItemActionRequest == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_LineItemActionRequest);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_LineItemAmount" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_LineItemAmount == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_LineItemAmount);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_MeasureUnitQualifier" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_MeasureUnitQualifier == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_MeasureUnitQualifier);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_OrderedQuantity" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_OrderedQuantity == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_OrderedQuantity);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_PositionNumber" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_PositionNumber == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_PositionNumber);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateQualifier" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateQualifier == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateQualifier);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateTime" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateTime == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateTime);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_LineNumber" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_LineNumber == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_LineNumber);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceNumber" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceNumber == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceNumber);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceQualifier" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceQualifier == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceQualifier);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_SuppliersItemMaterialNumber" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_SuppliersItemMaterialNumber == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_SuppliersItemMaterialNumber);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_UnitPriceBasis" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_UnitPriceBasis == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_UnitPriceBasis);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.PRS.XML_OrdersLineItemEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__EDI_XML_OrdersLineItem] SET [EditTime]=@EditTime, [IdFile]=@IdFile, [Message_Document_Details_OrdersData_OrdersLineItem_CurrentItemPriceCalculationNet]=@Message_Document_Details_OrdersData_OrdersLineItem_CurrentItemPriceCalculationNet, [Message_Document_Details_OrdersData_OrdersLineItem_CustomersItemMaterialNumber]=@Message_Document_Details_OrdersData_OrdersLineItem_CustomersItemMaterialNumber, [Message_Document_Details_OrdersData_OrdersLineItem_FreeText_Text]=@Message_Document_Details_OrdersData_OrdersLineItem_FreeText_Text, [Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextLanguage]=@Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextLanguage, [Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextQualifier]=@Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextQualifier, [Message_Document_Details_OrdersData_OrdersLineItem_ItemDescription]=@Message_Document_Details_OrdersData_OrdersLineItem_ItemDescription, [Message_Document_Details_OrdersData_OrdersLineItem_LineItemActionRequest]=@Message_Document_Details_OrdersData_OrdersLineItem_LineItemActionRequest, [Message_Document_Details_OrdersData_OrdersLineItem_LineItemAmount]=@Message_Document_Details_OrdersData_OrdersLineItem_LineItemAmount, [Message_Document_Details_OrdersData_OrdersLineItem_MeasureUnitQualifier]=@Message_Document_Details_OrdersData_OrdersLineItem_MeasureUnitQualifier, [Message_Document_Details_OrdersData_OrdersLineItem_OrderedQuantity]=@Message_Document_Details_OrdersData_OrdersLineItem_OrderedQuantity, [Message_Document_Details_OrdersData_OrdersLineItem_PositionNumber]=@Message_Document_Details_OrdersData_OrdersLineItem_PositionNumber, [Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateQualifier]=@Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateQualifier, [Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateTime]=@Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateTime, [Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_LineNumber]=@Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_LineNumber, [Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceNumber]=@Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceNumber, [Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceQualifier]=@Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceQualifier, [Message_Document_Details_OrdersData_OrdersLineItem_SuppliersItemMaterialNumber]=@Message_Document_Details_OrdersData_OrdersLineItem_SuppliersItemMaterialNumber, [Message_Document_Details_OrdersData_OrdersLineItem_UnitPriceBasis]=@Message_Document_Details_OrdersData_OrdersLineItem_UnitPriceBasis WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("EditTime", item.EditTime == null ? (object)DBNull.Value : item.EditTime);
				sqlCommand.Parameters.AddWithValue("IdFile", item.IdFile);
				sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_CurrentItemPriceCalculationNet", item.Message_Document_Details_OrdersData_OrdersLineItem_CurrentItemPriceCalculationNet == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_CurrentItemPriceCalculationNet);
				sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_CustomersItemMaterialNumber", item.Message_Document_Details_OrdersData_OrdersLineItem_CustomersItemMaterialNumber == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_CustomersItemMaterialNumber);
				sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_FreeText_Text", item.Message_Document_Details_OrdersData_OrdersLineItem_FreeText_Text == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_FreeText_Text);
				sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextLanguage", item.Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextLanguage == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextLanguage);
				sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextQualifier", item.Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextQualifier == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextQualifier);
				sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_ItemDescription", item.Message_Document_Details_OrdersData_OrdersLineItem_ItemDescription == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_ItemDescription);
				sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_LineItemActionRequest", item.Message_Document_Details_OrdersData_OrdersLineItem_LineItemActionRequest == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_LineItemActionRequest);
				sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_LineItemAmount", item.Message_Document_Details_OrdersData_OrdersLineItem_LineItemAmount == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_LineItemAmount);
				sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_MeasureUnitQualifier", item.Message_Document_Details_OrdersData_OrdersLineItem_MeasureUnitQualifier == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_MeasureUnitQualifier);
				sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_OrderedQuantity", item.Message_Document_Details_OrdersData_OrdersLineItem_OrderedQuantity == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_OrderedQuantity);
				sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_PositionNumber", item.Message_Document_Details_OrdersData_OrdersLineItem_PositionNumber == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_PositionNumber);
				sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateQualifier", item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateQualifier == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateQualifier);
				sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateTime", item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateTime == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateTime);
				sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_LineNumber", item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_LineNumber == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_LineNumber);
				sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceNumber", item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceNumber == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceNumber);
				sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceQualifier", item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceQualifier == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceQualifier);
				sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_SuppliersItemMaterialNumber", item.Message_Document_Details_OrdersData_OrdersLineItem_SuppliersItemMaterialNumber == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_SuppliersItemMaterialNumber);
				sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_UnitPriceBasis", item.Message_Document_Details_OrdersData_OrdersLineItem_UnitPriceBasis == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_UnitPriceBasis);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersLineItemEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 21; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersLineItemEntity> items)
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
						query += " UPDATE [__EDI_XML_OrdersLineItem] SET "

							+ "[EditTime]=@EditTime" + i + ","
							+ "[IdFile]=@IdFile" + i + ","
							+ "[Message_Document_Details_OrdersData_OrdersLineItem_CurrentItemPriceCalculationNet]=@Message_Document_Details_OrdersData_OrdersLineItem_CurrentItemPriceCalculationNet" + i + ","
							+ "[Message_Document_Details_OrdersData_OrdersLineItem_CustomersItemMaterialNumber]=@Message_Document_Details_OrdersData_OrdersLineItem_CustomersItemMaterialNumber" + i + ","
							+ "[Message_Document_Details_OrdersData_OrdersLineItem_FreeText_Text]=@Message_Document_Details_OrdersData_OrdersLineItem_FreeText_Text" + i + ","
							+ "[Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextLanguage]=@Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextLanguage" + i + ","
							+ "[Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextQualifier]=@Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextQualifier" + i + ","
							+ "[Message_Document_Details_OrdersData_OrdersLineItem_ItemDescription]=@Message_Document_Details_OrdersData_OrdersLineItem_ItemDescription" + i + ","
							+ "[Message_Document_Details_OrdersData_OrdersLineItem_LineItemActionRequest]=@Message_Document_Details_OrdersData_OrdersLineItem_LineItemActionRequest" + i + ","
							+ "[Message_Document_Details_OrdersData_OrdersLineItem_LineItemAmount]=@Message_Document_Details_OrdersData_OrdersLineItem_LineItemAmount" + i + ","
							+ "[Message_Document_Details_OrdersData_OrdersLineItem_MeasureUnitQualifier]=@Message_Document_Details_OrdersData_OrdersLineItem_MeasureUnitQualifier" + i + ","
							+ "[Message_Document_Details_OrdersData_OrdersLineItem_OrderedQuantity]=@Message_Document_Details_OrdersData_OrdersLineItem_OrderedQuantity" + i + ","
							+ "[Message_Document_Details_OrdersData_OrdersLineItem_PositionNumber]=@Message_Document_Details_OrdersData_OrdersLineItem_PositionNumber" + i + ","
							+ "[Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateQualifier]=@Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateQualifier" + i + ","
							+ "[Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateTime]=@Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateTime" + i + ","
							+ "[Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_LineNumber]=@Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_LineNumber" + i + ","
							+ "[Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceNumber]=@Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceNumber" + i + ","
							+ "[Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceQualifier]=@Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceQualifier" + i + ","
							+ "[Message_Document_Details_OrdersData_OrdersLineItem_SuppliersItemMaterialNumber]=@Message_Document_Details_OrdersData_OrdersLineItem_SuppliersItemMaterialNumber" + i + ","
							+ "[Message_Document_Details_OrdersData_OrdersLineItem_UnitPriceBasis]=@Message_Document_Details_OrdersData_OrdersLineItem_UnitPriceBasis" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("EditTime" + i, item.EditTime == null ? (object)DBNull.Value : item.EditTime);
						sqlCommand.Parameters.AddWithValue("IdFile" + i, item.IdFile);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_CurrentItemPriceCalculationNet" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_CurrentItemPriceCalculationNet == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_CurrentItemPriceCalculationNet);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_CustomersItemMaterialNumber" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_CustomersItemMaterialNumber == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_CustomersItemMaterialNumber);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_FreeText_Text" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_FreeText_Text == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_FreeText_Text);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextLanguage" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextLanguage == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextLanguage);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextQualifier" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextQualifier == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_FreeText_TextQualifier);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_ItemDescription" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_ItemDescription == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_ItemDescription);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_LineItemActionRequest" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_LineItemActionRequest == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_LineItemActionRequest);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_LineItemAmount" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_LineItemAmount == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_LineItemAmount);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_MeasureUnitQualifier" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_MeasureUnitQualifier == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_MeasureUnitQualifier);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_OrderedQuantity" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_OrderedQuantity == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_OrderedQuantity);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_PositionNumber" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_PositionNumber == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_PositionNumber);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateQualifier" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateQualifier == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateQualifier);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateTime" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateTime == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_Date_DateTime);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_LineNumber" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_LineNumber == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_LineNumber);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceNumber" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceNumber == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceNumber);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceQualifier" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceQualifier == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_References_OrderReference_ReferenceQualifier);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_SuppliersItemMaterialNumber" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_SuppliersItemMaterialNumber == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_SuppliersItemMaterialNumber);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_UnitPriceBasis" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_UnitPriceBasis == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_UnitPriceBasis);
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
				string query = "DELETE FROM [__EDI_XML_OrdersLineItem] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__EDI_XML_OrdersLineItem] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersLineItemEntity> GetByFileId(int fileId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__EDI_XML_OrdersLineItem] WHERE [IdFile]=@fileId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("fileId", fileId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.XML_OrdersLineItemEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersLineItemEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersLineItemEntity> GetByFileIdAndPositionNr(int fileId, int positionNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__EDI_XML_OrdersLineItem] WHERE [IdFile]=@fileId AND [Message_Document_Details_OrdersData_OrdersLineItem_PositionNumber]=@positionNr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("fileId", fileId);
				sqlCommand.Parameters.AddWithValue("positionNr", positionNr);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.XML_OrdersLineItemEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersLineItemEntity>();
			}
		}
		#endregion
	}
}
