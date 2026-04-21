using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Helpers
{
	public class BlanketHelper
	{
		public static Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity GetCalculatedPositon(int ABId, int articleId, decimal qty, bool posPrice, DateTime wunshtermin,
			 int abPosZuRAPos, string postext)
		{

			var PriceHistory = Infrastructure.Data.Access.Tables.CTS.RahmenPriceHistoryAccess.GetByMaxPriceAndDate(abPosZuRAPos, wunshtermin);
			int newPositionNumber = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetMaxPositionNumberByOrderId(ABId) + 10;
			var itemDB = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(articleId);
			var itemPricingGroupsDb = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(itemDB.ArtikelNr);
			var prcieToApply = 0m;
			if(posPrice)
			{
				if(PriceHistory != null && PriceHistory.Count > 0)
					prcieToApply = PriceHistory[0].BasePrice ?? 0m;
				else
					prcieToApply = itemPricingGroupsDb.Verkaufspreis ?? 0m;
			}
			else
				prcieToApply = itemPricingGroupsDb.Verkaufspreis ?? 0m;

			//calculating prices
			var discount = 0m;
			var unitPriceBasis = /*posPrice == true ? price : */itemDB.Preiseinheit ?? 0m;
			var fixedPrice = false;//itemDB.VKFestpreis ?? false;
			var cuWeight = Convert.ToDecimal(itemDB.CuGewicht ?? 0);
			var del = (itemDB.DEL ?? 0);

			var me1 = 0m;
			var me2 = 0m;
			var me3 = 0m;
			var me4 = 0m;
			var pm1 = 0m;
			var pm2 = 0m;
			var pm3 = 0m;
			var pm4 = 0m;
			var verkaufspreis = 0m;

			if(itemPricingGroupsDb != null)
			{
				me1 = Convert.ToDecimal(itemPricingGroupsDb.ME1 ?? 0m);
				me2 = Convert.ToDecimal(itemPricingGroupsDb.ME2 ?? 0m);
				me3 = Convert.ToDecimal(itemPricingGroupsDb.ME3 ?? 0m);
				me4 = Convert.ToDecimal(itemPricingGroupsDb.ME4 ?? 0m);
				pm1 = Convert.ToDecimal(itemPricingGroupsDb.PM1 ?? 0m);
				pm2 = Convert.ToDecimal(itemPricingGroupsDb.PM2 ?? 0m);
				pm3 = Convert.ToDecimal(itemPricingGroupsDb.PM3 ?? 0m);
				pm4 = Convert.ToDecimal(itemPricingGroupsDb.PM4 ?? 0m);
				verkaufspreis = prcieToApply;
			}
			var singleCopperSurcharge = Common.Helpers.CTS.BlanketHelpers.CalculateSingleCopperSurcharge(fixedPrice,
					del,
					cuWeight);

			var totalCopperSurcharge = Common.Helpers.CTS.BlanketHelpers.CalculateTotalCopperSurcharge(fixedPrice,
				qty,
				singleCopperSurcharge);

			var vkUnitPrice = Common.Helpers.CTS.BlanketHelpers.CalculateVkUnitPrice(fixedPrice,
				verkaufspreis,
				qty,
				me1,
				me2,
				me3,
				me4,
				pm2,
				pm3,
				pm4);

			var unitPrice = Common.Helpers.CTS.BlanketHelpers.CalculateUnitPrice(fixedPrice,
				unitPriceBasis,
				qty,
				vkUnitPrice,
				verkaufspreis,
				singleCopperSurcharge,
				me1,
				me2,
				me3,
				me4,
				pm2,
				pm3,
				pm4);

			var totalPrice = Common.Helpers.CTS.BlanketHelpers.CalculateTotalPrice(unitPriceBasis,
				unitPrice,
				qty,
				discount);

			var vKTotalPrice = Common.Helpers.CTS.BlanketHelpers.CalculateVkTotalPrice(unitPriceBasis,
				vkUnitPrice,
				qty);

			var totalCuWeight = Common.Helpers.CTS.BlanketHelpers.CalculateTotalWeight((decimal)qty,
				cuWeight);
			var articleProductionExtensionEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.GetByArticleId(itemDB.ArtikelNr);
			var articleProductionPlace = articleProductionExtensionEntity?.ProductionPlace1_Id;
			var storageLocation = new KeyValuePair<int, string>();
			if(articleProductionPlace != null)
			{
				storageLocation = Enums.OrderEnums.GetArticleHauplager((Psz.Core.Common.Enums.ArticleEnums.ArticleProductionPlace)articleProductionPlace);
			}
			var response = new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity
			{
				// TODO Verif: Order Element Type :  Prototyp - Erstmuster - Nullserie - Serie + Bezug +Bemerkung [Type Position]
				Typ = (int)Enums.OrderEnums.ItemType.Serie, // <<<  
				AngebotNr = ABId,
				Position = newPositionNumber,
				//newPositionNumber,
				Wunschtermin = wunshtermin,//OrderDb.Wunschtermin ?? DateTime.Now.AddDays(+30),
				Anzahl = qty,
				Abladestelle = "",
				Bezeichnung2_Kunde = itemDB.Bezeichnung2,
				OriginalAnzahl = qty,
				Freies_Format_EDI = "",

				EDI_PREIS_KUNDE = 0m,
				EDI_PREISEINHEIT = 0m,
				EDI_Quantity_Ordered = 0m,
				EDI_Historie_Nr = null,

				LieferanweisungP_FTXDIN_TEXT = "",
				Bemerkungsfeld1 = "",
				Bemerkungsfeld2 = "",

				Bezeichnung1 = itemDB.Bezeichnung1,
				Bezeichnung2 = itemDB.Bezeichnung2,
				Bezeichnung3 = "-",
				Einheit = itemDB.Einheit,
				ArtikelNr = itemDB.ArtikelNr,

				Kupferbasis = 150,

				Preiseinheit = unitPriceBasis,
				DELFixiert = itemDB.DELFixiert ?? false,
				DEL = itemDB.DEL ?? 0,
				EinzelCuGewicht = Convert.ToDecimal(itemDB.CuGewicht ?? 0),
				VKFestpreis = fixedPrice,
				USt = Convert.ToDecimal(itemDB.Umsatzsteuer ?? 0),
				Einzelkupferzuschlag = singleCopperSurcharge,
				GesamtCuGewicht = totalCuWeight,
				Einzelpreis = unitPrice,
				VKEinzelpreis = vkUnitPrice,
				Gesamtpreis = totalPrice,
				Gesamtkupferzuschlag = totalCopperSurcharge,
				VKGesamtpreis = vKTotalPrice,
				erledigt_pos = false,
				POSTEXT = posPrice ? postext : "",
				Geliefert = 0,
				Rabatt = 0,
				Index_Kunde = itemDB.Index_Kunde,
				Index_Kunde_Datum = itemDB.Index_Kunde_Datum,
				Lagerort_id = articleProductionPlace != null ? storageLocation.Key : null,
				ABPoszuRAPos = posPrice ? abPosZuRAPos : null,
				EKPreise_Fix = true
			};
			return response;
		}
		public static Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity GetCalculatedPositon(int PosId, decimal qty, bool posPrice, decimal price, int? rahmenPositionNr)
		{
			var OrderItemDb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(PosId);
			var itemDB = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(OrderItemDb.ArtikelNr ?? -1);
			//calculating prices
			var discount = 0m;
			var unitPriceBasis = /*posPrice == true ? price :*/ itemDB.Preiseinheit ?? 0m;
			var fixedPrice = true;
			var cuWeight = Convert.ToDecimal(itemDB.CuGewicht ?? 0);
			var del = (itemDB.DEL ?? 0);
			var freeText = "";

			var me1 = 0m;
			var me2 = 0m;
			var me3 = 0m;
			var me4 = 0m;
			var pm1 = 0m;
			var pm2 = 0m;
			var pm3 = 0m;
			var pm4 = 0m;
			var verkaufspreis = 0m;

			var itemPricingGroupsDb = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(itemDB.ArtikelNr);
			if(itemPricingGroupsDb != null)
			{
				me1 = Convert.ToDecimal(itemPricingGroupsDb.ME1 ?? 0m);
				me2 = Convert.ToDecimal(itemPricingGroupsDb.ME2 ?? 0m);
				me3 = Convert.ToDecimal(itemPricingGroupsDb.ME3 ?? 0m);
				me4 = Convert.ToDecimal(itemPricingGroupsDb.ME4 ?? 0m);
				pm1 = Convert.ToDecimal(itemPricingGroupsDb.PM1 ?? 0m);
				pm2 = Convert.ToDecimal(itemPricingGroupsDb.PM2 ?? 0m);
				pm3 = Convert.ToDecimal(itemPricingGroupsDb.PM3 ?? 0m);
				pm4 = Convert.ToDecimal(itemPricingGroupsDb.PM4 ?? 0m);
				verkaufspreis = itemPricingGroupsDb.Verkaufspreis ?? 0m;
			}
			verkaufspreis = posPrice ? price : verkaufspreis;
			var singleCopperSurcharge = Common.Helpers.CTS.BlanketHelpers.CalculateSingleCopperSurcharge(fixedPrice,
					del,
					cuWeight);

			var totalCopperSurcharge = Common.Helpers.CTS.BlanketHelpers.CalculateTotalCopperSurcharge(fixedPrice,
				(decimal)qty,
				singleCopperSurcharge);

			var vkUnitPrice = Common.Helpers.CTS.BlanketHelpers.CalculateVkUnitPrice(fixedPrice,
				verkaufspreis,
				(decimal)qty,
				me1,
				me2,
				me3,
				me4,
				pm2,
				pm3,
				pm4);

			var unitPrice = Common.Helpers.CTS.BlanketHelpers.CalculateUnitPrice(fixedPrice,
				unitPriceBasis,
				(decimal)qty,
				vkUnitPrice,
				verkaufspreis,
				singleCopperSurcharge,
				me1,
				me2,
				me3,
				me4,
				pm2,
				pm3,
				pm4);

			var totalPrice = Common.Helpers.CTS.BlanketHelpers.CalculateTotalPrice(unitPriceBasis,
				unitPrice,
				(decimal)qty,
				discount);

			var vKTotalPrice = Common.Helpers.CTS.BlanketHelpers.CalculateVkTotalPrice(unitPriceBasis,
				vkUnitPrice,
				(decimal)qty);

			var totalCuWeight = Common.Helpers.CTS.BlanketHelpers.CalculateTotalWeight((decimal)qty,
				cuWeight);


			OrderItemDb.Preiseinheit = unitPriceBasis;
			OrderItemDb.DELFixiert = itemDB.DELFixiert ?? false;
			OrderItemDb.DEL = itemDB.DEL ?? 0;
			OrderItemDb.EinzelCuGewicht = Convert.ToDecimal(itemDB.CuGewicht ?? 0);
			OrderItemDb.VKFestpreis = fixedPrice;
			OrderItemDb.Einzelkupferzuschlag = singleCopperSurcharge;
			OrderItemDb.GesamtCuGewicht = totalCuWeight;
			OrderItemDb.Einzelpreis = unitPrice;
			OrderItemDb.VKEinzelpreis = vkUnitPrice;
			OrderItemDb.Gesamtpreis = totalPrice;
			OrderItemDb.Gesamtkupferzuschlag = totalCopperSurcharge;
			OrderItemDb.VKGesamtpreis = vKTotalPrice;
			OrderItemDb.ABPoszuRAPos = rahmenPositionNr;
			return OrderItemDb;
		}

		//public static void StatusEmailNotification(Enums.BlanketEnums.ActionStatus action, Identity.Models.UserModel _user, string reason, int RahmenId)
		//{
		//	try
		//	{
		//		var body = "";
		//		//var content = "";
		//		var title = "";
		//		var addresses = new List<string>();
		//		var RahmenExtensionEntity = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(RahmenId);
		//		var RahmenEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(RahmenId);

		//		#region // - 2022-12-01 - send Mail to Assigned Employee
		//		var assignedEmployeeAddresses = new List<string>();
		//		var pCustomersNumbers = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetBy_IsPrimary_CustomerNumber(true, RahmenExtensionEntity.CustomerId ?? -1);
		//		var npCustomersNumbers = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetBy_IsPrimary_CustomerNumber(false, RahmenExtensionEntity.CustomerId ?? -1);
		//		if(npCustomersNumbers != null)
		//		{
		//			if(DateTime.Now >= npCustomersNumbers.ValidFromTime.Date && DateTime.Now <= npCustomersNumbers.ValidIntoTime.Date.AddDays(1))
		//			{
		//				if(pCustomersNumbers != null)
		//				{
		//					assignedEmployeeAddresses.Add(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(npCustomersNumbers.UserId)?.Email ?? "");
		//				}
		//			}
		//			else
		//			{
		//				npCustomersNumbers = null;
		//			}
		//		}
		//		if(pCustomersNumbers != null)
		//		{
		//			assignedEmployeeAddresses.Add(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(pCustomersNumbers.UserId)?.Email ?? "");
		//		}
		//		#endregion

		//		switch(action)
		//		{
		//			case Enums.BlanketEnums.ActionStatus.SubmitValidate:
		//				body = Template("request_validation", RahmenEntity, null, _user.Name);
		//				title = $"Validation Request – Rahmen | Document No. {RahmenEntity.Bezug}";
		//				addresses.AddRange(
		//				Infrastructure.Data.Access.Tables.COR.UserAccess.Get(
		//			Infrastructure.Data.Access.Tables.CTS.AccessProfileUsersAccess.GetByAccessProfileIds(
		//  Infrastructure.Data.Access.Tables.CTS.AccessProfileAccess.GetAdminBlanket()?.Select(x => x.Id)?.ToList())?.Select(x => x.UserId)?.ToList())?.Select(x => x.Email)?.ToList());
		//				break;
		//			case Enums.BlanketEnums.ActionStatus.Valider:
		//				body = Template("confirm_validation", RahmenEntity, null, _user.Name);
		//				title = $"Confirmation of Validation – Rahmen | Document No. {RahmenEntity.Bezug}";
		//				addresses.Add(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(RahmenExtensionEntity.CreateUserId)?.Email);
		//				// - 2022-12-01 send Mail to Assigned Employee
		//				if(assignedEmployeeAddresses.Count > 0)
		//				{
		//					addresses.AddRange(assignedEmployeeAddresses);
		//				}
		//				break;
		//			case Enums.BlanketEnums.ActionStatus.Rejeter:
		//				body = Template("reject_validation", RahmenEntity, "rejected", _user.Name);
		//				title = $"Cancellation Notice – Rahmen | Document No. {RahmenEntity.Bezug}";
		//				addresses.Add(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(RahmenExtensionEntity.CreateUserId)?.Email);
		//				// - 2022-12-01 send Mail to Assigned Employee
		//				if(assignedEmployeeAddresses.Count > 0)
		//				{
		//					addresses.AddRange(assignedEmployeeAddresses);
		//				}
		//				break;
		//			case Enums.BlanketEnums.ActionStatus.Fermer:
		//				body = Template("closing", RahmenEntity, null, _user.Name);
		//				title = $"Closing Notice – Rahmen | Document No. {RahmenEntity.Bezug}";
		//				addresses.Add(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(RahmenExtensionEntity.CreateUserId)?.Email);
		//				// - 2022-12-01 send Mail to Assigned Employee
		//				if(assignedEmployeeAddresses.Count > 0)
		//				{
		//					addresses.AddRange(assignedEmployeeAddresses);
		//				}
		//				break;
		//			case Enums.BlanketEnums.ActionStatus.Annuler:
		//				body = Template("reject_validation", RahmenEntity, "cancelled", _user.Name);
		//				title = $"Cancellation Notice – Rahmen | Document No. {RahmenEntity.Bezug}";
		//				addresses.Add(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(RahmenExtensionEntity.CreateUserId)?.Email);
		//				// - 2022-12-01 send Mail to Assigned Employee
		//				if(assignedEmployeeAddresses.Count > 0)
		//				{
		//					addresses.AddRange(assignedEmployeeAddresses);
		//				}
		//				break;
		//			default:
		//				break;
		//		}
		//		//content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
		//		//   + $"<span style='font-size:1.5em;'> Good {(DateTime.Now.Hour <= 12 ? "Morning" : " Afternoon")},</span><br/>"
		//		//   + $"{body}"
		//		//   + $"</span><br/><br/>"
		//		//   + "</div>";
		//		//content += "<br/><br/>";
		//		//content += $"<br/><span style='font-size:1em;'>Sincerely,</span>";
		//		//content += $"<br/><span style='font-size:1em;font-weight:bold'>IT Department </span></br>";
		//		addresses.Add("Mohamed.Souilmi@psz-electronic.com");
		//		Module.EmailingService.SendEmailAsync(title, body, addresses, null, IsTable: true);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		throw;
		//	}
		//}
		public static List<Tuple<int, int, int>> GetPositionsColors(List<int> posNrs)
		{
			var _result = new List<Tuple<int, int, int>>();
			foreach(var item in posNrs)
			{
				var rahmenPos = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(item);
				var rahmenPosExtension = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngeboteneArtikelNr(item);
				var linkedAbPos = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetAbByRahmenPosition(item);
				var linkedLsPos = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetLSLinkedToAbs(linkedAbPos.Select(x => x.Nr).ToList());
				// setting params
				var ExpiryDate = rahmenPosExtension?.GultigBis;
				if(ExpiryDate.HasValue)
				{
					TimeSpan ts = Convert.ToDateTime(rahmenPosExtension.GultigBis).Subtract(DateTime.Now);
					var dateDiff = ts.Days;
					var totalWeeks = (int)dateDiff / 7;

					var DeliveredQty = linkedLsPos?.Sum(x => x.Anzahl ?? -1);
					var ConfirmedQty = linkedAbPos?.Sum(x => x.Geliefert ?? -1);
					var Percentage = ((decimal)(rahmenPos?.OriginalAnzahl ?? 0m) * 75) / 100;
					var _temp = new List<Tuple<int, int, int>>();

					if(linkedLsPos != null && linkedLsPos.Count > 0)
					{
						if(totalWeeks <= 12 && DeliveredQty < Percentage)//red pos
							_temp.Add(new Tuple<int, int, int>(rahmenPos.AngebotNr ?? -1, item, (int)Enums.BlanketEnums.ColorStatus.Red));
						else if(totalWeeks > 12 && DeliveredQty >= Percentage)//yellow pos
							_temp.Add(new Tuple<int, int, int>(rahmenPos.AngebotNr ?? -1, item, (int)Enums.BlanketEnums.ColorStatus.Yellow));
					}
					if(totalWeeks <= 12 && ConfirmedQty < Percentage)//orange pos
						_temp.Add(new Tuple<int, int, int>(rahmenPos.AngebotNr ?? -1, item, (int)Enums.BlanketEnums.ColorStatus.Orange));

					else//green pos
						_temp.Add(new Tuple<int, int, int>(rahmenPos.AngebotNr ?? -1, item, (int)Enums.BlanketEnums.ColorStatus.Green));

					if(_temp != null && _temp.Count > 0)
					{
						if(_temp.Count > 1)
						{
							var _BiggestColor = _temp.Max(x => x.Item3);
							_result.Add(_temp.Find(x => x.Item3 == _BiggestColor));
						}
						else
							_result.AddRange(_temp);
					}
				}
			}
			return _result;
		}
		public static Infrastructure.Data.Entities.Tables.MTM.Bestellte_ArtikelEntity GetBestellArtikelEntity(
			decimal cuPrice,
			int bestellungNr,
			Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity,
			Infrastructure.Data.Entities.Tables.MTM.BestellnummernEntity bestellnummernEntity,
			decimal quantity,
			int positionId,
			decimal price,
			int lagerort,
			DateTime? liefertermin,
			Infrastructure.Data.Entities.Tables.MTM.Bestellte_ArtikelEntity bestellte_ArtikelEntity = null)
		{
			if(bestellte_ArtikelEntity == null)
				bestellte_ArtikelEntity = new();

			bestellte_ArtikelEntity.Bestellung_Nr = bestellungNr;
			bestellte_ArtikelEntity.Lagerort_id = lagerort;
			bestellte_ArtikelEntity.AB_Nr_Lieferant = "";
			bestellte_ArtikelEntity.Kanban = false;
			bestellte_ArtikelEntity.CUPreis = cuPrice;
			bestellte_ArtikelEntity.Artikel_Nr = artikelEntity.ArtikelNr;
			bestellte_ArtikelEntity.Bestellung_Nr = bestellungNr;
			bestellte_ArtikelEntity.Bezeichnung_1 = bestellnummernEntity.Artikelbezeichnung;
			bestellte_ArtikelEntity.Bezeichnung_2 = bestellnummernEntity.Artikelbezeichnung2;
			bestellte_ArtikelEntity.Einheit = artikelEntity.Einheit;
			bestellte_ArtikelEntity.Umsatzsteuer = bestellnummernEntity.Umsatzsteuer.HasValue ? (float)bestellnummernEntity.Umsatzsteuer.Value : 0;
			bestellte_ArtikelEntity.Einzelpreis = price;
			bestellte_ArtikelEntity.Bestellnummer = bestellnummernEntity.Bestell_Nr;
			bestellte_ArtikelEntity.Rabatt = (float)bestellnummernEntity.Rabatt;
			bestellte_ArtikelEntity.Preiseinheit = bestellnummernEntity.Einkaufspreis.HasValue ? bestellnummernEntity.Einkaufspreis.Value : 0;
			bestellte_ArtikelEntity.Liefertermin = liefertermin;
			bestellte_ArtikelEntity.Anzahl = quantity;
			bestellte_ArtikelEntity.Gesamtpreis = price * quantity;
			bestellte_ArtikelEntity.Bestatigter_Termin = new DateTime(2999, 12, 31);
			bestellte_ArtikelEntity.Position = 0;
			bestellte_ArtikelEntity.InfoRahmennummer = !string.IsNullOrEmpty(artikelEntity.RahmenNr) ? artikelEntity.RahmenNr : artikelEntity.RahmenNr2;
			bestellte_ArtikelEntity.AnfangLagerBestand = 0;
			bestellte_ArtikelEntity.Start_Anzahl = quantity;
			bestellte_ArtikelEntity.Erhalten = 0;
			bestellte_ArtikelEntity.Aktuelle_Anzahl = 0;
			bestellte_ArtikelEntity.EndeLagerBestand = 0;
			bestellte_ArtikelEntity.Rabatt1 = 0;
			bestellte_ArtikelEntity.Rabatt1 = 0;
			bestellte_ArtikelEntity.Produktionsort = 0;
			bestellte_ArtikelEntity.BP_zu_RBposition = 0;
			bestellte_ArtikelEntity.WE_Pos_zu_Bestellposition = 0;
			bestellte_ArtikelEntity.RB_OriginalAnzahl = 1;
			bestellte_ArtikelEntity.RB_Abgerufen = 1;
			bestellte_ArtikelEntity.RB_Offen = 1;
			bestellte_ArtikelEntity.erledigt_pos = false;
			bestellte_ArtikelEntity.Position_erledigt = false;
			bestellte_ArtikelEntity.Bemerkung_Pos = "-";
			bestellte_ArtikelEntity.Bemerkung_Pos_ID = false;
			bestellte_ArtikelEntity.In_Bearbeitung = false;
			bestellte_ArtikelEntity.Loschen = false;
			bestellte_ArtikelEntity.RA_Pos_zu_Bestellposition = positionId;

			return bestellte_ArtikelEntity;
		}
	}
}