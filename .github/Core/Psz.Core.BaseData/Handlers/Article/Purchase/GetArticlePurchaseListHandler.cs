using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.Purchase
{
	public class GetArticlePurchaseListHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Purchase.GetMinimalModel>>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public GetArticlePurchaseListHandler(UserModel user, int articleId)
		{
			this._user = user;
			this._data = articleId;
		}
		public ResponseModel<List<Models.Article.Purchase.GetMinimalModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var response = new List<Models.Article.Purchase.GetMinimalModel>();
				var purchaseEntities = Infrastructure.Data.Access.Tables.BSD.BestellnummernAccess.GetByArticle(this._data) ?? new List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity>();
				var wahrungenEntity = Infrastructure.Data.Access.Tables.BSD.WahrungenAccess.Get();
				var adressenEntities = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(purchaseEntities?.Select(x => x.Lieferanten_Nr ?? -1)?.ToList()) ?? new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
				var lieferantensEntities = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.GetByNummers(adressenEntities?.Select(x => x.Nr)?.ToList()) ?? new List<Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity>();
				var customPrices = Infrastructure.Data.Access.Tables.BSD.Bestellnummern_StaffelpreiseAccess.GetByOrderNumber(purchaseEntities?.Select(x => x.Nr)?.ToList());

				var joint = (from purchase in purchaseEntities
							 join lieferanten in lieferantensEntities on purchase.Lieferanten_Nr equals lieferanten.Nummer
							 select purchase).Distinct();

				if(joint != null && joint.Count() > 0)
				{
					foreach(var j in joint)
					{
						response.Add(new Models.Article.Purchase.GetMinimalModel(j));
					}
				}
				if(response.Count > 0)
				{
					foreach(var rs in response)
					{
						foreach(var adr in adressenEntities)
						{
							if(rs.Lieferanten_Nr == adr.Nr)
							{
								var wah = lieferantensEntities.Find(x => x.Nummer == adr.Nr)?.Wahrung;
								if(wah != null)
								{
									rs.Symbol = wahrungenEntity.Find(x => x.Nr == wah)?.Symbol;
								}
								rs.Lieferantennummer = adr.Lieferantennummer; //  adressenEntities.Where(x => x.Nr == rs.Lieferanten_Nr).ToList()[0].Lieferantennummer;
								rs.Lieferantenname = adr.Name1; //  adressenEntities.Where(x => x.Nr == rs.Lieferanten_Nr).ToList()[0].Name1;
							}
						}
						// - 
						rs.CustomPricesCount = customPrices?.Where(x => x.nummer == rs.Nr)?.Count() ?? 0;
					}
					// - 2023-11-06
					response.ForEach(x =>
					{
						var a = lieferantensEntities?.FirstOrDefault(b => b.Nummer == x.Lieferanten_Nr);
						if(a is not null)
						{
							x.SupplierId = a.Nr;
						}
					});
				}

				var OfferPerArticle = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.OfferToArticleEKAccess.GetByArtikelNr(_data);
				var Offers = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsAccess.Get(OfferPerArticle.Select(x => x.OfferId ?? -1).ToList());

				if(OfferPerArticle is not null && OfferPerArticle.Count > 0)
				{
					for(int i = 0; i < response.Count; i++)
					{
						var offer = OfferPerArticle?.Where(x => x.SupplierId == response[i].Lieferanten_Nr && x.ArtikelNr == response[i].ArtikelNr).FirstOrDefault();
						response[i].OfferId = (offer == null ? -1 : offer.OfferId) ?? -1;

						if(response[i].OfferId > 0)
						{
							response[i].FileId = Offers?.Where(x => x.Id == response[i].OfferId).FirstOrDefault().FileId;
						}
					}
				}


				return ResponseModel<List<Models.Article.Purchase.GetMinimalModel>>.SuccessResponse(response);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}
		public ResponseModel<List<Models.Article.Purchase.GetMinimalModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Purchase.GetMinimalModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data) == null)
				return ResponseModel<List<Models.Article.Purchase.GetMinimalModel>>.FailureResponse("Article not found");

			return ResponseModel<List<Models.Article.Purchase.GetMinimalModel>>.SuccessResponse();
		}
	}
}
