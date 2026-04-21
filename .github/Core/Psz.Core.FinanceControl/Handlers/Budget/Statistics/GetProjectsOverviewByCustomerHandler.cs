using Psz.Core.FinanceControl.Models.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class GetProjectsOverviewByCustomerHandler: IHandle<UserModel, ResponseModel<List<ProjectsOverviewModel>>>
	{
		private readonly UserModel _user;
		private readonly StatsRequestBestSuppliersOverviewOrdersCount _data;

		public GetProjectsOverviewByCustomerHandler(UserModel user, StatsRequestBestSuppliersOverviewOrdersCount data)
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

				var entities = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetProjectsOverviewByCustomer(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId, _data.Supplier);
				return ResponseModel<List<ProjectsOverviewModel>>.SuccessResponse(entities?.Select(x => new ProjectsOverviewModel(x)).ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<ProjectsOverviewModel>> Validate()
		{
			if(_user == null)
				return ResponseModel<List<ProjectsOverviewModel>>.AccessDeniedResponse();
			return ResponseModel<List<ProjectsOverviewModel>>.SuccessResponse();
		}
	}
}