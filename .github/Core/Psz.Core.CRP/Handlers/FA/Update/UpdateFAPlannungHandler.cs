using Infrastructure.Data.Access.Tables.CTS;
using Infrastructure.Data.Access.Tables.PRS;
using Infrastructure.Data.Entities.Tables.CTS;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.CustomerService.Models.InsideSalesWerksterminUpdates;
using Psz.Core.SharedKernel.Interfaces;


namespace Psz.Core.CRP.Handlers.FA
{
	public class UpdateFAPlannungHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private FADetailsModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public UpdateFAPlannungHandler(FADetailsModel data, Identity.Models.UserModel user)
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

				var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data.Fertigungsnummer ?? -1, botransaction.connection, botransaction.transaction);
				faEntity.Termin_Bestatigt2 = this._data.Termin_Bestatigt2;
				faEntity.Bemerkung_II_Planung = this._data.Bemerkung_II_Planung;
				faEntity.Bemerkung_zu_Prio = this._data.Bemerkung_zu_Prio;
				faEntity.Gewerk_Teilweise_Bemerkung = this._data.Gewerk_Teilweise_Bemerkung;

				var _oldEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data.Fertigungsnummer ?? -1, botransaction.connection, botransaction.transaction);
				// - 2023-08-04 - update Termin_Bestatigt2 only once
				if(_oldEntity.Termin_Bestatigt2 != this._data.Termin_Bestatigt2
					&& (faEntity.Termin_Bestatigt2_Updated == null || faEntity.Termin_Bestatigt2_Updated == false))
				{
					faEntity.Termin_Bestatigt2_Updated = true;
				}
				var response = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.UpdateWithTransaction(faEntity, botransaction.connection, botransaction.transaction);
				//logging
				var Logs = GetLogs(_oldEntity, faEntity);
				if(Logs != null && Logs.Count > 0)
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(Logs, botransaction.connection, botransaction.transaction);

				// - 2024-06-12 - Reasons for Termniwerk update
				if(_data.UpdateWerksterminData is not null)
				{
					InsertWerksterminChecks(botransaction);
				}

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
			if(string.Equals(this._data.FA_Status, "STORNO") == true)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"FA status is STORNO");
			if(string.Equals(this._data.FA_Status, "erledigt") == true)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"FA status is erledigt");
			
			//if(!this._data.Termin_Bestatigt2.HasValue)
			//	return ResponseModel<int>.FailureResponse(key: "1", value: $"date should not be null");

			if(this._data.Termin_Bestatigt2 != null && !faEntity.Termin_Bestatigt2.HasValue)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Cannot update empty Termin Werk");

			if(faEntity.Termin_Bestatigt2?.Date != this._data.Termin_Bestatigt2?.Date && faEntity.Termin_Bestatigt2_Updated.HasValue && faEntity.Termin_Bestatigt2_Updated.Value)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Termin Werk has already been updated");

			if(faEntity.FA_Gestartet == true && this._data.Termin_Bestatigt2 == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Cannot update Termin Werk because FA is started");

			// - 2024-06-12 - Reasons terminwerk update
			var terminwerkValidation = ValidateReason();
			if(!terminwerkValidation.Success)
			{
				return terminwerkValidation;
			}

			return ResponseModel<int>.SuccessResponse();
		}
		public List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity> GetLogs(Infrastructure.Data.Entities.Tables.PRS.FertigungEntity _old,
		   Infrastructure.Data.Entities.Tables.PRS.FertigungEntity _new)
		{
			var _Log = new Helpers.LogHelper((int)_old.Fertigungsnummer, 0, 0, "Fertigung", Helpers.LogHelper.LogType.MODIFICATIONOBJECT, "CTS", _user);
			List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity> _logs = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();
			if(_old.Termin_Bestatigt2 != _new.Termin_Bestatigt2)
			{
				_logs.Add(_Log.LogCTS("Termin_Bestatigt2", _old.Termin_Bestatigt2?.ToString(), _new.Termin_Bestatigt2?.ToString(), 0));
			}
			if(_old.Bemerkung_II_Planung != _new.Bemerkung_II_Planung)
			{
				_logs.Add(_Log.LogCTS("Bemerkung_II_Planung", _old.Bemerkung_II_Planung.ToString(), _new.Bemerkung_II_Planung.ToString(), 0));
			}
			if(_old.Bemerkung_zu_Prio != _new.Bemerkung_zu_Prio)
			{
				_logs.Add(_Log.LogCTS("Bemerkung_zu_Prio", _old.Bemerkung_zu_Prio.ToString(), _new.Bemerkung_zu_Prio.ToString(), 0));
			}
			if(_old.Gewerk_Teilweise_Bemerkung != _new.Gewerk_Teilweise_Bemerkung)
			{
				_logs.Add(_Log.LogCTS("Gewerk_Teilweise_Bemerkung", _old.Gewerk_Teilweise_Bemerkung.ToString(), _new.Gewerk_Teilweise_Bemerkung.ToString(), 0));
			}
			return _logs;
		}
		ResponseModel<int> ValidateReason()
		{
			if(_user == null || _user.Access.CRP.FaEdit != true)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(_data.UpdateWerksterminData?.FertigungNumber <= 0)
			{
				return ResponseModel<int>.SuccessResponse(0);
			}
			List<string> errors = new List<string>();
			if(_data.UpdateWerksterminData?.ReasonMaterial == true && String.IsNullOrEmpty(_data.UpdateWerksterminData?.ReasonMaterialComments) == true)
			{
				errors.Add("Material reason cannot be empty");
			}

			if(_data.UpdateWerksterminData?.ReasonCapacity == true && String.IsNullOrEmpty(_data.UpdateWerksterminData?.ReasonCapacityComments) == true)
			{
				errors.Add("Capacity reason cannot be empty");
			}

			if(_data.UpdateWerksterminData?.ReasonDefective == true && String.IsNullOrEmpty(_data.UpdateWerksterminData?.ReasonDefectiveComments) == true)
			{

				errors.Add("Defect reason cannot be empty");
			}

			if(_data.UpdateWerksterminData?.ReasonClarification == true && String.IsNullOrEmpty(_data.UpdateWerksterminData?.ReasonClarificationComments) == true)
			{
				errors.Add("Clarification reason cannot be empty");
			}
			if(errors != null && errors.Count > 0)
			{
				return ResponseModel<int>.FailureResponse(errors);
			}


			return ResponseModel<int>.SuccessResponse();
		}
		public ResponseModel<int> InsertWerksterminChecks(Infrastructure.Services.Utils.TransactionsManager botransaction)
		{
			UpdateWerksterminRequestModel data = _data.UpdateWerksterminData;
			// -
			var faEntity = FertigungAccess.GetByFertigungsnummer(data.FertigungNumber, botransaction.connection, botransaction.transaction);
			var articleByNumber = ArtikelAccess.GetWithTransaction((int)faEntity.Artikel_Nr, botransaction.connection, botransaction.transaction);
			var angeboteByNumber = AngeboteAccess.GetWithTransaction(faEntity.Angebot_nr ?? -1, botransaction.connection, botransaction.transaction);
			int result;
			try
			{
				/* Case where FA has Angebot_nr */
				if(angeboteByNumber != null)
				{
					var newInsideSalesWerksterminToInsert = new InsideSalesWerksterminUpdatesEntity
					{
						ArticleId = faEntity.Artikel_Nr,
						ArticleNumber = articleByNumber.ArtikelNummer,
						CustomerName = angeboteByNumber.Vorname_NameFirma,
						CustomerNumber = int.TryParse(angeboteByNumber.Unser_Zeichen, out var x) ? x : 0, // String.IsNullOrEmpty(angeboteByNumber.Projekt_Nr) == true ? 0 : Convert.ToInt32(angeboteByNumber.Projekt_Nr),
						CustomerOrderNumber=angeboteByNumber.Angebot_Nr,
						InsConfirmation = false,
						EditDate = DateTime.Now,
						EditUserId = _user.Id,
						EditUserName = _user.Username,
						FertigungId = faEntity.ID,
						FertigungNumber = faEntity.Fertigungsnummer,
						NewWorkDate = data.NewWorkDate,
						OldWorkDate = data.OldWorkDate,
						ReasonCapacity = data.ReasonCapacity,
						ReasonCapacityComments = data.ReasonCapacityComments,
						ReasonQuality = data.ReasonQuality,
						ReasonQualityComments = data.ReasonQualityComments,
						ReasonStatusP = data.ReasonStatusP,
						ReasonDefective = data.ReasonDefective,
						ReasonDefectiveComments = data.ReasonDefectiveComments,
						ReasonMaterial = data.ReasonMaterial,
						ReasonMaterialComments = data.ReasonMaterialComments
					};

					result = InsideSalesWerksterminUpdatesAccess.InsertWithTransaction(newInsideSalesWerksterminToInsert, botransaction.connection, botransaction.transaction);

					return ResponseModel<int>.SuccessResponse(result);
				}

				/* Case where FA has no Angebot_nr */
				else
				{
					var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(faEntity.Artikel_Nr ?? -1, botransaction.connection, botransaction.transaction);
					var artikelByNummerschlussel = PSZ_Nummerschlüssel_KundeAccess.GetArtikelByFertigungNummer(articleEntity?.ArtikelNummer?.Trim()?.Substring(0, Math.Min(3, (articleEntity?.ArtikelNummer?.Trim()?.Length ?? 0))), botransaction.connection, botransaction.transaction);

					if(artikelByNummerschlussel is not null)
					{
						var newInsideSalesWerksterminToInsert = new InsideSalesWerksterminUpdatesEntity
						{
							ArticleId = faEntity.Artikel_Nr,
							ArticleNumber = articleByNumber.ArtikelNummer,
							CustomerName = artikelByNummerschlussel.Kunde,
							CustomerNumber = artikelByNummerschlussel.Kundennummer,
							CustomerOrderNumber = 0,
							InsConfirmation = false,
							EditDate = DateTime.Now,
							EditUserId = _user.Id,
							EditUserName = _user.Username,
							FertigungId = faEntity.ID,
							FertigungNumber = faEntity.Fertigungsnummer,
							NewWorkDate = data.NewWorkDate,
							OldWorkDate = data.OldWorkDate,
							ReasonCapacity = data.ReasonCapacity,
							ReasonCapacityComments = data.ReasonCapacityComments,
							ReasonClarification = data.ReasonClarification,
							ReasonClarificationComments = data.ReasonClarificationComments,
							ReasonDefective = data.ReasonDefective,
							ReasonDefectiveComments = data.ReasonDefectiveComments,
							ReasonMaterial = data.ReasonMaterial,
							ReasonMaterialComments = data.ReasonMaterialComments,
							ReasonQuality = data.ReasonQuality,
							ReasonQualityComments = data.ReasonQualityComments,
							ReasonStatusP = data.ReasonStatusP
						};

						result = InsideSalesWerksterminUpdatesAccess.InsertWithTransaction(newInsideSalesWerksterminToInsert, botransaction.connection, botransaction.transaction);

						return ResponseModel<int>.SuccessResponse(result);
					}
				}

				return ResponseModel<int>.SuccessResponse(0);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

	}
}