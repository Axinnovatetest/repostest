using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.HourlyRate
{
	public class GetByProductionSiteHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Settings.HourlyRate.HourlyRateResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetByProductionSiteHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<Models.Settings.HourlyRate.HourlyRateResponseModel> Handle()
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //
				// -
				var entities = Infrastructure.Data.Access.Tables.BSD.HourlyRateAccess.GetByProdutionSiteId(this._data, botransaction.connection, botransaction.transaction);

				#endregion // -- transaction-based logic -- //

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<Models.Settings.HourlyRate.HourlyRateResponseModel>.SuccessResponse(new Models.Settings.HourlyRate.HourlyRateResponseModel(entities?.Count > 0 ? entities[0] : null));
				}
				else
				{
					return ResponseModel<Models.Settings.HourlyRate.HourlyRateResponseModel>.FailureResponse("Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<Models.Settings.HourlyRate.HourlyRateResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Settings.HourlyRate.HourlyRateResponseModel>.AccessDeniedResponse();
			}

			// -
			return ResponseModel<Models.Settings.HourlyRate.HourlyRateResponseModel>.SuccessResponse();
		}
	}
}
