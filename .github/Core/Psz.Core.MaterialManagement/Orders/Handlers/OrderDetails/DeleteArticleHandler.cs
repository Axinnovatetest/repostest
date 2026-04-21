using Infrastructure.Data.Entities.Tables.MTM;
using Psz.Core.MaterialManagement.Orders.Helpers;
using Psz.Core.MaterialManagement.Orders.Models.OrderDetails;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.Orders.Handlers.OrderDetails
{
	public class DeleteArticleHandler: IHandle<DeleteArticleRequestModel, ResponseModel<DeleteArticleResponseModel>>
	{
		private DeleteArticleRequestModel data { get; set; }
		private UserModel user { get; set; }

		public DeleteArticleHandler(UserModel user, DeleteArticleRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<DeleteArticleResponseModel> Handle()
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

		private ResponseModel<DeleteArticleResponseModel> Perform(UserModel user, DeleteArticleRequestModel data)
		{
			var bestellArtikel = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.Get(data.OrderItemId);
			var bestellung = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(bestellArtikel.Bestellung_Nr.Value);
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			botransaction.beginTransaction();
			try
			{
				var response = new ResponseModel<DeleteArticleResponseModel>();
				if(bestellArtikel.RA_Pos_zu_Bestellposition.HasValue && bestellArtikel.RA_Pos_zu_Bestellposition.Value != -1)
				{
					var diff = (bestellArtikel.Anzahl ?? 0);
					var oldPosNr = bestellArtikel.RA_Pos_zu_Bestellposition.HasValue ? bestellArtikel.RA_Pos_zu_Bestellposition.Value : -1;
					bestellArtikel.RA_Pos_zu_Bestellposition = null;
					// - 2025-08-27 - update RA qty on BE validate - Khelil
					var responseUpdate = ResponseModel<DeleteArticleResponseModel>.SuccessResponse(); //MaterialManagement.Helpers.SpecialHelper.UpdateRahmenBS<UpdateArticleInformationResponseModel>(bestellArtikel, oldPosNr, diff, botransaction);
					if(!responseUpdate.Success)
					{
						botransaction.rollback();
						return response;
					}
				}

				int deleted = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.DeleteWithTransaction(bestellArtikel.Nr, botransaction.connection, botransaction.transaction);

				//Logging
				BestellungenProcessing_LogEntity _log = new LogHelper(
					bestellung.Nr,
					bestellArtikel.Bestellung_Nr ?? -1,
					0,
					$"{bestellung.Typ}",
					LogHelper.LogType.DELETEPOS, "MTM", user)
					.LogMTM(bestellArtikel.Position ?? 0, $"");
				Infrastructure.Data.Access.Tables.MTM.BestellungenProcessing_LogAccess.InsertWithTransaction(_log, botransaction.connection, botransaction.transaction);

				// - 
				if(botransaction.commit())
				{
					return ResponseModel<DeleteArticleResponseModel>.SuccessResponse(new DeleteArticleResponseModel(deleted > 0));

				}
				else
					return ResponseModel<DeleteArticleResponseModel>.FailureResponse("Transaction didn't commit.");

			} catch(Exception e)
			{
				botransaction.rollback();
				throw;
			}

		}

		public ResponseModel<DeleteArticleResponseModel> Validate()
		{
			if(user == null)
			{
				return ResponseModel<DeleteArticleResponseModel>.AccessDeniedResponse();
			}
			if(user.Number == 0)
				return ResponseModel<DeleteArticleResponseModel>.FailureResponse("User need to have a User Number");

			var bestellArtikel = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.Get(data.OrderItemId);

			if(bestellArtikel == null)
			{
				return ResponseModel<DeleteArticleResponseModel>.FailureResponse("Position doesn't exist.");
			}
			if(bestellArtikel.Bestellung_Nr.HasValue)
			{
				var bestellung = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(bestellArtikel.Bestellung_Nr.Value);
				if(bestellung is null)
					return ResponseModel<DeleteArticleResponseModel>.FailureResponse("Order doesn't exist.");
				if(bestellung.gebucht.HasValue && bestellung.gebucht.Value)
					return ResponseModel<DeleteArticleResponseModel>.FailureResponse("Can't edit validated Orders.");

				var wareneingang = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.GetCountWE(new List<int> { data.OrderItemId });
				if(bestellArtikel.Erhalten > 0 || wareneingang != null && wareneingang.Count > 0 && wareneingang.TryGetValue(data.OrderItemId, out var _count) == true && _count > 0)
				{
					return ResponseModel<DeleteArticleResponseModel>.FailureResponse("Can't delete Received Orders.");
				}
			}
			return ResponseModel<DeleteArticleResponseModel>.SuccessResponse();
		}
	}
}
