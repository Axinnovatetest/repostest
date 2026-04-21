using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class StucklistenArticle_VersionValidationEntity
	{
		public int ArticleBOMVersion { get; set; }
		public int? ArticleCPVersion { get; set; }
		public string ArticleKundenIndex { get; set; }
		public DateTime? ArticleKundenIndexDatum { get; set; }
		public int ArticleNr { get; set; }
		public string ArticleNumber { get; set; }
		public bool? Assembling1 { get; set; }
		public string Assembling1Notes { get; set; }
		public bool? Assembling2 { get; set; }
		public string Assembling2Notes { get; set; }
		public bool? Assembling3 { get; set; }
		public string Assembling3Notes { get; set; }
		public DateTime BOMValidationDate { get; set; }
		public bool? Commission { get; set; }
		public string CommissionNotes { get; set; }
		public bool? ControleElectrical { get; set; }
		public string ControleElectricalNotes { get; set; }
		public bool? ControleVisual { get; set; }
		public string ControleVisualNotes { get; set; }
		public DateTime? CPValidationDate { get; set; }
		public bool? CrimpAfter { get; set; }
		public bool? CrimpAfterManual { get; set; }
		public string CrimpAfterManualNotes { get; set; }
		public string CrimpAfterNotes { get; set; }
		public bool? CrimpBefore { get; set; }
		public bool? CrimpBeforeManual { get; set; }
		public string CrimpBeforeManualNotes { get; set; }
		public string CrimpBeforeNotes { get; set; }
		public bool EngineeringControl { get; set; }
		public DateTime? EngineeringControlEditDate { get; set; }
		public string EngineeringControlEditUserEmail { get; set; }
		public int? EngineeringControlEditUserId { get; set; }
		public string EngineeringControlEditUserName { get; set; }
		public bool EngineeringDistribution { get; set; }
		public DateTime? EngineeringDistributionEditDate { get; set; }
		public string EngineeringDistributionEditUserEmail { get; set; }
		public int? EngineeringDistributionEditUserId { get; set; }
		public string EngineeringDistributionEditUserName { get; set; }
		public bool EngineeringPrint { get; set; }
		public DateTime? EngineeringPrintEditDate { get; set; }
		public string EngineeringPrintEditUserEmail { get; set; }
		public int? EngineeringPrintEditUserId { get; set; }
		public string EngineeringPrintEditUserName { get; set; }
		public bool EngineeringUpdate { get; set; }
		public DateTime? EngineeringUpdateEditDate { get; set; }
		public string EngineeringUpdateEditUserEmail { get; set; }
		public int? EngineeringUpdateEditUserId { get; set; }
		public string EngineeringUpdateEditUserName { get; set; }
		public bool EngineeringValidationFull { get; set; }
		public bool? Finition { get; set; }
		public string FinitionNotes { get; set; }
		public int Id { get; set; }
		public bool? InjectionOnCables { get; set; }
		public string InjectionOnCablesNotes { get; set; }
		public bool? InjectionPlastic { get; set; }
		public string InjectionPlasticNotes { get; set; }
		public bool? IsPartialDocumentation { get; set; }
		public bool? LabelingPlan { get; set; }
		public string LabelingPlanNotes { get; set; }
		public bool? Packaging { get; set; }
		public string PackagingNotes { get; set; }
		public bool? Pressing { get; set; }
		public string PressingNotes { get; set; }
		public bool QualityControl { get; set; }
		public DateTime? QualityControlEditDate { get; set; }
		public string QualityControlEditUserEmail { get; set; }
		public int? QualityControlEditUserId { get; set; }
		public string QualityControlEditUserName { get; set; }
		public bool QualityDistribution { get; set; }
		public DateTime? QualityDistributionEditDate { get; set; }
		public string QualityDistributionEditUserEmail { get; set; }
		public int? QualityDistributionEditUserId { get; set; }
		public string QualityDistributionEditUserName { get; set; }
		public bool QualityPrint { get; set; }
		public DateTime? QualityPrintEditDate { get; set; }
		public string QualityPrintEditUserEmail { get; set; }
		public int? QualityPrintEditUserId { get; set; }
		public string QualityPrintEditUserName { get; set; }
		public bool QualityUpdate { get; set; }
		public DateTime? QualityUpdateEditDate { get; set; }
		public string QualityUpdateEditUserEmail { get; set; }
		public int? QualityUpdateEditUserId { get; set; }
		public string QualityUpdateEditUserName { get; set; }
		public bool QualityValidationFull { get; set; }
		public bool? Readiness { get; set; }
		public string ReadinessNotes { get; set; }
		public bool? Translation { get; set; }
		public string TranslationNotes { get; set; }
		public bool? Twisting { get; set; }
		public string TwistingNotes { get; set; }
		public bool? Ultrason { get; set; }
		public string UltrasonNotes { get; set; }
		public bool? Validation { get; set; }
		public bool ValidationFull { get; set; }
		public string ValidationNotes { get; set; }
		public bool? Welding { get; set; }
		public string WeldingNotes { get; set; }
		public bool PendingCpValidation { get; set; }


		public StucklistenArticle_VersionValidationEntity() { }

		public StucklistenArticle_VersionValidationEntity(DataRow dataRow)
		{
			ArticleBOMVersion = Convert.ToInt32(dataRow["ArticleBOMVersion"]);
			ArticleCPVersion = (dataRow["ArticleCPVersion"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArticleCPVersion"]);
			ArticleKundenIndex = (dataRow["ArticleKundenIndex"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleKundenIndex"]);
			ArticleKundenIndexDatum = (dataRow["ArticleKundenIndexDatum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ArticleKundenIndexDatum"]);
			ArticleNr = Convert.ToInt32(dataRow["ArticleNr"]);
			ArticleNumber = Convert.ToString(dataRow["ArticleNumber"]);
			Assembling1 = (dataRow["Assembling1"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Assembling1"]);
			Assembling1Notes = (dataRow["Assembling1Notes"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Assembling1Notes"]);
			Assembling2 = (dataRow["Assembling2"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Assembling2"]);
			Assembling2Notes = (dataRow["Assembling2Notes"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Assembling2Notes"]);
			Assembling3 = (dataRow["Assembling3"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Assembling3"]);
			Assembling3Notes = (dataRow["Assembling3Notes"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Assembling3Notes"]);
			BOMValidationDate = Convert.ToDateTime(dataRow["BOMValidationDate"]);
			Commission = (dataRow["Commission"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Commission"]);
			CommissionNotes = (dataRow["CommissionNotes"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CommissionNotes"]);
			ControleElectrical = (dataRow["ControleElectrical"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ControleElectrical"]);
			ControleElectricalNotes = (dataRow["ControleElectricalNotes"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ControleElectricalNotes"]);
			ControleVisual = (dataRow["ControleVisual"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ControleVisual"]);
			ControleVisualNotes = (dataRow["ControleVisualNotes"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ControleVisualNotes"]);
			CPValidationDate = (dataRow["CPValidationDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CPValidationDate"]);
			CrimpAfter = (dataRow["CrimpAfter"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CrimpAfter"]);
			CrimpAfterManual = (dataRow["CrimpAfterManual"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CrimpAfterManual"]);
			CrimpAfterManualNotes = (dataRow["CrimpAfterManualNotes"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CrimpAfterManualNotes"]);
			CrimpAfterNotes = (dataRow["CrimpAfterNotes"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CrimpAfterNotes"]);
			CrimpBefore = (dataRow["CrimpBefore"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CrimpBefore"]);
			CrimpBeforeManual = (dataRow["CrimpBeforeManual"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["CrimpBeforeManual"]);
			CrimpBeforeManualNotes = (dataRow["CrimpBeforeManualNotes"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CrimpBeforeManualNotes"]);
			CrimpBeforeNotes = (dataRow["CrimpBeforeNotes"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CrimpBeforeNotes"]);
			EngineeringControl = Convert.ToBoolean(dataRow["EngineeringControl"]);
			EngineeringControlEditDate = (dataRow["EngineeringControlEditDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["EngineeringControlEditDate"]);
			EngineeringControlEditUserEmail = (dataRow["EngineeringControlEditUserEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EngineeringControlEditUserEmail"]);
			EngineeringControlEditUserId = (dataRow["EngineeringControlEditUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["EngineeringControlEditUserId"]);
			EngineeringControlEditUserName = (dataRow["EngineeringControlEditUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EngineeringControlEditUserName"]);
			EngineeringDistribution = Convert.ToBoolean(dataRow["EngineeringDistribution"]);
			EngineeringDistributionEditDate = (dataRow["EngineeringDistributionEditDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["EngineeringDistributionEditDate"]);
			EngineeringDistributionEditUserEmail = (dataRow["EngineeringDistributionEditUserEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EngineeringDistributionEditUserEmail"]);
			EngineeringDistributionEditUserId = (dataRow["EngineeringDistributionEditUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["EngineeringDistributionEditUserId"]);
			EngineeringDistributionEditUserName = (dataRow["EngineeringDistributionEditUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EngineeringDistributionEditUserName"]);
			EngineeringPrint = Convert.ToBoolean(dataRow["EngineeringPrint"]);
			EngineeringPrintEditDate = (dataRow["EngineeringPrintEditDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["EngineeringPrintEditDate"]);
			EngineeringPrintEditUserEmail = (dataRow["EngineeringPrintEditUserEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EngineeringPrintEditUserEmail"]);
			EngineeringPrintEditUserId = (dataRow["EngineeringPrintEditUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["EngineeringPrintEditUserId"]);
			EngineeringPrintEditUserName = (dataRow["EngineeringPrintEditUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EngineeringPrintEditUserName"]);
			EngineeringUpdate = Convert.ToBoolean(dataRow["EngineeringUpdate"]);
			EngineeringUpdateEditDate = (dataRow["EngineeringUpdateEditDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["EngineeringUpdateEditDate"]);
			EngineeringUpdateEditUserEmail = (dataRow["EngineeringUpdateEditUserEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EngineeringUpdateEditUserEmail"]);
			EngineeringUpdateEditUserId = (dataRow["EngineeringUpdateEditUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["EngineeringUpdateEditUserId"]);
			EngineeringUpdateEditUserName = (dataRow["EngineeringUpdateEditUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EngineeringUpdateEditUserName"]);
			EngineeringValidationFull = Convert.ToBoolean(dataRow["EngineeringValidationFull"]);
			Finition = (dataRow["Finition"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Finition"]);
			FinitionNotes = (dataRow["FinitionNotes"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FinitionNotes"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			InjectionOnCables = (dataRow["InjectionOnCables"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["InjectionOnCables"]);
			InjectionOnCablesNotes = (dataRow["InjectionOnCablesNotes"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["InjectionOnCablesNotes"]);
			InjectionPlastic = (dataRow["InjectionPlastic"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["InjectionPlastic"]);
			InjectionPlasticNotes = (dataRow["InjectionPlasticNotes"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["InjectionPlasticNotes"]);
			IsPartialDocumentation = (dataRow["IsPartialDocumentation"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsPartialDocumentation"]);
			LabelingPlan = (dataRow["LabelingPlan"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["LabelingPlan"]);
			LabelingPlanNotes = (dataRow["LabelingPlanNotes"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LabelingPlanNotes"]);
			Packaging = (dataRow["Packaging"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Packaging"]);
			PackagingNotes = (dataRow["PackagingNotes"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PackagingNotes"]);
			Pressing = (dataRow["Pressing"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Pressing"]);
			PressingNotes = (dataRow["PressingNotes"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PressingNotes"]);
			QualityControl = Convert.ToBoolean(dataRow["QualityControl"]);
			QualityControlEditDate = (dataRow["QualityControlEditDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["QualityControlEditDate"]);
			QualityControlEditUserEmail = (dataRow["QualityControlEditUserEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["QualityControlEditUserEmail"]);
			QualityControlEditUserId = (dataRow["QualityControlEditUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["QualityControlEditUserId"]);
			QualityControlEditUserName = (dataRow["QualityControlEditUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["QualityControlEditUserName"]);
			QualityDistribution = Convert.ToBoolean(dataRow["QualityDistribution"]);
			QualityDistributionEditDate = (dataRow["QualityDistributionEditDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["QualityDistributionEditDate"]);
			QualityDistributionEditUserEmail = (dataRow["QualityDistributionEditUserEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["QualityDistributionEditUserEmail"]);
			QualityDistributionEditUserId = (dataRow["QualityDistributionEditUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["QualityDistributionEditUserId"]);
			QualityDistributionEditUserName = (dataRow["QualityDistributionEditUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["QualityDistributionEditUserName"]);
			QualityPrint = Convert.ToBoolean(dataRow["QualityPrint"]);
			QualityPrintEditDate = (dataRow["QualityPrintEditDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["QualityPrintEditDate"]);
			QualityPrintEditUserEmail = (dataRow["QualityPrintEditUserEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["QualityPrintEditUserEmail"]);
			QualityPrintEditUserId = (dataRow["QualityPrintEditUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["QualityPrintEditUserId"]);
			QualityPrintEditUserName = (dataRow["QualityPrintEditUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["QualityPrintEditUserName"]);
			QualityUpdate = Convert.ToBoolean(dataRow["QualityUpdate"]);
			QualityUpdateEditDate = (dataRow["QualityUpdateEditDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["QualityUpdateEditDate"]);
			QualityUpdateEditUserEmail = (dataRow["QualityUpdateEditUserEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["QualityUpdateEditUserEmail"]);
			QualityUpdateEditUserId = (dataRow["QualityUpdateEditUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["QualityUpdateEditUserId"]);
			QualityUpdateEditUserName = (dataRow["QualityUpdateEditUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["QualityUpdateEditUserName"]);
			QualityValidationFull = Convert.ToBoolean(dataRow["QualityValidationFull"]);
			Readiness = (dataRow["Readiness"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Readiness"]);
			ReadinessNotes = (dataRow["ReadinessNotes"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ReadinessNotes"]);
			Translation = (dataRow["Translation"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Translation"]);
			TranslationNotes = (dataRow["TranslationNotes"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["TranslationNotes"]);
			Twisting = (dataRow["Twisting"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Twisting"]);
			TwistingNotes = (dataRow["TwistingNotes"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["TwistingNotes"]);
			Ultrason = (dataRow["Ultrason"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Ultrason"]);
			UltrasonNotes = (dataRow["UltrasonNotes"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UltrasonNotes"]);
			Validation = (dataRow["Validation"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Validation"]);
			ValidationFull = Convert.ToBoolean(dataRow["ValidationFull"]);
			ValidationNotes = (dataRow["ValidationNotes"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ValidationNotes"]);
			Welding = (dataRow["Welding"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Welding"]);
			WeldingNotes = (dataRow["WeldingNotes"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["WeldingNotes"]);
			PendingCpValidation = Convert.ToBoolean(dataRow["PendingCpValidation"]);

		}

		public StucklistenArticle_VersionValidationEntity ShallowClone()
		{
			return new StucklistenArticle_VersionValidationEntity
			{
				ArticleBOMVersion = ArticleBOMVersion,
				ArticleCPVersion = ArticleCPVersion,
				ArticleKundenIndex = ArticleKundenIndex,
				ArticleKundenIndexDatum = ArticleKundenIndexDatum,
				ArticleNr = ArticleNr,
				ArticleNumber = ArticleNumber,
				Assembling1 = Assembling1,
				Assembling1Notes = Assembling1Notes,
				Assembling2 = Assembling2,
				Assembling2Notes = Assembling2Notes,
				Assembling3 = Assembling3,
				Assembling3Notes = Assembling3Notes,
				BOMValidationDate = BOMValidationDate,
				Commission = Commission,
				CommissionNotes = CommissionNotes,
				ControleElectrical = ControleElectrical,
				ControleElectricalNotes = ControleElectricalNotes,
				ControleVisual = ControleVisual,
				ControleVisualNotes = ControleVisualNotes,
				CPValidationDate = CPValidationDate,
				CrimpAfter = CrimpAfter,
				CrimpAfterManual = CrimpAfterManual,
				CrimpAfterManualNotes = CrimpAfterManualNotes,
				CrimpAfterNotes = CrimpAfterNotes,
				CrimpBefore = CrimpBefore,
				CrimpBeforeManual = CrimpBeforeManual,
				CrimpBeforeManualNotes = CrimpBeforeManualNotes,
				CrimpBeforeNotes = CrimpBeforeNotes,
				EngineeringControl = EngineeringControl,
				EngineeringControlEditDate = EngineeringControlEditDate,
				EngineeringControlEditUserEmail = EngineeringControlEditUserEmail,
				EngineeringControlEditUserId = EngineeringControlEditUserId,
				EngineeringControlEditUserName = EngineeringControlEditUserName,
				EngineeringDistribution = EngineeringDistribution,
				EngineeringDistributionEditDate = EngineeringDistributionEditDate,
				EngineeringDistributionEditUserEmail = EngineeringDistributionEditUserEmail,
				EngineeringDistributionEditUserId = EngineeringDistributionEditUserId,
				EngineeringDistributionEditUserName = EngineeringDistributionEditUserName,
				EngineeringPrint = EngineeringPrint,
				EngineeringPrintEditDate = EngineeringPrintEditDate,
				EngineeringPrintEditUserEmail = EngineeringPrintEditUserEmail,
				EngineeringPrintEditUserId = EngineeringPrintEditUserId,
				EngineeringPrintEditUserName = EngineeringPrintEditUserName,
				EngineeringUpdate = EngineeringUpdate,
				EngineeringUpdateEditDate = EngineeringUpdateEditDate,
				EngineeringUpdateEditUserEmail = EngineeringUpdateEditUserEmail,
				EngineeringUpdateEditUserId = EngineeringUpdateEditUserId,
				EngineeringUpdateEditUserName = EngineeringUpdateEditUserName,
				EngineeringValidationFull = EngineeringValidationFull,
				Finition = Finition,
				FinitionNotes = FinitionNotes,
				Id = Id,
				InjectionOnCables = InjectionOnCables,
				InjectionOnCablesNotes = InjectionOnCablesNotes,
				InjectionPlastic = InjectionPlastic,
				InjectionPlasticNotes = InjectionPlasticNotes,
				IsPartialDocumentation = IsPartialDocumentation,
				LabelingPlan = LabelingPlan,
				LabelingPlanNotes = LabelingPlanNotes,
				Packaging = Packaging,
				PackagingNotes = PackagingNotes,
				Pressing = Pressing,
				PressingNotes = PressingNotes,
				QualityControl = QualityControl,
				QualityControlEditDate = QualityControlEditDate,
				QualityControlEditUserEmail = QualityControlEditUserEmail,
				QualityControlEditUserId = QualityControlEditUserId,
				QualityControlEditUserName = QualityControlEditUserName,
				QualityDistribution = QualityDistribution,
				QualityDistributionEditDate = QualityDistributionEditDate,
				QualityDistributionEditUserEmail = QualityDistributionEditUserEmail,
				QualityDistributionEditUserId = QualityDistributionEditUserId,
				QualityDistributionEditUserName = QualityDistributionEditUserName,
				QualityPrint = QualityPrint,
				QualityPrintEditDate = QualityPrintEditDate,
				QualityPrintEditUserEmail = QualityPrintEditUserEmail,
				QualityPrintEditUserId = QualityPrintEditUserId,
				QualityPrintEditUserName = QualityPrintEditUserName,
				QualityUpdate = QualityUpdate,
				QualityUpdateEditDate = QualityUpdateEditDate,
				QualityUpdateEditUserEmail = QualityUpdateEditUserEmail,
				QualityUpdateEditUserId = QualityUpdateEditUserId,
				QualityUpdateEditUserName = QualityUpdateEditUserName,
				QualityValidationFull = QualityValidationFull,
				Readiness = Readiness,
				ReadinessNotes = ReadinessNotes,
				Translation = Translation,
				TranslationNotes = TranslationNotes,
				Twisting = Twisting,
				TwistingNotes = TwistingNotes,
				Ultrason = Ultrason,
				UltrasonNotes = UltrasonNotes,
				Validation = Validation,
				ValidationFull = ValidationFull,
				ValidationNotes = ValidationNotes,
				Welding = Welding,
				WeldingNotes = WeldingNotes,
				PendingCpValidation = PendingCpValidation,
			};
		}
	}
}

