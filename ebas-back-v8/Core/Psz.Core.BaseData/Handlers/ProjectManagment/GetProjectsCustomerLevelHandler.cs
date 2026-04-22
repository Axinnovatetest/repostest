using Psz.Core.BaseData.Models.ProjectManagment;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.ProjectManagment
{
	public partial class ProjectManagmentService
	{
		public ResponseModel<List<ProjectsMinimalModel>> GetProjectsCustomerLevel(UserModel user)
		{
			if(user == null)
				return ResponseModel<List<ProjectsMinimalModel>>.AccessDeniedResponse();
			try
			{
				var entities = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_ProjectsAccess.Get();
				var projects = entities?.Select(x => new ProjectsMinimalModel(x)).ToList();
				var response = new List<ProjectsMinimalModel>();
				if(projects != null && projects.Count > 0)
				{
					var kunden = projects.Select(x => x.CustomerNumber ?? -1).Distinct().ToList();
					foreach(var k in kunden)
					{
						var _projects = projects.Where(x => x.CustomerNumber == k).OrderBy(y => y.DateCreation).ToList();
						var status = "";
						if(_projects.All(x => x.Status == Enums.ProjectManagmentEnums.ProjectStatuses.Serie.GetDescription()))
							status = Enums.ProjectManagmentEnums.ProjectStatuses.Serie.GetDescription();
						else if(_projects.All(x => x.Status == Enums.ProjectManagmentEnums.ProjectStatuses.Sampling.GetDescription()))
							status = Enums.ProjectManagmentEnums.ProjectStatuses.Sampling.GetDescription();
						else if(_projects.All(x => x.Status == Enums.ProjectManagmentEnums.ProjectStatuses.Offer.GetDescription()))
							status = Enums.ProjectManagmentEnums.ProjectStatuses.Offer.GetDescription();
						else if(_projects.Any(x => x.Status == Enums.ProjectManagmentEnums.ProjectStatuses.Offer.GetDescription()))
							status = Enums.ProjectManagmentEnums.ProjectStatuses.Offer.GetDescription();
						else if(_projects.Any(x => x.Status == Enums.ProjectManagmentEnums.ProjectStatuses.Sampling.GetDescription()))
							status = Enums.ProjectManagmentEnums.ProjectStatuses.Sampling.GetDescription();
						else
							status = Enums.ProjectManagmentEnums.ProjectStatuses.Serie.GetDescription();

						response.Add(new ProjectsMinimalModel
						{
							CustomerNumber = k,
							Customer = _projects[0].Customer,
							DateCreation = _projects[0].DateCreation,
							Completed = _projects.Where(x => x.Status == Enums.ProjectManagmentEnums.ProjectStatuses.Serie.GetDescription()).Count(),
							Open = _projects.Where(x => x.Status == Enums.ProjectManagmentEnums.ProjectStatuses.Offer.GetDescription()).Count(),
							Status = status,
						});
					}
				}

				return ResponseModel<List<ProjectsMinimalModel>>.SuccessResponse(response);

			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}