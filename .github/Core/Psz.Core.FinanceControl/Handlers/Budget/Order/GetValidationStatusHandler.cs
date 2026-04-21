using System;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetValidationStatusHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetValidationStatusHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var validated = 1;
				var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data, isArchived: null);
				if(orderEntity.OrderType?.Trim().ToLower() == Enums.BudgetEnums.ProjectTypes.External.GetDescription().ToLower())
				{
					// search creator user - or validators
					var validatorEntities = Validators.getByOrderId(orderEntity.OrderId, out var errs);
					var validatorEntity = new Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity();
					if(orderEntity.Level == validatorEntities?.Count - 1) // Purchase
					{
						var companyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(orderEntity.CompanyId ?? -1);
						validatorEntity = Helpers.Processings.Budget.HasPurchaseProfile(this._user.Id, companyExtensionEntity)
								? validatorEntities?.Find(x => x.Id_Validator == companyExtensionEntity.PurchaseProfileId)
								: null;
					}
					else
					{
						validatorEntity = validatorEntities?.Find(x => x.Id_Validator == this._user.Id);
					}

					if(orderEntity.Level >= validatorEntities?.Count)
						validated = 0;

					if(validatorEntity != null && validatorEntity.Level.HasValue && validatorEntity.Level.Value >= orderEntity.Level)
						validated = 0;
				}
				else // internal project
				{
					var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
					var orderIssuerEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(orderEntity.IssuerId);
					var orderDepartmentEntity = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(orderIssuerEntity?.DepartmentId ?? -1);
					var orderCompanyEntity = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(orderDepartmentEntity?.CompanyId ?? -1);
					var orderPurchaseEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(orderCompanyEntity?.Id ?? -1);

					// User's budget
					if(orderEntity.Level == (int)Enums.BudgetEnums.ValidationLevels.Draft)
					{
						// do nthing - order not validated
						if(orderIssuerEntity.Id == userEntity.Id)
						{
							validated = 0;
						}
					}
					else if(orderEntity.Level == (int)Enums.BudgetEnums.ValidationLevels.DepartmentDirector) // department
					{
						if(userEntity.Id == orderDepartmentEntity?.HeadUserId
							|| userEntity.Id == orderCompanyEntity?.DirectorId
							|| Helpers.Processings.Budget.HasPurchaseProfile(userEntity.Id, orderPurchaseEntity))
						{
							validated = 0;
						}
					}
					else if(orderEntity.Level == (int)Enums.BudgetEnums.ValidationLevels.SiteDirector) // site
					{
						if(userEntity.Id == orderCompanyEntity.DirectorId
							|| Helpers.Processings.Budget.HasPurchaseProfile(userEntity.Id, orderPurchaseEntity))
						{
							validated = 0;
						}
					}
					else if(orderEntity.Level == (int)Enums.BudgetEnums.ValidationLevels.Purchase) // purchase dept
					{
						if(Helpers.Processings.Budget.HasPurchaseProfile(userEntity.Id, orderPurchaseEntity))
						{
							validated = 0;
						}
					}
					else
					{
						validated = 0;
					}
				}

				return ResponseModel<int>.SuccessResponse(validated);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data, isArchived: null);
			if(orderEntity == null)
				return ResponseModel<int>.FailureResponse("Order not found");

			var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(orderEntity.ProjectId ?? -1);
			if(orderEntity.OrderType?.Trim().ToLower() == Enums.BudgetEnums.ProjectTypes.External.GetDescription().ToLower())
			{
				// search creator user - or validators
				if(orderEntity.IssuerId != this._user.Id || (orderEntity.ProjectId == null || orderEntity.ProjectId < 1))
				{
					var validatorEntities = Validators.getByOrderId(orderEntity.OrderId, out var errs);
					if(validatorEntities == null || validatorEntities.Count <= 0)
						return ResponseModel<int>.FailureResponse(key: "1", value: "Empty validator list");
					if(orderEntity.Level == (int)Enums.BudgetEnums.ValidationLevels.SuperValidator)
					{
						var companyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.Get(orderEntity.CompanyId ?? -1);
						if(companyExtensionEntity.SuperValidatorOneId == _user.Id || companyExtensionEntity.SuperValidatorTowId == _user.Id)
						{
							validatorEntities.Add(new Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity
							{
								Id_Validator = _user.Id,
								Level = (int)Enums.BudgetEnums.ValidationLevels.SuperValidator,
							});
						}
					}
					if(orderEntity.Level > validatorEntities.Count - 1 && orderEntity.Level != (int)Enums.BudgetEnums.ValidationLevels.SuperValidator)
					{
						return ResponseModel<int>.FailureResponse(key: "1", value: "Order already validated");
					}
					else
					{
						if(orderEntity.Level == validatorEntities.Count - 1) // Purchase
						{
							var companyExtension = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(orderEntity.CompanyId ?? -1);
							if(Helpers.Processings.Budget.HasPurchaseProfile(this._user.Id, companyExtension) != true) // profileId
							{
								return ResponseModel<int>.FailureResponse(key: "1", value: "User not found as validator");
							}
						}
						else
						{
							var validatorEntity = validatorEntities.Find(x => x.Id_Validator == this._user.Id);
							if(validatorEntity == null)
								return ResponseModel<int>.FailureResponse(key: "1", value: "User not found as validator");

							if(!validatorEntity.Level.HasValue || validatorEntity.Level.Value != orderEntity.Level)
								return ResponseModel<int>.FailureResponse(key: "1", value: "User not found as validator in current step");
						}
					}
				}
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
