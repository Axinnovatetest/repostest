using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.EDI.Helpers
{
	public class CalculationHelper
	{
		internal static decimal CalculateTotalWeight(decimal ordredQuantity,
			decimal cuGewicht)
		{
			return ordredQuantity * cuGewicht;
		}

		internal static decimal CalculateTotalCopperSurcharge(bool fixedPrice,
			decimal ordredQuantity,
			decimal einzelkupferzuschlag)
		{
			return fixedPrice
				? 0
				: (ordredQuantity * einzelkupferzuschlag);
		}

		internal static decimal CalculateSingleCopperSurcharge(bool fixedPrice,
			int del,
			decimal cuGewicht,
			int kupferbasis)
		{
			// - 2022-08-04 - return 0 if del == 0 to prevent negative Surchage
			return !fixedPrice
				? del == 0 ? 0 : decimal.Round((Convert.ToDecimal((del * 1.01m) - kupferbasis) / 100m) * cuGewicht, 2)
				: 0;
		}

		internal static decimal CalculateVkUnitPrice(bool isSerie, bool fixedPrice,
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
			if(fixedPrice == true || isSerie == false)
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

		internal static decimal CalculateUnitPrice(bool isSerie, bool isFixedFrice,
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
			else if(isSerie == false)
			{
				return einzelkupferzuschlag * unitPriceBasis + vKEinzelpreis;
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

		internal static decimal CalculateTotalPrice(decimal unitPriceBasis,
			decimal einzelpreis,
			decimal ordredQuantity,
			decimal discount)
		{
			return (unitPriceBasis > 0 ? (einzelpreis / unitPriceBasis) : einzelpreis)
				* ordredQuantity
				* (1m - discount);
		}

		internal static decimal CalculateVkTotalPrice(decimal unitPriceBasis,
			decimal vKEinzelpreis,
			decimal ordredQuantity)
		{
			return ordredQuantity
				* (unitPriceBasis > 0 ? (vKEinzelpreis / unitPriceBasis) : vKEinzelpreis);
		}

		public static int CalculateRahmenGesamtPries(int rahmenId)
		{
			int result = -1;
			var rahmen = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(rahmenId);
			var rahmenPositions = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByRahmenNr(new List<int> { rahmenId });
			if(rahmenPositions != null && rahmenPositions.Count > 0)
			{
				var _gesamtPries = rahmenPositions.Sum(s => s.Gesamtpreis);
				rahmen.Gesamtpreis = (decimal?)(_gesamtPries ?? -1);
				result = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.Update(rahmen);
			}

			return result;
		}
		public static int CalculateRahmenGesamtPries(List<int> rahmenIds)
		{
			int result = -1;
			foreach(var item in rahmenIds)
			{
				var rahmen = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(item);
				var rahmenPositions = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByRahmenNr(new List<int> { item });
				if(rahmen != null && rahmenPositions != null && rahmenPositions.Count > 0)
				{
					var _gesamtPries = rahmenPositions.Sum(s => s.Gesamtpreis);
					rahmen.Gesamtpreis = (decimal?)(_gesamtPries ?? -1);
					result += Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.Update(rahmen);
				}
			}

			return result;
		}
	}
}
