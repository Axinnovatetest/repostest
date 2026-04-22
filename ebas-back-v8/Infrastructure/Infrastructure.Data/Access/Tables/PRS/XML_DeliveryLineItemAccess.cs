using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.PRS
{

	public class XML_DeliveryLineItemAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.XML_DeliveryLineItemEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__EDI_XML_DeliveryLineItem] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.XML_DeliveryLineItemEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.XML_DeliveryLineItemEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__EDI_XML_DeliveryLineItem]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.XML_DeliveryLineItemEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.XML_DeliveryLineItemEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.XML_DeliveryLineItemEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.XML_DeliveryLineItemEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.XML_DeliveryLineItemEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.XML_DeliveryLineItemEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.XML_DeliveryLineItemEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [__EDI_XML_DeliveryLineItem] WHERE [Id] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.XML_DeliveryLineItemEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.XML_DeliveryLineItemEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.XML_DeliveryLineItemEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.XML_DeliveryLineItemEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__EDI_XML_DeliveryLineItem] ([IdFile],[Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_NetWeight],[Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_PositionNumber],[Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_ShippingMark],[Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_Weight])  VALUES (@IdFile,@Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_NetWeight,@Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_PositionNumber,@Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_ShippingMark,@Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_Weight);";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("IdFile", item.IdFile);
					sqlCommand.Parameters.AddWithValue("Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_NetWeight", item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_NetWeight == null ? (object)DBNull.Value : item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_NetWeight);
					sqlCommand.Parameters.AddWithValue("Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_PositionNumber", item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_PositionNumber == null ? (object)DBNull.Value : item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_PositionNumber);
					sqlCommand.Parameters.AddWithValue("Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_ShippingMark", item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_ShippingMark == null ? (object)DBNull.Value : item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_ShippingMark);
					sqlCommand.Parameters.AddWithValue("Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_Weight", item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_Weight == null ? (object)DBNull.Value : item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_Weight);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.PRS.XML_DeliveryLineItemEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 6; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.PRS.XML_DeliveryLineItemEntity> items)
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
						query += " INSERT INTO [__EDI_XML_DeliveryLineItem] ([IdFile],[Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_NetWeight],[Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_PositionNumber],[Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_ShippingMark],[Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_Weight]) VALUES ( "

							+ "@IdFile" + i + ","
							+ "@Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_NetWeight" + i + ","
							+ "@Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_PositionNumber" + i + ","
							+ "@Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_ShippingMark" + i + ","
							+ "@Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_Weight" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("IdFile" + i, item.IdFile);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_NetWeight" + i, item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_NetWeight == null ? (object)DBNull.Value : item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_NetWeight);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_PositionNumber" + i, item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_PositionNumber == null ? (object)DBNull.Value : item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_PositionNumber);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_ShippingMark" + i, item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_ShippingMark == null ? (object)DBNull.Value : item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_ShippingMark);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_Weight" + i, item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_Weight == null ? (object)DBNull.Value : item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_Weight);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.PRS.XML_DeliveryLineItemEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__EDI_XML_DeliveryLineItem] SET [IdFile]=@IdFile, [Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_NetWeight]=@Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_NetWeight, [Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_PositionNumber]=@Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_PositionNumber, [Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_ShippingMark]=@Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_ShippingMark, [Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_Weight]=@Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_Weight WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("IdFile", item.IdFile);
				sqlCommand.Parameters.AddWithValue("Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_NetWeight", item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_NetWeight == null ? (object)DBNull.Value : item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_NetWeight);
				sqlCommand.Parameters.AddWithValue("Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_PositionNumber", item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_PositionNumber == null ? (object)DBNull.Value : item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_PositionNumber);
				sqlCommand.Parameters.AddWithValue("Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_ShippingMark", item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_ShippingMark == null ? (object)DBNull.Value : item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_ShippingMark);
				sqlCommand.Parameters.AddWithValue("Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_Weight", item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_Weight == null ? (object)DBNull.Value : item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_Weight);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRS.XML_DeliveryLineItemEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 6; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.PRS.XML_DeliveryLineItemEntity> items)
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
						query += " UPDATE [__EDI_XML_DeliveryLineItem] SET "

							+ "[IdFile]=@IdFile" + i + ","
							+ "[Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_NetWeight]=@Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_NetWeight" + i + ","
							+ "[Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_PositionNumber]=@Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_PositionNumber" + i + ","
							+ "[Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_ShippingMark]=@Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_ShippingMark" + i + ","
							+ "[Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_Weight]=@Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_Weight" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("IdFile" + i, item.IdFile);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_NetWeight" + i, item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_NetWeight == null ? (object)DBNull.Value : item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_NetWeight);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_PositionNumber" + i, item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_PositionNumber == null ? (object)DBNull.Value : item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_PositionNumber);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_ShippingMark" + i, item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_ShippingMark == null ? (object)DBNull.Value : item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_ShippingMark);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_Weight" + i, item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_Weight == null ? (object)DBNull.Value : item.Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_Weight);
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
				string query = "DELETE FROM [__EDI_XML_DeliveryLineItem] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__EDI_XML_DeliveryLineItem] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.PRS.XML_DeliveryLineItemEntity> GetByFileId(int fileId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__EDI_XML_DeliveryLineItem] WHERE [IdFile]=@fileId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("fileId", fileId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.XML_DeliveryLineItemEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.XML_DeliveryLineItemEntity> GetByFileIdAndPositionNr(int fileId, int positionNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__EDI_XML_DeliveryLineItem] WHERE [IdFile]=@fileId AND [Message_Document_Details_DispatchData_ConsignmentPackagingSequence_DeliveryLineItems_PositionNumber]=@positionNr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("fileId", fileId);
				sqlCommand.Parameters.AddWithValue("positionNr", positionNr);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.XML_DeliveryLineItemEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}

		#endregion
	}
}
