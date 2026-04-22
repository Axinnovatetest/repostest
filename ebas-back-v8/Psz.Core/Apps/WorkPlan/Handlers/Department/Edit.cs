using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.WorkPlan.Handlers
{
	public partial class Department
	{
		public static int Edit(Psz.Core.Apps.WorkPlan.Models.Department.CreateModel editModel,
			Core.Identity.Models.UserModel user)
		{
			try
			{
				var departementDb = Infrastructure.Data.Access.Tables.WPL.DepartmentAccess.Get(editModel.Id);
				if(departementDb == null)
				{
					throw new Core.Exceptions.NotFoundException("Department not found");
				}

				departementDb.LastEditTime = DateTime.Now;
				departementDb.LastEditUserId = user.Id;
				departementDb.Name = editModel.Name;

				var nbUpdated = Infrastructure.Data.Access.Tables.WPL.DepartmentAccess.Update(departementDb);
				if(nbUpdated > 0)
				{
					var languages = Psz.Core.Apps.WorkPlan.Handlers.Helpers.addMissingLanguages(user.Id) ?? new List<Infrastructure.Data.Entities.Tables.STG.LanguageEntity>();
					addNotExisting(editModel, departementDb, languages, Infrastructure.Data.Access.Tables.WPL.DepartmentI18NAccess.GetByDepartment(departementDb.Id));

					// DE Name
					var langDE = languages?.Find(l => l.Code.ToUpper() == "DE");
					if(langDE != null)
					{
						Infrastructure.Data.Access.Tables.WPL.DepartmentI18NAccess.UpdateByDepartmentWLanguage(new Infrastructure.Data.Entities.Tables.WPL.DepartmentI18NEntity
						{
							Description = string.Empty,
							IdDepartment = departementDb.Id,
							IdLanguage = (int)langDE?.Id,
							Name = editModel.NameDE,
						});
					}

					// TN Name
					var langTN = languages?.Find(l => l.Code.ToUpper() == "TN");
					if(langTN != null)
					{
						Infrastructure.Data.Access.Tables.WPL.DepartmentI18NAccess.UpdateByDepartmentWLanguage(new Infrastructure.Data.Entities.Tables.WPL.DepartmentI18NEntity
						{
							Description = string.Empty,
							IdDepartment = departementDb.Id,
							IdLanguage = (int)langTN?.Id,
							Name = editModel.NameTN
						});
					}

					// CZ Name
					var langCZ = languages?.Find(l => l.Code.ToUpper() == "CZ");
					if(langCZ != null)
					{
						Infrastructure.Data.Access.Tables.WPL.DepartmentI18NAccess.UpdateByDepartmentWLanguage(new Infrastructure.Data.Entities.Tables.WPL.DepartmentI18NEntity
						{
							Description = string.Empty,
							IdDepartment = departementDb.Id,
							IdLanguage = (int)langCZ?.Id,
							Name = editModel.NameCZ
						});
					}

					// AL Name
					var langAL = languages?.Find(l => l.Code.ToUpper() == "AL");
					if(langAL != null)
					{
						Infrastructure.Data.Access.Tables.WPL.DepartmentI18NAccess.UpdateByDepartmentWLanguage(new Infrastructure.Data.Entities.Tables.WPL.DepartmentI18NEntity
						{
							Description = string.Empty,
							IdDepartment = departementDb.Id,
							IdLanguage = (int)langAL?.Id,
							Name = editModel.NameAL
						});
					}
				}
				Infrastructure.Services.Logging.Logger.Log(editModel.Id + "+" + editModel.NameAL + " - " + editModel.NameDE + "+" + nbUpdated);
				return nbUpdated;
			} catch(System.Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		private static void addNotExisting(Models.Department.CreateModel editModel,
			Infrastructure.Data.Entities.Tables.WPL.DepartmentEntity standardOperationDb,
			List<Infrastructure.Data.Entities.Tables.STG.LanguageEntity> languages,
			List<Infrastructure.Data.Entities.Tables.WPL.DepartmentI18NEntity> i18n)
		{
			if(i18n.FindIndex(x => x.CodeLanguage.ToUpper() == "DE") < 0)
			{
				var lang = languages.Find(x => x.Code.ToUpper() == "DE");
				Infrastructure.Data.Access.Tables.WPL.DepartmentI18NAccess.Insert(
					new Infrastructure.Data.Entities.Tables.WPL.DepartmentI18NEntity
					{
						CodeLanguage = "DE",
						Description = string.Empty,
						IdDepartment = standardOperationDb.Id,
						IdLanguage = lang.Id,
						Name = editModel.NameDE
					});
			}

			// TN
			if(i18n.FindIndex(x => x.CodeLanguage.ToUpper() == "TN") < 0)
			{
				var lang = languages.Find(x => x.Code.ToUpper() == "TN");
				Infrastructure.Data.Access.Tables.WPL.DepartmentI18NAccess.Insert(
					new Infrastructure.Data.Entities.Tables.WPL.DepartmentI18NEntity
					{
						CodeLanguage = "TN",
						Description = string.Empty,
						IdDepartment = standardOperationDb.Id,
						IdLanguage = lang.Id,
						Name = editModel.NameTN
					});
			}

			// CZ
			if(i18n.FindIndex(x => x.CodeLanguage.ToUpper() == "CZ") < 0)
			{
				var lang = languages.Find(x => x.Code.ToUpper() == "CZ");
				Infrastructure.Data.Access.Tables.WPL.DepartmentI18NAccess.Insert(
					new Infrastructure.Data.Entities.Tables.WPL.DepartmentI18NEntity
					{
						CodeLanguage = "CZ",
						Description = string.Empty,
						IdDepartment = standardOperationDb.Id,
						IdLanguage = lang.Id,
						Name = editModel.NameCZ
					});
			}

			// AL
			if(i18n.FindIndex(x => x.CodeLanguage.ToUpper() == "AL") < 0)
			{
				var lang = languages.Find(x => x.Code.ToUpper() == "AL");
				Infrastructure.Data.Access.Tables.WPL.DepartmentI18NAccess.Insert(
					new Infrastructure.Data.Entities.Tables.WPL.DepartmentI18NEntity
					{
						CodeLanguage = "AL",
						Description = string.Empty,
						IdDepartment = standardOperationDb.Id,
						IdLanguage = lang.Id,
						Name = editModel.NameAL
					});
			}
		}
	}
}
