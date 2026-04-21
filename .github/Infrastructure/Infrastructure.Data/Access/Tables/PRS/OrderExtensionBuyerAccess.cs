using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.PRS
{

	public class OrderExtensionBuyerAccess
	{

		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.OrderExtensionBuyerEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [EDI_OrderExtensionBuyer] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.OrderExtensionBuyerEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionBuyerEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [EDI_OrderExtensionBuyer]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionBuyerEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionBuyerEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionBuyerEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionBuyerEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionBuyerEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionBuyerEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
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

					sqlCommand.CommandText = "SELECT * FROM [EDI_OrderExtensionBuyer] WHERE [Id] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionBuyerEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionBuyerEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.OrderExtensionBuyerEntity item)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [EDI_OrderExtensionBuyer] ([City],[ContactFax],[ContactName],[ContactTelephone],[CountryName],[DUNS],[Name],[Name2],[Name3],[OrderId],[PartyIdentification],[PartyIdentificationCodeListQualifier],[PostalCode],[PurchasingDepartment],[Street], [OrderType])  VALUES (@City,@ContactFax,@ContactName,@ContactTelephone,@CountryName,@DUNS,@Name,@Name2,@Name3,@OrderId,@PartyIdentification,@PartyIdentificationCodeListQualifier,@PostalCode,@PurchasingDepartment,@Street,@OrderType);";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("City", item.City == null ? (object)DBNull.Value : item.City);
					sqlCommand.Parameters.AddWithValue("ContactFax", item.ContactFax == null ? (object)DBNull.Value : item.ContactFax);
					sqlCommand.Parameters.AddWithValue("ContactName", item.ContactName == null ? (object)DBNull.Value : item.ContactName);
					sqlCommand.Parameters.AddWithValue("ContactTelephone", item.ContactTelephone == null ? (object)DBNull.Value : item.ContactTelephone);
					sqlCommand.Parameters.AddWithValue("CountryName", item.CountryName == null ? (object)DBNull.Value : item.CountryName);
					sqlCommand.Parameters.AddWithValue("DUNS", item.DUNS == null ? (object)DBNull.Value : item.DUNS);
					sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? (object)DBNull.Value : item.Name);
					sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
					sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
					sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
					sqlCommand.Parameters.AddWithValue("OrderType", item.OrderType);
					sqlCommand.Parameters.AddWithValue("PartyIdentification", item.PartyIdentification == null ? (object)DBNull.Value : item.PartyIdentification);
					sqlCommand.Parameters.AddWithValue("PartyIdentificationCodeListQualifier", item.PartyIdentificationCodeListQualifier == null ? (object)DBNull.Value : item.PartyIdentificationCodeListQualifier);
					sqlCommand.Parameters.AddWithValue("PostalCode", item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
					sqlCommand.Parameters.AddWithValue("PurchasingDepartment", item.PurchasingDepartment == null ? (object)DBNull.Value : item.PurchasingDepartment);
					sqlCommand.Parameters.AddWithValue("Street", item.Street == null ? (object)DBNull.Value : item.Street);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionBuyerEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE / 16; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionBuyerEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [EDI_OrderExtensionBuyer] ([City],[ContactFax],[ContactName],[ContactTelephone],[CountryName],[DUNS],[Name],[Name2],[Name3],[OrderId],[OrderType],[PartyIdentification],[PartyIdentificationCodeListQualifier],[PostalCode],[PurchasingDepartment],[Street]) VALUES ( "

							+ "@City" + i + ","
							+ "@ContactFax" + i + ","
							+ "@ContactName" + i + ","
							+ "@ContactTelephone" + i + ","
							+ "@CountryName" + i + ","
							+ "@DUNS" + i + ","
							+ "@Name" + i + ","
							+ "@Name2" + i + ","
							+ "@Name3" + i + ","
							+ "@OrderId" + i + ","
							+ "@OrderType" + i + ","
							+ "@PartyIdentification" + i + ","
							+ "@PartyIdentificationCodeListQualifier" + i + ","
							+ "@PostalCode" + i + ","
							+ "@PurchasingDepartment" + i + ","
							+ "@Street" + i
								+ "); ";


						sqlCommand.Parameters.AddWithValue("City" + i, item.City == null ? (object)DBNull.Value : item.City);
						sqlCommand.Parameters.AddWithValue("ContactFax" + i, item.ContactFax == null ? (object)DBNull.Value : item.ContactFax);
						sqlCommand.Parameters.AddWithValue("ContactName" + i, item.ContactName == null ? (object)DBNull.Value : item.ContactName);
						sqlCommand.Parameters.AddWithValue("ContactTelephone" + i, item.ContactTelephone == null ? (object)DBNull.Value : item.ContactTelephone);
						sqlCommand.Parameters.AddWithValue("CountryName" + i, item.CountryName == null ? (object)DBNull.Value : item.CountryName);
						sqlCommand.Parameters.AddWithValue("DUNS" + i, item.DUNS == null ? (object)DBNull.Value : item.DUNS);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name == null ? (object)DBNull.Value : item.Name);
						sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
						sqlCommand.Parameters.AddWithValue("Name3" + i, item.Name3 == null ? (object)DBNull.Value : item.Name3);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
						sqlCommand.Parameters.AddWithValue("OrderType" + i, item.OrderType);
						sqlCommand.Parameters.AddWithValue("PartyIdentification" + i, item.PartyIdentification == null ? (object)DBNull.Value : item.PartyIdentification);
						sqlCommand.Parameters.AddWithValue("PartyIdentificationCodeListQualifier" + i, item.PartyIdentificationCodeListQualifier == null ? (object)DBNull.Value : item.PartyIdentificationCodeListQualifier);
						sqlCommand.Parameters.AddWithValue("PostalCode" + i, item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
						sqlCommand.Parameters.AddWithValue("PurchasingDepartment" + i, item.PurchasingDepartment == null ? (object)DBNull.Value : item.PurchasingDepartment);
						sqlCommand.Parameters.AddWithValue("Street" + i, item.Street == null ? (object)DBNull.Value : item.Street);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.PRS.OrderExtensionBuyerEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [EDI_OrderExtensionBuyer] SET [City]=@City, [ContactFax]=@ContactFax, [ContactName]=@ContactName, [ContactTelephone]=@ContactTelephone, [CountryName]=@CountryName, [DUNS]=@DUNS, [Name]=@Name, [Name2]=@Name2, [Name3]=@Name3, [OrderId]=@OrderId, [PartyIdentification]=@PartyIdentification, [PartyIdentificationCodeListQualifier]=@PartyIdentificationCodeListQualifier, [PostalCode]=@PostalCode, [PurchasingDepartment]=@PurchasingDepartment, [Street]=@Street, [OrderType]=@OrderType WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("City", item.City == null ? (object)DBNull.Value : item.City);
				sqlCommand.Parameters.AddWithValue("ContactFax", item.ContactFax == null ? (object)DBNull.Value : item.ContactFax);
				sqlCommand.Parameters.AddWithValue("ContactName", item.ContactName == null ? (object)DBNull.Value : item.ContactName);
				sqlCommand.Parameters.AddWithValue("ContactTelephone", item.ContactTelephone == null ? (object)DBNull.Value : item.ContactTelephone);
				sqlCommand.Parameters.AddWithValue("CountryName", item.CountryName == null ? (object)DBNull.Value : item.CountryName);
				sqlCommand.Parameters.AddWithValue("DUNS", item.DUNS == null ? (object)DBNull.Value : item.DUNS);
				sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? (object)DBNull.Value : item.Name);
				sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
				sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
				sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
				sqlCommand.Parameters.AddWithValue("OrderType", item.OrderType);
				sqlCommand.Parameters.AddWithValue("PartyIdentification", item.PartyIdentification == null ? (object)DBNull.Value : item.PartyIdentification);
				sqlCommand.Parameters.AddWithValue("PartyIdentificationCodeListQualifier", item.PartyIdentificationCodeListQualifier == null ? (object)DBNull.Value : item.PartyIdentificationCodeListQualifier);
				sqlCommand.Parameters.AddWithValue("PostalCode", item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
				sqlCommand.Parameters.AddWithValue("PurchasingDepartment", item.PurchasingDepartment == null ? (object)DBNull.Value : item.PurchasingDepartment);
				sqlCommand.Parameters.AddWithValue("Street", item.Street == null ? (object)DBNull.Value : item.Street);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionBuyerEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE / 16; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionBuyerEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [EDI_OrderExtensionBuyer] SET "

								+ "[City]=@City" + i + ","
								+ "[ContactFax]=@ContactFax" + i + ","
								+ "[ContactName]=@ContactName" + i + ","
								+ "[ContactTelephone]=@ContactTelephone" + i + ","
								+ "[CountryName]=@CountryName" + i + ","
								+ "[DUNS]=@DUNS" + i + ","
								+ "[Name]=@Name" + i + ","
								+ "[Name2]=@Name2" + i + ","
								+ "[Name3]=@Name3" + i + ","
								+ "[OrderId]=@OrderId" + i + ","
								+ "[OrderType]=@OrderType" + i + ","
								+ "[PartyIdentification]=@PartyIdentification" + i + ","
								+ "[PartyIdentificationCodeListQualifier]=@PartyIdentificationCodeListQualifier" + i + ","
								+ "[PostalCode]=@PostalCode" + i + ","
								+ "[PurchasingDepartment]=@PurchasingDepartment" + i + ","
								+ "[Street]=@Street" + i + " WHERE [Id]=@Id" + i
								+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("City" + i, item.City == null ? (object)DBNull.Value : item.City);
						sqlCommand.Parameters.AddWithValue("ContactFax" + i, item.ContactFax == null ? (object)DBNull.Value : item.ContactFax);
						sqlCommand.Parameters.AddWithValue("ContactName" + i, item.ContactName == null ? (object)DBNull.Value : item.ContactName);
						sqlCommand.Parameters.AddWithValue("ContactTelephone" + i, item.ContactTelephone == null ? (object)DBNull.Value : item.ContactTelephone);
						sqlCommand.Parameters.AddWithValue("CountryName" + i, item.CountryName == null ? (object)DBNull.Value : item.CountryName);
						sqlCommand.Parameters.AddWithValue("DUNS" + i, item.DUNS == null ? (object)DBNull.Value : item.DUNS);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name == null ? (object)DBNull.Value : item.Name);
						sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
						sqlCommand.Parameters.AddWithValue("Name3" + i, item.Name3 == null ? (object)DBNull.Value : item.Name3);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
						sqlCommand.Parameters.AddWithValue("OrderType" + i, item.OrderType);
						sqlCommand.Parameters.AddWithValue("PartyIdentification" + i, item.PartyIdentification == null ? (object)DBNull.Value : item.PartyIdentification);
						sqlCommand.Parameters.AddWithValue("PartyIdentificationCodeListQualifier" + i, item.PartyIdentificationCodeListQualifier == null ? (object)DBNull.Value : item.PartyIdentificationCodeListQualifier);
						sqlCommand.Parameters.AddWithValue("PostalCode" + i, item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
						sqlCommand.Parameters.AddWithValue("PurchasingDepartment" + i, item.PurchasingDepartment == null ? (object)DBNull.Value : item.PurchasingDepartment);
						sqlCommand.Parameters.AddWithValue("Street" + i, item.Street == null ? (object)DBNull.Value : item.Street);
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
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [EDI_OrderExtensionBuyer] WHERE [Id]=@Id";
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
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE;
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
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
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

					string query = "DELETE FROM [EDI_OrderExtensionBuyer] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.OrderExtensionBuyerEntity item, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			int response = -1;
			string query = "INSERT INTO [EDI_OrderExtensionBuyer] ([City],[ContactFax],[ContactName],[ContactTelephone],[CountryName],[DUNS],[Name],[Name2],[Name3],[OrderId],[PartyIdentification],[PartyIdentificationCodeListQualifier],[PostalCode],[PurchasingDepartment],[Street], [OrderType])  VALUES (@City,@ContactFax,@ContactName,@ContactTelephone,@CountryName,@DUNS,@Name,@Name2,@Name3,@OrderId,@PartyIdentification,@PartyIdentificationCodeListQualifier,@PostalCode,@PurchasingDepartment,@Street,@OrderType);";
			query += "SELECT SCOPE_IDENTITY();";

			using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
			{

				sqlCommand.Parameters.AddWithValue("City", item.City == null ? (object)DBNull.Value : item.City);
				sqlCommand.Parameters.AddWithValue("ContactFax", item.ContactFax == null ? (object)DBNull.Value : item.ContactFax);
				sqlCommand.Parameters.AddWithValue("ContactName", item.ContactName == null ? (object)DBNull.Value : item.ContactName);
				sqlCommand.Parameters.AddWithValue("ContactTelephone", item.ContactTelephone == null ? (object)DBNull.Value : item.ContactTelephone);
				sqlCommand.Parameters.AddWithValue("CountryName", item.CountryName == null ? (object)DBNull.Value : item.CountryName);
				sqlCommand.Parameters.AddWithValue("DUNS", item.DUNS == null ? (object)DBNull.Value : item.DUNS);
				sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? (object)DBNull.Value : item.Name);
				sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
				sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
				sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
				sqlCommand.Parameters.AddWithValue("OrderType", item.OrderType);
				sqlCommand.Parameters.AddWithValue("PartyIdentification", item.PartyIdentification == null ? (object)DBNull.Value : item.PartyIdentification);
				sqlCommand.Parameters.AddWithValue("PartyIdentificationCodeListQualifier", item.PartyIdentificationCodeListQualifier == null ? (object)DBNull.Value : item.PartyIdentificationCodeListQualifier);
				sqlCommand.Parameters.AddWithValue("PostalCode", item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
				sqlCommand.Parameters.AddWithValue("PurchasingDepartment", item.PurchasingDepartment == null ? (object)DBNull.Value : item.PurchasingDepartment);
				sqlCommand.Parameters.AddWithValue("Street", item.Street == null ? (object)DBNull.Value : item.Street);

				var result = DbExecution.ExecuteScalar(sqlCommand);
				response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}

			return response;
		}
		public static Entities.Tables.PRS.OrderExtensionBuyerEntity GetByOrderType(int orderId, int orderType)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [EDI_OrderExtensionBuyer] WHERE [OrderId]=@orderId and [OrderType]=@orderType";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("orderId", orderId);
				sqlCommand.Parameters.AddWithValue("orderType", orderType);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.PRS.OrderExtensionBuyerEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Entities.Tables.PRS.OrderExtensionBuyerEntity GetByOrderType(int orderId, int orderType, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [EDI_OrderExtensionBuyer] WHERE [OrderId]=@orderId and [OrderType]=@orderType";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("orderId", orderId);
			sqlCommand.Parameters.AddWithValue("orderType", orderType);

			DbExecution.Fill(sqlCommand, dataTable);
			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.PRS.OrderExtensionBuyerEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.PRS.OrderExtensionBuyerEntity> GetByOrdersType(List<int> ordersIds, int orderType)
		{
			if(ordersIds != null && ordersIds.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				var response = new List<Entities.Tables.PRS.OrderExtensionBuyerEntity>();
				if(ordersIds.Count <= maxQueryNumber)
				{
					response = getOrdersIds(ordersIds, orderType);
				}
				else
				{
					int batchNumber = ordersIds.Count / maxQueryNumber;
					response = new List<Entities.Tables.PRS.OrderExtensionBuyerEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						response.AddRange(getOrdersIds(ordersIds.GetRange(i * maxQueryNumber, maxQueryNumber), orderType));
					}
					response.AddRange(getOrdersIds(ordersIds.GetRange(batchNumber * maxQueryNumber, ordersIds.Count - batchNumber * maxQueryNumber), orderType));
				}
				return response;
			}
			return new List<Entities.Tables.PRS.OrderExtensionBuyerEntity>();
		}
		private static List<Entities.Tables.PRS.OrderExtensionBuyerEntity> getOrdersIds(List<int> ordersIds, int orderType)
		{
			if(ordersIds != null && ordersIds.Count > 0)
			{
				var dataTable = new DataTable();

				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < ordersIds.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ordersIds[i]);
					}
					queryIds = queryIds.TrimEnd(',');
					if(orderType == -1)
					{
						sqlCommand.CommandText = "SELECT * FROM [EDI_OrderExtensionBuyer] WHERE [OrderId] IN (" + queryIds + ")";
					}
					else
					{
						sqlCommand.Parameters.AddWithValue("orderType", orderType);
						sqlCommand.CommandText = "SELECT * FROM [EDI_OrderExtensionBuyer] WHERE [OrderType]=@orderType and [OrderId] IN (" + queryIds + ")";
					}

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Entities.Tables.PRS.OrderExtensionBuyerEntity>();
				}
			}
			return new List<Entities.Tables.PRS.OrderExtensionBuyerEntity>();
		}

		#endregion

		#region Helpers
		public static int Update(Infrastructure.Data.Entities.Tables.PRS.OrderExtensionBuyerEntity item, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			string query = "UPDATE [EDI_OrderExtensionBuyer] SET [City]=@City, [ContactFax]=@ContactFax, [ContactName]=@ContactName, [ContactTelephone]=@ContactTelephone, [CountryName]=@CountryName, [DUNS]=@DUNS, [Name]=@Name, [Name2]=@Name2, [Name3]=@Name3, [OrderId]=@OrderId, [PartyIdentification]=@PartyIdentification, [PartyIdentificationCodeListQualifier]=@PartyIdentificationCodeListQualifier, [PostalCode]=@PostalCode, [PurchasingDepartment]=@PurchasingDepartment, [Street]=@Street, [OrderType]=@OrderType WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("City", item.City == null ? (object)DBNull.Value : item.City);
			sqlCommand.Parameters.AddWithValue("ContactFax", item.ContactFax == null ? (object)DBNull.Value : item.ContactFax);
			sqlCommand.Parameters.AddWithValue("ContactName", item.ContactName == null ? (object)DBNull.Value : item.ContactName);
			sqlCommand.Parameters.AddWithValue("ContactTelephone", item.ContactTelephone == null ? (object)DBNull.Value : item.ContactTelephone);
			sqlCommand.Parameters.AddWithValue("CountryName", item.CountryName == null ? (object)DBNull.Value : item.CountryName);
			sqlCommand.Parameters.AddWithValue("DUNS", item.DUNS == null ? (object)DBNull.Value : item.DUNS);
			sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? (object)DBNull.Value : item.Name);
			sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
			sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
			sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
			sqlCommand.Parameters.AddWithValue("OrderType", item.OrderType);
			sqlCommand.Parameters.AddWithValue("PartyIdentification", item.PartyIdentification == null ? (object)DBNull.Value : item.PartyIdentification);
			sqlCommand.Parameters.AddWithValue("PartyIdentificationCodeListQualifier", item.PartyIdentificationCodeListQualifier == null ? (object)DBNull.Value : item.PartyIdentificationCodeListQualifier);
			sqlCommand.Parameters.AddWithValue("PostalCode", item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
			sqlCommand.Parameters.AddWithValue("PurchasingDepartment", item.PurchasingDepartment == null ? (object)DBNull.Value : item.PurchasingDepartment);
			sqlCommand.Parameters.AddWithValue("Street", item.Street == null ? (object)DBNull.Value : item.Street);

			return DbExecution.ExecuteNonQuery(sqlCommand);
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionBuyerEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionBuyerEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.PRS.OrderExtensionBuyerEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
