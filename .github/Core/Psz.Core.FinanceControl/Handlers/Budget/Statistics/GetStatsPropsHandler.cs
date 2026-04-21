using Psz.Core.FinanceControl.Models.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class GetStatsPropsHandler: IHandle<UserModel, ResponseModel<StatsPropsModel>>
	{
		private readonly UserModel _user;
		public GetStatsPropsHandler(UserModel user)
		{
			_user = user;
		}
		public ResponseModel<StatsPropsModel> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;

			try
			{
				var landsEntities = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get();
				var deptsEntities = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get();
				var employeesEntities = Infrastructure.Data.Access.Tables.COR.UserAccess.Get();

				//filtering
				var lands = landsEntities?.Select(x => new LandsModel(x)).ToList();
				var depts = deptsEntities?.Select(x => new DepartementsModel(x)).ToList();
				var employees = employeesEntities?.Select(x => new EmplyoeesModel(x)).ToList();
				var isSiteDirector = false;
				var IsHeadOfDepartement = false;
				//user have view all access
				if((_user.Access.Financial.Budget?.StatisticsViewAll ?? false) || _user.IsCorporateDirector || _user.IsCorporateDirector || _user.SuperAdministrator)
				{
					return ResponseModel<StatsPropsModel>.SuccessResponse(new StatsPropsModel
					{
						Departements = depts,
						Employees = employees,
						Lands = lands,
						IsHeadOfDepartement = true,
						IsSiteDirector = true,
					});
				}
				else
				{
					var directedDepts = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByDirectorId(_user.Id);
					var directedSites = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetByDirectorId(new List<int> { _user.Id });
					var siteIds = new List<int>();
					var deptIds = new List<int>();
					var empIds = new List<int>();

					//check if user id site(s) director
					if(directedSites != null && directedSites.Count > 0)
					{
						isSiteDirector = true;
						var landsIds = directedSites.Select(x => x.Id).Distinct().ToList();
						lands = directedSites?.Select(x => new LandsModel(x)).ToList();
						var _depts = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByCompanies(directedSites.Select(x => (long)x.Id).ToList());
						var deptsIds = _depts?.Select(x => (int)x.Id).Distinct().ToList();
						depts = _depts?.Select(x => new DepartementsModel(x)).ToList();
						var _employees = employees.Where(x => landsIds.Contains(x.LandId) || deptsIds.Contains(x.DepartementId)).ToList();
						// -
						siteIds.AddRange(directedSites.Select(x => x.Id));
						deptIds.AddRange(deptsIds);
						empIds.AddRange(_employees.Select(x => x.Id));
					}
					//check if user is dept(s) head of
					if(directedDepts != null && directedDepts.Count > 0)
					{
						IsHeadOfDepartement = true;
						var deptsIds = directedDepts.Select(x => (int)x.Id).Distinct().ToList();
						var _deptsLandsIds = directedDepts.Select(x => x.CompanyId).Distinct().ToList();
						var _lands = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(_deptsLandsIds);
						var landsIds = _lands.Select(x => x.Id).Distinct().ToList();

						depts = depts.Where(x => deptsIds.Contains(x.Id)).ToList();
						lands = lands.Where(x => landsIds.Contains(x.Id)).ToList();
						var _employees = employees.Where(x => landsIds.Contains(x.LandId) || deptsIds.Contains(x.DepartementId)).ToList();
						// -
						siteIds.AddRange(landsIds);
						deptIds.AddRange(deptsIds);
						empIds.AddRange(_employees.Select(x => x.Id));
					}

					// - user props
					var land = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(_user.CompanyId);
					var dept = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(_user.DepartmentId);
					var employee = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(_user.Id);
					depts = new List<DepartementsModel> { new DepartementsModel(dept) };
					lands = new List<LandsModel> { new LandsModel(land) };
					employees = new List<EmplyoeesModel> { new EmplyoeesModel(employee) };
					// -
					siteIds.Add(land.Id);
					deptIds.Add((int)dept.Id);
					empIds.Add(_user.Id);

					// -
					siteIds = siteIds.Distinct()?.ToList();
					deptIds = deptIds.Distinct()?.ToList();
					empIds = empIds.Distinct()?.ToList();
					return ResponseModel<StatsPropsModel>.SuccessResponse(new StatsPropsModel
					{
						Departements = deptsEntities.Where(x=> deptIds.Exists(y=> y==x.Id))?.Select(x=> new DepartementsModel(x))?.ToList(),
						Employees = employeesEntities.Where(x => empIds.Exists(y => y == x.Id))?.Select(x => new EmplyoeesModel(x))?.ToList(),
						Lands = landsEntities.Where(x => siteIds.Exists(y => y == x.Id))?.Select(x => new LandsModel(x))?.ToList(),
						IsHeadOfDepartement = IsHeadOfDepartement,
						IsSiteDirector = isSiteDirector
					});
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<StatsPropsModel> Validate()
		{
			if(_user == null)
				return ResponseModel<StatsPropsModel>.AccessDeniedResponse();
			return ResponseModel<StatsPropsModel>.SuccessResponse();
		}
	}
}