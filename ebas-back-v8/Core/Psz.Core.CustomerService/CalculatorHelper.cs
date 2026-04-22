using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService
{
	public class CalculatorHelper
	{
		//Albania
		internal static List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1ALEntity> CalculateRestbestandAL(List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1ALEntity> entities)
		{
			List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1ALEntity> final_result = new List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1ALEntity>();
			if(entities == null || entities.Count <= 0)
				return null;

			var groupebByMaterial = entities.Select(x => x.Numeri_i_materialit).Distinct().ToList();


			foreach(var item in groupebByMaterial)
			{
				var list = InnerSortAL(entities.Where(x => x.Numeri_i_materialit == item).ToList());

				if(list == null || list.Count <= 0)
					continue;

				// - First
				// ---
				if(list.Count == 1)
				{
					var onlyindex = list.FindIndex(x => x.FA_Nr == list[0].FA_Nr);
					list[onlyindex].RestBestand = Math.Round(list[onlyindex].Im_Lager_Ne_magazine ?? 0m, 2) - list[onlyindex].Bedarf_Nevoje ?? 0m;
				}
				else
				{
					foreach(var l in list)
					{
						//var indexInEntities = entities.FindIndex(x => x.FA_Nr == l.FA_Nr);
						var indexInList = list.FindIndex(x => x.FA_Nr == l.FA_Nr);
						if(indexInList == 0)
						{
							list[indexInList].RestBestand = Math.Round(list[indexInList].Im_Lager_Ne_magazine ?? 0m, 2) - list[indexInList].Bedarf_Nevoje ?? 0m;
						}
						else
						{
							list[indexInList].RestBestand = Math.Round(list[indexInList - 1].RestBestand, 2) - list[indexInList].Bedarf_Nevoje ?? 0m;
						}
					}
				}
				final_result.AddRange(list);
			}
			return final_result;
		}
		internal static List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1ALEntity> InnerSortAL(List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1ALEntity> entities)
		{
			List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1ALEntity> final_result = new List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1ALEntity>();
			var Dates = entities.Select(x => (DateTime)x.Afati_i_prestarise).Distinct().ToList();
			foreach(var item in Dates)
			{
				var withSameDate = entities.Where(x => x.Afati_i_prestarise == item).OrderBy(y => y.FA_Nr).ToList();
				final_result.AddRange(withSameDate);
			}
			return final_result;
		}
		//czech
		internal static List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1CZEntity> CalculateRestbestandCZ(List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1CZEntity> entities)
		{
			List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1CZEntity> final_result = new List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1CZEntity>();
			if(entities == null || entities.Count <= 0)
				return null;

			var groupebByMaterial = entities.Select(x => x.Cislo_Material).Distinct().ToList();


			foreach(var item in groupebByMaterial)
			{
				var list = InnerSortCZ(entities.Where(x => x.Cislo_Material == item).ToList());

				if(list == null || list.Count <= 0)
					continue;

				// - First
				// ---
				if(list.Count == 1)
				{
					var onlyindex = list.FindIndex(x => x.FA_Cislo == list[0].FA_Cislo);
					list[onlyindex].RestBestand = Math.Round(list[onlyindex].Im_Lager ?? 0m, 2) - list[onlyindex].Bedarf ?? 0m;
				}
				else
				{
					foreach(var l in list)
					{
						//var indexInEntities = entities.FindIndex(x => x.FA_Nr == l.FA_Nr);
						var indexInList = list.FindIndex(x => x.FA_Cislo == l.FA_Cislo);
						if(indexInList == 0)
						{
							list[indexInList].RestBestand = Math.Round(list[indexInList].Im_Lager ?? 0m, 2) - list[indexInList].Bedarf ?? 0m;
						}
						else
						{
							list[indexInList].RestBestand = Math.Round(list[indexInList - 1].RestBestand, 2) - list[indexInList].Bedarf ?? 0m;
						}
					}
				}
				final_result.AddRange(list);
			}
			return final_result;
		}
		internal static List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1CZEntity> InnerSortCZ(List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1CZEntity> entities)
		{
			List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1CZEntity> final_result = new List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1CZEntity>();
			var Dates = entities.Select(x => (DateTime)x.Termin_Rezarna).Distinct().ToList();
			foreach(var item in Dates)
			{
				var withSameDate = entities.Where(x => x.Termin_Rezarna == item).OrderBy(y => y.FA_Cislo).ToList();
				final_result.AddRange(withSameDate);
			}
			return final_result;
		}
		//tunisia
		internal static List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1TNEntity> InnerSortTN(List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1TNEntity> entities)
		{
			List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1TNEntity> final_result = new List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1TNEntity>();
			var Dates = entities.Select(x => (DateTime)x.Termin_Schneiderei).Distinct().ToList();
			foreach(var item in Dates)
			{
				var withSameDate = entities.Where(x => x.Termin_Schneiderei == item).OrderBy(y => y.Fertigungsnummer).ToList();
				final_result.AddRange(withSameDate);
			}
			return final_result;
		}
		internal static List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1TNEntity> CalculateRestbestandTN(List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1TNEntity> entities)
		{
			List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1TNEntity> final_result = new List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1TNEntity>();
			if(entities == null || entities.Count <= 0)
				return null;

			var groupebByMaterial = entities.Select(x => x.ROH_Artikelnummer).Distinct().ToList();


			foreach(var item in groupebByMaterial)
			{
				var list = InnerSortTN(entities.Where(x => x.ROH_Artikelnummer == item).ToList());

				if(list == null || list.Count <= 0)
					continue;

				// - First
				// ---
				if(list.Count == 1)
				{
					var onlyindex = list.FindIndex(x => x.Fertigungsnummer == list[0].Fertigungsnummer);
					list[onlyindex].RestBestand = (list[onlyindex].Ve_skladu.HasValue) ? Math.Round(list[onlyindex].Ve_skladu.Value, 2) - list[onlyindex].Bedarf ?? 0m
						: 0;
				}
				else
				{
					foreach(var l in list)
					{
						//var indexInEntities = entities.FindIndex(x => x.FA_Nr == l.FA_Nr);
						var indexInList = list.FindIndex(x => x.Fertigungsnummer == l.Fertigungsnummer);
						if(indexInList == 0)
						{
							list[indexInList].RestBestand = (list[indexInList].Ve_skladu.HasValue) ? Math.Round(list[indexInList].Ve_skladu.Value, 2) - list[indexInList].Bedarf ?? 0m
								: 0;
						}
						else
						{
							list[indexInList].RestBestand = (list[indexInList - 1].RestBestand != 0) ? Math.Round(list[indexInList - 1].RestBestand, 2) - list[indexInList].Bedarf ?? 0m
								: 0;
						}
					}
				}
				final_result.AddRange(list);
			}
			return final_result;
		}


		#region CTS prices calculations
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


		internal static decimal CalculateSingleCopperSurcharge(bool fixedPrice,
			int del,
			decimal cuGewicht,
			int kupferbasis)
		{
			return !fixedPrice
				? decimal.Round((Convert.ToDecimal((del * 1.01m) - kupferbasis) / 100m) * cuGewicht, 2)
				: 0;
		}
		#endregion

	}
}
