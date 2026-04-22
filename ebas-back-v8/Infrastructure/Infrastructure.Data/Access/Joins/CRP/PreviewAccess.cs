using Infrastructure.Data.Entities.Joins.CRP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Joins.CRP
{
	public class PreviewAccess
	{
		public static List<PreviewQuantitiesEntity> GetQuantitiesByArticleId(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT 
									COALESCE(b.articleId, c.articleId, p.articleId, f.articleId) AS ArticleId,
									COALESCE(b.year, c.year, p.year, f.year) AS Year,
									COALESCE(b.week, c.week, p.week, f.week) AS Week,
									COALESCE(b.quantity, 0) AS abQuantity,
									COALESCE(c.quantity, 0) AS fcQuantity,
									COALESCE(p.quantity, 0) AS lpQuantity,
									COALESCE(f.quantity, 0) AS FaQuantity
								FROM [stats].CrpPreviewAb b
								FULL OUTER JOIN [stats].CrpPreviewFc c
									ON b.articleId = c.articleId AND b.year = c.year AND b.week = c.week
								FULL OUTER JOIN [stats].CrpPreviewLp p
									ON COALESCE(b.articleId, c.articleId) = p.articleId 
									   AND COALESCE(b.year, c.year) = p.year
									   AND COALESCE(b.week, c.week) = p.week
								FULL OUTER JOIN [stats].CrpPreviewFa f
									ON COALESCE(b.articleId, c.articleId, p.articleId) = f.articleId 
									   AND COALESCE(b.year, c.year, p.year) = f.year
									   AND COALESCE(b.week, c.week, p.week) = f.week
								Where COALESCE(b.articleId, c.articleId, p.articleId, f.articleId)=@id 
								AND GETDATE() <= DATEADD(DAY, 6, DATEADD(WEEK, COALESCE(b.week, c.week, p.week, f.week) - 1, DATEADD(DAY, -DATEPART(WEEKDAY, DATEFROMPARTS(COALESCE(b.year, c.year, p.year, f.year), 1, 4)) + 1, DATEFROMPARTS(COALESCE(b.year, c.year, p.year, f.year), 1, 4)))) AND  DATEADD(WEEK, COALESCE(b.week, c.week, p.week, f.week) - 1, DATEADD(DAY, -DATEPART(WEEKDAY, DATEFROMPARTS(COALESCE(b.year, c.year, p.year, f.year), 1, 4)) + 1, DATEFROMPARTS(COALESCE(b.year, c.year, p.year, f.year), 1, 4)))<=DATEADD(Year, 1, GETDATE())
								ORDER BY year, week, articleId;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new PreviewQuantitiesEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static PreviewQuantitiesEntity GetQuantitiesByArticleId_Backlog(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT 
									COALESCE(b.articleId, c.articleId, p.articleId, f.articleId) AS ArticleId, 0 as Year, 0 as Week,
									SUM(COALESCE(b.quantity, 0)) AS abQuantity,
									SUM(COALESCE(c.quantity, 0)) AS fcQuantity,
									SUM(COALESCE(p.quantity, 0)) AS lpQuantity,
									SUM(COALESCE(f.quantity, 0)) AS FaQuantity
								FROM [stats].CrpPreviewAb b
								FULL OUTER JOIN [stats].CrpPreviewFc c
									ON b.articleId = c.articleId AND b.year = c.year AND b.week = c.week
								FULL OUTER JOIN [stats].CrpPreviewLp p
									ON COALESCE(b.articleId, c.articleId) = p.articleId 
										AND COALESCE(b.year, c.year) = p.year
										AND COALESCE(b.week, c.week) = p.week
								FULL OUTER JOIN [stats].CrpPreviewFa f
									ON COALESCE(b.articleId, c.articleId, p.articleId) = f.articleId 
										AND COALESCE(b.year, c.year, p.year) = f.year
										AND COALESCE(b.week, c.week, p.week) = f.week
								Where COALESCE(b.articleId, c.articleId, p.articleId, f.articleId)=@id 
								AND GETDATE() > DATEADD(DAY, 6, DATEADD(WEEK, COALESCE(b.week, c.week, p.week, f.week) - 1, DATEADD(DAY, -DATEPART(WEEKDAY, DATEFROMPARTS(COALESCE(b.year, c.year, p.year, f.year), 1, 4)) + 1, DATEFROMPARTS(COALESCE(b.year, c.year, p.year, f.year), 1, 4)))) 
								GROUP BY COALESCE(b.articleId, c.articleId, p.articleId, f.articleId);";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new PreviewQuantitiesEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<WeekEntitiesEntity> GetEntitiesByArticleIdAndWeek(string entityType, int id, int year, int week)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT DISTINCT EntityId, EntityNumber, CustomerNumber, IsManual, Quantity FROM [stats].[CrpPreviewEntities]
									WHERE EntityType=@entityType AND [ArticleId]=@id AND [Year]=@year AND [Week]=@week;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("week", week);
				sqlCommand.Parameters.AddWithValue("entityType", (entityType ?? "").SqlEscape());
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new WeekEntitiesEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<WeekEntitiesEntity> GetEntitiesByArticleIdBacklog(string entityType, int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT DISTINCT EntityId, EntityNumber, CustomerNumber, IsManual, Quantity, [Year], [Week]  FROM [stats].[CrpPreviewEntities]
									WHERE EntityType=@entityType AND [ArticleId]=@id AND ([Year]<YEAR(GETDATE()) OR ([Year]=YEAR(GETDATE()) AND [Week]<DATEPART(ISO_WEEK, GETDATE())));";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);
				sqlCommand.Parameters.AddWithValue("entityType", (entityType ?? "").SqlEscape());
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new WeekEntitiesEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<KeyValuePair<int, string>> GetArticleNumbers(string searchTerm, int page, int pageSize)
		{
			if(pageSize <= 0)
			{ pageSize = 25; }
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT DISTINCT [Artikel-Nr], Artikelnummer FROM [dbo].[Artikel]
									WHERE Warengruppe='EF' AND ISNULL(Aktiv,0)=1 AND [Artikelnummer] LIKE '{(searchTerm ?? "").SqlEscape().Trim()}%'
									ORDER BY Artikelnummer ASC
									OFFSET {page * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY;";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, string>(Convert.ToInt32(x[0]), x[1].ToString())).ToList();
			}
			else
			{
				return null;
			}
		}
		public static int UpdateSnapshot()
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("usp_crp_preview_snapshot", sqlConnection))
			{
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlConnection.Open();
				DbExecution.ExecuteScalar(sqlCommand);
				return 1;
			}
		}
	}
}
