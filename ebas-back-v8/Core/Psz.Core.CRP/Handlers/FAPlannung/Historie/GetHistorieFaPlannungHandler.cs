using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FAPlanning.Historie;
using Psz.Core.Identity.Models;

namespace Psz.Core.CRP.Handlers.FAPlannung
{
	public partial class CrpFAPlannung
	{
		public ResponseModel<Models.FAPlanning.Historie.FaPlannungHistorieDetailsResponsetModel> GetHistorieFaPlannungDetails(UserModel user, Models.FAPlanning.Historie.FaPlannungHistorieDetailsRequestModel data)
		{
			if(user == null)
				return ResponseModel<Models.FAPlanning.Historie.FaPlannungHistorieDetailsResponsetModel>.AccessDeniedResponse();

			try
			{
				#region > Data sorting & paging
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = data.PageSize > 0 ? (data.RequestedPage * data.PageSize) : 0,
					RequestRows = data.PageSize
				};
				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				if(!string.IsNullOrWhiteSpace(data.SortField))
				{
					var sortFieldName = "";
					switch(data.SortField.ToLower())
					{
						default:
						case "werk":
							sortFieldName = "P.[Werk]";
							break;
						case "customer":
							sortFieldName = "P.[Customer]";
							break;
						case "fa_number":
							sortFieldName = "P.[FA Number]";
							break;
						case "open_qty":
							sortFieldName = "P.[FA Qty]";
							break;
						case "pn_psz":
							sortFieldName = "P.[PN PSZ]";
							break;
						case "freigabestatus":
							sortFieldName = "P.[Freigabestatus]";
							break;
						case "order_time":
							sortFieldName = "P.[Order Time]";
							break;
						case "fa_druckdatum":
							sortFieldName = "P.[FA_Druckdatum]";
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = data.SortDesc,
					};
				}
				#endregion

				var entities = Infrastructure.Data.Access.Tables.CRP.__crp_historie_fa_plannung_detailsAccess.GetHistorieDetails(data.IdHeader, data.SearchText, dataPaging, dataSorting);
				var count = Infrastructure.Data.Access.Tables.CRP.__crp_historie_fa_plannung_detailsAccess.GetHistorieDetailsCount(data.IdHeader, data.SearchText);

				var response = new FaPlannungHistorieDetailsResponsetModel
				{
					Items = entities?.Select(e => new FaPlannungHistorieDetailsModel(e)).ToList(),
					PageRequested = data.RequestedPage,
					PageSize = data.PageSize,
					TotalCount = count > 0 ? count : 0,
					TotalPageCount = data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(count > 0 ? count : 0) / data.PageSize)) : 0,
				};

				return ResponseModel<Models.FAPlanning.Historie.FaPlannungHistorieDetailsResponsetModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}