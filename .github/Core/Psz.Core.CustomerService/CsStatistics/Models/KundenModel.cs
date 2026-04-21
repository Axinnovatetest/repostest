namespace Psz.Core.CustomerService.CsStatistics.Models
{
	public class KundenModel
	{
		public int ID { get; set; }
		public string Nummernkreis { get; set; }
		public string Kunde { get; set; }
		public int? CS_ID { get; set; }
		public string CS_Kontakt { get; set; }
		public string Projektadminsitration { get; set; }
		public string Projektbetreuer_D { get; set; }
		public string Technik_Kontakt_TN { get; set; }
		public string Stufe { get; set; }
		public int? Kundennummer { get; set; }
		public KundenModel()
		{

		}

		public KundenModel(Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity entity)
		{
			ID = entity.ID;
			CS_ID = entity.CS_ID;
			CS_Kontakt = entity.CS_Kontakt;
			Projektadminsitration = entity.Technik_Kontakt;
			Kunde = entity.Kunde;
			Kundennummer = entity.Kundennummer;
			Nummernkreis = entity.Nummerschlüssel;
			Projektbetreuer_D = entity.Projektbetreuer_D;
			Technik_Kontakt_TN = entity.Technik_Kontakt_TN;
			Stufe = entity.Stufe;
		}

		public Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity
			{
				ID = ID,
				CS_ID = CS_ID,
				CS_Kontakt = CS_Kontakt,
				Kunde = Kunde,
				Technik_Kontakt = Projektadminsitration,
				Kundennummer = Kundennummer,
				Nummerschlüssel = Nummernkreis,
				Projektbetreuer_D = Projektbetreuer_D,
				Technik_Kontakt_TN = Technik_Kontakt_TN,
				Stufe = Stufe,
			};
		}
	}
}
