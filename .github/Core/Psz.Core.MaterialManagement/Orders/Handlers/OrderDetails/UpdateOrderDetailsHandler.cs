using Psz.Core.MaterialManagement.Orders.Helpers;
using Psz.Core.MaterialManagement.Orders.Models.OrderDetails;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.Orders.Handlers.OrderDetails
{
	public class UpdateOrderDetailsHandler: IHandle<UpdateOrderDetailsRequestModel, ResponseModel<UpdateOrderDetailsResponseModel>>
	{

		private UpdateOrderDetailsRequestModel data { get; set; }
		private UserModel user { get; set; }

		public UpdateOrderDetailsHandler(UserModel user, UpdateOrderDetailsRequestModel data)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<UpdateOrderDetailsResponseModel> Handle()
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

		private ResponseModel<UpdateOrderDetailsResponseModel> Perform(UserModel user, UpdateOrderDetailsRequestModel data)
		{
			var orderDb = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(data.id);

			orderDb.Konditionen = data.conditions;
			orderDb.Bestellbestatigung_erbeten_bis = data.confirmation_requested_Date;
			orderDb.Bearbeiter = data.editor;
			orderDb.Freitext = data.free_text;
			orderDb.Eingangslieferscheinnr = data.incoming_delivery_notes;
			orderDb.Eingangsrechnungsnr = data.incoming_invoice_no;
			orderDb.Zahlungsweise = data.payment_method;
			orderDb.Bezug = data.Relation;
			orderDb.Versandart = data.shipping_method;

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager(Infrastructure.Services.Utils.TransactionsManager.Database.Default);
			botransaction.beginTransaction();
			try
			{
				Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.UpdateWithTransaction(orderDb, botransaction.connection, botransaction.transaction);

				//Logging
				var _log = new LogHelper(
					orderDb.Nr,
					orderDb.Bestellung_Nr ?? -1,
					int.TryParse(orderDb.Projekt_Nr, out var val) ? val : 0,
					 $"{orderDb.Typ}",
					LogHelper.LogType.MODIFICATIONORDER,
					"MTM",
					user: user)
					.LogMTM(orderDb.Nr);
				Infrastructure.Data.Access.Tables.MTM.BestellungenProcessing_LogAccess.InsertWithTransaction(_log, botransaction.connection, botransaction.transaction);
				// - 
				if(botransaction.commit())
				{
					return ResponseModel<UpdateOrderDetailsResponseModel>.SuccessResponse();
				}
				else
					return ResponseModel<UpdateOrderDetailsResponseModel>.FailureResponse("Transaction didn't commit.");
			} catch(Exception e)
			{
				botransaction.rollback();
				throw;
			}

		}

		public ResponseModel<UpdateOrderDetailsResponseModel> Validate()
		{
			if(user == null)
			{
				return ResponseModel<UpdateOrderDetailsResponseModel>.AccessDeniedResponse();
			}

			if(user.Number == 0)
				return ResponseModel<UpdateOrderDetailsResponseModel>.FailureResponse("User need to have a User Number");

			var orderDb = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(data.id);

			if(orderDb == null)
				return ResponseModel<UpdateOrderDetailsResponseModel>.FailureResponse("Order not found");

			if(orderDb.gebucht == true)
				return ResponseModel<UpdateOrderDetailsResponseModel>.FailureResponse("Can't edit Validated Order");

			return ResponseModel<UpdateOrderDetailsResponseModel>.SuccessResponse();
		}
	}
}
