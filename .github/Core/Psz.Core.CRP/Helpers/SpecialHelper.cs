using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Helpers
{
	public class SpecialHelper
	{
		public static int Perform(Infrastructure.Data.Entities.Tables.PRS.FertigungEntity data)
		{
			try
			{
				var countriesEntities = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get();
				var hallEntities = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get();
				var departmentEntities = Infrastructure.Data.Access.Tables.WPL.DepartmentAccess.Get();
				var standardOperationEntities = Infrastructure.Data.Access.Tables.WPL.StandardOperationAccess.Get();
				var workAreaEntities = Infrastructure.Data.Access.Tables.WPL.WorkAreaAccess.Get();
				var workStationMachineEntities = Infrastructure.Data.Access.Tables.WPL.WorkStationMachineAccess.Get();
				var artikelOriginal = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get((int)data.Artikel_Nr);
				var EDIArticle = Infrastructure.Data.Access.Tables.WPL.ArticleAccess.GetByName(artikelOriginal?.ArtikelNummer);
				if(EDIArticle == null)
				{
					return -1;
				}
				KeyValuePair<int, int> artikel = new KeyValuePair<int, int>((int)data.Artikel_Nr, EDIArticle.Id);
				var countryId = 0;
				var hallId = 0;

				getCountryHall(data, ref countryId, ref hallId, countriesEntities, hallEntities);
				var countryEntity = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(countryId);
				if(countryEntity == null || countryEntity.IsArchived)
				{
					return -1;
				}

				var articlesWOwpl = new List<string>();
				var articlesWOActivewpl = new List<string>();
				var faConfig = Infrastructure.Data.Access.Tables.MTM.ConfigurationHeaderAccess.GetByCountryHall(countryId, hallId);
				if(faConfig != null)
				{
					// - WPLs
					var wpls = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.GetByArticleId(artikel.Key/*article.Id*/);
					if(wpls != null && wpls.Count > 0)
					{
						var wpl = wpls.Find(x => x.IsActive);
						if(wpl != null)
						{
							var wplDetails = Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.GetByWorkScheduleId(wpl.Id);
							if(wplDetails != null && wplDetails.Count > 0)
							{
								// - Config application
								var faConfigDetails = Infrastructure.Data.Access.Tables.MTM.ConfigurationDetailsAccess.GetByHeaders(new List<int> { faConfig.Id });

								var weeks1 = new List<Infrastructure.Data.Entities.Tables.MTM.ConfigurationDetailsEntity>();
								var weeks2 = new List<Infrastructure.Data.Entities.Tables.MTM.ConfigurationDetailsEntity>();
								var weeks3 = new List<Infrastructure.Data.Entities.Tables.MTM.ConfigurationDetailsEntity>();

								var capacityVersion = Infrastructure.Data.Access.Tables.MTM.CapacityRequiredAccess.GetVersionByFa(data.ID) + 1;

								var faTotoalTime = 0m;
								wplDetails.ForEach(x => faTotoalTime += getTotalTimeOperation(x, (decimal)data.Anzahl));
								faTotoalTime = faTotoalTime * (decimal)data.Anzahl / 60;
								// -- data.Anzahl * ((decimal?)data.Zeit ?? 0m) / 60
								if(faTotoalTime <= faConfig.ProductionOrderThreshold)
								{
									weeks1 = faConfigDetails.FindAll(x => x.DepartmentWeekNumber == 1 && x.IsLowerThanThreshold == true);
									weeks2 = faConfigDetails.FindAll(x => x.DepartmentWeekNumber == 2 && x.IsLowerThanThreshold == true);
								}
								else
								{
									weeks1 = faConfigDetails.FindAll(x => x.DepartmentWeekNumber == 1 && x.IsLowerThanThreshold == false);
									weeks2 = faConfigDetails.FindAll(x => x.DepartmentWeekNumber == 2 && x.IsLowerThanThreshold == false);
									weeks3 = faConfigDetails.FindAll(x => x.DepartmentWeekNumber == 3 && x.IsLowerThanThreshold == false);
								}


								// - W1
								var week1Operations = wplDetails.FindAll(x => weeks1.FindIndex(y => y.DepartmentId == x.DepartementId) >= 0);
								if(week1Operations != null && week1Operations.Count > 0)
								{
									Infrastructure.Data.Access.Tables.MTM.CapacityRequiredAccess.Insert(
										week1Operations.Select(x => getCapacity(capacityVersion, data.ID, data.Termin_Bestatigt1.Value, FetIso8601WeekOfYear(data.Termin_Bestatigt1.Value), (decimal)data.Anzahl, x,
										countriesEntities, hallEntities, departmentEntities, standardOperationEntities, workAreaEntities, workStationMachineEntities))
										?.ToList());
								}

								// - W2
								var week2Operations = wplDetails.FindAll(x => weeks2.FindIndex(y => y.DepartmentId == x.DepartementId) >= 0);
								if(week2Operations != null && week2Operations.Count > 0)
								{
									Infrastructure.Data.Access.Tables.MTM.CapacityRequiredAccess.Insert(
										week2Operations.Select(x => getCapacity(capacityVersion, data.ID, data.Termin_Bestatigt1.Value, FetIso8601WeekOfYear(data.Termin_Bestatigt1.Value) - 1, (decimal)data.Anzahl, x,
										countriesEntities, hallEntities, departmentEntities, standardOperationEntities, workAreaEntities, workStationMachineEntities))
										?.ToList());
								}

								// - W3
								if(weeks3 != null && weeks3.Count > 0)
								{
									var week3Operations = wplDetails.FindAll(x => weeks3.FindIndex(y => y.DepartmentId == x.DepartementId) >= 0);
									if(week3Operations != null && week3Operations.Count > 0)
									{
										Infrastructure.Data.Access.Tables.MTM.CapacityRequiredAccess.Insert(
											week3Operations.Select(x => getCapacity(capacityVersion, data.ID, data.Termin_Bestatigt1.Value, FetIso8601WeekOfYear(data.Termin_Bestatigt1.Value) - 2, (decimal)data.Anzahl, x,
											countriesEntities, hallEntities, departmentEntities, standardOperationEntities, workAreaEntities, workStationMachineEntities))
											?.ToList());
									}

								}
							}
							else
							{
								var art = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(artikel.Key);
								articlesWOActivewpl.Add(art.ArtikelNummer.Trim());
							}
						}
						else
						{
							var art = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(artikel.Key);
							articlesWOActivewpl.Add($"{artikel.Key}|{art?.ArtikelNummer.Trim()}");
						}
					}
					else
					{
						var art = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(artikel.Key);
						articlesWOwpl.Add(art?.ArtikelNummer.Trim());
					}
				}

				return 0;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return 0;
			}

		}
		private static void getCountryHall(Infrastructure.Data.Entities.Tables.PRS.FertigungEntity data, ref int countryId, ref int hallId,
			List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity> countriesEntities,
			List<Infrastructure.Data.Entities.Tables.WPL.HallEntity> hallEntities)
		{
			var _countryId = -1;
			if(data.Lagerort_id == 6)
			{
				//countryId = 1038;
				//hallId = 30;
				var c = countriesEntities.Find(x => x.Name.Trim().ToLower().StartsWith("c"));
				_countryId = c?.Id ?? 1038;
				var h = hallEntities.Where(x => x.CountryId == _countryId)?.ToList().Find(x => x.Name.Trim().ToLower().Contains("1"));
				hallId = h?.Id ?? 30;
			}
			else if(data.Lagerort_id == 7)
			{
				var c = countriesEntities.Find(x => x.Name.Trim().ToLower().StartsWith("t"));
				_countryId = c?.Id ?? 1040;
				var h = hallEntities.Where(x => x.CountryId == _countryId)?.ToList().Find(x => x.Name.Trim().ToLower().Contains("1"));
				hallId = h?.Id ?? 34;
			}
			else if(data.Lagerort_id == 15)
			{
				var c = countriesEntities.Find(x => x.Name.Trim().ToLower().StartsWith("g"));
				_countryId = c?.Id ?? 1039;
				var h = hallEntities.Where(x => x.CountryId == _countryId)?.ToList().Find(x => x.Name.Trim().ToLower().Contains("1"));
				hallId = h?.Id ?? 32;
			}
			else if(data.Lagerort_id == 26)
			{
				//countryId = 1037;
				//hallId = 29;
				var c = countriesEntities.Find(x => x.Name.Trim().ToLower().StartsWith("a"));
				_countryId = c?.Id ?? 1037;
				var h = hallEntities.Where(x => x.CountryId == _countryId)?.ToList().Find(x => x.Name.Trim().ToLower().Contains("1"));
				hallId = h?.Id ?? 29;
			}
			else if(data.Lagerort_id == 42)
			{
				//countryId = 1040;
				//hallId = 33;
				var c = countriesEntities.Find(x => x.Name.Trim().ToLower().StartsWith("t"));
				_countryId = c?.Id ?? 1040;
				var h = hallEntities.Where(x => x.CountryId == _countryId)?.ToList().Find(x => x.Name.Trim().ToLower().Contains("3"));
				hallId = h?.Id ?? 33;
			}
			else if(data.Lagerort_id == 60)
			{
				//countryId = 1040;
				//hallId = 36; // BE
				var c = countriesEntities.Find(x => x.Name.Trim().ToLower().StartsWith("t"));
				_countryId = c?.Id ?? 1040;
				var h = hallEntities.Where(x => x.CountryId == _countryId)?.ToList().Find(x => x.Name.Trim().ToLower().Contains("2"));
				hallId = h?.Id ?? 36;
			}

			countryId = _countryId;
		}
		static Infrastructure.Data.Entities.Tables.MTM.CapacityRequiredEntity getCapacity(int version, int faId, DateTime prodDate, int week, decimal quantity,
		   Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity detailsEntity,
			List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity> countriesEntities,
			List<Infrastructure.Data.Entities.Tables.WPL.HallEntity> hallEntities,
			List<Infrastructure.Data.Entities.Tables.WPL.DepartmentEntity> departmentEntities,
			List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationEntity> standardOperationEntities,
			List<Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity> workAreaEntities,
			List<Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity> workStationMachineEntities)
		{
			//var countryEntity = countriesEntities?.Find(x=> x.Id == detailsEntity.CountryId);
			//var hallEntity = hallEntities?.Find(x=> x.Id == detailsEntity.HallId);
			//var departmentEntity = departmentEntities?.Find(x=> x.Id == detailsEntity.DepartementId);
			//var operationEntity = standardOperationEntities?.Find(x=> x.Id == detailsEntity.StandardOperationId);
			//var wAreaEntity = workAreaEntities?.Find(x=> x.Id == detailsEntity.WorkAreaId);
			//var wStationEntity = workStationMachineEntities?.Find(x=> x.Id == detailsEntity.WorkStationMachineId);

			return new Infrastructure.Data.Entities.Tables.MTM.CapacityRequiredEntity
			{
				ArchiveTime = null,
				ArchiveUserId = null,
				Attendance = quantity * getTotalTimeOperation(detailsEntity, quantity) / 60,
				AvailableHrDaily = 0,
				AvailableSrDaily = 0,
				CountryId = detailsEntity.CountryId,
				CountryName = "", //countryEntity?.Name,
				CreationTime = DateTime.Now,
				CreationUserId = -1,
				DepartementId = detailsEntity.DepartementId,
				DepartementName = "", // departmentEntity?.Name,
				Factor1HrDaily = 0,
				Factor2HrDaily = 0,
				Factor3SrDaily = 0,
				FormToolInsert = detailsEntity.FromToolInsert,
				HallId = detailsEntity.HallId,
				HallName = "", //hallEntity?.Name,
				HrHardResourcesNumber = 0,
				Id = -1,
				IsArchived = false,
				LastUpdateTime = null,
				LastUpdateUserId = null,
				OperationId = detailsEntity.StandardOperationId,
				OperationName = "", //operationEntity?.Name,
				PlanCapacity = 0,
				ProductivityHrDaily = 0,
				ProductivitySrDaily = 0,
				RequiredEmployees = 0,
				ShiftsNumberWeekly = 0,
				SoftRessourcesNumberDaily = 0,
				SourceId = faId,
				SpecialHoursWeekly = 0,
				SpecialShiftsWeekly = 0,
				Version = version,
				WeekFirstDay = StartOfWeek(prodDate),
				WeekLastDay = prodDate,
				WeekNumber = week,
				WorkAreaId = detailsEntity.WorkAreaId,
				WorkAreaName = "", // wAreaEntity?.Name,
				WorkStationId = detailsEntity.WorkStationMachineId,
				WorkStationName = "", // wStationEntity?.Name,
				Year = prodDate.Year,
			};
		}

		static decimal getTotalTimeOperation(Infrastructure.Data.Entities.Tables.WPL.WorkScheduleDetailsEntity workSchedule, decimal faQuantity)
		{
			// Formula taken from excel doc
			try
			{
				workSchedule.TotalTimeOperation = 0;
				// Lot
				if(workSchedule.RelationOperationTime == 0)
				{
					workSchedule.TotalTimeOperation =
						workSchedule.Amount * workSchedule.OperationTimeSeconds / 60 / (faQuantity == 0 ? 1 : faQuantity)
						+ workSchedule.SetupTimeMinutes / (faQuantity == 0 ? 1 : faQuantity);
				}
				else
				{
					// Piece
					if(workSchedule.RelationOperationTime == 1)
					{
						workSchedule.TotalTimeOperation =
							workSchedule.Amount * workSchedule.OperationTimeSeconds / 60
							+ workSchedule.SetupTimeMinutes / (faQuantity == 0 ? 1 : faQuantity);
					}
				}

				return Math.Truncate(workSchedule.TotalTimeOperation * 100000) / 100000;
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.Message + "\n" + exception.StackTrace);
				return 0m;
			}
		}
		public static DateTime StartOfWeek(DateTime dateTime, DayOfWeek startOfWeek = DayOfWeek.Monday)
		{
			int diff = (7 + (dateTime.DayOfWeek - startOfWeek)) % 7;
			return dateTime.AddDays(-1 * diff).Date;
		}
		public static int FetIso8601WeekOfYear(DateTime time)
		{
			// Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
			// be the same week# as whatever Thursday, Friday or Saturday are,
			// and we always get those right
			//var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
			//if(day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
			//{
			//	time = time.AddDays(3);
			//}

			//// Return the week of our adjusted day
			//return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
			return ISOWeek.GetWeekOfYear(time);
		}
		public static bool CanArchiveOrderByAngebote(int? id)
		{
			if(!id.HasValue)
				return true;

			var fas = id.Value == 0 ? null : Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByAngeboteNr(id.Value);
			return fas == null || fas.Count <= 0;
		}
		public static bool CanDeleteOrder(int id, int angeboteNr)
		{
			var fas = angeboteNr == 0 ? null : Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByAngeboteNr(id);
			var canDelete1 = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.CanDelete(id);
			var canDelete2 = (fas == null || fas.Count <= 0);
			return canDelete1 && canDelete2;
		}
		public static int UpdateFACapacity(Infrastructure.Data.Entities.Tables.PRS.FertigungEntity data, int userID)
		{
			var version = Infrastructure.Data.Access.Tables.MTM.CapacityRequiredAccess.GetVersionByFa(data.ID);
			Infrastructure.Data.Access.Tables.MTM.CapacityRequiredAccess.ArchiveByFa(userID, data.ID, version);
			return Perform(data);
		}
	}
}
