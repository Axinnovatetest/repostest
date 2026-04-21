using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FAPlanning;
using Psz.Core.Identity.Models;
using System.Globalization;
using System.Diagnostics;


namespace Psz.Core.CRP.Handlers.UBGPlannung
{
	public partial class CrpUBGPlannung
	{
		public ResponseModel<FASystemModel> ValidateGetFaSystem(UserModel user)
		{
			if(user == null)
				return ResponseModel<FASystemModel>.AccessDeniedResponse();
			return ResponseModel<FASystemModel>.SuccessResponse();
		}
		public ResponseModel<FASystemModel> GetFaSystem(UserModel user, FASystemRequestModel data)
		{
			try
			{
				ResponseModel<FASystemModel>? validationResponse = ValidateGetFaSystem(user);
				if(!validationResponse.Success)
					return validationResponse;

				var range = new List<(int Week, int Year)>();
				var horizons = Module.CTS.FAHorizons;
				var H1StartWeek = ISOWeek.GetWeekOfYear(DateTime.Now); // - 2024-03-28 - Heidenreich - Residue FAs
				var H1EndWeek = ISOWeek.GetWeekOfYear(DateTime.Today.AddDays(horizons.H1LengthInDays));
				var FoorMonthsEndWeek = H1StartWeek + 16;
				var startDate = Helpers.DatesHelper.FirstDateOfWeek(DateTime.Today.Year, ISOWeek.GetWeekOfYear(DateTime.Today));
				var endDate = data.Period == (int)Enums.CRPEnums.FASystemPeriod.H1
					? DateTime.Today.AddDays(horizons.H1LengthInDays)
					: Helpers.DatesHelper.FirstDateOfWeek(DateTime.Today.Year, H1StartWeek).AddDays(16 * 7 + 6); // - + 6 as we went from the week start

				for(DateTime date = startDate; date <= endDate; date = date.AddDays(1))
				{
					int weekNumber = ISOWeek.GetWeekOfYear(date);
					int year = date.Month == 12 && date.Year == DateTime.Now.Year && weekNumber == 1
						? date.Year + 1
						: date.Year;
					if(!range.Contains((weekNumber, year)))
						range.Add((weekNumber, year));
				}
				range.Insert(0, (H1StartWeek - 1, DateTime.Now.Year));

				var response = new FASystemModel
				{
					ABMovement = new List<WeekQuantityModel> { },
					FAMovement = new List<WeekQuantityModel> { },
					LPMovement = new List<WeekQuantityModel> { },
					FCMovement = new List<WeekQuantityModel> { },
					InternBedarfMovement = new List<WeekQuantityModel> { },
					ExternBedarfMovement = new List<WeekQuantityModel> { },
					WeekFrozenZone = H1EndWeek
				};
				// -
				var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(data.ArticleNr);
				var lagerEntity = Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.GetTotalAndSecurityStockByArticleNrs(data.ArticleNr, data.Unit.HasValue ? new List<int> { data.Unit.Value } : null);
				var faEntities = Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.GetFAQuantitiesByArticle_delayed(data.ArticleNr, startDate, endDate);
				var abEntities = Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.GetABQuantitiesByArticle_delayed(data.ArticleNr, startDate, endDate);
				var lpEntities = Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.GetLPQuantitiesByArticle_delayed(data.ArticleNr, startDate, endDate);
				var fcEntities = Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.GetFCQuantitiesByArticle_delayed(data.ArticleNr, startDate, endDate);
				var internBedarfEntities = Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.GetInternalOrExternalNeeds_delayed(data.ArticleNr, startDate, endDate, true);
				var externBedarfEntities = Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.GetInternalOrExternalNeeds_delayed(data.ArticleNr, startDate, endDate);


				// -
				foreach(var i in range)
				{
					var faValue = faEntities?.Where(x => x.Key == i.Week);
					var abValue = abEntities?.Where(x => x.Key == i.Week);
					var lpValue = lpEntities?.Where(x => x.Key == i.Week);
					var fcValue = fcEntities?.Where(x => x.Key == i.Week);
					var internBedarfValue = internBedarfEntities?.Where(x => x.Key == i.Week);
					var extenBedarfValue = externBedarfEntities?.Where(x => x.Key == i.Week);

					response.FAMovement.Add(new WeekQuantityModel { Week = i.Week, Year = i.Year, Quantity = faValue?.Sum(f => f.Value) ?? 0m });
					response.ABMovement.Add(new WeekQuantityModel { Week = i.Week, Year = i.Year, Quantity = abValue?.Sum(f => f.Value) ?? 0m });
					response.LPMovement.Add(new WeekQuantityModel { Week = i.Week, Year = i.Year, Quantity = lpValue?.Sum(f => f.Value) ?? 0m });
					response.FCMovement.Add(new WeekQuantityModel { Week = i.Week, Year = i.Year, Quantity = fcValue?.Sum(f => f.Value) ?? 0m });
					response.InternBedarfMovement.Add(new WeekQuantityModel { Week = i.Week, Year = i.Year, Quantity = internBedarfValue?.Sum(f => f.Value) ?? 0m });
					response.ExternBedarfMovement.Add(new WeekQuantityModel { Week = i.Week, Year = i.Year, Quantity = extenBedarfValue?.Sum(f => f.Value) ?? 0m });
				}

				#region /// -- Residue -- /// 
				var rfaValue = Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.GetFAQuantitiesByArticle_Residue_delayed(data.ArticleNr);
				var rfaiValue = Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.GetFAQuantitiesByArticle_ResidueUBG_delayed(data.ArticleNr, true);
				var rfaeValue = Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.GetFAQuantitiesByArticle_ResidueUBG_delayed(data.ArticleNr);
				var rabValue = Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.GetABQuantitiesByArticle_Residue_delayed(data.ArticleNr);
				var rlpValue = Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.GetLPQuantitiesByArticle_Residue_delayed(data.ArticleNr);
				var rfcValue = Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.GetFCQuantitiesByArticle_Residue_delayed(data.ArticleNr);

				response.FAMovement.Where(x => x.Week == H1StartWeek - 1).ToList().ForEach(x => x.Quantity = rfaValue);
				response.ABMovement.Where(x => x.Week == H1StartWeek - 1).ToList().ForEach(x => x.Quantity = rabValue);
				response.LPMovement.Where(x => x.Week == H1StartWeek - 1).ToList().ForEach(x => x.Quantity = rlpValue);
				response.FCMovement.Where(x => x.Week == H1StartWeek - 1).ToList().ForEach(x => x.Quantity = rfcValue);
				response.InternBedarfMovement.Where(x => x.Week == H1StartWeek - 1).ToList().ForEach(x => x.Quantity = rfaiValue);
				response.ExternBedarfMovement.Where(x => x.Week == H1StartWeek - 1).ToList().ForEach(x => x.Quantity = rfaeValue);
				#endregion Residue

				var bestand = Helpers.CRPHelper.GetBestand(response, range, lagerEntity.Item2 - lagerEntity.Item3);

				response.Bestand = bestand;

				var _response = new FASystemModel
				{
					ABMovement = response.ABMovement.ConvertAll(x => (WeekQuantityModel)x.Clone()),
					FAMovement = response.FAMovement.ConvertAll(x => (WeekQuantityModel)x.Clone()),
					LPMovement = response.LPMovement.ConvertAll(x => (WeekQuantityModel)x.Clone()),
					FCMovement = response.FCMovement.ConvertAll(x => (WeekQuantityModel)x.Clone()),
					InternBedarfMovement = response.InternBedarfMovement.ConvertAll(x => (WeekQuantityModel)x.Clone()),
					ExternBedarfMovement = response.ExternBedarfMovement.ConvertAll(x => (WeekQuantityModel)x.Clone()),
					Bestand = response.Bestand.ConvertAll(x => (WeekQuantityModel)x.Clone()),
				};
				var corrections = data.Period == (int)Enums.CRPEnums.FASystemPeriod.H1
					? new KeyValuePair<List<WeekQuantityModel>, List<WeekQuantityModel>>()
					: Helpers.CRPHelper.GetFACorrections(_response, range, lagerEntity.Item2 - lagerEntity.Item3, article.ProductionLotSize ?? 0);

				response.Corrections = corrections.Key;
				response.ProposedFA = corrections.Value;
				response.CurrentWeek = H1StartWeek;
				response.CurrentStock = lagerEntity.Item2 - lagerEntity.Item3;

				return ResponseModel<FASystemModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}