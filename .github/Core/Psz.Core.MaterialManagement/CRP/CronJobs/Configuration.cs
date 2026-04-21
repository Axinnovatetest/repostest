namespace Psz.Core.MaterialManagement.CRP.CronJobs
{
	using Infrastructure.Data.Access.Tables.MTM;
	using Infrastructure.Data.Access.Tables.WPL;
	using Infrastructure.Data.Entities.Tables.MTM;
	using System.Linq;

	public static class Configuration
	{
		public static void Main()
		{
			int newDepts = ConfigurationAddNewDepartments();
			int updDepts = ConfigurationUpdateDepartments();
			int delDepts = ConfigurationClearDepartments();

			var emailContent = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
							+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>";
			#region >>>> Summary Notification <<<<
			try
			{
				emailContent += $"<br/>{DateTime.Now.ToString("dd.MM.yyyy HH.mm.ss")} | {newDepts} new departments <span style='color:green;'>added</span>.";
				emailContent += $"<br/>{DateTime.Now.ToString("dd.MM.yyyy HH.mm.ss")} | {updDepts} departments <span style='color:orange;'>updated</span>.";
				emailContent += $"<br/>{DateTime.Now.ToString("dd.MM.yyyy HH.mm.ss")} | {delDepts} departments <span style='color:red;'>deleted</span>.";

				// Send email notification
				Module.EmailingService.SendEmailAsync($"Department checks", emailContent, new List<string> { Module.EmailingService.EmailParamtersModel.AdminEmail }, null,
				saveHistory: true, senderEmail: "", senderName: "", senderId: 0, senderCC: false, attachmentIds: null);

				// - 
				var alertMessageBody = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
										+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>";
				if(newDepts > 0)
					alertMessageBody += $"<br/>{DateTime.Now.ToString("dd.MM.yyyy HH.mm.ss")} | {newDepts} new departments <strong style='color:black;'>added</strong>.";

				if(updDepts > 0)
					alertMessageBody += $"<br/>{DateTime.Now.ToString("dd.MM.yyyy HH.mm.ss")} | {updDepts} departments <strong style='color:black;'>updated</strong>.";

				if(delDepts > 0)
					alertMessageBody += $"<br/>{DateTime.Now.ToString("dd.MM.yyyy HH.mm.ss")} | {delDepts} departments <strong style='color:black;'>deleted</strong>.";

				if(Module.EmailingService.EmailParamtersModel.CRPEmailDestinations != null && Module.EmailingService.EmailParamtersModel.CRPEmailDestinations.Count > 0 &&
					(newDepts > 0 || updDepts > 0 || delDepts > 0))
				{
					try
					{
						Module.EmailingService.SendEmailAsync("Title", alertMessageBody, Module.EmailingService.EmailParamtersModel.CRPEmailDestinations);
					} catch(Exception e)
					{
						Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{string.Join(",", Module.EmailingService.EmailParamtersModel.CRPEmailDestinations)}]"));
						Infrastructure.Services.Logging.Logger.Log(e);
					}
				}
			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{Module.EmailingService.EmailParamtersModel.AdminEmail}]"));
				Infrastructure.Services.Logging.Logger.Log(ex);
			}
			#endregion Summary Notification
		}
		static int ConfigurationAddNewDepartments()
		{
			int results = 0;
			var configHeaders = ConfigurationHeaderAccess.Get() ?? new List<ConfigurationHeaderEntity>();
			var configDetailsEntities = ConfigurationDetailsAccess.GetByHeaders(configHeaders?.Select(x => x.Id)?.ToList());
			if(configDetailsEntities == null || configDetailsEntities.Count <= 0)
				return results;

			var departmentEntities = DepartmentAccess.Get();
			if(departmentEntities == null || departmentEntities.Count <= 0)
				return results;

			var depatmentsInConfig = configDetailsEntities.Select(x => x.DepartmentId)?.ToList() ?? new List<int>();
			var newDepartments = departmentEntities.Where(x => !depatmentsInConfig.Exists(y => y == x.Id))?.ToList();
			if(newDepartments == null || newDepartments.Count <= 0)
				return results;

			results = newDepartments.Count;
			foreach(var configHeader in configHeaders)
			{
				// - insert in 2 weeks config
				ConfigurationDetailsAccess.Insert(newDepartments.Select(x => new ConfigurationDetailsEntity
				{
					Id = -1,
					CreationTime = DateTime.Now,
					CreationUserId = -2,
					DepartmentId = x.Id,
					DepartmentName = x.Name,
					DepartmentWeekNumber = 2,
					HeaderId = configHeader.Id,
					IsLowerThanThreshold = true
				})?.ToList());

				// - insert in 3 weeks config
				ConfigurationDetailsAccess.Insert(newDepartments.Select(x => new ConfigurationDetailsEntity
				{
					Id = -1,
					CreationTime = DateTime.Now,
					CreationUserId = -2,
					DepartmentId = x.Id,
					DepartmentName = x.Name,
					DepartmentWeekNumber = 3,
					HeaderId = configHeader.Id,
					IsLowerThanThreshold = false,
				})?.ToList());
			}

			return results;
		}
		static int ConfigurationUpdateDepartments()
		{
			int results = 0;
			var configHeaders = ConfigurationHeaderAccess.Get() ?? new List<ConfigurationHeaderEntity>();
			var configDetailsEntities = ConfigurationDetailsAccess.GetByHeaders(configHeaders?.Select(x => x.Id)?.ToList());
			if(configDetailsEntities == null || configDetailsEntities.Count <= 0)
				return results;

			var departmentEntities = DepartmentAccess.Get();
			foreach(var configItem in configDetailsEntities)
			{
				var dept = departmentEntities.Find(x => x.Id == configItem.DepartmentId);
				if(dept == null)
					continue;

				if(dept.Name.Trim() != configItem.DepartmentName.Trim())
				{
					ConfigurationDetailsAccess.UpdateDepartmentName(configItem.Id, dept.Name);
					results++;
				}
			}

			return results;
		}
		static int ConfigurationClearDepartments()
		{
			int results = 0;
			var configHeaders = ConfigurationHeaderAccess.Get() ?? new List<ConfigurationHeaderEntity>();
			var configDetailsEntities = ConfigurationDetailsAccess.GetByHeaders(configHeaders?.Select(x => x.Id)?.ToList());
			if(configDetailsEntities == null || configDetailsEntities.Count <= 0)
				return results;

			var departmentEntities = DepartmentAccess.Get();
			foreach(var configItem in configDetailsEntities)
			{
				var dept = departmentEntities.Find(x => x.Id == configItem.DepartmentId);
				if(dept == null)
				{
					ConfigurationDetailsAccess.Delete(configItem.Id);
					results++;
				}
			}

			return results;
		}
	}
}
