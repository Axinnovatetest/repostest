using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.OrderProcessing;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CustomerService.Handlers.OrderProcessing
{
	public class GetOrderHandler: IHandle<Identity.Models.UserModel, ResponseModel<OrderModel>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetOrderHandler(int data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<OrderModel> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			try
			{
				var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data);
				var customerDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer((int)orderDb.Kunden_Nr);
				var adressDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get((int)orderDb.Kunden_Nr);
				var orderExtensionDb = Infrastructure.Data.Access.Tables.PRS.OrderExtensionAccess.GetByOrderId(_data);

				var order = new OrderModel()
				{
					Id = orderDb.Nr,
					CustomerId = customerDb?.Nr,
					CustomerNumber = orderDb.Kunden_Nr, // customerDb?.Nummer
					AdressCustomerNumber = adressDb?.Kundennummer, // adressDb.Kunden_Nr,

					AbId = (orderDb.Ab_id == orderDb.Nr) ? -1 : orderDb.Ab_id,
					Belegkreis = orderDb.Belegkreis,
					Conditions = orderDb.Konditionen,
					Contact = orderDb.Ansprechpartner,
					CountryPostcode = orderDb.Land_PLZ_Ort,
					Department = orderDb.Abteilung,
					ProjectNumber = orderDb.Projekt_Nr,
					Done = orderDb.Erledigt,
					Booked = orderDb.Gebucht,


					Date = orderDb.Datum,
					DesiredDate = orderDb.Wunschtermin,
					DueDate = orderDb.Falligkeit,
					DeliveryDate = orderDb.Liefertermin,
					ShippingDate = orderDb.Versanddatum_Auswahl,

					Freetext = orderDb.Freitext,
					Freetext2 = orderDb.Freie_Text,
					Name = orderDb.Vorname_NameFirma,
					Name2 = orderDb.Name2,
					Name3 = orderDb.Name3,
					New = orderDb.Neu,
					NewOrder = orderDb.Neu_Order ?? false,
					NrAuf = orderDb.Nr_auf,
					NrBv = orderDb.Nr_BV,
					NrGut = orderDb.Nr_gut,
					NrKanban = orderDb.Nr_Kanban,
					NrLie = orderDb.Nr_lie,
					NrPro = orderDb.Nr_pro,
					NrRa = orderDb.Nr_RA,
					NrRec = orderDb.Nr_rec,
					NrSto = orderDb.Nr_sto,
					OrderTitle = orderDb.LBriefanrede,
					Payment = orderDb.Zahlungsweise,
					PersonalNumber = orderDb.Personal_Nr,
					RepairNumber = orderDb.Reparatur_nr,
					Shipping = orderDb.Versandart,
					ShippingAddress = orderDb.Lieferadresse,
					StreetPOBox = orderDb.Straße_Postfach,
					SuppliedNumber = orderDb.Ihr_Zeichen,
					Type = orderDb.Typ,
					Vat = orderDb.USt_Berechnen ?? false,

					LClientName = orderDb.LVorname_NameFirma,
					LContact = orderDb.LAnsprechpartner,
					LCountryPostcode = orderDb.LLand_PLZ_Ort,
					LCountryZIPLocation = orderDb.LLand_PLZ_Ort, // << same as above ??
					LDepartment = orderDb.LAbteilung,
					LName = orderDb.LVorname_NameFirma,
					LName2 = orderDb.LName2,
					LName3 = orderDb.LName3,
					LOrderTitle = orderDb.LBriefanrede,
					LStreetMailbox = orderDb.LStraße_Postfach,
					LStreetPOBox = orderDb.LStraße_Postfach,
					LType = orderDb.LAnrede,

					Version = (orderExtensionDb?.Version ?? 0),
					//ValidationTime = orderExtensionDb?.ValidationTime,
					//ValidationUserId = orderExtensionDb?.ValidationUserId,
					//ValidationUser = orderExtensionDb != null
					//    ? validationUsersDb.Find(e => e.Id == orderExtensionDb.ValidationUserId)?.Name
					//    : null,

					VorfailNr = orderDb.Angebot_Nr.HasValue ? orderDb.Angebot_Nr.ToString() : string.Empty,

					DocumentNumber = orderDb.Bezug,
					IsManualCreation = !orderDb.Neu_Order.HasValue,

					CanArchive = (!orderDb.Angebot_Nr.HasValue || orderDb.Angebot_Nr.HasValue && orderDb.Angebot_Nr.Value <= 0)
							? true
							: Helpers.SpecialHelper.CanArchiveOrderByAngebote(orderDb.Angebot_Nr),
					CanDelete = (string.IsNullOrEmpty(orderDb.Projekt_Nr) || string.IsNullOrWhiteSpace(orderDb.Projekt_Nr))
							? true
							: Helpers.SpecialHelper.CanDeleteOrder(orderDb.Nr, orderDb.Angebot_Nr ?? -1),

					// > Consignee and Buyer
					//Buyer = new Models.Order.OrderModel.BuyerModel(orderExtBuyerDb),
					//Consignee = new Models.Order.ConsigneeModel(orderExtConsigneeDb),

					Changes = new OrderModel.ChangesModel()
				};
				return ResponseModel<OrderModel>.SuccessResponse(order);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{_data}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<OrderModel> Validate()
		{
			if(_user == null || (!_user.Access.CustomerService.ModuleActivated && !_user.Access.Purchase.ModuleActivated))
			{
				return ResponseModel<OrderModel>.AccessDeniedResponse();
			}
			var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data);
			if(orderDb == null)
				return ResponseModel<OrderModel>.FailureResponse(key: "1", value: $"Order not found");

			return ResponseModel<OrderModel>.SuccessResponse();
		}
	}
}
