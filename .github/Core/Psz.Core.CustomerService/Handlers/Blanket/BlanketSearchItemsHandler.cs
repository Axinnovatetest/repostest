using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Blanket;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class BlanketSearchItemsHandler: IHandle<Identity.Models.UserModel, ResponseModel<BlanketSearchItemsModel>>
	{
		private BlanketSearchItemRequestModel _data;
		private Identity.Models.UserModel _user { get; set; }
		public BlanketSearchItemsHandler(Identity.Models.UserModel user, BlanketSearchItemRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<BlanketSearchItemsModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				BlanketSearchItemsModel response = new BlanketSearchItemsModel();

				var BlanketArtikelEntity = _data.ArticleId>0
					? Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(_data.ArticleId)
					: Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByArtikelNummer(_data.ArticleNumber);

				if(BlanketArtikelEntity != null)
				{
					var bestellnummernEntity = Infrastructure.Data.Access.Tables.MTM.BestellnummernAccess.GetBySupplierIdArticleId(_data.SupplierAddressId, _data.ArticleId);
					response.Bezeichnung1 = BlanketArtikelEntity.Bezeichnung1;
					response.Bezeichnung2 = BlanketArtikelEntity.Bezeichnung2;
					response.Einheit = BlanketArtikelEntity.Einheit;
					response.StandardPrice = bestellnummernEntity?.Einkaufspreis ?? 0;
				}

				return ResponseModel<BlanketSearchItemsModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<BlanketSearchItemsModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<BlanketSearchItemsModel>.AccessDeniedResponse();
			}
			//var BlanketArtikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(ArtikelNummer);
			//if(BlanketArtikelEntity?.ArtikelNummer == null)
			//	return ResponseModel<BlanketSearchItemsModel>.FailureResponse("Artikel not found");

			// -
			return ResponseModel<BlanketSearchItemsModel>.SuccessResponse();
		}
	}
}
