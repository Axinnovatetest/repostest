using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Project
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;


	public class GetHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Budget.Project.ProjectModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<Models.Budget.Project.ProjectModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(this._data);

				var customer = projectEntity.CustomerId.HasValue
					? Infrastructure.Data.Access.Tables.FNC.KundenAccess.Get(projectEntity.CustomerId.Value)
					: null;

				Infrastructure.Data.Entities.Tables.FNC.AdressenEntity adressenEntity = null;
				if(customer != null)
				{
					adressenEntity = customer.Nummer.HasValue
					? Infrastructure.Data.Access.Tables.FNC.AdressenAccess.Get(customer.Nummer.Value)
					: null;
				}

				var validatorEntity = Infrastructure.Data.Access.Tables.FNC.ProjectValidatorsAccess.GetByProjectId(projectEntity.Id)?.ToList();
				var userValidatorEntities = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(validatorEntity?.Select(x => x.Id_Validator)?.ToList());
				var fileEntites = Infrastructure.Data.Access.Tables.FNC.ProjectFileAccess.GetByProjectIds(new List<int> { projectEntity.Id });
				var orderEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByProjects_MinLevel(new List<Tuple<int, int>> { new Tuple<int, int>(projectEntity.Id, 1) });
				var articleEntites = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(orderEntities?.Select(x => x.OrderId)?.ToList());

				var response = new Models.Budget.Project.ProjectModel();
				response = new Models.Budget.Project.ProjectModel(fileEntites, projectEntity, adressenEntity, validatorEntity, userValidatorEntities, orderEntities, articleEntites /*, DiverseCustomerEntity*/);
				if(projectEntity.Id_Type == (int)Enums.BudgetEnums.ProjectTypes.Finance)
				{
					var visibilityEntities = Infrastructure.Data.Access.Tables.FNC.FinanceProjectsVisibiltyUsersAccess.GetByProjectId(_data);
					var visibilityUsers = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(visibilityEntities?.Select(x => x.UserId ?? -1)?.ToList());
					response.Validators = visibilityEntities?.OrderBy(x=> x.Id)?.Select(U => new Models.Budget.Validator.ValidatorModel
					{
						email = visibilityUsers?.FirstOrDefault(x => x.Id == U.UserId)?.Email,
						Id_Validator = U.UserId ?? -1,
						Validator_Name = visibilityUsers?.FirstOrDefault(x => x.Id == U.UserId)?.Name,
						Id_Project = projectEntity.Id,
						Id_User = _user.Id,
					}).ToList();
				}
				return ResponseModel<Models.Budget.Project.ProjectModel>.SuccessResponse(response);


				return ResponseModel<Models.Budget.Project.ProjectModel>.SuccessResponse(
					new Models.Budget.Project.ProjectModel(fileEntites, projectEntity, adressenEntity, validatorEntity, userValidatorEntities, orderEntities, articleEntites /*, DiverseCustomerEntity*/));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Budget.Project.ProjectModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Budget.Project.ProjectModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(this._data) == null)
			{
				return ResponseModel<Models.Budget.Project.ProjectModel>.FailureResponse("Project not found");
			}

			return ResponseModel<Models.Budget.Project.ProjectModel>.SuccessResponse();
		}
	}
}
