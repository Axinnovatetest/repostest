using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.E_Rechnung;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CustomerService.Handlers.E_Rechnung
{
	public class AddTypedCustomerHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{

		private E_RechnungUntypedCustomerAddModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public AddTypedCustomerHandler(Identity.Models.UserModel user, E_RechnungUntypedCustomerAddModel data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = Infrastructure.Data.Access.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenAccess.Insert(new Infrastructure.Data.Entities.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenEntity
				{
					Kundennummer = _data.KundenNr,
					Kundenname = _data.Kundenname,
					Typ = ((Enums.E_RechnungEnums.RechnungTyp)_data.Type).GetDescription(),
					Rechnung_Name = _data.Rechnung_Name,
				});



				return ResponseModel<int>.SuccessResponse();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var kunde = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(_data.KundenNr ?? -1);
			if(kunde is null)
				return ResponseModel<int>.FailureResponse($"Customer {_data.Kundenname} not found");

			var adresse = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(_data.KundenNr ?? -1);
			if(adresse is null || adresse.Name1?.Trim()?.ToLower() != _data.Kundenname?.Trim()?.ToLower())
				return ResponseModel<int>.FailureResponse($"Customer {_data.Kundenname} not found");

			if(Infrastructure.Data.Access.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenAccess.GetByCustomerIdName(_data.KundenNr ?? -1, _data.Kundenname) is not null)
				return ResponseModel<int>.FailureResponse($"Customer {_data.Kundenname} already assigned");

			// -
			return ResponseModel<int>.SuccessResponse();
		}

	}
}
