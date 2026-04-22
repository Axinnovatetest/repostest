using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.PRS
{

	public class ArtikelExtensionAccess
	{

		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__PRS_ArtikelExtension] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__PRS_ArtikelExtension]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [__PRS_ArtikelExtension] WHERE [Id] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__PRS_ArtikelExtension] ([ArtikelNr],[Consumption12Months],[CopperCostBasis],[CopperCostBasis150],[CreatorID],[CreatorOrder],[CustomerContactPersonId],[CustomerInquiryNumber],[CustomerName],[DateCreation],[ImageId],[OrderNumber],[OrderValidity],[ProjectTypeId],[QuotationsBased12Months],[Sales12MonthsPerItem],[SOPAppointmentCustomer])  VALUES (@ArtikelNr,@Consumption12Months,@CopperCostBasis,@CopperCostBasis150,@CreatorID,@CreatorOrder,@CustomerContactPersonId,@CustomerInquiryNumber,@CustomerName,@DateCreation,@ImageId,@OrderNumber,@OrderValidity,@ProjectTypeId,@QuotationsBased12Months,@Sales12MonthsPerItem,@SOPAppointmentCustomer); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("Consumption12Months", item.Consumption12Months == null ? (object)DBNull.Value : item.Consumption12Months);
					sqlCommand.Parameters.AddWithValue("CopperCostBasis", item.CopperCostBasis == null ? (object)DBNull.Value : item.CopperCostBasis);
					sqlCommand.Parameters.AddWithValue("CopperCostBasis150", item.CopperCostBasis150 == null ? (object)DBNull.Value : item.CopperCostBasis150);
					sqlCommand.Parameters.AddWithValue("CreatorID", item.CreatorID == null ? (object)DBNull.Value : item.CreatorID);
					sqlCommand.Parameters.AddWithValue("CreatorOrder", item.CreatorOrder == null ? (object)DBNull.Value : item.CreatorOrder);
					sqlCommand.Parameters.AddWithValue("CustomerContactPersonId", item.CustomerContactPersonId == null ? (object)DBNull.Value : item.CustomerContactPersonId);
					sqlCommand.Parameters.AddWithValue("CustomerInquiryNumber", item.CustomerInquiryNumber == null ? (object)DBNull.Value : item.CustomerInquiryNumber);
					sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("DateCreation", item.DateCreation == null ? (object)DBNull.Value : item.DateCreation);
					sqlCommand.Parameters.AddWithValue("ImageId", item.ImageId);
					sqlCommand.Parameters.AddWithValue("OrderNumber", item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
					sqlCommand.Parameters.AddWithValue("OrderValidity", item.OrderValidity == null ? (object)DBNull.Value : item.OrderValidity);
					sqlCommand.Parameters.AddWithValue("ProjectTypeId", item.ProjectTypeId == null ? (object)DBNull.Value : item.ProjectTypeId);
					sqlCommand.Parameters.AddWithValue("QuotationsBased12Months", item.QuotationsBased12Months == null ? (object)DBNull.Value : item.QuotationsBased12Months);
					sqlCommand.Parameters.AddWithValue("Sales12MonthsPerItem", item.Sales12MonthsPerItem == null ? (object)DBNull.Value : item.Sales12MonthsPerItem);
					sqlCommand.Parameters.AddWithValue("SOPAppointmentCustomer", item.SOPAppointmentCustomer == null ? (object)DBNull.Value : item.SOPAppointmentCustomer);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 18; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity> items)
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
						query += " INSERT INTO [__PRS_ArtikelExtension] ([ArtikelNr],[Consumption12Months],[CopperCostBasis],[CopperCostBasis150],[CreatorID],[CreatorOrder],[CustomerContactPersonId],[CustomerInquiryNumber],[CustomerName],[DateCreation],[ImageId],[OrderNumber],[OrderValidity],[ProjectTypeId],[QuotationsBased12Months],[Sales12MonthsPerItem],[SOPAppointmentCustomer]) VALUES ( "

							+ "@ArtikelNr" + i + ","
							+ "@Consumption12Months" + i + ","
							+ "@CopperCostBasis" + i + ","
							+ "@CopperCostBasis150" + i + ","
							+ "@CreatorID" + i + ","
							+ "@CreatorOrder" + i + ","
							+ "@CustomerContactPersonId" + i + ","
							+ "@CustomerInquiryNumber" + i + ","
							+ "@CustomerName" + i + ","
							+ "@DateCreation" + i + ","
							+ "@ImageId" + i + ","
							+ "@OrderNumber" + i + ","
							+ "@OrderValidity" + i + ","
							+ "@ProjectTypeId" + i + ","
							+ "@QuotationsBased12Months" + i + ","
							+ "@Sales12MonthsPerItem" + i + ","
							+ "@SOPAppointmentCustomer" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr);
						sqlCommand.Parameters.AddWithValue("Consumption12Months" + i, item.Consumption12Months == null ? (object)DBNull.Value : item.Consumption12Months);
						sqlCommand.Parameters.AddWithValue("CopperCostBasis" + i, item.CopperCostBasis == null ? (object)DBNull.Value : item.CopperCostBasis);
						sqlCommand.Parameters.AddWithValue("CopperCostBasis150" + i, item.CopperCostBasis150 == null ? (object)DBNull.Value : item.CopperCostBasis150);
						sqlCommand.Parameters.AddWithValue("CreatorID" + i, item.CreatorID == null ? (object)DBNull.Value : item.CreatorID);
						sqlCommand.Parameters.AddWithValue("CreatorOrder" + i, item.CreatorOrder == null ? (object)DBNull.Value : item.CreatorOrder);
						sqlCommand.Parameters.AddWithValue("CustomerContactPersonId" + i, item.CustomerContactPersonId == null ? (object)DBNull.Value : item.CustomerContactPersonId);
						sqlCommand.Parameters.AddWithValue("CustomerInquiryNumber" + i, item.CustomerInquiryNumber == null ? (object)DBNull.Value : item.CustomerInquiryNumber);
						sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
						sqlCommand.Parameters.AddWithValue("DateCreation" + i, item.DateCreation == null ? (object)DBNull.Value : item.DateCreation);
						sqlCommand.Parameters.AddWithValue("ImageId" + i, item.ImageId);
						sqlCommand.Parameters.AddWithValue("OrderNumber" + i, item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
						sqlCommand.Parameters.AddWithValue("OrderValidity" + i, item.OrderValidity == null ? (object)DBNull.Value : item.OrderValidity);
						sqlCommand.Parameters.AddWithValue("ProjectTypeId" + i, item.ProjectTypeId == null ? (object)DBNull.Value : item.ProjectTypeId);
						sqlCommand.Parameters.AddWithValue("QuotationsBased12Months" + i, item.QuotationsBased12Months == null ? (object)DBNull.Value : item.QuotationsBased12Months);
						sqlCommand.Parameters.AddWithValue("Sales12MonthsPerItem" + i, item.Sales12MonthsPerItem == null ? (object)DBNull.Value : item.Sales12MonthsPerItem);
						sqlCommand.Parameters.AddWithValue("SOPAppointmentCustomer" + i, item.SOPAppointmentCustomer == null ? (object)DBNull.Value : item.SOPAppointmentCustomer);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		// REM: DOT NOT Update CreatorID & DateCreation
		public static int Update(Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__PRS_ArtikelExtension] SET [ArtikelNr]=@ArtikelNr, [Consumption12Months]=@Consumption12Months, [CopperCostBasis]=@CopperCostBasis, [CopperCostBasis150]=@CopperCostBasis150, [CreatorOrder]=@CreatorOrder, [CustomerContactPersonId]=@CustomerContactPersonId, [CustomerInquiryNumber]=@CustomerInquiryNumber, [CustomerName]=@CustomerName, [ImageId]=@ImageId, [OrderNumber]=@OrderNumber, [OrderValidity]=@OrderValidity, [ProjectTypeId]=@ProjectTypeId, [QuotationsBased12Months]=@QuotationsBased12Months, [Sales12MonthsPerItem]=@Sales12MonthsPerItem, [SOPAppointmentCustomer]=@SOPAppointmentCustomer WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
				sqlCommand.Parameters.AddWithValue("Consumption12Months", item.Consumption12Months == null ? (object)DBNull.Value : item.Consumption12Months);
				sqlCommand.Parameters.AddWithValue("CopperCostBasis", item.CopperCostBasis == null ? (object)DBNull.Value : item.CopperCostBasis);
				sqlCommand.Parameters.AddWithValue("CopperCostBasis150", item.CopperCostBasis150 == null ? (object)DBNull.Value : item.CopperCostBasis150);
				sqlCommand.Parameters.AddWithValue("CreatorOrder", item.CreatorOrder == null ? (object)DBNull.Value : item.CreatorOrder);
				sqlCommand.Parameters.AddWithValue("CustomerContactPersonId", item.CustomerContactPersonId == null ? (object)DBNull.Value : item.CustomerContactPersonId);
				sqlCommand.Parameters.AddWithValue("CustomerInquiryNumber", item.CustomerInquiryNumber == null ? (object)DBNull.Value : item.CustomerInquiryNumber);
				sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
				sqlCommand.Parameters.AddWithValue("ImageId", item.ImageId);
				sqlCommand.Parameters.AddWithValue("OrderNumber", item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
				sqlCommand.Parameters.AddWithValue("OrderValidity", item.OrderValidity == null ? (object)DBNull.Value : item.OrderValidity);
				sqlCommand.Parameters.AddWithValue("ProjectTypeId", item.ProjectTypeId == null ? (object)DBNull.Value : item.ProjectTypeId);
				sqlCommand.Parameters.AddWithValue("QuotationsBased12Months", item.QuotationsBased12Months == null ? (object)DBNull.Value : item.QuotationsBased12Months);
				sqlCommand.Parameters.AddWithValue("Sales12MonthsPerItem", item.Sales12MonthsPerItem == null ? (object)DBNull.Value : item.Sales12MonthsPerItem);
				sqlCommand.Parameters.AddWithValue("SOPAppointmentCustomer", item.SOPAppointmentCustomer == null ? (object)DBNull.Value : item.SOPAppointmentCustomer);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 18; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity> items)
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
						query += " UPDATE [__PRS_ArtikelExtension] SET "

							+ "[ArtikelNr]=@ArtikelNr" + i + ","
							+ "[Consumption12Months]=@Consumption12Months" + i + ","
							+ "[CopperCostBasis]=@CopperCostBasis" + i + ","
							+ "[CopperCostBasis150]=@CopperCostBasis150" + i + ","
							+ "[CreatorID]=@CreatorID" + i + ","
							+ "[CreatorOrder]=@CreatorOrder" + i + ","
							+ "[CustomerContactPersonId]=@CustomerContactPersonId" + i + ","
							+ "[CustomerInquiryNumber]=@CustomerInquiryNumber" + i + ","
							+ "[CustomerName]=@CustomerName" + i + ","
							+ "[DateCreation]=@DateCreation" + i + ","
							+ "[ImageId]=@ImageId" + i + ","
							+ "[OrderNumber]=@OrderNumber" + i + ","
							+ "[OrderValidity]=@OrderValidity" + i + ","
							+ "[ProjectTypeId]=@ProjectTypeId" + i + ","
							+ "[QuotationsBased12Months]=@QuotationsBased12Months" + i + ","
							+ "[Sales12MonthsPerItem]=@Sales12MonthsPerItem" + i + ","
							+ "[SOPAppointmentCustomer]=@SOPAppointmentCustomer" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr);
						sqlCommand.Parameters.AddWithValue("Consumption12Months" + i, item.Consumption12Months == null ? (object)DBNull.Value : item.Consumption12Months);
						sqlCommand.Parameters.AddWithValue("CopperCostBasis" + i, item.CopperCostBasis == null ? (object)DBNull.Value : item.CopperCostBasis);
						sqlCommand.Parameters.AddWithValue("CopperCostBasis150" + i, item.CopperCostBasis150 == null ? (object)DBNull.Value : item.CopperCostBasis150);
						sqlCommand.Parameters.AddWithValue("CreatorID" + i, item.CreatorID == null ? (object)DBNull.Value : item.CreatorID);
						sqlCommand.Parameters.AddWithValue("CreatorOrder" + i, item.CreatorOrder == null ? (object)DBNull.Value : item.CreatorOrder);
						sqlCommand.Parameters.AddWithValue("CustomerContactPersonId" + i, item.CustomerContactPersonId == null ? (object)DBNull.Value : item.CustomerContactPersonId);
						sqlCommand.Parameters.AddWithValue("CustomerInquiryNumber" + i, item.CustomerInquiryNumber == null ? (object)DBNull.Value : item.CustomerInquiryNumber);
						sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
						sqlCommand.Parameters.AddWithValue("DateCreation" + i, item.DateCreation == null ? (object)DBNull.Value : item.DateCreation);
						sqlCommand.Parameters.AddWithValue("ImageId" + i, item.ImageId);
						sqlCommand.Parameters.AddWithValue("OrderNumber" + i, item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
						sqlCommand.Parameters.AddWithValue("OrderValidity" + i, item.OrderValidity == null ? (object)DBNull.Value : item.OrderValidity);
						sqlCommand.Parameters.AddWithValue("ProjectTypeId" + i, item.ProjectTypeId == null ? (object)DBNull.Value : item.ProjectTypeId);
						sqlCommand.Parameters.AddWithValue("QuotationsBased12Months" + i, item.QuotationsBased12Months == null ? (object)DBNull.Value : item.QuotationsBased12Months);
						sqlCommand.Parameters.AddWithValue("Sales12MonthsPerItem" + i, item.Sales12MonthsPerItem == null ? (object)DBNull.Value : item.Sales12MonthsPerItem);
						sqlCommand.Parameters.AddWithValue("SOPAppointmentCustomer" + i, item.SOPAppointmentCustomer == null ? (object)DBNull.Value : item.SOPAppointmentCustomer);
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
				string query = "DELETE FROM [__PRS_ArtikelExtension] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__PRS_ArtikelExtension] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Transactions 

		public static Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			string query = "SELECT * FROM [__PRS_ArtikelExtension] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__PRS_ArtikelExtension]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);


			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();

				var sqlCommand = new SqlCommand();
				sqlCommand.Connection = connection;
				sqlCommand.Transaction = transaction;


				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				sqlCommand.CommandText = "SELECT * FROM [__PRS_ArtikelExtension] WHERE [Id] IN (" + queryIds + ")";
				DbExecution.Fill(sqlCommand, dataTable);


				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;

			string query = "INSERT INTO [__PRS_ArtikelExtension] ([ArtikelNr],[Consumption12Months],[CopperCostBasis],[CopperCostBasis150],[CreatorID],[CreatorOrder],[CustomerContactPersonId],[CustomerInquiryNumber],[CustomerName],[DateCreation],[ImageId],[OrderNumber],[OrderValidity],[ProjectTypeId],[QuotationsBased12Months],[Sales12MonthsPerItem],[SOPAppointmentCustomer])  VALUES (@ArtikelNr,@Consumption12Months,@CopperCostBasis,@CopperCostBasis150,@CreatorID,@CreatorOrder,@CustomerContactPersonId,@CustomerInquiryNumber,@CustomerName,@DateCreation,@ImageId,@OrderNumber,@OrderValidity,@ProjectTypeId,@QuotationsBased12Months,@Sales12MonthsPerItem,@SOPAppointmentCustomer); ";
			query += "SELECT SCOPE_IDENTITY();";

			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{

				sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
				sqlCommand.Parameters.AddWithValue("Consumption12Months", item.Consumption12Months == null ? (object)DBNull.Value : item.Consumption12Months);
				sqlCommand.Parameters.AddWithValue("CopperCostBasis", item.CopperCostBasis == null ? (object)DBNull.Value : item.CopperCostBasis);
				sqlCommand.Parameters.AddWithValue("CopperCostBasis150", item.CopperCostBasis150 == null ? (object)DBNull.Value : item.CopperCostBasis150);
				sqlCommand.Parameters.AddWithValue("CreatorID", item.CreatorID == null ? (object)DBNull.Value : item.CreatorID);
				sqlCommand.Parameters.AddWithValue("CreatorOrder", item.CreatorOrder == null ? (object)DBNull.Value : item.CreatorOrder);
				sqlCommand.Parameters.AddWithValue("CustomerContactPersonId", item.CustomerContactPersonId == null ? (object)DBNull.Value : item.CustomerContactPersonId);
				sqlCommand.Parameters.AddWithValue("CustomerInquiryNumber", item.CustomerInquiryNumber == null ? (object)DBNull.Value : item.CustomerInquiryNumber);
				sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
				sqlCommand.Parameters.AddWithValue("DateCreation", item.DateCreation == null ? (object)DBNull.Value : item.DateCreation);
				sqlCommand.Parameters.AddWithValue("ImageId", item.ImageId);
				sqlCommand.Parameters.AddWithValue("OrderNumber", item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
				sqlCommand.Parameters.AddWithValue("OrderValidity", item.OrderValidity == null ? (object)DBNull.Value : item.OrderValidity);
				sqlCommand.Parameters.AddWithValue("ProjectTypeId", item.ProjectTypeId == null ? (object)DBNull.Value : item.ProjectTypeId);
				sqlCommand.Parameters.AddWithValue("QuotationsBased12Months", item.QuotationsBased12Months == null ? (object)DBNull.Value : item.QuotationsBased12Months);
				sqlCommand.Parameters.AddWithValue("Sales12MonthsPerItem", item.Sales12MonthsPerItem == null ? (object)DBNull.Value : item.Sales12MonthsPerItem);
				sqlCommand.Parameters.AddWithValue("SOPAppointmentCustomer", item.SOPAppointmentCustomer == null ? (object)DBNull.Value : item.SOPAppointmentCustomer);

				var result = DbExecution.ExecuteScalar(sqlCommand);
				response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}

			return response;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 18; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " INSERT INTO [__PRS_ArtikelExtension] ([ArtikelNr],[Consumption12Months],[CopperCostBasis],[CopperCostBasis150],[CreatorID],[CreatorOrder],[CustomerContactPersonId],[CustomerInquiryNumber],[CustomerName],[DateCreation],[ImageId],[OrderNumber],[OrderValidity],[ProjectTypeId],[QuotationsBased12Months],[Sales12MonthsPerItem],[SOPAppointmentCustomer]) VALUES ( "

						+ "@ArtikelNr" + i + ","
						+ "@Consumption12Months" + i + ","
						+ "@CopperCostBasis" + i + ","
						+ "@CopperCostBasis150" + i + ","
						+ "@CreatorID" + i + ","
						+ "@CreatorOrder" + i + ","
						+ "@CustomerContactPersonId" + i + ","
						+ "@CustomerInquiryNumber" + i + ","
						+ "@CustomerName" + i + ","
						+ "@DateCreation" + i + ","
						+ "@ImageId" + i + ","
						+ "@OrderNumber" + i + ","
						+ "@OrderValidity" + i + ","
						+ "@ProjectTypeId" + i + ","
						+ "@QuotationsBased12Months" + i + ","
						+ "@Sales12MonthsPerItem" + i + ","
						+ "@SOPAppointmentCustomer" + i
						+ "); ";


					sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("Consumption12Months" + i, item.Consumption12Months == null ? (object)DBNull.Value : item.Consumption12Months);
					sqlCommand.Parameters.AddWithValue("CopperCostBasis" + i, item.CopperCostBasis == null ? (object)DBNull.Value : item.CopperCostBasis);
					sqlCommand.Parameters.AddWithValue("CopperCostBasis150" + i, item.CopperCostBasis150 == null ? (object)DBNull.Value : item.CopperCostBasis150);
					sqlCommand.Parameters.AddWithValue("CreatorID" + i, item.CreatorID == null ? (object)DBNull.Value : item.CreatorID);
					sqlCommand.Parameters.AddWithValue("CreatorOrder" + i, item.CreatorOrder == null ? (object)DBNull.Value : item.CreatorOrder);
					sqlCommand.Parameters.AddWithValue("CustomerContactPersonId" + i, item.CustomerContactPersonId == null ? (object)DBNull.Value : item.CustomerContactPersonId);
					sqlCommand.Parameters.AddWithValue("CustomerInquiryNumber" + i, item.CustomerInquiryNumber == null ? (object)DBNull.Value : item.CustomerInquiryNumber);
					sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("DateCreation" + i, item.DateCreation == null ? (object)DBNull.Value : item.DateCreation);
					sqlCommand.Parameters.AddWithValue("ImageId" + i, item.ImageId);
					sqlCommand.Parameters.AddWithValue("OrderNumber" + i, item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
					sqlCommand.Parameters.AddWithValue("OrderValidity" + i, item.OrderValidity == null ? (object)DBNull.Value : item.OrderValidity);
					sqlCommand.Parameters.AddWithValue("ProjectTypeId" + i, item.ProjectTypeId == null ? (object)DBNull.Value : item.ProjectTypeId);
					sqlCommand.Parameters.AddWithValue("QuotationsBased12Months" + i, item.QuotationsBased12Months == null ? (object)DBNull.Value : item.QuotationsBased12Months);
					sqlCommand.Parameters.AddWithValue("Sales12MonthsPerItem" + i, item.Sales12MonthsPerItem == null ? (object)DBNull.Value : item.Sales12MonthsPerItem);
					sqlCommand.Parameters.AddWithValue("SOPAppointmentCustomer" + i, item.SOPAppointmentCustomer == null ? (object)DBNull.Value : item.SOPAppointmentCustomer);
				}

				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);

				return results;
			}

			return -1;
		}

		// REM: DOT NOT Update CreatorID & DateCreation
		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			string query = "UPDATE [__PRS_ArtikelExtension] SET [ArtikelNr]=@ArtikelNr, [Consumption12Months]=@Consumption12Months, [CopperCostBasis]=@CopperCostBasis, [CopperCostBasis150]=@CopperCostBasis150, [CreatorOrder]=@CreatorOrder, [CustomerContactPersonId]=@CustomerContactPersonId, [CustomerInquiryNumber]=@CustomerInquiryNumber, [CustomerName]=@CustomerName, [ImageId]=@ImageId, [OrderNumber]=@OrderNumber, [OrderValidity]=@OrderValidity, [ProjectTypeId]=@ProjectTypeId, [QuotationsBased12Months]=@QuotationsBased12Months, [Sales12MonthsPerItem]=@Sales12MonthsPerItem, [SOPAppointmentCustomer]=@SOPAppointmentCustomer WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
			sqlCommand.Parameters.AddWithValue("Consumption12Months", item.Consumption12Months == null ? (object)DBNull.Value : item.Consumption12Months);
			sqlCommand.Parameters.AddWithValue("CopperCostBasis", item.CopperCostBasis == null ? (object)DBNull.Value : item.CopperCostBasis);
			sqlCommand.Parameters.AddWithValue("CopperCostBasis150", item.CopperCostBasis150 == null ? (object)DBNull.Value : item.CopperCostBasis150);
			sqlCommand.Parameters.AddWithValue("CreatorOrder", item.CreatorOrder == null ? (object)DBNull.Value : item.CreatorOrder);
			sqlCommand.Parameters.AddWithValue("CustomerContactPersonId", item.CustomerContactPersonId == null ? (object)DBNull.Value : item.CustomerContactPersonId);
			sqlCommand.Parameters.AddWithValue("CustomerInquiryNumber", item.CustomerInquiryNumber == null ? (object)DBNull.Value : item.CustomerInquiryNumber);
			sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
			sqlCommand.Parameters.AddWithValue("ImageId", item.ImageId);
			sqlCommand.Parameters.AddWithValue("OrderNumber", item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
			sqlCommand.Parameters.AddWithValue("OrderValidity", item.OrderValidity == null ? (object)DBNull.Value : item.OrderValidity);
			sqlCommand.Parameters.AddWithValue("ProjectTypeId", item.ProjectTypeId == null ? (object)DBNull.Value : item.ProjectTypeId);
			sqlCommand.Parameters.AddWithValue("QuotationsBased12Months", item.QuotationsBased12Months == null ? (object)DBNull.Value : item.QuotationsBased12Months);
			sqlCommand.Parameters.AddWithValue("Sales12MonthsPerItem", item.Sales12MonthsPerItem == null ? (object)DBNull.Value : item.Sales12MonthsPerItem);
			sqlCommand.Parameters.AddWithValue("SOPAppointmentCustomer", item.SOPAppointmentCustomer == null ? (object)DBNull.Value : item.SOPAppointmentCustomer);

			results = DbExecution.ExecuteNonQuery(sqlCommand);

			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 18; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__PRS_ArtikelExtension] SET "

						+ "[ArtikelNr]=@ArtikelNr" + i + ","
						+ "[Consumption12Months]=@Consumption12Months" + i + ","
						+ "[CopperCostBasis]=@CopperCostBasis" + i + ","
						+ "[CopperCostBasis150]=@CopperCostBasis150" + i + ","
						+ "[CreatorID]=@CreatorID" + i + ","
						+ "[CreatorOrder]=@CreatorOrder" + i + ","
						+ "[CustomerContactPersonId]=@CustomerContactPersonId" + i + ","
						+ "[CustomerInquiryNumber]=@CustomerInquiryNumber" + i + ","
						+ "[CustomerName]=@CustomerName" + i + ","
						+ "[DateCreation]=@DateCreation" + i + ","
						+ "[ImageId]=@ImageId" + i + ","
						+ "[OrderNumber]=@OrderNumber" + i + ","
						+ "[OrderValidity]=@OrderValidity" + i + ","
						+ "[ProjectTypeId]=@ProjectTypeId" + i + ","
						+ "[QuotationsBased12Months]=@QuotationsBased12Months" + i + ","
						+ "[Sales12MonthsPerItem]=@Sales12MonthsPerItem" + i + ","
						+ "[SOPAppointmentCustomer]=@SOPAppointmentCustomer" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("Consumption12Months" + i, item.Consumption12Months == null ? (object)DBNull.Value : item.Consumption12Months);
					sqlCommand.Parameters.AddWithValue("CopperCostBasis" + i, item.CopperCostBasis == null ? (object)DBNull.Value : item.CopperCostBasis);
					sqlCommand.Parameters.AddWithValue("CopperCostBasis150" + i, item.CopperCostBasis150 == null ? (object)DBNull.Value : item.CopperCostBasis150);
					sqlCommand.Parameters.AddWithValue("CreatorID" + i, item.CreatorID == null ? (object)DBNull.Value : item.CreatorID);
					sqlCommand.Parameters.AddWithValue("CreatorOrder" + i, item.CreatorOrder == null ? (object)DBNull.Value : item.CreatorOrder);
					sqlCommand.Parameters.AddWithValue("CustomerContactPersonId" + i, item.CustomerContactPersonId == null ? (object)DBNull.Value : item.CustomerContactPersonId);
					sqlCommand.Parameters.AddWithValue("CustomerInquiryNumber" + i, item.CustomerInquiryNumber == null ? (object)DBNull.Value : item.CustomerInquiryNumber);
					sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("DateCreation" + i, item.DateCreation == null ? (object)DBNull.Value : item.DateCreation);
					sqlCommand.Parameters.AddWithValue("ImageId" + i, item.ImageId);
					sqlCommand.Parameters.AddWithValue("OrderNumber" + i, item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
					sqlCommand.Parameters.AddWithValue("OrderValidity" + i, item.OrderValidity == null ? (object)DBNull.Value : item.OrderValidity);
					sqlCommand.Parameters.AddWithValue("ProjectTypeId" + i, item.ProjectTypeId == null ? (object)DBNull.Value : item.ProjectTypeId);
					sqlCommand.Parameters.AddWithValue("QuotationsBased12Months" + i, item.QuotationsBased12Months == null ? (object)DBNull.Value : item.QuotationsBased12Months);
					sqlCommand.Parameters.AddWithValue("Sales12MonthsPerItem" + i, item.Sales12MonthsPerItem == null ? (object)DBNull.Value : item.Sales12MonthsPerItem);
					sqlCommand.Parameters.AddWithValue("SOPAppointmentCustomer" + i, item.SOPAppointmentCustomer == null ? (object)DBNull.Value : item.SOPAppointmentCustomer);
				}

				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);

				return results;
			}

			return -1;
		}
		#endregion Transactions

		#region Custom Methods

		public static Entities.Tables.PRS.ArtikelExtensionEntity GetByArticleNr(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand("SELECT * FROM [__PRS_ArtikelExtension] WHERE [ArtikelNr]=@nr", sqlConnection))
				{
					sqlCommand.Parameters.AddWithValue("nr", nr);
					DbExecution.Fill(sqlCommand, dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.PRS.ArtikelExtensionEntity> GetByArticleNrs(List<int> nrs)
		{
			if(nrs?.Count <= 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity>();
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand($"SELECT * FROM [__PRS_ArtikelExtension] WHERE [ArtikelNr] IN ({string.Join(",", nrs)})", sqlConnection))
				{
					DbExecution.Fill(sqlCommand, dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity>();
			}
		}
		public static int UpdateArticleImage(int artikelNr, int newImageId)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__PRS_ArtikelExtension] SET [ImageId]=@ImageId WHERE [ArtikelNr]=@ArtikelNr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ArtikelNr", artikelNr);
				sqlCommand.Parameters.AddWithValue("ImageId", newImageId);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int EditExtensionData(Entities.Tables.PRS.ArtikelExtensionEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__PRS_ArtikelExtension] SET [ArtikelNr]=@ArtikelNr,"
				  + "[Consumption12Months]=@Consumption12Months, [CopperCostBasis]=@CopperCostBasis,"
				  + "[CopperCostBasis150]=@CopperCostBasis150,"
				  + "[CustomerContactPersonId]=@CustomerContactPersonId, "
				  + "[CustomerInquiryNumber]=@CustomerInquiryNumber,[CustomerName]=@CustomerName, [ImageId]=@ImageId, "
				  + "[OrderNumber]=@OrderNumber,[OrderValidity]=@OrderValidity, "
				  + "[ProjectTypeId]=@ProjectTypeId, [QuotationsBased12Months]=@QuotationsBased12Months,"
				  + "[Sales12MonthsPerItem]=@Sales12MonthsPerItem, "
				  + "[SOPAppointmentCustomer]=@SOPAppointmentCustomer WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
				sqlCommand.Parameters.AddWithValue("Consumption12Months", item.Consumption12Months == null ? (object)DBNull.Value : item.Consumption12Months);
				sqlCommand.Parameters.AddWithValue("CopperCostBasis", item.CopperCostBasis == null ? (object)DBNull.Value : item.CopperCostBasis);
				sqlCommand.Parameters.AddWithValue("CopperCostBasis150", item.CopperCostBasis150 == null ? (object)DBNull.Value : item.CopperCostBasis150);
				sqlCommand.Parameters.AddWithValue("CustomerContactPersonId", item.CustomerContactPersonId == null ? (object)DBNull.Value : item.CustomerContactPersonId);
				sqlCommand.Parameters.AddWithValue("CustomerInquiryNumber", item.CustomerInquiryNumber == null ? (object)DBNull.Value : item.CustomerInquiryNumber);
				sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName);
				sqlCommand.Parameters.AddWithValue("ImageId", item.ImageId);
				sqlCommand.Parameters.AddWithValue("OrderNumber", item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);
				sqlCommand.Parameters.AddWithValue("OrderValidity", item.OrderValidity == null ? (object)DBNull.Value : item.OrderValidity);
				sqlCommand.Parameters.AddWithValue("ProjectTypeId", item.ProjectTypeId == null ? (object)DBNull.Value : item.ProjectTypeId);
				sqlCommand.Parameters.AddWithValue("QuotationsBased12Months", item.QuotationsBased12Months == null ? (object)DBNull.Value : item.QuotationsBased12Months);
				sqlCommand.Parameters.AddWithValue("Sales12MonthsPerItem", item.Sales12MonthsPerItem == null ? (object)DBNull.Value : item.Sales12MonthsPerItem);
				sqlCommand.Parameters.AddWithValue("SOPAppointmentCustomer", item.SOPAppointmentCustomer == null ? (object)DBNull.Value : item.SOPAppointmentCustomer);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static List<Entities.Tables.PRS.ArtikelExtensionEntity> GetByProjectTypeId(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand("SELECT * FROM [__PRS_ArtikelExtension] WHERE [ProjectTypeId]=@id", sqlConnection))
				{
					sqlCommand.Parameters.AddWithValue("id", id);
					DbExecution.Fill(sqlCommand, dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.PRS.ArtikelExtensionEntity> GetByCustomerContactPersonId(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand("SELECT * FROM [__PRS_ArtikelExtension] WHERE [CustomerContactPersonId]=@id", sqlConnection))
				{
					sqlCommand.Parameters.AddWithValue("id", id);
					DbExecution.Fill(sqlCommand, dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static int EditOverview(Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			string query = "UPDATE [__PRS_ArtikelExtension] SET [Consumption12Months]=@Consumption12Months, [OrderNumber]=@OrderNumber WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
			sqlCommand.Parameters.AddWithValue("Consumption12Months", item.Consumption12Months == null ? (object)DBNull.Value : item.Consumption12Months);
			sqlCommand.Parameters.AddWithValue("OrderNumber", item.OrderNumber == null ? (object)DBNull.Value : item.OrderNumber);

			results = DbExecution.ExecuteNonQuery(sqlCommand);

			return results;
		}
		#endregion
	}
}
