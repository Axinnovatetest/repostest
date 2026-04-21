using Psz.Core.MaterialManagement.Orders.Models.OrderValidation;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.Orders.Handlers.OrderValidation
{
	public class OrderinformationHandler: IHandle<PlaceOrderInformationRequestModel, ResponseModel<PlaceOrderInformationResponseModel>>
	{
		private PlaceOrderInformationRequestModel data { get; set; }
		private UserModel user { get; set; }

		public OrderinformationHandler(UserModel user, PlaceOrderInformationRequestModel data)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<PlaceOrderInformationResponseModel> Handle()
		{

			try
			{
				var validation = Validate();
				if(!validation.Success)
				{
					return validation;
				}

				return Perform(this.user, this.data);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		private ResponseModel<PlaceOrderInformationResponseModel> Perform(UserModel user, PlaceOrderInformationRequestModel data)
		{
			try
			{
				var bestellung = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(data.OrderNumber);
				var adressen = Infrastructure.Data.Access.Tables.MTM.AdressenAccess.Get(bestellung.Lieferanten_Nr.Value);

				return ResponseModel<PlaceOrderInformationResponseModel>.SuccessResponse(new PlaceOrderInformationResponseModel() { SupplierEmail = adressen.eMail });

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<PlaceOrderInformationResponseModel> Validate()
		{
			if(user == null)
			{
				return ResponseModel<PlaceOrderInformationResponseModel>.AccessDeniedResponse();
			}
			if(user.Number == 0)
				return ResponseModel<PlaceOrderInformationResponseModel>.FailureResponse("User need to have a User Number");

			var bestellung = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(data.OrderNumber);

			if(bestellung == null)
				return ResponseModel<PlaceOrderInformationResponseModel>.FailureResponse("Order not found");

			var orderItems = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.GetByOrderId(bestellung.Nr);
			if(orderItems == null || orderItems.Count == 0)
			{
				return ResponseModel<PlaceOrderInformationResponseModel>.FailureResponse("No Position found");
			}
			foreach(var orderITem in orderItems)
			{
				if(orderITem.Bestatigter_Termin is null || orderITem.Liefertermin is null)
				{
					return ResponseModel<PlaceOrderInformationResponseModel>.FailureResponse("All Positions should have an Delivery Date and a Confirmed Delivery Date ");
				}
			}
			var Lieferanten = Infrastructure.Data.Access.Tables.MTM.LieferantenAccess.GetByAddressNr(bestellung.Lieferanten_Nr.HasValue ? bestellung.Lieferanten_Nr.Value : 0);
			if(Lieferanten is null)
			{
				return ResponseModel<PlaceOrderInformationResponseModel>.FailureResponse("No Supplier found");
			}

			return ResponseModel<PlaceOrderInformationResponseModel>.SuccessResponse();
		}
	}
}
