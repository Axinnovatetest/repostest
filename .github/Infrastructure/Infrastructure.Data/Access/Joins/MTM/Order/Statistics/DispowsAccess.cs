using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Joins.MTM.Order.Statistics
{
	public class DispoAccess
	{
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120Entity> Getws90(Settings.PaginModel paging = null, Settings.SortingModel sortingModel = null, string ArtikelNummer = "", int all = 0)
		{
			var dataTable = new DataTable();

			string paginationFilter = "";
			string sortingField = "Name1, Stücklisten_Artikelnummer";
			string sortingdesc = ((sortingModel is not null) && sortingModel.SortDesc) ? " Desc " : " asc ";
			string artikelnummerfilter = " ";
			if(sortingModel is not null)
			{
				if(!string.IsNullOrWhiteSpace(sortingModel.SortFieldName))
				{
					sortingField = sortingModel.SortFieldName;
				}
			}

			if(paginationFilter is not null && all == 1)
			{
				paginationFilter = "";
			}
			else if(paginationFilter is not null)
			{
				paginationFilter = $"OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY";
			}
			if(ArtikelNummer is not null && !string.IsNullOrWhiteSpace(ArtikelNummer))
			{
				artikelnummerfilter = @$"where Stücklisten_Artikelnummer like '{ArtikelNummer.SqlEscape()}%'";
			}
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
						with [Psz_disposition Anlayse I KHTN] as (
						SELECT 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Artikelnummer_stücklisten AS Stücklisten_Artikelnummer,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Fertigungsnummer,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Bestand,
							Lagerorte.Lagerort_id,
							Lagerorte.Lagerort, 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Bruttobedarf,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Termin_Materialbedarf,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Standardlieferant,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Name1,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].bearbeitet,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Mindestbestellmenge,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Rahmen,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].[Rahmen-Nr],
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Rahmenmenge,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Rahmenauslauf,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Termin_Bestätigt1
						FROM Lagerorte INNER JOIN [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN] ON Lagerorte.Lagerort_id = [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Lagerort_id
						GROUP BY [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Artikelnummer_stücklisten, 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Fertigungsnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Bestand
							, Lagerorte.Lagerort_id, Lagerorte.Lagerort
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Bruttobedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Termin_Materialbedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Standardlieferant
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Name1
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].bearbeitet
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Mindestbestellmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Rahmen
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].[Rahmen-Nr]
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Rahmenmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Rahmenauslauf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Termin_Bestätigt1
						HAVING ((([Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Artikelnummer_stücklisten)<>'reparatur') 
						AND (([Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Standardlieferant)=1))
						--ORDER BY [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Artikelnummer_stücklisten
						)
						, Lager_Obsolet as (
							SELECT Lager.[Artikel-Nr], Lager.Lagerort_id, Lager.Bestand
							FROM Lager
							WHERE (((Lager.Lagerort_id)=22) AND ((Lager.Bestand)>0))
						)

						, [PSz_disposition_Anlayse II KHTN] as (
								SELECT 
								[Psz_disposition Anlayse I KHTN].Name1,
								[Psz_disposition Anlayse I KHTN].Stücklisten_Artikelnummer,
								Sum([Psz_disposition Anlayse I KHTN].Bruttobedarf) AS SummevonBruttobedarf,
								[Psz_disposition Anlayse I KHTN].Lagerort_id,
								[Psz_disposition Anlayse I KHTN].Lagerort,
								[Psz_disposition Anlayse I KHTN].Bestand,
								Max([Psz_disposition Anlayse I KHTN].Termin_Materialbedarf) AS MaxvonTermin_Materialbedarf,
								Artikel.[Bezeichnung 1] AS [Bezeichnung des Bauteils],
								[Psz_disposition Anlayse I KHTN].bearbeitet,
								[Psz_disposition Anlayse I KHTN].Mindestbestellmenge,
								Artikel.Rahmen, Artikel.[Rahmen-Nr],
								Artikel.Rahmenmenge,
								Artikel.[Artikel-Nr],
								Artikel.Rahmenauslauf
						,Lager_Obsolet.Bestand AS Betand_Obsolete
						FROM ([Psz_disposition Anlayse I KHTN] INNER JOIN Artikel ON [Psz_disposition Anlayse I KHTN].Stücklisten_Artikelnummer = Artikel.Artikelnummer) LEFT JOIN Lager_Obsolet ON Artikel.[Artikel-Nr] = Lager_Obsolet.[Artikel-Nr]
						GROUP BY [Psz_disposition Anlayse I KHTN].Name1, [Psz_disposition Anlayse I KHTN].Stücklisten_Artikelnummer,
						Artikel.[Artikel-Nr],[Psz_disposition Anlayse I KHTN].Lagerort_id, [Psz_disposition Anlayse I KHTN].Lagerort, [Psz_disposition Anlayse I KHTN].Bestand, Artikel.[Bezeichnung 1], [Psz_disposition Anlayse I KHTN].bearbeitet, [Psz_disposition Anlayse I KHTN].Mindestbestellmenge, Artikel.Rahmen, Artikel.[Rahmen-Nr], Artikel.Rahmenmenge, Artikel.Rahmenauslauf, Lager_Obsolet.Bestand
						)

						select 
								daws.Name1
								,daws.[Artikel-Nr]
								,daws.Stücklisten_Artikelnummer
								,daws.[Bezeichnung des Bauteils]
								,daws.MaxvonTermin_Materialbedarf 
								,daws.SummevonBruttobedarf
								,daws.Bestand
								,daws.Bestand - daws.SummevonBruttobedarf Differenz
								,daws.Mindestbestellmenge
								,daws.Lagerort
								,daws.Lagerort_id
								,daws.[Rahmen-Nr]
								,daws.Rahmenmenge
								,daws.Rahmenauslauf
								,iif(daws.Betand_Obsolete>0,'true','false') obsolet
								, Count(*) Over() as TotalCount 
						from [PSz_disposition_Anlayse II KHTN]  daws
						{artikelnummerfilter}
						ORDER BY {sortingField}  {sortingdesc}
                      {paginationFilter}";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120Entity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120Entity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120Entity> GetGZ90(Settings.PaginModel paging = null, Settings.SortingModel sortingModel = null, string ArtikelNummer = "", int all = 0)
		{
			var dataTable = new DataTable();

			string paginationFilter = "";
			string sortingField = "Name1, Stücklisten_Artikelnummer";
			string sortingdesc = ((sortingModel is not null) && sortingModel.SortDesc) ? " Desc " : " asc ";
			string artikelnummerfilter = " ";
			if(sortingModel is not null)
			{
				if(!string.IsNullOrWhiteSpace(sortingModel.SortFieldName))
				{
					sortingField = sortingModel.SortFieldName;
				}
			}
			if(paginationFilter is not null && all == 1)
			{
				paginationFilter = "";
			}
			else if(paginationFilter is not null)
			{
				paginationFilter = $"OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY";
			}
			if(ArtikelNummer is not null && !string.IsNullOrWhiteSpace(ArtikelNummer))
			{
				artikelnummerfilter = @$"where Stücklisten_Artikelnummer like '{ArtikelNummer.SqlEscape()}%'";
			}
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
						with [Psz_disposition Anlayse I GZTN] as (
						SELECT 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Artikelnummer_stücklisten AS Stücklisten_Artikelnummer,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Fertigungsnummer,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Bestand,
							Lagerorte.Lagerort_id,
							Lagerorte.Lagerort, 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Bruttobedarf,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Termin_Materialbedarf,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Standardlieferant,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Name1,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].bearbeitet,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Mindestbestellmenge,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Rahmen,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].[Rahmen-Nr],
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Rahmenmenge,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Rahmenauslauf,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Termin_Bestätigt1
						FROM Lagerorte INNER JOIN [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN] ON Lagerorte.Lagerort_id = [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Lagerort_id
						GROUP BY [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Artikelnummer_stücklisten, 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Fertigungsnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Bestand
							, Lagerorte.Lagerort_id, Lagerorte.Lagerort
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Bruttobedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Termin_Materialbedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Standardlieferant
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Name1
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].bearbeitet
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Mindestbestellmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Rahmen
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].[Rahmen-Nr]
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Rahmenmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Rahmenauslauf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Termin_Bestätigt1
						HAVING ((([Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Artikelnummer_stücklisten)<>'reparatur') 
						AND (([Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Standardlieferant)=1))
						)
						, Lager_Obsolet as (
							SELECT Lager.[Artikel-Nr], Lager.Lagerort_id, Lager.Bestand
							FROM Lager
							WHERE (((Lager.Lagerort_id)=22) AND ((Lager.Bestand)>0))
						)

						, [PSz_disposition_Anlayse II GZTN] as (
								SELECT 
								[Psz_disposition Anlayse I GZTN].Name1,
								[Psz_disposition Anlayse I GZTN].Stücklisten_Artikelnummer,
								Sum([Psz_disposition Anlayse I GZTN].Bruttobedarf) AS SummevonBruttobedarf,
								[Psz_disposition Anlayse I GZTN].Lagerort_id,
								[Psz_disposition Anlayse I GZTN].Lagerort,
								[Psz_disposition Anlayse I GZTN].Bestand,
								Max([Psz_disposition Anlayse I GZTN].Termin_Materialbedarf) AS MaxvonTermin_Materialbedarf,
								Artikel.[Bezeichnung 1] AS [Bezeichnung des Bauteils],
								[Psz_disposition Anlayse I GZTN].bearbeitet,
								[Psz_disposition Anlayse I GZTN].Mindestbestellmenge,
								Artikel.Rahmen, Artikel.[Rahmen-Nr],
								Artikel.Rahmenmenge,
								Artikel.[Artikel-Nr],
								Artikel.Rahmenauslauf
						,Lager_Obsolet.Bestand AS Betand_Obsolete
						FROM ([Psz_disposition Anlayse I GZTN] INNER JOIN Artikel ON [Psz_disposition Anlayse I GZTN].Stücklisten_Artikelnummer = Artikel.Artikelnummer) LEFT JOIN Lager_Obsolet ON Artikel.[Artikel-Nr] = Lager_Obsolet.[Artikel-Nr]
						GROUP BY [Psz_disposition Anlayse I GZTN].Name1,Artikel.[Artikel-Nr], [Psz_disposition Anlayse I GZTN].Stücklisten_Artikelnummer, [Psz_disposition Anlayse I GZTN].Lagerort_id, [Psz_disposition Anlayse I GZTN].Lagerort, [Psz_disposition Anlayse I GZTN].Bestand, Artikel.[Bezeichnung 1], [Psz_disposition Anlayse I GZTN].bearbeitet, [Psz_disposition Anlayse I GZTN].Mindestbestellmenge, Artikel.Rahmen, Artikel.[Rahmen-Nr], Artikel.Rahmenmenge, Artikel.Rahmenauslauf, Lager_Obsolet.Bestand
						)

						select 
								daws.Name1
								,daws.[Artikel-Nr]
								,daws.Stücklisten_Artikelnummer
								,daws.[Bezeichnung des Bauteils]
								,daws.MaxvonTermin_Materialbedarf 
								,daws.SummevonBruttobedarf
								,daws.Bestand
								,daws.Bestand - daws.SummevonBruttobedarf Differenz
								,daws.Mindestbestellmenge
								,daws.Lagerort
								,daws.Lagerort_id
								,daws.[Rahmen-Nr]
								,daws.Rahmenmenge
								,daws.Rahmenauslauf
								,iif(daws.Betand_Obsolete>0,'true','false') obsolet
								, Count(*) Over() as TotalCount 
						from [PSz_disposition_Anlayse II GZTN]  daws
						{artikelnummerfilter}
						ORDER BY {sortingField}  {sortingdesc}
                      {paginationFilter}";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120Entity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120Entity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120Entity> GetGZ40(Settings.PaginModel paging = null, Settings.SortingModel sortingModel = null, string ArtikelNummer = "", int all = 0)
		{
			var dataTable = new DataTable();

			string paginationFilter = "";
			string sortingField = "Name1, Stücklisten_Artikelnummer";
			string sortingdesc = ((sortingModel is not null) && sortingModel.SortDesc) ? " Desc " : " asc ";
			string artikelnummerfilter = " ";
			if(sortingModel is not null)
			{
				if(!string.IsNullOrWhiteSpace(sortingModel.SortFieldName))
				{
					sortingField = sortingModel.SortFieldName;
				}
			}
			if(sortingModel is not null)
			{
				if(!string.IsNullOrWhiteSpace(sortingModel.SortFieldName))
				{
					sortingField = sortingModel.SortFieldName;
				}
			}

			if(paginationFilter is not null && all == 1)
			{
				paginationFilter = "";
			}
			else if(paginationFilter is not null)
			{
				paginationFilter = $"OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY";
			}
			if(ArtikelNummer is not null && !string.IsNullOrWhiteSpace(ArtikelNummer))
			{
				artikelnummerfilter = @$"where Stücklisten_Artikelnummer like '{ArtikelNummer.SqlEscape()}%'";
			}
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
						with [Psz_disposition Anlayse I GZTN40] as (
						SELECT 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Artikelnummer_stücklisten AS Stücklisten_Artikelnummer,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Fertigungsnummer,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Bestand,
							Lagerorte.Lagerort_id,
							Lagerorte.Lagerort, 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Bruttobedarf,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Termin_Materialbedarf,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Standardlieferant,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Name1,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].bearbeitet,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Mindestbestellmenge,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Rahmen,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].[Rahmen-Nr],
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Rahmenmenge,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Rahmenauslauf,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Termin_Bestätigt1
						FROM Lagerorte INNER JOIN [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40] ON Lagerorte.Lagerort_id = [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Lagerort_id
						GROUP BY [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Artikelnummer_stücklisten, 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Fertigungsnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Bestand
							, Lagerorte.Lagerort_id, Lagerorte.Lagerort
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Bruttobedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Termin_Materialbedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Standardlieferant
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Name1
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].bearbeitet
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Mindestbestellmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Rahmen
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].[Rahmen-Nr]
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Rahmenmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Rahmenauslauf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Termin_Bestätigt1
						HAVING ((([Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Artikelnummer_stücklisten)<>'reparatur') 
						AND (([Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Standardlieferant)=1))
						)
						, Lager_Obsolet as (
							SELECT Lager.[Artikel-Nr], Lager.Lagerort_id, Lager.Bestand
							FROM Lager
							WHERE (((Lager.Lagerort_id)=22) AND ((Lager.Bestand)>0))
						)

						, [PSz_disposition_Anlayse II GZTN40] as (
								SELECT 
								[Psz_disposition Anlayse I GZTN40].Name1,
								[Psz_disposition Anlayse I GZTN40].Stücklisten_Artikelnummer,
								Sum([Psz_disposition Anlayse I GZTN40].Bruttobedarf) AS SummevonBruttobedarf,
								[Psz_disposition Anlayse I GZTN40].Lagerort_id,
								[Psz_disposition Anlayse I GZTN40].Lagerort,
								[Psz_disposition Anlayse I GZTN40].Bestand,
								Max([Psz_disposition Anlayse I GZTN40].Termin_Materialbedarf) AS MaxvonTermin_Materialbedarf,
								Artikel.[Bezeichnung 1] AS [Bezeichnung des Bauteils],
								[Psz_disposition Anlayse I GZTN40].bearbeitet,
								[Psz_disposition Anlayse I GZTN40].Mindestbestellmenge,
								Artikel.Rahmen, Artikel.[Rahmen-Nr],
								Artikel.Rahmenmenge,
								Artikel.[Artikel-Nr],
								Artikel.Rahmenauslauf
						,Lager_Obsolet.Bestand AS Betand_Obsolete
						FROM ([Psz_disposition Anlayse I GZTN40] INNER JOIN Artikel ON [Psz_disposition Anlayse I GZTN40].Stücklisten_Artikelnummer = Artikel.Artikelnummer) LEFT JOIN Lager_Obsolet ON Artikel.[Artikel-Nr] = Lager_Obsolet.[Artikel-Nr]
						GROUP BY [Psz_disposition Anlayse I GZTN40].Name1,Artikel.[Artikel-Nr], [Psz_disposition Anlayse I GZTN40].Stücklisten_Artikelnummer, [Psz_disposition Anlayse I GZTN40].Lagerort_id, [Psz_disposition Anlayse I GZTN40].Lagerort, [Psz_disposition Anlayse I GZTN40].Bestand, Artikel.[Bezeichnung 1], [Psz_disposition Anlayse I GZTN40].bearbeitet, [Psz_disposition Anlayse I GZTN40].Mindestbestellmenge, Artikel.Rahmen, Artikel.[Rahmen-Nr], Artikel.Rahmenmenge, Artikel.Rahmenauslauf, Lager_Obsolet.Bestand
						)

						select 
								daws.Name1
								,daws.[Artikel-Nr]
								,daws.Stücklisten_Artikelnummer
								,daws.[Bezeichnung des Bauteils]
								,daws.MaxvonTermin_Materialbedarf 
								,daws.SummevonBruttobedarf
								,daws.Bestand
								,daws.Bestand - daws.SummevonBruttobedarf Differenz
								,daws.Mindestbestellmenge
								,daws.Lagerort
								,daws.Lagerort_id
								,daws.[Rahmen-Nr]
								,daws.Rahmenmenge
								,daws.Rahmenauslauf
								,iif(daws.Betand_Obsolete>0,'true','false') obsolet
								, Count(*) Over() as TotalCount 
						from [PSz_disposition_Anlayse II GZTN40]  daws
						{artikelnummerfilter}
						ORDER BY {sortingField}  {sortingdesc}
                      {paginationFilter}";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120Entity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120Entity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120Entity> GetTN90(Settings.PaginModel paging, Settings.SortingModel sortingModel, string ArtikelNummer, int all = 0)
		{
			var dataTable = new DataTable();

			string paginationFilter = "";
			string sortingField = "Name1, Stücklisten_Artikelnummer";
			string sortingdesc = ((sortingModel is not null) && sortingModel.SortDesc) ? " Desc " : " asc ";
			string artikelnummerfilter = " ";
			if(sortingModel is not null)
			{
				if(!string.IsNullOrWhiteSpace(sortingModel.SortFieldName))
				{
					sortingField = sortingModel.SortFieldName;
				}
			}

			if(paginationFilter is not null && all == 1)
			{
				paginationFilter = "";
			}
			else if(paginationFilter is not null)
			{
				paginationFilter = $"OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY";
			}
			if(ArtikelNummer is not null && !string.IsNullOrWhiteSpace(ArtikelNummer))
			{
				artikelnummerfilter = @$"where Stücklisten_Artikelnummer like '{ArtikelNummer.SqlEscape()}%'";
			}
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
							with [Psz_disposition Anlayse I TN] as (

							SELECT 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Artikelnummer_stücklisten AS Stücklisten_Artikelnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Fertigungsnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Bestand
							, Lagerorte.Lagerort_id
							, Lagerorte.Lagerort
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Bruttobedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Termin_Materialbedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Standardlieferant
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Name1
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].bearbeitet
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Mindestbestellmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Rahmen
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].[Rahmen-Nr]
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Rahmenmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Rahmenauslauf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Termin_Bestätigt1
							FROM 
							Lagerorte INNER JOIN [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn] 
							ON Lagerorte.Lagerort_id = [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Lagerort_id
							GROUP BY 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Artikelnummer_stücklisten
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Fertigungsnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Bestand
							, Lagerorte.Lagerort_id, Lagerorte.Lagerort
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Bruttobedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Termin_Materialbedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Standardlieferant
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Name1
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].bearbeitet
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Mindestbestellmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Rahmen
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].[Rahmen-Nr]
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Rahmenmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Rahmenauslauf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Termin_Bestätigt1
							HAVING ((
							([Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Artikelnummer_stücklisten)<>'reparatur')
							AND (([Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Standardlieferant)=1)))
							, Lager_Obsolet as
							(
							SELECT Lager.[Artikel-Nr], Lager.Lagerort_id, Lager.Bestand
							FROM Lager
							WHERE (((Lager.Lagerort_id)=22) AND ((Lager.Bestand)>0))
							)
							, [PSz_disposition_Anlayse II TN] as (


							SELECT 
							[Psz_disposition Anlayse I TN].Name1
							, [Psz_disposition Anlayse I TN].Stücklisten_Artikelnummer
							, Sum([Psz_disposition Anlayse I TN].Bruttobedarf) AS SummevonBruttobedarf
							, [Psz_disposition Anlayse I TN].Lagerort_id
							, [Psz_disposition Anlayse I TN].Lagerort
							, [Psz_disposition Anlayse I TN].Bestand
							, Max([Psz_disposition Anlayse I TN].Termin_Materialbedarf) AS MaxvonTermin_Materialbedarf
							, Artikel.[Bezeichnung 1] AS [Bezeichnung des Bauteils]
							, [Psz_disposition Anlayse I TN].bearbeitet
							, [Psz_disposition Anlayse I TN].Mindestbestellmenge
							, [Psz_disposition Anlayse I TN].Rahmen
							, [Psz_disposition Anlayse I TN].[Rahmen-Nr]
							, [Psz_disposition Anlayse I TN].Rahmenmenge
							, Artikel.[Artikel-Nr]
							, [Psz_disposition Anlayse I TN].Rahmenauslauf
							, Lager_Obsolet.Bestand AS Betand_Obsolete
							FROM (
							[Psz_disposition Anlayse I TN] INNER JOIN Artikel ON [Psz_disposition Anlayse I TN].Stücklisten_Artikelnummer = Artikel.Artikelnummer)
							LEFT JOIN Lager_Obsolet ON Artikel.[Artikel-Nr] = Lager_Obsolet.[Artikel-Nr]
							GROUP BY 
							[Psz_disposition Anlayse I TN].Name1
							, Artikel.[Artikel-Nr]
							, [Psz_disposition Anlayse I TN].Stücklisten_Artikelnummer
							, [Psz_disposition Anlayse I TN].Lagerort_id
							, [Psz_disposition Anlayse I TN].Lagerort
							, [Psz_disposition Anlayse I TN].Bestand
							, Artikel.[Bezeichnung 1]
							, [Psz_disposition Anlayse I TN].bearbeitet
							, [Psz_disposition Anlayse I TN].Mindestbestellmenge
							, [Psz_disposition Anlayse I TN].Rahmen
							, [Psz_disposition Anlayse I TN].[Rahmen-Nr]
							, [Psz_disposition Anlayse I TN].Rahmenmenge
							, [Psz_disposition Anlayse I TN].Rahmenauslauf
							, Lager_Obsolet.Bestand
							)
							select 
															Name1
															,[Artikel-Nr]
															,Stücklisten_Artikelnummer
															,[Bezeichnung des Bauteils]
															,MaxvonTermin_Materialbedarf 
															,SummevonBruttobedarf
															,Bestand
															,Bestand - SummevonBruttobedarf Differenz
															,Mindestbestellmenge
															,Lagerort
															,Lagerort_id
															,[Rahmen-Nr]
															,Rahmenmenge
															,Rahmenauslauf
															,iif(Betand_Obsolete>0,'true','false') obsolet
															, Count(*) Over() as TotalCount 
													from [PSz_disposition_Anlayse II TN] 
													{artikelnummerfilter}
													ORDER BY {sortingField}  {sortingdesc}
												{paginationFilter}";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120Entity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120Entity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120Entity> GetDE90(Settings.PaginModel paging, Settings.SortingModel sortingModel, string ArtikelNummer, int all = 0)
		{
			var dataTable = new DataTable();

			string paginationFilter = "";
			string sortingField = "Name1, Stücklisten_Artikelnummer";
			string sortingdesc = ((sortingModel is not null) && sortingModel.SortDesc) ? " Desc " : " asc ";
			string artikelnummerfilter = " ";
			if(sortingModel is not null)
			{
				if(!string.IsNullOrWhiteSpace(sortingModel.SortFieldName))
				{
					sortingField = sortingModel.SortFieldName;
				}
			}

			if(paginationFilter is not null && all == 1)
			{
				paginationFilter = "";
			}
			else if(paginationFilter is not null)
			{
				paginationFilter = $"OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY";
			}
			if(ArtikelNummer is not null && !string.IsNullOrWhiteSpace(ArtikelNummer))
			{
				artikelnummerfilter = @$"where Stücklisten_Artikelnummer like '{ArtikelNummer.SqlEscape()}%'";
			}
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
							with [psz_disposition Analyse I De] as (
		SELECT 
				[Psz_disposition_nettobedarfsermittlung Dispo Table_De].Artikelnummer_stücklisten AS Stücklisten_Artikelnummer
				, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Fertigungsnummer
				, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Bestand
				, Lagerorte.Lagerort_id
				, Lagerorte.Lagerort
				, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Bruttobedarf
				, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Termin_Materialbedarf
				, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Standardlieferant
				, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Name1
				, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].bearbeitet
				, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Mindestbestellmenge
				, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Rahmen
				, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].[Rahmen-Nr]
				, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Rahmenmenge
				, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Rahmenauslauf
				, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Termin_Bestätigt1
				FROM Lagerorte INNER JOIN [Psz_disposition_nettobedarfsermittlung Dispo Table_De]
				ON Lagerorte.Lagerort_id = [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Lagerort_id
				GROUP BY [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Artikelnummer_stücklisten
				, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Fertigungsnummer
				, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Bestand
				, Lagerorte.Lagerort_id, Lagerorte.Lagerort
				, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Bruttobedarf
				, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Termin_Materialbedarf
				, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Standardlieferant
				, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Name1
				, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].bearbeitet
				, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Mindestbestellmenge
				, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Rahmen
				, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].[Rahmen-Nr]
				, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Rahmenmenge
				, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Rahmenauslauf
				, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Termin_Bestätigt1
				HAVING ((([Psz_disposition_nettobedarfsermittlung Dispo Table_De].Artikelnummer_stücklisten)<>'reparatur') 
				AND (([Psz_disposition_nettobedarfsermittlung Dispo Table_De].Standardlieferant)=1))
				), Lager_Obsolet as (
									SELECT Lager.[Artikel-Nr], Lager.Lagerort_id, Lager.Bestand
									FROM Lager
									WHERE (((Lager.Lagerort_id)=22) AND ((Lager.Bestand)>0))
								),
								 [psz_disposition_analyse II De] as (
								SELECT [psz_disposition Analyse I De].Name1
								, [psz_disposition Analyse I De].Stücklisten_Artikelnummer
								, Sum([psz_disposition Analyse I De].Bruttobedarf) AS SummevonBruttobedarf
								, [psz_disposition Analyse I De].Lagerort_id
								, [psz_disposition Analyse I De].Lagerort, [psz_disposition Analyse I De].Bestand
								, Max([psz_disposition Analyse I De].Termin_Materialbedarf) AS MaxvonTermin_Materialbedarf
								, Artikel.[Bezeichnung 1] AS [Bezeichnung des Bauteils]
								, [psz_disposition Analyse I De].bearbeitet
								, [psz_disposition Analyse I De].Mindestbestellmenge
								, [psz_disposition Analyse I De].Rahmen
								,Artikel.[Artikel-Nr]
								, [psz_disposition Analyse I De].[Rahmen-Nr]
								, [psz_disposition Analyse I De].Rahmenmenge
								, [psz_disposition Analyse I De].Rahmenauslauf
								, Lager_Obsolet.Bestand AS Betand_Obsolete
						
						FROM [psz_disposition Analyse I De] 
						INNER JOIN Artikel ON [psz_disposition Analyse I De].Stücklisten_Artikelnummer = Artikel.Artikelnummer 
						LEFT JOIN Lager_Obsolet ON Artikel.[Artikel-Nr] = Lager_Obsolet.[Artikel-Nr]
						GROUP BY 
						[psz_disposition Analyse I De].Name1
						, [psz_disposition Analyse I De].Stücklisten_Artikelnummer
						,Artikel.[Artikel-Nr]
						, [psz_disposition Analyse I De].Lagerort_id
						, [psz_disposition Analyse I De].Lagerort
						, [psz_disposition Analyse I De].Bestand
						, Artikel.[Bezeichnung 1], [psz_disposition Analyse I De].bearbeitet
						, [psz_disposition Analyse I De].Mindestbestellmenge
						, [psz_disposition Analyse I De].Rahmen
						, [psz_disposition Analyse I De].[Rahmen-Nr]
						, [psz_disposition Analyse I De].Rahmenmenge
						, [psz_disposition Analyse I De].Rahmenauslauf
						, Lager_Obsolet.Bestand
						)

						select 
														Name1
														,[Artikel-Nr]
														,Stücklisten_Artikelnummer
														,[Bezeichnung des Bauteils]
														,MaxvonTermin_Materialbedarf 
														,SummevonBruttobedarf
														,Bestand
														,Bestand - SummevonBruttobedarf Differenz
														,Mindestbestellmenge
														,Lagerort
														,Lagerort_id
														,[Artikel-Nr]
														,[Rahmen-Nr]
														,Rahmenmenge
														,Rahmenauslauf
														,iif(Betand_Obsolete>0,'true','false') obsolet
														, Count(*) Over() as TotalCount 
										from [psz_disposition_analyse II De]
													{artikelnummerfilter}
													ORDER BY {sortingField}  {sortingdesc}
												{paginationFilter}";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120Entity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120Entity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120Entity> GetTN40(Settings.PaginModel paging, Settings.SortingModel sortingModel, string ArtikelNummer, int all = 0)
		{
			var dataTable = new DataTable();

			string paginationFilter = "";
			string sortingField = "Name1, Stücklisten_Artikelnummer";
			string sortingdesc = ((sortingModel is not null) && sortingModel.SortDesc) ? " Desc " : " asc ";
			string artikelnummerfilter = " ";
			if(sortingModel is not null)
			{
				if(!string.IsNullOrWhiteSpace(sortingModel.SortFieldName))
				{
					sortingField = sortingModel.SortFieldName;
				}
			}
			if(paginationFilter is not null && all == 1)
			{
				paginationFilter = "";
			}
			else if(paginationFilter is not null)
			{
				paginationFilter = $"OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY";
			}
			if(ArtikelNummer is not null && !string.IsNullOrWhiteSpace(ArtikelNummer))
			{
				artikelnummerfilter = @$"where Stücklisten_Artikelnummer like '{ArtikelNummer.SqlEscape()}%'";
			}
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
							With [psz_disposition Analyse I TN 30] as (
							SELECT 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Artikelnummer_stücklisten AS Stücklisten_Artikelnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Fertigungsnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Bestand
							, Lagerorte.Lagerort_id
							, Lagerorte.Lagerort
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Bruttobedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Termin_Materialbedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Standardlieferant
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Name1
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].bearbeitet
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Mindestbestellmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Rahmen
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].[Rahmen-Nr]
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Rahmenmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Rahmenauslauf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Termin_Bestätigt1
							FROM 
							Lagerorte INNER JOIN [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30] 
							ON Lagerorte.Lagerort_id = [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Lagerort_id
							GROUP BY [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Artikelnummer_stücklisten
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Fertigungsnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Bestand
							, Lagerorte.Lagerort_id
							, Lagerorte.Lagerort
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Bruttobedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Termin_Materialbedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Standardlieferant
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Name1
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].bearbeitet
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Mindestbestellmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Rahmen
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].[Rahmen-Nr]
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Rahmenmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Rahmenauslauf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Termin_Bestätigt1
							HAVING ((
							([Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Artikelnummer_stücklisten)<>'reparatur') 
							AND (([Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Standardlieferant)=1))
							)
							, Lager_Obsolet as
							(
							SELECT Lager.[Artikel-Nr], Lager.Lagerort_id, Lager.Bestand
							FROM Lager
							WHERE (((Lager.Lagerort_id)=22) AND ((Lager.Bestand)>0))
							)
							--select * from [psz_disposition Analyse I TN 30]
							,[psz_disposition_Analyse II TN_30] as (
							SELECT 
							[psz_disposition Analyse I TN 30].Name1
							, [psz_disposition Analyse I TN 30].Stücklisten_Artikelnummer
							, Sum([psz_disposition Analyse I TN 30].Bruttobedarf) AS SummevonBruttobedarf
							, [psz_disposition Analyse I TN 30].Lagerort_id
							, [psz_disposition Analyse I TN 30].Lagerort
							, [psz_disposition Analyse I TN 30].Bestand
							, Max([psz_disposition Analyse I TN 30].Termin_Materialbedarf) AS MaxvonTermin_Materialbedarf
							, Artikel.[Bezeichnung 1] AS [Bezeichnung des Bauteils]
							, [psz_disposition Analyse I TN 30].bearbeitet
							, [psz_disposition Analyse I TN 30].Mindestbestellmenge
							, [psz_disposition Analyse I TN 30].Rahmen
							, [psz_disposition Analyse I TN 30].[Rahmen-Nr]
							, [psz_disposition Analyse I TN 30].Rahmenmenge
							, [psz_disposition Analyse I TN 30].Rahmenauslauf
							,Artikel.[Artikel-Nr]
							, Lager_Obsolet.Bestand AS Betand_Obsolete
							FROM (
							[psz_disposition Analyse I TN 30] INNER JOIN Artikel 
							ON [psz_disposition Analyse I TN 30].Stücklisten_Artikelnummer = Artikel.Artikelnummer) LEFT JOIN Lager_Obsolet 
							ON Artikel.[Artikel-Nr] = Lager_Obsolet.[Artikel-Nr]
							GROUP BY [psz_disposition Analyse I TN 30].Name1
							, [psz_disposition Analyse I TN 30].Stücklisten_Artikelnummer
							, [psz_disposition Analyse I TN 30].Lagerort_id
							, [psz_disposition Analyse I TN 30].Lagerort, [psz_disposition Analyse I TN 30].Bestand
							, Artikel.[Bezeichnung 1], [psz_disposition Analyse I TN 30].bearbeitet
							, [psz_disposition Analyse I TN 30].Mindestbestellmenge
							, [psz_disposition Analyse I TN 30].Rahmen
							, [psz_disposition Analyse I TN 30].[Rahmen-Nr]
							, [psz_disposition Analyse I TN 30].Rahmenmenge
							, [psz_disposition Analyse I TN 30].Rahmenauslauf
							, Lager_Obsolet.Bestand
							,Artikel.[Artikel-Nr]
							)

							select 
															Name1
															,[Artikel-Nr]
															,Stücklisten_Artikelnummer
															,[Bezeichnung des Bauteils]
															,MaxvonTermin_Materialbedarf 
															,SummevonBruttobedarf
															,Bestand
															,Bestand - SummevonBruttobedarf Differenz
															,Mindestbestellmenge
															,Lagerort
															,Lagerort_id
															,[Rahmen-Nr]
															,Rahmenmenge
															,Rahmenauslauf
															,iif(Betand_Obsolete>0,'true','false') obsolet
															, Count(*) Over() as TotalCount 
													from [psz_disposition_Analyse II TN_30]   
													{artikelnummerfilter}
													ORDER BY {sortingField}  {sortingdesc}
												 {paginationFilter}";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120Entity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120Entity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120Entity> GetCZ90(Settings.PaginModel paging, Settings.SortingModel sortingModel, string ArtikelNummer, int all = 0)
		{
			var dataTable = new DataTable();

			string paginationFilter = "";
			string sortingField = "Name1, Stücklisten_Artikelnummer";
			string sortingdesc = ((sortingModel is not null) && sortingModel.SortDesc) ? " Desc " : " asc ";
			string artikelnummerfilter = " ";
			if(sortingModel is not null)
			{
				if(!string.IsNullOrWhiteSpace(sortingModel.SortFieldName))
				{
					sortingField = sortingModel.SortFieldName;
				}
			}
			if(paginationFilter is not null && all == 1)
			{
				paginationFilter = "";
			}
			else if(paginationFilter is not null)
			{
				paginationFilter = $"OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY";
			}
			if(ArtikelNummer is not null && !string.IsNullOrWhiteSpace(ArtikelNummer))
			{
				artikelnummerfilter = @$"where Stücklisten_Artikelnummer like '{ArtikelNummer.SqlEscape()}%'";
			}
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
							with [Psz_disposition Anlayse I CZ] as (
							SELECT 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Artikelnummer_stücklisten AS Stücklisten_Artikelnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Fertigungsnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Bestand
							, Lagerorte.Lagerort_id
							, Lagerorte.Lagerort
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Bruttobedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Termin_Materialbedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Standardlieferant
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Name1
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].bearbeitet
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Mindestbestellmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Rahmen
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].[Rahmen-Nr]
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Rahmenmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Rahmenauslauf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Termin_Bestätigt1
							FROM 
							Lagerorte INNER JOIN [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] 
							ON Lagerorte.Lagerort_id = [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Lagerort_id
							GROUP BY 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Artikelnummer_stücklisten
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Fertigungsnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Bestand
							, Lagerorte.Lagerort_id
							, Lagerorte.Lagerort
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Bruttobedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Termin_Materialbedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Standardlieferant
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Name1
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].bearbeitet
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Mindestbestellmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Rahmen
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].[Rahmen-Nr]
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Rahmenmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Rahmenauslauf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Termin_Bestätigt1
							HAVING ((
							([Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Artikelnummer_stücklisten)<>'reparatur') 
							AND (([Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Standardlieferant)=1))
							)
							, Lager_Obsolet as
							(
							SELECT Lager.[Artikel-Nr], Lager.Lagerort_id, Lager.Bestand
							FROM Lager
							WHERE (((Lager.Lagerort_id)=22) AND ((Lager.Bestand)>0))
							)
							,[PSz_disposition_Anlayse II CZ] as (
							SELECT 
							[Psz_disposition Anlayse I CZ].Name1
							, [Psz_disposition Anlayse I CZ].Stücklisten_Artikelnummer
							, Sum([Psz_disposition Anlayse I CZ].Bruttobedarf) AS SummevonBruttobedarf
							, [Psz_disposition Anlayse I CZ].Lagerort_id
							, [Psz_disposition Anlayse I CZ].Lagerort
							, [Psz_disposition Anlayse I CZ].Bestand
							, Max([Psz_disposition Anlayse I CZ].Termin_Materialbedarf) AS MaxvonTermin_Materialbedarf
							, Artikel.[Bezeichnung 1] AS [Bezeichnung des Bauteils]
							, [Psz_disposition Anlayse I CZ].bearbeitet
							, [Psz_disposition Anlayse I CZ].Mindestbestellmenge
							, [Psz_disposition Anlayse I CZ].Rahmen
							, [Psz_disposition Anlayse I CZ].[Rahmen-Nr]
							, [Psz_disposition Anlayse I CZ].Rahmenmenge
							,Artikel.[Artikel-Nr]
							, [Psz_disposition Anlayse I CZ].Rahmenauslauf
							, Lager_Obsolet.Bestand AS Betand_Obsolete
							FROM 
							([Psz_disposition Anlayse I CZ] INNER JOIN Artikel ON [Psz_disposition Anlayse I CZ].Stücklisten_Artikelnummer = Artikel.Artikelnummer)
							LEFT JOIN Lager_Obsolet ON Artikel.[Artikel-Nr] = Lager_Obsolet.[Artikel-Nr]
							GROUP BY 
							[Psz_disposition Anlayse I CZ].Name1
							, [Psz_disposition Anlayse I CZ].Stücklisten_Artikelnummer
							, [Psz_disposition Anlayse I CZ].Lagerort_id
							, [Psz_disposition Anlayse I CZ].Lagerort
							, [Psz_disposition Anlayse I CZ].Bestand
							, Artikel.[Bezeichnung 1]
							, [Psz_disposition Anlayse I CZ].bearbeitet
							, [Psz_disposition Anlayse I CZ].Mindestbestellmenge
							, [Psz_disposition Anlayse I CZ].Rahmen
							, [Psz_disposition Anlayse I CZ].[Rahmen-Nr]
							, [Psz_disposition Anlayse I CZ].Rahmenmenge
							, [Psz_disposition Anlayse I CZ].Rahmenauslauf
							, Lager_Obsolet.Bestand
							,Artikel.[Artikel-Nr]
							)
							select 
															Name1
															,[Artikel-Nr]
															,Stücklisten_Artikelnummer
															,[Bezeichnung des Bauteils]
															,MaxvonTermin_Materialbedarf 
															,SummevonBruttobedarf
															,Bestand
															,Bestand - SummevonBruttobedarf Differenz
															,Mindestbestellmenge
															,Lagerort
															,Lagerort_id
															,[Rahmen-Nr]
															,Rahmenmenge
															,Rahmenauslauf
															,iif(Betand_Obsolete>0,'true','false') obsolet
															, Count(*) Over() as TotalCount 
													from [PSz_disposition_Anlayse II CZ]
													{artikelnummerfilter}
													ORDER BY {sortingField}  {sortingdesc}
												  {paginationFilter}";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120Entity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120Entity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120Entity> GetCZ30(Settings.PaginModel paging, Settings.SortingModel sortingModel, string ArtikelNummer, int all = 0)
		{
			var dataTable = new DataTable();

			string paginationFilter = "";
			string sortingField = "Name1, Stücklisten_Artikelnummer";
			string sortingdesc = ((sortingModel is not null) && sortingModel.SortDesc) ? " Desc " : " asc ";
			string artikelnummerfilter = " ";
			if(sortingModel is not null)
			{
				if(!string.IsNullOrWhiteSpace(sortingModel.SortFieldName))
				{
					sortingField = sortingModel.SortFieldName;
				}
			}
			if(paginationFilter is not null && all == 1)
			{
				paginationFilter = "";
			}
			else if(paginationFilter is not null)
			{
				paginationFilter = $"OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY";
			}
			if(ArtikelNummer is not null && !string.IsNullOrWhiteSpace(ArtikelNummer))
			{
				artikelnummerfilter = @$"where Stücklisten_Artikelnummer like '{ArtikelNummer.SqlEscape()}%'";
			}
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
							with [Psz_disposition Anlayse I CZ_30] as (
							SELECT 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Artikelnummer_stücklisten AS Stücklisten_Artikelnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Fertigungsnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Bestand
							, Lagerorte.Lagerort_id
							, Lagerorte.Lagerort
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Bruttobedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Termin_Materialbedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Standardlieferant
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Name1
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].bearbeitet
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Mindestbestellmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Rahmen
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].[Rahmen-Nr]
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Rahmenmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Rahmenauslauf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Termin_Bestätigt1
							FROM 
							Lagerorte INNER JOIN [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30] 
							ON Lagerorte.Lagerort_id = [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Lagerort_id
							GROUP BY 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Artikelnummer_stücklisten
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Fertigungsnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Bestand
							, Lagerorte.Lagerort_id
							, Lagerorte.Lagerort
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Bruttobedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Termin_Materialbedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Standardlieferant
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Name1
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].bearbeitet
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Mindestbestellmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Rahmen
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].[Rahmen-Nr]
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Rahmenmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Rahmenauslauf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Termin_Bestätigt1
							HAVING ((
							([Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Artikelnummer_stücklisten)<>'reparatur') 
							AND (([Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Standardlieferant)=1))
							)
							, Lager_Obsolet as
							(
							SELECT Lager.[Artikel-Nr], Lager.Lagerort_id, Lager.Bestand
							FROM Lager
							WHERE (((Lager.Lagerort_id)=22) AND ((Lager.Bestand)>0))
							)
							, [PSz_disposition_Anlayse II CZ_30] as (
							SELECT 
							[Psz_disposition Anlayse I CZ_30].Name1
							, [Psz_disposition Anlayse I CZ_30].Stücklisten_Artikelnummer
							, Sum([Psz_disposition Anlayse I CZ_30].Bruttobedarf) AS SummevonBruttobedarf
							, [Psz_disposition Anlayse I CZ_30].Lagerort_id
							, [Psz_disposition Anlayse I CZ_30].Lagerort
							, [Psz_disposition Anlayse I CZ_30].Bestand
							, Max([Psz_disposition Anlayse I CZ_30].Termin_Materialbedarf) AS MaxvonTermin_Materialbedarf
							, Artikel.[Bezeichnung 1] AS [Bezeichnung des Bauteils]
							, [Psz_disposition Anlayse I CZ_30].bearbeitet
							, [Psz_disposition Anlayse I CZ_30].Mindestbestellmenge
							, [Psz_disposition Anlayse I CZ_30].Rahmen
							, [Psz_disposition Anlayse I CZ_30].[Rahmen-Nr]
							, [Psz_disposition Anlayse I CZ_30].Rahmenmenge
							,Artikel.[Artikel-Nr]
							, [Psz_disposition Anlayse I CZ_30].Rahmenauslauf
							, Lager_Obsolet.Bestand AS Betand_Obsolete
							FROM (
							[Psz_disposition Anlayse I CZ_30] INNER JOIN Artikel 
							ON [Psz_disposition Anlayse I CZ_30].Stücklisten_Artikelnummer = Artikel.Artikelnummer)
							LEFT JOIN Lager_Obsolet ON Artikel.[Artikel-Nr] = Lager_Obsolet.[Artikel-Nr]
							GROUP BY 
							[Psz_disposition Anlayse I CZ_30].Name1
							, [Psz_disposition Anlayse I CZ_30].Stücklisten_Artikelnummer
							, [Psz_disposition Anlayse I CZ_30].Lagerort_id
							, [Psz_disposition Anlayse I CZ_30].Lagerort
							, [Psz_disposition Anlayse I CZ_30].Bestand
							, Artikel.[Bezeichnung 1], [Psz_disposition Anlayse I CZ_30].bearbeitet
							, [Psz_disposition Anlayse I CZ_30].Mindestbestellmenge
							, [Psz_disposition Anlayse I CZ_30].Rahmen
							, [Psz_disposition Anlayse I CZ_30].[Rahmen-Nr]
							, [Psz_disposition Anlayse I CZ_30].Rahmenmenge
							, [Psz_disposition Anlayse I CZ_30].Rahmenauslauf
							, Lager_Obsolet.Bestand
							,Artikel.[Artikel-Nr])
							select 
															Name1
															,[Artikel-Nr]
															,Stücklisten_Artikelnummer
															,[Bezeichnung des Bauteils]
															,MaxvonTermin_Materialbedarf 
															,SummevonBruttobedarf
															,Bestand
															,Bestand - SummevonBruttobedarf Differenz
															,Mindestbestellmenge
															,Lagerort
															,Lagerort_id
															,[Rahmen-Nr]
															,Rahmenmenge
															,Rahmenauslauf
															,iif(Betand_Obsolete>0,'true','false') obsolet
															, Count(*) Over() as TotalCount 
													from [PSz_disposition_Anlayse II CZ_30]  
													{artikelnummerfilter}
													ORDER BY {sortingField}  {sortingdesc}
												 {paginationFilter}";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120Entity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120Entity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120Entity> Getws40New(Settings.PaginModel paging, Settings.SortingModel sortingModel, string ArtikelNummer, int all = 0)
		{

			string paginationFilter = "";
			string sortingField = "Name1, Stücklisten_Artikelnummer";
			string sortingdesc = ((sortingModel is not null) && sortingModel.SortDesc) ? " Desc " : " asc ";
			string artikelnummerfilter = " ";
			if(sortingModel is not null)
			{
				if(!string.IsNullOrWhiteSpace(sortingModel.SortFieldName))
				{
					sortingField = sortingModel.SortFieldName;
				}
			}
			if(paginationFilter is not null && all == 1)
			{
				paginationFilter = "";
			}
			else if(paginationFilter is not null)
			{
				paginationFilter = $"OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY";
			}
			if(ArtikelNummer is not null && !string.IsNullOrWhiteSpace(ArtikelNummer))
			{
				artikelnummerfilter = @$"where Stücklisten_Artikelnummer like '{ArtikelNummer.SqlEscape()}%'";
			}


			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
						with  [Psz_disposition Anlayse I KHTN_40] as 
										(
										SELECT 
										[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Artikelnummer_stücklisten AS Stücklisten_Artikelnummer
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Fertigungsnummer
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Bestand
										, Lagerorte.Lagerort_id, Lagerorte.Lagerort
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Bruttobedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Termin_Materialbedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Standardlieferant
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Name1
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].bearbeitet
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Mindestbestellmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Rahmen
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].[Rahmen-Nr]
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Rahmenmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Rahmenauslauf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Termin_Bestätigt1
										FROM Lagerorte INNER JOIN [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40] 
										ON Lagerorte.Lagerort_id = [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Lagerort_id
										GROUP BY [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Artikelnummer_stücklisten
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Fertigungsnummer
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Bestand
										, Lagerorte.Lagerort_id, Lagerorte.Lagerort
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Bruttobedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Termin_Materialbedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Standardlieferant
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Name1
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].bearbeitet
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Mindestbestellmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Rahmen
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].[Rahmen-Nr]
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Rahmenmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Rahmenauslauf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Termin_Bestätigt1
										HAVING ((([Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Artikelnummer_stücklisten)<>'reparatur') 
										AND (([Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Standardlieferant)=1))
										)

										,Lager_Obsolet as (
										SELECT Lager.[Artikel-Nr]
										, Lager.Lagerort_id
										, Lager.Bestand
										FROM Lager
										WHERE (((Lager.Lagerort_id)=22) AND ((Lager.Bestand)>0)))
										, [PSz_disposition_Anlayse II KHTN_40] as
										(SELECT
										[Psz_disposition Anlayse I KHTN_40].Name1,
										[Psz_disposition Anlayse I KHTN_40].Stücklisten_Artikelnummer
										, Sum([Psz_disposition Anlayse I KHTN_40].Bruttobedarf) AS SummevonBruttobedarf
										, [Psz_disposition Anlayse I KHTN_40].Lagerort_id, [Psz_disposition Anlayse I KHTN_40].Lagerort
										, [Psz_disposition Anlayse I KHTN_40].Bestand
										, Max([Psz_disposition Anlayse I KHTN_40].Termin_Materialbedarf) AS MaxvonTermin_Materialbedarf
										, Artikel.[Bezeichnung 1] AS [Bezeichnung des Bauteils]
										, [Psz_disposition Anlayse I KHTN_40].bearbeitet
										, [Psz_disposition Anlayse I KHTN_40].Mindestbestellmenge
										,Artikel.[Artikel-Nr]
										, Artikel.Rahmen, Artikel.[Rahmen-Nr], Artikel.Rahmenmenge
										, Artikel.Rahmenauslauf, Lager_Obsolet.Bestand AS Betand_Obsolete
										FROM (Artikel LEFT JOIN Lager_Obsolet ON Artikel.[Artikel-Nr] = Lager_Obsolet.[Artikel-Nr]) INNER JOIN [Psz_disposition Anlayse I KHTN_40] 
										ON Artikel.Artikelnummer = [Psz_disposition Anlayse I KHTN_40].Stücklisten_Artikelnummer
										GROUP BY [Psz_disposition Anlayse I KHTN_40].Name1
										, [Psz_disposition Anlayse I KHTN_40].Stücklisten_Artikelnummer
										, [Psz_disposition Anlayse I KHTN_40].Lagerort_id
										, [Psz_disposition Anlayse I KHTN_40].Lagerort
										, [Psz_disposition Anlayse I KHTN_40].Bestand
										, Artikel.[Bezeichnung 1]
										, [Psz_disposition Anlayse I KHTN_40].bearbeitet
										, [Psz_disposition Anlayse I KHTN_40].Mindestbestellmenge
										, Artikel.Rahmen, Artikel.[Rahmen-Nr], Artikel.Rahmenmenge
										, Artikel.Rahmenauslauf, Lager_Obsolet.Bestand
										,Artikel.[Artikel-Nr]
										)
										select dkt40.Name1
										,[Artikel-Nr]
										,dkt40.Stücklisten_Artikelnummer
										,dkt40.[Bezeichnung des Bauteils]
										,dkt40.SummevonBruttobedarf
										,dkt40.MaxvonTermin_Materialbedarf
										,dkt40.Bestand
										,dkt40.Bestand - dkt40.SummevonBruttobedarf Differenz
										,dkt40.Mindestbestellmenge
										,dkt40.Lagerort,
										dkt40.Lagerort_id
										,dkt40.[Rahmen-Nr]
										,dkt40.Rahmenmenge
										,dkt40.Rahmenauslauf
										,iif(dkt40.Betand_Obsolete>0,'true','false') obsolet
										,count(*) over() TotalCount
										from [PSz_disposition_Anlayse II KHTN_40] dkt40
										{artikelnummerfilter}
										ORDER BY {sortingField}  {sortingdesc}
										 {paginationFilter}";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120Entity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120Entity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120Entity> Getbetn90(Settings.PaginModel paging, Settings.SortingModel sortingModel, string ArtikelNummer, int all = 0)
		{
			string paginationFilter = "";
			string sortingField = "Name1, Stücklisten_Artikelnummer";
			string sortingdesc = ((sortingModel is not null) && sortingModel.SortDesc) ? " Desc " : " asc ";
			string artikelnummerfilter = " ";
			if(sortingModel is not null)
			{
				if(!string.IsNullOrWhiteSpace(sortingModel.SortFieldName))
				{
					sortingField = sortingModel.SortFieldName;
				}
			}
			if(paginationFilter is not null && all == 1)
			{
				paginationFilter = "";
			}
			else if(paginationFilter is not null)
			{
				paginationFilter = $"OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY";
			}
			if(sortingModel is not null && !string.IsNullOrEmpty(sortingModel.SortFieldName) && !string.IsNullOrWhiteSpace(sortingModel.SortFieldName))
			{
				sortingField = sortingModel.SortFieldName;
			}
			if(ArtikelNummer is not null && !string.IsNullOrWhiteSpace(ArtikelNummer))
			{
				artikelnummerfilter = @$"where Stücklisten_Artikelnummer like '{ArtikelNummer.SqlEscape()}%'";
			}

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
										with [Psz_disposition Anlayse I BETN] as (
										SELECT 
										[Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Artikelnummer_stücklisten AS Stücklisten_Artikelnummer
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Fertigungsnummer
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Bestand
										, Lagerorte.Lagerort_id
										, Lagerorte.Lagerort
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Bruttobedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Termin_Materialbedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Standardlieferant
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Name1
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].bearbeitet
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Mindestbestellmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Rahmen
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].[Rahmen-Nr]
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Rahmenmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Rahmenauslauf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Termin_Bestätigt1
										FROM Lagerorte INNER JOIN [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN] 
										ON Lagerorte.Lagerort_id = [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Lagerort_id
										GROUP BY 
										[Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Artikelnummer_stücklisten
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Fertigungsnummer
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Bestand
										, Lagerorte.Lagerort_id, Lagerorte.Lagerort
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Bruttobedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Termin_Materialbedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Standardlieferant
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Name1
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].bearbeitet
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Mindestbestellmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Rahmen
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].[Rahmen-Nr]
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Rahmenmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Rahmenauslauf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Termin_Bestätigt1
										HAVING ((
										([Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Artikelnummer_stücklisten)<>'reparatur') 
										AND (([Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Standardlieferant)=1))
										)
										,Lager_Obsolet as (
										SELECT Lager.[Artikel-Nr], Lager.Lagerort_id, Lager.Bestand
										FROM Lager
										WHERE (((Lager.Lagerort_id)=22) AND ((Lager.Bestand)>0))
										)
										, [PSz_disposition_Anlayse II BETN] as (
										SELECT 
										[Psz_disposition Anlayse I BETN].Name1
										, [Psz_disposition Anlayse I BETN].Stücklisten_Artikelnummer
										, Sum([Psz_disposition Anlayse I BETN].Bruttobedarf) AS SummevonBruttobedarf
										, [Psz_disposition Anlayse I BETN].Lagerort_id
										, [Psz_disposition Anlayse I BETN].Lagerort
										, [Psz_disposition Anlayse I BETN].Bestand
										, Max([Psz_disposition Anlayse I BETN].Termin_Materialbedarf) AS MaxvonTermin_Materialbedarf
										, Artikel.[Bezeichnung 1] AS [Bezeichnung des Bauteils]
										, [Psz_disposition Anlayse I BETN].bearbeitet
										, [Psz_disposition Anlayse I BETN].Mindestbestellmenge
										, Artikel.Rahmen, Artikel.[Rahmen-Nr], Artikel.Rahmenmenge
										,Artikel.[Artikel-Nr]
										, Artikel.Rahmenauslauf, Lager_Obsolet.Bestand AS Bestand_Obsolete
										FROM [Psz_disposition Anlayse I BETN] INNER JOIN (Artikel LEFT JOIN Lager_Obsolet 
										ON Artikel.[Artikel-Nr] = Lager_Obsolet.[Artikel-Nr]) 
										ON [Psz_disposition Anlayse I BETN].Stücklisten_Artikelnummer = Artikel.Artikelnummer
										GROUP BY [Psz_disposition Anlayse I BETN].Name1
										, [Psz_disposition Anlayse I BETN].Stücklisten_Artikelnummer
										, [Psz_disposition Anlayse I BETN].Lagerort_id
										, [Psz_disposition Anlayse I BETN].Lagerort
										, [Psz_disposition Anlayse I BETN].Bestand
										, Artikel.[Bezeichnung 1], [Psz_disposition Anlayse I BETN].bearbeitet
										, [Psz_disposition Anlayse I BETN].Mindestbestellmenge
										, Artikel.Rahmen, Artikel.[Rahmen-Nr], Artikel.Rahmenmenge
										, Artikel.Rahmenauslauf, Lager_Obsolet.Bestand
										,Artikel.[Artikel-Nr])
										select 
										Name1
										,[Artikel-Nr]
										,Stücklisten_Artikelnummer
										,[Bezeichnung des Bauteils]
										,SummevonBruttobedarf
										,MaxvonTermin_Materialbedarf
										,Bestand
										,Bestand - SummevonBruttobedarf Differenz
										,Mindestbestellmenge
										,Lagerort
										,Lagerort_id
										,[Rahmen-Nr]
										,Rahmenmenge
										,Rahmenauslauf
										,iif(Bestand_Obsolete>0,'true','false') obsolet
										,count(*) over() TotalCount
										from [PSz_disposition_Anlayse II BETN]
										{artikelnummerfilter}
										ORDER BY {sortingField}  {sortingdesc}
										{paginationFilter}";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120Entity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120Entity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120Entity> Getbetn40(Settings.PaginModel paging, Settings.SortingModel sortingModel, string ArtikelNummer, int all = 0)
		{
			string paginationFilter = "";
			string sortingField = "Name1, Stücklisten_Artikelnummer";
			string sortingdesc = ((sortingModel is not null) && sortingModel.SortDesc) ? " Desc " : " asc ";
			string artikelnummerfilter = " ";
			if(sortingModel is not null)
			{
				if(!string.IsNullOrWhiteSpace(sortingModel.SortFieldName))
				{
					sortingField = sortingModel.SortFieldName;
				}
			}
			if(paginationFilter is not null && all == 1)
			{
				paginationFilter = "";
			}
			else if(paginationFilter is not null)
			{
				paginationFilter = $"OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY";
			}
			if(ArtikelNummer is not null && !string.IsNullOrWhiteSpace(ArtikelNummer))
			{
				artikelnummerfilter = @$"where Stücklisten_Artikelnummer like '{ArtikelNummer.SqlEscape()}%'";
			}

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
										with [Psz_disposition Anlayse I BETN_40] as (
										SELECT 
										[Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Artikelnummer_stücklisten AS Stücklisten_Artikelnummer
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Fertigungsnummer
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Bestand
										, Lagerorte.Lagerort_id
										, Lagerorte.Lagerort
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Bruttobedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Termin_Materialbedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Standardlieferant
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Name1
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].bearbeitet
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Mindestbestellmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Rahmen
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].[Rahmen-Nr]
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Rahmenmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Rahmenauslauf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Termin_Bestätigt1
										FROM Lagerorte INNER JOIN [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40] 
										ON Lagerorte.Lagerort_id = [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Lagerort_id
										GROUP BY [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Artikelnummer_stücklisten
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Fertigungsnummer
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Bestand
										, Lagerorte.Lagerort_id, Lagerorte.Lagerort
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Bruttobedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Termin_Materialbedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Standardlieferant
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Name1
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].bearbeitet
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Mindestbestellmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Rahmen
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].[Rahmen-Nr]
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Rahmenmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Rahmenauslauf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Termin_Bestätigt1
										HAVING ((([Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Artikelnummer_stücklisten)<>'reparatur') 
										AND (([Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Standardlieferant)=1))
										)
										,Lager_Obsolet as (
										SELECT Lager.[Artikel-Nr], Lager.Lagerort_id, Lager.Bestand
										FROM Lager
										WHERE (((Lager.Lagerort_id)=22) AND ((Lager.Bestand)>0))
										)
										,[PSz_disposition_Anlayse II KHTN] as ( SELECT 
										[Psz_disposition Anlayse I BETN_40].Name1
										, [Psz_disposition Anlayse I BETN_40].Stücklisten_Artikelnummer
										, Sum([Psz_disposition Anlayse I BETN_40].Bruttobedarf) AS SummevonBruttobedarf
										, [Psz_disposition Anlayse I BETN_40].Lagerort_id, [Psz_disposition Anlayse I BETN_40].Lagerort
										, [Psz_disposition Anlayse I BETN_40].Bestand
										, Max([Psz_disposition Anlayse I BETN_40].Termin_Materialbedarf) AS MaxvonTermin_Materialbedarf
										, Artikel.[Bezeichnung 1] AS [Bezeichnung des Bauteils]
										, [Psz_disposition Anlayse I BETN_40].bearbeitet
										, [Psz_disposition Anlayse I BETN_40].Mindestbestellmenge
										, Artikel.Rahmen, Artikel.[Rahmen-Nr]
										, Artikel.Rahmenmenge
										,Artikel.[Artikel-Nr]
										, Artikel.Rahmenauslauf, Lager_Obsolet.Bestand AS Bestand_Obsolete
										FROM [Psz_disposition Anlayse I BETN_40] INNER JOIN (Artikel LEFT JOIN Lager_Obsolet 
										ON Artikel.[Artikel-Nr] = Lager_Obsolet.[Artikel-Nr])
										ON [Psz_disposition Anlayse I BETN_40].Stücklisten_Artikelnummer = Artikel.Artikelnummer
										GROUP BY 
										[Psz_disposition Anlayse I BETN_40].Name1
										, [Psz_disposition Anlayse I BETN_40].Stücklisten_Artikelnummer
										, [Psz_disposition Anlayse I BETN_40].Lagerort_id
										, [Psz_disposition Anlayse I BETN_40].Lagerort
										, [Psz_disposition Anlayse I BETN_40].Bestand, Artikel.[Bezeichnung 1]
										, [Psz_disposition Anlayse I BETN_40].bearbeitet
										, [Psz_disposition Anlayse I BETN_40].Mindestbestellmenge
										, Artikel.Rahmen, Artikel.[Rahmen-Nr], Artikel.Rahmenmenge
										, Artikel.Rahmenauslauf, Lager_Obsolet.Bestand
										,Artikel.[Artikel-Nr]
										)
										select 
										Name1
										,[Artikel-Nr]
										,Stücklisten_Artikelnummer
										,[Bezeichnung des Bauteils]
										,SummevonBruttobedarf
										,MaxvonTermin_Materialbedarf
										,Bestand
										,Bestand - SummevonBruttobedarf Differenz
										,Mindestbestellmenge
										,Lagerort
										,Lagerort_id
										,[Rahmen-Nr]
										,Rahmenmenge
										,Rahmenauslauf
										,iif(Bestand_Obsolete>0,'true','false') obsolet
										,count(*) over() TotalCount
										from [PSz_disposition_Anlayse II KHTN]
										{artikelnummerfilter}
										ORDER BY {sortingField}  {sortingdesc}
										 {paginationFilter}";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120Entity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120Entity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120Entity> Getal90(Settings.PaginModel paging, Settings.SortingModel sortingModel, string ArtikelNummer, int all = 0)
		{

			string paginationFilter = "";
			string sortingField = "Name1, Stücklisten_Artikelnummer";
			string sortingdesc = ((sortingModel is not null) && sortingModel.SortDesc) ? " Desc " : " asc ";
			string artikelnummerfilter = " ";
			if(sortingModel is not null)
			{
				if(!string.IsNullOrWhiteSpace(sortingModel.SortFieldName))
				{
					sortingField = sortingModel.SortFieldName;
				}
			}
			if(paginationFilter is not null && all == 1)
			{
				paginationFilter = "";
			}
			else if(paginationFilter is not null)
			{
				paginationFilter = $"OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY";
			}
			if(ArtikelNummer is not null && !string.IsNullOrWhiteSpace(ArtikelNummer))
			{
				artikelnummerfilter = @$"where Stücklisten_Artikelnummer like '{ArtikelNummer.SqlEscape()}%'";
			}

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
										with [PSz_disposition Analyse I AL] as (
										SELECT 
										[Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Artikelnummer_stücklisten AS Stücklisten_Artikelnummer
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Fertigungsnummer
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Bestand
										, Lagerorte.Lagerort_id
										, Lagerorte.Lagerort
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Bruttobedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Termin_Materialbedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Standardlieferant
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Name1
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].bearbeitet
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Mindestbestellmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Rahmen
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].[Rahmen-Nr]
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Rahmenmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Rahmenauslauf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Termin_Bestätigt1
										FROM Lagerorte INNER JOIN [Psz_disposition_nettobedarfsermittlung Dispo Table_AL] 
										ON Lagerorte.Lagerort_id = [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Lagerort_id
										GROUP BY [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Artikelnummer_stücklisten
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Fertigungsnummer
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Bestand
										, Lagerorte.Lagerort_id
										, Lagerorte.Lagerort
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Bruttobedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Termin_Materialbedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Standardlieferant
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Name1
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].bearbeitet
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Mindestbestellmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Rahmen
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].[Rahmen-Nr]
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Rahmenmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Rahmenauslauf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Termin_Bestätigt1
										HAVING ((([Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Artikelnummer_stücklisten)<>'reparatur') 
										AND (([Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Standardlieferant)=1))
										)
										,Lager_Obsolet as (
										SELECT Lager.[Artikel-Nr], Lager.Lagerort_id, Lager.Bestand
										FROM Lager
										WHERE (((Lager.Lagerort_id)=22) AND ((Lager.Bestand)>0))
										)
										,[psz_disposition_Analyse II AL] as (
										SELECT [PSz_disposition Analyse I AL].Name1
										, [PSz_disposition Analyse I AL].Stücklisten_Artikelnummer
										, Sum([PSz_disposition Analyse I AL].Bruttobedarf) AS SummevonBruttobedarf
										, [PSz_disposition Analyse I AL].Lagerort_id
										, [PSz_disposition Analyse I AL].Lagerort
										, [PSz_disposition Analyse I AL].Bestand
										, Max([PSz_disposition Analyse I AL].Termin_Materialbedarf) AS MaxvonTermin_Materialbedarf
										, Artikel.[Bezeichnung 1] AS [Bezeichnung des Bauteils]
										, [PSz_disposition Analyse I AL].bearbeitet
										, [PSz_disposition Analyse I AL].Mindestbestellmenge
										, [PSz_disposition Analyse I AL].Rahmen
										, [PSz_disposition Analyse I AL].[Rahmen-Nr]
										, [PSz_disposition Analyse I AL].Rahmenmenge
										, [PSz_disposition Analyse I AL].Rahmenauslauf
										,Artikel.[Artikel-Nr]
										, Lager_Obsolet.Bestand AS Bestand_Obsolete
										FROM ([PSz_disposition Analyse I AL] INNER JOIN Artikel 
										ON [PSz_disposition Analyse I AL].Stücklisten_Artikelnummer = Artikel.Artikelnummer) 
										LEFT JOIN Lager_Obsolet ON Artikel.[Artikel-Nr] = Lager_Obsolet.[Artikel-Nr]
										GROUP BY 
										[PSz_disposition Analyse I AL].Name1
										, [PSz_disposition Analyse I AL].Stücklisten_Artikelnummer
										, [PSz_disposition Analyse I AL].Lagerort_id
										, [PSz_disposition Analyse I AL].Lagerort
										, [PSz_disposition Analyse I AL].Bestand
										, Artikel.[Bezeichnung 1]
										, [PSz_disposition Analyse I AL].bearbeitet
										, [PSz_disposition Analyse I AL].Mindestbestellmenge
										, [PSz_disposition Analyse I AL].Rahmen
										, [PSz_disposition Analyse I AL].[Rahmen-Nr]
										, [PSz_disposition Analyse I AL].Rahmenmenge
										, [PSz_disposition Analyse I AL].Rahmenauslauf
										, Lager_Obsolet.Bestand
										,Artikel.[Artikel-Nr]
										)

										select 
										Name1
										,[Artikel-Nr]
										,Stücklisten_Artikelnummer
										,[Bezeichnung des Bauteils]
										,SummevonBruttobedarf
										,MaxvonTermin_Materialbedarf
										,Bestand
										,Bestand - SummevonBruttobedarf Differenz
										,Mindestbestellmenge
										,Lagerort
										,Lagerort_id
										,[Rahmen-Nr]
										,Rahmenmenge
										,Rahmenauslauf
										,iif(Bestand_Obsolete>0,'true','false') obsolet
										,count(*) over() TotalCount
										from [psz_disposition_Analyse II AL]
										{artikelnummerfilter}
										ORDER BY {sortingField}  {sortingdesc}
										{paginationFilter}";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120Entity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120Entity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120Entity> Getal30(Settings.PaginModel paging, Settings.SortingModel sortingModel, string ArtikelNummer, int all = 0)
		{

			string paginationFilter = "";
			string sortingField = "Name1, Stücklisten_Artikelnummer";
			string sortingdesc = ((sortingModel is not null) && sortingModel.SortDesc) ? " Desc " : " asc ";
			string artikelnummerfilter = " ";
			if(sortingModel is not null)
			{
				if(!string.IsNullOrWhiteSpace(sortingModel.SortFieldName))
				{
					sortingField = sortingModel.SortFieldName;
				}
			}
			if(paginationFilter is not null && all == 1)
			{
				paginationFilter = "";
			}
			else if(paginationFilter is not null)
			{
				paginationFilter = $"OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY";
			}
			if(ArtikelNummer is not null && !string.IsNullOrWhiteSpace(ArtikelNummer))
			{
				artikelnummerfilter = @$"where Stücklisten_Artikelnummer like '{ArtikelNummer.SqlEscape()}%'";
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
										with [PSz_disposition Analyse I AL_30] as (
											SELECT 
											[Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Artikelnummer_stücklisten AS Stücklisten_Artikelnummer
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Fertigungsnummer
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Bestand
											, Lagerorte.Lagerort_id, Lagerorte.Lagerort
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Bruttobedarf
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Termin_Materialbedarf
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Standardlieferant
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Name1
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].bearbeitet
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Mindestbestellmenge
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Rahmen
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].[Rahmen-Nr]
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Rahmenmenge
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Rahmenauslauf
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Termin_Bestätigt1
											FROM Lagerorte INNER JOIN [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30] 
											ON Lagerorte.Lagerort_id = [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Lagerort_id
											GROUP BY [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Artikelnummer_stücklisten
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Fertigungsnummer
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Bestand
											, Lagerorte.Lagerort_id, Lagerorte.Lagerort, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Bruttobedarf
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Termin_Materialbedarf
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Standardlieferant
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Name1
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].bearbeitet
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Mindestbestellmenge
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Rahmen
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].[Rahmen-Nr]
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Rahmenmenge
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Rahmenauslauf
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Termin_Bestätigt1
											HAVING ((([Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Artikelnummer_stücklisten)<>'reparatur') 
											AND (([Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Standardlieferant)=1))
											)
											,Lager_Obsolet as (
											SELECT Lager.[Artikel-Nr], Lager.Lagerort_id, Lager.Bestand
											FROM Lager
											WHERE (((Lager.Lagerort_id)=22) AND ((Lager.Bestand)>0))
											)
											, [psz_disposition_Analyse II AL_30] as (
											SELECT 
											[PSz_disposition Analyse I AL_30].Name1
											, [PSz_disposition Analyse I AL_30].Stücklisten_Artikelnummer
											, Sum([PSz_disposition Analyse I AL_30].Bruttobedarf) AS SummevonBruttobedarf
											, [PSz_disposition Analyse I AL_30].Lagerort_id
											, [PSz_disposition Analyse I AL_30].Lagerort
											, [PSz_disposition Analyse I AL_30].Bestand
											, Max([PSz_disposition Analyse I AL_30].Termin_Materialbedarf) AS MaxvonTermin_Materialbedarf
											, Artikel.[Bezeichnung 1] AS [Bezeichnung des Bauteils]
											, [PSz_disposition Analyse I AL_30].bearbeitet
											, [PSz_disposition Analyse I AL_30].Mindestbestellmenge
											, [PSz_disposition Analyse I AL_30].Rahmen
											, [PSz_disposition Analyse I AL_30].[Rahmen-Nr]
											, [PSz_disposition Analyse I AL_30].Rahmenmenge
											,Artikel.[Artikel-Nr]
											, [PSz_disposition Analyse I AL_30].Rahmenauslauf
											, Lager_Obsolet.Bestand AS Bestand_Obsolete
											FROM ([PSz_disposition Analyse I AL_30] INNER JOIN Artikel 
											ON [PSz_disposition Analyse I AL_30].Stücklisten_Artikelnummer = Artikel.Artikelnummer) LEFT JOIN Lager_Obsolet 
											ON Artikel.[Artikel-Nr] = Lager_Obsolet.[Artikel-Nr]
											GROUP BY [PSz_disposition Analyse I AL_30].Name1
											, [PSz_disposition Analyse I AL_30].Stücklisten_Artikelnummer
											, [PSz_disposition Analyse I AL_30].Lagerort_id, [PSz_disposition Analyse I AL_30].Lagerort
											, [PSz_disposition Analyse I AL_30].Bestand, Artikel.[Bezeichnung 1], [PSz_disposition Analyse I AL_30].bearbeitet
											, [PSz_disposition Analyse I AL_30].Mindestbestellmenge, [PSz_disposition Analyse I AL_30].Rahmen
											, [PSz_disposition Analyse I AL_30].[Rahmen-Nr], [PSz_disposition Analyse I AL_30].Rahmenmenge
											, [PSz_disposition Analyse I AL_30].Rahmenauslauf, Lager_Obsolet.Bestand
											,Artikel.[Artikel-Nr]
											)
											select 
											Name1
											,[Artikel-Nr]
											,Stücklisten_Artikelnummer
											,[Bezeichnung des Bauteils]
											,SummevonBruttobedarf
											,MaxvonTermin_Materialbedarf
											,Bestand
											,Bestand - SummevonBruttobedarf Differenz
											,Mindestbestellmenge
											,Lagerort
											,Lagerort_id
											,[Rahmen-Nr]
											,Rahmenmenge
											,Rahmenauslauf
											,iif(Bestand_Obsolete>0,'true','false') obsolet
											,count(*) over() TotalCount
											from [psz_disposition_Analyse II AL_30]
										{artikelnummerfilter}
										ORDER BY {sortingField}  {sortingdesc}
										 {paginationFilter}";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120Entity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120Entity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity> Getws120Details(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
								SELECT 
								[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Artikelnummer_stücklisten AS PSZ
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].[Vorname/NameFirma] AS Lieferant
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Standardlieferant AS Ausdr1
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].[Bestell-Nr] AS [Lieferanten-Bestellnummer]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Einkaufspreis, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Telefon
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Fax
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Wiederbeschaffungszeitraum
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Mindestbestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].[Bestellungen_Bestellung-Nr] AS [Bestellung#]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Liefertermin AS Wunschtermin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Bestätigter_Termin AS [Bestätigter Termin]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Anzahl AS Bestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].[Bezeichnung des Bauteils] 
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].[Artikel-Nr des Bauteils]
								FROM [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN]
								GROUP BY 
								[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Artikelnummer_stücklisten
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].[Vorname/NameFirma]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Standardlieferant
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].[Bestell-Nr]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Einkaufspreis
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Telefon
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Fax
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Wiederbeschaffungszeitraum
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Mindestbestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].[Bestellungen_Bestellung-Nr]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Liefertermin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Bestätigter_Termin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Anzahl
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].[Bezeichnung des Bauteils]  
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].[Artikel-Nr des Bauteils]
								HAVING ((([Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Artikelnummer_stücklisten) ='{artikelnummer ?? ""}' 
								)) order by PSZ
								OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity> GetDEDetails(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
								SELECT 
								[Psz_disposition_nettobedarfsermittlung Dispo Table_De].Artikelnummer_stücklisten AS PSZ
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].[Vorname/NameFirma] AS Lieferant
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Standardlieferant AS Ausdr1
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].[Bestell-Nr] AS [Lieferanten-Bestellnummer]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Einkaufspreis, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Telefon
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Fax
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Wiederbeschaffungszeitraum
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Mindestbestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].[Bestellungen_Bestellung-Nr] AS [Bestellung#]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Liefertermin AS Wunschtermin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Bestätigter_Termin AS [Bestätigter Termin]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Anzahl AS Bestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].[Bezeichnung des Bauteils] 
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].[Artikel-Nr des Bauteils]
								FROM [Psz_disposition_nettobedarfsermittlung Dispo Table_De]
								GROUP BY 
								[Psz_disposition_nettobedarfsermittlung Dispo Table_De].Artikelnummer_stücklisten
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].[Vorname/NameFirma]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Standardlieferant
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].[Bestell-Nr]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Einkaufspreis
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Telefon
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Fax
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Wiederbeschaffungszeitraum
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Mindestbestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].[Bestellungen_Bestellung-Nr]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Liefertermin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Bestätigter_Termin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Anzahl
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].[Bezeichnung des Bauteils]  
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].[Artikel-Nr des Bauteils]
								HAVING ((([Psz_disposition_nettobedarfsermittlung Dispo Table_De].Artikelnummer_stücklisten) ='{artikelnummer ?? ""}' 
								)) order by PSZ
								OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity> GetBeTn90Details(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
								SELECT 
			                 [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Artikelnummer_stücklisten AS PSZ
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].[Vorname/NameFirma] AS Lieferant
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Standardlieferant AS Ausdr1
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].[Bestell-Nr] AS [Lieferanten-Bestellnummer]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Einkaufspreis, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Telefon
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Fax
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Wiederbeschaffungszeitraum
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Mindestbestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].[Bestellungen_Bestellung-Nr] AS [Bestellung#]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Liefertermin AS Wunschtermin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Bestätigter_Termin AS [Bestätigter Termin]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Anzahl AS Bestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].[Bezeichnung des Bauteils] 
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].[Artikel-Nr des Bauteils]
								FROM [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN]
								GROUP BY 
								[Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Artikelnummer_stücklisten
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].[Vorname/NameFirma]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Standardlieferant
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].[Bestell-Nr]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Einkaufspreis
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Telefon
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Fax
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Wiederbeschaffungszeitraum
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Mindestbestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].[Bestellungen_Bestellung-Nr]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Liefertermin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Bestätigter_Termin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Anzahl
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].[Bezeichnung des Bauteils]  
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].[Artikel-Nr des Bauteils]
								HAVING ((([Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Artikelnummer_stücklisten) ='{artikelnummer ?? ""}' )) 
								order by PSZ
								OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity>();
			}
		}

		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity> GetGZ90Details(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
							select	[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Artikelnummer_stücklisten AS PSZ
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].[Vorname/NameFirma] AS Lieferant
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Standardlieferant AS Ausdr1
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].[Bestell-Nr] AS [Lieferanten-Bestellnummer]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Einkaufspreis, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Telefon
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Fax
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Wiederbeschaffungszeitraum
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Mindestbestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].[Bestellungen_Bestellung-Nr] AS [Bestellung#]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Liefertermin AS Wunschtermin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Bestätigter_Termin AS [Bestätigter Termin]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Anzahl AS Bestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].[Bezeichnung des Bauteils] 
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].[Artikel-Nr des Bauteils]
								FROM [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN]
								GROUP BY 
								[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Artikelnummer_stücklisten
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].[Vorname/NameFirma]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Standardlieferant
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].[Bestell-Nr]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Einkaufspreis
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Telefon
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Fax
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Wiederbeschaffungszeitraum
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Mindestbestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].[Bestellungen_Bestellung-Nr]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Liefertermin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Bestätigter_Termin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Anzahl
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].[Bezeichnung des Bauteils]  
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].[Artikel-Nr des Bauteils]
								HAVING ((([Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Artikelnummer_stücklisten) ='{artikelnummer ?? ""}' 
								)) 
								order by PSZ
								OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity> GetGZ40Details(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
							select	[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Artikelnummer_stücklisten AS PSZ
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].[Vorname/NameFirma] AS Lieferant
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Standardlieferant AS Ausdr1
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].[Bestell-Nr] AS [Lieferanten-Bestellnummer]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Einkaufspreis, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Telefon
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Fax
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Wiederbeschaffungszeitraum
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Mindestbestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].[Bestellungen_Bestellung-Nr] AS [Bestellung#]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Liefertermin AS Wunschtermin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Bestätigter_Termin AS [Bestätigter Termin]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Anzahl AS Bestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].[Bezeichnung des Bauteils] 
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].[Artikel-Nr des Bauteils]
								FROM [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40]
								GROUP BY 
								[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Artikelnummer_stücklisten
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].[Vorname/NameFirma]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Standardlieferant
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].[Bestell-Nr]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Einkaufspreis
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Telefon
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Fax
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Wiederbeschaffungszeitraum
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Mindestbestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].[Bestellungen_Bestellung-Nr]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Liefertermin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Bestätigter_Termin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Anzahl
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].[Bezeichnung des Bauteils]  
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].[Artikel-Nr des Bauteils]
								HAVING ((([Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Artikelnummer_stücklisten) ='{artikelnummer ?? ""}' 
								)) 
								order by PSZ
								OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity> GetBeTn40Details(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
								SELECT 
			                 [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Artikelnummer_stücklisten AS PSZ
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].[Vorname/NameFirma] AS Lieferant
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Standardlieferant AS Ausdr1
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].[Bestell-Nr] AS [Lieferanten-Bestellnummer]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Einkaufspreis, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Telefon
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Fax
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Wiederbeschaffungszeitraum
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Mindestbestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].[Bestellungen_Bestellung-Nr] AS [Bestellung#]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Liefertermin AS Wunschtermin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Bestätigter_Termin AS [Bestätigter Termin]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Anzahl AS Bestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].[Bezeichnung des Bauteils] 
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].[Artikel-Nr des Bauteils]
								FROM [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40]
								GROUP BY 
								[Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Artikelnummer_stücklisten
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].[Vorname/NameFirma]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Standardlieferant
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].[Bestell-Nr]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Einkaufspreis
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Telefon
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Fax
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Wiederbeschaffungszeitraum
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Mindestbestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].[Bestellungen_Bestellung-Nr]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Liefertermin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Bestätigter_Termin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Anzahl
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].[Bezeichnung des Bauteils]  
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].[Artikel-Nr des Bauteils]
								HAVING ((([Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Artikelnummer_stücklisten) ='{artikelnummer ?? ""}' )) 
								order by PSZ
								OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity> GetAl90Details(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
								SELECT 
			                 [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Artikelnummer_stücklisten AS PSZ
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].[Vorname/NameFirma] AS Lieferant
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Standardlieferant AS Ausdr1
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].[Bestell-Nr] AS [Lieferanten-Bestellnummer]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Einkaufspreis, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Telefon
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Fax
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Wiederbeschaffungszeitraum
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Mindestbestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].[Bestellungen_Bestellung-Nr] AS [Bestellung#]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Liefertermin AS Wunschtermin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Bestätigter_Termin AS [Bestätigter Termin]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Anzahl AS Bestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].[Bezeichnung des Bauteils] 
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].[Artikel-Nr des Bauteils]
								FROM [Psz_disposition_nettobedarfsermittlung Dispo Table_AL]
								GROUP BY 
								[Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Artikelnummer_stücklisten
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].[Vorname/NameFirma]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Standardlieferant
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].[Bestell-Nr]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Einkaufspreis
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Telefon
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Fax
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Wiederbeschaffungszeitraum
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Mindestbestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].[Bestellungen_Bestellung-Nr]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Liefertermin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Bestätigter_Termin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Anzahl
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].[Bezeichnung des Bauteils]  
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].[Artikel-Nr des Bauteils]
								HAVING ((([Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Artikelnummer_stücklisten) ='{artikelnummer ?? ""}' )) 
								order by PSZ
								OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity> GetAl30Details(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
					SELECT 
			                 [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Artikelnummer_stücklisten AS PSZ
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].[Vorname/NameFirma] AS Lieferant
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Standardlieferant AS Ausdr1
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].[Bestell-Nr] AS [Lieferanten-Bestellnummer]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Einkaufspreis, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Telefon
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Fax
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Wiederbeschaffungszeitraum
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Mindestbestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].[Bestellungen_Bestellung-Nr] AS [Bestellung#]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Liefertermin AS Wunschtermin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Bestätigter_Termin AS [Bestätigter Termin]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Anzahl AS Bestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].[Bezeichnung des Bauteils] 
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].[Artikel-Nr des Bauteils]
								FROM [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30]
								GROUP BY 
								[Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Artikelnummer_stücklisten
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].[Vorname/NameFirma]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Standardlieferant
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].[Bestell-Nr]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Einkaufspreis
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Telefon
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Fax
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Wiederbeschaffungszeitraum
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Mindestbestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].[Bestellungen_Bestellung-Nr]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Liefertermin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Bestätigter_Termin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Anzahl
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].[Bezeichnung des Bauteils]  
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].[Artikel-Nr des Bauteils]
								HAVING ((([Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Artikelnummer_stücklisten) ='{artikelnummer ?? ""}'  )) 
								order by PSZ
								OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity> GetTN90Details(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
					SELECT 
			                 [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Artikelnummer_stücklisten AS PSZ
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].[Vorname/NameFirma] AS Lieferant
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Standardlieferant AS Ausdr1
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].[Bestell-Nr] AS [Lieferanten-Bestellnummer]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Einkaufspreis, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Telefon
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Fax
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Wiederbeschaffungszeitraum
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Mindestbestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].[Bestellungen_Bestellung-Nr] AS [Bestellung#]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Liefertermin AS Wunschtermin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Bestätigter_Termin AS [Bestätigter Termin]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Anzahl AS Bestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].[Bezeichnung des Bauteils] 
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].[Artikel-Nr des Bauteils]
								FROM [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn]
								GROUP BY 
								[Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Artikelnummer_stücklisten
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].[Vorname/NameFirma]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Standardlieferant
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].[Bestell-Nr]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Einkaufspreis
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Telefon
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Fax
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Wiederbeschaffungszeitraum
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Mindestbestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].[Bestellungen_Bestellung-Nr]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Liefertermin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Bestätigter_Termin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Anzahl
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].[Bezeichnung des Bauteils]  
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].[Artikel-Nr des Bauteils]
								HAVING ((([Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Artikelnummer_stücklisten) ='{artikelnummer ?? ""}' )) 
								order by PSZ
								OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity> GetTN30Details(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
					SELECT 
			                 [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Artikelnummer_stücklisten AS PSZ
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].[Vorname/NameFirma] AS Lieferant
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Standardlieferant AS Ausdr1
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].[Bestell-Nr] AS [Lieferanten-Bestellnummer]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Einkaufspreis, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Telefon
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Fax
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Wiederbeschaffungszeitraum
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Mindestbestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].[Bestellungen_Bestellung-Nr] AS [Bestellung#]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Liefertermin AS Wunschtermin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Bestätigter_Termin AS [Bestätigter Termin]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Anzahl AS Bestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].[Bezeichnung des Bauteils] 
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].[Artikel-Nr des Bauteils]
								FROM [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30]
								GROUP BY 
								[Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Artikelnummer_stücklisten
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].[Vorname/NameFirma]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Standardlieferant
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].[Bestell-Nr]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Einkaufspreis
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Telefon
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Fax
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Wiederbeschaffungszeitraum
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Mindestbestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].[Bestellungen_Bestellung-Nr]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Liefertermin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Bestätigter_Termin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Anzahl
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].[Bezeichnung des Bauteils]  
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].[Artikel-Nr des Bauteils]
								HAVING ((([Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Artikelnummer_stücklisten) ='{artikelnummer ?? ""}' )) 
								order by PSZ
								OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity> GetCZ90Details(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
					SELECT 
			                 [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .Artikelnummer_stücklisten AS PSZ
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .[Vorname/NameFirma] AS Lieferant
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .Standardlieferant AS Ausdr1
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .[Bestell-Nr] AS [Lieferanten-Bestellnummer]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .Einkaufspreis, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .Telefon
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .Fax
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .Wiederbeschaffungszeitraum
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .Mindestbestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .[Bestellungen_Bestellung-Nr] AS [Bestellung#]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .Liefertermin AS Wunschtermin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .Bestätigter_Termin AS [Bestätigter Termin]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .Anzahl AS Bestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .[Bezeichnung des Bauteils] 
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .[Artikel-Nr des Bauteils]
								FROM [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] 
								GROUP BY 
								[Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .Artikelnummer_stücklisten
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .[Vorname/NameFirma]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .Standardlieferant
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .[Bestell-Nr]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .Einkaufspreis
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .Telefon
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .Fax
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .Wiederbeschaffungszeitraum
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .Mindestbestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .[Bestellungen_Bestellung-Nr]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .Liefertermin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .Bestätigter_Termin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .Anzahl
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .[Bezeichnung des Bauteils]  
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .[Artikel-Nr des Bauteils]
								HAVING ((([Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] .Artikelnummer_stücklisten) ='{artikelnummer ?? ""}' )) 
								order by PSZ
								OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity> GetCZ30Details(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
					SELECT 
			                 [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .Artikelnummer_stücklisten AS PSZ
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .[Vorname/NameFirma] AS Lieferant
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .Standardlieferant AS Ausdr1
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .[Bestell-Nr] AS [Lieferanten-Bestellnummer]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .Einkaufspreis, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .Telefon
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .Fax
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .Wiederbeschaffungszeitraum
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .Mindestbestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .[Bestellungen_Bestellung-Nr] AS [Bestellung#]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .Liefertermin AS Wunschtermin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .Bestätigter_Termin AS [Bestätigter Termin]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .Anzahl AS Bestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .[Bezeichnung des Bauteils] 
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .[Artikel-Nr des Bauteils]
								FROM [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  
								GROUP BY 
								[Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .Artikelnummer_stücklisten
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .[Vorname/NameFirma]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .Standardlieferant
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .[Bestell-Nr]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .Einkaufspreis
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .Telefon
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .Fax
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .Wiederbeschaffungszeitraum
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .Mindestbestellmenge
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .[Bestellungen_Bestellung-Nr]
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .Liefertermin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .Bestätigter_Termin
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .Anzahl
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .[Bezeichnung des Bauteils]  
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .[Artikel-Nr des Bauteils]
								HAVING ((([Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30]  .Artikelnummer_stücklisten) ='{artikelnummer ?? ""}' )) 
								order by PSZ
								OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity> Getws40Details(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
									SELECT 
									[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Artikelnummer_stücklisten  AS PSZ
									, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].[Bezeichnung des Bauteils]
									, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].[Vorname/NameFirma] AS Lieferant
									, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Standardlieferant AS Ausdr1
									, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].[Bestell-Nr] AS [Lieferanten-Bestellnummer]
									, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Einkaufspreis
									, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Telefon
									, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Fax
									, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Wiederbeschaffungszeitraum
									, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Mindestbestellmenge
									, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].[Bestellungen_Bestellung-Nr] AS [Bestellung#]
									, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Liefertermin AS Wunschtermin
									, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Bestätigter_Termin AS [Bestätigter Termin]
									, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Anzahl AS Bestellmenge

									, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].[Artikel-Nr des Bauteils]
									FROM [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40]
									GROUP BY 
									[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Artikelnummer_stücklisten
									, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].[Vorname/NameFirma]
									, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Standardlieferant
									, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].[Bestell-Nr]
									, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Einkaufspreis
									, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Telefon
									, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Fax
									, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Wiederbeschaffungszeitraum
									,[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Mindestbestellmenge
									, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].[Bestellungen_Bestellung-Nr]
									, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Liefertermin
									, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Bestätigter_Termin
									, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Anzahl
									, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].[Bezeichnung des Bauteils]
									, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].[Artikel-Nr des Bauteils]
									HAVING 
									[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Artikelnummer_stücklisten='{artikelnummer ?? ""}'	
									order by PSZ OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY ";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity> Getws40Bestand(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
								with [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II KHTN_40] as (
								SELECT 
								[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Artikelnummer_stücklisten AS [PSZ#]
								,[PSZ_Dispo31_Materialbestand Dispo_KHTn_40].Bestand, [PSZ_Dispo31_Materialbestand Dispo_KHTn_40].Lagerplatz AS Lagerort
								, [PSZ_Dispo31_Materialbestand Dispo_KHTn_40].Lagerort_id
								FROM [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40] INNER JOIN [PSZ_Dispo31_Materialbestand Dispo_KHTn_40] 
								ON [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].[Artikel-Nr des Bauteils] = [PSZ_Dispo31_Materialbestand Dispo_KHTn_40].[Artikel-Nr]
								GROUP BY 
								[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Artikelnummer_stücklisten
								, [PSZ_Dispo31_Materialbestand Dispo_KHTn_40].Bestand
								, [PSZ_Dispo31_Materialbestand Dispo_KHTn_40].Lagerplatz
								, [PSZ_Dispo31_Materialbestand Dispo_KHTn_40].Lagerort_id
								HAVING (
								[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Artikelnummer_stücklisten)='{artikelnummer ?? ""}'
								AND
								[PSZ_Dispo31_Materialbestand Dispo_KHTn_40].Bestand<>0
								)
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Form III KHTN_40] as 
								(SELECT 
								[PSZ_Disposition_Nettobedarfsermittlung Dispo Details II KHTN_40].[PSZ#]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II KHTN_40].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II KHTN_40].Lagerort
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II KHTN_40].Lagerort_id
								FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II KHTN_40])
								select * from [PSZ_Disposition_Nettobedarfsermittlung Dispo Form III KHTN_40]
								order by Lagerort_id
								";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity> GetGZ90Bestand(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
								with [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II GZTN] as (
								SELECT 
								[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Artikelnummer_stücklisten AS [PSZ#]
								,[PSZ_Dispo31_Materialbestand Dispo_GZTN].Bestand, [PSZ_Dispo31_Materialbestand Dispo_GZTN].Lagerplatz AS Lagerort
								, [PSZ_Dispo31_Materialbestand Dispo_GZTN].Lagerort_id
								FROM [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN] INNER JOIN [PSZ_Dispo31_Materialbestand Dispo_GZTN] 
								ON [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].[Artikel-Nr des Bauteils] = [PSZ_Dispo31_Materialbestand Dispo_GZTN].[Artikel-Nr]
								GROUP BY 
								[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Artikelnummer_stücklisten
								, [PSZ_Dispo31_Materialbestand Dispo_GZTN].Bestand
								, [PSZ_Dispo31_Materialbestand Dispo_GZTN].Lagerplatz
								, [PSZ_Dispo31_Materialbestand Dispo_GZTN].Lagerort_id
								HAVING
								([Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Artikelnummer_stücklisten)='{artikelnummer ?? ""}'
								AND
								[PSZ_Dispo31_Materialbestand Dispo_GZTN].Bestand<>0
								)
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Form III GZTN] as 
								(SELECT 
								[PSZ_Disposition_Nettobedarfsermittlung Dispo Details II GZTN].[PSZ#]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II GZTN].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II GZTN].Lagerort
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II GZTN].Lagerort_id
								FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II GZTN])
								select * from [PSZ_Disposition_Nettobedarfsermittlung Dispo Form III GZTN]
								order by Lagerort_id ";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity>();
			}
		}

		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity> GetGZ40Bestand(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
								with [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II GZTN_40] as (
								SELECT 
								[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Artikelnummer_stücklisten AS [PSZ#]
								,[PSZ_Dispo31_Materialbestand Dispo_GZTN_40].Bestand, [PSZ_Dispo31_Materialbestand Dispo_GZTN_40].Lagerplatz AS Lagerort
								, [PSZ_Dispo31_Materialbestand Dispo_GZTN_40].Lagerort_id
								FROM [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40] INNER JOIN [PSZ_Dispo31_Materialbestand Dispo_GZTN_40] 
								ON [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].[Artikel-Nr des Bauteils] = [PSZ_Dispo31_Materialbestand Dispo_GZTN_40].[Artikel-Nr]
								GROUP BY 
								[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Artikelnummer_stücklisten
								, [PSZ_Dispo31_Materialbestand Dispo_GZTN_40].Bestand
								, [PSZ_Dispo31_Materialbestand Dispo_GZTN_40].Lagerplatz
								, [PSZ_Dispo31_Materialbestand Dispo_GZTN_40].Lagerort_id
								HAVING
								([Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Artikelnummer_stücklisten)='{artikelnummer ?? ""}'
								AND
								[PSZ_Dispo31_Materialbestand Dispo_GZTN_40].Bestand<>0
								)
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details III GZTN_40] as 
								(SELECT 
								[PSZ_Disposition_Nettobedarfsermittlung Dispo Details II GZTN_40].[PSZ#]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II GZTN_40].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II GZTN_40].Lagerort
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II GZTN_40].Lagerort_id
								FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II GZTN_40])
								select * from [PSZ_Disposition_Nettobedarfsermittlung Dispo Details III GZTN_40]
								order by Lagerort_id";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity> Getws120Bestand(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
								with [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II KHTN] as (
								SELECT 
								[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Artikelnummer_stücklisten AS [PSZ#]
								, [PSZ_Dispo31_Materialbestand Dispo_KHTn].Bestand
								, [PSZ_Dispo31_Materialbestand Dispo_KHTn].Lagerplatz AS Lagerort
								, [PSZ_Dispo31_Materialbestand Dispo_KHTn].Lagerort_id
								FROM [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN] INNER JOIN [PSZ_Dispo31_Materialbestand Dispo_KHTn] 
								ON [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].[Artikel-Nr des Bauteils] = [PSZ_Dispo31_Materialbestand Dispo_KHTn].[Artikel-Nr]
								GROUP BY [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Artikelnummer_stücklisten
								, [PSZ_Dispo31_Materialbestand Dispo_KHTn].Bestand, [PSZ_Dispo31_Materialbestand Dispo_KHTn].Lagerplatz
								, [PSZ_Dispo31_Materialbestand Dispo_KHTn].Lagerort_id
								HAVING ((([Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Artikelnummer_stücklisten) ='{artikelnummer ?? ""}' 
								) 
								AND (([PSZ_Dispo31_Materialbestand Dispo_KHTn].Bestand)<>0)))

								select  Lagerort_id,Lagerort,Bestand from [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II KHTN] 
								order by Lagerort_id
								";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity> GetDE90Bestand(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
								with [psz_disposition_Nettobedarfsermittlung Dispo Details II De] as (
								SELECT 
								[PSZ_Dispo31_Materialbestand Dispo_De].Lagerplatz AS Lagerort
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Artikelnummer_stücklisten AS [psz#]
								, [PSZ_Dispo31_Materialbestand Dispo_De].Bestand
								, [PSZ_Dispo31_Materialbestand Dispo_De].Lagerort_id
								, [PSZ_Dispo31_Materialbestand Dispo_De].[Artikel-Nr]
								FROM [Psz_disposition_nettobedarfsermittlung Dispo Table_De] 
								INNER JOIN [PSZ_Dispo31_Materialbestand Dispo_De] 
								ON [Psz_disposition_nettobedarfsermittlung Dispo Table_De].[Artikel-Nr des Bauteils] = [PSZ_Dispo31_Materialbestand Dispo_De].[Artikel-Nr]
								GROUP BY 
								[PSZ_Dispo31_Materialbestand Dispo_De].Lagerplatz
								, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Artikelnummer_stücklisten
								, [PSZ_Dispo31_Materialbestand Dispo_De].Bestand
								, [PSZ_Dispo31_Materialbestand Dispo_De].Lagerort_id
								, [PSZ_Dispo31_Materialbestand Dispo_De].[Artikel-Nr]
								HAVING (((
								[Psz_disposition_nettobedarfsermittlung Dispo Table_De].Artikelnummer_stücklisten)='{artikelnummer ?? ""}' ) 
								AND (([PSZ_Dispo31_Materialbestand Dispo_De].Bestand)<>0)))
								select  Lagerort_id,Lagerort,Bestand from [psz_disposition_Nettobedarfsermittlung Dispo Details II De]
																order by Lagerort_id
								";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity> Getbetn90Bestand(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
								with [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II BETN] as (
								SELECT 
								[Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Artikelnummer_stücklisten AS [PSZ#]
								, [PSZ_Dispo31_Materialbestand Dispo_BETN].Bestand
								, [PSZ_Dispo31_Materialbestand Dispo_BETN].Lagerplatz AS Lagerort
								, [PSZ_Dispo31_Materialbestand Dispo_BETN].Lagerort_id
								FROM [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN] INNER JOIN [PSZ_Dispo31_Materialbestand Dispo_BETN] 
								ON [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].[Artikel-Nr des Bauteils] = [PSZ_Dispo31_Materialbestand Dispo_BETN].[Artikel-Nr]
								GROUP BY [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Artikelnummer_stücklisten
								, [PSZ_Dispo31_Materialbestand Dispo_BETN].Bestand, [PSZ_Dispo31_Materialbestand Dispo_BETN].Lagerplatz
								, [PSZ_Dispo31_Materialbestand Dispo_BETN].Lagerort_id
								HAVING (
								([Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Artikelnummer_stücklisten) ='{artikelnummer ?? ""}' 
								AND 
								(([PSZ_Dispo31_Materialbestand Dispo_BETN].Bestand)<>0)))
								select  Lagerort_id,Lagerort,Bestand from [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II BETN] 
								order by Lagerort_id
								";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity> Getbetn40Bestand(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
								with [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II BETN] as (
								SELECT 
								[Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Artikelnummer_stücklisten AS [PSZ#]
								, [PSZ_Dispo31_Materialbestand Dispo_BETN_40].Bestand
								, [PSZ_Dispo31_Materialbestand Dispo_BETN_40].Lagerplatz AS Lagerort
								, [PSZ_Dispo31_Materialbestand Dispo_BETN_40].Lagerort_id
								FROM [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40] INNER JOIN [PSZ_Dispo31_Materialbestand Dispo_BETN_40] 
								ON [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].[Artikel-Nr des Bauteils] = [PSZ_Dispo31_Materialbestand Dispo_BETN_40].[Artikel-Nr]
								GROUP BY [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Artikelnummer_stücklisten
								, [PSZ_Dispo31_Materialbestand Dispo_BETN_40].Bestand, [PSZ_Dispo31_Materialbestand Dispo_BETN_40].Lagerplatz
								, [PSZ_Dispo31_Materialbestand Dispo_BETN_40].Lagerort_id
								HAVING (
								([Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Artikelnummer_stücklisten) ='{artikelnummer ?? ""}' 
								AND 
								(([PSZ_Dispo31_Materialbestand Dispo_BETN_40].Bestand)<>0)))

								select  Lagerort_id,Lagerort,Bestand from [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II BETN] 
								order by Lagerort_id
								";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity> GetAl90Bestand(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
								with [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II BETN] as (
								SELECT 
								 [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Artikelnummer_stücklisten AS [PSZ#]
								, [PSZ_Dispo31_Materialbestand Dispo_AL].Bestand
								, [PSZ_Dispo31_Materialbestand Dispo_AL].Lagerplatz AS Lagerort
								, [PSZ_Dispo31_Materialbestand Dispo_AL].Lagerort_id
								FROM  [Psz_disposition_nettobedarfsermittlung Dispo Table_AL] INNER JOIN [PSZ_Dispo31_Materialbestand Dispo_AL] 
								ON  [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].[Artikel-Nr des Bauteils] = [PSZ_Dispo31_Materialbestand Dispo_AL].[Artikel-Nr]
								GROUP BY  [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Artikelnummer_stücklisten
								, [PSZ_Dispo31_Materialbestand Dispo_AL].Bestand, [PSZ_Dispo31_Materialbestand Dispo_AL].Lagerplatz
								, [PSZ_Dispo31_Materialbestand Dispo_AL].Lagerort_id
								HAVING (
								( [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Artikelnummer_stücklisten) ='{artikelnummer ?? ""}' 
								AND 
								(([PSZ_Dispo31_Materialbestand Dispo_AL].Bestand)<>0)))

								select  Lagerort_id,Lagerort,Bestand from [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II BETN] 
								order by Lagerort_id
								";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity> GetAl30Bestand(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
								with [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II BETN] as (
								SELECT 
								 [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Artikelnummer_stücklisten AS [PSZ#]
								, [PSZ_Dispo31_Materialbestand Dispo_AL_30].Bestand
								, [PSZ_Dispo31_Materialbestand Dispo_AL_30].Lagerplatz AS Lagerort
								, [PSZ_Dispo31_Materialbestand Dispo_AL_30].Lagerort_id
								FROM  [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30] INNER JOIN [PSZ_Dispo31_Materialbestand Dispo_AL_30] 
								ON  [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].[Artikel-Nr des Bauteils] = [PSZ_Dispo31_Materialbestand Dispo_AL_30].[Artikel-Nr]
								GROUP BY  [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Artikelnummer_stücklisten
								, [PSZ_Dispo31_Materialbestand Dispo_AL_30].Bestand, [PSZ_Dispo31_Materialbestand Dispo_AL_30].Lagerplatz
								, [PSZ_Dispo31_Materialbestand Dispo_AL_30].Lagerort_id
								HAVING (
								( [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Artikelnummer_stücklisten) ='{artikelnummer ?? ""}' 
								AND 
								(([PSZ_Dispo31_Materialbestand Dispo_AL_30].Bestand)<>0)))

								select  Lagerort_id,Lagerort,Bestand from [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II BETN] 
								order by Lagerort_id
								";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity> Gettn90Bestand(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
								with [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II BETN] as (
								SELECT 
								 [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Artikelnummer_stücklisten AS [PSZ#]
								, [PSZ_Dispo31_Materialbestand Dispo_Tn].Bestand
								, [PSZ_Dispo31_Materialbestand Dispo_Tn].Lagerplatz AS Lagerort
								, [PSZ_Dispo31_Materialbestand Dispo_Tn].Lagerort_id
								FROM  [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn] INNER JOIN [PSZ_Dispo31_Materialbestand Dispo_Tn] 
								ON  [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].[Artikel-Nr des Bauteils] = [PSZ_Dispo31_Materialbestand Dispo_Tn].[Artikel-Nr]
								GROUP BY  [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Artikelnummer_stücklisten
								, [PSZ_Dispo31_Materialbestand Dispo_Tn].Bestand, [PSZ_Dispo31_Materialbestand Dispo_Tn].Lagerplatz
								, [PSZ_Dispo31_Materialbestand Dispo_Tn].Lagerort_id
								HAVING (
								( [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Artikelnummer_stücklisten) ='{artikelnummer ?? ""}' 
								AND 
								(([PSZ_Dispo31_Materialbestand Dispo_Tn].Bestand)<>0)))
								select  Lagerort_id,Lagerort,Bestand from [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II BETN] 
								order by Lagerort_id
								";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity> Gettn30Bestand(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
							with [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II BETN] as (
								SELECT 
								 [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Artikelnummer_stücklisten AS [PSZ#]
								, [PSZ_Dispo31_Materialbestand Dispo_Tn_30].Bestand
								, [PSZ_Dispo31_Materialbestand Dispo_Tn_30].Lagerplatz AS Lagerort
								, [PSZ_Dispo31_Materialbestand Dispo_Tn_30].Lagerort_id
								FROM  [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30] INNER JOIN [PSZ_Dispo31_Materialbestand Dispo_Tn_30] 
								ON  [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].[Artikel-Nr des Bauteils] = [PSZ_Dispo31_Materialbestand Dispo_Tn_30].[Artikel-Nr]
								GROUP BY  [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Artikelnummer_stücklisten
								, [PSZ_Dispo31_Materialbestand Dispo_Tn_30].Bestand, [PSZ_Dispo31_Materialbestand Dispo_Tn_30].Lagerplatz
								, [PSZ_Dispo31_Materialbestand Dispo_Tn_30].Lagerort_id
								HAVING (
								( [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Artikelnummer_stücklisten) ='{artikelnummer ?? ""}' 
								AND 
								(([PSZ_Dispo31_Materialbestand Dispo_Tn_30].Bestand)<>0)))
								select  Lagerort_id,Lagerort,Bestand from [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II BETN] 
								order by Lagerort_id
								";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity> GetCZ90Bestand(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
							with [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II BETN] as (
								SELECT 
								 [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Artikelnummer_stücklisten AS [PSZ#]
								, [PSZ_Dispo31_Materialbestand Dispo_CZ].Bestand
								, [PSZ_Dispo31_Materialbestand Dispo_CZ].Lagerplatz AS Lagerort
								, [PSZ_Dispo31_Materialbestand Dispo_CZ].Lagerort_id
								FROM  [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] INNER JOIN [PSZ_Dispo31_Materialbestand Dispo_CZ] 
								ON  [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].[Artikel-Nr des Bauteils] = [PSZ_Dispo31_Materialbestand Dispo_CZ].[Artikel-Nr]
								GROUP BY  [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Artikelnummer_stücklisten
								, [PSZ_Dispo31_Materialbestand Dispo_CZ].Bestand, [PSZ_Dispo31_Materialbestand Dispo_CZ].Lagerplatz
								, [PSZ_Dispo31_Materialbestand Dispo_CZ].Lagerort_id
								HAVING (
								( [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Artikelnummer_stücklisten) ='{artikelnummer ?? ""}' 
								AND 
								(([PSZ_Dispo31_Materialbestand Dispo_CZ].Bestand)<>0)))
								select  Lagerort_id,Lagerort,Bestand from [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II BETN] 
								order by Lagerort_id
								";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity> GetCZ30Bestand(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
							with [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II BETN] as (
								SELECT 
								 [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Artikelnummer_stücklisten AS [PSZ#]
								, [PSZ_Dispo31_Materialbestand Dispo_CZ_30].Bestand
								, [PSZ_Dispo31_Materialbestand Dispo_CZ_30].Lagerplatz AS Lagerort
								, [PSZ_Dispo31_Materialbestand Dispo_CZ_30].Lagerort_id
								FROM  [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30] INNER JOIN [PSZ_Dispo31_Materialbestand Dispo_CZ_30] 
								ON  [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].[Artikel-Nr des Bauteils] = [PSZ_Dispo31_Materialbestand Dispo_CZ_30].[Artikel-Nr]
								GROUP BY  [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Artikelnummer_stücklisten
								, [PSZ_Dispo31_Materialbestand Dispo_CZ_30].Bestand, [PSZ_Dispo31_Materialbestand Dispo_CZ_30].Lagerplatz
								, [PSZ_Dispo31_Materialbestand Dispo_CZ_30].Lagerort_id
								HAVING (
								( [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Artikelnummer_stücklisten) ='{artikelnummer ?? ""}' 
								AND 
								(([PSZ_Dispo31_Materialbestand Dispo_CZ_30].Bestand)<>0)))
								select  Lagerort_id,Lagerort,Bestand from [PSZ_Disposition_Nettobedarfsermittlung Dispo Details II BETN] 
								order by Lagerort_id
								";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBestandEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsLieferantenEntity> Getws120Lieferanten(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
								with [PSZ_Disposition_Lieferanten zu PSZ Nr Dispo] as (
									SELECT 
									Artikel.Artikelnummer AS [PSZ#]
									, Bestellnummern.Standardlieferant
									, Bestellnummern.[Lieferanten-Nr]
									, adressen.Name1
									,CONCAT(
									adressen.Straße ,', ',  adressen.PLZ_Straße ,' ', adressen.Ort) AS Adresse
									, adressen.Telefon, adressen.Fax
									, adressen.eMail, Bestellnummern.[Bestell-Nr]
									, Bestellnummern.Einkaufspreis
									, Bestellnummern.Wiederbeschaffungszeitraum
									, Bestellnummern.Verpackungseinheit
									, Bestellnummern.Mindestbestellmenge
									FROM (Artikel LEFT JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) LEFT JOIN adressen 
									ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr
									WHERE Artikel.Artikelnummer = '{artikelnummer ?? ""}'
									)
									select 
									Name1
									,Adresse
									,IIF(Standardlieferant>0,'true','false') Standardlieferant
									,Wiederbeschaffungszeitraum
									,[Bestell-Nr]
									,Verpackungseinheit
									,Telefon
									,Einkaufspreis
									,Mindestbestellmenge
									,[Lieferanten-Nr]
									,count(*) over () TotalCount 
									from [PSZ_Disposition_Lieferanten zu PSZ Nr Dispo]
									order by Standardlieferant desc;
								";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsLieferantenEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsLieferantenEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsLieferantenEntity> Getws40Lieferanten(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
								with [PSZ_Disposition_Lieferanten zu PSZ Nr Dispo] as (
								SELECT
								Artikel.Artikelnummer AS [PSZ#]
								, Bestellnummern.Standardlieferant
								, Bestellnummern.[Lieferanten-Nr]
								, adressen.Name1
								,CONCAT(adressen.Straße ,', ',  adressen.PLZ_Straße ,' ', adressen.Ort) AS Adresse
								 ,adressen.Telefon
								  ,adressen.Fax
								  , adressen.eMail
								, Bestellnummern.[Bestell-Nr]
								, Bestellnummern.Einkaufspreis, Bestellnummern.Wiederbeschaffungszeitraum
								, Bestellnummern.Verpackungseinheit, Bestellnummern.Mindestbestellmenge
								FROM 
								(Artikel LEFT JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]) LEFT JOIN adressen 
								ON Bestellnummern.[Lieferanten-Nr] = adressen.Nr
								 WHERE Artikel.Artikelnummer = '{artikelnummer ?? ""}'
								)
								select 
								Name1
																	,Adresse
																	,IIF(Standardlieferant>0,'true','false') Standardlieferant
																	,Wiederbeschaffungszeitraum
																	,[Bestell-Nr]
																	,Verpackungseinheit
																	,Telefon
																	,Einkaufspreis
																	,Mindestbestellmenge
																	,[Lieferanten-Nr]
																	,count(*) over () TotalCount 
								from [PSZ_Disposition_Lieferanten zu PSZ Nr Dispo]
								order by Standardlieferant desc
								";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsLieferantenEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsLieferantenEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsOffenBestellungenEntity> Getws120OffneBestellungen(string artikelnummer, int LieferantenNr, Settings.PaginModel paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
							with [PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo] as (
								SELECT 
								Artikel.Artikelnummer AS [PSZ#]
								, Bestellungen.[Bestellung-Nr]
								, Bestellungen.Rahmenbestellung
								, Bestellungen.[Lieferanten-Nr]
								, [bestellte Artikel].[Start Anzahl] AS Bestellmenge
								, [bestellte Artikel].Anzahl AS Offen
								, [bestellte Artikel].Erhalten
								, [bestellte Artikel].Liefertermin
								, [bestellte Artikel].Nr
								, [bestellte Artikel].Einzelpreis
								, [bestellte Artikel].Lagerort_id
								, [bestellte Artikel].Bestätigter_Termin
								, [bestellte Artikel].[AB-Nr_Lieferant]
								FROM Artikel INNER JOIN (Bestellungen INNER JOIN [bestellte Artikel] 
								ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]) 
								ON Artikel.[Artikel-Nr] = [bestellte Artikel].[Artikel-Nr]
								WHERE (((Bestellungen.erledigt)=0) AND (([bestellte Artikel].erledigt_pos)=0)
								AND Artikel.Artikelnummer = '{artikelnummer}'
								AND [Lieferanten-Nr] = {LieferantenNr}
								AND ((Bestellungen.Typ)='Bestellung' Or (Bestellungen.Typ)='Kanbanabruf')) )
								SELECT 
								[PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo].[PSZ#]
								, [PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo].[Bestellung-Nr]
								, iif([PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo].Rahmenbestellung>0,'true','false')  Rahmenbestellung
								, [PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo].[Lieferanten-Nr]
								, [PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo].Bestellmenge
								, [PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo].Offen
								, [PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo].Erhalten
								, [PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo].Liefertermin
								, [PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo].Nr
								, [PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo].Einzelpreis
								, [PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo].Lagerort_id 
								, [PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo].Bestätigter_Termin 
								, [PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo].[AB-Nr_Lieferant] 
								,[Lieferanten-Nr]
								,Count(*) over() TotalCount
								FROM [PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo]
								ORDER BY Rahmenbestellung,Liefertermin
								OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY;
								";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsOffenBestellungenEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsOffenBestellungenEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsOffenBestellungenEntity> Getws40Bestellungen(string artikelnummer, int LieferantenNr, Settings.PaginModel paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
							WITH [PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo] as (
								SELECT 
								Artikel.Artikelnummer AS [PSZ#]
								, Bestellungen.[Bestellung-Nr]
								, Bestellungen.Rahmenbestellung
								, Bestellungen.[Lieferanten-Nr]
								, [bestellte Artikel].[Start Anzahl] AS Bestellmenge
								, [bestellte Artikel].Anzahl AS Offen
								, [bestellte Artikel].Erhalten
								, [bestellte Artikel].Liefertermin
								, [bestellte Artikel].Nr
								, [bestellte Artikel].Einzelpreis
								, [bestellte Artikel].Lagerort_id
								, [bestellte Artikel].Bestätigter_Termin
								, [bestellte Artikel].[AB-Nr_Lieferant]
								FROM 
								Artikel INNER JOIN (Bestellungen INNER JOIN [bestellte Artikel] 
								ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]) 
								ON Artikel.[Artikel-Nr] = [bestellte Artikel].[Artikel-Nr]
								WHERE ((
								(Bestellungen.erledigt)=0) 
								AND (([bestellte Artikel].erledigt_pos)=0) 
								AND ((Bestellungen.Typ)='Bestellung' Or (Bestellungen.Typ)='Kanbanabruf'))
								AND Artikel.Artikelnummer = '{artikelnummer}'
								AND [Lieferanten-Nr] ={LieferantenNr}
								)
								--ORDER BY Bestellungen.Rahmenbestellung, [bestellte Artikel].Liefertermin;

								SELECT 
								[PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo].[PSZ#]
								, [PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo].[Bestellung-Nr]
								, iif([PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo].Rahmenbestellung>0,'true','false')  Rahmenbestellung
								, [PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo].[Lieferanten-Nr]
								, [PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo].Bestellmenge
								, [PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo].Offen
								, [PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo].Erhalten
								, [PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo].Liefertermin
								, [PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo].Nr
								, [PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo].Einzelpreis
								, [PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo].Lagerort_id
								, [PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo].Bestätigter_Termin
								, [PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo].[AB-Nr_Lieferant]
								,[Lieferanten-Nr]
								,Count(*) over() TotalCount
								FROM [PSZ_Disposition_Bestellungen zu Lieferanten zu PSZ Nr Dispo]
								ORDER BY Rahmenbestellung,Liefertermin
								OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY;
								";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsOffenBestellungenEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsOffenBestellungenEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity> Getws120Bedarfe(string artikelnummer, Settings.PaginModel paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
						$@"
							with [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN] as (
								SELECT 
								[PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_KHTN].Stücklisten_Artikelnummer AS [PSZ#]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_KHTN].[Artikel-Nr des Bauteils]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_KHTN].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_KHTN].Fertigungsnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_KHTN].Artikelnummer AS Artikel_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_KHTN].[Bezeichnung 1]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_KHTN].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_KHTN].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_KHTN].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_KHTN].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_KHTN].Termin_Materialbedarf
								, (Select Sum(Bruttobedarf) FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_KHTN] AS Temp  
								WHERE Temp.Stücklisten_Artikelnummer = [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_KHTN].[Stücklisten_Artikelnummer] 
								AND Temp.Termin_Materialbedarf <= [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_KHTN].[Termin_Materialbedarf] ) AS [Laufende Summe]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_KHTN].Datum_Von
								FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_KHTN]
								GROUP BY [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_KHTN].Stücklisten_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_KHTN].[Artikel-Nr des Bauteils]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_KHTN].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_KHTN].Fertigungsnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_KHTN].Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_KHTN].[Bezeichnung 1]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_KHTN].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_KHTN].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_KHTN].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_KHTN].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_KHTN].Termin_Materialbedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_KHTN].Datum_Von
								HAVING ((
								([PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_KHTN].Stücklisten_Artikelnummer)='{artikelnummer}'
								)))
								SELECT 
								[PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN].[PSZ#] Psz
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN].[Artikel-Nr des Bauteils] Artikel_Nr_des_Bauteils
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN].Artikel_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN].[Bezeichnung 1] Bezeichnung1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN].Termin_Materialbedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN].[Laufende Summe] Laufende_Summe
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN].Fertigungsnummer 
								,[Bestand]-[Laufende Summe]	Verfügbar
								,count(*) over() TotalCount
								FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN] 
								ORDER BY [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN].Termin_Materialbedarf
								OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY;
								";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity> Getde90Bedarfe(string artikelnummer, Settings.PaginModel paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
						$@"
							with [PSZ_disposition_Nettobedarfsermittlung Dispo Details IV De] as (
							SELECT 
							[PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_De].Stücklisten_Artikelnummer AS [PSZ#]
							, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_De].[Artikel-Nr des Bauteils]
							, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_De].Termin_Bestätigt1
							, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_De].Fertigungsnummer
							, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_De].Artikelnummer AS Artikel_Artikelnummer
							, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_De].[Bezeichnung 1]
							, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_De].Fertigung_anzahl
							, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_De].Stücklisten_Anzahl
							, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_De].Bruttobedarf
							, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_De].Bestand
							, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_De].Termin_Materialbedarf
							, (Select Sum(Bruttobedarf) FROM [Psz_Disposition_Nettobedarfsermittlung Dispo Table FA_De] AS Temp 
							WHERE Temp.Stücklisten_Artikelnummer = [Psz_Disposition_Nettobedarfsermittlung Dispo Table FA_De].[Stücklisten_Artikelnummer] 
							AND Temp.Termin_Materialbedarf <= [Psz_Disposition_Nettobedarfsermittlung Dispo Table FA_De].[Termin_Materialbedarf] ) 
							AS [Laufende Summe], [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_De].Datum_Von
							FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_De]
							GROUP BY [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_De].Stücklisten_Artikelnummer, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_De].[Artikel-Nr des Bauteils], [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_De].Termin_Bestätigt1, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_De].Fertigungsnummer, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_De].Artikelnummer, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_De].[Bezeichnung 1], [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_De].Fertigung_anzahl, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_De].Stücklisten_Anzahl, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_De].Bruttobedarf, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_De].Bestand, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_De].Termin_Materialbedarf, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_De].Datum_Von
							HAVING ((([PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_De].Stücklisten_Artikelnummer)='{artikelnummer}'))
							)
							SELECT 
								[PSZ_disposition_Nettobedarfsermittlung Dispo Details IV De].[PSZ#] Psz
								, [PSZ_disposition_Nettobedarfsermittlung Dispo Details IV De].[Artikel-Nr des Bauteils] Artikel_Nr_des_Bauteils
								, [PSZ_disposition_Nettobedarfsermittlung Dispo Details IV De].Artikel_Artikelnummer
								, [PSZ_disposition_Nettobedarfsermittlung Dispo Details IV De].[Bezeichnung 1] Bezeichnung1
								, [PSZ_disposition_Nettobedarfsermittlung Dispo Details IV De].Fertigung_anzahl
								, [PSZ_disposition_Nettobedarfsermittlung Dispo Details IV De].Stücklisten_Anzahl
								, [PSZ_disposition_Nettobedarfsermittlung Dispo Details IV De].Bruttobedarf
								, [PSZ_disposition_Nettobedarfsermittlung Dispo Details IV De].Bestand
								, [PSZ_disposition_Nettobedarfsermittlung Dispo Details IV De].Termin_Materialbedarf
								, [PSZ_disposition_Nettobedarfsermittlung Dispo Details IV De].[Laufende Summe] Laufende_Summe
								, [PSZ_disposition_Nettobedarfsermittlung Dispo Details IV De].Termin_Bestätigt1
								, [PSZ_disposition_Nettobedarfsermittlung Dispo Details IV De].Fertigungsnummer 
								,[Bestand]-[Laufende Summe]	Verfügbar
								,count(*) over() TotalCount
								FROM [PSZ_disposition_Nettobedarfsermittlung Dispo Details IV De] 
								ORDER BY [PSZ_disposition_Nettobedarfsermittlung Dispo Details IV De].Termin_Materialbedarf
								OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY;
								";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity>();
			}
		}

		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity> GetGZ90Bedarfe(string artikelnummer, Settings.PaginModel paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
						$@"
							with [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN] as (
								SELECT 
								[PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZTN].Stücklisten_Artikelnummer AS [PSZ#]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZTN].[Artikel-Nr des Bauteils]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZTN].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZTN].Fertigungsnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZTN].Artikelnummer AS Artikel_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZTN].[Bezeichnung 1]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZTN].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZTN].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZTN].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZTN].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZTN].Termin_Materialbedarf
								, (Select Sum(Bruttobedarf) FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZTN] AS Temp  
								WHERE Temp.Stücklisten_Artikelnummer = [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZTN].[Stücklisten_Artikelnummer] 
								AND Temp.Termin_Materialbedarf <= [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZTN].[Termin_Materialbedarf] ) AS [Laufende Summe]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZTN].Datum_Von
								FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZTN]
								GROUP BY [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZTN].Stücklisten_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZTN].[Artikel-Nr des Bauteils]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZTN].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZTN].Fertigungsnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZTN].Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZTN].[Bezeichnung 1]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZTN].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZTN].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZTN].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZTN].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZTN].Termin_Materialbedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZTN].Datum_Von
								HAVING ((
								([PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZTN].Stücklisten_Artikelnummer)='{artikelnummer}'
								))
								)
								SELECT 
								[PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN].[PSZ#] Psz
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN].[Artikel-Nr des Bauteils] Artikel_Nr_des_Bauteils
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN].Artikel_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN].[Bezeichnung 1] Bezeichnung1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN].Termin_Materialbedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN].[Laufende Summe] Laufende_Summe
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN].Fertigungsnummer 
								,[Bestand]-[Laufende Summe]	Verfügbar
								,count(*) over() TotalCount
								FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN] 
								ORDER BY [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN].Termin_Materialbedarf
								OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY;
								";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity> GetGZ40Bedarfe(string artikelnummer, Settings.PaginModel paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
						$@"
							with [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN_40] as (
								SELECT 
								[PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZ_40].Stücklisten_Artikelnummer AS [PSZ#]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZ_40].[Artikel-Nr des Bauteils]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZ_40].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZ_40].Fertigungsnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZ_40].Artikelnummer AS Artikel_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZ_40].[Bezeichnung 1]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZ_40].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZ_40].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZ_40].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZ_40].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZ_40].Termin_Materialbedarf
								, (Select Sum(Bruttobedarf) FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZ_40] AS Temp  
								WHERE Temp.Stücklisten_Artikelnummer = [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZ_40].[Stücklisten_Artikelnummer] 
								AND Temp.Termin_Materialbedarf <= [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZ_40].[Termin_Materialbedarf] ) AS [Laufende Summe]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZ_40].Datum_Von
								FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZ_40]
								GROUP BY [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZ_40].Stücklisten_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZ_40].[Artikel-Nr des Bauteils]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZ_40].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZ_40].Fertigungsnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZ_40].Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZ_40].[Bezeichnung 1]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZ_40].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZ_40].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZ_40].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZ_40].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZ_40].Termin_Materialbedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZ_40].Datum_Von
								HAVING ((
								([PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_GZ_40].Stücklisten_Artikelnummer)='{artikelnummer}'
								))
								)
								SELECT 
								[PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN_40].[PSZ#] Psz
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN_40].[Artikel-Nr des Bauteils] Artikel_Nr_des_Bauteils
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN_40].Artikel_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN_40].[Bezeichnung 1] Bezeichnung1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN_40].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN_40].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN_40].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN_40].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN_40].Termin_Materialbedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN_40].[Laufende Summe] Laufende_Summe
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN_40].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN_40].Fertigungsnummer 
								,[Bestand]-[Laufende Summe]	Verfügbar
								,count(*) over() TotalCount
								FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN_40] 
								ORDER BY [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV GZTN_40].Termin_Materialbedarf
								OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY;
								";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity> Getws40Bedarfe(string artikelnummer, Settings.PaginModel paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
						$@"
							WITH [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN_40] as (
								SELECT 
								[PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_WS_40].Stücklisten_Artikelnummer AS [PSZ#]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_WS_40].[Artikel-Nr des Bauteils]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_WS_40].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_WS_40].Fertigungsnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_WS_40].Artikelnummer AS Artikel_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_WS_40].[Bezeichnung 1]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_WS_40].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_WS_40].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_WS_40].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_WS_40].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_WS_40].Termin_Materialbedarf
								, (Select Sum(Bruttobedarf) FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_WS_40] AS Temp 
								WHERE 
								Temp.Stücklisten_Artikelnummer = [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_WS_40].[Stücklisten_Artikelnummer] 
								AND 
								Temp.Termin_Materialbedarf <= [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_WS_40].Termin_Materialbedarf ) AS [Laufende Summe]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_WS_40].Datum_Von
								FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_WS_40]
								GROUP BY 
								[PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_WS_40].Stücklisten_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_WS_40].[Artikel-Nr des Bauteils]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_WS_40].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_WS_40].Fertigungsnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_WS_40].Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_WS_40].[Bezeichnung 1]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_WS_40].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_WS_40].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_WS_40].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_WS_40].Bestand
								,[PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_WS_40].Termin_Materialbedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_WS_40].Datum_Von
								HAVING 
								((
								([PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_WS_40].Stücklisten_Artikelnummer)='{artikelnummer}')
								))

								SELECT
								[PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN_40].[PSZ#] Psz
																, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN_40].[Artikel-Nr des Bauteils] Artikel_Nr_des_Bauteils
																, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN_40].Artikel_Artikelnummer
																, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN_40].[Bezeichnung 1] Bezeichnung1
																, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN_40].Fertigung_anzahl
																, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN_40].Stücklisten_Anzahl
																, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN_40].Bruttobedarf
																, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN_40].Bestand
																, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN_40].Termin_Materialbedarf
																, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN_40].[Laufende Summe] Laufende_Summe
																, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN_40].Termin_Bestätigt1
																, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN_40].Fertigungsnummer 
																,[Bestand]-[Laufende Summe]	Verfügbar
																,count(*) over() TotalCount
								FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN_40]
								ORDER BY [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV KHTN_40].Termin_Materialbedarf
								OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY;
								";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity> GetBETN90Bedarfe(string artikelnummer, Settings.PaginModel paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
						$@"
							with [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN] as (
								SELECT 
								[PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN].Stücklisten_Artikelnummer AS [PSZ#]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN].[Artikel-Nr des Bauteils]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN].Fertigungsnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN].Artikelnummer AS Artikel_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN].[Bezeichnung 1]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN].Termin_Materialbedarf
								, (Select Sum(Bruttobedarf) FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN] AS Temp  
								WHERE Temp.Stücklisten_Artikelnummer = [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN].[Stücklisten_Artikelnummer] 
								AND Temp.Termin_Materialbedarf <= [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN].[Termin_Materialbedarf] ) AS [Laufende Summe]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN].Datum_Von
								FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN]
								GROUP BY [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN].Stücklisten_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN].[Artikel-Nr des Bauteils]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN].Fertigungsnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN].Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN].[Bezeichnung 1]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN].Termin_Materialbedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN].Datum_Von
								HAVING ((
								([PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN].Stücklisten_Artikelnummer)='{artikelnummer}'
								))
								)
								SELECT 
								[PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN].[PSZ#] Psz
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN].[Artikel-Nr des Bauteils] Artikel_Nr_des_Bauteils
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN].Artikel_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN].[Bezeichnung 1] Bezeichnung1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN].Termin_Materialbedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN].[Laufende Summe] Laufende_Summe
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN].Fertigungsnummer 
								,[Bestand]-[Laufende Summe]	Verfügbar
								,count(*) over() TotalCount
								FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN] 
								ORDER BY [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN].Termin_Materialbedarf
								OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY;
								";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity> GetBETN40Bedarfe(string artikelnummer, Settings.PaginModel paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
						$@"
							with [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN_40] as (
								SELECT 
								[PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN_40].Stücklisten_Artikelnummer AS [PSZ#]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN_40].[Artikel-Nr des Bauteils]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN_40].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN_40].Fertigungsnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN_40].Artikelnummer AS Artikel_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN_40].[Bezeichnung 1]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN_40].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN_40].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN_40].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN_40].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN_40].Termin_Materialbedarf
								, (Select Sum(Bruttobedarf) FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN_40] AS Temp  
								WHERE Temp.Stücklisten_Artikelnummer = [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN_40].[Stücklisten_Artikelnummer] 
								AND Temp.Termin_Materialbedarf <= [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN_40].[Termin_Materialbedarf] ) AS [Laufende Summe]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN_40].Datum_Von
								FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN_40]
								GROUP BY [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN_40].Stücklisten_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN_40].[Artikel-Nr des Bauteils]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN_40].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN_40].Fertigungsnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN_40].Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN_40].[Bezeichnung 1]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN_40].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN_40].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN_40].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN_40].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN_40].Termin_Materialbedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN_40].Datum_Von
								HAVING ((
								([PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_BETN_40].Stücklisten_Artikelnummer)='{artikelnummer}'
								))
								)
								SELECT 
								[PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN_40].[PSZ#] Psz
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN_40].[Artikel-Nr des Bauteils] Artikel_Nr_des_Bauteils
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN_40].Artikel_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN_40].[Bezeichnung 1] Bezeichnung1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN_40].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN_40].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN_40].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN_40].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN_40].Termin_Materialbedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN_40].[Laufende Summe] Laufende_Summe
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN_40].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN_40].Fertigungsnummer 
								,[Bestand]-[Laufende Summe]	Verfügbar
								,count(*) over() TotalCount
								FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN_40] 
								ORDER BY [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV BETN_40].Termin_Materialbedarf
								OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY;";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity> GetAL90Bedarfe(string artikelnummer, Settings.PaginModel paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
						$@"
							with [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL] as (
								SELECT 
								[PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL].Stücklisten_Artikelnummer AS [PSZ#]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL].[Artikel-Nr des Bauteils]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL].Fertigungsnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL].Artikelnummer AS Artikel_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL].[Bezeichnung 1]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL].Termin_Materialbedarf
								, (Select Sum(Bruttobedarf) FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL] AS Temp  
								WHERE Temp.Stücklisten_Artikelnummer = [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL].[Stücklisten_Artikelnummer] 
								AND Temp.Termin_Materialbedarf <= [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL].[Termin_Materialbedarf] ) AS [Laufende Summe]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL].Datum_Von
								FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL]
								GROUP BY [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL].Stücklisten_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL].[Artikel-Nr des Bauteils]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL].Fertigungsnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL].Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL].[Bezeichnung 1]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL].Termin_Materialbedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL].Datum_Von
								HAVING ((
								([PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL].Stücklisten_Artikelnummer)='{artikelnummer}'
								))
								)
								SELECT 
								[PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL].[PSZ#] Psz
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL].[Artikel-Nr des Bauteils] Artikel_Nr_des_Bauteils
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL].Artikel_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL].[Bezeichnung 1] Bezeichnung1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL].Termin_Materialbedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL].[Laufende Summe] Laufende_Summe
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL].Fertigungsnummer 
								,[Bestand]-[Laufende Summe]	Verfügbar
								,count(*) over() TotalCount
								FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL] 
								ORDER BY [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL].Termin_Materialbedarf
								OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY;";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity> GetAL30Bedarfe(string artikelnummer, Settings.PaginModel paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
						$@"
							with [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL_30] as (
								SELECT 
								[PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL_30].Stücklisten_Artikelnummer AS [PSZ#]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL_30].[Artikel-Nr des Bauteils]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL_30].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL_30].Fertigungsnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL_30].Artikelnummer AS Artikel_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL_30].[Bezeichnung 1]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL_30].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL_30].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL_30].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL_30].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL_30].Termin_Materialbedarf
								, (Select Sum(Bruttobedarf) FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL_30] AS Temp  
								WHERE Temp.Stücklisten_Artikelnummer = [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL_30].[Stücklisten_Artikelnummer] 
								AND Temp.Termin_Materialbedarf <= [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL_30].[Termin_Materialbedarf] ) AS [Laufende Summe]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL_30].Datum_Von
								FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL_30]
								GROUP BY [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL_30].Stücklisten_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL_30].[Artikel-Nr des Bauteils]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL_30].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL_30].Fertigungsnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL_30].Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL_30].[Bezeichnung 1]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL_30].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL_30].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL_30].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL_30].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL_30].Termin_Materialbedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL_30].Datum_Von
								HAVING ((
								([PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_AL_30].Stücklisten_Artikelnummer)='{artikelnummer}'
								))
								)
								SELECT 
								[PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL_30].[PSZ#] Psz
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL_30].[Artikel-Nr des Bauteils] Artikel_Nr_des_Bauteils
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL_30].Artikel_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL_30].[Bezeichnung 1] Bezeichnung1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL_30].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL_30].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL_30].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL_30].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL_30].Termin_Materialbedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL_30].[Laufende Summe] Laufende_Summe
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL_30].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL_30].Fertigungsnummer 
								,[Bestand]-[Laufende Summe]	Verfügbar
								,count(*) over() TotalCount
								FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL_30] 
								ORDER BY [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV AL_30].Termin_Materialbedarf
								OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY;";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity>();
			}
		}

		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity> GetTN90Bedarfe(string artikelnummer, Settings.PaginModel paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
						$@"
							with [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN] as (
								SELECT 
								[PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn].Stücklisten_Artikelnummer AS [PSZ#]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn].[Artikel-Nr des Bauteils]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn].Fertigungsnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn].Artikelnummer AS Artikel_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn].[Bezeichnung 1]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn].Termin_Materialbedarf
								, (Select Sum(Bruttobedarf) FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn] AS Temp  
								WHERE Temp.Stücklisten_Artikelnummer = [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn].[Stücklisten_Artikelnummer] 
								AND Temp.Termin_Materialbedarf <= [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn].[Termin_Materialbedarf] ) AS [Laufende Summe]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn].Datum_Von
								FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn]
								GROUP BY [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn].Stücklisten_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn].[Artikel-Nr des Bauteils]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn].Fertigungsnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn].Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn].[Bezeichnung 1]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn].Termin_Materialbedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn].Datum_Von
								HAVING ((
								([PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn].Stücklisten_Artikelnummer)='{artikelnummer}'
								))
								)
								SELECT 
								[PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN].[PSZ#] Psz
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN].[Artikel-Nr des Bauteils] Artikel_Nr_des_Bauteils
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN].Artikel_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN].[Bezeichnung 1] Bezeichnung1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN].Termin_Materialbedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN].[Laufende Summe] Laufende_Summe
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN].Fertigungsnummer 
								,[Bestand]-[Laufende Summe]	Verfügbar
								,count(*) over() TotalCount
								FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN] 
								ORDER BY [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN].Termin_Materialbedarf
								OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY;";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity> GetTN40Bedarfe(string artikelnummer, Settings.PaginModel paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
						$@"
							with [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN_30] as (
								SELECT 
								[PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn_30].Stücklisten_Artikelnummer AS [PSZ#]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn_30].[Artikel-Nr des Bauteils]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn_30].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn_30].Fertigungsnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn_30].Artikelnummer AS Artikel_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn_30].[Bezeichnung 1]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn_30].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn_30].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn_30].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn_30].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn_30].Termin_Materialbedarf
								, (Select Sum(Bruttobedarf) FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn_30] AS Temp  
								WHERE Temp.Stücklisten_Artikelnummer = [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn_30].[Stücklisten_Artikelnummer] 
								AND Temp.Termin_Materialbedarf <= [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn_30].[Termin_Materialbedarf] ) AS [Laufende Summe]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn_30].Datum_Von
								FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn_30]
								GROUP BY [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn_30].Stücklisten_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn_30].[Artikel-Nr des Bauteils]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn_30].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn_30].Fertigungsnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn_30].Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn_30].[Bezeichnung 1]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn_30].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn_30].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn_30].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn_30].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn_30].Termin_Materialbedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn_30].Datum_Von
								HAVING ((
								([PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_Tn_30].Stücklisten_Artikelnummer)='{artikelnummer}'
								))
								)
								SELECT 
								[PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN_30].[PSZ#] Psz
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN_30].[Artikel-Nr des Bauteils] Artikel_Nr_des_Bauteils
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN_30].Artikel_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN_30].[Bezeichnung 1] Bezeichnung1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN_30].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN_30].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN_30].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN_30].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN_30].Termin_Materialbedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN_30].[Laufende Summe] Laufende_Summe
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN_30].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN_30].Fertigungsnummer 
								,[Bestand]-[Laufende Summe]	Verfügbar
								,count(*) over() TotalCount
								FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN_30] 
								ORDER BY [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV TN_30].Termin_Materialbedarf
								OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY;";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity> GetCZ90Bedarfe(string artikelnummer, Settings.PaginModel paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
						$@"
							with [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ] as (
								SELECT 
								[PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ].Stücklisten_Artikelnummer AS [PSZ#]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ].[Artikel-Nr des Bauteils]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ].Fertigungsnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ].Artikelnummer AS Artikel_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ].[Bezeichnung 1]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ].Termin_Materialbedarf
								, (Select Sum(Bruttobedarf) FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ] AS Temp  
								WHERE Temp.Stücklisten_Artikelnummer = [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ].[Stücklisten_Artikelnummer] 
								AND Temp.Termin_Materialbedarf <= [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ].[Termin_Materialbedarf] ) AS [Laufende Summe]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ].Datum_Von
								FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ]
								GROUP BY [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ].Stücklisten_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ].[Artikel-Nr des Bauteils]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ].Fertigungsnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ].Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ].[Bezeichnung 1]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ].Termin_Materialbedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ].Datum_Von
								HAVING ((
								([PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ].Stücklisten_Artikelnummer)='{artikelnummer}'
								))
								)
								SELECT 
								[PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ].[PSZ#] Psz
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ].[Artikel-Nr des Bauteils] Artikel_Nr_des_Bauteils
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ].Artikel_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ].[Bezeichnung 1] Bezeichnung1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ].Termin_Materialbedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ].[Laufende Summe] Laufende_Summe
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ].Fertigungsnummer 
								,[Bestand]-[Laufende Summe]	Verfügbar
								,count(*) over() TotalCount
								FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ] 
								ORDER BY [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ].Termin_Materialbedarf
								OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY;";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity> GetCZ30Bedarfe(string artikelnummer, Settings.PaginModel paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
						$@"
							with [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ_30] as (
								SELECT 
								[PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ_30].Stücklisten_Artikelnummer AS [PSZ#]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ_30].[Artikel-Nr des Bauteils]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ_30].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ_30].Fertigungsnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ_30].Artikelnummer AS Artikel_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ_30].[Bezeichnung 1]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ_30].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ_30].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ_30].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ_30].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ_30].Termin_Materialbedarf
								, (Select Sum(Bruttobedarf) FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ_30] AS Temp  
								WHERE Temp.Stücklisten_Artikelnummer = [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ_30].[Stücklisten_Artikelnummer] 
								AND Temp.Termin_Materialbedarf <= [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ_30].[Termin_Materialbedarf] ) AS [Laufende Summe]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ_30].Datum_Von
								FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ_30]
								GROUP BY [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ_30].Stücklisten_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ_30].[Artikel-Nr des Bauteils]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ_30].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ_30].Fertigungsnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ_30].Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ_30].[Bezeichnung 1]
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ_30].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ_30].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ_30].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ_30].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ_30].Termin_Materialbedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ_30].Datum_Von
								HAVING ((
								([PSZ_Disposition_Nettobedarfsermittlung Dispo Table FA_CZ_30].Stücklisten_Artikelnummer)='{artikelnummer}'
								))
								)
								SELECT 
								[PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ_30].[PSZ#] Psz
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ_30].[Artikel-Nr des Bauteils] Artikel_Nr_des_Bauteils
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ_30].Artikel_Artikelnummer
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ_30].[Bezeichnung 1] Bezeichnung1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ_30].Fertigung_anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ_30].Stücklisten_Anzahl
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ_30].Bruttobedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ_30].Bestand
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ_30].Termin_Materialbedarf
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ_30].[Laufende Summe] Laufende_Summe
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ_30].Termin_Bestätigt1
								, [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ_30].Fertigungsnummer 
								,[Bestand]-[Laufende Summe]	Verfügbar
								,count(*) over() TotalCount
								FROM [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ_30] 
								ORDER BY [PSZ_Disposition_Nettobedarfsermittlung Dispo Details IV CZ_30].Termin_Materialbedarf
								OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY;";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.Dispows120DetailsBedarfeEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity> GetFaultyArtikelNummers120(string Filter)
		{
			var dataTable = new DataTable();



			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
						with [Psz_disposition Anlayse I KHTN] as (
						SELECT 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Artikelnummer_stücklisten AS Stücklisten_Artikelnummer,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Fertigungsnummer,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Bestand,
							Lagerorte.Lagerort_id,
							Lagerorte.Lagerort, 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Bruttobedarf,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Termin_Materialbedarf,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Standardlieferant,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Name1,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].bearbeitet,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Mindestbestellmenge,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Rahmen,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].[Rahmen-Nr],
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Rahmenmenge,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Rahmenauslauf,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Termin_Bestätigt1
						FROM Lagerorte INNER JOIN [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN] ON Lagerorte.Lagerort_id = [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Lagerort_id
						GROUP BY [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Artikelnummer_stücklisten, 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Fertigungsnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Bestand
							, Lagerorte.Lagerort_id, Lagerorte.Lagerort
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Bruttobedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Termin_Materialbedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Standardlieferant
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Name1
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].bearbeitet
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Mindestbestellmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Rahmen
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].[Rahmen-Nr]
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Rahmenmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Rahmenauslauf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Termin_Bestätigt1
						HAVING ((([Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Artikelnummer_stücklisten)<>'reparatur') 
						AND (([Psz_disposition_nettobedarfsermittlung Dispo Table_KHTN].Standardlieferant)=1))
						)
						, Lager_Obsolet as (
							SELECT Lager.[Artikel-Nr], Lager.Lagerort_id, Lager.Bestand
							FROM Lager
							WHERE (((Lager.Lagerort_id)=22) AND ((Lager.Bestand)>0))
						)

						, [PSz_disposition_Anlayse II KHTN] as (
								SELECT 
								[Psz_disposition Anlayse I KHTN].Name1,
								[Psz_disposition Anlayse I KHTN].Stücklisten_Artikelnummer,
								Sum([Psz_disposition Anlayse I KHTN].Bruttobedarf) AS SummevonBruttobedarf,
								[Psz_disposition Anlayse I KHTN].Lagerort_id,
								[Psz_disposition Anlayse I KHTN].Lagerort,
								[Psz_disposition Anlayse I KHTN].Bestand,
								Max([Psz_disposition Anlayse I KHTN].Termin_Materialbedarf) AS MaxvonTermin_Materialbedarf,
								Artikel.[Bezeichnung 1] AS [Bezeichnung des Bauteils],
								[Psz_disposition Anlayse I KHTN].bearbeitet,
								[Psz_disposition Anlayse I KHTN].Mindestbestellmenge,
								Artikel.Rahmen, Artikel.[Rahmen-Nr],
								Artikel.Rahmenmenge,
								Artikel.Rahmenauslauf
						,Lager_Obsolet.Bestand AS Betand_Obsolete
						FROM ([Psz_disposition Anlayse I KHTN] INNER JOIN Artikel ON [Psz_disposition Anlayse I KHTN].Stücklisten_Artikelnummer = Artikel.Artikelnummer) LEFT JOIN Lager_Obsolet ON Artikel.[Artikel-Nr] = Lager_Obsolet.[Artikel-Nr]
						GROUP BY [Psz_disposition Anlayse I KHTN].Name1, [Psz_disposition Anlayse I KHTN].Stücklisten_Artikelnummer, [Psz_disposition Anlayse I KHTN].Lagerort_id, [Psz_disposition Anlayse I KHTN].Lagerort, [Psz_disposition Anlayse I KHTN].Bestand, Artikel.[Bezeichnung 1], [Psz_disposition Anlayse I KHTN].bearbeitet, [Psz_disposition Anlayse I KHTN].Mindestbestellmenge, Artikel.Rahmen, Artikel.[Rahmen-Nr], Artikel.Rahmenmenge, Artikel.Rahmenauslauf, Lager_Obsolet.Bestand
						)
						select top 10 
								daws.Stücklisten_Artikelnummer Artikelnummer
						from [PSz_disposition_Anlayse II KHTN]  daws
						where Stücklisten_Artikelnummer like '{Filter.SqlEscape()}%'
						order by Stücklisten_Artikelnummer
						";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity> GetFaultyArtikelNummersGZ90(string Filter)
		{
			var dataTable = new DataTable();



			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
						with [Psz_disposition Anlayse I GZTN] as (
						SELECT 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Artikelnummer_stücklisten AS Stücklisten_Artikelnummer,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Fertigungsnummer,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Bestand,
							Lagerorte.Lagerort_id,
							Lagerorte.Lagerort, 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Bruttobedarf,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Termin_Materialbedarf,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Standardlieferant,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Name1,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].bearbeitet,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Mindestbestellmenge,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Rahmen,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].[Rahmen-Nr],
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Rahmenmenge,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Rahmenauslauf,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Termin_Bestätigt1
						FROM Lagerorte INNER JOIN [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN] ON Lagerorte.Lagerort_id = [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Lagerort_id
						GROUP BY [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Artikelnummer_stücklisten, 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Fertigungsnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Bestand
							, Lagerorte.Lagerort_id, Lagerorte.Lagerort
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Bruttobedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Termin_Materialbedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Standardlieferant
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Name1
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].bearbeitet
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Mindestbestellmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Rahmen
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].[Rahmen-Nr]
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Rahmenmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Rahmenauslauf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Termin_Bestätigt1
						HAVING 
						(
						(([Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Artikelnummer_stücklisten)<>'reparatur') 
						AND (([Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN].Standardlieferant)=1))
						)
						, Lager_Obsolet as (
							SELECT Lager.[Artikel-Nr], Lager.Lagerort_id, Lager.Bestand
							FROM Lager
							WHERE (((Lager.Lagerort_id)=22) AND ((Lager.Bestand)>0))
						)

						, [PSz_disposition_Anlayse II GZTN] as (
								SELECT 
								[Psz_disposition Anlayse I GZTN].Name1,
								[Psz_disposition Anlayse I GZTN].Stücklisten_Artikelnummer,
								Sum([Psz_disposition Anlayse I GZTN].Bruttobedarf) AS SummevonBruttobedarf,
								[Psz_disposition Anlayse I GZTN].Lagerort_id,
								[Psz_disposition Anlayse I GZTN].Lagerort,
								[Psz_disposition Anlayse I GZTN].Bestand,
								Max([Psz_disposition Anlayse I GZTN].Termin_Materialbedarf) AS MaxvonTermin_Materialbedarf,
								Artikel.[Bezeichnung 1] AS [Bezeichnung des Bauteils],
								[Psz_disposition Anlayse I GZTN].bearbeitet,
								[Psz_disposition Anlayse I GZTN].Mindestbestellmenge,
								Artikel.Rahmen, Artikel.[Rahmen-Nr],
								Artikel.Rahmenmenge,
								Artikel.Rahmenauslauf
						,Lager_Obsolet.Bestand AS Betand_Obsolete
						FROM ([Psz_disposition Anlayse I GZTN] INNER JOIN Artikel ON [Psz_disposition Anlayse I GZTN].Stücklisten_Artikelnummer = Artikel.Artikelnummer) LEFT JOIN Lager_Obsolet ON Artikel.[Artikel-Nr] = Lager_Obsolet.[Artikel-Nr]
						GROUP BY [Psz_disposition Anlayse I GZTN].Name1, [Psz_disposition Anlayse I GZTN].Stücklisten_Artikelnummer, [Psz_disposition Anlayse I GZTN].Lagerort_id, [Psz_disposition Anlayse I GZTN].Lagerort, [Psz_disposition Anlayse I GZTN].Bestand, Artikel.[Bezeichnung 1], [Psz_disposition Anlayse I GZTN].bearbeitet, [Psz_disposition Anlayse I GZTN].Mindestbestellmenge, Artikel.Rahmen, Artikel.[Rahmen-Nr], Artikel.Rahmenmenge, Artikel.Rahmenauslauf, Lager_Obsolet.Bestand
						)
						select top 10 
								daws.Stücklisten_Artikelnummer Artikelnummer
						from [PSz_disposition_Anlayse II GZTN]  daws
						where Stücklisten_Artikelnummer like '{Filter.SqlEscape()}%'
						order by Stücklisten_Artikelnummer
						";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity> GetFaultyArtikelNummersGZ40(string Filter)
		{
			var dataTable = new DataTable();



			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
						with [Psz_disposition Anlayse I GZTN_40] as (
						SELECT 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Artikelnummer_stücklisten AS Stücklisten_Artikelnummer,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Fertigungsnummer,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Bestand,
							Lagerorte.Lagerort_id,
							Lagerorte.Lagerort, 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Bruttobedarf,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Termin_Materialbedarf,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Standardlieferant,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Name1,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].bearbeitet,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Mindestbestellmenge,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Rahmen,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].[Rahmen-Nr],
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Rahmenmenge,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Rahmenauslauf,
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Termin_Bestätigt1
						FROM Lagerorte INNER JOIN [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40] ON Lagerorte.Lagerort_id = [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Lagerort_id
						GROUP BY [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Artikelnummer_stücklisten, 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Fertigungsnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Bestand
							, Lagerorte.Lagerort_id, Lagerorte.Lagerort
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Bruttobedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Termin_Materialbedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Standardlieferant
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Name1
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].bearbeitet
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Mindestbestellmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Rahmen
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].[Rahmen-Nr]
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Rahmenmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Rahmenauslauf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Termin_Bestätigt1
						HAVING 
						(
						(([Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Artikelnummer_stücklisten)<>'reparatur') 
						AND (([Psz_disposition_nettobedarfsermittlung Dispo Table_GZTN_40].Standardlieferant)=1))
						)
						, Lager_Obsolet as (
							SELECT Lager.[Artikel-Nr], Lager.Lagerort_id, Lager.Bestand
							FROM Lager
							WHERE (((Lager.Lagerort_id)=22) AND ((Lager.Bestand)>0))
						)

						, [PSz_disposition_Anlayse II GZTN_40] as (
								SELECT 
								[Psz_disposition Anlayse I GZTN_40].Name1,
								[Psz_disposition Anlayse I GZTN_40].Stücklisten_Artikelnummer,
								Sum([Psz_disposition Anlayse I GZTN_40].Bruttobedarf) AS SummevonBruttobedarf,
								[Psz_disposition Anlayse I GZTN_40].Lagerort_id,
								[Psz_disposition Anlayse I GZTN_40].Lagerort,
								[Psz_disposition Anlayse I GZTN_40].Bestand,
								Max([Psz_disposition Anlayse I GZTN_40].Termin_Materialbedarf) AS MaxvonTermin_Materialbedarf,
								Artikel.[Bezeichnung 1] AS [Bezeichnung des Bauteils],
								[Psz_disposition Anlayse I GZTN_40].bearbeitet,
								[Psz_disposition Anlayse I GZTN_40].Mindestbestellmenge,
								Artikel.Rahmen, Artikel.[Rahmen-Nr],
								Artikel.Rahmenmenge,
								Artikel.Rahmenauslauf
						,Lager_Obsolet.Bestand AS Betand_Obsolete
						FROM ([Psz_disposition Anlayse I GZTN_40] INNER JOIN Artikel ON [Psz_disposition Anlayse I GZTN_40].Stücklisten_Artikelnummer = Artikel.Artikelnummer) LEFT JOIN Lager_Obsolet ON Artikel.[Artikel-Nr] = Lager_Obsolet.[Artikel-Nr]
						GROUP BY [Psz_disposition Anlayse I GZTN_40].Name1, [Psz_disposition Anlayse I GZTN_40].Stücklisten_Artikelnummer, [Psz_disposition Anlayse I GZTN_40].Lagerort_id, [Psz_disposition Anlayse I GZTN_40].Lagerort, [Psz_disposition Anlayse I GZTN_40].Bestand, Artikel.[Bezeichnung 1], [Psz_disposition Anlayse I GZTN_40].bearbeitet, [Psz_disposition Anlayse I GZTN_40].Mindestbestellmenge, Artikel.Rahmen, Artikel.[Rahmen-Nr], Artikel.Rahmenmenge, Artikel.Rahmenauslauf, Lager_Obsolet.Bestand
						)
						select top 10 
								daws.Stücklisten_Artikelnummer Artikelnummer
						from [PSz_disposition_Anlayse II GZTN_40]  daws
						where Stücklisten_Artikelnummer like '{Filter.SqlEscape()}%'
						order by Stücklisten_Artikelnummer
						";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity>();
			}
		}

		public static List<Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity> GetFaultyArtikelNummers40(string Filter)
		{
			var dataTable = new DataTable();



			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
						with  [Psz_disposition Anlayse I KHTN_40] as 
										(
										SELECT 
										[Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Artikelnummer_stücklisten AS Stücklisten_Artikelnummer
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Fertigungsnummer
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Bestand
										, Lagerorte.Lagerort_id, Lagerorte.Lagerort
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Bruttobedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Termin_Materialbedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Standardlieferant
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Name1
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].bearbeitet
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Mindestbestellmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Rahmen
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].[Rahmen-Nr]
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Rahmenmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Rahmenauslauf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Termin_Bestätigt1
										FROM Lagerorte INNER JOIN [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40] 
										ON Lagerorte.Lagerort_id = [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Lagerort_id
										GROUP BY [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Artikelnummer_stücklisten
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Fertigungsnummer
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Bestand
										, Lagerorte.Lagerort_id, Lagerorte.Lagerort
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Bruttobedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Termin_Materialbedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Standardlieferant
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Name1
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].bearbeitet
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Mindestbestellmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Rahmen
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].[Rahmen-Nr]
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Rahmenmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Rahmenauslauf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Termin_Bestätigt1
										HAVING ((([Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Artikelnummer_stücklisten)<>'reparatur') 
										AND (([Psz_disposition_nettobedarfsermittlung Dispo Table_KHTn_40].Standardlieferant)=1))
										)

										,Lager_Obsolet as (
										SELECT Lager.[Artikel-Nr]
										, Lager.Lagerort_id
										, Lager.Bestand
										FROM Lager
										WHERE (((Lager.Lagerort_id)=22) AND ((Lager.Bestand)>0)))
										, [PSz_disposition_Anlayse II KHTN_40] as
										(SELECT
										[Psz_disposition Anlayse I KHTN_40].Name1,
										[Psz_disposition Anlayse I KHTN_40].Stücklisten_Artikelnummer
										, Sum([Psz_disposition Anlayse I KHTN_40].Bruttobedarf) AS SummevonBruttobedarf
										, [Psz_disposition Anlayse I KHTN_40].Lagerort_id, [Psz_disposition Anlayse I KHTN_40].Lagerort
										, [Psz_disposition Anlayse I KHTN_40].Bestand
										, Max([Psz_disposition Anlayse I KHTN_40].Termin_Materialbedarf) AS MaxvonTermin_Materialbedarf
										, Artikel.[Bezeichnung 1] AS [Bezeichnung des Bauteils]
										, [Psz_disposition Anlayse I KHTN_40].bearbeitet
										, [Psz_disposition Anlayse I KHTN_40].Mindestbestellmenge
										, Artikel.Rahmen, Artikel.[Rahmen-Nr], Artikel.Rahmenmenge
										, Artikel.Rahmenauslauf, Lager_Obsolet.Bestand AS Betand_Obsolete
										FROM (Artikel LEFT JOIN Lager_Obsolet ON Artikel.[Artikel-Nr] = Lager_Obsolet.[Artikel-Nr]) INNER JOIN [Psz_disposition Anlayse I KHTN_40] 
										ON Artikel.Artikelnummer = [Psz_disposition Anlayse I KHTN_40].Stücklisten_Artikelnummer
										GROUP BY [Psz_disposition Anlayse I KHTN_40].Name1
										, [Psz_disposition Anlayse I KHTN_40].Stücklisten_Artikelnummer
										, [Psz_disposition Anlayse I KHTN_40].Lagerort_id
										, [Psz_disposition Anlayse I KHTN_40].Lagerort
										, [Psz_disposition Anlayse I KHTN_40].Bestand
										, Artikel.[Bezeichnung 1]
										, [Psz_disposition Anlayse I KHTN_40].bearbeitet
										, [Psz_disposition Anlayse I KHTN_40].Mindestbestellmenge
										, Artikel.Rahmen, Artikel.[Rahmen-Nr], Artikel.Rahmenmenge
										, Artikel.Rahmenauslauf, Lager_Obsolet.Bestand
										)
										select top 10
										dkt40.Stücklisten_Artikelnummer Artikelnummer
										from [PSz_disposition_Anlayse II KHTN_40] dkt40
										where dkt40.Stücklisten_Artikelnummer like  '{Filter.SqlEscape()}%'
									order by dkt40.Stücklisten_Artikelnummer ";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity> GetFaultyArtikelNummersTN90(string Filter)
		{
			var dataTable = new DataTable();



			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
						with [Psz_disposition Anlayse I TN] as (

							SELECT 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Artikelnummer_stücklisten AS Stücklisten_Artikelnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Fertigungsnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Bestand
							, Lagerorte.Lagerort_id
							, Lagerorte.Lagerort
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Bruttobedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Termin_Materialbedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Standardlieferant
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Name1
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].bearbeitet
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Mindestbestellmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Rahmen
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].[Rahmen-Nr]
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Rahmenmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Rahmenauslauf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Termin_Bestätigt1
							FROM 
							Lagerorte INNER JOIN [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn] 
							ON Lagerorte.Lagerort_id = [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Lagerort_id
							GROUP BY 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Artikelnummer_stücklisten
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Fertigungsnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Bestand
							, Lagerorte.Lagerort_id, Lagerorte.Lagerort
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Bruttobedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Termin_Materialbedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Standardlieferant
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Name1
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].bearbeitet
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Mindestbestellmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Rahmen
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].[Rahmen-Nr]
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Rahmenmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Rahmenauslauf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Termin_Bestätigt1
							HAVING ((
							([Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Artikelnummer_stücklisten)<>'reparatur')
							AND (([Psz_disposition_nettobedarfsermittlung Dispo Table_Tn].Standardlieferant)=1)))
							, Lager_Obsolet as
							(
							SELECT Lager.[Artikel-Nr], Lager.Lagerort_id, Lager.Bestand
							FROM Lager
							WHERE (((Lager.Lagerort_id)=22) AND ((Lager.Bestand)>0))
							)
							, [PSz_disposition_Anlayse II TN] as (


							SELECT 
							[Psz_disposition Anlayse I TN].Name1
							, [Psz_disposition Anlayse I TN].Stücklisten_Artikelnummer
							, Sum([Psz_disposition Anlayse I TN].Bruttobedarf) AS SummevonBruttobedarf
							, [Psz_disposition Anlayse I TN].Lagerort_id
							, [Psz_disposition Anlayse I TN].Lagerort
							, [Psz_disposition Anlayse I TN].Bestand
							, Max([Psz_disposition Anlayse I TN].Termin_Materialbedarf) AS MaxvonTermin_Materialbedarf
							, Artikel.[Bezeichnung 1] AS [Bezeichnung des Bauteils]
							, [Psz_disposition Anlayse I TN].bearbeitet
							, [Psz_disposition Anlayse I TN].Mindestbestellmenge
							, [Psz_disposition Anlayse I TN].Rahmen
							, [Psz_disposition Anlayse I TN].[Rahmen-Nr]
							, [Psz_disposition Anlayse I TN].Rahmenmenge
							, [Psz_disposition Anlayse I TN].Rahmenauslauf
							, Lager_Obsolet.Bestand AS Betand_Obsolete
							FROM (
							[Psz_disposition Anlayse I TN] INNER JOIN Artikel ON [Psz_disposition Anlayse I TN].Stücklisten_Artikelnummer = Artikel.Artikelnummer)
							LEFT JOIN Lager_Obsolet ON Artikel.[Artikel-Nr] = Lager_Obsolet.[Artikel-Nr]
							GROUP BY 
							[Psz_disposition Anlayse I TN].Name1
							, [Psz_disposition Anlayse I TN].Stücklisten_Artikelnummer
							, [Psz_disposition Anlayse I TN].Lagerort_id
							, [Psz_disposition Anlayse I TN].Lagerort
							, [Psz_disposition Anlayse I TN].Bestand
							, Artikel.[Bezeichnung 1]
							, [Psz_disposition Anlayse I TN].bearbeitet
							, [Psz_disposition Anlayse I TN].Mindestbestellmenge
							, [Psz_disposition Anlayse I TN].Rahmen
							, [Psz_disposition Anlayse I TN].[Rahmen-Nr]
							, [Psz_disposition Anlayse I TN].Rahmenmenge
							, [Psz_disposition Anlayse I TN].Rahmenauslauf
							, Lager_Obsolet.Bestand
							)
							select top 10 
								daws.Stücklisten_Artikelnummer Artikelnummer
						from [PSz_disposition_Anlayse II TN]  daws
						where Stücklisten_Artikelnummer like '{Filter.SqlEscape()}%'
						order by Stücklisten_Artikelnummer ";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity> GetFaultyArtikelNummersTN30(string Filter)
		{
			var dataTable = new DataTable();



			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
						With [psz_disposition Analyse I TN 30] as (
							SELECT 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Artikelnummer_stücklisten AS Stücklisten_Artikelnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Fertigungsnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Bestand
							, Lagerorte.Lagerort_id
							, Lagerorte.Lagerort
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Bruttobedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Termin_Materialbedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Standardlieferant
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Name1
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].bearbeitet
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Mindestbestellmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Rahmen
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].[Rahmen-Nr]
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Rahmenmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Rahmenauslauf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Termin_Bestätigt1
							FROM 
							Lagerorte INNER JOIN [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30] 
							ON Lagerorte.Lagerort_id = [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Lagerort_id
							GROUP BY [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Artikelnummer_stücklisten
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Fertigungsnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Bestand
							, Lagerorte.Lagerort_id
							, Lagerorte.Lagerort
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Bruttobedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Termin_Materialbedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Standardlieferant
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Name1
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].bearbeitet
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Mindestbestellmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Rahmen
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].[Rahmen-Nr]
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Rahmenmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Rahmenauslauf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Termin_Bestätigt1
							HAVING ((
							([Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Artikelnummer_stücklisten)<>'reparatur') 
							AND (([Psz_disposition_nettobedarfsermittlung Dispo Table_Tn_30].Standardlieferant)=1))
							)
							, Lager_Obsolet as
							(
							SELECT Lager.[Artikel-Nr], Lager.Lagerort_id, Lager.Bestand
							FROM Lager
							WHERE (((Lager.Lagerort_id)=22) AND ((Lager.Bestand)>0))
							)
							,[psz_disposition_Analyse II TN_30] as (
							SELECT 
							[psz_disposition Analyse I TN 30].Name1
							, [psz_disposition Analyse I TN 30].Stücklisten_Artikelnummer
							, Sum([psz_disposition Analyse I TN 30].Bruttobedarf) AS SummevonBruttobedarf
							, [psz_disposition Analyse I TN 30].Lagerort_id
							, [psz_disposition Analyse I TN 30].Lagerort
							, [psz_disposition Analyse I TN 30].Bestand
							, Max([psz_disposition Analyse I TN 30].Termin_Materialbedarf) AS MaxvonTermin_Materialbedarf
							, Artikel.[Bezeichnung 1] AS [Bezeichnung des Bauteils]
							, [psz_disposition Analyse I TN 30].bearbeitet
							, [psz_disposition Analyse I TN 30].Mindestbestellmenge
							, [psz_disposition Analyse I TN 30].Rahmen
							, [psz_disposition Analyse I TN 30].[Rahmen-Nr]
							, [psz_disposition Analyse I TN 30].Rahmenmenge
							, [psz_disposition Analyse I TN 30].Rahmenauslauf
							, Lager_Obsolet.Bestand AS Betand_Obsolete
							FROM (
							[psz_disposition Analyse I TN 30] INNER JOIN Artikel 
							ON [psz_disposition Analyse I TN 30].Stücklisten_Artikelnummer = Artikel.Artikelnummer) LEFT JOIN Lager_Obsolet 
							ON Artikel.[Artikel-Nr] = Lager_Obsolet.[Artikel-Nr]
							GROUP BY [psz_disposition Analyse I TN 30].Name1
							, [psz_disposition Analyse I TN 30].Stücklisten_Artikelnummer
							, [psz_disposition Analyse I TN 30].Lagerort_id
							, [psz_disposition Analyse I TN 30].Lagerort, [psz_disposition Analyse I TN 30].Bestand
							, Artikel.[Bezeichnung 1], [psz_disposition Analyse I TN 30].bearbeitet
							, [psz_disposition Analyse I TN 30].Mindestbestellmenge
							, [psz_disposition Analyse I TN 30].Rahmen
							, [psz_disposition Analyse I TN 30].[Rahmen-Nr]
							, [psz_disposition Analyse I TN 30].Rahmenmenge
							, [psz_disposition Analyse I TN 30].Rahmenauslauf
							, Lager_Obsolet.Bestand )
						select top 10 
								daws.Stücklisten_Artikelnummer Artikelnummer
						from [psz_disposition_Analyse II TN_30]  daws
						where Stücklisten_Artikelnummer like '{Filter.SqlEscape()}%'
						order by Stücklisten_Artikelnummer ";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity> GetFaultyArtikelNummersCZ90(string Filter)
		{
			var dataTable = new DataTable();



			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
						with [Psz_disposition Anlayse I CZ] as (
							SELECT 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Artikelnummer_stücklisten AS Stücklisten_Artikelnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Fertigungsnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Bestand
							, Lagerorte.Lagerort_id
							, Lagerorte.Lagerort
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Bruttobedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Termin_Materialbedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Standardlieferant
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Name1
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].bearbeitet
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Mindestbestellmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Rahmen
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].[Rahmen-Nr]
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Rahmenmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Rahmenauslauf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Termin_Bestätigt1
							FROM 
							Lagerorte INNER JOIN [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ] 
							ON Lagerorte.Lagerort_id = [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Lagerort_id
							GROUP BY 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Artikelnummer_stücklisten
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Fertigungsnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Bestand
							, Lagerorte.Lagerort_id
							, Lagerorte.Lagerort
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Bruttobedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Termin_Materialbedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Standardlieferant
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Name1
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].bearbeitet
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Mindestbestellmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Rahmen
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].[Rahmen-Nr]
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Rahmenmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Rahmenauslauf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Termin_Bestätigt1
							HAVING ((
							([Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Artikelnummer_stücklisten)<>'reparatur') 
							AND (([Psz_disposition_nettobedarfsermittlung Dispo Table_CZ].Standardlieferant)=1))
							)
							, Lager_Obsolet as
							(
							SELECT Lager.[Artikel-Nr], Lager.Lagerort_id, Lager.Bestand
							FROM Lager
							WHERE (((Lager.Lagerort_id)=22) AND ((Lager.Bestand)>0))
							)
							,[PSz_disposition_Anlayse II CZ] as (
							SELECT 
							[Psz_disposition Anlayse I CZ].Name1
							, [Psz_disposition Anlayse I CZ].Stücklisten_Artikelnummer
							, Sum([Psz_disposition Anlayse I CZ].Bruttobedarf) AS SummevonBruttobedarf
							, [Psz_disposition Anlayse I CZ].Lagerort_id
							, [Psz_disposition Anlayse I CZ].Lagerort
							, [Psz_disposition Anlayse I CZ].Bestand
							, Max([Psz_disposition Anlayse I CZ].Termin_Materialbedarf) AS MaxvonTermin_Materialbedarf
							, Artikel.[Bezeichnung 1] AS [Bezeichnung des Bauteils]
							, [Psz_disposition Anlayse I CZ].bearbeitet
							, [Psz_disposition Anlayse I CZ].Mindestbestellmenge
							, [Psz_disposition Anlayse I CZ].Rahmen
							, [Psz_disposition Anlayse I CZ].[Rahmen-Nr]
							, [Psz_disposition Anlayse I CZ].Rahmenmenge
							, [Psz_disposition Anlayse I CZ].Rahmenauslauf
							, Lager_Obsolet.Bestand AS Betand_Obsolete
							FROM 
							([Psz_disposition Anlayse I CZ] INNER JOIN Artikel ON [Psz_disposition Anlayse I CZ].Stücklisten_Artikelnummer = Artikel.Artikelnummer)
							LEFT JOIN Lager_Obsolet ON Artikel.[Artikel-Nr] = Lager_Obsolet.[Artikel-Nr]
							GROUP BY 
							[Psz_disposition Anlayse I CZ].Name1
							, [Psz_disposition Anlayse I CZ].Stücklisten_Artikelnummer
							, [Psz_disposition Anlayse I CZ].Lagerort_id
							, [Psz_disposition Anlayse I CZ].Lagerort
							, [Psz_disposition Anlayse I CZ].Bestand
							, Artikel.[Bezeichnung 1]
							, [Psz_disposition Anlayse I CZ].bearbeitet
							, [Psz_disposition Anlayse I CZ].Mindestbestellmenge
							, [Psz_disposition Anlayse I CZ].Rahmen
							, [Psz_disposition Anlayse I CZ].[Rahmen-Nr]
							, [Psz_disposition Anlayse I CZ].Rahmenmenge
							, [Psz_disposition Anlayse I CZ].Rahmenauslauf
							, Lager_Obsolet.Bestand
							)
						select top 10 
								daws.Stücklisten_Artikelnummer Artikelnummer
						from [PSz_disposition_Anlayse II CZ]  daws
						where Stücklisten_Artikelnummer like '{Filter.SqlEscape()}%'
						order by Stücklisten_Artikelnummer ";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity> GetFaultyArtikelNummersCZ30(string Filter)
		{
			var dataTable = new DataTable();



			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
						with [Psz_disposition Anlayse I CZ_30] as (
							SELECT 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Artikelnummer_stücklisten AS Stücklisten_Artikelnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Fertigungsnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Bestand
							, Lagerorte.Lagerort_id
							, Lagerorte.Lagerort
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Bruttobedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Termin_Materialbedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Standardlieferant
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Name1
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].bearbeitet
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Mindestbestellmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Rahmen
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].[Rahmen-Nr]
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Rahmenmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Rahmenauslauf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Termin_Bestätigt1
							FROM 
							Lagerorte INNER JOIN [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30] 
							ON Lagerorte.Lagerort_id = [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Lagerort_id
							GROUP BY 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Artikelnummer_stücklisten
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Fertigungsnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Bestand
							, Lagerorte.Lagerort_id
							, Lagerorte.Lagerort
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Bruttobedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Termin_Materialbedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Standardlieferant
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Name1
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].bearbeitet
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Mindestbestellmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Rahmen
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].[Rahmen-Nr]
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Rahmenmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Rahmenauslauf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Termin_Bestätigt1
							HAVING ((
							([Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Artikelnummer_stücklisten)<>'reparatur') 
							AND (([Psz_disposition_nettobedarfsermittlung Dispo Table_CZ_30].Standardlieferant)=1))
							)
							, Lager_Obsolet as
							(
							SELECT Lager.[Artikel-Nr], Lager.Lagerort_id, Lager.Bestand
							FROM Lager
							WHERE (((Lager.Lagerort_id)=22) AND ((Lager.Bestand)>0))
							)
							, [PSz_disposition_Anlayse II CZ_30] as (
							SELECT 
							[Psz_disposition Anlayse I CZ_30].Name1
							, [Psz_disposition Anlayse I CZ_30].Stücklisten_Artikelnummer
							, Sum([Psz_disposition Anlayse I CZ_30].Bruttobedarf) AS SummevonBruttobedarf
							, [Psz_disposition Anlayse I CZ_30].Lagerort_id
							, [Psz_disposition Anlayse I CZ_30].Lagerort
							, [Psz_disposition Anlayse I CZ_30].Bestand
							, Max([Psz_disposition Anlayse I CZ_30].Termin_Materialbedarf) AS MaxvonTermin_Materialbedarf
							, Artikel.[Bezeichnung 1] AS [Bezeichnung des Bauteils]
							, [Psz_disposition Anlayse I CZ_30].bearbeitet
							, [Psz_disposition Anlayse I CZ_30].Mindestbestellmenge
							, [Psz_disposition Anlayse I CZ_30].Rahmen
							, [Psz_disposition Anlayse I CZ_30].[Rahmen-Nr]
							, [Psz_disposition Anlayse I CZ_30].Rahmenmenge
							, [Psz_disposition Anlayse I CZ_30].Rahmenauslauf
							, Lager_Obsolet.Bestand AS Betand_Obsolete
							FROM (
							[Psz_disposition Anlayse I CZ_30] INNER JOIN Artikel 
							ON [Psz_disposition Anlayse I CZ_30].Stücklisten_Artikelnummer = Artikel.Artikelnummer)
							LEFT JOIN Lager_Obsolet ON Artikel.[Artikel-Nr] = Lager_Obsolet.[Artikel-Nr]
							GROUP BY 
							[Psz_disposition Anlayse I CZ_30].Name1
							, [Psz_disposition Anlayse I CZ_30].Stücklisten_Artikelnummer
							, [Psz_disposition Anlayse I CZ_30].Lagerort_id
							, [Psz_disposition Anlayse I CZ_30].Lagerort
							, [Psz_disposition Anlayse I CZ_30].Bestand
							, Artikel.[Bezeichnung 1], [Psz_disposition Anlayse I CZ_30].bearbeitet
							, [Psz_disposition Anlayse I CZ_30].Mindestbestellmenge
							, [Psz_disposition Anlayse I CZ_30].Rahmen
							, [Psz_disposition Anlayse I CZ_30].[Rahmen-Nr]
							, [Psz_disposition Anlayse I CZ_30].Rahmenmenge
							, [Psz_disposition Anlayse I CZ_30].Rahmenauslauf
							, Lager_Obsolet.Bestand)
						select top 10 
								daws.Stücklisten_Artikelnummer Artikelnummer
						from [PSz_disposition_Anlayse II CZ_30]  daws
						where Stücklisten_Artikelnummer like '{Filter.SqlEscape()}%'
						order by Stücklisten_Artikelnummer ";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity> GetFaultyArtikelNummersBETN90(string Filter)
		{
			var dataTable = new DataTable();



			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
						with [Psz_disposition Anlayse I BETN] as (
										SELECT 
										[Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Artikelnummer_stücklisten AS Stücklisten_Artikelnummer
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Fertigungsnummer
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Bestand
										, Lagerorte.Lagerort_id
										, Lagerorte.Lagerort
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Bruttobedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Termin_Materialbedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Standardlieferant
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Name1
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].bearbeitet
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Mindestbestellmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Rahmen
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].[Rahmen-Nr]
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Rahmenmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Rahmenauslauf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Termin_Bestätigt1
										FROM Lagerorte INNER JOIN [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN] 
										ON Lagerorte.Lagerort_id = [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Lagerort_id
										GROUP BY 
										[Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Artikelnummer_stücklisten
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Fertigungsnummer
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Bestand
										, Lagerorte.Lagerort_id, Lagerorte.Lagerort
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Bruttobedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Termin_Materialbedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Standardlieferant
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Name1
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].bearbeitet
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Mindestbestellmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Rahmen
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].[Rahmen-Nr]
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Rahmenmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Rahmenauslauf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Termin_Bestätigt1
										HAVING ((
										([Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Artikelnummer_stücklisten)<>'reparatur') 
										AND (([Psz_disposition_nettobedarfsermittlung Dispo Table_BETN].Standardlieferant)=1))
										)
										,Lager_Obsolet as (
										SELECT Lager.[Artikel-Nr], Lager.Lagerort_id, Lager.Bestand
										FROM Lager
										WHERE (((Lager.Lagerort_id)=22) AND ((Lager.Bestand)>0))
										)
										, [PSz_disposition_Anlayse II BETN] as (
										SELECT 
										[Psz_disposition Anlayse I BETN].Name1
										, [Psz_disposition Anlayse I BETN].Stücklisten_Artikelnummer
										, Sum([Psz_disposition Anlayse I BETN].Bruttobedarf) AS SummevonBruttobedarf
										, [Psz_disposition Anlayse I BETN].Lagerort_id
										, [Psz_disposition Anlayse I BETN].Lagerort
										, [Psz_disposition Anlayse I BETN].Bestand
										, Max([Psz_disposition Anlayse I BETN].Termin_Materialbedarf) AS MaxvonTermin_Materialbedarf
										, Artikel.[Bezeichnung 1] AS [Bezeichnung des Bauteils]
										, [Psz_disposition Anlayse I BETN].bearbeitet
										, [Psz_disposition Anlayse I BETN].Mindestbestellmenge
										, Artikel.Rahmen, Artikel.[Rahmen-Nr], Artikel.Rahmenmenge
										, Artikel.Rahmenauslauf, Lager_Obsolet.Bestand AS Bestand_Obsolete
										FROM [Psz_disposition Anlayse I BETN] INNER JOIN (Artikel LEFT JOIN Lager_Obsolet 
										ON Artikel.[Artikel-Nr] = Lager_Obsolet.[Artikel-Nr]) 
										ON [Psz_disposition Anlayse I BETN].Stücklisten_Artikelnummer = Artikel.Artikelnummer
										GROUP BY [Psz_disposition Anlayse I BETN].Name1
										, [Psz_disposition Anlayse I BETN].Stücklisten_Artikelnummer
										, [Psz_disposition Anlayse I BETN].Lagerort_id
										, [Psz_disposition Anlayse I BETN].Lagerort
										, [Psz_disposition Anlayse I BETN].Bestand
										, Artikel.[Bezeichnung 1], [Psz_disposition Anlayse I BETN].bearbeitet
										, [Psz_disposition Anlayse I BETN].Mindestbestellmenge
										, Artikel.Rahmen, Artikel.[Rahmen-Nr], Artikel.Rahmenmenge
										, Artikel.Rahmenauslauf, Lager_Obsolet.Bestand)
									select top 10 
								daws.Stücklisten_Artikelnummer Artikelnummer
						from [PSz_disposition_Anlayse II BETN]  daws
						where Stücklisten_Artikelnummer like '{Filter.SqlEscape()}%'
						order by Stücklisten_Artikelnummer";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity> GetFaultyArtikelNummersBETN40(string Filter)
		{
			var dataTable = new DataTable();



			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
						with [Psz_disposition Anlayse I BETN_40] as (
										SELECT 
										[Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Artikelnummer_stücklisten AS Stücklisten_Artikelnummer
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Fertigungsnummer
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Bestand
										, Lagerorte.Lagerort_id
										, Lagerorte.Lagerort
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Bruttobedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Termin_Materialbedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Standardlieferant
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Name1
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].bearbeitet
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Mindestbestellmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Rahmen
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].[Rahmen-Nr]
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Rahmenmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Rahmenauslauf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Termin_Bestätigt1
										FROM Lagerorte INNER JOIN [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40] 
										ON Lagerorte.Lagerort_id = [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Lagerort_id
										GROUP BY [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Artikelnummer_stücklisten
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Fertigungsnummer
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Bestand
										, Lagerorte.Lagerort_id, Lagerorte.Lagerort
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Bruttobedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Termin_Materialbedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Standardlieferant
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Name1
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].bearbeitet
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Mindestbestellmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Rahmen
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].[Rahmen-Nr]
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Rahmenmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Rahmenauslauf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Termin_Bestätigt1
										HAVING ((([Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Artikelnummer_stücklisten)<>'reparatur') 
										AND (([Psz_disposition_nettobedarfsermittlung Dispo Table_BETN_40].Standardlieferant)=1))
										)
										,Lager_Obsolet as (
										SELECT Lager.[Artikel-Nr], Lager.Lagerort_id, Lager.Bestand
										FROM Lager
										WHERE (((Lager.Lagerort_id)=22) AND ((Lager.Bestand)>0))
										)
										,[PSz_disposition_Anlayse II BETN_40] as ( SELECT 
										[Psz_disposition Anlayse I BETN_40].Name1
										, [Psz_disposition Anlayse I BETN_40].Stücklisten_Artikelnummer
										, Sum([Psz_disposition Anlayse I BETN_40].Bruttobedarf) AS SummevonBruttobedarf
										, [Psz_disposition Anlayse I BETN_40].Lagerort_id, [Psz_disposition Anlayse I BETN_40].Lagerort
										, [Psz_disposition Anlayse I BETN_40].Bestand
										, Max([Psz_disposition Anlayse I BETN_40].Termin_Materialbedarf) AS MaxvonTermin_Materialbedarf
										, Artikel.[Bezeichnung 1] AS [Bezeichnung des Bauteils]
										, [Psz_disposition Anlayse I BETN_40].bearbeitet
										, [Psz_disposition Anlayse I BETN_40].Mindestbestellmenge
										, Artikel.Rahmen, Artikel.[Rahmen-Nr]
										, Artikel.Rahmenmenge
										, Artikel.Rahmenauslauf, Lager_Obsolet.Bestand AS Bestand_Obsolete
										FROM [Psz_disposition Anlayse I BETN_40] INNER JOIN (Artikel LEFT JOIN Lager_Obsolet 
										ON Artikel.[Artikel-Nr] = Lager_Obsolet.[Artikel-Nr])
										ON [Psz_disposition Anlayse I BETN_40].Stücklisten_Artikelnummer = Artikel.Artikelnummer
										GROUP BY 
										[Psz_disposition Anlayse I BETN_40].Name1
										, [Psz_disposition Anlayse I BETN_40].Stücklisten_Artikelnummer
										, [Psz_disposition Anlayse I BETN_40].Lagerort_id
										, [Psz_disposition Anlayse I BETN_40].Lagerort
										, [Psz_disposition Anlayse I BETN_40].Bestand, Artikel.[Bezeichnung 1]
										, [Psz_disposition Anlayse I BETN_40].bearbeitet
										, [Psz_disposition Anlayse I BETN_40].Mindestbestellmenge
										, Artikel.Rahmen, Artikel.[Rahmen-Nr], Artikel.Rahmenmenge
										, Artikel.Rahmenauslauf, Lager_Obsolet.Bestand
										)
									select top 10 
								daws.Stücklisten_Artikelnummer Artikelnummer
						from [PSz_disposition_Anlayse II BETN_40]  daws
						where Stücklisten_Artikelnummer like '{Filter.SqlEscape()}%'
						order by Stücklisten_Artikelnummer";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity> GetFaultyArtikelNummersAL90(string Filter)
		{
			var dataTable = new DataTable();



			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
						with [PSz_disposition Analyse I AL] as (
										SELECT 
										[Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Artikelnummer_stücklisten AS Stücklisten_Artikelnummer
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Fertigungsnummer
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Bestand
										, Lagerorte.Lagerort_id
										, Lagerorte.Lagerort
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Bruttobedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Termin_Materialbedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Standardlieferant
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Name1
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].bearbeitet
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Mindestbestellmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Rahmen
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].[Rahmen-Nr]
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Rahmenmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Rahmenauslauf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Termin_Bestätigt1
										FROM Lagerorte INNER JOIN [Psz_disposition_nettobedarfsermittlung Dispo Table_AL] 
										ON Lagerorte.Lagerort_id = [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Lagerort_id
										GROUP BY [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Artikelnummer_stücklisten
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Fertigungsnummer
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Bestand
										, Lagerorte.Lagerort_id
										, Lagerorte.Lagerort
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Bruttobedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Termin_Materialbedarf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Standardlieferant
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Name1
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].bearbeitet
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Mindestbestellmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Rahmen
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].[Rahmen-Nr]
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Rahmenmenge
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Rahmenauslauf
										, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Termin_Bestätigt1
										HAVING ((([Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Artikelnummer_stücklisten)<>'reparatur') 
										AND (([Psz_disposition_nettobedarfsermittlung Dispo Table_AL].Standardlieferant)=1))
										)
										,Lager_Obsolet as (
										SELECT Lager.[Artikel-Nr], Lager.Lagerort_id, Lager.Bestand
										FROM Lager
										WHERE (((Lager.Lagerort_id)=22) AND ((Lager.Bestand)>0))
										)
										,[psz_disposition_Analyse II AL] as (
										SELECT [PSz_disposition Analyse I AL].Name1
										, [PSz_disposition Analyse I AL].Stücklisten_Artikelnummer
										, Sum([PSz_disposition Analyse I AL].Bruttobedarf) AS SummevonBruttobedarf
										, [PSz_disposition Analyse I AL].Lagerort_id
										, [PSz_disposition Analyse I AL].Lagerort
										, [PSz_disposition Analyse I AL].Bestand
										, Max([PSz_disposition Analyse I AL].Termin_Materialbedarf) AS MaxvonTermin_Materialbedarf
										, Artikel.[Bezeichnung 1] AS [Bezeichnung des Bauteils]
										, [PSz_disposition Analyse I AL].bearbeitet
										, [PSz_disposition Analyse I AL].Mindestbestellmenge
										, [PSz_disposition Analyse I AL].Rahmen
										, [PSz_disposition Analyse I AL].[Rahmen-Nr]
										, [PSz_disposition Analyse I AL].Rahmenmenge
										, [PSz_disposition Analyse I AL].Rahmenauslauf
										, Lager_Obsolet.Bestand AS Bestand_Obsolete
										FROM ([PSz_disposition Analyse I AL] INNER JOIN Artikel 
										ON [PSz_disposition Analyse I AL].Stücklisten_Artikelnummer = Artikel.Artikelnummer) 
										LEFT JOIN Lager_Obsolet ON Artikel.[Artikel-Nr] = Lager_Obsolet.[Artikel-Nr]
										GROUP BY 
										[PSz_disposition Analyse I AL].Name1
										, [PSz_disposition Analyse I AL].Stücklisten_Artikelnummer
										, [PSz_disposition Analyse I AL].Lagerort_id
										, [PSz_disposition Analyse I AL].Lagerort
										, [PSz_disposition Analyse I AL].Bestand
										, Artikel.[Bezeichnung 1]
										, [PSz_disposition Analyse I AL].bearbeitet
										, [PSz_disposition Analyse I AL].Mindestbestellmenge
										, [PSz_disposition Analyse I AL].Rahmen
										, [PSz_disposition Analyse I AL].[Rahmen-Nr]
										, [PSz_disposition Analyse I AL].Rahmenmenge
										, [PSz_disposition Analyse I AL].Rahmenauslauf
										, Lager_Obsolet.Bestand
										)

									select top 10 
								daws.Stücklisten_Artikelnummer Artikelnummer
						from [psz_disposition_Analyse II AL]  daws
						where Stücklisten_Artikelnummer like '{Filter.SqlEscape()}%'
						order by Stücklisten_Artikelnummer";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity> GetFaultyArtikelNummersAL30(string Filter)
		{
			var dataTable = new DataTable();



			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
						with [PSz_disposition Analyse I AL_30] as (
											SELECT 
											[Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Artikelnummer_stücklisten AS Stücklisten_Artikelnummer
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Fertigungsnummer
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Bestand
											, Lagerorte.Lagerort_id, Lagerorte.Lagerort
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Bruttobedarf
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Termin_Materialbedarf
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Standardlieferant
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Name1
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].bearbeitet
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Mindestbestellmenge
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Rahmen
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].[Rahmen-Nr]
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Rahmenmenge
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Rahmenauslauf
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Termin_Bestätigt1
											FROM Lagerorte INNER JOIN [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30] 
											ON Lagerorte.Lagerort_id = [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Lagerort_id
											GROUP BY [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Artikelnummer_stücklisten
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Fertigungsnummer
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Bestand
											, Lagerorte.Lagerort_id, Lagerorte.Lagerort, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Bruttobedarf
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Termin_Materialbedarf
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Standardlieferant
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Name1
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].bearbeitet
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Mindestbestellmenge
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Rahmen
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].[Rahmen-Nr]
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Rahmenmenge
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Rahmenauslauf
											, [Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Termin_Bestätigt1
											HAVING ((([Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Artikelnummer_stücklisten)<>'reparatur') 
											AND (([Psz_disposition_nettobedarfsermittlung Dispo Table_AL_30].Standardlieferant)=1))
											)
											,Lager_Obsolet as (
											SELECT Lager.[Artikel-Nr], Lager.Lagerort_id, Lager.Bestand
											FROM Lager
											WHERE (((Lager.Lagerort_id)=22) AND ((Lager.Bestand)>0))
											)
											, [psz_disposition_Analyse II AL_30] as (
											SELECT 
											[PSz_disposition Analyse I AL_30].Name1
											, [PSz_disposition Analyse I AL_30].Stücklisten_Artikelnummer
											, Sum([PSz_disposition Analyse I AL_30].Bruttobedarf) AS SummevonBruttobedarf
											, [PSz_disposition Analyse I AL_30].Lagerort_id
											, [PSz_disposition Analyse I AL_30].Lagerort
											, [PSz_disposition Analyse I AL_30].Bestand
											, Max([PSz_disposition Analyse I AL_30].Termin_Materialbedarf) AS MaxvonTermin_Materialbedarf
											, Artikel.[Bezeichnung 1] AS [Bezeichnung des Bauteils]
											, [PSz_disposition Analyse I AL_30].bearbeitet
											, [PSz_disposition Analyse I AL_30].Mindestbestellmenge
											, [PSz_disposition Analyse I AL_30].Rahmen
											, [PSz_disposition Analyse I AL_30].[Rahmen-Nr]
											, [PSz_disposition Analyse I AL_30].Rahmenmenge
											, [PSz_disposition Analyse I AL_30].Rahmenauslauf
											, Lager_Obsolet.Bestand AS Bestand_Obsolete
											FROM ([PSz_disposition Analyse I AL_30] INNER JOIN Artikel 
											ON [PSz_disposition Analyse I AL_30].Stücklisten_Artikelnummer = Artikel.Artikelnummer) LEFT JOIN Lager_Obsolet 
											ON Artikel.[Artikel-Nr] = Lager_Obsolet.[Artikel-Nr]
											GROUP BY [PSz_disposition Analyse I AL_30].Name1
											, [PSz_disposition Analyse I AL_30].Stücklisten_Artikelnummer
											, [PSz_disposition Analyse I AL_30].Lagerort_id, [PSz_disposition Analyse I AL_30].Lagerort
											, [PSz_disposition Analyse I AL_30].Bestand, Artikel.[Bezeichnung 1], [PSz_disposition Analyse I AL_30].bearbeitet
											, [PSz_disposition Analyse I AL_30].Mindestbestellmenge, [PSz_disposition Analyse I AL_30].Rahmen
											, [PSz_disposition Analyse I AL_30].[Rahmen-Nr], [PSz_disposition Analyse I AL_30].Rahmenmenge
											, [PSz_disposition Analyse I AL_30].Rahmenauslauf, Lager_Obsolet.Bestand
											)
										select top 10 
								daws.Stücklisten_Artikelnummer Artikelnummer
						from [psz_disposition_Analyse II AL_30]  daws
						where Stücklisten_Artikelnummer like '{Filter.SqlEscape()}%'
						order by Stücklisten_Artikelnummer";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity> GetFaultyArtikelNummersDE90(string Filter)
		{
			var dataTable = new DataTable();



			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"
						with [psz_disposition Analyse I De] as (
							SELECT 
							[Psz_disposition_nettobedarfsermittlung Dispo Table_De].Artikelnummer_stücklisten AS Stücklisten_Artikelnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Fertigungsnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Bestand
							, Lagerorte.Lagerort_id
							, Lagerorte.Lagerort
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Bruttobedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Termin_Materialbedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Standardlieferant
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Name1
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].bearbeitet
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Mindestbestellmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Rahmen
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].[Rahmen-Nr]
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Rahmenmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Rahmenauslauf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Termin_Bestätigt1
							FROM Lagerorte INNER JOIN [Psz_disposition_nettobedarfsermittlung Dispo Table_De]
							ON Lagerorte.Lagerort_id = [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Lagerort_id
							GROUP BY [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Artikelnummer_stücklisten
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Fertigungsnummer
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Bestand
							, Lagerorte.Lagerort_id
							, Lagerorte.Lagerort
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Bruttobedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Termin_Materialbedarf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Standardlieferant
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Name1
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].bearbeitet
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Mindestbestellmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Rahmen
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].[Rahmen-Nr]
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Rahmenmenge
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Rahmenauslauf
							, [Psz_disposition_nettobedarfsermittlung Dispo Table_De].Termin_Bestätigt1
							HAVING ((([Psz_disposition_nettobedarfsermittlung Dispo Table_De].Artikelnummer_stücklisten)<>'reparatur') 
							AND (([Psz_disposition_nettobedarfsermittlung Dispo Table_De].Standardlieferant)=1))
							)
							,Lager_Obsolet as (
							SELECT Lager.[Artikel-Nr], Lager.Lagerort_id, Lager.Bestand
							FROM Lager
							WHERE (((Lager.Lagerort_id)=22) AND ((Lager.Bestand)>0))
							)
							,[psz_disposition_analyse II De] as (
							SELECT [psz_disposition Analyse I De].Name1
							, [psz_disposition Analyse I De].Stücklisten_Artikelnummer
							, Sum([psz_disposition Analyse I De].Bruttobedarf) AS SummevonBruttobedarf
							, [psz_disposition Analyse I De].Lagerort_id
							, [psz_disposition Analyse I De].Lagerort
							, [psz_disposition Analyse I De].Bestand			
							, Max([psz_disposition Analyse I De].Termin_Materialbedarf) AS MaxvonTermin_Materialbedarf
							, Artikel.[Bezeichnung 1] AS [Bezeichnung des Bauteils]
							, [psz_disposition Analyse I De].bearbeitet
							, [psz_disposition Analyse I De].Mindestbestellmenge
							, [psz_disposition Analyse I De].Rahmen
							,Artikel.[Artikel-Nr]
							, [psz_disposition Analyse I De].[Rahmen-Nr]
							, [psz_disposition Analyse I De].Rahmenmenge
							, [psz_disposition Analyse I De].Rahmenauslauf
							, Lager_Obsolet.Bestand AS Betand_Obsolete

							FROM [psz_disposition Analyse I De] 
							INNER JOIN Artikel ON [psz_disposition Analyse I De].Stücklisten_Artikelnummer = Artikel.Artikelnummer 
							LEFT JOIN Lager_Obsolet ON Artikel.[Artikel-Nr] = Lager_Obsolet.[Artikel-Nr]
							GROUP BY 
							[psz_disposition Analyse I De].Name1
							, [psz_disposition Analyse I De].Stücklisten_Artikelnummer
							,Artikel.[Artikel-Nr]
							, [psz_disposition Analyse I De].Lagerort_id
							, [psz_disposition Analyse I De].Lagerort
							, [psz_disposition Analyse I De].Bestand
							, Artikel.[Bezeichnung 1]
							, [psz_disposition Analyse I De].bearbeitet									   
							, [psz_disposition Analyse I De].Mindestbestellmenge
							, [psz_disposition Analyse I De].Rahmen
							, [psz_disposition Analyse I De].[Rahmen-Nr]
							, [psz_disposition Analyse I De].Rahmenmenge
							, [psz_disposition Analyse I De].Rahmenauslauf
							, Lager_Obsolet.Bestand
							)

			  
							select top 10 daws.Stücklisten_Artikelnummer Artikelnummer
							from [psz_disposition_analyse II De] daws
						where Stücklisten_Artikelnummer like '{Filter.SqlEscape()}%'
						order by Stücklisten_Artikelnummer";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.GetFaultyArtikelNummerEntity>();
			}
		}
		public static List<Entities.Joins.MTM.Order.Statistics.DispowsDetailsCurrencyEntity> GetCurrencySymbol(int Nr = 18)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query =
						$@"	select w.Nr, w.Symbol Symbol from Währungen w where w.Nr = {Nr}";

				var sqlCommand = new SqlCommand(query, sqlConnection);


				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.DispowsDetailsCurrencyEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.DispowsDetailsCurrencyEntity>();
			}
		}
	}
}
