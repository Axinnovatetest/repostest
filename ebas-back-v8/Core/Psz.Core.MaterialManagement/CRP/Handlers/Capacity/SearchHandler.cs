using Infrastructure.Data.Access.Tables.MTM;
using Infrastructure.Data.Access.Tables.WPL;
using Infrastructure.Data.Entities.Tables.MTM;
using Psz.Core.MaterialManagement.CRP.Models.Capacity;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Capacity
{
	public class SearchHandler: IHandle<SearchRequestModel, ResponseModel<List<CapacityModel>>>
	{
		private SearchRequestModel data { get; set; }
		private Psz.Core.Identity.Models.UserModel user { get; set; }

		public SearchHandler(SearchRequestModel data,
			Identity.Models.UserModel user)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<List<CapacityModel>> Handle()
		{
			//lock(Locks.CapacityLock)
			{
				try
				{
					if(user == null)
					{
						throw new Psz.Core.SharedKernel.Exceptions.UnauthorizedException();
					}

					// -
					getFA();
					return Perform(this.user, this.data);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public static ResponseModel<List<CapacityModel>> Perform(Identity.Models.UserModel user,
			SearchRequestModel data)
		{
			#region > Validation
			var countryEntity = CountryAccess.Get(data.CountryId);
			if(countryEntity == null || countryEntity.IsArchived)
			{
				return ResponseModel<List<CapacityModel>>.FailureResponse("Country is not found");
			}
			#endregion

			// - show only CurrentWeek + 4
			if(data.Focus)
			{
				data.WeekNumberUntil = Helpers.Config.GetCapacityLastEditableWeek();
			}

			// - week from to
			var capacityEntities = CapacityAccess.Get(data.CountryId,
				data.Year,
				data.WeekNumberFrom,
				data.WeekNumberUntil,
				data.OperationId,
				data.HallId,
				data.DepartementId,
				data.WorkAreaId,
				data.WorkStationId);

			if(capacityEntities == null || capacityEntities.Count <= 0)
				return ResponseModel<List<CapacityModel>>.SuccessResponse(null);

			// -
			capacityEntities = Helpers.Capacity.AddHolidays(capacityEntities,
					data.CountryId, data.HallId, data.Year,
					data.WeekNumberFrom, data.WeekNumberUntil);

			var response = new List<CapacityModel>();

			foreach(var capacityEntity in capacityEntities)
			{
				var newCapacity = new CapacityModel(capacityEntity);
				newCapacity.CanEdit = Helpers.Config.CanEdit(newCapacity.Year, newCapacity.WeekNumber, Enums.Main.CapacityType.Capacity);
				newCapacity.CanDelete = Helpers.Config.CanEdit(newCapacity.Year, newCapacity.WeekNumber, Enums.Main.CapacityType.Capacity);
				response.Add(newCapacity);
			}

			// - Sort for display purposes
			response = response.OrderBy(x => x.WeekNumber)
				.ThenBy(x => x.OperationId)
				.ThenBy(x => x.HallId)
				.ThenBy(x => x.DepartementId)
				.ThenBy(x => x.WorkAreaId)
				.ThenBy(x => x.WorkStationId)
				.ToList();

			// -
			return ResponseModel<List<CapacityModel>>.SuccessResponse(response);
		}

		public ResponseModel<List<CapacityModel>> Validate()
		{
			throw new NotImplementedException();
		}
		void getFA()
		{
			// - FIXME: Cross-DB join!!!
			var newFa = Infrastructure.Data.Access.Tables.MTM.CapacityRequiredAccess.GetNew();

			if(newFa != null && newFa.Count > 0)
			{
				var articles = Infrastructure.Data.Access.Tables.MTM.CapacityRequiredAccess.GetIdsByIds(newFa?.Select(x => x.Artikel_Nr ?? -1).ToList());

				//-
				var countries = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get();
				var halls = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get();
				var departments = Infrastructure.Data.Access.Tables.WPL.DepartmentAccess.Get();
				var operations = Infrastructure.Data.Access.Tables.WPL.StandardOperationAccess.Get();
				var wAreas = Infrastructure.Data.Access.Tables.WPL.WorkAreaAccess.Get();
				var wStations = Infrastructure.Data.Access.Tables.WPL.WorkStationMachineAccess.Get();

				foreach(var fa in newFa)
				{
					try
					{
						if(fa.Termin_Fertigstellung.HasValue)
						{
							// Console.WriteLine($"Processing FA {fa.Fertigungsnummer}, KW {Helpers.DateTimeHelper.GetIso8601WeekOfYear(fa.Termin_Bestatigt1.HasValue ? fa.Termin_Bestatigt1.Value : DateTime.Now)}");
							var article = articles.Find(x => x == fa.Artikel_Nr);
							PerformFa(fa, article, countries, halls, departments, operations, wAreas, wStations);
						}
					} catch(Exception ex)
					{
						Infrastructure.Services.Logging.Logger.Log(ex);
						//System.IO.File.AppendAllText($"errors-{time}.log", $"{ex.Message}\n\n{ex.StackTrace}");
						//System.IO.File.AppendAllText($"errors-{time}.log", $"{Newtonsoft.Json.JsonConvert.SerializeObject(fa)}");
					}
				}
			}
		}
		public static int PerformFa(
			Infrastructure.Data.Entities.Tables.PRS.FertigungEntity data, int artikel,
			List<Infrastructure.Data.Entities.Tables.WPL.CountryEntity> countriesEntities,
			List<Infrastructure.Data.Entities.Tables.WPL.HallEntity> hallEntities,
			List<Infrastructure.Data.Entities.Tables.WPL.DepartmentEntity> departmentEntities,
			List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationEntity> standardOperationEntities,
			List<Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity> workAreaEntities,
			List<Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity> workStationMachineEntities)
		{
			#region > Validation
			var countryId = 0;
			var hallId = 0;
			/*
             Lagerort_id	Lagerort
                6	Eigenfertigung
                7	TN
                15	Fertigung D
                26	Albanien
                42	WS
                60	BE_TN
             */

			/*
            Id	Name
            1037	Albania
            1038	Czech Republic
            1039	Germany
            1040	Tunisia
             */
			/*
            Id	Name	Area	Country_Id
            29	Unit1	Tirana	1037
            30	Hala 1	Vysocany	1038
            31	Hala 2	Vysocany	1038
            32	Unit1	Vohenstrauss	1039
            33	Unit3	Bouhjar WS	1040
            34	Unit1	Ksar helal-ZI Ali Soua TN	1040
            35	Unit3	Ksar helal-Monoprix	1040
            36	Unit2	Bennen BE TN	1040
            37	Unit2	Vohenstrauss	1039 
            */


			getCountryHall(data, ref countryId, ref hallId, countriesEntities, hallEntities);
			var countryEntity = CountryAccess.Get(countryId);
			if(countryEntity == null || countryEntity.IsArchived)
			{
				return -1;
			}
			#endregion

			var articlesWOwpl = new List<string>();
			var articlesWOActivewpl = new List<string>();
			var faConfig = Infrastructure.Data.Access.Tables.MTM.ConfigurationHeaderAccess.GetByCountryHall(countryId, hallId);
			if(faConfig != null)
			{
				// - WPLs
				var wpls = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.GetByArticleId(artikel);
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

							var weeks1 = new List<ConfigurationDetailsEntity>();
							var weeks2 = new List<ConfigurationDetailsEntity>();
							var weeks3 = new List<ConfigurationDetailsEntity>();

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
									week1Operations.Select(x => getCapacity(data.ID, data.Termin_Bestatigt1.Value, Helpers.DateTimeHelper.GetIso8601WeekOfYear(data.Termin_Bestatigt1.Value), (decimal)data.Anzahl, x,
									countriesEntities, hallEntities, departmentEntities, standardOperationEntities, workAreaEntities, workStationMachineEntities))
									?.ToList());
							}

							// - W2
							var week2Operations = wplDetails.FindAll(x => weeks2.FindIndex(y => y.DepartmentId == x.DepartementId) >= 0);
							if(week2Operations != null && week2Operations.Count > 0)
							{
								Infrastructure.Data.Access.Tables.MTM.CapacityRequiredAccess.Insert(
									week2Operations.Select(x => getCapacity(data.ID, data.Termin_Bestatigt1.Value, Helpers.DateTimeHelper.GetIso8601WeekOfYear(data.Termin_Bestatigt1.Value) - 1, (decimal)data.Anzahl, x,
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
										week3Operations.Select(x => getCapacity(data.ID, data.Termin_Bestatigt1.Value, Helpers.DateTimeHelper.GetIso8601WeekOfYear(data.Termin_Bestatigt1.Value) - 2, (decimal)data.Anzahl, x,
										countriesEntities, hallEntities, departmentEntities, standardOperationEntities, workAreaEntities, workStationMachineEntities))
										?.ToList());
								}

							}
						}
						else
						{
							var art = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(artikel);
							articlesWOActivewpl.Add(art.ArtikelNummer.Trim());
						}
					}
					else
					{
						var art = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(artikel);
						articlesWOActivewpl.Add($"{artikel}|{art?.ArtikelNummer.Trim()}");
					}
				}
				else
				{
					var art = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(artikel);
					articlesWOwpl.Add(art?.ArtikelNummer.Trim());
				}
			}

			//if (articlesWOwpl.Count > 0)
			//    System.IO.File.AppendAllLines($"article-wo-ws-{time}.log", articlesWOwpl.Distinct());

			//if (articlesWOActivewpl.Count > 0)
			//    System.IO.File.AppendAllLines($"article-wo-akws-{time}.log", articlesWOActivewpl.Distinct());

			return 0;
		}

		private static void getCountryHall(
			Infrastructure.Data.Entities.Tables.PRS.FertigungEntity data, ref int countryId, ref int hallId,
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

		static Infrastructure.Data.Entities.Tables.MTM.CapacityRequiredEntity getCapacity(int faId, DateTime prodDate, int week, decimal quantity,
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
				Version = 0,
				WeekFirstDay = Helpers.DateTimeHelper.StartOfWeek(prodDate),
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
						workSchedule.Amount * workSchedule.OperationTimeSeconds / 60 / faQuantity
						+ workSchedule.SetupTimeMinutes / faQuantity;
				}
				else
				{
					// Piece
					if(workSchedule.RelationOperationTime == 1)
					{
						workSchedule.TotalTimeOperation =
							workSchedule.Amount * workSchedule.OperationTimeSeconds / 60
							+ workSchedule.SetupTimeMinutes / faQuantity;
					}
				}

				return workSchedule.TotalTimeOperation;  //  Math.Truncate(workSchedule.TotalTimeOperation * 100000) / 100000;
			} catch(Exception exception)
			{
				//Infrastructure.Services.Logging.Logger.Log(exception.Message + "\n" + exception.StackTrace);
				throw;
			}
		}

	}
}
