using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA.Update
{
	public class FAUpdate2Handler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private FAUpdateModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public FAUpdate2Handler(FAUpdateModel data, Identity.Models.UserModel user)
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
				var _oldFAEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data.Fertigungsnummer);
				lock(Locks.Locks.FACreateLock.GetOrAdd(this._data.Fertigungsnummer, new object()))
				{
					int response = 0;
					var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data.Fertigungsnummer);
					var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(faEntity.Artikel_Nr ?? -1);
					var LagerWithVersionning = Module.LagersWithVersionning;
					var errors = new List<string>();
					//opening sql transaction
					botransaction.beginTransaction();
					if(LagerWithVersionning.Contains((int)faEntity.Lagerort_id))
					{
						//updating FA
						Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.DeleteByIdFertigungWithTransaction(faEntity.ID, botransaction.connection, botransaction.transaction);
						var stcuklistSnapshotEntities = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetByArticleAndVersion((int)faEntity.Artikel_Nr, this._data.BOM_version);
						if(stcuklistSnapshotEntities != null && stcuklistSnapshotEntities.Count > 0)
						{
							var NewPositionsEntities = stcuklistSnapshotEntities.Select(x => new Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity
							{
								ID_Fertigung_HL = faEntity.ID,
								ID_Fertigung = faEntity.ID,
								Artikel_Nr = x.Artikel_Nr_des_Bauteils,
								Anzahl = faEntity.Anzahl * x.Anzahl,
								Lagerort_ID = faEntity.Lagerort_id,
								Buchen = true,
								Vorgang_Nr = x.Vorgang_Nr,
								ME_gebucht = false,
							}).ToList();
							Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.InsertWithTransaction(NewPositionsEntities, botransaction.connection, botransaction.transaction);
						}
						faEntity.KundenIndex = this._data.Index;
						faEntity.Kunden_Index_Datum = stcuklistSnapshotEntities != null && stcuklistSnapshotEntities.Count > 0 ? stcuklistSnapshotEntities[0].KundenIndexDate : articleEntity.Index_Kunde_Datum;
						faEntity.BomVersion = this._data.BOM_version;
						faEntity.CPVersion = this._data.CP_version ?? null;
						response = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.UpdateWithTransaction(faEntity, botransaction.connection, botransaction.transaction);
						//updating AB pos Index (if exsists)
						if(faEntity.Angebot_Artikel_Nr.HasValue && faEntity.Angebot_Artikel_Nr.Value != 0)
						{
							var OrderItemEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(faEntity.Angebot_Artikel_Nr ?? -1);
							OrderItemEntity.Index_Kunde = this._data.Index;
							OrderItemEntity.Index_Kunde_Datum = stcuklistSnapshotEntities != null && stcuklistSnapshotEntities.Count > 0 ? stcuklistSnapshotEntities[0].KundenIndexDate : articleEntity.Index_Kunde_Datum;
							Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateWithTransaction(OrderItemEntity, botransaction.connection, botransaction.transaction);
						}
					}
					else
					{
						//updating FA
						if(this._data.kundenIndex.HasValue && this._data.kundenIndex.Value)
						{
							faEntity.KundenIndex = articleEntity.Index_Kunde;
							faEntity.Kunden_Index_Datum = articleEntity.Index_Kunde_Datum;
							response = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.UpdateWithTransaction(faEntity, botransaction.connection, botransaction.transaction);
						}
						if(this._data.stucklisten.HasValue && this._data.stucklisten.Value)
						{
							Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.DeleteByIdFertigungWithTransaction(faEntity.ID, botransaction.connection, botransaction.transaction);
							var stucklistenEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(faEntity.Artikel_Nr ?? -1);
							if(stucklistenEntities != null && stucklistenEntities.Count > 0)
							{
								var NewPositionsEntities = stucklistenEntities.Select(x => new Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity
								{
									ID_Fertigung_HL = faEntity.ID,
									ID_Fertigung = faEntity.ID,
									Artikel_Nr = x.Artikel_Nr_des_Bauteils,
									Anzahl = faEntity.Anzahl * x.Anzahl,
									Lagerort_ID = faEntity.Lagerort_id,
									Buchen = true,
									Vorgang_Nr = x.Vorgang_Nr,
									ME_gebucht = false,
								}).ToList();
								Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.InsertWithTransaction(NewPositionsEntities, botransaction.connection, botransaction.transaction);
							}
						}
						//updating AB pos Index (if exsists)
						if(faEntity.Angebot_Artikel_Nr.HasValue && faEntity.Angebot_Artikel_Nr.Value != 0)
						{
							var OrderItemEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(faEntity.Angebot_Artikel_Nr ?? -1);
							OrderItemEntity.Index_Kunde = articleEntity.Index_Kunde;
							OrderItemEntity.Index_Kunde_Datum = articleEntity.Index_Kunde_Datum;
							Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateWithTransaction(OrderItemEntity, botransaction.connection, botransaction.transaction);
						}
					}
					//commiting transaction
					if(botransaction.commit())
					{
						//logging
						var _newFAEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data.Fertigungsnummer);
						if(LagerWithVersionning.Contains((int)faEntity.Lagerort_id))
						{
							var _logs = GetLogs(_oldFAEntity, _newFAEntity);
							if(_logs != null && _logs.Count > 0)
								Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_logs);
						}
						else
						{
							var _logs = GetLogs(_oldFAEntity, _newFAEntity);
							if(this._data.stucklisten.HasValue && this._data.stucklisten.Value)
								_logs.Add(new Helpers.LogHelper((int)_newFAEntity.Fertigungsnummer, 0, 0, "Fertigung", Helpers.LogHelper.LogType.MODIFICATIONSTUCKLIST, "CTS", _user)
							   .LogCTS(null, null, null, 0));
							Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_logs);
						}
						return ResponseModel<int>.SuccessResponse(1);
					}
					else
						return ResponseModel<int>.FailureResponse(key: "1", value: $"Transaction diden't commit");
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
			var LagerWithVersionning = Module.LagersWithVersionning;
			var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(this._data.Fertigungsnummer);
			if(faEntity.FA_Gestartet.HasValue && faEntity.FA_Gestartet.Value && faEntity.Lagerort_id != 15)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"FA Gestartet update not possible");

			if(faEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"FA not found");

			if(LagerWithVersionning.Contains((int)faEntity.Lagerort_id))
			{
				if(string.IsNullOrEmpty(this._data.Index) || string.IsNullOrWhiteSpace(this._data.Index))
					return ResponseModel<int>.FailureResponse(key: "1", value: $"Bitte Index auswählen");
				if(!this._data.BOM_version.HasValue || (this._data.BOM_version.HasValue && (string.IsNullOrEmpty(this._data.BOM_version.Value.ToString()) || string.IsNullOrWhiteSpace(this._data.BOM_version.Value.ToString()))))
					return ResponseModel<int>.FailureResponse(key: "1", value: $"Bitte BOM Version auswählen");
			}
			else
			{
				if(faEntity.Kennzeichen.ToLower() != "offen")
					return ResponseModel<int>.FailureResponse(key: "1", value: $"Aktualisierung nicht möglich; Auftrag nicht offen");
				if(faEntity.Anzahl != faEntity.Originalanzahl)
					return ResponseModel<int>.FailureResponse(key: "1", value: $"Aktualisierung nicht möglich; FA teilweise erledigt");
				if(faEntity.Kennzeichen == "erledigt")
					return ResponseModel<int>.FailureResponse(key: "1", value: $"Fa ist bereits erledigt!");
				if(this._data.stucklisten.HasValue && this._data.kundenIndex.HasValue && !this._data.stucklisten.Value && !this._data.kundenIndex.Value)
					return ResponseModel<int>.FailureResponse(key: "1", value: $"at least One value should be checked");
			}

			// - 2022-10-12 - ignore AB
			//if (faEntity.Angebot_Artikel_Nr.HasValue && faEntity.Angebot_Artikel_Nr.Value != 0)
			//{
			//    var OrderItemEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(faEntity.Angebot_Artikel_Nr ?? -1);
			//    var orderEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(OrderItemEntity?.AngebotNr ?? -1);
			//    if (OrderItemEntity != null && OrderItemEntity.Geliefert != 0)
			//        return ResponseModel<int>.FailureResponse($"FA could not be updated because the the linked AB [{orderEntity.Angebot_Nr}] has delivred quantity .");
			//}
			return ResponseModel<int>.SuccessResponse();
		}
		public List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity> GetLogs(Infrastructure.Data.Entities.Tables.PRS.FertigungEntity _old,
			Infrastructure.Data.Entities.Tables.PRS.FertigungEntity _new)
		{
			var _logs = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();
			var LOG = new Helpers.LogHelper((int)_old.Fertigungsnummer, 0, 0, "Fertigung", Helpers.LogHelper.LogType.MODIFICATIONOBJECT, "CTS", _user);
			if(_old.KundenIndex != _new.KundenIndex)
				_logs.Add(LOG.LogCTS("KundenIndex", _old.KundenIndex, _new.KundenIndex, 0));
			if(_old.BomVersion != _new.BomVersion)
				_logs.Add(LOG.LogCTS("BomVersion", _old.BomVersion.ToString(), _new.BomVersion.ToString(), 0));
			if(_old.CPVersion != _new.CPVersion)
				_logs.Add(LOG.LogCTS("CPVersion", _old.CPVersion.ToString(), _new.CPVersion.ToString(), 0));
			return _logs;
		}
	}
}