using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Models.OrderValidation
{
	public class GetPlacementHistoryResponseModel
	{
		public List<int> AttachmentIds { get; set; }
		public List<FileAttached> AttacedFiles { get; set; }
		public List<string> CCEmails { get; set; }
		public string EmailMessage { get; set; }
		public string EmailTitle { get; set; }
		public int Id { get; set; }
		public bool? SenderCC { get; set; }
		public string SenderUserEmail { get; set; }
		public int? SenderUserId { get; set; }
		public string SenderUserName { get; set; }
		public DateTime? SendingTime { get; set; }
		public string ToEmail { get; set; }
		public GetPlacementHistoryResponseModel(Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity orderPlacement)
		{
			if(orderPlacement == null)
			{
				return;
			}


			EmailMessage = orderPlacement.EmailMessage;
			EmailTitle = orderPlacement.EmailTitle;
			Id = orderPlacement.Id;
			SenderCC = orderPlacement.SenderCC;
			SenderUserEmail = orderPlacement.SenderUserEmail;
			SenderUserId = orderPlacement.SenderUserId;
			SenderUserName = orderPlacement.SenderUserName;
			SendingTime = orderPlacement.SendingTime;
			ToEmail = orderPlacement.ToEmail;
			AttachmentIds = orderPlacement.AttachmentIds?.Split('|')?.Select(x => int.TryParse(x, out var _v) ? _v : -1)?.ToList();
			CCEmails = orderPlacement.CCEmails?.Split(';')?.ToList();
			if(AttachmentIds is not null)
			{
				var files = Infrastructure.Data.Access.Tables.FileAccess.Get(AttachmentIds);
				AttacedFiles = files.Select(x => new FileAttached(x.Id, x.Name, x.Extension)).ToList();
			}
		}

	}

	public class FileAttached
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public string Extension { get; set; }
		public FileAttached(int id, string name, string extension)
		{
			Id = id;
			Name = name;
			Extension = extension;
		}
	}
}
