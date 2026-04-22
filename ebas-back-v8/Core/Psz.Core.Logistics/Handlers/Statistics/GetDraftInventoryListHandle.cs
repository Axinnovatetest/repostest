using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	public class GetDraftInventoryListHandle: IHandle<Identity.Models.UserModel, ResponseModel<DraftInventoryListModelSearch>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private DraftInventory _data { get; set; }
		public GetDraftInventoryListHandle(DraftInventory _data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = _data;
		}
		public ResponseModel<DraftInventoryListModelSearch> Handle()
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
				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				if(!string.IsNullOrWhiteSpace(this._data.SortFieldKey))
				{
					var sortFieldName = "";
					switch(this._data.SortFieldKey.ToLower())
					{
						default:

						case "artikelnr":
							sortFieldName = "[Artikel-Nr]";
							break;
						case "artikelnummer":
							sortFieldName = "Artikel.Artikelnummer";
							break;
						case "bezeichnung1":
							sortFieldName = "[Bezeichnung 1]";
							break;
						case "storageid":
							sortFieldName = "[Storage-ID]";
							break;
						case "quantityp3000":
							sortFieldName = "[Quantity P3000]";
							break;
						case "inventurquantity":
							sortFieldName = "[Inventur Quantity]";
							break;
						case "difference":
							sortFieldName = "Difference";
							break;
						case "letztebewegung":
							sortFieldName = "[letzte Bewegung]";
							break;
						case "ccid_datum":
							sortFieldName = "CCID_Datum";
							break;

					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = this._data.SortDesc,
					};
				}

				var response = new List<DraftInventoryListModel>();
				var PackingListEntity = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetDraftInventory(
					this._data.lagerOrt,
					dataSorting,
					dataPaging,
					this._data.SearchValue
					);
				if(PackingListEntity != null && PackingListEntity.Count > 0)
					response = PackingListEntity.Select(k => new DraftInventoryListModel(k)).ToList();
				int allCount = (response.Count() == 0 || response == null) ? 0 : response.Select(x => x.totalRows).First();
				return ResponseModel<DraftInventoryListModelSearch>.SuccessResponse(
			new DraftInventoryListModelSearch()
			{
				DraftInventoryListModel = response,
				RequestedPage = this._data.RequestedPage,
				ItemsPerPage = this._data.ItemsPerPage,
				AllCount = allCount > 0 ? allCount : 0,
				AllPagesCount = this._data.ItemsPerPage > 0 ? (int)Math.Ceiling(((decimal)(allCount > 0 ? allCount : 0) / this._data.ItemsPerPage)) : 0,
			}
			);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}

		}
		public ResponseModel<DraftInventoryListModelSearch> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<DraftInventoryListModelSearch>.AccessDeniedResponse();
			}

			return ResponseModel<DraftInventoryListModelSearch>.SuccessResponse();
		}
	}
}
