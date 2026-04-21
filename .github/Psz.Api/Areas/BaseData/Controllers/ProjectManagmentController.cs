using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.BaseData.Interfaces;
using Psz.Core.BaseData.Models.ProjectManagment;
using Psz.Core.Common.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

namespace Psz.Api.Areas.BaseData.Controllers
{

	[Authorize]
	[Area("BaseData")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class ProjectManagmentController: ControllerBase
	{
		private const string MODULE = "BaseData";
		private readonly IProjectManagmentService _projectManagmentService;

		public ProjectManagmentController(IProjectManagmentService projectManagmentService)
		{
			_projectManagmentService = projectManagmentService;
		}
		#region Projects
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<ProjectModel>>), 200)]
		public IActionResult GetProjectsProjectLevel(int customerNumber)
		{
			try
			{
				var response = _projectManagmentService.GetProjectsProjectLevel(this.GetCurrentUser(), customerNumber);
				return Ok(response);
			} catch(System.Exception e)
			{

				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<ProjectCableModel>>), 200)]
		public IActionResult GetProjectsCablesById(int id)
		{
			try
			{
				var response = _projectManagmentService.GetProjectCablesById(this.GetCurrentUser(), id);
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<ProjectCableTasksMinimalModel>>), 200)]
		public IActionResult GetProjectsTasksById(int id)
		{
			try
			{
				var response = _projectManagmentService.GetPorjectTasksById(this.GetCurrentUser(), id);
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<ProjectCableTasksMinimalModel>>), 200)]
		public IActionResult GetProjectsTasksByCableId(int id)
		{
			try
			{
				var response = _projectManagmentService.GetPorjectTasksByCableId(this.GetCurrentUser(), id);
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<ProjectsMinimalModel>>), 200)]
		public IActionResult GetProjectsCustomerLevel()
		{
			try
			{
				var response = _projectManagmentService.GetProjectsCustomerLevel(this.GetCurrentUser());
				return Ok(response);
			} catch(System.Exception e)
			{

				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<ProjectCableTasksModel>>), 200)]
		public IActionResult GetProjectsCableLevel(int projectId)
		{
			try
			{
				var response = _projectManagmentService.GetProjectsCableLevel(this.GetCurrentUser(), projectId);
				return Ok(response);
			} catch(System.Exception e)
			{

				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult AddProject(ProjectAddRequestModel data)
		{
			try
			{
				var response = _projectManagmentService.AddProject(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(System.Exception e)
			{

				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult EditProject(ProjectHeader data)
		{
			try
			{
				var response = _projectManagmentService.EditProject(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(System.Exception e)
			{

				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteProject(int id)
		{
			try
			{
				var response = _projectManagmentService.DeleteProject(this.GetCurrentUser(), id);
				return Ok(response);
			} catch(System.Exception e)
			{

				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult GetProjectById(int id)
		{
			try
			{
				var response = _projectManagmentService.GetProjectById(this.GetCurrentUser(), id);
				return Ok(response);
			} catch(System.Exception e)
			{

				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult AddProjectCableTask(CableTaskAddRequestModel data)
		{
			try
			{
				var response = _projectManagmentService.AddProjectCableCurrentTask(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(System.Exception e)
			{

				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult EditProjectCableTask(CableTaskAddRequestModel data)
		{
			try
			{
				var response = _projectManagmentService.EditProjectCableCurrentTask(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(System.Exception e)
			{

				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteProjectCableTask(int id)
		{
			try
			{
				var response = _projectManagmentService.DeleteProjectCableCurrentTask(this.GetCurrentUser(), id);
				return Ok(response);
			} catch(System.Exception e)
			{

				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult AddProjectCable(ProjectCable data)
		{
			try
			{
				var response = _projectManagmentService.AddProjectCable(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(System.Exception e)
			{

				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteProjectCable(int id)
		{
			try
			{
				var response = _projectManagmentService.DeleteCable(this.GetCurrentUser(), id);
				return Ok(response);
			} catch(System.Exception e)
			{

				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<OpenOrdersModel>>), 200)]
		public IActionResult GetOpenOrders(int articleNr)
		{
			try
			{
				var response = _projectManagmentService.GetOpenOrders(this.GetCurrentUser(), articleNr);
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		#endregion
		#region MileStones
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult AddMileStone(MileStoneModel data)
		{
			try
			{
				var response = _projectManagmentService.AddMileStone(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(System.Exception e)
			{

				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateMileStone(MileStoneModel data)
		{
			try
			{
				var response = _projectManagmentService.UpdateMileStone(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(System.Exception e)
			{

				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteMileStone(int id)
		{
			try
			{
				var response = _projectManagmentService.DeteleMileStone(this.GetCurrentUser(), id);
				return Ok(response);
			} catch(System.Exception e)
			{

				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<MileStoneModel>>), 200)]
		public IActionResult GetMileStone()
		{
			try
			{
				var response = _projectManagmentService.GetMileStones(this.GetCurrentUser());
				return Ok(response);
			} catch(System.Exception e)
			{

				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(type: typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult AffectTasksToMilestone(AffectMileStoneModel data)
		{
			try
			{
				var response = _projectManagmentService.AffectTasksToMilestone(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(System.Exception e)
			{

				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(type: typeof(Core.Models.ResponseModel<List<MileStoneTasks>>), 200)]
		public IActionResult GetMilestoneTasks(int id)
		{
			try
			{
				var response = _projectManagmentService.GetMilestoneTasks(this.GetCurrentUser(), id);
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(type: typeof(Core.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetTasksForMilestone(int id)
		{
			try
			{
				var response = _projectManagmentService.GetTasksForMilestone(this.GetCurrentUser(), id);
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(type: typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult RemoveTaskFromMilestone(int id)
		{
			try
			{
				var response = _projectManagmentService.RemoveTaskFromMilestone(this.GetCurrentUser(), id);
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		#endregion
		#region Selects
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult SearchProjectManagers(string searchText)
		{
			try
			{
				var response = _projectManagmentService.SearchManager(this.GetCurrentUser(), searchText);
				return Ok(response);
			} catch(System.Exception e)
			{

				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult SearchCables(string searchText)
		{
			try
			{
				var response = _projectManagmentService.SearchCable(this.GetCurrentUser(), searchText);
				return Ok(response);
			} catch(System.Exception e)
			{

				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetProjectsTypes()
		{
			try
			{
				var response = _projectManagmentService.GetProjectTypes(this.GetCurrentUser());
				return Ok(response);
			} catch(System.Exception e)
			{

				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetProjectsFactories()
		{
			try
			{
				var response = _projectManagmentService.GetProjectFactories(this.GetCurrentUser());
				return Ok(response);
			} catch(System.Exception e)
			{

				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetArticlesByCustomer(GetArticlesByCustomerRequestModel data)
		{
			try
			{
				var response = _projectManagmentService.GetArticlesByCustomer(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(System.Exception e)
			{

				return this.HandleException(e);
			}
		}
		#endregion
		#region Stats
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<StatsModel>), 200)]
		public IActionResult GetProjectsStats()
		{
			try
			{
				var response = _projectManagmentService.GetProjectsStats(this.GetCurrentUser());
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<ProjectsOverviewModel>>), 200)]
		public IActionResult GetProjectsOverviewModelByStatus(string status)
		{
			try
			{
				var response = _projectManagmentService.GetProjectsOverviewByStatus(this.GetCurrentUser(), status);
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<ProjectsOverviewModel>>), 200)]
		public IActionResult GetProjectsOverviewModelByTime(string time)
		{
			try
			{
				var response = _projectManagmentService.GetProjectsOverviewByTime(this.GetCurrentUser(), time);
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<TasksOverviewModel>>), 200)]
		public IActionResult GetTasksOverviewByStatus(string status)
		{
			try
			{
				var response = _projectManagmentService.GetTasksByStatusOverview(this.GetCurrentUser(), status);
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<ProjectFullModel>>), 200)]
		public IActionResult GetProjectsForGantt()
		{
			try
			{
				var response = _projectManagmentService.GetProjectsForGantt(this.GetCurrentUser());
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<CustomerModel>>), 200)]
		public IActionResult GetCustomers(string searchText)
		{
			try
			{
				var response = _projectManagmentService.getCustomers(this.GetCurrentUser(), searchText);
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		#endregion
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<ProjectLogsModel>>), 200)]
		public IActionResult GetProjectsLogs()
		{
			try
			{
				var response = _projectManagmentService.GetProjectsLogs(this.GetCurrentUser());
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<ProjectLogsModel>>), 200)]
		public IActionResult GetProjectsLogsByProject(int projectId)
		{
			try
			{
				var response = _projectManagmentService.GetProjectsLogsByProject(this.GetCurrentUser(), projectId);
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}