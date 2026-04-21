using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Settings.CustomerItemNumber
{
	public class EditHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Settings.CustomerItemNumber.CustomerItemNumberResponseModel _data { get; set; }

		public EditHandler(Identity.Models.UserModel user, Models.Settings.CustomerItemNumber.CustomerItemNumberResponseModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				//TODO: - insert process here
				botransaction.beginTransaction();

				var entity = Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.Get(this._data.ID);
				var data = this._data.ToEntity();
				data.LastEditUserId = this._user.Id;
				data.LastEditUserName = this._user.Username;
				var responseBody = Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.UpdateWithTransaction(data, botransaction.connection, botransaction.transaction);

				if(entity.Kundennummer != this._data.Kundennummer)
				{
					Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.EditCustomerWithTransaction(data.Kundennummer, this._data.Nummerschlussel,
						botransaction.connection, botransaction.transaction);
				}

				// -
				var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
				if(entity.Nummerschlüssel != this._data.Nummerschlussel)
				{
					logs.Add(ObjectLogHelper.getLog(this._user, responseBody,
						  $"Kunde: {this._data.Nummerschlussel} - Nummerschlüssel",
						  $"{entity.Nummerschlüssel}",
						  $"{this._data.Nummerschlussel}", Enums.ObjectLogEnums.Objects.ArticleConfig_CustomerItemNumber.GetDescription(),
						  Enums.ObjectLogEnums.LogType.Edit));
				}
				if(entity.Kunde != this._data.Kunde)
				{
					logs.Add(ObjectLogHelper.getLog(this._user, responseBody,
						  $"Kunde: {this._data.Nummerschlussel} - Kunde",
						  $"{entity.Kunde}",
						  $"{this._data.Kunde}", Enums.ObjectLogEnums.Objects.ArticleConfig_CustomerItemNumber.GetDescription(),
						  Enums.ObjectLogEnums.LogType.Edit));
				}
				if(entity.Kundennummer != this._data.Kundennummer)
				{
					logs.Add(ObjectLogHelper.getLog(this._user, responseBody,
						  $"Kunde: {this._data.Nummerschlussel} - Kundennummer",
						  $"{entity.Kundennummer}",
						  $"{this._data.Kundennummer}", Enums.ObjectLogEnums.Objects.ArticleConfig_CustomerItemNumber.GetDescription(),
						  Enums.ObjectLogEnums.LogType.Edit));
				}
				if(entity.CS_Kontakt != this._data.CS_Kontakt)
				{
					logs.Add(ObjectLogHelper.getLog(this._user, responseBody,
						  $"Kunde: {this._data.Nummerschlussel} - CS_Kontakt",
						  $"{entity.CS_Kontakt}",
						  $"{this._data.CS_Kontakt}", Enums.ObjectLogEnums.Objects.ArticleConfig_CustomerItemNumber.GetDescription(),
						  Enums.ObjectLogEnums.LogType.Edit));
				}
				if(entity.Technik_Kontakt != this._data.Technik_Kontakt)
				{
					logs.Add(ObjectLogHelper.getLog(this._user, responseBody,
						  $"Kunde: {this._data.Nummerschlussel} - Technik_Kontakt",
						  $"{entity.Technik_Kontakt}",
						  $"{this._data.Technik_Kontakt}", Enums.ObjectLogEnums.Objects.ArticleConfig_CustomerItemNumber.GetDescription(),
						  Enums.ObjectLogEnums.LogType.Edit));
				}
				if(entity.Stufe != this._data.Stufe)
				{
					logs.Add(ObjectLogHelper.getLog(this._user, responseBody,
						  $"Kunde: {this._data.Nummerschlussel} - Stufe",
						  $"{entity.Stufe}",
						  $"{this._data.Stufe}", Enums.ObjectLogEnums.Objects.ArticleConfig_CustomerItemNumber.GetDescription(),
						  Enums.ObjectLogEnums.LogType.Edit));
				}
				if(entity.Technik_Kontakt_TN != this._data.Technik_Kontakt_TN)
				{
					logs.Add(ObjectLogHelper.getLog(this._user, responseBody,
						  $"Kunde: {this._data.Nummerschlussel} - Technik_Kontakt_TN",
						  $"{entity.Technik_Kontakt_TN}",
						  $"{this._data.Technik_Kontakt_TN}", Enums.ObjectLogEnums.Objects.ArticleConfig_CustomerItemNumber.GetDescription(),
						  Enums.ObjectLogEnums.LogType.Edit));
				}
				if(entity.Projektbetreuer_D != this._data.Projektbetreuer_D)
				{
					logs.Add(ObjectLogHelper.getLog(this._user, responseBody,
						  $"Kunde: {this._data.Nummerschlussel} - Projektbetreuer_D",
						  $"{entity.Projektbetreuer_D}",
						  $"{this._data.Projektbetreuer_D}", Enums.ObjectLogEnums.Objects.ArticleConfig_CustomerItemNumber.GetDescription(),
						  Enums.ObjectLogEnums.LogType.Edit));
				}

				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(logs, botransaction.connection, botransaction.transaction);



				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(responseBody);
				}
				else
				{
					return ResponseModel<int>.FailureResponse(key: "1", value: "Transaction error");
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
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(string.IsNullOrEmpty(this._data.Kunde))
			{
				return ResponseModel<int>.FailureResponse("Kunde Name should not be empty");
			}
			if(!this._data.Kundennummer.HasValue || this._data.Kundennummer.Value <= 0)
			{
				return ResponseModel<int>.FailureResponse("Kundennummer should not be empty");
			}
			if(string.IsNullOrEmpty(this._data.Nummerschlussel))
			{
				return ResponseModel<int>.FailureResponse("Nummerschlüssel should not be empty");
			}

			var entity = Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.Get(this._data.ID);
			if(entity == null)
			{
				return ResponseModel<int>.FailureResponse("Item not found");
			}
			// - changing customer
			if(entity.Kundennummer != this._data.Kundennummer)
			{
				var newCustomer = Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.GetByCustomerAndKreis(this._data.Kundennummer ?? 0, this._data.Nummerschlussel);
				if(newCustomer != null && newCustomer.Count > 0)
				{
					return ResponseModel<int>.FailureResponse($"Kunde [{(this._data.Kundennummer ?? 0)} | {this._data.Kunde}] with Nummerschlüssel [{this._data.Nummerschlussel}] already exists");
				}

				//var customerArticles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetEFByKreis(this._data.Nummerschlussel);
				//if(customerArticles!= null && customerArticles.Count > 0)
				//{
				//    return ResponseModel<int>.FailureResponse($"Kunde [{(this._data.Kundennummer ?? 0)} | {this._data.Kunde}] has articles [{(string.Join(", ", customerArticles.Take(5).Select(x=>x.ArtikelNummer)))}]. Please change/delete the articles first.");
				//}
			}
			// - changing Nummer
			if(entity.Nummerschlüssel != this._data.Nummerschlussel)
			{
				var sameKreis = Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.GetByKundeKreis(this._data.Nummerschlussel);
				if(sameKreis != null && sameKreis.Count > 0)
				{
					return ResponseModel<int>.FailureResponse($"Nummerschlüssel [{this._data.Nummerschlussel}] already exists");
				}
			}

			// -
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
