using Psz.Core.MaterialManagement.Models;
using Psz.Core.MaterialManagement.Orders.Models.Orders;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Rahmen
{
	public class SearchRahmenHandler: IHandle<Identity.Models.UserModel, ResponseModel<RahmenPositionsConsumptionResponseModel>>
	{

		private RahmenListRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public SearchRahmenHandler(Identity.Models.UserModel user, RahmenListRequestModel data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<RahmenPositionsConsumptionResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new RahmenListResponseModel();
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = _data.PageSize > 0 ? (_data.RequestedPage * _data.PageSize) : 0,
					RequestRows = _data.PageSize
				};
				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				if(!string.IsNullOrWhiteSpace(_data.SortField))
				{
					var sortFieldName = "";
					switch(_data.SortField.Trim().ToLower())
					{
						case "position":
							sortFieldName = "aa.Position";
							break;
						case "forfailnr":
							sortFieldName = "a.[Angebot-Nr]";
							break;
						case "material":
							sortFieldName = "ar.Artikelnummer";
							break;
						case "bezeichnung":
							sortFieldName = "aa.Bezeichnung1";
							break;
						case "zielmenge":
							sortFieldName = "aa.OriginalAnzahl";
							break;
						case "delivredquantity":
							sortFieldName = "aa.Geliefert";
							break;
						case "restquantity":
							sortFieldName = "aa.Anzahl";
							break;
						case "consumption":
							sortFieldName = "Consumption";
							break;
						case "gesamtpreisdefault":
							sortFieldName = "bea.GesamtpreisDefault";
							break;
						case "dateofexpiry":
							sortFieldName = "bea.GultigBis";
							break;
						case "extensiondate":
							sortFieldName = "bea.ExtensionDate";
							break;
						case "state":
							sortFieldName = "be.StatusName";
							break;
						case "projekt_nr":
							sortFieldName = "a.[Projekt-Nr]";
							break;
						case "needed_in_fg_bom":
							sortFieldName = "NeededInBOM";
							break;
						case "needed_in_rahmen_sales":
							sortFieldName = "SumNeeded";
							break;
						case "documentnumber":
							sortFieldName = "Bezug";
							break;
						case "supplier":
							sortFieldName = "[Vorname/NameFirma]";
							break;
						default:
							sortFieldName = "Consumption";
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = _data.SortDesc,
					};
				}

				var entities = Infrastructure.Data.Access.Joins.MTM.Order.DashboardAccess.GetRahmenPositionsConsumption(_data.ArtikelNr, _data.ProjectNumber,
					int.TryParse(_data.PrefailNumber, out var p) ? p : null, _data.DocumentNumber, _data.SuppliersIds, _data.OnlyExpired, _data.Status, dataSorting, dataPaging);
				var allCount = Infrastructure.Data.Access.Joins.MTM.Order.DashboardAccess.GetRahmenPositionsConsumptionCount(_data.ArtikelNr, _data.ProjectNumber,
					int.TryParse(_data.PrefailNumber, out var p1) ? p1 : null, _data.DocumentNumber, _data.SuppliersIds, _data.OnlyExpired, _data.Status);

				return ResponseModel<RahmenPositionsConsumptionResponseModel>.SuccessResponse(
					new RahmenPositionsConsumptionResponseModel()
					{
						Items = entities?.Select(e => new RahmenPositionsConsumptionModel(e)).ToList(),
						PageRequested = _data.RequestedPage,
						PageSize = _data.PageSize,
						TotalCount = allCount > 0 ? allCount : 0,
						TotalPageCount = _data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(allCount > 0 ? allCount : 0) / _data.PageSize)) : 0,
					});
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<RahmenPositionsConsumptionResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<RahmenPositionsConsumptionResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<RahmenPositionsConsumptionResponseModel>.SuccessResponse();
		}

	}
}
