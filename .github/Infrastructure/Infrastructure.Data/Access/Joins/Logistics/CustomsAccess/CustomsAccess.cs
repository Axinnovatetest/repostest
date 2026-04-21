using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Joins.Logistics.CustomsAccess
{
	public class CustomsAccess
	{

		public static List<Infrastructure.Data.Entities.Joins.Logistics.CustomsEntity.AusfuhrEntity> GetAusfuhr(DateTime from, DateTime to, Settings.PaginModel paging = null, Settings.SortingModel sortingModel = null, int all = 0)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
						$@"
						with [PSZ_BH Statistik Ausfuhr]
							as (
							SELECT 
							Angebote.[Vorname/NameFirma]
							, Artikel.Artikelnummer
							, Artikel.[Bezeichnung 1]
							, Artikel.Zolltarif_nr
							, [angebotene Artikel].Gesamtpreis
							, [angebotene Artikel].Anzahl*Artikel.Größe/1000 AS [Masse in kg]
							, Artikel.Ursprungsland
							FROM 
							(Angebote INNER JOIN (Artikel 
							INNER JOIN [angebotene Artikel] 
							ON Artikel.[Artikel-Nr] = [angebotene Artikel].[Artikel-Nr]) 
							ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) 
							INNER JOIN Kunden 
							ON Angebote.[Kunden-Nr] = Kunden.nummer
							WHERE 
							Angebote.Datum>='{from.ToString("yyyyMMdd")}' --[Von] 
							And Angebote.Datum<='{to.ToString("yyyyMMdd")}'--[Bis]
							AND (Angebote.Typ='Rechnung') 
							AND (Kunden.fibu_rahmen=2)
							--ORDER BY Angebote.[Vorname/NameFirma], Angebote.Datum
							)
							SELECT 
							count(*) over() TotalCount,
							[PSZ_BH Statistik Ausfuhr].[Vorname/NameFirma] as Vorname
							, [PSZ_BH Statistik Ausfuhr].Zolltarif_nr
							, Sum([PSZ_BH Statistik Ausfuhr].Gesamtpreis) AS [VK-Nettosumme]
							, Sum([PSZ_BH Statistik Ausfuhr].[Masse in kg]) AS [Gewicht in kg]
							, [PSZ_BH Statistik Ausfuhr].Ursprungsland
							FROM [PSZ_BH Statistik Ausfuhr]
							GROUP BY 
							[PSZ_BH Statistik Ausfuhr].[Vorname/NameFirma]
							, [PSZ_BH Statistik Ausfuhr].Zolltarif_nr
							, [PSZ_BH Statistik Ausfuhr].Ursprungsland
							HAVING ((([PSZ_BH Statistik Ausfuhr].Zolltarif_nr) Is Not Null))
						";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.CustomsEntity.AusfuhrEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.CustomsEntity.AusfuhrEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.Logistics.CustomsEntity.StammdatenkontrolleWareneingangeEntity> GetStammdatenkontrolleWareneingange(DateTime from, DateTime to, string gruppe, Settings.PaginModel paging = null, Settings.SortingModel sortingModel = null, int all = 0)
		{
			var dataTable = new DataTable();
			string paginationFilter = null;
			string sortingField = "Lieferantengruppe";
			string sortingdesc = ((sortingModel is not null) && sortingModel.SortDesc) ? " Desc " : " asc ";

			if(sortingModel is not null)
			{
				if(!string.IsNullOrWhiteSpace(sortingModel.SortFieldName))
				{
					sortingField = sortingModel.SortFieldName;
				}
			}

			if(paging is not null && all == 1)
			{
				paginationFilter = "";

			}
			else if(paging is not null)
			{
				paginationFilter = $"OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY";
			}

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
						$@"
						SELECT 
							count(*) over() TotalCount
							,Lieferanten.Lieferantengruppe
							, Bestellungen.Datum
							, Artikel.Artikelnummer
							, [bestellte Artikel].Anzahl
							, Artikel.Warengruppe
							, Artikel.Größe AS [Gewicht in gr]
							, Artikel.Zolltarif_nr
							, Artikel.Ursprungsland
							, adressen.Name1
							, [bestellte Artikel].Gesamtpreis
							FROM (Bestellungen INNER JOIN (Lieferanten INNER JOIN adressen 
							ON Lieferanten.nummer = adressen.Nr) 
							ON Bestellungen.[Lieferanten-Nr] = Lieferanten.nummer)
							INNER JOIN (Artikel INNER JOIN [bestellte Artikel] 
							ON Artikel.[Artikel-Nr] = [bestellte Artikel].[Artikel-Nr]) 
							ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
							WHERE 

							TRIM(Lieferanten.Lieferantengruppe)=TRIM('{gruppe}') /*[Gruppe]*/ 
							AND Bestellungen.Datum>='{from.ToString("yyyyMMdd")}' /*[Ab Datum]*/ 
							AND Bestellungen.Datum<='{to.ToString("yyyyMMdd")}' /*[Bis]*/ 
							AND Bestellungen.Typ='Wareneingang'
						ORDER BY {sortingField}  {sortingdesc}
                      {paginationFilter}";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.CustomsEntity.StammdatenkontrolleWareneingangeEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.CustomsEntity.StammdatenkontrolleWareneingangeEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.Logistics.CustomsEntity.StammdatenkontrolleWareneingangeCountEntity> GetStammdatenkontrolleWareneingangeCount(DateTime from, DateTime to, string gruppe = "Inland")
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
						$@"
						select  count(*) over() TotalCount from (		SELECT 
							distinct 
							Lieferanten.Lieferantengruppe
							, Bestellungen.Datum
							, Artikel.Artikelnummer
							, [bestellte Artikel].Anzahl
							, Artikel.Warengruppe
							, Artikel.Größe AS [Gewicht in gr]
							, Artikel.Zolltarif_nr
							, Artikel.Ursprungsland
							, adressen.Name1
							, [bestellte Artikel].Gesamtpreis
							FROM [Statistiken Angebote]
							, (Bestellungen INNER JOIN (Lieferanten INNER JOIN adressen 
							ON Lieferanten.nummer = adressen.Nr) 
							ON Bestellungen.[Lieferanten-Nr] = Lieferanten.nummer)
							INNER JOIN (Artikel INNER JOIN [bestellte Artikel] 
							ON Artikel.[Artikel-Nr] = [bestellte Artikel].[Artikel-Nr]) 
							ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
							WHERE 

							Lieferanten.Lieferantengruppe='{gruppe}'--[Gruppe] 
							AND Bestellungen.Datum>='{from.ToString("yyyyMMdd")}'--[Ab Datum] 
							And Bestellungen.Datum<='{to.ToString("yyyyMMdd")}'--[Bis] ) 
							AND
							Bestellungen.Typ='Wareneingang' 
							) xx
						ORDER BY TotalCount    asc 
                      OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.CustomsEntity.StammdatenkontrolleWareneingangeCountEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.CustomsEntity.StammdatenkontrolleWareneingangeCountEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.Logistics.CustomsEntity.RMDCZEntity> GetRMDCZ(DateTime from, DateTime to, Settings.PaginModel paging = null, Settings.SortingModel sortingModel = null, int all = 0)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
						$@"
						SELECT 
							count(*) over() TotalCount,
							Artikel.Ursprungsland
							, Artikel.Zolltarif_nr
							, Sum(Artikel.Größe*Lagerbewegungen_Artikel.Anzahl) AS Gewichte
							, Sum(Bestellnummern.Einkaufspreis*Lagerbewegungen_Artikel.Anzahl) AS Warenwert
							FROM 
							(Lagerbewegungen INNER JOIN (Lagerbewegungen_Artikel 
							INNER JOIN Artikel 
							ON Lagerbewegungen_Artikel.[Artikel-nr] = Artikel.[Artikel-Nr]) 
							ON Lagerbewegungen.ID = Lagerbewegungen_Artikel.Lagerbewegungen_id) 
							INNER JOIN Bestellnummern 
							ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]
							WHERE 
							Lagerbewegungen.Datum>='{from.ToString("yyyyMMdd")}'--[Von] 
							And Lagerbewegungen.Datum<='{to.ToString("yyyyMMdd")}'--[Bis]
							AND Bestellnummern.Standardlieferant=1
							GROUP BY 
							Artikel.Ursprungsland
							, Artikel.Zolltarif_nr
							, Lagerbewegungen.Typ
							, Lagerbewegungen_Artikel.Lager_nach
							HAVING 
							(((Lagerbewegungen.Typ)='Umbuchung') 
							AND ((Lagerbewegungen_Artikel.Lager_nach)=6))
						";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.CustomsEntity.RMDCZEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.CustomsEntity.RMDCZEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.Logistics.CustomsEntity.EinfuhrEntity> GetEinfuhr(DateTime from, DateTime to, string gruppe, Settings.PaginModel paging = null, Settings.SortingModel sortingModel = null, int all = 0)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
						$@"
						with [CUPreis_LeereBestellung] as (
							SELECT TOP 1 Aktueller_Kupfer_Preis_in_Gramm, Datum
							FROM tbl_Kupfer )
							, [PSZ_BH Statistik Wareneingang 01] as (
							SELECT 
							Lieferanten.Lieferantengruppe
							, Bestellungen.Datum
							, [bestellte Artikel].[Bezeichnung 1]
							, [bestellte Artikel].Anzahl
							, adressen.Name1
							, [bestellte Artikel].Gesamtpreis
							, Artikel.Größe AS [Gewicht in gr]
							, Artikel.Zolltarif_nr, Artikel.Ursprungsland
							, Artikel.Artikelnummer
							, IIf([CUPreis] Is Null,([Aktueller_Kupfer_Preis_in_Gramm]*[Kupferzahl]/1000)*[Anzahl],[Bestellte Artikel].CUPreis) AS CUPreisBerechnung, CUPreis_LeereBestellung.Aktueller_Kupfer_Preis_in_Gramm
							FROM CUPreis_LeereBestellung, (((Bestellungen 
							INNER JOIN [bestellte Artikel] 
							ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]) 
							INNER JOIN adressen 
							ON Bestellungen.[Lieferanten-Nr] = adressen.Nr) 
							INNER JOIN Lieferanten 
							ON adressen.Nr = Lieferanten.nummer) 
							INNER JOIN Artikel 
							ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]
							WHERE 
							(((Lieferanten.Lieferantengruppe)='{gruppe}'--[Gruppe]
							) 
							AND ((Bestellungen.Datum)>='{from.ToString("yyyyMMdd")}'--[Ab Datum] 
							And (Bestellungen.Datum)<='{to.ToString("yyyyMMdd")}'--[Bis]
							) 
							AND ((Bestellungen.Typ)='Wareneingang')))

							SELECT 
							count(*) over() TotalCount,
							[PSZ_BH Statistik Wareneingang 01].Lieferantengruppe
							, [PSZ_BH Statistik Wareneingang 01].Name1
							, [PSZ_BH Statistik Wareneingang 01].Zolltarif_nr
							, [PSZ_BH Statistik Wareneingang 01].Ursprungsland
							, Sum([Gesamtpreis]+[CUPreisBerechnung]) AS Nettopreis
							, Sum([Anzahl]*[Gewicht in gr]/1000) AS Gewicht_in_kg
							FROM [PSZ_BH Statistik Wareneingang 01]
							GROUP BY 
							[PSZ_BH Statistik Wareneingang 01].Lieferantengruppe
							, [PSZ_BH Statistik Wareneingang 01].Name1
							, [PSZ_BH Statistik Wareneingang 01].Zolltarif_nr
							, [PSZ_BH Statistik Wareneingang 01].Ursprungsland
						";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.CustomsEntity.EinfuhrEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.CustomsEntity.EinfuhrEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.Logistics.CustomsEntity.LieferantenGruppeEntity> GetSupplierGroups()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"select Lieferanten.Lieferantengruppe from Lieferanten  group by Lieferanten.Lieferantengruppe";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.Logistics.CustomsEntity.LieferantenGruppeEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.Logistics.CustomsEntity.LieferantenGruppeEntity>();
			}
		}

	}
}
