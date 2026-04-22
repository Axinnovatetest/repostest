using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class HeaderAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.HeaderEntity Get(long id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__EDI_DLF_Header] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.HeaderEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__EDI_DLF_Header]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.HeaderEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> Get(List<long> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> get(List<long> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__EDI_DLF_Header] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.HeaderEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity>();
		}

		public static long Insert(Infrastructure.Data.Entities.Tables.CTS.HeaderEntity item)
		{
			long response = long.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__EDI_DLF_Header] ([BuyerContactName],[BuyerContactTelephone],[BuyerDUNS],[BuyerPartyIdentification],[BuyerPartyName],[BuyerPurchasingDepartment],[ConsigneeCity],[ConsigneeContactFax],[ConsigneeContactTelephone],[ConsigneeCountryName],[ConsigneeDUNS],[ConsigneePartyIdentification],[ConsigneePartyName],[ConsigneePostCode],[ConsigneeStorageLocation],[ConsigneeStreet],[ConsigneeUnloadingPoint],[CreationTime],[DocumentNumber],[Done],[ManualCreation],[MessageReferenceNumber],[MessageType],[PreviousReferenceVersionNumber],[PSZCustomernumber],[ReceivingDate],[RecipientId],[ReferenceNumber],[ReferenceVersionNumber],[SenderId],[SupplierCity],[SupplierContactFax],[SupplierContactTelephone],[SupplierCountryName],[SupplierDUNS],[SupplierPartyIdentification],[SupplierPartyName],[SupplierPostCode],[SupplierStreet],[ValidFrom],[ValidTill]) OUTPUT INSERTED.[Id] VALUES (@BuyerContactName,@BuyerContactTelephone,@BuyerDUNS,@BuyerPartyIdentification,@BuyerPartyName,@BuyerPurchasingDepartment,@ConsigneeCity,@ConsigneeContactFax,@ConsigneeContactTelephone,@ConsigneeCountryName,@ConsigneeDUNS,@ConsigneePartyIdentification,@ConsigneePartyName,@ConsigneePostCode,@ConsigneeStorageLocation,@ConsigneeStreet,@ConsigneeUnloadingPoint,@CreationTime,@DocumentNumber,@Done,@ManualCreation,@MessageReferenceNumber,@MessageType,@PreviousReferenceVersionNumber,@PSZCustomernumber,@ReceivingDate,@RecipientId,@ReferenceNumber,@ReferenceVersionNumber,@SenderId,@SupplierCity,@SupplierContactFax,@SupplierContactTelephone,@SupplierCountryName,@SupplierDUNS,@SupplierPartyIdentification,@SupplierPartyName,@SupplierPostCode,@SupplierStreet,@ValidFrom,@ValidTill); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("BuyerContactName", item.BuyerContactName);
					sqlCommand.Parameters.AddWithValue("BuyerContactTelephone", item.BuyerContactTelephone);
					sqlCommand.Parameters.AddWithValue("BuyerDUNS", item.BuyerDUNS);
					sqlCommand.Parameters.AddWithValue("BuyerPartyIdentification", item.BuyerPartyIdentification == null ? (object)DBNull.Value : item.BuyerPartyIdentification);
					sqlCommand.Parameters.AddWithValue("BuyerPartyName", item.BuyerPartyName);
					sqlCommand.Parameters.AddWithValue("BuyerPurchasingDepartment", item.BuyerPurchasingDepartment ?? "");
					sqlCommand.Parameters.AddWithValue("ConsigneeCity", item.ConsigneeCity);
					sqlCommand.Parameters.AddWithValue("ConsigneeContactFax", item.ConsigneeContactFax ?? "");
					sqlCommand.Parameters.AddWithValue("ConsigneeContactTelephone", item.ConsigneeContactTelephone ?? "");
					sqlCommand.Parameters.AddWithValue("ConsigneeCountryName", item.ConsigneeCountryName);
					sqlCommand.Parameters.AddWithValue("ConsigneeDUNS", item.ConsigneeDUNS);
					sqlCommand.Parameters.AddWithValue("ConsigneePartyIdentification", item.ConsigneePartyIdentification);
					sqlCommand.Parameters.AddWithValue("ConsigneePartyName", item.ConsigneePartyName);
					sqlCommand.Parameters.AddWithValue("ConsigneePostCode", item.ConsigneePostCode);
					sqlCommand.Parameters.AddWithValue("ConsigneeStorageLocation", item.ConsigneeStorageLocation == null ? (object)DBNull.Value : item.ConsigneeStorageLocation);
					sqlCommand.Parameters.AddWithValue("ConsigneeStreet", item.ConsigneeStreet);
					sqlCommand.Parameters.AddWithValue("ConsigneeUnloadingPoint", item.ConsigneeUnloadingPoint == null ? (object)DBNull.Value : item.ConsigneeUnloadingPoint);
					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("DocumentNumber", item.DocumentNumber);
					sqlCommand.Parameters.AddWithValue("Done", item.Done == null ? (object)DBNull.Value : item.Done);
					sqlCommand.Parameters.AddWithValue("ManualCreation", item.ManualCreation == null ? (object)DBNull.Value : item.ManualCreation);
					sqlCommand.Parameters.AddWithValue("MessageReferenceNumber", item.MessageReferenceNumber);
					sqlCommand.Parameters.AddWithValue("MessageType", item.MessageType);
					sqlCommand.Parameters.AddWithValue("PreviousReferenceVersionNumber", item.PreviousReferenceVersionNumber == null ? (object)DBNull.Value : item.PreviousReferenceVersionNumber);
					sqlCommand.Parameters.AddWithValue("PSZCustomernumber", item.PSZCustomernumber == null ? (object)DBNull.Value : item.PSZCustomernumber);
					sqlCommand.Parameters.AddWithValue("ReceivingDate", item.ReceivingDate == null ? (object)DBNull.Value : item.ReceivingDate);
					sqlCommand.Parameters.AddWithValue("RecipientId", item.RecipientId);
					sqlCommand.Parameters.AddWithValue("ReferenceNumber", item.ReferenceNumber);
					sqlCommand.Parameters.AddWithValue("ReferenceVersionNumber", item.ReferenceVersionNumber == null ? (object)DBNull.Value : item.ReferenceVersionNumber);
					sqlCommand.Parameters.AddWithValue("SenderId", item.SenderId);
					sqlCommand.Parameters.AddWithValue("SupplierCity", item.SupplierCity ?? "");
					sqlCommand.Parameters.AddWithValue("SupplierContactFax", item.SupplierContactFax ?? "");
					sqlCommand.Parameters.AddWithValue("SupplierContactTelephone", item.SupplierContactTelephone ?? "");
					sqlCommand.Parameters.AddWithValue("SupplierCountryName", item.SupplierCountryName ?? "");
					sqlCommand.Parameters.AddWithValue("SupplierDUNS", item.SupplierDUNS);
					sqlCommand.Parameters.AddWithValue("SupplierPartyIdentification", item.SupplierPartyIdentification);
					sqlCommand.Parameters.AddWithValue("SupplierPartyName", item.SupplierPartyName);
					sqlCommand.Parameters.AddWithValue("SupplierPostCode", item.SupplierPostCode);
					sqlCommand.Parameters.AddWithValue("SupplierStreet", item.SupplierStreet);
					sqlCommand.Parameters.AddWithValue("ValidFrom", item.ValidFrom == null ? (object)DBNull.Value : item.ValidFrom);
					sqlCommand.Parameters.AddWithValue("ValidTill", item.ValidTill == null ? (object)DBNull.Value : item.ValidTill);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? long.MinValue : long.TryParse(result.ToString(), out var insertedId) ? insertedId : long.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 43; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> items)
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
						query += " INSERT INTO [__EDI_DLF_Header] ([BuyerContactName],[BuyerContactTelephone],[BuyerDUNS],[BuyerPartyIdentification],[BuyerPartyName],[BuyerPurchasingDepartment],[ConsigneeCity],[ConsigneeContactFax],[ConsigneeContactTelephone],[ConsigneeCountryName],[ConsigneeDUNS],[ConsigneePartyIdentification],[ConsigneePartyName],[ConsigneePostCode],[ConsigneeStorageLocation],[ConsigneeStreet],[ConsigneeUnloadingPoint],[CreationTime],[DocumentNumber],[Done],[ManualCreation],[MessageReferenceNumber],[MessageType],[PreviousReferenceVersionNumber],[PSZCustomernumber],[ReceivingDate],[RecipientId],[ReferenceNumber],[ReferenceVersionNumber],[SenderId],[SupplierCity],[SupplierContactFax],[SupplierContactTelephone],[SupplierCountryName],[SupplierDUNS],[SupplierPartyIdentification],[SupplierPartyName],[SupplierPostCode],[SupplierStreet],[ValidFrom],[ValidTill]) VALUES ( "

							+ "@BuyerContactName" + i + ","
							+ "@BuyerContactTelephone" + i + ","
							+ "@BuyerDUNS" + i + ","
							+ "@BuyerPartyIdentification" + i + ","
							+ "@BuyerPartyName" + i + ","
							+ "@BuyerPurchasingDepartment" + i + ","
							+ "@ConsigneeCity" + i + ","
							+ "@ConsigneeContactFax" + i + ","
							+ "@ConsigneeContactTelephone" + i + ","
							+ "@ConsigneeCountryName" + i + ","
							+ "@ConsigneeDUNS" + i + ","
							+ "@ConsigneePartyIdentification" + i + ","
							+ "@ConsigneePartyName" + i + ","
							+ "@ConsigneePostCode" + i + ","
							+ "@ConsigneeStorageLocation" + i + ","
							+ "@ConsigneeStreet" + i + ","
							+ "@ConsigneeUnloadingPoint" + i + ","
							+ "@CreationTime" + i + ","
							+ "@DocumentNumber" + i + ","
							+ "@Done" + i + ","
							+ "@ManualCreation" + i + ","
							+ "@MessageReferenceNumber" + i + ","
							+ "@MessageType" + i + ","
							+ "@PreviousReferenceVersionNumber" + i + ","
							+ "@PSZCustomernumber" + i + ","
							+ "@ReceivingDate" + i + ","
							+ "@RecipientId" + i + ","
							+ "@ReferenceNumber" + i + ","
							+ "@ReferenceVersionNumber" + i + ","
							+ "@SenderId" + i + ","
							+ "@SupplierCity" + i + ","
							+ "@SupplierContactFax" + i + ","
							+ "@SupplierContactTelephone" + i + ","
							+ "@SupplierCountryName" + i + ","
							+ "@SupplierDUNS" + i + ","
							+ "@SupplierPartyIdentification" + i + ","
							+ "@SupplierPartyName" + i + ","
							+ "@SupplierPostCode" + i + ","
							+ "@SupplierStreet" + i + ","
							+ "@ValidFrom" + i + ","
							+ "@ValidTill" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("BuyerContactName" + i, item.BuyerContactName);
						sqlCommand.Parameters.AddWithValue("BuyerContactTelephone" + i, item.BuyerContactTelephone);
						sqlCommand.Parameters.AddWithValue("BuyerDUNS" + i, item.BuyerDUNS);
						sqlCommand.Parameters.AddWithValue("BuyerPartyIdentification" + i, item.BuyerPartyIdentification == null ? (object)DBNull.Value : item.BuyerPartyIdentification);
						sqlCommand.Parameters.AddWithValue("BuyerPartyName" + i, item.BuyerPartyName);
						sqlCommand.Parameters.AddWithValue("BuyerPurchasingDepartment" + i, item.BuyerPurchasingDepartment ?? "");
						sqlCommand.Parameters.AddWithValue("ConsigneeCity" + i, item.ConsigneeCity);
						sqlCommand.Parameters.AddWithValue("ConsigneeContactFax" + i, item.ConsigneeContactFax ?? "");
						sqlCommand.Parameters.AddWithValue("ConsigneeContactTelephone" + i, item.ConsigneeContactTelephone ?? "");
						sqlCommand.Parameters.AddWithValue("ConsigneeCountryName" + i, item.ConsigneeCountryName);
						sqlCommand.Parameters.AddWithValue("ConsigneeDUNS" + i, item.ConsigneeDUNS);
						sqlCommand.Parameters.AddWithValue("ConsigneePartyIdentification" + i, item.ConsigneePartyIdentification);
						sqlCommand.Parameters.AddWithValue("ConsigneePartyName" + i, item.ConsigneePartyName);
						sqlCommand.Parameters.AddWithValue("ConsigneePostCode" + i, item.ConsigneePostCode);
						sqlCommand.Parameters.AddWithValue("ConsigneeStorageLocation" + i, item.ConsigneeStorageLocation == null ? (object)DBNull.Value : item.ConsigneeStorageLocation);
						sqlCommand.Parameters.AddWithValue("ConsigneeStreet" + i, item.ConsigneeStreet);
						sqlCommand.Parameters.AddWithValue("ConsigneeUnloadingPoint" + i, item.ConsigneeUnloadingPoint == null ? (object)DBNull.Value : item.ConsigneeUnloadingPoint);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("DocumentNumber" + i, item.DocumentNumber);
						sqlCommand.Parameters.AddWithValue("Done" + i, item.Done == null ? (object)DBNull.Value : item.Done);
						sqlCommand.Parameters.AddWithValue("ManualCreation" + i, item.ManualCreation == null ? (object)DBNull.Value : item.ManualCreation);
						sqlCommand.Parameters.AddWithValue("MessageReferenceNumber" + i, item.MessageReferenceNumber);
						sqlCommand.Parameters.AddWithValue("MessageType" + i, item.MessageType);
						sqlCommand.Parameters.AddWithValue("PreviousReferenceVersionNumber" + i, item.PreviousReferenceVersionNumber == null ? (object)DBNull.Value : item.PreviousReferenceVersionNumber);
						sqlCommand.Parameters.AddWithValue("PSZCustomernumber" + i, item.PSZCustomernumber == null ? (object)DBNull.Value : item.PSZCustomernumber);
						sqlCommand.Parameters.AddWithValue("ReceivingDate" + i, item.ReceivingDate == null ? (object)DBNull.Value : item.ReceivingDate);
						sqlCommand.Parameters.AddWithValue("RecipientId" + i, item.RecipientId);
						sqlCommand.Parameters.AddWithValue("ReferenceNumber" + i, item.ReferenceNumber);
						sqlCommand.Parameters.AddWithValue("ReferenceVersionNumber" + i, item.ReferenceVersionNumber == null ? (object)DBNull.Value : item.ReferenceVersionNumber);
						sqlCommand.Parameters.AddWithValue("SenderId" + i, item.SenderId);
						sqlCommand.Parameters.AddWithValue("SupplierCity" + i, item.SupplierCity);
						sqlCommand.Parameters.AddWithValue("SupplierContactFax" + i, item.SupplierContactFax);
						sqlCommand.Parameters.AddWithValue("SupplierContactTelephone" + i, item.SupplierContactTelephone);
						sqlCommand.Parameters.AddWithValue("SupplierCountryName" + i, item.SupplierCountryName);
						sqlCommand.Parameters.AddWithValue("SupplierDUNS" + i, item.SupplierDUNS);
						sqlCommand.Parameters.AddWithValue("SupplierPartyIdentification" + i, item.SupplierPartyIdentification);
						sqlCommand.Parameters.AddWithValue("SupplierPartyName" + i, item.SupplierPartyName);
						sqlCommand.Parameters.AddWithValue("SupplierPostCode" + i, item.SupplierPostCode);
						sqlCommand.Parameters.AddWithValue("SupplierStreet" + i, item.SupplierStreet);
						sqlCommand.Parameters.AddWithValue("ValidFrom" + i, item.ValidFrom == null ? (object)DBNull.Value : item.ValidFrom);
						sqlCommand.Parameters.AddWithValue("ValidTill" + i, item.ValidTill == null ? (object)DBNull.Value : item.ValidTill);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.HeaderEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__EDI_DLF_Header] SET [BuyerContactName]=@BuyerContactName, [BuyerContactTelephone]=@BuyerContactTelephone, [BuyerDUNS]=@BuyerDUNS, [BuyerPartyIdentification]=@BuyerPartyIdentification, [BuyerPartyName]=@BuyerPartyName, [BuyerPurchasingDepartment]=@BuyerPurchasingDepartment, [ConsigneeCity]=@ConsigneeCity, [ConsigneeContactFax]=@ConsigneeContactFax, [ConsigneeContactTelephone]=@ConsigneeContactTelephone, [ConsigneeCountryName]=@ConsigneeCountryName, [ConsigneeDUNS]=@ConsigneeDUNS, [ConsigneePartyIdentification]=@ConsigneePartyIdentification, [ConsigneePartyName]=@ConsigneePartyName, [ConsigneePostCode]=@ConsigneePostCode, [ConsigneeStorageLocation]=@ConsigneeStorageLocation, [ConsigneeStreet]=@ConsigneeStreet, [ConsigneeUnloadingPoint]=@ConsigneeUnloadingPoint, [CreationTime]=@CreationTime, [DocumentNumber]=@DocumentNumber, [Done]=@Done, [ManualCreation]=@ManualCreation, [MessageReferenceNumber]=@MessageReferenceNumber, [MessageType]=@MessageType, [PreviousReferenceVersionNumber]=@PreviousReferenceVersionNumber, [PSZCustomernumber]=@PSZCustomernumber, [ReceivingDate]=@ReceivingDate, [RecipientId]=@RecipientId, [ReferenceNumber]=@ReferenceNumber, [ReferenceVersionNumber]=@ReferenceVersionNumber, [SenderId]=@SenderId, [SupplierCity]=@SupplierCity, [SupplierContactFax]=@SupplierContactFax, [SupplierContactTelephone]=@SupplierContactTelephone, [SupplierCountryName]=@SupplierCountryName, [SupplierDUNS]=@SupplierDUNS, [SupplierPartyIdentification]=@SupplierPartyIdentification, [SupplierPartyName]=@SupplierPartyName, [SupplierPostCode]=@SupplierPostCode, [SupplierStreet]=@SupplierStreet, [ValidFrom]=@ValidFrom, [ValidTill]=@ValidTill WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("BuyerContactName", item.BuyerContactName);
				sqlCommand.Parameters.AddWithValue("BuyerContactTelephone", item.BuyerContactTelephone);
				sqlCommand.Parameters.AddWithValue("BuyerDUNS", item.BuyerDUNS);
				sqlCommand.Parameters.AddWithValue("BuyerPartyIdentification", item.BuyerPartyIdentification == null ? (object)DBNull.Value : item.BuyerPartyIdentification);
				sqlCommand.Parameters.AddWithValue("BuyerPartyName", item.BuyerPartyName);
				sqlCommand.Parameters.AddWithValue("BuyerPurchasingDepartment", item.BuyerPurchasingDepartment ?? "");
				sqlCommand.Parameters.AddWithValue("ConsigneeCity", item.ConsigneeCity);
				sqlCommand.Parameters.AddWithValue("ConsigneeContactFax", item.ConsigneeContactFax ?? "");
				sqlCommand.Parameters.AddWithValue("ConsigneeContactTelephone", item.ConsigneeContactTelephone ?? "");
				sqlCommand.Parameters.AddWithValue("ConsigneeCountryName", item.ConsigneeCountryName);
				sqlCommand.Parameters.AddWithValue("ConsigneeDUNS", item.ConsigneeDUNS);
				sqlCommand.Parameters.AddWithValue("ConsigneePartyIdentification", item.ConsigneePartyIdentification);
				sqlCommand.Parameters.AddWithValue("ConsigneePartyName", item.ConsigneePartyName);
				sqlCommand.Parameters.AddWithValue("ConsigneePostCode", item.ConsigneePostCode);
				sqlCommand.Parameters.AddWithValue("ConsigneeStorageLocation", item.ConsigneeStorageLocation == null ? (object)DBNull.Value : item.ConsigneeStorageLocation);
				sqlCommand.Parameters.AddWithValue("ConsigneeStreet", item.ConsigneeStreet);
				sqlCommand.Parameters.AddWithValue("ConsigneeUnloadingPoint", item.ConsigneeUnloadingPoint == null ? (object)DBNull.Value : item.ConsigneeUnloadingPoint);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
				sqlCommand.Parameters.AddWithValue("DocumentNumber", item.DocumentNumber);
				sqlCommand.Parameters.AddWithValue("Done", item.Done == null ? (object)DBNull.Value : item.Done);
				sqlCommand.Parameters.AddWithValue("ManualCreation", item.ManualCreation == null ? (object)DBNull.Value : item.ManualCreation);
				sqlCommand.Parameters.AddWithValue("MessageReferenceNumber", item.MessageReferenceNumber);
				sqlCommand.Parameters.AddWithValue("MessageType", item.MessageType);
				sqlCommand.Parameters.AddWithValue("PreviousReferenceVersionNumber", item.PreviousReferenceVersionNumber == null ? (object)DBNull.Value : item.PreviousReferenceVersionNumber);
				sqlCommand.Parameters.AddWithValue("PSZCustomernumber", item.PSZCustomernumber == null ? (object)DBNull.Value : item.PSZCustomernumber);
				sqlCommand.Parameters.AddWithValue("ReceivingDate", item.ReceivingDate == null ? (object)DBNull.Value : item.ReceivingDate);
				sqlCommand.Parameters.AddWithValue("RecipientId", item.RecipientId);
				sqlCommand.Parameters.AddWithValue("ReferenceNumber", item.ReferenceNumber);
				sqlCommand.Parameters.AddWithValue("ReferenceVersionNumber", item.ReferenceVersionNumber == null ? (object)DBNull.Value : item.ReferenceVersionNumber);
				sqlCommand.Parameters.AddWithValue("SenderId", item.SenderId);
				sqlCommand.Parameters.AddWithValue("SupplierCity", item.SupplierCity ?? "");
				sqlCommand.Parameters.AddWithValue("SupplierContactFax", item.SupplierContactFax ?? "");
				sqlCommand.Parameters.AddWithValue("SupplierContactTelephone", item.SupplierContactTelephone ?? "");
				sqlCommand.Parameters.AddWithValue("SupplierCountryName", item.SupplierCountryName ?? "");
				sqlCommand.Parameters.AddWithValue("SupplierDUNS", item.SupplierDUNS);
				sqlCommand.Parameters.AddWithValue("SupplierPartyIdentification", item.SupplierPartyIdentification);
				sqlCommand.Parameters.AddWithValue("SupplierPartyName", item.SupplierPartyName);
				sqlCommand.Parameters.AddWithValue("SupplierPostCode", item.SupplierPostCode);
				sqlCommand.Parameters.AddWithValue("SupplierStreet", item.SupplierStreet);
				sqlCommand.Parameters.AddWithValue("ValidFrom", item.ValidFrom == null ? (object)DBNull.Value : item.ValidFrom);
				sqlCommand.Parameters.AddWithValue("ValidTill", item.ValidTill == null ? (object)DBNull.Value : item.ValidTill);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 43; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> items)
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
						query += " UPDATE [__EDI_DLF_Header] SET "

							+ "[BuyerContactName]=@BuyerContactName" + i + ","
							+ "[BuyerContactTelephone]=@BuyerContactTelephone" + i + ","
							+ "[BuyerDUNS]=@BuyerDUNS" + i + ","
							+ "[BuyerPartyIdentification]=@BuyerPartyIdentification" + i + ","
							+ "[BuyerPartyName]=@BuyerPartyName" + i + ","
							+ "[BuyerPurchasingDepartment]=@BuyerPurchasingDepartment" + i + ","
							+ "[ConsigneeCity]=@ConsigneeCity" + i + ","
							+ "[ConsigneeContactFax]=@ConsigneeContactFax" + i + ","
							+ "[ConsigneeContactTelephone]=@ConsigneeContactTelephone" + i + ","
							+ "[ConsigneeCountryName]=@ConsigneeCountryName" + i + ","
							+ "[ConsigneeDUNS]=@ConsigneeDUNS" + i + ","
							+ "[ConsigneePartyIdentification]=@ConsigneePartyIdentification" + i + ","
							+ "[ConsigneePartyName]=@ConsigneePartyName" + i + ","
							+ "[ConsigneePostCode]=@ConsigneePostCode" + i + ","
							+ "[ConsigneeStorageLocation]=@ConsigneeStorageLocation" + i + ","
							+ "[ConsigneeStreet]=@ConsigneeStreet" + i + ","
							+ "[ConsigneeUnloadingPoint]=@ConsigneeUnloadingPoint" + i + ","
							+ "[CreationTime]=@CreationTime" + i + ","
							+ "[DocumentNumber]=@DocumentNumber" + i + ","
							+ "[Done]=@Done" + i + ","
							+ "[ManualCreation]=@ManualCreation" + i + ","
							+ "[MessageReferenceNumber]=@MessageReferenceNumber" + i + ","
							+ "[MessageType]=@MessageType" + i + ","
							+ "[PreviousReferenceVersionNumber]=@PreviousReferenceVersionNumber" + i + ","
							+ "[PSZCustomernumber]=@PSZCustomernumber" + i + ","
							+ "[ReceivingDate]=@ReceivingDate" + i + ","
							+ "[RecipientId]=@RecipientId" + i + ","
							+ "[ReferenceNumber]=@ReferenceNumber" + i + ","
							+ "[ReferenceVersionNumber]=@ReferenceVersionNumber" + i + ","
							+ "[SenderId]=@SenderId" + i + ","
							+ "[SupplierCity]=@SupplierCity" + i + ","
							+ "[SupplierContactFax]=@SupplierContactFax" + i + ","
							+ "[SupplierContactTelephone]=@SupplierContactTelephone" + i + ","
							+ "[SupplierCountryName]=@SupplierCountryName" + i + ","
							+ "[SupplierDUNS]=@SupplierDUNS" + i + ","
							+ "[SupplierPartyIdentification]=@SupplierPartyIdentification" + i + ","
							+ "[SupplierPartyName]=@SupplierPartyName" + i + ","
							+ "[SupplierPostCode]=@SupplierPostCode" + i + ","
							+ "[SupplierStreet]=@SupplierStreet" + i + ","
							+ "[ValidFrom]=@ValidFrom" + i + ","
							+ "[ValidTill]=@ValidTill" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("BuyerContactName" + i, item.BuyerContactName);
						sqlCommand.Parameters.AddWithValue("BuyerContactTelephone" + i, item.BuyerContactTelephone);
						sqlCommand.Parameters.AddWithValue("BuyerDUNS" + i, item.BuyerDUNS);
						sqlCommand.Parameters.AddWithValue("BuyerPartyIdentification" + i, item.BuyerPartyIdentification == null ? (object)DBNull.Value : item.BuyerPartyIdentification);
						sqlCommand.Parameters.AddWithValue("BuyerPartyName" + i, item.BuyerPartyName);
						sqlCommand.Parameters.AddWithValue("BuyerPurchasingDepartment" + i, item.BuyerPurchasingDepartment ?? "");
						sqlCommand.Parameters.AddWithValue("ConsigneeCity" + i, item.ConsigneeCity);
						sqlCommand.Parameters.AddWithValue("ConsigneeContactFax" + i, item.ConsigneeContactFax ?? "");
						sqlCommand.Parameters.AddWithValue("ConsigneeContactTelephone" + i, item.ConsigneeContactTelephone ?? "");
						sqlCommand.Parameters.AddWithValue("ConsigneeCountryName" + i, item.ConsigneeCountryName);
						sqlCommand.Parameters.AddWithValue("ConsigneeDUNS" + i, item.ConsigneeDUNS);
						sqlCommand.Parameters.AddWithValue("ConsigneePartyIdentification" + i, item.ConsigneePartyIdentification);
						sqlCommand.Parameters.AddWithValue("ConsigneePartyName" + i, item.ConsigneePartyName);
						sqlCommand.Parameters.AddWithValue("ConsigneePostCode" + i, item.ConsigneePostCode);
						sqlCommand.Parameters.AddWithValue("ConsigneeStorageLocation" + i, item.ConsigneeStorageLocation == null ? (object)DBNull.Value : item.ConsigneeStorageLocation);
						sqlCommand.Parameters.AddWithValue("ConsigneeStreet" + i, item.ConsigneeStreet);
						sqlCommand.Parameters.AddWithValue("ConsigneeUnloadingPoint" + i, item.ConsigneeUnloadingPoint == null ? (object)DBNull.Value : item.ConsigneeUnloadingPoint);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("DocumentNumber" + i, item.DocumentNumber);
						sqlCommand.Parameters.AddWithValue("Done" + i, item.Done == null ? (object)DBNull.Value : item.Done);
						sqlCommand.Parameters.AddWithValue("ManualCreation" + i, item.ManualCreation == null ? (object)DBNull.Value : item.ManualCreation);
						sqlCommand.Parameters.AddWithValue("MessageReferenceNumber" + i, item.MessageReferenceNumber);
						sqlCommand.Parameters.AddWithValue("MessageType" + i, item.MessageType);
						sqlCommand.Parameters.AddWithValue("PreviousReferenceVersionNumber" + i, item.PreviousReferenceVersionNumber == null ? (object)DBNull.Value : item.PreviousReferenceVersionNumber);
						sqlCommand.Parameters.AddWithValue("PSZCustomernumber" + i, item.PSZCustomernumber == null ? (object)DBNull.Value : item.PSZCustomernumber);
						sqlCommand.Parameters.AddWithValue("ReceivingDate" + i, item.ReceivingDate == null ? (object)DBNull.Value : item.ReceivingDate);
						sqlCommand.Parameters.AddWithValue("RecipientId" + i, item.RecipientId);
						sqlCommand.Parameters.AddWithValue("ReferenceNumber" + i, item.ReferenceNumber);
						sqlCommand.Parameters.AddWithValue("ReferenceVersionNumber" + i, item.ReferenceVersionNumber == null ? (object)DBNull.Value : item.ReferenceVersionNumber);
						sqlCommand.Parameters.AddWithValue("SenderId" + i, item.SenderId);
						sqlCommand.Parameters.AddWithValue("SupplierCity" + i, item.SupplierCity);
						sqlCommand.Parameters.AddWithValue("SupplierContactFax" + i, item.SupplierContactFax);
						sqlCommand.Parameters.AddWithValue("SupplierContactTelephone" + i, item.SupplierContactTelephone);
						sqlCommand.Parameters.AddWithValue("SupplierCountryName" + i, item.SupplierCountryName);
						sqlCommand.Parameters.AddWithValue("SupplierDUNS" + i, item.SupplierDUNS);
						sqlCommand.Parameters.AddWithValue("SupplierPartyIdentification" + i, item.SupplierPartyIdentification);
						sqlCommand.Parameters.AddWithValue("SupplierPartyName" + i, item.SupplierPartyName);
						sqlCommand.Parameters.AddWithValue("SupplierPostCode" + i, item.SupplierPostCode);
						sqlCommand.Parameters.AddWithValue("SupplierStreet" + i, item.SupplierStreet);
						sqlCommand.Parameters.AddWithValue("ValidFrom" + i, item.ValidFrom == null ? (object)DBNull.Value : item.ValidFrom);
						sqlCommand.Parameters.AddWithValue("ValidTill" + i, item.ValidTill == null ? (object)DBNull.Value : item.ValidTill);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Delete(long id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__EDI_DLF_Header] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Delete(List<long> ids)
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
		private static int delete(List<long> ids)
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

					string query = "DELETE FROM [__EDI_DLF_Header] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CTS.HeaderEntity GetWithTransaction(long id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__EDI_DLF_Header] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.HeaderEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__EDI_DLF_Header]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.HeaderEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> GetWithTransaction(List<long> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> getWithTransaction(List<long> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();

				var sqlCommand = new SqlCommand("", connection, transaction);
				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				sqlCommand.CommandText = $"SELECT * FROM [__EDI_DLF_Header] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.HeaderEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity>();
		}

		public static long InsertWithTransaction(Infrastructure.Data.Entities.Tables.CTS.HeaderEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			long response = long.MinValue;


			string query = "INSERT INTO [__EDI_DLF_Header] ([BuyerContactName],[BuyerContactTelephone],[BuyerDUNS],[BuyerPartyIdentification],[BuyerPartyName],[BuyerPurchasingDepartment],[ConsigneeCity],[ConsigneeContactFax],[ConsigneeContactTelephone],[ConsigneeCountryName],[ConsigneeDUNS],[ConsigneePartyIdentification],[ConsigneePartyName],[ConsigneePostCode],[ConsigneeStorageLocation],[ConsigneeStreet],[ConsigneeUnloadingPoint],[CreationTime],[DocumentNumber],[Done],[ManualCreation],[MessageReferenceNumber],[MessageType],[PreviousReferenceVersionNumber],[PSZCustomernumber],[ReceivingDate],[RecipientId],[ReferenceNumber],[ReferenceVersionNumber],[SenderId],[SupplierCity],[SupplierContactFax],[SupplierContactTelephone],[SupplierCountryName],[SupplierDUNS],[SupplierPartyIdentification],[SupplierPartyName],[SupplierPostCode],[SupplierStreet],[ValidFrom],[ValidTill]) OUTPUT INSERTED.[Id] VALUES (@BuyerContactName,@BuyerContactTelephone,@BuyerDUNS,@BuyerPartyIdentification,@BuyerPartyName,@BuyerPurchasingDepartment,@ConsigneeCity,@ConsigneeContactFax,@ConsigneeContactTelephone,@ConsigneeCountryName,@ConsigneeDUNS,@ConsigneePartyIdentification,@ConsigneePartyName,@ConsigneePostCode,@ConsigneeStorageLocation,@ConsigneeStreet,@ConsigneeUnloadingPoint,@CreationTime,@DocumentNumber,@Done,@ManualCreation,@MessageReferenceNumber,@MessageType,@PreviousReferenceVersionNumber,@PSZCustomernumber,@ReceivingDate,@RecipientId,@ReferenceNumber,@ReferenceVersionNumber,@SenderId,@SupplierCity,@SupplierContactFax,@SupplierContactTelephone,@SupplierCountryName,@SupplierDUNS,@SupplierPartyIdentification,@SupplierPartyName,@SupplierPostCode,@SupplierStreet,@ValidFrom,@ValidTill); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("BuyerContactName", item.BuyerContactName);
			sqlCommand.Parameters.AddWithValue("BuyerContactTelephone", item.BuyerContactTelephone);
			sqlCommand.Parameters.AddWithValue("BuyerDUNS", item.BuyerDUNS);
			sqlCommand.Parameters.AddWithValue("BuyerPartyIdentification", item.BuyerPartyIdentification == null ? (object)DBNull.Value : item.BuyerPartyIdentification);
			sqlCommand.Parameters.AddWithValue("BuyerPartyName", item.BuyerPartyName);
			sqlCommand.Parameters.AddWithValue("BuyerPurchasingDepartment", item.BuyerPurchasingDepartment ?? "");
			sqlCommand.Parameters.AddWithValue("ConsigneeCity", item.ConsigneeCity);
			sqlCommand.Parameters.AddWithValue("ConsigneeContactFax", item.ConsigneeContactFax ?? "");
			sqlCommand.Parameters.AddWithValue("ConsigneeContactTelephone", item.ConsigneeContactTelephone ?? "");
			sqlCommand.Parameters.AddWithValue("ConsigneeCountryName", item.ConsigneeCountryName);
			sqlCommand.Parameters.AddWithValue("ConsigneeDUNS", item.ConsigneeDUNS ?? "");
			sqlCommand.Parameters.AddWithValue("ConsigneePartyIdentification", item.ConsigneePartyIdentification);
			sqlCommand.Parameters.AddWithValue("ConsigneePartyName", item.ConsigneePartyName);
			sqlCommand.Parameters.AddWithValue("ConsigneePostCode", item.ConsigneePostCode);
			sqlCommand.Parameters.AddWithValue("ConsigneeStorageLocation", item.ConsigneeStorageLocation == null ? (object)DBNull.Value : item.ConsigneeStorageLocation);
			sqlCommand.Parameters.AddWithValue("ConsigneeStreet", item.ConsigneeStreet);
			sqlCommand.Parameters.AddWithValue("ConsigneeUnloadingPoint", item.ConsigneeUnloadingPoint == null ? (object)DBNull.Value : item.ConsigneeUnloadingPoint);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
			sqlCommand.Parameters.AddWithValue("DocumentNumber", item.DocumentNumber);
			sqlCommand.Parameters.AddWithValue("Done", item.Done == null ? (object)DBNull.Value : item.Done);
			sqlCommand.Parameters.AddWithValue("ManualCreation", item.ManualCreation == null ? (object)DBNull.Value : item.ManualCreation);
			sqlCommand.Parameters.AddWithValue("MessageReferenceNumber", item.MessageReferenceNumber);
			sqlCommand.Parameters.AddWithValue("MessageType", item.MessageType);
			sqlCommand.Parameters.AddWithValue("PreviousReferenceVersionNumber", item.PreviousReferenceVersionNumber == null ? (object)DBNull.Value : item.PreviousReferenceVersionNumber);
			sqlCommand.Parameters.AddWithValue("PSZCustomernumber", item.PSZCustomernumber == null ? (object)DBNull.Value : item.PSZCustomernumber);
			sqlCommand.Parameters.AddWithValue("ReceivingDate", item.ReceivingDate == null ? (object)DBNull.Value : item.ReceivingDate);
			sqlCommand.Parameters.AddWithValue("RecipientId", item.RecipientId);
			sqlCommand.Parameters.AddWithValue("ReferenceNumber", item.ReferenceNumber);
			sqlCommand.Parameters.AddWithValue("ReferenceVersionNumber", item.ReferenceVersionNumber == null ? (object)DBNull.Value : item.ReferenceVersionNumber);
			sqlCommand.Parameters.AddWithValue("SenderId", item.SenderId);
			sqlCommand.Parameters.AddWithValue("SupplierCity", item.SupplierCity ?? "");
			sqlCommand.Parameters.AddWithValue("SupplierContactFax", item.SupplierContactFax ?? "");
			sqlCommand.Parameters.AddWithValue("SupplierContactTelephone", item.SupplierContactTelephone ?? "");
			sqlCommand.Parameters.AddWithValue("SupplierCountryName", item.SupplierCountryName ?? "");
			sqlCommand.Parameters.AddWithValue("SupplierDUNS", item.SupplierDUNS);
			sqlCommand.Parameters.AddWithValue("SupplierPartyIdentification", item.SupplierPartyIdentification);
			sqlCommand.Parameters.AddWithValue("SupplierPartyName", item.SupplierPartyName);
			sqlCommand.Parameters.AddWithValue("SupplierPostCode", item.SupplierPostCode);
			sqlCommand.Parameters.AddWithValue("SupplierStreet", item.SupplierStreet);
			sqlCommand.Parameters.AddWithValue("ValidFrom", item.ValidFrom == null ? (object)DBNull.Value : item.ValidFrom);
			sqlCommand.Parameters.AddWithValue("ValidTill", item.ValidTill == null ? (object)DBNull.Value : item.ValidTill);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? long.MinValue : long.TryParse(result.ToString(), out var insertedId) ? insertedId : long.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 43; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__EDI_DLF_Header] ([BuyerContactName],[BuyerContactTelephone],[BuyerDUNS],[BuyerPartyIdentification],[BuyerPartyName],[BuyerPurchasingDepartment],[ConsigneeCity],[ConsigneeContactFax],[ConsigneeContactTelephone],[ConsigneeCountryName],[ConsigneeDUNS],[ConsigneePartyIdentification],[ConsigneePartyName],[ConsigneePostCode],[ConsigneeStorageLocation],[ConsigneeStreet],[ConsigneeUnloadingPoint],[CreationTime],[DocumentNumber],[Done],[ManualCreation],[MessageReferenceNumber],[MessageType],[PreviousReferenceVersionNumber],[PSZCustomernumber],[ReceivingDate],[RecipientId],[ReferenceNumber],[ReferenceVersionNumber],[SenderId],[SupplierCity],[SupplierContactFax],[SupplierContactTelephone],[SupplierCountryName],[SupplierDUNS],[SupplierPartyIdentification],[SupplierPartyName],[SupplierPostCode],[SupplierStreet],[ValidFrom],[ValidTill]) VALUES ( "

						+ "@BuyerContactName" + i + ","
						+ "@BuyerContactTelephone" + i + ","
						+ "@BuyerDUNS" + i + ","
						+ "@BuyerPartyIdentification" + i + ","
						+ "@BuyerPartyName" + i + ","
						+ "@BuyerPurchasingDepartment" + i + ","
						+ "@ConsigneeCity" + i + ","
						+ "@ConsigneeContactFax" + i + ","
						+ "@ConsigneeContactTelephone" + i + ","
						+ "@ConsigneeCountryName" + i + ","
						+ "@ConsigneeDUNS" + i + ","
						+ "@ConsigneePartyIdentification" + i + ","
						+ "@ConsigneePartyName" + i + ","
						+ "@ConsigneePostCode" + i + ","
						+ "@ConsigneeStorageLocation" + i + ","
						+ "@ConsigneeStreet" + i + ","
						+ "@ConsigneeUnloadingPoint" + i + ","
						+ "@CreationTime" + i + ","
						+ "@DocumentNumber" + i + ","
						+ "@Done" + i + ","
						+ "@ManualCreation" + i + ","
						+ "@MessageReferenceNumber" + i + ","
						+ "@MessageType" + i + ","
						+ "@PreviousReferenceVersionNumber" + i + ","
						+ "@PSZCustomernumber" + i + ","
						+ "@ReceivingDate" + i + ","
						+ "@RecipientId" + i + ","
						+ "@ReferenceNumber" + i + ","
						+ "@ReferenceVersionNumber" + i + ","
						+ "@SenderId" + i + ","
						+ "@SupplierCity" + i + ","
						+ "@SupplierContactFax" + i + ","
						+ "@SupplierContactTelephone" + i + ","
						+ "@SupplierCountryName" + i + ","
						+ "@SupplierDUNS" + i + ","
						+ "@SupplierPartyIdentification" + i + ","
						+ "@SupplierPartyName" + i + ","
						+ "@SupplierPostCode" + i + ","
						+ "@SupplierStreet" + i + ","
						+ "@ValidFrom" + i + ","
						+ "@ValidTill" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("BuyerContactName" + i, item.BuyerContactName);
					sqlCommand.Parameters.AddWithValue("BuyerContactTelephone" + i, item.BuyerContactTelephone);
					sqlCommand.Parameters.AddWithValue("BuyerDUNS" + i, item.BuyerDUNS);
					sqlCommand.Parameters.AddWithValue("BuyerPartyIdentification" + i, item.BuyerPartyIdentification == null ? (object)DBNull.Value : item.BuyerPartyIdentification);
					sqlCommand.Parameters.AddWithValue("BuyerPartyName" + i, item.BuyerPartyName);
					sqlCommand.Parameters.AddWithValue("BuyerPurchasingDepartment" + i, item.BuyerPurchasingDepartment ?? "");
					sqlCommand.Parameters.AddWithValue("ConsigneeCity" + i, item.ConsigneeCity);
					sqlCommand.Parameters.AddWithValue("ConsigneeContactFax" + i, item.ConsigneeContactFax ?? "");
					sqlCommand.Parameters.AddWithValue("ConsigneeContactTelephone" + i, item.ConsigneeContactTelephone ?? "");
					sqlCommand.Parameters.AddWithValue("ConsigneeCountryName" + i, item.ConsigneeCountryName);
					sqlCommand.Parameters.AddWithValue("ConsigneeDUNS" + i, item.ConsigneeDUNS);
					sqlCommand.Parameters.AddWithValue("ConsigneePartyIdentification" + i, item.ConsigneePartyIdentification);
					sqlCommand.Parameters.AddWithValue("ConsigneePartyName" + i, item.ConsigneePartyName);
					sqlCommand.Parameters.AddWithValue("ConsigneePostCode" + i, item.ConsigneePostCode);
					sqlCommand.Parameters.AddWithValue("ConsigneeStorageLocation" + i, item.ConsigneeStorageLocation == null ? (object)DBNull.Value : item.ConsigneeStorageLocation);
					sqlCommand.Parameters.AddWithValue("ConsigneeStreet" + i, item.ConsigneeStreet);
					sqlCommand.Parameters.AddWithValue("ConsigneeUnloadingPoint" + i, item.ConsigneeUnloadingPoint == null ? (object)DBNull.Value : item.ConsigneeUnloadingPoint);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("DocumentNumber" + i, item.DocumentNumber);
					sqlCommand.Parameters.AddWithValue("Done" + i, item.Done == null ? (object)DBNull.Value : item.Done);
					sqlCommand.Parameters.AddWithValue("ManualCreation" + i, item.ManualCreation == null ? (object)DBNull.Value : item.ManualCreation);
					sqlCommand.Parameters.AddWithValue("MessageReferenceNumber" + i, item.MessageReferenceNumber);
					sqlCommand.Parameters.AddWithValue("MessageType" + i, item.MessageType);
					sqlCommand.Parameters.AddWithValue("PreviousReferenceVersionNumber" + i, item.PreviousReferenceVersionNumber == null ? (object)DBNull.Value : item.PreviousReferenceVersionNumber);
					sqlCommand.Parameters.AddWithValue("PSZCustomernumber" + i, item.PSZCustomernumber == null ? (object)DBNull.Value : item.PSZCustomernumber);
					sqlCommand.Parameters.AddWithValue("ReceivingDate" + i, item.ReceivingDate == null ? (object)DBNull.Value : item.ReceivingDate);
					sqlCommand.Parameters.AddWithValue("RecipientId" + i, item.RecipientId);
					sqlCommand.Parameters.AddWithValue("ReferenceNumber" + i, item.ReferenceNumber);
					sqlCommand.Parameters.AddWithValue("ReferenceVersionNumber" + i, item.ReferenceVersionNumber == null ? (object)DBNull.Value : item.ReferenceVersionNumber);
					sqlCommand.Parameters.AddWithValue("SenderId" + i, item.SenderId);
					sqlCommand.Parameters.AddWithValue("SupplierCity" + i, item.SupplierCity);
					sqlCommand.Parameters.AddWithValue("SupplierContactFax" + i, item.SupplierContactFax);
					sqlCommand.Parameters.AddWithValue("SupplierContactTelephone" + i, item.SupplierContactTelephone);
					sqlCommand.Parameters.AddWithValue("SupplierCountryName" + i, item.SupplierCountryName);
					sqlCommand.Parameters.AddWithValue("SupplierDUNS" + i, item.SupplierDUNS);
					sqlCommand.Parameters.AddWithValue("SupplierPartyIdentification" + i, item.SupplierPartyIdentification);
					sqlCommand.Parameters.AddWithValue("SupplierPartyName" + i, item.SupplierPartyName);
					sqlCommand.Parameters.AddWithValue("SupplierPostCode" + i, item.SupplierPostCode);
					sqlCommand.Parameters.AddWithValue("SupplierStreet" + i, item.SupplierStreet);
					sqlCommand.Parameters.AddWithValue("ValidFrom" + i, item.ValidFrom == null ? (object)DBNull.Value : item.ValidFrom);
					sqlCommand.Parameters.AddWithValue("ValidTill" + i, item.ValidTill == null ? (object)DBNull.Value : item.ValidTill);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CTS.HeaderEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__EDI_DLF_Header] SET [BuyerContactName]=@BuyerContactName, [BuyerContactTelephone]=@BuyerContactTelephone, [BuyerDUNS]=@BuyerDUNS, [BuyerPartyIdentification]=@BuyerPartyIdentification, [BuyerPartyName]=@BuyerPartyName, [BuyerPurchasingDepartment]=@BuyerPurchasingDepartment, [ConsigneeCity]=@ConsigneeCity, [ConsigneeContactFax]=@ConsigneeContactFax, [ConsigneeContactTelephone]=@ConsigneeContactTelephone, [ConsigneeCountryName]=@ConsigneeCountryName, [ConsigneeDUNS]=@ConsigneeDUNS, [ConsigneePartyIdentification]=@ConsigneePartyIdentification, [ConsigneePartyName]=@ConsigneePartyName, [ConsigneePostCode]=@ConsigneePostCode, [ConsigneeStorageLocation]=@ConsigneeStorageLocation, [ConsigneeStreet]=@ConsigneeStreet, [ConsigneeUnloadingPoint]=@ConsigneeUnloadingPoint, [CreationTime]=@CreationTime, [DocumentNumber]=@DocumentNumber, [Done]=@Done, [ManualCreation]=@ManualCreation, [MessageReferenceNumber]=@MessageReferenceNumber, [MessageType]=@MessageType, [PreviousReferenceVersionNumber]=@PreviousReferenceVersionNumber, [PSZCustomernumber]=@PSZCustomernumber, [ReceivingDate]=@ReceivingDate, [RecipientId]=@RecipientId, [ReferenceNumber]=@ReferenceNumber, [ReferenceVersionNumber]=@ReferenceVersionNumber, [SenderId]=@SenderId, [SupplierCity]=@SupplierCity, [SupplierContactFax]=@SupplierContactFax, [SupplierContactTelephone]=@SupplierContactTelephone, [SupplierCountryName]=@SupplierCountryName, [SupplierDUNS]=@SupplierDUNS, [SupplierPartyIdentification]=@SupplierPartyIdentification, [SupplierPartyName]=@SupplierPartyName, [SupplierPostCode]=@SupplierPostCode, [SupplierStreet]=@SupplierStreet, [ValidFrom]=@ValidFrom, [ValidTill]=@ValidTill WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("BuyerContactName", item.BuyerContactName);
			sqlCommand.Parameters.AddWithValue("BuyerContactTelephone", item.BuyerContactTelephone);
			sqlCommand.Parameters.AddWithValue("BuyerDUNS", item.BuyerDUNS);
			sqlCommand.Parameters.AddWithValue("BuyerPartyIdentification", item.BuyerPartyIdentification == null ? (object)DBNull.Value : item.BuyerPartyIdentification);
			sqlCommand.Parameters.AddWithValue("BuyerPartyName", item.BuyerPartyName);
			sqlCommand.Parameters.AddWithValue("BuyerPurchasingDepartment", item.BuyerPurchasingDepartment ?? "");
			sqlCommand.Parameters.AddWithValue("ConsigneeCity", item.ConsigneeCity);
			sqlCommand.Parameters.AddWithValue("ConsigneeContactFax", item.ConsigneeContactFax ?? "");
			sqlCommand.Parameters.AddWithValue("ConsigneeContactTelephone", item.ConsigneeContactTelephone ?? "");
			sqlCommand.Parameters.AddWithValue("ConsigneeCountryName", item.ConsigneeCountryName);
			sqlCommand.Parameters.AddWithValue("ConsigneeDUNS", item.ConsigneeDUNS);
			sqlCommand.Parameters.AddWithValue("ConsigneePartyIdentification", item.ConsigneePartyIdentification);
			sqlCommand.Parameters.AddWithValue("ConsigneePartyName", item.ConsigneePartyName);
			sqlCommand.Parameters.AddWithValue("ConsigneePostCode", item.ConsigneePostCode);
			sqlCommand.Parameters.AddWithValue("ConsigneeStorageLocation", item.ConsigneeStorageLocation == null ? (object)DBNull.Value : item.ConsigneeStorageLocation);
			sqlCommand.Parameters.AddWithValue("ConsigneeStreet", item.ConsigneeStreet);
			sqlCommand.Parameters.AddWithValue("ConsigneeUnloadingPoint", item.ConsigneeUnloadingPoint == null ? (object)DBNull.Value : item.ConsigneeUnloadingPoint);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
			sqlCommand.Parameters.AddWithValue("DocumentNumber", item.DocumentNumber);
			sqlCommand.Parameters.AddWithValue("Done", item.Done == null ? (object)DBNull.Value : item.Done);
			sqlCommand.Parameters.AddWithValue("ManualCreation", item.ManualCreation == null ? (object)DBNull.Value : item.ManualCreation);
			sqlCommand.Parameters.AddWithValue("MessageReferenceNumber", item.MessageReferenceNumber);
			sqlCommand.Parameters.AddWithValue("MessageType", item.MessageType);
			sqlCommand.Parameters.AddWithValue("PreviousReferenceVersionNumber", item.PreviousReferenceVersionNumber == null ? (object)DBNull.Value : item.PreviousReferenceVersionNumber);
			sqlCommand.Parameters.AddWithValue("PSZCustomernumber", item.PSZCustomernumber == null ? (object)DBNull.Value : item.PSZCustomernumber);
			sqlCommand.Parameters.AddWithValue("ReceivingDate", item.ReceivingDate == null ? (object)DBNull.Value : item.ReceivingDate);
			sqlCommand.Parameters.AddWithValue("RecipientId", item.RecipientId);
			sqlCommand.Parameters.AddWithValue("ReferenceNumber", item.ReferenceNumber);
			sqlCommand.Parameters.AddWithValue("ReferenceVersionNumber", item.ReferenceVersionNumber == null ? (object)DBNull.Value : item.ReferenceVersionNumber);
			sqlCommand.Parameters.AddWithValue("SenderId", item.SenderId);
			sqlCommand.Parameters.AddWithValue("SupplierCity", item.SupplierCity ?? "");
			sqlCommand.Parameters.AddWithValue("SupplierContactFax", item.SupplierContactFax ?? "");
			sqlCommand.Parameters.AddWithValue("SupplierContactTelephone", item.SupplierContactTelephone ?? "");
			sqlCommand.Parameters.AddWithValue("SupplierCountryName", item.SupplierCountryName ?? "");
			sqlCommand.Parameters.AddWithValue("SupplierDUNS", item.SupplierDUNS);
			sqlCommand.Parameters.AddWithValue("SupplierPartyIdentification", item.SupplierPartyIdentification);
			sqlCommand.Parameters.AddWithValue("SupplierPartyName", item.SupplierPartyName);
			sqlCommand.Parameters.AddWithValue("SupplierPostCode", item.SupplierPostCode);
			sqlCommand.Parameters.AddWithValue("SupplierStreet", item.SupplierStreet);
			sqlCommand.Parameters.AddWithValue("ValidFrom", item.ValidFrom == null ? (object)DBNull.Value : item.ValidFrom);
			sqlCommand.Parameters.AddWithValue("ValidTill", item.ValidTill == null ? (object)DBNull.Value : item.ValidTill);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 43; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__EDI_DLF_Header] SET "

					+ "[BuyerContactName]=@BuyerContactName" + i + ","
					+ "[BuyerContactTelephone]=@BuyerContactTelephone" + i + ","
					+ "[BuyerDUNS]=@BuyerDUNS" + i + ","
					+ "[BuyerPartyIdentification]=@BuyerPartyIdentification" + i + ","
					+ "[BuyerPartyName]=@BuyerPartyName" + i + ","
					+ "[BuyerPurchasingDepartment]=@BuyerPurchasingDepartment" + i + ","
					+ "[ConsigneeCity]=@ConsigneeCity" + i + ","
					+ "[ConsigneeContactFax]=@ConsigneeContactFax" + i + ","
					+ "[ConsigneeContactTelephone]=@ConsigneeContactTelephone" + i + ","
					+ "[ConsigneeCountryName]=@ConsigneeCountryName" + i + ","
					+ "[ConsigneeDUNS]=@ConsigneeDUNS" + i + ","
					+ "[ConsigneePartyIdentification]=@ConsigneePartyIdentification" + i + ","
					+ "[ConsigneePartyName]=@ConsigneePartyName" + i + ","
					+ "[ConsigneePostCode]=@ConsigneePostCode" + i + ","
					+ "[ConsigneeStorageLocation]=@ConsigneeStorageLocation" + i + ","
					+ "[ConsigneeStreet]=@ConsigneeStreet" + i + ","
					+ "[ConsigneeUnloadingPoint]=@ConsigneeUnloadingPoint" + i + ","
					+ "[CreationTime]=@CreationTime" + i + ","
					+ "[DocumentNumber]=@DocumentNumber" + i + ","
					+ "[Done]=@Done" + i + ","
					+ "[ManualCreation]=@ManualCreation" + i + ","
					+ "[MessageReferenceNumber]=@MessageReferenceNumber" + i + ","
					+ "[MessageType]=@MessageType" + i + ","
					+ "[PreviousReferenceVersionNumber]=@PreviousReferenceVersionNumber" + i + ","
					+ "[PSZCustomernumber]=@PSZCustomernumber" + i + ","
					+ "[ReceivingDate]=@ReceivingDate" + i + ","
					+ "[RecipientId]=@RecipientId" + i + ","
					+ "[ReferenceNumber]=@ReferenceNumber" + i + ","
					+ "[ReferenceVersionNumber]=@ReferenceVersionNumber" + i + ","
					+ "[SenderId]=@SenderId" + i + ","
					+ "[SupplierCity]=@SupplierCity" + i + ","
					+ "[SupplierContactFax]=@SupplierContactFax" + i + ","
					+ "[SupplierContactTelephone]=@SupplierContactTelephone" + i + ","
					+ "[SupplierCountryName]=@SupplierCountryName" + i + ","
					+ "[SupplierDUNS]=@SupplierDUNS" + i + ","
					+ "[SupplierPartyIdentification]=@SupplierPartyIdentification" + i + ","
					+ "[SupplierPartyName]=@SupplierPartyName" + i + ","
					+ "[SupplierPostCode]=@SupplierPostCode" + i + ","
					+ "[SupplierStreet]=@SupplierStreet" + i + ","
					+ "[ValidFrom]=@ValidFrom" + i + ","
					+ "[ValidTill]=@ValidTill" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("BuyerContactName" + i, item.BuyerContactName);
					sqlCommand.Parameters.AddWithValue("BuyerContactTelephone" + i, item.BuyerContactTelephone);
					sqlCommand.Parameters.AddWithValue("BuyerDUNS" + i, item.BuyerDUNS);
					sqlCommand.Parameters.AddWithValue("BuyerPartyIdentification" + i, item.BuyerPartyIdentification == null ? (object)DBNull.Value : item.BuyerPartyIdentification);
					sqlCommand.Parameters.AddWithValue("BuyerPartyName" + i, item.BuyerPartyName);
					sqlCommand.Parameters.AddWithValue("BuyerPurchasingDepartment" + i, item.BuyerPurchasingDepartment ?? "");
					sqlCommand.Parameters.AddWithValue("ConsigneeCity" + i, item.ConsigneeCity);
					sqlCommand.Parameters.AddWithValue("ConsigneeContactFax" + i, item.ConsigneeContactFax ?? "");
					sqlCommand.Parameters.AddWithValue("ConsigneeContactTelephone" + i, item.ConsigneeContactTelephone ?? "");
					sqlCommand.Parameters.AddWithValue("ConsigneeCountryName" + i, item.ConsigneeCountryName);
					sqlCommand.Parameters.AddWithValue("ConsigneeDUNS" + i, item.ConsigneeDUNS);
					sqlCommand.Parameters.AddWithValue("ConsigneePartyIdentification" + i, item.ConsigneePartyIdentification);
					sqlCommand.Parameters.AddWithValue("ConsigneePartyName" + i, item.ConsigneePartyName);
					sqlCommand.Parameters.AddWithValue("ConsigneePostCode" + i, item.ConsigneePostCode);
					sqlCommand.Parameters.AddWithValue("ConsigneeStorageLocation" + i, item.ConsigneeStorageLocation == null ? (object)DBNull.Value : item.ConsigneeStorageLocation);
					sqlCommand.Parameters.AddWithValue("ConsigneeStreet" + i, item.ConsigneeStreet);
					sqlCommand.Parameters.AddWithValue("ConsigneeUnloadingPoint" + i, item.ConsigneeUnloadingPoint == null ? (object)DBNull.Value : item.ConsigneeUnloadingPoint);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("DocumentNumber" + i, item.DocumentNumber);
					sqlCommand.Parameters.AddWithValue("Done" + i, item.Done == null ? (object)DBNull.Value : item.Done);
					sqlCommand.Parameters.AddWithValue("ManualCreation" + i, item.ManualCreation == null ? (object)DBNull.Value : item.ManualCreation);
					sqlCommand.Parameters.AddWithValue("MessageReferenceNumber" + i, item.MessageReferenceNumber);
					sqlCommand.Parameters.AddWithValue("MessageType" + i, item.MessageType);
					sqlCommand.Parameters.AddWithValue("PreviousReferenceVersionNumber" + i, item.PreviousReferenceVersionNumber == null ? (object)DBNull.Value : item.PreviousReferenceVersionNumber);
					sqlCommand.Parameters.AddWithValue("PSZCustomernumber" + i, item.PSZCustomernumber == null ? (object)DBNull.Value : item.PSZCustomernumber);
					sqlCommand.Parameters.AddWithValue("ReceivingDate" + i, item.ReceivingDate == null ? (object)DBNull.Value : item.ReceivingDate);
					sqlCommand.Parameters.AddWithValue("RecipientId" + i, item.RecipientId);
					sqlCommand.Parameters.AddWithValue("ReferenceNumber" + i, item.ReferenceNumber);
					sqlCommand.Parameters.AddWithValue("ReferenceVersionNumber" + i, item.ReferenceVersionNumber == null ? (object)DBNull.Value : item.ReferenceVersionNumber);
					sqlCommand.Parameters.AddWithValue("SenderId" + i, item.SenderId);
					sqlCommand.Parameters.AddWithValue("SupplierCity" + i, item.SupplierCity);
					sqlCommand.Parameters.AddWithValue("SupplierContactFax" + i, item.SupplierContactFax);
					sqlCommand.Parameters.AddWithValue("SupplierContactTelephone" + i, item.SupplierContactTelephone);
					sqlCommand.Parameters.AddWithValue("SupplierCountryName" + i, item.SupplierCountryName);
					sqlCommand.Parameters.AddWithValue("SupplierDUNS" + i, item.SupplierDUNS);
					sqlCommand.Parameters.AddWithValue("SupplierPartyIdentification" + i, item.SupplierPartyIdentification);
					sqlCommand.Parameters.AddWithValue("SupplierPartyName" + i, item.SupplierPartyName);
					sqlCommand.Parameters.AddWithValue("SupplierPostCode" + i, item.SupplierPostCode);
					sqlCommand.Parameters.AddWithValue("SupplierStreet" + i, item.SupplierStreet);
					sqlCommand.Parameters.AddWithValue("ValidFrom" + i, item.ValidFrom == null ? (object)DBNull.Value : item.ValidFrom);
					sqlCommand.Parameters.AddWithValue("ValidTill" + i, item.ValidTill == null ? (object)DBNull.Value : item.ValidTill);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(long id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__EDI_DLF_Header] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			results = DbExecution.ExecuteNonQuery(sqlCommand);


			return results;
		}
		public static int DeleteWithTransaction(List<long> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE;
				int results = 0;
				if(ids.Count <= maxParamsNumber)
				{
					results = deleteWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += deleteWithTransaction(ids.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += deleteWithTransaction(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber), connection, transaction);
				}
			}
			return -1;
		}
		private static int deleteWithTransaction(List<long> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int results = -1;

				var sqlCommand = new SqlCommand("", connection, transaction);

				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				string query = "DELETE FROM [__EDI_DLF_Header] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> GetByDocumentNumber(string documentNumber, bool? includeArchived = false)
		{
			if(string.IsNullOrWhiteSpace(documentNumber))
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__EDI_DLF_Header] WHERE {(includeArchived.HasValue ? $"ISNULL([Done],0)={(includeArchived.Value ? "1" : "0")} AND " : "")}[DocumentNumber]={documentNumber.Trim()}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.HeaderEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity>();
			}
		}
		public static bool ExistsDocumentNumber(string documentNumber)
		{
			if(string.IsNullOrWhiteSpace(documentNumber))
				return false;

			//using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
			//{
			//    sqlConnection.Open();
			//    string query = "SELECT COUNT(*) FROM [__EDI_DLF_Header] WHERE [DocumentNumber]=@documentNumber";
			//    var sqlCommand = new SqlCommand(query, sqlConnection);
			//    sqlCommand.Parameters.AddWithValue("documentNumber", documentNumber.Trim());

			//    return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var _r) ? _r > 0 : false;
			//}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT TOP 1 * FROM [__EDI_DLF_Header] WHERE [DocumentNumber]=@documentNumber";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("documentNumber", documentNumber.Trim());

				DbExecution.Fill(sqlCommand, dataTable);

			}

			return dataTable.Rows.Count > 0;
		}
		public static bool ExistsDocumentNumber(string documentNumber, SqlConnection connection, SqlTransaction transaction)
		{
			if(string.IsNullOrWhiteSpace(documentNumber))
				return false;

			var dataTable = new DataTable();
			string query = "SELECT TOP 1 * FROM [__EDI_DLF_Header] WHERE [DocumentNumber]=@documentNumber";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("documentNumber", documentNumber.Trim());

			DbExecution.Fill(sqlCommand, dataTable);

			return dataTable.Rows.Count > 0;
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> GetLastByBuyerDUNS(string buyerDuns, bool? includeArchived = false)
		{
			if(string.IsNullOrWhiteSpace(buyerDuns))
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT H1.* FROM [__EDI_DLF_Header] H1 
                    JOIN (SELECT [DocumentNumber], MAX([ReferenceVersionNumber]) RefVersion FROM [__EDI_DLF_Header] WHERE [BuyerDUNS]=@buyerDuns GROUP BY [DocumentNumber]) AS H2 
                    ON H2.[DocumentNumber]=H1.[DocumentNumber] AND H2.RefVersion=H1.[ReferenceVersionNumber]
					{(includeArchived.HasValue ? $"WHERE ISNULL(H1.[Done],0)={(includeArchived.Value ? "1" : "0")}" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("buyerDuns", buyerDuns.Trim());

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.HeaderEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> GetBeforeLastByBuyerDUNS(string buyerDuns, bool? includeArchived = false)
		{
			if(string.IsNullOrWhiteSpace(buyerDuns))
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT H1.* FROM [__EDI_DLF_Header] H1 JOIN "
					+ "(SELECT [DocumentNumber], MAX([ReferenceVersionNumber]) RefVersion  FROM [__EDI_DLF_Header] T1 WHERE [BuyerDUNS]=@buyerDuns AND "
					+ "[ReferenceVersionNumber]<>(SELECT MAX([ReferenceVersionNumber]) RefVersion FROM [__EDI_DLF_Header] T2 "
					+ "WHERE [BuyerDUNS]=@buyerDuns  AND T1.[DocumentNumber]=T2.[DocumentNumber] AND T1.[BuyerDUNS]=T2.[BuyerDUNS] GROUP BY [DocumentNumber]) "
					+ "GROUP BY [DocumentNumber]) AS H2  ON H2.[DocumentNumber]=H1.[DocumentNumber] AND H2.RefVersion=H1.[ReferenceVersionNumber]" +
					$"{(includeArchived.HasValue ? $" WHERE ISNULL(H1.[Done],0)={(includeArchived.Value ? "1" : "0")}" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("buyerDuns", buyerDuns.Trim());

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.HeaderEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity>();
			}
		}
		public static Infrastructure.Data.Entities.Tables.CTS.HeaderEntity GetPreviousVersion(int headerId, bool? includeArchived = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT H1.* FROM [__EDI_DLF_Header] H1 JOIN "
					+ $"(SELECT [DocumentNumber], MAX([ReferenceVersionNumber]) RefVersion  FROM [__EDI_DLF_Header] T1 WHERE {(includeArchived.HasValue ? $"ISNULL(T1.[Done],0)={(includeArchived.Value ? "1" : "0")} AND " : "")}"
					+ "[ReferenceVersionNumber]=(SELECT MAX([ReferenceVersionNumber]) RefVersion FROM [__EDI_DLF_Header] T2 "
					+ $"WHERE {(includeArchived.HasValue ? $"ISNULL(T2.[Done],0)={(includeArchived.Value ? "1" : "0")} AND " : "")}T1.[DocumentNumber]=T2.[DocumentNumber] AND T1.[BuyerDUNS]=T2.[BuyerDUNS] AND "
					+ "[ReferenceVersionNumber] < (SELECT [ReferenceVersionNumber] FROM [__EDI_DLF_Header] T3 WHERE T3.Id=@id AND T2.[DocumentNumber]=T3.[DocumentNumber] AND T2.[BuyerDUNS]=T3.[BuyerDUNS])  GROUP BY [DocumentNumber]"
					+ ") GROUP BY [DocumentNumber] "
					+ ") AS H2  ON H2.[DocumentNumber]=H1.[DocumentNumber] AND H2.RefVersion=H1.[ReferenceVersionNumber]" +
					$"{(includeArchived.HasValue ? $" WHERE ISNULL(H1.[Done],0)={(includeArchived.Value ? "1" : "0")}" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", headerId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.HeaderEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.CTS.HeaderEntity GetNextVersion(int headerId, bool? includeArchived = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT H1.* FROM [__EDI_DLF_Header] H1 JOIN "
					+ $"(SELECT [DocumentNumber], MIN([ReferenceVersionNumber]) RefVersion  FROM [__EDI_DLF_Header] T1 WHERE {(includeArchived.HasValue ? $"ISNULL(T1.[Done],0)={(includeArchived.Value ? "1" : "0")} AND " : "")}"
					+ "[ReferenceVersionNumber]=(SELECT MIN([ReferenceVersionNumber]) RefVersion FROM [__EDI_DLF_Header] T2 "
					+ $"WHERE {(includeArchived.HasValue ? $"ISNULL(T2.[Done],0)={(includeArchived.Value ? "1" : "0")} AND " : "")}T1.[DocumentNumber]=T2.[DocumentNumber] AND T1.[BuyerDUNS]=T2.[BuyerDUNS] AND "
					+ "[ReferenceVersionNumber] > (SELECT [ReferenceVersionNumber] FROM [__EDI_DLF_Header] T3 WHERE T3.Id=@id AND T2.[DocumentNumber]=T3.[DocumentNumber] AND T2.[BuyerDUNS]=T3.[BuyerDUNS])  GROUP BY [DocumentNumber]"
					+ ") GROUP BY [DocumentNumber] "
					+ ") AS H2  ON H2.[DocumentNumber]=H1.[DocumentNumber] AND H2.RefVersion=H1.[ReferenceVersionNumber]" +
					$"{(includeArchived.HasValue ? $" WHERE ISNULL(H1.[Done],0)={(includeArchived.Value ? "1" : "0")}" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", headerId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.HeaderEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<KeyValuePair<long, string>> GetDocumentVersions(string buyerDuns, string documentNumber, bool? includeArchived = false)
		{
			if(string.IsNullOrWhiteSpace(buyerDuns))
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT [Id],[ReferenceVersionNumber] FROM [__EDI_DLF_Header] WHERE {(includeArchived.HasValue ? $"ISNULL([Done],0)={(includeArchived.Value ? "1" : "0")} AND " : "")}[SenderId]=@buyerDuns AND [ReferenceNumber]=@documentNumber";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("buyerDuns", buyerDuns.Trim());
				sqlCommand.Parameters.AddWithValue("documentNumber", documentNumber.Trim());

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<long, string>(long.TryParse(x[0].ToString(), out var _id) ? _id : -1, x[1].ToString())).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> GetDocumentVersions(string buyerDuns, List<string> documentNumbers, bool? includeArchived = false)
		{
			if(string.IsNullOrWhiteSpace(buyerDuns) || documentNumbers == null || documentNumbers.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__EDI_DLF_Header] WHERE {(includeArchived.HasValue ? $"ISNULL([Done],0)={(includeArchived.Value ? "1" : "0")} AND " : "")}[SenderId]=@buyerDuns AND [ReferenceNumber] IN ('{string.Join("','", documentNumbers)}')";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("buyerDuns", buyerDuns.Trim());

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.HeaderEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> GetLastByBuyerDUNS(List<string> buyerDuns, bool? includeArchived = false)
		{
			if(buyerDuns == null || buyerDuns.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"SELECT H1.* FROM [__EDI_DLF_Header] H1
                    JOIN (SELECT [DocumentNumber], MAX([ReferenceVersionNumber]) RefVersion FROM [__EDI_DLF_Header] 
                    WHERE {(includeArchived.HasValue ? $"ISNULL([Done],0)={(includeArchived.Value ? "1" : "0")} AND " : "")}[BuyerDUNS] IN ('{string.Join("','", buyerDuns.Select(x => x.Trim()))}') 
                    AND (ManualCreation IS NULL OR ManualCreation=0)
                    GROUP BY [DocumentNumber]) AS H2
                    ON H2.[DocumentNumber]=H1.[DocumentNumber] AND H2.RefVersion=H1.[ReferenceVersionNumber]
					{(includeArchived.HasValue ? $"WHERE ISNULL(H1.[Done],0)={(includeArchived.Value ? "1" : "0")}" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.HeaderEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity>();
			}
		}

		public static Infrastructure.Data.Entities.Tables.CTS.HeaderEntity GetLastVersion(string BuyerDUNS, string documentNumber, bool? includeArchived = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__EDI_DLF_Header] WHERE {(includeArchived.HasValue ? $"ISNULL([Done],0)={(includeArchived.Value ? "1" : "0")} AND " : "")}[DocumentNumber]=@documentNumber AND [BuyerDUNS]=@BuyerDUNS "
					+ $"AND [ReferenceVersionNumber] = (SELECT MAX([ReferenceVersionNumber]) RefVersion FROM [__EDI_DLF_Header] WHERE {(includeArchived.HasValue ? $"ISNULL([Done],0)={(includeArchived.Value ? "1" : "0")} AND " : "")}[DocumentNumber]=@documentNumber AND [BuyerDUNS]=@BuyerDUNS)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("BuyerDUNS", BuyerDUNS.Trim());
				sqlCommand.Parameters.AddWithValue("documentNumber", documentNumber.Trim());

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.HeaderEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.CTS.HeaderEntity GetLastVersion(int kundenNr, string documentNumber, bool? includeArchived = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__EDI_DLF_Header] WHERE {(includeArchived.HasValue ? $"ISNULL([Done],0)={(includeArchived.Value ? "1" : "0")} AND " : "")}[DocumentNumber]=@documentNumber AND [PSZCustomernumber]=@kundenNr "
					+ $"AND [ReferenceVersionNumber] = (SELECT MAX([ReferenceVersionNumber]) RefVersion FROM [__EDI_DLF_Header] WHERE {(includeArchived.HasValue ? $"ISNULL([Done],0)={(includeArchived.Value ? "1" : "0")} AND " : "")}[DocumentNumber]=@documentNumber AND [PSZCustomernumber]=@kundenNr)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("kundenNr", kundenNr);
				sqlCommand.Parameters.AddWithValue("documentNumber", documentNumber?.Trim() ?? "");

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.HeaderEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		//Manual
		public static List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> GetLastManualDelfors(bool? includeArchived = false)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT H1.* FROM [__EDI_DLF_Header] H1 
									JOIN (SELECT [DocumentNumber], MAX([ReferenceVersionNumber]) RefVersion FROM [__EDI_DLF_Header]
									WHERE {(includeArchived.HasValue ? $"ISNULL(H1.[Done],0)={(includeArchived.Value ? "1" : "0")} AND " : "")}[PSZCustomernumber] IN
									(
									select distinct [PSZCustomernumber] from __EDI_DLF_Header where {(includeArchived.HasValue ? $"ISNULL([Done],0)={(includeArchived.Value ? "1" : "0")} AND " : "")}ManualCreation=1
									)
									GROUP BY [DocumentNumber]) AS H2 
									ON H2.[DocumentNumber]=H1.[DocumentNumber] AND H2.RefVersion=H1.[ReferenceVersionNumber]
									ORDER BY [PSZCustomernumber]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.HeaderEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> GetDeflorOtherversions(string documentNumber, int customerNumber, int version, bool exept = true, bool? includeArchived = false)
		{
			if(string.IsNullOrWhiteSpace(documentNumber))
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__EDI_DLF_Header] WHERE {(includeArchived.HasValue ? $"ISNULL([Done],0)={(includeArchived.Value ? "1" : "0")} AND " : "")}[DocumentNumber]='{documentNumber.Trim()}' AND [PSZCustomernumber]=@customerNumber {(exept ? "AND [ReferenceVersionNumber]<>@version" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);
				sqlCommand.Parameters.AddWithValue("version", version);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.HeaderEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> GetDeflorOtherversions(string documentNumber, int customerNumber, int version, bool exept = true, bool? includeArchived = false, SqlConnection connection = null, SqlTransaction transaction = null)
		{
			if(string.IsNullOrWhiteSpace(documentNumber))
				return null;

			var dataTable = new DataTable();
			string query = $"SELECT * FROM [__EDI_DLF_Header] WHERE {(includeArchived.HasValue ? $"ISNULL([Done],0)={(includeArchived.Value ? "1" : "0")} AND " : "")}[DocumentNumber]='{documentNumber.Trim()}' AND [PSZCustomernumber]=@customerNumber {(exept ? "AND [ReferenceVersionNumber]<>@version" : "")}";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);
			sqlCommand.Parameters.AddWithValue("version", version);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.HeaderEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.CTS.HeaderEntity GetByDocumentNumberAndCustomerNumber(string documentNumber, int customerNumber, int version, bool? includeArchived = false)
		{
			if(string.IsNullOrWhiteSpace(documentNumber))
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__EDI_DLF_Header] WHERE {(includeArchived.HasValue ? $"ISNULL([Done],0)={(includeArchived.Value ? "1" : "0")} AND " : "")}[DocumentNumber]='{documentNumber.Trim()}' AND [PSZCustomernumber]=@customerNumber AND [ReferenceVersionNumber]=@version";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);
				sqlCommand.Parameters.AddWithValue("version", version);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.HeaderEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> GetByDocumentAndCustomer(string documentNumber, int customerNumber, bool? includeArchived = false, SqlConnection connection = null, SqlTransaction transaction = null)
		{
			if(string.IsNullOrWhiteSpace(documentNumber))
				return null;

			var dataTable = new DataTable();
			string query = $"SELECT * FROM [__EDI_DLF_Header] WHERE {(includeArchived.HasValue ? $"ISNULL([Done],0)={(includeArchived.Value ? "1" : "0")} AND " : "")}[DocumentNumber]='{documentNumber.Trim()}' AND [PSZCustomernumber]=@customerNumber";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.HeaderEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> GetByDocumentAndCustomer(string documentNumber, int customerNumber, bool? includeArchived = false)
		{
			if(string.IsNullOrWhiteSpace(documentNumber))
				return null;

			var dataTable = new DataTable();
			using(var connection = new SqlConnection(Settings.ConnectionString))
			{
				connection.Open();
				string query = $"SELECT * FROM [__EDI_DLF_Header] WHERE {(includeArchived.HasValue ? $"ISNULL([Done],0)={(includeArchived.Value ? "1" : "0")} AND " : "")}[DocumentNumber]='{documentNumber.Trim()}' AND [PSZCustomernumber]=@customerNumber";
				var sqlCommand = new SqlCommand(query, connection);
				sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.HeaderEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> GetManualDelfors(bool? includeArchived = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__EDI_DLF_Header] WHERE {(includeArchived.HasValue ? $"ISNULL([Done],0)={(includeArchived.Value ? "1" : "0")} AND " : "")}[ManualCreation]=1";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.HeaderEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> GetDelforsByDuns(string duns, bool? includeArchived = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__EDI_DLF_Header] WHERE {(includeArchived.HasValue ? $"ISNULL([Done],0)={(includeArchived.Value ? "1" : "0")} AND " : "")}([ManualCreation]=0 OR [ManualCreation] IS NULL) AND [BuyerDUNS]=@duns";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("duns", duns);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.HeaderEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> GetDelforsByDuns(List<string> duns, bool manual, bool? includeArchived = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__EDI_DLF_Header] WHERE {(includeArchived.HasValue ? $"ISNULL([Done],0)={(includeArchived.Value ? "1" : "0")} AND " : "")}ISNULL([ManualCreation],0)=@manual AND [BuyerDUNS] IN ('{(string.Join("','", duns))}')";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("manual", manual);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.HeaderEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> GetDelforsByCustomerNumber(int customerNumber, bool manual, bool? includeArchived = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__EDI_DLF_Header] WHERE {(includeArchived.HasValue ? $"ISNULL([Done],0)={(includeArchived.Value ? "1" : "0")} AND " : "")}ISNULL([ManualCreation],0)=@manual AND [PSZCustomerNumber]=@customerNumber";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);
				sqlCommand.Parameters.AddWithValue("manual", manual);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.HeaderEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity> GetDelforsByCustomerNumber(List<int> customerNumbers, bool manual, bool? includeArchived = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__EDI_DLF_Header] WHERE {(includeArchived.HasValue ? $"ISNULL([Done],0)={(includeArchived.Value ? "1" : "0")} AND " : "")}ISNULL([ManualCreation],0)=@manual AND [PSZCustomerNumber] IN ('{(string.Join("','", customerNumbers))}')";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("manual", manual);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.HeaderEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity>();
			}
		}
		public static int ToggleStatus(int id, bool done, SqlConnection connection, SqlTransaction transaction)
		{

			string query = "UPDATE [__EDI_DLF_Header] SET [Done]=@done WHERE [Id]=@Id AND ISNULL([Done],0)<>@done";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", id);
			sqlCommand.Parameters.AddWithValue("done", done);

			return DbExecution.ExecuteNonQuery(sqlCommand);
		}
		public static int ToggleStatus(List<long> headerIds, string documentNumber, bool done, SqlConnection connection, SqlTransaction transaction)
		{

			string query = $"UPDATE [__EDI_DLF_Header] SET [Done]=@done, [DocumentNumber]=@documentNumber WHERE [Id] IN ({(headerIds?.Count > 0 ? string.Join(",", headerIds) : "")}) AND ISNULL([Done],0)<>@done";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("done", done);
			sqlCommand.Parameters.AddWithValue("documentNumber", documentNumber);

			return DbExecution.ExecuteNonQuery(sqlCommand);
		}

		////13-02-2025
		////
		//public static Infrastructure.Data.Entities.Tables.CTS.HeaderEntity GetByConsigneeUnloadingPoint(string id, SqlConnection connection, SqlTransaction transaction)
		//{
		//	var dataTable = new DataTable();

		//	string query = "SELECT * FROM [__EDI_DLF_Header] WHERE [ConsigneeUnloadingPoint]=@Id";
		//	var sqlCommand = new SqlCommand(query, connection, transaction);
		//	sqlCommand.Parameters.AddWithValue("Id", id);
		//	DbExecution.Fill(sqlCommand, dataTable);

		//	if(dataTable.Rows.Count > 0)
		//	{
		//		return new Infrastructure.Data.Entities.Tables.CTS.HeaderEntity(dataTable.Rows[0]);
		//	}
		//	else
		//	{
		//		return null;
		//	}
		//}
		#endregion Custom Methods

		static string ARCHIVE_SEPARATOR = "_-_-_";
		public static string AddSuffixForArchive(string documentNumber, SqlConnection connection, SqlTransaction transaction)
		{
			var query = $"SELECT COUNT(DISTINCT [DocumentNumber]) FROM [__EDI_DLF_Header] WHERE ISNULL([Done],0)=1 AND [DocumentNumber] LIKE '{documentNumber}{ARCHIVE_SEPARATOR}%'";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				return $"{documentNumber}{ARCHIVE_SEPARATOR}ARCHIVED_{(int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var _x) ? _x : 0) + 1}";
			}
		}
		public static string RemoveSuffixForArchive(string documentNumber)
		{
			documentNumber = documentNumber ?? "";
			var idx = documentNumber.IndexOf(ARCHIVE_SEPARATOR);
			return idx < 0 ? documentNumber : documentNumber.Substring(0, 1 + documentNumber.Length - idx - (ARCHIVE_SEPARATOR.Length + 1));
		}
		#region Helpers 

		#endregion
	}
}
