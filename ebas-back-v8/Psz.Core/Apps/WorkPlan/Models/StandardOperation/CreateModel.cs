using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Psz.Core.Apps.WorkPlan.Models.StandardOperation
{
	public class CreateModel
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public Boolean OperationValueAdding { get; set; }
		public string CreationUsername { get; set; }
		public string LastEditUsername { get; set; }
		public DateTime CreationTime { get; set; }
		public DateTime? LastEditTime { get; set; }
		public Boolean CanSafeDelete { get; set; }
		public double RelationOperationTime { get; set; }
		public string NameDE { get; set; }
		public string NameTN { get; set; }
		public string NameCZ { get; set; }
		public string NameAL { get; set; }
		public int CreationUserId { get; set; }
		public List<string> Articles { get; set; }
		public string Code { get; set; }
		public CreateModel() { }
		public CreateModel(Infrastructure.Data.Entities.Tables.WPL.StandardOperationEntity stdOpdb,
			List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationI18NEntity> standardOperationI18NEntity)
		{
			this.Code=stdOpdb.Code;
			this.Id = stdOpdb.Id;
			this.Name = stdOpdb.Name;
			this.OperationValueAdding = stdOpdb.OperationValueAdding;
			this.CreationTime = stdOpdb.CreationTime;
			this.CreationUsername = Helpers.User.GetUserNameById(stdOpdb.CreationUserId);
			this.CreationUserId = stdOpdb.CreationUserId;
			this.LastEditTime = stdOpdb.LastEditTime.HasValue ? stdOpdb.LastEditTime.Value : (DateTime?)null;
			this.LastEditUsername = stdOpdb.LastEditUserId.HasValue ? Helpers.User.GetUserNameById(stdOpdb.LastEditUserId.Value) : "";

			var articles = Psz.Core.Apps.WorkPlan.Helpers.DeleteCheck.CanSafeDeleteStandardOperation(stdOpdb.Id);
			this.CanSafeDelete = articles == null || articles.Count == 0;
			this.Articles = articles; // this.CanSafeDelete ? "" : string.Join(", ", articles);


			RelationOperationTime = stdOpdb.RelationOperationTime;
			this.NameDE = standardOperationI18NEntity?.Find(s => s.CodeLanguage.ToUpper() == "DE")?.Name;
			this.NameTN = standardOperationI18NEntity?.Find(s => s.CodeLanguage.ToUpper() == "TN")?.Name;
			this.NameCZ = standardOperationI18NEntity?.Find(s => s.CodeLanguage.ToUpper() == "CZ")?.Name;
			this.NameAL = standardOperationI18NEntity?.Find(s => s.CodeLanguage.ToUpper() == "AL")?.Name;
		}
	}
}
