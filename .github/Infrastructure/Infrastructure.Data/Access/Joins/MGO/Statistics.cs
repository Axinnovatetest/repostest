using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Joins.MGO
{
	public class Statistics
	{
		public static List<Entities.Joins.MGO.BudgetSummaryEntity> GetBudgetSummary()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				string query = @"SELECT CASE WHEN b.CompanyId=1 THEN 'DE' 
										WHEN b.CompanyId=2 THEN 'CZ' 
										WHEN b.CompanyId=3 THEN 'TN' 
										WHEN b.CompanyId=4 THEN 'AL' 
										WHEN b.CompanyId=5 THEN 'WS' 
										WHEN b.CompanyId=7 THEN 'BETN'
										WHEN b.CompanyId=8 THEN 'GZ' 
										END [Site]
									, b.OrderNumber [PO Nummer], b.IssuerName Anfrager, b.DepartmentName Abteilung, b.SupplierName Lieferant,
									REPLACE(TotalCost,',', '.') [Betrag TND], REPLACE(TotalCostDefaultCurrency, ',', '.') [Betrag EUR],
									DateName( MONTH , DateAdd( MONTH , DATEPART(MONTH,b.CreationDate) , -1 ) ) [Monat], DATEPART(ISO_WEEK,b.CreationDate) [KW] 
									FROM __FNC_BestellungenExtension b
									JOIN (SELECT OrderId, SUM(TotalCost) TotalCost, SUM(TotalCostDefaultCurrency) TotalCostDefaultCurrency FROM __FNC_BestellteArtikelExtension GROUP BY OrderId) p on p.OrderId=b.OrderId
									WHERE YEAR(b.CreationDate)=2024 AND b.ApprovalTime IS NOT NULL AND b.ApprovalUserId IS NOT NULL
									ORDER BY b.CompanyId, b.CreationDate";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 360;
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MGO.BudgetSummaryEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MGO.BudgetSummaryEntity>();
			}
		}
		public static List<Entities.Joins.MGO.RawMaterialStockValueEntity> GetRawMaterialStockValue_GZ()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"SELECT a.Warengruppe, la.Lagerort,
								CASE WHEN a.Warengruppe='ef' THEN 
										REPLACE(CAST(SUM(CASE WHEN LEFT(TRIM(la.Lagerort),2)<>'PL' THEN l.bestand WHEN LEFT(TRIM(la.Lagerort),2)<>'PL' AND a.Warentyp<>2 THEN l.Bestand ELSE l.GesamtBestand END * s.[Verkaufspreis]) AS DECIMAL(20, 3)),',','.') 
									ELSE REPLACE(CAST(SUM(CASE WHEN LEFT(TRIM(la.Lagerort),2)<>'PL' THEN l.bestand WHEN LEFT(TRIM(la.Lagerort),2)<>'PL' AND a.Warentyp<>2 THEN l.Bestand ELSE l.GesamtBestand END * b.Einkaufspreis) AS DECIMAL(20, 3)),',','.') 
									END [Bestandswert (Summe EUR)]
								/*, CASE WHEN a.Warengruppe='ef' THEN REPLACE(SUM(l.mindestbestand*s.[Verkaufspreis]),',','.') ELSE REPLACE(SUM(l.mindestbestand*b.Einkaufspreis),',','.') END [Mindestbestandswert (Summe EUR)]*/
								FROM Lager l 
								join Lagerorte la on la.lagerort_id=l.lagerort_id
								join Artikel a on a.[Artikel-Nr]=l.[Artikel-Nr]
								left join __BSD_ArtikelSalesExtension s on s.[ArticleNr]=a.[Artikel-Nr]
								left join (SELECT [Artikel-Nr], Einkaufspreis from Bestellnummern where Standardlieferant=1) b on b.[Artikel-Nr]=a.[Artikel-Nr]
								Where (((a.Warengruppe='ef' and ISNULL(l.bestand,0)>0)or(a.Warengruppe<>'ef' and ISNULL(l.bestand,0)<>0)) OR ISNULL(l.mindestbestand,0)<>0)
								AND s.[ArticleSalesType]='serie'
								and l.Lagerort_id in (102, 103)
								and a.Warengruppe='roh'
								GROUP BY a.Warengruppe, la.Lagerort,la.Lagerort_id
								ORDER BY la.Lagerort";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 360;
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MGO.RawMaterialStockValueEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MGO.RawMaterialStockValueEntity>();
			}
		}
		public static List<Entities.Joins.MGO.ProductionWorkload_WeekFa> GetProductionWorkload_WeekFas(int warehouseId, int week, int year, bool isBacklog = false)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"DECLARE @syncId INT;
								SELECT @syncId=ISNULL(MAX(ISNULL(RecordSyncId,0)),0) FROM [__MGO_ProductionWorkload];
								SELECT w.FaId, f.Fertigungsnummer FaNumber, f.Datum FaCreationTime, w.FaProductionTime,
								a.[Artikel-Nr] ArticleId, a.Artikelnummer ArticleNumber, b.Nr OrderId, b.[Angebot-Nr] OrderNumber
								FROM [__MGO_ProductionWorkloadWeekFas] w 
								LEFT JOIN Fertigung f on f.ID=w.FaId
								LEFT JOIN Artikel a on a.[Artikel-Nr]=f.Artikel_Nr
								LEFT JOIN [angebotene Artikel] p on p.Fertigungsnummer=f.Fertigungsnummer
								LEFT JOIN Angebote b on b.Nr=p.[Angebot-Nr]
								WHERE w.RecordSyncId=@syncId AND WarehouseId={warehouseId} AND 
								{(isBacklog ? $"CAST(DATEADD(DAY, (FaWeek - 1) * 7, DATEADD(WEEK, DATEDIFF(WEEK, 0, DATEFROMPARTS(FaYear, 1, 4)), 0)) AS DATE) < CAST(DATEADD(DAY,-(DATEPART(dw,GETDATE())-1),GETDATE()) AS DATE)" : $"w.FaYear={year} AND w.FaWeek={week}")}
								";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 360;
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MGO.ProductionWorkload_WeekFa(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MGO.ProductionWorkload_WeekFa>();
			}
		}
	}
}
