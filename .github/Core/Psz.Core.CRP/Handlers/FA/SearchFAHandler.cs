using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA
{
	public class SearchFAHandler: IHandle<Models.FA.FASearchModel, ResponseModel<Models.FA.FASearchResponseModel>>
	{
		private FASearchModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public SearchFAHandler(FASearchModel data, Identity.Models.UserModel user)
		{
			this._data = data;
			this._user = user;
		}
		public ResponseModel<Models.FA.FASearchResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				#region > Data sorting & paging
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
						case "fertigungsnummer":
							sortFieldName = "F.[Fertigungsnummer]";
							break;
						case "artikelnummer":
							sortFieldName = "A.[Artikelnummer]";
							break;
						case "bezeichung_1":
							sortFieldName = "A.[Bezeichung 1]";
							break;
						case "fa_menge":
							sortFieldName = "F.[Originalanzahl]";
							break;
						case "fa_status":
							sortFieldName = "F.[Planungsstatus]";
							break;
						case "produktionstermin":
							sortFieldName = "F.[Termin_Bestätigt1]";
							break;
						case "lager":
							sortFieldName = "F.[Lagerort_id]";
							break;
						case "gestart":
							sortFieldName = "F.[FA_Gestartet]";
							break;
						case "prio":
							sortFieldName = "F.[Prio]";
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = this._data.SortDesc,
					};
				}

				#endregion

				var orders = new List<FAListModule>();
				int allCount = 0;

				var FAEntities = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetListFertigung(
				   this._data.Kunde,
				   this._data.Artikelnummer,
				   this._data.Fertigungsnummer,
				   this._data.FA_Status,
				   this._data.Lager,
				   this._data.Gestart,
				   _data.Prio,
				   dataSorting,
				   dataPaging);
				FAEntities = FAEntities.Distinct().ToList();

				if(FAEntities != null && FAEntities.Count > 0)
				{
					allCount = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetListFertigungCount(
				   this._data.Kunde,
				   this._data.Artikelnummer,
				   this._data.Fertigungsnummer,
				   this._data.FA_Status,
				   this._data.Lager,
				   this._data.Gestart,
				   _data.Prio
				   );

					orders = FAEntities.Select(x => new FAListModule(x)).ToList();
				}

				return ResponseModel<Models.FA.FASearchResponseModel>.SuccessResponse(
					new Models.FA.FASearchResponseModel()
					{
						Orders = orders,
						RequestedPage = this._data.RequestedPage,
						ItemsPerPage = this._data.ItemsPerPage,
						AllCount = allCount > 0 ? allCount : 0,
						AllPagesCount = this._data.ItemsPerPage > 0 ? (int)Math.Ceiling(((decimal)(allCount > 0 ? allCount : 0) / this._data.ItemsPerPage)) : 0,
					});
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<Models.FA.FASearchResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<Models.FA.FASearchResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<Models.FA.FASearchResponseModel>.SuccessResponse();
		}
	}
}