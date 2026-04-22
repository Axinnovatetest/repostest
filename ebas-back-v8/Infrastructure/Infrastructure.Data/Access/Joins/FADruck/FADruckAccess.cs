using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Joins.FADruck
{
	public class FADruckAccess
	{
		public static Infrastructure.Data.Entities.Joins.FADruck.FAReport1HeaderEntity GetFAReportHeader(int fa)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT Fertigung.ID, Fertigung.ID_Rahmenfertigung, Artikel_1.Artikelnummer, Artikel_1.[Bezeichnung 1],
                               Artikel_1.[Bezeichnung 2], Fertigung.Anzahl, Lagerorte.Lagerort, Fertigung.Fertigungsnummer,
                               Fertigung.Datum, Fertigung.Termin_Fertigstellung, Fertigung.Kennzeichen, Fertigung.Bemerkung,
                               Artikel_1.EAN,artikel_kalkulatorische_kosten.Betrag, Artikel_1.Freigabestatus, Fertigung.Zeit AS Produktionszeit,
                               Fertigung.Termin_Bestätigt1, Fertigung.Erstmuster, Artikel_1.[Freigabestatus TN intern],
                               Fertigung.KundenIndex AS Index_Kunde, Fertigung.[Lagerort_id zubuchen], Fertigung.Mandant,
                               Artikel_1.Sysmonummer, /*Artikel.Sysmonummer,*/ Artikel_1.[UL Etikett], Fertigung.Technik,
                               Fertigung.Techniker, Artikel_1.Kanban, Artikel_1.Verpackungsart, Artikel_1.Verpackungsmenge,
                               Artikel_1.Losgroesse, Fertigung.Quick_Area, Artikel_1.Artikelfamilie_Kunde,
                               Artikel_1.Artikelfamilie_Kunde_Detail1, Artikel_1.Artikelfamilie_Kunde_Detail2,
                               Artikel_1.Halle/*, Artikel.ESD_Schutz*/
                               FROM 
                               Fertigung INNER JOIN Lagerorte ON Fertigung.Lagerort_id = Lagerorte.Lagerort_id
                               INNER JOIN Artikel Artikel_1 ON Artikel_1.[Artikel-Nr] = Fertigung.Artikel_Nr
                               LEFT JOIN artikel_kalkulatorische_kosten ON Artikel_1.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]
                               WHERE Fertigung.Fertigungsnummer=@fa";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("fa", fa);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Joins.FADruck.FAReport1HeaderEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.FADruck.FAReport1PositionsEntity> GetFAReportPositions(int faId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT Artikel.Artikelnummer, Artikel.[Bezeichnung 1], Artikel.[Bezeichnung 2],
                               Fertigung_Positionen.Anzahl, Fertigung_Positionen.Arbeitsanweisung, Fertigung_Positionen.Fertiger,
                               Fertigung_Positionen.Termin_Soll, Fertigung_Positionen.Bemerkungen, Lagerorte_1.Lagerort, 
                               Artikel.ESD_Schutz
                               FROM Artikel INNER JOIN Fertigung_Positionen ON Artikel.[Artikel-Nr] = Fertigung_Positionen.Artikel_Nr
                               INNER JOIN Lagerorte Lagerorte_1 ON Lagerorte_1.Lagerort_id = Fertigung_Positionen.Lagerort_ID
                               WHERE Fertigung_Positionen.ID_Fertigung=@faId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("faId", faId);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FADruck.FAReport1PositionsEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.FADruck.FAReport1PlannungEntity> GetFAReportPlannung(int faNummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT Artikel_1.Artikelnummer, Artikel_1.[Bezeichnung 1], Artikel_1.[Bezeichnung 2], 
                               Fertigung.Anzahl, Lagerorte.Lagerort, Fertigung.Fertigungsnummer, Fertigung.Datum, 
                               Fertigung.Termin_Fertigstellung, Fertigung.Kennzeichen, Fertigung.Bemerkung, 
                               Artikel.Artikelnummer, Artikel.[Bezeichnung 1], Artikel.[Bezeichnung 2], 
                               Fertigung_Positionen.Anzahl, Fertigung_Positionen.Arbeitsanweisung, 
                               Fertigung_Positionen.Fertiger, Fertigung_Positionen.Termin_Soll, 
                               Fertigung_Positionen.Bemerkungen, Lagerorte_1.Lagerort, Artikel_1.EAN, 
                               artikel_kalkulatorische_kosten.Betrag, Artikel_1.Freigabestatus, 
                               Fertigung.Zeit AS Produktionszeit, Fertigung.Termin_Bestätigt1, 
                               Fertigung.Erstmuster, Artikel_1.[Freigabestatus TN intern], Artikel_1.Index_Kunde,
                               Fertigung.[Lagerort_id zubuchen], Fertigung.Mandant, Artikel_1.Sysmonummer,
                               Artikel.Sysmonummer, Artikel_1.[UL Etikett], Fertigung.Technik, Fertigung.Techniker, 
                               Artikel_1.Kanban, Artikel_1.Verpackungsart, Artikel_1.Verpackungsmenge, Artikel_1.Losgroesse, 
                               Fertigung.Quick_Area, Artikel_1.Artikelfamilie_Kunde, Artikel_1.Artikelfamilie_Kunde_Detail1, 
                               Artikel_1.Artikelfamilie_Kunde_Detail2, Artikel.Klassifizierung, Artikelstamm_Klassifizierung.Bezeichnung,
                               Artikelstamm_Klassifizierung.Nummernkreis, Artikelstamm_Klassifizierung.Kupferzahl, 
                               Artikelstamm_Klassifizierung.ID, Artikelstamm_Klassifizierung.Gewerk
                               FROM ((Lagerorte AS Lagerorte_1 INNER JOIN ((Artikel AS Artikel_1 
                               INNER JOIN (Artikel INNER JOIN 
                               (Fertigung INNER JOIN 
                               Fertigung_Positionen ON 
                               Fertigung.ID = Fertigung_Positionen.ID_Fertigung) ON 
                               Artikel.[Artikel-Nr] = Fertigung_Positionen.Artikel_Nr) ON 
                               Artikel_1.[Artikel-Nr] = Fertigung.Artikel_Nr) INNER JOIN Lagerorte ON 
                               Fertigung.Lagerort_id = Lagerorte.Lagerort_id) ON 
                               Lagerorte_1.Lagerort_id = Fertigung_Positionen.Lagerort_ID) 
                               LEFT JOIN artikel_kalkulatorische_kosten ON 
                               Artikel_1.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]) 
                               INNER JOIN Artikelstamm_Klassifizierung ON
                               Artikel.ID_Klassifizierung = Artikelstamm_Klassifizierung.ID
                               WHERE (((Fertigung.Fertigungsnummer)=@faNummer) AND ((Artikelstamm_Klassifizierung.Gewerk) Is Not Null))
                               ORDER BY Artikelstamm_Klassifizierung.Gewerk";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("faNummer", faNummer);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FADruck.FAReport1PlannungEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.FADruck.ListFAStappleDruckEntity> GetListFAStappleDruck(string article, DateTime date, int lager)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT Artikel.Artikelnummer, Fertigung.Fertigungsnummer, Fertigung.Termin_Bestätigt1,
Fertigung.Lagerort_id, Fertigung.Kennzeichen, Fertigung.gedruckt, Artikel.[Artikel-Nr] AS Artikel_Nr,
Fertigung.FA_Druckdatum, Artikel.Freigabestatus, Artikel.[Freigabestatus TN intern], Fertigung.Technik,
Fertigung.Erstmuster --INTO [PSZ_FADRUCKSTAPPEL Hilfstabelle]
FROM Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]
WHERE (((Artikel.Artikelnummer) Like @article) AND
((Fertigung.Termin_Bestätigt1)<=@date) AND
((Fertigung.Lagerort_id)=@lager) AND ((Fertigung.Kennzeichen)='Offen') AND
((Fertigung.gedruckt)=0) AND ((Fertigung.FA_Druckdatum) Is Null) AND ((Artikel.Freigabestatus)<>'N' And
(Artikel.Freigabestatus)<>'P') AND ((Artikel.[Freigabestatus TN intern])<>'N') AND ((Fertigung.Technik)=0) AND
((Fertigung.Erstmuster)=0)) OR (((Artikel.Artikelnummer) Like @article) AND
((Fertigung.Termin_Bestätigt1)<=DATEADD(day,14,@date)) AND
((Fertigung.Lagerort_id)=@lager) AND ((Fertigung.Kennzeichen)='Offen') AND
((Fertigung.gedruckt)=0) AND ((Fertigung.FA_Druckdatum) Is Null) AND ((Artikel.Freigabestatus)<>'N' And 
(Artikel.Freigabestatus)<>'P') AND ((Artikel.[Freigabestatus TN intern])='N') AND ((Fertigung.Erstmuster)=1)) OR
(((Artikel.Artikelnummer) Like @article) AND 
((Fertigung.Termin_Bestätigt1)<=@date) AND 
((Fertigung.Lagerort_id)=@lager) AND ((Fertigung.Kennzeichen)='Offen') AND
((Fertigung.gedruckt)=0) AND ((Fertigung.FA_Druckdatum) Is Null) AND ((Artikel.Freigabestatus)<>'N' And
(Artikel.Freigabestatus)<>'P') AND ((Artikel.[Freigabestatus TN intern])='N') AND ((Fertigung.Technik)=1)) OR
(((Artikel.Artikelnummer) Like @article) AND 
((Fertigung.Termin_Bestätigt1)<=DATEADD(day,14,@date)) AND 
((Fertigung.Lagerort_id)=@lager) AND ((Fertigung.Kennzeichen)='Offen') AND
((Fertigung.gedruckt)=0) AND ((Fertigung.FA_Druckdatum) Is Null) AND ((Artikel.Freigabestatus)='N') AND
((Artikel.[Freigabestatus TN intern])='N') AND ((Fertigung.Erstmuster)=1)) OR
(((Artikel.Artikelnummer) Like @article) AND 
((Fertigung.Termin_Bestätigt1)<=@date) AND 
((Fertigung.Lagerort_id)=@lager) AND ((Fertigung.Kennzeichen)='Offen') AND
((Fertigung.gedruckt)=0) AND ((Fertigung.FA_Druckdatum) Is Null) AND ((Artikel.Freigabestatus)='N') AND
((Artikel.[Freigabestatus TN intern])='N') AND ((Fertigung.Technik)=1))
ORDER BY Fertigung.Fertigungsnummer";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("article", "%" + article + "%");
				sqlCommand.Parameters.AddWithValue("date", date);
				sqlCommand.Parameters.AddWithValue("lager", lager);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FADruck.ListFAStappleDruckEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
	}
}
