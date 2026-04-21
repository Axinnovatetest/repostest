using System;

namespace Psz.Core.BaseData.Models.Article.BillOfMaterial.VersionControl
{
	public class GetArticleListResponseModel
	{
		public int ArticleBOMVersion { get; set; }
		public int? ArticleCPVersion { get; set; }
		public string ArticleKundenIndex { get; set; }
		public DateTime? ArticleKundenIndexDatum { get; set; }
		public int ArticleNr { get; set; }
		public string ArticleNumber { get; set; }
		public DateTime BOMValidationDate { get; set; }
		public DateTime? CPValidationDate { get; set; }
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
		public long Id { get; set; }
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
		public bool ValidationFull { get; set; }

		// -2022-07-13 - Partial Documentation
		public bool IsPartialDocumentation { get; set; }
		public bool Commission { get; set; }
		public bool Readiness { get; set; }
		public bool CrimpBeforeManual { get; set; }
		public bool CrimpBefore { get; set; }
		public bool Ultrason { get; set; }
		public bool Twisting { get; set; }
		public bool InjectionPlastic { get; set; }
		public bool Welding { get; set; }
		public bool Assembling1 { get; set; }
		public bool Assembling2 { get; set; }
		public bool Assembling3 { get; set; }
		public bool CrimpAfterManual { get; set; }
		public bool CrimpAfter { get; set; }
		public bool InjectionOnCables { get; set; }
		public bool Pressing { get; set; }
		public bool LabelingPlan { get; set; }
		public bool ControleElectrical { get; set; }
		public bool ControleVisual { get; set; }
		public bool Finition { get; set; }
		public bool Packaging { get; set; }
		public bool Validation { get; set; }
		public bool Translation { get; set; }
		public string CommissionNotes { get; set; }
		public string ReadinessNotes { get; set; }
		public string CrimpBeforeManualNotes { get; set; }
		public string CrimpBeforeNotes { get; set; }
		public string UltrasonNotes { get; set; }
		public string TwistingNotes { get; set; }
		public string InjectionPlasticNotes { get; set; }
		public string WeldingNotes { get; set; }
		public string Assembling1Notes { get; set; }
		public string Assembling2Notes { get; set; }
		public string Assembling3Notes { get; set; }
		public string CrimpAfterManualNotes { get; set; }
		public string CrimpAfterNotes { get; set; }
		public string InjectionOnCablesNotes { get; set; }
		public string PressingNotes { get; set; }
		public string LabelingPlanNotes { get; set; }
		public string ControleElectricalNotes { get; set; }
		public string ControleVisualNotes { get; set; }
		public string FinitionNotes { get; set; }
		public string PackagingNotes { get; set; }
		public string ValidationNotes { get; set; }
		public string TranslationNotes { get; set; }


		public GetArticleListResponseModel(Infrastructure.Data.Entities.Tables.BSD.StucklistenArticle_VersionValidationEntity stucklistenArticle_VersionValidationEntity)
		{
			if(stucklistenArticle_VersionValidationEntity == null)
				return;


			ArticleBOMVersion = stucklistenArticle_VersionValidationEntity.ArticleBOMVersion;
			ArticleCPVersion = stucklistenArticle_VersionValidationEntity.ArticleCPVersion;
			ArticleKundenIndex = stucklistenArticle_VersionValidationEntity.ArticleKundenIndex;
			ArticleKundenIndexDatum = stucklistenArticle_VersionValidationEntity.ArticleKundenIndexDatum;
			ArticleNr = stucklistenArticle_VersionValidationEntity.ArticleNr;
			ArticleNumber = stucklistenArticle_VersionValidationEntity.ArticleNumber;
			BOMValidationDate = stucklistenArticle_VersionValidationEntity.BOMValidationDate;
			CPValidationDate = stucklistenArticle_VersionValidationEntity.CPValidationDate;
			EngineeringControl = stucklistenArticle_VersionValidationEntity.EngineeringControl;
			EngineeringControlEditDate = stucklistenArticle_VersionValidationEntity.EngineeringControlEditDate;
			EngineeringControlEditUserEmail = stucklistenArticle_VersionValidationEntity.EngineeringControlEditUserEmail;
			EngineeringControlEditUserId = stucklistenArticle_VersionValidationEntity.EngineeringControlEditUserId;
			EngineeringControlEditUserName = stucklistenArticle_VersionValidationEntity.EngineeringControlEditUserName;
			EngineeringDistribution = stucklistenArticle_VersionValidationEntity.EngineeringDistribution;
			EngineeringDistributionEditDate = stucklistenArticle_VersionValidationEntity.EngineeringDistributionEditDate;
			EngineeringDistributionEditUserEmail = stucklistenArticle_VersionValidationEntity.EngineeringDistributionEditUserEmail;
			EngineeringDistributionEditUserId = stucklistenArticle_VersionValidationEntity.EngineeringDistributionEditUserId;
			EngineeringDistributionEditUserName = stucklistenArticle_VersionValidationEntity.EngineeringDistributionEditUserName;
			EngineeringPrint = stucklistenArticle_VersionValidationEntity.EngineeringPrint;
			EngineeringPrintEditDate = stucklistenArticle_VersionValidationEntity.EngineeringPrintEditDate;
			EngineeringPrintEditUserEmail = stucklistenArticle_VersionValidationEntity.EngineeringPrintEditUserEmail;
			EngineeringPrintEditUserId = stucklistenArticle_VersionValidationEntity.EngineeringPrintEditUserId;
			EngineeringPrintEditUserName = stucklistenArticle_VersionValidationEntity.EngineeringPrintEditUserName;
			EngineeringUpdate = stucklistenArticle_VersionValidationEntity.EngineeringUpdate;
			EngineeringUpdateEditDate = stucklistenArticle_VersionValidationEntity.EngineeringUpdateEditDate;
			EngineeringUpdateEditUserEmail = stucklistenArticle_VersionValidationEntity.EngineeringUpdateEditUserEmail;
			EngineeringUpdateEditUserId = stucklistenArticle_VersionValidationEntity.EngineeringUpdateEditUserId;
			EngineeringUpdateEditUserName = stucklistenArticle_VersionValidationEntity.EngineeringUpdateEditUserName;
			EngineeringValidationFull = stucklistenArticle_VersionValidationEntity.EngineeringValidationFull;
			Id = stucklistenArticle_VersionValidationEntity.Id;
			QualityControl = stucklistenArticle_VersionValidationEntity.QualityControl;
			QualityControlEditDate = stucklistenArticle_VersionValidationEntity.QualityControlEditDate;
			QualityControlEditUserEmail = stucklistenArticle_VersionValidationEntity.QualityControlEditUserEmail;
			QualityControlEditUserId = stucklistenArticle_VersionValidationEntity.QualityControlEditUserId;
			QualityControlEditUserName = stucklistenArticle_VersionValidationEntity.QualityControlEditUserName;
			QualityDistribution = stucklistenArticle_VersionValidationEntity.QualityDistribution;
			QualityDistributionEditDate = stucklistenArticle_VersionValidationEntity.QualityDistributionEditDate;
			QualityDistributionEditUserEmail = stucklistenArticle_VersionValidationEntity.QualityDistributionEditUserEmail;
			QualityDistributionEditUserId = stucklistenArticle_VersionValidationEntity.QualityDistributionEditUserId;
			QualityDistributionEditUserName = stucklistenArticle_VersionValidationEntity.QualityDistributionEditUserName;
			QualityPrint = stucklistenArticle_VersionValidationEntity.QualityPrint;
			QualityPrintEditDate = stucklistenArticle_VersionValidationEntity.QualityPrintEditDate;
			QualityPrintEditUserEmail = stucklistenArticle_VersionValidationEntity.QualityPrintEditUserEmail;
			QualityPrintEditUserId = stucklistenArticle_VersionValidationEntity.QualityPrintEditUserId;
			QualityPrintEditUserName = stucklistenArticle_VersionValidationEntity.QualityPrintEditUserName;
			QualityUpdate = stucklistenArticle_VersionValidationEntity.QualityUpdate;
			QualityUpdateEditDate = stucklistenArticle_VersionValidationEntity.QualityUpdateEditDate;
			QualityUpdateEditUserEmail = stucklistenArticle_VersionValidationEntity.QualityUpdateEditUserEmail;
			QualityUpdateEditUserId = stucklistenArticle_VersionValidationEntity.QualityUpdateEditUserId;
			QualityUpdateEditUserName = stucklistenArticle_VersionValidationEntity.QualityUpdateEditUserName;
			QualityValidationFull = stucklistenArticle_VersionValidationEntity.QualityValidationFull;
			ValidationFull = stucklistenArticle_VersionValidationEntity.ValidationFull;


			// - 2022-07-13 - Partial
			IsPartialDocumentation = (stucklistenArticle_VersionValidationEntity.IsPartialDocumentation ?? false);
			Commission = (stucklistenArticle_VersionValidationEntity.Commission ?? false);                     // - Commission
			Readiness = (stucklistenArticle_VersionValidationEntity.Readiness ?? false);                       // - Bereit
			CrimpBeforeManual = (stucklistenArticle_VersionValidationEntity.CrimpBeforeManual ?? false);       // - Sertissage avant manuel
			CrimpBefore = (stucklistenArticle_VersionValidationEntity.CrimpBefore ?? false);                   // - Sertissage avant
			Ultrason = (stucklistenArticle_VersionValidationEntity.Ultrason ?? false);                         // - ultrason
			Twisting = (stucklistenArticle_VersionValidationEntity.Twisting ?? false);                         // - Torsadage
			InjectionPlastic = (stucklistenArticle_VersionValidationEntity.InjectionPlastic ?? false);         // - Injection Plastique
			Welding = (stucklistenArticle_VersionValidationEntity.Welding ?? false);                           // - Soudure
			Assembling1 = (stucklistenArticle_VersionValidationEntity.Assembling1 ?? false);                   // - Assemblage (1)
			Assembling2 = (stucklistenArticle_VersionValidationEntity.Assembling2 ?? false);                   // - Assemblage (2)
			Assembling3 = (stucklistenArticle_VersionValidationEntity.Assembling3 ?? false);                   // - Assemblage (3)
			CrimpAfterManual = (stucklistenArticle_VersionValidationEntity.CrimpAfterManual ?? false);         // - Sertissage apres manuel
			CrimpAfter = (stucklistenArticle_VersionValidationEntity.CrimpAfter ?? false);                     // - Sertissage apres
			InjectionOnCables = (stucklistenArticle_VersionValidationEntity.InjectionOnCables ?? false);       // - Injection sur cables
			Pressing = (stucklistenArticle_VersionValidationEntity.Pressing ?? false);                         // - Pressage
			LabelingPlan = (stucklistenArticle_VersionValidationEntity.LabelingPlan ?? false);                 // - Plan d´etiquette
			ControleElectrical = (stucklistenArticle_VersionValidationEntity.ControleElectrical ?? false);     // - Controle Electrique
			ControleVisual = (stucklistenArticle_VersionValidationEntity.ControleVisual ?? false);             // - Contrôle visual
			Finition = (stucklistenArticle_VersionValidationEntity.Finition ?? false);                         // - Finition
			Packaging = (stucklistenArticle_VersionValidationEntity.Packaging ?? false);                       // - Emballage
			Validation = (stucklistenArticle_VersionValidationEntity.Validation ?? false);                     // - Validation
			Translation = (stucklistenArticle_VersionValidationEntity.Translation ?? false);                   // - Traduction

			CommissionNotes = stucklistenArticle_VersionValidationEntity.CommissionNotes;
			ReadinessNotes = stucklistenArticle_VersionValidationEntity.ReadinessNotes;
			CrimpBeforeManualNotes = stucklistenArticle_VersionValidationEntity.CrimpBeforeManualNotes;
			CrimpBeforeNotes = stucklistenArticle_VersionValidationEntity.CrimpBeforeNotes;
			UltrasonNotes = stucklistenArticle_VersionValidationEntity.UltrasonNotes;
			TwistingNotes = stucklistenArticle_VersionValidationEntity.TwistingNotes;
			InjectionPlasticNotes = stucklistenArticle_VersionValidationEntity.InjectionPlasticNotes;
			WeldingNotes = stucklistenArticle_VersionValidationEntity.WeldingNotes;
			Assembling1Notes = stucklistenArticle_VersionValidationEntity.Assembling1Notes;
			Assembling2Notes = stucklistenArticle_VersionValidationEntity.Assembling2Notes;
			Assembling3Notes = stucklistenArticle_VersionValidationEntity.Assembling3Notes;
			CrimpAfterManualNotes = stucklistenArticle_VersionValidationEntity.CrimpAfterManualNotes;
			CrimpAfterNotes = stucklistenArticle_VersionValidationEntity.CrimpAfterNotes;
			InjectionOnCablesNotes = stucklistenArticle_VersionValidationEntity.InjectionOnCablesNotes;
			PressingNotes = stucklistenArticle_VersionValidationEntity.PressingNotes;
			LabelingPlanNotes = stucklistenArticle_VersionValidationEntity.LabelingPlanNotes;
			ControleElectricalNotes = stucklistenArticle_VersionValidationEntity.ControleElectricalNotes;
			ControleVisualNotes = stucklistenArticle_VersionValidationEntity.ControleVisualNotes;
			FinitionNotes = stucklistenArticle_VersionValidationEntity.FinitionNotes;
			PackagingNotes = stucklistenArticle_VersionValidationEntity.PackagingNotes;
			ValidationNotes = stucklistenArticle_VersionValidationEntity.ValidationNotes;
			TranslationNotes = stucklistenArticle_VersionValidationEntity.TranslationNotes;
		}
	}
}
