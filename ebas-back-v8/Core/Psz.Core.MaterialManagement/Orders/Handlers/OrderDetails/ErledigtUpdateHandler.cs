using Infrastructure.Data.Entities.Tables.MTM;
using Psz.Core.MaterialManagement.Orders.Helpers;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.Orders.Handlers.OrderDetails
{
	public class ErledigtUpdateHandler: IHandle<int, ResponseModel<bool>>
	{
		private int data { get; set; }
		private UserModel user { get; set; }

		public ErledigtUpdateHandler(UserModel user, int data)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<bool> Handle()
		{
			try
			{
				var validation = Validate();
				if(!validation.Success)
				{
					return validation;
				}

				return Perform();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<bool> Perform()
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			botransaction.beginTransaction();
			try
			{
				var bestellArtikel = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.GetWithTransaction(data, botransaction.connection, botransaction.transaction);
				bestellArtikel.erledigt_pos = bestellArtikel.erledigt_pos.HasValue ? !bestellArtikel.erledigt_pos.Value : true;
				var deleted = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.UpdateWithTransaction(bestellArtikel, botransaction.connection, botransaction.transaction);
				// - Logging
				var bestellung = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(bestellArtikel.Bestellung_Nr ?? -1);
				BestellungenProcessing_LogEntity _log = new LogHelper(
					bestellung.Nr,
					bestellArtikel.Bestellung_Nr ?? -1,
					0,
					$"{bestellung.Typ}",
					LogHelper.LogType.MODIFICATIONORDER, "MTM", user)
					.LogMTM(bestellArtikel.Position ?? 0, $"Set from {!bestellArtikel.erledigt_pos} to {bestellArtikel.erledigt_pos}");
				Infrastructure.Data.Access.Tables.MTM.BestellungenProcessing_LogAccess.InsertWithTransaction(_log, botransaction.connection, botransaction.transaction);

				// - 
				if(botransaction.commit())
				{
					return ResponseModel<bool>.SuccessResponse(deleted > 0);

				}
				else
					return ResponseModel<bool>.FailureResponse("Transaction didn't commit.");

			} catch(Exception e)
			{
				botransaction.rollback();
				throw;
			}

			//return ResponseModel<bool>.SuccessResponse();
		}

		public ResponseModel<bool> Validate()
		{
			if(user == null)
			{
				return ResponseModel<bool>.AccessDeniedResponse();
			}
			if(user.Number == 0)
				return ResponseModel<bool>.FailureResponse("User need to have a User Number");

			var bestellArtikel = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.Get(data);

			if(bestellArtikel == null)
			{
				return ResponseModel<bool>.FailureResponse("Position doesn't exist.");
			}
			if(bestellArtikel.Bestellung_Nr.HasValue)
			{
				var bestellung = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(bestellArtikel.Bestellung_Nr.Value);
				if(bestellung is null)
					return ResponseModel<bool>.FailureResponse("Order doesn't exist.");

				// - 2023-07-26 -  remove for Brenner
				//if(bestellung.gebucht.HasValue && bestellung.gebucht.Value)
				//	return ResponseModel<bool>.FailureResponse("Can't edit validated Orders.");

			}
			return ResponseModel<bool>.SuccessResponse();
		}
	}
}
