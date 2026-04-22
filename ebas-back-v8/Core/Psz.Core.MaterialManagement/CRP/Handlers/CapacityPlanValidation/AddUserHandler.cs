using Infrastructure.Data.Access.Tables.MTM;
using Infrastructure.Data.Access.Tables.WPL;
using Infrastructure.Data.Entities.Tables.MTM;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.CapacityPlanValidation
{
	public class AddUserHandler: IHandle<Models.CapacityPlanValidation.AddUserModel, ResponseModel<int>>
	{
		private Psz.Core.Identity.Models.UserModel user { get; set; }
		private Models.CapacityPlanValidation.AddUserModel data { get; set; }

		public AddUserHandler(Identity.Models.UserModel user, Models.CapacityPlanValidation.AddUserModel data)
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
					var countryEntity = CountryAccess.Get(data.CountryId);
					var userEntities = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data.UserIds);
					if(userEntities == null || userEntities.Count <= 0)
						return ResponseModel<int>.SuccessResponse();

					return ResponseModel<int>.SuccessResponse(
						CapacityPlanValidationUserAccess.Insert(userEntities.Select(x => new CapacityPlanValidationUserEntity
						{
							CountryId = countryEntity.Id,
							CountryName = countryEntity.Name,
							CreationTime = DateTime.Now,
							CreationUserId = user.Id,
							UserEmail = x.Email,
							UserId = x.Id,
							UserName = x.Name,
							ValidationLevel = data.Level
						})?.ToList()));
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

			if(CountryAccess.Get(data.CountryId) == null)
				return ResponseModel<int>.FailureResponse("Country not found.");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
