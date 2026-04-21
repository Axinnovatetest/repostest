using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Email
{
	public class GlobalModel
	{

		[Required]
		public string TemplateID { get; set; }
	}
	public class SendgridTemplateModel: GlobalModel
	{
		public string Subject { get; set; }
		public string greeting { get; set; }
		public string firstebody { get; set; }
		public string secondebody { get; set; }
		public string thirdebody { get; set; }
		public string foot { get; set; }
		public string url { get; set; }

	}
	public class SendgridTemplateRejectOrderModel: GlobalModel
	{
		public string Subject { get; set; }
		public string SendOn { get; set; }
		public string RejectedOn { get; set; }
		public string Greeting { get; set; }
		public string User { get; set; }
		public string Ordernumber { get; set; }
		public string fromproject { get; set; }
		public string rejectedbyuser { get; set; }
		public string notesIfExist { get; set; }
		public string domaineName { get; set; }
		public string notesPrefix { get; set; }
		public string notes { get; set; }

	}
}
