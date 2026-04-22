using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace Infrastructure.Data.Access.Joins.CRP
{
	public class CRPStatisticsAccess
	{
		public static List<Infrastructure.Data.Entities.Joins.CRP.LagerBestandFG_CRPEntity> GetLagerBestandFG_CRP()
		{
			// - REM: replace Kosten gesamt 
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT   v.Artikelnummer, v.Kunde,
                               v.[Bezeichnung 1], v.[Bezeichnung 2],
                               v.Freigabestatus, v.[CS Kontakt],
                               v.Lagerort, v.Bestand,   ISNULL(L.Mindestbestand,0) AS Mindestbestand, v.VK
                               AS [VK Gesamt], v.Gesamtpreis_mit_cu AS [Kosten gesamt], v.Gesamtpreis_ohne_cu AS [Kosten gesamt ohne CU], v.Verkaufspreis AS VKE,
							   a.UBG, IsNULL(a.EdiDefault,0) EdiDefault , a.[BemerkungCRP]
                               FROM [View_Lagerbestände Analyse L_CS_wNegativ] v LEFT JOIN [Artikel] a on a.Artikelnummer=v.Artikelnummer 
							   left join [Lager] L on L.[Artikel-Nr]=a.[Artikel-Nr] and  v.Lagerort_id = L.Lagerort_id
							   ORDER BY v.Artikelnummer;";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CRP.LagerBestandFG_CRPEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CRP.LagerBestandFG_CRPEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.CRP.OrderProcessingLogs> GetOrderProcessingLogs(Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging,
			string SearchValueVorfallNr, string SearchValuePosition, string SearchValueartikelnummer, string SearchValueUsername, List<string> ListSearchType)
		{
			// - REM: replace Kosten gesamt 
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"select 0 as totalRows, L.[AngebotNr] as [Vorfall-Nr] ,A.bezug as [DokNr],AR.Position as [Pos],ART.Artikelnummer as [PSZ#],
							L.Username as [User] ,ISNULL(A.Typ,L.LogObject) AS Typ,L.LogText as [Log],L.[DateTime]
							from [__CTS_OrderProcesssing_Log] L
							left join Angebote A on L.Nr=A.Nr
							left join [angebotene Artikel] AR on L.PositionNr =AR.Nr
							left join Artikel ART on AR.[Artikel-Nr]=ART.[Artikel-Nr]
							WHERE L.[AngebotNr]<>0  ";
				if(SearchValueVorfallNr != "" && SearchValueVorfallNr != null)
				{
					query += $@" and  L.[AngebotNr] Like '{SearchValueVorfallNr}%' ";
				}
				if(SearchValuePosition != "" && SearchValuePosition != null)
				{
					query += $@" and AR.Position  Like '{SearchValuePosition}%' ";
				}
				if(SearchValueartikelnummer != "" && SearchValueartikelnummer != null)
				{
					query += $@" and ART.Artikelnummer Like '{SearchValueartikelnummer}%' ";
				}
				if(SearchValueUsername != "" && SearchValueUsername != null)
				{
					query += $@" and L.Username Like '{SearchValueUsername}%' ";
				}

				if(ListSearchType != null && ListSearchType.Count > 0)
				{

					query += " and ( ";
					query += $@" A.Typ IN ({string.Join(",", ListSearchType.Select(x => $"'{x}'").ToList())}) 
                    OR L.LogObject IN ({string.Join(",", ListSearchType.Select(x => $"'{x}'").ToList())})";
					//if(ListSearchType.Count > 1)
					//{

					//	foreach(var item in ListSearchType.Where((value, index) => index > 0))
					//	{
					//		query += $@" or A.Typ Like '{item}%' ";
					//	}
					//}

					query += ") ";
				}

				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += "ORDER BY [Vorfall-Nr]";
				}
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CRP.OrderProcessingLogs(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CRP.OrderProcessingLogs>();
			}
		}
		public static int GetOrderProcessingLogs_count(string SearchValueVorfallNr, string SearchValuePosition, string SearchValueartikelnummer, string SearchValueUsername, List<string> ListSearchType)
		{
			// - REM: replace Kosten gesamt 
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"select count(*)
							from [__CTS_OrderProcesssing_Log] L
							left join Angebote A on L.Nr=A.Nr
							left join [angebotene Artikel] AR on L.PositionNr =AR.Nr
							left join Artikel ART on AR.[Artikel-Nr]=ART.[Artikel-Nr]
							WHERE L.[AngebotNr]<>0  ";
				if(SearchValueVorfallNr != "" && SearchValueVorfallNr != null)
				{
					query += $@" and  L.[AngebotNr] Like '{SearchValueVorfallNr}%' ";
				}
				if(SearchValuePosition != "" && SearchValuePosition != null)
				{
					query += $@" and AR.Position  Like '{SearchValuePosition}%' ";
				}
				if(SearchValueartikelnummer != "" && SearchValueartikelnummer != null)
				{
					query += $@" and ART.Artikelnummer Like '{SearchValueartikelnummer}%' ";
				}
				if(SearchValueUsername != "" && SearchValueUsername != null)
				{
					query += $@" and L.Username Like '{SearchValueUsername}%' ";
				}

				if(ListSearchType != null && ListSearchType.Count > 0)
				{

					query += " and ( ";
					query += $@" A.Typ IN ({string.Join(",", ListSearchType.Select(x => $"'{x}'").ToList())}) 
                    OR L.LogObject IN ({string.Join(",", ListSearchType.Select(x => $"'{x}'").ToList())})";
					//if(ListSearchType.Count > 1)
					//{

					//	foreach(var item in ListSearchType.Where((value, index) => index > 0))
					//	{
					//		query += $@" or A.Typ Like '{item}%' ";
					//	}
					//}

					query += ") ";
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);
				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var v) ? v : 0;
			}
		}
		public static List<string> getAutoComplete(string columnName, string searchValue)
		{

			var result = new List<string>();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT DISTINCT Top 50 {columnName} AS [Output] 
                              FROM [__CTS_OrderProcesssing_Log] L
                              INNER JOIN Angebote A ON L.Nr = A.Nr
                              LEFT JOIN [angebotene Artikel] AR ON L.PositionNr = AR.Nr
                              LEFT JOIN Artikel ART ON AR.[Artikel-Nr] = ART.[Artikel-Nr]
                              WHERE L.[AngebotNr] <> 0 
                              AND {columnName} 
								";
				if(columnName == "L.Username")
				{
					query = query + $@" LIKE '%{searchValue}%'";
				}
				else
				{
					query = query + $@" LIKE '{searchValue}%'";
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);


				using(var reader = DbExecution.ExecuteReader(sqlCommand))
				{
					while(reader.Read())
					{
						result.Add(reader["Output"].ToString());
					}
				}
			}

			return result;

		}
		public static List<Infrastructure.Data.Entities.Joins.CRP.ForecastsExcelEntity> GetCRPDataForExcel(int? kundennummer, int? typeId, bool onlyLastVerion, DateOnly? dateFrom, DateOnly? dateTo)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var conditions = new List<string>();
				var __conditions = new List<string>();
				if(kundennummer is not null)
				{
					conditions.Add($"[kundennummer]={kundennummer}");
					__conditions.Add($"F.[kundennummer]={kundennummer}");
				}
				if(typeId is not null)
				{
					conditions.Add($"[TypeId]={typeId}");
					__conditions.Add($"F.[TypeId]={typeId}");
				}
				if(dateFrom.HasValue)
				{
					conditions.Add($"[Datum] >= '{dateFrom.Value.ToString("yyyyMMdd")}'");
				}
				if(dateTo.HasValue)
				{
					conditions.Add($"[Datum] <= '{dateTo.Value.ToString("yyyyMMdd")}'");
				}

				string query = $@"SELECT f.kunden,f.Type,p.Datum AS Bedarfstermin,p.Artikelnummer,p.Material,p.Menge,p.Jahr,p.KW,p.VKE,p.GesamtPreis,f.Datum
                                 FROM Forecasts F inner join  ForecastsPosition P ON F.Id=P.IdForcast 
								{(onlyLastVerion == true ? $"JOIN (SELECT MAX([Datum]) [Datum], Kundennummer FROM [Forecasts] {(conditions.Count > 0 ? $" WHERE {string.Join(" AND ", conditions)}" : "")} GROUP BY Kundennummer) as l on l.Kundennummer=F.Kundennummer AND l.Datum=F.Datum" : "")}
								{(__conditions.Count > 0 ? $" WHERE {string.Join(" AND ", __conditions)}" : "")}
                                {(__conditions.Count > 0 ? "AND" : "WHERE")} (p.IsOrdered=0 OR p.IsOrdered is null)";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CRP.ForecastsExcelEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Joins.CRP.CRPAuswertungRahmenFGArtikelEntity> GetCRPAuswertungRahmenFGArtikel()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT a.[Angebot-Nr],abe.BlanketTypeName,abe.CustomerName,aabe.Material AS Artikelnummer,aabe.[Bezeichnung],aa.OriginalAnzahl,aa.Anzahl,
								aabe.PreisDefault,aabe.GesamtpreisDefault,aabe.GultigBis,abe.StatusName
								from __CTS_AngeboteBlanketExtension abe 
								INNER JOIN __CTS_AngeboteArticleBlanketExtension aabe on abe.AngeboteNr=aabe.RahmenNr
								INNER JOIN [angebotene Artikel] aa on aa.Nr=aabe.AngeboteArtikelNr
								INNER JOIN Angebote a on a.Nr=aa.[Angebot-Nr]
								WHERE abe.BlanketTypeName='Sale'";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CRP.CRPAuswertungRahmenFGArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CRP.CRPAuswertungRahmenFGArtikelEntity>();
			}
		}

		public static List<KeyValuePair<int, string>> GetCustomersForFACreation()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select k.nummer,CONCAT(a.Kundennummer,' || ',a.Name1) Name1 from kunden k inner join adressen a on k.nummer=a.Nr
								where ISNULL([gesperrt für weitere Lieferungen],0)=0
								and a.Kundennummer is not null
								order by a.Name1";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(selectCommand: sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, string>(
					Convert.ToInt32(x["nummer"]),
					Convert.ToString(x["Name1"]))).ToList();
			}
			else
			{
				return null;
			}
		}
	}
}
