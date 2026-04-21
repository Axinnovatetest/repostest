using Psz.Core.BaseData.Models.ProjectManagment;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.ProjectManagment
{
	public partial class ProjectManagmentService
	{
		public ResponseModel<List<ProjectGANTTModel>> GetProjectsForGantt(UserModel user)
		{
			if(user == null)
				return ResponseModel<List<ProjectGANTTModel>>.AccessDeniedResponse();

			var response = new List<ProjectGANTTModel>();
			var mileStones = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_MileStonesAccess.Get();
			var projects = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_ProjectsAccess.Get();
			var tasks = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_CurrentTaskAccess.Get();
			var cables = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_CablesAccess.Get();
			foreach(var project in projects)
			{
				var task = tasks?.Where(x => x.ProjectId == project.Id);
				var startDate = task.Min(x => x.StartDate);
				var endDate = task.Max(x => x.Deadline);
				response.Add(new ProjectGANTTModel
				{
					Header = new ProjectHeader(project),
					MileStoneId = 1,
					MileStoneName = "",
					MileStoneStartDate = startDate,
					MileStoneEndDate = endDate,
					Cables = cables.Where(x => x.ProjectId == project.Id)
					.Select(x => new ProjectCableModel(x, true)).ToList(),
					Tasks = task?.Select(x => new ProjectCableTasksMinimalModel(x)).ToList()
				});
			}

			//if(response != null && response.Count > 0)
			//{
			//	foreach(var item in response)
			//	{
			//		foreach(var cable in item.Cables)
			//		{
			//			var tasks = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_CurrentTaskAccess.GetByCable(cable.Id);
			//			cable.Tasks = tasks?.Select(x => new ProjectCableTasksMinimalModel(x)).ToList();
			//		}
			//	}
			//}
			return ResponseModel<List<ProjectGANTTModel>>.SuccessResponse(response);
		}
	}
}