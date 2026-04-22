using Infrastructure.Data.Access.Tables.MTM;
using Infrastructure.Data.Access.Tables.WPL;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.CapacityPlanValidation
{
	public class GetAllHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.CapacityPlanValidation.GetModel>>>
	{
		private Psz.Core.Identity.Models.UserModel user { get; set; }

		public GetAllHandler(Identity.Models.UserModel user)
		{
			this.user = user;
		}

		public ResponseModel<List<Models.CapacityPlanValidation.GetModel>> Handle()
		{
			lock(Locks.CapacityPlanLock)
			{
				try
				{
					if(user == null)
					{
						throw new Psz.Core.SharedKernel.Exceptions.UnauthorizedException();
					}

					var capacityPlanEntities = CapacityPlanValidationUserAccess.Get();
					var capacityValidationItems = new List<Models.CapacityPlanValidation.GetModel>();
					var countryEntities = CountryAccess.Get();

					var countryWConfig = capacityPlanEntities?.Select(x => x.CountryId)?.Distinct()?.ToList() ?? new List<int> { };

					foreach(var countryId in countryWConfig)
					{
						var countryConfigItem = capacityPlanEntities.FindAll(x => x.CountryId == countryId);
						if(countryConfigItem != null && countryConfigItem.Count > 0)
						{
							capacityValidationItems.Add(new Models.CapacityPlanValidation.GetModel()
							{
								CountryId = countryConfigItem[0].CountryId,
								CountryName = countryConfigItem[0].CountryName,
								Level1 = countryConfigItem
									.Where(x => x.ValidationLevel == 1)
									?.Select(x => new Models.CapacityPlanValidation.ValidationUserModel
									{
										UserId = x.UserId,
										UserName = x.UserName,
										UserEmail = x.UserEmail
									})?.ToList(),
								Level2 = countryConfigItem
									.Where(x => x.ValidationLevel == 2)
									?.Select(x => new Models.CapacityPlanValidation.ValidationUserModel
									{
										UserId = x.UserId,
										UserName = x.UserName,
										UserEmail = x.UserEmail
									})?.ToList()
							});
						}
					}

					// - filter country w/ config
					countryEntities = countryEntities.Where(x => !countryWConfig.Exists(y => y == x.Id))?.ToList();
					if(countryEntities != null && countryEntities.Count > 0)
					{
						foreach(var countryItem in countryEntities)
						{
							capacityValidationItems.Add(new Models.CapacityPlanValidation.GetModel
							{
								CountryId = countryItem.Id,
								CountryName = countryItem.Name,
								Level1 = new List<Models.CapacityPlanValidation.ValidationUserModel>(),
								Level2 = new List<Models.CapacityPlanValidation.ValidationUserModel>()
							});
						}
					}

					capacityValidationItems = capacityValidationItems?.OrderBy(x => x.CountryId)?.ToList();
					return ResponseModel<List<Models.CapacityPlanValidation.GetModel>>.SuccessResponse(capacityValidationItems);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public ResponseModel<List<Models.CapacityPlanValidation.GetModel>> Validate()
		{
			throw new NotImplementedException();
		}
	}
}
