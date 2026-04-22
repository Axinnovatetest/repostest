using iText.StyledXmlParser.Jsoup.Nodes;
using MoreLinq;
using Psz.Core.BaseData.Models.ProjectManagment;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.ProjectManagment
{
	public partial class ProjectManagmentService
	{
		public ResponseModel<List<ProjectLogsModel>> GetProjectsLogs(UserModel user)
		{
			if(user == null)
				return ResponseModel<List<ProjectLogsModel>>.AccessDeniedResponse();

			try
			{
				var entities = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_logsAccess.Get();
				var projects = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_ProjectsAccess.Get(entities?.Select(x => x.ProjectId ?? -1)?.ToList());
				var response = entities?.Select(x => new ProjectLogsModel
				{
					ProjectId = x.ProjectId ?? -1,
					ProjectName = projects.FirstOrDefault(y=>y.Id == x.ProjectId)?.ProjectName,
					LogMessage = x.LogText,
					Username = x.Username,
					Date = x.LogTime
				}).ToList();

				return ResponseModel<List<ProjectLogsModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<ProjectLogsModel>> GetProjectsLogsByProject(UserModel user, int prjectId)
		{
			if(user == null)
				return ResponseModel<List<ProjectLogsModel>>.AccessDeniedResponse();

			try
			{
				var entities = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_logsAccess.GetByProject(prjectId);
				var projects = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_ProjectsAccess.Get(entities.Select(p => p.ProjectId ?? -1).ToList());
				var response = entities.Select(x => new ProjectLogsModel
				{
					ProjectId = x.ProjectId ?? -1,
					ProjectName = projects.FirstOrDefault(p => p.Id == x.ProjectId)?.ProjectName,
					LogMessage = x.LogText,
					Username = x.Username,
					Date = x.LogTime
				}).ToList();

				return ResponseModel<List<ProjectLogsModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}