using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Joins.MTM.Order.Statistics
{
	public class FehlerMaterialFAAccess
	{
		public static List<Infrastructure.Data.Entities.Joins.MTM.Order.Statistics.FehlermaterialFAEntity> GetFehlermaterialFA(string article, decimal menge, string lagerort, DateTime date)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT Artikel.[Artikel-Nr], T5.Materialnummer AS Artikelnummer, T5.[Bezeichnung 1], T5.Bestand, T5.[SummevonBedarf Materialnummer] 
AS [Bedarf Gesamt], T5.Verfügbar, adressen.Name1, Bestellnummern.Einkaufspreis, T5.Lagerort FROM
((
SELECT T4.Materialnummer, Artikel.[Bezeichnung 1], Lager.Bestand, T4.[SummevonBedarf Materialnummer], Lager.Bestand-T4.[SummevonBedarf Materialnummer] 
AS Verfügbar, Lagerorte.Lagerort, @menge AS [Neue Menge] FROM 
((
SELECT T3.Materialnummer as Materialnummer ,sum(T3.[Bedarf Materialnummer] ) as [SummevonBedarf Materialnummer] FROM
(
SELECT T2.Stücklisten_Artikelnummer AS Materialnummer, Stücklisten.Anzahl*Fertigung.Anzahl AS [Bedarf Materialnummer] FROM
((
SELECT T1.Art_F  ,T1.Art_N_B as [Artikel-Nr des Bauteils],T1. Art_D as Stücklisten_Artikelnummer,T1.St_d,T1. Anz,T1.Eben FROM
(
select A1.Artikelnummer as Art_F,S1.[Artikel-Nr des Bauteils] as Art_N_B,S1.Artikelnummer as Art_D,A2.Stückliste as St_d,s1.Anzahl as Anz,
1 as Eben from Artikel A1,Stücklisten S1,Artikel A2
WHERE
A1.[Artikel-Nr] = S1.[Artikel-Nr]
and S1.[Artikel-Nr des Bauteils]=A2.[Artikel-Nr]
and A1.Artikelnummer=@article
union all 
select A1.Artikelnummer as Art_F,S2.[Artikel-Nr des Bauteils] as Art_N_B,S2.Artikelnummer as Art_D,A3.Stückliste St_d,s2.Anzahl as Anz, 1 as Eben
from Artikel A1,Stücklisten S1,Artikel A2,Artikel A3,Stücklisten S2
where
A1.[Artikel-Nr] = S1.[Artikel-Nr]
and S1.[Artikel-Nr des Bauteils]=A2.[Artikel-Nr]
and A2.[Artikel-Nr]=S2.[Artikel-Nr]
and S2.[Artikel-Nr des Bauteils]=A3.[Artikel-Nr]
and A1.Artikelnummer=@article  and A1.Stückliste=1
union all
select A1.Artikelnummer as Art_F,S3.[Artikel-Nr des Bauteils] as Art_N_B,S3.Artikelnummer as Art_D,A4.Stückliste St_d,s3.Anzahl as Anz, 1 as Eben
from Artikel A1,Stücklisten S1,Artikel A2,Artikel A3,Stücklisten S2,Artikel A4, Stücklisten S3
where
A1.[Artikel-Nr] = S1.[Artikel-Nr]
and S1.[Artikel-Nr des Bauteils]=A2.[Artikel-Nr]
and A2.[Artikel-Nr]=S2.[Artikel-Nr]
and S2.[Artikel-Nr des Bauteils]=A3.[Artikel-Nr]
and A3.[Artikel-Nr]=S3.[Artikel-Nr]
and S3.[Artikel-Nr des Bauteils]=A4.[Artikel-Nr]
and A1.Artikelnummer=@article and A1.Stückliste=1 and A2.Stückliste=1 ) as T1
)T2
INNER JOIN (Fertigung INNER JOIN Stücklisten ON Fertigung.Artikel_Nr = Stücklisten.[Artikel-Nr]) ON T2.[Artikel-Nr des Bauteils] = Stücklisten.[Artikel-Nr des Bauteils]) 
INNER JOIN Lagerorte ON Fertigung.Lagerort_id = Lagerorte.Lagerort_id
GROUP BY T2.Stücklisten_Artikelnummer, Stücklisten.Anzahl*Fertigung.Anzahl, Stücklisten.Anzahl, Lagerorte.Lagerort, Fertigung.Fertigungsnummer,
Fertigung.Anzahl, Fertigung.Kennzeichen, Fertigung.Termin_Bestätigt1
HAVING (((Lagerorte.Lagerort)=@lagerort) AND ((Fertigung.Kennzeichen)='Offen') AND ((Fertigung.Termin_Bestätigt1)=@date))
union all
(
select S1.Artikelnummer as Materialnummer,s1.Anzahl* @menge AS [Bedarf Materialnummer] from Artikel A1,Stücklisten S1,Artikel A2
where
A1.[Artikel-Nr] = S1.[Artikel-Nr]
and S1.[Artikel-Nr des Bauteils]=A2.[Artikel-Nr]
and A1.Artikelnummer=@article
union all
select S2.Artikelnummer as Materialnummer,s2.Anzahl* @menge AS [Bedarf Materialnummer]
from Artikel A1,Stücklisten S1,Artikel A2,Artikel A3,Stücklisten S2
where
A1.[Artikel-Nr] = S1.[Artikel-Nr]
and S1.[Artikel-Nr des Bauteils]=A2.[Artikel-Nr]
and A2.[Artikel-Nr]=S2.[Artikel-Nr]
and S2.[Artikel-Nr des Bauteils]=A3.[Artikel-Nr]
and A1.Artikelnummer=@article
union all
select S3.Artikelnummer as Materialnummer,s3.Anzahl* @menge AS [Bedarf Materialnummer]
from Artikel A1,Stücklisten S1,Artikel A2,Artikel A3,Stücklisten S2,Artikel A4, Stücklisten S3
where
A1.[Artikel-Nr] = S1.[Artikel-Nr]
and S1.[Artikel-Nr des Bauteils]=A2.[Artikel-Nr]
and A2.[Artikel-Nr]=S2.[Artikel-Nr]
and S2.[Artikel-Nr des Bauteils]=A3.[Artikel-Nr]
and A3.[Artikel-Nr]=S3.[Artikel-Nr]
and S3.[Artikel-Nr des Bauteils]=A4.[Artikel-Nr]
and A1.Artikelnummer=@article)
) as T3
Group BY
T3.Materialnummer
) as T4
LEFT JOIN (Artikel LEFT JOIN Lager ON Artikel.[Artikel-Nr] = Lager.[Artikel-Nr]) ON T4.Materialnummer = Artikel.Artikelnummer) LEFT JOIN Lagerorte ON Lager.Lagerort_id = Lagerorte.Lagerort_id
GROUP BY T4.Materialnummer, Artikel.[Bezeichnung 1], Lager.Bestand, T4.[SummevonBedarf Materialnummer], Lager.Bestand-T4.[SummevonBedarf Materialnummer], Lagerorte.Lagerort
HAVING (((Lagerorte.Lagerort)=@lagerort))
) as T5
INNER JOIN (Bestellnummern INNER JOIN Artikel ON Bestellnummern.[Artikel-Nr] = Artikel.[Artikel-Nr]) ON T5.Materialnummer = Artikel.Artikelnummer) INNER JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr
WHERE (((Bestellnummern.Standardlieferant) = 1))
GROUP BY Artikel.[Artikel-Nr], T5.Materialnummer, T5.[Bezeichnung 1], T5.Bestand, T5.[SummevonBedarf Materialnummer], T5.Verfügbar, adressen.Name1, Bestellnummern.Einkaufspreis, T5.Lagerort
ORDER BY T5.Verfügbar;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("article", article);
				sqlCommand.Parameters.AddWithValue("menge", menge);
				sqlCommand.Parameters.AddWithValue("lagerort", lagerort);
				sqlCommand.Parameters.AddWithValue("date", date);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.FehlermaterialFAEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.FehlermaterialFAEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.MTM.Order.Statistics.FAVerschiebungEntity> GetFAVerschiebung(int lager, int period)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT Fertigung.ID,Fertigung.Fertigungsnummer, Fertigung.Lagerort_id, Fertigung.Termin_Ursprünglich, Auswertung_Fertigung_Historie_01.Termin_voränderung,
Fertigung.Termin_Bestätigt1, Fertigung.Kennzeichen, Auswertung_Fertigung_Historie_01.Änderungsdatum,
IIf(Auswertung_Fertigung_Historie_01.Termin_voränderung Is Null,
Fertigung.Termin_Bestätigt1-Fertigung.Termin_Ursprünglich,Fertigung.Termin_Bestätigt1-Auswertung_Fertigung_Historie_01.Termin_voränderung) 
AS Zeitraum 
FROM 
(
SELECT [PSZ_Fertigungsauftrag Änderungshistorie].Fertigungsnummer, Max([PSZ_Fertigungsauftrag Änderungshistorie].ID) AS MaxvonID, 
[PSZ_Fertigungsauftrag Änderungshistorie].Termin_voränderung, [PSZ_Fertigungsauftrag Änderungshistorie].Ursprünglicher_termin,
[PSZ_Fertigungsauftrag Änderungshistorie].Termin_Bestätigt1, [PSZ_Fertigungsauftrag Änderungshistorie].Änderungsdatum
FROM [PSZ_Fertigungsauftrag Änderungshistorie]
GROUP BY [PSZ_Fertigungsauftrag Änderungshistorie].Fertigungsnummer, [PSZ_Fertigungsauftrag Änderungshistorie].Termin_voränderung,
[PSZ_Fertigungsauftrag Änderungshistorie].Ursprünglicher_termin, [PSZ_Fertigungsauftrag Änderungshistorie].Termin_Bestätigt1,
[PSZ_Fertigungsauftrag Änderungshistorie].Änderungsdatum
) Auswertung_Fertigung_Historie_01
INNER JOIN Fertigung 
ON Auswertung_Fertigung_Historie_01.Fertigungsnummer = Fertigung.Fertigungsnummer
WHERE Fertigung.Kennzeichen='Offen' AND Fertigung.Lagerort_id=@lager AND IIf(Auswertung_Fertigung_Historie_01.Termin_voränderung Is Null,
Fertigung.Termin_Bestätigt1-Fertigung.Termin_Ursprünglich,Fertigung.Termin_Bestätigt1-Auswertung_Fertigung_Historie_01.Termin_voränderung) >=@period
ORDER BY Fertigung.Fertigungsnummer";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("lager", lager);
				sqlCommand.Parameters.AddWithValue("period", period);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.FAVerschiebungEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.FAVerschiebungEntity>();
			}
		}
	}
}
