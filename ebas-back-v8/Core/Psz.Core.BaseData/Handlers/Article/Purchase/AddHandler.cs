using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.Purchase
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class AddHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		public Models.Article.Purchase.GetModel _data { get; set; }
		public AddHandler(UserModel user, Models.Article.Purchase.GetModel purchaseItem)
		{
			this._user = user;
			this._data = purchaseItem;
		}
		public ResponseModel<int> Handle()
		{

			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //

				this._data.letzte_Aktualisierung = DateTime.Now;
				var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(this._data.ArtikelNr ?? -1, botransaction.connection, botransaction.transaction);
				var PurchaseEntity = this._data.ToEntity(new Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity(), this._user, Enums.ObjectLogEnums.Objects.Article, articleEntity.ArtikelNr, logs, Enums.ObjectLogEnums.LogType.Add);
				PurchaseEntity.Pruftiefe_WE = 1; //  default = 1

				// - 2023-06-15
				GeneratePriceGroupsHandler.InitPricingGroup(this._data.ArtikelNr ?? -1, this._user, botransaction);

				#region >>> Logs & Notifications <<<
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(logs, botransaction.connection, botransaction.transaction);
				// -- Article level Logging
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(
					ObjectLogHelper.getLog(this._user, articleEntity.ArtikelNr,
					$"Article Purchase Price",
					$"",
					$"{PurchaseEntity.Einkaufspreis}",
					Enums.ObjectLogEnums.Objects.Article.GetDescription(),
					Enums.ObjectLogEnums.LogType.Add),
					botransaction.connection, botransaction.transaction);

				// - 
				var title = $"[{articleEntity.ArtikelNummer}] New Purchase";
				var content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
					+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>"
					+ $"<br/><span style='font-size:1.15em;'><strong>{this._user.Name?.ToUpper()}</strong> has just added a new purchase price of <strong>{PurchaseEntity.Einkaufspreis}</strong> for article <strong>{articleEntity.ArtikelNummer?.ToUpper()}</strong>."
					+ $"</span><br/><br/>You can login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/articles/{articleEntity.ArtikelNr}/purchase'>here</a>"
					+ "<br/><br/>Regards, <br/>IT Department </div>";

				var addresses = (Infrastructure.Data.Access.Tables.BSD.EmailNotificationUsersAccess.GetPurchase(botransaction.connection, botransaction.transaction)
							?? new List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity>())
							.Select(x => x.UserEmail)?.ToList();
				Module.EmailingService.SendEmailAsync(title, content, addresses, null);
				#endregion Logs & Notifications

				var insertId = Infrastructure.Data.Access.Tables.BSD.BestellnummernAccess.InsertWithTransaction(PurchaseEntity, botransaction.connection, botransaction.transaction);

				// - 2022-03-30
				CreateHandler.generateFileDAT(this._data.ArtikelNr ?? -1);

				#endregion // -- transaction-based logic -- //

				if(botransaction.commit())
				{
					// -
					return ResponseModel<int>.SuccessResponse(insertId);
				}
				else
				{
					return ResponseModel<int>.FailureResponse("Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(this._data == null)
				return ResponseModel<int>.FailureResponse("Invalid input data");

			// -
			if(this._data.Lieferanten_Nr == null)
			{
				return ResponseModel<int>.FailureResponse("Supplier must have a value");
			}
			if(this._data.Einkaufspreis == null)
			{
				return ResponseModel<int>.FailureResponse("Standard Price must have a value");
			}
			if(this._data.Einkaufspreis == null && (this._data.Einkaufspreis1 != null || this._data.Einkaufspreis2 != null))
			{
				return ResponseModel<int>.FailureResponse("You must put a Standard Price to add other prices");
			}
			//if (this._data != null && Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(this._data.Lieferanten_Nr??-1)==null)
			var supplierEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetLiefrantByNr(this._data.Lieferanten_Nr ?? -1);
			if(supplierEntity == null)
			{
				return ResponseModel<int>.FailureResponse("Supplier not found");
			}
			// - check if address is Standard
			//if(supplierEntity.Adresstyp != (int)Infrastructure.Data.Entities.Tables.PRS.AdressenTypEnum.Standard)
			//{
			//	return ResponseModel<int>.FailureResponse("Supplier Address is not Standard");
			//}
			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArtikelNr ?? -1) == null)
			{
				return ResponseModel<int>.FailureResponse("Article not found");
			}

			// -
			if(!string.IsNullOrWhiteSpace(this._data.Angebot_Datum) && DateTime.TryParseExact(this._data.Angebot_Datum?.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var d) == false)
				return ResponseModel<int>.FailureResponse($"Angebotsdatum value incorrect: {this._data.Angebot_Datum?.Trim()}");

			if(!string.IsNullOrWhiteSpace(this._data.Einkaufspreis_gultig_bis) && DateTime.TryParseExact(this._data.Einkaufspreis_gultig_bis?.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var d1) == false)
				return ResponseModel<int>.FailureResponse($"Einkaufspreis gultig bis value incorrect: {this._data.Einkaufspreis_gultig_bis?.Trim()}");

			if(!string.IsNullOrWhiteSpace(this._data.Einkaufspreis1_gultig_bis) && DateTime.TryParseExact(this._data.Einkaufspreis1_gultig_bis?.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var d2) == false)
				return ResponseModel<int>.FailureResponse($"Einkaufspreis 1 gultig bis value incorrect: {this._data.Einkaufspreis1_gultig_bis?.Trim()}");

			if(!string.IsNullOrWhiteSpace(this._data.Einkaufspreis2_gultig_bis) && DateTime.TryParseExact(this._data.Einkaufspreis2_gultig_bis?.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var d3) == false)
				return ResponseModel<int>.FailureResponse($"Einkaufspreis 2 gultig bis value incorrect: {this._data.Einkaufspreis2_gultig_bis?.Trim()}");

			var have_standard_supplier = Infrastructure.Data.Access.Tables.BSD.BestellnummernAccess.GetByStandardSupplier(this._data.ArtikelNr ?? -1);
			var exsisting_supplier = Infrastructure.Data.Access.Tables.BSD.BestellnummernAccess.GetByArticleAndSupplier(this._data.ArtikelNr ?? -1, this._data.Lieferanten_Nr ?? -1);
			if(exsisting_supplier != null && exsisting_supplier.Count > 0)
			{
				return ResponseModel<int>.FailureResponse("Same supplier already exsists for this article !");
			}
			if(this._data.Standardlieferant.HasValue && this._data.Standardlieferant.Value && (have_standard_supplier != null && have_standard_supplier.Count > 0))
			{
				return ResponseModel<int>.FailureResponse("Article already have Standard Supplier!");
			}
			// - 2024-02-28 - Frischholz art. should have at least one std supp.
			if((have_standard_supplier == null || have_standard_supplier.Count <= 0) && this._data.Standardlieferant != true)
			{
				return ResponseModel<int>.FailureResponse("Article does not have any Standard Supplier!");
			}
			// - 2023-07-06 - Required fields
			var errors = new List<string>();
			if(this._data.Mindestbestellmenge.HasValue == false)
			{
				errors.Add("Price must have a min. quantity (Mindestbestellmenge)");
			}
			else if(this._data.Mindestbestellmenge < 0)
			{
				errors.Add($"Min. quantity (Mindestbestellmenge) value incorrect: [{this._data.Mindestbestellmenge}]");
			}
			if(this._data.Wiederbeschaffungszeitraum.HasValue == false)
			{
				errors.Add("Price must have a replacement period (Wiederbeschaffungszeitraum)");
			}
			else if(this._data.Wiederbeschaffungszeitraum < 0)
			{
				errors.Add($"Replacement period (Wiederbeschaffungszeitraum) value incorrect: [{this._data.Wiederbeschaffungszeitraum}]");
			}
			if(this._data.Verpackungseinheit.HasValue == false)
			{
				errors.Add("Price must have a Packaging unit (Verpackungseinheit)");
			}
			else if(this._data.Verpackungseinheit < 0)
			{
				errors.Add($"Packaging unit (Verpackungseinheit) value incorrect: [{this._data.Verpackungseinheit}]");
			}
			if(this._data.Einkaufspreis.HasValue == false)
			{
				errors.Add("Price must have a purchase price (Einkaufspreis)");
			}
			else if(this._data.Einkaufspreis < 0)
			{
				errors.Add($"Purchase price (Einkaufspreis) value incorrect: [{this._data.Einkaufspreis}]");
			}

			if(errors.Count > 0)
			{
				return ResponseModel<int>.FailureResponse(errors);
			}
			return ResponseModel<int>.SuccessResponse();
		}

	}

}
