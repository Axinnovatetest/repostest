using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Helpers;
using Psz.Core.CustomerService.Models.Blanket;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class AddABFromRahmenHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private AddABFromRahmenModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public AddABFromRahmenHandler(Identity.Models.UserModel user, AddABFromRahmenModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{

			var maxAngebotNr = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetMaxAngebotNrByTypeAndPrefix(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CONFIRMATION, (int)Common.Enums.INSEnums.INSOrderTypesAngebotNrPrefix.AB);
			//check for double angebotNr souilmi 12-05-2025
			var checkAngebotNrExist = Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.GetByAngebotNr(maxAngebotNr);
			if(checkAngebotNrExist != null && checkAngebotNrExist.Count > 0)
			{
				return ResponseModel<int>.FailureResponse("Another AB is in creation, please try again in a moment .");
			}
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				botransaction.beginTransaction();
				Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.Insert(new Infrastructure.Data.Entities.Tables.CRP.__crp_FertigungsnummerEntity
				{
					angebotNr = maxAngebotNr,
					User = $"{_user.Username}-{_user.Id}",
				});

				var response = -1;
				var rahmenEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data.RahmenId);
				var rahmenExtensionEntity = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(rahmenEntity.Nr);
				var customerDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(_data.CustomerId);
				var customerId = customerDb?.Nr;
				var customerNummer = customerDb?.Nummer;
				var adressDb = customerDb?.Nummer.HasValue == true
					? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customerDb.Nummer.Value)
					: null;
				var mailBoxIsPreferred = adressDb?.Postfach_bevorzugt == true;
				var conditionAssignementTableDb = customerDb?.Konditionszuordnungs_Nr.HasValue == true
						? Infrastructure.Data.Access.Tables.PRS.KonditionsZuordnungstabelleEntity.Get(customerDb.Konditionszuordnungs_Nr.Value)
						: null;
				//Angebot insert
				var orderDb = new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity()
				{
					Bezug = rahmenEntity.Bezug,
					EDI_Dateiname_CSV = "",
					ABSENDER = adressDb?.Name1,
					Kunden_Nr = adressDb?.Nr,
					Typ = Enums.OrderEnums.TypeToData(Enums.OrderEnums.Types.Confirmation),
					Mandant = "PSZ electronic",
					EDI_Order_Neu = false,
					Vorname_NameFirma = adressDb.Name1,
					Name2 = adressDb.Name2,
					Name3 = adressDb.Name3,
					Ansprechpartner = adressDb.Abteilung,
					Abteilung = adressDb.Abteilung,
					Straße_Postfach = mailBoxIsPreferred
								   ? $"Postfach {adressDb.Postfach}"
								   : $"{adressDb.StraBe}",
					Land_PLZ_Ort = mailBoxIsPreferred
								   ? $"{adressDb.PLZ_Postfach} {adressDb.Ort}"
								   : $"{adressDb.PLZ_StraBe} {adressDb.Ort} ",
					Versandart = customerDb?.Versandart,
					Zahlungsweise = customerDb?.Zahlungsweise,
					Konditionen = conditionAssignementTableDb?.Text,
					Unser_Zeichen = adressDb.Kundennummer.HasValue ? adressDb.Kundennummer.ToString() : "",
					Ihr_Zeichen = customerDb?.Lieferantenummer__Kunden_,
					USt_Berechnen = customerDb?.Umsatzsteuer_berechnen,
					Falligkeit = DateTime.Now.AddDays(+30),
					Datum = DateTime.Now,
					Briefanrede = adressDb.Briefanrede,
					Personal_Nr = 0,
					Freitext = $"USt - ID - Nr.: {customerDb?.EG___Identifikationsnummer}",
					Lieferadresse = "0",
					Reparatur_nr = 0,
					Ab_id = -1, // update after insert
					Nr_BV = 0,
					Nr_RA = _data.RahmenId,
					Nr_Kanban = 0,
					Nr_auf = 0,
					Nr_lie = 0,
					Nr_rec = 0,
					Nr_pro = 0,
					Nr_gut = 0,
					Nr_sto = 0,
					Belegkreis = 0,
					Wunschtermin = new DateTime(2999, 12, 31),
					Neu = -1,
					LAnrede = adressDb.Anrede,
					LVorname_NameFirma = adressDb.Name1,
					LName2 = adressDb.Name2,
					LName3 = adressDb.Name3,
					LAnsprechpartner = adressDb.Abteilung,
					LAbteilung = adressDb.Abteilung,
					LStraße_Postfach = $"{adressDb.StraBe}",
					LLand_PLZ_Ort = $"{adressDb.PLZ_StraBe}, {adressDb.Ort}",
					LBriefanrede = adressDb.Briefanrede,
					Neu_Order = null,

					// souilmi 12-05-2025 new double angebotNr prevention process
					Angebot_Nr = maxAngebotNr,
					Projekt_Nr = rahmenEntity.Projekt_Nr,

					Erledigt = false,
					LsAddressNr = adressDb.Nr,
					StorageLocation = adressDb.StorageLocation,
					UnloadingPoint = adressDb.UnloadingPoint,
				};
				response = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.InsertWithTransaction(orderDb/*, MaxCurrentValue, MinNewValue, Enums.OrderEnums.TypeToData(Enums.OrderEnums.Types.Confirmation)*/
					, botransaction.connection, botransaction.transaction);


				var orderExtensionDb = Infrastructure.Data.Access.Tables.PRS.OrderExtensionAccess.GetByOrderId(response);
				if(orderExtensionDb == null)
				{
					Infrastructure.Data.Access.Tables.PRS.OrderExtensionAccess.InsertWithTRansaction(new Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity()
					{
						Id = -1,
						Version = 0,
						LastUpdateTime = DateTime.Now,
						LastUpdateUserId = _user.Id,
						LastUpdateUsername = _user.Username,
						OrderId = response,
						EdiValidationTime = DateTime.Now,
						EdiValidationUserId = -1,
					}, botransaction.connection, botransaction.transaction);
				}
				else
				{
					orderExtensionDb.LastUpdateTime = DateTime.Now;
					orderExtensionDb.LastUpdateUserId = _user.Id;
					orderExtensionDb.LastUpdateUsername = _user.Username;
					orderExtensionDb.Version += 1;
					Infrastructure.Data.Access.Tables.PRS.OrderExtensionAccess.UpdateWithTRansaction(orderExtensionDb, botransaction.connection, botransaction.transaction);
				}
				//Angebot Artikel insert
				var ItemsToInsert = _data.Positions.Where(i => i.ABQuantity.HasValue && i.ABQuantity.Value > 0).ToList();
				var entitiesToInsert = new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
				var RahmenItemsToUpdate = new List<KeyValuePair<int, decimal>>();
				if(ItemsToInsert != null && ItemsToInsert.Count > 0)
				{
					foreach(var item in ItemsToInsert)
					{
						var extension = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngeboteneArtikelNr(item.AngeboteneArtikelNr, botransaction.connection, botransaction.transaction);
						var angebotPosition = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetWithTransaction(extension?.AngeboteArtikelNr ?? -1, botransaction.connection, botransaction.transaction);
						var postext = $"Aus Ihrer Rahmenbestellung: {rahmenEntity.Bezug}, PSZ Rahmenauftrag Nr.: {rahmenEntity.Angebot_Nr}, Position: {angebotPosition.Position}";
						if(item.ABQuantity > angebotPosition.Anzahl)
						{
							var pos1 = Helpers.BlanketHelper.GetCalculatedPositon(response, item.ArticleId, angebotPosition.Anzahl ?? 0, true, item.ABWunstermin ?? System.Data.SqlTypes.SqlDateTime.MinValue.Value, angebotPosition.Nr, postext);
							entitiesToInsert.Add(pos1);
							var pos2 = Helpers.BlanketHelper.GetCalculatedPositon(response, item.ArticleId, (item.ABQuantity.Value - angebotPosition.Anzahl.Value), false, (DateTime)item.ABWunstermin, angebotPosition.Nr, postext);
							entitiesToInsert.Add(pos2);
						}
						else
						{
							var pos3 = Helpers.BlanketHelper.GetCalculatedPositon(response, item.ArticleId, item.ABQuantity.Value, true, item.ABWunstermin ?? System.Data.SqlTypes.SqlDateTime.MinValue.Value, angebotPosition.Nr, postext);
							entitiesToInsert.Add(pos3);
						}
					}
					// - 2022-10-31
					var errorsQty = new List<string>();
					var raPositions = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data.RahmenId, botransaction.connection, botransaction.transaction)
						?? new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
					foreach(var raPos in raPositions)
					{
						var abPos = this._data.Positions?.Where(x => x.AngeboteneArtikelNr == raPos.Nr)?.ToList();
						if(abPos != null && abPos.Count > 0)
						{
							RahmenItemsToUpdate.Add(new KeyValuePair<int, decimal>(raPos.Nr, abPos.Sum(x => x.ABQuantity ?? 0)));
						}
					}
				}
				var i = 10;
				foreach(var a in entitiesToInsert)
				{
					a.Position = i;
					i += 10;
				}
				Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.InsertWithTransaction(entitiesToInsert, botransaction.connection, botransaction.transaction);

				// - 2022-10-31
				UpdateRahmenPostions(RahmenItemsToUpdate, _data.RahmenId, botransaction);
				var RahmenAfterInsertion = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(response, botransaction.connection, botransaction.transaction);
				RahmenAfterInsertion.Projekt_Nr = rahmenEntity.Projekt_Nr;
				Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateWithTransaction(RahmenAfterInsertion, botransaction.connection, botransaction.transaction);

				if(botransaction.commit())
				{
					Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.DeleteByAngebotNr(maxAngebotNr);
					//Logging
					var _log = new LogHelper(response, (int)orderDb.Angebot_Nr,
						int.TryParse(orderDb.Projekt_Nr, out var val) ? val : 0, orderDb.Typ, LogHelper.LogType.CREATIONOBJECT, "CTS", _user)
						.LogCTS(null, null, null, 0);
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_log);
					var insertedOrder = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(response);
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(new Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity
					{
						AngebotNr = rahmenEntity.Angebot_Nr,
						Nr = rahmenEntity.Nr,
						DateTime = DateTime.Now,
						LogObject = rahmenEntity.Typ,
						LogText = $"AB [{insertedOrder.Angebot_Nr}] Created from Position(s) [{string.Join(",", ItemsToInsert.Select(x => x.PositionId).ToList())}]",
						LogType = "CREATIONOBJECT",
						Origin = "CTS",
						ProjektNr = int.TryParse(rahmenEntity.Projekt_Nr, out var v) ? v : 0,
						UserId = _user.Id,
						Username = _user.Name
					});
					return ResponseModel<int>.SuccessResponse(response);
				}
				else
				{
					Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.DeleteByAngebotNr(maxAngebotNr);
					botransaction.rollback();
					return ResponseModel<int>.FailureResponse("Transaction did not commit.");
				}
			} catch(Exception e)
			{
				Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.DeleteByAngebotNr(maxAngebotNr);
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var rahmenEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data.RahmenId);
			if(rahmenEntity == null)
				return ResponseModel<int>.FailureResponse("Rahmen not found.");
			if(_data.Positions == null || _data.Positions.Count <= 0)
				return ResponseModel<int>.FailureResponse("No positions to add.");
			if(_data.Positions?.Where(x => x.ABQuantity.HasValue && x.ABQuantity.Value > 0)?.Count() <= 0)
				return ResponseModel<int>.FailureResponse("No positions with valid AB Quantity.");
			var errors = new List<string>();
			//var technicArticles = Module.BSD.TechnicArticleIds;
			for(int i = 0; i < _data.Positions.Count; i++)
			{
				if(_data.Positions[i].ABQuantity.HasValue && _data.Positions[i].ABQuantity.Value > 0)
				{
					// validate date
					if(!_data.Positions[i].ABWunstermin.HasValue || _data.Positions[i].ABWunstermin.Value <= System.Data.SqlTypes.SqlDateTime.MinValue.Value)
					{
						errors.Add($"Position {i + 1}: invalid Wunschtermin {_data.Positions[i].ABWunstermin}");
					}
					var rahmenExtensionEntity = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(rahmenEntity.Nr);
					var extension = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngeboteneArtikelNr(_data.Positions[i].AngeboteneArtikelNr);
					if(rahmenExtensionEntity.BlanketTypeId == (int)Enums.BlanketEnums.Types.purchase)
					{
						extension.ExtensionDate = extension.GultigBis;// - 2025-08-14 Hejdukova remove ExtDate .Value.AddDays(90);
					}

					// - 2025-08-14 Hejdukova remove ExtDate 
					if(DateTime.Now > extension.ExtensionDate)
					{
						errors.Add($"Position {i + 10}: Expired");
					}
					if(_data.Positions[i].ABWunstermin >= extension.ExtensionDate)
					{
						errors.Add($"Position {i + 10}: invalid Wunschtermin should be before RA ExpiryDate");
					}
					DateTime _newDate, _oldDate;
					_newDate = _oldDate = _data.Positions[i].ABWunstermin ?? new DateTime(1900, 1, 1);
					var horizonCheck = Helpers.HorizonsHelper.userHasABPosHorizonRight(_newDate, _oldDate, _user, out List<string> messages);
					if(!horizonCheck && !Helpers.HorizonsHelper.ArticleIsTechnic(_data.Positions[i].ArticleId))
						errors.AddRange(messages);
				}
			}

			// - 2022-10-31
			var errorsQty = new List<string>();
			var raPositions = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data.RahmenId)
				?? new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
			foreach(var raPos in raPositions)
			{
				var abPos = this._data.Positions?.Where(x => x.AngeboteneArtikelNr == raPos.Nr)?.ToList();
				if(abPos != null && abPos.Count > 0)
				{
					if(abPos.Sum(x => x.ABQuantity ?? 0) > raPos.Anzahl)
					{
						errorsQty.Add($"Position [{raPos.Position}]: not enough quantity");
					}
				}
			}
			if(errorsQty.Count > 0)
				return ResponseModel<int>.FailureResponse(errorsQty);

			var rahmenExtension = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(_data.RahmenId);
			if(rahmenExtension.StatusId != (int)Enums.BlanketEnums.RAStatus.Validated)
				errors.Add($"Rahmen is not validated, Action is blocked .");
			if(errors.Count > 0)
				return ResponseModel<int>.FailureResponse(errors);

			return ResponseModel<int>.SuccessResponse();
		}
		public static string getNextAngebotNr(Enums.OrderEnums.Types type)
		{
			var maxAngebotNrString = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.MaxAngebotNrByTyp(Enums.OrderEnums.TypeToData(type));
			return $"{Convert.ToInt32(Convert.ToDecimal(maxAngebotNrString)) + 1}";
		}
		public static void UpdateRahmenPostions(List<KeyValuePair<int, decimal>> _positons, int rahmenId, Infrastructure.Services.Utils.TransactionsManager botransaction)
		{
			var _toUpdate = new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
			var nrs = _positons?.Select(x => x.Key)?.ToList();
			var rahmenItemExtensionEntities = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngeboteneArtikelNr(nrs, botransaction.connection, botransaction.transaction);
			var rahmenItemEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetWithTransaction(nrs, botransaction.connection, botransaction.transaction);
			foreach(var item in _positons)
			{
				var rahmenItemExtensionEntity = rahmenItemExtensionEntities?.FirstOrDefault(x => x.AngeboteArtikelNr == item.Key);
				var rahmenItemEntity = rahmenItemEntities?.FirstOrDefault(x => x.Nr == item.Key);
				if(item.Value > rahmenItemEntity.Anzahl)
				{
					rahmenItemEntity.Anzahl = 0m;
					rahmenItemEntity.Geliefert = rahmenItemEntity.OriginalAnzahl;
				}
				else
				{
					rahmenItemEntity.Anzahl -= item.Value;
					rahmenItemEntity.Geliefert += item.Value;
				}
				rahmenItemEntity.Gesamtpreis = rahmenItemEntity.Anzahl * rahmenItemEntity.Einzelpreis;
				rahmenItemEntity.erledigt_pos = rahmenItemEntity.Geliefert != rahmenItemEntity.OriginalAnzahl ? false : true;
				_toUpdate.Add(rahmenItemEntity);
			}
			Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateWithTransaction(_toUpdate, botransaction.connection, botransaction.transaction);
			Common.Helpers.CTS.BlanketHelpers.CalculateRahmenGesamtPries(rahmenId, botransaction);
		}
	}
}