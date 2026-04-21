using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.MTM
{
	public class ArticleProductionAccess
	{
		#region Default Methods
		public static Entities.Tables.MTM.ArticleProductionEntity Get(int id)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__MTM_CRP_ArticleProduction] WHERE [Id]=@Id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.MTM.ArticleProductionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.MTM.ArticleProductionEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__MTM_CRP_ArticleProduction]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.MTM.ArticleProductionEntity>();
			}
		}
		public static List<Entities.Tables.MTM.ArticleProductionEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				var results = new List<Entities.Tables.MTM.ArticleProductionEntity>();
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Entities.Tables.MTM.ArticleProductionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Entities.Tables.MTM.ArticleProductionEntity>();
		}
		private static List<Entities.Tables.MTM.ArticleProductionEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
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

					sqlCommand.CommandText = "SELECT * FROM [__MTM_CRP_ArticleProduction] WHERE [Id] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Entities.Tables.MTM.ArticleProductionEntity>();
				}
			}
			return new List<Entities.Tables.MTM.ArticleProductionEntity>();
		}

		public static int Insert(Entities.Tables.MTM.ArticleProductionEntity item)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__MTM_CRP_ArticleProduction] ([Angebot_Artikel_Nr],[Angebot_nr],[Artikel_Nr],[Bemerkung],[CreationDate],[Erstmuster],[Fertigungsnummer],[Lagerort_id],[ProductionDeadlineDate],[Quantity],[WSAmount],[WSComment],[WSCountryId],[WSCreationDate],[WSCreationUserId],[WSDepartmentId],[WSFromToolInsert],[WSFromToolInsert2],[WSHallId],[WSLastEditDate],[WSLastEditUserId],[WSLotSizeSTD],[WSOperationDescriptionId],[WSOperationNumber],[WSOperationTimeSeconds],[WSOperationTimeValueAdding],[WSOperationValueAdding],[WSPredecessorOperation],[WSPredecessorSubOperation],[WSRelationOperationTime],[WSSetupTimeMinutes],[WSStandardOccupancy],[WSStandardOperationId],[WSSubOperationNumber],[WSTotalTimeOperation],[WSWorkAreaId],[WSWorkScheduleId],[WSWorkStationMachineId])  VALUES (@Angebot_Artikel_Nr,@Angebot_nr,@Artikel_Nr,@Bemerkung,@CreationDate,@Erstmuster,@Fertigungsnummer,@Lagerort_id,@ProductionDeadlineDate,@Quantity,@WSAmount,@WSComment,@WSCountryId,@WSCreationDate,@WSCreationUserId,@WSDepartmentId,@WSFromToolInsert,@WSFromToolInsert2,@WSHallId,@WSLastEditDate,@WSLastEditUserId,@WSLotSizeSTD,@WSOperationDescriptionId,@WSOperationNumber,@WSOperationTimeSeconds,@WSOperationTimeValueAdding,@WSOperationValueAdding,@WSPredecessorOperation,@WSPredecessorSubOperation,@WSRelationOperationTime,@WSSetupTimeMinutes,@WSStandardOccupancy,@WSStandardOperationId,@WSSubOperationNumber,@WSTotalTimeOperation,@WSWorkAreaId,@WSWorkScheduleId,@WSWorkStationMachineId)";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("Angebot_Artikel_Nr", item.Angebot_Artikel_Nr == null ? (object)DBNull.Value : item.Angebot_Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Angebot_nr", item.Angebot_nr == null ? (object)DBNull.Value : item.Angebot_nr);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("CreationDate", item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
					sqlCommand.Parameters.AddWithValue("Erstmuster", item.Erstmuster == null ? (object)DBNull.Value : item.Erstmuster);
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("ProductionDeadlineDate", item.ProductionDeadlineDate == null ? (object)DBNull.Value : item.ProductionDeadlineDate);
					sqlCommand.Parameters.AddWithValue("Quantity", item.Quantity == null ? (object)DBNull.Value : item.Quantity);
					sqlCommand.Parameters.AddWithValue("WSAmount", item.WSAmount);
					sqlCommand.Parameters.AddWithValue("WSComment", item.WSComment == null ? (object)DBNull.Value : item.WSComment);
					sqlCommand.Parameters.AddWithValue("WSCountryId", item.WSCountryId);
					sqlCommand.Parameters.AddWithValue("WSCreationDate", item.WSCreationDate);
					sqlCommand.Parameters.AddWithValue("WSCreationUserId", item.WSCreationUserId);
					sqlCommand.Parameters.AddWithValue("WSDepartmentId", item.WSDepartmentId);
					sqlCommand.Parameters.AddWithValue("WSFromToolInsert", item.WSFromToolInsert == null ? (object)DBNull.Value : item.WSFromToolInsert);
					sqlCommand.Parameters.AddWithValue("WSFromToolInsert2", item.WSFromToolInsert2 == null ? (object)DBNull.Value : item.WSFromToolInsert2);
					sqlCommand.Parameters.AddWithValue("WSHallId", item.WSHallId);
					sqlCommand.Parameters.AddWithValue("WSLastEditDate", item.WSLastEditDate == null ? (object)DBNull.Value : item.WSLastEditDate);
					sqlCommand.Parameters.AddWithValue("WSLastEditUserId", item.WSLastEditUserId == null ? (object)DBNull.Value : item.WSLastEditUserId);
					sqlCommand.Parameters.AddWithValue("WSLotSizeSTD", item.WSLotSizeSTD);
					sqlCommand.Parameters.AddWithValue("WSOperationDescriptionId", item.WSOperationDescriptionId == null ? (object)DBNull.Value : item.WSOperationDescriptionId);
					sqlCommand.Parameters.AddWithValue("WSOperationNumber", item.WSOperationNumber);
					sqlCommand.Parameters.AddWithValue("WSOperationTimeSeconds", item.WSOperationTimeSeconds);
					sqlCommand.Parameters.AddWithValue("WSOperationTimeValueAdding", item.WSOperationTimeValueAdding);
					sqlCommand.Parameters.AddWithValue("WSOperationValueAdding", item.WSOperationValueAdding == null ? (object)DBNull.Value : item.WSOperationValueAdding);
					sqlCommand.Parameters.AddWithValue("WSPredecessorOperation", item.WSPredecessorOperation);
					sqlCommand.Parameters.AddWithValue("WSPredecessorSubOperation", item.WSPredecessorSubOperation);
					sqlCommand.Parameters.AddWithValue("WSRelationOperationTime", item.WSRelationOperationTime);
					sqlCommand.Parameters.AddWithValue("WSSetupTimeMinutes", item.WSSetupTimeMinutes);
					sqlCommand.Parameters.AddWithValue("WSStandardOccupancy", item.WSStandardOccupancy);
					sqlCommand.Parameters.AddWithValue("WSStandardOperationId", item.WSStandardOperationId);
					sqlCommand.Parameters.AddWithValue("WSSubOperationNumber", item.WSSubOperationNumber);
					sqlCommand.Parameters.AddWithValue("WSTotalTimeOperation", item.WSTotalTimeOperation);
					sqlCommand.Parameters.AddWithValue("WSWorkAreaId", item.WSWorkAreaId);
					sqlCommand.Parameters.AddWithValue("WSWorkScheduleId", item.WSWorkScheduleId);
					sqlCommand.Parameters.AddWithValue("WSWorkStationMachineId", item.WSWorkStationMachineId == null ? (object)DBNull.Value : item.WSWorkStationMachineId);

					sqlCommand.ExecuteNonQuery();
				}

				using(var sqlCommand = new SqlCommand("SELECT [Id] FROM [__MTM_CRP_ArticleProduction] WHERE [Id] = @@IDENTITY", sqlConnection, sqlTransaction))
				{
					response = int.Parse(sqlCommand.ExecuteScalar()?.ToString() ?? "-1");
				}

				sqlTransaction.Commit();

				return response;
			}
		}

		public static int Update(Entities.Tables.MTM.ArticleProductionEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "UPDATE [__MTM_CRP_ArticleProduction] SET [Angebot_Artikel_Nr]=@Angebot_Artikel_Nr, [Angebot_nr]=@Angebot_nr, [Artikel_Nr]=@Artikel_Nr, [Bemerkung]=@Bemerkung, [CreationDate]=@CreationDate, [Erstmuster]=@Erstmuster, [Fertigungsnummer]=@Fertigungsnummer, [Lagerort_id]=@Lagerort_id, [ProductionDeadlineDate]=@ProductionDeadlineDate, [Quantity]=@Quantity, [WSAmount]=@WSAmount, [WSComment]=@WSComment, [WSCountryId]=@WSCountryId, [WSCreationDate]=@WSCreationDate, [WSCreationUserId]=@WSCreationUserId, [WSDepartmentId]=@WSDepartmentId, [WSFromToolInsert]=@WSFromToolInsert, [WSFromToolInsert2]=@WSFromToolInsert2, [WSHallId]=@WSHallId, [WSLastEditDate]=@WSLastEditDate, [WSLastEditUserId]=@WSLastEditUserId, [WSLotSizeSTD]=@WSLotSizeSTD, [WSOperationDescriptionId]=@WSOperationDescriptionId, [WSOperationNumber]=@WSOperationNumber, [WSOperationTimeSeconds]=@WSOperationTimeSeconds, [WSOperationTimeValueAdding]=@WSOperationTimeValueAdding, [WSOperationValueAdding]=@WSOperationValueAdding, [WSPredecessorOperation]=@WSPredecessorOperation, [WSPredecessorSubOperation]=@WSPredecessorSubOperation, [WSRelationOperationTime]=@WSRelationOperationTime, [WSSetupTimeMinutes]=@WSSetupTimeMinutes, [WSStandardOccupancy]=@WSStandardOccupancy, [WSStandardOperationId]=@WSStandardOperationId, [WSSubOperationNumber]=@WSSubOperationNumber, [WSTotalTimeOperation]=@WSTotalTimeOperation, [WSWorkAreaId]=@WSWorkAreaId, [WSWorkScheduleId]=@WSWorkScheduleId, [WSWorkStationMachineId]=@WSWorkStationMachineId WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Angebot_Artikel_Nr", item.Angebot_Artikel_Nr == null ? (object)DBNull.Value : item.Angebot_Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("Angebot_nr", item.Angebot_nr == null ? (object)DBNull.Value : item.Angebot_nr);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
				sqlCommand.Parameters.AddWithValue("CreationDate", item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
				sqlCommand.Parameters.AddWithValue("Erstmuster", item.Erstmuster == null ? (object)DBNull.Value : item.Erstmuster);
				sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
				sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
				sqlCommand.Parameters.AddWithValue("ProductionDeadlineDate", item.ProductionDeadlineDate == null ? (object)DBNull.Value : item.ProductionDeadlineDate);
				sqlCommand.Parameters.AddWithValue("Quantity", item.Quantity == null ? (object)DBNull.Value : item.Quantity);
				sqlCommand.Parameters.AddWithValue("WSAmount", item.WSAmount);
				sqlCommand.Parameters.AddWithValue("WSComment", item.WSComment == null ? (object)DBNull.Value : item.WSComment);
				sqlCommand.Parameters.AddWithValue("WSCountryId", item.WSCountryId);
				sqlCommand.Parameters.AddWithValue("WSCreationDate", item.WSCreationDate);
				sqlCommand.Parameters.AddWithValue("WSCreationUserId", item.WSCreationUserId);
				sqlCommand.Parameters.AddWithValue("WSDepartmentId", item.WSDepartmentId);
				sqlCommand.Parameters.AddWithValue("WSFromToolInsert", item.WSFromToolInsert == null ? (object)DBNull.Value : item.WSFromToolInsert);
				sqlCommand.Parameters.AddWithValue("WSFromToolInsert2", item.WSFromToolInsert2 == null ? (object)DBNull.Value : item.WSFromToolInsert2);
				sqlCommand.Parameters.AddWithValue("WSHallId", item.WSHallId);
				sqlCommand.Parameters.AddWithValue("WSLastEditDate", item.WSLastEditDate == null ? (object)DBNull.Value : item.WSLastEditDate);
				sqlCommand.Parameters.AddWithValue("WSLastEditUserId", item.WSLastEditUserId == null ? (object)DBNull.Value : item.WSLastEditUserId);
				sqlCommand.Parameters.AddWithValue("WSLotSizeSTD", item.WSLotSizeSTD);
				sqlCommand.Parameters.AddWithValue("WSOperationDescriptionId", item.WSOperationDescriptionId == null ? (object)DBNull.Value : item.WSOperationDescriptionId);
				sqlCommand.Parameters.AddWithValue("WSOperationNumber", item.WSOperationNumber);
				sqlCommand.Parameters.AddWithValue("WSOperationTimeSeconds", item.WSOperationTimeSeconds);
				sqlCommand.Parameters.AddWithValue("WSOperationTimeValueAdding", item.WSOperationTimeValueAdding);
				sqlCommand.Parameters.AddWithValue("WSOperationValueAdding", item.WSOperationValueAdding == null ? (object)DBNull.Value : item.WSOperationValueAdding);
				sqlCommand.Parameters.AddWithValue("WSPredecessorOperation", item.WSPredecessorOperation);
				sqlCommand.Parameters.AddWithValue("WSPredecessorSubOperation", item.WSPredecessorSubOperation);
				sqlCommand.Parameters.AddWithValue("WSRelationOperationTime", item.WSRelationOperationTime);
				sqlCommand.Parameters.AddWithValue("WSSetupTimeMinutes", item.WSSetupTimeMinutes);
				sqlCommand.Parameters.AddWithValue("WSStandardOccupancy", item.WSStandardOccupancy);
				sqlCommand.Parameters.AddWithValue("WSStandardOperationId", item.WSStandardOperationId);
				sqlCommand.Parameters.AddWithValue("WSSubOperationNumber", item.WSSubOperationNumber);
				sqlCommand.Parameters.AddWithValue("WSTotalTimeOperation", item.WSTotalTimeOperation);
				sqlCommand.Parameters.AddWithValue("WSWorkAreaId", item.WSWorkAreaId);
				sqlCommand.Parameters.AddWithValue("WSWorkScheduleId", item.WSWorkScheduleId);
				sqlCommand.Parameters.AddWithValue("WSWorkStationMachineId", item.WSWorkStationMachineId == null ? (object)DBNull.Value : item.WSWorkStationMachineId);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}

		public static int Delete(int id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__MTM_CRP_ArticleProduction] WHERE [Id]=@Id";
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
				using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
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

					string query = "DELETE FROM [__MTM_CRP_ArticleProduction] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		#endregion

		#region Helpers
		private static List<Entities.Tables.MTM.ArticleProductionEntity> toList(DataTable dataTable)
		{
			var list = new List<Entities.Tables.MTM.ArticleProductionEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Entities.Tables.MTM.ArticleProductionEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
