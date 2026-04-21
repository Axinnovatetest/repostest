using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.PRS
{
	public class OrderChangeAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity Get(int id)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__EDI_OrderChange] WHERE [Id]=@Id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity> Get()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__EDI_OrderChange]";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				var results = new List<Entities.Tables.PRS.OrderChangeEntity>();
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [__EDI_OrderChange] WHERE [Id] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__EDI_OrderChange] ([ActionTime],[ActionUserId],[ActionUserName],[ConsigneeCity],[ConsigneeContactFax],[ConsigneeContactName],[ConsigneeContactTelephone],[ConsigneeCountryName],[ConsigneeName],[ConsigneeName2],[ConsigneeName3],[ConsigneePartyIdentification],[ConsigneePartyIdentificationCodeListQualifier],[ConsigneePostalCode],[ConsigneePurchasingDepartment],[ConsigneeStorageLocation],[ConsigneeStreet],[ConsigneeStreetPostalCode],[ConsigneeUnloadingPoint],[CreationTime],[CustomerCity],[CustomerContactFax],[CustomerContactName],[CustomerContactTelephone],[CustomerCountryName],[CustomerName],[CustomerName2],[CustomerName3],[CustomerPartyIdentification],[CustomerPartyIdentificationCodeListQualifier],[CustomerPostalCode],[CustomerPurchasingDepartment],[CustomerStreet],[CustomerStreetCityPostalCode],[CustomerStreetPostalCode],[DocumentName],[DocumentNumber],[Duns],[GlobalStatus],[MessageReferenceNumber],[Notes],[OrderId],[Reference])  VALUES (@ActionTime,@ActionUserId,@ActionUserName,@ConsigneeCity,@ConsigneeContactFax,@ConsigneeContactName,@ConsigneeContactTelephone,@ConsigneeCountryName,@ConsigneeName,@ConsigneeName2,@ConsigneeName3,@ConsigneePartyIdentification,@ConsigneePartyIdentificationCodeListQualifier,@ConsigneePostalCode,@ConsigneePurchasingDepartment,@ConsigneeStorageLocation,@ConsigneeStreet,@ConsigneeStreetPostalCode,@ConsigneeUnloadingPoint,@CreationTime,@CustomerCity,@CustomerContactFax,@CustomerContactName,@CustomerContactTelephone,@CustomerCountryName,@CustomerName,@CustomerName2,@CustomerName3,@CustomerPartyIdentification,@CustomerPartyIdentificationCodeListQualifier,@CustomerPostalCode,@CustomerPurchasingDepartment,@CustomerStreet,@CustomerStreetCityPostalCode,@CustomerStreetPostalCode,@DocumentName,@DocumentNumber,@Duns,@GlobalStatus,@MessageReferenceNumber,@Notes,@OrderId,@Reference); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ActionTime", item.ActionTime == null ? (object)DBNull.Value : item.ActionTime);
					sqlCommand.Parameters.AddWithValue("ActionUserId", item.ActionUserId == null ? (object)DBNull.Value : item.ActionUserId);
					sqlCommand.Parameters.AddWithValue("ActionUserName", item.ActionUsername == null ? (object)DBNull.Value : item.ActionUsername);
					sqlCommand.Parameters.AddWithValue("ConsigneeCity", item.ConsigneeCity == null ? (object)DBNull.Value : item.ConsigneeCity);
					sqlCommand.Parameters.AddWithValue("ConsigneeContactFax", item.ConsigneeContactFax == null ? (object)DBNull.Value : item.ConsigneeContactFax);
					sqlCommand.Parameters.AddWithValue("ConsigneeContactName", item.ConsigneeContactName == null ? (object)DBNull.Value : item.ConsigneeContactName);
					sqlCommand.Parameters.AddWithValue("ConsigneeContactTelephone", item.ConsigneeContactTelephone == null ? (object)DBNull.Value : item.ConsigneeContactTelephone);
					sqlCommand.Parameters.AddWithValue("ConsigneeCountryName", item.ConsigneeCountryName == null ? (object)DBNull.Value : item.ConsigneeCountryName);
					sqlCommand.Parameters.AddWithValue("ConsigneeName", item.ConsigneeName == null ? (object)DBNull.Value : item.ConsigneeName);
					sqlCommand.Parameters.AddWithValue("ConsigneeName2", item.ConsigneeName2 == null ? (object)DBNull.Value : item.ConsigneeName2);
					sqlCommand.Parameters.AddWithValue("ConsigneeName3", item.ConsigneeName3 == null ? (object)DBNull.Value : item.ConsigneeName3);
					sqlCommand.Parameters.AddWithValue("ConsigneePartyIdentification", item.ConsigneePartyIdentification == null ? (object)DBNull.Value : item.ConsigneePartyIdentification);
					sqlCommand.Parameters.AddWithValue("ConsigneePartyIdentificationCodeListQualifier", item.ConsigneePartyIdentificationCodeListQualifier == null ? (object)DBNull.Value : item.ConsigneePartyIdentificationCodeListQualifier);
					sqlCommand.Parameters.AddWithValue("ConsigneePostalCode", item.ConsigneePostalCode == null ? (object)DBNull.Value : item.ConsigneePostalCode);
					sqlCommand.Parameters.AddWithValue("ConsigneePurchasingDepartment", item.ConsigneePurchasingDepartment == null ? (object)DBNull.Value : item.ConsigneePurchasingDepartment);
					sqlCommand.Parameters.AddWithValue("ConsigneeStorageLocation", item.ConsigneeStorageLocation == null ? (object)DBNull.Value : item.ConsigneeStorageLocation);
					sqlCommand.Parameters.AddWithValue("ConsigneeStreet", item.ConsigneeStreet == null ? (object)DBNull.Value : item.ConsigneeStreet);
					sqlCommand.Parameters.AddWithValue("ConsigneeStreetPostalCode", item.ConsigneeStreetPostalCode == null ? (object)DBNull.Value : item.ConsigneeStreetPostalCode);
					sqlCommand.Parameters.AddWithValue("ConsigneeUnloadingPoint", item.ConsigneeUnloadingPoint == null ? (object)DBNull.Value : item.ConsigneeUnloadingPoint);
					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CustomerCity", item.CustomerCity == null ? (object)DBNull.Value : item.CustomerCity);
					sqlCommand.Parameters.AddWithValue("CustomerContactFax", item.CustomerContactFax == null ? (object)DBNull.Value : item.CustomerContactFax);
					sqlCommand.Parameters.AddWithValue("CustomerContactName", item.CustomerContactName == null ? (object)DBNull.Value : item.CustomerContactName);
					sqlCommand.Parameters.AddWithValue("CustomerContactTelephone", item.CustomerContactTelephone == null ? (object)DBNull.Value : item.CustomerContactTelephone);
					sqlCommand.Parameters.AddWithValue("CustomerCountryName", item.CustomerCountryName == null ? (object)DBNull.Value : item.CustomerCountryName);
					sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("CustomerName2", item.CustomerName2 == null ? (object)DBNull.Value : item.CustomerName2);
					sqlCommand.Parameters.AddWithValue("CustomerName3", item.CustomerName3 == null ? (object)DBNull.Value : item.CustomerName3);
					sqlCommand.Parameters.AddWithValue("CustomerPartyIdentification", item.CustomerPartyIdentification == null ? (object)DBNull.Value : item.CustomerPartyIdentification);
					sqlCommand.Parameters.AddWithValue("CustomerPartyIdentificationCodeListQualifier", item.CustomerPartyIdentificationCodeListQualifier == null ? (object)DBNull.Value : item.CustomerPartyIdentificationCodeListQualifier);
					sqlCommand.Parameters.AddWithValue("CustomerPostalCode", item.CustomerPostalCode == null ? (object)DBNull.Value : item.CustomerPostalCode);
					sqlCommand.Parameters.AddWithValue("CustomerPurchasingDepartment", item.CustomerPurchasingDepartment == null ? (object)DBNull.Value : item.CustomerPurchasingDepartment);
					sqlCommand.Parameters.AddWithValue("CustomerStreet", item.CustomerStreet == null ? (object)DBNull.Value : item.CustomerStreet);
					sqlCommand.Parameters.AddWithValue("CustomerStreetCityPostalCode", item.CustomerStreetCityPostalCode == null ? (object)DBNull.Value : item.CustomerStreetCityPostalCode);
					sqlCommand.Parameters.AddWithValue("CustomerStreetPostalCode", item.CustomerStreetPostalCode == null ? (object)DBNull.Value : item.CustomerStreetPostalCode);
					sqlCommand.Parameters.AddWithValue("DocumentName", item.DocumentName);
					sqlCommand.Parameters.AddWithValue("DocumentNumber", item.DocumentNumber);
					sqlCommand.Parameters.AddWithValue("Duns", item.Duns);
					sqlCommand.Parameters.AddWithValue("GlobalStatus", item.GlobalStatus);
					sqlCommand.Parameters.AddWithValue("MessageReferenceNumber", item.MessageReferenceNumber == null ? (object)DBNull.Value : item.MessageReferenceNumber);
					sqlCommand.Parameters.AddWithValue("Notes", item.Notes == null ? (object)DBNull.Value : item.Notes);
					sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
					sqlCommand.Parameters.AddWithValue("Reference", item.Reference);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 44; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity> items)
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
						query += " INSERT INTO [__EDI_OrderChange] ([ActionTime],[ActionUserId],[ActionUserName],[ConsigneeCity],[ConsigneeContactFax],[ConsigneeContactName],[ConsigneeContactTelephone],[ConsigneeCountryName],[ConsigneeName],[ConsigneeName2],[ConsigneeName3],[ConsigneePartyIdentification],[ConsigneePartyIdentificationCodeListQualifier],[ConsigneePostalCode],[ConsigneePurchasingDepartment],[ConsigneeStorageLocation],[ConsigneeStreet],[ConsigneeStreetPostalCode],[ConsigneeUnloadingPoint],[CreationTime],[CustomerCity],[CustomerContactFax],[CustomerContactName],[CustomerContactTelephone],[CustomerCountryName],[CustomerName],[CustomerName2],[CustomerName3],[CustomerPartyIdentification],[CustomerPartyIdentificationCodeListQualifier],[CustomerPostalCode],[CustomerPurchasingDepartment],[CustomerStreet],[CustomerStreetCityPostalCode],[CustomerStreetPostalCode],[DocumentName],[DocumentNumber],[Duns],[GlobalStatus],[MessageReferenceNumber],[Notes],[OrderId],[Reference]) VALUES ( "

							+ "@ActionTime" + i + ","
							+ "@ActionUserId" + i + ","
							+ "@ActionUserName" + i + ","
							+ "@ConsigneeCity" + i + ","
							+ "@ConsigneeContactFax" + i + ","
							+ "@ConsigneeContactName" + i + ","
							+ "@ConsigneeContactTelephone" + i + ","
							+ "@ConsigneeCountryName" + i + ","
							+ "@ConsigneeName" + i + ","
							+ "@ConsigneeName2" + i + ","
							+ "@ConsigneeName3" + i + ","
							+ "@ConsigneePartyIdentification" + i + ","
							+ "@ConsigneePartyIdentificationCodeListQualifier" + i + ","
							+ "@ConsigneePostalCode" + i + ","
							+ "@ConsigneePurchasingDepartment" + i + ","
							+ "@ConsigneeStorageLocation" + i + ","
							+ "@ConsigneeStreet" + i + ","
							+ "@ConsigneeStreetPostalCode" + i + ","
							+ "@ConsigneeUnloadingPoint" + i + ","
							+ "@CreationTime" + i + ","
							+ "@CustomerCity" + i + ","
							+ "@CustomerContactFax" + i + ","
							+ "@CustomerContactName" + i + ","
							+ "@CustomerContactTelephone" + i + ","
							+ "@CustomerCountryName" + i + ","
							+ "@CustomerName" + i + ","
							+ "@CustomerName2" + i + ","
							+ "@CustomerName3" + i + ","
							+ "@CustomerPartyIdentification" + i + ","
							+ "@CustomerPartyIdentificationCodeListQualifier" + i + ","
							+ "@CustomerPostalCode" + i + ","
							+ "@CustomerPurchasingDepartment" + i + ","
							+ "@CustomerStreet" + i + ","
							+ "@CustomerStreetCityPostalCode" + i + ","
							+ "@CustomerStreetPostalCode" + i + ","
							+ "@DocumentName" + i + ","
							+ "@DocumentNumber" + i + ","
							+ "@Duns" + i + ","
							+ "@GlobalStatus" + i + ","
							+ "@MessageReferenceNumber" + i + ","
							+ "@Notes" + i + ","
							+ "@OrderId" + i + ","
							+ "@Reference" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ActionTime" + i, item.ActionTime == null ? (object)DBNull.Value : item.ActionTime);
						sqlCommand.Parameters.AddWithValue("ActionUserId" + i, item.ActionUserId == null ? (object)DBNull.Value : item.ActionUserId);
						sqlCommand.Parameters.AddWithValue("ActionUserName" + i, item.ActionUsername == null ? (object)DBNull.Value : item.ActionUsername);
						sqlCommand.Parameters.AddWithValue("ConsigneeCity" + i, item.ConsigneeCity == null ? (object)DBNull.Value : item.ConsigneeCity);
						sqlCommand.Parameters.AddWithValue("ConsigneeContactFax" + i, item.ConsigneeContactFax == null ? (object)DBNull.Value : item.ConsigneeContactFax);
						sqlCommand.Parameters.AddWithValue("ConsigneeContactName" + i, item.ConsigneeContactName == null ? (object)DBNull.Value : item.ConsigneeContactName);
						sqlCommand.Parameters.AddWithValue("ConsigneeContactTelephone" + i, item.ConsigneeContactTelephone == null ? (object)DBNull.Value : item.ConsigneeContactTelephone);
						sqlCommand.Parameters.AddWithValue("ConsigneeCountryName" + i, item.ConsigneeCountryName == null ? (object)DBNull.Value : item.ConsigneeCountryName);
						sqlCommand.Parameters.AddWithValue("ConsigneeName" + i, item.ConsigneeName == null ? (object)DBNull.Value : item.ConsigneeName);
						sqlCommand.Parameters.AddWithValue("ConsigneeName2" + i, item.ConsigneeName2 == null ? (object)DBNull.Value : item.ConsigneeName2);
						sqlCommand.Parameters.AddWithValue("ConsigneeName3" + i, item.ConsigneeName3 == null ? (object)DBNull.Value : item.ConsigneeName3);
						sqlCommand.Parameters.AddWithValue("ConsigneePartyIdentification" + i, item.ConsigneePartyIdentification == null ? (object)DBNull.Value : item.ConsigneePartyIdentification);
						sqlCommand.Parameters.AddWithValue("ConsigneePartyIdentificationCodeListQualifier" + i, item.ConsigneePartyIdentificationCodeListQualifier == null ? (object)DBNull.Value : item.ConsigneePartyIdentificationCodeListQualifier);
						sqlCommand.Parameters.AddWithValue("ConsigneePostalCode" + i, item.ConsigneePostalCode == null ? (object)DBNull.Value : item.ConsigneePostalCode);
						sqlCommand.Parameters.AddWithValue("ConsigneePurchasingDepartment" + i, item.ConsigneePurchasingDepartment == null ? (object)DBNull.Value : item.ConsigneePurchasingDepartment);
						sqlCommand.Parameters.AddWithValue("ConsigneeStorageLocation" + i, item.ConsigneeStorageLocation == null ? (object)DBNull.Value : item.ConsigneeStorageLocation);
						sqlCommand.Parameters.AddWithValue("ConsigneeStreet" + i, item.ConsigneeStreet == null ? (object)DBNull.Value : item.ConsigneeStreet);
						sqlCommand.Parameters.AddWithValue("ConsigneeStreetPostalCode" + i, item.ConsigneeStreetPostalCode == null ? (object)DBNull.Value : item.ConsigneeStreetPostalCode);
						sqlCommand.Parameters.AddWithValue("ConsigneeUnloadingPoint" + i, item.ConsigneeUnloadingPoint == null ? (object)DBNull.Value : item.ConsigneeUnloadingPoint);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CustomerCity" + i, item.CustomerCity == null ? (object)DBNull.Value : item.CustomerCity);
						sqlCommand.Parameters.AddWithValue("CustomerContactFax" + i, item.CustomerContactFax == null ? (object)DBNull.Value : item.CustomerContactFax);
						sqlCommand.Parameters.AddWithValue("CustomerContactName" + i, item.CustomerContactName == null ? (object)DBNull.Value : item.CustomerContactName);
						sqlCommand.Parameters.AddWithValue("CustomerContactTelephone" + i, item.CustomerContactTelephone == null ? (object)DBNull.Value : item.CustomerContactTelephone);
						sqlCommand.Parameters.AddWithValue("CustomerCountryName" + i, item.CustomerCountryName == null ? (object)DBNull.Value : item.CustomerCountryName);
						sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
						sqlCommand.Parameters.AddWithValue("CustomerName2" + i, item.CustomerName2 == null ? (object)DBNull.Value : item.CustomerName2);
						sqlCommand.Parameters.AddWithValue("CustomerName3" + i, item.CustomerName3 == null ? (object)DBNull.Value : item.CustomerName3);
						sqlCommand.Parameters.AddWithValue("CustomerPartyIdentification" + i, item.CustomerPartyIdentification == null ? (object)DBNull.Value : item.CustomerPartyIdentification);
						sqlCommand.Parameters.AddWithValue("CustomerPartyIdentificationCodeListQualifier" + i, item.CustomerPartyIdentificationCodeListQualifier == null ? (object)DBNull.Value : item.CustomerPartyIdentificationCodeListQualifier);
						sqlCommand.Parameters.AddWithValue("CustomerPostalCode" + i, item.CustomerPostalCode == null ? (object)DBNull.Value : item.CustomerPostalCode);
						sqlCommand.Parameters.AddWithValue("CustomerPurchasingDepartment" + i, item.CustomerPurchasingDepartment == null ? (object)DBNull.Value : item.CustomerPurchasingDepartment);
						sqlCommand.Parameters.AddWithValue("CustomerStreet" + i, item.CustomerStreet == null ? (object)DBNull.Value : item.CustomerStreet);
						sqlCommand.Parameters.AddWithValue("CustomerStreetCityPostalCode" + i, item.CustomerStreetCityPostalCode == null ? (object)DBNull.Value : item.CustomerStreetCityPostalCode);
						sqlCommand.Parameters.AddWithValue("CustomerStreetPostalCode" + i, item.CustomerStreetPostalCode == null ? (object)DBNull.Value : item.CustomerStreetPostalCode);
						sqlCommand.Parameters.AddWithValue("DocumentName" + i, item.DocumentName);
						sqlCommand.Parameters.AddWithValue("DocumentNumber" + i, item.DocumentNumber);
						sqlCommand.Parameters.AddWithValue("Duns" + i, item.Duns);
						sqlCommand.Parameters.AddWithValue("GlobalStatus" + i, item.GlobalStatus);
						sqlCommand.Parameters.AddWithValue("MessageReferenceNumber" + i, item.MessageReferenceNumber == null ? (object)DBNull.Value : item.MessageReferenceNumber);
						sqlCommand.Parameters.AddWithValue("Notes" + i, item.Notes == null ? (object)DBNull.Value : item.Notes);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
						sqlCommand.Parameters.AddWithValue("Reference" + i, item.Reference);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__EDI_OrderChange] SET [ActionTime]=@ActionTime, [ActionUserId]=@ActionUserId, [ActionUserName]=@ActionUserName, [ConsigneeCity]=@ConsigneeCity, [ConsigneeContactFax]=@ConsigneeContactFax, [ConsigneeContactName]=@ConsigneeContactName, [ConsigneeContactTelephone]=@ConsigneeContactTelephone, [ConsigneeCountryName]=@ConsigneeCountryName, [ConsigneeName]=@ConsigneeName, [ConsigneeName2]=@ConsigneeName2, [ConsigneeName3]=@ConsigneeName3, [ConsigneePartyIdentification]=@ConsigneePartyIdentification, [ConsigneePartyIdentificationCodeListQualifier]=@ConsigneePartyIdentificationCodeListQualifier, [ConsigneePostalCode]=@ConsigneePostalCode, [ConsigneePurchasingDepartment]=@ConsigneePurchasingDepartment, [ConsigneeStorageLocation]=@ConsigneeStorageLocation, [ConsigneeStreet]=@ConsigneeStreet, [ConsigneeStreetPostalCode]=@ConsigneeStreetPostalCode, [ConsigneeUnloadingPoint]=@ConsigneeUnloadingPoint, [CreationTime]=@CreationTime, [CustomerCity]=@CustomerCity, [CustomerContactFax]=@CustomerContactFax, [CustomerContactName]=@CustomerContactName, [CustomerContactTelephone]=@CustomerContactTelephone, [CustomerCountryName]=@CustomerCountryName, [CustomerName]=@CustomerName, [CustomerName2]=@CustomerName2, [CustomerName3]=@CustomerName3, [CustomerPartyIdentification]=@CustomerPartyIdentification, [CustomerPartyIdentificationCodeListQualifier]=@CustomerPartyIdentificationCodeListQualifier, [CustomerPostalCode]=@CustomerPostalCode, [CustomerPurchasingDepartment]=@CustomerPurchasingDepartment, [CustomerStreet]=@CustomerStreet, [CustomerStreetCityPostalCode]=@CustomerStreetCityPostalCode, [CustomerStreetPostalCode]=@CustomerStreetPostalCode, [DocumentName]=@DocumentName, [DocumentNumber]=@DocumentNumber, [Duns]=@Duns, [GlobalStatus]=@GlobalStatus, [MessageReferenceNumber]=@MessageReferenceNumber, [Notes]=@Notes, [OrderId]=@OrderId, [Reference]=@Reference WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ActionTime", item.ActionTime == null ? (object)DBNull.Value : item.ActionTime);
				sqlCommand.Parameters.AddWithValue("ActionUserId", item.ActionUserId == null ? (object)DBNull.Value : item.ActionUserId);
				sqlCommand.Parameters.AddWithValue("ActionUserName", item.ActionUsername == null ? (object)DBNull.Value : item.ActionUsername);
				sqlCommand.Parameters.AddWithValue("ConsigneeCity", item.ConsigneeCity == null ? (object)DBNull.Value : item.ConsigneeCity);
				sqlCommand.Parameters.AddWithValue("ConsigneeContactFax", item.ConsigneeContactFax == null ? (object)DBNull.Value : item.ConsigneeContactFax);
				sqlCommand.Parameters.AddWithValue("ConsigneeContactName", item.ConsigneeContactName == null ? (object)DBNull.Value : item.ConsigneeContactName);
				sqlCommand.Parameters.AddWithValue("ConsigneeContactTelephone", item.ConsigneeContactTelephone == null ? (object)DBNull.Value : item.ConsigneeContactTelephone);
				sqlCommand.Parameters.AddWithValue("ConsigneeCountryName", item.ConsigneeCountryName == null ? (object)DBNull.Value : item.ConsigneeCountryName);
				sqlCommand.Parameters.AddWithValue("ConsigneeName", item.ConsigneeName == null ? (object)DBNull.Value : item.ConsigneeName);
				sqlCommand.Parameters.AddWithValue("ConsigneeName2", item.ConsigneeName2 == null ? (object)DBNull.Value : item.ConsigneeName2);
				sqlCommand.Parameters.AddWithValue("ConsigneeName3", item.ConsigneeName3 == null ? (object)DBNull.Value : item.ConsigneeName3);
				sqlCommand.Parameters.AddWithValue("ConsigneePartyIdentification", item.ConsigneePartyIdentification == null ? (object)DBNull.Value : item.ConsigneePartyIdentification);
				sqlCommand.Parameters.AddWithValue("ConsigneePartyIdentificationCodeListQualifier", item.ConsigneePartyIdentificationCodeListQualifier == null ? (object)DBNull.Value : item.ConsigneePartyIdentificationCodeListQualifier);
				sqlCommand.Parameters.AddWithValue("ConsigneePostalCode", item.ConsigneePostalCode == null ? (object)DBNull.Value : item.ConsigneePostalCode);
				sqlCommand.Parameters.AddWithValue("ConsigneePurchasingDepartment", item.ConsigneePurchasingDepartment == null ? (object)DBNull.Value : item.ConsigneePurchasingDepartment);
				sqlCommand.Parameters.AddWithValue("ConsigneeStorageLocation", item.ConsigneeStorageLocation == null ? (object)DBNull.Value : item.ConsigneeStorageLocation);
				sqlCommand.Parameters.AddWithValue("ConsigneeStreet", item.ConsigneeStreet == null ? (object)DBNull.Value : item.ConsigneeStreet);
				sqlCommand.Parameters.AddWithValue("ConsigneeStreetPostalCode", item.ConsigneeStreetPostalCode == null ? (object)DBNull.Value : item.ConsigneeStreetPostalCode);
				sqlCommand.Parameters.AddWithValue("ConsigneeUnloadingPoint", item.ConsigneeUnloadingPoint == null ? (object)DBNull.Value : item.ConsigneeUnloadingPoint);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CustomerCity", item.CustomerCity == null ? (object)DBNull.Value : item.CustomerCity);
				sqlCommand.Parameters.AddWithValue("CustomerContactFax", item.CustomerContactFax == null ? (object)DBNull.Value : item.CustomerContactFax);
				sqlCommand.Parameters.AddWithValue("CustomerContactName", item.CustomerContactName == null ? (object)DBNull.Value : item.CustomerContactName);
				sqlCommand.Parameters.AddWithValue("CustomerContactTelephone", item.CustomerContactTelephone == null ? (object)DBNull.Value : item.CustomerContactTelephone);
				sqlCommand.Parameters.AddWithValue("CustomerCountryName", item.CustomerCountryName == null ? (object)DBNull.Value : item.CustomerCountryName);
				sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
				sqlCommand.Parameters.AddWithValue("CustomerName2", item.CustomerName2 == null ? (object)DBNull.Value : item.CustomerName2);
				sqlCommand.Parameters.AddWithValue("CustomerName3", item.CustomerName3 == null ? (object)DBNull.Value : item.CustomerName3);
				sqlCommand.Parameters.AddWithValue("CustomerPartyIdentification", item.CustomerPartyIdentification == null ? (object)DBNull.Value : item.CustomerPartyIdentification);
				sqlCommand.Parameters.AddWithValue("CustomerPartyIdentificationCodeListQualifier", item.CustomerPartyIdentificationCodeListQualifier == null ? (object)DBNull.Value : item.CustomerPartyIdentificationCodeListQualifier);
				sqlCommand.Parameters.AddWithValue("CustomerPostalCode", item.CustomerPostalCode == null ? (object)DBNull.Value : item.CustomerPostalCode);
				sqlCommand.Parameters.AddWithValue("CustomerPurchasingDepartment", item.CustomerPurchasingDepartment == null ? (object)DBNull.Value : item.CustomerPurchasingDepartment);
				sqlCommand.Parameters.AddWithValue("CustomerStreet", item.CustomerStreet == null ? (object)DBNull.Value : item.CustomerStreet);
				sqlCommand.Parameters.AddWithValue("CustomerStreetCityPostalCode", item.CustomerStreetCityPostalCode == null ? (object)DBNull.Value : item.CustomerStreetCityPostalCode);
				sqlCommand.Parameters.AddWithValue("CustomerStreetPostalCode", item.CustomerStreetPostalCode == null ? (object)DBNull.Value : item.CustomerStreetPostalCode);
				sqlCommand.Parameters.AddWithValue("DocumentName", item.DocumentName);
				sqlCommand.Parameters.AddWithValue("DocumentNumber", item.DocumentNumber);
				sqlCommand.Parameters.AddWithValue("Duns", item.Duns);
				sqlCommand.Parameters.AddWithValue("GlobalStatus", item.GlobalStatus);
				sqlCommand.Parameters.AddWithValue("MessageReferenceNumber", item.MessageReferenceNumber == null ? (object)DBNull.Value : item.MessageReferenceNumber);
				sqlCommand.Parameters.AddWithValue("Notes", item.Notes == null ? (object)DBNull.Value : item.Notes);
				sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
				sqlCommand.Parameters.AddWithValue("Reference", item.Reference);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 44; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity> items)
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
						query += " UPDATE [__EDI_OrderChange] SET "

							+ "[ActionTime]=@ActionTime" + i + ","
							+ "[ActionUserId]=@ActionUserId" + i + ","
							+ "[ActionUserName]=@ActionUserName" + i + ","
							+ "[ConsigneeCity]=@ConsigneeCity" + i + ","
							+ "[ConsigneeContactFax]=@ConsigneeContactFax" + i + ","
							+ "[ConsigneeContactName]=@ConsigneeContactName" + i + ","
							+ "[ConsigneeContactTelephone]=@ConsigneeContactTelephone" + i + ","
							+ "[ConsigneeCountryName]=@ConsigneeCountryName" + i + ","
							+ "[ConsigneeName]=@ConsigneeName" + i + ","
							+ "[ConsigneeName2]=@ConsigneeName2" + i + ","
							+ "[ConsigneeName3]=@ConsigneeName3" + i + ","
							+ "[ConsigneePartyIdentification]=@ConsigneePartyIdentification" + i + ","
							+ "[ConsigneePartyIdentificationCodeListQualifier]=@ConsigneePartyIdentificationCodeListQualifier" + i + ","
							+ "[ConsigneePostalCode]=@ConsigneePostalCode" + i + ","
							+ "[ConsigneePurchasingDepartment]=@ConsigneePurchasingDepartment" + i + ","
							+ "[ConsigneeStorageLocation]=@ConsigneeStorageLocation" + i + ","
							+ "[ConsigneeStreet]=@ConsigneeStreet" + i + ","
							+ "[ConsigneeStreetPostalCode]=@ConsigneeStreetPostalCode" + i + ","
							+ "[ConsigneeUnloadingPoint]=@ConsigneeUnloadingPoint" + i + ","
							+ "[CreationTime]=@CreationTime" + i + ","
							+ "[CustomerCity]=@CustomerCity" + i + ","
							+ "[CustomerContactFax]=@CustomerContactFax" + i + ","
							+ "[CustomerContactName]=@CustomerContactName" + i + ","
							+ "[CustomerContactTelephone]=@CustomerContactTelephone" + i + ","
							+ "[CustomerCountryName]=@CustomerCountryName" + i + ","
							+ "[CustomerName]=@CustomerName" + i + ","
							+ "[CustomerName2]=@CustomerName2" + i + ","
							+ "[CustomerName3]=@CustomerName3" + i + ","
							+ "[CustomerPartyIdentification]=@CustomerPartyIdentification" + i + ","
							+ "[CustomerPartyIdentificationCodeListQualifier]=@CustomerPartyIdentificationCodeListQualifier" + i + ","
							+ "[CustomerPostalCode]=@CustomerPostalCode" + i + ","
							+ "[CustomerPurchasingDepartment]=@CustomerPurchasingDepartment" + i + ","
							+ "[CustomerStreet]=@CustomerStreet" + i + ","
							+ "[CustomerStreetCityPostalCode]=@CustomerStreetCityPostalCode" + i + ","
							+ "[CustomerStreetPostalCode]=@CustomerStreetPostalCode" + i + ","
							+ "[DocumentName]=@DocumentName" + i + ","
							+ "[DocumentNumber]=@DocumentNumber" + i + ","
							+ "[Duns]=@Duns" + i + ","
							+ "[GlobalStatus]=@GlobalStatus" + i + ","
							+ "[MessageReferenceNumber]=@MessageReferenceNumber" + i + ","
							+ "[Notes]=@Notes" + i + ","
							+ "[OrderId]=@OrderId" + i + ","
							+ "[Reference]=@Reference" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ActionTime" + i, item.ActionTime == null ? (object)DBNull.Value : item.ActionTime);
						sqlCommand.Parameters.AddWithValue("ActionUserId" + i, item.ActionUserId == null ? (object)DBNull.Value : item.ActionUserId);
						sqlCommand.Parameters.AddWithValue("ActionUserName" + i, item.ActionUsername == null ? (object)DBNull.Value : item.ActionUsername);
						sqlCommand.Parameters.AddWithValue("ConsigneeCity" + i, item.ConsigneeCity == null ? (object)DBNull.Value : item.ConsigneeCity);
						sqlCommand.Parameters.AddWithValue("ConsigneeContactFax" + i, item.ConsigneeContactFax == null ? (object)DBNull.Value : item.ConsigneeContactFax);
						sqlCommand.Parameters.AddWithValue("ConsigneeContactName" + i, item.ConsigneeContactName == null ? (object)DBNull.Value : item.ConsigneeContactName);
						sqlCommand.Parameters.AddWithValue("ConsigneeContactTelephone" + i, item.ConsigneeContactTelephone == null ? (object)DBNull.Value : item.ConsigneeContactTelephone);
						sqlCommand.Parameters.AddWithValue("ConsigneeCountryName" + i, item.ConsigneeCountryName == null ? (object)DBNull.Value : item.ConsigneeCountryName);
						sqlCommand.Parameters.AddWithValue("ConsigneeName" + i, item.ConsigneeName == null ? (object)DBNull.Value : item.ConsigneeName);
						sqlCommand.Parameters.AddWithValue("ConsigneeName2" + i, item.ConsigneeName2 == null ? (object)DBNull.Value : item.ConsigneeName2);
						sqlCommand.Parameters.AddWithValue("ConsigneeName3" + i, item.ConsigneeName3 == null ? (object)DBNull.Value : item.ConsigneeName3);
						sqlCommand.Parameters.AddWithValue("ConsigneePartyIdentification" + i, item.ConsigneePartyIdentification == null ? (object)DBNull.Value : item.ConsigneePartyIdentification);
						sqlCommand.Parameters.AddWithValue("ConsigneePartyIdentificationCodeListQualifier" + i, item.ConsigneePartyIdentificationCodeListQualifier == null ? (object)DBNull.Value : item.ConsigneePartyIdentificationCodeListQualifier);
						sqlCommand.Parameters.AddWithValue("ConsigneePostalCode" + i, item.ConsigneePostalCode == null ? (object)DBNull.Value : item.ConsigneePostalCode);
						sqlCommand.Parameters.AddWithValue("ConsigneePurchasingDepartment" + i, item.ConsigneePurchasingDepartment == null ? (object)DBNull.Value : item.ConsigneePurchasingDepartment);
						sqlCommand.Parameters.AddWithValue("ConsigneeStorageLocation" + i, item.ConsigneeStorageLocation == null ? (object)DBNull.Value : item.ConsigneeStorageLocation);
						sqlCommand.Parameters.AddWithValue("ConsigneeStreet" + i, item.ConsigneeStreet == null ? (object)DBNull.Value : item.ConsigneeStreet);
						sqlCommand.Parameters.AddWithValue("ConsigneeStreetPostalCode" + i, item.ConsigneeStreetPostalCode == null ? (object)DBNull.Value : item.ConsigneeStreetPostalCode);
						sqlCommand.Parameters.AddWithValue("ConsigneeUnloadingPoint" + i, item.ConsigneeUnloadingPoint == null ? (object)DBNull.Value : item.ConsigneeUnloadingPoint);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CustomerCity" + i, item.CustomerCity == null ? (object)DBNull.Value : item.CustomerCity);
						sqlCommand.Parameters.AddWithValue("CustomerContactFax" + i, item.CustomerContactFax == null ? (object)DBNull.Value : item.CustomerContactFax);
						sqlCommand.Parameters.AddWithValue("CustomerContactName" + i, item.CustomerContactName == null ? (object)DBNull.Value : item.CustomerContactName);
						sqlCommand.Parameters.AddWithValue("CustomerContactTelephone" + i, item.CustomerContactTelephone == null ? (object)DBNull.Value : item.CustomerContactTelephone);
						sqlCommand.Parameters.AddWithValue("CustomerCountryName" + i, item.CustomerCountryName == null ? (object)DBNull.Value : item.CustomerCountryName);
						sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
						sqlCommand.Parameters.AddWithValue("CustomerName2" + i, item.CustomerName2 == null ? (object)DBNull.Value : item.CustomerName2);
						sqlCommand.Parameters.AddWithValue("CustomerName3" + i, item.CustomerName3 == null ? (object)DBNull.Value : item.CustomerName3);
						sqlCommand.Parameters.AddWithValue("CustomerPartyIdentification" + i, item.CustomerPartyIdentification == null ? (object)DBNull.Value : item.CustomerPartyIdentification);
						sqlCommand.Parameters.AddWithValue("CustomerPartyIdentificationCodeListQualifier" + i, item.CustomerPartyIdentificationCodeListQualifier == null ? (object)DBNull.Value : item.CustomerPartyIdentificationCodeListQualifier);
						sqlCommand.Parameters.AddWithValue("CustomerPostalCode" + i, item.CustomerPostalCode == null ? (object)DBNull.Value : item.CustomerPostalCode);
						sqlCommand.Parameters.AddWithValue("CustomerPurchasingDepartment" + i, item.CustomerPurchasingDepartment == null ? (object)DBNull.Value : item.CustomerPurchasingDepartment);
						sqlCommand.Parameters.AddWithValue("CustomerStreet" + i, item.CustomerStreet == null ? (object)DBNull.Value : item.CustomerStreet);
						sqlCommand.Parameters.AddWithValue("CustomerStreetCityPostalCode" + i, item.CustomerStreetCityPostalCode == null ? (object)DBNull.Value : item.CustomerStreetCityPostalCode);
						sqlCommand.Parameters.AddWithValue("CustomerStreetPostalCode" + i, item.CustomerStreetPostalCode == null ? (object)DBNull.Value : item.CustomerStreetPostalCode);
						sqlCommand.Parameters.AddWithValue("DocumentName" + i, item.DocumentName);
						sqlCommand.Parameters.AddWithValue("DocumentNumber" + i, item.DocumentNumber);
						sqlCommand.Parameters.AddWithValue("Duns" + i, item.Duns);
						sqlCommand.Parameters.AddWithValue("GlobalStatus" + i, item.GlobalStatus);
						sqlCommand.Parameters.AddWithValue("MessageReferenceNumber" + i, item.MessageReferenceNumber == null ? (object)DBNull.Value : item.MessageReferenceNumber);
						sqlCommand.Parameters.AddWithValue("Notes" + i, item.Notes == null ? (object)DBNull.Value : item.Notes);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
						sqlCommand.Parameters.AddWithValue("Reference" + i, item.Reference);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
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
				string query = "DELETE FROM [__EDI_OrderChange] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				results = sqlCommand.ExecuteNonQuery();
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

					string query = "DELETE FROM [__EDI_OrderChange] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity item, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			int response = int.MinValue;
			string query = "INSERT INTO [__EDI_OrderChange] ([ActionTime],[ActionUserId],[ActionUserName],[ConsigneeCity],[ConsigneeContactFax],[ConsigneeContactName],[ConsigneeContactTelephone],[ConsigneeCountryName],[ConsigneeName],[ConsigneeName2],[ConsigneeName3],[ConsigneePartyIdentification],[ConsigneePartyIdentificationCodeListQualifier],[ConsigneePostalCode],[ConsigneePurchasingDepartment],[ConsigneeStorageLocation],[ConsigneeStreet],[ConsigneeStreetPostalCode],[ConsigneeUnloadingPoint],[CreationTime],[CustomerCity],[CustomerContactFax],[CustomerContactName],[CustomerContactTelephone],[CustomerCountryName],[CustomerName],[CustomerName2],[CustomerName3],[CustomerPartyIdentification],[CustomerPartyIdentificationCodeListQualifier],[CustomerPostalCode],[CustomerPurchasingDepartment],[CustomerStreet],[CustomerStreetCityPostalCode],[CustomerStreetPostalCode],[DocumentName],[DocumentNumber],[Duns],[GlobalStatus],[MessageReferenceNumber],[Notes],[OrderId],[Reference])  VALUES (@ActionTime,@ActionUserId,@ActionUserName,@ConsigneeCity,@ConsigneeContactFax,@ConsigneeContactName,@ConsigneeContactTelephone,@ConsigneeCountryName,@ConsigneeName,@ConsigneeName2,@ConsigneeName3,@ConsigneePartyIdentification,@ConsigneePartyIdentificationCodeListQualifier,@ConsigneePostalCode,@ConsigneePurchasingDepartment,@ConsigneeStorageLocation,@ConsigneeStreet,@ConsigneeStreetPostalCode,@ConsigneeUnloadingPoint,@CreationTime,@CustomerCity,@CustomerContactFax,@CustomerContactName,@CustomerContactTelephone,@CustomerCountryName,@CustomerName,@CustomerName2,@CustomerName3,@CustomerPartyIdentification,@CustomerPartyIdentificationCodeListQualifier,@CustomerPostalCode,@CustomerPurchasingDepartment,@CustomerStreet,@CustomerStreetCityPostalCode,@CustomerStreetPostalCode,@DocumentName,@DocumentNumber,@Duns,@GlobalStatus,@MessageReferenceNumber,@Notes,@OrderId,@Reference); ";
			query += "SELECT SCOPE_IDENTITY();";
			using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
			{

				sqlCommand.Parameters.AddWithValue("ActionTime", item.ActionTime == null ? (object)DBNull.Value : item.ActionTime);
				sqlCommand.Parameters.AddWithValue("ActionUserId", item.ActionUserId == null ? (object)DBNull.Value : item.ActionUserId);
				sqlCommand.Parameters.AddWithValue("ActionUserName", item.ActionUsername == null ? (object)DBNull.Value : item.ActionUsername);
				sqlCommand.Parameters.AddWithValue("ConsigneeCity", item.ConsigneeCity == null ? (object)DBNull.Value : item.ConsigneeCity);
				sqlCommand.Parameters.AddWithValue("ConsigneeContactFax", item.ConsigneeContactFax == null ? (object)DBNull.Value : item.ConsigneeContactFax);
				sqlCommand.Parameters.AddWithValue("ConsigneeContactName", item.ConsigneeContactName == null ? (object)DBNull.Value : item.ConsigneeContactName);
				sqlCommand.Parameters.AddWithValue("ConsigneeContactTelephone", item.ConsigneeContactTelephone == null ? (object)DBNull.Value : item.ConsigneeContactTelephone);
				sqlCommand.Parameters.AddWithValue("ConsigneeCountryName", item.ConsigneeCountryName == null ? (object)DBNull.Value : item.ConsigneeCountryName);
				sqlCommand.Parameters.AddWithValue("ConsigneeName", item.ConsigneeName == null ? (object)DBNull.Value : item.ConsigneeName);
				sqlCommand.Parameters.AddWithValue("ConsigneeName2", item.ConsigneeName2 == null ? (object)DBNull.Value : item.ConsigneeName2);
				sqlCommand.Parameters.AddWithValue("ConsigneeName3", item.ConsigneeName3 == null ? (object)DBNull.Value : item.ConsigneeName3);
				sqlCommand.Parameters.AddWithValue("ConsigneePartyIdentification", item.ConsigneePartyIdentification == null ? (object)DBNull.Value : item.ConsigneePartyIdentification);
				sqlCommand.Parameters.AddWithValue("ConsigneePartyIdentificationCodeListQualifier", item.ConsigneePartyIdentificationCodeListQualifier == null ? (object)DBNull.Value : item.ConsigneePartyIdentificationCodeListQualifier);
				sqlCommand.Parameters.AddWithValue("ConsigneePostalCode", item.ConsigneePostalCode == null ? (object)DBNull.Value : item.ConsigneePostalCode);
				sqlCommand.Parameters.AddWithValue("ConsigneePurchasingDepartment", item.ConsigneePurchasingDepartment == null ? (object)DBNull.Value : item.ConsigneePurchasingDepartment);
				sqlCommand.Parameters.AddWithValue("ConsigneeStorageLocation", item.ConsigneeStorageLocation == null ? (object)DBNull.Value : item.ConsigneeStorageLocation);
				sqlCommand.Parameters.AddWithValue("ConsigneeStreet", item.ConsigneeStreet == null ? (object)DBNull.Value : item.ConsigneeStreet);
				sqlCommand.Parameters.AddWithValue("ConsigneeStreetPostalCode", item.ConsigneeStreetPostalCode == null ? (object)DBNull.Value : item.ConsigneeStreetPostalCode);
				sqlCommand.Parameters.AddWithValue("ConsigneeUnloadingPoint", item.ConsigneeUnloadingPoint == null ? (object)DBNull.Value : item.ConsigneeUnloadingPoint);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CustomerCity", item.CustomerCity == null ? (object)DBNull.Value : item.CustomerCity);
				sqlCommand.Parameters.AddWithValue("CustomerContactFax", item.CustomerContactFax == null ? (object)DBNull.Value : item.CustomerContactFax);
				sqlCommand.Parameters.AddWithValue("CustomerContactName", item.CustomerContactName == null ? (object)DBNull.Value : item.CustomerContactName);
				sqlCommand.Parameters.AddWithValue("CustomerContactTelephone", item.CustomerContactTelephone == null ? (object)DBNull.Value : item.CustomerContactTelephone);
				sqlCommand.Parameters.AddWithValue("CustomerCountryName", item.CustomerCountryName == null ? (object)DBNull.Value : item.CustomerCountryName);
				sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
				sqlCommand.Parameters.AddWithValue("CustomerName2", item.CustomerName2 == null ? (object)DBNull.Value : item.CustomerName2);
				sqlCommand.Parameters.AddWithValue("CustomerName3", item.CustomerName3 == null ? (object)DBNull.Value : item.CustomerName3);
				sqlCommand.Parameters.AddWithValue("CustomerPartyIdentification", item.CustomerPartyIdentification == null ? (object)DBNull.Value : item.CustomerPartyIdentification);
				sqlCommand.Parameters.AddWithValue("CustomerPartyIdentificationCodeListQualifier", item.CustomerPartyIdentificationCodeListQualifier == null ? (object)DBNull.Value : item.CustomerPartyIdentificationCodeListQualifier);
				sqlCommand.Parameters.AddWithValue("CustomerPostalCode", item.CustomerPostalCode == null ? (object)DBNull.Value : item.CustomerPostalCode);
				sqlCommand.Parameters.AddWithValue("CustomerPurchasingDepartment", item.CustomerPurchasingDepartment == null ? (object)DBNull.Value : item.CustomerPurchasingDepartment);
				sqlCommand.Parameters.AddWithValue("CustomerStreet", item.CustomerStreet == null ? (object)DBNull.Value : item.CustomerStreet);
				sqlCommand.Parameters.AddWithValue("CustomerStreetCityPostalCode", item.CustomerStreetCityPostalCode == null ? (object)DBNull.Value : item.CustomerStreetCityPostalCode);
				sqlCommand.Parameters.AddWithValue("CustomerStreetPostalCode", item.CustomerStreetPostalCode == null ? (object)DBNull.Value : item.CustomerStreetPostalCode);
				sqlCommand.Parameters.AddWithValue("DocumentName", item.DocumentName);
				sqlCommand.Parameters.AddWithValue("DocumentNumber", item.DocumentNumber);
				sqlCommand.Parameters.AddWithValue("Duns", item.Duns);
				sqlCommand.Parameters.AddWithValue("GlobalStatus", item.GlobalStatus);
				sqlCommand.Parameters.AddWithValue("MessageReferenceNumber", item.MessageReferenceNumber == null ? (object)DBNull.Value : item.MessageReferenceNumber);
				sqlCommand.Parameters.AddWithValue("Notes", item.Notes == null ? (object)DBNull.Value : item.Notes);
				sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
				sqlCommand.Parameters.AddWithValue("Reference", item.Reference);

				var result = sqlCommand.ExecuteScalar();
				response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}

			return response;
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity> GetByOrderId(int orderId)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__EDI_OrderChange] WHERE [OrderId]=@orderId";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("orderId", orderId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity> GetByOrdersIds(List<int> ordersIds)
		{
			if(ordersIds != null && ordersIds.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				var results = new List<Entities.Tables.PRS.OrderChangeEntity>();
				if(ordersIds.Count <= maxQueryNumber)
				{
					results = getByOrdersIds(ordersIds);
				}
				else
				{
					int batchNumber = ordersIds.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getByOrdersIds(ordersIds.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(getByOrdersIds(ordersIds.GetRange(batchNumber * maxQueryNumber, ordersIds.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity> getByOrdersIds(List<int> ordersIds)
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

					sqlCommand.CommandText = "SELECT * FROM [__EDI_OrderChange] WHERE [OrderId] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity>();
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity> GetBy_GlobalStatus_OrdersIds(int globalStatus, List<int> ordersIds)
		{
			if(ordersIds != null && ordersIds.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				var results = new List<Entities.Tables.PRS.OrderChangeEntity>();
				if(ordersIds.Count <= maxQueryNumber)
				{
					results = getBy_GlobalStatus_OrdersIds(globalStatus, ordersIds);
				}
				else
				{
					int batchNumber = ordersIds.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getBy_GlobalStatus_OrdersIds(globalStatus, ordersIds.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(getBy_GlobalStatus_OrdersIds(globalStatus, ordersIds.GetRange(batchNumber * maxQueryNumber, ordersIds.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity> getBy_GlobalStatus_OrdersIds(int globalStatus, List<int> ordersIds)
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

					sqlCommand.CommandText = "SELECT * FROM [__EDI_OrderChange] WHERE GlobalStatus=@GlobalStatus AND [OrderId] IN (" + queryIds + ")";
					sqlCommand.Parameters.AddWithValue("GlobalStatus", globalStatus);

					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity>();
		}
		#endregion

		#region Helpers

		public static Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity Get(int id, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__EDI_OrderChange] WHERE [Id]=@Id";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static int Update(Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity item, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			string query = "UPDATE [__EDI_OrderChange] SET [ActionTime]=@ActionTime, [ActionUserId]=@ActionUserId, [ActionUserName]=@ActionUserName, [ConsigneeCity]=@ConsigneeCity, [ConsigneeContactFax]=@ConsigneeContactFax, [ConsigneeContactName]=@ConsigneeContactName, [ConsigneeContactTelephone]=@ConsigneeContactTelephone, [ConsigneeCountryName]=@ConsigneeCountryName, [ConsigneeName]=@ConsigneeName, [ConsigneeName2]=@ConsigneeName2, [ConsigneeName3]=@ConsigneeName3, [ConsigneePartyIdentification]=@ConsigneePartyIdentification, [ConsigneePartyIdentificationCodeListQualifier]=@ConsigneePartyIdentificationCodeListQualifier, [ConsigneePostalCode]=@ConsigneePostalCode, [ConsigneePurchasingDepartment]=@ConsigneePurchasingDepartment, [ConsigneeStorageLocation]=@ConsigneeStorageLocation, [ConsigneeStreet]=@ConsigneeStreet, [ConsigneeStreetPostalCode]=@ConsigneeStreetPostalCode, [ConsigneeUnloadingPoint]=@ConsigneeUnloadingPoint, [CreationTime]=@CreationTime, [CustomerCity]=@CustomerCity, [CustomerContactFax]=@CustomerContactFax, [CustomerContactName]=@CustomerContactName, [CustomerContactTelephone]=@CustomerContactTelephone, [CustomerCountryName]=@CustomerCountryName, [CustomerName]=@CustomerName, [CustomerName2]=@CustomerName2, [CustomerName3]=@CustomerName3, [CustomerPartyIdentification]=@CustomerPartyIdentification, [CustomerPartyIdentificationCodeListQualifier]=@CustomerPartyIdentificationCodeListQualifier, [CustomerPostalCode]=@CustomerPostalCode, [CustomerPurchasingDepartment]=@CustomerPurchasingDepartment, [CustomerStreet]=@CustomerStreet, [CustomerStreetCityPostalCode]=@CustomerStreetCityPostalCode, [CustomerStreetPostalCode]=@CustomerStreetPostalCode, [DocumentName]=@DocumentName, [DocumentNumber]=@DocumentNumber, [Duns]=@Duns, [GlobalStatus]=@GlobalStatus, [MessageReferenceNumber]=@MessageReferenceNumber, [Notes]=@Notes, [OrderId]=@OrderId, [Reference]=@Reference WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("ActionTime", item.ActionTime == null ? (object)DBNull.Value : item.ActionTime);
			sqlCommand.Parameters.AddWithValue("ActionUserId", item.ActionUserId == null ? (object)DBNull.Value : item.ActionUserId);
			sqlCommand.Parameters.AddWithValue("ActionUserName", item.ActionUsername == null ? (object)DBNull.Value : item.ActionUsername);
			sqlCommand.Parameters.AddWithValue("ConsigneeCity", item.ConsigneeCity == null ? (object)DBNull.Value : item.ConsigneeCity);
			sqlCommand.Parameters.AddWithValue("ConsigneeContactFax", item.ConsigneeContactFax == null ? (object)DBNull.Value : item.ConsigneeContactFax);
			sqlCommand.Parameters.AddWithValue("ConsigneeContactName", item.ConsigneeContactName == null ? (object)DBNull.Value : item.ConsigneeContactName);
			sqlCommand.Parameters.AddWithValue("ConsigneeContactTelephone", item.ConsigneeContactTelephone == null ? (object)DBNull.Value : item.ConsigneeContactTelephone);
			sqlCommand.Parameters.AddWithValue("ConsigneeCountryName", item.ConsigneeCountryName == null ? (object)DBNull.Value : item.ConsigneeCountryName);
			sqlCommand.Parameters.AddWithValue("ConsigneeName", item.ConsigneeName == null ? (object)DBNull.Value : item.ConsigneeName);
			sqlCommand.Parameters.AddWithValue("ConsigneeName2", item.ConsigneeName2 == null ? (object)DBNull.Value : item.ConsigneeName2);
			sqlCommand.Parameters.AddWithValue("ConsigneeName3", item.ConsigneeName3 == null ? (object)DBNull.Value : item.ConsigneeName3);
			sqlCommand.Parameters.AddWithValue("ConsigneePartyIdentification", item.ConsigneePartyIdentification == null ? (object)DBNull.Value : item.ConsigneePartyIdentification);
			sqlCommand.Parameters.AddWithValue("ConsigneePartyIdentificationCodeListQualifier", item.ConsigneePartyIdentificationCodeListQualifier == null ? (object)DBNull.Value : item.ConsigneePartyIdentificationCodeListQualifier);
			sqlCommand.Parameters.AddWithValue("ConsigneePostalCode", item.ConsigneePostalCode == null ? (object)DBNull.Value : item.ConsigneePostalCode);
			sqlCommand.Parameters.AddWithValue("ConsigneePurchasingDepartment", item.ConsigneePurchasingDepartment == null ? (object)DBNull.Value : item.ConsigneePurchasingDepartment);
			sqlCommand.Parameters.AddWithValue("ConsigneeStorageLocation", item.ConsigneeStorageLocation == null ? (object)DBNull.Value : item.ConsigneeStorageLocation);
			sqlCommand.Parameters.AddWithValue("ConsigneeStreet", item.ConsigneeStreet == null ? (object)DBNull.Value : item.ConsigneeStreet);
			sqlCommand.Parameters.AddWithValue("ConsigneeStreetPostalCode", item.ConsigneeStreetPostalCode == null ? (object)DBNull.Value : item.ConsigneeStreetPostalCode);
			sqlCommand.Parameters.AddWithValue("ConsigneeUnloadingPoint", item.ConsigneeUnloadingPoint == null ? (object)DBNull.Value : item.ConsigneeUnloadingPoint);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
			sqlCommand.Parameters.AddWithValue("CustomerCity", item.CustomerCity == null ? (object)DBNull.Value : item.CustomerCity);
			sqlCommand.Parameters.AddWithValue("CustomerContactFax", item.CustomerContactFax == null ? (object)DBNull.Value : item.CustomerContactFax);
			sqlCommand.Parameters.AddWithValue("CustomerContactName", item.CustomerContactName == null ? (object)DBNull.Value : item.CustomerContactName);
			sqlCommand.Parameters.AddWithValue("CustomerContactTelephone", item.CustomerContactTelephone == null ? (object)DBNull.Value : item.CustomerContactTelephone);
			sqlCommand.Parameters.AddWithValue("CustomerCountryName", item.CustomerCountryName == null ? (object)DBNull.Value : item.CustomerCountryName);
			sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
			sqlCommand.Parameters.AddWithValue("CustomerName2", item.CustomerName2 == null ? (object)DBNull.Value : item.CustomerName2);
			sqlCommand.Parameters.AddWithValue("CustomerName3", item.CustomerName3 == null ? (object)DBNull.Value : item.CustomerName3);
			sqlCommand.Parameters.AddWithValue("CustomerPartyIdentification", item.CustomerPartyIdentification == null ? (object)DBNull.Value : item.CustomerPartyIdentification);
			sqlCommand.Parameters.AddWithValue("CustomerPartyIdentificationCodeListQualifier", item.CustomerPartyIdentificationCodeListQualifier == null ? (object)DBNull.Value : item.CustomerPartyIdentificationCodeListQualifier);
			sqlCommand.Parameters.AddWithValue("CustomerPostalCode", item.CustomerPostalCode == null ? (object)DBNull.Value : item.CustomerPostalCode);
			sqlCommand.Parameters.AddWithValue("CustomerPurchasingDepartment", item.CustomerPurchasingDepartment == null ? (object)DBNull.Value : item.CustomerPurchasingDepartment);
			sqlCommand.Parameters.AddWithValue("CustomerStreet", item.CustomerStreet == null ? (object)DBNull.Value : item.CustomerStreet);
			sqlCommand.Parameters.AddWithValue("CustomerStreetCityPostalCode", item.CustomerStreetCityPostalCode == null ? (object)DBNull.Value : item.CustomerStreetCityPostalCode);
			sqlCommand.Parameters.AddWithValue("CustomerStreetPostalCode", item.CustomerStreetPostalCode == null ? (object)DBNull.Value : item.CustomerStreetPostalCode);
			sqlCommand.Parameters.AddWithValue("DocumentName", item.DocumentName);
			sqlCommand.Parameters.AddWithValue("DocumentNumber", item.DocumentNumber);
			sqlCommand.Parameters.AddWithValue("Duns", item.Duns);
			sqlCommand.Parameters.AddWithValue("GlobalStatus", item.GlobalStatus);
			sqlCommand.Parameters.AddWithValue("MessageReferenceNumber", item.MessageReferenceNumber == null ? (object)DBNull.Value : item.MessageReferenceNumber);
			sqlCommand.Parameters.AddWithValue("Notes", item.Notes == null ? (object)DBNull.Value : item.Notes);
			sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
			sqlCommand.Parameters.AddWithValue("Reference", item.Reference);

			return sqlCommand.ExecuteNonQuery();

		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.PRS.OrderChangeEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
