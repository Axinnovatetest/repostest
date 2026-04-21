using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FAPlanning;
using Psz.Core.Identity.Models;

namespace Psz.Core.CRP.Handlers.FAPlannung
{
	public partial class CrpFAPlannung
	{
		public ResponseModel<FaultyNeedsResponseModel> ValidateGetFaultyNeeds(UserModel user)
		{
			if(user == null)
				return ResponseModel<FaultyNeedsResponseModel>.AccessDeniedResponse();
			return ResponseModel<FaultyNeedsResponseModel>.SuccessResponse();
		}
		public ResponseModel<FaultyNeedsResponseModel> GetFaultyNeeds(UserModel user, FaultyNeedsResquestModel data)
		{
			try
			{
				var validationResponse = ValidateGetFaultyNeeds(user);
				if(!validationResponse.Success)
					return validationResponse;

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
						case "vorfallNr":
							sortFieldName = "[vorfallNr]";
							break;
						case "DocumentNumber":
							sortFieldName = "[DocumentNumber]";
							break;
						case "DeliveryDate":
							sortFieldName = "[DeliveryDate]";
							break;
						case "Quantity":
							sortFieldName = "[Quantity]";
							break;
					}
					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = data.SortDesc,
					};
				}
				#endregion

				var faultyEntities = Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.GetFaultyNeeds(data.SearchTerms, data.Ubg ?? false, dataSorting, dataPaging);
				var faltyCount = Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.GetFaultyNeedsCount(data.SearchTerms, data.Ubg ?? false);
				var response = new FaultyNeedsResponseModel
				{
					Items = faultyEntities?.Select(x => new FaultyNeeds(x)).ToList(),
					PageRequested = data.RequestedPage,
					PageSize = data.PageSize,
					TotalCount = faltyCount > 0 ? faltyCount : 0,
					TotalPageCount = data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(faltyCount > 0 ? faltyCount : 0) / data.PageSize)) : 0,
				};

				return ResponseModel<FaultyNeedsResponseModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}