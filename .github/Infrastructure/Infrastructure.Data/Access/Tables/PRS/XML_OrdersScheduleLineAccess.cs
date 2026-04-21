using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.PRS
{

	public class XML_OrdersScheduleLineAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.XML_OrdersScheduleLineEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__EDI_XML_OrdersScheduleLine] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.XML_OrdersScheduleLineEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersScheduleLineEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__EDI_XML_OrdersScheduleLine]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.XML_OrdersScheduleLineEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersScheduleLineEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersScheduleLineEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersScheduleLineEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersScheduleLineEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersScheduleLineEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersScheduleLineEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [__EDI_XML_OrdersScheduleLine] WHERE [Id] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.XML_OrdersScheduleLineEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersScheduleLineEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersScheduleLineEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.XML_OrdersScheduleLineEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__EDI_XML_OrdersScheduleLine] ([CreateTime],[EditTime],[IdOrderElement],[IdOrdersLineItem],[Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_PreviousScheduledQuantity],[Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantity],[Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateQualifier],[Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateTime])  VALUES (@CreateTime,@EditTime,@IdOrderElement,@IdOrdersLineItem,@Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_PreviousScheduledQuantity,@Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantity,@Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateQualifier,@Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateTime);";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("CreateTime", item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
					sqlCommand.Parameters.AddWithValue("EditTime", item.EditTime == null ? (object)DBNull.Value : item.EditTime);
					sqlCommand.Parameters.AddWithValue("IdOrderElement", item.IdOrderElement);
					sqlCommand.Parameters.AddWithValue("IdOrdersLineItem", item.IdOrdersLineItem);
					sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_PreviousScheduledQuantity", item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_PreviousScheduledQuantity == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_PreviousScheduledQuantity);
					sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantity", item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantity == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantity);
					sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateQualifier", item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateQualifier == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateQualifier);
					sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateTime", item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateTime == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateTime);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersScheduleLineEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersScheduleLineEntity> items)
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
						query += " INSERT INTO [__EDI_XML_OrdersScheduleLine] ([CreateTime],[EditTime],[IdOrderElement],[IdOrdersLineItem],[Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_PreviousScheduledQuantity],[Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantity],[Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateQualifier],[Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateTime]) VALUES ( "

							+ "@CreateTime" + i + ","
							+ "@EditTime" + i + ","
							+ "@IdOrderElement" + i + ","
							+ "@IdOrdersLineItem" + i + ","
							+ "@Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_PreviousScheduledQuantity" + i + ","
							+ "@Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantity" + i + ","
							+ "@Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateQualifier" + i + ","
							+ "@Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateTime" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("CreateTime" + i, item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
						sqlCommand.Parameters.AddWithValue("EditTime" + i, item.EditTime == null ? (object)DBNull.Value : item.EditTime);
						sqlCommand.Parameters.AddWithValue("IdOrderElement" + i, item.IdOrderElement);
						sqlCommand.Parameters.AddWithValue("IdOrdersLineItem" + i, item.IdOrdersLineItem);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_PreviousScheduledQuantity" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_PreviousScheduledQuantity == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_PreviousScheduledQuantity);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantity" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantity == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantity);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateQualifier" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateQualifier == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateQualifier);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateTime" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateTime == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateTime);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.PRS.XML_OrdersScheduleLineEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__EDI_XML_OrdersScheduleLine] SET [CreateTime]=@CreateTime, [EditTime]=@EditTime, [IdOrderElement]=@IdOrderElement, [IdOrdersLineItem]=@IdOrdersLineItem, [Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_PreviousScheduledQuantity]=@Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_PreviousScheduledQuantity, [Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantity]=@Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantity, [Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateQualifier]=@Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateQualifier, [Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateTime]=@Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateTime WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("CreateTime", item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
				sqlCommand.Parameters.AddWithValue("EditTime", item.EditTime == null ? (object)DBNull.Value : item.EditTime);
				sqlCommand.Parameters.AddWithValue("IdOrderElement", item.IdOrderElement);
				sqlCommand.Parameters.AddWithValue("IdOrdersLineItem", item.IdOrdersLineItem);
				sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_PreviousScheduledQuantity", item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_PreviousScheduledQuantity == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_PreviousScheduledQuantity);
				sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantity", item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantity == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantity);
				sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateQualifier", item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateQualifier == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateQualifier);
				sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateTime", item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateTime == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateTime);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersScheduleLineEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersScheduleLineEntity> items)
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
						query += " UPDATE [__EDI_XML_OrdersScheduleLine] SET "

							+ "[CreateTime]=@CreateTime" + i + ","
							+ "[EditTime]=@EditTime" + i + ","
							+ "[IdOrderElement]=@IdOrderElement" + i + ","
							+ "[IdOrdersLineItem]=@IdOrdersLineItem" + i + ","
							+ "[Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_PreviousScheduledQuantity]=@Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_PreviousScheduledQuantity" + i + ","
							+ "[Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantity]=@Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantity" + i + ","
							+ "[Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateQualifier]=@Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateQualifier" + i + ","
							+ "[Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateTime]=@Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateTime" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("CreateTime" + i, item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
						sqlCommand.Parameters.AddWithValue("EditTime" + i, item.EditTime == null ? (object)DBNull.Value : item.EditTime);
						sqlCommand.Parameters.AddWithValue("IdOrderElement" + i, item.IdOrderElement);
						sqlCommand.Parameters.AddWithValue("IdOrdersLineItem" + i, item.IdOrdersLineItem);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_PreviousScheduledQuantity" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_PreviousScheduledQuantity == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_PreviousScheduledQuantity);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantity" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantity == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantity);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateQualifier" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateQualifier == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateQualifier);
						sqlCommand.Parameters.AddWithValue("Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateTime" + i, item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateTime == null ? (object)DBNull.Value : item.Message_Document_Details_OrdersData_OrdersLineItem_OrdersScheduleLine_ScheduledQuantityDate_DateTime);
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
				string query = "DELETE FROM [__EDI_XML_OrdersScheduleLine] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__EDI_XML_OrdersScheduleLine] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion


		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersScheduleLineEntity> GetByOrderLineIds(List<int> orderLineIds)
		{
			if(orderLineIds == null || orderLineIds.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__EDI_XML_OrdersScheduleLine] WHERE IdOrdersLineItem IN ({string.Join(",", orderLineIds)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.XML_OrdersScheduleLineEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.XML_OrdersScheduleLineEntity>();
			}
		}

		public static Infrastructure.Data.Entities.Tables.PRS.XML_OrdersScheduleLineEntity GetByOrderElementId(int orderElementId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__EDI_XML_OrdersScheduleLine] WHERE [IdOrderElement] = @orderElementId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("orderElementId", orderElementId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.XML_OrdersScheduleLineEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		#endregion
	}
}
