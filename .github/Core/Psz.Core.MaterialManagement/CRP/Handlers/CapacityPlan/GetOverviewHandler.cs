using Infrastructure.Data.Access.Tables.MTM;
using Infrastructure.Data.Access.Tables.WPL;
using Psz.Core.MaterialManagement.CRP.Models.CapacityPlan;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.CapacityPlan
{
	public class GetOverviewHandler: IHandle<OverviewRequestModel, ResponseModel<OverviewModel>>
	{
		private OverviewRequestModel data { get; set; }
		private Psz.Core.Identity.Models.UserModel user { get; set; }

		public GetOverviewHandler(OverviewRequestModel data,
			Identity.Models.UserModel user)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<OverviewModel> Handle()
		{
			lock(Locks.HolidayLock)
			{
				try
				{
					if(user == null)
					{
						throw new Psz.Core.SharedKernel.Exceptions.UnauthorizedException();
					}

					#region > Validation
					if(this.data.Year < 1900 || 9999 < this.data.Year)
					{
						return ResponseModel<OverviewModel>.FailureResponse("Year: Invalid value");
					}

					var countryEntity = CountryAccess.Get(this.data.CurrentCountryId);
					if(countryEntity == null || countryEntity.IsArchived)
					{
						return ResponseModel<OverviewModel>.FailureResponse("Country is not found");
					}
					#endregion

					var operationEntities = StandardOperationAccess.Get().FindAll(e => !e.IsArchived);
					var hallEntities = HallAccess.Get().FindAll(e => !e.IsArchived);
					var departementEntities = DepartmentAccess.Get().FindAll(e => !e.IsArchived);
					var workAreaEntities = WorkAreaAccess.Get().FindAll(e => !e.IsArchived);
					var workStationEntities = WorkStationMachineAccess.Get().FindAll(e => !e.IsArchived);

					var responseData = new List<OverviewModel.WeeklyPlan>();

					var capacityPlanEntities = CapacityPlanAccess.GetBy_CountryId_HallId_Year(this.data.CurrentCountryId,
						this.data.CurrentHallId,
						this.data.Year);

					var weeksNumber = Helpers.DateTimeHelper.GetWeeksNumberInYear(this.data.Year);

					OverviewModel.WeeklyPlan currentWeekPlan = null;

					// I want my check dude, get the work done b4, it is done, for now! it is called so far, we'll see.
					for(int weekNumber = 1; weekNumber < (weeksNumber + 1); weekNumber++)
					{
						var weekPlanItems = new List<OverviewModel.Item>();

						#region > Get week-plan's data
						var list = capacityPlanEntities.FindAll(e => e.WeekNumber == weekNumber);
						foreach(var capacityPlanEntity in list)
						{
							var weekPlanItem = new OverviewModel.Item()
							{
								Type = OverviewModel.ItemTypes.New,

								OperationId = capacityPlanEntity.OperationId,
								OperationName = capacityPlanEntity.OperationName,
								HallId = capacityPlanEntity.HallId,
								HallName = capacityPlanEntity.HallName,
								DepartementId = capacityPlanEntity.DepartementId,
								DepartementName = capacityPlanEntity.DepartementName,
								WorkAreaId = capacityPlanEntity.WorkAreaId,
								WorkAreaName = capacityPlanEntity.WorkAreaName,
								WorkStationId = capacityPlanEntity.WorkStationId,
								WorkStationName = capacityPlanEntity.WorkStationName,
								FormToolInsert = capacityPlanEntity.FormToolInsert,

								HrHardResourcesNumber = capacityPlanEntity.HrHardResourcesNumber,
								HrF1PersonPerMachine = capacityPlanEntity.Factor1HrDaily,
								HrF2UtilisationRate = capacityPlanEntity.Factor2HrDaily,
								HrProductivity = capacityPlanEntity.ProductivityHrDaily,

								SrSoftResourcesNumber = capacityPlanEntity.SoftRessourcesNumberDaily,
								SrF3AttendanceLevel = capacityPlanEntity.Factor3SrDaily,
								SrProductivity = capacityPlanEntity.ProductivitySrDaily,

								AvailableHr = capacityPlanEntity.AvailableHrDaily,
								AvailableSr = capacityPlanEntity.AvailableSrDaily,

								ShiftsPerWeek = capacityPlanEntity.ShiftsNumberWeekly,
								SpecialShiftsPerWeek = capacityPlanEntity.SpecialShiftsWeekly,
								SpecialHours = capacityPlanEntity.SpecialHoursWeekly,
								WorkingHoursPerShift = capacityPlanEntity.WorkingHoursPerShift,

								AttendancePerWeek = Common.Helpers.MathHelper.RoundDecimal(capacityPlanEntity.Attendance, Helpers.Config.DecimalPart),
								PlannableCapacites = Common.Helpers.MathHelper.RoundDecimal(capacityPlanEntity.PlanCapacity, Helpers.Config.DecimalPart),
								RequiredEmployeesNumber = Common.Helpers.MathHelper.RoundDecimal(capacityPlanEntity.RequiredEmployees, Helpers.Config.DecimalPart),

								// -
								Id = capacityPlanEntity.Id
							};

							List<OverviewModel.Item> existantItem = null;

							#region > Compare
							if(currentWeekPlan != null)
							{
								existantItem = currentWeekPlan.Items.FindAll(e =>
									e.OperationId == weekPlanItem.OperationId
									&& e.HallId == weekPlanItem.HallId
									&& e.DepartementId == weekPlanItem.DepartementId
									&& e.WorkAreaId == weekPlanItem.WorkAreaId
									&& e.WorkStationId == weekPlanItem.WorkStationId
									// -
									//&& e.AttendancePerWeek == weekPlanItem.AttendancePerWeek
									//&& e.PlannableCapacites == weekPlanItem.PlannableCapacites
									//&& e.RequiredEmployeesNumber == weekPlanItem.RequiredEmployeesNumber
									);
								if(existantItem != null && existantItem.Count > 0)
								{
									weekPlanItem.Type = OverviewModel.ItemTypes.NotChanged;

									if(existantItem.Count > 1)
									{ }
									bool exists;
									foreach(var _existingItem in existantItem)
									{
										exists = true;
										weekPlanItem.HrHardResourcesNumber_Changed = false;
										weekPlanItem.HrF1PersonPerMachine_Changed = false;
										weekPlanItem.HrF2UtilisationRate_Changed = false;
										weekPlanItem.HrProductivity_Changed = false;
										weekPlanItem.SrSoftResourcesNumber_Changed = false;
										weekPlanItem.SrF3AttendanceLevel_Changed = false;
										weekPlanItem.SrProductivity_Changed = false;

										weekPlanItem.AvailableHr_Changed = false;
										weekPlanItem.AvailableSr_Changed = false;

										weekPlanItem.ShiftsPerWeek_Changed = false;
										weekPlanItem.SpecialShiftsPerWeek_Changed = false;
										weekPlanItem.SpecialHours_Changed = false;
										weekPlanItem.WorkingHoursPerShift_Changed = false;

										weekPlanItem.AttendancePerWeek_Changed = false;
										weekPlanItem.PlannableCapacites_Changed = false;
										weekPlanItem.RequiredEmployeesNumber_Changed = false;

										weekPlanItem.Type = OverviewModel.ItemTypes.NotChanged;

										#region > Compare each value -> set _changed & set type to Changed
										if(weekPlanItem.HrHardResourcesNumber != _existingItem.HrHardResourcesNumber)
										{
											weekPlanItem.HrHardResourcesNumber_Changed = true;
											weekPlanItem.Type = OverviewModel.ItemTypes.Changed;
											exists = false;
										}
										if(weekPlanItem.HrF1PersonPerMachine != _existingItem.HrF1PersonPerMachine)
										{
											weekPlanItem.HrF1PersonPerMachine_Changed = true;
											weekPlanItem.Type = OverviewModel.ItemTypes.Changed;
											exists = false;
										}
										if(weekPlanItem.HrF2UtilisationRate != _existingItem.HrF2UtilisationRate)
										{
											weekPlanItem.HrF2UtilisationRate_Changed = true;
											weekPlanItem.Type = OverviewModel.ItemTypes.Changed;
											exists = false;
										}
										if(weekPlanItem.HrProductivity != _existingItem.HrProductivity)
										{
											weekPlanItem.HrProductivity_Changed = true;
											weekPlanItem.Type = OverviewModel.ItemTypes.Changed;
											exists = false;
										}
										if(weekPlanItem.SrSoftResourcesNumber != _existingItem.SrSoftResourcesNumber)
										{
											weekPlanItem.SrSoftResourcesNumber_Changed = true;
											weekPlanItem.Type = OverviewModel.ItemTypes.Changed;
											exists = false;
										}
										if(weekPlanItem.SrF3AttendanceLevel != _existingItem.SrF3AttendanceLevel)
										{
											weekPlanItem.SrF3AttendanceLevel_Changed = true;
											weekPlanItem.Type = OverviewModel.ItemTypes.Changed;
											exists = false;
										}
										if(weekPlanItem.SrProductivity != _existingItem.SrProductivity)
										{
											weekPlanItem.SrProductivity_Changed = true;
											weekPlanItem.Type = OverviewModel.ItemTypes.Changed;
											exists = false;
										}

										if(weekPlanItem.AvailableHr != _existingItem.AvailableHr)
										{
											weekPlanItem.AvailableHr_Changed = true;
											weekPlanItem.Type = OverviewModel.ItemTypes.Changed;
											exists = false;
										}
										if(weekPlanItem.AvailableSr != _existingItem.AvailableSr)
										{
											weekPlanItem.AvailableSr_Changed = true;
											weekPlanItem.Type = OverviewModel.ItemTypes.Changed;
											exists = false;
										}

										if(weekPlanItem.ShiftsPerWeek != _existingItem.ShiftsPerWeek)
										{
											weekPlanItem.ShiftsPerWeek_Changed = true;
											weekPlanItem.Type = OverviewModel.ItemTypes.Changed;
											exists = false;
										}
										if(weekPlanItem.SpecialShiftsPerWeek != _existingItem.SpecialShiftsPerWeek)
										{
											weekPlanItem.SpecialShiftsPerWeek_Changed = true;
											weekPlanItem.Type = OverviewModel.ItemTypes.Changed;
											exists = false;
										}
										if(weekPlanItem.SpecialHours != _existingItem.SpecialHours)
										{
											weekPlanItem.SpecialHours_Changed = true;
											weekPlanItem.Type = OverviewModel.ItemTypes.Changed;
											exists = false;
										}
										if(weekPlanItem.WorkingHoursPerShift != _existingItem.WorkingHoursPerShift)
										{
											weekPlanItem.WorkingHoursPerShift_Changed = true;
											weekPlanItem.Type = OverviewModel.ItemTypes.Changed;
											exists = false;
										}

										if(weekPlanItem.AttendancePerWeek != Common.Helpers.MathHelper.RoundDecimal(_existingItem.AttendancePerWeek, Helpers.Config.DecimalPart))
										{
											weekPlanItem.AttendancePerWeek_Changed = true;
											weekPlanItem.Type = OverviewModel.ItemTypes.Changed;
											exists = false;
										}
										if(weekPlanItem.PlannableCapacites != Common.Helpers.MathHelper.RoundDecimal(_existingItem.PlannableCapacites, Helpers.Config.DecimalPart))
										{
											weekPlanItem.PlannableCapacites_Changed = true;
											weekPlanItem.Type = OverviewModel.ItemTypes.Changed;
											exists = false;
										}
										if(weekPlanItem.RequiredEmployeesNumber != Common.Helpers.MathHelper.RoundDecimal(_existingItem.RequiredEmployeesNumber, Helpers.Config.DecimalPart))
										{
											weekPlanItem.RequiredEmployeesNumber_Changed = true;
											weekPlanItem.Type = OverviewModel.ItemTypes.Changed;
											exists = false;
										}

										// - match found
										if(exists == true)
											break;

										#endregion
									}

								}
							}
							#endregion

							weekPlanItems.Add(weekPlanItem);
						}
						#endregion

						#region > Choose between keeping same weekly-plan, creating a new one (& insert deleted items)
						if(currentWeekPlan == null)
						{
							currentWeekPlan = new OverviewModel.WeeklyPlan()
							{
								FirstWeekNumber = weekNumber,
								LastWeekNumber = weekNumber,
								Items = new List<OverviewModel.Item>(),
							};
							currentWeekPlan.Items.AddRange(weekPlanItems);
							responseData.Add(currentWeekPlan);
						}
						else if(weekPlanItems.Exists(e => e.Type != OverviewModel.ItemTypes.NotChanged))
						{
							// -
							if(weekPlanItems.Count != currentWeekPlan.Items.FindAll(e => e.Type != OverviewModel.ItemTypes.Deleted).Count) // Some items are deleted!
							{
								foreach(var item in currentWeekPlan.Items.FindAll(e => e.Type != OverviewModel.ItemTypes.Deleted))
								{
									var deletedThisWeek = weekPlanItems.Find(e =>
										e.OperationId == item.OperationId
										&& e.HallId == item.HallId
										&& e.DepartementId == item.DepartementId
										&& e.WorkAreaId == item.WorkAreaId
										&& e.WorkStationId == item.WorkStationId);
									if(deletedThisWeek == null)
									{
										weekPlanItems.Add(new OverviewModel.Item()
										{
											Type = OverviewModel.ItemTypes.Deleted,

											OperationId = item.OperationId,
											OperationName = item.OperationName,
											HallId = item.HallId,
											HallName = item.HallName,
											DepartementId = item.DepartementId,
											DepartementName = item.DepartementName,
											WorkAreaId = item.WorkAreaId,
											WorkAreaName = item.WorkAreaName,
											WorkStationId = item.WorkStationId,
											WorkStationName = item.WorkStationName,
											FormToolInsert = item.FormToolInsert,

											HrHardResourcesNumber = item.HrHardResourcesNumber,
											HrF1PersonPerMachine = item.HrF1PersonPerMachine,
											HrF2UtilisationRate = item.HrF2UtilisationRate,
											HrProductivity = item.HrProductivity,

											SrSoftResourcesNumber = item.SrSoftResourcesNumber,
											SrF3AttendanceLevel = item.SrF3AttendanceLevel,
											SrProductivity = item.SrProductivity,

											AvailableHr = item.AvailableHr,
											AvailableSr = item.AvailableSr,

											ShiftsPerWeek = item.ShiftsPerWeek,
											SpecialShiftsPerWeek = item.SpecialShiftsPerWeek,
											SpecialHours = item.SpecialHours,
											WorkingHoursPerShift = item.WorkingHoursPerShift,

											AttendancePerWeek = Common.Helpers.MathHelper.RoundDecimal(item.AttendancePerWeek, Helpers.Config.DecimalPart),
											PlannableCapacites = Common.Helpers.MathHelper.RoundDecimal(item.PlannableCapacites, Helpers.Config.DecimalPart),
											RequiredEmployeesNumber = Common.Helpers.MathHelper.RoundDecimal(item.RequiredEmployeesNumber, Helpers.Config.DecimalPart),

											// -
											Id = item.Id
										});
									}
								}
							}
							// -
							currentWeekPlan = new OverviewModel.WeeklyPlan()
							{
								FirstWeekNumber = weekNumber,
								LastWeekNumber = weekNumber,
								Items = new List<OverviewModel.Item>(),
							};
							currentWeekPlan.Items.AddRange(weekPlanItems);
							responseData.Add(currentWeekPlan);
						}
						else
						{
							if(weekPlanItems.Count != currentWeekPlan.Items.FindAll(e => e.Type != OverviewModel.ItemTypes.Deleted).Count) // Some items are deleted!
							{
								foreach(var item in currentWeekPlan.Items.FindAll(e => e.Type != OverviewModel.ItemTypes.Deleted))
								{
									var deletedThisWeek = weekPlanItems.Find(e =>
										e.OperationId == item.OperationId
										&& e.HallId == item.HallId
										&& e.DepartementId == item.DepartementId
										&& e.WorkAreaId == item.WorkAreaId
										&& e.WorkStationId == item.WorkStationId);
									if(deletedThisWeek == null)
									{
										weekPlanItems.Add(new OverviewModel.Item()
										{
											Type = OverviewModel.ItemTypes.Deleted,

											OperationId = item.OperationId,
											OperationName = item.OperationName,
											HallId = item.HallId,
											HallName = item.HallName,
											DepartementId = item.DepartementId,
											DepartementName = item.DepartementName,
											WorkAreaId = item.WorkAreaId,
											WorkAreaName = item.WorkAreaName,
											WorkStationId = item.WorkStationId,
											WorkStationName = item.WorkStationName,
											FormToolInsert = item.FormToolInsert,

											HrHardResourcesNumber = item.HrHardResourcesNumber,
											HrF1PersonPerMachine = item.HrF1PersonPerMachine,
											HrF2UtilisationRate = item.HrF2UtilisationRate,
											HrProductivity = item.HrProductivity,

											SrSoftResourcesNumber = item.SrSoftResourcesNumber,
											SrF3AttendanceLevel = item.SrF3AttendanceLevel,
											SrProductivity = item.SrProductivity,

											AvailableHr = item.AvailableHr,
											AvailableSr = item.AvailableSr,

											ShiftsPerWeek = item.ShiftsPerWeek,
											SpecialShiftsPerWeek = item.SpecialShiftsPerWeek,
											SpecialHours = item.SpecialHours,
											WorkingHoursPerShift = item.WorkingHoursPerShift,

											AttendancePerWeek = Common.Helpers.MathHelper.RoundDecimal(item.AttendancePerWeek, Helpers.Config.DecimalPart),
											PlannableCapacites = Common.Helpers.MathHelper.RoundDecimal(item.PlannableCapacites, Helpers.Config.DecimalPart),
											RequiredEmployeesNumber = Common.Helpers.MathHelper.RoundDecimal(item.RequiredEmployeesNumber, Helpers.Config.DecimalPart),

											// -
											Id = item.Id
										});
									}
								}

								currentWeekPlan = new OverviewModel.WeeklyPlan()
								{
									FirstWeekNumber = weekNumber,
									LastWeekNumber = weekNumber,
									Items = new List<OverviewModel.Item>(),
								};
								currentWeekPlan.Items.AddRange(weekPlanItems);
								responseData.Add(currentWeekPlan);
							}
							else
							{
								currentWeekPlan.LastWeekNumber = weekNumber;
							}
						}
						#endregion
					}

					// > Remove empty lists
					responseData = responseData.FindAll(e => e.Items.Count > 0);
					//?.FindAll(x=> x.Items.Exists(y=> y.Type != OverviewModel.ItemTypes.Deleted)); // - do not return all deleted blocks

					for(int i = 0; i < responseData.Count; i++)
					{
						responseData[i].Items = responseData[i].Items?.OrderBy(y => y.HallId)
								?.ThenBy(y => y.DepartementId)
								?.ThenBy(y => y.WorkAreaId)
								?.ThenBy(y => y.WorkStationId)
								?.ToList();
					}

					var response = new OverviewModel()
					{
						CanEdit = true,
						CanReject = false,
						CanValidate = false,
						CanUnValidate = false,
						Level = 0,
						Data = responseData
					};

					#region >>> Edit, Validation & Rejection
					if(response.Data.Count > 0)
					{
						// -
						response.CanEdit = false;
						response.CanValidate = false;
						response.CanReject = false;

						var firstCreated = CapacityPlanAccess.GetCreationBy_CountryId_HallId_Year(this.data.CurrentCountryId,
						this.data.CurrentHallId ?? -1,
						this.data.Year);
						var createUserId = firstCreated?.CreationUserId;

						var createUser = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(createUserId ?? -1);
						response.CreateuserId = createUserId ?? -1;
						response.CreateUserName = createUser?.Username;
						response.CreateTime = firstCreated?.CreationTime;

						var lastValidationEntity = CapacityPlanValidationLogAccess.GetLastByYearCountryHall(data.Year, data.CurrentCountryId, data.CurrentHallId ?? -1);
						if(lastValidationEntity == null)
						{
							if(this.user.Id == createUserId)
							{
								response.CanEdit = true;
								response.CanValidate = true;
								response.CanReject = false;
							}
						}
						else
						{
							if((int)lastValidationEntity.ValidationStatus == (int)Enums.CapacityPlan.ValidationStatuses.Rejected
								|| (int)lastValidationEntity.ValidationStatus == (int)Enums.CapacityPlan.ValidationStatuses.Unvalidated)
							{
								if(this.user.Id == createUserId)
								{
									response.CanEdit = true;
									response.CanValidate = true;
									response.CanReject = false;
								}
							}
							else
							{
								if(lastValidationEntity.ValidationLevel == 0)
								{
									response.Level = 1;
									var level1UserEntities = CapacityPlanValidationUserAccess.GetByCountryUserLevel(data.CurrentCountryId, 1);
									if(level1UserEntities != null && level1UserEntities.FindIndex(x => x.UserId == this.user.Id) >= 0)
									{
										response.CanEdit = false;
										response.CanValidate = true;
										response.CanReject = true;
									}
								}
								else if(lastValidationEntity.ValidationLevel == 1)
								{
									response.Level = 2;
									var level2UserEntities = CapacityPlanValidationUserAccess.GetByCountryUserLevel(data.CurrentCountryId, 2);
									if(level2UserEntities != null && level2UserEntities.FindIndex(x => x.UserId == this.user.Id) >= 0)
									{
										response.CanEdit = false;
										response.CanValidate = true;
										response.CanReject = true;
									}
								}
								else if(lastValidationEntity.ValidationLevel == 2)
								{
									response.Level = 3;
									var level2UserEntities = CapacityPlanValidationUserAccess.GetByCountryUserLevel(data.CurrentCountryId, 2);
									if(level2UserEntities != null && level2UserEntities.FindIndex(x => x.UserId == this.user.Id) >= 0)
									{
										response.CanEdit = false;
										response.CanValidate = false;
										response.CanReject = false;
										response.CanUnValidate = true;
									}
								}
							}
						}
					}
					#endregion

					return ResponseModel<OverviewModel>.SuccessResponse(response);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public ResponseModel<OverviewModel> Validate()
		{
			throw new NotImplementedException();
		}
	}
}
