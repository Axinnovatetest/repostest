using System;

namespace Psz.Core.BaseData.Handlers
{
	public class ObjectLogHelper
	{
		public static Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity getLog(Identity.Models.UserModel user, int objectId,
			string column, string oldValue, string newValue, string tableName, Enums.ObjectLogEnums.LogType logType)
		{
			var _description = "";
			switch(logType)
			{
				case Enums.ObjectLogEnums.LogType.Add:
					_description = $"{logType.GetDescription()} [{column}] {{{newValue}}} to [{tableName}]";
					break;
				case Enums.ObjectLogEnums.LogType.Edit:
					_description = $"{logType.GetDescription()} [{column}] from {{{oldValue}}} to {{{newValue}}} on [{tableName}]";
					break;
				case Enums.ObjectLogEnums.LogType.Delete:
					_description = $"{logType.GetDescription()} [{column}] {{{oldValue}}} from [{tableName}]";
					break;
				case Enums.ObjectLogEnums.LogType.Archive:
					_description = $"{logType.GetDescription()} [{column}] {{{oldValue}}} from [{tableName}]";
					break;
				case Enums.ObjectLogEnums.LogType.BulkUpdate:
					_description = $"{logType.GetDescription()} [{column}] from {{{oldValue}}} to {{{newValue}}} on [{tableName}]";
					break;
				case Enums.ObjectLogEnums.LogType.AddFromCopy:
					_description = $"{logType.GetDescription()} [{column}] {{{newValue}}} to [{tableName}] from [{oldValue}]";
					break;
				default:
					break;
			}
			return new Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity
			{
				Id = -1, //
				LastUpdateUserId = user.Id,
				LastUpdateTime = DateTime.Now,
				LastUpdateUsername = user.Username,
				LastUpdateUserFullName = user.Name,
				LogObject = tableName,
				LogDescription = _description,
				LogObjectId = objectId
			};
		}
		public static Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity getBOMLog(Identity.Models.UserModel user, int? pos_id, int Alt, string article, string oldQty, string newQty,
			string oldArticle, string newArticle, Enums.ObjectLogEnums.BOMLogType logType)
		{
			var _description = "";
			string alternanative = (Alt == 1) ? "Alternative" : "";
			string originalArticle = (Alt == 1) ? $"for the Parent Article [{article}]" : "";
			switch(logType)
			{
				case Enums.ObjectLogEnums.BOMLogType.Add:
					_description = $"New {alternanative} Position Added with article [{newArticle}] and quantity [{newQty}] {originalArticle}";
					break;
				case Enums.ObjectLogEnums.BOMLogType.EditArt:
					_description = $"{alternanative} Position with article [{oldArticle}] updated --> article changed from [{oldArticle}] to [{newArticle}] {originalArticle}";
					break;
				case Enums.ObjectLogEnums.BOMLogType.EditQty:
					_description = $"{alternanative} Position with article [{oldArticle}] updated --> quantity changed from [{oldQty}] to [{newQty}] {originalArticle}";
					break;
				case Enums.ObjectLogEnums.BOMLogType.Delete:
					_description = $"{alternanative} Position with article [{newArticle}] and quantity [{newQty}] deleted {originalArticle}";
					break;
				case Enums.ObjectLogEnums.BOMLogType.StatusChange:
					_description = $"The Bom Status for the article [{article}] has changed from [{oldArticle}] to [{newArticle}] ";
					break;
				case Enums.ObjectLogEnums.BOMLogType.ImportExcel:
					_description = $"BOM list imported from Excel, number of positions [{oldQty}] ";
					break;
				case Enums.ObjectLogEnums.BOMLogType.Overwrite:
					_description = $"BOM positions Deleted (Overwritten) by Excel import, number of positions [{oldQty}] ";
					break;
				case Enums.ObjectLogEnums.BOMLogType.Version:
					_description = $"The Bom Version for the article [{article}] has changed from [{oldArticle}] to [{newArticle}] ";
					break;
				case Enums.ObjectLogEnums.BOMLogType.CPRequired:
					_description = $"The Bom CP Required for the article [{article}] has changed from [{oldArticle}] to [{newArticle}] ";
					break;
				case Enums.ObjectLogEnums.BOMLogType.ValidFrom:
					_description = $"The Bom Validat From for the article [{article}] has changed from [{oldArticle}] to [{newArticle}] ";
					break;
				default:
					break;
			}
			return new Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity
			{
				Aenderungsdatum = DateTime.Now,
				Alter_menge = oldQty,
				Bearbeiter = user.Name,
				FG_Artikelnummer = article,
				ID = -1,
				Neuer_menge = newQty,
				Status = _description,
				Stück_Artikelnummer_Aktuell = newArticle,
				Stück_Artikelnummer_Voränderung = oldArticle
			};
		}

		public static Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity getContactPersonLog(Identity.Models.UserModel user, int objectId,
			string column, string oldValue, string newValue, string tableName, Enums.ObjectLogEnums.LogType logType)
		{
			var _description = "";
			switch(logType)
			{
				case Enums.ObjectLogEnums.LogType.Add:
					_description = $"New Contact Person [{newValue}] Added for the [{column}] Number [{objectId}]";
					break;
				case Enums.ObjectLogEnums.LogType.Delete:
					_description = $"Contact Person [{oldValue}] deleted for the [{column}] Number [{objectId}]";
					break;
				default:
					break;
			}
			return new Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity
			{
				Id = -1, //
				LastUpdateUserId = user.Id,
				LastUpdateTime = DateTime.Now,
				LastUpdateUsername = user.Username,
				LastUpdateUserFullName = user.Name,
				LogObject = tableName,
				LogDescription = _description,
				LogObjectId = objectId
			};
		}
	}
}
