using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using Infrastructure.Data.Entities.Tables.PRS;
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Budget.Order.OrderModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<Models.Budget.Order.OrderModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var currentUser = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
				var companyExtensionEntities = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.Get();

				var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data, isArchived: null);
				var bestellungEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(orderEntity.OrderId);

				var fileEntity = Infrastructure.Data.Access.Tables.FNC.Budget_JointFile_OrderAccess.GetByIdOrder(orderEntity.OrderId);
				var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(orderEntity.ProjectId ?? -1);

				var articleEntity = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderId(orderEntity.OrderId)?.ToList();
				var bestellteArticleEntities = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.Get(articleEntity?.Select(x => x.BestellteArtikelNr)?.ToList());

				var articlesEntities = Infrastructure.Data.Access.Tables.FNC.Artikel_BudgetAccess.Get(articleEntity?.Select(x => x.ArticleId)?.ToList());

				// Validators
				var validators = Validators.getByOrderId(orderEntity.OrderId, out List<string> errors);
				if(errors != null && errors.Count > 0)
					ResponseModel<List<Models.Budget.Order.OrderModel>>.FailureResponse(string.Join(", ", errors));

				var uValidators = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(validators?.Select(x => x.Id_Validator)?.ToList());
				var orderCompany = companyExtensionEntities.Find(x => x.CompanyId == orderEntity.CompanyId);

				var order = new Models.Budget.Order.OrderModel(orderEntity, bestellungEntity, projectEntity, articleEntity, bestellteArticleEntities, articlesEntities, fileEntity, validators, uValidators,
					this._user,
					Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.GetReceptionsByOrderId_Count(orderEntity.OrderId));
				order.CanValidate = getCanValidate(orderEntity, validators);

				return ResponseModel<Models.Budget.Order.OrderModel>.SuccessResponse(order);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Budget.Order.OrderModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<Models.Budget.Order.OrderModel>.AccessDeniedResponse();
			}

			var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data, isArchived: null);
			if(orderEntity == null)
				return ResponseModel<Models.Budget.Order.OrderModel>.FailureResponse("Order not found");

			if(orderEntity.OrderType == Enums.BudgetEnums.ProjectTypes.Finance.GetDescription())
			{
				if(!_user.Access.Financial.Budget.FinanceOrder)
					return ResponseModel<Models.Budget.Order.OrderModel>.AccessDeniedResponse();
				else
				{
					var visibilityProjectsEntities = Infrastructure.Data.Access.Tables.FNC.FinanceProjectsVisibiltyUsersAccess.GetByUserId(_user.Id);
					var visibilityProjects = visibilityProjectsEntities?.Select(x => x.ProjectId).ToList();
					if(!visibilityProjects.Contains(orderEntity.ProjectId))
						return ResponseModel<Models.Budget.Order.OrderModel>.AccessDeniedResponse();
				}
			}

			return ResponseModel<Models.Budget.Order.OrderModel>.SuccessResponse();
		}
		internal bool getCanValidate(
			Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity extensionEntity,
			List<Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity> validators)
		{
			var companyEntity = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(extensionEntity.CompanyId ?? -1);
			var companyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(extensionEntity.CompanyId ?? -1);

			if(extensionEntity.Level == (int)Enums.BudgetEnums.ValidationLevels.SuperValidator)
			{
				if(companyExtensionEntity.SuperValidatorOneId == _user.Id || companyExtensionEntity.SuperValidatorTowId == _user.Id)
					validators.Add(new Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity
					{
						Id_Validator = _user.Id,
						Level = (int)Enums.BudgetEnums.ValidationLevels.SuperValidator
					});
			}
			if(extensionEntity.OrderType.ToLower() == Enums.BudgetEnums.ProjectTypes.Finance.GetDescription().ToLower())
			{
				// - already fully approved
				if(extensionEntity.ApprovalUserId is not null && extensionEntity.ApprovalUserId > 0)
				{
					return false;
				}

				// - first validation request
				if(extensionEntity.Level == (int)Enums.BudgetEnums.ValidationLevels.Draft)
				{
					return this._user.Id == extensionEntity.IssuerId;
				}

				// - in-between validation request
				return Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.IsUserFinanceValidator(extensionEntity.CompanyId ?? -1, this._user.Id);
			}
			else if(extensionEntity.OrderType.ToLower() == Enums.BudgetEnums.ProjectTypes.External.GetDescription().ToLower())
			{
				if(extensionEntity.Level == validators.Count - 1)
				{
					return Helpers.Processings.Budget.HasPurchaseProfile(this._user.Id, companyExtensionEntity);
				}
				else
				{
					return validators.Exists(x => x.Id_Validator == this._user.Id && x.Level == extensionEntity.Level);
				}
			}
			else
			{
				if(extensionEntity.Level == (int)Enums.BudgetEnums.ValidationLevels.Draft)
				{
					return true;
				}

				if(extensionEntity.Level == (int)Enums.BudgetEnums.ValidationLevels.DepartmentDirector)
				{
					var departmentEntity = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(extensionEntity.DepartmentId ?? -1);
					return departmentEntity?.HeadUserId == this._user.Id || companyEntity?.DirectorId == this._user.Id;
				}

				if(extensionEntity.Level == (int)Enums.BudgetEnums.ValidationLevels.SiteDirector)
				{
					return companyEntity?.DirectorId == this._user.Id;
				}

				if(extensionEntity.Level == (int)Enums.BudgetEnums.ValidationLevels.SuperValidator)
				{
					var companyExtEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(extensionEntity.CompanyId ?? -1);
					return companyExtEntity.SuperValidatorOneId == this._user.Id || companyExtEntity.SuperValidatorTowId == this._user.Id;
				}

				if(extensionEntity.Level == (int)Enums.BudgetEnums.ValidationLevels.Purchase)
				{
					var companyExtEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(extensionEntity.CompanyId ?? -1);
					return Helpers.Processings.Budget.HasPurchaseProfile(this._user.Id, companyExtEntity);
				}
			}

			return false;
		}
	}
}