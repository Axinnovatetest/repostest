using Psz.Core.MaterialManagement.Orders.Helpers;
using Psz.Core.MaterialManagement.Orders.Models.Orders;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Orders
{
	public class AddOrderHandler: IHandle<AddOrderRequestModel, ResponseModel<AddOrderResponseModel>>
	{
		private AddOrderRequestModel data { get; set; }
		private UserModel user { get; set; }

		public AddOrderHandler(UserModel user, AddOrderRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<AddOrderResponseModel> Handle()
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

		private ResponseModel<AddOrderResponseModel> Perform(UserModel user, AddOrderRequestModel data)
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager(Infrastructure.Services.Utils.TransactionsManager.Database.Default);


			var adress = Infrastructure.Data.Access.Tables.MTM.AdressenAccess.Get(data.SupplierId);
			var supplier = Infrastructure.Data.Access.Tables.MTM.LieferantenAccess.GetByAddressNr(data.SupplierId);
			var condition = Infrastructure.Data.Access.Tables.MTM.KonditionszuordnungstabelleAccess.Get((supplier.Konditionszuordnungs_Nr.HasValue) ? supplier.Konditionszuordnungs_Nr.Value : -1);

			var bestellungenMax = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.GetMax(((Enums.OrderEnums.OrderTypes)this.data.OrderType).GetDescription());

			try
			{
				botransaction.beginTransaction();
				var orderData = new Infrastructure.Data.Entities.Tables.MTM.BestellungenEntity()
				{
					Anrede = adress.Anrede,
					Vorname_NameFirma = adress.Name1,
					Name2 = adress.Name2,
					Name3 = adress.Name3,
					Abteilung = adress.Abteilung,
					Strasse_Postfach = adress.Strasse,
					Land_PLZ_Ort = String.Concat(adress.PLZ_Strasse, ' ', adress.Ort),
					Versandart = supplier.Versandart,
					Zahlungsweise = supplier.Zahlungsweise,
					Konditionen = condition?.Text,
					Unser_Zeichen = adress.Lieferantennummer.HasValue ? Convert.ToString(adress.Lieferantennummer.Value) : null,
					Ihr_Zeichen = supplier.Kundennummer__Lieferanten_,
					Wahrung = supplier.Wahrung,
					Frachtfreigrenze = supplier.Frachtfreigrenze,
					Mindestbestellwert = supplier.Mindestbestellwert,
					Briefanrede = adress.Briefanrede,

					Lieferanten_Nr = data.SupplierId,
					Bestellung_Nr = bestellungenMax + 1,
					Projekt_Nr = (bestellungenMax + 1).ToString(),
					Bestellbestatigung_erbeten_bis = DateTime.Now.AddDays(+3).Date,
					Typ = ((Enums.OrderEnums.OrderTypes)this.data.OrderType).GetDescription(),// - 2023-03-30 - add Kanbanabruf option -- "Bestellung",
					Rahmenbestellung = false, // INITIAL Value ???
					Bearbeiter = this.user.Number,
					Datum = DateTime.Now.Date,
					Personal_Nr = 0,
					USt = 0,
					Rabatt = 0,
					Kundenbestellung = 0,
					best_id = 0,
					nr_anf = 0,
					nr_RB = 0,
					nr_bes = 0,
					nr_war = 0,
					nr_gut = 0,
					nr_sto = 0,
					Belegkreis = 0,
					// - 2023-03-30
					Neu = true,
					Loschen = false,
					In_Bearbeitung = false,
					Offnen = false,
					Kanban = ((Enums.OrderEnums.OrderTypes)this.data.OrderType) == Enums.OrderEnums.OrderTypes.Kanaban,
					Benutzer = $"{this.user.LegacyUserName} [{this.user.Username}] {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}"
				};

				var orderId = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.InsertWithTransaction(orderData, botransaction.connection, botransaction.transaction);
				orderData.Projekt_Nr = orderId.ToString();
				Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.UpdateWithTransaction(orderData, botransaction.connection, botransaction.transaction);

				//Logging
				var orderAfterInsert = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.GetWithTransaction(orderId, botransaction.connection, botransaction.transaction);
				var _log = new LogHelper(
					orderAfterInsert.Nr,
					orderAfterInsert.Bestellung_Nr ?? -1,
					int.TryParse(orderAfterInsert.Projekt_Nr, out var val) ? val : 0,
					orderAfterInsert.Typ,
					LogHelper.LogType.CREATIONORDER,
					"MTM",
					user)
						.LogMTM(orderAfterInsert.Nr);
				Infrastructure.Data.Access.Tables.MTM.BestellungenProcessing_LogAccess.InsertWithTransaction(_log, botransaction.connection, botransaction.transaction);

				// - 
				if(botransaction.commit())
				{
					return ResponseModel<AddOrderResponseModel>.SuccessResponse(new AddOrderResponseModel(orderId));
				}
				else
					return ResponseModel<AddOrderResponseModel>.FailureResponse("Transaction didn't commit.");

			} catch(Exception e)
			{
				botransaction.rollback();
				throw;
			}
		}

		public ResponseModel<AddOrderResponseModel> Validate()
		{
			if(user == null)
			{
				return ResponseModel<AddOrderResponseModel>.AccessDeniedResponse();
			}
			if(user.Number == 0)
				return ResponseModel<AddOrderResponseModel>.FailureResponse("User need to have a User Number");

			if(user.Access?.MaterialManagement?.Purchasing?.OrderQuickPO != true)
			{
				return ResponseModel<AddOrderResponseModel>.FailureResponse("User does not have access");
			}
			var types = new GetTypesHandler(this.user).Handle();
			if(types.Success && types.Body?.Exists(x => x.Id == this.data.OrderType) != true)
			{
				return ResponseModel<AddOrderResponseModel>.FailureResponse("Selected order not found");
			}
			var adress = Infrastructure.Data.Access.Tables.MTM.AdressenAccess.Get(data.SupplierId);
			if(adress == null)
				return ResponseModel<AddOrderResponseModel>.FailureResponse("Supplier Not found");
			var supplier = Infrastructure.Data.Access.Tables.MTM.LieferantenAccess.GetByAddressNr(data.SupplierId);
			if(supplier == null)
				return ResponseModel<AddOrderResponseModel>.FailureResponse("Supplier Not found");

			return ResponseModel<AddOrderResponseModel>.SuccessResponse();
		}
	}
}
