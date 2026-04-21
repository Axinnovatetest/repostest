using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Reception.Article
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetByReceptionHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Reception.Article.GetModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetByReceptionHandler(Identity.Models.UserModel user, int model)
		{
			this._user = user;
			this._data = model;
		}

		public ResponseModel<List<Models.Budget.Reception.Article.GetModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var articleEntities = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.GetByOrderId(this._data) ?? new List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity>();
				var bestellungEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(this._data);
				var artikelEntities = Infrastructure.Data.Access.Tables.FNC.Artikel_BudgetAccess.Get(articleEntities.Select(x => x.Artikel_Nr ?? -1)?.ToList());
				var responseBody = new List<Models.Budget.Reception.Article.GetModel>();
				foreach(var item in articleEntities)
				{
					var artikelItem = artikelEntities?.Find(x => x.Artikel_Nr == item.Artikel_Nr);
					responseBody.Add(new Models.Budget.Reception.Article.GetModel(item, artikelItem, bestellungEntity));
				}
				return ResponseModel<List<Models.Budget.Reception.Article.GetModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Budget.Reception.Article.GetModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Budget.Reception.Article.GetModel>>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<List<Models.Budget.Reception.Article.GetModel>>.FailureResponse(key: "1", value: "User not found");

			if(Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.GetOpenReception(this._data) == null)
				return ResponseModel<List<Models.Budget.Reception.Article.GetModel>>.FailureResponse(key: "1", value: "Order not found");

			return ResponseModel<List<Models.Budget.Reception.Article.GetModel>>.SuccessResponse();
		}
	}
}
