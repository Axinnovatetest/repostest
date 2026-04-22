using System;

namespace Psz.Core.FinanceControl.Handlers.Budget.Reception
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Budget.Reception.UpdateModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetHandler(Identity.Models.UserModel user, int model)
		{
			this._user = user;
			this._data = model;
		}

		public ResponseModel<Models.Budget.Reception.UpdateModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var bestellungEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.GetOpenReception(this._data) ?? new Infrastructure.Data.Entities.Tables.FNC.BestellungenEntity();
				var bestellungExtensionEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(bestellungEntities?.Nr ?? -1);
				var companyExtension = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(bestellungExtensionEntity.CompanyId ?? -1); // PO's company

				return ResponseModel<Models.Budget.Reception.UpdateModel>.SuccessResponse(new Models.Budget.Reception.UpdateModel(bestellungEntities, bestellungExtensionEntity,
						bestellungExtensionEntity.ApprovalTime.HasValue && Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetBooked_Count() > 0 &&
						(_user.IsGlobalDirector || _user.SuperAdministrator || _user.Id == bestellungExtensionEntity.IssuerId ||
						(((_user.Access?.Financial.ModuleActivated ?? false)
						&& (
							(bestellungExtensionEntity.OrderType.ToLower() == Enums.BudgetEnums.ProjectTypes.External.GetDescription().ToLower() && ((_user?.Access?.Financial.Budget.CommandeExternalViewInvoice ?? false) || (_user?.Access?.Financial.Budget.CommandeExternalViewInvoiceAllGroup ?? false)))
							|| (bestellungExtensionEntity.OrderType.ToLower() == Enums.BudgetEnums.ProjectTypes.Internal.GetDescription().ToLower() && ((_user?.Access?.Financial.Budget.CommandeInternalViewInvoice ?? false) || (_user?.Access?.Financial.Budget.CommandeInternalViewInvoiceAllGroup ?? false)))
					)))
					)));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Budget.Reception.UpdateModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<Models.Budget.Reception.UpdateModel>.AccessDeniedResponse();
			}

			// - 
			if(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id) == null)
				return ResponseModel<Models.Budget.Reception.UpdateModel>.FailureResponse(key: "1", value: "User not found");

			if(Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(this._data) == null)
				return ResponseModel<Models.Budget.Reception.UpdateModel>.FailureResponse(key: "1", value: "Order not found");

			return ResponseModel<Models.Budget.Reception.UpdateModel>.SuccessResponse();
		}
	}
}
