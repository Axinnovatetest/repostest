using System;
using System.Collections.Generic;

namespace Infrastructure.Services.Email
{
	public class EmailModel
	{
		public List<string> To { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public Enums.MailPriority Priority { get; set; }

		public Enums.Types Type { get; set; } = Enums.Types.General;

		private DateTime _creationTime { get; set; } = DateTime.Now;
		public DateTime CreationTime { get { return this._creationTime; } }
	}
	//public class EmailParamtersModel
	//{
	//    public string Host { get; set; }
	//    public int Port { get; set; }
	//    public string Username { get; set; }
	//    public string Password { get; set; }
	//    public bool SslEnabled { get; set; }
	//    public string Address { get; set; }
	//    public string DisplayName { get; set; }
	//}
}
