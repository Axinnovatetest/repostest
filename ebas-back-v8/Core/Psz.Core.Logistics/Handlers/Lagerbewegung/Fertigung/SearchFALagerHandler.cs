using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Lagebewegung.Fertigung;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Lagerbewegung.Fertigung
{
	public class SearchFALagerHandler: IHandle<Models.Lagebewegung.Fertigung.FALagerSearchModel, ResponseModel<Models.Lagebewegung.Fertigung.FASearchResponseModel>>
	{
		private FALagerSearchModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public SearchFALagerHandler(FALagerSearchModel data, Identity.Models.UserModel user)
		{
			this._data = data;
			this._user = user;
		}
		public ResponseModel<Models.Lagebewegung.Fertigung.FASearchResponseModel> Handle()
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
				//if(!string.IsNullOrWhiteSpace(this._data.SortFieldKey))
				//{
				//	var sortFieldName = "";
				//	switch(this._data.SortFieldKey.ToLower())
				//	{
				//		default:
				//		case "fertigungsnummer":
				//			sortFieldName = "F.[Fertigungsnummer]";
				//			break;
				//		case "artikelnummer":
				//			sortFieldName = "A.[Artikelnummer]";
				//			break;
				//		case "freigabestatus":
				//			sortFieldName = "A.[Bezeichung 1]";
				//			break;
				//		case "anzahl":
				//			sortFieldName = "F.[Originalanzahl]";
				//			break;

				//	}

				//	dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
				//	{
				//		SortFieldName = sortFieldName,
				//		SortDesc = this._data.SortDesc,
				//	};
				//}

				#endregion

				var orders = new List<FALagerModel>();
				int allCount = 0;

				var FAEntities = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetListFertigung(
				   this._data.Fertigungsnummer,
				   dataSorting,
				   dataPaging);
				FAEntities = FAEntities.Distinct().ToList();

				if(FAEntities != null && FAEntities.Count > 0)
				{
					allCount = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetListFertigungCount(

				   this._data.Fertigungsnummer
				   );


					orders = FAEntities.Select(x => new FALagerModel(x)).ToList();
				}

				return ResponseModel<Models.Lagebewegung.Fertigung.FASearchResponseModel>.SuccessResponse(
					new Models.Lagebewegung.Fertigung.FASearchResponseModel()
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
		public ResponseModel<Models.Lagebewegung.Fertigung.FASearchResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<Models.Lagebewegung.Fertigung.FASearchResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<Models.Lagebewegung.Fertigung.FASearchResponseModel>.SuccessResponse();
		}
	}
}
