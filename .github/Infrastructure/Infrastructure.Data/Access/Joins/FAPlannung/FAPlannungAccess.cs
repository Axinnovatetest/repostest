using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace Infrastructure.Data.Access.Joins.FAPlannung
{
	using MoreLinq;
	public class FAPlannungAccess
	{
		public static IEnumerable<KeyValuePair<int, decimal>> GetLagersProduktionPlannungControlledQty(int? lager, IEnumerable<int> fas)
		{
			int batchSize = 5000;
			if(fas.Count() > batchSize)
			{
				IEnumerable<KeyValuePair<int, decimal>> results = new List<KeyValuePair<int, decimal>>();
				foreach(var batch in fas.Batch(batchSize))
				{
					var r = getLagersProduktionPlannungControlledQty(lager, batch);
					if(r?.Count() > 0)
					{
						results = Enumerable.Concat(results, r);
					}
				}

				// -
				return results;
			}
			else
			{
				return getLagersProduktionPlannungControlledQty(lager, fas);
			}
		}
		internal static IEnumerable<KeyValuePair<int, decimal>> getLagersProduktionPlannungControlledQty(int? lager, IEnumerable<int> fas)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"";
				string fasList = string.Join(",", fas);
				var lagers = new Dictionary<int, string>
				{
					{ 6, "[PSZ_Einlagerung_täglich]" },
					{ 7,"[PSZTN_Lieferliste täglich]" },
					{ 15,"[PSZ_Einlagerung_täglich]" },
					{ 26, "[PSZ_Lieferliste täglich]"},
					{ 42,"[PSZGZTN_Lieferliste täglich]"},
					{ 60,"[PSZKsarHelal_Lieferliste täglich]" },
					{ 102, "[PSZAL_Lieferliste täglich]" }
				};
				if(lager.HasValue && lagers.ContainsKey(lager.Value))
				{
					query = $@"SELECT Fertigungsnummer, isnull(SUM([Anzahl_aktuelle Lieferung]),0) nb FROM {lagers.GetValueOrDefault(lager.Value)} WHERE Fertigungsnummer IN ({fasList}) GROUP BY Fertigungsnummer";
				}
				else
				{
					query = string.Join(" UNION ALL ", lagers.AsEnumerable().Select(x => $"SELECT Fertigungsnummer, isnull(SUM([Anzahl_aktuelle Lieferung]),0) nb FROM {x.Value} WHERE Fertigungsnummer IN ({fasList}) GROUP BY Fertigungsnummer"));
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 180;
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, decimal>(int.TryParse(x[0].ToString(), out int k) ? k : 0, decimal.TryParse(x[1].ToString(), out decimal v) ? v : 0));
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.FAPlannung.FAProduktionPlannungEntity> GetProduktionPlannung(string articleNmuber, string productionDate,
			string lager, bool technik)
		{
			if(string.IsNullOrWhiteSpace(articleNmuber) == true)
			{
				articleNmuber = "";
			}

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT Fertigung.Planungsstatus AS [Stav Planovani/Status], [View_PSZ_Artikel Kundenzuweisung2].Kunde AS [Zákaznik/Kunde],
                               Fertigung.Kennzeichen AS Atribut, Fertigung.Fertigungsnummer AS [?islo Zakázky/FA#],Fertigung.[Prio],
                               Fertigung.Termin_Fertigstellung AS [Termin Zákaznika/Kundentermin],
                               Fertigung.Termin_Bestätigt1 AS [Termin Výroba/Plantermin],
                               Fertigung.[Bemerkung II Planung] AS [Komentá? 1/Bemerkung1],
                               Fertigung.Bemerkung_Planung AS [Komentá? 2/Bemerkung2],
                               Fertigung.Quick_Area AS Sonderfertigung, Fertigung.Bemerkung AS [Komentá? ZS/Bemerkung CS],
                               Fertigung.Originalanzahl AS [Original Množství/Originalmenge],
                               Fertigung.Anzahl_erledigt AS [Vyvezené Množství/Menge erledigt],
                               Fertigung.Anzahl AS [Ot Množství/Menge offen], Artikel.Sysmonummer AS [?islo Sysmo/Sysmo#],
                               Artikel.Artikelnummer AS [?islo PSZ/PSZ Nummer], Artikel.Freigabestatus AS [Stav/Freigabestatus],
                               (Fertigung.Anzahl*Artikel.Produktionszeit/60) AS [?as na Zakázku/FA Zeit],
                               (Fertigung.Anzahl*Fertigung.Preis) AS [Peníze/FA Lohn], Fertigung.Menge1 AS [Vyvezené Množství man],
                               Artikel.Index_Kunde AS [Index], Artikel.Index_Kunde_Datum AS Indexdatum, Fertigung.Technik
                               FROM Fertigung LEFT JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]
                               LEFT JOIN [View_PSZ_Artikel Kundenzuweisung2] ON Artikel.Artikelnummer = [View_PSZ_Artikel Kundenzuweisung2].Artikelnummer
                               WHERE (((Fertigung.Kennzeichen)='Offen') AND ((Fertigung.Termin_Bestätigt1)<=@productionDate) 
                               AND ((Artikel.Artikelnummer) Like (@articleNmuber)) 
                               AND ((Fertigung.Technik)=@technik) 
                               AND ((Fertigung.Lagerort_id) LIKE @lager) AND ((Fertigung.Erstmuster)=0))
                               ORDER BY Fertigung.Planungsstatus, [View_PSZ_Artikel Kundenzuweisung2].Kunde, Fertigung.Termin_Bestätigt1";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNmuber", articleNmuber + "%");
				sqlCommand.Parameters.AddWithValue("productionDate", productionDate);
				sqlCommand.Parameters.AddWithValue("lager", lager);
				sqlCommand.Parameters.AddWithValue("technik", technik);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FAPlannung.FAProduktionPlannungEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.FAPlannung.FALagersPlannungEntity> GetLagersProduktionPlannung(string productionDate, string lager)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT Fertigung.Lagerort_id AS Werk, Fertigung.Planungsstatus, 
                               [View_PSZ_Artikel Kundenzuweisung2].Kunde AS Customer, [View_PSZ_Artikel Kundenzuweisung2].[CS Kontakt],
                               Artikel.Halle AS PB, Fertigung.Kennzeichen AS Atribut, Artikel.Artikelkurztext AS [Short],
                               Fertigung.Fertigungsnummer AS [FA Number],Fertigung.[Prio],Fertigung.[Bemerkung II Planung] AS [Comment 1],
                               Fertigung.Bemerkung_Planung AS [Comment 2], Fertigung.Originalanzahl AS [FA Qty],
                               Fertigung.Anzahl_erledigt AS [Shipped Qty], Fertigung.Anzahl AS [Open Qty],
                               Artikel.Artikelnummer AS [PN PSZ], Artikel.[Freigabestatus TN intern] AS [Status TN],
                               (Fertigung.Anzahl*Fertigung.Zeit/60) AS [Order Time], (Fertigung.Anzahl*Fertigung.Preis) AS Costs,
                               Fertigung.Menge1 AS [Shipped Qty Man], Fertigung.Kommisioniert_teilweise, Fertigung.Kommisioniert_komplett,
                               Fertigung.Kabel_geschnitten, Fertigung.Kabel_geschnitten_Datum, Fertigung.Termin_Bestätigt2 AS [Termin Werk],
                               Fertigung.Termin_Bestätigt1 AS [Ack Date], datepart(ISO_WEEK, [Termin_Bestätigt1]) AS KW, Fertigung.FA_Druckdatum,
                               Artikel.Freigabestatus, Fertigung.Termin_Fertigstellung AS [Wish Date], Fertigung.Bemerkung, Fertigung.Gewerk_Teilweise_Bemerkung,
                               Artikel.Verpackungsart, Artikel.Verpackungsmenge, Artikel.Losgroesse, Fertigung.Techniker, [View_PSZ_Artikel Kundenzuweisung2].[Technik Kontakt],
                               [View_PSZ_Artikel Kundenzuweisung2].[Technik Kontakt TN], [View_PSZ_Artikel Kundenzuweisung2].[Freigabestatus TN intern] AS [Status Intern],
                               Fertigung.Datum AS erstelldatum, Fertigung.Bemerkung_Kommissionierung_AL, FertigungType
                               FROM (Fertigung LEFT JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
                               LEFT JOIN [View_PSZ_Artikel Kundenzuweisung2] ON Artikel.Artikelnummer = [View_PSZ_Artikel Kundenzuweisung2].Artikelnummer
                               WHERE (((Fertigung.Lagerort_id) LIKE @lager) 
                               AND ((Fertigung.Kennzeichen)='Offen') AND ((Fertigung.Termin_Bestätigt1)<=@productionDate))
                               ORDER BY Fertigung.Planungsstatus, [View_PSZ_Artikel Kundenzuweisung2].Kunde, Fertigung.Termin_Bestätigt1;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("productionDate", productionDate);
				sqlCommand.Parameters.AddWithValue("lager", string.IsNullOrWhiteSpace(lager) ? "%" : lager);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FAPlannung.FALagersPlannungEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.FAPlannung.FATechnikPlanungEntity> GetTechnickPlannung(string technicien, string lager)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Artikel.[Artikel-Nr] [ArtikelNr], Fertigung.Lagerort_id, Fertigung.Erstmuster, Fertigung.Quick_Area AS Sonderfertigung,
                               Fertigung.Techniker, [angebotene Artikel].Liefertermin AS AB_Termin, Fertigung.Termin_Bestätigt1 AS [Plan],
                               '' AS [Termin besprochen], Artikel.Artikelnummer AS [PSZ], Fertigung.Originalanzahl AS Menge,
                               Fertigung.Anzahl AS Offen_Anzahl, Fertigung.Fertigungsnummer AS FA, [Zeit]/1 AS [Zeit in min pro Stück],
                               Artikel.Freigabestatus AS Status, Artikel.[Prüfstatus TN Ware], Artikel.[Freigabestatus TN intern] AS [Status intern],
                               Fertigung.Bemerkung_Technik, Fertigung.Bemerkung AS [Info CS], Fertigung.Quick_Area, Fertigung.Kommisioniert_teilweise,
                               Fertigung.Kommisioniert_komplett, Fertigung.Kabel_geschnitten, Fertigung.Kabel_geschnitten_Datum, Fertigung.FA_Gestartet,
                               Fertigung.[Urs-Artikelnummer]
                               FROM (Fertigung LEFT JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
                               LEFT JOIN [angebotene Artikel] ON Fertigung.Angebot_Artikel_Nr = [angebotene Artikel].Nr
                               WHERE 
                               {(!string.IsNullOrEmpty(lager) && !string.IsNullOrWhiteSpace(lager) ? "Fertigung.Lagerort_id = @lager" : "Fertigung.Lagerort_id LIKE '%%'")}
                               AND (Fertigung.Quick_Area IS NULL OR Fertigung.Quick_Area = 0)
                               AND Fertigung.Techniker Like @technicien
                               AND Fertigung.Technik=1 AND Fertigung.Kennzeichen=N'offen'
                               ORDER BY Fertigung.Techniker, Fertigung.Termin_Bestätigt1, Artikel.Artikelnummer;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("technicien", "%" + technicien + "%");
				if(!string.IsNullOrEmpty(lager) && !string.IsNullOrWhiteSpace(lager))
					sqlCommand.Parameters.AddWithValue("lager", lager);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FAPlannung.FATechnikPlanungEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.FAPlannung.FAKommisionertEntity> GetFAKommisionert(DateTime date)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT Fertigung.Termin_Bestätigt1 AS [Geplanter Termin], Artikel.Artikelnummer,
                               Fertigung.Fertigungsnummer, Artikel.Halle, Fertigung.Kommisioniert_teilweise AS [Teilweise kommisioniert],
                               Fertigung.Originalanzahl AS [FA Menge], Fertigung.Anzahl_erledigt AS Erledigt, Artikel.[Bezeichnung 1],
                               Artikel.Artikelkurztext, Fertigung.Bemerkung
                               FROM Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]
                               WHERE (((Fertigung.Termin_Bestätigt1)<=@date) AND ((Artikel.Artikelnummer)<>'Reparatur')
                               AND ((Fertigung.Kommisioniert_komplett)=0) AND 
                               ((Fertigung.gebucht)=1) AND ((Fertigung.Kennzeichen)='Offen')
                               AND ((Fertigung.Lagerort_id)=7))
                               ORDER BY Fertigung.Termin_Bestätigt1, Artikel.Artikelnummer, Fertigung.Fertigungsnummer;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("date", date);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FAPlannung.FAKommisionertEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.FAPlannung.AuswertungEndkontrolleEntity> GetAuswertingEndkontrolle()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT Artikel.Artikelnummer, Fertigung.Originalanzahl AS GesamtMenge,
                               Fertigung.Anzahl AS MengeOffen, Fertigung.Fertigungsnummer,
                               Fertigung.Lagerort_id, Fertigung.Datum, Fertigung.[Urs-Artikelnummer],
                               Fertigung.[Urs-Fa], Fertigung.Endkontrolle, Fertigung.Kennzeichen, Fertigung.Termin_Bestätigt1
                               FROM Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]
                               WHERE (((Fertigung.Endkontrolle)=1) AND ((Fertigung.Kennzeichen)='Offen'));";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FAPlannung.AuswertungEndkontrolleEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.FAPlannung.LaufkarteSchneidereiEntity> GetLaufkarteSchneiderei(int fa)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT [PSZ_FA Klassifizierung laufkarte aus FA].Klassifizierung, [PSZ_FA Klassifizierung laufkarte aus FA].Gewerk,
                  [PSZ_FA Klassifizierung laufkarte aus FA].Artikelnummer1 as Artikelnummer,
      [PSZ_FA Klassifizierung laufkarte aus FA].anz1 as Anzahl,
      [PSZ_FA Klassifizierung laufkarte aus FA].Termin_Bestätigt1,      [PSZ_FA Klassifizierung laufkarte aus FA].Fertigungsnummer,
      [PSZ_FA Klassifizierung laufkarte aus FA].Bezeichnung,
      [PSZ_FA Klassifizierung laufkarte aus FA].[bez11] AS FGArtikelBZ1,
      [PSZ_FA Klassifizierung laufkarte aus FA].Artikelfamilie_Kunde
              FROM(
      SELECT Artikel_1.Artikelnummer as Artikelnummer1, Artikel_1.[Bezeichnung 1] as bez11, Artikel_1.[Bezeichnung 2] as bez22,
      Fertigung.Anzahl as anz1, Lagerorte.Lagerort as Lagerort1, Fertigung.Fertigungsnummer, Fertigung.Datum,
      Fertigung.Termin_Fertigstellung, Fertigung.Kennzeichen, Fertigung.Bemerkung,
      Artikel.Artikelnummer, Artikel.[Bezeichnung 1], Artikel.[Bezeichnung 2],
      Fertigung_Positionen.Anzahl, Fertigung_Positionen.Arbeitsanweisung, Fertigung_Positionen.Fertiger,
      Fertigung_Positionen.Termin_Soll, Fertigung_Positionen.Bemerkungen, Lagerorte_1.Lagerort,
      Artikel_1.EAN, artikel_kalkulatorische_kosten.Betrag, Artikel_1.Freigabestatus,
      Fertigung.Zeit AS Produktionszeit, Fertigung.Termin_Bestätigt1, Fertigung.Erstmuster,
      Artikel_1.[Freigabestatus TN intern], Artikel_1.Index_Kunde, Fertigung.[Lagerort_id zubuchen],
      Fertigung.Mandant, Artikel_1.Sysmonummer as Sysmonummer1, Artikel.Sysmonummer, Artikel_1.[UL Etikett],
      Fertigung.Technik, Fertigung.Techniker, Artikel_1.Kanban, Artikel_1.Verpackungsart,
      Artikel_1.Verpackungsmenge, Artikel_1.Losgroesse, Fertigung.Quick_Area, Artikel_1.Artikelfamilie_Kunde as Artikelfamilie_Kunde1,
      Artikel_1.Artikelfamilie_Kunde_Detail1, Artikel_1.Artikelfamilie_Kunde_Detail2, Artikel.Klassifizierung,
      Artikelstamm_Klassifizierung.Bezeichnung, Artikelstamm_Klassifizierung.Nummernkreis,
      Artikelstamm_Klassifizierung.Kupferzahl, Artikelstamm_Klassifizierung.ID, Artikelstamm_Klassifizierung.Gewerk,
      Artikel_1.Artikelfamilie_Kunde FROM((Lagerorte AS Lagerorte_1 INNER JOIN((Artikel AS Artikel_1 INNER JOIN(Artikel INNER JOIN(Fertigung INNER JOIN Fertigung_Positionen ON Fertigung.ID = Fertigung_Positionen.ID_Fertigung) ON Artikel.[Artikel-Nr] = Fertigung_Positionen.Artikel_Nr) ON Artikel_1.[Artikel-Nr] = Fertigung.Artikel_Nr) INNER JOIN Lagerorte ON Fertigung.Lagerort_id = Lagerorte.Lagerort_id) ON Lagerorte_1.Lagerort_id = Fertigung_Positionen.Lagerort_ID) LEFT JOIN artikel_kalkulatorische_kosten ON Artikel_1.[Artikel-Nr] = artikel_kalkulatorische_kosten.[Artikel-Nr]) INNER JOIN Artikelstamm_Klassifizierung ON Artikel.ID_Klassifizierung = Artikelstamm_Klassifizierung.ID WHERE Fertigung.Fertigungsnummer = @fa AND Artikelstamm_Klassifizierung.Gewerk Is Not Null
              )[PSZ_FA Klassifizierung laufkarte aus FA]
                GROUP BY[PSZ_FA Klassifizierung laufkarte aus FA].Klassifizierung,
      [PSZ_FA Klassifizierung laufkarte aus FA].Gewerk,
      [PSZ_FA Klassifizierung laufkarte aus FA].Artikelnummer1,
      [PSZ_FA Klassifizierung laufkarte aus FA].anz1,
      [PSZ_FA Klassifizierung laufkarte aus FA].Termin_Bestätigt1,
      [PSZ_FA Klassifizierung laufkarte aus FA].Fertigungsnummer,
      [PSZ_FA Klassifizierung laufkarte aus FA].Bezeichnung,
      [PSZ_FA Klassifizierung laufkarte aus FA].[bez11],
      [PSZ_FA Klassifizierung laufkarte aus FA].Artikelfamilie_Kunde
              ORDER BY[PSZ_FA Klassifizierung laufkarte aus FA].Gewerk";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("fa", fa);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FAPlannung.LaufkarteSchneidereiEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Joins.FAUpdate.FAErlidigtEntity GetFAErlidgit(int faId, string username)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT Fertigung.Fertigungsnummer, Artikel.Artikelnummer, Artikel.[Bezeichnung 1],
                               Fertigung.Anzahl, Fertigung.Originalanzahl, GETDATE() AS Termin_Fertigstellung,
                               Fertigung.Anzahl_erledigt,@username AS Mitarbeiter,
                               Lagerorte.Lagerort, Fertigung.Anzahl AS Anzahl_aktuell, Fertigung.Lagerort_id,
                               'erledigt' AS Kennzeichen, Fertigung.Anzahl AS [Faktor Material], IIf([Lagerorte].[Lagerort]='SC CZ','CZ',
                               IIf([Lagerorte].[Lagerort]='BE_TN','PLBE',IIf([Lagerorte].[Lagerort]='WS','PLWS',
                               IIf([Lagerorte].[Lagerort]='TN','PLTN',IIf([Lagerorte].[Lagerort]='Eigenfertigung','PLCZ',[Lagerorte].[Lagerort]))))) AS Lagerort01,
                               IIf(Fertigung.Lagerort_id=21,6,IIf(Fertigung.Lagerort_id=42,420,IIf(Fertigung.Lagerort_id=60,580,IIf(Fertigung.Lagerort_id=102,103,
                               IIf(Fertigung.Lagerort_id=6,66,IIf(Fertigung.Lagerort_id=7,77,Fertigung.Lagerort_id)))))) AS Lage, Fertigung.Zeit
                               FROM Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]
                               INNER JOIN Lagerorte ON Fertigung.Lagerort_id = Lagerorte.Lagerort_id
                               WHERE (((Fertigung.ID)=@fa));";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("fa", faId);
				sqlCommand.Parameters.AddWithValue("username", $"{username} {DateTime.Now}");
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Joins.FAUpdate.FAErlidigtEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<int> GetFAUpdateLagers(int articleNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT DISTINCT Fertigung.Lagerort_id --INTO Liste_Autrage_Offnen_Lager_T
FROM Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]
WHERE (((Fertigung.Lagerort_id)=42) AND ((Fertigung.Kennzeichen)='Offen') 
AND ((Fertigung.Artikel_Nr)=@articleNr) 
AND ((Fertigung.Anzahl_erledigt)<=0) AND ((Fertigung.FA_Gestartet)=0 Or (Fertigung.FA_Gestartet) Is Null)) OR (((Fertigung.Lagerort_id)<>42 
And (Fertigung.Lagerort_id)<>60 And (Fertigung.Lagerort_id)<>102 And (Fertigung.Lagerort_id)<>7 And (Fertigung.Lagerort_id)<>6 And (Fertigung.Lagerort_id)<>21) 
AND ((Fertigung.Kennzeichen)='Offen') AND ((Fertigung.Artikel_Nr)=@articleNr) 
AND ((Fertigung.Anzahl_erledigt)<=0)) OR (((Fertigung.Lagerort_id)=60) AND ((Fertigung.Kennzeichen)='Offen') 
AND ((Fertigung.Artikel_Nr)=@articleNr) AND ((Fertigung.Anzahl_erledigt)<=0) 
AND ((Fertigung.FA_Gestartet)=0 Or (Fertigung.FA_Gestartet) Is Null)) OR (((Fertigung.Lagerort_id)=7) 
AND ((Fertigung.Kennzeichen)='Offen') AND ((Fertigung.Artikel_Nr)=@articleNr) 
AND ((Fertigung.Anzahl_erledigt)<=0) AND ((Fertigung.FA_Gestartet)=0 Or (Fertigung.FA_Gestartet) Is Null)) OR (((Fertigung.Lagerort_id)=6) 
AND ((Fertigung.Kennzeichen)='Offen') AND ((Fertigung.Artikel_Nr)=@articleNr) 
AND ((Fertigung.Anzahl_erledigt)<=0) AND ((Fertigung.FA_Gestartet)=0 Or (Fertigung.FA_Gestartet) Is Null)) OR (((Fertigung.Lagerort_id)=21) 
AND ((Fertigung.Kennzeichen)='Offen') AND ((Fertigung.Artikel_Nr)=@articleNr) 
AND ((Fertigung.Anzahl_erledigt)<=0) AND ((Fertigung.FA_Gestartet)=0 Or (Fertigung.FA_Gestartet) Is Null));";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => Convert.ToInt32(x["Lagerort_id"])).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<string> GetFAUpdateIndexes(int articleNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT [__BSD_Stucklisten_Snapshot].KundenIndex
                               FROM [__BSD_Stucklisten_Snapshot]
                               GROUP BY [__BSD_Stucklisten_Snapshot].KundenIndex, [__BSD_Stucklisten_Snapshot].[Artikel-Nr]
                               HAVING [__BSD_Stucklisten_Snapshot].[Artikel-Nr]=@articleNr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => Convert.ToString(x["KundenIndex"])).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<int> GetFAUpdateBOMVersions(int articleNr, string index)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT [__BSD_Stucklisten_Snapshot].BomVersion
                               FROM [__BSD_Stucklisten_Snapshot]
                               GROUP BY [__BSD_Stucklisten_Snapshot].BomVersion, [__BSD_Stucklisten_Snapshot].[Artikel-Nr], [__BSD_Stucklisten_Snapshot].KundenIndex
                               HAVING [__BSD_Stucklisten_Snapshot].[Artikel-Nr]=@articleNr
                               AND [__BSD_Stucklisten_Snapshot].KundenIndex=@index";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				sqlCommand.Parameters.AddWithValue("index", index);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => Convert.ToInt32(x["BomVersion"])).ToList();
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_headerEntity GetMaxCPVersionByBomVersion(int articleNr, int bomVersion)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"select * from CP_snapshot_header where Artikel_Nr=@articleNr
                               and BOM_version=@bomVersion
                               and CP_version=(select max(CP_version) from CP_snapshot_header where Artikel_Nr=@articleNr and BOM_version=@bomVersion)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				sqlCommand.Parameters.AddWithValue("bomVersion", bomVersion);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_headerEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<int> GetAllBOMVersions(int articleNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"select distinct (BomVersion) from [__BSD_Stucklisten_Snapshot] where [Artikel-Nr]=@articleNr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => Convert.ToInt32(x["BomVersion"])).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.FAUpdate.OpenFABETNEntity> GetOpenFAVersionning(int article, List<int> lagers)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Fertigung.Fertigungsnummer, Artikel.Artikelnummer, Fertigung.Kennzeichen, 
                               IIf(Fertigung.FA_Druckdatum=1,0,1) AS UpdateS, Fertigung.Artikel_Nr, 
                               CAST(Fertigung.ID as int) AS ID_Fer, 0 AS Type_Update, Fertigung.gedruckt, Fertigung.FA_Druckdatum,
                               Fertigung.Lagerort_id, Fertigung.BomVersion, Fertigung.CPVersion
                               FROM Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]
                               WHERE (((Fertigung.Kennzeichen)='Offen') AND 
                               ((Fertigung.Artikel_Nr)=@article) AND 
                               ((Fertigung.Anzahl_erledigt)<=0) AND 
                               ((Fertigung.FA_Gestartet)=0 Or (Fertigung.FA_Gestartet) Is Null))
                               AND ((Fertigung.Lagerort_id) IN ({string.Join(",", lagers)}))
                               ORDER BY Fertigung.gedruckt DESC , Fertigung.FA_Druckdatum DESC;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("article", article);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FAUpdate.OpenFABETNEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.FAUpdate.OpenFANotVersionningEntity> GetOpenFANotVersionning(int article, List<int> lagers)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Fertigung.Fertigungsnummer, Artikel.Artikelnummer, Fertigung.Kennzeichen,
IIf(Fertigung.FA_Druckdatum=1,0,1) AS UpdateS, Fertigung.Artikel_Nr,
Fertigung.ID AS ID_Fer, 0 AS Type_Update, Fertigung.gedruckt, Fertigung.FA_Druckdatum,
Fertigung.Lagerort_id
FROM Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]
WHERE
 (((Fertigung.Kennzeichen)='Offen')
AND ((Fertigung.Artikel_Nr)=@article)
AND ((Fertigung.Lagerort_id) NOT IN ({string.Join(",", lagers)})) AND ((Fertigung.Anzahl_erledigt)<=0) AND ((Fertigung.FA_Gestartet)=0
Or (Fertigung.FA_Gestartet) Is Null))
ORDER BY Fertigung.gedruckt DESC , Fertigung.FA_Druckdatum DESC;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("article", article);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FAUpdate.OpenFANotVersionningEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.FAUpdate.FAAnalyse1Entity> GetFAAnalyseFehlmaterial(List<int> lager, List<int> hauptlagers, int fa, string date)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT T5.Termin_Fertigstellung as T_F, T5.Termin_Bestätigt1 as T_B1, T5.Fertigungsnummer as Fer, T5.Anzahl as Anz, T5.Artikelnummer as Artik_Nr, T5.[Bezeichnung 1] as B1, T5.UmsatzCZ as UmCZ, T5.[ProdZeit(h)] as Prod, T7.Artikelnummer as Artik_Nr2, T7.Verfügbar as Ver, T7.SummevonFABedarf as SummFAB, T7.SummevonBestand as SummBe, Artikel.[Bezeichnung 1] as BZ2
FROM ((
(
SELECT Fertigung.Fertigungsnummer, Fertigung.Termin_Fertigstellung, Fertigung.Termin_Bestätigt1, Fertigung.Anzahl, Artikel_1.Artikelnummer, Artikel_1.[Bezeichnung 1], [Betrag]*[Anzahl] AS UmsatzCZ, [Produktionszeit]*[Anzahl]/60 AS [ProdZeit(h)]
FROM (Fertigung LEFT JOIN Artikel AS Artikel_1 ON Fertigung.Artikel_Nr = Artikel_1.[Artikel-Nr]) LEFT JOIN artikel_kalkulatorische_kosten ON Artikel_1.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]
WHERE (((Fertigung.Termin_Bestätigt1) {date}) AND ((Fertigung.Kennzeichen)='offen') AND ((Fertigung.gebucht)=1) AND ((artikel_kalkulatorische_kosten.Kostenart)='Arbeitskosten') AND ((Fertigung.Lagerort_id) in ({string.Join(",", lager)}) ))
)
as T5 LEFT JOIN
(
SELECT Stücklisten.Artikelnummer, T1.Termin_Fertigstellung, T1.Fertigungsnummer, (T1.Anzahl*Stücklisten.Anzahl) AS FABedarf
FROM
(
SELECT Fertigung.Fertigungsnummer, Fertigung.Termin_Fertigstellung, Fertigung.Termin_Bestätigt1, Fertigung.Anzahl, Artikel_1.Artikelnummer, Artikel_1.[Bezeichnung 1], [Betrag]*[Anzahl] AS UmsatzCZ, [Produktionszeit]*[Anzahl]/60 AS [ProdZeit(h)]
FROM (Fertigung LEFT JOIN Artikel AS Artikel_1 ON Fertigung.Artikel_Nr = Artikel_1.[Artikel-Nr]) LEFT JOIN artikel_kalkulatorische_kosten ON Artikel_1.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]
 WHERE (((Fertigung.Termin_Bestätigt1) {date}) AND ((Fertigung.Kennzeichen)='offen') AND ((Fertigung.gebucht)=1) AND ((artikel_kalkulatorische_kosten.Kostenart)='Arbeitskosten') AND ((Fertigung.Lagerort_id) in ({string.Join(",", lager)}) ))
)
as T1 INNER JOIN (Artikel INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) ON T1.Artikelnummer = Artikel.Artikelnummer
) as T6 ON T5.Fertigungsnummer =T6.Fertigungsnummer) LEFT JOIN
(
SELECT T3.SummevonFABedarf, T4.SummevonBestand, [SummevonBestand]-[SummevonFABedarf] AS Verfügbar, T3.Artikelnummer
FROM
(
SELECT T2.Artikelnummer, Sum(T2.FABedarf) AS SummevonFABedarf
FROM
(
SELECT Stücklisten.Artikelnummer, T1.Termin_Fertigstellung, T1.Fertigungsnummer, (T1.Anzahl*Stücklisten.Anzahl) AS FABedarf
FROM
(
SELECT Fertigung.Fertigungsnummer, Fertigung.Termin_Fertigstellung, Fertigung.Termin_Bestätigt1, Fertigung.Anzahl, Artikel_1.Artikelnummer, Artikel_1.[Bezeichnung 1], [Betrag]*[Anzahl] AS UmsatzCZ, [Produktionszeit]*[Anzahl]/60 AS [ProdZeit(h)]
FROM (Fertigung LEFT JOIN Artikel AS Artikel_1 ON Fertigung.Artikel_Nr = Artikel_1.[Artikel-Nr]) LEFT JOIN artikel_kalkulatorische_kosten ON Artikel_1.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]
WHERE (((Fertigung.Termin_Bestätigt1) {date}) AND ((Fertigung.Kennzeichen)='offen') AND ((Fertigung.gebucht)=1) AND ((artikel_kalkulatorische_kosten.Kostenart)='Arbeitskosten') AND ((Fertigung.Lagerort_id) in ({string.Join(",", lager)}) ))
)
as T1 INNER JOIN (Artikel INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) ON T1.Artikelnummer = Artikel.Artikelnummer
) as T2
GROUP BY T2.Artikelnummer
) as T3 INNER JOIN
(
SELECT Sum(Lager.Bestand) AS SummevonBestand, Artikel.Artikelnummer
FROM Lager INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr]
WHERE Lager.Lagerort_id in ({string.Join(",", hauptlagers)})
GROUP BY Artikel.Artikelnummer, Lager.[Artikel-Nr]
) as T4  ON T3.Artikelnummer =T4 .Artikelnummer
WHERE ((([SummevonBestand] - [SummevonFABedarf]) < 0))
) as T7 
ON T6.Artikelnummer = T7.Artikelnummer) LEFT JOIN Artikel ON T7.Artikelnummer = Artikel.Artikelnummer
GROUP BY T5.Termin_Fertigstellung, T5.Termin_Bestätigt1, T5.Fertigungsnummer, T5.Anzahl, T5.Artikelnummer, T5.[Bezeichnung 1], T5.UmsatzCZ, T5.[ProdZeit(h)], T7.Artikelnummer, T7.Verfügbar, T7.SummevonFABedarf, T7.SummevonBestand, Artikel.[Bezeichnung 1]
HAVING (((T5.Fertigungsnummer) =@fa) And T7.Artikelnummer Is Not Null)
ORDER BY T5.Termin_Fertigstellung, T5.Fertigungsnummer, T5.Artikelnummer;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("fa", fa);
				sqlCommand.Parameters.AddWithValue("date", date);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FAUpdate.FAAnalyse1Entity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static int UpdateBestandType1(int order, Decimal quantity, int land)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"update L set L.Bestand -= (FP.Anzahl / F.Originalanzahl) * @quantity from
                           (Lager L inner join (
                           Fertigung F inner join Fertigung_Positionen FP on F.ID=FP.ID_Fertigung
                           ) on FP.Artikel_Nr=L.[Artikel-Nr])
                           left join tbl_Planung_gestartet_ROH2 tbl on (FP.Artikel_Nr = tbl.Artikel_Nr) AND (F.ID = tbl.ID_Fertigung)
                           where
                           F.Fertigungsnummer=@order
                           and
                           L.Lagerort_id=@land
                           and
                           tbl.Artikel_Nr Is Null";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("order", order);
				sqlCommand.Parameters.AddWithValue("quantity", quantity);
				sqlCommand.Parameters.AddWithValue("land", land);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int UpdateBestandType2(int order, Decimal quantity, int land)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"UPDATE L SET L.Bestand_reserviert = Round(L.[Bestand_reserviert]- (FP.Anzahl/F.Originalanzahl)*@quantity,4), L.GesamtBestand = L.[Bestand]+L.[Bestand_reserviert]- (FP.Anzahl/F.Originalanzahl)*@quantity from
                           (Lager L inner join (
                           Fertigung F inner join Fertigung_Positionen FP on F.ID=FP.ID_Fertigung
                           ) on FP.Artikel_Nr=L.[Artikel-Nr] )
                           left join tbl_Planung_gestartet_ROH2 tbl on (FP.Artikel_Nr = tbl.Artikel_Nr) AND (F.ID = tbl.ID_Fertigung)
                           where
                           F.Fertigungsnummer=@order
                           and
                           L.Lagerort_id=@land
                           and
                           tbl.Artikel_Nr Is Not Null";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("order", order);
				sqlCommand.Parameters.AddWithValue("quantity", quantity);
				sqlCommand.Parameters.AddWithValue("land", land);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		//gewerk 1 analyse
		public static List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1ALEntity> GetFAAnalyseGewerk1AL(string date)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Fertigung.Fertigungsnummer AS FA_Nr, Artikel.Freigabestatus, Fertigung.Originalanzahl AS [FA Sasia],
Artikel.Artikelnummer AS [Numeri i artikullit], Stücklisten.Artikelnummer AS [Numeri i materialit],
[Stücklisten].[Anzahl]*[Originalanzahl] AS [Bedarf Nevoje], [PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand AL].SummevonBestand AS [In P3000],
[SummevonBestand]-IIf([SummevonBedarf]>0,[SummevonBedarf],0) AS [Im Lager Ne magazine],
IIf([SummevonBedarf]>0,[SummevonBedarf],0) AS [In Produktion Ne prodhim], [Termin_Bestätigt1]-21 AS [Afati i prestarise],
Artikel_1.[Bezeichnung 1] AS Material, Artikel_1.Klassifizierung AS Typ_Material, Fertigung.[Gewerk 1],
Fertigung.[Gewerk 2], Fertigung.[Gewerk 3], [PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand AL].SummevonBestand,
[PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Summenmengen AL].SummevonBedarf, Fertigung.FA_begonnen

FROM (((((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
INNER JOIN Artikelstamm_Klassifizierung ON Artikel_1.Klassifizierung = Artikelstamm_Klassifizierung.Klassifizierung) 
LEFT JOIN 
(
SELECT Artikel.Artikelnummer, Sum(Lager.Bestand) AS SummevonBestand
FROM (Artikel INNER JOIN Lager ON Artikel.[Artikel-Nr] = Lager.[Artikel-Nr]) 
INNER JOIN Artikelstamm_Klassifizierung ON Artikel.ID_Klassifizierung = Artikelstamm_Klassifizierung.ID
WHERE (((Lager.Lagerort_id)=26) AND ((Artikelstamm_Klassifizierung.ID)=3 Or 
(Artikelstamm_Klassifizierung.ID)=6 Or (Artikelstamm_Klassifizierung.ID)=8))
GROUP BY Artikel.Artikelnummer
)
[PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand AL] 
ON Artikel_1.Artikelnummer = [PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand AL].Artikelnummer) 
LEFT JOIN 
(
SELECT SUM(Bedarf) AS SummevonBedarf,Artikelnummer from
(
SELECT [Stücklisten].[Anzahl]*[Originalanzahl] AS Bedarf, Stücklisten.Artikelnummer
FROM (((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr])
INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr])
INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr])
INNER JOIN Artikelstamm_Klassifizierung ON Artikel_1.Klassifizierung = Artikelstamm_Klassifizierung.Klassifizierung
WHERE (((Fertigung.Kabel_geschnitten)=1) AND ((Fertigung.Kennzeichen)='offen') AND
((Fertigung.Lagerort_id)=26) AND ((Artikelstamm_Klassifizierung.ID)=3 Or 
(Artikelstamm_Klassifizierung.ID)=6 Or (Artikelstamm_Klassifizierung.ID)=8))

) X
GROUP BY X.Artikelnummer
)
[PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Summenmengen AL] 
ON Artikel_1.Artikelnummer = [PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Summenmengen AL].Artikelnummer
WHERE (((Fertigung.[Gewerk 1])='False') AND ((Fertigung.Kennzeichen)='offen') 
AND ((Fertigung.Kabel_geschnitten)=0) AND ((Fertigung.Lagerort_id)=26) AND ((Fertigung.Termin_Bestätigt1)<=@date) 
AND ((Artikelstamm_Klassifizierung.ID)=3 Or (Artikelstamm_Klassifizierung.ID)=6 Or (Artikelstamm_Klassifizierung.ID)=8))
ORDER BY Stücklisten.Artikelnummer, [Termin_Bestätigt1]-21;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("date", date);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1ALEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1CZEntity> GetFAAnalyseGewerk1CZ(string date)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Fertigung.Fertigungsnummer AS FA_Cislo, Artikel.Freigabestatus, Fertigung.Originalanzahl AS FA_Mnostvi,
Artikel.Artikelnummer AS Cislo_Zbozi, Stücklisten.Artikelnummer AS Cislo_Material, [Stücklisten].[Anzahl]*[Originalanzahl] AS Bedarf,
PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand.SummevonBestand AS P3000,
[SummevonBestand]-IIf([SummevonBedarf]>0,[SummevonBedarf],0) AS [Im Lager], IIf([SummevonBedarf]>0,[SummevonBedarf],0) AS [In derProduktion],
[Termin_Bestätigt1]-21 AS Termin_Rezarna, Artikel_1.[Bezeichnung 1] AS Material, Artikel_1.Klassifizierung AS Typ_Material,
Fertigung.[Gewerk 1], Fertigung.[Gewerk 2], Fertigung.[Gewerk 3], Fertigung.FA_begonnen
FROM (((((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
INNER JOIN Artikelstamm_Klassifizierung ON Artikel_1.Klassifizierung = Artikelstamm_Klassifizierung.Klassifizierung) 
LEFT JOIN 
(
SELECT SUM(Bedarf) AS SummevonBedarf,Artikelnummer FROM(
SELECT [Stücklisten].[Anzahl]*[Originalanzahl] AS Bedarf, Stücklisten.Artikelnummer
FROM (((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
INNER JOIN Artikelstamm_Klassifizierung ON Artikel_1.Klassifizierung = Artikelstamm_Klassifizierung.Klassifizierung
WHERE (((Fertigung.Kabel_geschnitten)=1) AND ((Fertigung.Kennzeichen)='offen') AND 
((Fertigung.Lagerort_id)=6 Or (Fertigung.Lagerort_id)=21) AND ((Artikelstamm_Klassifizierung.ID)=3 
Or (Artikelstamm_Klassifizierung.ID)=6 Or (Artikelstamm_Klassifizierung.ID)=8))
) X GROUP BY Artikelnummer
)
PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Summenmengen 
ON Artikel_1.Artikelnummer = PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Summenmengen.Artikelnummer) 
LEFT JOIN 
(
SELECT Artikel.Artikelnummer, Sum(Lager.Bestand) AS SummevonBestand
FROM (Artikel INNER JOIN Lager ON Artikel.[Artikel-Nr] = Lager.[Artikel-Nr]) 
INNER JOIN Artikelstamm_Klassifizierung ON Artikel.ID_Klassifizierung = Artikelstamm_Klassifizierung.ID
WHERE (((Lager.Lagerort_id)=6 Or (Lager.Lagerort_id)=21) AND ((Artikelstamm_Klassifizierung.ID)=3 Or 
(Artikelstamm_Klassifizierung.ID)=6 Or (Artikelstamm_Klassifizierung.ID)=8))
GROUP BY Artikel.Artikelnummer
)
PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand 
ON Artikel_1.Artikelnummer = PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand.Artikelnummer
WHERE (((Fertigung.[Gewerk 1])='False') AND ((Fertigung.Kennzeichen)='offen') AND 
((Fertigung.Kabel_geschnitten)=0) AND ((Fertigung.Lagerort_id)=6) AND ((Fertigung.Termin_Bestätigt1)<=@date) 
AND ((Artikelstamm_Klassifizierung.ID)=3 Or (Artikelstamm_Klassifizierung.ID)=6 Or (Artikelstamm_Klassifizierung.ID)=8))
ORDER BY Stücklisten.Artikelnummer, [Termin_Bestätigt1]-21;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("date", date);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1CZEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1TNEntity> GetFAAnalyseGewerk1TN(string date)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Fertigung.Fertigungsnummer, Artikel.Freigabestatus, Fertigung.Originalanzahl AS [Gesamt Menge],
Artikel.Artikelnummer, Stücklisten.Artikelnummer AS [ROH Artikelnummer], 
[Stücklisten].[Anzahl]*[Originalanzahl] AS Bedarf, [PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand TN].SummevonBestand AS [In p3000],
[SummevonBestand]-IIf([SummevonBedarf]>0,[SummevonBedarf],0) AS Ve_skladu, IIf([SummevonBedarf]>0,[SummevonBedarf],0) AS Ve_vyrobe,
[Termin_Bestätigt1]-21 AS Termin_Schneiderei, Artikel_1.[Bezeichnung 1] AS Material,
Artikel_1.Klassifizierung AS Typ_Material, Fertigung.[Gewerk 1], Fertigung.[Gewerk 2], Fertigung.[Gewerk 3],
[PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand TN].SummevonBestand,
[PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Summenmengen TN].SummevonBedarf, Fertigung.FA_begonnen
FROM (((((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
INNER JOIN Artikelstamm_Klassifizierung ON Artikel_1.Klassifizierung = Artikelstamm_Klassifizierung.Klassifizierung) 
LEFT JOIN 
(
SELECT Artikel.Artikelnummer, Sum(Lager.Bestand) AS SummevonBestand
FROM (Artikel INNER JOIN Lager ON Artikel.[Artikel-Nr] = Lager.[Artikel-Nr]) 
INNER JOIN Artikelstamm_Klassifizierung ON Artikel.ID_Klassifizierung = Artikelstamm_Klassifizierung.ID
WHERE (((Lager.Lagerort_id)=7) AND ((Artikelstamm_Klassifizierung.ID)=3 Or 
(Artikelstamm_Klassifizierung.ID)=6 Or (Artikelstamm_Klassifizierung.ID)=8))
GROUP BY Artikel.Artikelnummer
)
[PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand TN] 
ON Artikel_1.Artikelnummer = [PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand TN].Artikelnummer) 
LEFT JOIN 
(
SELECT SUM(Bedarf) AS SummevonBedarf,Artikelnummer FROM(
SELECT [Stücklisten].[Anzahl]*[Originalanzahl] AS Bedarf, Stücklisten.Artikelnummer
FROM (((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
INNER JOIN Artikelstamm_Klassifizierung ON Artikel_1.Klassifizierung = Artikelstamm_Klassifizierung.Klassifizierung
WHERE (((Fertigung.Kabel_geschnitten)=1) AND ((Fertigung.Kennzeichen)='offen') AND 
((Fertigung.Lagerort_id)=7) AND ((Artikelstamm_Klassifizierung.ID)=3 Or 
(Artikelstamm_Klassifizierung.ID)=6 Or (Artikelstamm_Klassifizierung.ID)=8))
)X GROUP BY Artikelnummer
)
[PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Summenmengen TN] 
ON Artikel_1.Artikelnummer = [PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Summenmengen TN].Artikelnummer
WHERE (((Fertigung.[Gewerk 1])='False') AND ((Fertigung.Kennzeichen)='offen') AND 
((Fertigung.Kabel_geschnitten)=0) AND ((Fertigung.Lagerort_id)=7) AND ((Fertigung.Termin_Bestätigt1)<=@date) 
AND ((Artikelstamm_Klassifizierung.ID)=3 Or (Artikelstamm_Klassifizierung.ID)=6 Or (Artikelstamm_Klassifizierung.ID)=8))
ORDER BY Stücklisten.Artikelnummer, [Termin_Bestätigt1]-21;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("date", date);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1TNEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1TNEntity> GetFAAnalyseGewerk1KHTN(string date)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Fertigung.Fertigungsnummer, Artikel.Freigabestatus, Fertigung.Originalanzahl AS [Gesamt Menge],
Artikel.Artikelnummer, Stücklisten.Artikelnummer AS [ROH Artikelnummer], Stücklisten.Anzahl*[Originalanzahl] AS Bedarf,
[PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand KHTN].SummevonBestand AS [In p3000],
[SummevonBestand]-IIf([SummevonBedarf]>0,[SummevonBedarf],0) AS Ve_skladu,
IIf([SummevonBedarf]>0,[SummevonBedarf],0) AS Ve_vyrobe, [Termin_Bestätigt1]-21 AS Termin_Schneiderei,
Artikel_1.[Bezeichnung 1] AS Material, Artikel_1.Klassifizierung AS Typ_Material, Fertigung.[Gewerk 1],
Fertigung.[Gewerk 2], Fertigung.[Gewerk 3], [PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand KHTN].SummevonBestand,
[PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Summenmengen TN].SummevonBedarf, Fertigung.FA_begonnen
FROM (((((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
INNER JOIN Artikelstamm_Klassifizierung ON Artikel_1.Klassifizierung = Artikelstamm_Klassifizierung.Klassifizierung) 
LEFT JOIN 
(
SELECT SUM(Bedarf) AS SummevonBedarf,Artikelnummer FROM(
SELECT Stücklisten.Anzahl*[Originalanzahl] AS Bedarf, Stücklisten.Artikelnummer
FROM (((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
INNER JOIN Artikelstamm_Klassifizierung ON Artikel_1.Klassifizierung = Artikelstamm_Klassifizierung.Klassifizierung
WHERE (((Fertigung.Kabel_geschnitten)=1) AND ((Fertigung.Kennzeichen)='offen') 
AND ((Fertigung.Lagerort_id)=42) AND ((Artikelstamm_Klassifizierung.ID)=3 
Or (Artikelstamm_Klassifizierung.ID)=6 Or (Artikelstamm_Klassifizierung.ID)=8))
) X GROUP BY Artikelnummer
)
[PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Summenmengen TN] 
ON Artikel_1.Artikelnummer = [PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Summenmengen TN].Artikelnummer) 
LEFT JOIN 
(
SELECT Artikel.Artikelnummer, Sum(Lager.Bestand) AS SummevonBestand
FROM (Artikel INNER JOIN Lager ON Artikel.[Artikel-Nr] = Lager.[Artikel-Nr]) 
INNER JOIN Artikelstamm_Klassifizierung ON Artikel.ID_Klassifizierung = Artikelstamm_Klassifizierung.ID
WHERE (((Lager.Lagerort_id)=42) AND ((Artikelstamm_Klassifizierung.ID)=3 Or (Artikelstamm_Klassifizierung.ID)=6 
Or (Artikelstamm_Klassifizierung.ID)=8))
GROUP BY Artikel.Artikelnummer
)
[PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand KHTN] 
ON Artikel_1.Artikelnummer = [PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand KHTN].Artikelnummer
WHERE (((Fertigung.[Gewerk 1])='False') AND ((Fertigung.Kennzeichen)='offen') 
AND ((Fertigung.Kabel_geschnitten)=0) AND ((Fertigung.Lagerort_id)=42) AND ((Fertigung.Termin_Bestätigt1)<=@date) 
AND ((Artikelstamm_Klassifizierung.ID)=3 Or (Artikelstamm_Klassifizierung.ID)=6 Or (Artikelstamm_Klassifizierung.ID)=8))
ORDER BY Stücklisten.Artikelnummer, [Termin_Bestätigt1]-21;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("date", date);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1TNEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1TNEntity> GetFAAnalyseGewerk1BETN(string date)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Fertigung.Fertigungsnummer, Artikel.Freigabestatus, Fertigung.Originalanzahl AS [Gesamt Menge],
Artikel.Artikelnummer, Stücklisten.Artikelnummer AS [ROH Artikelnummer], Stücklisten.Anzahl*[Originalanzahl] AS Bedarf,
[PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand BETN].SummevonBestand AS [In p3000],
[SummevonBestand]-IIf([SummevonBedarf]>0,[SummevonBedarf],0) AS Ve_skladu,
IIf([SummevonBedarf]>0,[SummevonBedarf],0) AS Ve_vyrobe, [Termin_Bestätigt1]-21 AS Termin_Schneiderei,
Artikel_1.[Bezeichnung 1] AS Material, Artikel_1.Klassifizierung AS Typ_Material, Fertigung.[Gewerk 1],
Fertigung.[Gewerk 2], Fertigung.[Gewerk 3], [PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand BETN].SummevonBestand,
[PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Summenmengen TN].SummevonBedarf, Fertigung.FA_begonnen
FROM (((((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
INNER JOIN Artikelstamm_Klassifizierung ON Artikel_1.Klassifizierung = Artikelstamm_Klassifizierung.Klassifizierung) 
LEFT JOIN 
(
SELECT SUM(Bedarf) AS SummevonBedarf,Artikelnummer FROM(
SELECT Stücklisten.Anzahl*[Originalanzahl] AS Bedarf, Stücklisten.Artikelnummer
FROM (((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
INNER JOIN Artikelstamm_Klassifizierung ON Artikel_1.Klassifizierung = Artikelstamm_Klassifizierung.Klassifizierung
WHERE (((Fertigung.Kabel_geschnitten)=1) AND ((Fertigung.Kennzeichen)='offen') AND ((Fertigung.Lagerort_id)=60) 
AND ((Artikelstamm_Klassifizierung.ID)=3 Or (Artikelstamm_Klassifizierung.ID)=6 Or (Artikelstamm_Klassifizierung.ID)=8))
) X GROUP BY Artikelnummer
)
[PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Summenmengen TN] 
ON Artikel_1.Artikelnummer = [PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Summenmengen TN].Artikelnummer) 
LEFT JOIN 
(
SELECT Artikel.Artikelnummer, Sum(Lager.Bestand) AS SummevonBestand
FROM (Artikel INNER JOIN Lager ON Artikel.[Artikel-Nr] = Lager.[Artikel-Nr]) 
INNER JOIN Artikelstamm_Klassifizierung ON Artikel.ID_Klassifizierung = Artikelstamm_Klassifizierung.ID
WHERE (((Lager.Lagerort_id)=60) AND ((Artikelstamm_Klassifizierung.ID)=3 Or 
(Artikelstamm_Klassifizierung.ID)=6 Or (Artikelstamm_Klassifizierung.ID)=8))
GROUP BY Artikel.Artikelnummer
)
[PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand BETN] 
ON Artikel_1.Artikelnummer = [PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand BETN].Artikelnummer
WHERE (((Fertigung.[Gewerk 1])='False') AND ((Fertigung.Kennzeichen)='offen') AND 
((Fertigung.Kabel_geschnitten)=0) AND ((Fertigung.Lagerort_id)=60) AND ((Fertigung.Termin_Bestätigt1)<=@date) 
AND ((Artikelstamm_Klassifizierung.ID)=3 Or (Artikelstamm_Klassifizierung.ID)=6 Or (Artikelstamm_Klassifizierung.ID)=8))
ORDER BY Stücklisten.Artikelnummer, [Termin_Bestätigt1]-21;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("date", date);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1TNEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1TNEntity> GetFAAnalyseGewerk1GZTN(string date)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Fertigung.Fertigungsnummer, Artikel.Freigabestatus, Fertigung.Originalanzahl AS [Gesamt Menge],
									Artikel.Artikelnummer, Stücklisten.Artikelnummer AS [ROH Artikelnummer], Stücklisten.Anzahl*[Originalanzahl] AS Bedarf,
									[PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand BETN].SummevonBestand AS [In p3000],
									[SummevonBestand]-IIf([SummevonBedarf]>0,[SummevonBedarf],0) AS Ve_skladu,
									IIf([SummevonBedarf]>0,[SummevonBedarf],0) AS Ve_vyrobe, [Termin_Bestätigt1]-21 AS Termin_Schneiderei,
									Artikel_1.[Bezeichnung 1] AS Material, Artikel_1.Klassifizierung AS Typ_Material, Fertigung.[Gewerk 1],
									Fertigung.[Gewerk 2], Fertigung.[Gewerk 3], [PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand BETN].SummevonBestand,
									[PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Summenmengen TN].SummevonBedarf, Fertigung.FA_begonnen
									FROM (((((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
									INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
									INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
									INNER JOIN Artikelstamm_Klassifizierung ON Artikel_1.Klassifizierung = Artikelstamm_Klassifizierung.Klassifizierung) 
									LEFT JOIN 
									(
									SELECT SUM(Bedarf) AS SummevonBedarf,Artikelnummer FROM(
									SELECT Stücklisten.Anzahl*[Originalanzahl] AS Bedarf, Stücklisten.Artikelnummer
									FROM (((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
									INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
									INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
									INNER JOIN Artikelstamm_Klassifizierung ON Artikel_1.Klassifizierung = Artikelstamm_Klassifizierung.Klassifizierung
									WHERE (((Fertigung.Kabel_geschnitten)=1) AND ((Fertigung.Kennzeichen)='offen') AND ((Fertigung.Lagerort_id)=102) 
									AND ((Artikelstamm_Klassifizierung.ID)=3 Or (Artikelstamm_Klassifizierung.ID)=6 Or (Artikelstamm_Klassifizierung.ID)=8))
									) X GROUP BY Artikelnummer
									)
									[PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Summenmengen TN] 
									ON Artikel_1.Artikelnummer = [PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Summenmengen TN].Artikelnummer) 
									LEFT JOIN 
									(
									SELECT Artikel.Artikelnummer, Sum(Lager.Bestand) AS SummevonBestand
									FROM (Artikel INNER JOIN Lager ON Artikel.[Artikel-Nr] = Lager.[Artikel-Nr]) 
									INNER JOIN Artikelstamm_Klassifizierung ON Artikel.ID_Klassifizierung = Artikelstamm_Klassifizierung.ID
									WHERE (((Lager.Lagerort_id)=102) AND ((Artikelstamm_Klassifizierung.ID)=3 Or 
									(Artikelstamm_Klassifizierung.ID)=6 Or (Artikelstamm_Klassifizierung.ID)=8))
									GROUP BY Artikel.Artikelnummer
									)
									[PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand BETN] 
									ON Artikel_1.Artikelnummer = [PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand BETN].Artikelnummer
									WHERE (((Fertigung.[Gewerk 1])='False') AND ((Fertigung.Kennzeichen)='offen') AND 
									((Fertigung.Kabel_geschnitten)=0) AND ((Fertigung.Lagerort_id)=102) AND ((Fertigung.Termin_Bestätigt1)<=@date) 
									AND ((Artikelstamm_Klassifizierung.ID)=3 Or (Artikelstamm_Klassifizierung.ID)=6 Or (Artikelstamm_Klassifizierung.ID)=8))
									ORDER BY Stücklisten.Artikelnummer, [Termin_Bestätigt1]-21;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("date", date);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1TNEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		//gewerk 3 analyse
		public static List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk3ALEntity> GetFAAnalyseGewerk3AL(string date)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Fertigung.Fertigungsnummer AS FA_Nr, Artikel.Freigabestatus, Fertigung.Originalanzahl AS FA_Sasia,
Artikel.Artikelnummer AS [Numeri i artikullit], Stücklisten.Artikelnummer AS [Numeri i materialit],
[Stücklisten].[Anzahl]*[Originalanzahl] AS Nevoje, [SummevonBestand]-IIf([Bedarf]>0,[Bedarf],0) AS [Ne magazine],
IIf([Bedarf]>0,[Bedarf],0) AS [Ne prodhim], [Termin_Bestätigt1]-21 AS [Afati i prestarise],
Artikel_1.[Bezeichnung 1] AS Material, Artikel_1.Klassifizierung AS Typ_Material, Fertigung.[Gewerk 1],
Fertigung.[Gewerk 2], Fertigung.[Gewerk 3], Fertigung.FA_begonnen
FROM (((((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
INNER JOIN Artikelstamm_Klassifizierung ON Artikel_1.Klassifizierung = Artikelstamm_Klassifizierung.Klassifizierung)
LEFT JOIN
(
SELECT Artikel.Artikelnummer, Sum(Lager.Bestand) AS SummevonBestand
FROM (Artikel INNER JOIN Lager ON Artikel.[Artikel-Nr] = Lager.[Artikel-Nr])
INNER JOIN Artikelstamm_Klassifizierung ON Artikel.ID_Klassifizierung = Artikelstamm_Klassifizierung.ID
WHERE (((Lager.Lagerort_id)=26) AND ((Artikelstamm_Klassifizierung.ID)=4 Or 
(Artikelstamm_Klassifizierung.ID)=5 Or (Artikelstamm_Klassifizierung.ID)=7))
GROUP BY Artikel.Artikelnummer
)
X ON Artikel_1.Artikelnummer =
X.Artikelnummer) 
LEFT JOIN 
(
SELECT [Stücklisten].[Anzahl]*[Originalanzahl] AS Bedarf, Stücklisten.Artikelnummer
FROM (((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) INNER JOIN Artikelstamm_Klassifizierung 
ON Artikel_1.Klassifizierung = Artikelstamm_Klassifizierung.Klassifizierung
WHERE (((Fertigung.Kabel_geschnitten)=1) AND ((Fertigung.Kennzeichen)='offen') 
AND ((Fertigung.Lagerort_id)=26) AND ((Artikelstamm_Klassifizierung.ID)=3 
Or (Artikelstamm_Klassifizierung.ID)=6 Or (Artikelstamm_Klassifizierung.ID)=8))
)
Y ON Artikel_1.Artikelnummer = 
Y.Artikelnummer
WHERE (((Fertigung.[Gewerk 1])='NO' Or (Fertigung.[Gewerk 1])='True') AND ((Fertigung.[Gewerk 2])='NO' Or 
(Fertigung.[Gewerk 2])='True') AND ((Fertigung.[Gewerk 3])='False') AND ((Fertigung.Kennzeichen)='offen') AND 
((Fertigung.Kabel_geschnitten)=0) AND ((Fertigung.Lagerort_id)=26) AND ((Fertigung.Termin_Bestätigt1)<=@date) AND 
((Artikelstamm_Klassifizierung.ID)=4 Or (Artikelstamm_Klassifizierung.ID)=5 Or (Artikelstamm_Klassifizierung.ID)=7))
ORDER BY Stücklisten.Artikelnummer, [Termin_Bestätigt1]-21;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("date", date);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk3ALEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk3CZEntity> GetFAAnalyseGewerk3CZ(string date)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Fertigung.Fertigungsnummer AS FA_Cislo, Artikel.Freigabestatus, Fertigung.Originalanzahl AS FA_Mnostvi,
Artikel.Artikelnummer AS Cislo_Zbozi, Stücklisten.Artikelnummer AS Cislo_Material, [Stücklisten].[Anzahl]*[Originalanzahl] AS Potreba,
[SummevonBestand]-IIf([SummevonBedarf]>0,[SummevonBedarf],0) AS Ve_skladu, IIf([SummevonBedarf]>0,[SummevonBedarf],0) AS Ve_vyrobe,
[Termin_Bestätigt1]-21 AS Termin_Rezarna, Artikel_1.[Bezeichnung 1] AS Material, Artikel_1.Klassifizierung AS Typ_Material,
Fertigung.[Gewerk 1], Fertigung.[Gewerk 2], Fertigung.[Gewerk 3], Fertigung.FA_begonnen
FROM (((((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) INNER JOIN Artikel AS Artikel_1 
ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) INNER JOIN Artikelstamm_Klassifizierung 
ON Artikel_1.Klassifizierung = Artikelstamm_Klassifizierung.Klassifizierung) 
LEFT JOIN 
(
SELECT Artikel.Artikelnummer, Sum(Lager.Bestand) AS SummevonBestand
FROM (Artikel INNER JOIN Lager ON Artikel.[Artikel-Nr] = Lager.[Artikel-Nr]) 
INNER JOIN Artikelstamm_Klassifizierung ON Artikel.ID_Klassifizierung = Artikelstamm_Klassifizierung.ID
WHERE (((Lager.Lagerort_id)=6 Or (Lager.Lagerort_id)=21) AND ((Artikelstamm_Klassifizierung.ID)=4 Or 
(Artikelstamm_Klassifizierung.ID)=5 Or (Artikelstamm_Klassifizierung.ID)=7))
GROUP BY Artikel.Artikelnummer
)
PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand_G3 
ON Artikel_1.Artikelnummer = PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand_G3.Artikelnummer) 
LEFT JOIN 
(
SELECT SUM(Bedarf) AS SummevonBedarf,Artikelnummer FROM(
SELECT [Stücklisten].[Anzahl]*[Originalanzahl] AS Bedarf, Stücklisten.Artikelnummer
FROM (((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
INNER JOIN Artikelstamm_Klassifizierung ON Artikel_1.Klassifizierung = Artikelstamm_Klassifizierung.Klassifizierung
WHERE (((Fertigung.Kabel_geschnitten)=1) AND ((Fertigung.Kennzeichen)='offen') AND 
((Fertigung.Lagerort_id)=6 Or (Fertigung.Lagerort_id)=21) AND ((Artikelstamm_Klassifizierung.ID)=4 Or 
(Artikelstamm_Klassifizierung.ID)=5 Or (Artikelstamm_Klassifizierung.ID)=7))
)X GROUP BY Artikelnummer
)
[PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Summenmengen G3] 
ON Artikel_1.Artikelnummer = [PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Summenmengen G3].Artikelnummer
WHERE (((Fertigung.[Gewerk 1])='NO' Or (Fertigung.[Gewerk 1])='True') AND ((Fertigung.[Gewerk 2])='NO' 
Or (Fertigung.[Gewerk 2])='True') AND ((Fertigung.[Gewerk 3])='False') AND ((Fertigung.Kennzeichen)='offen') 
AND ((Fertigung.Kabel_geschnitten)=0) AND ((Fertigung.Lagerort_id)=6) AND ((Fertigung.Termin_Bestätigt1)<=@date) 
AND ((Artikelstamm_Klassifizierung.ID)=4 Or (Artikelstamm_Klassifizierung.ID)=5 Or (Artikelstamm_Klassifizierung.ID)=7))
ORDER BY Stücklisten.Artikelnummer, [Termin_Bestätigt1]-21;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("date", date);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk3CZEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk3CZEntity> GetFAAnalyseGewerk3TN(string date)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Fertigung.Fertigungsnummer AS FA_Cislo, Artikel.Freigabestatus, Fertigung.Originalanzahl AS FA_Mnostvi,
Artikel.Artikelnummer AS Cislo_Zbozi, Stücklisten.Artikelnummer AS Cislo_Material, [Stücklisten].[Anzahl]*[Originalanzahl] AS Potreba,
[SummevonBestand]-IIf([SummevonBedarf]>0,[SummevonBedarf],0) AS Ve_skladu, IIf([SummevonBedarf]>0,[SummevonBedarf],0) AS Ve_vyrobe,
[Termin_Bestätigt1]-21 AS Termin_Rezarna, Artikel_1.[Bezeichnung 1] AS Material, Artikel_1.Klassifizierung AS Typ_Material,
Fertigung.[Gewerk 1], Fertigung.[Gewerk 2], Fertigung.[Gewerk 3], Fertigung.FA_begonnen
FROM (((((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
INNER JOIN Artikelstamm_Klassifizierung ON Artikel_1.Klassifizierung = Artikelstamm_Klassifizierung.Klassifizierung) 
LEFT JOIN 
(
SELECT SUM(Bedarf) AS SummevonBedarf,Artikelnummer FROM(
SELECT [Stücklisten].[Anzahl]*[Originalanzahl] AS Bedarf, Stücklisten.Artikelnummer
FROM (((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
INNER JOIN Artikelstamm_Klassifizierung ON Artikel_1.Klassifizierung = Artikelstamm_Klassifizierung.Klassifizierung
WHERE (((Fertigung.Kabel_geschnitten)=1) AND ((Fertigung.Kennzeichen)='offen') 
AND ((Fertigung.Lagerort_id)=7 Or (Fertigung.Lagerort_id)=4) AND ((Artikelstamm_Klassifizierung.ID)=4 
Or (Artikelstamm_Klassifizierung.ID)=5 Or (Artikelstamm_Klassifizierung.ID)=7))
) X GROUP BY Artikelnummer
)
[PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Summenmengen G3] 
ON Artikel_1.Artikelnummer = [PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Summenmengen G3].Artikelnummer) 
LEFT JOIN 
(
SELECT Artikel.Artikelnummer, Sum(Lager.Bestand) AS SummevonBestand
FROM (Artikel INNER JOIN Lager ON Artikel.[Artikel-Nr] = Lager.[Artikel-Nr]) 
INNER JOIN Artikelstamm_Klassifizierung ON Artikel.ID_Klassifizierung = Artikelstamm_Klassifizierung.ID
WHERE (((Lager.Lagerort_id)=7) AND ((Artikelstamm_Klassifizierung.ID)=4 Or 
(Artikelstamm_Klassifizierung.ID)=5 Or (Artikelstamm_Klassifizierung.ID)=7))
GROUP BY Artikel.Artikelnummer
)
[PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand_G3 TN] 
ON Artikel_1.Artikelnummer = [PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand_G3 TN].Artikelnummer
WHERE (((Fertigung.[Gewerk 1])='NO' Or (Fertigung.[Gewerk 1])='True') AND ((Fertigung.[Gewerk 2])='NO' 
Or (Fertigung.[Gewerk 2])='True') AND ((Fertigung.[Gewerk 3])='False') AND ((Fertigung.Kennzeichen)='offen') 
AND ((Fertigung.Kabel_geschnitten)=0) AND ((Fertigung.Lagerort_id)=7) AND ((Fertigung.Termin_Bestätigt1)<=@date) 
AND ((Artikelstamm_Klassifizierung.ID)=4 Or (Artikelstamm_Klassifizierung.ID)=5 Or (Artikelstamm_Klassifizierung.ID)=7))
ORDER BY Stücklisten.Artikelnummer, [Termin_Bestätigt1]-21;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("date", date);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk3CZEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk3CZEntity> GetFAAnalyseGewerk3KHTN(string date)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Fertigung.Fertigungsnummer AS FA_Cislo, Artikel.Freigabestatus, Fertigung.Originalanzahl AS FA_Mnostvi,
Artikel.Artikelnummer AS Cislo_Zbozi, Stücklisten.Artikelnummer AS Cislo_Material, 
Stücklisten.Anzahl*[Originalanzahl] AS Potreba,
[SummevonBestand]-IIf([SummevonBedarf]>0,[SummevonBedarf],0) AS Ve_skladu, IIf([SummevonBedarf]>0,[SummevonBedarf],0) AS Ve_vyrobe,
[Termin_Bestätigt1]-21 AS Termin_Rezarna, Artikel_1.[Bezeichnung 1] AS Material, Artikel_1.Klassifizierung AS Typ_Material,
Fertigung.[Gewerk 1], Fertigung.[Gewerk 2], Fertigung.[Gewerk 3], Fertigung.FA_begonnen
FROM (((((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
INNER JOIN Artikelstamm_Klassifizierung ON Artikel_1.Klassifizierung = Artikelstamm_Klassifizierung.Klassifizierung) 
LEFT JOIN 
(
SELECT SUM(Bedarf) AS SummevonBedarf,Artikelnummer FROM(
SELECT [Stücklisten].[Anzahl]*[Originalanzahl] AS Bedarf, Stücklisten.Artikelnummer
FROM (((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
INNER JOIN Artikelstamm_Klassifizierung ON Artikel_1.Klassifizierung = Artikelstamm_Klassifizierung.Klassifizierung
WHERE (((Fertigung.Kabel_geschnitten)=1) AND ((Fertigung.Kennzeichen)='offen') AND 
((Fertigung.Lagerort_id)=42 Or (Fertigung.Lagerort_id)=40) AND ((Artikelstamm_Klassifizierung.ID)=4 
Or (Artikelstamm_Klassifizierung.ID)=5 Or (Artikelstamm_Klassifizierung.ID)=7))
) X GROUP BY Artikelnummer
)
[PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Summenmengen G3] 
ON Artikel_1.Artikelnummer = [PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Summenmengen G3].Artikelnummer) 
LEFT JOIN 
(
SELECT Artikel.Artikelnummer, Sum(Lager.Bestand) AS SummevonBestand
FROM (Artikel INNER JOIN Lager ON Artikel.[Artikel-Nr] = Lager.[Artikel-Nr]) 
INNER JOIN Artikelstamm_Klassifizierung ON Artikel.ID_Klassifizierung = Artikelstamm_Klassifizierung.ID
WHERE (((Lager.Lagerort_id)=42) AND ((Artikelstamm_Klassifizierung.ID)=4 Or 
(Artikelstamm_Klassifizierung.ID)=5 Or (Artikelstamm_Klassifizierung.ID)=7))
GROUP BY Artikel.Artikelnummer
)
[PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand_G3 KH] 
ON Artikel_1.Artikelnummer = [PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand_G3 KH].Artikelnummer
WHERE (((Fertigung.[Gewerk 1])='NO' Or (Fertigung.[Gewerk 1])='True') AND ((Fertigung.[Gewerk 2])='NO' 
Or (Fertigung.[Gewerk 2])='True') AND ((Fertigung.[Gewerk 3])='False') AND ((Fertigung.Kennzeichen)='offen') 
AND ((Fertigung.Kabel_geschnitten)=0) AND ((Fertigung.Lagerort_id)=42) AND ((Fertigung.Termin_Bestätigt1)<=@date) 
AND ((Artikelstamm_Klassifizierung.ID)=4 Or (Artikelstamm_Klassifizierung.ID)=5 Or (Artikelstamm_Klassifizierung.ID)=7))
ORDER BY Stücklisten.Artikelnummer, [Termin_Bestätigt1]-21;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("date", date);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk3CZEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk3CZEntity> GetFAAnalyseGewerk3BETN(string date)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Fertigung.Fertigungsnummer AS FA_Cislo, Artikel.Freigabestatus, Fertigung.Originalanzahl AS FA_Mnostvi,
Artikel.Artikelnummer AS Cislo_Zbozi, Stücklisten.Artikelnummer AS Cislo_Material, Stücklisten.Anzahl*[Originalanzahl] AS Potreba,
[SummevonBestand]-IIf([SummevonBedarf]>0,[SummevonBedarf],0) AS Ve_skladu, 
IIf([SummevonBedarf]>0,[SummevonBedarf],0) AS Ve_vyrobe, [Termin_Bestätigt1]-21 AS Termin_Rezarna, Artikel_1.[Bezeichnung 1] AS Material,
Artikel_1.Klassifizierung AS Typ_Material, Fertigung.[Gewerk 1], Fertigung.[Gewerk 2], Fertigung.[Gewerk 3], Fertigung.FA_begonnen
FROM (((((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
INNER JOIN Artikelstamm_Klassifizierung ON Artikel_1.Klassifizierung = Artikelstamm_Klassifizierung.Klassifizierung) 
LEFT JOIN 
(
SELECT SUM(Bedarf) AS SummevonBedarf,Artikelnummer FROM (
SELECT [Stücklisten].[Anzahl]*[Originalanzahl] AS Bedarf, Stücklisten.Artikelnummer
FROM (((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
INNER JOIN Artikelstamm_Klassifizierung ON Artikel_1.Klassifizierung = Artikelstamm_Klassifizierung.Klassifizierung
WHERE (((Fertigung.Kabel_geschnitten)=1) AND ((Fertigung.Kennzeichen)='offen') AND
((Fertigung.Lagerort_id)=60 Or (Fertigung.Lagerort_id)=58) AND ((Artikelstamm_Klassifizierung.ID)=4 
Or (Artikelstamm_Klassifizierung.ID)=5 Or (Artikelstamm_Klassifizierung.ID)=7))
) X GROUP BY Artikelnummer
)
[PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Summenmengen G3] 
ON Artikel_1.Artikelnummer = [PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Summenmengen G3].Artikelnummer) 
LEFT JOIN 
(
SELECT Artikel.Artikelnummer, Sum(Lager.Bestand) AS SummevonBestand
FROM (Artikel INNER JOIN Lager ON Artikel.[Artikel-Nr] = Lager.[Artikel-Nr]) 
INNER JOIN Artikelstamm_Klassifizierung ON Artikel.ID_Klassifizierung = Artikelstamm_Klassifizierung.ID
WHERE (((Lager.Lagerort_id)=60) AND ((Artikelstamm_Klassifizierung.ID)=4 Or 
(Artikelstamm_Klassifizierung.ID)=5 Or (Artikelstamm_Klassifizierung.ID)=7))
GROUP BY Artikel.Artikelnummer
)
[PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand_G3 BE] 
ON Artikel_1.Artikelnummer = [PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand_G3 BE].Artikelnummer
WHERE (((Fertigung.[Gewerk 1])='NO' Or (Fertigung.[Gewerk 1])='True') AND ((Fertigung.[Gewerk 2])='NO' 
Or (Fertigung.[Gewerk 2])='True') AND ((Fertigung.[Gewerk 3])='False') AND ((Fertigung.Kennzeichen)='offen') 
AND ((Fertigung.Kabel_geschnitten)=0) AND ((Fertigung.Lagerort_id)=60) AND ((Fertigung.Termin_Bestätigt1)<=@date) 
AND ((Artikelstamm_Klassifizierung.ID)=4 Or (Artikelstamm_Klassifizierung.ID)=5 Or (Artikelstamm_Klassifizierung.ID)=7))
ORDER BY Stücklisten.Artikelnummer, [Termin_Bestätigt1]-21;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("date", date);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk3CZEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk3CZEntity> GetFAAnalyseGewerk3GZTN(string date)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Fertigung.Fertigungsnummer AS FA_Cislo, Artikel.Freigabestatus, Fertigung.Originalanzahl AS FA_Mnostvi,
									Artikel.Artikelnummer AS Cislo_Zbozi, Stücklisten.Artikelnummer AS Cislo_Material, Stücklisten.Anzahl*[Originalanzahl] AS Potreba,
									[SummevonBestand]-IIf([SummevonBedarf]>0,[SummevonBedarf],0) AS Ve_skladu, 
									IIf([SummevonBedarf]>0,[SummevonBedarf],0) AS Ve_vyrobe, [Termin_Bestätigt1]-21 AS Termin_Rezarna, Artikel_1.[Bezeichnung 1] AS Material,
									Artikel_1.Klassifizierung AS Typ_Material, Fertigung.[Gewerk 1], Fertigung.[Gewerk 2], Fertigung.[Gewerk 3], Fertigung.FA_begonnen
									FROM (((((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
									INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
									INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
									INNER JOIN Artikelstamm_Klassifizierung ON Artikel_1.Klassifizierung = Artikelstamm_Klassifizierung.Klassifizierung) 
									LEFT JOIN 
									(
									SELECT SUM(Bedarf) AS SummevonBedarf,Artikelnummer FROM (
									SELECT [Stücklisten].[Anzahl]*[Originalanzahl] AS Bedarf, Stücklisten.Artikelnummer
									FROM (((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
									INNER JOIN Stücklisten ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr]) 
									INNER JOIN Artikel AS Artikel_1 ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel_1.[Artikel-Nr]) 
									INNER JOIN Artikelstamm_Klassifizierung ON Artikel_1.Klassifizierung = Artikelstamm_Klassifizierung.Klassifizierung
									WHERE (((Fertigung.Kabel_geschnitten)=1) AND ((Fertigung.Kennzeichen)='offen') AND
									((Fertigung.Lagerort_id)=102 Or (Fertigung.Lagerort_id)=101) AND ((Artikelstamm_Klassifizierung.ID)=4 
									Or (Artikelstamm_Klassifizierung.ID)=5 Or (Artikelstamm_Klassifizierung.ID)=7))
									) X GROUP BY Artikelnummer
									)
									[PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Summenmengen G3] 
									ON Artikel_1.Artikelnummer = [PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Summenmengen G3].Artikelnummer) 
									LEFT JOIN 
									(
									SELECT Artikel.Artikelnummer, Sum(Lager.Bestand) AS SummevonBestand
									FROM (Artikel INNER JOIN Lager ON Artikel.[Artikel-Nr] = Lager.[Artikel-Nr]) 
									INNER JOIN Artikelstamm_Klassifizierung ON Artikel.ID_Klassifizierung = Artikelstamm_Klassifizierung.ID
									WHERE (((Lager.Lagerort_id)=102) AND ((Artikelstamm_Klassifizierung.ID)=4 Or 
									(Artikelstamm_Klassifizierung.ID)=5 Or (Artikelstamm_Klassifizierung.ID)=7))
									GROUP BY Artikel.Artikelnummer
									)
									[PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand_G3 BE] 
									ON Artikel_1.Artikelnummer = [PSZ_Rezarna_Material_Gleichheit_geschnittene_FA_Bestand_G3 BE].Artikelnummer
									WHERE (((Fertigung.[Gewerk 1])='NO' Or (Fertigung.[Gewerk 1])='True') AND ((Fertigung.[Gewerk 2])='NO' 
									Or (Fertigung.[Gewerk 2])='True') AND ((Fertigung.[Gewerk 3])='False') AND ((Fertigung.Kennzeichen)='offen') 
									AND ((Fertigung.Kabel_geschnitten)=0) AND ((Fertigung.Lagerort_id)=102) AND ((Fertigung.Termin_Bestätigt1)<=@date) 
									AND ((Artikelstamm_Klassifizierung.ID)=4 Or (Artikelstamm_Klassifizierung.ID)=5 Or (Artikelstamm_Klassifizierung.ID)=7))
									ORDER BY Stücklisten.Artikelnummer, [Termin_Bestätigt1]-21;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("date", date);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk3CZEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<string> GetLikeNumberWithFA(string searchText)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT DISTINCT Artikelnummer FROM(
                               SELECT A.* FROM [Artikel] A INNER JOIN Fertigung F
                               ON A.[Artikel-Nr]=F.Artikel_Nr
                               WHERE A.[Artikelnummer] LIKE @searchText and A.Warengruppe='EF'
                               AND F.Kennzeichen=N'offen' and (F.FA_Gestartet=0 or F.FA_Gestartet is null)) X
                               ORDER BY Artikelnummer";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("searchText", "%" + searchText + "%");

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => Convert.ToString(x["Artikelnummer"])).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.FAPlannung.FAListShneidereiKabelGeschnittenEntity> GetFAListShneidereiKabelGeschnitten(int lager, string term)
		{
			term = term ?? "";
			var dataTable = new DataTable();
			var lagersGestartet = new List<int> { 7, 60, 102, 42, 6/*, 26*/ };
			var lagersGeschnitten = new List<int> { 7, 60, 102, 42 };
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				var ext = lagersGestartet.Contains(lager) ? "AND ((Fertigung.FA_Gestartet)=1 Or (Fertigung.FA_Gestartet)=-1)" : "";
				var extGeschnitten = lagersGeschnitten.Contains(lager) ? "AND ((Fertigung.Kabel_geschnitten)=0 Or (Fertigung.Kabel_geschnitten) Is Null)" : "";
				sqlConnection.Open();
				string query = $@"SELECT Fertigung.Fertigungsnummer, Artikel.Artikelnummer, Fertigung.Originalanzahl AS FA_Menge,
                               Fertigung.Anzahl_erledigt AS Erledigt, Fertigung.Termin_Bestätigt1 AS Termin, Fertigung.Kabel_geschnitten,
                               Fertigung.FA_Gestartet
                               FROM Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]
                               WHERE ( 
                               ((Fertigung.Kennzeichen)=N'offen')
                               AND ((Fertigung.Lagerort_id) IN ({(lager == 6 ? "6, 3, 21" : $"{lager}")})))
                               {ext}
                                {extGeschnitten}
                               AND Fertigung.Fertigungsnummer LIKE '{term.Trim()}%' 
                               ORDER BY Fertigung.Fertigungsnummer;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				//sqlCommand.Parameters.AddWithValue("lager", lager);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FAPlannung.FAListShneidereiKabelGeschnittenEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.FAPlannung.FAListShneidereiKabelGeschnittenEntity> GetFAListShneidereiKabelGeschnittenExact(int lager, string term)
		{
			term = term ?? "";
			var dataTable = new DataTable();
			var lagersGestartet = new List<int> { 7, 60, 102, 42, 6/*, 26*/ };
			var lagersGeschnitten = new List<int> { 7, 60, 102, 42 };
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				var ext = lagersGestartet.Contains(lager) ? "AND ((Fertigung.FA_Gestartet)=1 Or (Fertigung.FA_Gestartet)=-1)" : "";
				var extGeschnitten = lagersGeschnitten.Contains(lager) ? "AND ((Fertigung.Kabel_geschnitten)=0 Or (Fertigung.Kabel_geschnitten) Is Null)" : "";
				sqlConnection.Open();
				string query = $@"SELECT Fertigung.Fertigungsnummer, Artikel.Artikelnummer, Fertigung.Originalanzahl AS FA_Menge,
                               Fertigung.Anzahl_erledigt AS Erledigt, Fertigung.Termin_Bestätigt1 AS Termin, Fertigung.Kabel_geschnitten,
                               Fertigung.FA_Gestartet
                               FROM Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]
                               WHERE ( 
                               ((Fertigung.Kennzeichen)=N'offen')
                               AND ((Fertigung.Lagerort_id) IN ({(lager == 6 ? "6, 3, 21" : $"{lager}")})))
                                {ext}
                                {extGeschnitten}
                               AND Fertigung.Fertigungsnummer = '{term.Trim()}' 
                               ORDER BY Fertigung.Fertigungsnummer;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				//sqlCommand.Parameters.AddWithValue("@lager", lager);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FAPlannung.FAListShneidereiKabelGeschnittenEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<KeyValuePair<int, string>> GetGewerksValues(int id_fa)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT Fertigungsnummer,Gewerk FROM(
SELECT Artikel_1.Artikelnummer AS Artikelnummer1, Artikel_1.[Bezeichnung 1] AS B1,
Artikel_1.[Bezeichnung 2] AS B2, Fertigung.Anzahl AS Anzahl1, Lagerorte.Lagerort AS L1,
Fertigung.Datum, Fertigung.Termin_Fertigstellung, Fertigung.Kennzeichen, Fertigung.Bemerkung,
Artikel.Artikelnummer, Artikel.[Bezeichnung 1], Artikel.[Bezeichnung 2], Fertigung_Positionen.Anzahl,
Fertigung_Positionen.Arbeitsanweisung, Fertigung_Positionen.Fertiger, Fertigung_Positionen.Termin_Soll,
Fertigung_Positionen.Bemerkungen, Lagerorte_1.Lagerort, Artikel_1.EAN, artikel_kalkulatorische_kosten.Betrag,
Artikel_1.Freigabestatus, Fertigung.Zeit AS Produktionszeit, Fertigung.Termin_Bestätigt1, Fertigung.Erstmuster,
Artikel_1.[Freigabestatus TN intern], Artikel_1.Index_Kunde, Fertigung.[Lagerort_id zubuchen], Fertigung.Mandant,
Artikel_1.Sysmonummer AS S1, Artikel.Sysmonummer, Artikel_1.[UL Etikett], Fertigung.Technik, Fertigung.Techniker,
Artikel_1.Kanban, Artikel_1.Verpackungsart, Artikel_1.Verpackungsmenge, Artikel_1.Losgroesse, Fertigung.Quick_Area,
Artikel_1.Artikelfamilie_Kunde, Artikel_1.Artikelfamilie_Kunde_Detail1, Artikel_1.Artikelfamilie_Kunde_Detail2,
Artikel.Klassifizierung, Artikelstamm_Klassifizierung.Bezeichnung, Artikelstamm_Klassifizierung.Nummernkreis,
Artikelstamm_Klassifizierung.Kupferzahl, Artikelstamm_Klassifizierung.ID, Artikelstamm_Klassifizierung.Gewerk,Fertigung.Fertigungsnummer
FROM ((Lagerorte AS Lagerorte_1 INNER JOIN ((Artikel AS Artikel_1 
INNER JOIN (Artikel INNER JOIN (Fertigung INNER JOIN Fertigung_Positionen 
ON Fertigung.ID = Fertigung_Positionen.ID_Fertigung) ON Artikel.[Artikel-Nr] = Fertigung_Positionen.Artikel_Nr) 
ON Artikel_1.[Artikel-Nr] = Fertigung.Artikel_Nr) INNER JOIN Lagerorte ON Fertigung.Lagerort_id = Lagerorte.Lagerort_id) 
ON Lagerorte_1.Lagerort_id = Fertigung_Positionen.Lagerort_ID) LEFT JOIN artikel_kalkulatorische_kosten 
ON Artikel_1.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]) INNER JOIN Artikelstamm_Klassifizierung 
ON Artikel.ID_Klassifizierung = Artikelstamm_Klassifizierung.ID
WHERE (((Fertigung.ID)=@id_fa) 
AND ((Artikelstamm_Klassifizierung.Gewerk) Is Not Null))
) X
GROUP BY Fertigungsnummer,Gewerk";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id_fa", id_fa);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, string>(Convert.ToInt32(x["Fertigungsnummer"]), Convert.ToString(x["Gewerk"]))).ToList();
			}
			else
			{
				return null;
			}
		}

		public static Tuple<int, string, string> GetCSKontakt(int fa)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT Fertigung.Fertigungsnummer, [PSZ_Nummerschlüssel Kunde].Nummerschlüssel, [PSZ_Nummerschlüssel Kunde].[CS Kontakt]
                               FROM [PSZ_Nummerschlüssel Kunde], Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]
                               WHERE (((Fertigung.Fertigungsnummer)=@fa) 
                               AND (([PSZ_Nummerschlüssel Kunde].Nummerschlüssel)=(LEFT(Artikel.[Artikelnummer], (CASE WHEN CHARINDEX('-',Artikel.[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',Artikel.[Artikelnummer],0)-1 END)))));";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("fa", fa);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Tuple<int, string, string>(
					Convert.ToInt32(dataTable.Rows[0]["Fertigungsnummer"]),
					Convert.ToString(dataTable.Rows[0]["Nummerschlüssel"]),
					Convert.ToString(dataTable.Rows[0]["CS Kontakt"]));
			}
			else
			{
				return null;
			}
		}

		#region Query with transaction
		public static List<KeyValuePair<int, string>> GetGewerksValuesWithTransaction(int id_fa, SqlConnection connection, SqlTransaction transaction)
		{
			Debug.WriteLine($"transaction get FA gewerks {transaction.ToString()}");
			var dataTable = new DataTable();
			//using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
			//{
			//sqlConnection.Open();
			string query = @"SELECT Fertigungsnummer,Gewerk FROM(
SELECT Artikel_1.Artikelnummer AS Artikelnummer1, Artikel_1.[Bezeichnung 1] AS B1,
Artikel_1.[Bezeichnung 2] AS B2, Fertigung.Anzahl AS Anzahl1, Lagerorte.Lagerort AS L1,
Fertigung.Datum, Fertigung.Termin_Fertigstellung, Fertigung.Kennzeichen, Fertigung.Bemerkung,
Artikel.Artikelnummer, Artikel.[Bezeichnung 1], Artikel.[Bezeichnung 2], Fertigung_Positionen.Anzahl,
Fertigung_Positionen.Arbeitsanweisung, Fertigung_Positionen.Fertiger, Fertigung_Positionen.Termin_Soll,
Fertigung_Positionen.Bemerkungen, Lagerorte_1.Lagerort, Artikel_1.EAN, artikel_kalkulatorische_kosten.Betrag,
Artikel_1.Freigabestatus, Fertigung.Zeit AS Produktionszeit, Fertigung.Termin_Bestätigt1, Fertigung.Erstmuster,
Artikel_1.[Freigabestatus TN intern], Artikel_1.Index_Kunde, Fertigung.[Lagerort_id zubuchen], Fertigung.Mandant,
Artikel_1.Sysmonummer AS S1, Artikel.Sysmonummer, Artikel_1.[UL Etikett], Fertigung.Technik, Fertigung.Techniker,
Artikel_1.Kanban, Artikel_1.Verpackungsart, Artikel_1.Verpackungsmenge, Artikel_1.Losgroesse, Fertigung.Quick_Area,
Artikel_1.Artikelfamilie_Kunde, Artikel_1.Artikelfamilie_Kunde_Detail1, Artikel_1.Artikelfamilie_Kunde_Detail2,
Artikel.Klassifizierung, Artikelstamm_Klassifizierung.Bezeichnung, Artikelstamm_Klassifizierung.Nummernkreis,
Artikelstamm_Klassifizierung.Kupferzahl, Artikelstamm_Klassifizierung.ID, Artikelstamm_Klassifizierung.Gewerk,Fertigung.Fertigungsnummer
FROM ((Lagerorte AS Lagerorte_1 INNER JOIN ((Artikel AS Artikel_1 
INNER JOIN (Artikel INNER JOIN (Fertigung INNER JOIN Fertigung_Positionen 
ON Fertigung.ID = Fertigung_Positionen.ID_Fertigung) ON Artikel.[Artikel-Nr] = Fertigung_Positionen.Artikel_Nr) 
ON Artikel_1.[Artikel-Nr] = Fertigung.Artikel_Nr) INNER JOIN Lagerorte ON Fertigung.Lagerort_id = Lagerorte.Lagerort_id) 
ON Lagerorte_1.Lagerort_id = Fertigung_Positionen.Lagerort_ID) LEFT JOIN artikel_kalkulatorische_kosten 
ON Artikel_1.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]) INNER JOIN Artikelstamm_Klassifizierung 
ON Artikel.ID_Klassifizierung = Artikelstamm_Klassifizierung.ID
WHERE (((Fertigung.ID)=@id_fa) 
AND ((Artikelstamm_Klassifizierung.Gewerk) Is Not Null))
) X
GROUP BY Fertigungsnummer,Gewerk";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("id_fa", id_fa);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			//}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, string>(Convert.ToInt32(x["Fertigungsnummer"]), Convert.ToString(x["Gewerk"]))).ToList();
			}
			else
			{
				return null;
			}
		}
		#endregion
		#region Querys with transaction
		public static int UpdateBestandType1WithTransaction(int order, Decimal quantity, int land, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			//using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
			//{
			//sqlConnection.Open();
			string query = @"update L set L.Bestand -= (FP.Anzahl / F.Originalanzahl) * @quantity from
                           (Lager L inner join (
                           Fertigung F inner join Fertigung_Positionen FP on F.ID=FP.ID_Fertigung
                           ) on FP.Artikel_Nr=L.[Artikel-Nr])
                           left join tbl_Planung_gestartet_ROH2 tbl on (FP.Artikel_Nr = tbl.Artikel_Nr) AND (F.ID = tbl.ID_Fertigung)
                           where
                           F.Fertigungsnummer=@order
                           and
                           L.Lagerort_id=@land
                           and
                           tbl.Artikel_Nr Is Null";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("order", order);
			sqlCommand.Parameters.AddWithValue("quantity", quantity);
			sqlCommand.Parameters.AddWithValue("land", land);

			results = sqlCommand.ExecuteNonQuery();
			//}

			return results;
		}
		public static int UpdateBestandType2WithTransaction(int order, Decimal quantity, int land, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			//using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
			//{
			//sqlConnection.Open();
			string query = @"UPDATE L SET L.Bestand_reserviert = Round(L.[Bestand_reserviert]- (FP.Anzahl/F.Originalanzahl)*@quantity,4), L.GesamtBestand = L.[Bestand]+L.[Bestand_reserviert]- (FP.Anzahl/F.Originalanzahl)*@quantity from
                           (Lager L inner join (
                           Fertigung F inner join Fertigung_Positionen FP on F.ID=FP.ID_Fertigung
                           ) on FP.Artikel_Nr=L.[Artikel-Nr] )
                           left join tbl_Planung_gestartet_ROH2 tbl on (FP.Artikel_Nr = tbl.Artikel_Nr) AND (F.ID = tbl.ID_Fertigung)
                           where
                           F.Fertigungsnummer=@order
                           and
                           L.Lagerort_id=@land
                           and
                           tbl.Artikel_Nr Is Not Null";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("order", order);
			sqlCommand.Parameters.AddWithValue("quantity", quantity);
			sqlCommand.Parameters.AddWithValue("land", land);

			results = sqlCommand.ExecuteNonQuery();
			//}

			return results;
		}
		public static int UpdateBestandQtyTransaction(int land, int order, double qte, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			string query = @"update L set L.Bestand-=(FP.Anzahl/F.Originalanzahl) * @qte  from " +
							"Lager L inner join (  " +
							"Fertigung F inner join Fertigung_Positionen FP on F.ID=FP.ID_Fertigung " +
							") on FP.Artikel_Nr=L.[Artikel-Nr] and F.Lagerort_id=L.Lagerort_id  " +
							"where " +
							"F.Lagerort_id = @userland  " +
							"and " +
							"F.Fertigungsnummer=@order";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("userland", land);
			sqlCommand.Parameters.AddWithValue("order", order);
			sqlCommand.Parameters.AddWithValue("qte", qte);

			results = sqlCommand.ExecuteNonQuery();

			return results;
		}
		#endregion
	}
}
