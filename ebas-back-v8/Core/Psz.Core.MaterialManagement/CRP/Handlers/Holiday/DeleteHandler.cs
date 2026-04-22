using Infrastructure.Data.Access.Tables.MTM;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Holiday
{
	public class DeleteHandler: IHandle<int, ResponseModel<object>>
	{
		private CRP.Models.Holiday.DeleteHolidayModel data { get; set; }
		private Psz.Core.Identity.Models.UserModel user { get; set; }

		public DeleteHandler(CRP.Models.Holiday.DeleteHolidayModel data,
			Core.Identity.Models.UserModel user)
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

					var holidayEntity = HolidayAccess.Get(this.data.Id);
					if(holidayEntity == null || holidayEntity.IsArchived)
					{
						return ResponseModel<object>.SuccessResponse();
					}

					if(holidayEntity.Day <= DateTime.Today)
					{
						return ResponseModel<object>.FailureResponse("Cannot delete past holiday");
					}

					// > Archive (can be improved)
					holidayEntity.IsArchived = true;
					holidayEntity.ArchiveTime = DateTime.Now;
					holidayEntity.ArchiveUserId = user.Id;
					if(this.data.EditSimilar == true)
					{
						HolidayAccess.UpdateWSimilar(holidayEntity);
					}
					else
					{
						HolidayAccess.Update(holidayEntity);
					}

					return ResponseModel<object>.SuccessResponse();
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public ResponseModel<object> Validate()
		{
			throw new NotImplementedException();
		}
	}
}
