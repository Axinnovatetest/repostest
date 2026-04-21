using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data.Entities.Joins
{
	public class Kunden_Lieferanten_LPEntities
	{
		public int Nr { get; set; }
		public int ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public string StandardLiefrentenName { get; set; }
		public string Name1 { get; set; }
		public bool? Standardlieferant { get; set; }
		public string Artikelbezeichnung { get; set; }
		public string Artikelbezeichnung2 { get; set; }
		public string Bestell_Nr { get; set; }
		public decimal? Einkaufspreis { get; set; }
		public decimal? Einkaufspreis1 { get; set; }
		public decimal? Einkaufspreis2 { get; set; }
		public string Angebot { get; set; }
		public DateTime? Angebot_Datum { get; set; }
		public DateTime? Einkaufspreis1_gultig_bis { get; set; }
		public DateTime? Einkaufspreis2_gultig_bis { get; set; }
		public string Telefon { get; set; }
		public string Fax { get; set; }
		public string eMail { get; set; }
		public int? Mindestbestellmenge { get; set; }
		public int? Wiederbeschaffungszeitraum { get; set; }

		public Kunden_Lieferanten_LPEntities(DataRow dataRow)
		{
			ArtikelNr = Convert.ToInt32(dataRow["Artikel-Nr"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
			StandardLiefrentenName = (dataRow["StandardLiefrentenName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["StandardLiefrentenName"]);
			Standardlieferant = (dataRow["Standardlieferant"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["Standardlieferant"]);
			Artikelbezeichnung = (dataRow["Artikelbezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelbezeichnung"]);
			Artikelbezeichnung2 = (dataRow["Artikelbezeichnung2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelbezeichnung2"]);
			Bestell_Nr = (dataRow["Bestell-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestell-Nr"]);
			Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufspreis"]);
			Angebot = (dataRow["Angebot"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebot"]);
			Angebot_Datum = (dataRow["Angebot_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Angebot_Datum"]);
			Telefon = (dataRow["Telefon"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Telefon"]);
			Fax = (dataRow["Fax"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Fax"]);
			eMail = (dataRow["eMail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["eMail"]);
			Mindestbestellmenge = (dataRow["Mindestbestellmenge"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Mindestbestellmenge"]);
			Wiederbeschaffungszeitraum = (dataRow["Wiederbeschaffungszeitraum"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Wiederbeschaffungszeitraum"]);
		}
		public Kunden_Lieferanten_LPEntities(DataRow dataRow, bool getNr)
		{
			if(getNr)
			{
				Nr = Convert.ToInt32(dataRow["Nr"]);
			}
			ArtikelNr = Convert.ToInt32(dataRow["Artikel-Nr"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
			Standardlieferant = (dataRow["Standardlieferant"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["Standardlieferant"]);
			Artikelbezeichnung = (dataRow["Artikelbezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelbezeichnung"]);
			Artikelbezeichnung2 = (dataRow["Artikelbezeichnung2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelbezeichnung2"]);
			Bestell_Nr = (dataRow["Bestell-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestell-Nr"]);
			Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufspreis"]);
			Einkaufspreis1 = (dataRow["Einkaufspreis1"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufspreis1"]);
			Einkaufspreis2 = (dataRow["Einkaufspreis2"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufspreis2"]);
			Angebot = (dataRow["Angebot"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebot"]);
			Angebot_Datum = (dataRow["Angebot_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Angebot_Datum"]);
			Einkaufspreis1_gultig_bis = (dataRow["Einkaufspreis1 gültig bis"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Einkaufspreis1 gültig bis"]);
			Einkaufspreis2_gultig_bis = (dataRow["Einkaufspreis2 gültig bis"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Einkaufspreis2 gültig bis"]);
			Telefon = (dataRow["Telefon"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Telefon"]);
			Fax = (dataRow["Fax"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Fax"]);
			eMail = (dataRow["eMail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["eMail"]);
			Mindestbestellmenge = (dataRow["Mindestbestellmenge"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Mindestbestellmenge"]);
			Wiederbeschaffungszeitraum = (dataRow["Wiederbeschaffungszeitraum"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Wiederbeschaffungszeitraum"]);
		}
	}

	public class Kunden_Lieferanten_LPExtendedEntity
	{
		public int Nr { get; set; }
		public int ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public string StandardLiefrentenName { get; set; }
		public string Name1 { get; set; }
		public bool? Standardlieferant { get; set; }
		public string Artikelbezeichnung { get; set; }
		public string Artikelbezeichnung2 { get; set; }
		public string Bestell_Nr { get; set; }
		public decimal? Einkaufspreis { get; set; }
		public decimal? Einkaufspreis1 { get; set; }
		public decimal? Einkaufspreis2 { get; set; }
		public string Angebot { get; set; }
		public DateTime? Angebot_Datum { get; set; }
		public DateTime? Einkaufspreis1_gultig_bis { get; set; }
		public DateTime? Einkaufspreis2_gultig_bis { get; set; }
		public string Telefon { get; set; }
		public string Fax { get; set; }
		public string eMail { get; set; }
		public int? Mindestbestellmenge { get; set; }
		public int? Wiederbeschaffungszeitraum { get; set; }
		public decimal? Verpackungseinheit { get; set; }
		public decimal? Last2YearsOrderQuantity { get; set; }
		public decimal? BedarfPO { get; set; }


		public Kunden_Lieferanten_LPExtendedEntity(DataRow dataRow)
		{
			ArtikelNr = Convert.ToInt32(dataRow["Artikel-Nr"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
			StandardLiefrentenName = (dataRow["StandardLiefrentenName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["StandardLiefrentenName"]);
			Standardlieferant = (dataRow["Standardlieferant"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["Standardlieferant"]);
			Artikelbezeichnung = (dataRow["Artikelbezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelbezeichnung"]);
			Artikelbezeichnung2 = (dataRow["Artikelbezeichnung2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelbezeichnung2"]);
			Bestell_Nr = (dataRow["Bestell-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestell-Nr"]);
			Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufspreis"]);
			Angebot = (dataRow["Angebot"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebot"]);
			Angebot_Datum = (dataRow["Angebot_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Angebot_Datum"]);
			Telefon = (dataRow["Telefon"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Telefon"]);
			Fax = (dataRow["Fax"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Fax"]);
			eMail = (dataRow["eMail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["eMail"]);
			Mindestbestellmenge = (dataRow["Mindestbestellmenge"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Mindestbestellmenge"]);
			Wiederbeschaffungszeitraum = (dataRow["Wiederbeschaffungszeitraum"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Wiederbeschaffungszeitraum"]);
			Verpackungseinheit = (dataRow["Verpackungseinheit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Verpackungseinheit"]);
			Last2YearsOrderQuantity = (dataRow["Last2YearsOrderQuantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Last2YearsOrderQuantity"]);
			BedarfPO = (dataRow["BedarfPO"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["BedarfPO"]);
		}
		public Kunden_Lieferanten_LPExtendedEntity(DataRow dataRow, bool getNr)
		{
			if(getNr)
			{
				Nr = Convert.ToInt32(dataRow["Nr"]);
			}
			ArtikelNr = Convert.ToInt32(dataRow["Artikel-Nr"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
			Standardlieferant = (dataRow["Standardlieferant"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["Standardlieferant"]);
			Artikelbezeichnung = (dataRow["Artikelbezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelbezeichnung"]);
			Artikelbezeichnung2 = (dataRow["Artikelbezeichnung2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelbezeichnung2"]);
			Bestell_Nr = (dataRow["Bestell-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestell-Nr"]);
			Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufspreis"]);
			Einkaufspreis1 = (dataRow["Einkaufspreis1"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufspreis1"]);
			Einkaufspreis2 = (dataRow["Einkaufspreis2"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufspreis2"]);
			Angebot = (dataRow["Angebot"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Angebot"]);
			Angebot_Datum = (dataRow["Angebot_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Angebot_Datum"]);
			Einkaufspreis1_gultig_bis = (dataRow["Einkaufspreis1 gültig bis"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Einkaufspreis1 gültig bis"]);
			Einkaufspreis2_gultig_bis = (dataRow["Einkaufspreis2 gültig bis"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Einkaufspreis2 gültig bis"]);
			Telefon = (dataRow["Telefon"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Telefon"]);
			Fax = (dataRow["Fax"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Fax"]);
			eMail = (dataRow["eMail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["eMail"]);
			Mindestbestellmenge = (dataRow["Mindestbestellmenge"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Mindestbestellmenge"]);
			Wiederbeschaffungszeitraum = (dataRow["Wiederbeschaffungszeitraum"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Wiederbeschaffungszeitraum"]);
		}
	}
	public class LPMinimalUpdateEntity
	{
		public bool EinkaufspreisChanged { get; set; } = default;
		public bool AngebotChanged { get; set; } = default;
		public bool MindestbestellmengeChanged { get; set; } = default;
		public bool WiederbeschaffungszeitraumChanged { get; set; } = default;
		public bool Angebot_DatumChanged { get; set; } = default;
		public bool Bestell_NrChanged { get; set; } = default;

		// Logs
		public string EinkaufspreisChangesLog { get; set; } = default;
		public string AngebotChangesLog { get; set; } = default;
		public string MindestbestellmengeChangesLog { get; set; } = default;
		public string WiederbeschaffungszeitraumChangesLog { get; set; } = default;
		public string Angebot_DatumChangesLog { get; set; } = default;
		public string Bestell_NrChangesLog { get; set; } = default;
		public decimal? Einkaufspreis1 { get; set; }
		public decimal? Einkaufspreis2 { get; set; }
		public DateTime? Einkaufspreis1_gultig_bis { get; set; }
		public DateTime? Einkaufspreis2_gultig_bis { get; set; }
		public int ArtikelNr { get; set; }
		public int Nr { get; set; }
		[Required]
		public string Artikelnummer { get; set; }
		public string Name1 { get; set; }
		public string StandardLiefrentenName { get; set; }
		public string errors { get; set; } = string.Empty;
		public bool? Standardlieferant { get; set; }
		public string Artikelbezeichnung { get; set; }
		public string Artikelbezeichnung2 { get; set; }
		[Required]
		public string Bestell_Nr { get; set; }
		[Required]
		public decimal? Einkaufspreis { get; set; }
		[Required]
		public string Angebot { get; set; }
		[Required]
		public DateTime? Angebot_Datum { get; set; }
		public string Telefon { get; set; }
		public string Fax { get; set; }
		public string eMail { get; set; }
		[Required]
		public int? Mindestbestellmenge { get; set; }
		[Required]
		public int? Wiederbeschaffungszeitraum { get; set; }
		public LPMinimalUpdateEntity()
		{

		}
		public LPMinimalUpdateEntity(Infrastructure.Data.Entities.Joins.Kunden_Lieferanten_LPEntities LPEntity)
		{
			Nr = LPEntity.Nr;
			ArtikelNr = LPEntity.ArtikelNr;
			Artikelnummer = LPEntity.Artikelnummer;
			StandardLiefrentenName = LPEntity.StandardLiefrentenName;
			Name1 = LPEntity.Name1;
			Standardlieferant = LPEntity.Standardlieferant;
			Artikelbezeichnung = LPEntity.Artikelbezeichnung;
			Artikelbezeichnung2 = LPEntity.Artikelbezeichnung2;
			Bestell_Nr = LPEntity.Bestell_Nr;
			Einkaufspreis = LPEntity.Einkaufspreis;
			Angebot = LPEntity.Angebot;
			Angebot_Datum = LPEntity.Angebot_Datum;
			Telefon = LPEntity.Telefon;
			Fax = LPEntity.Fax;
			eMail = LPEntity.eMail;
			Mindestbestellmenge = LPEntity.Mindestbestellmenge;
			Wiederbeschaffungszeitraum = LPEntity.Wiederbeschaffungszeitraum;
			Einkaufspreis1 = LPEntity.Einkaufspreis1;
			Einkaufspreis2 = LPEntity.Einkaufspreis2;
			Einkaufspreis1_gultig_bis = LPEntity.Einkaufspreis1_gultig_bis;
			Einkaufspreis2_gultig_bis = LPEntity.Einkaufspreis2_gultig_bis;
		}
	}

}
