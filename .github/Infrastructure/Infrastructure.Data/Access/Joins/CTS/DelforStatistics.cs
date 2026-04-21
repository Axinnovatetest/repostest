using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Joins.CTS
{
	public class DelforStatistics
	{
		public static List<Infrastructure.Data.Entities.Joins.CTS.DLF_AnalysisEntity> GetAnalysis(bool? isManual, string customerNumber, string documentNumber, bool onlyLastVersion, bool onlyOpen, DateOnly? dateFrom, DateOnly? dateTo, string sortColumn, bool sortDesc, int currentPage = 0, int pageSize = 100)
		{
			customerNumber = customerNumber ?? "";
			documentNumber = documentNumber ?? "";
			List<string> whereClauses = new List<string>();
			if(pageSize == 0)
				pageSize = 100;
			if(string.IsNullOrWhiteSpace(sortColumn))
				sortColumn = "BuyerPartyName";

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT h.ConsigneePartyName, h.BuyerPartyName, h.DocumentNumber, li.PositionNumber, lp.PlanningQuantityRequestedShipmentDate, li.SuppliersItemMaterialNumber, 
					a.[Bezeichnung 1] AS [Bezeichnung1],  lp.PlanningQuantityQuantity, lp.PlanningQuantityCumulativeQuantity, Year(lp.PlanningQuantityRequestedShipmentDate) AS RSDYear, 
					DATEPART(ISO_WEEK, lp.PlanningQuantityRequestedShipmentDate) AS RSDWeek, 
					(a.[Einzelkupferzuschlag]*a.[Preiseinheit]+ ISNULL(p.[Verkaufspreis],0)) AS UnitPrice, 
					((ISNULL(a.[Einzelkupferzuschlag],0)*ISNULL(a.[Preiseinheit],0)+ ISNULL(p.[Verkaufspreis],0))/(CASE WHEN ISNULL(a.Preiseinheit,0)=0 THEN 1 ELSE a.Preiseinheit END)*ISNULL(lp.PlanningQuantityQuantity,0)) AS TotalPrice, 
					li.LastASNNumber, li.LastReceivedQuantity, 
					li.LastASNDate, ReceivingDate, ISNULL(ab.AbTotalQty,0) AS AbTotalQty,
                    CONCAT(ConsigneePartyName,' | ',ConsigneeStreet,' | ',ConsigneePostCode,' ',ConsigneeCity) AS Lieferadresse
					FROM {(onlyLastVersion ? @"(SELECT m.* FROM [__EDI_DLF_Header] m 
													Join (SELECT [PSZCustomernumber],[DocumentNumber], MAX([ReferenceVersionNumber]) [ReferenceVersionNumber] 
													FROM [__EDI_DLF_Header] GROUP BY [PSZCustomernumber],[DocumentNumber],ISNULL([Done],0)) n 
													on n.[PSZCustomernumber]=m.[PSZCustomernumber] 
														AND n.DocumentNumber=m.DocumentNumber 
														AND n.ReferenceVersionNumber=m.ReferenceVersionNumber
												)" : "[__EDI_DLF_Header]")} AS h
					Join [__EDI_DLF_LineItem] li on li.HeaderId=h.Id
					Join [__EDI_DLF_LineItemPlan] lp on lp.LineItemId=li.Id
					LEFT Join (SELECT Round(IIf([VK-Festpreis] = 0, ((DEL * 1.01) - Kupferbasis) / 100 * [Cu-Gewicht], 0), 2) AS [Einzelkupferzuschlag],* FROM Artikel) a on a.Artikelnummer=li.SuppliersItemMaterialNumber
					LEFT Join [Preisgruppen] AS p on p.[Artikel-Nr]=a.[Artikel-Nr]
					Left Join (SELECT a.nr_dlf, SUM(ap.[OriginalAnzahl]) as AbTotalQty FROM Angebote a join [angebotene Artikel] ap on a.Nr=ap.[Angebot-Nr] WHERE ISNULL(a.nr_dlf,0)<>0 GROUP BY a.nr_dlf) ab on ab.nr_dlf=lp.Id
					
					";
				if(onlyOpen)
				{
					whereClauses.Add("IsNULL([Done],0) = 0");
				}
				if(dateFrom.HasValue)
				{
					whereClauses.Add($"[ReceivingDate] >= '{dateFrom.Value.ToString("yyyyMMdd")}'");
				}
				if(dateTo.HasValue)
				{
					whereClauses.Add($"[ReceivingDate] <= '{dateTo.Value.ToString("yyyyMMdd")}'");
				}
				if(isManual.HasValue)
				{
					whereClauses.Add($"IsNULL([ManualCreation],0) = {(isManual.Value ? "1" : "0")}");
				}
				if(!string.IsNullOrWhiteSpace(customerNumber))
				{
					whereClauses.Add($"[PSZCustomernumber] = '{customerNumber.SqlEscape()}'");
				}
				if(!string.IsNullOrWhiteSpace(documentNumber))
				{
					whereClauses.Add($"h.[DocumentNumber] LIKE '{documentNumber.SqlEscape()}%'");
				}
				if(whereClauses.Count>0)
				{
					query += $" WHERE {string.Join(" AND ", whereClauses)}";
				}

				// query += $" ORDER BY {sortColumn} {(sortDesc ? "DESC" : "ASC")}";
				query += $" ORDER BY h.BuyerPartyName, h.DocumentNumber, li.PositionNumber, lp.PlanningQuantityRequestedShipmentDate";

				// - negative pageSize retrieves all data
				if(pageSize >= 0)
					query += $" OFFSET {currentPage * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 660;
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.CTS.DLF_AnalysisEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.CTS.DLF_AnalysisEntity>();
			}
		}
		public async static Task<int> GetAnalysis_count(bool? isManual, string customerNumber, string documentNumber, bool onlyLastVersion, bool onlyOpen, DateOnly? dateFrom, DateOnly? dateTo)
		{
			customerNumber = customerNumber ?? "";
			documentNumber = documentNumber ?? "";
			List<string> whereClauses = new List<string>();

			var dataTable = new DataTable();
			return await Task<int>.Factory.StartNew(() =>
			{
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $@"SELECT COUNT(*) AS nb
					FROM {(onlyLastVersion ? @"(SELECT m.* FROM [__EDI_DLF_Header] m 
													Join (SELECT [PSZCustomernumber],[DocumentNumber], MAX([ReferenceVersionNumber]) [ReferenceVersionNumber] 
													FROM [__EDI_DLF_Header] GROUP BY [PSZCustomernumber],[DocumentNumber],ISNULL([Done],0)) n 
													on n.[PSZCustomernumber]=m.[PSZCustomernumber] 
														AND n.DocumentNumber=m.DocumentNumber 
														AND n.ReferenceVersionNumber=m.ReferenceVersionNumber
												)" : "[__EDI_DLF_Header]")} AS h
					Join [__EDI_DLF_LineItem] li on li.HeaderId=h.Id
					Join [__EDI_DLF_LineItemPlan] lp on lp.LineItemId=li.Id
					LEFT Join Artikel a on a.Artikelnummer=li.SuppliersItemMaterialNumber
					LEFT Join [Preisgruppen] AS p on p.[Artikel-Nr]=a.[Artikel-Nr]
					
					";
					if(onlyOpen)
					{
						whereClauses.Add("IsNULL([Done],0) = 0");
					}
					if(dateFrom.HasValue)
					{
						whereClauses.Add($"[ReceivingDate] >= '{dateFrom.Value.ToString("yyyyMMdd")}'");
					}
					if(dateTo.HasValue)
					{
						whereClauses.Add($"[ReceivingDate] <= '{dateTo.Value.ToString("yyyyMMdd")}'");
					}
					if(isManual.HasValue)
					{
						whereClauses.Add($"IsNULL([ManualCreation],0) = {(isManual.Value ? "1" : "0")}");
					}
					if(!string.IsNullOrWhiteSpace(customerNumber))
					{
						whereClauses.Add($"[PSZCustomernumber] = '{customerNumber.SqlEscape()}'");
					}
					if(!string.IsNullOrWhiteSpace(documentNumber))
					{
						whereClauses.Add($"h.[DocumentNumber] LIKE '{documentNumber.SqlEscape()}%'");
					}
					if(whereClauses.Count > 0)
					{
						query += $" WHERE {string.Join(" AND ", whereClauses)}";
					}

					var sqlCommand = new SqlCommand(query, sqlConnection);
					DbExecution.Fill(sqlCommand, dataTable);
					return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
				}
			});
		}
	}
}
