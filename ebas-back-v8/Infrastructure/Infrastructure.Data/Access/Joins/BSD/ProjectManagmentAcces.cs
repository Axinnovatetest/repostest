using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System;

namespace Infrastructure.Data.Access.Joins.BSD
{
	public class ProjectManagmentAcces
	{
		public static List<Infrastructure.Data.Entities.Joins.BSD.ProjectTasksEntity> GetProjectsCables(int projectId)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"select P.Id as ProjectId,C.Id as CableId,C.ArticleId,C.ArticleNumber,C.ArticleCustomerNumber,
                                 --T.Id AS TaskId,T.CurrentTaskName,
                                 C.ResponsibleUsername,/*T.StartDate,T.Deadline,*/P.PMManagerUsername,P.CreationTime as CreationDate,C.Status
                                 from __bsd_pm_Projects P inner join __bsd_pm_Cables C on P.Id=C.ProjectId
                                 --left join __bsd_pm_CurrentTask T on P.Id=T.ProjectId and C.Id=T.CableId 
                                 WHERE P.Id=@projectId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("projectId", projectId);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.BSD.ProjectTasksEntity(x))?.ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.BSD.ProjectTasksEntity>();
			}
		}
		public static List<KeyValuePair<string, int>> GetProjectByStatus()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"select [Status],COUNT([Status]) as StatusCount from __bsd_pm_Projects group by [Status]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<string, int>(
					Convert.ToString(x["Status"]),
					Convert.ToInt32(x["StatusCount"])
					))?.ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<KeyValuePair<string, int>> GetProjectByTasksStatus()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"select COUNT(Status) AS StatusCount,Status from __bsd_pm_Cables GROUP BY Status";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<string, int>(
					Convert.ToString(x["Status"]),
					Convert.ToInt32(x["StatusCount"])
					))?.ToList();
			}
			else
			{
				return null;
			}
		}
		public static Tuple<int, int, int> GetProjectByTime()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"SELECT 
								SUM(CASE WHEN DeliveryDate < GETDATE() THEN 1 ELSE 0 END) AS LateProjects,
								SUM(CASE WHEN DeliveryDate >= GETDATE() THEN 1 ELSE 0 END) AS OnTimeProjects,
								SUM(CASE WHEN [Status] ='Closed' THEN 1 ELSE 0 END) AS ClosedProjects
							    FROM 
								__bsd_pm_Projects";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return new Tuple<int, int, int>(
					(dataTable.Rows[0]["OnTimeProjects"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataTable.Rows[0]["LateProjects"]),
					(dataTable.Rows[0]["OnTimeProjects"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataTable.Rows[0]["OnTimeProjects"]),
					(dataTable.Rows[0]["ClosedProjects"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataTable.Rows[0]["ClosedProjects"])
					);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetArticleOpenOrders(int articleNr)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = @"select * from Fertigung where artikel_nr=@articleNr and (Erstmuster=1 or Technik=1)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleNr", articleNr);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.FertigungEntity(x))?.ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
			}
		}
	}
}