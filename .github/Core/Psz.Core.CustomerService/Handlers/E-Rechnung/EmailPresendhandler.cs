using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.E_Rechnung;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CustomerService.Handlers.E_Rechnung
{
	public class EmailPresendhandler: IHandle<Identity.Models.UserModel, ResponseModel<EmailPresendModel>>
	{

		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public EmailPresendhandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<EmailPresendModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var rechung = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data);
				var rechnungCustomer = Infrastructure.Data.Access.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenAccess.GetByKundennummer(rechung.Kunden_Nr ?? -1);

				var response = new EmailPresendModel
				{
					RechnungCustomerEmail = rechnungCustomer.Email,
					RechnungDocumentName = $"Rechnung_{rechung.Angebot_Nr}_{rechung.Bezug}",
				};

				return ResponseModel<EmailPresendModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<EmailPresendModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<EmailPresendModel>.AccessDeniedResponse();
			}
			var rechung = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data);
			if(rechung == null)
				return ResponseModel<EmailPresendModel>.FailureResponse("Invoice not found .");
			return ResponseModel<EmailPresendModel>.SuccessResponse();
		}

	}
}
