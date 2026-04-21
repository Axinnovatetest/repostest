using Infrastructure.Data.Access.Tables.MTM;
using Psz.Core.MaterialManagement.CRP.Models.Configuration;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Configuration
{
	public class DeleteHandler: IHandle<DeleteRequestModel, ResponseModel<int>>
	{
		private DeleteRequestModel data { get; set; }
		private Psz.Core.Identity.Models.UserModel user { get; set; }

		public DeleteHandler(DeleteRequestModel data, Identity.Models.UserModel user)
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

		public static ResponseModel<int> Perform(Identity.Models.UserModel user, DeleteRequestModel data)
		{
			var configEntity = ConfigurationHeaderAccess.GetByCountryHallThreshold(data.CountryId, data.HallId, data.ProductionOrderThreshold);
			ConfigurationDetailsAccess.DeleteByHeader(configEntity.Id);
			return ResponseModel<int>.SuccessResponse(ConfigurationHeaderAccess.Delete(configEntity.Id));
		}

		public ResponseModel<int> Validate()
		{
			if(user == null)
				throw new Psz.Core.SharedKernel.Exceptions.UnauthorizedException();

			if(ConfigurationHeaderAccess.GetByCountryHallThreshold(data.CountryId, data.HallId, data.ProductionOrderThreshold) == null)
				return ResponseModel<int>.FailureResponse("Configuration not found.");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
