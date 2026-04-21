using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CapitalRequests.Helpers
{
	public class MailHelper
	{
		public static void SendNewRequestMail(int requestId)
		{
			var request = Infrastructure.Data.Access.Tables.CPL.Capital_requests_headerAccess.Get(requestId);
			var teams = Infrastructure.Data.Access.Tables.CPL.Capital_requests_teamsAccess.Get();
			var capitalTeam = teams?.Where(x => x.Team == "Cpl").ToList();
			var EngeneeringTeam = teams?.Where(x => x.Team == "Eng" && x.PlantId == request.PlantId).ToList();
			var ids = capitalTeam?.Select(c => c.UserId ?? -1).ToList().Union(EngeneeringTeam?.Select(c => c.UserId ?? -1).ToList());
			var users = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(ids.ToList());
			var adresses = users?.Select(u => u.Email).ToList();

			if(adresses != null && adresses.Count > 0)
			{
				var statusStyle = "";
				switch(request.StatusId)
				{
					case (int)Enums.RequestEnums.RequestStatus.Open:
						statusStyle = "Color:green";
						break;
					case (int)Enums.RequestEnums.RequestStatus.InProgress:
						statusStyle = "Color:orange";
						break;
					case (int)Enums.RequestEnums.RequestStatus.Closed:
						statusStyle = "Color:red";
						break;
					default:
						break;
				}
				var title = $"New Ticket (Incident)";
				var content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
				+ $"<span style='font-size:1.15em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>"
				+ $"<br/><span style='font-size:1em;'><strong>You have received a new incident report. </strong></span><br/>"
				+ $"<br/><span style='font-size:1em;'><strong>The details are as follows:</strong></span><br/>"
				+ $"<br/><ul>"
				+ $"<li>Ticket number: [#{request.Id}]</li>"
				+ $"<li>Production item: [{request.Artikelnummer}/{request.Fertigungsnummer}]</li>"
				+ $"<li>Current status: <strong style='{statusStyle}'>{request.Status}</strong></li>"
				+ $"<li>Description of the problem: <a href='https://designsync-pro.psz-soft.com/#/request/{requestId}'>Here</a></li>"
				+ $"</ul>"
				+ $"<br/><br/>"
				+ "</div>";
				content += $"<hr>";
				content += $"<br/><span style='font-size:1.em;font-weight:bold'>Regards,</span>";
				content += $"<br/><span style='font-size:1.em;font-weight:bold'>IT Department </span></br>";
				try
				{
					Module.EmailingService.SendEmailAsync(title, content, adresses.DistinctBy(x => x).ToList(), null);
				} catch(Exception ex)
				{
					Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email NotifyExpiredNearlyRahmens"));
					Infrastructure.Services.Logging.Logger.Log(ex);
				}
			}
		}
		public static void SendRequestCloseMail(int requestId)
		{
			var request = Infrastructure.Data.Access.Tables.CPL.Capital_requests_headerAccess.Get(requestId);
			var teams = Infrastructure.Data.Access.Tables.CPL.Capital_requests_teamsAccess.Get();
			var EngeneeringTeam = teams?.Where(x => x.Team == "Eng" && x.PlantId == request.PlantId).ToList();
			var ids = EngeneeringTeam?.Select(x => x.UserId ?? -1).ToList();
			var users = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(ids);
			var adresses = users?.Select(u => u.Email).ToList();
			var requestUser = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(request.UserId ?? -1);
			adresses.Add(requestUser?.Email);
			if(adresses is not null && adresses.Count > 0)
			{
				var statusStyle = "";
				switch(request.StatusId)
				{
					case (int)Enums.RequestEnums.RequestStatus.Open:
						statusStyle = "Color:green";
						break;
					case (int)Enums.RequestEnums.RequestStatus.InProgress:
						statusStyle = "Color:orange";
						break;
					case (int)Enums.RequestEnums.RequestStatus.Closed:
						statusStyle = "Color:red";
						break;
					default:
						break;
				}
				var title = $"Closure of Your Incident Report [#{requestId}]";
				var content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
				+ $"<span style='font-size:1.15em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>"
				+ $"<br/><span style='font-size:1em;'><strong>We are pleased to inform you that your incident report has been successfully resolved, and the ticket is now closed.</strong></span><br/>"
				+ $"<br/><span style='font-size:1em;'><strong>The details are as follows:</strong></span><br/>"
				+ $"<br/><ul>"
				+ $"<li>Ticket number: [#{request.Id}]</li>"
				+ $"<li>Current status: <strong style='{statusStyle}'>{request.Status}</strong></li>"
				+ $"<li>Description of the problem: <a href='https://designsync-pro.psz-soft.com/#/request/{requestId}'>Here</a></li>"
				+ $"</ul>"
				+ $"<br/><br/>"
				+ "</div>";
				content += $"<hr>";
				content += $"<br/><span style='font-size:1.em;font-weight:bold'>Regards,</span>";
				content += $"<br/><span style='font-size:1.em;font-weight:bold'>IT Department </span></br>";
				try
				{
					Module.EmailingService.SendEmailAsync(title, content, adresses.DistinctBy(x => x).ToList(), null);
				} catch(Exception ex)
				{
					Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email NotifyExpiredNearlyRahmens"));
					Infrastructure.Services.Logging.Logger.Log(ex);
				}
			}
		}
		public static void SendRequestUpdateMail(int requestId, List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity>? closed = null, bool inEng = false)
		{
			var request = Infrastructure.Data.Access.Tables.CPL.Capital_requests_headerAccess.Get(requestId);
			var teams = Infrastructure.Data.Access.Tables.CPL.Capital_requests_teamsAccess.Get();
			var EngeneeringTeam = teams?.Where(x => x.Team == "Eng" && x.PlantId == request.PlantId).ToList();
			var ids = EngeneeringTeam?.Select(x => x.UserId ?? -1).ToList();
			var users = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(ids);
			var adresses = users?.Select(u => u.Email).ToList();
			var requestUser = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(request.UserId ?? -1);
			adresses.Add(requestUser?.Email);
			if(adresses is not null && adresses.Count > 0)
			{
				var statusStyle = "";
				switch(request.StatusId)
				{
					case (int)Enums.RequestEnums.RequestStatus.Open:
						statusStyle = "Color:green";
						break;
					case (int)Enums.RequestEnums.RequestStatus.InProgress:
						statusStyle = "Color:orange";
						break;
					case (int)Enums.RequestEnums.RequestStatus.Closed:
						statusStyle = "Color:red";
						break;
					default:
						break;
				}
				var title = $"Ticket Update";
				var content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
				+ $"<span style='font-size:1.15em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>"
				+ $"<br/><span style='font-size:1em;'><strong>We would like to inform you about the current status of your incident report.</strong></span><br/>"
				+ $"<br/><ul>"
				+ $"<li>Ticket number: [#{request.Id}]</li>"
				+ $"{(closed != null && closed.Count > 0 ? $"<li>Position(s) Num° [{string.Join(",", closed.Select(x => x.PositionId))}] changed in {(inEng ? "Engeneering" : "Capial")}</li>" : "")}"
				+ $"<li>Current status: <strong style='{statusStyle}'>{request.Status}</strong></li>"
				+ $"<li>Description of the problem: <a href='https://designsync-pro.psz-soft.com/#/request/{requestId}'>Here</a></li>"
				+ $"</ul>"
				+ $"<br/><br/>"
				+ "</div>";
				content += $"<hr>";
				content += $"<br/><span style='font-size:1.em;font-weight:bold'>Regards,</span>";
				content += $"<br/><span style='font-size:1.em;font-weight:bold'>IT Department </span></br>";
				try
				{
					Module.EmailingService.SendEmailAsync(title, content, adresses.DistinctBy(x => x).ToList(), null);
				} catch(Exception ex)
				{
					Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email NotifyExpiredNearlyRahmens"));
					Infrastructure.Services.Logging.Logger.Log(ex);
				}
			}
		}
	}
}
