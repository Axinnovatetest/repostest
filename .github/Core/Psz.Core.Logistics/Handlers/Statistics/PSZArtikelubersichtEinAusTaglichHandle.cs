using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	public class PSZArtikelubersichtEinAusTaglichHandle: IHandle<Identity.Models.UserModel, ResponseModel<PSZArtikelubersichtEinAusTaglichEntityModelSearch>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private WEnachDatumSearch _data { get; set; }
		public PSZArtikelubersichtEinAusTaglichHandle(WEnachDatumSearch _data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = _data;
		}
		public ResponseModel<PSZArtikelubersichtEinAusTaglichEntityModelSearch> Handle()
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

						case "artikelnummer":
							sortFieldName = "Artikel.Artikelnummer";
							break;
						case "typ":
							sortFieldName = "Typ";
							break;
						case "bestellungnr":
							sortFieldName = "[Bestellung-Nr]";
							break;
						case "anzahl":
							sortFieldName = "Anzahl";
							break;
						case "datum":
							sortFieldName = "Datum";
							break;
						case "name1":
							sortFieldName = "Name1";
							break;
						case "lagerplatz_von":
							sortFieldName = "Lagerplatz_von";
							break;
						case "lagerplatz_nach":
							sortFieldName = "Lagerplatz_nach";
							break;
						case "mindestbestellmenge":
							sortFieldName = "Mindestbestellmenge";
							break;
						case "verpackungseinheit":
							sortFieldName = "Verpackungseinheit";
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = this._data.SortDesc,
					};
				}

				var response = new List<PSZArtikelubersichtEinAusTaglichEntityModel>();
				var PackingListEntity = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetWEnachDatumList(
					this._data.DateBegin,
					this._data.DateEnd,
					dataSorting,
					dataPaging,
					this._data.SearchValue
					);
				if(PackingListEntity != null && PackingListEntity.Count > 0)
					response = PackingListEntity.Select(k => new PSZArtikelubersichtEinAusTaglichEntityModel(k)).ToList();
				int allCount = (response.Count() == 0 || response == null) ? 0 : response.Select(x => x.totalRows).First();
				return ResponseModel<PSZArtikelubersichtEinAusTaglichEntityModelSearch>.SuccessResponse(
			new PSZArtikelubersichtEinAusTaglichEntityModelSearch()
			{
				PSZArtikelubersichtEinAusTaglichEntityModel = response,
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
		public ResponseModel<PSZArtikelubersichtEinAusTaglichEntityModelSearch> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<PSZArtikelubersichtEinAusTaglichEntityModelSearch>.AccessDeniedResponse();
			}

			return ResponseModel<PSZArtikelubersichtEinAusTaglichEntityModelSearch>.SuccessResponse();
		}
	}
}
