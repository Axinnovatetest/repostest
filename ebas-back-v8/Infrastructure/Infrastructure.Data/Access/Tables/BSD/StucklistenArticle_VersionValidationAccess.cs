using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class StucklistenArticle_VersionValidationAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity Get(long id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_StucklistenArticle_VersionValidation] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_StucklistenArticle_VersionValidation]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__BSD_StucklistenArticle_VersionValidation] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__BSD_StucklistenArticle_VersionValidation] ([ArticleBOMVersion],[ArticleCPVersion],[ArticleKundenIndex],[ArticleKundenIndexDatum],[ArticleNr],[ArticleNumber],[Assembling1],[Assembling1Notes],[Assembling2],[Assembling2Notes],[Assembling3],[Assembling3Notes],[BOMValidationDate],[Commission],[CommissionNotes],[ControleElectrical],[ControleElectricalNotes],[ControleVisual],[ControleVisualNotes],[CPValidationDate],[CrimpAfter],[CrimpAfterManual],[CrimpAfterManualNotes],[CrimpAfterNotes],[CrimpBefore],[CrimpBeforeManual],[CrimpBeforeManualNotes],[CrimpBeforeNotes],[EngineeringControl],[EngineeringControlEditDate],[EngineeringControlEditUserEmail],[EngineeringControlEditUserId],[EngineeringControlEditUserName],[EngineeringDistribution],[EngineeringDistributionEditDate],[EngineeringDistributionEditUserEmail],[EngineeringDistributionEditUserId],[EngineeringDistributionEditUserName],[EngineeringPrint],[EngineeringPrintEditDate],[EngineeringPrintEditUserEmail],[EngineeringPrintEditUserId],[EngineeringPrintEditUserName],[EngineeringUpdate],[EngineeringUpdateEditDate],[EngineeringUpdateEditUserEmail],[EngineeringUpdateEditUserId],[EngineeringUpdateEditUserName],[EngineeringValidationFull],[Finition],[FinitionNotes],[InjectionOnCables],[InjectionOnCablesNotes],[InjectionPlastic],[InjectionPlasticNotes],[IsPartialDocumentation],[LabelingPlan],[LabelingPlanNotes],[Packaging],[PackagingNotes],[PendingCpValidation],[Pressing],[PressingNotes],[QualityControl],[QualityControlEditDate],[QualityControlEditUserEmail],[QualityControlEditUserId],[QualityControlEditUserName],[QualityDistribution],[QualityDistributionEditDate],[QualityDistributionEditUserEmail],[QualityDistributionEditUserId],[QualityDistributionEditUserName],[QualityPrint],[QualityPrintEditDate],[QualityPrintEditUserEmail],[QualityPrintEditUserId],[QualityPrintEditUserName],[QualityUpdate],[QualityUpdateEditDate],[QualityUpdateEditUserEmail],[QualityUpdateEditUserId],[QualityUpdateEditUserName],[QualityValidationFull],[Readiness],[ReadinessNotes],[Translation],[TranslationNotes],[Twisting],[TwistingNotes],[Ultrason],[UltrasonNotes],[Validation],[ValidationFull],[ValidationNotes],[Welding],[WeldingNotes]) OUTPUT INSERTED.[Id] VALUES (@ArticleBOMVersion,@ArticleCPVersion,@ArticleKundenIndex,@ArticleKundenIndexDatum,@ArticleNr,@ArticleNumber,@Assembling1,@Assembling1Notes,@Assembling2,@Assembling2Notes,@Assembling3,@Assembling3Notes,@BOMValidationDate,@Commission,@CommissionNotes,@ControleElectrical,@ControleElectricalNotes,@ControleVisual,@ControleVisualNotes,@CPValidationDate,@CrimpAfter,@CrimpAfterManual,@CrimpAfterManualNotes,@CrimpAfterNotes,@CrimpBefore,@CrimpBeforeManual,@CrimpBeforeManualNotes,@CrimpBeforeNotes,@EngineeringControl,@EngineeringControlEditDate,@EngineeringControlEditUserEmail,@EngineeringControlEditUserId,@EngineeringControlEditUserName,@EngineeringDistribution,@EngineeringDistributionEditDate,@EngineeringDistributionEditUserEmail,@EngineeringDistributionEditUserId,@EngineeringDistributionEditUserName,@EngineeringPrint,@EngineeringPrintEditDate,@EngineeringPrintEditUserEmail,@EngineeringPrintEditUserId,@EngineeringPrintEditUserName,@EngineeringUpdate,@EngineeringUpdateEditDate,@EngineeringUpdateEditUserEmail,@EngineeringUpdateEditUserId,@EngineeringUpdateEditUserName,@EngineeringValidationFull,@Finition,@FinitionNotes,@InjectionOnCables,@InjectionOnCablesNotes,@InjectionPlastic,@InjectionPlasticNotes,@IsPartialDocumentation,@LabelingPlan,@LabelingPlanNotes,@Packaging,@PackagingNotes,@PendingCpValidation,@Pressing,@PressingNotes,@QualityControl,@QualityControlEditDate,@QualityControlEditUserEmail,@QualityControlEditUserId,@QualityControlEditUserName,@QualityDistribution,@QualityDistributionEditDate,@QualityDistributionEditUserEmail,@QualityDistributionEditUserId,@QualityDistributionEditUserName,@QualityPrint,@QualityPrintEditDate,@QualityPrintEditUserEmail,@QualityPrintEditUserId,@QualityPrintEditUserName,@QualityUpdate,@QualityUpdateEditDate,@QualityUpdateEditUserEmail,@QualityUpdateEditUserId,@QualityUpdateEditUserName,@QualityValidationFull,@Readiness,@ReadinessNotes,@Translation,@TranslationNotes,@Twisting,@TwistingNotes,@Ultrason,@UltrasonNotes,@Validation,@ValidationFull,@ValidationNotes,@Welding,@WeldingNotes); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArticleBOMVersion", item.ArticleBOMVersion);
					sqlCommand.Parameters.AddWithValue("ArticleCPVersion", item.ArticleCPVersion == null ? (object)DBNull.Value : item.ArticleCPVersion);
					sqlCommand.Parameters.AddWithValue("ArticleKundenIndex", item.ArticleKundenIndex == null ? (object)DBNull.Value : item.ArticleKundenIndex);
					sqlCommand.Parameters.AddWithValue("ArticleKundenIndexDatum", item.ArticleKundenIndexDatum == null ? (object)DBNull.Value : item.ArticleKundenIndexDatum);
					sqlCommand.Parameters.AddWithValue("ArticleNr", item.ArticleNr);
					sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("Assembling1", item.Assembling1 == null ? (object)DBNull.Value : item.Assembling1);
					sqlCommand.Parameters.AddWithValue("Assembling1Notes", item.Assembling1Notes == null ? (object)DBNull.Value : item.Assembling1Notes);
					sqlCommand.Parameters.AddWithValue("Assembling2", item.Assembling2 == null ? (object)DBNull.Value : item.Assembling2);
					sqlCommand.Parameters.AddWithValue("Assembling2Notes", item.Assembling2Notes == null ? (object)DBNull.Value : item.Assembling2Notes);
					sqlCommand.Parameters.AddWithValue("Assembling3", item.Assembling3 == null ? (object)DBNull.Value : item.Assembling3);
					sqlCommand.Parameters.AddWithValue("Assembling3Notes", item.Assembling3Notes == null ? (object)DBNull.Value : item.Assembling3Notes);
					sqlCommand.Parameters.AddWithValue("BOMValidationDate", item.BOMValidationDate);
					sqlCommand.Parameters.AddWithValue("Commission", item.Commission == null ? (object)DBNull.Value : item.Commission);
					sqlCommand.Parameters.AddWithValue("CommissionNotes", item.CommissionNotes == null ? (object)DBNull.Value : item.CommissionNotes);
					sqlCommand.Parameters.AddWithValue("ControleElectrical", item.ControleElectrical == null ? (object)DBNull.Value : item.ControleElectrical);
					sqlCommand.Parameters.AddWithValue("ControleElectricalNotes", item.ControleElectricalNotes == null ? (object)DBNull.Value : item.ControleElectricalNotes);
					sqlCommand.Parameters.AddWithValue("ControleVisual", item.ControleVisual == null ? (object)DBNull.Value : item.ControleVisual);
					sqlCommand.Parameters.AddWithValue("ControleVisualNotes", item.ControleVisualNotes == null ? (object)DBNull.Value : item.ControleVisualNotes);
					sqlCommand.Parameters.AddWithValue("CPValidationDate", item.CPValidationDate == null ? (object)DBNull.Value : item.CPValidationDate);
					sqlCommand.Parameters.AddWithValue("CrimpAfter", item.CrimpAfter == null ? (object)DBNull.Value : item.CrimpAfter);
					sqlCommand.Parameters.AddWithValue("CrimpAfterManual", item.CrimpAfterManual == null ? (object)DBNull.Value : item.CrimpAfterManual);
					sqlCommand.Parameters.AddWithValue("CrimpAfterManualNotes", item.CrimpAfterManualNotes == null ? (object)DBNull.Value : item.CrimpAfterManualNotes);
					sqlCommand.Parameters.AddWithValue("CrimpAfterNotes", item.CrimpAfterNotes == null ? (object)DBNull.Value : item.CrimpAfterNotes);
					sqlCommand.Parameters.AddWithValue("CrimpBefore", item.CrimpBefore == null ? (object)DBNull.Value : item.CrimpBefore);
					sqlCommand.Parameters.AddWithValue("CrimpBeforeManual", item.CrimpBeforeManual == null ? (object)DBNull.Value : item.CrimpBeforeManual);
					sqlCommand.Parameters.AddWithValue("CrimpBeforeManualNotes", item.CrimpBeforeManualNotes == null ? (object)DBNull.Value : item.CrimpBeforeManualNotes);
					sqlCommand.Parameters.AddWithValue("CrimpBeforeNotes", item.CrimpBeforeNotes == null ? (object)DBNull.Value : item.CrimpBeforeNotes);
					sqlCommand.Parameters.AddWithValue("EngineeringControl", item.EngineeringControl);
					sqlCommand.Parameters.AddWithValue("EngineeringControlEditDate", item.EngineeringControlEditDate == null ? (object)DBNull.Value : item.EngineeringControlEditDate);
					sqlCommand.Parameters.AddWithValue("EngineeringControlEditUserEmail", item.EngineeringControlEditUserEmail == null ? (object)DBNull.Value : item.EngineeringControlEditUserEmail);
					sqlCommand.Parameters.AddWithValue("EngineeringControlEditUserId", item.EngineeringControlEditUserId == null ? (object)DBNull.Value : item.EngineeringControlEditUserId);
					sqlCommand.Parameters.AddWithValue("EngineeringControlEditUserName", item.EngineeringControlEditUserName == null ? (object)DBNull.Value : item.EngineeringControlEditUserName);
					sqlCommand.Parameters.AddWithValue("EngineeringDistribution", item.EngineeringDistribution);
					sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditDate", item.EngineeringDistributionEditDate == null ? (object)DBNull.Value : item.EngineeringDistributionEditDate);
					sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditUserEmail", item.EngineeringDistributionEditUserEmail == null ? (object)DBNull.Value : item.EngineeringDistributionEditUserEmail);
					sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditUserId", item.EngineeringDistributionEditUserId == null ? (object)DBNull.Value : item.EngineeringDistributionEditUserId);
					sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditUserName", item.EngineeringDistributionEditUserName == null ? (object)DBNull.Value : item.EngineeringDistributionEditUserName);
					sqlCommand.Parameters.AddWithValue("EngineeringPrint", item.EngineeringPrint);
					sqlCommand.Parameters.AddWithValue("EngineeringPrintEditDate", item.EngineeringPrintEditDate == null ? (object)DBNull.Value : item.EngineeringPrintEditDate);
					sqlCommand.Parameters.AddWithValue("EngineeringPrintEditUserEmail", item.EngineeringPrintEditUserEmail == null ? (object)DBNull.Value : item.EngineeringPrintEditUserEmail);
					sqlCommand.Parameters.AddWithValue("EngineeringPrintEditUserId", item.EngineeringPrintEditUserId == null ? (object)DBNull.Value : item.EngineeringPrintEditUserId);
					sqlCommand.Parameters.AddWithValue("EngineeringPrintEditUserName", item.EngineeringPrintEditUserName == null ? (object)DBNull.Value : item.EngineeringPrintEditUserName);
					sqlCommand.Parameters.AddWithValue("EngineeringUpdate", item.EngineeringUpdate);
					sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditDate", item.EngineeringUpdateEditDate == null ? (object)DBNull.Value : item.EngineeringUpdateEditDate);
					sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditUserEmail", item.EngineeringUpdateEditUserEmail == null ? (object)DBNull.Value : item.EngineeringUpdateEditUserEmail);
					sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditUserId", item.EngineeringUpdateEditUserId == null ? (object)DBNull.Value : item.EngineeringUpdateEditUserId);
					sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditUserName", item.EngineeringUpdateEditUserName == null ? (object)DBNull.Value : item.EngineeringUpdateEditUserName);
					sqlCommand.Parameters.AddWithValue("EngineeringValidationFull", item.EngineeringValidationFull);
					sqlCommand.Parameters.AddWithValue("Finition", item.Finition == null ? (object)DBNull.Value : item.Finition);
					sqlCommand.Parameters.AddWithValue("FinitionNotes", item.FinitionNotes == null ? (object)DBNull.Value : item.FinitionNotes);
					sqlCommand.Parameters.AddWithValue("InjectionOnCables", item.InjectionOnCables == null ? (object)DBNull.Value : item.InjectionOnCables);
					sqlCommand.Parameters.AddWithValue("InjectionOnCablesNotes", item.InjectionOnCablesNotes == null ? (object)DBNull.Value : item.InjectionOnCablesNotes);
					sqlCommand.Parameters.AddWithValue("InjectionPlastic", item.InjectionPlastic == null ? (object)DBNull.Value : item.InjectionPlastic);
					sqlCommand.Parameters.AddWithValue("InjectionPlasticNotes", item.InjectionPlasticNotes == null ? (object)DBNull.Value : item.InjectionPlasticNotes);
					sqlCommand.Parameters.AddWithValue("IsPartialDocumentation", item.IsPartialDocumentation == null ? (object)DBNull.Value : item.IsPartialDocumentation);
					sqlCommand.Parameters.AddWithValue("LabelingPlan", item.LabelingPlan == null ? (object)DBNull.Value : item.LabelingPlan);
					sqlCommand.Parameters.AddWithValue("LabelingPlanNotes", item.LabelingPlanNotes == null ? (object)DBNull.Value : item.LabelingPlanNotes);
					sqlCommand.Parameters.AddWithValue("Packaging", item.Packaging == null ? (object)DBNull.Value : item.Packaging);
					sqlCommand.Parameters.AddWithValue("PackagingNotes", item.PackagingNotes == null ? (object)DBNull.Value : item.PackagingNotes);
					sqlCommand.Parameters.AddWithValue("PendingCpValidation", item.PendingCpValidation);
					sqlCommand.Parameters.AddWithValue("Pressing", item.Pressing == null ? (object)DBNull.Value : item.Pressing);
					sqlCommand.Parameters.AddWithValue("PressingNotes", item.PressingNotes == null ? (object)DBNull.Value : item.PressingNotes);
					sqlCommand.Parameters.AddWithValue("QualityControl", item.QualityControl);
					sqlCommand.Parameters.AddWithValue("QualityControlEditDate", item.QualityControlEditDate == null ? (object)DBNull.Value : item.QualityControlEditDate);
					sqlCommand.Parameters.AddWithValue("QualityControlEditUserEmail", item.QualityControlEditUserEmail == null ? (object)DBNull.Value : item.QualityControlEditUserEmail);
					sqlCommand.Parameters.AddWithValue("QualityControlEditUserId", item.QualityControlEditUserId == null ? (object)DBNull.Value : item.QualityControlEditUserId);
					sqlCommand.Parameters.AddWithValue("QualityControlEditUserName", item.QualityControlEditUserName == null ? (object)DBNull.Value : item.QualityControlEditUserName);
					sqlCommand.Parameters.AddWithValue("QualityDistribution", item.QualityDistribution);
					sqlCommand.Parameters.AddWithValue("QualityDistributionEditDate", item.QualityDistributionEditDate == null ? (object)DBNull.Value : item.QualityDistributionEditDate);
					sqlCommand.Parameters.AddWithValue("QualityDistributionEditUserEmail", item.QualityDistributionEditUserEmail == null ? (object)DBNull.Value : item.QualityDistributionEditUserEmail);
					sqlCommand.Parameters.AddWithValue("QualityDistributionEditUserId", item.QualityDistributionEditUserId == null ? (object)DBNull.Value : item.QualityDistributionEditUserId);
					sqlCommand.Parameters.AddWithValue("QualityDistributionEditUserName", item.QualityDistributionEditUserName == null ? (object)DBNull.Value : item.QualityDistributionEditUserName);
					sqlCommand.Parameters.AddWithValue("QualityPrint", item.QualityPrint);
					sqlCommand.Parameters.AddWithValue("QualityPrintEditDate", item.QualityPrintEditDate == null ? (object)DBNull.Value : item.QualityPrintEditDate);
					sqlCommand.Parameters.AddWithValue("QualityPrintEditUserEmail", item.QualityPrintEditUserEmail == null ? (object)DBNull.Value : item.QualityPrintEditUserEmail);
					sqlCommand.Parameters.AddWithValue("QualityPrintEditUserId", item.QualityPrintEditUserId == null ? (object)DBNull.Value : item.QualityPrintEditUserId);
					sqlCommand.Parameters.AddWithValue("QualityPrintEditUserName", item.QualityPrintEditUserName == null ? (object)DBNull.Value : item.QualityPrintEditUserName);
					sqlCommand.Parameters.AddWithValue("QualityUpdate", item.QualityUpdate);
					sqlCommand.Parameters.AddWithValue("QualityUpdateEditDate", item.QualityUpdateEditDate == null ? (object)DBNull.Value : item.QualityUpdateEditDate);
					sqlCommand.Parameters.AddWithValue("QualityUpdateEditUserEmail", item.QualityUpdateEditUserEmail == null ? (object)DBNull.Value : item.QualityUpdateEditUserEmail);
					sqlCommand.Parameters.AddWithValue("QualityUpdateEditUserId", item.QualityUpdateEditUserId == null ? (object)DBNull.Value : item.QualityUpdateEditUserId);
					sqlCommand.Parameters.AddWithValue("QualityUpdateEditUserName", item.QualityUpdateEditUserName == null ? (object)DBNull.Value : item.QualityUpdateEditUserName);
					sqlCommand.Parameters.AddWithValue("QualityValidationFull", item.QualityValidationFull);
					sqlCommand.Parameters.AddWithValue("Readiness", item.Readiness == null ? (object)DBNull.Value : item.Readiness);
					sqlCommand.Parameters.AddWithValue("ReadinessNotes", item.ReadinessNotes == null ? (object)DBNull.Value : item.ReadinessNotes);
					sqlCommand.Parameters.AddWithValue("Translation", item.Translation == null ? (object)DBNull.Value : item.Translation);
					sqlCommand.Parameters.AddWithValue("TranslationNotes", item.TranslationNotes == null ? (object)DBNull.Value : item.TranslationNotes);
					sqlCommand.Parameters.AddWithValue("Twisting", item.Twisting == null ? (object)DBNull.Value : item.Twisting);
					sqlCommand.Parameters.AddWithValue("TwistingNotes", item.TwistingNotes == null ? (object)DBNull.Value : item.TwistingNotes);
					sqlCommand.Parameters.AddWithValue("Ultrason", item.Ultrason == null ? (object)DBNull.Value : item.Ultrason);
					sqlCommand.Parameters.AddWithValue("UltrasonNotes", item.UltrasonNotes == null ? (object)DBNull.Value : item.UltrasonNotes);
					sqlCommand.Parameters.AddWithValue("Validation", item.Validation == null ? (object)DBNull.Value : item.Validation);
					sqlCommand.Parameters.AddWithValue("ValidationFull", item.ValidationFull);
					sqlCommand.Parameters.AddWithValue("ValidationNotes", item.ValidationNotes == null ? (object)DBNull.Value : item.ValidationNotes);
					sqlCommand.Parameters.AddWithValue("Welding", item.Welding == null ? (object)DBNull.Value : item.Welding);
					sqlCommand.Parameters.AddWithValue("WeldingNotes", item.WeldingNotes == null ? (object)DBNull.Value : item.WeldingNotes);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 99; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity> items)
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
						query += " INSERT INTO [__BSD_StucklistenArticle_VersionValidation] ([ArticleBOMVersion],[ArticleCPVersion],[ArticleKundenIndex],[ArticleKundenIndexDatum],[ArticleNr],[ArticleNumber],[Assembling1],[Assembling1Notes],[Assembling2],[Assembling2Notes],[Assembling3],[Assembling3Notes],[BOMValidationDate],[Commission],[CommissionNotes],[ControleElectrical],[ControleElectricalNotes],[ControleVisual],[ControleVisualNotes],[CPValidationDate],[CrimpAfter],[CrimpAfterManual],[CrimpAfterManualNotes],[CrimpAfterNotes],[CrimpBefore],[CrimpBeforeManual],[CrimpBeforeManualNotes],[CrimpBeforeNotes],[EngineeringControl],[EngineeringControlEditDate],[EngineeringControlEditUserEmail],[EngineeringControlEditUserId],[EngineeringControlEditUserName],[EngineeringDistribution],[EngineeringDistributionEditDate],[EngineeringDistributionEditUserEmail],[EngineeringDistributionEditUserId],[EngineeringDistributionEditUserName],[EngineeringPrint],[EngineeringPrintEditDate],[EngineeringPrintEditUserEmail],[EngineeringPrintEditUserId],[EngineeringPrintEditUserName],[EngineeringUpdate],[EngineeringUpdateEditDate],[EngineeringUpdateEditUserEmail],[EngineeringUpdateEditUserId],[EngineeringUpdateEditUserName],[EngineeringValidationFull],[Finition],[FinitionNotes],[InjectionOnCables],[InjectionOnCablesNotes],[InjectionPlastic],[InjectionPlasticNotes],[IsPartialDocumentation],[LabelingPlan],[LabelingPlanNotes],[Packaging],[PackagingNotes],[PendingCpValidation],[Pressing],[PressingNotes],[QualityControl],[QualityControlEditDate],[QualityControlEditUserEmail],[QualityControlEditUserId],[QualityControlEditUserName],[QualityDistribution],[QualityDistributionEditDate],[QualityDistributionEditUserEmail],[QualityDistributionEditUserId],[QualityDistributionEditUserName],[QualityPrint],[QualityPrintEditDate],[QualityPrintEditUserEmail],[QualityPrintEditUserId],[QualityPrintEditUserName],[QualityUpdate],[QualityUpdateEditDate],[QualityUpdateEditUserEmail],[QualityUpdateEditUserId],[QualityUpdateEditUserName],[QualityValidationFull],[Readiness],[ReadinessNotes],[Translation],[TranslationNotes],[Twisting],[TwistingNotes],[Ultrason],[UltrasonNotes],[Validation],[ValidationFull],[ValidationNotes],[Welding],[WeldingNotes]) VALUES ( "

							+ "@ArticleBOMVersion" + i + ","
							+ "@ArticleCPVersion" + i + ","
							+ "@ArticleKundenIndex" + i + ","
							+ "@ArticleKundenIndexDatum" + i + ","
							+ "@ArticleNr" + i + ","
							+ "@ArticleNumber" + i + ","
							+ "@Assembling1" + i + ","
							+ "@Assembling1Notes" + i + ","
							+ "@Assembling2" + i + ","
							+ "@Assembling2Notes" + i + ","
							+ "@Assembling3" + i + ","
							+ "@Assembling3Notes" + i + ","
							+ "@BOMValidationDate" + i + ","
							+ "@Commission" + i + ","
							+ "@CommissionNotes" + i + ","
							+ "@ControleElectrical" + i + ","
							+ "@ControleElectricalNotes" + i + ","
							+ "@ControleVisual" + i + ","
							+ "@ControleVisualNotes" + i + ","
							+ "@CPValidationDate" + i + ","
							+ "@CrimpAfter" + i + ","
							+ "@CrimpAfterManual" + i + ","
							+ "@CrimpAfterManualNotes" + i + ","
							+ "@CrimpAfterNotes" + i + ","
							+ "@CrimpBefore" + i + ","
							+ "@CrimpBeforeManual" + i + ","
							+ "@CrimpBeforeManualNotes" + i + ","
							+ "@CrimpBeforeNotes" + i + ","
							+ "@EngineeringControl" + i + ","
							+ "@EngineeringControlEditDate" + i + ","
							+ "@EngineeringControlEditUserEmail" + i + ","
							+ "@EngineeringControlEditUserId" + i + ","
							+ "@EngineeringControlEditUserName" + i + ","
							+ "@EngineeringDistribution" + i + ","
							+ "@EngineeringDistributionEditDate" + i + ","
							+ "@EngineeringDistributionEditUserEmail" + i + ","
							+ "@EngineeringDistributionEditUserId" + i + ","
							+ "@EngineeringDistributionEditUserName" + i + ","
							+ "@EngineeringPrint" + i + ","
							+ "@EngineeringPrintEditDate" + i + ","
							+ "@EngineeringPrintEditUserEmail" + i + ","
							+ "@EngineeringPrintEditUserId" + i + ","
							+ "@EngineeringPrintEditUserName" + i + ","
							+ "@EngineeringUpdate" + i + ","
							+ "@EngineeringUpdateEditDate" + i + ","
							+ "@EngineeringUpdateEditUserEmail" + i + ","
							+ "@EngineeringUpdateEditUserId" + i + ","
							+ "@EngineeringUpdateEditUserName" + i + ","
							+ "@EngineeringValidationFull" + i + ","
							+ "@Finition" + i + ","
							+ "@FinitionNotes" + i + ","
							+ "@InjectionOnCables" + i + ","
							+ "@InjectionOnCablesNotes" + i + ","
							+ "@InjectionPlastic" + i + ","
							+ "@InjectionPlasticNotes" + i + ","
							+ "@IsPartialDocumentation" + i + ","
							+ "@LabelingPlan" + i + ","
							+ "@LabelingPlanNotes" + i + ","
							+ "@Packaging" + i + ","
							+ "@PackagingNotes" + i + ","
							+ "@PendingCpValidation" + i + ","
							+ "@Pressing" + i + ","
							+ "@PressingNotes" + i + ","
							+ "@QualityControl" + i + ","
							+ "@QualityControlEditDate" + i + ","
							+ "@QualityControlEditUserEmail" + i + ","
							+ "@QualityControlEditUserId" + i + ","
							+ "@QualityControlEditUserName" + i + ","
							+ "@QualityDistribution" + i + ","
							+ "@QualityDistributionEditDate" + i + ","
							+ "@QualityDistributionEditUserEmail" + i + ","
							+ "@QualityDistributionEditUserId" + i + ","
							+ "@QualityDistributionEditUserName" + i + ","
							+ "@QualityPrint" + i + ","
							+ "@QualityPrintEditDate" + i + ","
							+ "@QualityPrintEditUserEmail" + i + ","
							+ "@QualityPrintEditUserId" + i + ","
							+ "@QualityPrintEditUserName" + i + ","
							+ "@QualityUpdate" + i + ","
							+ "@QualityUpdateEditDate" + i + ","
							+ "@QualityUpdateEditUserEmail" + i + ","
							+ "@QualityUpdateEditUserId" + i + ","
							+ "@QualityUpdateEditUserName" + i + ","
							+ "@QualityValidationFull" + i + ","
							+ "@Readiness" + i + ","
							+ "@ReadinessNotes" + i + ","
							+ "@Translation" + i + ","
							+ "@TranslationNotes" + i + ","
							+ "@Twisting" + i + ","
							+ "@TwistingNotes" + i + ","
							+ "@Ultrason" + i + ","
							+ "@UltrasonNotes" + i + ","
							+ "@Validation" + i + ","
							+ "@ValidationFull" + i + ","
							+ "@ValidationNotes" + i + ","
							+ "@Welding" + i + ","
							+ "@WeldingNotes" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArticleBOMVersion" + i, item.ArticleBOMVersion);
						sqlCommand.Parameters.AddWithValue("ArticleCPVersion" + i, item.ArticleCPVersion == null ? (object)DBNull.Value : item.ArticleCPVersion);
						sqlCommand.Parameters.AddWithValue("ArticleKundenIndex" + i, item.ArticleKundenIndex == null ? (object)DBNull.Value : item.ArticleKundenIndex);
						sqlCommand.Parameters.AddWithValue("ArticleKundenIndexDatum" + i, item.ArticleKundenIndexDatum == null ? (object)DBNull.Value : item.ArticleKundenIndexDatum);
						sqlCommand.Parameters.AddWithValue("ArticleNr" + i, item.ArticleNr);
						sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber);
						sqlCommand.Parameters.AddWithValue("Assembling1" + i, item.Assembling1 == null ? (object)DBNull.Value : item.Assembling1);
						sqlCommand.Parameters.AddWithValue("Assembling1Notes" + i, item.Assembling1Notes == null ? (object)DBNull.Value : item.Assembling1Notes);
						sqlCommand.Parameters.AddWithValue("Assembling2" + i, item.Assembling2 == null ? (object)DBNull.Value : item.Assembling2);
						sqlCommand.Parameters.AddWithValue("Assembling2Notes" + i, item.Assembling2Notes == null ? (object)DBNull.Value : item.Assembling2Notes);
						sqlCommand.Parameters.AddWithValue("Assembling3" + i, item.Assembling3 == null ? (object)DBNull.Value : item.Assembling3);
						sqlCommand.Parameters.AddWithValue("Assembling3Notes" + i, item.Assembling3Notes == null ? (object)DBNull.Value : item.Assembling3Notes);
						sqlCommand.Parameters.AddWithValue("BOMValidationDate" + i, item.BOMValidationDate);
						sqlCommand.Parameters.AddWithValue("Commission" + i, item.Commission == null ? (object)DBNull.Value : item.Commission);
						sqlCommand.Parameters.AddWithValue("CommissionNotes" + i, item.CommissionNotes == null ? (object)DBNull.Value : item.CommissionNotes);
						sqlCommand.Parameters.AddWithValue("ControleElectrical" + i, item.ControleElectrical == null ? (object)DBNull.Value : item.ControleElectrical);
						sqlCommand.Parameters.AddWithValue("ControleElectricalNotes" + i, item.ControleElectricalNotes == null ? (object)DBNull.Value : item.ControleElectricalNotes);
						sqlCommand.Parameters.AddWithValue("ControleVisual" + i, item.ControleVisual == null ? (object)DBNull.Value : item.ControleVisual);
						sqlCommand.Parameters.AddWithValue("ControleVisualNotes" + i, item.ControleVisualNotes == null ? (object)DBNull.Value : item.ControleVisualNotes);
						sqlCommand.Parameters.AddWithValue("CPValidationDate" + i, item.CPValidationDate == null ? (object)DBNull.Value : item.CPValidationDate);
						sqlCommand.Parameters.AddWithValue("CrimpAfter" + i, item.CrimpAfter == null ? (object)DBNull.Value : item.CrimpAfter);
						sqlCommand.Parameters.AddWithValue("CrimpAfterManual" + i, item.CrimpAfterManual == null ? (object)DBNull.Value : item.CrimpAfterManual);
						sqlCommand.Parameters.AddWithValue("CrimpAfterManualNotes" + i, item.CrimpAfterManualNotes == null ? (object)DBNull.Value : item.CrimpAfterManualNotes);
						sqlCommand.Parameters.AddWithValue("CrimpAfterNotes" + i, item.CrimpAfterNotes == null ? (object)DBNull.Value : item.CrimpAfterNotes);
						sqlCommand.Parameters.AddWithValue("CrimpBefore" + i, item.CrimpBefore == null ? (object)DBNull.Value : item.CrimpBefore);
						sqlCommand.Parameters.AddWithValue("CrimpBeforeManual" + i, item.CrimpBeforeManual == null ? (object)DBNull.Value : item.CrimpBeforeManual);
						sqlCommand.Parameters.AddWithValue("CrimpBeforeManualNotes" + i, item.CrimpBeforeManualNotes == null ? (object)DBNull.Value : item.CrimpBeforeManualNotes);
						sqlCommand.Parameters.AddWithValue("CrimpBeforeNotes" + i, item.CrimpBeforeNotes == null ? (object)DBNull.Value : item.CrimpBeforeNotes);
						sqlCommand.Parameters.AddWithValue("EngineeringControl" + i, item.EngineeringControl);
						sqlCommand.Parameters.AddWithValue("EngineeringControlEditDate" + i, item.EngineeringControlEditDate == null ? (object)DBNull.Value : item.EngineeringControlEditDate);
						sqlCommand.Parameters.AddWithValue("EngineeringControlEditUserEmail" + i, item.EngineeringControlEditUserEmail == null ? (object)DBNull.Value : item.EngineeringControlEditUserEmail);
						sqlCommand.Parameters.AddWithValue("EngineeringControlEditUserId" + i, item.EngineeringControlEditUserId == null ? (object)DBNull.Value : item.EngineeringControlEditUserId);
						sqlCommand.Parameters.AddWithValue("EngineeringControlEditUserName" + i, item.EngineeringControlEditUserName == null ? (object)DBNull.Value : item.EngineeringControlEditUserName);
						sqlCommand.Parameters.AddWithValue("EngineeringDistribution" + i, item.EngineeringDistribution);
						sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditDate" + i, item.EngineeringDistributionEditDate == null ? (object)DBNull.Value : item.EngineeringDistributionEditDate);
						sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditUserEmail" + i, item.EngineeringDistributionEditUserEmail == null ? (object)DBNull.Value : item.EngineeringDistributionEditUserEmail);
						sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditUserId" + i, item.EngineeringDistributionEditUserId == null ? (object)DBNull.Value : item.EngineeringDistributionEditUserId);
						sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditUserName" + i, item.EngineeringDistributionEditUserName == null ? (object)DBNull.Value : item.EngineeringDistributionEditUserName);
						sqlCommand.Parameters.AddWithValue("EngineeringPrint" + i, item.EngineeringPrint);
						sqlCommand.Parameters.AddWithValue("EngineeringPrintEditDate" + i, item.EngineeringPrintEditDate == null ? (object)DBNull.Value : item.EngineeringPrintEditDate);
						sqlCommand.Parameters.AddWithValue("EngineeringPrintEditUserEmail" + i, item.EngineeringPrintEditUserEmail == null ? (object)DBNull.Value : item.EngineeringPrintEditUserEmail);
						sqlCommand.Parameters.AddWithValue("EngineeringPrintEditUserId" + i, item.EngineeringPrintEditUserId == null ? (object)DBNull.Value : item.EngineeringPrintEditUserId);
						sqlCommand.Parameters.AddWithValue("EngineeringPrintEditUserName" + i, item.EngineeringPrintEditUserName == null ? (object)DBNull.Value : item.EngineeringPrintEditUserName);
						sqlCommand.Parameters.AddWithValue("EngineeringUpdate" + i, item.EngineeringUpdate);
						sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditDate" + i, item.EngineeringUpdateEditDate == null ? (object)DBNull.Value : item.EngineeringUpdateEditDate);
						sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditUserEmail" + i, item.EngineeringUpdateEditUserEmail == null ? (object)DBNull.Value : item.EngineeringUpdateEditUserEmail);
						sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditUserId" + i, item.EngineeringUpdateEditUserId == null ? (object)DBNull.Value : item.EngineeringUpdateEditUserId);
						sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditUserName" + i, item.EngineeringUpdateEditUserName == null ? (object)DBNull.Value : item.EngineeringUpdateEditUserName);
						sqlCommand.Parameters.AddWithValue("EngineeringValidationFull" + i, item.EngineeringValidationFull);
						sqlCommand.Parameters.AddWithValue("Finition" + i, item.Finition == null ? (object)DBNull.Value : item.Finition);
						sqlCommand.Parameters.AddWithValue("FinitionNotes" + i, item.FinitionNotes == null ? (object)DBNull.Value : item.FinitionNotes);
						sqlCommand.Parameters.AddWithValue("InjectionOnCables" + i, item.InjectionOnCables == null ? (object)DBNull.Value : item.InjectionOnCables);
						sqlCommand.Parameters.AddWithValue("InjectionOnCablesNotes" + i, item.InjectionOnCablesNotes == null ? (object)DBNull.Value : item.InjectionOnCablesNotes);
						sqlCommand.Parameters.AddWithValue("InjectionPlastic" + i, item.InjectionPlastic == null ? (object)DBNull.Value : item.InjectionPlastic);
						sqlCommand.Parameters.AddWithValue("InjectionPlasticNotes" + i, item.InjectionPlasticNotes == null ? (object)DBNull.Value : item.InjectionPlasticNotes);
						sqlCommand.Parameters.AddWithValue("IsPartialDocumentation" + i, item.IsPartialDocumentation == null ? (object)DBNull.Value : item.IsPartialDocumentation);
						sqlCommand.Parameters.AddWithValue("LabelingPlan" + i, item.LabelingPlan == null ? (object)DBNull.Value : item.LabelingPlan);
						sqlCommand.Parameters.AddWithValue("LabelingPlanNotes" + i, item.LabelingPlanNotes == null ? (object)DBNull.Value : item.LabelingPlanNotes);
						sqlCommand.Parameters.AddWithValue("Packaging" + i, item.Packaging == null ? (object)DBNull.Value : item.Packaging);
						sqlCommand.Parameters.AddWithValue("PackagingNotes" + i, item.PackagingNotes == null ? (object)DBNull.Value : item.PackagingNotes);
						sqlCommand.Parameters.AddWithValue("PendingCpValidation" + i, item.PendingCpValidation);
						sqlCommand.Parameters.AddWithValue("Pressing" + i, item.Pressing == null ? (object)DBNull.Value : item.Pressing);
						sqlCommand.Parameters.AddWithValue("PressingNotes" + i, item.PressingNotes == null ? (object)DBNull.Value : item.PressingNotes);
						sqlCommand.Parameters.AddWithValue("QualityControl" + i, item.QualityControl);
						sqlCommand.Parameters.AddWithValue("QualityControlEditDate" + i, item.QualityControlEditDate == null ? (object)DBNull.Value : item.QualityControlEditDate);
						sqlCommand.Parameters.AddWithValue("QualityControlEditUserEmail" + i, item.QualityControlEditUserEmail == null ? (object)DBNull.Value : item.QualityControlEditUserEmail);
						sqlCommand.Parameters.AddWithValue("QualityControlEditUserId" + i, item.QualityControlEditUserId == null ? (object)DBNull.Value : item.QualityControlEditUserId);
						sqlCommand.Parameters.AddWithValue("QualityControlEditUserName" + i, item.QualityControlEditUserName == null ? (object)DBNull.Value : item.QualityControlEditUserName);
						sqlCommand.Parameters.AddWithValue("QualityDistribution" + i, item.QualityDistribution);
						sqlCommand.Parameters.AddWithValue("QualityDistributionEditDate" + i, item.QualityDistributionEditDate == null ? (object)DBNull.Value : item.QualityDistributionEditDate);
						sqlCommand.Parameters.AddWithValue("QualityDistributionEditUserEmail" + i, item.QualityDistributionEditUserEmail == null ? (object)DBNull.Value : item.QualityDistributionEditUserEmail);
						sqlCommand.Parameters.AddWithValue("QualityDistributionEditUserId" + i, item.QualityDistributionEditUserId == null ? (object)DBNull.Value : item.QualityDistributionEditUserId);
						sqlCommand.Parameters.AddWithValue("QualityDistributionEditUserName" + i, item.QualityDistributionEditUserName == null ? (object)DBNull.Value : item.QualityDistributionEditUserName);
						sqlCommand.Parameters.AddWithValue("QualityPrint" + i, item.QualityPrint);
						sqlCommand.Parameters.AddWithValue("QualityPrintEditDate" + i, item.QualityPrintEditDate == null ? (object)DBNull.Value : item.QualityPrintEditDate);
						sqlCommand.Parameters.AddWithValue("QualityPrintEditUserEmail" + i, item.QualityPrintEditUserEmail == null ? (object)DBNull.Value : item.QualityPrintEditUserEmail);
						sqlCommand.Parameters.AddWithValue("QualityPrintEditUserId" + i, item.QualityPrintEditUserId == null ? (object)DBNull.Value : item.QualityPrintEditUserId);
						sqlCommand.Parameters.AddWithValue("QualityPrintEditUserName" + i, item.QualityPrintEditUserName == null ? (object)DBNull.Value : item.QualityPrintEditUserName);
						sqlCommand.Parameters.AddWithValue("QualityUpdate" + i, item.QualityUpdate);
						sqlCommand.Parameters.AddWithValue("QualityUpdateEditDate" + i, item.QualityUpdateEditDate == null ? (object)DBNull.Value : item.QualityUpdateEditDate);
						sqlCommand.Parameters.AddWithValue("QualityUpdateEditUserEmail" + i, item.QualityUpdateEditUserEmail == null ? (object)DBNull.Value : item.QualityUpdateEditUserEmail);
						sqlCommand.Parameters.AddWithValue("QualityUpdateEditUserId" + i, item.QualityUpdateEditUserId == null ? (object)DBNull.Value : item.QualityUpdateEditUserId);
						sqlCommand.Parameters.AddWithValue("QualityUpdateEditUserName" + i, item.QualityUpdateEditUserName == null ? (object)DBNull.Value : item.QualityUpdateEditUserName);
						sqlCommand.Parameters.AddWithValue("QualityValidationFull" + i, item.QualityValidationFull);
						sqlCommand.Parameters.AddWithValue("Readiness" + i, item.Readiness == null ? (object)DBNull.Value : item.Readiness);
						sqlCommand.Parameters.AddWithValue("ReadinessNotes" + i, item.ReadinessNotes == null ? (object)DBNull.Value : item.ReadinessNotes);
						sqlCommand.Parameters.AddWithValue("Translation" + i, item.Translation == null ? (object)DBNull.Value : item.Translation);
						sqlCommand.Parameters.AddWithValue("TranslationNotes" + i, item.TranslationNotes == null ? (object)DBNull.Value : item.TranslationNotes);
						sqlCommand.Parameters.AddWithValue("Twisting" + i, item.Twisting == null ? (object)DBNull.Value : item.Twisting);
						sqlCommand.Parameters.AddWithValue("TwistingNotes" + i, item.TwistingNotes == null ? (object)DBNull.Value : item.TwistingNotes);
						sqlCommand.Parameters.AddWithValue("Ultrason" + i, item.Ultrason == null ? (object)DBNull.Value : item.Ultrason);
						sqlCommand.Parameters.AddWithValue("UltrasonNotes" + i, item.UltrasonNotes == null ? (object)DBNull.Value : item.UltrasonNotes);
						sqlCommand.Parameters.AddWithValue("Validation" + i, item.Validation == null ? (object)DBNull.Value : item.Validation);
						sqlCommand.Parameters.AddWithValue("ValidationFull" + i, item.ValidationFull);
						sqlCommand.Parameters.AddWithValue("ValidationNotes" + i, item.ValidationNotes == null ? (object)DBNull.Value : item.ValidationNotes);
						sqlCommand.Parameters.AddWithValue("Welding" + i, item.Welding == null ? (object)DBNull.Value : item.Welding);
						sqlCommand.Parameters.AddWithValue("WeldingNotes" + i, item.WeldingNotes == null ? (object)DBNull.Value : item.WeldingNotes);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__BSD_StucklistenArticle_VersionValidation] SET [ArticleBOMVersion]=@ArticleBOMVersion, [ArticleCPVersion]=@ArticleCPVersion, [ArticleKundenIndex]=@ArticleKundenIndex, [ArticleKundenIndexDatum]=@ArticleKundenIndexDatum, [ArticleNr]=@ArticleNr, [ArticleNumber]=@ArticleNumber, [Assembling1]=@Assembling1, [Assembling1Notes]=@Assembling1Notes, [Assembling2]=@Assembling2, [Assembling2Notes]=@Assembling2Notes, [Assembling3]=@Assembling3, [Assembling3Notes]=@Assembling3Notes, [BOMValidationDate]=@BOMValidationDate, [Commission]=@Commission, [CommissionNotes]=@CommissionNotes, [ControleElectrical]=@ControleElectrical, [ControleElectricalNotes]=@ControleElectricalNotes, [ControleVisual]=@ControleVisual, [ControleVisualNotes]=@ControleVisualNotes, [CPValidationDate]=@CPValidationDate, [CrimpAfter]=@CrimpAfter, [CrimpAfterManual]=@CrimpAfterManual, [CrimpAfterManualNotes]=@CrimpAfterManualNotes, [CrimpAfterNotes]=@CrimpAfterNotes, [CrimpBefore]=@CrimpBefore, [CrimpBeforeManual]=@CrimpBeforeManual, [CrimpBeforeManualNotes]=@CrimpBeforeManualNotes, [CrimpBeforeNotes]=@CrimpBeforeNotes, [EngineeringControl]=@EngineeringControl, [EngineeringControlEditDate]=@EngineeringControlEditDate, [EngineeringControlEditUserEmail]=@EngineeringControlEditUserEmail, [EngineeringControlEditUserId]=@EngineeringControlEditUserId, [EngineeringControlEditUserName]=@EngineeringControlEditUserName, [EngineeringDistribution]=@EngineeringDistribution, [EngineeringDistributionEditDate]=@EngineeringDistributionEditDate, [EngineeringDistributionEditUserEmail]=@EngineeringDistributionEditUserEmail, [EngineeringDistributionEditUserId]=@EngineeringDistributionEditUserId, [EngineeringDistributionEditUserName]=@EngineeringDistributionEditUserName, [EngineeringPrint]=@EngineeringPrint, [EngineeringPrintEditDate]=@EngineeringPrintEditDate, [EngineeringPrintEditUserEmail]=@EngineeringPrintEditUserEmail, [EngineeringPrintEditUserId]=@EngineeringPrintEditUserId, [EngineeringPrintEditUserName]=@EngineeringPrintEditUserName, [EngineeringUpdate]=@EngineeringUpdate, [EngineeringUpdateEditDate]=@EngineeringUpdateEditDate, [EngineeringUpdateEditUserEmail]=@EngineeringUpdateEditUserEmail, [EngineeringUpdateEditUserId]=@EngineeringUpdateEditUserId, [EngineeringUpdateEditUserName]=@EngineeringUpdateEditUserName, [EngineeringValidationFull]=@EngineeringValidationFull, [Finition]=@Finition, [FinitionNotes]=@FinitionNotes, [InjectionOnCables]=@InjectionOnCables, [InjectionOnCablesNotes]=@InjectionOnCablesNotes, [InjectionPlastic]=@InjectionPlastic, [InjectionPlasticNotes]=@InjectionPlasticNotes, [IsPartialDocumentation]=@IsPartialDocumentation, [LabelingPlan]=@LabelingPlan, [LabelingPlanNotes]=@LabelingPlanNotes, [Packaging]=@Packaging, [PackagingNotes]=@PackagingNotes, [PendingCpValidation]=@PendingCpValidation, [Pressing]=@Pressing, [PressingNotes]=@PressingNotes, [QualityControl]=@QualityControl, [QualityControlEditDate]=@QualityControlEditDate, [QualityControlEditUserEmail]=@QualityControlEditUserEmail, [QualityControlEditUserId]=@QualityControlEditUserId, [QualityControlEditUserName]=@QualityControlEditUserName, [QualityDistribution]=@QualityDistribution, [QualityDistributionEditDate]=@QualityDistributionEditDate, [QualityDistributionEditUserEmail]=@QualityDistributionEditUserEmail, [QualityDistributionEditUserId]=@QualityDistributionEditUserId, [QualityDistributionEditUserName]=@QualityDistributionEditUserName, [QualityPrint]=@QualityPrint, [QualityPrintEditDate]=@QualityPrintEditDate, [QualityPrintEditUserEmail]=@QualityPrintEditUserEmail, [QualityPrintEditUserId]=@QualityPrintEditUserId, [QualityPrintEditUserName]=@QualityPrintEditUserName, [QualityUpdate]=@QualityUpdate, [QualityUpdateEditDate]=@QualityUpdateEditDate, [QualityUpdateEditUserEmail]=@QualityUpdateEditUserEmail, [QualityUpdateEditUserId]=@QualityUpdateEditUserId, [QualityUpdateEditUserName]=@QualityUpdateEditUserName, [QualityValidationFull]=@QualityValidationFull, [Readiness]=@Readiness, [ReadinessNotes]=@ReadinessNotes, [Translation]=@Translation, [TranslationNotes]=@TranslationNotes, [Twisting]=@Twisting, [TwistingNotes]=@TwistingNotes, [Ultrason]=@Ultrason, [UltrasonNotes]=@UltrasonNotes, [Validation]=@Validation, [ValidationFull]=@ValidationFull, [ValidationNotes]=@ValidationNotes, [Welding]=@Welding, [WeldingNotes]=@WeldingNotes WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArticleBOMVersion", item.ArticleBOMVersion);
				sqlCommand.Parameters.AddWithValue("ArticleCPVersion", item.ArticleCPVersion == null ? (object)DBNull.Value : item.ArticleCPVersion);
				sqlCommand.Parameters.AddWithValue("ArticleKundenIndex", item.ArticleKundenIndex == null ? (object)DBNull.Value : item.ArticleKundenIndex);
				sqlCommand.Parameters.AddWithValue("ArticleKundenIndexDatum", item.ArticleKundenIndexDatum == null ? (object)DBNull.Value : item.ArticleKundenIndexDatum);
				sqlCommand.Parameters.AddWithValue("ArticleNr", item.ArticleNr);
				sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber);
				sqlCommand.Parameters.AddWithValue("Assembling1", item.Assembling1 == null ? (object)DBNull.Value : item.Assembling1);
				sqlCommand.Parameters.AddWithValue("Assembling1Notes", item.Assembling1Notes == null ? (object)DBNull.Value : item.Assembling1Notes);
				sqlCommand.Parameters.AddWithValue("Assembling2", item.Assembling2 == null ? (object)DBNull.Value : item.Assembling2);
				sqlCommand.Parameters.AddWithValue("Assembling2Notes", item.Assembling2Notes == null ? (object)DBNull.Value : item.Assembling2Notes);
				sqlCommand.Parameters.AddWithValue("Assembling3", item.Assembling3 == null ? (object)DBNull.Value : item.Assembling3);
				sqlCommand.Parameters.AddWithValue("Assembling3Notes", item.Assembling3Notes == null ? (object)DBNull.Value : item.Assembling3Notes);
				sqlCommand.Parameters.AddWithValue("BOMValidationDate", item.BOMValidationDate);
				sqlCommand.Parameters.AddWithValue("Commission", item.Commission == null ? (object)DBNull.Value : item.Commission);
				sqlCommand.Parameters.AddWithValue("CommissionNotes", item.CommissionNotes == null ? (object)DBNull.Value : item.CommissionNotes);
				sqlCommand.Parameters.AddWithValue("ControleElectrical", item.ControleElectrical == null ? (object)DBNull.Value : item.ControleElectrical);
				sqlCommand.Parameters.AddWithValue("ControleElectricalNotes", item.ControleElectricalNotes == null ? (object)DBNull.Value : item.ControleElectricalNotes);
				sqlCommand.Parameters.AddWithValue("ControleVisual", item.ControleVisual == null ? (object)DBNull.Value : item.ControleVisual);
				sqlCommand.Parameters.AddWithValue("ControleVisualNotes", item.ControleVisualNotes == null ? (object)DBNull.Value : item.ControleVisualNotes);
				sqlCommand.Parameters.AddWithValue("CPValidationDate", item.CPValidationDate == null ? (object)DBNull.Value : item.CPValidationDate);
				sqlCommand.Parameters.AddWithValue("CrimpAfter", item.CrimpAfter == null ? (object)DBNull.Value : item.CrimpAfter);
				sqlCommand.Parameters.AddWithValue("CrimpAfterManual", item.CrimpAfterManual == null ? (object)DBNull.Value : item.CrimpAfterManual);
				sqlCommand.Parameters.AddWithValue("CrimpAfterManualNotes", item.CrimpAfterManualNotes == null ? (object)DBNull.Value : item.CrimpAfterManualNotes);
				sqlCommand.Parameters.AddWithValue("CrimpAfterNotes", item.CrimpAfterNotes == null ? (object)DBNull.Value : item.CrimpAfterNotes);
				sqlCommand.Parameters.AddWithValue("CrimpBefore", item.CrimpBefore == null ? (object)DBNull.Value : item.CrimpBefore);
				sqlCommand.Parameters.AddWithValue("CrimpBeforeManual", item.CrimpBeforeManual == null ? (object)DBNull.Value : item.CrimpBeforeManual);
				sqlCommand.Parameters.AddWithValue("CrimpBeforeManualNotes", item.CrimpBeforeManualNotes == null ? (object)DBNull.Value : item.CrimpBeforeManualNotes);
				sqlCommand.Parameters.AddWithValue("CrimpBeforeNotes", item.CrimpBeforeNotes == null ? (object)DBNull.Value : item.CrimpBeforeNotes);
				sqlCommand.Parameters.AddWithValue("EngineeringControl", item.EngineeringControl);
				sqlCommand.Parameters.AddWithValue("EngineeringControlEditDate", item.EngineeringControlEditDate == null ? (object)DBNull.Value : item.EngineeringControlEditDate);
				sqlCommand.Parameters.AddWithValue("EngineeringControlEditUserEmail", item.EngineeringControlEditUserEmail == null ? (object)DBNull.Value : item.EngineeringControlEditUserEmail);
				sqlCommand.Parameters.AddWithValue("EngineeringControlEditUserId", item.EngineeringControlEditUserId == null ? (object)DBNull.Value : item.EngineeringControlEditUserId);
				sqlCommand.Parameters.AddWithValue("EngineeringControlEditUserName", item.EngineeringControlEditUserName == null ? (object)DBNull.Value : item.EngineeringControlEditUserName);
				sqlCommand.Parameters.AddWithValue("EngineeringDistribution", item.EngineeringDistribution);
				sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditDate", item.EngineeringDistributionEditDate == null ? (object)DBNull.Value : item.EngineeringDistributionEditDate);
				sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditUserEmail", item.EngineeringDistributionEditUserEmail == null ? (object)DBNull.Value : item.EngineeringDistributionEditUserEmail);
				sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditUserId", item.EngineeringDistributionEditUserId == null ? (object)DBNull.Value : item.EngineeringDistributionEditUserId);
				sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditUserName", item.EngineeringDistributionEditUserName == null ? (object)DBNull.Value : item.EngineeringDistributionEditUserName);
				sqlCommand.Parameters.AddWithValue("EngineeringPrint", item.EngineeringPrint);
				sqlCommand.Parameters.AddWithValue("EngineeringPrintEditDate", item.EngineeringPrintEditDate == null ? (object)DBNull.Value : item.EngineeringPrintEditDate);
				sqlCommand.Parameters.AddWithValue("EngineeringPrintEditUserEmail", item.EngineeringPrintEditUserEmail == null ? (object)DBNull.Value : item.EngineeringPrintEditUserEmail);
				sqlCommand.Parameters.AddWithValue("EngineeringPrintEditUserId", item.EngineeringPrintEditUserId == null ? (object)DBNull.Value : item.EngineeringPrintEditUserId);
				sqlCommand.Parameters.AddWithValue("EngineeringPrintEditUserName", item.EngineeringPrintEditUserName == null ? (object)DBNull.Value : item.EngineeringPrintEditUserName);
				sqlCommand.Parameters.AddWithValue("EngineeringUpdate", item.EngineeringUpdate);
				sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditDate", item.EngineeringUpdateEditDate == null ? (object)DBNull.Value : item.EngineeringUpdateEditDate);
				sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditUserEmail", item.EngineeringUpdateEditUserEmail == null ? (object)DBNull.Value : item.EngineeringUpdateEditUserEmail);
				sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditUserId", item.EngineeringUpdateEditUserId == null ? (object)DBNull.Value : item.EngineeringUpdateEditUserId);
				sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditUserName", item.EngineeringUpdateEditUserName == null ? (object)DBNull.Value : item.EngineeringUpdateEditUserName);
				sqlCommand.Parameters.AddWithValue("EngineeringValidationFull", item.EngineeringValidationFull);
				sqlCommand.Parameters.AddWithValue("Finition", item.Finition == null ? (object)DBNull.Value : item.Finition);
				sqlCommand.Parameters.AddWithValue("FinitionNotes", item.FinitionNotes == null ? (object)DBNull.Value : item.FinitionNotes);
				sqlCommand.Parameters.AddWithValue("InjectionOnCables", item.InjectionOnCables == null ? (object)DBNull.Value : item.InjectionOnCables);
				sqlCommand.Parameters.AddWithValue("InjectionOnCablesNotes", item.InjectionOnCablesNotes == null ? (object)DBNull.Value : item.InjectionOnCablesNotes);
				sqlCommand.Parameters.AddWithValue("InjectionPlastic", item.InjectionPlastic == null ? (object)DBNull.Value : item.InjectionPlastic);
				sqlCommand.Parameters.AddWithValue("InjectionPlasticNotes", item.InjectionPlasticNotes == null ? (object)DBNull.Value : item.InjectionPlasticNotes);
				sqlCommand.Parameters.AddWithValue("IsPartialDocumentation", item.IsPartialDocumentation == null ? (object)DBNull.Value : item.IsPartialDocumentation);
				sqlCommand.Parameters.AddWithValue("LabelingPlan", item.LabelingPlan == null ? (object)DBNull.Value : item.LabelingPlan);
				sqlCommand.Parameters.AddWithValue("LabelingPlanNotes", item.LabelingPlanNotes == null ? (object)DBNull.Value : item.LabelingPlanNotes);
				sqlCommand.Parameters.AddWithValue("Packaging", item.Packaging == null ? (object)DBNull.Value : item.Packaging);
				sqlCommand.Parameters.AddWithValue("PackagingNotes", item.PackagingNotes == null ? (object)DBNull.Value : item.PackagingNotes);
				sqlCommand.Parameters.AddWithValue("PendingCpValidation", item.PendingCpValidation);
				sqlCommand.Parameters.AddWithValue("Pressing", item.Pressing == null ? (object)DBNull.Value : item.Pressing);
				sqlCommand.Parameters.AddWithValue("PressingNotes", item.PressingNotes == null ? (object)DBNull.Value : item.PressingNotes);
				sqlCommand.Parameters.AddWithValue("QualityControl", item.QualityControl);
				sqlCommand.Parameters.AddWithValue("QualityControlEditDate", item.QualityControlEditDate == null ? (object)DBNull.Value : item.QualityControlEditDate);
				sqlCommand.Parameters.AddWithValue("QualityControlEditUserEmail", item.QualityControlEditUserEmail == null ? (object)DBNull.Value : item.QualityControlEditUserEmail);
				sqlCommand.Parameters.AddWithValue("QualityControlEditUserId", item.QualityControlEditUserId == null ? (object)DBNull.Value : item.QualityControlEditUserId);
				sqlCommand.Parameters.AddWithValue("QualityControlEditUserName", item.QualityControlEditUserName == null ? (object)DBNull.Value : item.QualityControlEditUserName);
				sqlCommand.Parameters.AddWithValue("QualityDistribution", item.QualityDistribution);
				sqlCommand.Parameters.AddWithValue("QualityDistributionEditDate", item.QualityDistributionEditDate == null ? (object)DBNull.Value : item.QualityDistributionEditDate);
				sqlCommand.Parameters.AddWithValue("QualityDistributionEditUserEmail", item.QualityDistributionEditUserEmail == null ? (object)DBNull.Value : item.QualityDistributionEditUserEmail);
				sqlCommand.Parameters.AddWithValue("QualityDistributionEditUserId", item.QualityDistributionEditUserId == null ? (object)DBNull.Value : item.QualityDistributionEditUserId);
				sqlCommand.Parameters.AddWithValue("QualityDistributionEditUserName", item.QualityDistributionEditUserName == null ? (object)DBNull.Value : item.QualityDistributionEditUserName);
				sqlCommand.Parameters.AddWithValue("QualityPrint", item.QualityPrint);
				sqlCommand.Parameters.AddWithValue("QualityPrintEditDate", item.QualityPrintEditDate == null ? (object)DBNull.Value : item.QualityPrintEditDate);
				sqlCommand.Parameters.AddWithValue("QualityPrintEditUserEmail", item.QualityPrintEditUserEmail == null ? (object)DBNull.Value : item.QualityPrintEditUserEmail);
				sqlCommand.Parameters.AddWithValue("QualityPrintEditUserId", item.QualityPrintEditUserId == null ? (object)DBNull.Value : item.QualityPrintEditUserId);
				sqlCommand.Parameters.AddWithValue("QualityPrintEditUserName", item.QualityPrintEditUserName == null ? (object)DBNull.Value : item.QualityPrintEditUserName);
				sqlCommand.Parameters.AddWithValue("QualityUpdate", item.QualityUpdate);
				sqlCommand.Parameters.AddWithValue("QualityUpdateEditDate", item.QualityUpdateEditDate == null ? (object)DBNull.Value : item.QualityUpdateEditDate);
				sqlCommand.Parameters.AddWithValue("QualityUpdateEditUserEmail", item.QualityUpdateEditUserEmail == null ? (object)DBNull.Value : item.QualityUpdateEditUserEmail);
				sqlCommand.Parameters.AddWithValue("QualityUpdateEditUserId", item.QualityUpdateEditUserId == null ? (object)DBNull.Value : item.QualityUpdateEditUserId);
				sqlCommand.Parameters.AddWithValue("QualityUpdateEditUserName", item.QualityUpdateEditUserName == null ? (object)DBNull.Value : item.QualityUpdateEditUserName);
				sqlCommand.Parameters.AddWithValue("QualityValidationFull", item.QualityValidationFull);
				sqlCommand.Parameters.AddWithValue("Readiness", item.Readiness == null ? (object)DBNull.Value : item.Readiness);
				sqlCommand.Parameters.AddWithValue("ReadinessNotes", item.ReadinessNotes == null ? (object)DBNull.Value : item.ReadinessNotes);
				sqlCommand.Parameters.AddWithValue("Translation", item.Translation == null ? (object)DBNull.Value : item.Translation);
				sqlCommand.Parameters.AddWithValue("TranslationNotes", item.TranslationNotes == null ? (object)DBNull.Value : item.TranslationNotes);
				sqlCommand.Parameters.AddWithValue("Twisting", item.Twisting == null ? (object)DBNull.Value : item.Twisting);
				sqlCommand.Parameters.AddWithValue("TwistingNotes", item.TwistingNotes == null ? (object)DBNull.Value : item.TwistingNotes);
				sqlCommand.Parameters.AddWithValue("Ultrason", item.Ultrason == null ? (object)DBNull.Value : item.Ultrason);
				sqlCommand.Parameters.AddWithValue("UltrasonNotes", item.UltrasonNotes == null ? (object)DBNull.Value : item.UltrasonNotes);
				sqlCommand.Parameters.AddWithValue("Validation", item.Validation == null ? (object)DBNull.Value : item.Validation);
				sqlCommand.Parameters.AddWithValue("ValidationFull", item.ValidationFull);
				sqlCommand.Parameters.AddWithValue("ValidationNotes", item.ValidationNotes == null ? (object)DBNull.Value : item.ValidationNotes);
				sqlCommand.Parameters.AddWithValue("Welding", item.Welding == null ? (object)DBNull.Value : item.Welding);
				sqlCommand.Parameters.AddWithValue("WeldingNotes", item.WeldingNotes == null ? (object)DBNull.Value : item.WeldingNotes);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 99; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity> items)
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
						query += " UPDATE [__BSD_StucklistenArticle_VersionValidation] SET "

							+ "[ArticleBOMVersion]=@ArticleBOMVersion" + i + ","
							+ "[ArticleCPVersion]=@ArticleCPVersion" + i + ","
							+ "[ArticleKundenIndex]=@ArticleKundenIndex" + i + ","
							+ "[ArticleKundenIndexDatum]=@ArticleKundenIndexDatum" + i + ","
							+ "[ArticleNr]=@ArticleNr" + i + ","
							+ "[ArticleNumber]=@ArticleNumber" + i + ","
							+ "[Assembling1]=@Assembling1" + i + ","
							+ "[Assembling1Notes]=@Assembling1Notes" + i + ","
							+ "[Assembling2]=@Assembling2" + i + ","
							+ "[Assembling2Notes]=@Assembling2Notes" + i + ","
							+ "[Assembling3]=@Assembling3" + i + ","
							+ "[Assembling3Notes]=@Assembling3Notes" + i + ","
							+ "[BOMValidationDate]=@BOMValidationDate" + i + ","
							+ "[Commission]=@Commission" + i + ","
							+ "[CommissionNotes]=@CommissionNotes" + i + ","
							+ "[ControleElectrical]=@ControleElectrical" + i + ","
							+ "[ControleElectricalNotes]=@ControleElectricalNotes" + i + ","
							+ "[ControleVisual]=@ControleVisual" + i + ","
							+ "[ControleVisualNotes]=@ControleVisualNotes" + i + ","
							+ "[CPValidationDate]=@CPValidationDate" + i + ","
							+ "[CrimpAfter]=@CrimpAfter" + i + ","
							+ "[CrimpAfterManual]=@CrimpAfterManual" + i + ","
							+ "[CrimpAfterManualNotes]=@CrimpAfterManualNotes" + i + ","
							+ "[CrimpAfterNotes]=@CrimpAfterNotes" + i + ","
							+ "[CrimpBefore]=@CrimpBefore" + i + ","
							+ "[CrimpBeforeManual]=@CrimpBeforeManual" + i + ","
							+ "[CrimpBeforeManualNotes]=@CrimpBeforeManualNotes" + i + ","
							+ "[CrimpBeforeNotes]=@CrimpBeforeNotes" + i + ","
							+ "[EngineeringControl]=@EngineeringControl" + i + ","
							+ "[EngineeringControlEditDate]=@EngineeringControlEditDate" + i + ","
							+ "[EngineeringControlEditUserEmail]=@EngineeringControlEditUserEmail" + i + ","
							+ "[EngineeringControlEditUserId]=@EngineeringControlEditUserId" + i + ","
							+ "[EngineeringControlEditUserName]=@EngineeringControlEditUserName" + i + ","
							+ "[EngineeringDistribution]=@EngineeringDistribution" + i + ","
							+ "[EngineeringDistributionEditDate]=@EngineeringDistributionEditDate" + i + ","
							+ "[EngineeringDistributionEditUserEmail]=@EngineeringDistributionEditUserEmail" + i + ","
							+ "[EngineeringDistributionEditUserId]=@EngineeringDistributionEditUserId" + i + ","
							+ "[EngineeringDistributionEditUserName]=@EngineeringDistributionEditUserName" + i + ","
							+ "[EngineeringPrint]=@EngineeringPrint" + i + ","
							+ "[EngineeringPrintEditDate]=@EngineeringPrintEditDate" + i + ","
							+ "[EngineeringPrintEditUserEmail]=@EngineeringPrintEditUserEmail" + i + ","
							+ "[EngineeringPrintEditUserId]=@EngineeringPrintEditUserId" + i + ","
							+ "[EngineeringPrintEditUserName]=@EngineeringPrintEditUserName" + i + ","
							+ "[EngineeringUpdate]=@EngineeringUpdate" + i + ","
							+ "[EngineeringUpdateEditDate]=@EngineeringUpdateEditDate" + i + ","
							+ "[EngineeringUpdateEditUserEmail]=@EngineeringUpdateEditUserEmail" + i + ","
							+ "[EngineeringUpdateEditUserId]=@EngineeringUpdateEditUserId" + i + ","
							+ "[EngineeringUpdateEditUserName]=@EngineeringUpdateEditUserName" + i + ","
							+ "[EngineeringValidationFull]=@EngineeringValidationFull" + i + ","
							+ "[Finition]=@Finition" + i + ","
							+ "[FinitionNotes]=@FinitionNotes" + i + ","
							+ "[InjectionOnCables]=@InjectionOnCables" + i + ","
							+ "[InjectionOnCablesNotes]=@InjectionOnCablesNotes" + i + ","
							+ "[InjectionPlastic]=@InjectionPlastic" + i + ","
							+ "[InjectionPlasticNotes]=@InjectionPlasticNotes" + i + ","
							+ "[IsPartialDocumentation]=@IsPartialDocumentation" + i + ","
							+ "[LabelingPlan]=@LabelingPlan" + i + ","
							+ "[LabelingPlanNotes]=@LabelingPlanNotes" + i + ","
							+ "[Packaging]=@Packaging" + i + ","
							+ "[PackagingNotes]=@PackagingNotes" + i + ","
							+ "[PendingCpValidation]=@PendingCpValidation" + i + ","
							+ "[Pressing]=@Pressing" + i + ","
							+ "[PressingNotes]=@PressingNotes" + i + ","
							+ "[QualityControl]=@QualityControl" + i + ","
							+ "[QualityControlEditDate]=@QualityControlEditDate" + i + ","
							+ "[QualityControlEditUserEmail]=@QualityControlEditUserEmail" + i + ","
							+ "[QualityControlEditUserId]=@QualityControlEditUserId" + i + ","
							+ "[QualityControlEditUserName]=@QualityControlEditUserName" + i + ","
							+ "[QualityDistribution]=@QualityDistribution" + i + ","
							+ "[QualityDistributionEditDate]=@QualityDistributionEditDate" + i + ","
							+ "[QualityDistributionEditUserEmail]=@QualityDistributionEditUserEmail" + i + ","
							+ "[QualityDistributionEditUserId]=@QualityDistributionEditUserId" + i + ","
							+ "[QualityDistributionEditUserName]=@QualityDistributionEditUserName" + i + ","
							+ "[QualityPrint]=@QualityPrint" + i + ","
							+ "[QualityPrintEditDate]=@QualityPrintEditDate" + i + ","
							+ "[QualityPrintEditUserEmail]=@QualityPrintEditUserEmail" + i + ","
							+ "[QualityPrintEditUserId]=@QualityPrintEditUserId" + i + ","
							+ "[QualityPrintEditUserName]=@QualityPrintEditUserName" + i + ","
							+ "[QualityUpdate]=@QualityUpdate" + i + ","
							+ "[QualityUpdateEditDate]=@QualityUpdateEditDate" + i + ","
							+ "[QualityUpdateEditUserEmail]=@QualityUpdateEditUserEmail" + i + ","
							+ "[QualityUpdateEditUserId]=@QualityUpdateEditUserId" + i + ","
							+ "[QualityUpdateEditUserName]=@QualityUpdateEditUserName" + i + ","
							+ "[QualityValidationFull]=@QualityValidationFull" + i + ","
							+ "[Readiness]=@Readiness" + i + ","
							+ "[ReadinessNotes]=@ReadinessNotes" + i + ","
							+ "[Translation]=@Translation" + i + ","
							+ "[TranslationNotes]=@TranslationNotes" + i + ","
							+ "[Twisting]=@Twisting" + i + ","
							+ "[TwistingNotes]=@TwistingNotes" + i + ","
							+ "[Ultrason]=@Ultrason" + i + ","
							+ "[UltrasonNotes]=@UltrasonNotes" + i + ","
							+ "[Validation]=@Validation" + i + ","
							+ "[ValidationFull]=@ValidationFull" + i + ","
							+ "[ValidationNotes]=@ValidationNotes" + i + ","
							+ "[Welding]=@Welding" + i + ","
							+ "[WeldingNotes]=@WeldingNotes" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ArticleBOMVersion" + i, item.ArticleBOMVersion);
						sqlCommand.Parameters.AddWithValue("ArticleCPVersion" + i, item.ArticleCPVersion == null ? (object)DBNull.Value : item.ArticleCPVersion);
						sqlCommand.Parameters.AddWithValue("ArticleKundenIndex" + i, item.ArticleKundenIndex == null ? (object)DBNull.Value : item.ArticleKundenIndex);
						sqlCommand.Parameters.AddWithValue("ArticleKundenIndexDatum" + i, item.ArticleKundenIndexDatum == null ? (object)DBNull.Value : item.ArticleKundenIndexDatum);
						sqlCommand.Parameters.AddWithValue("ArticleNr" + i, item.ArticleNr);
						sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber);
						sqlCommand.Parameters.AddWithValue("Assembling1" + i, item.Assembling1 == null ? (object)DBNull.Value : item.Assembling1);
						sqlCommand.Parameters.AddWithValue("Assembling1Notes" + i, item.Assembling1Notes == null ? (object)DBNull.Value : item.Assembling1Notes);
						sqlCommand.Parameters.AddWithValue("Assembling2" + i, item.Assembling2 == null ? (object)DBNull.Value : item.Assembling2);
						sqlCommand.Parameters.AddWithValue("Assembling2Notes" + i, item.Assembling2Notes == null ? (object)DBNull.Value : item.Assembling2Notes);
						sqlCommand.Parameters.AddWithValue("Assembling3" + i, item.Assembling3 == null ? (object)DBNull.Value : item.Assembling3);
						sqlCommand.Parameters.AddWithValue("Assembling3Notes" + i, item.Assembling3Notes == null ? (object)DBNull.Value : item.Assembling3Notes);
						sqlCommand.Parameters.AddWithValue("BOMValidationDate" + i, item.BOMValidationDate);
						sqlCommand.Parameters.AddWithValue("Commission" + i, item.Commission == null ? (object)DBNull.Value : item.Commission);
						sqlCommand.Parameters.AddWithValue("CommissionNotes" + i, item.CommissionNotes == null ? (object)DBNull.Value : item.CommissionNotes);
						sqlCommand.Parameters.AddWithValue("ControleElectrical" + i, item.ControleElectrical == null ? (object)DBNull.Value : item.ControleElectrical);
						sqlCommand.Parameters.AddWithValue("ControleElectricalNotes" + i, item.ControleElectricalNotes == null ? (object)DBNull.Value : item.ControleElectricalNotes);
						sqlCommand.Parameters.AddWithValue("ControleVisual" + i, item.ControleVisual == null ? (object)DBNull.Value : item.ControleVisual);
						sqlCommand.Parameters.AddWithValue("ControleVisualNotes" + i, item.ControleVisualNotes == null ? (object)DBNull.Value : item.ControleVisualNotes);
						sqlCommand.Parameters.AddWithValue("CPValidationDate" + i, item.CPValidationDate == null ? (object)DBNull.Value : item.CPValidationDate);
						sqlCommand.Parameters.AddWithValue("CrimpAfter" + i, item.CrimpAfter == null ? (object)DBNull.Value : item.CrimpAfter);
						sqlCommand.Parameters.AddWithValue("CrimpAfterManual" + i, item.CrimpAfterManual == null ? (object)DBNull.Value : item.CrimpAfterManual);
						sqlCommand.Parameters.AddWithValue("CrimpAfterManualNotes" + i, item.CrimpAfterManualNotes == null ? (object)DBNull.Value : item.CrimpAfterManualNotes);
						sqlCommand.Parameters.AddWithValue("CrimpAfterNotes" + i, item.CrimpAfterNotes == null ? (object)DBNull.Value : item.CrimpAfterNotes);
						sqlCommand.Parameters.AddWithValue("CrimpBefore" + i, item.CrimpBefore == null ? (object)DBNull.Value : item.CrimpBefore);
						sqlCommand.Parameters.AddWithValue("CrimpBeforeManual" + i, item.CrimpBeforeManual == null ? (object)DBNull.Value : item.CrimpBeforeManual);
						sqlCommand.Parameters.AddWithValue("CrimpBeforeManualNotes" + i, item.CrimpBeforeManualNotes == null ? (object)DBNull.Value : item.CrimpBeforeManualNotes);
						sqlCommand.Parameters.AddWithValue("CrimpBeforeNotes" + i, item.CrimpBeforeNotes == null ? (object)DBNull.Value : item.CrimpBeforeNotes);
						sqlCommand.Parameters.AddWithValue("EngineeringControl" + i, item.EngineeringControl);
						sqlCommand.Parameters.AddWithValue("EngineeringControlEditDate" + i, item.EngineeringControlEditDate == null ? (object)DBNull.Value : item.EngineeringControlEditDate);
						sqlCommand.Parameters.AddWithValue("EngineeringControlEditUserEmail" + i, item.EngineeringControlEditUserEmail == null ? (object)DBNull.Value : item.EngineeringControlEditUserEmail);
						sqlCommand.Parameters.AddWithValue("EngineeringControlEditUserId" + i, item.EngineeringControlEditUserId == null ? (object)DBNull.Value : item.EngineeringControlEditUserId);
						sqlCommand.Parameters.AddWithValue("EngineeringControlEditUserName" + i, item.EngineeringControlEditUserName == null ? (object)DBNull.Value : item.EngineeringControlEditUserName);
						sqlCommand.Parameters.AddWithValue("EngineeringDistribution" + i, item.EngineeringDistribution);
						sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditDate" + i, item.EngineeringDistributionEditDate == null ? (object)DBNull.Value : item.EngineeringDistributionEditDate);
						sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditUserEmail" + i, item.EngineeringDistributionEditUserEmail == null ? (object)DBNull.Value : item.EngineeringDistributionEditUserEmail);
						sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditUserId" + i, item.EngineeringDistributionEditUserId == null ? (object)DBNull.Value : item.EngineeringDistributionEditUserId);
						sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditUserName" + i, item.EngineeringDistributionEditUserName == null ? (object)DBNull.Value : item.EngineeringDistributionEditUserName);
						sqlCommand.Parameters.AddWithValue("EngineeringPrint" + i, item.EngineeringPrint);
						sqlCommand.Parameters.AddWithValue("EngineeringPrintEditDate" + i, item.EngineeringPrintEditDate == null ? (object)DBNull.Value : item.EngineeringPrintEditDate);
						sqlCommand.Parameters.AddWithValue("EngineeringPrintEditUserEmail" + i, item.EngineeringPrintEditUserEmail == null ? (object)DBNull.Value : item.EngineeringPrintEditUserEmail);
						sqlCommand.Parameters.AddWithValue("EngineeringPrintEditUserId" + i, item.EngineeringPrintEditUserId == null ? (object)DBNull.Value : item.EngineeringPrintEditUserId);
						sqlCommand.Parameters.AddWithValue("EngineeringPrintEditUserName" + i, item.EngineeringPrintEditUserName == null ? (object)DBNull.Value : item.EngineeringPrintEditUserName);
						sqlCommand.Parameters.AddWithValue("EngineeringUpdate" + i, item.EngineeringUpdate);
						sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditDate" + i, item.EngineeringUpdateEditDate == null ? (object)DBNull.Value : item.EngineeringUpdateEditDate);
						sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditUserEmail" + i, item.EngineeringUpdateEditUserEmail == null ? (object)DBNull.Value : item.EngineeringUpdateEditUserEmail);
						sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditUserId" + i, item.EngineeringUpdateEditUserId == null ? (object)DBNull.Value : item.EngineeringUpdateEditUserId);
						sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditUserName" + i, item.EngineeringUpdateEditUserName == null ? (object)DBNull.Value : item.EngineeringUpdateEditUserName);
						sqlCommand.Parameters.AddWithValue("EngineeringValidationFull" + i, item.EngineeringValidationFull);
						sqlCommand.Parameters.AddWithValue("Finition" + i, item.Finition == null ? (object)DBNull.Value : item.Finition);
						sqlCommand.Parameters.AddWithValue("FinitionNotes" + i, item.FinitionNotes == null ? (object)DBNull.Value : item.FinitionNotes);
						sqlCommand.Parameters.AddWithValue("InjectionOnCables" + i, item.InjectionOnCables == null ? (object)DBNull.Value : item.InjectionOnCables);
						sqlCommand.Parameters.AddWithValue("InjectionOnCablesNotes" + i, item.InjectionOnCablesNotes == null ? (object)DBNull.Value : item.InjectionOnCablesNotes);
						sqlCommand.Parameters.AddWithValue("InjectionPlastic" + i, item.InjectionPlastic == null ? (object)DBNull.Value : item.InjectionPlastic);
						sqlCommand.Parameters.AddWithValue("InjectionPlasticNotes" + i, item.InjectionPlasticNotes == null ? (object)DBNull.Value : item.InjectionPlasticNotes);
						sqlCommand.Parameters.AddWithValue("IsPartialDocumentation" + i, item.IsPartialDocumentation == null ? (object)DBNull.Value : item.IsPartialDocumentation);
						sqlCommand.Parameters.AddWithValue("LabelingPlan" + i, item.LabelingPlan == null ? (object)DBNull.Value : item.LabelingPlan);
						sqlCommand.Parameters.AddWithValue("LabelingPlanNotes" + i, item.LabelingPlanNotes == null ? (object)DBNull.Value : item.LabelingPlanNotes);
						sqlCommand.Parameters.AddWithValue("Packaging" + i, item.Packaging == null ? (object)DBNull.Value : item.Packaging);
						sqlCommand.Parameters.AddWithValue("PackagingNotes" + i, item.PackagingNotes == null ? (object)DBNull.Value : item.PackagingNotes);
						sqlCommand.Parameters.AddWithValue("PendingCpValidation" + i, item.PendingCpValidation);
						sqlCommand.Parameters.AddWithValue("Pressing" + i, item.Pressing == null ? (object)DBNull.Value : item.Pressing);
						sqlCommand.Parameters.AddWithValue("PressingNotes" + i, item.PressingNotes == null ? (object)DBNull.Value : item.PressingNotes);
						sqlCommand.Parameters.AddWithValue("QualityControl" + i, item.QualityControl);
						sqlCommand.Parameters.AddWithValue("QualityControlEditDate" + i, item.QualityControlEditDate == null ? (object)DBNull.Value : item.QualityControlEditDate);
						sqlCommand.Parameters.AddWithValue("QualityControlEditUserEmail" + i, item.QualityControlEditUserEmail == null ? (object)DBNull.Value : item.QualityControlEditUserEmail);
						sqlCommand.Parameters.AddWithValue("QualityControlEditUserId" + i, item.QualityControlEditUserId == null ? (object)DBNull.Value : item.QualityControlEditUserId);
						sqlCommand.Parameters.AddWithValue("QualityControlEditUserName" + i, item.QualityControlEditUserName == null ? (object)DBNull.Value : item.QualityControlEditUserName);
						sqlCommand.Parameters.AddWithValue("QualityDistribution" + i, item.QualityDistribution);
						sqlCommand.Parameters.AddWithValue("QualityDistributionEditDate" + i, item.QualityDistributionEditDate == null ? (object)DBNull.Value : item.QualityDistributionEditDate);
						sqlCommand.Parameters.AddWithValue("QualityDistributionEditUserEmail" + i, item.QualityDistributionEditUserEmail == null ? (object)DBNull.Value : item.QualityDistributionEditUserEmail);
						sqlCommand.Parameters.AddWithValue("QualityDistributionEditUserId" + i, item.QualityDistributionEditUserId == null ? (object)DBNull.Value : item.QualityDistributionEditUserId);
						sqlCommand.Parameters.AddWithValue("QualityDistributionEditUserName" + i, item.QualityDistributionEditUserName == null ? (object)DBNull.Value : item.QualityDistributionEditUserName);
						sqlCommand.Parameters.AddWithValue("QualityPrint" + i, item.QualityPrint);
						sqlCommand.Parameters.AddWithValue("QualityPrintEditDate" + i, item.QualityPrintEditDate == null ? (object)DBNull.Value : item.QualityPrintEditDate);
						sqlCommand.Parameters.AddWithValue("QualityPrintEditUserEmail" + i, item.QualityPrintEditUserEmail == null ? (object)DBNull.Value : item.QualityPrintEditUserEmail);
						sqlCommand.Parameters.AddWithValue("QualityPrintEditUserId" + i, item.QualityPrintEditUserId == null ? (object)DBNull.Value : item.QualityPrintEditUserId);
						sqlCommand.Parameters.AddWithValue("QualityPrintEditUserName" + i, item.QualityPrintEditUserName == null ? (object)DBNull.Value : item.QualityPrintEditUserName);
						sqlCommand.Parameters.AddWithValue("QualityUpdate" + i, item.QualityUpdate);
						sqlCommand.Parameters.AddWithValue("QualityUpdateEditDate" + i, item.QualityUpdateEditDate == null ? (object)DBNull.Value : item.QualityUpdateEditDate);
						sqlCommand.Parameters.AddWithValue("QualityUpdateEditUserEmail" + i, item.QualityUpdateEditUserEmail == null ? (object)DBNull.Value : item.QualityUpdateEditUserEmail);
						sqlCommand.Parameters.AddWithValue("QualityUpdateEditUserId" + i, item.QualityUpdateEditUserId == null ? (object)DBNull.Value : item.QualityUpdateEditUserId);
						sqlCommand.Parameters.AddWithValue("QualityUpdateEditUserName" + i, item.QualityUpdateEditUserName == null ? (object)DBNull.Value : item.QualityUpdateEditUserName);
						sqlCommand.Parameters.AddWithValue("QualityValidationFull" + i, item.QualityValidationFull);
						sqlCommand.Parameters.AddWithValue("Readiness" + i, item.Readiness == null ? (object)DBNull.Value : item.Readiness);
						sqlCommand.Parameters.AddWithValue("ReadinessNotes" + i, item.ReadinessNotes == null ? (object)DBNull.Value : item.ReadinessNotes);
						sqlCommand.Parameters.AddWithValue("Translation" + i, item.Translation == null ? (object)DBNull.Value : item.Translation);
						sqlCommand.Parameters.AddWithValue("TranslationNotes" + i, item.TranslationNotes == null ? (object)DBNull.Value : item.TranslationNotes);
						sqlCommand.Parameters.AddWithValue("Twisting" + i, item.Twisting == null ? (object)DBNull.Value : item.Twisting);
						sqlCommand.Parameters.AddWithValue("TwistingNotes" + i, item.TwistingNotes == null ? (object)DBNull.Value : item.TwistingNotes);
						sqlCommand.Parameters.AddWithValue("Ultrason" + i, item.Ultrason == null ? (object)DBNull.Value : item.Ultrason);
						sqlCommand.Parameters.AddWithValue("UltrasonNotes" + i, item.UltrasonNotes == null ? (object)DBNull.Value : item.UltrasonNotes);
						sqlCommand.Parameters.AddWithValue("Validation" + i, item.Validation == null ? (object)DBNull.Value : item.Validation);
						sqlCommand.Parameters.AddWithValue("ValidationFull" + i, item.ValidationFull);
						sqlCommand.Parameters.AddWithValue("ValidationNotes" + i, item.ValidationNotes == null ? (object)DBNull.Value : item.ValidationNotes);
						sqlCommand.Parameters.AddWithValue("Welding" + i, item.Welding == null ? (object)DBNull.Value : item.Welding);
						sqlCommand.Parameters.AddWithValue("WeldingNotes" + i, item.WeldingNotes == null ? (object)DBNull.Value : item.WeldingNotes);
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
				string query = "DELETE FROM [__BSD_StucklistenArticle_VersionValidation] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__BSD_StucklistenArticle_VersionValidation] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__BSD_StucklistenArticle_VersionValidation] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__BSD_StucklistenArticle_VersionValidation]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__BSD_StucklistenArticle_VersionValidation] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [__BSD_StucklistenArticle_VersionValidation] ([ArticleBOMVersion],[ArticleCPVersion],[ArticleKundenIndex],[ArticleKundenIndexDatum],[ArticleNr],[ArticleNumber],[Assembling1],[Assembling1Notes],[Assembling2],[Assembling2Notes],[Assembling3],[Assembling3Notes],[BOMValidationDate],[Commission],[CommissionNotes],[ControleElectrical],[ControleElectricalNotes],[ControleVisual],[ControleVisualNotes],[CPValidationDate],[CrimpAfter],[CrimpAfterManual],[CrimpAfterManualNotes],[CrimpAfterNotes],[CrimpBefore],[CrimpBeforeManual],[CrimpBeforeManualNotes],[CrimpBeforeNotes],[EngineeringControl],[EngineeringControlEditDate],[EngineeringControlEditUserEmail],[EngineeringControlEditUserId],[EngineeringControlEditUserName],[EngineeringDistribution],[EngineeringDistributionEditDate],[EngineeringDistributionEditUserEmail],[EngineeringDistributionEditUserId],[EngineeringDistributionEditUserName],[EngineeringPrint],[EngineeringPrintEditDate],[EngineeringPrintEditUserEmail],[EngineeringPrintEditUserId],[EngineeringPrintEditUserName],[EngineeringUpdate],[EngineeringUpdateEditDate],[EngineeringUpdateEditUserEmail],[EngineeringUpdateEditUserId],[EngineeringUpdateEditUserName],[EngineeringValidationFull],[Finition],[FinitionNotes],[InjectionOnCables],[InjectionOnCablesNotes],[InjectionPlastic],[InjectionPlasticNotes],[IsPartialDocumentation],[LabelingPlan],[LabelingPlanNotes],[Packaging],[PackagingNotes],[PendingCpValidation],[Pressing],[PressingNotes],[QualityControl],[QualityControlEditDate],[QualityControlEditUserEmail],[QualityControlEditUserId],[QualityControlEditUserName],[QualityDistribution],[QualityDistributionEditDate],[QualityDistributionEditUserEmail],[QualityDistributionEditUserId],[QualityDistributionEditUserName],[QualityPrint],[QualityPrintEditDate],[QualityPrintEditUserEmail],[QualityPrintEditUserId],[QualityPrintEditUserName],[QualityUpdate],[QualityUpdateEditDate],[QualityUpdateEditUserEmail],[QualityUpdateEditUserId],[QualityUpdateEditUserName],[QualityValidationFull],[Readiness],[ReadinessNotes],[Translation],[TranslationNotes],[Twisting],[TwistingNotes],[Ultrason],[UltrasonNotes],[Validation],[ValidationFull],[ValidationNotes],[Welding],[WeldingNotes]) OUTPUT INSERTED.[Id] VALUES (@ArticleBOMVersion,@ArticleCPVersion,@ArticleKundenIndex,@ArticleKundenIndexDatum,@ArticleNr,@ArticleNumber,@Assembling1,@Assembling1Notes,@Assembling2,@Assembling2Notes,@Assembling3,@Assembling3Notes,@BOMValidationDate,@Commission,@CommissionNotes,@ControleElectrical,@ControleElectricalNotes,@ControleVisual,@ControleVisualNotes,@CPValidationDate,@CrimpAfter,@CrimpAfterManual,@CrimpAfterManualNotes,@CrimpAfterNotes,@CrimpBefore,@CrimpBeforeManual,@CrimpBeforeManualNotes,@CrimpBeforeNotes,@EngineeringControl,@EngineeringControlEditDate,@EngineeringControlEditUserEmail,@EngineeringControlEditUserId,@EngineeringControlEditUserName,@EngineeringDistribution,@EngineeringDistributionEditDate,@EngineeringDistributionEditUserEmail,@EngineeringDistributionEditUserId,@EngineeringDistributionEditUserName,@EngineeringPrint,@EngineeringPrintEditDate,@EngineeringPrintEditUserEmail,@EngineeringPrintEditUserId,@EngineeringPrintEditUserName,@EngineeringUpdate,@EngineeringUpdateEditDate,@EngineeringUpdateEditUserEmail,@EngineeringUpdateEditUserId,@EngineeringUpdateEditUserName,@EngineeringValidationFull,@Finition,@FinitionNotes,@InjectionOnCables,@InjectionOnCablesNotes,@InjectionPlastic,@InjectionPlasticNotes,@IsPartialDocumentation,@LabelingPlan,@LabelingPlanNotes,@Packaging,@PackagingNotes,@PendingCpValidation,@Pressing,@PressingNotes,@QualityControl,@QualityControlEditDate,@QualityControlEditUserEmail,@QualityControlEditUserId,@QualityControlEditUserName,@QualityDistribution,@QualityDistributionEditDate,@QualityDistributionEditUserEmail,@QualityDistributionEditUserId,@QualityDistributionEditUserName,@QualityPrint,@QualityPrintEditDate,@QualityPrintEditUserEmail,@QualityPrintEditUserId,@QualityPrintEditUserName,@QualityUpdate,@QualityUpdateEditDate,@QualityUpdateEditUserEmail,@QualityUpdateEditUserId,@QualityUpdateEditUserName,@QualityValidationFull,@Readiness,@ReadinessNotes,@Translation,@TranslationNotes,@Twisting,@TwistingNotes,@Ultrason,@UltrasonNotes,@Validation,@ValidationFull,@ValidationNotes,@Welding,@WeldingNotes); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ArticleBOMVersion", item.ArticleBOMVersion);
			sqlCommand.Parameters.AddWithValue("ArticleCPVersion", item.ArticleCPVersion == null ? (object)DBNull.Value : item.ArticleCPVersion);
			sqlCommand.Parameters.AddWithValue("ArticleKundenIndex", item.ArticleKundenIndex == null ? (object)DBNull.Value : item.ArticleKundenIndex);
			sqlCommand.Parameters.AddWithValue("ArticleKundenIndexDatum", item.ArticleKundenIndexDatum == null ? (object)DBNull.Value : item.ArticleKundenIndexDatum);
			sqlCommand.Parameters.AddWithValue("ArticleNr", item.ArticleNr);
			sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber);
			sqlCommand.Parameters.AddWithValue("Assembling1", item.Assembling1 == null ? (object)DBNull.Value : item.Assembling1);
			sqlCommand.Parameters.AddWithValue("Assembling1Notes", item.Assembling1Notes == null ? (object)DBNull.Value : item.Assembling1Notes);
			sqlCommand.Parameters.AddWithValue("Assembling2", item.Assembling2 == null ? (object)DBNull.Value : item.Assembling2);
			sqlCommand.Parameters.AddWithValue("Assembling2Notes", item.Assembling2Notes == null ? (object)DBNull.Value : item.Assembling2Notes);
			sqlCommand.Parameters.AddWithValue("Assembling3", item.Assembling3 == null ? (object)DBNull.Value : item.Assembling3);
			sqlCommand.Parameters.AddWithValue("Assembling3Notes", item.Assembling3Notes == null ? (object)DBNull.Value : item.Assembling3Notes);
			sqlCommand.Parameters.AddWithValue("BOMValidationDate", item.BOMValidationDate);
			sqlCommand.Parameters.AddWithValue("Commission", item.Commission == null ? (object)DBNull.Value : item.Commission);
			sqlCommand.Parameters.AddWithValue("CommissionNotes", item.CommissionNotes == null ? (object)DBNull.Value : item.CommissionNotes);
			sqlCommand.Parameters.AddWithValue("ControleElectrical", item.ControleElectrical == null ? (object)DBNull.Value : item.ControleElectrical);
			sqlCommand.Parameters.AddWithValue("ControleElectricalNotes", item.ControleElectricalNotes == null ? (object)DBNull.Value : item.ControleElectricalNotes);
			sqlCommand.Parameters.AddWithValue("ControleVisual", item.ControleVisual == null ? (object)DBNull.Value : item.ControleVisual);
			sqlCommand.Parameters.AddWithValue("ControleVisualNotes", item.ControleVisualNotes == null ? (object)DBNull.Value : item.ControleVisualNotes);
			sqlCommand.Parameters.AddWithValue("CPValidationDate", item.CPValidationDate == null ? (object)DBNull.Value : item.CPValidationDate);
			sqlCommand.Parameters.AddWithValue("CrimpAfter", item.CrimpAfter == null ? (object)DBNull.Value : item.CrimpAfter);
			sqlCommand.Parameters.AddWithValue("CrimpAfterManual", item.CrimpAfterManual == null ? (object)DBNull.Value : item.CrimpAfterManual);
			sqlCommand.Parameters.AddWithValue("CrimpAfterManualNotes", item.CrimpAfterManualNotes == null ? (object)DBNull.Value : item.CrimpAfterManualNotes);
			sqlCommand.Parameters.AddWithValue("CrimpAfterNotes", item.CrimpAfterNotes == null ? (object)DBNull.Value : item.CrimpAfterNotes);
			sqlCommand.Parameters.AddWithValue("CrimpBefore", item.CrimpBefore == null ? (object)DBNull.Value : item.CrimpBefore);
			sqlCommand.Parameters.AddWithValue("CrimpBeforeManual", item.CrimpBeforeManual == null ? (object)DBNull.Value : item.CrimpBeforeManual);
			sqlCommand.Parameters.AddWithValue("CrimpBeforeManualNotes", item.CrimpBeforeManualNotes == null ? (object)DBNull.Value : item.CrimpBeforeManualNotes);
			sqlCommand.Parameters.AddWithValue("CrimpBeforeNotes", item.CrimpBeforeNotes == null ? (object)DBNull.Value : item.CrimpBeforeNotes);
			sqlCommand.Parameters.AddWithValue("EngineeringControl", item.EngineeringControl);
			sqlCommand.Parameters.AddWithValue("EngineeringControlEditDate", item.EngineeringControlEditDate == null ? (object)DBNull.Value : item.EngineeringControlEditDate);
			sqlCommand.Parameters.AddWithValue("EngineeringControlEditUserEmail", item.EngineeringControlEditUserEmail == null ? (object)DBNull.Value : item.EngineeringControlEditUserEmail);
			sqlCommand.Parameters.AddWithValue("EngineeringControlEditUserId", item.EngineeringControlEditUserId == null ? (object)DBNull.Value : item.EngineeringControlEditUserId);
			sqlCommand.Parameters.AddWithValue("EngineeringControlEditUserName", item.EngineeringControlEditUserName == null ? (object)DBNull.Value : item.EngineeringControlEditUserName);
			sqlCommand.Parameters.AddWithValue("EngineeringDistribution", item.EngineeringDistribution);
			sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditDate", item.EngineeringDistributionEditDate == null ? (object)DBNull.Value : item.EngineeringDistributionEditDate);
			sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditUserEmail", item.EngineeringDistributionEditUserEmail == null ? (object)DBNull.Value : item.EngineeringDistributionEditUserEmail);
			sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditUserId", item.EngineeringDistributionEditUserId == null ? (object)DBNull.Value : item.EngineeringDistributionEditUserId);
			sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditUserName", item.EngineeringDistributionEditUserName == null ? (object)DBNull.Value : item.EngineeringDistributionEditUserName);
			sqlCommand.Parameters.AddWithValue("EngineeringPrint", item.EngineeringPrint);
			sqlCommand.Parameters.AddWithValue("EngineeringPrintEditDate", item.EngineeringPrintEditDate == null ? (object)DBNull.Value : item.EngineeringPrintEditDate);
			sqlCommand.Parameters.AddWithValue("EngineeringPrintEditUserEmail", item.EngineeringPrintEditUserEmail == null ? (object)DBNull.Value : item.EngineeringPrintEditUserEmail);
			sqlCommand.Parameters.AddWithValue("EngineeringPrintEditUserId", item.EngineeringPrintEditUserId == null ? (object)DBNull.Value : item.EngineeringPrintEditUserId);
			sqlCommand.Parameters.AddWithValue("EngineeringPrintEditUserName", item.EngineeringPrintEditUserName == null ? (object)DBNull.Value : item.EngineeringPrintEditUserName);
			sqlCommand.Parameters.AddWithValue("EngineeringUpdate", item.EngineeringUpdate);
			sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditDate", item.EngineeringUpdateEditDate == null ? (object)DBNull.Value : item.EngineeringUpdateEditDate);
			sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditUserEmail", item.EngineeringUpdateEditUserEmail == null ? (object)DBNull.Value : item.EngineeringUpdateEditUserEmail);
			sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditUserId", item.EngineeringUpdateEditUserId == null ? (object)DBNull.Value : item.EngineeringUpdateEditUserId);
			sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditUserName", item.EngineeringUpdateEditUserName == null ? (object)DBNull.Value : item.EngineeringUpdateEditUserName);
			sqlCommand.Parameters.AddWithValue("EngineeringValidationFull", item.EngineeringValidationFull);
			sqlCommand.Parameters.AddWithValue("Finition", item.Finition == null ? (object)DBNull.Value : item.Finition);
			sqlCommand.Parameters.AddWithValue("FinitionNotes", item.FinitionNotes == null ? (object)DBNull.Value : item.FinitionNotes);
			sqlCommand.Parameters.AddWithValue("InjectionOnCables", item.InjectionOnCables == null ? (object)DBNull.Value : item.InjectionOnCables);
			sqlCommand.Parameters.AddWithValue("InjectionOnCablesNotes", item.InjectionOnCablesNotes == null ? (object)DBNull.Value : item.InjectionOnCablesNotes);
			sqlCommand.Parameters.AddWithValue("InjectionPlastic", item.InjectionPlastic == null ? (object)DBNull.Value : item.InjectionPlastic);
			sqlCommand.Parameters.AddWithValue("InjectionPlasticNotes", item.InjectionPlasticNotes == null ? (object)DBNull.Value : item.InjectionPlasticNotes);
			sqlCommand.Parameters.AddWithValue("IsPartialDocumentation", item.IsPartialDocumentation == null ? (object)DBNull.Value : item.IsPartialDocumentation);
			sqlCommand.Parameters.AddWithValue("LabelingPlan", item.LabelingPlan == null ? (object)DBNull.Value : item.LabelingPlan);
			sqlCommand.Parameters.AddWithValue("LabelingPlanNotes", item.LabelingPlanNotes == null ? (object)DBNull.Value : item.LabelingPlanNotes);
			sqlCommand.Parameters.AddWithValue("Packaging", item.Packaging == null ? (object)DBNull.Value : item.Packaging);
			sqlCommand.Parameters.AddWithValue("PackagingNotes", item.PackagingNotes == null ? (object)DBNull.Value : item.PackagingNotes);
			sqlCommand.Parameters.AddWithValue("PendingCpValidation", item.PendingCpValidation);
			sqlCommand.Parameters.AddWithValue("Pressing", item.Pressing == null ? (object)DBNull.Value : item.Pressing);
			sqlCommand.Parameters.AddWithValue("PressingNotes", item.PressingNotes == null ? (object)DBNull.Value : item.PressingNotes);
			sqlCommand.Parameters.AddWithValue("QualityControl", item.QualityControl);
			sqlCommand.Parameters.AddWithValue("QualityControlEditDate", item.QualityControlEditDate == null ? (object)DBNull.Value : item.QualityControlEditDate);
			sqlCommand.Parameters.AddWithValue("QualityControlEditUserEmail", item.QualityControlEditUserEmail == null ? (object)DBNull.Value : item.QualityControlEditUserEmail);
			sqlCommand.Parameters.AddWithValue("QualityControlEditUserId", item.QualityControlEditUserId == null ? (object)DBNull.Value : item.QualityControlEditUserId);
			sqlCommand.Parameters.AddWithValue("QualityControlEditUserName", item.QualityControlEditUserName == null ? (object)DBNull.Value : item.QualityControlEditUserName);
			sqlCommand.Parameters.AddWithValue("QualityDistribution", item.QualityDistribution);
			sqlCommand.Parameters.AddWithValue("QualityDistributionEditDate", item.QualityDistributionEditDate == null ? (object)DBNull.Value : item.QualityDistributionEditDate);
			sqlCommand.Parameters.AddWithValue("QualityDistributionEditUserEmail", item.QualityDistributionEditUserEmail == null ? (object)DBNull.Value : item.QualityDistributionEditUserEmail);
			sqlCommand.Parameters.AddWithValue("QualityDistributionEditUserId", item.QualityDistributionEditUserId == null ? (object)DBNull.Value : item.QualityDistributionEditUserId);
			sqlCommand.Parameters.AddWithValue("QualityDistributionEditUserName", item.QualityDistributionEditUserName == null ? (object)DBNull.Value : item.QualityDistributionEditUserName);
			sqlCommand.Parameters.AddWithValue("QualityPrint", item.QualityPrint);
			sqlCommand.Parameters.AddWithValue("QualityPrintEditDate", item.QualityPrintEditDate == null ? (object)DBNull.Value : item.QualityPrintEditDate);
			sqlCommand.Parameters.AddWithValue("QualityPrintEditUserEmail", item.QualityPrintEditUserEmail == null ? (object)DBNull.Value : item.QualityPrintEditUserEmail);
			sqlCommand.Parameters.AddWithValue("QualityPrintEditUserId", item.QualityPrintEditUserId == null ? (object)DBNull.Value : item.QualityPrintEditUserId);
			sqlCommand.Parameters.AddWithValue("QualityPrintEditUserName", item.QualityPrintEditUserName == null ? (object)DBNull.Value : item.QualityPrintEditUserName);
			sqlCommand.Parameters.AddWithValue("QualityUpdate", item.QualityUpdate);
			sqlCommand.Parameters.AddWithValue("QualityUpdateEditDate", item.QualityUpdateEditDate == null ? (object)DBNull.Value : item.QualityUpdateEditDate);
			sqlCommand.Parameters.AddWithValue("QualityUpdateEditUserEmail", item.QualityUpdateEditUserEmail == null ? (object)DBNull.Value : item.QualityUpdateEditUserEmail);
			sqlCommand.Parameters.AddWithValue("QualityUpdateEditUserId", item.QualityUpdateEditUserId == null ? (object)DBNull.Value : item.QualityUpdateEditUserId);
			sqlCommand.Parameters.AddWithValue("QualityUpdateEditUserName", item.QualityUpdateEditUserName == null ? (object)DBNull.Value : item.QualityUpdateEditUserName);
			sqlCommand.Parameters.AddWithValue("QualityValidationFull", item.QualityValidationFull);
			sqlCommand.Parameters.AddWithValue("Readiness", item.Readiness == null ? (object)DBNull.Value : item.Readiness);
			sqlCommand.Parameters.AddWithValue("ReadinessNotes", item.ReadinessNotes == null ? (object)DBNull.Value : item.ReadinessNotes);
			sqlCommand.Parameters.AddWithValue("Translation", item.Translation == null ? (object)DBNull.Value : item.Translation);
			sqlCommand.Parameters.AddWithValue("TranslationNotes", item.TranslationNotes == null ? (object)DBNull.Value : item.TranslationNotes);
			sqlCommand.Parameters.AddWithValue("Twisting", item.Twisting == null ? (object)DBNull.Value : item.Twisting);
			sqlCommand.Parameters.AddWithValue("TwistingNotes", item.TwistingNotes == null ? (object)DBNull.Value : item.TwistingNotes);
			sqlCommand.Parameters.AddWithValue("Ultrason", item.Ultrason == null ? (object)DBNull.Value : item.Ultrason);
			sqlCommand.Parameters.AddWithValue("UltrasonNotes", item.UltrasonNotes == null ? (object)DBNull.Value : item.UltrasonNotes);
			sqlCommand.Parameters.AddWithValue("Validation", item.Validation == null ? (object)DBNull.Value : item.Validation);
			sqlCommand.Parameters.AddWithValue("ValidationFull", item.ValidationFull);
			sqlCommand.Parameters.AddWithValue("ValidationNotes", item.ValidationNotes == null ? (object)DBNull.Value : item.ValidationNotes);
			sqlCommand.Parameters.AddWithValue("Welding", item.Welding == null ? (object)DBNull.Value : item.Welding);
			sqlCommand.Parameters.AddWithValue("WeldingNotes", item.WeldingNotes == null ? (object)DBNull.Value : item.WeldingNotes);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 99; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__BSD_StucklistenArticle_VersionValidation] ([ArticleBOMVersion],[ArticleCPVersion],[ArticleKundenIndex],[ArticleKundenIndexDatum],[ArticleNr],[ArticleNumber],[Assembling1],[Assembling1Notes],[Assembling2],[Assembling2Notes],[Assembling3],[Assembling3Notes],[BOMValidationDate],[Commission],[CommissionNotes],[ControleElectrical],[ControleElectricalNotes],[ControleVisual],[ControleVisualNotes],[CPValidationDate],[CrimpAfter],[CrimpAfterManual],[CrimpAfterManualNotes],[CrimpAfterNotes],[CrimpBefore],[CrimpBeforeManual],[CrimpBeforeManualNotes],[CrimpBeforeNotes],[EngineeringControl],[EngineeringControlEditDate],[EngineeringControlEditUserEmail],[EngineeringControlEditUserId],[EngineeringControlEditUserName],[EngineeringDistribution],[EngineeringDistributionEditDate],[EngineeringDistributionEditUserEmail],[EngineeringDistributionEditUserId],[EngineeringDistributionEditUserName],[EngineeringPrint],[EngineeringPrintEditDate],[EngineeringPrintEditUserEmail],[EngineeringPrintEditUserId],[EngineeringPrintEditUserName],[EngineeringUpdate],[EngineeringUpdateEditDate],[EngineeringUpdateEditUserEmail],[EngineeringUpdateEditUserId],[EngineeringUpdateEditUserName],[EngineeringValidationFull],[Finition],[FinitionNotes],[InjectionOnCables],[InjectionOnCablesNotes],[InjectionPlastic],[InjectionPlasticNotes],[IsPartialDocumentation],[LabelingPlan],[LabelingPlanNotes],[Packaging],[PackagingNotes],[PendingCpValidation],[Pressing],[PressingNotes],[QualityControl],[QualityControlEditDate],[QualityControlEditUserEmail],[QualityControlEditUserId],[QualityControlEditUserName],[QualityDistribution],[QualityDistributionEditDate],[QualityDistributionEditUserEmail],[QualityDistributionEditUserId],[QualityDistributionEditUserName],[QualityPrint],[QualityPrintEditDate],[QualityPrintEditUserEmail],[QualityPrintEditUserId],[QualityPrintEditUserName],[QualityUpdate],[QualityUpdateEditDate],[QualityUpdateEditUserEmail],[QualityUpdateEditUserId],[QualityUpdateEditUserName],[QualityValidationFull],[Readiness],[ReadinessNotes],[Translation],[TranslationNotes],[Twisting],[TwistingNotes],[Ultrason],[UltrasonNotes],[Validation],[ValidationFull],[ValidationNotes],[Welding],[WeldingNotes]) VALUES ( "

						+ "@ArticleBOMVersion" + i + ","
						+ "@ArticleCPVersion" + i + ","
						+ "@ArticleKundenIndex" + i + ","
						+ "@ArticleKundenIndexDatum" + i + ","
						+ "@ArticleNr" + i + ","
						+ "@ArticleNumber" + i + ","
						+ "@Assembling1" + i + ","
						+ "@Assembling1Notes" + i + ","
						+ "@Assembling2" + i + ","
						+ "@Assembling2Notes" + i + ","
						+ "@Assembling3" + i + ","
						+ "@Assembling3Notes" + i + ","
						+ "@BOMValidationDate" + i + ","
						+ "@Commission" + i + ","
						+ "@CommissionNotes" + i + ","
						+ "@ControleElectrical" + i + ","
						+ "@ControleElectricalNotes" + i + ","
						+ "@ControleVisual" + i + ","
						+ "@ControleVisualNotes" + i + ","
						+ "@CPValidationDate" + i + ","
						+ "@CrimpAfter" + i + ","
						+ "@CrimpAfterManual" + i + ","
						+ "@CrimpAfterManualNotes" + i + ","
						+ "@CrimpAfterNotes" + i + ","
						+ "@CrimpBefore" + i + ","
						+ "@CrimpBeforeManual" + i + ","
						+ "@CrimpBeforeManualNotes" + i + ","
						+ "@CrimpBeforeNotes" + i + ","
						+ "@EngineeringControl" + i + ","
						+ "@EngineeringControlEditDate" + i + ","
						+ "@EngineeringControlEditUserEmail" + i + ","
						+ "@EngineeringControlEditUserId" + i + ","
						+ "@EngineeringControlEditUserName" + i + ","
						+ "@EngineeringDistribution" + i + ","
						+ "@EngineeringDistributionEditDate" + i + ","
						+ "@EngineeringDistributionEditUserEmail" + i + ","
						+ "@EngineeringDistributionEditUserId" + i + ","
						+ "@EngineeringDistributionEditUserName" + i + ","
						+ "@EngineeringPrint" + i + ","
						+ "@EngineeringPrintEditDate" + i + ","
						+ "@EngineeringPrintEditUserEmail" + i + ","
						+ "@EngineeringPrintEditUserId" + i + ","
						+ "@EngineeringPrintEditUserName" + i + ","
						+ "@EngineeringUpdate" + i + ","
						+ "@EngineeringUpdateEditDate" + i + ","
						+ "@EngineeringUpdateEditUserEmail" + i + ","
						+ "@EngineeringUpdateEditUserId" + i + ","
						+ "@EngineeringUpdateEditUserName" + i + ","
						+ "@EngineeringValidationFull" + i + ","
						+ "@Finition" + i + ","
						+ "@FinitionNotes" + i + ","
						+ "@InjectionOnCables" + i + ","
						+ "@InjectionOnCablesNotes" + i + ","
						+ "@InjectionPlastic" + i + ","
						+ "@InjectionPlasticNotes" + i + ","
						+ "@IsPartialDocumentation" + i + ","
						+ "@LabelingPlan" + i + ","
						+ "@LabelingPlanNotes" + i + ","
						+ "@Packaging" + i + ","
						+ "@PackagingNotes" + i + ","
						+ "@PendingCpValidation" + i + ","
						+ "@Pressing" + i + ","
						+ "@PressingNotes" + i + ","
						+ "@QualityControl" + i + ","
						+ "@QualityControlEditDate" + i + ","
						+ "@QualityControlEditUserEmail" + i + ","
						+ "@QualityControlEditUserId" + i + ","
						+ "@QualityControlEditUserName" + i + ","
						+ "@QualityDistribution" + i + ","
						+ "@QualityDistributionEditDate" + i + ","
						+ "@QualityDistributionEditUserEmail" + i + ","
						+ "@QualityDistributionEditUserId" + i + ","
						+ "@QualityDistributionEditUserName" + i + ","
						+ "@QualityPrint" + i + ","
						+ "@QualityPrintEditDate" + i + ","
						+ "@QualityPrintEditUserEmail" + i + ","
						+ "@QualityPrintEditUserId" + i + ","
						+ "@QualityPrintEditUserName" + i + ","
						+ "@QualityUpdate" + i + ","
						+ "@QualityUpdateEditDate" + i + ","
						+ "@QualityUpdateEditUserEmail" + i + ","
						+ "@QualityUpdateEditUserId" + i + ","
						+ "@QualityUpdateEditUserName" + i + ","
						+ "@QualityValidationFull" + i + ","
						+ "@Readiness" + i + ","
						+ "@ReadinessNotes" + i + ","
						+ "@Translation" + i + ","
						+ "@TranslationNotes" + i + ","
						+ "@Twisting" + i + ","
						+ "@TwistingNotes" + i + ","
						+ "@Ultrason" + i + ","
						+ "@UltrasonNotes" + i + ","
						+ "@Validation" + i + ","
						+ "@ValidationFull" + i + ","
						+ "@ValidationNotes" + i + ","
						+ "@Welding" + i + ","
						+ "@WeldingNotes" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("ArticleBOMVersion" + i, item.ArticleBOMVersion);
					sqlCommand.Parameters.AddWithValue("ArticleCPVersion" + i, item.ArticleCPVersion == null ? (object)DBNull.Value : item.ArticleCPVersion);
					sqlCommand.Parameters.AddWithValue("ArticleKundenIndex" + i, item.ArticleKundenIndex == null ? (object)DBNull.Value : item.ArticleKundenIndex);
					sqlCommand.Parameters.AddWithValue("ArticleKundenIndexDatum" + i, item.ArticleKundenIndexDatum == null ? (object)DBNull.Value : item.ArticleKundenIndexDatum);
					sqlCommand.Parameters.AddWithValue("ArticleNr" + i, item.ArticleNr);
					sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("Assembling1" + i, item.Assembling1 == null ? (object)DBNull.Value : item.Assembling1);
					sqlCommand.Parameters.AddWithValue("Assembling1Notes" + i, item.Assembling1Notes == null ? (object)DBNull.Value : item.Assembling1Notes);
					sqlCommand.Parameters.AddWithValue("Assembling2" + i, item.Assembling2 == null ? (object)DBNull.Value : item.Assembling2);
					sqlCommand.Parameters.AddWithValue("Assembling2Notes" + i, item.Assembling2Notes == null ? (object)DBNull.Value : item.Assembling2Notes);
					sqlCommand.Parameters.AddWithValue("Assembling3" + i, item.Assembling3 == null ? (object)DBNull.Value : item.Assembling3);
					sqlCommand.Parameters.AddWithValue("Assembling3Notes" + i, item.Assembling3Notes == null ? (object)DBNull.Value : item.Assembling3Notes);
					sqlCommand.Parameters.AddWithValue("BOMValidationDate" + i, item.BOMValidationDate);
					sqlCommand.Parameters.AddWithValue("Commission" + i, item.Commission == null ? (object)DBNull.Value : item.Commission);
					sqlCommand.Parameters.AddWithValue("CommissionNotes" + i, item.CommissionNotes == null ? (object)DBNull.Value : item.CommissionNotes);
					sqlCommand.Parameters.AddWithValue("ControleElectrical" + i, item.ControleElectrical == null ? (object)DBNull.Value : item.ControleElectrical);
					sqlCommand.Parameters.AddWithValue("ControleElectricalNotes" + i, item.ControleElectricalNotes == null ? (object)DBNull.Value : item.ControleElectricalNotes);
					sqlCommand.Parameters.AddWithValue("ControleVisual" + i, item.ControleVisual == null ? (object)DBNull.Value : item.ControleVisual);
					sqlCommand.Parameters.AddWithValue("ControleVisualNotes" + i, item.ControleVisualNotes == null ? (object)DBNull.Value : item.ControleVisualNotes);
					sqlCommand.Parameters.AddWithValue("CPValidationDate" + i, item.CPValidationDate == null ? (object)DBNull.Value : item.CPValidationDate);
					sqlCommand.Parameters.AddWithValue("CrimpAfter" + i, item.CrimpAfter == null ? (object)DBNull.Value : item.CrimpAfter);
					sqlCommand.Parameters.AddWithValue("CrimpAfterManual" + i, item.CrimpAfterManual == null ? (object)DBNull.Value : item.CrimpAfterManual);
					sqlCommand.Parameters.AddWithValue("CrimpAfterManualNotes" + i, item.CrimpAfterManualNotes == null ? (object)DBNull.Value : item.CrimpAfterManualNotes);
					sqlCommand.Parameters.AddWithValue("CrimpAfterNotes" + i, item.CrimpAfterNotes == null ? (object)DBNull.Value : item.CrimpAfterNotes);
					sqlCommand.Parameters.AddWithValue("CrimpBefore" + i, item.CrimpBefore == null ? (object)DBNull.Value : item.CrimpBefore);
					sqlCommand.Parameters.AddWithValue("CrimpBeforeManual" + i, item.CrimpBeforeManual == null ? (object)DBNull.Value : item.CrimpBeforeManual);
					sqlCommand.Parameters.AddWithValue("CrimpBeforeManualNotes" + i, item.CrimpBeforeManualNotes == null ? (object)DBNull.Value : item.CrimpBeforeManualNotes);
					sqlCommand.Parameters.AddWithValue("CrimpBeforeNotes" + i, item.CrimpBeforeNotes == null ? (object)DBNull.Value : item.CrimpBeforeNotes);
					sqlCommand.Parameters.AddWithValue("EngineeringControl" + i, item.EngineeringControl);
					sqlCommand.Parameters.AddWithValue("EngineeringControlEditDate" + i, item.EngineeringControlEditDate == null ? (object)DBNull.Value : item.EngineeringControlEditDate);
					sqlCommand.Parameters.AddWithValue("EngineeringControlEditUserEmail" + i, item.EngineeringControlEditUserEmail == null ? (object)DBNull.Value : item.EngineeringControlEditUserEmail);
					sqlCommand.Parameters.AddWithValue("EngineeringControlEditUserId" + i, item.EngineeringControlEditUserId == null ? (object)DBNull.Value : item.EngineeringControlEditUserId);
					sqlCommand.Parameters.AddWithValue("EngineeringControlEditUserName" + i, item.EngineeringControlEditUserName == null ? (object)DBNull.Value : item.EngineeringControlEditUserName);
					sqlCommand.Parameters.AddWithValue("EngineeringDistribution" + i, item.EngineeringDistribution);
					sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditDate" + i, item.EngineeringDistributionEditDate == null ? (object)DBNull.Value : item.EngineeringDistributionEditDate);
					sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditUserEmail" + i, item.EngineeringDistributionEditUserEmail == null ? (object)DBNull.Value : item.EngineeringDistributionEditUserEmail);
					sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditUserId" + i, item.EngineeringDistributionEditUserId == null ? (object)DBNull.Value : item.EngineeringDistributionEditUserId);
					sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditUserName" + i, item.EngineeringDistributionEditUserName == null ? (object)DBNull.Value : item.EngineeringDistributionEditUserName);
					sqlCommand.Parameters.AddWithValue("EngineeringPrint" + i, item.EngineeringPrint);
					sqlCommand.Parameters.AddWithValue("EngineeringPrintEditDate" + i, item.EngineeringPrintEditDate == null ? (object)DBNull.Value : item.EngineeringPrintEditDate);
					sqlCommand.Parameters.AddWithValue("EngineeringPrintEditUserEmail" + i, item.EngineeringPrintEditUserEmail == null ? (object)DBNull.Value : item.EngineeringPrintEditUserEmail);
					sqlCommand.Parameters.AddWithValue("EngineeringPrintEditUserId" + i, item.EngineeringPrintEditUserId == null ? (object)DBNull.Value : item.EngineeringPrintEditUserId);
					sqlCommand.Parameters.AddWithValue("EngineeringPrintEditUserName" + i, item.EngineeringPrintEditUserName == null ? (object)DBNull.Value : item.EngineeringPrintEditUserName);
					sqlCommand.Parameters.AddWithValue("EngineeringUpdate" + i, item.EngineeringUpdate);
					sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditDate" + i, item.EngineeringUpdateEditDate == null ? (object)DBNull.Value : item.EngineeringUpdateEditDate);
					sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditUserEmail" + i, item.EngineeringUpdateEditUserEmail == null ? (object)DBNull.Value : item.EngineeringUpdateEditUserEmail);
					sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditUserId" + i, item.EngineeringUpdateEditUserId == null ? (object)DBNull.Value : item.EngineeringUpdateEditUserId);
					sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditUserName" + i, item.EngineeringUpdateEditUserName == null ? (object)DBNull.Value : item.EngineeringUpdateEditUserName);
					sqlCommand.Parameters.AddWithValue("EngineeringValidationFull" + i, item.EngineeringValidationFull);
					sqlCommand.Parameters.AddWithValue("Finition" + i, item.Finition == null ? (object)DBNull.Value : item.Finition);
					sqlCommand.Parameters.AddWithValue("FinitionNotes" + i, item.FinitionNotes == null ? (object)DBNull.Value : item.FinitionNotes);
					sqlCommand.Parameters.AddWithValue("InjectionOnCables" + i, item.InjectionOnCables == null ? (object)DBNull.Value : item.InjectionOnCables);
					sqlCommand.Parameters.AddWithValue("InjectionOnCablesNotes" + i, item.InjectionOnCablesNotes == null ? (object)DBNull.Value : item.InjectionOnCablesNotes);
					sqlCommand.Parameters.AddWithValue("InjectionPlastic" + i, item.InjectionPlastic == null ? (object)DBNull.Value : item.InjectionPlastic);
					sqlCommand.Parameters.AddWithValue("InjectionPlasticNotes" + i, item.InjectionPlasticNotes == null ? (object)DBNull.Value : item.InjectionPlasticNotes);
					sqlCommand.Parameters.AddWithValue("IsPartialDocumentation" + i, item.IsPartialDocumentation == null ? (object)DBNull.Value : item.IsPartialDocumentation);
					sqlCommand.Parameters.AddWithValue("LabelingPlan" + i, item.LabelingPlan == null ? (object)DBNull.Value : item.LabelingPlan);
					sqlCommand.Parameters.AddWithValue("LabelingPlanNotes" + i, item.LabelingPlanNotes == null ? (object)DBNull.Value : item.LabelingPlanNotes);
					sqlCommand.Parameters.AddWithValue("Packaging" + i, item.Packaging == null ? (object)DBNull.Value : item.Packaging);
					sqlCommand.Parameters.AddWithValue("PackagingNotes" + i, item.PackagingNotes == null ? (object)DBNull.Value : item.PackagingNotes);
					sqlCommand.Parameters.AddWithValue("PendingCpValidation" + i, item.PendingCpValidation);
					sqlCommand.Parameters.AddWithValue("Pressing" + i, item.Pressing == null ? (object)DBNull.Value : item.Pressing);
					sqlCommand.Parameters.AddWithValue("PressingNotes" + i, item.PressingNotes == null ? (object)DBNull.Value : item.PressingNotes);
					sqlCommand.Parameters.AddWithValue("QualityControl" + i, item.QualityControl);
					sqlCommand.Parameters.AddWithValue("QualityControlEditDate" + i, item.QualityControlEditDate == null ? (object)DBNull.Value : item.QualityControlEditDate);
					sqlCommand.Parameters.AddWithValue("QualityControlEditUserEmail" + i, item.QualityControlEditUserEmail == null ? (object)DBNull.Value : item.QualityControlEditUserEmail);
					sqlCommand.Parameters.AddWithValue("QualityControlEditUserId" + i, item.QualityControlEditUserId == null ? (object)DBNull.Value : item.QualityControlEditUserId);
					sqlCommand.Parameters.AddWithValue("QualityControlEditUserName" + i, item.QualityControlEditUserName == null ? (object)DBNull.Value : item.QualityControlEditUserName);
					sqlCommand.Parameters.AddWithValue("QualityDistribution" + i, item.QualityDistribution);
					sqlCommand.Parameters.AddWithValue("QualityDistributionEditDate" + i, item.QualityDistributionEditDate == null ? (object)DBNull.Value : item.QualityDistributionEditDate);
					sqlCommand.Parameters.AddWithValue("QualityDistributionEditUserEmail" + i, item.QualityDistributionEditUserEmail == null ? (object)DBNull.Value : item.QualityDistributionEditUserEmail);
					sqlCommand.Parameters.AddWithValue("QualityDistributionEditUserId" + i, item.QualityDistributionEditUserId == null ? (object)DBNull.Value : item.QualityDistributionEditUserId);
					sqlCommand.Parameters.AddWithValue("QualityDistributionEditUserName" + i, item.QualityDistributionEditUserName == null ? (object)DBNull.Value : item.QualityDistributionEditUserName);
					sqlCommand.Parameters.AddWithValue("QualityPrint" + i, item.QualityPrint);
					sqlCommand.Parameters.AddWithValue("QualityPrintEditDate" + i, item.QualityPrintEditDate == null ? (object)DBNull.Value : item.QualityPrintEditDate);
					sqlCommand.Parameters.AddWithValue("QualityPrintEditUserEmail" + i, item.QualityPrintEditUserEmail == null ? (object)DBNull.Value : item.QualityPrintEditUserEmail);
					sqlCommand.Parameters.AddWithValue("QualityPrintEditUserId" + i, item.QualityPrintEditUserId == null ? (object)DBNull.Value : item.QualityPrintEditUserId);
					sqlCommand.Parameters.AddWithValue("QualityPrintEditUserName" + i, item.QualityPrintEditUserName == null ? (object)DBNull.Value : item.QualityPrintEditUserName);
					sqlCommand.Parameters.AddWithValue("QualityUpdate" + i, item.QualityUpdate);
					sqlCommand.Parameters.AddWithValue("QualityUpdateEditDate" + i, item.QualityUpdateEditDate == null ? (object)DBNull.Value : item.QualityUpdateEditDate);
					sqlCommand.Parameters.AddWithValue("QualityUpdateEditUserEmail" + i, item.QualityUpdateEditUserEmail == null ? (object)DBNull.Value : item.QualityUpdateEditUserEmail);
					sqlCommand.Parameters.AddWithValue("QualityUpdateEditUserId" + i, item.QualityUpdateEditUserId == null ? (object)DBNull.Value : item.QualityUpdateEditUserId);
					sqlCommand.Parameters.AddWithValue("QualityUpdateEditUserName" + i, item.QualityUpdateEditUserName == null ? (object)DBNull.Value : item.QualityUpdateEditUserName);
					sqlCommand.Parameters.AddWithValue("QualityValidationFull" + i, item.QualityValidationFull);
					sqlCommand.Parameters.AddWithValue("Readiness" + i, item.Readiness == null ? (object)DBNull.Value : item.Readiness);
					sqlCommand.Parameters.AddWithValue("ReadinessNotes" + i, item.ReadinessNotes == null ? (object)DBNull.Value : item.ReadinessNotes);
					sqlCommand.Parameters.AddWithValue("Translation" + i, item.Translation == null ? (object)DBNull.Value : item.Translation);
					sqlCommand.Parameters.AddWithValue("TranslationNotes" + i, item.TranslationNotes == null ? (object)DBNull.Value : item.TranslationNotes);
					sqlCommand.Parameters.AddWithValue("Twisting" + i, item.Twisting == null ? (object)DBNull.Value : item.Twisting);
					sqlCommand.Parameters.AddWithValue("TwistingNotes" + i, item.TwistingNotes == null ? (object)DBNull.Value : item.TwistingNotes);
					sqlCommand.Parameters.AddWithValue("Ultrason" + i, item.Ultrason == null ? (object)DBNull.Value : item.Ultrason);
					sqlCommand.Parameters.AddWithValue("UltrasonNotes" + i, item.UltrasonNotes == null ? (object)DBNull.Value : item.UltrasonNotes);
					sqlCommand.Parameters.AddWithValue("Validation" + i, item.Validation == null ? (object)DBNull.Value : item.Validation);
					sqlCommand.Parameters.AddWithValue("ValidationFull" + i, item.ValidationFull);
					sqlCommand.Parameters.AddWithValue("ValidationNotes" + i, item.ValidationNotes == null ? (object)DBNull.Value : item.ValidationNotes);
					sqlCommand.Parameters.AddWithValue("Welding" + i, item.Welding == null ? (object)DBNull.Value : item.Welding);
					sqlCommand.Parameters.AddWithValue("WeldingNotes" + i, item.WeldingNotes == null ? (object)DBNull.Value : item.WeldingNotes);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__BSD_StucklistenArticle_VersionValidation] SET [ArticleBOMVersion]=@ArticleBOMVersion, [ArticleCPVersion]=@ArticleCPVersion, [ArticleKundenIndex]=@ArticleKundenIndex, [ArticleKundenIndexDatum]=@ArticleKundenIndexDatum, [ArticleNr]=@ArticleNr, [ArticleNumber]=@ArticleNumber, [Assembling1]=@Assembling1, [Assembling1Notes]=@Assembling1Notes, [Assembling2]=@Assembling2, [Assembling2Notes]=@Assembling2Notes, [Assembling3]=@Assembling3, [Assembling3Notes]=@Assembling3Notes, [BOMValidationDate]=@BOMValidationDate, [Commission]=@Commission, [CommissionNotes]=@CommissionNotes, [ControleElectrical]=@ControleElectrical, [ControleElectricalNotes]=@ControleElectricalNotes, [ControleVisual]=@ControleVisual, [ControleVisualNotes]=@ControleVisualNotes, [CPValidationDate]=@CPValidationDate, [CrimpAfter]=@CrimpAfter, [CrimpAfterManual]=@CrimpAfterManual, [CrimpAfterManualNotes]=@CrimpAfterManualNotes, [CrimpAfterNotes]=@CrimpAfterNotes, [CrimpBefore]=@CrimpBefore, [CrimpBeforeManual]=@CrimpBeforeManual, [CrimpBeforeManualNotes]=@CrimpBeforeManualNotes, [CrimpBeforeNotes]=@CrimpBeforeNotes, [EngineeringControl]=@EngineeringControl, [EngineeringControlEditDate]=@EngineeringControlEditDate, [EngineeringControlEditUserEmail]=@EngineeringControlEditUserEmail, [EngineeringControlEditUserId]=@EngineeringControlEditUserId, [EngineeringControlEditUserName]=@EngineeringControlEditUserName, [EngineeringDistribution]=@EngineeringDistribution, [EngineeringDistributionEditDate]=@EngineeringDistributionEditDate, [EngineeringDistributionEditUserEmail]=@EngineeringDistributionEditUserEmail, [EngineeringDistributionEditUserId]=@EngineeringDistributionEditUserId, [EngineeringDistributionEditUserName]=@EngineeringDistributionEditUserName, [EngineeringPrint]=@EngineeringPrint, [EngineeringPrintEditDate]=@EngineeringPrintEditDate, [EngineeringPrintEditUserEmail]=@EngineeringPrintEditUserEmail, [EngineeringPrintEditUserId]=@EngineeringPrintEditUserId, [EngineeringPrintEditUserName]=@EngineeringPrintEditUserName, [EngineeringUpdate]=@EngineeringUpdate, [EngineeringUpdateEditDate]=@EngineeringUpdateEditDate, [EngineeringUpdateEditUserEmail]=@EngineeringUpdateEditUserEmail, [EngineeringUpdateEditUserId]=@EngineeringUpdateEditUserId, [EngineeringUpdateEditUserName]=@EngineeringUpdateEditUserName, [EngineeringValidationFull]=@EngineeringValidationFull, [Finition]=@Finition, [FinitionNotes]=@FinitionNotes, [InjectionOnCables]=@InjectionOnCables, [InjectionOnCablesNotes]=@InjectionOnCablesNotes, [InjectionPlastic]=@InjectionPlastic, [InjectionPlasticNotes]=@InjectionPlasticNotes, [IsPartialDocumentation]=@IsPartialDocumentation, [LabelingPlan]=@LabelingPlan, [LabelingPlanNotes]=@LabelingPlanNotes, [Packaging]=@Packaging, [PackagingNotes]=@PackagingNotes, [PendingCpValidation]=@PendingCpValidation, [Pressing]=@Pressing, [PressingNotes]=@PressingNotes, [QualityControl]=@QualityControl, [QualityControlEditDate]=@QualityControlEditDate, [QualityControlEditUserEmail]=@QualityControlEditUserEmail, [QualityControlEditUserId]=@QualityControlEditUserId, [QualityControlEditUserName]=@QualityControlEditUserName, [QualityDistribution]=@QualityDistribution, [QualityDistributionEditDate]=@QualityDistributionEditDate, [QualityDistributionEditUserEmail]=@QualityDistributionEditUserEmail, [QualityDistributionEditUserId]=@QualityDistributionEditUserId, [QualityDistributionEditUserName]=@QualityDistributionEditUserName, [QualityPrint]=@QualityPrint, [QualityPrintEditDate]=@QualityPrintEditDate, [QualityPrintEditUserEmail]=@QualityPrintEditUserEmail, [QualityPrintEditUserId]=@QualityPrintEditUserId, [QualityPrintEditUserName]=@QualityPrintEditUserName, [QualityUpdate]=@QualityUpdate, [QualityUpdateEditDate]=@QualityUpdateEditDate, [QualityUpdateEditUserEmail]=@QualityUpdateEditUserEmail, [QualityUpdateEditUserId]=@QualityUpdateEditUserId, [QualityUpdateEditUserName]=@QualityUpdateEditUserName, [QualityValidationFull]=@QualityValidationFull, [Readiness]=@Readiness, [ReadinessNotes]=@ReadinessNotes, [Translation]=@Translation, [TranslationNotes]=@TranslationNotes, [Twisting]=@Twisting, [TwistingNotes]=@TwistingNotes, [Ultrason]=@Ultrason, [UltrasonNotes]=@UltrasonNotes, [Validation]=@Validation, [ValidationFull]=@ValidationFull, [ValidationNotes]=@ValidationNotes, [Welding]=@Welding, [WeldingNotes]=@WeldingNotes WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("ArticleBOMVersion", item.ArticleBOMVersion);
			sqlCommand.Parameters.AddWithValue("ArticleCPVersion", item.ArticleCPVersion == null ? (object)DBNull.Value : item.ArticleCPVersion);
			sqlCommand.Parameters.AddWithValue("ArticleKundenIndex", item.ArticleKundenIndex == null ? (object)DBNull.Value : item.ArticleKundenIndex);
			sqlCommand.Parameters.AddWithValue("ArticleKundenIndexDatum", item.ArticleKundenIndexDatum == null ? (object)DBNull.Value : item.ArticleKundenIndexDatum);
			sqlCommand.Parameters.AddWithValue("ArticleNr", item.ArticleNr);
			sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber);
			sqlCommand.Parameters.AddWithValue("Assembling1", item.Assembling1 == null ? (object)DBNull.Value : item.Assembling1);
			sqlCommand.Parameters.AddWithValue("Assembling1Notes", item.Assembling1Notes == null ? (object)DBNull.Value : item.Assembling1Notes);
			sqlCommand.Parameters.AddWithValue("Assembling2", item.Assembling2 == null ? (object)DBNull.Value : item.Assembling2);
			sqlCommand.Parameters.AddWithValue("Assembling2Notes", item.Assembling2Notes == null ? (object)DBNull.Value : item.Assembling2Notes);
			sqlCommand.Parameters.AddWithValue("Assembling3", item.Assembling3 == null ? (object)DBNull.Value : item.Assembling3);
			sqlCommand.Parameters.AddWithValue("Assembling3Notes", item.Assembling3Notes == null ? (object)DBNull.Value : item.Assembling3Notes);
			sqlCommand.Parameters.AddWithValue("BOMValidationDate", item.BOMValidationDate);
			sqlCommand.Parameters.AddWithValue("Commission", item.Commission == null ? (object)DBNull.Value : item.Commission);
			sqlCommand.Parameters.AddWithValue("CommissionNotes", item.CommissionNotes == null ? (object)DBNull.Value : item.CommissionNotes);
			sqlCommand.Parameters.AddWithValue("ControleElectrical", item.ControleElectrical == null ? (object)DBNull.Value : item.ControleElectrical);
			sqlCommand.Parameters.AddWithValue("ControleElectricalNotes", item.ControleElectricalNotes == null ? (object)DBNull.Value : item.ControleElectricalNotes);
			sqlCommand.Parameters.AddWithValue("ControleVisual", item.ControleVisual == null ? (object)DBNull.Value : item.ControleVisual);
			sqlCommand.Parameters.AddWithValue("ControleVisualNotes", item.ControleVisualNotes == null ? (object)DBNull.Value : item.ControleVisualNotes);
			sqlCommand.Parameters.AddWithValue("CPValidationDate", item.CPValidationDate == null ? (object)DBNull.Value : item.CPValidationDate);
			sqlCommand.Parameters.AddWithValue("CrimpAfter", item.CrimpAfter == null ? (object)DBNull.Value : item.CrimpAfter);
			sqlCommand.Parameters.AddWithValue("CrimpAfterManual", item.CrimpAfterManual == null ? (object)DBNull.Value : item.CrimpAfterManual);
			sqlCommand.Parameters.AddWithValue("CrimpAfterManualNotes", item.CrimpAfterManualNotes == null ? (object)DBNull.Value : item.CrimpAfterManualNotes);
			sqlCommand.Parameters.AddWithValue("CrimpAfterNotes", item.CrimpAfterNotes == null ? (object)DBNull.Value : item.CrimpAfterNotes);
			sqlCommand.Parameters.AddWithValue("CrimpBefore", item.CrimpBefore == null ? (object)DBNull.Value : item.CrimpBefore);
			sqlCommand.Parameters.AddWithValue("CrimpBeforeManual", item.CrimpBeforeManual == null ? (object)DBNull.Value : item.CrimpBeforeManual);
			sqlCommand.Parameters.AddWithValue("CrimpBeforeManualNotes", item.CrimpBeforeManualNotes == null ? (object)DBNull.Value : item.CrimpBeforeManualNotes);
			sqlCommand.Parameters.AddWithValue("CrimpBeforeNotes", item.CrimpBeforeNotes == null ? (object)DBNull.Value : item.CrimpBeforeNotes);
			sqlCommand.Parameters.AddWithValue("EngineeringControl", item.EngineeringControl);
			sqlCommand.Parameters.AddWithValue("EngineeringControlEditDate", item.EngineeringControlEditDate == null ? (object)DBNull.Value : item.EngineeringControlEditDate);
			sqlCommand.Parameters.AddWithValue("EngineeringControlEditUserEmail", item.EngineeringControlEditUserEmail == null ? (object)DBNull.Value : item.EngineeringControlEditUserEmail);
			sqlCommand.Parameters.AddWithValue("EngineeringControlEditUserId", item.EngineeringControlEditUserId == null ? (object)DBNull.Value : item.EngineeringControlEditUserId);
			sqlCommand.Parameters.AddWithValue("EngineeringControlEditUserName", item.EngineeringControlEditUserName == null ? (object)DBNull.Value : item.EngineeringControlEditUserName);
			sqlCommand.Parameters.AddWithValue("EngineeringDistribution", item.EngineeringDistribution);
			sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditDate", item.EngineeringDistributionEditDate == null ? (object)DBNull.Value : item.EngineeringDistributionEditDate);
			sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditUserEmail", item.EngineeringDistributionEditUserEmail == null ? (object)DBNull.Value : item.EngineeringDistributionEditUserEmail);
			sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditUserId", item.EngineeringDistributionEditUserId == null ? (object)DBNull.Value : item.EngineeringDistributionEditUserId);
			sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditUserName", item.EngineeringDistributionEditUserName == null ? (object)DBNull.Value : item.EngineeringDistributionEditUserName);
			sqlCommand.Parameters.AddWithValue("EngineeringPrint", item.EngineeringPrint);
			sqlCommand.Parameters.AddWithValue("EngineeringPrintEditDate", item.EngineeringPrintEditDate == null ? (object)DBNull.Value : item.EngineeringPrintEditDate);
			sqlCommand.Parameters.AddWithValue("EngineeringPrintEditUserEmail", item.EngineeringPrintEditUserEmail == null ? (object)DBNull.Value : item.EngineeringPrintEditUserEmail);
			sqlCommand.Parameters.AddWithValue("EngineeringPrintEditUserId", item.EngineeringPrintEditUserId == null ? (object)DBNull.Value : item.EngineeringPrintEditUserId);
			sqlCommand.Parameters.AddWithValue("EngineeringPrintEditUserName", item.EngineeringPrintEditUserName == null ? (object)DBNull.Value : item.EngineeringPrintEditUserName);
			sqlCommand.Parameters.AddWithValue("EngineeringUpdate", item.EngineeringUpdate);
			sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditDate", item.EngineeringUpdateEditDate == null ? (object)DBNull.Value : item.EngineeringUpdateEditDate);
			sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditUserEmail", item.EngineeringUpdateEditUserEmail == null ? (object)DBNull.Value : item.EngineeringUpdateEditUserEmail);
			sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditUserId", item.EngineeringUpdateEditUserId == null ? (object)DBNull.Value : item.EngineeringUpdateEditUserId);
			sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditUserName", item.EngineeringUpdateEditUserName == null ? (object)DBNull.Value : item.EngineeringUpdateEditUserName);
			sqlCommand.Parameters.AddWithValue("EngineeringValidationFull", item.EngineeringValidationFull);
			sqlCommand.Parameters.AddWithValue("Finition", item.Finition == null ? (object)DBNull.Value : item.Finition);
			sqlCommand.Parameters.AddWithValue("FinitionNotes", item.FinitionNotes == null ? (object)DBNull.Value : item.FinitionNotes);
			sqlCommand.Parameters.AddWithValue("InjectionOnCables", item.InjectionOnCables == null ? (object)DBNull.Value : item.InjectionOnCables);
			sqlCommand.Parameters.AddWithValue("InjectionOnCablesNotes", item.InjectionOnCablesNotes == null ? (object)DBNull.Value : item.InjectionOnCablesNotes);
			sqlCommand.Parameters.AddWithValue("InjectionPlastic", item.InjectionPlastic == null ? (object)DBNull.Value : item.InjectionPlastic);
			sqlCommand.Parameters.AddWithValue("InjectionPlasticNotes", item.InjectionPlasticNotes == null ? (object)DBNull.Value : item.InjectionPlasticNotes);
			sqlCommand.Parameters.AddWithValue("IsPartialDocumentation", item.IsPartialDocumentation == null ? (object)DBNull.Value : item.IsPartialDocumentation);
			sqlCommand.Parameters.AddWithValue("LabelingPlan", item.LabelingPlan == null ? (object)DBNull.Value : item.LabelingPlan);
			sqlCommand.Parameters.AddWithValue("LabelingPlanNotes", item.LabelingPlanNotes == null ? (object)DBNull.Value : item.LabelingPlanNotes);
			sqlCommand.Parameters.AddWithValue("Packaging", item.Packaging == null ? (object)DBNull.Value : item.Packaging);
			sqlCommand.Parameters.AddWithValue("PackagingNotes", item.PackagingNotes == null ? (object)DBNull.Value : item.PackagingNotes);
			sqlCommand.Parameters.AddWithValue("PendingCpValidation", item.PendingCpValidation);
			sqlCommand.Parameters.AddWithValue("Pressing", item.Pressing == null ? (object)DBNull.Value : item.Pressing);
			sqlCommand.Parameters.AddWithValue("PressingNotes", item.PressingNotes == null ? (object)DBNull.Value : item.PressingNotes);
			sqlCommand.Parameters.AddWithValue("QualityControl", item.QualityControl);
			sqlCommand.Parameters.AddWithValue("QualityControlEditDate", item.QualityControlEditDate == null ? (object)DBNull.Value : item.QualityControlEditDate);
			sqlCommand.Parameters.AddWithValue("QualityControlEditUserEmail", item.QualityControlEditUserEmail == null ? (object)DBNull.Value : item.QualityControlEditUserEmail);
			sqlCommand.Parameters.AddWithValue("QualityControlEditUserId", item.QualityControlEditUserId == null ? (object)DBNull.Value : item.QualityControlEditUserId);
			sqlCommand.Parameters.AddWithValue("QualityControlEditUserName", item.QualityControlEditUserName == null ? (object)DBNull.Value : item.QualityControlEditUserName);
			sqlCommand.Parameters.AddWithValue("QualityDistribution", item.QualityDistribution);
			sqlCommand.Parameters.AddWithValue("QualityDistributionEditDate", item.QualityDistributionEditDate == null ? (object)DBNull.Value : item.QualityDistributionEditDate);
			sqlCommand.Parameters.AddWithValue("QualityDistributionEditUserEmail", item.QualityDistributionEditUserEmail == null ? (object)DBNull.Value : item.QualityDistributionEditUserEmail);
			sqlCommand.Parameters.AddWithValue("QualityDistributionEditUserId", item.QualityDistributionEditUserId == null ? (object)DBNull.Value : item.QualityDistributionEditUserId);
			sqlCommand.Parameters.AddWithValue("QualityDistributionEditUserName", item.QualityDistributionEditUserName == null ? (object)DBNull.Value : item.QualityDistributionEditUserName);
			sqlCommand.Parameters.AddWithValue("QualityPrint", item.QualityPrint);
			sqlCommand.Parameters.AddWithValue("QualityPrintEditDate", item.QualityPrintEditDate == null ? (object)DBNull.Value : item.QualityPrintEditDate);
			sqlCommand.Parameters.AddWithValue("QualityPrintEditUserEmail", item.QualityPrintEditUserEmail == null ? (object)DBNull.Value : item.QualityPrintEditUserEmail);
			sqlCommand.Parameters.AddWithValue("QualityPrintEditUserId", item.QualityPrintEditUserId == null ? (object)DBNull.Value : item.QualityPrintEditUserId);
			sqlCommand.Parameters.AddWithValue("QualityPrintEditUserName", item.QualityPrintEditUserName == null ? (object)DBNull.Value : item.QualityPrintEditUserName);
			sqlCommand.Parameters.AddWithValue("QualityUpdate", item.QualityUpdate);
			sqlCommand.Parameters.AddWithValue("QualityUpdateEditDate", item.QualityUpdateEditDate == null ? (object)DBNull.Value : item.QualityUpdateEditDate);
			sqlCommand.Parameters.AddWithValue("QualityUpdateEditUserEmail", item.QualityUpdateEditUserEmail == null ? (object)DBNull.Value : item.QualityUpdateEditUserEmail);
			sqlCommand.Parameters.AddWithValue("QualityUpdateEditUserId", item.QualityUpdateEditUserId == null ? (object)DBNull.Value : item.QualityUpdateEditUserId);
			sqlCommand.Parameters.AddWithValue("QualityUpdateEditUserName", item.QualityUpdateEditUserName == null ? (object)DBNull.Value : item.QualityUpdateEditUserName);
			sqlCommand.Parameters.AddWithValue("QualityValidationFull", item.QualityValidationFull);
			sqlCommand.Parameters.AddWithValue("Readiness", item.Readiness == null ? (object)DBNull.Value : item.Readiness);
			sqlCommand.Parameters.AddWithValue("ReadinessNotes", item.ReadinessNotes == null ? (object)DBNull.Value : item.ReadinessNotes);
			sqlCommand.Parameters.AddWithValue("Translation", item.Translation == null ? (object)DBNull.Value : item.Translation);
			sqlCommand.Parameters.AddWithValue("TranslationNotes", item.TranslationNotes == null ? (object)DBNull.Value : item.TranslationNotes);
			sqlCommand.Parameters.AddWithValue("Twisting", item.Twisting == null ? (object)DBNull.Value : item.Twisting);
			sqlCommand.Parameters.AddWithValue("TwistingNotes", item.TwistingNotes == null ? (object)DBNull.Value : item.TwistingNotes);
			sqlCommand.Parameters.AddWithValue("Ultrason", item.Ultrason == null ? (object)DBNull.Value : item.Ultrason);
			sqlCommand.Parameters.AddWithValue("UltrasonNotes", item.UltrasonNotes == null ? (object)DBNull.Value : item.UltrasonNotes);
			sqlCommand.Parameters.AddWithValue("Validation", item.Validation == null ? (object)DBNull.Value : item.Validation);
			sqlCommand.Parameters.AddWithValue("ValidationFull", item.ValidationFull);
			sqlCommand.Parameters.AddWithValue("ValidationNotes", item.ValidationNotes == null ? (object)DBNull.Value : item.ValidationNotes);
			sqlCommand.Parameters.AddWithValue("Welding", item.Welding == null ? (object)DBNull.Value : item.Welding);
			sqlCommand.Parameters.AddWithValue("WeldingNotes", item.WeldingNotes == null ? (object)DBNull.Value : item.WeldingNotes);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 99; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__BSD_StucklistenArticle_VersionValidation] SET "

					+ "[ArticleBOMVersion]=@ArticleBOMVersion" + i + ","
					+ "[ArticleCPVersion]=@ArticleCPVersion" + i + ","
					+ "[ArticleKundenIndex]=@ArticleKundenIndex" + i + ","
					+ "[ArticleKundenIndexDatum]=@ArticleKundenIndexDatum" + i + ","
					+ "[ArticleNr]=@ArticleNr" + i + ","
					+ "[ArticleNumber]=@ArticleNumber" + i + ","
					+ "[Assembling1]=@Assembling1" + i + ","
					+ "[Assembling1Notes]=@Assembling1Notes" + i + ","
					+ "[Assembling2]=@Assembling2" + i + ","
					+ "[Assembling2Notes]=@Assembling2Notes" + i + ","
					+ "[Assembling3]=@Assembling3" + i + ","
					+ "[Assembling3Notes]=@Assembling3Notes" + i + ","
					+ "[BOMValidationDate]=@BOMValidationDate" + i + ","
					+ "[Commission]=@Commission" + i + ","
					+ "[CommissionNotes]=@CommissionNotes" + i + ","
					+ "[ControleElectrical]=@ControleElectrical" + i + ","
					+ "[ControleElectricalNotes]=@ControleElectricalNotes" + i + ","
					+ "[ControleVisual]=@ControleVisual" + i + ","
					+ "[ControleVisualNotes]=@ControleVisualNotes" + i + ","
					+ "[CPValidationDate]=@CPValidationDate" + i + ","
					+ "[CrimpAfter]=@CrimpAfter" + i + ","
					+ "[CrimpAfterManual]=@CrimpAfterManual" + i + ","
					+ "[CrimpAfterManualNotes]=@CrimpAfterManualNotes" + i + ","
					+ "[CrimpAfterNotes]=@CrimpAfterNotes" + i + ","
					+ "[CrimpBefore]=@CrimpBefore" + i + ","
					+ "[CrimpBeforeManual]=@CrimpBeforeManual" + i + ","
					+ "[CrimpBeforeManualNotes]=@CrimpBeforeManualNotes" + i + ","
					+ "[CrimpBeforeNotes]=@CrimpBeforeNotes" + i + ","
					+ "[EngineeringControl]=@EngineeringControl" + i + ","
					+ "[EngineeringControlEditDate]=@EngineeringControlEditDate" + i + ","
					+ "[EngineeringControlEditUserEmail]=@EngineeringControlEditUserEmail" + i + ","
					+ "[EngineeringControlEditUserId]=@EngineeringControlEditUserId" + i + ","
					+ "[EngineeringControlEditUserName]=@EngineeringControlEditUserName" + i + ","
					+ "[EngineeringDistribution]=@EngineeringDistribution" + i + ","
					+ "[EngineeringDistributionEditDate]=@EngineeringDistributionEditDate" + i + ","
					+ "[EngineeringDistributionEditUserEmail]=@EngineeringDistributionEditUserEmail" + i + ","
					+ "[EngineeringDistributionEditUserId]=@EngineeringDistributionEditUserId" + i + ","
					+ "[EngineeringDistributionEditUserName]=@EngineeringDistributionEditUserName" + i + ","
					+ "[EngineeringPrint]=@EngineeringPrint" + i + ","
					+ "[EngineeringPrintEditDate]=@EngineeringPrintEditDate" + i + ","
					+ "[EngineeringPrintEditUserEmail]=@EngineeringPrintEditUserEmail" + i + ","
					+ "[EngineeringPrintEditUserId]=@EngineeringPrintEditUserId" + i + ","
					+ "[EngineeringPrintEditUserName]=@EngineeringPrintEditUserName" + i + ","
					+ "[EngineeringUpdate]=@EngineeringUpdate" + i + ","
					+ "[EngineeringUpdateEditDate]=@EngineeringUpdateEditDate" + i + ","
					+ "[EngineeringUpdateEditUserEmail]=@EngineeringUpdateEditUserEmail" + i + ","
					+ "[EngineeringUpdateEditUserId]=@EngineeringUpdateEditUserId" + i + ","
					+ "[EngineeringUpdateEditUserName]=@EngineeringUpdateEditUserName" + i + ","
					+ "[EngineeringValidationFull]=@EngineeringValidationFull" + i + ","
					+ "[Finition]=@Finition" + i + ","
					+ "[FinitionNotes]=@FinitionNotes" + i + ","
					+ "[InjectionOnCables]=@InjectionOnCables" + i + ","
					+ "[InjectionOnCablesNotes]=@InjectionOnCablesNotes" + i + ","
					+ "[InjectionPlastic]=@InjectionPlastic" + i + ","
					+ "[InjectionPlasticNotes]=@InjectionPlasticNotes" + i + ","
					+ "[IsPartialDocumentation]=@IsPartialDocumentation" + i + ","
					+ "[LabelingPlan]=@LabelingPlan" + i + ","
					+ "[LabelingPlanNotes]=@LabelingPlanNotes" + i + ","
					+ "[Packaging]=@Packaging" + i + ","
					+ "[PackagingNotes]=@PackagingNotes" + i + ","
					+ "[PendingCpValidation]=@PendingCpValidation" + i + ","
					+ "[Pressing]=@Pressing" + i + ","
					+ "[PressingNotes]=@PressingNotes" + i + ","
					+ "[QualityControl]=@QualityControl" + i + ","
					+ "[QualityControlEditDate]=@QualityControlEditDate" + i + ","
					+ "[QualityControlEditUserEmail]=@QualityControlEditUserEmail" + i + ","
					+ "[QualityControlEditUserId]=@QualityControlEditUserId" + i + ","
					+ "[QualityControlEditUserName]=@QualityControlEditUserName" + i + ","
					+ "[QualityDistribution]=@QualityDistribution" + i + ","
					+ "[QualityDistributionEditDate]=@QualityDistributionEditDate" + i + ","
					+ "[QualityDistributionEditUserEmail]=@QualityDistributionEditUserEmail" + i + ","
					+ "[QualityDistributionEditUserId]=@QualityDistributionEditUserId" + i + ","
					+ "[QualityDistributionEditUserName]=@QualityDistributionEditUserName" + i + ","
					+ "[QualityPrint]=@QualityPrint" + i + ","
					+ "[QualityPrintEditDate]=@QualityPrintEditDate" + i + ","
					+ "[QualityPrintEditUserEmail]=@QualityPrintEditUserEmail" + i + ","
					+ "[QualityPrintEditUserId]=@QualityPrintEditUserId" + i + ","
					+ "[QualityPrintEditUserName]=@QualityPrintEditUserName" + i + ","
					+ "[QualityUpdate]=@QualityUpdate" + i + ","
					+ "[QualityUpdateEditDate]=@QualityUpdateEditDate" + i + ","
					+ "[QualityUpdateEditUserEmail]=@QualityUpdateEditUserEmail" + i + ","
					+ "[QualityUpdateEditUserId]=@QualityUpdateEditUserId" + i + ","
					+ "[QualityUpdateEditUserName]=@QualityUpdateEditUserName" + i + ","
					+ "[QualityValidationFull]=@QualityValidationFull" + i + ","
					+ "[Readiness]=@Readiness" + i + ","
					+ "[ReadinessNotes]=@ReadinessNotes" + i + ","
					+ "[Translation]=@Translation" + i + ","
					+ "[TranslationNotes]=@TranslationNotes" + i + ","
					+ "[Twisting]=@Twisting" + i + ","
					+ "[TwistingNotes]=@TwistingNotes" + i + ","
					+ "[Ultrason]=@Ultrason" + i + ","
					+ "[UltrasonNotes]=@UltrasonNotes" + i + ","
					+ "[Validation]=@Validation" + i + ","
					+ "[ValidationFull]=@ValidationFull" + i + ","
					+ "[ValidationNotes]=@ValidationNotes" + i + ","
					+ "[Welding]=@Welding" + i + ","
					+ "[WeldingNotes]=@WeldingNotes" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("ArticleBOMVersion" + i, item.ArticleBOMVersion);
					sqlCommand.Parameters.AddWithValue("ArticleCPVersion" + i, item.ArticleCPVersion == null ? (object)DBNull.Value : item.ArticleCPVersion);
					sqlCommand.Parameters.AddWithValue("ArticleKundenIndex" + i, item.ArticleKundenIndex == null ? (object)DBNull.Value : item.ArticleKundenIndex);
					sqlCommand.Parameters.AddWithValue("ArticleKundenIndexDatum" + i, item.ArticleKundenIndexDatum == null ? (object)DBNull.Value : item.ArticleKundenIndexDatum);
					sqlCommand.Parameters.AddWithValue("ArticleNr" + i, item.ArticleNr);
					sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("Assembling1" + i, item.Assembling1 == null ? (object)DBNull.Value : item.Assembling1);
					sqlCommand.Parameters.AddWithValue("Assembling1Notes" + i, item.Assembling1Notes == null ? (object)DBNull.Value : item.Assembling1Notes);
					sqlCommand.Parameters.AddWithValue("Assembling2" + i, item.Assembling2 == null ? (object)DBNull.Value : item.Assembling2);
					sqlCommand.Parameters.AddWithValue("Assembling2Notes" + i, item.Assembling2Notes == null ? (object)DBNull.Value : item.Assembling2Notes);
					sqlCommand.Parameters.AddWithValue("Assembling3" + i, item.Assembling3 == null ? (object)DBNull.Value : item.Assembling3);
					sqlCommand.Parameters.AddWithValue("Assembling3Notes" + i, item.Assembling3Notes == null ? (object)DBNull.Value : item.Assembling3Notes);
					sqlCommand.Parameters.AddWithValue("BOMValidationDate" + i, item.BOMValidationDate);
					sqlCommand.Parameters.AddWithValue("Commission" + i, item.Commission == null ? (object)DBNull.Value : item.Commission);
					sqlCommand.Parameters.AddWithValue("CommissionNotes" + i, item.CommissionNotes == null ? (object)DBNull.Value : item.CommissionNotes);
					sqlCommand.Parameters.AddWithValue("ControleElectrical" + i, item.ControleElectrical == null ? (object)DBNull.Value : item.ControleElectrical);
					sqlCommand.Parameters.AddWithValue("ControleElectricalNotes" + i, item.ControleElectricalNotes == null ? (object)DBNull.Value : item.ControleElectricalNotes);
					sqlCommand.Parameters.AddWithValue("ControleVisual" + i, item.ControleVisual == null ? (object)DBNull.Value : item.ControleVisual);
					sqlCommand.Parameters.AddWithValue("ControleVisualNotes" + i, item.ControleVisualNotes == null ? (object)DBNull.Value : item.ControleVisualNotes);
					sqlCommand.Parameters.AddWithValue("CPValidationDate" + i, item.CPValidationDate == null ? (object)DBNull.Value : item.CPValidationDate);
					sqlCommand.Parameters.AddWithValue("CrimpAfter" + i, item.CrimpAfter == null ? (object)DBNull.Value : item.CrimpAfter);
					sqlCommand.Parameters.AddWithValue("CrimpAfterManual" + i, item.CrimpAfterManual == null ? (object)DBNull.Value : item.CrimpAfterManual);
					sqlCommand.Parameters.AddWithValue("CrimpAfterManualNotes" + i, item.CrimpAfterManualNotes == null ? (object)DBNull.Value : item.CrimpAfterManualNotes);
					sqlCommand.Parameters.AddWithValue("CrimpAfterNotes" + i, item.CrimpAfterNotes == null ? (object)DBNull.Value : item.CrimpAfterNotes);
					sqlCommand.Parameters.AddWithValue("CrimpBefore" + i, item.CrimpBefore == null ? (object)DBNull.Value : item.CrimpBefore);
					sqlCommand.Parameters.AddWithValue("CrimpBeforeManual" + i, item.CrimpBeforeManual == null ? (object)DBNull.Value : item.CrimpBeforeManual);
					sqlCommand.Parameters.AddWithValue("CrimpBeforeManualNotes" + i, item.CrimpBeforeManualNotes == null ? (object)DBNull.Value : item.CrimpBeforeManualNotes);
					sqlCommand.Parameters.AddWithValue("CrimpBeforeNotes" + i, item.CrimpBeforeNotes == null ? (object)DBNull.Value : item.CrimpBeforeNotes);
					sqlCommand.Parameters.AddWithValue("EngineeringControl" + i, item.EngineeringControl);
					sqlCommand.Parameters.AddWithValue("EngineeringControlEditDate" + i, item.EngineeringControlEditDate == null ? (object)DBNull.Value : item.EngineeringControlEditDate);
					sqlCommand.Parameters.AddWithValue("EngineeringControlEditUserEmail" + i, item.EngineeringControlEditUserEmail == null ? (object)DBNull.Value : item.EngineeringControlEditUserEmail);
					sqlCommand.Parameters.AddWithValue("EngineeringControlEditUserId" + i, item.EngineeringControlEditUserId == null ? (object)DBNull.Value : item.EngineeringControlEditUserId);
					sqlCommand.Parameters.AddWithValue("EngineeringControlEditUserName" + i, item.EngineeringControlEditUserName == null ? (object)DBNull.Value : item.EngineeringControlEditUserName);
					sqlCommand.Parameters.AddWithValue("EngineeringDistribution" + i, item.EngineeringDistribution);
					sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditDate" + i, item.EngineeringDistributionEditDate == null ? (object)DBNull.Value : item.EngineeringDistributionEditDate);
					sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditUserEmail" + i, item.EngineeringDistributionEditUserEmail == null ? (object)DBNull.Value : item.EngineeringDistributionEditUserEmail);
					sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditUserId" + i, item.EngineeringDistributionEditUserId == null ? (object)DBNull.Value : item.EngineeringDistributionEditUserId);
					sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditUserName" + i, item.EngineeringDistributionEditUserName == null ? (object)DBNull.Value : item.EngineeringDistributionEditUserName);
					sqlCommand.Parameters.AddWithValue("EngineeringPrint" + i, item.EngineeringPrint);
					sqlCommand.Parameters.AddWithValue("EngineeringPrintEditDate" + i, item.EngineeringPrintEditDate == null ? (object)DBNull.Value : item.EngineeringPrintEditDate);
					sqlCommand.Parameters.AddWithValue("EngineeringPrintEditUserEmail" + i, item.EngineeringPrintEditUserEmail == null ? (object)DBNull.Value : item.EngineeringPrintEditUserEmail);
					sqlCommand.Parameters.AddWithValue("EngineeringPrintEditUserId" + i, item.EngineeringPrintEditUserId == null ? (object)DBNull.Value : item.EngineeringPrintEditUserId);
					sqlCommand.Parameters.AddWithValue("EngineeringPrintEditUserName" + i, item.EngineeringPrintEditUserName == null ? (object)DBNull.Value : item.EngineeringPrintEditUserName);
					sqlCommand.Parameters.AddWithValue("EngineeringUpdate" + i, item.EngineeringUpdate);
					sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditDate" + i, item.EngineeringUpdateEditDate == null ? (object)DBNull.Value : item.EngineeringUpdateEditDate);
					sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditUserEmail" + i, item.EngineeringUpdateEditUserEmail == null ? (object)DBNull.Value : item.EngineeringUpdateEditUserEmail);
					sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditUserId" + i, item.EngineeringUpdateEditUserId == null ? (object)DBNull.Value : item.EngineeringUpdateEditUserId);
					sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditUserName" + i, item.EngineeringUpdateEditUserName == null ? (object)DBNull.Value : item.EngineeringUpdateEditUserName);
					sqlCommand.Parameters.AddWithValue("EngineeringValidationFull" + i, item.EngineeringValidationFull);
					sqlCommand.Parameters.AddWithValue("Finition" + i, item.Finition == null ? (object)DBNull.Value : item.Finition);
					sqlCommand.Parameters.AddWithValue("FinitionNotes" + i, item.FinitionNotes == null ? (object)DBNull.Value : item.FinitionNotes);
					sqlCommand.Parameters.AddWithValue("InjectionOnCables" + i, item.InjectionOnCables == null ? (object)DBNull.Value : item.InjectionOnCables);
					sqlCommand.Parameters.AddWithValue("InjectionOnCablesNotes" + i, item.InjectionOnCablesNotes == null ? (object)DBNull.Value : item.InjectionOnCablesNotes);
					sqlCommand.Parameters.AddWithValue("InjectionPlastic" + i, item.InjectionPlastic == null ? (object)DBNull.Value : item.InjectionPlastic);
					sqlCommand.Parameters.AddWithValue("InjectionPlasticNotes" + i, item.InjectionPlasticNotes == null ? (object)DBNull.Value : item.InjectionPlasticNotes);
					sqlCommand.Parameters.AddWithValue("IsPartialDocumentation" + i, item.IsPartialDocumentation == null ? (object)DBNull.Value : item.IsPartialDocumentation);
					sqlCommand.Parameters.AddWithValue("LabelingPlan" + i, item.LabelingPlan == null ? (object)DBNull.Value : item.LabelingPlan);
					sqlCommand.Parameters.AddWithValue("LabelingPlanNotes" + i, item.LabelingPlanNotes == null ? (object)DBNull.Value : item.LabelingPlanNotes);
					sqlCommand.Parameters.AddWithValue("Packaging" + i, item.Packaging == null ? (object)DBNull.Value : item.Packaging);
					sqlCommand.Parameters.AddWithValue("PackagingNotes" + i, item.PackagingNotes == null ? (object)DBNull.Value : item.PackagingNotes);
					sqlCommand.Parameters.AddWithValue("PendingCpValidation" + i, item.PendingCpValidation);
					sqlCommand.Parameters.AddWithValue("Pressing" + i, item.Pressing == null ? (object)DBNull.Value : item.Pressing);
					sqlCommand.Parameters.AddWithValue("PressingNotes" + i, item.PressingNotes == null ? (object)DBNull.Value : item.PressingNotes);
					sqlCommand.Parameters.AddWithValue("QualityControl" + i, item.QualityControl);
					sqlCommand.Parameters.AddWithValue("QualityControlEditDate" + i, item.QualityControlEditDate == null ? (object)DBNull.Value : item.QualityControlEditDate);
					sqlCommand.Parameters.AddWithValue("QualityControlEditUserEmail" + i, item.QualityControlEditUserEmail == null ? (object)DBNull.Value : item.QualityControlEditUserEmail);
					sqlCommand.Parameters.AddWithValue("QualityControlEditUserId" + i, item.QualityControlEditUserId == null ? (object)DBNull.Value : item.QualityControlEditUserId);
					sqlCommand.Parameters.AddWithValue("QualityControlEditUserName" + i, item.QualityControlEditUserName == null ? (object)DBNull.Value : item.QualityControlEditUserName);
					sqlCommand.Parameters.AddWithValue("QualityDistribution" + i, item.QualityDistribution);
					sqlCommand.Parameters.AddWithValue("QualityDistributionEditDate" + i, item.QualityDistributionEditDate == null ? (object)DBNull.Value : item.QualityDistributionEditDate);
					sqlCommand.Parameters.AddWithValue("QualityDistributionEditUserEmail" + i, item.QualityDistributionEditUserEmail == null ? (object)DBNull.Value : item.QualityDistributionEditUserEmail);
					sqlCommand.Parameters.AddWithValue("QualityDistributionEditUserId" + i, item.QualityDistributionEditUserId == null ? (object)DBNull.Value : item.QualityDistributionEditUserId);
					sqlCommand.Parameters.AddWithValue("QualityDistributionEditUserName" + i, item.QualityDistributionEditUserName == null ? (object)DBNull.Value : item.QualityDistributionEditUserName);
					sqlCommand.Parameters.AddWithValue("QualityPrint" + i, item.QualityPrint);
					sqlCommand.Parameters.AddWithValue("QualityPrintEditDate" + i, item.QualityPrintEditDate == null ? (object)DBNull.Value : item.QualityPrintEditDate);
					sqlCommand.Parameters.AddWithValue("QualityPrintEditUserEmail" + i, item.QualityPrintEditUserEmail == null ? (object)DBNull.Value : item.QualityPrintEditUserEmail);
					sqlCommand.Parameters.AddWithValue("QualityPrintEditUserId" + i, item.QualityPrintEditUserId == null ? (object)DBNull.Value : item.QualityPrintEditUserId);
					sqlCommand.Parameters.AddWithValue("QualityPrintEditUserName" + i, item.QualityPrintEditUserName == null ? (object)DBNull.Value : item.QualityPrintEditUserName);
					sqlCommand.Parameters.AddWithValue("QualityUpdate" + i, item.QualityUpdate);
					sqlCommand.Parameters.AddWithValue("QualityUpdateEditDate" + i, item.QualityUpdateEditDate == null ? (object)DBNull.Value : item.QualityUpdateEditDate);
					sqlCommand.Parameters.AddWithValue("QualityUpdateEditUserEmail" + i, item.QualityUpdateEditUserEmail == null ? (object)DBNull.Value : item.QualityUpdateEditUserEmail);
					sqlCommand.Parameters.AddWithValue("QualityUpdateEditUserId" + i, item.QualityUpdateEditUserId == null ? (object)DBNull.Value : item.QualityUpdateEditUserId);
					sqlCommand.Parameters.AddWithValue("QualityUpdateEditUserName" + i, item.QualityUpdateEditUserName == null ? (object)DBNull.Value : item.QualityUpdateEditUserName);
					sqlCommand.Parameters.AddWithValue("QualityValidationFull" + i, item.QualityValidationFull);
					sqlCommand.Parameters.AddWithValue("Readiness" + i, item.Readiness == null ? (object)DBNull.Value : item.Readiness);
					sqlCommand.Parameters.AddWithValue("ReadinessNotes" + i, item.ReadinessNotes == null ? (object)DBNull.Value : item.ReadinessNotes);
					sqlCommand.Parameters.AddWithValue("Translation" + i, item.Translation == null ? (object)DBNull.Value : item.Translation);
					sqlCommand.Parameters.AddWithValue("TranslationNotes" + i, item.TranslationNotes == null ? (object)DBNull.Value : item.TranslationNotes);
					sqlCommand.Parameters.AddWithValue("Twisting" + i, item.Twisting == null ? (object)DBNull.Value : item.Twisting);
					sqlCommand.Parameters.AddWithValue("TwistingNotes" + i, item.TwistingNotes == null ? (object)DBNull.Value : item.TwistingNotes);
					sqlCommand.Parameters.AddWithValue("Ultrason" + i, item.Ultrason == null ? (object)DBNull.Value : item.Ultrason);
					sqlCommand.Parameters.AddWithValue("UltrasonNotes" + i, item.UltrasonNotes == null ? (object)DBNull.Value : item.UltrasonNotes);
					sqlCommand.Parameters.AddWithValue("Validation" + i, item.Validation == null ? (object)DBNull.Value : item.Validation);
					sqlCommand.Parameters.AddWithValue("ValidationFull" + i, item.ValidationFull);
					sqlCommand.Parameters.AddWithValue("ValidationNotes" + i, item.ValidationNotes == null ? (object)DBNull.Value : item.ValidationNotes);
					sqlCommand.Parameters.AddWithValue("Welding" + i, item.Welding == null ? (object)DBNull.Value : item.Welding);
					sqlCommand.Parameters.AddWithValue("WeldingNotes" + i, item.WeldingNotes == null ? (object)DBNull.Value : item.WeldingNotes);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__BSD_StucklistenArticle_VersionValidation] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			results = DbExecution.ExecuteNonQuery(sqlCommand);


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

				string query = "DELETE FROM [__BSD_StucklistenArticle_VersionValidation] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity> Get(bool? engValidation, bool? qualValidation, bool fullValidation = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_StucklistenArticle_VersionValidation] WHERE [ValidationFull]=@fullValidation "
					+ $" {(engValidation.HasValue ? $"AND [EngineeringValidationFull]={(engValidation.Value ? 1 : 0)}" : "")}"
					+ $" {(qualValidation.HasValue ? $" AND [QualityValidationFull]={(qualValidation.Value ? 1 : 0)}" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("fullValidation", fullValidation);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity> GetForEngineering(bool? includePendingCPValidation = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_StucklistenArticle_VersionValidation] WHERE [ValidationFull]=0 "
					+ $" AND ([QualityControl]=0 AND [QualityDistribution]=0 AND [QualityPrint]=0 AND [QualityUpdate]=0 AND [QualityValidationFull]=0){(includePendingCPValidation.HasValue == false ? "" : $" AND [PendingCpValidation]={(includePendingCPValidation.Value ? "1" : "0")}")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity>();
			}
		}
		public static int GetForEngineeringCount(bool? includePendingCPValidation = false)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT COUNT(*) FROM [__BSD_StucklistenArticle_VersionValidation] WHERE [ValidationFull]=0 "
					+ $" AND ([QualityControl]=0 AND [QualityDistribution]=0 AND [QualityPrint]=0 AND [QualityUpdate]=0 AND [QualityValidationFull]=0){(includePendingCPValidation.HasValue == false ? "" : $" AND [PendingCpValidation]={(includePendingCPValidation.Value ? "1" : "0")}")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var v) ? v : 0;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity> GetForQuality(bool? includePendingCPValidation = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__BSD_StucklistenArticle_VersionValidation] WHERE [ValidationFull]=0 AND [EngineeringValidationFull]=1{(includePendingCPValidation.HasValue == false ? "" : $" AND [PendingCpValidation]={(includePendingCPValidation.Value ? "1" : "0")}")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity>();
			}
		}
		public static int GetForQualityCount(bool? includePendingCPValidation = false)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT COUNT(*) FROM [__BSD_StucklistenArticle_VersionValidation] WHERE [ValidationFull]=0 AND [EngineeringValidationFull]=1{(includePendingCPValidation.HasValue == false ? "" : $" AND [PendingCpValidation]={(includePendingCPValidation.Value ? "1" : "0")}")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var v) ? v : 0;
			}
		}
		public static bool CanValidateBom(int articleNr, int bomVersion, int? cpVersion)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT COUNT(*) FROM [__BSD_StucklistenArticle_VersionValidation] WHERE [ValidationFull]<>1 AND [ArticleNr]=@articleNr AND [ArticleBOMVersion]=@bomVersion "
				+ $"AND {(cpVersion.HasValue ? $"[ArticleCPVersion]={cpVersion}" : "[ArticleCPVersion] IS NULL")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				sqlCommand.Parameters.AddWithValue("bomVersion", bomVersion);

				return (int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var v) ? v : -1) == 0;
			}
		}
		public static long Init(Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			// - Init for Article on CP Validation
			string query = "INSERT INTO [__BSD_StucklistenArticle_VersionValidation] ([PendingCpValidation],[ArticleBOMVersion],[ArticleCPVersion],[ArticleKundenIndex],[ArticleKundenIndexDatum],[ArticleNr],[ArticleNumber],[Assembling1],[Assembling1Notes],[Assembling2],[Assembling2Notes],[Assembling3],[Assembling3Notes],[BOMValidationDate],[Commission],[CommissionNotes],[ControleElectrical],[ControleElectricalNotes],[ControleVisual],[ControleVisualNotes],[CPValidationDate],[CrimpAfter],[CrimpAfterManual],[CrimpAfterManualNotes],[CrimpAfterNotes],[CrimpBefore],[CrimpBeforeManual],[CrimpBeforeManualNotes],[CrimpBeforeNotes],[EngineeringControl],[EngineeringControlEditDate],[EngineeringControlEditUserEmail],[EngineeringControlEditUserId],[EngineeringControlEditUserName],[EngineeringDistribution],[EngineeringDistributionEditDate],[EngineeringDistributionEditUserEmail],[EngineeringDistributionEditUserId],[EngineeringDistributionEditUserName],[EngineeringPrint],[EngineeringPrintEditDate],[EngineeringPrintEditUserEmail],[EngineeringPrintEditUserId],[EngineeringPrintEditUserName],[EngineeringUpdate],[EngineeringUpdateEditDate],[EngineeringUpdateEditUserEmail],[EngineeringUpdateEditUserId],[EngineeringUpdateEditUserName],[EngineeringValidationFull],[Finition],[FinitionNotes],[InjectionOnCables],[InjectionOnCablesNotes],[InjectionPlastic],[InjectionPlasticNotes],[IsPartialDocumentation],[LabelingPlan],[LabelingPlanNotes],[Packaging],[PackagingNotes],[Pressing],[PressingNotes],[QualityControl],[QualityControlEditDate],[QualityControlEditUserEmail],[QualityControlEditUserId],[QualityControlEditUserName],[QualityDistribution],[QualityDistributionEditDate],[QualityDistributionEditUserEmail],[QualityDistributionEditUserId],[QualityDistributionEditUserName],[QualityPrint],[QualityPrintEditDate],[QualityPrintEditUserEmail],[QualityPrintEditUserId],[QualityPrintEditUserName],[QualityUpdate],[QualityUpdateEditDate],[QualityUpdateEditUserEmail],[QualityUpdateEditUserId],[QualityUpdateEditUserName],[QualityValidationFull],[Readiness],[ReadinessNotes],[Translation],[TranslationNotes],[Twisting],[TwistingNotes],[Ultrason],[UltrasonNotes],[Validation],[ValidationFull],[ValidationNotes],[Welding],[WeldingNotes]) OUTPUT INSERTED.[Id] VALUES (@PendingCpValidation,@ArticleBOMVersion,@ArticleCPVersion,@ArticleKundenIndex,@ArticleKundenIndexDatum,@ArticleNr,@ArticleNumber,@Assembling1,@Assembling1Notes,@Assembling2,@Assembling2Notes,@Assembling3,@Assembling3Notes,@BOMValidationDate,@Commission,@CommissionNotes,@ControleElectrical,@ControleElectricalNotes,@ControleVisual,@ControleVisualNotes,@CPValidationDate,@CrimpAfter,@CrimpAfterManual,@CrimpAfterManualNotes,@CrimpAfterNotes,@CrimpBefore,@CrimpBeforeManual,@CrimpBeforeManualNotes,@CrimpBeforeNotes,@EngineeringControl,@EngineeringControlEditDate,@EngineeringControlEditUserEmail,@EngineeringControlEditUserId,@EngineeringControlEditUserName,@EngineeringDistribution,@EngineeringDistributionEditDate,@EngineeringDistributionEditUserEmail,@EngineeringDistributionEditUserId,@EngineeringDistributionEditUserName,@EngineeringPrint,@EngineeringPrintEditDate,@EngineeringPrintEditUserEmail,@EngineeringPrintEditUserId,@EngineeringPrintEditUserName,@EngineeringUpdate,@EngineeringUpdateEditDate,@EngineeringUpdateEditUserEmail,@EngineeringUpdateEditUserId,@EngineeringUpdateEditUserName,@EngineeringValidationFull,@Finition,@FinitionNotes,@InjectionOnCables,@InjectionOnCablesNotes,@InjectionPlastic,@InjectionPlasticNotes,@IsPartialDocumentation,@LabelingPlan,@LabelingPlanNotes,@Packaging,@PackagingNotes,@Pressing,@PressingNotes,@QualityControl,@QualityControlEditDate,@QualityControlEditUserEmail,@QualityControlEditUserId,@QualityControlEditUserName,@QualityDistribution,@QualityDistributionEditDate,@QualityDistributionEditUserEmail,@QualityDistributionEditUserId,@QualityDistributionEditUserName,@QualityPrint,@QualityPrintEditDate,@QualityPrintEditUserEmail,@QualityPrintEditUserId,@QualityPrintEditUserName,@QualityUpdate,@QualityUpdateEditDate,@QualityUpdateEditUserEmail,@QualityUpdateEditUserId,@QualityUpdateEditUserName,@QualityValidationFull,@Readiness,@ReadinessNotes,@Translation,@TranslationNotes,@Twisting,@TwistingNotes,@Ultrason,@UltrasonNotes,@Validation,@ValidationFull,@ValidationNotes,@Welding,@WeldingNotes); ";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("PendingCpValidation", item.PendingCpValidation);
			sqlCommand.Parameters.AddWithValue("ArticleBOMVersion", item.ArticleBOMVersion);
			sqlCommand.Parameters.AddWithValue("ArticleCPVersion", item.ArticleCPVersion == null ? (object)DBNull.Value : item.ArticleCPVersion);
			sqlCommand.Parameters.AddWithValue("ArticleKundenIndex", item.ArticleKundenIndex == null ? (object)DBNull.Value : item.ArticleKundenIndex);
			sqlCommand.Parameters.AddWithValue("ArticleKundenIndexDatum", item.ArticleKundenIndexDatum == null ? (object)DBNull.Value : item.ArticleKundenIndexDatum);
			sqlCommand.Parameters.AddWithValue("ArticleNr", item.ArticleNr);
			sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber);
			sqlCommand.Parameters.AddWithValue("Assembling1", item.Assembling1 == null ? (object)DBNull.Value : item.Assembling1);
			sqlCommand.Parameters.AddWithValue("Assembling1Notes", item.Assembling1Notes == null ? (object)DBNull.Value : item.Assembling1Notes);
			sqlCommand.Parameters.AddWithValue("Assembling2", item.Assembling2 == null ? (object)DBNull.Value : item.Assembling2);
			sqlCommand.Parameters.AddWithValue("Assembling2Notes", item.Assembling2Notes == null ? (object)DBNull.Value : item.Assembling2Notes);
			sqlCommand.Parameters.AddWithValue("Assembling3", item.Assembling3 == null ? (object)DBNull.Value : item.Assembling3);
			sqlCommand.Parameters.AddWithValue("Assembling3Notes", item.Assembling3Notes == null ? (object)DBNull.Value : item.Assembling3Notes);
			sqlCommand.Parameters.AddWithValue("BOMValidationDate", item.BOMValidationDate);
			sqlCommand.Parameters.AddWithValue("Commission", item.Commission == null ? (object)DBNull.Value : item.Commission);
			sqlCommand.Parameters.AddWithValue("CommissionNotes", item.CommissionNotes == null ? (object)DBNull.Value : item.CommissionNotes);
			sqlCommand.Parameters.AddWithValue("ControleElectrical", item.ControleElectrical == null ? (object)DBNull.Value : item.ControleElectrical);
			sqlCommand.Parameters.AddWithValue("ControleElectricalNotes", item.ControleElectricalNotes == null ? (object)DBNull.Value : item.ControleElectricalNotes);
			sqlCommand.Parameters.AddWithValue("ControleVisual", item.ControleVisual == null ? (object)DBNull.Value : item.ControleVisual);
			sqlCommand.Parameters.AddWithValue("ControleVisualNotes", item.ControleVisualNotes == null ? (object)DBNull.Value : item.ControleVisualNotes);
			sqlCommand.Parameters.AddWithValue("CPValidationDate", item.CPValidationDate == null ? (object)DBNull.Value : item.CPValidationDate);
			sqlCommand.Parameters.AddWithValue("CrimpAfter", item.CrimpAfter == null ? (object)DBNull.Value : item.CrimpAfter);
			sqlCommand.Parameters.AddWithValue("CrimpAfterManual", item.CrimpAfterManual == null ? (object)DBNull.Value : item.CrimpAfterManual);
			sqlCommand.Parameters.AddWithValue("CrimpAfterManualNotes", item.CrimpAfterManualNotes == null ? (object)DBNull.Value : item.CrimpAfterManualNotes);
			sqlCommand.Parameters.AddWithValue("CrimpAfterNotes", item.CrimpAfterNotes == null ? (object)DBNull.Value : item.CrimpAfterNotes);
			sqlCommand.Parameters.AddWithValue("CrimpBefore", item.CrimpBefore == null ? (object)DBNull.Value : item.CrimpBefore);
			sqlCommand.Parameters.AddWithValue("CrimpBeforeManual", item.CrimpBeforeManual == null ? (object)DBNull.Value : item.CrimpBeforeManual);
			sqlCommand.Parameters.AddWithValue("CrimpBeforeManualNotes", item.CrimpBeforeManualNotes == null ? (object)DBNull.Value : item.CrimpBeforeManualNotes);
			sqlCommand.Parameters.AddWithValue("CrimpBeforeNotes", item.CrimpBeforeNotes == null ? (object)DBNull.Value : item.CrimpBeforeNotes);
			sqlCommand.Parameters.AddWithValue("EngineeringControl", item.EngineeringControl);
			sqlCommand.Parameters.AddWithValue("EngineeringControlEditDate", item.EngineeringControlEditDate == null ? (object)DBNull.Value : item.EngineeringControlEditDate);
			sqlCommand.Parameters.AddWithValue("EngineeringControlEditUserEmail", item.EngineeringControlEditUserEmail == null ? (object)DBNull.Value : item.EngineeringControlEditUserEmail);
			sqlCommand.Parameters.AddWithValue("EngineeringControlEditUserId", item.EngineeringControlEditUserId == null ? (object)DBNull.Value : item.EngineeringControlEditUserId);
			sqlCommand.Parameters.AddWithValue("EngineeringControlEditUserName", item.EngineeringControlEditUserName == null ? (object)DBNull.Value : item.EngineeringControlEditUserName);
			sqlCommand.Parameters.AddWithValue("EngineeringDistribution", item.EngineeringDistribution);
			sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditDate", item.EngineeringDistributionEditDate == null ? (object)DBNull.Value : item.EngineeringDistributionEditDate);
			sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditUserEmail", item.EngineeringDistributionEditUserEmail == null ? (object)DBNull.Value : item.EngineeringDistributionEditUserEmail);
			sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditUserId", item.EngineeringDistributionEditUserId == null ? (object)DBNull.Value : item.EngineeringDistributionEditUserId);
			sqlCommand.Parameters.AddWithValue("EngineeringDistributionEditUserName", item.EngineeringDistributionEditUserName == null ? (object)DBNull.Value : item.EngineeringDistributionEditUserName);
			sqlCommand.Parameters.AddWithValue("EngineeringPrint", item.EngineeringPrint);
			sqlCommand.Parameters.AddWithValue("EngineeringPrintEditDate", item.EngineeringPrintEditDate == null ? (object)DBNull.Value : item.EngineeringPrintEditDate);
			sqlCommand.Parameters.AddWithValue("EngineeringPrintEditUserEmail", item.EngineeringPrintEditUserEmail == null ? (object)DBNull.Value : item.EngineeringPrintEditUserEmail);
			sqlCommand.Parameters.AddWithValue("EngineeringPrintEditUserId", item.EngineeringPrintEditUserId == null ? (object)DBNull.Value : item.EngineeringPrintEditUserId);
			sqlCommand.Parameters.AddWithValue("EngineeringPrintEditUserName", item.EngineeringPrintEditUserName == null ? (object)DBNull.Value : item.EngineeringPrintEditUserName);
			sqlCommand.Parameters.AddWithValue("EngineeringUpdate", item.EngineeringUpdate);
			sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditDate", item.EngineeringUpdateEditDate == null ? (object)DBNull.Value : item.EngineeringUpdateEditDate);
			sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditUserEmail", item.EngineeringUpdateEditUserEmail == null ? (object)DBNull.Value : item.EngineeringUpdateEditUserEmail);
			sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditUserId", item.EngineeringUpdateEditUserId == null ? (object)DBNull.Value : item.EngineeringUpdateEditUserId);
			sqlCommand.Parameters.AddWithValue("EngineeringUpdateEditUserName", item.EngineeringUpdateEditUserName == null ? (object)DBNull.Value : item.EngineeringUpdateEditUserName);
			sqlCommand.Parameters.AddWithValue("EngineeringValidationFull", item.EngineeringValidationFull);
			sqlCommand.Parameters.AddWithValue("Finition", item.Finition == null ? (object)DBNull.Value : item.Finition);
			sqlCommand.Parameters.AddWithValue("FinitionNotes", item.FinitionNotes == null ? (object)DBNull.Value : item.FinitionNotes);
			sqlCommand.Parameters.AddWithValue("InjectionOnCables", item.InjectionOnCables == null ? (object)DBNull.Value : item.InjectionOnCables);
			sqlCommand.Parameters.AddWithValue("InjectionOnCablesNotes", item.InjectionOnCablesNotes == null ? (object)DBNull.Value : item.InjectionOnCablesNotes);
			sqlCommand.Parameters.AddWithValue("InjectionPlastic", item.InjectionPlastic == null ? (object)DBNull.Value : item.InjectionPlastic);
			sqlCommand.Parameters.AddWithValue("InjectionPlasticNotes", item.InjectionPlasticNotes == null ? (object)DBNull.Value : item.InjectionPlasticNotes);
			sqlCommand.Parameters.AddWithValue("IsPartialDocumentation", ((item.Commission ?? false) || (item.Readiness ?? false) || (item.CrimpBeforeManual ?? false) || (item.CrimpBefore ?? false) || (item.Ultrason ?? false) || (item.Twisting ?? false) || (item.InjectionPlastic ?? false) || (item.Welding ?? false) || (item.Assembling1 ?? false) || (item.Assembling2 ?? false) || (item.Assembling3 ?? false) || (item.CrimpAfterManual ?? false) || (item.CrimpAfter ?? false) || (item.InjectionOnCables ?? false) || (item.Pressing ?? false) || (item.LabelingPlan ?? false) || (item.ControleElectrical ?? false) || (item.ControleVisual ?? false) || (item.Finition ?? false) || (item.Packaging ?? false) || (item.Validation ?? false) || (item.Translation ?? false)));
			sqlCommand.Parameters.AddWithValue("LabelingPlan", item.LabelingPlan == null ? (object)DBNull.Value : item.LabelingPlan);
			sqlCommand.Parameters.AddWithValue("LabelingPlanNotes", item.LabelingPlanNotes == null ? (object)DBNull.Value : item.LabelingPlanNotes);
			sqlCommand.Parameters.AddWithValue("Packaging", item.Packaging == null ? (object)DBNull.Value : item.Packaging);
			sqlCommand.Parameters.AddWithValue("PackagingNotes", item.PackagingNotes == null ? (object)DBNull.Value : item.PackagingNotes);
			sqlCommand.Parameters.AddWithValue("Pressing", item.Pressing == null ? (object)DBNull.Value : item.Pressing);
			sqlCommand.Parameters.AddWithValue("PressingNotes", item.PressingNotes == null ? (object)DBNull.Value : item.PressingNotes);
			sqlCommand.Parameters.AddWithValue("QualityControl", item.QualityControl);
			sqlCommand.Parameters.AddWithValue("QualityControlEditDate", item.QualityControlEditDate == null ? (object)DBNull.Value : item.QualityControlEditDate);
			sqlCommand.Parameters.AddWithValue("QualityControlEditUserEmail", item.QualityControlEditUserEmail == null ? (object)DBNull.Value : item.QualityControlEditUserEmail);
			sqlCommand.Parameters.AddWithValue("QualityControlEditUserId", item.QualityControlEditUserId == null ? (object)DBNull.Value : item.QualityControlEditUserId);
			sqlCommand.Parameters.AddWithValue("QualityControlEditUserName", item.QualityControlEditUserName == null ? (object)DBNull.Value : item.QualityControlEditUserName);
			sqlCommand.Parameters.AddWithValue("QualityDistribution", item.QualityDistribution);
			sqlCommand.Parameters.AddWithValue("QualityDistributionEditDate", item.QualityDistributionEditDate == null ? (object)DBNull.Value : item.QualityDistributionEditDate);
			sqlCommand.Parameters.AddWithValue("QualityDistributionEditUserEmail", item.QualityDistributionEditUserEmail == null ? (object)DBNull.Value : item.QualityDistributionEditUserEmail);
			sqlCommand.Parameters.AddWithValue("QualityDistributionEditUserId", item.QualityDistributionEditUserId == null ? (object)DBNull.Value : item.QualityDistributionEditUserId);
			sqlCommand.Parameters.AddWithValue("QualityDistributionEditUserName", item.QualityDistributionEditUserName == null ? (object)DBNull.Value : item.QualityDistributionEditUserName);
			sqlCommand.Parameters.AddWithValue("QualityPrint", item.QualityPrint);
			sqlCommand.Parameters.AddWithValue("QualityPrintEditDate", item.QualityPrintEditDate == null ? (object)DBNull.Value : item.QualityPrintEditDate);
			sqlCommand.Parameters.AddWithValue("QualityPrintEditUserEmail", item.QualityPrintEditUserEmail == null ? (object)DBNull.Value : item.QualityPrintEditUserEmail);
			sqlCommand.Parameters.AddWithValue("QualityPrintEditUserId", item.QualityPrintEditUserId == null ? (object)DBNull.Value : item.QualityPrintEditUserId);
			sqlCommand.Parameters.AddWithValue("QualityPrintEditUserName", item.QualityPrintEditUserName == null ? (object)DBNull.Value : item.QualityPrintEditUserName);
			sqlCommand.Parameters.AddWithValue("QualityUpdate", item.QualityUpdate);
			sqlCommand.Parameters.AddWithValue("QualityUpdateEditDate", item.QualityUpdateEditDate == null ? (object)DBNull.Value : item.QualityUpdateEditDate);
			sqlCommand.Parameters.AddWithValue("QualityUpdateEditUserEmail", item.QualityUpdateEditUserEmail == null ? (object)DBNull.Value : item.QualityUpdateEditUserEmail);
			sqlCommand.Parameters.AddWithValue("QualityUpdateEditUserId", item.QualityUpdateEditUserId == null ? (object)DBNull.Value : item.QualityUpdateEditUserId);
			sqlCommand.Parameters.AddWithValue("QualityUpdateEditUserName", item.QualityUpdateEditUserName == null ? (object)DBNull.Value : item.QualityUpdateEditUserName);
			sqlCommand.Parameters.AddWithValue("QualityValidationFull", item.QualityValidationFull);
			sqlCommand.Parameters.AddWithValue("Readiness", item.Readiness == null ? (object)DBNull.Value : item.Readiness);
			sqlCommand.Parameters.AddWithValue("ReadinessNotes", item.ReadinessNotes == null ? (object)DBNull.Value : item.ReadinessNotes);
			sqlCommand.Parameters.AddWithValue("Translation", item.Translation == null ? (object)DBNull.Value : item.Translation);
			sqlCommand.Parameters.AddWithValue("TranslationNotes", item.TranslationNotes == null ? (object)DBNull.Value : item.TranslationNotes);
			sqlCommand.Parameters.AddWithValue("Twisting", item.Twisting == null ? (object)DBNull.Value : item.Twisting);
			sqlCommand.Parameters.AddWithValue("TwistingNotes", item.TwistingNotes == null ? (object)DBNull.Value : item.TwistingNotes);
			sqlCommand.Parameters.AddWithValue("Ultrason", item.Ultrason == null ? (object)DBNull.Value : item.Ultrason);
			sqlCommand.Parameters.AddWithValue("UltrasonNotes", item.UltrasonNotes == null ? (object)DBNull.Value : item.UltrasonNotes);
			sqlCommand.Parameters.AddWithValue("Validation", item.Validation == null ? (object)DBNull.Value : item.Validation);
			sqlCommand.Parameters.AddWithValue("ValidationFull", item.ValidationFull);
			sqlCommand.Parameters.AddWithValue("ValidationNotes", item.ValidationNotes == null ? (object)DBNull.Value : item.ValidationNotes);
			sqlCommand.Parameters.AddWithValue("Welding", item.Welding == null ? (object)DBNull.Value : item.Welding);
			sqlCommand.Parameters.AddWithValue("WeldingNotes", item.WeldingNotes == null ? (object)DBNull.Value : item.WeldingNotes);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity GetByArticleBomWithTransaction(int id, int bom, string index, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__BSD_StucklistenArticle_VersionValidation] WHERE [ArticleNr]=@Id AND [ArticleBOMVersion]=@bom AND TRIM([ArticleKundenIndex])=TRIM(@indexK)";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			sqlCommand.Parameters.AddWithValue("bom", bom);
			sqlCommand.Parameters.AddWithValue("indexK", index ?? "");
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		#endregion
	}
}
