using Infrastructure.Data.Access.Tables.MTM;
using Infrastructure.Data.Access.Tables.WPL;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.WorkLocation
{
	public class GetWorkAreasHandler: IHandle<Psz.Core.Identity.Models.UserModel, ResponseModel<List<Models.WorkLocation.WorkAreaModel>>>
	{
		private Psz.Core.Identity.Models.UserModel user { get; set; }

		public GetWorkAreasHandler(Psz.Core.Identity.Models.UserModel user)
		{
			this.user = user;
		}

		public ResponseModel<List<Models.WorkLocation.WorkAreaModel>> Handle()
		{
			try
			{
				if(user == null)
				{
					throw new Psz.Core.SharedKernel.Exceptions.UnauthorizedException();
				}

				var authorizedHallEntities = HallAuthorizationAccess.Get(); // - >>> This is defined in WPL >>> HallAuthorizationAccess.GetByUserId(this.user.Id);
				var authorizedWorkAreaEntities = WorkAreaAuthorizationAccess.Get(); // - >>> This is defined in WPL >>> WorkAreaAuthorizationAccess.GetByUserId(this.user.Id);
				var authorizedWorkStationEntities = UserWorkStationAuthorizationAccess.Get(); // - >>> This is defined in WPL >>> UserWorkStationAuthorizationAccess.GetByUserId(this.user.Id);

				var departmentEntities = DepartmentAccess.Get()?.Where(x => x.IsArchived != true)?.Distinct()?.ToList();
				var hallIds = authorizedHallEntities.Select(e => e.HallId).ToList();
				var hallEntities = HallAccess.Get(/*hallIds*/).FindAll(e => !e.IsArchived);

				var countryIds = hallEntities.Select(e => e.CountryId).ToList();
				var countryEntities = CountryAccess.Get(/*countryIds*/)
					.FindAll(e => !e.IsArchived);

				var workAreaIds = authorizedWorkAreaEntities.Select(e => e.WorkAreaId).ToList();
				var workAreaEntities = WorkAreaAccess.Get(/*workAreaIds*/)
					.FindAll(e => !e.IsArchived);

				var workStationIds = authorizedWorkStationEntities.Select(e => e.WorkStationId).ToList();
				var workStationEntities = WorkStationMachineAccess.Get(/*workStationIds*/)
					.FindAll(e => !e.IsArchived);

				var response = new List<Models.WorkLocation.WorkAreaModel>();

				foreach(var workAreaEntity in workAreaEntities)
				{
					var countryEntity = countryEntities.Find(e => e.Id == workAreaEntity.CountryId);
					var hallEntity = hallEntities.Find(e => e.Id == workAreaEntity.HallId);
					var departmentEntity = departmentEntities?.Find(e => e.Id == workAreaEntity.DepartmentId);

					var workArea = new Models.WorkLocation.WorkAreaModel(workAreaEntity,
						workStationEntities.FindAll(e => e.WorkAreaId == workAreaEntity.Id),
						countryEntity, hallEntity, departmentEntity);

					response.Add(workArea);
				}

				return ResponseModel<List<Models.WorkLocation.WorkAreaModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.WorkLocation.WorkAreaModel>> Validate()
		{
			throw new NotImplementedException();
		}
	}
}
