using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
namespace Infrastructure.Data.Access.Joins.MTM.CRP
{
	public class ArticleFaTimeAccess
	{
		public static List<Entities.Joins.MTM.CRP.ArticleFaTimeEntity> GetArticlesFaTimeDiff(int? lager)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "";

				// - Q1 
				query += "IF OBJECT_ID('tempdb..#__Temp_Analyse_CRP_Time') IS NOT NULL DROP TABLE #__Temp_Analyse_CRP_Time;";
				query += "IF OBJECT_ID('tempdb..#temp_Analyse_crp') IS NOT NULL DROP TABLE #temp_Analyse_crp;";
				query += "IF OBJECT_ID('tempdb..#temp_Analyse_crp_ap') IS NOT NULL DROP TABLE #temp_Analyse_crp_ap;";

				// - Q2 - 
				query += "SELECT A.[Artikel-Nr] ArticleNr, A.Artikelnummer, F.ID FaNr, F.Fertigungsnummer, F.Originalanzahl, F.Lagerort_id, F.Termin_Bestätigt1, F.Zeit, ";
				query += "F.Originalanzahl * F.Zeit / 60 Total_Hr_W27, F.Artikel_Nr ";
				query += " INTO #__Temp_Analyse_CRP_Time";
				query += " FROM Fertigung F JOIN Artikel A ON A.[Artikel-Nr]=F.Artikel_Nr ";
				query += $" WHERE {(lager.HasValue ? $"F.Lagerort_id={lager.Value} AND " : "")}F.Kennzeichen='offen'";
				query += " ORDER BY F.Termin_Bestätigt1, Artikel_Nr ASC;";

				// - Q3 - 
				query += " SELECT ArticleNr, FaNr, Artikel_Nr, Artikelnummer, Fertigungsnummer, Originalanzahl Total_Qty, Total_Hr_W27";
				query += " INTO #temp_Analyse_crp ";
				query += " FROM #__Temp_Analyse_CRP_Time A;";

				// - Q4 - 
				query += " SELECT A.FaNr,";
				query += " SUM(";
				query += "   CASE";
				query += "     WHEN Wd.RelationOperationTime=1 THEN";
				query += "       Wd.Amount * Wd.OperationTimeSeconds / 60 + Wd.SetupTimeMinutes/NULLIF(A.Total_Qty,0)";
				query += "     ELSE";
				query += "       Wd.Amount * Wd.OperationTimeSeconds / 60 / NULLIF(A.Total_Qty,0) + Wd.SetupTimeMinutes/NULLIF(A.Total_Qty,0)";
				query += "   END";
				query += " ) AS ArticleOperationTime";
				query += " INTO #temp_Analyse_crp_ap";
				query += " FROM #temp_Analyse_crp A";
				query += " LEFT JOIN dbo.WorkSchedule Ws ON Ws.Article_Id = A.ArticleNr";
				query += " LEFT JOIN dbo.WorkScheduleDetails Wd ON Wd.WorkScheduleId = Ws.Id";
				query += " WHERE Ws.Is_Active=1";
				query += " GROUP BY A.FaNr;";

				// - Q5 - final select
				query += " SELECT A.ArticleNr, A.FaNr, A.Artikelnummer AS ArticleNumber, A.Fertigungsnummer AS FaNumber, ";
				query += " A.Total_Qty [FaQuantity], A.Total_Hr_W27 [FaTime], B.ArticleOperationTime * A.Total_Qty / 60 AS [ApTime]";
				query += " FROM #temp_Analyse_crp A JOIN #temp_Analyse_crp_ap B ON B.FaNr=A.FaNr";
				query += " ORDER BY Artikelnummer;";

				//// - Q1 - drop temp tables
				//query += "IF OBJECT_ID('tempdb..#__Temp_Analyse_CRP_Time') IS NOT NULL DROP TABLE #__Temp_Analyse_CRP_Time;";
				//query += "IF OBJECT_ID('tempdb..#temp_Analyse_crp') IS NOT NULL DROP TABLE #temp_Analyse_crp;";
				//query += "IF OBJECT_ID('tempdb..#temp_Analyse_crp_ap') IS NOT NULL DROP TABLE #temp_Analyse_crp_ap;";

				//// - Q2 - 
				//query += "SELECT A.[Artikel-Nr] ArticleNr, A.Artikelnummer, F.ID FaNr, F.Fertigungsnummer, F.Originalanzahl, F.Lagerort_id, F.Termin_Bestätigt1, F.Zeit, F.Originalanzahl * F.Zeit / 60 Total_Hr_W27, F.Artikel_Nr ";
				//query += " INTO #__Temp_Analyse_CRP_Time";
				//query += "  FROM Fertigung F JOIN Artikel A ON A.[Artikel-Nr]=F.Artikel_Nr ";
				//query += $" WHERE {(lager.HasValue ? $"F.Lagerort_id={lager.Value} AND " : "")}F.Kennzeichen='offen'";
				//query += " order by F.Termin_Bestätigt1, Artikel_Nr asc;";

				//// - Q3 -
				//query += " SELECT ArticleNr, FaNr, Artikel_Nr, Artikelnummer, Fertigungsnummer, Originalanzahl Total_Qty, Total_Hr_W27 Total_Hr_W27";
				//query += " INTO #temp_Analyse_crp ";
				//query += " FROM #__Temp_Analyse_CRP_Time A;";

				//// - Q4 -
				//query += " SELECT A.FaNr,";
				//query += " SUM(";
				//query += " CASE";
				//query += " WHEN Wd.RelationOperationTime=1 THEN";
				//query += " Wd.Amount * Wd.OperationTimeSeconds / 60  + Wd.SetupTimeMinutes/A.Total_Qty";
				//query += " ELSE";
				//query += " Wd.Amount * Wd.OperationTimeSeconds / 60 / A.Total_Qty  + Wd.SetupTimeMinutes/A.Total_Qty";
				//query += " END) AS ArticleOperationTime";
				//query += " INTO #temp_Analyse_crp_ap";
				//query += " FROM #temp_Analyse_crp A";
				//query += " JOIN dbo.Article AA ON AA.[Name]=A.Artikelnummer";
				//query += " LEFT JOIN dbo.WorkSchedule Ws ON Ws.Article_Id=AA.Id";
				//query += " LEFT JOIN dbo.WorkScheduleDetails Wd ON Wd.WorkScheduleId=Ws.Id";
				//query += " WHERE Ws.Is_Active=1";
				//query += " GROUP BY A.FaNr;";

				//// - Q5 -
				//query += " SELECT A.ArticleNr, A.FaNr, A.Artikelnummer AS ArticleNumber, A.Fertigungsnummer AS FaNumber, A.Total_Qty [FaQuantity], A.Total_Hr_W27 [FaTime], B.ArticleOperationTime * A.Total_Qty / 60 AS [ApTime]";
				//query += " FROM #temp_Analyse_crp A JOIN #temp_Analyse_crp_ap B ON B.FaNr=A.FaNr";
				//query += " ORDER BY Artikelnummer;";


				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.MTM.CRP.ArticleFaTimeEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.MTM.CRP.ArticleFaTimeEntity>();
			}
		}
	}
}
