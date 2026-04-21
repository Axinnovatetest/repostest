using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.OrderProcessing;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CustomerService.Handlers.OrderProcessing
{
	public class GetABNewCustomerHandler: IHandle<Identity.Models.UserModel, ResponseModel<OrderCustomerModel>>
	{

		private GeNewCustomerEntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetABNewCustomerHandler(Identity.Models.UserModel user, GeNewCustomerEntryModel data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<OrderCustomerModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var Order = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data.Id);
				var customerDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(_data.CustomerId);
				var customerNummer = customerDb.Nummer;
				var lieferadressDb = new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity();

				if(customerDb.LSADR.HasValue && customerDb.LSADR.Value > 0)
					lieferadressDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get((int)customerDb.LSADR);
				else
					lieferadressDb = customerDb.Nummer.HasValue
				? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customerDb.Nummer.Value)
				: null;

				var adressDb = customerDb.Nummer.HasValue
					? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customerDb.Nummer.Value)
					: null;
				var mailBoxIsPreferred = adressDb?.Postfach_bevorzugt == true;
				var conditionAssignementTableDb = customerDb.Konditionszuordnungs_Nr.HasValue
						? Infrastructure.Data.Access.Tables.PRS.KonditionsZuordnungstabelleEntity.Get(customerDb.Konditionszuordnungs_Nr.Value)
						: null;
				var response = new OrderCustomerModel
				{
					Id = _data.Id,
					CustomerId = _data.CustomerId,
					CustomerNumber = adressDb.Nr,
					AdressCustomerNumber = adressDb?.Kundennummer ?? 0,
					Contact = adressDb.Abteilung,
					CountryPostcode = mailBoxIsPreferred
									? $"{adressDb.PLZ_Postfach} {adressDb.Ort}"
									: $"{adressDb.PLZ_StraBe} {adressDb.Ort} ",
					StreetPOBox = $"{lieferadressDb?.StraBe}",
					Name = adressDb.Name1,
					Name2 = adressDb.Name2,
					Name3 = adressDb.Name3,
				};

				return ResponseModel<OrderCustomerModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<OrderCustomerModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<OrderCustomerModel>.AccessDeniedResponse();
			}
			var Order = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data.Id);
			if(Order == null)
				return ResponseModel<OrderCustomerModel>.FailureResponse("Order not found .");
			var ABdeliveries = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetByTypAndABid("Lieferschein", _data.Id);
			if(ABdeliveries != null && ABdeliveries.Count > 0)
				return ResponseModel<OrderCustomerModel>.FailureResponse("Order have deliveries, customer change impossible .");
			return ResponseModel<OrderCustomerModel>.SuccessResponse();
		}
	}
}
