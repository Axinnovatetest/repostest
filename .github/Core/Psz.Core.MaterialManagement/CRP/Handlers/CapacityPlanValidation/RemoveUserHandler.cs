using Infrastructure.Data.Access.Tables.MTM;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.CRP.Handlers.CapacityPlanValidation
{
	public class RemoveUserHandler: IHandle<Models.CapacityPlanValidation.RemoveUserModel, ResponseModel<int>>
	{
		private Psz.Core.Identity.Models.UserModel user { get; set; }
		private Models.CapacityPlanValidation.RemoveUserModel data { get; set; }

		public RemoveUserHandler(Identity.Models.UserModel user, Models.CapacityPlanValidation.RemoveUserModel data)
		{
			this.user = user;
			this.data = data;
		}

		public ResponseModel<int> Handle()
		{
			lock(Locks.CapacityPlanLock)
			{
				try
				{
					Validate();

					//-

					return ResponseModel<int>.SuccessResponse(
						CapacityPlanValidationUserAccess.DeleteByCountryUserLevel(data.CountryId, data.UserId, data.Level));
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public ResponseModel<int> Validate()
		{
			if(user == null)
			{
				throw new Psz.Core.SharedKernel.Exceptions.UnauthorizedException();
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
