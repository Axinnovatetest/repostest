using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Statistics
{
	public class GetArticleFAByYearBYMonthHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<ArticleFAResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Statistics.StatRequestModel _data { get; set; }

		public GetArticleFAByYearBYMonthHandler(Identity.Models.UserModel user, Models.Statistics.StatRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<ArticleFAResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var statEntityARFAEntity = Infrastructure.Data.Access.Joins.CTS.StatisticFAAccess.GetFA9(this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month);
				List<ArticleFAResponseModel> response = new List<ArticleFAResponseModel>();
				if(statEntityARFAEntity != null && statEntityARFAEntity.Count > 0)
					response = statEntityARFAEntity.Select(x => new ArticleFAResponseModel(x)).ToList();
				return ResponseModel<List<ArticleFAResponseModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<ArticleFAResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<ArticleFAResponseModel>>.AccessDeniedResponse();
			}
			return ResponseModel<List<ArticleFAResponseModel>>.SuccessResponse();
		}
	}
}
