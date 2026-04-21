using Infrastructure.Data.Access.Tables.MTM;
using Infrastructure.Data.Access.Tables.WPL;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.WorkLocation
{
	public class GetDepartementsHandler: IHandle<Psz.Core.Identity.Models.UserModel, ResponseModel<List<Models.WorkLocation.DepartementModel>>>
	{
		private Psz.Core.Identity.Models.UserModel user { get; set; }

		public GetDepartementsHandler(Psz.Core.Identity.Models.UserModel user)
		{
			this.user = user;
		}

		public ResponseModel<List<Models.WorkLocation.DepartementModel>> Handle()
		{
			try
			{
				if(user == null)
				{
					throw new Psz.Core.SharedKernel.Exceptions.UnauthorizedException();
				}

				var authorizedDepartementEntities = UserDepartementAuthorizationAccess.Get(); // - >>> This is defined in WPL >>> UserDepartementAuthorizationAccess.GetByUserId(this.user.Id);
				var authorizedWorkAreaEntities = WorkAreaAuthorizationAccess.Get(); // - >>> This is defined in WPL >>> WorkAreaAuthorizationAccess.GetByUserId(this.user.Id);
				var authorizedWorkStationEntities = UserWorkStationAuthorizationAccess.Get(); // - >>> This is defined in WPL >>> UserWorkStationAuthorizationAccess.GetByUserId(this.user.Id);

				var departementIds = authorizedDepartementEntities.Select(e => e.DepartementId).ToList();
				var departementEntities = DepartmentAccess.Get(/*departementIds*/)
					.FindAll(e => !e.IsArchived)?.Distinct()?.ToList();

				var workAreaIds = authorizedWorkAreaEntities.Select(e => e.WorkAreaId).ToList();
				var workAreaEntities = WorkAreaAccess.Get(/*workAreaIds*/)
					.FindAll(e => !e.IsArchived);

				var workStationIds = authorizedWorkStationEntities.Select(e => e.WorkStationId).ToList();
				var workStationEntities = WorkStationMachineAccess.Get(/*workStationIds*/)
					.FindAll(e => !e.IsArchived);

				var countryEntities = CountryAccess.Get()?.Where(x => x.IsArchived != true)?.ToList();
				var hallEntities = HallAccess.Get()?.Where(x => x.IsArchived != true)?.ToList();

				var response = new List<Models.WorkLocation.DepartementModel>();

				foreach(var departementEntity in departementEntities)
				{
					var departementWorkAreaEntities = workAreaEntities.FindAll(e => e.DepartmentId.HasValue && e.DepartmentId.Value == departementEntity.Id)?.Distinct()?.ToList();
					var departementWorkAreaIds = departementWorkAreaEntities.Select(e => e.Id)?.Distinct()?.ToList();
					var departementWorkStationEntities = workStationEntities.FindAll(e => departementWorkAreaIds.Contains(e.WorkAreaId))?.Distinct()?.ToList();

					var departement = new Models.WorkLocation.DepartementModel(departementEntity,
						departementWorkAreaEntities,
						departementWorkStationEntities,
						countryEntities,
						hallEntities,
						departementEntities);

					response.Add(departement);
				}

				return ResponseModel<List<Models.WorkLocation.DepartementModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.WorkLocation.DepartementModel>> Validate()
		{
			throw new NotImplementedException();
		}
	}
}
