using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Joins.MTM.Order.Statistics
{
	public class Offene_mat_bst_access
	{
		/// <summary>
		/// it returns all the data if all is 1 or it add the pagination filter if all is 0
		/// </summary>
		/// <param name="paging"></param>
		/// <param name="sortingModel"></param>
		/// <param name="Benutzer"></param>
		/// <param name="fromdate"></param>
		/// <param name="todate"></param>
		/// <param name="all"></param>
		/// <returns></returns>
		public static List<Entities.Joins.MTM.Order.Statistics.OffeneMat_BstEntity> GetOffeneMatBst(Settings.PaginModel paging = null, Settings.SortingModel sortingModel = null, string Benutzer = "", DateTime fromdate = default, DateTime todate = default, int all = 0)
		{
			var dataTable = new DataTable();
			string paginationFilter = "";
			string sortingField = "[bestellte Artikel].Bestätigter_Termin";
			string sortingdesc = ((sortingModel is not null) && sortingModel.SortDesc) ? " Desc " : " asc ";
			string Benutzerfilter = " ";
			if(paginationFilter is not null && all == 1)
			{
				paginationFilter = "";
			}
			else if(paginationFilter is not null)
			{
				paginationFilter = $"OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY";
			}
			if(sortingModel is not null && !string.IsNullOrEmpty(sortingModel.SortFieldName))
			{
				sortingField = sortingModel.SortFieldName;
			}
			if(Benutzer is not null && !string.IsNullOrWhiteSpace(Benutzer))
			{
				Benutzerfilter = @$"where Benutzer like '{Benutzer.SqlEscape()}%'";
			}
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				//paginationFilter
				string query =
						$@"
							SELECT Bestellungen.Benutzer
							, Bestellungen.[Unser Zeichen] AS Lieferantennr
							, Bestellungen.[Vorname/NameFirma] AS Lieferant
							, Bestellungen.[Bestellung-Nr] --
							, [bestellte Artikel].Anzahl --
							, Bestellnummern.Mindestbestellmenge
							, Bestellnummern.Verpackungseinheit
							, [bestellte Artikel].[Bezeichnung 1]
							, Artikel.Artikelnummer --
							, [bestellte Artikel].Bestellnummer
							, [bestellte Artikel].Einzelpreis
							, [bestellte Artikel].Gesamtpreis --
							, [bestellte Artikel].Bestätigter_Termin AS Anlieferung
							, Konditionszuordnungstabelle.Nettotage AS [Zahlungsziel Netto]
							,IIf([bestellte Artikel].[Bestätigter_Termin]<'99990101',DATEADD(day, [Nettotage], [bestellte Artikel].Bestätigter_Termin),[bestellte Artikel].[Bestätigter_Termin])AS Fälligkeit
							,IIf([bestellte Artikel].Lagerort_id=6 Or [bestellte Artikel].Lagerort_id=3,'CZ'
							,IIf([bestellte Artikel].Lagerort_id=58 Or [bestellte Artikel].Lagerort_id=60,'BE-TN'
							,IIf([bestellte Artikel].Lagerort_id=4 Or [bestellte Artikel].Lagerort_id=7,'TN'
							,IIf([bestellte Artikel].Lagerort_id=41 Or [bestellte Artikel].Lagerort_id=42,'WS'
							,IIf([bestellte Artikel].Lagerort_id=24 Or [bestellte Artikel].Lagerort_id=26,'AL'
							,IIf([bestellte Artikel].Lagerort_id=20 Or [bestellte Artikel].Lagerort_id=21,'SC'
							,IIf([bestellte Artikel].Lagerort_id=101 Or [bestellte Artikel].Lagerort_id=102,'GZ'
							,'D'))))))) 
							AS Produktionsstätte
							, Bestellungen.Mandant
							, Bestellungen.Bearbeiter
							, Bestellungen.Datum AS Belegdatum
							, [bestellte Artikel].Liefertermin AS Wünschtermin
							, [bestellte Artikel].Bemerkung_Pos
							, IIF(Bestellnummern.Standardlieferant=1,'true','false') Standardlieferant
							, r.[Angebot-Nr] RaNumber, r.Nr RaNr							
							,count(*) over () TotalCount
							FROM ((
							(Bestellungen INNER JOIN [bestellte Artikel] 
							ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]) 
							INNER JOIN Artikel 
							ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN (Lieferanten INNER JOIN Konditionszuordnungstabelle 
							ON Lieferanten.[Konditionszuordnungs-Nr] = Konditionszuordnungstabelle.Nr) 
							ON Bestellungen.[Lieferanten-Nr] = Lieferanten.nummer) INNER JOIN Bestellnummern 
							ON [bestellte Artikel].[Artikel-Nr] = Bestellnummern.[Artikel-Nr]
							LEFT JOIN [angebotene Artikel] p ON p.[Nr]=[bestellte Artikel].[RA Pos zu Bestellposition] LEFT JOIN angebote r ON p.[Angebot-Nr]=r.Nr							
							WHERE (((Bestellnummern.Standardlieferant)=1) 
							AND ((IIf([bestellte Artikel].[Bestätigter_Termin]<'29001231',[bestellte Artikel].[Bestätigter_Termin],[bestellte Artikel].[Liefertermin]))>='{fromdate.ToString("yyyyMMdd")}'
							And (IIf([bestellte Artikel].[Bestätigter_Termin]<'29001231',[bestellte Artikel].[Bestätigter_Termin],[bestellte Artikel].[Liefertermin]))<='{todate.ToString("yyyyMMdd")}'
							) 
							AND ((Bestellungen.Typ)='Bestellung') 
							AND ((Bestellungen.erledigt)=0) 
							AND (([bestellte Artikel].erledigt_pos)=0) 
							AND ((Bestellungen.Rahmenbestellung)=0) 
							AND ((Left([artikelnummer],3))<>'227' And (Left([artikelnummer],3))<>'226') AND ((Bestellungen.gebucht)=1))
							{Benutzerfilter}
							ORDER BY {sortingField} {sortingdesc}
							{paginationFilter}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 300;
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.OffeneMat_BstEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.OffeneMat_BstEntity>();
			}
		}

		/// <summary>
		/// it returns all the data if all is 1 or it add the pagination filter if all is 0
		/// </summary>
		/// <param name="paging"></param>
		/// <param name="sortingModel"></param>
		/// <param name="Benutzer"></param>
		/// <param name="fromdate"></param>
		/// <param name="todate"></param>
		/// <param name="all"></param>
		/// <returns></returns>
		public static List<Entities.Joins.MTM.Order.Statistics.GeschMat_BstEntity> GetGeschlMatBst(Settings.PaginModel paging = null, Settings.SortingModel sortingModel = null, string Benutzer = "", DateTime fromdate = default, DateTime todate = default, int all = 0)
		{
			var dataTable = new DataTable();
			string paginationFilter = "";
			string sortingField = "Bestellungen.Benutzer, IIf([bestellte Artikel].Bestätigter_Termin<'29001231' ,[bestellte Artikel].Bestätigter_Termin,[bestellte Artikel].Liefertermin)";
			string sortingdesc = ((sortingModel is not null) && sortingModel.SortDesc) ? " Desc " : " asc ";
			string Benutzerfilter = " ";
			if(paginationFilter is not null && all == 1)
			{
				paginationFilter = "";
			}
			else if(paging is not null)
			{
				paginationFilter = $"OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY";
			}
			if(sortingModel is not null && !string.IsNullOrEmpty(sortingModel.SortFieldName))
			{
				sortingField = sortingModel.SortFieldName;
			}
			if(Benutzer is not null && !string.IsNullOrWhiteSpace(Benutzer))
			{
				Benutzerfilter = @$"where Benutzer like '{Benutzer.SqlEscape()}%'";
			}
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				//paginationFilter
				string query =
						$@"
							SELECT 
									Bestellungen.Benutzer
									, Bestellungen.[Unser Zeichen] AS Lieferantennr
									, Bestellungen.[Vorname/NameFirma] AS Lieferant
									, Bestellungen.[Bestellung-Nr]
									, [bestellte Artikel].[Start Anzahl]
									, Bestellnummern.Mindestbestellmenge
									, Bestellnummern.Verpackungseinheit
									, [bestellte Artikel].[Bezeichnung 1]
									, Artikel.Artikelnummer
									, [bestellte Artikel].Bestellnummer
									, [bestellte Artikel].Einzelpreis
									, [Start Anzahl]*[Einzelpreis] AS Preis_Gesamt
									, IIf([bestellte Artikel].Bestätigter_Termin<'29001231',[bestellte Artikel].Bestätigter_Termin
									,[bestellte Artikel].Liefertermin) AS Anlieferung, Konditionszuordnungstabelle.Nettotage AS [Zahlungsziel Netto]
									, [bestellte Artikel].Liefertermin+[Nettotage] AS Fälligkeit
									,IIf([bestellte Artikel].Lagerort_id=6 Or [bestellte Artikel].Lagerort_id=3,'CZ'
									,IIf([bestellte Artikel].Lagerort_id=4 Or [bestellte Artikel].Lagerort_id=7,'TN'
									,IIf([bestellte Artikel].Lagerort_id=42 Or [bestellte Artikel].Lagerort_id=41,'WS'
									,IIf([bestellte Artikel].Lagerort_id=20 Or [bestellte Artikel].Lagerort_id=21,'SC'
									,IIf([bestellte Artikel].Lagerort_id=24 Or [bestellte Artikel].Lagerort_id=26,'AL'
									,IIf([bestellte Artikel].Lagerort_id=102 Or [bestellte Artikel].Lagerort_id=101,'GZ'
									,IIf([bestellte Artikel].Lagerort_id=60 Or [bestellte Artikel].Lagerort_id=58,'BE-TN','D'))))))) AS Produktionsstätte
									,Bestellungen.Mandant, Bestellungen.Bearbeiter
									,Bestellungen.Datum AS Belegdatum, [bestellte Artikel].Liefertermin AS Wünschtermin
									,IIF( Bestellnummern.Standardlieferant=1,'true','false') Standardlieferant
									,count(*) over() TotalCount
									FROM 
									(((Bestellungen INNER JOIN [bestellte Artikel] ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]) 
									INNER JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
									INNER JOIN
									(Lieferanten INNER JOIN Konditionszuordnungstabelle ON Lieferanten.[Konditionszuordnungs-Nr] = Konditionszuordnungstabelle.Nr) 
									ON Bestellungen.[Lieferanten-Nr] = Lieferanten.nummer) 
									INNER JOIN Bestellnummern ON Artikel.[Artikel-Nr] = Bestellnummern.[Artikel-Nr]
									WHERE 
									(((Bestellnummern.Standardlieferant)=1) 
									AND ((IIf([bestellte Artikel].[Bestätigter_Termin]<'29001231'
									,[bestellte Artikel].[Bestätigter_Termin]
									,[bestellte Artikel].[Liefertermin]))>='{fromdate.ToString("yyyyMMdd")}' 
									And (IIf([bestellte Artikel].[Bestätigter_Termin]<'29001231'
									,[bestellte Artikel].[Bestätigter_Termin],[bestellte Artikel].[Liefertermin]))<='{todate.ToString("yyyyMMdd")}'
									) 
									AND ((Bestellungen.Typ)='Bestellung') 
									AND (([bestellte Artikel].erledigt_pos)=1) 
									AND ((Bestellungen.Rahmenbestellung)=0) 
									AND ((Left([artikelnummer],3))<>'227' 
									And (Left([artikelnummer],3))<>'226') 
									AND ((Bestellungen.gebucht)=1))
                                    {Benutzerfilter}
									ORDER  BY {sortingField} {sortingdesc} 
									
										{paginationFilter}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 300;
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.GeschMat_BstEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.GeschMat_BstEntity>();
			}
		}


		/// <summary>
		/// it returns all the data if all is 1 or it add the pagination filter if all is 0
		/// </summary>
		/// <param name="paging"></param>
		/// <param name="all"></param>
		/// <returns></returns>
		public static List<Entities.Joins.MTM.Order.Statistics.GetUngebuchteMatBstEntity> GetUngebuchteMatBstdata(Settings.PaginModel paging = null, int all = 0)
		{
			var dataTable = new DataTable();
			string paginationFilter = "";
			if(all == 1)
			{
				paginationFilter = "";
			}
			if(paging is not null && all == 0)
			{
				paginationFilter = $"OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY";
			}

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				//paginationFilter
				string query =
						$@"
							SELECT 
									Bestellungen.Datum AS [Bestellung angelegt]
									, IIf([bestellte Artikel].Bestätigter_Termin<'29001231'
									,[bestellte Artikel].Bestätigter_Termin
									,[bestellte Artikel].Liefertermin) AS Anlieferung
									, Bestellungen.Bearbeiter AS von
									, Bestellungen.Benutzer
									, Bestellungen.[Unser Zeichen] AS Lieferantennr
									, Bestellungen.[Vorname/NameFirma] AS Lieferant
									, Bestellungen.[Bestellung-Nr]
									, [bestellte Artikel].Anzahl
									, [bestellte Artikel].[Bezeichnung 1]
									, Artikel.Artikelnummer
									, [bestellte Artikel].Bestellnummer
									, [bestellte Artikel].Einzelpreis
									, [bestellte Artikel].Gesamtpreis
									, Konditionszuordnungstabelle.Nettotage AS [Zahlungsziel Netto]
									, Lagerorte.Lagerort AS Fertigungsstätte 
									,Count(*) over () TotalCount
									FROM 
									(((Bestellungen 
									INNER JOIN [bestellte Artikel] ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]) 
									INNER JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) 
									INNER JOIN (Lieferanten 
									INNER JOIN Konditionszuordnungstabelle 
									ON Lieferanten.[Konditionszuordnungs-Nr] = Konditionszuordnungstabelle.Nr) 
									ON Bestellungen.[Lieferanten-Nr] = Lieferanten.nummer) 
									INNER JOIN Lagerorte 
									ON [bestellte Artikel].Lagerort_id = Lagerorte.Lagerort_id
									WHERE (((Bestellungen.Rahmenbestellung)=0) 
									AND ((Bestellungen.Typ)='Bestellung') 
									AND ((Bestellungen.erledigt)=0) 
									AND (([bestellte Artikel].erledigt_pos)=0) 
									AND ((Left([artikelnummer],3))<>'227'
									And (Left([artikelnummer],3))<>'226') AND ((Bestellungen.gebucht)=0))
									ORDER BY Bestellungen.Datum
									, IIf([bestellte Artikel].Bestätigter_Termin<'29001231',[bestellte Artikel].Bestätigter_Termin,[bestellte Artikel].Liefertermin)
									{paginationFilter}
										";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.GetUngebuchteMatBstEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.GetUngebuchteMatBstEntity>();
			}
		}
		/// <summary>
		/// the sql connection timeout is set  70s , it should throw an exception if the connection is exceeding 70 s
		/// </summary>
		/// <param name="Lager_Haup"></param>
		/// <param name="Lager_Fer"></param>
		/// <param name="Lager_fer2"></param>
		/// <returns></returns>
		public static List<Entities.Joins.MTM.Order.Statistics.BestellungohneFAEntity> GetBestellungohneFA(int Lager_Haup, int Lager_Fer, string Lager_fer2)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				//paginationFilter
				string query =
						$@"
							SELECT isnull(T.Artikelnummer,'') as Art
								, isnull(T.Lieferant,'') as Lief
								, isnull(T.[Bestellung-Nr],0) as Be
								, isnull(T.Anzahl,0) as Anz
								, isnull(T.Wünschtermin,'') as WunchT
								, isnull(T.Bestätigter_Termin,'') as BesTermin
								 FROM (SELECT Bestellungen.Benutzer
								 , Bestellungen.[Unser Zeichen] AS Lieferantennr
								 , Bestellungen.[Vorname/NameFirma] AS Lieferant
								 ,Bestellungen.[Bestellung-Nr]
								 , [bestellte Artikel].Anzahl
								 , [bestellte Artikel].[Bezeichnung 1]
								 , Artikel.Artikelnummer
								 , [bestellte Artikel].Bestellnummer
								 , [bestellte Artikel].Einzelpreis
								 , [bestellte Artikel].Gesamtpreis
								 , IIf([bestellte Artikel].Bestätigter_Termin<'12/12/2900'
								 ,[bestellte Artikel].Bestätigter_Termin
								 ,[bestellte Artikel].Liefertermin) AS Anlieferung
								 , Konditionszuordnungstabelle.Nettotage AS [Zahlungsziel Netto]
								 ,IIf([bestellte Artikel].Lagerort_id=6 Or [bestellte Artikel].Lagerort_id=3,'CZ',IIf([bestellte Artikel].Lagerort_id=4 Or [bestellte Artikel].Lagerort_id=7,'TN',IIf([bestellte Artikel].Lagerort_id=41 Or [bestellte Artikel].Lagerort_id=42,'KHTN',IIf([bestellte Artikel].Lagerort_id=24 Or [bestellte Artikel].Lagerort_id=26,'AL',IIf([bestellte Artikel].Lagerort_id=20 Or [bestellte Artikel].Lagerort_id=21,'SC',IIf([bestellte Artikel].Lagerort_id=58 Or [bestellte Artikel].Lagerort_id=60,'BETN','D')))))) AS Produktionsstätte, Bestellungen.Mandant,
								   Bestellungen.Bearbeiter
								   , Bestellungen.Datum AS Belegdatum
								   , [bestellte Artikel].Liefertermin AS Wünschtermin
								   , [bestellte Artikel].Lagerort_id
								   , [bestellte Artikel].Bestätigter_Termin
								   , [bestellte Artikel].Nr
								 FROM 
								 ((Bestellungen INNER JOIN [bestellte Artikel] 
								 ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]) INNER JOIN Artikel 
								 ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]) INNER JOIN (Lieferanten INNER JOIN Konditionszuordnungstabelle 
								 ON Lieferanten.[Konditionszuordnungs-Nr] = Konditionszuordnungstabelle.Nr) 
								 ON Bestellungen.[Lieferanten-Nr] = Lieferanten.nummer
								  WHERE (
								  (([bestellte Artikel].Lagerort_id) ={Lager_Haup}) /*& Lager_Haup &*/
								  And 
								  ((Bestellungen.Typ) = 'Bestellung') 
								  And ((Bestellungen.erledigt) = 0) 
								  And (([bestellte Artikel].erledigt_pos) = 0) 
								  And ((Bestellungen.Rahmenbestellung) = 0) 
								  And ((Left([Artikelnummer], 3)) <> '227' 
								  And (Left([Artikelnummer], 3)) <> '226') 
								  And ((Bestellungen.gebucht) = 1))) AS T
								 GROUP BY T.Artikelnummer
								 , T.Lieferant
								 , T.[Bestellung-Nr]
								 , T.Anzahl
								 , T.Wünschtermin
								 , T.Bestätigter_Termin, T.Nr
								  HAVING (((T.Artikelnummer) Not In (SELECT Artikel.Artikelnummer
								  FROM (Fertigung INNER JOIN Stücklisten 
								  ON Fertigung.Artikel_Nr = Stücklisten.[Artikel-Nr]) 
								  INNER JOIN Artikel 
								  ON Stücklisten.[Artikel-Nr des Bauteils] = Artikel.[Artikel-Nr]
								 WHERE (
								 ( (Fertigung.Lagerort_id)= {Lager_Haup} or (Fertigung.Lagerort_id={Lager_Fer}) {Lager_fer2} )   /* & Lager_Fer &  & Lager_fer1 & */
								 AND
								 ((Fertigung.Kennzeichen)='OFFEN')
								 ))))
								  ORDER BY T.Artikelnummer;
										
										";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 70;
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.BestellungohneFAEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.BestellungohneFAEntity>();
			}
		}

		/// <summary>
		/// it returns all the data if all is 1 or it add the pagination filter if all is 0 (the sql connection Timeout is unbounded)
		/// </summary>
		/// <param name="productionSort"></param>
		/// <param name="paging"></param>
		/// <param name="all"></param>
		/// <returns></returns>
		public static List<Entities.Joins.MTM.Order.Statistics.GetArtikelStatisticsEntity> GetArtikel_Statistik(int productionSort, Settings.PaginModel paging = null, int all = 0)
		{
			var dataTable = new DataTable();
			string paginationFilter = "";
			if(all == 1)
			{
				paginationFilter = "";
			}
			if(paging is not null && all == 0)
			{
				paginationFilter = $"OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY";
			}

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query6 =
						   $@"
									 SELECT 
								   View_PSZ_Auswertung_EK_Bestand_CZ.Artikelnummer
									, View_PSZ_Auswertung_EK_Bestand_CZ.[Bezeichnung 1]
									, View_PSZ_Auswertung_EK_Bestand_CZ.EK
									, View_PSZ_Auswertung_EK_Bestand_CZ.Bestand
									, View_PSZ_Auswertung_EK_Bestand_CZ.Sicherheitsbestand
									, View_PSZ_Auswertung_EK_Bestand_CZ.Lagerort
									, View_PSZ_Auswertung_EK_Bestand_CZ.[Bedarf +1Mo]
									, View_PSZ_Auswertung_EK_Bestand_CZ.[Gesamtbedarf max  1 Jahr]
									, View_PSZ_Auswertung_EK_Bestand_CZ.[off Best]
									, View_PSZ_Auswertung_EK_Bestand_CZ.[Entnahme der letzen 12 monate]
									,count(*) over() TotalCount
									FROM View_PSZ_Auswertung_EK_Bestand_CZ
									ORDER BY View_PSZ_Auswertung_EK_Bestand_CZ.Artikelnummer
										
										";
				string query60 =
							   $@"
								  SELECT 
								View_PSZ_Auswertung_EK_Bestand_BETN.Artikelnummer
								, View_PSZ_Auswertung_EK_Bestand_BETN.[Bezeichnung 1]
								, View_PSZ_Auswertung_EK_Bestand_BETN.EK
								, View_PSZ_Auswertung_EK_Bestand_BETN.Bestand
								, View_PSZ_Auswertung_EK_Bestand_BETN.Sicherheitsbestand
								, View_PSZ_Auswertung_EK_Bestand_BETN.Lagerort
								, View_PSZ_Auswertung_EK_Bestand_BETN.[Bedarf +1Mo]
								, View_PSZ_Auswertung_EK_Bestand_BETN.[Gesamtbedarf max 1 Jahr] [Gesamtbedarf max  1 Jahr]
								, View_PSZ_Auswertung_EK_Bestand_BETN.[off Best]
								, View_PSZ_Auswertung_EK_Bestand_BETN.[Entnahme der letzen 12 monate]
								,count(*) over() TotalCount
								FROM View_PSZ_Auswertung_EK_Bestand_BETN
								ORDER BY View_PSZ_Auswertung_EK_Bestand_BETN.Artikelnummer
									
										";
				string query7 =
							   $@"
														SELECT 
							View_PSZ_Auswertung_EK_Bestand_TN.Artikelnummer
							, View_PSZ_Auswertung_EK_Bestand_TN.[Bezeichnung 1]
							, View_PSZ_Auswertung_EK_Bestand_TN.EK
							, View_PSZ_Auswertung_EK_Bestand_TN.Bestand
							, View_PSZ_Auswertung_EK_Bestand_TN.Sicherheitsbestand
							, View_PSZ_Auswertung_EK_Bestand_TN.Lagerort
							, View_PSZ_Auswertung_EK_Bestand_TN.[Bedarf +1Mo]
							, View_PSZ_Auswertung_EK_Bestand_TN.[Gesamtbedarf max 1 Jahr] [Gesamtbedarf max  1 Jahr]
							, View_PSZ_Auswertung_EK_Bestand_TN.[off Best]
							, View_PSZ_Auswertung_EK_Bestand_TN.[Entnahme der letzen 12 monate]
							,count(*) over() TotalCount
							FROM View_PSZ_Auswertung_EK_Bestand_TN
							ORDER BY View_PSZ_Auswertung_EK_Bestand_TN.Artikelnummer ;
										";
				string query42 =
							   $@"
									SELECT 
								View_PSZ_Auswertung_EK_Bestand_WS.Artikelnummer
								, View_PSZ_Auswertung_EK_Bestand_WS.[Bezeichnung 1]
								, View_PSZ_Auswertung_EK_Bestand_WS.EK
								, View_PSZ_Auswertung_EK_Bestand_WS.Bestand
								, View_PSZ_Auswertung_EK_Bestand_WS.Sicherheitsbestand
								, View_PSZ_Auswertung_EK_Bestand_WS.Lagerort
								, View_PSZ_Auswertung_EK_Bestand_WS.[Bedarf +1Mo]
								, View_PSZ_Auswertung_EK_Bestand_WS.[Gesamtbedarf max 1 Jahr] [Gesamtbedarf max  1 Jahr]
								, View_PSZ_Auswertung_EK_Bestand_WS.[off Best]
								, View_PSZ_Auswertung_EK_Bestand_WS.[Entnahme der letzen 12 monate]
								,count(*) over() TotalCount
								FROM View_PSZ_Auswertung_EK_Bestand_WS
								ORDER BY View_PSZ_Auswertung_EK_Bestand_WS.Artikelnummer
								
										";
				string query26 =
						   $@"
									SELECT 
								View_PSZ_Auswertung_EK_Bestand_AL.Artikelnummer
								, View_PSZ_Auswertung_EK_Bestand_AL.[Bezeichnung 1]
								, View_PSZ_Auswertung_EK_Bestand_AL.EK
								, View_PSZ_Auswertung_EK_Bestand_AL.Bestand
								, View_PSZ_Auswertung_EK_Bestand_AL.Sicherheitsbestand
								, View_PSZ_Auswertung_EK_Bestand_AL.Lagerort
								, View_PSZ_Auswertung_EK_Bestand_AL.[Bedarf +1Mo]
								, View_PSZ_Auswertung_EK_Bestand_AL.[Gesamtbedarf max 1 Jahr] [Gesamtbedarf max  1 Jahr]
								, View_PSZ_Auswertung_EK_Bestand_AL.[off Best]
								, View_PSZ_Auswertung_EK_Bestand_AL.[Entnahme der letzen 12 monate]
								,count(*) over() TotalCount
								FROM View_PSZ_Auswertung_EK_Bestand_AL
								order by View_PSZ_Auswertung_EK_Bestand_AL.Artikelnummer
								;
										";
				string query15 =
							   $@"
								 SELECT 
								View_PSZ_Auswertung_EK_Bestand_DE.Artikelnummer
								, View_PSZ_Auswertung_EK_Bestand_DE.[Bezeichnung 1]
								, View_PSZ_Auswertung_EK_Bestand_DE.EK
								, View_PSZ_Auswertung_EK_Bestand_DE.Bestand
								, View_PSZ_Auswertung_EK_Bestand_DE.Sicherheitsbestand
								, View_PSZ_Auswertung_EK_Bestand_DE.Lagerort
								, View_PSZ_Auswertung_EK_Bestand_DE.[Bedarf +1Mo]
								, View_PSZ_Auswertung_EK_Bestand_DE.[Gesamtbedarf max 1 Jahr] [Gesamtbedarf max  1 Jahr]
								, View_PSZ_Auswertung_EK_Bestand_DE.[off Best]
								, View_PSZ_Auswertung_EK_Bestand_DE.[Entnahme der letzen 12 monate]
								,count(*) over() TotalCount
								FROM View_PSZ_Auswertung_EK_Bestand_DE
								ORDER BY View_PSZ_Auswertung_EK_Bestand_DE.Artikelnummer 
								;
										";
				string query = productionSort switch
				{
					6 => query6,
					60 => query60,
					7 => query7,
					42 => query42,
					26 => query26,
					15 => query15,
					_ => query6

				};
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 0;
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.GetArtikelStatisticsEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.GetArtikelStatisticsEntity>();
			}
		}


		public static List<Entities.Joins.MTM.Order.Statistics.BestandProWerkohneBedarfEntity> GetBestandProWerkohneBedarf(int Lager, int lagerPr, int lagerEX01, int LagerEX02, int LagerEX03, int LagerEX04, int lagerEX05, int lagerEX06)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				//paginationFilter
				string query =
						$@"
							select 
					 isnull(T0.Artikelnummer,'')as Artikelnummer
					 ,round(isnull(T0.Bestand,0),4) as Bestand
					 ,round(isnull(T2.Reserviert,0),4) as BedarfTN
					 ,round(isnull(T3.Reserviert,0),4) as BedarfBETN
					 ,round(isnull(T4.Reserviert,0),4) as BedarfWS
					 ,round(isnull(T5.Reserviert,0),4) as BedarfAL
					 ,round(isnull(T6.Reserviert,0),4) as BedarfDE   
					,round(isnull(T7.Reserviert,0),4) as BedarfGZTN
					 from

					(select T.Artikelnummer,Round(T.Bestand,4)as Bestand from
					(
					 select A.Artikelnummer,L.Bestand from Lager L inner join Artikel A on A.[Artikel-Nr]=L.[Artikel-Nr]
					 where A.Warentyp=1 and L.Lagerort_id= {Lager} /*& Lager &*/  and L.bestand<>0
					 and A.Warengruppe<>'EF'
					 ) T
					 Left Join
					 (
					  select 
					  P.Artikel_Nr as Artikel_Nr
					  ,A.artikelnummer
					  , sum(P.Anzahl/F.Originalanzahl*F.Anzahl) as Reserviert from fertigung F 
					  inner join Fertigung_Positionen P 
					  on P.ID_Fertigung=F.id
					  inner join Artikel A on A.[Artikel-Nr]=P.[Artikel_Nr]
					  where F.Kennzeichen='offen' and F.Lagerort_id= {Lager} --& Lager
					  group by  P.Artikel_Nr , Artikelnummer
					  )T1 on T.Artikelnummer=T1.Artikelnummer
					  where T1.Artikelnummer Is Null
					  Union all
					 select T.Artikelnummer,T.Bestand from
					 (
					  select A.artikelnummer,isnull(T1.Bestand,0)+isnull(T2.BestandPR,0) as Bestand from Artikel A left join
					  (
					  select A.Artikelnummer,L.Bestand from Lager L inner join Artikel A on A.[Artikel-Nr]=L.[Artikel-Nr]
					  where A.Warentyp=2 and L.Lagerort_id= {Lager} /*& Lager &*/  and L.bestand<>0
					  and A.Warengruppe<>'EF'
					  )T1 on T1.Artikelnummer=A.Artikelnummer
					  Left Join
					 (
					  select A.Artikelnummer
					  ,round(isnull(L.Bestand,0)+isnull(L.Bestand_reserviert,0),4) as BestandPR from Lager L 
					  inner join Artikel A on A.[Artikel-Nr]=L.[Artikel-Nr]
					  where A.Warentyp=2 and  Lagerort_id={lagerPr} /*& lagerPr &*/  and round(isnull(L.Bestand,0)+isnull(L.Bestand_reserviert,0),4)<>0
					  and A.Warengruppe<>'EF'
					  )T2 on T2.Artikelnummer=A.Artikelnummer
					  where Round(IsNull(T1.Bestand, 0) + IsNull(T2.BestandPR, 0), 4) <> 0
					  ) T
					  Left Join
					  (
					  select P.Artikel_Nr as Artikel_Nr,A.artikelnummer
					  , sum(P.Anzahl/F.Originalanzahl*F.Anzahl) as Reserviert from fertigung F 
					  inner join Fertigung_Positionen P 
					  on P.ID_Fertigung=F.id
					  inner join Artikel A on A.[Artikel-Nr]=P.[Artikel_Nr]
					  where F.Kennzeichen='offen' and F.Lagerort_id= {Lager} --& Lager
					  group by  P.Artikel_Nr , Artikelnummer
					  )T1 on T.Artikelnummer=T1.Artikelnummer
					  where T1.Artikelnummer Is Null
					  )T0

					  Left Join
					  (
					  select P.Artikel_Nr as Artikel_Nr,A.artikelnummer, sum(P.Anzahl/F.Originalanzahl*F.Anzahl) as Reserviert 
					  from fertigung F inner join Fertigung_Positionen P on P.ID_Fertigung=F.id
					  inner join Artikel A on A.[Artikel-Nr]=P.[Artikel_Nr]
					  where F.Kennzeichen='offen' and F.Lagerort_id= {lagerEX01} --& lagerEX01
					  group by  P.Artikel_Nr , Artikelnummer
					  )T2 on T0.Artikelnummer=T2.Artikelnummer
					  Left Join
					  (
					  select P.Artikel_Nr as Artikel_Nr
					  ,A.artikelnummer, sum(P.Anzahl/F.Originalanzahl*F.Anzahl) as Reserviert 
					  from fertigung F 
					  inner join Fertigung_Positionen P on P.ID_Fertigung=F.id
					  inner join Artikel A on A.[Artikel-Nr]=P.[Artikel_Nr]
					  where F.Kennzeichen='offen' and F.Lagerort_id= {LagerEX02} --& LagerEX02
					  group by  P.Artikel_Nr , Artikelnummer
					  )T3 on T0.Artikelnummer=T3.Artikelnummer
					  Left Join
						( 
					  select P.Artikel_Nr as Artikel_Nr,A.artikelnummer, sum(P.Anzahl/F.Originalanzahl*F.Anzahl) as Reserviert 
					  from fertigung F inner join Fertigung_Positionen P on P.ID_Fertigung=F.id
					  inner join Artikel A on A.[Artikel-Nr]=P.[Artikel_Nr]
					  where F.Kennzeichen='offen' and F.Lagerort_id={lagerEX06} -- & LagerEX04
					 group by  P.Artikel_Nr , Artikelnummer
					  )T7 on T0.Artikelnummer=T7.Artikelnummer
					  left join
					  (
					  select P.Artikel_Nr as Artikel_Nr,A.artikelnummer, sum(P.Anzahl/F.Originalanzahl*F.Anzahl) as Reserviert 
					  from fertigung F inner join Fertigung_Positionen P on P.ID_Fertigung=F.id
					  inner join Artikel A on A.[Artikel-Nr]=P.[Artikel_Nr]
					  where F.Kennzeichen='offen' and F.Lagerort_id= {LagerEX03} --& LagerEX03
					  group by  P.Artikel_Nr , Artikelnummer
					  )T4 on T0.Artikelnummer=T4.Artikelnummer
					  Left Join
					  (
					  select P.Artikel_Nr as Artikel_Nr,A.artikelnummer, sum(P.Anzahl/F.Originalanzahl*F.Anzahl) as Reserviert 
					  from fertigung F inner join Fertigung_Positionen P on P.ID_Fertigung=F.id
					  inner join Artikel A on A.[Artikel-Nr]=P.[Artikel_Nr]
					  where F.Kennzeichen='offen' and F.Lagerort_id={LagerEX04} -- & LagerEX04
					 group by  P.Artikel_Nr , Artikelnummer
					  )T5 on T0.Artikelnummer=T5.Artikelnummer
					  left join
					 (
					  select P.Artikel_Nr as Artikel_Nr,A.artikelnummer
					  , sum(P.Anzahl/F.Originalanzahl*F.Anzahl) as Reserviert
					  from fertigung F 
					  inner join Fertigung_Positionen P on P.ID_Fertigung=F.id
					  inner join Artikel A on A.[Artikel-Nr]=P.[Artikel_Nr]
					  where F.Kennzeichen='offen' and F.Lagerort_id={lagerEX05}-- & lagerEX05
					  group by  P.Artikel_Nr , Artikelnummer
					  )T6 on T0.Artikelnummer=T6.Artikelnummer
					  order by T0.Artikelnummer
										";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 0;
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.BestandProWerkohneBedarfEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.Order.Statistics.BestandProWerkohneBedarfEntity>();
			}
		}

		public static List<Entities.Joins.MTM.Order.Statistics.StdSupplierViolationEntity> GetStdSupplierViolation(DateTime start)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query =
						$@"
						SELECT 
							b.[Bestellung-Nr], 
							cast(b.Datum as date) Datum, 
							p.Position, 
							a.Artikelnummer, 
							p.[Start Anzahl], 
							b.[Vorname/NameFirma] Lieferant, 
							d.Name1 Standardlieferant,
							p.Einzelpreis [EK Price Second Source], 
							n.Einkaufspreis [Preis von Standardlieferant],
							p.[Start Anzahl] * p.Einzelpreis [Total Second Source], 
							p.[Start Anzahl] * n.Einkaufspreis [Total von Standardlieferant],
							p.[Start Anzahl] * p.Einzelpreis - p.[Start Anzahl] * n.Einkaufspreis [Diff]
						FROM Bestellungen b
						JOIN [bestellte Artikel] p on p.[Bestellung-Nr]=b.Nr
						JOIN (SELECT DISTINCT [Artikel-Nr], [Lieferanten-Nr], Einkaufspreis FROM Bestellnummern WHERE ISNULL(Standardlieferant,0)=1) n on n.[Artikel-Nr]=p.[Artikel-Nr]
						LEFT JOIN Artikel a on a.[Artikel-Nr]=p.[Artikel-Nr]
						LEFT JOIN adressen d on d.Nr=n.[Lieferanten-Nr]
						WHERE b.Typ='bestellung' 
						and YEAR(b.Datum)=Year(GETDATE()) 
						and MONTH(b.Datum)>=MONTH(DATEADD(MONTH, -2, GETDATE()))
						and b.[Lieferanten-Nr]<>n.[Lieferanten-Nr]
						and p.Einzelpreis > n.Einkaufspreis
						ORDER BY b.Datum";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 70;
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.Order.Statistics.StdSupplierViolationEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
	}
}
