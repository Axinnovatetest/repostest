using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;

namespace Infrastructure.Data.Access.Joins.CTS
{
	public class CSStatisticsAccess
	{
		public static List<Infrastructure.Data.Entities.Joins.CTS.KapazitatLangEntity> GetKapazitatLang(DateTime? dateFrom, DateTime? dateTo, List<int> lagers, string client, int? AT)
		{
			client = (client ?? "").Trim();
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Fertigung.Artikel_Nr, Artikel.Artikelnummer, Artikel.[Bezeichnung 1], Fertigung.gebucht,
                               Fertigung.Kennzeichen, artikel_kalkulatorische_kosten.[artikel-nr], artikel_kalkulatorische_kosten.Kostenart,
                               artikel_kalkulatorische_kosten.Betrag, (Artikel.Produktionszeit*Fertigung.Anzahl)/60 AS Auftragszeit,
                               Fertigung.Termin_Fertigstellung, @AT AS ProdTage,
                               @dateFrom AS Ausdr1, @dateTo AS Ausdr2,
                               Fertigung.Anzahl, Fertigung.Preis*Fertigung.Anzahl AS LohnkostLohnkosten, Fertigung.Termin_Bestätigt1,
                               Fertigung.Fertigungsnummer, Fertigung.Bemerkung, Artikel.Freigabestatus, Fertigung.Lagerort_id, Fertigung.Preis,
                               Lagerorte.Lagerort, Artikel.Sysmonummer, Fertigung.Technik, Fertigung.Techniker, Fertigung.Erstmuster,
                               [View_PSZ_Artikel Kundenzuweisung2].Kunde
                               FROM (((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
                               INNER JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]) 
                               INNER JOIN Lagerorte ON Fertigung.Lagerort_id = Lagerorte.Lagerort_id) 
                               INNER JOIN [View_PSZ_Artikel Kundenzuweisung2] ON Artikel.Artikelnummer = [View_PSZ_Artikel Kundenzuweisung2].Artikelnummer
                               WHERE (((Artikel.Artikelnummer) Like (@client)) 
                               AND ((Fertigung.gebucht)=1) AND ((Fertigung.Kennzeichen)='offen') 
                               AND ((artikel_kalkulatorische_kosten.Kostenart)='Arbeitskosten') 
                               AND ((Fertigung.Termin_Bestätigt1)>=@dateFrom
                               AND (Fertigung.Termin_Bestätigt1)<=@dateTo) 
                               AND ((Fertigung.Lagerort_id) IN ({string.Join(",", lagers)})));";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("dateFrom", dateFrom);
				sqlCommand.Parameters.AddWithValue("dateTo", dateTo);
				sqlCommand.Parameters.AddWithValue("client", "%" + client + "%");
				sqlCommand.Parameters.AddWithValue("AT", AT == null ? (object)DBNull.Value : AT);
				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.KapazitatLangEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.KapazitatLangEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.CTS.KapazitatKurzEntity> GetKapazitatCZKurz(DateTime? dateFrom, DateTime? dateTo, int? tage)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT [View_PSZ_Artikel Kundenzuweisung2].Kunde, Fertigung.Fertigungsnummer AS [FA#],
                              Fertigung.Termin_Fertigstellung AS Wunschtermin, Fertigung.Termin_Bestätigt1 AS TerminProd,
                              Fertigung.Anzahl AS Auftragsmenge, Artikel.Artikelnummer, Artikel.[Bezeichnung 1] AS [Kunden#],
                              Artikel.Produktionszeit*Fertigung.Anzahl/60 AS [Auftragszeit(h)], Fertigung.Preis*Fertigung.Anzahl AS UmsatzCZ,
                              (Artikel.Produktionszeit*Fertigung.Anzahl/60)/(@tage*6) AS AnzahlMA, Fertigung.Lagerort_id, Artikel.Stundensatz
                              FROM ((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
                              INNER JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]) 
                              INNER JOIN [View_PSZ_Artikel Kundenzuweisung2] ON Artikel.Artikelnummer = [View_PSZ_Artikel Kundenzuweisung2].Artikelnummer
                              WHERE (((Fertigung.Termin_Bestätigt1)>=@dateFrom And (Fertigung.Termin_Bestätigt1)<=@dateTo) 
                              AND ((Fertigung.Lagerort_id)=3 Or (Fertigung.Lagerort_id)=6) AND ((Fertigung.gebucht)=1) 
                              AND ((Fertigung.Kennzeichen)='offen') AND ((artikel_kalkulatorische_kosten.Kostenart)='Arbeitskosten')) 
                              OR (((Fertigung.Termin_Bestätigt1)='31/12/1900') AND ((Fertigung.gebucht)=1) AND ((Fertigung.Kennzeichen)='offen') 
                              AND ((artikel_kalkulatorische_kosten.Kostenart)='Arbeitskosten'))
                              ORDER BY [View_PSZ_Artikel Kundenzuweisung2].Kunde, Fertigung.Termin_Bestätigt1;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("dateFrom", dateFrom);
				sqlCommand.Parameters.AddWithValue("dateTo", dateTo);
				sqlCommand.Parameters.AddWithValue("tage", tage);
				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.KapazitatKurzEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.KapazitatKurzEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.CTS.KapazitatKurzEntity> GetKapazitatTHKurz(DateTime? dateFrom, DateTime? dateTo, int? tage)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT [View_PSZ_Artikel Kundenzuweisung2].Kunde, Fertigung.Fertigungsnummer AS [FA#],
                               Fertigung.Termin_Fertigstellung AS Wunschtermin, Fertigung.Termin_Bestätigt1 AS TerminProd,
                               Fertigung.Anzahl AS Auftragsmenge, Artikel.Artikelnummer, Artikel.[Bezeichnung 1] AS [Kunden#],
                               Artikel.Produktionszeit*Fertigung.Anzahl/60 AS [Auftragszeit(h)], Fertigung.Preis*Fertigung.Anzahl AS UmsatzCZ,
                               (Artikel.Produktionszeit*Fertigung.Anzahl/60)/(@tage*6.75) AS AnzahlMA, Fertigung.Lagerort_id, Artikel.Stundensatz
                               FROM ((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
                               INNER JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]) 
                               INNER JOIN [View_PSZ_Artikel Kundenzuweisung2] ON Artikel.Artikelnummer = [View_PSZ_Artikel Kundenzuweisung2].Artikelnummer
                               WHERE (((Fertigung.Termin_Bestätigt1)>=@dateFrom And (Fertigung.Termin_Bestätigt1)<=@dateTo) 
                               AND ((Fertigung.Lagerort_id)=4 Or (Fertigung.Lagerort_id)=7) AND ((Fertigung.gebucht)=1) 
                               AND ((Fertigung.Kennzeichen)='offen') AND ((artikel_kalkulatorische_kosten.Kostenart)='Arbeitskosten')) 
                               OR (((Fertigung.Termin_Bestätigt1)='31/12/1900') AND ((Fertigung.gebucht)=1) AND ((Fertigung.Kennzeichen)='offen') 
                               AND ((artikel_kalkulatorische_kosten.Kostenart)='Arbeitskosten'))
                               ORDER BY [View_PSZ_Artikel Kundenzuweisung2].Kunde, Fertigung.Termin_Bestätigt1;	";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("dateFrom", dateFrom);
				sqlCommand.Parameters.AddWithValue("dateTo", dateTo);
				sqlCommand.Parameters.AddWithValue("tage", tage);
				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.KapazitatKurzEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.KapazitatKurzEntity>();
			}
		}


		public static List<Infrastructure.Data.Entities.Joins.CTS.KapazitatKurzEntity> GetKapazitatALKurz(DateTime? dateFrom, DateTime? dateTo, int? tage)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT [View_PSZ_Artikel Kundenzuweisung2].Kunde, Fertigung.Fertigungsnummer AS [FA#],
                               Fertigung.Termin_Fertigstellung AS Wunschtermin, Fertigung.Termin_Bestätigt1 AS TerminProd,
                               Fertigung.Anzahl AS Auftragsmenge, Artikel.Artikelnummer, Artikel.[Bezeichnung 1] AS [Kunden#],
                               Artikel.Produktionszeit*Fertigung.Anzahl/60 AS [Auftragszeit(h)], Fertigung.Preis*Fertigung.Anzahl AS UmsatzCZ,
                               (Artikel.Produktionszeit*Fertigung.Anzahl/60)/(@tage*6) AS AnzahlMA, Fertigung.Lagerort_id, Artikel.Stundensatz
                               FROM ((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
                               INNER JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]) 
                               INNER JOIN [View_PSZ_Artikel Kundenzuweisung2] ON Artikel.Artikelnummer = [View_PSZ_Artikel Kundenzuweisung2].Artikelnummer
                               WHERE (((Fertigung.Termin_Bestätigt1)>=@dateFrom And (Fertigung.Termin_Bestätigt1)<=@dateTo) 
                               AND ((Fertigung.Lagerort_id)=26) AND ((Fertigung.gebucht)=1) AND ((Fertigung.Kennzeichen)='offen') 
                               AND ((artikel_kalkulatorische_kosten.Kostenart)='Arbeitskosten')) OR (((Fertigung.Termin_Bestätigt1)='31/12/1900') 
                               AND ((Fertigung.gebucht)=1) AND ((Fertigung.Kennzeichen)='offen') AND ((artikel_kalkulatorische_kosten.Kostenart)='Arbeitskosten'))
                               ORDER BY [View_PSZ_Artikel Kundenzuweisung2].Kunde, Fertigung.Termin_Bestätigt1;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("dateFrom", dateFrom);
				sqlCommand.Parameters.AddWithValue("dateTo", dateTo);
				sqlCommand.Parameters.AddWithValue("tage", tage);
				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.KapazitatKurzEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.KapazitatKurzEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.CTS.KapazitatKurzEntity> GetKapazitatBETNKurz(DateTime? dateFrom, DateTime? dateTo, int? tage)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT [View_PSZ_Artikel Kundenzuweisung2].Kunde, Fertigung.Fertigungsnummer AS [FA#],
                               Fertigung.Termin_Fertigstellung AS Wunschtermin, Fertigung.Termin_Bestätigt1 AS TerminProd,
                               Fertigung.Anzahl AS Auftragsmenge, Artikel.Artikelnummer, Artikel.[Bezeichnung 1] AS [Kunden#],
                               Artikel.Produktionszeit*Fertigung.Anzahl/60 AS [Auftragszeit(h)], Fertigung.Preis*Fertigung.Anzahl AS UmsatzCZ,
                               (Artikel.Produktionszeit*Fertigung.Anzahl/60)/(@tage*6.75) AS AnzahlMA, Fertigung.Lagerort_id, Artikel.Stundensatz
                               FROM ((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
                               INNER JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]) 
                               INNER JOIN [View_PSZ_Artikel Kundenzuweisung2] ON Artikel.Artikelnummer = [View_PSZ_Artikel Kundenzuweisung2].Artikelnummer
                               WHERE (((Fertigung.Termin_Bestätigt1)>=@dateFrom And (Fertigung.Termin_Bestätigt1)<=@dateTo) 
                               AND ((Fertigung.Lagerort_id)=58 Or (Fertigung.Lagerort_id)=60) AND ((Fertigung.gebucht)=1) 
                               AND ((Fertigung.Kennzeichen)='offen') AND ((artikel_kalkulatorische_kosten.Kostenart)='Arbeitskosten')) 
                               OR (((Fertigung.Termin_Bestätigt1)='31/12/1900') AND ((Fertigung.gebucht)=1) AND ((Fertigung.Kennzeichen)='offen') 
                               AND ((artikel_kalkulatorische_kosten.Kostenart)='Arbeitskosten'))
                               ORDER BY [View_PSZ_Artikel Kundenzuweisung2].Kunde, Fertigung.Termin_Bestätigt1;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("dateFrom", dateFrom);
				sqlCommand.Parameters.AddWithValue("dateTo", dateTo);
				sqlCommand.Parameters.AddWithValue("tage", tage);
				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.KapazitatKurzEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.KapazitatKurzEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.KapazitatKurzEntity> GetKapazitatGZTNKurz(DateTime? dateFrom, DateTime? dateTo, int? tage)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT [View_PSZ_Artikel Kundenzuweisung2].Kunde, Fertigung.Fertigungsnummer AS [FA#],
                               Fertigung.Termin_Fertigstellung AS Wunschtermin, Fertigung.Termin_Bestätigt1 AS TerminProd,
                               Fertigung.Anzahl AS Auftragsmenge, Artikel.Artikelnummer, Artikel.[Bezeichnung 1] AS [Kunden#],
                               Artikel.Produktionszeit*Fertigung.Anzahl/60 AS [Auftragszeit(h)], Fertigung.Preis*Fertigung.Anzahl AS UmsatzCZ,
                               (Artikel.Produktionszeit*Fertigung.Anzahl/60)/(@tage*6.75) AS AnzahlMA, Fertigung.Lagerort_id, Artikel.Stundensatz
                               FROM ((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
                               INNER JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]) 
                               INNER JOIN [View_PSZ_Artikel Kundenzuweisung2] ON Artikel.Artikelnummer = [View_PSZ_Artikel Kundenzuweisung2].Artikelnummer
                               WHERE (((Fertigung.Termin_Bestätigt1)>=@dateFrom And (Fertigung.Termin_Bestätigt1)<=@dateTo) 
                               AND ((Fertigung.Lagerort_id)=101 Or (Fertigung.Lagerort_id)=102) AND ((Fertigung.gebucht)=1) 
                               AND ((Fertigung.Kennzeichen)='offen') AND ((artikel_kalkulatorische_kosten.Kostenart)='Arbeitskosten')) 
                               OR (((Fertigung.Termin_Bestätigt1)='31/12/1900') AND ((Fertigung.gebucht)=1) AND ((Fertigung.Kennzeichen)='offen') 
                               AND ((artikel_kalkulatorische_kosten.Kostenart)='Arbeitskosten'))
                               ORDER BY [View_PSZ_Artikel Kundenzuweisung2].Kunde, Fertigung.Termin_Bestätigt1;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("dateFrom", dateFrom);
				sqlCommand.Parameters.AddWithValue("dateTo", dateTo);
				sqlCommand.Parameters.AddWithValue("tage", tage);
				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.KapazitatKurzEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.KapazitatKurzEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.CTS.KapazitatKurzEntity> GetKapazitatKHTNKurz(DateTime? dateFrom, DateTime? dateTo, int? tage)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT [View_PSZ_Artikel Kundenzuweisung2].Kunde, Fertigung.Fertigungsnummer AS [FA#],
                               Fertigung.Termin_Fertigstellung AS Wunschtermin, Fertigung.Termin_Bestätigt1 AS TerminProd,
                               Fertigung.Anzahl AS Auftragsmenge, Artikel.Artikelnummer, Artikel.[Bezeichnung 1] AS [Kunden#],
                               Artikel.Produktionszeit*Fertigung.Anzahl/60 AS [Auftragszeit(h)], Fertigung.Preis*Fertigung.Anzahl AS UmsatzCZ,
                               (Artikel.Produktionszeit*Fertigung.Anzahl/60)/(@tage*6.75) AS AnzahlMA, Fertigung.Lagerort_id, Artikel.Stundensatz
                               FROM ((Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
                               INNER JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]) 
                               INNER JOIN [View_PSZ_Artikel Kundenzuweisung2] ON Artikel.Artikelnummer = [View_PSZ_Artikel Kundenzuweisung2].Artikelnummer
                               WHERE (((Fertigung.Termin_Bestätigt1)>=@dateFrom And (Fertigung.Termin_Bestätigt1)<=@dateTo) 
                               AND ((Fertigung.Lagerort_id)=41 Or (Fertigung.Lagerort_id)=42) AND ((Fertigung.gebucht)=1) 
                               AND ((Fertigung.Kennzeichen)='offen') AND ((artikel_kalkulatorische_kosten.Kostenart)='Arbeitskosten')) 
                               OR (((Fertigung.Termin_Bestätigt1)='31/12/1900') AND ((Fertigung.gebucht)=1) AND ((Fertigung.Kennzeichen)='offen') 
                               AND ((artikel_kalkulatorische_kosten.Kostenart)='Arbeitskosten'))
                               ORDER BY [View_PSZ_Artikel Kundenzuweisung2].Kunde, Fertigung.Termin_Bestätigt1;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("dateFrom", dateFrom);
				sqlCommand.Parameters.AddWithValue("dateTo", dateTo);
				sqlCommand.Parameters.AddWithValue("tage", tage);
				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.KapazitatKurzEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.KapazitatKurzEntity>();
			}
		}


		public static List<Infrastructure.Data.Entities.Joins.CTS.ExportEntity> GetExportCZ(DateTime? dateFrom, DateTime? dateTo, string article, int grouping)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT Fertigung.Preis, [PSZ_Lieferliste täglich].Liefermenge_QRCode*Fertigung.Preis AS Gesamtpreis,
                               [PSZ_Lieferliste täglich].Datum, [PSZ_Lieferliste täglich].Artikelnummer, [PSZ_Lieferliste täglich].Fertigungsnummer,
                               [PSZ_Lieferliste täglich].Uhrzeit, [PSZ_Lieferliste täglich].Bezeichnung, [PSZ_Lieferliste täglich].Auftragsbemerkung,
                               [PSZ_Lieferliste täglich].Anzahl_Auftrag, [PSZ_Lieferliste täglich].[Anzahl_aktuelle Lieferung], [PSZ_Lieferliste täglich].Anzahl_Kartons,
                               [PSZ_Lieferliste täglich].Mitarbeiter_Name, [PSZ_Lieferliste täglich].Bemerkung, @dateTo AS Ausdr1,
                               @dateFrom AS Ausdr2, Lagerorte.Lagerort, [PSZ_Nummerschlüssel Kunde].[CS Kontakt], Fertigung.Kennzeichen,
                               [PSZ_Lieferliste täglich].Liefermenge_QRCode
                               FROM [PSZ_Nummerschlüssel Kunde], ([PSZ_Lieferliste täglich] 
                               INNER JOIN Fertigung ON [PSZ_Lieferliste täglich].Fertigungsnummer = Fertigung.Fertigungsnummer) 
                               INNER JOIN Lagerorte ON Fertigung.Lagerort_id = Lagerorte.Lagerort_id
                               WHERE ((([PSZ_Lieferliste täglich].Datum)>=@dateFrom And ([PSZ_Lieferliste täglich].Datum)<=@dateTo) 
                               AND (([PSZ_Lieferliste täglich].Artikelnummer) Like @article) 
                               AND (([PSZ_Lieferliste täglich].Liefermenge_QRCode) Is Not Null) 
                               AND (((LEFT([PSZ_Lieferliste täglich].[Artikelnummer], (CASE WHEN CHARINDEX('-',[PSZ_Lieferliste täglich].[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',[PSZ_Lieferliste täglich].[Artikelnummer],0)-1 END))))=[PSZ_Nummerschlüssel Kunde].[Nummerschlüssel]))";
				if(grouping == 1)
					query += "ORDER BY [PSZ_Lieferliste täglich].Artikelnummer,[PSZ_Lieferliste täglich].Fertigungsnummer,[PSZ_Lieferliste täglich].Datum,[PSZ_Lieferliste täglich].Uhrzeit;";
				else
					query += "ORDER BY [PSZ_Lieferliste täglich].Datum,[PSZ_Lieferliste täglich].Uhrzeit,[PSZ_Lieferliste täglich].Artikelnummer,[PSZ_Lieferliste täglich].Fertigungsnummer;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("dateFrom", dateFrom);
				sqlCommand.Parameters.AddWithValue("dateTo", dateTo);
				sqlCommand.Parameters.AddWithValue("article", "%" + article + "%");
				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.ExportEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.ExportEntity>();
			}
		}


		public static List<Infrastructure.Data.Entities.Joins.CTS.ExportEntity> GetExportTN(DateTime? dateFrom, DateTime? dateTo, string article)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT Fertigung.Preis, [PSZTN_Lieferliste täglich].Liefermenge_QRCode*Fertigung.Preis AS Gesamtpreis,
                               [PSZTN_Lieferliste täglich].Datum, [PSZTN_Lieferliste täglich].Artikelnummer, [PSZTN_Lieferliste täglich].Fertigungsnummer,
                               [PSZTN_Lieferliste täglich].Uhrzeit, [PSZTN_Lieferliste täglich].Bezeichnung, [PSZTN_Lieferliste täglich].Auftragsbemerkung,
                               [PSZTN_Lieferliste täglich].Anzahl_Auftrag, [PSZTN_Lieferliste täglich].[Anzahl_aktuelle Lieferung],
                               [PSZTN_Lieferliste täglich].Anzahl_Kartons, [PSZTN_Lieferliste täglich].Mitarbeiter_Name, [PSZTN_Lieferliste täglich].Bemerkung,
                               @dateFrom AS Ausdr1, @dateTo AS Ausdr2,Lagerorte.Lagerort,[PSZ_Nummerschlüssel Kunde].[CS Kontakt],
                               Fertigung.Kennzeichen,[PSZTN_Lieferliste täglich].Liefermenge_QRCode
                               FROM [PSZ_Nummerschlüssel Kunde], [PSZTN_Lieferliste täglich] 
                               INNER JOIN Fertigung ON [PSZTN_Lieferliste täglich].Fertigungsnummer = Fertigung.Fertigungsnummer
                               INNER JOIN Lagerorte ON Fertigung.Lagerort_id = Lagerorte.Lagerort_id
                               WHERE ((([PSZTN_Lieferliste täglich].Datum)>=@dateFrom And ([PSZTN_Lieferliste täglich].Datum)<=@dateTo) 
                               AND (([PSZTN_Lieferliste täglich].Artikelnummer) Like @article) AND (([PSZTN_Lieferliste täglich].Liefermenge_QRCode) Is Not Null)
                               AND (((LEFT([PSZTN_Lieferliste täglich].[Artikelnummer], (CASE WHEN CHARINDEX('-',[PSZTN_Lieferliste täglich].[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',[PSZTN_Lieferliste täglich].[Artikelnummer],0)-1 END))))=[PSZ_Nummerschlüssel Kunde].[Nummerschlüssel]))
                               ORDER BY [PSZTN_Lieferliste täglich].Artikelnummer,[PSZTN_Lieferliste täglich].Fertigungsnummer,[PSZTN_Lieferliste täglich].Datum, 
                              [PSZTN_Lieferliste täglich].Uhrzeit;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("dateFrom", dateFrom);
				sqlCommand.Parameters.AddWithValue("dateTo", dateTo);
				sqlCommand.Parameters.AddWithValue("article", "%" + article + "%");
				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.ExportEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.ExportEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.CTS.ExportEntity> GetExportAL(DateTime? dateFrom, DateTime? dateTo, string article, int grouping)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT Fertigung.Preis, [PSZAL_Lieferliste täglich].Liefermenge_QRCode*Fertigung.Preis AS Gesamtpreis,
                               [PSZAL_Lieferliste täglich].Datum, [PSZAL_Lieferliste täglich].Artikelnummer, [PSZAL_Lieferliste täglich].Fertigungsnummer,
                               [PSZAL_Lieferliste täglich].Uhrzeit, [PSZAL_Lieferliste täglich].Bezeichnung, [PSZAL_Lieferliste täglich].Auftragsbemerkung,
                               [PSZAL_Lieferliste täglich].Anzahl_Auftrag, [PSZAL_Lieferliste täglich].[Anzahl_aktuelle Lieferung], [PSZAL_Lieferliste täglich].Anzahl_Kartons,
                               [PSZAL_Lieferliste täglich].Mitarbeiter_Name, [PSZAL_Lieferliste täglich].Bemerkung, @dateTo AS Ausdr1,
                               @dateFrom AS Ausdr2, Lagerorte.Lagerort, [PSZ_Nummerschlüssel Kunde].[CS Kontakt], Fertigung.Kennzeichen,
                               [PSZAL_Lieferliste täglich].Liefermenge_QRCode
                               FROM [PSZ_Nummerschlüssel Kunde], ([PSZAL_Lieferliste täglich] 
                               INNER JOIN Fertigung ON [PSZAL_Lieferliste täglich].Fertigungsnummer = Fertigung.Fertigungsnummer) 
                               INNER JOIN Lagerorte ON Fertigung.Lagerort_id = Lagerorte.Lagerort_id
                               WHERE ((([PSZAL_Lieferliste täglich].Datum)>=@dateFrom And ([PSZAL_Lieferliste täglich].Datum)<=@dateTo) 
                               AND (([PSZAL_Lieferliste täglich].Artikelnummer) Like @article) 
                               AND (([PSZAL_Lieferliste täglich].Liefermenge_QRCode) Is Not Null) 
                               AND (((LEFT([PSZAL_Lieferliste täglich].[Artikelnummer], (CASE WHEN CHARINDEX('-',[PSZAL_Lieferliste täglich].[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',[PSZAL_Lieferliste täglich].[Artikelnummer],0)-1 END))))=[PSZ_Nummerschlüssel Kunde].[Nummerschlüssel]))";
				if(grouping == 1)
					query += "ORDER BY [PSZAL_Lieferliste täglich].Artikelnummer,[PSZAL_Lieferliste täglich].Fertigungsnummer,[PSZAL_Lieferliste täglich].Datum,[PSZAL_Lieferliste täglich].Uhrzeit;";
				else
					query += "ORDER BY [PSZAL_Lieferliste täglich].Datum,[PSZAL_Lieferliste täglich].Uhrzeit,[PSZAL_Lieferliste täglich].Artikelnummer,[PSZAL_Lieferliste täglich].Fertigungsnummer;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("dateFrom", dateFrom);
				sqlCommand.Parameters.AddWithValue("dateTo", dateTo);
				sqlCommand.Parameters.AddWithValue("article", "%" + article + "%");
				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.ExportEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.ExportEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.CTS.ExportEntity> GetExportKHTN(DateTime? dateFrom, DateTime? dateTo, string article)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT Fertigung.Preis, [PSZKsarHelal_Lieferliste täglich].Liefermenge_QRCode*Fertigung.Preis AS Gesamtpreis,
                               [PSZKsarHelal_Lieferliste täglich].Datum, [PSZKsarHelal_Lieferliste täglich].Artikelnummer, [PSZKsarHelal_Lieferliste täglich].Fertigungsnummer,
                               [PSZKsarHelal_Lieferliste täglich].Uhrzeit, [PSZKsarHelal_Lieferliste täglich].Bezeichnung, [PSZKsarHelal_Lieferliste täglich].Auftragsbemerkung,
                               [PSZKsarHelal_Lieferliste täglich].Anzahl_Auftrag, [PSZKsarHelal_Lieferliste täglich].[Anzahl_aktuelle Lieferung],
                               [PSZKsarHelal_Lieferliste täglich].Anzahl_Kartons, [PSZKsarHelal_Lieferliste täglich].Mitarbeiter_Name, [PSZKsarHelal_Lieferliste täglich].Bemerkung,
                               @dateFrom AS Ausdr1, @dateTo AS Ausdr2,Lagerorte.Lagerort,[PSZ_Nummerschlüssel Kunde].[CS Kontakt],
                               Fertigung.Kennzeichen,[PSZKsarHelal_Lieferliste täglich].Liefermenge_QRCode
                               FROM [PSZ_Nummerschlüssel Kunde], [PSZKsarHelal_Lieferliste täglich] 
                               INNER JOIN Fertigung ON [PSZKsarHelal_Lieferliste täglich].Fertigungsnummer = Fertigung.Fertigungsnummer
                               INNER JOIN Lagerorte ON Fertigung.Lagerort_id = Lagerorte.Lagerort_id
                               WHERE ((([PSZKsarHelal_Lieferliste täglich].Datum)>=@dateFrom And ([PSZKsarHelal_Lieferliste täglich].Datum)<=@dateTo) 
                               AND (([PSZKsarHelal_Lieferliste täglich].Artikelnummer) Like @article) AND (([PSZKsarHelal_Lieferliste täglich].Liefermenge_QRCode) Is Not Null)
                               AND (((LEFT([PSZKsarHelal_Lieferliste täglich].[Artikelnummer], (CASE WHEN CHARINDEX('-',[PSZKsarHelal_Lieferliste täglich].[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',[PSZKsarHelal_Lieferliste täglich].[Artikelnummer],0)-1 END))))=[PSZ_Nummerschlüssel Kunde].[Nummerschlüssel]))
                               ORDER BY [PSZKsarHelal_Lieferliste täglich].Artikelnummer,[PSZKsarHelal_Lieferliste täglich].Fertigungsnummer,[PSZKsarHelal_Lieferliste täglich].Datum, 
                              [PSZKsarHelal_Lieferliste täglich].Uhrzeit;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("dateFrom", dateFrom);
				sqlCommand.Parameters.AddWithValue("dateTo", dateTo);
				sqlCommand.Parameters.AddWithValue("article", "%" + article + "%");
				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.ExportEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.ExportEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.CTS.ExportEntity> GetExportBETN(DateTime? dateFrom, DateTime? dateTo, string article)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT Fertigung.Preis, [PSZBETN_Lieferliste täglich].Liefermenge_QRCode*Fertigung.Preis AS Gesamtpreis,
                               [PSZBETN_Lieferliste täglich].Datum, [PSZBETN_Lieferliste täglich].Artikelnummer, [PSZBETN_Lieferliste täglich].Fertigungsnummer,
                               [PSZBETN_Lieferliste täglich].Uhrzeit, [PSZBETN_Lieferliste täglich].Bezeichnung, [PSZBETN_Lieferliste täglich].Auftragsbemerkung,
                               [PSZBETN_Lieferliste täglich].Anzahl_Auftrag, [PSZBETN_Lieferliste täglich].[Anzahl_aktuelle Lieferung],
                               [PSZBETN_Lieferliste täglich].Anzahl_Kartons, [PSZBETN_Lieferliste täglich].Mitarbeiter_Name, [PSZBETN_Lieferliste täglich].Bemerkung,
                               @dateFrom AS Ausdr1, @dateTo AS Ausdr2,Lagerorte.Lagerort,[PSZ_Nummerschlüssel Kunde].[CS Kontakt],
                               Fertigung.Kennzeichen,[PSZBETN_Lieferliste täglich].Liefermenge_QRCode
                               FROM [PSZ_Nummerschlüssel Kunde], [PSZBETN_Lieferliste täglich] 
                               INNER JOIN Fertigung ON [PSZBETN_Lieferliste täglich].Fertigungsnummer = Fertigung.Fertigungsnummer
                               INNER JOIN Lagerorte ON Fertigung.Lagerort_id = Lagerorte.Lagerort_id
                               WHERE ((([PSZBETN_Lieferliste täglich].Datum)>=@dateFrom And ([PSZBETN_Lieferliste täglich].Datum)<=@dateTo) 
                               AND (([PSZBETN_Lieferliste täglich].Artikelnummer) Like @article) AND (([PSZBETN_Lieferliste täglich].Liefermenge_QRCode) Is Not Null)
                               AND (((LEFT([PSZBETN_Lieferliste täglich].[Artikelnummer], (CASE WHEN CHARINDEX('-',[PSZBETN_Lieferliste täglich].[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',[PSZBETN_Lieferliste täglich].[Artikelnummer],0)-1 END))))=[PSZ_Nummerschlüssel Kunde].[Nummerschlüssel]))
                               ORDER BY [PSZBETN_Lieferliste täglich].Artikelnummer,[PSZBETN_Lieferliste täglich].Fertigungsnummer,[PSZBETN_Lieferliste täglich].Datum, 
                              [PSZBETN_Lieferliste täglich].Uhrzeit;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("dateFrom", dateFrom);
				sqlCommand.Parameters.AddWithValue("dateTo", dateTo);
				sqlCommand.Parameters.AddWithValue("article", "%" + article + "%");
				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.ExportEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.ExportEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.ExportEntity> GetExportGZTN(DateTime? dateFrom, DateTime? dateTo, string article)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT Fertigung.Preis, [PSZGZTN_Lieferliste täglich].Liefermenge_QRCode*Fertigung.Preis AS Gesamtpreis,
									[PSZGZTN_Lieferliste täglich].Datum, [PSZGZTN_Lieferliste täglich].Artikelnummer, [PSZGZTN_Lieferliste täglich].Fertigungsnummer,
									[PSZGZTN_Lieferliste täglich].Uhrzeit, [PSZGZTN_Lieferliste täglich].Bezeichnung, [PSZGZTN_Lieferliste täglich].Auftragsbemerkung,
									[PSZGZTN_Lieferliste täglich].Anzahl_Auftrag, [PSZGZTN_Lieferliste täglich].[Anzahl_aktuelle Lieferung],
									[PSZGZTN_Lieferliste täglich].Anzahl_Kartons, [PSZGZTN_Lieferliste täglich].Mitarbeiter_Name, [PSZGZTN_Lieferliste täglich].Bemerkung,
									@dateFrom AS Ausdr1, @dateTo AS Ausdr2,Lagerorte.Lagerort,[PSZ_Nummerschlüssel Kunde].[CS Kontakt],
									Fertigung.Kennzeichen,[PSZGZTN_Lieferliste täglich].Liefermenge_QRCode
									FROM [PSZ_Nummerschlüssel Kunde], [PSZGZTN_Lieferliste täglich] 
									INNER JOIN Fertigung ON [PSZGZTN_Lieferliste täglich].Fertigungsnummer = Fertigung.Fertigungsnummer
									INNER JOIN Lagerorte ON Fertigung.Lagerort_id = Lagerorte.Lagerort_id
									WHERE ((([PSZGZTN_Lieferliste täglich].Datum)>=@dateFrom And ([PSZGZTN_Lieferliste täglich].Datum)<=@dateTo) 
									AND (([PSZGZTN_Lieferliste täglich].Artikelnummer) Like @article) AND (([PSZGZTN_Lieferliste täglich].Liefermenge_QRCode) Is Not Null)
									AND (((LEFT([PSZGZTN_Lieferliste täglich].[Artikelnummer], (CASE WHEN CHARINDEX('-',[PSZGZTN_Lieferliste täglich].[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',[PSZGZTN_Lieferliste täglich].[Artikelnummer],0)-1 END))))=[PSZ_Nummerschlüssel Kunde].[Nummerschlüssel]))
									ORDER BY [PSZGZTN_Lieferliste täglich].Artikelnummer,[PSZGZTN_Lieferliste täglich].Fertigungsnummer,[PSZGZTN_Lieferliste täglich].Datum, 
									[PSZGZTN_Lieferliste täglich].Uhrzeit;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("dateFrom", dateFrom);
				sqlCommand.Parameters.AddWithValue("dateTo", dateTo);
				sqlCommand.Parameters.AddWithValue("article", "%" + article + "%");
				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.ExportEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.ExportEntity>();
			}
		}


		public static List<Infrastructure.Data.Entities.Joins.CTS.BackLogFGEntity> GetBacklogFG_Multi(DateTime dateFrom, DateTime dateTo, List<int> lagers)
		{
			if(lagers == null || lagers.Count <= 0)
				return null;

			// - 
			var results = new List<Infrastructure.Data.Entities.Joins.CTS.BackLogFGEntity>();
			foreach(var lager in lagers)
			{
				var r = GetBacklogFG(dateFrom, dateTo, lager);
				if(r != null && r.Count > 0)
				{
					results.AddRange(r);
				}
			}

			// -
			return results;

			// return getBacklogFG(dateFrom, dateTo, lagers);
		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.BackLogFGEntity> GetBacklogFG_Multi2(DateTime dateFrom, DateTime dateTo, List<int> lagers)
		{
			if(lagers == null || lagers.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var query = getBacklogMultiQueryOptimized(dateFrom, dateTo.Date, lagers);
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 480;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.BackLogFGEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.BackLogFGEntity>();
			}
		}
		static string getBacklogMultiQueryOptimized(DateTime dateFrom, DateTime dateTo, List<int> lagers)
		{
			string query = "";

			if(lagers?.Count > 0)
			{
				query = $@"
						DECLARE @vom AS datetime='{dateFrom.ToString("yyyyMMdd")}';
						DECLARE @bis AS datetime='{dateTo.ToString("yyyyMMdd")}';
						/* Simulate a set of active Lagerort_ids */
						DECLARE @LagerList TABLE (Lagerort_id INT);
						INSERT INTO @LagerList (Lagerort_id) VALUES {string.Join(",", lagers.Select((l, i) => $"({l})"))};

						;WITH FilteredBase AS (
							SELECT 
								BIV.Name1,
								BIV.[Angebot-Nr],
								BIV.Nr,
								BIV.[Artikel-Nr],
								BIV.[Bezeichnung 1],
								BIV.Bezeichnung1,
								BIV.Anzahl,
								BIV.Liefertermin,
								BIV.Einzelpreis,
								BIV.Preiseinheit,
								BIV.Gesamtpreis,
								@vom AS Ausdr2,
								@bis AS Ausdr1,
								BIV.Anzahl * GEB.Kosten AS Gesamtkosten,
								BIV.Stückliste,
								BIV.Kostenart,
								BIV.Betrag,
								BIV.Anzahl * BIV.Betrag AS Gesamtpersonalkosten,
								BIV.Artikelnummer,
								BIV.Fertigungsnummer,
								BIV.OriginalAnzahl,
								BIV.Mandant,
								CASE WHEN BIV.Stückliste = 1 THEN GEB.Kosten ELSE Einkaufspreis * 1.05 END AS Kosten, 
								BIV.Lagerort,
								BIV.[DOC Number]
							FROM 
								[PSZ_View_Offene Aufträge ohne FA mit Backlog IV] AS BIV
								INNER JOIN [PSZ_offene Aufträge ohne FA mit Backlog Gewinn ermitteln III] AS GEB
									ON BIV.Artikelnummer = GEB.Artikelnummer
							WHERE 
								BIV.Liefertermin BETWEEN @vom AND @bis
								AND EXISTS (
									SELECT 1 
									FROM @LagerList L 
									WHERE BIV.Lagerort_id IN (
										/* mapping logic simulating original IIF expressions */
										CASE WHEN L.Lagerort_id = 6 THEN 6 ELSE NULL END,
										CASE WHEN L.Lagerort_id = 6 THEN 3 ELSE NULL END,
										CASE WHEN L.Lagerort_id = 7 THEN 7 ELSE NULL END,
										CASE WHEN L.Lagerort_id = 7 THEN 4 ELSE NULL END,
										CASE WHEN L.Lagerort_id = 15 THEN 15 ELSE NULL END,
										CASE WHEN L.Lagerort_id = 15 THEN 14 ELSE NULL END,
										CASE WHEN L.Lagerort_id = 21 THEN 21 ELSE NULL END,
										CASE WHEN L.Lagerort_id = 21 THEN 20 ELSE NULL END,
										CASE WHEN L.Lagerort_id = 26 THEN 26 ELSE NULL END,
										CASE WHEN L.Lagerort_id = 26 THEN 24 ELSE NULL END,
										CASE WHEN L.Lagerort_id = 42 THEN 42 ELSE NULL END,
										CASE WHEN L.Lagerort_id = 42 THEN 41 ELSE NULL END,
										CASE WHEN L.Lagerort_id = 60 THEN 60 ELSE NULL END,
										CASE WHEN L.Lagerort_id = 60 THEN 58 ELSE NULL END,
										CASE WHEN L.Lagerort_id = 102 THEN 102 ELSE NULL END,
										CASE WHEN L.Lagerort_id = 102 THEN 101 ELSE NULL END
									)
								)
						)

						SELECT *,
							/* Map warehouse to PRORT label */
							CASE Lagerort
								WHEN 'Hauptlager/CZ' THEN 'Eigenfertigung'
								WHEN 'Hauptlager/TN' THEN 'Tunesien'
								WHEN 'Hauptlager/BE_TN' THEN 'BETN'
								WHEN 'Hauptlager/WS' THEN 'WS'
								WHEN 'Hauptlager/GZ' THEN 'GZTN'
								WHEN 'Hauptlager/AL' THEN 'Albanien'
								WHEN 'Hauptlager/DE' THEN 'Deutschland'
								ELSE 'Sonderfertigung'
							END AS PRORT
						FROM FilteredBase
						ORDER BY [Artikel-Nr], Liefertermin;
						";
			}

			// - 
			return query;
		}
		static string getBacklogMultiQuery(DateTime dateFrom, DateTime dateTo, List<int> lagers)
		{
			string query = "";

			if(lagers?.Count > 0)
			{
				query = $@"
						DECLARE @vom AS datetime='{dateFrom.ToString("yyyyMMdd")}';
						DECLARE @bis AS datetime='{dateTo.ToString("yyyyMMdd")}';
						{string.Join("\n", lagers.Select((l, i) => $"DECLARE @lager_{i} AS INT={l};"))}

						SELECT * FROM (
						{string.Join("\n UNION ALL \n", lagers.Select((_, i) => getBacklogMultiSingleQuery(i)))}
						) AS TMP 
						ORDER BY [Artikel-Nr],Liefertermin;
						";
			}

			// - 
			return query;
		}
		static string getBacklogMultiSingleQuery(int i)
		{
			return $@"
						SELECT [PSZ_Offene Auftraege ohne FA mit Backlog].*, 
						IIf([PSZ_Offene Auftraege ohne FA mit Backlog].Lagerort='Hauptlager/CZ','Eigenfertigung',
						IIf([PSZ_Offene Auftraege ohne FA mit Backlog].Lagerort='Hauptlager/TN' ,'Tunesien',
						IIf([PSZ_Offene Auftraege ohne FA mit Backlog].Lagerort='Hauptlager/BE_TN' ,'BETN',
						IIf([PSZ_Offene Auftraege ohne FA mit Backlog].Lagerort='Hauptlager/WS' ,'WS',
						IIf([PSZ_Offene Auftraege ohne FA mit Backlog].Lagerort='Hauptlager/GZ' ,'GZTN',
						IIf([PSZ_Offene Auftraege ohne FA mit Backlog].Lagerort='Hauptlager/AL','Albanien',
						IIf([PSZ_Offene Auftraege ohne FA mit Backlog].Lagerort='Hauptlager/DE','Deutschland','Sonderfertigung'))))))) AS PRORT
						FROM (
						SELECT [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Name1, [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].[Angebot-Nr],
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Nr, [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].[Artikel-Nr],
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].[Bezeichnung 1], [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Bezeichnung1,
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Anzahl, [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin,
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Einzelpreis, [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Preiseinheit,
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Gesamtpreis, @vom AS Ausdr2, @bis AS Ausdr1, [Anzahl]*[PSZ_offene Aufträge ohne FA mit Backlog Gewinn ermitteln III].[Kosten] AS Gesamtkosten,
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Stückliste, [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Kostenart,
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Betrag, [Anzahl]*[Betrag] AS Gesamtpersonalkosten, 
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Artikelnummer, [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Fertigungsnummer,
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].OriginalAnzahl, [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Mandant,
						IIf([Stückliste]=1,[PSZ_offene Aufträge ohne FA mit Backlog Gewinn ermitteln III].Kosten,[Einkaufspreis]*1.05) AS Kosten,
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort,
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].[DOC Number] 
						FROM [PSZ_View_Offene Aufträge ohne FA mit Backlog IV] INNER JOIN [PSZ_offene Aufträge ohne FA mit Backlog Gewinn ermitteln III] 
						ON [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Artikelnummer = [PSZ_offene Aufträge ohne FA mit Backlog Gewinn ermitteln III].Artikelnummer
						WHERE ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
						And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
						AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager_{i}=6,3,0) 
						Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager_{i}=6,6,0))) 
						OR ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
						And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
						AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager_{i}=7,4,0) 
						Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager_{i}=7,7,0))) 
						OR ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
						And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
						AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager_{i}=21,21,0) 
						Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager_{i}=21,20,0))) 
						OR ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
						And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
						AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager_{i}=15,14,0) 
						Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager_{i}=15,15,0))) 
						OR ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
						And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
						AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager_{i}=26,24,0) 
						Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager_{i}=26,26,0))) 
						OR ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
						And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
						AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager_{i}=42,41,0) 
						Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager_{i}=42,42,0))) 
						OR ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
						And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
						AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager_{i}=60,58,0) 
						Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager_{i}=60,60,0))) 
						OR ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
						And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
						AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager_{i}=102,101,0) 
						Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager_{i}=102,102,0))) 
						)
						[PSZ_Offene Auftraege ohne FA mit Backlog]";
		}
		internal static List<Infrastructure.Data.Entities.Joins.CTS.BackLogFGEntity> getBacklogFG(DateTime dateFrom, DateTime dateTo, List<int> lagers)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"DECLARE @vom AS datetime='{dateFrom.ToString("yyyyMMdd")}';DECLARE @bis AS datetime='{dateTo.ToString("yyyyMMdd")}';\n\n";
				var body = new List<string>();
				for(int idx = 0; idx < lagers.Count; idx++)
				{
					query += $"DECLARE @lager{idx} AS INT={lagers[idx]};\n";
					body.Add($@"
SELECT [PSZ_Offene Auftraege ohne FA mit Backlog].*, 
IIf([PSZ_Offene Auftraege ohne FA mit Backlog].Lagerort='Hauptlager/CZ','Eigenfertigung',
IIf([PSZ_Offene Auftraege ohne FA mit Backlog].Lagerort='Hauptlager/TN' And
[PSZ_Offene Auftraege ohne FA mit Backlog].Artikelnummer Not Like '825-*','Tunesien',
IIf([PSZ_Offene Auftraege ohne FA mit Backlog].Lagerort='Hauptlager/TN' And [PSZ_Offene Auftraege ohne FA mit Backlog].Artikelnummer Like '825-*','KHTN',
IIf([PSZ_Offene Auftraege ohne FA mit Backlog].Lagerort='Hauptlager/AL','Albanien',
IIf([PSZ_Offene Auftraege ohne FA mit Backlog].Lagerort='Hauptlager/DE','Deutschland','Sonderfertigung'))))) AS PRORT
FROM (
SELECT [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Name1, [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].[Angebot-Nr],
[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Nr, [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].[Artikel-Nr],
[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].[Bezeichnung 1], [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Bezeichnung1,
[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Anzahl, [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin,
[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Einzelpreis, [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Preiseinheit,
[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Gesamtpreis, @vom AS Ausdr2, @bis AS Ausdr1, [Anzahl]*[PSZ_offene Aufträge ohne FA mit Backlog Gewinn ermitteln III].[Kosten] AS Gesamtkosten,
[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Stückliste, [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Kostenart,
[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Betrag, [Anzahl]*[Betrag] AS Gesamtpersonalkosten, 
[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Artikelnummer, [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Fertigungsnummer,
[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].OriginalAnzahl, [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Mandant,
IIf([Stückliste]=1,[PSZ_offene Aufträge ohne FA mit Backlog Gewinn ermitteln III].Kosten,[Einkaufspreis]*1.05) AS Kosten,
[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort,
[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].[DOC Number] 
FROM [PSZ_View_Offene Aufträge ohne FA mit Backlog IV] INNER JOIN [PSZ_offene Aufträge ohne FA mit Backlog Gewinn ermitteln III] 
ON [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Artikelnummer = [PSZ_offene Aufträge ohne FA mit Backlog Gewinn ermitteln III].Artikelnummer
WHERE ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager{idx}=6,3,0) 
Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager{idx}=6,6,0))) 
OR ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager{idx}=7,4,0) 
Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager{idx}=7,7,0))) 
OR ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager{idx}=21,21,0) 
Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager{idx}=21,20,0))) 
OR ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager{idx}=15,14,0) 
Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager{idx}=15,15,0))) 
OR ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager{idx}=26,24,0) 
Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager{idx}=26,26,0))) 
OR ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager{idx}=42,41,0) 
Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager{idx}=42,42,0))) 
OR ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager{idx}=60,58,0) 
Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager{idx}=60,60,0)))
) [PSZ_Offene Auftraege ohne FA mit Backlog]
");
				}
				var sqlCommand = new SqlCommand($"{query}{string.Join("\nUNION ALL\n", body)}", sqlConnection);
				sqlCommand.CommandTimeout = 300;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.BackLogFGEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.BackLogFGEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.BackLogFGEntity> GetBacklogFG(DateTime dateFrom, DateTime dateTo, int? lager)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = string.Empty;
				if(lager is null)
				{
					query = $@"DECLARE @lager AS INT = null
						DECLARE @vom AS datetime='{dateFrom.ToString("yyyyMMdd")}'
						DECLARE @bis AS datetime='{dateTo.ToString("yyyyMMdd")}'

						SELECT [PSZ_Offene Auftraege ohne FA mit Backlog].*, 
						IIf([PSZ_Offene Auftraege ohne FA mit Backlog].Lagerort='Hauptlager/CZ','Eigenfertigung',
						IIf([PSZ_Offene Auftraege ohne FA mit Backlog].Lagerort='Hauptlager/TN' ,'Tunesien',
						IIf([PSZ_Offene Auftraege ohne FA mit Backlog].Lagerort='Hauptlager/BE_TN' ,'BETN',
						IIf([PSZ_Offene Auftraege ohne FA mit Backlog].Lagerort='Hauptlager/WS' ,'WS',
						IIf([PSZ_Offene Auftraege ohne FA mit Backlog].Lagerort='Hauptlager/GZ' ,'GZTN',
						IIf([PSZ_Offene Auftraege ohne FA mit Backlog].Lagerort='Hauptlager/AL','Albanien',
						IIf([PSZ_Offene Auftraege ohne FA mit Backlog].Lagerort='Hauptlager/DE','Deutschland','Sonderfertigung'))))))) AS PRORT
						FROM (
						SELECT [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Name1, [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].[Angebot-Nr],
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Nr, [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].[Artikel-Nr],
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].[Bezeichnung 1], [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Bezeichnung1,
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Anzahl, [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin,
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Einzelpreis, [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Preiseinheit,
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Gesamtpreis, @vom AS Ausdr2, @bis AS Ausdr1, [Anzahl]*[PSZ_offene Aufträge ohne FA mit Backlog Gewinn ermitteln III].[Kosten] AS Gesamtkosten,
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Stückliste, [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Kostenart,
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Betrag, [Anzahl]*[Betrag] AS Gesamtpersonalkosten, 
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Artikelnummer, [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Fertigungsnummer,
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].OriginalAnzahl, [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Mandant,
						IIf([Stückliste]=1,[PSZ_offene Aufträge ohne FA mit Backlog Gewinn ermitteln III].Kosten,[Einkaufspreis]*1.05) AS Kosten,
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort,
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].[DOC Number] 
						FROM [PSZ_View_Offene Aufträge ohne FA mit Backlog IV] INNER JOIN [PSZ_offene Aufträge ohne FA mit Backlog Gewinn ermitteln III] 
						ON [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Artikelnummer = [PSZ_offene Aufträge ohne FA mit Backlog Gewinn ermitteln III].Artikelnummer
						WHERE ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
						And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
						AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id) IS NULL 
						Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id) IS NULL)) 
						OR ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
						And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
						AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id) IS NULL 
						Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id) IS NULL)) 
						OR ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
						And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
						AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id) IS NULL 
						Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id) IS NULL)) 
						OR ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
						And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
						AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id) IS NULL 
						Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id) IS NULL)) 
						OR ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
						And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
						AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id) IS NULL 
						Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id) IS NULL)) 
						OR ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
						And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
						AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id) IS NULL 
						Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id) IS NULL)) 
						OR ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
						And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
						AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id) IS NULL 
						Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id) IS NULL)) 
						OR ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
						And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
						AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id) IS NULL 
						Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id) IS NULL)) 
						)
						[PSZ_Offene Auftraege ohne FA mit Backlog]
						ORDER BY [PSZ_Offene Auftraege ohne FA mit Backlog].[Artikel-Nr],[PSZ_Offene Auftraege ohne FA mit Backlog].Liefertermin;";
				}
				else
				{
					query = $@"DECLARE @lager AS INT={lager}
						DECLARE @vom AS datetime='{dateFrom.ToString("yyyyMMdd")}'
						DECLARE @bis AS datetime='{dateTo.ToString("yyyyMMdd")}'

						SELECT [PSZ_Offene Auftraege ohne FA mit Backlog].*, 
						IIf([PSZ_Offene Auftraege ohne FA mit Backlog].Lagerort='Hauptlager/CZ','Eigenfertigung',
						IIf([PSZ_Offene Auftraege ohne FA mit Backlog].Lagerort='Hauptlager/TN' ,'Tunesien',
						IIf([PSZ_Offene Auftraege ohne FA mit Backlog].Lagerort='Hauptlager/BE_TN' ,'BETN',
						IIf([PSZ_Offene Auftraege ohne FA mit Backlog].Lagerort='Hauptlager/WS' ,'WS',
						IIf([PSZ_Offene Auftraege ohne FA mit Backlog].Lagerort='Hauptlager/GZ' ,'GZTN',
						IIf([PSZ_Offene Auftraege ohne FA mit Backlog].Lagerort='Hauptlager/AL','Albanien',
						IIf([PSZ_Offene Auftraege ohne FA mit Backlog].Lagerort='Hauptlager/DE','Deutschland','Sonderfertigung'))))))) AS PRORT
						FROM (
						SELECT [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Name1, [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].[Angebot-Nr],
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Nr, [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].[Artikel-Nr],
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].[Bezeichnung 1], [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Bezeichnung1,
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Anzahl, [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin,
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Einzelpreis, [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Preiseinheit,
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Gesamtpreis, @vom AS Ausdr2, @bis AS Ausdr1, [Anzahl]*[PSZ_offene Aufträge ohne FA mit Backlog Gewinn ermitteln III].[Kosten] AS Gesamtkosten,
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Stückliste, [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Kostenart,
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Betrag, [Anzahl]*[Betrag] AS Gesamtpersonalkosten, 
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Artikelnummer, [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Fertigungsnummer,
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].OriginalAnzahl, [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Mandant,
						IIf([Stückliste]=1,[PSZ_offene Aufträge ohne FA mit Backlog Gewinn ermitteln III].Kosten,[Einkaufspreis]*1.05) AS Kosten,
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort,
						[PSZ_View_Offene Aufträge ohne FA mit Backlog IV].[DOC Number] 
						FROM [PSZ_View_Offene Aufträge ohne FA mit Backlog IV] INNER JOIN [PSZ_offene Aufträge ohne FA mit Backlog Gewinn ermitteln III] 
						ON [PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Artikelnummer = [PSZ_offene Aufträge ohne FA mit Backlog Gewinn ermitteln III].Artikelnummer
						WHERE ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
						And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
						AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager=6,3,0) 
						Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager=6,6,0))) 
						OR ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
						And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
						AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager=7,4,0) 
						Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager=7,7,0))) 
						OR ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
						And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
						AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager=21,21,0) 
						Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager=21,20,0))) 
						OR ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
						And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
						AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager=15,14,0) 
						Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager=15,15,0))) 
						OR ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
						And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
						AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager=26,24,0) 
						Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager=26,26,0))) 
						OR ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
						And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
						AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager=42,41,0) 
						Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager=42,42,0))) 
						OR ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
						And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
						AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager=60,58,0) 
						Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager=60,60,0))) 
						OR ((([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)>=@vom 
						And ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Liefertermin)<=@bis) 
						AND (([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager=102,101,0) 
						Or ([PSZ_View_Offene Aufträge ohne FA mit Backlog IV].Lagerort_id)=IIf(@lager=102,102,0))) 
						)
						[PSZ_Offene Auftraege ohne FA mit Backlog]
						ORDER BY [PSZ_Offene Auftraege ohne FA mit Backlog].[Artikel-Nr],[PSZ_Offene Auftraege ohne FA mit Backlog].Liefertermin;";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.BackLogFGEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.BackLogFGEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.CTS.BacklogHWEntity> GetBacklogHW(DateTime dateFrom, DateTime dateTo)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT adressen.Name1, Angebote.[Angebot-Nr], [angebotene Artikel].erledigt_pos, Angebote.erledigt,
[angebotene Artikel].Nr, [angebotene Artikel].[Artikel-Nr], Artikel.[Bezeichnung 1], [angebotene Artikel].Bezeichnung1,
[angebotene Artikel].Anzahl, [angebotene Artikel].Liefertermin, [angebotene Artikel].Einzelpreis,
[angebotene Artikel].Preiseinheit, [angebotene Artikel].Gesamtpreis, Bestellnummern.Einkaufspreis*(1+([Gewicht]/100))*[Anzahl] AS Gesamtkosten,
Artikel.Stückliste, Bestellnummern.Einkaufspreis*(1+([Gewicht]/100)) AS Kosten, '-' AS Kostenart, 0 AS Betrag,
0 AS Gesamtpersonalkosten, Artikel.Artikelnummer, [angebotene Artikel].Fertigungsnummer, [angebotene Artikel].Lagerort_id,
Angebote.Typ, Artikel.Gewicht, Bestellnummern.Standardlieferant, @dateFrom AS Ausdr2, @dateTo AS Ausdr1,
[angebotene Artikel].OriginalAnzahl 
--INTO [PSZ_Offene Auftraege HW]
FROM (((Angebote LEFT JOIN [angebotene Artikel] ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) 
LEFT JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
LEFT JOIN adressen ON Angebote.[Kunden-Nr] = adressen.Nr) LEFT JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]
WHERE ((([angebotene Artikel].erledigt_pos)=0) AND ((Angebote.erledigt)=0) 
AND (([angebotene Artikel].Liefertermin)>=@dateFrom And ([angebotene Artikel].Liefertermin)<=@dateTo) 
AND ((Artikel.Stückliste)=0) AND ((Angebote.Typ)='Auftragsbestätigung') 
AND ((Bestellnummern.Standardlieferant)=1))
ORDER BY [angebotene Artikel].[Artikel-Nr], [angebotene Artikel].Liefertermin;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("dateFrom", dateFrom);
				sqlCommand.Parameters.AddWithValue("dateTo", dateTo);
				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.BacklogHWEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.BacklogHWEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.CTS.BestandCSProEntity> GetBestandProCSByFilter(string contact, string kunde, string lager)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT [View_PSZ_FG Bestand nach CS mit Werten].Bestand*([View_PSZ_FG kosten 02 ohne Materialzuschlag ermitteln].Materialkosten+[View_PSZ_FG Kosten 02 ohne Materialzuschlag ermitteln].Arbeitskosten) AS Wert,
                               [View_PSZ_FG Bestand nach CS mit Werten].Artikelnummer, [View_PSZ_FG Bestand nach CS mit Werten].Kunde, [View_PSZ_FG Bestand nach CS mit Werten].[CS Kontakt],
                               [View_PSZ_FG Bestand nach CS mit Werten].Lagerort_id, [View_PSZ_FG Bestand nach CS mit Werten].Bestand, [View_PSZ_FG Bestand nach CS mit Werten].Lagerort,
                               [View_PSZ_FG Bestand nach CS mit Werten].VK, [View_PSZ_FG Bestand nach CS mit Werten].Materialkosten, [View_PSZ_FG Bestand nach CS mit Werten].Arbeitskosten
                               FROM [View_PSZ_FG Bestand nach CS mit Werten] INNER JOIN [View_PSZ_FG kosten 02 ohne Materialzuschlag ermitteln] 
                               ON [View_PSZ_FG Bestand nach CS mit Werten].Artikelnummer = [View_PSZ_FG kosten 02 ohne Materialzuschlag ermitteln].Artikelnummer
                               WHERE ((([View_PSZ_FG Bestand nach CS mit Werten].Lagerort_id)<>825 And ([View_PSZ_FG Bestand nach CS mit Werten].Lagerort_id)<>928))
                               AND [CS Kontakt]LIKE @contact AND [Kunde] LIKE @kunde AND [Lagerort] LIKE @lager";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("contact", "%" + contact + "%");
				sqlCommand.Parameters.AddWithValue("kunde", "%" + kunde + "%");
				sqlCommand.Parameters.AddWithValue("lager", "%" + lager + "%");
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.BestandCSProEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.BestandCSProEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.BestandAusenlagerEntity> GetBestandAusenlager(int lager)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT [View_PSZ_FG Bestand nach CS mit Werten].Kunde, [View_PSZ_FG Bestand nach CS mit Werten].[CS Kontakt],
                              [View_PSZ_FG Bestand nach CS mit Werten].Artikelnummer, Artikel.[Bezeichnung 1], [View_PSZ_FG Bestand nach CS mit Werten].Bestand
                              *([View_PSZ_FG kosten 02 ohne Materialzuschlag ermitteln].Materialkosten
                              +[View_PSZ_FG kosten 02 ohne Materialzuschlag ermitteln].Arbeitskosten) AS Wert,
                              [View_PSZ_FG Bestand nach CS mit Werten].Bestand, [View_PSZ_FG Bestand nach CS mit Werten].Lagerort,
                              [View_PSZ_FG Bestand nach CS mit Werten].VK, [View_PSZ_FG Bestand nach CS mit Werten].Materialkosten,
                              [View_PSZ_FG Bestand nach CS mit Werten].Arbeitskosten, [View_PSZ_FG Bestand nach CS mit Werten].Lagerort_id
                              FROM ([View_PSZ_FG Bestand nach CS mit Werten] 
                              INNER JOIN [View_PSZ_FG kosten 02 ohne Materialzuschlag ermitteln] 
                              ON [View_PSZ_FG Bestand nach CS mit Werten].Artikelnummer = [View_PSZ_FG kosten 02 ohne Materialzuschlag ermitteln].Artikelnummer) 
                              INNER JOIN Artikel ON [View_PSZ_FG kosten 02 ohne Materialzuschlag ermitteln].Artikelnummer = Artikel.Artikelnummer
                              WHERE ((([View_PSZ_FG Bestand nach CS mit Werten].Lagerort_id)=@lager));";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("lager", lager);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.BestandAusenlagerEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.BestandAusenlagerEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.CTS.LagerBestandFGEntity> GetLagerBestandFG(Settings.SortingModel sorting, Settings.PaginModel paging, string artikel, string kunde)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT v.Artikelnummer, v.Kunde,
                              v.[Bezeichnung 1], v.[Bezeichnung 2],
                              v.Freigabestatus, v.[CS Kontakt],
                              v.Lagerort, v.Bestand, v.VK
                              AS [VK Gesamt], v.Gesamtpreis AS [Kosten gesamt], v.Verkaufspreis AS VKE, a.UBG
                              FROM [View_Lagerbestände Analyse L_CS] v LEFT JOIN Artikel a on a.Artikelnummer=v.Artikelnummer 
                              WHERE v.[Artikelnummer] IS NOT NULL";
				if(!string.IsNullOrEmpty(artikel) && !string.IsNullOrWhiteSpace(artikel))
				{
					query += $" AND [Artikelnummer]='{artikel}'";
				}
				if(!string.IsNullOrEmpty(kunde) && !string.IsNullOrWhiteSpace(kunde))
				{
					query += $" AND [Kunde]='{kunde}'";
				}
				#region >>>>> pagination <<<<<<<
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY [Artikelnummer] ";
				}
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				#endregion pagination

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.LagerBestandFGEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.LagerBestandFGEntity>();
			}
		}
		public static int GetLagerBestandFG_Count(string artikel, string kunde)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string baseQuery = @"SELECT v.Artikelnummer, v.Kunde,
                              v.[Bezeichnung 1], v.[Bezeichnung 2],
                              v.Freigabestatus, v.[CS Kontakt],
                              v.Lagerort, v.Bestand, v.VK
                              AS [VK Gesamt], v.Gesamtpreis_mit_cu AS [Kosten gesamt], v.Gesamtpreis_ohne_cu AS [Kosten gesamt ohne CU], v.Verkaufspreis AS VKE, a.UBG
                              FROM [View_Lagerbestände Analyse L_CS] v left join Artikel a on a.Artikelnummer=v.Artikelnummer 
                              WHERE v.[Artikelnummer] IS NOT NULL";

				if(!string.IsNullOrEmpty(artikel) && !string.IsNullOrWhiteSpace(artikel))
				{
					baseQuery += $" AND [Artikelnummer]='{artikel}'";
				}
				if(!string.IsNullOrEmpty(kunde) && !string.IsNullOrWhiteSpace(kunde))
				{
					baseQuery += $" AND [Kunde]='{kunde}'";
				}
				var query = $"SELECT COUNT(*) FROM( {baseQuery} ) X";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var v) ? v : 0;
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.CTS.LagerBestandFGEntity> GetLagerBestandFG()
		{
			// - REM: replace Kosten gesamt 
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT v.Artikelnummer, v.Kunde,
                               v.[Bezeichnung 1], v.[Bezeichnung 2],
                               v.Freigabestatus, v.[CS Kontakt],
                               v.Lagerort, v.Bestand, v.VK
                               AS [VK Gesamt], v.Gesamtpreis_mit_cu AS [Kosten gesamt], v.Gesamtpreis_ohne_cu AS [Kosten gesamt ohne CU], v.Verkaufspreis AS VKE, a.UBG, IsNULL(a.EdiDefault,0) EdiDefault 
                               FROM (SELECT * FROM [View_Lagerbestände Analyse L_CS_wNegativ] WHERE Lagerort_id<>928 AND Lagerort_id<>825) v 
								LEFT JOIN [Artikel] a on a.Artikelnummer=v.Artikelnummer 
								ORDER BY v.Artikelnummer;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 300;
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.LagerBestandFGEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.LagerBestandFGEntity>();
			}
		}

		public static List<KeyValuePair<string, string>> GetBestandMitarbeiter()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT DISTINCT [CS Kontakt] FROM [View_PSZ_Artikel Kundenzuweisung2] WHERE [CS Kontakt] IS NOT NULL";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<string, string>(Convert.ToString(x["CS Kontakt"]), Convert.ToString(x["CS Kontakt"]))).ToList();
			}
			else
			{
				return new List<KeyValuePair<string, string>> { };
			}
		}

		public static List<KeyValuePair<string, string>> GetBestandKundenByContact(string conctat)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT DISTINCT [Kunde] FROM [View_PSZ_Artikel Kundenzuweisung2] WHERE [CS Kontakt] LIKE @conctat";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("conctat", "%" + conctat + "%");
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<string, string>(Convert.ToString(x["Kunde"]), Convert.ToString(x["Kunde"]))).ToList();
			}
			else
			{
				return new List<KeyValuePair<string, string>> { };
			}
		}

		public static List<KeyValuePair<string, string>> GetBestandLager(string conctat, string kunde)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT DISTINCT [Lagerort] FROM  [View_PSZ_FG Bestand nach CS mit Werten] WHERE [CS Kontakt]=@conctat AND [Kunde]=@kunde";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("conctat", conctat);
				sqlCommand.Parameters.AddWithValue("kunde", kunde);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<string, string>(Convert.ToString(x["Lagerort"]), Convert.ToString(x["Lagerort"]))).ToList();
			}
			else
			{
				return new List<KeyValuePair<string, string>> { };
			}
		}


		public static List<Infrastructure.Data.Entities.Joins.CTS.FA_NPEX_REP.Query1Entity> GetFA_NPEXQuery1(List<int> lagers, string kunde, List<string> articles)
		{
			if(lagers == null || lagers.Count <= 0)
			{
				return null;
			}
			var dataTable = new DataTable();
			var Complement = articles != null && articles.Count > 0 ? $"AND Artikel.Artikelnummer IN ({string.Join(",", articles.Select(a => $"'{a}'").ToList())})" : "";
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Artikel.Artikelnummer, Artikel.Freigabestatus, Fertigung.Fertigungsnummer, Artikel.[Bezeichnung 1],
                               Fertigung.Termin_Fertigstellung, Fertigung.Anzahl AS Anzahl, Fertigung.Bemerkung, Fertigung.Preis, Artikel.[Artikel-Nr],
                               [View_PSZ_Artikel Kundenzuweisung2].Kunde, Fertigung.Termin_Bestätigt1, Fertigung.Erstmuster 
                               FROM (Fertigung LEFT JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
                               LEFT JOIN [View_PSZ_Artikel Kundenzuweisung2] ON Artikel.Artikelnummer = [View_PSZ_Artikel Kundenzuweisung2].Artikelnummer
                               WHERE (((Artikel.Freigabestatus)='N' Or (Artikel.Freigabestatus)='P' 
                               Or (Artikel.Freigabestatus)='X' Or (Artikel.Freigabestatus)='E') AND ((Fertigung.Termin_Bestätigt1)<=DATEADD(DD,30,getdate())) 
                               AND ((Fertigung.Kennzeichen)='offen') AND ((Fertigung.gebucht)=1) 
                               AND ((Fertigung.Lagerort_id) IN ({string.Join(",", lagers)}))) and Kunde=@kunde {Complement}
                               ORDER BY Artikel.Artikelnummer, Fertigung.Termin_Fertigstellung;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("kunde", kunde);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.FA_NPEX_REP.Query1Entity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.FA_NPEX_REP.Query1Entity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.CTS.FA_NPEX_REP.Query1Entity> GetFA_NPEXQuery1(List<int> lagers, string kunde)
		{
			if(lagers == null || lagers.Count <= 0)
			{
				return null;
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Artikel.Artikelnummer, Artikel.Freigabestatus, Fertigung.Fertigungsnummer, Artikel.[Bezeichnung 1],
                               Fertigung.Termin_Fertigstellung, Fertigung.Anzahl AS Anzahl, Fertigung.Bemerkung, Fertigung.Preis, Artikel.[Artikel-Nr],
                               [View_PSZ_Artikel Kundenzuweisung2].Kunde, Fertigung.Termin_Bestätigt1, Fertigung.Erstmuster 
                               FROM (Fertigung LEFT JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
                               LEFT JOIN [View_PSZ_Artikel Kundenzuweisung2] ON Artikel.Artikelnummer = [View_PSZ_Artikel Kundenzuweisung2].Artikelnummer
                               WHERE (((Artikel.Freigabestatus)='N' Or (Artikel.Freigabestatus)='P' 
                               Or (Artikel.Freigabestatus)='X' Or (Artikel.Freigabestatus)='E') AND ((Fertigung.Termin_Bestätigt1)<=DATEADD(DD,30,getdate())) 
                               AND ((Fertigung.Kennzeichen)='offen') AND ((Fertigung.gebucht)=1) 
                               AND ((Fertigung.Lagerort_id) IN ({string.Join(",", lagers)}))) and Kunde=@kunde
                               ORDER BY Artikel.Artikelnummer, Fertigung.Termin_Fertigstellung;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("kunde", kunde);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.FA_NPEX_REP.Query1Entity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.FA_NPEX_REP.Query1Entity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.FA_NPEX_REP.Query1Entity> GetFA_NPEXQuery1(int lager, string kunde, int fa)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Artikel.Artikelnummer, Artikel.Freigabestatus, Fertigung.Fertigungsnummer, Artikel.[Bezeichnung 1],
                               Fertigung.Termin_Fertigstellung, Fertigung.Anzahl AS Anzahl, Fertigung.Bemerkung, Fertigung.Preis, Artikel.[Artikel-Nr],
                               [View_PSZ_Artikel Kundenzuweisung2].Kunde, Fertigung.Termin_Bestätigt1, Fertigung.Erstmuster 
                               FROM (Fertigung LEFT JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
                               LEFT JOIN [View_PSZ_Artikel Kundenzuweisung2] ON Artikel.Artikelnummer = [View_PSZ_Artikel Kundenzuweisung2].Artikelnummer
                               WHERE (((Artikel.Freigabestatus)='N' Or (Artikel.Freigabestatus)='P' 
                               Or (Artikel.Freigabestatus)='X' Or (Artikel.Freigabestatus)='E') AND ((Fertigung.Termin_Bestätigt1)<=DATEADD(DD,30,getdate())) 
                               AND ((Fertigung.Kennzeichen)='offen') AND ((Fertigung.gebucht)=1) 
                               AND ((Fertigung.Lagerort_id)=@lager)) AND Fertigungsnummer=@fa
                               ORDER BY Artikel.Artikelnummer, Fertigung.Termin_Fertigstellung;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("lager", lager);
				sqlCommand.Parameters.AddWithValue("kunde", kunde);
				sqlCommand.Parameters.AddWithValue("fa", fa);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.FA_NPEX_REP.Query1Entity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.FA_NPEX_REP.Query1Entity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.CTS.FA_NPEX_REP.Query2Entity> GetFA_NPEXQuery2(List<int> lagers, string kunde, List<string> articles)
		{
			if(lagers == null || lagers.Count <= 0)
			{
				return null;
			}
			var dataTable = new DataTable();
			var Complement = articles != null && articles.Count > 0 ? $"AND Artikel.Artikelnummer IN ({string.Join(",", articles.Select(a => $"'{a}'").ToList())})" : "";
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT X.Fertigungsnummer, X.Artikelnummer,
                               X.Anzahl, X.[Bezeichnung 1],
                               Stücklisten.[Artikel-Nr des Bauteils], Stücklisten.Artikelnummer AS [Artikelnummer ROH], Stücklisten.[Bezeichnung des Bauteils], Stücklisten.Anzahl AS [Anzahl ROH],
                               X.Anzahl*Stücklisten.Anzahl AS Bedarf, Lager.Bestand 
                               --INTO Y
                               FROM 
                               (
                               SELECT Artikel.Artikelnummer, Artikel.Freigabestatus, Fertigung.Fertigungsnummer, Artikel.[Bezeichnung 1],
                               Fertigung.Termin_Fertigstellung, Fertigung.Anzahl AS Anzahl, Fertigung.Bemerkung, Fertigung.Preis, Artikel.[Artikel-Nr],
                               [View_PSZ_Artikel Kundenzuweisung2].Kunde, Fertigung.Termin_Bestätigt1, Fertigung.Erstmuster 
                               FROM (Fertigung LEFT JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
                               LEFT JOIN [View_PSZ_Artikel Kundenzuweisung2] ON Artikel.Artikelnummer = [View_PSZ_Artikel Kundenzuweisung2].Artikelnummer
                               WHERE (((Artikel.Freigabestatus)='N' Or (Artikel.Freigabestatus)='P' 
                               Or (Artikel.Freigabestatus)='X' Or (Artikel.Freigabestatus)='E') AND ((Fertigung.Termin_Bestätigt1)<=DATEADD(DD,30,getdate())) 
                               AND ((Fertigung.Kennzeichen)='offen') AND ((Fertigung.gebucht)=1) 
                               AND ((Fertigung.Lagerort_id) IN ({string.Join(",", lagers)}))) AND Kunde=@kunde {Complement}
                               ) X 
                               INNER JOIN 
                               (Stücklisten INNER JOIN Lager ON Stücklisten.[Artikel-Nr des Bauteils] = Lager.[Artikel-Nr]) 
                               ON X.[Artikel-Nr] = Stücklisten.[Artikel-Nr]
                               GROUP BY X.Fertigungsnummer, 
                               X.Artikelnummer, X.Anzahl,
                               X.[Bezeichnung 1], Stücklisten.[Artikel-Nr des Bauteils],
                               Stücklisten.Artikelnummer, Stücklisten.[Bezeichnung des Bauteils], Stücklisten.Anzahl,
                               X.Anzahl*Stücklisten.Anzahl, Lager.Bestand, Lager.Lagerort_id
                               HAVING (((Lager.Lagerort_id) IN ({string.Join(",", lagers)})))
                               ORDER BY X.Artikelnummer,
                               X.Anzahl";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("kunde", kunde);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.FA_NPEX_REP.Query2Entity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.FA_NPEX_REP.Query2Entity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.FA_NPEX_REP.Query2Entity> GetFA_NPEXQuery2(List<int> lagers, string kunde)
		{
			if(lagers == null || lagers.Count <= 0)
			{
				return null;
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT X.Fertigungsnummer, X.Artikelnummer,
                               X.Anzahl, X.[Bezeichnung 1],
                               Stücklisten.[Artikel-Nr des Bauteils], Stücklisten.Artikelnummer AS [Artikelnummer ROH], Stücklisten.[Bezeichnung des Bauteils], Stücklisten.Anzahl AS [Anzahl ROH],
                               X.Anzahl*Stücklisten.Anzahl AS Bedarf, Lager.Bestand 
                               --INTO Y
                               FROM 
                               (
                               SELECT Artikel.Artikelnummer, Artikel.Freigabestatus, Fertigung.Fertigungsnummer, Artikel.[Bezeichnung 1],
                               Fertigung.Termin_Fertigstellung, Fertigung.Anzahl AS Anzahl, Fertigung.Bemerkung, Fertigung.Preis, Artikel.[Artikel-Nr],
                               [View_PSZ_Artikel Kundenzuweisung2].Kunde, Fertigung.Termin_Bestätigt1, Fertigung.Erstmuster 
                               FROM (Fertigung LEFT JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
                               LEFT JOIN [View_PSZ_Artikel Kundenzuweisung2] ON Artikel.Artikelnummer = [View_PSZ_Artikel Kundenzuweisung2].Artikelnummer
                               WHERE (((Artikel.Freigabestatus)='N' Or (Artikel.Freigabestatus)='P' 
                               Or (Artikel.Freigabestatus)='X' Or (Artikel.Freigabestatus)='E') AND ((Fertigung.Termin_Bestätigt1)<=DATEADD(DD,30,getdate())) 
                               AND ((Fertigung.Kennzeichen)='offen') AND ((Fertigung.gebucht)=1) 
                               AND ((Fertigung.Lagerort_id) IN ({string.Join(",", lagers)}))) AND Kunde=@kunde
                               ) X 
                               INNER JOIN 
                               (Stücklisten INNER JOIN Lager ON Stücklisten.[Artikel-Nr des Bauteils] = Lager.[Artikel-Nr]) 
                               ON X.[Artikel-Nr] = Stücklisten.[Artikel-Nr]
                               GROUP BY X.Fertigungsnummer, 
                               X.Artikelnummer, X.Anzahl,
                               X.[Bezeichnung 1], Stücklisten.[Artikel-Nr des Bauteils],
                               Stücklisten.Artikelnummer, Stücklisten.[Bezeichnung des Bauteils], Stücklisten.Anzahl,
                               X.Anzahl*Stücklisten.Anzahl, Lager.Bestand, Lager.Lagerort_id
                               HAVING (((Lager.Lagerort_id) IN ({string.Join(",", lagers)})))
                               ORDER BY X.Artikelnummer,
                               X.Anzahl";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("kunde", kunde);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.FA_NPEX_REP.Query2Entity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.FA_NPEX_REP.Query2Entity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.FA_NPEX_REP.Query2Entity> GetFA_NPEXQuery2(int lager, string kunde, int fa)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT X.Fertigungsnummer, X.Artikelnummer,
                               X.Anzahl, X.[Bezeichnung 1],
                               Stücklisten.[Artikel-Nr des Bauteils], Stücklisten.Artikelnummer AS [Artikelnummer ROH], Stücklisten.[Bezeichnung des Bauteils], Stücklisten.Anzahl AS [Anzahl ROH],
                               X.Anzahl*Stücklisten.Anzahl AS Bedarf, Lager.Bestand 
                               --INTO Y
                               FROM 
                               (
                               SELECT Artikel.Artikelnummer, Artikel.Freigabestatus, Fertigung.Fertigungsnummer, Artikel.[Bezeichnung 1],
                               Fertigung.Termin_Fertigstellung, Fertigung.Anzahl AS Anzahl, Fertigung.Bemerkung, Fertigung.Preis, Artikel.[Artikel-Nr],
                               [View_PSZ_Artikel Kundenzuweisung2].Kunde, Fertigung.Termin_Bestätigt1, Fertigung.Erstmuster 
                               FROM (Fertigung LEFT JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
                               LEFT JOIN [View_PSZ_Artikel Kundenzuweisung2] ON Artikel.Artikelnummer = [View_PSZ_Artikel Kundenzuweisung2].Artikelnummer
                               WHERE (((Artikel.Freigabestatus)='N' Or (Artikel.Freigabestatus)='P' 
                               Or (Artikel.Freigabestatus)='X' Or (Artikel.Freigabestatus)='E') AND ((Fertigung.Termin_Bestätigt1)<=DATEADD(DD,30,getdate())) 
                               AND ((Fertigung.Kennzeichen)='offen') AND ((Fertigung.gebucht)=1) 
                               AND ((Fertigung.Lagerort_id)=@lager)) AND Fertigungsnummer=@fa
                               ) X 
                               INNER JOIN 
                               (Stücklisten INNER JOIN Lager ON Stücklisten.[Artikel-Nr des Bauteils] = Lager.[Artikel-Nr]) 
                               ON X.[Artikel-Nr] = Stücklisten.[Artikel-Nr]
                               GROUP BY X.Fertigungsnummer, 
                               X.Artikelnummer, X.Anzahl,
                               X.[Bezeichnung 1], Stücklisten.[Artikel-Nr des Bauteils],
                               Stücklisten.Artikelnummer, Stücklisten.[Bezeichnung des Bauteils], Stücklisten.Anzahl,
                               X.Anzahl*Stücklisten.Anzahl, Lager.Bestand, Lager.Lagerort_id
                               HAVING (((Lager.Lagerort_id)=@lager))
                               ORDER BY X.Artikelnummer,
                               X.Anzahl";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("lager", lager);
				sqlCommand.Parameters.AddWithValue("kunde", kunde);
				sqlCommand.Parameters.AddWithValue("fa", fa);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.FA_NPEX_REP.Query2Entity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.FA_NPEX_REP.Query2Entity>();
			}
		}

		public static List<Tuple<string, string, decimal>> GetVersandBerechnet(DateTime from, DateTime to)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT Angebote.Typ, Artikel.Artikelnummer, Sum([angebotene Artikel].Gesamtpreis) AS SummevonGesamtpreis
                               FROM (Angebote INNER JOIN [angebotene Artikel] ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) 
                               INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]
                               WHERE (((Angebote.Datum)>=@from And (Angebote.Datum)<=@to))
                               GROUP BY Angebote.Typ, Artikel.Artikelnummer
                               HAVING (((Angebote.Typ)='Rechnung') AND ((Artikel.Artikelnummer)='Fracht'));	";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("from", from);
				sqlCommand.Parameters.AddWithValue("to", to);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Tuple<string, string, decimal>(Convert.ToString(x["Typ"]), Convert.ToString(x["Artikelnummer"]), Convert.ToDecimal(x["SummevonGesamtpreis"]))).ToList();
			}
			else
			{
				return new List<Tuple<string, string, decimal>> { };
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.LierferPlannungEntity> GetLieferPlannung(string kunde)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT X.Dokumentnummer, X.[Vorname/NameFirma],X.LName2, X.[LLand/PLZ/Ort] [LLand_PLZ_Ort],
X.Wunschtermin, X.Liefertermin,
X.[Angebot-Nr], X.Artikelnummer,
X.[Bezeichnung 1], X.[Menge Offen], X.[CSInterneBemerkung],
ISNULL(Lager.Bestand, 0) AS Bestand, X.Jahr, X.KW, X.Gesamtpreis
FROM
(
SELECT Angebote.Bezug as Dokumentnummer, Angebote.[Vorname/NameFirma], [angebotene Artikel].Wunschtermin,
[angebotene Artikel].Liefertermin, Angebote.[Angebot-Nr], Artikel.Artikelnummer,
Artikel.[Bezeichnung 1], DatePart(YYYY,[angebotene Artikel].Liefertermin) AS Jahr,
DatePart(isoww,[angebotene Artikel].Liefertermin) AS KW, [angebotene Artikel].Lagerort_id,
[angebotene Artikel].Anzahl AS [Menge Offen], [angebotene Artikel].OriginalAnzahl,
[angebotene Artikel].[Artikel-Nr], [angebotene Artikel].Gesamtpreis, [Angebotene Artikel].CSInterneBemerkung,Angebote.LName2, Angebote.[LLand/PLZ/Ort]
FROM Angebote INNER JOIN ([angebotene Artikel] 
INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]
WHERE (((Angebote.[Vorname/NameFirma]) Like @kunde) 
AND (([angebotene Artikel].Wunschtermin) Is Not Null) AND (([angebotene Artikel].Liefertermin) Is Not Null) 
AND (([angebotene Artikel].Anzahl)<>0) AND [angebotene Artikel].[Liefertermin]<=(dateadd(week, datediff(week, 0, getdate()), 0)+7*7)
AND ((Angebote.Typ)='Auftragsbestätigung') AND (([angebotene Artikel].erledigt_pos)=0) AND ((Angebote.erledigt)=0))
) X 
LEFT JOIN Lager ON (X.[Artikel-Nr] = Lager.[Artikel-Nr]) 
AND (X.Lagerort_id = Lager.Lagerort_id)
ORDER BY X.[Vorname/NameFirma], X.Jahr,X.KW;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("kunde", "%" + kunde + "%");
				sqlCommand.CommandTimeout = 330;
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.LierferPlannungEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.LierferPlannungEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.CTS.LieferPlannungBestandEntity> GetLieferPlannungBestand(string article)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Lager.Lagerort_id, Lager.Bestand, Artikel.Artikelnummer AS [psz#], Lagerorte.Lagerort
                                FROM (Artikel INNER JOIN Lager ON Artikel.[Artikel-Nr] = Lager.[Artikel-Nr]) 
                                INNER JOIN Lagerorte ON Lager.Lagerort_id = Lagerorte.Lagerort_id
                                WHERE (((Lager.Bestand)<>0)) AND Artikel.Artikelnummer=@article";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("article", article);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.LieferPlannungBestandEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.LieferPlannungBestandEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.LieferPlannungFertigungEntity> GetLieferPlannungFertigung(string article)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Artikel.Artikelnummer AS [psz#], Fertigung.Fertigungsnummer, Fertigung.FA_Gestartet,
                                Lagerorte.Lagerort, Fertigung.Kennzeichen, Fertigung.Anzahl AS [Menge Offen], Fertigung.Originalanzahl, Fertigung.Termin_Bestätigt1
                                FROM Artikel INNER JOIN (Lagerorte 
                                INNER JOIN Fertigung ON Lagerorte.Lagerort_id = Fertigung.Lagerort_id) 
                                ON Artikel.[Artikel-Nr] = Fertigung.Artikel_Nr
                                WHERE (((Fertigung.Kennzeichen)='Offen')) AND Artikel.Artikelnummer=@article";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("article", article);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.LieferPlannungFertigungEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.LieferPlannungFertigungEntity>();
			}
		}


		public static List<Infrastructure.Data.Entities.Joins.CTS.ErstelltRehugenEntity> GetErstelltRehugen(DateTime From, DateTime To)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Angebote.[Vorname/NameFirma], Artikel.Artikelnummer, Angebote.Datum,
                                Angebote.[Angebot-Nr] AS Rgnummer, Angebote.[Projekt-Nr] AS Angebotnummer,
                                [angebotene Artikel].VKGesamtpreis, [angebotene Artikel].Gesamtkupferzuschlag,
                                ([VKGesamtpreis]*1)+[Gesamtkupferzuschlag] AS NettoBetrag, Angebote.Bezug,[CS Kontakt]
                                FROM ((Angebote INNER JOIN [angebotene Artikel] ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) 
                                INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
                                INNER JOIN Kunden ON Angebote.[Kunden-Nr] = Kunden.nummer
								LEFT JOIN (SELECT DISTINCT Kundennummer,[CS Kontakt] FROM [PSZ_Nummerschlüssel Kunde] WHERE Kundennummer IN (SELECT Kundennummer FROM [PSZ_Nummerschlüssel Kunde] group by Kundennummer having  Count(Distinct [CS Kontakt])=1 )) N  ON N.Kundennummer=Angebote.[Unser Zeichen]
                                WHERE (((Angebote.Datum)>=@From And (Angebote.Datum)<=@To) AND ((Angebote.Typ)='Rechnung'));";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("From", From);
				sqlCommand.Parameters.AddWithValue("To", To);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.ErstelltRehugenEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.ErstelltRehugenEntity>();
			}
		}


		public static List<Infrastructure.Data.Entities.Joins.CTS.CSContactFAEntity> GetContactFA(DateTime From, DateTime To)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT [PSZ_Nummerschlüssel Kunde].[CS Kontakt], X.Kennzeichen,
                               X.Lagerort_id, 
                               X.Datum,
                               X.Fertigungsnummer,
                               X.Termin_Bestätigt1,
                               X.Termin_Bestätigt2,
                               X.Artikelnummer, 
                               [PSZ_Nummerschlüssel Kunde].Kunde
                               FROM
                               (
                               SELECT Fertigung.Artikel_Nr, Artikel.Artikelnummer, (LEFT([Artikelnummer], (CASE WHEN CHARINDEX('-',[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',[Artikelnummer],0)-1 END))) AS Art3,
                               Fertigung.Termin_Bestätigt1,Fertigung.Termin_Bestätigt2,Fertigung.Lagerort_id,
                               Fertigung.Datum,Fertigung.Fertigungsnummer,Fertigung.Kennzeichen
                               FROM Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]
                               WHERE Fertigung.Kennzeichen<>'Storno' AND Fertigung.Datum>=@From AND Fertigung.Datum<=@To
                               ) X
                               INNER JOIN [PSZ_Nummerschlüssel Kunde] ON X.Art3=[PSZ_Nummerschlüssel Kunde].Nummerschlüssel";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("From", From);
				sqlCommand.Parameters.AddWithValue("To", To);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.CSContactFAEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.CSContactFAEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.CTS.CSContactFAExcelEntity> GetContactFAExcel(DateTime From, DateTime To)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT 
                                X.Datum,
X.Fertigungsnummer,
X.Artikelnummer,
X.[Bezeichnung 1],
X.[Originalanzahl],
X.[Anzahl_erledigt],
X.Anzahl_aktuell,
X.Kennzeichen,
X.Lagerort_id, 
X.Termin_Bestätigt1,
X.Termin_Bestätigt2,
X.Bemerkung,
[PSZ_Nummerschlüssel Kunde].Kunde,
[PSZ_Nummerschlüssel Kunde].[CS Kontakt]
FROM
(
SELECT Fertigung.Artikel_Nr, Artikel.Artikelnummer, (LEFT([Artikelnummer], (CASE WHEN CHARINDEX('-',[Artikelnummer],0)<=0 THEN 0 ELSE CHARINDEX('-',[Artikelnummer],0)-1 END))) AS Art3,Artikel.[Bezeichnung 1],
Fertigung.Termin_Bestätigt1,Fertigung.Termin_Bestätigt2,Fertigung.Lagerort_id,
Fertigung.Datum,Fertigung.Fertigungsnummer,Fertigung.Kennzeichen,
Fertigung.Originalanzahl,Fertigung.Anzahl_erledigt,Fertigung.Anzahl_aktuell,
Fertigung.Bemerkung
FROM Fertigung INNER JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]
WHERE Fertigung.Kennzeichen<>'Storno' AND Fertigung.Datum>=@From AND Fertigung.Datum<=@To
) X
INNER JOIN [PSZ_Nummerschlüssel Kunde] ON X.Art3=[PSZ_Nummerschlüssel Kunde].Nummerschlüssel
ORDER BY [PSZ_Nummerschlüssel Kunde].[CS Kontakt]";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("From", From);
				sqlCommand.Parameters.AddWithValue("To", To);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.CSContactFAExcelEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.CSContactFAExcelEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.CTS.AuswertungSchneidereiEntity> GetAuswertungSchneiderei(string table)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT * FROM {table}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.AuswertungSchneidereiEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.AuswertungSchneidereiEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.AuswertungSchneidereiEntity> GetAuswertungSchneidereiAll()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select * from Auswertung_Albanien_Schneiderei
union 
select * from Auswertung_Tunesien_Schneiderei
union 
select * from Auswertung_KHTN_Schneiderei
union 
select * from Auswertung_Deutschland_Schneiderei
union 
select * from Auswertung_Eigenfertigung_Schneiderei
union 
select * from Auswertung_Schneiderei_GZTN";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.AuswertungSchneidereiEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.AuswertungSchneidereiEntity>();
			}
		}


		public static List<string> GetKundeStufe()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT * FROM [Kunden_Stufe]";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => Convert.ToString(x["Kunden_Stufe"])).ToList();
			}
			else
			{
				return new List<string> { };
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.CTS.RechnungEntity> GetRechnung(DateTime From, DateTime To, KeyValuePair<int, int> lager)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT '' AS Ausdr1, Fertigung_Fertigungsvorgang.Anzahl*[Preis] AS Ausdr3,'' AS Ausdr2,''AS Ausdr4,Fertigung_Fertigungsvorgang.Datum as Datum,
								Fertigung.Fertigungsnummer as Fertigungsnummer, Fertigung.Originalanzahl, Fertigung.Anzahl_erledigt, Fertigung_Fertigungsvorgang.Anzahl,
								Artikel.Artikelnummer, Artikel.[Bezeichnung 1] as Bezeichnung1, artikel_kalkulatorische_kosten.Betrag, Fertigung.Preis,
								isnull(Fertigung.Bemerkung,'') as Bemerkung,
								IIf([Bezeichnung 1]='Reparatur',[Bemerkung],IIf([Bezeichnung 1]='Technische Dienstleistung',[Bemerkung],
								IIf([Bezeichnung 1]='Kabelsätze',[Bemerkung],[Bezeichnung 1]))) AS Bezfeld,
								IIF( Fertigung.Erstmuster=0,0,1)  as Erstmuster, ISNULL(Artikel.Zolltarif_nr,0) as Zolltarif_nr,
								ISNULL(Fertigung_Fertigungsvorgang.Anzahl*[Materialkosten],0) AS Material1,
								Artikel.Größe, Fertigung_Fertigungsvorgang.Anzahl*Artikel.Größe AS Gesamtgewicht,
								',' AS Ausdr5, Artikel.Stundensatz, [Stundensatz]/60 AS MinutenKosten,
								IIF([Originalanzahl]<=5,([Stundensatz]/60)*30,IIf([Originalanzahl]>5 and [Originalanzahl]<=10 ,([Stundensatz]/60)*20,
								IIF([Originalanzahl]>10 and [Originalanzahl]<=15 ,([Stundensatz]/60)*10,0))) AS [Zusatzkosten_FA(Basis 30 Min)],
								/* IIF([Originalanzahl]<=5,(([Stundensatz]/60)*30)/Fertigung.Originalanzahl+([Preis]),
								IIF([Originalanzahl]<=10 and Originalanzahl>5,(([Stundensatz]/60)*20)/Fertigung.Originalanzahl+([Preis]),
								IIF([Originalanzahl]<=15 and originalanzahl>10,(([Stundensatz]/60)*10)/Fertigung.Originalanzahl+([Preis]),[Preis])))*/ 0 AS PREIS_Mit_Zusatzkosten_Pro_Stück,
								/* IIF([Originalanzahl]<=5,(([Stundensatz]/60)*30)/Fertigung.Originalanzahl*Fertigung_Fertigungsvorgang.Anzahl,
								IIF([Originalanzahl]<=10 and originalanzahl>5,(([Stundensatz]/60)*20)/Fertigung.Originalanzahl*Fertigung_Fertigungsvorgang.Anzahl,
								IIF([Originalanzahl]<=15 and originalanzahl>10,(([Stundensatz]/60)*10)/Fertigung.Originalanzahl*Fertigung_Fertigungsvorgang.Anzahl,0)))*/ 0 
								as Zusatzkosten_Produktion,
								/* IIF([OriginalAnzahl] <= 5, (([Stundensatz] / 60) * 30) / Fertigung.OriginalAnzahl * Fertigung_Fertigungsvorgang.Anzahl + (Fertigung_Fertigungsvorgang.Anzahl 
								* [Preis]), IIF([OriginalAnzahl] <= 10 And OriginalAnzahl > 5, 
								(([Stundensatz] / 60) * 20) / Fertigung.OriginalAnzahl * Fertigung_Fertigungsvorgang.Anzahl + (Fertigung_Fertigungsvorgang.Anzahl * [Preis]), 
								IIF([OriginalAnzahl] <= 15 And OriginalAnzahl > 10, (([Stundensatz] / 60) * 10) / Fertigung.OriginalAnzahl * Fertigung_Fertigungsvorgang.Anzahl 
								+ (Fertigung_Fertigungsvorgang.Anzahl * [Preis]), Fertigung_Fertigungsvorgang.Anzahl * [Preis])))*/ 0 As Preis_Total_Mit_Zusatzkosten
								FROM (((Fertigung_Fertigungsvorgang LEFT JOIN Fertigung ON Fertigung_Fertigungsvorgang.Fertigung_Nr = Fertigung.ID) 
								LEFT JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
								LEFT JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]) 
								LEFT JOIN [View_PSZ_Rechnungsabfrage Eigenfertigung Material 02_ROH] 
								ON Artikel.Artikelnummer = [View_PSZ_Rechnungsabfrage Eigenfertigung Material 02_ROH].Artikelnummer
								WHERE ((CAST(Fertigung_Fertigungsvorgang.Datum AS DATE)>=@From And CAST(Fertigung_Fertigungsvorgang.Datum AS DATE)<=@To) 
								AND ((Fertigung.Preis)<>0) AND ((Fertigung.Lagerort_id)=@lager1 Or (Fertigung.Lagerort_id)=@lager2) 
								AND ((Fertigung_Fertigungsvorgang.ab_buchen)=0))
								ORDER BY Fertigung_Fertigungsvorgang.Datum, Fertigung.Fertigungsnummer;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("From", From);
				sqlCommand.Parameters.AddWithValue("To", To);
				sqlCommand.Parameters.AddWithValue("lager1", lager.Key);
				sqlCommand.Parameters.AddWithValue("lager2", lager.Value);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.RechnungEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.RechnungEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.RechnungROHEntity> GetRechnungROH_1(DateTime From, DateTime To, int lager)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Lagerbewegungen.Typ, Lagerbewegungen.Datum, Artikel.Artikelnummer, Artikel.Zolltarif_nr,
Lagerbewegungen_Artikel.Lager_nach AS Lagerplatz, Lagerbewegungen_Artikel.Anzahl_nach AS Eingangsmenge,
Lagerbewegungen_Artikel.Anzahl_nach*Artikel.Größe/1000 AS Gewicht, Artikel.Größe, Bestellnummern.Standardlieferant,
Bestellnummern.Einkaufspreis, Lagerbewegungen_Artikel.Anzahl_nach*Bestellnummern.Einkaufspreis AS Wert
--INTO [PSZ_Intrastat Rohmaterial Hilfstabelle]
FROM ((Lagerbewegungen INNER JOIN Lagerbewegungen_Artikel ON Lagerbewegungen.ID = Lagerbewegungen_Artikel.Lagerbewegungen_id) 
INNER JOIN Artikel ON Lagerbewegungen_Artikel.[Artikel-nr] = Artikel.[Artikel-Nr]) 
INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]
WHERE (((Lagerbewegungen.Typ)='Umbuchung' Or (Lagerbewegungen.Typ)='Zugang direkt') AND (CAST(Lagerbewegungen.Datum AS DATE)>=@From 
And CAST(Lagerbewegungen.Datum AS DATE)<=@To) AND ((Artikel.Zolltarif_nr)<>'') AND ((Lagerbewegungen_Artikel.Lager_nach)=@Lager) 
AND ((Bestellnummern.Standardlieferant)=1) AND ((Lagerbewegungen.gebucht)=1) AND ((Artikel.Stückliste)=0))";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("From", From);
				sqlCommand.Parameters.AddWithValue("To", To);
				sqlCommand.Parameters.AddWithValue("Lager", lager);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.RechnungROHEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.RechnungROHEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.RechnungROHEntity> GetRechnungROH_2(DateTime From, DateTime To, int lager)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Lagerbewegungen.Typ, Lagerbewegungen.Datum, Artikel.Artikelnummer, Artikel.Zolltarif_nr,
Lagerbewegungen_Artikel.Lager_von AS Lagerplatz, Lagerbewegungen_Artikel.Anzahl AS Eingangsmenge,
Lagerbewegungen_Artikel.Anzahl*Artikel.Größe/1000 AS Gewicht,
Artikel.Größe, Bestellnummern.Standardlieferant, Bestellnummern.Einkaufspreis,
Lagerbewegungen_Artikel.Anzahl*Bestellnummern.Einkaufspreis AS Wert
FROM ((Lagerbewegungen INNER JOIN Lagerbewegungen_Artikel ON Lagerbewegungen.ID = Lagerbewegungen_Artikel.Lagerbewegungen_id) 
INNER JOIN Artikel ON Lagerbewegungen_Artikel.[Artikel-nr] = Artikel.[Artikel-Nr]) 
INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]
WHERE (((Lagerbewegungen.Typ)='Zugang direkt') AND (CAST(Lagerbewegungen.Datum AS DATE)>=@From And CAST(Lagerbewegungen.Datum AS DATE)<=@To) 
AND ((Artikel.Zolltarif_nr)<>'') AND ((Lagerbewegungen_Artikel.Lager_von)=@Lager) AND ((Bestellnummern.Standardlieferant)=1) 
AND ((Lagerbewegungen.gebucht)=1) AND ((Artikel.Stückliste)=0))";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("From", From);
				sqlCommand.Parameters.AddWithValue("To", To);
				sqlCommand.Parameters.AddWithValue("Lager", lager);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.RechnungROHEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.RechnungROHEntity>();
			}
		}


		public static List<Infrastructure.Data.Entities.Joins.CTS.RechnungROHTNEntity> GetRechnungROHTN_1(DateTime From, DateTime To, KeyValuePair<int, int> lager)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Artikel.Artikelnummer,
Artikel.[Bezeichnung 1], Fertigung_Fertigungsvorgang.Anzahl*(-1) AS Menge FROM (
SELECT '' AS Ausdr1, Fertigung_Fertigungsvorgang.Anzahl*[Preis] AS Ausdr3,'' AS Ausdr2,''AS Ausdr4,Fertigung_Fertigungsvorgang.Datum as Datum,
Fertigung.Fertigungsnummer as Fertigungsnummer, Fertigung.Originalanzahl, Fertigung.Anzahl_erledigt, Fertigung_Fertigungsvorgang.Anzahl,
Artikel.Artikelnummer, Artikel.[Bezeichnung 1] as Bezeichnung1, artikel_kalkulatorische_kosten.Betrag, Fertigung.Preis,
isnull(Fertigung.Bemerkung,'') as Bemerkung,
IIf([Bezeichnung 1]='Reparatur',[Bemerkung],IIf([Bezeichnung 1]='Technische Dienstleistung',[Bemerkung],
IIf([Bezeichnung 1]='Kabelsätze',[Bemerkung],[Bezeichnung 1]))) AS Bezfeld,
IIF( Fertigung.Erstmuster=0,0,1)  as Erstmuster, ISNULL(Artikel.Zolltarif_nr,0) as Zolltarif_nr,
ISNULL(Fertigung_Fertigungsvorgang.Anzahl*[Materialkosten],0) AS Material1,
Artikel.Größe, Fertigung_Fertigungsvorgang.Anzahl*Artikel.Größe AS Gesamtgewicht,
',' AS Ausdr5, Artikel.Stundensatz, [Stundensatz]/60 AS MinutenKosten,
IIF([Originalanzahl]<=5,([Stundensatz]/60)*30,IIf([Originalanzahl]>5 and [Originalanzahl]<=10 ,([Stundensatz]/60)*20,
IIF([Originalanzahl]>10 and [Originalanzahl]<=15 ,([Stundensatz]/60)*10,0))) AS [Zusatzkosten_FA(Basis 30 Min)],
IIF([Originalanzahl]<=5,(([Stundensatz]/60)*30)/Fertigung.Originalanzahl+([Preis]),
IIF([Originalanzahl]<=10 and Originalanzahl>5,(([Stundensatz]/60)*20)/Fertigung.Originalanzahl+([Preis]),
IIF([Originalanzahl]<=15 and originalanzahl>10,(([Stundensatz]/60)*10)/Fertigung.Originalanzahl+([Preis]),[Preis]))) AS PREIS_Mit_Zusatzkosten_Pro_Stück,
IIF([Originalanzahl]<=5,(([Stundensatz]/60)*30)/Fertigung.Originalanzahl*Fertigung_Fertigungsvorgang.Anzahl,
IIF([Originalanzahl]<=10 and originalanzahl>5,(([Stundensatz]/60)*20)/Fertigung.Originalanzahl*Fertigung_Fertigungsvorgang.Anzahl,
IIF([Originalanzahl]<=15 and originalanzahl>10,(([Stundensatz]/60)*10)/Fertigung.Originalanzahl*Fertigung_Fertigungsvorgang.Anzahl,0))) 
as Zusatzkosten_Produktion,
IIF([OriginalAnzahl] <= 5, (([Stundensatz] / 60) * 30) / Fertigung.OriginalAnzahl * Fertigung_Fertigungsvorgang.Anzahl + (Fertigung_Fertigungsvorgang.Anzahl 
* [Preis]), IIF([OriginalAnzahl] <= 10 And OriginalAnzahl > 5, 
(([Stundensatz] / 60) * 20) / Fertigung.OriginalAnzahl * Fertigung_Fertigungsvorgang.Anzahl + (Fertigung_Fertigungsvorgang.Anzahl * [Preis]), 
IIF([OriginalAnzahl] <= 15 And OriginalAnzahl > 10, (([Stundensatz] / 60) * 10) / Fertigung.OriginalAnzahl * Fertigung_Fertigungsvorgang.Anzahl 
+ (Fertigung_Fertigungsvorgang.Anzahl * [Preis]), Fertigung_Fertigungsvorgang.Anzahl * [Preis]))) As Preis_Total_Mit_Zusatzkosten
FROM (((Fertigung_Fertigungsvorgang LEFT JOIN Fertigung ON Fertigung_Fertigungsvorgang.Fertigung_Nr = Fertigung.ID) 
LEFT JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
LEFT JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]) 
LEFT JOIN [View_PSZ_Rechnungsabfrage Eigenfertigung Material 02_ROH] 
ON Artikel.Artikelnummer = [View_PSZ_Rechnungsabfrage Eigenfertigung Material 02_ROH].Artikelnummer
WHERE ((CAST(Fertigung_Fertigungsvorgang.Datum AS DATE)>=@From And CAST(Fertigung_Fertigungsvorgang.Datum AS DATE)<=@To) 
AND ((Fertigung.Preis)<>0) AND ((Fertigung.Lagerort_id)=@lager2 Or (Fertigung.Lagerort_id)=@lager1) 
AND ((Fertigung_Fertigungsvorgang.ab_buchen)=0))
) X
INNER JOIN (Fertigung INNER JOIN (Fertigung_Fertigungsvorgang 
INNER JOIN Artikel ON Fertigung_Fertigungsvorgang.Artikel_nr = Artikel.[Artikel-Nr]) 
ON Fertigung.ID = Fertigung_Fertigungsvorgang.Fertigung_Nr) 
ON X.Fertigungsnummer = Fertigung.Fertigungsnummer
WHERE (((Fertigung_Fertigungsvorgang.ab_buchen)=1))
order by Artikel.Artikelnummer";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("From", From);
				sqlCommand.Parameters.AddWithValue("To", To);
				sqlCommand.Parameters.AddWithValue("lager2", lager.Value);
				sqlCommand.Parameters.AddWithValue("lager1", lager.Key);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.RechnungROHTNEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.RechnungROHTNEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.CTS.RechnungROHTNEntity> GetRechnungROHTN_2(DateTime From, DateTime To, KeyValuePair<int, int> lager)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Artikel.Artikelnummer,Lagerbewegungen_Artikel.[Bezeichnung 1], Lagerbewegungen_Artikel.Anzahl AS Menge
FROM
(
SELECT '' AS Ausdr1, Fertigung_Fertigungsvorgang.Anzahl*[Preis] AS Ausdr3,'' AS Ausdr2,''AS Ausdr4,Fertigung_Fertigungsvorgang.Datum as Datum,
Fertigung.Fertigungsnummer as Fertigungsnummer, Fertigung.Originalanzahl, Fertigung.Anzahl_erledigt, Fertigung_Fertigungsvorgang.Anzahl,
Artikel.Artikelnummer, Artikel.[Bezeichnung 1] as Bezeichnung1, artikel_kalkulatorische_kosten.Betrag, Fertigung.Preis,
isnull(Fertigung.Bemerkung,'') as Bemerkung,
IIf([Bezeichnung 1]='Reparatur',[Bemerkung],IIf([Bezeichnung 1]='Technische Dienstleistung',[Bemerkung],
IIf([Bezeichnung 1]='Kabelsätze',[Bemerkung],[Bezeichnung 1]))) AS Bezfeld,
IIF( Fertigung.Erstmuster=0,0,1)  as Erstmuster, ISNULL(Artikel.Zolltarif_nr,0) as Zolltarif_nr,
ISNULL(Fertigung_Fertigungsvorgang.Anzahl*[Materialkosten],0) AS Material1,
Artikel.Größe, Fertigung_Fertigungsvorgang.Anzahl*Artikel.Größe AS Gesamtgewicht,
',' AS Ausdr5, Artikel.Stundensatz, [Stundensatz]/60 AS MinutenKosten,
IIF([Originalanzahl]<=5,([Stundensatz]/60)*30,IIf([Originalanzahl]>5 and [Originalanzahl]<=10 ,([Stundensatz]/60)*20,
IIF([Originalanzahl]>10 and [Originalanzahl]<=15 ,([Stundensatz]/60)*10,0))) AS [Zusatzkosten_FA(Basis 30 Min)],
IIF([Originalanzahl]<=5,(([Stundensatz]/60)*30)/Fertigung.Originalanzahl+([Preis]),
IIF([Originalanzahl]<=10 and Originalanzahl>5,(([Stundensatz]/60)*20)/Fertigung.Originalanzahl+([Preis]),
IIF([Originalanzahl]<=15 and originalanzahl>10,(([Stundensatz]/60)*10)/Fertigung.Originalanzahl+([Preis]),[Preis]))) AS PREIS_Mit_Zusatzkosten_Pro_Stück,
IIF([Originalanzahl]<=5,(([Stundensatz]/60)*30)/Fertigung.Originalanzahl*Fertigung_Fertigungsvorgang.Anzahl,
IIF([Originalanzahl]<=10 and originalanzahl>5,(([Stundensatz]/60)*20)/Fertigung.Originalanzahl*Fertigung_Fertigungsvorgang.Anzahl,
IIF([Originalanzahl]<=15 and originalanzahl>10,(([Stundensatz]/60)*10)/Fertigung.Originalanzahl*Fertigung_Fertigungsvorgang.Anzahl,0))) 
as Zusatzkosten_Produktion,
IIF([OriginalAnzahl] <= 5, (([Stundensatz] / 60) * 30) / Fertigung.OriginalAnzahl * Fertigung_Fertigungsvorgang.Anzahl + (Fertigung_Fertigungsvorgang.Anzahl 
* [Preis]), IIF([OriginalAnzahl] <= 10 And OriginalAnzahl > 5, 
(([Stundensatz] / 60) * 20) / Fertigung.OriginalAnzahl * Fertigung_Fertigungsvorgang.Anzahl + (Fertigung_Fertigungsvorgang.Anzahl * [Preis]), 
IIF([OriginalAnzahl] <= 15 And OriginalAnzahl > 10, (([Stundensatz] / 60) * 10) / Fertigung.OriginalAnzahl * Fertigung_Fertigungsvorgang.Anzahl 
+ (Fertigung_Fertigungsvorgang.Anzahl * [Preis]), Fertigung_Fertigungsvorgang.Anzahl * [Preis]))) As Preis_Total_Mit_Zusatzkosten
FROM (((Fertigung_Fertigungsvorgang LEFT JOIN Fertigung ON Fertigung_Fertigungsvorgang.Fertigung_Nr = Fertigung.ID) 
LEFT JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
LEFT JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]) 
LEFT JOIN [View_PSZ_Rechnungsabfrage Eigenfertigung Material 02_ROH] 
ON Artikel.Artikelnummer = [View_PSZ_Rechnungsabfrage Eigenfertigung Material 02_ROH].Artikelnummer
WHERE ((CAST(Fertigung_Fertigungsvorgang.Datum AS DATE)>=@From And CAST(Fertigung_Fertigungsvorgang.Datum AS DATE)<=@To) 
AND ((Fertigung.Preis)<>0) AND ((Fertigung.Lagerort_id)=@lager2 Or (Fertigung.Lagerort_id)=@lager1) 
AND ((Fertigung_Fertigungsvorgang.ab_buchen)=0))
) X, (Lagerbewegungen INNER JOIN Lagerbewegungen_Artikel
ON Lagerbewegungen.ID = Lagerbewegungen_Artikel.Lagerbewegungen_id) INNER JOIN Artikel 
ON Lagerbewegungen_Artikel.[Artikel-nr] = Artikel.[Artikel-Nr]
WHERE ((CAST(Lagerbewegungen.Datum AS DATE)>=@From 
And CAST(Lagerbewegungen.Datum AS DATE)<=@To))
GROUP BY Lagerbewegungen_Artikel.Anzahl, Artikel.Artikelnummer, Lagerbewegungen_Artikel.[Bezeichnung 1],
Lagerbewegungen.Datum, Lagerbewegungen_Artikel.ID,
Lagerbewegungen.Typ, Lagerbewegungen_Artikel.Lager_von, Lagerbewegungen_Artikel.Lager_nach
HAVING (((Lagerbewegungen.Typ)='Entnahme') AND ((Lagerbewegungen_Artikel.Lager_von)=@lager1));";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("From", From);
				sqlCommand.Parameters.AddWithValue("To", To);
				sqlCommand.Parameters.AddWithValue("lager2", lager.Value);
				sqlCommand.Parameters.AddWithValue("lager1", lager.Key);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.RechnungROHTNEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.RechnungROHTNEntity>();
			}
		}


		public static List<Infrastructure.Data.Entities.Joins.CTS.RahmenABEntity> GetAbLinkBlanket()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"Select X.* from (select top 10 [Nr],[Projekt-Nr],[Angebot-Nr],[Vorname/NameFirma],[Fälligkeit],[Bezug],[Datum],[nr_RA]  
                              from [Angebote] Where [Typ]='Auftragsbestätigung' AND nr_RA IN( select Nr from [Angebote] Where [Typ]='Rahmenauftrag')) X";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.RahmenABEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.RahmenABEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.CTS.RechnungEndkontrolleEntity> GetRechnungEndkontrlle(DateTime From, DateTime To)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"SELECT Fertigung_Fertigungsvorgang.Datum, Fertigung.Fertigungsnummer, Fertigung.Originalanzahl,
                               Fertigung.Anzahl_erledigt, Fertigung_Fertigungsvorgang.Anzahl, Artikel.Artikelnummer,
                               Artikel.[Bezeichnung 1], artikel_kalkulatorische_kosten.Betrag,
                               Fertigung_Fertigungsvorgang.Anzahl*[Preis] AS Ausdr3,
                               Fertigung.Preis, Fertigung.Bemerkung, IIf([Bezeichnung 1]='Reparatur',[Bemerkung],[Bezeichnung 1]) AS Bezfeld, Fertigung.Erstmuster
                               FROM ((Fertigung_Fertigungsvorgang LEFT JOIN Fertigung ON Fertigung_Fertigungsvorgang.Fertigung_Nr = Fertigung.ID) 
                               LEFT JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
                               LEFT JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]
                               WHERE ((CAST(Fertigung_Fertigungsvorgang.Datum AS DATE)>=@From And CAST(Fertigung_Fertigungsvorgang.Datum AS DATE)<=@To) 
                               AND ((Artikel.Artikelnummer)='Endkontrolle') AND ((Fertigung.Lagerort_id)=3 Or (Fertigung.Lagerort_id)=6) 
                               AND ((Fertigung_Fertigungsvorgang.ab_buchen)=0))
                               ORDER BY Fertigung_Fertigungsvorgang.Datum, Fertigung.Fertigungsnummer;	";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("From", From);
				sqlCommand.Parameters.AddWithValue("To", To);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.RechnungEndkontrolleEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.RechnungEndkontrolleEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.CTS.NachBerechnungTNEntity> GetNachBerechnungTN(DateTime From, DateTime To)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"SELECT artikel_kalkulatorische_kosten.Kostenart, Fertigung_Fertigungsvorgang.ab_buchen,
                               Fertigung_Fertigungsvorgang.Datum, Fertigung.Fertigungsnummer, Artikel.Artikelnummer, Fertigung.Originalanzahl,
                               Fertigung_Fertigungsvorgang.Anzahl, artikel_kalkulatorische_kosten.Betrag, Fertigung.Preis,
                               Fertigung_Fertigungsvorgang.Anzahl*Fertigung.Preis AS [Lohn alt],
                               artikel_kalkulatorische_kosten.Betrag*Fertigung_Fertigungsvorgang.Anzahl AS [Lohn neu], [Betrag]-[Preis] AS Ausdr5,
                               ([Betrag]-[Preis])*Fertigung_Fertigungsvorgang.Anzahl AS Ausdr3, 
                               IIf([Bezeichnung 1]='Reparatur',[Bemerkung],[Bezeichnung 1]) AS Bezfeld
                               FROM ((Artikel INNER JOIN Fertigung ON Artikel.[Artikel-Nr] = Fertigung.Artikel_Nr) 
                               INNER JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]) 
                               INNER JOIN Fertigung_Fertigungsvorgang ON Fertigung.ID = Fertigung_Fertigungsvorgang.Fertigung_Nr
                               WHERE (((Fertigung.Lagerort_id)=7) AND ((artikel_kalkulatorische_kosten.Kostenart)='Arbeitskosten') 
                               AND ((Fertigung_Fertigungsvorgang.ab_buchen)=0) AND (CAST(Fertigung_Fertigungsvorgang.Datum AS DATE)>=@From 
                               And CAST(Fertigung_Fertigungsvorgang.Datum AS DATE)<=@To) AND ((Artikel.Artikelnummer)<>'Reparatur' 
                               And (Artikel.Artikelnummer)<>'Endkontrolle') AND ((artikel_kalkulatorische_kosten.Betrag)>[Preis]));";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("From", From);
				sqlCommand.Parameters.AddWithValue("To", To);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.NachBerechnungTNEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.NachBerechnungTNEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.CTS.RgSpritzgussEntity> GetRGSpritzguss(DateTime From, DateTime To)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"SELECT Fertigung_Fertigungsvorgang.Datum, Fertigung.Fertigungsnummer, Fertigung.Originalanzahl,
                               Fertigung.Anzahl_erledigt, Fertigung_Fertigungsvorgang.Anzahl, Artikel.Artikelnummer, 
                               Artikel.[Bezeichnung 1], artikel_kalkulatorische_kosten.Betrag,
                               Fertigung_Fertigungsvorgang.Anzahl*[Preis] AS Ausdr3,
                               Fertigung.Preis, Fertigung.Bemerkung, IIf([Bezeichnung 1]='Reparatur',[Bemerkung],[Bezeichnung 1]) AS Bezfeld,
                               Fertigung.Erstmuster, Artikel.Zolltarif_nr, Fertigung_Fertigungsvorgang.Anzahl*[Materialkosten] AS Material,
                               Artikel.Größe, Fertigung_Fertigungsvorgang.Anzahl*Artikel.Größe AS Gesamtgewicht
                               FROM (((Fertigung_Fertigungsvorgang LEFT JOIN Fertigung ON Fertigung_Fertigungsvorgang.Fertigung_Nr = Fertigung.ID) 
                               LEFT JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
                               LEFT JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]) 
                               LEFT JOIN [View_PSZ_Rechnungsabfrage Eigenfertigung Material 02] ON Artikel.Artikelnummer = [View_PSZ_Rechnungsabfrage Eigenfertigung Material 02].Artikelnummer
                               WHERE ((CAST(Fertigung_Fertigungsvorgang.Datum AS DATE)>=@From And CAST(Fertigung_Fertigungsvorgang.Datum AS DATE)<=@To) 
                               AND ((Artikel.Artikelnummer) Not Like '987%') AND ((Fertigung.Lagerort_id)=14 Or (Fertigung.Lagerort_id)=15) 
                               AND ((Fertigung_Fertigungsvorgang.ab_buchen)=0))
                               ORDER BY Fertigung_Fertigungsvorgang.Datum, Fertigung.Fertigungsnummer;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("From", From);
				sqlCommand.Parameters.AddWithValue("To", To);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.RgSpritzgussEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.RgSpritzgussEntity>();
			}
		}


		public static List<Infrastructure.Data.Entities.Joins.CTS.RgSpritzgussEntity> GetRGWerkzeugbau(DateTime From, DateTime To)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"SELECT Fertigung_Fertigungsvorgang.Datum, Fertigung.Fertigungsnummer, Fertigung.Originalanzahl,
                               Fertigung.Anzahl_erledigt, Fertigung_Fertigungsvorgang.Anzahl, Artikel.Artikelnummer,
                               Artikel.[Bezeichnung 1], artikel_kalkulatorische_kosten.Betrag,
                               Fertigung_Fertigungsvorgang.Anzahl*[Preis] AS Ausdr3,
                               Fertigung.Preis, Fertigung.Bemerkung, IIf([Bezeichnung 1]='Reparatur',[Bemerkung],[Bezeichnung 1]) AS Bezfeld,
                               Fertigung.Erstmuster, Artikel.Zolltarif_nr, Fertigung_Fertigungsvorgang.Anzahl*[Materialkosten] AS Material,
                               Artikel.Größe, Fertigung_Fertigungsvorgang.Anzahl*Artikel.Größe AS Gesamtgewicht
                               FROM (((Fertigung_Fertigungsvorgang LEFT JOIN Fertigung ON Fertigung_Fertigungsvorgang.Fertigung_Nr = Fertigung.ID) 
                               LEFT JOIN Artikel ON Fertigung.Artikel_Nr = Artikel.[Artikel-Nr]) 
                               LEFT JOIN artikel_kalkulatorische_kosten ON Artikel.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]) 
                               LEFT JOIN [View_PSZ_Rechnungsabfrage Eigenfertigung Material 02] ON Artikel.Artikelnummer = [View_PSZ_Rechnungsabfrage Eigenfertigung Material 02].Artikelnummer
                               WHERE ((CAST(Fertigung_Fertigungsvorgang.Datum AS DATE)>=@From And CAST(Fertigung_Fertigungsvorgang.Datum AS DATE)<=@To) 
                               AND ((Artikel.Artikelnummer) Like '987%') AND ((Fertigung.Lagerort_id)=14 Or (Fertigung.Lagerort_id)=15) 
                               AND ((Fertigung_Fertigungsvorgang.ab_buchen)=0))
                               ORDER BY Fertigung_Fertigungsvorgang.Datum, Fertigung.Fertigungsnummer;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("From", From);
				sqlCommand.Parameters.AddWithValue("To", To);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.RgSpritzgussEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.RgSpritzgussEntity>();
			}
		}
		#region dashbord blanket
		public static List<int> GetABLinkedTorahmens(List<int> raNrs = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				//                string query = @"select distinct [AB Pos zu RA Pos] from [angebotene Artikel] where Nr in
				//(
				//select [LS Pos zu AB Pos] from [angebotene Artikel] where [LS Pos zu AB Pos] in 
				//(
				//Select AR.Nr from [angebotene Artikel] AR inner join Angebote A on AR.[Angebot-Nr]=A.Nr
				//Where [AB Pos zu RA Pos] is not null and [AB Pos zu RA Pos] <>0 and A.Typ='Auftragsbestätigung'
				//)
				//)";
				string query = $@"select distinct [AB Pos zu RA Pos] from [angebotene Artikel] 
                               where [AB Pos zu RA Pos] is not null and [AB Pos zu RA Pos]<>0 and [AB Pos zu RA Pos]<>-1 {(raNrs == null ? "" : ($" AND [Angebot-Nr] IN ({string.Join(",", raNrs)})"))}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => Convert.ToInt32(x["AB Pos zu RA Pos"])).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> GetLSLinkedToAbs(List<int> Nrs)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select * from [angebotene Artikel] where [LS Pos zu AB Pos] IN ({string.Join(",", Nrs)}) ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.RahmenExportEntity> GetExportBlanket(bool ignoreClosed)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"SELECT 
									a.[Bezug] BlanketDocNumber,
									a.[Angebot-Nr] BlanketNumber,
									a.[Vorname/NameFirma] SupplierName,
									b.Artikelnummer ArticleNumber,
									b.[Bezeichnung 1] ArticleDesignation1,
									b.[Bezeichnung 2] ArticleDesignation2,
									ISNULL(x.Zielmenge,0) OriginalQuantity,
									ISNULL(x.Zielmenge,0) - ISNULL(p.Geliefert,0) RestQuantity,
									ISNULL(x.Preis,0) UnitPrice,
									(ISNULL(x.Zielmenge,0) - ISNULL(p.Geliefert,0)) * ISNULL(x.Preis,0) RestPrice,
									x.GultigAb StartDate,
									x.ExtensionDate EndDate,
									e.StatusName [Status],
									case
									when p.OriginalAnzahl>0 and p.OriginalAnzahl is not null then (CAST(p.Geliefert AS MONEY)/p.OriginalAnzahl)*100
									else 0
									end as Consumption
									FROM Angebote a 
									join __CTS_AngeboteBlanketExtension e on e.AngeboteNr=a.Nr
									LEFT join [angebotene Artikel] p on p.[Angebot-Nr]=a.Nr
									LEFT join __CTS_AngeboteArticleBlanketExtension x on x.AngeboteArtikelNr=p.Nr
									LEFT join Artikel b on b.[Artikel-Nr]=p.[Artikel-Nr]
									WHERE e.BlanketTypeId=1 {(ignoreClosed ? "AND e.Status<>4" : "")}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.RahmenExportEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.RahmenExportEntity>();
			}
		}
		#endregion


		public static List<Entities.Joins.CTS.DeliveryNotesCompilationEntity> GetDeliveryNotesCompilation(int customerNumber, DateTime dateFrom, DateTime dateTo)
		{
			var sqlConnection = new SqlConnection(Settings.ConnectionString);
			sqlConnection.Open();
			var dataTable = new DataTable();
			#region OLD QUERY
			//			string query = $@"SELECT Angebote.[Nr] [Angebote_Nr]
			//								,Angebote.[Projekt-Nr] [Angebote_Projekt-Nr]
			//								,Angebote.[Angebot-Nr] [Angebote_Angebot-Nr]
			//								,Angebote.[Typ] [Angebote_Typ]
			//								,Angebote.[Datum] [Angebote_Datum]
			//								,Angebote.[Liefertermin] [Angebote_Liefertermin]
			//								,Angebote.[Kunden-Nr] [Angebote_Kunden-Nr]
			//								,Angebote.[Debitorennummer] [Angebote_Debitorennummer]
			//								,Angebote.[Fälligkeit] [Angebote_Fälligkeit]
			//								,Angebote.[Anrede] [Angebote_Anrede]
			//								,Angebote.[Vorname/NameFirma] [Angebote_Vorname/NameFirma]
			//								,Angebote.[Name2] [Angebote_Name2]
			//								,Angebote.[Name3] [Angebote_Name3]
			//								,Angebote.[Ansprechpartner] [Angebote_Ansprechpartner]
			//								,Angebote.[Abteilung] [Angebote_Abteilung]
			//								,Angebote.[Straße/Postfach] [Angebote_Straße/Postfach]
			//								,Angebote.[Land/PLZ/Ort] [Angebote_Land/PLZ/Ort]
			//								,Angebote.[Briefanrede] [Angebote_Briefanrede]
			//								,Angebote.[LAnrede] [Angebote_LAnrede]
			//								,Angebote.[LVorname/NameFirma] [Angebote_LVorname/NameFirma]
			//								,Angebote.[LName2] [Angebote_LName2]
			//								,Angebote.[LName3] [Angebote_LName3]
			//								,Angebote.[LAnsprechpartner] [Angebote_LAnsprechpartner]
			//								,Angebote.[LAbteilung] [Angebote_LAbteilung]
			//								,Angebote.[LStraße/Postfach] [Angebote_LStraße/Postfach]
			//								,Angebote.[LLand/PLZ/Ort] [Angebote_LLand/PLZ/Ort]
			//								,Angebote.[LBriefanrede] [Angebote_LBriefanrede]
			//								,Angebote.[Personal-Nr] [Angebote_Personal-Nr]
			//								,Angebote.[Versandart] [Angebote_Versandart]
			//								,Angebote.[Zahlungsweise] [Angebote_Zahlungsweise]
			//								,Angebote.[Konditionen] [Angebote_Konditionen]
			//								,Angebote.[Zahlungsziel] [Angebote_Zahlungsziel]
			//								,Angebote.[USt_Berechnen] [Angebote_USt_Berechnen]
			//								,Angebote.[Bezug] [Angebote_Bezug]
			//								,Angebote.[Ihr Zeichen] [Angebote_Ihr Zeichen]
			//								,Angebote.[Unser Zeichen] [Angebote_Unser Zeichen]
			//								,Angebote.[Freitext] [Angebote_Freitext]
			//								,Angebote.[gebucht] [Angebote_gebucht]
			//								,Angebote.[gedruckt] [Angebote_gedruckt]
			//								,Angebote.[erledigt] [Angebote_erledigt]
			//								,Angebote.[Auswahl] [Angebote_Auswahl]
			//								,Angebote.[Mahnung] [Angebote_Mahnung]
			//								,Angebote.[Lieferadresse] [Angebote_Lieferadresse]
			//								,Angebote.[reparatur_nr] [Angebote_reparatur_nr]
			//								,Angebote.[Interessent] [Angebote_Interessent]
			//								,Angebote.[ab_id] [Angebote_ab_id]
			//								,Angebote.[datueber] [Angebote_datueber]
			//								,Angebote.[nr_BV] [Angebote_nr_BV]
			//								,Angebote.[nr_RA] [Angebote_nr_RA]
			//								,Angebote.[nr_Kanban] [Angebote_nr_Kanban]
			//								,Angebote.[nr_ang] [Angebote_nr_ang]
			//								,Angebote.[nr_auf] [Angebote_nr_auf]
			//								,Angebote.[nr_lie] [Angebote_nr_lie]
			//								,Angebote.[nr_rec] [Angebote_nr_rec]
			//								,Angebote.[nr_pro] [Angebote_nr_pro]
			//								,Angebote.[nr_gut] [Angebote_nr_gut]
			//								,Angebote.[nr_sto] [Angebote_nr_sto]
			//								,Angebote.[Status] [Angebote_Status]
			//								,Angebote.[Bemerkung] [Angebote_Bemerkung]
			//								,Angebote.[Belegkreis] [Angebote_Belegkreis]
			//								,Angebote.[Bereich] [Angebote_Bereich]
			//								,Angebote.[Wunschtermin] [Angebote_Wunschtermin]
			//								,Angebote.[Benutzer] [Angebote_Benutzer]
			//								,Angebote.[Mandant] [Angebote_Mandant]
			//								,Angebote.[Neu] [Angebote_Neu]
			//								,Angebote.[Löschen] [Angebote_Löschen]
			//								,Angebote.[In Bearbeitung] [Angebote_In Bearbeitung]
			//								,Angebote.[Öffnen] [Angebote_Öffnen]
			//								,Angebote.[termin_eingehalten] [Angebote_termin_eingehalten]
			//								,Angebote.[Versandarten_Auswahl] [Angebote_Versandarten_Auswahl]
			//								,Angebote.[EDI_Order_Neu] [Angebote_EDI_Order_Neu]
			//								,Angebote.[EDI_Order_Change] [Angebote_EDI_Order_Change]
			//								,Angebote.[EDI_Order_Change_Updated] [Angebote_EDI_Order_Change_Updated]
			//								,Angebote.[EDI_Kundenbestellnummer] [Angebote_EDI_Kundenbestellnummer]
			//								,Angebote.[EDI_Dateiname_CSV] [Angebote_EDI_Dateiname_CSV]
			//								,Angebote.[ABSENDER] [Angebote_ABSENDER]
			//								,Angebote.[Freie_Text] [Angebote_Freie_Text]
			//								,Angebote.[Dplatz_Sirona] [Angebote_Dplatz_Sirona]
			//								,Angebote.[Neu_Order] [Angebote_Neu_Order]
			//								,Angebote.[nr_dlf] [Angebote_nr_dlf]
			//								,Angebote.[rec_sent] [Angebote_rec_sent]
			//								, 
			//								[Angebotene Artikel].[Nr] AS [ANgeboteArtikel_Nr]
			//								,[Angebotene Artikel].[Position] AS [ANgeboteArtikel_Position]
			//								,[Angebotene Artikel].[Angebot-Nr] AS [ANgeboteArtikel_Angebot-Nr]
			//								,[Angebotene Artikel].[Artikel-Nr] AS [ANgeboteArtikel_Artikel-Nr]
			//								,[Angebotene Artikel].[Bezeichnung1] AS [ANgeboteArtikel_Bezeichnung1]
			//								,[Angebotene Artikel].[Bezeichnung2] AS [ANgeboteArtikel_Bezeichnung2]
			//								,[Angebotene Artikel].[Bezeichnung3] AS [ANgeboteArtikel_Bezeichnung3]
			//								,[Angebotene Artikel].[Einheit] AS [ANgeboteArtikel_Einheit]
			//								,[Angebotene Artikel].[AnfangLagerBestand] AS [ANgeboteArtikel_AnfangLagerBestand]
			//								,[Angebotene Artikel].[Anzahl] AS [ANgeboteArtikel_Anzahl]
			//								,[Angebotene Artikel].[OriginalAnzahl] AS [ANgeboteArtikel_OriginalAnzahl]
			//								,[Angebotene Artikel].[Geliefert] AS [ANgeboteArtikel_Geliefert]
			//								,[Angebotene Artikel].[Aktuelle Anzahl] AS [ANgeboteArtikel_Aktuelle Anzahl]
			//								,[Angebotene Artikel].[EndeLagerBestand] AS [ANgeboteArtikel_EndeLagerBestand]
			//								,[Angebotene Artikel].[Einzelpreis] AS [ANgeboteArtikel_Einzelpreis]
			//								,[Angebotene Artikel].[Gesamtpreis] AS [ANgeboteArtikel_Gesamtpreis]
			//								,[Angebotene Artikel].[Preisgruppe] AS [ANgeboteArtikel_Preisgruppe]
			//								,[Angebotene Artikel].[Bestellnummer] AS [ANgeboteArtikel_Bestellnummer]
			//								,[Angebotene Artikel].[Rabatt] AS [ANgeboteArtikel_Rabatt]
			//								,[Angebotene Artikel].[USt] AS [ANgeboteArtikel_USt]
			//								,[Angebotene Artikel].[Lagerbewegung] AS [ANgeboteArtikel_Lagerbewegung]
			//								,[Angebotene Artikel].[Lagerbewegung_rückgängig] AS [ANgeboteArtikel_Lagerbewegung_rückgängig]
			//								,[Angebotene Artikel].[Auswahl] AS [ANgeboteArtikel_Auswahl]
			//								,[Angebotene Artikel].[FM_Einzelpreis] AS [ANgeboteArtikel_FM_Einzelpreis]
			//								,[Angebotene Artikel].[FM_Gesamtpreis] AS [ANgeboteArtikel_FM_Gesamtpreis]
			//								,[Angebotene Artikel].[FM] AS [ANgeboteArtikel_FM]
			//								,[Angebotene Artikel].[Summenberechnung] AS [ANgeboteArtikel_Summenberechnung]
			//								,[Angebotene Artikel].[zwischensumme] AS [ANgeboteArtikel_zwischensumme]
			//								,[Angebotene Artikel].[POSTEXT] AS [ANgeboteArtikel_POSTEXT]
			//								,[Angebotene Artikel].[schriftart] AS [ANgeboteArtikel_schriftart]
			//								,[Angebotene Artikel].[sortierung] AS [ANgeboteArtikel_sortierung]
			//								,[Angebotene Artikel].[Preiseinheit] AS [ANgeboteArtikel_Preiseinheit]
			//								,[Angebotene Artikel].[Preis_ausweisen] AS [ANgeboteArtikel_Preis_ausweisen]
			//								,[Angebotene Artikel].[Zeichnungsnummer] AS [ANgeboteArtikel_Zeichnungsnummer]
			//								,[Angebotene Artikel].[Liefertermin] AS [ANgeboteArtikel_Liefertermin]
			//								,[Angebotene Artikel].[erledigt_pos] AS [ANgeboteArtikel_erledigt_pos]
			//								,[Angebotene Artikel].[Stückliste] AS [ANgeboteArtikel_Stückliste]
			//								,[Angebotene Artikel].[Stückliste_drucken] AS [ANgeboteArtikel_Stückliste_drucken]
			//								,[Angebotene Artikel].[Langtext] AS [ANgeboteArtikel_Langtext]
			//								,[Angebotene Artikel].[Langtext_drucken] AS [ANgeboteArtikel_Langtext_drucken]
			//								,[Angebotene Artikel].[Lagerort_id] AS [ANgeboteArtikel_Lagerort_id]
			//								,[Angebotene Artikel].[Seriennummern_drucken] AS [ANgeboteArtikel_Seriennummern_drucken]
			//								,[Angebotene Artikel].[Wunschtermin] AS [ANgeboteArtikel_Wunschtermin]
			//								,[Angebotene Artikel].[Fertigungsnummer] AS [ANgeboteArtikel_Fertigungsnummer]
			//								,[Angebotene Artikel].[LS Pos zu KB Pos] AS [ANgeboteArtikel_LS Pos zu KB Pos]
			//								,[Angebotene Artikel].[LS Pos zu AB Pos] AS [ANgeboteArtikel_LS Pos zu AB Pos]
			//								,[Angebotene Artikel].[RA Pos zu BV Pos] AS [ANgeboteArtikel_RA Pos zu BV Pos]
			//								,[Angebotene Artikel].[KB Pos zu BV Pos] AS [ANgeboteArtikel_KB Pos zu BV Pos]
			//								,[Angebotene Artikel].[AB Pos zu BV Pos] AS [ANgeboteArtikel_AB Pos zu BV Pos]
			//								,[Angebotene Artikel].[KB Pos zu RA Pos] AS [ANgeboteArtikel_KB Pos zu RA Pos]
			//								,[Angebotene Artikel].[AB Pos zu RA Pos] AS [ANgeboteArtikel_AB Pos zu RA Pos]
			//								,[Angebotene Artikel].[VK-Festpreis] AS [ANgeboteArtikel_VK-Festpreis]
			//								,[Angebotene Artikel].[Kupferbasis] AS [ANgeboteArtikel_Kupferbasis]
			//								,[Angebotene Artikel].[DEL] AS [ANgeboteArtikel_DEL]
			//								,[Angebotene Artikel].[EinzelCu-Gewicht] AS [ANgeboteArtikel_EinzelCu-Gewicht]
			//								,[Angebotene Artikel].[GesamtCu-Gewicht] AS [ANgeboteArtikel_GesamtCu-Gewicht]
			//								,[Angebotene Artikel].[Einzelkupferzuschlag] AS [ANgeboteArtikel_Einzelkupferzuschlag]
			//								,[Angebotene Artikel].[Gesamtkupferzuschlag] AS [ANgeboteArtikel_Gesamtkupferzuschlag]
			//								,[Angebotene Artikel].[VKEinzelpreis] AS [ANgeboteArtikel_VKEinzelpreis]
			//								,[Angebotene Artikel].[VKGesamtpreis] AS [ANgeboteArtikel_VKGesamtpreis]
			//								,[Angebotene Artikel].[DEL fixiert] AS [ANgeboteArtikel_DEL fixiert]
			//								,[Angebotene Artikel].[Löschen] AS [ANgeboteArtikel_Löschen]
			//								,[Angebotene Artikel].[In Bearbeitung] AS [ANgeboteArtikel_In Bearbeitung]
			//								,[Angebotene Artikel].[RA_OriginalAnzahl] AS [ANgeboteArtikel_RA_OriginalAnzahl]
			//								,[Angebotene Artikel].[RA_Abgerufen] AS [ANgeboteArtikel_RA_Abgerufen]
			//								,[Angebotene Artikel].[RA_Offen] AS [ANgeboteArtikel_RA_Offen]
			//								,[Angebotene Artikel].[Packstatus] AS [ANgeboteArtikel_Packstatus]
			//								,[Angebotene Artikel].[Versandinfo_von_CS] AS [ANgeboteArtikel_Versandinfo_von_CS]
			//								,[Angebotene Artikel].[Gepackt_von] AS [ANgeboteArtikel_Gepackt_von]
			//								,[Angebotene Artikel].[Gepackt_Zeitpunkt] AS [ANgeboteArtikel_Gepackt_Zeitpunkt]
			//								,[Angebotene Artikel].[Packinfo_von_Lager] AS [ANgeboteArtikel_Packinfo_von_Lager]
			//								,[Angebotene Artikel].[Versandstatus] AS [ANgeboteArtikel_Versandstatus]
			//								,[Angebotene Artikel].[Versanddienstleister] AS [ANgeboteArtikel_Versanddienstleister]
			//								,[Angebotene Artikel].[Versandnummer] AS [ANgeboteArtikel_Versandnummer]
			//								,[Angebotene Artikel].[Versandinfo_von_Lager] AS [ANgeboteArtikel_Versandinfo_von_Lager]
			//								,[Angebotene Artikel].[Versand_gedruckt] AS [ANgeboteArtikel_Versand_gedruckt]
			//								,[Angebotene Artikel].[Abladestelle] AS [ANgeboteArtikel_Abladestelle]
			//								,[Angebotene Artikel].[LS_von_Versand_gedruckt] AS [ANgeboteArtikel_LS_von_Versand_gedruckt]
			//								,[Angebotene Artikel].[termin_eingehalten] AS [ANgeboteArtikel_termin_eingehalten]
			//								,[Angebotene Artikel].[Versandarten_Auswahl] AS [ANgeboteArtikel_Versandarten_Auswahl]
			//								,[Angebotene Artikel].[Versanddatum_Auswahl] AS [ANgeboteArtikel_Versanddatum_Auswahl]
			//								,[Angebotene Artikel].[RP] AS [ANgeboteArtikel_RP]
			//								,[Angebotene Artikel].[Bezeichnung2_Kunde] AS [ANgeboteArtikel_Bezeichnung2_Kunde]
			//								,[Angebotene Artikel].[EDI_Quantity_Ordered] AS [ANgeboteArtikel_EDI_Quantity_Ordered]
			//								,[Angebotene Artikel].[Freies_Format_EDI] AS [ANgeboteArtikel_Freies_Format_EDI]
			//								,[Angebotene Artikel].[EDI_Historie_Nr] AS [ANgeboteArtikel_EDI_Historie_Nr]
			//								,[Angebotene Artikel].[Lieferanweisung (P_FTXDIN_TEXT)] AS [ANgeboteArtikel_Lieferanweisung (P_FTXDIN_TEXT)]
			//								,[Angebotene Artikel].[Bemerkungsfeld1] AS [ANgeboteArtikel_Bemerkungsfeld1]
			//								,[Angebotene Artikel].[Bemerkungsfeld2] AS [ANgeboteArtikel_Bemerkungsfeld2]
			//								,[Angebotene Artikel].[PositionZUEDI] AS [ANgeboteArtikel_PositionZUEDI]
			//								,[Angebotene Artikel].[EKPreise_Fix] AS [ANgeboteArtikel_EKPreise_Fix]
			//								,[Angebotene Artikel].[VDA_gedruckt] AS [ANgeboteArtikel_VDA_gedruckt]
			//								,[Angebotene Artikel].[EDI_PREIS_KUNDE] AS [ANgeboteArtikel_EDI_PREIS_KUNDE]
			//								,[Angebotene Artikel].[EDI_PREISEINHEIT] AS [ANgeboteArtikel_EDI_PREISEINHEIT]
			//								,[Angebotene Artikel].[Typ] AS [ANgeboteArtikel_Typ]
			//								,[Angebotene Artikel].[Zuschlag_VK] AS [ANgeboteArtikel_Zuschlag_VK]
			//								,[Angebotene Artikel].[Index_Kunde] AS [ANgeboteArtikel_Index_Kunde]
			//								,[Angebotene Artikel].[Index_Kunde_datum] AS [ANgeboteArtikel_Index_Kunde_datum]
			//								,[Angebotene Artikel].[CSInterneBemerkung] AS [ANgeboteArtikel_CSInterneBemerkung]
			//								,[Angebotene Artikel].[RE Pos zu GS Pos] AS [ANgeboteArtikel_RE Pos zu GS Pos]
			//								,[Angebotene Artikel].[GSInternComment] AS [ANgeboteArtikel_GSInternComment]
			//								,[Angebotene Artikel].[GSExternComment] AS [ANgeboteArtikel_GSExternComment]
			//								,[Angebotene Artikel].[GSWithoutCopper] AS [ANgeboteArtikel_GSWithoutCopper]
			//								, 
			//								Konditionszuordnungstabelle.Skontotage, 
			//									Konditionszuordnungstabelle.Skonto, Konditionszuordnungstabelle.Nettotage, 
			//									Artikel.Artikelnummer, Angebote.Nr, adressen.Fax, Artikel.Ursprungsland, 
			//									[Textbausteine AB LS RG GU B].Auftragsbestätigung, 
			//									[Textbausteine AB LS RG GU B].Lieferschein, [Textbausteine AB LS RG GU B].Rechnung, 
			//									[Textbausteine AB LS RG GU B].Gutschrift, Artikel.Zolltarif_nr, 
			//									Artikel.Größe*[angebotene Artikel].Anzahl AS Gesamtgewicht, Artikel.Größe, Angebote.[Angebot-Nr] AS LS

			//								FROM [Textbausteine AB LS RG GU B], 
			//								Angebote INNER JOIN [angebotene Artikel] ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]
			//								INNER JOIN Kunden ON Angebote.[Kunden-Nr] = Kunden.nummer
			//								INNER JOIN Konditionszuordnungstabelle 
			//									ON Kunden.[Konditionszuordnungs-Nr] = Konditionszuordnungstabelle.Nr 
			//									INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr] 
			//									INNER JOIN adressen ON Kunden.nummer = adressen.Nr
			//								WHERE Angebote.[Unser Zeichen]='{customerNumber}' 
			//									AND [angebotene Artikel].Liefertermin>='{dateFrom.ToString("yyyyMMdd")}' 
			//									And [angebotene Artikel].Liefertermin<='{dateTo.ToString("yyyyMMdd")}' AND Angebote.Typ='Lieferschein';

			//";
			#endregion OLD QUERY

			string query = $@"SELECT Angebote.[Nr] [Angebote_Nr]
								,Angebote.[Bezug] [Angebote_Bezug]
								,Angebote.[Angebot-Nr] [Angebote_Angebot-Nr]
								,Angebote.[Anrede] [Angebote_Anrede]
								,Angebote.[Vorname/NameFirma] [Angebote_Vorname/NameFirma]
								,Angebote.[Name2] [Angebote_Name2]
								,Angebote.[Name3] [Angebote_Name3]
								,Angebote.[Straße/Postfach] [Angebote_Straße/Postfach]
								,Angebote.[Land/PLZ/Ort] [Angebote_Land/PLZ/Ort]
								,Angebote.[LAnrede] [Angebote_LAnrede]
								,Angebote.[LVorname/NameFirma] [Angebote_LVorname/NameFirma]
								,Angebote.[LName2] [Angebote_LName2]
								,Angebote.[LName3] [Angebote_LName3]
								,Angebote.[LStraße/Postfach] [Angebote_LStraße/Postfach]
								,Angebote.[LLand/PLZ/Ort] [Angebote_LLand/PLZ/Ort]
								,Angebote.[Versandart] [Angebote_Versandart]
								,Angebote.[Ihr Zeichen] [Angebote_Ihr Zeichen]
								,Angebote.[Unser Zeichen] [Angebote_Unser Zeichen]
								,Angebote.[Freitext] [Angebote_Freitext]
								,Angebote.[Lieferadresse] [Angebote_Lieferadresse]
								,[Angebotene Artikel].[Anzahl] AS [ANgeboteArtikel_Anzahl]
								,[Angebotene Artikel].[Bezeichnung1] AS [ANgeboteArtikel_Bezeichnung1]
								,[Angebotene Artikel].[Bezeichnung2] AS [ANgeboteArtikel_Bezeichnung2]
								,[Angebotene Artikel].[Bezeichnung3] AS [ANgeboteArtikel_Bezeichnung3]
								,[Angebotene Artikel].[Einheit] AS [ANgeboteArtikel_Einheit]
								,[Angebotene Artikel].[Liefertermin] AS [ANgeboteArtikel_Liefertermin]
								,Artikel.[Größe]/1000 AS [ANgeboteArtikel_EinzelCu-Gewicht]
								,[Angebotene Artikel].[Bezeichnung2_Kunde] AS [ANgeboteArtikel_Bezeichnung2_Kunde] 
								,ISNULL(Artikel.[Größe],0)/1000*ISNULL([Angebotene Artikel].[Anzahl],0) AS [ANgeboteArtikel_GesamtCu-Gewicht]
								,Artikel.Artikelnummer, 
								Artikel.Ursprungsland, 
								Artikel.Zolltarif_nr,
								[Textbausteine AB LS RG GU B].Lieferschein

								FROM [Textbausteine AB LS RG GU B], 
								Angebote INNER JOIN [angebotene Artikel] ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]
								INNER JOIN Kunden ON Angebote.[Kunden-Nr] = Kunden.nummer
								INNER JOIN Konditionszuordnungstabelle 
									ON Kunden.[Konditionszuordnungs-Nr] = Konditionszuordnungstabelle.Nr 
									INNER JOIN Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr] 
									INNER JOIN adressen ON Kunden.nummer = adressen.Nr
								WHERE Angebote.[Unser Zeichen]='{customerNumber}' 
									AND CAST([angebotene Artikel].Liefertermin AS DATE)>='{dateFrom.ToString("yyyyMMdd")}' 
									AND CAST([angebotene Artikel].Liefertermin AS DATE)<='{dateTo.ToString("yyyyMMdd")}' AND Angebote.Typ='Lieferschein' ORDER BY [angebotene Artikel].Liefertermin,Artikel.Artikelnummer;

";
			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.CommandTimeout = 180; // 3 mn
			new SqlDataAdapter(sqlCommand).Fill(dataTable);
			sqlConnection.Close();

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.CTS.DeliveryNotesCompilationEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.CTS.DeliveryNotesCompilationEntity>();
			}
		}

		#region Capa
		public static IEnumerable<Infrastructure.Data.Entities.Joins.CTS.CapacityEntity> GetCapacityStatus(DateTime? dateFrom, DateTime? dateTo)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var connectionId = sqlConnection.ClientConnectionId.ToString().Replace('-', '_');
				string faTableName = $"#CTE_FA_{connectionId}";
				string abTableName = $"#CTE_AB_{connectionId}";
				string lpTableName = $"#CTE_LP_{connectionId}";
				string arTableName = $"#CTE_ART_{connectionId}";
				string frcTableName = $"#CTE_FRC_{connectionId}";
				string query = $@"DROP TABLE IF EXISTS {abTableName}; 
								DROP TABLE IF EXISTS {faTableName}; 
								DROP TABLE IF EXISTS {lpTableName}; 
								DROP TABLE IF EXISTS {arTableName}; 								
                                DROP TABLE IF EXISTS {frcTableName}; 


								/* -- // -- */
								SELECT pab.[Artikel-Nr] AS Artikel_Nr/*, ab.Nr, pab.Nr AS pNr, ab.[Angebot-Nr], ab.Bezug*/, SUM(ISNULL(pab.Anzahl,0)) AS Anzahl  INTO {abTableName} FROM Angebote ab join [angebotene Artikel] pab ON pab.[Angebot-Nr]=ab.Nr
								WHERE ab.Typ='auftragsbestätigung' AND ISNULL(ab.gebucht,0)=1 AND ISNULL(ab.erledigt,0)=0 AND ISNULL(pab.erledigt_pos,0)=0 AND (pab.Liefertermin IS NOT NULL OR pab.Wunschtermin IS NOT NULL) 
								{(dateTo.HasValue ? $" AND ISNULL(pab.Liefertermin, pab.Wunschtermin) <= '{dateTo.Value.ToString("yyyyMMdd")}'" : "")} 
								{(dateFrom.HasValue ? $" AND ISNULL(pab.Liefertermin, pab.Wunschtermin) >= '{dateFrom.Value.ToString("yyyyMMdd")}'" : "")} GROUP BY pab.[Artikel-Nr];
								/* -- // -- */
								SELECT f.Artikel_Nr/*, f.ID, f.Fertigungsnummer, f.Termin_Bestätigt1*/, SUM(ISNULL(f.Anzahl,0)) AS Anzahl INTO {faTableName} FROM Fertigung f JOIN (SELECT [Artikel-Nr], UBG FROM Artikel WHERE Warengruppe='EF') a ON a.[Artikel-Nr]=f.Artikel_Nr
								WHERE f.Kennzeichen='offen' AND f.Termin_Bestätigt1 IS NOT NULL  AND ISNULL(a.UBG,0)=0
								{(dateTo.HasValue ? $" AND f.Termin_Bestätigt1 <= '{dateTo.Value.AddDays(-2 * 7).ToString("yyyyMMdd")}'" : "")}
								{(dateFrom.HasValue ? $" AND f.Termin_Bestätigt1 >= '{dateFrom.Value.AddDays(-2 * 7).ToString("yyyyMMdd")}'" : "")} GROUP BY f.[Artikel_Nr];
								/* -- // -- */
								SELECT a.[Artikel-Nr] AS Artikel_Nr, SUM(ISNULL(PlanningQuantityQuantity,0) - ISNULL(p.Originalanzahl,0)) Anzahl INTO {lpTableName} FROM (SELECT Id, PlanningQuantityQuantity, OrderItemId, LineItemId FROM __EDI_DLF_LineItemPlan 
								WHERE PlanningQuantityRequestedShipmentDate IS NOT NULL AND PlanningQuantityRequestedShipmentDate >= '{DateTime.Today.ToString("yyyyMMdd")}'
								{(dateTo.HasValue ? $" AND PlanningQuantityRequestedShipmentDate <= '{dateTo.Value.ToString("yyyyMMdd")}'" : "")} 
								{(dateFrom.HasValue ? $" AND PlanningQuantityRequestedShipmentDate >= '{dateFrom.Value.ToString("yyyyMMdd")}'" : "")}
								AND LineItemId IN (SELECT Id FROM __EDI_DLF_LineItem WHERE HeaderId IN (SELECT h.Id FROM __EDI_DLF_Header h join (SELECT PSZCustomernumber, DocumentNumber, MAX(ReferenceVersionNumber) as ReferenceVersionNumber FROM __EDI_DLF_Header WHERE ISNULL(Done,0)=0 GROUP BY PSZCustomernumber, DocumentNumber) u on u.DocumentNumber=h.DocumentNumber and u.PSZCustomernumber=h.PSZCustomernumber and u.ReferenceVersionNumber=h.ReferenceVersionNumber))) as li LEFT JOIN (SELECT Nr, Originalanzahl, [Artikel-Nr] FROM [angebotene Artikel] WHERE [Angebot-Nr] IN (SELECT Nr FROM Angebote WHERE Typ='Auftragsbestätigung') ) p on p.Nr=li.OrderItemId
								JOIN __EDI_DLF_LineItem l on l.Id=li.LineItemId JOIN (SELECT [Artikel-Nr], Artikelnummer FROM Artikel) AS a on a.Artikelnummer=l.SuppliersItemMaterialNumber
								GROUP BY a.[Artikel-Nr];						
								/* -- // -- */
								WITH LastVersion as (
								SELECT [TypeId], [kundennummer], MAX(Version) [Version] from [Forecasts]
								GROUP BY [TypeId], [kundennummer])
                                SELECT ArtikelNr as Artikel_Nr,SUM(Menge) AS Anzahl into {frcTableName} FROM [LastVersion] l JOIN [Forecasts] f ON l.kundennummer=f.kundennummer AND l.TypeId=f.TypeId AND l.[Version]=f.[Version]
								JOIN [ForecastsPosition] p on p.IdForcast=f.Id WHERE p.Datum IS NOT NULL AND ISNULL(p.IsOrdered,0)=0
                                {(dateTo.HasValue ? $" AND p.Datum <= '{dateTo.Value.ToString("yyyyMMdd")}'" : "")} 
								{(dateFrom.HasValue ? $" AND p.Datum >= '{dateFrom.Value.ToString("yyyyMMdd")}'" : "")}
                                GROUP BY [ArtikelNr];
                                /* -- // -- */
								SELECT DISTINCT Artikel_Nr INTO {arTableName} FROM (SELECT DISTINCT Artikel_Nr FROM {abTableName} UNION ALL SELECT Artikel_Nr FROM {lpTableName} UNION ALL SELECT Artikel_Nr FROM {faTableName} UNION ALL SELECT Artikel_Nr FROM {frcTableName}) AS T WHERE ISNULL(Artikel_Nr,0)>0;

								/* --select count(*) as countAB From {abTableName}; -- */
								/* --select count(*) as countFA From {faTableName}; -- */
								/* --select count(*) as countFA From {lpTableName}; -- */
								/* --select count(*) as countArticles From {arTableName}; -- */

								/* -- // -- */
								SELECT DISTINCT Artikel_Nr,Artikelnummer, faCount, ISNULL(faAnzahl,0) AS faAnzahl, ISNULL(abAnzahl,0) AS abAnzahl, abPosCount, ISNULL(lpAnzahl,0) AS lpAnzahl,lpCount,ISNULL(frcAnzahl,0) as frcAnzahl,frcCount, ISNULL(Bestand,0) AS Bestand, ISNULL(Mindestbestand,0) AS Mindestbestand, (ISNULL(faAnzahl,0)+ISNULL(Bestand,0) - (ISNULL(lpAnzahl,0)+ISNULL(abAnzahl,0)+ISNULL(Mindestbestand,0)+ISNULL(frcAnzahl,0))) Summe FROM (
									SELECT a.Artikel_Nr, (ISNULL(fa.Anzahl,0)) faAnzahl, 0 AS faCount, (ISNULL(ab.Anzahl,0)) abAnzahl, 0 abPosCount, ISNULL(lp.Anzahl,0) lpAnzahl, 0 as lpCount, ISNULL(frc.Anzahl,0) frcAnzahl,0 as frcCount
									FROM {arTableName} a 
									LEFT JOIN {abTableName} ab ON ab.Artikel_Nr=a.Artikel_Nr
									LEFT JOIN {faTableName} fa ON fa.Artikel_Nr=a.Artikel_Nr
									LEFT JOIN {lpTableName} lp ON lp.Artikel_Nr=a.Artikel_Nr									
                                    LEFT JOIN {frcTableName} frc ON frc.Artikel_Nr=a.Artikel_Nr

									/* GROUP BY a.Artikel_Nr */
									) AS D
									LEFT JOIN (SELECT [Artikel-Nr], SUM(ISNULL(Bestand,0)) AS Bestand, SUM(ISNULL(Mindestbestand,0)) AS Mindestbestand FROM Lager WHERE Lagerort_id NOT IN (SELECT Lagerort_id FROM Lagerorte WHERE Lagerort LIKE '%AUSS%') GROUP BY [Artikel-Nr]) l ON l.[Artikel-Nr]=D.Artikel_Nr
									LEFT JOIN (SELECT [Artikel-Nr], Artikelnummer FROM Artikel) aa ON aa.[Artikel-Nr]=D.Artikel_Nr
									ORDER BY Artikelnummer;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.CapacityEntity(x));
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.CapacityABEntity> GetCapacityStatus_AB(int articleId, DateTime? dateFrom, DateTime? dateTo)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT ab.Nr abNr, ab.Bezug abBezug, ab.[Angebot-nr] abNummer, ab.[Vorname/NameFirma] AbCustomer, pab.Position AbPosition, pab.[Artikel-Nr] AS Artikel_Nr, ISNULL(pab.Anzahl,0) AS AbAnzahl, Artikelnummer 
									, 0 FaId, '' FaNummer, '' FaStatus, '' FaLager, 0 faAnzahl 
									FROM Angebote ab join [angebotene Artikel] pab ON pab.[Angebot-Nr]=ab.Nr left join (SELECT Artikelnummer, [Artikel-Nr] FROM Artikel) as a on a.[Artikel-Nr]=pab.[Artikel-Nr]
									WHERE ab.Typ='auftragsbestätigung' AND ISNULL(ab.gebucht,0)=1 AND ISNULL(ab.erledigt,0)=0 AND ISNULL(pab.erledigt_pos,0)=0 AND (pab.Liefertermin IS NOT NULL OR pab.Wunschtermin IS NOT NULL) 
									{(dateTo.HasValue ? $" AND ISNULL(pab.Liefertermin, pab.Wunschtermin) <= '{dateTo.Value.ToString("yyyyMMdd")}'" : "")} 
									{(dateFrom.HasValue ? $" AND ISNULL(pab.Liefertermin, pab.Wunschtermin) >= '{dateFrom.Value.ToString("yyyyMMdd")}'" : "")} 
									AND pab.[Artikel-Nr]=@articleId;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);
				sqlCommand.CommandTimeout = 180;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.CapacityABEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.CapacityABEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.CapacityABEntity> GetCapacityStatus_FA(int articleId, DateTime? dateFrom, DateTime? dateTo)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var connectionId = sqlConnection.ClientConnectionId.ToString().Replace('-', '_');

				string query = $@"SELECT f.ID FaId, f.Fertigungsnummer FaNummer, f.Kennzeichen FaStatus, f.Lagerort_id FaLager, ISNULL(f.Anzahl,0) AS FaAnzahl, f.Artikel_Nr, Artikelnummer 
									,0 abNr, '' abBezug, '' abNummer, '' AbCustomer, 0 AbPosition, 0 AbAnzahl							
									FROM Fertigung f JOIN (SELECT [Artikel-Nr], UBG, Artikelnummer FROM Artikel WHERE Warengruppe='EF') a ON a.[Artikel-Nr]=f.Artikel_Nr
									WHERE f.Kennzeichen='offen' AND f.Termin_Bestätigt1 IS NOT NULL  AND ISNULL(a.UBG,0)=0
										{(dateTo.HasValue ? $" AND f.Termin_Bestätigt1 <= '{dateTo.Value.AddDays(-2 * 7).ToString("yyyyMMdd")}'" : "")}
										{(dateFrom.HasValue ? $" AND f.Termin_Bestätigt1 >= '{dateFrom.Value.AddDays(-2 * 7).ToString("yyyyMMdd")}'" : "")}
										AND f.[Artikel_Nr]=@articleId;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 180;
				sqlCommand.Parameters.AddWithValue("articleId", articleId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.CapacityABEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.CapacityABEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.CapacityLPEntity> GetCapacityStatus_LP(int articleId, DateTime? dateFrom, DateTime? dateTo)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var connectionId = sqlConnection.ClientConnectionId.ToString().Replace('-', '_');

				string query = $@"SELECT h.ManualCreation AS IsManual, a.[Artikel-Nr] AS Artikel_Nr, a.Artikelnummer, h.Id HeaderId, l.Id LineItemId, li.Id LineItemPlanId, h.DocumentNumber Nummer, h.BuyerPartyName Customer, h.PSZCustomernumber, l.PositionNumber Position, SUM(ISNULL(PlanningQuantityQuantity,0) - ISNULL(p.Originalanzahl,0)) AS Anzahl
									FROM (SELECT Id, PlanningQuantityQuantity, OrderItemId, LineItemId FROM __EDI_DLF_LineItemPlan WHERE PlanningQuantityRequestedShipmentDate IS NOT NULL AND PlanningQuantityRequestedShipmentDate >= '{DateTime.Today.ToString("yyyyMMdd")}'
										{(dateTo.HasValue ? $" AND PlanningQuantityRequestedShipmentDate <= '{dateTo.Value.ToString("yyyyMMdd")}'" : "")} 
										{(dateFrom.HasValue ? $" AND PlanningQuantityRequestedShipmentDate >= '{dateFrom.Value.ToString("yyyyMMdd")}'" : "")} AND
									LineItemId IN (SELECT Id FROM __EDI_DLF_LineItem WHERE HeaderId IN (SELECT h.Id FROM __EDI_DLF_Header h join (SELECT PSZCustomernumber, DocumentNumber, MAX(ReferenceVersionNumber) as ReferenceVersionNumber FROM __EDI_DLF_Header WHERE ISNULL(Done,0)=0 GROUP BY PSZCustomernumber, DocumentNumber) u on u.DocumentNumber=h.DocumentNumber and u.PSZCustomernumber=h.PSZCustomernumber and u.ReferenceVersionNumber=h.ReferenceVersionNumber))) as li LEFT JOIN (SELECT Nr, Originalanzahl, [Artikel-Nr] FROM [angebotene Artikel] WHERE [Angebot-Nr] IN (SELECT Nr FROM Angebote WHERE Typ='Auftragsbestätigung')) p on p.Nr=li.OrderItemId
									JOIN __EDI_DLF_LineItem l on l.Id=li.LineItemId
									JOIN Artikel a on a.ArticleNumber=l.SuppliersItemMaterialNumber
									JOIN __EDI_DLF_Header h on h.Id=l.HeaderId
									WHERE a.[Artikel-Nr]=@articleId
									GROUP BY h.ManualCreation, a.[Artikel-Nr], a.Artikelnummer, h.Id, l.Id, li.Id, h.DocumentNumber, h.BuyerPartyName, l.PositionNumber, h.PSZCustomernumber;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 180;
				sqlCommand.Parameters.AddWithValue("articleId", articleId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.CapacityLPEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.CapacityLPEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.CTS.FaProductionStatusEntity> GetFaProductionStatus(string searchValue, string productionStatus, DateTime? dateFrom, DateTime? dateTo, Settings.SortingModel? sorting, Settings.PaginModel? paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT 
Count(*) over () TotalCount,
  f.ID, 
  Fertigungsnummer, 
  f.Termin_Bestätigt2 as Werktermin, 
  0 as Flag, 
  Lagerort_id, 
  f.Termin_Bestätigt1 as Produktionstermin, 
  Anzahl, 
  a.[Artikel-Nr], 
  a.Artikelnummer, 
  Bemerkung, 
  case when ISNULL(f.FA_Gestartet, 0) = 0 Then 'Nicht Gestartet' 
  /*without color*/
  when f.Termin_Bestätigt2 <= GETDATE() Then 'Werktermin im Rückstand' 
  /*red */
  when f.Termin_Bestätigt2 <= f.Termin_Bestätigt1 Then 'Werktermin Ok' 
  /*green */
  else 'Werktermin zu spät' 
  /*orange*/
  end as Status 
FROM 
  Fertigung f 
  LEFT JOIN Artikel a on a.[Artikel-Nr] = f.Artikel_Nr 
WHERE 
  F.Kennzeichen = 'offen' 
AND a.Artikelnummer   NOT LIKE 'Reparatur%'
  AND a.Artikelnummer NOT LIKE 'Technik%' 
  AND a.Artikelnummer NOT LIKE 'Analyse%'
{(dateTo.HasValue ? $" AND F.Termin_Bestätigt1 <= '{dateTo.Value.AddDays(-2 * 7).ToString("yyyyMMdd")}'" : "")}
{(dateFrom.HasValue ? $" AND F.Termin_Bestätigt1 >= '{dateFrom.Value.AddDays(-2 * 7).ToString("yyyyMMdd")}'" : "")}";

				using(var sqlCommand = new SqlCommand())
				{
					if(!String.IsNullOrEmpty(searchValue))
					{
						query += $@" AND  (F.Fertigungsnummer LIKE '{searchValue}%' OR Bemerkung LIKE '%{searchValue}%' OR Artikelnummer LIKE '%{searchValue}%')";
					}

					query += "ORDER BY Produktionstermin ASC";

					if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
					{
						query += $",{sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
					}
					if(paging != null)
					{
						query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
					}

					if(productionStatus != "" && productionStatus != null)
					{
						query = $@"select *,Count(*) over () TotalCount from (SELECT 
  f.ID, 
  Fertigungsnummer, 
  f.Termin_Bestätigt2 as Werktermin, 
  0 as Flag, 
  Lagerort_id, 
  f.Termin_Bestätigt1 as Produktionstermin, 
  Anzahl, 
  a.[Artikel-Nr], 
  a.Artikelnummer, 
  Bemerkung, 
  case when ISNULL(f.FA_Gestartet, 0) = 0 Then 'Nicht Gestartet' 
  /*without color*/
  when f.Termin_Bestätigt2 <= GETDATE() Then 'Werktermin im Rückstand' 
  /*red */
  when f.Termin_Bestätigt2 <= f.Termin_Bestätigt1 Then 'Werktermin Ok' 
  /*green */
  else 'Werktermin zu spät' 
  /*orange*/
  end as Status
FROM 
  Fertigung f 
  LEFT JOIN Artikel a on a.[Artikel-Nr] = f.Artikel_Nr WHERE 
  F.Kennzeichen = 'offen' 
AND a.Artikelnummer   NOT LIKE 'Reparatur%'
  AND a.Artikelnummer NOT LIKE 'Technik%' 
  AND a.Artikelnummer NOT LIKE 'Analyse%' 
{(dateTo.HasValue ? $" AND F.Termin_Bestätigt1 <= '{dateTo.Value.AddDays(-2 * 7).ToString("yyyyMMdd")}'" : "")}
{(dateFrom.HasValue ? $" AND F.Termin_Bestätigt1 >= '{dateFrom.Value.AddDays(-2 * 7).ToString("yyyyMMdd")}'" : "")}
) d where d.Status ='{productionStatus}' ORDER BY Produktionstermin ASC";

						if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
						{
							query += $" ,{sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
						}

						if(paging != null)
						{
							query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
						}

					}

					sqlCommand.CommandText = query;
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.FaProductionStatusEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.FaProductionStatusEntity>();
			}
		}

		public static int count_By_Search_Value(string searchValue)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"SELECT COUNT(DISTINCT f.ID) as CountNR FROM 
	( SELECT ID, MIN(Flag) Flag FROM ( SELECT ID, 2 AS Flag from fertigung /* Check */ WHERE Kennzeichen='offen'  /*AND ISNULL(FA_Gestartet,0)=0*/ 
		AND CAST(Termin_Bestätigt2 AS DATE)>CAST(Termin_Bestätigt1 AS DATE)
		UNION ALL SELECT ID, 0 AS Flag from fertigung /* Escalation */ WHERE Kennzeichen='offen' /*AND ISNULL(FA_Gestartet,0)=0 */
		AND CAST(Termin_Bestätigt1 AS DATE)<=CAST(GETDATE() AS DATE)
		UNION ALL SELECT ID, 1 AS Flag from fertigung /* Preventive */ WHERE Kennzeichen='offen' AND ISNULL(FA_Gestartet,0)=0 
		AND CAST(Termin_Bestätigt1 AS DATE)>=CAST(GETDATE() AS DATE) AND CAST(Termin_Bestätigt1 AS DATE)<=CAST((GETDATE()+17) AS DATE)
		/*UNION ALL SELECT ID, 3 AS Flag from fertigung  /* OnTime */ WHERE Kennzeichen='offen' AND ISNULL(FA_Gestartet,0)=1 AND 
		DATEPART(WEEK, CAST(Termin_Bestätigt1 AS DATE))= DATEPART(WEEK,CAST(Termin_Bestätigt2 AS DATE)) */) AS FlagGroup GROUP BY ID) AS T
		JOIN (SELECT ID, Termin_Bestätigt1 AS Produktionstermin, Termin_Bestätigt2 AS Werktermin, Fertigungsnummer, Lagerort_id, Artikel_Nr, 
		Anzahl, CAST(Bemerkung as nvarchar(250)) AS Bemerkung  FROM Fertigung) AS f ON f.ID=T.ID
		JOIN Artikel a on a.[Artikel-Nr]=f.Artikel_Nr";

				using(var sqlCommand = new SqlCommand())
				{
					bool isFirstCondition = true;

					if(!string.IsNullOrWhiteSpace(searchValue))
					{
						query += $" {(isFirstCondition ? " WHERE " : " AND ")} [Fertigungsnummer] LIKE '%{searchValue.Trim()}%' OR [Bemerkung] LIKE '%{searchValue.Trim()}%' OR [Artikelnummer] LIKE '{searchValue.Trim()}%'";

						isFirstCondition = false;
					}
					sqlCommand.CommandText = query;
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count == 0)
			{
				return 0;
			}

			return Convert.ToInt32(dataTable.Rows[0]["CountNR"]);
		}
		#endregion Capa
	}
}
