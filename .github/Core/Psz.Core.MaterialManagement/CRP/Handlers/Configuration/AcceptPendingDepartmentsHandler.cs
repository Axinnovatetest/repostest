using Infrastructure.Data.Access.Tables.MTM;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Configuration
{
	public class AcceptPendingDepartmentsHandler: IHandle<Psz.Core.Identity.Models.UserModel, ResponseModel<int>>
	{
		private Psz.Core.Identity.Models.UserModel user { get; set; }
		private List<int> data { get; set; }

		public AcceptPendingDepartmentsHandler(List<int> data, Identity.Models.UserModel user)
		{
			this.user = user;
			this.data = data;
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

					return Perform(this.user, this.data);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public static ResponseModel<int> Perform(Identity.Models.UserModel user, List<int> data)
		{
			return ResponseModel<int>.SuccessResponse(ConfigurationDetailsAccess.AcceptPendingDepartments(data, user.Id, DateTime.Now));
		}

		public ResponseModel<int> Validate()
		{
			if(user == null)
				throw new Psz.Core.SharedKernel.Exceptions.UnauthorizedException();

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
