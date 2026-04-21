using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	public class GetAuschusskosten_Technik_InfoHandler: IHandle<Identity.Models.UserModel, ResponseModel<GetAuschusskosten_Technik_InfoSearchModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private AusschusskostenInfoSearchModel _data { get; set; }
		public GetAuschusskosten_Technik_InfoHandler(AusschusskostenInfoSearchModel _data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = _data;
		}
		public ResponseModel<GetAuschusskosten_Technik_InfoSearchModel> Handle()
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
						case "datum":
							sortFieldName = "Lagerbewegungen.Datum";
							break;
						case "typ":
							sortFieldName = "Lagerbewegungen.Typ";
							break;
						case "artikelnummer":
							sortFieldName = "Artikel.Artikelnummer";
							break;
						case "bezeichnung1":
							sortFieldName = "Artikel.[Bezeichnung 1]";
							break;
						case "anzahl":
							sortFieldName = "Lagerbewegungen_Artikel.Anzahl";
							break;
						case "einheit":
							sortFieldName = "Lagerbewegungen_Artikel.Einheit";
							break;
						case "lagerort":
							sortFieldName = "Lagerorte.Lagerort";
							break;
						case "fertigungsnummer":
							sortFieldName = "Lagerbewegungen_Artikel.Fertigungsnummer";
							break;
						case "grund":
							sortFieldName = "Lagerbewegungen_Artikel.Grund";
							break;
						case "rollennummer":
							sortFieldName = " Lagerbewegungen_Artikel.Rollennummer ";
							break;
						case "kosten":
							sortFieldName = "Kosten";
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = this._data.SortDesc,
					};
				}

				var response = new List<GetAuschusskosten_Technik_InfoModel>();
				var PackingListEntity = Infrastructure.Data.Access.Joins.Logistics.GetAuschusskosten_Technik_InfoAcess.GetAuschusskosten_Technik_Info(
					this._data.LagerFertigung,
					this._data.LagerP,
					this._data.DateBegin,
					this._data.DateEnd,
					dataSorting,
					dataPaging,
					this._data.SearchValue
					);
				if(PackingListEntity != null && PackingListEntity.Count > 0)
					response = PackingListEntity.Select(k => new GetAuschusskosten_Technik_InfoModel(k)).ToList();
				int allCount = (response.Count() == 0 || response == null) ? 0 : response.Select(x => x.totalRows).First();
				return ResponseModel<GetAuschusskosten_Technik_InfoSearchModel>.SuccessResponse(
					new GetAuschusskosten_Technik_InfoSearchModel()
					{
						Auschusskosten = response,
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
		public ResponseModel<GetAuschusskosten_Technik_InfoSearchModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<GetAuschusskosten_Technik_InfoSearchModel>.AccessDeniedResponse();
			}

			return ResponseModel<GetAuschusskosten_Technik_InfoSearchModel>.SuccessResponse();
		}
	}
}
