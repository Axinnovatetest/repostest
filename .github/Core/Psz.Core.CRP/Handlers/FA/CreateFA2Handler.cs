using Infrastructure.Data.Entities.Tables.PRS;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.CustomerService.Helpers;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA
{
	public class CreateFA2Handler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private FACreateModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public CreateFA2Handler(FACreateModel data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			int response = 0;
			//opening sql transaction
			botransaction.beginTransaction();
			List<int> insertedFaNumbers = new List<int>();
			var maxFa = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetMaxFertigungsnummer(this._data.Mandant) + 1;
			//check for doubles souilmi --08/05/2025
			var checkFaNummerExsist = Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.GetByFertigungsnummer(maxFa);
			if(checkFaNummerExsist != null && checkFaNummerExsist.Count > 0)
			{
				maxFa = Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.GetMaxFertigungsnummer() + 1;
				if(Infrastructure.Data.Access.Tables.PRS.FertigungAccess.ExistFertigungsnummer(maxFa))
				{
					return ResponseModel<int>.FailureResponse("Another FA is in creation, please try again in a moment .");
				}
			}

			var faMaxRespnse = Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.Insert(new Infrastructure.Data.Entities.Tables.CRP.__crp_FertigungsnummerEntity
			{
				Fertigungsnummer = maxFa,
				User = $"{_user.Username}-{_user.Id}",
			});

			if(faMaxRespnse == -1)
				return ResponseModel<int>.FailureResponse("Another FA is in creation, please try again in a moment .");
			lock(Locks.Locks.FACreateLock.GetOrAdd(maxFa, new object()))
			{
				try
				{
					var LagerWithVersionning = Module.LagersWithVersionning;
					//preparing
					var storageLocationEntity = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get(this._data.Produktionsort ?? -1);
					var itemCalculatoryCostsEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelKalkulatorischeKostenAccess.GetArbeitskostenByArtikelNr((int)this._data.ArticleId);
					var articleEntiy = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get((int)this._data.ArticleId);
					var priceGroupEntity = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr((int)this._data.ArticleId);

					// - 2025-03-20 - handle all sales prices
					var salesPriceType = Psz.Core.CustomerService.Enums.OrderEnums.ConvertToMTDSalesItemType(_data.Typ?.Trim() ?? "serie");
					var salesPriceExtention = Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.GetByArticleNrAndTypeId(_data.ArticleId ?? 0, (int)salesPriceType)
						?? new Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity();
					//  - 2025-03-25 use custom price only for Serie
					var priceType = salesPriceType != Common.Enums.ArticleEnums.SalesItemType.Serie ? "" : Psz.Core.CRP.Helpers.CRPHelper.getPriceType(priceGroupEntity, Convert.ToDecimal(this._data.Menge));
					var staffPriceEntity = Infrastructure.Data.Access.Tables.PRS.StaffelpreisKonditionzuordnungAccess.GetByArtikelNrAndType((int)this._data.ArticleId, priceType);


					var price = this._data.Erstmusterauftrag.HasValue && this._data.Erstmusterauftrag.Value
							? /*(itemCalculatoryCostsEntity?.Betrag ?? 0m)*/this._data.Surcharge ?? 0m + this._data.Erstmusterpreis
							: (priceType == "S0" || staffPriceEntity == null)
								? (itemCalculatoryCostsEntity?.Betrag ?? 0m)
								: (staffPriceEntity?.Betrag ?? 0m);

					var time = (priceType == "S0" || staffPriceEntity == null)
						? (articleEntiy.Produktionszeit ?? 0m)
						: (staffPriceEntity?.ProduKtionzeit ?? 0m);

					// - 2025-03-20 - handle all sales prices
					switch(salesPriceType)
					{
						case Core.Common.Enums.ArticleEnums.SalesItemType.FirstSample:
							{
								price = (salesPriceExtention.Produktionskosten ?? 0m) + Convert.ToDecimal(this._data.Surcharge);
								time = salesPriceExtention.Profuktionszeit ?? 0m;
								break;
							}
						case Core.Common.Enums.ArticleEnums.SalesItemType.Prototype:
						case Core.Common.Enums.ArticleEnums.SalesItemType.NullSerie:
							{
								price = salesPriceExtention.Produktionskosten ?? 0m;
								time = salesPriceExtention.Profuktionszeit ?? 0m;
								break;
							}
						case Core.Common.Enums.ArticleEnums.SalesItemType.Serie:
						default:
							{
								price = (priceType == "S0" || staffPriceEntity == null) ? (itemCalculatoryCostsEntity?.Betrag ?? 0m) : (staffPriceEntity.Betrag ?? 0m);
								time = (priceType == "S0" || staffPriceEntity == null) ? ((articleEntiy?.Produktionszeit ?? salesPriceExtention?.Profuktionszeit) ?? 0m) : (staffPriceEntity.ProduKtionzeit ?? 0m);
								break;
							}
					}

					var bemerkung = $"{storageLocationEntity?.Lagerort} {this._data.Kunde} {this._data.Typ} {this._data.Kontakt}";
					var bemerkungPlannung = $"Erstellt: {this._user.Name},{DateTime.Now}";
					var bemerkungOhneStatte = $"Eigenfertigung . {this._data.Kunde}, {this._data.Typ}, {this._data.Kontakt}";
					var snapshotStucklistEntity = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetLastByArticle((int)this._data.ArticleId);
					var snapshotCPEntity = Infrastructure.Data.Access.Tables.BSD.CP_snapshot_positionsAccess.GetLastByArticle((int)this._data.ArticleId, snapshotStucklistEntity?.BomVersion ?? 0);
					var articleExtensionEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle((int)this._data.ArticleId);
					int? bomVersion = null;
					int? cpVersion = null;
					if(LagerWithVersionning.Contains(this._data.Produktionsort ?? -1))
					{
						bomVersion = snapshotStucklistEntity?.BomVersion ?? articleExtensionEntity?.BomVersion ?? null;
						cpVersion = snapshotCPEntity?.CP_version ?? null;
					}
					var urspArticleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.Ursp_Artikel ?? -1);


					var fertigungEntity = new Infrastructure.Data.Entities.Tables.PRS.FertigungEntity()
					{
						Artikel_Nr = articleEntiy.ArtikelNr,
						Anzahl = this._data.Menge,
						Lagerort_id = this._data.Produktionsort,
						Fertigungsnummer = maxFa,
						Datum = DateTime.Now,
						Termin_Fertigstellung = this._data.Produktionstermin,
						Termin_Bestatigt1 = this._data.Produktionstermin,
						Kennzeichen = "Offen",
						Preis = (this._data.Produktionsort == 6 && this._data.Technikauftrage) ? 0 : price,
						Gebucht = true,
						Bemerkung = bemerkung,
						Originalanzahl = this._data.Menge,
						Anzahl_erledigt = 0,
						Bemerkung_Planung = bemerkungPlannung,
						Mandant = this._data.Mandant,
						Lagerort_id_zubuchen = this._data.Hauptlager,
						Technik = this._data.Technikauftrage,
						Erstmuster = this._data.Erstmusterauftrag.HasValue && this._data.Erstmusterauftrag.Value ? true : false,
						Zeit = time,
						Techniker = (!string.IsNullOrEmpty(this._data.Techniker) && !string.IsNullOrWhiteSpace(this._data.Techniker)) ? this._data.Techniker : "-",
						Bemerkung_ohne_statte = bemerkungOhneStatte,
						Termin_Ursprunglich = this._data.Produktionstermin,
						KundenIndex = articleEntiy.Index_Kunde,
						Urs_Artikelnummer = urspArticleEntity?.ArtikelNummer ?? "-",
						UBG = this._data.UBG,
						UBGTransfer = this._data.UBGTransfer,
						BomVersion = bomVersion,
						CPVersion = cpVersion,
						Planungsstatus = "A",
						Anzahl_aktuell = 0,
						Angebot_nr = 0,
						Angebot_Artikel_Nr = 0,
						ID_Rahmenfertigung = 0,
						ID_Hauptartikel = 0,
						Gedruckt = false,
						Kommisioniert_teilweise = false,
						Kommisioniert_komplett = false,
						Kabel_geschnitten = false,
						Kabel_Schneidebeginn = false,
						Tage_Abweichung = 0,
						Letzte_Gebuchte_Menge = 0,
						Löschen = false,
						Quick_Area = false,
						Check_Kabelgeschnitten = false,
						HBGFAPositionId = this._data.HBGFaPositionId,
						FertigungType = salesPriceType.GetDescription() // - 2025-04-10 - KH for print and Packaging in Versand app
					};
					//inserting FA
					int insertedId = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.InsertWithTransaction(fertigungEntity, botransaction.connection, botransaction.transaction);
					var frZone = DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays);

					// - 2025-08-06 // deactivate ALL FA Email notifs - Khelil
					////updating FA if FA production date in frozen zone
					//if(this._data.Produktionstermin <= frZone)
					//{
					//	SendFACreationEmail(fertigungEntity, articleEntiy, _user, insertedId);
					//}



					//Inserting fa history entity to track FA history 2025-06-20
					var historyEntity = new Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity
					{
						Fertigungsnummer = fertigungEntity.Fertigungsnummer,
						Artikelnummer = articleEntiy.ArticleNumber,
						Bezeichnung = articleEntiy.Bezeichnung1,
						FA_Menge = Convert.ToInt32(fertigungEntity.Anzahl ?? -1),
						Änderungsdatum = DateTime.Now,
						Bemerkung = fertigungEntity.Bemerkung,
						Termin_Wunsch = fertigungEntity.Termin_Fertigstellung,
						CS_Mitarbeiter = "",
						Termin_Bestätigt1 = fertigungEntity.Termin_Bestatigt1,
						Termin_voränderung = new DateTime(2999, 12, 31).Date,
						Ursprünglicher_termin = fertigungEntity.Termin_Ursprunglich,
						Mitarbeiter = this._user.Name,
						Lagerort_id = fertigungEntity.Lagerort_id,
						Erstmuster = fertigungEntity.Erstmuster
					};
					Infrastructure.Data.Access.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieAccess.InsertWithTransaction(historyEntity, botransaction.connection, botransaction.transaction);

					//adding FA stucklist
					if(LagerWithVersionning.Contains(this._data.Produktionsort ?? -1))
					{
						if(snapshotStucklistEntity != null && snapshotStucklistEntity.BomVersion.HasValue)
						{
							var stcuklistenEntity = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetByArticleAndVersion((int)this._data.ArticleId, snapshotStucklistEntity.BomVersion);
							foreach(var item in stcuklistenEntity)
							{
								Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity
								{
									ID_Fertigung_HL = insertedId,
									ID_Fertigung = insertedId,
									Artikel_Nr = item.Artikel_Nr_des_Bauteils,
									Anzahl = this._data.Menge * item.Anzahl,
									Lagerort_ID = this._data.Produktionsort,
									Buchen = true,
									Vorgang_Nr = item.Vorgang_Nr,
									ME_gebucht = false,
									Löschen = false,
								}, botransaction.connection, botransaction.transaction);
							}
						}
						else
						{
							var stcuklistenEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle((int)this._data.ArticleId);
							foreach(var item in stcuklistenEntity)
							{
								Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity
								{
									ID_Fertigung_HL = insertedId,
									ID_Fertigung = insertedId,
									Artikel_Nr = item.Artikel_Nr_des_Bauteils,
									Anzahl = this._data.Menge * item.Anzahl,
									Lagerort_ID = this._data.Produktionsort,
									Buchen = true,
									Vorgang_Nr = item.Vorgang_Nr,
									ME_gebucht = false,
									Löschen = false,
								}, botransaction.connection, botransaction.transaction);
							}
						}
					}
					else
					{
						var stcuklistenEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle((int)this._data.ArticleId);
						foreach(var item in stcuklistenEntity)
						{
							Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity
							{
								ID_Fertigung_HL = insertedId,
								ID_Fertigung = insertedId,
								Artikel_Nr = item.Artikel_Nr_des_Bauteils,
								Anzahl = this._data.Menge * item.Anzahl,
								Lagerort_ID = this._data.Produktionsort,
								Buchen = true,
								Vorgang_Nr = item.Vorgang_Nr,
								ME_gebucht = false,
								Löschen = false,
							}, botransaction.connection, botransaction.transaction);
						}
					}
					//update gewerks values
					var insertedFA = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetWithTransaction(insertedId, botransaction.connection, botransaction.transaction);
					var gewerks = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetGewerksValuesWithTransaction(insertedId, botransaction.connection, botransaction.transaction);
					string gewerk1 = null;
					string gewerk2 = null;
					string gewerk3 = null;
					if(gewerks != null && gewerks.Count > 0)
					{
						gewerk1 = gewerks.FirstOrDefault(x => x.Value == "Gewerk 1").Value;
						gewerk2 = gewerks.FirstOrDefault(x => x.Value == "Gewerk 2").Value;
						gewerk3 = gewerks.FirstOrDefault(x => x.Value == "Gewerk 3").Value;
					}
					insertedFA.Gewerk_1 = gewerk1 != null ? "False" : "No";
					insertedFA.Gewerk_2 = gewerk2 != null ? "False" : "No";
					insertedFA.Gewerk_3 = gewerk3 != null ? "False" : "No";
					if(this._data.Produktionstermin < frZone)
					{
						insertedFA.PlanningDateViolation = true;
					}
					Infrastructure.Data.Access.Tables.PRS.FertigungAccess.UpdateWithTransaction(insertedFA, botransaction.connection, botransaction.transaction);

					// - 2022-10-26 - keep track of HBG
					if(this._data.UBG == true && this._data.HBGFaPositionId > 0)
					{
						var hbgPos = Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.GetWithTransaction(this._data.HBGFaPositionId, botransaction.connection, botransaction.transaction);
						if(hbgPos != null)
						{
							hbgPos.UBGFertigungsId = insertedId;
							hbgPos.UBGFertigungsnummer = maxFa;
							hbgPos.IsUBG = true;
							Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.UpdateWithTransaction(hbgPos, botransaction.connection, botransaction.transaction);

							// - 2022-11-23 - add comments for HBG
							var hbgFa = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetWithTransaction(hbgPos.ID_Fertigung ?? -1, botransaction.connection, botransaction.transaction);
							fertigungEntity.Bemerkung += $" für HBG FA-{hbgFa?.Fertigungsnummer}";
							fertigungEntity.Bemerkung_Planung += $" HBG FA-{hbgFa?.Fertigungsnummer}";
							fertigungEntity.Bemerkung_ohne_statte += $" HBG FA-{hbgFa?.Fertigungsnummer}";
							fertigungEntity.ID = insertedId;
							Infrastructure.Data.Access.Tables.PRS.FertigungAccess.UpdateCommentsWithTransaction(fertigungEntity, botransaction.connection, botransaction.transaction);
						}
					}

					//logging
					var _log = new Helpers.LogHelper((int)insertedFA.Fertigungsnummer, 0, 0, "Fertigung", Helpers.LogHelper.LogType.CREATIONFA, "CTS", _user)
						.LogCTS(null, null, null, 0);
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(_log, botransaction.connection, botransaction.transaction);
					// -- ubg logic 05-03-2025
					var stucklistenEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle((int)this._data.ArticleId);
					var positionsData = stucklistenEntities.Select(stucklistenEntity => new Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity
					{
						ID_Fertigung_HL = fertigungEntity.ID,
						ID_Fertigung = fertigungEntity.ID,
						Artikel_Nr = stucklistenEntity.Artikel_Nr_des_Bauteils,
						Anzahl = fertigungEntity.Anzahl * stucklistenEntity.Anzahl,
						Lagerort_ID = fertigungEntity.Lagerort_id,
						Buchen = true,
						Vorgang_Nr = stucklistenEntity.Vorgang_Nr,
						ME_gebucht = false
					}).ToList();
					if(this._data.CreateUBGFas && this._data.UbgFaItems != null)
					{
						var ubgFas = this._data.UbgFaItems.Where(x => x.Checked == true)?.ToList();
						if(ubgFas != null && ubgFas.Count > 0)
						{
							positionsData = Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.GetByIdFertigung(insertedId, botransaction.connection, botransaction.transaction);
							foreach(var ubgFaItem in ubgFas)
							{
								var hauptLager = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.GetHauptByStandard(ubgFaItem.ProdLager ?? 0);
								ubgFaItem.DestLager = hauptLager?.Lagerort_id ?? -1;
								var faPosition = positionsData.FirstOrDefault(x => x.Artikel_Nr == ubgFaItem.ArticleId && (decimal?)x.Anzahl == ubgFaItem.ProdQuantity);
								createFA(salesPriceType, ubgFaItem, fertigungEntity.Fertigungsnummer ?? -1, faPosition, this._user, this._data, botransaction, ref insertedFaNumbers);
							}
						}
					}

					//commiting
					if(botransaction.commit())
					{
						//updating article production place
						var articleProductionExtensionEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.GetByArticleId(articleEntiy.ArtikelNr);
						var _prodPlace = -1;

						// - 2022-12-22 - return EnumValue
						foreach(int i in Enum.GetValues(typeof(Common.Enums.ArticleEnums.ArticleProductionPlace)))
						{
							if(i == storageLocationEntity.LagerortId)
							{
								_prodPlace = i;
								break;
							}
						}

						if(articleProductionExtensionEntity != null)
						{
							if(!articleProductionExtensionEntity.ProductionPlace1_Id.HasValue && _prodPlace != -1)
							{
								articleProductionExtensionEntity.ProductionPlace1_Id = _prodPlace;
								articleProductionExtensionEntity.UpdateTime = DateTime.Now;
								articleProductionExtensionEntity.UpdateUserId = _user.Id;
								Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.Update(articleProductionExtensionEntity);
							}

						}
						else
						{
							Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.Insert(new Infrastructure.Data.Entities.Tables.BSD.ArtikelProductionExtensionEntity
							{
								ArticleId = articleEntiy.ArtikelNr,
								Id = -1,
								CreateTime = DateTime.Now,
								CreateUserId = _user.Id,
								ProductionPlace1_Id = _prodPlace != -1 ? _prodPlace : null,
								AlternativeProductionPlace = false,
							});
						}
						//adding capacity requirements
						Helpers.SpecialHelper.Perform(insertedFA);
						response = insertedFA?.ID ?? -1;
					}
					//deleting fa nummer
					if(insertedFaNumbers.Count>0)
					{
						Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.DeleteByFertigungsnummers(insertedFaNumbers);
					}
					Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.DeleteByFertigungsnummer(maxFa);

					return ResponseModel<int>.SuccessResponse(response);
				} catch(Exception e)
				{
					//deleting fa nummer
					if(insertedFaNumbers.Count > 0)
					{
						Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.DeleteByFertigungsnummers(insertedFaNumbers);
					}
					Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.DeleteByFertigungsnummer(maxFa);
					botransaction.rollback();
					Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
					throw;
				}
			}
		}

		public static void SendFACreationEmail(FertigungEntity fertigungEntity, ArtikelEntity articleEntiy, Identity.Models.UserModel user, int insertedId)
		{
			var addresses = new List<string>();

			// - 2025-07-31 - remove Fr. Hejdukova
			var _lagerCompany = Infrastructure.Data.Access.Tables.CTS.lagerCompanyAccess.GetByLagerId(fertigungEntity.Lagerort_id ?? -1);
			var _company = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(_lagerCompany?.Company_id ?? -1);
			if(_company != null)
				addresses.Add(_company.DirectorEmail);

			var _kundeMitarbeiter = Infrastructure.Data.Access.Joins.CTS.Divers.FAMitarbiter((int)fertigungEntity.Fertigungsnummer);
			if(_kundeMitarbeiter != null)
			{
				var _mitarbeiterUser = Infrastructure.Data.Access.Tables.COR.UserAccess.GetByName(_kundeMitarbeiter);
				addresses.Add(_mitarbeiterUser?.Email);
			}

			addresses.Add(user.Email);

			/// - 2025-04-26 Add Fa notification users 
			List<int> faUsersIds = Infrastructure.Data.Access.Tables.CRP.CRP_FA_EmailUsersAccess
				.Get()
				.Where(x => x.UserId.HasValue)
				.Select(x => x.UserId.Value)
				.ToList();
			List<string> faUsersSiteEmails = Infrastructure.Data.Access.Tables.COR.UserAccess.GetBySite(faUsersIds, (int)fertigungEntity.Lagerort_id)
				.Where(user => user.IsActivated == true)
				.Select(x => x.Email).ToList();

			addresses.AddRange(faUsersSiteEmails);

			string faLink = $"{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/fertigung/details/{insertedId}";
			string subject = "⚠️ [ALERT] new FA in Frozen Zone";

			var content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
			+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>"
			+ $"<br/><span style='font-size:1.15em;'><strong>{user.Name?.ToUpper()}</strong> has just created a new FA {fertigungEntity.Fertigungsnummer}</strong>."
			+ $"</span><br/><br/>"
			+ "</div>";
			content += $"<hr>";
			content += $"<div style='background-color:#ffdddd;border-left:6px solid #d9534f;padding:12px;margin-top:16px;margin-bottom:16px;font-size:1.1em;'>"
			+ $"⚠️ <strong> Alert:</strong> The new FA date <strong>{fertigungEntity.Termin_Bestatigt1.Value.ToString("dd-MM-yyyy", new System.Globalization.CultureInfo("en-US"))}</ strong > is in the frozen Zone.Please review carefully."
			+ "</div>";

			content += $"<br/><span style='font-size:1.em;font-weight:bold'> FA :<a href='{faLink}'>{fertigungEntity.Fertigungsnummer}</a></span>";
			content += $"<br/><span style='font-size:1.em;font-weight:bold'> FA Menge :{fertigungEntity.Originalanzahl}</span>";
			content += $"<br/><span style='font-size:1.em;font-weight:bold'> Article :{articleEntiy.ArtikelNummer}</span>";
			content += $"<br/><span style='font-size:1.em;font-weight:bold'> Artikel Bezeichnung  :{articleEntiy.Bezeichnung1}</span>";
			content += $"<br/><span style='font-size:1.em;font-weight:bold; color:red'> FA Termin :{fertigungEntity.Termin_Bestatigt1.Value.ToString("dd-MM-yyyy", new System.Globalization.CultureInfo("en-US"))}</span>";
			content += "<br/><br/>";
			content += $"<br/><span style='font-size:1.em;font-weight:bold'>Regards,</span>";
			content += $"<br/><span style='font-size:1.em;font-weight:bold'>IT Department </span></br>";
			try
			{
				// - 2025-07-31 - remove Fr. Hejdukova - correct - 2025-08-05 - remove even coming from other email configs
				if(Module.CTS?.IgnoreFaCreateNotificationEmails?.Count > 0)
				{
					addresses = addresses.Where(x => Module.CTS?.IgnoreFaCreateNotificationEmails?.Exists(y => y.IsSameAs(x, false, true)) == false)?.ToList();
				}
				sendEmailNotification(subject, content, addresses);
			} catch(Exception exm)
			{
				Infrastructure.Services.Logging.Logger.Log(exm);
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			if(this._data.Menge <= 0)
			{
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Quantity should not be null");
			}
			if(!this._data.ArticleId.HasValue || (this._data.ArticleId.HasValue && (string.IsNullOrEmpty(this._data.ArticleId.Value.ToString()) || string.IsNullOrWhiteSpace(this._data.ArticleId.Value.ToString()))))
			{
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Artikelnummer should not be Empty");
			}
			if(string.IsNullOrEmpty(this._data.Mandant) || string.IsNullOrWhiteSpace(this._data.Mandant))
			{
				return ResponseModel<int>.FailureResponse(key: "2", value: $"Mandant should not be Empty");
			}
			if(!this._data.Produktionsort.HasValue || (this._data.Produktionsort.HasValue && (string.IsNullOrEmpty(this._data.Produktionsort.Value.ToString()) || string.IsNullOrWhiteSpace(this._data.Produktionsort.Value.ToString()))))
			{
				return ResponseModel<int>.FailureResponse(key: "3", value: $"FertigungStatte should not be Empty");
			}
			if(!this._data.Hauptlager.HasValue || (this._data.Hauptlager.HasValue && (string.IsNullOrEmpty(this._data.Hauptlager.Value.ToString()) || string.IsNullOrWhiteSpace(this._data.Hauptlager.Value.ToString()))))
			{
				return ResponseModel<int>.FailureResponse(key: "4", value: $"FG Zubuchen should not be Empty");
			}
			if(!this._data.Produktionstermin.HasValue)
			{
				return ResponseModel<int>.FailureResponse(key: "5", value: $"Produktionstermin should not be Empty");
			}
			//-
			if((_data.Typ.ToLower() == "erstmuster") && (!_data.Erstmusterauftrag.Value || !_data.Erstmusterauftrag.HasValue))
				return ResponseModel<int>.FailureResponse(key: "7", value: $"Please check Technical Order and Erstmusterauftrag and put the Price");

			if((_data.Typ.ToLower() == "serie") && _data.Erstmusterauftrag.Value)
				return ResponseModel<int>.FailureResponse(key: "7", value: $"FA cannot be of type Serie and be Erstmusterauftrag at the same time");
			if(this._data.Technikauftrage && (string.IsNullOrEmpty(this._data.Techniker) || string.IsNullOrWhiteSpace(this._data.Techniker)))
			{
				return ResponseModel<int>.FailureResponse(key: "6", value: $"Tecknik FA must have a technicien");
			}
			if(this._data.Erstmusterauftrag.HasValue && this._data.Erstmusterauftrag.Value)
			{
				if(this._data.Erstmusterpreis == 0)
					return ResponseModel<int>.FailureResponse(key: "7", value: $"Erstmuster FA must have a Price");
				if(this._data.Surcharge.HasValue)
				{
					if(this._data.Surcharge > 75m)
						return ResponseModel<int>.FailureResponse(key: "7", value: $"Erstmuster FA Surcharge cannot be bigger then 75");
					if(this._data.Surcharge < 0)
						return ResponseModel<int>.FailureResponse(key: "7", value: $"Erstmuster FA Surcharge cannot be negative");
				}
			}
			var itemCalculatoryCostsEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelKalkulatorischeKostenAccess.GetByArtikelNr(_data.ArticleId.Value);
			if(itemCalculatoryCostsEntity == null && _data.Typ?.ToLower() == "serie")
			{
				return ResponseModel<int>.FailureResponse("Position article cost record not found");
			}

			if(this._data.ArticleId.HasValue && Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.IsArticleReparatur(this._data.ArticleId.Value, Module.CTS.ReparaturArticles))
			{
				if(!this._data.Ursp_Artikel.HasValue)
					return ResponseModel<int>.FailureResponse(key: "8", value: $"Reparatur FA must have a Urs article");
			}
			var stcuklistenEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle((int)this._data.ArticleId);
			if(stcuklistenEntity == null || stcuklistenEntity.Count == 0)
			{
				return ResponseModel<int>.FailureResponse(key: "8", value: $"Article has no Stucklist positions");
			}

			var frZone = DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays);
			if(this._user?.Access?.CustomerService?.FAWerkWunshAdmin != true && this._user.Access?.CustomerService?.FaAdmin != true && this._data.Produktionsort != 6 && this._data.Technikauftrage != true && !Module.BSD.TechnicArticleIds.Exists(x => x == this._data.ArticleId))
			{
				var _newDate = this._data.Produktionstermin ?? new DateTime(1900, 1, 1);
				if(_newDate < DateTime.Today)
				{
					return ResponseModel<int>.FailureResponse($"Production date invalid: can not add FA [{_newDate.ToString("dd/MM/yyyy")}] in the past.");
				}

				if(_newDate <= frZone)
				{
					return ResponseModel<int>.FailureResponse($"Production date invalid: can not add FA before Frozen Zone limit [{frZone.ToString("dd/MM/yyyy")}].");
				}
			}
			//var technicArticles = Module.BSD.TechnicArticleIds;
			var hozizonCheck = Helpers.HorizonsHelper.userHasFaCreateHorizonRight(this._data.Produktionstermin ?? new DateTime(1900, 1, 1), _user, out List<string> messages);
			if(!hozizonCheck && !Helpers.HorizonsHelper.ArticleIsTechnic(_data.ArticleId ?? -1))
				return ResponseModel<int>.FailureResponse(messages);


			var errors = new List<ResponseModel<int>.ResponseError>();
			#region UBG FAs
			if(this._data.CreateUBGFas == true && this._data.UbgFaItems != null && this._data.UbgFaItems.Where(x => x.Checked == true).Count() > 0)
			{
				var salesPriceTypeId = Psz.Core.CustomerService.Enums.OrderEnums.ConvertToMTDSalesItemType(_data.Typ?.Trim() ?? "serie");
				var selectedUbgFas = this._data.UbgFaItems.Where(x => x.Checked)?.ToList();
				if(selectedUbgFas.Count > 0)
				{
					var ubgArticles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(selectedUbgFas.Select(x => x.ArticleId).ToList());
					var _errors = new List<string>();
					for(int i = 0; i < this._data.UbgFaItems.Where(x => x.Checked == true).Count(); i++)
					{
						var hauptLager = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.GetHauptByStandard(this._data.UbgFaItems[i].ProdLager ?? 0);
						this._data.UbgFaItems[i].DestLager = hauptLager?.Lagerort_id ?? -1;
						if(this._data.UbgFaItems[i].Checked == true)
						{
							validateUbgFa(_errors, salesPriceTypeId, this._data.UbgFaItems[i], i, this._user);
						}
					}
					//- 
					if(_errors != null && _errors.Count > 0)
					{
						errors.AddRange(_errors.Select(x => new ResponseModel<int>.ResponseError(x)));
					}
				}
				else
				{
					errors.Add(new ResponseModel<int>.ResponseError("No UBG FA selected"));
				}
			}
			#endregion Ubg Fas


			// >>>
			if(errors.Count > 0)
			{
				return new ResponseModel<int>() { Errors = errors };
			}

			return ResponseModel<int>.SuccessResponse();
		}
		static void createFA(Core.Common.Enums.ArticleEnums.SalesItemType salesPriceType, UbgFaItem ubgFaItem, int hbgFaNummer, Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity fertigungPositionenEntity, Identity.Models.UserModel user, FACreateModel data, Infrastructure.Services.Utils.TransactionsManager botransaction, ref List<int> insertedFaNumbers)
		{
			// - 2025-04-30 - Schremmer UBG should always be Serie.
			salesPriceType = Common.Enums.ArticleEnums.SalesItemType.Serie;
			var validationErrors = new List<string>();

			var LagerWithVersionning = Module.LagersWithVersionning ?? new List<int>();
			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(ubgFaItem.ArticleId, botransaction.connection, botransaction.transaction);
			var priceGroupEntity = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(ubgFaItem.ArticleId, botransaction.connection, botransaction.transaction);

			// - 2025-03-20 - handle all sales prices
			var salesPriceExtention = Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.GetByArticleNrAndTypeId(ubgFaItem.ArticleId, (int)salesPriceType)
				?? new Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity();
			var priceType = salesPriceType != Common.Enums.ArticleEnums.SalesItemType.Serie ? "" : Psz.Core.CRP.Helpers.CRPHelper.getPriceType(priceGroupEntity, ubgFaItem.ProdQuantity);
			var staffPriceEntity = Infrastructure.Data.Access.Tables.PRS.StaffelpreisKonditionzuordnungAccess.GetByArtikelNrAndType(ubgFaItem.ArticleId, priceType, botransaction.connection, botransaction.transaction);

			// - 
			var stucklistenEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticleWithTransaction(ubgFaItem.ArticleId, botransaction.connection, botransaction.transaction);
			var bomSnapshotCountByArticle = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetBOMVersionByArticle_Count(ubgFaItem.ArticleId, botransaction.connection, botransaction.transaction);

			// - if BOM activated for versioning
			if(LagerWithVersionning.Contains(ubgFaItem.ProdLager ?? -1))
			{
				// - If not first BOM for current Article, check Snapshot w Index
				if(bomSnapshotCountByArticle > 0)
				{
					var stucklistenSnapshot = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetLastByArticleAndIndex(ubgFaItem.ArticleId, articleEntity.Index_Kunde, botransaction.connection, botransaction.transaction) // - 2022-05-18 - take the BOM for Pos Index
						?.Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity
						{
							Anzahl = (float?)x.Anzahl,
							Artikelnummer = x.Artikelnummer,
							Artikel_Nr = x.Artikel_Nr,
							Artikel_Nr_des_Bauteils = x.Artikel_Nr_des_Bauteils,
							Bezeichnung_des_Bauteils = x.Bezeichnung_des_Bauteils,
							DocumentId = x.DocumentId,
							Nr = -1,
							Position = x.Position,
							Variante = x.Variante,
							Vorgang_Nr = x.Vorgang_Nr
						})?.ToList();

					if(/*articleEntity.Index_Kunde?.Trim() != orderItemEntity.Index_Kunde?.Trim() && */
						(stucklistenSnapshot == null || stucklistenSnapshot.Count <= 0))
					{
						validationErrors.Add($"Validated BOM not found for Artikel [{articleEntity.ArtikelNummer}] and Index [{articleEntity.Index_Kunde}]");
					}
					// - 
					if(stucklistenSnapshot != null && stucklistenSnapshot.Count > 0)
					{
						stucklistenEntities = stucklistenSnapshot;
					}
				}
				else
				{
					// 
					var stucklistenSnapshot = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(ubgFaItem.ArticleId, botransaction.connection, botransaction.transaction);
					/////////// 

					if(stucklistenSnapshot == null || stucklistenSnapshot.Count <= 0)
					{
						validationErrors.Add($"BOM not found for Artikel [{articleEntity.ArtikelNummer}]");
					}
					// - 
					if(stucklistenSnapshot != null && stucklistenSnapshot.Count > 0)
					{
						stucklistenEntities = stucklistenSnapshot;
					}
				}
			}

			if(stucklistenEntities.Count == 0)
			{
				validationErrors.Add("BOM not found");
			}

			// - 
			if(validationErrors.Count > 0)
			{
				return;
			}

			var itemCalculatoryCostsEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelKalkulatorischeKostenAccess.GetByArtikelNr(ubgFaItem.ArticleId, botransaction.connection, botransaction.transaction);
			var storageLocationEntity = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.GetWithTransaction(ubgFaItem.ProdLager ?? -1/* (int)orderItemEntity.Lagerort_id*/, botransaction.connection, botransaction.transaction);
			var nextFertigungsnummer = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetMaxFertigungsnummer("PSZ Electronic"/*orderEntity.Mandant*/)
				+ 1;

			// - 2026-02-05 - handle UBG FAs getting same number as HBG
			var checkFaNummerExsist = Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.GetByFertigungsnummer(nextFertigungsnummer);
			if(checkFaNummerExsist != null && checkFaNummerExsist.Count > 0)
			{
				nextFertigungsnummer = Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.GetMaxFertigungsnummer() + 1;
				if(Infrastructure.Data.Access.Tables.PRS.FertigungAccess.ExistFertigungsnummer(nextFertigungsnummer))
				{
					throw new Exception("Another FA is in creation, please try again in a moment.");
				}
			}

			var faMaxRespnse = Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.Insert(new Infrastructure.Data.Entities.Tables.CRP.__crp_FertigungsnummerEntity
			{
				Fertigungsnummer = nextFertigungsnummer,
				User = $"{user.Username}-{user.Id}",
			});
			insertedFaNumbers.Add(nextFertigungsnummer);

			var completionDeadlineDate = ubgFaItem.ProdDate;
			var appointmentConfirmedDate1 = ubgFaItem.ProdDate;

			var notes = $"{storageLocationEntity?.Lagerort}: {data.Typ} ,{data.Kunde} für HBG FA-{hbgFaNummer}";
			var creationNotes = $"Erstellt: {user.Name}, {DateTime.Now.ToString("dddd, dd MMMM yyyy")} für HBG FA-{hbgFaNummer}";
			var notesWithoutSite = $"Eigenfertigung, UBG ,{data.Kunde} für HBG FA-{hbgFaNummer}.";

			var price = data.Typ.ToLower() == "erstmuster"
				? /*(itemCalculatoryCostsEntity.Betrag ?? 0m)*/Convert.ToDecimal(data.Surcharge ?? 0m) + Convert.ToDecimal(data.Erstmusterpreis)
				: (priceType == "S0" || staffPriceEntity == null)
					? (itemCalculatoryCostsEntity?.Betrag ?? 0m)
					: (staffPriceEntity?.Betrag ?? 0m);

			var time = (priceType == "S0" || staffPriceEntity == null)
				? (articleEntity.Produktionszeit ?? 0m)
				: (staffPriceEntity?.ProduKtionzeit ?? 0m);

			// - 2025-03-20 - handle all sales prices
			switch(salesPriceType)
			{
				case Core.Common.Enums.ArticleEnums.SalesItemType.FirstSample:
					{
						price = (salesPriceExtention.Produktionskosten ?? 0m) + Convert.ToDecimal(data.Surcharge);
						time = salesPriceExtention.Profuktionszeit ?? 0m;
						break;
					}
				case Core.Common.Enums.ArticleEnums.SalesItemType.Prototype:
				case Core.Common.Enums.ArticleEnums.SalesItemType.NullSerie:
					{
						price = salesPriceExtention.Produktionskosten ?? 0m;
						time = salesPriceExtention.Profuktionszeit ?? 0m;
						break;
					}
				case Core.Common.Enums.ArticleEnums.SalesItemType.Serie:
				default:
					{
						price = (priceType == "S0" || staffPriceEntity == null) ? (itemCalculatoryCostsEntity?.Betrag ?? 0m) : (staffPriceEntity.Betrag ?? 0m);
						time = (priceType == "S0" || staffPriceEntity == null) ? ((articleEntity?.Produktionszeit ?? salesPriceExtention?.Profuktionszeit) ?? 0m) : (staffPriceEntity.ProduKtionzeit ?? 0m);
						break;
					}
			}
			if(itemCalculatoryCostsEntity.Kostenart.ToLower() == "arbeitskosten")
			{
				price = data.Typ.ToLower() == "erstmuster"
				? /*(itemCalculatoryCostsEntity.Betrag ?? 0m)*/Convert.ToDecimal(data.Surcharge ?? 0m) + Convert.ToDecimal(data.Erstmusterpreis)
					: itemCalculatoryCostsEntity.Betrag ?? 0m;

				time = (articleEntity?.Produktionszeit ?? salesPriceExtention?.Profuktionszeit) ?? 0m;
			}

			// > Insert Production > Queries: 1, 2, 3, 4, 6, 7, 8, 10 and 11
			var fertigungEntity = new Infrastructure.Data.Entities.Tables.PRS.FertigungEntity()
			{
				//Angebot_nr = orderItemEntity.AngebotNr ,
				//Angebot_Artikel_Nr = orderItemEntity.Nr ,
				Artikel_Nr = ubgFaItem.ArticleId,
				Anzahl = int.Parse(ubgFaItem.ProdQuantity.ToString()),
				Lagerort_id = ubgFaItem.ProdLager,
				Fertigungsnummer = nextFertigungsnummer,
				Datum = DateTime.Now,
				Termin_Fertigstellung = completionDeadlineDate.Date,

				Termin_Bestatigt1 = appointmentConfirmedDate1.Date,
				//Gebucht = false, // 1
				//Kennzeichen = "gesperrt", // 1
				Bemerkung = notes, // 1, 6, 7 and 8
				Originalanzahl = int.Parse(ubgFaItem.ProdQuantity.ToString()),
				Bemerkung_Planung = creationNotes,
				Mandant = "PSZ Electronic"/*orderEntity.Mandant*/,

				Lagerort_id_zubuchen = ubgFaItem.DestLager, // orderItemEntity.Lagerort_id ,
				Techniker = null, // data.Techniker,
				Erstmuster = false, //data.Erstmusterauftrag ,
				Technik = false, // data.Technikauftrage ,
				Bemerkung_ohne_statte = notesWithoutSite,
				Termin_Ursprunglich = ubgFaItem.ProdDate,// data.ProductionDate.Date ,

				KundenIndex = articleEntity.Index_Kunde,
				Kunden_Index_Datum = articleEntity.Index_Kunde_Datum,
				Urs_Artikelnummer = "-",
				//data.OriginalArticleId.ToString(),
				UBG = true, //data.Storage_Subassembly ,
				UBGTransfer = false,

				Preis = price,  // 1, 3, 4 and 10
				Zeit = time, // 3, 4 and 10

				Gebucht = true, // 6, 7 and 8
				Kennzeichen = "Offen", // 6, 7 and 8

				// > Missing
				Anzahl_erledigt = 0,
				Anzahl_aktuell = 0,
				ID_Hauptartikel = 0,
				ID_Rahmenfertigung = 0,
				Planungsstatus = "A",
				Tage_Abweichung = 0,
				Letzte_Gebuchte_Menge = 0,

				Gedruckt = false, // issue #81

				Kabel_geschnitten = false,//souilmi 21/06/2022
				Check_Kabelgeschnitten = false,//souilmi 21/06/2022
				HBGFAPositionId = fertigungPositionenEntity?.ID ?? -1, // data.HBGFAPositionId
				FertigungType = salesPriceType.GetDescription() // - 205-04-10 KH for print and packaging in Versand app
			};

			// - set BOM & CP Version - Update only BETN - for now
			var logEntity = new Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity();
			if(LagerWithVersionning.Contains(fertigungEntity.Lagerort_id ?? -1))
			{
				var articleSnapshotEntity = new Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity();
				var cpSnapshotEntity = new Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_positionsEntity();
				var articleExtension = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(ubgFaItem.ArticleId, botransaction.connection, botransaction.transaction);
				if(bomSnapshotCountByArticle > 0)
				{
					articleSnapshotEntity = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetLastByArticleAndIndex(ubgFaItem.ArticleId, articleEntity.Index_Kunde, botransaction.connection, botransaction.transaction)[0]; // - Already checked that it has at least 1 elmt
					cpSnapshotEntity = Infrastructure.Data.Access.Tables.BSD.CP_snapshot_positionsAccess.GetLastByArticle(ubgFaItem.ArticleId, articleSnapshotEntity?.BomVersion ?? -1, botransaction.connection, botransaction.transaction);
				}
				else
				{
					// - fake Snapshot - just to have BOM & CP info
					articleSnapshotEntity = new Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity
					{
						KundenIndex = articleEntity.Index_Kunde,
						KundenIndexDate = articleEntity.Index_Kunde_Datum,
						BomVersion = articleExtension?.BomVersion ?? 0
					};
				}


				fertigungEntity.BomVersion = articleSnapshotEntity?.BomVersion;
				fertigungEntity.KundenIndex = articleSnapshotEntity?.KundenIndex;
				fertigungEntity.CPVersion = cpSnapshotEntity?.CP_version;
				fertigungEntity.Kunden_Index_Datum = articleSnapshotEntity?.KundenIndexDate;

				// -
				logEntity =
					Psz.Core.BaseData.Handlers.ObjectLogHelper.getLog(user, -1, $"[FA:{fertigungEntity.Fertigungsnummer}] BOM || CP || IndexKunde",
					$"{fertigungEntity.BomVersion} || {cpSnapshotEntity?.CP_version} || {fertigungEntity.KundenIndex}",
					$"{articleSnapshotEntity?.BomVersion} || {articleSnapshotEntity?.KundenIndex}",
					"Fertigung",
					Core.BaseData.Enums.ObjectLogEnums.LogType.Edit);
			}

			fertigungEntity.ID = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.InsertWithTransaction(fertigungEntity, botransaction.connection, botransaction.transaction);

			if(LagerWithVersionning.Contains(fertigungEntity.Lagerort_id ?? -1)) // -- logging // - Update only BETN - for now
			{
				logEntity.LogObjectId = fertigungEntity.ID;
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(logEntity, botransaction.connection, botransaction.transaction);
			}

			Infrastructure.Services.Logging.Logger.LogTrace("STEP 1 COMPLETED");

			// > Insert Production Item > Query: 5
			Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.InsertWithTransaction(stucklistenEntities.Select(stucklistenEntity => new Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity
			{
				ID_Fertigung_HL = fertigungEntity.ID,
				ID_Fertigung = fertigungEntity.ID,
				Artikel_Nr = stucklistenEntity.Artikel_Nr_des_Bauteils,
				Anzahl = fertigungEntity.Anzahl * stucklistenEntity.Anzahl,
				Lagerort_ID = fertigungEntity.Lagerort_id,
				Buchen = true,
				Vorgang_Nr = stucklistenEntity.Vorgang_Nr,
				ME_gebucht = false
			}).ToList(), botransaction.connection, botransaction.transaction);

			// > WorkArea > Queries: 12, 13 and 14
			var list_Gewerk_Fertigungsnummer = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get_Gewerk_Fertigungsnummer_Query11(nextFertigungsnummer, botransaction.connection, botransaction.transaction);

			var gewerk1 = list_Gewerk_Fertigungsnummer.Exists(e => e.Item1.ToLower() == "gewerk 1")
				? "False"
				: "No";
			var gewerk2 = list_Gewerk_Fertigungsnummer.Exists(e => e.Item1.ToLower() == "gewerk 2")
				? "False"
				: "No";
			var gewerk3 = list_Gewerk_Fertigungsnummer.Exists(e => e.Item1.ToLower() == "gewerk 3")
				? "False"
				: "No";
			Infrastructure.Data.Access.Tables.PRS.FertigungAccess.UpdateGewerk(nextFertigungsnummer, gewerk1, gewerk2, gewerk3, botransaction.connection, botransaction.transaction);
			//update article production place
			var articleExtensionEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.GetByArticleId(ubgFaItem.ArticleId, botransaction.connection, botransaction.transaction);
			var _prodPlace = -1;
			// - 2022-12-22 - return EnumValue
			foreach(int i in Enum.GetValues(typeof(Common.Enums.ArticleEnums.ArticleProductionPlace)))
			{
				if(i == storageLocationEntity.LagerortId)
				{
					_prodPlace = i;
					break;
				}
			}
			if(articleExtensionEntity != null)
			{
				if(_prodPlace != -1 && !articleExtensionEntity.ProductionPlace1_Id.HasValue)
				{
					articleExtensionEntity.ProductionPlace1_Id = _prodPlace;
					articleExtensionEntity.UpdateTime = DateTime.Now;
					articleExtensionEntity.UpdateUserId = user.Id;
					Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.UpdateWithTransaction(articleExtensionEntity, botransaction.connection, botransaction.transaction);
				}

			}
			else
			{
				Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.BSD.ArtikelProductionExtensionEntity
				{
					ArticleId = ubgFaItem.ArticleId,
					Id = -1,
					CreateTime = DateTime.Now,
					CreateUserId = user.Id,
					ProductionPlace1_Id = _prodPlace != -1 ? _prodPlace : null,
					AlternativeProductionPlace = false,
				}, botransaction.connection, botransaction.transaction);
			}

			// - save UBG FA Id in HBG FA Position
			Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.UpdateUBGIdWithTransaction(fertigungPositionenEntity.ID, fertigungEntity.ID, fertigungEntity.Fertigungsnummer, botransaction.connection, botransaction.transaction);
			//logging
			var _logfa = new LogHelper((int)fertigungEntity.Fertigungsnummer, 0, 0, "Fertigung", LogHelper.LogType.CREATIONFA, "CTS", user)
				.LogCTS(null, null, null, 0);
			Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_logfa);
		}
		static bool validateUbgFa(List<string> errors, Common.Enums.ArticleEnums.SalesItemType salesPriceTypeId, UbgFaItem ubgFaItem, int idx, Identity.Models.UserModel user)
		{
			if(ubgFaItem == null)
				errors.Add($"UBG FA [{idx}]: invalid data");

			#region >>> fertigung data

			var itemDb = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(ubgFaItem.ArticleId);
			if(itemDb == null)
			{
				errors.Add($"UBG FA [{idx}]: Article not found");
			}
			if(itemDb.Freigabestatus.ToUpper() == "O")
			{
				errors.Add($"UBG FA [{idx}]: Article is 'Obsolete'");
			}
			if(itemDb.aktiv != true)
			{
				errors.Add($"UBG FA [{idx}]: Article not active");
			}
			var bomEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(ubgFaItem.ArticleId);
			if(bomEntities == null || bomEntities.Count <= 0)
			{
				errors.Add($"UBG FA [{idx}]: Article BOM empty");
			}

			var itemCalculatoryCostsEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelKalkulatorischeKostenAccess.GetByArtikelNr(itemDb.ArtikelNr);
			if(itemCalculatoryCostsEntity == null && salesPriceTypeId == Common.Enums.ArticleEnums.SalesItemType.Serie)
			{
				errors.Add($"UBG FA [{idx}]: Article cost record not found");
			}

			// - 2024-10-25
			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.IsArticleReparatur(ubgFaItem.ArticleId, Psz.Core.CustomerService.Module.CTS.ReparaturArticles))
			{
				errors.Add($"UBG FA [{idx}]: Reparatur FA must have a Urs article");
			}
			#endregion fertigung data

			#region >>> Pricing group
			var preisgruppenEntity = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(itemDb.ArtikelNr);
			if(preisgruppenEntity == null)
			{
				errors.Add($"UBG FA [{idx}]: Princing group invalid");
			}
			#endregion Pricing group

			// - 2023-02-03
			var frZone = DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays); // 2024-01-25 - Khelil change H1 to 41 days
			if(user?.Access?.CustomerService?.FAWerkWunshAdmin != true && user.Access?.CustomerService?.FaAdmin != true && ubgFaItem.ProdLager != 6 && !Module.BSD.TechnicArticleIds.Exists(x => x == itemDb.ArtikelNr))
			{
				var _newDate = ubgFaItem.ProdDate;
				if(_newDate < DateTime.Today)
				{
					errors.Add($"UBG FA [{idx}] Production date invalid: cannot add FA [{_newDate.ToString("dd/MM/yyyy")}] in the past.");
				}

				if(_newDate <= frZone)
				{
					errors.Add($"UBG FA [{idx}] Production date invalid: cannot add FA before Frozen Zone limit [{frZone.ToString("dd/MM/yyyy")}].");
				}
			}

			// - 2023-11-06
			var horizonCheck = HorizonsHelper.userHasFaCreateHorizonRight(ubgFaItem.ProdDate, user, out List<string> messages);
			if(!horizonCheck && !HorizonsHelper.ArticleIsTechnic(ubgFaItem.ArticleId))
				errors.AddRange(messages);

			// -2025-03-20
			var salesPriceEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.GetByArticleNrAndTypeId(ubgFaItem.ArticleId, (int)salesPriceTypeId);
			if(salesPriceEntity is null)
			{
				errors.Add($"UBG FA [{idx}] Production sale type invalid: sales type [{salesPriceTypeId.ToString()}] not found for article [{itemDb.ArtikelNummer}].");
			}
			// - 
			return errors == null || errors.Count <= 0;
		}

		public static void sendEmailNotification(string title, string contentHtml, List<string> toEmailAddresses)
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