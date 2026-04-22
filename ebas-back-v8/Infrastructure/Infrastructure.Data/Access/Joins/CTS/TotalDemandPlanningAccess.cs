using Infrastructure.Data.Entities.Joins.CTS;

namespace Infrastructure.Data.Access.Joins.CTS
{
	public class TotalDemandPlanningAccess
	{
		public static List<CustomerSummaryEntity> Get(string inputFilter, DateTime startDate, DateTime endDate, string documentType, Settings.PaginModel paging = null, Settings.SortingModel sorting = null)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"DECLARE @start date = DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0); " +
					$"DECLARE @end date = DATEADD(DAY, 6, DATEADD(WEEK, 8, DATEADD(DAY, 1 - DATEPART(WEEKDAY, GETDATE()), CAST(GETDATE() AS DATE)))); " +
					$"DECLARE @inputFilter NVARCHAR(100) = '{inputFilter}';" +
					$"SELECT  [CustomerName],[DocumentType],[DocumentNumber],[DocumentAngebotNr],a.Artikelnummer [ArticleNumber], a.[Artikel-Nr] [ArticleId]," +
					$" a.[Bezeichnung 1] [ArticleDesignation],[OpenQuantity],[FANumber],[Date],DATEPART(ISO_WEEK, [Date]) [Week]," +
					$" YEAR([Date]) [Year],[UnitPrice],[TotalPrice],[AngeboteNR],[CustomerId],COUNT(*) OVER() AS TotalCount FROM (SELECT a.[Vorname/NameFirma] [CustomerName]," +
					$" a.Bezug [DocumentNumber],a.Typ [DocumentType],a.[Angebot-Nr] [DocumentAngebotNr]," +
					$" p.Anzahl [OpenQuantity],p.Fertigungsnummer [FANumber],p.Liefertermin [Date]," +
					$" p.Einzelpreis [UnitPrice],p.Anzahl*p.Einzelpreis [TotalPrice],p.[Artikel-Nr] [ArticleId],a.[Nr] [AngeboteNR], [Kunden-Nr] [CustomerId] " +
					$" FROM Angebote A INNER JOIN [angebotene Artikel] p ON a.Nr=p.[Angebot-Nr]" +
					$" WHERE (a.Typ='Auftragsbestätigung' OR a.Typ='Kanban')AND a.gebucht=1 AND ISNULL(a.erledigt,0)=0 " +
					$" AND ISNULL(p.erledigt_pos,0)=0 AND @start <= p.Liefertermin AND p.Liefertermin <= @end UNION ALL/*- LP + Forecasts -*/ " +
					$" SELECT  h.BuyerPartyName [CustomerName],h.DocumentNumber [DocumentNumber],CASE WHEN h.ManualCreation=1 " +
					$" THEN 'DELFOR [MANUAL]' ELSE 'DELFOR [EDI]' END [DocumentType],0 [DocumentAngebotNr]," +
					$" lp.PlanningQuantityQuantity [OpenQuantity],0 [FANumber],lp.PlanningQuantityRequestedShipmentDate [Date]," +
					$" p.Verkaufspreis [UnitPrice],lp.PlanningQuantityQuantity * p.Verkaufspreis [TotalPrice]," +
					$" li.ArticleId [ArticleId],h.Id [AngeboteNR], h.PSZCustomernumber [CustomerId] from (SELECT PSZCustomernumber, DocumentNumber, " +
					$" MAX(ReferenceVersionNumber) ReferenceVersionNumber    FROM __EDI_DLF_Header  " +
					$" WHERE ISNULL(Done,0)=0 GROUP BY PSZCustomernumber, DocumentNumber) AS u    " +
					$" INNER JOIN __EDI_DLF_Header h on h.PSZCustomernumber=u.PSZCustomernumber AND  h.ReferenceVersionNumber=u.ReferenceVersionNumber " +
					$" AND h.DocumentNumber=u.DocumentNumber    INNER JOIN __EDI_DLF_LineItem li on li.HeaderId=h.Id    " +
					$" INNER JOIN __EDI_DLF_LineItemPlan lp on lp.LineItemId=li.Id    " +
					$" INNER JOIN Preisgruppen p on p.[Artikel-Nr]=li.ArticleId WHERE @start <= [PlanningQuantityRequestedShipmentDate] AND [PlanningQuantityRequestedShipmentDate] <= @end ) as x " +
					$" JOIN Artikel a on a.[Artikel-Nr]=x.ArticleId" +
					$" WHERE DATEPART(ISO_WEEK, GETDATE())<=DATEPART(ISO_WEEK, [Date])";


				if(inputFilter != null || inputFilter != "")
				{
					query += $" AND (x.[CustomerName] LIKE '%' + @inputFilter + '%' OR " +
					"x.[DocumentNumber] LIKE '%' + @inputFilter + '%' OR " +
					"x.[DocumentAngebotNr] LIKE '%' + @inputFilter + '%' OR " +
					"a.Artikelnummer LIKE @inputFilter + '%' OR " +
					"a.[Bezeichnung 1] LIKE  @inputFilter + '%' OR " +
					"x.[FANumber] LIKE '%' + @inputFilter + '%') ";
				}
				if(!string.IsNullOrWhiteSpace(documentType))
				{
					query += $" AND x.[DocumentType] ='{documentType}'";
				}

				query += $" AND '{startDate.ToString("yyyyMMdd")}'<=[Date] AND [Date]<='{endDate.ToString("yyyyMMdd")}'";

				#region >>>>> pagination <<<<<<<
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY x.[Date] DESC ";
				}

				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				#endregion pagination


				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandText = query;
				sqlCommand.Connection = sqlConnection;
				sqlCommand.CommandTimeout = 240; //in seconds
				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new CustomerSummaryEntity(x)).ToList();
			}
			else
			{
				return new List<CustomerSummaryEntity>();
			}
		}
	}
}

