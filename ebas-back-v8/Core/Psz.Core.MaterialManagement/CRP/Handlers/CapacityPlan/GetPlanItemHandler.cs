using Infrastructure.Data.Access.Tables.MTM;
using Psz.Core.MaterialManagement.CRP.Models.CapacityPlan;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.CRP.Handlers.CapacityPlan
{
	public class GetPlanItemHandler: IHandle<GetPlanItemRequestModel, ResponseModel<List<CapacityPlanItemModel>>>
	{
		private GetPlanItemRequestModel data { get; set; }
		private Psz.Core.Identity.Models.UserModel user { get; set; }

		public GetPlanItemHandler(GetPlanItemRequestModel data,
			Identity.Models.UserModel user)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<List<CapacityPlanItemModel>> Handle()
		{
			lock(Locks.HolidayLock)
			{
				try
				{
					if(user == null)
					{
						throw new Psz.Core.SharedKernel.Exceptions.UnauthorizedException();
					}

					var capacityPlanEntities = CapacityPlanAccess.Get(this.data.CurrentCountryId,
						this.data.Year,
						this.data.OperationId,
						this.data.HallId,
						this.data.DepartementId,
						this.data.WorkAreaId,
						this.data.WorkStationId,
						this.data.WeekFrom,
						this.data.WeekTo);

					var capacityItems = new List<CapacityPlanItemModel>();

					foreach(var capacityPlanEntity in capacityPlanEntities)
					{
						capacityItems.Add(new CapacityPlanItemModel(capacityPlanEntity));
					}

					return ResponseModel<List<CapacityPlanItemModel>>.SuccessResponse(capacityItems);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public ResponseModel<List<CapacityPlanItemModel>> Validate()
		{
			throw new NotImplementedException();
		}
	}
}
