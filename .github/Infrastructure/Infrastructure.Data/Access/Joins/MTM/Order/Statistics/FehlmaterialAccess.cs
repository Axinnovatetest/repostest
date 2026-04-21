using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Joins.MTM.Order.Statistics
{
	public class FehlmaterialAccess
	{
		public static List<Infrastructure.Data.Entities.Joins.MTM.Order.Statistics.FehlmaterialEntity> GetSiteFehlamaterial(int site, int fa, DateTime date)
		{
			var place1 = "";
			var place = "";

			if(site == 1 || site == 11)
			{
				place1 = "AND ((Fertigung.Lagerort_id)=7 )";
				place = "(Lager.Lagerort_id) = 7 Or (Lager.Lagerort_id) = 10 Or (Lager.Lagerort_id) = 23 Or (Lager.Lagerort_id) = 29 Or (Lager.Lagerort_id) = 30 Or (Lager.Lagerort_id) = 56)";
			}
			else if(site == 2 || site == 21)
			{

				place1 = "AND ((Fertigung.Lagerort_id)=42 )";
				place = "(Lager.Lagerort_id) = 40 Or (Lager.Lagerort_id) = 42 Or (Lager.Lagerort_id) = 46 Or (Lager.Lagerort_id) = 47 Or (Lager.Lagerort_id) = 49 Or (Lager.Lagerort_id) = 57)";

			}
			else if(site == 3 || site == 31)
			{
				place1 = " AND ((Fertigung.Lagerort_id)=6 Or (Fertigung.Lagerort_id)=3 Or (Fertigung.Lagerort_id)=20 Or (Fertigung.Lagerort_id)=21)";
				place = "(Lager.Lagerort_id) = 3 Or (Lager.Lagerort_id) = 6 Or (Lager.Lagerort_id) = 9 Or (Lager.Lagerort_id) = 52)";

			}
			else if(site == 4 || site == 41)
			{
				place1 = "AND ((Fertigung.Lagerort_id)=24 Or (Fertigung.Lagerort_id)=26 )";
				place = "(Lager.Lagerort_id) = 24 Or (Lager.Lagerort_id) = 25 Or (Lager.Lagerort_id) = 26 Or (Lager.Lagerort_id) = 34 Or (Lager.Lagerort_id) = 35 Or (Lager.Lagerort_id) = 50)";
			}
			else if(site == 6 || site == 61)
			{
				place1 = " AND ((Fertigung.Lagerort_id)=14 Or (Fertigung.Lagerort_id)=15 )";
				place = "(Lager.Lagerort_id) = 14 Or (Lager.Lagerort_id) = 15 Or (Lager.Lagerort_id) = 16 Or (Lager.Lagerort_id) = 22)";
			}
			else if(site == 7 || site == 71)
			{
				place1 = " AND ((Fertigung.Lagerort_id)=60 )";
				place = "(Lager.Lagerort_id) = 59 Or (Lager.Lagerort_id) = 60 Or (Lager.Lagerort_id) = 63 Or (Lager.Lagerort_id) = 64 Or (Lager.Lagerort_id) = 61)";
			}
			else if(site == 8 || site == 81)
			{
				place1 = " AND ((Fertigung.Lagerort_id)=102 Or (Fertigung.Lagerort_id)=101 )";
				place = "(Lager.Lagerort_id) = 101 Or (Lager.Lagerort_id) = 102 Or (Lager.Lagerort_id) = 104 Or (Lager.Lagerort_id) = 105)";

			}



			var _date = $"'{date.ToString("yyyyMMdd")}'";
			//	var _site = $"{(site!=null && site.Count>0? $"AND Fertigung.Lagerort_id IN ({string.Join(",", site)})": "")}";
			//	var _warehouse = $"{(werehouses != null && werehouses.Count > 0 ? $"WHERE Lager.Lagerort_id in ({string.Join(",", werehouses)})" : "")}";
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT
  T5.Termin_Fertigstellung AS T_F,
  T5.Termin_Bestätigt1 AS T_B1,
  T5.Fertigungsnummer AS Fer,
  T5.Anzahl AS Anz,
  T5.Artikelnummer AS Artik_Nr,
  T5.[Bezeichnung 1] AS B1,
  T5.UmsatzCZ AS UmCZ,
  T5.[ProdZeit(h)] AS Prod,
  T7.Artikelnummer AS Artik_Nr2,
  T7.Verfügbar AS Ver,
  T7.SummevonFABedarf AS SummFAB,
  T7.SummevonBestand AS SummBe,
  Artikel.[Bezeichnung 1] AS BZ2
FROM (((SELECT
  Fertigung.Fertigungsnummer,
  Fertigung.Termin_Fertigstellung,
  Fertigung.Termin_Bestätigt1,
  Fertigung.Anzahl,
  Artikel_1.Artikelnummer,
  Artikel_1.[Bezeichnung 1],
  [Betrag] * [Anzahl] AS UmsatzCZ,
  [Produktionszeit] * [Anzahl] / 60 AS [ProdZeit(h)]
FROM (Fertigung
LEFT JOIN Artikel AS Artikel_1
  ON Fertigung.Artikel_Nr = Artikel_1.[Artikel-Nr])
LEFT JOIN artikel_kalkulatorische_kosten
  ON Artikel_1.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]
WHERE (((Fertigung.Termin_Bestätigt1) <= {_date})
AND ((Fertigung.Kennzeichen) = 'offen')
AND ((Fertigung.gebucht) = 1)
AND ((artikel_kalkulatorische_kosten.Kostenart) = 'Arbeitskosten')
{place1}/*AND ((Fertigung.Lagerort_id) = 7)*/
)) AS T5
LEFT JOIN (SELECT
  Stücklisten.Artikelnummer,
  T1.Termin_Fertigstellung,
  T1.Fertigungsnummer,
  (T1.Anzahl * Stücklisten.Anzahl) AS FABedarf
FROM (SELECT
  Fertigung.Fertigungsnummer,
  Fertigung.Termin_Fertigstellung,
  Fertigung.Termin_Bestätigt1,
  Fertigung.Anzahl,
  Artikel_1.Artikelnummer,
  Artikel_1.[Bezeichnung 1],
  [Betrag] * [Anzahl] AS UmsatzCZ,
  [Produktionszeit] * [Anzahl] / 60 AS [ProdZeit(h)]
FROM (Fertigung
LEFT JOIN Artikel AS Artikel_1
  ON Fertigung.Artikel_Nr = Artikel_1.[Artikel-Nr])
LEFT JOIN artikel_kalkulatorische_kosten
  ON Artikel_1.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]
WHERE (((Fertigung.Termin_Bestätigt1) <= {_date})
AND ((Fertigung.Kennzeichen) = 'offen')
AND ((Fertigung.gebucht) = 1)
AND ((artikel_kalkulatorische_kosten.Kostenart) = 'Arbeitskosten')
{place1}/*AND ((Fertigung.Lagerort_id) = 7)*/)) AS T1
INNER JOIN (Artikel
INNER JOIN Stücklisten
  ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr])
  ON T1.Artikelnummer = Artikel.Artikelnummer) AS T6
  ON T5.Fertigungsnummer = T6.Fertigungsnummer)
LEFT JOIN (SELECT
  T3.SummevonFABedarf,
  T4.SummevonBestand,
  [SummevonBestand] - [SummevonFABedarf] AS Verfügbar,
  T3.Artikelnummer
FROM (SELECT
  T2.Artikelnummer,
  SUM(T2.FABedarf) AS SummevonFABedarf
FROM (SELECT
  Stücklisten.Artikelnummer,
  T1.Termin_Fertigstellung,
  T1.Fertigungsnummer,
  (T1.Anzahl * Stücklisten.Anzahl) AS FABedarf
FROM (SELECT
  Fertigung.Fertigungsnummer,
  Fertigung.Termin_Fertigstellung,
  Fertigung.Termin_Bestätigt1,
  Fertigung.Anzahl,
  Artikel_1.Artikelnummer,
  Artikel_1.[Bezeichnung 1],
  [Betrag] * [Anzahl] AS UmsatzCZ,
  [Produktionszeit] * [Anzahl] / 60 AS [ProdZeit(h)]
FROM (Fertigung
LEFT JOIN Artikel AS Artikel_1
  ON Fertigung.Artikel_Nr = Artikel_1.[Artikel-Nr])
LEFT JOIN artikel_kalkulatorische_kosten
  ON Artikel_1.[Artikel-Nr] = artikel_kalkulatorische_kosten.[artikel-nr]
WHERE (((Fertigung.Termin_Bestätigt1) <= {_date})
AND ((Fertigung.Kennzeichen) = 'offen')
AND ((Fertigung.gebucht) = 1)
AND ((artikel_kalkulatorische_kosten.Kostenart) = 'Arbeitskosten')
{place1}/*AND ((Fertigung.Lagerort_id) = 7)*/)) AS T1
INNER JOIN (Artikel
INNER JOIN Stücklisten
  ON Artikel.[Artikel-Nr] = Stücklisten.[Artikel-Nr])
  ON T1.Artikelnummer = Artikel.Artikelnummer) AS T2
GROUP BY T2.Artikelnummer) AS T3
INNER JOIN (SELECT
  SUM(Lager.Bestand) AS SummevonBestand,
  Artikel.Artikelnummer
FROM Lager
INNER JOIN Artikel
  ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr]
WHERE ({place}/*((Lager.Lagerort_id) = 7
OR (Lager.Lagerort_id) = 10
OR (Lager.Lagerort_id) = 23
OR (Lager.Lagerort_id) = 29
OR (Lager.Lagerort_id) = 30
OR (Lager.Lagerort_id) = 56))*/
GROUP BY Artikel.Artikelnummer,
         Lager.[Artikel-Nr]) AS T4
  ON T3.Artikelnummer = T4.Artikelnummer
WHERE ((([SummevonBestand] - [SummevonFABedarf]) < 0))) AS T7
  ON T6.Artikelnummer = T7.Artikelnummer)
LEFT JOIN Artikel
  ON T7.Artikelnummer = Artikel.Artikelnummer
GROUP BY T5.Termin_Fertigstellung,
         T5.Termin_Bestätigt1,
         T5.Fertigungsnummer,
         T5.Anzahl,
         T5.Artikelnummer,
         T5.[Bezeichnung 1],
         T5.UmsatzCZ,
         T5.[ProdZeit(h)],
         T7.Artikelnummer,
         T7.Verfügbar,
         T7.SummevonFABedarf,
         T7.SummevonBestand,
         Artikel.[Bezeichnung 1]
HAVING (((T5.Fertigungsnummer) = {fa})
AND T7.Artikelnummer IS NOT NULL)
ORDER BY T5.Termin_Fertigstellung, T5.Fertigungsnummer, T5.Artikelnummer;";


				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MTM.Order.Statistics.FehlmaterialEntity(x)).ToList();
			}
			else
			{ return null; }
		}
	}
}
