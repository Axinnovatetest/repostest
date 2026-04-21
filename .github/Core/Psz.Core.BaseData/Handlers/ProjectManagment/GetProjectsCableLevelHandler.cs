using Psz.Core.BaseData.Models.ProjectManagment;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.ProjectManagment
{
	public partial class ProjectManagmentService
	{
		public ResponseModel<List<ProjectCableTasksModel>> GetProjectsCableLevel(UserModel user, int projectId)
		{
			if(user == null)
				return ResponseModel<List<ProjectCableTasksModel>>.AccessDeniedResponse();
			try
			{
				var entities = Infrastructure.Data.Access.Joins.BSD.ProjectManagmentAcces.GetProjectsCables(projectId);
				if(entities?.Count > 0)
				{
					var fertigungEntities = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetFirstSampleTechnicByArticles(entities.Select(x => x.ArticleId ?? 0));
					var response = new List<ProjectCableTasksModel>();
					foreach(var item in entities)
					{
						var faEntities = fertigungEntities?.Where(x => x.Artikel_Nr == item.ArticleId);
						response.Add(new ProjectCableTasksModel(item, getProductionStatus(faEntities?.Select(x => x.Kennzeichen?.Trim()?.ToLower()))));
					}

					return ResponseModel<List<ProjectCableTasksModel>>.SuccessResponse(response);
				}
				// -
				return ResponseModel<List<ProjectCableTasksModel>>.SuccessResponse(null);

			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}