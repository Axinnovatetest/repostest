using System;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class InvoiceBookingHandler: IHandle<UserModel, ResponseModel<string>>
	{
		private UserModel _user { get; set; }
		private int _data { get; set; }
		private string _languageCode { get; set; }
		public InvoiceBookingHandler(int invoiceNr, string langCode, UserModel user)
		{
			_user = user;
			_data = invoiceNr;
			_languageCode = langCode;
		}
		public ResponseModel<string> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				//
				var invoiceItem = Infrastructure.Data.Access.Tables.FNC.InvoiceAccess.GetByBookingId(this._data);
				return ResponseModel<string>.SuccessResponse(invoiceItem == null ? "" : InvoiceHandler.generateInvoiceReport(invoiceItem.Id, null));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<string> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<string>.AccessDeniedResponse();
			}

			var bookingItems = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(this._data, Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Types.Booking);
			if(bookingItems == null)
				return ResponseModel<string>.FailureResponse(key: "1", value: "No booking found");

			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<string>.FailureResponse(key: "1", value: "User not found");


			return ResponseModel<string>.SuccessResponse();
		}
	}
}
