using System;

namespace Psz.Core.Apps.Purchase.Helpers
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
			decimal cuGewicht)
		{
			return !fixedPrice
				? decimal.Round((Convert.ToDecimal((del * 1.01m) - 150m) / 100m) * cuGewicht, 2)
				: 0;
		}

		internal static decimal CalculateVkUnitPrice(bool fixedPrice,
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

		internal static decimal CalculateUnitPrice(bool isFixedFrice,
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
	}
}
