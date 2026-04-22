using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	public class PSZ_PV_ListeHandler: IHandle<Identity.Models.UserModel, ResponseModel<PSZ_PV_ListeSearchModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private PVModelSearch _data { get; set; }
		public PSZ_PV_ListeHandler(Identity.Models.UserModel user, PVModelSearch _data)
		{
			this._user = user;
			this._data = _data;

		}
		public ResponseModel<PSZ_PV_ListeSearchModel> Handle()
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

						case "artikelnr":
							sortFieldName = "[Artikel-Nr]";
							break;
						case "artikelnummer":
							sortFieldName = "Artikelnummer";
							break;
						case "bezeichnung1":
							sortFieldName = "Bezeichnung1";
							break;
						case "bestand":
							sortFieldName = "Bestand";
							break;
						case "einheit":
							sortFieldName = "Einheit";
							break;
						case "lagerort":
							sortFieldName = "Lagerort";
							break;
						case "ek":
							sortFieldName = "EK";
							break;
						case "ek_summe":
							sortFieldName = "EK_Summe";
							break;
						case "gewicht":
							sortFieldName = "Gewicht";
							break;
						case "gesamtgewicht":
							sortFieldName = "Gesamtgewicht";
							break;
						case "zolltarifnr":
							sortFieldName = "Zolltarif_nr";
							break;
						case "ursprungsland":
							sortFieldName = "Ursprungsland";
							break;
						case "lieferantennr":
							sortFieldName = "[Lieferanten-Nr]";
							break;
						case "name1":
							sortFieldName = "Name1";
							break;
						case "bestellnr":
							sortFieldName = "[Bestell-Nr]";
							break;
						case "bezeichnungal":
							sortFieldName = "BezeichnungAL";
							break;
						case "praferenz":
							sortFieldName = "Präferenz";
							break;
						case "sendungsnummer":
							sortFieldName = "Sendungsnummer";
							break;

					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = this._data.SortDesc,
					};
				}
				var response = new List<PSZ_PV_ListeModel>();
				var PackingListEntity = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetPSZ_PV_ListeEntity(
					this._data.PVSendungsnummer,
					this._data.Lagernummer,
					dataSorting,
					dataPaging, this._data.SearchValue);
				if(PackingListEntity != null && PackingListEntity.Count > 0)
					response = PackingListEntity.Select(k => new PSZ_PV_ListeModel(k)).ToList();
				int allCount = (response.Count() == 0 || response == null) ? 0 : response.Select(x => x.totalRows).First();
				return ResponseModel<PSZ_PV_ListeSearchModel>.SuccessResponse(
			new PSZ_PV_ListeSearchModel()
			{
				PSZ_PV_ListeModel = response,
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

		public ResponseModel<PSZ_PV_ListeSearchModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<PSZ_PV_ListeSearchModel>.AccessDeniedResponse();
			}

			return ResponseModel<PSZ_PV_ListeSearchModel>.SuccessResponse();
		}
	}
}
