using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Project
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetByDepartmentHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Project.ProjectModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetByDepartmentHandler(Identity.Models.UserModel user)
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

				///  -
				var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
				var departmentIds = new List<long>() { userEntity?.DepartmentId ?? -1 };

				var deptDirectorEntities = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(departmentIds);
				if(deptDirectorEntities == null)
				{
					return ResponseModel<List<Models.Budget.Project.ProjectModel>>.SuccessResponse(null);
				}

				var departmentEntities = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByDirectorId(userEntity?.Id ?? -1);
				if(departmentEntities?.Count > 0)
				{
					departmentIds.AddRange(departmentEntities.Select(x => x.Id));
				}

				var projectEntities =
					Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetByDepartmentIds(departmentIds.Select(x => (int)x).ToList(), null)
					?? new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>();

				if(projectEntities == null || projectEntities.Count <= 0)
					return ResponseModel<List<Models.Budget.Project.ProjectModel>>.SuccessResponse(null);

				var customerEntities = Infrastructure.Data.Access.Tables.FNC.KundenAccess.Get(projectEntities.Select(x => x.CustomerId ?? -1)?.ToList());

				List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity> adressenEntities = null;
				if(customerEntities != null && customerEntities.Count > 0)
				{
					adressenEntities = Infrastructure.Data.Access.Tables.FNC.AdressenAccess.Get(customerEntities.Select(x => x.Nummer ?? -1)?.ToList());
				}

				var fileEntites = Infrastructure.Data.Access.Tables.FNC.ProjectFileAccess.GetByProjectIds(projectEntities.Select(x => x.Id)?.ToList());

				var responseBody = new List<Models.Budget.Project.ProjectModel>();
				projectEntities = projectEntities?.DistinctBy(x => x.Id)?.ToList();
				projectEntities = projectEntities.OrderByDescending(x => x.Id)?.ToList();
				var orderEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByProjects_MinLevel(projectEntities.Select(x => new Tuple<int, int>(x.Id, 1))?.ToList());
				var articleEntites = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(orderEntities?.Select(x => x.OrderId)?.ToList());
				foreach(var item in projectEntities)
				{
					var customerEntity = customerEntities?.Find(x => x.Nr == item.CustomerId);
					var adressenEntity = adressenEntities?.Find(x => x.Nr == customerEntity?.Nummer);

					var validatorEntity = Infrastructure.Data.Access.Tables.FNC.ProjectValidatorsAccess.GetByProjectId(item.Id)?.ToList();
					var userValidatorEntities = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(validatorEntity?.Select(x => x.Id_Validator)?.ToList());
					var fileEntity = fileEntites?.FindAll(x => x.ProjectId == item.Id)?.ToList();

					var orderEntity = orderEntities?.FindAll(x => x.ProjectId == item.Id);
					var articleEntity = orderEntity != null && orderEntity.Count > 0
						? articleEntites?.FindAll(x => orderEntity.Exists(y => y.OrderId == x.OrderId))
						: new List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity>();

					responseBody.Add(new Models.Budget.Project.ProjectModel(fileEntity, item, adressenEntity, validatorEntity, userValidatorEntities, orderEntity, articleEntity));
				}

				return ResponseModel<List<Models.Budget.Project.ProjectModel>>.SuccessResponse(responseBody);
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

			if(Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByDirectorId(this._user.Id) == null)
			{
				return ResponseModel<List<Models.Budget.Project.ProjectModel>>.FailureResponse("User is not a Department Director");
			}

			return ResponseModel<List<Models.Budget.Project.ProjectModel>>.SuccessResponse();
		}
	}
}
