using Infrastructure.Data.Access.Tables.MTM;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Configuration
{
	public class GetValidationPendingDepartmentsHandler: IHandle<Psz.Core.Identity.Models.UserModel, ResponseModel<int>>
	{
		private Psz.Core.Identity.Models.UserModel user { get; set; }

		public GetValidationPendingDepartmentsHandler(Identity.Models.UserModel user)
		{
			this.user = user;
		}

		public ResponseModel<int> Handle()
		{
			lock(Locks.CapacityLock)
			{
				try
				{
					var validation = Validate();
					if(!validation.Success)
					{
						return validation;
					}

					return Perform(this.user);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public static ResponseModel<int> Perform(Identity.Models.UserModel user)
		{
			return ResponseModel<int>.SuccessResponse(ConfigurationDetailsAccess.GetValidationPendingDepartments());
		}

		public ResponseModel<int> Validate()
		{
			if(user == null)
				throw new Psz.Core.SharedKernel.Exceptions.UnauthorizedException();

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
