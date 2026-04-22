using Psz.Core.BaseData.Models.ProjectManagment;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.ProjectManagment
{
	public partial class ProjectManagmentService
	{
		public ResponseModel<StatsModel> GetProjectsStats(UserModel user)
		{
			if(user == null)
				return ResponseModel<StatsModel>.AccessDeniedResponse();

			try
			{
			var response = new StatsModel();
			var projectsByStatus = Infrastructure.Data.Access.Joins.BSD.ProjectManagmentAcces.GetProjectByStatus();
			var projectsByTaskStatus = Infrastructure.Data.Access.Joins.BSD.ProjectManagmentAcces.GetProjectByTasksStatus();
			var projectsByTime = Infrastructure.Data.Access.Joins.BSD.ProjectManagmentAcces.GetProjectByTime();

			response.ProjectsByStatus = projectsByStatus?.Select(x => new CountsModel { Count = x.Value, Status = x.Key }).ToList();
			response.ProjectsByTasks = projectsByTaskStatus?.Select(x => new CountsModel { Status = x.Key, Count = x.Value }).ToList();
			response.ProjectsByTime = new List<CountsModel>
			{
				new CountsModel{Status="Closed",Count=projectsByTime.Item3},
				new CountsModel{Status="Late",Count=projectsByTime.Item1},
				new CountsModel{Status="OnTime",Count=projectsByTime.Item2},
			};

			return ResponseModel<StatsModel>.SuccessResponse(response);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}