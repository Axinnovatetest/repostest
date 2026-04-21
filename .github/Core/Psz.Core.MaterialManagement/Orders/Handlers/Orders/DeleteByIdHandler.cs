using Psz.Core.MaterialManagement.Orders.Helpers;
using Psz.Core.MaterialManagement.Orders.Models.Orders;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Orders
{
	public class DeleteByIdHandler: IHandle<DeleteByIdRequestModel, ResponseModel<DeleteByIdResponseModel>>
	{
		private DeleteByIdRequestModel data { get; set; }
		private UserModel user { get; set; }

		public DeleteByIdHandler(UserModel user, DeleteByIdRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<DeleteByIdResponseModel> Handle()
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

		private ResponseModel<DeleteByIdResponseModel> Perform(UserModel user, DeleteByIdRequestModel data)
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager(Infrastructure.Services.Utils.TransactionsManager.Database.Default);
			var bestellung = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(data.OrderId);

			try
			{
				botransaction.beginTransaction();
				var orderAfterInsert = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.GetWithTransaction(data.OrderId, botransaction.connection, botransaction.transaction);

				var item = new Infrastructure.Data.Entities.Tables.MTM.Orders.PSZ_Protokolierung_Geloschte_Bestellungen2Entity()
				{
					Bestellung_Nr = bestellung.Bestellung_Nr,
					Bestellung_Typ = bestellung.Typ,
					Geloscht_AM = DateTime.Now,
					Geloscht_durch = this.user.Name,
					Lieferanten_nr = bestellung.Lieferanten_Nr,
					Name = bestellung.Vorname_NameFirma,
					Projekt_Nr = bestellung.Projekt_Nr
				};
				var insertedProtokole = Infrastructure.Data.Access.Tables.MTM.Orders.PSZ_Protokolierung_Geloschte_Bestellungen2Access.InsertWithTransaction(item, botransaction.connection, botransaction.transaction);
				var _log = new LogHelper(
					orderAfterInsert.Nr,
					orderAfterInsert.Bestellung_Nr ?? -1,
					int.TryParse(orderAfterInsert.Projekt_Nr, out var val) ? val : 0,
					orderAfterInsert.Typ,
					LogHelper.LogType.DELETIONORDER,
					"MTM",
					user)
						.LogMTM(orderAfterInsert.Nr);

				int deleted = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.DeleteWithTransaction(data.OrderId, botransaction.connection, botransaction.transaction);

				Infrastructure.Data.Access.Tables.MTM.BestellungenProcessing_LogAccess.InsertWithTransaction(_log, botransaction.connection, botransaction.transaction);

				if(botransaction.commit())
				{
					return ResponseModel<DeleteByIdResponseModel>.SuccessResponse(new DeleteByIdResponseModel(deleted > 0));
				}
				else
					return ResponseModel<DeleteByIdResponseModel>.FailureResponse("Transaction didn't commit.");

			} catch(Exception)
			{
				botransaction.rollback();
				throw;
			}
		}

		public ResponseModel<DeleteByIdResponseModel> Validate()
		{
			if(user == null)
			{
				return ResponseModel<DeleteByIdResponseModel>.AccessDeniedResponse();
			}
			if(user.Number == 0)
				return ResponseModel<DeleteByIdResponseModel>.FailureResponse("User need to have a User Number");
			var bestellung = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(data.OrderId);
			if(bestellung == null)
			{
				return ResponseModel<DeleteByIdResponseModel>.FailureResponse("Order Doesn't exist.");
			}
			if(bestellung.gebucht == true)
			{
				return ResponseModel<DeleteByIdResponseModel>.FailureResponse("Can't delete confirmed Orders.");
			}
			if(bestellung.ProjectPurchase == true && user.Access.MaterialManagement.Purchasing.ProjectPurchaseDeleteOrder != true)
			{
				return ResponseModel<DeleteByIdResponseModel>.FailureResponse("User can't delete [Projektbestellung] Orders.");
			}
			var wareneingang = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.GetByWareneingangByNr(bestellung.Nr);
			if(wareneingang != null)
			{
				return ResponseModel<DeleteByIdResponseModel>.FailureResponse("Can't delete Received Orders.");
			}
			return ResponseModel<DeleteByIdResponseModel>.SuccessResponse();
		}
	}
}
