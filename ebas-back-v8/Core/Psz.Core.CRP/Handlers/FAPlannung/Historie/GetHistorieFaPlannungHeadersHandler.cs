using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FAPlanning.Historie;
using Psz.Core.Identity.Models;

namespace Psz.Core.CRP.Handlers.FAPlannung
{
	public partial class CrpFAPlannung
	{
		public ResponseModel<FaPlannungHistorieHeadersResponseModel> GetHistorieFaPlannungHeaders(UserModel user, FaPlannungHistorieHeadersRequestModel data)
		{
			if(user == null)
				return ResponseModel<FaPlannungHistorieHeadersResponseModel>.AccessDeniedResponse();

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
						case "importuser":
							sortFieldName = "[ImportUsername]";
							break;
						case "importtype":
							sortFieldName = "[ImportTyeName]";
							break;
						case "dateimport":
							sortFieldName = "[DateImport]";
							break;
						case "datehistorie":
							sortFieldName = "[DateHistorie]";
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = data.SortDesc,
					};
				}
				#endregion

				var entities = Infrastructure.Data.Access.Tables.CRP.__crp_historie_fa_plannung_headerAccess.GetHistorieHeaders(data.From, data.To, dataPaging, dataSorting);
				var count = Infrastructure.Data.Access.Tables.CRP.__crp_historie_fa_plannung_headerAccess.GetHistorieHeadersCount(data.From, data.To);

				var response = new FaPlannungHistorieHeadersResponseModel
				{
					Items = entities?.Select(e => new FaPlannungHistorieHeadersModel(e)).ToList(),
					PageRequested = data.RequestedPage,
					PageSize = data.PageSize,
					TotalCount = count > 0 ? count : 0,
					TotalPageCount = data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(count > 0 ? count : 0) / data.PageSize)) : 0,
				};

				return ResponseModel<FaPlannungHistorieHeadersResponseModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}