using System.Collections.Generic;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.HistoryFG;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.HistoryFG
{
		public class GetHistoryHeaderFGBestandHandler: IHandle<UserModel, ResponseModel<HistoryFGHeaderResponseModel>>
	{
		private readonly HistoryFGHeaderRequestModel? _request;
		private readonly UserModel? _user;
		public GetHistoryHeaderFGBestandHandler(HistoryFGHeaderRequestModel request, Core.Identity.Models.UserModel user)
		{
			_request = request;
			_user = user;
		}

		public ResponseModel<HistoryFGHeaderResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var HistoryFGHeaderList = new List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity>();

				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				Infrastructure.Data.Access.Settings.PaginModel dataPaging = null;
				if(!string.IsNullOrWhiteSpace(this._request.SortField))
				{
					var sortFieldName = "";
					switch(this._request.SortField.ToLower())
					{
						default:
						case "id":
							sortFieldName = "[Id]";
							break;
						case "createdate":
							sortFieldName = "[CreateDate]";
							break;
						case "createuserid":
							sortFieldName = "[CreateUserId]";
							break;
						case "customername":
							sortFieldName = "[CustomerName]";
							break;		
						case "customernumber":
							sortFieldName = "[CustomerNumber]";
							break;
						case "documenttype":
							sortFieldName = "[DocumentType]";
							break;
						case "Importdate":
							sortFieldName = "[ImportDate]";
							break;
						case "importtype":
							sortFieldName = "[ImportType]";
							break;
						case "logobjectid":
							sortFieldName = "[LogObjectId]";
							break;
					}

				dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = this._request.SortDesc,
					};
				}
				var customersUser = new CustomerService.Handlers.OrderProcessing.GetMyCustomersHandler(null, _user).Handle().Body ?? new List<CustomerService.Models.OrderProcessing.CustomerModel>();
				var usersCustomersNumbers= customersUser?.Select(c=>c.AdressCustomerNumber??-1).ToList();

				var filtredNumbers = _request.CustomerNumber is not null
					? new List<int> { _request.CustomerNumber ?? -1 }
					: usersCustomersNumbers;

				int totalCount = Infrastructure.Data.Access.Tables.CRP.HistoryFG.HistoryHeaderFGBestandAccess.CountHistoryFGHeaderData(_request.From,_request.To);

				dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this._request.PageSize > 0 ? (this._request.RequestedPage * this._request.PageSize) : 0,
					RequestRows = this._request.FullData ? totalCount : this._request.PageSize

				};


				HistoryFGHeaderList = Infrastructure.Data.Access.Tables.CRP.HistoryFG.HistoryHeaderFGBestandAccess.GetHistoryFGHeaderData(_request.From, _request.To, dataSorting, dataPaging);
				var r = this._request.PageSize > 0 ? totalCount / decimal.Parse((this._request.PageSize).ToString()) : 0;
				if(HistoryFGHeaderList is null || HistoryFGHeaderList.Count == 0)
					return ResponseModel<HistoryFGHeaderResponseModel>.SuccessResponse(new HistoryFGHeaderResponseModel
					{
						Items = new List<HistoryDataFGHeaderResponseModel>(),
						PageRequested = this._request.RequestedPage,
						PageSize = this._request.PageSize,
						TotalCount = totalCount,
						TotalPageCount = (int)Math.Ceiling(decimal.Parse(r.ToString())),
					});

				var result = HistoryFGHeaderList.Select(x => new HistoryDataFGHeaderResponseModel(x)).ToList();


				return ResponseModel<HistoryFGHeaderResponseModel>.SuccessResponse(new HistoryFGHeaderResponseModel
				{
					Items = result,
					PageRequested = this._request.RequestedPage,
					PageSize = this._request.PageSize,
					TotalCount = totalCount,
					TotalPageCount = (int)Math.Ceiling(decimal.Parse(r.ToString())),
				});


			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<HistoryFGHeaderResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<HistoryFGHeaderResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<HistoryFGHeaderResponseModel>.SuccessResponse();
		}
	}
}
