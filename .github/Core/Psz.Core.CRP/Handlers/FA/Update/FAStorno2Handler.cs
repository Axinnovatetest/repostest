using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.SharedKernel.Interfaces;


namespace Psz.Core.CRP.Handlers.FA.Update
{
	public class FAStorno2Handler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private FAStornoModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public FAStorno2Handler(FAStornoModel data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				lock(Locks.Locks.FACreateLock.GetOrAdd(this._data.fa, new object()))
				{
					var validationResponse = this.Validate();
					if(!validationResponse.Success)
					{
						return validationResponse;
					}

					//opening sql transaction
					botransaction.beginTransaction();

					var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetWithTransaction(this._data.FaId, botransaction.connection, botransaction.transaction);
					var positionIds = (Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.GetByIdFertigung(faEntity.ID, botransaction.connection, botransaction.transaction) ??
						new List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity>()).Select(x => x.ID).ToList();
					var lagers = Module.BSD.ProductionLagerIds; // new List<int> { 42, 60, 7, 6, 21, 26 };
					if(lagers.Contains((int)faEntity.Lagerort_id))
					{
						if(faEntity.FA_Gestartet.HasValue && faEntity.FA_Gestartet.Value)
							return ResponseModel<int>.FailureResponse($"Fertigungsauftrag ist Bereit Gestartet und kann nicht storniert werden.");
					}
					if(faEntity.Angebot_nr.HasValue && faEntity.Angebot_nr.Value != 0)
						return ResponseModel<int>.FailureResponse($"Auftrag muss aus AB storniert werden!");

					if(faEntity.Kennzeichen.ToLower() == "erledigt")
						return ResponseModel<int>.FailureResponse($"Auftrag ist bereits erledigt!");
					if(faEntity.Kennzeichen.ToLower() == "storno")
						return ResponseModel<int>.FailureResponse($"Auftrag ist bereits storniert!");


					// - 2022-11-23 - cancel UBG Fas option
					var errors = new List<string>();
					if(this._data.CancelUBGFas == true)
					{
						var ubgFas = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetNonStartedByHBGFaPositionId(positionIds, botransaction.connection, botransaction.transaction)
						?? new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
						foreach(var ubgFaItem in ubgFas)
						{
							cancelFa(errors, ubgFaItem.Fertigungsnummer ?? -1, $"{this._data.grund} - von HBG {this._data.fa}", this._user, botransaction);
						}
					}
					if(errors != null && errors.Count > 0)
					{
						return ResponseModel<int>.FailureResponse(errors);
					}

					var actualBemerkung = faEntity.Bemerkung;
					faEntity.Kennzeichen = "STORNO";
					faEntity.Bemerkung = $"STORNO am {DateTime.Now} GRUND: {this._data.grund}/{actualBemerkung}";
					faEntity.Angebot_nr = 0;
					faEntity.Angebot_Artikel_Nr = 0;
					faEntity.Anzahl = 0;
					// - 2022-11-17 - break link to HBG FA Position
					if(faEntity.UBG == true)
					{
						// - 
						var hbgPositionEntity = Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.GetWithTransaction(faEntity.HBGFAPositionId ?? -1, botransaction.connection, botransaction.transaction);
						if(hbgPositionEntity != null)
						{
							Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.UpdateUBGIdWithTransaction(hbgPositionEntity.ID, null, null, botransaction.connection, botransaction.transaction);
						}
						// -
						faEntity.HBGFAPositionId = null;
					}


					var response = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.UpdateWithTransaction(faEntity, botransaction.connection, botransaction.transaction);
					Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.DeleteByIdFertigungWithTransaction(faEntity.ID, botransaction.connection, botransaction.transaction);

					// - 2022-11-17 - logs
					var _log = new Helpers.LogHelper((int)faEntity.Fertigungsnummer, 0, 0, "Fertigung", Helpers.LogHelper.LogType.DELETIONOBJECT, "CTS", _user)
						.LogCTS(null, null, null, 0);
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(_log, botransaction.connection, botransaction.transaction);

					if(botransaction.commit())
					{
						Helpers.SpecialHelper.UpdateFACapacity(faEntity, _user.Id);
						return ResponseModel<int>.SuccessResponse(response);
					}
					else
						return ResponseModel<int>.FailureResponse($"Transaction did not commit");
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
			if(string.IsNullOrEmpty(this._data.grund) || string.IsNullOrWhiteSpace(this._data.grund))
				return ResponseModel<int>.FailureResponse($"Please fill the reason for annulation");
			var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get(this._data.FaId);
			if(faEntity == null)
				return ResponseModel<int>.FailureResponse($"FA does not exist");

			// 2024-01-25 - Khelil change H1 to 41 days
			var frZone = DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays);
			if(this._user?.Access?.CustomerService?.FAWerkWunshAdmin != true && this._user.Access?.CustomerService?.FaAdmin != true && faEntity.Lagerort_id != 6 && faEntity.Technik != true && !Module.BSD.TechnicArticleIds.Exists(x => x == faEntity.Artikel_Nr))
			{
				var _newDate = faEntity.Termin_Bestatigt1 ?? new DateTime(1900, 1, 1);
				if(_newDate < DateTime.Today)
				{
					return ResponseModel<int>.FailureResponse($"Production date invalid: can not cancel FA [{_newDate.ToString("dd/MM/yyyy")}] in the past.");
				}

				if(_newDate <= frZone)
				{
					return ResponseModel<int>.FailureResponse($"Production date invalid: can not cancel FA in Frozen Zone limit [{frZone.ToString("dd/MM/yyyy")}].");
				}
			}
			//var technicArticles = Module.BSD.TechnicArticleIds;
			var horizonCheck = Helpers.HorizonsHelper.userHasFaCancelHorizonRight(faEntity.Termin_Bestatigt1 ?? new DateTime(1900, 1, 1), _user, out List<string> messages);
			if(!horizonCheck && !Helpers.HorizonsHelper.ArticleIsTechnic(faEntity.Artikel_Nr ?? -1))
				return ResponseModel<int>.FailureResponse(messages);

			return ResponseModel<int>.SuccessResponse(1);
		}
		public static void cancelFa(List<string> errors, int ubgFaFertigungsnummer, string reason, Identity.Models.UserModel user, Infrastructure.Services.Utils.TransactionsManager botransaction)
		{
			var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(ubgFaFertigungsnummer, botransaction.connection, botransaction.transaction);
			if(faEntity == null)
				errors.Add($"UGB FA does not exsist");

			var positionIds = (Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.GetByIdFertigung(faEntity.ID, botransaction.connection, botransaction.transaction) ??
				new List<Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity>()).Select(x => x.ID).ToList();
			var lagers = Module.BSD.ProductionLagerIds;// new List<int> { 42, 60, 7, 6, 21, 26 };

			if(lagers.Contains((int)faEntity.Lagerort_id))
			{
				if(faEntity.FA_Gestartet.HasValue && faEntity.FA_Gestartet.Value)
					errors.Add($"UGB FA {faEntity.Fertigungsnummer}: Fertigungsauftrag ist Bereit Gestartet und kann nicht storniert werden.");
			}
			if(faEntity.Angebot_nr.HasValue && faEntity.Angebot_nr.Value != 0)
				errors.Add($"UGB FA {faEntity.Fertigungsnummer}: Auftrag muss aus AB storniert werden!");

			if(faEntity.Kennzeichen.ToLower() == "erledigt")
				errors.Add($"UGB FA {faEntity.Fertigungsnummer}: Auftrag ist bereits erledigt!");
			if(faEntity.Kennzeichen.ToLower() == "storno")
				errors.Add($"UGB FA {faEntity.Fertigungsnummer}: Auftrag ist bereits storniert!");

			// - 2023-02-03 // 2024-01-25 - Khelil change H1 to 41 days
			var frZone = DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays);
			if(user?.Access?.CustomerService?.FAWerkWunshAdmin != true && user.Access?.CustomerService?.FaAdmin != true && faEntity.Lagerort_id != 6 && faEntity.Technik != true && !Module.BSD.TechnicArticleIds.Exists(x => x == faEntity.Artikel_Nr))
			{
				var _newDate = faEntity.Termin_Bestatigt1 ?? new DateTime(1900, 1, 1);
				if(_newDate < DateTime.Today)
				{
					errors.Add($"Production date invalid: can not cancel FA [{_newDate.ToString("dd/MM/yyyy")}] in the past.");
				}

				if(_newDate <= frZone)
				{
					errors.Add($"Production date invalid: can not cancel FA in Frozen Zone limit [{frZone.ToString("dd/MM/yyyy")}].");
				}
			}

			if(errors != null && errors.Count > 0)
			{
				return;
			}

			var actualBemerkung = faEntity.Bemerkung;
			faEntity.Kennzeichen = "STORNO";
			faEntity.Bemerkung = $"STORNO am {DateTime.Now} GRUND: {reason}/{actualBemerkung}";
			faEntity.Angebot_nr = 0;
			faEntity.Angebot_Artikel_Nr = 0;
			faEntity.Anzahl = 0;
			// - 2022-11-17 - break link to HBG FA Position
			if(faEntity.UBG == true)
			{
				// - 
				var hbgPositionEntity = Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.GetWithTransaction(faEntity.HBGFAPositionId ?? -1, botransaction.connection, botransaction.transaction);
				if(hbgPositionEntity != null)
				{
					Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.UpdateUBGIdWithTransaction(hbgPositionEntity.ID, null, null, botransaction.connection, botransaction.transaction);
				}
				// -
				faEntity.HBGFAPositionId = null;
			}
			var response = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.UpdateWithTransaction(faEntity, botransaction.connection, botransaction.transaction);
			Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.DeleteByIdFertigungWithTransaction(faEntity.ID, botransaction.connection, botransaction.transaction);

			// - 2022-11-17 - logs
			var _log = new Helpers.LogHelper((int)faEntity.Fertigungsnummer, 0, 0, "Fertigung", Helpers.LogHelper.LogType.DELETIONOBJECT, "CTS", user)
				.LogCTS(null, null, null, 0);
			Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(_log, botransaction.connection, botransaction.transaction);

		}
	}
}