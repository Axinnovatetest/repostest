using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Reception
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class UnbookHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public UnbookHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				var bookingEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(_data, Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Types.Booking);
				var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(bookingEntity.Bestellung_Nr ?? -1);
				var orderExtEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(orderEntity.Nr);

				// - cancel Invoice, delete Booking & reset Lager
				UnbookReception(bookingEntity.Nr);

				// - 3 Reset Order
				Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.UpdateCompletionStatus(orderEntity.Nr, false);
				var bookingArticleEntities = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.GetByOrderId(bookingEntity.Nr)
					?? new List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity>();
				var orderArticleEntities = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.GetByOrderId(orderEntity.Nr)
					?? new List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity>();

				var orderArticlesToUpdate = new List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity>();
				foreach(var bookedItem in bookingArticleEntities)
				{
					var orderArticle = orderArticleEntities.FirstOrDefault(x => x.Artikel_Nr == bookedItem.Artikel_Nr
							&& x.Position == bookedItem.Position
							&& x.Aktuelle_Anzahl == bookedItem.Aktuelle_Anzahl);
					if(orderArticle != null)
					{
						orderArticle.Anzahl = (orderArticle.Anzahl ?? 0) + (bookedItem.Anzahl ?? 0);
						orderArticle.Erhalten = (orderArticle.Erhalten ?? 0) - (bookedItem.Erhalten);
						orderArticle.Gesamtpreis = ((orderArticle.Anzahl ?? 0) + (bookedItem.Anzahl ?? 0)) * bookedItem.Einzelpreis / bookedItem.Preiseinheit;
						orderArticle.erledigt_pos = false;
						orderArticle.Bemerkung_Pos += $" // Unbooking {this._user.Username}, {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}";

						// -
						orderArticlesToUpdate.Add(orderArticle);
					}
				}
				// -
				Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.Update(orderArticlesToUpdate);


				// -
				#region Email notifs
				var originalOrderUser = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(orderExtEntity?.IssuerId ?? -1);
				string emailTitle = "[Budget] Order Un-booking", emailContent;
				var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(orderExtEntity?.ProjectId ?? -1);
				emailContent = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif; max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>";
				emailContent += $"<span style='font-size:1.5em'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")} {originalOrderUser?.Username},</span><br/>";
				emailContent += $"<br/><span style='font-size:1.15em'><strong>{this._user.Name?.ToUpper()}</strong> has just unbook the order <strong>{orderExtEntity?.OrderNumber?.ToUpper()}</strong>";


				if(projectEntity != null)
					emailContent += $" from project <strong>{projectEntity?.ProjectName?.ToUpper()}</strong>";

				emailContent += $" that you have placed.</span><br/><br/>Please, login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/budgets/budget-rebuild/orders'>here</a>";
				emailContent += "<br/><br/>Regards, <br/>IT Department</div>";

				try
				{
					// Send email notification
					Module.EmailingService.SendEmailSendGridWithStaticTemplate(
						emailContent,
						emailTitle,
					   new List<string> { originalOrderUser?.Email }, null, null,
					   true, this._user.Email, this._user.Username, senderId: this._user.Id, senderCC: false, attachmentIds: null);

					/*Module.EmailingService.SendEmailAsync(emailTitle, emailContent, new List<string> { originalOrderUser?.Email }, null,
					saveHistory: true, senderEmail: this._user.Email, senderName: this._user.Username, senderId: this._user.Id, senderCC: false, attachmentIds: null);*/

				} catch(Exception ex)
				{
					Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{string.Join(", ", originalOrderUser?.Email)}]", ex));
				}
				#endregion Email

				// - workflow history
				Helpers.Processings.Budget.Order.SaveOrderHistory(orderExtEntity, Enums.BudgetEnums.OrderWorkflowActions.Unbook, this._user, $"Un-Booked reception [{bookingEntity?.Bestellung_Nr}]");

				return ResponseModel<int>.SuccessResponse(1);
			} catch(Exception e)
			{
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

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "User not found");

			var bookingEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(_data, Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Types.Booking);
			if(bookingEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "Booking not found");

			var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(bookingEntity.Bestellung_Nr ?? -1);
			if(orderEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "Order not found");

			return ResponseModel<int>.SuccessResponse();
		}
		public static void UnbookReception(int receptionId)
		{
			try
			{
				// - 1 Delete Invoice
				var invoiceEntity = Infrastructure.Data.Access.Tables.FNC.InvoiceAccess.GetByBookingId(receptionId);
				Infrastructure.Data.Access.Tables.FNC.InvoiceAccess.SoftDelete(invoiceEntity?.Id ?? -1);

				// - 2 Delete Booking
				Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.SoftDeleteBooking(receptionId);

				// - 3 Reset Lager
				var bookedArticleEntities = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.GetByOrderId(receptionId)
					?? new List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity>();

				var lagerEntities = Infrastructure.Data.Access.Tables.FNC.LagerAccess.GetByLagerOrtAndArticle(
					bookedArticleEntities.Select(x => new Tuple<int, int>(x.Lagerort_id ?? -1, x.Artikel_Nr ?? -1)).ToList());

				foreach(var bookedItem in bookedArticleEntities)
				{
					for(int j = 0; j < lagerEntities.Count; j++)
					{
						if(lagerEntities[j].Lagerort_id == bookedItem.Lagerort_id &&
							lagerEntities[j].Artikel_Nr == bookedItem.Artikel_Nr)
						{
							lagerEntities[j].Bestand -= bookedItem.Anzahl;
							lagerEntities[j].letzte_Bewegung = DateTime.Now;
						}
					}
				}
				//-
				Infrastructure.Data.Access.Tables.FNC.LagerAccess.Update(lagerEntities);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
			}
		}
	}
}
