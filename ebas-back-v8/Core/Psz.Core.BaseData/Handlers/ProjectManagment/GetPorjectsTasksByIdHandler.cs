using Psz.Core.BaseData.Models.ProjectManagment;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.ProjectManagment
{
	public partial class ProjectManagmentService
	{
		public ResponseModel<List<ProjectCableTasksMinimalModel>> GetPorjectTasksById(UserModel user, int id)
		{
			if(user == null)
				return ResponseModel<List<ProjectCableTasksMinimalModel>>.AccessDeniedResponse();

			try
			{
				var entities=Infrastructure.Data.Access.Tables.BSD.__bsd_pm_CurrentTaskAccess.GetByProject(id);
				var response=entities?.Select(x=>new ProjectCableTasksMinimalModel(x)).ToList();

				return ResponseModel<List<ProjectCableTasksMinimalModel>>.SuccessResponse(response);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<ProjectCableTasksMinimalModel>> GetPorjectTasksByCableId(UserModel user, int id)
		{
			if(user == null)
				return ResponseModel<List<ProjectCableTasksMinimalModel>>.AccessDeniedResponse();

			try
			{
				var entities = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_CurrentTaskAccess.GetByCable(id);
				var response = entities?.Select(x => new ProjectCableTasksMinimalModel(x)).ToList();

                return ResponseModel<List<ProjectCableTasksMinimalModel>>.SuccessResponse(response);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}