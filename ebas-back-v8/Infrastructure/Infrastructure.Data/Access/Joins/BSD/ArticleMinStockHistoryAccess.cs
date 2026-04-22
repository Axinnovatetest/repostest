
namespace Infrastructure.Data.Access.Joins.BSD
{
	using Entities.Joins;

	public class ArticleMinStockHistoryAccess
	{

		public static List<MinStockAnalysisEntity> getMinStockDetailedAnalysis(string articleNumber, Settings.SortingModel sorting, Settings.PaginModel paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT b.[Artikel-Nr] AS[ArticleNr],b.Artikelnummer,[Mindestbestand] ,[Bestand], " +
				"ISNULL(artEr.VK_CU150, 0)[Vkmitcu150Einzelpreis], " +
				"(ISNULL(artEr.VK_CU150, 0) * [Mindestbestand])[Vkmitcu150MindestbestandGesamtpreis], " +
				"ISNULL(artEr.VK_PSZ_ink_Kupfer, 0) as [VkmitcuEinzelpreis], " +
				"(ISNULL(artEr.VK_PSZ_ink_Kupfer, 0) * Mindestbestand) as [VkmitcuMindestbestandGesamtpreis]," +
				 "ISNULL(artEr.[Kalkulatorische kosten], 0)[Vkcu150Herstellkosten]," +
				"ISNULL(artEr.[Kalkulatorische kosten], 0) [VkCuDelHerstellkosten]," +
				" (ISNULL(artEr.[Kalkulatorische kosten], 0) * [Mindestbestand]) as [GesamtpreisHerstellkosten150]," +
				"(ISNULL(artEr.[Kalkulatorische kosten], 0) * [Mindestbestand]) as [GesamtpreisHerstellkostenDel]," +
				"0 AS TotalCount /* see count method */" +
				"FROM(SELECT a.[Artikel-Nr], Artikelnummer, l.Lagerort_id, " +
				"SUM(ISNULL(l.Mindestbestand, 0))[Mindestbestand], SUM(ISNULL(l.Bestand, 0))[Bestand] " +
				"FROM[Artikel] a JOIN Lager l on l.[Artikel-Nr] = a.[Artikel-Nr] " +
				" JOIN Lagerorte la on l.Lagerort_id = la.Lagerort_id " +
				" WHERE Warengruppe<>'ROH' AND ISNULL(l.Mindestbestand, 0) <> 0 AND la.Lagerort LIKE 'Hauptlager%' ";

				if(!string.IsNullOrEmpty(articleNumber))
				{
					query += $" AND Artikelnummer LIKE '{articleNumber}%'";
				}

				query += $" GROUP BY a.[Artikel-Nr], Artikelnummer, l.Lagerort_id) AS b " +
				" left JOIN[View_PSZ_steinbacher Marge berechnung alle Artikel ergebniss] artEr " +
				" on artEr.Artikelnummer = b.Artikelnummer";

				#region >>>>> pagination <<<<<<<
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}

				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				#endregion pagination

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 240; //in seconds


				DbExecution.Fill(sqlCommand, dataTable);
			}


			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new MinStockAnalysisEntity(x)).ToList();
			}
			else
			{
				return new List<MinStockAnalysisEntity>();
			}
		}
		public static int getMinStockDetailedAnalysis_Count(string articleNumber)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT COUNT(DISTINCT b.[Artikel-Nr])" +
				"FROM(SELECT a.[Artikel-Nr], Artikelnummer, l.Lagerort_id, " +
				"SUM(ISNULL(l.Mindestbestand, 0))[Mindestbestand], SUM(ISNULL(l.Bestand, 0))[Bestand] " +
				"FROM[Artikel] a JOIN Lager l on l.[Artikel-Nr] = a.[Artikel-Nr] " +
				" JOIN Lagerorte la on l.Lagerort_id = la.Lagerort_id " +
				" WHERE Warengruppe<>'ROH' AND ISNULL(l.Mindestbestand, 0) <> 0 AND la.Lagerort LIKE 'Hauptlager%' ";

				if(!string.IsNullOrEmpty(articleNumber))
				{
					query += $" AND Artikelnummer LIKE '{articleNumber}%'";
				}

				query += $" GROUP BY a.[Artikel-Nr], Artikelnummer, l.Lagerort_id) AS b " +
				" left JOIN[View_PSZ_steinbacher Marge berechnung alle Artikel ergebniss] artEr " +
				" on artEr.Artikelnummer = b.Artikelnummer";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand)?.ToString(), out var i) ? i : 0;
			}
		}
	}
}
