using Psz.Core.MaterialManagement.CRP.Models.CapacityPlan;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.CRP.Handlers.CapacityPlan
{
	public class CalculateItemHandler: IHandle<CalculateItemModel, ResponseModel<CalculatedItemModel>>
	{
		private CalculateItemModel data { get; set; }
		private Psz.Core.Identity.Models.UserModel user { get; set; }

		public CalculateItemHandler(CalculateItemModel data,
			Identity.Models.UserModel user)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<CalculatedItemModel> Handle()
		{
			lock(Locks.HolidayLock)
			{
				try
				{
					if(user == null)
					{
						throw new Psz.Core.SharedKernel.Exceptions.UnauthorizedException();
					}

					var errors = new List<ResponseModel<CalculatedItemModel>.ResponseError>();

					#region > Validation
					foreach(var errorMessage in this.data.Validate())
					{
						errors.Add(new ResponseModel<CalculatedItemModel>.ResponseError()
						{
							Value = errorMessage,
						});
					}
					#endregion

					var responseData = Perform(this.data);

					return ResponseModel<CalculatedItemModel>.SuccessResponse(responseData);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public ResponseModel<CalculatedItemModel> Validate()
		{
			throw new NotImplementedException();
		}

		internal static CalculatedItemModel Perform(CalculateItemModel data)
		{
			var availableHr = data.HrHardResourcesNumber * data.HrF2UtilisationRate * data.HrProductivity;
			var availableSr = data.SrSoftResourcesNumber * data.SrF3AttendanceLevel * data.SrProductivity;
			var attendancePerWeek = (data.ShiftsPerWeek * data.WorkingHoursPerShift) + (data.SpecialShiftsPerWeek * data.WorkingHoursPerShift) + data.SpecialHours;
			var plannableCapacites = (availableHr * attendancePerWeek) + (availableSr * attendancePerWeek);
			var requiredEmployeesNumber = calculateRequiredEmployeesNumber(data.ShiftsPerWeek,
				data.HrHardResourcesNumber,
				data.HrF1PersonPerMachine,
				data.SrSoftResourcesNumber);

			var responseData = new CalculatedItemModel()
			{
				Id = data.Id,
				OperationId = data.OperationId,
				OperationName = data.OperationName,
				HallId = data.HallId,
				HallName = data.HallName,
				DepartementId = data.DepartementId,
				DepartementName = data.DepartementName,
				WorkAreaId = data.WorkAreaId,
				WorkAreaName = data.WorkAreaName,
				WorkStationId = data.WorkStationId,
				WorkStationName = data.WorkStationName,
				FormToolInsert = data.FormToolInsert,

				HrHardResourcesNumber = data.HrHardResourcesNumber,
				HrF1PersonPerMachine = data.HrF1PersonPerMachine, // [0,25 : 3,00] +0,25		
				HrF2UtilisationRate = data.HrF2UtilisationRate, // [0,50 : 1,00] +0,05		
				HrProductivity = data.HrProductivity, // [0,20 : 1,00] +0,01		

				SrSoftResourcesNumber = data.SrSoftResourcesNumber,
				SrF3AttendanceLevel = data.SrF3AttendanceLevel, // Dropdown-list [0,50 : 1,00] +0,05		
				SrProductivity = data.SrProductivity, // Dropdown-list [0,20 : 1,00] +0,01		

				ShiftsPerWeek = data.ShiftsPerWeek, // Dropdown-list [0:15] +1		
				SpecialShiftsPerWeek = data.SpecialShiftsPerWeek, // Dropdown-list[0:5] +1	
				SpecialHours = data.SpecialHours, // Dropdown-list [0:28] +1	
				WorkingHoursPerShift = data.WorkingHoursPerShift, // Dropdown-list [0:12] +0.25	

				AvailableHr = availableHr,
				AvailableSr = availableSr,


				AttendancePerWeek = attendancePerWeek,
				PlannableCapacites = plannableCapacites,
				RequiredEmployeesNumber = requiredEmployeesNumber,
			};

			return responseData;
		}

		private static decimal calculateRequiredEmployeesNumber(decimal shiftsPerWeek,
			decimal hrHardResourcesNumber,
			decimal hrF1PersonPerMachine,
			decimal srSoftResourcesNumber)
		{
			if(shiftsPerWeek < 1)
			{
				return 0m;
			}
			else if(shiftsPerWeek <= 5)
			{
				return (hrHardResourcesNumber * hrF1PersonPerMachine) + srSoftResourcesNumber;
			}
			else if(shiftsPerWeek <= 10)
			{
				return ((hrHardResourcesNumber * hrF1PersonPerMachine) + srSoftResourcesNumber) * 2;
			}
			else if(shiftsPerWeek <= 15)
			{
				return ((hrHardResourcesNumber * hrF1PersonPerMachine) + srSoftResourcesNumber) * 3;
			}
			else
			{
				throw new Exception("Required Employees Number Calulation: shiftsPerWeek is not valid");
			}
		}
	}
}
