using Psz.Core.MaterialManagement.Orders.Helpers;
using Psz.Core.MaterialManagement.Orders.Models.OrderValidation;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.OrderValidation
{
	public class UnValidateOrderHandler: IHandle<UnValidateRequestModel, ResponseModel<UnValidateResponseModel>>
	{
		private UnValidateRequestModel data { get; set; }
		private UserModel user { get; set; }

		public UnValidateOrderHandler(UserModel user, UnValidateRequestModel data)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<UnValidateResponseModel> Handle()
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

		private ResponseModel<UnValidateResponseModel> Perform(UserModel user, UnValidateRequestModel data)
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager(Infrastructure.Services.Utils.TransactionsManager.Database.Default);
			try
			{
				botransaction.beginTransaction();
				var bestellung = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.GetWithTransaction(data.OrderNumber, botransaction.connection, botransaction.transaction);
				var orderItems = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.GetByOrderId(bestellung.Nr, botransaction.connection, botransaction.transaction);

				// - 2025-08-27 - update RA qty on BE validate - Khelil
				// - send back reseved qty in RA
				var orderItemsWRa = orderItems?.Where(x => x.RA_Pos_zu_Bestellposition.HasValue && x.RA_Pos_zu_Bestellposition.Value > 0);
				if(orderItemsWRa?.Count() > 0)
				{
					var raPositions = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(orderItemsWRa.Select(x => x.RA_Pos_zu_Bestellposition.Value).ToList());
					var positionToUpdate = new List<KeyValuePair<int, decimal>>();
					foreach(var item in raPositions)
					{
						var totalNeededQty = orderItemsWRa.Where(x => x.RA_Pos_zu_Bestellposition == item.Nr)?.Sum(x => x.Anzahl ?? 0) ?? 0;
						positionToUpdate.Add(new KeyValuePair<int, decimal>(item.Nr, (item.Anzahl ?? 0) + totalNeededQty));
					}
					Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateQuantities(positionToUpdate, botransaction.connection, botransaction.transaction);
				}
				bestellung.gebucht = false;
				bestellung.erledigt = false;

				Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.UpdateWithTransaction(bestellung, botransaction.connection, botransaction.transaction);
				var _log = new LogHelper(
					bestellung.Nr,
					bestellung.Bestellung_Nr ?? -1,
					int.TryParse(bestellung.Projekt_Nr, out var val) ? val : 0,
					bestellung.Typ,
					LogHelper.LogType.UNVALIDATEORDER,
					"MTM",
					user).LogMTM(bestellung.Nr);
				Infrastructure.Data.Access.Tables.MTM.BestellungenProcessing_LogAccess.InsertWithTransaction(_log, botransaction.connection, botransaction.transaction);

				if(botransaction.commit())
				{
					return ResponseModel<UnValidateResponseModel>.SuccessResponse();
				}
				else
					return ResponseModel<UnValidateResponseModel>.FailureResponse("Transaction didn't commit.");
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<UnValidateResponseModel> Validate()
		{
			if(user == null)
			{
				return ResponseModel<UnValidateResponseModel>.AccessDeniedResponse();
			}
			if(user.Number == 0)
				return ResponseModel<UnValidateResponseModel>.FailureResponse("User need to have a User Number");

			var bestellung = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(data.OrderNumber);
			if(bestellung == null)
				return ResponseModel<UnValidateResponseModel>.NotFoundResponse();
			return ResponseModel<UnValidateResponseModel>.SuccessResponse();
		}
	}
}
