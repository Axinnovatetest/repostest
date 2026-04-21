using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	public class GetBestandlistLager: IHandle<Identity.Models.UserModel, ResponseModel<MainRequestModelSearch>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public BestandlistLagerSearchModel _data { get; set; }
		public GetBestandlistLager(Identity.Models.UserModel user, BestandlistLagerSearchModel _data)
		{
			this._user = user;
			this._data = _data;
		}
		public ResponseModel<MainRequestModelSearch> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				if(!string.IsNullOrWhiteSpace(this._data.SortFieldKey))
				{
					var sortFieldName = "";
					switch(this._data.SortFieldKey.ToLower())
					{
						default:
						case "sklad":
							sortFieldName = "Sklad";
							break;
						case "poslednitransakce":
							sortFieldName = "[Poslední Transakce]";
							break;
						case "cislopsz":
							sortFieldName = "[Cislo PSZ]";
							break;
						case "mnozstvi":
							sortFieldName = "Množství";
							break;
						case "cislovyrobce":
							sortFieldName = "[Cislo Vyrobce]";
							break;
						case "kontrolaok":
							sortFieldName = "[Kontrola ok?]";
							break;
						case "we_voh_id":
							sortFieldName = "WE_VOH_ID";
							break;

					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = this._data.SortDesc,
					};
				}
				var response = new List<MainRequestModel>();
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this._data.ItemsPerPage > 0 ? (this._data.RequestedPage * this._data.ItemsPerPage) : 0,
					RequestRows = this._data.ItemsPerPage
				};
				int allCount = 0;
				var PackingListEntity = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetBestandlistLagerAccess(this._data.LagerID, dataPaging, dataSorting, this._data.SearchValue);
				if(PackingListEntity != null && PackingListEntity.Count > 0)
					response = PackingListEntity.Select(k => new MainRequestModel(k)).ToList();
				allCount = (response.Count() == 0 || response == null) ? 0 : response.Select(x => x.totalNombre).First();
				return ResponseModel<MainRequestModelSearch>.SuccessResponse(
					new MainRequestModelSearch()
					{
						mainRequestModel = response,
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
		public ResponseModel<MainRequestModelSearch> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<MainRequestModelSearch>.AccessDeniedResponse();
			}

			return ResponseModel<MainRequestModelSearch>.SuccessResponse();
		}
	}
}
