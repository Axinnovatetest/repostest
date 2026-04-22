using Psz.Core.BaseData.Handlers;
using Psz.Core.Identity.Models;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Article.Purchase
{
	public class GetModel
	{
		public string Angebot { get; set; }
		public string Angebot_Datum { get; set; }
		public string Artikelbezeichnung { get; set; }
		public string Artikelbezeichnung2 { get; set; }
		public int? ArtikelNr { get; set; }
		public double? Basispreis { get; set; }
		public string Bemerkungen { get; set; }
		public string Bestell_Nr { get; set; }
		public double? Einkaufspreis { get; set; }
		public string Einkaufspreis_gultig_bis { get; set; }
		public double? Einkaufspreis1 { get; set; }
		public string Einkaufspreis1_gultig_bis { get; set; }
		public double? Einkaufspreis2 { get; set; }
		public string Einkaufspreis2_gultig_bis { get; set; }
		public double? EK_EUR { get; set; }
		public double? EK_total { get; set; }
		public double? Fracht { get; set; }
		public DateTime? letzte_Aktualisierung { get; set; }
		public int? Lieferanten_Nr { get; set; }
		public double? Logistik { get; set; }
		public double? Mindestbestellmenge { get; set; }
		public int Nr { get; set; }
		public double? Preiseinheit { get; set; }
		public int? Pruftiefe_WE { get; set; }
		public decimal? Rabatt { get; set; }
		public bool? Standardlieferant { get; set; }
		public double? Umsatzsteuer { get; set; }
		public double? Verpackungseinheit { get; set; }
		public string Warengruppe { get; set; }
		public int? Wiederbeschaffungszeitraum { get; set; }
		public double? Zoll { get; set; }
		public double? Zusatz { get; set; }
		public bool SetToStandardSupplier { get; set; }
		public GetModel()
		{

		}
		public GetModel(Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity entity)
		{
			if(entity == null)
				return;

			Angebot = entity.Angebot;
			Angebot_Datum = entity.Angebot_Datum?.ToString("dd/MM/yyyy");
			Artikelbezeichnung = entity.Artikelbezeichnung;
			Artikelbezeichnung2 = entity.Artikelbezeichnung2;
			ArtikelNr = entity.ArtikelNr;
			Basispreis = entity.Basispreis;
			Bemerkungen = entity.Bemerkungen;
			Bestell_Nr = entity.Bestell_Nr;
			Einkaufspreis = entity.Einkaufspreis;
			Einkaufspreis_gultig_bis = entity.Einkaufspreis_gultig_bis?.ToString("dd/MM/yyyy");
			Einkaufspreis1 = entity.Einkaufspreis1;
			Einkaufspreis1_gultig_bis = entity.Einkaufspreis1_gultig_bis?.ToString("dd/MM/yyyy");
			Einkaufspreis2 = entity.Einkaufspreis2;
			Einkaufspreis2_gultig_bis = entity.Einkaufspreis2_gultig_bis?.ToString("dd/MM/yyyy");
			EK_EUR = entity.EK_EUR;
			EK_total = entity.EK_total;
			Fracht = entity.Fracht;
			letzte_Aktualisierung = entity.letzte_Aktualisierung;
			Lieferanten_Nr = entity.Lieferanten_Nr;
			Logistik = entity.Logistik;
			Mindestbestellmenge = entity.Mindestbestellmenge;
			Nr = entity.Nr;
			Preiseinheit = entity.Preiseinheit;
			Pruftiefe_WE = entity.Pruftiefe_WE;
			Rabatt = entity.Rabatt;
			Standardlieferant = entity.Standardlieferant;
			Umsatzsteuer = entity.Umsatzsteuer;
			Verpackungseinheit = entity.Verpackungseinheit;
			Warengruppe = entity.Warengruppe;
			Wiederbeschaffungszeitraum = entity.Wiederbeschaffungszeitraum;
			Zoll = entity.Zoll;
			Zusatz = entity.Zusatz;
		}
		public Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity ToEntity(
			Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity prevEntity,
			UserModel user,
			Enums.ObjectLogEnums.Objects objectItem,
			int objectItemId,
			List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> objectLogs,
			Enums.ObjectLogEnums.LogType logType = Enums.ObjectLogEnums.LogType.Edit)
		{
			if(prevEntity != null)
			{
				if(objectLogs == null)
					objectLogs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();

				if(prevEntity.Angebot != Angebot)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Angebot", $"{prevEntity.Angebot}",
									$"{Angebot}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Angebot_Datum?.ToString("dd/MM/yyyy") != Angebot_Datum)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Angebot_Datum", $"{prevEntity.Angebot_Datum}",
									$"{Angebot_Datum}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.ArtikelNr != ArtikelNr)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"ArtikelNr", $"{prevEntity.ArtikelNr}",
									$"{ArtikelNr}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Basispreis != Basispreis)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Basispreis", $"{prevEntity.Basispreis}",
									$"{Basispreis}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Bestell_Nr != Bestell_Nr)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Bestell_Nr", $"{prevEntity.Bestell_Nr}",
									$"{Bestell_Nr}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Einkaufspreis != Einkaufspreis)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Einkaufspreis", $"{prevEntity.Einkaufspreis}",
									$"{Einkaufspreis}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Einkaufspreis_gultig_bis?.ToString("dd/MM/yyyy") != Einkaufspreis_gultig_bis)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Einkaufspreis_gultig_bis", $"{prevEntity.Einkaufspreis_gultig_bis}",
									$"{Einkaufspreis_gultig_bis}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Einkaufspreis1 != Einkaufspreis1)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Einkaufspreis1", $"{prevEntity.Einkaufspreis1}",
									$"{Einkaufspreis1}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Einkaufspreis1_gultig_bis?.ToString("dd/MM/yyyy") != Einkaufspreis1_gultig_bis)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Einkaufspreis1_gultig_bis", $"{prevEntity.Einkaufspreis1_gultig_bis}",
									$"{Einkaufspreis1_gultig_bis}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Einkaufspreis2 != Einkaufspreis2)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Einkaufspreis2", $"{prevEntity.Einkaufspreis2}",
									$"{Einkaufspreis2}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Einkaufspreis2_gultig_bis?.ToString("dd/MM/yyyy") != Einkaufspreis2_gultig_bis)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Einkaufspreis2_gultig_bis", $"{prevEntity.Einkaufspreis2_gultig_bis}",
									$"{Einkaufspreis2_gultig_bis}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.EK_EUR != EK_EUR)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"EK_EUR", $"{prevEntity.EK_EUR}",
									$"{EK_EUR}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.EK_total != EK_total)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"EK_total", $"{prevEntity.EK_total}",
									$"{EK_total}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Fracht != Fracht)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Fracht", $"{prevEntity.Fracht}",
									$"{Fracht}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Lieferanten_Nr != Lieferanten_Nr)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Lieferanten_Nr", $"{prevEntity.Lieferanten_Nr}",
									$"{Lieferanten_Nr}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Logistik != Logistik)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Logistik", $"{prevEntity.Logistik}",
									$"{Logistik}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Mindestbestellmenge != Mindestbestellmenge)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Mindestbestellmenge", $"{prevEntity.Mindestbestellmenge}",
									$"{Mindestbestellmenge}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Preiseinheit != Preiseinheit)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Preiseinheit", $"{prevEntity.Preiseinheit}",
									$"{Preiseinheit}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Pruftiefe_WE != Pruftiefe_WE)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Pruftiefe_WE", $"{prevEntity.Pruftiefe_WE}",
									$"{Pruftiefe_WE}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Rabatt != Rabatt)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Rabatt", $"{prevEntity.Rabatt}",
									$"{Rabatt}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Standardlieferant != Standardlieferant)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Standardlieferant", $"{prevEntity.Standardlieferant}",
									$"{Standardlieferant}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Umsatzsteuer != Umsatzsteuer)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Umsatzsteuer", $"{prevEntity.Umsatzsteuer}",
									$"{Umsatzsteuer}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Verpackungseinheit != Verpackungseinheit)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Verpackungseinheit", $"{prevEntity.Verpackungseinheit}",
									$"{Verpackungseinheit}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Warengruppe != Warengruppe)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Warengruppe", $"{prevEntity.Warengruppe}",
									$"{Warengruppe}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Wiederbeschaffungszeitraum != Wiederbeschaffungszeitraum)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Wiederbeschaffungszeitraum", $"{prevEntity.Wiederbeschaffungszeitraum}",
									$"{Wiederbeschaffungszeitraum}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Zoll != Zoll)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Zoll", $"{prevEntity.Zoll}",
									$"{Zoll}", $"{objectItem.GetDescription()}", logType));
				}
				if(prevEntity.Zusatz != Zusatz)
				{
					objectLogs.Add(ObjectLogHelper.getLog(user, objectItemId, $"Zusatz", $"{prevEntity.Zusatz}",
									$"{Zusatz}", $"{objectItem.GetDescription()}", logType));
				}
			}
			// -

			return new Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity
			{
				Angebot = Angebot,
				Angebot_Datum = string.IsNullOrWhiteSpace(Angebot_Datum) ? null : DateTime.TryParseExact(Angebot_Datum?.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var d) == true ? d : null,
				Artikelbezeichnung = Artikelbezeichnung,
				Artikelbezeichnung2 = Artikelbezeichnung2,
				ArtikelNr = ArtikelNr,
				Basispreis = Basispreis,
				Bemerkungen = Bemerkungen,
				Bestell_Nr = Bestell_Nr,
				Einkaufspreis = Einkaufspreis,
				Einkaufspreis_gultig_bis = string.IsNullOrWhiteSpace(Angebot_Datum) ? null : DateTime.TryParseExact(Einkaufspreis_gultig_bis?.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var d1) == true ? d1 : null,
				Einkaufspreis1 = Einkaufspreis1,
				Einkaufspreis1_gultig_bis = string.IsNullOrWhiteSpace(Angebot_Datum) ? null : DateTime.TryParseExact(Einkaufspreis1_gultig_bis?.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var d2) == true ? d2 : null,
				Einkaufspreis2 = Einkaufspreis2,
				Einkaufspreis2_gultig_bis = string.IsNullOrWhiteSpace(Angebot_Datum) ? null : DateTime.TryParseExact(Einkaufspreis2_gultig_bis?.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var d3) == true ? d3 : null,
				EK_EUR = EK_EUR,
				EK_total = EK_total,
				Fracht = Fracht,
				letzte_Aktualisierung = letzte_Aktualisierung,
				Lieferanten_Nr = Lieferanten_Nr,
				Logistik = Logistik,
				Mindestbestellmenge = Mindestbestellmenge,
				Nr = Nr,
				Preiseinheit = Preiseinheit,
				Pruftiefe_WE = Pruftiefe_WE,
				Rabatt = Rabatt ?? 0,
				Standardlieferant = Standardlieferant,
				Umsatzsteuer = Umsatzsteuer,
				Verpackungseinheit = Verpackungseinheit,
				Warengruppe = Warengruppe,
				Wiederbeschaffungszeitraum = Wiederbeschaffungszeitraum,
				Zoll = Zoll,
				Zusatz = Zusatz,
			};
		}
	}

	public class GetMinimalRequestModel
	{
		public int? Lieferanten_Nr { get; set; }
		public int? ArtikelNr { get; set; }
	}
	public class OfferDateMinimalRequestModel
	{
		public int? Lieferanten_Nr { get; set; }
		public int? ArtikelNr { get; set; }
		public DateTime? OfferDate { get; set; }
	}
}



