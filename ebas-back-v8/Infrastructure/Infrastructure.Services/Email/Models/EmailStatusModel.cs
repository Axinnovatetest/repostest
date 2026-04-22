using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Email.Models
{
	public class EmailStatusModel
	{
		public List<messages> messages { get; set; }
	}
	public class messages
	{
		public string from_email { get; set; }
		public string msg_id { get; set; }
		public string subject { get; set; }
		public string to_email { get; set; }
		public string status { get; set; }
		public string opens_count { get; set; }
		public string last_event_time { get; set; }



		private List<string> GetSendToEnumerable()
		{

			return null;
		}
	}
	public class messagesModel
	{
		public string from_email { get; set; }
		public string msg_id { get; set; }
		public string subject { get; set; }
		public string to_email { get; set; }
		public string status { get; set; }
		public string opens_count { get; set; }
		public string last_event_time { get; set; }
		public messagesModel(messages data)
		{
			from_email = data.from_email;
			msg_id = data.msg_id.Substring(0, data.msg_id.IndexOf(".")) ?? "";
			subject = data.subject;
			to_email = data.to_email;
			status = data.status;
			opens_count = data.opens_count;
			last_event_time = last_event_time;
		}
	}

}
