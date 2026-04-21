using System.Buffers;
using Infrastructure.Data.Entities.Joins.PRS;

namespace Infrastructure.Data.Access.Joins.PRS
{
	public class PRSStockWarningsAccess
	{
		public static List<KeyValuePair<int, string>> GetArticles(string unit, int? prio, string articleNumber, Settings.SortingModel sorting = null, Settings.PaginModel paging = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"WITH RankedNegatives AS (
								SELECT 
									ArtikelNr,
									[Year],
									[Week],
									Unit,
									SumProd_cumul,
									ROW_NUMBER() OVER (PARTITION BY ArtikelNr,Unit ORDER BY [Year],[Week],Unit) AS RowNum
								FROM 
									__PRS_StockWarnings_Cumuls
								WHERE 
									SumProd_cumul < 0
							)
							SELECT 
							  distinct t.ArtikelNr,a.Artikelnummer
							FROM 
								__PRS_StockWarnings_Cumuls t
							JOIN 
								RankedNegatives rn ON t.ArtikelNr = rn.ArtikelNr 
												  AND t.[Year] = rn.[Year]
												  AND t.[Week] = rn.[Week]
							JOIN Artikel a ON rn.ArtikelNr=a.[Artikel-Nr]
                            {(prio == 4 ? "JOIN Bestellnummern bs ON bs.[Artikel-Nr]=a.[Artikel-Nr]" : "")}
							WHERE 
								rn.RowNum = 1
								AND rn.[Year] = YEAR(GETDATE())";

				if(unit is not null)
				{
					query += $" AND rn.Unit='{unit}'";
				}
				if(prio is not null)
				{
					switch(prio)
					{
						case 1:
						case 2:
							query += $" AND rn.Week<= DATEPART(ISO_WEEK, DATEADD(DAY, {(prio == 1 ? "42" : "90")}, GETDATE()))";
							break;
						case 3:
							query += $" AND rn.Week> DATEPART(ISO_WEEK, DATEADD(DAY, 90, GETDATE()))";
							break;
						case 4:
							query += $@" AND ISNULL(bs.Standardlieferant,0)=1
								         AND bs.Wiederbeschaffungszeitraum>59
                                         AND rn.[Week] > DATEPART(ISO_WEEK, GETDATE()) AND rn.Week<= DATEPART(ISO_WEEK, DATEADD(DAY, 180, GETDATE()))";
							break;
						default:
							break;
					}
				}
				if(!string.IsNullOrEmpty(articleNumber) && !string.IsNullOrWhiteSpace(articleNumber))
				{
					query += $" AND a.Artikelnummer LIKE '%{articleNumber}%'";
				}

				query += $"ORDER BY {(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName) ? $"{sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")}" : "a.[Artikelnummer]")} {(paging is null ? "" : $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ")}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(selectCommand: sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, string>(
					Convert.ToInt32(x["ArtikelNr"]),
					Convert.ToString(x["Artikelnummer"]))).ToList();
			}
			else
			{
				return null;
			}
		}
		public static int GetArticlesCount(string unit, int? prio, string articleNumber)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"WITH RankedNegatives AS (
								SELECT 
									ArtikelNr,
									[Year],
									[Week],
									Unit,
									SumProd_cumul,
									ROW_NUMBER() OVER (PARTITION BY ArtikelNr,Unit ORDER BY [Year],[Week],Unit) AS RowNum
								FROM 
									__PRS_StockWarnings_Cumuls
								WHERE 
									SumProd_cumul < 0
							)
							SELECT 
							  COUNT(distinct t.ArtikelNr)
							FROM 
								__PRS_StockWarnings_Cumuls t
							JOIN 
								RankedNegatives rn ON t.ArtikelNr = rn.ArtikelNr 
												  AND t.[Year] = rn.[Year]
												  AND t.[Week] = rn.[Week]
							JOIN Artikel a ON rn.ArtikelNr=a.[Artikel-Nr]
                           {(prio == 4 ? "JOIN Bestellnummern bs ON bs.[Artikel-Nr]=a.[Artikel-Nr]" : "")}
							WHERE 
								rn.RowNum = 1
								AND rn.[Year] = YEAR(GETDATE())";

				if(unit is not null)
				{
					query += $" AND rn.Unit='{unit}'";
				}
				if(prio is not null)
				{
					switch(prio)
					{
						case 1:
						case 2:
							query += $" AND rn.Week<= DATEPART(ISO_WEEK, DATEADD(DAY, {(prio == 1 ? "42" : "90")}, GETDATE()))";
							break;
						case 3:
							query += $" AND rn.Week> DATEPART(ISO_WEEK, DATEADD(DAY, 90, GETDATE()))";
							break;
						case 4:
							query += $@" AND ISNULL(bs.Standardlieferant,0)=1
								         AND bs.Wiederbeschaffungszeitraum>59
                                         AND rn.[Week] > DATEPART(ISO_WEEK, GETDATE()) AND rn.Week<= DATEPART(ISO_WEEK, DATEADD(DAY, 180, GETDATE()))";
							break;
						default:
							break;
					}
				}
				if(!string.IsNullOrEmpty(articleNumber) && !string.IsNullOrWhiteSpace(articleNumber))
				{
					query += $" AND a.Artikelnummer LIKE '%{articleNumber}%'";
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 90; // sec
				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out int count) ? count : 0;
			}
		}
		public static int PrsStockWarningsData(int userId)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("usp_prs_compute_all", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandTimeout = 400;
				sqlCommand.Parameters.AddWithValue("userId", userId);

				return sqlCommand.ExecuteNonQuery();
			}
		}
		public static DateTime? GetLastAgentExecutionTime()
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT TOP 1 [Date] FROM [__PRS_StockWarnings_ComputeLogs] ORDER BY Id Desc";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				return DateTime.TryParse(sqlCommand.ExecuteScalar().ToString(), out DateTime date) ? date : null;
			}
		}
		public static List<__PRS_StockWarnings_CumulsEntity> GetCumuls(string unit, int artikelNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__PRS_StockWarnings_Cumuls] WHERE [ArtikelNr]=@artikelNr AND [Unit]=@unit AND [Year]=YEAR(GETDATE()) AND [Week]>=DATEPART(ISO_WEEK,GETDATE())";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("artikelNr", artikelNr);
				sqlCommand.Parameters.AddWithValue("unit", unit);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.PRS.__PRS_StockWarnings_CumulsEntity(x)).ToList();
			}
			else
			{
				return new List<__PRS_StockWarnings_CumulsEntity>();
			}
		}
		public static List<__PRS_StockWarnings_FaStatusEntity> GetFas(string unit, int artikelNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"SELECT ArtikelNr ,[Year],[Week],SUM([Qty]) Qty ,Unit  from __PRS_StockWarnings_FaStatus 
                                WHERE [ArtikelNr]=@artikelNr AND [Unit]=@unit  AND [Year]=YEAR(GETDATE()) AND [Week]>=DATEPART(ISO_WEEK,GETDATE())
                                GROUP BY ArtikelNr ,[Year],[Week],Unit";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("artikelNr", artikelNr);
				sqlCommand.Parameters.AddWithValue("unit", unit);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.PRS.__PRS_StockWarnings_FaStatusEntity(x)).ToList();
			}
			else
			{
				return new List<__PRS_StockWarnings_FaStatusEntity>();
			}
		}
		public static List<__PRS_StockWarnings_PoStatusEntity> GetPos(string unit, int artikelNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"SELECT ArtikelNr ,[Year],[Week],SUM([Qty]) Qty ,Unit  
                                FROM __PRS_StockWarnings_PoStatus 
                                WHERE [ArtikelNr]=@artikelNr AND [Unit]=@unit AND [Year]=YEAR(GETDATE()) AND [Week]>=DATEPART(ISO_WEEK,GETDATE())
                                GROUP BY ArtikelNr ,[Year],[Week],Unit";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("artikelNr", artikelNr);
				sqlCommand.Parameters.AddWithValue("unit", unit);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.PRS.__PRS_StockWarnings_PoStatusEntity(x)).ToList();
			}
			else
			{
				return new List<__PRS_StockWarnings_PoStatusEntity>();
			}
		}
		public static decimal GetBacklogNeeds(string unit, int artikelNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @$"select SUM(Qty) from [__PRS_StockWarnings_FaStatus] where  
								Unit=@unit
                                and ArtikelNr=@artikelNr
								and ([Year]<Year(GETDATE()) 
                                OR ([Year]=Year(GETDATE()) and [Week]<DATEPART(ISO_WEEK,GETDATE())))";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("unit", unit);
				sqlCommand.Parameters.AddWithValue("artikelNr", artikelNr);
				sqlCommand.CommandTimeout = 90; // sec
				return decimal.TryParse(sqlCommand.ExecuteScalar().ToString(), out decimal count) ? count : 0;
			}
		}
		public static decimal GetBacklogOrders(List<int> warehouses, int artikelNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @$"SELECT ISNULL(SUM(ba.Anzahl),0)
								FROM [bestellte Artikel] ba INNER Join Bestellungen b ON b.Nr = ba.[Bestellung-Nr] 
								AND b.Typ = 'Bestellung' INNER JOIN Artikel A ON A.[Artikel-Nr]=ba.[Artikel-Nr] 
								WHERE  
								ISNULL([Position erledigt],0) <> 1 AND Anzahl > 0 
								AND ISNULL(b.erledigt,0)<>1 
								AND ISNULL(erledigt_pos,0)<>1
								AND ba.Lagerort_id in ({string.Join(",", warehouses)}) 
								AND b.gebucht=1 
								AND (YEAR(ba.Liefertermin)<Year(GETDATE()) 
								OR (YEAR(ba.Liefertermin)=Year(GETDATE()) 
								AND DATEPART(ISO_WEEK,ba.Liefertermin)<DATEPART(ISO_WEEK,GETDATE())))
								AND ba.[Artikel-Nr]=@artikelNr";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("artikelNr", artikelNr);
				sqlCommand.CommandTimeout = 90; // sec
				return decimal.TryParse(sqlCommand.ExecuteScalar().ToString(), out decimal count) ? count : 0;
			}
		}
		public static decimal GetBedarf(int unitId, int artikelNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @$"SELECT Bestand FROM __PRS_StockWarnings_Bedarf WHERE [Artikel-Nr]=@artikelNr and MainLager=@unitId";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("artikelNr", artikelNr);
				sqlCommand.Parameters.AddWithValue("unitId", unitId);
				sqlCommand.CommandTimeout = 90; // sec
				var result = sqlCommand.ExecuteScalar();
				return result != null
					? decimal.TryParse(sqlCommand.ExecuteScalar().ToString(), out decimal count) ? count : 0
					: 0;
			}
		}
		public static decimal GetMinimumStock(List<int> warehouses, int artikelNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @$"SELECT SUM(Mindestbestand) FROM lager WHERE [Artikel-Nr]=@artikelNr and Lagerort_id in ({string.Join(",", warehouses)})";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("artikelNr", artikelNr);
				sqlCommand.CommandTimeout = 90; // sec
				return decimal.TryParse(sqlCommand.ExecuteScalar().ToString(), out decimal count) ? count : 0;
			}
		}
		public static List<StockWarningsNeedsInOtherPlantsEntity> GetNeedsInOtherPlants(string unit, int artikelNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"SELECT b.Unit,ISNULL(SUM(f.Qty),0) Needed,ISNULL(b.Bestand,0) Bedarf,ISNULL(SUM(p.Qty),0) OpenOrders 
								FROM __PRS_StockWarnings_BruttoBedarf b 
								LEFT JOIN __PRS_StockWarnings_PoStatus p
								ON b.Unit=p.Unit AND b.[Artikel-Nr]=p.ArtikelNr
								LEFT JOIN __PRS_StockWarnings_FaStatus f
								ON f.Unit=b.Unit AND f.ArtikelNr=b.[Artikel-Nr]
								WHERE b.[Artikel-Nr]=@artikelNr AND b.Unit<>@unit
								GROUP BY b.Unit,b.Bestand";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("artikelNr", artikelNr);
				sqlCommand.Parameters.AddWithValue("unit", unit);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.PRS.StockWarningsNeedsInOtherPlantsEntity(x)).ToList();
			}
			else
			{
				return new List<StockWarningsNeedsInOtherPlantsEntity>();
			}
		}
		public static List<StockWarningsFaViewEntity> GetFaView(int unitId, int artikelNr, int year, int week, bool backlog, string unit, bool tn = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "";
				if(backlog)
					query = @$"SELECT  F.ID,Fertigungsnummer,Termin_Bestätigt1 AS Termin,SUM(FP.Anzahl / F.Originalanzahl * F.Anzahl) Qty
								FROM Fertigung F INNER JOIN Fertigung_Positionen FP ON F.ID = FP.ID_Fertigung 
								AND F.Termin_Bestätigt1 <= DATEADD(MONTH, 6, GETDATE()) 
								AND F.Kennzeichen = 'offen' AND IsNULL(F.FA_Gestartet, 0) = 0 AND F.Originalanzahl <> 0 
								AND F.Originalanzahl IS NOT NULL AND F.Anzahl IS NOT NULL
								AND (F.Lagerort_ID = {unitId} {(tn ? "OR F.Lagerort_ID=7" : "")} )
								AND FP.Artikel_Nr=@artikelNr
                                AND (Year(F.Termin_Bestätigt1)<Year(GETDATE()) OR Year(F.Termin_Bestätigt1)=Year(GETDATE()) 
                                AND DATEPART(ISO_WEEK,F.Termin_Bestätigt1)<DATEPART(ISO_WEEK,GETDATE()))
								GROUP BY F.ID,Fertigungsnummer,Termin_Bestätigt1";
				else
					query = $@"SELECT ID,Fertigungsnummer,Termin_Bestätigt1 AS Termin,Qty FROM __PRS_StockWarnings_FaStatus 
                               WHERE Year(Termin_Bestätigt1)={year} AND DATEPART(ISO_WEEK,Termin_Bestätigt1)={week} AND Unit='{unit}' AND ArtikelNr=@artikelNr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("artikelNr", artikelNr);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new StockWarningsFaViewEntity(x)).ToList();
			}
			else
			{
				return new List<StockWarningsFaViewEntity>();
			}
		}
		public static List<StockWarningsPoViewEntity> GetPoView(List<int> warehouses, int artikelNr, bool backlog, int year, int week, string unit)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "";
				if(backlog)
					query = @$"SELECT b.Nr,b.[Bestellung-Nr],ba.Anzahl,ba.Liefertermin,ba.Bestätigter_Termin
								FROM [bestellte Artikel] ba INNER Join Bestellungen b ON b.Nr = ba.[Bestellung-Nr] 
								AND b.Typ = 'Bestellung' INNER JOIN Artikel A ON A.[Artikel-Nr]=ba.[Artikel-Nr] 
								WHERE Anzahl > 0
								AND ISNULL([Position erledigt],0) <> 1 
								AND ISNULL(b.erledigt,0)<>1 
								AND ISNULL(erledigt_pos,0)<>1 AND ba.[Artikel-Nr]=@artikelNr 
								AND ba.Lagerort_id in ({string.Join(",", warehouses)}) 
								AND b.gebucht=1 
                                AND ba.Bestätigter_Termin != '31-12-2999' 
                                AND Year(ba.Liefertermin)=Year(GETDATE())
								AND DATEPART(ISO_WEEK,ba.Liefertermin)<DATEPART(ISO_WEEK,GETDATE())";
				else
					query = @$"SELECT Nr,[Bestellung-Nr],Qty Anzahl,Liefertermin,Bestätigter_Termin
                             FROM __PRS_StockWarnings_PoStatus WHERE Year(Liefertermin)={year} AND DATEPART(ISO_WEEK,Liefertermin)={week}
                             AND ArtikelNr=@artikelNr and Unit='{unit}'";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("artikelNr", artikelNr);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new StockWarningsPoViewEntity(x)).ToList();
			}
			else
			{
				return new List<StockWarningsPoViewEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.MTM.Order.FaultyOrdersEntity> GetFaultyOrders(List<int> warehouses, int? artikelNr, Settings.SortingModel sorting = null, Settings.PaginModel paging = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"SELECT COUNT(*) OVER() TotalCount,
							B.Nr,B.[Bestellung-Nr],BA.Bestätigter_Termin,B.[Vorname/NameFirma] Supplier
							FROM [bestellte Artikel] BA 
							JOIN Bestellungen B 
							ON BA.[Bestellung-Nr] = B.Nr 
							AND ISNULL(BA.Anzahl,0) > 0 
							AND (BA.Bestätigter_Termin <=  GETDATE() OR ISDATE(BA.Bestätigter_Termin) = 0) 
							AND ISNULL(BA.[Start Anzahl],0) > 0
							AND ISNULL(BA.erledigt_pos,0) <> 1
                            {(warehouses != null && warehouses.Count > 0 ? $"AND BA.Lagerort_id IN ({string.Join(",", warehouses)})" : "")}
                            {(artikelNr is not null ? $"AND BA.[Artikel-Nr] = {artikelNr}" : "")}
							GROUP BY B.Nr,B.[Bestellung-Nr],BA.Bestätigter_Termin,B.[Vorname/NameFirma] ";
				query += @$" ORDER BY {(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName) ? $"{sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")}" : "BA.Bestätigter_Termin")} 
                {(paging is null ? "" : $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ")}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MTM.Order.FaultyOrdersEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MTM.Order.FaultyOrdersEntity>();
			}
		}
		public static int GetFaultyOrdersCount(List<int> warehouses, int? artikelNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"SELECT COUNT(*) FROM (
								SELECT COUNT(*) OVER() TotalCount
								,B.[Bestellung-Nr],BA.Bestätigter_Termin,B.[Vorname/NameFirma] Supplier
								FROM [bestellte Artikel] BA 
								JOIN Bestellungen B 
								ON BA.[Bestellung-Nr] = B.Nr 
								AND ISNULL(BA.Anzahl,0) > 0 
								AND (BA.Bestätigter_Termin <=  GETDATE() OR ISDATE(BA.Bestätigter_Termin) = 0) 
								AND ISNULL(BA.[Start Anzahl],0) > 0
								AND ISNULL(BA.erledigt_pos,0) <> 1
                               {(warehouses != null && warehouses.Count > 0 ? $"AND BA.Lagerort_id IN ({string.Join(",", warehouses)})" : "")}
                               {(artikelNr is not null ? $"AND BA.[Artikel-Nr] = {artikelNr}" : "")}
								GROUP BY B.[Bestellung-Nr],BA.Bestätigter_Termin,B.[Vorname/NameFirma]
								) X";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 90; // sec
				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out int count) ? count : 0;
			}
		}

		public static List<Infrastructure.Data.Entities.Joins.MTM.Order.GetFaultyFasEntity> GetFaultyFas(List<int> warehouses, int? artikelNr, string? SearchTerms, Settings.SortingModel sorting = null, Settings.PaginModel paging = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"SELECT COUNT(*) OVER() TotalCount
								,F.Fertigungsnummer Fertigungsnummer,Termin_Bestätigt1,F.ID
								FROM Fertigung F 
								JOIN Fertigung_Positionen fp 
								ON F.ID = fp.ID_Fertigung 
								AND  F.Termin_Bestätigt1 <= GETDATE() 
								AND F.Kennzeichen = 'offen' 
								AND IsNULL(F.FA_Gestartet,0)= 0 
								AND ISNULL (F.Originalanzahl,0 ) > 0  
								AND ISNULL (F.Originalanzahl,0 ) > ISNULL (F.Anzahl_erledigt,0 )  
								AND ISNULL(F.Anzahl,0) > 0 
                                {(warehouses != null && warehouses.Count > 0 ? $"AND F.Lagerort_ID IN ({string.Join(",", warehouses)})" : "")}
                                {(artikelNr is not null ? $"AND FP.Artikel_Nr = {artikelNr}" : "")}";
				if(SearchTerms != null)
				{
					query += $" AND ( F.[Fertigungsnummer] LIKE '{SearchTerms}%')";

				}
				query += @$"GROUP BY F.Fertigungsnummer ,Termin_Bestätigt1,F.ID";

				query += @$" ORDER BY {(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName) ? $"{sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")}" : "Termin_Bestätigt1")} 
                {(paging is null ? "" : $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ")}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MTM.Order.GetFaultyFasEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MTM.Order.GetFaultyFasEntity>();
			}
		}
		public static int GetFaultyFasCount(List<int> warehouses, int? artikelNr, string? SearchTerms)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"SELECT COUNT(*) FROM (
								SELECT COUNT(*) OVER() TotalCount
								,F.Fertigungsnummer Fertigungsnummer,Termin_Bestätigt1,F.ID
								FROM Fertigung F 
								JOIN Fertigung_Positionen fp 
								ON F.ID = fp.ID_Fertigung 
								AND  F.Termin_Bestätigt1 <= GETDATE() 
								AND F.Kennzeichen = 'offen' 
								AND IsNULL(F.FA_Gestartet,0)= 0 
								AND ISNULL (F.Originalanzahl,0 ) > 0  
								AND ISNULL (F.Originalanzahl,0 ) > ISNULL (F.Anzahl_erledigt,0 )  
								AND ISNULL(F.Anzahl,0) > 0 
                                {(warehouses != null && warehouses.Count > 0 ? $"AND F.Lagerort_ID IN ({string.Join(",", warehouses)})" : "")}
                                {(artikelNr is not null ? $"AND FP.Artikel_Nr = {artikelNr}" : "")}";
				if(SearchTerms != null)
				{
					query += $" AND ( F.[Fertigungsnummer] LIKE '{SearchTerms}%')";

				}
				query += @$"GROUP BY F.Fertigungsnummer ,Termin_Bestätigt1,F.ID) x";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 90; // sec
				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out int count) ? count : 0;
			}
		}

		public static List<StockWarningsPoViewEntity> GetUnconfirmedOrders(List<int> warehouses, int artikelNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT b.Nr,b.[Bestellung-Nr],ba.[Artikel-Nr], Anzahl,Lagerort_id,b.Datum,ba.Liefertermin,ba.Bestätigter_Termin
								FROM [bestellte Artikel] ba INNER Join Bestellungen b ON b.Nr = ba.[Bestellung-Nr] 
								AND b.Typ = 'Bestellung' 
								INNER JOIN Artikel A on A.[Artikel-Nr]=ba.[Artikel-Nr]
								WHERE Anzahl > 0 
								AND ba.Liefertermin <= DATEADD(MONTH, 6, GETDATE())
								AND ba.Bestätigter_Termin = '31-12-2999'
								AND ISNULL([Position erledigt],0) <> 1 AND Anzahl > 0 
								AND ISNULL(b.erledigt,0)<>1 and ISNULL(erledigt_pos,0)<>1 and b.gebucht=1
								AND Lagerort_ID IN ({string.Join(",", warehouses)})
								AND ba.[Artikel-Nr]=@artikelNr";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("artikelNr", artikelNr);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new StockWarningsPoViewEntity(x, true)).ToList();
			}
			else
			{
				return new List<StockWarningsPoViewEntity>();
			}
		}
		public static List<ExtraOrdersNeedsInOtherPlantsEntity> GetNeedsInOtherPlantsExtraOrders(int orderWarehouse, List<int> faWarehouses, int artikelNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"SELECT	
							Bestellungen.[Vorname/NameFirma] AS Lieferant,
							Bestellungen.[Bestellung-Nr], 
							[bestellte Artikel].Anzahl, 
							[bestellte Artikel].Liefertermin AS Wünschtermin,
							[bestellte Artikel].Lagerort_id,
							[bestellte Artikel].Bestätigter_Termin,
							[Bestellungen].Nr,
							Bestellungen.ProjectPurchase
							FROM [bestellte Artikel]
								INNER JOIN Bestellungen ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
								INNER JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]
							INNER JOIN Lieferanten ON Bestellungen.[Lieferanten-Nr] = Lieferanten.nummer
							WHERE 
								[bestellte Artikel].Lagerort_id <> @orderWarehouse  
								And Bestellungen.Typ = 'Bestellung' 
								And Bestellungen.erledigt = 0 
								And [bestellte Artikel].erledigt_pos = 0 
								And Bestellungen.Rahmenbestellung = 0
								And Left([Artikelnummer], 3) <> '227' 
								And Left([Artikelnummer], 3) <> '226'
								And Bestellungen.gebucht = 1
								and artikel.[Artikel-Nr]=66329


				AND 
				Artikelnummer Not In 
				(
					select distinct A.Artikelnummer from Fertigung_Positionen FP inner join Artikel A
					on A.[Artikel-Nr]=FP.Artikel_Nr
					inner join Fertigung F on FP.ID_Fertigung=F.ID
					WHERE F.Lagerort_id not in ({string.Join(",", faWarehouses)}) AND F.Kennzeichen=N'offen' 
				)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("orderWarehouse", orderWarehouse);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.PRS.ExtraOrdersNeedsInOtherPlantsEntity(x)).ToList();
			}
			else
			{
				return new List<ExtraOrdersNeedsInOtherPlantsEntity>();
			}
		}

		public static List<StockWarningsAuswertungExcelEntity> GetStockWarningAuswertungExcel(string unit, int? prio, List<int> warehouses)
		{
			var prioClaus = "";
			if(prio is not null)
			{
				switch(prio)
				{
					case 1:
					case 2:
						prioClaus = $" AND rn.Week<= DATEPART(ISO_WEEK, DATEADD(DAY, {(prio == 1 ? "42" : "90")}, GETDATE()))";
						break;
					case 3:
						prioClaus = $" AND rn.Week> DATEPART(ISO_WEEK, DATEADD(DAY, 90, GETDATE()))";
						break;
					case 4:
						prioClaus = $@" AND ISNULL(b.Standardlieferant,0)=1
								         AND b.Wiederbeschaffungszeitraum>59
                                         AND rn.[Week] > DATEPART(ISO_WEEK, GETDATE()) AND rn.Week<= DATEPART(ISO_WEEK, DATEADD(DAY, 180, GETDATE()))";
						break;
					default:
						break;
				}
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"--preparing filtred articles
								IF OBJECT_ID('tempdb..#FiltredArticles') IS NOT NULL DROP TABLE #FiltredArticles;
								WITH RankedNegatives AS (
								SELECT 
								ArtikelNr,
								[Year],
								[Week],
								Unit,
								SumProd_cumul,
								ROW_NUMBER() OVER (PARTITION BY ArtikelNr,Unit ORDER BY [Year],[Week],Unit) AS RowNum
								FROM 
								__PRS_StockWarnings_Cumuls
								WHERE 
								SumProd_cumul < 0
								)
								SELECT 
								distinct t.ArtikelNr,a.Artikelnummer,a.[Bezeichnung 1],ad.Name1,b.Einkaufspreis,
								null stock,null Total_gross_requirements,null Total_confirmed_orders,null Total_unconfirmed_orders,
								rn.[Week] Material_date_CW,null [Difference],null Minimum_stock,b.Mindestbestellmenge Minimum_order_quantity,
								a.Verpackungsmenge Packaging_quantity,b.Wiederbeschaffungszeitraum Replenishment_time,null Open_frame_quantity,null Obsolete

								into #FiltredArticles
								FROM 
								__PRS_StockWarnings_Cumuls t
								JOIN 
								RankedNegatives rn ON t.ArtikelNr = rn.ArtikelNr 
								AND t.[Year] = rn.[Year]
								AND t.[Week] = rn.[Week]
								JOIN Artikel a ON rn.ArtikelNr=a.[Artikel-Nr]
								JOIN Bestellnummern b on b.[Artikel-Nr]=rn.ArtikelNr
								JOIN Lieferanten l on b.[Lieferanten-Nr]=l.Nr
								JOIN adressen ad on ad.Nr=l.nummer
								WHERE 
								rn.RowNum = 1
								AND rn.[Year] = YEAR(GETDATE())
								AND rn.Unit='{unit}'
								AND ISNULL(b.Standardlieferant,0)=1
								{prioClaus}
								--update confirmed
								update f
								set f.Total_confirmed_orders=ISNULL(c.SumConfirmed,0)
								FROM #FiltredArticles f inner join
								(
								select ISNULL(SUM(Anzahl),0) SumConfirmed,[Artikel-Nr]
								FROM [bestellte Artikel] ba INNER Join Bestellungen b ON b.Nr = ba.[Bestellung-Nr] 
								inner join #FiltredArticles f on f.ArtikelNr=ba.[Artikel-Nr]
								WHERE Anzahl > 0 
								AND b.Typ = 'Bestellung' 
								AND ba.Liefertermin <= DATEADD(MONTH, 6, GETDATE()) AND
								ba.Bestätigter_Termin != '31-12-2999'
								and ba.Lagerort_id in ({string.Join(",", warehouses)})
								group by [Artikel-Nr]
								) c
								on f.ArtikelNr=c.[Artikel-Nr]
								--update unconfirmed
								update f
								set f.Total_unconfirmed_orders=ISNULL(c.SumUnconfirmed,0)
								FROM #FiltredArticles f inner join
								(
								select ISNULL(SUM(Anzahl),0) SumUnconfirmed,[Artikel-Nr]
								FROM [bestellte Artikel] ba INNER Join Bestellungen b ON b.Nr = ba.[Bestellung-Nr] 
								inner join #FiltredArticles f on f.ArtikelNr=ba.[Artikel-Nr]
								WHERE Anzahl > 0 
								AND b.Typ = 'Bestellung' 
								AND ba.Liefertermin <= DATEADD(MONTH, 6, GETDATE()) AND
								ba.Bestätigter_Termin = '31-12-2999'
								and ba.Lagerort_id in ({string.Join(",", warehouses)})
								group by [Artikel-Nr]
								) c
								on f.ArtikelNr=c.[Artikel-Nr]
								-- update minimum stock
								update f
								set f.Minimum_stock=c.Mindestbestand from
								#FiltredArticles f inner join 
								(
								select SUM(Mindestbestand) Mindestbestand,l.[Artikel-Nr] from lager l
								inner join Artikel a on a.[Artikel-Nr]=l.[Artikel-Nr]
								where a.Warengruppe<>N'EF'
								and l.Lagerort_id in ({string.Join(",", warehouses)})
								group by l.[Artikel-Nr]
								) c on f.ArtikelNr=c.[Artikel-Nr]
								-- update open ra
								update f
								set f.Open_frame_quantity=c.SumOpenRA
								from #FiltredArticles f inner join 
								(
								select ar.[Artikel-Nr],SUM(ar.Anzahl) SumOpenRA from Angebote a inner join [angebotene Artikel] ar
								on a.Nr=ar.[Angebot-Nr]
								inner join __CTS_AngeboteBlanketExtension eb
								on eb.AngeboteNr=a.Nr
								inner join __CTS_AngeboteArticleBlanketExtension e
								on e.RahmenNr=a.Nr
								where a.Typ='Rahmenauftrag'
								and eb.BlanketTypeId=1
								and eb.StatusId=2
								and e.ExtensionDate>=GETDATE()
								group by ar.[Artikel-Nr]
								) c on f.ArtikelNr=c.[Artikel-Nr]
								-- update qty in absolete
								update f set f.Obsolete=c.Bestand
								from #FiltredArticles f inner join 
								(
								select SUM(Bestand) Bestand,l.[Artikel-Nr] from lager l
								inner join Artikel a on a.[Artikel-Nr]=l.[Artikel-Nr]
								where a.Warengruppe<>N'EF'
								and l.Lagerort_id=22
								group by l.[Artikel-Nr]
								) c on c.[Artikel-Nr]=f.ArtikelNr
								--update stock
								update f set f.stock=b.bestand
								from #FiltredArticles f inner join __PRS_StockWarnings_BruttoBedarf b
								on f.ArtikelNr=b.[Artikel-Nr]
								where b.Unit='{unit}'
								-- update gross requirement
								update f set f.Total_gross_requirements=c.sumNeeds
								from #FiltredArticles f inner join
								(
								select SUM(Qty) sumNeeds,ArtikelNr from __PRS_StockWarnings_FaStatus 
								where Unit='{unit}' and [Week]<42
								group by ArtikelNr
								) c on f.ArtikelNr=c.ArtikelNr
								-- update diffrence
								update f set f.[Difference]=ISNULL(f.Total_gross_requirements,0)-ISNULL(f.stock,0)-ISNULL(f.Total_confirmed_orders,0)
								from #FiltredArticles f

								select * from #FiltredArticles";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new StockWarningsAuswertungExcelEntity(x)).ToList();
			}
			else
			{
				return new List<StockWarningsAuswertungExcelEntity>();
			}
		}
		public static List<ExtraOrdersAuswertungEntity> GetExtraOrdersAuswertungExcel(List<int> fertigungLager, int hauftLager)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"--preparing filtred articles
								IF OBJECT_ID('tempdb..#FiltredArticles') IS NOT NULL DROP TABLE #FiltredArticles;
												SELECT DISTINCT
														Artikel.[Artikel-Nr],
														Artikel.Artikelnummer,
														adressen.Name1,
                                                        Artikel.[Bezeichnung 1],
														null Stock,
														Bestellnummern.Einkaufspreis,
														null SumOrders,
														null ValueWithoutRequirement,
														null OtherPlantsRequirement,
														'WS-TN' Unit
														into #FiltredArticles
														FROM [bestellte Artikel]
															INNER JOIN Bestellungen ON Bestellungen.Nr = [bestellte Artikel].[Bestellung-Nr]
															INNER JOIN Artikel ON [bestellte Artikel].[Artikel-Nr] = Artikel.[Artikel-Nr]
														INNER JOIN Lieferanten ON Bestellungen.[Lieferanten-Nr] = Lieferanten.nummer
														INNER JOIN adressen on adressen.Nr=Lieferanten.nummer
														INNER JOIN Bestellnummern on Bestellnummern.[Artikel-Nr]=[bestellte Artikel].[Artikel-Nr]
														WHERE 
															[bestellte Artikel].Lagerort_id ={hauftLager}   
															And Bestellungen.Typ = 'Bestellung' 
															And Bestellungen.erledigt = 0 
															And [bestellte Artikel].erledigt_pos = 0 
															And Bestellungen.Rahmenbestellung = 0
															And Left([Artikelnummer], 3) <> '227' 
															And Left([Artikelnummer], 3) <> '226'
															And Bestellungen.gebucht = 1
															AND Bestellnummern.Standardlieferant=1
															AND (Bestellungen.ProjectPurchase is null or Bestellungen.ProjectPurchase=0)

								AND 
								Artikel.Artikelnummer Not In 
								(
												select distinct A.Artikelnummer from Fertigung_Positionen FP inner join Artikel A
												on A.[Artikel-Nr]=FP.Artikel_Nr
												inner join Fertigung F on FP.ID_Fertigung=F.ID
												WHERE F.Lagerort_id in ({string.Join(",", fertigungLager)}) AND F.Kennzeichen=N'offen' 
								)
								-- update stock
								update f
								set f.Stock=b.Bestand
								from #FiltredArticles f left join __PRS_StockWarnings_BruttoBedarf b
								on f.[Artikel-Nr]=b.[Artikel-Nr] and b.Unit=f.Unit
								-- update SumOrders
								update f
								set f.SumOrders=c.SumOrders
								from #FiltredArticles f left join
								(
								select ISNULL(SUM(Anzahl),0) SumOrders,ba.[Artikel-Nr]
								FROM [bestellte Artikel] ba INNER Join Bestellungen b ON b.Nr = ba.[Bestellung-Nr] 
								inner join #FiltredArticles f on f.[Artikel-Nr]=ba.[Artikel-Nr]
								WHERE b.Typ='Bestellung'
								and b.erledigt=0
								and ba.erledigt_pos=0
								and ba.Lagerort_id={hauftLager}
								and b.gebucht=1
								group by ba.[Artikel-Nr]
								) c
								on c.[Artikel-Nr]=f.[Artikel-Nr]
								-- update value without requirement
								update f set f.ValueWithoutRequirement=(ISNULL(f.Stock,0)+ISNULL(f.SumOrders,0))*ISNULL(f.Einkaufspreis,0)
								from #FiltredArticles f
								-- update other plants requirement
								update f
								set f.OtherPlantsRequirement=c.SumOtherPlants
								from #FiltredArticles f left join 
								(
								select ArtikelNr,SUM(Qty) SumOtherPlants from __PRS_StockWarnings_FaStatus
								where Unit<>'WS-TN'
								group by ArtikelNr
								) c on f.[Artikel-Nr]=c.ArtikelNr


								select f.*,c.OtherPlants from #FiltredArticles f
								left join 
								(
								SELECT STRING_AGG(Unit, '/') AS OtherPlants,ArtikelNr
								FROM 
								(select DISTINCT ArtikelNr,Unit from __PRS_StockWarnings_FaStatus where Unit<>'WS-TN') X
								group by ArtikelNr
								) c on f.[Artikel-Nr]=c.ArtikelNr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new ExtraOrdersAuswertungEntity(x)).ToList();
			}
			else
			{
				return new List<ExtraOrdersAuswertungEntity>();
			}
		}
	}
}