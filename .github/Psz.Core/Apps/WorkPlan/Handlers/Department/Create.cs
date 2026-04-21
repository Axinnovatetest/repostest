using System;

namespace Psz.Core.Apps.WorkPlan.Handlers
{
	public partial class Department
	{
		public static int Create(Apps.WorkPlan.Models.Department.CreateModel departementModel,
			Core.Identity.Models.UserModel user)
		{
			try
			{
				var departementDb = new Infrastructure.Data.Entities.Tables.WPL.DepartmentEntity
				{
					CreationTime = DateTime.Now,
					CreationUserId = user.Id,
					IsArchived = false,
					Name = departementModel.Name,
				};

				int insertedId = Infrastructure.Data.Access.Tables.WPL.DepartmentAccess.Insert(departementDb);
				if(insertedId > 0)
				{
					var languages = Infrastructure.Data.Access.Tables.STG.LanguageAccess.Get();

					// DE Name
					var langDE = languages?.Find(l => l.Code.ToUpper() == "DE");
					Infrastructure.Data.Access.Tables.WPL.DepartmentI18NAccess.Insert(new Infrastructure.Data.Entities.Tables.WPL.DepartmentI18NEntity
					{
						CodeLanguage = langDE?.Code,
						Description = "",
						IdDepartment = insertedId,
						IdLanguage = (int)langDE?.Id,
						Name = departementModel.NameDE
					});

					// TN Name
					var langTN = languages?.Find(l => l.Code.ToUpper() == "TN");
					Infrastructure.Data.Access.Tables.WPL.DepartmentI18NAccess.Insert(new Infrastructure.Data.Entities.Tables.WPL.DepartmentI18NEntity
					{
						CodeLanguage = langTN?.Code,
						Description = "",
						IdDepartment = insertedId,
						IdLanguage = (int)langTN?.Id,
						Name = departementModel.NameDE
					});

					// CZ Name
					var langCZ = languages?.Find(l => l.Code.ToUpper() == "CZ");
					Infrastructure.Data.Access.Tables.WPL.DepartmentI18NAccess.Insert(new Infrastructure.Data.Entities.Tables.WPL.DepartmentI18NEntity
					{
						CodeLanguage = langCZ?.Code,
						Description = "",
						IdDepartment = insertedId,
						IdLanguage = (int)langCZ?.Id,
						Name = departementModel.NameCZ
					});

					// AL Name
					var langAL = languages?.Find(l => l.Code.ToUpper() == "AL");
					Infrastructure.Data.Access.Tables.WPL.DepartmentI18NAccess.Insert(new Infrastructure.Data.Entities.Tables.WPL.DepartmentI18NEntity
					{
						CodeLanguage = langAL?.Code,
						Description = "",
						IdDepartment = insertedId,
						IdLanguage = (int)langAL?.Id,
						Name = departementModel.NameAL
					});
				}
				return insertedId;
			} catch(System.Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}
	}
}
