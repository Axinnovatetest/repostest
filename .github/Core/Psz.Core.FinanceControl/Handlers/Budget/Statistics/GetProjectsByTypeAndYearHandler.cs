using Psz.Core.FinanceControl.Models.Statistics;
using System.Collections.Generic;
using System.Linq;


namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class GetProjectsByTypeAndYearHandler: IHandle<UserModel, ResponseModel<List<ProjectsOverviewModel>>>
	{
		private readonly UserModel _user;
		private readonly StatsRequestModelType _data;

		public GetProjectsByTypeAndYearHandler(UserModel user, StatsRequestModelType data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<List<ProjectsOverviewModel>> Handle()
		{
			try
			{
				var validationResponse = Validate();
				if(!validationResponse.Success)
					return validationResponse;

				var prjectOverviewEntities = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetProjectsOverviewByType(_data.Type, _data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);
				var response = prjectOverviewEntities?.Select(x => new ProjectsOverviewModel(x)).ToList();

				return ResponseModel<List<ProjectsOverviewModel>>.SuccessResponse(response);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<ProjectsOverviewModel>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<ProjectsOverviewModel>>.AccessDeniedResponse();
			}
			return ResponseModel<List<ProjectsOverviewModel>>.SuccessResponse();
		}
	}
}