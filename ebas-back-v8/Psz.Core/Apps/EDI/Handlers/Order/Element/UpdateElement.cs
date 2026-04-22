using iText.Layout.Properties;
using Psz.Core.CustomerService.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Order
	{
		public static Core.Models.ResponseModel<object> UpdateElement(Models.Order.UpdateElementModel data,
			Core.Identity.Models.UserModel user)
		{
			lock(Locks.OrderElementsLock.GetOrAdd(data.Id, new object()))
			{
				try
				{
					if(user == null || (!user.Access.Purchase.ModuleActivated && !user.Access.CustomerService.ModuleActivated))
					{
						throw new Core.Exceptions.UnauthorizedException();
					}

					var orderPosition = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(data.Id);
					if(orderPosition == null)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Position not found" }
						};
					}
					var itemDb = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(data.ItemNumber);
					if(itemDb == null)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Item not found" }
						};
					}
					if(itemDb.Freigabestatus.ToUpper() == "O")
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Item is 'Obsolete'" }
						};
					}
					var itemWsamePositionNr = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNrAndPosition(data.Id, data.OrderId, data.PositionNumber);
					if(itemWsamePositionNr != null && itemWsamePositionNr.Count > 0)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { $"Position [{data.PositionNumber}] already exists in Order" }
						};
					}
					var ItemOrder = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(data.OrderId);
					if(ItemOrder != null)
					{
						if(ItemOrder.Gebucht.HasValue && ItemOrder.Gebucht.Value && !user.SuperAdministrator)
							return new Core.Models.ResponseModel<object>()
							{
								Errors = new List<string>() { $"Order is Booked, modifications impossible." }
							};
						if(ItemOrder.Erledigt.HasValue && ItemOrder.Erledigt.Value && !user.SuperAdministrator)
							return new Core.Models.ResponseModel<object>()
							{
								Errors = new List<string>() { $"Order is Done, modifications impossible." }
							};
					}
					//horizon check
					var horizonCheck = false;
					var horizonErrors = new List<string>();
					//var technicArticles = Program.BSD.TechnicArticleIds;
					var orderArticleIsTechnic = CustomerService.Helpers.HorizonsHelper.ArticleIsTechnic(orderPosition.ArtikelNr ?? -1);
					if(orderPosition.Liefertermin != data.DeliveryDate || itemDb.ArtikelNummer != data.ItemNumber || orderPosition.VKEinzelpreis != data.UnitPrice
						|| orderPosition.EKPreise_Fix != data.FixedPrice || orderPosition.VKFestpreis != data.FixedTotalPrice || orderPosition.USt != data.VAT)
					{
						var _newDate = data.DeliveryDate ?? data.DesiredDate ?? new DateTime(1900, 1, 1);
						var _oldDate = orderPosition.Liefertermin ?? orderPosition.Wunschtermin ?? new DateTime(1900, 1, 1);
						switch(ItemOrder.Typ.Trim().ToLower())
						{
							case Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CONFIRMATION:
								horizonCheck = Psz.Core.CustomerService.Helpers.HorizonsHelper.userHasABPosHorizonRight(_newDate, _oldDate, user, out List<string> messages_1);
								horizonErrors.AddRange(messages_1 ?? new List<string> { });
								break;
							case Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CREDIT:
								horizonCheck = Psz.Core.CustomerService.Helpers.HorizonsHelper.userHasGSPosHorizonRight(_oldDate, user, out List<string> messages_2);
								horizonErrors.AddRange(messages_2 ?? new List<string> { });
								break;
							case Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_FORECAST:
								horizonCheck = Psz.Core.CustomerService.Helpers.HorizonsHelper.userHasFRCPosHorizonRight(_newDate, _oldDate, user, out List<string> messages_3);
								horizonErrors.AddRange(messages_3 ?? new List<string> { });
								break;
							case Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_DELIVERY:
								horizonCheck = Psz.Core.CustomerService.Helpers.HorizonsHelper.userHasLSPosHorizonRight(_newDate, _oldDate, user, out List<string> messages_4);
								horizonErrors.AddRange(messages_4 ?? new List<string> { });
								break;

						}
					}
					if(horizonErrors != null && horizonErrors.Count > 0 && !orderArticleIsTechnic)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = horizonErrors
						};
					}

					Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity _oldAbPosition;
					List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> rahmenPositionsToUpdate;
					#region Apply RA 
					_oldAbPosition = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(data.Id);
					if(_oldAbPosition.ABPoszuRAPos.HasValue && _oldAbPosition.ABPoszuRAPos.Value != 0 && _oldAbPosition.ABPoszuRAPos.Value != -1)
					{
						var RAPositionExtension = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngeboteneArtikelNr(_oldAbPosition.ABPoszuRAPos ?? -1);
						if(RAPositionExtension != null && RAPositionExtension.GultigAb.HasValue && RAPositionExtension.GultigBis.HasValue)
						{
							if(data.DeliveryDate.HasValue && (data.DeliveryDate < RAPositionExtension.GultigAb))
								return new Core.Models.ResponseModel<object>()
								{
									Errors = new List<string>() { $"Delivery date should be in Rahmen dates range [{RAPositionExtension.GultigAb?.ToString("dd/MM/yyyy")}]." }
								};
						}
					}
					//choosing rahmen
					rahmenPositionsToUpdate = new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
					var warnings = new List<string>();
					//var aBPosRahmenList = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetbyRahmenPosition(data.RahmenPosId ?? -1) ?? new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
					if(data.RahmenPosId.HasValue && data.RahmenPosId > 0)
					{
						if(_oldAbPosition.Geliefert > 0 && data.RahmenPosId != _oldAbPosition.ABPoszuRAPos)
							return new Core.Models.ResponseModel<object>()
							{
								Errors = new List<string>() { $"Ordred has delivred quantity, cannot apply Rahmen." }
							};
						if(_oldAbPosition != null && _oldAbPosition.ABPoszuRAPos.HasValue && _oldAbPosition.ABPoszuRAPos.Value != 0 && _oldAbPosition.ABPoszuRAPos.Value != -1)
						{
							var rahmenPosition = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(_oldAbPosition.ABPoszuRAPos ?? -1);
							//choosing same rahmen
							if(rahmenPosition != null && rahmenPosition.Nr == data.RahmenPosId)
							{
								// - Rahme.OriginalQty >= Sum(aBPosRahmenList) + NewQty - OldQty
								var availableQty = CalculateRahmenVailableQty(rahmenPosition.Nr, _oldAbPosition.Nr);
								if(data.OrderedQuantity != _oldAbPosition.OriginalAnzahl && data.OrderedQuantity > availableQty)
									return new Core.Models.ResponseModel<object>()
									{
										Errors = new List<string>() { $"Ordred quantity [{data.OrderedQuantity}] is bigger then rahmen available quantity [{availableQty}]" }
									};
								//return old qty
								rahmenPosition.Anzahl += _oldAbPosition.OriginalAnzahl;
								rahmenPosition.Geliefert -= _oldAbPosition.OriginalAnzahl;
								//take new qty
								rahmenPosition.Anzahl -= data.OrderedQuantity;
								rahmenPosition.Geliefert += data.OrderedQuantity;
								data.ItemNumber = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(rahmenPosition.ArtikelNr ?? -1)?.ArtikelNummer;

								//get right price from rahmen prices history         
								var rahmenPriceToApply = rahmenPosition.VKEinzelpreis;
								if(data.DeliveryDate.HasValue || data.DesiredDate.HasValue)
								{
									var rahmenPrices = Infrastructure.Data.Access.Tables.CTS.RahmenPriceHistoryAccess.GetByMaxPriceAndDate(rahmenPosition.Nr, data.DeliveryDate ?? data.DesiredDate);
									var rightPrice = rahmenPrices?[0].BasePrice;
									if(rightPrice != null)
										rahmenPriceToApply = rightPrice;
								}

								data.UnitPrice = rahmenPriceToApply ?? 0m;
								data.FixedPrice = true;
								rahmenPositionsToUpdate.Add(rahmenPosition);

							}
							// choosing diffrent rahmen
							else if(rahmenPosition != null && rahmenPosition.Nr != data.RahmenPosId)
							{
								// - Rahme.OriginalQty >= Sum(aBPosRahmenList) + NewQty - OldQty
								var newRahmenPosition = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(data.RahmenPosId ?? -1);
								var availableQty = CalculateRahmenVailableQty(newRahmenPosition.Nr, _oldAbPosition.Nr);
								if(data.OrderedQuantity > availableQty)
									return new Core.Models.ResponseModel<object>()
									{
										Errors = new List<string>() { $"Ordred quantity [{data.OrderedQuantity}] is bigger then rahmen available quantity [{availableQty}]" }
									};
								//return old qty
								rahmenPosition.Anzahl += _oldAbPosition.OriginalAnzahl;
								rahmenPosition.Geliefert -= _oldAbPosition.OriginalAnzahl;
								//take new qty
								newRahmenPosition.Anzahl -= data.OrderedQuantity;
								newRahmenPosition.Geliefert += data.OrderedQuantity;

								rahmenPositionsToUpdate.Add(rahmenPosition);
								rahmenPositionsToUpdate.Add(newRahmenPosition);
								data.ItemNumber = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(newRahmenPosition.ArtikelNr ?? -1)?.ArtikelNummer;

								//get right price from rahmen prices history
								var rahmenPriceToApply = newRahmenPosition.VKEinzelpreis;
								if(data.DeliveryDate.HasValue || data.DesiredDate.HasValue)
								{
									var rahmenPrices = Infrastructure.Data.Access.Tables.CTS.RahmenPriceHistoryAccess.GetByMaxPriceAndDate(newRahmenPosition.Nr, data.DeliveryDate ?? data.DesiredDate);
									var rightPrice = rahmenPrices?[0].BasePrice;
									if(rightPrice != null)
										rahmenPriceToApply = rightPrice;
								}
								data.UnitPrice = rahmenPriceToApply ?? 0m;
								data.FixedPrice = true;
							}
						}
						else
						{
							// - Old AB Position w/o RA
							var newRahmenPosition = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(data.RahmenPosId ?? -1);
							var availableQty = CalculateRahmenVailableQty(newRahmenPosition.Nr, _oldAbPosition.Nr);
							//var XX = (newRahmenPosition.Anzahl) - (aBPosRahmenList?.Sum(x => x.Anzahl) ?? 0) + (_oldAbPosition.Anzahl ?? 0); if (data.OrderedQuantity > availableQty/*newRahmenPosition.Anzahl*/)
							if(data.OrderedQuantity > availableQty)
								return new Core.Models.ResponseModel<object>()
								{
									Errors = new List<string>() { $"Ordred quantity [{data.OrderedQuantity}] is bigger then rahmen available quantity [{availableQty}]" }
								};
							//take new qty
							newRahmenPosition.Anzahl -= data.OrderedQuantity;
							newRahmenPosition.Geliefert += data.OrderedQuantity;
							data.ItemNumber = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(newRahmenPosition.ArtikelNr ?? -1)?.ArtikelNummer;
							//get right price from rahmen prices history
							var rahmenPriceToApply = newRahmenPosition.VKEinzelpreis;
							if(data.DeliveryDate.HasValue || data.DesiredDate.HasValue)
							{
								var rahmenPrices = Infrastructure.Data.Access.Tables.CTS.RahmenPriceHistoryAccess.GetByMaxPriceAndDate(newRahmenPosition.Nr, data.DeliveryDate ?? data.DesiredDate);
								var rightPrice = rahmenPrices?[0].BasePrice;
								if(rightPrice != null)
									rahmenPriceToApply = rightPrice;
							}

							data.UnitPrice = rahmenPriceToApply ?? 0m;
							data.FixedPrice = true;
							rahmenPositionsToUpdate.Add(newRahmenPosition);
						}
					}
					else // not choosing rahmen
					{
						if(_oldAbPosition.Geliefert > 0 && data.RahmenPosId != _oldAbPosition.ABPoszuRAPos)
							return new Core.Models.ResponseModel<object>()
							{
								Errors = new List<string>() { $"Order has delivred quantity, cannot apply/change Rahmen." }
							};
						// - Old AB Pos has RA link
						if(_oldAbPosition != null && _oldAbPosition.ABPoszuRAPos.HasValue && _oldAbPosition.ABPoszuRAPos.Value > 0)
						{
							// - Delete link to RA -- return Qty to RA
							var rahmenPosition = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(_oldAbPosition.ABPoszuRAPos ?? -1);
							rahmenPosition.Anzahl += _oldAbPosition.OriginalAnzahl;
							rahmenPosition.Geliefert -= _oldAbPosition.OriginalAnzahl;
							//data.UnitPriceBasis = _oldAbPosition.VKEinzelpreis ?? 0m;
							data.UnitPrice = _oldAbPosition.VKEinzelpreis ?? 0m;
							data.FixedPrice = false;
							rahmenPositionsToUpdate.Add(rahmenPosition);
						}
						else
						{ // - Old AB Pos does not have RA Link
						  // - DO NOTHING
						}
					}

					#endregion Apply RA

					var orderElementDbResponse = CalculateOrderElement(data);

					if(!orderElementDbResponse.Success)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = orderElementDbResponse.Errors
						};
					}
					Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Update(orderElementDbResponse.Body);
					if(rahmenPositionsToUpdate != null && rahmenPositionsToUpdate.Count > 0)
					{
						Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Update(rahmenPositionsToUpdate);
						Helpers.CalculationHelper.CalculateRahmenGesamtPries(rahmenPositionsToUpdate.Select(x => x.AngebotNr ?? -1).ToList());
					}
					//if(ChoiceUpdate == true)
					//{

					//}
					//logging
					//var _Logs = GetLog(orderElementDbResponse.Body, _oldAbPosition, user);
					//if(_Logs != null && _Logs.Count > 0)
					//{
					//	Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_Logs);
					//}
					var logs = SharedKernel.Extensions.SharedExtensions.CompareObjects(_oldAbPosition, orderElementDbResponse.Body, $"AB Position {_oldAbPosition.Position}");
					if(logs?.Count > 0)
					{
						var logHelper = new LogHelper(ItemOrder.Nr, (int)ItemOrder.Angebot_Nr, int.TryParse(ItemOrder.Projekt_Nr, out var val) ? val : 0, ItemOrder.Typ, LogHelper.LogType.MODIFICATIONPOS, "CTS", user);
						Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(
							logs.Select(x => logHelper.LogCTS(x, LogHelper.LogType.MODIFICATIONPOS, _oldAbPosition.Nr)).ToList());
					}
					OrderElementExtension.SetStatus(orderElementDbResponse.Body.Nr);

					var updated = GetElement(orderElementDbResponse.Body.Nr);
					return new Core.Models.ResponseModel<object>
					{
						Body = updated,
						Success = true,
						Warnings = orderElementDbResponse.Warnings
					};
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		internal static Core.Models.ResponseModel<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> CalculateOrderElement(Models.Order.UpdateElementModel data)
		{
			var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(data.OrderId);
			if(orderDb == null)
			{
				return new Core.Models.ResponseModel<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>()
				{
					Errors = new List<string>() { "Order not found" }
				};
			}


			var orderElementDb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(data.Id);
			#region default inits
			////orderElementDb.OriginalAnzahl = (decimal)data.OrderedQuantity;
			////orderElementDb.Typ = data.ItemTypeId;
			////orderElementDb.Position = data.PositionNumber;
			////orderElementDb.Wunschtermin = orderElementDb.Wunschtermin;
			////orderElementDb.Anzahl = (decimal)data.OrderedQuantity;
			////orderElementDb.AktuelleAnzahl = (decimal)data.OrderedQuantity;
			////orderElementDb.Abladestelle = data.UnloadingPoint;
			orderElementDb.Freies_Format_EDI = data.FreeText;
			orderElementDb.Bemerkungsfeld1 = data.Note1;
			orderElementDb.Bemerkungsfeld2 = data.Note2;
			////orderElementDb.Bezeichnung1 = data.Designation1;
			////orderElementDb.Bezeichnung2 = data.Designation2;
			////orderElementDb.Einheit = data.MeasureUnitQualifier;
			orderElementDb.DELFixiert = data.DelFixed; // itemDb.DELFixiert;
			orderElementDb.DEL = data.DelNote; // data.DelFixed.HasValue && !data.DelFixed.Value ? data.DelNote: itemDb.DEL; // itemDb.DEL;
											   ////orderElementDb.USt = (decimal)data.VAT; //itemDb.Umsatzsteuer;
											   ////orderElementDb.RP = data.RP;
			orderElementDb.EKPreise_Fix = data.FixedPrice;
			orderElementDb.POSTEXT = data.Postext;
			////orderElementDb.Index_Kunde = data.Index_Kunde;
			////orderElementDb.Index_Kunde_Datum = data.Index_Kunde_Datum;
			////orderElementDb.Rabatt = data.Discount;
			orderElementDb.CSInterneBemerkung = data.CSInterneBemerkung;
			////orderElementDb.ABPoszuRAPos = data.RahmenId;
			#endregion default inits

			if(orderElementDb == null)
			{
				return new Core.Models.ResponseModel<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>()
				{
					Errors = new List<string>() { "Order Element not found" }
				};
			}

			//  - 2023-10-26 - Heidenreich - DO NOT update Price when already partially delivered
			if(orderElementDb.OriginalAnzahl - orderElementDb.Anzahl > 0)
			{
				return new Core.Models.ResponseModel<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>()
				{
					Success = true,
					Body = orderElementDb
				};
			}

			if(orderElementDb.AngebotNr != orderDb.Nr)
			{
				return new Core.Models.ResponseModel<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>()
				{
					Errors = new List<string>() { "Element is not Element of Order" }
				};
			}

			var customerDb = orderDb.Kunden_Nr.HasValue
				? Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(orderDb.Kunden_Nr.Value)
				: null;
			if(customerDb == null)
			{
				return new Core.Models.ResponseModel<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>()
				{
					Errors = new List<string>()
						{
							"Customer not found"
						}
				};
			}

			var itemDb = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(data.ItemNumber);
			if(itemDb == null)
			{
				return new Core.Models.ResponseModel<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>()
				{
					Errors = new List<string>() { "Item not found" }
				};
			}
			if(itemDb.Freigabestatus.ToUpper() == "O")
			{
				return new Core.Models.ResponseModel<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>()
				{
					Errors = new List<string>() { "Item is 'Obsolete'" }
				};
			}

			Infrastructure.Data.Entities.Tables.INV.LagerorteEntity storageLocationDb = null;
			if(data.StorageLocationId > 0)
			{
				storageLocationDb = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get(data.StorageLocationId);
				if(storageLocationDb == null)
				{
					return new Core.Models.ResponseModel<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>()
					{
						Errors = new List<string>() { "Storage Location not found" }
					};
				}
			}
						
			var errors = new List<string>();
			var warnings = new List<string>();

			if(data.UnitPriceBasis <= 0)
			{
				errors.Add("UnitPriceBasis " + data.UnitPriceBasis + " is invalid");
			}

			if(data.OrderedQuantity < 0)
			{
				errors.Add("Ordered Quantity " + data.OrderedQuantity + " is invalid");
			}

			if(orderElementDb.Anzahl != (decimal?)data.OrderedQuantity && data.OrderedQuantity == 0)
			{
				warnings.Add("Ordered Quantity is 0!");
				warnings.Add("The position will be send as order annulation.");
			}
			var _oldPosition = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(data.Id);

			if(_oldPosition.ABPoszuRAPos.HasValue && _oldPosition.ABPoszuRAPos.Value != 0 && _oldPosition.ABPoszuRAPos.Value != -1)
			{
				var rahmenPosition = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get((int)_oldPosition.ABPoszuRAPos);
				if(rahmenPosition == null)
				{
					warnings.Add("Ordered Quantity is 0!");
					warnings.Add("The rahmen position have been deleted");
				}
			}

			// >>> Update LINKED Positions
			#region >>> Update LINKED Positions
			if(orderElementDb.PositionZUEDI != null && (int)orderElementDb.PositionZUEDI > 0)
			{
				var originalPosition = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get((int)orderElementDb.PositionZUEDI);
				if(originalPosition == null)
				{
					errors.Add("Original Position " + orderElementDb.PositionZUEDI + " is not found");
				}
				else
				{
					if(originalPosition.ArtikelNr != orderElementDb.ArtikelNr)
					{
						errors.Add("Split Position should have same Article as Original.");
					}
					// originalPosition.AktuelleAnzahl = originalPosition.AktuelleAnzahl - (decimal)data.OrderedQuantity;
					// Infrastructure.Data.Access.Tables.EDI.AngeboteneArtikelAccess.Update(originalPosition);
				}
			}
			#endregion >>> Update LINKED Positions

			if(errors.Count > 0)
			{
				return new Core.Models.ResponseModel<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>()
				{
					Errors = errors
				};
			}
			var posAB = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(data.Id);
			var extension = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngeboteneArtikelNr(posAB.ABPoszuRAPos ?? -1);
			if(extension != null)
			{
				// - 2025-08-14 Hejdukova remove ExtDate 
				if(data.DeliveryDate > extension.GultigBis)
				{
					errors.Add($" invalid Lifertermin should be before ExpiryDate");
				}
				if(data.DesiredDate > extension.GultigBis)
				{
					errors.Add($" invalid Wunshtermin should be before ExpiryDate");
				}
			}
			var itemPricingGroupDb = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(itemDb.ArtikelNr);

			//var conditionAssignementTableDb = customerDb.Konditionszuordnungs_Nr.HasValue
			//    ? Infrastructure.Data.Access.Tables.PRS.KonditionsZuordnungstabelleEntity.Get(customerDb.Konditionszuordnungs_Nr.Value)
			//    : null;

			var discount = data.Discount;
			var fixedPrice = data.FixedTotalPrice;
			var unitPriceBasis = data.UnitPriceBasis;
			var cuWeight = Convert.ToDecimal(itemDb.CuGewicht ?? 0);
			var del = (data.DelNote ?? 0); //  (itemDb.DEL ?? 0);

			var me1 = 0m;
			var me2 = 0m;
			var me3 = 0m;
			var me4 = 0m;
			var pm1 = 0m;
			var pm2 = 0m;
			var pm3 = 0m;
			var pm4 = 0m;
			var verkaufspreis = 0m;
			var kupferbasis = data.CopperBase;

			if(itemPricingGroupDb != null)
			{
				me1 = Convert.ToDecimal(itemPricingGroupDb.ME1 ?? 0m);
				me2 = Convert.ToDecimal(itemPricingGroupDb.ME2 ?? 0m);
				me3 = Convert.ToDecimal(itemPricingGroupDb.ME3 ?? 0m);
				me4 = Convert.ToDecimal(itemPricingGroupDb.ME4 ?? 0m);
				pm1 = Convert.ToDecimal(itemPricingGroupDb.PM1 ?? 0m);
				pm2 = Convert.ToDecimal(itemPricingGroupDb.PM2 ?? 0m);
				pm3 = Convert.ToDecimal(itemPricingGroupDb.PM3 ?? 0m);
				pm4 = Convert.ToDecimal(itemPricingGroupDb.PM4 ?? 0m);
				verkaufspreis = Convert.ToDecimal(itemPricingGroupDb.Verkaufspreis ?? 0m);
			}
			// - 2023-09-29 - Reil price according to article type (Prototype, FirstSample, Serie, NullSerie)
			if(data.ItemTypeId != (int)CustomerService.Enums.OrderEnums.ItemType.Serie)
			{
				if(data.ItemTypeId is null || !Enum.IsDefined(typeof(CustomerService.Enums.OrderEnums.ItemType), data.ItemTypeId))
				{
					errors.Add($"Invalid value [{data.ItemTypeId}] for field [Type]");
					return new Core.Models.ResponseModel<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>()
					{
						Errors = errors
					};
				}
				var articleType = Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.GetByArticleNrAndType(new List<int>() { itemDb.ArtikelNr }, (int)Common.Enums.ArticleEnums.GetItemType(((CustomerService.Enums.OrderEnums.ItemType)(data.ItemTypeId)).GetDescription()));
				if(articleType?.Count <= 0 || articleType?[0].Verkaufspreis is null)
				{
					errors.Add($"Sales price for type [{(CustomerService.Enums.OrderEnums.ItemType)data.ItemTypeId}] not found");
					return new Core.Models.ResponseModel<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>()
					{
						Errors = errors
					};
				}
				else
				{
					verkaufspreis = Convert.ToDecimal(articleType[0].Verkaufspreis);
				}

				
			}
			// - Einzelkupferzuschlag
			var singleCopperSurcharge = Helpers.CalculationHelper.CalculateSingleCopperSurcharge(fixedPrice,
				del,
				cuWeight,
				kupferbasis);

			// - Gesamtkupferzuschlag
			var totalCopperSurcharge = Helpers.CalculationHelper.CalculateTotalCopperSurcharge(fixedPrice,
				data.OrderedQuantity,
				singleCopperSurcharge);

			// - VKEinzelpreis
			var vkUnitPrice = data.FixedPrice ? data.UnitPrice : Helpers.CalculationHelper.CalculateVkUnitPrice(data.ItemTypeId == (int)CustomerService.Enums.OrderEnums.ItemType.Serie,
				fixedPrice,
				verkaufspreis,
				data.OrderedQuantity,
				me1,
				me2,
				me3,
				me4,
				pm2,
				pm3,
				pm4);

			// - Einzelpreis
			var unitPrice = Helpers.CalculationHelper.CalculateUnitPrice(data.ItemTypeId == (int)CustomerService.Enums.OrderEnums.ItemType.Serie, 
				data.FixedPrice, //fixedPrice,
				unitPriceBasis,
				data.OrderedQuantity,
				data.FixedPrice ? vkUnitPrice + singleCopperSurcharge : vkUnitPrice,
				verkaufspreis,
				singleCopperSurcharge,
				me1,
				me2,
				me3,
				me4,
				pm2,
				pm3,
				pm4);

			// - Gesamtpreis
			var totalPrice = Helpers.CalculationHelper.CalculateTotalPrice(unitPriceBasis,
				unitPrice,
				data.OrderedQuantity,
				discount);

			// - VKGesamtpreis
			var vKTotalPrice = Helpers.CalculationHelper.CalculateVkTotalPrice(unitPriceBasis,
				vkUnitPrice,
				data.OrderedQuantity);

			// - GesamtCuGewicht
			var totalCuWeight = Helpers.CalculationHelper.CalculateTotalWeight(data.OrderedQuantity,
				cuWeight);
			orderElementDb.OriginalAnzahl = (decimal)data.OrderedQuantity;
			orderElementDb.Typ = data.ItemTypeId;
			orderElementDb.Position = data.PositionNumber;
			orderElementDb.Wunschtermin = orderElementDb.Wunschtermin;
			orderElementDb.Anzahl = (decimal)data.OrderedQuantity;
			orderElementDb.AktuelleAnzahl = (decimal)data.OrderedQuantity;
			orderElementDb.Abladestelle = data.UnloadingPoint;
			//orderElementDb.Bezeichnung2_Kunde = itemDb.Bezeichnung2;
			orderElementDb.Freies_Format_EDI = data.FreeText;
			orderElementDb.Bemerkungsfeld1 = data.Note1;
			orderElementDb.Bemerkungsfeld2 = data.Note2;
			orderElementDb.Bezeichnung1 = data.Designation1;
			//itemDb.Bezeichnung1;
			orderElementDb.Bezeichnung2 = data.Designation2;
			//itemDb.Bezeichnung2;
			orderElementDb.Einheit = data.MeasureUnitQualifier;
			// orderElementDb.ArtikelNr = itemDb.ArtikelNr; // - 2022-12-15 - goto: change Article
			orderElementDb.Kupferbasis = kupferbasis;// 150;
			orderElementDb.Preiseinheit = unitPriceBasis == 0 ? 1 : unitPriceBasis; // - 2022-05-30 - init to 1 to respect DB Constraint
			orderElementDb.DELFixiert = data.DelFixed; // itemDb.DELFixiert;
			orderElementDb.DEL = data.DelNote; // data.DelFixed.HasValue && !data.DelFixed.Value ? data.DelNote: itemDb.DEL; // itemDb.DEL;
			orderElementDb.EinzelCuGewicht = itemDb.CuGewicht ?? 0;
			orderElementDb.VKFestpreis = fixedPrice;
			orderElementDb.USt = (decimal)data.VAT; //itemDb.Umsatzsteuer;
			orderElementDb.Einzelkupferzuschlag = (decimal)singleCopperSurcharge;
			orderElementDb.GesamtCuGewicht = (decimal)totalCuWeight;
			orderElementDb.Einzelpreis = (decimal)unitPrice;
			orderElementDb.VKEinzelpreis = (decimal)vkUnitPrice;
			orderElementDb.Gesamtpreis = (decimal)totalPrice;
			orderElementDb.Gesamtkupferzuschlag = (decimal)totalCopperSurcharge;
			orderElementDb.VKGesamtpreis = (decimal)vKTotalPrice;
			orderElementDb.Lagerort_id = storageLocationDb != null
				? storageLocationDb.LagerortId
				: (int?)null;
			orderElementDb.Liefertermin = orderElementDb.Liefertermin;
			orderElementDb.RP = data.RP;
			orderElementDb.EKPreise_Fix = data.FixedPrice;
			orderElementDb.POSTEXT = data.Postext;
			//orderElementDb.Bezeichnung1 = data.Designation1;
			//orderElementDb.Bezeichnung2 = data.Designation2;
			orderElementDb.Index_Kunde = data.Index_Kunde;
			orderElementDb.Index_Kunde_Datum = data.Index_Kunde_Datum;
			orderElementDb.Rabatt = data.Discount;
			orderElementDb.CSInterneBemerkung = data.CSInterneBemerkung;
			orderElementDb.ABPoszuRAPos = data.RahmenId;

			// - 2022-12-15 - if Article has changed - update!
			#region change Article ?
			if(orderElementDb.ArtikelNr != itemDb.ArtikelNr)
			{
				orderElementDb.Bezeichnung1 = itemDb.Bezeichnung1;
				orderElementDb.Bezeichnung2 = itemDb.Bezeichnung2;
				orderElementDb.Bezeichnung3 = itemDb.Bezeichnung3;
				// - 
				orderElementDb.Index_Kunde = itemDb.Index_Kunde;
				orderElementDb.Index_Kunde_Datum = itemDb.Index_Kunde_Datum;
				// -
				orderElementDb.DELFixiert = itemDb.DELFixiert;
				orderElementDb.DEL = itemDb.DEL;
				orderElementDb.Einheit = itemDb.Einheit;
			}
			orderElementDb.ArtikelNr = itemDb.ArtikelNr;
			// -
			#endregion

			orderElementDb.ABPoszuRAPos = data.RahmenPosId.HasValue && data.RahmenPosId.Value > 0 ? data.RahmenPosId.Value : null;
			return new Core.Models.ResponseModel<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>()
			{
				Success = true,
				Body = orderElementDb,
				Warnings = warnings
			};
		}
		internal static List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity> GetLog(Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity _new,
			Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity _old, Core.Identity.Models.UserModel user)
		{
			var _object = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_old.AngebotNr ?? -1);
			var _newArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(_new?.ArtikelNr ?? -1);
			var _oldArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(_old.ArtikelNr ?? -1);
			var _oldStorageLocation = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get(_old.Lagerort_id ?? -1);
			var _newStorageLocation = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get(_new.Lagerort_id ?? -1);
			List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity> _logs = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();
			var _toLog = new LogHelper(_object.Nr, (int)_object.Angebot_Nr, int.TryParse(_object.Projekt_Nr, out var val) ? val : 0, _object.Typ, LogHelper.LogType.MODIFICATIONPOS, "CTS", user);
			if(_old?.Kupferbasis != _new?.Kupferbasis)
			{
				_logs.Add(_toLog.LogCTS("Kupferbasis", _old?.Kupferbasis?.ToString(), _new?.Kupferbasis?.ToString(), (int)_old?.Position, _old.Nr));
			}
			if(_old?.DEL != _new?.DEL)
			{
				_logs.Add(_toLog.LogCTS("DEL", _old?.DEL?.ToString(), _new?.DEL?.ToString(), (int)_old?.Position, _old.Nr));
			}
			if(_old?.Liefertermin != _new?.Liefertermin)
			{
				_logs.Add(_toLog.LogCTS("Liefertermin", _old?.Liefertermin?.ToString(), _new?.Liefertermin?.ToString(), (int)_old?.Position, _old.Nr));
			}
			if(_old?.Bezeichnung1 != _new?.Bezeichnung1)
			{
				_logs.Add(_toLog.LogCTS("Bezeichnung1", _old?.Bezeichnung1?.ToString(), _new?.Bezeichnung1?.ToString(), (int)_old?.Position, _old.Nr));
			}
			if(_old?.Bezeichnung2 != _new?.Bezeichnung2)
			{
				_logs.Add(_toLog.LogCTS("Bezeichnung2", _old?.Bezeichnung2?.ToString(), _new?.Bezeichnung2?.ToString(), (int)_old?.Position, _old.Nr));
			}
			if(_old?.Wunschtermin != _new?.Wunschtermin)
			{
				_logs.Add(_toLog.LogCTS("Wunschtermin", _old?.Wunschtermin?.ToString(), _new?.Wunschtermin?.ToString(), (int)_old?.Position, _old.Nr));
			}
			if(_old?.EKPreise_Fix.HasValue == true && _old?.EKPreise_Fix != _new?.EKPreise_Fix)
			{
				_logs.Add(_toLog.LogCTS("EKPreise_Fix", _old?.EKPreise_Fix?.ToString(), _new?.EKPreise_Fix?.ToString(), (int)_old?.Position, _old.Nr));
			}
			if(_old?.VKFestpreis != _new?.VKFestpreis)
			{
				_logs.Add(_toLog.LogCTS("VKFestpreis", _old?.VKFestpreis?.ToString(), _new?.VKFestpreis?.ToString(), (int)_old?.Position, _old.Nr));
			}
			if(_old?.Freies_Format_EDI != _new?.Freies_Format_EDI)
			{
				_logs.Add(_toLog.LogCTS("Freies_Format_EDI", _old?.Freies_Format_EDI?.ToString(), _new?.Freies_Format_EDI?.ToString(), (int)_old?.Position, _old.Nr));
			}
			if(_old?.ArtikelNr != _newArticle.ArtikelNr)
			{
				_logs.Add(_toLog.LogCTS("Artikel", _oldArticle.ArtikelNummer, _newArticle.ArtikelNummer?.ToString(), (int)_old?.Position, _old.Nr));
			}
			if(_old?.Bemerkungsfeld1 != _new?.Bemerkungsfeld1)
			{
				_logs.Add(_toLog.LogCTS("Bemerkungsfeld1", _old?.Bemerkungsfeld1?.ToString(), _new?.Bemerkungsfeld1?.ToString(), (int)_old?.Position, _old.Nr));
			}
			if(_old?.Bemerkungsfeld2 != _new?.Bemerkungsfeld2)
			{
				_logs.Add(_toLog.LogCTS("Bemerkungsfeld2", _old?.Bemerkungsfeld2?.ToString(), _new?.Bemerkungsfeld2?.ToString(), (int)_old?.Position, _old.Nr));
			}
			if(_old?.Position != _new?.Position)
			{
				_logs.Add(_toLog.LogCTS("Position", _old?.Position?.ToString(), _new?.Position?.ToString(), (int)_old?.Position, _old.Nr));
			}
			if(_old?.Lagerort_id != _new?.Lagerort_id)
			{
				_logs.Add(_toLog.LogCTS("Lagerort", _oldStorageLocation?.Lagerort, _newStorageLocation?.Lagerort, (int)_old?.Position, _old.Nr));
			}
			if(_old?.VKEinzelpreis != _new?.VKEinzelpreis)
			{
				_logs.Add(_toLog.LogCTS("VKEinzelpreis", _old?.VKEinzelpreis?.ToString(), _new?.VKEinzelpreis?.ToString(), (int)_old?.Position, _old.Nr));
			}
			if(_old?.Preiseinheit != _new?.Preiseinheit)
			{
				_logs.Add(_toLog.LogCTS("Preiseinheit", _old?.Preiseinheit?.ToString(), _new?.Preiseinheit?.ToString(), (int)_old?.Position, _old.Nr));
			}
			if(_old?.Abladestelle != _new?.Abladestelle)
			{
				_logs.Add(_toLog.LogCTS("Abladestelle", _old?.Abladestelle, _new?.Abladestelle, (int)_old?.Position, _old.Nr));
			}
			if(_old?.USt != _new?.USt)
			{
				_logs.Add(_toLog.LogCTS("USt", _old?.USt?.ToString(), _new?.USt?.ToString(), (int)_old?.Position, _old.Nr));
			}
			if(_old?.AktuelleAnzahl != _new?.AktuelleAnzahl)
			{
				_logs.Add(_toLog.LogCTS("Quantity", _old?.AktuelleAnzahl?.ToString(), _new?.AktuelleAnzahl?.ToString(), (int)_old?.Position, _old.Nr));
			}
			return _logs;
		}
		public static decimal CalculateRahmenVailableQty(int NrRahmenPos, int NrABPos)
		{

			var rahmenPosEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(NrRahmenPos);
			var abPosEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(NrABPos);
			var aBPosRahmenList = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetAbByRahmenPosition(NrRahmenPos) ?? new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();

			var rahmenOriginalQty = rahmenPosEntity.OriginalAnzahl ?? 0;
			var abLinksTakenQty = aBPosRahmenList?.Sum(x => x.OriginalAnzahl) ?? 0;
			var abRequestedQty = abPosEntity.Anzahl ?? 0;

			var _available = 0m;
			if(abPosEntity.ABPoszuRAPos.HasValue && abPosEntity.ABPoszuRAPos.Value == NrRahmenPos)
				_available = (rahmenOriginalQty - abLinksTakenQty) + abRequestedQty;
			else
				_available = rahmenOriginalQty - abLinksTakenQty;
			return _available;
		}
	}
}
