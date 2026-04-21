using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Joins.FNC.Accounting
{
	public class AccountingAccess
	{
		public static List<Entities.Joins.FNC.Accounting.LiquiditatsplanungSkontozahlerEntity> GetLiquiditatsplanungSkontozahler(Settings.PaginModel paging = null, Settings.SortingModel sortingModel = null, int all = 0)
		{
			var dataTable = new DataTable();
			string paginationFilter = "";
			string sortingField = "Name1";
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
					drop table if exists [PSZ_BH_Liquidationsplanung Skontozahler]

						SELECT 
							Angebote.Typ
							, Angebote.[Angebot-Nr]
							, IIf([angebotene Artikel].Liefertermin>GETDATE(),[angebotene Artikel].Liefertermin,GETDATE()+3) AS Ausliefertermin
							, [angebotene Artikel].Gesamtpreis
							, ([Gesamtpreis]*1.19)*0.98 AS Brutto
							, Angebote.Konditionen
							,  IIf([angebotene Artikel].Liefertermin>GetDate() /*Date()*/,[angebotene Artikel].Liefertermin,GetDate()+3)+2+TRY_PARSE((Left([Konditionen],2)) AS INT) AS Zahlungseingang
							, adressen.Name1
							INTO [PSZ_BH_Liquidationsplanung Skontozahler]
							FROM (Angebote INNER JOIN [angebotene Artikel] 
							ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) 
							INNER JOIN adressen 
							ON Angebote.[Kunden-Nr] = adressen.Nr
							WHERE (((Angebote.Typ)='Auftragsbestätigung') 
							AND ((Angebote.Konditionen)='14 Tage 2%, 30 Tage netto') 
							AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) 
							AND (([angebotene Artikel].erledigt_pos)=0))
							ORDER BY IIf([angebotene Artikel].Liefertermin>GETDATE(),[angebotene Artikel].Liefertermin,GETDATE()+3);

						----> Query 2

						INSERT INTO [PSZ_BH_Liquidationsplanung Skontozahler] 
						( Typ, [Angebot-Nr], Ausliefertermin, Gesamtpreis, Brutto, Konditionen, Zahlungseingang, Name1 )
						SELECT Angebote.Typ
						, Angebote.[Angebot-Nr]
						, IIf([angebotene Artikel].Liefertermin>GetDate() /*Date()*/,[angebotene Artikel].Liefertermin,GetDate() +3) AS Ausliefertermin
						, [angebotene Artikel].Gesamtpreis
						, ([Gesamtpreis]*1.19)*0.98 AS Brutto
						, Angebote.Konditionen
						,  IIf([angebotene Artikel].Liefertermin>GetDate() /*Date()*/,[angebotene Artikel].Liefertermin,GetDate() +3)+2+TRY_PARSE((Left([Konditionen],2)) AS INT) AS Zahlungseingang
						, adressen.Name1
						FROM (Angebote INNER JOIN [angebotene Artikel] 
						ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) 
						INNER JOIN adressen 
						ON Angebote.[Kunden-Nr] = adressen.Nr
						WHERE (((Angebote.Typ)='Auftragsbestätigung') 
						AND ((Angebote.Konditionen)='10 Tage 2%, 30 Tage netto') 
						AND ((Angebote.gebucht)=1) 
						AND ((Angebote.erledigt)=0) 
						AND (([angebotene Artikel].erledigt_pos)=0))
						ORDER BY IIf([angebotene Artikel].Liefertermin>GetDate() /*Date()*/,[angebotene Artikel].Liefertermin,GetDate() /*Date()*/+3);

						----> Query 3  MRGL LI7AD EEEN

						INSERT INTO [PSZ_BH_Liquidationsplanung Skontozahler] ( Typ, [Angebot-Nr], Ausliefertermin
						, Gesamtpreis, Brutto, Konditionen, Zahlungseingang, Name1 )
						SELECT 
						Angebote.Typ
						, Angebote.[Angebot-Nr]
						, IIf([angebotene Artikel].Liefertermin>GetDate() /*Date()*/
						,[angebotene Artikel].Liefertermin,GetDate() /*Date()*/+3) AS Ausliefertermin
						, [angebotene Artikel].Gesamtpreis
						, ([Gesamtpreis]*1.19)*0.97 AS Brutto
						, Angebote.Konditionen
						,  IIf([angebotene Artikel].Liefertermin>GetDate() /*Date()*/,[angebotene Artikel].Liefertermin,GetDate() /*Date()*/+3)+2+TRY_PARSE((Left([Konditionen],2)) AS INT) AS Zahlungseingang
						, adressen.Name1
						FROM (Angebote INNER JOIN [angebotene Artikel] 
						ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) 
						INNER JOIN adressen 
						ON Angebote.[Kunden-Nr] = adressen.Nr
						WHERE (((Angebote.Typ)='Auftragsbestätigung') 
						AND ((Angebote.Konditionen)='14 Tage 3%, 30 Tage netto') 
						AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) 
						AND (([angebotene Artikel].erledigt_pos)=0))
						ORDER BY IIf([angebotene Artikel].Liefertermin>GETDATE(),[angebotene Artikel].Liefertermin,GETDATE()+3);

						----> Query 4

						INSERT INTO [PSZ_BH_Liquidationsplanung Skontozahler] ( Typ, [Angebot-Nr], Ausliefertermin
						, Gesamtpreis, Brutto, Konditionen, Zahlungseingang, Name1 )
						SELECT Angebote.Typ
						, Angebote.[Angebot-Nr]
						, IIf([angebotene Artikel].Liefertermin>GetDate() /*Date()*/
						,[angebotene Artikel].Liefertermin
						,GetDate() /*Date()*/+3) AS Ausliefertermin
						,[angebotene Artikel].Gesamtpreis
						, ([Gesamtpreis]*1.19)*0.98 AS Brutto
						, Angebote.Konditionen
						,  IIf([angebotene Artikel].Liefertermin>GetDate() /*Date()*/,[angebotene Artikel].Liefertermin,GetDate() /*Date()*/+3)+2+TRY_PARSE((Left([Konditionen],2)) AS INT) AS Zahlungseingang
						, adressen.Name1
						FROM (Angebote INNER JOIN [angebotene Artikel] ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) 
						INNER JOIN adressen 
						ON Angebote.[Kunden-Nr] = adressen.Nr
						WHERE (((Angebote.Typ)='Auftragsbestätigung')
						AND ((Angebote.Konditionen)='8 Tage 2 %, 30 Tage netto') 
						AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) 
						AND (([angebotene Artikel].erledigt_pos)=0))
						ORDER BY IIf([angebotene Artikel].Liefertermin>GetDate() /*Date()*/,[angebotene Artikel].Liefertermin,GetDate() /*Date()*/+3);

						----> Query 5  lfou9 mrigl 

						INSERT INTO [PSZ_BH_Liquidationsplanung Skontozahler] ( Typ, [Angebot-Nr], Ausliefertermin
						, Gesamtpreis, Brutto, Konditionen, Zahlungseingang, Name1 )
						SELECT Angebote.Typ
						, Angebote.[Angebot-Nr]
						, IIf([angebotene Artikel].Liefertermin>GetDate() /*Date()*/
						,[angebotene Artikel].Liefertermin,GetDate() /*Date()*/+3) AS Ausliefertermin
						, [angebotene Artikel].Gesamtpreis
						, ([Gesamtpreis]*1.19)*0.98 AS Brutto
						, Angebote.Konditionen
						, IIf([angebotene Artikel].Liefertermin>GetDate() /*Date()*/
						,[angebotene Artikel].Liefertermin,GetDate() /*Date()*/+3)+2+
						TRY_PARSE((Left([Konditionen],2)) AS INT) AS Zahlungseingang
						, adressen.Name1
						FROM (Angebote INNER JOIN [angebotene Artikel] 
						ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) 
						INNER JOIN adressen 
						ON Angebote.[Kunden-Nr] = adressen.Nr
						WHERE (((Angebote.Typ)='Auftragsbestätigung') 
						AND ((Angebote.Konditionen)='21 Tage 2%, 45 Tage netto') 
						AND ((Angebote.gebucht)=1) AND ((Angebote.erledigt)=0) 
						AND (([angebotene Artikel].erledigt_pos)=0))
						ORDER BY IIf([angebotene Artikel].Liefertermin>GetDate() /*Date()*/,[angebotene Artikel].Liefertermin,GetDate() /*Date()*/+3);

						------> Query 6

						SELECT 
						COUNT(*) over() TotalCount,
						[PSZ_BH_Liquidationsplanung Skontozahler].Name1
						,
						 CONVERT(date, [PSZ_BH_Liquidationsplanung Skontozahler].Ausliefertermin) Ausliefertermin
						, [PSZ_BH_Liquidationsplanung Skontozahler].Konditionen
						, CONVERT(date,[PSZ_BH_Liquidationsplanung Skontozahler].Zahlungseingang) Zahlungseingang
						, Sum([PSZ_BH_Liquidationsplanung Skontozahler].Brutto) AS Brutto_inkl_Skonto
						FROM [PSZ_BH_Liquidationsplanung Skontozahler]
						GROUP BY 
						[PSZ_BH_Liquidationsplanung Skontozahler].Name1
						, CONVERT(date, [PSZ_BH_Liquidationsplanung Skontozahler].Ausliefertermin)
						, [PSZ_BH_Liquidationsplanung Skontozahler].Konditionen
						,CONVERT(date,[PSZ_BH_Liquidationsplanung Skontozahler].Zahlungseingang) 
						HAVING   
						CONVERT(date,[PSZ_BH_Liquidationsplanung Skontozahler].Zahlungseingang)>= (DATEADD(day ,17,CONVERT(date,GetDate())))
						And 
						CONVERT(date,[PSZ_BH_Liquidationsplanung Skontozahler].Zahlungseingang)<=( DATEADD(day ,30,CONVERT(date,GetDate())))
						
						
                      ";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.FNC.Accounting.LiquiditatsplanungSkontozahlerEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.FNC.Accounting.LiquiditatsplanungSkontozahlerEntity>();
			}
		}
		public static List<Entities.Joins.FNC.Accounting.LiquiditatsplanungOffeneMaterialbestellungenEntity> GetLiquiditatsplanungOffeneMaterialbestellungen(DateTime from, DateTime to, Settings.PaginModel paging = null, Settings.SortingModel sortingModel = null, int all = 0)
		{
			var dataTable = new DataTable();
			string paginationFilter = null;
			string sortingField = "Bestätigter_Termin";
			string sortingdesc = ((sortingModel is not null) && sortingModel.SortDesc) ? " Desc " : " asc ";

			if(sortingModel is not null)
			{
				if(!string.IsNullOrWhiteSpace(sortingModel.SortFieldName))
				{
					sortingField = sortingModel.SortFieldName;
				}
			}

			if(paging == null && all == 1)
			{
				paginationFilter = "";

			}
			else if(paging != null)
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
							,Bestellungen.Benutzer
							, Bestellungen.[Unser Zeichen] AS Lieferantennr
							, Bestellungen.[Vorname/NameFirma] AS Lieferant
							, Bestellungen.[Bestellung-Nr]
							, [bestellte Artikel].Anzahl
							, Bestellnummern.Mindestbestellmenge
							, Bestellnummern.Verpackungseinheit
							, [bestellte Artikel].[Bezeichnung 1]
							, Artikel.Artikelnummer
							, [bestellte Artikel].Bestellnummer
							, [bestellte Artikel].Einzelpreis
							, [bestellte Artikel].Gesamtpreis
							, [bestellte Artikel].Bestätigter_Termin AS Anlieferung
							, Konditionszuordnungstabelle.Nettotage AS [Zahlungsziel Netto]
							,IIf([bestellte Artikel].[Bestätigter_Termin]<cast(cast('99990101' as char(8)) as datetime)
							,DATEADD(day, [Nettotage], [bestellte Artikel].Bestätigter_Termin),[bestellte Artikel].[Bestätigter_Termin])AS Fälligkeit
							, IIf([bestellte Artikel].Lagerort_id=6 Or [bestellte Artikel].Lagerort_id=3,'CZ'
							,IIf([bestellte Artikel].Lagerort_id=58 Or [bestellte Artikel].Lagerort_id=60 Or [bestellte Artikel].Lagerort_id=58,'BE-TN'
							,IIf([bestellte Artikel].Lagerort_id=4 Or [bestellte Artikel].Lagerort_id=7,'TN'
							,IIf([bestellte Artikel].Lagerort_id=41 Or [bestellte Artikel].Lagerort_id=42,'KHTN'
							,IIf([bestellte Artikel].Lagerort_id=24 Or [bestellte Artikel].Lagerort_id=26,'AL'
							,IIf([bestellte Artikel].Lagerort_id=20 Or [bestellte Artikel].Lagerort_id=21,'SC'
							,IIf([bestellte Artikel].Lagerort_id=60,'BE-TN'
							,IIf([bestellte Artikel].Lagerort_id=102,'GZ-TN','D')))))))) AS Produktionsstätte
							, Bestellungen.Mandant
							, Bestellungen.Bearbeiter
							, Bestellungen.Datum AS Belegdatum
							, [bestellte Artikel].Liefertermin AS Wünschtermin
							, [bestellte Artikel].Bemerkung_Pos
							, Bestellnummern.Standardlieferant
							FROM (((Bestellungen INNER JOIN [bestellte Artikel] 
							ON Bestellungen.Nr=[bestellte Artikel].[Bestellung-Nr]) INNER JOIN Artikel 
							ON [bestellte Artikel].[Artikel-Nr]=Artikel.[Artikel-Nr]) 
							INNER JOIN (Lieferanten 
							INNER JOIN Konditionszuordnungstabelle 
							ON Lieferanten.[Konditionszuordnungs-Nr]=Konditionszuordnungstabelle.Nr) 
							ON Bestellungen.[Lieferanten-Nr]=Lieferanten.nummer) 
							INNER JOIN Bestellnummern 
							ON [bestellte Artikel].[Artikel-Nr]=Bestellnummern.[Artikel-Nr]
							WHERE (Bestellnummern.Standardlieferant=1) 
							and ( IIf([bestellte Artikel].Bestätigter_Termin<cast(cast('29001231' as char(8)) as datetime),[bestellte Artikel].Bestätigter_Termin,[bestellte Artikel].Liefertermin)>=cast(cast('{from.ToString("yyyyMMdd")}' as char(8)) as datetime) 
							And IIf([bestellte Artikel].Bestätigter_Termin<cast(cast('29001231' as char(8)) as datetime),[bestellte Artikel].Bestätigter_Termin,[bestellte Artikel].Liefertermin)<=cast(cast('{to.ToString("yyyyMMdd")}' as char(8)) as datetime))
							And (Bestellungen.Typ='Bestellung')
							And (Bestellungen.erledigt=0) 
							And ([bestellte Artikel].erledigt_pos=0) 
							And (Bestellungen.Rahmenbestellung=0) 
							And (Left([artikelnummer],3)) <>'227' 
							And (Left([artikelnummer],3)) <> '226'
							And (Bestellungen.gebucht=1)
						ORDER BY {sortingField}  {sortingdesc}
                      {paginationFilter}";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.FNC.Accounting.LiquiditatsplanungOffeneMaterialbestellungenEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.FNC.Accounting.LiquiditatsplanungOffeneMaterialbestellungenEntity>();
			}
		}
		public static List<Entities.Joins.FNC.Accounting.ZahlungskonditionenKundenEntity> GetZahlungskonditionenKunden(Settings.PaginModel paging = null, Settings.SortingModel sortingModel = null, int all = 0)
		{
			var dataTable = new DataTable();




			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
						$@"
						SELECT 
							Count(*) over() TotalCount
							, Konditionszuordnungstabelle.Nr KonditionszuordnungstabelleNr
							,adressen.Nr adressenNr
							,adressen.Name1
							, adressen.PLZ_Straße
							, adressen.Ort
							, adressen.Land
							, adressen.Kundennummer
							, Konditionszuordnungstabelle.Text
							FROM 
							(adressen INNER JOIN Kunden 
							ON adressen.Nr = Kunden.nummer) 
							INNER JOIN Konditionszuordnungstabelle 
							ON Kunden.[Konditionszuordnungs-Nr] = Konditionszuordnungstabelle.Nr
							WHERE (((adressen.Kundennummer)>0))
						";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.FNC.Accounting.ZahlungskonditionenKundenEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.FNC.Accounting.ZahlungskonditionenKundenEntity>();
			}
		}
		public static List<Entities.Joins.FNC.Accounting.ZahlungskonditionenLieferantenEntity> GetZahlungskonditionenLieferanten(Settings.PaginModel paging = null, Settings.SortingModel sortingModel = null, int all = 0)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
						$@"
						SELECT 
							Count(*) over() TotalCount
							, Konditionszuordnungstabelle.Nr KonditionszuordnungstabelleNr
							,adressen.Nr adressenNr
							,adressen.Name1
							, adressen.PLZ_Straße
							, adressen.Ort
							, adressen.Land
							, adressen.Lieferantennummer
							, Konditionszuordnungstabelle.Text
							FROM 
							(adressen INNER JOIN Lieferanten 
							ON adressen.Nr = Lieferanten.nummer) 
							INNER JOIN Konditionszuordnungstabelle 
							ON Lieferanten.[Konditionszuordnungs-Nr] = Konditionszuordnungstabelle.Nr
							WHERE (((adressen.Lieferantennummer)>0))
						";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.FNC.Accounting.ZahlungskonditionenLieferantenEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.FNC.Accounting.ZahlungskonditionenLieferantenEntity>();
			}
		}
		public static List<Entities.Joins.FNC.Accounting.FactoringRgGutschriftlisteEntity> GetFactoringRgGutschriftlist(string typ, DateTime from, DateTime to, Settings.PaginModel paging = null, Settings.SortingModel sortingModel = null, int all = 0)
		{
			var dataTable = new DataTable();
			string paginationFilter = null;
			string sortingField = "adressen.Kundennummer";
			string sortingdesc = ((sortingModel is not null) && sortingModel.SortDesc) ? " Desc " : " asc ";

			if(sortingModel is not null)
			{
				if(!string.IsNullOrWhiteSpace(sortingModel.SortFieldName))
				{
					sortingField = sortingModel.SortFieldName;
				}
			}

			if(paging == null && all == 1)
			{
				paginationFilter = "";

			}
			else if(paging != null)
			{
				paginationFilter = $"OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY";
			}

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
						$@"
						SELECT 
							count(*) over() TotalCount,
							adressen.Kundennummer AS Debitor
							, Angebote.[Angebot-Nr] AS [Beleg-Nr]
							, Angebote.Typ
							, Angebote.Datum
							, Round(Sum([angebotene Artikel].Gesamtpreis*(1+[angebotene Artikel].USt)),2) AS Betrag
							, 'EUR' AS Währung
							, ([USt]) AS [MwSt-Satz]
							, Angebote.Fälligkeit AS [Fällig am]
							, Konditionszuordnungstabelle.Nettotage AS [Netto-Laufzeit]
							--, ' ' AS [Bezugbeleg-Nr]
							, Konditionszuordnungstabelle.Skontotage AS [Skontotage 1]
							, Konditionszuordnungstabelle.Skonto AS [Kondition 1]
							--, ' ' AS [Skontotage 2]
							--, ' ' AS [Kondition 2]
							FROM (((Angebote 
							INNER JOIN adressen ON Angebote.[Kunden-Nr] = adressen.Nr) 
							INNER JOIN [angebotene Artikel] 
							ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) INNER JOIN Kunden 
							ON adressen.Nr = Kunden.nummer) INNER JOIN Konditionszuordnungstabelle 
							ON Kunden.[Konditionszuordnungs-Nr] = Konditionszuordnungstabelle.Nr
							GROUP BY adressen.Kundennummer
							, Angebote.[Angebot-Nr]
							, Angebote.Typ
							, Angebote.Datum, ([USt])
							, Angebote.Fälligkeit
							, Konditionszuordnungstabelle.Nettotage
							, Konditionszuordnungstabelle.Skontotage
							, Konditionszuordnungstabelle.Skonto
							, Kunden.Factoring
							HAVING 
							Angebote.Typ ='{typ}'
							AND 
							(Angebote.Datum)>='{from.ToString("yyyyMMdd")}'--'2900/12/31'--[Von Datum] 
							And
							Angebote.Datum<='{to.ToString("yyyyMMdd")}'--[Bis Datum]) 
							AND (Kunden.Factoring=1)
						ORDER BY {sortingField}  {sortingdesc}
						{paginationFilter}";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.FNC.Accounting.FactoringRgGutschriftlisteEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.FNC.Accounting.FactoringRgGutschriftlisteEntity>();
			}
		}
		public static List<Entities.Joins.FNC.Accounting.AusfuhrEntity> GetAusfuhr(DateTime from, DateTime to, Settings.PaginModel paging = null, Settings.SortingModel sortingModel = null, int all = 0)
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
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.FNC.Accounting.AusfuhrEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.FNC.Accounting.AusfuhrEntity>();
			}
		}
		public static List<Entities.Joins.FNC.Accounting.StammdatenkontrolleWareneingangeEntity> GetStammdatenkontrolleWareneingange(DateTime from, DateTime to, string gruppe, Settings.PaginModel paging = null, Settings.SortingModel sortingModel = null, int all = 0)
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

							Lieferanten.Lieferantengruppe='{gruppe}'--[Gruppe] 
							AND Bestellungen.Datum>='{from.ToString("yyyyMMdd")}'--[Ab Datum] 
							And Bestellungen.Datum<='{to.ToString("yyyyMMdd")}'--[Bis] ) 
							AND
							Bestellungen.Typ='Wareneingang'
						ORDER BY {sortingField}  {sortingdesc}
                      {paginationFilter}";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.FNC.Accounting.StammdatenkontrolleWareneingangeEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.FNC.Accounting.StammdatenkontrolleWareneingangeEntity>();
			}
		}
		public static List<Entities.Joins.FNC.Accounting.StammdatenkontrolleWareneingangeCountEntity> GetStammdatenkontrolleWareneingangeCount(DateTime from, DateTime to, string gruppe = "Inland")
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
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.FNC.Accounting.StammdatenkontrolleWareneingangeCountEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.FNC.Accounting.StammdatenkontrolleWareneingangeCountEntity>();
			}
		}
		public static List<Entities.Joins.FNC.Accounting.RMDCZEntity> GetRMDCZ(DateTime from, DateTime to, Settings.PaginModel paging = null, Settings.SortingModel sortingModel = null, int all = 0)
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
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.FNC.Accounting.RMDCZEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.FNC.Accounting.RMDCZEntity>();
			}
		}
		public static List<Entities.Joins.FNC.Accounting.EinfuhrEntity> GetEinfuhr(DateTime from, DateTime to, string gruppe, Settings.PaginModel paging = null, Settings.SortingModel sortingModel = null, int all = 0)
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
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.FNC.Accounting.EinfuhrEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.FNC.Accounting.EinfuhrEntity>();
			}
		}
		public static List<Entities.Joins.FNC.Accounting.LieferantenGruppeEntity> GetSupplierGroups()
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
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.FNC.Accounting.LieferantenGruppeEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.FNC.Accounting.LieferantenGruppeEntity>();
			}
		}
		public static List<Entities.Joins.FNC.Accounting.RechnungstransferEntity> GetRechnungstransfer(int userId, Settings.PaginModel paging = null, Settings.SortingModel sortingModel = null, int all = 0)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
						$@"
						DROP TABLE IF EXISTS #PSZ_BH_Transfertabelle_aktuell   ;
CREATE TABLE #PSZ_BH_Transfertabelle_aktuell         
								(
									Belegdatum datetime,
									Periode smallint,
									Belegnummer int,
									Buchungstext varchar(40),
									Betrag float,
									Whrg nvarchar(6),
									Sollkto nvarchar(20),
									Habenkto nvarchar(20),
									Bezug nvarchar(250),
									ArchiveUserId int, 
									ArchiveDate datetime, 
									ArchiveIndex bigint
								);

with PSZ_BH_Transfer_01
							as (
							 SELECT Angebote.Datum
							,MONTH([Datum]) Periode
							, Angebote.[Angebot-Nr]
							, Angebote.Typ
							, Artikel.Warengruppe
							, Sum([angebotene Artikel].Gesamtpreis) AS SummevonGesamtpreis
							, 'EUR' AS Whrg
							, adressen.Kundennummer AS Sollkto
							, [angebotene Artikel].USt
							, fibu_kunden_rahmen.Rahmen
							,(LEFT(Angebote.Bezug , 250) ) Bezug
							FROM ((((Angebote LEFT JOIN [angebotene Artikel] 
							ON Angebote.Nr = [angebotene Artikel].[Angebot-Nr]) 
							LEFT JOIN adressen 
							ON Angebote.[Kunden-Nr] = adressen.Nr) LEFT JOIN Kunden 
							ON adressen.Nr = Kunden.nummer) LEFT JOIN fibu_kunden_rahmen 
							ON Kunden.fibu_rahmen = fibu_kunden_rahmen.ID) LEFT JOIN Artikel 
							ON [angebotene Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]
							GROUP BY Angebote.Datum
							, Right((Left([Datum],5)),2)
							, Angebote.[Angebot-Nr], Angebote.Typ
							, Artikel.Warengruppe
							, adressen.Kundennummer
							, [angebotene Artikel].USt
							, fibu_kunden_rahmen.Rahmen
							, Angebote.gebucht
							,Angebote.Bezug 
							HAVING (((Angebote.Datum)>(GetDate() /*Date()*/-180)) 
							AND ((Angebote.Typ)='Rechnung' Or (Angebote.Typ)='Gutschrift') 
							AND ((Angebote.gebucht)=1))
							)
							, PSZ_BH_Transfer_02  as (
							SELECT PSZ_BH_Transfer_01.*, PSZ_BH_Kontenrahmen.Habenkto
							FROM PSZ_BH_Transfer_01 
							INNER JOIN PSZ_BH_Kontenrahmen 
							ON PSZ_BH_Transfer_01.Warengruppe = PSZ_BH_Kontenrahmen.Warengruppe
							AND PSZ_BH_Transfer_01.Rahmen = PSZ_BH_Kontenrahmen.Rahmen)
							

							INSERT INTO #PSZ_BH_Transfertabelle_aktuell ( Belegdatum, Periode, Belegnummer, Buchungstext, Betrag, Whrg, Sollkto, Habenkto,Bezug, ArchiveUserId, ArchiveDate, ArchiveIndex ) (
							SELECT 
							CAST(PSZ_BH_Transfer_02.Datum AS DATE) Belegdatum 
							, PSZ_BH_Transfer_02.Periode
							, PSZ_BH_Transfer_02.[Angebot-Nr] Belegnummer
							, PSZ_BH_Transfer_02.Typ Buchungstext
							, (IIf([Typ]='Gutschrift',[SummevonGesamtpreis]*(1+[Ust])*(-1),[SummevonGesamtpreis]*(1+[Ust]))) AS Betrag
							, PSZ_BH_Transfer_02.Whrg
							, PSZ_BH_Transfer_02.Sollkto
							, PSZ_BH_Transfer_02.Habenkto
							,PSZ_BH_Transfer_02.Bezug
							,{userId} as ArchiveUserId
							,getdate() as ArchiveDate
							,(select max(ISNULL(ArchiveIndex,0))+1 from PSZ_BH_Transfertabelle_Archiv) as ArchiveIndex
							FROM PSZ_BH_Transfer_02 LEFT JOIN PSZ_BH_Transfertabelle_Archiv 
							ON PSZ_BH_Transfer_02.[Angebot-Nr] = PSZ_BH_Transfertabelle_Archiv.Belegnummer
							WHERE PSZ_BH_Transfertabelle_Archiv.Belegnummer Is Null
							)

							INSERT INTO PSZ_BH_Transfertabelle_Archiv ( Belegdatum, Periode, Belegnummer, Buchungstext, Betrag, Whrg, Sollkto, Habenkto,Bezug, ArchiveUserId, ArchiveDate, ArchiveIndex ) 
							(SELECT CAST(Belegdatum AS DATE) Belegdatum, Periode, Belegnummer, Buchungstext, Betrag, Whrg, Sollkto, Habenkto,Bezug, ArchiveUserId, ArchiveDate, ArchiveIndex FROM #PSZ_BH_Transfertabelle_aktuell)
							
							select count(*) over() TotalCount ,* from #PSZ_BH_Transfertabelle_aktuell
						";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.FNC.Accounting.RechnungstransferEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.FNC.Accounting.RechnungstransferEntity>();
			}
		}
		public static List<Entities.Joins.FNC.Accounting.RechnungstransferEntity> GetRechnungstransferLast()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
						$@"SELECT 
							CAST(Belegdatum AS DATE) Belegdatum
							,Periode
							,Belegnummer
							,Buchungstext
							,Betrag
							,Whrg
							,Sollkto
							,Habenkto
							,Bezug, 0 TotalCount
							FROM PSZ_BH_Transfertabelle_Archiv
							WHERE ArchiveIndex=(SELECT MAX(ArchiveIndex) FROM PSZ_BH_Transfertabelle_Archiv)
						";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.FNC.Accounting.RechnungstransferEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.FNC.Accounting.RechnungstransferEntity>();
			}
		}
		public static List<Entities.Joins.FNC.Accounting.RechnungstransferEntity> GetRechnungstransferHistory(DateTime dateFrom, DateTime dateTill)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
						$@"
							SELECT 
							CAST(Belegdatum AS DATE)  Belegdatum
							,Periode
							,Belegnummer
							,Buchungstext
							,Betrag
							,Whrg
							,Sollkto
							,Habenkto
							,Bezug, 0 TotalCount
							FROM PSZ_BH_Transfertabelle_Archiv
							WHERE  '{dateFrom.ToString("yyyyMMdd")}'<=CAST([Belegdatum] as DATE) AND CAST([Belegdatum] as DATE)<='{dateTill.ToString("yyyyMMdd")}'
						";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.FNC.Accounting.RechnungstransferEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.FNC.Accounting.RechnungstransferEntity>();
			}
		}
		public static int UpdateZahlungskonditionenLieferantenWithTransaction(Entities.Joins.FNC.Accounting.ZahlungskonditionenLieferantenEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			//string query = "UPDATE [PSZ_BH_Kontenrahmen] SET [Habenkto]=@Habenkto, [Rahmen]=@Rahmen, [Warengruppe]=@Warengruppe WHERE [ID]=@ID";
			string query = @"UPDATE a
							  SET a.Name1 = @Name1,a.PLZ_Straße =@PLZ_Straße,a.Ort =@Ort,a.Land = @Land,a.Lieferantennummer=@Lieferantennummer
							  FROM 
							(adressen a INNER JOIN Lieferanten 
							ON a.Nr = Lieferanten.nummer) 
							INNER JOIN Konditionszuordnungstabelle 
							ON Lieferanten.[Konditionszuordnungs-Nr] = Konditionszuordnungstabelle.Nr
							WHERE ((((a.Lieferantennummer)>0)) and a.Nr = @adressenNr)";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("adressenNr", item.adressenNr);
			sqlCommand.Parameters.AddWithValue("Name1", item.Name1);
			sqlCommand.Parameters.AddWithValue("PLZ_Straße", item.PLZ_Strabe);
			sqlCommand.Parameters.AddWithValue("Ort", item.Ort == null ? (object)DBNull.Value : item.Ort);
			sqlCommand.Parameters.AddWithValue("Land", item.Land == null ? (object)DBNull.Value : item.Land);
			sqlCommand.Parameters.AddWithValue("Lieferantennummer", item.Lieferantennummer);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateZahlungskonditionenLieferantenWithTransaction2(Entities.Joins.FNC.Accounting.ZahlungskonditionenLieferantenEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			string query = @"UPDATE k
					  SET k.Text = @text
					  FROM (adressen INNER JOIN Lieferanten 
					ON adressen.Nr = Lieferanten.nummer) 
					INNER JOIN Konditionszuordnungstabelle  k
					ON Lieferanten.[Konditionszuordnungs-Nr] = k.Nr
					WHERE ((((adressen.Lieferantennummer)>0))  and K.Nr =  @KonditionNr)";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("KonditionNr", item.KonditionszuordnungstabelleNr);
			sqlCommand.Parameters.AddWithValue("text", item.Text == null ? (object)DBNull.Value : item.Text);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateZahlungskonditionenKundenWithTransaction(Entities.Joins.FNC.Accounting.ZahlungskonditionenKundenEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			//string query = "UPDATE [PSZ_BH_Kontenrahmen] SET [Habenkto]=@Habenkto, [Rahmen]=@Rahmen, [Warengruppe]=@Warengruppe WHERE [ID]=@ID";
			string query = @"UPDATE a
							  SET a.Name1 = @Name1,a.PLZ_Straße =@PLZ_Straße,a.Ort =@Ort,a.Land = @Land,a.Kundennummer=@Kundennummer
							 FROM 
							(adressen a INNER JOIN Kunden 
							ON a.Nr = Kunden.nummer) 
							INNER JOIN Konditionszuordnungstabelle 
							ON Kunden.[Konditionszuordnungs-Nr] = Konditionszuordnungstabelle.Nr
							WHERE ((((a.Kundennummer)>0)) and a.Nr = @adressenNr)";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("adressenNr", item.adressenNr);
			sqlCommand.Parameters.AddWithValue("Name1", item.Name1);
			sqlCommand.Parameters.AddWithValue("PLZ_Straße", item.PLZ_Strabe == null ? (object)DBNull.Value : item.PLZ_Strabe);
			sqlCommand.Parameters.AddWithValue("Ort", item.Ort == null ? (object)DBNull.Value : item.Ort);
			sqlCommand.Parameters.AddWithValue("Land", item.Land == null ? (object)DBNull.Value : item.Land);
			sqlCommand.Parameters.AddWithValue("Kundennummer", item.Kundennummer);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateZahlungskonditionenKundenWithTransaction2(Entities.Joins.FNC.Accounting.ZahlungskonditionenKundenEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			string query = @"UPDATE k
							  SET k.Text = @text
							 FROM 
							(adressen INNER JOIN Kunden 
							ON adressen.Nr = Kunden.nummer) 
							INNER JOIN Konditionszuordnungstabelle k
							ON Kunden.[Konditionszuordnungs-Nr] = k.Nr
							WHERE (((adressen.Kundennummer)>0)) and K.Nr =  @KonditionNr";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("text", item.Text == null ? (object)DBNull.Value : item.Text);
			sqlCommand.Parameters.AddWithValue("KonditionNr", item.KonditionszuordnungstabelleNr);
			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static List<KeyValuePair<int, int>> GetInternalOrders(int id, bool byCompany)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = byCompany

					? @"select COUNT(OrderId) as Count,[Level] from [__FNC_BestellungenExtension]
where  BudgetYear=YEAR(GETDATE()) and 0<[Level] AND [ApprovalUserId] is null and CompanyId=@id  group by  [Level],CompanyId"
:
@"select COUNT(OrderId) as Count,[Level] from [__FNC_BestellungenExtension] 
where  BudgetYear=YEAR(GETDATE()) and 0<[Level] AND [ApprovalUserId] is null and DepartmentId=@id group by  [Level],DepartmentId";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, int>(Convert.ToInt32(x["Count"]), Convert.ToInt32(x["Level"]))).ToList();
			}
			else
			{
				return new List<KeyValuePair<int, int>>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.FNC.Accounting.CompanyExtOrdersNotFullValidatedEntity> GetExternalOrdersNotFullyValidated(int id)
		{

			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query =
						$@"
							select COUNT(V.OrderId) as Count,V.ValidationLevel ,B.CompanyName, V.Username
from [__FNC_BestellungenExtension] B inner join [__FNC_OrderValidation] V
on B.OrderId=V.OrderId
where B.CompanyId=@id
and B.OrderType='External'
and V.ValidationLevel <= 2
group by V.ValidationLevel,B.CompanyName, V.Username";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.FNC.Accounting.CompanyExtOrdersNotFullValidatedEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.FNC.Accounting.CompanyExtOrdersNotFullValidatedEntity>();
			}

		}
	}
}
