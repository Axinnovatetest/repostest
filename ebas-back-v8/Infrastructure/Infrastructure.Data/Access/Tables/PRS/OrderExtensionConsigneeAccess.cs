using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.PRS
{

	public class OrderExtensionConsigneeAccess
	{

		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [EDI_OrderExtensionConsignee] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [EDI_OrderExtensionConsignee]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [EDI_OrderExtensionConsignee] WHERE [Id] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity item)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [EDI_OrderExtensionConsignee] ([City],[ContactFax],[ContactName],[ContatTelephone],[CountryName],[DUNS],[Name],[Name2],[Name3],[OrderId],[OrderType],[OrderElementId],[PartyIdentificationCodeListQualifier],[PostalCode],[PurchasingDepartment],[StorageLocation],[Street],[UnloadingPoint])  VALUES (@City,@ContactFax,@ContactName,@ContatTelephone,@CountryName,@DUNS,@Name,@Name2,@Name3,@OrderId,@OrderType,@OrderElementId,@PartyIdentificationCodeListQualifier,@PostalCode,@PurchasingDepartment,@StorageLocation,@Street,@UnloadingPoint);";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("City", item.City == null ? (object)DBNull.Value : item.City);
					sqlCommand.Parameters.AddWithValue("ContactFax", item.ContactFax == null ? (object)DBNull.Value : item.ContactFax);
					sqlCommand.Parameters.AddWithValue("ContactName", item.ContactName == null ? (object)DBNull.Value : item.ContactName);
					sqlCommand.Parameters.AddWithValue("ContatTelephone", item.ContatTelephone == null ? (object)DBNull.Value : item.ContatTelephone);
					sqlCommand.Parameters.AddWithValue("CountryName", item.CountryName == null ? (object)DBNull.Value : item.CountryName);
					sqlCommand.Parameters.AddWithValue("DUNS", item.DUNS == null ? (object)DBNull.Value : item.DUNS);
					sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? (object)DBNull.Value : item.Name);
					sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
					sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
					sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
					sqlCommand.Parameters.AddWithValue("OrderType", item.OrderType);
					sqlCommand.Parameters.AddWithValue("OrderElementId", item.OrderElementId == null ? (object)DBNull.Value : item.OrderElementId);
					sqlCommand.Parameters.AddWithValue("PartyIdentificationCodeListQualifier", item.PartyIdentificationCodeListQualifier == null ? (object)DBNull.Value : item.PartyIdentificationCodeListQualifier);
					sqlCommand.Parameters.AddWithValue("PostalCode", item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
					sqlCommand.Parameters.AddWithValue("PurchasingDepartment", item.PurchasingDepartment == null ? (object)DBNull.Value : item.PurchasingDepartment);
					sqlCommand.Parameters.AddWithValue("StorageLocation", item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
					sqlCommand.Parameters.AddWithValue("Street", item.Street == null ? (object)DBNull.Value : item.Street);
					sqlCommand.Parameters.AddWithValue("UnloadingPoint", item.UnloadingPoint == null ? (object)DBNull.Value : item.UnloadingPoint);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE / 17; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity> items)
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
						query += " INSERT INTO [EDI_OrderExtensionConsignee] ([City],[ContactFax],[ContactName],[ContatTelephone],[CountryName],[DUNS],[Name],[Name2],[Name3],[OrderId],[OrderId],[OrderElementId],[PartyIdentificationCodeListQualifier],[PostalCode],[PurchasingDepartment],[StorageLocation],[Street],[UnloadingPoint]) VALUES ( "

							+ "@City" + i + ","
							+ "@ContactFax" + i + ","
							+ "@ContactName" + i + ","
							+ "@ContatTelephone" + i + ","
							+ "@CountryName" + i + ","
							+ "@DUNS" + i + ","
							+ "@Name" + i + ","
							+ "@Name2" + i + ","
							+ "@Name3" + i + ","
							+ "@OrderId" + i + ","
							+ "@OrderType" + i + ","
							+ "@OrderElementId" + i + ","
							+ "@PartyIdentificationCodeListQualifier" + i + ","
							+ "@PostalCode" + i + ","
							+ "@PurchasingDepartment" + i + ","
							+ "@StorageLocation" + i + ","
							+ "@Street" + i + ","
							+ "@UnloadingPoint" + i
								+ "); ";


						sqlCommand.Parameters.AddWithValue("City" + i, item.City == null ? (object)DBNull.Value : item.City);
						sqlCommand.Parameters.AddWithValue("ContactFax" + i, item.ContactFax == null ? (object)DBNull.Value : item.ContactFax);
						sqlCommand.Parameters.AddWithValue("ContactName" + i, item.ContactName == null ? (object)DBNull.Value : item.ContactName);
						sqlCommand.Parameters.AddWithValue("ContatTelephone" + i, item.ContatTelephone == null ? (object)DBNull.Value : item.ContatTelephone);
						sqlCommand.Parameters.AddWithValue("CountryName" + i, item.CountryName == null ? (object)DBNull.Value : item.CountryName);
						sqlCommand.Parameters.AddWithValue("DUNS" + i, item.DUNS == null ? (object)DBNull.Value : item.DUNS);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name == null ? (object)DBNull.Value : item.Name);
						sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
						sqlCommand.Parameters.AddWithValue("Name3" + i, item.Name3 == null ? (object)DBNull.Value : item.Name3);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
						sqlCommand.Parameters.AddWithValue("OrderType" + i, item.OrderType);
						sqlCommand.Parameters.AddWithValue("OrderElementId", item.OrderElementId == null ? (object)DBNull.Value : item.OrderElementId);
						sqlCommand.Parameters.AddWithValue("PartyIdentificationCodeListQualifier" + i, item.PartyIdentificationCodeListQualifier == null ? (object)DBNull.Value : item.PartyIdentificationCodeListQualifier);
						sqlCommand.Parameters.AddWithValue("PostalCode" + i, item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
						sqlCommand.Parameters.AddWithValue("PurchasingDepartment" + i, item.PurchasingDepartment == null ? (object)DBNull.Value : item.PurchasingDepartment);
						sqlCommand.Parameters.AddWithValue("StorageLocation" + i, item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
						sqlCommand.Parameters.AddWithValue("Street" + i, item.Street == null ? (object)DBNull.Value : item.Street);
						sqlCommand.Parameters.AddWithValue("UnloadingPoint" + i, item.UnloadingPoint == null ? (object)DBNull.Value : item.UnloadingPoint);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [EDI_OrderExtensionConsignee] SET [City]=@City, [ContactFax]=@ContactFax, [ContactName]=@ContactName, [ContatTelephone]=@ContatTelephone, [CountryName]=@CountryName, [DUNS]=@DUNS, [Name]=@Name, [Name2]=@Name2, [Name3]=@Name3, [OrderId]=@OrderId, [PartyIdentificationCodeListQualifier]=@PartyIdentificationCodeListQualifier, [PostalCode]=@PostalCode, [PurchasingDepartment]=@PurchasingDepartment, [StorageLocation]=@StorageLocation, [Street]=@Street, [UnloadingPoint]=@UnloadingPoint, [OrderType]=@OrderType WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("City", item.City == null ? (object)DBNull.Value : item.City);
				sqlCommand.Parameters.AddWithValue("ContactFax", item.ContactFax == null ? (object)DBNull.Value : item.ContactFax);
				sqlCommand.Parameters.AddWithValue("ContactName", item.ContactName == null ? (object)DBNull.Value : item.ContactName);
				sqlCommand.Parameters.AddWithValue("ContatTelephone", item.ContatTelephone == null ? (object)DBNull.Value : item.ContatTelephone);
				sqlCommand.Parameters.AddWithValue("CountryName", item.CountryName == null ? (object)DBNull.Value : item.CountryName);
				sqlCommand.Parameters.AddWithValue("DUNS", item.DUNS == null ? (object)DBNull.Value : item.DUNS);
				sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? (object)DBNull.Value : item.Name);
				sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
				sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
				sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
				sqlCommand.Parameters.AddWithValue("OrderType", item.OrderType);
				sqlCommand.Parameters.AddWithValue("PartyIdentificationCodeListQualifier", item.PartyIdentificationCodeListQualifier == null ? (object)DBNull.Value : item.PartyIdentificationCodeListQualifier);
				sqlCommand.Parameters.AddWithValue("PostalCode", item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
				sqlCommand.Parameters.AddWithValue("PurchasingDepartment", item.PurchasingDepartment == null ? (object)DBNull.Value : item.PurchasingDepartment);
				sqlCommand.Parameters.AddWithValue("StorageLocation", item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
				sqlCommand.Parameters.AddWithValue("Street", item.Street == null ? (object)DBNull.Value : item.Street);
				sqlCommand.Parameters.AddWithValue("UnloadingPoint", item.UnloadingPoint == null ? (object)DBNull.Value : item.UnloadingPoint);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE / 17; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity> items)
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
						query += " UPDATE [EDI_OrderExtensionConsignee] SET "

								+ "[City]=@City" + i + ","
								+ "[ContactFax]=@ContactFax" + i + ","
								+ "[ContactName]=@ContactName" + i + ","
								+ "[ContatTelephone]=@ContatTelephone" + i + ","
								+ "[CountryName]=@CountryName" + i + ","
								+ "[DUNS]=@DUNS" + i + ","
								+ "[Name]=@Name" + i + ","
								+ "[Name2]=@Name2" + i + ","
								+ "[Name3]=@Name3" + i + ","
								+ "[OrderId]=@OrderId" + i + ","
								+ "[OrderType]=@OrderType" + i + ","
								+ "[OrderElementId]=@OrderElementId" + i + ","
								+ "[PartyIdentificationCodeListQualifier]=@PartyIdentificationCodeListQualifier" + i + ","
								+ "[PostalCode]=@PostalCode" + i + ","
								+ "[PurchasingDepartment]=@PurchasingDepartment" + i + ","
								+ "[StorageLocation]=@StorageLocation" + i + ","
								+ "[Street]=@Street" + i + ","
								+ "[UnloadingPoint]=@UnloadingPoint" + i + " WHERE [Id]=@Id" + i
								+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("City" + i, item.City == null ? (object)DBNull.Value : item.City);
						sqlCommand.Parameters.AddWithValue("ContactFax" + i, item.ContactFax == null ? (object)DBNull.Value : item.ContactFax);
						sqlCommand.Parameters.AddWithValue("ContactName" + i, item.ContactName == null ? (object)DBNull.Value : item.ContactName);
						sqlCommand.Parameters.AddWithValue("ContatTelephone" + i, item.ContatTelephone == null ? (object)DBNull.Value : item.ContatTelephone);
						sqlCommand.Parameters.AddWithValue("CountryName" + i, item.CountryName == null ? (object)DBNull.Value : item.CountryName);
						sqlCommand.Parameters.AddWithValue("DUNS" + i, item.DUNS == null ? (object)DBNull.Value : item.DUNS);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name == null ? (object)DBNull.Value : item.Name);
						sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
						sqlCommand.Parameters.AddWithValue("Name3" + i, item.Name3 == null ? (object)DBNull.Value : item.Name3);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
						sqlCommand.Parameters.AddWithValue("OrderType" + i, item.OrderType);
						sqlCommand.Parameters.AddWithValue("OrderElementId", item.OrderElementId == null ? (object)DBNull.Value : item.OrderElementId);
						sqlCommand.Parameters.AddWithValue("PartyIdentificationCodeListQualifier" + i, item.PartyIdentificationCodeListQualifier == null ? (object)DBNull.Value : item.PartyIdentificationCodeListQualifier);
						sqlCommand.Parameters.AddWithValue("PostalCode" + i, item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
						sqlCommand.Parameters.AddWithValue("PurchasingDepartment" + i, item.PurchasingDepartment == null ? (object)DBNull.Value : item.PurchasingDepartment);
						sqlCommand.Parameters.AddWithValue("StorageLocation" + i, item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
						sqlCommand.Parameters.AddWithValue("Street" + i, item.Street == null ? (object)DBNull.Value : item.Street);
						sqlCommand.Parameters.AddWithValue("UnloadingPoint" + i, item.UnloadingPoint == null ? (object)DBNull.Value : item.UnloadingPoint);
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
				string query = "DELETE FROM [EDI_OrderExtensionConsignee] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [EDI_OrderExtensionConsignee] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity item, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			int response = -1;
			string query = "INSERT INTO [EDI_OrderExtensionConsignee] ([City],[ContactFax],[ContactName],[ContatTelephone],[CountryName],[DUNS],[Name],[Name2],[Name3],[OrderId],[OrderType],[OrderElementId],[PartyIdentificationCodeListQualifier],[PostalCode],[PurchasingDepartment],[StorageLocation],[Street],[UnloadingPoint])  VALUES (@City,@ContactFax,@ContactName,@ContatTelephone,@CountryName,@DUNS,@Name,@Name2,@Name3,@OrderId,@OrderType,@OrderElementId,@PartyIdentificationCodeListQualifier,@PostalCode,@PurchasingDepartment,@StorageLocation,@Street,@UnloadingPoint);";
			query += "SELECT SCOPE_IDENTITY();";

			using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
			{

				sqlCommand.Parameters.AddWithValue("City", item.City == null ? (object)DBNull.Value : item.City);
				sqlCommand.Parameters.AddWithValue("ContactFax", item.ContactFax == null ? (object)DBNull.Value : item.ContactFax);
				sqlCommand.Parameters.AddWithValue("ContactName", item.ContactName == null ? (object)DBNull.Value : item.ContactName);
				sqlCommand.Parameters.AddWithValue("ContatTelephone", item.ContatTelephone == null ? (object)DBNull.Value : item.ContatTelephone);
				sqlCommand.Parameters.AddWithValue("CountryName", item.CountryName == null ? (object)DBNull.Value : item.CountryName);
				sqlCommand.Parameters.AddWithValue("DUNS", item.DUNS == null ? (object)DBNull.Value : item.DUNS);
				sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? (object)DBNull.Value : item.Name);
				sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
				sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
				sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
				sqlCommand.Parameters.AddWithValue("OrderType", item.OrderType);
				sqlCommand.Parameters.AddWithValue("OrderElementId", item.OrderElementId == null ? (object)DBNull.Value : item.OrderElementId);
				sqlCommand.Parameters.AddWithValue("PartyIdentificationCodeListQualifier", item.PartyIdentificationCodeListQualifier == null ? (object)DBNull.Value : item.PartyIdentificationCodeListQualifier);
				sqlCommand.Parameters.AddWithValue("PostalCode", item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
				sqlCommand.Parameters.AddWithValue("PurchasingDepartment", item.PurchasingDepartment == null ? (object)DBNull.Value : item.PurchasingDepartment);
				sqlCommand.Parameters.AddWithValue("StorageLocation", item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
				sqlCommand.Parameters.AddWithValue("Street", item.Street == null ? (object)DBNull.Value : item.Street);
				sqlCommand.Parameters.AddWithValue("UnloadingPoint", item.UnloadingPoint == null ? (object)DBNull.Value : item.UnloadingPoint);

				var result = DbExecution.ExecuteScalar(sqlCommand);
				response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}

			return response;
		}
		public static List<Entities.Tables.PRS.OrderExtensionConsigneeEntity> GetByOrderType(int orderId, int orderType)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [EDI_OrderExtensionConsignee] WHERE [OrderId]=@orderId and [orderType]=@orderType";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("orderId", orderId);
				sqlCommand.Parameters.AddWithValue("orderType", orderType);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				var list = new List<Entities.Tables.PRS.OrderExtensionConsigneeEntity>();
				foreach(DataRow dataRow in dataTable.Rows)
				{
					list.Add(new Entities.Tables.PRS.OrderExtensionConsigneeEntity(dataRow));
				}
				return list;
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.PRS.OrderExtensionConsigneeEntity> GetByOrderType(int orderId, int orderType, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();
			string query = "SELECT * FROM [EDI_OrderExtensionConsignee] WHERE [OrderId]=@orderId and [orderType]=@orderType";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("orderId", orderId);
			sqlCommand.Parameters.AddWithValue("orderType", orderType);

			DbExecution.Fill(sqlCommand, dataTable);


			if(dataTable.Rows.Count > 0)
			{
				var list = new List<Entities.Tables.PRS.OrderExtensionConsigneeEntity>();
				foreach(DataRow dataRow in dataTable.Rows)
				{
					list.Add(new Entities.Tables.PRS.OrderExtensionConsigneeEntity(dataRow));
				}
				return list;
			}
			else
			{
				return null;
			}
		}
		public static Entities.Tables.PRS.OrderExtensionConsigneeEntity GetByOrderType(int orderId, int orderType, int? elementId)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [EDI_OrderExtensionConsignee] WHERE [OrderId]=@orderId and [orderType]=@orderType AND [OrderElementId] is NULL";
				if(elementId != null)
				{
					query = "SELECT * FROM [EDI_OrderExtensionConsignee] WHERE [OrderId]=@orderId and [orderType]=@orderType AND [OrderElementId]=@elementId";
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("orderId", orderId);
				sqlCommand.Parameters.AddWithValue("orderType", orderType);
				if(elementId != null)
				{
					sqlCommand.Parameters.AddWithValue("elementId", elementId);
				}

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Entities.Tables.PRS.OrderExtensionConsigneeEntity GetByOrderType(int orderId, int orderType, int? elementId, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [EDI_OrderExtensionConsignee] WHERE [OrderId]=@orderId and [orderType]=@orderType AND [OrderElementId] is NULL";
			if(elementId != null)
			{
				query = "SELECT * FROM [EDI_OrderExtensionConsignee] WHERE [OrderId]=@orderId and [orderType]=@orderType AND [OrderElementId]=@elementId";
			}

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("orderId", orderId);
			sqlCommand.Parameters.AddWithValue("orderType", orderType);
			if(elementId != null)
			{
				sqlCommand.Parameters.AddWithValue("elementId", elementId);
			}

			DbExecution.Fill(sqlCommand, dataTable);


			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.PRS.OrderExtensionConsigneeEntity> GetByOrdersType(List<int> ordersIds, int orderType)
		{
			if(ordersIds != null && ordersIds.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				var response = new List<Entities.Tables.PRS.OrderExtensionConsigneeEntity>();
				if(ordersIds.Count <= maxQueryNumber)
				{
					response = getOrdersIds(ordersIds, orderType);
				}
				else
				{
					int batchNumber = ordersIds.Count / maxQueryNumber;
					response = new List<Entities.Tables.PRS.OrderExtensionConsigneeEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						response.AddRange(getOrdersIds(ordersIds.GetRange(i * maxQueryNumber, maxQueryNumber), orderType));
					}
					response.AddRange(getOrdersIds(ordersIds.GetRange(batchNumber * maxQueryNumber, ordersIds.Count - batchNumber * maxQueryNumber), orderType));
				}
				return response;
			}
			return new List<Entities.Tables.PRS.OrderExtensionConsigneeEntity>();
		}
		private static List<Entities.Tables.PRS.OrderExtensionConsigneeEntity> getOrdersIds(List<int> ordersIds, int orderType)
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
						sqlCommand.CommandText = "SELECT * FROM [EDI_OrderExtensionConsignee] WHERE [OrderId] IN (" + queryIds + ")";
					}
					else
					{
						sqlCommand.CommandText = $"SELECT * FROM [EDI_OrderExtensionConsignee] WHERE [OrderType]=@orderType and [OrderId] IN (" + queryIds + ")";
						sqlCommand.Parameters.AddWithValue("orderType", orderType);
					}

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Entities.Tables.PRS.OrderExtensionConsigneeEntity>();
				}
			}
			return new List<Entities.Tables.PRS.OrderExtensionConsigneeEntity>();
		}
		public static int Update(Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity item, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			string query = "UPDATE [EDI_OrderExtensionConsignee] SET [City]=@City, [ContactFax]=@ContactFax, [ContactName]=@ContactName, [ContatTelephone]=@ContatTelephone, [CountryName]=@CountryName, [DUNS]=@DUNS, [Name]=@Name, [Name2]=@Name2, [Name3]=@Name3, [OrderId]=@OrderId, [PartyIdentificationCodeListQualifier]=@PartyIdentificationCodeListQualifier, [PostalCode]=@PostalCode, [PurchasingDepartment]=@PurchasingDepartment, [StorageLocation]=@StorageLocation, [Street]=@Street, [UnloadingPoint]=@UnloadingPoint, [OrderType]=@OrderType WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("City", item.City == null ? (object)DBNull.Value : item.City);
			sqlCommand.Parameters.AddWithValue("ContactFax", item.ContactFax == null ? (object)DBNull.Value : item.ContactFax);
			sqlCommand.Parameters.AddWithValue("ContactName", item.ContactName == null ? (object)DBNull.Value : item.ContactName);
			sqlCommand.Parameters.AddWithValue("ContatTelephone", item.ContatTelephone == null ? (object)DBNull.Value : item.ContatTelephone);
			sqlCommand.Parameters.AddWithValue("CountryName", item.CountryName == null ? (object)DBNull.Value : item.CountryName);
			sqlCommand.Parameters.AddWithValue("DUNS", item.DUNS == null ? (object)DBNull.Value : item.DUNS);
			sqlCommand.Parameters.AddWithValue("Name", item.Name == null ? (object)DBNull.Value : item.Name);
			sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
			sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
			sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
			sqlCommand.Parameters.AddWithValue("OrderType", item.OrderType);
			sqlCommand.Parameters.AddWithValue("PartyIdentificationCodeListQualifier", item.PartyIdentificationCodeListQualifier == null ? (object)DBNull.Value : item.PartyIdentificationCodeListQualifier);
			sqlCommand.Parameters.AddWithValue("PostalCode", item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
			sqlCommand.Parameters.AddWithValue("PurchasingDepartment", item.PurchasingDepartment == null ? (object)DBNull.Value : item.PurchasingDepartment);
			sqlCommand.Parameters.AddWithValue("StorageLocation", item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
			sqlCommand.Parameters.AddWithValue("Street", item.Street == null ? (object)DBNull.Value : item.Street);
			sqlCommand.Parameters.AddWithValue("UnloadingPoint", item.UnloadingPoint == null ? (object)DBNull.Value : item.UnloadingPoint);

			return DbExecution.ExecuteNonQuery(sqlCommand);
		}

		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
