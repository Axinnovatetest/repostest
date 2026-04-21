using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Reception
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class UnbookFullHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public UnbookFullHandler(Identity.Models.UserModel user, int id)
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
				var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(_data);
				var orderExtEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(orderEntity.Nr);
				var bookingEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.GetReceptionsByOrderId(this._data);

				foreach(var bookingItem in bookingEntities)
				{
					// - cancel Invoice, delete Booking & reset Lager
					UnbookHandler.UnbookReception(bookingItem.Nr);
				}

				// - 3 Reset Order
				Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.UpdateCompletionStatus(this._data, false);
				var orderArticleEntities = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.GetByOrderId(this._data)
					?? new List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity>();

				var bookedOrderArticles = new List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity>();
				foreach(var bookedItem in orderArticleEntities)
				{
					bookedItem.Anzahl = bookedItem.Aktuelle_Anzahl;
					bookedItem.Erhalten = 0;
					bookedItem.Gesamtpreis = bookedItem.Aktuelle_Anzahl * bookedItem.Einzelpreis / bookedItem.Preiseinheit;
					bookedItem.erledigt_pos = false;
					bookedItem.Bemerkung_Pos = "";
				}
				// -
				Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.Update(orderArticleEntities);


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

					Module.EmailingService.SendEmailSendGridWithStaticTemplate(emailContent, emailTitle,
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
				Helpers.Processings.Budget.Order.SaveOrderHistory(orderExtEntity, Enums.BudgetEnums.OrderWorkflowActions.Unbook, this._user, $"Un-Booked Order [{orderExtEntity?.OrderNumber?.ToUpper()}]");

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

			var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(this._data);
			if(orderEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "Order not found");

			if(Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.GetReceptionsByOrderId(_data) == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "Booking not found");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
