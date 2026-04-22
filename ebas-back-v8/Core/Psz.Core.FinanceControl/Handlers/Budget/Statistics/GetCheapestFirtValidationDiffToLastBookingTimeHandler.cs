using Psz.Core.FinanceControl.Models.Budget.Order.Statistics;
using Psz.Core.FinanceControl.Models.Budget.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class GetCheapestFirtValidationDiffToLastBookingTimeHandler: IHandle<Identity.Models.UserModel, ResponseModel<CheapestResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		//private int _data { get; set; }
		public GetCheapestFirtValidationDiffToLastBookingTimeHandler(Identity.Models.UserModel user)
		{
			this._user = user;

		}


		public string ConvertToYearsMonthsDaysHoursMinutesSeconds(TimeSpan timeSpan)
		{
			int days = (int)timeSpan.TotalDays;
			int years = days / 365;
			int remainingDays = days % 365;

			int months = remainingDays / 30; // Approximation, 30 days per month
			int remainingDaysInMonth = remainingDays % 30;

			int hours = timeSpan.Hours;
			int minutes = timeSpan.Minutes;
			int seconds = timeSpan.Seconds;

			return $"{years} années {months} mois {remainingDaysInMonth} jours {hours} heures {minutes} minutes {seconds} secondes";
		}
		public ResponseModel<CheapestResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var filteredOrders = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetTopFiveCheapestFilteredOrders();

				//take diff
				var timeSpanOrders = filteredOrders.Select(x => new OrderDiffModel
				{
					OrderId = x.OrderId,

					OrderNum = x.OrderNumber,

					ValidationRequestTime = x.ValidationRequestTime,

					ValidationTime = x.ValidationTime,

					Date_Time = x.TimeDifference

				}).ToList();


				var response = new CheapestResponseModel
				{
					TopFiveCheapest = timeSpanOrders,
				};


				return ResponseModel<CheapestResponseModel>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<CheapestResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<CheapestResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<CheapestResponseModel>.SuccessResponse();
		}
	}
}
