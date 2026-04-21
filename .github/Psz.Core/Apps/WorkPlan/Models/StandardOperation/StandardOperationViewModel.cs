using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.WorkPlan.Models.StandardOperation
{
	public class StdOperationDescriptionViewModel
	{
		public int Id { get; set; }
		public int StandardOperationId { get; set; }
		public string Description { get; set; }
		public string CreationUsername { get; set; }
		public string LastEditUsername { get; set; }
		public DateTime CreationTime { get; set; }
		public DateTime? LastEditTime { get; set; }
		public bool CanSafeDelete { get; set; }
		public List<string> Articles { get; set; }
		public string NameDE { get; set; }
		public string NameTN { get; set; }
		public string NameCZ { get; set; }
		public string NameAL { get; set; }

		public StdOperationDescriptionViewModel() { }
		public StdOperationDescriptionViewModel(Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity stdOpDescDb,
			List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionI18NEntity> standardOperationDescriptionI18NEntity)
		{
			this.Id = stdOpDescDb.Id;
			this.StandardOperationId = stdOpDescDb.StandardOperationId;
			this.Description = stdOpDescDb.Description;
			this.CreationTime = stdOpDescDb.CreationTime;
			this.CreationUsername = Helpers.User.GetUserNameById(stdOpDescDb.CreationUserId);
			this.LastEditTime = stdOpDescDb.LastEditTime.HasValue ? stdOpDescDb.LastEditTime.Value : (DateTime?)null;
			this.LastEditUsername = stdOpDescDb.LastEditUserId.HasValue ? Helpers.User.GetUserNameById(stdOpDescDb.LastEditUserId.Value) : "";

			var articles = Helpers.DeleteCheck.CanSafeDeleteOperationDescription(stdOpDescDb.Id);
			this.CanSafeDelete = articles == null || articles.Count == 0;
			this.Articles = articles; //this.CanSafeDelete ? "" : string.Join(", ", articles);

			var standardOperationDescriptionI18NEntityCurrent = standardOperationDescriptionI18NEntity?.FindAll(s => s.IdStandardOperationDescription == stdOpDescDb.Id);
			if(standardOperationDescriptionI18NEntityCurrent != null && standardOperationDescriptionI18NEntityCurrent.Count > 0)
			{
				this.NameDE = standardOperationDescriptionI18NEntityCurrent.Find(s => s.CodeLanguage.ToUpper() == "DE")?.Name;
				this.NameTN = standardOperationDescriptionI18NEntityCurrent.Find(s => s.CodeLanguage.ToUpper() == "TN")?.Name;
				this.NameCZ = standardOperationDescriptionI18NEntityCurrent.Find(s => s.CodeLanguage.ToUpper() == "CZ")?.Name;
				this.NameAL = standardOperationDescriptionI18NEntityCurrent.Find(s => s.CodeLanguage.ToUpper() == "AL")?.Name;
			}
		}
	}
}
