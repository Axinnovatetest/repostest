using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.SharedKernel.Interfaces;


namespace Psz.Core.CRP.Handlers.FA
{
	public class FAErlidigt2Handler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private FAErlidigtEntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public FAErlidigt2Handler(FAErlidigtEntryModel data, Identity.Models.UserModel user)
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
				lock(Locks.Locks.FACreateLock.GetOrAdd((int)this._data.Id, new object()))
				{
					var lagers = Enum.GetValues(typeof(Enums.FAEnums.FaLands)).Cast<Enums.FAEnums.FaLands>().Select(x => (int)x).ToList();/// new List<int> { 42, 60, 7, 6, 21, 26 };
					var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get(this._data.Id);
					int warehouse_reparatur = 0;
					int warehouseLand = 0;
					int ursArtikelNr = 0;
					switch(faEntity.Lagerort_id)
					{
						case 102:
							warehouse_reparatur = 106;
							break;
						case 7:
							warehouse_reparatur = 19;
							break;
						case 60:
							warehouse_reparatur = 62;
							break;
						case 42:
							warehouse_reparatur = 48;
							break;
						case 6:
							warehouse_reparatur = 18;
							break;
						case 26:
							warehouse_reparatur = 28;
							break;
						case 15:
							warehouse_reparatur = 13;
							break;
					}
					switch(faEntity.Lagerort_id)
					{
						case 102:
							warehouseLand = 103;
							break;
						case 60:
							warehouseLand = 580;
							break;
						case 42:
						case 7:
							warehouseLand = 420;
							break;
						//case 7:
						//	warehouseLand = 77;
						//break;
						case 6:
							warehouseLand = 66;
							break;
						case 26:
							warehouseLand = 260;
							break;
						default:
							warehouseLand = faEntity.Lagerort_id ?? 0;
							break;
					}
					if(faEntity.Artikel_Nr == 3604)
					{
						var ursArtikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(faEntity.Urs_Artikelnummer);
						ursArtikelNr = ursArtikelEntity?.ArtikelNr ?? -1;
					}
					var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(faEntity?.Artikel_Nr ?? -1);
					var data = this._data;

					// - 2023-04-05 FA will close auto when full Quantity is reached
					//if(this._data.Erledigt && faEntity.Anzahl > _data.Quantity)
					//	return ResponseModel<int>.FailureResponse("FA Open quantity not reached, Closing impossible.");

					var faErledegenEntity = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetFAErlidgit(faEntity?.ID ?? -1, this._user.Username);
					var faErledegenModel = new FAErlidigtModel(faErledegenEntity);
					var faErledegenLogEntity = faErledegenModel.ToFAErledigenEntity();

					var lagerExtenionEntities = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndex(articleEntity.ArtikelNr, faEntity.KundenIndex);

					//opening sql transaction
					botransaction.beginTransaction();

					var insertedID = Infrastructure.Data.Access.Tables.CTS.PSZ_FA_erledigen_HilfstabelleAccess.InsertWithTransaction(faErledegenLogEntity, botransaction.connection, botransaction.transaction);
					var FertingungPositionEntities = Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.GetByIdFertigung(faEntity.ID);
					if(FertingungPositionEntities != null && FertingungPositionEntities.Count > 0)
					{
						var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(FertingungPositionEntities.Select(x => (int)x.Artikel_Nr).ToList());
						//insert 1 ROH
						var fertigungsVorgangEntites_1 = FertingungPositionEntities.Select(x => new Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity
						{
							Personal_Nr = 0,
							Fertigung_Nr = x.ID_Fertigung,
							Datum = faErledegenEntity.Termin_Fertigstellung,
							Artikel_nr = articleEntities.Find(y => y.ArtikelNr == x.Artikel_Nr).ArtikelNr,
							Anzahl = -1 * ((float)x.Anzahl / (float)faEntity.Originalanzahl * (float)_data.Quantity),
							Lagerort_id = warehouseLand,//faErledegenEntity.Lagerort_id_Entnahme,
							ab_buchen = true,
							Mitarbeiter = faErledegenEntity.Mitarbeiter,
							Vorgang = insertedID
						}).ToList();
						Infrastructure.Data.Access.Tables.CTS.Fertigung_FertigungsvorgangAccess.InsertWithTransaction(fertigungsVorgangEntites_1, botransaction.connection, botransaction.transaction);                        //var fertigungsVorgangEntites_2 = FertingungPositionEntities.Select(x => new Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity

					}
					//insert 2 FG --souilmi 26/05/2022
					Infrastructure.Data.Access.Tables.CTS.Fertigung_FertigungsvorgangAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.CTS.Fertigung_FertigungsvorgangEntity
					{
						Fertigung_Nr = faEntity.ID,
						Artikel_nr = articleEntity.ArtikelNr,
						Anzahl = _data.Quantity,
						Lagerort_id = faEntity.Lagerort_id,
						Mitarbeiter = $"{_user.Name} {DateTime.Now}",
						Personal_Nr = 0,
						ab_buchen = false,
						Datum = DateTime.Now,
						Vorgang = insertedID
					}, botransaction.connection, botransaction.transaction);
					var lagerBewegungEntity = new Infrastructure.Data.Entities.Tables.CTS.LagerbewegungenEntity
					{
						Typ = "Entnahme",
						Datum = faErledegenEntity.Termin_Fertigstellung,
						gebucht = true,
						angebot_nr = insertedID,
					};
					var insertlagerBewegungID = Infrastructure.Data.Access.Tables.CTS.LagerbewegungenAccess.InsertWithTransaction(lagerBewegungEntity, botransaction.connection, botransaction.transaction);


					var lagerEntities = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetByArticleNrs(new List<int> { articleEntity.ArtikelNr });
					var lagerEntity = lagerEntities?.Find(x => x.Lagerort_id == faErledegenEntity.Lagerort_id);
					lagerEntity.Bestand += _data.Quantity;
					Infrastructure.Data.Access.Tables.PRS.LagerAccess.UpdateWithTransaction(lagerEntity, botransaction.connection, botransaction.transaction);

					// - 2022-03-11 LagerExtension to track KI changes w Bestand
					Core.CRP.Helpers.ItemElementHelper.UpdateLagerExtension(_user,
						new Core.CRP.Helpers.ItemElementHelper.UpdateLagerExtensionModel
						{
							ArticleId = articleEntity.ArtikelNr,
							OldKundenIndex = faEntity.KundenIndex,
							NewKundenIndex = faEntity.KundenIndex,
							NewLagerorId = faEntity.Lagerort_id ?? -1,
							OldLagerorId = faEntity.Lagerort_id ?? -1,
							OldBestand = _data.Quantity,
							NewBestand = 0m,
						}, botransaction);
					//
					var fertigungVorganbEntities = Infrastructure.Data.Access.Tables.CTS.Fertigung_FertigungsvorgangAccess.GetByVorgangNr(insertedID);
					if(fertigungVorganbEntities != null && fertigungVorganbEntities.Count > 0)
					{
						var vorganglagerEntities = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetByArticleNrs(fertigungVorganbEntities.Select(x => x.Artikel_nr ?? -1)?.ToList());
						var lagerVorgExtenionEntities = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleIdAndKundenIndex(fertigungVorganbEntities.Select(x => new KeyValuePair<int, string>(x.Artikel_nr ?? -1, faEntity.KundenIndex))?.ToList());
						// -
						var toUpdateLagers = new List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity>();
						var toUpdateLagersExt = new List<Core.CRP.Helpers.ItemElementHelper.UpdateLagerExtensionModel>();

						foreach(var item in fertigungVorganbEntities)
						{
							var vorganglagerEntity = vorganglagerEntities?.Find(x => x.Lagerort_id == item.Lagerort_id);
							if(vorganglagerEntity != null)
							{
								vorganglagerEntity.Bestand = (Decimal)item.Anzahl;
								toUpdateLagers.Add(vorganglagerEntity);
							}
						}
						// - refactor
						if(toUpdateLagers.Count > 0)
							Infrastructure.Data.Access.Tables.PRS.LagerAccess.UpdateWithTransaction(toUpdateLagers, botransaction.connection, botransaction.transaction);
						// - 2022-03-11 track KundenIndex for Lager Bestand
						if(toUpdateLagersExt.Count > 0)
							Core.CRP.Helpers.ItemElementHelper.UpdateLagerExtension(_user, toUpdateLagersExt, botransaction);
					}


					//
					if(lagers.Contains((int)faEntity.Lagerort_id))
					{
						Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.UpdateBestandType1WithTransaction((int)faEntity.Fertigungsnummer, (Decimal)_data.Quantity, warehouseLand, botransaction.connection, botransaction.transaction);
						Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.UpdateBestandType2WithTransaction((int)faEntity.Fertigungsnummer, (Decimal)_data.Quantity, warehouseLand, botransaction.connection, botransaction.transaction);
					}
					else
						Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.UpdateBestandQtyTransaction((int)faEntity.Lagerort_id, (int)faEntity.Fertigungsnummer, (double)_data.Quantity, botransaction.connection, botransaction.transaction);

					//reparatur concept
					if(faEntity.Artikel_Nr == 3604)
					{
						var lagerEntities_3604 = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetByArticleNrs(new List<int> { ursArtikelNr });
						var lagerEntity_3604 = lagerEntities_3604?.Find(x => x.Lagerort_id == warehouse_reparatur);
						lagerEntity.Bestand -= _data.Quantity;
						Infrastructure.Data.Access.Tables.PRS.LagerAccess.UpdateWithTransaction(lagerEntity_3604, botransaction.connection, botransaction.transaction);


						// - 2022-03-11 track KundenIndex for Lager Bestand
						Core.CRP.Helpers.ItemElementHelper.UpdateLagerExtension(this._user,
							new Core.CRP.Helpers.ItemElementHelper.UpdateLagerExtensionModel
							{
								ArticleId = ursArtikelNr,
								OldKundenIndex = faEntity.KundenIndex,
								NewKundenIndex = faEntity.KundenIndex,
								OldLagerorId = warehouse_reparatur,
								NewLagerorId = warehouse_reparatur,
								OldBestand = 0m,
								NewBestand = _data.Quantity
							}, botransaction);

					}
					faEntity.Anzahl_erledigt += _data.Quantity;
					faEntity.Anzahl = faEntity.Originalanzahl - faEntity.Anzahl_erledigt;
					faEntity.Kennzeichen = faEntity.Anzahl > 0 ? "offen" : "erledigt";
					faEntity.Erledigte_FA_Datum = faErledegenEntity.Termin_Fertigstellung;
					faEntity.Tage_Abweichung = faErledegenEntity.Termin_Fertigstellung.Value.Subtract(faEntity.Termin_Bestatigt1.Value).Days;
					faEntity.Letzte_Gebuchte_Menge = _data.Quantity; // - 2024-01-09 - KH
					faEntity.Bemerkung_Planung += $"/ Buchung: {_data.Quantity} Stk. durch {faErledegenEntity.Mitarbeiter} mit Status: {faEntity.Kennzeichen}";
					Infrastructure.Data.Access.Tables.PRS.FertigungAccess.UpdateWithTransaction(faEntity, botransaction.connection, botransaction.transaction);

					// - 2023-10-26 - Remove MHD check, on complet closing /*openQty<=0*/
					if(faEntity.Anzahl <= 0)
					{
						resetMHD(faEntity.ID, faEntity.Lagerort_id ?? -1, botransaction);
					}
					// - 2023-11-27
					var _log = new Helpers.LogHelper(faEntity.Fertigungsnummer ?? 0, 0, 0, "Fertigung", Helpers.LogHelper.LogType.MODIFICATIONFA, "CTS", _user)
						.LogCTS($"Anzahl Erledigt", $"{faEntity.Anzahl_erledigt - _data.Quantity}", $"{faEntity.Anzahl_erledigt}", 0);
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(_log, botransaction.connection, botransaction.transaction);

					// -
					if(botransaction.commit())
					{
						Helpers.SpecialHelper.UpdateFACapacity(faEntity, _user.Id);
						return ResponseModel<int>.SuccessResponse(1);
					}
					else
						return ResponseModel<int>.FailureResponse(key: "1", value: $"Transaction did not commit");
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
			var faEntity = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get(this._data.Id);
			if(faEntity == null)
				return ResponseModel<int>.FailureResponse("FA not found");
			if(_data.Quantity <= 0 || _data.Quantity > faEntity.Anzahl)
				return ResponseModel<int>.FailureResponse("Unvalid quantity.");
			if(faEntity.Lagerort_id == 156)
				return ResponseModel<int>.FailureResponse("Closing for warehouse 156 is currently blocked, check with IT service .");
			if(this._data.Quantity > faEntity.Anzahl)
				return ResponseModel<int>.FailureResponse("Anzahl zu groß");
			if((!faEntity.FA_Gestartet.HasValue || (faEntity.FA_Gestartet.HasValue && !faEntity.FA_Gestartet.Value)) && faEntity.Lagerort_id != 15 && faEntity.Lagerort_id != 156)
				return ResponseModel<int>.FailureResponse("Fertigungsauftrag ist nicht Gestartet");
			if(faEntity.Kennzeichen.ToLower() == "storno")
				return ResponseModel<int>.FailureResponse("FA ist storniert!");
			if(faEntity.Kennzeichen.ToLower() == "erledigt")
				return ResponseModel<int>.FailureResponse("FA ist bereits erledigt!");
			if(faEntity.Kennzeichen.ToLower() == "gesperrt")
				return ResponseModel<int>.FailureResponse("FA ist noch nicht gebucht!");
			return ResponseModel<int>.SuccessResponse();
		}
		internal void resetMHD(int faId, int faLagerort, Infrastructure.Services.Utils.TransactionsManager botransaction)
		{
			var umbuchungHistorieEntities = Infrastructure.Data.Access.Tables.CTS.Tbl_umbuchung_historienAccess.GetByFertigung(faId, botransaction.connection, botransaction.transaction);
			if(umbuchungHistorieEntities?.Count > 0)
			{
				var mhdIds = new List<int>();
				mhdIds.AddRange(umbuchungHistorieEntities.Select(x => x.IDMHD1 ?? -1));
				mhdIds.AddRange(umbuchungHistorieEntities.Select(x => x.IDMHD2 ?? -1));
				mhdIds.AddRange(umbuchungHistorieEntities.Select(x => x.IDMHD3 ?? -1));
				mhdIds.AddRange(umbuchungHistorieEntities.Select(x => x.IDMHD4 ?? -1));
				mhdIds.AddRange(umbuchungHistorieEntities.Select(x => x.IDMHD5 ?? -1));
				mhdIds.AddRange(umbuchungHistorieEntities.Select(x => x.IDMHD6 ?? -1));
				// - 
				mhdIds = mhdIds.Where(x => x != -1)?.Distinct()?.ToList();
				// - remove odd count number
				var idToRemove = new List<int>();
				for(int i = 0; i < mhdIds.Count; i++)
				{
					if(mhdIds.Count(x => x == mhdIds[i]) % 2 == 0)
					{
						idToRemove.Add(mhdIds[i]);
					}
				}
				if(idToRemove?.Count > 0)
				{
					mhdIds = mhdIds.Where(x => !idToRemove.Exists(y => y == x)).ToList();
				}

				// - 
				if(mhdIds?.Count > 0)
				{
					switch((Enums.FAEnums.FaLands)faLagerort)
					{
						case Enums.FAEnums.FaLands.AL:
							Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_ALAccess.ResetMHD(mhdIds, botransaction.connection, botransaction.transaction);
							break;
						case Enums.FAEnums.FaLands.CZ:
							Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_CZAccess.ResetMHD(mhdIds, botransaction.connection, botransaction.transaction);
							break;
						case Enums.FAEnums.FaLands.TN:
						case Enums.FAEnums.FaLands.BETN:
						case Enums.FAEnums.FaLands.WS:
						case Enums.FAEnums.FaLands.GZTN:
							Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_TNAccess.ResetMHD(mhdIds, botransaction.connection, botransaction.transaction);
							break;
						default:
							break;
					}
				}
			}
		}
	}
}