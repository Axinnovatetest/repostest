using Psz.Core.BaseData.Models.ProjectManagment;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.ProjectManagment
{
	public partial class ProjectManagmentService
	{
		public ResponseModel<int> AddMileStone(UserModel user, MileStoneModel data)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();

			try
			{
				var entity = data.ToEntity();
				entity.CreationTime = DateTime.Now;
				entity.CreationUserId = user.Id;
				var response = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_MileStonesAccess.Insert(entity);
				Infrastructure.Data.Access.Tables.BSD.__bsd_pm_logsAccess.Insert(new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_logsEntity
				{
					LogText = $"Milestone [#{response}] added",
					LogTime = DateTime.Now,
					ProjectId = null,
					UserId = user.Id,
					Username = user.Name,
				});

				return ResponseModel<int>.SuccessResponse(response);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> UpdateMileStone(UserModel user, MileStoneModel data)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();
			try
			{
				var old_entity = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_MileStonesAccess.Get(data.Id);
				var entity = data.ToEntity();
				entity.CreationTime = old_entity.CreationTime;
				entity.CreationUserId = old_entity.CreationUserId;
				var logs = new List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_logsEntity>();
				if(old_entity.StartDate != entity.StartDate)
					logs.Add(new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_logsEntity
					{
						LogText = $"Milestone [#{entity.Id}] StartDate changed from [{old_entity.StartDate}] to [{entity.StartDate}]",
						LogTime = DateTime.Now,
						ProjectId = null,
						UserId = user.Id,
						Username = user.Name,
					});
				if(old_entity.EndDate != entity.EndDate)
					logs.Add(new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_logsEntity
					{
						LogText = $"Milestone [#{entity.Id}] EndDate changed from [{old_entity.EndDate}] to [{entity.EndDate}]",
						LogTime = DateTime.Now,
						ProjectId = null,
						UserId = user.Id,
						Username = user.Name,
					});
				if(old_entity.Comment != entity.Comment)
					logs.Add(new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_logsEntity
					{
						LogText = $"Milestone [#{entity.Id}] Comment changed from [{old_entity.Comment}] to [{entity.Comment}]",
						LogTime = DateTime.Now,
						ProjectId = null,
						UserId = user.Id,
						Username = user.Name,
					});
				var response = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_MileStonesAccess.Update(entity);
				if(logs != null && logs.Count > 0)
					Infrastructure.Data.Access.Tables.BSD.__bsd_pm_logsAccess.Insert(logs);

				return ResponseModel<int>.SuccessResponse(response);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> DeteleMileStone(UserModel user, int id)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();
			try
			{
				var response = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_MileStonesAccess.Delete(id);
				Infrastructure.Data.Access.Tables.BSD.__bsd_pm_logsAccess.Insert(new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_logsEntity
				{
					LogText = $"Milestone [#{id}] deleted",
					LogTime = DateTime.Now,
					ProjectId = null,
					UserId = user.Id,
					Username = user.Name,
				});

				return ResponseModel<int>.SuccessResponse(response);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<MileStoneModel>> GetMileStones(UserModel user)
		{
			if(user == null)
				return ResponseModel<List<MileStoneModel>>.AccessDeniedResponse();

			try
			{
				var entities = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_MileStonesAccess.Get();
				var response = entities?.Select(x => new MileStoneModel(x)).ToList();

				return ResponseModel<List<MileStoneModel>>.SuccessResponse(response);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> AffectTasksToMilestone(UserModel user, AffectMileStoneModel data)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();

			try
			{
				var milestone = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_MileStonesAccess.Get(data.mileStoneId);
				var errors = new List<string>();
				data.TasksIds?.ForEach(x =>
				{
					var task = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_CurrentTaskAccess.Get(x);
					if(task.StartDate < milestone.StartDate || task.Deadline > milestone.EndDate)
						errors.Add($"Task [{task.CurrentTaskName}] date is out of the milestone date range.");
				});
				if(errors != null && errors.Count > 0)
					return ResponseModel<int>.FailureResponse(errors);

				var entities = data.TasksIds.Select(x => new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_TasksMileStonesEntity
				{
					IdTask = x,
					TaskName = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_CurrentTaskAccess.Get(x)?.CurrentTaskName,
					IdMileStone = milestone.Id
				}).ToList();
				var response = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_TasksMileStonesAccess.Insert(entities);
				Infrastructure.Data.Access.Tables.BSD.__bsd_pm_logsAccess.Insert(new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_logsEntity
				{
					LogText = $"Tasks [{string.Join(",", data.TasksIds.Select(t => $"#{t}"))}] affected to Milestone [#{data.mileStoneId}]",
					LogTime = DateTime.Now,
					ProjectId = null,
					UserId = user.Id,
					Username = user.Name,
				});

				return ResponseModel<int>.SuccessResponse(response);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<MileStoneTasks>> GetMilestoneTasks(UserModel user, int id)
		{
			if(user == null)
				return ResponseModel<List<MileStoneTasks>>.AccessDeniedResponse();

			var entities = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_TasksMileStonesAccess.GetByMilestone(id);
			var response = entities?.Select(x => new MileStoneTasks
			{
				Id = x.Id,
				TaskId = x.IdTask ?? -1,
				TaskName = x.TaskName,
			}).ToList();
			return ResponseModel<List<MileStoneTasks>>.SuccessResponse(response);
		}
		public ResponseModel<List<KeyValuePair<int, string>>> GetTasksForMilestone(UserModel user, int milestoneId)
		{
			if(user == null)
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();

			var milestone = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_MileStonesAccess.Get(milestoneId);
			var tasks = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_CurrentTaskAccess.Get();
			var mileStonetasks = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_TasksMileStonesAccess.GetByMilestone(milestoneId);

			var tasksIds = mileStonetasks?.Select(x => x.IdTask).Distinct().ToList();
			if(tasksIds != null && tasksIds.Count > 0)
				tasks = tasks?.Where(x => !tasksIds.Contains(x.Id)).ToList();
			tasks = tasks?.Where(x => x.StartDate >= milestone.StartDate && x.Deadline <= milestone.EndDate).ToList();
			var response = tasks?.Select(x => new KeyValuePair<int, string>(x.Id, x.CurrentTaskName)).ToList();
			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(response);

		}
		public ResponseModel<int> RemoveTaskFromMilestone(UserModel user, int Id)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();
			var mileStoneTask = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_TasksMileStonesAccess.Get(Id);
			var response = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_TasksMileStonesAccess.Delete(Id);
			Infrastructure.Data.Access.Tables.BSD.__bsd_pm_logsAccess.Insert(new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_logsEntity
			{
				LogText = $"Task [#{mileStoneTask.IdTask}] removed from Milestone [#{mileStoneTask.IdMileStone}]",
				LogTime = DateTime.Now,
				ProjectId = null,
				UserId = user.Id,
				Username = user.Name,
			});
			return ResponseModel<int>.SuccessResponse();
		}
	}
}