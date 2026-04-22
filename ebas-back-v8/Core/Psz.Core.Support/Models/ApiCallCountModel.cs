using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Support.Models
{
	public class ApiCallCountModel
	{
		public string Api { get; set; }
		public string Url { get; set; }
		public int? CallCountAll { get; set; }
		public int? Calls_Last_3_Months { get; set; }
		public int? Calls_Last_6_Months { get; set; }
		public int? Calls_Last_12_Months { get; set; }
		public ApiCallCountModel(Infrastructure.Data.Entities.Tables.NLogs.__ERP_Nlog_ApisCallsEntity entity)
		{
			Api = entity.Api;
			Url = entity.Url;
			CallCountAll = entity.calls_all;
			Calls_Last_3_Months = entity.calls_last_3_months;
			Calls_Last_6_Months = entity.calls_last_6_months;
			Calls_Last_12_Months = entity.calls_last_12_months;
		}
	}

	public class ApiCallCountResponseModel: IPaginatedResponseModel<ApiCallCountModel> { }
	public class ApiCallCountRequestModel: IPaginatedRequestModel {
		public string? SearchValue { get; set; }
	}

	public class ApiCallCountSixMonthModel
	{
		public string MonthYear { get; set; }
		public int? CallCount { get; set; }
		public ApiCallCountSixMonthModel()
		{

		}
		public ApiCallCountSixMonthModel(Infrastructure.Data.Entities.Tables.NLogs.ApiCallsSixMonthsPriorEntity entity)
		{
			MonthYear = entity.MonthYear;
			CallCount = entity.CallCount;
		}
	}
}