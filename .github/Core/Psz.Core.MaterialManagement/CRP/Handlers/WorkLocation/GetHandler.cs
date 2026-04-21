using Infrastructure.Data.Access.Tables.WPL;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.WorkLocation
{
	public class GetHandler: IHandle<Psz.Core.Identity.Models.UserModel, ResponseModel<Models.WorkLocation.WorkLocationsModel>>
	{
		private Psz.Core.Identity.Models.UserModel user { get; set; }
		public bool data { get; set; }

		public GetHandler(Psz.Core.Identity.Models.UserModel user, bool filterByUser)
		{
			this.user = user;
			this.data = filterByUser;
		}

		public ResponseModel<Models.WorkLocation.WorkLocationsModel> Handle()
		{
			try
			{
				if(user == null)
				{
					throw new Psz.Core.SharedKernel.Exceptions.UnauthorizedException();
				}

				var userHallEntities = HallAuthorizationAccess.GetByUserId(this.user.Id); // - >>> This is defined in WPL >>> HallAuthorizationAccess.GetByUserId(this.user.Id);

				var hallIds = userHallEntities.Select(e => e.HallId).ToList();
				var hallEntities = (this.data ? HallAccess.Get(hallIds) : HallAccess.Get())
					.FindAll(e => !e.IsArchived);

				var countryIds = hallEntities.Select(e => e.CountryId).ToList();
				var countryEntities = CountryAccess.Get(countryIds)
					.FindAll(e => !e.IsArchived);

				var response = new Models.WorkLocation.WorkLocationsModel();

				foreach(var countryEntity in countryEntities)
				{
					var country = new Models.WorkLocation.WorkLocationsModel.Country()
					{
						Id = countryEntity.Id,
						Name = countryEntity.Name,
						Halls = new List<Models.WorkLocation.WorkLocationsModel.Country.Hall>(),
					};

					var _halls = hallEntities.FindAll(e => e.CountryId == countryEntity.Id)?.DistinctBy(x => x.Name);
					foreach(var hallEntity in _halls)
					{
						country.Halls.Add(new Models.WorkLocation.WorkLocationsModel.Country.Hall()
						{
							Id = hallEntity.Id,
							Name = hallEntity.Name,
							Area = hallEntity.Adress,
						});
					}

					response.Countries.Add(country);
				}

				return ResponseModel<Models.WorkLocation.WorkLocationsModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.WorkLocation.WorkLocationsModel> Validate()
		{
			throw new NotImplementedException();
		}
	}
}
