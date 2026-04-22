using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;
using static Psz.Core.CustomerService.Models.Statistics.FAResponseModel;


namespace Psz.Core.CustomerService.Handlers.Statistics
{
	public class GetFAByYearByMonthHandler: IHandle<Identity.Models.UserModel, ResponseModel<FAResponseModel>>

	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Statistics.StatRequestModel _data { get; set; }

		public GetFAByYearByMonthHandler(Identity.Models.UserModel user, Models.Statistics.StatRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<FAResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var nb3 = Infrastructure.Data.Access.Joins.CTS.StatisticFAAccess.GetFA1(this._data.AngeboteNr, this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month);
				var statEntityFALager = Infrastructure.Data.Access.Joins.CTS.StatisticFAAccess.GetFA2(this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month);
				var statEntityFALZ = Infrastructure.Data.Access.Joins.CTS.StatisticFAAccess.GetFA3(this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month);
				var statEntityFAKenz = Infrastructure.Data.Access.Joins.CTS.StatisticFAAccess.GetFA4(this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month);
				var statEntityFAGetstartet = Infrastructure.Data.Access.Joins.CTS.StatisticFAAccess.GetFA5(this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month);
				var statEntityFAKommComp = Infrastructure.Data.Access.Joins.CTS.StatisticFAAccess.GetFA6(this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month);
				var statEntityFAKommTrei = Infrastructure.Data.Access.Joins.CTS.StatisticFAAccess.GetFA7(this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month);
				var statEntityFATech = Infrastructure.Data.Access.Joins.CTS.StatisticFAAccess.GetFA8(this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month);
				FAResponseModel response = new FAResponseModel();
				response.TotFA = nb3;
				if(statEntityFALager != null && statEntityFALager.Count > 0)
					response.AllFAByLager = statEntityFALager.Select(x => new Item0(x)).ToList();
				if(statEntityFALZ != null && statEntityFALZ.Count > 0)
					response.AllFAByLagerZbuchen = statEntityFALZ.Select(x => new Item1(x)).ToList();
				if(statEntityFAKenz != null && statEntityFAKenz.Count > 0)
					response.ByKennzeichen = statEntityFAKenz.Select(x => new Item2(x)).ToList();
				if(statEntityFAGetstartet != null && statEntityFAGetstartet.Count > 0)
					response.FAGestartet = statEntityFAGetstartet.Select(x => new Item3(x)).ToList();
				if(statEntityFAKommComp != null && statEntityFAKommComp.Count > 0)
					response.FAKomC = statEntityFAKommComp.Select(x => new Item4(x)).ToList();
				if(statEntityFAKommTrei != null && statEntityFAKommTrei.Count > 0)
					response.FAKomT = statEntityFAKommTrei.Select(x => new Item5(x)).ToList();
				if(statEntityFATech != null && statEntityFATech.Count > 0)
					response.FATechn = statEntityFATech.Select(x => new Item6(x)).ToList();
				return ResponseModel<FAResponseModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<FAResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<FAResponseModel>.AccessDeniedResponse();
			}
			return ResponseModel<FAResponseModel>.SuccessResponse();
		}
	}
}
