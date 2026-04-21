using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	public class ExcessRohmaterialHandle: IHandle<Identity.Models.UserModel, ResponseModel<ExcessRohmaterialSearchListModel>>
	{
		//GetExcessRohmaterialEntity
		private Identity.Models.UserModel _user { get; set; }
		private ExcessRohmaterialSearchModel _data { get; set; }
		public ExcessRohmaterialHandle(Identity.Models.UserModel user, ExcessRohmaterialSearchModel _data)
		{
			this._user = user;
			this._data = _data;

		}
		public ResponseModel<ExcessRohmaterialSearchListModel> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			try
			{
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this._data.ItemsPerPage > 0 ? (this._data.RequestedPage * this._data.ItemsPerPage) : 0,
					RequestRows = this._data.ItemsPerPage
				};
				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				if(!string.IsNullOrWhiteSpace(this._data.SortFieldKey))
				{
					var sortFieldName = "";
					switch(this._data.SortFieldKey.ToLower())
					{
						default:

						case "ArtikelNr":
							sortFieldName = "[Artikel-Nr]";
							break;
						case "Artikelnummer":
							sortFieldName = "Artikelnummer";
							break;
						case "Bezeichnung1":
							sortFieldName = "[Bezeichnung 1]";
							break;
						case "SummevonBestand":
							sortFieldName = "SummevonBestand";
							break;
						case "Einkaufspreis":
							sortFieldName = "Einkaufspreis";
							break;
						case "Kosten":
							sortFieldName = "Kosten";
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = this._data.SortDesc,
					};
				}
				var response = new List<ExcessRohmaterialModel>();
				var PackingListEntity = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetExcessRohmaterialEntity(
					this._data.TageOhneBewegung,
					this._data.LagerNummer,
					dataSorting,
					dataPaging,
					this._data.SearchValue
					);
				if(PackingListEntity != null && PackingListEntity.Count > 0)
					response = PackingListEntity.Select(k => new ExcessRohmaterialModel(k)).ToList();
				int allCount = (response.Count() == 0 || response == null) ? 0 : response.Select(x => x.totalRows).First();
				return ResponseModel<ExcessRohmaterialSearchListModel>.SuccessResponse(
			new ExcessRohmaterialSearchListModel()
			{
				ExcessRohmaterialModel = response,
				RequestedPage = this._data.RequestedPage,
				ItemsPerPage = this._data.ItemsPerPage,
				AllCount = allCount > 0 ? allCount : 0,
				AllPagesCount = this._data.ItemsPerPage > 0 ? (int)Math.Ceiling(((decimal)(allCount > 0 ? allCount : 0) / this._data.ItemsPerPage)) : 0,
			});
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<ExcessRohmaterialSearchListModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<ExcessRohmaterialSearchListModel>.AccessDeniedResponse();
			}

			return ResponseModel<ExcessRohmaterialSearchListModel>.SuccessResponse();
		}
	}
}
