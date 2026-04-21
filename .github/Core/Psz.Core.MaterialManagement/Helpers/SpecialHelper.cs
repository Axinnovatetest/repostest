using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Psz.Core.MaterialManagement.Helpers
{
	public class SpecialHelper
	{
		/// <summary>
		/// return the list of Lagers for specefic country and Unit
		/// </summary>
		/// <param name="countryId"></param>
		/// <param name="unitId"></param>
		/// <returns></returns>
		public static List<int?> LagersListStore(int countryId, int unitId)
		{
			var halls = Infrastructure.Data.Access.Tables.WPL.HallAccess.GetHallsWithLagers();
			if(countryId > 0 && unitId > 0)
			{
				var res = halls.Where(temp => temp.CountryId == countryId && temp.Id == unitId).Select(x => x.LagerortId).ToList();
				return res;
			}
			else if(countryId > 0)
			{
				return halls.Where(temp => temp.CountryId == countryId).Select(x => x.LagerortId).ToList();
			}
			return null;
		}

		public static List<int?> AdditionalLagers(int CountryId, int HallId)
		{
			//PSZ TN
			if(CountryId == 1040 && HallId == 34)
			{ return new List<int?>() { 7, 4, 10, 23, 56, 29, 30 }; }
			// WS
			if(CountryId == 1040 && HallId == 33)
			{ return new List<int?>() { 42, 41, 40, 57, 49, 46, 47 }; }
			// BE-TN
			if(CountryId == 1040 && HallId == 36)
			{ return new List<int?>() { 58, 60, 65, 61, 59, 63, 64 }; }
			// GZ - TN
			if(CountryId == 1040 && HallId == 38)
			{ return new List<int?>() { 101, 102, 109, 105, 104, 107, 108 }; }
			// Tschechien
			if(CountryId == 1038)
			{ return new List<int?>() { 6, 3, 52, 9, 53, 17 }; }
			//Albania
			if(CountryId == 1037)
			{ return new List<int?>() { 26, 24, 25, 50, 34, 35 }; }
			//Germany
			if(CountryId == 1039)
			{ return new List<int?>() { 15, 8 }; }
			return null;
		}
		public static List<int> AdditionalLagers(int? CountryId, int? HallId)
		{
			var tn = new List<int>() { 7, 4, 10, 23, 56, 29, 30 };
			var ws = new List<int>() { 42, 41, 40, 57, 49, 46, 47 };
			var be = new List<int>() { 58, 60, 65, 61, 59, 63, 64 };
			var gz = new List<int>() { 101, 102, 109, 105, 104, 107, 108 };
			// 
			var cz = new List<int>() { 6, 3, 52, 9, 53, 17 };
			var al = new List<int>() { 26, 24, 25, 50, 34, 35 };
			var de = new List<int>() { 15, 8 };

			// - No country
			if(!CountryId.HasValue)
			{
				var results = new List<int> { };
				results.AddRange(tn);
				results.AddRange(ws);
				results.AddRange(be);
				results.AddRange(gz);
				results.AddRange(cz);
				results.AddRange(al);
				results.AddRange(de);

				// -
				return results;
			}
			else if(HallId.HasValue)
			{
				if(CountryId == 1040) // - Tunisia
				{
					if(!HallId.HasValue) // - No hall
					{
						var results = new List<int> { };
						results.AddRange(tn);
						results.AddRange(ws);
						results.AddRange(be);
						results.AddRange(gz);

						// -
						return results;
					}
					else
					{
						switch(HallId.Value)
						{
							case 34:
								return tn;
							case 33:
								return ws;
							case 36:
								return be;
							case 38:
								return gz;
							default:
								return new List<int> { };
						}
					}
				}
				else if(CountryId == 1039) // - Germany
				{
					return de;
				}
				else if(CountryId == 1038) // - Tschechien
				{
					return cz;
				}
				else if(CountryId == 1037) // - Albania
				{
					return al;
				}
			}

			// - 
			return new List<int> { };
		}
		public static List<int> AdditionalLagerssss(int CountryId, int HallId)
		{
			var tn = new List<int>() { 7, 4, 10, 23, 56, 29, 30 };
			var ws = new List<int>() { 42, 41, 40, 57, 49, 46, 47 };
			var be = new List<int>() { 58, 60, 65, 61, 59, 63, 64 };
			var gz = new List<int>() { 101, 102, 109, 105, 104, 107, 108 };
			// 
			var cz = new List<int>() { 6, 3, 52, 9, 53, 17 };
			var al = new List<int>() { 26, 24, 25, 50, 34, 35 };
			var de = new List<int>() { 15, 8 };

			// - No country

			if(CountryId == 1040) // - Tunisia
			{
				if(HallId == 0) // - No hall
				{
					var results = new List<int> { };
					results.AddRange(tn);
					results.AddRange(ws);
					results.AddRange(be);
					results.AddRange(gz);

					// -
					return results;
				}
				else
				{
					switch(HallId)
					{
						case 34:
							return tn;
						case 33:
							return ws;
						case 36:
							return be;
						case 38:
							return gz;
						default:
							return new List<int> { };
					}
				}
			}
			else if(CountryId == 1039) // - Germany
			{
				return de;
			}
			else if(CountryId == 1038) // - Tschechien
			{
				return cz;
			}
			else if(CountryId == 1037) // - Albania
			{
				return al;
			}

			return null;
		}
		public static Tuple<int, int> GetMainAndProductionLagers(int CountryId, int HallId)
		{
			//PSZ TN
			if(CountryId == 1040 && HallId == 34)
			{ return new Tuple<int, int>(7, 77); }
			// WS
			if(CountryId == 1040 && HallId == 33)
			{ return new Tuple<int, int>(42, 420); }
			// BE-TN
			if(CountryId == 1040 && HallId == 36)
			{ return new Tuple<int, int>(60, 580); }
			// GZ - TN
			if(CountryId == 1040 && HallId == 38)
			{ return new Tuple<int, int>(102, 103); }
			// Tschechien
			if(CountryId == 1038)
			{ return new Tuple<int, int>(6, 66); }
			//Albania
			if(CountryId == 1037)
			{ return new Tuple<int, int>(26, 260); }
			//Germany
			if(CountryId == 1039)
			{ return new Tuple<int, int>(15, int.MinValue); }
			return null;
		}


		public static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
		{
			DateTime jan1 = new DateTime(year, 1, 1);
			int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

			// Use first Thursday in January to get first week of the year as
			// it will never be in Week 52/53
			DateTime firstThursday = jan1.AddDays(daysOffset);
			var cal = CultureInfo.CurrentCulture.Calendar;
			int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

			var weekNum = weekOfYear;
			// As we're adding days to a date in Week 1,
			// we need to subtract 1 in order to get the right date for week #1
			if(firstWeek == 1)
			{
				weekNum -= 1;
			}

			// Using the first Thursday as starting week ensures that we are starting in the right year
			// then we add number of weeks multiplied with days
			var result = firstThursday.AddDays(weekNum * 7);

			// Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
			return result.AddDays(-3);
		}
		public static int GetHauptLager(int fetigungLager)
		{
			switch(fetigungLager)
			{
				case 7:
					return 4;
				case 42:
					return 41;
				case 26:
					return 24;
				case 6:
				case 21:
					return 3;
				case 60:
					return 58;
				default:
					return 0;
			}
		}
		public static List<int> GetFertigungLager(int hauptLager)
		{
			switch(hauptLager)
			{
				case 4:
					return new List<int>() { 7 };
				case 41:
					return new List<int>() { 42 };
				case 24:
					return new List<int>() { 26 };
				case 3:
					return new List<int>() { 6, 21 };
				case 58:
					return new List<int>() { 60 };
				default:
					return new List<int>() { 0 };
			}
		}
		/// <summary>
		/// extract the week in a year from a given date 
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public static int ExtractIsoWeek(DateTime time)
		{
			//DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
			//if(day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
			//{
			//	time = time.AddDays(3);
			//}
			//return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
			return ISOWeek.GetWeekOfYear(time);
		}
		/// <summary>
		/// return true if week1 equal to week2 otherwise return false
		/// </summary>
		/// <param name="week1"></param>
		/// <param name="week2"></param>
		/// <returns></returns>
		public static bool CompareWeekPattern(string week1, string week2)
		{
			string pattern = @"[0-9]['/'][0-9][0-9][0-9][0-9]";
			Regex rg = new Regex(pattern);
			if(!rg.IsMatch(week1) || !rg.IsMatch(week2))
				return false;
			var temp1 = week1.Split("/");
			var temp2 = week2.Split("/");
			return (int.Parse(temp1[0]) == int.Parse(temp2[0])) && (int.Parse(temp1[1]) == int.Parse(temp2[1]));
		}
		/// <summary>
		/// return true if week 1 is afer week2 otherwise it return false
		/// </summary>
		/// <param name="week1"></param>
		/// <param name="week2"></param>
		/// <returns></returns>
		public static bool CompareWeekPatternDiff(string week1, string week2)
		{
			string pattern = @"[0-9]['/'][0-9][0-9][0-9][0-9]";
			Regex rg = new Regex(pattern);
			if(!rg.IsMatch(week1) || !rg.IsMatch(week2))
				return false;
			var temp1 = week1.Split("/");
			var temp2 = week2.Split("/");

			var x = (int.Parse(temp1[1]) > int.Parse(temp2[1])) || ((int.Parse(temp1[1]) == int.Parse(temp2[1])) && (int.Parse(temp1[0]) > int.Parse(temp2[0])));
			return x;
		}
		public static Orders.Models.Statistics.PlantsAndLagers GetPlant(string plant)
		{
			var res = plant.ToUpper() switch
			{
				//"TN" => new Orders.Models.Statistics.PlantsAndLagers() { PlantFull = "Tunesien", Plant = "TN", LagerHaupt = 4, Lager_fert = 7, Lager_fert_2 = string.Empty },
				"WS/TN" => new Orders.Models.Statistics.PlantsAndLagers() { PlantFull = "Bouhjar", Plant = "WS", LagerHaupt = 41, Lager_fert = 42, Lager_fert_2 = "or (Fertigung.Lagerort_id=7)" },
				"AL" => new Orders.Models.Statistics.PlantsAndLagers() { PlantFull = "Albanien", Plant = "AL", LagerHaupt = 24, Lager_fert = 26, Lager_fert_2 = string.Empty },
				"CZ" => new Orders.Models.Statistics.PlantsAndLagers() { PlantFull = "Tschechien", Plant = "CZ", LagerHaupt = 3, Lager_fert = 6, Lager_fert_2 = "or (Fertigung.Lagerort_id=21)" },
				//"BETN" => new Orders.Models.Statistics.PlantsAndLagers() { PlantFull = "Bennane", Plant = "BETN", LagerHaupt = 58, Lager_fert = 60, Lager_fert_2 = string.Empty },
				"GZ" => new Orders.Models.Statistics.PlantsAndLagers() { PlantFull = "Ghezala", Plant = "GZ", LagerHaupt = 101, Lager_fert = 102, Lager_fert_2 = string.Empty },
				_ => null
			};
			return res;
		}
		public static List<Orders.Models.Statistics.PlantsAndLagers> GetAllPlants()
		{
			return new List<Orders.Models.Statistics.PlantsAndLagers>() {
				//new Orders.Models.Statistics.PlantsAndLagers() {PlantFull= "Tunesien",Plant= "TN", LagerHaupt = 4,Lager_fert = 7,Lager_fert_2 = string.Empty },
				new Orders.Models.Statistics.PlantsAndLagers() { PlantFull = "Bouhjar", Plant = "WS/TN", LagerHaupt = 41, Lager_fert = 42, Lager_fert_2 = string.Empty },
				new Orders.Models.Statistics.PlantsAndLagers() {PlantFull = "Albanien", Plant = "AL", LagerHaupt = 24, Lager_fert = 26, Lager_fert_2 = string.Empty },
				new Orders.Models.Statistics.PlantsAndLagers() {PlantFull = "Tschechien", Plant = "CZ", LagerHaupt = 3, Lager_fert = 6, Lager_fert_2 = "or (Fertigung.Lagerort_id=21)" },
				//new Orders.Models.Statistics.PlantsAndLagers() {PlantFull = "Bennane", Plant = "BETN", LagerHaupt = 58, Lager_fert = 60, Lager_fert_2 = string.Empty },
				new Orders.Models.Statistics.PlantsAndLagers () { PlantFull = "Ghezala", Plant = "GZ", LagerHaupt = 4, Lager_fert = 102, Lager_fert_2 = string.Empty }
				 };
		}
		public static List<int> GetLagersPerMainLager(int inp)
		{
			var lagers = inp switch
			{
				6 => new List<int>() { 6, 66, 7, 60, 42, 26, 15, 102 },
				60 => new List<int>() { 60, 580, 7, 6, 42, 26, 15, 102 },
				7 => new List<int>() { 7, 77, 6, 60, 42, 26, 15, 102 },
				42 => new List<int>() { 42, 420, 7, 60, 6, 26, 15, 102 },
				26 => new List<int>() { 26, 260, 7, 60, 42, 6, 15, 102 },
				15 => new List<int>() { 15, -1, 7, 60, 42, 26, 6, 102 },
				102 => new List<int>() { 102, 103, 7, 60, 42, 26, 15, 6 },
				_ => null
			};
			return lagers;
		}
		public static List<int> GetLMainLagers()
		{
			var lagers = new List<int>()
			{
				6 ,
				60 ,
				7 ,
				42 ,
				26 ,
				15 ,
				102
			};
			return lagers;
		}

		/// <summary>
		/// update a rahmen position qty according to the attached bestell position.
		/// Updating the Bestellete artikel in the transaction is done inside this method.. No need to update it again outside 
		/// </summary>
		/// <typeparam name="T"> Response Type.</typeparam>
		/// <param name="updatedBSPosition"> Updated bestellete Artikel with new values. // EXCEPTION with Delete the RA_Pos_zu_Bestellposition needs to be null before update</param>
		/// <param name="oldRahmePositionNr"> -1 in case it's a new bestellete Artikel</param>
		/// <param name="diffQty"> Difference in quantity between old and new Anzahl </param>
		/// <param name="boTransactions"> Transaction</param>
		/// <returns></returns>
		public static ResponseModel<T> UpdateRahmenBS<T>(Infrastructure.Data.Entities.Tables.MTM.Bestellte_ArtikelEntity updatedBSPosition, int oldRahmePositionNr,
			decimal diffQty, Infrastructure.Services.Utils.TransactionsManager boTransactions, int? SupplierId = null, bool setInPos = false, UserModel user = null)
		{
			try
			{
				var oldbePos = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.Get(updatedBSPosition.Nr);
				var rahmenPos = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(updatedBSPosition.RA_Pos_zu_Bestellposition ?? -1);
				//check if rahmen have same supplier --- Mario Heidenreich request (mail 26/09/2024)
				var rahmenExtension = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(rahmenPos?.AngebotNr ?? -1);
				var bs = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.GetWithTransaction(updatedBSPosition.Bestellung_Nr ?? -1, boTransactions.connection, boTransactions.transaction);
				if(rahmenExtension != null)
				{
					var bsSupplier = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.GetByNummer(bs.Lieferanten_Nr ?? -1);
					if(bsSupplier != null && bsSupplier.Nr != rahmenExtension.SupplierId)
						return ResponseModel<T>.FailureResponse("Chosen rahmen and order does not have the same supplier .");
				}
				//choosing same RA
				if(updatedBSPosition.RA_Pos_zu_Bestellposition.HasValue && updatedBSPosition.RA_Pos_zu_Bestellposition.Value == oldRahmePositionNr && oldRahmePositionNr != -1)
				{
					if(rahmenPos is null)
						return ResponseModel<T>.FailureResponse($"New Rahmen position {updatedBSPosition.RA_Pos_zu_Bestellposition.Value} was not found");

					if(diffQty > rahmenPos.Anzahl)
						return ResponseModel<T>.FailureResponse($"Added Ordred quantity [{updatedBSPosition.Anzahl}] is bigger then rahmen available quantity [{rahmenPos.Anzahl + oldbePos.Anzahl}]");

					//updating RA qty
					rahmenPos.Anzahl -= diffQty;
					rahmenPos.Geliefert += diffQty;

					//updateing BS price
					updatedBSPosition = GetRightPrice(rahmenPos, updatedBSPosition);

					// Update Old and New RA Position. and bestellteArtickel Position with new prices.
					Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateWithTransaction(rahmenPos, boTransactions.connection, boTransactions.transaction);

				}
				//choosing new rahmen
				else if(oldRahmePositionNr == -1 && updatedBSPosition.RA_Pos_zu_Bestellposition.HasValue && updatedBSPosition.RA_Pos_zu_Bestellposition.Value != -1)
				{
					if(rahmenPos is null)
						return ResponseModel<T>.FailureResponse($"New Rahmen position {updatedBSPosition.RA_Pos_zu_Bestellposition.Value} was not found");

					if(diffQty > rahmenPos.Anzahl)
						return ResponseModel<T>.FailureResponse($"Ordred quantity [{updatedBSPosition.Anzahl}] is bigger then rahmen available quantity [{rahmenPos.Anzahl}]");

					//updating RAs Qtys
					rahmenPos.Anzahl -= updatedBSPosition.Anzahl;
					rahmenPos.Geliefert += updatedBSPosition.Anzahl;

					//Update price of bestellteArtikel
					updatedBSPosition = GetRightPrice(rahmenPos, updatedBSPosition);

					// Update Old and New RA Position. and bestellteArtickel Position with new prices.
					Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateWithTransaction(rahmenPos, boTransactions.connection, boTransactions.transaction);

				}
				//choosing different RA
				else if(oldRahmePositionNr != -1
					&& updatedBSPosition.RA_Pos_zu_Bestellposition.HasValue && updatedBSPosition.RA_Pos_zu_Bestellposition != -1
					&& updatedBSPosition.RA_Pos_zu_Bestellposition.Value != oldRahmePositionNr)
				{
					if(rahmenPos is null)
						return ResponseModel<T>.FailureResponse($"New Rahmen position {updatedBSPosition.RA_Pos_zu_Bestellposition.Value} was not found");

					if(diffQty > rahmenPos.Anzahl)
						return ResponseModel<T>.FailureResponse($"Ordred quantity [{updatedBSPosition.Anzahl}] is bigger then rahmen available quantity [{rahmenPos.Anzahl}]");

					var oldRApos = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(oldRahmePositionNr);
					if(oldRApos is null)
						return ResponseModel<T>.FailureResponse($"Old Rahmen position {updatedBSPosition.RA_Pos_zu_Bestellposition.Value} was not found");

					var oldBSPositionEnity = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.Get(updatedBSPosition.Nr);

					//updating RAs Qtys
					oldRApos.Anzahl += oldBSPositionEnity.Anzahl;
					oldRApos.Geliefert -= oldBSPositionEnity.Anzahl;

					rahmenPos.Anzahl -= updatedBSPosition.Anzahl;
					rahmenPos.Geliefert += updatedBSPosition.Anzahl;

					//Update price of bestellteArtikel
					updatedBSPosition = GetRightPrice(rahmenPos, updatedBSPosition);


					// Update Old and New RA Position. and bestellteArtickel Position with new prices.
					Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateWithTransaction(new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> { oldRApos, rahmenPos }, boTransactions.connection, boTransactions.transaction);
				}
				//cancelling exsisting RA
				else if(!updatedBSPosition.RA_Pos_zu_Bestellposition.HasValue || updatedBSPosition.RA_Pos_zu_Bestellposition == -1 && oldRahmePositionNr != -1)
				{
					var oldRApos = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(oldRahmePositionNr);
					// No Previoussly selected RA
					if(oldRApos != null)
					{
						var oldBSPositionEnity = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.Get(updatedBSPosition.Nr);
						//updating RAs Qtys
						oldRApos.Anzahl += oldBSPositionEnity.Anzahl;
						oldRApos.Geliefert -= oldBSPositionEnity.Anzahl;

						var bestellnummern = Infrastructure.Data.Access.Tables.MTM.BestellnummernAccess.GetBySupplierIdArticleId(SupplierId ?? -1, updatedBSPosition.Artikel_Nr ?? -1);
						updatedBSPosition.Einzelpreis = bestellnummern?.Einkaufspreis;
						updatedBSPosition.Gesamtpreis = updatedBSPosition.Anzahl * bestellnummern?.Einkaufspreis;
						// Update OldRA Position.
						Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateWithTransaction(oldRApos, boTransactions.connection, boTransactions.transaction);
					}
				}

				Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.UpdateWithTransaction(updatedBSPosition, boTransactions.connection, boTransactions.transaction);
				//-- check consumption and send mail if it exxeds 75%
				// - 2025-08-27 - send mail on validate BE
				////var rahmenPosAfterUpdate = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetWithTransaction(updatedBSPosition.RA_Pos_zu_Bestellposition ?? -1,
				////	boTransactions.connection, boTransactions.transaction);
				////var consumptionPercentage = 0m;
				////if(rahmenPosAfterUpdate != null)
				////{
				////	var rahmen = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(rahmenPosAfterUpdate?.AngebotNr ?? -1);
				////	consumptionPercentage = (rahmenPosAfterUpdate.OriginalAnzahl is not null && rahmenPosAfterUpdate.OriginalAnzahl.Value > 0)
				////	   ? Math.Floor(((rahmenPosAfterUpdate.Geliefert / rahmenPosAfterUpdate.OriginalAnzahl) * 100) ?? 0)
				////	   : 0;
				////	if(consumptionPercentage >= Module.ModuleSettings.RahmenConsumptionNotificationThreshold)
				////	{
				////		Infrastructure.Services.Email.Helpers.SendConsumptionEmail(rahmenPosAfterUpdate, rahmen, consumptionPercentage);
				////	}
				////}

				return ResponseModel<T>.SuccessResponse();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return ResponseModel<T>.FailureResponse(e.Message);
			}
		}

		public static Infrastructure.Data.Entities.Tables.MTM.Bestellte_ArtikelEntity GetRightPrice(Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity rahmenPos, Infrastructure.Data.Entities.Tables.MTM.Bestellte_ArtikelEntity BSPosition)
		{
			if(rahmenPos is null)
				return BSPosition;

			var rahmenPrices = Infrastructure.Data.Access.Tables.CTS.RahmenPriceHistoryAccess.GetByMaxPriceAndDate(rahmenPos.Nr, BSPosition.Liefertermin ?? DateTime.Now);
			var rightPrice = rahmenPrices != null && rahmenPrices.Count > 0 ? rahmenPrices[0].Price : rahmenPos.Einzelpreis;
			BSPosition.Einzelpreis = rightPrice;
			BSPosition.Gesamtpreis = rightPrice * BSPosition.Anzahl;

			return BSPosition;
		}
	}
}
