using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using static Psz.Core.CustomerService.Enums.StatEnum;

namespace Psz.Core.CustomerService.Handlers.Statistics
{
	public class GetABBYearByMonthHandler: IHandle<Identity.Models.UserModel, ResponseModel<ABResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Statistics.StatRequestModel _data { get; set; }

		public GetABBYearByMonthHandler(Identity.Models.UserModel user, Models.Statistics.StatRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<ABResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var nb1 = Infrastructure.Data.Access.Joins.CTS.StatisticProjectAccess.GetStats7(this._data.Typ, this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month);
				var nb2 = Infrastructure.Data.Access.Joins.CTS.StatisticProjectAccess.GetStatsZahlungsweise(this._data.Typ, this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month, StatPayment.Rechnung.GetDescription());
				var nb3 = Infrastructure.Data.Access.Joins.CTS.StatisticProjectAccess.GetStatsZahlungsweise(this._data.Typ, this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month, StatPayment.Vorkasse.GetDescription());
				var nb4 = Infrastructure.Data.Access.Joins.CTS.StatisticProjectAccess.GetStatsZahlungsweise(this._data.Typ, this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month, StatPayment.Vorauskasse.GetDescription());
				var nb5 = Infrastructure.Data.Access.Joins.CTS.StatisticProjectAccess.GetStatsZahlungsweise(this._data.Typ, this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month, StatPayment.Gutschriftverfahren.GetDescription());
				var nb6 = Infrastructure.Data.Access.Joins.CTS.StatisticProjectAccess.GetStatsZahlungsweise(this._data.Typ, this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month, StatPayment.Lastschrift.GetDescription());
				var nb7 = Infrastructure.Data.Access.Joins.CTS.StatisticProjectAccess.GetStatsZahlungsweise(this._data.Typ, this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month, StatPayment.Überweisungdirekt.GetDescription());
				var nb8 = Infrastructure.Data.Access.Joins.CTS.StatisticProjectAccess.GetStatsEdi(this._data.Typ, this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month, (int)StatEdi.Edi);
				var nb9 = Infrastructure.Data.Access.Joins.CTS.StatisticProjectAccess.GetStatsEdi(this._data.Typ, this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month, (int)StatEdi.NotEdi);
				ABResponseModel response = new ABResponseModel();
				if(nb1 != 0)
					response.TotalAB = nb1;
				if(nb2 != 0)
					response.ZahlungsweiseRechnung = nb2;
				if(nb3 != 0)
					response.ZahlungsweiseVorkasse = nb3;
				if(nb4 != 0)
					response.ZahlungsweiseVorauskasse = nb4;
				if(nb5 != 0)
					response.ZahlungsweiseGutschrift = nb5;
				if(nb6 != 0)
					response.ZahlungsweiseLastschrift = nb6;
				if(nb7 != 0)
					response.ZahlungsweiseUberweisung = nb7;
				if(nb8 != 0)
					response.Edi = nb8;
				if(nb9 != 0)
					response.NotEdi = nb9;


				return ResponseModel<ABResponseModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<ABResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<ABResponseModel>.AccessDeniedResponse();
			}

			if(string.IsNullOrWhiteSpace(this._data.Typ))
				return ResponseModel<ABResponseModel>.FailureResponse("Data cannot be empty");

			// -
			return ResponseModel<ABResponseModel>.SuccessResponse();
		}
	}
}
