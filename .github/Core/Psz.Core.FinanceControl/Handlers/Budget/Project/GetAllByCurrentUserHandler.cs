using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Project
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetAllByCurrentUserHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Project.ProjectModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public GetAllByCurrentUserHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
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
				var userEentity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);

				var projectEntities = new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>();
				projectEntities.AddRange(
					(this._user.SuperAdministrator
					? Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get()
					: Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetbyCurrent(this._data))
					?? new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>());

				// - department internal projects
				var deptProjects = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetByDepartmentIds(
					new List<int> { userEentity?.DepartmentId ?? -1 }, (int)Enums.BudgetEnums.ProjectTypes.Internal)
					?.Where(x => x.Id_State != (int)Enums.BudgetEnums.ProjectApprovalStatuses.Reject)
					?.ToList();

				if(!this._user.IsGlobalDirector && !this._user.SuperAdministrator)
				{
					deptProjects = deptProjects
						?.Where(x => x.ProjectStatus == (int)Enums.BudgetEnums.ProjectStatuses.Active)
					?.Where(x => x.Id_State == (int)Enums.BudgetEnums.ProjectApprovalStatuses.Active)
						?.ToList();
				}

				projectEntities.AddRange(deptProjects
					?? new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>());

				// - Company external projects
				var compProjects = new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>();

				if(this._user.IsGlobalDirector || this._user.SuperAdministrator)
				{
					compProjects = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetByType((int)Enums.BudgetEnums.ProjectTypes.External);
				}
				else
				{
					compProjects = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetBySitesIds(
					new List<int> { userEentity?.CompanyId ?? -1 }, (int)Enums.BudgetEnums.ProjectTypes.External)
					?.Where(x => x.Id_State != (int)Enums.BudgetEnums.ProjectApprovalStatuses.Reject)
					?.ToList();
				}

				if(!this._user.IsGlobalDirector && !this._user.SuperAdministrator)
				{
					compProjects = compProjects
						?.Where(x => x.ProjectStatus == (int)Enums.BudgetEnums.ProjectStatuses.Active)
					?.Where(x => x.Id_State == (int)Enums.BudgetEnums.ProjectApprovalStatuses.Active)
						?.ToList();
				}

				projectEntities.AddRange(compProjects
					?? new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>());


				if(projectEntities == null || projectEntities.Count <= 0)
				{
					return ResponseModel<List<Models.Budget.Project.ProjectModel>>.SuccessResponse();
				}

				projectEntities = projectEntities.Distinct().ToList();

				var nrCustomers = projectEntities.Select(x => int.TryParse(x.CustomerId?.ToString(), out var id) ? id : 0)?.ToList();
				nrCustomers = nrCustomers.FindAll(x => x != 0).ToList();
				var cutomers = Infrastructure.Data.Access.Tables.FNC.KundenAccess.Get(nrCustomers);
				var adressenEntities = Infrastructure.Data.Access.Tables.FNC.AdressenAccess.Get(cutomers?.Select(x => x.Nummer.HasValue ? x.Nummer.Value : -1).ToList());

				var validatorEntites = Infrastructure.Data.Access.Tables.FNC.ProjectValidatorsAccess.GetByProjectIds(projectEntities.Select(x => x.Id)?.ToList());
				var fileEntites = Infrastructure.Data.Access.Tables.FNC.ProjectFileAccess.GetByProjectIds(projectEntities.Select(x => x.Id)?.ToList());

				var response = new List<Models.Budget.Project.ProjectModel>();

				projectEntities = projectEntities?.DistinctBy(x => x.Id)?.ToList();
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

					var orderEntity = orderEntities?.FindAll(x => x.ProjectId == projectEntity.Id);
					var articleEntity = orderEntity != null && orderEntity.Count > 0
						? articleEntites?.FindAll(x => orderEntity.Exists(y => y.OrderId == x.OrderId))
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
			if(Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetbyCurrent(this._data) == null)
				return ResponseModel<List<Models.Budget.Project.ProjectModel>>.FailureResponse("Project not found");
			return ResponseModel<List<Models.Budget.Project.ProjectModel>>.SuccessResponse();
		}
	}
}
