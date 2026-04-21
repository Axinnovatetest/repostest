using System;
using System.Collections.Generic;
using System.Globalization;


namespace Psz.Core.CustomerService.Helpers
{
	public class HorizonsHelper
	{
		public static Enums.HorizonsEnums.horizons GetHorizonRange(DateTime date)
		{
			var horizons = Module.CTS.FAHorizons;
			////var H1StartWeek = GetIsoWeekNumber(DateTime.Now);
			////var H1EndWeek = H1StartWeek + horizons.H1KWLength;

			////var h1StartDate = Helpers.DelforHelper.FirstDateOfWeek(DateTime.Now.Year, H1StartWeek, new CultureInfo("de-DE"));
			////var h1EndDate = Helpers.DelforHelper.FirstDateOfWeek(DateTime.Now.Year, H1EndWeek, new CultureInfo("de-DE")).AddDays(6);

			////var h2StartWeek = H1EndWeek + 1;
			////var H2EndWeek = h2StartWeek + horizons.H2KWLength - 1;

			////var h2StartDate = Helpers.DelforHelper.FirstDateOfWeek(DateTime.Now.Year, h2StartWeek, new CultureInfo("de-DE"));
			////var h2EndDate = Helpers.DelforHelper.FirstDateOfWeek(DateTime.Now.Year, H2EndWeek, new CultureInfo("de-DE")).AddDays(6);

			////var H3StartWeek = H2EndWeek + 1;
			////var h3StartDate = Helpers.DelforHelper.FirstDateOfWeek(DateTime.Now.Year, H3StartWeek, new CultureInfo("de-DE"));

			// 2024-01-25 - Khelil change H1 to 41 days
			var h1EndDate = DateTime.Today.AddDays(horizons.H1LengthInDays);
			var h2StartDate = h1EndDate.AddDays(1);
			var h2EndDate = h2StartDate.AddDays(7 * horizons.H2KWLength - 1);
			var h3StartDate = h2EndDate.AddDays(1);


			if(date <= h1EndDate)
				return Enums.HorizonsEnums.horizons.horizon1;
			else if(IsDateInRange(date, h2StartDate, h2EndDate))
				return Enums.HorizonsEnums.horizons.horizon2;
			else if(date >= h3StartDate)
				return Enums.HorizonsEnums.horizons.horizon3;
			else
				return Enums.HorizonsEnums.horizons.out_of_horizons;

		}
		public static int GetIsoWeekNumber(DateTime date)
		{
			return System.Globalization.ISOWeek.GetWeekOfYear(date);
		}
		public static bool IsDateInRange(DateTime dateToCheck, DateTime startDate, DateTime endDate)
		{
			return dateToCheck >= startDate && dateToCheck <= endDate;
		}
		//
		public static bool userHasFaCreateHorizonRight(DateTime date, Identity.Models.UserModel user, out List<string> errors)
		{
			errors = new List<string>();
			// - 2023-12-05
			if(user?.SuperAdministrator == true || user?.IsGlobalDirector == true)
			{
				return true;
			}
			var hDate = GetHorizonRange(date);
			var createFAH1Right = user.Access.CustomerService.FACreateHorizon1;
			var createFAH2Right = user.Access.CustomerService.FACreateHorizon2;
			var createFAH3Right = user.Access.CustomerService.FACreateHorizon3;
			if(date.dateIsInPast())
			{
				errors.Add("FA date should not be in the past.");
				return false;
			}
			if(!createFAH1Right && !createFAH2Right && !createFAH3Right)
			{
				errors.Add("You have no FA creation horizon(s) assigned.");
				return false;
			}

			var check = checkIfUserCanPerfomAction(new HorizonsRightsCarrier
			{
				H1 = createFAH1Right,
				H2 = createFAH2Right,
				H3 = createFAH3Right,
			}, hDate);
			if(!check)
			{
				errors.Add($"The date [{date.ToString("dd/MM/yyyy")}] of the FA you want to create is out of your assigned horizon(s).");
				return false;
			}
			if(errors != null && errors.Count > 0)
				return false;
			else
				return true;
		}
		public static bool userHasFaUpdateTerminHorizonRight(DateTime newDate, DateTime oldDate, Identity.Models.UserModel user, out List<string> errors)
		{
			errors = new List<string>();// - 2023-12-05
			if(user?.SuperAdministrator == true || user?.IsGlobalDirector == true)
			{
				return true;
			}
			var hnewDate = GetHorizonRange(newDate);
			var holdDate = GetHorizonRange(oldDate);
			var updateFADateH1Right = user.Access.CustomerService.FAUpdateTerminHorizon1;
			var updateFADateH2Right = user.Access.CustomerService.FAUpdateTerminHorizon2;
			var updateFADateH3Right = user.Access.CustomerService.FAUpdateTerminHorizon3;

			// - oldDate in the past
			if(oldDate.dateIsInPast())
			{
				//// - newDate in the future not allowed
				//if(DateTime.Today <= newDate)
				//{
				//	errors.Add($"FA in the past [{oldDate.ToString("dd.MM.yyyy")}] can only be moved to a date in the past (before today).");
				//	return false;
				//}
				// - newDate before oldDate not allowed
				if(newDate < oldDate)
				{
					errors.Add($"FA in the past cannot be moved to a date before current [{oldDate.ToString("dd.MM.yyyy")}].");
					return false;
				}
			}
			else
			{
				// - oldDate in the future & newDate in the past not allowed
				if(newDate.dateIsInPast())
				{
					errors.Add($"FA cannot be moved to a date in the past (before today).");
					return false;
				}
			}

			if(!updateFADateH1Right && !updateFADateH2Right && !updateFADateH3Right)
			{
				errors.Add("You have no FA Date update horizon assigned.");
				return false;
			}

			if(hnewDate != holdDate)
			{
				var canSwitch = checkIfUserCanSwitchHorizons(new HorizonsRightsCarrier
				{
					H1 = updateFADateH1Right,
					H2 = updateFADateH2Right,
					H3 = updateFADateH3Right
				}, hnewDate, holdDate);
				if(!canSwitch)
				{
					errors.Add($"you must have {holdDate.ToString()} and {hnewDate.ToString()} assigned in order to update this FA Date.");
					return false;
				}
			}


			var check = checkIfUserCanPerfomAction(new HorizonsRightsCarrier
			{
				H1 = updateFADateH1Right,
				H2 = updateFADateH2Right,
				H3 = updateFADateH3Right
			}, hnewDate);
			if(!check)
			{
				errors.Add($"the new date [{newDate.ToString("dd/MM/yyyy")}] you are trying to affect is out of your assigned horizon(s)");
				return false;
			}

			if(errors != null && errors.Count > 0)
				return false;
			else
				return true;

		}
		public static bool userHasFaCancelHorizonRight(DateTime date, Identity.Models.UserModel user, out List<string> errors)
		{
			errors = new List<string>();// - 2023-12-05
			if(user?.SuperAdministrator == true || user?.IsGlobalDirector == true)
			{
				return true;
			}
			var hDate = GetHorizonRange(date);
			var cancelFAH1Right = user.Access.CustomerService.FACancelHorizon1;
			var cancelFAH2Right = user.Access.CustomerService.FACancelHorizon2;
			var cancelFAH3Right = user.Access.CustomerService.FACancelHorizon3;
			if(!cancelFAH1Right && !cancelFAH2Right && !cancelFAH3Right)
			{
				errors.Add("You have no FA Cancel horizon assigned.");
				return false;
			}

			var check = checkIfUserCanPerfomAction(new HorizonsRightsCarrier
			{
				H1 = cancelFAH1Right,
				H2 = cancelFAH2Right,
				H3 = cancelFAH3Right
			}, hDate);
			if(!check)
			{
				errors.Add($"The date [{date.ToString("dd/MM/yyyy")}] of the FA you'r trying to cancel is out of your assigned horizon(s)");
				return false;
			}

			if(errors != null && errors.Count > 0)
				return false;
			else
				return true;
		}
		public static bool userHasABPosHorizonRight(DateTime newDate, DateTime oldDate, Identity.Models.UserModel user, out List<string> errors)
		{
			errors = new List<string>();// - 2023-12-05
			if(user?.SuperAdministrator == true || user?.IsGlobalDirector == true)
			{
				return true;
			}
			var hnewDate = GetHorizonRange(newDate);
			var holdDate = GetHorizonRange(oldDate);
			var abPosH1Right = user.Access.CustomerService.ABPosHorizon1;
			var abPosH2Right = user.Access.CustomerService.ABPosHorizon2;
			var abPosH3Right = user.Access.CustomerService.ABPosHorizon3;

			if(newDate.dateIsInPast())
			{
				errors.Add("AB position date should not be in the past.");
				return false;
			}

			if(!abPosH1Right && !abPosH2Right && !abPosH3Right)
			{
				errors.Add("You have no AB position update horizon assigned.");
				return false;
			}

			if(hnewDate != holdDate)
			{
				var canSwitch = checkIfUserCanSwitchHorizons(new HorizonsRightsCarrier
				{
					H1 = abPosH1Right,
					H2 = abPosH2Right,
					H3 = abPosH3Right
				}, hnewDate, holdDate);
				if(!canSwitch)
				{
					errors.Add($"you must have {holdDate.ToString()} and {hnewDate.ToString()} assigned in order to update this AB position with this date [{newDate.ToString("dd/MM/yyyy")}].");
					return false;
				}
			}


			var check = checkIfUserCanPerfomAction(new HorizonsRightsCarrier
			{
				H1 = abPosH1Right,
				H2 = abPosH2Right,
				H3 = abPosH3Right
			}, hnewDate);
			if(!check)
			{
				errors.Add($"the delivery date [{newDate.ToString("dd/MM/yyyy")}] of the AB position you are trying to edit is out of your assigned horizon(s)");
				return false;
			}

			if(errors != null && errors.Count > 0)
				return false;
			else
				return true;
		}
		public static bool userHasGSPosHorizonRight(DateTime date, Identity.Models.UserModel user, out List<string> errors)
		{
			errors = new List<string>();// - 2023-12-05
			if(user?.SuperAdministrator == true || user?.IsGlobalDirector == true)
			{
				return true;
			}
			var hDate = GetHorizonRange(date);
			var gsPosH1Right = user.Access.CustomerService.GSPosHorizon1;
			var gsPosH2Right = user.Access.CustomerService.GSPosHorizon2;
			var gsPosH3Right = user.Access.CustomerService.GSPosHorizon3;

			//if(date.dateIsInPast())
			//{
			//	errors.Add("GS position date should not be in the past.");
			//	return false;
			//}

			if(!gsPosH1Right && !gsPosH2Right && !gsPosH3Right)
			{
				errors.Add("You have no GS Position Edit horizon assigned.");
				return false;
			}

			var check = checkIfUserCanPerfomAction(new HorizonsRightsCarrier
			{
				H1 = gsPosH1Right,
				H2 = gsPosH2Right,
				H3 = gsPosH3Right
			}, hDate);
			if(!check)
				errors.Add($"the delivery date [{date.ToString("dd/MM/yyyy")}] of the GS position you'r trying to edit is out of your assigned horizon(s)");

			if(errors != null && errors.Count > 0)
				return false;
			else
				return true;

		}
		public static bool userHasLSPosHorizonRight(DateTime newDate, DateTime oldDate, Identity.Models.UserModel user, out List<string> errors)
		{
			errors = new List<string>();// - 2023-12-05
			if(user?.SuperAdministrator == true || user?.IsGlobalDirector == true)
			{
				return true;
			}
			var hnewDate = GetHorizonRange(newDate);
			var holdDate = GetHorizonRange(oldDate);
			var lsPosH1Right = user.Access.CustomerService.LSPosHorizon1;
			var lsPosH2Right = user.Access.CustomerService.LSPosHorizon2;
			var lsPosH3Right = user.Access.CustomerService.LSPosHorizon3;

			if(newDate.dateIsInPast())
			{
				errors.Add("LS position date should not be in the past.");
				return false;
			}

			if(!lsPosH1Right && !lsPosH2Right && !lsPosH3Right)
			{
				errors.Add("You have no LS position update horizon assigned.");
				return false;
			}

			if(hnewDate != holdDate)
			{
				var canSwitch = checkIfUserCanSwitchHorizons(new HorizonsRightsCarrier
				{
					H1 = lsPosH1Right,
					H2 = lsPosH2Right,
					H3 = lsPosH3Right
				}, hnewDate, holdDate);
				if(!canSwitch)
				{
					errors.Add($"you must have {holdDate.ToString()} and {hnewDate.ToString()} assigned in order to update this LS position Date.");
					return false;
				}
			}


			var check = checkIfUserCanPerfomAction(new HorizonsRightsCarrier
			{
				H1 = lsPosH1Right,
				H2 = lsPosH2Right,
				H3 = lsPosH3Right
			}, hnewDate);
			if(!check)
			{
				errors.Add($"the delivery date [{newDate.ToString("dd/MM/yyyy")}] of the LS you are trying to edit is out of your assigned horizon(s)");
				return false;
			}

			if(errors != null && errors.Count > 0)
				return false;
			else
				return true;

		}
		public static bool userHasRAPosHorizonRight(DateTime newDate, DateTime oldDate, Identity.Models.UserModel user, out List<string> errors)
		{
			errors = new List<string>();// - 2023-12-05
			if(user?.SuperAdministrator == true || user?.IsGlobalDirector == true)
			{
				return true;
			}
			var hnewDate = GetHorizonRange(newDate);
			var holdDate = GetHorizonRange(oldDate);
			var raPosH1Right = user.Access.CustomerService.RAPosHorizon1;
			var raPosH2Right = user.Access.CustomerService.RAPosHorizon2;
			var raPosH3Right = user.Access.CustomerService.RAPosHorizon3;

			if(newDate.dateIsInPast())
			{
				errors.Add("RA position date should not be in the past.");
				return false;
			}

			if(!raPosH1Right && !raPosH2Right && !raPosH3Right)
			{
				errors.Add("You have no RA position update horizon assigned.");
				return false;
			}

			if(hnewDate != holdDate)
			{
				var canSwitch = checkIfUserCanSwitchHorizons(new HorizonsRightsCarrier
				{
					H1 = raPosH1Right,
					H2 = raPosH2Right,
					H3 = raPosH3Right
				}, hnewDate, holdDate);
				if(!canSwitch)
				{
					errors.Add($"you must have {holdDate.ToString()} and {hnewDate.ToString()} assigned in order to update this RA position end date.");
					return false;
				}
			}


			var check = checkIfUserCanPerfomAction(new HorizonsRightsCarrier
			{
				H1 = raPosH1Right,
				H2 = raPosH2Right,
				H3 = raPosH3Right
			}, hnewDate);
			if(!check)
			{
				errors.Add($"the end date [{newDate.ToString("dd/MM/yyyy")}] of the RA position you are trying to edit is out of your assigned horizon(s)");
				return false;
			}

			if(errors != null && errors.Count > 0)
				return false;
			else
				return true;
		}
		public static bool userHasRGPosHorizonRight(DateTime newDate, DateTime oldDate, Identity.Models.UserModel user, out List<string> errors)
		{
			errors = new List<string>();// - 2023-12-05
			if(user?.SuperAdministrator == true || user?.IsGlobalDirector == true)
			{
				return true;
			}
			var hnewDate = GetHorizonRange(newDate);
			var holdDate = GetHorizonRange(oldDate);
			var rgPosH1Right = user.Access.CustomerService.RGPosHorizon1;
			var rgPosH2Right = user.Access.CustomerService.RGPosHorizon2;
			var rgPosH3Right = user.Access.CustomerService.RGPosHorizon3;

			if(newDate.dateIsInPast())
			{
				errors.Add("RG position date should not be in the past.");
				return false;
			}

			if(!rgPosH1Right && !rgPosH2Right && !rgPosH3Right)
			{
				errors.Add("You have no RG position update horizon assigned.");
				return false;
			}

			if(hnewDate != holdDate)
			{
				var canSwitch = checkIfUserCanSwitchHorizons(new HorizonsRightsCarrier
				{
					H1 = rgPosH1Right,
					H2 = rgPosH2Right,
					H3 = rgPosH3Right
				}, hnewDate, holdDate);
				if(!canSwitch)
				{
					errors.Add($"you must have {holdDate.ToString()} and {hnewDate.ToString()} assigned in order to update this RG position end date.");
					return false;
				}
			}


			var check = checkIfUserCanPerfomAction(new HorizonsRightsCarrier
			{
				H1 = rgPosH1Right,
				H2 = rgPosH2Right,
				H3 = rgPosH3Right
			}, hnewDate);
			if(!check)
			{
				errors.Add($"the end date [{newDate.ToString("dd/MM/yyyy")}] of the RG position you are trying to edit is out of your assigned horizon(s)");
				return false;
			}

			if(errors != null && errors.Count > 0)
				return false;
			else
				return true;
		}
		public static bool userHasFRCPosHorizonRight(DateTime newDate, DateTime oldDate, Identity.Models.UserModel user, out List<string> errors)
		{
			errors = new List<string>();// - 2023-12-05
			if(user?.SuperAdministrator == true || user?.IsGlobalDirector == true)
			{
				return true;
			}
			var hnewDate = GetHorizonRange(newDate);
			var holdDate = GetHorizonRange(oldDate);
			var frcPosH1Right = user.Access.CustomerService.FRCPosHorizon1;
			var frcPosH2Right = user.Access.CustomerService.FRCPosHorizon2;
			var frcPosH3Right = user.Access.CustomerService.FRCPosHorizon3;

			if(newDate.dateIsInPast())
			{
				errors.Add("Forcast position date should not be in the past.");
				return false;
			}

			if(!frcPosH1Right && !frcPosH2Right && !frcPosH3Right)
			{
				errors.Add("You have no Forcast position update horizon assigned.");
				return false;
			}

			if(hnewDate != holdDate)
			{
				var canSwitch = checkIfUserCanSwitchHorizons(new HorizonsRightsCarrier
				{
					H1 = frcPosH1Right,
					H2 = frcPosH2Right,
					H3 = frcPosH3Right
				}, hnewDate, holdDate);
				if(!canSwitch)
					errors.Add($"you must have {holdDate.ToString()} and {hnewDate.ToString()} assigned in order to update this Forcast position end date.");
			}


			var check = checkIfUserCanPerfomAction(new HorizonsRightsCarrier
			{
				H1 = frcPosH1Right,
				H2 = frcPosH2Right,
				H3 = frcPosH3Right
			}, hnewDate);
			if(!check)
				errors.Add($"the end date [{newDate.ToString("dd/MM/yyyy")}] of the Forcast you are trying to edit is out of your assigned horizon(s)");

			if(errors != null && errors.Count > 0)
				return false;
			else
				return true;
		}
		public static bool userHasDLFPosHorizonRight(DateTime date, Identity.Models.UserModel user, out List<string> errors)
		{
			errors = new List<string>();// - 2023-12-05
			if(user?.SuperAdministrator == true || user?.IsGlobalDirector == true)
			{
				return true;
			}
			var hDate = GetHorizonRange(date);
			var dlfPosH1Right = user.Access.CustomerService.DLFPosHorizon1;
			var dlfPosH2Right = user.Access.CustomerService.DLFPosHorizon2;
			var dlfPosH3Right = user.Access.CustomerService.DLFPosHorizon3;

			if(date.dateIsInPast())
			{
				errors.Add("Delfor position date should not be in the past.");
				return false;
			}

			if(!dlfPosH1Right && !dlfPosH2Right && !dlfPosH3Right)
			{
				errors.Add("You have no Delfor Position Edit horizon assigned.");
				return false;
			}

			var check = checkIfUserCanPerfomAction(new HorizonsRightsCarrier
			{
				H1 = dlfPosH1Right,
				H2 = dlfPosH2Right,
				H3 = dlfPosH3Right
			}, hDate);
			if(!check)
			{
				errors.Add($"the delivery date [{date.ToString("dd/MM/yyyy")}] of the Delfor position you'r trying to edit is out of your assigned horizon(s)");
				return false;
			}

			if(errors != null && errors.Count > 0)
				return false;
			else
				return true;
		}
		//
		public static bool checkIfUserCanSwitchHorizons(HorizonsRightsCarrier rights, Enums.HorizonsEnums.horizons hnewDate, Enums.HorizonsEnums.horizons holdDate)
		{
			var check = false;
			//h1-h2 or h2-h1
			if((hnewDate == Enums.HorizonsEnums.horizons.horizon1 && holdDate == Enums.HorizonsEnums.horizons.horizon2)
				|| (hnewDate == Enums.HorizonsEnums.horizons.horizon2 && holdDate == Enums.HorizonsEnums.horizons.horizon1))
				if(rights.H1 && rights.H2)
					check = true;
			//h1-h3 or h3-h1
			if((hnewDate == Enums.HorizonsEnums.horizons.horizon1 && holdDate == Enums.HorizonsEnums.horizons.horizon3)
				|| (hnewDate == Enums.HorizonsEnums.horizons.horizon3 && holdDate == Enums.HorizonsEnums.horizons.horizon1))
				if(rights.H1 && rights.H3)
					check = true;
			//h2-h3 or h3-h2
			if((hnewDate == Enums.HorizonsEnums.horizons.horizon2 && holdDate == Enums.HorizonsEnums.horizons.horizon3)
				|| (hnewDate == Enums.HorizonsEnums.horizons.horizon3 && holdDate == Enums.HorizonsEnums.horizons.horizon2))
				if(rights.H2 && rights.H3)
					check = true;
			return check;
		}
		public static bool checkIfUserCanPerfomAction(HorizonsRightsCarrier rights, Enums.HorizonsEnums.horizons hDate)
		{
			bool check = false;

			if(hDate == Enums.HorizonsEnums.horizons.horizon1)
				if(rights.H1)
					check = true;
			if(hDate == Enums.HorizonsEnums.horizons.horizon2)
				if(rights.H2)
					check = true;
			if(hDate == Enums.HorizonsEnums.horizons.horizon3)
				if(rights.H3)
					check = true;

			return check;
		}
		public static bool ArticleIsTechnic(int articleNr)
		{
			var TechnicArticlesNrs = Infrastructure.Data.Access.Joins.CTS.Divers.GetTechnicArticlesNrs();
			return TechnicArticlesNrs.Contains(articleNr);
		}
		public class HorizonsRightsCarrier
		{
			public bool H1 { get; set; }
			public bool H2 { get; set; }
			public bool H3 { get; set; }
		}
	}
}