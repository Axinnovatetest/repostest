using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{

	public class ArtikelQualityExtensionAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_ArtikelQualityExtension] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_ArtikelQualityExtension]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [__BSD_ArtikelQualityExtension] WHERE [Id] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__BSD_ArtikelQualityExtension] ([ArticleId],[CoC_Pflichtig_AttachmentId],[CreateTime],[CreateUserId],[Dienstleistung_AttachmentId],[EMPB_AttachmentId],[EMPB_Freigegeben_AttachmentId],[ESD_AttachmentId],[HM_AttachmentId],[LLE_AttachmentId],[MHD_AttachmentId],[MineralsConfirmity_AttachmentId],[PackagingRegulation_Available],[PackagingRegulation_Available_AttachmentId],[PurchasingArticleInspection__SpecialArticlesCustomerSpecific],[PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId],[QSV],[QSV_AttachmentId],[REACH_SVHC_Confirmity_AttachmentId],[ROHS_EEE_Confirmity_AttachmentId],[SpecialCustomerReleases__DeviationReleases],[SpecialCustomerReleases__DeviationReleases_AttachmentId],[TSP_Available],[TSP_Available_AttachmentId],[UL_Etikett_AttachmentId],[UL_zertifiziert_AttachmentId],[UpdateTime],[UpdateUserId]) OUTPUT INSERTED.[Id] VALUES (@ArticleId,@CoC_Pflichtig_AttachmentId,@CreateTime,@CreateUserId,@Dienstleistung_AttachmentId,@EMPB_AttachmentId,@EMPB_Freigegeben_AttachmentId,@ESD_AttachmentId,@HM_AttachmentId,@LLE_AttachmentId,@MHD_AttachmentId,@MineralsConfirmity_AttachmentId,@PackagingRegulation_Available,@PackagingRegulation_Available_AttachmentId,@PurchasingArticleInspection__SpecialArticlesCustomerSpecific,@PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId,@QSV,@QSV_AttachmentId,@REACH_SVHC_Confirmity_AttachmentId,@ROHS_EEE_Confirmity_AttachmentId,@SpecialCustomerReleases__DeviationReleases,@SpecialCustomerReleases__DeviationReleases_AttachmentId,@TSP_Available,@TSP_Available_AttachmentId,@UL_Etikett_AttachmentId,@UL_zertifiziert_AttachmentId,@UpdateTime,@UpdateUserId); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId);
					sqlCommand.Parameters.AddWithValue("CoC_Pflichtig_AttachmentId", item.CoC_Pflichtig_AttachmentId == null ? (object)DBNull.Value : item.CoC_Pflichtig_AttachmentId);
					sqlCommand.Parameters.AddWithValue("CreateTime", item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
					sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
					sqlCommand.Parameters.AddWithValue("Dienstleistung_AttachmentId", item.Dienstleistung_AttachmentId == null ? (object)DBNull.Value : item.Dienstleistung_AttachmentId);
					sqlCommand.Parameters.AddWithValue("EMPB_AttachmentId", item.EMPB_AttachmentId == null ? (object)DBNull.Value : item.EMPB_AttachmentId);
					sqlCommand.Parameters.AddWithValue("EMPB_Freigegeben_AttachmentId", item.EMPB_Freigegeben_AttachmentId == null ? (object)DBNull.Value : item.EMPB_Freigegeben_AttachmentId);
					sqlCommand.Parameters.AddWithValue("ESD_AttachmentId", item.ESD_AttachmentId == null ? (object)DBNull.Value : item.ESD_AttachmentId);
					sqlCommand.Parameters.AddWithValue("HM_AttachmentId", item.HM_AttachmentId == null ? (object)DBNull.Value : item.HM_AttachmentId);
					sqlCommand.Parameters.AddWithValue("LLE_AttachmentId", item.LLE_AttachmentId == null ? (object)DBNull.Value : item.LLE_AttachmentId);
					sqlCommand.Parameters.AddWithValue("MHD_AttachmentId", item.MHD_AttachmentId == null ? (object)DBNull.Value : item.MHD_AttachmentId);
					sqlCommand.Parameters.AddWithValue("MineralsConfirmity_AttachmentId", item.MineralsConfirmity_AttachmentId == null ? (object)DBNull.Value : item.MineralsConfirmity_AttachmentId);
					sqlCommand.Parameters.AddWithValue("PackagingRegulation_Available", item.PackagingRegulation_Available == null ? (object)DBNull.Value : item.PackagingRegulation_Available);
					sqlCommand.Parameters.AddWithValue("PackagingRegulation_Available_AttachmentId", item.PackagingRegulation_Available_AttachmentId == null ? (object)DBNull.Value : item.PackagingRegulation_Available_AttachmentId);
					sqlCommand.Parameters.AddWithValue("PurchasingArticleInspection__SpecialArticlesCustomerSpecific", item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific == null ? (object)DBNull.Value : item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific);
					sqlCommand.Parameters.AddWithValue("PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId", item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId == null ? (object)DBNull.Value : item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId);
					sqlCommand.Parameters.AddWithValue("QSV", item.QSV == null ? (object)DBNull.Value : item.QSV);
					sqlCommand.Parameters.AddWithValue("QSV_AttachmentId", item.QSV_AttachmentId == null ? (object)DBNull.Value : item.QSV_AttachmentId);
					sqlCommand.Parameters.AddWithValue("REACH_SVHC_Confirmity_AttachmentId", item.REACH_SVHC_Confirmity_AttachmentId == null ? (object)DBNull.Value : item.REACH_SVHC_Confirmity_AttachmentId);
					sqlCommand.Parameters.AddWithValue("ROHS_EEE_Confirmity_AttachmentId", item.ROHS_EEE_Confirmity_AttachmentId == null ? (object)DBNull.Value : item.ROHS_EEE_Confirmity_AttachmentId);
					sqlCommand.Parameters.AddWithValue("SpecialCustomerReleases__DeviationReleases", item.SpecialCustomerReleases__DeviationReleases == null ? (object)DBNull.Value : item.SpecialCustomerReleases__DeviationReleases);
					sqlCommand.Parameters.AddWithValue("SpecialCustomerReleases__DeviationReleases_AttachmentId", item.SpecialCustomerReleases__DeviationReleases_AttachmentId == null ? (object)DBNull.Value : item.SpecialCustomerReleases__DeviationReleases_AttachmentId);
					sqlCommand.Parameters.AddWithValue("TSP_Available", item.TSP_Available == null ? (object)DBNull.Value : item.TSP_Available);
					sqlCommand.Parameters.AddWithValue("TSP_Available_AttachmentId", item.TSP_Available_AttachmentId == null ? (object)DBNull.Value : item.TSP_Available_AttachmentId);
					sqlCommand.Parameters.AddWithValue("UL_Etikett_AttachmentId", item.UL_Etikett_AttachmentId == null ? (object)DBNull.Value : item.UL_Etikett_AttachmentId);
					sqlCommand.Parameters.AddWithValue("UL_zertifiziert_AttachmentId", item.UL_zertifiziert_AttachmentId == null ? (object)DBNull.Value : item.UL_zertifiziert_AttachmentId);
					sqlCommand.Parameters.AddWithValue("UpdateTime", item.UpdateTime == null ? (object)DBNull.Value : item.UpdateTime);
					sqlCommand.Parameters.AddWithValue("UpdateUserId", item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 29; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity> items)
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
						query += " INSERT INTO [__BSD_ArtikelQualityExtension] ([ArticleId],[CoC_Pflichtig_AttachmentId],[CreateTime],[CreateUserId],[Dienstleistung_AttachmentId],[EMPB_AttachmentId],[EMPB_Freigegeben_AttachmentId],[ESD_AttachmentId],[HM_AttachmentId],[LLE_AttachmentId],[MHD_AttachmentId],[MineralsConfirmity_AttachmentId],[PackagingRegulation_Available],[PackagingRegulation_Available_AttachmentId],[PurchasingArticleInspection__SpecialArticlesCustomerSpecific],[PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId],[QSV],[QSV_AttachmentId],[REACH_SVHC_Confirmity_AttachmentId],[ROHS_EEE_Confirmity_AttachmentId],[SpecialCustomerReleases__DeviationReleases],[SpecialCustomerReleases__DeviationReleases_AttachmentId],[TSP_Available],[TSP_Available_AttachmentId],[UL_Etikett_AttachmentId],[UL_zertifiziert_AttachmentId],[UpdateTime],[UpdateUserId]) VALUES ( "

							+ "@ArticleId" + i + ","
							+ "@CoC_Pflichtig_AttachmentId" + i + ","
							+ "@CreateTime" + i + ","
							+ "@CreateUserId" + i + ","
							+ "@Dienstleistung_AttachmentId" + i + ","
							+ "@EMPB_AttachmentId" + i + ","
							+ "@EMPB_Freigegeben_AttachmentId" + i + ","
							+ "@ESD_AttachmentId" + i + ","
							+ "@HM_AttachmentId" + i + ","
							+ "@LLE_AttachmentId" + i + ","
							+ "@MHD_AttachmentId" + i + ","
							+ "@MineralsConfirmity_AttachmentId" + i + ","
							+ "@PackagingRegulation_Available" + i + ","
							+ "@PackagingRegulation_Available_AttachmentId" + i + ","
							+ "@PurchasingArticleInspection__SpecialArticlesCustomerSpecific" + i + ","
							+ "@PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId" + i + ","
							+ "@QSV" + i + ","
							+ "@QSV_AttachmentId" + i + ","
							+ "@REACH_SVHC_Confirmity_AttachmentId" + i + ","
							+ "@ROHS_EEE_Confirmity_AttachmentId" + i + ","
							+ "@SpecialCustomerReleases__DeviationReleases" + i + ","
							+ "@SpecialCustomerReleases__DeviationReleases_AttachmentId" + i + ","
							+ "@TSP_Available" + i + ","
							+ "@TSP_Available_AttachmentId" + i + ","
							+ "@UL_Etikett_AttachmentId" + i + ","
							+ "@UL_zertifiziert_AttachmentId" + i + ","
							+ "@UpdateTime" + i + ","
							+ "@UpdateUserId" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId);
						sqlCommand.Parameters.AddWithValue("CoC_Pflichtig_AttachmentId" + i, item.CoC_Pflichtig_AttachmentId == null ? (object)DBNull.Value : item.CoC_Pflichtig_AttachmentId);
						sqlCommand.Parameters.AddWithValue("CreateTime" + i, item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
						sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
						sqlCommand.Parameters.AddWithValue("Dienstleistung_AttachmentId" + i, item.Dienstleistung_AttachmentId == null ? (object)DBNull.Value : item.Dienstleistung_AttachmentId);
						sqlCommand.Parameters.AddWithValue("EMPB_AttachmentId" + i, item.EMPB_AttachmentId == null ? (object)DBNull.Value : item.EMPB_AttachmentId);
						sqlCommand.Parameters.AddWithValue("EMPB_Freigegeben_AttachmentId" + i, item.EMPB_Freigegeben_AttachmentId == null ? (object)DBNull.Value : item.EMPB_Freigegeben_AttachmentId);
						sqlCommand.Parameters.AddWithValue("ESD_AttachmentId" + i, item.ESD_AttachmentId == null ? (object)DBNull.Value : item.ESD_AttachmentId);
						sqlCommand.Parameters.AddWithValue("HM_AttachmentId" + i, item.HM_AttachmentId == null ? (object)DBNull.Value : item.HM_AttachmentId);
						sqlCommand.Parameters.AddWithValue("LLE_AttachmentId" + i, item.LLE_AttachmentId == null ? (object)DBNull.Value : item.LLE_AttachmentId);
						sqlCommand.Parameters.AddWithValue("MHD_AttachmentId" + i, item.MHD_AttachmentId == null ? (object)DBNull.Value : item.MHD_AttachmentId);
						sqlCommand.Parameters.AddWithValue("MineralsConfirmity_AttachmentId" + i, item.MineralsConfirmity_AttachmentId == null ? (object)DBNull.Value : item.MineralsConfirmity_AttachmentId);
						sqlCommand.Parameters.AddWithValue("PackagingRegulation_Available" + i, item.PackagingRegulation_Available == null ? (object)DBNull.Value : item.PackagingRegulation_Available);
						sqlCommand.Parameters.AddWithValue("PackagingRegulation_Available_AttachmentId" + i, item.PackagingRegulation_Available_AttachmentId == null ? (object)DBNull.Value : item.PackagingRegulation_Available_AttachmentId);
						sqlCommand.Parameters.AddWithValue("PurchasingArticleInspection__SpecialArticlesCustomerSpecific" + i, item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific == null ? (object)DBNull.Value : item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific);
						sqlCommand.Parameters.AddWithValue("PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId" + i, item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId == null ? (object)DBNull.Value : item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId);
						sqlCommand.Parameters.AddWithValue("QSV" + i, item.QSV == null ? (object)DBNull.Value : item.QSV);
						sqlCommand.Parameters.AddWithValue("QSV_AttachmentId" + i, item.QSV_AttachmentId == null ? (object)DBNull.Value : item.QSV_AttachmentId);
						sqlCommand.Parameters.AddWithValue("REACH_SVHC_Confirmity_AttachmentId" + i, item.REACH_SVHC_Confirmity_AttachmentId == null ? (object)DBNull.Value : item.REACH_SVHC_Confirmity_AttachmentId);
						sqlCommand.Parameters.AddWithValue("ROHS_EEE_Confirmity_AttachmentId" + i, item.ROHS_EEE_Confirmity_AttachmentId == null ? (object)DBNull.Value : item.ROHS_EEE_Confirmity_AttachmentId);
						sqlCommand.Parameters.AddWithValue("SpecialCustomerReleases__DeviationReleases" + i, item.SpecialCustomerReleases__DeviationReleases == null ? (object)DBNull.Value : item.SpecialCustomerReleases__DeviationReleases);
						sqlCommand.Parameters.AddWithValue("SpecialCustomerReleases__DeviationReleases_AttachmentId" + i, item.SpecialCustomerReleases__DeviationReleases_AttachmentId == null ? (object)DBNull.Value : item.SpecialCustomerReleases__DeviationReleases_AttachmentId);
						sqlCommand.Parameters.AddWithValue("TSP_Available" + i, item.TSP_Available == null ? (object)DBNull.Value : item.TSP_Available);
						sqlCommand.Parameters.AddWithValue("TSP_Available_AttachmentId" + i, item.TSP_Available_AttachmentId == null ? (object)DBNull.Value : item.TSP_Available_AttachmentId);
						sqlCommand.Parameters.AddWithValue("UL_Etikett_AttachmentId" + i, item.UL_Etikett_AttachmentId == null ? (object)DBNull.Value : item.UL_Etikett_AttachmentId);
						sqlCommand.Parameters.AddWithValue("UL_zertifiziert_AttachmentId" + i, item.UL_zertifiziert_AttachmentId == null ? (object)DBNull.Value : item.UL_zertifiziert_AttachmentId);
						sqlCommand.Parameters.AddWithValue("UpdateTime" + i, item.UpdateTime == null ? (object)DBNull.Value : item.UpdateTime);
						sqlCommand.Parameters.AddWithValue("UpdateUserId" + i, item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__BSD_ArtikelQualityExtension] SET [ArticleId]=@ArticleId, [CoC_Pflichtig_AttachmentId]=@CoC_Pflichtig_AttachmentId, [CreateTime]=@CreateTime, [CreateUserId]=@CreateUserId, [Dienstleistung_AttachmentId]=@Dienstleistung_AttachmentId, [EMPB_AttachmentId]=@EMPB_AttachmentId, [EMPB_Freigegeben_AttachmentId]=@EMPB_Freigegeben_AttachmentId, [ESD_AttachmentId]=@ESD_AttachmentId, [HM_AttachmentId]=@HM_AttachmentId, [LLE_AttachmentId]=@LLE_AttachmentId, [MHD_AttachmentId]=@MHD_AttachmentId, [MineralsConfirmity_AttachmentId]=@MineralsConfirmity_AttachmentId, [PackagingRegulation_Available]=@PackagingRegulation_Available, [PackagingRegulation_Available_AttachmentId]=@PackagingRegulation_Available_AttachmentId, [PurchasingArticleInspection__SpecialArticlesCustomerSpecific]=@PurchasingArticleInspection__SpecialArticlesCustomerSpecific, [PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId]=@PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId, [QSV]=@QSV, [QSV_AttachmentId]=@QSV_AttachmentId, [REACH_SVHC_Confirmity_AttachmentId]=@REACH_SVHC_Confirmity_AttachmentId, [ROHS_EEE_Confirmity_AttachmentId]=@ROHS_EEE_Confirmity_AttachmentId, [SpecialCustomerReleases__DeviationReleases]=@SpecialCustomerReleases__DeviationReleases, [SpecialCustomerReleases__DeviationReleases_AttachmentId]=@SpecialCustomerReleases__DeviationReleases_AttachmentId, [TSP_Available]=@TSP_Available, [TSP_Available_AttachmentId]=@TSP_Available_AttachmentId, [UL_Etikett_AttachmentId]=@UL_Etikett_AttachmentId, [UL_zertifiziert_AttachmentId]=@UL_zertifiziert_AttachmentId, [UpdateTime]=@UpdateTime, [UpdateUserId]=@UpdateUserId WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId);
				sqlCommand.Parameters.AddWithValue("CoC_Pflichtig_AttachmentId", item.CoC_Pflichtig_AttachmentId == null ? (object)DBNull.Value : item.CoC_Pflichtig_AttachmentId);
				sqlCommand.Parameters.AddWithValue("CreateTime", item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
				sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
				sqlCommand.Parameters.AddWithValue("Dienstleistung_AttachmentId", item.Dienstleistung_AttachmentId == null ? (object)DBNull.Value : item.Dienstleistung_AttachmentId);
				sqlCommand.Parameters.AddWithValue("EMPB_AttachmentId", item.EMPB_AttachmentId == null ? (object)DBNull.Value : item.EMPB_AttachmentId);
				sqlCommand.Parameters.AddWithValue("EMPB_Freigegeben_AttachmentId", item.EMPB_Freigegeben_AttachmentId == null ? (object)DBNull.Value : item.EMPB_Freigegeben_AttachmentId);
				sqlCommand.Parameters.AddWithValue("ESD_AttachmentId", item.ESD_AttachmentId == null ? (object)DBNull.Value : item.ESD_AttachmentId);
				sqlCommand.Parameters.AddWithValue("HM_AttachmentId", item.HM_AttachmentId == null ? (object)DBNull.Value : item.HM_AttachmentId);
				sqlCommand.Parameters.AddWithValue("LLE_AttachmentId", item.LLE_AttachmentId == null ? (object)DBNull.Value : item.LLE_AttachmentId);
				sqlCommand.Parameters.AddWithValue("MHD_AttachmentId", item.MHD_AttachmentId == null ? (object)DBNull.Value : item.MHD_AttachmentId);
				sqlCommand.Parameters.AddWithValue("MineralsConfirmity_AttachmentId", item.MineralsConfirmity_AttachmentId == null ? (object)DBNull.Value : item.MineralsConfirmity_AttachmentId);
				sqlCommand.Parameters.AddWithValue("PackagingRegulation_Available", item.PackagingRegulation_Available == null ? (object)DBNull.Value : item.PackagingRegulation_Available);
				sqlCommand.Parameters.AddWithValue("PackagingRegulation_Available_AttachmentId", item.PackagingRegulation_Available_AttachmentId == null ? (object)DBNull.Value : item.PackagingRegulation_Available_AttachmentId);
				sqlCommand.Parameters.AddWithValue("PurchasingArticleInspection__SpecialArticlesCustomerSpecific", item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific == null ? (object)DBNull.Value : item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific);
				sqlCommand.Parameters.AddWithValue("PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId", item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId == null ? (object)DBNull.Value : item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId);
				sqlCommand.Parameters.AddWithValue("QSV", item.QSV == null ? (object)DBNull.Value : item.QSV);
				sqlCommand.Parameters.AddWithValue("QSV_AttachmentId", item.QSV_AttachmentId == null ? (object)DBNull.Value : item.QSV_AttachmentId);
				sqlCommand.Parameters.AddWithValue("REACH_SVHC_Confirmity_AttachmentId", item.REACH_SVHC_Confirmity_AttachmentId == null ? (object)DBNull.Value : item.REACH_SVHC_Confirmity_AttachmentId);
				sqlCommand.Parameters.AddWithValue("ROHS_EEE_Confirmity_AttachmentId", item.ROHS_EEE_Confirmity_AttachmentId == null ? (object)DBNull.Value : item.ROHS_EEE_Confirmity_AttachmentId);
				sqlCommand.Parameters.AddWithValue("SpecialCustomerReleases__DeviationReleases", item.SpecialCustomerReleases__DeviationReleases == null ? (object)DBNull.Value : item.SpecialCustomerReleases__DeviationReleases);
				sqlCommand.Parameters.AddWithValue("SpecialCustomerReleases__DeviationReleases_AttachmentId", item.SpecialCustomerReleases__DeviationReleases_AttachmentId == null ? (object)DBNull.Value : item.SpecialCustomerReleases__DeviationReleases_AttachmentId);
				sqlCommand.Parameters.AddWithValue("TSP_Available", item.TSP_Available == null ? (object)DBNull.Value : item.TSP_Available);
				sqlCommand.Parameters.AddWithValue("TSP_Available_AttachmentId", item.TSP_Available_AttachmentId == null ? (object)DBNull.Value : item.TSP_Available_AttachmentId);
				sqlCommand.Parameters.AddWithValue("UL_Etikett_AttachmentId", item.UL_Etikett_AttachmentId == null ? (object)DBNull.Value : item.UL_Etikett_AttachmentId);
				sqlCommand.Parameters.AddWithValue("UL_zertifiziert_AttachmentId", item.UL_zertifiziert_AttachmentId == null ? (object)DBNull.Value : item.UL_zertifiziert_AttachmentId);
				sqlCommand.Parameters.AddWithValue("UpdateTime", item.UpdateTime == null ? (object)DBNull.Value : item.UpdateTime);
				sqlCommand.Parameters.AddWithValue("UpdateUserId", item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 29; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity> items)
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
						query += " UPDATE [__BSD_ArtikelQualityExtension] SET "

							+ "[ArticleId]=@ArticleId" + i + ","
							+ "[CoC_Pflichtig_AttachmentId]=@CoC_Pflichtig_AttachmentId" + i + ","
							+ "[CreateTime]=@CreateTime" + i + ","
							+ "[CreateUserId]=@CreateUserId" + i + ","
							+ "[Dienstleistung_AttachmentId]=@Dienstleistung_AttachmentId" + i + ","
							+ "[EMPB_AttachmentId]=@EMPB_AttachmentId" + i + ","
							+ "[EMPB_Freigegeben_AttachmentId]=@EMPB_Freigegeben_AttachmentId" + i + ","
							+ "[ESD_AttachmentId]=@ESD_AttachmentId" + i + ","
							+ "[HM_AttachmentId]=@HM_AttachmentId" + i + ","
							+ "[LLE_AttachmentId]=@LLE_AttachmentId" + i + ","
							+ "[MHD_AttachmentId]=@MHD_AttachmentId" + i + ","
							+ "[MineralsConfirmity_AttachmentId]=@MineralsConfirmity_AttachmentId" + i + ","
							+ "[PackagingRegulation_Available]=@PackagingRegulation_Available" + i + ","
							+ "[PackagingRegulation_Available_AttachmentId]=@PackagingRegulation_Available_AttachmentId" + i + ","
							+ "[PurchasingArticleInspection__SpecialArticlesCustomerSpecific]=@PurchasingArticleInspection__SpecialArticlesCustomerSpecific" + i + ","
							+ "[PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId]=@PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId" + i + ","
							+ "[QSV]=@QSV" + i + ","
							+ "[QSV_AttachmentId]=@QSV_AttachmentId" + i + ","
							+ "[REACH_SVHC_Confirmity_AttachmentId]=@REACH_SVHC_Confirmity_AttachmentId" + i + ","
							+ "[ROHS_EEE_Confirmity_AttachmentId]=@ROHS_EEE_Confirmity_AttachmentId" + i + ","
							+ "[SpecialCustomerReleases__DeviationReleases]=@SpecialCustomerReleases__DeviationReleases" + i + ","
							+ "[SpecialCustomerReleases__DeviationReleases_AttachmentId]=@SpecialCustomerReleases__DeviationReleases_AttachmentId" + i + ","
							+ "[TSP_Available]=@TSP_Available" + i + ","
							+ "[TSP_Available_AttachmentId]=@TSP_Available_AttachmentId" + i + ","
							+ "[UL_Etikett_AttachmentId]=@UL_Etikett_AttachmentId" + i + ","
							+ "[UL_zertifiziert_AttachmentId]=@UL_zertifiziert_AttachmentId" + i + ","
							+ "[UpdateTime]=@UpdateTime" + i + ","
							+ "[UpdateUserId]=@UpdateUserId" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId);
						sqlCommand.Parameters.AddWithValue("CoC_Pflichtig_AttachmentId" + i, item.CoC_Pflichtig_AttachmentId == null ? (object)DBNull.Value : item.CoC_Pflichtig_AttachmentId);
						sqlCommand.Parameters.AddWithValue("CreateTime" + i, item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
						sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
						sqlCommand.Parameters.AddWithValue("Dienstleistung_AttachmentId" + i, item.Dienstleistung_AttachmentId == null ? (object)DBNull.Value : item.Dienstleistung_AttachmentId);
						sqlCommand.Parameters.AddWithValue("EMPB_AttachmentId" + i, item.EMPB_AttachmentId == null ? (object)DBNull.Value : item.EMPB_AttachmentId);
						sqlCommand.Parameters.AddWithValue("EMPB_Freigegeben_AttachmentId" + i, item.EMPB_Freigegeben_AttachmentId == null ? (object)DBNull.Value : item.EMPB_Freigegeben_AttachmentId);
						sqlCommand.Parameters.AddWithValue("ESD_AttachmentId" + i, item.ESD_AttachmentId == null ? (object)DBNull.Value : item.ESD_AttachmentId);
						sqlCommand.Parameters.AddWithValue("HM_AttachmentId" + i, item.HM_AttachmentId == null ? (object)DBNull.Value : item.HM_AttachmentId);
						sqlCommand.Parameters.AddWithValue("LLE_AttachmentId" + i, item.LLE_AttachmentId == null ? (object)DBNull.Value : item.LLE_AttachmentId);
						sqlCommand.Parameters.AddWithValue("MHD_AttachmentId" + i, item.MHD_AttachmentId == null ? (object)DBNull.Value : item.MHD_AttachmentId);
						sqlCommand.Parameters.AddWithValue("MineralsConfirmity_AttachmentId" + i, item.MineralsConfirmity_AttachmentId == null ? (object)DBNull.Value : item.MineralsConfirmity_AttachmentId);
						sqlCommand.Parameters.AddWithValue("PackagingRegulation_Available" + i, item.PackagingRegulation_Available == null ? (object)DBNull.Value : item.PackagingRegulation_Available);
						sqlCommand.Parameters.AddWithValue("PackagingRegulation_Available_AttachmentId" + i, item.PackagingRegulation_Available_AttachmentId == null ? (object)DBNull.Value : item.PackagingRegulation_Available_AttachmentId);
						sqlCommand.Parameters.AddWithValue("PurchasingArticleInspection__SpecialArticlesCustomerSpecific" + i, item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific == null ? (object)DBNull.Value : item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific);
						sqlCommand.Parameters.AddWithValue("PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId" + i, item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId == null ? (object)DBNull.Value : item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId);
						sqlCommand.Parameters.AddWithValue("QSV" + i, item.QSV == null ? (object)DBNull.Value : item.QSV);
						sqlCommand.Parameters.AddWithValue("QSV_AttachmentId" + i, item.QSV_AttachmentId == null ? (object)DBNull.Value : item.QSV_AttachmentId);
						sqlCommand.Parameters.AddWithValue("REACH_SVHC_Confirmity_AttachmentId" + i, item.REACH_SVHC_Confirmity_AttachmentId == null ? (object)DBNull.Value : item.REACH_SVHC_Confirmity_AttachmentId);
						sqlCommand.Parameters.AddWithValue("ROHS_EEE_Confirmity_AttachmentId" + i, item.ROHS_EEE_Confirmity_AttachmentId == null ? (object)DBNull.Value : item.ROHS_EEE_Confirmity_AttachmentId);
						sqlCommand.Parameters.AddWithValue("SpecialCustomerReleases__DeviationReleases" + i, item.SpecialCustomerReleases__DeviationReleases == null ? (object)DBNull.Value : item.SpecialCustomerReleases__DeviationReleases);
						sqlCommand.Parameters.AddWithValue("SpecialCustomerReleases__DeviationReleases_AttachmentId" + i, item.SpecialCustomerReleases__DeviationReleases_AttachmentId == null ? (object)DBNull.Value : item.SpecialCustomerReleases__DeviationReleases_AttachmentId);
						sqlCommand.Parameters.AddWithValue("TSP_Available" + i, item.TSP_Available == null ? (object)DBNull.Value : item.TSP_Available);
						sqlCommand.Parameters.AddWithValue("TSP_Available_AttachmentId" + i, item.TSP_Available_AttachmentId == null ? (object)DBNull.Value : item.TSP_Available_AttachmentId);
						sqlCommand.Parameters.AddWithValue("UL_Etikett_AttachmentId" + i, item.UL_Etikett_AttachmentId == null ? (object)DBNull.Value : item.UL_Etikett_AttachmentId);
						sqlCommand.Parameters.AddWithValue("UL_zertifiziert_AttachmentId" + i, item.UL_zertifiziert_AttachmentId == null ? (object)DBNull.Value : item.UL_zertifiziert_AttachmentId);
						sqlCommand.Parameters.AddWithValue("UpdateTime" + i, item.UpdateTime == null ? (object)DBNull.Value : item.UpdateTime);
						sqlCommand.Parameters.AddWithValue("UpdateUserId" + i, item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);
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
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__BSD_ArtikelQualityExtension] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__BSD_ArtikelQualityExtension] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__BSD_ArtikelQualityExtension] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__BSD_ArtikelQualityExtension]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__BSD_ArtikelQualityExtension] WHERE [Id] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [__BSD_ArtikelQualityExtension] ([ArticleId],[CoC_Pflichtig_AttachmentId],[CreateTime],[CreateUserId],[Dienstleistung_AttachmentId],[EMPB_AttachmentId],[EMPB_Freigegeben_AttachmentId],[ESD_AttachmentId],[HM_AttachmentId],[LLE_AttachmentId],[MHD_AttachmentId],[MineralsConfirmity_AttachmentId],[PackagingRegulation_Available],[PackagingRegulation_Available_AttachmentId],[PurchasingArticleInspection__SpecialArticlesCustomerSpecific],[PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId],[QSV],[QSV_AttachmentId],[REACH_SVHC_Confirmity_AttachmentId],[ROHS_EEE_Confirmity_AttachmentId],[SpecialCustomerReleases__DeviationReleases],[SpecialCustomerReleases__DeviationReleases_AttachmentId],[TSP_Available],[TSP_Available_AttachmentId],[UL_Etikett_AttachmentId],[UL_zertifiziert_AttachmentId],[UpdateTime],[UpdateUserId]) OUTPUT INSERTED.[Id] VALUES (@ArticleId,@CoC_Pflichtig_AttachmentId,@CreateTime,@CreateUserId,@Dienstleistung_AttachmentId,@EMPB_AttachmentId,@EMPB_Freigegeben_AttachmentId,@ESD_AttachmentId,@HM_AttachmentId,@LLE_AttachmentId,@MHD_AttachmentId,@MineralsConfirmity_AttachmentId,@PackagingRegulation_Available,@PackagingRegulation_Available_AttachmentId,@PurchasingArticleInspection__SpecialArticlesCustomerSpecific,@PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId,@QSV,@QSV_AttachmentId,@REACH_SVHC_Confirmity_AttachmentId,@ROHS_EEE_Confirmity_AttachmentId,@SpecialCustomerReleases__DeviationReleases,@SpecialCustomerReleases__DeviationReleases_AttachmentId,@TSP_Available,@TSP_Available_AttachmentId,@UL_Etikett_AttachmentId,@UL_zertifiziert_AttachmentId,@UpdateTime,@UpdateUserId); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId);
			sqlCommand.Parameters.AddWithValue("CoC_Pflichtig_AttachmentId", item.CoC_Pflichtig_AttachmentId == null ? (object)DBNull.Value : item.CoC_Pflichtig_AttachmentId);
			sqlCommand.Parameters.AddWithValue("CreateTime", item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
			sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
			sqlCommand.Parameters.AddWithValue("Dienstleistung_AttachmentId", item.Dienstleistung_AttachmentId == null ? (object)DBNull.Value : item.Dienstleistung_AttachmentId);
			sqlCommand.Parameters.AddWithValue("EMPB_AttachmentId", item.EMPB_AttachmentId == null ? (object)DBNull.Value : item.EMPB_AttachmentId);
			sqlCommand.Parameters.AddWithValue("EMPB_Freigegeben_AttachmentId", item.EMPB_Freigegeben_AttachmentId == null ? (object)DBNull.Value : item.EMPB_Freigegeben_AttachmentId);
			sqlCommand.Parameters.AddWithValue("ESD_AttachmentId", item.ESD_AttachmentId == null ? (object)DBNull.Value : item.ESD_AttachmentId);
			sqlCommand.Parameters.AddWithValue("HM_AttachmentId", item.HM_AttachmentId == null ? (object)DBNull.Value : item.HM_AttachmentId);
			sqlCommand.Parameters.AddWithValue("LLE_AttachmentId", item.LLE_AttachmentId == null ? (object)DBNull.Value : item.LLE_AttachmentId);
			sqlCommand.Parameters.AddWithValue("MHD_AttachmentId", item.MHD_AttachmentId == null ? (object)DBNull.Value : item.MHD_AttachmentId);
			sqlCommand.Parameters.AddWithValue("MineralsConfirmity_AttachmentId", item.MineralsConfirmity_AttachmentId == null ? (object)DBNull.Value : item.MineralsConfirmity_AttachmentId);
			sqlCommand.Parameters.AddWithValue("PackagingRegulation_Available", item.PackagingRegulation_Available == null ? (object)DBNull.Value : item.PackagingRegulation_Available);
			sqlCommand.Parameters.AddWithValue("PackagingRegulation_Available_AttachmentId", item.PackagingRegulation_Available_AttachmentId == null ? (object)DBNull.Value : item.PackagingRegulation_Available_AttachmentId);
			sqlCommand.Parameters.AddWithValue("PurchasingArticleInspection__SpecialArticlesCustomerSpecific", item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific == null ? (object)DBNull.Value : item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific);
			sqlCommand.Parameters.AddWithValue("PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId", item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId == null ? (object)DBNull.Value : item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId);
			sqlCommand.Parameters.AddWithValue("QSV", item.QSV == null ? (object)DBNull.Value : item.QSV);
			sqlCommand.Parameters.AddWithValue("QSV_AttachmentId", item.QSV_AttachmentId == null ? (object)DBNull.Value : item.QSV_AttachmentId);
			sqlCommand.Parameters.AddWithValue("REACH_SVHC_Confirmity_AttachmentId", item.REACH_SVHC_Confirmity_AttachmentId == null ? (object)DBNull.Value : item.REACH_SVHC_Confirmity_AttachmentId);
			sqlCommand.Parameters.AddWithValue("ROHS_EEE_Confirmity_AttachmentId", item.ROHS_EEE_Confirmity_AttachmentId == null ? (object)DBNull.Value : item.ROHS_EEE_Confirmity_AttachmentId);
			sqlCommand.Parameters.AddWithValue("SpecialCustomerReleases__DeviationReleases", item.SpecialCustomerReleases__DeviationReleases == null ? (object)DBNull.Value : item.SpecialCustomerReleases__DeviationReleases);
			sqlCommand.Parameters.AddWithValue("SpecialCustomerReleases__DeviationReleases_AttachmentId", item.SpecialCustomerReleases__DeviationReleases_AttachmentId == null ? (object)DBNull.Value : item.SpecialCustomerReleases__DeviationReleases_AttachmentId);
			sqlCommand.Parameters.AddWithValue("TSP_Available", item.TSP_Available == null ? (object)DBNull.Value : item.TSP_Available);
			sqlCommand.Parameters.AddWithValue("TSP_Available_AttachmentId", item.TSP_Available_AttachmentId == null ? (object)DBNull.Value : item.TSP_Available_AttachmentId);
			sqlCommand.Parameters.AddWithValue("UL_Etikett_AttachmentId", item.UL_Etikett_AttachmentId == null ? (object)DBNull.Value : item.UL_Etikett_AttachmentId);
			sqlCommand.Parameters.AddWithValue("UL_zertifiziert_AttachmentId", item.UL_zertifiziert_AttachmentId == null ? (object)DBNull.Value : item.UL_zertifiziert_AttachmentId);
			sqlCommand.Parameters.AddWithValue("UpdateTime", item.UpdateTime == null ? (object)DBNull.Value : item.UpdateTime);
			sqlCommand.Parameters.AddWithValue("UpdateUserId", item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 29; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__BSD_ArtikelQualityExtension] ([ArticleId],[CoC_Pflichtig_AttachmentId],[CreateTime],[CreateUserId],[Dienstleistung_AttachmentId],[EMPB_AttachmentId],[EMPB_Freigegeben_AttachmentId],[ESD_AttachmentId],[HM_AttachmentId],[LLE_AttachmentId],[MHD_AttachmentId],[MineralsConfirmity_AttachmentId],[PackagingRegulation_Available],[PackagingRegulation_Available_AttachmentId],[PurchasingArticleInspection__SpecialArticlesCustomerSpecific],[PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId],[QSV],[QSV_AttachmentId],[REACH_SVHC_Confirmity_AttachmentId],[ROHS_EEE_Confirmity_AttachmentId],[SpecialCustomerReleases__DeviationReleases],[SpecialCustomerReleases__DeviationReleases_AttachmentId],[TSP_Available],[TSP_Available_AttachmentId],[UL_Etikett_AttachmentId],[UL_zertifiziert_AttachmentId],[UpdateTime],[UpdateUserId]) VALUES ( "

						+ "@ArticleId" + i + ","
						+ "@CoC_Pflichtig_AttachmentId" + i + ","
						+ "@CreateTime" + i + ","
						+ "@CreateUserId" + i + ","
						+ "@Dienstleistung_AttachmentId" + i + ","
						+ "@EMPB_AttachmentId" + i + ","
						+ "@EMPB_Freigegeben_AttachmentId" + i + ","
						+ "@ESD_AttachmentId" + i + ","
						+ "@HM_AttachmentId" + i + ","
						+ "@LLE_AttachmentId" + i + ","
						+ "@MHD_AttachmentId" + i + ","
						+ "@MineralsConfirmity_AttachmentId" + i + ","
						+ "@PackagingRegulation_Available" + i + ","
						+ "@PackagingRegulation_Available_AttachmentId" + i + ","
						+ "@PurchasingArticleInspection__SpecialArticlesCustomerSpecific" + i + ","
						+ "@PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId" + i + ","
						+ "@QSV" + i + ","
						+ "@QSV_AttachmentId" + i + ","
						+ "@REACH_SVHC_Confirmity_AttachmentId" + i + ","
						+ "@ROHS_EEE_Confirmity_AttachmentId" + i + ","
						+ "@SpecialCustomerReleases__DeviationReleases" + i + ","
						+ "@SpecialCustomerReleases__DeviationReleases_AttachmentId" + i + ","
						+ "@TSP_Available" + i + ","
						+ "@TSP_Available_AttachmentId" + i + ","
						+ "@UL_Etikett_AttachmentId" + i + ","
						+ "@UL_zertifiziert_AttachmentId" + i + ","
						+ "@UpdateTime" + i + ","
						+ "@UpdateUserId" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId);
					sqlCommand.Parameters.AddWithValue("CoC_Pflichtig_AttachmentId" + i, item.CoC_Pflichtig_AttachmentId == null ? (object)DBNull.Value : item.CoC_Pflichtig_AttachmentId);
					sqlCommand.Parameters.AddWithValue("CreateTime" + i, item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
					sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
					sqlCommand.Parameters.AddWithValue("Dienstleistung_AttachmentId" + i, item.Dienstleistung_AttachmentId == null ? (object)DBNull.Value : item.Dienstleistung_AttachmentId);
					sqlCommand.Parameters.AddWithValue("EMPB_AttachmentId" + i, item.EMPB_AttachmentId == null ? (object)DBNull.Value : item.EMPB_AttachmentId);
					sqlCommand.Parameters.AddWithValue("EMPB_Freigegeben_AttachmentId" + i, item.EMPB_Freigegeben_AttachmentId == null ? (object)DBNull.Value : item.EMPB_Freigegeben_AttachmentId);
					sqlCommand.Parameters.AddWithValue("ESD_AttachmentId" + i, item.ESD_AttachmentId == null ? (object)DBNull.Value : item.ESD_AttachmentId);
					sqlCommand.Parameters.AddWithValue("HM_AttachmentId" + i, item.HM_AttachmentId == null ? (object)DBNull.Value : item.HM_AttachmentId);
					sqlCommand.Parameters.AddWithValue("LLE_AttachmentId" + i, item.LLE_AttachmentId == null ? (object)DBNull.Value : item.LLE_AttachmentId);
					sqlCommand.Parameters.AddWithValue("MHD_AttachmentId" + i, item.MHD_AttachmentId == null ? (object)DBNull.Value : item.MHD_AttachmentId);
					sqlCommand.Parameters.AddWithValue("MineralsConfirmity_AttachmentId" + i, item.MineralsConfirmity_AttachmentId == null ? (object)DBNull.Value : item.MineralsConfirmity_AttachmentId);
					sqlCommand.Parameters.AddWithValue("PackagingRegulation_Available" + i, item.PackagingRegulation_Available == null ? (object)DBNull.Value : item.PackagingRegulation_Available);
					sqlCommand.Parameters.AddWithValue("PackagingRegulation_Available_AttachmentId" + i, item.PackagingRegulation_Available_AttachmentId == null ? (object)DBNull.Value : item.PackagingRegulation_Available_AttachmentId);
					sqlCommand.Parameters.AddWithValue("PurchasingArticleInspection__SpecialArticlesCustomerSpecific" + i, item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific == null ? (object)DBNull.Value : item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific);
					sqlCommand.Parameters.AddWithValue("PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId" + i, item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId == null ? (object)DBNull.Value : item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId);
					sqlCommand.Parameters.AddWithValue("QSV" + i, item.QSV == null ? (object)DBNull.Value : item.QSV);
					sqlCommand.Parameters.AddWithValue("QSV_AttachmentId" + i, item.QSV_AttachmentId == null ? (object)DBNull.Value : item.QSV_AttachmentId);
					sqlCommand.Parameters.AddWithValue("REACH_SVHC_Confirmity_AttachmentId" + i, item.REACH_SVHC_Confirmity_AttachmentId == null ? (object)DBNull.Value : item.REACH_SVHC_Confirmity_AttachmentId);
					sqlCommand.Parameters.AddWithValue("ROHS_EEE_Confirmity_AttachmentId" + i, item.ROHS_EEE_Confirmity_AttachmentId == null ? (object)DBNull.Value : item.ROHS_EEE_Confirmity_AttachmentId);
					sqlCommand.Parameters.AddWithValue("SpecialCustomerReleases__DeviationReleases" + i, item.SpecialCustomerReleases__DeviationReleases == null ? (object)DBNull.Value : item.SpecialCustomerReleases__DeviationReleases);
					sqlCommand.Parameters.AddWithValue("SpecialCustomerReleases__DeviationReleases_AttachmentId" + i, item.SpecialCustomerReleases__DeviationReleases_AttachmentId == null ? (object)DBNull.Value : item.SpecialCustomerReleases__DeviationReleases_AttachmentId);
					sqlCommand.Parameters.AddWithValue("TSP_Available" + i, item.TSP_Available == null ? (object)DBNull.Value : item.TSP_Available);
					sqlCommand.Parameters.AddWithValue("TSP_Available_AttachmentId" + i, item.TSP_Available_AttachmentId == null ? (object)DBNull.Value : item.TSP_Available_AttachmentId);
					sqlCommand.Parameters.AddWithValue("UL_Etikett_AttachmentId" + i, item.UL_Etikett_AttachmentId == null ? (object)DBNull.Value : item.UL_Etikett_AttachmentId);
					sqlCommand.Parameters.AddWithValue("UL_zertifiziert_AttachmentId" + i, item.UL_zertifiziert_AttachmentId == null ? (object)DBNull.Value : item.UL_zertifiziert_AttachmentId);
					sqlCommand.Parameters.AddWithValue("UpdateTime" + i, item.UpdateTime == null ? (object)DBNull.Value : item.UpdateTime);
					sqlCommand.Parameters.AddWithValue("UpdateUserId" + i, item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__BSD_ArtikelQualityExtension] SET [ArticleId]=@ArticleId, [CoC_Pflichtig_AttachmentId]=@CoC_Pflichtig_AttachmentId, [CreateTime]=@CreateTime, [CreateUserId]=@CreateUserId, [Dienstleistung_AttachmentId]=@Dienstleistung_AttachmentId, [EMPB_AttachmentId]=@EMPB_AttachmentId, [EMPB_Freigegeben_AttachmentId]=@EMPB_Freigegeben_AttachmentId, [ESD_AttachmentId]=@ESD_AttachmentId, [HM_AttachmentId]=@HM_AttachmentId, [LLE_AttachmentId]=@LLE_AttachmentId, [MHD_AttachmentId]=@MHD_AttachmentId, [MineralsConfirmity_AttachmentId]=@MineralsConfirmity_AttachmentId, [PackagingRegulation_Available]=@PackagingRegulation_Available, [PackagingRegulation_Available_AttachmentId]=@PackagingRegulation_Available_AttachmentId, [PurchasingArticleInspection__SpecialArticlesCustomerSpecific]=@PurchasingArticleInspection__SpecialArticlesCustomerSpecific, [PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId]=@PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId, [QSV]=@QSV, [QSV_AttachmentId]=@QSV_AttachmentId, [REACH_SVHC_Confirmity_AttachmentId]=@REACH_SVHC_Confirmity_AttachmentId, [ROHS_EEE_Confirmity_AttachmentId]=@ROHS_EEE_Confirmity_AttachmentId, [SpecialCustomerReleases__DeviationReleases]=@SpecialCustomerReleases__DeviationReleases, [SpecialCustomerReleases__DeviationReleases_AttachmentId]=@SpecialCustomerReleases__DeviationReleases_AttachmentId, [TSP_Available]=@TSP_Available, [TSP_Available_AttachmentId]=@TSP_Available_AttachmentId, [UL_Etikett_AttachmentId]=@UL_Etikett_AttachmentId, [UL_zertifiziert_AttachmentId]=@UL_zertifiziert_AttachmentId, [UpdateTime]=@UpdateTime, [UpdateUserId]=@UpdateUserId WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId);
			sqlCommand.Parameters.AddWithValue("CoC_Pflichtig_AttachmentId", item.CoC_Pflichtig_AttachmentId == null ? (object)DBNull.Value : item.CoC_Pflichtig_AttachmentId);
			sqlCommand.Parameters.AddWithValue("CreateTime", item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
			sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
			sqlCommand.Parameters.AddWithValue("Dienstleistung_AttachmentId", item.Dienstleistung_AttachmentId == null ? (object)DBNull.Value : item.Dienstleistung_AttachmentId);
			sqlCommand.Parameters.AddWithValue("EMPB_AttachmentId", item.EMPB_AttachmentId == null ? (object)DBNull.Value : item.EMPB_AttachmentId);
			sqlCommand.Parameters.AddWithValue("EMPB_Freigegeben_AttachmentId", item.EMPB_Freigegeben_AttachmentId == null ? (object)DBNull.Value : item.EMPB_Freigegeben_AttachmentId);
			sqlCommand.Parameters.AddWithValue("ESD_AttachmentId", item.ESD_AttachmentId == null ? (object)DBNull.Value : item.ESD_AttachmentId);
			sqlCommand.Parameters.AddWithValue("HM_AttachmentId", item.HM_AttachmentId == null ? (object)DBNull.Value : item.HM_AttachmentId);
			sqlCommand.Parameters.AddWithValue("LLE_AttachmentId", item.LLE_AttachmentId == null ? (object)DBNull.Value : item.LLE_AttachmentId);
			sqlCommand.Parameters.AddWithValue("MHD_AttachmentId", item.MHD_AttachmentId == null ? (object)DBNull.Value : item.MHD_AttachmentId);
			sqlCommand.Parameters.AddWithValue("MineralsConfirmity_AttachmentId", item.MineralsConfirmity_AttachmentId == null ? (object)DBNull.Value : item.MineralsConfirmity_AttachmentId);
			sqlCommand.Parameters.AddWithValue("PackagingRegulation_Available", item.PackagingRegulation_Available == null ? (object)DBNull.Value : item.PackagingRegulation_Available);
			sqlCommand.Parameters.AddWithValue("PackagingRegulation_Available_AttachmentId", item.PackagingRegulation_Available_AttachmentId == null ? (object)DBNull.Value : item.PackagingRegulation_Available_AttachmentId);
			sqlCommand.Parameters.AddWithValue("PurchasingArticleInspection__SpecialArticlesCustomerSpecific", item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific == null ? (object)DBNull.Value : item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific);
			sqlCommand.Parameters.AddWithValue("PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId", item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId == null ? (object)DBNull.Value : item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId);
			sqlCommand.Parameters.AddWithValue("QSV", item.QSV == null ? (object)DBNull.Value : item.QSV);
			sqlCommand.Parameters.AddWithValue("QSV_AttachmentId", item.QSV_AttachmentId == null ? (object)DBNull.Value : item.QSV_AttachmentId);
			sqlCommand.Parameters.AddWithValue("REACH_SVHC_Confirmity_AttachmentId", item.REACH_SVHC_Confirmity_AttachmentId == null ? (object)DBNull.Value : item.REACH_SVHC_Confirmity_AttachmentId);
			sqlCommand.Parameters.AddWithValue("ROHS_EEE_Confirmity_AttachmentId", item.ROHS_EEE_Confirmity_AttachmentId == null ? (object)DBNull.Value : item.ROHS_EEE_Confirmity_AttachmentId);
			sqlCommand.Parameters.AddWithValue("SpecialCustomerReleases__DeviationReleases", item.SpecialCustomerReleases__DeviationReleases == null ? (object)DBNull.Value : item.SpecialCustomerReleases__DeviationReleases);
			sqlCommand.Parameters.AddWithValue("SpecialCustomerReleases__DeviationReleases_AttachmentId", item.SpecialCustomerReleases__DeviationReleases_AttachmentId == null ? (object)DBNull.Value : item.SpecialCustomerReleases__DeviationReleases_AttachmentId);
			sqlCommand.Parameters.AddWithValue("TSP_Available", item.TSP_Available == null ? (object)DBNull.Value : item.TSP_Available);
			sqlCommand.Parameters.AddWithValue("TSP_Available_AttachmentId", item.TSP_Available_AttachmentId == null ? (object)DBNull.Value : item.TSP_Available_AttachmentId);
			sqlCommand.Parameters.AddWithValue("UL_Etikett_AttachmentId", item.UL_Etikett_AttachmentId == null ? (object)DBNull.Value : item.UL_Etikett_AttachmentId);
			sqlCommand.Parameters.AddWithValue("UL_zertifiziert_AttachmentId", item.UL_zertifiziert_AttachmentId == null ? (object)DBNull.Value : item.UL_zertifiziert_AttachmentId);
			sqlCommand.Parameters.AddWithValue("UpdateTime", item.UpdateTime == null ? (object)DBNull.Value : item.UpdateTime);
			sqlCommand.Parameters.AddWithValue("UpdateUserId", item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 29; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				//int results = -1;
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " UPDATE [__BSD_ArtikelQualityExtension] SET "

					+ "[ArticleId]=@ArticleId" + i + ","
					+ "[CoC_Pflichtig_AttachmentId]=@CoC_Pflichtig_AttachmentId" + i + ","
					+ "[CreateTime]=@CreateTime" + i + ","
					+ "[CreateUserId]=@CreateUserId" + i + ","
					+ "[Dienstleistung_AttachmentId]=@Dienstleistung_AttachmentId" + i + ","
					+ "[EMPB_AttachmentId]=@EMPB_AttachmentId" + i + ","
					+ "[EMPB_Freigegeben_AttachmentId]=@EMPB_Freigegeben_AttachmentId" + i + ","
					+ "[ESD_AttachmentId]=@ESD_AttachmentId" + i + ","
					+ "[HM_AttachmentId]=@HM_AttachmentId" + i + ","
					+ "[LLE_AttachmentId]=@LLE_AttachmentId" + i + ","
					+ "[MHD_AttachmentId]=@MHD_AttachmentId" + i + ","
					+ "[MineralsConfirmity_AttachmentId]=@MineralsConfirmity_AttachmentId" + i + ","
					+ "[PackagingRegulation_Available]=@PackagingRegulation_Available" + i + ","
					+ "[PackagingRegulation_Available_AttachmentId]=@PackagingRegulation_Available_AttachmentId" + i + ","
					+ "[PurchasingArticleInspection__SpecialArticlesCustomerSpecific]=@PurchasingArticleInspection__SpecialArticlesCustomerSpecific" + i + ","
					+ "[PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId]=@PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId" + i + ","
					+ "[QSV]=@QSV" + i + ","
					+ "[QSV_AttachmentId]=@QSV_AttachmentId" + i + ","
					+ "[REACH_SVHC_Confirmity_AttachmentId]=@REACH_SVHC_Confirmity_AttachmentId" + i + ","
					+ "[ROHS_EEE_Confirmity_AttachmentId]=@ROHS_EEE_Confirmity_AttachmentId" + i + ","
					+ "[SpecialCustomerReleases__DeviationReleases]=@SpecialCustomerReleases__DeviationReleases" + i + ","
					+ "[SpecialCustomerReleases__DeviationReleases_AttachmentId]=@SpecialCustomerReleases__DeviationReleases_AttachmentId" + i + ","
					+ "[TSP_Available]=@TSP_Available" + i + ","
					+ "[TSP_Available_AttachmentId]=@TSP_Available_AttachmentId" + i + ","
					+ "[UL_Etikett_AttachmentId]=@UL_Etikett_AttachmentId" + i + ","
					+ "[UL_zertifiziert_AttachmentId]=@UL_zertifiziert_AttachmentId" + i + ","
					+ "[UpdateTime]=@UpdateTime" + i + ","
					+ "[UpdateUserId]=@UpdateUserId" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId);
					sqlCommand.Parameters.AddWithValue("CoC_Pflichtig_AttachmentId" + i, item.CoC_Pflichtig_AttachmentId == null ? (object)DBNull.Value : item.CoC_Pflichtig_AttachmentId);
					sqlCommand.Parameters.AddWithValue("CreateTime" + i, item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
					sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
					sqlCommand.Parameters.AddWithValue("Dienstleistung_AttachmentId" + i, item.Dienstleistung_AttachmentId == null ? (object)DBNull.Value : item.Dienstleistung_AttachmentId);
					sqlCommand.Parameters.AddWithValue("EMPB_AttachmentId" + i, item.EMPB_AttachmentId == null ? (object)DBNull.Value : item.EMPB_AttachmentId);
					sqlCommand.Parameters.AddWithValue("EMPB_Freigegeben_AttachmentId" + i, item.EMPB_Freigegeben_AttachmentId == null ? (object)DBNull.Value : item.EMPB_Freigegeben_AttachmentId);
					sqlCommand.Parameters.AddWithValue("ESD_AttachmentId" + i, item.ESD_AttachmentId == null ? (object)DBNull.Value : item.ESD_AttachmentId);
					sqlCommand.Parameters.AddWithValue("HM_AttachmentId" + i, item.HM_AttachmentId == null ? (object)DBNull.Value : item.HM_AttachmentId);
					sqlCommand.Parameters.AddWithValue("LLE_AttachmentId" + i, item.LLE_AttachmentId == null ? (object)DBNull.Value : item.LLE_AttachmentId);
					sqlCommand.Parameters.AddWithValue("MHD_AttachmentId" + i, item.MHD_AttachmentId == null ? (object)DBNull.Value : item.MHD_AttachmentId);
					sqlCommand.Parameters.AddWithValue("MineralsConfirmity_AttachmentId" + i, item.MineralsConfirmity_AttachmentId == null ? (object)DBNull.Value : item.MineralsConfirmity_AttachmentId);
					sqlCommand.Parameters.AddWithValue("PackagingRegulation_Available" + i, item.PackagingRegulation_Available == null ? (object)DBNull.Value : item.PackagingRegulation_Available);
					sqlCommand.Parameters.AddWithValue("PackagingRegulation_Available_AttachmentId" + i, item.PackagingRegulation_Available_AttachmentId == null ? (object)DBNull.Value : item.PackagingRegulation_Available_AttachmentId);
					sqlCommand.Parameters.AddWithValue("PurchasingArticleInspection__SpecialArticlesCustomerSpecific" + i, item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific == null ? (object)DBNull.Value : item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific);
					sqlCommand.Parameters.AddWithValue("PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId" + i, item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId == null ? (object)DBNull.Value : item.PurchasingArticleInspection__SpecialArticlesCustomerSpecific_AttachmentId);
					sqlCommand.Parameters.AddWithValue("QSV" + i, item.QSV == null ? (object)DBNull.Value : item.QSV);
					sqlCommand.Parameters.AddWithValue("QSV_AttachmentId" + i, item.QSV_AttachmentId == null ? (object)DBNull.Value : item.QSV_AttachmentId);
					sqlCommand.Parameters.AddWithValue("REACH_SVHC_Confirmity_AttachmentId" + i, item.REACH_SVHC_Confirmity_AttachmentId == null ? (object)DBNull.Value : item.REACH_SVHC_Confirmity_AttachmentId);
					sqlCommand.Parameters.AddWithValue("ROHS_EEE_Confirmity_AttachmentId" + i, item.ROHS_EEE_Confirmity_AttachmentId == null ? (object)DBNull.Value : item.ROHS_EEE_Confirmity_AttachmentId);
					sqlCommand.Parameters.AddWithValue("SpecialCustomerReleases__DeviationReleases" + i, item.SpecialCustomerReleases__DeviationReleases == null ? (object)DBNull.Value : item.SpecialCustomerReleases__DeviationReleases);
					sqlCommand.Parameters.AddWithValue("SpecialCustomerReleases__DeviationReleases_AttachmentId" + i, item.SpecialCustomerReleases__DeviationReleases_AttachmentId == null ? (object)DBNull.Value : item.SpecialCustomerReleases__DeviationReleases_AttachmentId);
					sqlCommand.Parameters.AddWithValue("TSP_Available" + i, item.TSP_Available == null ? (object)DBNull.Value : item.TSP_Available);
					sqlCommand.Parameters.AddWithValue("TSP_Available_AttachmentId" + i, item.TSP_Available_AttachmentId == null ? (object)DBNull.Value : item.TSP_Available_AttachmentId);
					sqlCommand.Parameters.AddWithValue("UL_Etikett_AttachmentId" + i, item.UL_Etikett_AttachmentId == null ? (object)DBNull.Value : item.UL_Etikett_AttachmentId);
					sqlCommand.Parameters.AddWithValue("UL_zertifiziert_AttachmentId" + i, item.UL_zertifiziert_AttachmentId == null ? (object)DBNull.Value : item.UL_zertifiziert_AttachmentId);
					sqlCommand.Parameters.AddWithValue("UpdateTime" + i, item.UpdateTime == null ? (object)DBNull.Value : item.UpdateTime);
					sqlCommand.Parameters.AddWithValue("UpdateUserId" + i, item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__BSD_ArtikelQualityExtension] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			results = sqlCommand.ExecuteNonQuery();


			return results;
		}
		public static int DeleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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
		private static int deleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				string query = "DELETE FROM [__BSD_ArtikelQualityExtension] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods


		public static Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity GetByArticleId(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_ArtikelQualityExtension] WHERE [ArticleId]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static int UpdateAttachmentId(int articleId, int newFileId, string columnName)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"UPDATE [__BSD_ArtikelQualityExtension] SET [{columnName}]=@fileId WHERE [ArticleId]=@articleId";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("articleId", articleId);
				sqlCommand.Parameters.AddWithValue("fileId", newFileId);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int DeleteByArticleWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__BSD_ArtikelQualityExtension] WHERE [ArticleId]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			results = sqlCommand.ExecuteNonQuery();


			return results;
		}

		#endregion
	}
}
