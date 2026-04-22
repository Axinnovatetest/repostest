using Infrastructure.Data.Access.Tables.MTM;
using Infrastructure.Data.Access.Tables.WPL;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.WorkLocation
{
	public class GetHallsHandler: IHandle<Psz.Core.Identity.Models.UserModel, ResponseModel<List<Models.WorkLocation.HallModel>>>
	{
		private Psz.Core.Identity.Models.UserModel user { get; set; }
		private int? data { get; set; }

		public GetHallsHandler(int? countryId, Psz.Core.Identity.Models.UserModel user)
		{
			this.user = user;
			this.data = countryId;
		}

		public ResponseModel<List<Models.WorkLocation.HallModel>> Handle()
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
				var hallEntities = HallAccess.Get(/*hallIds*/)
					.FindAll(e => !e.IsArchived);
				if(this.data.HasValue)
				{
					hallEntities = hallEntities.FindAll(e => e.CountryId == this.data.Value);
				}

				var countryIds = hallEntities.Select(e => e.CountryId).ToList();
				var countryEntities = CountryAccess.Get(/*countryIds*/)
					.FindAll(e => !e.IsArchived);

				var workAreaIds = authorizedWorkAreaEntities.Select(e => e.WorkAreaId).ToList();
				var workAreaEntities = WorkAreaAccess.Get(/*workAreaIds*/)
					.FindAll(e => !e.IsArchived);

				var workStationIds = authorizedWorkStationEntities.Select(e => e.WorkStationId).ToList();
				var workStationEntities = WorkStationMachineAccess.Get(/*workStationIds*/)
					.FindAll(e => !e.IsArchived);

				var response = new List<Models.WorkLocation.HallModel>();

				foreach(var hallEntity in hallEntities)
				{
					var countryEntity = countryEntities.Find(e => e.Id == hallEntity.CountryId);
					var hallWorkAreaEntities = workAreaEntities.FindAll(e => e.HallId == hallEntity.Id);
					var hallWorkAreaIds = hallWorkAreaEntities.Select(e => e.Id).ToList();
					var hallWorkStationEntities = workStationEntities.FindAll(e => hallWorkAreaIds.Contains(e.WorkAreaId));

					var hall = new Models.WorkLocation.HallModel(hallEntity,
						countryEntity,
						hallWorkAreaEntities,
						hallWorkStationEntities,
						departmentEntities);

					response.Add(hall);
				}

				return ResponseModel<List<Models.WorkLocation.HallModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.WorkLocation.HallModel>> Validate()
		{
			throw new NotImplementedException();
		}
	}
}
