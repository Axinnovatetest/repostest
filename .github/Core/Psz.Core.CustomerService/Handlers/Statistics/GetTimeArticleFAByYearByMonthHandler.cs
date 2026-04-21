using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Statistics
{
	public class GetTimeArticleFAByYearByMonthHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<TimeArticleResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Statistics.StatRequestModel _data { get; set; }
		public GetTimeArticleFAByYearByMonthHandler(Identity.Models.UserModel user, Models.Statistics.StatRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<TimeArticleResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var statEntityTimeAREntity = Infrastructure.Data.Access.Joins.CTS.StatisticFAAccess.GetFA12(this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month);
				List<TimeArticleResponseModel> response = new List<TimeArticleResponseModel>();

				if(statEntityTimeAREntity != null && statEntityTimeAREntity.Count > 0)
					response = statEntityTimeAREntity.Select(x => new TimeArticleResponseModel(x)).ToList();
				return ResponseModel<List<TimeArticleResponseModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<TimeArticleResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<TimeArticleResponseModel>>.AccessDeniedResponse();
			}
			return ResponseModel<List<TimeArticleResponseModel>>.SuccessResponse();
		}
	}
}
