using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	public class GetProformaListHandler: IHandle<Identity.Models.UserModel, ResponseModel<ProformaModelSearch>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private RohmaterialSearch _data { get; set; }
		public GetProformaListHandler(RohmaterialSearch _data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = _data;
		}
		public ResponseModel<ProformaModelSearch> Handle()
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
						case "bezeichnung1":
							sortFieldName = "[Bezeichnung 1]";
							break;
						case "bestand":
							sortFieldName = "Lager.Bestand";
							break;
						case "einheit":
							sortFieldName = "Einheit";
							break;
						case "ek":
							sortFieldName = "[EK]";
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
						case "zolltarif_nr":
							sortFieldName = "Artikel.Zolltarif_nr";
							break;
						case "ursprungsland":
							sortFieldName = "Artikel.Ursprungsland";
							break;

						case "name1":
							sortFieldName = "adressen.Name1";
							break;
						case "praeferenz_aktuelles_jahr":
							sortFieldName = "Praeferenz_Aktuelles_jahr";
							break;
						case "standardlieferant":
							sortFieldName = "Standardlieferant";
							break;

					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = this._data.SortDesc,
					};
				}

				var response = new List<ProformaModel>();
				var PackingListEntity = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetProformaList(
					this._data.Mindestlagerbestand,
					this._data.Lagerort_id,
					dataPaging,
					dataSorting,
					this._data.SearchValue
					);
				if(PackingListEntity != null && PackingListEntity.Count > 0)
					response = PackingListEntity.Select(k => new ProformaModel(k)).ToList();
				int allCount = (response.Count() == 0 || response == null) ? 0 : response.Select(x => x.totalRows).First();
				return ResponseModel<ProformaModelSearch>.SuccessResponse(
			new ProformaModelSearch()
			{
				ProformaModel = response,
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
		public ResponseModel<ProformaModelSearch> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<ProformaModelSearch>.AccessDeniedResponse();
			}

			return ResponseModel<ProformaModelSearch>.SuccessResponse();
		}
	}
}
