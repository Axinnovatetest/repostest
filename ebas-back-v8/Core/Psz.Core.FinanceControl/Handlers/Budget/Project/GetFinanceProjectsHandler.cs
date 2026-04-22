using Psz.Core.FinanceControl.Models.Budget.Project;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Budget.Project
{
	public class GetFinanceProjectsHandler: IHandle<UserModel, ResponseModel<List<ProjectModel>>>
	{
		private readonly UserModel _user;
		public GetFinanceProjectsHandler(UserModel user)
		{
			_user = user;
		}
		public ResponseModel<List<ProjectModel>> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			try
			{
				if(!_user.Access.Financial.Budget.FinanceProject)
					return ResponseModel<List<Models.Budget.Project.ProjectModel>>.SuccessResponse(null);
				var userVisibiltyEntities = Infrastructure.Data.Access.Tables.FNC.FinanceProjectsVisibiltyUsersAccess.GetByUserId(_user.Id);
				var projectEntities = new List<Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity>();
				projectEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(userVisibiltyEntities?.Select(x => x.ProjectId ?? -1).ToList()));
				projectEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetByType((int)Enums.BudgetEnums.ProjectTypes.Finance)?.Where(x => x.ResponsableId == _user.Id).ToList());
				if(projectEntities == null || projectEntities.Count <= 0)
					return ResponseModel<List<Models.Budget.Project.ProjectModel>>.SuccessResponse(null);

				projectEntities = projectEntities.DistinctBy(x => x.Id).ToList();
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

		public ResponseModel<List<ProjectModel>> Validate()
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