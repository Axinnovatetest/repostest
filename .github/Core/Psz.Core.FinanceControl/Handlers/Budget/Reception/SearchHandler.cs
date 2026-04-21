using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Reception
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class SearchHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Budget.Reception.SearchResponseModel<Models.Budget.Reception.GetMinimalModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Budget.Reception.SearchRequestModel _data { get; set; }

		public SearchHandler(Identity.Models.UserModel user, Models.Budget.Reception.SearchRequestModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<Models.Budget.Reception.SearchResponseModel<Models.Budget.Reception.GetMinimalModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				string orderByColumn = null;
				switch(this._data.SortFieldKey)
				{
					case Enums.BudgetEnums.ReceptionSearchFields.Nr:
						orderByColumn = "Nr";
						break;
					case Enums.BudgetEnums.ReceptionSearchFields.OrderNumber:
						orderByColumn = "OrderNumber";
						break;
					case Enums.BudgetEnums.ReceptionSearchFields.Type:
						orderByColumn = "Typ";
						break;
					case Enums.BudgetEnums.ReceptionSearchFields.Employee:
						orderByColumn = "IssuerName";
						break;
					case Enums.BudgetEnums.ReceptionSearchFields.Company:
						orderByColumn = "CompanyName";
						break;
					case Enums.BudgetEnums.ReceptionSearchFields.Department:
						orderByColumn = "DepartmentName";
						break;
					case Enums.BudgetEnums.ReceptionSearchFields.Supplier:
						orderByColumn = "[Vorname/NameFirma]";
						break;
					case Enums.BudgetEnums.ReceptionSearchFields.Date:
						orderByColumn = "Datum";
						break;
					case Enums.BudgetEnums.ReceptionSearchFields.DeliveryDate:
						orderByColumn = "Liefertermin";
						break;
					case Enums.BudgetEnums.ReceptionSearchFields.Done:
						orderByColumn = "erledigt";
						break;
					default:
						orderByColumn = "Nr";
						break;
				}

				/// 
				var bestellungEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.FilterReceptionsAndOrdersByNummer(this._data.Bestellung_Nr,
					this._data.Artikel_Nr,
					this._data.Bezug,
					this._data.Projekt_Nr,
					this._data.Lieferanten_Nr,
					this._data.CompanyId,
					this._data.DepartmentId,
					this._data.IssuerId,
					this._data.InProgressOnly,
					(Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Types)this._data.OrderType,
					this._data.PageNumber, this._data.PageSize, orderByColumn, this._data.SortDesc)
					?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenEntity>();
				var allCount = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.FilterReceptionsAndOrdersByNummer_Count(this._data.Bestellung_Nr,
					this._data.Artikel_Nr,
					this._data.Bezug,
					this._data.Projekt_Nr,
					this._data.Lieferanten_Nr,
					this._data.CompanyId,
					this._data.DepartmentId,
					this._data.IssuerId,
					this._data.InProgressOnly,
					(Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Types)this._data.OrderType,
					this._data.PageNumber, this._data.PageSize, orderByColumn, this._data.SortDesc);
				var bestellungExtEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderIds(bestellungEntities?.Select(x => x.Nr)?.ToList());
				var companyExtensionEntities = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.Get();

				var userFNCProfileIds = (Infrastructure.Data.Access.Tables.FNC.UserAccessProfilesAccess.GetByUserId(new List<int> { this._user.Id })
					?? new List<Infrastructure.Data.Entities.Tables.FNC.UserAccessProfilesEntity>()).Select(x => x.AccessProfileId).ToList();
				var userProfileIds = (Infrastructure.Data.Access.Tables.FNC.AccessProfileAccess.Get(userFNCProfileIds)
					?? new List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity>())?.Select(x => x.MainAccessProfileId).ToList();

				var results = new List<Models.Budget.Reception.GetMinimalModel> { };
				foreach(var bestellungentItem in bestellungEntities)
				{
					var extensionItem = bestellungExtEntities?.Find(x => x.OrderId == bestellungentItem.Nr);
					var companyExtension = companyExtensionEntities.Find(x => x.CompanyId == extensionItem.CompanyId); // PO's company
					results.Add(new Models.Budget.Reception.GetMinimalModel(bestellungentItem, extensionItem,
					   extensionItem.ApprovalTime.HasValue && Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.GetReceptionsByOrderId_Count(extensionItem.OrderId) > 0 &&
					   (_user.IsGlobalDirector || _user.SuperAdministrator || _user.Id == extensionItem.IssuerId || userProfileIds.Exists(x => x == companyExtension?.FinanceProfileId))));
				}

				return ResponseModel<Models.Budget.Reception.SearchResponseModel<Models.Budget.Reception.GetMinimalModel>>.SuccessResponse(
					new Models.Budget.Reception.SearchResponseModel<Models.Budget.Reception.GetMinimalModel>
					{
						PageSize = this._data.PageSize,
						PageNumber = this._data.PageNumber,
						Results = results,
						AllCount = allCount
					});
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Budget.Reception.SearchResponseModel<Models.Budget.Reception.GetMinimalModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<Models.Budget.Reception.SearchResponseModel<Models.Budget.Reception.GetMinimalModel>>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<Models.Budget.Reception.SearchResponseModel<Models.Budget.Reception.GetMinimalModel>>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<Models.Budget.Reception.SearchResponseModel<Models.Budget.Reception.GetMinimalModel>>.SuccessResponse();
		}
	}
}
