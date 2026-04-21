using Infrastructure.Data.Access.Tables.MTM;
using Infrastructure.Data.Access.Tables.WPL;
using Psz.Core.MaterialManagement.CRP.Models.Holiday;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Holiday
{
	public class CreateHolidaysHandler: IHandle<Models.Holiday.CreateHolidaysModel, ResponseModel<object>>
	{
		private Models.Holiday.CreateHolidaysModel data { get; set; }
		private Psz.Core.Identity.Models.UserModel user { get; set; }

		public CreateHolidaysHandler(Models.Holiday.CreateHolidaysModel data,
			Identity.Models.UserModel user)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<object> Handle()
		{
			lock(Locks.HolidayLock)
			{
				try
				{
					if(user == null)
					{
						throw new Psz.Core.SharedKernel.Exceptions.UnauthorizedException();
					}

					var countryEntity = CountryAccess.Get(this.data.CurrentCountryId);
					if(countryEntity == null || countryEntity.IsArchived)
					{
						return ResponseModel<object>.FailureResponse("Country is not found");
					}

					var errors = new List<ResponseModel<object>.ResponseError>();
					var existingHolidayEntities = HolidayAccess.GetByCountryHall(this.data.CurrentCountryId, this.data.CurrentHallId)
						?? new List<Infrastructure.Data.Entities.Tables.MTM.HolidayEntity>();

					var i = 0;
					foreach(var item in this.data.Items)
					{
						i++;

						//if(item.Day.Date <= DateTime.Today)
						//{
						//	errors.Add(new ResponseModel<object>.ResponseError()
						//	{
						//		Value = $"Item {i}: Day is not valid",
						//	});
						//}
						//else
						{
							if(existingHolidayEntities.Where(x => x.CountryId == data.CurrentCountryId && (!data.CurrentHallId.HasValue || data.CurrentHallId.HasValue && x.HallId == data.CurrentHallId) && x.Day == item.Day)?.Any() == true)
							{
								errors.Add(new ResponseModel<object>.ResponseError()
								{
									Value = $"Item {i}: A holiday exists with same Date, Country and Hall",
								});
							}
						}

						if(string.IsNullOrWhiteSpace(item.Name))
						{
							errors.Add(new ResponseModel<object>.ResponseError()
							{
								Value = $"Item {i}: Name is empty",
							});
						}
						else
						{
							if(existingHolidayEntities.Where(x => x.Day == item.Day).Where(x => x.CountryId == data.CurrentCountryId && (!data.CurrentHallId.HasValue || data.CurrentHallId.HasValue && x.HallId == data.CurrentHallId) && x.Name.Trim().ToLower() == item.Name.Trim().ToLower())?.Any() == true)
							{
								errors.Add(new ResponseModel<object>.ResponseError()
								{
									Value = $"Item {i}: A holiday exists with same Name, Country and Hall",
								});
							}
						}


					}

					if(errors.Count > 0)
					{
						return ResponseModel<object>.FailureResponse(errors.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToList());
					}

					// MISSING MORE VALIDATIONS (name exists if same hallId in database and in request data)

					var countryHallEntities = new List<Infrastructure.Data.Entities.Tables.WPL.HallEntity>();
					if(this.data.CurrentHallId.HasValue)
					{// - Holiday for a specific Hall
						var hallEntity = HallAccess.Get(this.data.CurrentHallId.Value);
						if(hallEntity != null && !hallEntity.IsArchived && hallEntity.CountryId == countryEntity.Id)
						{
							countryHallEntities.Add(hallEntity);
						}
					}
					else
					{// - Holiday for all Halls in Country
						countryHallEntities = HallAccess.GetByCountryId(countryEntity.Id)
							.FindAll(e => !e.IsArchived);
					}

					if(countryHallEntities.Count == 0)
					{
						return ResponseModel<object>.FailureResponse($"{(this.data.CurrentHallId.HasValue ? "Hall" : "Halls")} not found");
					}

					var holidays = new List<Infrastructure.Data.Entities.Tables.MTM.HolidayEntity>();

					foreach(var item in this.data.Items)
					{
						var weekNumber = Helpers.DateTimeHelper.GetIso8601WeekOfYear(item.Day);
						holidays.AddRange(countryHallEntities.Select(hallEntity => new Infrastructure.Data.Entities.Tables.MTM.HolidayEntity()
						{
							Id = -1,

							CountryId = countryEntity.Id,
							CountryName = countryEntity.Name,
							HallId = hallEntity.Id,
							HallName = hallEntity.Name,

							Day = item.Day,
							Name = item.Name.Trim(),

							CreationTime = DateTime.Now,
							CreationUserId = user.Id,
							IsArchived = false,
							ArchiveTime = null,
							ArchiveUserId = null,
							WeekNumber = weekNumber,
							//IsOverwritten= isHolidayOverwritten(countryEntity.Id, hallEntity.Id, item.Day)
						})?.ToList() ?? new List<Infrastructure.Data.Entities.Tables.MTM.HolidayEntity>());
					}

					if(holidays.Count > 0)
						HolidayAccess.Insert(holidays);

					if(errors.Count > 0)
					{
						return ResponseModel<object>.FailureResponse(errors.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToList());
					}

					return ResponseModel<object>.SuccessResponse();
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		ResponseModel<object> IHandle<CreateHolidaysModel, ResponseModel<object>>.Validate()
		{
			throw new NotImplementedException();
		}

		public static bool isHolidayOverwritten(int countryId, int hallId, DateTime date)
		{
			if(date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
				return false;

			var kw = Helpers.DateTimeHelper.GetWeeksNumberInYear(date);
			return Infrastructure.Data.Access.Tables.MTM.CapacityAccess.HasSpecialShifts(countryId, hallId, date.Year, kw);
		}
	}
}
