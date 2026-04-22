using iText.Layout.Element;
using Psz.Core.BaseData.Models.ProjectManagment;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.ProjectManagment
{
	public partial class ProjectManagmentService
	{
		public ResponseModel<int> AddProjectCableCurrentTask(UserModel user, CableTaskAddRequestModel data)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();

			try
			{
				var entity = new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CurrentTaskEntity
				{
					CreationDate = System.DateTime.Now,
					CreationUserId = user.Id,
					CreationUsername = user.Name,
					CurrentTaskName = data.TaskName,
					Deadline = data.Deadline,
					ProjectId = data.ProjectId,
					StartDate = DateTime.Now,
					Status = data.Status,
					StatusId = data.StatusId,
					Comment = data.Comment
				};
				var response = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_CurrentTaskAccess.Insert(entity);
				Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_ProjectsEntity project = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_ProjectsAccess.Get(data.ProjectId);
				Infrastructure.Data.Access.Tables.BSD.__bsd_pm_logsAccess.Insert(new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_logsEntity
				{
					LogText = $"Task [{data.TaskName}] added",
					LogTime = DateTime.Now,
					ProjectId = data.ProjectId,
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
		public ResponseModel<int> EditProjectCableCurrentTask(UserModel user, CableTaskAddRequestModel data)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();

			var entity = data.ToEntity();
			var logEntities = GetEntityChange(entity);

			var response = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_CurrentTaskAccess.Update(entity);
			if(logEntities != null && logEntities.Count > 0)
				Infrastructure.Data.Access.Tables.BSD.__bsd_pm_logsAccess.Insert(logEntities);

			return ResponseModel<int>.SuccessResponse(response);
		}
		public ResponseModel<int> DeleteProjectCableCurrentTask(UserModel user, int id)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();

			try
			{
				var task = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_CurrentTaskAccess.Get(id);
				var response = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_CurrentTaskAccess.Delete(id);
				Infrastructure.Data.Access.Tables.BSD.__bsd_pm_logsAccess.Insert(new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_logsEntity
				{
					LogText = $"Task [{task.CurrentTaskName}] deleted",
					LogTime = DateTime.Now,
					ProjectId = task.ProjectId,
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

		private List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_logsEntity> GetEntityChange(Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CurrentTaskEntity _new)
		{
			var _old = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_CurrentTaskAccess.Get(_new.Id);
			var Logtexts = new List<string>();
			var Logentities = new List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_logsEntity>();

			if(_old.StartDate != _new.StartDate)
				Logtexts.Add($"StartDate changed from [{_old.StartDate}] to [{_new.StartDate}]");
			if(_old.Deadline != _new.Deadline)
				Logtexts.Add($"Deadline changed from [{_old.Deadline}] to [{_new.Deadline}]");
			if(_old.CurrentTaskName != _new.CurrentTaskName)
				Logtexts.Add($"TaskName changed from [{_old.CurrentTaskName}] to [{_new.CurrentTaskName}]");
			if(_old.Status != _new.Status)
				Logtexts.Add($"Status changed from [{_old.Status}] to [{_new.Status}]");

			if(Logtexts != null && Logtexts.Count > 0)
			{
				foreach(var logtext in Logtexts)
				{
					Logentities.Add(new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_logsEntity
					{
						LogText = logtext,
						LogTime = DateTime.Now,
						ProjectId = _new.ProjectId,
						UserId = _new.CreationUserId,
						Username = _new.CreationUsername,
					});
				}
			}
			return Logentities;
		}
	}
}