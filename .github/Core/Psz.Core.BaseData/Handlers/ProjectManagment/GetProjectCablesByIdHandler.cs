using Psz.Core.BaseData.Models.ProjectManagment;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.ProjectManagment
{
	public partial class ProjectManagmentService
	{
		public ResponseModel<List<ProjectCableModel>> GetProjectCablesById(UserModel user, int id)
		{
			if(user == null)
				return ResponseModel<List<ProjectCableModel>>.AccessDeniedResponse();

			try
			{
				var entities = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_CablesAccess.GetByByProjectAndStatus(id, null);
				if(entities?.Count > 0)
				{
					var fertigungEntities = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetFirstSampleTechnicByArticles(entities.Select(x => x.ArticleId ?? 0));
					var response = new List<ProjectCableModel>();
					foreach(var item in entities)
					{
						var faEntities = fertigungEntities?.Where(x => x.Artikel_Nr == item.ArticleId);
						response.Add(new ProjectCableModel(item, getProductionStatus(faEntities?.Select(x => x.Kennzeichen?.Trim()?.ToLower()))));
					}

					return ResponseModel<List<ProjectCableModel>>.SuccessResponse(response);
				}

				// - 
				return ResponseModel<List<ProjectCableModel>>.SuccessResponse(null);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		static int getProductionStatus(IEnumerable<string> faStatuses)
		{
			if(faStatuses == null || faStatuses.Count() <= 0)
			{
				return (int)Enums.ProjectManagmentEnums.ProductionStatuses.Red;
			}

			// - red
			if(faStatuses.All(x => x == "offen"))
			{
				return (int)Enums.ProjectManagmentEnums.ProductionStatuses.Red;
			}

			// - green
			if(faStatuses.All(x => x == "erledigt"))
			{
				return (int)Enums.ProjectManagmentEnums.ProductionStatuses.Green;
			}

			// - Orange
			return (int)Enums.ProjectManagmentEnums.ProductionStatuses.Orange;
		}
	}
}