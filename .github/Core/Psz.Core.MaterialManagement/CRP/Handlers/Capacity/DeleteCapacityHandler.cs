using Infrastructure.Data.Access.Tables.MTM;
using Psz.Core.MaterialManagement.CRP.Models.Capacity;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Capacity
{
	public class DeleteCapacityHandler: IHandle<SingleCapacityUpdateModel, ResponseModel<int>>
	{
		private int data { get; set; }
		private Psz.Core.Identity.Models.UserModel user { get; set; }

		public DeleteCapacityHandler(int data,
			Identity.Models.UserModel user)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<int> Handle()
		{
			lock(Locks.CapacityLock)
			{
				try
				{
					if(user == null)
					{
						throw new Psz.Core.SharedKernel.Exceptions.UnauthorizedException();
					}

					return Perform(this.user, this.data);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public static ResponseModel<int> Perform(Identity.Models.UserModel user, int data)
		{
			#region > Validation
			var capacityEntity = CapacityAccess.Get(data);
			if(capacityEntity == null)
				return ResponseModel<int>.FailureResponse("Capacity not found");

			if(DateTime.Today > capacityEntity.WeekLastDay.Date.AddDays(+1))
				return ResponseModel<int>.FailureResponse("Cannot change Capacity from the past");

			if(!Helpers.Config.CanEdit(capacityEntity.Year, capacityEntity.WeekNumber, Enums.Main.CapacityType.Capacity))
				return ResponseModel<int>.FailureResponse($"Cannot edit capacity beyond KW {Helpers.Config.GetCapacityLastEditableWeek()}");

			#endregion

			return ResponseModel<int>.SuccessResponse(CapacityAccess.Update_IsArchived_ArchiveTime_ArchiveUserId(capacityEntity.Id, true, DateTime.Now, user.Id));
		}

		public ResponseModel<int> Validate()
		{
			throw new NotImplementedException();
		}
	}
}
