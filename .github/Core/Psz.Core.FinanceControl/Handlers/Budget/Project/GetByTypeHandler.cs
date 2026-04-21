using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Project
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetByTypeHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Project.ProjectModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public GetByTypeHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<List<Models.Budget.Project.ProjectModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var projectsEntities = new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>();
				var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);

				if(this._data == (int)Enums.BudgetEnums.ProjectTypes.External)
				{
					//projectsEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetBySitesIds(new List<int> { userEntity.CompanyId ?? -1 }, (int)Enums.BudgetEnums.ProjectTypes.External));
					projectsEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetByType((int)Enums.BudgetEnums.ProjectTypes.External));
				}
				else // Internal projects
				{
					var siteEntities = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetByDirectorId(new List<int> { userEntity.Id });

					// User director of site
					if(siteEntities != null && siteEntities.Count > 0)
					{
						projectsEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetBySitesIds(siteEntities?.Select(x => x.Id)?.ToList(), (int)Enums.BudgetEnums.ProjectTypes.Internal));
					}
					else
					{
						var departmentEntities = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByDirectorId(userEntity.Id);
						// User director of department
						if(departmentEntities != null && departmentEntities.Count > 0)
						{
							projectsEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetByDepartmentIds(departmentEntities?.Select(x => (int)x.Id)?.ToList(), (int)Enums.BudgetEnums.ProjectTypes.Internal));
						}
						else
						{
							projectsEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetBySiteWODepartment(new List<int> { (userEntity?.CompanyId ?? -1) }, (int)Enums.BudgetEnums.ProjectTypes.Internal));
							var userProjectEntities = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetbyCurrent(this._user.Id);
							projectsEntities.AddRange(userProjectEntities);

						}
					}
				}

				// - opt
				if(projectsEntities == null || projectsEntities.Count <= 0)
				{
					return ResponseModel<List<Models.Budget.Project.ProjectModel>>.SuccessResponse();
				}

				projectsEntities = projectsEntities?.DistinctBy(x => x.Id)?.ToList();
				var nrCustomers = projectsEntities.Select(x => int.TryParse(x.CustomerId?.ToString(), out var id) ? id : 0)?.ToList();
				nrCustomers = nrCustomers.FindAll(x => x != 0)?.ToList();
				var cutomers = Infrastructure.Data.Access.Tables.FNC.KundenAccess.Get(nrCustomers);
				var adressenEntities = Infrastructure.Data.Access.Tables.FNC.AdressenAccess.Get(cutomers?.Select(x => x.Nummer.HasValue ? x.Nummer.Value : -1)?.ToList());

				var validatorEntites = Infrastructure.Data.Access.Tables.FNC.ProjectValidatorsAccess.GetByProjectIds(projectsEntities.Select(x => x.Id)?.ToList());
				var fileEntites = Infrastructure.Data.Access.Tables.FNC.ProjectFileAccess.GetByProjectIds(projectsEntities.Select(x => x.Id)?.ToList());

				var response = new List<Models.Budget.Project.ProjectModel>();

				projectsEntities = projectsEntities.Distinct()?.ToList();
				projectsEntities = projectsEntities.OrderByDescending(x => x.Id)?.ToList();
				var orderEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByProjects_MinLevel(projectsEntities.Select(x => new Tuple<int, int>(x.Id, 1))?.ToList());
				var articleEntites = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(orderEntities?.Select(x => x.OrderId)?.ToList());
				foreach(var projectEntity in projectsEntities)
				{
					var nrCustomer = cutomers.Find(x => x.Nr == projectEntity.CustomerId)?.Nummer;
					var adressenEntity = nrCustomer.HasValue
						? adressenEntities?.Find(e => e.Nr == nrCustomer.Value)
						: null;

					var validatorEntity = validatorEntites?.FindAll(x => x.Id_Project == projectEntity.Id)?.ToList();
					var fileEntity = fileEntites?.FindAll(x => x.ProjectId == projectEntity.Id)?.ToList();

					var orderEntity = orderEntities.FindAll(x => x.ProjectId == projectEntity.Id);
					var articleEntity = orderEntity != null && orderEntity.Count > 0
						? articleEntites.FindAll(x => orderEntity.Exists(y => y.OrderId == x.OrderId))
						: new List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity>();
					response.Add(new Models.Budget.Project.ProjectModel(fileEntity, projectEntity, adressenEntity, validatorEntity, orderEntity, articleEntity));
				}

				return ResponseModel<List<Models.Budget.Project.ProjectModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Budget.Project.ProjectModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Budget.Project.ProjectModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Budget.Project.ProjectModel>>.SuccessResponse();
		}
	}
}
