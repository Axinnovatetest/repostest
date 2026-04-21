using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CustomerService.Handlers.Statistics
{
	public class GetAllProjectsByYearByMonthHandler: IHandle<Identity.Models.UserModel, ResponseModel<ProjectResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Statistics.StatRequestModel _data { get; set; }

		public GetAllProjectsByYearByMonthHandler(Identity.Models.UserModel user, Models.Statistics.StatRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<ProjectResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var nb1 = Infrastructure.Data.Access.Joins.CTS.StatisticProjectAccess.GetStats2(this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month);
				ProjectResponseModel response = new ProjectResponseModel();
				if(nb1 != 0)
					response.TotalProjects = nb1;
				return ResponseModel<ProjectResponseModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<ProjectResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<ProjectResponseModel>.AccessDeniedResponse();
			}
			return ResponseModel<ProjectResponseModel>.SuccessResponse();
		}
	}
}
