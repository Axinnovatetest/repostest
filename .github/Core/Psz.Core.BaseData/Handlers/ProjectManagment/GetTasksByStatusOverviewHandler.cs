using Psz.Core.BaseData.Models.ProjectManagment;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.ProjectManagment
{
	public partial class ProjectManagmentService
	{
		public ResponseModel<List<TasksOverviewModel>> GetTasksByStatusOverview(UserModel user, string status)
		{
			if(user == null)
				return ResponseModel<List<TasksOverviewModel>>.AccessDeniedResponse();

			try
			{
			var entities = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_CurrentTaskAccess.GetByStatus(status);
			var response = entities?.Select(x => new TasksOverviewModel(x)).ToList();

			return ResponseModel<List<TasksOverviewModel>>.SuccessResponse(response);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}