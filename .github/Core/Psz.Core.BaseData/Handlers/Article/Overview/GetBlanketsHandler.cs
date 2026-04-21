using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.Overview
{
	public class GetBlanketsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Article.ArticleOverviewModel.Blanket>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public GetBlanketsHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<List<Models.Article.ArticleOverviewModel.Blanket>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
				var responseBody = new List<Models.Article.ArticleOverviewModel.Blanket>();
				// -
				responseBody.Add(new Models.Article.ArticleOverviewModel.Blanket
				{
					OrderId = 1,
					Rahmen = articleEntity.Rahmen ?? false,
					Rahmenauslauf = articleEntity.Rahmenauslauf,
					Rahmenmenge = articleEntity.Rahmenmenge,
					Rahmen_Nr = articleEntity.RahmenNr,
					Restmenge = (articleEntity.Rahmenmenge ?? 0) - getBlanketReminderQty(articleEntity.RahmenNr),
					// -
					ArticleId = this._data
				});

				// -
				responseBody.Add(new Models.Article.ArticleOverviewModel.Blanket
				{
					OrderId = 2,
					Rahmen = articleEntity.Rahmen2 ?? false,
					Rahmenauslauf = articleEntity.Rahmenauslauf2,
					Rahmenmenge = articleEntity.Rahmenmenge2,
					Rahmen_Nr = articleEntity.RahmenNr2,
					Restmenge = (articleEntity.Rahmenmenge2 ?? 0) - getBlanketReminderQty(articleEntity.RahmenNr2),
					// -
					ArticleId = this._data
				});

				// -
				return ResponseModel<List<Models.Article.ArticleOverviewModel.Blanket>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.ArticleOverviewModel.Blanket>> Validate()
		{
			if(this._user == null/* this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.ArticleOverviewModel.Blanket>>.AccessDeniedResponse();
			}

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
			if(articleEntity == null)
			{
				return ResponseModel<List<Models.Article.ArticleOverviewModel.Blanket>>.FailureResponse("Article not found");
			}

			// -
			return ResponseModel<List<Models.Article.ArticleOverviewModel.Blanket>>.SuccessResponse();
		}
		public static decimal getBlanketReminderQty(string blanketNr)
		{
			var qty = 0m;

			var articleEntitites = Infrastructure.Data.Access.Tables.BSD.Bestellte_ArtikelAccess.GetByBlanket(blanketNr);
			// -
			if(articleEntitites != null && articleEntitites.Count > 0)
			{
				var orderEntities = Infrastructure.Data.Access.Tables.BSD.BestellungenAccess.GetByBlanket(articleEntitites.Select(x => x.Bestellung_Nr ?? -1)?.ToList());

				foreach(var articleItem in articleEntitites)
				{
					var orderItem = orderEntities?.Find(x => x.Nr == articleItem.Bestellung_Nr);
					if(orderItem != null)
					{
						qty += (articleItem.Start_Anzahl ?? 0);
					}
				}
			}

			// -
			return qty;
		}
	}
}
