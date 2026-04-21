using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Psz.Core.Common.Helpers.CTS
{
	public class BlanketHelpers
	{
		#region CTS prices calculations
		public static decimal CalculateTotalWeight(decimal ordredQuantity,
			decimal cuGewicht)
		{
			return ordredQuantity * cuGewicht;
		}
		internal static decimal CalculateGesamtpreisPosition(decimal Zielmenge,
		   decimal Preis)
		{
			return Zielmenge * Preis;
		}

		public static decimal CalculateTotalCopperSurcharge(bool fixedPrice,
			decimal ordredQuantity,
			decimal einzelkupferzuschlag)
		{
			return fixedPrice
				? 0
				: (ordredQuantity * einzelkupferzuschlag);
		}

		public static decimal CalculateSingleCopperSurcharge(bool fixedPrice,
			int del,
			decimal cuGewicht)
		{
			// - 2022-08-04 - return 0 if del == 0 to prevent negative Surchage
			return !fixedPrice
				? del == 0 ? 0 : decimal.Round((Convert.ToDecimal((del * 1.01m) - 150m) / 100m) * cuGewicht, 2)
				: 0;
		}

		public static decimal CalculateVkUnitPrice(bool fixedPrice,
			decimal verkaufspreis,
			decimal orderedQuantity,
			decimal me1,
			decimal me2,
			decimal me3,
			decimal me4,
			decimal pm2,
			decimal pm3,
			decimal pm4)
		{
			if(fixedPrice == true)
			{
				return verkaufspreis;
			}
			else if(orderedQuantity <= me1)
			{
				return verkaufspreis;
			}
			else if(orderedQuantity > me1 && orderedQuantity <= me2)
			{
				return verkaufspreis - verkaufspreis * pm2 / 100;
			}
			else if(orderedQuantity > me2 && orderedQuantity <= me3)
			{
				return verkaufspreis - verkaufspreis * pm3 / 100;
			}
			else if(orderedQuantity > me3 && orderedQuantity <= me4)
			{
				return verkaufspreis - verkaufspreis * pm4 / 100;
			}
			else
			{
				return verkaufspreis;
			}
		}

		public static decimal CalculateUnitPrice(bool isFixedFrice,
			decimal unitPriceBasis,
			decimal orderedQuantity,
			decimal vKEinzelpreis,
			decimal verkaufspreis,
			decimal einzelkupferzuschlag,
			decimal me1,
			decimal me2,
			decimal me3,
			decimal me4,
			decimal pm2,
			decimal pm3,
			decimal pm4)
		{
			if(isFixedFrice)
			{
				return vKEinzelpreis;
			}
			else if(orderedQuantity <= me1)
			{
				return einzelkupferzuschlag * unitPriceBasis + (verkaufspreis);
			}
			else if(orderedQuantity > me1 && orderedQuantity <= me2)
			{
				return einzelkupferzuschlag * unitPriceBasis + (verkaufspreis - verkaufspreis * pm2 / 100);
			}
			else if(orderedQuantity > me2 && orderedQuantity <= me3)
			{
				return einzelkupferzuschlag * unitPriceBasis + (verkaufspreis - verkaufspreis * pm3 / 100);
			}
			else if(orderedQuantity > me3 && orderedQuantity <= me4)
			{
				return einzelkupferzuschlag * unitPriceBasis + (verkaufspreis - verkaufspreis * pm4 / 100);
			}
			else
			{
				return einzelkupferzuschlag * unitPriceBasis + vKEinzelpreis;
			}
		}

		public static decimal CalculateTotalPrice(decimal unitPriceBasis,
			decimal einzelpreis,
			decimal ordredQuantity,
			decimal discount)
		{
			return (unitPriceBasis > 0 ? (einzelpreis / unitPriceBasis) : einzelpreis)
				* ordredQuantity
				* (1m - discount);
		}

		public static decimal CalculateVkTotalPrice(decimal unitPriceBasis,
			decimal vKEinzelpreis,
			decimal ordredQuantity)
		{
			return ordredQuantity
				* (unitPriceBasis > 0 ? (vKEinzelpreis / unitPriceBasis) : vKEinzelpreis);
		}


		public static decimal CalculateSingleCopperSurcharge(bool fixedPrice,
			int del,
			decimal cuGewicht,
			int kupferbasis)
		{
			return !fixedPrice
				? decimal.Round((Convert.ToDecimal((del * 1.01m) - kupferbasis) / 100m) * cuGewicht, 2)
				: 0;
		}
		#endregion

		public static void ComputePositionPrice(Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity orderElementDb,
			Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity orderElementDbExtension,
			Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity itemDB, Infrastructure.Data.Entities.Tables.BSD.WahrungenEntity wahrungenEntity,
			decimal quantity, decimal price)
		{
			#region price computations - 2022-09-06 - Reil
			//calculating prices
			var discount = 0m;
			var unitPriceBasis = Convert.ToDecimal(itemDB.Preiseinheit ?? 0m);
			var fixedTotalPrice = false; // -  cauz RA - itemDB.VKFestpreis ?? false;
			var fixedUnitPrice = true; // - cauz RA
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
			var verkaufspreis = price; //  - 0m;

			var singleCopperSurcharge = CalculateSingleCopperSurcharge(fixedTotalPrice,
					del,
					cuWeight);

			var totalCopperSurcharge = CalculateTotalCopperSurcharge(fixedTotalPrice,
				quantity,
				singleCopperSurcharge);

			var vkUnitPrice = fixedUnitPrice ? price : CalculateVkUnitPrice(fixedTotalPrice,
				verkaufspreis,
				quantity,
				me1,
				me2,
				me3,
				me4,
				pm2,
				pm3,
				pm4);

			var unitPrice = CalculateUnitPrice(fixedUnitPrice, //fixedTotalPrice,
				unitPriceBasis,
				quantity,
				fixedUnitPrice ? vkUnitPrice + singleCopperSurcharge : vkUnitPrice,
				verkaufspreis,
				singleCopperSurcharge,
				me1,
				me2,
				me3,
				me4,
				pm2,
				pm3,
				pm4);

			var totalPrice = CalculateTotalPrice(unitPriceBasis,
				unitPrice,
				quantity,
				discount);

			var vKTotalPrice = CalculateVkTotalPrice(unitPriceBasis,
				vkUnitPrice,
				quantity);

			var totalCuWeight = CalculateTotalWeight(quantity,
				cuWeight);
			#endregion price computations

			orderElementDb.Einzelpreis = unitPrice;
			orderElementDb.Gesamtpreis = totalPrice;
			orderElementDb.VKFestpreis = fixedTotalPrice;
			orderElementDb.GesamtCuGewicht = totalCuWeight;
			orderElementDb.Einzelkupferzuschlag = singleCopperSurcharge;
			orderElementDb.Gesamtkupferzuschlag = totalCopperSurcharge;
			orderElementDb.VKEinzelpreis = vkUnitPrice;
			orderElementDb.VKGesamtpreis = vKTotalPrice;
			orderElementDb.EKPreise_Fix = fixedUnitPrice;

			//order item extension
			orderElementDbExtension.Gesamtpreis = totalPrice;
			orderElementDbExtension.Preis = unitPrice;
			orderElementDbExtension.PreisDefault = unitPrice * (wahrungenEntity.Entspricht_DM ?? 0);
			orderElementDbExtension.GesamtpreisDefault = totalPrice * (wahrungenEntity.Entspricht_DM ?? 0);
		}
		public static int CalculateRahmenGesamtPries(int rahmenId)
		{
			int result = -1;
			var rahmen = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(rahmenId);
			if(rahmen == null)
			{
				return -1;
			}
			var rahmenPositions = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByRahmenNr(new List<int> { rahmenId });
			if(rahmenPositions != null && rahmenPositions.Count > 0)
			{
				var _gesamtPries = rahmenPositions.Sum(s => s.Gesamtpreis ?? 0);
				var _gesamtPriesDefault = rahmenPositions.Sum(s => s.GesamtpreisDefault ?? 0);

				rahmen.Gesamtpreis = _gesamtPries;
				rahmen.GesamtpreisDefault = _gesamtPriesDefault;
			}
			else
			{
				rahmen.Gesamtpreis = 0m;
				rahmen.GesamtpreisDefault = 0m;
			}
			result = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.Update(rahmen);

			return result;
		}
		public static int CalculateRahmenGesamtPries(int rahmenId, Infrastructure.Services.Utils.TransactionsManager boTransactions)
		{
			int result = -1;
			var rahmen = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(rahmenId, boTransactions.connection, boTransactions.transaction);
			if(rahmen == null)
			{
				return -1;
			}
			var rahmenPositions = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByRahmenNr(new List<int> { rahmenId }, boTransactions.connection, boTransactions.transaction);
			if(rahmenPositions != null && rahmenPositions.Count > 0)
			{
				var _gesamtPries = rahmenPositions.Sum(s => s.Gesamtpreis ?? 0);
				var _gesamtPriesDefault = rahmenPositions.Sum(s => s.GesamtpreisDefault ?? 0);

				rahmen.Gesamtpreis = _gesamtPries;
				rahmen.GesamtpreisDefault = _gesamtPriesDefault;
			}
			else
			{
				rahmen.Gesamtpreis = 0m;
				rahmen.GesamtpreisDefault = 0m;
			}
			result = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.UpdateWithTransaction(rahmen, boTransactions.connection, boTransactions.transaction);

			return result;
		}

		public static decimal CalculateRahmenVailableQty(int NrRahmenPos, int NrABPos)
		{

			var rahmenPosEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(NrRahmenPos);
			var abPosEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(NrABPos);
			var aBPosRahmenList = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetAbByRahmenPosition(NrRahmenPos) ?? new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();

			var rahmenOriginalQty = rahmenPosEntity.OriginalAnzahl ?? 0;
			var abLinksTakenQty = aBPosRahmenList?.Sum(x => x.Anzahl) ?? 0;
			var abRequestedQty = abPosEntity.Anzahl ?? 0;

			var _available = 0m;
			if(abPosEntity.ABPoszuRAPos.HasValue && abPosEntity.ABPoszuRAPos.Value == NrRahmenPos)
				_available = (rahmenOriginalQty - abLinksTakenQty) + abRequestedQty;
			else
				_available = rahmenOriginalQty - abLinksTakenQty;
			return _available;
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
		public static string TrimStartConditionsID(string input)
		{
			return Regex.Replace(input ?? "", @"^\d+\|\|\s*", "");
		}
	}
}
