using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.CTS.Models;
using Psz.Core.ManagementOverview.Statistics.Enums;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.ManagementOverview.CTS.Handlers
{
	public class GetCTSDashboardSummaryTopHandler: IHandle<Identity.Models.UserModel, ResponseModel<DashboardSummaryTopResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private ManagementOverview.CTS.Models.DashboardTopRequestModel _data { get; set; }

		public GetCTSDashboardSummaryTopHandler(Identity.Models.UserModel user, ManagementOverview.CTS.Models.DashboardTopRequestModel data)
		{
			this._user = user;
			_data = data;
		}

		public ResponseModel<DashboardSummaryTopResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var responseBody = new DashboardSummaryTopResponseModel();
				var date = this._data.DateTill ?? DateTime.Today.AddDays(7 * 5);
				var customerGroups = Infrastructure.Data.Access.Joins.MGO.MainViewsAccess.GetCTSCustomersGroupesAccess();
				var customerGroupNames = customerGroups?.Select(c => c.Stufe)?.Distinct().ToList();
				if(customerGroupNames != null && customerGroupNames.Count > 0)
				{
					var customerGroupData = new List<DashboardSummaryGroupTopResponseModel>();
					foreach(var item in customerGroupNames)
					{
						var customers = customerGroups.Where(x => x.Stufe == item)?.ToList();
						var customerNumbers = customers.Select(x => x.Nr).ToList();
						var customersGroupData = Infrastructure.Data.Access.Joins.MGO.MainViewsAccess.GetCTSDashboardSummaryByCustomerGroupAccess(date, customerNumbers);
						// - 
						customerGroupData.Add(new DashboardSummaryGroupTopResponseModel
						{
							SuspiciousArticles = Infrastructure.Data.Access.Joins.MGO.MainViewsAccess.GetCTSDashboardAccess(date, customerNumbers, true)?.Select(x => new CTSDashboardItemModel(x))?.ToList(),
							CustomerGroupClass = item,
							DateTill = date,
							CustomerGroupOrder = 0,
							CustomersName = customers.Select(x => x.Name).ToList(),
							TotalAmount = customersGroupData.TotalAmount ?? 0,
							ImmediatAmount = customersGroupData.ImmediatAmount ?? 0,
							ProductionAmount = customersGroupData.ProductionAmount ?? 0,
							Results = (customersGroupData.ImmediatAmount ?? 0) + (customersGroupData.ProductionAmount ?? 0),
							CustomersData = null // - REM! too heavy - compute data by Customer
						});
					}

					// - sort data & assign order
					if(customerGroupNames.Count > 1)
					{
						var _temp = customerGroupData.OrderByDescending(x => x.TotalAmount).ToList();
						for(int i = 0; i < _temp.Count; i++)
						{
							var idx = customerGroupData.FindIndex(x => x.CustomerGroupClass == _temp[i].CustomerGroupClass);
							if(idx >= 0)
							{
								customerGroupData[idx].CustomerGroupOrder = i + 1;
							}
						}
					}

					// - 
					var idy = customerGroupData.FindIndex(x => string.IsNullOrWhiteSpace(x.CustomerGroupClass));
					if(idy >= 0)
					{
						customerGroupData[idy].CustomerGroupClass = "Others";
					}

					// - 
					responseBody.CustomerGroupItems = customerGroupData;
					responseBody.DateTill = date;
				}

				// -
				return ResponseModel<DashboardSummaryTopResponseModel>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<DashboardSummaryTopResponseModel> HandleSec()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var responseBody = new DashboardSummaryTopResponseModel();
				var date = this._data.DateTill ?? DateTime.Today.AddDays(7 * 5);



				responseBody.DateTill = date;
				string res = Infrastructure.Data.Access.Joins.MGO.MainViewsAccess.GetCTSDashboardSecAccess((int)StatisticType.Dashboard, date);
				List<DashboardSummaryGroupTopResponseModel> lsCustomers = DBHelper<List<DashboardSummaryGroupTopResponseModel>>.Get(res);
				if(lsCustomers is not null)
				{
					foreach(DashboardSummaryGroupTopResponseModel group in lsCustomers)
					{
						if(group.DateLastRefresh is not null && responseBody.DateLastRefresh is null)
						{
							responseBody.DateLastRefresh = group.DateLastRefresh;
						}

						if(group.CustomersData is not null)
						{
							group.CustomersName = new List<string>();
							group.CustomersName = group.CustomersData.Select(x => x.CustomerName).ToList();
						}
						else
						{
							group.CustomersData = new List<DashboardSummaryResponseModel>();
						}

						if(group.SuspiciousArticles is null)
						{
							group.SuspiciousArticles = new List<CTSDashboardItemModel>();
						}

					}
				}
				responseBody.CustomerGroupItems = lsCustomers;

				return ResponseModel<DashboardSummaryTopResponseModel>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<DashboardSummaryTopResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<DashboardSummaryTopResponseModel>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<DashboardSummaryTopResponseModel>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<DashboardSummaryTopResponseModel>.SuccessResponse();
		}
	}

	public class DBHelper<T>
	{
		public static T Get(string res)
		{
			return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(res);
		}
	}
}
