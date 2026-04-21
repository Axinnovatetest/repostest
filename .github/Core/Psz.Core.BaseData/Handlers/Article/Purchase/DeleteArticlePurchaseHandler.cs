using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.Purchase
{
	public class DeleteArticlePurchaseHandler: IHandle<int, ResponseModel<object>>
	{
		private int _purchaseNr { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public DeleteArticlePurchaseHandler(Identity.Models.UserModel user, int purchaseNr)
		{
			this._user = user;
			this._purchaseNr = purchaseNr;
		}
		public ResponseModel<object> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var PurchaseEntity = Infrastructure.Data.Access.Tables.BSD.BestellnummernAccess.Get(this._purchaseNr);
				if(PurchaseEntity == null)
				{
					return ResponseModel<object>.SuccessResponse();
				}
				var result = Infrastructure.Data.Access.Tables.BSD.BestellnummernAccess.Delete(this._purchaseNr);

				#region >>> Logs & Notifications <<<
				// -
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(PurchaseEntity.ArtikelNr ?? -1);

				// -- Article level Logging
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
					ObjectLogHelper.getLog(this._user, articleEntity?.ArtikelNr ?? -1,
					$"Article Purchase Price",
					$"{PurchaseEntity.Einkaufspreis}",
					$"{PurchaseEntity.Einkaufspreis}",
					Enums.ObjectLogEnums.Objects.Article.GetDescription(),
					Enums.ObjectLogEnums.LogType.Delete));

				// - 
				if(articleEntity != null)
				{
					// - 2022-03-30
					CreateHandler.generateFileDAT(articleEntity.ArtikelNr);

					var title = $"[{articleEntity.ArtikelNummer}] Delete Purchase";
					var content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
						+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>"
						+ $"<br/><span style='font-size:1.15em;'><strong>{this._user.Name?.ToUpper()}</strong> has just deleted the purchase price <strong>{PurchaseEntity.Einkaufspreis}</strong> from article <strong>{articleEntity.ArtikelNummer?.ToUpper()}</strong>."
						+ $"</span><br/><br/>You can login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/articles/{articleEntity.ArtikelNr}/purchase'>here</a>"
						+ "<br/><br/>Regards, <br/>IT Department </div>";

					var addresses = (Infrastructure.Data.Access.Tables.BSD.EmailNotificationUsersAccess.GetPurchase()
								?? new List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity>())
								.Select(x => x.UserEmail)?.ToList();
					Module.EmailingService.SendEmailAsync(title, content, addresses, null);
				}
				#endregion Logs & Notifications

				// -
				return ResponseModel<object>.SuccessResponse(result);
			} catch(Exception)
			{

				throw;
			}
		}
		public ResponseModel<object> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<object>.AccessDeniedResponse();
			}


			var PurchaseEntity = Infrastructure.Data.Access.Tables.BSD.BestellnummernAccess.Get(this._purchaseNr);
			if(PurchaseEntity == null)
			{
				return ResponseModel<object>.SuccessResponse();
			}

			var have_standard_supplier = Infrastructure.Data.Access.Tables.BSD.BestellnummernAccess.GetByStandardSupplier(PurchaseEntity.ArtikelNr ?? -1);
			// - 2024-02-28 - Frischholz art. should have at least one std supp.
			if(have_standard_supplier?.Count <= 0)
			{
				return ResponseModel<object>.FailureResponse("Article does not have any Standard Supplier!");
			}
			if(have_standard_supplier?.Count == 1 && have_standard_supplier[0].Nr == this._purchaseNr)
			{
				return ResponseModel<object>.FailureResponse("Article will not have any Standard Supplier after deletion!");
			}

			// - 2024-03-12 - prevent delete EK-price when exist open Bestellung - KH
			if(PurchaseEntity.Lieferanten_Nr.HasValue && PurchaseEntity.Lieferanten_Nr.Value > 0)
			{
				var openOrders = Infrastructure.Data.Access.Tables.BSD.BestellungenAccess.GetOpenOrdersByLieferant(PurchaseEntity.Lieferanten_Nr ?? -1);
				if(openOrders?.Count > 0)
				{
					var openPositions = Infrastructure.Data.Access.Tables.BSD.Bestellte_ArtikelAccess.GetByArticleAndBestellung(PurchaseEntity.ArtikelNr ?? -1, openOrders.Select(x => x.Nr).ToList(), true);
					if(openPositions?.Count > 0)
					{
						return ResponseModel<object>.FailureResponse($"Deletion aborted: Article has booked open orders in progress. Please change/delete them first.");
					}
				}
			}
			return ResponseModel<object>.SuccessResponse();
		}
	}
}
