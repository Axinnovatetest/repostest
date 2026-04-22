using Infrastructure.Data.Access.Tables.MTM;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Configuration
{
	public class DeleteByIdHandler: IHandle<int, ResponseModel<int>>
	{
		private int data { get; set; }
		private Psz.Core.Identity.Models.UserModel user { get; set; }

		public DeleteByIdHandler(int data, Identity.Models.UserModel user)
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

		public static ResponseModel<int> Perform(Identity.Models.UserModel user, int data)
		{
			ConfigurationDetailsAccess.DeleteByHeader(data);
			return ResponseModel<int>.SuccessResponse(ConfigurationHeaderAccess.Delete(data));
		}

		public ResponseModel<int> Validate()
		{
			if(user == null)
				throw new Psz.Core.SharedKernel.Exceptions.UnauthorizedException();

			if(ConfigurationHeaderAccess.Get(data) == null)
				return ResponseModel<int>.FailureResponse("Configuration not found.");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
