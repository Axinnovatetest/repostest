using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.CsStatistics.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CustomerService.CsStatistics.Handlers
{
	public class GetVersandBerechnetHandler: IHandle<Identity.Models.UserModel, ResponseModel<VersandBerechnetResponseModel>>
	{
		private IDateRangeModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetVersandBerechnetHandler(Identity.Models.UserModel user, IDateRangeModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<VersandBerechnetResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new VersandBerechnetResponseModel();
				var versandBerechnetEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetVersandBerechnet(_data.From, _data.To);
				if(versandBerechnetEntity != null && versandBerechnetEntity.Count > 0)
					response = new VersandBerechnetResponseModel
					{
						Artikelnummer = versandBerechnetEntity[0].Item2,
						Typ = versandBerechnetEntity[0].Item1,
						SummevonGesamtpreis = versandBerechnetEntity[0].Item3
					};
				return ResponseModel<VersandBerechnetResponseModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
				throw;
			}
		}
		public ResponseModel<VersandBerechnetResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<VersandBerechnetResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<VersandBerechnetResponseModel>.SuccessResponse();
		}
	}
}
