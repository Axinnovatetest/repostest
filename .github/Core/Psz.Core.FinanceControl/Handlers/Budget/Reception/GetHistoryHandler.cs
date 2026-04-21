using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Reception
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetHistoryHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Reception.HistoryModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetHistoryHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<List<Models.Budget.Reception.HistoryModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// - 
				var historyEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.GetHistory(this._data) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenEntity>();
				var historyArticles = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.GetByOrderIds(historyEntities?.Select(x => x.Nr)?.ToList());
				var artikelEntities = Infrastructure.Data.Access.Tables.FNC.Artikel_BudgetAccess.Get(historyArticles?.Select(x => x.Artikel_Nr ?? -1)?.ToList());
				var responseBody = new List<Models.Budget.Reception.HistoryModel>();

				foreach(var item in historyEntities)
				{
					responseBody.Add(new Models.Budget.Reception.HistoryModel(item, artikelEntities, historyArticles.FindAll(x => x.Bestellung_Nr == item.Nr)?.ToList()));
				}

				return ResponseModel<List<Models.Budget.Reception.HistoryModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Budget.Reception.HistoryModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Budget.Reception.HistoryModel>>.AccessDeniedResponse();
			}

			// - 
			if(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id) == null)
				return ResponseModel<List<Models.Budget.Reception.HistoryModel>>.FailureResponse(key: "1", value: "User not found");

			if(Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(this._data) == null)
				return ResponseModel<List<Models.Budget.Reception.HistoryModel>>.FailureResponse(key: "1", value: "Order not found");

			return ResponseModel<List<Models.Budget.Reception.HistoryModel>>.SuccessResponse();
		}
	}
}
