using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.WorkPlan.Handlers
{
	public partial class Department
	{
		internal class Helpers
		{
			public static Infrastructure.Data.Entities.Tables.WPL.DepartmentEntity ToDataEntity(Psz.Core.Apps.WorkPlan.Models.Department.CreateModel model)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.DepartmentEntity
				{
					ArchiveTime = model.ArchiveTime,
					ArchiveUserId = model.ArchiveUserId,
					CreationTime = model.CreationTime,
					CreationUserId = model.CreationUserId,
					Id = model.Id,
					IsArchived = model.IsArchived,
					Name = model.Name,
					LastEditTime = model.LastEditTime,
					LastEditUserId = model.LastEditUserId,
				};
			}
		}
	}

	public class Helpers
	{
		public static List<Infrastructure.Data.Entities.Tables.STG.LanguageEntity> addMissingLanguages(int userId)
		{
			var languages = Infrastructure.Data.Access.Tables.STG.LanguageAccess.Get();

			// DE
			if(languages.FindIndex(x => x.Code.ToUpper() == "DE") < 0)
			{
				Infrastructure.Data.Access.Tables.STG.LanguageAccess.Insert(
				   new Infrastructure.Data.Entities.Tables.STG.LanguageEntity
				   {
					   Code = "DE",
					   Name = "Deutsch",
					   Description = "Deutsch language",
					   CreationDate = DateTime.Now,
					   CreationUserId = userId,
					   IsArchived = false
				   });
			}

			// TN
			if(languages.FindIndex(x => x.Code.ToUpper() == "TN") < 0)
			{
				Infrastructure.Data.Access.Tables.STG.LanguageAccess.Insert(
				   new Infrastructure.Data.Entities.Tables.STG.LanguageEntity
				   {
					   Code = "TN",
					   Name = "Tunisia",
					   Description = "Tunisia language",
					   CreationDate = DateTime.Now,
					   CreationUserId = userId,
					   IsArchived = false
				   });
			}

			// CZ
			if(languages.FindIndex(x => x.Code.ToUpper() == "CZ") < 0)
			{
				Infrastructure.Data.Access.Tables.STG.LanguageAccess.Insert(
				   new Infrastructure.Data.Entities.Tables.STG.LanguageEntity
				   {
					   Code = "CZ",
					   Name = "Czech",
					   Description = "Czech language",
					   CreationDate = DateTime.Now,
					   CreationUserId = userId,
					   IsArchived = false
				   });
			}

			// AL
			if(languages.FindIndex(x => x.Code.ToUpper() == "AL") < 0)
			{
				Infrastructure.Data.Access.Tables.STG.LanguageAccess.Insert(
				   new Infrastructure.Data.Entities.Tables.STG.LanguageEntity
				   {
					   Code = "AL",
					   Name = "Alabania",
					   Description = "Alabania language",
					   CreationDate = DateTime.Now,
					   CreationUserId = userId,
					   IsArchived = false
				   });
			}

			return Infrastructure.Data.Access.Tables.STG.LanguageAccess.Get();
		}
	}
}
