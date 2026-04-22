using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;



namespace Infrastructure.Data.Access.Joins.MTM.Order
{
	public class NeededQuantityInFasAccess
	{

		public static List<Entities.Joins.MTM.Order.NeededQuantityEntity> GetNeededQuantityInFas4(List<int?> LagersList, int Months, int ArtikelNr)
		{
			if(LagersList is null || LagersList.Count <= 0)
			{
				return new List<Entities.Joins.MTM.Order.NeededQuantityEntity>();
			}

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string artikleNr_Sub_Filter = "";
				string Lagerort_sub_filter = "";
				if(LagersList is not null && (LagersList.Count > 0))
				{
					string pattern = null;
					foreach(var item in LagersList)
					{
						if(LagersList.IndexOf(item) != 0)
							pattern = pattern + $" , {item}";
						if(LagersList.IndexOf(item) == 0)
							pattern = pattern + $"{item}";
					}
					Lagerort_sub_filter = $"AND FP.Lagerort_ID IN({pattern})";


				}
				if(ArtikelNr > 0)
				{
					artikleNr_Sub_Filter = $"WHERE A.[Artikel-Nr]  = {ArtikelNr}";
				}
				string query =
				@$"SELECT 
					[Week],
					SUM(FPAnzahl / Originalanzahl * FAnzahl) NeedQuantity,
					Artikelnummer
					,Artikel_Nr FROM(
					select
					case WHEN DATEPART(MONTH, Termin_Bestätigt1-28) = 1 AND DATEPART(DAY, Termin_Bestätigt1-28) < 8 AND DATEPART(iso_week , Termin_Bestätigt1-28) <> 1
					THEN CONCAT(DATEPART(iso_week, Termin_Bestätigt1-28) , '/' , DATEPART(YEAR , Termin_Bestätigt1-28) - 1) 
					ELSE CONCAT(DATEPART(iso_week, Termin_Bestätigt1-28) , '/' , DATEPART(YEAR , Termin_Bestätigt1-28)) 
					END AS [Week],

					Artikelnummer
					, FP.[Artikel_Nr]
					,FP.Anzahl FPAnzahl
					, F.Originalanzahl
					,F.Anzahl FAnzahl
					, F.Termin_Bestätigt1 - 28 AS Termin_Bestätigt1 FROM Fertigung F
					INNER JOIN Fertigung_Positionen FP  ON F.ID = FP.ID_Fertigung
						AND(F.Termin_Bestätigt1 <= DATEADD(MONTH , 6 , GETDATE()))
						AND F.Kennzeichen = 'offen' AND IsNULL(F.FA_Gestartet , 0) = 0 AND F.Originalanzahl <> 0 AND F.Originalanzahl IS NOT NULL
						AND F.Anzahl IS NOT NULL {Lagerort_sub_filter}
					INNER JOIN Artikel A ON FP.[Artikel_Nr] = A.[Artikel-Nr]

					{artikleNr_Sub_Filter}
					) s
					Group by
					[Week],
					Artikel_Nr
					,Artikelnummer
					";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.NeededQuantityEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.NeededQuantityEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.MTM.Order.GetFAsInTimeSpanEntity> GetFasInTimeSpan(List<int?> LagersList, int ArtikelNr, DateTime startSpan, DateTime EndSpan)
		{
			if(LagersList is null || LagersList.Count <= 0)
			{
				return new List<Infrastructure.Data.Entities.Joins.MTM.Order.GetFAsInTimeSpanEntity>();
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				string Lagerort_sub_filter = "";
				if(LagersList is not null && (LagersList.Count > 0))
				{
					string pattern = null;
					foreach(var item in LagersList)
					{
						if(LagersList.IndexOf(item) != 0)
							pattern = pattern + $" , {item}";
						if(LagersList.IndexOf(item) == 0)
							pattern = pattern + $"{item}";
					}
					Lagerort_sub_filter = $"AND FP.Lagerort_ID IN({pattern})";
				}

				sqlConnection.Open();
				string query =
				$@"SELECT
					ID,
					Termin_Bestätigt1,
					Fertigungsnummer,
					SUM(FPAnzahl / Originalanzahl * FAnzahl) NeededQuantity,
					Artikel_Nr FROM(
					select
					Artikelnummer
					, F.ID
					, F.Fertigungsnummer
					, FP.[Artikel_Nr]
					, FP.Anzahl FPAnzahl
					, F.Originalanzahl
					, F.Anzahl FAnzahl
					, F.Termin_Bestätigt1 - 28 AS Termin_Bestätigt1 FROM Fertigung F
					INNER JOIN Fertigung_Positionen FP  ON F.ID = FP.ID_Fertigung
						AND (F.Termin_Bestätigt1-28) BETWEEN '{startSpan.ToString("dd/MM/yyyy")}' AND  '{EndSpan.ToString("dd/MM/yyyy")}'
						AND F.Kennzeichen = 'offen' 
						AND IsNULL(F.FA_Gestartet , 0) = 0 
						AND F.Originalanzahl <> 0 AND F.Originalanzahl IS NOT NULL
						AND F.Anzahl IS NOT NULL {Lagerort_sub_filter}
					INNER JOIN Artikel A ON FP.[Artikel_Nr] = A.[Artikel-Nr]

					WHERE A.[Artikel-Nr] = {ArtikelNr}
					) s
					Group by
					ID,
					Termin_Bestätigt1,
					Fertigungsnummer,
					Artikel_Nr";



				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MTM.Order.GetFAsInTimeSpanEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MTM.Order.GetFAsInTimeSpanEntity>();
			}
		}

		#region Need Analyse
		public static List<Infrastructure.Data.Entities.Joins.MTM.Order.CTSNeedEntity> GetCTSNeedAnalysis(DateTime dateTill, bool isExtra, List<int> lagerIds)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"";
				string whereLagers = lagerIds == null || lagerIds.Count <= 0 ? "" : $" WHERE Lagerort_id IN ({string.Join(",", lagerIds)})";

				if(isExtra)
				{
					// - /* --  Analyse Bestand - Bedarf ( zu Viel bestellt ) -- */ //
					query = $@"SELECT t2.*,
						   t1.ROH_Quantity,
						   ISNULL(ROH_Quantity, 0) * Einkaufspreis AS Wert_LagerBestandBedarf,
						   (ROH_Bestand - ISNULL(ROH_Quantity, 0)) DiffQuantity,
						   (ROH_Bestand - ISNULL(ROH_Quantity, 0)) * Einkaufspreis AS DiffPrice
					FROM
					  (SELECT Artikelnummer,
							  SUM(Bestand) AS ROH_Bestand, Einkaufspreis, SUM(Bestand*Einkaufspreis) AS Gesamtpreis,
																			Name1,
																			[Bestell-Nr]
					   FROM
						 (SELECT a.Name1,
								 Bestellnummern.[Bestell-Nr],
								 Artikel.Artikelnummer,
								 Bestellnummern.Standardlieferant,
								 L.Bestand+L.Bestand_reserviert AS Bestand,
								 L.Lagerort_id,
								 Artikel.Warengruppe,
								 Bestellnummern.Einkaufspreis
						  FROM Artikel
						  INNER JOIN (SELECT * FROM Lager{whereLagers}) AS L ON Artikel.[Artikel-Nr] = L.[Artikel-Nr]
						  INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]
						  JOIN adressen a ON a.Nr=Bestellnummern.[Lieferanten-Nr]
						  WHERE (Artikel.Warengruppe <> N'EF')
							AND Bestellnummern.Standardlieferant=1 ) AS Tmp
					   GROUP BY Artikelnummer,
								Einkaufspreis,
								Name1,
								[Bestell-Nr]) t2
					LEFT JOIN
					  (SELECT Artikelnummer,
							  SUM(ROH_quantity) AS ROH_Quantity
					   FROM
						 (SELECT Artikel.Artikelnummer,
								 Fertigung_Positionen.Anzahl PositionAnzahl,
								 F.Kennzeichen,
								 F.FA_Gestartet,
								 F.Fertigungsnummer,
								 F.Originalanzahl,
								 F.Anzahl,
								 Fertigung_Positionen.Anzahl / F.Originalanzahl * F.Anzahl AS ROH_Quantity,
								 F.Lagerort_id
						  FROM (SELECT * FROM Fertigung{whereLagers}) AS F
						  INNER JOIN Fertigung_Positionen ON F.ID = Fertigung_Positionen.ID_Fertigung_HL
						  INNER JOIN Artikel ON Fertigung_Positionen.Artikel_Nr = Artikel.[Artikel-Nr]
						  WHERE (F.Kennzeichen = N'Offen')
							AND (F.Termin_Bestätigt1<='{dateTill.ToString("yyyyMMdd")}') ) AS Tmp
					   GROUP BY Artikelnummer) t1 ON t1.Artikelnummer=t2.Artikelnummer";
				}
				else
				{
					// - /* -- Analyse Bedarf - Bestand (Missing ROH) -- */ //
					query += $@"SELECT t1.*,
								   t2.ROH_Quantity,
								   ISNULL(ROH_Quantity, 0) * Einkaufspreis AS Wert_LagerBestandBedarf,
								   (ROH_Bestand - ISNULL(ROH_Quantity, 0)) DiffQuantity,
								   (ROH_Bestand - ISNULL(ROH_Quantity, 0)) * Einkaufspreis AS DiffPrice
							FROM
							  (SELECT Artikelnummer,
									  SUM(ROH_quantity) AS ROH_Quantity
							   FROM
								 (SELECT Artikel.Artikelnummer,
										 Fertigung_Positionen.Anzahl PositionAnzahl,
										 F.Kennzeichen,
										 F.FA_Gestartet,
										 F.Fertigungsnummer,
										 F.Originalanzahl,
										 F.Anzahl,
										 Fertigung_Positionen.Anzahl / F.Originalanzahl * F.Anzahl AS ROH_Quantity,
										 F.Lagerort_id
								  FROM (SELECT * FROM Fertigung{whereLagers}) AS F
								  INNER JOIN Fertigung_Positionen ON F.ID = Fertigung_Positionen.ID_Fertigung_HL
								  INNER JOIN Artikel ON Fertigung_Positionen.Artikel_Nr = Artikel.[Artikel-Nr]
								  WHERE (F.Kennzeichen = N'Offen')
									AND (F.Termin_Bestätigt1<='{dateTill.ToString("yyyyMMdd")}') ) AS Tmp
							   GROUP BY Artikelnummer) t2
							LEFT JOIN
							  (SELECT Artikelnummer,
									  SUM(Bestand) AS ROH_Bestand, (Einkaufspreis), SUM(Bestand*(Einkaufspreis)) AS Gesamtpreis,
																					Name1,
																					[Bestell-Nr]
							   FROM
								 (SELECT a.Name1,
										 Bestellnummern.[Bestell-Nr],
										 Artikel.Artikelnummer,
										 Bestellnummern.Standardlieferant,
										 L.Bestand+L.Bestand_reserviert AS Bestand,
										 L.Lagerort_id,
										 Artikel.Warengruppe,
										 Bestellnummern.Einkaufspreis
								  FROM Artikel
								  INNER JOIN (SELECT * FROM Lager{whereLagers}) AS L ON Artikel.[Artikel-Nr] = L.[Artikel-Nr]
								  INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]
								  JOIN adressen a ON a.Nr=Bestellnummern.[Lieferanten-Nr]
								  WHERE (Artikel.Warengruppe <> N'EF')
									AND Bestellnummern.Standardlieferant=1 ) AS Tmp
							   GROUP BY Artikelnummer,
										Einkaufspreis,
										Name1,
										[Bestell-Nr]) t1 ON t1.Artikelnummer=t2.Artikelnummer
							WHERE ISNULL((ROH_Bestand - ISNULL(ROH_Quantity, 0)),-1)<0";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MTM.Order.CTSNeedEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MTM.Order.CTSNeedEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.MTM.Order.CTSNeedEntity> GetCTSNeedAnalysis(string filter, DateTime dateTill, bool isExtra, List<int> lagerIds, string sortColumn, bool sortDesc, int currentPage = 0, int pageSize = 100, bool fullData = false)
		{
			filter = (filter ?? "").Trim();
			if(pageSize <= 0)
				pageSize = 1;
			if(string.IsNullOrWhiteSpace(sortColumn))
				sortColumn = "Artikelnummer";

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"";
				string whereLagers = lagerIds == null || lagerIds.Count <= 0 ? "" : $" WHERE Lagerort_id IN ({string.Join(",", lagerIds)})";

				if(isExtra)
				{
					// - /* --  Analyse Bestand - Bedarf ( zu Viel bestellt ) -- */ //
					query = $@"SELECT t2.*,
						   t1.ROH_Quantity,
						   ISNULL(ROH_Quantity, 0) * ISNULL(Einkaufspreis,0) AS Wert_LagerBestandBedarf,
						   (ISNULL(ROH_Bestand,0) - ISNULL(ROH_Quantity, 0)) DiffQuantity,
						   (ISNULL(ROH_Bestand,0) - ISNULL(ROH_Quantity, 0)) * ISNULL(Einkaufspreis,0) AS DiffPrice
					FROM
					  (SELECT Artikelnummer,
							  SUM(ISNULL(Bestand,0)) AS ROH_Bestand, Einkaufspreis, SUM(ISNULL(Bestand,0)*ISNULL(Einkaufspreis,0)) AS Gesamtpreis,
																			Name1,
																			[Bestell-Nr]
					   FROM
						 (SELECT a.Name1,
								 Bestellnummern.[Bestell-Nr],
								 Artikel.Artikelnummer,
								 Bestellnummern.Standardlieferant,
								 ISNULL(L.Bestand,0)+ISNULL(L.Bestand_reserviert,0) AS Bestand,
								 L.Lagerort_id,
								 Artikel.Warengruppe,
								 Bestellnummern.Einkaufspreis
						  FROM Artikel
						  INNER JOIN (SELECT * FROM Lager{whereLagers}) AS L ON Artikel.[Artikel-Nr] = L.[Artikel-Nr]
						  INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]
						  JOIN adressen a ON a.Nr=Bestellnummern.[Lieferanten-Nr]
						  WHERE (Artikel.Warengruppe <> N'EF')
							AND Bestellnummern.Standardlieferant=1 ) AS Tmp
					   GROUP BY Artikelnummer,
								Einkaufspreis,
								Name1,
								[Bestell-Nr]) t2
					LEFT JOIN
					  (SELECT Artikelnummer,
							  SUM(ISNULL(ROH_quantity,0)) AS ROH_Quantity
					   FROM
						 (SELECT Artikel.Artikelnummer,
								 Fertigung_Positionen.Anzahl PositionAnzahl,
								 F.Kennzeichen,
								 F.FA_Gestartet,
								 F.Fertigungsnummer,
								 F.Originalanzahl,
								 F.Anzahl,
								 Fertigung_Positionen.Anzahl / F.Originalanzahl * F.Anzahl AS ROH_Quantity,
								 F.Lagerort_id
						  FROM (SELECT * FROM Fertigung{whereLagers}) AS F
						  INNER JOIN Fertigung_Positionen ON F.ID = Fertigung_Positionen.ID_Fertigung_HL
						  INNER JOIN Artikel ON Fertigung_Positionen.Artikel_Nr = Artikel.[Artikel-Nr]
						  WHERE (F.Kennzeichen = N'Offen')
							AND (F.Termin_Bestätigt1<='{dateTill.ToString("yyyyMMdd")}') ) AS Tmp
					   GROUP BY Artikelnummer) t1 ON t1.Artikelnummer=t2.Artikelnummer{(!string.IsNullOrWhiteSpace(filter) ? $" WHERE (t1.Artikelnummer LIKE '{filter.SqlEscape()}%' OR  t2.Artikelnummer LIKE '{filter.SqlEscape()}%' OR Name1 LIKE '{filter.SqlEscape()}%' OR [Bestell-Nr] LIKE '{filter.SqlEscape()}%')" : "")}";
				}
				else
				{
					// - /* -- Analyse Bedarf - Bestand (Missing ROH) -- */ //
					query += $@"SELECT COALESCE(t1.Artikelnummer, t2.Artikelnummer) Artikelnummer, t1.ROH_Bestand, t1.Einkaufspreis, t1.Gesamtpreis, t1.Name1, t1.[Bestell-Nr],
							t2.ROH_Quantity,
							ISNULL(ROH_Quantity, 0) * ISNULL(Einkaufspreis,0) AS Wert_LagerBestandBedarf,
							(ISNULL(ROH_Bestand,0) - ISNULL(ROH_Quantity, 0)) DiffQuantity,
							(ISNULL(ROH_Bestand,0) - ISNULL(ROH_Quantity, 0)) * ISNULL(Einkaufspreis,0) AS DiffPrice
							FROM
							  (SELECT Artikelnummer,
									  SUM(ISNULL(ROH_quantity,0)) AS ROH_Quantity
							   FROM
								 (SELECT Artikel.Artikelnummer,
										 Fertigung_Positionen.Anzahl PositionAnzahl,
										 F.Kennzeichen,
										 F.FA_Gestartet,
										 F.Fertigungsnummer,
										 F.Originalanzahl,
										 F.Anzahl,
										 Fertigung_Positionen.Anzahl / F.Originalanzahl * F.Anzahl AS ROH_Quantity,
										 F.Lagerort_id
								  FROM (SELECT * FROM Fertigung{whereLagers}) AS F
								  INNER JOIN Fertigung_Positionen ON F.ID = Fertigung_Positionen.ID_Fertigung_HL
								  INNER JOIN Artikel ON Fertigung_Positionen.Artikel_Nr = Artikel.[Artikel-Nr]
								  WHERE (F.Kennzeichen = N'Offen')
									AND (F.Termin_Bestätigt1<='{dateTill.ToString("yyyyMMdd")}') ) AS Tmp
							   GROUP BY Artikelnummer) t2
							LEFT JOIN
							  (SELECT Artikelnummer, SUM(ISNULL(Bestand,0)) AS ROH_Bestand, ISNULL(Einkaufspreis,0) AS Einkaufspreis, SUM(ISNULL(Bestand,0)*ISNULL(Einkaufspreis,0)) AS Gesamtpreis,
																					Name1,
																					[Bestell-Nr]
							   FROM
								 (SELECT a.Name1,
										 Bestellnummern.[Bestell-Nr],
										 Artikel.Artikelnummer,
										 Bestellnummern.Standardlieferant,
										 L.Bestand+L.Bestand_reserviert AS Bestand,
										 L.Lagerort_id,
										 Artikel.Warengruppe,
										 Bestellnummern.Einkaufspreis
								  FROM Artikel
								  INNER JOIN (SELECT * FROM Lager{whereLagers}) AS L ON Artikel.[Artikel-Nr] = L.[Artikel-Nr]
								  INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]
								  JOIN adressen a ON a.Nr=Bestellnummern.[Lieferanten-Nr]
								  WHERE (Artikel.Warengruppe <> N'EF')
									AND Bestellnummern.Standardlieferant=1 ) AS Tmp
							   GROUP BY Artikelnummer,
										Einkaufspreis,
										Name1,
										[Bestell-Nr]) t1 ON t1.Artikelnummer=t2.Artikelnummer
							WHERE ISNULL((ROH_Bestand - ISNULL(ROH_Quantity, 0)),-1)<0{(!string.IsNullOrWhiteSpace(filter) ? $" AND (t1.Artikelnummer LIKE '{filter.SqlEscape()}%' OR  t2.Artikelnummer LIKE '{filter.SqlEscape()}%' OR Name1 LIKE '{filter.SqlEscape()}%' OR [Bestell-Nr] LIKE '{filter.SqlEscape()}%')" : "")}";
				}

				if(!fullData)
				{
					query += $" ORDER BY {sortColumn} {(sortDesc ? "DESC" : "ASC")} OFFSET {currentPage * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 660;
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MTM.Order.CTSNeedEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MTM.Order.CTSNeedEntity>();
			}
		}
		public static int GetCTSNeedAnalysis_count(string filter, DateTime dateTill, bool isExtra, List<int> lagerIds)
		{
			filter = (filter ?? "").Trim();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"";
				string whereLagers = lagerIds == null || lagerIds.Count <= 0 ? "" : $" WHERE Lagerort_id IN ({string.Join(",", lagerIds)})";

				if(isExtra)
				{
					// - /* --  Analyse Bestand - Bedarf ( zu Viel bestellt ) -- */ //
					query = $@"SELECT COUNT(*) Nb
					FROM
					  (SELECT Artikelnummer,
							  SUM(Bestand) AS ROH_Bestand, Einkaufspreis, SUM(Bestand*Einkaufspreis) AS Gesamtpreis,
																			Name1,
																			[Bestell-Nr]
					   FROM
						 (SELECT a.Name1,
								 Bestellnummern.[Bestell-Nr],
								 Artikel.Artikelnummer,
								 Bestellnummern.Standardlieferant,
								 L.Bestand+L.Bestand_reserviert AS Bestand,
								 L.Lagerort_id,
								 Artikel.Warengruppe,
								 Bestellnummern.Einkaufspreis
						  FROM Artikel
						  INNER JOIN (SELECT * FROM Lager{whereLagers}) AS L ON Artikel.[Artikel-Nr] = L.[Artikel-Nr]
						  INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]
						  JOIN adressen a ON a.Nr=Bestellnummern.[Lieferanten-Nr]
						  WHERE (Artikel.Warengruppe <> N'EF')
							AND Bestellnummern.Standardlieferant=1 ) AS Tmp
					   GROUP BY Artikelnummer,
								Einkaufspreis,
								Name1,
								[Bestell-Nr]) t2
					LEFT JOIN
					  (SELECT Artikelnummer,
							  SUM(ROH_quantity) AS ROH_Quantity
					   FROM
						 (SELECT Artikel.Artikelnummer,
								 Fertigung_Positionen.Anzahl PositionAnzahl,
								 F.Kennzeichen,
								 F.FA_Gestartet,
								 F.Fertigungsnummer,
								 F.Originalanzahl,
								 F.Anzahl,
								 Fertigung_Positionen.Anzahl / F.Originalanzahl * F.Anzahl AS ROH_Quantity,
								 F.Lagerort_id
						  FROM (SELECT * FROM Fertigung{whereLagers}) AS F
						  INNER JOIN Fertigung_Positionen ON F.ID = Fertigung_Positionen.ID_Fertigung_HL
						  INNER JOIN Artikel ON Fertigung_Positionen.Artikel_Nr = Artikel.[Artikel-Nr]
						  WHERE (F.Kennzeichen = N'Offen')
							AND (F.Termin_Bestätigt1<='{dateTill.ToString("yyyyMMdd")}') ) AS Tmp
					   GROUP BY Artikelnummer) t1 ON t1.Artikelnummer=t2.Artikelnummer{(!string.IsNullOrWhiteSpace(filter) ? $" WHERE (t1.Artikelnummer LIKE '{filter.SqlEscape()}%' OR  t2.Artikelnummer LIKE '{filter.SqlEscape()}%' OR Name1 LIKE '{filter.SqlEscape()}%' OR [Bestell-Nr] LIKE '{filter.SqlEscape()}%')" : "")}";
				}
				else
				{
					// - /* -- Analyse Bedarf - Bestand (Missing ROH) -- */ //
					query += $@"SELECT COUNT(*) Nb
							FROM
							  (SELECT Artikelnummer,
									  SUM(ROH_quantity) AS ROH_Quantity
							   FROM
								 (SELECT Artikel.Artikelnummer,
										 Fertigung_Positionen.Anzahl PositionAnzahl,
										 F.Kennzeichen,
										 F.FA_Gestartet,
										 F.Fertigungsnummer,
										 F.Originalanzahl,
										 F.Anzahl,
										 Fertigung_Positionen.Anzahl / F.Originalanzahl * F.Anzahl AS ROH_Quantity,
										 F.Lagerort_id
								  FROM (SELECT * FROM Fertigung{whereLagers}) AS F
								  INNER JOIN Fertigung_Positionen ON F.ID = Fertigung_Positionen.ID_Fertigung_HL
								  INNER JOIN Artikel ON Fertigung_Positionen.Artikel_Nr = Artikel.[Artikel-Nr]
								  WHERE (F.Kennzeichen = N'Offen')
									AND (F.Termin_Bestätigt1<='{dateTill.ToString("yyyyMMdd")}') ) AS Tmp
							   GROUP BY Artikelnummer) t2
							LEFT JOIN
							  (SELECT Artikelnummer,
									  SUM(Bestand) AS ROH_Bestand, (Einkaufspreis), SUM(Bestand*(Einkaufspreis)) AS Gesamtpreis,
																					Name1,
																					[Bestell-Nr]
							   FROM
								 (SELECT a.Name1,
										 Bestellnummern.[Bestell-Nr],
										 Artikel.Artikelnummer,
										 Bestellnummern.Standardlieferant,
										 L.Bestand+L.Bestand_reserviert AS Bestand,
										 L.Lagerort_id,
										 Artikel.Warengruppe,
										 Bestellnummern.Einkaufspreis
								  FROM Artikel
								  INNER JOIN (SELECT * FROM Lager{whereLagers}) AS L ON Artikel.[Artikel-Nr] = L.[Artikel-Nr]
								  INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]
								  JOIN adressen a ON a.Nr=Bestellnummern.[Lieferanten-Nr]
								  WHERE (Artikel.Warengruppe <> N'EF')
									AND Bestellnummern.Standardlieferant=1 ) AS Tmp
							   GROUP BY Artikelnummer,
										Einkaufspreis,
										Name1,
										[Bestell-Nr]) t1 ON t1.Artikelnummer=t2.Artikelnummer
							WHERE ISNULL((ROH_Bestand - ISNULL(ROH_Quantity, 0)),-1)<0{(!string.IsNullOrWhiteSpace(filter) ? $" AND (t1.Artikelnummer LIKE '{filter.SqlEscape()}%' OR  t2.Artikelnummer LIKE '{filter.SqlEscape()}%' OR Name1 LIKE '{filter.SqlEscape()}%' OR [Bestell-Nr] LIKE '{filter.SqlEscape()}%')" : "")}";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
			}
		}
		public static Infrastructure.Data.Entities.Joins.MTM.Order.CTSNeedSummaryEntity GetCTSNeedAnalysisSummary(DateTime dateTill, List<int> lagerIds)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"";
				string whereLagers = lagerIds == null || lagerIds.Count <= 0 ? "" : $" WHERE Lagerort_id IN ({string.Join(",", lagerIds)})";

				// - /* --  Analyse Bestand - Bedarf ( zu Viel bestellt ) -- */ //
				query = $@"SELECT SUM (t2.Gesamtpreis) TotalAmount, SUM(ISNULL(ROH_Quantity, 0) * ISNULL(Einkaufspreis,0)) PeriodAmount
					FROM
					  (SELECT Artikelnummer,
							  SUM(Bestand) AS ROH_Bestand, Einkaufspreis, SUM(Bestand*Einkaufspreis) AS Gesamtpreis,
																			Name1,
																			[Bestell-Nr]
					   FROM
						 (SELECT a.Name1,
								 Bestellnummern.[Bestell-Nr],
								 Artikel.Artikelnummer,
								 Bestellnummern.Standardlieferant,
								 L.Bestand+L.Bestand_reserviert AS Bestand,
								 L.Lagerort_id,
								 Artikel.Warengruppe,
								 Bestellnummern.Einkaufspreis
						  FROM Artikel
						  INNER JOIN (SELECT * FROM Lager{whereLagers}) AS L ON Artikel.[Artikel-Nr] = L.[Artikel-Nr]
						  INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]
						  JOIN adressen a ON a.Nr=Bestellnummern.[Lieferanten-Nr]
						  WHERE (Artikel.Warengruppe <> N'EF')
							AND Bestellnummern.Standardlieferant=1 ) AS Tmp
					   GROUP BY Artikelnummer,
								Einkaufspreis,
								Name1,
								[Bestell-Nr]) t2
					LEFT JOIN
					  (SELECT Artikelnummer,
							  SUM(ROH_quantity) AS ROH_Quantity
					   FROM
						 (SELECT Artikel.Artikelnummer,
								 Fertigung_Positionen.Anzahl PositionAnzahl,
								 F.Kennzeichen,
								 F.FA_Gestartet,
								 F.Fertigungsnummer,
								 F.Originalanzahl,
								 F.Anzahl,
								 Fertigung_Positionen.Anzahl / F.Originalanzahl * F.Anzahl AS ROH_Quantity,
								 F.Lagerort_id
						  FROM (SELECT * FROM Fertigung{whereLagers}) AS F
						  INNER JOIN Fertigung_Positionen ON F.ID = Fertigung_Positionen.ID_Fertigung_HL
						  INNER JOIN Artikel ON Fertigung_Positionen.Artikel_Nr = Artikel.[Artikel-Nr]
						  WHERE (F.Kennzeichen = N'Offen')
							AND (F.Termin_Bestätigt1<='{dateTill.ToString("yyyyMMdd")}') ) AS Tmp
					   GROUP BY Artikelnummer) t1 ON t1.Artikelnummer=t2.Artikelnummer";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Joins.MTM.Order.CTSNeedSummaryEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.MTM.Order.CTSNeedSummaryEntity> GetCTSNeedAnalysisSummary(List<DateTime> dateTill, List<int> lagerIds)
		{
			if(dateTill == null || dateTill.Count <= 0)
			{
				return new List<Entities.Joins.MTM.Order.CTSNeedSummaryEntity>();
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"";
				string whereLagers = lagerIds == null || lagerIds.Count <= 0 ? "" : $" WHERE Lagerort_id IN ({string.Join(",", lagerIds)})";


				// - /* --  Analyse Bestand - Bedarf ( zu Viel bestellt ) -- */ //

				query = string.Join(" UNION ALL ", dateTill.Select(x => $@"(SELECT SUM (t2.Gesamtpreis) TotalAmount, SUM(ISNULL(ROH_Quantity, 0) * ISNULL(Einkaufspreis,0)) PeriodAmount, DATEPART(ISO_WEEK,'{x.ToString("yyyyMMdd")}') KW, YEAR('{x.ToString("yyyyMMdd")}') [Year]
					FROM
					  (SELECT Artikelnummer,
							  SUM(Bestand) AS ROH_Bestand, Einkaufspreis, SUM(Bestand*Einkaufspreis) AS Gesamtpreis,
																			Name1,
																			[Bestell-Nr]
					   FROM
						 (SELECT a.Name1,
								 Bestellnummern.[Bestell-Nr],
								 Artikel.Artikelnummer,
								 Bestellnummern.Standardlieferant,
								 L.Bestand+L.Bestand_reserviert AS Bestand,
								 L.Lagerort_id,
								 Artikel.Warengruppe,
								 Bestellnummern.Einkaufspreis
						  FROM Artikel
						  INNER JOIN (SELECT * FROM Lager{whereLagers}) AS L ON Artikel.[Artikel-Nr] = L.[Artikel-Nr]
						  INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]
						  JOIN adressen a ON a.Nr=Bestellnummern.[Lieferanten-Nr]
						  WHERE (Artikel.Warengruppe <> N'EF')
							AND Bestellnummern.Standardlieferant=1 ) AS Tmp
					   GROUP BY Artikelnummer,
								Einkaufspreis,
								Name1,
								[Bestell-Nr]) t2
					LEFT JOIN
					  (SELECT Artikelnummer,
							  SUM(ROH_quantity) AS ROH_Quantity
					   FROM
						 (SELECT Artikel.Artikelnummer,
								 Fertigung_Positionen.Anzahl PositionAnzahl,
								 F.Kennzeichen,
								 F.FA_Gestartet,
								 F.Fertigungsnummer,
								 F.Originalanzahl,
								 F.Anzahl,
								 Fertigung_Positionen.Anzahl / F.Originalanzahl * F.Anzahl AS ROH_Quantity,
								 F.Lagerort_id
						  FROM (SELECT * FROM Fertigung{whereLagers}) AS F
						  INNER JOIN Fertigung_Positionen ON F.ID = Fertigung_Positionen.ID_Fertigung_HL
						  INNER JOIN Artikel ON Fertigung_Positionen.Artikel_Nr = Artikel.[Artikel-Nr]
						  WHERE (F.Kennzeichen = N'Offen')
							AND (F.Termin_Bestätigt1<='{x.ToString("yyyyMMdd")}') ) AS Tmp
					   GROUP BY Artikelnummer) t1 ON t1.Artikelnummer=t2.Artikelnummer)"));

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MTM.Order.CTSNeedSummaryEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MTM.Order.CTSNeedSummaryEntity>();
			}
		}
		#endregion Need Analyse
	}
}
