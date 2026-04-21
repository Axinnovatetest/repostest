using Infrastructure.Data.Access.Tables.MTM;
using Psz.Core.MaterialManagement.CRP.Models.Configuration;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Configuration
{
	public class GetHandler: IHandle<Psz.Core.Identity.Models.UserModel, ResponseModel<List<AddResponseModel>>>
	{
		private Psz.Core.Identity.Models.UserModel user { get; set; }

		public GetHandler(Identity.Models.UserModel user)
		{
			this.user = user;
		}

		public ResponseModel<List<AddResponseModel>> Handle()
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

		public static ResponseModel<List<AddResponseModel>> Perform(Identity.Models.UserModel user)
		{
			var configHeaderEntities = ConfigurationHeaderAccess.Get();

			var response = new List<AddResponseModel>();
			if(configHeaderEntities != null && configHeaderEntities.Count > 0)
			{
				var configDetailEntities = ConfigurationDetailsAccess.GetByHeaders(configHeaderEntities.Select(x => x.Id)?.ToList());
				foreach(var item in configHeaderEntities)
				{
					var d = configDetailEntities?.Where(x => x.HeaderId == item.Id)?.ToList();
					response.Add(new AddResponseModel(item, d));
				}
			}

			return ResponseModel<List<AddResponseModel>>.SuccessResponse(response);
		}

		public ResponseModel<List<AddResponseModel>> Validate()
		{
			if(user == null)
				throw new Psz.Core.SharedKernel.Exceptions.UnauthorizedException();

			return ResponseModel<List<AddResponseModel>>.SuccessResponse();
		}
	}
}
