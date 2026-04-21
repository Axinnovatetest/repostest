using Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Email.Models
{
	public class NotifyUserByEmailModel
	{
		public DateTime? AddedOn { get; set; }
		public string link { get; set; }
		public int UserId { get; set; }
		public string EmailTo { get; set; }
		public string Subject { get; set; }
		public NotifyUserByEmailModel() { }
		public NotifyUserByEmailModel(PSZ_SendGrid_Email_Not_DeliveredMinimalEntity2 data)
		{
			AddedOn = data.AddedOn;
			link = Infrastructure.Services.Helpers.SendGridHelper.GetLinkFromString(data.EmailContent);
			EmailTo = data.EmailTo;
			Subject = data.Subject;
			UserId = (int)data.UserId;
		}
	}
}
