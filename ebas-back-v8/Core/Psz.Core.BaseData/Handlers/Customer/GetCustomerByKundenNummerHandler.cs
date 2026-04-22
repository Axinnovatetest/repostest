using System;
using Infrastructure.Data.Access.Tables.PRS;
using Psz.Core.BaseData.Models.Customer;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.BaseData.Handlers.Customer
{
	public class GetCustomerByKundenNummerHandler: IHandle<Identity.Models.UserModel,
		ResponseModel<CustomerCommunicationModel>>
	{
		int _kundenNummer;
		UserModel _user;
		public GetCustomerByKundenNummerHandler(Identity.Models.UserModel user, int kundenNummer)
		{
			_user = user;
			_kundenNummer = kundenNummer;
		}
		public ResponseModel<CustomerCommunicationModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				if(_kundenNummer != 0)
				{
					var customerByKundenNummer = AdressenAccess.GetByKundenExt(_kundenNummer);

					if(customerByKundenNummer is not null)
					{
						var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(customerByKundenNummer.Nr);

						return ResponseModel<CustomerCommunicationModel>.SuccessResponse(new CustomerCommunicationModel(customerByKundenNummer, kundenEntity));
					}
				}
				return ResponseModel<CustomerCommunicationModel>.FailureResponse("Customer does not have contact address ");

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<CustomerCommunicationModel> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<CustomerCommunicationModel>.AccessDeniedResponse();
			}

			return ResponseModel<CustomerCommunicationModel>.SuccessResponse();
		}
	}
}
