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
		public string Code { get; set; }
		public string LotPiece { get; set; }
		public string MachineToolInsert { get; set; }
		public string ManuelMachinel { get; set; }
		public bool Operation_Value_Adding { get; set; }
		public string ReationSetup { get; set; }
		public string RelationOperationSetup { get; set; }
		public string Remark { get; set; }
		public string Remark2 { get; set; }
		public string SecondsPerSubOperation { get; set; }
		public string Setup { get; set; }
		public int StdOperationId { get; set; }
		public string TechnologieArea { get; set; }
		public string ValueAdding { get; set; }
		public StdOperationDescriptionViewModel() { }
		public StdOperationDescriptionViewModel(Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity stdOpDescDb,
			List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionI18NEntity> standardOperationDescriptionI18NEntity)
		{
			this.Code = stdOpDescDb.Code;
			this.Id = stdOpDescDb.Id;
			this.StandardOperationId = stdOpDescDb.StdOperationId;
			this.Description = stdOpDescDb.Description;
			this.CreationTime = stdOpDescDb.Creation_Date;
			LotPiece = stdOpDescDb?.LotPiece;
			MachineToolInsert = stdOpDescDb?.MachineToolInsert;
			ManuelMachinel = stdOpDescDb?.ManuelMachinel;
			Operation_Value_Adding = stdOpDescDb?.Operation_Value_Adding ?? false;
			ReationSetup = stdOpDescDb?.ReationSetup;
			RelationOperationSetup = stdOpDescDb?.RelationOperationSetup;
			Remark = stdOpDescDb?.Remark;
			Remark2 = stdOpDescDb?.Remark2;
			SecondsPerSubOperation = stdOpDescDb?.SecondsPerSubOperation;
			Setup = stdOpDescDb?.Setup;
			StdOperationId = stdOpDescDb?.StdOperationId ?? 0;
			TechnologieArea = stdOpDescDb?.TechnologieArea;
			ValueAdding = stdOpDescDb?.ValueAdding;
			this.CreationUsername = Helpers.User.GetUserNameById(stdOpDescDb.Creation_User_Id);
			this.LastEditTime = stdOpDescDb.Last_Edit_Date.HasValue ? stdOpDescDb.Last_Edit_Date.Value : (DateTime?)null;
			this.LastEditUsername = stdOpDescDb.Last_Edit_User_Id.HasValue ? Helpers.User.GetUserNameById(stdOpDescDb.Last_Edit_User_Id.Value) : "";

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

		//
		public StdOperationDescriptionViewModel(Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionEntity stdOpDescDb,
			List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationDescriptionI18NEntity> standardOperationDescriptionI18NEntity, string cretateUserName, string editUserName)
		{
			this.Id = stdOpDescDb.Id;
			this.StandardOperationId = stdOpDescDb.StdOperationId;
			this.Description = stdOpDescDb.Description;
			this.CreationTime = stdOpDescDb.Creation_Date;
			this.CreationUsername = cretateUserName;//Helpers.User.GetUserNameById(stdOpDescDb.CreationUserId);
			this.LastEditTime = stdOpDescDb.Last_Edit_Date.HasValue ? stdOpDescDb.Last_Edit_Date.Value : (DateTime?)null;
			this.LastEditUsername = editUserName;//stdOpDescDb.LastEditUserId.HasValue ? Helpers.User.GetUserNameById(stdOpDescDb.LastEditUserId.Value) : "";

			//var articles = Helpers.DeleteCheck.CanSafeDeleteOperationDescription(stdOpDescDb.Id);
			//this.CanSafeDelete = articles == null || articles.Count == 0;
			//this.Articles = articles; //this.CanSafeDelete ? "" : string.Join(", ", articles);

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
