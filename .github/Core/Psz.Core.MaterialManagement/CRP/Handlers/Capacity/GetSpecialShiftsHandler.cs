using Infrastructure.Data.Access.Tables.MTM;
using Infrastructure.Data.Access.Tables.WPL;
using Infrastructure.Data.Entities.Tables.MTM;
using Psz.Core.MaterialManagement.CRP.Models.Capacity;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Capacity
{
	public class GetSpecialShiftsHandler: IHandle<SpecialShiftRequestModel, ResponseModel<List<SpecialShiftResponseModel>>>
	{
		private SpecialShiftRequestModel data { get; set; }
		private Psz.Core.Identity.Models.UserModel user { get; set; }

		public GetSpecialShiftsHandler(Identity.Models.UserModel user, SpecialShiftRequestModel data)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<List<SpecialShiftResponseModel>> Handle()
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
					var countryEntity = CountryAccess.Get(this.data.CountryId);
					if(countryEntity == null || countryEntity.IsArchived)
					{
						return ResponseModel<List<SpecialShiftResponseModel>>.FailureResponse("Country is not found");
					}
					var hallEntity = HallAccess.Get(this.data.HallId);
					if(hallEntity == null || hallEntity.IsArchived)
					{
						return ResponseModel<List<SpecialShiftResponseModel>>.FailureResponse("Hall is not found");
					}
					#endregion

					var responseData = new List<SpecialShiftResponseModel>();
					var capacityEntities = CapacityAccess.GetSpecialShifts(year: null, countryId: this.data.CountryId, hallId: this.data.HallId)
						?? new List<CapacityEntity>();

					foreach(var capacityItem in capacityEntities)
					{
						responseData.Add(new SpecialShiftResponseModel(capacityItem));
					}

					return ResponseModel<List<SpecialShiftResponseModel>>.SuccessResponse(responseData);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public ResponseModel<List<SpecialShiftResponseModel>> Validate()
		{
			throw new NotImplementedException();
		}
	}
}
