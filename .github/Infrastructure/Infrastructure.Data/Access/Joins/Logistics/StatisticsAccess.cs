using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Joins.Logistics
{
	public class StatisticsAccess
	{
		public static List<Infrastructure.Data.Entities.Joins.Logistics.StatisticsEntity> DownloadExcelBestandlistLagerAccess(int lager)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT   count(*) over () as totalNombre , Lagerorte.Lagerort AS Sklad, Lager.[letzte Bewegung] AS [Poslední Transakce], Artikel.Artikelnummer AS [Cislo PSZ]
								,Lager.Bestand AS Množství, adressen.Name1 , Bestellnummern.[Bestell-Nr] AS [Cislo Vyrobce], ISNULL( Artikel.Kriterium1 ,'') AS [Kontrola ok?], ' ' AS WE_VOH_ID
								FROM (((Lager LEFT JOIN Lagerorte ON Lager.Lagerort_id = Lagerorte.Lagerort_id)
								INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr]) LEFT JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) 
								LEFT JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr
								WHERE (((Lager.Bestand)>0) AND ((Lagerorte.Lagerort_id)=@lager) AND ((Bestellnummern.Standardlieferant)=1))
								ORDER BY Artikel.Artikelnummer ";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("lager", lager);
				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.StatisticsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.StatisticsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.Logistics.StatisticsEntity> GetBestandlistLagerAccess(int lager, Data.Access.Settings.PaginModel paging, Data.Access.Settings.SortingModel sorting, string SearchValue)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT count(*) over () as totalNombre , Lagerorte.Lagerort AS Sklad, Lager.[letzte Bewegung] AS [Poslední Transakce], Artikel.Artikelnummer AS [Cislo PSZ]
								,Lager.Bestand AS Množství, adressen.Name1 , Bestellnummern.[Bestell-Nr] AS [Cislo Vyrobce], ISNULL( Artikel.Kriterium1 ,'') AS [Kontrola ok?], ' ' AS WE_VOH_ID
								FROM (((Lager LEFT JOIN Lagerorte ON Lager.Lagerort_id = Lagerorte.Lagerort_id)
								INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr]) LEFT JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) 
								LEFT JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr
								WHERE (((Lager.Bestand)>0) AND ((Lagerorte.Lagerort_id)=@lager) AND ((Bestellnummern.Standardlieferant)=1))
								 ";
				if(SearchValue != null)
				{
					query += $" and ( CONVERT(VARCHAR, Lager.[letzte Bewegung] ,103)='{string.Format("{0:MM/dd/yyyy}", SearchValue)}' or Artikel.Artikelnummer Like '{SearchValue}%' or Lager.Bestand Like '{SearchValue}%' or adressen.Name1 Like '{SearchValue}%' or Bestellnummern.[Bestell-Nr]  Like '{SearchValue}%' )";
				}
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY Artikel.Artikelnummer ASC ";
				}
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("lager", lager);
				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.StatisticsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.StatisticsEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.Logistics.InventurlisteRohmaterial> GetInventurlisteRohmaterial(decimal Mindestlagerbestand, int Lagerort_id, Data.Access.Settings.PaginModel paging, Data.Access.Settings.SortingModel sorting, string SearchValue)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT count(*) over() as totalRows, Artikel.[Artikel-Nr], Artikel.Artikelnummer,concat( Artikel.[Bezeichnung 1],'|',Artikel.BezeichnungAL) AS [Bezeichnung 1], 
				Lager.Bestand, Lagerorte.Lagerort, 
				IIf(Lager.Lagerort_id<>22,Bestellnummern.Einkaufspreis,0) AS EK, Lager.Bestand* IIf(Lager.Lagerort_id<>22,Bestellnummern.Einkaufspreis,0) AS EK_Summe,
				Artikel.Größe AS Gewicht, [Größe]*[Bestand]/1000 AS Gesamtgewicht, Artikel.Zolltarif_nr, Artikel.Ursprungsland, 
				Bestellnummern.[Lieferanten-Nr], adressen.Name1, Bestellnummern.[Bestell-Nr], Artikel.BezeichnungAL, 
				IIf([Praeferenz_Aktuelles_jahr] Is Null Or [Praeferenz_Aktuelles_jahr]='','NEIN',[Praeferenz_Aktuelles_jahr]) AS Präferenz
				FROM ((((Lager LEFT JOIN Preisgruppen ON Lager.[Artikel-Nr] = Preisgruppen.[Artikel-Nr])
				LEFT JOIN Lagerorte ON Lager.Lagerort_id = Lagerorte.Lagerort_id) INNER JOIN Artikel 
				ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr]) LEFT JOIN Bestellnummern ON Lager.[Artikel-Nr] = Bestellnummern.[Artikel-Nr])
				LEFT JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr
				WHERE (((Lager.Bestand)>=@Mindestlagerbestand) AND ((Lagerorte.Lagerort_id)=@Lagerort_id)
				AND ((Preisgruppen.Preisgruppe)=1)  AND ((Bestellnummern.Standardlieferant)=1) ) ";
				if(SearchValue != null)
				{
					query += $@" and ( Artikel.[Artikel-Nr] Like '{SearchValue}%' or  Artikel.Artikelnummer Like '{SearchValue}%' or concat( Artikel.[Bezeichnung 1] ,'|' , Artikel.BezeichnungAL) Like '{SearchValue}%' or  
				Lager.Bestand Like '{SearchValue}%' or  Lagerorte.Lagerort Like '{SearchValue}%' or  
				IIf(Lager.Lagerort_id<>22 , Bestellnummern.Einkaufspreis , 0)  Like '{SearchValue}%' or  Lager.Bestand* IIf(Lager.Lagerort_id<>22 , Bestellnummern.Einkaufspreis , 0) Like '{SearchValue}%' or 
				Artikel.Größe  Like '{SearchValue}%' or  [Größe]*[Bestand]/1000  Like '{SearchValue}%' or  Artikel.Zolltarif_nr Like '{SearchValue}%' or  Artikel.Ursprungsland Like '{SearchValue}%' or  
				Bestellnummern.[Lieferanten-Nr] Like '{SearchValue}%' or  adressen.Name1 Like '{SearchValue}%' or  Bestellnummern.[Bestell-Nr] Like '{SearchValue}%' or  Artikel.BezeichnungAL Like '{SearchValue}%' or  
				IIf([Praeferenz_Aktuelles_jahr] Is Null Or [Praeferenz_Aktuelles_jahr]='' , 'NEIN' ,  [Praeferenz_Aktuelles_jahr]) Like '{SearchValue}%' )";
				}
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY Artikel.Artikelnummer ASC ";
				}
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Mindestlagerbestand", Mindestlagerbestand);
				sqlCommand.Parameters.AddWithValue("Lagerort_id", Lagerort_id);
				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.InventurlisteRohmaterial(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.InventurlisteRohmaterial>();
			}
		}


		public static List<Infrastructure.Data.Entities.Joins.Logistics.InventurlisteEF> GetInventurlisteEF(decimal Mindestlagerbestand, int Lagerort_id, Data.Access.Settings.PaginModel paging, Data.Access.Settings.SortingModel sorting, string SearchValue)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT count(*) over() as totalRows, Artikel.Artikelnummer, Artikel.[Bezeichnung 1], Lager.Bestand
				FROM Artikel INNER JOIN Lager ON Artikel.[Artikel-Nr] = Lager.[Artikel-Nr]
				WHERE (((Lager.Bestand)>=@Mindestlagerbestand) AND ((Artikel.Warengruppe)='EF') AND ((Lager.Lagerort_id)=@Lagerort_id)) ";
				if(SearchValue != null)
				{
					query += $@" and ( Artikel.Artikelnummer Like '{SearchValue}%' or Artikel.[Bezeichnung 1] Like '{SearchValue}%' or Lager.Bestand Like '{SearchValue}%')";
				}
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY Artikel.Artikelnummer ASC ";
				}
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Mindestlagerbestand", Mindestlagerbestand);
				sqlCommand.Parameters.AddWithValue("Lagerort_id", Lagerort_id);
				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.InventurlisteEF(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.InventurlisteEF>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.Logistics.InventurlistePetraEntity> GetInventurlistePetra(decimal Mindestlagerbestand, int Lagerort_id, Data.Access.Settings.PaginModel paging, Data.Access.Settings.SortingModel sorting, string SearchValue)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT count(*) over() as totalRows, Artikel.[Artikel-Nr], Artikel.Artikelnummer, Artikel.[Bezeichnung 1], Lager.Bestand, Preisgruppen.Verkaufspreis
				FROM (Artikel INNER JOIN Lager ON Artikel.[Artikel-Nr] = Lager.[Artikel-Nr]) LEFT JOIN Preisgruppen ON Artikel.[Artikel-Nr] = Preisgruppen.[Artikel-Nr]
				WHERE (((Artikel.Artikelnummer)<>'Endkontrolle' And (Artikel.Artikelnummer)<>'Reparatur') 
				AND ((Lager.Bestand)>=@Mindestlagerbestand) AND ((Artikel.Stückliste)=1) AND ((Lager.Lagerort_id)=@Lagerort_id)) ";
				if(SearchValue != null)
				{
					query += $@" and ( Artikel.[Artikel-Nr] Like '{SearchValue}%' or  Artikel.Artikelnummer Like '{SearchValue}%' or Artikel.[Bezeichnung 1] Like '{SearchValue}%' or Lager.Bestand Like '{SearchValue}%' or Preisgruppen.Verkaufspreis Like '{SearchValue}%')";
				}
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY Artikel.Artikelnummer ASC ";
				}
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Mindestlagerbestand", Mindestlagerbestand);
				sqlCommand.Parameters.AddWithValue("Lagerort_id", Lagerort_id);
				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.InventurlistePetraEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.InventurlistePetraEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.Logistics.Proforma> GetProformaList(decimal Mindestlagerbestand, int Lagerort_id, Data.Access.Settings.PaginModel paging, Data.Access.Settings.SortingModel sorting, string SearchValue)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT count(*)over()as totalRows,  Artikel.Artikelnummer, Artikel.[Bezeichnung 1], Lager.Bestand, Artikel.Einheit,
					IIf(Lager.Lagerort_id<>22,Bestellnummern.Einkaufspreis,0) AS EK,
					Lager.Bestand*IIf(Lager.Lagerort_id<>22,Bestellnummern.Einkaufspreis,0) AS EK_Summe, Artikel.Größe AS Gewicht, [Größe]*[Bestand]/1000 AS Gesamtgewicht, 
					Artikel.Zolltarif_nr, Artikel.Ursprungsland, adressen.Name1, Artikel.Praeferenz_Aktuelles_jahr, Bestellnummern.Standardlieferant
					FROM ((((Lager LEFT JOIN Preisgruppen ON Lager.[Artikel-Nr] = Preisgruppen.[Artikel-Nr]) 
					LEFT JOIN Lagerorte ON Lager.Lagerort_id = Lagerorte.Lagerort_id) INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr])
					LEFT JOIN Bestellnummern ON Lager.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) LEFT JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr
					WHERE (((Lager.Bestand)>=@Mindestlagerbestand) AND ((Bestellnummern.Standardlieferant)=1)
					AND ((Lagerorte.Lagerort_id)=@Lagerort_id) AND ((Preisgruppen.Preisgruppe)=1)) ";
				if(SearchValue != null)
				{
					query += $@" and (  Artikel.Artikelnummer Like '{SearchValue}%' or   Artikel.[Bezeichnung 1] Like '{SearchValue}%' or   Lager.Bestand Like '{SearchValue}%' or   Artikel.Einheit Like '{SearchValue}%' or  
					IIf(Lager.Lagerort_id<>22,Bestellnummern.Einkaufspreis,0)  Like '{SearchValue}%' or  
					Lager.Bestand*IIf(Lager.Lagerort_id<>22,Bestellnummern.Einkaufspreis,0)  Like '{SearchValue}%' or   Artikel.Größe  Like '{SearchValue}%' or   [Größe]*[Bestand]/1000  Like '{SearchValue}%' or  
					Artikel.Zolltarif_nr Like '{SearchValue}%' or   Artikel.Ursprungsland Like '{SearchValue}%' or   adressen.Name1 Like '{SearchValue}%' or   Artikel.Praeferenz_Aktuelles_jahr Like '{SearchValue}%' or   Bestellnummern.Standardlieferant  Like '{SearchValue}%')";
				}
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY Artikel.Artikelnummer ASC ";
				}
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Mindestlagerbestand", Mindestlagerbestand);
				sqlCommand.Parameters.AddWithValue("Lagerort_id", Lagerort_id);
				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.Proforma(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.Proforma>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.Logistics.PSZArtikelubersichtEinAusTaglichEntity> GetWEnachDatumList(DateTime DateBegin, DateTime DateEnd, Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging, string SearchValue)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT count(*) over() as totalRows, Artikel.Artikelnummer, Bestellungen.Typ, Bestellungen.[Bestellung-Nr], [bestellte Artikel].Anzahl, 
					Bestellungen.Datum, adressen.Name1, 0 AS Lagerplatz_von, [bestellte Artikel].Lagerort_id AS Lagerplatz_nach,
					Bestellnummern.Mindestbestellmenge, Bestellnummern.Verpackungseinheit 
					FROM Bestellnummern RIGHT JOIN (((Artikel INNER JOIN [bestellte Artikel] ON Artikel.[Artikel-Nr] = [bestellte Artikel].[Artikel-Nr])
					INNER JOIN Bestellungen ON [bestellte Artikel].[Bestellung-Nr] = Bestellungen.Nr) INNER JOIN adressen 
					ON Bestellungen.[Lieferanten-Nr] = adressen.Nr) ON Bestellnummern.[Artikel-Nr] = [bestellte Artikel].[Artikel-Nr]
					WHERE (((Bestellungen.Typ)='Wareneingang') AND ((Bestellungen.Datum)>=@DateBegin And (Bestellungen.Datum)<=@DateEnd) 
					AND ((Bestellnummern.Standardlieferant)=1))";
				if(SearchValue != null)
				{
					query += $@" and (  Artikel.Artikelnummer Like '{SearchValue}%' or Bestellungen.Typ Like  '{SearchValue}%'  or Bestellungen.[Bestellung-Nr] Like  '{SearchValue}%'  or  [bestellte Artikel].Anzahl Like  '{SearchValue}%'  or  
					CONVERT(VARCHAR, Bestellungen.Datum ,103)='{string.Format("{0:MM/dd/yyyy}", SearchValue)}' or  adressen.Name1 Like  '{SearchValue}%'  or  [bestellte Artikel].Lagerort_id  Like  '{SearchValue}%'  or 
					Bestellnummern.Mindestbestellmenge Like  '{SearchValue}%'  or  Bestellnummern.Verpackungseinheit Like '{SearchValue}%'   )";
				}
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY Bestellungen.Datum ASC ";
				}
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("DateBegin", DateBegin);
				sqlCommand.Parameters.AddWithValue("DateEnd", DateEnd);
				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.PSZArtikelubersichtEinAusTaglichEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.PSZArtikelubersichtEinAusTaglichEntity>();
			}
		}

		public static List<Entities.Joins.Logistics.DraftInventoryListEntity> GetDraftInventory(int lagerOrt, Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging, string SearchValue)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT count(*) over() as totalRows, Artikel.[Artikel-Nr], Artikel.Artikelnummer, CONCAT (Artikel.[Bezeichnung 1] 
								, ' | '  , Artikel.BezeichnungAL) AS [Bezeichnung 1],
								Lagerorte.Lagerort_id AS [Storage-ID], Lager.Bestand AS [Quantity P3000],
								'' AS [Inventur Quantity], '' AS Difference, Lager.[letzte Bewegung], Lager.CCID_Datum
								FROM (Lager LEFT JOIN Lagerorte ON Lager.Lagerort_id = Lagerorte.Lagerort_id) INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr]
								WHERE (((Lagerorte.Lagerort_id)=@lagerOrt) AND ((Lager.Bestand)<>0))";
				if(SearchValue != null)
				{
					query += $@" and ( Artikel.[Artikel-Nr] Like '{SearchValue}%' or Artikel.Artikelnummer Like '{SearchValue}%' or CONCAT (Artikel.[Bezeichnung 1] 
								, ' | '  , Artikel.BezeichnungAL) Like '{SearchValue}%' or
								Lagerorte.Lagerort_id Like '{SearchValue}%' or Lager.Bestand Like '{SearchValue}%' or
								Lager.[letzte Bewegung] Like '{SearchValue}%' or Lager.CCID_Datum Like '{SearchValue}%' )";
				}
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY Artikel.Artikelnummer ASC ";
				}
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("lagerOrt", lagerOrt);
				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.DraftInventoryListEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.DraftInventoryListEntity>();
			}
		}

		public static List<Entities.Joins.Logistics.BestandSperrlagerListReportEntity> GetBestandSperrlagerReport()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Artikel.Artikelnummer, Artikel.[Bezeichnung 1], Lager.Bestand, Lager.Lagerort_id
				FROM Lager INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr]
				GROUP BY Artikel.Artikelnummer, Artikel.[Bezeichnung 1], Lager.Bestand, Lager.Lagerort_id
				HAVING (((Lager.Bestand)<>0) AND ((Lager.Lagerort_id)=91))";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.BestandSperrlagerListReportEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.BestandSperrlagerListReportEntity>();
			}
		}
		public static List<Entities.Joins.Logistics.CableWithoutOrderEntity> GetCableWithoutOrder(int Do)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Lager.Bestand, Artikel.Artikelnummer, Artikel.[Bezeichnung 1]
				FROM (Lager LEFT JOIN 
				(
					SELECT PSZ_Kabel_Bedarfsanalyse_I.[Artikel-Nr], PSZ_Kabel_Bedarfsanalyse_I.Artikelnummer, PSZ_Kabel_Bedarfsanalyse_I.Bestand
					FROM (
							(
							SELECT Artikel.Artikelnummer, Artikel.[Bezeichnung 1], Lager.Lagerort_id, Lager.Bestand, Artikel.[Artikel-Nr]
							FROM Artikel INNER JOIN Lager ON Artikel.[Artikel-Nr] = Lager.[Artikel-Nr]
							WHERE (((Artikel.Artikelnummer) Like '22%') AND ((Lager.Lagerort_id)=6) AND ((Lager.Bestand)>10))

							)
							PSZ_Kabel_Bedarfsanalyse_I INNER JOIN Stücklisten ON 
							PSZ_Kabel_Bedarfsanalyse_I.[Artikel-Nr] = Stücklisten.[Artikel-Nr des Bauteils]) 
							INNER JOIN Fertigung ON Stücklisten.[Artikel-Nr] = Fertigung.Artikel_Nr
							WHERE (((Fertigung.Lagerort_id)=6) AND ((Fertigung.gebucht)=1) AND ((Fertigung.Kennzeichen)='Offen') 
							AND ((Fertigung.Termin_Bestätigt1)<=GetDate()+@Do))
							GROUP BY PSZ_Kabel_Bedarfsanalyse_I.[Artikel-Nr], PSZ_Kabel_Bedarfsanalyse_I.Artikelnummer, PSZ_Kabel_Bedarfsanalyse_I.Bestand

							)
				PSZ_Kabel_Bedarfsanalyse_II ON Lager.[Artikel-Nr] = PSZ_Kabel_Bedarfsanalyse_II.[Artikel-Nr]) LEFT JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr]
				WHERE (((Lager.Bestand)>10) AND ((Artikel.Artikelnummer) Like '22%' ) AND ((Lager.Lagerort_id)=6) AND ((PSZ_Kabel_Bedarfsanalyse_II.[Artikel-Nr]) Is Null))
				ORDER BY Lager.Bestand DESC;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Do", Do);
				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.CableWithoutOrderEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.CableWithoutOrderEntity>();
			}
		}

		//ROHOhneBedarfEntity
		public static List<Entities.Joins.Logistics.ROHOhneBedarfEntity> GetROHOhneBedarfEntity(int lagerOrt, int Days, Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT  Lager.Bestand, Artikel.Artikelnummer, Artikel.[Bezeichnung 1],isnull(T.Einkaufspreis,0)as Einkaufspreis
								FROM (Lager INNER JOIN (SELECT [Analyse_STK-5M].Artikelnummer, Analyse_FA_STK_Zukunft.Artikel_Nr, [Analyse_STK-5M].[Artikel-Nr des Bauteils]
								FROM (SELECT DISTINCT Fertigung_Positionen.Artikel_Nr FROM Fertigung INNER JOIN Fertigung_Positionen ON Fertigung.ID = Fertigung_Positionen.ID_Fertigung
								WHERE (((Fertigung.Kennzeichen)='Offen') AND ((Fertigung.Lagerort_id)=@lagerOrt))
								GROUP BY Fertigung_Positionen.Artikel_Nr )as Analyse_FA_STK_Zukunft RIGHT JOIN 
								(select T1.[Artikel-Nr des Bauteils],T1.Artikelnummer from( SELECT Stücklisten.[Artikel-Nr des Bauteils], Artikel.Artikelnummer
								FROM (SELECT Angebote.Typ, Artikel.Artikelnummer, MAX(Angebote.Datum) AS MaxvonDatum, Artikel.Warengruppe, [angebotene Artikel].[Artikel-Nr] 
								FROM Angebote INNER JOIN [angebotene Artikel] INNER JOIN
								Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr] ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]
								GROUP BY Angebote.Typ, Artikel.Artikelnummer, Artikel.Warengruppe, [angebotene Artikel].[Artikel-Nr]
								HAVING (Angebote.Typ = 'Rechnung') AND (MAX(Angebote.Datum) <= GETDATE()- @Days ) AND (Artikel.Warengruppe = 'EF'))
								AS Analyse_FG_5M_T_1 INNER JOIN Stücklisten ON Analyse_FG_5M_T_1.[Artikel-Nr] = Stücklisten.[Artikel-Nr] INNER JOIN
								Artikel ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel.[Artikel-Nr]
								GROUP BY Stücklisten.[Artikel-Nr des Bauteils], Artikel.Artikelnummer 
								) as T1 left join 
								(
								SELECT     S1.[Artikel-Nr des Bauteils], Artikel.Artikelnummer 
								FROM (SELECT Angebote.Typ, Artikel.Artikelnummer, MAX(Angebote.Datum) AS MaxvonDatum, Artikel.Warengruppe, [angebotene Artikel].[Artikel-Nr]
								FROM Angebote INNER JOIN  [angebotene Artikel] INNER JOIN
								 Artikel ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr] ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]
								 GROUP BY Angebote.Typ, Artikel.Artikelnummer, Artikel.Warengruppe, [angebotene Artikel].[Artikel-Nr]
								 HAVING (Angebote.Typ = 'Rechnung') AND (MAX(Angebote.Datum) > GETDATE()-@Days) AND (Artikel.Warengruppe = 'EF'))
								 AS Analyse_FG_5M_T_1 INNER JOIN Stücklisten S1 ON Analyse_FG_5M_T_1.[Artikel-Nr] = S1.[Artikel-Nr] INNER JOIN
								 Artikel ON S1.[Artikel-Nr des Bauteils] = Artikel.[Artikel-Nr]GROUP BY S1.[Artikel-Nr des Bauteils],
								 Artikel.Artikelnummer)as T2 on T1.Artikelnummer=T2.Artikelnummer where T2.Artikelnummer is null)
								 as [Analyse_STK-5M] ON Analyse_FA_STK_Zukunft.Artikel_Nr = [Analyse_STK-5M].[Artikel-Nr des Bauteils]
								WHERE (((Analyse_FA_STK_Zukunft.Artikel_Nr) Is Null)))Analyse_Artikel_Nicht_benötigt ON 
								Lager.[Artikel-Nr] = Analyse_Artikel_Nicht_benötigt.[Artikel-Nr des Bauteils]) 
								INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr]
								Left Join (select Bestellnummern.[Artikel-Nr],Bestellnummern.Einkaufspreis from Bestellnummern inner join
								(SELECT Bestellnummern.[Artikel-Nr] FROM Bestellnummern GROUP BY Bestellnummern.[Artikel-Nr]
								HAVING (((Count([Nr]))=1)))T1 on Bestellnummern.[Artikel-Nr]=T1.[Artikel-Nr]  Union all select Bestellnummern.[Artikel-Nr],
								Bestellnummern.Einkaufspreis from Bestellnummern inner join (SELECT E.[Artikel-Nr], Count(E.Nr) AS Ausdr1
								FROM Bestellnummern E  GROUP BY E.[Artikel-Nr] HAVING (((Count(E.Nr))>1)))T2 on Bestellnummern.[Artikel-Nr]=T2.[Artikel-Nr]
								where Bestellnummern.Standardlieferant <> 0 )T on T.[Artikel-Nr]= Lager.[Artikel-Nr]
								WHERE (((Lager.Lagerort_id)=@lagerOrt) AND ((Lager.Bestand)>0))";
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY Einkaufspreis DESC";
				}
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("lagerOrt", lagerOrt);
				sqlCommand.Parameters.AddWithValue("Days", Days);
				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.ROHOhneBedarfEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.ROHOhneBedarfEntity>();
			}
		}

		public static List<Entities.Joins.Logistics.ScannerRohmaterialEntity> GetScannerRohmaterialEntity(DateTime From, DateTime To, Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging, string SearchValue, int? SearchLager)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Tbl_Versand_ROH.*
									FROM Tbl_Versand_ROH
									WHERE 
									(((Tbl_Versand_ROH.Scanndatum)>=@From 
									And (Tbl_Versand_ROH.Scanndatum)<= DATEADD(DAY, 1, @To )))
									";
				if(SearchLager != null)
				{
					query = query + $@"And Transferlager = {SearchLager}";
				}
				if(SearchValue != null)
				{
					query = query + $@"	And (Artikelnummer like '%{SearchValue}%' or Lagerplatz_pos like '%{SearchValue}%' or Menge like '%{SearchValue}%' or CONVERT(VARCHAR, Scanndatum ,103)='{string.Format("{0:MM/dd/yyyy}", SearchValue)}')";
				}
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " order by Transferlager ASC, Lagerplatz_pos ASC, CAST(Datum AS NVARCHAR(100)) ASC";
				}
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("From", From);
				sqlCommand.Parameters.AddWithValue("To", To);
				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.ScannerRohmaterialEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.ScannerRohmaterialEntity>();
			}
		}
		public static int DeleteScannerRohmaterial(int IdVersand, SqlConnection conncection, SqlTransaction transaction)
		{
			int results = -1;
			//using (var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			//{
			//sqlConnection.Open();
			string query = "DELETE FROM [Tbl_Versand_ROH] WHERE [IdVersand]=@IdVersand";
			var sqlCommand = new SqlCommand(query, conncection, transaction);
			sqlCommand.Parameters.AddWithValue("IdVersand", IdVersand);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			//}

			return results;
		}
		//Excess Rohmaterial
		public static List<Entities.Joins.Logistics.ExcessRohmaterialEntity> GetExcessRohmaterialEntity(int Tage, int lager, Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging, string SearchValue)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"/* SELECT count(*) over () as totalRows , [PSZ_Excess Material Tabelle].[Artikel-Nr], [PSZ_Excess Material Tabelle].Artikelnummer, 
					[PSZ_Excess Material Tabelle].[Bezeichnung 1], [PSZ_Excess Material Tabelle].SummevonBestand,
					[PSZ_Excess Material Tabelle].Einkaufspreis, [PSZ_Excess Material Tabelle].SummevonBestand*[PSZ_Excess Material Tabelle].Einkaufspreis AS Kosten
					FROM 
					(
					SELECT Artikel.[Artikel-Nr], Artikel.Artikelnummer, Artikel.[Bezeichnung 1], 
					Sum(Lager.Bestand) AS SummevonBestand, Bestellnummern.Einkaufspreis 
					FROM (Artikel INNER JOIN Lager ON Artikel.[Artikel-Nr] = Lager.[Artikel-Nr]) INNER JOIN Bestellnummern ON Lager.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]
					WHERE (((Artikel.Stückliste)=0) AND ((Artikel.Lagerartikel)=1) AND ((Bestellnummern.Standardlieferant)=1))
					GROUP BY Artikel.[Artikel-Nr], Artikel.Artikelnummer, Artikel.[Bezeichnung 1], Bestellnummern.Einkaufspreis, Lager.Lagerort_id
					HAVING (((Artikel.Artikelnummer) Not Like 'ALB%' And (Artikel.Artikelnummer) Not Like '227%') AND ((Sum(Lager.Bestand))>0) AND 
					((Lager.Lagerort_id)=@lager))
					)
					as 
					[PSZ_Excess Material Tabelle] LEFT JOIN 
					(
					SELECT [PSZ_Excessmaterial bewegte Artikel].[Artikel-nr]
					FROM 
					(
					SELECT Lagerbewegungen_Artikel.[Artikel-nr], Lagerbewegungen_Artikel.[Bezeichnung 1], Lagerbewegungen.Datum, 
					Lagerbewegungen.Typ 
					FROM Lagerbewegungen INNER JOIN Lagerbewegungen_Artikel ON Lagerbewegungen.ID = Lagerbewegungen_Artikel.Lagerbewegungen_id
					GROUP BY Lagerbewegungen_Artikel.[Artikel-nr], Lagerbewegungen_Artikel.[Bezeichnung 1], Lagerbewegungen.Datum, Lagerbewegungen.Typ
					HAVING (((Lagerbewegungen_Artikel.[Artikel-nr])>0) AND ((Lagerbewegungen.Datum)>GetDate()-@Tage))
					union all 
					SELECT [bestellte Artikel].[Artikel-Nr], [bestellte Artikel].[Bezeichnung 1], Bestellungen.Datum, Bestellungen.Typ
					FROM Bestellungen INNER JOIN [bestellte Artikel] ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
					GROUP BY [bestellte Artikel].[Artikel-Nr], [bestellte Artikel].[Bezeichnung 1], Bestellungen.Datum, Bestellungen.Typ
					HAVING (((Bestellungen.Datum)>GetDate()-90) AND ((Bestellungen.Typ)='Wareneingang'))
					union all
					SELECT Fertigung_Fertigungsvorgang.Artikel_nr,  Artikel.[Bezeichnung 1],Fertigung_Fertigungsvorgang.Datum, 'Fertigung' AS Type
					FROM Fertigung_Fertigungsvorgang INNER JOIN Artikel ON Fertigung_Fertigungsvorgang.Artikel_nr = Artikel.[Artikel-Nr]
					WHERE (((Fertigung_Fertigungsvorgang.ab_buchen)=1))
					GROUP BY Fertigung_Fertigungsvorgang.Artikel_nr, Fertigung_Fertigungsvorgang.Datum, Artikel.[Bezeichnung 1]
					HAVING (((Fertigung_Fertigungsvorgang.Datum)>GetDate()-@Tage))
					)as
					[PSZ_Excessmaterial bewegte Artikel]
					GROUP BY [PSZ_Excessmaterial bewegte Artikel].[Artikel-nr]
					)as
					[PSZ_Excessmaterial 03 bewegte Artikel gruppieren] ON
					[PSZ_Excess Material Tabelle].[Artikel-Nr] = [PSZ_Excessmaterial 03 bewegte Artikel gruppieren].[Artikel-nr]
					WHERE ((([PSZ_Excessmaterial 03 bewegte Artikel gruppieren].[Artikel-nr]) Is Null))*/
					

					WITH ArticleDetails AS (
						SELECT a.[Artikel-Nr], a.Artikelnummer, a.[Bezeichnung 1], SUM(l.Bestand) AS SummevonBestand, bn.Einkaufspreis
						FROM 
							(SELECT [Artikel-Nr], Artikelnummer, [Bezeichnung 1] 
							 FROM Artikel 
							 WHERE Artikelnummer NOT LIKE 'ALB%' AND Artikelnummer NOT LIKE '227%' AND Stückliste = 0 AND Lagerartikel = 1
							) a
						INNER JOIN 
							(SELECT [Artikel-Nr], Lagerort_id, SUM(Bestand) AS Bestand 
							 FROM Lager 
							 WHERE Lagerort_id = @lager 
							 GROUP BY [Artikel-Nr], Lagerort_id 
							 HAVING SUM(Bestand) > 0
							) l ON a.[Artikel-Nr] = l.[Artikel-Nr]
						INNER JOIN 
							(SELECT [Artikel-Nr], Einkaufspreis 
							 FROM Bestellnummern 
							 WHERE Standardlieferant = 1
							) bn ON a.[Artikel-Nr] = bn.[Artikel-Nr]
						GROUP BY a.[Artikel-Nr], a.Artikelnummer, a.[Bezeichnung 1],  bn.Einkaufspreis
					),
					RecentMovements AS (
						SELECT DISTINCT la.[Artikel-nr]
						FROM Lagerbewegungen lb
						INNER JOIN Lagerbewegungen_Artikel la ON lb.ID = la.Lagerbewegungen_id
						WHERE la.[Artikel-nr] > 0 AND lb.Datum > DATEADD(DAY, -@Tage, GETDATE())

						UNION ALL 

						SELECT DISTINCT ba.[Artikel-Nr]
						FROM Bestellungen b
						INNER JOIN [bestellte Artikel] ba ON b.Nr = ba.[Bestellung-Nr]
						WHERE b.Datum > DATEADD(DAY, -90, GETDATE()) AND b.Typ = 'Wareneingang'

						UNION ALL

						SELECT DISTINCT f.Artikel_nr
						FROM Fertigung_Fertigungsvorgang f
						INNER JOIN Artikel a ON f.Artikel_nr = a.[Artikel-Nr]
						WHERE f.ab_buchen = 1 AND f.Datum > DATEADD(DAY, -@Tage, GETDATE())
					)
					SELECT count(*) over () as totalRows,
						ad.[Artikel-Nr], 
						ad.Artikelnummer, 
						ad.[Bezeichnung 1], 
						ad.SummevonBestand, 
						ad.Einkaufspreis, 
						ad.SummevonBestand * ad.Einkaufspreis AS Kosten
					FROM ArticleDetails ad
					LEFT JOIN RecentMovements rm ON ad.[Artikel-Nr] = rm.[Artikel-nr]
					WHERE rm.[Artikel-nr] IS NULL
";
				if(SearchValue != null)
				{
					query += $@" and (ad.[Artikel-Nr] like '{SearchValue}%' or  ad.Artikelnummer like '{SearchValue}%' or 
					ad.[Bezeichnung 1] like '{SearchValue}%' or ad.SummevonBestand like '{SearchValue}%' or
					ad.Einkaufspreis like '{SearchValue}%' or ad.SummevonBestand * ad.Einkaufspreis  like '{SearchValue}%')";
				}
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += "ORDER BY Kosten DESC";
				}
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Tage", Tage);
				sqlCommand.Parameters.AddWithValue("lager", lager);
				sqlCommand.CommandTimeout = 300;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.ExcessRohmaterialEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.ExcessRohmaterialEntity>();
			}
		}

		public static List<Entities.Joins.Logistics.LieferantMaterialEntity> GetLieferantMaterialEntity()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT adressen.Name1 FROM adressen WHERE adressen.Lieferantennummer<>1";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.LieferantMaterialEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.LieferantMaterialEntity>();
			}
		}
		public static List<Entities.Joins.Logistics.FertigungFGEntity> GetFertigungFGEntity()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Lagerorte.Lagerort FROM Lagerorte WHERE 
								Lagerorte.Lagerort_id in (
								6,15,21,26,42,102
								)";
				//6,7,15,21,26,42,60,102
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.FertigungFGEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.FertigungFGEntity>();
			}
		}

		public static List<Entities.Joins.Logistics.PSZ_PV_ListeEntity> GetPSZ_PV_ListeEntity(int Sendungsnummer, int Lagernummer, Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging, string SearchValue)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT  count(*) over() as totalRows, Artikel.[Artikel-Nr], Artikel.Artikelnummer, Replace(Replace([Bezeichnung 1],CHAR(13),' '),CHAR(10),' ') AS Bezeichnung1, 
					Lager.Bestand, Artikel.Einheit, Lagerorte.Lagerort, IIf(Lager.Lagerort_id<>22,Bestellnummern.Einkaufspreis,0) AS EK, 
					Lager.Bestand*(IIf(Lager.Lagerort_id<>22,Bestellnummern.Einkaufspreis,0)) AS EK_Summe,
					Artikel.Größe AS Gewicht, [Größe]*[Bestand]/1000 AS Gesamtgewicht, Artikel.Zolltarif_nr, Artikel.Ursprungsland, 
					Bestellnummern.[Lieferanten-Nr], adressen.Name1, Bestellnummern.[Bestell-Nr], Artikel.BezeichnungAL, 
					IIf([Praeferenz_Aktuelles_jahr] Is Null Or [Praeferenz_Aktuelles_jahr]='','NEIN',[Praeferenz_Aktuelles_jahr]) AS Präferenz, 
					@Sendungsnummer AS Sendungsnummer
					FROM ((((Lager LEFT JOIN Preisgruppen ON Lager.[Artikel-Nr] = Preisgruppen.[Artikel-Nr]) 
					LEFT JOIN Lagerorte ON Lager.Lagerort_id = Lagerorte.Lagerort_id) INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr]) 
					LEFT JOIN Bestellnummern ON Lager.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) LEFT JOIN adressen ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr
					WHERE (((Lager.Bestand)>='0.1') AND ((Lagerorte.Lagerort_id)=@Lagernummer) AND ((Preisgruppen.Preisgruppe)=1) AND ((Bestellnummern.Standardlieferant)=1))";
				if(SearchValue != null)
				{
					query += $@" and ( Artikel.[Artikel-Nr] Like '{SearchValue}%' or Artikel.Artikelnummer Like '{SearchValue}%' or Replace(Replace([Bezeichnung 1],CHAR(13),' '),CHAR(10),' ')  Like '{SearchValue}%' or  
					Lager.Bestand Like '{SearchValue}%' or  Artikel.Einheit Like '{SearchValue}%' or  Lagerorte.Lagerort Like '{SearchValue}%' or  IIf(Lager.Lagerort_id<>22,Bestellnummern.Einkaufspreis,0)  Like '{SearchValue}%' or 
					Lager.Bestand*(IIf(Lager.Lagerort_id<>22,Bestellnummern.Einkaufspreis,0))  Like '{SearchValue}%' or 
					Artikel.Größe  Like '{SearchValue}%' or  [Größe]*[Bestand]/1000  Like '{SearchValue}%' or  Artikel.Zolltarif_nr Like '{SearchValue}%' or  Artikel.Ursprungsland Like '{SearchValue}%' or  
					Bestellnummern.[Lieferanten-Nr] Like '{SearchValue}%' or  adressen.Name1 Like '{SearchValue}%' or  Bestellnummern.[Bestell-Nr] Like '{SearchValue}%' or  Artikel.BezeichnungAL Like '{SearchValue}%' or  
					IIf([Praeferenz_Aktuelles_jahr] Is Null Or [Praeferenz_Aktuelles_jahr]='','NEIN',[Praeferenz_Aktuelles_jahr])  Like '{SearchValue}%')";
				}
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += "ORDER BY Artikel.Artikelnummer";
				}
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Sendungsnummer", Sendungsnummer);
				sqlCommand.Parameters.AddWithValue("Lagernummer", Lagernummer);
				sqlCommand.CommandTimeout = 180;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.PSZ_PV_ListeEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.PSZ_PV_ListeEntity>();
			}
		}

		public static List<Entities.Joins.Logistics.Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse_Entity> GetPsz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse_Entity(string withFG, string withoutFG, string Lieferant,
			 DateTime bis, string lager, int Lager_Id, int Lager_P_Id, List<int> Lagerorte_Lagerort_id, List<int> bestellte_Artikel_Lagerort_id, List<int> tbl_Planung_gestartet_Lagerort_ID
			, Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging)
		{

			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				var connectionId = sqlConnection.ClientConnectionId.ToString().Replace('-', '_');
				sqlConnection.Open();
				var PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY = $"[##PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY{connectionId}]";
				var PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY = $"[##PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY{connectionId}]";
				var PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY = $"[##PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY{connectionId}]";
				var PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY = $"[##PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY{connectionId}]";

				var PSZ_Disposition_Offene_Materialbestellungen_Umbuchung_TEMPORARY = $"[##PSZ_Disposition_Offene_Materialbestellungen_Umbuchung_TEMPORARY{connectionId}]";
				var Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY = $"[##Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY{connectionId}]";
				var Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table = $"[##Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table{connectionId}]";

				//-----------------------------1------------------
				string query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = $@"
				IF OBJECT_ID('tempdb..{PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}') IS NOT NULL DROP TABLE {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY};

                                    CREATE TABLE {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}(
										[Standardlieferant] [bit] NULL,
										[Bestell-Nr] [nvarchar](250) NULL,
										[Einkaufspreis] [decimal](28,13) NULL,
										[Wiederbeschaffungszeitraum] [smallint] NULL,
										[Mindestbestellmenge] [decimal](28,13) NULL,
										[Artikel-Nr] [int] NULL,
										[Name1] [nvarchar](50) NULL,
										[Telefon] [nvarchar](250) NULL,
										[Fax] [nvarchar](250) NULL,
										[Lagerort_id] [int] NULL,
										[Termin_Bestätigt1] [datetime] NULL,
										[Bruttobedarf] [decimal](28,13) NULL,
										[Termin_Materialbedarf] [datetime] NULL,
										[Fertigungsnummer] [int] NULL,
										[Artikel-Nr des Bauteils] [int] NULL,
										[Bezeichnung des Bauteils] [nvarchar](1000) NULL,
										[Stücklisten_Artikelnummer] [nvarchar](250) NULL,
										[Artikel_Nr] [int] NULL,
										[Termin_Fertigstellung] [datetime] NULL,
										[Bezeichnung 1] [nvarchar](200) NULL,
										[Stücklisten_Anzahl] [decimal](28,13) NULL,
										[Artikel_Artikelnummer] [nvarchar](50) NULL,
										[Fertigung_Anzahl] [int] NULL,
										[Lagerort] [nvarchar](100) NULL
									);

				INSERT INTO {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}
				SELECT UoFilter.Standardlieferant, UoFilter.[Bestell-Nr], UoFilter.Einkaufspreis, 
				UoFilter.Wiederbeschaffungszeitraum, UoFilter.Mindestbestellmenge, UoFilter.[Artikel-Nr], 
				UoFilter.Name1, UoFilter.Telefon, UoFilter.Fax, IIF(UoFilter.Lagerort_id=7,42,UoFilter.Lagerort_id) as Lagerort_id, 
				UoFilter.Termin_Bestätigt1, UoFilter.Bruttobedarf, UoFilter.Termin_Materialbedarf, 
				UoFilter.Fertigungsnummer, UoFilter.[Artikel-Nr des Bauteils], UoFilter.[Bezeichnung des Bauteils], 
				UoFilter.Stücklisten_Artikelnummer, UoFilter.Artikel_Nr, UoFilter.Termin_Fertigstellung, 
				UoFilter.[Bezeichnung 1], UoFilter.Stücklisten_Anzahl, UoFilter.Artikel_Artikelnummer, 
				UoFilter.Fertigung_Anzahl, IIF(UoFilter.Lagerort='TN','WS',UoFilter.Lagerort ) as Lagerort  
				FROM [View_PSZ_Disposition_Bruttobedarfsermittlung Umbuchung] as UoFilter
				WHERE  UoFilter.Termin_Bestätigt1<=@bis AND UoFilter.Name1 Like '{Lieferant}%' ";
				if(withFG != "" && withFG != null)
				{
					query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + $@"
					And UoFilter.Artikel_Artikelnummer Like '{withFG}%'";
				}
				if(withoutFG != "" && withoutFG != null)
				{
					query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + $@"
					AND UoFilter.Artikel_Artikelnummer Not Like '{withoutFG}%'";
				}


				if(lager == "WS")
				{
					query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + $@"
				AND (UoFilter.Lagerort Like '{lager}%' or UoFilter.Lagerort ='TN' ) ";
				}
				else
				{
					query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + $@"
				AND UoFilter.Lagerort Like '{lager}%' ";
				}

				if(lager == "Eigenfertigung")
				{
					query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + $@"INSERT INTO {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}
				SELECT UoFilter.Standardlieferant, UoFilter.[Bestell-Nr], UoFilter.Einkaufspreis, 
				UoFilter.Wiederbeschaffungszeitraum, UoFilter.Mindestbestellmenge, UoFilter.[Artikel-Nr], 
				UoFilter.Name1, UoFilter.Telefon, UoFilter.Fax, UoFilter.Lagerort_id, 
				UoFilter.Termin_Bestätigt1, UoFilter.Bruttobedarf, UoFilter.Termin_Materialbedarf, 
				UoFilter.Fertigungsnummer, UoFilter.[Artikel-Nr des Bauteils], UoFilter.[Bezeichnung des Bauteils], 
				UoFilter.Stücklisten_Artikelnummer, UoFilter.Artikel_Nr, UoFilter.Termin_Fertigstellung, 
				UoFilter.[Bezeichnung 1], UoFilter.Stücklisten_Anzahl, UoFilter.Artikel_Artikelnummer, 
				UoFilter.Fertigung_Anzahl, UoFilter.Lagerort 
				FROM [View_PSZ_Disposition_Bruttobedarfsermittlung Umbuchung] as UoFilter
				WHERE   UoFilter.Termin_Bestätigt1<=@bis AND UoFilter.Name1 Like '{Lieferant}%'  AND (UoFilter.Lagerort Like 'SC%') ";
					if(withFG != "" && withFG != null)
					{
						query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + $@"
					And UoFilter.Artikel_Artikelnummer Like '{withFG}%'";
					}
					if(withoutFG != "" && withoutFG != null)
					{
						query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + $@"
					AND UoFilter.Artikel_Artikelnummer Not Like '{withoutFG}%' ";
					}

				}
				query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + " ; ";
				//------------2
				string query_PSZ_Disposition_Materialbestand_Umbuchung = "";
				if(lager != "Eigenfertigung")
				{
					query_PSZ_Disposition_Materialbestand_Umbuchung = $@"
				IF OBJECT_ID('tempdb..{PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}') IS NOT NULL DROP TABLE {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY};
				      CREATE TABLE {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}(
										[Artikel-Nr] [int] NULL,
										[Bestand] [decimal](28,13)NULL
									);

				INSERT INTO {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}
					SELECT Lager.[Artikel-Nr], Lager.Bestand 
				FROM {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY} INNER JOIN (Lager INNER JOIN Lagerorte ON Lager.Lagerort_id = Lagerorte.Lagerort_id) 
				ON {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}.[Artikel-Nr des Bauteils] = Lager.[Artikel-Nr]
				WHERE ((Lagerorte.Lagerort Like '{lager}%' ))
				GROUP BY Lager.[Artikel-Nr], Lager.Bestand, Lager.Lagerort_id;";
				}
				else
				{
					query_PSZ_Disposition_Materialbestand_Umbuchung = $@"
				IF OBJECT_ID('tempdb..{PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}') IS NOT NULL DROP TABLE {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY};
				      CREATE TABLE {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}(
										[Artikel-Nr] [int] NULL,
										[Bestand] [decimal](28,13)NULL
									);

				INSERT INTO {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}
				SELECT Lager.[Artikel-Nr], Sum(Lager.Bestand) AS Bestand
				FROM Lager INNER JOIN Lagerorte ON Lager.Lagerort_id = Lagerorte.Lagerort_id
				WHERE (((Lager.Lagerort_id)=9 Or (Lager.Lagerort_id)=52 Or (Lager.Lagerort_id)=6 Or (Lager.Lagerort_id)=53 Or (Lager.Lagerort_id)=17))
				GROUP BY Lager.[Artikel-Nr];";
				}

				string query__Reserviert_Menge_Umbuchung = $@"
					IF OBJECT_ID('tempdb..{Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY}') IS NOT NULL DROP TABLE {Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY};
				      CREATE TABLE {Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY}(
										[Artikel_Nr] [int] NULL,
										[Artikelnummer] [nvarchar](200) NULL,
										[Menge_Reserviert] [decimal](28,13) NULL ,
										[Type] [int] NULL,
										[Lagerort_ID] [int] NULL
									);

					INSERT INTO {Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY}
					SELECT tbl_Planung_gestartet.Artikel_Nr AS Artikel_Nr, Artikel.Artikelnummer AS Artikelnummer, Sum(tbl_Planung_gestartet.Menge_reserviert) AS Menge_Reserviert, 
					Artikel.Warentyp AS Type, tbl_Planung_gestartet.Lagerort_ID 
					FROM tbl_Planung_gestartet INNER JOIN Artikel ON tbl_Planung_gestartet.Artikel_Nr = Artikel.[Artikel-Nr]
					WHERE ((

					(tbl_Planung_gestartet.Lagerort_ID) in ({string.Join(',', tbl_Planung_gestartet_Lagerort_ID.ToArray())})
					
					)) OR (((Artikel.Warentyp)<>2 Or 
					(Artikel.Warentyp) Is Null))
					GROUP BY tbl_Planung_gestartet.Artikel_Nr, Artikel.Artikelnummer, Artikel.Warentyp, tbl_Planung_gestartet.Lagerort_ID ;
					";
				string queryUpdate_Bestand_Material = $@"
					
					UPDATE {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}
					SET {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand 
					= {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand+Lager.bestand
					from  {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}
					INNER JOIN 
					(Lager INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr]) 
					 ON Artikel.[Artikel-Nr] ={PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.[Artikel-Nr] 
					WHERE Lager.Lagerort_id={Lager_P_Id} AND Artikel.Warentyp=2 ";
				if(lager != "Eigenfertigung")
				{
					queryUpdate_Bestand_Material = queryUpdate_Bestand_Material + $@"
					UPDATE {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}  
					SET {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand = 
					{PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand+Lager.Bestand
					from {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY} 
					INNER JOIN (select [artikel-nr] as [Artikel-Nr],sum(Bestand) as bestand from lager where Lagerort_id 
					in({string.Join(',', Lagerorte_Lagerort_id.ToArray())})
					group by [artikel-nr])Lager  
					ON {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.[Artikel-Nr] = Lager.[Artikel-Nr] 
					WHERE (((Lager.Bestand)>0)
					) 
					";
				}
				queryUpdate_Bestand_Material = queryUpdate_Bestand_Material + $@"
					UPDATE {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY} 
					SET {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand =
					{PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand-{Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY}.Menge_Reserviert
					from {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}
					INNER JOIN {Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY} 
					ON {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.[Artikel-Nr] = {Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY}.Artikel_Nr 
					WHERE ((({Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY}.Lagerort_ID)={Lager_Id}))  ;";
				//-----------------3--------------
				string query_PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung = $@"

				IF OBJECT_ID('tempdb..{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}') IS NOT NULL DROP TABLE {PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY};
								CREATE TABLE {PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}(
										[Artikel-Nr des Bauteils] [int] NULL,
										[SummevonBruttobedarf] [decimal](28,13)  NULL,
										[Bestand] [decimal](28,13)  NULL
									);
								INSERT INTO {PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY} 
								SELECT {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}.[Artikel-Nr des Bauteils], Sum({PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}.Bruttobedarf)
							AS SummevonBruttobedarf, {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand as  Bestand
								FROM {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY} INNER JOIN {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY} 
							ON {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}.[Artikel-Nr des Bauteils] = {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.[Artikel-Nr]
								GROUP BY {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}.[Artikel-Nr des Bauteils], {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand ;";
				string query_PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung = $@"
								IF OBJECT_ID('tempdb..{PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY}') IS NOT NULL DROP TABLE {PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY};
								CREATE TABLE {PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY}(
										[Artikel-Nr des Bauteils] [int] NULL,
										[SummevonBruttobedarf] [decimal](28,13)  NULL,
										[Verfügbar] [decimal](28,13) NULL,
										[Bestand] [decimal](28,13)  NULL
									);
								INSERT INTO {PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY} 
				SELECT {PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.[Artikel-Nr des Bauteils],
				{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.SummevonBruttobedarf, 
				{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.Bestand-{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.SummevonBruttobedarf AS Verfügbar, 
				{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.Bestand 
				FROM {PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}
				GROUP BY {PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.[Artikel-Nr des Bauteils], {PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.SummevonBruttobedarf,
				{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.Bestand-{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.SummevonBruttobedarf, 
				{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.Bestand;";
				string delete_PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung = $@"
DELETE 
FROM {PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY}
WHERE {PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY}.Verfügbar>=0;";
				//----------4-----------
				string query_PSZ_Disposition_Offene_Materialbestellungen_Umbuchung = $@"
				IF OBJECT_ID('tempdb..{PSZ_Disposition_Offene_Materialbestellungen_Umbuchung_TEMPORARY}') IS NOT NULL DROP TABLE {PSZ_Disposition_Offene_Materialbestellungen_Umbuchung_TEMPORARY};
				CREATE TABLE {PSZ_Disposition_Offene_Materialbestellungen_Umbuchung_TEMPORARY}(
									[bestellte Artikel_Bestellung-Nr] [int] NULL,
									[Artikel-Nr] [int] NULL,
									[Bezeichnung 1] [nvarchar](200) NULL,
									[Bezeichnung 2] [ntext] NULL,
									[Anzahl]  [decimal](28,13) NULL,
									[Liefertermin] [datetime] NULL,
									[erledigt_pos] [bit] NULL,
									[erledigt] [bit] NULL,
									[Bestellungen_Bestellung-Nr] [int] NULL,
									[gebucht] [bit] NULL,
									[Vorname/NameFirma] [nvarchar](100) NULL,
									[Projekt-Nr] [nvarchar](250) NULL,
									[Bestätigter_Termin] [datetime] NULL,
									[Lagerort_id] [int] NULL,
									[Typ] [nvarchar](50) NULL,
									[Datum] [datetime] NULL,
									[AB-Nr_Lieferant] [nvarchar](50) NULL
									);
				INSERT INTO {PSZ_Disposition_Offene_Materialbestellungen_Umbuchung_TEMPORARY}
				SELECT distinct [bestellte Artikel].[Bestellung-Nr]  as [bestellte Artikel_Bestellung-Nr], [bestellte Artikel].[Artikel-Nr], [bestellte Artikel].[Bezeichnung 1], CAST([bestellte Artikel].[Bezeichnung 2] AS NVARCHAR(MAX))
				as [Bezeichnung 2], 
				[bestellte Artikel].Anzahl, [bestellte Artikel].Liefertermin, [bestellte Artikel].erledigt_pos, Bestellungen.erledigt, Bestellungen.[Bestellung-Nr]as [Bestellungen_Bestellung-Nr], 
				Bestellungen.gebucht, Bestellungen.[Vorname/NameFirma], Bestellungen.[Projekt-Nr], [bestellte Artikel].Bestätigter_Termin, [bestellte Artikel].Lagerort_id, 
				Bestellungen.Typ, Bestellungen.Datum, [bestellte Artikel].[AB-Nr_Lieferant] 
				FROM {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY} INNER JOIN ([bestellte Artikel] INNER JOIN Bestellungen ON [bestellte Artikel].[Bestellung-Nr] = Bestellungen.Nr)
				ON {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}.[Artikel-Nr des Bauteils] = [bestellte Artikel].[Artikel-Nr]
				WHERE ((([bestellte Artikel].erledigt_pos)=0) AND ((Bestellungen.erledigt)=0) AND ((Bestellungen.gebucht)=1) AND (
				([bestellte Artikel].Lagerort_id) in ({string.Join(',', bestellte_Artikel_Lagerort_id.ToArray())}) 
				) AND ((Bestellungen.Typ)='Bestellung' Or (Bestellungen.Typ)='Kanbanabruf') AND ((Bestellungen.Datum)>GetDate()-400))
				ORDER BY [bestellte Artikel].[Artikel-Nr];";

				//----------5-------------
				string QueryPsz_Disposition_Nettobedarfsermittlung_Umbuchung_Table = $@"
				IF OBJECT_ID('tempdb..{Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table}') IS NOT NULL DROP TABLE {Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table};
				CREATE TABLE {Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table}(
									[Fertigungsnummer] [int] NULL,
									[Artikel_Artikelnummer] [nvarchar](200) NULL,
									[Bezeichnung 1] [nvarchar](200) NULL,
									[Fertigung_Anzahl] [int] NULL,
									[Termin_Fertigstellung]  [datetime] NULL,
									[Artikel-Nr des Bauteils]  [int] NULL,
									[Stücklisten_Artikelnummer] [nvarchar](250) NULL,
									[Bezeichnung des Bauteils] [nvarchar](1000) NULL,
									[Stücklisten_Anzahl] [decimal](28,13) NULL,
									[Bruttobedarf] [decimal](28,13) NULL,
									[Bestand] [decimal](28,13)  NULL,
									[Artikel-Nr] [int] NULL,
									[Vorname/NameFirma] [nvarchar](100) NULL,
									[Anzahl] [decimal](28,13) NULL,
									[Liefertermin] [datetime]  NULL,
									[Termin_Materialbedarf] [datetime] NULL,
									[Standardlieferant] [bit] NULL,
									[Bestell-Nr] [nvarchar](250) NULL,
									[Einkaufspreis] [decimal](28,13) NULL,
									[Name1] [nvarchar](50) NULL,
									[Telefon] [nvarchar](250) NULL,
									[Fax] [nvarchar](250) NULL,
									[Wiederbeschaffungszeitraum] [smallint] NULL,
									[Mindestbestellmenge] [decimal](28,13) NULL,
									[Lagerort_id] [int] NULL,
									[bestellte Artikel_Bestellung-Nr] [int] NULL,
									[Bestätigter_Termin] [datetime] NULL,
									[Termin_Bestätigt1] [datetime] NULL,
									[Bearbeitet][int] NULL,
									[AB-Nr_Lieferant] [nvarchar](50) NULL,
									[Bestellungen_Bestellung-Nr] [int] NULL
									);
					INSERT INTO {Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table}
					SELECT
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Fertigungsnummer,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Artikel_Artikelnummer,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bezeichnung 1],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Fertigung_Anzahl,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Fertigstellung,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Artikel-Nr des Bauteils],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Stücklisten_Artikelnummer,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bezeichnung des Bauteils],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Stücklisten_Anzahl,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Bruttobedarf,
				   PSZ_Disposition_Materialbestand_Umbuchung.Bestand as Bestand,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[Artikel-Nr],
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[Vorname/NameFirma],
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].Anzahl,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].Liefertermin,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Materialbedarf,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Standardlieferant,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bestell-Nr],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Einkaufspreis,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Name1,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Telefon,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Fax,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Wiederbeschaffungszeitraum,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Mindestbestellmenge,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Lagerort_id,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[bestellte Artikel_Bestellung-Nr],
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].Bestätigter_Termin,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Bestätigt1,
					0 AS Bearbeitet,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[AB-Nr_Lieferant],
					[PSZ_Disposition_Offene Materialbestellungen Umbuchung].[Bestellungen_Bestellung-Nr]
				FROM
				   (
				({PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY}as[PSZ_Disposition_Bruttomaterialbedarf sum m Verfügbark Umbuchung]

					  LEFT JOIN

						{PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY} as PSZ_Disposition_Bruttomaterialbedarf_Umbuchung

						 ON[PSZ_Disposition_Bruttomaterialbedarf sum m Verfügbark Umbuchung].[Artikel-Nr des Bauteils] = PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Artikel-Nr des Bauteils])
					  LEFT JOIN

					   {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY} as PSZ_Disposition_Materialbestand_Umbuchung

						 ON PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Artikel-Nr des Bauteils] = PSZ_Disposition_Materialbestand_Umbuchung.[Artikel-Nr]
				   )
				   LEFT JOIN

					{PSZ_Disposition_Offene_Materialbestellungen_Umbuchung_TEMPORARY} as [PSZ_Disposition_Offene Materialbestellungen Umbuchung]

					  ON PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Artikel-Nr des Bauteils] = [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[Artikel-Nr]
				GROUP BY
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Fertigungsnummer,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Artikel_Artikelnummer,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bezeichnung 1],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Fertigung_Anzahl,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Fertigstellung,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Artikel-Nr des Bauteils],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Stücklisten_Artikelnummer,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bezeichnung des Bauteils],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Stücklisten_Anzahl,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Bruttobedarf,
				   PSZ_Disposition_Materialbestand_Umbuchung.Bestand,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[Artikel-Nr],
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[Vorname/NameFirma],
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].Anzahl,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].Liefertermin,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Materialbedarf,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Standardlieferant,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bestell-Nr],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Einkaufspreis,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Name1,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Telefon,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Fax,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Wiederbeschaffungszeitraum,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Mindestbestellmenge,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Lagerort_id,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[bestellte Artikel_Bestellung-Nr],
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].Bestätigter_Termin,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Bestätigt1,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[AB-Nr_Lieferant],
				[PSZ_Disposition_Offene Materialbestellungen Umbuchung].[Bestellungen_Bestellung-Nr]
				HAVING
				(((PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bezeichnung 1]) <> 'Reparatur'))
				ORDER BY PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Materialbedarf
				";
				//---------------result-----------
				string query_Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse = $@"SELECT [Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse II].Name1, [Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse II].Stücklisten_Artikelnummer,
				[Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse II].[Bezeichnung des Bauteils],
				[Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse II].SummevonBruttobedarf, [Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse II].Lagerort_id,
				Lagerorte.Lagerort, Sum([Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse II].Bestand) AS SummevonBestand,
				[Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse II].MaxvonTermin_Materialbedarf,
				[Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse II].Bestand -[Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse II].SummevonBruttobedarf as Differenz,
				[Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse II].Bearbeitet
				FROM
				(
				SELECT[Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse I].Name1, [Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse I].Stücklisten_Artikelnummer,
				Sum([Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse I].Bruttobedarf) AS SummevonBruttobedarf,
				[Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse I].Bestand,
				Max([Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse I].Termin_Materialbedarf) AS MaxvonTermin_Materialbedarf,
				Artikel.[Bezeichnung 1] AS[Bezeichnung des Bauteils], [Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse I].Bearbeitet,
				[Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse I].Lagerort_id
				FROM
				(
				SELECT[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Stücklisten_Artikelnummer,
				[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Fertigungsnummer, [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Bestand,
				[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Bruttobedarf, [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Termin_Materialbedarf,
				[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Standardlieferant, [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Name1,
				[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Bearbeitet,
				IIf([Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].[Lagerort_Id] = 21, 6, IIf([Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].[Lagerort_id] = 6, 6,
				[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].[Lagerort_id])) AS Lagerort_id
				FROM
				
				{Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table}
				as
				[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table]
				GROUP BY[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Stücklisten_Artikelnummer, [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Fertigungsnummer,
				[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Bestand, [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Bruttobedarf,
				[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Termin_Materialbedarf, [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Standardlieferant,
				[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Name1, [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Bearbeitet,
				IIf([Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].[Lagerort_Id] = 21,6,IIf([Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].[Lagerort_id] = 6,6,
				[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].[Lagerort_id]))
				HAVING((([Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Standardlieferant) = 1))
				)
				as[Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse I] INNER JOIN Artikel
				ON[Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse I].Stücklisten_Artikelnummer = Artikel.Artikelnummer
				GROUP BY[Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse I].Name1, [Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse I].Stücklisten_Artikelnummer,
				[Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse I].Bestand, Artikel.[Bezeichnung 1], [Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse I].Bearbeitet,
				[Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse I].Lagerort_id
				HAVING(((Sum([Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse I].Bruttobedarf)) >[Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse I].[Bestand]))
				)as
				[Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse II] INNER JOIN Lagerorte ON
				[Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse II].Lagerort_id = Lagerorte.Lagerort_id
				GROUP BY[Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse II].Name1,
				[Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse II].Stücklisten_Artikelnummer,
				[Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse II].SummevonBruttobedarf, [Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse II].Lagerort_id,
				[Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse II].MaxvonTermin_Materialbedarf,
				[Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse II].[Bezeichnung des Bauteils],
				[Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse II].Bestand -[Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse II].SummevonBruttobedarf ,
				[Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse II].Bearbeitet, Lagerorte.Lagerort
				HAVING(((Sum([Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse II].Bestand)) <[Psz_Disposition_Nettobedarfsermittlung Umbuchung Analyse II].[SummevonBruttobedarf]));
				";


				// - execute queries
				using(var sqlCommand = new SqlCommand($"BEGIN TRAN ; {query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung};{query_PSZ_Disposition_Materialbestand_Umbuchung};{query__Reserviert_Menge_Umbuchung};{queryUpdate_Bestand_Material};{query_PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung};{query_PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung};{delete_PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung};{query_PSZ_Disposition_Offene_Materialbestellungen_Umbuchung} ;{QueryPsz_Disposition_Nettobedarfsermittlung_Umbuchung_Table};{query_Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse} ; COMMIT TRAN ; ", sqlConnection))
				{
					sqlCommand.Parameters.AddWithValue("bis", bis);
					sqlCommand.CommandTimeout = 0;

					DbExecution.Fill(sqlCommand, dataTable);

				}

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse_Entity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse_Entity>();
			}
		}

		public static List<Entities.Joins.Logistics.PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_II_Entity> GetPsz_Disposition_UmbuchungList_Details_II(string withFG, string withoutFG, string Lieferant,
			 DateTime bis, string lager, int Lager_Id, int Lager_P_Id, List<int> Lagerorte_Lagerort_id, List<int> bestellte_Artikel_Lagerort_id, List<int> tbl_Planung_gestartet_Lagerort_ID
			, string Stucklisten_Artikelnummer, Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging)
		{

			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				var connectionId = sqlConnection.ClientConnectionId.ToString().Replace('-', '_');
				sqlConnection.Open();
				var PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY = $"[##PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_Details_TEMPORARY{connectionId}]";
				var PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY = $"[##PSZ_Disposition_Materialbestand_Umbuchung_Details_TEMPORARY{connectionId}]";
				var PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY = $"[##PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_Details_TEMPORARY{connectionId}]";
				var PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY = $"[##PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY{connectionId}]";

				var PSZ_Disposition_Offene_Materialbestellungen_Umbuchung_TEMPORARY = $"[##PSZ_Disposition_Offene_Materialbestellungen_Umbuchung_Details_TEMPORARY{connectionId}]";
				var Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY = $"[##Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_Details_TEMPORARY{connectionId}]";
				var Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table = $"[##Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Details_Table{connectionId}]";

				//-----------------------------1------------------
				string query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = $@"
				IF OBJECT_ID('tempdb..{PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}') IS NOT NULL DROP TABLE {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY};

                                    CREATE TABLE {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}(
										[Standardlieferant] [bit] NULL,
										[Bestell-Nr] [nvarchar](250) NULL,
										[Einkaufspreis] [decimal](28,13) NULL,
										[Wiederbeschaffungszeitraum] [smallint] NULL,
										[Mindestbestellmenge] [decimal](28,13) NULL,
										[Artikel-Nr] [int] NULL,
										[Name1] [nvarchar](50) NULL,
										[Telefon] [nvarchar](250) NULL,
										[Fax] [nvarchar](250) NULL,
										[Lagerort_id] [int] NULL,
										[Termin_Bestätigt1] [datetime] NULL,
										[Bruttobedarf] [decimal](28,13) NULL,
										[Termin_Materialbedarf] [datetime] NULL,
										[Fertigungsnummer] [int] NULL,
										[Artikel-Nr des Bauteils] [int] NULL,
										[Bezeichnung des Bauteils] [nvarchar](1000) NULL,
										[Stücklisten_Artikelnummer] [nvarchar](250) NULL,
										[Artikel_Nr] [int] NULL,
										[Termin_Fertigstellung] [datetime] NULL,
										[Bezeichnung 1] [nvarchar](200) NULL,
										[Stücklisten_Anzahl] [decimal](28,13) NULL,
										[Artikel_Artikelnummer] [nvarchar](50) NULL,
										[Fertigung_Anzahl] [int] NULL,
										[Lagerort] [nvarchar](100) NULL
									);

				INSERT INTO {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}
				SELECT UoFilter.Standardlieferant, UoFilter.[Bestell-Nr], UoFilter.Einkaufspreis, 
				UoFilter.Wiederbeschaffungszeitraum, UoFilter.Mindestbestellmenge, UoFilter.[Artikel-Nr], 
				UoFilter.Name1, UoFilter.Telefon, UoFilter.Fax, UoFilter.Lagerort_id, 
				UoFilter.Termin_Bestätigt1, UoFilter.Bruttobedarf, UoFilter.Termin_Materialbedarf, 
				UoFilter.Fertigungsnummer, UoFilter.[Artikel-Nr des Bauteils], UoFilter.[Bezeichnung des Bauteils], 
				UoFilter.Stücklisten_Artikelnummer, UoFilter.Artikel_Nr, UoFilter.Termin_Fertigstellung, 
				UoFilter.[Bezeichnung 1], UoFilter.Stücklisten_Anzahl, UoFilter.Artikel_Artikelnummer, 
				UoFilter.Fertigung_Anzahl, UoFilter.Lagerort 
				FROM [View_PSZ_Disposition_Bruttobedarfsermittlung Umbuchung] as UoFilter
				WHERE UoFilter.Termin_Bestätigt1<=@bis AND UoFilter.Name1 Like '{Lieferant}%' ";
				if(withFG != "" && withFG != null)
				{
					query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + $@"
					And UoFilter.Artikel_Artikelnummer Like '{withFG}%'";
				}
				if(withoutFG != "" && withoutFG != null)
				{
					query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + $@"
					AND UoFilter.Artikel_Artikelnummer Not Like '{withoutFG}%'";
				}


				if(lager == "WS")
				{
					query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + $@"
				
				AND (UoFilter.Lagerort Like '{lager}%' or UoFilter.Lagerort ='TN') ";
				}
				else
				{
					query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + $@"
				
				AND UoFilter.Lagerort Like '{lager}%' ";
				}

				if(lager == "Eigenfertigung")
				{
					query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + $@"INSERT INTO {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}
				SELECT UoFilter.Standardlieferant, UoFilter.[Bestell-Nr], UoFilter.Einkaufspreis, 
				UoFilter.Wiederbeschaffungszeitraum, UoFilter.Mindestbestellmenge, UoFilter.[Artikel-Nr], 
				UoFilter.Name1, UoFilter.Telefon, UoFilter.Fax, UoFilter.Lagerort_id, 
				UoFilter.Termin_Bestätigt1, UoFilter.Bruttobedarf, UoFilter.Termin_Materialbedarf, 
				UoFilter.Fertigungsnummer, UoFilter.[Artikel-Nr des Bauteils], UoFilter.[Bezeichnung des Bauteils], 
				UoFilter.Stücklisten_Artikelnummer, UoFilter.Artikel_Nr, UoFilter.Termin_Fertigstellung, 
				UoFilter.[Bezeichnung 1], UoFilter.Stücklisten_Anzahl, UoFilter.Artikel_Artikelnummer, 
				UoFilter.Fertigung_Anzahl, UoFilter.Lagerort 
				FROM [View_PSZ_Disposition_Bruttobedarfsermittlung Umbuchung] as UoFilter
				WHERE  UoFilter.Termin_Bestätigt1<=@bis AND UoFilter.Name1 Like '{Lieferant}%'   AND (UoFilter.Lagerort Like 'SC%') ";
					if(withFG != "" && withFG != null)
					{
						query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + $@"
					And UoFilter.Artikel_Artikelnummer Like '{withFG}%'";
					}
					if(withoutFG != "" && withoutFG != null)
					{
						query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + $@"
					AND UoFilter.Artikel_Artikelnummer Not Like '{withoutFG}%' ";
					}

				}
				query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + " ; ";
				//------------2
				string query_PSZ_Disposition_Materialbestand_Umbuchung = "";
				if(lager != "Eigenfertigung")
				{
					query_PSZ_Disposition_Materialbestand_Umbuchung = $@"
				IF OBJECT_ID('tempdb..{PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}') IS NOT NULL DROP TABLE {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY};
				      CREATE TABLE {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}(
										[Artikel-Nr] [int] NULL,
										[Bestand] [decimal](28,13)NULL
									);

				INSERT INTO {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}
					SELECT Lager.[Artikel-Nr], Lager.Bestand 
				FROM {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY} INNER JOIN (Lager INNER JOIN Lagerorte ON Lager.Lagerort_id = Lagerorte.Lagerort_id) 
				ON {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}.[Artikel-Nr des Bauteils] = Lager.[Artikel-Nr]
				WHERE ((Lagerorte.Lagerort Like '{lager}%' ))
				GROUP BY Lager.[Artikel-Nr], Lager.Bestand, Lager.Lagerort_id;";
				}
				else
				{
					query_PSZ_Disposition_Materialbestand_Umbuchung = $@"
				IF OBJECT_ID('tempdb..{PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}') IS NOT NULL DROP TABLE {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY};
				      CREATE TABLE {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}(
										[Artikel-Nr] [int] NULL,
										[Bestand] [decimal](28,13)NULL
									);

				INSERT INTO {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}
				SELECT Lager.[Artikel-Nr], Sum(Lager.Bestand) AS Bestand
				FROM Lager INNER JOIN Lagerorte ON Lager.Lagerort_id = Lagerorte.Lagerort_id
				WHERE (((Lager.Lagerort_id)=9 Or (Lager.Lagerort_id)=52 Or (Lager.Lagerort_id)=6 Or (Lager.Lagerort_id)=53 Or (Lager.Lagerort_id)=17))
				GROUP BY Lager.[Artikel-Nr];";
				}

				string query__Reserviert_Menge_Umbuchung = $@"
					IF OBJECT_ID('tempdb..{Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY}') IS NOT NULL DROP TABLE {Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY};
				      CREATE TABLE {Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY}(
										[Artikel_Nr] [int] NULL,
										[Artikelnummer] [nvarchar](200) NULL,
										[Menge_Reserviert] [decimal](28,13) NULL ,
										[Type] [int] NULL,
										[Lagerort_ID] [int] NULL
									);

					INSERT INTO {Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY}
					SELECT tbl_Planung_gestartet.Artikel_Nr AS Artikel_Nr, Artikel.Artikelnummer AS Artikelnummer, Sum(tbl_Planung_gestartet.Menge_reserviert) AS Menge_Reserviert, 
					Artikel.Warentyp AS Type, tbl_Planung_gestartet.Lagerort_ID 
					FROM tbl_Planung_gestartet INNER JOIN Artikel ON tbl_Planung_gestartet.Artikel_Nr = Artikel.[Artikel-Nr]
					WHERE ((

					(tbl_Planung_gestartet.Lagerort_ID) in ({string.Join(',', tbl_Planung_gestartet_Lagerort_ID.ToArray())})
					
					)) OR (((Artikel.Warentyp)<>2 Or 
					(Artikel.Warentyp) Is Null))
					GROUP BY tbl_Planung_gestartet.Artikel_Nr, Artikel.Artikelnummer, Artikel.Warentyp, tbl_Planung_gestartet.Lagerort_ID ;
					";
				string queryUpdate_Bestand_Material = $@"
					
					UPDATE {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}
					SET {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand 
					= {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand+Lager.bestand
					from  {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}
					INNER JOIN 
					(Lager INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr]) 
					 ON Artikel.[Artikel-Nr] ={PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.[Artikel-Nr] 
					WHERE Lager.Lagerort_id={Lager_P_Id} AND Artikel.Warentyp=2 ";
				if(lager != "Eigenfertigung")
				{
					queryUpdate_Bestand_Material = queryUpdate_Bestand_Material + $@"
					UPDATE {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}  
					SET {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand = 
					{PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand+Lager.Bestand
					from {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY} 
					INNER JOIN (select [artikel-nr] as [Artikel-Nr],sum(Bestand) as bestand from lager where Lagerort_id in({string.Join(',', Lagerorte_Lagerort_id.ToArray())})
					group by [artikel-nr])Lager  
					ON {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.[Artikel-Nr] = Lager.[Artikel-Nr] 
					WHERE (((Lager.Bestand)>0)
					) 
					";


				}
				queryUpdate_Bestand_Material = queryUpdate_Bestand_Material + $@"
					UPDATE {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY} 
					SET {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand =
					{PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand-{Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY}.Menge_Reserviert
					from {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}
					INNER JOIN {Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY} 
					ON {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.[Artikel-Nr] = {Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY}.Artikel_Nr 
					WHERE ((({Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY}.Lagerort_ID)={Lager_Id}))  ;";
				//-----------------3--------------
				string query_PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung = $@"

				IF OBJECT_ID('tempdb..{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}') IS NOT NULL DROP TABLE {PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY};
								CREATE TABLE {PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}(
										[Artikel-Nr des Bauteils] [int] NULL,
										[SummevonBruttobedarf] [decimal](28,13)  NULL,
										[Bestand] [decimal](28,13)  NULL
									);
								INSERT INTO {PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY} 
								SELECT {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}.[Artikel-Nr des Bauteils], Sum({PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}.Bruttobedarf)
							AS SummevonBruttobedarf, {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand as  Bestand
								FROM {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY} INNER JOIN {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY} 
							ON {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}.[Artikel-Nr des Bauteils] = {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.[Artikel-Nr]
								GROUP BY {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}.[Artikel-Nr des Bauteils], {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand ;";
				string query_PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung = $@"
								IF OBJECT_ID('tempdb..{PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY}') IS NOT NULL DROP TABLE {PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY};
								CREATE TABLE {PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY}(
										[Artikel-Nr des Bauteils] [int] NULL,
										[SummevonBruttobedarf] [decimal](28,13)  NULL,
										[Verfügbar] [decimal](28,13) NULL,
										[Bestand] [decimal](28,13)  NULL
									);
								INSERT INTO {PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY} 
				SELECT {PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.[Artikel-Nr des Bauteils],
				{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.SummevonBruttobedarf, 
				{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.Bestand-{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.SummevonBruttobedarf AS Verfügbar, 
				{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.Bestand 
				FROM {PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}
				GROUP BY {PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.[Artikel-Nr des Bauteils], {PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.SummevonBruttobedarf,
				{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.Bestand-{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.SummevonBruttobedarf, 
				{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.Bestand;";
				string delete_PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung = $@"
DELETE 
FROM {PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY}
WHERE {PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY}.Verfügbar>=0;";
				//----------4-----------
				string query_PSZ_Disposition_Offene_Materialbestellungen_Umbuchung = $@"
				IF OBJECT_ID('tempdb..{PSZ_Disposition_Offene_Materialbestellungen_Umbuchung_TEMPORARY}') IS NOT NULL DROP TABLE {PSZ_Disposition_Offene_Materialbestellungen_Umbuchung_TEMPORARY};
				CREATE TABLE {PSZ_Disposition_Offene_Materialbestellungen_Umbuchung_TEMPORARY}(
									[bestellte Artikel_Bestellung-Nr] [int] NULL,
									[Artikel-Nr] [int] NULL,
									[Bezeichnung 1] [nvarchar](200) NULL,
									[Bezeichnung 2] [ntext] NULL,
									[Anzahl]  [decimal](28,13) NULL,
									[Liefertermin] [datetime] NULL,
									[erledigt_pos] [bit] NULL,
									[erledigt] [bit] NULL,
									[Bestellungen_Bestellung-Nr] [int] NULL,
									[gebucht] [bit] NULL,
									[Vorname/NameFirma] [nvarchar](100) NULL,
									[Projekt-Nr] [nvarchar](250) NULL,
									[Bestätigter_Termin] [datetime] NULL,
									[Lagerort_id] [int] NULL,
									[Typ] [nvarchar](50) NULL,
									[Datum] [datetime] NULL,
									[AB-Nr_Lieferant] [nvarchar](50) NULL
									);
				INSERT INTO {PSZ_Disposition_Offene_Materialbestellungen_Umbuchung_TEMPORARY}
				SELECT distinct [bestellte Artikel].[Bestellung-Nr]  as [bestellte Artikel_Bestellung-Nr], [bestellte Artikel].[Artikel-Nr], [bestellte Artikel].[Bezeichnung 1], CAST([bestellte Artikel].[Bezeichnung 2] AS NVARCHAR(MAX))
				as [Bezeichnung 2], 
				[bestellte Artikel].Anzahl, [bestellte Artikel].Liefertermin, [bestellte Artikel].erledigt_pos, Bestellungen.erledigt, Bestellungen.[Bestellung-Nr]as [Bestellungen_Bestellung-Nr], 
				Bestellungen.gebucht, Bestellungen.[Vorname/NameFirma], Bestellungen.[Projekt-Nr], [bestellte Artikel].Bestätigter_Termin, [bestellte Artikel].Lagerort_id, 
				Bestellungen.Typ, Bestellungen.Datum, [bestellte Artikel].[AB-Nr_Lieferant] 
				FROM {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY} INNER JOIN ([bestellte Artikel] INNER JOIN Bestellungen ON [bestellte Artikel].[Bestellung-Nr] = Bestellungen.Nr)
				ON {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}.[Artikel-Nr des Bauteils] = [bestellte Artikel].[Artikel-Nr]
				WHERE ((([bestellte Artikel].erledigt_pos)=0) AND ((Bestellungen.erledigt)=0) AND ((Bestellungen.gebucht)=1) AND (
				([bestellte Artikel].Lagerort_id) in ({string.Join(',', bestellte_Artikel_Lagerort_id.ToArray())}) 
				) AND ((Bestellungen.Typ)='Bestellung' Or (Bestellungen.Typ)='Kanbanabruf') AND ((Bestellungen.Datum)>GetDate()-400))
				ORDER BY [bestellte Artikel].[Artikel-Nr];";

				//----------5-------------
				string QueryPsz_Disposition_Nettobedarfsermittlung_Umbuchung_Table = $@"
				IF OBJECT_ID('tempdb..{Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table}') IS NOT NULL DROP TABLE {Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table};
				CREATE TABLE {Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table}(
									[Fertigungsnummer] [int] NULL,
									[Artikel_Artikelnummer] [nvarchar](200) NULL,
									[Bezeichnung 1] [nvarchar](200) NULL,
									[Fertigung_Anzahl] [int] NULL,
									[Termin_Fertigstellung]  [datetime] NULL,
									[Artikel-Nr des Bauteils]  [int] NULL,
									[Stücklisten_Artikelnummer] [nvarchar](250) NULL,
									[Bezeichnung des Bauteils] [nvarchar](1000) NULL,
									[Stücklisten_Anzahl] [decimal](28,13) NULL,
									[Bruttobedarf] [decimal](28,13) NULL,
									[Bestand] [decimal](28,13)  NULL,
									[Artikel-Nr] [int] NULL,
									[Vorname/NameFirma] [nvarchar](100) NULL,
									[Anzahl] [decimal](28,13) NULL,
									[Liefertermin] [datetime]  NULL,
									[Termin_Materialbedarf] [datetime] NULL,
									[Standardlieferant] [bit] NULL,
									[Bestell-Nr] [nvarchar](250) NULL,
									[Einkaufspreis] [decimal](28,13) NULL,
									[Name1] [nvarchar](50) NULL,
									[Telefon] [nvarchar](250) NULL,
									[Fax] [nvarchar](250) NULL,
									[Wiederbeschaffungszeitraum] [smallint] NULL,
									[Mindestbestellmenge] [decimal](28,13) NULL,
									[Lagerort_id] [int] NULL,
									[bestellte Artikel_Bestellung-Nr] [int] NULL,
									[Bestätigter_Termin] [datetime] NULL,
									[Termin_Bestätigt1] [datetime] NULL,
									[Bearbeitet][int] NULL,
									[AB-Nr_Lieferant] [nvarchar](50) NULL,
									[Bestellungen_Bestellung-Nr] [int] NULL
									);
					INSERT INTO {Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table}
					SELECT
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Fertigungsnummer,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Artikel_Artikelnummer,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bezeichnung 1],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Fertigung_Anzahl,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Fertigstellung,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Artikel-Nr des Bauteils],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Stücklisten_Artikelnummer,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bezeichnung des Bauteils],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Stücklisten_Anzahl,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Bruttobedarf,
				   PSZ_Disposition_Materialbestand_Umbuchung.Bestand as Bestand,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[Artikel-Nr],
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[Vorname/NameFirma],
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].Anzahl,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].Liefertermin,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Materialbedarf,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Standardlieferant,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bestell-Nr],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Einkaufspreis,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Name1,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Telefon,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Fax,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Wiederbeschaffungszeitraum,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Mindestbestellmenge,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Lagerort_id,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[bestellte Artikel_Bestellung-Nr],
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].Bestätigter_Termin,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Bestätigt1,
					0 AS Bearbeitet,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[AB-Nr_Lieferant],
					[PSZ_Disposition_Offene Materialbestellungen Umbuchung].[Bestellungen_Bestellung-Nr]
				FROM
				   (
				({PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY}as[PSZ_Disposition_Bruttomaterialbedarf sum m Verfügbark Umbuchung]

					  LEFT JOIN

						{PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY} as PSZ_Disposition_Bruttomaterialbedarf_Umbuchung

						 ON[PSZ_Disposition_Bruttomaterialbedarf sum m Verfügbark Umbuchung].[Artikel-Nr des Bauteils] = PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Artikel-Nr des Bauteils])
					  LEFT JOIN

					   {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY} as PSZ_Disposition_Materialbestand_Umbuchung

						 ON PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Artikel-Nr des Bauteils] = PSZ_Disposition_Materialbestand_Umbuchung.[Artikel-Nr]
				   )
				   LEFT JOIN

					{PSZ_Disposition_Offene_Materialbestellungen_Umbuchung_TEMPORARY} as [PSZ_Disposition_Offene Materialbestellungen Umbuchung]

					  ON PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Artikel-Nr des Bauteils] = [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[Artikel-Nr]
				GROUP BY
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Fertigungsnummer,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Artikel_Artikelnummer,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bezeichnung 1],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Fertigung_Anzahl,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Fertigstellung,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Artikel-Nr des Bauteils],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Stücklisten_Artikelnummer,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bezeichnung des Bauteils],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Stücklisten_Anzahl,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Bruttobedarf,
				   PSZ_Disposition_Materialbestand_Umbuchung.Bestand,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[Artikel-Nr],
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[Vorname/NameFirma],
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].Anzahl,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].Liefertermin,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Materialbedarf,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Standardlieferant,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bestell-Nr],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Einkaufspreis,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Name1,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Telefon,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Fax,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Wiederbeschaffungszeitraum,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Mindestbestellmenge,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Lagerort_id,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[bestellte Artikel_Bestellung-Nr],
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].Bestätigter_Termin,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Bestätigt1,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[AB-Nr_Lieferant],
				[PSZ_Disposition_Offene Materialbestellungen Umbuchung].[Bestellungen_Bestellung-Nr]
				HAVING
				(((PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bezeichnung 1]) <> 'Reparatur'))
				ORDER BY PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Materialbedarf
				";
				//---------------result-----------
				string query_Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Details_II = $@"
					SELECT [PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details II].[PSZ#], [PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details II].Bestand, 
					[PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details II].Lagerort, [PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details II].Lagerort_id 
					FROM 
					(
						SELECT [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Stücklisten_Artikelnummer AS [PSZ#], Lager.Bestand, Lagerorte.Lagerort, Lagerorte.Lagerort_id
										FROM 
										{Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table} as
										[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table] INNER JOIN (Lager INNER JOIN Lagerorte ON 
										Lager.Lagerort_id = Lagerorte.Lagerort_id) ON [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].[Artikel-Nr des Bauteils] = Lager.[Artikel-Nr]
										GROUP BY [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Stücklisten_Artikelnummer, Lager.Bestand, Lagerorte.Lagerort, Lagerorte.Lagerort_id
										HAVING ((([Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Stücklisten_Artikelnummer)=@Stucklisten_Artikelnummer) 
										AND ((Lager.Bestand)<>0))
					)as
					[PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details II] ORDER BY [PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details II].Lagerort_id; 
					";


				// - execute queries
				using(var sqlCommand = new SqlCommand($"BEGIN TRAN ; {query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung};{query_PSZ_Disposition_Materialbestand_Umbuchung};{query__Reserviert_Menge_Umbuchung};{queryUpdate_Bestand_Material};{query_PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung};{query_PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung};{delete_PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung};{query_PSZ_Disposition_Offene_Materialbestellungen_Umbuchung} ;{QueryPsz_Disposition_Nettobedarfsermittlung_Umbuchung_Table};{query_Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Details_II} ; COMMIT TRAN ;", sqlConnection))
				{
					sqlCommand.Parameters.AddWithValue("bis", bis);
					sqlCommand.Parameters.AddWithValue("Stucklisten_Artikelnummer", Stucklisten_Artikelnummer);
					sqlCommand.CommandTimeout = 0;

					DbExecution.Fill(sqlCommand, dataTable);

				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_II_Entity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_II_Entity>();
			}
		}

		public static List<Entities.Joins.Logistics.PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_III_Entity> GetPsz_Disposition_UmbuchungList_Details_III(
			string withFG, string withoutFG, string Lieferant,
			 DateTime bis, string lager, int Lager_Id, int Lager_P_Id, List<int> Lagerorte_Lagerort_id, List<int> bestellte_Artikel_Lagerort_id, List<int> tbl_Planung_gestartet_Lagerort_ID
			, string Stucklisten_Artikelnummer, Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging)
		{

			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				var connectionId = sqlConnection.ClientConnectionId.ToString().Replace('-', '_');
				sqlConnection.Open();
				var PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY = $"[##PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_Details_TEMPORARY{connectionId}]";
				var PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY = $"[##PSZ_Disposition_Materialbestand_Umbuchung_Details_TEMPORARY{connectionId}]";
				var PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY = $"[##PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_Details_TEMPORARY{connectionId}]";
				var PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY = $"[##PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY{connectionId}]";

				var PSZ_Disposition_Offene_Materialbestellungen_Umbuchung_TEMPORARY = $"[##PSZ_Disposition_Offene_Materialbestellungen_Umbuchung_Details_TEMPORARY{connectionId}]";
				var Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY = $"[##Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_Details_TEMPORARY{connectionId}]";
				var Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table = $"[##Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Details_Table{connectionId}]";

				//-----------------------------1------------------
				string query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = $@"
				IF OBJECT_ID('tempdb..{PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}') IS NOT NULL DROP TABLE {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY};

                                    CREATE TABLE {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}(
										[Standardlieferant] [bit] NULL,
										[Bestell-Nr] [nvarchar](250) NULL,
										[Einkaufspreis] [decimal](28,13) NULL,
										[Wiederbeschaffungszeitraum] [smallint] NULL,
										[Mindestbestellmenge] [decimal](28,13) NULL,
										[Artikel-Nr] [int] NULL,
										[Name1] [nvarchar](50) NULL,
										[Telefon] [nvarchar](250) NULL,
										[Fax] [nvarchar](250) NULL,
										[Lagerort_id] [int] NULL,
										[Termin_Bestätigt1] [datetime] NULL,
										[Bruttobedarf] [decimal](28,13) NULL,
										[Termin_Materialbedarf] [datetime] NULL,
										[Fertigungsnummer] [int] NULL,
										[Artikel-Nr des Bauteils] [int] NULL,
										[Bezeichnung des Bauteils] [nvarchar](1000) NULL,
										[Stücklisten_Artikelnummer] [nvarchar](250) NULL,
										[Artikel_Nr] [int] NULL,
										[Termin_Fertigstellung] [datetime] NULL,
										[Bezeichnung 1] [nvarchar](200) NULL,
										[Stücklisten_Anzahl] [decimal](28,13) NULL,
										[Artikel_Artikelnummer] [nvarchar](50) NULL,
										[Fertigung_Anzahl] [int] NULL,
										[Lagerort] [nvarchar](100) NULL
									);

				INSERT INTO {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}
				SELECT UoFilter.Standardlieferant, UoFilter.[Bestell-Nr], UoFilter.Einkaufspreis, 
				UoFilter.Wiederbeschaffungszeitraum, UoFilter.Mindestbestellmenge, UoFilter.[Artikel-Nr], 
				UoFilter.Name1, UoFilter.Telefon, UoFilter.Fax, UoFilter.Lagerort_id, 
				UoFilter.Termin_Bestätigt1, UoFilter.Bruttobedarf, UoFilter.Termin_Materialbedarf, 
				UoFilter.Fertigungsnummer, UoFilter.[Artikel-Nr des Bauteils], UoFilter.[Bezeichnung des Bauteils], 
				UoFilter.Stücklisten_Artikelnummer, UoFilter.Artikel_Nr, UoFilter.Termin_Fertigstellung, 
				UoFilter.[Bezeichnung 1], UoFilter.Stücklisten_Anzahl, UoFilter.Artikel_Artikelnummer, 
				UoFilter.Fertigung_Anzahl, UoFilter.Lagerort 
				FROM [View_PSZ_Disposition_Bruttobedarfsermittlung Umbuchung] as UoFilter
				WHERE   UoFilter.Termin_Bestätigt1<=@bis AND UoFilter.Name1 Like '{Lieferant}%' ";
				if(withFG != "" && withFG != null)
				{
					query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + $@"
					And UoFilter.Artikel_Artikelnummer Like '{withFG}%'";
				}
				if(withoutFG != "" && withoutFG != null)
				{
					query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + $@"
					AND UoFilter.Artikel_Artikelnummer Not Like '{withoutFG}%'";
				}


				if(lager == "WS")
				{
					query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + $@"
			    AND (UoFilter.Lagerort Like '{lager}%' or UoFilter.Lagerort='TN') ";
				}
				else
				{
					query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + $@"
			    AND UoFilter.Lagerort Like '{lager}%' ";
				}

				if(lager == "Eigenfertigung")
				{
					query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + $@"INSERT INTO {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}
				SELECT UoFilter.Standardlieferant, UoFilter.[Bestell-Nr], UoFilter.Einkaufspreis, 
				UoFilter.Wiederbeschaffungszeitraum, UoFilter.Mindestbestellmenge, UoFilter.[Artikel-Nr], 
				UoFilter.Name1, UoFilter.Telefon, UoFilter.Fax, UoFilter.Lagerort_id, 
				UoFilter.Termin_Bestätigt1, UoFilter.Bruttobedarf, UoFilter.Termin_Materialbedarf, 
				UoFilter.Fertigungsnummer, UoFilter.[Artikel-Nr des Bauteils], UoFilter.[Bezeichnung des Bauteils], 
				UoFilter.Stücklisten_Artikelnummer, UoFilter.Artikel_Nr, UoFilter.Termin_Fertigstellung, 
				UoFilter.[Bezeichnung 1], UoFilter.Stücklisten_Anzahl, UoFilter.Artikel_Artikelnummer, 
				UoFilter.Fertigung_Anzahl, UoFilter.Lagerort 
				FROM [View_PSZ_Disposition_Bruttobedarfsermittlung Umbuchung] as UoFilter
				WHERE  UoFilter.Termin_Bestätigt1<=@bis AND UoFilter.Name1 Like '{Lieferant}%'  AND (UoFilter.Lagerort Like 'SC%') ";
					if(withFG != "" && withFG != null)
					{
						query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + $@"
					And UoFilter.Artikel_Artikelnummer Like '{withFG}%'";
					}
					if(withoutFG != "" && withoutFG != null)
					{
						query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + $@"
					AND UoFilter.Artikel_Artikelnummer Not Like '{withoutFG}%' ";
					}

				}
				query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + " ; ";
				//------------2
				string query_PSZ_Disposition_Materialbestand_Umbuchung = "";
				if(lager != "Eigenfertigung")
				{
					query_PSZ_Disposition_Materialbestand_Umbuchung = $@"
				IF OBJECT_ID('tempdb..{PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}') IS NOT NULL DROP TABLE {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY};
				      CREATE TABLE {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}(
										[Artikel-Nr] [int] NULL,
										[Bestand] [decimal](28,13)NULL
									);

				INSERT INTO {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}
					SELECT Lager.[Artikel-Nr], Lager.Bestand 
				FROM {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY} INNER JOIN (Lager INNER JOIN Lagerorte ON Lager.Lagerort_id = Lagerorte.Lagerort_id) 
				ON {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}.[Artikel-Nr des Bauteils] = Lager.[Artikel-Nr]
				WHERE ((Lagerorte.Lagerort Like '{lager}%' ))
				GROUP BY Lager.[Artikel-Nr], Lager.Bestand, Lager.Lagerort_id;";
				}
				else
				{
					query_PSZ_Disposition_Materialbestand_Umbuchung = $@"
				IF OBJECT_ID('tempdb..{PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}') IS NOT NULL DROP TABLE {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY};
				      CREATE TABLE {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}(
										[Artikel-Nr] [int] NULL,
										[Bestand] [decimal](28,13)NULL
									);

				INSERT INTO {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}
				SELECT Lager.[Artikel-Nr], Sum(Lager.Bestand) AS Bestand
				FROM Lager INNER JOIN Lagerorte ON Lager.Lagerort_id = Lagerorte.Lagerort_id
				WHERE (((Lager.Lagerort_id)=9 Or (Lager.Lagerort_id)=52 Or (Lager.Lagerort_id)=6 Or (Lager.Lagerort_id)=53 Or (Lager.Lagerort_id)=17))
				GROUP BY Lager.[Artikel-Nr];";
				}

				string query__Reserviert_Menge_Umbuchung = $@"
					IF OBJECT_ID('tempdb..{Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY}') IS NOT NULL DROP TABLE {Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY};
				      CREATE TABLE {Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY}(
										[Artikel_Nr] [int] NULL,
										[Artikelnummer] [nvarchar](200) NULL,
										[Menge_Reserviert] [decimal](28,13) NULL ,
										[Type] [int] NULL,
										[Lagerort_ID] [int] NULL
									);

					INSERT INTO {Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY}
					SELECT tbl_Planung_gestartet.Artikel_Nr AS Artikel_Nr, Artikel.Artikelnummer AS Artikelnummer, Sum(tbl_Planung_gestartet.Menge_reserviert) AS Menge_Reserviert, 
					Artikel.Warentyp AS Type, tbl_Planung_gestartet.Lagerort_ID 
					FROM tbl_Planung_gestartet INNER JOIN Artikel ON tbl_Planung_gestartet.Artikel_Nr = Artikel.[Artikel-Nr]
					WHERE ((

					(tbl_Planung_gestartet.Lagerort_ID) in ({string.Join(',', tbl_Planung_gestartet_Lagerort_ID.ToArray())})
					
					)) OR (((Artikel.Warentyp)<>2 Or 
					(Artikel.Warentyp) Is Null))
					GROUP BY tbl_Planung_gestartet.Artikel_Nr, Artikel.Artikelnummer, Artikel.Warentyp, tbl_Planung_gestartet.Lagerort_ID ;
					";
				string queryUpdate_Bestand_Material = $@"
					
					UPDATE {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}
					SET {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand 
					= {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand+Lager.bestand
					from  {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}
					INNER JOIN 
					(Lager INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr]) 
					 ON Artikel.[Artikel-Nr] ={PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.[Artikel-Nr] 
					WHERE Lager.Lagerort_id={Lager_P_Id} AND Artikel.Warentyp=2 ";
				if(lager != "Eigenfertigung")
				{
					queryUpdate_Bestand_Material = queryUpdate_Bestand_Material + $@"
					UPDATE {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}  
					SET {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand = 
					{PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand+Lager.Bestand
					from {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY} 
					INNER JOIN (select [artikel-nr] as [Artikel-Nr],sum(Bestand) as bestand from lager where Lagerort_id in({string.Join(',', Lagerorte_Lagerort_id.ToArray())})
					group by [artikel-nr])Lager  
					ON {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.[Artikel-Nr] = Lager.[Artikel-Nr] 
					WHERE (((Lager.Bestand)>0)
					) 
					";


				}
				queryUpdate_Bestand_Material = queryUpdate_Bestand_Material + $@"
					UPDATE {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY} 
					SET {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand =
					{PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand-{Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY}.Menge_Reserviert
					from {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}
					INNER JOIN {Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY} 
					ON {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.[Artikel-Nr] = {Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY}.Artikel_Nr 
					WHERE ((({Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY}.Lagerort_ID)={Lager_Id}))  ;";
				//-----------------3--------------
				string query_PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung = $@"

				IF OBJECT_ID('tempdb..{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}') IS NOT NULL DROP TABLE {PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY};
								CREATE TABLE {PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}(
										[Artikel-Nr des Bauteils] [int] NULL,
										[SummevonBruttobedarf] [decimal](28,13)  NULL,
										[Bestand] [decimal](28,13)  NULL
									);
								INSERT INTO {PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY} 
								SELECT {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}.[Artikel-Nr des Bauteils], Sum({PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}.Bruttobedarf)
							AS SummevonBruttobedarf, {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand as  Bestand
								FROM {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY} INNER JOIN {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY} 
							ON {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}.[Artikel-Nr des Bauteils] = {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.[Artikel-Nr]
								GROUP BY {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}.[Artikel-Nr des Bauteils], {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand ;";
				string query_PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung = $@"
								IF OBJECT_ID('tempdb..{PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY}') IS NOT NULL DROP TABLE {PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY};
								CREATE TABLE {PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY}(
										[Artikel-Nr des Bauteils] [int] NULL,
										[SummevonBruttobedarf] [decimal](28,13)  NULL,
										[Verfügbar] [decimal](28,13) NULL,
										[Bestand] [decimal](28,13)  NULL
									);
								INSERT INTO {PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY} 
				SELECT {PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.[Artikel-Nr des Bauteils],
				{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.SummevonBruttobedarf, 
				{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.Bestand-{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.SummevonBruttobedarf AS Verfügbar, 
				{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.Bestand 
				FROM {PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}
				GROUP BY {PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.[Artikel-Nr des Bauteils], {PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.SummevonBruttobedarf,
				{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.Bestand-{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.SummevonBruttobedarf, 
				{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.Bestand;";
				string delete_PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung = $@"
DELETE 
FROM {PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY}
WHERE {PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY}.Verfügbar>=0;";
				//----------4-----------
				string query_PSZ_Disposition_Offene_Materialbestellungen_Umbuchung = $@"
				IF OBJECT_ID('tempdb..{PSZ_Disposition_Offene_Materialbestellungen_Umbuchung_TEMPORARY}') IS NOT NULL DROP TABLE {PSZ_Disposition_Offene_Materialbestellungen_Umbuchung_TEMPORARY};
				CREATE TABLE {PSZ_Disposition_Offene_Materialbestellungen_Umbuchung_TEMPORARY}(
									[bestellte Artikel_Bestellung-Nr] [int] NULL,
									[Artikel-Nr] [int] NULL,
									[Bezeichnung 1] [nvarchar](200) NULL,
									[Bezeichnung 2] [ntext] NULL,
									[Anzahl]  [decimal](28,13) NULL,
									[Liefertermin] [datetime] NULL,
									[erledigt_pos] [bit] NULL,
									[erledigt] [bit] NULL,
									[Bestellungen_Bestellung-Nr] [int] NULL,
									[gebucht] [bit] NULL,
									[Vorname/NameFirma] [nvarchar](100) NULL,
									[Projekt-Nr] [nvarchar](250) NULL,
									[Bestätigter_Termin] [datetime] NULL,
									[Lagerort_id] [int] NULL,
									[Typ] [nvarchar](50) NULL,
									[Datum] [datetime] NULL,
									[AB-Nr_Lieferant] [nvarchar](50) NULL
									);
				INSERT INTO {PSZ_Disposition_Offene_Materialbestellungen_Umbuchung_TEMPORARY}
				SELECT distinct [bestellte Artikel].[Bestellung-Nr]  as [bestellte Artikel_Bestellung-Nr], [bestellte Artikel].[Artikel-Nr], [bestellte Artikel].[Bezeichnung 1], CAST([bestellte Artikel].[Bezeichnung 2] AS NVARCHAR(MAX))
				as [Bezeichnung 2], 
				[bestellte Artikel].Anzahl, [bestellte Artikel].Liefertermin, [bestellte Artikel].erledigt_pos, Bestellungen.erledigt, Bestellungen.[Bestellung-Nr]as [Bestellungen_Bestellung-Nr], 
				Bestellungen.gebucht, Bestellungen.[Vorname/NameFirma], Bestellungen.[Projekt-Nr], [bestellte Artikel].Bestätigter_Termin, [bestellte Artikel].Lagerort_id, 
				Bestellungen.Typ, Bestellungen.Datum, [bestellte Artikel].[AB-Nr_Lieferant] 
				FROM {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY} INNER JOIN ([bestellte Artikel] INNER JOIN Bestellungen ON [bestellte Artikel].[Bestellung-Nr] = Bestellungen.Nr)
				ON {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}.[Artikel-Nr des Bauteils] = [bestellte Artikel].[Artikel-Nr]
				WHERE ((([bestellte Artikel].erledigt_pos)=0) AND ((Bestellungen.erledigt)=0) AND ((Bestellungen.gebucht)=1) AND (
				([bestellte Artikel].Lagerort_id) in ({string.Join(',', bestellte_Artikel_Lagerort_id.ToArray())}) 
				) AND ((Bestellungen.Typ)='Bestellung' Or (Bestellungen.Typ)='Kanbanabruf') AND ((Bestellungen.Datum)>GetDate()-400))
				ORDER BY [bestellte Artikel].[Artikel-Nr];";

				//----------5-------------
				string QueryPsz_Disposition_Nettobedarfsermittlung_Umbuchung_Table = $@"
				IF OBJECT_ID('tempdb..{Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table}') IS NOT NULL DROP TABLE {Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table};
				CREATE TABLE {Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table}(
									[Fertigungsnummer] [int] NULL,
									[Artikel_Artikelnummer] [nvarchar](200) NULL,
									[Bezeichnung 1] [nvarchar](200) NULL,
									[Fertigung_Anzahl] [int] NULL,
									[Termin_Fertigstellung]  [datetime] NULL,
									[Artikel-Nr des Bauteils]  [int] NULL,
									[Stücklisten_Artikelnummer] [nvarchar](250) NULL,
									[Bezeichnung des Bauteils] [nvarchar](1000) NULL,
									[Stücklisten_Anzahl] [decimal](28,13) NULL,
									[Bruttobedarf] [decimal](28,13) NULL,
									[Bestand] [decimal](28,13)  NULL,
									[Artikel-Nr] [int] NULL,
									[Vorname/NameFirma] [nvarchar](100) NULL,
									[Anzahl] [decimal](28,13) NULL,
									[Liefertermin] [datetime]  NULL,
									[Termin_Materialbedarf] [datetime] NULL,
									[Standardlieferant] [bit] NULL,
									[Bestell-Nr] [nvarchar](250) NULL,
									[Einkaufspreis] [decimal](28,13) NULL,
									[Name1] [nvarchar](50) NULL,
									[Telefon] [nvarchar](250) NULL,
									[Fax] [nvarchar](250) NULL,
									[Wiederbeschaffungszeitraum] [smallint] NULL,
									[Mindestbestellmenge] [decimal](28,13) NULL,
									[Lagerort_id] [int] NULL,
									[bestellte Artikel_Bestellung-Nr] [int] NULL,
									[Bestätigter_Termin] [datetime] NULL,
									[Termin_Bestätigt1] [datetime] NULL,
									[Bearbeitet][int] NULL,
									[AB-Nr_Lieferant] [nvarchar](50) NULL,
									[Bestellungen_Bestellung-Nr] [int] NULL
									);
					INSERT INTO {Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table}
					SELECT
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Fertigungsnummer,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Artikel_Artikelnummer,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bezeichnung 1],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Fertigung_Anzahl,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Fertigstellung,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Artikel-Nr des Bauteils],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Stücklisten_Artikelnummer,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bezeichnung des Bauteils],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Stücklisten_Anzahl,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Bruttobedarf,
				   PSZ_Disposition_Materialbestand_Umbuchung.Bestand as Bestand,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[Artikel-Nr],
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[Vorname/NameFirma],
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].Anzahl,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].Liefertermin,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Materialbedarf,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Standardlieferant,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bestell-Nr],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Einkaufspreis,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Name1,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Telefon,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Fax,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Wiederbeschaffungszeitraum,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Mindestbestellmenge,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Lagerort_id,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[bestellte Artikel_Bestellung-Nr],
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].Bestätigter_Termin,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Bestätigt1,
					0 AS Bearbeitet,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[AB-Nr_Lieferant],
					[PSZ_Disposition_Offene Materialbestellungen Umbuchung].[Bestellungen_Bestellung-Nr]
				FROM
				   (
				({PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY}as[PSZ_Disposition_Bruttomaterialbedarf sum m Verfügbark Umbuchung]

					  LEFT JOIN

						{PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY} as PSZ_Disposition_Bruttomaterialbedarf_Umbuchung

						 ON[PSZ_Disposition_Bruttomaterialbedarf sum m Verfügbark Umbuchung].[Artikel-Nr des Bauteils] = PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Artikel-Nr des Bauteils])
					  LEFT JOIN

					   {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY} as PSZ_Disposition_Materialbestand_Umbuchung

						 ON PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Artikel-Nr des Bauteils] = PSZ_Disposition_Materialbestand_Umbuchung.[Artikel-Nr]
				   )
				   LEFT JOIN

					{PSZ_Disposition_Offene_Materialbestellungen_Umbuchung_TEMPORARY} as [PSZ_Disposition_Offene Materialbestellungen Umbuchung]

					  ON PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Artikel-Nr des Bauteils] = [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[Artikel-Nr]
				GROUP BY
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Fertigungsnummer,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Artikel_Artikelnummer,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bezeichnung 1],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Fertigung_Anzahl,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Fertigstellung,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Artikel-Nr des Bauteils],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Stücklisten_Artikelnummer,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bezeichnung des Bauteils],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Stücklisten_Anzahl,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Bruttobedarf,
				   PSZ_Disposition_Materialbestand_Umbuchung.Bestand,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[Artikel-Nr],
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[Vorname/NameFirma],
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].Anzahl,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].Liefertermin,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Materialbedarf,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Standardlieferant,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bestell-Nr],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Einkaufspreis,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Name1,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Telefon,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Fax,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Wiederbeschaffungszeitraum,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Mindestbestellmenge,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Lagerort_id,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[bestellte Artikel_Bestellung-Nr],
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].Bestätigter_Termin,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Bestätigt1,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[AB-Nr_Lieferant],
				[PSZ_Disposition_Offene Materialbestellungen Umbuchung].[Bestellungen_Bestellung-Nr]
				HAVING
				(((PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bezeichnung 1]) <> 'Reparatur'))
				ORDER BY PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Materialbedarf
				";


				//---------------result-----------
				string query_Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Details_III = $@"
					SELECT [PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details III].[PSZ#], [PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details III].[Artikel-Nr des Bauteils],
					[PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details III].[Vorname/NameFirma], 
					[PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details III].Standardlieferant, 
					[PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details III].[Bestell-Nr], [PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details III].Einkaufspreis, 
					[PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details III].Telefon, [PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details III].Fax, 
					[PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details III].Wiederbeschaffungszeitraum, 
					[PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details III].Mindestbestellmenge, 
					[PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details III].[Bestellungen_Bestellung-Nr], 
					[PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details III].Anzahl, 
					[PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details III].Liefertermin, 
					[PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details III].Bestätigter_Termin, 
					[PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details III].[bestellte Artikel_Bestellung-Nr], 
					[PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details III].Name1, [PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details III].[AB-Nr_Lieferant], 
					[PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details III].Lagerort_id
					FROM 
					(SELECT [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Stücklisten_Artikelnummer AS [PSZ#], 
					[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].[Artikel-Nr des Bauteils], [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].[Vorname/NameFirma], 
					[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Standardlieferant, [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].[Bestell-Nr], 
					[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Einkaufspreis, [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Telefon, 
					[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Fax, [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Wiederbeschaffungszeitraum,
					[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Mindestbestellmenge, [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].[Bestellungen_Bestellung-Nr], 
					[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Anzahl, [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Liefertermin, 
					[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Bestätigter_Termin, [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].[bestellte Artikel_Bestellung-Nr], 
					[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Name1, [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].[AB-Nr_Lieferant], 
					[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Lagerort_id
					FROM {Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table} as
					[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table]
					GROUP BY [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Stücklisten_Artikelnummer, [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].[Artikel-Nr des Bauteils],
					[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].[Vorname/NameFirma], [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Standardlieferant, 
					[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].[Bestell-Nr], [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Einkaufspreis,
					[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Telefon, [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Fax, 
					[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Wiederbeschaffungszeitraum, [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Mindestbestellmenge, 
					[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].[Bestellungen_Bestellung-Nr], [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Anzahl, 
					[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Liefertermin, [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Bestätigter_Termin, 
					[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].[bestellte Artikel_Bestellung-Nr], [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Name1, 
					[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].[AB-Nr_Lieferant], [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Lagerort_id
					HAVING ((([Psz_Disposition_Nettobedarfsermittlung Umbuchung Table].Stücklisten_Artikelnummer)=@Stucklisten_Artikelnummer)))as
					[PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details III] ORDER BY [PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details III].Liefertermin;
					";


				// - execute queries
				using(var sqlCommand = new SqlCommand($"BEGIN TRAN ; {query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung};{query_PSZ_Disposition_Materialbestand_Umbuchung};{query__Reserviert_Menge_Umbuchung};{queryUpdate_Bestand_Material};{query_PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung};{query_PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung};{delete_PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung};{query_PSZ_Disposition_Offene_Materialbestellungen_Umbuchung} ;{QueryPsz_Disposition_Nettobedarfsermittlung_Umbuchung_Table};{query_Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Details_III}; COMMIT TRAN ;", sqlConnection))
				{
					sqlCommand.Parameters.AddWithValue("Stucklisten_Artikelnummer", Stucklisten_Artikelnummer);
					sqlCommand.Parameters.AddWithValue("bis", bis);
					sqlCommand.CommandTimeout = 0;

					DbExecution.Fill(sqlCommand, dataTable);

				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_III_Entity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_III_Entity>();
			}
		}
		public static List<Entities.Joins.Logistics.PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_IV_Entity> GetPsz_Disposition_UmbuchungList_Details_IV(
		string withFG, string withoutFG, string Lieferant,
		 DateTime bis, string lager, int Lager_Id, int Lager_P_Id, List<int> Lagerorte_Lagerort_id, List<int> bestellte_Artikel_Lagerort_id, List<int> tbl_Planung_gestartet_Lagerort_ID
		, string Stucklisten_Artikelnummer, Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging)
		{

			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				var connectionId = sqlConnection.ClientConnectionId.ToString().Replace('-', '_');
				sqlConnection.Open();
				var PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY = $"[##PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_Details_TEMPORARY{connectionId}]";
				var PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY = $"[##PSZ_Disposition_Materialbestand_Umbuchung_Details_TEMPORARY{connectionId}]";
				var PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY = $"[##PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_Details_TEMPORARY{connectionId}]";
				var PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY = $"[##PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY{connectionId}]";

				var PSZ_Disposition_Offene_Materialbestellungen_Umbuchung_TEMPORARY = $"[##PSZ_Disposition_Offene_Materialbestellungen_Umbuchung_Details_TEMPORARY{connectionId}]";
				var Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY = $"[##Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_Details_TEMPORARY{connectionId}]";
				var Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table = $"[##Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Details_Table{connectionId}]";
				var Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table_FA = $"[##Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Details_Table_FA{connectionId}]";

				//-----------------------------1------------------
				string query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = $@"
				IF OBJECT_ID('tempdb..{PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}') IS NOT NULL DROP TABLE {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY};

                                    CREATE TABLE {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}(
										[Standardlieferant] [bit] NULL,
										[Bestell-Nr] [nvarchar](250) NULL,
										[Einkaufspreis] [decimal](28,13) NULL,
										[Wiederbeschaffungszeitraum] [smallint] NULL,
										[Mindestbestellmenge] [decimal](28,13) NULL,
										[Artikel-Nr] [int] NULL,
										[Name1] [nvarchar](50) NULL,
										[Telefon] [nvarchar](250) NULL,
										[Fax] [nvarchar](250) NULL,
										[Lagerort_id] [int] NULL,
										[Termin_Bestätigt1] [datetime] NULL,
										[Bruttobedarf] [decimal](28,13) NULL,
										[Termin_Materialbedarf] [datetime] NULL,
										[Fertigungsnummer] [int] NULL,
										[Artikel-Nr des Bauteils] [int] NULL,
										[Bezeichnung des Bauteils] [nvarchar](1000) NULL,
										[Stücklisten_Artikelnummer] [nvarchar](250) NULL,
										[Artikel_Nr] [int] NULL,
										[Termin_Fertigstellung] [datetime] NULL,
										[Bezeichnung 1] [nvarchar](200) NULL,
										[Stücklisten_Anzahl] [decimal](28,13) NULL,
										[Artikel_Artikelnummer] [nvarchar](50) NULL,
										[Fertigung_Anzahl] [int] NULL,
										[Lagerort] [nvarchar](100) NULL
									);

				INSERT INTO {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}
				SELECT UoFilter.Standardlieferant, UoFilter.[Bestell-Nr], UoFilter.Einkaufspreis, 
				UoFilter.Wiederbeschaffungszeitraum, UoFilter.Mindestbestellmenge, UoFilter.[Artikel-Nr], 
				UoFilter.Name1, UoFilter.Telefon, UoFilter.Fax, UoFilter.Lagerort_id, 
				UoFilter.Termin_Bestätigt1, UoFilter.Bruttobedarf, UoFilter.Termin_Materialbedarf, 
				UoFilter.Fertigungsnummer, UoFilter.[Artikel-Nr des Bauteils], UoFilter.[Bezeichnung des Bauteils], 
				UoFilter.Stücklisten_Artikelnummer, UoFilter.Artikel_Nr, UoFilter.Termin_Fertigstellung, 
				UoFilter.[Bezeichnung 1], UoFilter.Stücklisten_Anzahl, UoFilter.Artikel_Artikelnummer, 
				UoFilter.Fertigung_Anzahl, UoFilter.Lagerort 
				FROM [View_PSZ_Disposition_Bruttobedarfsermittlung Umbuchung] as UoFilter
				WHERE   UoFilter.Termin_Bestätigt1<=@bis AND UoFilter.Name1 Like '{Lieferant}%' ";
				if(withFG != "" && withFG != null)
				{
					query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + $@"
					And UoFilter.Artikel_Artikelnummer Like '{withFG}%'";
				}
				if(withoutFG != "" && withoutFG != null)
				{
					query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + $@"
					AND UoFilter.Artikel_Artikelnummer Not Like '{withoutFG}%'";
				}


				if(lager == "WS")
				{
					query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + $@"
			
				AND (UoFilter.Lagerort Like '{lager}%' or UoFilter.Lagerort='TN') ";
				}
				else
				{

					query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + $@"
			
				AND UoFilter.Lagerort Like '{lager}%' ";
				}

				if(lager == "Eigenfertigung")
				{
					query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + $@"INSERT INTO {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}
				SELECT UoFilter.Standardlieferant, UoFilter.[Bestell-Nr], UoFilter.Einkaufspreis, 
				UoFilter.Wiederbeschaffungszeitraum, UoFilter.Mindestbestellmenge, UoFilter.[Artikel-Nr], 
				UoFilter.Name1, UoFilter.Telefon, UoFilter.Fax, UoFilter.Lagerort_id, 
				UoFilter.Termin_Bestätigt1, UoFilter.Bruttobedarf, UoFilter.Termin_Materialbedarf, 
				UoFilter.Fertigungsnummer, UoFilter.[Artikel-Nr des Bauteils], UoFilter.[Bezeichnung des Bauteils], 
				UoFilter.Stücklisten_Artikelnummer, UoFilter.Artikel_Nr, UoFilter.Termin_Fertigstellung, 
				UoFilter.[Bezeichnung 1], UoFilter.Stücklisten_Anzahl, UoFilter.Artikel_Artikelnummer, 
				UoFilter.Fertigung_Anzahl, UoFilter.Lagerort 
				FROM [View_PSZ_Disposition_Bruttobedarfsermittlung Umbuchung] as UoFilter
				WHERE  UoFilter.Termin_Bestätigt1<=@bis AND UoFilter.Name1 Like '{Lieferant}%'  AND (UoFilter.Lagerort Like 'SC%') ";
					if(withFG != "" && withFG != null)
					{
						query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + $@"
					And UoFilter.Artikel_Artikelnummer Like '{withFG}%'";
					}
					if(withoutFG != "" && withoutFG != null)
					{
						query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + $@"
					AND UoFilter.Artikel_Artikelnummer Not Like '{withoutFG}%' ";
					}

				}
				query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung = query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung + " ; ";
				//------------2
				string query_PSZ_Disposition_Materialbestand_Umbuchung = "";
				if(lager != "Eigenfertigung")
				{
					query_PSZ_Disposition_Materialbestand_Umbuchung = $@"
				IF OBJECT_ID('tempdb..{PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}') IS NOT NULL DROP TABLE {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY};
				      CREATE TABLE {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}(
										[Artikel-Nr] [int] NULL,
										[Bestand] [decimal](28,13)NULL
									);

				INSERT INTO {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}
					SELECT Lager.[Artikel-Nr], Lager.Bestand 
				FROM {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY} INNER JOIN (Lager INNER JOIN Lagerorte ON Lager.Lagerort_id = Lagerorte.Lagerort_id) 
				ON {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}.[Artikel-Nr des Bauteils] = Lager.[Artikel-Nr]
				WHERE ((Lagerorte.Lagerort Like '{lager}%' ))
				GROUP BY Lager.[Artikel-Nr], Lager.Bestand, Lager.Lagerort_id;";
				}
				else
				{
					query_PSZ_Disposition_Materialbestand_Umbuchung = $@"
				IF OBJECT_ID('tempdb..{PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}') IS NOT NULL DROP TABLE {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY};
				      CREATE TABLE {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}(
										[Artikel-Nr] [int] NULL,
										[Bestand] [decimal](28,13)NULL
									);

				INSERT INTO {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}
				SELECT Lager.[Artikel-Nr], Sum(Lager.Bestand) AS Bestand
				FROM Lager INNER JOIN Lagerorte ON Lager.Lagerort_id = Lagerorte.Lagerort_id
				WHERE (((Lager.Lagerort_id)=9 Or (Lager.Lagerort_id)=52 Or (Lager.Lagerort_id)=6 Or (Lager.Lagerort_id)=53 Or (Lager.Lagerort_id)=17))
				GROUP BY Lager.[Artikel-Nr];";
				}

				string query__Reserviert_Menge_Umbuchung = $@"
					IF OBJECT_ID('tempdb..{Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY}') IS NOT NULL DROP TABLE {Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY};
				      CREATE TABLE {Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY}(
										[Artikel_Nr] [int] NULL,
										[Artikelnummer] [nvarchar](200) NULL,
										[Menge_Reserviert] [decimal](28,13) NULL ,
										[Type] [int] NULL,
										[Lagerort_ID] [int] NULL
									);

					INSERT INTO {Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY}
					SELECT tbl_Planung_gestartet.Artikel_Nr AS Artikel_Nr, Artikel.Artikelnummer AS Artikelnummer, Sum(tbl_Planung_gestartet.Menge_reserviert) AS Menge_Reserviert, 
					Artikel.Warentyp AS Type, tbl_Planung_gestartet.Lagerort_ID 
					FROM tbl_Planung_gestartet INNER JOIN Artikel ON tbl_Planung_gestartet.Artikel_Nr = Artikel.[Artikel-Nr]
					WHERE ((

					(tbl_Planung_gestartet.Lagerort_ID) in ({string.Join(',', tbl_Planung_gestartet_Lagerort_ID.ToArray())})
					
					)) OR (((Artikel.Warentyp)<>2 Or 
					(Artikel.Warentyp) Is Null))
					GROUP BY tbl_Planung_gestartet.Artikel_Nr, Artikel.Artikelnummer, Artikel.Warentyp, tbl_Planung_gestartet.Lagerort_ID ;
					";
				string queryUpdate_Bestand_Material = $@"
					
					UPDATE {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}
					SET {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand 
					= {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand+Lager.bestand
					from  {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}
					INNER JOIN 
					(Lager INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr]) 
					 ON Artikel.[Artikel-Nr] ={PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.[Artikel-Nr] 
					WHERE Lager.Lagerort_id={Lager_P_Id} AND Artikel.Warentyp=2 ";
				if(lager != "Eigenfertigung")
				{
					queryUpdate_Bestand_Material = queryUpdate_Bestand_Material + $@"
					UPDATE {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}  
					SET {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand = 
					{PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand+Lager.Bestand
					from {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY} 
					INNER JOIN (select [artikel-nr] as [Artikel-Nr],sum(Bestand) as bestand from lager where Lagerort_id in({string.Join(',', Lagerorte_Lagerort_id.ToArray())})
					group by [artikel-nr])Lager  
					ON {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.[Artikel-Nr] = Lager.[Artikel-Nr] 
					WHERE (((Lager.Bestand)>0)
					)

					";


				}
				queryUpdate_Bestand_Material = queryUpdate_Bestand_Material + $@"
					UPDATE {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY} 
					SET {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand =
					{PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand-{Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY}.Menge_Reserviert
					from {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}
					INNER JOIN {Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY} 
					ON {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.[Artikel-Nr] = {Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY}.Artikel_Nr 
					WHERE ((({Tabell_Disposition_Reserviert_Menge_Umbuchung_Liste_TEMPORARY}.Lagerort_ID)={Lager_Id}))  ;";
				//-----------------3--------------
				string query_PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung = $@"

				IF OBJECT_ID('tempdb..{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}') IS NOT NULL DROP TABLE {PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY};
								CREATE TABLE {PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}(
										[Artikel-Nr des Bauteils] [int] NULL,
										[SummevonBruttobedarf] [decimal](28,13)  NULL,
										[Bestand] [decimal](28,13)  NULL
									);
								INSERT INTO {PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY} 
								SELECT {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}.[Artikel-Nr des Bauteils], Sum({PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}.Bruttobedarf)
							AS SummevonBruttobedarf, {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand as  Bestand
								FROM {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY} INNER JOIN {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY} 
							ON {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}.[Artikel-Nr des Bauteils] = {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.[Artikel-Nr]
								GROUP BY {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}.[Artikel-Nr des Bauteils], {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY}.Bestand ;";
				string query_PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung = $@"
								IF OBJECT_ID('tempdb..{PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY}') IS NOT NULL DROP TABLE {PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY};
								CREATE TABLE {PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY}(
										[Artikel-Nr des Bauteils] [int] NULL,
										[SummevonBruttobedarf] [decimal](28,13)  NULL,
										[Verfügbar] [decimal](28,13) NULL,
										[Bestand] [decimal](28,13)  NULL
									);
								INSERT INTO {PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY} 
				SELECT {PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.[Artikel-Nr des Bauteils],
				{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.SummevonBruttobedarf, 
				{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.Bestand-{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.SummevonBruttobedarf AS Verfügbar, 
				{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.Bestand 
				FROM {PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}
				GROUP BY {PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.[Artikel-Nr des Bauteils], {PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.SummevonBruttobedarf,
				{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.Bestand-{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.SummevonBruttobedarf, 
				{PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung_TEMPORARY}.Bestand;";
				string delete_PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung = $@"
DELETE 
FROM {PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY}
WHERE {PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY}.Verfügbar>=0;";
				//----------4-----------
				string query_PSZ_Disposition_Offene_Materialbestellungen_Umbuchung = $@"
				IF OBJECT_ID('tempdb..{PSZ_Disposition_Offene_Materialbestellungen_Umbuchung_TEMPORARY}') IS NOT NULL DROP TABLE {PSZ_Disposition_Offene_Materialbestellungen_Umbuchung_TEMPORARY};
				CREATE TABLE {PSZ_Disposition_Offene_Materialbestellungen_Umbuchung_TEMPORARY}(
									[bestellte Artikel_Bestellung-Nr] [int] NULL,
									[Artikel-Nr] [int] NULL,
									[Bezeichnung 1] [nvarchar](200) NULL,
									[Bezeichnung 2] [ntext] NULL,
									[Anzahl]  [decimal](28,13) NULL,
									[Liefertermin] [datetime] NULL,
									[erledigt_pos] [bit] NULL,
									[erledigt] [bit] NULL,
									[Bestellungen_Bestellung-Nr] [int] NULL,
									[gebucht] [bit] NULL,
									[Vorname/NameFirma] [nvarchar](100) NULL,
									[Projekt-Nr] [nvarchar](250) NULL,
									[Bestätigter_Termin] [datetime] NULL,
									[Lagerort_id] [int] NULL,
									[Typ] [nvarchar](50) NULL,
									[Datum] [datetime] NULL,
									[AB-Nr_Lieferant] [nvarchar](50) NULL
									);
				INSERT INTO {PSZ_Disposition_Offene_Materialbestellungen_Umbuchung_TEMPORARY}
				SELECT distinct [bestellte Artikel].[Bestellung-Nr]  as [bestellte Artikel_Bestellung-Nr], [bestellte Artikel].[Artikel-Nr], [bestellte Artikel].[Bezeichnung 1], CAST([bestellte Artikel].[Bezeichnung 2] AS NVARCHAR(MAX))
				as [Bezeichnung 2], 
				[bestellte Artikel].Anzahl, [bestellte Artikel].Liefertermin, [bestellte Artikel].erledigt_pos, Bestellungen.erledigt, Bestellungen.[Bestellung-Nr]as [Bestellungen_Bestellung-Nr], 
				Bestellungen.gebucht, Bestellungen.[Vorname/NameFirma], Bestellungen.[Projekt-Nr], [bestellte Artikel].Bestätigter_Termin, [bestellte Artikel].Lagerort_id, 
				Bestellungen.Typ, Bestellungen.Datum, [bestellte Artikel].[AB-Nr_Lieferant] 
				FROM {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY} INNER JOIN ([bestellte Artikel] INNER JOIN Bestellungen ON [bestellte Artikel].[Bestellung-Nr] = Bestellungen.Nr)
				ON {PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY}.[Artikel-Nr des Bauteils] = [bestellte Artikel].[Artikel-Nr]
				WHERE ((([bestellte Artikel].erledigt_pos)=0) AND ((Bestellungen.erledigt)=0) AND ((Bestellungen.gebucht)=1) AND (
				([bestellte Artikel].Lagerort_id) in ({string.Join(',', bestellte_Artikel_Lagerort_id.ToArray())}) 
				) AND ((Bestellungen.Typ)='Bestellung' Or (Bestellungen.Typ)='Kanbanabruf') AND ((Bestellungen.Datum)>GetDate()-400))
				ORDER BY [bestellte Artikel].[Artikel-Nr];";

				//----------5-------------
				string QueryPsz_Disposition_Nettobedarfsermittlung_Umbuchung_Table = $@"
				IF OBJECT_ID('tempdb..{Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table}') IS NOT NULL DROP TABLE {Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table};
				CREATE TABLE {Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table}(
									[Fertigungsnummer] [int] NULL,
									[Artikel_Artikelnummer] [nvarchar](200) NULL,
									[Bezeichnung 1] [nvarchar](200) NULL,
									[Fertigung_Anzahl] [int] NULL,
									[Termin_Fertigstellung]  [datetime] NULL,
									[Artikel-Nr des Bauteils]  [int] NULL,
									[Stücklisten_Artikelnummer] [nvarchar](250) NULL,
									[Bezeichnung des Bauteils] [nvarchar](1000) NULL,
									[Stücklisten_Anzahl] [decimal](28,13) NULL,
									[Bruttobedarf] [decimal](28,13) NULL,
									[Bestand] [decimal](28,13)  NULL,
									[Artikel-Nr] [int] NULL,
									[Vorname/NameFirma] [nvarchar](100) NULL,
									[Anzahl] [decimal](28,13) NULL,
									[Liefertermin] [datetime]  NULL,
									[Termin_Materialbedarf] [datetime] NULL,
									[Standardlieferant] [bit] NULL,
									[Bestell-Nr] [nvarchar](250) NULL,
									[Einkaufspreis] [decimal](28,13) NULL,
									[Name1] [nvarchar](50) NULL,
									[Telefon] [nvarchar](250) NULL,
									[Fax] [nvarchar](250) NULL,
									[Wiederbeschaffungszeitraum] [smallint] NULL,
									[Mindestbestellmenge] [decimal](28,13) NULL,
									[Lagerort_id] [int] NULL,
									[bestellte Artikel_Bestellung-Nr] [int] NULL,
									[Bestätigter_Termin] [datetime] NULL,
									[Termin_Bestätigt1] [datetime] NULL,
									[Bearbeitet][int] NULL,
									[AB-Nr_Lieferant] [nvarchar](50) NULL,
									[Bestellungen_Bestellung-Nr] [int] NULL
									);
					INSERT INTO {Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table}
					SELECT
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Fertigungsnummer,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Artikel_Artikelnummer,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bezeichnung 1],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Fertigung_Anzahl,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Fertigstellung,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Artikel-Nr des Bauteils],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Stücklisten_Artikelnummer,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bezeichnung des Bauteils],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Stücklisten_Anzahl,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Bruttobedarf,
				   PSZ_Disposition_Materialbestand_Umbuchung.Bestand as Bestand,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[Artikel-Nr],
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[Vorname/NameFirma],
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].Anzahl,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].Liefertermin,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Materialbedarf,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Standardlieferant,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bestell-Nr],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Einkaufspreis,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Name1,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Telefon,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Fax,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Wiederbeschaffungszeitraum,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Mindestbestellmenge,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Lagerort_id,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[bestellte Artikel_Bestellung-Nr],
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].Bestätigter_Termin,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Bestätigt1,
					0 AS Bearbeitet,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[AB-Nr_Lieferant],
					[PSZ_Disposition_Offene Materialbestellungen Umbuchung].[Bestellungen_Bestellung-Nr]
				FROM
				   (
				({PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY}as[PSZ_Disposition_Bruttomaterialbedarf sum m Verfügbark Umbuchung]

					  LEFT JOIN

						{PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY} as PSZ_Disposition_Bruttomaterialbedarf_Umbuchung

						 ON[PSZ_Disposition_Bruttomaterialbedarf sum m Verfügbark Umbuchung].[Artikel-Nr des Bauteils] = PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Artikel-Nr des Bauteils])
					  LEFT JOIN

					   {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY} as PSZ_Disposition_Materialbestand_Umbuchung

						 ON PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Artikel-Nr des Bauteils] = PSZ_Disposition_Materialbestand_Umbuchung.[Artikel-Nr]
				   )
				   LEFT JOIN

					{PSZ_Disposition_Offene_Materialbestellungen_Umbuchung_TEMPORARY} as [PSZ_Disposition_Offene Materialbestellungen Umbuchung]

					  ON PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Artikel-Nr des Bauteils] = [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[Artikel-Nr]
				GROUP BY
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Fertigungsnummer,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Artikel_Artikelnummer,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bezeichnung 1],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Fertigung_Anzahl,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Fertigstellung,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Artikel-Nr des Bauteils],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Stücklisten_Artikelnummer,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bezeichnung des Bauteils],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Stücklisten_Anzahl,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Bruttobedarf,
				   PSZ_Disposition_Materialbestand_Umbuchung.Bestand,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[Artikel-Nr],
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[Vorname/NameFirma],
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].Anzahl,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].Liefertermin,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Materialbedarf,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Standardlieferant,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bestell-Nr],
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Einkaufspreis,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Name1,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Telefon,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Fax,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Wiederbeschaffungszeitraum,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Mindestbestellmenge,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Lagerort_id,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[bestellte Artikel_Bestellung-Nr],
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].Bestätigter_Termin,
				   PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Bestätigt1,
				   [PSZ_Disposition_Offene Materialbestellungen Umbuchung].[AB-Nr_Lieferant],
				[PSZ_Disposition_Offene Materialbestellungen Umbuchung].[Bestellungen_Bestellung-Nr]
				HAVING
				(((PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bezeichnung 1]) <> 'Reparatur'))
				ORDER BY PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Materialbedarf
				";


				//---------------result-----------
				string query_Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table_FA = $@"
		IF OBJECT_ID('tempdb..{Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table_FA}') IS NOT NULL DROP TABLE {Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table_FA};

                                    CREATE TABLE {Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table_FA}(
										[Fertigungsnummer] [int] NULL,
										[Artikel_Artikelnummer] [nvarchar](50) NULL,
										[Bezeichnung 1] [nvarchar](200) NULL,
										[Fertigung_Anzahl] [int] NULL,
										[Termin_Fertigstellung] [datetime] NULL,
										[Artikel-Nr des Bauteils] [int] NULL,		
										[Stücklisten_Artikelnummer] [nvarchar](250) NULL,
										[Bezeichnung des Bauteils] [nvarchar](1000) NULL,
										[Stücklisten_Anzahl] [decimal](28,13) NULL,
										[Bruttobedarf] [decimal](28,13) NULL,
										[Bestand] [decimal](28,13)NULL,
										[Termin_Materialbedarf] [datetime] NULL,
										[Lagerort_id] [int] NULL,
										[Termin_Bestätigt1] [datetime] NULL,
										Datum_Von [datetime] NULL,
										Bearbeitet [bit] Null
									);
							INSERT INTO {Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table_FA}
							SELECT PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Fertigungsnummer, PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Artikel_Artikelnummer, 
							PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bezeichnung 1], PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Fertigung_Anzahl,
							PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Fertigstellung, PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Artikel-Nr des Bauteils],
							PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Stücklisten_Artikelnummer, PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bezeichnung des Bauteils],
							PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Stücklisten_Anzahl, PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Bruttobedarf, PSZ_Disposition_Materialbestand_Umbuchung.Bestand,
							PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Materialbedarf, PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Lagerort_id,
							PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Bestätigt1, '1/1/2000' AS Datum_Von, 0 AS Bearbeitet 

							FROM ({PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung_TEMPORARY} as [PSZ_Disposition_Bruttomaterialbedarf sum m Verfügbark Umbuchung] LEFT JOIN
							{PSZ_Disposition_Bruttomaterialbedarf_Umbuchung_TEMPORARY} as PSZ_Disposition_Bruttomaterialbedarf_Umbuchung 
							ON  [PSZ_Disposition_Bruttomaterialbedarf sum m Verfügbark Umbuchung].[Artikel-Nr des Bauteils] = PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Artikel-Nr des Bauteils]) 
							LEFT JOIN {PSZ_Disposition_Materialbestand_Umbuchung_TEMPORARY} as  PSZ_Disposition_Materialbestand_Umbuchung ON PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Artikel-Nr des Bauteils] = PSZ_Disposition_Materialbestand_Umbuchung.[Artikel-Nr]
							GROUP BY PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Fertigungsnummer, PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Artikel_Artikelnummer, 
							PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bezeichnung 1], PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Fertigung_Anzahl, 
							PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Fertigstellung, PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Artikel-Nr des Bauteils], 
							PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Stücklisten_Artikelnummer, PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bezeichnung des Bauteils], 
							PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Stücklisten_Anzahl, PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Bruttobedarf, 
							PSZ_Disposition_Materialbestand_Umbuchung.Bestand, PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Materialbedarf, 
							PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Lagerort_id, PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Bestätigt1
							HAVING (((PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.[Bezeichnung 1])<>'Reparatur'))
							ORDER BY PSZ_Disposition_Bruttomaterialbedarf_Umbuchung.Termin_Materialbedarf
				";
				string query_Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Details_IV = $@"SELECT [PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details IV].[PSZ#], 
						[PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details IV].[Artikel-Nr des Bauteils], 
						[PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details IV].Termin_Bestätigt1, 
						[PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details IV].Fertigungsnummer, 
						[PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details IV].Artikel_Artikelnummer, 
						[PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details IV].[Bezeichnung 1],
						[PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details IV].Fertigung_Anzahl, 
						[PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details IV].Stücklisten_Anzahl, 
						[PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details IV].Bruttobedarf, 
						[PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details IV].SummevonBestand AS Bestand, 
						[PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details IV].Termin_Materialbedarf,
						[PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details IV].[Laufende Summe] FROM 
						(
						SELECT [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table FA].Stücklisten_Artikelnummer AS [PSZ#],
						[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table FA].[Artikel-Nr des Bauteils],
						[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table FA].Termin_Bestätigt1, 
						[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table FA].Fertigungsnummer, 
						[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table FA].Artikel_Artikelnummer,
						[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table FA].[Bezeichnung 1], 
						[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table FA].Fertigung_Anzahl,
						[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table FA].Stücklisten_Anzahl, 
						[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table FA].Bruttobedarf, 
						Sum([Psz_Disposition_Nettobedarfsermittlung Umbuchung Table FA].Bestand) AS SummevonBestand, 
						[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table FA].Termin_Materialbedarf,
						(
						Select Sum(Bruttobedarf) FROM  {Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table_FA} AS Temp 
						WHERE Temp.Stücklisten_Artikelnummer =
						[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table FA].[Stücklisten_Artikelnummer] 
						AND Temp.Termin_Materialbedarf >= Datum_Von AND Temp.Termin_Materialbedarf <= 
						[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table FA].[Termin_Materialbedarf]
						) AS [Laufende Summe]
						FROM 
						{Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table_FA} as
						[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table FA]
						GROUP BY [Psz_Disposition_Nettobedarfsermittlung Umbuchung Table FA].Stücklisten_Artikelnummer, 
						[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table FA].[Artikel-Nr des Bauteils], 
						[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table FA].Termin_Bestätigt1, 
						[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table FA].Fertigungsnummer, 
						[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table FA].Artikel_Artikelnummer, 
						[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table FA].[Bezeichnung 1], 
						[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table FA].Fertigung_Anzahl,
						[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table FA].Stücklisten_Anzahl, 
						[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table FA].Bruttobedarf, 
						[Psz_Disposition_Nettobedarfsermittlung Umbuchung Table FA].Termin_Materialbedarf
						HAVING ((([Psz_Disposition_Nettobedarfsermittlung Umbuchung Table FA].Stücklisten_Artikelnummer)='{Stucklisten_Artikelnummer}'))

						)
						as
						[PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details IV] ORDER BY 
						[PSZ_Disposition_Nettobedarfsermittlung Umbuchung Details IV].Termin_Materialbedarf;";

				// - execute queries
				using(var sqlCommand = new SqlCommand($"BEGIN TRAN ; {query_PSZ_Disposition_Bruttomaterialbedarf_Umbuchung};{query_PSZ_Disposition_Materialbestand_Umbuchung};{query__Reserviert_Menge_Umbuchung};{queryUpdate_Bestand_Material};{query_PSZ_Disposition_Bruttomaterialbedarf_summiert_Umbuchung};{query_PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung};{delete_PSZ_Disposition_Bruttomaterialbedarf_VerfugbarkUmbuchung};{query_PSZ_Disposition_Offene_Materialbestellungen_Umbuchung} ;{QueryPsz_Disposition_Nettobedarfsermittlung_Umbuchung_Table};{query_Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Table_FA};{query_Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Details_IV} ; COMMIT TRAN ;", sqlConnection))
				{
					sqlCommand.Parameters.AddWithValue("Stucklisten_Artikelnummer", Stucklisten_Artikelnummer);
					sqlCommand.Parameters.AddWithValue("bis", bis);
					sqlCommand.CommandTimeout = 0;

					DbExecution.Fill(sqlCommand, dataTable);

				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_IV_Entity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_IV_Entity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.Logistics.lagerorte> GetLagerOrt(List<int> Listlager)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT Lagerort,Lagerort_id FROM Lagerorte 
								WHERE  Lagerorte.Lagerort_id in ({string.Join(',', Listlager.ToArray())} )";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 0;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.lagerorte(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.lagerorte>();
			}
		}
		public static List<Entities.Joins.Logistics.Liste_50_ROH_Artikel_Selectione_Entity> getListe_50_ROH_Artikel_Selectione_Entity(int Lager)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT TOP 50 Lager.Lagerort_id, Lager.CCID, Lager.[Artikel-Nr], Lager.Bestand, Artikel.Artikelnummer, Artikel.[Bezeichnung 1], 
							Artikel.Stückliste, Bestellnummern.Standardlieferant, Bestellnummern.Einkaufspreis, [Bestand]*[Einkaufspreis] AS Wert, Lager.CCID_Datum, Lagerorte.Lagerort, 
							isnull(Artikel.Größe,0) AS Gewichte
							FROM ((Lager INNER JOIN Artikel ON Lager.[Artikel-Nr]=Artikel.[Artikel-Nr]) 
							INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr]=Bestellnummern.[Artikel-Nr]) INNER JOIN Lagerorte ON Lager.Lagerort_id=Lagerorte.Lagerort_id
							WHERE (((Lager.Lagerort_id) = @Lager) And 
							((Lager.CCID) = 0) And ((Lager.Bestand) <> 0) And ((Artikel.Stückliste) = 0) And ((Bestellnummern.Standardlieferant) = 1))
							ORDER BY [Bestand]*[Einkaufspreis] DESC";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Lager", Lager);
				sqlCommand.CommandTimeout = 0;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.Logistics.Liste_50_ROH_Artikel_Selectione_Entity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.Logistics.Liste_50_ROH_Artikel_Selectione_Entity>();
			}
		}
		public static int Rest_ROH_Artikel(int Lager)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT count(lager.[Artikel-Nr]) as Rest
				FROM ((Lager INNER JOIN Artikel ON Lager.[Artikel-Nr]=Artikel.[Artikel-Nr]) INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr]=Bestellnummern.[Artikel-Nr]) 
				INNER JOIN Lagerorte ON Lager.Lagerort_id=Lagerorte.Lagerort_id
				WHERE (((Lager.Lagerort_id) = @Lager) And ((Lager.CCID) = 0) And ((Lager.Bestand) <> 0) And ((Artikel.Stückliste) = 0) And ((Bestellnummern.Standardlieferant) = 1))";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Lager", Lager);
				DbExecution.Fill(sqlCommand, dataTable);
				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
			}


		}
		public static int somme_ROH_Artikel(int Lager)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT count(lager.[Artikel-Nr]) as Somme
				FROM ((Lager INNER JOIN Artikel ON Lager.[Artikel-Nr]=Artikel.[Artikel-Nr]) INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr]=Bestellnummern.[Artikel-Nr])
				INNER JOIN Lagerorte ON Lager.Lagerort_id=Lagerorte.Lagerort_id
				WHERE (((Lager.Lagerort_id) = @Lager)  And ((Lager.Bestand) <> 0) And ((Artikel.Stückliste) = 0) And ((Bestellnummern.Standardlieferant) = 1))";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Lager", Lager);
				DbExecution.Fill(sqlCommand, dataTable);
				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
			}


		}

		public static List<Entities.Joins.Logistics.Liste_50_FG_Artikel_Selectionee_Entity> getListe_50_ROH_Artikel_Selectione_CCFG_Entity(int Lager)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@" SELECT TOP 50 Lager.Lagerort_id, Lager.CCID, Lager.[Artikel-Nr], Lager.Bestand, Artikel.Artikelnummer, Artikel.[Bezeichnung 1], Artikel.Stückliste,
				 Lager.CCID_Datum, Lagerorte.Lagerort
				 FROM (Lager INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Lagerorte ON Lager.Lagerort_id = Lagerorte.Lagerort_id
				 WHERE (((Lager.Lagerort_id) = @Lager) And ((Lager.CCID) = 0) And ((Lager.Bestand) <> 0) And ((Artikel.Stückliste) = 1))
				 ORDER BY Lager.Bestand;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Lager", Lager);
				sqlCommand.CommandTimeout = 0;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.Logistics.Liste_50_FG_Artikel_Selectionee_Entity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.Logistics.Liste_50_FG_Artikel_Selectionee_Entity>();
			}
		}
		public static int Rest_ROH_CCFG_Artikel(int Lager)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select count(lager.[Artikel-Nr]) as Rest
				 FROM (Lager INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Lagerorte ON Lager.Lagerort_id = Lagerorte.Lagerort_id
				 WHERE (((Lager.Lagerort_id) = @Lager) And ((Lager.CCID) = 0) And ((Lager.Bestand) <> 0) And ((Artikel.Stückliste) = 1))";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Lager", Lager);
				DbExecution.Fill(sqlCommand, dataTable);
				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
			}


		}
		public static int somme_ROH_CCFG_Artikel(int Lager)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"
				 select count(lager.[Artikel-Nr]) as Somme
				 FROM (Lager INNER JOIN Artikel ON Lager.[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN Lagerorte ON Lager.Lagerort_id = Lagerorte.Lagerort_id
				 WHERE (((Lager.Lagerort_id) = @Lager)  And ((Lager.Bestand) <> 0) And ((Artikel.Stückliste) = 1))";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Lager", Lager);
				DbExecution.Fill(sqlCommand, dataTable);
				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
			}


		}
		public static int Reset_de_list_des_Article_CC_en_debut_dannee(int Lager, string Type, SqlConnection conncection, SqlTransaction transaction)
		{
			int results = -1;
			//using (var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			//{
			//sqlConnection.Open();
			string query1 = "";
			string query2 = "";
			if(Type == "ROH")
			{
				query1 = "from Lager inner join Artikel on Artikel.[Artikel-Nr]=Lager.[Artikel-Nr]";
				query2 = " and Artikel.Stückliste=0";
			}
			else if(Type == "FG")
			{
				query1 = "from Lager inner join Artikel on Artikel.[Artikel-Nr]=Lager.[Artikel-Nr]";
				query2 = " and Artikel.Stückliste=1";
			}
			string query = $@"UPDATE Lager SET Lager.CCID = 0 " + query1 + $@" WHERE  [Lagerort_id]={Lager} " + query2;
			var sqlCommand = new SqlCommand(query, conncection, transaction);
			//sqlCommand.Parameters.AddWithValue("IdVersand", IdVersand);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			//}

			return results;
		}
		public static List<Entities.Joins.Logistics.LieferantMitAnzahEntity> GetListeLieferantMitAnzahlWareneingang(DateTime d1, DateTime d2)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT [View_Wareneingänge für Rohmaterial komplett].Name1
                               ,Count([View_Wareneingänge für Rohmaterial komplett].Liefertermin) AS AnzahlvonLiefertermin 
                               FROM [View_Wareneingänge für Rohmaterial komplett]
                               WHERE ((([View_Wareneingänge für Rohmaterial komplett].Liefertermin)>=@d1 And ([View_Wareneingänge für Rohmaterial komplett].Liefertermin)<=@d2) AND (([View_Wareneingänge für Rohmaterial komplett].Typ)='Wareneingang')) 
                               GROUP BY [View_Wareneingänge für Rohmaterial komplett].Name1 
                               HAVING (((Count([View_Wareneingänge für Rohmaterial komplett].Liefertermin))<>0)) ORDER BY [View_Wareneingänge für Rohmaterial komplett].Name1";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("D1", d1);
				sqlCommand.Parameters.AddWithValue("D2", d2);
				sqlCommand.CommandTimeout = 0;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.Logistics.LieferantMitAnzahEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.Logistics.LieferantMitAnzahEntity>();
			}
		}
		public static List<Entities.Joins.Logistics.WareneingangLieferantDetailsEntity> GetListeWareneingangByLieferant(DateTime d1, DateTime d2, string name1)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT [View_Wareneingänge für Rohmaterial komplett].[Projekt-Nr] as ProjektNr
                                , [View_Wareneingänge für Rohmaterial komplett].Typ
                                , [View_Wareneingänge für Rohmaterial komplett].Artikelnummer
                                , Sum([View_Wareneingänge für Rohmaterial komplett].Anzahl) AS SummevonAnzahl
                                , [View_Wareneingänge für Rohmaterial komplett].Einheit
                                , [View_Wareneingänge für Rohmaterial komplett].Name1
                                , [View_Wareneingänge für Rohmaterial komplett].Liefertermin 
                                , MONTH([View_Wareneingänge für Rohmaterial komplett].Liefertermin ) as Mois
                                , year([View_Wareneingänge für Rohmaterial komplett].Liefertermin ) as annee
                                 FROM [View_Wareneingänge für Rohmaterial komplett] 
                                 GROUP BY [View_Wareneingänge für Rohmaterial komplett].[Projekt-Nr], [View_Wareneingänge für Rohmaterial komplett].Typ, 
                                 [View_Wareneingänge für Rohmaterial komplett].Artikelnummer, [View_Wareneingänge für Rohmaterial komplett].Einheit, 
                                 [View_Wareneingänge für Rohmaterial komplett].Name1, [View_Wareneingänge für Rohmaterial komplett].Liefertermin 
                                 HAVING ((([View_Wareneingänge für Rohmaterial komplett].Typ)='Wareneingang') 
                                 AND (([View_Wareneingänge für Rohmaterial komplett].Name1)=@Name1)
                                 AND (([View_Wareneingänge für Rohmaterial komplett].Liefertermin)>=@D1 And ([View_Wareneingänge für Rohmaterial komplett].Liefertermin)<=@D2))
                                 order by  [View_Wareneingänge für Rohmaterial komplett].Liefertermin ";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("D1", d1);
				sqlCommand.Parameters.AddWithValue("D2", d2);
				sqlCommand.Parameters.AddWithValue("Name1", name1);
				sqlCommand.CommandTimeout = 0;

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.Logistics.WareneingangLieferantDetailsEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.Logistics.WareneingangLieferantDetailsEntity>();
			}
		}

		// - 2024-02-06 - FormatSoftware
		public static List<Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungHeaderFormatEntity> GetFormatDataHeader(DateTime date, int lagerFrom, int lagerTo)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT DISTINCT l.ID, l.Datum, vct.Designation CountryFrom, nct.Designation CountryTo FROM [Lagerbewegungen] l 
									/* - Source Country - */
									JOIN (SELECT Lagerort_id, WerkVonId FROM Lagerorte WHERE Lagerort_id={lagerFrom}) vla on vla.Lagerort_id=p.Lager_von
									JOIN (SELECT Id, IdCompany FROM __LGT_Werke) vwe on vwe.Id=vla.WerkVonId
									JOIN (SELECT Id, CountryId FROM __STG_Company) vcp on vcp.Id=vwe.IdCompany
									JOIN (SELECT Id, Designation FROM Countries) vct on vct.Id=vcp.CountryId
									/* - Destination Country - */
									JOIN (SELECT Lagerort_id, WerkVonId FROM Lagerorte WHERE Lagerort_id={lagerTo}) nla on nla.Lagerort_id=p.Lager_nach
									JOIN (SELECT Id, IdCompany FROM __LGT_Werke) nwe on nwe.Id=nla.WerkVonId
									JOIN (SELECT Id, CountryId FROM __STG_Company) ncp on ncp.Id=nwe.IdCompany
									JOIN (SELECT Id, Designation FROM Countries) nct on nct.Id=ncp.CountryId
									WHERE l.typ='umbuchung'
									AND CAST(l.Datum AS DATE)='{date.ToString("yyyyMMdd")}'";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungHeaderFormatEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungHeaderFormatEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungHeaderFormatEntity> GetFormatDataHeader(DateTime date, string country, bool isRawMaterialTransfer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT DISTINCT l.ID, l.Datum, vct.Designation CountryFrom, nct.Designation CountryTo FROM [Lagerbewegungen] l 
									JOIN [Lagerbewegungen_Artikel] p ON p.Lagerbewegungen_id=l.ID
									/* - Source Country - */
									JOIN (SELECT Lagerort_id, WerkVonId FROM Lagerorte) vla on vla.Lagerort_id=p.Lager_von
									JOIN (SELECT Id, IdCompany FROM __LGT_Werke) vwe on vwe.Id=vla.WerkVonId
									JOIN (SELECT Id, CountryId FROM __STG_Company) vcp on vcp.Id=vwe.IdCompany
									JOIN (SELECT Id, Designation FROM Countries WHERE Designation='{(isRawMaterialTransfer ? "DE" : country)}') vct on vct.Id=vcp.CountryId
									/* - Destination Country - */
									JOIN (SELECT Lagerort_id, WerkVonId FROM Lagerorte) nla on nla.Lagerort_id=p.Lager_nach
									JOIN (SELECT Id, IdCompany FROM __LGT_Werke) nwe on nwe.Id=nla.WerkVonId
									JOIN (SELECT Id, CountryId FROM __STG_Company) ncp on ncp.Id=nwe.IdCompany
									JOIN (SELECT Id, Designation FROM Countries WHERE Designation='{(isRawMaterialTransfer ? country : "DE")}') nct on nct.Id=ncp.CountryId
									WHERE l.typ='umbuchung'
									AND CAST(l.Datum AS DATE)='{date.ToString("yyyyMMdd")}'";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 180; // - 3 min

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungHeaderFormatEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungHeaderFormatEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungHeaderFormatExtendedEntity> GetFormatDataHeaderByMonth(DateTime date, string siteName, bool isRawMaterialTransfer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT DISTINCT l.ID, l.Datum, vct.Designation CountryFrom, nct.Designation CountryTo, g.ExportUserId LogUserId, {(isRawMaterialTransfer ? "Lager_nach" : "Lager_von")} LagerId FROM [Lagerbewegungen] l 
									JOIN [Lagerbewegungen_Artikel] p ON p.Lagerbewegungen_id=l.ID
									/* - Source Country - */
									JOIN (SELECT Lagerort_id, WerkVonId FROM Lagerorte) vla on vla.Lagerort_id=p.Lager_von
									JOIN (SELECT Id, IdCompany FROM __LGT_Werke WHERE [SiteName]='{(isRawMaterialTransfer ? "VOH" : siteName)}') vwe on vwe.Id=vla.WerkVonId
									JOIN (SELECT Id, CountryId FROM __STG_Company) vcp on vcp.Id=vwe.IdCompany
									JOIN (SELECT Id, Designation FROM Countries) vct on vct.Id=vcp.CountryId
									/* - Destination Country - */
									JOIN (SELECT Lagerort_id, WerkVonId FROM Lagerorte) nla on nla.Lagerort_id=p.Lager_nach
									JOIN (SELECT Id, IdCompany FROM __LGT_Werke WHERE [SiteName]='{(isRawMaterialTransfer ? siteName : "VOH")}') nwe on nwe.Id=nla.WerkVonId
									JOIN (SELECT Id, CountryId FROM __STG_Company) ncp on ncp.Id=nwe.IdCompany
									JOIN (SELECT Id, Designation FROM Countries) nct on nct.Id=ncp.CountryId
									/* - Logs - */
									LEFT JOIN __LGT_FormatExportLog g on g.LagerBewegungId=l.ID
									WHERE l.typ='umbuchung'
									AND DATEFROMPARTS(YEAR(@transferDate), MONTH(@transferDate), 1)<=CAST(l.Datum AS DATE) AND CAST(l.Datum AS DATE)<=EOMONTH(@transferDate)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("transferDate", date);
				sqlCommand.CommandTimeout = 180; // - 3 min

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungHeaderFormatExtendedEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungHeaderFormatExtendedEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungHeaderEntity> GetFormatDataHeader(DateTime date, IEnumerable<int> lagerFrom, IEnumerable<int> lagerTo)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT DISTINCT l.* FROM [Lagerbewegungen] l 
									JOIN [Lagerbewegungen_Artikel] p ON p.Lagerbewegungen_id=l.ID
									WHERE l.typ='umbuchung' AND p.Lager_von IN ({string.Join(",", lagerFrom)}) AND p.Lager_nach IN ({string.Join(",", lagerTo)})
									AND CAST(l.Datum AS DATE)='{date.ToString("yyyyMMdd")}'";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungHeaderEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungHeaderEntity>();
			}
		}
		public static List<KeyValuePair<int, decimal>> GetArticlesUnitPrice(IEnumerable<int> articleIds)
		{
			if(articleIds?.Count() <= 0)
			{
				return new List<KeyValuePair<int, decimal>>();
			}

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT DISTINCT [Artikel-Nr], Einkaufspreis FROM Bestellnummern WHERE Standardlieferant=1 AND [Artikel-Nr] IN ({string.Join(",", articleIds)});";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, decimal>(int.TryParse(x[0].ToString(), out var _x) ? _x : 0, decimal.TryParse(x[1].ToString(), out var _y) ? _y : 0)).ToList();
			}
			else
			{
				return new List<KeyValuePair<int, decimal>>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungPositionFormatEntity> GetFormatDataBody(bool forFG, DateTime date, int lagerFrom, int lagerTo)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = forFG ?
					$@"SELECT MAX(p.id) id, MAX(p.Lagerbewegungen_id) [Lagerbewegungen_id], p.[Artikel-nr],p.[Bezeichnung 1],p.[Artikel-nr_nach],p.[Bezeichnung 1_nach],p.[Preiseinheit]
						,COALESCE(p.[Einheit],a.einheit) [Einheit]
						,SUM(ISNULL(p.[Anzahl],0)) AS [Anzahl]
						,p.[Lager_von],p.[Lager_nach]
						,p.[Fertigungsnummer]
						,a.Artikelnummer as Artikelnummer, a.Artikelnummer as artikelnummerNach, be.Einkaufspreis ArticleUnitPrice, 
						SUM(ISNULL(p.Anzahl,0)*ISNULL(be.Einkaufspreis,0)) ArticleTotalPrice, a.Warengruppe ArticleWarengruppe, 
						a.Größe/1000 ArticleWeight, ISNULL(TRY_CAST(a.Zolltarif_nr as bigint),0) ArticleCustomsNumber, a.[Bezeichnung 1] ArticleDesignation, a.Artikelnummer Artikelnummer_FG, 
						ISNULL(TRY_CAST(a.Zolltarif_nr as bigint),0) Zolltariffnummer_FG, a.Größe/1000 Gewicht_FG, a.[Artikel-Nr] AS ArtikelNr_FG, a.Ursprungsland AS Ursprungsland_FG, e.Verkaufspreis UnitPrice_FG
					FROM [Lagerbewegungen] l 
						JOIN [Lagerbewegungen_Artikel] p ON p.Lagerbewegungen_id=l.ID JOIN Artikel a on a.[Artikel-Nr]=p.[Artikel-nr]
						LEFT JOIN (SELECT DISTINCT [Artikel-Nr], Einkaufspreis FROM Bestellnummern WHERE Standardlieferant=1) be on be.[Artikel-Nr]=a.[Artikel-Nr]
						LEFT JOIN [Preisgruppen] e on e.[Artikel-Nr]=a.[Artikel-Nr]
					WHERE (l.typ='umbuchung') AND p.Lager_von={lagerFrom} AND p.Lager_nach={lagerTo}
						AND CAST(l.Datum AS DATE)='{date.ToString("yyyyMMdd")}' AND a.Warengruppe='EF'
					GROUP BY
						p.[Artikel-nr],p.[Bezeichnung 1],p.[Artikel-nr_nach],p.[Bezeichnung 1_nach],p.[Preiseinheit]
						,COALESCE(p.[Einheit],a.einheit) 
						,p.[Lager_von],p.[Lager_nach]
						,p.[Fertigungsnummer]
						,a.Artikelnummer
						,a.Artikelnummer 
						,be.Einkaufspreis
						,a.Warengruppe
						,a.Größe/1000
						,a.Zolltarif_nr
						,a.[Bezeichnung 1]
						,a.Artikelnummer
						,a.Zolltarif_nr 
						,a.Größe/1000 
						,a.[Artikel-Nr] 
						,a.Ursprungsland
						,e.Verkaufspreis UnitPrice_FG;"

					: $@"SELECT MAX(p.[ID]) ID, MAX(p.[Lagerbewegungen_id]) [Lagerbewegungen_id],p.[Artikel-nr],p.[Bezeichnung 1],p.[Artikel-nr_nach],p.[Bezeichnung 1_nach],p.[Preiseinheit]
								,COALESCE(p.[Einheit],a.einheit) [Einheit],SUM(ISNULL(p.[Anzahl],0)) AS [Anzahl],p.[Lager_von],p.[Lager_nach]
								,p.[Fertigungsnummer], a.Artikelnummer, b.Artikelnummer as artikelnummerNach,
								be.Einkaufspreis ArticleUnitPrice,SUM(ISNULL(p.Anzahl,0)*ISNULL(be.Einkaufspreis,0)) ArticleTotalPrice, 
								a.Warengruppe ArticleWarengruppe, a.Größe/1000 ArticleWeight, ISNULL(TRY_CAST(a.Zolltarif_nr as bigint),0) ArticleCustomsNumber, 
								a.[Bezeichnung 1] ArticleDesignation, a.Artikelnummer Artikelnummer_FG, ISNULL(TRY_CAST(a.Zolltarif_nr as bigint),0) Zolltariffnummer_FG, a.Größe/1000 Gewicht_FG, a.[Artikel-Nr] AS ArtikelNr_FG, a.Ursprungsland AS Ursprungsland_FG, e.Verkaufspreis UnitPrice_FG
					FROM [Lagerbewegungen] l 
						JOIN [Lagerbewegungen_Artikel] p ON p.Lagerbewegungen_id=l.ID JOIN Artikel a on a.[Artikel-Nr]=p.[Artikel-nr] join Artikel b on b.[Artikel-Nr]=p.[Artikel-nr_nach]
						LEFT JOIN (SELECT DISTINCT [Artikel-Nr], Einkaufspreis FROM Bestellnummern WHERE Standardlieferant=1) be on be.[Artikel-Nr]=a.[Artikel-Nr]
						LEFT JOIN [Preisgruppen] e on e.[Artikel-Nr]=a.[Artikel-Nr]
					WHERE (l.typ='umbuchung') AND p.Lager_von={lagerFrom} AND p.Lager_nach={lagerTo}
						AND CAST(l.Datum AS DATE)='{date.ToString("yyyyMMdd")}' AND a.Warengruppe<>'EF'
					GROUP BY
						p.[Artikel-nr],p.[Bezeichnung 1],p.[Artikel-nr_nach],p.[Bezeichnung 1_nach],p.[Preiseinheit]
						,COALESCE(p.[Einheit],a.einheit) 
						,p.[Lager_von],p.[Lager_nach]
						,p.[Fertigungsnummer]
						,a.Artikelnummer
						,b.Artikelnummer 
						,be.Einkaufspreis
						,a.Warengruppe
						,a.Größe/1000
						,a.Zolltarif_nr
						,a.[Bezeichnung 1]
						,a.Artikelnummer
						,a.Zolltarif_nr 
						,a.Größe/1000 
						,a.[Artikel-Nr] 
						,a.Ursprungsland
						,e.Verkaufspreis UnitPrice_FG;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 180; // - 3 min

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungPositionFormatEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungPositionFormatEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungPositionFormatEntity> GetFormatDataBody(bool forFG, DateTime date, string country, bool isRawMaterialTransfer)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = forFG ?
					$@"SELECT MAX(p.id) id, MAX(p.Lagerbewegungen_id) [Lagerbewegungen_id], p.[Artikel-nr],p.[Bezeichnung 1],p.[Artikel-nr_nach],p.[Bezeichnung 1_nach],p.[Preiseinheit]
						,COALESCE(p.[Einheit],a.einheit) [Einheit]
						,SUM(ISNULL(p.[Anzahl],0)) AS [Anzahl]
						,p.[Lager_von],p.[Lager_nach]
						,p.[Fertigungsnummer]
						,a.Artikelnummer as Artikelnummer, a.Artikelnummer as artikelnummerNach, be.Einkaufspreis ArticleUnitPrice, 
						SUM(ISNULL(p.Anzahl,0)*ISNULL(be.Einkaufspreis,0)) ArticleTotalPrice, a.Warengruppe ArticleWarengruppe, 
						a.Größe/1000 ArticleWeight, ISNULL(TRY_CAST(a.Zolltarif_nr as bigint),0) ArticleCustomsNumber, a.[Bezeichnung 1] ArticleDesignation, a.Artikelnummer Artikelnummer_FG, 
						ISNULL(TRY_CAST(a.Zolltarif_nr as bigint),0) Zolltariffnummer_FG, a.Größe/1000 Gewicht_FG, a.[Artikel-Nr] AS ArtikelNr_FG, a.Ursprungsland AS Ursprungsland_FG, e.Verkaufspreis UnitPrice_FG
					FROM [Lagerbewegungen] l 
						JOIN [Lagerbewegungen_Artikel] p ON p.Lagerbewegungen_id=l.ID JOIN Artikel a on a.[Artikel-Nr]=p.[Artikel-nr]
						LEFT JOIN (SELECT DISTINCT [Artikel-Nr], Einkaufspreis FROM Bestellnummern WHERE Standardlieferant=1) be on be.[Artikel-Nr]=a.[Artikel-Nr]
						LEFT JOIN [Preisgruppen] e on e.[Artikel-Nr]=a.[Artikel-Nr]
						/* - Source Country - */
						JOIN (SELECT Lagerort_id, WerkVonId FROM Lagerorte) vla on vla.Lagerort_id=p.Lager_von
						JOIN (SELECT Id, IdCompany FROM __LGT_Werke) vwe on vwe.Id=vla.WerkVonId
						JOIN (SELECT Id, CountryId FROM __STG_Company) vcp on vcp.Id=vwe.IdCompany
						JOIN (SELECT Id, Designation FROM Countries WHERE Designation='{(isRawMaterialTransfer ? "DE" : country)}') vct on vct.Id=vcp.CountryId
						/* - Destination Country - */
						JOIN (SELECT Lagerort_id, WerkVonId FROM Lagerorte) nla on nla.Lagerort_id=p.Lager_nach
						JOIN (SELECT Id, IdCompany FROM __LGT_Werke) nwe on nwe.Id=nla.WerkVonId
						JOIN (SELECT Id, CountryId FROM __STG_Company) ncp on ncp.Id=nwe.IdCompany
						JOIN (SELECT Id, Designation FROM Countries WHERE Designation='{(isRawMaterialTransfer ? country : "DE")}') nct on nct.Id=ncp.CountryId
					WHERE (l.typ='umbuchung') 
						AND CAST(l.Datum AS DATE)='{date.ToString("yyyyMMdd")}' AND a.Warengruppe='EF'
					GROUP BY
						p.[Artikel-nr],p.[Bezeichnung 1],p.[Artikel-nr_nach],p.[Bezeichnung 1_nach],p.[Preiseinheit]
						,COALESCE(p.[Einheit],a.einheit) 
						,p.[Lager_von],p.[Lager_nach]
						,p.[Fertigungsnummer]
						,a.Artikelnummer
						,a.Artikelnummer 
						,be.Einkaufspreis
						,a.Warengruppe
						,a.Größe/1000
						,a.Zolltarif_nr
						,a.[Bezeichnung 1]
						,a.Artikelnummer
						,a.Zolltarif_nr 
						,a.Größe/1000 
						,a.[Artikel-Nr] 
						,a.Ursprungsland
						,e.Verkaufspreis;"

					: $@"SELECT MAX(p.[ID]) ID, MAX(p.[Lagerbewegungen_id]) [Lagerbewegungen_id],p.[Artikel-nr],p.[Bezeichnung 1],p.[Artikel-nr_nach],p.[Bezeichnung 1_nach],p.[Preiseinheit]
								,COALESCE(p.[Einheit],a.einheit) [Einheit],SUM(ISNULL(p.[Anzahl],0)) AS [Anzahl],p.[Lager_von],p.[Lager_nach]
								,p.[Fertigungsnummer], a.Artikelnummer, b.Artikelnummer as artikelnummerNach,
								be.Einkaufspreis ArticleUnitPrice,SUM(ISNULL(p.Anzahl,0)*ISNULL(be.Einkaufspreis,0)) ArticleTotalPrice, 
								a.Warengruppe ArticleWarengruppe, a.Größe/1000 ArticleWeight, ISNULL(TRY_CAST(a.Zolltarif_nr as bigint),0) ArticleCustomsNumber, 
								a.[Bezeichnung 1] ArticleDesignation, a.Artikelnummer Artikelnummer_FG, ISNULL(TRY_CAST(a.Zolltarif_nr as bigint),0) Zolltariffnummer_FG, a.Größe/1000 Gewicht_FG, a.[Artikel-Nr] AS ArtikelNr_FG, a.Ursprungsland AS Ursprungsland_FG, e.Verkaufspreis UnitPrice_FG
					FROM [Lagerbewegungen] l 
						JOIN [Lagerbewegungen_Artikel] p ON p.Lagerbewegungen_id=l.ID JOIN Artikel a on a.[Artikel-Nr]=p.[Artikel-nr] join Artikel b on b.[Artikel-Nr]=p.[Artikel-nr_nach]
						LEFT JOIN (SELECT DISTINCT [Artikel-Nr], Einkaufspreis FROM Bestellnummern WHERE Standardlieferant=1) be on be.[Artikel-Nr]=a.[Artikel-Nr]
						LEFT JOIN [Preisgruppen] e on e.[Artikel-Nr]=a.[Artikel-Nr]
						/* - Source Country - */
						JOIN (SELECT Lagerort_id, WerkVonId FROM Lagerorte) vla on vla.Lagerort_id=p.Lager_von
						JOIN (SELECT Id, IdCompany FROM __LGT_Werke) vwe on vwe.Id=vla.WerkVonId
						JOIN (SELECT Id, CountryId FROM __STG_Company) vcp on vcp.Id=vwe.IdCompany
						JOIN (SELECT Id, Designation FROM Countries WHERE Designation='{(isRawMaterialTransfer ? "DE" : country)}') vct on vct.Id=vcp.CountryId
						/* - Destination Country - */
						JOIN (SELECT Lagerort_id, WerkVonId FROM Lagerorte) nla on nla.Lagerort_id=p.Lager_nach
						JOIN (SELECT Id, IdCompany FROM __LGT_Werke) nwe on nwe.Id=nla.WerkVonId
						JOIN (SELECT Id, CountryId FROM __STG_Company) ncp on ncp.Id=nwe.IdCompany
						JOIN (SELECT Id, Designation FROM Countries WHERE Designation='{(isRawMaterialTransfer ? country : "DE")}') nct on nct.Id=ncp.CountryId
					WHERE (l.typ='umbuchung')
						AND CAST(l.Datum AS DATE)='{date.ToString("yyyyMMdd")}' AND a.Warengruppe<>'EF'
					GROUP BY
						p.[Artikel-nr],p.[Bezeichnung 1],p.[Artikel-nr_nach],p.[Bezeichnung 1_nach],p.[Preiseinheit]
						,COALESCE(p.[Einheit],a.einheit) 
						,p.[Lager_von],p.[Lager_nach]
						,p.[Fertigungsnummer]
						,a.Artikelnummer
						,b.Artikelnummer 
						,be.Einkaufspreis
						,a.Warengruppe
						,a.Größe/1000
						,a.Zolltarif_nr
						,a.[Bezeichnung 1]
						,a.Artikelnummer
						,a.Zolltarif_nr 
						,a.Größe/1000 
						,a.[Artikel-Nr] 
						,a.Ursprungsland
						,e.Verkaufspreis;";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungPositionFormatEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungPositionFormatEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungPositionFormatEntity> GetFormatDataBodyForDay(bool forFG, DateTime date, string siteName, bool isRawMaterialTransfer, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			using(var sqlCommand = new SqlCommand("", connection, transaction))
			{
				string query = forFG ?
					$@"SELECT MAX(p.id) id, MAX(p.Lagerbewegungen_id) [Lagerbewegungen_id], p.[Artikel-nr],p.[Bezeichnung 1],p.[Artikel-nr_nach],p.[Bezeichnung 1_nach],p.[Preiseinheit]
						,COALESCE(p.[Einheit],a.einheit) [Einheit]
						,SUM(ISNULL(p.[Anzahl],0)) AS [Anzahl]
						,p.[Lager_von],p.[Lager_nach]
						,p.[Fertigungsnummer]
						,a.Artikelnummer as Artikelnummer, a.Artikelnummer as artikelnummerNach, be.Einkaufspreis ArticleUnitPrice, 
						SUM(ISNULL(p.Anzahl,0)*ISNULL(be.Einkaufspreis,0)) ArticleTotalPrice, a.Warengruppe ArticleWarengruppe, 
						a.Größe/1000 ArticleWeight, ISNULL(TRY_CAST(a.Zolltarif_nr as bigint),0) ArticleCustomsNumber, a.[Bezeichnung 1] ArticleDesignation, a.Artikelnummer Artikelnummer_FG, 
						ISNULL(TRY_CAST(a.Zolltarif_nr as bigint),0) Zolltariffnummer_FG, a.Größe/1000 Gewicht_FG, a.[Artikel-Nr] AS ArtikelNr_FG, a.Ursprungsland AS Ursprungsland_FG, e.Verkaufspreis UnitPrice_FG
					FROM [Lagerbewegungen] l 
						JOIN [Lagerbewegungen_Artikel] p ON p.Lagerbewegungen_id=l.ID JOIN Artikel a on a.[Artikel-Nr]=p.[Artikel-nr]
						LEFT JOIN (SELECT DISTINCT [Artikel-Nr], Einkaufspreis FROM Bestellnummern WHERE Standardlieferant=1) be on be.[Artikel-Nr]=a.[Artikel-Nr]
						LEFT JOIN [Preisgruppen] e on e.[Artikel-Nr]=a.[Artikel-Nr]
						/* - Source Country - */
						JOIN (SELECT Lagerort_id, WerkVonId FROM Lagerorte) vla on vla.Lagerort_id=p.Lager_von
						JOIN (SELECT Id, IdCompany FROM __LGT_Werke WHERE [SiteName]='{(isRawMaterialTransfer ? "VOH" : siteName)}') vwe on vwe.Id=vla.WerkVonId
						JOIN (SELECT Id, CountryId FROM __STG_Company) vcp on vcp.Id=vwe.IdCompany
						JOIN (SELECT Id, Designation FROM Countries) vct on vct.Id=vcp.CountryId
						/* - Destination Country - */
						JOIN (SELECT Lagerort_id, WerkVonId FROM Lagerorte) nla on nla.Lagerort_id=p.Lager_nach
						JOIN (SELECT Id, IdCompany FROM __LGT_Werke WHERE [SiteName]='{(isRawMaterialTransfer ? siteName : "VOH")}') nwe on nwe.Id=nla.WerkVonId
						JOIN (SELECT Id, CountryId FROM __STG_Company) ncp on ncp.Id=nwe.IdCompany
						JOIN (SELECT Id, Designation FROM Countries) nct on nct.Id=ncp.CountryId
					WHERE (l.typ='umbuchung') 
						AND CAST(l.Datum AS DATE)='{date.ToString("yyyyMMdd")}' AND a.Warengruppe='EF'
					GROUP BY
						p.[Artikel-nr],p.[Bezeichnung 1],p.[Artikel-nr_nach],p.[Bezeichnung 1_nach],p.[Preiseinheit]
						,COALESCE(p.[Einheit],a.einheit) 
						,p.[Lager_von],p.[Lager_nach]
						,p.[Fertigungsnummer]
						,a.Artikelnummer
						,a.Artikelnummer 
						,be.Einkaufspreis
						,a.Warengruppe
						,a.Größe/1000
						,a.Zolltarif_nr
						,a.[Bezeichnung 1]
						,a.Artikelnummer
						,a.Zolltarif_nr 
						,a.Größe/1000 
						,a.[Artikel-Nr] 
						,a.Ursprungsland
						,e.Verkaufspreis;"

					: $@"SELECT MAX(p.[ID]) ID, MAX(p.[Lagerbewegungen_id]) [Lagerbewegungen_id],p.[Artikel-nr],p.[Bezeichnung 1],p.[Artikel-nr_nach],p.[Bezeichnung 1_nach],p.[Preiseinheit]
								,COALESCE(p.[Einheit],a.einheit) [Einheit],SUM(ISNULL(p.[Anzahl],0)) AS [Anzahl],p.[Lager_von],p.[Lager_nach]
								,p.[Fertigungsnummer], a.Artikelnummer, b.Artikelnummer as artikelnummerNach,
								be.Einkaufspreis ArticleUnitPrice,SUM(ISNULL(p.Anzahl,0)*ISNULL(be.Einkaufspreis,0)) ArticleTotalPrice, 
								a.Warengruppe ArticleWarengruppe, a.Größe/1000 ArticleWeight, ISNULL(TRY_CAST(a.Zolltarif_nr as bigint),0) ArticleCustomsNumber, 
								a.[Bezeichnung 1] ArticleDesignation, a.Artikelnummer Artikelnummer_FG, ISNULL(TRY_CAST(a.Zolltarif_nr as bigint),0) Zolltariffnummer_FG, a.Größe/1000 Gewicht_FG, a.[Artikel-Nr] AS ArtikelNr_FG, a.Ursprungsland AS Ursprungsland_FG, e.Verkaufspreis UnitPrice_FG
					FROM [Lagerbewegungen] l 
						JOIN [Lagerbewegungen_Artikel] p ON p.Lagerbewegungen_id=l.ID JOIN Artikel a on a.[Artikel-Nr]=p.[Artikel-nr] join Artikel b on b.[Artikel-Nr]=p.[Artikel-nr_nach]
						LEFT JOIN [Preisgruppen] e on e.[Artikel-Nr]=a.[Artikel-Nr]
						LEFT JOIN (SELECT DISTINCT [Artikel-Nr], Einkaufspreis FROM Bestellnummern WHERE Standardlieferant=1) be on be.[Artikel-Nr]=a.[Artikel-Nr]
						/* - Source Country - */
						JOIN (SELECT Lagerort_id, WerkVonId FROM Lagerorte) vla on vla.Lagerort_id=p.Lager_von
						JOIN (SELECT Id, IdCompany FROM __LGT_Werke WHERE [SiteName]='{(isRawMaterialTransfer ? "VOH" : siteName)}') vwe on vwe.Id=vla.WerkVonId
						JOIN (SELECT Id, CountryId FROM __STG_Company) vcp on vcp.Id=vwe.IdCompany
						JOIN (SELECT Id, Designation FROM Countries) vct on vct.Id=vcp.CountryId
						/* - Destination Country - */
						JOIN (SELECT Lagerort_id, WerkVonId FROM Lagerorte) nla on nla.Lagerort_id=p.Lager_nach
						JOIN (SELECT Id, IdCompany FROM __LGT_Werke WHERE [SiteName]='{(isRawMaterialTransfer ? siteName : "VOH")}') nwe on nwe.Id=nla.WerkVonId
						JOIN (SELECT Id, CountryId FROM __STG_Company) ncp on ncp.Id=nwe.IdCompany
						JOIN (SELECT Id, Designation FROM Countries) nct on nct.Id=ncp.CountryId
					WHERE (l.typ='umbuchung')
						AND CAST(l.Datum AS DATE)='{date.ToString("yyyyMMdd")}' AND a.Warengruppe<>'EF'
					GROUP BY
						p.[Artikel-nr],p.[Bezeichnung 1],p.[Artikel-nr_nach],p.[Bezeichnung 1_nach],p.[Preiseinheit]
						,COALESCE(p.[Einheit],a.einheit) 
						,p.[Lager_von],p.[Lager_nach]
						,p.[Fertigungsnummer]
						,a.Artikelnummer
						,b.Artikelnummer 
						,be.Einkaufspreis
						,a.Warengruppe
						,a.Größe/1000
						,a.Zolltarif_nr
						,a.[Bezeichnung 1]
						,a.Artikelnummer
						,a.Zolltarif_nr 
						,a.Größe/1000 
						,a.[Artikel-Nr] 
						,a.Ursprungsland
						,e.Verkaufspreis;";

				sqlCommand.CommandText = query;
				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungPositionFormatEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungPositionFormatEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungPositionFormatEntity> GetFormatDataBodyForMonth(bool forFG, DateTime date, string siteName, bool isRawMaterialTransfer, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			using(var sqlCommand = new SqlCommand("", connection, transaction))
			{
				string query = forFG ?
					$@"SELECT MAX(p.id) id, MAX(p.Lagerbewegungen_id) [Lagerbewegungen_id], p.[Artikel-nr],p.[Bezeichnung 1],p.[Artikel-nr_nach],p.[Bezeichnung 1_nach],p.[Preiseinheit]
						,COALESCE(p.[Einheit],a.einheit) [Einheit]
						,SUM(ISNULL(p.[Anzahl],0)) AS [Anzahl]
						,p.[Lager_von],p.[Lager_nach]
						,p.[Fertigungsnummer]
						,a.Artikelnummer as Artikelnummer, a.Artikelnummer as artikelnummerNach, be.Einkaufspreis ArticleUnitPrice, 
						SUM(ISNULL(p.Anzahl,0)*ISNULL(be.Einkaufspreis,0)) ArticleTotalPrice, a.Warengruppe ArticleWarengruppe, 
						a.Größe/1000 ArticleWeight, ISNULL(TRY_CAST(a.Zolltarif_nr as bigint),0) ArticleCustomsNumber, a.[Bezeichnung 1] ArticleDesignation, a.Artikelnummer Artikelnummer_FG, 
						ISNULL(TRY_CAST(a.Zolltarif_nr as bigint),0) Zolltariffnummer_FG, a.Größe/1000 Gewicht_FG, a.[Artikel-Nr] AS ArtikelNr_FG, a.Ursprungsland AS Ursprungsland_FG, e.Verkaufspreis UnitPrice_FG
					FROM [Lagerbewegungen] l 
						JOIN [Lagerbewegungen_Artikel] p ON p.Lagerbewegungen_id=l.ID JOIN Artikel a on a.[Artikel-Nr]=p.[Artikel-nr]
						LEFT JOIN (SELECT DISTINCT [Artikel-Nr], Einkaufspreis FROM Bestellnummern WHERE Standardlieferant=1) be on be.[Artikel-Nr]=a.[Artikel-Nr]
						LEFT JOIN [Preisgruppen] e on e.[Artikel-Nr]=a.[Artikel-Nr]
						/* - Source Country - */
						JOIN (SELECT Lagerort_id, WerkVonId FROM Lagerorte) vla on vla.Lagerort_id=p.Lager_von
						JOIN (SELECT Id, IdCompany FROM __LGT_Werke WHERE [SiteName]='{(isRawMaterialTransfer ? "VOH" : siteName)}') vwe on vwe.Id=vla.WerkVonId
						JOIN (SELECT Id, CountryId FROM __STG_Company) vcp on vcp.Id=vwe.IdCompany
						JOIN (SELECT Id, Designation FROM Countries) vct on vct.Id=vcp.CountryId
						/* - Destination Country - */
						JOIN (SELECT Lagerort_id, WerkVonId FROM Lagerorte) nla on nla.Lagerort_id=p.Lager_nach
						JOIN (SELECT Id, IdCompany FROM __LGT_Werke WHERE [SiteName]='{(isRawMaterialTransfer ? siteName : "VOH")}') nwe on nwe.Id=nla.WerkVonId
						JOIN (SELECT Id, CountryId FROM __STG_Company) ncp on ncp.Id=nwe.IdCompany
						JOIN (SELECT Id, Designation FROM Countries) nct on nct.Id=ncp.CountryId
					WHERE (l.typ='umbuchung') 
						AND DATEFROMPARTS(YEAR(@transferDate), MONTH(@transferDate), 1)<= CAST(l.Datum AS DATE) AND CAST(l.Datum AS DATE)<=EOMONTH(@transferDate) AND a.Warengruppe='EF'
					GROUP BY
						p.[Artikel-nr],p.[Bezeichnung 1],p.[Artikel-nr_nach],p.[Bezeichnung 1_nach],p.[Preiseinheit]
						,COALESCE(p.[Einheit],a.einheit) 
						,p.[Lager_von],p.[Lager_nach]
						,p.[Fertigungsnummer]
						,a.Artikelnummer
						,a.Artikelnummer 
						,be.Einkaufspreis
						,a.Warengruppe
						,a.Größe/1000
						,a.Zolltarif_nr
						,a.[Bezeichnung 1]
						,a.Artikelnummer
						,a.Zolltarif_nr 
						,a.Größe/1000 
						,a.[Artikel-Nr] 
						,a.Ursprungsland
						,e.Verkaufspreis;"

					: $@"SELECT MAX(p.[ID]) ID, MAX(p.[Lagerbewegungen_id]) [Lagerbewegungen_id],p.[Artikel-nr],p.[Bezeichnung 1],p.[Artikel-nr_nach],p.[Bezeichnung 1_nach],p.[Preiseinheit]
								,COALESCE(p.[Einheit],a.einheit) [Einheit],SUM(ISNULL(p.[Anzahl],0)) AS [Anzahl],p.[Lager_von],p.[Lager_nach]
								,p.[Fertigungsnummer], a.Artikelnummer, b.Artikelnummer as artikelnummerNach,
								be.Einkaufspreis ArticleUnitPrice,SUM(ISNULL(p.Anzahl,0)*ISNULL(be.Einkaufspreis,0)) ArticleTotalPrice, 
								a.Warengruppe ArticleWarengruppe, a.Größe/1000 ArticleWeight, ISNULL(TRY_CAST(a.Zolltarif_nr as bigint),0) ArticleCustomsNumber, 
								a.[Bezeichnung 1] ArticleDesignation, a.Artikelnummer Artikelnummer_FG, ISNULL(TRY_CAST(a.Zolltarif_nr as bigint),0) Zolltariffnummer_FG, a.Größe/1000 Gewicht_FG, a.[Artikel-Nr] AS ArtikelNr_FG, a.Ursprungsland AS Ursprungsland_FG, e.Verkaufspreis UnitPrice_FG
					FROM [Lagerbewegungen] l 
						JOIN [Lagerbewegungen_Artikel] p ON p.Lagerbewegungen_id=l.ID JOIN Artikel a on a.[Artikel-Nr]=p.[Artikel-nr] join Artikel b on b.[Artikel-Nr]=p.[Artikel-nr_nach]
						LEFT JOIN (SELECT DISTINCT [Artikel-Nr], Einkaufspreis FROM Bestellnummern WHERE Standardlieferant=1) be on be.[Artikel-Nr]=a.[Artikel-Nr]
						LEFT JOIN [Preisgruppen] e on e.[Artikel-Nr]=a.[Artikel-Nr]
						/* - Source Country - */
						JOIN (SELECT Lagerort_id, WerkVonId FROM Lagerorte) vla on vla.Lagerort_id=p.Lager_von
						JOIN (SELECT Id, IdCompany FROM __LGT_Werke WHERE [SiteName]='{(isRawMaterialTransfer ? "VOH" : siteName)}') vwe on vwe.Id=vla.WerkVonId
						JOIN (SELECT Id, CountryId FROM __STG_Company) vcp on vcp.Id=vwe.IdCompany
						JOIN (SELECT Id, Designation FROM Countries) vct on vct.Id=vcp.CountryId
						/* - Destination Country - */
						JOIN (SELECT Lagerort_id, WerkVonId FROM Lagerorte) nla on nla.Lagerort_id=p.Lager_nach
						JOIN (SELECT Id, IdCompany FROM __LGT_Werke WHERE [SiteName]='{(isRawMaterialTransfer ? siteName : "VOH")}') nwe on nwe.Id=nla.WerkVonId
						JOIN (SELECT Id, CountryId FROM __STG_Company) ncp on ncp.Id=nwe.IdCompany
						JOIN (SELECT Id, Designation FROM Countries) nct on nct.Id=ncp.CountryId
					WHERE (l.typ='umbuchung')
						AND DATEFROMPARTS(YEAR(@transferDate), MONTH(@transferDate), 1)<= CAST(l.Datum AS DATE) AND CAST(l.Datum AS DATE)<=EOMONTH(@transferDate) AND a.Warengruppe<>'EF'
					GROUP BY
						p.[Artikel-nr],p.[Bezeichnung 1],p.[Artikel-nr_nach],p.[Bezeichnung 1_nach],p.[Preiseinheit]
						,COALESCE(p.[Einheit],a.einheit) 
						,p.[Lager_von],p.[Lager_nach]
						,p.[Fertigungsnummer]
						,a.Artikelnummer
						,b.Artikelnummer 
						,be.Einkaufspreis
						,a.Warengruppe
						,a.Größe/1000
						,a.Zolltarif_nr
						,a.[Bezeichnung 1]
						,a.Artikelnummer
						,a.Zolltarif_nr 
						,a.Größe/1000 
						,a.[Artikel-Nr] 
						,a.Ursprungsland
						,e.Verkaufspreis;";
				sqlCommand.CommandText = query;
				sqlCommand.Parameters.AddWithValue("transferDate", date);
				sqlCommand.CommandTimeout = 180; // - 3 min

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungPositionFormatEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungPositionFormatEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungPositionEntity> GetFormatDataExport(DateTime date, int lagerFrom, int lagerTo)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT p.[ID],p.[Lagerbewegungen_id],p.[Artikel-nr],p.[Bezeichnung 1],p.[Artikel-nr_nach],p.[Bezeichnung 1_nach],p.[Preiseinheit]
								,COALESCE(p.[Einheit],a.einheit) [Einheit],p.[AnfangLagerBestand],p.[Anzahl],p.[EndeLagerBestand],p.[Lager_von],p.[Lager_nach],p.[Anzahl_nach]
								,p.[Fertigungsnummer],p.[Grund],p.[Bemerkung],p.[Löschen],p.[upsize_ts],p.[Gebucht von],p.[STRG_SCAN],p.[Karton_ID]
								,p.[Umlaufartikel],p.[Rollennummer],p.[ID_Schneiderei], a.Artikelnummer, b.Artikelnummer as artikelnummerNach, null as Datum, 
								be.Einkaufspreis ArticleUnitPrice,p.Anzahl*be.Einkaufspreis ArticleTotalPrice, 
								a.Warengruppe ArticleWarengruppe, a.Größe/1000 ArticleWeight, a.Zolltarif_nr ArticleCustomsNumber, 
								a.[Bezeichnung 1] ArticleDesignation, a.Artikelnummer Artikelnummer_FG, a.Zolltarif_nr Zolltariffnummer_FG, a.Größe/1000 Gewicht_FG, a.[Artikel-Nr] AS ArtikelNr_FG, a.Ursprungsland AS Ursprungsland_FG, e.Verkaufspreis UnitPrice_FG;
					FROM [Lagerbewegungen] l 
						JOIN [Lagerbewegungen_Artikel] p ON p.Lagerbewegungen_id=l.ID JOIN Artikel a on a.[Artikel-Nr]=p.[Artikel-nr] join Artikel b on b.[Artikel-Nr]=p.[Artikel-nr_nach]
						LEFT JOIN (SELECT DISTINCT [Artikel-Nr], Einkaufspreis FROM Bestellnummern WHERE Standardlieferant=1) be on be.[Artikel-Nr]=a.[Artikel-Nr]
						LEFT JOIN [Preisgruppen] e on e.[Artikel-Nr]=a.[Artikel-Nr]
					WHERE (l.typ='umbuchung') AND p.Lager_von={lagerFrom} AND p.Lager_nach={lagerTo}
						AND CAST(l.Datum AS DATE)='{date.ToString("yyyyMMdd")}';";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungPositionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.LagerbewegungPositionEntity>();
			}
		}
		public static int SaveLogsBySiteForDay(int userId, string userName, string siteName, DateTime date, bool isRawMaterialTransfer, SqlConnection connection, SqlTransaction transaction)
		{
			string query = $@"INSERT INTO [__LGT_FormatExportLog] ([ExportDate],[ExportUserId],[ExportUserName],[LagerBewegungId],[SelectedDate],[SelectedLagerFrom],[SelectedLagerTo]) OUTPUT INSERTED.[Id] 
					SELECT DISTINCT GETDATE() AS [ExportDate], @ExportUserId AS [ExportUserId], @ExportUserName AS [ExportUserName], p.Lagerbewegungen_id AS [LagerBewegungId],
						CAST(l.Datum AS DATE) AS [SelectedDate], p.[Lager_von] AS [SelectedLagerFrom], p.[Lager_nach] AS [SelectedLagerTo]
					FROM [Lagerbewegungen] l 
						JOIN [Lagerbewegungen_Artikel] p ON p.Lagerbewegungen_id=l.ID JOIN Artikel a on a.[Artikel-Nr]=p.[Artikel-nr] join Artikel b on b.[Artikel-Nr]=p.[Artikel-nr_nach]
						LEFT JOIN (SELECT DISTINCT [Artikel-Nr], Einkaufspreis FROM Bestellnummern WHERE Standardlieferant=1) be on be.[Artikel-Nr]=a.[Artikel-Nr]
						/* - Source Country - */
						JOIN (SELECT Lagerort_id, WerkVonId FROM Lagerorte) vla on vla.Lagerort_id=p.Lager_von
						JOIN (SELECT Id, IdCompany FROM __LGT_Werke WHERE [SiteName]='{(isRawMaterialTransfer ? "VOH" : siteName)}') vwe on vwe.Id=vla.WerkVonId
						JOIN (SELECT Id, CountryId FROM __STG_Company) vcp on vcp.Id=vwe.IdCompany
						JOIN (SELECT Id, Designation FROM Countries) vct on vct.Id=vcp.CountryId
						/* - Destination Country - */
						JOIN (SELECT Lagerort_id, WerkVonId FROM Lagerorte) nla on nla.Lagerort_id=p.Lager_nach
						JOIN (SELECT Id, IdCompany FROM __LGT_Werke WHERE [SiteName]='{(isRawMaterialTransfer ? siteName : "VOH")}') nwe on nwe.Id=nla.WerkVonId
						JOIN (SELECT Id, CountryId FROM __STG_Company) ncp on ncp.Id=nwe.IdCompany
						JOIN (SELECT Id, Designation FROM Countries) nct on nct.Id=ncp.CountryId
					WHERE (l.typ='umbuchung')
						AND CAST(l.Datum AS DATE)='{date.ToString("yyyyMMdd")}';";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ExportUserId", userId);
			sqlCommand.Parameters.AddWithValue("ExportUserName", userName);
			sqlCommand.CommandTimeout = 180; // - 3 min

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int SaveLogsBySiteForMonth(int userId, string userName, string siteName, DateTime date, bool isRawMaterialTransfer, SqlConnection connection, SqlTransaction transaction)
		{
			string query = $@"INSERT INTO [__LGT_FormatExportLog] ([ExportDate],[ExportUserId],[ExportUserName],[LagerBewegungId],[SelectedDate],[SelectedLagerFrom],[SelectedLagerTo]) OUTPUT INSERTED.[Id] 
					SELECT DISTINCT GETDATE() AS [ExportDate], @ExportUserId AS [ExportUserId], @ExportUserName AS [ExportUserName], p.Lagerbewegungen_id AS [LagerBewegungId],
						CAST(l.Datum AS DATE) AS [SelectedDate], p.[Lager_von] AS [SelectedLagerFrom], p.[Lager_nach] AS [SelectedLagerTo]
					FROM [Lagerbewegungen] l 
						JOIN [Lagerbewegungen_Artikel] p ON p.Lagerbewegungen_id=l.ID JOIN Artikel a on a.[Artikel-Nr]=p.[Artikel-nr] join Artikel b on b.[Artikel-Nr]=p.[Artikel-nr_nach]
						LEFT JOIN (SELECT DISTINCT [Artikel-Nr], Einkaufspreis FROM Bestellnummern WHERE Standardlieferant=1) be on be.[Artikel-Nr]=a.[Artikel-Nr]
						/* - Source Country - */
						JOIN (SELECT Lagerort_id, WerkVonId FROM Lagerorte) vla on vla.Lagerort_id=p.Lager_von
						JOIN (SELECT Id, IdCompany FROM __LGT_Werke WHERE [SiteName]='{(isRawMaterialTransfer ? "VOH" : siteName)}') vwe on vwe.Id=vla.WerkVonId
						JOIN (SELECT Id, CountryId FROM __STG_Company) vcp on vcp.Id=vwe.IdCompany
						JOIN (SELECT Id, Designation FROM Countries) vct on vct.Id=vcp.CountryId
						/* - Destination Country - */
						JOIN (SELECT Lagerort_id, WerkVonId FROM Lagerorte) nla on nla.Lagerort_id=p.Lager_nach
						JOIN (SELECT Id, IdCompany FROM __LGT_Werke WHERE [SiteName]='{(isRawMaterialTransfer ? siteName : "VOH")}') nwe on nwe.Id=nla.WerkVonId
						JOIN (SELECT Id, CountryId FROM __STG_Company) ncp on ncp.Id=nwe.IdCompany
						JOIN (SELECT Id, Designation FROM Countries) nct on nct.Id=ncp.CountryId
					WHERE (l.typ='umbuchung')
						AND DATEFROMPARTS(YEAR(@transferDate), MONTH(@transferDate), 1)<= CAST(l.Datum AS DATE) AND CAST(l.Datum AS DATE)<=EOMONTH(@transferDate);";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ExportUserId", userId);
			sqlCommand.Parameters.AddWithValue("ExportUserName", userName);
			sqlCommand.Parameters.AddWithValue("transferDate", date);
			sqlCommand.CommandTimeout = 180; // - 3 min

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}

		public static List<Infrastructure.Data.Entities.Joins.Logistics.LagerbewegungEntity> GetFormatDataRecent_ROH()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"/* ROH */
									SELECT x.Lager LagerFrom, x.TransferDate, w.Id SiteId, w.SiteName, p.Lager_nach LagerTo FROM 
									(SELECT DISTINCT p.Lager_von Lager, MAX(l.Datum) TransferDate, MAX(p.Id) Id FROM [Lagerbewegungen] l 
										JOIN [Lagerbewegungen_Artikel] p ON p.Lagerbewegungen_id=l.ID
										WHERE l.typ='umbuchung' 
										AND p.Lager_von IN (59, 10, 40, 25, 104) 
										AND p.Lager_nach IN (63, 64, 65, 23, 29, 30, 46, 47, 49, 35, 34, 107, 108, 109)	
										GROUP BY p.Lager_von) x
									LEFT JOIN Lagerorte l on l.Lagerort_id=x.Lager
									LEFT JOIN __LGT_Werke w on w.Id=l.WerkNachId
									JOIN [Lagerbewegungen_Artikel] p on p.ID=x.Id
";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 0;

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.LagerbewegungEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.LagerbewegungEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.Logistics.LagerbewegungEntity> GetFormatDataRecent_FG()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"
								/* FG */
								SELECT x.Lager LagerTo, x.TransferDate, w.Id SiteId, w.SiteName, p.Lager_von LagerFrom FROM 
								(SELECT DISTINCT p.Lager_nach Lager, MAX(l.Datum) TransferDate, MAX(p.Id) Id FROM [Lagerbewegungen] l 
												JOIN [Lagerbewegungen_Artikel] p ON p.Lagerbewegungen_id=l.ID
												WHERE l.typ='umbuchung' 
												AND p.Lager_nach IN (12,14, 32, 44, 45, 16, 31, 43, 85, 86, 110, 111, 112, 113, 114, 115, 27, 33, 36, 37, 11)	
												GROUP BY p.Lager_nach) x
								LEFT JOIN Lagerorte l on l.Lagerort_id=x.Lager
								LEFT JOIN __LGT_Werke w on w.Id=l.WerkVonId
								JOIN [Lagerbewegungen_Artikel] p on p.ID=x.Id
";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 0;

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.LagerbewegungEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.LagerbewegungEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.Logistics.LagerbewegungEntity> GetFormatDataRecent()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"WITH RecentTransfers AS (
									SELECT 
										p.Lager_von, 
										p.Lager_nach, 
										MAX(l.Datum) AS TransferDate, 
										MAX(p.Id) AS Id
									FROM Lagerbewegungen l
									JOIN Lagerbewegungen_Artikel p ON p.Lagerbewegungen_id = l.ID
									WHERE 
										l.typ = 'umbuchung' 
										AND p.Lager_von IS NOT NULL 
										AND p.Lager_von > 0 
										AND p.Lager_nach IS NOT NULL 
										AND p.Lager_nach > 0 
										AND p.Lager_von <> p.Lager_nach 
										AND l.Datum >= DATEADD(DAY, 1, EOMONTH(GETDATE(), -2))
									GROUP BY 
										p.Lager_von, 
										p.Lager_nach
								)
								SELECT 
									x.Lager_von AS LagerFrom,
									x.Lager_nach AS LagerTo,
									x.TransferDate,
									w.Id AS SiteFromId,
									w.SiteName AS SiteFromName,
									v.Id AS SiteToId,
									v.SiteName AS SiteToName
								FROM RecentTransfers x
								JOIN Lagerbewegungen_Artikel p ON p.ID = x.Id
								JOIN Lagerorte l ON l.Lagerort_id = x.Lager_von AND l.WerkVonId IS NOT NULL/* AND l.WerkNachId IS NOT NULL*/
								LEFT JOIN __LGT_Werke w ON w.Id = l.WerkVonId
								JOIN Lagerorte k ON k.Lagerort_id = x.Lager_nach AND k.WerkVonId IS NOT NULL/* AND k.WerkNachId IS NOT NULL*/
								LEFT JOIN __LGT_Werke v ON v.Id = k.WerkVonId
								WHERE ISNULL(w.Country, '') <> ISNULL(v.Country, '');
";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 0;

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.LagerbewegungEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.LagerbewegungEntity>();
			}
		}
		public static List<KeyValuePair<int, string>> GetFormatTransferLagers()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT DISTINCT Lagerort_id, Lagerort FROM Lagerorte Where Lagerort LIKE 'Transfer%' OR Lagerort LIKE 'eingang%' OR Lagerort LIKE 'Luftf%' ORDER BY Lagerort_id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, string>(int.TryParse(x["Lagerort_id"].ToString(), out var _x) ? _x : 0, x["Lagerort"].ToString())).ToList();
			}
			else
			{
				return new List<KeyValuePair<int, string>>();
			}
		}
		public static List<KeyValuePair<int, string>> GetFormatVOHLagers()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT DISTINCT l.Lagerort_id, l.Lagerort FROM Lagerorte l join __LGT_Werke w on w.Id=l.WerkVonId WHERE w.Country='de'";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, string>(int.TryParse(x["Lagerort_id"].ToString(), out var _x) ? _x : 0, x["Lagerort"].ToString())).ToList();
			}
			else
			{
				return new List<KeyValuePair<int, string>>();
			}
		}
		public static List<KeyValuePair<int, string>> GetFormatLagersBySite(string siteName)
		{
			if(string.IsNullOrWhiteSpace(siteName))
			{
				return null;
			}
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT DISTINCT l.Lagerort_id, l.Lagerort FROM Lagerorte l join __LGT_Werke w on w.Id=l.WerkVonId WHERE w.SiteName='{siteName}'";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, string>(int.TryParse(x["Lagerort_id"].ToString(), out var _x) ? _x : 0, x["Lagerort"].ToString())).ToList();
			}
			else
			{
				return new List<KeyValuePair<int, string>>();
			}
		}
	}

}
