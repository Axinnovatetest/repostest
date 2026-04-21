using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.Overview
{
	public class GetBlanketDetailHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Article.ArticleOverviewModel.BlanketDetail>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private string _data { get; set; }


		public GetBlanketDetailHandler(Identity.Models.UserModel user, string id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<List<Models.Article.ArticleOverviewModel.BlanketDetail>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var articleEntitites = Infrastructure.Data.Access.Tables.BSD.Bestellte_ArtikelAccess.GetByBlanket(this._data);
				var responseBody = new List<Models.Article.ArticleOverviewModel.BlanketDetail>();
				// -
				if(articleEntitites != null && articleEntitites.Count > 0)
				{
					var orderEntities = Infrastructure.Data.Access.Tables.BSD.BestellungenAccess.GetByBlanket(articleEntitites.Select(x => x.Bestellung_Nr ?? -1)?.ToList());

					foreach(var articleItem in articleEntitites)
					{
						var orderItem = orderEntities?.Find(x => x.Nr == articleItem.Bestellung_Nr);
						if(orderItem != null)
						{
							responseBody.Add(new Models.Article.ArticleOverviewModel.BlanketDetail(articleItem, orderItem));
						}
					}
				}

				// -
				return ResponseModel<List<Models.Article.ArticleOverviewModel.BlanketDetail>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.ArticleOverviewModel.BlanketDetail>> Validate()
		{
			if(this._user == null/* this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.ArticleOverviewModel.BlanketDetail>>.AccessDeniedResponse();
			}

			// -
			return ResponseModel<List<Models.Article.ArticleOverviewModel.BlanketDetail>>.SuccessResponse();
		}
	}
}
