using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.WorkPlan.Models.StandardOperationDescription
{
	public class CreateModel
	{
		public int Id { get; set; }
		public int StandardOperationId { get; set; }
		public string Description { get; set; }
		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public DateTime? ArchiveTime { get; set; }
		public int? ArchiveUserId { get; set; }
		public Boolean IsArchived { get; set; }

		public string NameDE { get; set; }
		public string NameTN { get; set; }
		public string NameCZ { get; set; }
		public string NameAL { get; set; }
		public string CreationUsername { get; set; }
		public string LastEditUsername { get; set; }
		public CreateModel()
		{

		}

		public CreateModel(Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity stdOpdb,
			List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionI18NEntity> standardOperationI18NEntity)
		{
			this.Id = stdOpdb.Id;
			this.StandardOperationId = stdOpdb.StandardOperationId;
			this.Description = stdOpdb.Description;
			this.CreationTime = stdOpdb.CreationTime;
			this.CreationUsername = Helpers.User.GetUserNameById(stdOpdb.CreationUserId);
			this.CreationUserId = stdOpdb.CreationUserId;
			this.LastEditTime = stdOpdb.LastEditTime.HasValue ? stdOpdb.LastEditTime.Value : (DateTime?)null;
			this.LastEditUsername = stdOpdb.LastEditUserId.HasValue ? Helpers.User.GetUserNameById(stdOpdb.LastEditUserId.Value) : "";
			// this.CanSafeDelete = Psz.Core.Apps.WorkPlan.Helpers.DeleteCheck.CanSafeDeleteStandardOperation(stdOpdb.Id);

			this.NameDE = standardOperationI18NEntity?.Find(s => s.CodeLanguage.ToUpper() == "DE")?.Name;
			this.NameTN = standardOperationI18NEntity?.Find(s => s.CodeLanguage.ToUpper() == "TN")?.Name;
			this.NameCZ = standardOperationI18NEntity?.Find(s => s.CodeLanguage.ToUpper() == "CZ")?.Name;
			this.NameAL = standardOperationI18NEntity?.Find(s => s.CodeLanguage.ToUpper() == "AL")?.Name;
		}
	}
}
