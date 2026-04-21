using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Reception
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetAllHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Reception.UpdateModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetAllHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Budget.Reception.UpdateModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var bestellungEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.GetOpenReceptions() ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenEntity>();
				var bestellungExtEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderIds(bestellungEntities?.Select(x => x.Nr)?.ToList());
				var companyExtensionEntities = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.Get();
				var ReceptionCounts = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.GetReceptionsByOrderIds_Count(bestellungEntities?.Select(x => x.Nr)?.ToList());

				var results = new List<Models.Budget.Reception.UpdateModel> { };
				foreach(var bestellungentItem in bestellungEntities)
				{
					var extensionItem = bestellungExtEntities.Find(x => x.OrderId == bestellungentItem.Nr);
					var companyExtension = companyExtensionEntities.Find(x => x.CompanyId == extensionItem.CompanyId); // PO's company
					var orderBookingCounts = ReceptionCounts?.Find(x => x.Item1 == bestellungentItem.Nr);
					results.Add(new Models.Budget.Reception.UpdateModel(bestellungentItem, extensionItem,
						extensionItem.ApprovalTime.HasValue && orderBookingCounts.Item2 > 0 &&
						(_user.IsGlobalDirector || _user.SuperAdministrator || _user.Id == extensionItem.IssuerId ||
						(((_user.Access?.Financial.ModuleActivated ?? false)
						&& (
							(extensionItem.OrderType.ToLower() == Enums.BudgetEnums.ProjectTypes.External.GetDescription().ToLower() && ((_user?.Access?.Financial.Budget.CommandeExternalViewInvoice ?? false) || (_user?.Access?.Financial.Budget.CommandeExternalViewInvoiceAllGroup ?? false)))
							|| (extensionItem.OrderType.ToLower() == Enums.BudgetEnums.ProjectTypes.Internal.GetDescription().ToLower() && ((_user?.Access?.Financial.Budget.CommandeInternalViewInvoice ?? false) || (_user?.Access?.Financial.Budget.CommandeInternalViewInvoiceAllGroup ?? false)))
					)))
						)));
				}

				return ResponseModel<List<Models.Budget.Reception.UpdateModel>>.SuccessResponse(results);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Budget.Reception.UpdateModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Budget.Reception.UpdateModel>>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<List<Models.Budget.Reception.UpdateModel>>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<List<Models.Budget.Reception.UpdateModel>>.SuccessResponse();
		}
	}
}
