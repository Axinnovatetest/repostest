using Infrastructure.Data.Access.Joins.CTS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.CronJobs
{
	public class Jobs
	{
		public static void ArticleCustomsNumberChecksLastExec()
		{
			var lastExec = Infrastructure.Data.Access.Tables.LGT.ArticleCustomsNumberCheckAccess.GetLastInsertedRecord();
			if(lastExec != null && lastExec.CheckDate.HasValue && lastExec.CheckDate.Value.AddDays(Module.LGT?.ArticleCustomsMaxRenewalDays ?? 30) < DateTime.Today)
			{
				var title = $"[REM] Zolltarifnummer Actualization";
				var content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
				+ $"<span style='font-size:1.15em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>"
				+ $"<br/><span style='font-size:1em;'>This is a reminder to check the <strong>article customs numbers (Zolltarifnummern)</strong> in the system."
				+ $"</span><br/><span>The last check was done  {Math.Floor((DateTime.Now - lastExec.CheckDate.Value).TotalDays)} days ago.</span><br/>"
				+ "</div>";
				content += "<br/>";
				content += $"<br/><span style='font-size:1.em;font-weight:bold'>Regards,</span>";
				content += $"<br/><span style='font-size:1.em;font-weight:bold'>IT Department </span></br>";

				try
				{
					var addresses = new List<string>();
					var globalDirs = Infrastructure.Data.Access.Tables.COR.UserAccess.GetGlobalDirectors();
					if(globalDirs?.Count > 0)
					{
						addresses.AddRange(globalDirs.Select(x => x.Email));
					}
					if(!string.IsNullOrWhiteSpace(Module.EmailingService.EmailParamtersModel.AdminEmail))
					{
						addresses.Add(Module.EmailingService.EmailParamtersModel.AdminEmail);
					}
					if(Module.LGT.ArticleCustomsMaxRenewalDaysEmails?.Count > 0)
					{
						addresses.AddRange(Module.LGT.ArticleCustomsMaxRenewalDaysEmails);
					}

					Module.EmailingService.SendEmailAsync(title, content, addresses, null);
				} catch(Exception ex)
				{
					Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email NotifyExpiredNearlyRahmens"));
					Infrastructure.Services.Logging.Logger.Log(ex);
				}
			}
		}
	}
}
