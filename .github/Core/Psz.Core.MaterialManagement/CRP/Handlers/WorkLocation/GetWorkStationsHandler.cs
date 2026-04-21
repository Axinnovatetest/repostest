using Infrastructure.Data.Access.Tables.MTM;
using Infrastructure.Data.Access.Tables.WPL;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.WorkLocation
{
	public class GetWorkStationsHandler: IHandle<Psz.Core.Identity.Models.UserModel, ResponseModel<List<Models.WorkLocation.WorkStationModel>>>
	{
		private Psz.Core.Identity.Models.UserModel user { get; set; }

		public GetWorkStationsHandler(Psz.Core.Identity.Models.UserModel user)
		{
			this.user = user;
		}

		public ResponseModel<List<Models.WorkLocation.WorkStationModel>> Handle()
		{
			try
			{
				if(user == null)
				{
					throw new Psz.Core.SharedKernel.Exceptions.UnauthorizedException();
				}

				var authorizedWorkStationEntities = UserWorkStationAuthorizationAccess.Get(); // - >>> This is defined in WPL >>> UserWorkStationAuthorizationAccess.GetByUserId(this.user.Id);

				var workStationIds = authorizedWorkStationEntities.Select(e => e.WorkStationId).ToList();
				var workStationEntities = WorkStationMachineAccess.Get(/*workStationIds*/)
					.FindAll(e => !e.IsArchived);

				var countryEntities = CountryAccess.Get().FindAll(e => !e.IsArchived);
				var hallEntities = HallAccess.Get().FindAll(e => !e.IsArchived);

				var response = new List<Models.WorkLocation.WorkStationModel>();

				foreach(var workStationEntity in workStationEntities)
				{
					var countryEntity = countryEntities.Find(e => e.Id == workStationEntity.CountryId);
					var hallEntity = hallEntities.Find(e => e.Id == workStationEntity.HallId);

					var workStation = new Models.WorkLocation.WorkStationModel(workStationEntity, countryEntity, hallEntity);

					response.Add(workStation);
				}

				return ResponseModel<List<Models.WorkLocation.WorkStationModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.WorkLocation.WorkStationModel>> Validate()
		{
			throw new NotImplementedException();
		}
	}
}
