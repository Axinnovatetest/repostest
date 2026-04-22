using System;

namespace Psz.Core.BaseData.Models.Settings.CustomerItemNumber
{
	public class CustomerNumbersRequestModel
	{
		public string Key { get; set; }
		public string Value { get; set; }
		public bool Analyse { get; set; }
	}
	public class CustomerItemNumberResponseModel
	{
		public int? CS_ID { get; set; }
		public string CS_Kontakt { get; set; }
		public int ID { get; set; }
		public string Kunde { get; set; }
		public int? Kundennummer { get; set; }
		public string Nummerschlussel { get; set; }
		public string Projektbetreuer_D { get; set; }
		public string Stufe { get; set; }
		public string Technik_Kontakt { get; set; }
		public string Technik_Kontakt_TN { get; set; }
		public bool Analyse { get; set; }
		public string AnalyseName { get; set; }
		public DateTime? CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public string CreationUserName { get; set; }
		public bool IsDeprecated { get; set; } = false;

		public CustomerItemNumberResponseModel(Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity entity)
		{
			if(entity == null)
				return;

			// -
			CS_ID = entity.CS_ID;
			CS_Kontakt = entity.CS_Kontakt;
			ID = entity.ID;
			Kunde = entity.Kunde;
			Kundennummer = entity.Kundennummer;
			Nummerschlussel = entity.Nummerschlüssel;
			Projektbetreuer_D = entity.Projektbetreuer_D;
			Stufe = entity.Stufe;
			Technik_Kontakt = entity.Technik_Kontakt;
			Technik_Kontakt_TN = entity.Technik_Kontakt_TN;
			//-
			Analyse = entity.Analyse ?? false;
			AnalyseName = entity.AnalyseName;
			CreationTime = entity.CreationTime;
			CreationUserId = entity.CreationUserId ?? -1;
			CreationUserName = entity.CreationUserName;
			// -
			IsDeprecated = entity.Analyse == true && (entity.CreationTime ?? DateTime.MinValue) < DateTime.Today.AddMonths(-12);
		}

		public Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity
			{
				CS_ID = CS_ID,
				CS_Kontakt = CS_Kontakt,
				ID = ID,
				Kunde = Kunde,
				Kundennummer = Kundennummer,
				Nummerschlüssel = Nummerschlussel,
				Projektbetreuer_D = Projektbetreuer_D,
				Stufe = Stufe,
				Technik_Kontakt = Technik_Kontakt,
				Technik_Kontakt_TN = Technik_Kontakt_TN,
				Analyse = Analyse,
				AnalyseName = AnalyseName
			};
		}

	}
	public class PromoteToCustomerRequestModel
	{
		public int CustomerItemId { get; set; }
		public int TargetCustomerNumber { get; set; }
		public string TargetCustomerName { get; set; }
	}
}
