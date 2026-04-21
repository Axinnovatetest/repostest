using Psz.Core.MaterialManagement.Orders.Helpers;
using Psz.Core.MaterialManagement.Orders.Models.OrderDetails;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.Orders.Handlers.OrderDetails
{
	public class SplitPositionHandler: IHandle<UpdateArticleInformationRequestModel, ResponseModel<UpdateArticleInformationResponseModel>>
	{

		private UpdateArticleInformationRequestModel data { get; set; }
		private UserModel user { get; set; }

		public SplitPositionHandler(UserModel user, UpdateArticleInformationRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<UpdateArticleInformationResponseModel> Handle()
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

		private ResponseModel<UpdateArticleInformationResponseModel> Perform()
		{
			var bestellArtikel = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.Get(data.Nr);

			//Calculate CUPreis Field.
			var tbl_kupfer = Infrastructure.Data.Access.Tables.MTM.TBL_KUPFERAccess.GetLatestPrice();
			var Kupferpreis = tbl_kupfer.Aktueller_Kupfer_Preis_in_Gramm.HasValue ? tbl_kupfer.Aktueller_Kupfer_Preis_in_Gramm.Value : 0;
			var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(data.Artikel_Nr);

			var articleKupferzahl = article.Kupferzahl.HasValue ? article.Kupferzahl.Value : 0;
			data.CUPreis = (Kupferpreis * articleKupferzahl / 1000) * data.Quantity;
			//End Calculate CUPreis Field.

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			botransaction.beginTransaction();
			try
			{
				var bestellung = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.GetWithTransaction(data.Bestellung_Nr, botransaction.connection, botransaction.transaction);
				// New Psoition
				// - 2023-03-30
				data.Bemerkung = data.Bemerkung ?? "-";
				data.CUPreis = 0;
				var newPosition = data.GetEntity();
				newPosition.Bemerkung_Pos = "";
				var bestelleArtikelId = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.InsertWithTransaction(newPosition, botransaction.connection, botransaction.transaction);
				// - update old Position
				bestellArtikel.Anzahl = bestellArtikel.Anzahl - newPosition.Anzahl;
				bestellArtikel.Gesamtpreis = bestellArtikel.Gesamtpreis - newPosition.Gesamtpreis;
				bestellArtikel.Start_Anzahl = bestellArtikel.Start_Anzahl - newPosition.Anzahl;
				Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.UpdateWithTransaction(bestellArtikel, botransaction.connection, botransaction.transaction);
				//Logging
				Infrastructure.Data.Access.Tables.MTM.BestellungenProcessing_LogAccess.InsertWithTransaction(new LogHelper(
					bestellung.Nr,
					data.Bestellung_Nr,
					0,
					$"{bestellung.Typ}",
					LogHelper.LogType.CREATIONPOS, "MTM", user)
					.LogMTM(bestellArtikel.Position ?? 0), botransaction.connection, botransaction.transaction);
				Infrastructure.Data.Access.Tables.MTM.BestellungenProcessing_LogAccess.InsertWithTransaction(new LogHelper(
					bestellung.Nr,
					data.Bestellung_Nr,
					0,
					$"{bestellung.Typ}",
					LogHelper.LogType.MODIFICATIONPOS, "MTM", user)
					.LogMTM(bestellArtikel.Position ?? 0, $""), botransaction.connection, botransaction.transaction);

				// - 
				if(botransaction.commit())
				{
					return ResponseModel<UpdateArticleInformationResponseModel>.SuccessResponse();
				}
				else
					return ResponseModel<UpdateArticleInformationResponseModel>.FailureResponse("Transaction didn't commit.");

			} catch(Exception)
			{
				botransaction.rollback();
				throw;
			}
		}

		public ResponseModel<UpdateArticleInformationResponseModel> Validate()
		{
			if(user == null)
			{
				return ResponseModel<UpdateArticleInformationResponseModel>.AccessDeniedResponse();
			}

			if(user.Number == 0)
				return ResponseModel<UpdateArticleInformationResponseModel>.FailureResponse("User need to have a User Number");

			var bestellung = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(data.Bestellung_Nr);
			if(bestellung is null)
				return ResponseModel<UpdateArticleInformationResponseModel>.FailureResponse("Order not found");
			var position = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.Get(data.Nr);
			if(position is null)
				return ResponseModel<UpdateArticleInformationResponseModel>.FailureResponse("Position not found");
			//if((position.Anzahl ?? 0) < 2)
			//	return ResponseModel<UpdateArticleInformationResponseModel>.FailureResponse("Position quantity must be > 2");
			if((data.Quantity) < 1)
				return ResponseModel<UpdateArticleInformationResponseModel>.FailureResponse("Position quantity must be > 0");
			if(position.Start_Anzahl == position.Anzahl && (position.Anzahl ?? 0) - data.Quantity < 1)
				return ResponseModel<UpdateArticleInformationResponseModel>.FailureResponse("Old Position quantity must be > 0");

			if(bestellung is not null && bestellung.gebucht == true)
				return ResponseModel<UpdateArticleInformationResponseModel>.FailureResponse("Can't edit Validated Order");

			return ResponseModel<UpdateArticleInformationResponseModel>.SuccessResponse();
		}
	}
}
