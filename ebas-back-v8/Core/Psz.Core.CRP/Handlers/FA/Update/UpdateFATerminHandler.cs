using Psz.Core.Common.Models;
using Psz.Core.CRP.Enums;
using Psz.Core.CRP.Models.FA;
using Psz.Core.SharedKernel.Interfaces;


namespace Psz.Core.CRP.Handlers.FA
{
	public class UpdateFATerminHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private FAUpdateTerminModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public UpdateFATerminHandler(FAUpdateTerminModel data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data.Fertigungsnummer ?? -1);
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get((int)faEntity.Artikel_Nr);
				var historyEntity = new Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity
				{
					Fertigungsnummer = this._data.Fertigungsnummer,
					Artikelnummer = this._data.Artikelnummer,
					Bezeichnung = this._data.Bezeichnung,
					FA_Menge = Convert.ToInt32(this._data.FA_Menge ?? -1),
					Änderungsdatum = DateTime.Now,
					Bemerkung = faEntity.Bemerkung,
					Termin_Wunsch = faEntity.Termin_Fertigstellung,
					Angebot_Nr = int.TryParse(this._data.Angebot_Nr, out var val) ? val : 0,
					Termin_Angebot = this._data.Termin_Angebot,
					CS_Mitarbeiter = "",
					Termin_Bestätigt1 = this._data.Termin_Bestatigt1,
					Termin_voränderung = faEntity.Termin_Bestatigt1,
					Ursprünglicher_termin = this._data.Ursprunglicher_termin,
					Mitarbeiter = this._user.Name,
					Lagerort_id = faEntity.Lagerort_id,
					Erstmuster = faEntity.Erstmuster,
					Materialproblem = this._data.Materialproblem,
					Materialproblematik = this._data.Materialproblematik,
					Kapazitätsproblem = this._data.Kapazitatsproblem,
					Kapazitätsproblematik = this._data.Kapazitatsproblematik,
					Werkzeugproblem = this._data.Werkzeugproblem,
					Werkzeugproblematik = this._data.Werkzeugproblematik,
					Sonstiges = this._data.Sonstiges,
					Sonstige_Problematik = this._data.Sonstige_Problematik,
					Wunsch_CS = this._data.Wunsch_CS,
					Grund_CS = this._data.Grund_CS,
				};
				Infrastructure.Data.Access.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieAccess.InsertWithTransaction(historyEntity, botransaction.connection, botransaction.transaction);
				// - logging -  2025-02-13
				Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(new Core.CRP.Helpers.LogHelper(faEntity.Fertigungsnummer ?? -1, 0, 0, "Fertigung", Core.CRP.Helpers.LogHelper.LogType.MODIFICATIONOBJECT, "CTS", _user)
						.LogCTS($"Termin_Bestätigt1", $"{faEntity.Termin_Bestatigt1}", $"{_data.Termin_Bestatigt1}", 0), botransaction.connection, botransaction.transaction);
				// -
				faEntity.Termin_Bestatigt1 = this._data.Termin_Bestatigt1;

				#region Mail notification
				var addresses = new List<string>();

				var _lagerCompany = Infrastructure.Data.Access.Tables.CTS.lagerCompanyAccess.GetByLagerId(faEntity.Lagerort_id ?? -1);
				var _company = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(_lagerCompany?.Company_id ?? -1);
				if(_company != null)
					addresses.Add(_company.DirectorEmail);

				var _kundeMitarbeiter = Infrastructure.Data.Access.Joins.CTS.Divers.FAMitarbiter((int)faEntity.Fertigungsnummer);
				if(_kundeMitarbeiter != null)
				{
					var _mitarbeiterUser = Infrastructure.Data.Access.Tables.COR.UserAccess.GetByName(_kundeMitarbeiter);
					addresses.Add(_mitarbeiterUser?.Email);
				}

				addresses.Add(_user.Email);

				/// - 2025-04-26 Add Fa notification users 
				List<int> faUsersIds = Infrastructure.Data.Access.Tables.CRP.CRP_FA_EmailUsersAccess
					.Get()
					.Where(x => x.UserId.HasValue)
					.Select(x => x.UserId.Value)
					.ToList();
				List<string> faUsersSiteEmails = Infrastructure.Data.Access.Tables.COR.UserAccess.GetBySite(faUsersIds, (int)faEntity.Lagerort_id)
					.Where(user => user.IsActivated == true)
					.Select(x => x.Email).ToList();

				addresses.AddRange(faUsersSiteEmails);

				string faLink = $"{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/fertigung/details/{faEntity.ID}";



				var frozenZone = DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays);
				var subject = "";

				if(_data.Termin_Bestatigt1 <= frozenZone && _data.Termin_voranderung1 > frozenZone)
				{
					faEntity.PlanningDateViolation = true;
					Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Update(faEntity);
					subject = $"[{this._data.Fertigungsnummer}] Termin changed - ⚠️ [ALERT] FA in Frozen Zone";
				}
				else
				{
					subject = $"[{this._data.Fertigungsnummer}] Termin changed";
				}


				var content = $"" +
					$"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
				+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>"
				+ $"<br/><span style='font-size:1.15em;'><strong>{this._user.Name?.ToUpper()}</strong> has just changed the Termin of the FA {_data.Fertigungsnummer}</strong>."
				+ $"</span><br/><br/>"
				+ "</div>";

				if(_data.Termin_Bestatigt1 <= frozenZone && _data.Termin_voranderung1 > frozenZone)
				{
					content += $"<hr>";
					content += $"<div style='background-color:#ffdddd;border-left:6px solid #d9534f;padding:12px;margin-top:16px;margin-bottom:16px;font-size:1.1em;'> " +
						$"⚠️ <strong>Alert:</strong> The new FA date <strong>{historyEntity.Termin_Bestätigt1.Value.ToString("dd-MM-yyyy", new System.Globalization.CultureInfo("en-US"))}</strong> is in the Frozen Zone. Please review carefully.</div>";
				}

				content += $"<br/><span style='font-size:1.em;font-weight:bold'> FA :<a href='{faLink}'>{faEntity.Fertigungsnummer}</a></span>";
				content += $"<br/><span style='font-size:1.em;font-weight:bold'> FA Menge :{historyEntity.FA_Menge}</span>";
				content += $"<br/><span style='font-size:1.em;font-weight:bold'> Article :{articleEntity.ArtikelNummer}</span>";
				content += $"<br/><span style='font-size:1.em;font-weight:bold'> Artikel Bezeichnung  :{articleEntity.Bezeichnung1}</span>";
				content += $"<br/><span style='font-size:1.em;font-weight:bold'> Termin vor Änderung :{_data.Termin_voranderung1.Value.ToString("dd-MM-yyyy")}</span>";
				content += $"<br/><span style='font-size:1.em;font-weight:bold; color:red'> Neuer Termin :{_data.Termin_Bestatigt1.Value.ToString("dd-MM-yyyy")}</span>";
				content += $"<br/><span style='font-size:1.em;font-weight:bold'> Geändert von :{_user.Name}</span>";
				content += $"<hr>";
				content += $"<br/><span style='font-size:2.em;font-weight:bold;color:blue'>[Grund für Verschiebung Inhalt]</span></br>";
				if(historyEntity.Materialproblem.HasValue && historyEntity.Materialproblem.Value)
				{
					content += $"<br/><span style='font-size:2.em;font-weight:bold;color:green'>MaterialProblem :</span>";
					content += $"<br/><span style='font-size:1.em;font-weight:bold'>Materialproblematik : {historyEntity.Materialproblematik}</span></br>";
				}
				if(historyEntity.Kapazitätsproblem.HasValue && historyEntity.Kapazitätsproblem.Value)
				{
					content += $"<br/><span style='font-size:2.em;font-weight:bold;color:green'>Kapazitätsproblem :</span>";
					content += $"<br/><span style='font-size:1.em;font-weight:bold'>Kapazitätsproblematik : {historyEntity.Kapazitätsproblematik}</span></br>";
				}
				if(historyEntity.Werkzeugproblem.HasValue && historyEntity.Werkzeugproblem.Value)
				{
					content += $"<br/><span style='font-size:2.em;font-weight:bold;color:green'>Fehlender Werkzeug :</span>";
					content += $"<br/><span style='font-size:1.em;font-weight:bold'>Werkzeugproblematik : {historyEntity.Werkzeugproblematik}</span></br>";
				}
				if(historyEntity.Sonstiges.HasValue && historyEntity.Sonstiges.Value)
				{
					content += $"<br/><span style='font-size:2.em;font-weight:bold;color:green'>Sonstiges :</span>";
					content += $"<br/><span style='font-size:1.em;font-weight:bold'>Reason : {historyEntity.Sonstige_Problematik}</span></br>";
				}
				if(historyEntity.Wunsch_CS.HasValue && historyEntity.Wunsch_CS.Value)
				{
					content += $"<br/><span style='font-size:2.em;font-weight:bold;color:green'>Wunsch von CS :</span>";
					content += $"<br/><span style='font-size:1.em;font-weight:bold'>Grund_CS : {historyEntity.Grund_CS}</span>";
				}
				content += "<br/><br/>";
				content += $"<br/><span style='font-size:1.em;font-weight:bold'>Regards,</span>";
				content += $"<br/><span style='font-size:1.em;font-weight:bold'>IT Department </span></br>";
				try
				{
					// - 2025-08-06 // deactivate ALL FA Email notifs - Khelil
					//sendEmailNotification(subject, content, addresses);
				} catch(Exception exm)
				{
					Infrastructure.Services.Logging.Logger.Log(exm);
				}
				#endregion
				var response = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.UpdateWithTransaction(faEntity, botransaction.connection, botransaction.transaction);
				Helpers.SpecialHelper.UpdateFACapacity(faEntity, _user.Id);

				#endregion // -- transaction-based logic -- //


				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(response);
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
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data.Fertigungsnummer ?? -1);
			if(faEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"FA not found");
			if(CheckIfOneChecked(this._data.Materialproblem, this._data.Kapazitatsproblem, this._data.Werkzeugproblem, this._data.Wunsch_CS, this._data.Sonstiges) == false)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"please check at least one reason");

			if(this._data.Materialproblem.HasValue && this._data.Materialproblem.Value
					&& (string.IsNullOrEmpty(this._data.Materialproblematik) || string.IsNullOrWhiteSpace(this._data.Materialproblematik)))
				return ResponseModel<int>.FailureResponse(key: "1", value: $"please enter reason for Materialproblem");

			if(this._data.Kapazitatsproblem.HasValue && this._data.Kapazitatsproblem.Value
				&& (string.IsNullOrEmpty(this._data.Kapazitatsproblematik) || string.IsNullOrWhiteSpace(this._data.Kapazitatsproblematik)))
				return ResponseModel<int>.FailureResponse(key: "1", value: $"please enter reason for Kapazitatsproblematik");

			if(this._data.Werkzeugproblem.HasValue && this._data.Werkzeugproblem.Value
				&& (string.IsNullOrEmpty(this._data.Werkzeugproblematik) || string.IsNullOrWhiteSpace(this._data.Werkzeugproblematik)))
				return ResponseModel<int>.FailureResponse(key: "1", value: $"please enter reason for Werkzeugproblematik");

			if(this._data.Wunsch_CS.HasValue && this._data.Wunsch_CS.Value
				&& (string.IsNullOrEmpty(this._data.Grund_CS) || string.IsNullOrWhiteSpace(this._data.Grund_CS)))
				return ResponseModel<int>.FailureResponse(key: "1", value: $"please enter reason for Wunsch_CS");

			if(this._data.Sonstiges.HasValue && this._data.Sonstiges.Value
				&& (string.IsNullOrEmpty(this._data.Sonstige_Problematik) || string.IsNullOrWhiteSpace(this._data.Sonstige_Problematik)))
				return ResponseModel<int>.FailureResponse(key: "1", value: $"please enter reason for Wunsch_CS");

			if(!this._data.Termin_Bestatigt1.HasValue)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"date can not be null");
			// - 2023-04-11
			if(this._user?.Access?.CustomerService?.FAWerkWunshAdmin != true && faEntity.FA_Gestartet == true)
			{
				return ResponseModel<int>.FailureResponse($"Cannot update FA already started");
			}
			if(faEntity.Kennzeichen.ToLower() == FAEnums.FaStatus.storno.ToString())
			{
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Cannot update FA , FA Status is STORNO");
			}
			// - 2023-02-03 // 2024-01-25 - Khelil change H1 to 41 days
			var frZone = DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays);
			var _oldDate = faEntity.Termin_Bestatigt1 ?? new DateTime(1900, 1, 1);
			var _newDate = this._data.Termin_Bestatigt1 ?? new DateTime(1900, 1, 1);

			if(_newDate < DateTime.Today)
			{
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Production Date [{_newDate.ToString("dd/MM/yyyy")}] in the past .");
			}

			// - 2023-04-11
			if(this._user?.Access?.CustomerService?.FAWerkWunshAdmin != true && faEntity.FA_Gestartet == true)
			{
				return ResponseModel<int>.FailureResponse($"Cannot update FA already started");
			}

			if(this._user?.Access?.CustomerService?.FAWerkWunshAdmin != true && this._user.Access?.CustomerService?.FaAdmin != true && faEntity.Lagerort_id != 6 && faEntity.Technik != true && !Module.BSD.TechnicArticleIds.Exists(x => x == faEntity.Artikel_Nr))
			{
				// - 2023-06-13 check all FA - if(faEntity.FA_Gestartet != true) // - 2023-04-04 - publish - 2023-04-05 
				{
					if(_newDate < DateTime.Today)
					{
						return ResponseModel<int>.FailureResponse($"Production date invalid: can not add FA [{faEntity.Fertigungsnummer}] [{_newDate.ToString("dd/MM/yyyy")}] in the past.");
					}
					if(_newDate <= frZone && _oldDate > frZone)
					{
						return ResponseModel<int>.FailureResponse($"Production date invalid: can not bring FA [{faEntity.Fertigungsnummer}] [{_newDate.ToString("dd/MM/yyyy")}] in Frozen Zone [{frZone.ToString("dd/MM/yyyy")}].");
					}
					if(_newDate > frZone && _oldDate <= frZone)
					{
						return ResponseModel<int>.FailureResponse($"Production date invalid: can not move FA [{faEntity.Fertigungsnummer}] [{_newDate.ToString("dd/MM/yyyy")}] out of Frozen Zone [{frZone.ToString("dd/MM/yyyy")}].");
					}
				}
			}
			//var technicArticles = Module.BSD.TechnicArticleIds;
			var horizonCheck = Helpers.HorizonsHelper.userHasFaUpdateTerminHorizonRight(_newDate, _oldDate, _user, out List<string> messages);
			if(!horizonCheck && !Helpers.HorizonsHelper.ArticleIsTechnic(faEntity.Artikel_Nr ?? -1))
				return ResponseModel<int>.FailureResponse(messages);
			return ResponseModel<int>.SuccessResponse();
		}
		public bool CheckIfOneChecked(bool? val1, bool? val2, bool? val3, bool? val4, bool? val5)
		{
			if((!val1.HasValue || (val1.HasValue && !val1.Value))
				&&
				(!val2.HasValue || (val2.HasValue && !val2.Value))
				&&
				(!val3.HasValue || (val3.HasValue && !val3.Value))
				&&
				(!val4.HasValue || (val4.HasValue && !val4.Value))
				&
				(!val5.HasValue || (val5.HasValue && !val5.Value))
				)
				return false;
			else
				return true;
		}
		void sendEmailNotification(string title, string contentHtml, List<string> toEmailAddresses)
		{
			try
			{
				Module.EmailingService.SendEmailAsync(title, contentHtml, toEmailAddresses, null);
			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{string.Join(",", Module.EmailingService.EmailParamtersModel.BOMEmailDestinations)}]"));
				Infrastructure.Services.Logging.Logger.Log(ex);
			}
		}
	}
}