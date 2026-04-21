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
			if(mileStones != null && mileStones.Count > 0)
			{
				foreach(var milestone in mileStones)
				{
					var milestoneTasks = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_TasksMileStonesAccess.GetByMilestone(milestone.Id);
					if(milestoneTasks != null && milestoneTasks.Count > 0)
					{
						var tasks = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_CurrentTaskAccess.Get(milestoneTasks?.Select(x => x.IdTask ?? -1).ToList());
						var cables = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_CablesAccess.Get(tasks.DistinctBy(x => x.ProjectId).Select(x => x.ProjectId ?? -1).ToList());
						var projects = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_ProjectsAccess.Get(cables.Select(x => x.ProjectId ?? -1).ToList());
						foreach(var project in projects)
						{
							response.Add(new ProjectGANTTModel
							{
								Header = new ProjectHeader(project),
								MileStoneId = milestone.Id,
								MileStoneName = milestone.Comment,
								MileStoneStartDate = milestone.StartDate,
								MileStoneEndDate = milestone.EndDate,
								Cables = cables.Where(x => x.ProjectId == project.Id)
								.Select(x => new ProjectCableModel(x)).ToList(),
							});
						}
					}
				}
			}
			if(response != null && response.Count > 0)
			{
				foreach(var item in response)
				{
					foreach(var cable in item.Cables)
					{
						var tasks = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_CurrentTaskAccess.GetByCable(cable.Id);
						cable.Tasks = tasks?.Select(x => new ProjectCableTasksMinimalModel(x)).ToList();
					}
				}
			}
			return ResponseModel<List<ProjectGANTTModel>>.SuccessResponse(response);
		}
	}
}