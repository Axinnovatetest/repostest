using Psz.Core.FinanceControl.Models.Budget.Order.Statistics;
using Psz.Core.FinanceControl.Models.Budget.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class GetFastestFirtValidationDiffToLastBookingTimeHandler: IHandle<Identity.Models.UserModel, ResponseModel<FastestResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		//private int _data { get; set; }
		public GetFastestFirtValidationDiffToLastBookingTimeHandler(Identity.Models.UserModel user)
		{
			this._user = user;

		}


		public ResponseModel<FastestResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var filteredOrders = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetTopFiveFastesFilteredOrders();


				var timeSpanOrders = filteredOrders.Select(x => new OrderDiffModel
				{
					OrderId = x.OrderId,

					OrderNum = x.OrderNumber,

					ValidationRequestTime = x.ValidationRequestTime,

					ValidationTime = x.ValidationTime,


					Date_Time = x.TimeDifference

				}).ToList();


				var response = new FastestResponseModel
				{
					TopFiveFastest = timeSpanOrders
				};

				return ResponseModel<FastestResponseModel>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<FastestResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<FastestResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<FastestResponseModel>.SuccessResponse();
		}
	}
}
