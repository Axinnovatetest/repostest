namespace Psz.Core.MaterialManagement.CRP.Helpers
{
	public static class Email
	{
		public static void Send(Identity.Models.UserModel sendingUser, string title, string htmlContent, List<string> toAddresses, List<string> ccAddresses = null, List<KeyValuePair<string, System.IO.Stream>> attachments = null)
		{
			title = $"[MTM] {title}";
			var content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
				+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>"
				+ $"{htmlContent}"
				+ "<br/><br/>Regards, <br/>IT Department </div>";

			Module.EmailingService.SendEmailAsync(title, content, toAddresses, attachments, ccAddresses);
		}
	}
}
