using System;

namespace Psz.Core.BaseData.Models.Article.BillOfMaterial
{
	public class ValidateBomResponseModel
	{
		public int ID { get; set; }
		public DateTime? Datum { get; set; } // Date
		public int? Lagerort_id { get; set; }

		// -
		public int? BomVersion { get; set; }
		public int? Artikel_Nr { get; set; }
		public string Artikel_Nummer { get; set; }

		// -
		public DateTime? Termin_Bestatigt1 { get; set; } // Appointment confirmed 1 date
		public DateTime? Termin_Bestatigt2 { get; set; } // Appointment confirmed 2 date
		public string Gewerk_1 { get; set; }
		public string Gewerk_2 { get; set; }
		public string Gewerk_3 { get; set; }

		public int? Originalanzahl { get; set; }
		public int? Anzahl_erledigt { get; set; }
		public int? Tage_Abweichung { get; set; }
		public string KundenIndex { get; set; }
		public int? Fertigungsnummer { get; set; }

		public ValidateBomResponseModel(Infrastructure.Data.Entities.Tables.PRS.FertigungEntity fertigungEntity, string articleNumber)
		{
			Artikel_Nummer = articleNumber;
			if(fertigungEntity == null)
				return;

			ID = fertigungEntity.ID;
			Datum = fertigungEntity.Datum;  // Date
			Lagerort_id = fertigungEntity.Lagerort_id;

			BomVersion = fertigungEntity.BomVersion;
			Artikel_Nr = fertigungEntity.Artikel_Nr;

			Termin_Bestatigt1 = fertigungEntity.Termin_Bestatigt1;  // Appointment confirmed 1 date
			Termin_Bestatigt2 = fertigungEntity.Termin_Bestatigt2;  // Appointment confirmed 2 date
			Gewerk_1 = fertigungEntity.Gewerk_1;
			Gewerk_2 = fertigungEntity.Gewerk_2;
			Gewerk_3 = fertigungEntity.Gewerk_3;
			Anzahl_erledigt = fertigungEntity.Anzahl_erledigt;
			Originalanzahl = fertigungEntity.Originalanzahl;
			Tage_Abweichung = fertigungEntity.Tage_Abweichung;
			KundenIndex = fertigungEntity.KundenIndex;
			Fertigungsnummer = fertigungEntity.Fertigungsnummer;
		}
	}

	public class ValidateWPartialDocRequestModel
	{
		public int ArticleId { get; set; }
		public bool CanPartialValidation { get; set; } = false;

		// - data
		public bool Commission { get; set; } = false;                     // - Commission
		public bool Readiness { get; set; } = false;                      // - Bereit
		public bool CrimpBeforeManual { get; set; } = false;              // - Sertissage avant manuel
		public bool CrimpBefore { get; set; } = false;                  // - Sertissage avant
		public bool Ultrason { get; set; } = false;                       // - ultrason
		public bool Twisting { get; set; } = false;                       // - Torsadage
		public bool InjectionPlastic { get; set; } = false;               // - Injection Plastique
		public bool Welding { get; set; } = false;                        // - Soudure
		public bool Assembling1 { get; set; } = false;                    // - Assemblage (1)
		public bool Assembling2 { get; set; } = false;                    // - Assemblage (2)
		public bool Assembling3 { get; set; } = false;                    // - Assemblage (3)
		public bool CrimpAfterManual { get; set; } = false;               // - Sertissage apres manuel
		public bool CrimpAfter { get; set; } = false;                     // - Sertissage apres
		public bool InjectionOnCables { get; set; } = false;              // - Injection sur cables
		public bool Pressing { get; set; } = false;                       // - Pressage
		public bool LabelingPlan { get; set; } = false;                   // - Plan d´etiquette
		public bool ControleElectrical { get; set; } = false;             // - Controle Electrique
		public bool ControleVisual { get; set; } = false;                 // - Contrôle visual
		public bool Finition { get; set; } = false;                       // - Finition
		public bool Packaging { get; set; } = false;                      // - Emballage
		public bool Validation { get; set; } = false;                     // - Validation
		public bool Translation { get; set; } = false;                    // - Traduction

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
	}
}
