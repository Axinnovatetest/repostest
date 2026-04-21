using Infrastructure.Services.Reporting.Models.CTS;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Helpers;
using Psz.Core.CRP.Models.FA.Update;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA.Purchase
{
	public class FAUpdateByArticle2Handler: IHandle<Identity.Models.UserModel, ResponseModel<FAUpdateByArticleFinalModel>>
	{
		private AllOpenFAForUpdateModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public FAUpdateByArticle2Handler(AllOpenFAForUpdateModel data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<FAUpdateByArticleFinalModel> Handle()
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var _logs = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();
				lock(Locks.Locks.FALock)
				{
					List<FAUpdateByArticleListModel> updated = new List<FAUpdateByArticleListModel>();
					FAUpdateByArticleFinalModel final = new FAUpdateByArticleFinalModel();
					var _toNotUpdate = new List<FANotUpdateByArticleListModel>();
					var FAVersionning = this._data.FAWithVersionning;
					var FANotVersionning = this._data.FAWithoutVersionning;
					//opening sql transaction
					botransaction.beginTransaction();
					if(FAVersionning != null && FAVersionning.Count > 0)
					{
						var updateS = FAVersionning.Where(x => x.UpdateS.HasValue && x.UpdateS.Value).ToList();
						if(updateS != null && updateS.Count > 0)
						{
							foreach(var item in updateS)
							{
								var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get(item.ID_Fer ?? -1);
								var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(item.Artikel_Nr ?? -1);
								//updating order item (if exsist) souilmi 22/06/2022
								if(faEntity.Angebot_Artikel_Nr.HasValue && faEntity.Angebot_Artikel_Nr.Value != 0)
								{
									var OrderItemEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(faEntity.Angebot_Artikel_Nr ?? -1);
									// if (OrderItemEntity != null && OrderItemEntity.Geliefert != 0)
									//{
									// - 2022-10-12 - ignore link to AB
									//var orderEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(OrderItemEntity.AngebotNr ?? -1);
									//_toNotUpdate.Add(new FANotUpdateByArticleListModel(faEntity.Fertigungsnummer ?? -1, $"The the linked AB [{orderEntity.Angebot_Nr}] has delivred quantity ."));
									//}
									//else
									//{
									var newPositions = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetByArticleAndVersion(item.Artikel_Nr ?? -1, this._data.BomVersion);
									//updating Ab pos index (if exsists)
									OrderItemEntity.Index_Kunde = this._data.Index;
									OrderItemEntity.Index_Kunde_Datum = newPositions != null && newPositions.Count > 0 ? newPositions[0].KundenIndexDate : articleEntity.Index_Kunde_Datum;
									Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateWithTransaction(OrderItemEntity, botransaction.connection, botransaction.transaction);
									//updating FA
									Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.DeleteByIdFertigungWithTransaction(item.ID_Fer ?? -1, botransaction.connection, botransaction.transaction);
									if(newPositions != null && newPositions.Count > 0)
									{
										var NewPositionsEntities = newPositions.Select(x => new Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity
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
									faEntity.Kunden_Index_Datum = newPositions != null && newPositions.Count > 0 ? newPositions[0].KundenIndexDate : articleEntity.Index_Kunde_Datum;
									faEntity.BomVersion = this._data.BomVersion;
									faEntity.CPVersion = this._data.CPVersion ?? null;
									var _oldFAEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get(item.ID_Fer ?? -1);
									Infrastructure.Data.Access.Tables.PRS.FertigungAccess.UpdateWithTransaction(faEntity, botransaction.connection, botransaction.transaction);
									updated.Add(new FAUpdateByArticleListModel(item.Fertigungsnummer ?? -1, item.Lagerort_id ?? -1));

									_logs.AddRange(GetLogs(_oldFAEntity, faEntity));
									//}

								}
								else
								{
									//updating FA
									Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.DeleteByIdFertigungWithTransaction(item.ID_Fer ?? -1, botransaction.connection, botransaction.transaction);
									var newPositions = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetByArticleAndVersion(item.Artikel_Nr ?? -1, this._data.BomVersion, botransaction.connection, botransaction.transaction);
									if(newPositions != null && newPositions.Count > 0)
									{
										var NewPositionsEntities = newPositions.Select(x => new Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity
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
									faEntity.Kunden_Index_Datum = newPositions != null && newPositions.Count > 0 ? newPositions[0].KundenIndexDate : articleEntity.Index_Kunde_Datum;
									faEntity.BomVersion = this._data.BomVersion;
									faEntity.CPVersion = this._data.CPVersion ?? null;
									var _oldFAEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetWithTransaction(item.ID_Fer ?? -1, botransaction.connection, botransaction.transaction);
									Infrastructure.Data.Access.Tables.PRS.FertigungAccess.UpdateWithTransaction(faEntity, botransaction.connection, botransaction.transaction);
									updated.Add(new FAUpdateByArticleListModel(item.Fertigungsnummer ?? -1, item.Lagerort_id ?? -1));

									_logs.AddRange(GetLogs(_oldFAEntity, faEntity));
								}
							}
						}
					}
					if(FANotVersionning != null && FANotVersionning.Count > 0)
					{
						var updateS = FANotVersionning.Where(x => x.UpdateS.HasValue && x.UpdateS.Value).ToList();
						if(updateS != null && updateS.Count > 0)
							foreach(var item in updateS)
							{
								var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get(item.ID_Fer ?? -1);
								var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(item.Artikel_Nr ?? -1);
								//updating order item (if exsist) souilmi 22/06/2022
								if(faEntity.Angebot_Artikel_Nr.HasValue && faEntity.Angebot_Artikel_Nr.Value != 0)
								{
									var OrderItemEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(faEntity.Angebot_Artikel_Nr ?? -1);
									//if (OrderItemEntity != null && OrderItemEntity.Geliefert != 0)
									//{
									// - 2022-10-12 - ignore link to AB
									//var orderEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(OrderItemEntity.AngebotNr ?? -1);
									//_toNotUpdate.Add(new FANotUpdateByArticleListModel(faEntity.Fertigungsnummer ?? -1, $"The linked AB [{orderEntity.Angebot_Nr}] has delivred quantity ."));
									//}
									//else
									//{
									//updating Ab pos index (if exsists)
									OrderItemEntity.Index_Kunde = articleEntity.Index_Kunde;
									OrderItemEntity.Index_Kunde_Datum = articleEntity.Index_Kunde_Datum;
									Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateWithTransaction(OrderItemEntity, botransaction.connection, botransaction.transaction);
									if(this._data.Stucklisten.HasValue && this._data.Stucklisten.Value)
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

										_logs.Add(new LogHelper(faEntity.Fertigungsnummer ?? -1, 0, 0, "Fertigung", LogHelper.LogType.MODIFICATIONSTUCKLIST, "CTS", _user)
										.LogCTS(null, null, null, 0));
									}
									var _oldFAEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get(item.ID_Fer ?? -1);
									if(this._data.KundenIndex.HasValue && this._data.KundenIndex.Value)
									{
										faEntity.KundenIndex = articleEntity.Index_Kunde;
										faEntity.Kunden_Index_Datum = articleEntity.Index_Kunde_Datum;
										Infrastructure.Data.Access.Tables.PRS.FertigungAccess.UpdateWithTransaction(faEntity, botransaction.connection, botransaction.transaction);
										_logs.AddRange(GetLogs(_oldFAEntity, faEntity));
									}
									updated.Add(new FAUpdateByArticleListModel(item.Fertigungsnummer ?? -1, item.Lagerort_id ?? -1));
									//}
								}
								else
								{
									if(this._data.Stucklisten.HasValue && this._data.Stucklisten.Value)
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

										_logs.Add(new LogHelper(faEntity.Fertigungsnummer ?? -1, 0, 0, "Fertigung", LogHelper.LogType.MODIFICATIONSTUCKLIST, "CTS", _user)
										.LogCTS(null, null, null, 0));
									}
									var _oldFAEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetWithTransaction(item.ID_Fer ?? -1, botransaction.connection, botransaction.transaction);
									if(this._data.KundenIndex.HasValue && this._data.KundenIndex.Value)
									{
										faEntity.KundenIndex = articleEntity.Index_Kunde;
										faEntity.Kunden_Index_Datum = articleEntity.Index_Kunde_Datum;
										Infrastructure.Data.Access.Tables.PRS.FertigungAccess.UpdateWithTransaction(faEntity, botransaction.connection, botransaction.transaction);
										_logs.AddRange(GetLogs(_oldFAEntity, faEntity));
									}
									updated.Add(new FAUpdateByArticleListModel(item.Fertigungsnummer ?? -1, item.Lagerort_id ?? -1));
								}

							}
					}
					// - 
					if(_logs != null && _logs.Count > 0)
						Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(_logs, botransaction.connection, botransaction.transaction);
					if(botransaction.commit())
					{
						final.Updated = updated;
						final.NotUpdated = _toNotUpdate;

						return ResponseModel<FAUpdateByArticleFinalModel>.SuccessResponse(final);
					}
					else
						return ResponseModel<FAUpdateByArticleFinalModel>.FailureResponse(key: "1", value: $"Transaction diden't commit");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<FAUpdateByArticleFinalModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<FAUpdateByArticleFinalModel>.AccessDeniedResponse();
			}
			if((this._data.FAWithVersionning == null || this._data.FAWithVersionning.Count == 0) &&
				(this._data.FAWithoutVersionning == null || this._data.FAWithoutVersionning.Count == 0)
				)
			{
				return ResponseModel<FAUpdateByArticleFinalModel>.FailureResponse(key: "1", value: $"Data empty No FA to update");
			}
			var FAVersionning = this._data.FAWithVersionning?.Where(x => x.UpdateS.HasValue && x.UpdateS.Value).ToList();
			var FANotVersionning = this._data.FAWithoutVersionning?.Where(x => x.UpdateS.HasValue && x.UpdateS.Value).ToList();
			if((FAVersionning == null || FAVersionning.Count == 0) && (FANotVersionning == null || FANotVersionning.Count == 0))
			{
				return ResponseModel<FAUpdateByArticleFinalModel>.FailureResponse(key: "1", value: $"No FA to update");
			}
			if(FAVersionning != null && FAVersionning.Count > 0)
			{
				if(string.IsNullOrEmpty(this._data.Index) || string.IsNullOrWhiteSpace(this._data.Index))
					return ResponseModel<FAUpdateByArticleFinalModel>.FailureResponse(key: "1", value: $"Please select index");
				if(!this._data.BomVersion.HasValue || (this._data.BomVersion.HasValue && (string.IsNullOrEmpty(this._data.BomVersion.ToString()) || string.IsNullOrWhiteSpace(this._data.BomVersion.ToString()))))
					return ResponseModel<FAUpdateByArticleFinalModel>.FailureResponse(key: "2", value: $"Please select bom version");
			}
			if(FANotVersionning != null && FANotVersionning.Count > 0)
			{
				if((!this._data.Stucklisten.HasValue && !this._data.KundenIndex.Value) ||
					(this._data.Stucklisten.HasValue && !this._data.Stucklisten.Value && this._data.KundenIndex.HasValue && !this._data.KundenIndex.Value))
				{
					return ResponseModel<FAUpdateByArticleFinalModel>.FailureResponse(key: "3", value: $"Please select at least one option");
				}
			}
			return ResponseModel<FAUpdateByArticleFinalModel>.SuccessResponse();
		}
		public List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity> GetLogs(Infrastructure.Data.Entities.Tables.PRS.FertigungEntity _old,
		  Infrastructure.Data.Entities.Tables.PRS.FertigungEntity _new)
		{
			var _logs = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();
			var LOG = new LogHelper(_old.Fertigungsnummer ?? -1, 0, 0, "Fertigung", LogHelper.LogType.MODIFICATIONOBJECT, "CTS", _user);
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