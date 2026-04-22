using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Helpers;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class UpdateBlanketPositionHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Models.Blanket.BlanketItem _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public UpdateBlanketPositionHandler(Models.Blanket.BlanketItem data, Identity.Models.UserModel user)
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

				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //
				var warnings = new List<string>();
				var positionEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetWithTransaction((int)_data.AngeboteArtiklNr, botransaction.connection, botransaction.transaction);
				var rahmenEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(positionEntity.AngebotNr ?? -1, botransaction.connection, botransaction.transaction);
				var positionExtensionEntity = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngeboteneArtikelNr(_data.AngeboteArtiklNr, botransaction.connection, botransaction.transaction);
				var RahmenExtensionEntity = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(positionExtensionEntity.RahmenNr, botransaction.connection, botransaction.transaction);
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumberWithTransaction(_data.Material?.ToLower()?.Trim(), null, botransaction.connection, botransaction.transaction);
				var wahrungenEntity = Infrastructure.Data.Access.Tables.BSD.WahrungenAccess.Get((int)_data.WahrungId);
				var itemDB = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(_data.MaterialNr ?? -1, botransaction.connection, botransaction.transaction);
				var customerDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetWithTransaction(RahmenExtensionEntity.CustomerId ?? -1, botransaction.connection, botransaction.transaction);
				var supplierEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(RahmenExtensionEntity.SupplierId ?? -1, botransaction.connection, botransaction.transaction);
				var bestellnummernEntity = Infrastructure.Data.Access.Tables.BSD.BestellnummernAccess.GetByArticleAndSupplier(itemDB.ArtikelNr, supplierEntity.Nummer ?? -1, botransaction.connection, botransaction.transaction);

				//var convertedPrice = Decimal.Multiply(_data.PreisDefault ?? 0, wahrungenEntity.Entspricht_DM ?? 0);
				//var ConvertedTotalPrice = convertedPrice * _data.Zielmenge ?? 0;

				if(positionExtensionEntity == null)
				{
					Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity()
					{
						AngeboteArtikelNr = (int)_data.AngeboteArtiklNr,
						BasePrice = _data.BasePrice,
						Bezeichnung = _data.Bezeichnung,
						GultigAb = _data.ValidFrom,
						GultigBis = _data.DateOfExpiry,
						ExtensionDate = RahmenExtensionEntity.BlanketTypeId == (int)Enums.BlanketEnums.Types.purchase? _data.DateOfExpiry: _data.ExtensionDate, // - 2025-08-14 Hejdukova remove ExtDate .Value.AddDays(90),
						Id = -1,
						KundenMatNummer = _data.KundenMatNummer,
						Material = _data.Material,
						ME = _data.ME,
						Preis = positionEntity.Einzelpreis, // - convertedPrice,
						Gesamtpreis = positionEntity.Gesamtpreis, // - ConvertedTotalPrice,
						WahrungName = _data.WahrungName,
						WahrungSymbole = _data.WahrungSymbole,
						WahrungId = _data.WahrungId,
						Zielmenge = _data.Zielmenge,
						RahmenNr = _data.AngebotNr ?? -1,
						PreisDefault = _data.PreisDefault,
						GesamtpreisDefault = _data.PreisDefault * _data.Zielmenge,
						AB_nummer = _data.ABNummer,
					}, botransaction.connection, botransaction.transaction);
					positionExtensionEntity = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngeboteneArtikelNr(_data.AngeboteArtiklNr, botransaction.connection, botransaction.transaction);
				}

				var _oldPositionBlanket = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngeboteneArtikelNr((int)_data.AngeboteArtiklNr, botransaction.connection, botransaction.transaction);
				positionEntity.Bezeichnung1 = this._data.Bezeichnung;
				positionEntity.Bezeichnung2 = this._data.KundenMatNummer;
				positionEntity.ArtikelNr = articleEntity.ArtikelNr;
				positionEntity.Index_Kunde = articleEntity.Index_Kunde;
				positionEntity.Index_Kunde_Datum = articleEntity.Index_Kunde_Datum;
				positionEntity.OriginalAnzahl = _data.Zielmenge;
				positionEntity.Anzahl = (_data.Zielmenge ?? 0) - (positionEntity.Geliefert ?? 0);
				positionEntity.Index_Kunde_Datum = articleEntity.Index_Kunde_Datum;
				positionEntity.Einheit = itemDB.Einheit;
				positionEntity.ArtikelNr = itemDB.ArtikelNr;
				positionEntity.Preiseinheit = Convert.ToDecimal(itemDB.Preiseinheit ?? 1); // - 2022-05-30 Init to 1 as to respect DB Constraint
				positionEntity.DELFixiert = itemDB.DELFixiert ?? false;
				positionEntity.DEL = itemDB.DEL ?? 0;
				positionEntity.EinzelCuGewicht = Convert.ToDecimal(itemDB.CuGewicht ?? 0);
				//
				positionEntity.USt = customerDb.Umsatzsteuer_berechnen.HasValue && customerDb.Umsatzsteuer_berechnen.Value ? 0.19m : 0m;
				positionEntity.Liefertermin = _data.DateOfExpiry;
				positionEntity.Bestellnummer = bestellnummernEntity?.Count > 0 ? bestellnummernEntity[0]?.Bestell_Nr : "";
				//positionEntity.Einzelpreis = convertedPrice;
				//positionEntity.Gesamtpreis = ConvertedTotalPrice;
				CreateBlanketElementsHandler.ComputePositionPrice(positionEntity, positionExtensionEntity, articleEntity, wahrungenEntity, _data.Zielmenge ?? 0, _data.BasePrice);

				Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateWithTransaction(positionEntity, botransaction.connection, botransaction.transaction);

				positionExtensionEntity.AngeboteArtikelNr = _data.AngeboteArtiklNr;
				positionExtensionEntity.BasePrice = _data.BasePrice;
				positionExtensionEntity.Bezeichnung = _data.Bezeichnung;
				positionExtensionEntity.GultigBis = _data.DateOfExpiry;
				positionExtensionEntity.ExtensionDate = RahmenExtensionEntity.BlanketTypeId == (int)Enums.BlanketEnums.Types.purchase ? _data.DateOfExpiry : _data.ExtensionDate;// - 2025-08-14 Hejdukova remove ExtDate ExtensionDate;
				positionExtensionEntity.KundenMatNummer = _data.KundenMatNummer;
				positionExtensionEntity.Material = _data.Material;
				positionExtensionEntity.MaterialNr = articleEntity.ArtikelNr;
				positionExtensionEntity.ME = _data.ME;
				positionExtensionEntity.Zielmenge = _data.Zielmenge;
				positionExtensionEntity.AB_nummer = _data.ABNummer;
				positionExtensionEntity.Comment = _data.Comment;

				if(RahmenExtensionEntity.StatusId != (int)Enums.BlanketEnums.RAStatus.Validated)
				{
					positionExtensionEntity.GultigAb = _data.ValidFrom;
					positionExtensionEntity.WahrungName = wahrungenEntity.Wahrung;
					positionExtensionEntity.WahrungSymbole = wahrungenEntity.Symbol;
					positionExtensionEntity.WahrungId = wahrungenEntity.Nr;
				}

				Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.UpdateWithTransaction(positionExtensionEntity, botransaction.connection, botransaction.transaction);

				if(positionExtensionEntity.Preis != positionEntity.Einzelpreis && RahmenExtensionEntity.StatusId == (int)Enums.BlanketEnums.RAStatus.Validated)
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity
					{
						AngebotNr = rahmenEntity.Angebot_Nr,
						DateTime = DateTime.Now,
						LogObject = rahmenEntity.Typ,
						LogText = $"[Rahmenauftrag] Position [{positionEntity.Position}], New price added [{positionEntity.Einzelpreis}] Valid from [{_data.ValidFrom}]",
						LogType = "MODIFICATIONPOS",
						Nr = rahmenEntity.Nr,
						Origin = "CTS",
						ProjektNr = int.TryParse(rahmenEntity.Projekt_Nr, out var v) ? v : 0,
						UserId = _user.Id,
						Username = _user.Username,
					}, botransaction.connection, botransaction.transaction);
				//rahmen price history
				if(RahmenExtensionEntity.StatusId == (int)Enums.BlanketEnums.RAStatus.Validated)
				{
					var _checkHistoryPrice = Infrastructure.Data.Access.Tables.CTS.RahmenPriceHistoryAccess.GetByPositionPriceAndDate(_data.AngeboteArtiklNr, positionEntity.Einzelpreis, _data.ValidFrom, botransaction.connection, botransaction.transaction);
					if(_checkHistoryPrice == null || _checkHistoryPrice.Count == 0)
					{
						Infrastructure.Data.Access.Tables.CTS.RahmenPriceHistoryAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity()
						{
							RahmenNr = _data.AngebotNr ?? -1,
							PositionNr = _data.AngeboteArtiklNr,
							Price = positionEntity.Einzelpreis, // - convertedPrice,
							PriceDefault = positionExtensionEntity.PreisDefault, // - _data.PreisDefault,
							WarungSymbol = wahrungenEntity.Symbol,
							ValidFrom = _data.ValidFrom,
							DateUpdate = DateTime.Now,
							UserName = _user.Name,
							BasePrice = positionExtensionEntity.BasePrice
						}, botransaction.connection, botransaction.transaction);
						//
						UpdateABPositions(_data.AngeboteArtiklNr, (DateTime)_data.ValidFrom, positionEntity.Einzelpreis ?? 0, botransaction);
					}
				}
				else
				{
					Infrastructure.Data.Access.Tables.CTS.RahmenPriceHistoryAccess.DeleteByPosition(_data.AngeboteArtiklNr, botransaction.connection, botransaction.transaction);
					Infrastructure.Data.Access.Tables.CTS.RahmenPriceHistoryAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.CTS.RahmenPriceHistoryEntity()
					{
						RahmenNr = _data.AngebotNr ?? -1,
						PositionNr = _data.AngeboteArtiklNr,
						Price = positionEntity.Einzelpreis,
						PriceDefault = positionExtensionEntity.PreisDefault,
						WarungSymbol = wahrungenEntity.Symbol,
						ValidFrom = _data.ValidFrom,
						DateUpdate = DateTime.Now,
						UserName = _user.Name,
						BasePrice = positionExtensionEntity.BasePrice
					}, botransaction.connection, botransaction.transaction);
				}

				//updating sum price
				Common.Helpers.CTS.BlanketHelpers.CalculateRahmenGesamtPries(_data.AngebotNr ?? -1, botransaction);
				//logging
				var _Logs = GetLog(positionExtensionEntity, _oldPositionBlanket, _user, botransaction);
				if(_Logs != null && _Logs.Count > 0)
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(_Logs, botransaction.connection, botransaction.transaction);

				#endregion // -- transaction-based logic -- //


				// -handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return new ResponseModel<int>
					{
						Success = true,
						Errors = null,
						Warnings = warnings,
						Infos = null,
						Body = 1
					};
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

			if(_user == null || (!_user.Access.CustomerService.ModuleActivated && !_user.Access.Purchase.ModuleActivated))
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var rahmenEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data.AngebotNr ?? -1);
			if(rahmenEntity == null)
				return ResponseModel<int>.FailureResponse($"Rahmen not found");

			if(_data.Material == null)
				return ResponseModel<int>.FailureResponse($"Item not found");

			var itemDb = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(_data.Material?.ToLower()?.Trim());
			if(itemDb == null)
				return ResponseModel<int>.FailureResponse($"Article not found");

			if(_data.Zielmenge == null || _data.Zielmenge < 0)
				return ResponseModel<int>.FailureResponse($"Zielmenge " + _data.Zielmenge + " is invalid");
			if(_data.BasePrice <= 0)
				return ResponseModel<int>.FailureResponse($"Preis " + _data.BasePrice + " is invalid");
			if(_data.WahrungId == null || _data.WahrungId <= 0)
				return ResponseModel<int>.FailureResponse($"Wharung " + _data.WahrungId + " is invalid");
			if(_data.ValidFrom == null)
				return ResponseModel<int>.FailureResponse($"DateStart " + _data.ValidFrom + " is invalid");
			if(_data.DateOfExpiry == null)
				return ResponseModel<int>.FailureResponse($"DateEnd " + _data.DateOfExpiry + " is invalid");
			if(_data.ValidFrom >= _data.DateOfExpiry)
				return ResponseModel<int>.FailureResponse($"DateStart [{_data.ValidFrom.Value.ToString("dd/MM/yyyy")}] is greater than DateEnd [{_data.DateOfExpiry.Value.ToString("dd/MM/yyyy")}]");



			///---Test AB Ra Pos
			var ListpositionsAb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetAbByRahmenPosition(_data.AngeboteArtiklNr);
			var listPositionsBS = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.GetbyRahmenPositions(new List<int> { _data.AngeboteArtiklNr });
			var oldPositionRA = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get((int)_data.AngeboteArtiklNr);

			if(ListpositionsAb != null && ListpositionsAb.Count > 0)
			{
				if(_data.MaterialNr != oldPositionRA.ArtikelNr)
					return ResponseModel<int>.FailureResponse($"There are AB position(s) linked to this Position No Update Artikel Allowed!");
				if(_data.Zielmenge < oldPositionRA.Anzahl)
					return ResponseModel<int>.FailureResponse($"The Original Quantity [{_data.Zielmenge}] must not be greater then the Rest Quantity [{oldPositionRA.Anzahl}]");
				if(_data.DateOfExpiry < DateTime.Today)
					return ResponseModel<int>.FailureResponse($"End Date [{_data.DateOfExpiry.Value.ToString("dd/MM/yyyy")}] must be greater then Current Date [{DateTime.Today.ToString("dd/MM/yyyy")}]");
			}

			var rahmenPosExtension = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngeboteneArtikelNr(_data.AngeboteArtiklNr);
			var rahmenExtension = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(_data.AngebotNr ?? -1);
			if(rahmenExtension.BlanketTypeId == (int)Enums.BlanketEnums.Types.purchase && listPositionsBS != null && listPositionsBS.Count > 0)
			{
				if(_data.MaterialNr != oldPositionRA.ArtikelNr)
					return ResponseModel<int>.FailureResponse($"There are orders position(s) linked to this Position,Artikel update not allowed!");
				if(_data.PreisDefault != oldPositionRA.Einzelpreis)
					return ResponseModel<int>.FailureResponse($"There are orders position(s) linked to this Position,price update not allowed!");
				// - 2025-08-14 Hejdukova remove ExtDate 
				if(_data.DateOfExpiry != rahmenPosExtension.GultigBis && listPositionsBS.Exists(x=> x.Liefertermin > _data.DateOfExpiry))
					return ResponseModel<int>.FailureResponse($"There are orders position(s) linked to this Position, Date value not allowed before [{listPositionsBS.Max(x=>x.Liefertermin).Value.ToString("dd/MM/yyyy")}]!");
			}
			if(rahmenExtension.StatusId != (int)Enums.BlanketEnums.RAStatus.Validated && rahmenExtension.StatusId != (int)Enums.BlanketEnums.RAStatus.Draft)
				return ResponseModel<int>.FailureResponse("Update allowed only in Draft or Validated Satatus.");
			if(rahmenExtension.StatusId == (int)Enums.BlanketEnums.RAStatus.Validated && (_data.ValidFrom < rahmenPosExtension.GultigAb || _data.ValidFrom > rahmenPosExtension.GultigBis))
				return ResponseModel<int>.FailureResponse("Valid From date should be in current position dates range .");

			if(rahmenExtension.BlanketTypeId == (int)Enums.BlanketEnums.Types.sale)
			{
				var _newDate = _data.DateOfExpiry ?? new DateTime(1900, 1, 1);
				var _oldDate = rahmenPosExtension.GultigBis ?? new DateTime(1900, 1, 1);
				var horizonCheck = Helpers.HorizonsHelper.userHasRAPosHorizonRight(_newDate, _oldDate, _user, out List<string> messages);
				if(!horizonCheck)
					return ResponseModel<int>.FailureResponse(messages);
			}
			else
			{
				// - 2023-11-13 - check if has orders
				var orderedQty = listPositionsBS.Select(x => x.Anzahl ?? 0).Sum();
				if(listPositionsBS?.Count > 0 && orderedQty > this._data.Zielmenge)
				{
					return ResponseModel<int>.FailureResponse($"Cannot update quantity to {_data.Zielmenge} because {orderedQty} has already been order!");
				}
				// - RA-Purchase make sure price is smaller or equal to std
				var bestellnummernEntity = Infrastructure.Data.Access.Tables.MTM.BestellnummernAccess.GetBySupplierIdArticleId(rahmenEntity.Kunden_Nr ?? 0, itemDb.ArtikelNr);
				if((bestellnummernEntity?.Einkaufspreis ?? 0) < _data.BasePrice)
				{
					return ResponseModel<int>.FailureResponse($"Price [{_data.BasePrice}] cannot be bigger than standard price [{bestellnummernEntity?.Einkaufspreis ?? 0}].");
				}
			}

			return ResponseModel<int>.SuccessResponse();
		}
		internal static List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity> GetLog(Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity _new,
		  Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity _old, Core.Identity.Models.UserModel user, Infrastructure.Services.Utils.TransactionsManager botransaction)
		{
			List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity> _logs = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();
			var angebotArtikelPosition = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetWithTransaction(_old.AngeboteArtikelNr, botransaction.connection, botransaction.transaction);
			var rahmenEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(_old.RahmenNr, botransaction.connection, botransaction.transaction);
			var _toLog = new LogHelper(rahmenEntity.Nr, (int)rahmenEntity.Angebot_Nr, int.TryParse(rahmenEntity.Projekt_Nr, out var v) ? v : 0, "Rahmenauftrag", LogHelper.LogType.MODIFICATIONPOS, "CTS", user);
			if(_old.Bezeichnung != _new.Bezeichnung)
			{
				_logs.Add(_toLog.LogCTS("Bezeichnung", _old.Bezeichnung.ToString(), _new.Bezeichnung.ToString(), (int)angebotArtikelPosition.Position, angebotArtikelPosition.Nr));
			}
			if(_old.Preis != _new.Preis)
			{
				_logs.Add(_toLog.LogCTS("Preis", _old.Preis.ToString(), _new.Preis.ToString(), (int)angebotArtikelPosition.Position, angebotArtikelPosition.Nr));
			}
			if(_old.GultigAb != _new.GultigAb)
			{
				_logs.Add(_toLog.LogCTS("GultigAb", _old.GultigAb.ToString(), _new.GultigAb.ToString(), (int)angebotArtikelPosition.Position, angebotArtikelPosition.Nr));
			}
			if(_old.GultigBis != _new.GultigBis)
			{
				_logs.Add(_toLog.LogCTS("GultigBis", _old.GultigBis.ToString(), _new.GultigBis.ToString(), (int)angebotArtikelPosition.Position, angebotArtikelPosition.Nr));
			}
			if(_old.KundenMatNummer != _new.KundenMatNummer)
			{
				_logs.Add(_toLog.LogCTS("KundenMatNummer", _old.KundenMatNummer.ToString(), _new.KundenMatNummer.ToString(), (int)angebotArtikelPosition.Position, angebotArtikelPosition.Nr));
			}
			if(_old.Material != _new.Material)
			{
				_logs.Add(_toLog.LogCTS("Material", _old.Material.ToString(), _new.Material.ToString(), (int)angebotArtikelPosition.Position, angebotArtikelPosition.Nr));
			}
			if(_old.ME != _new.ME)
			{
				_logs.Add(_toLog.LogCTS("ME", _old.ME.ToString(), _new.ME.ToString(), (int)angebotArtikelPosition.Position, angebotArtikelPosition.Nr));
			}
			if(_old.WahrungName != _new.WahrungName)
			{
				_logs.Add(_toLog.LogCTS("Wahrung", _old.WahrungName.ToString(), _new.WahrungName.ToString(), (int)angebotArtikelPosition.Position, angebotArtikelPosition.Nr));
			}
			if(_old.Zielmenge != _new.Zielmenge)
			{
				_logs.Add(_toLog.LogCTS("Zielmenge", _old.Zielmenge.ToString(), _new.Zielmenge.ToString(), (int)angebotArtikelPosition.Position, angebotArtikelPosition.Nr));
			}
			return _logs;
		}
		public static void UpdateABPositions(int raPositionNr, DateTime date, decimal price, Infrastructure.Services.Utils.TransactionsManager botransaction)
		{
			var raPosition = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetWithTransaction(raPositionNr, botransaction.connection, botransaction.transaction);
			var raPositionExtension = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngeboteneArtikelNr(raPositionNr, botransaction.connection, botransaction.transaction);
			var abPositionsToUpdateCalculated = new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();

			var abPositionsToUpdate = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetABBeforPreisUpdate(0, raPositionNr, date, botransaction.connection, botransaction.transaction);
			if(abPositionsToUpdate != null && abPositionsToUpdate.Count > 0)
			{
				foreach(var item in abPositionsToUpdate)
				{
					var calulatedPosition = Helpers.BlanketHelper.GetCalculatedPositon(item.Nr, item.OriginalAnzahl ?? 0, true, price, raPositionNr);
					abPositionsToUpdateCalculated.Add(calulatedPosition);
				}
				Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateWithTransaction(abPositionsToUpdateCalculated, botransaction.connection, botransaction.transaction);
			}
		}
	}
}