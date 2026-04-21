using Psz.Core.BaseData.Models.ProjectManagment;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Interfaces
{
	public interface IProjectManagmentService
	{
		ResponseModel<List<ProjectModel>> GetProjectsProjectLevel(UserModel user, int customerNumber);
		ResponseModel<List<ProjectCableModel>> GetProjectCablesById(UserModel user, int id);
		ResponseModel<List<ProjectCableTasksMinimalModel>> GetPorjectTasksById(UserModel user, int id);
		ResponseModel<List<ProjectCableTasksMinimalModel>> GetPorjectTasksByCableId(UserModel user, int id);
		ResponseModel<List<ProjectsMinimalModel>> GetProjectsCustomerLevel(UserModel user);
		ResponseModel<List<ProjectCableTasksModel>> GetProjectsCableLevel(UserModel user, int projectId);
		//
		ResponseModel<string> AddProject(UserModel user, ProjectAddRequestModel data);
		ResponseModel<int> EditProject(UserModel user, ProjectHeader data);
		ResponseModel<int> DeleteProject(UserModel user, int id);
		ResponseModel<int> AddProjectCableCurrentTask(UserModel user, CableTaskAddRequestModel data);
		ResponseModel<int> EditProjectCableCurrentTask(UserModel user, CableTaskAddRequestModel data);
		ResponseModel<int> DeleteProjectCableCurrentTask(UserModel user, int id);
		ResponseModel<int> AddProjectCable(UserModel user, ProjectCable data);
		ResponseModel<int> DeleteCable(UserModel user, int id);
		ResponseModel<ProjectHeader> GetProjectById(UserModel user, int id);
		//
		ResponseModel<List<MileStoneModel>> GetMileStones(UserModel user);
		ResponseModel<int> AddMileStone(UserModel user, MileStoneModel data);
		ResponseModel<int> UpdateMileStone(UserModel user, MileStoneModel data);
		ResponseModel<int> DeteleMileStone(UserModel user, int id);
		ResponseModel<int> AffectTasksToMilestone(UserModel user, AffectMileStoneModel data);
		ResponseModel<int> RemoveTaskFromMilestone(UserModel user, int Id);
		ResponseModel<List<MileStoneTasks>> GetMilestoneTasks(UserModel user, int id);
		ResponseModel<List<KeyValuePair<int, string>>> GetTasksForMilestone(UserModel user, int milestoneId);
		//
		ResponseModel<List<KeyValuePair<int, string>>> SearchManager(UserModel user, string searchtext);
		ResponseModel<List<KeyValuePair<int, string>>> SearchCable(UserModel user, string searchtext);
		//
		ResponseModel<List<KeyValuePair<int, string>>> GetProjectTypes(UserModel user);
		ResponseModel<List<KeyValuePair<int, string>>> GetProjectFactories(UserModel user);
		//
		ResponseModel<StatsModel> GetProjectsStats(UserModel user);
		ResponseModel<List<ProjectsOverviewModel>> GetProjectsOverviewByStatus(UserModel user, string status);
		ResponseModel<List<ProjectsOverviewModel>> GetProjectsOverviewByTime(UserModel user, string time);
		ResponseModel<List<TasksOverviewModel>> GetTasksByStatusOverview(UserModel user, string status);
		//
		ResponseModel<List<ProjectGANTTModel>> GetProjectsForGantt(UserModel user);
		ResponseModel<List<KeyValuePair<int, string>>> GetArticlesByCustomer(UserModel user, GetArticlesByCustomerRequestModel data);
		ResponseModel<List<CustomerModel>> getCustomers(UserModel user, string searchText);

		ResponseModel<List<OpenOrdersModel>> GetOpenOrders(UserModel user, int articleNr);
		ResponseModel<List<ProjectLogsModel>> GetProjectsLogs(UserModel user);
		ResponseModel<List<ProjectLogsModel>> GetProjectsLogsByProject(UserModel user, int projectId);
	}
}