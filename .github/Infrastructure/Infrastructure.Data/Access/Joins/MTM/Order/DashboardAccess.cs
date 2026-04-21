using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Joins.MTM.Order
{
	public class DashboardAccess
	{
		public static List<Infrastructure.Data.Entities.Joins.MTM.Order.DashboardEntity> GetByRahmenStatus(List<int> ids, DateTime maxExpiryDate)
		{
			if(ids == null || ids.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT e.*, ISNULL(
				CASE 
					WHEN e.GultigBis<GETDATE() OR ap.Anzahl<e.Zielmenge*0.75 THEN 1 /* red */
					WHEN ap.Anzahl>e.Zielmenge*0.75 AND ap.Erhalten<e.Zielmenge*0.75 THEN 2 /* orange */
					WHEN ap.Erhalten>=e.Zielmenge*0.75 THEN 3 /* green */
					END,0) [RahmenStatus]  
				FROM [__CTS_AngeboteArticleBlanketExtension] e
				LEFT JOIN (SELECT [RA Pos zu Bestellposition], ISNULL(SUM(ISNULL(anzahl,0)),0) Anzahl,ISNULL(SUM(ISNULL([Erhalten],0)),0) Erhalten
					FROM [bestellte Artikel] p join Bestellungen b on b.Nr=p.[Bestellung-Nr] 
					WHERE ISNULL([RA Pos zu Bestellposition],0)<>0 AND b.Typ='bestellung' 
					GROUP BY [RA Pos zu Bestellposition]) ap on ap.[RA Pos zu Bestellposition]=e.AngeboteArtikelNr
				WHERE [AngeboteArtikelNr] IN ({string.Join(",", ids)}) AND [GultigBis] IS NOT NULL AND [GultigBis]<=@maxExpiryDate";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("maxExpiryDate", maxExpiryDate);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MTM.Order.DashboardEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MTM.Order.DashboardEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.MTM.Order.OrdersPrioViewEntity> GetOrdersPrioView(Settings.SortingModel sorting,
			Settings.PaginModel paging, int? supplierNr, string type, int? orderNumber, string user, int? artikelNr,
			string projectNumber, string from, string to, bool draftOnly, bool wareneingang_ready_only, bool purchase_project_only, bool includeDone)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select b.Nr,b.[Bestellung-Nr],b.[Projekt-Nr],b.Typ,b.[Vorname/NameFirma],b.Datum,
								b.Konditionen,u.[Name],b.[gebucht],ba.Position,ba.[Artikel-Nr],a.Artikelnummer,ba.Liefertermin,ba.Bestätigter_Termin,
								l.Lagerort,b.[ProjectPurchase],ba.StandardSupplierViolation
								from Bestellungen b inner join [bestellte Artikel] ba 
								on b.Nr=ba.[Bestellung-Nr]
								inner join Artikel a on a.[Artikel-Nr]=ba.[Artikel-Nr]
								inner join lagerorte l on l.Lagerort_id=ba.Lagerort_id
								inner join [User] u on u.Id=b.Bearbeiter
                                WHERE (ba.Bestätigter_Termin IS NULL OR ba.Bestätigter_Termin >= '29991231')";

				if(supplierNr.HasValue)
				{
					query += $" AND b.[Lieferanten-Nr]={supplierNr}";
				}
				if(!type.StringIsNullOrEmptyOrWhiteSpaces())
				{
					query += $"AND b.[Typ]='{type}'";
				}
				if(orderNumber.HasValue)
				{
					query += $"AND b.[Bestellung-Nr]={orderNumber}";
				}
				if(!user.StringIsNullOrEmptyOrWhiteSpaces())
				{
					query += @$"AND (u.[Name] LIKE '%{user}%' OR u.[Username] LIKE '%{user}%' 
                    OR u.[LegacyUserName] LIKE '%{user}%') AND u.[LegacyUserName] is not null AND u.[LegacyUserName] <> '')";
				}
				if(artikelNr.HasValue)
				{
					query += $" AND ba.[Artikel-Nr]={artikelNr}";
				}
				if(!projectNumber.StringIsNullOrEmptyOrWhiteSpaces())
				{
					query += $" AND b.[Projekt-Nr]='{projectNumber}'";
				}
				if(!from.StringIsNullOrEmptyOrWhiteSpaces())
				{
					if(!to.StringIsNullOrEmptyOrWhiteSpaces())
					{
						query += $" AND b.[Datum] BETWEEN '{from}' AND '{to}'";
					}
					else
					{
						query += $" AND b.[Datum] >= @from";
					}
				}
				if(draftOnly)
				{
					query += $" AND b.[gebucht]=0";
				}
				if(wareneingang_ready_only)
				{
					query += @$" AND ba.erledigt_pos=0 AND b.erledigt=0 AND b.gebucht=1 AND b.[Rahmenbestellung]=1 AND b.[typ]='Rahmenbestellung'";
				}
				if(purchase_project_only)
				{
					query += $" AND b.[ProjectPurchase]=1";
				}
				if(includeDone)
					query += " AND b.[erledigt]=0";

				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY ba.Liefertermin DESC";
				}

				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MTM.Order.OrdersPrioViewEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MTM.Order.OrdersPrioViewEntity>();
			}
		}
		public static int GetOrdersPrioViewCount(int? supplierNr, string type, int? orderNumber, string user, int? artikelNr,
			string projectNumber, string from, string to, bool draftOnly, bool wareneingang_ready_only, bool purchase_project_only, bool includeDone)
		{
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select count(*) from ( 
                                select b.Nr,b.[Bestellung-Nr],b.[Projekt-Nr],b.Typ,b.[Vorname/NameFirma],b.Datum,
								b.Konditionen,u.[Name],b.[gebucht],ba.Position,ba.[Artikel-Nr],a.Artikelnummer,ba.Liefertermin,ba.Bestätigter_Termin,
								l.Lagerort,b.[ProjectPurchase],ba.StandardSupplierViolation
								from Bestellungen b inner join [bestellte Artikel] ba 
								on b.Nr=ba.[Bestellung-Nr]
								inner join Artikel a on a.[Artikel-Nr]=ba.[Artikel-Nr]
								inner join lagerorte l on l.Lagerort_id=ba.Lagerort_id
								inner join [User] u on u.Id=b.Bearbeiter
                                WHERE (ba.Bestätigter_Termin IS NULL OR ba.Bestätigter_Termin >= '29991231')";
				using(var sqlCommand = new SqlCommand())
				{
					if(supplierNr.HasValue)
					{
						query += $" AND b.[Lieferanten-Nr]={supplierNr}";
					}
					if(!type.StringIsNullOrEmptyOrWhiteSpaces())
					{
						query += $" AND b.[Typ]='{type}'";
					}
					if(orderNumber.HasValue)
					{
						query += $" AND b.[Bestellung-Nr]={orderNumber}";
					}
					if(!user.StringIsNullOrEmptyOrWhiteSpaces())
					{
						query += @$" AND (u.[Name] LIKE '%{user}%' OR u.[Username] LIKE '%{user}%' 
                    OR u.[LegacyUserName] LIKE '%{user}%') AND u.[LegacyUserName] is not null AND u.[LegacyUserName] <> '')";
					}
					if(artikelNr.HasValue)
					{
						query += $" AND ba.[Artikel-Nr]={artikelNr}";
					}
					if(!projectNumber.StringIsNullOrEmptyOrWhiteSpaces())
					{
						query += $" AND b.[Projekt-Nr]='{projectNumber}'";
					}
					if(!from.StringIsNullOrEmptyOrWhiteSpaces())
					{
						if(!to.StringIsNullOrEmptyOrWhiteSpaces())
						{
							query += $" AND b.[Datum] BETWEEN '{from}' AND '{to}'";
						}
						else
						{
							query += $" AND b.[Datum] >= @from";
						}
					}
					if(draftOnly)
					{
						query += $" AND b.[gebucht]=0";
					}
					if(wareneingang_ready_only)
					{
						query += @$" AND ba.erledigt_pos=0 AND b.erledigt=0 AND b.gebucht=1 AND b.[Rahmenbestellung]=1 AND b.[typ]='Rahmenbestellung'";
					}
					if(purchase_project_only)
					{
						query += $"AND b.[ProjectPurchase]=1";
					}
					if(includeDone)
						query += " AND b.[erledigt]=0";
					query += ") subquery";
					sqlCommand.CommandText = query;
					sqlCommand.Connection = sqlConnection;
					return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out int count) ? count : 0;
				}
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.MTM.Order.OrdersPrioViewEntity> GetOrdersAnomalies(string searchText, Settings.SortingModel sorting,
			Settings.PaginModel paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select b.Nr,b.[Bestellung-Nr],b.[Projekt-Nr],b.Typ,b.[Vorname/NameFirma],b.Datum,
								b.Konditionen,u.[Name],b.[gebucht],ba.Position,ba.[Artikel-Nr],a.Artikelnummer,ba.Liefertermin,ba.Bestätigter_Termin,
								l.Lagerort,b.[ProjectPurchase],ba.StandardSupplierViolation
								from Bestellungen b inner join [bestellte Artikel] ba 
								on b.Nr=ba.[Bestellung-Nr]
								inner join Artikel a on a.[Artikel-Nr]=ba.[Artikel-Nr]
								inner join lagerorte l on l.Lagerort_id=ba.Lagerort_id
								inner join [User] u on u.Id=b.Bearbeiter
								where (YEAR(ba.Bestätigter_Termin)>YEAR(ba.Liefertermin) OR 
								(YEAR(ba.Bestätigter_Termin)=YEAR(ba.Liefertermin) AND DATEPART(ISO_WEEK, ba.Bestätigter_Termin)>DATEPART(ISO_WEEK, ba.Liefertermin)))
								AND ba.Bestätigter_Termin<>'29991231' AND ba.Bestätigter_Termin IS NOT NULL
                                AND b.Typ='Bestellung'";
				if(!searchText.StringIsNullOrEmptyOrWhiteSpaces())
				{
					query += @$" AND (b.[Bestellung-Nr] LIKE '%{searchText}%' OR b.[Projekt-Nr] LIKE '%{searchText}%' OR a.Artikelnummer LIKE '%{searchText}%' OR b.[Vorname/NameFirma] LIKE '%{searchText}%')";
				}
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY ba.Bestätigter_Termin DESC";
				}

				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MTM.Order.OrdersPrioViewEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MTM.Order.OrdersPrioViewEntity>();
			}
		}
		public static int GetOrdersAnomaliesCount(string searchText)
		{
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select count(*) from ( 
                                select b.Nr,b.[Bestellung-Nr],b.[Projekt-Nr],b.Typ,b.[Vorname/NameFirma],b.Datum,
								b.Konditionen,u.[Name],b.[gebucht],ba.Position,ba.[Artikel-Nr],a.Artikelnummer,ba.Liefertermin,ba.Bestätigter_Termin,
								l.Lagerort,b.[ProjectPurchase],ba.StandardSupplierViolation
								from Bestellungen b inner join [bestellte Artikel] ba 
								on b.Nr=ba.[Bestellung-Nr]
								inner join Artikel a on a.[Artikel-Nr]=ba.[Artikel-Nr]
								inner join lagerorte l on l.Lagerort_id=ba.Lagerort_id
								inner join [User] u on u.Id=b.Bearbeiter
								where (YEAR(ba.Bestätigter_Termin)>YEAR(ba.Liefertermin) OR 
								(YEAR(ba.Bestätigter_Termin)=YEAR(ba.Liefertermin) AND DATEPART(ISO_WEEK, ba.Bestätigter_Termin)>DATEPART(ISO_WEEK, ba.Liefertermin)))
								AND ba.Bestätigter_Termin<>'29991231' AND ba.Bestätigter_Termin IS NOT NULL
                                AND b.Typ='Bestellung'";
				if(!searchText.StringIsNullOrEmptyOrWhiteSpaces())
				{
					query += @$" AND (b.[Bestellung-Nr] LIKE '%{searchText}%' OR b.[Projekt-Nr] LIKE '%{searchText}%' OR a.Artikelnummer LIKE '%{searchText}%' OR b.[Vorname/NameFirma] LIKE '%{searchText}%')";
				}
				query += " ) subquery ";
				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = query;
					sqlCommand.Connection = sqlConnection;
					return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out int count) ? count : 0;
				}
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.MTM.Order.RahmensPositionsConsumptionEntity> GetRahmenPositionsConsumption(int? ArtikelNr, string projectNr, int? vorfallNr, string documentNumber,
			List<int> supplierIds, bool onlyExpired, int? status, Settings.SortingModel sorting, Settings.PaginModel paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select a.[Vorname/NameFirma] Supplier, a.[Bezug] DocumentNumber, a.Nr,aa.Position,a.[Angebot-Nr],a.[Projekt-Nr],aa.[Artikel-Nr],ar.Artikelnummer,aa.Bezeichnung1,aa.OriginalAnzahl,aa.Geliefert,aa.Anzahl,
								ISNULL(SUM(n.NeededInBOM),0) NeededInBOM, ISNULL(SUM(n.SumNeeded),0) SumNeeded,
                                ISNULL(SUM(n.NeededInBOM),0)*ISNULL(SUM(n.SumNeeded),0) Total,
								case
								when aa.OriginalAnzahl>0 and aa.OriginalAnzahl is not null then (aa.Geliefert/aa.OriginalAnzahl)*100
								else 0
								end
								as Consumption,bea.GesamtpreisDefault,bea.GultigBis,bea.ExtensionDate,be.StatusName,be.StatusId, a.[Kunden-Nr] AS SupplierId
								from __CTS_AngeboteBlanketExtension be
								left join __CTS_AngeboteArticleBlanketExtension bea on bea.RahmenNr=be.AngeboteNr
								left join [angebotene Artikel] aa on aa.Nr=bea.AngeboteArtikelNr
								left join Angebote a on a.Nr=be.AngeboteNr
								left join Artikel ar on ar.[Artikel-Nr]=aa.[Artikel-Nr]
								left join __PRS_RA_ROH_Needs n on n.ROH=aa.[Artikel-Nr]
								WHERE a.typ='Rahmenauftrag' AND be.BlanketTypeId=1";

				if(ArtikelNr is not null)
					query += @$" AND ar.[Artikel-Nr] = {ArtikelNr}";

				if(!projectNr.StringIsNullOrEmptyOrWhiteSpaces())
					query += $" AND a.[Projekt-Nr]='{projectNr}'";

				if(vorfallNr is not null)
					query += $" AND a.[Angebot-Nr]={vorfallNr}";

				if(!documentNumber.StringIsNullOrEmptyOrWhiteSpaces())
					query += $" AND a.[Bezug]='{documentNumber}'";

				if(supplierIds != null && supplierIds.Count > 0)
					query += $" AND a.[Kunden-Nr] in ({string.Join(",", supplierIds)})";

				if(onlyExpired)
					query += " AND GETDATE()>bea.ExtensionDate AND be.StatusId=2";

				if(status is not null)
					query += $" AND be.StatusId={status}";

				query += $@" GROUP BY a.[Vorname/NameFirma], a.[Bezug],a.Nr,aa.Position,a.[Angebot-Nr],a.[Projekt-Nr],aa.[Artikel-Nr],ar.Artikelnummer,aa.Bezeichnung1,aa.OriginalAnzahl,aa.Geliefert,aa.Anzahl, a.[Kunden-Nr],
						case
						when aa.OriginalAnzahl>0 and aa.OriginalAnzahl is not null then (aa.Geliefert/aa.OriginalAnzahl)*100
						else 0
						end,
						bea.GesamtpreisDefault,bea.GultigBis,bea.ExtensionDate,be.StatusName,be.StatusId";

				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY bea.ExtensionDate DESC ";
				}

				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MTM.Order.RahmensPositionsConsumptionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MTM.Order.RahmensPositionsConsumptionEntity>();
			}
		}
		public static int GetRahmenPositionsConsumptionCount(int? ArtikelNr, string projectNr, int? vorfallNr, string documentNumber, List<int> supplierIds, bool onlyExpired, int? status)
		{
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select count(*) from ( 
                                select a.[Vorname/NameFirma], a.[Bezug], a.Nr,aa.Position,a.[Angebot-Nr],a.[Projekt-Nr],aa.[Artikel-Nr],ar.Artikelnummer,aa.Bezeichnung1,aa.OriginalAnzahl,aa.Geliefert,aa.Anzahl,
								ISNULL(SUM(n.NeededInBOM),0) NeededInBOM, ISNULL(SUM(n.SumNeeded),0) SumNeeded,
                                ISNULL(SUM(n.NeededInBOM),0)*ISNULL(SUM(n.SumNeeded),0) Total,
								case
								when aa.OriginalAnzahl>0 and aa.OriginalAnzahl is not null then (aa.Geliefert/aa.OriginalAnzahl)*100
								else 0
								end
								as Consumption,bea.GesamtpreisDefault,bea.GultigBis,bea.ExtensionDate,be.StatusName,be.StatusId
								from __CTS_AngeboteBlanketExtension be
								left join __CTS_AngeboteArticleBlanketExtension bea on bea.RahmenNr=be.AngeboteNr
								left join [angebotene Artikel] aa on aa.Nr=bea.AngeboteArtikelNr
								left join Angebote a on a.Nr=be.AngeboteNr
								left join Artikel ar on ar.[Artikel-Nr]=aa.[Artikel-Nr]
								left join __PRS_RA_ROH_Needs n on n.ROH=aa.[Artikel-Nr]
								WHERE a.typ='Rahmenauftrag' AND be.BlanketTypeId=1";
				if(ArtikelNr is not null)
					query += @$" AND ar.[Artikel-Nr] = {ArtikelNr}";

				if(!projectNr.StringIsNullOrEmptyOrWhiteSpaces())
					query += $" AND a.[Projekt-Nr]='{projectNr}'";

				if(vorfallNr is not null)
					query += $" AND a.[Angebot-Nr]={vorfallNr}";

				if(!documentNumber.StringIsNullOrEmptyOrWhiteSpaces())
					query += $" AND a.[Bezug]='{documentNumber}'";

				if(supplierIds != null && supplierIds.Count > 0)
					query += $" AND a.[Kunden-Nr] in ({string.Join(",", supplierIds)})";

				if(onlyExpired)
					query += " AND GETDATE()>bea.ExtensionDate AND be.StatusId=2";

				if(status is not null)
					query += $" AND be.StatusId={status}";

				query += $@" GROUP BY a.[Vorname/NameFirma], a.[Bezug],a.Nr,aa.Position,a.[Angebot-Nr],a.[Projekt-Nr],aa.[Artikel-Nr],ar.Artikelnummer,aa.Bezeichnung1,aa.OriginalAnzahl,aa.Geliefert,aa.Anzahl,
						case
						when aa.OriginalAnzahl>0 and aa.OriginalAnzahl is not null then (aa.Geliefert/aa.OriginalAnzahl)*100
						else 0
						end,
						bea.GesamtpreisDefault,bea.GultigBis,bea.ExtensionDate,be.StatusName,be.StatusId";

				query += " ) subquery";
				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = query;
					sqlCommand.Connection = sqlConnection;
					return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out int count) ? count : 0;
				}
			}
		}

		public static List<int> GetFGThatNeedsROHArticle(int artikelNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select distinct f.Artikel_Nr from Fertigung_Positionen fp inner join Fertigung f
								on fp.ID_Fertigung=f.ID
								where fp.Artikel_Nr=@artikelNr AND f.Kennzeichen=N'offen' 
								AND (f.FA_Gestartet=0 OR f.FA_Gestartet IS NULL)";


				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("artikelNr", artikelNr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => Convert.ToInt32(x["Artikel_Nr"])).ToList();
			}
			else
			{
				return null;
			}
		}
		public static decimal GetRahmenPurchaseNeeds(int artikelNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select SUM(ar.Anzahl) 
								from __CTS_AngeboteArticleBlanketExtension be inner join [angebotene Artikel] ar ON
                                ar.nr=be.AngeboteArtikelNr
								inner join Angebote a on a.Nr=ar.[Angebot-Nr]
                                inner join __CTS_AngeboteBlanketExtension b on b.AngeboteNr=a.Nr
								where be.MaterialNr=@artikelNr AND b.BlanketTypeId=1 and b.StatusId=2";


				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("artikelNr", artikelNr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return decimal.TryParse(dataTable.Rows[0][0].ToString(), out var _x) ? _x : 0;
			}
			else
			{
				return 0;
			}
		}
		public static decimal GetRahmenSaleNeeds(List<int> artikelNrs, int artikelNr)
		{
			if(artikelNrs is null || artikelNrs.Count == 0)
				return 0;
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select sum(SumNeeded) from (
								select a.Nr,a.[Angebot-Nr], aa.[Artikel-Nr],ar.Artikelnummer,aa.[Position],aa.Anzahl AS RestQty,
								s.Anzahl as NeededInBOM,aa.Anzahl*s.Anzahl as SumNeeded,a.Bezug,be.ExtensionDate
								from Angebote a 
								inner join [angebotene Artikel] aa on a.Nr=aa.[Angebot-Nr]
								inner join __CTS_AngeboteBlanketExtension b on b.AngeboteNr=a.Nr
								inner join __CTS_AngeboteArticleBlanketExtension be on be.AngeboteArtikelNr=aa.Nr
								inner join Stücklisten s on s.[Artikel-Nr]=aa.[Artikel-Nr] and s.[Artikel-Nr des Bauteils]=@artikelNr
								inner join Artikel ar on ar.[Artikel-Nr]=aa.[Artikel-Nr]
								where a.Typ='Rahmenauftrag' and b.StatusId=2 and aa.Anzahl>0
								AND aa.[Artikel-Nr] IN ({string.Join(",", artikelNrs)})
								) as subquery";


				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("artikelNr", artikelNr);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return decimal.TryParse(dataTable.Rows[0][0].ToString(), out var _x) ? _x : 0;

			}
			else
			{
				return 0;
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.MTM.Order.NeedsInRahmenSaleEntity> GetSaleRahmenNeeded(List<int> artikelNrs, int artikelNr)
		{
			if(artikelNrs == null || artikelNrs.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select a.[Vorname/NameFirma] Customer, a.Nr,a.[Angebot-Nr], aa.[Artikel-Nr],ar.Artikelnummer,aa.[Position],aa.Anzahl AS RestQty,
								s.Anzahl as NeededInBOM,aa.Anzahl*s.Anzahl as SumNeeded,a.Bezug,be.ExtensionDate
								from Angebote a 
								inner join [angebotene Artikel] aa on a.Nr=aa.[Angebot-Nr]
								inner join __CTS_AngeboteBlanketExtension b on b.AngeboteNr=a.Nr
								inner join __CTS_AngeboteArticleBlanketExtension be on be.AngeboteArtikelNr=aa.Nr
								inner join Stücklisten s on s.[Artikel-Nr]=aa.[Artikel-Nr] and s.[Artikel-Nr des Bauteils]=@artikelNr
								inner join Artikel ar on ar.[Artikel-Nr]=aa.[Artikel-Nr]
								where a.Typ='Rahmenauftrag' and b.StatusId=2 and aa.Anzahl>0
								AND aa.[Artikel-Nr] IN ({string.Join(",", artikelNrs)})
                                order by a.[Angebot-Nr]";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("artikelNr", artikelNr);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MTM.Order.NeedsInRahmenSaleEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static int ROH_RA_Needs_agent(int userId, string username)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("usp_prs_compute_roh_rahmen_needs", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandTimeout = 400;
				sqlCommand.Parameters.AddWithValue("UserId", userId);
				sqlCommand.Parameters.AddWithValue("User", username);

				return sqlCommand.ExecuteNonQuery();
			}
		}
		public static DateTime? GetLastROH_RA_Needs_agentAgentExecutionTime()
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT TOP 1 [Date] FROM [__PRS_RA_Needs_ComputeLogs] ORDER BY Id Desc";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				return DateTime.TryParse(sqlCommand.ExecuteScalar()?.ToString(), out DateTime date) ? date : null;
			}
		}
		public static List<KeyValuePair<DateTime, string>> ROH_RA_Needs_agentLogs()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__PRS_RA_Needs_ComputeLogs]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<DateTime, string>(Convert.ToDateTime(x["Date"]), Convert.ToString(x["User"]))).ToList();
			}
			else
			{
				return null;
			}
		}
	}
}