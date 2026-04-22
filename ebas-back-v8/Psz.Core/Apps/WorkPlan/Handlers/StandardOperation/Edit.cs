using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.WorkPlan.Handlers
{
	public partial class StandardOperation
	{
		public static Core.Models.ResponseModel<int> Edit(Psz.Core.Apps.WorkPlan.Models.StandardOperation.CreateModel editModel,
			Core.Identity.Models.UserModel user)
		{
			try
			{
				var standardOperationDb = Infrastructure.Data.Access.Tables.WPL.StandardOperationAccess.Get(editModel.Id);
				if(standardOperationDb == null)
				{
					throw new Core.Exceptions.NotFoundException("Standard operation not found");
				}

				standardOperationDb.Code = editModel.Code;
				standardOperationDb.Name = editModel.Name;
				standardOperationDb.OperationValueAdding = editModel.OperationValueAdding;
				standardOperationDb.RelationOperationTime = editModel.RelationOperationTime;
				standardOperationDb.LastEditTime = DateTime.Now;
				standardOperationDb.LastEditUserId = user.Id;


				List<string> errors = new List<string>();
				// - same code
				if(Infrastructure.Data.Access.Tables.WPL.StandardOperationAccess.GetByCode(editModel.Id, editModel.Code) != null)
				{
					errors.Add($"Operation with code [{editModel.Code}] already exists");
				}
				// - same name
				if(Infrastructure.Data.Access.Tables.WPL.StandardOperationAccess.GetByName(editModel.Id, editModel.Name) != null)
				{
					errors.Add($"Operation [{editModel.Name}] already exists");
				}
				if(errors.Count > 0)
				{
					return Core.Models.ResponseModel<int>.FailureResponse(errors);
				}

				var nbUpdated = Infrastructure.Data.Access.Tables.WPL.StandardOperationAccess.Update(standardOperationDb);
				if(nbUpdated > 0)
				{
					var languages = Psz.Core.Apps.WorkPlan.Handlers.Helpers.addMissingLanguages(user.Id) ?? new List<Infrastructure.Data.Entities.Tables.STG.LanguageEntity>();

					addNotExisting(editModel, standardOperationDb, languages, Infrastructure.Data.Access.Tables.WPL.StandardOperationI18NAccess.GetByStandardOperation(standardOperationDb.Id));

					// DE Name
					var langDE = languages?.Find(l => l.Code.ToUpper() == "DE");
					if(langDE != null)
					{
						Infrastructure.Data.Access.Tables.WPL.StandardOperationI18NAccess.UpdateByStandardOperation(
							new Infrastructure.Data.Entities.Tables.WPL.StandardOperationI18NEntity
							{
								Description = string.Empty,
								IdStandardOperation = standardOperationDb.Id,
								IdLanguage = (int)langDE?.Id,
								Name = editModel.NameDE
							});
					}

					// TN Name
					var langTN = languages?.Find(l => l.Code.ToUpper() == "TN");
					if(langTN != null)
					{
						Infrastructure.Data.Access.Tables.WPL.StandardOperationI18NAccess.UpdateByStandardOperation(
							new Infrastructure.Data.Entities.Tables.WPL.StandardOperationI18NEntity
							{
								Description = string.Empty,
								IdStandardOperation = standardOperationDb.Id,
								IdLanguage = (int)langTN?.Id,
								Name = editModel.NameTN
							});
					}

					// CZ Name
					var langCZ = languages?.Find(l => l.Code.ToUpper() == "CZ");
					if(langCZ != null)
					{
						Infrastructure.Data.Access.Tables.WPL.StandardOperationI18NAccess.UpdateByStandardOperation(
							new Infrastructure.Data.Entities.Tables.WPL.StandardOperationI18NEntity
							{
								Description = string.Empty,
								IdStandardOperation = standardOperationDb.Id,
								IdLanguage = (int)langCZ?.Id,
								Name = editModel.NameCZ
							});
					}

					// AL Name
					var langAL = languages?.Find(l => l.Code.ToUpper() == "AL");
					if(langAL != null)
					{
						Infrastructure.Data.Access.Tables.WPL.StandardOperationI18NAccess.UpdateByStandardOperation(
							new Infrastructure.Data.Entities.Tables.WPL.StandardOperationI18NEntity
							{
								Description = string.Empty,
								IdStandardOperation = standardOperationDb.Id,
								IdLanguage = (int)langAL?.Id,
								Name = editModel.NameAL
							});
					}
				}

				return Core.Models.ResponseModel<int>.SuccessResponse(nbUpdated);
			} catch(System.Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		private static void addNotExisting(Models.StandardOperation.CreateModel editModel,
			Infrastructure.Data.Entities.Tables.WPL.StandardOperationEntity standardOperationDb,
			List<Infrastructure.Data.Entities.Tables.STG.LanguageEntity> languages,
			List<Infrastructure.Data.Entities.Tables.WPL.StandardOperationI18NEntity> i18n)
		{
			if(i18n.FindIndex(x => x.CodeLanguage.ToUpper() == "DE") < 0)
			{
				var lang = languages.Find(x => x.Code.ToUpper() == "DE");
				Infrastructure.Data.Access.Tables.WPL.StandardOperationI18NAccess.Insert(
					new Infrastructure.Data.Entities.Tables.WPL.StandardOperationI18NEntity
					{
						CodeLanguage = "DE",
						Description = string.Empty,
						IdStandardOperation = standardOperationDb.Id,
						IdLanguage = lang.Id,
						Name = editModel.NameDE
					});
			}

			// TN
			if(i18n.FindIndex(x => x.CodeLanguage.ToUpper() == "TN") < 0)
			{
				var lang = languages.Find(x => x.Code.ToUpper() == "TN");
				Infrastructure.Data.Access.Tables.WPL.StandardOperationI18NAccess.Insert(
					new Infrastructure.Data.Entities.Tables.WPL.StandardOperationI18NEntity
					{
						CodeLanguage = "TN",
						Description = string.Empty,
						IdStandardOperation = standardOperationDb.Id,
						IdLanguage = lang.Id,
						Name = editModel.NameTN
					});
			}

			// CZ
			if(i18n.FindIndex(x => x.CodeLanguage.ToUpper() == "CZ") < 0)
			{
				var lang = languages.Find(x => x.Code.ToUpper() == "CZ");
				Infrastructure.Data.Access.Tables.WPL.StandardOperationI18NAccess.Insert(
					new Infrastructure.Data.Entities.Tables.WPL.StandardOperationI18NEntity
					{
						CodeLanguage = "CZ",
						Description = string.Empty,
						IdStandardOperation = standardOperationDb.Id,
						IdLanguage = lang.Id,
						Name = editModel.NameCZ
					});
			}

			// AL
			if(i18n.FindIndex(x => x.CodeLanguage.ToUpper() == "AL") < 0)
			{
				var lang = languages.Find(x => x.Code.ToUpper() == "AL");
				Infrastructure.Data.Access.Tables.WPL.StandardOperationI18NAccess.Insert(
					new Infrastructure.Data.Entities.Tables.WPL.StandardOperationI18NEntity
					{
						CodeLanguage = "AL",
						Description = string.Empty,
						IdStandardOperation = standardOperationDb.Id,
						IdLanguage = lang.Id,
						Name = editModel.NameAL
					});
			}
		}
	}
}
