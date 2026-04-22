using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Verpackung
{
	public class GetPaginationListHistoriePackingHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Verpackung.HistoriePackingResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Verpackung.HistoriePackingSearchModel _data { get; set; }
		public GetPaginationListHistoriePackingHandler(Models.Verpackung.HistoriePackingSearchModel data, Identity.Models.UserModel user)
		{
			this._data = data;
			this._user = user;
		}
		public ResponseModel<Models.Verpackung.HistoriePackingResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this._data.ItemsPerPage > 0 ? (this._data.RequestedPage * this._data.ItemsPerPage) : 0,
					RequestRows = this._data.ItemsPerPage
				};

				int allCount = 0;
				var listeVerpackung = new List<Models.Verpackung.HistoriePackingModel>();

				var VerpackungListEntity = Infrastructure.Data.Access.Joins.Logistics.HistoriePackingAccess.GetPaginationListeHistoriePacking(this._data.SearchTerms, this._data.SortFieldKey, this._data.SortDesc, dataPaging);

				foreach(var verpackungEntity in VerpackungListEntity)
				{

					listeVerpackung.Add(new Models.Verpackung.HistoriePackingModel(verpackungEntity));
				}
				if(VerpackungListEntity.Count() > 0)
				{
					allCount = VerpackungListEntity[0].nombreTotal;
				}


				return ResponseModel<Models.Verpackung.HistoriePackingResponseModel>.SuccessResponse(
					new Models.Verpackung.HistoriePackingResponseModel()
					{
						listVerpackung = listeVerpackung,
						RequestedPage = this._data.RequestedPage,
						ItemsPerPage = this._data.ItemsPerPage,
						AllCount = allCount > 0 ? allCount : 0,
						AllPagesCount = this._data.ItemsPerPage > 0 ? (int)Math.Ceiling(((decimal)(allCount > 0 ? allCount : 0) / this._data.ItemsPerPage)) : 0,
					});
				;
				;
				;// ;

			} catch(Exception e)
			{
				throw;
			}
		}
		public ResponseModel<Models.Verpackung.HistoriePackingResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<Models.Verpackung.HistoriePackingResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<Models.Verpackung.HistoriePackingResponseModel>.SuccessResponse();
		}
	}
}
