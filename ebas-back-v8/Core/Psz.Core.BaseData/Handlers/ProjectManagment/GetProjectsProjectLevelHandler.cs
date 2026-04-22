using Psz.Core.BaseData.Interfaces;
using Psz.Core.BaseData.Models.ProjectManagment;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.ProjectManagment
{
	public partial class ProjectManagmentService: IProjectManagmentService
	{
		public ResponseModel<List<ProjectModel>> GetProjectsProjectLevel(UserModel user, int customerNumber)
		{
			if(user == null)
				return ResponseModel<List<ProjectModel>>.AccessDeniedResponse();
			try
			{
				var entities = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_ProjectsAccess.GetByCustomer(customerNumber);
				var response = entities?.Select(x => new ProjectModel(x)).ToList();

				return ResponseModel<List<ProjectModel>>.SuccessResponse(response);

			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}