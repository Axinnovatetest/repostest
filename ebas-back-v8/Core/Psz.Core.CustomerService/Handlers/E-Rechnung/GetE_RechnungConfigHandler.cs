using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.E_Rechnung;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.E_Rechnung
{
	public class GetE_RechnungConfigHandler: IHandle<Identity.Models.UserModel, ResponseModel<ERechnungConfigModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetE_RechnungConfigHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<ERechnungConfigModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var result = new ERechnungConfigModel();

				var e_rechnungTypedCustomers = Infrastructure.Data.Access.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenAccess.Get();
				var e_rechnungUntypedCustomers = Infrastructure.Data.Access.Joins.CTS.Divers.GetKundenListForERechnung((int)Enums.E_RechnungEnums.RechnungTyp.Sonderregelung, false);
				var mailAndCronJob = Infrastructure.Data.Access.Joins.CTS.Divers.GetE_Rechnung_Config();

				result = new ERechnungConfigModel
				{
					MailAndJobModel = new E_RechnungMailAndJobModel(mailAndCronJob),
					TypedCustomers = e_rechnungTypedCustomers?.Select(x => new E_RechnungTypedCustomerModel(x)).ToList(),
					UntypedCustomers = e_rechnungUntypedCustomers?.Select(x => new E_RechnungUntypedCustomerModel
					{
						Kundenname = x.Name1,
						KundenNr = x.Nr,
						Kundennummer = x.Kundennummer,
					}).ToList()
				};

				return ResponseModel<ERechnungConfigModel>.SuccessResponse(result);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<ERechnungConfigModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<ERechnungConfigModel>.AccessDeniedResponse();
			}

			return ResponseModel<ERechnungConfigModel>.SuccessResponse();
		}
	}
}
