using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Project
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetInernalForOrdersHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Project.ProjectModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetInernalForOrdersHandler(Identity.Models.UserModel user)
		{
			this._user = user;
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
				var projectEntities = new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>();
				var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
				var siteEntities = this._user.IsGlobalDirector
					? Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get()
					: Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetByDirectorId(new List<int> { userEntity.Id });

				// User director of site
				if(siteEntities != null && siteEntities.Count > 0)
				{
					projectEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetBySitesIds(siteEntities?.Select(x => x.Id)?.ToList(), (int)Enums.BudgetEnums.ProjectTypes.Internal));
				}

				// User director of department
				var departmentEntities = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByDirectorId(userEntity.Id)
				?? new List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity>();

				// User' s department
				if(userEntity.DepartmentId.HasValue && userEntity.DepartmentId > 0)
				{
					var d = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(userEntity.DepartmentId.Value);
					if(d is not null)
					{
						departmentEntities.Add(d);
					}
				}

				if(departmentEntities != null && departmentEntities.Count > 0)
				{
					projectEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetByDepartmentIds(departmentEntities?.Select(x => ((int?)x?.Id)??0)?.ToList(), (int)Enums.BudgetEnums.ProjectTypes.Internal));
				}

				// User just employee
				{
					projectEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetBySiteWODepartment(new List<int> { (userEntity?.CompanyId ?? -1) }, (int)Enums.BudgetEnums.ProjectTypes.Internal)); // - should be empty
					projectEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetbyCurrent(this._user.Id)
						?.Where(x => x.Id_Type == (int)Enums.BudgetEnums.ProjectTypes.Internal));
				}

				// - 2024-02-17 - Site-level projects
				var departmentIds = new List<long>() { userEntity.DepartmentId ?? -1 };
				departmentIds.AddRange(Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByDirectorId(userEntity?.Id ?? -1)
					?.Select(x => x.Id)
					?? new List<long>());

				var companyEntities = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(
					   Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(departmentIds)
					   ?.Select(x => x.CompanyId)?.ToList());
				projectEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetSiteLevel(companyEntities?.Select(x => x.Id)?.ToList())
					?? new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>());

				// - opt
				if(projectEntities == null || projectEntities.Count <= 0)
				{
					return ResponseModel<List<Models.Budget.Project.ProjectModel>>.SuccessResponse();
				}
				projectEntities = projectEntities?.DistinctBy(x => x.Id)
					?.Where(x => (!x.Archived.HasValue || x.Archived.Value == false)
						&& (!x.Deleted.HasValue || x.Deleted.Value == false)
						//&& x.ProjectStatus.HasValue && x.ProjectStatus == (int)Enums.BudgetEnums.ProjectStatuses.Active
						//&& x.Id_State == (int)Enums.BudgetEnums.ProjectApprovalStatuses.Active
						)?.ToList();

				var nrCustomers = projectEntities.Select(x => int.TryParse(x.CustomerId?.ToString(), out var id) ? id : 0)?.ToList();
				nrCustomers = nrCustomers.FindAll(x => x != 0)?.ToList();
				var cutomers = Infrastructure.Data.Access.Tables.FNC.KundenAccess.Get(nrCustomers);
				var adressenEntities = Infrastructure.Data.Access.Tables.FNC.AdressenAccess.Get(cutomers?.Select(x => x.Nummer.HasValue ? x.Nummer.Value : -1)?.ToList());

				var validatorEntites = Infrastructure.Data.Access.Tables.FNC.ProjectValidatorsAccess.GetByProjectIds(projectEntities.Select(x => x.Id)?.ToList());
				var fileEntites = Infrastructure.Data.Access.Tables.FNC.ProjectFileAccess.GetByProjectIds(projectEntities.Select(x => x.Id)?.ToList());

				var response = new List<Models.Budget.Project.ProjectModel>();

				projectEntities = projectEntities.Distinct()?.ToList();
				projectEntities = projectEntities.OrderByDescending(x => x.Id)?.ToList();
				var orderEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByProjects_MinLevel(projectEntities.Select(x => new Tuple<int, int>(x.Id, 1))?.ToList());
				var articleEntites = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(orderEntities?.Select(x => x.OrderId)?.ToList());
				foreach(var projectEntity in projectEntities)
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
